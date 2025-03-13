Imports System.Drawing
Imports System.Linq
Imports OzrollPSLVSchedulingModel.SharedEnums
Imports OzrollPSLVSchedulingModel.SharedConstants

Partial Class UpdateAwaitingInvoicing
    Inherits System.Web.UI.Page

    Dim _Service As New AppService

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
            service.AddWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)
            service = Nothing

            Me.txtInvoiceDate.Text = Format(DateTime.Now, "d MMM yyyy")

            Dim dt As DataTable = loadDatatable()
            Me.dgvInvoicingList.DataSource = dt
            Me.dgvInvoicingList.DataBind()

        End If

        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "styleFocusElements", "styleFocusElements();", True)

    End Sub

    Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        SharedFunctions.PageError(Page, form1)
    End Sub

    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        Response.Redirect("Logout.aspx", False)
    End Sub

    Protected Function loadDatatable() As DataTable

        Dim service As New AppService
        Dim dt As DataTable = createDatatable()
        'Dim dtMain As DataTable = service.runSQLScheduling("select dbo.tblProductionScheduleList.*, dbo.tblCustomers.CustomerName as BranchName from dbo.tblProductionScheduleList inner join dbo.tblCustomers on dbo.tblProductionScheduleList.CustomeriD=dbo.tblCustomers.CustomeriD where OrderStatus in(4,7)")
        Dim dtMain As DataTable = service.GetToBeInvoiced(CInt(Me.txtProductTypeID.Text))
        service = Nothing

        For i As Integer = 0 To dtMain.Rows.Count - 1
            Dim drow As DataRow = dt.NewRow

            drow("Failed") = False
            drow("ScheduleID") = dtMain.Rows(i)("ID")
            drow("ShutterProNumber") = dtMain.Rows(i)("ShutterProNumber")
            drow("BranchName") = dtMain.Rows(i)("BranchName")
            drow("ReferenceNumber") = dtMain.Rows(i)("OrderReference")
            drow("ReferenceName") = dtMain.Rows(i)("CustomerName")
            drow("NoOfPanels") = dtMain.Rows(i)("TotalPanels")
            drow("TotalSQM") = dtMain.Rows(i)("TotalSQM")

            drow("InvoicedDate") = DBNull.Value
            drow("Invoiced") = False
            drow("CollectionFromFactory") = dtMain.Rows(i)("CollectionFromFactory")

            drow("ShippingDetails") = dtMain.Rows(i)("ShippingDetails")
            drow("CustomerID") = dtMain.Rows(i)("CustomerID")

            dt.Rows.Add(drow)
            drow = Nothing
        Next i

        Dim lFailedInvoices As List(Of InvoiceOrder) = GetFailedInvoices()

        For Each i As InvoiceOrder In lFailedInvoices
            Dim cProdSchedule As ProductionSchedule = _Service.GetProdScheduleClsByID(i.ProductionScheduleID)

            If cProdSchedule.ID > 0 Then
                Dim cCustomer As Customer = _Service.GetCustomerByID(cProdSchedule.CustomerID)
                Dim drow As DataRow = dt.NewRow

                drow("Failed") = True
                drow("ScheduleID") = cProdSchedule.ID
                drow("ShutterProNumber") = cProdSchedule.ShutterProNumber
                drow("BranchName") = cCustomer.CustomerName
                drow("ReferenceNumber") = cProdSchedule.OrderReference
                drow("ReferenceName") = cProdSchedule.CustomerName
                drow("NoOfPanels") = cProdSchedule.TotalPanels
                drow("TotalSQM") = cProdSchedule.TotalSQM

                drow("InvoicedDate") = DBNull.Value
                drow("Invoiced") = False

                drow("CollectionFromFactory") = cCustomer.CollectionFromFactory

                drow("StatusMessage") = IIf(String.IsNullOrWhiteSpace(i.StatusMessage), "ERROR", i.StatusMessage)

                'Added by Michael Behar - 14-10-2020
                drow("ShippingDetails") = cProdSchedule.ShippingDetails
                drow("CustomerID") = cProdSchedule.CustomerID

                dt.Rows.Add(drow)
                drow = Nothing
            End If
        Next i

        Return dt

    End Function

    Private Function GetFailedInvoices() As List(Of InvoiceOrder)
        Dim cParams As New QueryParams.InvoiceOrderQueryParams

        cParams.ProductTypeID = ProductType.Louvres
        cParams.ErrorState = 1

        Return _Service.GetInvoiceOrdersByParams(cParams)
    End Function

    Protected Function createDatatable() As DataTable

        Dim dt As DataTable = New DataTable
        Dim col As DataColumn = New DataColumn
        col.ColumnName = "ScheduleID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Failed"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ShutterProNumber"
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
        col.ColumnName = "InvoicedDate"
        col.DataType = System.Type.GetType("System.DateTime")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Invoiced"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "CollectionFromFactory"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ShippingDetails"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "CustomerID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "StatusMessage"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        Return dt

    End Function

    Protected Function GetStockDeductionText(cDataRow As DataRow) As String
        Dim strText As String = "<span style='Color:darkred;'>Not Done</span>"

        If cDataRow("ScheduleID") > 0 Then
            Dim lStockDeductions As List(Of StockDeduction) = _Service.GetStockDeductionsByProductionScheduleID(cDataRow("ScheduleID"))
            Dim enumDeductionStatus As StockDeductionStatus = StockDeductionStatus.Success

            If lStockDeductions.Where(Function(x) x.Status = StockDeductionStatus.Failure).Any() Then
                enumDeductionStatus = StockDeductionStatus.Failure
            ElseIf lStockDeductions.Where(Function(x) x.Status = StockDeductionStatus.AwaitingProcessing).Any() Then
                enumDeductionStatus = StockDeductionStatus.AwaitingProcessing
            End If

            If enumDeductionStatus = StockDeductionStatus.AwaitingProcessing Then
                strText = "<span style='Color:orange;'>" & _STOCK_DEDUCTIONS_PROCESSING_MSG & "</span>"

            ElseIf enumDeductionStatus = StockDeductionStatus.Failure Then
                strText = "<span style='Color:red;'>" & _STOCK_DEDUCTIONS_FAILURE_MSG & " - " & lStockDeductions(0).StatusMessage & "</span>"

            ElseIf (lStockDeductions.Count > 0) AndAlso enumDeductionStatus = StockDeductionStatus.Success Then
                strText = "<span style='Color:green;'>" & _STOCK_DEDUCTIONS_COMPLETE_MSG & "</span>"
            End If
        End If

        Return strText
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

        'Added For Manifest
        Dim dt As New DataTable
        Dim strOrdersToInvoice As String = ""

        Try

            cnn.Open()
            trans = cnn.BeginTransaction

            dt = setInvoicedValues()

            'Added For Manifest
            dt = SharedFunctions.PerformDatatableFilterSortField(dt, "", "ShippingDetails, CustomerID asc")

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim cInvoiceOrder As New InvoiceOrder

                ' Write to the InvoiceOrders table.
                If bolContinue Then
                    If dt.Rows(i)("Failed") Then
                        ' Rety existing. Just reset invoice error state.
                        cInvoiceOrder = _Service.GetInvoiceOrderByProductionScheduleID(dt.Rows(i)("ScheduleID"), cnn, trans)
                        cInvoiceOrder.ErrorState = 0
                        cInvoiceOrder.StatusMessage = String.Empty
                        'cInvoiceOrder.InvoiceDate = CDate(txtInvoiceDate.Text)
                        'FL #64416 18-02-2021
                        If CDate(txtInvoiceDate.Text) <> Date.Today Then
                            cInvoiceOrder.InvoiceDate = CDate(txtInvoiceDate.Text).Date
                            cInvoiceOrder.InvoiceDate = cInvoiceOrder.InvoiceDate.Add(DateTime.Now.TimeOfDay)
                        Else cInvoiceOrder.InvoiceDate = DateTime.Now
                        End If
                        'FL #64416 18-02-2021
                        cInvoiceOrder.InvoicedDateTime = SharedConstants.MIN_DATE

                        bolContinue = (cInvoiceOrder.ID > 0)
                    Else
                        With cInvoiceOrder
                            .ID = 0
                            .ErrorState = 0
                            .GenerationTypeID = 1
                            .ProductionScheduleID = CInt(dt.Rows(i)("ScheduleID"))
                            '.InvoiceDate = CDate(txtInvoiceDate.Text)
                            'FL #64416 18-02-2021
                            If CDate(txtInvoiceDate.Text) <> Date.Today Then
                                .InvoiceDate = CDate(txtInvoiceDate.Text).Date
                                .InvoiceDate = .InvoiceDate.Add(DateTime.Now.TimeOfDay)
                            Else .InvoiceDate = DateTime.Now
                            End If
                            'FL #64416 18-02-2021
                            .InvoicedDateTime = SharedConstants.MIN_DATE
                            .StatusMessage = String.Empty
                        End With
                    End If

                    If bolContinue Then
                        bolContinue = (service.AddOrUpdateInvoiceOrder(cInvoiceOrder, cnn, trans) > 0)
                    End If

                    If bolContinue Then
                        ' Set completion date in PS. The invoicing app with set the invoiced date later.
                        Dim cProductionSchedule As ProductionSchedule = service.GetProdScheduleClsByID(CInt(dt.Rows(i)("ScheduleID")), cnn, trans)
                        Dim cNewProductionSchedule As ProductionSchedule = CType(cProductionSchedule.Clone, ProductionSchedule)

                        If CInt(dt.Rows(i)("CollectionFromFactory")) = 0 Then
                            cNewProductionSchedule.CompletedDate = DateTime.Now
                        End If

                        bolContinue = service.UpdateProductionScheduleByID(cNewProductionSchedule, cnn, trans)

                        If bolContinue Then
                            bolContinue = service.AddProdScheduleHistoryRcd(0, cProductionSchedule, cnn, trans)
                        End If
                    End If
                End If

                If Not bolContinue Then
                    Exit For
                End If
            Next i

            If bolContinue Then
                trans.Commit()
                Response.Redirect("UpdateAwaitingInvoicing.aspx", False)
            Else
                EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - Transaction Rolled Back" & Environment.NewLine & SharedFunctions.GetPageInfo(form1, Session))
                trans.Rollback()
                If lblStatus.Text = String.Empty Then
                    lblStatus.Text = "Error saving details. Please try again."
                End If
                btnSave.Enabled = True
                btnSave.CssClass = "form-button"
                bolContinue = False
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

        'Added For Manifest After Successful Invoice
        If bolContinue Then
            SendMailManifest(dt, strOrdersToInvoice)
        End If


    End Sub

    Protected Function setInvoicedValues() As DataTable

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "AutoID"
        col.DataType = System.Type.GetType("System.Int32")
        col.AutoIncrement = True
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Failed"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ScheduleID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "InvoicedDate"
        col.DataType = System.Type.GetType("System.DateTime")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Invoiced"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ShippingDetails"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "CustomerID"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "CollectionFromFactory"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        Dim intID As Integer = SharedConstants.DEFAULT_INTEGER_VALUE
        Dim drow As DataRow = Nothing

        For Each gvr As GridViewRow In Me.dgvInvoicingList.Rows
            intID = CInt(dgvInvoicingList.DataKeys(gvr.RowIndex).Value)
            'despatch stage checks
            Dim chk As CheckBox = DirectCast(gvr.FindControl("chkInvoice"), CheckBox)
            Dim hidFactory As HiddenField = DirectCast(gvr.FindControl("hidCollectFactory"), HiddenField)
            Dim boolFailed As Boolean = DirectCast(gvr.FindControl("lblFailed"), Label).Text
            Dim hidShippingDetails As HiddenField = DirectCast(gvr.FindControl("hidShippingDetails"), HiddenField)
            Dim hidCustomerID As HiddenField = DirectCast(gvr.FindControl("hidCustomerID"), HiddenField)

            If chk.Checked Then
                'update to completed
                drow = dt.NewRow
                drow("Failed") = boolFailed
                drow("ScheduleID") = intID
                'drow("InvoicedDate") = Me.txtInvoiceDate.Text
                'FL #64416 18-02-2021
                If CDate(txtInvoiceDate.Text) <> Date.Today Then
                    drow("InvoicedDate") = Me.txtInvoiceDate.Text
                Else drow("InvoicedDate") = DateTime.Now
                End If
                'FL #64416 18-02-2021
                drow("Invoiced") = True
                drow("CollectionFromFactory") = CInt(hidFactory.Value)
                drow("ShippingDetails") = hidShippingDetails.Value
                drow("CustomerID") = hidCustomerID.Value
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

    Private Sub dgvInvoicingList_DataBound(sender As Object, e As EventArgs) Handles dgvInvoicingList.DataBound
        If dgvInvoicingList.DataSource IsNot Nothing Then
            Dim boolVisible As Boolean = False

            For Each r As DataRow In DirectCast(dgvInvoicingList.DataSource, DataTable).Rows
                If r("Failed") Then
                    boolVisible = True
                    Exit For
                End If
            Next r

            dgvInvoicingList.Columns(10).Visible = boolVisible
        End If
    End Sub

    Private Sub SendMailManifest(dt As DataTable, strOrdersToInvoice As String)

        Dim strShippingDetails As String = 0
        Dim strPrevShippingDetails As String = 0
        Dim intCustomer As Integer = 0
        Dim intPrevCustomer As Integer = 0

        'Get Only Collection From Factory
        Dim dtDeliveryOnly As DataTable = SharedFunctions.PerformDatatableFilterSortField(dt, "CollectionFromFactory=0", "CustomerID asc")

        For i As Integer = 0 To dtDeliveryOnly.Rows.Count - 1

            'Get CustomerID
            strShippingDetails = dtDeliveryOnly.Rows(i).Item("ShippingDetails")
            intCustomer = dtDeliveryOnly.Rows(i).Item("CustomerID")

            'First Record - Set Previous
            If i = 0 Then
                strPrevShippingDetails = dtDeliveryOnly.Rows(i).Item("ShippingDetails")
                intPrevCustomer = dtDeliveryOnly.Rows(i).Item("CustomerID")
            End If

            'If Same Record - Customer ID
            If strShippingDetails = strPrevShippingDetails Then

                If intPrevCustomer = intCustomer Then
                    strOrdersToInvoice &= dtDeliveryOnly.Rows(i).Item("ScheduleID").ToString & ","
                    Continue For
                End If

            ElseIf strShippingDetails <> strPrevShippingDetails Then ' Else If Different Customer

                If intPrevCustomer <> intCustomer Then
                    strOrdersToInvoice = Left(strOrdersToInvoice, Len(strOrdersToInvoice) - 1)
                    GenerateLouvresManifest.GetLouvreManifest(strOrdersToInvoice, CDate(txtInvoiceDate.Text))
                    strPrevShippingDetails = strShippingDetails
                    intPrevCustomer = intCustomer
                    strOrdersToInvoice = dtDeliveryOnly.Rows(i).Item("ScheduleID").ToString & ","
                End If

            End If

        Next

        'Send For Final Records
        If strOrdersToInvoice.Length > 0 Then
            strOrdersToInvoice = Left(strOrdersToInvoice, Len(strOrdersToInvoice) - 1)
            GenerateLouvresManifest.GetLouvreManifest(strOrdersToInvoice, CDate(txtInvoiceDate.Text))
            strOrdersToInvoice = ""
        End If
    End Sub
End Class
