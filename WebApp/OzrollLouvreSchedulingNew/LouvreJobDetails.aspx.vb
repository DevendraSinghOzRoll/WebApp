Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports Sybiz
Imports OzrollPSLVSchedulingModel.SharedEnums
Imports OzrollPSLVSchedulingModel.SharedFunctions
Imports System.Linq
Imports AjaxControlToolkit
Imports OzrollPSLVSchedulingModel.Costing
Imports OzrollPSLVSchedulingModel.SharedConstants
Imports iTextSharp.text.pdf
Imports Microsoft.Reporting.WebForms

Partial Class LouvreJobDetails
    Inherits System.Web.UI.Page

    Private _Costing As CostingLouvres
    Private _LouvreCategoryID As Integer = 0
    Private _CustomerDiscount As Decimal = 0
    Private _RulesLouvreDetails As New RulesLouvreDetails
    Private _PowderCoaters As New List(Of PowderCoater)
    Private _AdditionalRequirementTypes As New List(Of AdditionalRequirementType)
    Private _Service As New AppService
    Private _Users As New List(Of User)
    Private _UserHasSalePriceOverridePermission As Boolean = False
    Private _UserHasCreditCheckOverridePermission As Boolean = False
    Private _CacheColours As New OzrollPSLVSchedulingModel.Cache.CacheColours
    Private _CacheExtras As New OzrollPSLVSchedulingModel.Cache.CacheExtraProductLouvres

    Private Const _PRICE_AUTO_CALCULATED_MSG As String = "Price has been auto calculated."
    Private Const _PRICE_OVERRIDDEN_MSG As String = "Price has been overridden."
    ''added by surendra ticket #75183 dt:01/06/2023 check user premission
    Dim cUserPermissions As UserPermissionsContainer
    Dim bolHasPermission As Boolean = False

    Private Sub LouvreJobDetails_PreRender(sender As Object, e As EventArgs) Handles Me.PreLoad

        RedirectIfInvalidUserSession(Session, Response)

        CacheGlobals()
        CheckPermissions()
        ''added by surendra dt:01/06/2023 ticket #75183 prevent extarnal customer login as internal.
        Dim service As AppService = New AppService()
        If Not IsNothing(Session("sessUserID")) Then
            cUserPermissions = service.Permissions.UserHasPermissions(CInt(Session("sessUserID")))
            bolHasPermission = cUserPermissions.HasPermission(Permissions.Admin) Or cUserPermissions.HasPermission(Permissions.TrackingPageAccess)
        End If
        service = Nothing

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim intSaveStatus As Integer = 0
        Dim strPDFToGenerate As String = String.Empty

        Page.Header.DataBind()
        'Added By Pradeep Singh for ticket #62561
        btnAddExtra.Visible = False
        'ModalPopupExtender.Show()

        '-----------------------------------------
        'If Request.QueryString("var1") = "PM" Then
        '    ModalPopupExtender.Show()
        'End If
        'ModalPopupExtenderP.Show()

        '----------------------------------

        If Not IsPostBack Then

            If Session("sessUserID") = String.Empty Then
                Response.Redirect("Logout.aspx", False)
                Exit Sub
            Else
                If Not IsNumeric(Session("sessUserID")) Then
                    Response.Redirect("Logout.aspx", False)
                    Exit Sub
                End If
            End If

            'don't allow access to this page if in as plantation
            If Session("sessProductTypeID").ToString = "1" Then
                Response.Redirect("Home.aspx", False)
                Exit Sub
            End If

            txtProductTypeID.Text = Session("sessProductTypeID").ToString

            Dim intScheduleID As Integer = SharedConstants.DEFAULT_INTEGER_VALUE
            Dim intViewType As ViewType = ViewType.Add

            ' The URL to return to when closing the schedule (if set)
            ViewState("ReturnURL") = 0

            If Not Request.QueryString.Count = 0 Then
                If Not IsNothing(Request.Params("ScheduleID")) Then
                    intScheduleID = CInt(Request.Params("ScheduleID"))
                End If
                If Not IsNothing(Request.Params("ViewType")) Then
                    intViewType = CInt(Request.Params("ViewType"))
                End If
                If Not IsNothing(Request.Params("Status")) Then
                    intSaveStatus = CInt(Request.Params("Status"))
                End If
                If Not IsNothing(Request.Params("doc")) Then
                    strPDFToGenerate = Request.Params("doc")
                End If
                If Not IsNothing(Request.Params("Return")) Then
                    ViewState("ReturnURL") = CInt(Request.Params("Return"))
                End If

            End If

            txtId.Text = intScheduleID
            txtIntScheduleID.Text = intScheduleID
            txtViewType.Text = intViewType.ToString

            lblStatus.Text = String.Empty

            _Service.AddWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)

            ' Configure costing in basic mode when not a post back as we dont have a customer to cost yet.
            _Costing = New CostingLouvres()

            LoadProductionSchedule()

            GeneratePDFDocument(strPDFToGenerate)

            If intSaveStatus = 1 Then
                lblStatus.ForeColor = Color.Green
                lblStatus.Text = "Details successfully saved."

                If strPDFToGenerate = "emailquote" Then
                    lblStatus.Text &= "<br />Quote sent to customer."
                ElseIf strPDFToGenerate = "optimisersheet" Then
                    lblStatus.Text = "<br />Optimiser sheet generated successfully."

                ElseIf strPDFToGenerate = "excelpowdercoat" Then
                    lblStatus.Text = "<br />Powder Coat sheet generated successfully."
                End If
            End If

            'Added by Michael Behar - 01-04-2021 - Ticket #64935
            If lblCurrentStatus.Text = "" Then
                lblCurrentStatus.Text = [Enum].GetName(GetType(ProductionScheduleStatus), ProductionScheduleStatus.Quote)
            End If

        Else
            If IsNumeric(cboCustomer.SelectedValue) Then
                Dim cCustomer As Customer = _Service.GetCustomerByID(cboCustomer.SelectedValue)

                InitCosting(cCustomer, txtEnteredDatetime.Text)
            End If

            lblStatus.Text = String.Empty
        End If

        SetFreightPriceOverrideControlVisibility()

        CheckSelectedCustomerIsLinkedToSybizCustomer()
        SetCustomerPricingCategoryUI()

        ConfigureStockDeductionsUI()

        ' Only display Ozroll Contract for old orders. It is no longer used in new orders.
        pnlOzrollContract.Visible = (txtOzrollContract.Text.Length > 0)

        ' Special case validation for details popups textchanged events. Validation would not fire when enter key pressed.
        If ViewState("DetailsTextChanged") IsNot Nothing Then
            If ViewState("DetailsTextChanged") Then

                Page.Validate("details")
                ViewState("DetailsTextChanged") = False
            End If
        End If


        ''condition added by surendra dt:01/06/2023 restrict extarnal customer login as intarnal user.
        If (Convert.ToInt32(Session("CustomerID")) > 0) Or (Convert.ToInt32(Session("CustomerId")) <= 0 AndAlso (bolHasPermission=False)) Then
            Dim customerId As Integer
            customerId = Convert.ToInt32(Session("CustomerID"))
            btnHome.Visible = False
            btnDashBoard.Visible = True
            pnlOnHold.Visible = False
            txtOrderDate.Enabled = False
            btnPlaceOrder.Visible = True
            If String.IsNullOrEmpty(txtOrderDate.Text) Then
                btnPlaceOrder.Enabled = True
            Else
                btnPlaceOrder.Enabled = False
            End If

            'If Not IsNothing(Request.Params("ScheduleID")) Then
            '        btnPlaceOrder.Enabled = False

            '    End If

            cboOrderType.SelectedIndex = cboOrderType.Items.IndexOf(cboOrderType.Items.FindByValue(1))
            cboOrderType.Enabled = False
            cboOrderType.ForeColor = System.Drawing.Color.Gray

            cboJobType.SelectedIndex = cboJobType.Items.IndexOf(cboJobType.Items.FindByValue(Convert.ToInt32(Session("sessProductTypeID"))))
            cboJobType.Enabled = False
            cboJobType.ForeColor = System.Drawing.Color.Gray
            cboCustomer.SelectedIndex = cboCustomer.Items.IndexOf(cboCustomer.Items.FindByValue(customerId))

            Dim send As Object
            send = ""
            Dim evnt As EventArgs
            If Not IsPostBack Then
                cboCustomer_SelectedIndexChanged(Nothing, Nothing)
            End If
            cboCustomer.Enabled = False
            cboCustomer.ForeColor = System.Drawing.Color.Gray

            txtCustomerName.Text = cboCustomer.SelectedItem.Text
            pnlCustomerName.Visible = False
            btnEmailQuote.Visible = False
            pnlHideShowButton.Visible = False
            pnlShowHideTextBox.Visible = False
            btnCancellation.Visible = False
            pnlRequirements.Visible = False
            pnlShowHideActualShippingDate.Visible = False
            pnlStockDeduction.Visible = False
            pnlsyBizInoviceNumber.Visible = False
            pnlSybizJobcode.Visible = False
            pnlPromisedExpectedShipingDate.Visible = False
            pnlImages.Visible = False
            If CInt(Request.Params("ScheduleID")) > 0 And Not String.IsNullOrEmpty(txtOrderDate.Text) Then
                btnViewNotes.Enabled = False
                btnAddDetails.Enabled = False
                btnAddPrivacyScreen.Enabled = False
                btnAddExtrasProdSchedule.Enabled = False
                txtContractNumber.Enabled = False
                radDelivery.Enabled = False
                radPickup.Enabled = False
                cboDeliveryAddress.Enabled = False
                pnlAddLouvers.Enabled = False
                pnlAddLouversDetails.Enabled = False
                btnAddExtra.Enabled = False
                btnSaveDetails.Enabled = False
                'added by surendra ticket#62230   date 12 Nov 2020
                btnGeneratesOPTIMISER.Visible = False
                'added by pradeep Singh 05-10-2021
                btExcelOPTIMISER.Visible = False
                'changed by surendra on 16/11/2020 ticket #63286
                btnFactoryPaperwork.Visible = False
            End If
            '    ''added by surendra dt:01/06/2023 restrict extarnal customer login as intarnal user.
            'ElseIf Convert.ToInt32(Session("CustomerId")) = 0 AndAlso cUserPermissions.HasPermission(Permissions.Admin) Then
            '    txtCustomerName.Visible = False
            '    cboJobType.SelectedIndex = cboJobType.Items.IndexOf(cboJobType.Items.FindByValue(2))
            '    cboJobType.Enabled = False
        End If
        hfUserType.Value = Convert.ToInt32(Session("CustomerID"))
		
			'Added By Michael Behar - 26-03-2021 - Ticket #64821 - To Dynamically Do Based On Database
			btnGenerateOrder.Text = "Generate PDF Quote"
			If txtOrderDate.Text <> "" Then
				 btnGenerateOrder.Text = "Generate PDF Order"
			End If
			
    End Sub

    Private Sub GeneratePDFDocument(strPDFToGenerate As String)
        If strPDFToGenerate = "order" Then

            lnkGenerateOrderPDFDummy.PostBackUrl = "LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=order"

            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "generatePDF", "generatePDF('order');", True)
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideHider", "HideHider();", True)

        ElseIf strPDFToGenerate = "deliverydocket" Then

            lnkGenerateDeliveryDocketPDFDummy.PostBackUrl = "LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=deliverydocket"

            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "generatePDF", "generatePDF('deliverydocket');", True)
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideHider", "HideHider();", True)

        ElseIf strPDFToGenerate = "runningsheet" Then

            lnkGenerateRunningSheetPDFDummy.PostBackUrl = "LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=runningsheet"

            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "generatePDF", "generatePDF('runningsheet');", True)
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideHider", "HideHider();", True)

        ElseIf strPDFToGenerate = "timesheet" Then

            lnkGenerateTimeSheetPDFDummy.PostBackUrl = "LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=timesheet"

            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "generatePDF", "generatePDF('timesheet');", True)
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideHider", "HideHider();", True)

        ElseIf strPDFToGenerate = "coversheet" Then

            lnkGenerateCoverSheetPDFDummy.PostBackUrl = "LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=coversheet"

            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "generatePDF", "generatePDF('coversheet');", True)
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideHider", "HideHider();", True)

        ElseIf strPDFToGenerate = "emailquote" Then

            lnkEmailQuotePDFDummy.PostBackUrl = "LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=emailquote"

            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "generatePDF", "generatePDF('emailquote');", True)
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideHider", "HideHider();", True)

        ElseIf strPDFToGenerate = "productionsheet" Then

            lnkGenerateCoverSheetPDFDummy.PostBackUrl = "LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=productionsheet"

            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "generatePDF", "generatePDF('productionsheet');", True)
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideHider", "HideHider();", True)
            'added by surendra 22-10-2020
        ElseIf strPDFToGenerate = "optimisersheet" Then

            lnkGenerateOptimiserSheetPDFDummy.PostBackUrl = "LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=optimisersheet"

            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "generatePDF", "generatePDF('optimisersheet');", True)
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideHider", "HideHider();", True)

        ElseIf strPDFToGenerate = "excelpowdercoat" Then
            lnkGenerateOptimiserSheetExcelDummy.PostBackUrl = "LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=excelpowdercoat"

            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "generatePDF", "generatePDF('excelpowdercoat');", True)
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideHider", "HideHider();", True)
        End If
    End Sub

    Private Sub LoadProductionSchedule()
        ' Used to hold the Notes for the production schedule.
        ViewState("Notes") = New List(Of ProdScheduleNote)

        ' Used to hold the Additional Requirements for the production schedule.
        ViewState("Requirements") = New List(Of AdditionalRequirements)

        ' Used to hold the Powder Coating for the production schedule.
        ViewState("PowderCoating") = New List(Of AdditionalRequirements)

        ' Used to indicate if we are currently adding a new louvre detail so it can be unwound.
        ViewState("NewLouvreDetail") = False

        ' Used to remember deleted louvre detail IDs.
        ViewState("DeletedDetailIDs") = New HashSet(Of Integer)

        ' Used to remember production schedule address zone selection.
        ViewState("AddressZoneID") = 0

        ' IDs for removed files
        ViewState("DeletedProdScheduleFileIDs") = New List(Of Integer)

        ' Stores existing and new files.
        Session("ProdScheduleFiles") = New List(Of ProductionScheduleFile)

        ' IDs for edited existing files
        ViewState("EditedProdScheduleFileIDs") = New List(Of Integer)

        Try
            lblPostProduction1.Text = "Despatch Date"
            lblPostProduction2.Text = "Invoice Date"

            InitCtrls()
            PopulateDetails(txtId.Text)

            CheckAndSetDropDownsEnabled()
            CheckAndSetClearButtonVisibilityForStatusDates()

            LoadProductionScheduleExtrasFromDB()
            LoadLouvreDetailsFromDB()

            LoadProductionScheduleExtrasIntoDataGrid()
            LoadShutterDetailsForDataGrid()

            LoadAdditionaRequirementsFromDB()
            LoadAdditionalRequirementsForDataGrid()

            OtherDetails()

            If txtId.Text <> String.Empty Then
                pnlCancellation.Visible = True
            End If

            CheckStatus()

            EnableDisableDetailsPopupUI()

        Catch ex As Exception
            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message & Environment.NewLine & GetPageInfo(form1, Session))
        End Try
    End Sub

    Private Sub CacheGlobals()

        _AdditionalRequirementTypes = _Service.GetAdditionalRequirementTypes().FindAll(Function(x) Not x.Discontinued)
        _PowderCoaters = _Service.GetPowderCoaters().FindAll(Function(x) Not x.Discontinued)
        _Users = _Service.GetUsers

    End Sub

    Private Sub InitCosting(cCustomer As Customer, dteCostingDate As Date)

        _LouvreCategoryID = 0
        _CustomerDiscount = 0

        If cCustomer IsNot Nothing AndAlso cCustomer.CustomerID > 0 Then
            If cCustomer.LouvreCategoryID > 0 Then
                _LouvreCategoryID = cCustomer.LouvreCategoryID
            End If

            _CustomerDiscount = cCustomer.Discount
        End If

        _Costing = New CostingLouvres(_LouvreCategoryID, dteCostingDate, _CustomerDiscount)
    End Sub

    Private Sub CheckAndSetDropDownsEnabled()
        Dim enumStatus As ProductionScheduleStatus = CalculateStatus()

        cboJobType.Enabled = ((enumStatus = ProductionScheduleStatus.Quote) OrElse
                             (enumStatus = ProductionScheduleStatus.AwaitingAcceptance))

        cboCustomer.Enabled = ((enumStatus = ProductionScheduleStatus.Quote) OrElse
                              (enumStatus = ProductionScheduleStatus.AwaitingAcceptance))

        RemoveControlsFormClasses(cboJobType)

        If cboJobType.Enabled Then
            cboJobType.CssClass &= " form-select"
        Else
            cboJobType.CssClass &= " form-select-disabled"
        End If

        RemoveControlsFormClasses(cboCustomer)

        If cboCustomer.Enabled Then
            cboCustomer.CssClass &= " form-select"
        Else
            cboCustomer.CssClass &= " form-select-disabled"
        End If

    End Sub

    Private Sub CheckStatus()
        If cboJobType.SelectedIndex > 0 Then
            Select Case CInt(Me.cboJobType.SelectedValue)
                Case 1 'domestic
                    Me.pnlCheckMeasureDate.Visible = True
                    Me.pnlInstallDate.Visible = True
                    Me.pnlRetailInstallDetails.Visible = True
                    CheckForCreatedLouvre()
                    If txtCheckMeasureDate.Text <> String.Empty And txtCheckMeasureDate.Enabled = False And txtPickingDate.Text = String.Empty Then
                        Me.btnClearCM.Visible = True
                        Me.btnClearReceived.Visible = False
                    ElseIf txtPickingDate.Text = String.Empty And txtReceived.Text <> String.Empty Then
                        Me.btnClearReceived.Visible = True
                        Me.btnClearCM.Visible = False
                        Me.btnClearPicking.Visible = False
                    ElseIf txtReceived.Text <> String.Empty And txtPickingDate.Text <> String.Empty And txtScheduledDate.Text = String.Empty Then
                        Me.btnClearReceived.Visible = False
                        Me.btnClearCM.Visible = False
                        Me.btnClearPicking.Visible = True
                    ElseIf txtReceived.Text <> String.Empty And txtPickingDate.Text <> String.Empty And txtScheduledDate.Text <> String.Empty Then
                        Me.btnClearReceived.Visible = False
                        Me.btnClearCM.Visible = False
                        Me.btnClearPicking.Visible = False
                    Else
                        If txtInstallDate.Text <> String.Empty And txtInstallDate.Enabled = False And txtPostProduction1.Text = String.Empty Then
                            Me.btnClearPostProduction1.Visible = True
                            Me.btnClearQC.Visible = False
                        ElseIf txtPostProduction1.Text = String.Empty And txtQCDate.Text <> String.Empty Then
                            Me.btnClearQC.Visible = True
                            Me.btnClearInstall.Visible = False
                            Me.btnClearPostProduction1.Visible = False
                        ElseIf btnClearQC.Text <> String.Empty And txtPostProduction1.Text <> String.Empty And txtPostProduction2.Text = String.Empty Then
                            Me.btnClearQC.Visible = False
                            Me.btnClearInstall.Visible = False
                            Me.btnClearPostProduction1.Visible = True
                        ElseIf btnClearQC.Text <> String.Empty And txtPostProduction1.Text <> String.Empty And txtPostProduction2.Text <> String.Empty Then
                            Me.btnClearQC.Visible = False
                            Me.btnClearInstall.Visible = False
                            Me.btnClearPostProduction1.Visible = False
                        Else
                            Me.btnClearReceived.Visible = False
                            Me.btnClearCM.Visible = False
                            Me.btnClearCM.Text = String.Empty
                            Me.btnClearPicking.Visible = False
                        End If
                    End If
                Case 2 'wholesale
                    Me.pnlCheckMeasureDate.Visible = False
                    Me.pnlInstallDate.Visible = False
                    Me.pnlRetailInstallDetails.Visible = False
                    CheckForCreatedLouvre()
                Case Else
                    Me.pnlCheckMeasureDate.Visible = False
                    Me.pnlInstallDate.Visible = False
                    Me.pnlRetailInstallDetails.Visible = False
                    CheckForCreatedLouvre()
            End Select
        End If
    End Sub

    Private Sub CheckForCreatedLouvre()
        'Dim _Service As New App_Service

        'Dim strSQL As String = "Select * from tblLouvreDetails where ProductionScheduleID = " & txtIntScheduleID.Text
        'Dim dtCreatedLouvre As DataTable = _Service.runSQLScheduling(strSQL)

        'If dtCreatedLouvre.Rows.Count > 0 Then
        '    Me.dgvDetails.DataSource = dtCreatedLouvre
        '    Me.dgvDetails.DataBind()
        'End If

    End Sub

    Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        PageError(Me, form1)
    End Sub

    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click

        ' Clear temp storage for uploaded files
        Session("ProdScheduleFiles") = Nothing

        Response.Redirect("Logout.aspx", False)

    End Sub

    Protected Sub InitCtrls()

        ' Only load customers that have a louvre category ID for costing.
        Dim cCustomers As List(Of Customer) = _Service.GetCustomers().FindAll(Function(x) (x.WholesaleLouvres OrElse x.RetailLouvres) _
                                                AndAlso x.LouvreCategoryID > 0 AndAlso Not x.Discontinued).OrderBy(Function(x) x.SortOrder).ThenBy(Function(x) x.CustomerName).ToList()

        cboCustomer.Items.Clear()
        cboCustomer.Items.Add(New ListItem(String.Empty, 0))
        'Add by surendra ticket #65452
        If Convert.ToInt32(Session("CustomerID")) > 0 Then
            For Each c As Customer In cCustomers.Where(Function(x) x.CustomerID = Convert.ToInt32(Session("CustomerID")))
                cboCustomer.Items.Add(New ListItem(c.CustomerName, c.CustomerID))
            Next c
            ''added by surendra dt:01/06/2023 check user peremission
            ''check internal customer permission if user is not admin by surendra dt:06/06/2023
        ElseIf bolHasPermission Then
            For Each c As Customer In cCustomers
                cboCustomer.Items.Add(New ListItem(c.CustomerName, c.CustomerID))
            Next c
        End If



        'setup order types list
        Dim dtOrderType As DataTable = createOrderTypeDatatable()
        ViewState("OrderType") = dtOrderType
        cboOrderType.DataSource = dtOrderType
        cboOrderType.DataValueField = "OrderTypeID"
        cboOrderType.DataTextField = "OrderType"
        cboOrderType.DataBind()
        cboOrderType.SelectedIndex = 0

        'setup priority list
        Dim dtPriority As DataTable = createPriorityDatatable()
        cboPriority.DataSource = dtPriority
        cboPriority.DataValueField = "PriorityID"
        cboPriority.DataTextField = "PriorityName"
        cboPriority.DataBind()
        cboPriority.SelectedIndex = 0

        Dim dtType As DataTable = _Service.RunSQLScheduling("select * from dbo.tblJobTypes where Discontinued=0 order by SortOrder ASC")
        Dim drow As DataRow = dtType.NewRow
        drow("JobTypeID") = 0
        drow("JobTypeName") = ""
        dtType.Rows.InsertAt(drow, 0)
        cboJobType.DataSource = dtType
        cboJobType.DataValueField = "JobTypeID"
        cboJobType.DataTextField = "JobTypeName"
        cboJobType.DataBind()
        cboJobType.SelectedIndex = 0


        Dim dtPowdercoater As DataTable = _Service.RunSQLScheduling("select * from dbo.tblPowdercoater where Discontinued=0 Order By SortOrder ASC")
        drow = dtPowdercoater.NewRow
        drow("PowdercoaterID") = 0
        drow("PowdercoaterName") = ""
        dtPowdercoater.Rows.InsertAt(drow, 0)
        cboPowdercoater.DataSource = dtPowdercoater
        cboPowdercoater.DataValueField = "PowdercoaterID"
        cboPowdercoater.DataTextField = "PowdercoaterName"
        cboPowdercoater.DataBind()
        cboPowdercoater.SelectedIndex = 0

        'hardcoded to exclude powdercoating for now
        Dim dtRequirementType As DataTable = _Service.RunSQLScheduling("select * from dbo.tblAdditionalRequirementTypes where AdditionalRequirementTypeID<>1 and Discontinued=0 and ProductTypeID = " & txtProductTypeID.Text & " Order By SortOrder ASC")
        drow = dtRequirementType.NewRow
        drow("AdditionalRequirementTypeID") = 0
        drow("RequirementTypeName") = ""
        dtRequirementType.Rows.InsertAt(drow, 0)
        cboRequirementType.DataSource = dtRequirementType
        cboRequirementType.DataValueField = "AdditionalRequirementTypeID"
        cboRequirementType.DataTextField = "RequirementTypeName"
        cboRequirementType.DataBind()
        cboRequirementType.SelectedIndex = 0

        PopulateShutterTypeDDL(0)

        pnlCreditHold.Visible = False
        btnGenerateProductionSheet.Visible = False
        btnGeneratesOPTIMISER.Visible = False
        'Added by Pradeep Singh on 05-10-2021
        btExcelOPTIMISER.Visible = False

    End Sub

    Protected Function createOrderTypeDatatable() As DataTable

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "OrderTypeID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "OrderType"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        Dim drow As DataRow = dt.NewRow
        drow("OrderTypeID") = 0
        drow("OrderType") = String.Empty
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("OrderTypeID") = 1
        drow("OrderType") = "Order"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("OrderTypeID") = 2
        drow("OrderType") = "Remake"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("OrderTypeID") = 3
        drow("OrderType") = "Rectification"
        dt.Rows.Add(drow)
        drow = Nothing

        'Added by Michael Behar - Ticket #66172 - 05/07/2021
        drow = dt.NewRow
        drow("OrderTypeID") = 4
        drow("OrderType") = "Samples"
        dt.Rows.Add(drow)
        drow = Nothing

        Return dt

    End Function

    Protected Function createPriorityDatatable() As DataTable

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "PriorityID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "PriorityName"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        Dim drow As DataRow = dt.NewRow
        drow("PriorityID") = 0
        drow("PriorityName") = String.Empty
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("PriorityID") = 1
        drow("PriorityName") = "High"
        dt.Rows.Add(drow)
        drow = Nothing


        Return dt

    End Function

    Private Sub OtherDetails()

        'txtTotalPrice.Enabled = False
        'txtShutterPrice.Enabled = False
        'txtExtraPrice.Enabled = False

        'Tool Tips

        imgColour.ToolTip = "Colour Of Louvre, Standard and Premium"
        imgHeight.ToolTip = "The Height Of The Opening Or Make Size"
        imgWidth.ToolTip = "The Width Of The Opening Or Make Size"
        imgMakeOpenSize.ToolTip = "Opening Or Make"
        imgLouvreProd.ToolTip = "Product Type, DLi or CL"
        imgNoOfPanels.ToolTip = "The Number Of Panels"
        imgBladeSize.ToolTip = "Blade Size Of Louvre"
        imgEndCapColour.ToolTip = "End Cap Colour For Louvre"
        imgBladeClipColour.ToolTip = "Blade Clip Colour For Louvre"
        imgPileColour.ToolTip = "Pile Colour (Grey Or Black)"
        imgMidRailHeight.ToolTip = "Transom Desired Height"
        imgFlushBoltsTop.ToolTip = "Top Flush Bolt For Louvre"
        imgFlushBoltsPosition.ToolTip = "Left or Right Stile Positioning of Flush Bolts"
        imgFlushBoltsBottom.ToolTip = "Bottom Flush Bolt For Louvre"
        imgLockOptions.ToolTip = "Lock Options For Louvre"
        imgBottomTrackType.ToolTip = "Desired Bottom Track DLi Or CL"
        imgBladeLocks.ToolTip = "Ability To Lock The Blades"
        imgCChannel.ToolTip = "Is Top and Bottom C Channel Fixing Required?"
        imgHChannel.ToolTip = "H Channel For Louvre"
        imgLReveal.ToolTip = "L Reveal For Louvre"
        imgZReveal.ToolTip = "Z Reveal For Louvre"
        imgBladeOperation.ToolTip = "Blade Operation For Louvre"
        imgWinder.ToolTip = "Manual Winder For Louvre"
        imgFlyScreen.ToolTip = "Optional Fly Screen For Louvre"
        imgBladeOperationBottom.ToolTip = "Blade Operation Bottom"
        imgInsertTop.ToolTip = "Top Insert For Louvre"
        imgCurvedTrack.ToolTip = "Optional Curved Track"
        imgExtraTrack.ToolTip = "Extra Tracks For Louvre"
        imgSlide.ToolTip = "Operable Blades will require a wider track spacing for blade clearance if required to slide past each other when blades are open."
        imgInsertBottom.ToolTip = "Bottom Insert Of Louvre"
        imgStacker.ToolTip = "Stacker Storage Bay Location"
        imgTopTrackType.ToolTip = "Top Track Type DLi Or CL"
        imgFixedPanelSides.ToolTip = "Fixed Sides For Panel"
        imgLouvreType.ToolTip = "Type Of Louvre BiFold, Stacker etc."
        imgSpecialRequirements.ToolTip = "Special Requirements"
        imgVerticalLeftInfo.ToolTip = "Louvre Open's Left"
        imgVerticalRightInfo.ToolTip = "Louvre Open's Right"
        imgBiFoldHingedDoor.ToolTip = "Louvre Open In Or Out"

        imgHinges.ToolTip = "Hinges Required for the Louvre Panel"
        imgPanelTopRail.ToolTip = "The Top Rail for the Louvre Panel"
        imgPanelBottomRail.ToolTip = "The Bottom Rail for the Louvre Panel"
        imgPanelMidRail.ToolTip = "The Mid Rail for the Louvre Panel"

    End Sub

    Protected Sub PopulateDetails(ByVal intProdScheduleId As Integer)

        Dim intViewType As ViewType = ViewType.NONE

        If Not Request.QueryString.Count = 0 Then
            If Not IsNothing(Request.Params("ViewType")) Then
                intViewType = CInt(Request.Params("ViewType"))
            End If
        End If

        txtViewType.Text = intViewType

        Dim clsProdSchedule As ProductionSchedule = New ProductionSchedule

        If intViewType = ViewType.Update Then
            clsProdSchedule = _Service.GetProdScheduleClsByID(intProdScheduleId)
        End If

        txtId.Text = clsProdSchedule.ID
        txtIntScheduleID.Text = clsProdSchedule.ID
        txtEnteredDatetime.Enabled = False
        ''added by surendra dt:07/06/2023 ticket #75293
        hfOrderStatus.Value = clsProdSchedule.OrderStatus
        EnableDisableDelivery(clsProdSchedule.DeliveryAddressID > 0)

        ViewState("AddressZoneID") = clsProdSchedule.AddressZoneID

        If clsProdSchedule.ID > 0 Then

            ' Get the prod schedule notes asnd load into UI.
            Dim lNotes As List(Of ProdScheduleNote) = _Service.GetProdScheduleNotesByProductionScheduleID(intProdScheduleId)

            ViewState("Notes") = lNotes
            LoadNotesToAccordian()

            ' Get the files attached to the production schedule
            Dim lFiles As List(Of ProductionScheduleFile) = _Service.GetProductionScheduleFilesByParameters(False, intProdScheduleId)

            Session("ProdScheduleFiles") = lFiles
            LoadFilesToProductionScheduleList()

            txtJobNumber.Text = clsProdSchedule.JobNumber
            txtContractNumber.Text = clsProdSchedule.OrderReference
            lblConNote.Text = clsProdSchedule.ShippingDetails

            lblOzrollID.Text = clsProdSchedule.ShutterProNumber

            If clsProdSchedule.EnteredDatetime <> SharedConstants.DEFAULT_DATE_VALUE Then
                txtEnteredDatetime.Text = Format(clsProdSchedule.EnteredDatetime, "d MMM yyyy")
            Else
                txtEnteredDatetime.Text = Format(Now, "d MMM yyyy")
            End If

            cboCustomer.SelectedValue = clsProdSchedule.CustomerID
            cboCustomer_SelectedIndexChanged(Nothing, Nothing)

            txtCustomerName.Text = clsProdSchedule.CustomerName

            txtCustomerId.Text = cboCustomer.SelectedValue

            UpdateFreightPriceUIFromSavedProdSchedule(clsProdSchedule)

            txtOzrollContract.Text = clsProdSchedule.OzrollContractNo

            lblSybizJobCode.Text = clsProdSchedule.SybizJobCode
            lblSybizSalesInvoiceNo.Text = clsProdSchedule.SybizSalesInvoiceNumber

            If clsProdSchedule.QuoteExpiryDateTime <> SharedConstants.MAX_DATE Then
                lblExpiryDateTime.Text = Format(clsProdSchedule.QuoteExpiryDateTime, "d MMM yyyy HH:mm")
            Else
                lblExpiryDateTime.Text = SharedConstants.STR_VALUE_NOT_FOUND
            End If

            If clsProdSchedule.OrderDate <> SharedConstants.DEFAULT_DATE_VALUE Then
                txtOrderDate.Text = Format(clsProdSchedule.OrderDate, "d MMM yyyy")
                txtOrderDate.Enabled = False
            End If

            If clsProdSchedule.SalePriceIsValid Then
                txtSalePrice.Text = FormatCurrency(clsProdSchedule.SalePrice)
            Else
                txtSalePrice.Text = FormatCurrency(0)
            End If

            If clsProdSchedule.TotalSQM <> SharedConstants.DEFAULT_DECIMAL_VALUE Then
                txtTotalSQM.Text = clsProdSchedule.TotalSQM
            End If

            If clsProdSchedule.TotalPanels <> SharedConstants.DEFAULT_INTEGER_VALUE Then
                txtTotalPanels.Text = clsProdSchedule.TotalPanels
            End If

            Dim dtLouvreSpecs As DataTable = _Service.GetLouvreSpecsByProdSchID(intProdScheduleId)

            If dtLouvreSpecs.Rows.Count > 0 Then

                If Not IsDBNull(dtLouvreSpecs.Rows(0)("LouvreJobTypeID")) Then
                    cboJobType.SelectedValue = dtLouvreSpecs.Rows(0)("LouvreJobTypeID")
                End If

                If Not IsDBNull(dtLouvreSpecs.Rows(0)("CheckMeasureDate")) Then
                    txtCheckMeasureDate.Text = Format(CDate(dtLouvreSpecs.Rows(0)("CheckMeasureDate")), "d MMM yyyy")
                    txtCheckMeasureDate.Enabled = False
                End If

                If Not IsDBNull(dtLouvreSpecs.Rows(0)("CheckMeasureID")) Then
                    cboCheckMeasure.SelectedValue = dtLouvreSpecs.Rows(0)("CheckMeasureID")
                End If

                If Not IsDBNull(dtLouvreSpecs.Rows(0)("InstallDate")) Then
                    txtInstallDate.Text = Format(CDate(dtLouvreSpecs.Rows(0)("InstallDate")), "d MMM yyyy")
                    txtInstallDate.Enabled = False
                End If

                If Not IsDBNull(dtLouvreSpecs.Rows(0)("InstallID")) Then
                    cboInstaller.SelectedValue = dtLouvreSpecs.Rows(0)("InstallID")
                End If

            End If

            dtLouvreSpecs.Dispose()
            dtLouvreSpecs = Nothing

            If clsProdSchedule.PromisedDate <> SharedConstants.DEFAULT_DATE_VALUE Then
                txtPromisedDate.Text = Format(clsProdSchedule.PromisedDate, "d MMM yyyy")
            End If

            If clsProdSchedule.ExpectedShippingDate <> SharedConstants.DEFAULT_DATE_VALUE Then
                txtPlannedShippingDate.Text = Format(clsProdSchedule.ExpectedShippingDate, "d MMM yyyy")
            End If

            If clsProdSchedule.ActualShippingDate <> SharedConstants.DEFAULT_DATE_VALUE Then
                txtActualShippingDate.Text = Format(clsProdSchedule.ActualShippingDate, "d MMM yyyy")
            End If

            If clsProdSchedule.OrderTypeID <> SharedConstants.DEFAULT_INTEGER_VALUE Then

                cboOrderType.SelectedValue = clsProdSchedule.OrderTypeID
                SetUIForOrderType(clsProdSchedule.OrderTypeID)
                ''Added by surendra Ticket #66605
                If Not String.IsNullOrEmpty(clsProdSchedule.SybizJobCode) Then
                    DisableOrderType(clsProdSchedule.OrderTypeID)
                End If

            Else
                cboOrderType.SelectedIndex = 0
                SetUIForOrderType(0)
            End If

            txtRemakeDescription.Text = clsProdSchedule.RemakeIssueDescription

            cboOrderType_SelectedIndexChanged(Me, Nothing)

            If clsProdSchedule.PriorityLevel <> SharedConstants.DEFAULT_INTEGER_VALUE Then
                cboPriority.SelectedValue = clsProdSchedule.PriorityLevel
            Else
                cboPriority.SelectedIndex = 0
            End If

            If clsProdSchedule.ScheduledDate <> SharedConstants.DEFAULT_DATE_VALUE Then
                txtScheduledDate.Text = Format(clsProdSchedule.ScheduledDate, "d MMM yyyy")
                txtScheduledDate.Enabled = False
            End If

            If clsProdSchedule.ReceivedDate <> SharedConstants.DEFAULT_DATE_VALUE Then
                txtReceived.Text = Format(clsProdSchedule.ReceivedDate, "d MMM yyyy")
                txtReceived.Enabled = False
            End If

            If clsProdSchedule.PickingDate <> SharedConstants.DEFAULT_DATE_VALUE Then
                txtPickingDate.Text = Format(clsProdSchedule.PickingDate, "d MMM yyyy")
                txtPickingDate.Enabled = False
            End If

            'fill job stages dates
            Dim dtJobStages As DataTable = _Service.RunSQLScheduling("select * from dbo.tblJobStages where ScheduleID=" & intProdScheduleId.ToString)

            If dtJobStages.Rows.Count > 0 Then
                For i As Integer = 0 To dtJobStages.Rows.Count - 1
                    If Not IsDBNull(dtJobStages.Rows(i)("CompletedDateTime")) Then
                        Select Case CInt(dtJobStages.Rows(i)("StageID").ToString)
                            Case 1
                                txtCuttingDate.Text = CDate(dtJobStages.Rows(i)("CompletedDateTime")).ToString("d MMM yyyy")
                                txtCuttingDate.Enabled = False
                            Case 2
                                txtPrepDate.Text = CDate(dtJobStages.Rows(i)("CompletedDateTime")).ToString("d MMM yyyy")
                                txtPrepDate.Enabled = False
                            Case 3
                                txtAssemblyDate.Text = CDate(dtJobStages.Rows(i)("CompletedDateTime")).ToString("d MMM yyyy")
                                txtAssemblyDate.Enabled = False
                            Case 4
                                'Me.txtFramingDate.Text = CDate(dtJobStages.Rows(i)("CompletedDateTime")).ToString("d MMM yyyy")
                                'Me.txtFramingDate.Enabled = False
                            Case 5
                                txtQCDate.Text = CDate(dtJobStages.Rows(i)("CompletedDateTime")).ToString("d MMM yyyy")
                                txtQCDate.Enabled = False
                            Case 6
                                'Me.txtWrappingDate.Text = CDate(dtJobStages.Rows(i)("CompletedDateTime")).ToString("d MMM yyyy")
                                'Me.txtWrappingDate.Enabled = False
                            Case 7
                                'despatch which is currently not used
                            Case 8
                                txtPiningDate.Text = CDate(dtJobStages.Rows(i)("CompletedDateTime")).ToString("d MMM yyyy")
                                txtPiningDate.Enabled = False
                            Case 9
                                txtHingingDate.Text = CDate(dtJobStages.Rows(i)("CompletedDateTime")).ToString("d MMM yyyy")
                                txtHingingDate.Enabled = False
                            Case 10
                                txtPackupDate.Text = CDate(dtJobStages.Rows(i)("CompletedDateTime")).ToString("d MMM yyyy")
                                txtPackupDate.Enabled = False
                            Case Else
                                'not implemented
                        End Select
                    End If
                Next
            End If

            dtJobStages.Dispose()
            dtJobStages = Nothing

            Dim bolCollectFromFactory As Boolean = False

            Dim cCustomer As Customer = _Service.GetCustomerByID(clsProdSchedule.CustomerID)

            If cCustomer.CustomerID > 0 Then
                bolCollectFromFactory = cCustomer.CollectionFromFactory
                txtSybizCustomerID.Text = cCustomer.SybizCustomerID

                InitCosting(cCustomer, clsProdSchedule.EnteredDatetime)

                pnlCreditHold.Visible = ((Not clsProdSchedule.CreditCheckOverride) AndAlso clsProdSchedule.CreditHold(_Service, cCustomer))
            End If

            If bolCollectFromFactory Then

                lblPostProduction1.Text = "Invoice Date"
                lblPostProduction1.ToolTip = "Invoiced"
                txtPostProduction1.Text = String.Empty

                tdPostProduction11.Attributes("class") = "form-label-td " & GetStatusColourClassName(ProductionScheduleStatus.Invoiced)
                tdPostProduction12.Attributes("class") = "form-label-td " & GetStatusColourClassName(ProductionScheduleStatus.Invoiced)

                If clsProdSchedule.InvoicedDate <> SharedConstants.DEFAULT_DATE_VALUE Then
                    txtPostProduction1.Text = Format(clsProdSchedule.InvoicedDate, "d MMM yyyy")
                    pnlCancellation.Visible = False
                End If

                lblPostProduction2.Text = "Collect Date"
                lblPostProduction2.ToolTip = "Collected"
                txtPostProduction2.Text = String.Empty

                tdPostProduction21.Attributes("class") = "form-label-td " & GetStatusColourClassName(ProductionScheduleStatus.Collected)
                tdPostProduction22.Attributes("class") = "form-label-td " & GetStatusColourClassName(ProductionScheduleStatus.Collected)

                If clsProdSchedule.CollectedFactoryDate <> SharedConstants.DEFAULT_DATE_VALUE Then
                    txtPostProduction2.Text = Format(clsProdSchedule.CollectedFactoryDate, "d MMM yyyy")
                    pnlCancellation.Visible = False
                End If
            Else
                lblPostProduction1.Text = "Despatch Date"
                lblPostProduction1.ToolTip = "Despatch"
                txtPostProduction1.Text = String.Empty

                tdPostProduction11.Attributes("class") = "form-label-td " & GetStatusColourClassName(ProductionScheduleStatus.Despatch)
                tdPostProduction12.Attributes("class") = "form-label-td " & GetStatusColourClassName(ProductionScheduleStatus.Despatch)

                Dim dtDespatch As DataTable = _Service.RunSQLScheduling("select * from dbo.tblJobStages where (StageID=7) and  (ScheduleID=" & intProdScheduleId.ToString & ")")

                If dtDespatch.Rows.Count > 0 Then
                    If Not IsDBNull(dtDespatch.Rows(0)("CompletedDateTime")) Then
                        txtPostProduction1.Text = CDate(dtDespatch.Rows(0)("CompletedDateTime")).ToString("d MMM yyyy")
                        pnlCancellation.Visible = False
                    Else
                        txtPostProduction1.Text = String.Empty
                    End If
                End If

                lblPostProduction2.Text = "Invoice Date"
                lblPostProduction2.ToolTip = "Invoiced"
                txtPostProduction2.Text = String.Empty

                tdPostProduction21.Attributes("class") = "form-label-td " & GetStatusColourClassName(ProductionScheduleStatus.Invoiced)
                tdPostProduction22.Attributes("class") = "form-label-td " & GetStatusColourClassName(ProductionScheduleStatus.Invoiced)

                If clsProdSchedule.InvoicedDate <> SharedConstants.DEFAULT_DATE_VALUE Then
                    txtPostProduction2.Text = Format(clsProdSchedule.InvoicedDate, "d MMM yyyy")
                    pnlCancellation.Visible = False
                End If
            End If

            'get status text
            Dim dtStatus As DataTable = _Service.RunSQLScheduling("select * from dbo.tblStatus")

            If dtStatus.Rows.Count > 0 Then
                Dim drows() As DataRow = dtStatus.Select("StatusID=" & clsProdSchedule.OrderStatus)

                If drows.Length > 0 Then
                    lblCurrentStatus.Text = drows(0)("StatusName").ToString

                    tdCurrentStatus1.Attributes("class") = "form-label-td " & GetStatusColourClassName(clsProdSchedule.OrderStatus)
                    tdCurrentStatus2.Attributes("class") = "form-label-td " & GetStatusColourClassName(clsProdSchedule.OrderStatus)
                End If
                drows = Nothing
            End If

            dtStatus.Dispose()
            dtStatus = Nothing

            If clsProdSchedule.OnHold = 1 Then
                chkHoldJob.Checked = True
                lblCurrentStatus.Text = "On Hold" & " - " & Me.lblCurrentStatus.Text
            End If

            chkCreditCheckOverride.Checked = clsProdSchedule.CreditCheckOverride

            If clsProdSchedule.CompletedDate <> SharedConstants.DEFAULT_DATE_VALUE Then
                pnlCancellation.Visible = False
            End If

            btnEmailQuote.Visible = clsProdSchedule.OrderStatus = ProductionScheduleStatus.Quote

            If clsProdSchedule.OrderStatus = 8 Then
                disableAllControls()
            End If

            pnlStopCustomer.Visible = False
            pnlAcceptanceAlert.Visible = False

            If clsProdSchedule.OrderStatus = ProductionScheduleStatus.AwaitingAcceptance Then

                ' Customer credit/cod check
                If clsProdSchedule.CreditOverrideUserID <= 0 Then

                    ' If not already overridden - do check
                    If txtSybizCustomerID.Text <> String.Empty Then
                        pnlStopCustomer.Visible = CheckSybizCustomerDetails()
                    End If
                End If

                If clsProdSchedule.OutstandingItemsConfirmID <= 0 Then

                    ' If not already overridden - do check. If awaiting acceptance, get checklist to display
                    CheckOutstandingItemsForAcceptance()

                    pnlAcceptanceAlert.Visible = (lblAcceptance.Text <> String.Empty)
                End If
            End If
            'Changed by Michael Behar - Ticket #62817 - Requesting For It To Be At Created Status (Saved Quote)
            'btnGenerateProductionSheet.Visible = clsProdSchedule.OrderStatus >= ProductionScheduleStatus.Quote
            'changed by surendra date:16/11/2020 ticket #62817
            btnGenerateProductionSheet.Visible = True
            'Changed by Michael Behar - Ticket #62229 - If order is accepted
            btnFactoryPaperwork.Visible = clsProdSchedule.OrderStatus > ProductionScheduleStatus.Quote

            ' Only display production sheet generation button when the production shedule has passed Awaiting Acceptance.
            'btnGenerateProductionSheet.Visible = clsProdSchedule.OrderStatus > ProductionScheduleStatus.AwaitingAcceptance
            'changed by surendra on 30-10-2020 ticket #62230 -
            btnGeneratesOPTIMISER.Visible = clsProdSchedule.OrderStatus > ProductionScheduleStatus.Quote
            'Added By Pradeep Singh on 05-10-2021
            'btExcelOPTIMISER.Visible = clsProdSchedule.OrderStatus >= ProductionScheduleStatus.Quote
	    ' Changed by Kartar on 09/02/2022
	    btExcelOPTIMISER.Visible = True
        Else
            Dim dteNow As Date = Date.Now

            txtEnteredDatetime.Text = Format(dteNow, "d MMM yyyy")
            lblExpiryDateTime.Text = Format(dteNow.AddDays(SharedConstants._INT_QUOTE_EXPIRY_DAYS), "d MMM yyyy HH:mm")

            cboOrderType.SelectedIndex = -1

            ' Production schedlule must be saved before progression past AA status is allowed.
            txtReceived.Enabled = False
            txtCheckMeasureDate.Enabled = False
            txtPickingDate.Enabled = False
            txtScheduledDate.Enabled = False
            txtCuttingDate.Enabled = False
            txtPiningDate.Enabled = False
            txtPrepDate.Enabled = False
            txtAssemblyDate.Enabled = False
            txtHingingDate.Enabled = False
            txtPackupDate.Enabled = False
            txtQCDate.Enabled = False
            txtPostProduction1.Enabled = False
            txtPostProduction2.Enabled = False
            txtInstallDate.Enabled = False

            btnGenerateProductionSheet.Visible = False
            btnGeneratesOPTIMISER.Visible = False
            'Added By Pradeep Singh on 05-10-2021
            btExcelOPTIMISER.Visible = False
        End If
        'added by surendra ticket #66281
        ''If there is any error in future then we will replace it with below condition used for ticket#66605
        If clsProdSchedule.OrderStatus <> ProductionScheduleStatus.Quote AndAlso clsProdSchedule.OrderStatus <> ProductionScheduleStatus.AwaitingAcceptance AndAlso clsProdSchedule.OrderStatus <> ProductionScheduleStatus.PaperworkProcessing Then
            btnAddDetails.Enabled = False
        Else
            btnAddDetails.Enabled = True
        End If

        ''Added by surendra Ticket #66605 Date-08-08-2021
        If String.IsNullOrEmpty(clsProdSchedule.SybizJobCode) Then
            cboOrderType.Enabled = True
        Else
            cboOrderType.Enabled = False
        End If

    End Sub

    Private Function GetStatusColourClassName(enumStatus As ProductionScheduleStatus) As String

        Select Case enumStatus
            Case ProductionScheduleStatus.Quote
                Return "quote"

            Case ProductionScheduleStatus.AwaitingAcceptance
                Return "awaitingAcceptance"

            Case ProductionScheduleStatus.PaperworkProcessing
                Return "orderAccepted"

            Case ProductionScheduleStatus.CheckMeasure
                Return "checkMeasure"

            Case ProductionScheduleStatus.Picking
                Return "picking"

            Case ProductionScheduleStatus.InProduction
                Return "inProduction"

            Case ProductionScheduleStatus.Despatch
                Return "despatched"

            Case ProductionScheduleStatus.Installed
                Return "invoiced"

            Case ProductionScheduleStatus.Invoiced
                Return "invoiced"

            Case ProductionScheduleStatus.Collected
                Return "collected"

        End Select

        Return String.Empty
    End Function

    Private Function Save() As Boolean
        Dim bolContinue As Boolean = False

        Page.Validate("productionschedule")

        If Page.IsValid() Then
            Dim intScheduleID As Integer = SharedConstants.DEFAULT_INTEGER_VALUE
            Dim intViewType As ViewType = ViewType.Add

            If txtViewType.Text <> String.Empty Then
                intViewType = CInt(txtViewType.Text)
            End If

            If txtId.Text <> String.Empty Then
                If CInt(txtId.Text) > 0 Then
                    intScheduleID = CInt(txtId.Text)
                End If
            End If

            bolContinue = True

            Dim dbConn As New DBConnection
            Dim cnn As SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
            Dim trans As SqlTransaction = Nothing
            Dim cProductionScheduleExisting As ProductionSchedule = Nothing
            Dim cNewProductionSchedule As ProductionSchedule = Nothing

            dbConn = Nothing
            lblStatus.Text = String.Empty

            Try
                cnn.Open()

                Dim dtLouvreSpecs As DataTable = _Service.GetLouvreSpecsByProdSchID(intScheduleID)

                trans = cnn.BeginTransaction

                Dim intMaxChangeId As Integer = _Service.GetProductScheduleHistoryNewChgID()

                cProductionScheduleExisting = _Service.GetProdScheduleClsByID(intScheduleID, cnn, trans)
                cNewProductionSchedule = CType(cProductionScheduleExisting.Clone, ProductionSchedule)
                Dim cLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")

                ' Modified by Kartar on 18Nov2022 Ticket#72522' Added By Pradeep Singh against ticket #72522
                If ViewState("AddressZoneID") > -1 Then
                    'If radDelivery.Checked AndAlso (txtFreightTotal.ToolTip <> _PRICE_OVERRIDDEN_MSG) Then
                    'If radDelivery.Checked AndAlso cNewProductionSchedule.FreightPriceIsOverridden = False Then
                    Dim Freightchk As String = txtFreightTotal.Text
                    If radDelivery.Checked Then
                        If Freightchk(0) = "$" Then
                            Freightchk = Freightchk.Substring(1)
                        End If
                        If Not Decimal.TryParse(Freightchk, Nothing) Then
                            RecalculateUIFreightFromDB()
                        ElseIf CDec(Freightchk) < CostingLouvres.GetMinimumFreight Then
                            RecalculateUIFreightFromDB()
                        End If
                        Freightchk = txtFreightTotal.Text
                        If Freightchk(0) = "$" Then
                            Freightchk = Freightchk.Substring(1)
                        End If
                        If Decimal.TryParse(Freightchk, Nothing) Then
                            If CDec(Freightchk) < CostingLouvres.GetMinimumFreight Then
                                txtFreightTotal.Text = CostingLouvres.GetMinimumFreight
                            End If
                        Else
                            txtFreightTotal.Text = CostingLouvres.GetMinimumFreight
                        End If
                    End If
                End If

                UpdateProductionScheduleForSave(cNewProductionSchedule)

                If bolContinue Then
                    If intViewType = ViewType.Update Then
                        bolContinue = _Service.UpdateProductionScheduleByID(cNewProductionSchedule, cnn, trans)
                    Else
                        cNewProductionSchedule.ProductTypeID = ProductType.Louvres
                        cNewProductionSchedule.ID = _Service.AddProductionScheduleRecord(cNewProductionSchedule, cnn, trans)
                        If cNewProductionSchedule.ID = SharedConstants.DEFAULT_INTEGER_VALUE Then
                            bolContinue = False
                        End If
                    End If
                End If

                If bolContinue Then
                    bolContinue = updateJobStages(cNewProductionSchedule.ID, cnn, trans)
                End If

                Dim cLouvreSpecs As New LouvreSpecs

                If bolContinue Then
                    'update louvre specs table

                    Dim bolAdd As Boolean = False

                    If dtLouvreSpecs.Rows.Count > 0 Then
                        cLouvreSpecs = _Service.GetLouvreSpecsClassFromDatarow(dtLouvreSpecs.Rows(0))
                    Else
                        cLouvreSpecs.ProductScheduleID = cNewProductionSchedule.ID
                        bolAdd = True
                    End If

                    cLouvreSpecs.LouvreJobTypeID = CInt(Me.cboJobType.SelectedValue)

                    If Me.txtCheckMeasureDate.Text <> String.Empty Then
                        cLouvreSpecs.CheckMeasureDate = CDate(Me.txtCheckMeasureDate.Text)
                    Else
                        cLouvreSpecs.CheckMeasureDate = SharedConstants.DEFAULT_DATE_VALUE
                    End If
                    If Me.cboCheckMeasure.SelectedIndex > 0 Then
                        cLouvreSpecs.CheckMeasureID = CInt(Me.cboCheckMeasure.SelectedValue)
                    Else
                        cLouvreSpecs.CheckMeasureID = SharedConstants.DEFAULT_INTEGER_VALUE
                    End If
                    If Me.txtInstallDate.Text <> String.Empty Then
                        cLouvreSpecs.InstallDate = CDate(Me.txtInstallDate.Text)
                    Else
                        cLouvreSpecs.InstallDate = SharedConstants.DEFAULT_DATE_VALUE
                    End If
                    If Me.cboInstaller.SelectedIndex > 0 Then
                        cLouvreSpecs.InstallID = CInt(Me.cboInstaller.SelectedValue)
                    Else
                        cLouvreSpecs.InstallID = SharedConstants.DEFAULT_INTEGER_VALUE
                    End If

                    'update database
                    If bolAdd Then
                        bolContinue = _Service.AddLouvreSpecs(cLouvreSpecs, cnn, trans)
                    Else
                        bolContinue = _Service.UpdateLouvreSpecs(cLouvreSpecs, cnn, trans)
                    End If

                End If

                dtLouvreSpecs.Dispose()
                dtLouvreSpecs = Nothing

                If intViewType = ViewType.Update And bolContinue Then
                    bolContinue = _Service.AddProdScheduleHistoryRcd(0, cProductionScheduleExisting, cnn, trans)
                End If

                Dim oldLouvreDetailsIDToNewID As New Dictionary(Of Integer, Integer)

                ' Louvre Details and their extras Deleted.
                Dim lDeletedDetailIDs As HashSet(Of Integer) = ViewState("DeletedDetailIDs")

                For Each id As Integer In lDeletedDetailIDs
                    _Service.DeleteLouvreDetailByID(id, cnn, trans)
                    _Service.DeleteLouvreExtraProductsByLouvreDetailsID(id, cnn, trans)
                Next id

                ' Louvre Details to Add
               'Below Boolean has been added By Pradeep Singh against ticket #72024
	        Dim bolChkMinOrder As Boolean = False
                If bolContinue Then
                    For Each l As LouvreDetails In cLouvreDetails
                        Dim oldID As Integer = l.LouvreDetailID
                        Dim intNewPSDetailsID As Integer = 0

                        'Added By Pradeep Singh against ticket #72024
                        Dim OrderTypVal = cboOrderType.SelectedValue
                        If l Is cLouvreDetails.Last AndAlso OrderTypVal = SharedEnums.OrderTypeEnum.Order Then
                            'Check each blind Price
                            bolChkMinOrder = bolCheck800(l)
                        End If

                        If l.LouvreDetailID <= 0 Then
                            ' Add
                            l.ProductionScheduleID = cNewProductionSchedule.ID
                            l.LouvreDetailID = SharedConstants.DEFAULT_INTEGER_VALUE
                            intNewPSDetailsID = _Service.AddLouvreDetails(l, cnn, trans)

                            If intNewPSDetailsID <= 0 Then
                                bolContinue = False
                            End If
                        Else
                            ' Update
                            bolContinue = _Service.UpdateLouvreDetails(l, cnn, trans)
                            intNewPSDetailsID = l.LouvreDetailID
                        End If

                        If bolContinue = False Then
                            Exit For
                        Else
                            oldLouvreDetailsIDToNewID.Add(oldID, intNewPSDetailsID)
                        End If
                    Next l
                    'Added By Pradeep Singh against ticket #72024
                    If bolChkMinOrder AndAlso bolContinue AndAlso cNewProductionSchedule.SalePrice < SharedConstants.Min_Order_Price Then
                        cNewProductionSchedule.SalePrice = cNewProductionSchedule.SalePrice + (SharedConstants.Min_Order_Price - CDec(cNewProductionSchedule.SalePrice))
                        bolContinue = _Service.UpdateProductionScheduleByID(cNewProductionSchedule, cnn, trans)
                    End If

                End If

                ' Powder Coating to Add
                If bolContinue Then
                    Dim lPowderCoating As List(Of AdditionalRequirements) = ViewState("PowderCoating")

                    For Each ar As AdditionalRequirements In lPowderCoating
                        If ar.AdditionalRequirementsID <= 0 Then
                            Dim newID As Integer

                            ' Add
                            ar.ProductionScheduleID = cNewProductionSchedule.ID

                            newID = _Service.AddAdditionalRequirementsRecord(ar, cnn, trans)

                            If newID <= 0 Then
                                bolContinue = False
                            End If
                        Else
                            ' Update
                            bolContinue = _Service.UpdateAdditionalRequirementsRecord(ar, cnn, trans)
                        End If

                        If bolContinue = False Then
                            Exit For
                        End If
                    Next ar
                End If

                ' Additional Requirements to Add
                If bolContinue Then
                    Dim lRequirements As List(Of AdditionalRequirements) = ViewState("Requirements")

                    For Each ar As AdditionalRequirements In lRequirements
                        If ar.AdditionalRequirementsID <= 0 Then
                            Dim newID As Integer

                            ' Add
                            ar.ProductionScheduleID = cNewProductionSchedule.ID

                            newID = _Service.AddAdditionalRequirementsRecord(ar, cnn, trans)

                            If newID <= 0 Then
                                bolContinue = False
                            End If
                        Else
                            ' Update
                            bolContinue = _Service.UpdateAdditionalRequirementsRecord(ar, cnn, trans)
                        End If

                        If bolContinue = False Then
                            Exit For
                        End If
                    Next ar
                End If

                ' Louvre Detail Extras
                If bolContinue Then
                    Dim dicExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")

                    For Each kvp As KeyValuePair(Of Integer, List(Of LouvreExtraProduct)) In dicExtras

                        ' Delete all extras for this detail from DB. Any still in dictionary will be recreated.
                        _Service.DeleteLouvreExtraProductsByLouvreDetailsID(kvp.Key, cnn, trans)

                        For Each cExtra As LouvreExtraProduct In kvp.Value
                            ' Get the new Louvre Details ID for the old one so the extras are mapped correctly.
                            cExtra.LouvreDetailsID = oldLouvreDetailsIDToNewID(cExtra.LouvreDetailsID)
                            cExtra.ProductionScheduleID = cNewProductionSchedule.ID

                            Dim intNewPSDetailsID As Integer = _Service.AddLouvreExtraProduct(cExtra, cnn, trans)

                            If intNewPSDetailsID <= 0 Then
                                bolContinue = False
                            End If

                            If bolContinue = False Then
                                Exit For
                            End If
                        Next cExtra
                    Next kvp
                End If

                ' Production Schedule Extras
                If bolContinue Then
                    Dim lExtras As List(Of LouvreExtraProduct) = ViewState("ProductionScheduleExtras")

                    ' Delete all extras for this prod schedule (not at detail level) from DB.
                    _Service.DeleteLouvreExtraProductsAtProductionScheduleLevelByPSID(txtIntScheduleID.Text, cnn, trans)

                    For Each cExtra As LouvreExtraProduct In lExtras

                        cExtra.ProductionScheduleID = cNewProductionSchedule.ID
                        cExtra.LouvreDetailsID = 0

                        Dim intNewExtraID As Integer = _Service.AddLouvreExtraProduct(cExtra, cnn, trans)

                        If intNewExtraID <= 0 Then
                            bolContinue = False
                        End If

                        If bolContinue = False Then
                            Exit For
                        End If
                    Next cExtra
                End If

                Dim lNotes As List(Of ProdScheduleNote) = ViewState("Notes")

                If bolContinue Then

                    For Each n As ProdScheduleNote In lNotes
                        ' New records dont have positive IDs so add them.
                        If n.ID <= 0 Then
                            ' Set the prod schedule ID for the note.
                            n.ProdScheduleID = cNewProductionSchedule.ID

                            If Not _Service.AddProdScheduleNoteRcd(n, cnn, trans) Then
                                bolContinue = False
                                Exit For
                            End If
                        End If
                    Next n
                End If

                If bolContinue Then
                    ' Add Uploaded Files
                    Dim lFiles As List(Of ProductionScheduleFile) = Session("ProdScheduleFiles")

                    For Each f As ProductionScheduleFile In lFiles
                        If f.ID <= 0 Then
                            ' Set the prod schedule ID for the note.
                            f.ProdScheduleID = cNewProductionSchedule.ID

                            If _Service.AddOrUpdateProductionScheduleFile(f, cnn, trans) <= 0 Then
                                bolContinue = False
                                Exit For
                            Else
                                ' Log the add
                                Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                                         Session("sessUserID"),
                                                         LogCategory.ProductionScheduleFile,
                                                         LogChangeType.Add,
                                                         String.Empty,
                                                         f.LogString,
                                                         "File " & f.FileName & " added to production schedule " & cProductionScheduleExisting.ShutterProNumber & ".",
                                                         Date.Now)

                                If Not _Service.Log.AddLogEntry(cLog, cnn, trans) Then
                                    bolContinue = False
                                    Exit For
                                End If
                            End If
                        End If
                    Next f
                End If

                If bolContinue Then
                    ' Edit Existing Files
                    Dim lFiles As List(Of ProductionScheduleFile) = Session("ProdScheduleFiles")
                    Dim lEditedFileIDs As List(Of Integer) = ViewState("EditedProdScheduleFileIDs")

                    For Each id As Integer In lEditedFileIDs
                        Dim cEditedFile As ProductionScheduleFile = lFiles.Find(Function(x) x.ID = id)

                        If cEditedFile IsNot Nothing AndAlso cEditedFile.ID > 0 Then

                            Dim cPreEditFile As ProductionScheduleFile = cEditedFile.Clone

                            If _Service.AddOrUpdateProductionScheduleFile(cEditedFile, cnn, trans) <= 0 Then
                                bolContinue = False
                                Exit For
                            Else
                                ' Log the delete
                                Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                                         Session("sessUserID"),
                                                         LogCategory.ProductionScheduleFile,
                                                         LogChangeType.Edit,
                                                         cPreEditFile.LogString,
                                                         cEditedFile.LogString,
                                                         "File " & cEditedFile.FileName & " edited in production schedule " & cProductionScheduleExisting.ShutterProNumber & ".",
                                                         Date.Now)

                                If Not _Service.Log.AddLogEntry(cLog, cnn, trans) Then
                                    bolContinue = False
                                    Exit For
                                End If
                            End If
                        End If
                    Next id
                End If

                If bolContinue Then
                    ' Remove Deleted Files
                    Dim lFileIDs As List(Of Integer) = ViewState("DeletedProdScheduleFileIDs")

                    For Each id As Integer In lFileIDs
                        Dim cRemovedFile As ProductionScheduleFile = _Service.GetProductionScheduleFileByID(False, id)

                        If cRemovedFile.ID > 0 Then
                            If _Service.DeleteProductionScheduleFileByID(id, cnn, trans) <= 0 Then
                                bolContinue = False
                                Exit For
                            Else
                                ' Log the delete
                                Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                                         Session("sessUserID"),
                                                         LogCategory.ProductionScheduleFile,
                                                         LogChangeType.Delete,
                                                         cRemovedFile.LogString,
                                                         String.Empty,
                                                         "File " & cRemovedFile.FileName & " removed from production schedule " & cProductionScheduleExisting.ShutterProNumber & ".",
                                                         Date.Now)

                                If Not _Service.Log.AddLogEntry(cLog, cnn, trans) Then
                                    bolContinue = False
                                    Exit For
                                End If
                            End If
                        End If
                    Next id
                End If

                Dim strSybizErrors As String = String.Empty

                'check and add job to sybiz job costing
                If bolContinue Then
                    Dim cNewProdScheduleContainer As New ProductionScheduleContainerLouvres
                    Dim cSybizJob As Vision.Platform.JobCosting.Job = Nothing
                    Dim strJobCostCreated As String = cNewProductionSchedule.SybizJobCode 'Ticket #66674

                    With cNewProdScheduleContainer
                        .ProductionSchedule = cNewProductionSchedule
                        .LouvreDetails = cLouvreDetails
                        .LouvreSpecs = cLouvreSpecs
                        .Notes = lNotes
                    End With
                    'added by surendra #66281 [created jobcost when louvres details greater than 0 and in producttion date is not null]
                    If cNewProductionSchedule.OrderStatus = SharedEnums.ProductionScheduleStatus.Picking AndAlso (dgvDetails.Rows.Count > 0 Or gdvExtrasProdSchedule.Rows.Count > 0) Then
                        bolContinue = SyBizShared.AddJobToSybizIfNeeded(cProductionScheduleExisting, cNewProdScheduleContainer, strSybizErrors, cnn, trans) 'Ticket #66674
                    End If

                    ' If there is no sybiz job it did not need to be created, so it may need updating.
                    'Code reworked - Ticket #66590 - Kartar Singh and Michael Behar / Ticket #66674
                    If bolContinue Then
                        If cNewProductionSchedule.SybizJobCode = "" Then 'Ticket #66674
                            If cNewProductionSchedule.OrderStatus > ProductionScheduleStatus.PaperworkProcessing AndAlso cNewProductionSchedule.OrderStatus <> ProductionScheduleStatus.Picking Then
                                'Commented 03-08-2021 - Ticket #66590 - By Kartar and Michael
                                bolContinue = SyBizShared.AddJobToSybizIfNeeded(cProductionScheduleExisting, cNewProdScheduleContainer, strSybizErrors, cnn, trans, cSybizJob, True)
                            End If
                        Else
                            bolContinue = SyBizShared.SybizUpdateJobIfChanged(cNewProdScheduleContainer, strSybizErrors)
                        End If
                    End If
                End If



                If bolContinue Then
                    trans.Commit()

                    ' Clear temp storage for uploaded files after a save
                    Session("ProdScheduleFiles") = New List(Of ProductionScheduleFile)
                Else
                    'EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - Transaction Rolled Back" & Environment.NewLine & getPageInfo())
                    trans.Rollback()

                    lblStatus.ForeColor = Color.Red
                    lblStatus.Text = "Error saving details. Please try again."

                    If strSybizErrors.Length > 0 Then
                        lblStatus.Text &= SharedConstants.STR_BREAK & strSybizErrors
                    End If
                End If

            Catch ex As Exception
                If Not trans Is Nothing Then
                    trans.Rollback()
                End If
                If cnn.State = ConnectionState.Open Then
                    cnn.Close()
                End If
                EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message & Environment.NewLine & GetPageInfo(form1, Session))
                bolContinue = False
            Finally
                trans.Dispose()
                trans = Nothing
                If cnn.State = ConnectionState.Open Then
                    cnn.Close()
                End If
                cnn.Dispose()
                cnn = Nothing
            End Try

            dbConn = Nothing

            ' Send any required auto emails.
            If bolContinue Then

                Dim cCustomer As Customer = _Service.GetCustomerByID(cNewProductionSchedule.CustomerID)

                If cCustomer.CustomerID > 0 Then

                    Dim cUser As User = _Service.getUserByID(Session("sessUserID"))

                    If cUser.ID > 0 Then
                        ' Send confirmation email if status has progressed past Awaiting Acceptance.
                        If cProductionScheduleExisting.OrderStatus <= ProductionScheduleStatus.AwaitingAcceptance AndAlso cNewProductionSchedule.OrderStatus > ProductionScheduleStatus.AwaitingAcceptance Then
                            If _Service.Mail.CustomerIsOnActiveMailingList(MailingListID.OrderConfirmation, cCustomer.CustomerID) Then
                                _Service.Mail.SendOrderConfirmationEmail(cCustomer, cUser, cNewProductionSchedule.ID, cNewProductionSchedule.OrderReference)
                            End If
                        End If

                        Dim lCompletedStatuses As List(Of ProductionScheduleStatus)

                        ' Send order completed email
                        If cCustomer.CollectionFromFactory Then
                            lCompletedStatuses = New List(Of ProductionScheduleStatus) From {ProductionScheduleStatus.Invoiced,
                                                                                             ProductionScheduleStatus.Installed,
                                                                                             ProductionScheduleStatus.Collected}
                        Else
                            lCompletedStatuses = New List(Of ProductionScheduleStatus) From {ProductionScheduleStatus.Invoiced,
                                                                                             ProductionScheduleStatus.Installed,
                                                                                             ProductionScheduleStatus.Despatch}
                        End If

                        ' If moving from a non completed status to a completed status, send the email.
                        If Not lCompletedStatuses.Contains(cProductionScheduleExisting.OrderStatus) AndAlso lCompletedStatuses.Contains(cNewProductionSchedule.OrderStatus) Then
                            If _Service.Mail.CustomerIsOnActiveMailingList(MailingListID.OrderCompleted, cCustomer.CustomerID) Then
                                _Service.Mail.SendOrderCompletedEmail(cCustomer, cUser, cNewProductionSchedule.ID, cNewProductionSchedule.OrderReference)
                            End If
                        End If
                        'Sending a ordered email copy to ozroll. Added by Surendra on 29/09/2020
                        If CInt(Session("CustomerID") > 0 And txtOrderDate.Text.Length > 6 And cProductionScheduleExisting.OrderStatus = ProductionScheduleStatus.Quote) Then
                            'If intScheduleID = SharedConstants.DEFAULT_INTEGER_VALUE Then
                            cCustomer.Email = SharedConstants.ORDER_EMAIL_COPY_TO_OZROLL_QLD
                            _Service.Mail.SendOrderEmailCopyToOzroll(cCustomer, cUser, cNewProductionSchedule.ID, cNewProductionSchedule.OrderReference)
                            'End If
                        End If

                        'Added by surendra Ticket.#66352
                        If cProductionScheduleExisting.ExpectedShippingDate <> SharedConstants.DEFAULT_DATE_VALUE Then
                            If cProductionScheduleExisting.ExpectedShippingDate <> cNewProductionSchedule.ExpectedShippingDate Then
                                If String.IsNullOrEmpty(cCustomer.Email) Then
                                    cCustomer.Email = cUser.Email
                                End If
                                _Service.Mail.SendEmailOnShippingDateChanged(cCustomer, cUser, cProductionScheduleExisting.ExpectedShippingDate, cNewProductionSchedule.ExpectedShippingDate,
cNewProductionSchedule.ShutterProNumber, cNewProductionSchedule.OrderReference)

                            End If
                        End If

                    End If
                End If
            End If

            If bolContinue Then
                If cNewProductionSchedule IsNot Nothing Then
                    lblStatus.ForeColor = Color.Green
                    lblStatus.Text = "Details successfully saved."
                    txtId.Text = cNewProductionSchedule.ID
                    txtIntScheduleID.Text = cNewProductionSchedule.ID
                Else
                    lblStatus.ForeColor = Color.Red
                    lblStatus.Text = "Error saving details. Please try again."
                End If
            End If
        End If

        Return bolContinue
    End Function

    Private Function bolCheck800(ByRef l As LouvreDetails) As Boolean

        Dim bolBelow800 As Boolean = False
        'Compare the price for the $800 for total for last blind
        If CDec(txtSalePrice.Text) < SharedConstants.Min_Order_Price Then
            l.SalePrice = l.SalePrice + (SharedConstants.Min_Order_Price - CDec(txtSalePrice.Text))
            txtSalePrice.Text = SharedConstants.Min_Order_Price
            bolBelow800 = True
        End If
        Return bolBelow800
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Save() Then
            If CInt(txtViewType.Text) = ViewType.Add Then
                Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & " &ViewType=" & ViewType.Update & "&Status=1", False)
            Else
                Response.Redirect("LouvreJobDetails.aspx?" & GetCleanQueryString() & "&Status=1", False)
            End If
        End If
    End Sub

    Private Sub btnGenerateOrder_Click(sender As Object, e As EventArgs) Handles btnGenerateOrder.Click

        If txtId.Text <> String.Empty Then
            If Save() Then
                If CInt(txtViewType.Text) = ViewType.Add Then
                    Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=order", False)
                Else
                    Response.Redirect("LouvreJobDetails.aspx?" & GetCleanQueryString() & "&Status=1" & "&doc=order", False)
                End If
            End If
        End If
    End Sub

    Private Sub btnEmailQuote_Click(sender As Object, e As EventArgs) Handles btnEmailQuote.Click
        If txtId.Text <> String.Empty Then
            If Save() Then
                If CInt(txtViewType.Text) = ViewType.Add Then
                    Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=emailquote", False)
                Else
                    Response.Redirect("LouvreJobDetails.aspx?" & GetCleanQueryString() & "&Status=1" & "&doc=emailquote", False)
                End If
            End If
        End If
    End Sub

    Private Sub btnGenerateProductionSheet_Click(sender As Object, e As EventArgs) Handles btnGenerateProductionSheet.Click
        If txtId.Text <> String.Empty Then
            If Save() Then
                If CInt(txtViewType.Text) = ViewType.Add Then
                    Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=productionsheet", False)
                Else
                    Response.Redirect("LouvreJobDetails.aspx?" & GetCleanQueryString() & "&Status=1" & "&doc=productionsheet", False)
                End If
            End If
        End If
    End Sub

    Private Sub btnGeneratesOPTIMISER_Click(sender As Object, e As EventArgs) Handles btnGeneratesOPTIMISER.Click
        If txtId.Text <> String.Empty Then
            If Save() Then
                If CInt(txtViewType.Text) = ViewType.Add Then
                    Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=optimisersheet", False)
                Else
                    Response.Redirect("LouvreJobDetails.aspx?" & GetCleanQueryString() & "&Status=1" & "&doc=optimisersheet", False)
                End If
            End If
        End If
    End Sub

    'Added By Pradeep Singh on 05-10-2021
    Private Sub btExcelOPTIMISER_Click(sender As Object, e As EventArgs) Handles btExcelOPTIMISER.Click
        If txtId.Text <> String.Empty Then
            'If Save() Then
            If CInt(txtViewType.Text) = ViewType.Add Then
                    Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=excelpowdercoat", False)
                Else
                    Response.Redirect("LouvreJobDetails.aspx?" & GetCleanQueryString() & "&Status=1" & "&doc=excelpowdercoat", False)
                End If
            ' End If
        End If
    End Sub



    Private Sub GeneratePDF(cPDFType As PDFType)

        Select Case cPDFType
            Case PDFType.Order
                Dim cLouvreOrder As New LouvreOrder
                Dim cPDF As PDFGenerationResult = cLouvreOrder.GetLouvreOrderPDF(txtIntScheduleID.Text)

                Response.Clear()
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-Disposition", "attachment;filename=" & cPDF.FileName)
                Response.BinaryWrite(cPDF.PDFBytes)
                Response.End()

            Case PDFType.CoverSheet
                Dim cCoverSheet As New LouvreJobCoverSheet

                cCoverSheet.GetProjectCoverSheetPDF(txtIntScheduleID.Text)

            Case PDFType.DeliveryDocket
                LouvreJobDeliveryDocket.GetLouvreJobDeliveryDocket(txtIntScheduleID.Text)

            Case PDFType.RunningSheet
                LouvreJobRunningSheet.GetJobRunningSheetPDF(txtIntScheduleID.Text)

            Case PDFType.TimeSheet
                LouvreJobTimeSheet.JobTimeSheet(txtIntScheduleID.Text)

            Case PDFType.ProductionSheet
                Dim cProdSheet As New ProductionScheduleGenerator
                Dim cPDF As PDFGenerationResult = cProdSheet.GeneratePDF(txtIntScheduleID.Text)

                Response.Clear()
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-Disposition", "attachment;filename=" & cPDF.FileName)
                Response.BinaryWrite(cPDF.PDFBytes)
                Response.End()
            'added by surendra 22-10-2020
            Case PDFType.OptimiserSheet
                Dim cProdSheet As New ProductionScheduleGenerator
                Dim cPDF As PDFGenerationResult = cProdSheet.GenerateOptimiserPowderCoatPDF(txtIntScheduleID.Text)
                Response.Clear()
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-Disposition", "attachment;filename=" & cPDF.FileName.Replace("ProductionSchedule_", "OptimiserPowderCoat_"))
                Response.BinaryWrite(cPDF.PDFBytes)
                Response.End()

                'Added By Pradeep Singh 5-10-2021 against ticket #67162
            Case PDFType.ExcelPowderCoat
                Dim cProdSheet As New ProductionScheduleGenerator
                Dim dt As DataTable = cProdSheet.GenerateOptimiserPowderCoatExcel(txtIntScheduleID.Text)
                Response.Clear()
                GetSSRSReport(dt)
        End Select

    End Sub

    'Added By Pradeep Singh on 05-10-2021 against ticket #67162
    Public Sub GetSSRSReport(dt As DataTable)
        Dim rds As New ReportDataSource("DataSet1", dt)
        'Set up the whole basis of the report
        Dim warnings As Warning() = Nothing
        Dim streamIds As String() = Nothing
        Dim strDeviceInfo As String = String.Empty
        Dim mimeType As String = String.Empty
        Dim encoding As String = String.Empty
        Dim extension As String = String.Empty

        'when dt is empty  - Added by Pradeep on 5th Dec 2021  Ticket#67162
        If dt.Rows.Count = 0 Then
            lblStatus.ForeColor = Color.Red
            lblStatus.Text = "There is no item for Powder Coat."
            Exit Sub
        End If
        Try
            'Create Report - Get Path - Add Data Source
            Dim reportViewer As New ReportViewer()
            reportViewer.ProcessingMode = ProcessingMode.Local
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) & "Reports\RDL\PowderCoatReport.rdl"
            reportViewer.LocalReport.DataSources.Add(rds)
            reportViewer.SizeToReportContent = True
            reportViewer.Width = Unit.Percentage(150)
            reportViewer.Height = Unit.Percentage(150)

            'Create Bytes Of Report

            Dim bytes As Byte() = reportViewer.LocalReport.Render("Excel", strDeviceInfo, mimeType, encoding, extension, streamIds, warnings)


            'Download the RDLC Report in Word, Excel, PDF and Image formats.
            Response.Buffer = True
            Response.Clear()
            Response.ContentType = mimeType
            Response.AddHeader("content-disposition", "attachment; filename=PowderCoatReport." & extension)
            Response.BinaryWrite(bytes)
            ' create the file
            Response.Flush()

            dt.Dispose()
            dt = Nothing


        Catch ex As Exception

        End Try


    End Sub

    Private Sub EmailPDF(cPDFType As PDFType)

        Select Case cPDFType
            Case PDFType.Order
                Dim cProdSchedule As ProductionSchedule = _Service.GetProdScheduleClsByID(txtIntScheduleID.Text)

                If cProdSchedule.ID > 0 Then
                    Dim cUser As User = _Service.getUserByID(Session("sessUserID"))

                    If cUser.ID > 0 Then
                        Dim cCustomer As Customer = _Service.GetCustomerByID(cProdSchedule.CustomerID)

                        If cCustomer.CustomerID > 0 Then
                            _Service.Mail.SendQuoteEmail(cCustomer, cUser, cProdSchedule.ID, cProdSchedule.OrderReference, cProdSchedule.ProductTypeID)
                        End If
                    End If
                End If

        End Select

    End Sub

    Private Sub lnkEmailQuotePDFDummy_Click(sender As Object, e As EventArgs) Handles lnkEmailQuotePDFDummy.Click
        EmailPDF(PDFType.Order)
    End Sub

    Private Sub lnkGenerateOrderPDFDummy_Click(sender As Object, e As EventArgs) Handles lnkGenerateOrderPDFDummy.Click
        GeneratePDF(PDFType.Order)
    End Sub

    Private Sub lnkGenerateDeliveryDocketPDFDummy_Click(sender As Object, e As EventArgs) Handles lnkGenerateDeliveryDocketPDFDummy.Click
        GeneratePDF(PDFType.DeliveryDocket)
    End Sub

    Private Sub lnkGenerateRunningSheetPDFDummy_Click(sender As Object, e As EventArgs) Handles lnkGenerateRunningSheetPDFDummy.Click
        GeneratePDF(PDFType.RunningSheet)
    End Sub

    Private Sub lnkGenerateTimeSheetPDFDummy_Click(sender As Object, e As EventArgs) Handles lnkGenerateTimeSheetPDFDummy.Click
        GeneratePDF(PDFType.TimeSheet)
    End Sub

    Private Sub lnkGenerateCoverSheetPDFDummy_Click(sender As Object, e As EventArgs) Handles lnkGenerateCoverSheetPDFDummy.Click
        GeneratePDF(PDFType.CoverSheet)
    End Sub
    'added by surendra 22-10-2020
    Private Sub lnkGenerateOptimiserSheetPDFDummy_Click(sender As Object, e As EventArgs) Handles lnkGenerateOptimiserSheetPDFDummy.Click
        GeneratePDF(PDFType.OptimiserSheet)
    End Sub
    'Added by Pradeep Singh 5-10-2021
    Private Sub lnkGenerateOptimiserSheetExcelDummy_Click(sender As Object, e As EventArgs) Handles lnkGenerateOptimiserSheetExcelDummy.Click
        GeneratePDF(PDFType.ExcelPowderCoat)
    End Sub

    Private Sub lnkGenerateProductionSheetPDFDummy_Click(sender As Object, e As EventArgs) Handles lnkGenerateProductionSheetPDFDummy.Click
        GeneratePDF(PDFType.ProductionSheet)
    End Sub

    Protected Function updateJobStages(intScheduleID As Integer, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean
        Dim bolSavedOK As Boolean = True
        Dim lJobStages As List(Of JobStages) = _Service.GetJobStagesByProductionScheduleID(intScheduleID, cnn, trans)
        Dim cCustomer As Customer = Nothing

        If IsNumeric(cboCustomer.SelectedValue) Then
            cCustomer = _Service.GetCustomerByID(cboCustomer.SelectedValue, cnn, trans)
        End If

        If cCustomer Is Nothing OrElse cCustomer.CustomerID <= 0 Then
            bolSavedOK = False
        End If

        If bolSavedOK Then
            For i As JobStagesEnum = 1 To 10
                Dim j As Integer = i
                Dim cJobStage As JobStages = lJobStages.Find(Function(x) x.StageID = j)
                Dim txt As TextBox = Nothing
                Dim boolNewJobStage As Boolean = False
                Dim bolCollectFromFactory As Boolean = False

                Select Case i
                    Case JobStagesEnum.Cutting
                        txt = txtCuttingDate
                    Case JobStagesEnum.Prep
                        txt = txtPrepDate
                    Case JobStagesEnum.Assembly
                        txt = txtAssemblyDate
                    Case JobStagesEnum.Framing
                        ' Not used
                        txt = Nothing
                    Case JobStagesEnum.QC
                        txt = txtQCDate
                    Case JobStagesEnum.Wrapping
                        ' Not used
                        txt = Nothing
                    Case JobStagesEnum.Despatch
                        txt = txtPostProduction1
                    Case JobStagesEnum.Pinning
                        txt = txtPiningDate
                    Case JobStagesEnum.Hanging
                        txt = txtHingingDate
                    Case JobStagesEnum.Packup
                        txt = txtPackupDate
                    Case Else
                        'shouldn't happen
                        txt = Nothing
                End Select

                If txt IsNot Nothing Then

                    If cJobStage Is Nothing Then

                        ' Record doesn't exist yet so setup a new one.
                        cJobStage = New JobStages
                        cJobStage.StageID = i
                        cJobStage.ScheduleID = intScheduleID

                        boolNewJobStage = True
                    End If

                    With cJobStage

                        ' If the date is empty, clear the values in the DB. If it is dispatch, only write the values if not collecting from factory, otherwise clear them.
                        If txt.Text <> String.Empty AndAlso (i <> JobStagesEnum.Despatch OrElse Not cCustomer.CollectionFromFactory) Then
                            .StageStatus = JobStageStatus.Completed
                            .CompletedDateTime = CDate(txt.Text)
                            .CompletedByID = CInt(Session("sessUserID"))
                        Else
                            ' Stage not started so wipe the date.
                            .StageStatus = JobStageStatus.NotStarted
                            .CompletedDateTime = SharedConstants.MIN_DATE
                            .CompletedByID = 0
                        End If

                    End With

                    If boolNewJobStage Then
                        bolSavedOK = _Service.AddJobStages(cJobStage, cnn, trans)
                    Else
                        bolSavedOK = _Service.UpdateJobStages(cJobStage, cnn, trans)
                    End If

                    If bolSavedOK Then
                        bolSavedOK = _Service.AddJobStagesHistoryRecord(cJobStage, Session("sessUserID"), cnn, trans)
                    End If

                    If Not bolSavedOK Then
                        Exit For
                    End If
                End If
            Next i
        End If

        Return bolSavedOK

    End Function

    ''' <summary>
    ''' Removes all this pages variables from the query string.
    ''' </summary>
    ''' <returns>A version of the query string with all page related variables removed.</returns>
    Private Function GetCleanQueryString() As String
        Dim strCleanQueryString As String = Request.QueryString.ToString.Replace("&Status=1", String.Empty)

        strCleanQueryString = strCleanQueryString.Replace("&doc=order", String.Empty)
        strCleanQueryString = strCleanQueryString.Replace("&doc=deliverydocket", String.Empty)
        strCleanQueryString = strCleanQueryString.Replace("&doc=runningsheet", String.Empty)
        strCleanQueryString = strCleanQueryString.Replace("&doc=timesheet", String.Empty)
        strCleanQueryString = strCleanQueryString.Replace("&doc=coversheet", String.Empty)
        strCleanQueryString = strCleanQueryString.Replace("&doc=emailquote", String.Empty)
        strCleanQueryString = strCleanQueryString.Replace("&doc=productionsheet", String.Empty)
        'added by surendra 22-10-2020 Ticket#62230 
        strCleanQueryString = strCleanQueryString.Replace("&doc=optimisersheet", String.Empty)
	'Added by Pradeep on 5 Dec 2021 Ticket#67612
        strCleanQueryString = strCleanQueryString.Replace("&doc=excelpowdercoat", String.Empty)

        Return strCleanQueryString
    End Function

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        ' Clear temp storage for uploaded files
        Session("ProdScheduleFiles") = Nothing

        If ViewState("ReturnURL") = CInt(ReturnURLID.UpdateAwaitingAcceptance) Then
            Response.Redirect("UpdateAwaitingAcceptance.aspx", False)
        Else
            'changes by surendra 16 Aug-2020
            ''check internal customer permission if user is not admin by surendra dt:06/06/2023
            If (Convert.ToInt32(Session("CustomerID")) > 0) Or (Convert.ToInt32(Session("CustomerId")) <= 0 AndAlso bolHasPermission=False) Then
                Response.Redirect("Dashboard.aspx", False)
                ''added by surendra surendra dt:01/06/2023 to check
                ''check internal customer permission if user is not admin by surendra dt:06/06/2023
            ElseIf bolHasPermission Then
                Response.Redirect("ProductionScheduleList.aspx?" & GetCleanQueryString(), False)
            End If
        End If

    End Sub

    Protected Sub UpdateProductionScheduleForSave(ByRef cNewProductionSchedule As ProductionSchedule)

        If CInt(txtViewType.Text) = ViewType.Update Then
            cNewProductionSchedule.ID = txtId.Text
            cNewProductionSchedule.JobNumber = txtJobNumber.Text
        End If

        cNewProductionSchedule.OrderReference = RemoveSqlChars(txtContractNumber.Text).Trim

        If cboCustomer.SelectedIndex > -1 Then
            cNewProductionSchedule.CustomerID = cboCustomer.SelectedValue
        End If

        cNewProductionSchedule.CustomerName = RemoveSqlChars(txtCustomerName.Text).Trim
        cNewProductionSchedule.EnteredDatetime = CDate(txtEnteredDatetime.Text)

        If IsDate(lblExpiryDateTime.Text) AndAlso CalculateStatus() = ProductionScheduleStatus.Quote Then
            cNewProductionSchedule.QuoteExpiryDateTime = CDate(lblExpiryDateTime.Text)
        Else
            cNewProductionSchedule.QuoteExpiryDateTime = SharedConstants.MAX_DATE
        End If

        If txtState.Text <> String.Empty Then
            cNewProductionSchedule.State = txtState.Text
        Else
            cNewProductionSchedule.State = String.Empty
        End If

        cNewProductionSchedule.OzrollContractNo = RemoveSqlChars(txtOzrollContract.Text).Trim

        If (txtOrderDate.Text <> String.Empty) Then
            cNewProductionSchedule.OrderDate = CDate(txtOrderDate.Text)
        Else
            cNewProductionSchedule.OrderDate = SharedConstants.DEFAULT_DATE_VALUE
        End If

        If IsNumeric(txtSalePrice.Text) Then
            cNewProductionSchedule.SalePrice = CDec(txtSalePrice.Text)
            cNewProductionSchedule.SalePriceIsValid = True
        Else
            cNewProductionSchedule.SalePrice = 0
            cNewProductionSchedule.SalePriceIsValid = False
        End If

        If IsNumeric(txtFreightTotal.Text) Then
            cNewProductionSchedule.FreightAmount = CDec(txtFreightTotal.Text)
        Else
            cNewProductionSchedule.FreightAmount = 0
        End If

        If txtTotalSQM.Text <> String.Empty Then
            cNewProductionSchedule.TotalSQM = CDec(txtTotalSQM.Text)
        Else
            cNewProductionSchedule.TotalSQM = 0
        End If

        If txtTotalPanels.Text <> String.Empty Then
            cNewProductionSchedule.TotalPanels = CInt(txtTotalPanels.Text)
        Else
            cNewProductionSchedule.TotalPanels = 0
        End If

        If txtPromisedDate.Text <> String.Empty Then
            cNewProductionSchedule.PromisedDate = CDate(txtPromisedDate.Text)
        Else
            cNewProductionSchedule.ActualShippingDate = SharedConstants.DEFAULT_DATE_VALUE
        End If

        If txtPlannedShippingDate.Text <> String.Empty Then
            cNewProductionSchedule.ExpectedShippingDate = CDate(txtPlannedShippingDate.Text)
        Else
            cNewProductionSchedule.ExpectedShippingDate = SharedConstants.DEFAULT_DATE_VALUE
        End If

        If txtActualShippingDate.Text <> String.Empty Then
            cNewProductionSchedule.ActualShippingDate = CDate(txtActualShippingDate.Text)
        Else
            cNewProductionSchedule.ActualShippingDate = SharedConstants.DEFAULT_DATE_VALUE
        End If

        If txtScheduledDate.Text <> String.Empty Then
            cNewProductionSchedule.ScheduledDate = CDate(txtScheduledDate.Text)
        Else
            cNewProductionSchedule.ScheduledDate = SharedConstants.DEFAULT_DATE_VALUE
        End If

        If cboOrderType.SelectedIndex > 0 Then
            cNewProductionSchedule.OrderTypeID = CInt(cboOrderType.SelectedValue)
        End If

        If cNewProductionSchedule.OrderTypeID > 0 Then
            cNewProductionSchedule.RemakeIssueDescription = txtRemakeDescription.Text
        Else
            cNewProductionSchedule.RemakeIssueDescription = String.Empty
        End If

        If cboPriority.SelectedIndex > 0 Then
            cNewProductionSchedule.PriorityLevel = CInt(cboPriority.SelectedValue)
        Else
            cNewProductionSchedule.PriorityLevel = SharedConstants.DEFAULT_INTEGER_VALUE
        End If

        Dim bolCollectFromFactory As Boolean = False
        Dim dtCustomer As DataTable = _Service.RunSQLScheduling("Select * from dbo.tblCustomers where CustomeriD=" & cboCustomer.SelectedValue.ToString)

        If dtCustomer.Rows.Count > 0 Then
            If Not IsDBNull(dtCustomer.Rows(0)("CollectionFromFactory")) Then
                If CInt(dtCustomer.Rows(0)("CollectionFromFactory")) = 1 Then
                    bolCollectFromFactory = True
                End If
            End If
        End If

        dtCustomer.Dispose()
        dtCustomer = Nothing

        If bolCollectFromFactory Then
            If txtPostProduction1.Text <> String.Empty Then
                cNewProductionSchedule.InvoicedDate = CDate(txtPostProduction1.Text)
            Else
                cNewProductionSchedule.InvoicedDate = SharedConstants.DEFAULT_DATE_VALUE
            End If

            If txtPostProduction2.Text <> String.Empty Then
                cNewProductionSchedule.CollectedFactoryDate = CDate(txtPostProduction2.Text)
                cNewProductionSchedule.CompletedDate = DateTime.Now
            Else
                cNewProductionSchedule.CollectedFactoryDate = SharedConstants.DEFAULT_DATE_VALUE
                cNewProductionSchedule.CompletedDate = SharedConstants.DEFAULT_DATE_VALUE
            End If
        Else
            'despatch is first - handle separately

            If txtPostProduction2.Text <> String.Empty Then
                cNewProductionSchedule.InvoicedDate = CDate(txtPostProduction2.Text)
                cNewProductionSchedule.CompletedDate = DateTime.Now
            Else
                cNewProductionSchedule.InvoicedDate = SharedConstants.DEFAULT_DATE_VALUE
                cNewProductionSchedule.CompletedDate = SharedConstants.DEFAULT_DATE_VALUE
            End If
        End If

        If chkHoldJob.Checked = True Then
            cNewProductionSchedule.OnHold = 1
        Else
            cNewProductionSchedule.OnHold = 0
        End If

        If txtReceived.Text <> String.Empty Then
            cNewProductionSchedule.ReceivedDate = CDate(txtReceived.Text)
        Else
            cNewProductionSchedule.ReceivedDate = SharedConstants.DEFAULT_DATE_VALUE
        End If

        If txtPickingDate.Text <> String.Empty Then
            cNewProductionSchedule.PickingDate = CDate(txtPickingDate.Text)
        Else
            cNewProductionSchedule.PickingDate = SharedConstants.DEFAULT_DATE_VALUE
        End If

        cNewProductionSchedule.OrderStatus = CalculateStatus()
        cNewProductionSchedule.ProductTypeID = CInt(txtProductTypeID.Text)

        If IsNumeric(cboDeliveryAddress.SelectedValue) Then
            cNewProductionSchedule.DeliveryAddressID = cboDeliveryAddress.SelectedValue
        Else
            cNewProductionSchedule.DeliveryAddressID = 0
        End If

        If cNewProductionSchedule.DeliveryAddressID <= 0 Then
            cNewProductionSchedule.State = String.Empty
        End If

        cNewProductionSchedule.AddressZoneID = ViewState("AddressZoneID")
        cNewProductionSchedule.CreditCheckOverride = chkCreditCheckOverride.Checked

    End Sub

    Protected Sub processClearButton(sender As Object, e As System.EventArgs) Handles btnClearReceived.Click, btnClearScheduled.Click,
                              btnClearCutting.Click, btnClearPrep.Click, btnClearAssembly.Click, btnClearQC.Click,
                              btnClearPostProduction1.Click, btnClearPostProduction2.Click, btnClearPicking.Click, btnClearCM.Click, btnClearInstall.Click,
                              btnClearPining.Click, btnClearHinging.Click, btnClearPackup.Click

        Dim btn As Button = CType(sender, Button)
        Dim cTxtDate As TextBox = Page.FindControl(btn.CommandArgument)

        If cTxtDate IsNot Nothing Then
            cTxtDate.Enabled = True
            cTxtDate.Text = String.Empty

            If Save() Then
                If CInt(txtViewType.Text) = ViewType.Add Then
                    Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & " &ViewType=" & ViewType.Update & "&Status=1", False)
                Else
                    Response.Redirect("LouvreJobDetails.aspx?" & GetCleanQueryString() & "&Status=1", False)
                End If
            End If
        End If

    End Sub

    Private Sub btnCancellation_Click(sender As Object, e As EventArgs) Handles btnCancellation.Click
        If Save() Then
            Response.Redirect("ConfirmCancelJob.aspx?" & Request.QueryString.ToString, False)
        End If
    End Sub

    Protected Sub CheckAndSetClearButtonVisibilityForStatusDates()
        Dim lTxtDates As List(Of TextBox) = GetDateTextBoxesInOrder(True)
        Dim prevTxt As TextBox = Nothing

        ' Hide all clear buttons
        btnClearAssembly.Visible = False
        btnClearCM.Visible = False
        btnClearCutting.Visible = False
        btnClearHinging.Visible = False
        btnClearInstall.Visible = False
        btnClearPackup.Visible = False
        btnClearPicking.Visible = False
        btnClearPining.Visible = False
        btnClearPostProduction1.Visible = False
        btnClearPostProduction2.Visible = False
        btnClearPrep.Visible = False
        btnClearQC.Visible = False
        btnClearReceived.Visible = False
        btnClearScheduled.Visible = False

        If CalculateStatus() <> ProductionScheduleStatus.PaperworkProcessing Then
            Dim boolFoundClearableBox As Boolean = False

            ' Loop the date boxes in order finding the first enabled one. Set the previous to clearable.
            For Each t As TextBox In lTxtDates
                If t.Enabled Then
                    boolFoundClearableBox = True
                    ' The box before this one is clearable.
                    Exit For
                End If

                ' Remember this as the last box.
                prevTxt = t
            Next t

            If boolFoundClearableBox AndAlso (prevTxt IsNot Nothing) Then
                If prevTxt.Attributes("ClearButton") IsNot Nothing Then
                    Dim btn As Button = Page.FindControl(prevTxt.Attributes("ClearButton"))

                    btn.Visible = True
                End If
            End If
        End If

    End Sub

    Protected Function CalculateStatus() As ProductionScheduleStatus
        Dim intStatusID As ProductionScheduleStatus = ProductionScheduleStatus.Quote

        If txtReceived.Text <> String.Empty Then
            intStatusID = ProductionScheduleStatus.PaperworkProcessing
        Else
            If txtReceived.Text <> String.Empty Then
                intStatusID = ProductionScheduleStatus.PaperworkProcessing
            ElseIf txtOrderDate.Text <> String.Empty Then
                intStatusID = ProductionScheduleStatus.AwaitingAcceptance
            End If

        End If

        'check measure
        If pnlCheckMeasureDate.Visible Then
            If txtCheckMeasureDate.Text <> String.Empty Then
                intStatusID = ProductionScheduleStatus.CheckMeasure
            Else
                If txtCheckMeasureDate.Text <> String.Empty Then
                    intStatusID = ProductionScheduleStatus.CheckMeasure
                End If
            End If
        End If

        'picking
        If txtPickingDate.Text <> String.Empty Then
            If txtScheduledDate.Text = String.Empty Then
                intStatusID = ProductionScheduleStatus.Picking
            End If
        Else
            If txtPickingDate.Text <> String.Empty Then
                intStatusID = ProductionScheduleStatus.Picking
            Else
                If txtCheckMeasureDate.Text = String.Empty Then
                    If txtReceived.Text <> String.Empty Then
                        intStatusID = ProductionScheduleStatus.PaperworkProcessing
                    ElseIf txtOrderDate.Text <> String.Empty Then
                        intStatusID = ProductionScheduleStatus.AwaitingAcceptance
                    End If
                Else
                    intStatusID = ProductionScheduleStatus.CheckMeasure
                End If
            End If
        End If

        If txtScheduledDate.Text <> String.Empty Then
            If txtCuttingDate.Text = String.Empty Then
                intStatusID = ProductionScheduleStatus.InProduction
            End If
        Else
            If txtScheduledDate.Text <> String.Empty Then
                intStatusID = ProductionScheduleStatus.InProduction
            End If
        End If

        If txtCuttingDate.Text <> String.Empty Then
            If txtPiningDate.Text = String.Empty Then
                intStatusID = ProductionScheduleStatus.InProduction
            End If
        End If

        If txtPiningDate.Text <> String.Empty Then
            If txtPrepDate.Text = String.Empty Then
                intStatusID = ProductionScheduleStatus.InProduction
            End If
        End If

        If txtPrepDate.Text <> String.Empty Then
            If txtAssemblyDate.Text = String.Empty Then
                intStatusID = ProductionScheduleStatus.InProduction
            End If
        End If

        If txtAssemblyDate.Text <> String.Empty Then
            If txtHingingDate.Text = String.Empty Then
                intStatusID = ProductionScheduleStatus.InProduction
            End If
        End If

        If txtHingingDate.Text <> String.Empty Then
            If txtPackupDate.Text = String.Empty Then
                intStatusID = ProductionScheduleStatus.InProduction
            End If
        End If

        If txtPackupDate.Text <> String.Empty Then
            If txtQCDate.Text = String.Empty Then
                intStatusID = ProductionScheduleStatus.InProduction
            End If
        End If

        If txtQCDate.Text <> String.Empty Then
            If txtPostProduction1.Text = String.Empty Then
                intStatusID = ProductionScheduleStatus.InProduction
            End If
        End If

        If lblPostProduction1.Text = "Invoice Date" Then
            If txtPostProduction1.Text <> String.Empty Then
                If txtPostProduction2.Text = String.Empty Then
                    intStatusID = ProductionScheduleStatus.Invoiced
                End If
            End If
        Else
            If txtPostProduction1.Text <> String.Empty Then
                If txtPostProduction2.Text = String.Empty Then
                    intStatusID = ProductionScheduleStatus.Despatch
                End If
            End If
        End If

        If pnlInstallDate.Visible Then
            If txtInstallDate.Text <> String.Empty Then
                intStatusID = ProductionScheduleStatus.Installed
            Else
                If txtInstallDate.Text <> String.Empty Then
                    intStatusID = ProductionScheduleStatus.Installed
                End If
            End If
        End If

        If lblPostProduction2.Text = "Invoice Date" Then
            If txtPostProduction2.Text <> String.Empty Then
                intStatusID = ProductionScheduleStatus.Invoiced
            End If
        Else
            If txtPostProduction2.Text <> String.Empty Then
                intStatusID = ProductionScheduleStatus.Collected
            End If
        End If

        Return intStatusID

    End Function

    Private Sub SetUIForOrderType(enumOrderType As SharedEnums.OrderTypeEnum)

        pnlRemake.Visible = False

        If enumOrderType = SharedEnums.OrderTypeEnum.Remake Then
            pnlRemake.Visible = True
        End If
    End Sub

    Protected Sub cboOrderType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboOrderType.SelectedIndexChanged
        SetUIForOrderType(cboOrderType.SelectedValue)
        Page.Validate("productionschedule")
    End Sub

    Private Function GetDateTextBoxesInOrder(boolExcludePermanentlyLocked As Boolean) As List(Of TextBox)
        Dim lTxtDates As New List(Of TextBox)
        Dim jobType As SharedEnums.JobType

        If IsNumeric(cboJobType.SelectedIndex) Then
            jobType = cboJobType.SelectedIndex
        End If

        ' Load in the date txt controls in status order
        With lTxtDates
            .Add(txtOrderDate)
            .Add(txtReceived)

            If jobType <> SharedEnums.JobType.Wholesale Then
                .Add(txtCheckMeasureDate)
            End If

            .Add(txtPickingDate)
            .Add(txtScheduledDate)
            .Add(txtCuttingDate)
            .Add(txtPiningDate)
            .Add(txtPrepDate)
            .Add(txtAssemblyDate)
            .Add(txtHingingDate)
            .Add(txtPackupDate)
            .Add(txtQCDate)

            If Not boolExcludePermanentlyLocked Then
                .Add(txtPostProduction1)
            End If

            If jobType <> SharedEnums.JobType.Wholesale Then
                .Add(txtInstallDate)
            End If

            If Not boolExcludePermanentlyLocked Then
                .Add(txtPostProduction2)
            End If
        End With

        Return lTxtDates
    End Function

    Private Function SetPreviousStatusDates(cTxt As TextBox) As Boolean
        Dim lTxtDates As List(Of TextBox) = GetDateTextBoxesInOrder(False)
        Dim lTxtModified As New List(Of TextBox)
        Dim boolSuccess As Boolean = True
        Dim boolInRange As Boolean = False

        If String.IsNullOrEmpty(cTxt.Text) Then
            boolSuccess = False
        Else
            ' Loop the controls backwards populating empty dates.
            For i As Integer = lTxtDates.Count - 1 To 0 Step -1

                If boolInRange Then
                    If String.IsNullOrEmpty(lTxtDates(i).Text) Then
                        If lTxtDates(i).Enabled Then
                            lTxtDates(i).Text = cTxt.Text
                            lTxtModified.Add(lTxtDates(i))
                        End If
                    Else
                        ' This date exists so check new date is greater than or equal to this preceding one.
                        boolSuccess = (CDate(cTxt.Text) >= CDate(lTxtDates(i).Text))

                        If Not boolSuccess Then
                            ' Dates are invalid so clear all the new ones.
                            For j As Integer = i + 1 To lTxtDates.Count - 1
                                lTxtDates(j).Text = String.Empty
                            Next j
                        End If

                        ' Stop checking For empty
                        Exit For

                    End If
                Else
                    If lTxtDates(i) Is cTxt Then
                        boolInRange = True
                    End If
                End If
            Next i
        End If

        Return boolSuccess
    End Function

    Protected Sub compareDates(sender As Object, e As EventArgs) Handles txtOrderDate.TextChanged, txtScheduledDate.TextChanged, txtHingingDate.TextChanged,
                              txtCuttingDate.TextChanged, txtPrepDate.TextChanged, txtAssemblyDate.TextChanged, txtQCDate.TextChanged,
                              txtPostProduction1.TextChanged, txtPostProduction2.TextChanged, txtReceived.TextChanged, txtCheckMeasureDate.TextChanged,
                              txtPickingDate.TextChanged, txtInstallDate.TextChanged, txtPiningDate.TextChanged, txtPackupDate.TextChanged, txtQCDate.TextChanged

        Dim txt As TextBox = CType(sender, TextBox)

        If SetPreviousStatusDates(txt) Then
            If txtViewType.Text = ViewType.Update Then
                If Save() Then
                    Response.Redirect("LouvreJobDetails.aspx?" & GetCleanQueryString() & "&Status=1", False)
                End If
            End If
        End If

    End Sub

    Protected Sub CheckPermissions()
        Dim service As New AppService
        Dim cUserPermissions As UserPermissionsContainer = service.Permissions.UserHasPermissions(CInt(Session("sessUserID")))

        'clear date status buttons
        If Not cUserPermissions.HasPermission(Permissions.AddNote) Then
            makeAllClearButtonsHidden()
        End If

        'set date textbox permission disabling

        'daily schedule update
        If Not cUserPermissions.HasPermission(Permissions.TrackOrderButton) Then
            txtCuttingDate.Enabled = False
            txtPrepDate.Enabled = False
            txtAssemblyDate.Enabled = False
            txtQCDate.Enabled = False
        End If

        'schedule date update
        If Not cUserPermissions.HasPermission(Permissions.AddLouvreButton) Then
            txtScheduledDate.Enabled = False
        End If

        'awaiting acceptance
        If Not cUserPermissions.HasPermission(Permissions.CreateLoginButton) Then
            txtReceived.Enabled = False
        End If

        'to be despatched
        If Not cUserPermissions.HasPermission(Permissions.NotDecidedYet) Then
            If lblPostProduction2.Text = "Invoice Date" Then
                txtPostProduction1.Enabled = False
            End If
        End If

        'to be collected
        If Not cUserPermissions.HasPermission(Permissions.CollectFactory) Then
            If lblPostProduction1.Text = "Invoice Date" Then
                txtPostProduction2.Enabled = False
            End If
        End If

        'to be invoiced
        If Not cUserPermissions.HasPermission(Permissions.AwaitingInvoice) Then
            If lblPostProduction2.Text = "Invoice Date" Then
                txtPostProduction2.Enabled = False
            Else
                txtPostProduction1.Enabled = False
            End If
        End If

        _UserHasSalePriceOverridePermission = cUserPermissions.HasPermission(Permissions.ProdScheduleOverridePrices)
        _UserHasCreditCheckOverridePermission = cUserPermissions.HasPermission(Permissions.ProdScheduleCreditCheckOverride)

        pnlCreditCheckOverride.Visible = _UserHasCreditCheckOverridePermission

    End Sub

    Protected Sub makeAllClearButtonsHidden()

        Me.btnClearReceived.Visible = False
        Me.btnClearScheduled.Visible = False

        Me.btnClearCutting.Visible = False
        Me.btnClearPrep.Visible = False
        Me.btnClearAssembly.Visible = False
        Me.btnClearQC.Visible = False

        Me.btnClearPining.Visible = False
        Me.btnClearHinging.Visible = False
        Me.btnClearPackup.Visible = False

        Me.btnClearPostProduction1.Visible = False
        Me.btnClearPostProduction2.Visible = False

    End Sub

    Protected Sub makeAllStatusActionButtonsHidden()

        Me.btnConfirmOutstanding.Visible = False
        Me.btnCreditApprove.Visible = False

        Me.btnAcceptOrder.Visible = False
        Me.btnPickingList.Visible = False
        Me.btnPackUp.Visible = False
        Me.btnDispatchDate.Visible = False

    End Sub

    Protected Sub checkAllDatesValid(sender As Object)

        Dim txt As TextBox = CType(sender, TextBox)

        Select Case txt.ID
            Case "txtReceived"
                If txt.Text = String.Empty Then
                    Me.txtScheduledDate.Text = String.Empty
                    Me.txtCuttingDate.Text = String.Empty
                    Me.txtPiningDate.Text = String.Empty
                    Me.txtPrepDate.Text = String.Empty
                    Me.txtAssemblyDate.Text = String.Empty
                    Me.txtHingingDate.Text = String.Empty
                    Me.txtPackupDate.Text = String.Empty
                    Me.txtQCDate.Text = String.Empty
                    Me.txtPostProduction1.Text = String.Empty
                    Me.txtPostProduction2.Text = String.Empty
                Else
                    If Me.txtScheduledDate.Text <> String.Empty Then
                        If DateDiff(DateInterval.Day, CDate(Me.txtScheduledDate.Text), CDate(txt.Text)) > 0 Then
                            txt.Text = String.Empty
                            Me.txtScheduledDate.Text = String.Empty
                            Me.txtPiningDate.Text = String.Empty
                            Me.txtPrepDate.Text = String.Empty
                            Me.txtAssemblyDate.Text = String.Empty
                            Me.txtHingingDate.Text = String.Empty
                            Me.txtPackupDate.Text = String.Empty
                            Me.txtQCDate.Text = String.Empty
                            Me.txtPostProduction1.Text = String.Empty
                            Me.txtPostProduction2.Text = String.Empty
                        End If
                    End If
                End If

            Case "txtScheduledDate"
                If txt.Text = String.Empty Then
                    Me.txtCuttingDate.Text = String.Empty
                    Me.txtPiningDate.Text = String.Empty
                    Me.txtPrepDate.Text = String.Empty
                    Me.txtAssemblyDate.Text = String.Empty
                    Me.txtHingingDate.Text = String.Empty
                    Me.txtPackupDate.Text = String.Empty
                    Me.txtQCDate.Text = String.Empty
                    Me.txtPostProduction1.Text = String.Empty
                    Me.txtPostProduction2.Text = String.Empty
                Else
                    If Me.txtCuttingDate.Text <> String.Empty Then
                        If DateDiff(DateInterval.Day, CDate(Me.txtCuttingDate.Text), CDate(txt.Text)) > 0 Then
                            txt.Text = String.Empty
                            Me.txtCuttingDate.Text = String.Empty
                            Me.txtPiningDate.Text = String.Empty
                            Me.txtPrepDate.Text = String.Empty
                            Me.txtAssemblyDate.Text = String.Empty
                            Me.txtHingingDate.Text = String.Empty
                            Me.txtPackupDate.Text = String.Empty
                            Me.txtQCDate.Text = String.Empty
                            Me.txtPostProduction1.Text = String.Empty
                            Me.txtPostProduction2.Text = String.Empty
                        End If
                    End If
                End If

            Case "txtCuttingDate"
                If txt.Text = String.Empty Then
                    Me.txtPiningDate.Text = String.Empty
                    Me.txtAssemblyDate.Text = String.Empty
                    Me.txtHingingDate.Text = String.Empty
                    Me.txtPackupDate.Text = String.Empty
                    Me.txtQCDate.Text = String.Empty
                    Me.txtPostProduction1.Text = String.Empty
                    Me.txtPostProduction2.Text = String.Empty
                Else
                    If Me.txtPrepDate.Text <> String.Empty Then
                        If DateDiff(DateInterval.Day, CDate(Me.txtPrepDate.Text), CDate(txt.Text)) > 0 Then
                            txt.Text = String.Empty
                            Me.txtPiningDate.Text = String.Empty
                            Me.txtPrepDate.Text = String.Empty
                            Me.txtAssemblyDate.Text = String.Empty
                            Me.txtHingingDate.Text = String.Empty
                            Me.txtPackupDate.Text = String.Empty
                            Me.txtQCDate.Text = String.Empty
                            Me.txtPostProduction1.Text = String.Empty
                            Me.txtPostProduction2.Text = String.Empty
                        End If
                    End If
                End If

            Case "txtPiningDate"
                If txt.Text = String.Empty Then
                    Me.txtPrepDate.Text = String.Empty
                    Me.txtAssemblyDate.Text = String.Empty
                    Me.txtHingingDate.Text = String.Empty
                    Me.txtPackupDate.Text = String.Empty
                    Me.txtQCDate.Text = String.Empty
                    Me.txtPostProduction1.Text = String.Empty
                    Me.txtPostProduction2.Text = String.Empty
                Else
                    If Me.txtPrepDate.Text <> String.Empty Then
                        If DateDiff(DateInterval.Day, CDate(Me.txtPrepDate.Text), CDate(txt.Text)) > 0 Then
                            txt.Text = String.Empty
                            Me.txtPrepDate.Text = String.Empty
                            Me.txtAssemblyDate.Text = String.Empty
                            Me.txtHingingDate.Text = String.Empty
                            Me.txtPackupDate.Text = String.Empty
                            Me.txtQCDate.Text = String.Empty
                            Me.txtPostProduction1.Text = String.Empty
                            Me.txtPostProduction2.Text = String.Empty
                        End If
                    End If
                End If

            Case "txtPrepDate"
                If txt.Text = String.Empty Then
                    Me.txtAssemblyDate.Text = String.Empty
                    Me.txtHingingDate.Text = String.Empty
                    Me.txtPackupDate.Text = String.Empty
                    Me.txtQCDate.Text = String.Empty
                    Me.txtPostProduction1.Text = String.Empty
                    Me.txtPostProduction2.Text = String.Empty
                Else
                    If Me.txtAssemblyDate.Text <> String.Empty Then
                        If DateDiff(DateInterval.Day, CDate(Me.txtAssemblyDate.Text), CDate(txt.Text)) > 0 Then
                            txt.Text = String.Empty
                            Me.txtAssemblyDate.Text = String.Empty
                            Me.txtHingingDate.Text = String.Empty
                            Me.txtPackupDate.Text = String.Empty
                            Me.txtQCDate.Text = String.Empty
                            Me.txtPostProduction1.Text = String.Empty
                            Me.txtPostProduction2.Text = String.Empty
                        End If
                    End If
                End If

            Case "txtAssemblyDate"
                If txt.Text = String.Empty Then
                    Me.txtHingingDate.Text = String.Empty
                    Me.txtPackupDate.Text = String.Empty
                    Me.txtQCDate.Text = String.Empty
                    Me.txtPostProduction1.Text = String.Empty
                    Me.txtPostProduction2.Text = String.Empty
                Else
                    If Me.txtQCDate.Text <> String.Empty Then
                        If DateDiff(DateInterval.Day, CDate(Me.txtQCDate.Text), CDate(txt.Text)) > 0 Then
                            txt.Text = String.Empty
                            Me.txtHingingDate.Text = String.Empty
                            Me.txtPackupDate.Text = String.Empty
                            Me.txtQCDate.Text = String.Empty
                            Me.txtPostProduction1.Text = String.Empty
                            Me.txtPostProduction2.Text = String.Empty
                        End If
                    End If
                End If

            Case "txtHingingDate"
                If txt.Text = String.Empty Then
                    Me.txtPackupDate.Text = String.Empty
                    Me.txtQCDate.Text = String.Empty
                    Me.txtPostProduction1.Text = String.Empty
                    Me.txtPostProduction2.Text = String.Empty
                Else
                    If Me.txtQCDate.Text <> String.Empty Then
                        If DateDiff(DateInterval.Day, CDate(Me.txtQCDate.Text), CDate(txt.Text)) > 0 Then
                            txt.Text = String.Empty
                            Me.txtPackupDate.Text = String.Empty
                            Me.txtQCDate.Text = String.Empty
                            Me.txtPostProduction1.Text = String.Empty
                            Me.txtPostProduction2.Text = String.Empty
                        End If
                    End If
                End If

            Case "txtPackupDate"
                If txt.Text = String.Empty Then
                    Me.txtQCDate.Text = String.Empty
                    Me.txtPostProduction1.Text = String.Empty
                    Me.txtPostProduction2.Text = String.Empty
                Else
                    If Me.txtQCDate.Text <> String.Empty Then
                        If DateDiff(DateInterval.Day, CDate(Me.txtQCDate.Text), CDate(txt.Text)) > 0 Then
                            txt.Text = String.Empty
                            Me.txtQCDate.Text = String.Empty
                            Me.txtPostProduction1.Text = String.Empty
                            Me.txtPostProduction2.Text = String.Empty
                        End If
                    End If
                End If

            Case "txtQCDate"
                If txt.Text = String.Empty Then
                    Me.txtPostProduction1.Text = String.Empty
                    Me.txtPostProduction2.Text = String.Empty
                Else
                    If Me.txtPostProduction1.Text <> String.Empty Then
                        If DateDiff(DateInterval.Day, CDate(Me.txtPostProduction1.Text), CDate(txt.Text)) > 0 Then
                            txt.Text = String.Empty
                            Me.txtPostProduction1.Text = String.Empty
                            Me.txtPostProduction2.Text = String.Empty
                        End If
                    End If
                End If

            Case "txtPostProduction1"
                If txt.Text = String.Empty Then
                    Me.txtPostProduction2.Text = String.Empty
                Else
                    If Me.txtPostProduction2.Text <> String.Empty Then
                        If DateDiff(DateInterval.Day, CDate(Me.txtPostProduction2.Text), CDate(txt.Text)) > 0 Then
                            txt.Text = String.Empty
                            Me.txtPostProduction2.Text = String.Empty
                        End If
                    End If
                End If

            Case "txtPostProduction2"

            Case Else

        End Select

        txt = Nothing

    End Sub

    Private Sub LoadFilesToProductionScheduleList()
        Dim lFiles As List(Of ProductionScheduleFile) = Session("ProdScheduleFiles")

        gdvProdScheduleFiles.DataSource = lFiles
        gdvProdScheduleFiles.DataBind()

        ToggleDisabledGridViewRowButtons(gdvProdScheduleFiles, New List(Of String) From {"lnkProdScheduleFileName"}, (Not pnlUploadFiles.Visible))
        ToggleDisabledGridViewRowButtons(gdvProdScheduleFiles, New List(Of String) From {"btnProdScheduleDelete"}, (Not pnlUploadFiles.Visible))
    End Sub

    Protected Sub LoadNotesToAccordian()
        Dim lNotes As List(Of ProdScheduleNote) = ViewState("Notes")
        'set text of notes button to show number of notes
        btnViewNotes.Text = "Notes (" & lNotes.Count & ")"

        acc1.DataSource = lNotes
        acc1.DataBind()
    End Sub

    Protected Function GetNoteHeaderText(cNote As ProdScheduleNote) As String

        Dim strText As String = String.Empty
        Dim cUser As User = _Users.Find(Function(x) x.ID = cNote.UserID)

        If cUser IsNot Nothing Then
            strText = "Entered On " & Format(cNote.EntryDate, "d MMM yyyy h:mm tt") & " by " & cUser.UserFirstName & " " & cUser.UserLastName
        Else
            strText = "Entered on " & Format(cNote.EntryDate, "d MMM yyyy h:mm tt")
        End If

        Return strText
    End Function

    Private Sub btnCancelNote_Click(sender As Object, e As EventArgs) Handles btnCancelNote.Click

        btnAddNewNote.Visible = True
        btnCloseNotes.Visible = True
        txtNewNoteText.Text = String.Empty
        chkNoteVisible.Checked = False
        pnlAddNote.Visible = False

    End Sub

    Private Sub btnAddNewNote_Click(sender As Object, e As EventArgs) Handles btnAddNewNote.Click

        btnAddNewNote.Visible = False
        btnCloseNotes.Visible = False
        txtNewNoteText.Text = String.Empty
        chkNoteVisible.Checked = False
        pnlAddNote.Visible = True

    End Sub

    Private Sub btnSaveNote_Click(sender As Object, e As EventArgs) Handles btnSaveNote.Click

        If Not String.IsNullOrWhiteSpace(txtNewNoteText.Text.Trim) Then
            Dim cNote As New ProdScheduleNote

            cNote.ProdScheduleID = SharedConstants.DEFAULT_INTEGER_VALUE
            cNote.NoteDetails = txtNewNoteText.Text.Trim
            cNote.NoteTypeID = 1
            cNote.UserID = CInt(Session("sessUserID"))
            cNote.EntryDate = DateTime.Now
            cNote.VisibleToCustomer = chkNoteVisible.Checked

            ' Add note to viewstate.
            Dim lNotes As List(Of ProdScheduleNote) = ViewState("Notes")

            lNotes.Add(cNote)

            txtNewNoteText.Text = String.Empty
            chkNoteVisible.Checked = False
            pnlAddNote.Visible = False
            btnAddNewNote.Visible = True
            btnCloseNotes.Visible = True

            LoadNotesToAccordian()
        End If
    End Sub

    Private Sub btnOrderForm_Click(sender As Object, e As EventArgs) Handles btnOrderForm.Click

        'Dim strRptsPath As String = Server.MapPath("") & "\ExcelRpts"
        'Dim objBuffer As Byte() = Nothing
        ''ExcelReport.createReport(strRptsPath, objBuffer, dtScheduleList)
        'Dim rptFileName As String = PlantationOrderForm.generatePlantationOrderForm(strRptsPath, objBuffer, CInt(Me.txtId.Text), 1)
        'Dim strRPTFileName As String = IO.Path.Combine(strRptsPath, rptFileName)
        'If objBuffer IsNot Nothing Then
        '    '----
        '    Response.BinaryWrite(objBuffer)
        '    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        '    Response.AddHeader("content-disposition", "attachment;  filename=" & rptFileName)
        '    Response.End()
        '    '
        'End If
        'objBuffer = Nothing

    End Sub

    Protected Sub loadOrderFormLinks()

        'check if order form should be diplayed - records for job in ozrolls details table
        Dim dtDetails As DataTable = _Service.GetPlantationJobDetailsRecordsByPlantationScheduleID(CInt(Me.txtId.Text))
        If dtDetails.Rows.Count > 0 Then

            'get details to search modern tables from
            Dim dtOrder As DataTable = _Service.RunSQLScheduling("select dbo.tblProductionScheduleList.*, dbo.tblCustomers.SiteID from dbo.tblProductionScheduleList inner join dbo.tblCustomers on dbo.tblProductionScheduleList.CustomeriD = dbo.tblCustomers.CustomeriD where dbo.tblProductionScheduleList.ID = " & Me.txtId.Text)
            If dtOrder.Rows.Count > 0 Then

                If Not IsDBNull(dtOrder.Rows(0)("SiteID")) And Not IsDBNull(dtOrder.Rows(0)("JobNumber")) Then

                    Dim strSQL As String = "select * from dbo.tblEOrderSubmission where [Type] = 1 and JobNumber = " & dtOrder.Rows(0)("JobNumber").ToString
                    Dim dtOrderForms As DataTable = _Service.RunSQLHive(CInt(dtOrder.Rows(0)("SiteID")), strSQL)

                    If dtOrderForms.Rows.Count > 0 Then
                        For i As Integer = 0 To dtOrderForms.Rows.Count - 1
                            '\\HIVEAUTO11\c$\My Documents\Plantation Shutters Order\Plantation Shutters Order PIL532525 for Modern NSW (17 May 2017) - 001.xls                            
                            Dim strFileName As String = Right(dtOrderForms.Rows(i)("FileName").ToString, Len(dtOrderForms.Rows(i)("FileName").ToString) - 55)
                            strFileName = Left(strFileName, Len(strFileName) - 3) & "pdf"
                            If IO.File.Exists(Server.MapPath("~") & "orderforms\" & strFileName) Then
                                Dim strOrderForm As String = "orderforms/" & strFileName
                                Me.lblOrderForm.Text &= "<a target=""_blank"" href=""" & ResolveUrl(strOrderForm) & """>Display Order Form</a><br />"
                                Me.pnlOrderForms.Visible = True
                            End If
                        Next
                    Else
                        Me.pnlOrderForms.Visible = False
                    End If
                    dtOrderForms.Dispose()
                    dtOrderForms = Nothing
                Else
                    Me.pnlOrderForms.Visible = False
                End If

            Else
                Me.pnlOrderForms.Visible = False
            End If
            dtOrder.Dispose()
            dtOrder = Nothing
        Else
            Me.pnlOrderForms.Visible = False
        End If

        dtDetails.Dispose()
        dtDetails = Nothing

    End Sub


#Region "Shutter Details Function"

    Protected Sub dgvDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvDetails.RowCommand

        If (e.CommandName = "LouvreDetail") Then
            Dim cLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")
            Dim id As Integer = Convert.ToInt32(e.CommandArgument)

            Dim details As LouvreDetails = cLouvreDetails.Find(Function(x) x.LouvreDetailID = id)

            If details.ShutterTypeId = LouvreTypes.PrivacyScreen Then
                ucPrivacyScreenDetails.ShowEditPrivacyScreen(cLouvreDetails, id)
            Else
                LoadShutterDetailsToPopup(id)
                TempBackupExtrasForLouvreDetail(id)

                txtHiddenPSDetailID.Text = id
                ModalPopupExtender.Show()

                LoadShutterDetailsForDataGrid()
            End If


        ElseIf (e.CommandName = "DeleteDetail") Then
            Dim id As Integer = Convert.ToInt32(e.CommandArgument)
            Dim cLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")
            Dim dicExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")

            ' Remove details and extras.
            cLouvreDetails.RemoveAll(Function(x) x.LouvreDetailID = id)
            dicExtras.Remove(id)

            Dim lDeletedDetailIDs As HashSet(Of Integer) = ViewState("DeletedDetailIDs")

            lDeletedDetailIDs.Add(id)

            RecalculateUIFreightFromDB()

            LoadShutterDetailsForDataGrid()

        ElseIf (e.CommandName = "DetailDuplicate") Then
            Dim id As Integer = Convert.ToInt32(e.CommandArgument)
            Dim cLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")
            Dim newDetails As LouvreDetails

            ' Clone the details
            newDetails = cLouvreDetails.Find(Function(x) x.LouvreDetailID = id).Clone
            newDetails = cLouvreDetails.Add(newDetails, True)

            ' Set shutter ID to first available positive number.
            Dim lastID As Integer = 0

            Dim sortedDetails As List(Of LouvreDetails) = cLouvreDetails.OrderBy(Function(x) x.ShutterID).ToList()

            For Each detail As LouvreDetails In sortedDetails
                If lastID <> 0 Then
                    If (detail.ShutterID - lastID) > 1 Then
                        Exit For
                    End If
                End If

                lastID = detail.ShutterID
            Next detail

            newDetails.ShutterID = lastID + 1

            ' Clone the extras
            Dim dicExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")
            Dim lNewExtras As New List(Of LouvreExtraProduct)
            Dim lExtras As List(Of LouvreExtraProduct) = Nothing

            dicExtras.TryGetValue(id, lExtras)

            If lExtras IsNot Nothing Then
                For Each extra As LouvreExtraProduct In lExtras
                    Dim newExtra As New LouvreExtraProduct

                    ' Clone and assign unique negative ID.
                    newExtra = extra.Clone()
                    newExtra.LouvreDetailsID = newDetails.LouvreDetailID
                    newExtra.ProductionScheduleID = newDetails.ProductionScheduleID

                    If lNewExtras.Any Then
                        newExtra.ID = lNewExtras.Min(Function(x) x.ID) - 1

                        If newExtra.ID >= 0 Then
                            newExtra.ID = -1
                        End If
                    Else
                        newExtra.ID = -1
                    End If

                    lNewExtras.Add(newExtra)
                Next extra

                dicExtras.Add(newDetails.LouvreDetailID, lNewExtras)
            End If

            RecalculateUIFreightFromDB()

            LoadShutterDetailsForDataGrid()

        ElseIf (e.CommandName = "SalePriceOverrideRemove") Then
            Dim cLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")
            Dim cDetails = cLouvreDetails.Find(Function(x) x.LouvreDetailID = e.CommandArgument)
            Dim dicExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")
            Dim cExtras As List(Of LouvreExtraProduct) = dicExtras(cDetails.LouvreDetailID)

            cDetails.LouvrePriceID = 0
            cDetails.SalePrice = 0

            ' Recost this detail.
            RecalculateLouvreDetailsCostFromDBMatrix(cDetails, cExtras)

            RecalculateUIFreightFromDB()

            LoadShutterDetailsForDataGrid()

        End If

    End Sub

    Private Sub TempBackupExtrasForLouvreDetail(aLouvreDetailID As Integer)

        Dim lDetailIDToExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")
        Dim lExtras As List(Of LouvreExtraProduct) = lDetailIDToExtras(aLouvreDetailID)
        Dim lExtrasCopy As New List(Of LouvreExtraProduct)

        ' Create a copy of the louvre extra product and backup in viewstate.
        For Each e As LouvreExtraProduct In lExtras
            lExtrasCopy.Add(e.Clone)
        Next e

        ViewState("TempExtrasBackup") = lExtrasCopy
    End Sub

    Private Sub RestoreTempBackupExtrasForLouvreDetail(aLouvreDetailID As Integer)
        Dim lDetailIDToExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")

        lDetailIDToExtras(txtHiddenPSDetailID.Text) = ViewState("TempExtrasBackup")
    End Sub

    Private Sub UpdateUIData()
        UpdateSalePriceUI()
        UpdateTotalPanelsUI()
        UpdateTotalSQMUI()
    End Sub

    Protected Sub LoadShutterDetailsForDataGrid()
        Dim cLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")

        dgvDetails.DataSource = cLouvreDetails
        dgvDetails.DataBind()

        If cLouvreDetails.Count > 0 Then
            pnlOpeningDetails.Visible = True
        End If

        UpdateUIData()

    End Sub

    Protected Sub LoadProductionScheduleExtrasIntoDataGrid()
        Dim lExtras As List(Of LouvreExtraProduct) = ViewState("ProductionScheduleExtras")

        gdvExtrasProdSchedule.DataSource = lExtras
        gdvExtrasProdSchedule.DataBind()

        If lExtras.Count > 0 Then
            pnlExtrasProdSchedule.Visible = True
        End If

        UpdateUIData()

    End Sub

    Protected Sub LoadAdditionalRequirementsForDataGrid()
        Dim cAdditionalRequirements As List(Of AdditionalRequirements) = ViewState("Requirements")
        Dim cPowderCoating As List(Of AdditionalRequirements) = ViewState("PowderCoating")

        dgvAdditionalRequirements.DataSource = cAdditionalRequirements
        dgvAdditionalRequirements.DataBind()

        dgvPowdercoat.DataSource = cPowderCoating
        dgvPowdercoat.DataBind()
    End Sub

    ''' <summary>
    ''' Checks if the validation controls in the given validation group name are all valid.
    ''' </summary>
    ''' <param name="strValidationGroup">The name of the validation group.</param>
    ''' <returns>True is all controls are valid, otherwise false.</returns>
    Protected Function IsValidationGroupValid(strValidationGroup As String) As Boolean

        Dim valCollection As ValidatorCollection = Page.GetValidators(strValidationGroup)

        For Each v As BaseValidator In valCollection
            Dim fValid As Boolean = v.IsValid

            If fValid Then
                v.Validate()
                fValid = v.IsValid
            End If

            If Not fValid Then
                Return False
            End If
        Next v

        Return True
    End Function

    ''' <summary>
    ''' Checks the given original louvre details against the current UI state for relevant changes.
    ''' </summary>
    ''' <param name="cOriginalLouvreDetails">The original state of the louvre details to compare against.</param>
    ''' <returns>True if a change is found, otherwise false.</returns>
    Private Function DetailsNeedCostRecalculation(cOriginalLouvreDetails As LouvreDetails, lOriginalExtras As List(Of LouvreExtraProduct)) As Boolean

        ' CHECKING FOR CHANGES TO:
        ' * Colour
        ' * Louvre Style
        ' * Make / Opening
        ' * Width
        ' * Height
        ' * Curved Track
        ' * Insert Top
        ' * Insert Bottom
        ' * Blade Size
        ' * Blade Lock
        ' * Extra Track
        ' * Bottom Track
        ' * Flush Bolts Top
        ' * Flush Bolts Bottom
        ' * Fly Screen
        ' * Lock Options
        ' * Changes to extras

        If Not cOriginalLouvreDetails.LouvrePriceIsOverridden Then
            If cOriginalLouvreDetails IsNot Nothing Then
                Dim dicExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")
                Dim cExtras As List(Of LouvreExtraProduct) = dicExtras(cOriginalLouvreDetails.LouvreDetailID)

                ' Colour change
                If IsNumeric(hdnColourID.Value) AndAlso hdnColourID.Value > 0 Then
                    ' Only recalculate the price if the colour has changed grade (std to premium, etc).
                    Dim colours As List(Of Colour) = _Service.getColours().FindAll(Function(x) x.Discontinued = False)

                    ' get original value
                    Dim newColour As Colour = colours.Find(Function(x) x.ID = hdnColourID.Value)
                    Dim oldColour As Colour = colours.Find(Function(x) x.ID = cOriginalLouvreDetails.ColourId)

                    If newColour IsNot Nothing AndAlso oldColour IsNot Nothing Then
                        If newColour.CoatingTypeID <> oldColour.CoatingTypeID Then
                            Return True
                        End If
                    Else
                        Return True
                    End If
                Else
                    ' No value so recalculate
                    If cOriginalLouvreDetails.ColourId > 0 Then
                        Return True
                    End If
                End If

                ' Louvre Style change
                If cboLouvreProd.SelectedValue <> cOriginalLouvreDetails.ProductId Then
                    Return True
                End If

                ' Make / Opening change
                If cboMakeOpenSize.SelectedValue <> cOriginalLouvreDetails.MakeOrOpenSizesId Then
                    Return True
                End If

                ' Width
                If txtWidth.Text <> cOriginalLouvreDetails.Width Then
                    Return True
                End If

                ' Height
                If txtHeight.Text <> cOriginalLouvreDetails.Height Then
                    Return True
                End If

                ' Curved Track
                If cboCurvedTrack.SelectedValue <> cOriginalLouvreDetails.CurvedTrack Then
                    Return True
                End If

                ' Insert Top
                If cboInsertTop.SelectedValue <> cOriginalLouvreDetails.InsertTopId Then
                    Return True
                End If

                ' Insert Bottom
                If cboInsertBottom.SelectedValue <> cOriginalLouvreDetails.InsertBottomId Then
                    Return True
                End If

                ' Blade Size
                If cboBladeSize.SelectedValue <> cOriginalLouvreDetails.BladeSizeId Then
                    Return True
                End If

                ' Blade Lock
                If cboBladeLocks.SelectedValue <> cOriginalLouvreDetails.BladeLocks Then
                    Return True
                End If

                ' Extra Track
                If IIf(IsNumeric(txtExtraTrack.Text), txtExtraTrack.Text, 0) <> cOriginalLouvreDetails.ExtraTrack Then
                    Return True
                End If

                ' Bottom Track
                If cboBottomTrackType.SelectedValue <> cOriginalLouvreDetails.BottomTrackTypeId Then
                    Return True
                End If

                ' Flush Bolts Top
                If cboFlushBoltsTop.SelectedValue <> cOriginalLouvreDetails.FlushBoltsTopId Then
                    Return True
                End If

                ' Flush Bolts Bottom
                If cboFlushBoltsBottom.SelectedValue <> cOriginalLouvreDetails.FlushBoltsBottomId Then
                    Return True
                End If

                ' Fly Screen
                If cboFlyScreen.SelectedValue <> cOriginalLouvreDetails.FlyScreen Then
                    Return True
                End If

                ' Lock Options
                If cboLockOptions.SelectedValue <> cOriginalLouvreDetails.LockOptionsId Then
                    Return True
                End If

                If lOriginalExtras IsNot Nothing Then
                    ' Extras
                    If lOriginalExtras.Count <> cExtras.Count Then
                        ' Something has been added/removed.
                        Return True
                    End If

                    For i As Integer = 0 To lOriginalExtras.Count - 1
                        If lOriginalExtras(i).CutLength <> cExtras(i).CutLength Then
                            Return True
                        End If

                        If lOriginalExtras(i).ExtraProductID <> cExtras(i).ExtraProductID Then
                            Return True
                        End If

                        If lOriginalExtras(i).Quantity <> cExtras(i).Quantity Then
                            Return True
                        End If
                    Next i
                End If

            Else
                ' No details means new details so recalculate.
                Return True
            End If
        End If

        Return False
    End Function

    Protected Sub btnSaveDetails_Click(sender As Object, e As System.EventArgs) Handles btnSaveDetails.Click
        If IsValidationGroupValid("details") Then
            If Not SaveDetails() Then
                ' Keep the popup
                ModalPopupExtender.Show()
            End If
        Else
            ' Keep the popup
            ModalPopupExtender.Show()
        End If
    End Sub

    Private Function SaveDetails() As Boolean
        Dim dicExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")
        Dim cLouvreDetailsCollection As LouvreDetailsCollection = ViewState("LouvreDetails")
        Dim cLouvreJobDetails As LouvreDetails = cLouvreDetailsCollection.Find(Function(x) x.LouvreDetailID = txtHiddenPSDetailID.Text)

        If cLouvreJobDetails IsNot Nothing Then
            Dim cExtras As List(Of LouvreExtraProduct) = Nothing

            ' Extras may be nothing as privacy screen don't support them at the detail level.
            dicExtras.TryGetValue(cLouvreJobDetails.LouvreDetailID, cExtras)

            ' Remember the original settings for change detection.
            Dim cOriginalLouvreJobDetails As LouvreDetails = cLouvreJobDetails.Clone

            If cLouvreJobDetails.ShutterTypeId = LouvreTypes.PrivacyScreen Then
                cLouvreJobDetails = ucPrivacyScreenDetails.PopulateLouvreDetailsFromUI(cLouvreJobDetails)
            Else
                cLouvreJobDetails = PopulateLouvreDetailsFromUI(cLouvreJobDetails)
            End If

            If cLouvreJobDetails.ShutterID <= 0 Then
                ' Set shutter ID to first available positive number.
                Dim lastID As Integer = 0

                Dim sortedDetails As List(Of LouvreDetails) = cLouvreDetailsCollection.OrderBy(Function(x) x.ShutterID).ToList()

                For Each detail As LouvreDetails In sortedDetails

                    If lastID <> 0 Then
                        If (detail.ShutterID - lastID) > 1 Then
                            Exit For
                        End If
                    End If

                    lastID = Math.Max(0, detail.ShutterID)
                Next detail

                cLouvreJobDetails.ShutterID = lastID + 1
            End If

            Dim cPriceFound As LouvrePrice = Nothing
            Dim lExtraPriceIDsNotFound As New List(Of ExtraProductLouvres)

            ' Get new cost from DB matrix.
            If cLouvreJobDetails.ShutterTypeId = LouvreTypes.PrivacyScreen Then
                If ucPrivacyScreenDetails.DetailsNeedCostRecalculation(cOriginalLouvreJobDetails) Then
                    RecalculateLouvreDetailsCostFromDBMatrix(cLouvreJobDetails, cExtras)
                End If
            Else
                If DetailsNeedCostRecalculation(cOriginalLouvreJobDetails, ViewState("TempExtrasBackup")) Then
                    RecalculateLouvreDetailsCostFromDBMatrix(cLouvreJobDetails, cExtras)
                End If
            End If

            ' Added By Pradeep on 02 Dec22 against ticket #72880.
            If cLouvreJobDetails.LouvrePriceID = 0 Then
                Return False
            End If

            ' Don't need to save extras as they are saved/updated to viewstate on the fly. They are reverted on cancel if needed.

            ResetPopupControlsToDefault()
            txtPSDetailID.Text = "0"
            LoadShutterDetailsForDataGrid()

            ViewState("NewLouvreDetail") = False
        Else
            ' The EndRequestHandler won't fire so have to manually hide the loading hider over the popup.
            ScriptManager.RegisterStartupScript(Page, GetType(String), "HideHider", "HideHider();", True)

            Return False
        End If

        Return True
    End Function

    Private Sub UpdateTotalPanelsUI()
        Dim cLouvreDetailsCollection As LouvreDetailsCollection = ViewState("LouvreDetails")

        txtTotalPanels.Text = cLouvreDetailsCollection.TotalNoOfPanels
    End Sub

    Private Sub UpdateTotalSQMUI()
        Dim cLouvreDetailsCollection As LouvreDetailsCollection = ViewState("LouvreDetails")

        txtTotalSQM.Text = cLouvreDetailsCollection.TotalSQM
    End Sub

    Protected Sub btnCancelDetails_Click(sender As Object, e As System.EventArgs) Handles btnCancelDetails.Click
        Dim cLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")
        Dim dicExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")

        If ViewState("NewLouvreDetail") Then
            ' We were adding a new louvre detail so delete it and its extra dictionary entry.
            cLouvreDetails.RemoveAll(Function(x) x.LouvreDetailID = txtHiddenPSDetailID.Text)
            dicExtras.Remove(txtHiddenPSDetailID.Text)
        Else
            ' We were editing an existing louvre detail record so restore the extras to original state.
            RestoreTempBackupExtrasForLouvreDetail(txtHiddenPSDetailID.Text)
        End If

        ResetPopupControlsToDefault()
        txtPSDetailID.Text = "0"

        ViewState("NewLouvreDetail") = False
    End Sub

    Protected Function LoadShutterDetailsToPopup(intPSDetailID As Integer) As LouvreDetails
        Dim cLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")
        Dim cQuoteDetail As LouvreDetails = cLouvreDetails.Find(Function(x) x.LouvreDetailID = intPSDetailID)

        If cQuoteDetail.LouvreDetailID <> 0 Then

            txtHiddenPSDetailID.Text = intPSDetailID.ToString

            cboRemakeType.SelectedValue = cQuoteDetail.RemakeTypeID
            txtLocation.Text = cQuoteDetail.Location

            If cQuoteDetail.Width > 0 Then
                txtWidth.Text = cQuoteDetail.Width
            Else
                txtWidth.Text = String.Empty
            End If

            If cQuoteDetail.Height > 0 Then
                txtHeight.Text = cQuoteDetail.Height
            Else
                txtHeight.Text = String.Empty
            End If

            If cQuoteDetail.MidRailHeight > 0 Then
                txtMidRailHeight.Text = cQuoteDetail.MidRailHeight
            Else
                txtMidRailHeight.Text = String.Empty
            End If

            cboInsertTop.SelectedValue = cQuoteDetail.InsertTopId
            cboBladeOperation.SelectedValue = cQuoteDetail.BladeOperationId
            cboInsertBottom.SelectedValue = cQuoteDetail.InsertBottomId
            cboBladeOperationBottom.SelectedValue = cQuoteDetail.BladeOperationBottomId
            cboLouvreProd.SelectedValue = cQuoteDetail.ProductId

            PopulateShutterTypeDDL(0)

            cboLouvreType.SelectedValue = cQuoteDetail.ShutterTypeId
            cboSlide.SelectedValue = cQuoteDetail.SlideId
            cboBiFoldHingedDoor.SelectedValue = cQuoteDetail.BiFoldHingedDoorInOutId

            If cQuoteDetail.BladeSizeId > 0 Then
                cboBladeSize.SelectedValue = cQuoteDetail.BladeSizeId
            End If

            cboFlushBoltsTop.SelectedValue = cQuoteDetail.FlushBoltsTopId
            cboFlushBoltsPosition.SelectedValue = cQuoteDetail.FlushBoltPosition
            cboFlushBoltsBottom.SelectedValue = cQuoteDetail.FlushBoltsBottomId
            cboCChannel.SelectedValue = cQuoteDetail.CChannel
            cboHChannel.SelectedValue = cQuoteDetail.HChannel
            cboLReveal.SelectedValue = cQuoteDetail.LChannel
            cboZReveal.SelectedValue = cQuoteDetail.ZChannel
            txtSpecialRequirements.Text = cQuoteDetail.SpecialRequirements
            txtLocation.Text = cQuoteDetail.Location
            cboMakeOpenSize.SelectedValue = cQuoteDetail.MakeOrOpenSizesId
            cboEndCapColour.SelectedValue = cQuoteDetail.EndCapColourId
            cboBladeClipColour.SelectedValue = cQuoteDetail.BladeClipColourId
            cboPileColour.SelectedValue = cQuoteDetail.PileColourId
            cboLockOptions.SelectedValue = cQuoteDetail.LockOptionsId
            cboBottomTrackType.SelectedValue = cQuoteDetail.BottomTrackTypeId
            cboBladeLocks.SelectedValue = cQuoteDetail.BladeLocks

            If cQuoteDetail.ColourId > 0 Then
                hdnColourID.Value = cQuoteDetail.ColourId
                hdnColourName.Value = cQuoteDetail.Colour
                txtColour.Text = cQuoteDetail.Colour
                txtColour.ToolTip = txtColour.Text
            Else
                hdnColourID.Value = 0
                hdnColourName.Value = String.Empty
                txtColour.Text = String.Empty
                txtColour.ToolTip = txtColour.Text
            End If

            cboStacker.SelectedValue = cQuoteDetail.StackerLocationId
            cboCurvedTrack.SelectedValue = cQuoteDetail.CurvedTrack

            If cQuoteDetail.CurvedTrackMaxDeflection > 0 Then
                txtCurvedTrackMaxDeflection.Text = cQuoteDetail.CurvedTrackMaxDeflection
            Else
                txtCurvedTrackMaxDeflection.Text = String.Empty
            End If

            If cQuoteDetail.ExtraTrack > 0 Then
                txtExtraTrack.Text = cQuoteDetail.ExtraTrack
            Else
                txtExtraTrack.Text = String.Empty
            End If

            cboFlyScreen.SelectedValue = cQuoteDetail.FlyScreen
            cboWinder.SelectedValue = cQuoteDetail.Winder
            txtSpecialRequirements.Text = cQuoteDetail.SpecialRequirements

            _RulesLouvreDetails.PopulateTopTrackDDL(cboTopTrackType, cboLouvreType.SelectedValue, cQuoteDetail.TopTrackID)

            cboFixedPanelSides.SelectedValue = cQuoteDetail.FixedPanelChannelID

            _RulesLouvreDetails.PopulateHingesDDL(cboHinges, cQuoteDetail.ProductId, cQuoteDetail.ShutterTypeId, cQuoteDetail.BiFoldHingedDoorInOutId, cQuoteDetail.Hinges)
            _RulesLouvreDetails.PopulatePanelTopRailDDL(cboPanelTopRail, cQuoteDetail.ProductId, cboInsertTop.SelectedValue, cQuoteDetail.PanelTopRailType)
            _RulesLouvreDetails.PopulatePanelMidRailDDL(cboPanelMidRail, cQuoteDetail.ProductId, cboInsertTop.SelectedValue, cboInsertBottom.SelectedValue, cQuoteDetail.PanelMidRailType)
            _RulesLouvreDetails.PopulatePanelBottomRailDDL(cboPanelBottomRail, cQuoteDetail.ProductId, cboInsertTop.SelectedValue, cboInsertBottom.SelectedValue, (cboPanelMidRail.SelectedValue > 0), cQuoteDetail.PanelBottomRailType)

            If (cQuoteDetail.OpenVertical = 1) Then
                rdoOpenLeft.Checked = cQuoteDetail.OpenVertical
            ElseIf (cQuoteDetail.OpenVertical = 2) Then
                rdoOpenRight.Checked = cQuoteDetail.OpenVertical
            Else
                rdoOpenLeft.Checked = 0
                rdoOpenRight.Checked = 0
            End If

            ' Need to populate values as they may be set to the previously loaded details panel number list.
            _RulesLouvreDetails.PopulateNoOfPanelsDDL(cQuoteDetail, cboNoOfPanels, cboMakeOpenSize.SelectedValue, cboLouvreType.SelectedValue, cQuoteDetail.Width, cQuoteDetail.NoOfPanels)

            PopulateExtrasTable(txtHiddenPSDetailID.Text)

            PopulateBladeClipColourDDL()

        End If

        EnableDisableDetailsPopupUI()

        SetTopTrackValue()
        SetBottomTrackValue()

        Return cQuoteDetail

    End Function

    Private Function PopulateLouvreDetailsFromUI() As LouvreDetails
        Return PopulateLouvreDetailsFromUI(New LouvreDetails)
    End Function

    Protected Function PopulateLouvreDetailsFromUI(cQuoteDetail As LouvreDetails) As LouvreDetails

        Try
            If CInt(txtHiddenPSDetailID.Text) > 0 Then
                cQuoteDetail.LouvreDetailID = CInt(Me.txtHiddenPSDetailID.Text)
            End If

            If cboRemakeType.SelectedIndex > 0 Then
                cQuoteDetail.RemakeTypeID = CInt(Me.cboRemakeType.SelectedValue)
            End If

            If txtLocation.Text = String.Empty Then
                cQuoteDetail.Location = txtLocation.Text
            End If

            If IsNumeric(txtWidth.Text) Then
                cQuoteDetail.Width = CInt(txtWidth.Text)
            End If

            If IsNumeric(txtHeight.Text) Then
                cQuoteDetail.Height = CInt(txtHeight.Text)
            End If

            If IsNumeric(cboLouvreProd.SelectedValue) Then
                cQuoteDetail.ProductId = CInt(cboLouvreProd.SelectedValue)
                cQuoteDetail.Product = cboLouvreProd.SelectedItem.Text
            End If

            If IsNumeric(cboLouvreType.SelectedValue) Then
                cQuoteDetail.ShutterTypeId = CInt(cboLouvreType.SelectedValue)
                cQuoteDetail.ShutterType = cboLouvreType.SelectedItem.Text
            End If

            If IsNumeric(cboSlide.SelectedValue) Then
                cQuoteDetail.SlideId = CInt(cboSlide.SelectedValue)
                cQuoteDetail.Slide = cboSlide.SelectedItem.Text
            End If

            If txtOpen.Text = "0" Then
                If IsNumeric(cboBiFoldHingedDoor.SelectedValue) Then
                    cQuoteDetail.BiFoldHingedDoorInOutId = CInt(cboBiFoldHingedDoor.SelectedValue)
                    cQuoteDetail.BiFoldHingedDoorInOut = cboBiFoldHingedDoor.SelectedItem.Text
                End If
            End If

            If IsNumeric(cboNoOfPanels.SelectedValue) Then
                cQuoteDetail.NoOfPanels = CInt(cboNoOfPanels.SelectedValue)
            Else
                ' If 0 no panels have been selected then attempt to calculate based on given details.
                cQuoteDetail.NoOfPanels = _RulesLouvreDetails.CalculateMinNumberOfPanelsForOpening(cQuoteDetail, cQuoteDetail.Width, cQuoteDetail.ShutterTypeId)
            End If

            If IsNumeric(cboBladeSize.SelectedValue) Then
                cQuoteDetail.BladeSizeId = CInt(cboBladeSize.SelectedValue)
                cQuoteDetail.BladeSize = cboBladeSize.SelectedItem.Text
            End If

            If IsNumeric(cboFlushBoltsTop.SelectedValue) Then
                cQuoteDetail.FlushBoltsTopId = CInt(cboFlushBoltsTop.SelectedValue)
                cQuoteDetail.FlushBoltsTop = cboFlushBoltsTop.SelectedItem.Text
            End If

            cQuoteDetail.FlushBoltPosition = cboFlushBoltsPosition.SelectedValue

            If IsNumeric(cboFlushBoltsBottom.SelectedValue) Then
                cQuoteDetail.FlushBoltsBottomId = CInt(cboFlushBoltsBottom.SelectedValue)
                cQuoteDetail.FlushBoltsBottom = cboFlushBoltsBottom.SelectedItem.Text
            End If

            If IsNumeric(cboCChannel.SelectedValue) Then
                cQuoteDetail.CChannel = CInt(cboCChannel.SelectedValue)
            End If

            If IsNumeric(cboHChannel.SelectedValue) Then
                cQuoteDetail.HChannel = CInt(cboHChannel.SelectedValue)
            End If

            If IsNumeric(cboLReveal.SelectedValue) Then
                cQuoteDetail.LChannel = CInt(cboLReveal.SelectedValue)
            End If

            If IsNumeric(cboZReveal.SelectedValue) Then
                cQuoteDetail.ZChannel = CInt(cboZReveal.SelectedValue)
            End If

            If txtSpecialRequirements.Text = String.Empty Then
                cQuoteDetail.SpecialRequirements = String.Empty
            Else
                cQuoteDetail.SpecialRequirements = txtSpecialRequirements.Text
            End If

            If (txtLocation.Text = String.Empty) Then
                cQuoteDetail.Location = String.Empty
            Else
                cQuoteDetail.Location = txtLocation.Text
            End If

            If IsNumeric(cboMakeOpenSize.SelectedValue) Then
                cQuoteDetail.MakeOrOpenSizesId = CInt(cboMakeOpenSize.SelectedValue)
            End If

            If IsNumeric(cboEndCapColour.SelectedValue) Then
                cQuoteDetail.EndCapColourId = CInt(cboEndCapColour.SelectedValue)
                cQuoteDetail.EndCapColour = cboEndCapColour.SelectedItem.Text
            End If

            If IsNumeric(cboBladeClipColour.SelectedValue) Then
                cQuoteDetail.BladeClipColourId = CInt(cboBladeClipColour.SelectedValue)
                cQuoteDetail.BladeClipColour = cboBladeClipColour.SelectedItem.Text
            End If

            If IsNumeric(cboPileColour.SelectedValue) Then
                cQuoteDetail.PileColourId = CInt(cboPileColour.SelectedValue)
                cQuoteDetail.PileColour = cboPileColour.SelectedItem.Text
            End If

            If txtMR.Text = "0" Then
                If IsNumeric(txtMidRailHeight.Text) Then
                    cQuoteDetail.MidRailHeight = CInt(txtMidRailHeight.Text)
                Else
                    cQuoteDetail.MidRailHeight = 0
                End If

                If IsNumeric(cboInsertBottom.SelectedValue) Then
                    cQuoteDetail.InsertBottomId = CInt(cboInsertBottom.SelectedValue)
                    cQuoteDetail.InsertBottom = cboInsertBottom.SelectedItem.Text
                End If
            End If

            If IsNumeric(cboInsertTop.SelectedValue) Then
                cQuoteDetail.InsertTopId = CInt(cboInsertTop.SelectedValue)
                cQuoteDetail.InsertTop = cboInsertTop.SelectedItem.Text
            End If

            If txtBladeOperation.Text = "0" Then
                If IsNumeric(cboBladeOperation.SelectedValue) Then
                    cQuoteDetail.BladeOperationId = CInt(cboBladeOperation.SelectedValue)
                    cQuoteDetail.BladeOperation = cboBladeOperation.SelectedItem.Text
                End If
            End If

            If txtBladeOperationBottom.Text = "0" Then
                If IsNumeric(cboBladeOperationBottom.SelectedValue) Then
                    cQuoteDetail.BladeOperationBottomId = CInt(cboBladeOperationBottom.SelectedValue)
                    cQuoteDetail.BladeOperationBottom = cboBladeOperationBottom.SelectedItem.Text
                End If
            End If

            If IsNumeric(cboBladeLocks.SelectedValue) Then
                cQuoteDetail.BladeLocks = CInt(cboBladeLocks.SelectedValue)
            End If

            If IsNumeric(cboLockOptions.SelectedValue) Then
                cQuoteDetail.LockOptionsId = CInt(cboLockOptions.SelectedValue)
                cQuoteDetail.LockOptions = cboLockOptions.SelectedItem.Text
            End If
            If txtBottomTrack.Text = "0" Then
                If IsNumeric(cboBottomTrackType.SelectedValue) Then
                    cQuoteDetail.BottomTrackTypeId = CInt(cboBottomTrackType.SelectedValue)
                    cQuoteDetail.BottomTrackType = cboBottomTrackType.SelectedItem.Text
                End If
            End If

            cQuoteDetail.ColourId = hdnColourID.Value
            cQuoteDetail.Colour = hdnColourName.Value

            If cboStacker.SelectedIndex > 0 Then
                If IsNumeric(cboStacker.SelectedValue) Then
                    cQuoteDetail.StackerLocationId = CInt(cboStacker.SelectedValue)
                    cQuoteDetail.StackerLocation = cboStacker.SelectedItem.Text
                End If
            End If

            If txtCurvedTrack.Text = "0" Then
                If IsNumeric(cboCurvedTrack.SelectedValue) Then
                    cQuoteDetail.CurvedTrack = CInt(cboCurvedTrack.SelectedValue)
                End If

                If IsNumeric(txtExtraTrack.Text) Then
                    cQuoteDetail.ExtraTrack = CInt(txtExtraTrack.Text)
                End If
            End If

            cQuoteDetail.CurvedTrackMaxDeflection = 0

            If IsNumeric(txtCurvedTrackMaxDeflection.Text) Then
                cQuoteDetail.CurvedTrackMaxDeflection = txtCurvedTrackMaxDeflection.Text
            End If

            If IsNumeric(cboFlyScreen.SelectedValue) Then
                cQuoteDetail.FlyScreen = CInt(cboFlyScreen.SelectedValue)
            End If

            If txtWinder.Text = "0" Then
                If IsNumeric(cboWinder.SelectedValue) Then
                    cQuoteDetail.Winder = CInt(cboWinder.SelectedValue)
                End If
            End If

            If IsNumeric(cboTopTrackType.SelectedIndex) Then
                cQuoteDetail.TopTrackID = CInt(cboTopTrackType.SelectedValue)
                cQuoteDetail.TopTrack = cboTopTrackType.SelectedItem.Text
            End If

            If IsNumeric(cboFixedPanelSides.SelectedValue) Then
                cQuoteDetail.FixedPanelChannelID = CInt(cboFixedPanelSides.SelectedValue)
                cQuoteDetail.FixedPanelChannel = cboFixedPanelSides.SelectedItem.Text
            End If

            If IsNumeric(cboHinges.SelectedValue) Then
                cQuoteDetail.Hinges = CInt(cboHinges.SelectedValue)
            End If

            cQuoteDetail.PanelTopRailType = cboPanelTopRail.SelectedValue
            cQuoteDetail.PanelMidRailType = cboPanelMidRail.SelectedValue
            cQuoteDetail.PanelBottomRailType = cboPanelBottomRail.SelectedValue

            If (rdoOpenLeft.Checked = True) Then
                cQuoteDetail.OpenVertical = 1
            ElseIf (rdoOpenRight.Checked = True) Then
                cQuoteDetail.OpenVertical = 2
            Else
                cQuoteDetail.OpenVertical = 0
            End If

        Catch ex As Exception
            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message)
        End Try

        Return cQuoteDetail

    End Function



    Protected Sub ResetPopupControlsToDefault()

        Me.txtHiddenPSDetailID.Text = "0"

        Me.cboRemakeType.SelectedIndex = 0

        'Left Hand Side Of Modal
        txtLocation.Text = String.Empty

        hdnColourID.Value = 0
        hdnColourName.Value = String.Empty
        txtColour.Text = String.Empty
        txtColour.ToolTip = txtColour.Text

        txtHeight.Text = String.Empty
        txtWidth.Text = String.Empty
        cboMakeOpenSize.SelectedValue = MakeOpen.Opening
        cboLouvreProd.SelectedIndex = 0

        If cboNoOfPanels.Items.Count > 0 Then
            cboNoOfPanels.SelectedIndex = 0
        End If

        cboBladeSize.SelectedIndex = 0
        cboEndCapColour.SelectedIndex = 0
        cboBladeClipColour.SelectedIndex = 0
        cboPileColour.SelectedIndex = 0
        txtMidRailHeight.Text = String.Empty
        'cboPivotType.SelectedIndex = 0
        cboFlushBoltsTop.SelectedIndex = 0
        cboFlushBoltsPosition.SelectedValue = 0
        cboFlushBoltsBottom.SelectedIndex = 0
        cboLockOptions.SelectedIndex = 0
        cboBottomTrackType.SelectedIndex = 1
        cboBladeLocks.SelectedIndex = 0
        cboCChannel.SelectedIndex = 0

        'Right Hand Side Of Modal
        cboHChannel.SelectedIndex = 0
        cboLReveal.SelectedIndex = 0
        cboZReveal.SelectedIndex = 0
        cboBladeOperation.SelectedIndex = 1
        ' cboFinishType.SelectedIndex = 0
        cboWinder.SelectedIndex = 0
        'cboHighsetAllowance.SelectedIndex = 0
        cboFlyScreen.SelectedIndex = 0
        cboBladeOperationBottom.SelectedIndex = 0
        cboInsertTop.SelectedIndex = 1
        cboCurvedTrack.SelectedIndex = 0
        txtExtraTrack.Text = String.Empty
        cboInsertBottom.SelectedIndex = 0
        cboSlide.SelectedIndex = 0
        cboStacker.SelectedIndex = 0

        'Added by Michael Behar 22/02/2016
        cboTopTrackType.SelectedIndex = 0
        cboFixedPanelSides.SelectedIndex = 0
        cboLouvreType.SelectedIndex = 0

        'Bottom of Modal
        txtSpecialRequirements.Text = String.Empty
        rdoOpenLeft.Checked = False
        rdoOpenRight.Checked = False

        PopulateShutterTypeDDL(0)
        _RulesLouvreDetails.PopulateNoOfPanelsDDL(Nothing, cboNoOfPanels, MakeOpen.NONE, LouvreTypes.NONE, 0, 0)

    End Sub

    Protected Sub setAllControlsToViewModeOnly()

        Try
            'Handles the shutter details section
            Me.cboRemakeType.Enabled = False
            Me.cboRemakeType.CssClass = "form-select-disable"

            txtLocation.Enabled = False
            txtWidth.Enabled = False
            txtHeight.Enabled = False
            cboLouvreProd.Enabled = False
            cboLouvreProd.CssClass = "form-select-disable"
            cboLouvreType.Enabled = False
            cboLouvreType.CssClass = "form-select-disable"

            cboBiFoldHingedDoor.Enabled = False
            cboBiFoldHingedDoor.CssClass = "form-select-disable"
            cboNoOfPanels.Enabled = False
            cboNoOfPanels.CssClass = "form-select-disable"

            cboEndCapColour.Enabled = False
            cboEndCapColour.CssClass = "form-select-disable"
            cboBladeClipColour.Enabled = False
            cboBladeClipColour.CssClass = "form-select-disable"
            cboPileColour.Enabled = False
            cboPileColour.CssClass = "form-select-disable"
            cboInsertTop.Enabled = False
            cboInsertTop.CssClass = "form-select-disable"

            cboBottomTrackType.Enabled = False
            cboBottomTrackType.CssClass = "form-select-disable"
            cboBladeLocks.Enabled = False
            cboBladeLocks.CssClass = "form-select-disable"


            txtColour.Enabled = False
            txtColour.CssClass = "form-field-disable"

            cboBladeOperation.Enabled = False
            cboBladeOperation.CssClass = "form-select-disable"

            cboCurvedTrack.Enabled = False
            cboCurvedTrack.CssClass = "form-select-disable"

            txtExtraTrack.Enabled = False

            cboFlyScreen.Enabled = False
            cboFlyScreen.CssClass = "form-select-disable"

            cboWinder.Enabled = False
            cboWinder.CssClass = "form-select-disable"


        Catch ex As Exception
            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message)
        End Try
    End Sub


#End Region

    Protected Sub disableAllControls()

        ' BTODO: expand

        txtOrderDate.Enabled = False
        txtReceived.Enabled = False
        txtPickingDate.Enabled = False
        txtScheduledDate.Enabled = False
        txtCuttingDate.Enabled = False
        txtPiningDate.Enabled = False
        txtPrepDate.Enabled = False
        txtAssemblyDate.Enabled = False
        txtHingingDate.Enabled = False
        txtPackupDate.Enabled = False
        txtQCDate.Enabled = False
        txtPostProduction1.Enabled = False
        txtPostProduction2.Enabled = False

        cboOrderType.Enabled = False
        txtActualShippingDate.Enabled = False
        txtPromisedDate.Enabled = False
        txtContractNumber.Enabled = False
        txtSalePrice.Enabled = False
        txtFreightTotal.Enabled = False
        txtCustomerName.Enabled = False
        cboCustomer.Enabled = False
        txtState.Enabled = False
        txtTotalPanels.Enabled = False
        txtTotalSQM.Enabled = False
        cboPriority.Enabled = False
        radDelivery.Enabled = False
        radPickup.Enabled = False


        'disable powdercoat popup controls
        txtPCPickDate.Enabled = False
        txtPCStartDate.Enabled = False
        txtPCETADate.Enabled = False
        txtPCCompleteDate.Enabled = False
        txtPCDescription.Enabled = False
        txtPCPurchaseOrder.Enabled = False
        txtPCCostPrice.Enabled = False
        cboPowdercoater.Enabled = False
        btnSavePowdercoat.Enabled = False

        'disable additional requirements popup controls
        txtRequirementPickDate.Enabled = False
        txtRequirementStartDate.Enabled = False
        txtRequirementETADate.Enabled = False
        txtRequirementCompleteDate.Enabled = False
        txtRequirementDescription.Enabled = False
        txtRequirementPurchaseOrder.Enabled = False
        txtRequirementCostPrice.Enabled = False
        cboRequirementType.Enabled = False
        btnSaveRequirement.Enabled = False

        btnAddDetails.Enabled = False
        btnAddPowdercoat.Enabled = False
        btnAddRequirement.Enabled = False

        btnSaveDetails.Enabled = False 'opening details popup
        btnAddNewNote.Enabled = False

        btnCancellation.Enabled = False

        btnSave.Enabled = False

        makeAllClearButtonsHidden()

        makeAllStatusActionButtonsHidden()


    End Sub

    Private Sub cboJobType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboJobType.SelectedIndexChanged

        pnlCheckMeasureDate.Visible = (cboJobType.SelectedValue = JobType.Retail)
        pnlInstallDate.Visible = (cboJobType.SelectedValue = JobType.Retail)
        pnlRetailInstallDetails.Visible = (cboJobType.SelectedValue = JobType.Retail)

        Page.Validate("productionschedule")
    End Sub

    Protected Function GetPowderCoaterName(cRequirements As AdditionalRequirements) As String
        Dim cPowderCoater As PowderCoater = _PowderCoaters.Find(Function(x) x.ID = cRequirements.PowdercoaterID)

        If cPowderCoater IsNot Nothing Then
            Return cPowderCoater.Name
        Else
            Return SharedConstants.STR_VALUE_NOT_FOUND
        End If
    End Function

    Protected Function GetAdditionalRequirementTypeName(cRequirements As AdditionalRequirements) As String
        Dim cAdditionalRequirementType As AdditionalRequirementType = _AdditionalRequirementTypes.Find(Function(x) x.ID = cRequirements.AdditionalRequirementTypeID)

        If cAdditionalRequirementType IsNot Nothing Then
            Return cAdditionalRequirementType.RequirementTypeName
        Else
            Return SharedConstants.STR_VALUE_NOT_FOUND
        End If
    End Function

    Private Sub btnSavePowdercoat_Click(sender As Object, e As EventArgs) Handles btnSavePowdercoat.Click
        Dim lPowderCoating As List(Of AdditionalRequirements) = ViewState("PowderCoating")
        Dim cRequirement As AdditionalRequirements = lPowderCoating.Find(Function(x) x.AdditionalRequirementsID = txtHidPCRequirementsID.Text)

        If cRequirement Is Nothing Then
            ' New object with new ID.
            cRequirement = New AdditionalRequirements()
            cRequirement.AdditionalRequirementsID = GetUniqueNegativeIDForPowderCoating()
            lPowderCoating.Add(cRequirement)
        End If

        With cRequirement
            .AdditionalRequirementTypeID = AdditionalRequirementTypeID.PowderCoating
            .DescriptionText = txtPCDescription.Text
            .StartDate = CDate(txtPCStartDate.Text)
            .ETADate = CDate(txtPCETADate.Text)
            .CompleteDate = CDate(txtPCCompleteDate.Text)
            .CostPrice = CInt(txtPCCostPrice.Text)
            .PurchaseOrderNo = txtPCPurchaseOrder.Text
            .PickDate = CDate(txtPCPickDate.Text)
            .PowdercoaterID = CInt(cboPowdercoater.SelectedValue)
        End With

        dgvPowdercoat.DataSource = lPowderCoating
        dgvPowdercoat.DataBind()

        ResetPowderCoatingUI()
    End Sub

    Private Sub btnSaveRequirement_Click(sender As Object, e As EventArgs) Handles btnSaveRequirement.Click
        Dim lRequirements As List(Of AdditionalRequirements) = ViewState("Requirements")
        Dim cRequirement As AdditionalRequirements = lRequirements.Find(Function(x) x.AdditionalRequirementsID = txtHidRequirementsID.Text)

        If cRequirement Is Nothing Then
            ' New object with new ID.
            cRequirement = New AdditionalRequirements()
            cRequirement.AdditionalRequirementsID = GetUniqueNegativeIDForAdditionalRequirements()
            lRequirements.Add(cRequirement)
        End If

        With cRequirement
            .AdditionalRequirementTypeID = CInt(cboRequirementType.SelectedValue)
            .DescriptionText = txtRequirementDescription.Text
            .StartDate = CDate(txtRequirementStartDate.Text)
            .ETADate = CDate(txtRequirementETADate.Text)
            .CompleteDate = CDate(txtRequirementCompleteDate.Text)
            .CostPrice = CInt(txtRequirementCostPrice.Text)
            .PurchaseOrderNo = txtRequirementPurchaseOrder.Text
            .PickDate = CDate(txtRequirementPickDate.Text)
        End With

        dgvAdditionalRequirements.DataSource = lRequirements
        dgvAdditionalRequirements.DataBind()

        ResetRequirementsUI()
    End Sub

    Private Sub ResetPowderCoatingUI()

        cboPowdercoater.SelectedValue = 0
        txtPCDescription.Text = String.Empty
        txtPCStartDate.Text = String.Empty
        txtPCETADate.Text = String.Empty
        txtPCCompleteDate.Text = String.Empty
        txtPCCostPrice.Text = String.Empty
        txtPCPurchaseOrder.Text = String.Empty
        txtPCPickDate.Text = String.Empty
    End Sub

    Private Sub ResetRequirementsUI()

        cboRequirementType.SelectedValue = 0
        txtRequirementDescription.Text = String.Empty
        txtRequirementPickDate.Text = String.Empty
        txtRequirementETADate.Text = String.Empty
        txtRequirementCompleteDate.Text = String.Empty
        txtRequirementCostPrice.Text = String.Empty
        txtRequirementPurchaseOrder.Text = String.Empty
        txtRequirementStartDate.Text = String.Empty
    End Sub

    Public Function GetUniqueNegativeIDForAdditionalRequirements() As Integer
        Dim intLowestID As Integer = -1
        Dim cRequirements As List(Of AdditionalRequirements) = ViewState("Requirements")

        For Each ar As AdditionalRequirements In cRequirements
            If ar.AdditionalRequirementsID < intLowestID Then
                intLowestID = ar.AdditionalRequirementsID
            End If
        Next ar

        Return intLowestID - 1
    End Function

    Public Function GetUniqueNegativeIDForPowderCoating() As Integer
        Dim intLowestID As Integer = -1
        Dim cRequirements As List(Of AdditionalRequirements) = ViewState("PowderCoating")

        For Each ar As AdditionalRequirements In cRequirements
            If ar.AdditionalRequirementsID < intLowestID Then
                intLowestID = ar.AdditionalRequirementsID
            End If
        Next ar

        Return intLowestID - 1
    End Function

    Private Sub CheckSelectedCustomerIsLinkedToSybizCustomer()

        If cboCustomer.SelectedValue > 0 Then
            Dim cCustomer As Customer = _Service.GetCustomerByID(cboCustomer.SelectedValue)

            imgSybizLinked.Visible = True

            If cCustomer.CustomerID > 0 Then
                If cCustomer.SybizCustomerID > 0 Then

                    ' If a code exists then sybiz link has been made in the past.
                    imgSybizLinked.ImageUrl = "~/images/green_tick_32x32.png"
                    imgSybizLinked.ToolTip = "Linked to Sybiz"

                    Exit Sub
                End If
            End If

            ' Not linked
            imgSybizLinked.ImageUrl = "~/images/error_x_32x32.png"
            imgSybizLinked.ToolTip = "Not linked to Sybiz"
        Else
            imgSybizLinked.Visible = False
        End If

    End Sub

    Private Sub SetCustomerPricingCategoryUI()

        If cboCustomer.SelectedValue > 0 Then
            Dim cCustomer As Customer = _Service.GetCustomerByID(cboCustomer.SelectedValue)

            imgCustomerPricingCategory.Visible = True

            If cCustomer.CustomerID > 0 Then
                Select Case cCustomer.LouvreCategoryID
                    Case LouvreCategoryEnum.Gold

                        ' If a code exists then sybiz link has been made in the past.
                        imgCustomerPricingCategory.ImageUrl = "~/images/medal_gold_icon_32x32.png"
                        imgCustomerPricingCategory.ToolTip = "Gold Pricing"

                        Exit Sub

                    Case LouvreCategoryEnum.Silver

                        ' If a code exists then sybiz link has been made in the past.
                        imgCustomerPricingCategory.ImageUrl = "~/images/medal_silver_icon_32x32.png"
                        imgCustomerPricingCategory.ToolTip = "Silver Pricing"

                        Exit Sub

                    Case LouvreCategoryEnum.Trade

                        ' If a code exists then sybiz link has been made in the past.
                        imgCustomerPricingCategory.ImageUrl = "~/images/medal_trade_icon_32x32.png"
                        imgCustomerPricingCategory.ToolTip = "Trade Pricing"

                        Exit Sub

                    Case LouvreCategoryEnum.Retail

                        ' If a code exists then sybiz link has been made in the past.
                        imgCustomerPricingCategory.ImageUrl = "~/images/medal_retail_icon_32x32.png"
                        imgCustomerPricingCategory.ToolTip = "Retail Pricing"

                        Exit Sub

                End Select
            End If

            imgCustomerPricingCategory.Visible = False
        Else
            imgCustomerPricingCategory.Visible = False
        End If

    End Sub

    Private Sub cboCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCustomer.SelectedIndexChanged

        pnlCreditHold.Visible = False

        If cboCustomer.SelectedIndex > 0 Then
            Dim cCustomer As Customer = _Service.GetCustomerByID(cboCustomer.SelectedValue)

            If cCustomer.CustomerID > 0 Then

                InitCosting(cCustomer, txtEnteredDatetime.Text)

                lblCustomerDetails.Text = String.Empty

                If Not String.IsNullOrEmpty(cCustomer.TradingName) Then
                    lblCustomerDetails.Text &= cCustomer.TradingName & SharedConstants.STR_BREAK
                End If

                If Not String.IsNullOrEmpty(cCustomer.CustomerAbbreviation) Then
                    lblCustomerDetails.Text &= cCustomer.CustomerAbbreviation & SharedConstants.STR_BREAK
                End If

                ' Populate physical address details.
                Dim dtAddresses As DataTable = _Service.GetAddressesByCustomerID(CInt(cboCustomer.SelectedValue))

                For Each r As DataRow In dtAddresses.Rows
                    If Not IsDBNull(r("AddressType")) AndAlso Not IsDBNull(r("IsPrimary")) Then
                        If r("AddressType") = SharedEnums.AddressType.Physical AndAlso CBool(r("IsPrimary")) Then

                            lblCustomerDetails.Text &= _Service.convertAddressDataRowToString(dtAddresses.Rows(0), SharedConstants.STR_BREAK) & SharedConstants.STR_BREAK
                            Exit For
                        End If
                    End If
                Next r

                If Not String.IsNullOrEmpty(cCustomer.CustomerPhone1) Then
                    lblCustomerDetails.Text &= cCustomer.CustomerPhone1 & SharedConstants.STR_BREAK
                End If

                If Not String.IsNullOrEmpty(cCustomer.CustomerPhone2) Then
                    lblCustomerDetails.Text &= cCustomer.CustomerPhone2 & SharedConstants.STR_BREAK
                End If

                If Not String.IsNullOrEmpty(cCustomer.CustomerPhone3) Then
                    lblCustomerDetails.Text &= cCustomer.CustomerPhone3 & SharedConstants.STR_BREAK
                End If

                ' Populate all delivery addresses into ddl for customer.
                cboDeliveryAddress.Items.Clear()
                cboDeliveryAddress.Items.Add(New ListItem(String.Empty, 0))

                ' Remember the previously selected address ID from the production schedule.
                Dim cProdSchedule As ProductionSchedule = _Service.GetProdScheduleClsByID(txtId.Text)

                If cProdSchedule.ID > 0 Then
                    pnlCreditHold.Visible = ((Not chkCreditCheckOverride.Checked) AndAlso cProdSchedule.CreditHold(_Service, cCustomer))
                End If

                For Each r As DataRow In dtAddresses.Rows
                    If Not IsDBNull(r("AddressType")) AndAlso Not IsDBNull(r("Discontinued")) Then
                        If r("AddressType") = SharedEnums.AddressType.Delivery AndAlso Not CBool(r("Discontinued")) Then
                            Dim cListItem As New ListItem(_Service.convertAddressDataRowToString(r, ", "), CInt(r("ID")))

                            cboDeliveryAddress.Items.Add(cListItem)

                            If Not IsDBNull(r("IsPrimary")) Then
                                If CBool(r("IsPrimary")) Then
                                    cListItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.FontWeight, "bold")
                                End If
                            End If
                        End If
                    End If
                Next r

                EnableDisableDelivery(cProdSchedule.DeliveryAddressID > 0)

                ' Select the delivery address
                If cProdSchedule.DeliveryAddressID >= 0 Then
                    If cboDeliveryAddress.Items.FindByValue(cProdSchedule.DeliveryAddressID) IsNot Nothing Then
                        cboDeliveryAddress.SelectedValue = cProdSchedule.DeliveryAddressID
                    End If
                End If

                'condition add by surendra on temp base as cCustomer.SybizCustomerID returning value with </br> in case of external customer date-16-07-2020
                txtSybizCustomerID.Text = cCustomer.SybizCustomerID
                If Convert.ToInt32(Session("CustomerId")) = 0 Then
                    txtSybizCustomerID.Text &= SharedConstants.STR_BREAK
                End If


                If cCustomer.CollectionFromFactory Then
                    lblPostProduction1.Text = "Invoice Date"
                    lblPostProduction2.Text = "Collect Date"
                Else
                    lblPostProduction1.Text = "Despatch Date"
                    lblPostProduction2.Text = "Invoice Date"
                End If
            Else
                lblPostProduction1.Text = "Despatch Date"
                lblPostProduction2.Text = "Invoice Date"
                lblCustomerDetails.Text = String.Empty
            End If
        Else
            lblPostProduction1.Text = "Despatch Date"
            lblPostProduction2.Text = "Invoice Date"
            lblCustomerDetails.Text = String.Empty
        End If
        'Added by surendra 3-sep-2020
        If cboDeliveryAddress.Items.Count > 0 And Convert.ToInt32(Request.Params("ScheduleId")) = 0 Then
            If cboDeliveryAddress.Items.Count > 1 Then
                cboDeliveryAddress.SelectedIndex = 1
            End If
            radDelivery.Checked = True
            'added by surendra 24 sep 2020'
            ViewState("AddressZoneID") = 0
            EnableDisableDelivery(True)
        End If


        CheckSelectedCustomerIsLinkedToSybizCustomer()
        SetCustomerPricingCategoryUI()

        Page.Validate("productionschedule")

    End Sub

    Private Sub btnAddRequirement_Click(sender As Object, e As EventArgs) Handles btnAddRequirement.Click
        txtHidRequirementsID.Text = 0
        mpeAdditionalRequirements.Show()
    End Sub

    Private Sub dgvPowdercoat_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvPowdercoat.RowCommand

        If (e.CommandName = "PowdercoatDetail") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dgvPowdercoat.Rows(index)

            Dim intAdditionalRequirementsID As Integer = dgvPowdercoat.DataKeys(row.RowIndex).Values("AdditionalRequirementsID")

            LoadPowdercoatingToPopup(intAdditionalRequirementsID)

            mpePowderCoat.Show()
        End If
    End Sub

    Private Sub dgvAdditionalRequirements_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvAdditionalRequirements.RowCommand

        If (e.CommandName = "RequirementDetail") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dgvAdditionalRequirements.Rows(index)

            Dim intAdditionalRequirementsID As Integer = dgvAdditionalRequirements.DataKeys(row.RowIndex).Values("AdditionalRequirementsID")

            LoadRequirementsToPopup(intAdditionalRequirementsID)

            mpeAdditionalRequirements.Show()
        End If
    End Sub

    Protected Sub LoadPowdercoatingToPopup(intAdditionalRequirementsID As Integer)
        Dim lPowderCoating As List(Of AdditionalRequirements) = ViewState("PowderCoating")
        Dim cPowderCoating As AdditionalRequirements = lPowderCoating.Find(Function(x) x.AdditionalRequirementsID = intAdditionalRequirementsID)

        cboPowdercoater.SelectedValue = cPowderCoating.PowdercoaterID
        txtPCPickDate.Text = Format(cPowderCoating.PickDate, "d MMM yyyy")
        txtPCStartDate.Text = Format(cPowderCoating.StartDate, "d MMM yyyy")
        txtPCETADate.Text = Format(cPowderCoating.ETADate, "d MMM yyyy")
        txtPCDescription.Text = cPowderCoating.DescriptionText
        txtPCPurchaseOrder.Text = cPowderCoating.PurchaseOrderNo
        txtPCCostPrice.Text = cPowderCoating.CostPrice
        txtPCCompleteDate.Text = Format(cPowderCoating.CompleteDate, "d MMM yyyy")

        txtHidPCRequirementsID.Text = intAdditionalRequirementsID

        Dim cJobStockUsage As List(Of StockUsage) = _Service.GetJobStockUsageByReqID(intAdditionalRequirementsID)

        dgvStockList2.DataSource = cJobStockUsage
        dgvStockList2.DataBind()

    End Sub

    Protected Sub LoadRequirementsToPopup(intAdditionalRequirementsID As Integer)
        Dim lRequirements As List(Of AdditionalRequirements) = ViewState("Requirements")
        Dim cRequirement As AdditionalRequirements = lRequirements.Find(Function(x) x.AdditionalRequirementsID = intAdditionalRequirementsID)

        cboRequirementType.SelectedValue = cRequirement.AdditionalRequirementTypeID
        txtRequirementPickDate.Text = Format(cRequirement.PickDate, "d MMM yyyy")
        txtRequirementStartDate.Text = Format(cRequirement.StartDate, "d MMM yyyy")
        txtRequirementETADate.Text = Format(cRequirement.ETADate, "d MMM yyyy")
        txtRequirementCompleteDate.Text = Format(cRequirement.CompleteDate, "d MMM yyyy")
        txtRequirementDescription.Text = cRequirement.DescriptionText
        txtRequirementPurchaseOrder.Text = cRequirement.PurchaseOrderNo
        txtRequirementCostPrice.Text = cRequirement.CostPrice

        txtHidRequirementsID.Text = intAdditionalRequirementsID


        Dim cStockUsage As List(Of StockUsage) = _Service.GetJobStockUsageByReqID(intAdditionalRequirementsID)

        dgvStockList.DataSource = cStockUsage
        dgvStockList.DataBind()

    End Sub

    Private Sub btnCancelPowdercoat_Click(sender As Object, e As EventArgs) Handles btnCancelPowdercoat.Click
        ResetPowderCoatingUI()
    End Sub

    Private Sub btnCancelRequirement_Click(sender As Object, e As EventArgs) Handles btnCancelRequirement.Click
        ResetRequirementsUI()
    End Sub

    Protected Sub CheckOutstandingItemsForAcceptance()

        lblAcceptance.Text = String.Empty

        Dim lLouvres As LouvreDetailsCollection = _Service.GetLouvreDetailsCollectionByProductionScheduleID(txtId.Text)

        If lLouvres.Count > 0 Then
            Dim boolCurvedTrack As Boolean = (lLouvres.Sum(Function(x) x.CurvedTrack) > 0)

            If boolCurvedTrack Then
                'curved track required
                'prompt for bender order to be done

                lblAcceptance.Text &= "- Curved track Is required. Please ensure the order Is entered for the bender.<br/><br/>"
                lblAcceptance.Text &= "- A separate powdercoat order Is also required.<br/><br/>"

                lblAcceptance.Text &= "- A CAD drawing Is required for this job.<br/><br/>"
                lblAcceptance.Text &= "- Please ensure a copy of this Is uploaded to the job once completed.<br/>"
            End If
        End If

    End Sub

    Private Function GetSybizTargetDate(dteStartDate As Date) As Date
        Dim lLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")
        Dim lColours As List(Of Colour) = _Service.getColours()

        For Each l As LouvreDetails In lLouvreDetails
            If Not l.Deleted Then
                Dim cColour As Colour = lColours.Find(Function(x) x.ID = l.ColourId)

                If cColour IsNot Nothing Then
                    If cColour.CoatingTypeID > CoatingType.StandardPowderCoat Then
                        ' 6 weeks
                        Return dteStartDate.AddDays(42)
                    End If
                End If
            End If
        Next l

        ' 4 weeks
        Return dteStartDate.AddDays(28)
    End Function

    Protected Sub populateGRADetailsGrid()


        Dim SQLstr As String
        SQLstr = "Select GRAID, ScheduleID, OpeningNumber, PanelNumber, IssueDescription, CauseOfIssue, SuggestedAction from tblGRADetails where ScheduleID = " & Me.txtId.Text
        Dim dtGridDetails As DataTable = _Service.RunSQLScheduling(SQLstr)
        Me.dgvGRADetails.DataSource = dtGridDetails
        Me.dgvGRADetails.DataBind()

    End Sub

    Protected Function generateProductionSheetData(dtLouvreDetails As DataTable, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

        Dim bolOK As Boolean = True

        'check for template existence
        Dim dtProductionSheetTemplate As DataTable = New DataTable
        If dtProductionSheetTemplate.Rows.Count > 0 Then


        Else
            'none found for this combination yet
            Return False
        End If
        dtProductionSheetTemplate.Dispose()
        dtProductionSheetTemplate = Nothing

        Dim dtProductionSheetControlValues As DataTable = New DataTable
        If dtProductionSheetControlValues.Rows.Count > 0 Then

            Dim cLouvreDetails As LouvreDetails = _Service.SetLouvreDetailsObjectFromDataRow(dtLouvreDetails.Rows(0))
            bolOK = runCalculations(cLouvreDetails, cnn, trans)

        Else
            'none found for this combination yet
            Return False
        End If
        dtProductionSheetControlValues.Dispose()
        dtProductionSheetControlValues = Nothing



        Return bolOK

    End Function

    Protected Function runCalculations(cLouvreDetails As LouvreDetails, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean
        Dim cLouvreSpec As New LouvreSpecDesign

        Dim intOpeningHeight As Integer = 0
        Dim intOpeningWidth As Integer = 0
        Dim intNoOfPanels As Integer = 0

        Dim intPanelHeight As Integer = 0
        Dim intPanelWidth As Integer = 0
        Dim intGrossTransomHeight As Integer = 0
        Dim intPanelTransomHeight As Integer = 0
        Dim intStileLength As Integer = 0
        Dim intRailsLength As Integer = 0
        Dim intNoOfProfiles As Integer = 0
        Dim intProfileCutLength As Integer = 0
        Dim intBladeInfillTop As Integer = 0
        Dim intBladeInfillBottom As Integer = 0
        Dim intTransom1Cut As Integer = 0
        Dim intTransom2Cut As Integer = 0
        Dim intDummyBladeTop As Integer = 0
        Dim intDummyBladeBottom As Integer = 0



        'below calculations are for a DLi bifold with transom - taken from the production sheet

        intOpeningHeight = cLouvreDetails.Height
        intOpeningWidth = cLouvreDetails.Width
        intNoOfPanels = cLouvreDetails.NoOfPanels

        intPanelHeight = intOpeningHeight - (40 - 10 - 13 - 12)
        intPanelWidth = intOpeningWidth - (8 - ((intNoOfPanels - 1) * 5) - 8)
        intGrossTransomHeight = cLouvreDetails.MidRailHeight

        intPanelTransomHeight = 0

        intStileLength = intPanelHeight - (2 * 3)
        intRailsLength = intPanelWidth - 89

        intProfileCutLength = intPanelWidth - 114
        intNoOfProfiles = (intPanelHeight - 95) / 80

        intBladeInfillTop = intPanelHeight - intPanelTransomHeight - 45
        intBladeInfillBottom = intPanelTransomHeight - 135
        intTransom1Cut = intPanelWidth - 89
        intTransom2Cut = intPanelWidth - 89
        intDummyBladeTop = intPanelWidth - 100
        intDummyBladeBottom = intPanelWidth - 100





        Dim intCornerPlugs As Integer = 0
        Dim intHoleCaps As Integer = 0

        intCornerPlugs = intNoOfPanels * 4
        intHoleCaps = intNoOfPanels * 12








        cLouvreSpec.LouvreDetailsID = cLouvreDetails.LouvreDetailID

        cLouvreSpec.OpeningHeight = intOpeningHeight
        cLouvreSpec.OpeningWidth = intOpeningWidth
        cLouvreSpec.NoOfPanels = intNoOfPanels

        cLouvreSpec.PanelHeight = intPanelHeight
        cLouvreSpec.PanelWidth = intPanelWidth
        cLouvreSpec.PanelTransomHeight = intPanelTransomHeight

        cLouvreSpec.StileLength = intStileLength
        cLouvreSpec.BottomRailsLength = intRailsLength
        cLouvreSpec.TopRailsLength = intRailsLength

        cLouvreSpec.ProfileCutLength = intProfileCutLength
        cLouvreSpec.ProfileSize = CInt(cLouvreDetails.BladeSize)
        cLouvreSpec.NoOfProfileSlats = intNoOfProfiles

        cLouvreSpec.TopInfillTopLength = intBladeInfillTop
        cLouvreSpec.BottomInfillTopLength = intBladeInfillBottom

        cLouvreSpec.TransomWidth = intTransom1Cut
        cLouvreSpec.Transom2Width = intTransom2Cut

        cLouvreSpec.DummyBladeTopWidth = intDummyBladeTop
        cLouvreSpec.DummyBladeBottomWidth = intDummyBladeBottom

        cLouvreSpec.NoOfCornerPlugs = intCornerPlugs
        cLouvreSpec.NoOfHoleCaps = intHoleCaps


        Dim intID As Integer = SharedConstants.DEFAULT_INTEGER_VALUE

        Dim dt As DataTable = _Service.GetLouvreSpecDesignRecordByLouvreDetailsID(cLouvreDetails.LouvreDetailID, cnn, trans)
        If dt.Rows.Count > 0 Then
            Dim bolOK As Boolean = _Service.UpdateLouvreSpecDesign(cLouvreSpec, cnn, trans)
            If bolOK Then
                intID = cLouvreSpec.LouvreDetailsID
            End If
        Else
            intID = _Service.AddLouvreSpecDesign(cLouvreSpec, cnn, trans)
        End If
        dt.Dispose()
        dt = Nothing



        If intID = SharedConstants.DEFAULT_INTEGER_VALUE Then
            Return False
        Else
            Return True
        End If

    End Function

    Protected Function CheckSybizCustomerDetails() As Boolean
        Dim bolStopCustomer As Boolean = False
        Dim dt As DataTable = _Service.RunSQLOzrollSybiz("select * from dr.Customer where CustomerId=" & Me.txtSybizCustomerID.Text)

        lblStopCustomerLabel.Text = String.Empty

        If dt.Rows.Count > 0 Then
            If CInt(dt.Rows(0)("TradingTermId")) = 1 Then
                'cod
                lblStopCustomerLabel.Text = "Cash on Delivery customer - approval is required"
                bolStopCustomer = True
            End If

            If CBool(dt.Rows(0)("StopJobs")) = True Then
                'stop jobs - there is also a "stop orders" field
                lblStopCustomerLabel.Text = "This customer is currently under a stop."
                bolStopCustomer = True
            End If
        End If

        dt.Dispose()
        dt = Nothing

        Return bolStopCustomer
    End Function

    Private Sub btnCreditApprove_Click(sender As Object, e As EventArgs) Handles btnCreditApprove.Click

        Me.pnlStopCustomer.Visible = False

        'update table with who allowed approval


        _Service.ExecuteSQLScheduling("update dbo.tblProductionScheduleList set CreditOverrideUserID=" & Session("sessUserID").ToString & ",CreditOverrideDateTime=getdate() where ID=" & Me.txtId.Text)


    End Sub

    Private Sub btnCreditCancel_Click(sender As Object, e As EventArgs) Handles btnCreditCancel.Click

        Me.ModalPopupExtender1.Hide()

    End Sub

    Private Sub btnViewCreditDetails_Click(sender As Object, e As EventArgs) Handles btnViewCreditDetails.Click

        Me.ModalPopupExtender1.Show()

    End Sub

    Private Sub btnOutstanding_Click(sender As Object, e As EventArgs) Handles btnOutstanding.Click

        Me.ModalPopupExtender5.Show()

    End Sub

    Private Sub btnCancelOutstanding_Click(sender As Object, e As EventArgs) Handles btnCancelOutstanding.Click

        Me.ModalPopupExtender5.Hide()

    End Sub

    Private Sub btnConfirmOutstanding_Click(sender As Object, e As EventArgs) Handles btnConfirmOutstanding.Click

        Me.pnlOutstandingItems.Visible = False

        'update table with who confirmed the action

        _Service.ExecuteSQLScheduling("update dbo.tblProductionScheduleList set OutstandingItemsConfirmID=" & Session("sessUserID").ToString & ",OutstandingItemsConfirmDateTime=getdate() where ID=" & Me.txtId.Text)


    End Sub

    Protected Sub txtPlannedShippingDate_TextChanged(sender As Object, e As EventArgs) Handles txtPlannedShippingDate.TextChanged
        If txtPlannedShippingDate.Text.Trim.Length > 0 And txtRequirementETADate.Text.Trim.Length > 0 Then
            If CDate(txtPlannedShippingDate.Text) < CDate(txtRequirementETADate.Text) Then
                txtPlannedShippingDate.ForeColor = Color.Red
            Else
                txtPlannedShippingDate.ForeColor = Color.Black
            End If
        Else
            txtPlannedShippingDate.ForeColor = Color.Black
        End If
    End Sub

    Protected Sub txtRequirementETADate_TextChanged(sender As Object, e As EventArgs) Handles txtRequirementETADate.TextChanged
        If txtPlannedShippingDate.Text.Trim.Length > 0 And txtRequirementETADate.Text.Trim.Length > 0 Then
            If CDate(txtPlannedShippingDate.Text) < CDate(txtRequirementETADate.Text) Then
                txtPlannedShippingDate.ForeColor = Color.Red
            Else
                txtPlannedShippingDate.ForeColor = Color.Black
            End If
        Else
            txtPlannedShippingDate.ForeColor = Color.Black
        End If
    End Sub
    Protected Sub btnAcceptOrder_Click(sender As Object, e As EventArgs) Handles btnAcceptOrder.Click

        Response.Redirect("AcceptOrder.aspx?ScheduleId=" & Me.txtId.Text, False)

    End Sub
    Protected Sub btnPickingList_Click(sender As Object, e As EventArgs) Handles btnPickingList.Click

        Response.Redirect("UpdateStockPicking.aspx", False)

    End Sub
    Protected Sub btnAddPowdercoat_Click(sender As Object, e As EventArgs) Handles btnAddPowdercoat.Click
        txtHidPCRequirementsID.Text = 0
        mpePowderCoat.Show()
    End Sub

    Protected Function calculateStatusDisplayText(intScheduleID As Integer, intOrderStatusID As Integer) As String

        Dim strStatus As String = String.Empty



        'get orderstatus list from backend table
        Dim dtStatus As DataTable = _Service.RunSQLScheduling("select * from dbo.tblStatus")
        If dtStatus.Rows.Count = 0 Then
            Return String.Empty
        End If

        Dim drows() As DataRow = dtStatus.Select("StatusID=" & intOrderStatusID.ToString)
        If drows.Length > 0 Then
            strStatus = drows(0)("OrderStatusName").ToString
        Else
            Return String.Empty
        End If
        drows = Nothing

        Dim bolCheckAddReq As Boolean = False

        'check current statusid
        Select Case intOrderStatusID
            Case 2, 3, 9, 10, 11, 12
                bolCheckAddReq = True
            Case Else

        End Select

        If bolCheckAddReq Then
            'if between accepted and despatch (or invoicing if for collection) then check for additional requirements outstanding
            Dim dtAddReq As DataTable = _Service.RunSQLScheduling("select * from dbo.tblAdditionalRequirements where ProductionScheduleID=" & intScheduleID.ToString & " and AdditionalRequirementTypeID=1 and CompleteDate is null")
            If dtAddReq.Rows.Count > 0 Then
                If IsDBNull(dtAddReq.Rows(0)("PickDate")) Then
                    strStatus = "Powdercoat - Awaiting Picking"
                End If

                If strStatus = String.Empty Then
                    If IsDBNull(dtAddReq.Rows(0)("StartDate")) Then
                        strStatus = "Powdercoat - Awaiting Despatch"
                    End If
                End If

                If strStatus = String.Empty Then
                    If IsDBNull(dtAddReq.Rows(0)("CompleteDate")) Then
                        strStatus = "Powdercoat - Awaiting Return"
                    End If
                End If
            End If
            dtAddReq.Dispose()
            dtAddReq = Nothing
        End If



        Return strStatus

    End Function

    Protected Function checkOutstandingAddtionalRequirements(intScheduleID As Integer) As Boolean


        Dim bolOutstanding As Boolean = False

        'if between accepted and despatch (or invoicing if for collection) then check for additional requirements outstanding
        Dim dtAddReq As DataTable = _Service.RunSQLScheduling("select * from dbo.tblAdditionalRequirements where ProductionScheduleID=" & intScheduleID.ToString & " and CompleteDate is null")
        If dtAddReq.Rows.Count > 0 Then
            bolOutstanding = True
        End If
        dtAddReq.Dispose()
        dtAddReq = Nothing



        Return bolOutstanding

    End Function

    Private Sub btnAddDetails_Click(sender As Object, e As EventArgs) Handles btnAddDetails.Click

        ''added by surendra ticket #66605
        Dim clsProdSchedule As ProductionSchedule = New ProductionSchedule

        clsProdSchedule = _Service.GetProdScheduleClsByID(txtId.Text)
        If Not String.IsNullOrEmpty(clsProdSchedule.SybizJobCode) Then
            lblStatus.Text = "Louvres detail can not be added in disable mode."
        Else
            Dim lLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")
            Dim cNewDetails As New LouvreDetails

            ' Remember we are adding a new louvre detail
            ViewState("NewLouvreDetail") = True

            ' Add new louvre details with unique negative ID.
            cNewDetails = lLouvreDetails.Add(cNewDetails, True)

            ResetPopupControlsToDefault()
            txtHiddenPSDetailID.Text = cNewDetails.LouvreDetailID

            PopulateExtrasTable(cNewDetails.LouvreDetailID)

            EnableDisableDetailsPopupUI()

            ModalPopupExtender.Show()
        End If

    End Sub


    Sub txtHeight_OnTextChanged(sender As Object, e As EventArgs) Handles txtHeight.TextChanged
        EnableDisableDetailsPopupUI()
        ViewState("DetailsTextChanged") = True
        Page.Validate("details")
    End Sub

    Private Sub txtCurvedTrackMaxDeflection_TextChanged(sender As Object, e As EventArgs) Handles txtCurvedTrackMaxDeflection.TextChanged
        EnableDisableDetailsPopupUI()
        ViewState("DetailsTextChanged") = True
        Page.Validate("details")
    End Sub

    Protected Sub SetDefaultMidrailHeight()
        Dim intHeight As Integer = 0

        If txtHeight.Text <> String.Empty And cboLouvreType.SelectedValue > 0 Then

            intHeight = CInt(txtHeight.Text)

            If intHeight >= _RulesLouvreDetails.MidRailRequiredHeight Then
                Dim intPanelCentreMidrailHeight As Integer = intHeight / 2

                ' The midrail is required so auto position the mid rail value.
                txtMidRailHeight.Text = _RulesLouvreDetails.CalculateMidRailHeight(cboLouvreType.SelectedValue, intPanelCentreMidrailHeight)
            End If

        End If

    End Sub

    Private Sub btnDeliveryDocket_Click(sender As Object, e As EventArgs) Handles btnDeliveryDocket.Click

        If txtId.Text <> String.Empty Then
            If Save() Then
                If txtViewType.Text = ViewType.Update Then
                    If CInt(txtViewType.Text) = ViewType.Add Then
                        Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=deliverydocket", False)
                    Else
                        Response.Redirect("LouvreJobDetails.aspx?" & GetCleanQueryString() & "&Status=1" & "&doc=deliverydocket", False)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub btnRunningSheet_Click(sender As Object, e As EventArgs) Handles btnRunningSheet.Click

        If txtId.Text <> String.Empty Then
            If Save() Then
                If txtViewType.Text = ViewType.Update Then
                    If CInt(txtViewType.Text) = ViewType.Add Then
                        Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=runningsheet", False)
                    Else
                        Response.Redirect("LouvreJobDetails.aspx?" & GetCleanQueryString() & "&Status=1" & "&doc=runningsheet", False)
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub btnTimeSheet_Click(sender As Object, e As EventArgs) Handles btnTimeSheet.Click

        If txtId.Text <> String.Empty Then
            If Save() Then
                If txtViewType.Text = ViewType.Update Then
                    If CInt(txtViewType.Text) = ViewType.Add Then
                        Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=timesheet", False)
                    Else
                        Response.Redirect("LouvreJobDetails.aspx?" & GetCleanQueryString() & "&Status=1" & "&doc=timesheet", False)
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub btnCoverSheet_Click(sender As Object, e As EventArgs) Handles btnCoverSheet.Click

        If txtId.Text <> String.Empty Then
            If Save() Then
                If txtViewType.Text = ViewType.Update Then
                    If CInt(txtViewType.Text) = ViewType.Add Then
                        Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ViewType=" & ViewType.Update & "&Status=1" & "&doc=coversheet", False)
                    Else
                        Response.Redirect("LouvreJobDetails.aspx?" & GetCleanQueryString() & "&Status=1" & "&doc=coversheet", False)
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub cboDeliveryAddress_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDeliveryAddress.SelectedIndexChanged
        RecalculateUIFreightFromDB()
        Page.Validate("productionschedule")
    End Sub

    Private Sub cboLouvreType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboLouvreType.SelectedIndexChanged
        Dim intWidth As Integer = 0
        Dim cLouvreDetails As LouvreDetails = PopulateLouvreDetailsFromUI()

        If IsNumeric(txtWidth.Text) Then
            intWidth = txtWidth.Text
        End If

        If String.IsNullOrEmpty(cboNoOfPanels.SelectedValue) Then
            _RulesLouvreDetails.PopulateNoOfPanelsDDL(cLouvreDetails, cboNoOfPanels, cboMakeOpenSize.SelectedValue, cboLouvreType.SelectedValue, intWidth, 0)
        Else
            _RulesLouvreDetails.PopulateNoOfPanelsDDL(cLouvreDetails, cboNoOfPanels, cboMakeOpenSize.SelectedValue, cboLouvreType.SelectedValue, intWidth, cboNoOfPanels.SelectedValue)
        End If
        'added by surendra 30/10/2020 ticket #62387 
        SetCChanelCombo()
        'added by surendra 30/10/2020 ticket #62782 
        SetHChannelCombo()

        EnableDisableDetailsPopupUI()
        SetTopTrackValue()
        SetBottomTrackValue()

        'Added By Pradeep Singh for the ticket id #62385
        If ViewState("NewLouvreDetail") Then
            SetTopPannelValue()
            SetBottomPannelValue()
            'Added By Pradeep Singh for the ticket id #62385
            SetFlushBoltValue()
        End If
        'Added By Pradeep Singh for the ticket id #63195
        'SetUIforFixedBladeAngles()   -- Commented by Surendra dt 01/12/2020
        Page.Validate("details")
    End Sub

    Private Sub txtWidth_TextChanged(sender As Object, e As EventArgs) Handles txtWidth.TextChanged
        Dim intWidth As Integer = 0
        Dim cLouvreDetails As LouvreDetails = PopulateLouvreDetailsFromUI()

        If IsNumeric(txtWidth.Text) Then
            intWidth = txtWidth.Text
        End If

        If String.IsNullOrEmpty(cboNoOfPanels.SelectedValue) Then
            _RulesLouvreDetails.PopulateNoOfPanelsDDL(cLouvreDetails, cboNoOfPanels, cboMakeOpenSize.SelectedValue, cboLouvreType.SelectedValue, intWidth, 0)
        Else
            _RulesLouvreDetails.PopulateNoOfPanelsDDL(cLouvreDetails, cboNoOfPanels, cboMakeOpenSize.SelectedValue, cboLouvreType.SelectedValue, intWidth, cboNoOfPanels.SelectedValue)
        End If

        'added by surendra 30/10/2020 ticket #62782
        SetHChannelCombo()
        EnableDisableDetailsPopupUI()
        ViewState("DetailsTextChanged") = True
        Page.Validate("details")
    End Sub

    Private Sub SetTopTrackValue()

        If _RulesLouvreDetails.ShouldBeSetTopTrack(cboLouvreType.SelectedValue, cboLouvreProd.SelectedValue) Then
            Select Case cboLouvreProd.SelectedValue
                Case LouvreStyles.CL

                    ' Default selection
                    If cboTopTrackType.SelectedValue <= TopTrackTypes.NONE OrElse
                        cboTopTrackType.SelectedValue = TopTrackTypes.DLiWithoutSubHead Then

                        If cboLouvreType.SelectedValue = LouvreTypes.Stacker Then
                            cboTopTrackType.SelectedValue = TopTrackTypes.Box60x40
                        Else
                            cboTopTrackType.SelectedValue = TopTrackTypes.CL
                        End If
                    End If

                Case LouvreStyles.DLi

                    ' Default selection
                    If cboTopTrackType.SelectedValue <= TopTrackTypes.NONE OrElse
                        cboTopTrackType.SelectedValue = TopTrackTypes.CL Then

                        If cboLouvreType.SelectedValue = LouvreTypes.Stacker Then
                            cboTopTrackType.SelectedValue = TopTrackTypes.Box60x40
                        Else
                            cboTopTrackType.SelectedValue = TopTrackTypes.DLiWithoutSubHead
                        End If

                    End If

                Case LouvreStyles.NONE
                    cboTopTrackType.SelectedValue = TopTrackTypes.NONE
            End Select
        Else
            cboTopTrackType.SelectedValue = TopTrackTypes.NONE
        End If
    End Sub

    Private Sub SetBottomTrackValue()

        If _RulesLouvreDetails.ShouldBeSetBottomTrack(cboLouvreType.SelectedValue, cboLouvreProd.SelectedValue) Then
            Select Case cboLouvreProd.SelectedValue
                Case LouvreStyles.DLi

                    ' Default selection
                    If cboBottomTrackType.SelectedValue <= BottomTrackTypes.NONE Then
                        cboBottomTrackType.SelectedValue = BottomTrackTypes.Dli12mm
                    End If

                Case LouvreStyles.CL

                    ' Default selection
                    If cboBottomTrackType.SelectedValue <= BottomTrackTypes.NONE Then
                        cboBottomTrackType.SelectedValue = BottomTrackTypes.CL25mm
                    End If

                Case LouvreStyles.NONE
                    cboBottomTrackType.SelectedValue = TopTrackTypes.NONE
            End Select
        Else
            cboBottomTrackType.SelectedValue = TopTrackTypes.NONE
        End If
    End Sub

    Private Sub cboLouvreProd_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboLouvreProd.SelectedIndexChanged

        PopulateShutterTypeDDL(cboLouvreType.SelectedIndex)

        EnableDisableDetailsPopupUI()

        SetTopTrackValue()
        SetBottomTrackValue()
        Page.Validate("details")
    End Sub

    Protected Sub valcustCutLengthMax_ServerValidate(source As Object, args As ServerValidateEventArgs)

        Dim intUnitOfMeasurement As Integer = 0

        ' Get the extra
        Dim valControl As CustomValidator = DirectCast(source, CustomValidator)
        Dim extrasDdl As DropDownList = valControl.NamingContainer.FindControl("ddlExtraName")
        Dim cExtra As ExtraProductLouvres = _Service.GetExtraProductLouvresByID(extrasDdl.SelectedValue)

        ' Check against its max length
        If cExtra.ExtraProductID > 0 Then
            intUnitOfMeasurement = cExtra.UnitOfMeasurement
        End If

        If intUnitOfMeasurement <> 0 Then
            If (Not IsNumeric(args.Value)) OrElse (args.Value <= 0) Then
                args.IsValid = False
                valControl.Text = "<br />Value is not valid."
            ElseIf (intUnitOfMeasurement > 0) AndAlso (args.Value > intUnitOfMeasurement) Then
                args.IsValid = False
                valControl.Text = "<br />Length exceeds maximum of " & intUnitOfMeasurement & "."
            End If
        Else
            If (Not IsNumeric(args.Value)) OrElse (args.Value <> 0) Then
                args.IsValid = False
                valControl.Text = "<br />Length must be 0."
            End If
        End If

    End Sub

    Private Sub EnableDisableDelivery(boolEnable As Boolean)

        'If radDelivery.Checked = True Then
        '    boolEnable = True

        'End If
        ' Only clear the address zone if it is not overridden.
        If ViewState("AddressZoneID") <> -1 Then
            ViewState("AddressZoneID") = 0
        End If

        If cboDeliveryAddress.Items.Count > 0 Then
            cboDeliveryAddress.SelectedIndex = 0
        End If

        If boolEnable Then
            radDelivery.Checked = True
            radPickup.Checked = False
            pnlDeliveryAddress.Visible = True

            txtFreightTotal.Text = 0
            pnlFreightTotal.Visible = True
        Else
            radPickup.Checked = True
            radDelivery.Checked = False
            pnlDeliveryAddress.Visible = False
            txtState.Text = String.Empty

            txtFreightTotal.Text = 0
            OverrideFreightSave()
            pnlFreightTotal.Visible = False
        End If
    End Sub

#Region "Costing"

    Private Sub RecalculateProductionScheduleExtrasCostFromDB()
        Dim lExtras As List(Of LouvreExtraProduct) = ViewState("ProductionScheduleExtras")

        For Each e As LouvreExtraProduct In lExtras
            If Not e.LouvreExtraPriceIsOverridden Then
                Dim boolIsValid As Boolean = False

                ' Sets extra object properties internally.
                _Costing.GetExtraPriceOrPercentageFromDB(e, boolIsValid)

                ' Always recost postage when any louvredetails/extras have changed.
                RecalculateUIFreightFromDB()
            End If
        Next e

        LoadProductionScheduleExtrasIntoDataGrid()
    End Sub

    Private Sub RecalculateLouvreDetailsCostFromDBMatrix(cLouvreDetails As LouvreDetails, cExtras As List(Of LouvreExtraProduct))

        If Not cLouvreDetails.LouvrePriceIsOverridden Then
            Dim cLouvrePriceFound As LouvrePrice = Nothing
            Dim lExtraPriceIDsNotFound As List(Of ExtraProductLouvresEnum) = Nothing

            ' This function now modifies the object values (prices, priceID's, etc).
            _Costing.GetLouvrePrice(cLouvreDetails, cExtras, cLouvrePriceFound, lExtraPriceIDsNotFound)

            ' Always recost postage when any louvredetails/extras have changed.
            RecalculateUIFreightFromDB()
        End If
    End Sub

    Private Sub RecalculateUIFreightFromDB()

        If ViewState("AddressZoneID") <> -1 Then
            Dim cLouvreDetailsCollection As LouvreDetailsCollection = ViewState("LouvreDetails")

            Dim decFreightPrice As Decimal = 0

            txtState.Text = String.Empty
            txtFreightTotal.Text = FormatCurrency(0)

            If radDelivery.Checked Then
                Dim cAddressZoneFound As AddressZone = Nothing
                Dim boolSalesPriceValid As Boolean = False

                If IsNumeric(cboDeliveryAddress.SelectedValue) Then
                    decFreightPrice = _Costing.GetFreightPriceByAddressIDFromDB(cboDeliveryAddress.SelectedValue, cLouvreDetailsCollection.TotalSalesPrice(boolSalesPriceValid), cAddressZoneFound)
                End If

                If cAddressZoneFound IsNot Nothing Then
                    ViewState("AddressZoneID") = cAddressZoneFound.ID
                    txtFreightTotal.Text = FormatCurrency(decFreightPrice)
                Else
                    ViewState("AddressZoneID") = 0
                    txtFreightTotal.Text = _Costing.NoPriceMsg
                End If
            Else
                decFreightPrice = 0
                txtFreightTotal.Text = FormatCurrency(0)
            End If

        End If

        If IsNumeric(cboDeliveryAddress.SelectedValue) Then
            Dim cAddress As Address = _Service.GetAddressByID(cboDeliveryAddress.SelectedValue)

            If cAddress IsNot Nothing Then
                txtState.Text = cAddress.State
            End If
        End If
    End Sub

    Protected Sub UpdateFreightPriceUIFromSavedProdSchedule(cProdSchedule As ProductionSchedule)

        txtState.Text = String.Empty

        If cProdSchedule.IsPickup AndAlso Not cProdSchedule.FreightPriceIsOverridden Then
            txtFreightTotal.Text = FormatCurrency(0)
        Else
            If cProdSchedule.FreightPriceIsValid Then
                ViewState("AddressZoneID") = cProdSchedule.AddressZoneID
                txtFreightTotal.Text = FormatCurrency(cProdSchedule.FreightAmount)

            Else
                ViewState("AddressZoneID") = 0
                txtFreightTotal.Text = _Costing.NoPriceMsg
            End If
        End If

        ' Set delivery address
        Dim cAddress As Address = _Service.GetAddressByID(cProdSchedule.DeliveryAddressID)

        If cAddress IsNot Nothing Then
            txtState.Text = cAddress.State
        End If

        cboDeliveryAddress.SelectedValue = cProdSchedule.DeliveryAddressID
    End Sub

    Protected Sub UpdateSalePriceUI()

        Dim cLouvreDetailsCollection As LouvreDetailsCollection = ViewState("LouvreDetails")
        Dim lExtras As List(Of LouvreExtraProduct) = ViewState("ProductionScheduleExtras")
        Dim boolTotalPriceIsValid As Boolean = False

        Dim decTotalPrice As Decimal = _Costing.GetTotalPriceForProductionSchedule(cLouvreDetailsCollection, lExtras, boolTotalPriceIsValid)

        If boolTotalPriceIsValid Then
            txtSalePrice.Text = FormatCurrency(decTotalPrice)
        Else
            txtSalePrice.Text = _Costing.NoPriceMsg
        End If
    End Sub

    Protected Function GetLouvreSalePriceForRow(cLouvreDetails As LouvreDetails) As String
        ' Do NOT do a matrix lookup for price. Use existing LouvreDetails data.

        If cLouvreDetails.LouvrePriceIsValid Then
            Return FormatCurrency(cLouvreDetails.SalePrice)
        Else
            Return _Costing.NoPriceMsg
        End If
    End Function

    Protected Function GetExtraSalePriceForRow(cExtra As LouvreExtraProduct) As String
        ' Do NOT do a matrix lookup for price. Use existing LouvreExtraProduct data.

        If cExtra.LouvreExtraPriceIsValid Then
            If cExtra.PriceIsPercentage Then
                Return FormatPercent(cExtra.SalePrice)
            Else
                Return FormatCurrency(cExtra.SalePrice)
            End If
        Else
            Return _Costing.NoPriceMsg
        End If
    End Function

#End Region

    Protected Function GetColourNameForID(cLouvreExtraProduct As LouvreExtraProduct) As String

        If cLouvreExtraProduct.ColourID > 0 Then
            ' Cache the colours if not already.
            If _CacheColours.ItemCount = 0 Then
                _CacheColours.CacheAllColours(_Service)
            End If

            Dim cColour As Colour = _CacheColours.ItemByID(cLouvreExtraProduct.ColourID)

            If cColour.ID > 0 Then
                Return cColour.Name
            End If
        End If

        Return String.Empty
    End Function

    Protected Function GetColourNameVisibleForExtra(cLouvreExtraProduct As LouvreExtraProduct) As String

        ' Cache the extras if not already.
        If _CacheExtras.ItemCount = 0 Then
            _CacheExtras.CacheAllExtras(_Service)
        End If

        Dim cExtra As ExtraProductLouvres = _CacheExtras.ItemByID(cLouvreExtraProduct.ExtraProductID)

        If cExtra.ExtraProductID > 0 Then
            Return cExtra.AppendColourCode
        End If

        Return False
    End Function

    Protected Function GetCutLengthVisibleForExtra(cLouvreExtraProduct As LouvreExtraProduct) As String

        ' Cache the extras if not already.
        If _CacheExtras.ItemCount = 0 Then
            _CacheExtras.CacheAllExtras(_Service)
        End If

        Dim cExtra As ExtraProductLouvres = _CacheExtras.ItemByID(cLouvreExtraProduct.ExtraProductID)

        If cExtra.ExtraProductID > 0 Then
            Return (cExtra.UnitOfMeasurement > 0)
        End If

        Return False
    End Function

    Protected Function GetSalePriceDetailToolTipText(cLouvreDetails As LouvreDetails) As String

        If cLouvreDetails.LouvrePriceIsOverridden Then
            Return _PRICE_OVERRIDDEN_MSG
        Else
            Return _PRICE_AUTO_CALCULATED_MSG
        End If
    End Function

    Protected Function GetSalePriceDetailToolTipText(cExtra As LouvreExtraProduct) As String

        If cExtra.LouvreExtraPriceIsOverridden Then
            Return _PRICE_OVERRIDDEN_MSG
        Else
            Return _PRICE_AUTO_CALCULATED_MSG
        End If
    End Function

    Protected Function GetSalePriceTextColour(cLouvreDetails As LouvreDetails) As Color

        If cLouvreDetails.LouvrePriceIsOverridden Then
            Return Color.Red
        Else
            Return Color.Black
        End If
    End Function

    Protected Function GetSalePriceTextColour(cExtra As LouvreExtraProduct) As Color

        If cExtra.LouvreExtraPriceIsOverridden Then
            Return Color.Red
        Else
            Return Color.Black
        End If
    End Function

    Protected Function UserHasPriceOverridePermission() As Boolean
        Return _UserHasSalePriceOverridePermission
    End Function

    Protected Function UserHasCreditCheckOverridePermission() As Boolean
        Return _UserHasCreditCheckOverridePermission
    End Function

    Protected Function FreightPriceIsOverridden() As Boolean
        Return ViewState("AddressZoneID") = -1
    End Function

    Private Sub SetFreightPriceOverrideControlVisibility()
        Dim enumStatus As ProductionScheduleStatus = CalculateStatus()

        btnFreightPriceOverride.Visible = ((enumStatus > ProductionScheduleStatus.Quote) AndAlso UserHasPriceOverridePermission() AndAlso (Not FreightPriceIsOverridden()))
        btnFreightPriceOverrideRemove.Visible = ((enumStatus > ProductionScheduleStatus.Quote) AndAlso UserHasPriceOverridePermission() AndAlso FreightPriceIsOverridden())

        If ViewState("AddressZoneID") = -1 Then
            txtFreightTotal.ForeColor = Color.Red
            txtFreightTotal.ToolTip = _PRICE_OVERRIDDEN_MSG
        Else
            txtFreightTotal.ForeColor = Color.Black
            txtFreightTotal.ToolTip = _PRICE_AUTO_CALCULATED_MSG
        End If

    End Sub

    Private Sub radDelivery_CheckedChanged(sender As Object, e As EventArgs) Handles radDelivery.CheckedChanged

        RemoveFreightOverride()
        EnableDisableDelivery(True)
        RecalculateUIFreightFromDB()
        'Added by surendra 16-aug-2020
        'If cboDeliveryAddress.Items.Count > 0 Then
        '    cboDeliveryAddress.SelectedIndex = 1
        'End If

    End Sub

    Private Sub radPickup_CheckedChanged(sender As Object, e As EventArgs) Handles radPickup.CheckedChanged
        EnableDisableDelivery(False)
        RecalculateUIFreightFromDB()
    End Sub

    Private Sub PopulateShutterTypeDDL(intSelectedValue As Integer)

        Dim cLouvreTypes As List(Of LouvreType) = _Service.GetLouvreTypes().FindAll(Function(x) x.Discontinued = False)

        cLouvreTypes.RemoveAll(Function(x) x.ID = LouvreTypes.PrivacyScreen)

        If cboLouvreProd.SelectedValue = LouvreStyles.CL Then
            ' Hinged is not an option with CL
            cLouvreTypes.RemoveAll(Function(x) x.ID = LouvreTypes.Hinged2Panels OrElse x.ID = LouvreTypes.HingedPanelLeft OrElse x.ID = LouvreTypes.HingedPanelRight)
        End If

        cboLouvreType.Items.Clear()
        cboLouvreType.Items.Add(New ListItem(String.Empty, 0))

        For Each t As LouvreType In cLouvreTypes
            Dim item = New ListItem(t.Name, t.ID)

            cboLouvreType.Items.Add(item)
        Next t
    End Sub

    Private Sub gdvOpeningExtras_DataBound(sender As Object, e As EventArgs) Handles gdvOpeningExtras.DataBound
        '
    End Sub

    Private Sub gdvOpeningExtras_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gdvOpeningExtras.RowCancelingEdit
        gdvOpeningExtras.EditIndex = -1

        ' load changes from the viewstate datatable
        Dim lDetailIDToExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")
        Dim lExtras As List(Of LouvreExtraProduct) = lDetailIDToExtras(txtHiddenPSDetailID.Text)

        ' Remove the blank row. Will be the last row added.
        If ViewState("IsAddExtra") = True Then
            If lExtras.Count > 0 Then
                lExtras.RemoveAt(lExtras.Count - 1)
            End If
        End If

        ' Populate the table from the viewstate datatable
        gdvOpeningExtras.DataSource = lExtras
        gdvOpeningExtras.DataBind()

        ToggleAddExtraControls(gdvOpeningExtras, True)

        ViewState("IsAddExtra") = False
    End Sub

    Private Sub gdvOpeningExtras_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvOpeningExtras.RowCommand
        If e.CommandName = "DeleteExtraItem" Then
            Dim lDetailIDToExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")
            Dim lExtras As List(Of LouvreExtraProduct) = lDetailIDToExtras(txtHiddenPSDetailID.Text)

            Dim cExtraToRemove As LouvreExtraProduct = lExtras.Find(Function(x) x.ID = CInt(e.CommandArgument))

            If cExtraToRemove IsNot Nothing Then
                lExtras.Remove(cExtraToRemove)
            End If

            gdvOpeningExtras.DataSource = lExtras
            gdvOpeningExtras.DataBind()
        End If
    End Sub

    Private Sub grdOpeningExtras_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvOpeningExtras.RowDataBound
        Dim idLabel As Label = DirectCast(e.Row.Cells(0).FindControl("lblID"), Label)

        If idLabel IsNot Nothing Then
            Dim lExtras As List(Of LouvreExtraProduct) = DirectCast(sender.DataSource, List(Of LouvreExtraProduct))
            Dim cExtra As LouvreExtraProduct = lExtras.Find(Function(x) x.ID = idLabel.Text)

            If cExtra IsNot Nothing Then
                ' Populate extras dll for each row
                Dim extrasddl As DropDownList = DirectCast(e.Row.FindControl("ddlExtraName"), DropDownList)

                If Not extrasddl Is Nothing Then
                    PopulateExtrasDDL(extrasddl, ExtraProductVisibilityLevel.Details)

                    ' Set the style drop down vale from the dataset.
                    extrasddl.SelectedValue = cExtra.ExtraProductID
                End If
                'added by surendra on 12/08/2020
                If CInt(Request.Params("ScheduleID")) > 0 And Convert.ToInt32(Session("customerId")) > 0 Then
                    Dim btnEdit As Button = DirectCast(e.Row.FindControl("btnEditExtra"), Button)
                    Dim btnDelete As Button = DirectCast(e.Row.FindControl("btnDeleteExtra"), Button)
                    btnEdit.Enabled = False
                    btnDelete.Enabled = False


                End If
                ' Set length visibility


            End If
        End If

    End Sub

    Private Sub gdvOpeningExtras_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gdvOpeningExtras.RowEditing
        gdvOpeningExtras.EditIndex = e.NewEditIndex

        ' load changes from the viewstate
        Dim lDetailIDToExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")
        Dim lExtras As List(Of LouvreExtraProduct) = lDetailIDToExtras(txtHiddenPSDetailID.Text)

        ' Populate the table from the viewstate datatable
        gdvOpeningExtras.DataSource = lExtras
        gdvOpeningExtras.DataBind()

        ToggleAddExtraControls(gdvOpeningExtras, False)
    End Sub

    Private Sub gdvOpeningExtras_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gdvOpeningExtras.RowUpdating
        If IsValidationGroupValid("extras") Then
            ' Save changes to the viewstate extras
            Dim lDetailIDToExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")
            Dim lExtras As List(Of LouvreExtraProduct) = lDetailIDToExtras(txtHiddenPSDetailID.Text)
            Dim row As GridViewRow = gdvOpeningExtras.Rows(e.RowIndex)

            Dim intID As Integer = DirectCast(row.FindControl("lblID"), Label).Text
            Dim intExtraID As String = DirectCast(row.FindControl("ddlExtraName"), DropDownList).SelectedValue
            Dim intQty As String = DirectCast(row.FindControl("txtQty"), TextBox).Text.Trim()
            Dim strCutLength As String = DirectCast(row.FindControl("txtCutLength"), TextBox).Text.Trim()

            ' Find the ID of the record in the list
            Dim cExtra As LouvreExtraProduct = lExtras.Find(Function(x) x.ID = intID)

            If cExtra IsNot Nothing Then
                With cExtra
                    ' Update values
                    .ExtraProductID = intExtraID
                    .Quantity = intQty
                    .CutLength = strCutLength
                End With
            End If

            ' Finished editing
            gdvOpeningExtras.EditIndex = -1

            gdvOpeningExtras.DataSource = lExtras
            gdvOpeningExtras.DataBind()

            ToggleAddExtraControls(gdvOpeningExtras, True)

            ViewState("IsAddExtra") = False

        End If
    End Sub

    Private Sub PopulateExtrasTable(intLouvreDetailID As Integer)
        Dim dicExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")

        Dim lExtras As List(Of LouvreExtraProduct) = Nothing

        If dicExtras.TryGetValue(intLouvreDetailID, lExtras) Then
            gdvOpeningExtras.DataSource = lExtras
        Else
            ' Create empty list.
            lExtras = New List(Of LouvreExtraProduct)
            gdvOpeningExtras.DataSource = lExtras

            ' Add to dictionary.
            dicExtras.Add(intLouvreDetailID, lExtras)
        End If

        gdvOpeningExtras.DataBind()

    End Sub

    Private Sub btnAddExtra_Click(sender As Object, e As EventArgs) Handles btnAddExtra.Click
        ViewState("IsAddExtra") = True

        ' Add new row to viewstate datatable
        Dim lDetailIDToExtras As Dictionary(Of Integer, List(Of LouvreExtraProduct)) = ViewState("LouvreDetailExtras")
        Dim lExtras As List(Of LouvreExtraProduct) = lDetailIDToExtras(txtHiddenPSDetailID.Text)

        Dim newExtra As New LouvreExtraProduct

        ' Allocate a unique negative ID
        If lExtras.Any Then
            newExtra.ID = lExtras.Min(Function(x) x.ID) - 1

            If newExtra.ID >= 0 Then
                newExtra.ID = -1
            End If
        Else
            newExtra.ID = -1
        End If

        newExtra.LouvreDetailsID = txtHiddenPSDetailID.Text
        newExtra.ProductionScheduleID = txtIntScheduleID.Text

        lExtras.Add(newExtra)

        gdvOpeningExtras.DataSource = lExtras
        gdvOpeningExtras.DataBind()

        ' Set new row to edit mode.
        gdvOpeningExtras.EditIndex = gdvOpeningExtras.Rows.Count - 1

        ' Have to rebind to get the edit of the new row to trigger.
        gdvOpeningExtras.DataBind()

        ToggleAddExtraControls(gdvOpeningExtras, False)
    End Sub

    Private Sub ToggleAddExtraControls(cGridView As GridView, boolEnable As Boolean)

        btnAddExtra.Enabled = boolEnable
        btnSaveDetails.Enabled = boolEnable
        btnCancelDetails.Enabled = boolEnable

        If boolEnable Then
            btnAddExtra.CssClass = btnAddExtra.CssClass.Replace(" form-button-disabled", String.Empty)
            btnSaveDetails.CssClass = btnSaveDetails.CssClass.Replace(" form-button-disabled", String.Empty)
            btnCancelDetails.CssClass = btnCancelDetails.CssClass.Replace(" form-button-disabled", String.Empty)
        Else
            btnAddExtra.CssClass &= " form-button-disabled"
            btnSaveDetails.CssClass &= " form-button-disabled"
            btnCancelDetails.CssClass &= " form-button-disabled"
        End If

        ToggleDisabledGridViewRowButtons(cGridView, New List(Of String) From {"btnEditExtra", "btnDeleteExtra"}, boolEnable)

        pnlupdateSaveCancel.Update()
    End Sub

    ''' <summary>
    ''' Hides all buttons outside of the editing row.
    ''' </summary>
    ''' <param name="gGridView">The <see cref="GridView"/> to use.</param>
    ''' <param name="lButtonIDs">The button control ID strings to enable/disable.</param>
    ''' <param name="boolEnabled">Disable or enable the buttons.</param>
    Shared Sub ToggleDisabledGridViewRowButtons(gGridView As GridView, lButtonIDs As List(Of String), boolEnabled As Boolean)

        For Each r As GridViewRow In gGridView.Rows
            If r.RowIndex <> gGridView.SelectedIndex Then
                For Each id As String In lButtonIDs
                    For Each cell As TableCell In r.Cells
                        Dim cControl As WebControl = cell.FindControl(id)

                        If Not cControl Is Nothing Then
                            If boolEnabled Then
                                cControl.Enabled = True
                            Else
                                cControl.Enabled = False
                            End If
                        End If
                    Next cell
                Next id
            End If
        Next r
    End Sub

    Public Sub PopulateExtrasDDL(dll As DropDownList, enumVisibilityLevel As ExtraProductVisibilityLevel)
        Dim cExtraProduct As List(Of ExtraProductLouvres) = _Service.GetExtraProductLouvresByVisibility(ExtraProductLouvresPageVisibility.OZRoll_LouvreJobDetails,
                                                                                                        enumVisibilityLevel,
                                                                                                        False).OrderBy(Function(x) x.Description).ToList()
        dll.Items.Clear()
        dll.Items.Add(New ListItem(String.Empty, 0))

        For Each p As ExtraProductLouvres In cExtraProduct

            'Added By Michael Behar - 18-02-2021 - Mark Griffiths Email On This Day
            If p.Description.ToLower.Contains("discount") Then
                If Session("CustomerID") = 0 Then
                    dll.Items.Add(New ListItem(p.Description, p.ExtraProductID))
                End If
            Else
                dll.Items.Add(New ListItem(p.Description, p.ExtraProductID))
            End If

        Next p

    End Sub

    Protected Sub LoadAdditionaRequirementsFromDB()
        Dim lRequirements As List(Of AdditionalRequirements) = Nothing
        Dim lPowderCoating As List(Of AdditionalRequirements) = Nothing
        Dim intScheduleID As Integer = SharedConstants.DEFAULT_INTEGER_VALUE

        If txtId.Text <> String.Empty Then
            intScheduleID = CInt(txtId.Text)



            lRequirements = _Service.GetAdditionalRequirementsListByProductionScheduleID(intScheduleID)

            If lRequirements IsNot Nothing Then
                lPowderCoating = lRequirements.FindAll(Function(x) x.AdditionalRequirementTypeID = AdditionalRequirementTypeID.PowderCoating)
                lRequirements = lRequirements.FindAll(Function(x) x.AdditionalRequirementTypeID <> AdditionalRequirementTypeID.PowderCoating)
            End If

        End If

        If lPowderCoating Is Nothing Then
            lPowderCoating = New List(Of AdditionalRequirements)
        End If

        If lRequirements Is Nothing Then
            lRequirements = New List(Of AdditionalRequirements)
        End If

        ViewState("Requirements") = lRequirements
        ViewState("PowderCoating") = lPowderCoating
    End Sub

    Protected Sub LoadLouvreDetailsFromDB()
        Dim cLouvreDetails As New LouvreDetailsCollection
        Dim intScheduleID As Integer = SharedConstants.DEFAULT_INTEGER_VALUE

        If txtId.Text <> String.Empty Then
            intScheduleID = CInt(txtId.Text)

            cLouvreDetails = _Service.GetLouvreDetailsCollectionByProductionScheduleID(intScheduleID)
        End If

        ViewState("LouvreDetails") = cLouvreDetails
        LoadLouvreDetailExtrasFromDB()
    End Sub

    Private Sub LoadLouvreDetailExtrasFromDB()
        Dim cLouvreDetailsCollection As LouvreDetailsCollection = ViewState("LouvreDetails")

        ' Map LouvreDetailIDs to Extras list and store in viewstate.
        Dim LouvreDetailIDToExtra As New Dictionary(Of Integer, List(Of LouvreExtraProduct))

        For Each l As LouvreDetails In cLouvreDetailsCollection
            Dim lExtras As List(Of LouvreExtraProduct) = _Service.GetLouvreExtraProductsListByLouvreDetailsID(l.LouvreDetailID)

            LouvreDetailIDToExtra.Add(l.LouvreDetailID, lExtras)
        Next

        ViewState("LouvreDetailExtras") = LouvreDetailIDToExtra
    End Sub

    Private Sub LoadProductionScheduleExtrasFromDB()
        Dim lExtras As List(Of LouvreExtraProduct) = Nothing

        If txtIntScheduleID.Text > 0 Then
            lExtras = _Service.GetLouvreExtraProductsAtProductionScheduleLevelByPSID(txtIntScheduleID.Text)
        Else
            lExtras = New List(Of LouvreExtraProduct)
        End If

        ViewState("ProductionScheduleExtras") = lExtras
    End Sub

    Private Sub cboMakeOpenSize_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMakeOpenSize.SelectedIndexChanged
        Dim intWidth As Integer = 0
        Dim cLouvreDetails As LouvreDetails = PopulateLouvreDetailsFromUI()

        If IsNumeric(txtWidth.Text) Then
            intWidth = txtWidth.Text
        End If

        If String.IsNullOrEmpty(cboNoOfPanels.SelectedValue) Then
            _RulesLouvreDetails.PopulateNoOfPanelsDDL(cLouvreDetails, cboNoOfPanels, cboMakeOpenSize.SelectedValue, cboLouvreType.SelectedValue, intWidth, 0)
        Else
            _RulesLouvreDetails.PopulateNoOfPanelsDDL(cLouvreDetails, cboNoOfPanels, cboMakeOpenSize.SelectedValue, cboLouvreType.SelectedValue, intWidth, cboNoOfPanels.SelectedValue)
        End If

        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub valcustPanelHeight_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valcustPanelHeight.ServerValidate
        Dim strErrorMsg As String = String.Empty
        Dim cValidator As CustomValidator = DirectCast(source, CustomValidator)
        Dim intHeight As Integer = 0
        Dim cLouvreDetails As LouvreDetails = PopulateLouvreDetailsFromUI()

        If IsNumeric(txtHeight.Text) Then
            intHeight = txtHeight.Text
        End If

        args.IsValid = _RulesLouvreDetails.PanelHeightIsValid(cLouvreDetails, intHeight, cboLouvreType.SelectedValue, cboMakeOpenSize.SelectedValue, strErrorMsg)

        If Not args.IsValid Then
            cValidator.ErrorMessage = strErrorMsg
        End If
    End Sub

    Protected Sub valcustPanelWidth_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valcustPanelWidth.ServerValidate
        Dim strErrorMsg As String = String.Empty
        Dim cValidator As CustomValidator = DirectCast(source, CustomValidator)
        Dim intWidth As Integer = 0
        Dim cLouvreDetails As LouvreDetails = PopulateLouvreDetailsFromUI()

        If IsNumeric(txtWidth.Text) Then
            intWidth = txtWidth.Text
        End If
        'Added by surendra ticket no#62428
        args.IsValid = _RulesLouvreDetails.ValidateMaxWidth(intWidth, cboLouvreType.SelectedValue, cboLouvreProd.SelectedValue, strErrorMsg)
        If args.IsValid = True Then
            args.IsValid = _RulesLouvreDetails.PanelWidthIsValid(cLouvreDetails, intWidth, cboLouvreType.SelectedValue, cboMakeOpenSize.SelectedValue, strErrorMsg)

        End If

        If Not args.IsValid Then
            cValidator.ErrorMessage = strErrorMsg
        End If
    End Sub

    Private Sub EnableDisableDetailsPopupUI()

        ' This is the complete UI evaluating function. Some previous functionality is outside this sub still.

        ' PANEL MID RAIL

        Dim intPanelHeight As Integer = 0

        If IsNumeric(txtHeight.Text) Then
            intPanelHeight = txtHeight.Text
        End If

        If _RulesLouvreDetails.CanSetMidRail(intPanelHeight) Then
            pnlPanelMidRail.Visible = True
        Else
            pnlPanelMidRail.Visible = False
            cboPanelMidRail.SelectedValue = 0
        End If

        ' MIDRAIL

        _RulesLouvreDetails.PopulatePanelMidRailDDL(cboPanelMidRail, cboLouvreProd.SelectedValue, cboInsertTop.SelectedValue, cboInsertBottom.SelectedValue, cboPanelMidRail.SelectedValue)

        pnlMidRailHeight.Visible = False

        If _RulesLouvreDetails.CanSetMidRailHeight(cboPanelMidRail.SelectedValue) Then
            If txtMidRailHeight.Text = String.Empty Then
                SetDefaultMidrailHeight()
            End If

            pnlMidRailHeight.Visible = True
        End If

        If Not pnlMidRailHeight.Enabled Then
            txtMidRailHeight.Text = String.Empty
        End If

        valrfMidRailHeight.Enabled = (pnlMidRailHeight.Visible AndAlso (intPanelHeight > _RulesLouvreDetails.MidRailRequiredHeight))

        ' TOP INSERT

        ' Change the text on the top insert to main insert as assume by default there is no top and bottom.
        lblMainInsert.Text = "Main Insert"

        If _RulesLouvreDetails.CanSetTopInsert(cboLouvreType.SelectedValue) Then
            pnlInsertTop.Visible = True
        Else
            pnlInsertTop.Visible = False
            cboInsertTop.SelectedValue = 0
        End If

        valrfInsertTop.Enabled = pnlInsertTop.Visible

        ' TOP OPERATION

        ' Change the text on the top operation to main operation as assume by default there is no top and bottom.
        lblOperationMain.Text = "Main Operation"

        If _RulesLouvreDetails.CanSetTopOperation(cboLouvreType.SelectedValue) Then
            pnlBladeOperation.Visible = True
        Else
            pnlBladeOperation.Visible = False
            cboBladeOperation.SelectedValue = 0
        End If

        valrfBladeOperation.Enabled = pnlBladeOperation.Visible

        ' BOTTOM INSERT

        Dim intMidRailHeight = 0

        If IsNumeric(txtMidRailHeight.Text) Then
            intMidRailHeight = txtMidRailHeight.Text
        End If

        If _RulesLouvreDetails.CanSetBottomInsert(intMidRailHeight) Then

            ' Change the text on the main insert to top insert as there is a top and bottom now.
            lblOperationMain.Text = "Top Operation"
            lblMainInsert.Text = "Top Insert"

            pnlInsertBottom.Visible = True
        Else
            pnlInsertBottom.Visible = False
            cboInsertBottom.SelectedValue = 0
        End If

        valrfInsertBottom.Enabled = pnlInsertBottom.Visible

        ' BOTTOM OPERATION

        If _RulesLouvreDetails.CanSetBottomOperation(intMidRailHeight) Then
            pnlBladeOperationBottom.Visible = True
        Else
            pnlBladeOperationBottom.Visible = False
            cboBladeOperationBottom.SelectedValue = 0
        End If

        valrfBladeOperationBottom.Enabled = pnlBladeOperationBottom.Visible

        ' LOUVRE DIRECTION

        EnableDisableLouvreDirectionPanel()

        ' OPEN IN/OUT

        If _RulesLouvreDetails.CanSetBifoldHingedDoorOpenInOut(cboLouvreType.SelectedValue) Then
            pnlBiFoldHingedDoor.Visible = True
        Else
            pnlBiFoldHingedDoor.Visible = False
            cboBiFoldHingedDoor.SelectedValue = 0
        End If

        valrfBiFoldHingedDoor.Enabled = pnlBiFoldHingedDoor.Visible

        ' EXTRA TRACK

          'Below section commented By pradeep singh against ticket #65726(point 2nd) and #65756 (5th Point)	
           pnlExtraTrack.Visible = False
    '    If _RulesLouvreDetails.CanSetExtraTrack(cboLouvreType.SelectedValue) Then
    '        'pnlExtraTrack.Visible = True        
    '    Else
    '      pnlExtraTrack.Visible = False
    '        txtExtraTrack.Text = String.Empty
    '    End If

        ' STACKER

        RemoveControlsFormClasses(cboStacker)

        If _RulesLouvreDetails.CanSetStacker(cboLouvreType.SelectedValue) Then
            pnlStacker.Visible = True
        Else
            pnlStacker.Visible = False
            cboStacker.SelectedValue = 0
        End If

        valrfStacker.Enabled = pnlStacker.Visible

        ' FLUSH BOLTS TOP

        If _RulesLouvreDetails.CanSetFlushBoltTop(cboLouvreType.SelectedValue, cboLockOptions.SelectedValue) Then
            pnlFlushBoltsTop.Visible = True
        Else
            pnlFlushBoltsTop.Visible = False
            cboFlushBoltsTop.SelectedValue = 0
        End If

        ' FLUSH BOLTS BOTTOM

        If _RulesLouvreDetails.CanSetFlushBoltBottom(cboLouvreType.SelectedValue, cboLockOptions.SelectedValue) Then
            pnlFlushBoltsBottom.Visible = True
        Else
            pnlFlushBoltsBottom.Visible = False
            cboFlushBoltsBottom.SelectedValue = 0
        End If

        ' FLUSH BOLT POSITION

        If _RulesLouvreDetails.CanSetFlushBoltPosition(cboLouvreType.SelectedValue, cboFlushBoltsTop.SelectedValue, cboFlushBoltsBottom.SelectedValue) Then
            pnlFlushBoltsPosition.Visible = True
        Else
            pnlFlushBoltsPosition.Visible = False
            cboFlushBoltsPosition.SelectedValue = 0
        End If

        valrfFlushBoltsPosition.Enabled = pnlFlushBoltsPosition.Visible

        ' LOCK OPTIONS

        If _RulesLouvreDetails.CanSetLockOption(cboLouvreType.SelectedValue, cboFlushBoltsTop.SelectedValue, cboFlushBoltsBottom.SelectedValue) Then
            pnlLockOptions.Visible = True
        Else
            pnlLockOptions.Visible = False
            cboLockOptions.SelectedValue = 0
        End If

        ' CURVED TRACK

        If _RulesLouvreDetails.CanSetCurvedTrack(cboLouvreType.SelectedValue) Then
            pnlCurvedTrack.Visible = True
        Else
            pnlCurvedTrack.Visible = False
            cboCurvedTrack.SelectedValue = 0
        End If

        ' CURVED TRACK MAX DEFLECTION

        If _RulesLouvreDetails.CanSetCurvedTrackMaxDeflection(cboCurvedTrack.SelectedValue) Then
            pnlCurvedTrackMaxDeflection.Visible = True
        Else
            pnlCurvedTrackMaxDeflection.Visible = False
            txtCurvedTrackMaxDeflection.Text = String.Empty
        End If

        valcustCurvedTrackMaxDeflection.Enabled = pnlCurvedTrackMaxDeflection.Visible

        ' TOP TRACK

        _RulesLouvreDetails.PopulateTopTrackDDL(cboTopTrackType, cboLouvreType.SelectedValue, cboTopTrackType.SelectedValue)

        If _RulesLouvreDetails.CanSetTopTrack(cboLouvreType.SelectedValue, cboLouvreProd.SelectedValue, TopTrackTypes.NONE) Then
            pnlTopTrackType.Visible = True
        Else
            pnlTopTrackType.Visible = False
            cboTopTrackType.SelectedValue = 0
        End If

        valrfTopTrackType.Enabled = pnlTopTrackType.Visible

        ' BOTTOM TRACK

        If _RulesLouvreDetails.CanSetBottomTrack(cboLouvreType.SelectedValue) Then
            pnlBottomTrackType.Visible = True
        Else
            pnlBottomTrackType.Visible = False
            cboBottomTrackType.SelectedValue = 0
        End If

        valrfBottomTrackType.Enabled = pnlBottomTrackType.Visible

        ' L REVEAL

        If _RulesLouvreDetails.CanSetLReveal(cboLouvreType.SelectedValue, cboZReveal.SelectedValue) Then
            pnlLReveal.Visible = True
        Else
            pnlLReveal.Visible = False
            cboLReveal.SelectedValue = 0
        End If

        valrfLReveal.Enabled = pnlLReveal.Visible

        ' Z REVEAL

        If _RulesLouvreDetails.CanSetZReveal(cboLouvreType.SelectedValue, cboLReveal.SelectedValue) Then
            pnlZReveal.Visible = True
        Else
            pnlZReveal.Visible = False
            cboZReveal.SelectedValue = 0
        End If

        valrfZReveal.Enabled = pnlZReveal.Visible

        ' H CHANNEL

        If _RulesLouvreDetails.CanSetHChannel(cboLouvreType.SelectedValue) Then
            pnlHChannel.Visible = True
        Else
            pnlHChannel.Visible = False
            cboHChannel.SelectedValue = 0
        End If

        valrfHChannel.Enabled = pnlHChannel.Visible

        ' C CHANNEL (fixed panel channel)

        If _RulesLouvreDetails.CanSetCChannel(cboLouvreType.SelectedValue) Then
            pnlCChannel.Visible = True
        Else
            pnlCChannel.Visible = False
            cboCChannel.SelectedValue = 0
        End If

        valrfCChannel.Enabled = pnlCChannel.Visible

        ' FIXED PANEL SIDES

        If _RulesLouvreDetails.CanSetFixedPanelSides(cboLouvreType.SelectedValue) Then
            pnlFixedPanelSides.Visible = True
        Else
            pnlFixedPanelSides.Visible = False
            cboFixedPanelSides.SelectedValue = 0
        End If

        ' WINDER

        If _RulesLouvreDetails.CanSetWinder(cboLouvreType.SelectedValue) Then
            pnlWinder.Visible = True
        Else
            pnlWinder.Visible = False
            cboWinder.SelectedValue = 0
        End If

        ' SLIDE

        If _RulesLouvreDetails.CanSetSlide(cboLouvreType.SelectedValue) Then
            pnlSlide.Visible = True
        Else
            pnlSlide.Visible = False
            cboSlide.SelectedValue = 0
        End If

        valrfSlide.Enabled = pnlSlide.Visible

        ' STACKER LOCATION

        If _RulesLouvreDetails.CanSetStacker(cboLouvreType.SelectedValue) Then
            pnlStacker.Visible = True
        Else
            pnlStacker.Visible = False
            cboStacker.SelectedValue = 0
        End If

        valrfStacker.Enabled = pnlStacker.Visible

        EnableDisableStackerBayLocationItems()

        ' BLADE LOCKS

        If _RulesLouvreDetails.CanSetBladeLocks(cboLouvreType.SelectedValue, cboBladeOperation.SelectedValue, cboBladeOperationBottom.SelectedValue) Then
            pnlBladeLocks.Visible = True
        Else
            pnlBladeLocks.Visible = False
            cboBladeLocks.SelectedValue = 0
        End If

        valrfBladeLocks.Enabled = pnlBladeLocks.Visible

        ' END PLUG COLOUR

        If _RulesLouvreDetails.CanSetEndCapColour(cboLouvreProd.SelectedValue) Then
            pnlEndCapColour.Visible = True
        Else
            pnlEndCapColour.Visible = False
            cboEndCapColour.SelectedValue = 0
        End If

        valrfEndCapColour.Enabled = pnlEndCapColour.Visible

        ' HINGES

        _RulesLouvreDetails.PopulateHingesDDL(cboHinges, cboLouvreProd.SelectedValue, cboLouvreType.SelectedValue, cboBiFoldHingedDoor.SelectedValue, cboHinges.SelectedValue)

        If _RulesLouvreDetails.CanSetHinges(cboLouvreType.SelectedValue) Then
            pnlHinges.Visible = True
        Else
            pnlHinges.Visible = False
            cboHinges.SelectedValue = 0
        End If

        ' PANEL TOP RAIL

        _RulesLouvreDetails.PopulatePanelTopRailDDL(cboPanelTopRail, cboLouvreProd.SelectedValue, cboInsertTop.SelectedValue, cboPanelTopRail.SelectedValue)

        If _RulesLouvreDetails.CanSetPanelTopRail(cboLouvreType.SelectedValue) Then
            pnlPanelTopRail.Visible = True

            'Added by Michael Behar - 16-06-2020 - Ticket #57947
            CheckDualRail()

        Else
            pnlPanelTopRail.Visible = False
            cboPanelTopRail.SelectedValue = 0
        End If

        valrfPanelTopRail.Enabled = pnlPanelTopRail.Visible

        ' PANEL BOTTOM RAIL

        _RulesLouvreDetails.PopulatePanelBottomRailDDL(cboPanelBottomRail, cboLouvreProd.SelectedValue, cboInsertTop.SelectedValue, cboInsertBottom.SelectedValue, (cboPanelMidRail.SelectedValue > 0), cboPanelBottomRail.SelectedValue)

        If _RulesLouvreDetails.CanSetPanelBottomRail(cboLouvreType.SelectedValue) Then
            pnlPanelBottomRail.Visible = True

            'Added by Michael Behar - 16-06-2020 - Ticket #57947
            CheckDualRail()

        Else
            pnlPanelBottomRail.Visible = False
            cboPanelBottomRail.SelectedValue = 0
        End If

        valrfPanelBottomRail.Enabled = pnlPanelBottomRail.Visible
        'Added by surendra 25/11/2020 ticket #63195

        If cboLouvreType.SelectedValue > 0 Then
            If _RulesLouvreDetails.SetDropDownfixedBlade(cboLouvreType.SelectedValue) Then
                pnlPanelTopRail.Visible = False
                pnlPanelBottomRail.Visible = False
                pnlEndCapColour.Visible = False
                pnlBladeClipColour.Visible = False
                pnlPileColour.Visible = False
                pnlCChannel.Visible = False
                pnlHChannel.Visible = False
                pnlFlyScreen.Visible = False
                pnlBladeLocks.Visible = False

                cboPanelTopRail.SelectedValue = 0
                cboPanelBottomRail.SelectedValue = 0
                cboEndCapColour.SelectedValue = 0
                cboBladeClipColour.SelectedValue = 0
                cboPileColour.SelectedValue = 0
                cboCChannel.SelectedValue = 0
                cboHChannel.SelectedValue = 0
                cboFlyScreen.SelectedValue = 0
                cboBladeLocks.SelectedValue = 0
                cboFixedPanelSides.SelectedValue = 2
                cboInsertTop.SelectedValue = 1
                cboBladeOperation.SelectedValue = 2
                cboFixedPanelSides.Enabled = False
                cboInsertTop.Enabled = False
                cboBladeOperation.Enabled = False
            Else
                cboFixedPanelSides.Enabled = True
                cboInsertTop.Enabled = True
                cboBladeOperation.Enabled = True
            End If
        End If


    End Sub

    Private Sub RemoveControlsFormClasses(cControl As Object)
        If TypeOf cControl Is DropDownList Then
            Dim ddl As DropDownList = cControl

            ddl.CssClass = ddl.CssClass.Replace("form-select-disabled", String.Empty)
            ddl.CssClass = ddl.CssClass.Replace("form-select", String.Empty)

        ElseIf TypeOf cControl Is TextBox Then
            Dim tb As TextBox = cControl

            tb.CssClass = tb.CssClass.Replace("form-field-disabled", String.Empty)
            tb.CssClass = tb.CssClass.Replace("form-field", String.Empty)
        End If
    End Sub

    Private Sub EnableDisableLouvreDirectionPanel()
        pnlVerticalOpen.Visible = (cboInsertBottom.SelectedValue = InsertTypes.VerticalBlade OrElse cboInsertTop.SelectedValue = InsertTypes.VerticalBlade)
    End Sub

    Private Sub ColourChange()
        ' BTODO: lost this when the auto complete was added in AJAX form for colour.
        ' pearl white gloss, white satin, surfmist satin, anodic silver grey
        If (hdnColourID.Value = 3 OrElse hdnColourID.Value = 4 OrElse hdnColourID.Value = 5 OrElse hdnColourID.Value = 8) Then
            ' Light
            cboEndCapColour.SelectedValue = 2
            cboPileColour.SelectedValue = 2
            cboBladeClipColour.SelectedValue = 2
        Else
            ' Dark
            cboEndCapColour.SelectedValue = 1
            cboPileColour.SelectedValue = 1
            cboBladeClipColour.SelectedValue = 1
        End If
    End Sub

    Private Sub txtMidRailHeight_TextChanged(sender As Object, e As EventArgs) Handles txtMidRailHeight.TextChanged
        EnableDisableDetailsPopupUI()
        ViewState("DetailsTextChanged") = True
        'Added By Pradeep against ticket  #62389
        If ViewState("NewLouvreDetail") Then
            SetTopBottomInsertOperation()
        End If
        Page.Validate("details")
    End Sub

    Private Sub cboInsertTop_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboInsertTop.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboInsertBottom_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboInsertBottom.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboBladeOperation_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBladeOperation.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub valcustOpenDirection_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valcustOpenDirection.ServerValidate
        args.IsValid = (rdoOpenLeft.Checked OrElse rdoOpenRight.Checked)
    End Sub

    Private Sub valcustCurvedTrackMaxDeflection_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valcustCurvedTrackMaxDeflection.ServerValidate
        Dim cValidator As CustomValidator = DirectCast(source, CustomValidator)

        args.IsValid = True

        If IsNumeric(args.Value) Then
            If args.Value > _RulesLouvreDetails.MaxCurvedTrackDeflection Then

                cValidator.ErrorMessage = "<br />Max Deflection must be less than <b>" & _RulesLouvreDetails.MaxCurvedTrackDeflection & "mm</b>."
                args.IsValid = False

            ElseIf args.Value < 0 Then

                cValidator.ErrorMessage = "<br />Max Deflection must be positive."
                args.IsValid = False

            End If

        ElseIf String.IsNullOrWhiteSpace(args.Value) Then
            cValidator.ErrorMessage = "<br />Max Deflection is required."
        Else
            cValidator.ErrorMessage = "<br />Max Deflection is invalid."
            args.IsValid = False
        End If
    End Sub

    Private Sub rdoOpenLeft_CheckedChanged(sender As Object, e As EventArgs) Handles rdoOpenLeft.CheckedChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub rdoOpenRight_CheckedChanged(sender As Object, e As EventArgs) Handles rdoOpenRight.CheckedChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub txtLocation_TextChanged(sender As Object, e As EventArgs) Handles txtLocation.TextChanged
        EnableDisableDetailsPopupUI()
        ViewState("DetailsTextChanged") = True
        Page.Validate("details")
    End Sub

    'Private Sub cboColour_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboColour.SelectedIndexChanged
    ' BTODO: lost this when the auto complete was added in AJAX form for colour.
    '    ColourChange()
    '    EnableDisableDetailsPopupUI()
    '    Page.Validate("details")
    'End Sub

    Private Sub cboPileColour_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPileColour.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboBladeOperationBottom_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBladeOperationBottom.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboBiFoldHingedDoor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBiFoldHingedDoor.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboEndCapColour_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEndCapColour.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboBladeClipColour_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBladeClipColour.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboStacker_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboStacker.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboHinges_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboHinges.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub valcustMidRailHeight_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valcustMidRailHeight.ServerValidate
        Dim cValidator As CustomValidator = DirectCast(source, CustomValidator)
        Dim intHeight As Integer = 0

        If IsNumeric(txtHeight.Text) Then
            intHeight = txtHeight.Text
        End If

        args.IsValid = True

        If IsNumeric(args.Value) Then
            If args.Value > (intHeight - _RulesLouvreDetails.MinMidRailDistanceFromPanelEdge) Then

                cValidator.ErrorMessage = "<br />Mid Rail must not be within <b>" & _RulesLouvreDetails.MinMidRailDistanceFromPanelEdge & "mm</b><br /> from top of panel."
                args.IsValid = False

            ElseIf args.Value < _RulesLouvreDetails.MinMidRailDistanceFromPanelEdge Then

                cValidator.ErrorMessage = "<br />Mid Rail must not be within <b>" & _RulesLouvreDetails.MinMidRailDistanceFromPanelEdge & "mm</b><br />from bottom of panel."
                args.IsValid = False

            End If

        ElseIf String.IsNullOrWhiteSpace(args.Value) Then
            ' No midrail requested so blank is valid.
        Else
            cValidator.ErrorMessage = "<br />Mid Rail is invalid."
            args.IsValid = False
        End If

    End Sub

    Private Sub cboLockOptions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboLockOptions.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboFlushBoltsTop_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFlushBoltsTop.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboFlushBoltsBottom_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFlushBoltsBottom.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboBottomTrackType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBottomTrackType.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboTopTrackType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTopTrackType.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboLReveal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboLReveal.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboZReveal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboZReveal.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboHChannel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboHChannel.SelectedIndexChanged
        'added by surendra ticket #62782 
        If Not IsPostBack Then
            EnableDisableDetailsPopupUI()
        End If
        Page.Validate("details")
    End Sub

    Private Sub cboFixedPanelSides_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFixedPanelSides.SelectedIndexChanged
        'added by surendra ticket #62387 
        If Not IsPostBack Then
            EnableDisableDetailsPopupUI()
        End If
        Page.Validate("details")
    End Sub

    Private Sub cboWinder_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboWinder.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboSlide_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSlide.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboCurvedTrack_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCurvedTrack.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        'Added by pradeep Singh against ticket #65756 for point 2nd 
        TopTrackSelectionUI()
        Page.Validate("details")
    End Sub

    Private Sub txtExtraTrack_TextChanged(sender As Object, e As EventArgs) Handles txtExtraTrack.TextChanged
        EnableDisableDetailsPopupUI()
        ViewState("DetailsTextChanged") = True
        Page.Validate("details")
    End Sub

    Private Sub cboNoOfPanels_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboNoOfPanels.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        'added by surendra for ticket #62782 
        SetHChannelCombo()
        Page.Validate("details")
    End Sub

    Private Sub cboBladeSize_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBladeSize.SelectedIndexChanged

        PopulateBladeClipColourDDL()

        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub PopulateBladeClipColourDDL()
        cboBladeClipColour.Items.FindByValue(BladeClipColour.White).Enabled = (cboBladeSize.SelectedValue <> BladeSizes.Blade150mm)
    End Sub

    Private Sub cboBladeLocks_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBladeLocks.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboCChannel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCChannel.SelectedIndexChanged
        'added by surendra ticket #62387 
        If Not IsPostBack Then
            EnableDisableDetailsPopupUI()
        End If
        Page.Validate("details")
    End Sub

    Private Sub cboPanelTopRail_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPanelTopRail.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboPanelBottomRail_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPanelBottomRail.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboPanelMidRail_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPanelMidRail.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        'added by Pradeep Singh against ticket #65756  for point 4th under 3rd sheet
        CustomizemidrailUI()
        Page.Validate("details")
    End Sub

    Private Sub cboFlyScreen_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFlyScreen.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub cboFlushBoltsPosition_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFlushBoltsPosition.SelectedIndexChanged
        EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub

    Private Sub EnableDisableStackerBayLocationItems()

        cboStacker.Items.FindByValue(StackerBayLocation.Right).Enabled = True
        cboStacker.Items.FindByValue(StackerBayLocation.Left).Enabled = True

        If cboLouvreType.SelectedValue = LouvreTypes.Stacker Then
            If cboFlushBoltsPosition.SelectedValue = FlushBoltPosition.Left Then

                cboStacker.Items.FindByValue(StackerBayLocation.Right).Enabled = False

            ElseIf cboFlushBoltsPosition.SelectedValue = FlushBoltPosition.Right Then

                cboStacker.Items.FindByValue(StackerBayLocation.Left).Enabled = False

            End If
        End If
    End Sub

    Protected Sub ajaxUploader_UploadComplete(sender As Object, e As AjaxFileUploadEventArgs) Handles ajaxUploader.UploadComplete
        Dim lFiles As List(Of ProductionScheduleFile) = Session("ProdScheduleFiles")

        If lFiles.Count < ajaxUploader.MaximumNumberOfFiles Then
            Dim cNewFile As New ProductionScheduleFile

            With cNewFile
                .ID = GetUniqueNegativeIDForProdScheduleFiles(lFiles)
                .FileBinary = e.GetContents
                .FileName = e.FileName.Trim
                .CreationDateTime = Date.Now
                .SetFileTypeByFileName()
                .FileSize = e.FileSize
                .CanDeletePortal = False
                .VisiblePortal = False

                ' All ID's decided at save time
                .ProdScheduleID = 0
                .DetailID = 0
                .ExtraID = 0
            End With

            lFiles.Add(cNewFile)

            LoadFilesToProductionScheduleList()
        End If

        btnUploadFiles.Visible = UploadFilesButtonShouldBeVisible()
    End Sub

    Private Function UploadFilesButtonShouldBeVisible() As Boolean
        Dim lFiles As List(Of ProductionScheduleFile) = Session("ProdScheduleFiles")

        'If PageShouldBeLocked() Then
        '    Return False
        'End If

        Return (lFiles.Count < ajaxUploader.MaximumNumberOfFiles)
    End Function

    Private Function GetUniqueNegativeIDForProdScheduleFiles(cFiles As List(Of ProductionScheduleFile)) As Integer
        Dim lowestID As Integer = -1

        For Each f As ProductionScheduleFile In cFiles
            If f.ID <= lowestID Then
                lowestID = f.ID - 1
            End If
        Next

        Return lowestID
    End Function

    Protected Function FileIsUnsaved(intID As Integer) As Boolean
        Return intID <= 0
    End Function

    Private Sub btnUploadFiles_Click(sender As Object, e As EventArgs) Handles btnUploadFiles.Click
        pnlUploadFiles.Visible = True
        btnUploadFiles.Visible = False

        ToggleDisabledGridViewRowButtons(gdvProdScheduleFiles, New List(Of String) From {"lnkProdScheduleFileName"}, False)
        ToggleDisabledGridViewRowButtons(gdvProdScheduleFiles, New List(Of String) From {"btnProdScheduleDelete"}, False)

        lblProdScheduleFilesSupportedFormats.Text = "Supported file types: " & ajaxUploader.AllowedFileTypes.Replace(",", ", ") & "."
    End Sub

    Private Sub gdvProdScheduleFiles_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvProdScheduleFiles.RowCommand

        If e.CommandName = "DeleteProdScheduleFile" Then
            Dim lFiles As List(Of ProductionScheduleFile) = Session("ProdScheduleFiles")

            lFiles.RemoveAll(Function(x) x.ID = e.CommandArgument)

            Dim lFileIDs As List(Of Integer) = ViewState("DeletedProdScheduleFileIDs")

            If e.CommandArgument > 0 Then
                ' Only add it if it has a positive ID as it will be in the DB.
                lFileIDs.Add(e.CommandArgument)

                Dim lEditedFileIDs As List(Of Integer) = ViewState("EditedProdScheduleFileIDs")

                ' Remove deleted file id from edited ids collection
                lEditedFileIDs.Remove(e.CommandArgument)
            End If

            LoadFilesToProductionScheduleList()

            btnUploadFiles.Visible = lFiles.Count < ajaxUploader.MaximumNumberOfFiles

        ElseIf e.CommandName = "GetProdScheduleFile" Then
            Dim lFiles As List(Of ProductionScheduleFile) = Session("ProdScheduleFiles")
            Dim cFile As ProductionScheduleFile = lFiles.Find(Function(x) x.ID = e.CommandArgument)

            If cFile IsNot Nothing Then
                cFile.ToResponse(Page.Response)
            End If
        End If
    End Sub

    Protected Sub chkProdScheduleFileVisiblePortal_CheckChanged(sender As Object, e As EventArgs)
        Dim chk As CheckBox = sender
        Dim strCmd As String = chk.Attributes("CommandName")
        Dim intFileID As String = chk.Attributes("CommandArgument")
        Dim lFiles As List(Of ProductionScheduleFile) = Session("ProdScheduleFiles")
        Dim lFileIDs As List(Of Integer) = ViewState("EditedProdScheduleFileIDs")

        If strCmd = "ProdScheduleFileVisiblePortal" Then

            Dim cFile As ProductionScheduleFile = lFiles.Find(Function(x) x.ID = intFileID)

            If cFile IsNot Nothing Then
                If cFile.VisiblePortal <> chk.Checked Then

                    cFile.VisiblePortal = chk.Checked

                    ' File IDs above 0 are from the DB so can be saved as edited.
                    If cFile.ID > 0 Then
                        lFileIDs.Add(cFile.ID)
                    End If
                End If
            End If

        ElseIf strCmd = "ProdScheduleFileCanDeletePortal" Then

            Dim cFile As ProductionScheduleFile = lFiles.Find(Function(x) x.ID = intFileID)

            If cFile IsNot Nothing Then
                If cFile.CanDeletePortal <> chk.Checked Then

                    cFile.CanDeletePortal = chk.Checked

                    ' File IDs above 0 are from the DB so can be saved as edited.
                    If cFile.ID > 0 Then
                        lFileIDs.Add(cFile.ID)
                    End If
                End If
            End If

        End If
    End Sub

    Private Sub gdvProdScheduleFiles_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gdvProdScheduleFiles.RowCreated
        Dim lnk As LinkButton = e.Row.FindControl("lnkProdScheduleFileName")

        ' Ensure the file download links do full page postbacks within the gridview.
        If lnk IsNot Nothing Then
            ScriptManager1.RegisterPostBackControl(lnk)
        End If
    End Sub

    Private Sub btnCancelUploadFiles_Click(sender As Object, e As EventArgs) Handles btnCancelUploadFiles.Click
        pnlUploadFiles.Visible = False
        btnUploadFiles.Visible = True

        LoadFilesToProductionScheduleList()

        Dim lFiles As List(Of ProductionScheduleFile) = Session("ProdScheduleFiles")

        btnUploadFiles.Visible = lFiles.Count < ajaxUploader.MaximumNumberOfFiles
    End Sub

    Private Sub dgvDetails_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles dgvDetails.RowEditing

        dgvDetails.EditIndex = e.NewEditIndex
        LoadShutterDetailsForDataGrid()
    End Sub

    Private Sub dgvDetails_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles dgvDetails.RowUpdating
        Dim cLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")
        Dim cRow As GridViewRow = dgvDetails.Rows(e.RowIndex)
        Dim intID As Integer = DirectCast(cRow.FindControl("lblLouvreDetailID"), Label).Text
        Dim decSalePrice As Decimal = DirectCast(cRow.FindControl("txtSalePriceDetail"), TextBox).Text.Trim()

        If intID > 0 Then
            Dim cDetail As LouvreDetails = cLouvreDetails.Find(Function(x) x.LouvreDetailID = intID)

            If cDetail IsNot Nothing Then
                cDetail.LouvrePriceID = -1
                cDetail.SalePrice = decSalePrice
            End If
        End If

        dgvDetails.EditIndex = -1
        LoadShutterDetailsForDataGrid()
    End Sub

    Private Sub dgvDetails_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles dgvDetails.RowCancelingEdit

        dgvDetails.EditIndex = -1
        LoadShutterDetailsForDataGrid()
    End Sub

    Private Sub btnFreightPriceOverride_Click(sender As Object, e As EventArgs) Handles btnFreightPriceOverride.Click

        txtFreightTotal.Enabled = True
        txtFreightTotal.ToolTip = String.Empty

        btnFreightPriceOverride.Visible = False
        btnFreightPriceOverrideDetailSave.Visible = True
        btnFreightPriceOverrideDetailCancel.Visible = True
    End Sub

    Private Sub btnFreightPriceOverrideRemove_Click(sender As Object, e As EventArgs) Handles btnFreightPriceOverrideRemove.Click
        RemoveFreightOverride()
    End Sub

    Private Sub RemoveFreightOverride()
        ' Reset to 0 from -1. -1 is overridden, 0 is unset.
        ViewState("AddressZoneID") = 0
        txtFreightTotal.Text = FormatCurrency(0)

        ' Auto cost the freight
        RecalculateUIFreightFromDB()

        SetFreightPriceOverrideControlVisibility()
    End Sub

    Private Sub OverrideFreightSave()
        ' Set address zone id to -1 to indicate overridden.
        ViewState("AddressZoneID") = -1

        txtFreightTotal.Enabled = False

        SetFreightPriceOverrideControlVisibility()

        btnFreightPriceOverrideDetailSave.Visible = False
        btnFreightPriceOverrideDetailCancel.Visible = False
    End Sub

    Private Sub btnFreightPriceOverrideDetailSave_Click(sender As Object, e As EventArgs) Handles btnFreightPriceOverrideDetailSave.Click
        OverrideFreightSave()
    End Sub

    Private Sub btnFreightPriceOverrideDetailCancel_Click(sender As Object, e As EventArgs) Handles btnFreightPriceOverrideDetailCancel.Click

        RecalculateUIFreightFromDB()

        txtFreightTotal.Enabled = False

        btnFreightPriceOverrideDetailSave.Visible = False
        btnFreightPriceOverrideDetailCancel.Visible = False
    End Sub

    Private Sub ConfigureStockDeductionsUI()

        If txtId.Text > 0 Then
            Dim lStockDeductions As List(Of StockDeduction) = _Service.GetStockDeductionsByProductionScheduleID(txtId.Text)
            Dim enumDeductionStatus As StockDeductionStatus = StockDeductionStatus.Success

            If lStockDeductions.Where(Function(x) x.Status = StockDeductionStatus.Failure).Any() Then
                enumDeductionStatus = StockDeductionStatus.Failure
            ElseIf lStockDeductions.Where(Function(x) x.Status = StockDeductionStatus.AwaitingProcessing).Any() Then
                enumDeductionStatus = StockDeductionStatus.AwaitingProcessing
            End If

            If enumDeductionStatus = StockDeductionStatus.AwaitingProcessing Then
                lblStockDeductionsStatus.ForeColor = Color.Orange
                lblStockDeductionsStatus.Text = _STOCK_DEDUCTIONS_PROCESSING_MSG
                btnStockDeductionsPage.Visible = False

            ElseIf enumDeductionStatus = StockDeductionStatus.Failure Then
                lblStockDeductionsStatus.ForeColor = Color.Red
                lblStockDeductionsStatus.Text = _STOCK_DEDUCTIONS_FAILURE_MSG & " - " & lStockDeductions(0).StatusMessage
                btnStockDeductionsPage.Visible = True

            ElseIf (lStockDeductions.Count > 0) AndAlso enumDeductionStatus = StockDeductionStatus.Success Then
                lblStockDeductionsStatus.ForeColor = Color.Green
                lblStockDeductionsStatus.Text = _STOCK_DEDUCTIONS_COMPLETE_MSG
                btnStockDeductionsPage.Visible = False
            Else
                btnStockDeductionsPage.Visible = (CalculateStatus() > ProductionScheduleStatus.PaperworkProcessing)
            End If
        End If

    End Sub

#Region "Production Schedule Extras"

    Private Sub gdvExtrasProdSchedule_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gdvExtrasProdSchedule.RowCancelingEdit
        gdvExtrasProdSchedule.EditIndex = -1

        ' load changes from the viewstate datatable
        Dim lExtras As List(Of LouvreExtraProduct) = ViewState("ProductionScheduleExtras")

        ' Remove the blank row. Will be the last row added.
        If ViewState("IsAddExtra") = True Then
            If lExtras.Count > 0 Then
                lExtras.RemoveAt(lExtras.Count - 1)
            End If
        End If

        ' Populate the table from the viewstate datatable
        gdvExtrasProdSchedule.DataSource = lExtras
        gdvExtrasProdSchedule.DataBind()

        ToggleAddExtraControls(gdvExtrasProdSchedule, True)

        ViewState("IsAddExtra") = False
    End Sub

    Private Sub gdvExtrasProdSchedule_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvExtrasProdSchedule.RowCommand
        If e.CommandName = "DeleteExtraItem" Then
            Dim lExtras As List(Of LouvreExtraProduct) = ViewState("ProductionScheduleExtras")
            Dim cExtraToRemove As LouvreExtraProduct = lExtras.Find(Function(x) x.ID = CInt(e.CommandArgument))

            If cExtraToRemove IsNot Nothing Then
                lExtras.Remove(cExtraToRemove)
            End If

            RecalculateProductionScheduleExtrasCostFromDB()

            gdvExtrasProdSchedule.DataSource = lExtras
            gdvExtrasProdSchedule.DataBind()

        ElseIf (e.CommandName = "SalePriceOverride") Then
            Dim lExtras As List(Of LouvreExtraProduct) = ViewState("ProductionScheduleExtras")

            gdvExtrasProdSchedule.EditIndex = e.CommandArgument
            gdvExtrasProdSchedule.DataSource = lExtras
            gdvExtrasProdSchedule.DataBind()

            Dim cRow As GridViewRow = gdvExtrasProdSchedule.Rows(e.CommandArgument)
            Dim cddlExtraName As DropDownList = cRow.FindControl("ddlExtraName")
            Dim ctxtQty As TextBox = cRow.FindControl("txtQty")
            Dim ctxtCutLength As TextBox = cRow.FindControl("txtCutLength")
            Dim cbtnOpeningExtraUpdate As Button = cRow.FindControl("btnOpeningExtraUpdate")
            Dim cbtnOpeningExtraCancel As Button = cRow.FindControl("btnOpeningExtraCancel")
            Dim ctxtColour As TextBox = cRow.FindControl("txtColour")

            cddlExtraName.Enabled = False
            ctxtQty.Enabled = False
            ctxtCutLength.Enabled = False
            cbtnOpeningExtraUpdate.Visible = False
            cbtnOpeningExtraCancel.Visible = False
            ctxtColour.Enabled = False

        ElseIf (e.CommandName = "EditExtra") Then
            Dim lExtras As List(Of LouvreExtraProduct) = ViewState("ProductionScheduleExtras")

            gdvExtrasProdSchedule.EditIndex = e.CommandArgument
            gdvExtrasProdSchedule.DataSource = lExtras
            gdvExtrasProdSchedule.DataBind()

            Dim cRow As GridViewRow = gdvExtrasProdSchedule.Rows(e.CommandArgument)

            Dim ctxtSalePriceDetail As TextBox = cRow.FindControl("txtSalePriceDetail")
            Dim clblSalePrice As Label = cRow.FindControl("lblSalePrice")
            Dim cbtnSalePriceOverrideDetailSave As Button = cRow.FindControl("btnSalePriceOverrideDetailSave")
            Dim cbtnSalePriceOverrideDetailCancel As Button = cRow.FindControl("btnSalePriceOverrideDetailCancel")

            clblSalePrice.Visible = True
            ctxtSalePriceDetail.Visible = False

            If cbtnSalePriceOverrideDetailSave IsNot Nothing Then
                cbtnSalePriceOverrideDetailSave.Visible = False
            End If

            If cbtnSalePriceOverrideDetailCancel IsNot Nothing Then
                cbtnSalePriceOverrideDetailCancel.Visible = False
            End If

        ElseIf (e.CommandName = "SalePriceOverrideRemove") Then
            Dim lExtras As List(Of LouvreExtraProduct) = ViewState("ProductionScheduleExtras")
            Dim cExtra = lExtras.Find(Function(x) x.ID = e.CommandArgument)

            cExtra.LouvreExtraPriceID = 0
            cExtra.SalePrice = 0

            ' Recost extras.
            RecalculateProductionScheduleExtrasCostFromDB()
        End If
    End Sub

    Private Sub gdvExtrasProdSchedule_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvExtrasProdSchedule.RowDataBound
        Dim idLabel As Label = DirectCast(e.Row.Cells(0).FindControl("lblID"), Label)

        If idLabel IsNot Nothing Then
            Dim lExtras As List(Of LouvreExtraProduct) = DirectCast(sender.DataSource, List(Of LouvreExtraProduct))
            Dim cExtra As LouvreExtraProduct = lExtras.Find(Function(x) x.ID = idLabel.Text)

            If cExtra IsNot Nothing Then
                ' Populate extras dll for each row
                Dim extrasddl As DropDownList = DirectCast(e.Row.FindControl("ddlExtraName"), DropDownList)

                If Not extrasddl Is Nothing Then
                    PopulateExtrasDDL(extrasddl, ExtraProductVisibilityLevel.ProductionSchedule)

                    ' Set the style drop down vale from the dataset.
                    extrasddl.SelectedValue = cExtra.ExtraProductID
                End If

            End If
        End If
    End Sub

    Protected Sub ddlExtraName_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = sender
        Dim cRow As GridViewRow = ddl.Parent.Parent
        Dim idx As Integer = cRow.RowIndex
        Dim txtColour As TextBox = cRow.FindControl("txtColour")
        Dim valrfColour As RequiredFieldValidator = cRow.FindControl("valrfColour")
        Dim chdnColourID As HiddenField = cRow.FindControl("hdnColourID")
        Dim chdnColourName As HiddenField = cRow.FindControl("hdnColourName")
        Dim intExtraID As String = DirectCast(sender, DropDownList).SelectedValue

        Dim txtCutLength As TextBox = cRow.FindControl("txtCutLength")
        Dim valrfCutLength As RequiredFieldValidator = cRow.FindControl("valrfCutLength")
        Dim valcompCutLength As CompareValidator = cRow.FindControl("valcompCutLength")
        Dim valcustCutLengthMax As CustomValidator = cRow.FindControl("valcustCutLengthMax")

        ' Colour box does not exist at the louvre detail level.
        If txtColour IsNot Nothing Then
            txtColour.Text = String.Empty
            txtColour.Visible = False
            valrfColour.Enabled = False
        End If

        If chdnColourID IsNot Nothing Then
            chdnColourID.Value = 0
        End If

        If chdnColourName IsNot Nothing Then
            chdnColourName.Value = String.Empty
        End If

        txtCutLength.Text = 0
        txtCutLength.Visible = False
        valrfCutLength.Enabled = False
        valcompCutLength.Enabled = False
        valcustCutLengthMax.Enabled = False

        If intExtraID > 0 Then
            Dim cExtra As ExtraProductLouvres = _Service.GetExtraProductLouvresByID(intExtraID)

            If cExtra.ExtraProductID > 0 Then
                If cExtra.AppendColourCode Then

                    ' Colour box does not exist at the louvre detail level.
                    If txtColour IsNot Nothing Then
                        txtColour.Visible = True
                        valrfColour.Enabled = True
                    End If
                End If

                If cExtra.UnitOfMeasurement > 0 Then
                    txtCutLength.Visible = True
                    valrfCutLength.Enabled = True
                    valcompCutLength.Enabled = True
                    valcustCutLengthMax.Enabled = True
                End If
            End If
        End If

    End Sub

    Private Sub gdvExtrasProdSchedule_DataBound(sender As Object, e As EventArgs) Handles gdvExtrasProdSchedule.DataBound
        '
    End Sub

    Private Sub gdvExtrasProdSchedule_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gdvExtrasProdSchedule.RowEditing
        gdvExtrasProdSchedule.EditIndex = e.NewEditIndex

        ' load changes from the viewstate
        Dim lExtras As List(Of LouvreExtraProduct) = ViewState("ProductionScheduleExtras")

        ' Populate the table from the viewstate datatable
        gdvExtrasProdSchedule.DataSource = lExtras
        gdvExtrasProdSchedule.DataBind()

        ToggleAddExtraControls(gdvExtrasProdSchedule, False)
    End Sub

    Private Sub gdvExtrasProdSchedule_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gdvExtrasProdSchedule.RowUpdating
        If IsValidationGroupValid("productionscheduleextras") Then
            ' Save changes to the viewstate extras
            Dim lExtras As List(Of LouvreExtraProduct) = ViewState("ProductionScheduleExtras")
            Dim row As GridViewRow = gdvExtrasProdSchedule.Rows(e.RowIndex)

            Dim intID As Integer = DirectCast(row.FindControl("lblID"), Label).Text
            Dim intExtraID As String = DirectCast(row.FindControl("ddlExtraName"), DropDownList).SelectedValue
            Dim intQty As String = DirectCast(row.FindControl("txtQty"), TextBox).Text.Trim()
            Dim strCutLength As String = DirectCast(row.FindControl("txtCutLength"), TextBox).Text.Trim()
            Dim ctxtSalePriceDetail As TextBox = row.FindControl("txtSalePriceDetail")
            Dim chdnColourID As HiddenField = row.FindControl("hdnColourID")

            ' Find the ID of the record in the list
            Dim cExtra As LouvreExtraProduct = lExtras.Find(Function(x) x.ID = intID)

            If cExtra IsNot Nothing Then
                With cExtra
                    ' Update values
                    .ExtraProductID = intExtraID
                    .Quantity = intQty
                    .CutLength = strCutLength

                    ' Colour ID does not exist at detail level.
                    If chdnColourID IsNot Nothing Then
                        .ColourID = chdnColourID.Value
                    Else
                        .ColourID = 0
                    End If

                    If IsNumeric(ctxtSalePriceDetail.Text.Trim()) Then
                        .SalePrice = CDec(ctxtSalePriceDetail.Text.Trim())
                    Else
                        .SalePrice = 0
                    End If

                    If ctxtSalePriceDetail.Visible Then
                        ' If box is visible then the sale price is being overridden.
                        .LouvreExtraPriceID = -1
                        .PriceIsPercentage = False
                    End If
                End With
            End If

            RecalculateProductionScheduleExtrasCostFromDB()

            ' Finished editing
            gdvExtrasProdSchedule.EditIndex = -1

            gdvExtrasProdSchedule.DataSource = lExtras
            gdvExtrasProdSchedule.DataBind()

            ToggleAddExtraControls(gdvExtrasProdSchedule, True)

            ViewState("IsAddExtra") = False

        End If
    End Sub

    Private Sub btnAddExtrasProdSchedule_Click(sender As Object, e As EventArgs) Handles btnAddExtrasProdSchedule.Click

        ViewState("IsAddExtra") = True

        ' Add new row to viewstate datatable
        Dim lExtras As List(Of LouvreExtraProduct) = ViewState("ProductionScheduleExtras")
        Dim newExtra As New LouvreExtraProduct

        ' Allocate a unique negative ID
        If lExtras.Any Then
            newExtra.ID = lExtras.Min(Function(x) x.ID) - 1

            If newExtra.ID >= 0 Then
                newExtra.ID = -1
            End If
        Else
            newExtra.ID = -1
        End If

        newExtra.ProductionScheduleID = txtIntScheduleID.Text

        lExtras.Add(newExtra)

        gdvExtrasProdSchedule.DataSource = lExtras
        gdvExtrasProdSchedule.DataBind()

        ' Set new row to edit mode.
        gdvExtrasProdSchedule.EditIndex = gdvExtrasProdSchedule.Rows.Count - 1

        ' Have to rebind to get the edit of the new row to trigger.
        gdvExtrasProdSchedule.DataBind()

        Dim cRow As GridViewRow = gdvExtrasProdSchedule.Rows(gdvExtrasProdSchedule.Rows.Count - 1)

        Dim ctxtSalePriceDetail As TextBox = cRow.FindControl("txtSalePriceDetail")
        Dim clblSalePrice As Label = cRow.FindControl("lblSalePrice")
        Dim cbtnSalePriceOverrideDetailSave As Button = cRow.FindControl("btnSalePriceOverrideDetailSave")
        Dim cbtnSalePriceOverrideDetailCancel As Button = cRow.FindControl("btnSalePriceOverrideDetailCancel")

        clblSalePrice.Visible = True
        ctxtSalePriceDetail.Visible = False

        If cbtnSalePriceOverrideDetailSave IsNot Nothing Then
            cbtnSalePriceOverrideDetailSave.Visible = False
        End If

        If cbtnSalePriceOverrideDetailCancel IsNot Nothing Then
            cbtnSalePriceOverrideDetailCancel.Visible = False
        End If

        ToggleAddExtraControls(gdvExtrasProdSchedule, False)
    End Sub

#End Region

    Private Sub btnAddPrivacyScreen_Click(sender As Object, e As EventArgs) Handles btnAddPrivacyScreen.Click
        Dim lLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")

        ' Remember we are adding a new louvre detail
        ViewState("NewLouvreDetail") = True

        ucPrivacyScreenDetails.ShowAddPrivacyScreen(lLouvreDetails)
    End Sub

    Private Sub ucPrivacyScreenDetails_SaveDetails(intPSDetailID As Integer) Handles ucPrivacyScreenDetails.SaveDetails

        txtHiddenPSDetailID.Text = intPSDetailID

        If Not SaveDetails() Then
            Dim lLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")

            ' Keep the popup
            ucPrivacyScreenDetails.Show()

            ' Added By Pradeep Singh against id #72880
            If txtHiddenPSDetailID.Text <> "" Then
                Dim cLouvreJobDetails As LouvreDetails = lLouvreDetails.Find(Function(x) x.LouvreDetailID = txtHiddenPSDetailID.Text)
                If IsNothing(cLouvreJobDetails) Or cLouvreJobDetails.LouvrePriceID = 0 Then
                    lblStatus.ForeColor = Color.Red
                    lblStatus.Text = "Error saving details. Please try again."
                    ucPrivacyScreenDetails.Hide()
                End If
            End If

        End If
    End Sub

    Protected Sub ucPrivacyScreenDetails_CancelDetails(intPSDetailID As Integer) Handles ucPrivacyScreenDetails.CancelDetails
        Dim cLouvreDetails As LouvreDetailsCollection = ViewState("LouvreDetails")

        If ViewState("NewLouvreDetail") Then
            ' We were adding a new louvre detail so delete it and its extra dictionary entry.
            cLouvreDetails.RemoveAll(Function(x) x.LouvreDetailID = intPSDetailID)
        End If

        ViewState("NewLouvreDetail") = False
    End Sub

    Private Sub btnStockDeductionsPage_Click(sender As Object, e As EventArgs) Handles btnStockDeductionsPage.Click
        ' BTODO: add return page variable.
        Response.Redirect("StockDeductions.aspx?ScheduleId=" & txtIntScheduleID.Text & "&ReturnURL=" & CInt(ReturnURLID.LouvreJobDetails))
    End Sub

    Private Sub CheckDualRail()

        'No Product, No Rail Choice
        If cboLouvreProd.SelectedIndex = 0 Then
            Exit Sub
        End If

        'Check DLi Only
        If cboLouvreProd.SelectedValue = CStr(LouvreStyles.DLi) Then

            'Top and Bottom Rail
            If CInt(cboLouvreType.SelectedValue) = LouvreTypes.BiFold Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.Stacker Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.Sliding1Track Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.Sliding2Track Or
                   CInt(cboLouvreType.SelectedValue) = LouvreTypes.Sliding3Track Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.Sliding4Track Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.Hinged2Panels Or
                   CInt(cboLouvreType.SelectedValue) = LouvreTypes.HingedPanelLeft Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.HingedPanelRight Then

                If txtHeight.Text <> String.Empty Then
                    If CInt(txtHeight.Text) > 2500 Then

                        If (cboPanelTopRail.Visible) Then
                            cboPanelTopRail.SelectedValue = CStr(PanelTopRailType.Dual)
                        End If

                        If (cboPanelBottomRail.Visible) Then
                            cboPanelBottomRail.SelectedValue = CStr(PanelTopRailType.Dual)
                        End If

                    End If
                End If

                'No Of Panels Can Be "" When Not Valid Sized Shutter
                'Fixed By Michael Behar - Ticket #62839 
                If cboNoOfPanels.SelectedValue <> "" Then
                    If txtWidth.Text <> String.Empty And CInt(cboNoOfPanels.SelectedValue) > 0 Then
                        If CDec(txtWidth.Text) / CDec(cboNoOfPanels.SelectedItem.Text) >= 600 Then

                            If (cboPanelTopRail.Visible) Then
                                cboPanelTopRail.SelectedValue = CStr(PanelTopRailType.Dual)
                            End If

                            If (cboPanelBottomRail.Visible) Then
                                cboPanelBottomRail.SelectedValue = CStr(PanelTopRailType.Dual)
                            End If
                        End If
                    End If
                End If
            End If
        End If


    End Sub

    ''' <summary>
    ''' FOR TESTING BUTTON ONLY. COMMENT ME OUT AND HIDE BUTTON WHEN RELEASING.
    ''' </summary>
    'Private Sub btnPDFTest_Click(sender As Object, e As EventArgs) Handles btnPDFTest.Click
    '    'Dim cReport As New OrderConfirmation()
    '    'Dim cPDF As PDFGenerationResult = cReport.GeneratePDF(txtIntScheduleID.Text)

    '    'Dim cReport As New OrderDailyUpdate()
    '    'Dim cPDF As PDFGenerationResult = cReport.GeneratePDF(1)

    '    Dim cReport As New OrderCompleted()
    '    Dim cPDF As PDFGenerationResult = cReport.GeneratePDF(txtIntScheduleID.Text)

    '    Response.Clear()
    '    Response.ContentType = "application/pdf"
    '    Response.AddHeader("Content-Disposition", "attachment;filename=" & cPDF.FileName)
    '    Response.BinaryWrite(cPDF.PDFBytes)
    '    Response.End()
    'End Sub

    Private Enum ViewType
        NONE = -1
        Add = 0
        Update = 1
    End Enum

    Private Enum PDFType
        NONE = 0
        Order
        DeliveryDocket
        RunningSheet
        TimeSheet
        CoverSheet
        ProductionSheet
        'added by surendra 22-10-2020
        OptimiserSheet
        ExcelPowderCoat
    End Enum



    Protected Sub btnDashBoard_Click(sender As Object, e As EventArgs) Handles btnDashBoard.Click
        Response.Redirect("Dashboard.aspx", False)
    End Sub

    Protected Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        Response.Redirect("home.aspx", False)
    End Sub

    Protected Sub btnPlaceOrder_Click(sender As Object, e As EventArgs) Handles btnPlaceOrder.Click

        txtOrderDate.Text = DateTime.Now.ToString("dd MMM yyyy")
        If Save() Then
            If CInt(txtViewType.Text) = ViewType.Add Then
                Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & txtIntScheduleID.Text & " &ViewType=" & ViewType.Update & "&Status=1", False)
            Else
                Response.Redirect("LouvreJobDetails.aspx?" & GetCleanQueryString() & "&Status=1", False)
            End If
        End If

    End Sub

    Protected Sub dgvDetails_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgvDetails.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ''check order status condition by surendra dt:07/06/2023 ticket #75293
            If CInt(Request.Params("ScheduleID")) > 0 AndAlso CInt(hfOrderStatus.Value) <> SharedEnums.ProductionScheduleStatus.Quote AndAlso Convert.ToInt32(Session("customerId")) > 0 Then
                Dim btnDuplicate As Button = CType(e.Row.FindControl("btnDuplicateDetail"), Button)
                Dim btnDelete As Button = CType(e.Row.FindControl("btnDeleteDetail"), Button)
                btnDelete.Enabled = False
                btnDuplicate.Enabled = False
                ''added by surendra dt:07/06/2023 grayed out duplicate and delete button. ticket #75293
                btnDelete.Style.Add("opacity", "0.4")
                btnDuplicate.Style.Add("opacity", "0.4")
            End If
        End If
    End Sub

    Protected Sub btnFactoryPaperwork_Click(sender As Object, e As EventArgs) Handles btnFactoryPaperwork.Click

        'Create Memory Streams
        Dim memoryStreamReports As MemoryStream = New MemoryStream
        Dim memoryStreamFinalOutput As MemoryStream = New MemoryStream

        'Create PDF Copy
        Dim pdfCopyFields As PdfCopyFields = New PdfCopyFields(memoryStreamFinalOutput)

        'File Bytes
        Dim file As Byte()

        For i As Integer = 1 To 4

            If i = 1 Then 'Delivery Docket
                file = LouvreJobDeliveryDocket.GetLouvreJobDeliveryDocketMemoryStream(txtIntScheduleID.Text)
            ElseIf i = 2 Then   'Production Sheet
                Dim cProdSheet As New ProductionScheduleGenerator
                file = cProdSheet.GenerateProductionSheetPDF(txtIntScheduleID.Text)
            ElseIf i = 3 Then 'Running Sheet
                file = LouvreJobRunningSheet.GetJobRunningSheetMemoryStream(txtIntScheduleID.Text)
            Else 'Production Sheet Again
                Dim cProdSheet As New ProductionScheduleGenerator
                file = cProdSheet.GenerateProductionSheetPDF(txtIntScheduleID.Text)
            End If

            'Write File To Memory Stream Report
            memoryStreamReports.Write(file, 0, file.Length)

            'Set File Position
            memoryStreamReports.Position = 0

            'Add To pdfCopyFields
            pdfCopyFields.AddDocument(New PdfReader(memoryStreamReports))

        Next

        'Dispose Of Memory Stream
        memoryStreamReports.Dispose()

        'Close Copy Fields
        pdfCopyFields.Close()

        'Write Final Memory Stream
        file = memoryStreamFinalOutput.ToArray()

        'Clear and Display Memory Stream
        Response.Clear()
        Response.BufferOutput = False
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment; filename=FactoryPaperwork - " & txtIntScheduleID.Text & ".pdf")
        Response.BinaryWrite(file.ToArray())
        Response.End()

    End Sub

    'Added By Pradeep Singh for the ticket id #62385
    Private Sub SetTopPannelValue()

        If (cboLouvreProd.SelectedValue = LouvreStyles.DLi) Then
            If CInt(cboLouvreType.SelectedValue) = LouvreTypes.BiFold Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.Hinged2Panels Or
                CInt(cboLouvreType.SelectedValue) = LouvreTypes.HingedPanelLeft Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.HingedPanelRight Then

                'If cboPanelTopRail.SelectedValue <= PanelTopRailType.NONE Then
                cboPanelTopRail.SelectedValue = CStr(PanelTopRailType.NONE)
                'End If
            Else
                'If cboPanelTopRail.SelectedValue <= PanelTopRailType.NONE Then
                cboPanelTopRail.SelectedValue = PanelTopRailType.Slimline
                'End If

            End If


        End If
    End Sub

    'Added By Pradeep Singh for the ticket id #62385
    Private Sub SetBottomPannelValue()


        If (cboLouvreProd.SelectedValue = LouvreStyles.DLi) Then
            If CInt(cboLouvreType.SelectedValue) = LouvreTypes.BiFold Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.Hinged2Panels Or
                        CInt(cboLouvreType.SelectedValue) = LouvreTypes.HingedPanelLeft Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.HingedPanelRight Then

                ' If cboPanelBottomRail.SelectedValue <= PanelBottomRailType.NONE Then
                cboPanelBottomRail.SelectedValue = CStr(PanelBottomRailType.NONE)
                'End If
            Else
                'If cboPanelBottomRail.SelectedValue <= PanelBottomRailType.NONE Then
                cboPanelBottomRail.SelectedValue = PanelBottomRailType.Slimline
                'End If

            End If

        End If

    End Sub

    'Added by Pradeep Singh against ticket # 62427

    Private Sub SetFlushBoltValue()

        If _RulesLouvreDetails.ShouldBeSetBottomTrack(cboLouvreType.SelectedValue, cboLouvreProd.SelectedValue) Then
            Select Case cboLouvreProd.SelectedValue
                Case LouvreStyles.CL

                    If CInt(cboLouvreType.SelectedValue) = LouvreTypes.Sliding1Track Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.Sliding2Track Or
                        CInt(cboLouvreType.SelectedValue) = LouvreTypes.Stacker Or
                        CInt(cboLouvreType.SelectedValue) = LouvreTypes.Sliding3Track Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.Sliding4Track Then

                        'If cboFlushBoltsTop.SelectedValue <= FlushBolts.NONE Then
                        cboFlushBoltsTop.SelectedValue = FlushBolts.Short235mm
                        'End If

                        'If cboFlushBoltsBottom.SelectedValue <= FlushBolts.NONE Then
                        cboFlushBoltsBottom.SelectedValue = FlushBolts.Short235mm
                        'End If
                    Else
                        cboFlushBoltsTop.SelectedValue = FlushBolts.NONE
                        cboFlushBoltsBottom.SelectedValue = FlushBolts.NONE

                    End If
            End Select
        End If
    End Sub

    'Added By Pradeep against ticket  #62389
    Private Sub SetTopBottomInsertOperation()
        If _RulesLouvreDetails.CanSetTopInsert(cboLouvreType.SelectedValue) Then
            Select Case cboLouvreProd.SelectedValue
                Case LouvreStyles.DLi
                    If CInt(cboLouvreType.SelectedValue) = LouvreTypes.Fixed Then
                        If (pnlInsertTop.Visible) Then
                            cboInsertTop.SelectedValue = 1
                        End If
                        If (pnlBladeOperation.Visible) Then
                            cboBladeOperation.SelectedValue = 1
                        End If
                        If (pnlInsertBottom.Visible = True) Then
                            cboInsertBottom.SelectedValue = 1
                        End If
                        If (pnlBladeOperationBottom.Visible = True) Then
                            cboBladeOperationBottom.SelectedValue = 1
                        End If
                    End If
            End Select
        End If

    End Sub

    'added by surendra 30/10/2020 ticket #62782  
    Private Sub SetHChannelCombo()
        Dim cLouvreCollection As LouvreDetailsCollection = ViewState("LouvreDetails")
        Dim louvresId As Integer = Convert.ToInt32(txtHiddenPSDetailID.Text)
        Dim details As LouvreDetails = New LouvreDetails()
        Dim noOfPanel As Integer = 0
        If louvresId <> 0 Then
            details = cLouvreCollection.Find(Function(x) x.LouvreDetailID = louvresId)
        End If
        If IsNothing(details) Then
            details.Product = ""
            details.ShutterType = ""
            details.NoOfPanels = 0
        End If

        'Ticket #62386 - Added below 3 lines to getrid of Null error - 11-11-2020
        If Not String.IsNullOrEmpty(cboNoOfPanels.SelectedValue) Then
            noOfPanel = cboNoOfPanels.SelectedValue
        End If
        'If condition added by Surendra on 05/11/2020 to check this only incase of new entry. This condition was contradicting with ticket no.ticket #62782 
        If ViewState("NewLouvreDetail") = True Then
            'Ticket #62386 - Added by Michael Behar - 16-10-2020

            _RulesLouvreDetails.SetHingesForFixed(cboLouvreType.SelectedValue, noOfPanel, cboHChannel)
        Else
            'If ViewState("NewLouvreDetail") = False And cboNoOfPanels.SelectedValue <> "" And cboNoOfPanels.SelectedValue <> details.NoOfPanels Then
            '      _RulesLouvreDetails.SetHingesForFixed(cboLouvreType.SelectedValue, cboNoOfPanels.SelectedValue, cboHChannel)
            If ViewState("NewLouvreDetail") = False And cboNoOfPanels.SelectedValue <> "" And noOfPanel <> details.NoOfPanels Then
                _RulesLouvreDetails.SetHingesForFixed(cboLouvreType.SelectedValue, noOfPanel, cboHChannel)
            Else
                cboHChannel.SelectedValue = details.HChannel
            End If

        End If




        'If ViewState("NewLouvreDetail") = True Then
        '    If cboNoOfPanels.SelectedItem.Value = 1 Then
        '        cboHChannel.SelectedIndex = cboHChannel.Items.IndexOf(cboHChannel.Items.FindByValue(2))
        '    Else
        '        cboHChannel.SelectedIndex = cboHChannel.Items.IndexOf(cboHChannel.Items.FindByValue(0))
        '    End If
        'Else
        '    cboHChannel.SelectedValue = details.CChannel
        'End If
    End Sub

    'added by surendra 30/10/2020 ticket #62387
    Private Sub SetCChanelCombo()
        Dim cLouvreCollection As LouvreDetailsCollection = ViewState("LouvreDetails")
        Dim louvresId As Integer = Convert.ToInt32(txtHiddenPSDetailID.Text)
        Dim details As LouvreDetails = New LouvreDetails()
        If louvresId > 0 Or louvresId <= -1 Then
            details = cLouvreCollection.Find(Function(x) x.LouvreDetailID = louvresId)
        End If
        If IsNothing(details) Then
            details.Product = ""
            details.ShutterType = ""
            details.NoOfPanels = 0
        End If
        If ViewState("NewLouvreDetail") = True Then
            If cboLouvreProd.SelectedItem.Text.ToLower() = "dli" And cboLouvreType.SelectedItem.Text.ToLower() = "fixed" Then
                cboCChannel.SelectedIndex = cboCChannel.Items.IndexOf(cboCChannel.Items.FindByValue(1))
                cboFixedPanelSides.SelectedIndex = cboFixedPanelSides.Items.IndexOf(cboFixedPanelSides.Items.FindByValue(3))
            Else
                cboCChannel.SelectedIndex = cboCChannel.Items.IndexOf(cboCChannel.Items.FindByValue(0))
                cboFixedPanelSides.SelectedIndex = cboFixedPanelSides.Items.IndexOf(cboFixedPanelSides.Items.FindByValue(0))
            End If
        Else
            cboCChannel.SelectedValue = details.CChannel
            cboFixedPanelSides.SelectedValue = details.FixedPanelChannelID
        End If
    End Sub

    ' Added By Pradeep Singh against ticket #63915 
    Private Sub SetUIforFixedBladeAngles()

        If (cboLouvreProd.SelectedValue = LouvreStyles.DLi) Then
            If CInt(cboLouvreType.SelectedValue) = LouvreTypes.FixedBladesInAngle Then


                'Panel Top Rail
                pnlPanelTopRail.Visible = False
                cboPanelTopRail.SelectedValue = 0

                'Panel Bottom Rail
                pnlPanelBottomRail.Visible = False
                cboInsertTop.SelectedValue = 0

                ' End Cap Colour
                pnlEndCapColour.Visible = False
                cboEndCapColour.SelectedValue = 0

                'Blade Clip Colour

                pnlBladeClipColour.Visible = False
                cboBladeClipColour.SelectedValue = 0

                'Pile Colour
                pnlPileColour.Visible = False
                cboPileColour.SelectedValue = 0

                'T&B Fixing Channe
                pnlCChannel.Visible = False
                cboCChannel.SelectedValue = 0

                'H Joiner
                pnlHChannel.Visible = False
                cboHChannel.SelectedValue = 0

                'Fly Screen
                pnlFlyScreen.Visible = False
                cboFlyScreen.SelectedValue = 0


            Else
                pnlBladeClipColour.Visible = True
                pnlPileColour.Visible = True
                pnlFlyScreen.Visible = True

            End If
        End If
    End Sub
    ''Added by surendra Ticket #66605
    Public Sub DisableOrderType(orderTypeId As Integer)
        cboOrderType.Items.Clear()
        If ViewState("OrderType") IsNot Nothing Then
            Dim dtOrderType As DataTable = New DataTable()
            dtOrderType = ViewState("OrderType")
            If dtOrderType.Rows.Count > 0 Then
                dtOrderType = SharedFunctions.PerformDatatableFilterSortField(dtOrderType, "OrderTypeID='" & orderTypeId & "'", "")
                ViewState("OrderType") = dtOrderType
                cboOrderType.DataSource = dtOrderType
                cboOrderType.DataValueField = "OrderTypeID"
                cboOrderType.DataTextField = "OrderType"
                cboOrderType.DataBind()
            End If
        End If
    End Sub

#Region "Report Form"

    Protected Sub btnRepairForm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRepairForm.Click

        Dim objBuffer As Byte() = Nothing
        Dim strRptsPath As String = Server.MapPath("\ExcelRpts\WLRepairReturnAuthority.xlsx")

        Dim rptFileName As String = LouvreRepairForm.RepairFormGenerate(strRptsPath, objBuffer)
        If objBuffer IsNot Nothing Then
            '----
            Response.BinaryWrite(objBuffer)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;  filename=" & rptFileName)
            Response.End()
            '
        End If

    End Sub
#End Region

    'added by Pradeep Singh against ticket #65756  for point 4th under 3rd sheet
    Public Function CustomizemidrailUI()


        If cboPanelMidRail.SelectedValue = 0 Then
            txtMidRailHeight.Text = 0
            pnlMidRailHeight.Visible = False

            cboInsertBottom.SelectedValue = 0
            pnlInsertBottom.Visible = False

            cboBladeOperationBottom.SelectedValue = 0
            pnlBladeOperationBottom.Visible = False

        End If

    End Function

    'Added by pradeep Singh against ticket #65756 for point 2nd 
    Public Function TopTrackSelectionUI()

        If cboCurvedTrack.SelectedValue = 1 Then

            cboTopTrackType.SelectedValue = 4
            cboTopTrackType.Enabled = False
        Else

            cboTopTrackType.Enabled = True

        End If

    End Function

 ''Added by surendra ticket #66912
    Protected Sub btnQCChecklist_Click(sender As Object, e As EventArgs) Handles btnQCChecklist.Click
        Dim appServices As New AppService
        Dim dt As New DataTable
        Dim filePath As String = Server.MapPath("~/PriceMatrix/")
        Dim fileName As String = "QUALITY_CONTROL_CHECK_LIST.pdf"
        Response.ContentType = ContentType
        Response.AppendHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(Path.Combine(filePath, fileName))))
        Response.WriteFile(Path.Combine(filePath, fileName))
        Response.End()
    End Sub
End Class