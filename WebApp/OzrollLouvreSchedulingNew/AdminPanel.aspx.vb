Imports System.Data.SqlClient
Imports System.IO
Imports System.Linq
Imports AjaxControlToolkit
Imports OzrollPSLVSchedulingModel.SharedEnums

Partial Class AdminPanel
    Inherits System.Web.UI.Page

    Dim _Service As New AppService
    Dim _UserPermissions As UserPermissionsContainer

    Dim _LouvreCategories As List(Of LouvreCategory) = Nothing
    Dim _LouvreStyles As List(Of LouvreStyle) = Nothing
    Dim _LouvreTypes As List(Of LouvreType) = Nothing

    Private Sub AdminPanel_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim ExitPage As Boolean = False

        If Session("sessUserID") = String.Empty Then
            ExitPage = True
        Else
            If Not IsNumeric(Session("sessUserID")) Then
                ExitPage = True
            End If
        End If

        ' Permission check
        If Not ExitPage Then
            If Not _Service.Permissions.UserHasPermission(Session("sessUserID"), Permissions.Admin) Then
                ExitPage = True
            End If
        End If

        If ExitPage = True Then
            Response.Redirect("Logout.aspx", False)
            Exit Sub
        End If

        If Not IsPostBack Then

            Dim intLoginID As Integer = Session("sessUserID")


            _Service.AddWebsitePageAccess("Ozroll Customer Portal", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)

            SetInitialUIState()
        End If

        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "styleFocusElements", "styleFocusElements();", True)
    End Sub

    Private Sub CheckPermissionsUILockdown()
        Dim cUserPermissions As UserPermissionsContainer = _Service.Permissions.UserHasPermissions(Session("sessUserID"))
        btnLogins.Visible = False
        'btnLogins.Visible = cUserPermissions.HasPermission(Permissions.AdminManageLogins)
        btnAddressZones.Visible = cUserPermissions.HasPermission(Permissions.AdminManageAddressZones)
        btnExtraProducts.Visible = cUserPermissions.HasPermission(Permissions.AdminManageExtras)
        btnLouvrePrices.Visible = cUserPermissions.HasPermission(Permissions.AdminManageLouvrePrices)
        btnColours.Visible = cUserPermissions.HasPermission(Permissions.AdminManageColours)
        btnPermissions.Visible = cUserPermissions.HasPermission(Permissions.AdminManagePermissions)
        btnLog.Visible = cUserPermissions.HasPermission(Permissions.AdminViewLog)
        btnAddUser.Visible = cUserPermissions.HasPermission(Permissions.AddUser)

    End Sub

    Private Sub SetInitialUIState()

        pnlAdminMenu.Visible = True

        CheckPermissionsUILockdown()

        HideAllPanels()
    End Sub

    Private Sub HideAllPanels()
        pnlLogins.Visible = False
        pnlAddressZones.Visible = False
        pnlExtraProducts.Visible = False
        pnlLouvrePrices.Visible = False
        pnlColours.Visible = False
        pnlPermissions.Visible = False
        pnlLog.Visible = False
    End Sub

    Private Sub btnLogins_Click(sender As Object, e As EventArgs) Handles btnLogins.Click
        ResetTables()

        If _Service.Permissions.UserHasPermission(Session("sessUserID"), Permissions.AdminManageLogins) Then
            PopulateLogins()
        End If
    End Sub

    Private Sub btnAddressZones_Click(sender As Object, e As EventArgs) Handles btnAddressZones.Click
        ResetTables()

        If _Service.Permissions.UserHasPermission(Session("sessUserID"), Permissions.AdminManageAddressZones) Then
            PopulateAddressZones()
        End If
    End Sub

    Private Sub btnExtraProducts_Click(sender As Object, e As EventArgs) Handles btnExtraProducts.Click
        ResetTables()

        If _Service.Permissions.UserHasPermission(Session("sessUserID"), Permissions.AdminManageExtras) Then
            PopulateExtraProducts()
        End If
    End Sub

    Private Sub btnLouvrePrices_Click(sender As Object, e As EventArgs) Handles btnLouvrePrices.Click
        ResetTables()

        If _Service.Permissions.UserHasPermission(Session("sessUserID"), Permissions.AdminManageLouvrePrices) Then
            PopulateLouvrePriceDDLs()
            PopulateLouvrePrices()
        End If
    End Sub

    Private Sub btnColours_Click(sender As Object, e As EventArgs) Handles btnColours.Click
        ResetTables()

        If _Service.Permissions.UserHasPermission(Session("sessUserID"), Permissions.AdminManageColours) Then
            PopulateColours()
        End If
    End Sub

    Private Sub btnPermissions_Click(sender As Object, e As EventArgs) Handles btnPermissions.Click
        ResetTables()

        If _Service.Permissions.UserHasPermission(Session("sessUserID"), Permissions.AdminManagePermissions) Then
            PopulatePermissionsFilterUI()
            PopulatePermissions()
        End If
    End Sub

    Private Sub btnLog_Click(sender As Object, e As EventArgs) Handles btnLog.Click
        ResetTables()

        If _Service.Permissions.UserHasPermission(Session("sessUserID"), Permissions.AdminViewLog) Then
            PopulateLog()
        End If
    End Sub

    Private Sub ResetTables()
        gdvAddressZones.ShowFooter = False
        gdvAddressZones.EditIndex = -1
        ToggleAddAddressZoneButtonsEnabled(True)

        gdvExtraProducts.ShowFooter = False
        gdvExtraProducts.EditIndex = -1
        ToggleAddExtraProductButtonsEnabled(True)
    End Sub

#Region "Logins"

    Private Sub PopulateLogins()

        HideAllPanels()
        pnlLogins.Visible = True

        Dim lLogins As List(Of LoginDetails) = _Service.GetLoginDetails()

        If Not chkLoginsShowDiscontinued.Checked Then
            ' Remove discontinued logins.
            lLogins = lLogins.FindAll(Function(x) Not x.Discontinued)

            If lLogins Is Nothing Then
                lLogins = New List(Of LoginDetails)
            End If
        End If

        gdvLogins.DataSource = lLogins
        gdvLogins.DataBind()

    End Sub

    Private Sub chkLoginsShowDiscontinued_CheckedChanged(sender As Object, e As EventArgs) Handles chkLoginsShowDiscontinued.CheckedChanged
        PopulateLogins()
    End Sub

    Protected Sub gdvLogins_LinkSort(sender As Object, e As CommandEventArgs)
        If (e.CommandName = "Sort") Then
            SortLoginsGridView(e.CommandArgument)
        End If
    End Sub

    Private Sub gdvLogins_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gdvLogins.Sorting
        SortLoginsGridView(e.SortExpression)
    End Sub

    Private Sub SortLoginsGridView(strColumn As String)
        PopulateLogins()

        Dim strPreviousSortColumn As String = gdvLogins.Attributes("SortColumn")
        Dim strSortDirection As String = gdvLogins.Attributes("SortDirection")
        Dim lLogins As List(Of LoginDetails) = gdvLogins.DataSource

        If String.IsNullOrEmpty(strSortDirection) Then
            ' No previous sort direction so set it to default of ASC.
            strSortDirection = SortDirection.Ascending
        End If

        If Not String.IsNullOrEmpty(strPreviousSortColumn) Then
            ' If the colomn was the last column sorted, flip the direction.
            If strPreviousSortColumn = strColumn Then

                If strSortDirection = SortDirection.Ascending Then
                    strSortDirection = SortDirection.Descending
                Else
                    strSortDirection = SortDirection.Ascending
                End If
            Else
                ' New column being sorted so default to ASC.
                strSortDirection = SortDirection.Ascending
            End If
        End If

        Select Case strColumn
            Case "LoginName"
                If strSortDirection = SortDirection.Ascending Then
                    lLogins = lLogins.OrderBy(Function(x) x.LoginName).ToList()
                Else
                    lLogins = lLogins.OrderByDescending(Function(x) x.LoginName).ToList()
                End If

            Case "FirstName"
                If strSortDirection = SortDirection.Ascending Then
                    lLogins = lLogins.OrderBy(Function(x) x.FirstName).ToList()
                Else
                    lLogins = lLogins.OrderByDescending(Function(x) x.FirstName).ToList()
                End If

            Case "LastName"
                If strSortDirection = SortDirection.Ascending Then
                    lLogins = lLogins.OrderBy(Function(x) x.LastName).ToList()
                Else
                    lLogins = lLogins.OrderByDescending(Function(x) x.LastName).ToList()
                End If

            Case "EmailAddress"
                If strSortDirection = SortDirection.Ascending Then
                    lLogins = lLogins.OrderBy(Function(x) x.EmailAddress).ToList()
                Else
                    lLogins = lLogins.OrderByDescending(Function(x) x.EmailAddress).ToList()
                End If

            Case "Customer"
                If strSortDirection = SortDirection.Ascending Then
                    lLogins = lLogins.OrderBy(Function(x) GetCustomerNameByID(x.CustomerID)).ToList()
                Else
                    lLogins = lLogins.OrderByDescending(Function(x) GetCustomerNameByID(x.CustomerID)).ToList()
                End If

            Case "Administrator"
                If strSortDirection = SortDirection.Ascending Then
                    lLogins = lLogins.OrderBy(Function(x) x.AdminUser).ToList()
                Else
                    lLogins = lLogins.OrderByDescending(Function(x) x.AdminUser).ToList()
                End If

            Case "Discontinued"
                If strSortDirection = SortDirection.Ascending Then
                    lLogins = lLogins.OrderBy(Function(x) x.Discontinued).ToList()
                Else
                    lLogins = lLogins.OrderByDescending(Function(x) x.Discontinued).ToList()
                End If

        End Select

        ' Remember the last sorted colomn and its direction so we can flip the direction next time.
        gdvLogins.Attributes("SortColumn") = strColumn
        gdvLogins.Attributes("SortDirection") = strSortDirection

        gdvLogins.DataSource = lLogins
        gdvLogins.DataBind()
    End Sub

    Private Sub gdvLogins_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvLogins.RowCommand

        ViewState("LoginIDEditing") = 0
        lblLoginPopupStatus.Visible = False

        If e.CommandName = "EditLogin" Then
            Dim intLoginID As Integer = e.CommandArgument
            Dim cLogin As LoginDetails = _Service.GetLoginDetailsByID(intLoginID)

            If cLogin.LoginID > 0 Then

                ViewState("LoginIDEditing") = cLogin.LoginID

                ' Populate the Login popup.
                txtLoginEmail.Text = cLogin.EmailAddress
                txtLoginFirstName.Text = cLogin.FirstName
                txtLoginLastName.Text = cLogin.LastName
                txtLoginUserName.Text = cLogin.LoginName
                chkLoginIsAdmin.Checked = cLogin.AdminUser
                chkLoginIsDiscontinued.Checked = cLogin.Discontinued

                PopulateCustomers(cLogin.CustomerID)
                PopulateDeliveryAddresses(cLogin.DeliveryAddressID, cLogin.CustomerID)

                lblLoginPopupHeader.Text = "Edit Login"

                btnSendPasswordResetEmail.Visible = True
                txtLoginUserName.Enabled = False

                mpeLogin.Show()
            End If
        End If
    End Sub

    Private Sub ddlLoginPopupCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlLoginPopupCustomer.SelectedIndexChanged
        PopulateDeliveryAddresses(0, ddlLoginPopupCustomer.SelectedValue)

        mpeLogin.Show()
    End Sub

    Private Sub PopulateDeliveryAddresses(intDeliveryAddressID As Integer, intCustomerID As Integer)

        ddlDeliveryAddress.Items.Clear()

        If intCustomerID > 0 Then
            Dim lAddresses As List(Of Address) = _Service.GetAddressesByCustomerIDAndAddressType(intCustomerID, AddressType.Delivery)

            lAddresses = lAddresses.FindAll(Function(x) Not x.Discontinued)

            If lAddresses IsNot Nothing Then

                ddlDeliveryAddress.Items.Add(New ListItem(String.Empty, 0))

                For Each a As Address In lAddresses
                    ddlDeliveryAddress.Items.Add(New ListItem(a.ToString, a.ID))

                    If a.ID = intDeliveryAddressID Then
                        ddlDeliveryAddress.SelectedValue = a.ID
                    End If
                Next a
            End If
        End If
    End Sub

    Protected Function GetCustomerNameByID(intCustomerID As Integer) As String
        If intCustomerID > 0 Then
            Dim cCustomerID As Customer = _Service.GetCustomerByID(intCustomerID)

            If cCustomerID.CustomerID > 0 Then
                Return cCustomerID.CustomerName
            End If
        End If

        Return String.Empty
    End Function

    Private Sub PopulateCustomers(intSelectedCustomerID As Integer)
        Dim lCustomers As List(Of Customer) = _Service.GetCustomers().FindAll(Function(x) Not x.Discontinued).OrderBy(Function(x) x.CustomerName).ToList()

        If lCustomers IsNot Nothing Then
            ddlLoginPopupCustomer.Items.Clear()

            ddlLoginPopupCustomer.Items.Add(New ListItem(String.Empty, 0))

            For Each c As Customer In lCustomers
                ddlLoginPopupCustomer.Items.Add(New ListItem(c.CustomerName, c.CustomerID))

                If c.CustomerID = intSelectedCustomerID Then
                    ddlLoginPopupCustomer.SelectedValue = c.CustomerID
                End If
            Next c
        End If
    End Sub

    Private Sub btnAddNewLogin_Click(sender As Object, e As EventArgs) Handles btnAddNewLogin.Click

        ViewState("LoginIDEditing") = 0
        lblLoginPopupStatus.Visible = False

        ' Populate the Login popup.
        txtLoginEmail.Text = String.Empty
        txtLoginFirstName.Text = String.Empty
        txtLoginLastName.Text = String.Empty
        txtLoginUserName.Text = String.Empty
        chkLoginIsAdmin.Checked = False
        chkLoginIsDiscontinued.Checked = False

        PopulateCustomers(0)
        PopulateDeliveryAddresses(0, 0)

        lblLoginPopupHeader.Text = "Create Login"

        btnSendPasswordResetEmail.Visible = False
        txtLoginUserName.Enabled = True

        mpeLogin.Show()
    End Sub

    Private Sub btnSaveLogin_Click(sender As Object, e As EventArgs) Handles btnSaveLogin.Click

        Page.Validate()

        If Page.IsValid() Then
            Dim intLoginID As Integer = ViewState("LoginIDEditing")
            Dim cLogin As LoginDetails = Nothing
            Dim cOldLogin As LoginDetails = Nothing
            Dim success As Boolean = False
            Dim boolNewAccount As Boolean = False
            Dim intCustomerID As Integer = 0

            If IsNumeric(ddlLoginPopupCustomer.SelectedValue) Then
                intCustomerID = ddlLoginPopupCustomer.SelectedValue
            End If

            If intLoginID > 0 Then
                ' Edit
                cLogin = _Service.GetLoginDetailsByID(intLoginID)

                If cLogin.LoginID > 0 Then

                    ' Remember for logging later.
                    cOldLogin = cLogin.Clone

                    With cLogin
                        .AdminUser = chkLoginIsAdmin.Checked
                        .CustomerID = intCustomerID
                        .DeliveryAddressID = ddlDeliveryAddress.SelectedValue
                        .Discontinued = chkLoginIsDiscontinued.Checked
                        .EmailAddress = txtLoginEmail.Text
                        .FirstName = txtLoginFirstName.Text
                        .LastName = txtLoginLastName.Text
                        .LoginName = txtLoginUserName.Text

                        ' Dont change password related properties.
                        .Password = cLogin.Password
                        .PasswordResetKey = cLogin.PasswordResetKey
                        .PasswordResetKeyExpiryDateTime = cLogin.PasswordResetKeyExpiryDateTime
                    End With

                    success = _Service.UpdateLoginDetails(cLogin)
                Else
                    ' Not found. Something gone wrong.
                End If
            Else
                ' Add
                cLogin = New LoginDetails

                With cLogin
                    .AdminUser = chkLoginIsAdmin.Checked
                    .CustomerID = intCustomerID
                    .DeliveryAddressID = ddlDeliveryAddress.SelectedValue
                    .Discontinued = chkLoginIsDiscontinued.Checked
                    .EmailAddress = txtLoginEmail.Text
                    .FirstName = txtLoginFirstName.Text
                    .LastName = txtLoginLastName.Text
                    .LoginName = txtLoginUserName.Text
                    .Password = String.Empty

                    ' Generate a new reset key and give them 2 days to activate it.
                    .PasswordResetKey = Guid.NewGuid.ToString
                    .PasswordResetKeyExpiryDateTime = Date.Now.AddDays(2)
                End With

                cLogin.LoginID = _Service.AddLoginDetails(cLogin)

                success = (cLogin.LoginID > 0)
                boolNewAccount = True
            End If

            If success Then

                If boolNewAccount Then

                    ' Log it
                    Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                            Session("sessuserID"),
                                            LogCategory.AdminLoginChange,
                                            LogChangeType.Add,
                                            String.Empty,
                                            cLogin.LogString,
                                            "New login created.",
                                            Date.Now)

                    _Service.Log.AddLogEntry(cLog)

                    ' Send an email
                    Dim encString As String = HttpUtility.UrlEncode(SharedFunctions.Encrypt("key=" & cLogin.PasswordResetKey & ",id=" & cLogin.LoginID))

                    _Service.Mail.SendPasswordResetEmail(SharedConstants.SiteURL(Request) & "/PasswordResetLouvres.aspx?var1=" & encString, cLogin, boolNewAccount, Session("sessUserName"))
                Else
                    ' Log it
                    Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                            Session("sessuserID"),
                                            LogCategory.AdminLoginChange,
                                            LogChangeType.Edit,
                                            cOldLogin.LogString,
                                            cLogin.LogString,
                                            "Login edited.",
                                            Date.Now)

                    _Service.Log.AddLogEntry(cLog)
                End If

                ViewState("LoginIDEditing") = 0
                mpeLogin.Hide()
                PopulateLogins()
            Else
                ' Show an error.
                lblLoginPopupStatus.Visible = True
                lblLoginPopupStatus.Text = "An error occured while attempting to save."
                lblLoginPopupStatus.ForeColor = Drawing.Color.Red

                mpeLogin.Show()
            End If
        Else
            mpeLogin.Show()
        End If

    End Sub

    Private Sub btnCancelLogin_Click(sender As Object, e As EventArgs) Handles btnCancelLogin.Click
        ViewState("LoginIDEditing") = 0
        mpeLogin.Hide()
    End Sub

    Private Sub btnSendPasswordResetEmail_Click(sender As Object, e As EventArgs) Handles btnSendPasswordResetEmail.Click
        Dim intLoginID As Integer = ViewState("LoginIDEditing")

        If intLoginID > 0 Then
            Dim cLogin As LoginDetails = _Service.GetLoginDetailsByID(intLoginID)

            If cLogin.LoginID > 0 Then

                ' Generate a new reset key and give them 2 days to activate it.
                cLogin.PasswordResetKey = Guid.NewGuid.ToString
                cLogin.PasswordResetKeyExpiryDateTime = Date.Now.AddDays(2)

                If _Service.UpdateLoginDetails(cLogin) Then

                    ' Log it
                    Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                            Session("sessuserID"),
                                            LogCategory.AdminLoginChange,
                                            LogChangeType.Edit,
                                            String.Empty,
                                            String.Empty,
                                            "Password reset email sent to " & cLogin.LoginName & ".",
                                            Date.Now)

                    _Service.Log.AddLogEntry(cLog)

                    Dim encString As String = HttpUtility.UrlEncode(SharedFunctions.Encrypt("key=" & cLogin.PasswordResetKey & ",id=" & cLogin.LoginID))

                    _Service.Mail.SendPasswordResetEmail(SharedConstants.SiteURL(Request) & "/PasswordResetLouvres.aspx?var1=" & encString, cLogin, False, Session("sessUsername"))

                    lblLoginPopupStatus.Visible = True
                    lblLoginPopupStatus.Text = "Email sent."
                    lblLoginPopupStatus.ForeColor = Drawing.Color.Green
                Else
                    lblLoginPopupStatus.Visible = True
                    lblLoginPopupStatus.Text = "An error occured. Please try again."
                    lblLoginPopupStatus.ForeColor = Drawing.Color.Red
                End If
            End If
        End If

        mpeLogin.Show()
    End Sub

    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click

        Response.Redirect("Logout.aspx", False)
    End Sub

    Protected Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        Response.Redirect("Home.aspx")
    End Sub

    Private Sub valcustUsername_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valcustUsername.ServerValidate

        ' Ensure username is unique
        Dim cValidator As CustomValidator = DirectCast(source, CustomValidator)
        Dim intLoginIDEditing As Integer = ViewState("LoginIDEditing")
        Dim strLoginName As String = args.Value
        Dim cLogins As List(Of LoginDetails) = _Service.GetLoginDetailsByParameters(, strLoginName)

        If intLoginIDEditing > 0 Then
            ' Edit so dont count self in DB.
            cLogins = cLogins.FindAll(Function(x) x.LoginID <> intLoginIDEditing)

            If cLogins Is Nothing Then
                cLogins = New List(Of LoginDetails)
            End If
        End If

        args.IsValid = (cLogins.Count = 0)

        If Not args.IsValid Then
            cValidator.Text = "Username already exists."
        End If
    End Sub

