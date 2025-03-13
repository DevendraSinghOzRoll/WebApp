
Imports System.Globalization

Partial Class Dispatch
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub chkLocal_CheckedChanged(sender As Object, e As EventArgs) Handles chkLocal.CheckedChanged
        If chkLocal.Checked Then
            lblCheckLocal.Text = "Outstation Dispatch"
        Else
            lblCheckLocal.Text = "Local Dispatch"
        End If
    End Sub
    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim dateVal As DateTime
        lblStatus.Text = ""
        If Not IsDate(Me.txtDelDate.Text) Then
            lblStatus.Text = "Could Not save with invalid date!!Please try again"
        Else
            If Date.TryParseExact(Me.txtDelDate.Text, "dd MMM yyyy", System.Globalization.CultureInfo.CurrentCulture,
           DateTimeStyles.None, dateVal) Then

                lblStatus.Text = "  Date Format  is  OK"
            Else
                lblStatus.Text = " Incorrect Date Format"
            End If
        End If
    End Sub
End Class
