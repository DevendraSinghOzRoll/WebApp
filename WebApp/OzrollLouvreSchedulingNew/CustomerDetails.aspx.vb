Imports System.Linq
Imports Microsoft.Reporting.WebForms

Partial Class CustomerDetails
    Inherits System.Web.UI.Page

    Private _Service As New AppService
    Private _LouvreCategories As List(Of LouvreCategory)

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

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

            _Service.addWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)

            Dim intScheduleId As Integer = 0

            If Not Request.QueryString.Count = 0 Then
                If Not IsNothing(Request.Params("ScheduleId")) Then
                    intScheduleId = CInt(Request.Params("ScheduleId"))
                End If
            End If

            CacheGlobals()

            LoadGrid()

        Else
            CacheGlobals()
        End If


    End Sub

    Private Sub CacheGlobals()
        _LouvreCategories = _Service.GetLouvreCategories()
    End Sub

    Private Sub LoadGrid()

        dgvCustomerList.Visible = True

        Dim lCustomers As List(Of Customer) = _Service.GetCustomers().OrderBy(Function(x) x.CustomerName).ToList()

        If Not chkShowDiscontinued.Checked Then
            ' Remove discontinued customers.
            lCustomers = lCustomers.FindAll(Function(x) Not x.Discontinued)

            If lCustomers Is Nothing Then
                lCustomers = New List(Of Customer)
            End If
        End If

        dgvCustomerList.DataSource = lCustomers
        dgvCustomerList.DataBind()

    End Sub

    Protected Function GetLouvreCategoryNameByID(cLouvreCategoryID As Integer) As String
        Dim strName As String = String.Empty

        Dim cCat As LouvreCategory = _LouvreCategories.Find(Function(x) x.ID = cLouvreCategoryID)

        If cCat IsNot Nothing Then
            strName = cCat.Name
        End If

        Return strName
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

    Protected Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        Response.Redirect("Home.aspx", False)
    End Sub

    Protected Sub dgvCustomerList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvCustomerList.RowCommand
        Dim service As New AppService

        If e.CommandName = "CustomerUpdate" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dgvCustomerList.Rows(index)
            Dim c As String = dgvCustomerList.DataKeys(row.DataItemIndex).Value
            txtHidCustomerID.Text = c
            Response.Redirect(String.Format("~/AddCustomer.aspx?c={0}", c))
        End If

    End Sub

    Protected Sub btnAddCustomer_Click(sender As Object, e As EventArgs) Handles btnAddCustomer.Click

        Dim flagUpdate As Boolean = False
        txtHidFlag.Text = flagUpdate
        Dim flag As String = txtHidFlag.Text
        Response.Redirect("AddCustomer.aspx", False)
    End Sub

    Protected Sub dgvCustomerList_LinkSort(sender As Object, e As CommandEventArgs)
        If (e.CommandName = "Sort") Then
            SortCustomerGridView(e.CommandArgument)
        End If
    End Sub

    Private Sub dgvCustomerList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgvCustomerList.Sorting
        SortCustomerGridView(e.SortExpression)
    End Sub

    Private Sub SortCustomerGridView(strColumn As String)
        LoadGrid()

        Dim strPreviousSortColumn As String = dgvCustomerList.Attributes("SortColumn")
        Dim strSortDirection As String = dgvCustomerList.Attributes("SortDirection")
        Dim lCustomers As List(Of Customer) = dgvCustomerList.DataSource

        If String.IsNullOrEmpty(strSortDirection) Then
            ' No previous sort direction so set it to default of ASC.
            strSortDirection = SortDirection.Ascending
        End If

        If Not String.IsNullOrEmpty(strPreviousSortColumn) Then
            ' If the colomn was the last column sorted, flip the direction.
            If strPreviousSortColumn = strColumn Then

                If strSortDirection = SortDirection.Ascending Then
                    strSortDirection = SortDirection.Descending
                Else
                    strSortDirection = SortDirection.Ascending
                End If
            Else
                ' New column being sorted so default to ASC.
                strSortDirection = SortDirection.Ascending
            End If
        End If

        Select Case strColumn
            Case "PricingCategory"
                If strSortDirection = SortDirection.Ascending Then
                    lCustomers = lCustomers.OrderBy(Function(x) GetLouvreCategoryNameByID(x.LouvreCategoryID)).ToList()
                Else
                    lCustomers = lCustomers.OrderByDescending(Function(x) GetLouvreCategoryNameByID(x.LouvreCategoryID)).ToList()
                End If

            Case "Discontinued"
                If strSortDirection = SortDirection.Ascending Then
                    lCustomers = lCustomers.OrderBy(Function(x) x.Discontinued).ToList()
                Else
                    lCustomers = lCustomers.OrderByDescending(Function(x) x.Discontinued).ToList()
                End If

            Case "ExternalCustomer"
                If strSortDirection = SortDirection.Ascending Then
                    lCustomers = lCustomers.OrderBy(Function(x) x.ExternalCustomer).ToList()
                Else
                    lCustomers = lCustomers.OrderByDescending(Function(x) x.ExternalCustomer).ToList()
                End If

            Case "CollectionFromFactory"
                If strSortDirection = SortDirection.Ascending Then
                    lCustomers = lCustomers.OrderBy(Function(x) x.CollectionFromFactory).ToList()
                Else
                    lCustomers = lCustomers.OrderByDescending(Function(x) x.CollectionFromFactory).ToList()
                End If

            Case "Plantations"
                If strSortDirection = SortDirection.Ascending Then
                    lCustomers = lCustomers.OrderBy(Function(x) x.Plantations).ToList()
                Else
                    lCustomers = lCustomers.OrderByDescending(Function(x) x.Plantations).ToList()
                End If

            Case "WholesaleLouvres"
                If strSortDirection = SortDirection.Ascending Then
                    lCustomers = lCustomers.OrderBy(Function(x) x.WholesaleLouvres).ToList()
                Else
                    lCustomers = lCustomers.OrderByDescending(Function(x) x.WholesaleLouvres).ToList()
                End If

            Case "RetailLouvres"
                If strSortDirection = SortDirection.Ascending Then
                    lCustomers = lCustomers.OrderBy(Function(x) x.RetailLouvres).ToList()
                Else
                    lCustomers = lCustomers.OrderByDescending(Function(x) x.RetailLouvres).ToList()
                End If

            Case "FreightPercentage"
                If strSortDirection = SortDirection.Ascending Then
                    lCustomers = lCustomers.OrderBy(Function(x) x.FreightPercentage).ToList()
                Else
                    lCustomers = lCustomers.OrderByDescending(Function(x) x.FreightPercentage).ToList()
                End If

            Case "CustomerAbbreviation"
                If strSortDirection = SortDirection.Ascending Then
                    lCustomers = lCustomers.OrderBy(Function(x) x.CustomerAbbreviation).ToList()
                Else
                    lCustomers = lCustomers.OrderByDescending(Function(x) x.CustomerAbbreviation).ToList()
                End If

            Case "TradingName"
                If strSortDirection = SortDirection.Ascending Then
                    lCustomers = lCustomers.OrderBy(Function(x) x.TradingName).ToList()
                Else
                    lCustomers = lCustomers.OrderByDescending(Function(x) x.TradingName).ToList()
                End If

            Case "CustomerName"
                If strSortDirection = SortDirection.Ascending Then
                    lCustomers = lCustomers.OrderBy(Function(x) x.CustomerName).ToList()
                Else
                    lCustomers = lCustomers.OrderByDescending(Function(x) x.CustomerName).ToList()
                End If

        End Select

        ' Remember the last sorted colomn and its direction so we can flip the direction next time.
        dgvCustomerList.Attributes("SortColumn") = strColumn
        dgvCustomerList.Attributes("SortDirection") = strSortDirection

        dgvCustomerList.DataSource = lCustomers
        dgvCustomerList.DataBind()
    End Sub

    Private Sub chkShowDiscontinued_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowDiscontinued.CheckedChanged
        LoadGrid()
    End Sub

    'Added By Pradeep Singh against the ticket #66945
    Private Sub btnGenerateReportExcel_Click(sender As Object, e As EventArgs) Handles btnGenerateReportExcel.Click
        Page.Validate()

        If Page.IsValid() Then
            'Create Service

            Dim service As New AppService
            Dim Discontinued As Integer = 0
            If (chkShowDiscontinued.Checked = True) Then
                Discontinued = 1
            End If
            'Set Datatable and DataSet as Report Data Soruce
            Dim dt As DataTable = service.GetCustomerReport(Discontinued)
            Dim rds As New ReportDataSource("DataSet1", dt)

            'Set up the whole basis of the report
            Dim warnings As Warning() = Nothing
            Dim streamIds As String() = Nothing
            Dim strDeviceInfo As String = String.Empty
            Dim mimeType As String = String.Empty
            Dim encoding As String = String.Empty
            Dim extension As String = String.Empty

            'Create Report - Get Path - Add Data Source
            Dim reportViewer As New ReportViewer()
            reportViewer.ProcessingMode = ProcessingMode.Local
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) & "Reports\RDL\CustomerReport.rdl"
            reportViewer.LocalReport.DataSources.Add(rds)
            reportViewer.SizeToReportContent = True
            reportViewer.Width = Unit.Percentage(150)
            reportViewer.Height = Unit.Percentage(150)

            'Create Bytes Of Report

            Dim bytes As Byte() = reportViewer.LocalReport.Render("Excel", strDeviceInfo, mimeType, encoding, extension, streamIds, warnings)


            'Download the RDLC Report in Word, Excel, PDF and Image formats.
            Response.Buffer = True
            Response.Clear()
            Response.ContentType = mimeType
            Response.AddHeader("content-disposition", "attachment; filename=CustomerReport." & extension)
            Response.BinaryWrite(bytes)
            ' create the file
            Response.Flush()

            service = Nothing
            dt.Dispose()
            dt = Nothing

        End If
    End Sub


End Class