#End Region

#Region "AddressZones"

    Private Sub PopulateAddressZones()

        HideAllPanels()
        pnlAddressZones.Visible = True

        SortAdressZonesGridView(gdvAddressZones.Attributes("SortColumn"), True)

    End Sub

    Private Function GetAddressZonesBySearchCriteria() As List(Of AddressZone)
        Dim lAddressZones As List(Of AddressZone) = _Service.GetAddressZones()

        Return lAddressZones
    End Function

    Private Sub gdvAddressZones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvAddressZones.RowCommand

        Select Case e.CommandName
            Case "EditAddressZoneRanges"
                Dim intZoneID = e.CommandArgument
                Dim cZone As AddressZone = _Service.GetAddressZones().Find(Function(x) x.ID = intZoneID)

                If cZone IsNot Nothing Then

                    lblAddressZoneHeader.Text = cZone.ZoneName

                    gdvAddressZoneRanges.DataSource = cZone.AddressZoneRanges
                    gdvAddressZoneRanges.DataBind()

                    ViewState("AddressZoneIDEditing") = intZoneID

                    mpeAddressZone.Show()
                End If

            Case "SaveNewAddressZone"

                Page.Validate("NewAddressZone")

                If Page.IsValid() Then
                    Dim strName As String = DirectCast(gdvAddressZones.FooterRow.FindControl("txtNewAddressZoneName"), TextBox).Text.Trim
                    Dim decPrice As Decimal = DirectCast(gdvAddressZones.FooterRow.FindControl("txtNewAddressZonePrice"), TextBox).Text.Trim
                    Dim dteEffectiveDateTime As Date = DirectCast(gdvAddressZones.FooterRow.FindControl("txtNewAddressZoneEffectiveDate"), TextBox).Text.Trim

                    ' If valid
                    SaveNewAddressZone(strName, decPrice, dteEffectiveDateTime)
                End If

            Case "CancelNewAddressZone"
                gdvAddressZones.ShowFooter = False
                PopulateAddressZones()
                ToggleAddAddressZoneButtonsEnabled(True)

        End Select
    End Sub

    Protected Sub gdvAddressZones_LinkSort(sender As Object, e As CommandEventArgs)
        If (e.CommandName = "Sort") Then
            SortAdressZonesGridView(e.CommandArgument)
        End If
    End Sub

    Private Sub SortAdressZonesGridView(strSortColumn As String, Optional boolMaintainSortDirection As Boolean = False)
        Dim strPreviousSortColumn As String = gdvAddressZones.Attributes("SortColumn")
        Dim strSortDirection As String = gdvAddressZones.Attributes("SortDirection")
        Dim lAddressZones As List(Of AddressZone) = GetAddressZonesBySearchCriteria()

        If String.IsNullOrEmpty(strSortDirection) Then
            ' No previous sort direction so set it to default of ASC.
            strSortDirection = SortDirection.Ascending
        End If

        If Not boolMaintainSortDirection Then
            If Not String.IsNullOrEmpty(strPreviousSortColumn) Then
                ' If the colomn was the last column sorted, flip the direction.
                If strPreviousSortColumn = strSortColumn Then
                    If strSortDirection = SortDirection.Ascending Then
                        strSortDirection = SortDirection.Descending
                    Else
                        strSortDirection = SortDirection.Ascending
                    End If
                Else
                    ' New column being sorted so default to ASC.
                    strSortDirection = SortDirection.Ascending
                End If
            End If
        End If

        Select Case strSortColumn
            Case "ZoneName"
                If strSortDirection = SortDirection.Ascending Then
                    lAddressZones = lAddressZones.OrderBy(Function(x) x.ZoneName).ToList()
                Else
                    lAddressZones = lAddressZones.OrderByDescending(Function(x) x.ZoneName).ToList()
                End If

            Case "Price"
                If strSortDirection = SortDirection.Ascending Then
                    lAddressZones = lAddressZones.OrderBy(Function(x) x.Price).ToList()
                Else
                    lAddressZones = lAddressZones.OrderByDescending(Function(x) x.Price).ToList()
                End If

            Case "Effective"
                If strSortDirection = SortDirection.Ascending Then
                    lAddressZones = lAddressZones.OrderBy(Function(x) x.EffectiveDateTime).ToList()
                Else
                    lAddressZones = lAddressZones.OrderByDescending(Function(x) x.EffectiveDateTime).ToList()
                End If

            Case "Created"
                If strSortDirection = SortDirection.Ascending Then
                    lAddressZones = lAddressZones.OrderBy(Function(x) x.CreationDateTime).ToList()
                Else
                    lAddressZones = lAddressZones.OrderByDescending(Function(x) x.CreationDateTime).ToList()
                End If

        End Select

        ' Remember the last sorted colomn and its direction so we can flip the direction next time.
        gdvAddressZones.Attributes("SortColumn") = strSortColumn
        gdvAddressZones.Attributes("SortDirection") = strSortDirection

        gdvAddressZones.DataSource = lAddressZones
        gdvAddressZones.DataBind()
    End Sub

    Protected Sub valcustNewAddressZoneEffectiveDate_Validate(source As Object, e As ServerValidateEventArgs)
        Dim dteDate As Date

        e.IsValid = False

        If Not String.IsNullOrWhiteSpace(e.Value.Trim) Then
            e.IsValid = Date.TryParse(e.Value, dteDate)
        End If
    End Sub

    Private Sub gdvAddressZoneRanges_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvAddressZoneRanges.RowCommand

        Select Case e.CommandName
            Case "DeleteAddressZoneRange"
                Dim intRangeID As Integer = e.CommandArgument
                Dim intZoneID As Integer = ViewState("AddressZoneIDEditing")
                Dim cAddressZone As AddressZone = _Service.GetAddressZones().Find(Function(x) x.ID = intZoneID)
                Dim cRangeToDelete As AddressZoneRange = cAddressZone.AddressZoneRanges.Find(Function(x) x.ID = intRangeID)

                If cRangeToDelete IsNot Nothing Then

                    _Service.DeleteAddressZoneRangeByID(intRangeID)

                    ' Log add details
                    Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                             Session("sessUserID"),
                                             LogCategory.AdminAddressZoneChange,
                                             LogChangeType.Delete,
                                             cRangeToDelete.LogString(),
                                             String.Empty,
                                             "Address zone range deleted.",
                                             Date.Now)

                    _Service.Log.AddLogEntry(cLog)

                    ' Refresh list from DB to ensure data has been updated in the DB.
                    Dim cZone As AddressZone = _Service.GetAddressZones().Find(Function(x) x.ID = intZoneID)

                    If cZone IsNot Nothing Then
                        gdvAddressZoneRanges.DataSource = cZone.AddressZoneRanges
                        gdvAddressZoneRanges.DataBind()
                    End If
                End If

        End Select

        mpeAddressZone.Show()

    End Sub

    Private Sub ToggleAddAddressZoneButtonsEnabled(boolEnable As Boolean)

        pnlAddressZonesFilterCriteria.Enabled = boolEnable

        btnAddAddressZone.Visible = boolEnable

        ' Disable sorting links in header.
        If gdvAddressZones.HeaderRow IsNot Nothing Then
            For Each c As TableCell In gdvAddressZones.HeaderRow.Cells
                For Each cont As Control In c.Controls
                    If TypeOf cont Is LinkButton Then
                        DirectCast(cont, LinkButton).Enabled = boolEnable
                    End If
                Next cont
            Next c
        End If

        EnableDisableNonEditingEditButtons(gdvAddressZones, "btnEditAddressZoneRanges", 5, Not boolEnable)
        EnableDisableNonEditingEditButtons(gdvAddressZones, "btnEditAddressZone", 5, Not boolEnable)
    End Sub

    Private Sub ToggleAddAddressZoneRangeButtonsEnabled(boolEnable As Boolean)

        btnAddAddressZoneRange.Visible = boolEnable
        btnCloseAddressZoneRangePopup.Visible = boolEnable

        ' Disable sorting links in header.
        If gdvAddressZoneRanges.HeaderRow IsNot Nothing Then
            For Each c As TableCell In gdvAddressZoneRanges.HeaderRow.Cells
                For Each cont As Control In c.Controls
                    If TypeOf cont Is LinkButton Then
                        DirectCast(cont, LinkButton).Enabled = boolEnable
                    End If
                Next cont
            Next c
        End If

        EnableDisableNonEditingEditButtons(gdvAddressZoneRanges, "btnDeleteAddressZoneRange", 3, Not boolEnable)
    End Sub

    Private Sub btnAddAddressZoneRange_Click(sender As Object, e As EventArgs) Handles btnAddAddressZoneRange.Click
        pnlAddAddressZoneRange.Visible = True

        txtAddAddressZoneRangeEndPostcode.Text = String.Empty
        txtAddAddressZoneRangeStartPostcode.Text = String.Empty

        ToggleAddAddressZoneRangeButtonsEnabled(False)

        mpeAddressZone.Show()
    End Sub

    Private Sub btnCancelAddressZoneRange_Click(sender As Object, e As EventArgs) Handles btnCancelAddressZoneRange.Click
        pnlAddAddressZoneRange.Visible = False

        ToggleAddAddressZoneRangeButtonsEnabled(True)

        mpeAddressZone.Show()
    End Sub

    Private Sub btnSaveAddressZoneRange_Click(sender As Object, e As EventArgs) Handles btnSaveAddressZoneRange.Click
        Dim intZoneID As Integer = ViewState("AddressZoneIDEditing")
        Dim cZone As AddressZone = _Service.GetAddressZones().Find(Function(x) x.ID = intZoneID)

        If cZone IsNot Nothing Then

            Dim cNewRange As New AddressZoneRange

            cNewRange.AddressZoneID = intZoneID
            cNewRange.Start = txtAddAddressZoneRangeStartPostcode.Text.Trim
            cNewRange.End = txtAddAddressZoneRangeEndPostcode.Text.Trim

            cZone.AddressZoneRanges.Add(cNewRange)

            cNewRange.ID = _Service.AddOrUpdateAddressZone(cZone)

            If cNewRange.ID > 0 Then

                ' Log add details
                Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                         Session("sessUserID"),
                                         LogCategory.AdminAddressZoneChange,
                                         LogChangeType.Add,
                                         String.Empty,
                                         cNewRange.LogString(),
                                         "New address zone range added.",
                                         Date.Now)

                _Service.Log.AddLogEntry(cLog)

            End If
        End If

        ' Refresh list from DB to ensure data has been updated in the DB.
        cZone = _Service.GetAddressZones().Find(Function(x) x.ID = intZoneID)

        If cZone IsNot Nothing Then
            gdvAddressZoneRanges.DataSource = cZone.AddressZoneRanges
            gdvAddressZoneRanges.DataBind()
        End If

        pnlAddAddressZoneRange.Visible = False

        ToggleAddAddressZoneRangeButtonsEnabled(True)

        mpeAddressZone.Show()
    End Sub

    Private Sub btnCloseAddressZoneRangePopup_Click(sender As Object, e As EventArgs) Handles btnCloseAddressZoneRangePopup.Click
        pnlAddAddressZoneRange.Visible = False

        mpeAddressZone.Hide()
    End Sub

    Private Sub btnAddAddressZone_Click(sender As Object, e As EventArgs) Handles btnAddAddressZone.Click
        gdvAddressZones.ShowFooter = True
        PopulateAddressZones()
        ToggleAddAddressZoneButtonsEnabled(False)
    End Sub

    Private Sub SaveNewAddressZone(strZoneName As String,
                                   decPrice As Decimal,
                                   dteEffectiveDateTime As Date)

        Dim cNewZone As New AddressZone

        With cNewZone
            .CreationDateTime = Date.Now
            .EffectiveDateTime = dteEffectiveDateTime
            .Price = decPrice
            .ZoneName = strZoneName
        End With

        cNewZone.ID = _Service.AddOrUpdateAddressZone(cNewZone)

        If cNewZone.ID > 0 Then

            ' Log add details
            Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                        Session("sessUserID"),
                                        LogCategory.AdminAddressZoneChange,
                                        LogChangeType.Add,
                                        String.Empty,
                                        cNewZone.LogString(),
                                        "New address zone added.",
                                        Date.Now)

            _Service.Log.AddLogEntry(cLog)

        End If

        gdvAddressZones.ShowFooter = False
        PopulateAddressZones()
        ToggleAddAddressZoneButtonsEnabled(True)

    End Sub

    Private Sub gdvAddressZones_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gdvAddressZones.RowEditing

        gdvAddressZones.EditIndex = e.NewEditIndex

        ' Populate the table
        SortAdressZonesGridView(gdvAddressZones.Attributes("SortColumn"), True)

        ToggleAddAddressZoneButtonsEnabled(False)
    End Sub

    Private Sub gdvAddressZones_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gdvAddressZones.RowUpdating

        Page.Validate("NewAddressZone")

        If Page.IsValid() Then

            Dim row As GridViewRow = gdvAddressZones.Rows(e.RowIndex)

            Dim intID As Integer = DirectCast(row.FindControl("lblAddressZoneID"), Label).Text
            Dim strZoneName As String = DirectCast(row.FindControl("txtNewAddressZoneName"), TextBox).Text.Trim
            Dim decPrice As Decimal = DirectCast(row.FindControl("txtNewAddressZonePrice"), TextBox).Text.Trim
            Dim dteEffectiveDate As Date = DirectCast(row.FindControl("txtNewAddressZoneEffectiveDate"), TextBox).Text.Trim

            Dim cAddressZone As AddressZone = _Service.GetAddressZones().Find(Function(x) x.ID = intID)

            If cAddressZone.ID > 0 Then

                ' For logging later
                Dim cBeforeAddressZone As AddressZone = cAddressZone.Clone

                With cAddressZone
                    .EffectiveDateTime = dteEffectiveDate

                    If decPrice > 0 Then
                        .Price = (decPrice / 100)
                    Else
                        .Price = 0
                    End If

                    .ZoneName = strZoneName
                End With

                If _Service.AddOrUpdateAddressZone(cAddressZone) > 0 Then
                    ' Log update details
                    Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                                Session("sessUserID"),
                                                LogCategory.AdminAddressZoneChange,
                                                LogChangeType.Edit,
                                                cBeforeAddressZone.LogString(),
                                                cAddressZone.LogString(),
                                                "Address zone edited.",
                                                Date.Now)

                    _Service.Log.AddLogEntry(cLog)
                End If

            End If

            gdvAddressZones.EditIndex = -1

            ' Populate the table
            SortAdressZonesGridView(gdvAddressZones.Attributes("SortColumn"), True)

        End If
    End Sub

    Private Sub gdvAddressZones_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gdvAddressZones.RowCancelingEdit

        gdvAddressZones.EditIndex = -1

        ' Populate the table
        SortAdressZonesGridView(gdvAddressZones.Attributes("SortColumn"), True)

        ToggleAddAddressZoneButtonsEnabled(True)
    End Sub

    Private Sub gdvAddressZoneRanges_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gdvAddressZoneRanges.Sorting
        SortAdressZoneRangesGridView(e.SortExpression)
    End Sub

    Private Sub SortAdressZoneRangesGridView(strSortColumn As String, Optional boolMaintainSortDirection As Boolean = False)
        Dim strPreviousSortColumn As String = gdvAddressZoneRanges.Attributes("SortColumn")
        Dim strSortDirection As String = gdvAddressZoneRanges.Attributes("SortDirection")
        Dim intZoneID As Integer = ViewState("AddressZoneIDEditing")
        Dim cZone As AddressZone = _Service.GetAddressZones().Find(Function(x) x.ID = intZoneID)
        Dim lAddressZoneRanges As List(Of AddressZoneRange) = cZone.AddressZoneRanges

        If String.IsNullOrEmpty(strSortDirection) Then
            ' No previous sort direction so set it to default of ASC.
            strSortDirection = SortDirection.Ascending
        End If

        If Not boolMaintainSortDirection Then
            If Not String.IsNullOrEmpty(strPreviousSortColumn) Then
                ' If the colomn was the last column sorted, flip the direction.
                If strPreviousSortColumn = strSortColumn Then
                    If strSortDirection = SortDirection.Ascending Then
                        strSortDirection = SortDirection.Descending
                    Else
                        strSortDirection = SortDirection.Ascending
                    End If
                Else
                    ' New column being sorted so default to ASC.
                    strSortDirection = SortDirection.Ascending
                End If
            End If
        End If

        Select Case strSortColumn
            Case "Start"
                If strSortDirection = SortDirection.Ascending Then
                    lAddressZoneRanges = lAddressZoneRanges.OrderBy(Function(x) x.Start).ToList()
                Else
                    lAddressZoneRanges = lAddressZoneRanges.OrderByDescending(Function(x) x.Start).ToList()
                End If

            Case "End"
                If strSortDirection = SortDirection.Ascending Then
                    lAddressZoneRanges = lAddressZoneRanges.OrderBy(Function(x) x.End).ToList()
                Else
                    lAddressZoneRanges = lAddressZoneRanges.OrderByDescending(Function(x) x.End).ToList()
                End If

        End Select

        ' Remember the last sorted colomn and its direction so we can flip the direction next time.
        gdvAddressZoneRanges.Attributes("SortColumn") = strSortColumn
        gdvAddressZoneRanges.Attributes("SortDirection") = strSortDirection

        gdvAddressZoneRanges.DataSource = lAddressZoneRanges
        gdvAddressZoneRanges.DataBind()
    End Sub

