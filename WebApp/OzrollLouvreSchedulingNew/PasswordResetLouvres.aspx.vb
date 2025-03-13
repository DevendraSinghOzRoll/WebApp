
Imports System.Data
Imports System.IO
Imports System.Security.Cryptography
Imports OzrollPSLVSchedulingModel.SharedFunctions

Partial Class PasswordResetLouvres
    Inherits System.Web.UI.Page

    Dim _Service As New AppService

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Dim strKey As String = String.Empty
            Dim intLoginID As Integer = 0

            ViewState("LoginID") = 0

            If Not Request.QueryString.Count = 0 Then
                If Not IsNothing(Request.Params("var1")) Then
                    strKey = SharedFunctions.FindInString(Request.QueryString("var1"), "key")
                    intLoginID = SharedFunctions.FindInString(Request.QueryString("var1"), "id")
                End If
            End If

            If CanResetPassword(strKey, intLoginID) Then
                pnlPasswordEntry.Visible = True
                lblStatus.Visible = False
            Else
                ResetFailed("You cannot reset the password using this link.")
            End If
        End If
    End Sub

    Private Sub ResetSuccess(strMsg As String)

        pnlPasswordEntry.Visible = False
        lblStatus.Visible = True
        btnRedirectLogin.Visible = True

        lblStatus.ForeColor = Drawing.Color.Green
        lblStatus.Text = strMsg
    End Sub

    Private Sub ResetFailed(strMsg As String)

        pnlPasswordEntry.Visible = False
        lblStatus.Visible = True
        btnRedirectLogin.Visible = False

        lblStatus.ForeColor = Drawing.Color.Red
        lblStatus.Text = strMsg
    End Sub

    Private Function CanResetPassword(strKey As String, intLoginID As Integer) As Boolean

        If strKey.Length > 0 AndAlso intLoginID > 0 Then

            Dim cLogin As LoginDetails = _Service.GetLoginDetailsByID(intLoginID)

            If cLogin IsNot Nothing AndAlso Not cLogin.Discontinued Then
                If cLogin.PasswordResetKey = strKey Then
                    If cLogin.PasswordResetKeyExpiryDateTime > Date.Now Then

                        ' All good
                        ViewState("LoginID") = cLogin.LoginID
                        Return True
                    End If
                End If
            End If
        End If

        Return False
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

    Private Sub btnSaveNewPassword_Click(sender As Object, e As EventArgs) Handles btnSaveNewPassword.Click

        Page.Validate()

        If Page.IsValid Then
            If SaveNewPassword() Then
                ResetSuccess("Password successfully changed.")
            End If
        End If
    End Sub

    Private Function SaveNewPassword() As Boolean

        If ViewState("LoginID") IsNot Nothing AndAlso ViewState("LoginID") > 0 Then
            Dim cLogin As LoginDetails = _Service.GetLoginDetailsByID(ViewState("LoginID"))

            If cLogin IsNot Nothing AndAlso Not cLogin.Discontinued Then
                cLogin.Password = SharedFunctions.Encrypt(txtNewPassword.Text)
                cLogin.PasswordResetKey = String.Empty
                cLogin.PasswordResetKeyExpiryDateTime = SharedConstants.MIN_DATE

                Return _Service.UpdateLoginDetails(cLogin)
            End If
        End If

        Return False
    End Function

    Private Sub btnRedirectLogin_Click(sender As Object, e As EventArgs) Handles btnRedirectLogin.Click
        Response.Redirect("Login.aspx")
    End Sub

    Private Sub valcustConfirmNewPassword_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valcustConfirmNewPassword.ServerValidate
        Dim lErrors As List(Of String) = Nothing
        Dim boolValid = True

        If txtNewPassword.Text <> txtConfirmNewPassword.Text Then
            valcustConfirmNewPassword.Text = "Passwords do not match."
            boolValid = False
        End If

        If boolValid Then
            If Not SharedFunctions.PasswordValidate(txtConfirmNewPassword.Text, lErrors) Then
                boolValid = False

                For Each s As String In lErrors
                    valcustConfirmNewPassword.Text = s & "<br />"
                Next s
            End If
        End If

        args.IsValid = boolValid
    End Sub
End Class

