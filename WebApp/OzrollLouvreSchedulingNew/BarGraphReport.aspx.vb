﻿Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Web.UI.DataVisualization.Charting

Partial Class BarGraphReport
    Inherits System.Web.UI.Page
    Public dteDate As Date
    Public intWeekID As Integer = 0
    Public intMonthID As Integer = 0


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'If Session("sessUserID") = String.Empty Then
            '    Response.Redirect("Logout.aspx", False)
            '    Exit Sub
            'Else
            '    If Not IsNumeric(Session("sessUserID")) Then
            '        Response.Redirect("Logout.aspx", False)
            '        Exit Sub
            '    End If
            'End If

            Dim service As New AppService
            Dim dtDate As DataTable = service.runSQLScheduling("SELECT GETDATE() AS LocalDate")
            If dtDate.Rows.Count > 0 Then
                dteDate = dtDate.Rows(0)(0)
            Else
                dteDate = DateTime.Now
            End If
            dtDate.Dispose()

            'this gets the weekid and monthid for the current date
            Dim strSQL As String = "Select loctblDays.DayDate, loctblDays.WeekID, loctblWeeks.MonthID from loctblDays Inner Join loctblWeeks ON loctblDays.WeekID = loctblWeeks.WeekID Where loctblDays.DayDate = '" & Format(dteDate, "yyyy-MM-dd") & "'"
            dtDate = service.runSQLScheduling(strSQL)
            If dtDate.Rows.Count > 0 Then
                intWeekID = CInt(dtDate.Rows(0).Item("WeekID").ToString)
                intMonthID = CInt(dtDate.Rows(0).Item("MonthID").ToString)
            End If

            'get earliest date for each date range
            Dim dte2Weeks As Date
            Dim dte3MonthsStart As Date
            Dim dte12MonthsStart As Date
            Dim intWeek3Months As Integer
            Dim int12Months As Integer

            dte2Weeks = DateAdd(DateInterval.Day, -13, dteDate)

            intWeek3Months = intWeekID - 12
            strSQL = "Select min(loctblDays.DayDate) as daydate "
            strSQL += "From loctblDays "
            strSQL += "Where (loctblDays.WeekID = " & intWeek3Months & ")"
            dtDate = service.runSQLScheduling(strSQL)
            If dtDate.Rows.Count > 0 Then
                dte3MonthsStart = CDate(dtDate.Rows(0)(0))
            End If
            dtDate.Dispose()
            dtDate = Nothing

            int12Months = intMonthID - 11
            strSQL = "Select min(loctblDays.DayDate) as daydate "
            strSQL += "from loctblDays  inner join dbo.loctblWeeks on loctblDays.WeekID = loctblWeeks.WeekID "
            strSQL += "where (loctblWeeks.MonthID = " & int12Months & ")"
            dtDate = service.runSQLScheduling(strSQL)
            If dtDate.Rows.Count > 0 Then
                dte12MonthsStart = CDate(dtDate.Rows(0)(0))
            End If
            dtDate.Dispose()
            dtDate = Nothing

            cboType.SelectedIndex = 0
            'cboType_SelectedIndexChanged(sender, Nothing)
            txtCurrDate.Text = dteDate.ToString("dd/MM/yyyy")
            txtWeekID.Text = intWeekID
            txtMonthID.Text = intMonthID

            Me.txtDate2Weeks.Text = dte2Weeks
            Me.txtDate3Months.Text = dte3MonthsStart
            Me.txtDate12Months.Text = dte12MonthsStart
            Me.txtWeek3Months.Text = intWeek3Months
            Me.txtMonth12Months.Text = int12Months
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
    Protected Sub btnHome_Click(sender As Object, e As System.EventArgs) Handles btnHome.Click
        Response.Redirect("Home.aspx", False)
    End Sub

    Protected Sub btnLast2_Click(sender As Object, e As EventArgs) Handles btnLast2.Click

        If CInt(Me.cboProduct.SelectedValue) = 3 Then
            If CInt(Me.cboType.SelectedValue) = 1 Then
                'customers selection
                displayShuttersChart(1, CInt(Me.cboReport.SelectedValue), CInt(Me.cboSubType.SelectedValue))
            Else
                'not customers selection
                displayShuttersChart(1, CInt(Me.cboReport.SelectedValue), -1)
            End If
            Exit Sub
        End If

        Dim strSql As String
        Dim strProductType As String = cboProduct.SelectedIndex
        Dim service As New AppService
        Dim dtDate As DataTable = service.runSQLScheduling("SELECT GETDATE() AS LocalDate")
        If dtDate.Rows.Count > 0 Then
            dteDate = dtDate.Rows(0)(0)
        Else
            dteDate = DateTime.Now
        End If
        dtDate.Dispose()
        dteDate = Date.ParseExact(txtCurrDate.Text, "dd/MM/yyyy",
            System.Globalization.DateTimeFormatInfo.InvariantInfo)
        intMonthID = txtMonthID.Text
        intWeekID = txtWeekID.Text

        lblChartHeader.Text = "Last 2 Week Presentation"

        Dim dte2Weeks As Date = DateAdd(DateInterval.Day, -13, dteDate)
        strSql = "Select Convert(Date, OrderDate) As neworderdate, sum(TotalPanels) As TotalPanelNos, sum(CostPrice) as TotalCost from tblProductionScheduleList "
        strSql += " Where ProductTypeID = " + strProductType + " and OrderDate >= '" & Format(dte2Weeks, "yyyy-MM-dd")
        strSql += "' group by CAST(OrderDate AS DATE) order by CAST(OrderDate AS DATE) desc"
        If cboReport.SelectedIndex > 1 Then
            strSql = "Select Convert(Date, q1.DespatchDate) As neworderdate, sum(TotalPanels) As TotalPanelNos, sum(CostPrice) as TotalCost from tblProductionScheduleList "
            strSql += " Cross Apply fn_getJobStagesForScheduleID(dbo.tblProductionScheduleList.ID) q1 "
            strSql += " Where ProductTypeID = " + strProductType + " and q1.DespatchDate >= '" & Format(dte2Weeks, "yyyy-MM-dd")
            strSql += "' group by CAST(q1.DespatchDate AS DATE) order by CAST(q1.DespatchDate AS DATE) desc"
        End If

        'Checking and adding the condition for CustomerID 
        strSql = ChkCBOcustomer(strSql)

        ChartSales.Series("SalesValue").Color = Drawing.Color.Gold
        ChartSales.Series("Quantity").Color = Drawing.Color.RosyBrown
        ChartSales.Series("SalesValue").CustomProperties = "PointWidth= 0.2,DrawingStyle=Emboss"
        ChartSales.Series("Quantity").CustomProperties = "PointWidth= 0.2,DrawingStyle=Emboss"
        ChartSales.ChartAreas(0).Area3DStyle.Enable3D = False

        ChartSales.ChartAreas("ChartArea1").AxisX.IntervalType = DateTimeIntervalType.Days
        ChartSales.ChartAreas("ChartArea1").AxisX.Interval = 1
        ChartSales.ChartAreas("ChartArea1").AxisX.IntervalAutoMode = True
        ChartSales.ChartAreas("ChartArea1").AxisX.IntervalOffset = 1
        '    ChartSales.ChartAreas(0).AxisX.Minimum = minDate.ToOADate
        '   ChartSales.ChartAreas(0).AxisX.Maximum = maxDate.ToOADate
        'ChartSales.ChartAreas(0).AxisX.ScaleView.Size = 25

        ChartSales.ChartAreas("ChartArea1").AxisY.IntervalType = DateTimeIntervalType.Number
        FillChartSales(ChartSales, strSql, "14")
        Return

    End Sub
    Protected Sub btnLast13_Click(sender As Object, e As EventArgs) Handles btnLast13.Click

        If CInt(Me.cboProduct.SelectedValue) = 3 Then
            If CInt(Me.cboType.SelectedValue) = 1 Then
                'customers selection
                displayShuttersChart(2, CInt(Me.cboReport.SelectedValue), CInt(Me.cboSubType.SelectedValue))
            Else
                'not customers selection
                displayShuttersChart(2, CInt(Me.cboReport.SelectedValue), -1)
            End If
            Exit Sub
        End If

        Dim strSql As String
        Dim strProductType As String = cboProduct.SelectedIndex
        dteDate = Date.ParseExact(txtCurrDate.Text, "dd/MM/yyyy",
            System.Globalization.DateTimeFormatInfo.InvariantInfo)
        intMonthID = txtMonthID.Text
        intWeekID = txtWeekID.Text

        lblChartHeader.Text = "Last 13 Week Presentation"
        'New Section
        Dim intWeek3Months As Integer = intWeekID - 12
        strSql = "Select loctblDays.WeekID, sum(TotalPanels) as TotalPanelNos, sum(CostPrice) as TotalCost from tblProductionScheduleList "
        strSql += " inner join loctblDays On tblProductionScheduleList.OrderDate = loctblDays.DayDate "
        strSql += " Where ProductTypeID = " + strProductType + " and (loctblDays.WeekID >= " & intWeek3Months
        strSql += " And loctblDays.WeekID <= " & intWeekID & ") group by loctblDays.WeekID"
        If cboReport.SelectedIndex > 1 Then
            strSql = "Select loctblDays.WeekID, sum(TotalPanels) as TotalPanelNos, sum(CostPrice) as TotalCost from tblProductionScheduleList "
            strSql += " Cross Apply fn_getJobStagesForScheduleID(dbo.tblProductionScheduleList.ID) q1 "
            strSql += " inner join loctblDays On q1.DespatchDate = loctblDays.DayDate "
            strSql += " Where ProductTypeID = " + strProductType + " and (loctblDays.WeekID >= " & intWeek3Months
            strSql += " And loctblDays.WeekID <= " & intWeekID & ") group by loctblDays.WeekID"
        End If
        strSql = ChkCBOcustomer(strSql)

        ChartSales.Series("SalesValue").Color = Drawing.Color.Gold
        ChartSales.Series("Quantity").Color = Drawing.Color.RosyBrown
        ChartSales.Series("SalesValue").CustomProperties = "PointWidth= 0.2,DrawingStyle=Emboss"
        ChartSales.Series("Quantity").CustomProperties = "PointWidth= 0.2,DrawingStyle=Emboss"
        ChartSales.ChartAreas(0).Area3DStyle.Enable3D = False

        ChartSales.ChartAreas("ChartArea1").AxisX.IntervalType = DateTimeIntervalType.Number
        ChartSales.ChartAreas("ChartArea1").AxisX.Interval = 1
        ChartSales.ChartAreas("ChartArea1").AxisX.IntervalOffset = 1
        ChartSales.ChartAreas(0).AxisX.Minimum = "NaN"
        ChartSales.ChartAreas(0).AxisX.Maximum = "NaN"
        ChartSales.ChartAreas(0).AxisX.ScaleView.Size = 13

        ChartSales.ChartAreas("ChartArea1").AxisY.IntervalType = DateTimeIntervalType.Number
        lblChartHeader.Text = "Last 13 Weeks Presentation"

        FillChartSales(ChartSales, strSql, "13")

    End Sub


    Protected Sub btnLast365_Click(sender As Object, e As EventArgs) Handles btnLast365.Click

        If CInt(Me.cboProduct.SelectedValue) = 3 Then
            If CInt(Me.cboType.SelectedValue) = 1 Then
                'customers selection
                displayShuttersChart(3, CInt(Me.cboReport.SelectedValue), CInt(Me.cboSubType.SelectedValue))
            Else
                'not customers selection
                displayShuttersChart(3, CInt(Me.cboReport.SelectedValue), -1)
            End If
            Exit Sub
        End If

        Dim strSql As String
        Dim strProductType As String = cboProduct.SelectedIndex
        ChartSales.Series("SalesValue").Color = Drawing.Color.Gold
        ChartSales.Series("Quantity").Color = Drawing.Color.RosyBrown

        dteDate = Date.ParseExact(txtCurrDate.Text, "dd/MM/yyyy",
            System.Globalization.DateTimeFormatInfo.InvariantInfo)
        intMonthID = txtMonthID.Text
        intWeekID = txtWeekID.Text

        Dim int12Months As Integer = intMonthID - 11
        strSql = "Select loctblWeeks.MonthID, sum(TotalPanels) as TotalPanelNos, sum(CostPrice) as TotalCost from tblProductionScheduleList "
        strSql += "inner join loctblDays on tblProductionScheduleList.OrderDate = loctblDays.DayDate"
        strSql += " inner join dbo.loctblWeeks on loctblDays.WeekID = loctblWeeks.WeekID Where ProductTypeID = "
        strSql += strProductType & " and (loctblWeeks.MonthID >= " & int12Months & " and loctblWeeks.MonthID <= " & intMonthID
        strSql &= ") group by loctblWeeks.MonthID  order by loctblWeeks.MonthID"
        If cboReport.SelectedIndex > 1 Then
            strSql = "Select loctblWeeks.MonthID, sum(TotalPanels) as TotalPanelNos, sum(CostPrice) as TotalCost from tblProductionScheduleList "
            strSql += " Cross Apply fn_getJobStagesForScheduleID(dbo.tblProductionScheduleList.ID) q1 "
            strSql += "inner join loctblDays on q1.DespatchDate = loctblDays.DayDate"
            strSql += " inner join dbo.loctblWeeks on loctblDays.WeekID = loctblWeeks.WeekID Where ProductTypeID = "
            strSql += strProductType & " and (loctblWeeks.MonthID >= " & int12Months & " and loctblWeeks.MonthID <= " & intMonthID
            strSql &= ") group by loctblWeeks.MonthID  order by loctblWeeks.MonthID"
        End If
        strSql = ChkCBOcustomer(strSql)

        ChartSales.Series("SalesValue").CustomProperties = "PointWidth= 0.2,DrawingStyle=Emboss"
        ChartSales.Series("Quantity").CustomProperties = "PointWidth= 0.2,DrawingStyle=Emboss"
        ChartSales.ChartAreas(0).Area3DStyle.Enable3D = False

        ChartSales.ChartAreas("ChartArea1").AxisX.IntervalType = DateTimeIntervalType.Number
        ChartSales.ChartAreas("ChartArea1").AxisX.Interval = 1
        ChartSales.ChartAreas("ChartArea1").AxisX.IntervalOffset = 1
        ChartSales.ChartAreas(0).AxisX.Minimum = "NaN"
        ChartSales.ChartAreas(0).AxisX.Maximum = "NaN"
        ChartSales.ChartAreas(0).AxisX.ScaleView.Size = 12

        ChartSales.ChartAreas("ChartArea1").AxisY.IntervalType = DateTimeIntervalType.Number

        lblChartHeader.Text = "Last 12 Months Presentation"
        FillChartSales(ChartSales, strSql, "12")
    End Sub
    Private Sub FillChartSales(ChartSales As Chart, SqlStr As String, NoOfDays As String)
        Dim strChartname As String, strChartColumn As String
        Dim dbConn As New DBConnection
        Dim cnn As SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim cmd As New SqlCommand
        Dim adp As New SqlDataAdapter
        Dim dt As New DataTable, dtAxisLabel As New DataTable
        'DataTble for Display Field
        Dim strSQL As String = "Select loctblDays.DayDate, loctblDays.WeekID, loctblWeeks.MonthID, loctblWeeks.WeekNumber, loctblMonths.MonthAbb, loctblMonths.YearID from loctblDays "
        strSQL += "Inner Join loctblWeeks ON loctblDays.WeekID = loctblWeeks.WeekID  "
        strSQL += " inner join dbo.loctblMonths on loctblWeeks.MonthID = loctblMonths.MonthID Where (loctblDays.DayDate >= '"
        If NoOfDays = 14 Then
            strSQL &= Format(dteDate.AddDays(-14), "yyyy-MM-dd") & "') and (loctblDays.DayDate <= '" & Format(dteDate, "yyyy-MM-dd") & "')"
        ElseIf NoOfDays = 12 Then
            strSQL &= Format(dteDate.AddDays(-365), "yyyy-MM-dd") & "') and (loctblDays.DayDate <= '" & Format(dteDate, "yyyy-MM-dd") & "')"
        ElseIf NoOfDays = 13 Then
            strSQL &= Format(dteDate.AddDays(-91), "yyyy-MM-dd") & "') and (loctblDays.DayDate <= '" & Format(dteDate, "yyyy-MM-dd") & "')"
        End If

        If cboReport.SelectedIndex = 0 Or cboReport.SelectedIndex = 2 Then
            strChartname = "Quantity"
            strChartColumn = "TotalPanelNos"
        Else
            strChartname = "SalesValue"
            strChartColumn = "TotalCost"
        End If
        ChartSales.Visible = True
        If cboReport.SelectedIndex = 2 Or cboReport.SelectedIndex = 4 Then
            ChartSales.Series(strChartname).ToolTip = "#VAL{$0.0}"
        Else
            ChartSales.Series(strChartname).ToolTip = "#VAL{0.0}"
        End If


        cnn.Open()
        cmd.Connection = cnn
        cmd.CommandText = SqlStr
        cmd.CommandType = CommandType.Text
        adp.SelectCommand = cmd
        adp.Fill(dt)

        cmd.CommandText = strSQL
        adp.Fill(dtAxisLabel)

        dt = FillEmptyRecords(dt, dtAxisLabel, NoOfDays)
        For i = 0 To dt.Rows.Count - 1
            ChartSales.Series(strChartname).Points.AddXY(dt.Rows(i).Item("Display").ToString, dt.Rows(i).Item(strChartColumn).ToString)
        Next


    End Sub

    Function FillEmptyRecords(dt As DataTable, dtAxisLabel As DataTable, NoRecords As Integer) As DataTable
        Dim service As New AppService
        Dim intFirstOccurrence As Integer = 0
        Dim intMaxPanel As Integer = 0, intMaxCost As Integer = 0
        Dim dteTempDates As Date
        Dim strSql As String
        Dim drResult As DataRow(), drAxisLabels As DataRow()
        Dim drow As DataRow
        ''Adding Display Column
        Dim col As DataColumn = New DataColumn
        col.ColumnName = "Display"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        If dt.Rows.Count > 0 Then
            ChartSales.ChartAreas("ChartArea1").AxisY.Interval = "NaN"
            ChartSales.ChartAreas(0).AxisY.Minimum = "NaN"
            ChartSales.ChartAreas(0).AxisY.Maximum = "NaN"
        Else
            ChartSales.ChartAreas("ChartArea1").AxisY.Interval = 1
            ChartSales.ChartAreas(0).AxisY.Minimum = 0
            ChartSales.ChartAreas(0).AxisY.Maximum = 15
        End If
        If NoRecords = "14" Then
            For i = 0 To 13
                dteTempDates = DateAdd(DateInterval.Day, -13 + i, dteDate)
                strSql = String.Format("neworderdate = #{0}#", dteTempDates.ToString("MM-dd-yyyy"))
                drAxisLabels = dtAxisLabel.Select(strSql.Replace("neworderdate", "DayDate"))
                drResult = dt.Select(strSql)
                If drResult.Length = 0 Then
                    drow = dt.NewRow
                    drow("neworderdate") = dteTempDates.ToString("d MMM yyyy")
                    drow("Display") = dteTempDates.Day().ToString() + " " + drAxisLabels(0).Item("MonthAbb") + " " + drAxisLabels(0).Item("YearID").ToString
                    drow("TotalPanelNos") = "0"
                    drow("TotalCost") = "0"
                    dt.Rows.Add(drow)
                    drow = Nothing
                Else
                    drResult(0).Item("Display") = dteTempDates.Day().ToString() + " " + drAxisLabels(0).Item("MonthAbb") + " " + drAxisLabels(0).Item("YearID").ToString
                End If
            Next
            Return dt
        ElseIf NoRecords = 13 Then
            For i = 0 To 12
                intFirstOccurrence = intWeekID - (NoRecords - 1 - i)
                strSql = String.Format("WeekID = {0}", intFirstOccurrence.ToString())
                drAxisLabels = dtAxisLabel.Select(strSql)
                drResult = dt.Select(strSql)
                If drResult.Length = 0 Then
                    drow = dt.NewRow
                    drow("WeekID") = intFirstOccurrence
                    drow("Display") = drAxisLabels(0).Item("MonthAbb") + " wk " + drAxisLabels(0).Item("WeekNumber").ToString
                    drow("TotalPanelNos") = "0"
                    drow("TotalCost") = "0"
                    dt.Rows.Add(drow)
                    drow = Nothing
                Else
                    drResult(0).Item("Display") = drAxisLabels(0).Item("MonthAbb") + " wk " + drAxisLabels(0).Item("WeekNumber").ToString
                End If
            Next
            Return dt.Select("", "WeekID").CopyToDataTable
        ElseIf NoRecords = 12 Then
            For i = 0 To 11
                intFirstOccurrence = intMonthID - (NoRecords - 1 - i)
                strSql = String.Format("MonthID = {0}", intFirstOccurrence.ToString())
                drAxisLabels = dtAxisLabel.Select(strSql)
                drResult = dt.Select(strSql)
                If drResult.Length = 0 Then
                    drow = dt.NewRow
                    drow("MonthID") = intFirstOccurrence
                    drow("Display") = intFirstOccurrence.ToString()
                    drow("Display") = drAxisLabels(0).Item("MonthAbb") + " " + drAxisLabels(0).Item("YearID").ToString
                    drow("TotalPanelNos") = "0"
                    drow("TotalCost") = "0"
                    dt.Rows.Add(drow)
                    drow = Nothing
                Else
                    drResult(0).Item("Display") = drAxisLabels(0).Item("MonthAbb") + " " + drAxisLabels(0).Item("YearID").ToString
                End If
            Next
            Return dt.Select("", "MonthID").CopyToDataTable
        End If
        Return dt

    End Function

    Protected Sub cboType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboType.SelectedIndexChanged
        Select Case cboType.SelectedValue
            Case 1
                getCustomers()
            Case 2
                getInternalExternal()
            Case 3
                getRegions()
            Case 4
                getTiers()
        End Select
    End Sub

    Protected Sub getCustomers()

        If Me.cboProduct.SelectedIndex = 0 Then
            Exit Sub
        End If

        Dim service As New AppService
        Dim dtCustomer As New DataTable

        If cboProduct.SelectedValue = 3 Then
            dtCustomer = service.runSQLOzrollTracking("select customer_number as CustomerID, company_name as CustomerName from dbo.tblCustomerDetails order by company_name")
        Else
            dtCustomer = service.runSQLScheduling("Select CustomerID, CustomerName from tblCustomers order by CustomerName")
        End If

        Dim drow As DataRow = dtCustomer.NewRow
        drow("CustomerID") = -1
        drow("CustomerName") = "All"
        dtCustomer.Rows.InsertAt(drow, 0)
        drow = Nothing

        service = Nothing

        Me.cboSubType.DataSource = dtCustomer
        Me.cboSubType.DataValueField = "CustomerID"
        Me.cboSubType.DataTextField = "CustomerName"
        Me.cboSubType.DataBind()
        Me.cboSubType.SelectedIndex = 0

    End Sub

    Protected Sub getInternalExternal()

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "GroupID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "GroupName"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        Dim drow As DataRow

        drow = dt.NewRow
        drow("GroupID") = 0
        drow("GroupName") = "All"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("GroupID") = 1
        drow("GroupName") = "Internal"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("GroupID") = 2
        drow("GroupName") = "External"
        dt.Rows.Add(drow)
        drow = Nothing

        Me.cboSubType.DataSource = dt
        Me.cboSubType.DataValueField = "GroupID"
        Me.cboSubType.DataTextField = "GroupName"
        Me.cboSubType.DataBind()
        Me.cboSubType.SelectedIndex = 0


    End Sub
    Protected Sub getProductType()

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "ProductID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Product"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        Dim drow As DataRow

        drow = dt.NewRow
        drow("GroupID") = 0
        drow("GroupName") = "All"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("GroupID") = 1
        drow("GroupName") = "Internal"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("GroupID") = 2
        drow("GroupName") = "External"
        dt.Rows.Add(drow)
        drow = Nothing

        Me.cboSubType.DataSource = dt
        Me.cboSubType.DataValueField = "GroupID"
        Me.cboSubType.DataTextField = "GroupName"
        Me.cboSubType.DataBind()
        Me.cboSubType.SelectedIndex = 0


    End Sub
    Protected Sub getRegions()

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "RegionID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "RegionName"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        Dim drow As DataRow

        drow = dt.NewRow
        drow("RegionID") = 0
        drow("RegionName") = "All"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("RegionID") = 1
        drow("RegionName") = "NSW"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("RegionID") = 2
        drow("RegionName") = "VIC"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("RegionID") = 3
        drow("RegionName") = "SA"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("RegionID") = 4
        drow("RegionName") = "QLD"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("RegionID") = 5
        drow("RegionName") = "WA"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("RegionID") = 6
        drow("RegionName") = "TAS"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("RegionID") = 7
        drow("RegionName") = "ACT"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("RegionID") = 8
        drow("RegionName") = "NT"
        dt.Rows.Add(drow)
        drow = Nothing


        Me.cboSubType.DataSource = dt
        Me.cboSubType.DataValueField = "RegionID"
        Me.cboSubType.DataTextField = "RegionName"
        Me.cboSubType.DataBind()
        Me.cboSubType.SelectedIndex = 0

    End Sub

    Protected Sub getTiers()

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "TierID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "TierName"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        Dim drow As DataRow

        drow = dt.NewRow
        drow("TierID") = 0
        drow("TierName") = "All"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("TierID") = 1
        drow("TierName") = "Platinum"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("TierID") = 2
        drow("TierName") = "Gold"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("TierID") = 2
        drow("TierName") = "Silver"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("TierID") = 2
        drow("TierName") = "Bronze"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("TierID") = 2
        drow("TierName") = "Trade"
        dt.Rows.Add(drow)
        drow = Nothing

        Me.cboSubType.DataSource = dt
        Me.cboSubType.DataValueField = "TierID"
        Me.cboSubType.DataTextField = "TierName"
        Me.cboSubType.DataBind()
        Me.cboSubType.SelectedIndex = 0

    End Sub

    Private Sub FillCboType(arrType As Array)
        cboSubType.Items.Clear()
        If arrType(0) = "searchtable" Then
            Dim service As New AppService
            Dim dtCustomer As DataTable = service.runSQLScheduling("Select * from [tblCustomers]")
            Me.cboSubType.DataSource = dtCustomer
            Me.cboSubType.DataValueField = "CustomerID"
            Me.cboSubType.DataTextField = "CustomerName"

            Dim drow As DataRow = dtCustomer.NewRow
            drow("CustomerID") = 0
            drow("CustomerName") = "All"
            dtCustomer.Rows.InsertAt(drow, 0)
            drow = Nothing
            Me.cboSubType.DataBind()
            Me.cboSubType.SelectedIndex = 0
        Else
            For i = 0 To arrType.Length - 1
                cboSubType.Items.Add(arrType(i))
            Next
        End If
    End Sub
    Function ChkCBOcustomer(strSql As String) As String
        Dim strCustomerID As String
        If cboType.SelectedIndex = 0 And cboSubType.SelectedIndex > 0 Then
            strCustomerID = String.Format("where CustomerID = {0} and ProductTypeID", cboSubType.SelectedValue)
            strSql = strSql.ToLower.Replace("where producttypeid", strCustomerID)
        End If
        Return strSql
    End Function

    Protected Sub resetReportValueSelection(intProductID As Integer)

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "ReportID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ReportName"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        Dim drow As DataRow

        drow = dt.NewRow
        drow("ReportID") = 0
        drow("ReportName") = ""
        dt.Rows.Add(drow)
        drow = Nothing

        If intProductID = 1 Or intProductID = 2 Then
            drow = dt.NewRow
            drow("ReportID") = 1
            drow("ReportName") = "Panels Sold"
            dt.Rows.Add(drow)
            drow = Nothing

            drow = dt.NewRow
            drow("ReportID") = 2
            drow("ReportName") = "$ Sold"
            dt.Rows.Add(drow)
            drow = Nothing

            drow = dt.NewRow
            drow("ReportID") = 3
            drow("ReportName") = "Panels Despatched"
            dt.Rows.Add(drow)
            drow = Nothing

            drow = dt.NewRow
            drow("ReportID") = 4
            drow("ReportName") = "$ Despatched"
            dt.Rows.Add(drow)
            drow = Nothing
        ElseIf intProductID = 3 Then
            drow = dt.NewRow
            drow("ReportID") = 1
            drow("ReportName") = "Shutters Sold"
            dt.Rows.Add(drow)
            drow = Nothing

            drow = dt.NewRow
            drow("ReportID") = 2
            drow("ReportName") = "$ Sold"
            dt.Rows.Add(drow)
            drow = Nothing

            drow = dt.NewRow
            drow("ReportID") = 3
            drow("ReportName") = "Shutters Invoiced"
            dt.Rows.Add(drow)
            drow = Nothing

            drow = dt.NewRow
            drow("ReportID") = 4
            drow("ReportName") = "$ Invoiced"
            dt.Rows.Add(drow)
            drow = Nothing

        End If

        Me.cboReport.DataSource = dt
        Me.cboReport.DataValueField = "ReportID"
        Me.cboReport.DataTextField = "ReportName"
        Me.cboReport.DataBind()
        Me.cboReport.SelectedIndex = 0

    End Sub

    Private Sub displayShuttersChart(intDateRange As Integer, intReportType As Integer, intCustomerID As Integer)


        ChartSales.Series("SalesValue").CustomProperties = "PointWidth= 0.2,DrawingStyle=Emboss"
        ChartSales.Series("Quantity").CustomProperties = "PointWidth= 0.2,DrawingStyle=Emboss"
        ChartSales.ChartAreas(0).Area3DStyle.Enable3D = False

        '      ChartSales.ChartAreas(0).AxisX.LabelStyle.Format = "N2"
        ChartSales.ChartAreas("ChartArea1").AxisX.IntervalType = DateTimeIntervalType.Number
        ChartSales.ChartAreas("ChartArea1").AxisX.Interval = 1
        ChartSales.ChartAreas("ChartArea1").AxisX.IntervalOffset = 1
        ChartSales.ChartAreas(0).AxisX.Minimum = "NaN"
        ChartSales.ChartAreas(0).AxisX.Maximum = "NaN"

        Select Case intDateRange
            Case 1
                ChartSales.ChartAreas(0).AxisX.ScaleView.Size = 14
            Case 2
                ChartSales.ChartAreas(0).AxisX.ScaleView.Size = 13
            Case 3
                ChartSales.ChartAreas(0).AxisX.ScaleView.Size = 12
        End Select

        '    ChartSales.ChartAreas(0).AxisY.LabelStyle.Format = "N2"
        ChartSales.ChartAreas("ChartArea1").AxisY.IntervalType = DateTimeIntervalType.Number

        Dim dt As DataTable = getShuttersDataForGraph(intDateRange, intReportType, intCustomerID)

        For i = 0 To dt.Rows.Count - 1
            If intReportType = 1 Or intReportType = 3 Then
                ChartSales.Series(0).Points.AddXY(dt.Rows(i).Item("Display").ToString, dt.Rows(i).Item("NoOfShutters").ToString)
            Else
                ChartSales.Series(0).Points.AddXY(dt.Rows(i).Item("Display").ToString, dt.Rows(i).Item("SalesPrice").ToString)
            End If
        Next



    End Sub

    Protected Function getShuttersDataForGraph(intDateRange As Integer, intReportType As Integer, intCustomerID As Integer) As DataTable


        Dim dteStartDate As Date
        Dim dteEndDate As Date

        Dim service As New AppService

        'get data for each range from each database
        Select Case intDateRange
            Case 1
                dteStartDate = CDate(Me.txtDate2Weeks.Text)
            Case 2
                dteStartDate = CDate(Me.txtDate3Months.Text)
            Case 3
                dteStartDate = CDate(Me.txtDate12Months.Text)
        End Select

        dteEndDate = CDate(Me.txtCurrDate.Text)

        Dim strSQLOZOTS As String = String.Empty
        Dim strSQLTracking As String = String.Empty

        If intReportType = 1 Or intReportType = 2 Then
            'sold date query from OZOTS
            strSQLOZOTS = "Select count(dbo.[order].id) As NoOfJobs, dbo.[order].customer_id as CustomerID, sum(q1.NoOfShutters) As NoOfShutters, sum(q2.SalesPrice) As SalesPrice, CAST(dbo.[order].created AS DATE) as DayDate "
            strSQLOZOTS &= "From dbo.[order] INNER Join dbo.customer ON dbo.[order].customer_id = dbo.customer.id "
            strSQLOZOTS &= "inner Join(SELECT count(dbo.order_assembled_detail_complete.id) As NoOfShutters, dbo.order_assembled_detail_complete.order_id FROM dbo.order_assembled_detail_complete group by dbo.order_assembled_detail_complete.order_id) q1 on q1.order_id = dbo.[order].id "
            strSQLOZOTS &= "inner Join(SELECT sum(dbo.order_line.sell_price) As SalesPrice, dbo.order_line.order_id FROM dbo.order_line group by dbo.order_line.order_id) q2 on q2.order_id = dbo.[order].id "
            strSQLOZOTS &= "Left outer join dbo.invoice_details on dbo.invoice_details.order_id = dbo.[order].id "
            strSQLOZOTS &= "where dbo.[order].status In (4, 5, 9) and (CAST(dbo.[order].created AS DATE) >= '" & Format(dteStartDate, "yyyy-MM-dd") & "' and CAST(dbo.[order].created AS DATE) <= '" & Format(dteEndDate, "yyyy-MM-dd") & "') "
            strSQLOZOTS &= "Group by dbo.[order].customer_id, CAST(dbo.[order].created AS DATE) "
        Else
            'invoiced date query from OZOTS
            strSQLOZOTS = "Select count(dbo.[order].id) As NoOfJobs, dbo.[order].customer_id as CustomerID, sum(q1.NoOfShutters) As NoOfShutters, sum(q2.SalesPrice) As SalesPrice, CAST(dbo.[invoice_details].issued_date AS DATE) as DayDate "
            strSQLOZOTS &= "From dbo.[order] INNER Join dbo.customer ON dbo.[order].customer_id = dbo.customer.id "
            strSQLOZOTS &= "inner Join(SELECT count(dbo.order_assembled_detail_complete.id) As NoOfShutters, dbo.order_assembled_detail_complete.order_id FROM dbo.order_assembled_detail_complete group by dbo.order_assembled_detail_complete.order_id) q1 on q1.order_id = dbo.[order].id "
            strSQLOZOTS &= "inner Join(SELECT sum(dbo.order_line.sell_price) As SalesPrice, dbo.order_line.order_id FROM dbo.order_line group by dbo.order_line.order_id) q2 on q2.order_id = dbo.[order].id "
            strSQLOZOTS &= "Left outer join dbo.invoice_details on dbo.invoice_details.order_id = dbo.[order].id "
            strSQLOZOTS &= "where dbo.[order].status In (9) and (CAST(dbo.[invoice_details].issued_date AS DATE) >= '" & Format(dteStartDate, "yyyy-MM-dd") & "' and CAST(dbo.[invoice_details].issued_date AS DATE) <= '" & Format(dteEndDate, "yyyy-MM-dd") & "') "
            strSQLOZOTS &= "Group by dbo.[order].customer_id, CAST(dbo.[invoice_details].issued_date AS DATE) "
        End If

        If intReportType = 1 Or intReportType = 2 Then
            'sold date query from Ozroll Tracking
            strSQLTracking = "Select count(dbo.tblTracking.lngID) As NoOfJobs, dbo.tblTracking.CustomerID, sum(q1.NoOfShutters) As NoOfShutters, sum(dbo.tblTracking.sngShutterPrice) As SalesPrice, dbo.tblTracking.datDate as DayDate "
            strSQLTracking &= "From dbo.tblTracking INNER Join dbo.tblCustomerDetails ON dbo.tblTracking.CustomerID = dbo.tblCustomerDetails.customer_number "
            strSQLTracking &= "inner Join(SELECT count(dbo.tblOrderDetails.lngID) As NoOfShutters, dbo.tblOrderDetails.lngTrackingID FROM dbo.tblOrderDetails group by dbo.tblOrderDetails.lngTrackingID) q1 on q1.lngTrackingID = dbo.tblTracking.lngID "
            strSQLTracking &= "where dbo.tblTracking.blnDisabled=0 and dbo.tblTracking.intOrderType=1 and (CAST(dbo.tblTracking.datDate AS DATE) >= '" & Format(dteStartDate, "yyyy-MM-dd") & "' and CAST(dbo.tblTracking.datDate AS DATE) <= '" & Format(dteEndDate, "yyyy-MM-dd") & "') "
            strSQLTracking &= "Group by dbo.tblTracking.CustomerID, dbo.tblTracking.datDate "
        Else
            'invoiced date query from Ozroll Tracking
            strSQLTracking = "Select count(dbo.tblTracking.lngID) As NoOfJobs, dbo.tblTracking.CustomerID, sum(q1.NoOfShutters) As NoOfShutters, sum(dbo.tblTracking.sngShutterPrice) As SalesPrice, dbo.tblTracking.datInvoiced as DayDate "
            strSQLTracking &= "From dbo.tblTracking INNER Join dbo.tblCustomerDetails ON dbo.tblTracking.CustomerID = dbo.tblCustomerDetails.customer_number "
            strSQLTracking &= "inner Join(SELECT count(dbo.tblOrderDetails.lngID) As NoOfShutters, dbo.tblOrderDetails.lngTrackingID FROM dbo.tblOrderDetails group by dbo.tblOrderDetails.lngTrackingID) q1 on q1.lngTrackingID = dbo.tblTracking.lngID "
            strSQLTracking &= "where dbo.tblTracking.blnDisabled=0 and dbo.tblTracking.intOrderType=1 and (CAST(dbo.tblTracking.datInvoiced AS DATE) >= '" & Format(dteStartDate, "yyyy-MM-dd") & "' and CAST(dbo.tblTracking.datInvoiced AS DATE) <= '" & Format(dteEndDate, "yyyy-MM-dd") & "') "
            strSQLTracking &= "Group by dbo.tblTracking.CustomerID, dbo.tblTracking.datInvoiced "
        End If


        Dim dtDataOZOTS As DataTable = service.runSQLOZOTS(strSQLOZOTS)
        Dim dtDataOzrollTracking As DataTable = service.runSQLOzrollTracking(strSQLTracking)

        'update ozots data with ozroll tracking customerid values for merging
        Dim dtCustomerMapping As DataTable = service.runSQLOzrollTracking("select * from dbo.tblOZOTSCustomerMapping")
        dtDataOZOTS = updateOZOTSDatatableWithTrackingCustomerID(dtDataOZOTS, dtCustomerMapping)

        'merge both tables together
        Dim dtDataCombined As DataTable = combineDatatables(dtDataOZOTS, dtDataOzrollTracking)

        'filter for selection data
        If intCustomerID >= 0 Then
            'filter for customerid value
            dtDataCombined = SharedFunctions.performDatatableSelect(dtDataCombined, "CustomerID=" & intCustomerID)
        End If

        'update data with weekid and monthid
        Dim strSQL As String = "Select loctblDays.DayDate, loctblDays.WeekID, loctblWeeks.MonthID, loctblWeeks.WeekNumber, loctblMonths.MonthAbb, loctblMonths.YearID from loctblDays Inner Join loctblWeeks ON loctblDays.WeekID = loctblWeeks.WeekID  inner join dbo.loctblMonths on loctblWeeks.MonthID = loctblMonths.MonthID Where (loctblDays.DayDate >= '" & Format(dteStartDate, "yyyy-MM-dd") & "') and (loctblDays.DayDate <= '" & Format(dteEndDate, "yyyy-MM-dd") & "')"
        Dim dtDateRange As DataTable = service.runSQLScheduling(strSQL)
        dtDataCombined = updateOZOTSDatatableWithWeekMonth(dtDataCombined, dtDateRange)

        'dtDataOZOTS = updateOZOTSDatatableWithWeekMonth(dtDataOZOTS, dtDateRange)
        'dtDataOzrollTracking = updateOZOTSDatatableWithWeekMonth(dtDataOzrollTracking, dtDateRange)

        'fill datatables for the three date ranges and return to display on graph
        Dim dtReturnDatatable As DataTable = New DataTable

        Select Case intDateRange
            Case 1
                Dim dt2Weeks As DataTable = create2WeeksDatatable()
                dt2Weeks = fill2WeeksDatatable(dt2Weeks, dtDataCombined, CDate(Me.txtDate2Weeks.Text), CDate(Me.txtCurrDate.Text))
                dtReturnDatatable = dt2Weeks.Copy
            Case 2
                Dim dt3Months As DataTable = create3MonthsDatatable()
                dt3Months = fill3MonthsDatatable(dt3Months, dtDataCombined, CInt(Me.txtWeek3Months.Text), CInt(Me.txtWeekID.Text), dtDateRange)
                dtReturnDatatable = dt3Months.Copy
            Case 3
                Dim dt12Months As DataTable = create12MonthsDatatable()
                dt12Months = fill12MonthsDatatable(dt12Months, dtDataCombined, CInt(Me.txtMonth12Months.Text), CInt(Me.txtMonthID.Text), dtDateRange)
                dtReturnDatatable = dt12Months.Copy
        End Select


        Return dtReturnDatatable

    End Function

    Protected Function combineDatatables(dtOZOTS As DataTable, dtTracking As DataTable) As DataTable

        For i As Integer = 0 To dtOZOTS.Rows.Count - 1
            Dim drow As DataRow = dtTracking.NewRow
            For Each col As DataColumn In dtTracking.Columns
                drow(col.ColumnName) = (dtOZOTS.Rows(i)(col.ColumnName))
            Next
            dtTracking.Rows.Add(drow)
            drow = Nothing
        Next

        Return dtTracking

    End Function


    Protected Function updateOZOTSDatatableWithWeekMonth(dtData As DataTable, dtDaysWeeksMonths As DataTable) As DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "WeekID"
        col.DataType = System.Type.GetType("System.Int32")
        dtData.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "MonthID"
        col.DataType = System.Type.GetType("System.Int32")
        dtData.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "WeekNumber"
        col.DataType = System.Type.GetType("System.Int32")
        dtData.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "MonthAbb"
        col.DataType = System.Type.GetType("System.String")
        dtData.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "YearID"
        col.DataType = System.Type.GetType("System.Int32")
        dtData.Columns.Add(col)
        col = Nothing


        For i As Integer = 0 To dtData.Rows.Count - 1
            Dim drows() As DataRow = dtDaysWeeksMonths.Select("DayDate = '" & Format(dtData.Rows(i)("DayDate"), "dd-MM-yyyy") & "'")
            If drows.Length > 0 Then
                dtData.Rows(i)("WeekID") = drows(0)("WeekID")
                dtData.Rows(i)("MonthID") = drows(0)("MonthID")
                dtData.Rows(i)("WeekNumber") = drows(0)("WeekNumber")
                dtData.Rows(i)("MonthAbb") = drows(0)("MonthAbb")
                dtData.Rows(i)("YearID") = drows(0)("YearID")
            End If
            drows = Nothing
        Next

        Return dtData

    End Function

    Protected Function updateOZOTSDatatableWithTrackingCustomerID(dtData As DataTable, dtCustomerMapping As DataTable) As DataTable

        For i As Integer = 0 To dtData.Rows.Count - 1
            Dim drows() As DataRow = dtCustomerMapping.Select("OZOTSCustomerID = " & dtData.Rows(i)("CustomerID").ToString)
            If drows.Length > 0 Then
                dtData.Rows(i)("CustomerID") = drows(0)("TrackingCustomerID")
            End If
            drows = Nothing
        Next

        Return dtData

    End Function

    Protected Function fill2WeeksDatatable(dt As DataTable, dtData As DataTable, dteStartDate As Date, dteEndDate As Date) As DataTable

        Dim dteDate As Date = dteStartDate

        While dteDate <= dteEndDate
            'Dim drows() As DataRow =  dtData.Select("DayDate = '" & Format(dtData.Rows("DayDate"), "dd-MM-yyyy") & "'")
            Dim decJobs As Decimal = 0
            Dim decShutters As Decimal = 0
            Dim decSalesPrice As Decimal = 0

            decJobs = SharedFunctions.performDatatableSelectSum(dtData, "NoOfJobs", "DayDate = '" & Format(dteDate, "dd-MM-yyyy") & "'")
            decShutters = SharedFunctions.performDatatableSelectSum(dtData, "NoOfShutters", "DayDate = '" & Format(dteDate, "dd-MM-yyyy") & "'")
            decSalesPrice = SharedFunctions.performDatatableSelectSum(dtData, "SalesPrice", "DayDate = '" & Format(dteDate, "dd-MM-yyyy") & "'")

            Dim drow As DataRow = dt.NewRow
            drow("DayDate") = dteDate
            drow("Display") = Format(dteDate, "d MMM yyyy")
            drow("NoOfJobs") = CInt(decJobs)
            drow("NoOfShutters") = CInt(decShutters)
            drow("SalesPrice") = decSalesPrice
            dt.Rows.Add(drow)
            drow = Nothing

            dteDate = DateAdd(DateInterval.Day, 1, dteDate)
        End While

        Return dt

    End Function

    Protected Function fill3MonthsDatatable(dt As DataTable, dtData As DataTable, intStartWeekID As Integer, intEndWeekID As Integer, dtDateRange As DataTable) As DataTable

        For intWeek As Integer = intStartWeekID To intEndWeekID
            'Dim drows() As DataRow = dtData.Select("WeekID = " & intWeek.ToString)

            Dim decJobs As Decimal = 0
            Dim decShutters As Decimal = 0
            Dim decSalesPrice As Decimal = 0

            decJobs = SharedFunctions.performDatatableSelectSum(dtData, "NoOfJobs", "WeekID = " & intWeek.ToString)
            decShutters = SharedFunctions.performDatatableSelectSum(dtData, "NoOfShutters", "WeekID = " & intWeek.ToString)
            decSalesPrice = SharedFunctions.performDatatableSelectSum(dtData, "SalesPrice", "WeekID = " & intWeek.ToString)

            Dim drow As DataRow = dt.NewRow
            drow("WeekID") = intWeek

            Dim drows() As DataRow = dtDateRange.Select("WeekID = " & intWeek.ToString)
            If drows.Length > 0 Then
                drow("Display") = drows(0)("MonthAbb").ToString & " Wk" & drows(0)("WeekNumber").ToString
            End If
            drows = Nothing

            drow("NoOfJobs") = CInt(decJobs)
            drow("NoOfShutters") = CInt(decShutters)
            drow("SalesPrice") = decSalesPrice
            dt.Rows.Add(drow)
            drow = Nothing
        Next

        Return dt

    End Function

    Protected Function fill12MonthsDatatable(dt As DataTable, dtData As DataTable, intStartMonthID As Integer, intEndMonthID As Integer, dtDateRange As DataTable) As DataTable

        For intMonth As Integer = intStartMonthID To intEndMonthID
            'Dim drows() As DataRow = dtData.Select("MonthID = " & intMonth.ToString)
            Dim decJobs As Decimal = 0
            Dim decShutters As Decimal = 0
            Dim decSalesPrice As Decimal = 0

            decJobs = SharedFunctions.performDatatableSelectSum(dtData, "NoOfJobs", "MonthID = " & intMonth.ToString)
            decShutters = SharedFunctions.performDatatableSelectSum(dtData, "NoOfShutters", "MonthID = " & intMonth.ToString)
            decSalesPrice = SharedFunctions.performDatatableSelectSum(dtData, "SalesPrice", "MonthID = " & intMonth.ToString)

            Dim drow As DataRow = dt.NewRow
            drow("MonthID") = intMonth

            Dim drows() As DataRow = dtDateRange.Select("MonthID = " & intMonth.ToString)
            If drows.Length > 0 Then
                drow("Display") = drows(0)("MonthAbb").ToString & " " & drows(0)("YearID").ToString
            End If
            drows = Nothing

            drow("NoOfJobs") = CInt(decJobs)
            drow("NoOfShutters") = CInt(decShutters)
            drow("SalesPrice") = decSalesPrice
            dt.Rows.Add(drow)
            drow = Nothing
        Next

        Return dt

    End Function

    Protected Function create2WeeksDatatable() As DataTable

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "DayDate"
        col.DataType = System.Type.GetType("System.DateTime")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Display"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "NoOfJobs"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "NoOfShutters"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "SalesPrice"
        col.DataType = System.Type.GetType("System.Decimal")
        dt.Columns.Add(col)
        col = Nothing

        Return dt

    End Function

    Protected Function create3MonthsDatatable() As DataTable

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "WeekID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Display"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "NoOfJobs"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "NoOfShutters"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "SalesPrice"
        col.DataType = System.Type.GetType("System.Decimal")
        dt.Columns.Add(col)
        col = Nothing

        Return dt

    End Function

    Protected Function create12MonthsDatatable() As DataTable

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "MonthID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Display"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "NoOfJobs"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "NoOfShutters"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "SalesPrice"
        col.DataType = System.Type.GetType("System.Decimal")
        dt.Columns.Add(col)
        col = Nothing

        Return dt

    End Function

    Private Sub cboProduct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProduct.SelectedIndexChanged

        Me.cboReport.DataSource = Nothing
        If Me.cboProduct.SelectedIndex > 0 Then
            Me.cboType_SelectedIndexChanged(Me, Nothing)
            resetReportValueSelection(CInt(Me.cboProduct.SelectedValue))
        End If


    End Sub
End Class