#End Region

#Region "Extras"

    Private Sub PopulateExtraProducts()

        HideAllPanels()
        pnlExtraProducts.Visible = True

        SortExtraProductsGridView(gdvExtraProducts.Attributes("SortColumn"), True)

    End Sub

    Private Function GetExtraProductListForSearchCriteria() As List(Of ExtraProductLouvres)
        Dim lExtraProducts As List(Of ExtraProductLouvres) = _Service.GetExtraProductLouvresList()

        If Not chkExtraProductsShowDiscontinued.Checked Then
            ' Remove discontinued products
            lExtraProducts = lExtraProducts.FindAll(Function(x) Not x.Discontinued)
        End If

        Return lExtraProducts
    End Function

    Private Sub chkExtraProductsShowDiscontinued_CheckedChanged(sender As Object, e As EventArgs) Handles chkExtraProductsShowDiscontinued.CheckedChanged
        PopulateExtraProducts()
    End Sub

    Protected Sub gdvExtraProducts_LinkSort(sender As Object, e As CommandEventArgs)
        If (e.CommandName = "Sort") Then
            SortExtraProductsGridView(e.CommandArgument)
        End If
    End Sub

    Private Sub SortExtraProductsGridView(strSortColumn As String, Optional boolMaintainSortDirection As Boolean = False)
        Dim strPreviousSortColumn As String = gdvExtraProducts.Attributes("SortColumn")
        Dim strSortDirection As String = gdvExtraProducts.Attributes("SortDirection")
        Dim lExtraProducts As List(Of ExtraProductLouvres) = GetExtraProductListForSearchCriteria()


        If String.IsNullOrEmpty(strSortDirection) Then
            ' No previous sort direction so set it to default of ASC.
            strSortDirection = SortDirection.Ascending
        End If

        If Not boolMaintainSortDirection Then
            If Not String.IsNullOrEmpty(strPreviousSortColumn) Then
                ' If the colomn was the last column sorted, flip the direction.
                If strPreviousSortColumn = strSortColumn Then
                    If strSortDirection = SortDirection.Ascending Then
                        strSortDirection = SortDirection.Descending
                    Else
                        strSortDirection = SortDirection.Ascending
                    End If
                Else
                    ' New column being sorted so default to ASC.
                    strSortDirection = SortDirection.Ascending
                End If
            End If
        End If

        Select Case strSortColumn
            Case "Description"
                If strSortDirection = SortDirection.Ascending Then
                    lExtraProducts = lExtraProducts.OrderBy(Function(x) x.Description).ToList()
                Else
                    lExtraProducts = lExtraProducts.OrderByDescending(Function(x) x.Description).ToList()
                End If

            Case "ProductCode"
                If strSortDirection = SortDirection.Ascending Then
                    lExtraProducts = lExtraProducts.OrderBy(Function(x) x.ProductCode).ToList()
                Else
                    lExtraProducts = lExtraProducts.OrderByDescending(Function(x) x.ProductCode).ToList()
                End If

            Case "UnitOfMeasurement"
                If strSortDirection = SortDirection.Ascending Then
                    lExtraProducts = lExtraProducts.OrderBy(Function(x) x.UnitOfMeasurement).ToList()
                Else
                    lExtraProducts = lExtraProducts.OrderByDescending(Function(x) x.UnitOfMeasurement).ToList()
                End If

            Case "SortOrder"
                If strSortDirection = SortDirection.Ascending Then
                    lExtraProducts = lExtraProducts.OrderBy(Function(x) x.SortOrder).ToList()
                Else
                    lExtraProducts = lExtraProducts.OrderByDescending(Function(x) x.SortOrder).ToList()
                End If

            Case "VisibilityLevel"
                If strSortDirection = SortDirection.Ascending Then
                    lExtraProducts = lExtraProducts.OrderBy(Function(x) x.VisibilityLevel).ToList()
                Else
                    lExtraProducts = lExtraProducts.OrderByDescending(Function(x) x.VisibilityLevel).ToList()
                End If

            Case "PageVisibility"
                If strSortDirection = SortDirection.Ascending Then
                    lExtraProducts = lExtraProducts.OrderBy(Function(x) x.PageVisibility).ToList()
                Else
                    lExtraProducts = lExtraProducts.OrderByDescending(Function(x) x.PageVisibility).ToList()
                End If

            Case "DeductionWidth"
                If strSortDirection = SortDirection.Ascending Then
                    lExtraProducts = lExtraProducts.OrderBy(Function(x) x.DeductionWidth).ToList()
                Else
                    lExtraProducts = lExtraProducts.OrderByDescending(Function(x) x.DeductionWidth).ToList()
                End If

            Case "AppendColourCode"
                If strSortDirection = SortDirection.Ascending Then
                    lExtraProducts = lExtraProducts.OrderBy(Function(x) x.AppendColourCode).ToList()
                Else
                    lExtraProducts = lExtraProducts.OrderByDescending(Function(x) x.AppendColourCode).ToList()
                End If

            Case "Discontinued"
                If strSortDirection = SortDirection.Ascending Then
                    lExtraProducts = lExtraProducts.OrderBy(Function(x) x.Discontinued).ToList()
                Else
                    lExtraProducts = lExtraProducts.OrderByDescending(Function(x) x.Discontinued).ToList()
                End If

        End Select

        ' Remember the last sorted colomn and its direction so we can flip the direction next time.
        gdvExtraProducts.Attributes("SortColumn") = strSortColumn
        gdvExtraProducts.Attributes("SortDirection") = strSortDirection

        gdvExtraProducts.DataSource = lExtraProducts
        gdvExtraProducts.DataBind()
    End Sub

    Private Function GetLouvreExtraPricesForSearchCriteria(intExtraProductID As Integer) As List(Of LouvreExtraPrice)

        'Expand parameters to include other filtering options.
        Dim lLouvreExtraPrices As List(Of LouvreExtraPrice) = _Service.GetAllLouvreExtraPricesByParameters(, intExtraProductID)

        If Not chkLouvreExtraPricesShowDiscontinued.Checked Then
            ' Remove discontinued
            lLouvreExtraPrices = lLouvreExtraPrices.FindAll(Function(x) Not x.Discontinued)
        End If

        Return lLouvreExtraPrices

    End Function

    Private Sub PopulateLouvreExtraPrices(intExtraProductID As Integer)
        Dim lLouvreExtraPrices As List(Of LouvreExtraPrice) = GetLouvreExtraPricesForSearchCriteria(intExtraProductID)

        gdvLouvreExtraPrices.DataSource = lLouvreExtraPrices
        gdvLouvreExtraPrices.DataBind()
    End Sub

    Private Sub gdvExtraProducts_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvExtraProducts.RowCommand

        lblNewLouvreExtraPriceStatus.Visible = False

        Select Case e.CommandName
            Case "EditExtraProductPrices"
                Dim intExtraProductID As Integer = e.CommandArgument

                ViewState("LouvreExtraPriceExtraProductIDEditing") = intExtraProductID

                Dim cExtraProduct As ExtraProductLouvres = _Service.GetExtraProductLouvresByID(intExtraProductID)

                If cExtraProduct.ExtraProductID > 0 Then
                    lblLouvreExtraPricesHeader.Text = cExtraProduct.Description & "<br /><span class='subheading'>" & cExtraProduct.ProductCode & "</span>"
                    PopulateLouvreExtraPrices(intExtraProductID)
                    mpeLouvreExtraPrices.Show()
                End If


            Case "SaveNewExtraProduct"

                Page.Validate("NewExtraProduct")

                If Page.IsValid() Then
                    Dim strDescription As String = DirectCast(gdvExtraProducts.FooterRow.FindControl("txtNewExtraProductDescription"), TextBox).Text.Trim
                    Dim strProductCode As String = DirectCast(gdvExtraProducts.FooterRow.FindControl("txtNewExtraProductProductCode"), TextBox).Text.Trim
                    Dim intUnitOfMeasurement As Integer = DirectCast(gdvExtraProducts.FooterRow.FindControl("txtNewExtraProductUnitOfMeasurement"), TextBox).Text.Trim
                    Dim intSortOrder As Integer = DirectCast(gdvExtraProducts.FooterRow.FindControl("txtNewExtraProductSortOrder"), TextBox).Text.Trim
                    Dim ccblNewExtraProductPageVisibility As CheckBoxList = DirectCast(gdvExtraProducts.FooterRow.FindControl("cblNewExtraProductPageVisibility"), CheckBoxList)
                    Dim ccblNewExtraProductVisibilityLevel As CheckBoxList = DirectCast(gdvExtraProducts.FooterRow.FindControl("cblNewExtraProductVisibilityLevel"), CheckBoxList)
                    Dim intDeductionWidth As Integer = DirectCast(gdvExtraProducts.FooterRow.FindControl("txtNewExtraProductDeductionWidth"), TextBox).Text.Trim
                    Dim boolAppendColourCode As Boolean = DirectCast(gdvExtraProducts.FooterRow.FindControl("chkAppendColourCode"), CheckBox).Checked
                    Dim supplier As String = DirectCast(gdvExtraProducts.FooterRow.FindControl("txtSupplier"), TextBox).Text.Trim
                    Dim enumPageVisibility As ExtraProductLouvresPageVisibility = ccblNewExtraProductPageVisibility.Items.Cast(Of ListItem).Where(
                                                                                    Function(x) x.Selected).Sum(Function(x) x.Value)

                    Dim enumVisibilityLevel As ExtraProductVisibilityLevel = ccblNewExtraProductVisibilityLevel.Items.Cast(Of ListItem).Where(
                                                                                    Function(x) x.Selected).Sum(Function(x) x.Value)

                    SaveNewExtraProduct(strDescription,
                                        strProductCode,
                                        intUnitOfMeasurement,
                                        intSortOrder,
                                        enumPageVisibility,
                                        enumVisibilityLevel,
                                        intDeductionWidth,
                                        boolAppendColourCode,
                                        supplier)
                End If

            Case "CancelNewExtraProduct"
                gdvExtraProducts.ShowFooter = False
                PopulateExtraProducts()
                ToggleAddExtraProductButtonsEnabled(True)

        End Select
    End Sub
    'changed by surendra 21 oct 2020
    Private Sub SaveNewExtraProduct(strDescription As String,
                                    strProductCode As String,
                                    intUnitOfMeasurement As Integer,
                                    intSortOrder As Integer,
                                    enumPageVisibility As ExtraProductLouvresPageVisibility,
                                    enumVisibilityLevel As ExtraProductVisibilityLevel,
                                    intDeductionWidth As Integer,
                                    boolAppendColourCode As Boolean,
                                    supplier As String)

        Dim cNewExtra As New ExtraProductLouvres

        With cNewExtra
            .Description = strDescription
            .Discontinued = False
            .AppendColourCode = boolAppendColourCode
            .PageVisibility = enumPageVisibility
            .VisibilityLevel = enumVisibilityLevel
            .ProductCode = strProductCode
            .SortOrder = intSortOrder
            .UnitOfMeasurement = intUnitOfMeasurement
            .DeductionWidth = intDeductionWidth
            .Supplier = supplier
        End With

        cNewExtra.ExtraProductID = _Service.AddOrUpdateExtraProductLouvres(cNewExtra)

        If cNewExtra.ExtraProductID > 0 Then

            ' Log add details
            Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                        Session("sessUserID"),
                                        LogCategory.AdminExtraProductChange,
                                        LogChangeType.Add,
                                        String.Empty,
                                        cNewExtra.LogString(),
                                        "New extra product added.",
                                        Date.Now)

            _Service.Log.AddLogEntry(cLog)

        End If

        gdvExtraProducts.ShowFooter = False
        PopulateExtraProducts()
        ToggleAddExtraProductButtonsEnabled(True)

    End Sub

    Private Sub btnAddExtraProduct_Click(sender As Object, e As EventArgs) Handles btnAddExtraProduct.Click
        gdvExtraProducts.ShowFooter = True
        PopulateExtraProducts()
        ToggleAddExtraProductButtonsEnabled(False)
    End Sub

    Private Sub ToggleAddExtraProductButtonsEnabled(boolEnable As Boolean)

        pnlExtraProductsSearchCriteria.Enabled = boolEnable

        btnAddExtraProduct.Visible = boolEnable

        ' Disable sorting links in header.
        If gdvExtraProducts.HeaderRow IsNot Nothing Then
            For Each c As TableCell In gdvExtraProducts.HeaderRow.Cells
                For Each cont As Control In c.Controls
                    If TypeOf cont Is LinkButton Then
                        DirectCast(cont, LinkButton).Enabled = boolEnable
                    End If
                Next cont
            Next c
        End If

        EnableDisableNonEditingEditButtons(gdvExtraProducts, "btnEditAddressZone", 6, Not boolEnable)
        EnableDisableNonEditingEditButtons(gdvExtraProducts, "btnEditLouvreExtraPrice", 6, Not boolEnable)
    End Sub

    Private Sub gdvLouvreExtraPrices_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvLouvreExtraPrices.RowDataBound
        Dim clblID As Label = e.Row.FindControl("lblLouvreExtraPriceID")
        Dim cddlUnitDescription As DropDownList = e.Row.FindControl("ddlLouvreExtraPriceUnitDescription")
        Dim cddlCategory As DropDownList = e.Row.FindControl("ddlLouvreExtraPriceCategoryID")

        If clblID IsNot Nothing Then
            Dim cDataSource As List(Of LouvreExtraPrice) = gdvLouvreExtraPrices.DataSource
            Dim cLouvreExtraPrice As LouvreExtraPrice = cDataSource.Find(Function(x) x.ID = clblID.Text)

            If cLouvreExtraPrice IsNot Nothing Then
                PopulateUnitDescriptionsDDL(cddlUnitDescription, cLouvreExtraPrice.UnitDescriptionID)
                PopulateCategoryDDL(cddlCategory, cLouvreExtraPrice.CategoryID)
            End If
        Else
            ' Populating footer/new item.
            PopulateUnitDescriptionsDDL(cddlUnitDescription, 0)
            PopulateCategoryDDL(cddlCategory, 0)
        End If

    End Sub

    Protected Function GetUnitDescriptionNameByID(intID As Integer) As String
        Dim cUnitDescription As UnitDescription = _Service.GetUnitDescriptions().Find(Function(x) x.ID = intID)

        If cUnitDescription IsNot Nothing Then
            Return cUnitDescription.Description
        End If

        Return String.Empty
    End Function

    Protected Function GetCategoryNameByID(intID As Integer) As String
        Dim cLouvreCategory As LouvreCategory = _Service.GetLouvreCategories().Find(Function(x) x.ID = intID)

        If cLouvreCategory IsNot Nothing Then
            Return cLouvreCategory.Name
        End If

        Return String.Empty
    End Function

    Private Sub PopulateUnitDescriptionsDDL(cddl As DropDownList, intSelectedValue As Integer)
        If cddl IsNot Nothing Then

            cddl.Items.Clear()
            cddl.Items.Add(New ListItem(String.Empty, 0))

            Dim cUnitDescriptions As List(Of UnitDescription) = _Service.GetUnitDescriptions()

            For Each d As UnitDescription In cUnitDescriptions
                cddl.Items.Add(New ListItem(d.Description, d.ID))
            Next d

            cddl.SelectedValue = intSelectedValue

        End If
    End Sub

    Private Sub PopulateCategoryDDL(cddl As DropDownList, intSelectedValue As Integer)
        If cddl IsNot Nothing Then

            cddl.Items.Clear()
            cddl.Items.Add(New ListItem(String.Empty, 0))

            Dim cCategories As List(Of LouvreCategory) = _Service.GetLouvreCategories()

            For Each c As LouvreCategory In cCategories
                cddl.Items.Add(New ListItem(c.Name, c.ID))
            Next c

            cddl.SelectedValue = intSelectedValue
        End If
    End Sub

    Private Sub btnAddLouvreExtraPrice_Click(sender As Object, e As EventArgs) Handles btnAddLouvreExtraPrice.Click

        lblNewLouvreExtraPriceStatus.Visible = False
        gdvLouvreExtraPrices.ShowFooter = True
        PopulateLouvreExtraPrices(ViewState("LouvreExtraPriceExtraProductIDEditing"))
        ToggleAddLouvreExtraPriceButtonsEnabled(False)

        PopulateUnitDescriptionsDDL(gdvLouvreExtraPrices.FooterRow.FindControl("ddlLouvreExtraPriceCategoryID"), 0)
        PopulateCategoryDDL(gdvLouvreExtraPrices.FooterRow.FindControl("ddlLouvreExtraPriceCategoryID"), 0)

        mpeLouvreExtraPrices.Show()
    End Sub

    Protected Sub valcustNewLouvreExtraPriceEffectiveDateTime_Validate(source As Object, e As ServerValidateEventArgs)
        Dim dteDate As Date

        e.IsValid = False

        If Not String.IsNullOrWhiteSpace(e.Value.Trim) Then
            e.IsValid = Date.TryParse(e.Value, dteDate)
        End If
    End Sub

    Protected Sub gdvLouvreExtraPrices_LinkSort(sender As Object, e As CommandEventArgs)
        If (e.CommandName = "Sort") Then
            SortLouvreExtraPricesGridView(e.CommandArgument)
        End If
    End Sub

    Private Sub SortLouvreExtraPricesGridView(strSortColumn As String)
        Dim strPreviousSortColumn As String = gdvLouvreExtraPrices.Attributes("SortColumn")
        Dim strSortDirection As String = gdvLouvreExtraPrices.Attributes("SortDirection")
        Dim lLouvreExtraPrices As List(Of LouvreExtraPrice) = GetLouvreExtraPricesForSearchCriteria(ViewState("LouvreExtraPriceExtraProductIDEditing"))

        If String.IsNullOrEmpty(strSortDirection) Then
            ' No previous sort direction so set it to default of ASC.
            strSortDirection = SortDirection.Ascending
        End If

        If Not String.IsNullOrEmpty(strPreviousSortColumn) Then
            ' If the colomn was the last column sorted, flip the direction.
            If strPreviousSortColumn = strSortColumn Then
                If strSortDirection = SortDirection.Ascending Then
                    strSortDirection = SortDirection.Descending
                Else
                    strSortDirection = SortDirection.Ascending
                End If
            Else
                ' New column being sorted so default to ASC.
                strSortDirection = SortDirection.Ascending
            End If
        End If

        Select Case strSortColumn

            Case "UnitPrice"
                If strSortDirection = SortDirection.Ascending Then
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderBy(Function(x) x.UnitPrice).ToList()
                Else
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderByDescending(Function(x) x.UnitPrice).ToList()
                End If

            Case "Unit"
                If strSortDirection = SortDirection.Ascending Then
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderBy(Function(x) GetUnitDescriptionNameByID(x.UnitDescriptionID)).ToList()
                Else
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderByDescending(Function(x) GetUnitDescriptionNameByID(x.UnitDescriptionID)).ToList()
                End If

            Case "PriceisPercent"
                If strSortDirection = SortDirection.Ascending Then
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderBy(Function(x) x.PriceIsPercentage).ToList()
                Else
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderByDescending(Function(x) x.PriceIsPercentage).ToList()
                End If

            Case "UnitSize"
                If strSortDirection = SortDirection.Ascending Then
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderBy(Function(x) x.UnitSize).ToList()
                Else
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderByDescending(Function(x) x.UnitSize).ToList()
                End If

            Case "MinimumCharge"
                If strSortDirection = SortDirection.Ascending Then
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderBy(Function(x) x.MinimumCharge).ToList()
                Else
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderByDescending(Function(x) x.MinimumCharge).ToList()
                End If

            Case "PricingCategory"
                If strSortDirection = SortDirection.Ascending Then
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderBy(Function(x) GetCategoryNameByID(x.CategoryID)).ToList()
                Else
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderByDescending(Function(x) GetCategoryNameByID(x.CategoryID)).ToList()
                End If

            Case "Discontinued"
                If strSortDirection = SortDirection.Ascending Then
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderBy(Function(x) x.Discontinued).ToList()
                Else
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderByDescending(Function(x) x.Discontinued).ToList()
                End If

            Case "Effective"
                If strSortDirection = SortDirection.Ascending Then
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderBy(Function(x) x.EffectiveDateTime).ToList()
                Else
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderByDescending(Function(x) x.EffectiveDateTime).ToList()
                End If

            Case "Created"
                If strSortDirection = SortDirection.Ascending Then
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderBy(Function(x) x.CreationDateTime).ToList()
                Else
                    lLouvreExtraPrices = lLouvreExtraPrices.OrderByDescending(Function(x) x.CreationDateTime).ToList()
                End If

        End Select

        ' Remember the last sorted colomn and its direction so we can flip the direction next time.
        gdvLouvreExtraPrices.Attributes("SortColumn") = strSortColumn
        gdvLouvreExtraPrices.Attributes("SortDirection") = strSortDirection

        gdvLouvreExtraPrices.DataSource = lLouvreExtraPrices
        gdvLouvreExtraPrices.DataBind()

        mpeLouvreExtraPrices.Show()
    End Sub

    Private Sub ToggleAddLouvreExtraPriceButtonsEnabled(boolEnable As Boolean)

        pnlLouvreExtraPricesSearchCriteria.Enabled = boolEnable

        btnAddLouvreExtraPrice.Visible = boolEnable
        btnCloseLouvreExtraPricePopup.Visible = boolEnable

        ' Disable sorting links in header.
        If gdvLouvreExtraPrices.HeaderRow IsNot Nothing Then
            For Each c As TableCell In gdvLouvreExtraPrices.HeaderRow.Cells
                For Each cont As Control In c.Controls
                    If TypeOf cont Is LinkButton Then
                        DirectCast(cont, LinkButton).Enabled = boolEnable
                    End If
                Next cont
            Next c
        End If

        EnableDisableNonEditingEditButtons(gdvLouvreExtraPrices, "btnEditLouvreExtraPrice", 11, Not boolEnable)
    End Sub

    Private Sub chkLouvreExtraPricesShowDiscontinued_CheckedChanged(sender As Object, e As EventArgs) Handles chkLouvreExtraPricesShowDiscontinued.CheckedChanged
        PopulateLouvreExtraPrices(ViewState("LouvreExtraPriceExtraProductIDEditing"))
        mpeLouvreExtraPrices.Show()
    End Sub

    Private Sub gdvLouvreExtraPrices_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvLouvreExtraPrices.RowCommand

        lblNewLouvreExtraPriceStatus.Visible = False

        Select Case e.CommandName
            Case "SaveNewLouvreExtraPrice"

                Page.Validate("NewLouvreExtraPrice")

                If Page.IsValid() Then
                    Dim decUnitPrice As Decimal = DirectCast(gdvLouvreExtraPrices.FooterRow.FindControl("txtNewLouvreExtraPriceUnitPrice"), TextBox).Text.Trim
                    Dim intUnitDescriptionID As Integer = DirectCast(gdvLouvreExtraPrices.FooterRow.FindControl("ddlLouvreExtraPriceUnitDescription"), DropDownList).SelectedValue
                    Dim boolPriceIsPercent As Boolean = DirectCast(gdvLouvreExtraPrices.FooterRow.FindControl("chkNewLouvreExtraPricePriceIsPercentage"), CheckBox).Checked
                    Dim intUnitSize As Integer = DirectCast(gdvLouvreExtraPrices.FooterRow.FindControl("txtNewLouvreExtraPriceUnitSize"), TextBox).Text.Trim
                    Dim decMinimumCharge As Decimal = DirectCast(gdvLouvreExtraPrices.FooterRow.FindControl("txtNewLouvreExtraPriceMinimumCharge"), TextBox).Text.Trim
                    Dim intPricingCategoryID As Integer = DirectCast(gdvLouvreExtraPrices.FooterRow.FindControl("ddlLouvreExtraPriceCategoryID"), DropDownList).SelectedValue
                    Dim decEffectiveDateTime As Date = DirectCast(gdvLouvreExtraPrices.FooterRow.FindControl("txtNewLouvreExtraPriceEffectiveDateTime"), TextBox).Text.Trim

                    ' If valid
                    SaveNewLouvreExtraPrice(ViewState("LouvreExtraPriceExtraProductIDEditing"),
                                            decUnitPrice,
                                            intUnitDescriptionID,
                                            boolPriceIsPercent,
                                            intUnitSize,
                                            decMinimumCharge,
                                            intPricingCategoryID,
                                            decEffectiveDateTime)

                    gdvLouvreExtraPrices.ShowFooter = False
                    PopulateLouvreExtraPrices(ViewState("LouvreExtraPriceExtraProductIDEditing"))
                    ToggleAddLouvreExtraPriceButtonsEnabled(True)

                End If

            Case "CancelNewLouvreExtraPrice"
                gdvLouvreExtraPrices.ShowFooter = False
                PopulateLouvreExtraPrices(ViewState("LouvreExtraPriceExtraProductIDEditing"))
                ToggleAddLouvreExtraPriceButtonsEnabled(True)

        End Select

        mpeLouvreExtraPrices.Show()

    End Sub

    Private Sub SaveNewLouvreExtraPrice(intExtraProductID As Integer,
                                        decUnitPrice As Decimal,
                                        intUnitDescriptionID As Integer,
                                        boolPriceIsPercent As Boolean,
                                        intUnitSize As Integer,
                                        decMinimumCharge As Decimal,
                                        intPricingCategoryID As Integer,
                                        decEffectiveDateTime As Date)

        If intExtraProductID > 0 Then
            Dim cNewExtraPrice As New LouvreExtraPrice

            With cNewExtraPrice
                .CategoryID = intPricingCategoryID
                .CreationDateTime = Date.Now
                .Discontinued = False
                .EffectiveDateTime = decEffectiveDateTime
                .ExtraProductID = intExtraProductID
                .MinimumCharge = decMinimumCharge
                .UnitDescriptionID = intUnitDescriptionID
                .UnitPrice = decUnitPrice
                .UnitSize = intUnitSize
                .PriceIsPercentage = boolPriceIsPercent
            End With

            cNewExtraPrice.ExtraProductID = _Service.AddOrUpdateLouvreExtraPrice(cNewExtraPrice)

            If cNewExtraPrice.ExtraProductID > 0 Then

                ' Log add details
                Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                        Session("sessUserID"),
                                        LogCategory.AdminExtraProductChange,
                                        LogChangeType.Add,
                                        String.Empty,
                                        cNewExtraPrice.LogString,
                                        "Louvre extra product pricing added.",
                                        Date.Now)

                _Service.Log.AddLogEntry(cLog)

                lblNewLouvreExtraPriceStatus.Visible = True
                lblNewLouvreExtraPriceStatus.ForeColor = Drawing.Color.Green
                lblNewLouvreExtraPriceStatus.Text = "Price successfully added."

            Else
                'Error
                lblNewLouvreExtraPriceStatus.Visible = True
                lblNewLouvreExtraPriceStatus.ForeColor = Drawing.Color.Red
                lblNewLouvreExtraPriceStatus.Text = "An error occured while attempting to save the record. Please try again."
            End If
        End If

        gdvLouvreExtraPrices.ShowFooter = False
        PopulateLouvreExtraPrices(intExtraProductID)
        ToggleAddLouvreExtraPriceButtonsEnabled(True)

        mpeLouvreExtraPrices.Show()

    End Sub

    Private Sub gdvExtraProducts_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gdvExtraProducts.RowEditing

        gdvExtraProducts.EditIndex = e.NewEditIndex

        ' Populate the table
        SortExtraProductsGridView(gdvExtraProducts.Attributes("SortColumn"), True)

        ToggleAddExtraProductButtonsEnabled(False)
    End Sub

    Private Sub gdvExtraProducts_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gdvExtraProducts.RowUpdating
        Page.Validate("NewExtraProduct")

        If Page.IsValid() Then

            Dim row As GridViewRow = gdvExtraProducts.Rows(e.RowIndex)

            Dim intID As Integer = DirectCast(row.FindControl("lblExtraProductID"), Label).Text
            Dim strDescription As String = DirectCast(row.FindControl("txtNewExtraProductDescription"), TextBox).Text.Trim
            Dim strProductCode As String = DirectCast(row.FindControl("txtNewExtraProductProductCode"), TextBox).Text.Trim
            Dim intUnitOfMeasurement As Integer = DirectCast(row.FindControl("txtNewExtraProductUnitOfMeasurement"), TextBox).Text.Trim
            Dim intSortOrder As Integer = DirectCast(row.FindControl("txtNewExtraProductSortOrder"), TextBox).Text.Trim
            Dim boolDiscontinued As Boolean = DirectCast(row.FindControl("chkDiscontinued"), CheckBox).Checked
            Dim boolAppendColourCode As Boolean = DirectCast(row.FindControl("chkAppendColourCode"), CheckBox).Checked
            Dim ccblNewExtraProductPageVisibility As CheckBoxList = DirectCast(row.FindControl("cblNewExtraProductPageVisibility"), CheckBoxList)
            Dim ccblNewExtraProductVisibilityLevel As CheckBoxList = DirectCast(row.FindControl("cblNewExtraProductVisibilityLevel"), CheckBoxList)
            Dim intDeductionWidth As Integer = DirectCast(row.FindControl("txtNewExtraProductDeductionWidth"), TextBox).Text.Trim
            'added by surendra 21 oct 2020
            Dim supplier As String = DirectCast(row.FindControl("txtSupplier"), TextBox).Text.Trim
            Dim enumPageVisibility As ExtraProductLouvresPageVisibility = ccblNewExtraProductPageVisibility.Items.Cast(Of ListItem).Where(
                                                                                    Function(x) x.Selected).Sum(Function(x) x.Value)

            Dim enumVisibilityLevel As ExtraProductVisibilityLevel = ccblNewExtraProductVisibilityLevel.Items.Cast(Of ListItem).Where(
                                                                                    Function(x) x.Selected).Sum(Function(x) x.Value)

            Dim cExtra As ExtraProductLouvres = _Service.GetExtraProductLouvresByID(intID)

            If cExtra.ExtraProductID > 0 Then

                ' For logging later
                Dim cBeforeExtra As ExtraProductLouvres = cExtra.Clone

                With cExtra
                    .Description = strDescription
                    .ProductCode = strProductCode
                    .UnitOfMeasurement = intUnitOfMeasurement
                    .SortOrder = intSortOrder
                    .Discontinued = boolDiscontinued
                    .AppendColourCode = boolAppendColourCode
                    .PageVisibility = enumPageVisibility
                    .VisibilityLevel = enumVisibilityLevel
                    .DeductionWidth = intDeductionWidth
                    'added by surendra 21 oct 2020
                    .Supplier = supplier
                End With

                If _Service.AddOrUpdateExtraProductLouvres(cExtra) > 0 Then
                    ' Log update details
                    Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                                Session("sessUserID"),
                                                LogCategory.AdminExtraProductChange,
                                                LogChangeType.Edit,
                                                cBeforeExtra.LogString(),
                                                cExtra.LogString(),
                                                "Extra edited.",
                                                Date.Now)

                    _Service.Log.AddLogEntry(cLog)
                End If

            End If

            gdvExtraProducts.EditIndex = -1

            ' Populate the table
            SortExtraProductsGridView(gdvExtraProducts.Attributes("SortColumn"), True)

            ToggleAddExtraProductButtonsEnabled(True)

        End If
    End Sub

    Private Sub gdvExtraProducts_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gdvExtraProducts.RowCancelingEdit

        gdvExtraProducts.EditIndex = -1

        ' Populate the table
        SortExtraProductsGridView(gdvExtraProducts.Attributes("SortColumn"), True)

        ToggleAddExtraProductButtonsEnabled(True)
    End Sub

    Protected Function GetPageVisibilityString(enumPageVisibility As ExtraProductLouvresPageVisibility) As String
        Return enumPageVisibility.ToString.Replace(", ", SharedConstants.STR_BREAK)
    End Function

    Protected Function GetVisibilityLevelString(enumVisibilitylevel As ExtraProductVisibilityLevel) As String
        Return enumVisibilitylevel.ToString.Replace(", ", SharedConstants.STR_BREAK)
    End Function

    Private Sub gdvExtraProducts_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvExtraProducts.RowDataBound
        Dim ccblNewExtraProductPageVisibility As CheckBoxList = DirectCast(e.Row.FindControl("cblNewExtraProductPageVisibility"), CheckBoxList)
        Dim ccblNewExtraProductVisibilityLevel As CheckBoxList = DirectCast(e.Row.FindControl("cblNewExtraProductVisibilityLevel"), CheckBoxList)
        Dim cExtra As ExtraProductLouvres = DirectCast(e.Row.DataItem, ExtraProductLouvres)

        If ccblNewExtraProductPageVisibility IsNot Nothing Then
            With ccblNewExtraProductPageVisibility
                Dim cListItem As ListItem

                .Items.Clear()

                cListItem = New ListItem(ExtraProductLouvresPageVisibility.OZRoll_AcceptOrder.ToString, ExtraProductLouvresPageVisibility.OZRoll_AcceptOrder)
                cListItem.Attributes.Add("class", "checkbox")
                .Items.Add(cListItem)

                ' Footer will be nothing so cant set it.
                If cExtra IsNot Nothing Then
                    cListItem.Selected = cExtra.IsVisibleToPage(ExtraProductLouvresPageVisibility.OZRoll_AcceptOrder)
                End If

                cListItem = New ListItem(ExtraProductLouvresPageVisibility.OZRoll_LouvreJobDetails.ToString, ExtraProductLouvresPageVisibility.OZRoll_LouvreJobDetails)
                .Items.Add(cListItem)
                cListItem.Attributes.Add("class", "checkbox")

                If cExtra IsNot Nothing Then
                    cListItem.Selected = cExtra.IsVisibleToPage(ExtraProductLouvresPageVisibility.OZRoll_LouvreJobDetails)
                End If

                cListItem = New ListItem(ExtraProductLouvresPageVisibility.Portal_AddLouvreOrder.ToString, ExtraProductLouvresPageVisibility.Portal_AddLouvreOrder)
                .Items.Add(cListItem)
                cListItem.Attributes.Add("class", "checkbox")

                If cExtra IsNot Nothing Then
                    cListItem.Selected = cExtra.IsVisibleToPage(ExtraProductLouvresPageVisibility.Portal_AddLouvreOrder)
                End If

                cListItem = New ListItem(ExtraProductLouvresPageVisibility.Portal_LouvreEstimate.ToString, ExtraProductLouvresPageVisibility.Portal_LouvreEstimate)
                .Items.Add(cListItem)
                cListItem.Attributes.Add("class", "checkbox")

                If cExtra IsNot Nothing Then
                    cListItem.Selected = cExtra.IsVisibleToPage(ExtraProductLouvresPageVisibility.Portal_LouvreEstimate)
                End If

            End With
        End If

        If ccblNewExtraProductVisibilityLevel IsNot Nothing Then
            With ccblNewExtraProductVisibilityLevel
                Dim cListItem As ListItem

                .Items.Clear()

                cListItem = New ListItem(ExtraProductVisibilityLevel.ProductionSchedule.ToString, ExtraProductVisibilityLevel.ProductionSchedule)
                cListItem.Attributes.Add("class", "checkbox")
                .Items.Add(cListItem)

                ' Footer will be nothing so cant set it.
                If cExtra IsNot Nothing Then
                    cListItem.Selected = cExtra.VisibilityLevel.HasFlag(ExtraProductVisibilityLevel.ProductionSchedule)
                End If

                cListItem = New ListItem(ExtraProductVisibilityLevel.Details.ToString, ExtraProductVisibilityLevel.Details)
                .Items.Add(cListItem)
                cListItem.Attributes.Add("class", "checkbox")

                If cExtra IsNot Nothing Then
                    cListItem.Selected = cExtra.VisibilityLevel.HasFlag(ExtraProductVisibilityLevel.Details)
                End If

            End With
        End If

    End Sub

    Private Sub gdvLouvreExtraPrices_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gdvLouvreExtraPrices.RowCancelingEdit

        gdvLouvreExtraPrices.EditIndex = -1

        ' Populate the table
        gdvLouvreExtraPrices.DataSource = GetLouvreExtraPricesForSearchCriteria(ViewState("LouvreExtraPriceExtraProductIDEditing"))
        gdvLouvreExtraPrices.DataBind()

        ToggleAddLouvreExtraPriceButtonsEnabled(True)
    End Sub

    Private Sub gdvLouvreExtraPrices_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gdvLouvreExtraPrices.RowEditing

        gdvLouvreExtraPrices.EditIndex = e.NewEditIndex

        ' Populate the table
        gdvLouvreExtraPrices.DataSource = GetLouvreExtraPricesForSearchCriteria(ViewState("LouvreExtraPriceExtraProductIDEditing"))
        gdvLouvreExtraPrices.DataBind()

        ToggleAddLouvreExtraPriceButtonsEnabled(False)
    End Sub

    Private Sub gdvLouvreExtraPrices_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gdvLouvreExtraPrices.RowUpdating

        Page.Validate("NewLouvreExtraPrice")

        If Page.IsValid() Then

            Dim row As GridViewRow = gdvLouvreExtraPrices.Rows(e.RowIndex)

            Dim intID As Integer = DirectCast(row.FindControl("lblLouvreExtraPriceID"), Label).Text
            Dim decUnitPrice As Decimal = DirectCast(row.FindControl("txtNewLouvreExtraPriceUnitPrice"), TextBox).Text.Trim
            Dim intUnitDescriptionID As String = DirectCast(row.FindControl("ddlLouvreExtraPriceUnitDescription"), DropDownList).SelectedValue
            Dim boolPriceIsPercent As Integer = DirectCast(row.FindControl("chkNewLouvreExtraPricePriceIsPercentage"), CheckBox).Checked
            Dim intUnitSize As Integer = DirectCast(row.FindControl("txtNewLouvreExtraPriceUnitSize"), TextBox).Text.Trim
            Dim decMinCharge As Decimal = DirectCast(row.FindControl("txtNewLouvreExtraPriceMinimumCharge"), TextBox).Text.Trim
            Dim intCategoryID As Integer = DirectCast(row.FindControl("ddlLouvreExtraPriceCategoryID"), DropDownList).SelectedValue
            Dim boolDiscontinued As Boolean = DirectCast(row.FindControl("chkDiscontinued"), CheckBox).Checked
            Dim dteEffectiveDateTime As Date = DirectCast(row.FindControl("txtNewLouvreExtraPriceEffectiveDateTime"), TextBox).Text.Trim

            Dim cExtraPrice As LouvreExtraPrice = _Service.GetAllLouvreExtraPricesByParameters().Find(Function(x) x.ID = intID)

            If cExtraPrice.ExtraProductID > 0 Then

                ' For logging later
                Dim cBeforeExtraPrice As LouvreExtraPrice = cExtraPrice.Clone

                With cExtraPrice
                    .CategoryID = intCategoryID
                    .Discontinued = boolDiscontinued
                    .EffectiveDateTime = dteEffectiveDateTime
                    .ExtraProductID = ViewState("LouvreExtraPriceExtraProductIDEditing")
                    .MinimumCharge = decMinCharge
                    .PriceIsPercentage = boolPriceIsPercent
                    .UnitDescriptionID = intUnitDescriptionID
                    .UnitPrice = decUnitPrice
                    .UnitSize = intUnitSize
                End With

                If _Service.AddOrUpdateLouvreExtraPrice(cExtraPrice) > 0 Then
                    ' Log update details
                    Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                                Session("sessUserID"),
                                                LogCategory.AdminExtraProductChange,
                                                LogChangeType.Edit,
                                                cBeforeExtraPrice.LogString(),
                                                cExtraPrice.LogString(),
                                                "Louvre Extra Price Edited.",
                                                Date.Now)

                    _Service.Log.AddLogEntry(cLog)
                End If

            End If

            gdvLouvreExtraPrices.EditIndex = -1

            ' Populate the table
            gdvLouvreExtraPrices.DataSource = GetLouvreExtraPricesForSearchCriteria(ViewState("LouvreExtraPriceExtraProductIDEditing"))
            gdvLouvreExtraPrices.DataBind()

            ToggleAddLouvreExtraPriceButtonsEnabled(True)

        End If
    End Sub

