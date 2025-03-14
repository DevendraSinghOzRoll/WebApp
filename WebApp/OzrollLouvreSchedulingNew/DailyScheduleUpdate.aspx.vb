﻿
Partial Class DailyScheduleUpdate
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

            If Me.txtProductTypeID.Text = "2" Then
                Me.dgvScheduleList.Columns(16).Visible = False 'framing  
                Me.dgvScheduleList.Columns(18).Visible = False 'wrapping

                'Me.dgvScheduleList.Columns(13).Visible = False 'framing    'Nos changed after adding 3 more columns
                'Me.dgvScheduleList.Columns(15).Visible = False 'wrapping   'Nos changed after adding 3 more columns
            ElseIf Me.txtProductTypeID.Text = "1" Then
                Me.dgvScheduleList.Columns(11).Visible = False 'Pining
                Me.dgvScheduleList.Columns(14).Visible = False 'Hinging  
                Me.dgvScheduleList.Columns(15).Visible = False 'PackUp
            End If

            Dim service As New AppService
            service.addWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)
            service = Nothing

            Me.txtScheduledDate.Text = Format(DateTime.Now, "d MMM yyyy")
            Me.txtScheduledDate_TextChanged(Me, Nothing)

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

    Protected Function getOrderTypeDescription(intOrderTypeID As Integer) As String

        Dim strOrderType As String = String.Empty
        Select Case intOrderTypeID
            Case 1
                strOrderType = "Order"
            Case 2
                strOrderType = "Remake"
            Case 3
                strOrderType = "Reorder"
            Case Else
                strOrderType = String.Empty
        End Select

        Return strOrderType

    End Function

    Protected Function loadDatatable() As DataTable

        Dim service As New AppService

        Dim dteScheduleDate As Date = CDate(Me.txtScheduledDate.Text)

        Dim dt As DataTable = createDatatable()
        Dim dtMain As DataTable = service.getDailyProductionSchedule(dteScheduleDate, CInt(Me.txtProductTypeID.Text))

        service = Nothing

        For i As Integer = 0 To dtMain.Rows.Count - 1
            Dim drow As DataRow = dt.NewRow

            drow("ScheduleID") = dtMain.Rows(i)("ID")
            drow("ShutterProNumber") = dtMain.Rows(i)("ShutterProNumber")
            If Not IsDBNull(dtMain.Rows(i)("OrderTypeID")) Then
                drow("OrderType") = getOrderTypeDescription(CInt(dtMain.Rows(i)("OrderTypeID")))
            Else
                drow("OrderType") = String.Empty
            End If
            drow("BranchName") = dtMain.Rows(i)("BranchName")
            drow("ReferenceNumber") = dtMain.Rows(i)("OrderReference")
            drow("ReferenceName") = dtMain.Rows(i)("CustomerName")
            drow("State") = dtMain.Rows(i)("State")
            drow("NoOfPanels") = dtMain.Rows(i)("TotalPanels")
            drow("TotalSQM") = dtMain.Rows(i)("TotalSQM")
            drow("ScheduledDate") = dtMain.Rows(i)("ScheduledDate")

            drow("PlannedShippingDate") = dtMain.Rows(i)("PlannedShippingDate")
            drow("ExpectedShippingDate") = dtMain.Rows(i)("ExpectedShippingDate")

            If Not IsDBNull(dtMain.Rows(i)("PriorityLevel")) Then
                If CInt(dtMain.Rows(i)("PriorityLevel")) Then
                    drow("Priority") = "High"
                Else
                    drow("Priority") = String.Empty
                End If
            Else
                drow("Priority") = String.Empty
            End If

            drow("hidCuttingID") = dtMain.Rows(i)("CuttingID")
            drow("hidCuttingStatus") = dtMain.Rows(i)("CuttingStatus")
            If Not IsDBNull(dtMain.Rows(i)("CuttingDate")) Then
                drow("hidCuttingDate") = Format(CDate(dtMain.Rows(i)("CuttingDate")), "d MMM yyyy")
            End If
            If Not IsDBNull(dtMain.Rows(i)("CuttingStatus")) Then
                If CInt(dtMain.Rows(i)("CuttingStatus")) = 0 Then
                    drow("Cutting") = False
                Else
                    drow("Cutting") = True
                End If
            Else
                drow("Cutting") = False
            End If

            drow("hidPrepID") = dtMain.Rows(i)("PrepID")
            drow("hidPrepStatus") = dtMain.Rows(i)("PrepStatus")
            If Not IsDBNull(dtMain.Rows(i)("PrepDate")) Then
                drow("hidPrepDate") = Format(CDate(dtMain.Rows(i)("PrepDate")), "d MMM yyyy")
            End If
            If Not IsDBNull(dtMain.Rows(i)("PrepStatus")) Then
                If CInt(dtMain.Rows(i)("PrepStatus")) = 0 Then
                    drow("Prep") = False
                Else
                    drow("Prep") = True
                End If
            Else
                drow("Prep") = False
            End If

            drow("hidAssemblyID") = dtMain.Rows(i)("AssemblyID")
            drow("hidAssemblyStatus") = dtMain.Rows(i)("AssemblyStatus")
            If Not IsDBNull(dtMain.Rows(i)("AssemblyDate")) Then
                drow("hidAssemblyDate") = Format(CDate(dtMain.Rows(i)("AssemblyDate")), "d MMM yyyy")
            End If
            If Not IsDBNull(dtMain.Rows(i)("AssemblyStatus")) Then
                If CInt(dtMain.Rows(i)("AssemblyStatus")) = 0 Then
                    drow("Assembly") = False
                Else
                    drow("Assembly") = True
                End If
            Else
                drow("Assembly") = False
            End If

            drow("hidFramingID") = dtMain.Rows(i)("FramingID")
            drow("hidFramingStatus") = dtMain.Rows(i)("FramingStatus")
            If Not IsDBNull(dtMain.Rows(i)("FramingDate")) Then
                drow("hidFramingDate") = Format(CDate(dtMain.Rows(i)("FramingDate")), "d MMM yyyy")
            End If
            If Not IsDBNull(dtMain.Rows(i)("FramingStatus")) Then
                If CInt(dtMain.Rows(i)("FramingStatus")) = 0 Then
                    drow("Framing") = False
                Else
                    drow("Framing") = True
                End If
            Else
                drow("Framing") = False
            End If

            drow("hidQCID") = dtMain.Rows(i)("QCID")
            drow("hidQCStatus") = dtMain.Rows(i)("QCStatus")
            If Not IsDBNull(dtMain.Rows(i)("QCDate")) Then
                drow("hidQCDate") = Format(CDate(dtMain.Rows(i)("QCDate")), "d MMM yyyy")
            End If
            If Not IsDBNull(dtMain.Rows(i)("QCStatus")) Then
                If CInt(dtMain.Rows(i)("QCStatus")) = 0 Then
                    drow("QC") = False
                Else
                    drow("QC") = True
                End If
            Else
                drow("QC") = False
            End If

            drow("hidWrappingID") = dtMain.Rows(i)("WrappingID")
            drow("hidWrappingStatus") = dtMain.Rows(i)("WrappingStatus")
            If Not IsDBNull(dtMain.Rows(i)("WrappingDate")) Then
                drow("hidWrappingDate") = Format(CDate(dtMain.Rows(i)("WrappingDate")), "d MMM yyyy")
            End If
            If Not IsDBNull(dtMain.Rows(i)("WrappingStatus")) Then
                If CInt(dtMain.Rows(i)("WrappingStatus")) = 0 Then
                    drow("Wrapping") = False
                Else
                    drow("Wrapping") = True
                End If
            Else
                drow("Wrapping") = False
            End If

            'Uncommented by Michael Behar - Ticket #62859 - Code already existed - 27-11-2020
            drow("hidDespatchID") = dtMain.Rows(i)("DespatchID")
            drow("hidDespatchStatus") = dtMain.Rows(i)("DespatchStatus")
            If Not IsDBNull(dtMain.Rows(i)("DespatchDate")) Then
                drow("hidDespatchDate") = Format(CDate(dtMain.Rows(i)("DespatchDate")), "d MMM yyyy")
            End If
            If Not IsDBNull(dtMain.Rows(i)("DespatchStatus")) Then
                If CInt(dtMain.Rows(i)("DespatchStatus")) = 0 Then
                    drow("Despatch") = False
                Else
                    drow("Despatch") = True
                End If
            Else
                drow("Despatch") = False
            End If

            'Added for Pining  on 29/06/2017 by Kartar - Stage 8
            drow("hidPiningID") = dtMain.Rows(i)("PiningID")
            drow("hidPiningStatus") = dtMain.Rows(i)("PiningStatus")
            If Not IsDBNull(dtMain.Rows(i)("PiningDate")) Then
                drow("hidPiningDate") = Format(CDate(dtMain.Rows(i)("PiningDate")), "d MMM yyyy")
            End If
            If Not IsDBNull(dtMain.Rows(i)("PiningStatus")) Then
                If CInt(dtMain.Rows(i)("PiningStatus")) = 0 Then
                    drow("Pining") = False
                Else
                    drow("Pining") = True
                End If
            Else
                drow("Pining") = False
            End If

            '--Stage 9  - Hinging
            drow("hidHingingID") = dtMain.Rows(i)("HingingID")
            drow("hidHingingStatus") = dtMain.Rows(i)("HingingStatus")
            If Not IsDBNull(dtMain.Rows(i)("HingingDate")) Then
                drow("hidHingingDate") = Format(CDate(dtMain.Rows(i)("HingingDate")), "d MMM yyyy")
            End If
            If Not IsDBNull(dtMain.Rows(i)("HingingStatus")) Then
                If CInt(dtMain.Rows(i)("HingingStatus")) = 0 Then
                    drow("Hinging") = False
                Else
                    drow("Hinging") = True
                End If
            Else
                drow("Hinging") = False
            End If

            '--Stage 10  - Packup
            drow("hidPackupID") = dtMain.Rows(i)("PackupID")
            drow("hidPackupStatus") = dtMain.Rows(i)("PackupStatus")
            If Not IsDBNull(dtMain.Rows(i)("PackupDate")) Then
                drow("hidPackupDate") = Format(CDate(dtMain.Rows(i)("PackupDate")), "d MMM yyyy")
            End If
            If Not IsDBNull(dtMain.Rows(i)("PackupStatus")) Then
                If CInt(dtMain.Rows(i)("PackupStatus")) = 0 Then
                    drow("Packup") = False
                Else
                    drow("Packup") = True
                End If
            Else
                drow("Packup") = False
            End If

            dt.Rows.Add(drow)
            drow = Nothing
        Next

        Return dt

    End Function

    Protected Function createDatatable() As DataTable

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "ScheduleID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ShutterProNumber"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "OrderType"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "BranchName"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ReferenceNumber"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ReferenceName"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "State"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ScheduledDate"
        col.DataType = System.Type.GetType("System.DateTime")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "PlannedShippingDate"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ExpectedShippingDate"
        col.DataType = System.Type.GetType("System.DateTime")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Priority"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "NoOfPanels"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "TotalSQM"
        col.DataType = System.Type.GetType("System.Decimal")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidCuttingID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidCuttingStatus"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidCuttingDate"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Cutting"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidPrepID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidPrepStatus"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidPrepDate"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Prep"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidAssemblyID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidAssemblyStatus"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidAssemblyDate"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Assembly"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidFramingID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidFramingStatus"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidFramingDate"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Framing"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidQCID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidQCStatus"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidQCDate"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "QC"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidWrappingID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidWrappingStatus"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidWrappingDate"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Wrapping"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        'Uncommented by Michael Behar - Ticket #62859 - Code already existed - 27-11-2020
        col = New DataColumn
        col.ColumnName = "hidDespatchID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidDespatchStatus"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidDespatchDate"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Despatch"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing
        'Finishes here for #62859

        'Added by Kartar on 29/06/2017  - Stage 8
        col = New DataColumn
        col.ColumnName = "hidPiningID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidPiningStatus"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidPiningDate"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Pining"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        '  Stage 9   
        col = New DataColumn
        col.ColumnName = "hidHingingID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidHingingStatus"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidHingingDate"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Hinging"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        '--------Stage 10
        col = New DataColumn
        col.ColumnName = "hidPackupID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidPackupStatus"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "hidPackupDate"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Packup"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing
        '----------------------------

        Return dt

    End Function

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
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

            Dim dt As DataTable = setJobStagesValues()

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim cJobStages As New JobStages
                cJobStages.JobStagesID = CInt(dt.Rows(i)("JobStagesID"))
                cJobStages.StageID = CInt(dt.Rows(i)("StageID"))
                cJobStages.StageStatus = CInt(dt.Rows(i)("StageStatus"))
                cJobStages.ScheduleID = CInt(dt.Rows(i)("ScheduleID"))
                If cJobStages.StageStatus = 3 Then
                    cJobStages.CompletedDateTime = CDate(Me.txtScheduledDate.Text)
                    'cJobStages.CompletedDateTime = DateTime.Today.Date
                Else
                    cJobStages.CompletedDateTime = SharedConstants.DEFAULT_DATE_VALUE
                End If

                If cJobStages.JobStagesID = SharedConstants.DEFAULT_INTEGER_VALUE Then
                    Dim intID As Integer = service.addJobStages(cJobStages, cnn, trans)
                    If intID = SharedConstants.DEFAULT_INTEGER_VALUE Then
                        bolContinue = False
                    End If
                    If bolContinue Then
                        bolContinue = service.addJobStagesHistoryRecord(cJobStages, CInt(Session("sessUserID")), cnn, trans)
                    End If
                Else
                    bolContinue = service.updateJobStages(cJobStages, cnn, trans)
                    If bolContinue Then
                        bolContinue = service.addJobStagesHistoryRecord(cJobStages, CInt(Session("sessUserID")), cnn, trans)
                    End If
                End If

                If bolContinue = False Then
                    Exit For
                End If
            Next

            'Uncommented by Michael Behar - Ticket #62859 - Code already existed - 27-11-2020
            'update jobs marked as despatched to despatch status
            If bolContinue Then
                Dim dtDespatch As DataTable = SharedFunctions.PerformDatatableSelect(dt, "StageID=7 and StageStatus=3")
                For i As Integer = 0 To dtDespatch.Rows.Count - 1
                    Dim cProductionSchedule As New ProductionSchedule
                    If bolContinue Then
                        cProductionSchedule = service.GetProdScheduleClsByID(CInt(dtDespatch.Rows(i)("ScheduleID")), cnn, trans)
                        Dim cNewProductionSchedule As ProductionSchedule = CType(cProductionSchedule.Clone, ProductionSchedule)
                        cNewProductionSchedule.OrderStatus = 4

                        bolContinue = service.UpdateProductionScheduleByID(cNewProductionSchedule, cnn, trans)
                    End If

                    If bolContinue Then
                        bolContinue = service.AddProdScheduleHistoryRcd(0, cProductionSchedule, cnn, trans)
                    End If

                    If bolContinue = False Then
                        Exit For
                    End If
                Next
            End If


            If bolContinue Then
                trans.Commit()
                Response.Redirect("DailyScheduleUpdate.aspx", False)
            Else
                EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - Transaction Rolled Back" & Environment.NewLine & getPageInfo())
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

        service = Nothing

    End Sub

    Protected Function setJobStagesValues() As DataTable

        Dim intStageCompleted As Integer = 3
        Dim intStageNotStarted As Integer = 0

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "AutoID"
        col.DataType = System.Type.GetType("System.Int32")
        col.AutoIncrement = True
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ScheduleID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "JobStagesID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "StageID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "StageStatus"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing


        For Each gvr As GridViewRow In Me.dgvScheduleList.Rows
            Dim drow As DataRow

            Dim intID As Integer = CInt(dgvScheduleList.DataKeys(gvr.RowIndex).Value)

            'cutting stage checks
            Dim chk As CheckBox = DirectCast(gvr.FindControl("chkCutting"), CheckBox)
            Dim hfStatus As HiddenField = DirectCast(gvr.FindControl("hidCuttingStatus"), HiddenField)
            Dim hfID As HiddenField = DirectCast(gvr.FindControl("hidCuttingID"), HiddenField)
            If hfID.Value <> String.Empty Then
                If hfStatus.Value <> 3 Then
                    If chk.Checked Then
                        'update to completed
                        drow = dt.NewRow
                        drow("ScheduleID") = intID
                        drow("JobStagesID") = hfID.Value
                        drow("StageID") = 1
                        drow("StageStatus") = intStageCompleted
                        dt.Rows.Add(drow)
                        drow = Nothing
                    End If
                Else
                    If chk.Checked = False Then
                        'update to not started
                        drow = dt.NewRow
                        drow("ScheduleID") = intID
                        drow("JobStagesID") = hfID.Value
                        drow("StageID") = 1
                        drow("StageStatus") = intStageNotStarted
                        dt.Rows.Add(drow)
                        drow = Nothing
                    End If
                End If
            Else
                'add new record 
                drow = dt.NewRow
                drow("ScheduleID") = intID
                drow("JobStagesID") = SharedConstants.DEFAULT_INTEGER_VALUE
                drow("StageID") = 1
                If chk.Checked = True Then
                    drow("StageStatus") = intStageCompleted
                Else
                    drow("StageStatus") = intStageNotStarted
                End If                
                dt.Rows.Add(drow)
                drow = Nothing
            End If

            'prep stage checks
            chk = DirectCast(gvr.FindControl("chkPrep"), CheckBox)
            hfStatus = DirectCast(gvr.FindControl("hidPrepStatus"), HiddenField)
            hfID = DirectCast(gvr.FindControl("hidPrepID"), HiddenField)
            If hfID.Value <> String.Empty Then
                If hfStatus.Value <> 3 Then
                    If chk.Checked Then
                        'update to completed
                        drow = dt.NewRow
                        drow("ScheduleID") = intID
                        drow("JobStagesID") = hfID.Value
                        drow("StageID") = 2
                        drow("StageStatus") = intStageCompleted
                        dt.Rows.Add(drow)
                        drow = Nothing
                    End If
                Else
                    If chk.Checked = False Then
                        'update to not started
                        drow = dt.NewRow
                        drow("ScheduleID") = intID
                        drow("JobStagesID") = hfID.Value
                        drow("StageID") = 2
                        drow("StageStatus") = intStageNotStarted
                        dt.Rows.Add(drow)
                        drow = Nothing
                    End If
                End If
            Else
                'add new record 
                drow = dt.NewRow
                drow("ScheduleID") = intID
                drow("JobStagesID") = SharedConstants.DEFAULT_INTEGER_VALUE
                drow("StageID") = 2
                If chk.Checked = True Then
                    drow("StageStatus") = intStageCompleted
                Else
                    drow("StageStatus") = intStageNotStarted
                End If
                dt.Rows.Add(drow)
                drow = Nothing
            End If

            'assembly stage checks
            chk = DirectCast(gvr.FindControl("chkAssembly"), CheckBox)
            hfStatus = DirectCast(gvr.FindControl("hidAssemblyStatus"), HiddenField)
            hfID = DirectCast(gvr.FindControl("hidAssemblyID"), HiddenField)
            If hfID.Value <> String.Empty Then
                If hfStatus.Value <> 3 Then
                    If chk.Checked Then
                        'update to completed
                        drow = dt.NewRow
                        drow("ScheduleID") = intID
                        drow("JobStagesID") = hfID.Value
                        drow("StageID") = 3
                        drow("StageStatus") = intStageCompleted
                        dt.Rows.Add(drow)
                        drow = Nothing
                    End If
                Else
                    If chk.Checked = False Then
                        'update to not started
                        drow = dt.NewRow
                        drow("ScheduleID") = intID
                        drow("JobStagesID") = hfID.Value
                        drow("StageID") = 3
                        drow("StageStatus") = intStageNotStarted
                        dt.Rows.Add(drow)
                        drow = Nothing
                    End If
                End If
            Else
                'add new record 
                drow = dt.NewRow
                drow("ScheduleID") = intID
                drow("JobStagesID") = SharedConstants.DEFAULT_INTEGER_VALUE
                drow("StageID") = 3
                If chk.Checked = True Then
                    drow("StageStatus") = intStageCompleted
                Else
                    drow("StageStatus") = intStageNotStarted
                End If
                dt.Rows.Add(drow)
                drow = Nothing
            End If

            'qc stage checks
            chk = DirectCast(gvr.FindControl("chkQC"), CheckBox)
            hfStatus = DirectCast(gvr.FindControl("hidQCStatus"), HiddenField)
            hfID = DirectCast(gvr.FindControl("hidQCID"), HiddenField)
            If hfID.Value <> String.Empty Then
                If hfStatus.Value <> 3 Then
                    If chk.Checked Then
                        'update to completed
                        drow = dt.NewRow
                        drow("ScheduleID") = intID
                        drow("JobStagesID") = hfID.Value
                        drow("StageID") = 5
                        drow("StageStatus") = intStageCompleted
                        dt.Rows.Add(drow)
                        drow = Nothing
                    End If
                Else
                    If chk.Checked = False Then
                        'update to not started
                        drow = dt.NewRow
                        drow("ScheduleID") = intID
                        drow("JobStagesID") = hfID.Value
                        drow("StageID") = 5
                        drow("StageStatus") = intStageNotStarted
                        dt.Rows.Add(drow)
                        drow = Nothing
                    End If
                End If
            Else
                'add new record 
                drow = dt.NewRow
                drow("ScheduleID") = intID
                drow("JobStagesID") = SharedConstants.DEFAULT_INTEGER_VALUE
                drow("StageID") = 5
                If chk.Checked = True Then
                    drow("StageStatus") = intStageCompleted
                Else
                    drow("StageStatus") = intStageNotStarted
                End If
                dt.Rows.Add(drow)
                drow = Nothing
            End If

            'If condition Added on 29/06/2017 for Framing & Wrapping
            If Me.txtProductTypeID.Text <> "2" Then   ' Check for Framing & Wrapping
                'framing stage checks
                chk = DirectCast(gvr.FindControl("chkFraming"), CheckBox)
                hfStatus = DirectCast(gvr.FindControl("hidFramingStatus"), HiddenField)
                hfID = DirectCast(gvr.FindControl("hidFramingID"), HiddenField)
                If hfID.Value <> String.Empty Then
                    If hfStatus.Value <> 3 Then
                        If chk.Checked Then
                            'update to completed
                            drow = dt.NewRow
                            drow("ScheduleID") = intID
                            drow("JobStagesID") = hfID.Value
                            drow("StageID") = 4
                            drow("StageStatus") = intStageCompleted
                            dt.Rows.Add(drow)
                            drow = Nothing
                        End If
                    Else
                        If chk.Checked = False Then
                            'update to not started
                            drow = dt.NewRow
                            drow("ScheduleID") = intID
                            drow("JobStagesID") = hfID.Value
                            drow("StageID") = 4
                            drow("StageStatus") = intStageNotStarted
                            dt.Rows.Add(drow)
                            drow = Nothing
                        End If
                    End If
                Else
                    'add new record 
                    drow = dt.NewRow
                    drow("ScheduleID") = intID
                    drow("JobStagesID") = SharedConstants.DEFAULT_INTEGER_VALUE
                    drow("StageID") = 4
                    If chk.Checked = True Then
                        drow("StageStatus") = intStageCompleted
                    Else
                        drow("StageStatus") = intStageNotStarted
                    End If
                    dt.Rows.Add(drow)
                    drow = Nothing
                End If

                'qc stage checks
                chk = DirectCast(gvr.FindControl("chkWrapping"), CheckBox)
                hfStatus = DirectCast(gvr.FindControl("hidWrappingStatus"), HiddenField)
                hfID = DirectCast(gvr.FindControl("hidWrappingID"), HiddenField)
                If hfID.Value <> String.Empty Then
                    If hfStatus.Value <> 3 Then
                        If chk.Checked Then
                            'update to completed
                            drow = dt.NewRow
                            drow("ScheduleID") = intID
                            drow("JobStagesID") = hfID.Value
                            drow("StageID") = 6
                            drow("StageStatus") = intStageCompleted
                            dt.Rows.Add(drow)
                            drow = Nothing
                        End If
                    Else
                        If chk.Checked = False Then
                            'update to not started
                            drow = dt.NewRow
                            drow("ScheduleID") = intID
                            drow("JobStagesID") = hfID.Value
                            drow("StageID") = 6
                            drow("StageStatus") = intStageNotStarted
                            dt.Rows.Add(drow)
                            drow = Nothing
                        End If
                    End If
                Else
                    'add new record 
                    drow = dt.NewRow
                    drow("ScheduleID") = intID
                    drow("JobStagesID") = SharedConstants.DEFAULT_INTEGER_VALUE
                    drow("StageID") = 6
                    If chk.Checked = True Then
                        drow("StageStatus") = intStageCompleted
                    Else
                        drow("StageStatus") = intStageNotStarted
                    End If
                    dt.Rows.Add(drow)
                    drow = Nothing
                End If
            End If     'If Me.txtProductTypeID.Text <> "2" Then   ' Check for Framing & Wrapping
            'If condition Added on 29/06/2017 for Framing & Wrapping


            'Uncommented by Michael Behar - Ticket #62859 - Code already existed - 27-11-2020
            'despatch stage checks
            chk = DirectCast(gvr.FindControl("chkDespatch"), CheckBox)
            hfStatus = DirectCast(gvr.FindControl("hidDespatchStatus"), HiddenField)
            hfID = DirectCast(gvr.FindControl("hidDespatchID"), HiddenField)
            If hfID.Value <> String.Empty Then
                If hfStatus.Value <> 3 Then
                    If chk.Checked Then
                        'update to completed
                        drow = dt.NewRow
                        drow("ScheduleID") = intID
                        drow("JobStagesID") = hfID.Value
                        drow("StageID") = 7
                        drow("StageStatus") = intStageCompleted
                        dt.Rows.Add(drow)
                        drow = Nothing
                    End If
                Else
                    If chk.Checked = False Then
                        'update to not started
                        drow = dt.NewRow
                        drow("ScheduleID") = intID
                        drow("JobStagesID") = hfID.Value
                        drow("StageID") = 7
                        drow("StageStatus") = intStageNotStarted
                        dt.Rows.Add(drow)
                        drow = Nothing
                    End If
                End If
            Else
                'add new record 
                drow = dt.NewRow
                drow("ScheduleID") = intID
                drow("JobStagesID") = SharedConstants.DEFAULT_INTEGER_VALUE
                drow("StageID") = 7
                If chk.Checked = True Then
                    drow("StageStatus") = intStageCompleted
                Else
                    drow("StageStatus") = intStageNotStarted
                End If
                dt.Rows.Add(drow)
                drow = Nothing
            End If

            'Added on 29/06/2017 for 3 new stages  -8,9 & 10
            If Me.txtProductTypeID.Text <> "1" Then
                'Pinging stage checks
                chk = DirectCast(gvr.FindControl("chkPining"), CheckBox)
                hfStatus = DirectCast(gvr.FindControl("hidPiningStatus"), HiddenField)
                hfID = DirectCast(gvr.FindControl("hidPiningID"), HiddenField)
                If hfID.Value <> String.Empty Then
                    If hfStatus.Value <> 3 Then
                        If chk.Checked Then
                            'update to completed
                            drow = dt.NewRow
                            drow("ScheduleID") = intID
                            drow("JobStagesID") = hfID.Value
                            drow("StageID") = 8
                            drow("StageStatus") = intStageCompleted
                            dt.Rows.Add(drow)
                            drow = Nothing
                        End If
                    Else
                        If chk.Checked = False Then
                            'update to not started
                            drow = dt.NewRow
                            drow("ScheduleID") = intID
                            drow("JobStagesID") = hfID.Value
                            drow("StageID") = 8
                            drow("StageStatus") = intStageNotStarted
                            dt.Rows.Add(drow)
                            drow = Nothing
                        End If
                    End If
                Else
                    'add new record 
                    drow = dt.NewRow
                    drow("ScheduleID") = intID
                    drow("JobStagesID") = SharedConstants.DEFAULT_INTEGER_VALUE
                    drow("StageID") = 8
                    If chk.Checked = True Then
                        drow("StageStatus") = intStageCompleted
                    Else
                        drow("StageStatus") = intStageNotStarted
                    End If
                    dt.Rows.Add(drow)
                    drow = Nothing
                End If

                'Hinging stage checks - Stage 9
                chk = DirectCast(gvr.FindControl("chkHinging"), CheckBox)
                hfStatus = DirectCast(gvr.FindControl("hidHingingStatus"), HiddenField)
                hfID = DirectCast(gvr.FindControl("hidHingingID"), HiddenField)
                If hfID.Value <> String.Empty Then
                    If hfStatus.Value <> 3 Then
                        If chk.Checked Then
                            'update to completed
                            drow = dt.NewRow
                            drow("ScheduleID") = intID
                            drow("JobStagesID") = hfID.Value
                            drow("StageID") = 9
                            drow("StageStatus") = intStageCompleted
                            dt.Rows.Add(drow)
                            drow = Nothing
                        End If
                    Else
                        If chk.Checked = False Then
                            'update to not started
                            drow = dt.NewRow
                            drow("ScheduleID") = intID
                            drow("JobStagesID") = hfID.Value
                            drow("StageID") = 9
                            drow("StageStatus") = intStageNotStarted
                            dt.Rows.Add(drow)
                            drow = Nothing
                        End If
                    End If
                Else
                    'add new record 
                    drow = dt.NewRow
                    drow("ScheduleID") = intID
                    drow("JobStagesID") = SharedConstants.DEFAULT_INTEGER_VALUE
                    drow("StageID") = 9
                    If chk.Checked = True Then
                        drow("StageStatus") = intStageCompleted
                    Else
                        drow("StageStatus") = intStageNotStarted
                    End If
                    dt.Rows.Add(drow)
                    drow = Nothing
                End If

                'Packup stage checks - Stage 10
                chk = DirectCast(gvr.FindControl("chkPackup"), CheckBox)
                hfStatus = DirectCast(gvr.FindControl("hidPackupStatus"), HiddenField)
                hfID = DirectCast(gvr.FindControl("hidPackupID"), HiddenField)
                If hfID.Value <> String.Empty Then
                    If hfStatus.Value <> 3 Then
                        If chk.Checked Then
                            'update to completed
                            drow = dt.NewRow
                            drow("ScheduleID") = intID
                            drow("JobStagesID") = hfID.Value
                            drow("StageID") = 10
                            drow("StageStatus") = intStageCompleted
                            dt.Rows.Add(drow)
                            drow = Nothing
                        End If
                    Else
                        If chk.Checked = False Then
                            'update to not started
                            drow = dt.NewRow
                            drow("ScheduleID") = intID
                            drow("JobStagesID") = hfID.Value
                            drow("StageID") = 10
                            drow("StageStatus") = intStageNotStarted
                            dt.Rows.Add(drow)
                            drow = Nothing
                        End If
                    End If
                Else
                    'add new record 
                    drow = dt.NewRow
                    drow("ScheduleID") = intID
                    drow("JobStagesID") = SharedConstants.DEFAULT_INTEGER_VALUE
                    drow("StageID") = 10
                    If chk.Checked = True Then
                        drow("StageStatus") = intStageCompleted
                    Else
                        drow("StageStatus") = intStageNotStarted
                    End If
                    dt.Rows.Add(drow)
                    drow = Nothing
                End If

            End If        'Me.txtProductTypeID.Text <> "1" Then
            'Added on 29/06/2017 for 3 new stages  -8,9 & 10

        Next

        Return dt

    End Function

    Protected Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click

        Dim dt As DataTable = loadDatatable()

        Dim strRptsPath As String = Server.MapPath("") & "\ExcelRpts"
        Dim objBuffer As Byte() = Nothing
        'ExcelReport.createReport(strRptsPath, objBuffer, dtScheduleList)
        'Dim rptFileName As String = DailyScheduleExport.generateDailyScheduleExport(strRptsPath, objBuffer, dt, Today.Date)
        Dim rptFileName As String
        If txtProductTypeID.Text = "1" Then
            rptFileName = LouvreDailyScheduleExport.generateDailyScheduleExport(strRptsPath, objBuffer, dt, Today.Date)
        Else
            rptFileName = DailyScheduleExport.generateDailyScheduleExport(strRptsPath, objBuffer, dt, Today.Date)
        End If

        Dim strRPTFileName As String = IO.Path.Combine(strRptsPath, rptFileName)
        If objBuffer IsNot Nothing Then
            '----            
            Response.BinaryWrite(objBuffer)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;  filename=" & rptFileName)
            Response.End()
            '
        End If
        objBuffer = Nothing



    End Sub

    Protected Sub txtScheduledDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtScheduledDate.TextChanged

        If Me.txtScheduledDate.Text <> String.Empty Then

            If IsDate(Me.txtScheduledDate.Text) Then

                Dim dt As DataTable = loadDatatable()

                Me.dgvScheduleList.DataSource = dt
                Me.dgvScheduleList.DataBind()
                Me.pnlDetails.Visible = True

            Else

                Me.dgvScheduleList.DataSource = Nothing
                Me.dgvScheduleList.DataBind()
                Me.pnlDetails.Visible = False

            End If

        Else

            Me.dgvScheduleList.DataSource = Nothing
            Me.dgvScheduleList.DataBind()
            Me.pnlDetails.Visible = False


        End If

    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click

        Response.Redirect("Home.aspx", False)

    End Sub

    Protected Sub btnHome_Click(sender As Object, e As System.EventArgs) Handles btnHome.Click

        Response.Redirect("Home.aspx", False)

    End Sub
End Class
