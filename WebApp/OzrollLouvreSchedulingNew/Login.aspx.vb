
Imports OzrollPSLVSchedulingModel.SharedEnums

Partial Class ozroll_Login
    Inherits System.Web.UI.Page

    Dim service As New AppService

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("sessUserID") = String.Empty
            Session("sessUserName") = String.Empty
            Session("sessSiteID") = String.Empty
            Session("sessSiteName") = String.Empty
            Session("sessEmployeeID") = String.Empty
            ' below line has been commented by Suredra on 16/07/2020
            'below line has been uncommented by Suredra on 26/08/2020
            Session("sessProductTypeID") = CInt(SharedEnums.ProductType.Louvres)

            Session("IsAdmin") = String.Empty

            btnLogin.Attributes.Add("onclick", "return ValidateAll();")



            Dim strP3 = Request.QueryString("P3") ' Time

            If (String.IsNullOrEmpty(strP3)) Then
            Else
                Dim plainText3 As String = DecryptString(strP3)
                If System.DateTime.Now <= Convert.ToDateTime(plainText3) Then
                    Dim strP1 As String = Request.QueryString("P1") ' User Name 
                    Dim strP2 As String = Request.QueryString("P2") ' Passwrd
                    Dim strProductTypeId As String = Request.QueryString("ID") ' Product TypeId

                    Dim plainText1 As String = DecryptString(strP1)
                    Dim plainText2 As String = DecryptString(strP2)
                    Dim strCID As String = Request.QueryString("CID") ' Product TypeId
                    Dim strCurrentCID As String = DecryptString(strCID)
                    'CheckAndUpdateUserPassword(plainText1, plainText2, strCurrentCID)
                    txtUsername.Text = plainText1
                    txtPassword.Text = plainText2
                    If strCurrentCID IsNot Nothing Then
                        OneLogin()
                    Else
                        UserLogin()
                    End If

                End If
                'End If
            End If
            ' Code End For
        End If
    End Sub

    Public Function DecryptString(ByVal encrString As String) As String
        Dim bytes As Byte()
        Dim decrypted As String
        Try
            bytes = Convert.FromBase64String(encrString)
            decrypted = System.Text.ASCIIEncoding.ASCII.GetString(bytes)
        Catch fe As FormatException
            decrypted = ""
        End Try

        Return decrypted
    End Function
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

        If Me.loginForm.HasControls Then
            strPageInfo &= "Visible Form Variables" & Environment.NewLine
            Dim frmCtrl As Control
            For Each frmCtrl In Me.loginForm.Controls
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

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click

        Dim strCID As String = Request.QueryString("CID") ' Product TypeId
        If strCID IsNot Nothing Then
            Dim strCurrentCID As String = DecryptString(strCID)
            If strCurrentCID IsNot Nothing Then
                OneLogin()
            Else
                UserLogin()
            End If
        Else
            UserLogin()
        End If



    End Sub

    Private Sub UserLogin()
        If txtUsername.Text = String.Empty Then
            lblStatus.Text = "Please enter your username."
            Exit Sub
        End If
        If txtPassword.Text = String.Empty Then
            lblStatus.Text = "Please enter your password."
            Exit Sub
        End If

        ' Testing Only
        'If (LCase(txtUsername.Text) = "plantation" And LCase(txtPassword.Text) = "plantation") Then
        '    FormsAuthentication.SetAuthCookie(Me.txtUsername.Text, False)

        '    Session("sessUserID") = "1"
        '    Session("sessUserName") = LCase(txtUsername.Text)
        '    Session("sessSiteID") = "1"

        '    Response.Redirect("Home.aspx", False)
        'Else
        '    lblStatus.Text = "Invalid login details."
        'End If
        ' Testing Only

        'added by Fritz to match plantation login #65783
        Dim dtTradeName As DataTable = New DataTable()
        Dim tradename As String = String.Empty
        Dim dtCustomerValiation As DataTable = New DataTable()

        Dim lUsers As List(Of User) = service.GetUsers()

        Dim cUser As User = lUsers.Find(Function(x) x.UserName = txtUsername.Text AndAlso
                                                    x.Password = txtPassword.Text AndAlso
                                                    x.ProductTypeID.HasFlag(SharedEnums.ProductType.Louvres) AndAlso
                                                    Not x.Discontinued)
        If cUser IsNot Nothing Then

            FormsAuthentication.SetAuthCookie(txtUsername.Text, True)
            'added by Fritz to match plantation login #65783
            dtTradeName = service.RunSQLScheduling("select ISNULL(TradingName,CustomerName) as TradingName from tblCustomers where customerid='" & cUser.CustomerID & "'")
            If dtTradeName IsNot Nothing AndAlso dtTradeName.Rows.Count > 0 Then
                tradename = Convert.ToString(dtTradeName.Rows.Item(0)(0))
            End If
            Session("sessTradeName") = tradename

            Session("CustomerID") = cUser.CustomerID
            'edited by Fritz 09/03/2021 #64432
            SharedConstants.CUSTOMER_ID = cUser.CustomerID
            Session("sessUserID") = CStr(cUser.ID)
            Session("sessUserName") = cUser.UserName
            Session("IsAdmin") = cUser.IsAdmin

            'commented by Fritz to match plantation login #65783
            'Session("CustomerName") = cUser.UserFirstName.ToUpper() + " " + cUser.UserLastName.ToUpper()
            ''commented by surendra 26/08/2020
            'Session("sessProductTypeID") = CInt(cUser.ProductTypeID)
            'If (cUser.CustomerID = 0) Then
            '	Response.Redirect("Home.aspx", False)
            'ElseIf (cUser.CustomerID > 0) Then
            '	'Response.Redirect("ProductionScheduleList.aspx", False)
            '	Response.Redirect("Dashboard.aspx", False)
            'Else
            '	lblLoginMsg.Text = "Login detail does not match. Please contact admin."
            'End If

            'updated by Fritz to match plantation login #65783
            ''added by surendra dt:01/06/2023 ticket #75183 prevent extarnal customer login as internal.
            Dim cUserPermissions As UserPermissionsContainer = service.Permissions.UserHasPermissions(CInt(cUser.ID))
            ''check internal customer permission if user is not admin by surendra dt:06/06/2023
            Dim bolHasPermission = cUserPermissions.HasPermission(Permissions.Admin) Or cUserPermissions.HasPermission(Permissions.TrackingPageAccess)
            If cUser.CustomerID <= 0 AndAlso (bolHasPermission) Then
                Response.Redirect("Home.aspx", False)
            Else
                Dim strSql As String = "select count(*) from tblCustomers where Customerid=" & cUser.CustomerID & ""
                dtCustomerValiation = service.RunSQLScheduling(strSql)
                If dtCustomerValiation IsNot Nothing AndAlso dtCustomerValiation.Rows.Count > 0 Then
                    Response.Redirect("Dashboard.aspx", False)
                Else
                    lblStatus.Text = "Invalid login. Contact Ozroll IT."
                End If
            End If
        Else
            lblLoginMsg.Text = "Invalid login details."
        End If

    End Sub


    Private Sub OneLogin()
        If txtUsername.Text = String.Empty Then
            lblStatus.Text = "Please enter your username."
            Exit Sub
        End If
        If txtPassword.Text = String.Empty Then
            lblStatus.Text = "Please enter your password."
            Exit Sub
        End If

        Dim strP1 As String = Request.QueryString("P1") ' User Name 
        Dim strP2 As String = Request.QueryString("P2") ' Passwrd

        Dim plainText1 As String = DecryptString(strP1)
        Dim plainText2 As String = DecryptString(strP2)

        Dim strProductTypeId As String = Request.QueryString("ID") 'customer_id

        Dim strCustomerId As String = DecryptString(strProductTypeId)


        Dim strCurrentUserId As String = Request.QueryString("CID") ' Table Id
        Dim strCurrentUserIdPlan As String = DecryptString(strCurrentUserId)


        Dim dtTradeName As DataTable = New DataTable()
        Dim tradename As String = String.Empty
        Dim dtCustomerValiation As DataTable = New DataTable()
        Dim lUsers As List(Of User) = service.GetUsers_OneLogin(plainText1, strCustomerId, strCurrentUserIdPlan)

        'Dim cUser As User = lUsers.Find(Function(x) x.UserName = txtUsername.Text AndAlso
        'x.Password = txtPassword.Text AndAlso
        'x.ProductTypeID.HasFlag(SharedEnums.ProductType.Plantation)) AndAlso Not x.Discontinued

        txtPassword.Text = lUsers(0).Password
        txtUsername.Text = lUsers(0).UserName

        Dim cUser As User = lUsers.Find(Function(x) x.UserName = txtUsername.Text AndAlso
        x.Password = txtPassword.Text AndAlso
        x.ProductTypeID.HasFlag(SharedEnums.ProductType.Louvres) AndAlso
                                                   Not x.Discontinued)
        If cUser IsNot Nothing Then

            FormsAuthentication.SetAuthCookie(txtUsername.Text, True)
            dtTradeName = service.RunSQLScheduling("select ISNULL(TradingName,CustomerName) as TradingName from tblCustomers where customerid='" & cUser.CustomerID & "'")
            If dtTradeName IsNot Nothing AndAlso dtTradeName.Rows.Count > 0 Then
                tradename = Convert.ToString(dtTradeName.Rows.Item(0)(0))
            End If
            Session("sessTradeName") = tradename
            Session("sessUserID") = CStr(cUser.ID)
            'edited by Fritz 09/03/2021 #64432  - Preventing the customer to Discount
            SharedConstants.CUSTOMER_ID = cUser.CustomerID
            Session("sessUserName") = cUser.UserName
            Session("IsAdmin") = cUser.IsAdmin
            Session("CustomerId") = cUser.CustomerID
            ''added by surendra dt:01/06/2023 ticket #75183 prevent extarnal customer login as internal.

            Dim cUserPermissions As UserPermissionsContainer = service.Permissions.UserHasPermissions(CInt(cUser.ID))
            Dim bolHasPermission As Boolean = cUserPermissions.HasPermission(Permissions.Admin) Or cUserPermissions.HasPermission(Permissions.TrackingPageAccess)
            If cUser.CustomerID = 0 AndAlso bolHasPermission Then
                Response.Redirect("Home.aspx", False)
            Else
                Dim strSql As String = "select count(*) from tblCustomers where Customerid=" & cUser.CustomerID & ""
                dtCustomerValiation = service.RunSQLScheduling(strSql)
                If dtCustomerValiation IsNot Nothing AndAlso dtCustomerValiation.Rows.Count > 0 Then
                    Response.Redirect("Dashboard.aspx", False)
                Else
                    lblStatus.Text = "Invalid login. Contact Ozroll IT."
                End If
            End If

        Else
            lblStatus.Text = "Invalid login details."
            txtUsername.Text = ""
        End If

    End Sub

End Class