#End Region

#Region "Louvre Prices"

    Private Sub PopulateLouvrePrices()

        lblLouvrePriceFileUploadStatus.Text = String.Empty
        HideAllPanels()
        pnlLouvrePrices.Visible = True

        Dim lLouvrePrices As List(Of LouvrePrice) = GetLouvrePricesForSearchCriteria()

        gdvLouvrePrice.DataSource = lLouvrePrices
        gdvLouvrePrice.DataBind()

    End Sub

    Private Sub PopulateLouvrePriceDDLs()

        ddlLouvrePriceCategory.Items.Clear()
        ddlLouvrePriceCategory.Items.Add(New ListItem(String.Empty, 0))

        Dim lLouvreCategories As List(Of LouvreCategory) = _Service.GetLouvreCategories()

        For Each c As LouvreCategory In lLouvreCategories
            ddlLouvrePriceCategory.Items.Add(New ListItem(c.Name, c.ID))
        Next c

        ddlLouvrePriceStyle.Items.Clear()
        ddlLouvrePriceStyle.Items.Add(New ListItem(String.Empty, 0))
        ddlLouvrePriceStyle.Items.Add(New ListItem("DLi", 2))
        ddlLouvrePriceStyle.Items.Add(New ListItem("CL", 3))
        ddlLouvrePriceStyle.Items.Add(New ListItem("Privacy Screen", 4))

        ddlLouvrePriceCoatingType.Items.Clear()
        ddlLouvrePriceCoatingType.Items.Add(New ListItem(String.Empty, 0))
        ddlLouvrePriceCoatingType.Items.Add(New ListItem("Standard Powder Coat", CoatingType.StandardPowderCoat))
        ddlLouvrePriceCoatingType.Items.Add(New ListItem("Premium Powder Coat", CoatingType.PremiumPowderCoat))
        ddlLouvrePriceCoatingType.Items.Add(New ListItem("Prestige Powder Coat", CoatingType.PrestigePowderCoat))

        ddlLouvrePriceType.Items.Clear()
        ddlLouvrePriceType.Items.Add(New ListItem(String.Empty, 0))

        Dim lLouvrePriceTypes As List(Of LouvreType) = _Service.GetLouvreTypes().FindAll(Function(x) Not x.Discontinued)

        For Each p As LouvreType In lLouvrePriceTypes
            ddlLouvrePriceType.Items.Add(New ListItem(p.Name, p.ID))
        Next p


    End Sub

    Private Function GetLouvrePricesForSearchCriteria() As List(Of LouvrePrice)
        Dim lLouvrePrices As List(Of LouvrePrice) = Nothing

        Page.Validate("LouvrePrice")

        If Page.IsValid() Then
            Dim intCategoryID As Integer? = Nothing
            Dim intLouvreStyleID As Integer? = Nothing
            Dim intCoatingTypeID As Integer? = Nothing
            Dim intLouvreTypeID As Integer? = Nothing
            Dim dteEffectiveDate As Date? = Nothing

            If ddlLouvrePriceCategory.SelectedValue > 0 Then
                intCategoryID = ddlLouvrePriceCategory.SelectedValue
            End If

            If ddlLouvrePriceStyle.SelectedValue > 0 Then
                intLouvreStyleID = ddlLouvrePriceStyle.SelectedValue
            End If

            If ddlLouvrePriceCoatingType.SelectedValue > 0 Then
                intCoatingTypeID = ddlLouvrePriceCoatingType.SelectedValue
            End If

            If ddlLouvrePriceType.SelectedValue > 0 Then
                intLouvreTypeID = ddlLouvrePriceType.SelectedValue
            End If

            If IsDate(txtLouvrePriceDate.Text.Trim) Then
                dteEffectiveDate = txtLouvrePriceDate.Text.Trim
            End If

            'Expand parameters to include other filtering options.
            lLouvrePrices = _Service.GetLouvrePricesByParameters(intCategoryID,
                                                                 intLouvreStyleID,
                                                                 intLouvreTypeID,
                                                                 intCoatingTypeID,
                                                                 dteEffectiveDate)

        End If

        Return lLouvrePrices

    End Function

    Private Sub afuLouvrePriceAjaxUploader_UploadComplete(sender As Object, e As AjaxFileUploadEventArgs) Handles afuLouvrePriceAjaxUploader.UploadComplete
        Session("LouvrePricesFile") = e.GetContents
    End Sub

    Private Sub btnLouvrePricesUploaded_Click(sender As Object, e As EventArgs) Handles btnLouvrePricesUploaded.Click
        Dim cBytes As Byte() = Session("LouvrePricesFile")
        Dim stream As Stream = New MemoryStream(cBytes)

        ReadLouvrePricesFromFile(stream)

        Session("StockDeductionsFile") = Nothing
    End Sub

    Private Sub ReadLouvrePricesFromFile(cFile As Stream)
        Dim strErrorMsg As String = String.Empty
        Dim bolContinue As Boolean = True
        Dim dbConn As New DBConnection
        Dim cnn As SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim trans As SqlTransaction = Nothing

        dbConn = Nothing

        Try
            cnn.Open()
            trans = cnn.BeginTransaction

            Dim cLouvrePricesReader As New LouvrePricesReader(Session("sessUserID"))
            Dim lLouvrePrices As List(Of LouvrePrice) = Nothing

            ' Read values from xls
            If cLouvrePricesReader.ReadExcel(cFile, lLouvrePrices, strErrorMsg) Then
                For Each p As LouvrePrice In lLouvrePrices
                    If bolContinue Then
                        ' Write to local table
                        bolContinue = (_Service.AddOrUpdateLouvrePrice(p, cnn, trans) > 0)
                    Else
                        Exit For
                    End If
                Next p
            Else
                bolContinue = False
            End If

            If bolContinue Then
                trans.Commit()
            Else
                EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - Transaction Rolled Back" & Environment.NewLine & SharedFunctions.GetPageInfo(form1, Session))
                trans.Rollback()

                If strErrorMsg.Length = 0 Then
                    ' Generic error.
                    strErrorMsg = "An error occurred."
                End If
            End If

        Catch ex As Exception

            If Not trans Is Nothing Then
                trans.Rollback()
            End If

            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If

            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message & Environment.NewLine & SharedFunctions.GetPageInfo(form1, Session))
            bolContinue = False

            If ex.Message.Length > 0 Then
                strErrorMsg = ex.Message
            ElseIf strErrorMsg.Length = 0 Then
                ' Generic error.
                strErrorMsg = "An error occurred."
            End If

        Finally

            trans.Dispose()
            trans = Nothing

            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If

            cnn.Dispose()
            cnn = Nothing

            If strErrorMsg.Length > 0 Then
                ' Critical error occurred. Print msg.
                lblLouvrePriceFileUploadStatus.Text = strErrorMsg
                lblLouvrePriceFileUploadStatus.ForeColor = Drawing.Color.Red
            Else
                ' All good
                lblLouvrePriceFileUploadStatus.Text = "Success"
                lblLouvrePriceFileUploadStatus.ForeColor = Drawing.Color.Green
            End If

            cFile.Close()
        End Try

        gdvLouvrePrice.DataSource = Nothing
        gdvLouvrePrice.DataBind()

    End Sub

    Protected Sub valcustDate_Validate(source As Object, e As ServerValidateEventArgs) Handles valcustDate.ServerValidate
        Dim dteDate As Date

        e.IsValid = False

        If Not String.IsNullOrWhiteSpace(e.Value.Trim) Then
            e.IsValid = Date.TryParse(e.Value, dteDate)
        End If
    End Sub

    Private Sub btnLouvrePriceSubmitSearch_Click(sender As Object, e As EventArgs) Handles btnLouvrePriceSubmitSearch.Click
        PopulateLouvrePrices()
    End Sub

    Protected Function GetPriceCategoryStringForID(intID As Integer) As String

        If _LouvreCategories Is Nothing Then
            _LouvreCategories = _Service.GetLouvreCategories()
        End If

        Dim cCategory As LouvreCategory = _LouvreCategories.Find(Function(x) x.ID = intID)

        If cCategory IsNot Nothing Then
            Return cCategory.Name
        Else
            Return intID
        End If

    End Function

    Protected Function GetLouvreStyleStringForID(intID As Integer) As String

        If _LouvreStyles Is Nothing Then
            _LouvreStyles = _Service.GetLouvreStyles()
        End If

        Dim cStyle As LouvreStyle = _LouvreStyles.Find(Function(x) x.ID = intID)

        If cStyle IsNot Nothing Then
            Return cStyle.Name
        Else
            Return intID
        End If

    End Function

    Protected Function GetLouvreTypeStringForID(intID As Integer) As String

        If _LouvreTypes Is Nothing Then
            _LouvreTypes = _Service.GetLouvreTypes()
        End If

        Dim cLouvreType As LouvreType = _LouvreTypes.Find(Function(x) x.ID = intID)

        If cLouvreType IsNot Nothing Then
            Return cLouvreType.Name
        Else
            Return intID
        End If

    End Function

    Protected Function GetCoatingTypeStringForID(intID As Integer) As String

        Select Case intID
            Case CoatingType.StandardPowderCoat
                Return "Standard Powder Coat"
            Case CoatingType.PremiumPowderCoat
                Return "Premium Powder Coat"
            Case CoatingType.PrestigePowderCoat
                Return "Prestige Powder Coat"
            Case Else
                Return intID
        End Select

    End Function

