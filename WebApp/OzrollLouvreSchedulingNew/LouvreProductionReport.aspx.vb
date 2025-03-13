Imports System.Data
Imports System.Net

Partial Class LouvreProductionReport
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Me.ClientScript.GetPostBackEventReference(Me, String.Empty)
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

            Dim service As New AppService
            service.addWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)

            Dim dtLouvreJobs As DataTable
            dtLouvreJobs = service.runSQLScheduling(generateSQLForReport)
            service = Nothing

            Me.lblStatus.Text = String.Empty
            '
            updateDatatable(dtLouvreJobs)

            'dgvScheduleList.Columns(0).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(1).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(2).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(3).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(4).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(5).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(6).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(7).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(8).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(9).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(10).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(11).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(12).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(13).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(14).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(15).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(16).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(17).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(18).ItemStyle.VerticalAlign = VerticalAlign.Middle
            'dgvScheduleList.Columns(19).ItemStyle.VerticalAlign = VerticalAlign.Middle


            Me.dgvScheduleList.DataSource = dtLouvreJobs
            Me.dgvScheduleList.DataBind()

        End If

    End Sub

    Protected Function updateDatatable(dtLouvreJobs As DataTable, Optional bolReport As Boolean = False) As DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "PanelWk1"
        col.DataType = System.Type.GetType("System.Int32")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "PanelWk2"
        col.DataType = System.Type.GetType("System.Int32")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "PanelWk3"
        col.DataType = System.Type.GetType("System.Int32")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "PanelWk4"
        col.DataType = System.Type.GetType("System.Int32")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "PanelWk5"
        col.DataType = System.Type.GetType("System.Int32")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "PanelWk6"
        col.DataType = System.Type.GetType("System.Int32")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "PanelWk7"
        col.DataType = System.Type.GetType("System.Int32")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "SQMWk1"
        col.DataType = System.Type.GetType("System.String")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "SQMWk2"
        col.DataType = System.Type.GetType("System.String")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "SQMWk3"
        col.DataType = System.Type.GetType("System.String")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "SQMWk4"
        col.DataType = System.Type.GetType("System.String")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "SQMWk5"
        col.DataType = System.Type.GetType("System.String")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "SQMWk6"
        col.DataType = System.Type.GetType("System.String")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "SQMWk7"
        col.DataType = System.Type.GetType("System.String")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "JobDisplay"
        col.DataType = System.Type.GetType("System.String")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "PowdercoatStartDate"
        col.DataType = System.Type.GetType("System.DateTime")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "PowdercoatEndDate"
        col.DataType = System.Type.GetType("System.DateTime")
        dtLouvreJobs.Columns.Add(col)
        col = Nothing

        'get current weekid
        Dim intWeekID As Integer = 0
        Dim service As New AppService
        Dim dtWeek As DataTable = service.runSQLScheduling("select * from dbo.loctblDays where  (DayDate = CONVERT(DATETIME, '" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00', 102))")
        If dtWeek.Rows.Count > 0 Then
            intWeekID = CInt(dtWeek.Rows(0)("WeekID"))
        End If
        dtWeek.Dispose()
        dtWeek = Nothing

        Dim dtWeekMonths As DataTable = service.runSQLScheduling("SELECT dbo.loctblWeeks.WeekID, dbo.loctblWeeks.WeekNumber, dbo.loctblMonths.MonthAbb FROM dbo.loctblWeeks INNER JOIN dbo.loctblMonths ON dbo.loctblWeeks.MonthID = dbo.loctblMonths.MonthID where dbo.loctblWeeks.WeekID >= " & intWeekID.ToString)

        Dim drows() As DataRow = dtWeekMonths.Select("WeekID=" & intWeekID)
        If drows.Length > 0 Then
            Me.dgvScheduleList.Columns(4).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
            Me.dgvScheduleList.Columns(11).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
        End If
        drows = dtWeekMonths.Select("WeekID=" & intWeekID + 1)
        If drows.Length > 0 Then
            Me.dgvScheduleList.Columns(5).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
            Me.dgvScheduleList.Columns(12).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
        End If
        drows = dtWeekMonths.Select("WeekID=" & intWeekID + 2)
        If drows.Length > 0 Then
            Me.dgvScheduleList.Columns(6).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
            Me.dgvScheduleList.Columns(13).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
        End If
        drows = dtWeekMonths.Select("WeekID=" & intWeekID + 3)
        If drows.Length > 0 Then
            Me.dgvScheduleList.Columns(7).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
            Me.dgvScheduleList.Columns(14).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
        End If
        drows = dtWeekMonths.Select("WeekID=" & intWeekID + 4)
        If drows.Length > 0 Then
            Me.dgvScheduleList.Columns(8).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
            Me.dgvScheduleList.Columns(15).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
        End If
        drows = dtWeekMonths.Select("WeekID=" & intWeekID + 5)
        If drows.Length > 0 Then
            Me.dgvScheduleList.Columns(9).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
            Me.dgvScheduleList.Columns(16).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
        End If
        drows = dtWeekMonths.Select("WeekID=" & intWeekID + 6)
        If drows.Length > 0 Then
            Me.dgvScheduleList.Columns(10).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
            Me.dgvScheduleList.Columns(17).HeaderText = drows(0)("MonthAbb").ToString & "<br /> Wk" & drows(0)("WeekNumber").ToString
        End If


        For i As Integer = 0 To dtLouvreJobs.Rows.Count - 1

            Select Case CInt(dtLouvreJobs.Rows(i)("WeekID"))
                Case Is <= intWeekID
                    dtLouvreJobs.Rows(i)("PanelWk1") = dtLouvreJobs.Rows(i)("TotalPanels")
                    dtLouvreJobs.Rows(i)("SQMWk1") = dtLouvreJobs.Rows(i)("TotalSQM")


                Case intWeekID + 1
                    dtLouvreJobs.Rows(i)("PanelWk2") = dtLouvreJobs.Rows(i)("TotalPanels")
                    dtLouvreJobs.Rows(i)("SQMWk2") = dtLouvreJobs.Rows(i)("TotalSQM")

                    '4,11

                Case intWeekID + 2
                    dtLouvreJobs.Rows(i)("PanelWk3") = dtLouvreJobs.Rows(i)("TotalPanels")
                    dtLouvreJobs.Rows(i)("SQMWk3") = dtLouvreJobs.Rows(i)("TotalSQM")

                    '5,12

                Case intWeekID + 3
                    dtLouvreJobs.Rows(i)("PanelWk4") = dtLouvreJobs.Rows(i)("TotalPanels")
                    dtLouvreJobs.Rows(i)("SQMWk4") = dtLouvreJobs.Rows(i)("TotalSQM")

                    '6,13

                Case intWeekID + 4
                    dtLouvreJobs.Rows(i)("PanelWk5") = dtLouvreJobs.Rows(i)("TotalPanels")
                    dtLouvreJobs.Rows(i)("SQMWk5") = dtLouvreJobs.Rows(i)("TotalSQM")

                    '7,14

                Case intWeekID + 5
                    dtLouvreJobs.Rows(i)("PanelWk6") = dtLouvreJobs.Rows(i)("TotalPanels")
                    dtLouvreJobs.Rows(i)("SQMWk6") = dtLouvreJobs.Rows(i)("TotalSQM")

                    '8,15

                Case >= intWeekID + 6
                    dtLouvreJobs.Rows(i)("PanelWk7") = dtLouvreJobs.Rows(i)("TotalPanels")
                    dtLouvreJobs.Rows(i)("SQMWk7") = dtLouvreJobs.Rows(i)("TotalSQM")

                    '9,16

            End Select

            'dtLouvreJobs.Rows(i)("Job") = dtLouvreJobs.Rows(i)("Job") & "" & dtLouvreJobs.Rows(i)("Colour")
            'dtLouvreJobs.Rows(i)("JobDisplay") = dtLouvreJobs.Rows(i)("Job") & "<br />" & dtLouvreJobs.Rows(i)("Colour")
            If bolReport Then
                dtLouvreJobs.Rows(i)("JobDisplay") = dtLouvreJobs.Rows(i)("ShutterProNumber").ToString & " - " & dtLouvreJobs.Rows(i)("OrderReference").ToString & " - " & dtLouvreJobs.Rows(i)("CustomerName").ToString
            Else
                dtLouvreJobs.Rows(i)("JobDisplay") = dtLouvreJobs.Rows(i)("ShutterProNumber").ToString & " - " & dtLouvreJobs.Rows(i)("OrderReference").ToString & " - " & dtLouvreJobs.Rows(i)("CustomerName").ToString & "<br />" & dtLouvreJobs.Rows(i)("ColourName").ToString
            End If


            'get powdercoat start/end if available
            Dim dtPowdercoat As DataTable = service.getAdditionalRequirementsByProductionScheduleID(CInt(dtLouvreJobs.Rows(i)("ID")))
            If dtPowdercoat.Rows.Count > 0 Then
                Dim dtTemp As DataTable = SharedFunctions.performDatatableSelect(dtPowdercoat, "AdditionalRequirementTypeID=1", "AdditionalRequirementsID DESC")
                If dtTemp.Rows.Count > 0 Then
                    dtLouvreJobs.Rows(i)("PowdercoatStartDate") = dtTemp.Rows(0)("StartDate")
                    dtLouvreJobs.Rows(i)("PowdercoatEndDate") = dtTemp.Rows(0)("CompleteDate")
                End If
                dtTemp.Dispose()
                dtTemp = Nothing
            End If
            dtPowdercoat.Dispose()
            dtPowdercoat = Nothing

        Next



        Return dtLouvreJobs

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


    Protected Sub btnHome_Click(sender As Object, e As System.EventArgs) Handles btnHome.Click

        Response.Redirect("Home.aspx", False)

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim service As New AppService
        Dim dtLouvreJobs As DataTable = service.runSQLScheduling(generateSQLForReport)
        dtLouvreJobs = updateDatatable(dtLouvreJobs)
        service = Nothing

        Dim strRptsPath As String = Server.MapPath("") & "\ExcelRpts"
        Dim objBuffer As Byte() = Nothing
        'ExcelReport.createReport(strRptsPath, objBuffer, dtScheduleList)
        Dim rptFileName As String = LouvreJobsReport.createReport(strRptsPath, objBuffer, dtLouvreJobs)
        Dim strRPTFileName As String = IO.Path.Combine(strRptsPath, rptFileName)
        If objBuffer IsNot Nothing Then
            '----
            Response.BinaryWrite(objBuffer)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;  filename=" & rptFileName)
            'Response.End()
            '
        End If
        objBuffer = Nothing

    End Sub

    Protected Sub dgvScheduleList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvScheduleList.RowCommand

        If (e.CommandName = "LouvreDetail") Then
            Dim currentRowIndex As Integer = Int32.Parse(e.CommandArgument.ToString())
            Dim intProductionScheduleID As String = CInt(dgvScheduleList.DataKeys(currentRowIndex).Values("ID"))
            If SharedConstants.LIVE_SITE Then
                Response.Redirect("LouvreJobDetails.aspx?ScheduleID=" & intProductionScheduleID.ToString & "&ViewType=1", False)
            Else
                Response.Redirect("LouvreJobDetails.aspx?ScheduleID=" & intProductionScheduleID.ToString & "&ViewType=1", False)
            End If

        End If


    End Sub

    Protected Function generateSQLForReport() As String

        Dim strSQL As String = "Select dbo.tblProductionScheduleList.ID, dbo.tblProductionScheduleList.ShutterProNumber, dbo.tblProductionScheduleList.OrderReference, dbo.tblProductionScheduleList.CustomerName, dbo.tblJobTypes.JobTypeName as TypeName, " &
                                "dbo.tblLouvreStyles.StyleName, loctblDays.WeekId, dbo.tblStatus.StatusName, " &
                                "dbo.tblColours.ColourName, dbo.tblProductionScheduleList.OrderDate, dbo.tblProductionScheduleList.ScheduledDate, dbo.tblProductionScheduleList.ExpectedShippingDate as DateRequired, dbo.tblProductionScheduleList.TotalPanels, " &
                                "q1.*, dbo.tblProductionScheduleList.TotalSQM, '' as ProdNotes , dbo.tblProductionScheduleList.PickingDate " &
                                "From dbo.tblProductionScheduleList " &
                                "left outer join dbo.tblLouvreSpecs on dbo.tblProductionScheduleList.ID = dbo.tblLouvreSpecs.ProductScheduleID " &
                                "Left outer Join dbo.tblStatus On dbo.tblProductionScheduleList.OrderStatus=dbo.tblStatus.StatusID " &
                                "Left outer Join dbo.tblLouvreStyles On dbo.tblLouvreSpecs.StyleID=dbo.tblLouvreStyles.StyleID " &
                                "Left outer Join dbo.tblColours On dbo.tblLouvreSpecs.ColourID=dbo.tblColours.ColourID " &
                                "Left outer Join dbo.tblJobTypes On dbo.tblLouvreSpecs.LouvreJobTypeID=dbo.tblJobTypes.JobTypeID " &
                                "Left outer Join loctblDays ON dbo.tblProductionScheduleList.ScheduledDate = loctblDays.DayDate " &
                                "Cross Apply fn_getJobStagesForScheduleID(dbo.tblProductionScheduleList.ID) q1 " &
                                "where dbo.tblProductionScheduleList.ProductTypeID = 2 and (dbo.tblProductionScheduleList.OrderStatus = 3) " &
                                "Order By loctblDays.WeekId,dbo.tblStatus.StatusName,dbo.tblProductionScheduleList.OrderReference"

        Return strSQL

        '"Where not dbo.tblProductionScheduleList.OrderStatus In(5,7) And dbo.tblProductionScheduleList.ScheduledDate is not null and dbo.tblProductionScheduleList.CompleteDate is null And dbo.tblProductionScheduleList.ProductTypeID = 2 " &

    End Function


End Class
