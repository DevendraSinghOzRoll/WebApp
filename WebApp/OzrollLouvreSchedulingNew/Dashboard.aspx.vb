
Imports System.IO

Partial Class Dashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Convert.ToInt32(Session("IsAdmin")) = 0 Then
                lnkAdministration.Enabled = False
                dvAdmin.Style.Add("opacity", "0.4")
            End If
            If Convert.ToInt32(Session("CustomerId")) = 0 Then
                Response.Redirect("LogOut.aspx", False)
            End If
            ''added by surendra 14/05/2021
            'lblUserName.Text = CStr(Session("CustomerName"))

            'changed by Fritz to match plantation login #65783
            lblUserName.Text = CStr(Session("sessTradeName"))
        End If

    End Sub

    Protected Sub btnLogOut_Click(sender As Object, e As EventArgs) Handles btnLogOut.Click
        Session.Abandon()
        Response.Redirect("Login.aspx", False)
    End Sub

    Protected Sub lnkAdministration_Click(sender As Object, e As EventArgs) Handles lnkAdministration.Click
        Response.Redirect("CreateLogin_shutter.aspx", False)
    End Sub

    Protected Sub lnkPriceMatrix_Click(sender As Object, e As EventArgs) Handles lnkPriceMatrix.Click
        'Dim appServices As New AppService
        'Dim dt As New DataTable
        'Dim filePath As String = Server.MapPath("~/PriceMatrix/")
        'Dim customerType As Integer
        'Dim fileName As String = ""
        'dt = appServices.RunSQLScheduling("select LouvreCategoryID from tblCustomers where customerid=" & Convert.ToInt32(Session("CustomerId")) & "")
        'If dt.Rows.Count > 0 Then
        '    customerType = Convert.ToInt32(dt.Rows(0)(0))
        '    If customerType = 1 Then
        '        fileName = "ELIPSOGridPricing-201808Gold.xlsx"
        '    ElseIf customerType = 2 Then
        '        fileName = "ELIPSOGridPricing-201808-Silver.xlsx"
        '    ElseIf customerType = 3 Then
        '        fileName = "ELIPSOGridPricing-201808-Trade.xlsx"
        '    End If
        '    Response.ContentType = ContentType
        '    Response.AppendHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(Path.Combine(filePath, fileName))))
        '    Response.WriteFile(Path.Combine(filePath, fileName))
        '    Response.End()
        'End If
    End Sub
End Class
