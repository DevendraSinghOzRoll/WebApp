
Partial Class UpdateAwaitingAcceptance
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        SharedFunctions.RedirectIfInvalidUserSession(Session, Response)

        If Not IsPostBack Then

            txtProductTypeID.Text = Session("sessProductTypeID").ToString

            Dim service As New AppService

            service.AddWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)
            service = Nothing

            txtReceivedDate.Text = Format(DateTime.Now, "d MMM yyyy")

            Dim dt As DataTable = loadDatatable()

            dgvShutterProList.DataSource = dt
            dgvShutterProList.DataBind()

        End If

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
        ' Below Line Commented & new Line added By Pradeep Singh for ticket #62498
        'Dim dtMain As DataTable = service.RunSQLScheduling("select dbo.tblProductionScheduleList.*, dbo.tblCustomers.CustomerName as BranchName from dbo.tblProductionScheduleList inner join dbo.tblCustomers on dbo.tblProductionScheduleList.CustomeriD=dbo.tblCustomers.CustomeriD where (OrderStatus=1 or OrderStatus=2) and ProductTypeID=" & Me.txtProductTypeID.Text)
        Dim dtMain As DataTable = service.RunSQLScheduling("select dbo.tblProductionScheduleList.*, dbo.tblCustomers.CustomerName as BranchName from dbo.tblProductionScheduleList inner join dbo.tblCustomers on dbo.tblProductionScheduleList.CustomeriD=dbo.tblCustomers.CustomeriD where (OrderStatus=1) and ProductTypeID=" & Me.txtProductTypeID.Text)

        service = Nothing

        For i As Integer = 0 To dtMain.Rows.Count - 1
            Dim drow As DataRow = dt.NewRow

            drow("ScheduleID") = dtMain.Rows(i)("ID")
            drow("ShutterProNumber") = dtMain.Rows(i)("ShutterProNumber")
            drow("BranchName") = dtMain.Rows(i)("BranchName")
            drow("ReferenceNumber") = dtMain.Rows(i)("OrderReference")
            drow("ReferenceName") = dtMain.Rows(i)("CustomerName")
            drow("NoOfPanels") = dtMain.Rows(i)("TotalPanels")
            drow("TotalSQM") = dtMain.Rows(i)("TotalSQM")
            drow("ReceivedDate") = DBNull.Value
            drow("Received") = False

            dt.Rows.Add(drow)

            drow = Nothing
        Next i

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
        col.ColumnName = "ReceivedDate"
        col.DataType = System.Type.GetType("System.DateTime")
        dt.Columns.Add(col)
        col = Nothing

        col = New DataColumn
        col.ColumnName = "Received"
        col.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(col)
        col = Nothing
        Return dt

    End Function

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Response.Redirect("Home.aspx", False)

    End Sub

    Protected Sub btnHome_Click(sender As Object, e As System.EventArgs) Handles btnHome.Click

        Response.Redirect("Home.aspx", False)

    End Sub

    Private Sub dgvShutterProList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvShutterProList.RowCommand

        If (e.CommandName = "ReviewJob") Then
            'If Not Request.QueryString.Count = 0 Then
            '    If Not IsNothing(Request.Params("StateId")) Then
            '        'Me.txtCurStateId.Text = Request.Params("StateId")
            '        intStateId = Request.Params("StateId")
            '    End If
            'End If
            Dim currentRowIndex As Integer = Int32.Parse(e.CommandArgument.ToString())
            Dim intScheduleId As String = dgvShutterProList.DataKeys(currentRowIndex).Values("ScheduleID").ToString

            Response.Redirect("AcceptOrder.aspx?ScheduleId=" & intScheduleId.ToString, False)

        End If

    End Sub
End Class
