
Partial Class UpdateStockPicking
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
            Me.dgvStockList.DataSource = dt
            Me.dgvStockList.DataBind()

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
        Dim dt As DataTable = createDatatable()

        Dim strSQL As String = String.Empty
        strSQL = "Select ID, ShutterProNumber, cus.CustomerName As Customer, OrderReference, prod.CustomerName As CustomerRef, TotalPanels, TotalSQM, Colour, "
        strSQL &= "Case prod.OrderTypeID When 1 Then 'Order' when 2 then 'Remake' when 3 then 'Rework' End as OrderTypeID "
        strSQL &= "from tblProductionScheduleList prod inner join tblCustomers cus on prod.CustomerID=cus.CustomerID "
        strSQL &= "Left Join tblLouvreDetails det on prod.ID=det.ProductionScheduleID where OrderStatus=2 and ProductTypeID=2"

        Dim dtMain As DataTable = service.runSQLScheduling(strSQL)
        service = Nothing

        For i As Integer = 0 To dtMain.Rows.Count - 1
            Dim drow As DataRow = dt.NewRow
            drow("ID") = dtMain.Rows(i)("ID")
            drow("ShutterProNumber") = dtMain.Rows(i)("ShutterProNumber")
            drow("Customer") = dtMain.Rows(i)("Customer")
            drow("OrderReference") = dtMain.Rows(i)("OrderReference")
            drow("CustomerRef") = dtMain.Rows(i)("CustomerRef")
            drow("TotalPanels") = dtMain.Rows(i)("TotalPanels")
            drow("TotalSQM") = dtMain.Rows(i)("TotalSQM")
            drow("Colour") = dtMain.Rows(i)("Colour")
            drow("OrderTypeID") = dtMain.Rows(i)("OrderTypeID")
            dt.Rows.Add(drow)
            drow = Nothing
        Next
        Return dt

    End Function

    Protected Function createDatatable() As DataTable

        Dim dt As DataTable = New DataTable
        Dim col As DataColumn = New DataColumn
        col.ColumnName = "ID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ShutterProNumber"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Customer"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "OrderReference"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "CustomerRef"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "TotalPanels"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "TotalSQM"
        col.DataType = System.Type.GetType("System.Decimal")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Colour"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "OrderTypeID"
        col.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(col)
        col = Nothing

        Return dt

    End Function

    'Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click

    '    Dim service As New AppService
    '    Dim bolContinue As Boolean = True

    '    Dim dbConn As New DBConnection
    '    Dim cnn As SqlClient.SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
    '    Dim trans As SqlClient.SqlTransaction = Nothing
    '    dbConn = Nothing
    '    btnSave.Enabled = False
    '    btnSave.CssClass = "form-button-disabled"
    '    lblStatus.Text = String.Empty

    '    Try

    '        cnn.Open()
    '        trans = cnn.BeginTransaction

    '        Dim dt As DataTable = setEnteredValues()
    '        For i As Integer = 0 To dt.Rows.Count - 1

    '            If bolContinue Then
    '                'update orderstatus to invoiced plus set invoiced date

    '                Dim cProductionSchedule As ProductionSchedule = service.getProdScheduleClsByID(CInt(dt.Rows(i)("ScheduleID")), cnn, trans)
    '                Dim cNewProductionSchedule As ProductionSchedule = CType(cProductionSchedule.Clone, ProductionSchedule)
    '                cNewProductionSchedule.OrderStatus = 2 'received - accepted


    '                bolContinue = service.updateProductionScheduleByID(cNewProductionSchedule, cnn, trans)

    '                If bolContinue Then
    '                    bolContinue = service.addProdScheduleHistoryRcd(0, cProductionSchedule, cnn, trans)
    '                End If
    '            End If

    '            If bolContinue = False Then
    '                Exit For
    '            End If
    '        Next

    '        If bolContinue Then
    '            trans.Commit()
    '            Response.Redirect("UpdateStockPicking.aspx", False)
    '        Else
    '            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - Transaction Rolled Back" & Environment.NewLine & getPageInfo())
    '            trans.Rollback()
    '            If lblStatus.Text = String.Empty Then
    '                lblStatus.Text = "Error saving details. Please try again."
    '            End If
    '            btnSave.Enabled = True
    '            btnSave.CssClass = "form-button"
    '        End If

    '    Catch ex As Exception
    '        If Not trans Is Nothing Then
    '            trans.Rollback()
    '        End If
    '        If cnn.State = ConnectionState.Open Then
    '            cnn.Close()
    '        End If
    '        EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message & Environment.NewLine & getPageInfo())
    '        bolContinue = False
    '    Finally
    '        trans.Dispose()
    '        trans = Nothing
    '        If cnn.State = ConnectionState.Open Then
    '            cnn.Close()
    '        End If
    '        cnn.Dispose()
    '        cnn = Nothing
    '    End Try
    '    service = Nothing

    'End Sub

    Protected Function setEnteredValues() As DataTable

        Dim dt As DataTable = New DataTable

        Dim col As DataColumn = New DataColumn
        col.ColumnName = "ID"
        col.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "ReceivedDate"
        col.DataType = System.Type.GetType("System.DateTime")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Received"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing

        Dim intID As Integer = SharedConstants.DEFAULT_INTEGER_VALUE
        Dim drow As DataRow = Nothing

        Return dt

    End Function

    Protected Sub dgvStockList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvStockList.RowCommand

        If (e.CommandName = "StockID") Then
            'If Not Request.QueryString.Count = 0 Then
            '    If Not IsNothing(Request.Params("StateId")) Then
            '        'Me.txtCurStateId.Text = Request.Params("StateId")
            '        intStateId = Request.Params("StateId")
            '    End If
            'End If

            Dim currentRowIndex As Integer = Int32.Parse(e.CommandArgument.ToString())
            Dim intStockID As String = dgvStockList.DataKeys(currentRowIndex).Values("ID").ToString
            Dim intOrderTypeID As Integer = CInt(dgvStockList.DataKeys(currentRowIndex).Values("OrderTypeID"))

            Dim strSearchQueryString As String = String.Empty

            If Me.txtProductTypeID.Text = "2" Then
                If SharedConstants.LIVE_SITE Then
                    Response.Redirect("StockPage.aspx?Id=" & intStockID.ToString & "&ViewType=1" & strSearchQueryString, False)
                Else
                    Response.Redirect("StockPage.aspx?Id=" & intStockID.ToString & "&ViewType=1" & strSearchQueryString, False)
                End If
            End If

        End If

    End Sub

    'Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

    '    Response.Redirect("Home.aspx", False)

    'End Sub

    Protected Sub btnHome_Click(sender As Object, e As System.EventArgs) Handles btnHome.Click

        Response.Redirect("Home.aspx", False)

    End Sub

End Class


