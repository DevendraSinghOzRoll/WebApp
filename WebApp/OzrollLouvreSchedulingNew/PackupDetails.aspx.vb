﻿
Partial Class PackupDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        'If Not IsPostBack Then

        '    If Session("sessUserID") = String.Empty Then
        '        Response.Redirect("Logout.aspx", False)
        '        Exit Sub
        '    Else
        '        If Not IsNumeric(Session("sessUserID")) Then
        '            Response.Redirect("Logout.aspx", False)
        '            Exit Sub
        '        End If
        '    End If

        '    Me.txtProductTypeID.Text = Session("sessProductTypeID").ToString

        '    Dim service As New AppService
        '    service.addWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)
        '    service = Nothing

        Dim ID As Integer = 1
        txtPackupID.Text = CStr(ID)
        loadPackupDetails(ID)

        'End If

    End Sub

    Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        'Dim objErr1 As Exception = Server.GetLastError().GetBaseException()

        'Dim strErrorMessage As String = String.Empty

        'strErrorMessage &= "Error In: " & Request.Url.ToString() & Environment.NewLine & Environment.NewLine

        'strErrorMessage &= "Server.GetLastError().GetBaseException()" & Environment.NewLine & Environment.NewLine

        'strErrorMessage &= "Error Message: " & objErr1.Message & Environment.NewLine
        'strErrorMessage &= "Stack Trace:" & Environment.NewLine
        'strErrorMessage &= objErr1.StackTrace & Environment.NewLine & Environment.NewLine

        'Dim objErr2 As Exception = Server.GetLastError()

        'strErrorMessage &= "Server.GetLastError()" & Environment.NewLine & Environment.NewLine

        'strErrorMessage &= "Error Message: " & objErr2.Message & Environment.NewLine
        'strErrorMessage &= "Stack Trace:" & Environment.NewLine
        'strErrorMessage &= objErr2.StackTrace & Environment.NewLine

        'EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & strErrorMessage)

        'Server.ClearError()

        'Response.Redirect("GenericErrorPage.aspx", False)
    End Sub

    Private Function getPageInfo() As String

        'Dim strPageInfo As String = String.Empty
        'Dim strName As String = String.Empty
        'If Session.Contents.Count > 0 Then
        '    strPageInfo &= "Session Variables" & Environment.NewLine
        '    For Each strName In Session.Contents
        '        strPageInfo &= strName & ": " & CStr(Session.Contents(strName)) & Environment.NewLine
        '    Next
        'Else
        '    strPageInfo &= "No Session Variables" & Environment.NewLine
        'End If

        'strPageInfo &= Environment.NewLine
        'If Me.HasControls Then
        '    strPageInfo &= "Form Controls" & Environment.NewLine
        '    getPageControls(Me, strPageInfo)
        'Else
        '    strPageInfo &= "No Form Controls" & Environment.NewLine
        'End If
        'Return strPageInfo

        Return String.Empty
    End Function

    Private Sub getPageControls(ByVal ctrl As Control, ByRef strPageControls As String)

        'If ctrl.HasControls Then
        '    For Each childCtrl As Control In ctrl.Controls
        '        getPageControls(childCtrl, strPageControls)
        '    Next
        'Else
        '    Select Case ctrl.GetType.Name
        '        Case "TextBox"
        '            Dim frmTxt As TextBox
        '            frmTxt = DirectCast(ctrl, TextBox)
        '            strPageControls &= frmTxt.ID & ": " & Left(frmTxt.Text, 100) & Environment.NewLine
        '        Case "DropDownList"
        '            Dim frmCbo As DropDownList
        '            frmCbo = DirectCast(ctrl, DropDownList)
        '            If frmCbo.Items.Count > 0 Then
        '                strPageControls &= frmCbo.ID & ": " & frmCbo.SelectedItem.Text & " (" & frmCbo.SelectedValue & ")" & Environment.NewLine
        '            Else
        '                strPageControls &= frmCbo.ID & ": Not Populated" & Environment.NewLine
        '            End If
        '        Case "CheckBox"
        '            Dim frmChk As CheckBox
        '            frmChk = DirectCast(ctrl, CheckBox)
        '            strPageControls &= frmChk.ID & ": " & frmChk.Checked & Environment.NewLine
        '        Case "RadioButton"
        '            Dim frmRdo As RadioButton
        '            frmRdo = DirectCast(ctrl, RadioButton)
        '            strPageControls &= frmRdo.ID & ": " & frmRdo.Checked & Environment.NewLine
        '        Case "RadioButtonList"
        '            Dim frmRdoLst As RadioButtonList
        '            frmRdoLst = DirectCast(ctrl, RadioButtonList)
        '            If frmRdoLst.SelectedIndex >= 0 Then
        '                strPageControls &= frmRdoLst.ID & ": " & frmRdoLst.SelectedItem.Text & " (" & frmRdoLst.SelectedValue & ")" & Environment.NewLine
        '            Else
        '                strPageControls &= frmRdoLst.ID & ": Not Selected" & Environment.NewLine
        '            End If
        '    End Select
        'End If

    End Sub

    Private Sub loadPackupDetails(ID As Integer)

        'Dim service As New AppService
        'Dim bolContinue As Boolean = True

        'Dim dbConn As New DBConnection
        'Dim cnn As SqlClient.SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling

        'Dim strSQL As String = "Select * from tblPackUp where ID=" & ID
        'Dim dt As DataTable = service.runSQLScheduling(strSQL)
        '       dt = service.getPlantationJobDetailsRecordsByPlantationScheduleID(231)
        'dbConn = Nothing

        'If dt.Rows.Count() > 0 Then
        '    lblPackupID.Text = dt.Rows(0).Item("ID")
        '    lblPackupStatus.Text = dt.Rows(0).Item("Status")
        '    lblPackupHeight.Text = dt.Rows(0).Item("Height")
        '    lblPackupWidth.Text = dt.Rows(0).Item("Width")
        'End If
        'dt = Nothing

        lblPackupID.Text = CStr(ID)
        lblPackupStatus.Text = "Completed"
        lblPackupHeight.Text = "1900mm"
        lblPackupWidth.Text = "1200mm"

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If txtPackupDate.Text = String.Empty Then
            lblStatus.Text = "Packup Date Cannot Be Empty"
            Exit Sub
        End If

        'Dim service As New AppService
        'Dim strSQL As String

        'strSQL = "select * from tblPackupDate where ID='" + txtPackupID.Text + "'"
        'Dim dtPackupDetails As DataTable = service.runSQLScheduling(strSQL)

        'Dim prodLead As New ProductionLeadDays

        'prodLead.LeadTimesID = txtTempID.Text
        'prodLead.ProductTypeID = dtProdLead.Rows(0).Item("ProductTypeID").ToString()
        'prodLead.EffectiveDate = dtProdLead.Rows(0).Item("EffectiveDate").ToString()
        'prodLead.StandardDays = txtStandardDays.Text
        'prodLead.PowdercoatDays = txtPowdercoatDays.Text

        'If service.updateProductionLeadDays(prodLead) = True Then
        '    lblStatus.Text = "Update Successful"
        'End If

    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("LouvreJobDetails.aspx", False)
    End Sub
    Protected Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        Response.Redirect("Home.aspx", False)
    End Sub
    Protected Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Response.Redirect("Logout.aspx", False)
    End Sub
End Class
