Imports System.Data
Imports System.Net
Imports OzrollPSLVSchedulingModel.SharedEnums
Imports System.Linq

Partial Class ProductionScheduleList
    Inherits System.Web.UI.Page
    Private _Service As New AppService
    'Dim arrStates As String() = {"", "NSW", "QLD", "VIC", "TAS", "WA", "SA"}
    'Dim arrStateIds() As Integer = {0, 1, 2, 3, 4, 5, 6}
    'Dim arrSiteIds() As Integer = {0, 1, 7, 2, 44, 4, 3}
    'Dim intStateId As Integer = SharedConstants.DEFAULT_INTEGER_VALUE
    'Dim intSiteId As Integer = SharedConstants.DEFAULT_INTEGER_VALUE
    'Dim dtScheduleList As New DataTable
    'Dim dtAllMonths As New DataTable
    'Dim dtScheduleMonths As New DataTable
    'Dim intSScheduleYear As Integer = 2015
    ''added by surendra dt:01/06/2023 check user permission
    Dim cUserPermissions As UserPermissionsContainer = New UserPermissionsContainer()
    Dim bolHasPermission As Boolean = False

    Private Sub ProductionScheduleList_PreRender(sender As Object, e As EventArgs) Handles Me.PreLoad

        ''added by surendra dt:01/06/2023 ticket #75183 prevent extarnal customer login as internal.
        Dim service As AppService = New AppService()
        If Not IsNothing(Session("sessUserID")) Then
            cUserPermissions = service.Permissions.UserHasPermissions(CInt(Session("sessUserID")))
            bolHasPermission = cUserPermissions.HasPermission(Permissions.Admin) Or cUserPermissions.HasPermission(Permissions.TrackingPageAccess)
	    hfAdminPermission.Value = bolHasPermission
        End If
        service = Nothing
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Me.ClientScript.GetPostBackEventReference(Me, String.Empty)
        System.Web.UI.ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnReport)
        If Request.QueryString("var1") = "PM" Then
            ModalPopupExtender2.Show()
        End If



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

            txtProductTypeID.Text = Session("sessProductTypeID").ToString
            ''added by surendra dt:01/06/2023 ticket #75183 prevent extarnal customer login as internal.
            Dim service As New AppService
            If (Convert.ToInt32(Session("CustomerID")) > 0) Or (Convert.ToInt32(Session("CustomerID")) <= 0 AndAlso bolHasPermission = False) Then
                btnDashBoard.Visible = True
                btnHome.Visible = False
                btnAdd.Visible = False
                hfCustomerId.Value = Convert.ToInt32(Session("CustomerID"))
                btnAddJobExtarnalCustomer.Visible = True
                btnAdd.Visible = False
            End If
            service.AddWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)
            service = Nothing

            'Try
            '    If Not Request.QueryString.Count = 0 Then
            '        If Not IsNothing(Request.Params("StateId")) Then
            '            'Me.txtCurStateId.Text = Request.Params("StateId")
            '            intStateId = Request.Params("StateId")
            '        End If
            '    End If
            'Catch ex As Exception
            '    EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message & vbCrLf & getPageInfo())
            'End Try

            'Dim strState As String = String.Empty
            'If intStateId <> SharedConstants.DEFAULT_INTEGER_VALUE Then
            '    strState = arrStates(intStateId)
            '    Me.txtCurState.Text = strState
            '    intSiteId = arrSiteIds(intStateId)
            'End If
            'Me.txtCurState.Text = strState
            'Me.txtSiteID.Text = intSiteId
            Me.lblStatus.Text = String.Empty
            Me.txtActualShippingDate.Attributes.Add("autocorrect", "off")
            Me.txtOrderDate.Attributes.Add("autocorrect", "off")
            Me.txtOrderReference.Attributes.Add("autocorrect", "off")
            Me.txtInvoiceDate.Attributes.Add("autocorrect", "off")
            Me.txtShutterProNumber.Attributes.Add("autocorrect", "off")
            'Me.txtInvMonth.Attributes.Add("pattern", "[0-9]*")
            initCtrls()
            '

            'check querystring, set search parameters and run search
            If Not Request.QueryString.Count = 0 Then
                Dim bolRunSearch As Boolean = False

                If Not IsNothing(Request.Params("customerid")) Then
                    Me.cboCustomerName.SelectedValue = Request.Params("customerid").ToString
                    bolRunSearch = True
                End If
                If Not IsNothing(Request.Params("ordertypeid")) Then
                    Me.cboOrderType.SelectedValue = Request.Params("ordertypeid").ToString
                    bolRunSearch = True
                End If
                If Not IsNothing(Request.Params("orderreference")) Then
                    Me.txtOrderReference.Text = Request.Params("orderreference").ToString
                    bolRunSearch = True
                End If
                If Not IsNothing(Request.Params("ozrollid")) Then
                    Me.txtShutterProNumber.Text = Request.Params("ozrollid").ToString
                    bolRunSearch = True
                End If
                If Not IsNothing(Request.Params("orderstatusid")) Then
                    Me.cboOrderStatus.SelectedValue = Request.Params("orderstatusid").ToString
                    bolRunSearch = True
                End If
                If Not IsNothing(Request.Params("datetypeid")) Then
                    Me.cboDateType.SelectedValue = Request.Params("datetypeid").ToString
                    bolRunSearch = True
                End If
                If Not IsNothing(Request.Params("startdate")) Then
                    Me.txtStartDate.Text = CDate(Request.Params("startdate")).ToString("d MMM yyyy")
                    bolRunSearch = True
                End If
                If Not IsNothing(Request.Params("enddate")) Then
                    Me.txtEndDate.Text = CDate(Request.Params("enddate")).ToString("d MMM yyyy")
                    bolRunSearch = True
                End If

                If Not IsNothing(Request.Params("prioritylevel")) Then
                    Me.cboPriority.SelectedValue = Request.Params("prioritylevel").ToString
                    bolRunSearch = True
                End If

                If Not IsNothing(Request.Params("activeonly")) Then
                    Me.chkActiveOnly.Checked = True
                    bolRunSearch = True
                End If

                If bolRunSearch Then
                    Me.btnSearch_Click(Me, Nothing)
                End If
            End If


        End If




    End Sub

    Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        Dim objErr1 As Exception = Server.GetLastError().GetBaseException()

        Dim strErrorMessage As String = String.Empty

        strErrorMessage &= "Error In: " & Request.Url.ToString() & Environment.NewLine & Environment.NewLine

        strErrorMessage &= "Server.GetLastError().GetBaseException()" & Environment.NewLine & Environment.NewLine

        strErrorMessage &= "Error Message: " & objErr1.Message & Environment.NewLine
        strErrorMessage &= "Stack Trace:" & Environment.NewLine
        strErrorMessage &= objErr1.StackTrace & Environment.NewLine & Environment.NewLine

        Dim objErr2 As Exception = Server.GetLastError()

        strErrorMessage &= "Server.GetLastError()" & Environment.NewLine & Environment.NewLine

        strErrorMessage &= "Error Message: " & objErr2.Message & Environment.NewLine
        strErrorMessage &= "Stack Trace:" & Environment.NewLine
        strErrorMessage &= objErr2.StackTrace & Environment.NewLine

        EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & strErrorMessage)

        Server.ClearError()

        ' Response.Redirect("GenericErrorPage.aspx", False)
    End Sub

    Private Function getPageInfo() As String

        Dim strPageInfo As String = String.Empty
        Dim strName As String = String.Empty
        If Session.Contents.Count > 0 Then
            strPageInfo &= "Session Variables" & Environment.NewLine
            For Each strName In Session.Contents
                strPageInfo &= strName & ": " & CStr(Session.Contents(strName)) & Environment.NewLine
            Next
        Else
            strPageInfo &= "No Session Variables" & Environment.NewLine
        End If
        strPageInfo &= Environment.NewLine
        If Me.HasControls Then
            strPageInfo &= "Form Controls" & Environment.NewLine
            getPageControls(Me, strPageInfo)
        Else
            strPageInfo &= "No Form Controls" & Environment.NewLine
        End If
        Return strPageInfo

    End Function

    Private Sub getPageControls(ByVal ctrl As Control, ByRef strPageControls As String)

        If ctrl.HasControls Then
            For Each childCtrl As Control In ctrl.Controls
                getPageControls(childCtrl, strPageControls)
            Next
        Else
            Select Case ctrl.GetType.Name
                Case "TextBox"
                    Dim frmTxt As TextBox
                    frmTxt = DirectCast(ctrl, TextBox)
                    strPageControls &= frmTxt.ID & ": " & Left(frmTxt.Text, 100) & Environment.NewLine
                Case "DropDownList"
                    Dim frmCbo As DropDownList
                    frmCbo = DirectCast(ctrl, DropDownList)
                    If frmCbo.Items.Count > 0 Then
                        strPageControls &= frmCbo.ID & ": " & frmCbo.SelectedItem.Text & " (" & frmCbo.SelectedValue & ")" & Environment.NewLine
                    Else
                        strPageControls &= frmCbo.ID & ": Not Populated" & Environment.NewLine
                    End If
                Case "CheckBox"
                    Dim frmChk As CheckBox
                    frmChk = DirectCast(ctrl, CheckBox)
                    strPageControls &= frmChk.ID & ": " & frmChk.Checked & Environment.NewLine
                Case "RadioButton"
                    Dim frmRdo As RadioButton
                    frmRdo = DirectCast(ctrl, RadioButton)
                    strPageControls &= frmRdo.ID & ": " & frmRdo.Checked & Environment.NewLine
                Case "RadioButtonList"
                    Dim frmRdoLst As RadioButtonList
                    frmRdoLst = DirectCast(ctrl, RadioButtonList)
                    If frmRdoLst.SelectedIndex >= 0 Then
                        strPageControls &= frmRdoLst.ID & ": " & frmRdoLst.SelectedItem.Text & " (" & frmRdoLst.SelectedValue & ")" & Environment.NewLine
                    Else
                        strPageControls &= frmRdoLst.ID & ": Not Selected" & Environment.NewLine
                    End If
            End Select
        End If

    End Sub

    Protected Sub initCtrls()

        Dim service As New AppService
        Dim dtOStatus As DataTable = service.CreateOrderStatusTBL(CInt(Me.txtProductTypeID.Text))
        Me.cboOrderStatus.DataSource = dtOStatus
        Me.cboOrderStatus.DataValueField = "StatusID"
        Me.cboOrderStatus.DataTextField = "StatusName"
        Me.cboOrderStatus.DataBind()
        Me.cboOrderStatus.SelectedIndex = 0
        '
        'Add parameter by surendra ticket #65452
        SharedFunctions.PopulateCustomersDDL(cboCustomerName, CInt(txtProductTypeID.Text), service, Convert.ToInt32(Session("CustomerID")))
        ''check internal customer permission if user is not admin by surendra dt:06/06/2023
        If Convert.ToInt32(Session("CustomerID")) > 0 Or (Convert.ToInt32(Session("CustomerID")) <= 0 AndAlso bolHasPermission=False) Then
            btnReport.Visible = False
            Dim customerId As Integer
            customerId = Convert.ToInt32(Session("CustomerID"))
            cboCustomerName.SelectedIndex = cboCustomerName.Items.IndexOf(cboCustomerName.Items.FindByValue(customerId))
            cboCustomerName.Enabled = False
            cboCustomerName.ForeColor = System.Drawing.Color.Gray
            Dim sender As Object
            sender = ""
            Dim e As EventArgs
            btnSearch_Click(sender, e)

        End If
        '
        'dtScheduleMonths = service.getSchedulingMonths(dtAllMonths, intSScheduleYear)
        'Dim drowMonth As DataRow = dtScheduleMonths.NewRow
        'drowMonth("MonthID") = 0
        'drowMonth("FullMonthName") = ""
        'dtScheduleMonths.Rows.InsertAt(drowMonth, 0)
        'Me.cboMonth.DataSource = dtScheduleMonths
        'Me.cboMonth.DataValueField = "MonthID"
        'Me.cboMonth.DataTextField = "FullMonthName"
        'Me.cboMonth.DataBind()
        'Me.cboMonth.SelectedIndex = -1


        'setup order types list
        Dim dtOrderType As DataTable = createOrderTypeDatatable()
        Me.cboOrderType.DataSource = dtOrderType
        Me.cboOrderType.DataValueField = "OrderTypeID"
        Me.cboOrderType.DataTextField = "OrderType"
        Me.cboOrderType.DataBind()
        Me.cboOrderType.SelectedIndex = 0


        Dim dtDateType As DataTable = createDateTypeDatatable()
        Me.cboDateType.DataSource = dtDateType
        Me.cboDateType.DataValueField = "DateTypeID"
        Me.cboDateType.DataTextField = "DateType"
        Me.cboDateType.DataBind()
        Me.cboDateType.SelectedIndex = 0

        Dim dtPriority As DataTable = createPriorityDatatable()
        Me.cboPriority.DataSource = dtPriority
        Me.cboPriority.DataValueField = "PriorityID"
        Me.cboPriority.DataTextField = "PriorityName"
        Me.cboPriority.DataBind()
        Me.cboPriority.SelectedIndex = 0


        service = Nothing
        'dtScheduleList = loadProductionSchedule()
        'Me.dgvScheduleList.DataSource = dtScheduleList
        'Me.dgvScheduleList.DataBind()

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
        drow("OrderType") = "Reorder"
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

    Protected Function createDateTypeDatatable() As DataTable

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "DateTypeID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "DateType"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        Dim drow As DataRow = dt.NewRow
        drow("DateTypeID") = 0
        drow("DateType") = String.Empty
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("DateTypeID") = 1
        drow("DateType") = "Order Date"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("DateTypeID") = 2
        drow("DateType") = "Est. Shipping Date"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("DateTypeID") = 3
        drow("DateType") = "Invoice Date"
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

    Protected Function LoadProductionSchedule() As DataTable

        'Declare Datatable
        Dim dt As New DataTable
        Dim strTextboxFailed As String = ""

        'Check SQL Injection
        Dim bolValidSQL As Boolean = ChkSQLInjection(strTextboxFailed)

        'Check If Success Injection
        If Not bolValidSQL Then
            lblStatus.Text = strTextboxFailed & " text is not valid!"
            Return dt
        End If

        Dim strSQL As String = "Select dbo.tblProductionScheduleList.* from dbo.tblProductionScheduleList "
        Dim strWhere As String = String.Empty

        strSQL &= "Where "

        'producttype first
        strSQL &= "ProductTypeID=" & txtProductTypeID.Text & " And "

        If cboOrderStatus.SelectedIndex > 0 Then
            strSQL &= "OrderStatus=" & CInt(cboOrderStatus.SelectedValue) & " And "
        End If

        ''added by surendra dt:30/05/2023 to prevent show all record for extarnal customer
        ''check internal customer permission if user is not admin by surendra dt:06/06/2023
        If (CInt(hfCustomerId.Value)) > 0 Or (CInt(hfCustomerId.Value) <= 0 AndAlso bolHasPermission=False) Then
            strSQL &= "CustomeriD=" & CInt(hfCustomerId.Value) & " And "
        Else
            If cboCustomerName.SelectedIndex > 0 Then
                strSQL &= "CustomeriD=" & CInt(cboCustomerName.SelectedValue) & " And "
            End If
        End If

        If txtOrderReference.Text <> String.Empty Then
            strSQL &= "OrderReference like '%" & txtOrderReference.Text & "%'" & " And "
        End If

        If txtShutterProNumber.Text <> String.Empty Then
            strSQL &= "ShutterProNumber like '%" & txtShutterProNumber.Text & "%'" & " And "
        End If

        If cboOrderType.SelectedIndex > 0 Then
            strSQL &= "OrderTypeID=" & CInt(cboOrderType.SelectedValue) & " And "
        End If

        If cboPriority.SelectedIndex > 0 Then
            strSQL &= "PriorityLevel=" & CInt(cboPriority.SelectedValue) & " And "
            'Else
            '    strSQL &= "((PriorityLevel=0) or (PriorityLevel is null))" & " And "
        End If

        If chkActiveOnly.Checked = True Then
            strSQL &= "(CompletedDate is null) And "
        End If

        'date selection entry
        If cboDateType.SelectedIndex > 0 Then
            Select Case CInt(cboDateType.SelectedValue)
                Case 1
                    'order date
                    strSQL &= "((OrderDate >='" & Format(CDate(txtStartDate.Text), "d/MMM/yyyy") & "') And (OrderDate <='" & Format(CDate(txtEndDate.Text), "d/MMM/yyyy") & "')) And "
                Case 2
                    'expected shipping date
                    strSQL &= "((ExpectedShippingDate >='" & Format(CDate(txtStartDate.Text), "d/MMM/yyyy") & "') And (ExpectedShippingDate <='" & Format(CDate(txtEndDate.Text), "d/MMM/yyyy") & "')) And "
                Case 3
                    'invoice date
                    strSQL &= "((InvoicedDate >='" & Format(CDate(txtStartDate.Text), "d/MMM/yyyy") & "') And (InvoicedDate <='" & Format(CDate(txtEndDate.Text), "d/MMM/yyyy") & "')) And "
                Case Else
                    'do nothing
            End Select
        End If

        'If txtActualShippingDate.Text <> "" Then
        '    strSQL &= "ExpectedShippingDate='" & Format(CDate(txtActualShippingDate.Text), "d/MMM/yyyy") & "' And "
        'End If
        'If txtOrderDate.Text <> "" Then
        '    strSQL &= "OrderDate='" & Format(CDate(txtOrderDate.Text), "d/MMM/yyyy") & "' And "
        'End If

        'If txtOrderDate.Text <> "" Then
        '    strSQL &= "InvoicedDate='" & Format(CDate(txtInvoiceDate.Text), "d/MMM/yyyy") & "' And "
        'End If

        'remove any empty trailing "and" clause
        If Trim(strSQL.Substring(strSQL.Length - 4, 4)) = "And" Then
            strSQL = Trim(strSQL.Substring(0, strSQL.Length - 4))
        End If

        'remove any empty trailing "where" clause
        If Trim(strSQL.Substring(strSQL.Length - 6, 6)) = "Where" Then
            strSQL = Trim(strSQL.Substring(0, strSQL.Length - 6))
        End If
        'added by surendra for display job list order by 
        strSQL &= "order by EnteredDatetime desc "
        '
        Dim service As New AppService
        dt = service.RunSQLScheduling(strSQL)
        Dim dtPlatationSpecs As DataTable = service.GetAllPlantationSpecs
        Dim dtStatus As DataTable = service.CreateOrderStatusTBL(CInt(txtProductTypeID.Text))
        Dim dtCustomers As DataTable = service.RunSQLScheduling("select * from dbo.tblCustomers")
        Dim dtOrderType As DataTable = createOrderTypeDatatable()
        service = Nothing


        Dim col As DataColumn = New DataColumn
        col.ColumnName = "SiteId"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing
        '
        col = New DataColumn
        col.ColumnName = "FullMonthName"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing
        '
        col = New DataColumn
        col.ColumnName = "OrderStatusName"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing
        '
        col = New DataColumn
        col.ColumnName = "Colour"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "OffWhiteYN"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing
        '
        col = New DataColumn
        col.ColumnName = "BrightWhiteYN"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing
        '
        col = New DataColumn
        col.ColumnName = "OrderType"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Customer"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        For i As Integer = 0 To dt.Rows.Count - 1
            'If Not IsDBNull(dt.Rows(i)("InvoiceMonth")) Then
            '    dt.Rows(i)("FullMonthName") = dt.Rows(i)("MonthName").ToString & " " & dt.Rows(i)("YearID").ToString
            'End If

            If Not IsDBNull(dt.Rows(i)("CustomeriD")) Then
                Dim drows() As DataRow = dtCustomers.Select("CustomeriD=" & dt.Rows(i)("CustomeriD").ToString)
                If drows.Length > 0 Then
                    dt.Rows(i)("Customer") = drows(0)("CustomerName").ToString
                End If
            End If

            If Not IsDBNull(dt.Rows(i)("OrderStatus")) Then
                Dim drows() As DataRow = dtStatus.Select("StatusID=" & dt.Rows(i)("OrderStatus").ToString)
                If drows.Length > 0 Then
                    dt.Rows(i)("OrderStatusName") = drows(0)("StatusName").ToString
                End If
            End If

            'check for on hold
            If Not IsDBNull(dt.Rows(i)("OnHold")) Then
                If CInt(dt.Rows(i)("OnHold")) = 1 Then
                    dt.Rows(i)("OrderStatusName") = "On Hold - " & dt.Rows(i)("OrderStatusName").ToString
                End If
            End If

            If Not IsDBNull(dt.Rows(i)("OrderTypeID")) Then
                Dim drows() As DataRow = dtOrderType.Select("OrderTypeID=" & dt.Rows(i)("OrderTypeID").ToString)
                If drows.Length > 0 Then
                    dt.Rows(i)("OrderType") = drows(0)("OrderType").ToString
                End If
            End If

            Dim drTMPs() As DataRow = dtPlatationSpecs.Select("ProductScheduleID=" & dt.Rows(i)("Id"))
            If drTMPs.Length > 0 Then
                If Not IsDBNull(drTMPs(0)("PanelsLess700")) Then
                    dt.Rows(i)("PanelsLess700") = drTMPs(0)("PanelsLess700")
                End If
                If Not IsDBNull(drTMPs(0)("PanelsMore700")) Then
                    dt.Rows(i)("PanelsMore700") = drTMPs(0)("PanelsMore700")
                End If

                If Not IsDBNull(drTMPs(0)("QtyHinges")) Then
                    dt.Rows(i)("QtyHinges") = drTMPs(0)("QtyHinges")
                End If

                If Not IsDBNull(drTMPs(0)("QtySliding")) Then
                    dt.Rows(i)("QtySliding") = drTMPs(0)("QtySliding")
                End If

                If Not IsDBNull(drTMPs(0)("QtyBifold")) Then
                    dt.Rows(i)("QtyBifold") = drTMPs(0)("QtyBifold")
                End If

                If Not IsDBNull(drTMPs(0)("QtyFixed")) Then
                    dt.Rows(i)("QtyFixed") = drTMPs(0)("QtyFixed")
                End If

                If Not IsDBNull(drTMPs(0)("QtyZFrame")) Then
                    dt.Rows(i)("QtyZFrame") = drTMPs(0)("QtyZFrame")
                End If

                If Not IsDBNull(drTMPs(0)("QtyLFrame")) Then
                    dt.Rows(i)("QtyLFrame") = drTMPs(0)("QtyLFrame")
                End If
                If Not IsDBNull(drTMPs(0)("ColourId")) Then
                    If CInt(drTMPs(0)("ColourId")) = 1 Then
                        dt.Rows(i)("Colour") = "Yes"
                    Else
                        dt.Rows(i)("Colour") = "No"
                    End If
                End If
            End If

            'If Not IsDBNull(dt.Rows(i)("OffWhite")) Then
            '    If CInt(dt.Rows(i)("OffWhite")) = 1 Then
            '        dt.Rows(i)("OffWhiteYN") = "Yes"
            '    Else
            '        dt.Rows(i)("OffWhiteYN") = "No"
            '    End If
            'End If

            'If Not IsDBNull(dt.Rows(i)("BrightWhite")) Then
            '    If CInt(dt.Rows(i)("BrightWhite")) = 1 Then
            '        dt.Rows(i)("BrightWhiteYN") = "Yes"
            '    Else
            '        dt.Rows(i)("BrightWhiteYN") = "No"
            '    End If
            'End If

        Next

        Return dt

    End Function

    Protected Sub cboStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim gvr As GridViewRow = DirectCast(DirectCast(sender, Control).NamingContainer, GridViewRow)
        Dim ddl As DropDownList = DirectCast(sender, DropDownList)

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Me.lblStatus.Text = String.Empty
        '
        Dim dtScheduleList As DataTable = LoadProductionSchedule()
        Me.dgvScheduleList.DataSource = dtScheduleList
        Me.dgvScheduleList.DataBind()
        If Me.dgvScheduleList.Rows.Count = 0 Then
            Me.pnlList.Visible = False

            If lblStatus.Text = String.Empty Then
                Me.lblStatus.Text = "No Records Found."
            End If

        Else
            Me.pnlList.Visible = True
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(String), "Script", "restBTNs();", True)

    End Sub

    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click

        Response.Redirect("Logout.aspx", False)

    End Sub

    Protected Sub dgvScheduleList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvScheduleList.RowCommand

        If (e.CommandName = "ScheduleDetail") Then
            'If Not Request.QueryString.Count = 0 Then
            '    If Not IsNothing(Request.Params("StateId")) Then
            '        'Me.txtCurStateId.Text = Request.Params("StateId")
            '        intStateId = Request.Params("StateId")
            '    End If
            'End If
            Dim currentRowIndex As Integer = Int32.Parse(e.CommandArgument.ToString())
            Dim intScheduleId As String = dgvScheduleList.DataKeys(currentRowIndex).Values("ID").ToString
            Dim intOrderTypeID As Integer = CInt(dgvScheduleList.DataKeys(currentRowIndex).Values("OrderTypeID"))

            Dim strSearchQueryString As String = String.Empty

            If Me.cboCustomerName.SelectedIndex > 0 Then
                strSearchQueryString &= "&customerid=" & Me.cboCustomerName.SelectedValue.ToString
            End If
            If Me.cboOrderType.SelectedIndex > 0 Then
                strSearchQueryString &= "&ordertypeid=" & Me.cboOrderType.SelectedValue.ToString
            End If
            If Me.cboOrderStatus.SelectedIndex > 0 Then
                strSearchQueryString &= "&orderstatusid=" & Me.cboOrderStatus.SelectedValue.ToString
            End If
            If Me.txtOrderReference.Text <> String.Empty Then
                strSearchQueryString &= "&orderreference=" & Me.txtOrderReference.Text
            End If
            If Me.txtShutterProNumber.Text <> String.Empty Then
                strSearchQueryString &= "&ozrollid=" & Me.txtShutterProNumber.Text
            End If

            If Me.cboPriority.SelectedIndex > 0 Then
                strSearchQueryString &= "&prioritylevel=" & Me.cboPriority.SelectedValue.ToString
            End If

            If Me.chkActiveOnly.Checked = True Then
                strSearchQueryString &= "&activeonly=1"
            End If

            If Me.cboDateType.SelectedIndex > 0 Then
                strSearchQueryString &= "&datetypeid=" & Me.cboDateType.SelectedValue.ToString
            End If
            If Me.txtStartDate.Text <> String.Empty Then
                strSearchQueryString &= "&startdate=" & Me.txtStartDate.Text
            End If
            If Me.txtEndDate.Text <> String.Empty Then
                strSearchQueryString &= "&enddate=" & Me.txtEndDate.Text
            End If

            If Me.txtProductTypeID.Text = "1" Then
                If SharedConstants.LIVE_SITE Then
                    Response.Redirect("ProductionScheduleDetailsNew.aspx?ScheduleId=" & intScheduleId.ToString & "&ViewType=1" & strSearchQueryString, False)
                Else
                    Response.Redirect("ProductionScheduleDetailsNew.aspx?ScheduleId=" & intScheduleId.ToString & "&ViewType=1" & strSearchQueryString, False)
                End If
            ElseIf Me.txtProductTypeID.Text = "2" Then
                If SharedConstants.LIVE_SITE Then
                    Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & intScheduleId.ToString & "&ViewType=1" & strSearchQueryString, False)
                Else
                    Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & intScheduleId.ToString & "&ViewType=1" & strSearchQueryString, False)
                End If
            End If

        End If

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        If Me.txtProductTypeID.Text = "1" Then
            Response.Redirect("ProductionScheduleDetailsNew.aspx?ScheduleId=0&ViewType=0", False)
        ElseIf Me.txtProductTypeID.Text = "2" Then
            Response.Redirect("LouvreJobDetails.aspx?ScheduleId=0&ViewType=0", False)
        End If


    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        '
        Dim dtScheduleList As DataTable = LoadProductionSchedule()
        Dim strRptsPath As String = Server.MapPath("") & "\ExcelRpts"
        Dim objBuffer As Byte() = Nothing
        'ExcelReport.createReport(strRptsPath, objBuffer, dtScheduleList)
        Dim rptFileName As String = ExcelReport.createReport(strRptsPath, objBuffer, dtScheduleList)
        Dim strRPTFileName As String = IO.Path.Combine(strRptsPath, rptFileName)
        If objBuffer IsNot Nothing Then
            '----
            Response.BinaryWrite(objBuffer)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;  filename=" & rptFileName)
            Response.End()
            '
        End If
        objBuffer = Nothing
        '

    End Sub

    Protected Sub btnClear_Click(sender As Object, e As System.EventArgs) Handles btnClear.Click

        Me.lblStatus.Text = String.Empty
        '
        Dim dtScheduleList As DataTable = New DataTable
        Me.pnlList.Visible = False
        'If Me.dtScheduleList.Rows.Count = 0 Then
        '    Me.lblStatus.Text = "No Records Found."
        'End If
        'Added by surendra '
        If Convert.ToInt32(Session("CustomerID")) = 0 Then
            Me.cboCustomerName.SelectedIndex = -1
        End If

        Me.txtInvoiceDate.Text = String.Empty
        Me.cboOrderStatus.SelectedIndex = -1
        Me.txtActualShippingDate.Text = String.Empty
        Me.txtOrderDate.Text = String.Empty
        Me.txtOrderReference.Text = String.Empty
        Me.cboOrderType.SelectedIndex = -1

        Me.cboDateType.SelectedIndex = -1
        Me.txtStartDate.Text = String.Empty
        Me.txtEndDate.Text = String.Empty

        ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(String), "Script", "restBTNs();", True)

    End Sub

    Protected Sub btnHome_Click(sender As Object, e As System.EventArgs) Handles btnHome.Click

        Response.Redirect("Home.aspx", False)

    End Sub

    Private Sub dgvScheduleList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgvScheduleList.Sorting

        Me.lblStatus.Text = String.Empty
        '
        Dim dtScheduleList As DataTable = LoadProductionSchedule()
        dtScheduleList = SharedFunctions.PerformDatatableFilterSortField(dtScheduleList, "", e.SortExpression)

        Me.dgvScheduleList.DataSource = dtScheduleList
        Me.dgvScheduleList.DataBind()
        If Me.dgvScheduleList.Rows.Count = 0 Then
            Me.pnlList.Visible = False
            Me.lblStatus.Text = "No Records Found."
        Else
            Me.pnlList.Visible = True
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(String), "Script", "restBTNs();", True)


    End Sub


    Protected Sub btnAddJobExtarnalCustomer_Click(sender As Object, e As EventArgs) Handles btnAddJobExtarnalCustomer.Click
        If Me.txtProductTypeID.Text = "1" Then
            Response.Redirect("ProductionScheduleDetailsNew.aspx?ScheduleId=0&ViewType=0", False)
        ElseIf Me.txtProductTypeID.Text = "2" Then
            Response.Redirect("LouvreJobDetails.aspx?ScheduleId=0&ViewType=0", False)
        End If
    End Sub

    Protected Sub btnDashBoard_Click(sender As Object, e As EventArgs) Handles btnDashBoard.Click
        Response.Redirect("Dashboard.aspx", False)
    End Sub

    Private Function ChkSQLInjection(ByRef strTextboxFailed As String) As Boolean

        'Variable
        Dim bolSafeQuery As Boolean = True

        'Check Order Reference
        bolSafeQuery = SharedFunctions.ChkSQlInjection(txtOrderReference, lblStatus)

        'Check Shutter Pro
        If bolSafeQuery Then
            bolSafeQuery = SharedFunctions.ChkSQlInjection(txtShutterProNumber, lblStatus)

            If Not bolSafeQuery Then
                strTextboxFailed = "Shutter Pro Number"
            End If

        Else
            strTextboxFailed = "Order Reference"
        End If

        'Return
        Return bolSafeQuery

    End Function


    'Protected Sub btnCancelQuote_Click(sender As Object, e As EventArgs) Handles btnCancelNote.Click
    '    Session("QuickQuote") = String.Empty
    '    Dim DummyVar As String = "PM"
    '    ' DummyVar = Request.QueryString("var1")
    '    ' Response.Redirect("Home.aspx?var1=" + DummyVar, False)
    '    Response.Redirect("Home.aspx", False)
    'End Sub


    Private Sub cboLouvreProd_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboLouvreProd.SelectedIndexChanged

        PopulateShutterTypeDDL(cboLouvreType.SelectedIndex)

        'EnableDisableDetailsPopupUI()

        'SetTopTrackValue()
        ' SetBottomTrackValue()
        Page.Validate("details")
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
    'Private Sub SetBottomTrackValue()

    '    If _RulesLouvreDetails.ShouldBeSetBottomTrack(cboLouvreType.SelectedValue, cboLouvreProd.SelectedValue) Then
    '        Select Case cboLouvreProd.SelectedValue
    '            Case LouvreStyles.DLi

    '                ' Default selection
    '                If cboBottomTrackType.SelectedValue <= BottomTrackTypes.NONE Then
    '                    cboBottomTrackType.SelectedValue = BottomTrackTypes.Dli12mm
    '                End If

    '            Case LouvreStyles.CL

    '                ' Default selection
    '                If cboBottomTrackType.SelectedValue <= BottomTrackTypes.NONE Then
    '                    cboBottomTrackType.SelectedValue = BottomTrackTypes.CL25mm
    '                End If

    '            Case LouvreStyles.NONE
    '                cboBottomTrackType.SelectedValue = TopTrackTypes.NONE
    '        End Select
    '    Else
    '        cboBottomTrackType.SelectedValue = TopTrackTypes.NONE
    '    End If
    'End Sub

    'Private Sub SetTopTrackValue()

    '    If _RulesLouvreDetails.ShouldBeSetTopTrack(cboLouvreType.SelectedValue, cboLouvreProd.SelectedValue) Then
    '        Select Case cboLouvreProd.SelectedValue
    '            Case LouvreStyles.CL

    '                ' Default selection
    '                If cboTopTrackType.SelectedValue <= TopTrackTypes.NONE OrElse
    '                    cboTopTrackType.SelectedValue = TopTrackTypes.DLiWithoutSubHead Then

    '                    If cboLouvreType.SelectedValue = LouvreTypes.Stacker Then
    '                        cboTopTrackType.SelectedValue = TopTrackTypes.Box60x40
    '                    Else
    '                        cboTopTrackType.SelectedValue = TopTrackTypes.CL
    '                    End If
    '                End If

    '            Case LouvreStyles.DLi

    '                ' Default selection
    '                If cboTopTrackType.SelectedValue <= TopTrackTypes.NONE OrElse
    '                    cboTopTrackType.SelectedValue = TopTrackTypes.CL Then

    '                    If cboLouvreType.SelectedValue = LouvreTypes.Stacker Then
    '                        cboTopTrackType.SelectedValue = TopTrackTypes.Box60x40
    '                    Else
    '                        cboTopTrackType.SelectedValue = TopTrackTypes.DLiWithoutSubHead
    '                    End If

    '                End If

    '            Case LouvreStyles.NONE
    '                cboTopTrackType.SelectedValue = TopTrackTypes.NONE
    '        End Select
    '    Else
    '        cboTopTrackType.SelectedValue = TopTrackTypes.NONE
    '    End If
    'End Sub  

    'Private Sub cboPanelTopRail_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPanelTopRail.SelectedIndexChanged
    '    ' EnableDisableDetailsPopupUI()
    '    Page.Validate("details")
    'End Sub

    Private Sub cboPanelBottomRail_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPanelBottomRail.SelectedIndexChanged
        'EnableDisableDetailsPopupUI()
        Page.Validate("details")
    End Sub
    Private Sub SetTopPannelValue()

        If (cboLouvreProd.SelectedValue = LouvreStyles.DLi) Then
            If CInt(cboLouvreType.SelectedValue) = LouvreTypes.BiFold Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.Hinged2Panels Or
                CInt(cboLouvreType.SelectedValue) = LouvreTypes.HingedPanelLeft Or CInt(cboLouvreType.SelectedValue) = LouvreTypes.HingedPanelRight Then

                'If cboPanelTopRail.SelectedValue <= PanelTopRailType.NONE Then
                'cboPanelTopRail.SelectedValue = CStr(PanelTopRailType.NONE)
                'End If
            Else
                'If cboPanelTopRail.SelectedValue <= PanelTopRailType.NONE Then
                'cboPanelTopRail.SelectedValue = PanelTopRailType.Slimline
                'End If

            End If


        End If
    End Sub
End Class