#End Region

#Region "Colours"

    Protected Function GetProductTypeStringForID(intID As Integer) As String

        Select Case intID
            Case ProductType.Plantation
                Return "Plantation"
            Case ProductType.Louvres
                Return "Louvres"
            Case Else
                Return intID
        End Select

    End Function

    Private Function GetColoursForSearchCriteria() As List(Of Colour)
        Dim lColours As List(Of Colour) = Nothing

        lColours = _Service.getColours().FindAll(Function(x) x.ProductTypeID = ProductType.Louvres)

        If Not chkColoursShowDiscontinued.Checked Then
            ' Remove discontinued logins.
            lColours = lColours.FindAll(Function(x) Not x.Discontinued)

            If lColours Is Nothing Then
                lColours = New List(Of Colour)
            End If
        End If

        Return lColours
    End Function

    Private Sub PopulateColours()

        HideAllPanels()
        pnlColours.Visible = True

        SortColoursGridView(gdvColours.Attributes("SortColumn"), True)
    End Sub

    ''' <summary>
    ''' Disables/Enables all edit buttons outside of the editing row.
    ''' </summary>
    ''' <param name="gGridView">The <see cref="GridView"/> to use.</param>
    ''' <param name="strButtonID">The button control ID string.</param>
    ''' <param name="boolHide">Disables/Enables the buttons.</param>
    Private Sub EnableDisableNonEditingEditButtons(gGridView As GridView,
                                          strButtonID As String,
                                          intGridViewButtonColumnIdx As Integer,
                                          boolHide As Boolean)

        For Each r As GridViewRow In gGridView.Rows
            If r.RowIndex <> gGridView.SelectedIndex Then
                Dim bEditButton As Button = DirectCast(r.Cells(intGridViewButtonColumnIdx).FindControl(strButtonID), Button)

                If Not bEditButton Is Nothing Then
                    If boolHide Then
                        bEditButton.Enabled = False
                    Else
                        bEditButton.Enabled = True
                    End If
                End If
            End If
        Next r
    End Sub

    Private Sub gdvColours_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gdvColours.RowEditing

        gdvColours.EditIndex = e.NewEditIndex

        ' Populate the table
        PopulateColours()

        ToggleAddColourButtonsEnabled(False)
    End Sub

    Private Sub gdvColours_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gdvColours.RowUpdating

        Page.Validate("NewColour")

        If Page.IsValid() Then
            Dim cRow As GridViewRow = gdvColours.Rows(e.RowIndex)
            Dim intID As Integer = DirectCast(cRow.FindControl("lblColourID"), Label).Text
            Dim intCoatingTypeID As Integer = DirectCast(cRow.FindControl("ddlCoatingType"), DropDownList).SelectedValue
            Dim strColourCode As String = DirectCast(cRow.FindControl("txtColourCode"), TextBox).Text.Trim
            Dim strName As String = DirectCast(cRow.FindControl("txtColourName"), TextBox).Text.Trim
            Dim intSortOrder As Integer = DirectCast(cRow.FindControl("txtSortOrder"), TextBox).Text.Trim
            Dim bolDiscontinued As Boolean = DirectCast(cRow.FindControl("chkDiscontinued"), CheckBox).Checked

            Dim cColour As Colour = _Service.getColours.Find(Function(x) x.ID = intID)
            Dim cBeforeColour As Colour = cColour.Clone

            With cColour
                .ID = intID
                .Discontinued = bolDiscontinued
                .CoatingTypeID = intCoatingTypeID
                .ColourCode = strColourCode
                .Name = strName
                .ProductTypeID = ProductType.Louvres
                .SortOrder = intSortOrder
            End With

            cColour.ID = _Service.AddOrUpdateColour(cColour)

            If cColour.ID > 0 Then

                ' Log add details
                Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                     Session("sessUserID"),
                                     LogCategory.AdminColourChange,
                                     LogChangeType.Edit,
                                     cBeforeColour.LogString(),
                                     cColour.LogString(),
                                     "Colour edited.",
                                     Date.Now)

                _Service.Log.AddLogEntry(cLog)

            End If

            gdvColours.EditIndex = -1

            ' Populate the table
            PopulateColours()

            ToggleAddColourButtonsEnabled(True)

        End If

    End Sub

    Private Sub gdvColours_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gdvColours.RowCancelingEdit
        gdvColours.EditIndex = -1

        ' Populate the table
        PopulateColours()

        ToggleAddColourButtonsEnabled(True)
    End Sub

    Private Sub gdvColours_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvColours.RowCommand

        Select Case e.CommandName
            Case "SaveNewColour"

                Page.Validate("NewColour")

                If Page.IsValid() Then
                    Dim intCoatingTypeID As Integer = DirectCast(gdvColours.FooterRow.FindControl("ddlCoatingType"), DropDownList).SelectedValue
                    Dim strColourCode As String = DirectCast(gdvColours.FooterRow.FindControl("txtColourCode"), TextBox).Text.Trim
                    Dim strName As String = DirectCast(gdvColours.FooterRow.FindControl("txtColourName"), TextBox).Text.Trim
                    Dim intSortOrder As Integer = DirectCast(gdvColours.FooterRow.FindControl("txtSortOrder"), TextBox).Text.Trim

                    SaveNewColour(intCoatingTypeID,
                                  strColourCode,
                                  strName,
                                  intSortOrder)
                End If

            Case "CancelNewColour"
                gdvColours.ShowFooter = False
                PopulateColours()
                ToggleAddColourButtonsEnabled(True)

        End Select
    End Sub

    Private Sub SaveNewColour(intCoatingTypeID As Integer,
                              strColourCode As String,
                              strName As String,
                              intSortOrder As Integer)

        Dim cNewColour As New Colour

        With cNewColour
            .CoatingTypeID = intCoatingTypeID
            .ColourCode = strColourCode
            .Name = strName
            .ProductTypeID = ProductType.Louvres
            .SortOrder = intSortOrder
        End With

        cNewColour.ID = _Service.AddOrUpdateColour(cNewColour)

        If cNewColour.ID > 0 Then

            ' Log add details
            Dim cLog As New LogEntry(SiteID.LouvreOzroll,
                                     Session("sessUserID"),
                                     LogCategory.AdminColourChange,
                                     LogChangeType.Add,
                                     String.Empty,
                                     cNewColour.LogString(),
                                     "New colour added.",
                                     Date.Now)

            _Service.Log.AddLogEntry(cLog)

        End If

        gdvColours.ShowFooter = False
        PopulateColours()
        ToggleAddColourButtonsEnabled(False)

    End Sub

    Private Sub ToggleAddColourButtonsEnabled(boolEnable As Boolean)

        btnAddColour.Visible = boolEnable

        ' Disable sorting links in header.
        If gdvColours.HeaderRow IsNot Nothing Then
            For Each c As TableCell In gdvColours.HeaderRow.Cells
                For Each cont As Control In c.Controls
                    If TypeOf cont Is LinkButton Then
                        DirectCast(cont, LinkButton).Enabled = boolEnable
                    End If
                Next cont
            Next c
        End If

        EnableDisableNonEditingEditButtons(gdvColours, "btnEditColour", 6, Not boolEnable)
    End Sub

    Private Sub btnAddColour_Click(sender As Object, e As EventArgs) Handles btnAddColour.Click

        gdvColours.ShowFooter = True
        PopulateColours()
        ToggleAddColourButtonsEnabled(False)

        PopulateCoulourCoatingTypeDDL(gdvColours.FooterRow.FindControl("ddlCoatingType"), 0)

    End Sub

    Private Sub PopulateCoulourCoatingTypeDDL(cddl As DropDownList, intSelectedValue As Integer)
        If cddl IsNot Nothing Then

            cddl.Items.Clear()
            cddl.Items.Add(New ListItem(String.Empty, 0))
            cddl.Items.Add(New ListItem("Standard Powder Coat", CoatingType.StandardPowderCoat))
            cddl.Items.Add(New ListItem("Premium Powder Coat", CoatingType.PremiumPowderCoat))
            cddl.Items.Add(New ListItem("Prestige Powder Coat", CoatingType.PrestigePowderCoat))

            If cddl.Items.FindByValue(intSelectedValue) IsNot Nothing Then
                cddl.SelectedValue = intSelectedValue
            End If

        End If
    End Sub

    Private Sub gdvColours_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvColours.RowDataBound
        Dim cddlCoatingType As DropDownList = DirectCast(e.Row.FindControl("ddlCoatingType"), DropDownList)
        Dim cColour As Colour = DirectCast(e.Row.DataItem, Colour)

        If cColour IsNot Nothing Then
            PopulateCoulourCoatingTypeDDL(cddlCoatingType, cColour.CoatingTypeID)
        End If
    End Sub

    Private Sub chkColoursShowDiscontinued_CheckedChanged(sender As Object, e As EventArgs) Handles chkColoursShowDiscontinued.CheckedChanged
        PopulateColours()
    End Sub

    Protected Sub gdvColours_LinkSort(sender As Object, e As CommandEventArgs)
        If (e.CommandName = "Sort") Then
            SortColoursGridView(e.CommandArgument)
        End If
    End Sub

    Private Sub gdvColours_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gdvColours.Sorting
        SortColoursGridView(e.SortExpression)
    End Sub

    Private Sub SortColoursGridView(strSortColumn As String, Optional boolMaintainSortDirection As Boolean = False)
        Dim strPreviousSortColumn As String = gdvColours.Attributes("SortColumn")
        Dim strSortDirection As String = gdvColours.Attributes("SortDirection")
        Dim lColours As List(Of Colour) = GetColoursForSearchCriteria()

        If String.IsNullOrEmpty(strSortDirection) Then
            ' No previous sort direction so set it to default of ASC.
            strSortDirection = SortDirection.Ascending
        End If

        If Not boolMaintainSortDirection Then
            If Not String.IsNullOrEmpty(strPreviousSortColumn) Then
                ' If the colomn was the last column sorted, flip the direction.
                If strPreviousSortColumn = strSortColumn Then
                    If strSortDirection = SortDirection.Ascending Then
                        strSortDirection = SortDirection.Descending
                    Else
                        strSortDirection = SortDirection.Ascending
                    End If
                Else
                    ' New column being sorted so default to ASC.
                    strSortDirection = SortDirection.Ascending
                End If
            End If
        End If

        Select Case strSortColumn
            Case "Name"
                If strSortDirection = SortDirection.Ascending Then
                    lColours = lColours.OrderBy(Function(x) x.Name).ToList()
                Else
                    lColours = lColours.OrderByDescending(Function(x) x.Name).ToList()
                End If

            Case "CoatingType"
                If strSortDirection = SortDirection.Ascending Then
                    lColours = lColours.OrderBy(Function(x) GetCoatingTypeStringForID(x.CoatingTypeID)).ToList()
                Else
                    lColours = lColours.OrderByDescending(Function(x) GetCoatingTypeStringForID(x.CoatingTypeID)).ToList()
                End If

            Case "Discontinued"
                If strSortDirection = SortDirection.Ascending Then
                    lColours = lColours.OrderBy(Function(x) x.Discontinued).ToList()
                Else
                    lColours = lColours.OrderByDescending(Function(x) x.Discontinued).ToList()
                End If

            Case "ColourCode"
                If strSortDirection = SortDirection.Ascending Then
                    lColours = lColours.OrderBy(Function(x) x.ColourCode).ToList()
                Else
                    lColours = lColours.OrderByDescending(Function(x) x.ColourCode).ToList()
                End If

            Case "SortOrder"
                If strSortDirection = SortDirection.Ascending Then
                    lColours = lColours.OrderBy(Function(x) x.SortOrder).ToList()
                Else
                    lColours = lColours.OrderByDescending(Function(x) x.SortOrder).ToList()
                End If

        End Select

        ' Remember the last sorted colomn and its direction so we can flip the direction next time.
        gdvColours.Attributes("SortColumn") = strSortColumn
        gdvColours.Attributes("SortDirection") = strSortDirection

        gdvColours.DataSource = lColours
        gdvColours.DataBind()
    End Sub

