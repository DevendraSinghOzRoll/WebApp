Imports System.Data
Imports System.Linq
Imports Microsoft.VisualBasic

Partial Class ozroll_Dashboard
    Inherits System.Web.UI.Page

    'Dim service As New AppService

    'Dim _Service As New AppService

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


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


        Return strPageInfo
    End Function



    'Private Function IsNewAccount(cLogin As LoginDetails) As Boolean
    '    If SharedFunctions.Decrypt(cLogin.Password) = String.Empty Then
    '        Return True
    '    End If

    '    Return False
    'End Function




    Protected Sub btnSlideTrack_Click(sender As Object, e As EventArgs) 'Handles btnSlideTrack.Click
        'Commented by Michael Behar - 24-03-2022 - Replaced with sessSlideTrackCustomerID
        'Session("SlideTrackCustomerID") = 0
        'Dim service As New AppService
        ''added by skm  dt:07/11/2022
        If Session("sessSlideTrackCustomerID") IsNot Nothing Then

            If Session("ExternID") IsNot Nothing Then
                Dim DummyVar As String = ""
                If CInt(Session("ExternID")) = -1 Then
                    'Changed by Michael Behar - 24-03-2022 - Replaced SlideTrackCustomerID session to below.
                    'Session("sessSlideTrackCustomerID") = Constants.SLIDETRACK_OZROLL_CUSTOMER_ID
                    DummyVar = Request.QueryString("var1")
                    Session("RedirectPageName") = "SlideTrack"
                    Response.Redirect("SlideTrackHome.aspx?var1=" + DummyVar, False)
                Else
                    Dim dtCustomer As DataTable = New DataTable()
                    Dim strSqlCheckSybiz As String = String.Empty
                    strSqlCheckSybiz = "select customer_number,SybizCustomerCode,SybizCustomerID from tblCustomerDetails "
                    strSqlCheckSybiz &= "where SybizCustomerID='" & Convert.ToInt32(Session("ExternID")) & "' "

                    'Commented by Michael Behar - 24-03-2022 - Needs To Use Smartbase
                    'dtCustomer = service.runSQLScheduling(strSqlCheckSybiz)     ' from smartbase datatabse 
                    'dtCustomer = service.returnSmartbaseSQL(strSqlCheckSybiz)     ' from smartbase datatabse 

                    If dtCustomer.Rows.Count > 0 Then
                        'Changed by Michael Behar - 24-03-2022 - Replaced SlideTrackCustomerID session to below.
                        Session("sessSlideTrackCustomerID") = CInt(dtCustomer.Rows.Item(0)("customer_number"))
                        'Dim encString As String = HttpUtility.UrlEncode(SharedFunctions.Encrypt("dummyvar=0,Status=0,customerid=" + CStr(Session("sessCustomerID"))))
                        'Response.Redirect("AddSlideTrackOrder.aspx?var1=" + encString, True
                        Session("RedirectPageName") = "SlideTrack"
                        DummyVar = Request.QueryString("var1")
                        Response.Redirect("SlideTrackHome.aspx?var1=" + DummyVar, False)
                    Else
                        'lblStatus.Text = "Something went wrong.Contact Ozroll IT department."
                    End If
                End If
            End If
        Else
            'lblStatus.Text = "Something went wrong. Please contact Ozroll Accounts."
        End If
    End Sub

    Protected Sub btnRollShield_Click(sender As Object, e As EventArgs) 'Handles btnRollShield.Click
        Dim DummyVar As String = ""
        DummyVar = Request.QueryString("var1")
        Session("RedirectPageName") = "RollaShield"
        Response.Redirect("shuttershome.aspx?var1=" + DummyVar, False)
    End Sub

    Protected Sub btnLogOut_Click(sender As Object, e As EventArgs) 'Handles btnLogOut.Click
        Response.Redirect("Logout.aspx", False)
    End Sub
End Class
