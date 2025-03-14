﻿Imports AjaxControlToolkit
Imports OzrollPSLVSchedulingModel.SharedConstants
Imports SybizDebtors = Sybiz.Vision.Platform.Debtors
Imports System.Linq

Partial Class AddCustomer
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
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

            Dim service As New AppService

            ' Log page access
            service.addWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)

            'Decrypt Query String
            txtHidCustomerID.Text = (Request.QueryString("c"))

            PopulateDropDownLists()

            If (txtHidCustomerID.Text = String.Empty) Then
                SetAddCustomerMode()
            Else
                SetUpdateCustomerMode()
            End If

            PopulateToolTips()
            PopulateCustomerUIFromDB()
        End If

        pnlResults.Visible = False
        lblStatus.Visible = False

        ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(String), "StyleCheckBoxesForFocus", "styleCheckboxes();", True)

    End Sub

    Private Sub PopulateToolTips()
        Dim service As New AppService

        ' Mail lists
        Dim cMail As List(Of MailingList) = service.GetMailingLists()

        For Each m As MailingList In cMail
            Select Case m.ID
                Case SharedEnums.MailingListID.OrderCompleted
                    imgTipMailingListOrderCompleted.ToolTip = m.Description
                Case SharedEnums.MailingListID.OrderConfirmation
                    imgTipMailingListOrderConfirmation.ToolTip = m.Description
                Case SharedEnums.MailingListID.WeeklyOrderStatusUpdate
                    imgTipMailingListWeeklyOrderStatusUpdate.ToolTip = m.Description
            End Select
        Next m
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

        Response.Redirect("GenericErrorPage.aspx", False)
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

    Private Sub PopulateCustomerUIFromDB()
        Dim service As New AppService
        Dim intCustomerID As Integer

        Integer.TryParse(txtHidCustomerID.Text, intCustomerID)

        If intCustomerID > 0 Then
            Dim cCustomer As Customer = service.GetCustomerByID(intCustomerID)

            If cCustomer.CustomerID > 0 Then
                With cCustomer

                    txtCode.Text = .Code

                    If .Code.Length > 0 Then
                        ' If a code exists then sybiz link has been made in the past.
                        imgSybizLinked.ImageUrl = "~/images/green_tick_32x32.png"
                        imgSybizLinked.ToolTip = "Linked to Sybiz"
                    End If

                    txtCustomerName.Text = .CustomerName
                    txtTradingName.Text = .TradingName
                    txtEmail.Text = .Email
                    txtPhone1.Text = .CustomerPhone1
                    txtPhone2.Text = .CustomerPhone2
                    txtPhone3.Text = .CustomerPhone3

                    txtFreightPercentage.Text = (.FreightPercentage * 100).ToString("0.##")
                    txtDiscountPercentage.Text = (.Discount * 100).ToString("0.##")
                    txtCustomerAbbreviation.Text = .CustomerAbbreviation
                    txtCreditLimit.Text = FormatCurrency(.CreditLimit, 2, True,, TriState.True)
                    txtABN.Text = .ABN

                    ' Drop down lists
                    ddlLouvreCategory.SelectedValue = .LouvreCategoryID

                    ' DDLs from sybiz
                    ddlTradingTerms.SelectedValue = .TradingTermID
                    ddlTaxStatus.SelectedValue = .TaxStatusID
                    ddlSortCode.SelectedValue = .SortCodeID
                    ddlAnalysisCode.SelectedValue = .AnalysisCodeID
                    ddlPriceScale.SelectedValue = .PriceScaleID

                    ' Check boxes
                    chkDiscontinued.Checked = .Discontinued
                    chkExternalCustomer.Checked = .ExternalCustomer
                    chkCollectionFactory.Checked = .CollectionFromFactory
                    chkPlantation.Checked = .Plantations
                    chkRetailLouvers.Checked = .RetailLouvres
                    chkWholesaleLouvers.Checked = .WholesaleLouvres

                    ViewState("SybizCustomerID") = .SybizCustomerID

                    ' Mailing lists
                    Dim cParams As New QueryParams.MailingListCustomerQueryParams

                    cParams.CustomerIDs.Add(.CustomerID)

                    Dim cMail As List(Of MailingListCustomer) = service.GetMailingListCustomersByParameters(cParams)

                    chkMailingListOrderCompleted.Checked = False
                    chkMailingListOrderConfirmation.Checked = False
                    chkMailingListWeeklyOrderStatusUpdate.Checked = False

                    For Each m As MailingListCustomer In cMail
                        Select Case m.MailingListID
                            Case SharedEnums.MailingListID.OrderCompleted
                                chkMailingListOrderCompleted.Checked = True
                            Case SharedEnums.MailingListID.OrderConfirmation
                                chkMailingListOrderConfirmation.Checked = True
                            Case SharedEnums.MailingListID.WeeklyOrderStatusUpdate
                                chkMailingListWeeklyOrderStatusUpdate.Checked = True
                        End Select
                    Next m

                End With
            End If
        End If

        ' Populate addresses and notes
        PopulateAddressesFromDB()
        PopulateNotesFromDB()
    End Sub

    ''' <summary>
    ''' Populates the viewstate with address data from the database.
    ''' </summary>
    Private Sub PopulateAddressesFromDB()
        Dim service As New AppService
        Dim intCustomerID As Integer

        Integer.TryParse(txtHidCustomerID.Text, intCustomerID)

        Dim dtPostalAddresses As DataTable = service.getAddressRecordsByCustomerIDAndAddressType(intCustomerID, SharedEnums.AddressType.Postal)
        Dim dtPhysicalAddresses As DataTable = service.getAddressRecordsByCustomerIDAndAddressType(intCustomerID, SharedEnums.AddressType.Physical)
        Dim dtDeliveryAddresses As DataTable = service.getAddressRecordsByCustomerIDAndAddressType(intCustomerID, SharedEnums.AddressType.Delivery)

        gdvPostalAddress.DataSource = dtPostalAddresses
        gdvPostalAddress.DataBind()

        gdvPhysicalAddress.DataSource = dtPhysicalAddresses
        gdvPhysicalAddress.DataBind()

        gdvDeliveryAddress.DataSource = dtDeliveryAddresses
        gdvDeliveryAddress.DataBind()

        ' Save into viewstate as data is not persisted to the DB when changed in the table.
        ViewState("PostalAddressesDataTable") = dtPostalAddresses
        ViewState("PhysicalAddressesDataTable") = dtPhysicalAddresses
        ViewState("DeliveryAddressesDataTable") = dtDeliveryAddresses

        PopulateDeliveryInstructionsFromDB()
    End Sub

    ''' <summary>
    ''' Populates the viewstate with delivery instruction data from the database.
    ''' </summary>
    Private Sub PopulateDeliveryInstructionsFromDB()
        Dim service As New AppService
        Dim dicDeliveryInstructions As New Dictionary(Of Integer, DataTable)
        Dim dtDeliveryAddresses As DataTable = ViewState("DeliveryAddressesDataTable")

        ' Pull the instructions for each address from DB
        For i As Integer = 0 To dtDeliveryAddresses.Rows.Count - 1
            Dim intAddressID As Integer = dtDeliveryAddresses.Rows(i).Item("ID")

            ' Map the current address datatable to its AddressID for lookup later.
            dicDeliveryInstructions.Add(intAddressID, (service.getDeliveryInstructionsByAddressID(intAddressID)))
        Next

        ' Put instructions in viewstate
        ViewState("DeliveryInstructionsDictionary") = dicDeliveryInstructions

        ' Rebind delivery addresses so instruction count is updated.
        gdvDeliveryAddress.DataBind()
    End Sub

    ''' <summary>
    ''' Populates the viewstate with notes data from the database.
    ''' </summary>
    Private Sub PopulateNotesFromDB()
        Dim service As New AppService
        Dim intCustomerID As Integer

        Integer.TryParse(txtHidCustomerID.Text, intCustomerID)

        Dim dtNotes As DataTable = service.getNotesByCustomerID(intCustomerID)

        PopulateNotesUIFromDataTable(dtNotes)
    End Sub

    ''' <summary>
    ''' Populates the notes UI from the given <see cref="DataTable"/>.
    ''' </summary>
    ''' <param name="dDataTable">The <see cref="DataTable"/> to bind data from.</param>
    Private Sub PopulateNotesUIFromDataTable(dDataTable As DataTable)
        Dim service As New AppService
        Dim intCustomerID As Integer

        Integer.TryParse(txtHidCustomerID.Text, intCustomerID)

        ' Create a temp data table to alter and bind to the display. Don't want to save it in viewstate.
        Dim dtTempData As New DataTable

        dtTempData.Merge(dDataTable)

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "DisplayHeaderText"
        col.DataType = System.Type.GetType("System.String")
        dtTempData.Columns.Add(col)
        col = Nothing

        Dim dtUsers As DataTable = service.getUsersRecords

        For i As Integer = 0 To dtTempData.Rows.Count - 1
            Dim strText As String = String.Empty
            Dim drows() As DataRow = dtUsers.Select("UserID=" & dtTempData.Rows(i)("UserID").ToString)

            If drows.Length > 0 Then
                strText = "Entered On " & Format(CDate(dtTempData.Rows(i)("CreationDateTime")), "d MMM yyyy h:mm tt") & " by " & drows(0)("UserFirstName").ToString & " " & drows(0)("UserLastName").ToString
            Else
                strText = "Entered on " & Format(CDate(dtTempData.Rows(i)("CreationDateTime")), "d MMM yyyy h:mm tt")
            End If

            dtTempData.Rows(i)("DisplayHeaderText") = strText
        Next

        accNotes.DataSource = New DataTableReader(dtTempData)
        accNotes.DataBind()

        ' Save into viewstate as data is not persisted to the DB when changed in the table.
        ViewState("NotesDataTable") = dDataTable
    End Sub

    Protected Function CountDeliveryInstructions(intAddressID As Integer) As Integer
        ' Get the addressID to datatable map from the viewstate
        Dim dicDeliveryInstructions As Dictionary(Of Integer, DataTable) = ViewState("DeliveryInstructionsDictionary")
        Dim dtDeliveryInstructions As DataTable = Nothing

        If dicDeliveryInstructions IsNot Nothing Then
            ' Get the datatable for the current addressID from the map.
            If dicDeliveryInstructions.TryGetValue(intAddressID, dtDeliveryInstructions) Then
                Return dtDeliveryInstructions.Rows.Count
            End If
        End If

        Return 0
    End Function

    ''' <summary>
    ''' Populates the deliver instructions UI for the given address ID.
    ''' </summary>
    ''' <param name="intAddressID">The address ID to populate the UI with data for.</param>
    Private Sub PopulateDeliveryInstructionsUIForAddressIDFromViewstate(intAddressID As Integer)
        Dim service As New AppService

        ' Get the addressID to datatable map from the viewstate
        Dim dicDeliveryInstructions As Dictionary(Of Integer, DataTable) = ViewState("DeliveryInstructionsDictionary")
        Dim dtDeliveryInstructions As DataTable = Nothing

        ' Get the datatable for the current addressID from the map.
        If dicDeliveryInstructions.TryGetValue(intAddressID, dtDeliveryInstructions) Then

            ' Create a temp data table to alter and bind to the display. Don't want to save it in viewstate.
            Dim dtTempData As New DataTable

            dtTempData.Merge(dtDeliveryInstructions)

            Dim col As DataColumn = New DataColumn
            col.ColumnName = "DisplayHeaderText"
            col.DataType = System.Type.GetType("System.String")
            dtTempData.Columns.Add(col)
            col = Nothing

            Dim dtUsers As DataTable = service.getUsersRecords

            For i As Integer = 0 To dtTempData.Rows.Count - 1
                Dim strText As String = String.Empty
                Dim drows() As DataRow = dtUsers.Select("UserID=" & dtTempData.Rows(i)("UserID").ToString)

                If drows.Length > 0 Then
                    strText = "Entered on " & Format(CDate(dtTempData.Rows(i)("CreationDateTime")), "d MMM yyyy h:mm tt") & " by " & drows(0)("UserFirstName").ToString & " " & drows(0)("UserLastName").ToString
                Else
                    strText = "Entered on " & Format(CDate(dtTempData.Rows(i)("CreationDateTime")), "d MMM yyyy h:mm tt")
                End If

                dtTempData.Rows(i)("DisplayHeaderText") = strText

            Next

            accDeliveryInstructions.DataSource = New DataTableReader(dtTempData)
        Else
            ' No instructions so bind empty.
            Dim dtTempData As New DataTable
            accDeliveryInstructions.DataSource = dtTempData
        End If

        accDeliveryInstructions.DataBind()
    End Sub

    ''' <summary>
    ''' Populates the drop down lists in the add customer form
    ''' </summary>
    Private Sub PopulateDropDownLists()
        ' Sybiz data
        PopulateDDLsFromSybiz()
    End Sub

    ''' <summary>
    ''' Populates all the drop down lists that require data from the Sybiz data tables.
    ''' </summary>
    Private Sub PopulateDDLsFromSybiz()
        Dim service As New AppService
        Dim strSQL As String
        Dim dtPriceScale As DataTable, dtAnalysis As DataTable, dtSort As DataTable

        ' Price Scale
        strSQL = "SELECT * FROM ic.priceScale ORDER BY Description ASC"
        dtPriceScale = service.runSQLOzrollSybiz(strSQL)

        ' Blank row
        Dim drow As DataRow = dtPriceScale.NewRow
        drow("PriceScaleId") = 0
        drow("Description") = String.Empty
        dtPriceScale.Rows.InsertAt(drow, 0)

        ddlPriceScale.DataSource = dtPriceScale
        ddlPriceScale.DataTextField = "Description"
        ddlPriceScale.DataValueField = "PriceScaleId"
        ddlPriceScale.SelectedIndex = 0
        ddlPriceScale.DataBind()

        ' Analysis Code
        strSQL = "SELECT * FROM cm.AnalysisCode WHERE CustomersActive = 1 ORDER BY Description ASC"
        dtAnalysis = service.runSQLOzrollSybiz(strSQL)

        ' Blank row
        drow = dtAnalysis.NewRow
        drow("AnalysisCodeID") = 0
        drow("Description") = String.Empty
        dtAnalysis.Rows.InsertAt(drow, 0)

        ddlAnalysisCode.DataSource = dtAnalysis
        ddlAnalysisCode.DataTextField = "Description"
        ddlAnalysisCode.DataValueField = "AnalysisCodeID"
        ddlAnalysisCode.SelectedIndex = 0
        ddlAnalysisCode.DataBind()

        ' Sort Code
        strSQL = "SELECT * FROM cm.SortCode WHERE CustomerActive = 1 ORDER BY Description ASC"
        dtSort = service.runSQLOzrollSybiz(strSQL)

        ' Blank row
        drow = dtSort.NewRow
        drow("SortCodeId") = 0
        drow("Description") = String.Empty
        dtSort.Rows.InsertAt(drow, 0)

        ddlSortCode.DataSource = dtSort
        ddlSortCode.DataTextField = "Description"
        ddlSortCode.DataValueField = "SortCodeId"
        ddlSortCode.SelectedIndex = 0
        ddlSortCode.DataBind()

        ' Trading Terms
        strSQL = "SELECT * FROM cm.TradingTerm WHERE CustomersActive = 1 ORDER BY Description ASC"
        dtSort = service.runSQLOzrollSybiz(strSQL)

        ' Blank row
        drow = dtSort.NewRow
        drow("TradingTermId") = 0
        drow("Description") = String.Empty
        dtSort.Rows.InsertAt(drow, 0)

        ddlTradingTerms.DataSource = dtSort
        ddlTradingTerms.DataTextField = "Description"
        ddlTradingTerms.DataValueField = "TradingTermId"
        ddlTradingTerms.SelectedIndex = 0
        ddlTradingTerms.DataBind()

        ' Tax Status
        strSQL = "SELECT * FROM core.TaxStatus ORDER BY TaxStatus ASC"
        dtSort = service.runSQLOzrollSybiz(strSQL)

        ' Blank row
        drow = dtSort.NewRow
        drow("TaxStatusId") = 0
        drow("TaxStatus") = String.Empty
        dtSort.Rows.InsertAt(drow, 0)

        ddlTaxStatus.DataSource = dtSort
        ddlTaxStatus.DataTextField = "TaxStatus"
        ddlTaxStatus.DataValueField = "TaxStatusId"
        ddlTaxStatus.SelectedIndex = 0
        ddlTaxStatus.DataBind()

        ' Ozroll Louvre Price Category
        Dim cCategories As List(Of LouvreCategory) = service.GetLouvreCategories()

        ddlLouvreCategory.Items.Clear()
        ddlLouvreCategory.Items.Add(New ListItem(String.Empty, 0))

        For Each c As LouvreCategory In cCategories
            ddlLouvreCategory.Items.Add(New ListItem(c.Name, c.ID))
        Next c

    End Sub


    Private Sub SetAddCustomerMode()
        lblTitle.Visible = True
        lblTitle.Text = "Add Customer"
        btnAddCustomer.Visible = True
        btnUpdateCustomer.Visible = False

        chkMailingListOrderCompleted.Checked = True
        chkMailingListOrderConfirmation.Checked = True
        chkMailingListWeeklyOrderStatusUpdate.Checked = True
    End Sub

    Private Sub SetUpdateCustomerMode()
        lblTitle.Visible = True
        lblTitle.Text = "Update Customer"
        btnAddCustomer.Visible = False
        btnUpdateCustomer.Visible = True
    End Sub

    Protected Sub btnAddCustomer_Click(sender As Object, e As EventArgs) Handles btnAddCustomer.Click
        Page.Validate()

        If Page.IsValid() Then
            AddOrUpdateCustomer(True, False)
            'mpeAddToSybiz.Show()
        End If

        pnlupdateMain.Update()
        pnlupdateAddRemoveCustomerButtons.Update()
    End Sub

    Protected Sub btnDoNotAddCustomerToSybiz_Click(sender As Object, e As EventArgs) Handles btnDoNotAddCustomerToSybiz.Click
        Page.Validate()

        If Page.IsValid() Then
            AddOrUpdateCustomer(True, False)
        End If

        pnlupdateMain.Update()
        pnlupdateAddRemoveCustomerButtons.Update()
    End Sub

    Protected Sub btnAddCustomerToSybiz_Click(sender As Object, e As EventArgs) Handles btnAddCustomerToSybiz.Click
        Page.Validate()

        If Page.IsValid() Then
            AddOrUpdateCustomer(True, True)
        End If

        pnlupdateMain.Update()
        pnlupdateAddRemoveCustomerButtons.Update()
    End Sub

    Private Function GetCustomerFromUI() As Customer
        Dim cCustomer As New Customer
        Dim intCustomerID As Integer
        Dim service As New AppService

        Integer.TryParse(txtHidCustomerID.Text, intCustomerID)

        If intCustomerID > 0 Then
            cCustomer = service.GetCustomerByID(intCustomerID)
        Else
            cCustomer = New Customer
        End If

        With cCustomer
            .CustomerID = IIf(intCustomerID > 0, intCustomerID, DEFAULT_INTEGER_VALUE)

            .CustomerName = txtCustomerName.Text.Trim()

            If String.IsNullOrEmpty(txtFreightPercentage.Text.Trim()) Then
                .FreightPercentage = 0
            Else
                .FreightPercentage = CDec(txtFreightPercentage.Text.Trim()) / 100
            End If

            .CustomerAbbreviation = txtCustomerAbbreviation.Text.Trim()
            .TradingName = txtTradingName.Text.Trim()
            .CustomerPhone1 = txtPhone1.Text.Trim()
            .CustomerPhone2 = txtPhone2.Text.Trim()
            .CustomerPhone3 = txtPhone3.Text.Trim()
            .Email = txtEmail.Text.Trim()
            .ABN = txtABN.Text.Trim()

            If String.IsNullOrEmpty(txtDiscountPercentage.Text.Trim()) Then
                .Discount = 0
            Else
                .Discount = CDec(txtDiscountPercentage.Text.Trim()) / 100
            End If

            ' Drop down lists
            .TradingTermID = ddlTradingTerms.SelectedValue
            .TaxStatusID = ddlTaxStatus.SelectedValue
            .SortCodeID = ddlSortCode.SelectedValue
            .AnalysisCodeID = ddlAnalysisCode.SelectedValue
            .PriceScaleID = ddlPriceScale.SelectedValue
            .LouvreCategoryID = ddlLouvreCategory.SelectedValue

            ' Check boxes
            .Discontinued = chkDiscontinued.Checked
            .ExternalCustomer = chkExternalCustomer.Checked
            .CollectionFromFactory = chkCollectionFactory.Checked
            .Plantations = chkPlantation.Checked
            .WholesaleLouvres = chkWholesaleLouvers.Checked
            .RetailLouvres = chkRetailLouvers.Checked

            .SybizCustomerID = ViewState("SybizCustomerID")

            ' Sort order 1 is reserved for top of list customers. All others are sort order 2.
            If cCustomer.SortOrder = 0 Then
                ' Sort order is unset. Default to 2.
                cCustomer.SortOrder = 2
            End If

        End With

        Return cCustomer
    End Function

    ''' <summary>
    ''' Gets a list off all addresses of the requested status from the UI (stored in viewstate).
    ''' </summary>
    ''' <param name="aAddressType">The <see cref="SharedEnums.AddressType"/> to return.</param>
    ''' <param name="aAddressStatus">The <see cref="AddressStatus"/> to return.</param>
    ''' <returns>A list off all addresses of the requested status from the UI (stored in viewstate).</returns>
    Private Function GetAddressesFromUI(aAddressType As SharedEnums.AddressType, aAddressStatus As AddressStatus) As List(Of Address)
        Dim lAddresses As New List(Of Address)
        Dim dtViewStateAddress As DataTable = Nothing
        Dim dtAddresses As New DataTable

        If aAddressType = SharedEnums.AddressType.Postal Or aAddressType = SharedEnums.AddressType.ALL Then
            dtViewStateAddress = DirectCast(ViewState("PostalAddressesDataTable"), DataTable)
            dtAddresses.Merge(dtViewStateAddress)
        End If

        If aAddressType = SharedEnums.AddressType.Physical Or aAddressType = SharedEnums.AddressType.ALL Then
            dtViewStateAddress = DirectCast(ViewState("PhysicalAddressesDataTable"), DataTable)
            dtAddresses.Merge(dtViewStateAddress)
        End If

        If aAddressType = SharedEnums.AddressType.Delivery Or aAddressType = SharedEnums.AddressType.ALL Then
            dtViewStateAddress = DirectCast(ViewState("DeliveryAddressesDataTable"), DataTable)
            dtAddresses.Merge(dtViewStateAddress)
        End If

        If Not dtAddresses Is Nothing Then
            For i = 0 To dtAddresses.Rows.Count - 1

                Dim intCurID = CInt(dtAddresses.Rows(i)("ID"))
                Dim asStatus = GetAddressStatusForID(intCurID)

                ' Only keep the record if it is the correct status (newly created vs existing entry).
                If asStatus = aAddressStatus Or aAddressStatus = AddressStatus.ALL Then
                    lAddresses.Add(New Address(
                    intCurID,
                    CInt(dtAddresses.Rows(i)("CustomerID")),
                    dtAddresses.Rows(i)("Street").ToString(),
                    dtAddresses.Rows(i)("Suburb").ToString(),
                    dtAddresses.Rows(i)("State").ToString(),
                    dtAddresses.Rows(i)("Postcode").ToString(),
                    dtAddresses.Rows(i)("DeliveryCode").ToString(),
                    CInt(dtAddresses.Rows(i)("AddressType")),
                    CInt(dtAddresses.Rows(i)("IsPrimary")),
                    CInt(dtAddresses.Rows(i)("Discontinued"))
                ))
                End If
            Next
        End If

        Return lAddresses
    End Function

    ''' <summary>
    ''' ''' Gets a list off all NEW delivery instructions from the UI (stored in viewstate).
    ''' </summary>
    ''' <returns>A list off all NEW delivery instructions from the UI (stored in viewstate).</returns>
    Private Function GetNewDeliveryInstructionsFromUI() As List(Of DeliveryInstruction)
        Dim lstInstructions As New List(Of DeliveryInstruction)
        Dim dicDeliveryInstructions As Dictionary(Of Integer, DataTable) = ViewState("DeliveryInstructionsDictionary")

        ' Get only the newly created instructions from the UI and create DI objects.
        For Each kvp As KeyValuePair(Of Integer, DataTable) In dicDeliveryInstructions
            Dim dtInstructions As DataTable = kvp.Value

            For i = 0 To dtInstructions.Rows.Count - 1
                If dtInstructions.Rows(i).Item("ID") < 0 Then
                    ' Negative ID's are new items so keep them. ID is defaulted but it is auto generated by DB identity anyway.
                    lstInstructions.Add(New DeliveryInstruction(
                        SharedConstants.DEFAULT_INTEGER_VALUE,
                        dtInstructions.Rows(i).Item("AddressID"),
                        dtInstructions.Rows(i).Item("InstructionText"),
                        dtInstructions.Rows(i).Item("CreationDateTime"),
                        dtInstructions.Rows(i).Item("UserID")
                    ))
                End If
            Next i
        Next kvp

        Return lstInstructions
    End Function

    ''' <summary>
    ''' ''' Gets a list off all NEW notes from the UI (stored in viewstate).
    ''' </summary>
    ''' <returns>A list off all NEW notes from the UI (stored in viewstate).</returns>
    Private Function GetNewNotesFromUI() As List(Of Note)
        Dim lstNotes As New List(Of Note)

        ' Get only the newly created notes from the UI and create note objects.
        Dim dtNotes As DataTable = ViewState("NotesDataTable")

        For i = 0 To dtNotes.Rows.Count - 1
            If dtNotes.Rows(i).Item("ID") < 0 Then
                ' Negative ID's are new items so keep them. ID is defaulted but it is auto generated by DB identity anyway.
                lstNotes.Add(New Note(
                    SharedConstants.DEFAULT_INTEGER_VALUE,
                    dtNotes.Rows(i).Item("CustomerID"),
                    dtNotes.Rows(i).Item("NoteText"),
                    dtNotes.Rows(i).Item("CreationDateTime"),
                    dtNotes.Rows(i).Item("UserID")
                ))
            End If
        Next i

        Return lstNotes
    End Function

    ''' <summary>
    ''' Gets the status of the given address ID (newly created vs existing entry DB entry).
    ''' </summary>
    ''' <param name="intID">The ID to get the status for.</param>
    ''' <returns>An <see cref="AddressStatus"/> enum inicating the status.</returns>
    Private Function GetAddressStatusForID(intID As Integer) As AddressStatus
        If intID < 0 Then
            Return AddressStatus.NewlyAdded
        End If

        Return AddressStatus.Existing
    End Function

    Protected Sub btnUpdateCustomer_Click(sender As Object, e As EventArgs) Handles btnUpdateCustomer.Click
        Page.Validate()

        If Page.IsValid() Then
            AddOrUpdateCustomer(False, False)
        End If

        pnlupdateMain.Update()
        pnlupdateAddRemoveCustomerButtons.Update()
    End Sub

    Private Sub AddOrUpdateCustomer(IsAdd As Boolean, addToSybiz As Boolean)
        Dim service As New AppService
        Dim dbConn As New DBConnection
        Dim cnn As SqlClient.SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim trans As SqlClient.SqlTransaction = Nothing
        Dim cCustomer As New Customer
        Dim lNewAddresses As List(Of Address)
        Dim lExistingAddresses As List(Of Address)
        Dim lDeliveryInstructions As List(Of DeliveryInstruction)
        Dim lNotes As List(Of Note)
        Dim allOK As Boolean = True
        Dim intCustomerID As Integer
        Dim strExtraErrorMessage As String = String.Empty

        Integer.TryParse(txtHidCustomerID.Text, intCustomerID)

        ' Customer
        cCustomer = GetCustomerFromUI()

        ' Existing possibly edited addresses for update.
        lExistingAddresses = GetAddressesFromUI(SharedEnums.AddressType.ALL, AddressStatus.Existing)

        ' Newly created addresses for add.
        lNewAddresses = GetAddressesFromUI(SharedEnums.AddressType.ALL, AddressStatus.NewlyAdded)

        ' New delivery instructions
        lDeliveryInstructions = GetNewDeliveryInstructionsFromUI()

        ' Notes
        lNotes = GetNewNotesFromUI()

        Try
            ' Execute all in a transaction so we can roll back if any failure occurs.
            cnn.Open()
            trans = cnn.BeginTransaction

            If IsAdd Then
                cCustomer.CustomerID = service.addCustomerRecord(cCustomer, cnn, trans)
            Else
                allOK = service.updateCustomerRecord(cCustomer, cnn, trans)
            End If

            ' If there is no customer ID at this point something has gone wrong.
            allOK = (cCustomer.CustomerID > 0)

            If allOK Then
                For Each a In lNewAddresses
                    ' Remember the original address ID
                    Dim intOriginalID As Integer = a.ID

                    ' New addresses have negative ID's. These will need to be set to default values before writing to DB.
                    a.ID = DEFAULT_INTEGER_VALUE

                    ' New customers will need their new ID set into all address objects.
                    a.CustomerID = cCustomer.CustomerID

                    ' Get the returned address ID then update all delivery instructions with that address ID ONLY IF ITS DELIVERY.
                    Dim intNewID = service.addAddressRecord(a, cnn, trans)

                    If intNewID > 0 Then

                        a.ID = intNewID

                        If a.AddressType = SharedEnums.AddressType.Delivery Then
                            For Each di In lDeliveryInstructions
                                If di.AddressID = intOriginalID Then
                                    di.AddressID = intNewID
                                End If
                            Next
                        End If

                        allOK = True
                    Else
                        allOK = False
                    End If

                    If Not allOK Then
                        Exit For
                    End If
                Next
            End If

            If allOK Then
                For Each a In lExistingAddresses
                    allOK = service.updateAddressRecord(a, cnn, trans)

                    If Not allOK Then
                        Exit For
                    End If
                Next
            End If

            If allOK Then
                For Each di In lDeliveryInstructions
                    ' New delivery instruction ID does not need to change as the table has ID is set to indentity increment.
                    allOK = service.addDeliveryInstructionRecord(di, cnn, trans)

                    If Not allOK Then
                        Exit For
                    End If
                Next
            End If

            If allOK Then
                For Each n In lNotes
                    ' New customers will need their new ID set into all note objects.
                    n.CustomerID = cCustomer.CustomerID

                    ' New notes IDs do not need to change as the table ID is set to indentity increment.
                    allOK = service.addNoteRecord(n, cnn, trans)

                    If Not allOK Then
                        Exit For
                    End If
                Next
            End If

            If allOK Then
                ' Mailing lists
                service.DeleteMailingListCustomerByID(SharedEnums.MailingListID.OrderCompleted, cCustomer.CustomerID, cnn, trans)
                service.DeleteMailingListCustomerByID(SharedEnums.MailingListID.OrderConfirmation, cCustomer.CustomerID, cnn, trans)
                service.DeleteMailingListCustomerByID(SharedEnums.MailingListID.WeeklyOrderStatusUpdate, cCustomer.CustomerID, cnn, trans)

                If chkMailingListOrderCompleted.Checked Then
                    allOK = service.AddMailingListCustomer(SharedEnums.MailingListID.OrderCompleted, cCustomer.CustomerID, cnn, trans)
                End If

                If allOK Then
                    If chkMailingListOrderConfirmation.Checked Then
                        allOK = service.AddMailingListCustomer(SharedEnums.MailingListID.OrderConfirmation, cCustomer.CustomerID, cnn, trans)
                    End If
                End If

                If allOK Then
                    If chkMailingListWeeklyOrderStatusUpdate.Checked Then
                        allOK = service.AddMailingListCustomer(SharedEnums.MailingListID.WeeklyOrderStatusUpdate, cCustomer.CustomerID, cnn, trans)
                    End If
                End If
            End If

            If allOK Then
                If addToSybiz Then
                    ' Save to sybiz
                    Dim lAllCustomerAddresses As New List(Of Address)

                    lAllCustomerAddresses.AddRange(lNewAddresses)
                    lAllCustomerAddresses.AddRange(lExistingAddresses)

                    Dim cPrimaryPostal As Address = lAllCustomerAddresses.Find(Function(x) x.AddressType = SharedEnums.AddressType.Postal AndAlso x.IsPrimary AndAlso Not x.Discontinued)
                    Dim cPrimaryPhysical As Address = lAllCustomerAddresses.Find(Function(x) x.AddressType = SharedEnums.AddressType.Physical AndAlso x.IsPrimary AndAlso Not x.Discontinued)
                    Dim cPrimaryDelivery As Address = lAllCustomerAddresses.Find(Function(x) x.AddressType = SharedEnums.AddressType.Delivery AndAlso x.IsPrimary AndAlso Not x.Discontinued)

                    Dim lPrimaryInstructions As List(Of DeliveryInstruction) = lDeliveryInstructions.FindAll(Function(x) x.AddressID = cPrimaryDelivery.ID)

                    Dim intSybizID As Integer = SaveCustomerToSyBiz(
                                                        cCustomer,
                                                        cPrimaryPostal,
                                                        cPrimaryPhysical,
                                                        cPrimaryDelivery,
                                                        lNotes,
                                                        lPrimaryInstructions
                                                    )

                    If intSybizID > 0 Then
                        ' Update the customer with new sybizID.
                        cCustomer.SybizCustomerID = intSybizID
                        allOK = service.updateCustomerRecord(cCustomer, cnn, trans)

                        If Not allOK Then
                            strExtraErrorMessage = " Sybiz save succeeded but customer save failed."
                        End If
                    Else
                        strExtraErrorMessage = " Sybiz save failed."
                        allOK = False
                    End If
                End If
            End If

            If allOK Then
                trans.Commit()
            Else
                EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - Transaction Rolled Back" & Environment.NewLine & getPageInfo())
                trans.Rollback()
            End If

        Catch ex As Exception
            If Not trans Is Nothing Then
                trans.Rollback()
            End If
            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If
            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message & Environment.NewLine & getPageInfo())
        Finally
            trans.Dispose()
            trans = Nothing
            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If
            cnn.Dispose()
            cnn = Nothing

            lblStatus.Visible = True

            If allOK Then
                Response.Redirect("CustomerDetails.aspx", False)
            Else
                lblStatus.Text = "Error saving details. Please try again." & strExtraErrorMessage
                pnlAddToSybiz.Visible = False
                pnlMain.Visible = False
                pnlAddRemoveCustomerButtons.Visible = False
                pnlResults.Visible = True

                pnlAddToSybizParent.Update()
                pnlupdateResults.Update()
                pnlupdateAddRemoveCustomerButtons.Update()
                pnlupdateMain.Update()
            End If
        End Try
    End Sub

    Private Function SaveCustomerToSyBiz(
            cCustomer As Customer,
            cPrimaryPostalAddress As Address,
            cPrimaryPhysicalAddress As Address,
            cPrimaryDeliveryAddress As Address,
            lNotes As List(Of Note),
            lDeliveryInstructions As List(Of DeliveryInstruction)
        ) As Integer

        Dim intExternalID As Integer = 0
        SyBizShared.LogoutSybiz()
        SyBizShared.PingSybiz()
        Dim newcustomer As SybizDebtors.Customer = SybizDebtors.Customer.GetObject(0)

        With newcustomer
            .Code = cCustomer.Code
            .TradingName = cCustomer.TradingName
            .Name = cCustomer.CustomerName
            .GroupId = 10
            .TradingTermsId = cCustomer.TradingTermID

            If cCustomer.ABN <> String.Empty Then
                .TaxNumber = cCustomer.ABN
            Else
                .TaxNumber = Nothing
            End If

            .TaxStatus = cCustomer.TaxStatusID
            .SortCodeId = cCustomer.SortCodeID
            .AnalysisCodeId = cCustomer.AnalysisCodeID
            .PriceScale = .PriceScale
            .Email = cCustomer.Email
            .Telephone1 = cCustomer.CustomerPhone1
            .Fax = cCustomer.CustomerPhone2
            .Telephone2 = cCustomer.CustomerPhone3

            ' Postal Address
            If cPrimaryPostalAddress IsNot Nothing Then
                .PostalAddress.State = cPrimaryPostalAddress.State
                .PostalAddress.Street = cPrimaryPostalAddress.Street
                .PostalAddress.Suburb = cPrimaryPostalAddress.Suburb
                .PostalAddress.PostCode = cPrimaryPostalAddress.Postcode
            End If

            ' Physical Address
            If cPrimaryPhysicalAddress IsNot Nothing Then
                .PhysicalAddress.Street = cPrimaryPhysicalAddress.Street
                .PhysicalAddress.Suburb = cPrimaryPhysicalAddress.Suburb
                .PhysicalAddress.State = cPrimaryPhysicalAddress.State
                .PhysicalAddress.PostCode = cPrimaryPhysicalAddress.Postcode
            End If

            ' Delivery Address
            If cPrimaryDeliveryAddress IsNot Nothing Then
                Dim varDelivery = .DeliveryAddress.AddNew()

                varDelivery.State = cPrimaryDeliveryAddress.State
                varDelivery.Street = cPrimaryDeliveryAddress.Street
                varDelivery.Suburb = cPrimaryDeliveryAddress.Suburb
                varDelivery.PostCode = cPrimaryDeliveryAddress.Postcode
                varDelivery.DeliveryCode = cPrimaryDeliveryAddress.DeliveryCode
            End If

            ' Delivery Instructions
            Dim strInstructions As String = String.Empty

            For Each i As DeliveryInstruction In lDeliveryInstructions
                ' Concatenate multiple instructions, each on a new line.
                strInstructions &= i.InstructionText & vbCrLf
            Next i

            .DeliveryInstructions = strInstructions

            ' Notes
            Dim strNotes As String = String.Empty

            For Each n As Note In lNotes
                ' Concatenate multiple notes, each on a new line.
                strNotes &= n.NoteText & vbCrLf
            Next n

            .Remarks = strNotes

            If .BrokenRulesCollection.Count = 0 Then
                Dim cSavedCustomer As SybizDebtors.Customer = .Save()

                intExternalID = cSavedCustomer.Id
            End If
        End With

        Return intExternalID
    End Function

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("CustomerDetails.aspx", False)
    End Sub

    Protected Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        Response.Redirect("Home.aspx", False)
    End Sub

    Protected Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Response.Redirect("Logout.aspx", False)
    End Sub

    ''' <summary>
    ''' Validates the given ABN.
    ''' </summary>
    ''' <returns>True if valid, otherwise false.</returns>
    Private Function ValidateABN() As Boolean
        Dim isValid As Boolean = True
        Dim weight As Integer() = {10, 1, 3, 5, 7, 9, 11, 13, 15, 17, 19}
        Dim weightedSum As Integer = 0
        Dim abn As String = txtABN.Text.Trim()

        ' Remove white space
        abn = abn.Replace(" ", "")

        '0. ABN must be 11 digits long
        If String.IsNullOrWhiteSpace(abn) Then
            Return False
        End If

        If Not Regex.IsMatch(abn, "^\d{11}$") Then
            Return False
        End If

        'Rules: 1,2,3                                  
        For i As Integer = 0 To weight.Length - 1
            weightedSum += (Integer.Parse(abn(i).ToString()) - (If((i = 0), 1, 0))) * weight(i)
        Next

        'Rules: 4,5                 
        Return ((weightedSum Mod 89) = 0)
    End Function

    Private Sub gdvPostalAddress_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvPostalAddress.RowDataBound
        Dim idLabel As Label = DirectCast(e.Row.FindControl("lblPostalAddressID"), Label)
        Dim state As DropDownList = DirectCast(e.Row.FindControl("ddlPostalAddressState"), DropDownList)

        If Not state Is Nothing And Not idLabel Is Nothing Then
            SharedFunctions.PopulateStateDDL(state)

            Dim intRowAddressID = CInt(idLabel.Text)
            Dim gridDataSource As DataTable = DirectCast(gdvPostalAddress.DataSource, DataTable)

            ' Find the row in the datatable with the item ID of the edited row.
            For i = 0 To gridDataSource.Rows.Count - 1
                If gridDataSource.Rows(e.Row.RowIndex).Item("ID") = intRowAddressID Then
                    ' Set the state drop down vale from the dataset.
                    state.SelectedValue = gridDataSource.Rows(e.Row.RowIndex).Item("State")
                End If
            Next i
        End If
    End Sub

    Private Sub gdvPostalAddress_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gdvPostalAddress.RowEditing
        ToggleAddressEditMode(False)
        gdvPostalAddress.EditIndex = e.NewEditIndex

        ' load changes from the viewstate datatable
        Dim dtViewstate As DataTable = DirectCast(ViewState("PostalAddressesDataTable"), DataTable)

        ' Populate the table from the viewstate datatable
        gdvPostalAddress.DataSource = dtViewstate
        gdvPostalAddress.DataBind()

        HideNonEditingEditButtons(gdvPostalAddress, "btnPostalAddressEdit", True)
    End Sub

    ''' <summary>
    ''' Hides all edit buttons outside of the editing row.
    ''' </summary>
    ''' <param name="gGridView">The <see cref="GridView"/> to use.</param>
    ''' <param name="strButtonID">The button control ID string.</param>
    ''' <param name="boolHide">Show or hide the buttons.</param>
    Private Sub HideNonEditingEditButtons(gGridView As GridView, strButtonID As String, boolHide As Boolean)

        For Each r As GridViewRow In gGridView.Rows
            If r.RowIndex <> gGridView.SelectedIndex Then
                Dim bEditButton As Button = DirectCast(r.Cells(7).FindControl(strButtonID), Button)

                If Not bEditButton Is Nothing Then
                    If boolHide Then
                        bEditButton.Enabled = False
                        bEditButton.CssClass += " form-button-disabled"
                    Else
                        bEditButton.Enabled = True
                        bEditButton.CssClass = bEditButton.CssClass.Replace(" form-button-disabled", String.Empty)
                    End If
                End If
            End If
        Next r
    End Sub

    ''' <summary>
    ''' Hides all view buttons outside of the editing row.
    ''' </summary>
    ''' <param name="gGridView">The <see cref="GridView"/> to use.</param>
    ''' <param name="strButtonID">The button control ID string.</param>
    ''' <param name="boolHide">Show or hide the buttons.</param>
    Private Sub HideNonEditingViewButtons(gGridView As GridView, strButtonID As String, boolHide As Boolean)

        For Each r As GridViewRow In gGridView.Rows
            Dim bViewButton As Button = DirectCast(r.Cells(7).FindControl(strButtonID), Button)

            If Not bViewButton Is Nothing Then
                If boolHide Then
                    If r.RowIndex <> gGridView.SelectedIndex Then
                        bViewButton.Enabled = False
                        bViewButton.CssClass += " form-button-disabled"
                    End If
                Else
                    bViewButton.Enabled = True
                    bViewButton.CssClass = bViewButton.CssClass.Replace(" form-button-disabled", String.Empty)
                End If
            End If
        Next r
    End Sub

    Private Sub gdvPostalAddress_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gdvPostalAddress.RowCancelingEdit
        gdvPostalAddress.EditIndex = -1
        ToggleAddressEditMode(True)

        ' load changes from the viewstate datatable
        Dim dtViewstate As DataTable = DirectCast(ViewState("PostalAddressesDataTable"), DataTable)

        ' Remove the blank row. Will be the last row added.
        If ViewState("IsAddAddress") = True Then
            If dtViewstate.Rows.Count > 0 Then
                dtViewstate.Rows.RemoveAt(dtViewstate.Rows.Count - 1)
            End If
        End If

        gdvPostalAddress.DataSource = dtViewstate
        gdvPostalAddress.DataBind()

        ViewState("PostalAddressesDataTable") = dtViewstate
        ViewState("IsAddAddress") = False
    End Sub

    Private Sub gdvPostalAddress_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gdvPostalAddress.RowUpdating

        If IsValidationGroupValid("postalAddress") Then
            ' Save changes to the viewstate datatable
            Dim dtViewstate As DataTable = DirectCast(ViewState("PostalAddressesDataTable"), DataTable)
            Dim row As GridViewRow = gdvPostalAddress.Rows(e.RowIndex)

            Dim intID As Integer = CInt(DirectCast(row.FindControl("lblPostalAddressID"), Label).Text)
            Dim intStreet As String = DirectCast(row.FindControl("txtPostalAddressStreet"), TextBox).Text.Trim()
            Dim intSuburb As String = DirectCast(row.FindControl("txtPostalAddressSuburb"), TextBox).Text.Trim()
            Dim intState As String = DirectCast(row.FindControl("ddlPostalAddressState"), DropDownList).SelectedValue
            Dim intPostcode As String = DirectCast(row.FindControl("txtPostalAddressPostcode"), TextBox).Text.Trim()
            Dim intIsPrimary As Integer = CInt(DirectCast(row.FindControl("chkPostalAddressPrimary"), CheckBox).Checked)
            Dim intDiscontinued As Integer = CInt(DirectCast(row.FindControl("chkPostalAddressDiscontinued"), CheckBox).Checked)

            ' Find the ID of the record in the datatable
            For i = 0 To dtViewstate.Rows.Count - 1
                If dtViewstate.Rows(i).Item("ID") = intID Then
                    ' Record matches so update values
                    dtViewstate.Rows(i).Item("Street") = intStreet
                    dtViewstate.Rows(i).Item("Suburb") = intSuburb
                    dtViewstate.Rows(i).Item("State") = intState
                    dtViewstate.Rows(i).Item("Postcode") = intPostcode
                    dtViewstate.Rows(i).Item("IsPrimary") = intIsPrimary
                    dtViewstate.Rows(i).Item("Discontinued") = intDiscontinued

                    Exit For
                End If
            Next

            If intIsPrimary Then
                UncheckOtherPrimaryCheckboxes(intID, dtViewstate)
            End If

            ' Finished editing
            gdvPostalAddress.EditIndex = -1

            gdvPostalAddress.DataSource = dtViewstate
            gdvPostalAddress.DataBind()

            ' Update the viewstate
            ViewState("PostalAddressesDataTable") = dtViewstate

            ToggleAddressEditMode(True)
            ViewState("IsAddAddress") = False
        End If
    End Sub

    ''' <summary>
    ''' Unchecks the ISPrimary attribute of all other addresses except the given address ID.
    ''' </summary>
    ''' <param name="intIDChecked">The address ID that has been checked.</param>
    ''' <param name="dtAddressData">The <see cref="DataTable"/> containing the address data.</param>
    Private Sub UncheckOtherPrimaryCheckboxes(intIDChecked As Integer, dtAddressData As DataTable)
        For Each r As DataRow In dtAddressData.Rows
            If r("ID") <> intIDChecked Then
                r("IsPrimary") = STR_FALSE
            End If
        Next
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
                v.IsValid = True
            End If

            If Not fValid Then
                Return False
            End If
        Next v

        Return True
    End Function

    Private Sub btnPostalAddressAddNew_Click(sender As Object, e As EventArgs) Handles btnPostalAddressAddNew.Click

        ' Add new row to viewstate datatable
        Dim dtViewstate As DataTable = DirectCast(ViewState("PostalAddressesDataTable"), DataTable)
        Dim drNewRow As DataRow
        Dim intCustomerID As Integer

        ' Remember this is an add action as opposed to an edit.
        ViewState("IsAddAddress") = True

        Integer.TryParse(txtHidCustomerID.Text, intCustomerID)

        drNewRow = dtViewstate.NewRow
        drNewRow("ID") = GetUniqueNegativeIDForDataTable(dtViewstate)
        drNewRow("CustomerID") = IIf(intCustomerID > 0, intCustomerID, DEFAULT_INTEGER_VALUE)
        drNewRow("Street") = String.Empty
        drNewRow("Suburb") = String.Empty
        drNewRow("State") = String.Empty
        drNewRow("Postcode") = String.Empty
        drNewRow("DeliveryCode") = 0
        drNewRow("AddressType") = SharedEnums.AddressType.Postal
        drNewRow("IsPrimary") = SharedConstants.STR_FALSE
        drNewRow("Discontinued") = SharedConstants.STR_FALSE

        dtViewstate.Rows.Add(drNewRow)

        gdvPostalAddress.DataSource = dtViewstate
        gdvPostalAddress.DataBind()

        ViewState("PostalAddressesDataTable") = dtViewstate

        ' Set new row to edit mode.
        gdvPostalAddress.EditIndex = gdvPostalAddress.Rows.Count - 1

        ' Have to rebind to get the edit of the new row to trigger.
        gdvPostalAddress.DataBind()

        ToggleAddressEditMode(False)
        HideNonEditingEditButtons(gdvPostalAddress, "btnPostalAddressEdit", True)
    End Sub

    ''' <summary>
    ''' Returns a negative number 1 unit lower than the datatables current lowest negative ID.
    ''' </summary>
    ''' <param name="dtDataTable">The <see cref="DataTable"/> to get a unique negative ID for.</param>
    ''' <returns>A negative number 1 unit lower than the datatables current lowest negative ID.
    ''' If no negative IDs exist, then -1 is returned.</returns>
    Private Function GetUniqueNegativeIDForDataTable(dtDataTable As DataTable) As Integer
        Dim intLowestNumber As Integer = -1

        ' Find the ID of the record in the datatable
        For i = 0 To dtDataTable.Rows.Count - 1
            If dtDataTable.Rows(i).Item("ID") <= intLowestNumber Then
                ' Current is lower so go one lower than it.
                intLowestNumber = dtDataTable.Rows(i).Item("ID") - 1
            End If
        Next

        Return intLowestNumber
    End Function

    Private Sub gdvPhysicalAddress_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvPhysicalAddress.RowDataBound
        Dim idLabel As Label = DirectCast(e.Row.FindControl("lblPhysicalAddressID"), Label)
        Dim state As DropDownList = DirectCast(e.Row.FindControl("ddlPhysicalAddressState"), DropDownList)

        If Not state Is Nothing And Not idLabel Is Nothing Then
            SharedFunctions.PopulateStateDDL(state)

            Dim intRowAddressID = CInt(idLabel.Text)
            Dim gridDataSource As DataTable = DirectCast(gdvPhysicalAddress.DataSource, DataTable)

            ' Find the row in the datatable with the item ID of the edited row.
            For i = 0 To gridDataSource.Rows.Count - 1
                If gridDataSource.Rows(e.Row.RowIndex).Item("ID") = intRowAddressID Then
                    ' Set the state drop down vale from the dataset.
                    state.SelectedValue = gridDataSource.Rows(e.Row.RowIndex).Item("State")
                End If
            Next i
        End If
    End Sub

    Private Sub gdvPhysicalAddress_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gdvPhysicalAddress.RowEditing
        ToggleAddressEditMode(False)
        gdvPhysicalAddress.EditIndex = e.NewEditIndex

        ' load changes from the viewstate datatable
        Dim dtViewstate As DataTable = DirectCast(ViewState("PhysicalAddressesDataTable"), DataTable)

        ' Populate the table from the viewstate datatable
        gdvPhysicalAddress.DataSource = dtViewstate
        gdvPhysicalAddress.DataBind()

        HideNonEditingEditButtons(gdvPhysicalAddress, "btnPhysicalAddressEdit", True)
    End Sub

    Private Sub gdvPhysicalAddress_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gdvPhysicalAddress.RowCancelingEdit
        gdvPhysicalAddress.EditIndex = -1
        ToggleAddressEditMode(True)

        ' load changes from the viewstate datatable
        Dim dtViewstate As DataTable = DirectCast(ViewState("PhysicalAddressesDataTable"), DataTable)

        ' Remove the blank row. Will be the last row added.
        If ViewState("IsAddAddress") = True Then
            If dtViewstate.Rows.Count > 0 Then
                dtViewstate.Rows.RemoveAt(dtViewstate.Rows.Count - 1)
            End If
        End If

        ' Populate the table from the viewstate datatable
        gdvPhysicalAddress.DataSource = dtViewstate
        gdvPhysicalAddress.DataBind()

        ViewState("PhysicalAddressesDataTable") = dtViewstate
        ViewState("IsAddAddress") = False
    End Sub

    Private Sub gdvPhysicalAddress_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gdvPhysicalAddress.RowUpdating
        If IsValidationGroupValid("physicalAddress") Then
            ' Save changes to the viewstate datatable
            Dim dtViewstate As DataTable = DirectCast(ViewState("PhysicalAddressesDataTable"), DataTable)
            Dim row As GridViewRow = gdvPhysicalAddress.Rows(e.RowIndex)

            Dim intID As Integer = CInt(DirectCast(row.FindControl("lblPhysicalAddressID"), Label).Text)
            Dim intStreet As String = DirectCast(row.FindControl("txtPhysicalAddressStreet"), TextBox).Text.Trim()
            Dim intSuburb As String = DirectCast(row.FindControl("txtPhysicalAddressSuburb"), TextBox).Text.Trim()
            Dim intState As String = DirectCast(row.FindControl("ddlPhysicalAddressState"), DropDownList).SelectedValue
            Dim intPostcode As String = DirectCast(row.FindControl("txtPhysicalAddressPostcode"), TextBox).Text.Trim()
            Dim intIsPrimary As Integer = CInt(DirectCast(row.FindControl("chkPhysicalAddressPrimary"), CheckBox).Checked)
            Dim intDiscontinued As Integer = CInt(DirectCast(row.FindControl("chkPhysicalAddressDiscontinued"), CheckBox).Checked)

            ' Find the ID of the record in the datatable
            For i = 0 To dtViewstate.Rows.Count - 1
                If dtViewstate.Rows(i).Item("ID") = intID Then
                    ' Record matches so update values
                    dtViewstate.Rows(i).Item("Street") = intStreet
                    dtViewstate.Rows(i).Item("Suburb") = intSuburb
                    dtViewstate.Rows(i).Item("State") = intState
                    dtViewstate.Rows(i).Item("Postcode") = intPostcode
                    dtViewstate.Rows(i).Item("IsPrimary") = intIsPrimary
                    dtViewstate.Rows(i).Item("Discontinued") = intDiscontinued

                    Exit For
                End If
            Next

            If intIsPrimary Then
                UncheckOtherPrimaryCheckboxes(intID, dtViewstate)
            End If

            ' Finished editing
            gdvPhysicalAddress.EditIndex = -1

            gdvPhysicalAddress.DataSource = dtViewstate
            gdvPhysicalAddress.DataBind()

            ' Update the viewstate
            ViewState("PhysicalAddressesDataTable") = dtViewstate
            ToggleAddressEditMode(True)
            ViewState("IsAddAddress") = False
        End If
    End Sub

    Private Sub btnPhysicalAddressAddNew_Click(sender As Object, e As EventArgs) Handles btnPhysicalAddressAddNew.Click

        ' Add new row to viewstate datatable
        Dim dtViewstate As DataTable = DirectCast(ViewState("PhysicalAddressesDataTable"), DataTable)
        Dim intCustomerID As Integer

        ' Remember this is an add action as opposed to an edit.
        ViewState("IsAddAddress") = True

        Integer.TryParse(txtHidCustomerID.Text, intCustomerID)

        Dim drNewRow As DataRow

        drNewRow = dtViewstate.NewRow
        drNewRow("ID") = GetUniqueNegativeIDForDataTable(dtViewstate)
        drNewRow("CustomerID") = IIf(intCustomerID > 0, intCustomerID, DEFAULT_INTEGER_VALUE)
        drNewRow("Street") = String.Empty
        drNewRow("Suburb") = String.Empty
        drNewRow("State") = String.Empty
        drNewRow("Postcode") = String.Empty
        drNewRow("DeliveryCode") = 0
        drNewRow("AddressType") = SharedEnums.AddressType.Physical
        drNewRow("IsPrimary") = SharedConstants.STR_FALSE
        drNewRow("Discontinued") = SharedConstants.STR_FALSE

        dtViewstate.Rows.Add(drNewRow)

        gdvPhysicalAddress.DataSource = dtViewstate
        gdvPhysicalAddress.DataBind()

        ViewState("PhysicalAddressesDataTable") = dtViewstate

        ' Set new row to edit mode.
        gdvPhysicalAddress.EditIndex = gdvPhysicalAddress.Rows.Count - 1

        ' Have to rebind to get the edit of the new row to trigger.
        gdvPhysicalAddress.DataBind()

        ToggleAddressEditMode(False)
        HideNonEditingEditButtons(gdvPhysicalAddress, "btnPhysicalAddressEdit", True)
    End Sub

    Private Sub gdvDeliveryAddress_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvDeliveryAddress.RowDataBound
        Dim idLabel As Label = DirectCast(e.Row.FindControl("lblDeliveryAddressID"), Label)
        Dim state As DropDownList = DirectCast(e.Row.FindControl("cboDeliveryAddressState"), DropDownList)

        If Not state Is Nothing And Not idLabel Is Nothing Then
            SharedFunctions.PopulateStateDDL(state)

            Dim intRowAddressID = CInt(idLabel.Text)
            Dim gridDataSource As DataTable = DirectCast(gdvDeliveryAddress.DataSource, DataTable)

            ' Find the row in the datatable with the item ID of the edited row.
            For i = 0 To gridDataSource.Rows.Count - 1
                If gridDataSource.Rows(e.Row.RowIndex).Item("ID") = intRowAddressID Then
                    ' Set the state drop down vale from the dataset.
                    state.SelectedValue = gridDataSource.Rows(e.Row.RowIndex).Item("State")
                End If
            Next i
        End If
    End Sub

    Private Sub gdvDeliveryAddress_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gdvDeliveryAddress.RowEditing
        ToggleAddressEditMode(False)

        gdvDeliveryAddress.EditIndex = e.NewEditIndex

        ' load changes from the viewstate datatable
        Dim dtViewstate As DataTable = DirectCast(ViewState("DeliveryAddressesDataTable"), DataTable)

        ' Populate the table from the viewstate datatable
        gdvDeliveryAddress.DataSource = dtViewstate
        gdvDeliveryAddress.DataBind()

        HideNonEditingEditButtons(gdvDeliveryAddress, "btnDeliveryAddressEdit", True)
        HideNonEditingViewButtons(gdvDeliveryAddress, "btnDeliveryAddressInstructions", True)
    End Sub

    Private Sub gdvDeliveryAddress_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gdvDeliveryAddress.RowCancelingEdit
        gdvDeliveryAddress.EditIndex = -1
        ToggleAddressEditMode(True)

        ' load changes from the viewstate datatable
        Dim dtViewstate As DataTable = DirectCast(ViewState("DeliveryAddressesDataTable"), DataTable)

        ' Remove the blank row. Will be the last row added.
        If ViewState("IsAddAddress") = True Then
            If dtViewstate.Rows.Count > 0 Then
                ' Remove from map.
                Dim dicDeliveryInstructions As Dictionary(Of Integer, DataTable) = ViewState("DeliveryInstructionsDictionary")
                dicDeliveryInstructions.Remove(CInt(dtViewstate.Rows(dtViewstate.Rows.Count - 1)("ID")))

                ' Remove from table.
                dtViewstate.Rows.RemoveAt(dtViewstate.Rows.Count - 1)
            End If
        End If

        gdvDeliveryAddress.DataSource = dtViewstate
        gdvDeliveryAddress.DataBind()

        ViewState("IsAddAddress") = False
        ViewState("DeliveryAddressesDataTable") = dtViewstate
    End Sub

    Private Sub ToggleAddAddressButtonsEnabled(boolEnable As Boolean)
        btnDeliveryAddressAddNew.Enabled = boolEnable
        btnPhysicalAddressAddNew.Enabled = boolEnable
        btnPostalAddressAddNew.Enabled = boolEnable

        If Not boolEnable Then
            btnDeliveryAddressAddNew.CssClass += " form-button-disabled"
            btnPhysicalAddressAddNew.CssClass += " form-button-disabled"
            btnPostalAddressAddNew.CssClass += " form-button-disabled"
        Else
            btnDeliveryAddressAddNew.CssClass = btnDeliveryAddressAddNew.CssClass.Replace(" form-button-disabled", String.Empty)
            btnPhysicalAddressAddNew.CssClass = btnPhysicalAddressAddNew.CssClass.Replace(" form-button-disabled", String.Empty)
            btnPostalAddressAddNew.CssClass = btnPostalAddressAddNew.CssClass.Replace(" form-button-disabled", String.Empty)
        End If

        pnlUpdateDeliveryAddress.Update()
        pnlUpdatePostalAddress.Update()
        pnlUpdatePhysicalAddress.Update()
    End Sub

    Private Sub ToggleAddUpdateCustomerButtonEnabled(boolEnable As Boolean)
        btnUpdateCustomer.Enabled = boolEnable
        btnAddCustomer.Enabled = boolEnable

        If Not boolEnable Then
            btnUpdateCustomer.CssClass += " form-button-disabled"
            btnAddCustomer.CssClass += " form-button-disabled"
        Else
            btnUpdateCustomer.CssClass = btnUpdateCustomer.CssClass.Replace(" form-button-disabled", String.Empty)
            btnAddCustomer.CssClass = btnAddCustomer.CssClass.Replace(" form-button-disabled", String.Empty)
        End If

        pnlupdateAddRemoveCustomerButtons.Update()
    End Sub

    ''' <summary>
    ''' Enables/disables UI buttons relating to address editing.
    ''' </summary>
    ''' <param name="boolEnable">True to enable or false to disable the controls.</param>
    Private Sub ToggleAddressEditMode(boolEnable As Boolean)
        ToggleAddUpdateCustomerButtonEnabled(boolEnable)
        ToggleAddAddressButtonsEnabled(boolEnable)

        HideNonEditingEditButtons(gdvPhysicalAddress, "btnPhysicalAddressEdit", Not boolEnable)
        HideNonEditingEditButtons(gdvDeliveryAddress, "btnDeliveryAddressEdit", Not boolEnable)
        HideNonEditingEditButtons(gdvPostalAddress, "btnPostalAddressEdit", Not boolEnable)
        HideNonEditingViewButtons(gdvDeliveryAddress, "btnDeliveryAddressInstructions", Not boolEnable)
    End Sub

    Private Sub gdvDeliveryAddress_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gdvDeliveryAddress.RowUpdating
        If IsValidationGroupValid("deliveryAddress") Then
            ' Save changes to the viewstate datatable
            Dim dtViewstate As DataTable = DirectCast(ViewState("DeliveryAddressesDataTable"), DataTable)
            Dim row As GridViewRow = gdvDeliveryAddress.Rows(e.RowIndex)

            Dim intID As Integer = CInt(DirectCast(row.FindControl("lblDeliveryAddressID"), Label).Text)
            Dim intStreet As String = DirectCast(row.FindControl("txtDeliveryAddressStreet"), TextBox).Text.Trim()
            Dim intSuburb As String = DirectCast(row.FindControl("txtDeliveryAddressSuburb"), TextBox).Text.Trim()
            Dim intState As String = DirectCast(row.FindControl("cboDeliveryAddressState"), DropDownList).SelectedValue
            Dim intPostcode As String = DirectCast(row.FindControl("txtDeliveryAddressPostcode"), TextBox).Text.Trim()
            Dim intIsPrimary As Integer = CInt(DirectCast(row.FindControl("chkDeliveryAddressPrimary"), CheckBox).Checked)
            Dim intDiscontinued As Integer = CInt(DirectCast(row.FindControl("chkDeliveryAddressDiscontinued"), CheckBox).Checked)

            ' Find the ID of the record in the datatable
            For i = 0 To dtViewstate.Rows.Count - 1
                If dtViewstate.Rows(i).Item("ID") = intID Then
                    ' Record matches so update values
                    dtViewstate.Rows(i).Item("Street") = intStreet
                    dtViewstate.Rows(i).Item("Suburb") = intSuburb
                    dtViewstate.Rows(i).Item("State") = intState
                    dtViewstate.Rows(i).Item("Postcode") = intPostcode
                    dtViewstate.Rows(i).Item("IsPrimary") = intIsPrimary
                    dtViewstate.Rows(i).Item("Discontinued") = intDiscontinued

                    Exit For
                End If
            Next

            If intIsPrimary Then
                UncheckOtherPrimaryCheckboxes(intID, dtViewstate)
            End If

            ' Finished editing
            gdvDeliveryAddress.EditIndex = -1

            gdvDeliveryAddress.DataSource = dtViewstate
            gdvDeliveryAddress.DataBind()

            ' Update the viewstate
            ViewState("DeliveryAddressesDataTable") = dtViewstate
            ToggleAddressEditMode(True)
            ViewState("IsAddAddress") = False
        End If
    End Sub

    Private Sub btnDeliveryAddressAddNew_Click(sender As Object, e As EventArgs) Handles btnDeliveryAddressAddNew.Click
        ' Add new row to viewstate datatable
        Dim dtViewstate As DataTable = DirectCast(ViewState("DeliveryAddressesDataTable"), DataTable)
        Dim intCustomerID As Integer

        ' Remember this is an add action as opposed to an edit.
        ViewState("IsAddAddress") = True

        Integer.TryParse(txtHidCustomerID.Text, intCustomerID)

        Dim drNewRow As DataRow

        drNewRow = dtViewstate.NewRow
        drNewRow("ID") = GetUniqueNegativeIDForDataTable(dtViewstate)
        drNewRow("CustomerID") = IIf(intCustomerID > 0, intCustomerID, DEFAULT_INTEGER_VALUE)
        drNewRow("Street") = String.Empty
        drNewRow("Suburb") = String.Empty
        drNewRow("State") = String.Empty
        drNewRow("Postcode") = String.Empty
        drNewRow("DeliveryCode") = 0
        drNewRow("AddressType") = SharedEnums.AddressType.Delivery
        drNewRow("IsPrimary") = SharedConstants.STR_FALSE
        drNewRow("Discontinued") = SharedConstants.STR_FALSE

        dtViewstate.Rows.Add(drNewRow)

        gdvDeliveryAddress.DataSource = dtViewstate
        gdvDeliveryAddress.DataBind()

        ViewState("DeliveryAddressesDataTable") = dtViewstate

        ' Set new row to edit mode.
        gdvDeliveryAddress.EditIndex = gdvDeliveryAddress.Rows.Count - 1

        ' Have to rebind to get the edit of the new row to trigger.
        gdvDeliveryAddress.DataBind()

        ToggleAddressEditMode(False)

        HideNonEditingEditButtons(gdvDeliveryAddress, "btnDeliveryAddressEdit", True)
        HideNonEditingViewButtons(gdvDeliveryAddress, "btnDeliveryAddressInstructions", True)
    End Sub

    Private Sub gdvDeliveryAddress_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvDeliveryAddress.RowCommand
        Dim service As New AppService

        If e.CommandName = "DeliveryInstructions" And e.CommandArgument IsNot Nothing Then
            ' Get the ID of the record on the row.
            Dim intRowIdx = CInt(e.CommandArgument)
            Dim intAddressID = CInt(DirectCast(gdvDeliveryAddress.Rows(intRowIdx).FindControl("lblDeliveryAddressID"), Label).Text)

            ViewState("DeliveryAddressesIDEditing") = intAddressID

            ' Populate the notes in the popup and show it.
            Dim mpe As ModalPopupExtender = DirectCast(gdvDeliveryAddress.Rows(intRowIdx).FindControl("ModalPopupExtender"), ModalPopupExtender)

            PopulateDeliveryInstructionsUIForAddressIDFromViewstate(intAddressID)
            mpe.Show()

            pnlUpdateDeliveryInstructions.Update()

        ElseIf e.CommandName = "Copy" Then

            ' Copy the address to the users session clipboard.
            Dim dtViewStateAddress As DataTable = DirectCast(ViewState("DeliveryAddressesDataTable"), DataTable)
            Dim drAddress As DataRow = dtViewStateAddress.Select("ID = '" & e.CommandArgument & "'").FirstOrDefault()

            Session("Clipboard") = service.ConvertAddressRecordToObject(drAddress)

        ElseIf e.CommandName = "Paste" Then

            If Session("Clipboard") IsNot Nothing AndAlso TypeOf Session("Clipboard") Is Address Then

                Dim cAddress As Address = Session("Clipboard")
                Dim row As GridViewRow = DirectCast(e.CommandSource, Button).NamingContainer

                DirectCast(row.FindControl("txtDeliveryAddressStreet"), TextBox).Text = cAddress.Street
                DirectCast(row.FindControl("txtDeliveryAddressSuburb"), TextBox).Text = cAddress.Suburb
                DirectCast(row.FindControl("cboDeliveryAddressState"), DropDownList).SelectedValue = cAddress.State
                DirectCast(row.FindControl("txtDeliveryAddressPostcode"), TextBox).Text = cAddress.Postcode
                DirectCast(row.FindControl("chkDeliveryAddressPrimary"), CheckBox).Checked = cAddress.IsPrimary
                DirectCast(row.FindControl("chkDeliveryAddressDiscontinued"), CheckBox).Checked = cAddress.Discontinued

            End If
        End If
    End Sub

    Private Sub btnAddNewInstruction_Click(sender As Object, e As EventArgs) Handles btnAddNewInstruction.Click
        txtNewInstructionText.Text = String.Empty
        pnlAddInstruction.Visible = True
    End Sub

    Private Sub btnCancelInstruction_Click(sender As Object, e As EventArgs) Handles btnCancelInstruction.Click
        txtNewInstructionText.Text = String.Empty
        pnlAddInstruction.Visible = False
    End Sub

    Private Sub btnCloseInstruction_Click(sender As Object, e As EventArgs) Handles btnCloseInstruction.Click
        Dim dtViewstate As DataTable = DirectCast(ViewState("DeliveryAddressesDataTable"), DataTable)

        gdvDeliveryAddress.DataSource() = dtViewstate
        gdvDeliveryAddress.DataBind()
    End Sub

    Private Sub btnSaveInstruction_Click(sender As Object, e As EventArgs) Handles btnSaveInstruction.Click

        Dim service As New AppService

        ' Get instruction map from viewstate
        Dim dicInstructions As Dictionary(Of Integer, DataTable) = ViewState("DeliveryInstructionsDictionary")

        ' Get the datatable for the address ID
        Dim intAddressID As Integer = ViewState("DeliveryAddressesIDEditing")

        ' Dont bother to save an empty instruction
        If txtNewInstructionText.Text.Trim().Length() > 0 Then
            Dim dtInstructions As DataTable = Nothing

            dicInstructions.TryGetValue(intAddressID, dtInstructions)

            If dtInstructions Is Nothing Then
                ' Dont have any instructions for this address yet so create a table and map it.
                dtInstructions = New DataTable

                With dtInstructions
                    .Columns.Add("ID")
                    .Columns.Add("AddressID")
                    .Columns.Add("InstructionText")
                    .Columns.Add("CreationDateTime")
                    .Columns.Add("UserID")
                End With

                dicInstructions.Add(intAddressID, dtInstructions)
            End If

            ' Add new intruction to collection with unique -ID.
            Dim drNewRow As DataRow

            drNewRow = dtInstructions.NewRow
            drNewRow("ID") = GetUniqueNegativeIDForDataTable(dtInstructions)
            drNewRow("AddressID") = intAddressID
            drNewRow("InstructionText") = txtNewInstructionText.Text.Trim()
            drNewRow("CreationDateTime") = Date.Now
            drNewRow("UserID") = CInt(Session("sessUserID"))

            dtInstructions.Rows.Add(drNewRow)

            ' Save the instruction map into the viewstate
            ViewState("DeliveryInstructionsDictionary") = dicInstructions
        End If

        ' Reset UI
        txtNewInstructionText.Text = String.Empty
        pnlAddInstruction.Visible = False

        ' Reload instructions for this address
        PopulateDeliveryInstructionsUIForAddressIDFromViewstate(intAddressID)
    End Sub

    Private Sub btnAddNewNote_Click(sender As Object, e As EventArgs) Handles btnAddNewNote.Click
        txtNewNoteText.Text = String.Empty
        pnlAddNote.Visible = True
        btnAddNewNote.Visible = False
    End Sub

    Private Sub btnCancelNote_Click(sender As Object, e As EventArgs) Handles btnCancelNote.Click
        txtNewNoteText.Text = String.Empty
        pnlAddNote.Visible = False
        btnAddNewNote.Visible = True
    End Sub

    Private Sub btnSaveNote_Click(sender As Object, e As EventArgs) Handles btnSaveNote.Click

        ' Dont bother to save an empty note
        If txtNewNoteText.Text.Trim().Length() > 0 Then
            Dim service As New AppService

            ' Get notes from viewstate
            Dim dtNotes As DataTable = ViewState("NotesDataTable")
            Dim intCustomerID As Integer

            Integer.TryParse(txtHidCustomerID.Text, intCustomerID)

            ' Add new note to datatable with unique -ID.
            Dim drNewRow As DataRow

            drNewRow = dtNotes.NewRow
            drNewRow("ID") = GetUniqueNegativeIDForDataTable(dtNotes)
            drNewRow("CustomerID") = IIf(intCustomerID > 0, intCustomerID, DEFAULT_INTEGER_VALUE)
            drNewRow("NoteText") = txtNewNoteText.Text.Trim()
            drNewRow("CreationDateTime") = Date.Now
            drNewRow("UserID") = CInt(Session("sessUserID"))

            dtNotes.Rows.Add(drNewRow)

            ' Save the notes into the viewstate
            ViewState("NotesDataTable") = dtNotes
        End If

        ' Reset UI
        txtNewNoteText.Text = String.Empty
        pnlAddNote.Visible = False

        ' Reload instructions for this address
        PopulateNotesUIFromDataTable(ViewState("NotesDataTable"))
        btnAddNewNote.Visible = True
    End Sub

    Protected Sub valcusABN_ServerValidate(source As Object, args As ServerValidateEventArgs)
        args.IsValid = ValidateABN()
    End Sub

    Private Sub gdvPostalAddress_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvPostalAddress.RowCommand
        Dim service As New AppService

        If e.CommandName = "Copy" Then

            ' Copy the address to the users session clipboard.
            Dim dtViewStateAddress As DataTable = DirectCast(ViewState("PostalAddressesDataTable"), DataTable)
            Dim drAddress As DataRow = dtViewStateAddress.Select("ID = '" & e.CommandArgument & "'").FirstOrDefault()

            Session("Clipboard") = service.ConvertAddressRecordToObject(drAddress)

        ElseIf e.CommandName = "Paste" Then

            If Session("Clipboard") IsNot Nothing AndAlso TypeOf Session("Clipboard") Is Address Then

                Dim cAddress As Address = Session("Clipboard")
                Dim row As GridViewRow = DirectCast(e.CommandSource, Button).NamingContainer

                DirectCast(row.FindControl("txtPostalAddressStreet"), TextBox).Text = cAddress.Street
                DirectCast(row.FindControl("txtPostalAddressSuburb"), TextBox).Text = cAddress.Suburb
                DirectCast(row.FindControl("ddlPostalAddressState"), DropDownList).SelectedValue = cAddress.State
                DirectCast(row.FindControl("txtPostalAddressPostcode"), TextBox).Text = cAddress.Postcode
                DirectCast(row.FindControl("chkPostalAddressPrimary"), CheckBox).Checked = cAddress.IsPrimary
                DirectCast(row.FindControl("chkPostalAddressDiscontinued"), CheckBox).Checked = cAddress.Discontinued

            End If
        End If
    End Sub

    Private Sub gdvPhysicalAddress_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvPhysicalAddress.RowCommand
        Dim service As New AppService

        If e.CommandName = "Copy" Then

            ' Copy the address to the users session clipboard.
            Dim dtViewStateAddress As DataTable = DirectCast(ViewState("PhysicalAddressesDataTable"), DataTable)
            Dim drAddress As DataRow = dtViewStateAddress.Select("ID = '" & e.CommandArgument & "'").FirstOrDefault()

            Session("Clipboard") = service.ConvertAddressRecordToObject(drAddress)

        ElseIf e.CommandName = "Paste" Then

            If Session("Clipboard") IsNot Nothing AndAlso TypeOf Session("Clipboard") Is Address Then

                Dim cAddress As Address = Session("Clipboard")
                Dim row As GridViewRow = DirectCast(e.CommandSource, Button).NamingContainer

                DirectCast(row.FindControl("txtPhysicalAddressStreet"), TextBox).Text = cAddress.Street
                DirectCast(row.FindControl("txtPhysicalAddressSuburb"), TextBox).Text = cAddress.Suburb
                DirectCast(row.FindControl("ddlPhysicalAddressState"), DropDownList).SelectedValue = cAddress.State
                DirectCast(row.FindControl("txtPhysicalAddressPostcode"), TextBox).Text = cAddress.Postcode
                DirectCast(row.FindControl("chkPhysicalAddressPrimary"), CheckBox).Checked = cAddress.IsPrimary
                DirectCast(row.FindControl("chkPhysicalAddressDiscontinued"), CheckBox).Checked = cAddress.Discontinued

            End If
        End If
    End Sub

    Private Sub btnSybizLink_Click(sender As Object, e As EventArgs) Handles btnSybizLink.Click
        Dim service As New AppService

        ' Default the sybiz UI elements to 0
        ddlTradingTerms.SelectedValue = 0
        ddlTaxStatus.SelectedValue = 0
        ddlSortCode.SelectedValue = 0
        ddlAnalysisCode.SelectedValue = 0
        ddlPriceScale.SelectedValue = 0

        ViewState("SybizCustomerID") = 0

        If txtCode.Text.Trim().Count > 0 Then
            Dim cSybizCustomer As SybizCustomer = service.GetSybizCustomerDataByCode(txtCode.Text.Trim())

            btnSybizLink.Style.Item("display") = "none"
            txtCode.Enabled = False

            If cSybizCustomer.SybizCustomerID > 0 Then
                imgSybizLinked.ImageUrl = "~/images/green_tick_32x32.png"
                imgSybizLinked.ToolTip = "Linked to Sybiz"
                btnChangeSybizCode.Enabled = True
                btnChangeSybizCode.Style.Item("display") = "inline"

                ' Set the sybiz UI elements only
                ddlTradingTerms.SelectedValue = cSybizCustomer.TradingTermId
                ddlTaxStatus.SelectedValue = cSybizCustomer.TaxStatusId
                ddlSortCode.SelectedValue = cSybizCustomer.SortCodeId
                ddlAnalysisCode.SelectedValue = cSybizCustomer.AnalysisCodeId
                ddlPriceScale.SelectedValue = cSybizCustomer.PriceScaleId

                ViewState("SybizCustomerID") = cSybizCustomer.SybizCustomerID

            Else
                imgSybizLinked.ImageUrl = "~/images/error_x_32x32.png"
                imgSybizLinked.ToolTip = "Error linking to Sybiz"
            End If
        Else
            imgSybizLinked.ImageUrl = String.Empty
            imgSybizLinked.ToolTip = String.Empty
            btnChangeSybizCode.Enabled = True
            btnChangeSybizCode.Style.Item("display") = "inline"
            txtCode.Enabled = False
        End If

    End Sub

    Private Enum AddressStatus
        ALL = 0
        Existing
        NewlyAdded
    End Enum

    Protected Sub valcustCustomerName_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim cValidator As CustomValidator = source

        args.IsValid = True
        cValidator.Text = String.Empty

        If String.IsNullOrWhiteSpace(args.Value) Then
            args.IsValid = False
            cValidator.Text = "Customer name required."
        End If

        If args.IsValid Then
            If Not SharedFunctions.StringContainsValidFilenameCharacters(args.Value) Then
                args.IsValid = False
                cValidator.Text = "Customer name cannot contain /:\*?""<>"
            End If
        End If
    End Sub

    Protected Sub valcustTradingName_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim cValidator As CustomValidator = source

        args.IsValid = True
        cValidator.Text = String.Empty

        If String.IsNullOrWhiteSpace(args.Value) Then
            args.IsValid = False
            cValidator.Text = "Trading name required."
        End If

        If args.IsValid Then
            If Not SharedFunctions.StringContainsValidFilenameCharacters(args.Value) Then
                args.IsValid = False
                cValidator.Text = "Trading name cannot contain /:\*?""<>"
            End If
        End If
    End Sub
End Class
