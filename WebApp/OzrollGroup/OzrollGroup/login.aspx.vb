Imports System.Data.SqlClient

Public Class login
    Inherits System.Web.UI.Page


    'Dim connectionString As String = "Server=OZOTSSVR\OZROLL;Database=smartbase16March2022;User ID=smartbase;Password=smartbase;Trusted_Connection=False;Connect Timeout=360"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Session("P1") = String.Empty ' user Name
            Session("P2") = String.Empty 'Password
            Session("P3") = String.Empty 'DateTime 
            Session("P4") = String.Empty 'DateTime 

        End If
    End Sub
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

        If txtUsername.Text = String.Empty Then
            lblResetSent.Text = "Please enter your username."
            Exit Sub
        End If
        If txtPassword.Text = String.Empty Then
            lblResetSent.Text = "Please enter your password."
            Exit Sub
        End If
        Dim dbcon As New DBConnection
        Dim connectionString As String = dbcon.getConnection_To_ozots()

        ' Check User EXISTS Or Not
        Dim service As New AppService
        'Dim strsalt As String
        Dim userResult As UserInfo = service.GetUserSalt(txtUsername.Text)
        Dim strsalt As String = userResult.Salt
        Dim userPasswordType As String = userResult.Password_Type
        Dim password As String = String.Empty

        If userPasswordType = "" Then
            Exit Sub
        End If
        Dim username As String = txtUsername.Text
        If userPasswordType IsNot Nothing Then
            password = service.getHashPassword(txtPassword.Text, strsalt)
        Else
            password = txtPassword.Text
        End If

        Session("P1") = String.Empty 'Username
        Session("P2") = String.Empty ' Password 
        Session("P3") = String.Empty ' Date Time
        Session("P4") = String.Empty ' Password Type

        If AuthenticateUser(username, password) Then
            If userPasswordType IsNot Nothing Then
                Session("P1") = username
                Session("P2") = txtPassword.Text
                Session("P4") = userPasswordType ' Password Type
                lblResetSent.Text = "Login successful!"
                Response.Redirect("MainDashboard.aspx", False)
            Else
                lblResetSent.Text = "Invalid username or password. Please try again."
            End If

        Else
            lblResetSent.Text = "Invalid username or password. Please try again."
        End If


    End Sub
    Private Function AuthenticateUser(username As String, password As String) As Boolean
        Dim dbcon As New DBConnection

        Using connection As New SqlConnection(dbcon.getConnection_To_ozots())
            Using command As New SqlCommand("CheckUserLoginWithExpirationDate", connection)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.AddWithValue("@InputUsername", username)
                command.Parameters.AddWithValue("@InputPassword", password)
                connection.Open()
                Dim isLoginSuccessful As Boolean = CBool(command.ExecuteScalar())
                Return isLoginSuccessful
            End Using
        End Using
    End Function

End Class