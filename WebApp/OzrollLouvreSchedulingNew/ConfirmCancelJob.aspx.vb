﻿
Partial Class ConfirmCancelJob
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

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

            Me.txtProductTypeID.Text = Session("sessProductTypeID").ToString

            Dim service As New AppService

            service.addWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)

            Dim intScheduleId As Integer = 0

            If Not Request.QueryString.Count = 0 Then
                If Not IsNothing(Request.Params("ScheduleId")) Then
                    intScheduleId = CInt(Request.Params("ScheduleId"))
                End If
            End If

            Me.txtId.Text = intScheduleId.ToString


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

    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click

        Response.Redirect("Logout.aspx", False)

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Dim service As New AppService
        Dim bolContinue As Boolean = True

        Dim dbConn As New DBConnection
        Dim cnn As SqlClient.SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim trans As SqlClient.SqlTransaction = Nothing
        dbConn = Nothing
        btnSave.Enabled = False
        btnSave.CssClass = "form-button-disabled"
        lblStatus.Text = String.Empty

        Try

            cnn.Open()
            trans = cnn.BeginTransaction

            Dim intScheduleId As Integer = CInt(Me.txtId.Text)
            Dim clsProdSchedule As ProductionSchedule

            Dim intMaxChangeId As Integer = service.getProductScheduleHistoryNewChgID()
            clsProdSchedule = service.getProdScheduleClsByID(intScheduleId)

            'Dim clsNewProdSchedule As ProductionSchedule = CType(clsProdSchedule.Clone, ProductionSchedule)
            Dim clsNewProdSchedule As ProductionSchedule = CType(clsProdSchedule.Clone, ProductionSchedule)

            'update completed date field
            clsNewProdSchedule.CompletedDate = DateTime.Now
            clsNewProdSchedule.OrderStatus = 8

            If bolContinue Then
                bolContinue = service.updateProductionScheduleByID(clsNewProdSchedule, cnn, trans)
            End If

            If bolContinue Then
                '
                bolContinue = service.addProdScheduleHistoryRcd(intMaxChangeId, clsProdSchedule, cnn, trans)
                '
            End If

            If bolContinue Then
                'add note
                Dim cNote As New ProdScheduleNote
                cNote.ProdScheduleID = clsNewProdSchedule.ID
                cNote.NoteDetails = "Reason for cancellation: " & Me.txtCancelNote.Text
                cNote.NoteTypeID = 1
                cNote.UserID = CInt(Session("sessUserID"))
                cNote.EntryDate = DateTime.Now
                cNote.VisibleToCustomer = 0
                bolContinue = service.addProdScheduleNoteRcd(cNote, cnn, trans)
            End If


            If bolContinue Then
                trans.Commit()

            Else
                'EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - Transaction Rolled Back" & Environment.NewLine & getPageInfo())
                trans.Rollback()
                If lblStatus.Text = String.Empty Then
                    lblStatus.Text = "Error saving details. Please try again."
                End If
                btnSave.Enabled = True
                btnSave.CssClass = "form-button"
            End If

        Catch ex As Exception
            If Not trans Is Nothing Then
                trans.Rollback()
            End If
            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If
            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message & Environment.NewLine & getPageInfo())
            bolContinue = False
        Finally
            trans.Dispose()
            trans = Nothing
            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If
            cnn.Dispose()
            cnn = Nothing
        End Try
        dbConn = Nothing
        service = Nothing



        If bolContinue Then
            Response.Redirect("ProductionScheduleList.aspx?" & Request.QueryString.ToString, False)
        End If

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        Response.Redirect("ProductionScheduleList.aspx?" & Request.QueryString.ToString, False)

    End Sub
End Class
