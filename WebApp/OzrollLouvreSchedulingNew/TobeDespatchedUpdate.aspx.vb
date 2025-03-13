Imports OzrollPSLVSchedulingModel.SharedEnums
Imports OzrollPSLVSchedulingModel.SharedConstants
Imports System.IO

Partial Class TobeDespatchedUpdate
    Inherits System.Web.UI.Page

    Dim dtToBeDespatched As DataTable
    Dim dtCompletedDespatchList As DataTable

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

            Me.txtProductTypeID.Text = ProductType.Louvres

            Dim service As New AppService
            service.AddWebsitePageAccess("Ozroll Louvres Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)
            service = Nothing

            Me.txtDespatchDate.Text = Format(DateTime.Now, "d MMM yyyy")

            GetToBeDespatchedList()
            GetCompletedDespatchList()

            Dim dt As DataTable = loadDatatable(False)
            Me.dgvDespatchedList.DataSource = dt
            Me.dgvDespatchedList.DataBind()

        End If

        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "styleFocusElements", "styleFocusElements();", True)

    End Sub

    Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        SharedFunctions.PageError(Page, form1)
    End Sub

    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click

        Response.Redirect("Logout.aspx", False)

    End Sub

    Protected Function loadDatatable(Optional bolLoadCompleted As Boolean = False) As DataTable

        Dim service As New AppService
        Dim dt As DataTable = createDatatable()
        Dim dtMain As DataTable = New DataTable

        If Not bolLoadCompleted Then
            dtMain = GetToBeDespatchedList()
        Else
            dtMain = GetCompletedDespatchList()
        End If

        For i As Integer = 0 To dtMain.Rows.Count - 1
            Dim drow As DataRow = dt.NewRow

            drow("ScheduleID") = dtMain.Rows(i)("ID")
            drow("BranchName") = dtMain.Rows(i)("BranchName")
            drow("ReferenceNumber") = dtMain.Rows(i)("OrderReference")
            drow("ReferenceName") = dtMain.Rows(i)("CustomerName")
            drow("NoOfPanels") = dtMain.Rows(i)("TotalPanels")
            drow("TotalSQM") = dtMain.Rows(i)("TotalSQM")

            drow("hidDespatchID") = dtMain.Rows(i)("DespatchID")
            drow("hidDespatchStatus") = dtMain.Rows(i)("DespatchStatus")
            drow("hidDespatchDate") = dtMain.Rows(i)("DespatchDate")
            drow("Despatch") = False
            If Not IsDBNull(dtMain.Rows(i)("DespatchStatus")) Then
                If CInt(dtMain.Rows(i)("DespatchStatus")) > 0 Then
                    drow("Despatch") = True
                End If
            End If

            Dim cCustomer As Customer = service.GetCustomerByID(dtMain.Rows(i)("CustomeriD"))

            If cCustomer.CustomerID > 0 Then
                Dim cSybizCustomer As SybizCustomer = service.GetSybizCustomerDataByCode(cCustomer.Code)

                If cSybizCustomer.SybizCustomerID > 0 Then
                    drow("StopDeliveries") = cSybizCustomer.StopDeliveries
                Else
                    drow("StopDeliveries") = "ERROR: SYBIZ CUSTOMER NOT FOUND!"
                End If
            Else
                drow("StopDeliveries") = "ERROR: CUSTOMER NOT FOUND!"
            End If

            If bolLoadCompleted Then
                drow("ConNote") = dtMain.Rows(i)("ShippingDetails")
                dgvDespatchedList.Columns(6).Visible = False
                dgvDespatchedList.Columns(7).Visible = True
                dgvDespatchedList.Columns(8).Visible = True
                dgvDespatchedList.Columns(9).Visible = False
                dgvDespatchedList.Columns(10).Visible = True
            Else
                dgvDespatchedList.Columns(6).Visible = True
                dgvDespatchedList.Columns(7).Visible = False
                dgvDespatchedList.Columns(8).Visible = False
                dgvDespatchedList.Columns(9).Visible = True
                dgvDespatchedList.Columns(10).Visible = False
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
        col.DataType = System.Type.GetType("System.DateTime")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Despatch"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "StopDeliveries"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ConNote"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        Return dt

    End Function

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click

        If btnSave.Text = "Save" Then
            SaveDespatchAndConNote()
        Else
            UpdateConNote()
        End If

    End Sub

    Private Sub SaveDespatchAndConNote()

        Dim service As New OzrollPSLVSchedulingModel.AppService
        Dim bolContinue As Boolean = True

        btnSave.Enabled = False
        btnSave.CssClass = "form-button-disabled"
        lblStatus.Text = String.Empty

        'Commented Out Code - Ticket #62735
        'If txtConNote.Text = String.Empty Then
        '    lblStatus.Text = "Consignment Note Cannot Be Empty!"
        '    btnSave.Enabled = True
        '    btnSave.CssClass = "form-button"
        '    Exit Sub
        'End If

        Dim dbConn As New DBConnection
        Dim cnn As SqlClient.SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim trans As SqlClient.SqlTransaction = Nothing
        dbConn = Nothing

        Try

            cnn.Open()
            trans = cnn.BeginTransaction

            Dim lProdSchedulesUpdated As New List(Of ProductionSchedule)
            Dim dt As DataTable = setJobStagesValues()

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim cJobStages As New JobStages
                cJobStages.JobStagesID = CInt(dt.Rows(i)("JobStagesID"))
                cJobStages.StageID = CInt(dt.Rows(i)("StageID"))
                cJobStages.StageStatus = CInt(dt.Rows(i)("StageStatus"))
                cJobStages.ScheduleID = CInt(dt.Rows(i)("ScheduleID"))
                If cJobStages.StageStatus = 3 Then
                    cJobStages.CompletedDateTime = CDate(Me.txtDespatchDate.Text)
                Else
                    cJobStages.CompletedDateTime = SharedConstants.DEFAULT_DATE_VALUE
                End If

                If cJobStages.JobStagesID = SharedConstants.DEFAULT_INTEGER_VALUE Then
                    Dim intID As Integer = service.AddJobStages(cJobStages, cnn, trans)
                    If intID = SharedConstants.DEFAULT_INTEGER_VALUE Then
                        bolContinue = False
                    End If
                Else
                    bolContinue = service.UpdateJobStages(cJobStages, cnn, trans)
                End If

                If bolContinue Then
                    bolContinue = service.AddJobStagesHistoryRecord(cJobStages, CInt(Session("sessUserID")), cnn, trans)
                End If

                If bolContinue Then
                    'update orderstatus to despatched
                    Dim cProductionSchedule As ProductionSchedule = service.GetProdScheduleClsByID(CInt(dt.Rows(i)("ScheduleID")), cnn, trans)
                    Dim cNewProductionSchedule As ProductionSchedule = CType(cProductionSchedule.Clone, ProductionSchedule)
                    cNewProductionSchedule.OrderStatus = 4

                    bolContinue = service.UpdateProductionScheduleByID(cNewProductionSchedule, cnn, trans)

                    If bolContinue Then
                        bolContinue = service.AddProdScheduleHistoryRcd(0, cProductionSchedule, cnn, trans)
                    End If

                    'Added by Michael Behar - 15-09-2020 - Ticket #59768
                    If bolContinue Then

                        'Get Shipping Details
                        cProductionSchedule.ShippingDetails = txtConNote.Text

                        bolContinue = service.UpdateShippingDetails(cProductionSchedule, cnn, trans)
                        lProdSchedulesUpdated.Add(cNewProductionSchedule)
                    End If
                End If

                If bolContinue = False Then
                    Exit For
                End If
            Next i

            If bolContinue Then
                trans.Commit()

                ' Send order completed emails to customers.
                Dim cUser As User = service.getUserByID(Session("sessUserID"))

                If cUser.ID > 0 Then
                    For Each p As ProductionSchedule In lProdSchedulesUpdated
                        Dim cCustomer As Customer = service.GetCustomerByID(p.CustomerID)

                        If cCustomer.CustomerID > 0 Then
                            If service.Mail.CustomerIsOnActiveMailingList(MailingListID.OrderCompleted, cCustomer.CustomerID) Then
                                service.Mail.SendOrderCompletedEmail(cCustomer, cUser, p.ID, p.OrderReference)
                            End If
                        End If
                    Next p
                End If

                Response.Redirect("ToBeDespatchedUpdate.aspx", False)
            Else
                EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - Transaction Rolled Back" & Environment.NewLine & SharedFunctions.GetPageInfo(form1, Session))
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
            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message & Environment.NewLine & SharedFunctions.GetPageInfo(form1, Session))
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


    Private Sub UpdateConNote()

        Dim service As New OzrollPSLVSchedulingModel.AppService
        Dim bolContinue As Boolean = True
        Dim cProductionSchedule As New ProductionSchedule

        Dim dbConn As New DBConnection
        Dim cnn As SqlClient.SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim trans As SqlClient.SqlTransaction = Nothing
        dbConn = Nothing

        Try
            cnn.Open()
            trans = cnn.BeginTransaction

            Dim dtRecordsToUpdate As DataTable = GetRecordsToUpdate()

            For Each row As DataRow In dtRecordsToUpdate.Rows

                'Get Shipping Details
                cProductionSchedule.ShippingDetails = row("ConNote")
                cProductionSchedule.ID = row("OrderID")

                bolContinue = service.UpdateShippingDetails(cProductionSchedule, cnn, trans)

                If Not bolContinue Then
                    Exit For
                End If

            Next

            If bolContinue Then
                trans.Commit()
            Else
                EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - Transaction Rolled Back" & Environment.NewLine & SharedFunctions.GetPageInfo(form1, Session))
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
            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message & Environment.NewLine & SharedFunctions.GetPageInfo(form1, Session))
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


        If bolContinue Then
            Response.Redirect("TobeDespatchedUpdate.aspx", False)
        End If

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

        Dim intID As Integer = SharedConstants.DEFAULT_INTEGER_VALUE
        Dim intStageID As Integer = 7
        Dim drow As DataRow = Nothing
        For Each gvr As GridViewRow In Me.dgvDespatchedList.Rows
            intID = CInt(dgvDespatchedList.DataKeys(gvr.RowIndex).Value)
            'despatch stage checks
            Dim chk As CheckBox = DirectCast(gvr.FindControl("chkDespatch"), CheckBox)
            Dim hfStatus As HiddenField = DirectCast(gvr.FindControl("hidDespatchStatus"), HiddenField)
            Dim hfID As HiddenField = DirectCast(gvr.FindControl("hidDespatchID"), HiddenField)
            If hfID.Value <> String.Empty Then
                If hfStatus.Value <> 3 Then
                    If chk.Checked Then
                        'update to completed
                        drow = dt.NewRow
                        drow("ScheduleID") = intID
                        drow("JobStagesID") = hfID.Value
                        drow("StageID") = intStageID
                        drow("StageStatus") = intStageCompleted
                        dt.Rows.Add(drow)
                    End If
                Else
                    If chk.Checked = False Then
                        'update to not started
                        drow = dt.NewRow
                        drow("ScheduleID") = intID
                        drow("JobStagesID") = hfID.Value
                        drow("StageID") = intStageID
                        drow("StageStatus") = intStageNotStarted
                        dt.Rows.Add(drow)
                    End If
                End If
            Else
                'add new record 
                drow = dt.NewRow
                drow("ScheduleID") = intID
                drow("JobStagesID") = SharedConstants.DEFAULT_INTEGER_VALUE
                drow("StageID") = intStageID
                If chk.Checked = True Then
                    drow("StageStatus") = intStageCompleted
                Else
                    drow("StageStatus") = intStageNotStarted
                End If
                dt.Rows.Add(drow)
            End If
            drow = Nothing
        Next
        Return dt

    End Function

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Response.Redirect("Home.aspx", False)

    End Sub

    Protected Sub btnHome_Click(sender As Object, e As System.EventArgs) Handles btnHome.Click

        Response.Redirect("Home.aspx", False)

    End Sub

    Protected Sub chkDespatch_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim bolVisible As Boolean = False

        For Each gvr As GridViewRow In Me.dgvDespatchedList.Rows
            Dim hfStatus As HiddenField = DirectCast(gvr.FindControl("hidDespatchStatus"), HiddenField)
            Dim hfID As HiddenField = DirectCast(gvr.FindControl("hidDespatchID"), HiddenField)
            Dim chk As CheckBox = DirectCast(gvr.FindControl("chkDespatch"), CheckBox)
            If hfID.Value <> String.Empty Then
                If hfStatus.Value <> 3 Then
                    If chk.Checked Then
                        bolVisible = True
                    End If
                End If
            End If
        Next

        btnSave.Enabled = False
        btnSave.CssClass = "form-button-disabled"
        If bolVisible Then
            btnSave.Enabled = True
            btnSave.CssClass = "form-button"
        End If

    End Sub

    Protected Sub ChkEdit_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim bolVisible As Boolean = False

        For Each gvr As GridViewRow In Me.dgvDespatchedList.Rows
            Dim hfStatus As HiddenField = DirectCast(gvr.FindControl("hidDespatchStatus"), HiddenField)
            Dim hfID As HiddenField = DirectCast(gvr.FindControl("hidDespatchID"), HiddenField)
            Dim chk As CheckBox = DirectCast(gvr.FindControl("chkEdit"), CheckBox)
            Dim textBox As TextBox = DirectCast(gvr.FindControl("txtInternalConNote"), TextBox)
            textBox.Enabled = False
            If chk.Checked Then
                bolVisible = True
                textBox.Enabled = bolVisible
            End If
        Next

        btnSave.Enabled = bolVisible
        btnSave.CssClass = "form-button-disabled"
        btnSave.Text = "Save"
        If bolVisible Then
            btnSave.CssClass = "form-button"
            btnSave.Text = "Update"
        End If

    End Sub


    Private Sub radioChecked(sender As Object, e As EventArgs) Handles rdoDespatched.CheckedChanged, rdoShowToBeDespatched.CheckedChanged

        Dim dt As New DataTable
        Dim bolDespatched As Boolean = False
        txtConNote.Text = ""
        txtConNote.Enabled = True

        If rdoDespatched.Checked Then
            bolDespatched = True
            txtConNote.Enabled = False
        End If

        btnSave.Enabled = False
        btnSave.CssClass = "form-button-disabled"
        btnSave.Text = "Save"

        dt = loadDatatable(bolDespatched)

        Me.dgvDespatchedList.DataSource = dt
        Me.dgvDespatchedList.DataBind()

    End Sub


    Private Function GetCompletedDespatchList() As DataTable

        Dim service As New AppService

        Dim strSQL As String = " Select tblProductionScheduleList.ID, tblProductionScheduleList.OrderReference, tblProductionScheduleList.ShippingDetails, "
        strSQL &= " tblProductionScheduleList.CustomerName, tblCustomers.CustomerName As BranchName, tblCustomers.CustomeriD, "
        strSQL &= " tblProductionScheduleList.TotalPanels, tblProductionScheduleList.TotalSQM, tblProductionScheduleList.ScheduledDate, q1.* "
        strSQL &= " From tblProductionScheduleList "
        strSQL &= " Inner Join tblCustomers On tblProductionScheduleList.CustomeriD = tblCustomers.CustomeriD "
        strSQL &= " Cross Apply fn_getJobStagesForScheduleID(dbo.tblProductionScheduleList.ID) q1"
        strSQL &= " Where (tblProductionScheduleList.OrderStatus = 4 And q1.QCStatus = 3 And "
        strSQL &= " tblCustomers.CollectionFromFactory = 0 And tblProductionScheduleList.ProductTypeID = " & ProductType.Louvres & ")"

        dtCompletedDespatchList = service.RunSQLScheduling(strSQL)
        ViewState("dtCompletedDespatchList") = dtCompletedDespatchList

        service = Nothing

        Return dtCompletedDespatchList

    End Function


    Private Function GetRecordsToUpdate() As DataTable

        Dim dtToUpdate As DataTable = createToUpdateDatatable()
        Dim dtCompleted As DataTable = ViewState("dtCompletedDespatchList")
        For Each gvr As GridViewRow In Me.dgvDespatchedList.Rows
            Dim chk As CheckBox = DirectCast(gvr.FindControl("chkEdit"), CheckBox)
            Dim txtBox As TextBox = DirectCast(gvr.FindControl("txtInternalConNote"), TextBox)
            If chk.Checked Then
                Dim intOrderID As Integer = CInt(gvr.Cells(0).Text)
                Dim strConNote As String = gvr.Cells(11).Text

                If txtBox.Text <> strConNote Then

                    'Add Record
                    Dim dataRowNew As DataRow = dtToUpdate.NewRow
                    dataRowNew("OrderId") = intOrderID
                    dataRowNew("ConNote") = txtBox.Text
                    dtToUpdate.Rows.Add(dataRowNew)

                End If
            End If
        Next

        Return dtToUpdate

    End Function

    Private Function GetToBeDespatchedList() As DataTable
        Dim service As New AppService
        dtToBeDespatched = service.GetToBeDespatchedList(CInt(Me.txtProductTypeID.Text))
        ViewState("dtToBeDespatched") = dtToBeDespatched
        service = Nothing

        Return dtToBeDespatched

    End Function

    Private Function createToUpdateDatatable() As DataTable

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "OrderId"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ConNote"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        Return dt

    End Function

End Class
