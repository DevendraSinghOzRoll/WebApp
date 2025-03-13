
Imports System.Activities.Expressions
Imports System.Data
Imports System.Linq
Imports System.Net.Mail
Imports System.Web.Services
Imports Newtonsoft.Json

Partial Class CreateLogin_Shutter
    Inherits System.Web.UI.Page
    Dim _Service As New AppService
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim QuitBoolean As Boolean = False
            If Not Request.QueryString.Count = 0 Then
                Dim DummyVar As String = ""
                If Not IsNothing(Request.Params("var1")) Then
                    DummyVar = SharedFunctions.FindInString(Request.QueryString("var1"), "dummyvar")
                End If
                If IsNothing(DummyVar) Then QuitBoolean = True
            Else
                QuitBoolean = True
            End If



            Dim intSiteID As Integer, intUserID As Integer, StrUserName As String = ""


            If Session("sessUserID") = String.Empty Then
                Response.Redirect("Logout.aspx", False)
                Exit Sub
                'ElseIf Convert.ToInt32(Session("IsAdmin")) = 0 Then
                '    Response.Redirect("Logout.aspx", False)
            ElseIf Not _Service.Permissions.UserHasPermission(Session("sessUserID"), SharedEnums.Permissions.AddUser) Then
                Response.Redirect("Logout.aspx", False)
            Else
                intUserID = CInt(Session("sessUserID"))
                StrUserName = Session("sessUserName")
                lblCustomerHeader.Text = Session("sessCustomerName")

                If Convert.ToInt32(Session("CustomerID")) > 0 Then
                    btnDashBoard.Visible = True
                    chkUserType.Checked = False
                    chkUserType.Enabled = False
                    btnHome.Visible = False
                    Dim strSQL As String = "select tu.UserID,ISNULL(tu.CustomerID,0) as CustomerID, tu.UserName,tu.UserFirstName, " &
                        "tu.UserLastName,tu.password,case when tu.Discontinued=0 then 1 when tu.Discontinued=1 then 0 end as Discontinued , " &
                        "ISNULL(tu.IsAdmin,0) as IsAdmin, " &
                        "ISNULL(tc.CustomerName,'') as CustomerName " &
                        "from dbo.tblusers tu " &
                        "left join dbo.tblCustomers tc " &
                        "on tc.CustomeriD=tu.customerid where tu.customerid = " & Convert.ToInt32(Session("CustomerID"))
                    Dim dtUser As DataTable = _Service.RunSQLScheduling(strSQL)
                    If dtUser.Rows.Count > 0 Then

                        hfCustomerID.Value = Convert.ToInt32(dtUser.Rows(0).Item("CustomerID"))
                        If (Convert.ToInt32(hfCustomerID.Value) > 0) Then
                            txtCustomerName.Enabled = False
                            txtCustomerName.Text = dtUser.Rows(0).Item("CustomerName").ToString() 'edited by Fritz 05-05-2021 to show customer name
                        Else
                            txtCustomerName.Text = ""
                        End If
                    End If
                End If
            End If



            If Not Request.QueryString.Count = 0 Then
                If Not IsNothing(Request.Params("SiteID")) Then
                    intSiteID = CInt(Request.Params("SiteID"))
                End If
                If Not IsNothing(Request.Params("UserID")) Then
                    intUserID = CInt(Request.Params("UserID"))
                End If
                If Not IsNothing(Request.Params("UserName")) Then
                    StrUserName = CStr(Request.Params("UserName"))
                End If
            End If




            'txtCurSiteID.Text = IIf(txtCurSiteID.Text.Trim = "", "3", intSiteID)  '//Added for DEV
            'txtCurUserID.Text = intUserID
            'txtCurUserName.Text = StrUserName
            Dim service As New AppService
            'service.addWebsitePageAccess("Ozroll Customer Portal", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)

            txtPassword.Attributes("type") = "password"
            txtConfirmPassword.Attributes("type") = "password"
            'initCtrls()

            'Dim Sqlstr1 As String = "select * from tblUser where customer_id=  " + txtCustID.Text
            FillGrid()
            'End If

            service = Nothing
            lblStatus.Text = ""
            'Else
            '    If btnAddLogin.Text = "Add User" Then
            '        btnCancelLogin.Visible = False
            '        'btnAddLogin.Text = "Add Login"
            '    Else
            '        btnCancelLogin.Visible = True
            '        'btnAddLogin.Text = "Update Login"
            '    End If
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

        'EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & strErrorMessage, getPageInfo())

        Server.ClearError()

        Response.Redirect("GenericErrorPage.aspx", False)
    End Sub

    Private Function getPageInfo() As String
        Dim strPageInfo As String = String.Empty
        Dim strName As String
        If Session.Contents.Count > 0 Then
            strPageInfo = "Session Variables" & Environment.NewLine
            For Each strName In Session.Contents
                strPageInfo &= strName & ": " & CStr(Session.Contents(strName)) & Environment.NewLine
            Next
        Else
            strPageInfo = "No Session Variables" & Environment.NewLine
        End If

        strPageInfo &= Environment.NewLine

        If Me.createLoginForm.HasControls Then
            strPageInfo &= "Visible Form Variables" & Environment.NewLine
            Dim frmCtrl As Control
            For Each frmCtrl In Me.createLoginForm.Controls
                Select Case frmCtrl.GetType.Name
                    Case "TextBox"
                        Dim frmTxt As TextBox
                        frmTxt = DirectCast(frmCtrl, TextBox)
                        strPageInfo &= frmTxt.ID & ": " & frmTxt.Text & Environment.NewLine
                    Case "DropDownList"
                        Dim frmCbo As DropDownList
                        frmCbo = DirectCast(frmCtrl, DropDownList)
                        If frmCbo.Items.Count > 0 Then
                            strPageInfo &= frmCbo.ID & ": " & frmCbo.SelectedItem.Text & " (" & frmCbo.SelectedValue & ")" & Environment.NewLine
                        Else
                            strPageInfo &= frmCbo.ID & ": Not Populated" & Environment.NewLine
                        End If
                    Case "CheckBox"
                        Dim frmChk As CheckBox
                        frmChk = DirectCast(frmCtrl, CheckBox)
                        strPageInfo &= frmChk.ID & ": " & frmChk.Checked & Environment.NewLine
                    Case "Panel"
                        Dim frmPnl As Panel
                        frmPnl = DirectCast(frmCtrl, Panel)
                        Dim pnlCtrl As Control
                        For Each pnlCtrl In frmPnl.Controls
                            Select Case pnlCtrl.GetType.Name
                                Case "TextBox"
                                    Dim pnlTxt As TextBox
                                    pnlTxt = DirectCast(pnlCtrl, TextBox)
                                    strPageInfo &= pnlTxt.ID & ": " & pnlTxt.Text & Environment.NewLine
                                Case "DropDownList"
                                    Dim pnlCbo As DropDownList
                                    pnlCbo = DirectCast(pnlCtrl, DropDownList)
                                    If pnlCbo.Items.Count > 0 Then
                                        strPageInfo &= pnlCbo.ID & ": " & pnlCbo.SelectedItem.Text & " (" & pnlCbo.SelectedValue & ")" & Environment.NewLine
                                    Else
                                        strPageInfo &= pnlCbo.ID & ": Not Populated" & Environment.NewLine
                                    End If
                                Case "CheckBox"
                                    Dim pnlChk As CheckBox
                                    pnlChk = DirectCast(pnlCtrl, CheckBox)
                                    strPageInfo &= pnlChk.ID & ": " & pnlChk.Checked & Environment.NewLine
                            End Select
                        Next
                End Select
            Next
        Else
            strPageInfo &= "No Visible Form Variables" & Environment.NewLine
        End If

        Return strPageInfo
    End Function

    Protected Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Response.Redirect("Logout.aspx", False)
    End Sub

    'Protected Sub initCtrls()
    '    If ChkAdminUser() Then
    '        Dim service As New AppService
    '        Dim dtData As DataTable = service.runSQLScheduling("Select CustomerID, CustomerName from Customers Order By SortOrder")
    '        Dim drow As DataRow = dtData.NewRow
    '        drow("CustomerID") = 0
    '        drow("CustomerName") = ""
    '        dtData.Rows.InsertAt(drow, 0)
    '        Me.cboCustomerName.DataSource = dtData
    '        Me.cboCustomerName.DataValueField = "CustomerID"
    '        Me.cboCustomerName.DataTextField = "CustomerName"
    '        Me.cboCustomerName.DataBind()
    '        Me.cboCustomerName.SelectedIndex = -1
    '        service = Nothing
    '        cboCustomerName.Visible = True
    '        lblCustomerName.Visible = False
    '    End If
    'End Sub
    'Function ChkAdminUser() As Boolean
    '    Try
    '        Dim service As New AppService
    '        Dim dtPermission As DataTable = service.getLoginPermsisionsByLoginID(txtCurUserID.Text)
    '        If dtPermission.Rows.Count > 0 Then
    '            If dtPermission.Select("PermissionID = " & CStr(Constants.DEFAULT_SUPER_ADMIN), String.Empty).Length > 0 Then
    '                Return True
    '            End If
    '        End If
    '        Return False
    '    Catch ex As Exception
    '        EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message)
    '    End Try
    '    Return False
    'End Function

    'Protected Sub cboCustomerName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCustomerName.SelectedIndexChanged
    '    If cboCustomerName.SelectedIndex >= 0 Then
    '        txtCustID.Text = cboCustomerName.SelectedValue.ToString()
    '        Dim Sqlstr As String = "select * from tbllogins lg  "
    '        If Not cboCustomerName.Visible Then
    '            Sqlstr += " inner join (select distinct UserID from tblLoginPermissions where PermissionID <> 51) pr"
    '            Sqlstr += " on lg.UserID=pr.UserID "
    '        End If
    '        Sqlstr += " where CustomerID = '" + txtCustID.Text + "'"
    '        FillGrid(Sqlstr)
    '        lblStatus.Text = ""
    '        If cboCustomerName.SelectedIndex > 0 Then
    '            Panel1.Visible = True
    '        Else
    '            Panel1.Visible = False
    '        End If
    '    End If
    'End Sub

    Private Sub FillGrid()
        Dim service As New AppService
        Dim sqlstr1 As String
        If Convert.ToInt32(Session("CustomerID") = 0) Then
            sqlstr1 = "select tu.UserID,ISNULL(tu.CustomerID,0) as CustomerID, tu.UserName,tu.UserFirstName, " &
                        "tu.UserLastName,tu.password,case when tu.Discontinued=0 then 1 when tu.Discontinued=1 then 0 end as Discontinued, " &
                        "ISNULL(tu.IsAdmin,0) as IsAdmin, " &
                        "ISNULL(tc.CustomerName,'') as CustomerName " &
                        "from dbo.tblusers tu " &
                        "left join dbo.tblCustomers tc " &
                        "on tc.CustomeriD=tu.customerid order by tu.UserID desc"
        Else
            sqlstr1 = "select tu.UserID,ISNULL(tu.CustomerID,0) as CustomerID, tu.UserName,tu.UserFirstName, " &
                        "tu.UserLastName,tu.password,case when tu.Discontinued=0 then 1 when tu.Discontinued=1 then 0 end as Discontinued, " &
                        "ISNULL(tu.IsAdmin,0) as IsAdmin, " &
                        "ISNULL(tc.CustomerName,'') as CustomerName " &
                        "from dbo.tblusers tu " &
                        "left join dbo.tblCustomers tc " &
                        "on tc.CustomeriD=tu.customerid where tu.customerid=" & Convert.ToInt32(Session("CustomerID")) & " order by tu.UserID desc"
        End If
        'Dim sqlstr1 As String = "select UserID,UserName,UserFirstName,UserLastName,Discontinued,ISNULL(IsInternalUser,0) as IsInternalUser from dbo.tblusers order by 1 desc"
        Dim dtUser As DataTable = service.RunSQLScheduling(sqlstr1)
        Me.dgvCustomerLogin.DataSource = dtUser
        Me.dgvCustomerLogin.DataBind()
        service = Nothing
    End Sub
    Protected Sub dgvCustomerLogin_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvCustomerLogin.RowCommand
        Dim service As New AppService
        Dim index As Integer = Convert.ToInt32(e.CommandArgument), strLoginID As String = "", strSQL As String = ""
        Dim row As GridViewRow = dgvCustomerLogin.Rows(index)
        txtGridrowindex.Text = index

        If (e.CommandName = "LoginDetail") Then
            strLoginID = CStr(dgvCustomerLogin.DataKeys(index).Value)
            btnAddLogin.Text = "Update User"
            strSQL = "select tu.UserID,ISNULL(tu.CustomerID,0) as CustomerID, tu.UserName,tu.UserFirstName, " &
                        "tu.UserLastName,tu.password,tu.Discontinued, ISNULL(tu.ProductTypeID,0) ProductTypeID, " &
                        "ISNULL(tu.IsAdmin,0) as IsAdmin, " &
                        "ISNULL(tc.CustomerName,'') as CustomerName " &
                        "from dbo.tblusers tu " &
                        "left join dbo.tblCustomers tc " &
                        "on tc.CustomeriD=tu.customerid where UserID = " & strLoginID
            Dim dtUser As DataTable = service.RunSQLScheduling(strSQL)
            If dtUser.Rows.Count > 0 Then
                'txtPassword.Text = SharedFunctions.Decrypt(HttpUtility.UrlDecode(dtUser.Rows(0).Item(0).ToString()))
                'txtConfirmPassword.Text = SharedFunctions.Decrypt(HttpUtility.UrlDecode(dtUser.Rows(0).Item(0).ToString()))
		'All the below integer values changed to Column Names by Surendra on 5th of June 2023
                hfUserID.Value = Convert.ToInt32(dtUser.Rows(0).Item("UserID"))
                hfCustomerID.Value = Convert.ToInt32(dtUser.Rows(0).Item("CustomerID"))
                txtUsername.Text = dtUser.Rows(0).Item("UserName").ToString()
                txtFirstName.Text = dtUser.Rows(0).Item("UserFirstName").ToString()
                txtLastName.Text = dtUser.Rows(0).Item("UserLastName").ToString()
                'txtEmail.Text = dtUser.Rows(0).Item(0).ToString()
                txtConfirmPassword.Text = dtUser.Rows(0).Item("password").ToString()
                txtPassword.Text = dtUser.Rows(0).Item("password").ToString()
                txtUsername.Enabled = False
                If (Convert.ToInt32(hfCustomerID.Value) > 0) Then
                    txtCustomerName.Enabled = False
                    txtCustomerName.Text = dtUser.Rows(0).Item("CustomerName").ToString() 'edited by Fritz 05-05-2021 to show customer name
                    chkUserType.Checked = False
                    'chkUserType.Enabled = False
                ElseIf Convert.ToInt32(hfCustomerID.Value) = 0 Then
                    txtCustomerName.Enabled = False
                    txtCustomerName.Text = ""
                    chkUserType.Checked = True
                    chkUserType.Enabled = True
                Else
                    txtCustomerName.Text = ""
                End If
                If (Convert.ToInt32(dtUser.Rows(0).Item("Discontinued")) = 0) Then
                    chkContinued.Checked = True
                Else
                    chkContinued.Checked = False
                End If
                If (Convert.ToInt32(dtUser.Rows(0).Item("IsAdmin")) = 1) Then
                    chkAdmin.Checked = True
                Else
                    chkAdmin.Checked = False
                End If

                'Added by Michael Behar - 20-05-2021 - To Allow Selection of What Portal is required.
                Dim intProductTypeID As Integer = CInt(dtUser.Rows(0).Item("ProductTypeID").ToString)
                chkPlantation.Checked = False
                chkLouvre.Checked = False

                If intProductTypeID = (SharedEnums.ProductType.Louvres + SharedEnums.ProductType.Plantation) Then
                    chkPlantation.Checked = True
                    chkLouvre.Checked = True
                ElseIf intProductTypeID = SharedEnums.ProductType.Louvres Then
                    chkLouvre.Checked = True
                ElseIf intProductTypeID = SharedEnums.ProductType.Plantation Then
                    chkPlantation.Checked = True
                End If

            End If
            lblStatus.Text = ""
            'ElseIf (e.CommandName = "ResetPassword") Then
            '    Dim passwordToken As String = service.GenerateToken(), strLink As String
            '    Dim strUser As String = row.Cells(GetGridColumnIndexByName(row, "UserName")).Text
            '    If service.addPasswordToken(txtCurUserName.Text, passwordToken) Then
            '        Dim encString As String = HttpUtility.UrlEncode(SharedFunctions.Encrypt("token=" + passwordToken))
            '        strLink = Request.Url.Host & "/passwordreset.aspx?var1=" + encString
            '        ' SendResetEmail(strLink, strUser)
            '    End If
        End If
        service = Nothing
    End Sub
    Private Sub FilltxtBoxes(row As GridViewRow, index As Integer)
        Dim ChkActive As CheckBox, ChkAdministartor As CheckBox
        txtUsername.Text = row.Cells(GetGridColumnIndexByName(row, "UserName")).Text
        txtFirstName.Text = row.Cells(GetGridColumnIndexByName(row, "First_Name")).Text
        txtLastName.Text = row.Cells(GetGridColumnIndexByName(row, "Last_Name")).Text
        'txtPassword.Text = row.Cells(GetGridColumnIndexByName(row, "password")).Text
        'txtConfirmPassword.Text = row.Cells(GetGridColumnIndexByName(row, "password")).Text
        'txtEmail.Text = row.Cells(GetGridColumnIndexByName(row, "Email")).Text
        ChkActive = TryCast(row.FindControl("chkGridContinued"), CheckBox)
        ChkAdministartor = TryCast(row.FindControl("chkGridAdmin"), CheckBox)
        btnAddLogin.Text = "Update User"
        btnCancelLogin.Visible = True
        chkContinued.Checked = ChkActive.Checked
        chkAdmin.Checked = ChkAdministartor.Checked
    End Sub
    Private Function GetGridColumnIndexByName(row As GridViewRow, columnName As String) As Integer
        Dim columnIndex As Integer = 0
        For Each cell As DataControlFieldCell In row.Cells
            Dim conName As String = cell.ContainingField.ToString.ToLower()
            If TypeOf cell.ContainingField Is BoundField Then
                ' If DirectCast(cell.ContainingField, BoundField).DataField.Equals(columnName) Then
                If DirectCast(cell.ContainingField, BoundField).DataField.Equals(columnName) Then
                    Exit For
                End If
            End If
            If TypeOf cell.ContainingField Is CommandField Then

                If conName.Equals(columnName) Then
                    ' If DirectCast(cell.ContainingField, CommandField).Equals(columnName) Then
                    Exit For
                End If
            End If
            ' keep adding 1 while we don't have the correct name
            columnIndex += 1
        Next
        Return columnIndex
        'Public Shared Function GetIndex(gv As GridView, columnText As String) As Integer
        'For i As Integer = 0 To gv.Columns.Count - 1
        '    If gv.Columns(i).HeaderText = columnText Then
        '        Return i
        '    End If
        'Next
        '    'Return -1
        'End Function
    End Function
    Protected Sub btnAddLogin_Click(sender As Object, e As EventArgs) Handles btnAddLogin.Click

        Dim cUsers As New User
        Dim result As Integer
        Dim strSQL As String
        Dim strSQL1 As String
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim flag As Integer
        Dim dtCustomer As DataTable = New DataTable()

        strSQL = "select ISNULL(WholesaleLouvres,0) WholesaleLouvres,ISNULL(Plantations,0) Plantations,ISNULL(RetailLouvres,0) RetailLouvres from tblCustomers with(nolock) where CustomeriD=" & hfCustomerID.Value & " and Discontinued=0"
        dtCustomer = _Service.RunSQLScheduling(strSQL)

        If hfUserID.Value = "" Then
            ''added by surendra dt:03/06/2023 to check userpermission
            ''check internal customer permission if user is not admin by surendra dt:06/06/2023
            Dim bolHasPermission As Boolean = _Service.Permissions.UserHasPermission(Session("sessUserID"), SharedEnums.Permissions.Admin) Or _Service.Permissions.UserHasPermission(Session("sessUserID"), SharedEnums.Permissions.TrackingPageAccess)

            ''If (Convert.ToInt32(Session("CustomerID")) > 0) Or (Convert.ToInt32(Session("CustomerID")) <= 0 AndAlso (Not _Service.Permissions.UserHasPermission(Session("sessUserID"), SharedEnums.Permissions.Admin) AndAlso Not _Service.Permissions.UserHasPermission(Session("sessUserID"), SharedEnums.Permissions.TrackingPageAccess))) Then
            If (Convert.ToInt32(Session("CustomerID")) > 0) Or (Convert.ToInt32(Session("CustomerID")) <= 0 AndAlso bolHasPermission = False) Then
                strSQL = "select * from dbo.tblUsers where customerid = '" & hfCustomerID.Value & "'"
                flag = 0
            Else
                strSQL = "select * from dbo.tblUsers where Createdby = '" & Convert.ToInt32(Session("sessUserID")) & "'"
                flag = 1
            End If
            dt = _Service.RunSQLScheduling(strSQL)

            strSQL1 = "select * from dbo.tblUsers where UserName = '" & txtUsername.Text & "'"
            dt1 = _Service.RunSQLScheduling(strSQL1)


            If dt.Rows.Count > 5 And flag = 0 Then
                lblStatus.Text = "You can add maximum 5 users per customer."
                'commented by Fritz 12/03/2021 64670
                'ElseIf dt.Rows.Count > 10 And flag = 1 Then
                '    lblStatus.Text = "Maximum 10 users can be created by an admin user."
            ElseIf dt1.Rows.Count > 0 Then
                lblStatus.Text = "User name already exists."
                ''added by surendra dt:04/06/2023 check user permission
            ElseIf dtCustomer.Rows.Count > 0 Then
                Dim intLouvres As Integer = 0
                Dim intPlantation As Integer = 0
                intLouvres = Convert.ToInt32(dtCustomer.Rows(0)("WholesaleLouvres")) + Convert.ToInt32(dtCustomer.Rows(0)("RetailLouvres"))
                intPlantation = Convert.ToInt32(dtCustomer.Rows(0)("Plantations"))
                If chkPlantation.Checked AndAlso intPlantation = 0 Then
                    lblStatus.Text = "Customer does not have the permission to access Plantation."
                ElseIf chkLouvre.Checked AndAlso intLouvres = 0 Then
                    lblStatus.Text = "Customer does not have the permission to access Louvres."
                Else
                    If chkUserType.Checked = True Then
                        cUsers.CustomerID = 0
                    Else
                        cUsers.CustomerID = Convert.ToInt32(hfCustomerID.Value)
                    End If

                    cUsers.UserFirstName = txtFirstName.Text
                cUsers.UserLastName = txtLastName.Text
                cUsers.UserName = txtUsername.Text
                cUsers.Email = ""
                cUsers.Password = txtConfirmPassword.Text
                If chkContinued.Checked Then
                    cUsers.Discontinued = 0
                Else
                    cUsers.Discontinued = 1
                End If
                'cUsers.Discontinued = chkContinued.Checked
                If (hfUserID.Value = "") Then
                    cUsers.UserID = 0
                Else
                    cUsers.UserID = Convert.ToInt32(hfUserID.Value)
                End If
                If chkAdmin.Checked = True Then
                    cUsers.IsAdmin = 1
                Else
                    cUsers.IsAdmin = 0
                End If
                cUsers.CreatedBy = Convert.ToInt32(Session("sessUserID"))

                'Added by Michael Behar - 20-05-2021
                Dim intProductTypeID As Integer = SharedEnums.ProductType.NONE
                If chkPlantation.Checked Then
                    intProductTypeID += SharedEnums.ProductType.Plantation
                End If
                If chkLouvre.Checked Then
                    intProductTypeID += SharedEnums.ProductType.Louvres
                End If
                cUsers.ProductTypeID = intProductTypeID
                result = _Service.AddUserLogin(cUsers)
                If result > 0 Then
                    lblStatus.Text = "Record Saved Successfully"
                    ClearText()
                    FillGrid()

                    Else
                        lblStatus.Text = "Could not save record.Please try again"
                    End If
                End If
            End If

        Else
            If chkUserType.Checked = True Then
                cUsers.CustomerID = 0
            Else
                cUsers.CustomerID = Convert.ToInt32(hfCustomerID.Value)
            End If
            cUsers.UserFirstName = txtFirstName.Text
            cUsers.UserLastName = txtLastName.Text
            cUsers.UserName = txtUsername.Text
            cUsers.Email = ""
            cUsers.Password = txtConfirmPassword.Text
            If chkContinued.Checked Then
                cUsers.Discontinued = 0
            Else
                cUsers.Discontinued = 1
            End If
            'cUsers.Discontinued = chkContinued.Checked
            cUsers.UserID = Convert.ToInt32(hfUserID.Value)
            If chkAdmin.Checked = True Then
                cUsers.IsAdmin = 1
            Else
                cUsers.IsAdmin = 0
            End If
            cUsers.CreatedBy = Convert.ToInt32(Session("sessUserID"))

            'Added by Michael Behar - 20-05-2021
            Dim intProductTypeID As Integer = SharedEnums.ProductType.NONE
            If chkPlantation.Checked Then
                intProductTypeID += SharedEnums.ProductType.Plantation
            End If
            If chkLouvre.Checked Then
                intProductTypeID += SharedEnums.ProductType.Louvres
            End If
            cUsers.ProductTypeID = intProductTypeID
            ''added by surendra dt:04/06/2023 check user permission
            If dtCustomer.Rows.Count > 0 Then
                Dim intLouvres As Integer = 0
                Dim intPlantation As Integer = 0
                intLouvres = Convert.ToInt32(dtCustomer.Rows(0)("WholesaleLouvres")) + Convert.ToInt32(dtCustomer.Rows(0)("RetailLouvres"))
                intPlantation = Convert.ToInt32(dtCustomer.Rows(0)("Plantations"))
                If chkPlantation.Checked AndAlso intPlantation = 0 Then
                    lblStatus.Text = "Customer does not have the permission to access Plantation."
                ElseIf chkLouvre.Checked AndAlso intLouvres = 0 Then
                    lblStatus.Text = "Customer does not have the permission to access Louvres."
                Else
                    result = _Service.AddUserLogin(cUsers)
                    If result > 0 Then
                        lblStatus.Text = "Record Update Successfully"
                        ClearText()
                        FillGrid()
                        btnAddLogin.Text = "Add User"
                    Else
                        lblStatus.Text = "Could not update record.Please try again"
                    End If
                End If
            End If

        End If
        'Dim AddStatus As Boolean
        'AddStatus = IIf(btnAddLogin.Text = "Add Login", True, False)

        'Dim textBoxes = Panel1.Controls.OfType(Of TextBox)()
        'For Each txtBox In textBoxes
        '    If txtBox.ClientID <> "txtLastName" And txtBox.ClientID <> "txtGridrowindex" Then
        '        If Not (AddStatus = False And (txtBox.ClientID = "txtPassword" Or txtBox.ClientID = "txtConfirmPassword") And txtBox.Text = "") And
        '                txtBox.Text = String.Empty Then
        '            lblStatus.Text = "Please input all required fields"
        '            Exit Sub
        '        End If
        '    End If
        'Next
        'If txtPassword.Text <> txtConfirmPassword.Text Then
        '    lblStatus.Text = "Password does not matches. Please try again."
        '    Exit Sub
        'End If
        'If Not SharedFunctions.ChkSQlInjection(textBoxes, lblStatus) Then
        '    ' txtErrorLabel.Text = lblStatus.Text
        '    Exit Sub
        'End If

        'lblStatus.Text = ""
        '' Dim RowId As Integer = DirectCast(DirectCast(sender, Button).Parent.Parent, GridViewRow).RowIndex
        'Dim service As New AppService
        'Dim strSQL As String, strLoginID As String, strSalt As String = service.GenerateSalt()
        'strLoginID = "-99"
        'strSQL = "select * from dbo.customer_login where username = '" & txtUsername.Text & "'"
        'If Not AddStatus Then
        '    strLoginID = CStr(dgvCustomerLogin.DataKeys(txtGridrowindex.Text).Value)
        '    strSQL &= " and id <> " & strLoginID
        'End If

        'Dim dtUser As DataTable = service.getUsersDetails(strSQL)
        'If dtUser.Rows.Count > 0 Then
        '    lblStatus.Text = "User Already Exists. Please try again"
        '    Exit Sub
        'End If

        'strSQL = "select * from dbo.customer_login where email = '" & txtEmail.Text + "'"
        'If Not AddStatus Then
        '    strSQL &= " and id <> " & strLoginID
        'End If

        'dtUser = service.getUsersDetails(strSQL)
        'If dtUser.Rows.Count > 0 Then
        '    lblStatus.Text = "Email Already Exists. Please try again"
        '    Exit Sub
        'End If

        'Dim drow As DataRow
        'If AddStatus Then
        '    drow = dtUser.NewRow
        '    drow.Item("ID") = strLoginID
        '    drow.Item("Customer_ID") = txtCustID.Text
        '    AddStatus = True
        'Else
        '    strSQL = "select * from dbo.customer_login where id = " & strLoginID
        '    dtUser = service.getUsersDetails(strSQL)
        '    If dtUser.Rows.Count < 1 Then Exit Sub

        '    drow = dtUser.Rows(0)
        '    strSalt = drow("salt")
        '    AddStatus = False
        'End If

        'drow.Item("UserName") = txtUsername.Text
        'drow.Item("First_Name") = txtFirstName.Text
        'drow.Item("Last_Name") = txtLastName.Text
        'If AddStatus Or txtPassword.Text <> String.Empty Then
        '    drow.Item("Password") = service.getHashPassword(txtPassword.Text, strSalt)
        '    drow.Item("salt") = strSalt
        'End If
        ''drow.Item("Discontinued") = IIf(chkContinued.Checked, 0, 1)
        'drow.Item("Active") = chkContinued.Checked
        'drow.Item("Administrator") = chkAdmin.Checked
        'drow.Item("Email") = txtEmail.Text

        'If service.addLoginDetailOZOTS(drow) = True Then
        '    If AddStatus = False Then
        '        btnCancelLogin.Visible = True
        '        btnAddLogin.Text = "Add Login"
        '    End If

        '    Dim Sqlstr1 = "select * from customer_login where customer_id=  " + txtCustID.Text
        '    FillGrid(Sqlstr1)

        '    ClearText()
        '    lblStatus.Text = "Record Saved Successfully"
        '    btnCancelLogin.Visible = False
        'Else
        '    lblStatus.Text = "Could Not Save the record"
        'End If

        'service = Nothing

    End Sub
    'Protected Sub dgvCustomerLogin_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgvCustomerLogin.RowDataBound
    '    If (e.Row.RowType = DataControlRowType.DataRow) Then
    '        txtGridrowindex.Attributes.Add("onfocus", "javascript:return DoPostBackWithRowIndex('" + e.Row.RowIndex.ToString() + "');")
    '    End If
    'End Sub
    Protected Sub btnCancelLogin_Click(sender As Object, e As EventArgs) Handles btnCancelLogin.Click

        btnAddLogin.Text = "Add User"
        txtGridrowindex.Text = ""
        ClearText()
        lblStatus.Text = ""
    End Sub
    Private Sub ClearText()

        hfUserID.Value = ""
        If (Convert.ToInt32(Session("CustomerID")) = 0) Then
            txtCustomerName.Text = ""
            hfCustomerID.Value = ""
        End If
        If chkUserType.Checked = True Then
            txtCustomerName.Enabled = False

        Else
            txtCustomerName.Enabled = True
        End If
        txtUsername.Text = ""
        txtLastName.Text = ""
        txtPassword.Text = ""
        txtConfirmPassword.Text = ""
        txtGridrowindex.Text = ""
        'txtEmail.Text = ""
        txtFirstName.Text = ""
        chkAdmin.Checked = False
        chkContinued.Checked = True
        txtCustomerName.Enabled = True
        txtUsername.Enabled = True

        FillGrid()

    End Sub
    Protected Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        'SharedFunctions.DeleteTempFoldersNfiles(Server.MapPath("~/UpLoadedFiles/"), "DTUploadFiles")
        Dim encString As String = HttpUtility.UrlEncode(SharedFunctions.Encrypt("dummyvar=0"))
        'Response.Redirect("ShuttersHome.aspx?var1=" + encString, True)
        Response.Redirect("Home.aspx", False)
    End Sub

    <WebMethod()>
    Public Shared Function AutoSearchCustomerName(ByVal text As String) As String
        Dim response As String
        Dim appServices As New AppService
        Dim dt As New DataTable
        Dim lstCusotmer As New List(Of Customer)
        response = ""
        dt = appServices.RunSQLScheduling("select CustomeriD,CustomerName from tblCustomers where CustomerName like'" + text + "%' and WholesaleLouvres <> 0")
        If dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                lstCusotmer.Add(New Customer With {
                    .CustomerName = Convert.ToString(dr("CustomerName")),
                    .CustomerID = Convert.ToInt32(dr("CustomeriD"))
                })
            Next
            response = JsonConvert.SerializeObject(lstCusotmer)
        End If
        Return response

    End Function


    Protected Sub dgvCustomerLogin_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvCustomerLogin.PageIndexChanging
        dgvCustomerLogin.PageIndex = e.NewPageIndex
        Me.FillGrid()
    End Sub

    Protected Sub txtCustomerName_TextChanged(sender As Object, e As EventArgs) Handles txtCustomerName.TextChanged





    End Sub

    'Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

    '    Dim strSql As String
    '    Dim dt As New DataTable
    '    Dim service As New AppService
    '    Dim CustomerId As Integer
    '    If hfCustomerID.Value = "" Then
    '        CustomerId = 0
    '    Else
    '        CustomerId = Convert.ToInt32(hfCustomerID.Value)
    '    End If
    '    Dim sqlstr1 As String = "select tu.UserID,ISNULL(tu.CustomerID,0) as CustomerID, tu.UserName,tu.UserFirstName, " &
    '                    "tu.UserLastName,tu.password,tu.Discontinued, " &
    '                    "ISNULL(tu.IsInternalUser,0) as IsInternalUser, " &
    '                    "ISNULL(tc.CustomerName,'') as CustomerName " &
    '                    "from dbo.tblusers tu " &
    '                    "left join dbo.tblCustomers tc " &
    '                    "on tc.CustomeriD=tu.customerid where tu.CustomerID=" & CustomerId & ""
    '    'Dim sqlstr1 As String = "select UserID,UserName,UserFirstName,UserLastName,Discontinued,ISNULL(IsInternalUser,0) as IsInternalUser from dbo.tblusers where customerId=" & CustomerId & ""
    '    Dim dtUser As DataTable = service.RunSQLScheduling(sqlstr1)
    '    If dtUser.Rows.Count > 0 Then
    '        Me.dgvCustomerLogin.DataSource = dtUser
    '        Me.dgvCustomerLogin.DataBind()
    '    Else
    '        service = Nothing
    '        FillGrid()

    '    End If

    'End Sub




    Protected Sub imgButton_Click(sender As Object, e As ImageClickEventArgs) Handles imgButton.Click

        Dim strSql As String
        Dim dt As New DataTable
        Dim service As New AppService
        Dim CustomerId As Integer
        If hfCustomerID.Value = "" Then
            CustomerId = 0
        Else
            CustomerId = Convert.ToInt32(hfCustomerID.Value)
        End If
        Dim sqlstr1 As String = "select tu.UserID,ISNULL(tu.CustomerID,0) as CustomerID, tu.UserName,tu.UserFirstName, " &
                        "tu.UserLastName,tu.password,case when tu.Discontinued=0 then 1 when tu.Discontinued=1 then 0 end as Discontinued, " &
                        "ISNULL(tu.IsAdmin,0) as IsAdmin, " &
                        "ISNULL(tc.CustomerName,'') as CustomerName " &
                        "from dbo.tblusers tu " &
                        "left join dbo.tblCustomers tc " &
                        "on tc.CustomeriD=tu.customerid where tc.CustomerName like '%" & txtCustomerName.Text & "%'"
        'Dim sqlstr1 As String = "select UserID,UserName,UserFirstName,UserLastName,Discontinued,ISNULL(IsInternalUser,0) as IsInternalUser from dbo.tblusers where customerId=" & CustomerId & ""
        Dim dtUser As DataTable = service.RunSQLScheduling(sqlstr1)
        If dtUser.Rows.Count > 0 Then
            Me.dgvCustomerLogin.DataSource = dtUser
            Me.dgvCustomerLogin.DataBind()
        Else
            service = Nothing
            FillGrid()

        End If


    End Sub

    Protected Sub chkAdmin_CheckedChanged(sender As Object, e As EventArgs) Handles chkAdmin.CheckedChanged

    End Sub

    Protected Sub chkUserType_CheckedChanged(sender As Object, e As EventArgs) Handles chkUserType.CheckedChanged
        If chkUserType.Checked = True Then
            txtCustomerName.Enabled = False

        Else
            txtCustomerName.Enabled = True
        End If
    End Sub

    Protected Sub btnDashBoard_Click(sender As Object, e As EventArgs) Handles btnDashBoard.Click
        Response.Redirect("Dashboard.aspx", False)
    End Sub
End Class
