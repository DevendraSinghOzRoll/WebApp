﻿
Partial Class StockPick
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

            Dim service As New AppService
            service.addWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)
            service = Nothing
            txtJobNo.Text = Session("JobNumber")

            LoadGrid()
            LoadJobData(2415)
            LoadPowderCoatData()

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



    Protected Function createDatatable() As DataTable

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "ProductCode"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ProductName"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ReqQuantity"
        col.DataType = System.Type.GetType("System.Decimal")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "QuantityPicked"
        col.DataType = System.Type.GetType("System.Decimal")
        dt.Columns.Add(col)
        col = Nothing

        'Only for testing Purpose
        Dim dRow As DataRow = dt.NewRow
        dRow("ProductCode") = "0"
        dRow("ProductName") = "Bottom Track 28mm (2900)"
        dRow("ReqQuantity") = "1"
        dt.Rows.Add(dRow)
        dRow = Nothing

        dRow = dt.NewRow
        dRow("ProductCode") = "0"
        dRow("ProductName") = "DLi 20mm Trim Infill (3650)"
        dRow("ReqQuantity") = "1"
        dt.Rows.Add(dRow)
        dRow = Nothing

        dRow = dt.NewRow
        dRow("ProductCode") = "0"
        dRow("ProductName") = "DLi 35mm Trim Infill (3650)"
        dRow("ReqQuantity") = "1"
        dt.Rows.Add(dRow)
        dRow = Nothing

        dRow = dt.NewRow
        dRow("ProductCode") = "0"
        dRow("ProductName") = "DLi Blade Infill (6500)"
        dRow("ReqQuantity") = "1"
        dt.Rows.Add(dRow)
        dRow = Nothing

        dRow = dt.NewRow
        dRow("ProductCode") = "0"
        dRow("ProductName") = "DLi Rail (6500)"
        dRow("ReqQuantity") = "1"
        dt.Rows.Add(dRow)
        dRow = Nothing

        dRow = dt.NewRow
        dRow("ProductCode") = "0"
        dRow("ProductName") = "DLi Stile (6500)"
        dRow("ReqQuantity") = "2"
        dt.Rows.Add(dRow)
        dRow = Nothing

        dRow = dt.NewRow
        dRow("ProductCode") = "0"
        dRow("ProductName") = "DLi Top Track (3250)"
        dRow("ReqQuantity") = "1"
        dt.Rows.Add(dRow)
        dRow = Nothing

        dRow = dt.NewRow
        dRow("ProductCode") = "0"
        dRow("ProductName") = "Finger Grip T2012 (Angle) (6500)"
        dRow("ReqQuantity") = "2"
        dt.Rows.Add(dRow)
        dRow = Nothing

        dRow = dt.NewRow
        dRow("ProductCode") = "0"
        dRow("ProductName") = "SC90 Profile 94.SC100-05MF Fluted (5800)"
        dRow("ReqQuantity") = "6"
        dt.Rows.Add(dRow)
        dRow = Nothing

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
            Dim strSQL As String

            strSQL = "select * from tblStockUsage where StockUsageID=" + txtStockUsage.Text
            Dim dt As DataTable = service.runSQLScheduling(strSQL)

            'Dim stockPick As New StockPicking

            'For Each row As GridViewRow In dgvStockPick.Rows
            '    Dim newValue As String = DirectCast(row.FindControl("txtQuantity"), TextBox).Text

            '    If newValue = String.Empty Then
            '        Continue For   
            '    End If

            '    Dim strStockArticleLength As String = dt.Rows(0).Item("StockArticleLength").ToString()
            '    Dim strCostPrice As String = dt.Rows(0).Item("CostPrice").ToString()
            '    Dim strCostPriceGST As String = dt.Rows(0).Item("CostPriceGST").ToString()
            '    Dim strDateBookedOut As String = dt.Rows(0).Item("DateBookedOut").ToString()
            '    Dim strBookedOutBy As String = dt.Rows(0).Item("BookedOutBy").ToString()
            '    Dim strAdditionalRequirementsID As String = dt.Rows(0).Item("AdditionalRequirementsID").ToString()
            '    Dim strTransferToSybiz As String = dt.Rows(0).Item("TransferToSybiz").ToString()

            '    stockPick.StockUsageID = dt.Rows(0).Item("StockUsageID").ToString()
            '    stockPick.ScheduleID = dt.Rows(0).Item("ScheduleID").ToString()
            '    stockPick.DetailID = dt.Rows(0).Item("DetailID").ToString()
            '    stockPick.StockArticleID = dt.Rows(0).Item("StockArticleID").ToString()
            '    stockPick.StockArticleTypeID = dt.Rows(0).Item("StockArticleTypeID").ToString()
            '    stockPick.OptimiserQuantity = newValue
            '    stockPick.ActualQuantity = SharedConstants.DEFAULT_DECIMAL_VALUE

            '    If strStockArticleLength = String.Empty Then
            '        stockPick.StockArticleLength = SharedConstants.DEFAULT_INTEGER_VALUE
            '    Else
            '        stockPick.StockArticleLength = strStockArticleLength
            '    End If

            '    If strStockArticleLength = String.Empty Then
            '        stockPick.StockArticleLength = SharedConstants.DEFAULT_INTEGER_VALUE
            '    Else
            '        stockPick.StockArticleLength = strStockArticleLength
            '    End If

            '    If strCostPrice = String.Empty Then
            '        stockPick.CostPrice = SharedConstants.DEFAULT_DECIMAL_VALUE
            '    Else
            '        stockPick.CostPrice = strCostPrice
            '    End If

            '    If strCostPriceGST = String.Empty Then
            '        stockPick.CostPriceGST = SharedConstants.DEFAULT_DECIMAL_VALUE
            '    Else
            '        stockPick.CostPriceGST = strCostPriceGST
            '    End If

            '    If strCostPriceGST = String.Empty Then
            '        stockPick.CostPriceGST = SharedConstants.DEFAULT_DECIMAL_VALUE
            '    Else
            '        stockPick.CostPriceGST = strCostPriceGST
            '    End If

            '    If strDateBookedOut = String.Empty Then
            '        stockPick.DateBookedOut = SharedConstants.DEFAULT_DATE_VALUE
            '    Else
            '        stockPick.DateBookedOut = strDateBookedOut
            '    End If

            '    If strBookedOutBy = String.Empty Then
            '        stockPick.BookedOutBy = SharedConstants.DEFAULT_INTEGER_VALUE
            '    Else
            '        stockPick.BookedOutBy = strBookedOutBy
            '    End If

            '    If strAdditionalRequirementsID = String.Empty Then
            '        stockPick.AdditionalRequirementsID = SharedConstants.DEFAULT_INTEGER_VALUE
            '    Else
            '        stockPick.AdditionalRequirementsID = strAdditionalRequirementsID
            '    End If

            '    If strTransferToSybiz = String.Empty Then
            '        stockPick.TransferToSybiz = SharedConstants.DEFAULT_DATE_VALUE
            '    Else
            '        stockPick.TransferToSybiz = strTransferToSybiz
            '    End If

            '    If service.updateStockPick(stockPick) = True Then
            '        lblStatus.Text = "Update Successful"
            '        LoadGrid()
            '    End If

            '    newValue = String.Empty

            'Next

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


        For Each gvr As GridViewRow In Me.dgvStockPick.Rows
            Dim drow As DataRow

            Dim intID As Integer = CInt(dgvStockPick.DataKeys(gvr.RowIndex).Value)

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


            ''despatch stage checks
            'chk = DirectCast(gvr.FindControl("chkDespatch"), CheckBox)
            'hfStatus = DirectCast(gvr.FindControl("hidDespatchStatus"), HiddenField)
            'hfID = DirectCast(gvr.FindControl("hidDespatchID"), HiddenField)
            'If hfID.Value <> String.Empty Then
            '    If hfStatus.Value <> 3 Then
            '        If chk.Checked Then
            '            'update to completed
            '            drow = dt.NewRow
            '            drow("ScheduleID") = intID
            '            drow("JobStagesID") = hfID.Value
            '            drow("StageID") = 7
            '            drow("StageStatus") = intStageCompleted
            '            dt.Rows.Add(drow)
            '            drow = Nothing
            '        End If
            '    Else
            '        If chk.Checked = False Then
            '            'update to not started
            '            drow = dt.NewRow
            '            drow("ScheduleID") = intID
            '            drow("JobStagesID") = hfID.Value
            '            drow("StageID") = 7
            '            drow("StageStatus") = intStageNotStarted
            '            dt.Rows.Add(drow)
            '            drow = Nothing
            '        End If
            '    End If
            'Else
            '    'add new record 
            '    drow = dt.NewRow
            '    drow("ScheduleID") = intID
            '    drow("JobStagesID") = SharedConstants.DEFAULT_INTEGER_VALUE
            '    drow("StageID") = 7
            '    If chk.Checked = True Then
            '        drow("StageStatus") = intStageCompleted
            '    Else
            '        drow("StageStatus") = intStageNotStarted
            '    End If
            '    dt.Rows.Add(drow)
            '    drow = Nothing
            'End If

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


    Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Home.aspx", False)
    End Sub

    Protected Sub btnHome_Click(sender As Object, e As System.EventArgs) Handles btnHome.Click
        Response.Redirect("Home.aspx", False)
    End Sub

    Private Sub LoadJobData(JobNumber As Integer)
        Dim service As New AppService
        Dim bolContinue As Boolean = True

        Dim dbConn As New DBConnection
        Dim cnn As SqlClient.SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim strSQL As String = "select pro.JobNumber,cus.CustomerName,pro.CustomerName as CusReference,pro.CustomerID,pro.TotalPanels,"
        strSQL += "pro.OrderReference,pro.OzrollContractNo,cast(cast(pro.TotalSQM as decimal(10,2)) as float) as TotalSQM,pro.ShutterProNumber,"
        strSQL += "ord.OrderDescription as OrderTypeID from tblProductionScheduleList"
        strSQL += " pro inner join Tblcustomers cus on cus.customerid = pro.customerID "
        strSQL += " inner join TblOrderType ord on ord.OrdertypeID = pro.OrderTypeID  where ID=" & JobNumber
        Dim dt As DataTable = service.runSQLScheduling(strSQL)
        '       dt = service.getPlantationJobDetailsRecordsByPlantationScheduleID(231)
        dbConn = Nothing

        If dt.Rows.Count() > 0 Then
            lblCustomer.Text = dt.Rows(0).Item("CustomerName").ToString
            lblCusReference.Text = dt.Rows(0).Item("CusReference").ToString
            lblPanels.Text = dt.Rows(0).Item("TotalPanels").ToString
            lblOrderRef.Text = dt.Rows(0).Item("OrderReference").ToString
            lblTotalSQM.Text = dt.Rows(0).Item("TotalSQM").ToString
            lblOrderType.Text = dt.Rows(0).Item("OrderTypeID").ToString
            'lblOzrollID.Text = dt.Rows(0).Item("ShutterProNumber")
            lblHeader.Text = dt.Rows(0).Item("OzrollContractNo").ToString & " - Pick List For Powdercoat"
        End If
        dt = Nothing
    End Sub
    Protected Sub btnAddStock_Click(sender As Object, e As EventArgs) Handles btnAddStock.Click
        Dim dtStock As DataTable
        Dim service As New AppService
        Dim dbConn As New DBConnection
        Dim cnn As SqlClient.SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim strSQL As String = "select * from tblStockArticlesPrices"
        dtStock = service.runSQLScheduling(strSQL)
        dbConn = Nothing
        SharedFunctions.fillDropDownList(dtStock, "StockPricesID", "ArticleID", cboStockItem, False, True)

        resetPopupControlsToDefault()
        ModalPopupExtender.Show()
    End Sub
    Protected Sub resetPopupControlsToDefault()
        txtQuantityBookOut.Text = String.Empty
        cboStockItem.SelectedIndex = -1
    End Sub
    Protected Function CreateAutoDatatable(Columns() As String, Types() As String) As DataTable

        Dim dt As DataTable = New DataTable
        Dim col As DataColumn
        For i = 0 To Columns.Length - 1
            col = New DataColumn
            col.ColumnName = Columns(i)
            col.DataType = System.Type.GetType(Types(i))
            dt.Columns.Add(col)
            col = Nothing
        Next

        Dim drow As DataRow = dt.NewRow
        drow(Columns(0)) = 0
        drow(Columns(1)) = "-Select Please-"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow(Columns(0)) = 1
        drow(Columns(1)) = "Order"
        dt.Rows.Add(drow)
        drow = Nothing


        drow = dt.NewRow
        drow("OrderTypeID") = 2
        drow("OrderType") = "Remake"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("OrderTypeID") = 3
        drow("OrderType") = "Reorder"
        dt.Rows.Add(drow)
        drow = Nothing


        'Added by Michael Behar - Ticket #66172  - 05/07/2021
        drow = dt.NewRow
        drow("OrderTypeID") = 4
        drow("OrderType") = "Samples"
        dt.Rows.Add(drow)
        drow = Nothing


        Return dt

    End Function
    Protected Function CreateStockDatatable() As DataTable

        Dim dt As DataTable = New DataTable
        Dim col As DataColumn

        col = New DataColumn
        col.ColumnName = "ProcessID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ProcessDescription"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing


        Dim drow As DataRow = dt.NewRow
        drow("ProcessID") = 0
        drow("ProcessDescription") = "-Select Please-"
        dt.Rows.Add(drow)
        drow = Nothing

        drow = dt.NewRow
        drow("ProcessID") = 1
        drow("ProcessDescription") = " Powdercoating"
        dt.Rows.Add(drow)
        drow = Nothing


        drow = dt.NewRow
        drow("ProcessID") = 2
        drow("ProcessDescription") = " Assembly"
        dt.Rows.Add(drow)
        drow = Nothing
        Return dt

    End Function
    Protected Sub cboProcess_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProcess.SelectedIndexChanged
        If cboProcess.SelectedIndex = 0 Or cboProcess.SelectedIndex = 2 Then
            pnlBottom.Visible = False
            txtPickDate.Text = String.Empty
            txtReturnDate.Text = String.Empty
            txtDespatchDate.Text = String.Empty
        Else
            pnlBottom.Visible = True
            'lblPanelBottom.Text = "Details of " + cboProcess.SelectedItem.ToString
        End If
    End Sub
    Protected Sub cboProduct_SelectedIndexChanged(sender As Object, e As EventArgs)

        Dim gvr As GridViewRow = DirectCast(DirectCast(sender, Control).NamingContainer, GridViewRow)
        Dim intRow As Integer = CInt(dgvStockPick.DataKeys(gvr.RowIndex).Values("ProductCode"))

        Dim row As GridViewRow
        row = Me.dgvStockPick.Rows(intRow - 1)

        If Not IsNothing(row) Then
            Dim Index As Integer = row.RowIndex
        End If

    End Sub

    Private Sub LoadGrid()

        Dim service As New AppService
        Dim bolContinue As Boolean = True

        txtStockUsage.Text = "2"

        Dim dbConn As New DBConnection
        Dim cnn As SqlClient.SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim strSQL As String = "select usg.StockUsageID, art.[Description], usg.OptimiserQuantity, usg.ActualQuantity from tblStockUsage usg inner join tblStockArticles art on usg.StockArticleID=art.StockArticleID where StockUsageID=" & txtStockUsage.Text

        Dim dt As DataTable = service.runSQLScheduling(strSQL)
        dgvStockPick.DataSource = dt
        dgvStockPick.DataBind()

        dbConn = Nothing


    End Sub


    Private Sub LoadPowderCoatData()

        Dim service As New AppService
        Dim dtStyle1 As DataTable = service.runSQLScheduling("Select PowdercoaterID, PowdercoaterName from tblPowdercoater")
        'Dim dtStyle1 As DataTable = service.runSQLScheduling("Select AdditionalRequirementsID, PowdercoaterName, DescriptionText, StartDate, ETADate, CompleteDate from tblAdditionalRequirements adreq inner join tblPowdercoater pwdr on adreq.PowderCoaterID=pwdr.PowderCoaterID where ProductionScheduleID=2191")
        'Dim drowStyle As DataRow = dtStyle1.NewRow
        'drowStyle("StyleID") = 0
        'drowStyle("StyleName") = ""
        'dtStyle1.Rows.InsertAt(drowStyle, 0)
        Me.cboPC.DataSource = dtStyle1
        Me.cboPC.DataValueField = "PowdercoaterID"
        Me.cboPC.DataTextField = "PowdercoaterName"
        Me.cboPC.DataBind()
        Me.cboPC.SelectedIndex = 0

        dtStyle1 = Nothing
    End Sub

    Protected Sub cboPC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPC.SelectedIndexChanged

        Dim service As New AppService

        Dim dtStyle1 As DataTable = service.runSQLScheduling("Select AdditionalRequirementsID, PowdercoaterName, DescriptionText, StartDate, ETADate, CompleteDate from tblAdditionalRequirements adreq inner join tblPowdercoater pwdr on adreq.PowderCoaterID=pwdr.PowderCoaterID where ProductionScheduleID=2191")

        txtPickDate.Text = dtStyle1.Rows(0).Item("CompletedDate").ToString()
        txtReturnDate.Text = dtStyle1.Rows(0).Item("ETADate").ToString()
        txtDespatchDate.Text = dtStyle1.Rows(0).Item("StartDate").ToString()

    End Sub
    Protected Sub AddNewRow()
        If Not IsNothing(ViewState("StockPicked")) Then
            Dim dtCurrentTable As DataTable = CType(ViewState("StockPicked"), DataTable)
            Dim dRow As DataRow = dtCurrentTable.NewRow

            dRow("ProductCode") = ""
            dRow("ProductName") = ""
            dRow("ReqQuantity") = 0

            dtCurrentTable.Rows.Add(dRow)
            dRow = Nothing
            ViewState("StockPicked") = dtCurrentTable
            dgvStockPick.DataSource = dtCurrentTable
            dgvStockPick.DataBind()
        End If

    End Sub
    'Protected Sub btnAddStockNew_Click(sender As Object, e As EventArgs) Handles btnAddStockNew.Click
    '    AddNewRow()
    'End Sub

    Protected Sub btnAddNewRow_Click(sender As Object, e As EventArgs)

    End Sub

End Class