#End Region

#Region "Log"

    Private Sub PopulateLog()

        HideAllPanels()
        pnlLog.Visible = True

        Dim lLogEntries As List(Of LogEntry) = GetLogEntryListForSearchCriteria()

        gdvLog.DataSource = lLogEntries
        gdvLog.DataBind()

    End Sub

    Private Function GetLogEntryListForSearchCriteria() As List(Of LogEntry)
        Dim lLogEntries As List(Of LogEntry) = _Service.Log.GetLogEntriesByParameters()

        Return lLogEntries
    End Function

    Private Sub gdvLog_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gdvLog.Sorting
        SortLogGridView(e.SortExpression)
    End Sub

    Protected Sub gdvLog_LinkSort(sender As Object, e As CommandEventArgs)
        If (e.CommandName = "Sort") Then
            SortLogGridView(e.CommandArgument)
        End If
    End Sub

    Private Sub SortLogGridView(strSortColumn As String)
        Dim strPreviousSortColumn As String = gdvLog.Attributes("SortColumn")
        Dim strSortDirection As String = gdvLog.Attributes("SortDirection")
        Dim lLog As List(Of LogEntry) = GetLogEntryListForSearchCriteria()

        If String.IsNullOrEmpty(strSortDirection) Then
            ' No previous sort direction so set it to default of ASC.
            strSortDirection = SortDirection.Ascending
        End If

        If Not String.IsNullOrEmpty(strPreviousSortColumn) Then
            ' If the colomn was the last column sorted, flip the direction.
            If strPreviousSortColumn = strSortColumn Then
                If strSortDirection = SortDirection.Ascending Then
                    strSortDirection = SortDirection.Descending
                Else
                    strSortDirection = SortDirection.Ascending
                End If
            Else
                ' New column being sorted so default to ASC.
                strSortDirection = SortDirection.Ascending
            End If
        End If

        Select Case strSortColumn
            Case "SiteID"
                If strSortDirection = SortDirection.Ascending Then
                    lLog = lLog.OrderBy(Function(x) x.SiteID).ToList()
                Else
                    lLog = lLog.OrderByDescending(Function(x) x.SiteID).ToList()
                End If

            Case "User"
                If strSortDirection = SortDirection.Ascending Then
                    lLog = lLog.OrderBy(Function(x) GetLoginNameByLoginID(x.UserID, x.SiteID)).ToList()
                Else
                    lLog = lLog.OrderByDescending(Function(x) GetLoginNameByLoginID(x.UserID, x.SiteID)).ToList()
                End If

            Case "Category"
                If strSortDirection = SortDirection.Ascending Then
                    lLog = lLog.OrderBy(Function(x) x.CategoryID).ToList()
                Else
                    lLog = lLog.OrderByDescending(Function(x) x.CategoryID).ToList()
                End If

            Case "ChangeType"
                If strSortDirection = SortDirection.Ascending Then
                    lLog = lLog.OrderBy(Function(x) x.ChangeTypeID).ToList()
                Else
                    lLog = lLog.OrderByDescending(Function(x) x.ChangeTypeID).ToList()
                End If

            Case "BeforeChange"
                If strSortDirection = SortDirection.Ascending Then
                    lLog = lLog.OrderBy(Function(x) x.BeforeChange).ToList()
                Else
                    lLog = lLog.OrderByDescending(Function(x) x.BeforeChange).ToList()
                End If

            Case "AfterChange"
                If strSortDirection = SortDirection.Ascending Then
                    lLog = lLog.OrderBy(Function(x) x.AfterChange).ToList()
                Else
                    lLog = lLog.OrderByDescending(Function(x) x.AfterChange).ToList()
                End If

            Case "AdditionalInfo"
                If strSortDirection = SortDirection.Ascending Then
                    lLog = lLog.OrderBy(Function(x) x.AdditionalInfo).ToList()
                Else
                    lLog = lLog.OrderByDescending(Function(x) x.AdditionalInfo).ToList()
                End If

            Case "CreationDateTime"
                If strSortDirection = SortDirection.Ascending Then
                    lLog = lLog.OrderBy(Function(x) x.CreationDateTime).ToList()
                Else
                    lLog = lLog.OrderByDescending(Function(x) x.CreationDateTime).ToList()
                End If

        End Select

        ' Remember the last sorted colomn and its direction so we can flip the direction next time.
        gdvLog.Attributes("SortColumn") = strSortColumn
        gdvLog.Attributes("SortDirection") = strSortDirection

        gdvLog.DataSource = lLog
        gdvLog.DataBind()
    End Sub

    Protected Function GetLoginNameByLoginID(intID As Integer, enumSiteID As SiteID) As String

        Select Case enumSiteID
            Case SiteID.LouvreOzroll
                Dim cUser As User = _Service.GetUsers().Find(Function(x) x.ID = intID)

                If cUser IsNot Nothing AndAlso cUser.ID > 0 Then
                    Return cUser.UserName
                End If

            Case SiteID.LouvrePortal
                Dim cLogin As LoginDetails = _Service.GetLoginDetailsByID(intID)

                If cLogin IsNot Nothing AndAlso cLogin.LoginID > 0 Then
                    Return cLogin.LoginName
                End If

        End Select

        Return String.Empty
    End Function

