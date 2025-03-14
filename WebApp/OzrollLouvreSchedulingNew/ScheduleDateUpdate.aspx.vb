﻿
Partial Class ScheduleDateUpdate
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

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
            service = Nothing

            Dim dt As DataTable = loadDatatable()
            Me.dgvScheduleList.DataSource = dt
            Me.dgvScheduleList.DataBind()

            popSummary(dt)

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

    Protected Function loadDatatable() As DataTable

        Dim service As New AppService
        Dim dtMain As DataTable = service.runSQLScheduling("Select tblProductionScheduleList.ID, tblProductionScheduleList.OrderReference, tblProductionScheduleList.CustomerName, " &
                                                        "tblProductionScheduleList.TotalPanels, tblProductionScheduleList.TotalSQM, tblProductionScheduleList.OnHold, tblProductionScheduleList.ScheduledDate, q1.*, " &
                                                        "tblProductionScheduleList.CustomeriD, tblCustomers.CustomerName As CustomerTitle, tblCustomers.SiteID, " &
                                                        "tblProductionScheduleList.OrderStatus, tblProductionScheduleList.PriorityLevel, tblProductionScheduleList.OrderDate " &
                                                        "From tblProductionScheduleList Cross Apply fn_getJobStagesForScheduleID(tblProductionScheduleList.ID) q1 " &
                                                        "Inner Join tblCustomers On tblProductionScheduleList.CustomeriD=tblCustomers.CustomeriD " &
                                                        "Where tblProductionScheduleList.OrderStatus in(3,6) and ProductTypeID=" & Me.txtProductTypeID.Text & " Order By tblCustomers.SiteID, CustomerTitle, OrderReference")


        '
        Dim col As New DataColumn
        col.ColumnName = "BranchName"
        col.DataType = System.Type.GetType("System.String")
        dtMain.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "StatusName"
        col.DataType = System.Type.GetType("System.String")
        dtMain.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Priority"
        col.DataType = System.Type.GetType("System.String")
        dtMain.Columns.Add(col)
        col = Nothing

        '---- for debugging add the missing field until it has been added to the backend table
        'col = New DataColumn
        'col.ColumnName = "ScheduleDate"
        'col.DataType = System.Type.GetType("System.DateTime")
        'col.DefaultValue = Today
        'dtMain.Columns.Add(col)
        'col = Nothing
        '----

        Dim dtStatus As DataTable = service.runSQLScheduling("select * from dbo.tblStatus")

        For i As Integer = 0 To dtMain.Rows.Count - 1
            dtMain.Rows(i)("BranchName") = dtMain.Rows(i)("CustomerTitle")

            Dim drows() As DataRow = dtStatus.Select("StatusID=" & dtMain.Rows(i)("OrderStatus").ToString)
            If drows.Length > 0 Then
                dtMain.Rows(i)("StatusName") = drows(0)("StatusName")
            Else
                dtMain.Rows(i)("StatusName") = String.Empty
            End If

            If Not IsDBNull(dtMain.Rows(i)("OnHold")) Then
                If CInt(dtMain.Rows(i)("OnHold")) = 1 Then
                    dtMain.Rows(i)("StatusName") = "On Hold - " & dtMain.Rows(i)("StatusName").ToString
                End If
            End If

            dtMain.Rows(i)("Priority") = String.Empty
            If Not IsDBNull(dtMain.Rows(i)("PriorityLevel")) Then
                If dtMain.Rows(i)("PriorityLevel").ToString = "1" Then
                    dtMain.Rows(i)("Priority") = "High"
                End If
            End If


            drows = Nothing
        Next

        service = Nothing
        Return dtMain

    End Function

    Protected Function GetDate(strDt As Object) As String

        Dim dt1 As DateTime
        If DateTime.TryParse(strDt.ToString(), dt1) Then
            Return dt1.ToString("d MMM yyyy")
        Else
            Return ""
        End If

    End Function


    Protected Sub txtScheduleDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.lblStatus.Text = String.Empty
        Dim gvr As GridViewRow = DirectCast(DirectCast(sender, Control).NamingContainer, GridViewRow)
        Dim intScheduleID As Integer = CInt(dgvScheduleList.DataKeys(gvr.RowIndex).Values("ScheduleID"))
        Dim txt As TextBox = DirectCast(sender, TextBox)
        Dim dteScheduleDate As Date = SharedConstants.DEFAULT_DATE_VALUE
        If txt.Text <> String.Empty Then
            If Not IsDate(txt.Text) Then
                txt.Text = String.Empty
            Else
                dteScheduleDate = CDate(txt.Text)
            End If
        End If
        '----
        'update scheduled date
        Dim service As New AppService
        '
        Dim clsProdSchedule As ProductionSchedule = service.getProdScheduleClsByID(intScheduleID)
        clsProdSchedule.ScheduledDate = dteScheduleDate

        If dteScheduleDate <> SharedConstants.DEFAULT_DATE_VALUE Then
            If clsProdSchedule.OrderStatus = 2 Then
                clsProdSchedule.OrderStatus = 3
            End If
        Else
            If clsProdSchedule.OrderStatus = 3 Then
                clsProdSchedule.OrderStatus = 2
            End If
        End If
        '
        Dim bolUpdateOK As Boolean = True

        Dim dbConn As New DBConnection
        Dim cnn As SqlClient.SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim trans As SqlClient.SqlTransaction = Nothing

        Try
            cnn.Open()
            trans = cnn.BeginTransaction

            bolUpdateOK = service.updateProductionScheduleByID(clsProdSchedule, cnn, trans)

            If bolUpdateOK Then
                trans.Commit()
            Else
                trans.Rollback()
            End If

        Catch ex As Exception
            If Not trans Is Nothing Then
                trans.Rollback()
            End If
            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If
            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message & Environment.NewLine & getPageInfo())
            bolUpdateOK = False
        Finally
            trans.Dispose()
            trans = Nothing
            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If
            cnn.Dispose()
            cnn = Nothing
        End Try

        If bolUpdateOK = False Then
            lblStatus.Text = "Error in saving 'Schedule Date'. Please try again."
        End If
        service = Nothing
        ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(String), "Script", "reloadPage();", True)

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Response.Redirect("Home.aspx", False)

    End Sub

    Protected Sub popSummary(ByVal dtJobList As DataTable)

        Dim arrIDcnt() As Integer = New Integer() {0, 0, 0, 0, 0, 0, 0}
        Dim arrTotalPnlCNT() As Integer = New Integer() {0, 0, 0, 0, 0, 0, 0}
        Dim arrTotlaSqMcnt() As Integer = New Integer() {0, 0, 0, 0, 0, 0, 0}
        Dim arrCalcDate() As Date = New Date() {Today, Today, Today, Today, Today, Today, Today}
        Dim dwJobLists() As DataRow = Nothing

        Me.lblSummary.Text = "<div style=""width: 100%; height: 80px; text-align: center;"">"
        Me.lblSummary.Text &= "<div style=""width: 100%; height: 70px; position: fixed;  text-align: center; top: 0px; z-index: 100; background-color: grey; color: White; font-weight: bold; padding-top: 10px;"">"
        Me.lblSummary.Text &= "<table border=""2px"" style=""width: 90%;"" cellspacing=""0"" summary="""" style=""text-align: center;"">"
        Me.lblSummary.Text &= "<tr>"
        Me.lblSummary.Text &= "<td style=""width: 9%;"">&nbsp;</td>"

        For i As Integer = 0 To 6
            arrIDcnt(i) = 0
            arrTotalPnlCNT(i) = 0
            arrTotlaSqMcnt(i) = 0
            arrCalcDate(i) = DateAdd(DateInterval.Day, i, Today)
            dwJobLists = dtJobList.Select("ScheduledDate='" & Format(arrCalcDate(i), "d/MMM/yyyy") & "'")
            If dwJobLists.Length > 0 Then
                For j As Integer = 0 To dwJobLists.Length - 1
                    arrIDcnt(i) += 1
                    If Not IsDBNull(dwJobLists(j)("TotalPanels")) Then
                        arrTotalPnlCNT(i) += CInt(dwJobLists(j)("TotalPanels"))
                    End If
                    If Not IsDBNull(dwJobLists(j)("TotalSQM")) Then
                        arrTotlaSqMcnt(i) += CDec(dwJobLists(j)("TotalSQM"))
                    End If
                Next
            End If
            Me.lblSummary.Text &= "<td style=""width: 13%;"">"
            Me.lblSummary.Text &= "<table cellspacing=""0"" summary="""">"
            Me.lblSummary.Text &= "<tr><td nowrap style=""font-size: 6;text-align: center;"">Date : " & Format(arrCalcDate(i), "d MMM yyyy") & "</td></tr>"
            Me.lblSummary.Text &= "<tr><td style=""text-align: center;"">Jobs : " & arrIDcnt(i) & "</td></tr>"
            Me.lblSummary.Text &= "<tr><td style=""text-align: center;"">Panels : " & arrTotalPnlCNT(i) & "</td></tr>"
            Me.lblSummary.Text &= "<tr><td style=""text-align: center;"">Total SqM : " & arrTotlaSqMcnt(i) & "</td></tr>"
            Me.lblSummary.Text &= "</table>"
            Me.lblSummary.Text &= "</td>"
        Next

        Me.lblSummary.Text &= "</tr>"
        Me.lblSummary.Text &= "</table>"
        Me.lblSummary.Text &= "</div></div>"

    End Sub

End Class
