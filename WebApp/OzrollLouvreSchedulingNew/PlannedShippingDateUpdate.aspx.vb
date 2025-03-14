﻿
Partial Class PlannedShippingDateUpdate
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

            'load customers list
            Dim service As New AppService

            service.addWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)

            SharedFunctions.PopulateCustomersDDL(cboCustomer, SharedEnums.ProductType.Louvres, service)

            Dim intCustomer As Integer = 0

            If Not Request.QueryString.Count = 0 Then
                If Not IsNothing(Request.Params("customer")) Then
                    intCustomer = CInt(Request.Params("customer"))
                End If
            End If

            If intCustomer > 0 Then
                Me.cboCustomer.SelectedValue = intCustomer
                Me.cboCustomer_SelectedIndexChanged(Me, Nothing)
            Else
                popSummary(New DataTable)
            End If

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

    Protected Function loadDatatable(intCustomerID As Integer) As DataTable

        Dim service As New AppService
        Dim dt As DataTable = createDatatable()
        Dim dtMain As DataTable = service.runSQLScheduling("select dbo.tblProductionScheduleList.*, dbo.tblCustomers.CustomerName as BranchName from dbo.tblProductionScheduleList inner join dbo.tblCustomers on dbo.tblProductionScheduleList.CustomeriD = dbo.tblCustomers.CustomeriD where (OrderStatus in(2,3,6)) and (dbo.tblProductionScheduleList.CustomeriD=" & intCustomerID.ToString & ") and (ProductTypeID=" & Me.txtProductTypeID.Text & ")")
        service = Nothing

        For i As Integer = 0 To dtMain.Rows.Count - 1
            Dim drow As DataRow = dt.NewRow

            drow("ScheduleID") = dtMain.Rows(i)("ID")
            drow("BranchName") = dtMain.Rows(i)("BranchName")
            drow("ReferenceNumber") = dtMain.Rows(i)("OrderReference")
            drow("ReferenceName") = dtMain.Rows(i)("CustomerName")
            drow("NoOfPanels") = dtMain.Rows(i)("TotalPanels")
            drow("TotalSQM") = dtMain.Rows(i)("TotalSQM")

            drow("ExpectedShippingDate") = dtMain.Rows(i)("ExpectedShippingDate")
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
        col.ColumnName = "ExpectedShippingDate"
        col.DataType = System.Type.GetType("System.DateTime")
        dt.Columns.Add(col)
        col = Nothing

        Return dt

    End Function

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click



    End Sub

    Protected Function setCollectedValues() As DataTable

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
        col.ColumnName = "ExpectedShippingDate"
        col.DataType = System.Type.GetType("System.DateTime")
        dt.Columns.Add(col)
        col = Nothing

        Dim intID As Integer = SharedConstants.DEFAULT_INTEGER_VALUE
        Dim drow As DataRow = Nothing
        For Each gvr As GridViewRow In Me.dgvShippingDateList.Rows
            intID = CInt(dgvShippingDateList.DataKeys(gvr.RowIndex).Value)
            'despatch stage checks
            Dim txt As TextBox = DirectCast(gvr.FindControl("txtPlannedShipDate"), TextBox)
            If txt.Text <> String.Empty Then
                'update to completed
                drow = dt.NewRow
                drow("ScheduleID") = intID
                drow("ExpectedShippingDate") = CDate(txt.Text)
                dt.Rows.Add(drow)
            Else
                drow = dt.NewRow
                drow("ScheduleID") = intID
                drow("ExpectedShippingDate") = DBNull.Value
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

    Protected Sub txtPlannedShipDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim service As New AppService
        Dim bolContinue As Boolean = True

        Dim dbConn As New DBConnection
        Dim cnn As SqlClient.SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim trans As SqlClient.SqlTransaction = Nothing
        dbConn = Nothing
        btnCancel.Enabled = False
        btnCancel.CssClass = "form-button-disabled"
        btnSave.Enabled = False
        btnSave.CssClass = "form-button-disabled"
        lblStatus.Text = String.Empty

        Try

            cnn.Open()
            trans = cnn.BeginTransaction

            Dim dt As DataTable = setCollectedValues()
            For i As Integer = 0 To dt.Rows.Count - 1

                If bolContinue Then
                    'update orderstatus to invoiced plus set invoiced date
                    Dim cProductionSchedule As ProductionSchedule = service.getProdScheduleClsByID(CInt(dt.Rows(i)("ScheduleID")), cnn, trans)
                    Dim cNewProductionSchedule As ProductionSchedule = CType(cProductionSchedule.Clone, ProductionSchedule)
                    If Not IsDBNull(dt.Rows(i)("ExpectedShippingDate")) Then
                        cNewProductionSchedule.ExpectedShippingDate = dt.Rows(i)("ExpectedShippingDate").ToString
                    Else
                        cNewProductionSchedule.ExpectedShippingDate = SharedConstants.DEFAULT_DATE_VALUE
                    End If

                    bolContinue = service.updateProductionScheduleByID(cNewProductionSchedule, cnn, trans)

                    If bolContinue Then
                        bolContinue = service.addProdScheduleHistoryRcd(0, cProductionSchedule, cnn, trans)
                    End If
                End If

                If bolContinue = False Then
                    Exit For
                End If
            Next

            If bolContinue Then
                trans.Commit()
                Response.Redirect("PlannedShippingDateUpdate.aspx?customer=" & Me.cboCustomer.SelectedValue.ToString, False)
                'btnSave.Enabled = True
                'btnSave.CssClass = "form-button"
            Else
                EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - Transaction Rolled Back" & Environment.NewLine & getPageInfo())
                trans.Rollback()
                If lblStatus.Text = String.Empty Then
                    lblStatus.Text = "Error saving details. Please try again."
                End If

                btnSave.Enabled = True
                btnSave.CssClass = "form-button"
                btnCancel.Enabled = True
                btnCancel.CssClass = "form-button"
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

    Protected Sub cboCustomer_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboCustomer.SelectedIndexChanged

        Dim intCustomerID As Integer = 0
        Dim dt As DataTable = New DataTable

        If Me.cboCustomer.SelectedIndex >= 0 Then
            intCustomerID = CInt(Me.cboCustomer.SelectedValue)
            dt = loadDatatable(intCustomerID)
            Me.dgvShippingDateList.DataSource = dt
            Me.dgvShippingDateList.DataBind()
        End If

        popSummary(dt)

    End Sub

    Protected Function GetDate(strDt As Object) As String

        Dim dt1 As DateTime
        If DateTime.TryParse(strDt.ToString(), dt1) Then
            Return dt1.ToString("d MMM yyyy")
        Else
            Return ""
        End If

    End Function

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
            If dtJobList.Rows.Count > 0 Then
                dwJobLists = dtJobList.Select("ExpectedShippingDate='" & Format(arrCalcDate(i), "d/MMM/yyyy") & "'")
                If dwJobLists.Length > 0 Then
                    For j As Integer = 0 To dwJobLists.Length - 1
                        arrIDcnt(i) += 1
                        If Not IsDBNull(dwJobLists(j)("NoOfPanels")) Then
                            arrTotalPnlCNT(i) += CInt(dwJobLists(j)("NoOfPanels"))
                        End If
                        If Not IsDBNull(dwJobLists(j)("TotalSQM")) Then
                            arrTotlaSqMcnt(i) += CDec(dwJobLists(j)("TotalSQM"))
                        End If
                    Next
                End If
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