#End Region

#Region "Permissions"

    Private Sub PopulatePermissionsFilterUI()
        ' Populate filter user ddl.
        Dim lUsers As List(Of User) = _Service.GetUsers()

        ddlPermissionsSearchCriteriaUser.Items.Clear()

        For Each u As User In lUsers
            Dim cItem As New ListItem(u.UserName & IIf(u.Discontinued, " [discontinued]", ""), u.ID)

            ddlPermissionsSearchCriteriaUser.Items.Add(cItem)
        Next u
    End Sub

    Private Sub PopulatePermissions()

        HideAllPanels()
        pnlPermissions.Visible = True

        ' Get all permissions
        Dim lPermissions As List(Of Permission) = _Service.Permissions.GetPermissions().OrderBy(Function(x) x.Category).ToList()

        ' Get all user permissions
        _UserPermissions = GetUserPermissionsForSearchCriteria()

        ' Check on all cross referenced permissions

        gdvPermissions.DataSource = lPermissions
        gdvPermissions.DataBind()

    End Sub

    Protected Function UserHasPermissionChecked(intPermissionID As Integer) As Boolean
        Return _UserPermissions.HasPermission(intPermissionID)
    End Function

    Private Function GetUserPermissionsForSearchCriteria() As UserPermissionsContainer
        Dim cUserPermissions As New UserPermissionsContainer

        If IsNumeric(ddlPermissionsSearchCriteriaUser.SelectedValue) Then
            cUserPermissions = _Service.Permissions.UserHasPermissions(ddlPermissionsSearchCriteriaUser.SelectedValue)
        End If

        Return cUserPermissions
    End Function

    Private Sub ddlPermissionsSearchCriteriaUser_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPermissionsSearchCriteriaUser.SelectedIndexChanged
        PopulatePermissions()
    End Sub

    Private Sub btnSavePermissions_Click(sender As Object, e As EventArgs) Handles btnSavePermissions.Click
        Dim cExistingUserPermissions As UserPermissionsContainer = GetUserPermissionsForSearchCriteria()
        Dim cNewUserPermission As UserPermissions
        Dim cLog As LogEntry
        Dim dbConn As New DBConnection
        Dim cnn As SqlClient.SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim trans As SqlClient.SqlTransaction = Nothing
        Dim allOK As Boolean = True

        Try
            ' Execute all in a transaction so we can roll back if any failure occurs.
            cnn.Open()
            trans = cnn.BeginTransaction

            For Each r As GridViewRow In gdvPermissions.Rows
                Dim intID As Integer = DirectCast(r.FindControl("lblPermissionID"), Label).Text
                Dim strName As String = DirectCast(r.FindControl("lblPermissionName"), Label).Text
                Dim boolEnabled As Boolean = DirectCast(r.FindControl("lblPermissionEnabled"), CheckBox).Checked

                If intID > 0 Then
                    If boolEnabled Then
                        If Not cExistingUserPermissions.HasPermission(intID) Then

                            cNewUserPermission = New UserPermissions

                            With cNewUserPermission
                                .PermissionID = intID
                                .UserID = ddlPermissionsSearchCriteriaUser.SelectedValue
                            End With

                            allOK = (_Service.Permissions.AddOrUpdateUserPermission(cNewUserPermission, cnn, trans) > 0)

                            If Not allOK Then
                                Exit For
                            End If

                            Dim cUser As User = _Service.getUserByID(cNewUserPermission.UserID)

                            ' They didn't have it and it was added so log change.
                            cLog = New LogEntry(SiteID.LouvreOzroll,
                                        ddlPermissionsSearchCriteriaUser.SelectedValue,
                                        LogCategory.AdminUserPermissionsChange,
                                        LogChangeType.Add,
                                        String.Empty,
                                        cNewUserPermission.LogString,
                                        "Added permission " & strName & " to user " & cUser.UserName & ".",
                                        Date.Now)

                            allOK = _Service.Log.AddLogEntry(cLog, cnn, trans)

                            If Not allOK Then
                                Exit For
                            End If

                        End If
                    Else
                        If cExistingUserPermissions.HasPermission(intID) Then

                            allOK = (_Service.Permissions.DeleteUserPermission(ddlPermissionsSearchCriteriaUser.SelectedValue, intID, cnn, trans) > 0)

                            If Not allOK Then
                                Exit For
                            End If

                            Dim cUser As User = _Service.getUserByID(cExistingUserPermissions.Find(Function(x) x.PermissionID = intID).UserID)

                            ' They had it and it was removed so log change.
                            cLog = New LogEntry(SiteID.LouvreOzroll,
                                        ddlPermissionsSearchCriteriaUser.SelectedValue,
                                        LogCategory.AdminUserPermissionsChange,
                                        LogChangeType.Delete,
                                        cExistingUserPermissions.Find(Function(x) x.PermissionID = intID).LogString,
                                        String.Empty,
                                        "Removed permission " & strName & " from user " & cUser.UserName & ".",
                                        Date.Now)

                            allOK = _Service.Log.AddLogEntry(cLog, cnn, trans)

                            If Not allOK Then
                                Exit For
                            End If

                        End If
                    End If
                End If
            Next r

            If allOK Then
                trans.Commit()
            Else
                EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - Transaction Rolled Back")
                trans.Rollback()
            End If

        Catch ex As Exception

            If Not trans Is Nothing Then
                trans.Rollback()
            End If

            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If

            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message)

        Finally
            trans.Dispose()
            trans = Nothing

            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If

            cnn.Dispose()
            cnn = Nothing
        End Try

        PopulatePermissions()

    End Sub

    Private Sub btnAddUse_Click(sender As Object, e As EventArgs) Handles btnAddUser.Click

        If _Service.Permissions.UserHasPermission(Session("sessUserID"), Permissions.AddUser) Then
           ' Dim encString As String = HttpUtility.UrlEncode(SharedFunctions.Encrypt("Dummyvar=0,userid" & CStr(Session("sessUserID"))))
           ' Response.Redirect("CreateLogin_shutter.aspx?var1=" + encString, False)
	    Response.Redirect("CreateLogin_shutter.aspx")
        End If
    End Sub

#End Region

End Class
