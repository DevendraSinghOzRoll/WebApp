
Imports System.Linq
Imports OzrollPSLVSchedulingModel.SharedEnums

Partial Class Home
    Inherits System.Web.UI.Page

    Dim _Service As New AppService

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim intErrorCount As Integer = 0

        If Not IsPostBack Then

            If Session("sessUserID") = String.Empty Then
                Response.Redirect("Logout.aspx", False)
                Exit Sub
            ElseIf Convert.ToInt32(Session("CustomerID")) > 0 Then
                Response.Redirect("Logout.aspx", False)
            Else
                If Not IsNumeric(Session("sessUserID")) Then
                    Response.Redirect("Logout.aspx", False)
                    Exit Sub
                End If
            End If



            txtProductTypeID.Text = Session("sessProductTypeID").ToString

            _Service.AddWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)

            ' Permission to access admin panel
            btnAdmin.Visible = _Service.Permissions.UserIsAdmin(Session("sessUserID"))

            If Session("sessUserName").ToString = "tbayley" Then
                '    Me.btnDeliveryDocket.Visible = True
                '    Me.btnRunningSheet.Visible = True
                '    Me.btnTimeSheet.Visible = True
                '    Me.btnCoverSheet.Visible = True
                'Me.btnCustomers.Visible = True
            End If

            'data to show in each section
            '-# jobs
            '-# of panels
            '-total sqm
            '-$value (cost price sum)??

            'section 1
            '-------
            'scheduled for today
            'scheduled for tomorrow
            '   if today is friday, show tomorrow as saturday and then add monday after that
            '   if today is saturday, show today as saturday and then tomorrow as monday
            Dim intWeekDay As Integer = Weekday(DateTime.Today.Date)

            Dim dteToday As Date = DateTime.Today.Date
            Dim dteTomorrow As Date = DateAdd(DateInterval.Day, 1, dteToday)
            Dim dteNextDay As Date = SharedConstants.DEFAULT_DATE_VALUE

            Select Case intWeekDay
                Case 6
                    'on friday, set next day to monday
                    dteNextDay = DateAdd(DateInterval.Day, 3, dteToday)
                Case 7
                    'on saturday, set tomorrow to monday
                    dteTomorrow = DateAdd(DateInterval.Day, 2, dteToday)
                Case Else

            End Select

            Dim dtScheduleToday As DataTable = _Service.GetDailyProductionSchedule(dteToday, CInt(Me.txtProductTypeID.Text))
            Dim dtScheduleTomorrow As DataTable = _Service.GetDailyProductionSchedule(dteTomorrow, CInt(Me.txtProductTypeID.Text))
            Dim dtScheduleNextDay As DataTable = New DataTable

            If dteNextDay <> SharedConstants.DEFAULT_DATE_VALUE Then
                dtScheduleNextDay = _Service.GetDailyProductionSchedule(dteNextDay, CInt(Me.txtProductTypeID.Text))
            End If

            Dim decNoPanels As Decimal = SharedFunctions.PerformDatatableSelectSum(dtScheduleToday, "TotalPanels", "")
            Dim decTotalSQM As Decimal = SharedFunctions.PerformDatatableSelectSum(dtScheduleToday, "TotalSQM", "")

            Me.lblScheduleTodayHeading.Text = "Scheduled for " & Format(dteToday, "ddd, d MMM yyyy")
            Me.lblScheduleToday.Text = "<b>No of Jobs: </b>" & dtScheduleToday.Rows.Count.ToString & "<br />"
            Me.lblScheduleToday.Text &= "<b>No of Panels: </b>" & decNoPanels.ToString & "<br />"
            Me.lblScheduleToday.Text &= "<b>Total SQM: </b>" & decTotalSQM.ToString("#.0")


            decNoPanels = SharedFunctions.PerformDatatableSelectSum(dtScheduleTomorrow, "TotalPanels", "")
            decTotalSQM = SharedFunctions.PerformDatatableSelectSum(dtScheduleTomorrow, "TotalSQM", "")

            Me.lblScheduleTomorrowHeading.Text = "Scheduled for " & Format(dteTomorrow, "ddd, d MMM yyyy")
            Me.lblScheduleTomorrow.Text = "<b>No of Jobs: </b>" & dtScheduleTomorrow.Rows.Count.ToString & "<br />"
            Me.lblScheduleTomorrow.Text &= "<b>No of Panels: </b>" & decNoPanels.ToString & "<br />"
            Me.lblScheduleTomorrow.Text &= "<b>Total SQM: </b>" & decTotalSQM.ToString("#.0")

            If dtScheduleNextDay.Columns.Count > 0 Then
                Me.pnlNextDay.Visible = True

                decNoPanels = SharedFunctions.PerformDatatableSelectSum(dtScheduleNextDay, "TotalPanels", "")
                decTotalSQM = SharedFunctions.PerformDatatableSelectSum(dtScheduleNextDay, "TotalSQM", "")

                Me.lblScheduleNextDayHeading.Text = "Scheduled for " & Format(dteNextDay, "ddd, d MMM yyyy")
                Me.lblScheduleNextDay.Text = "<b>No of Jobs: </b>" & dtScheduleNextDay.Rows.Count.ToString & "<br />"
                Me.lblScheduleNextDay.Text &= "<b>No of Panels: </b>" & decNoPanels.ToString & "<br />"
                Me.lblScheduleNextDay.Text &= "<b>Total SQM: </b>" & decTotalSQM.ToString("#.0")
            Else
                Me.pnlNextDay.Visible = False
                Me.lblScheduleNextDayHeading.Text = String.Empty
                Me.lblScheduleNextDay.Text = String.Empty
            End If

            dtScheduleToday.Dispose()
            dtScheduleToday = Nothing
            dtScheduleTomorrow.Dispose()
            dtScheduleTomorrow = Nothing
            dtScheduleNextDay.Dispose()
            dtScheduleNextDay = Nothing


            'section 1.1
            '-------
            'current high priority jobs
            Dim dtHighPriority As DataTable = _Service.RunSQLScheduling("select * from dbo.tblProductionScheduleList where OrderStatus in(2,3,6) and PriorityLevel=1 and ProductTypeID=" & Me.txtProductTypeID.Text)
            decNoPanels = SharedFunctions.PerformDatatableSelectSum(dtHighPriority, "TotalPanels", "")
            decTotalSQM = SharedFunctions.PerformDatatableSelectSum(dtHighPriority, "TotalSQM", "")

            Me.lblHighPriorityJobsHeading.Text = "Current High Priority Jobs"
            Me.lblHighPriorityJobs.Text = "<b>No of Jobs: </b>" & dtHighPriority.Rows.Count.ToString & "<br />"
            Me.lblHighPriorityJobs.Text &= "<b>No of Panels: </b>" & decNoPanels.ToString & "<br />"
            Me.lblHighPriorityJobs.Text &= "<b>Total SQM: </b>" & decTotalSQM.ToString("#.0")

            dtHighPriority.Dispose()
            dtHighPriority = Nothing

            'section 2
            '-------
            'jobs awaiting acceptance
            Dim cParams As New QueryParams.ProdScheduleQueryParams

            cParams.Statuses.Add(ProductionScheduleStatus.AwaitingAcceptance)
	    ' below line commented by Pradeep Singh dt 03-12-2020 Ticket#62498
            'cParams.Statuses.Add(ProductionScheduleStatus.PaperworkProcessing)
            cParams.ProductTypes.Add(CInt(txtProductTypeID.Text))

            Dim lProdSchedules As List(Of ProductionSchedule) = _Service.GetProdSchedulesByParameters(cParams)

            decNoPanels = lProdSchedules.Sum(Function(x) x.TotalPanels)
            decTotalSQM = lProdSchedules.Sum(Function(x) x.TotalSQM)

            lblAwaitAcceptanceHeading.Text = "Jobs Awaiting Acceptance"
            lblAwaitAcceptance.Text = "<b>No of Jobs: </b>" & lProdSchedules.Count & "<br />"
            lblAwaitAcceptance.Text &= "<b>No of Panels: </b>" & decNoPanels & "<br />"
            lblAwaitAcceptance.Text &= "<b>Total SQM: </b>" & decTotalSQM.ToString("#.0")

            'section 2.1
            '-------
            If Me.txtProductTypeID.Text = "1" Then
                'jobs awaiting entry to shutter pro
                'Dim dtEnterShutterPro As DataTable = New DataTable
                Dim dtEnterShutterPro As DataTable = _Service.RunSQLScheduling("select * from dbo.tblProductionScheduleList where OrderStatus=2 and ProductTypeID=" & Me.txtProductTypeID.Text)
                decNoPanels = SharedFunctions.PerformDatatableSelectSum(dtEnterShutterPro, "TotalPanels", "")
                decTotalSQM = SharedFunctions.PerformDatatableSelectSum(dtEnterShutterPro, "TotalSQM", "")

                Me.lblEnterShutterProHeading.Text = "Jobs To Be Entered into Shutter Pro"
                Me.lblEnterShutterPro.Text = "<b>No of Jobs: </b>" & dtEnterShutterPro.Rows.Count.ToString & "<br />"
                Me.lblEnterShutterPro.Text &= "<b>No of Panels: </b>" & decNoPanels.ToString & "<br />"
                Me.lblEnterShutterPro.Text &= "<b>Total SQM: </b>" & decTotalSQM.ToString("#.0")

                dtEnterShutterPro.Dispose()
                dtEnterShutterPro = Nothing
            Else
                Me.pnlShowShutterPro.Visible = False
            End If

            'section 3
            '-------
            'jobs to be despatched - link to page
            Dim dtToDespatch As DataTable = _Service.GetToBeDespatchedList(CInt(Me.txtProductTypeID.Text))
            decNoPanels = SharedFunctions.PerformDatatableSelectSum(dtToDespatch, "TotalPanels", "")
            decTotalSQM = SharedFunctions.PerformDatatableSelectSum(dtToDespatch, "TotalSQM", "")

            Me.lblToDespatchHeading.Text = "To Be Despatched"
            Me.lblToDespatch.Text = "<b>No of Jobs: </b>" & dtToDespatch.Rows.Count.ToString & "<br />"
            Me.lblToDespatch.Text &= "<b>No of Panels: </b>" & decNoPanels.ToString & "<br />"
            Me.lblToDespatch.Text &= "<b>Total SQM: </b>" & decTotalSQM.ToString("#.0")

            dtToDespatch.Dispose()
            dtToDespatch = Nothing

            'section 3.1
            '-------
            'rady for collection - link to page
            Dim dtToCollect As DataTable = _Service.GetToBeCollectedFromFactory(CInt(Me.txtProductTypeID.Text))
            decNoPanels = SharedFunctions.PerformDatatableSelectSum(dtToCollect, "TotalPanels", "")
            decTotalSQM = SharedFunctions.PerformDatatableSelectSum(dtToCollect, "TotalSQM", "")

            Me.lblToCollectHeading.Text = "Ready For Collection"
            Me.lblToCollect.Text = "<b>No of Jobs: </b>" & dtToCollect.Rows.Count.ToString & "<br />"
            Me.lblToCollect.Text &= "<b>No of Panels: </b>" & decNoPanels.ToString & "<br />"
            Me.lblToCollect.Text &= "<b>Total SQM: </b>" & decTotalSQM.ToString("#.0")

            dtToCollect.Dispose()
            dtToCollect = Nothing

            'section 3.2
            '-------
            'rady for invoice - link to page
            Dim dtToInvoice As DataTable = _Service.GetToBeInvoiced(CInt(Me.txtProductTypeID.Text))
            decNoPanels = SharedFunctions.PerformDatatableSelectSum(dtToInvoice, "TotalPanels", "")
            decTotalSQM = SharedFunctions.PerformDatatableSelectSum(dtToInvoice, "TotalSQM", "")

            ' Get failed invoices.
            Dim lFailedInvoices As List(Of InvoiceOrder) = GetFailedInvoices()
            Dim strFiledInvoiceMsg As String = String.Empty

            If lFailedInvoices.Count > 0 Then
                strFiledInvoiceMsg = " <span class='blink' style='color:red;font-weight:bold;'>(" & lFailedInvoices.Count & " error)</span>"
                intErrorCount += lFailedInvoices.Count
            End If

            Me.lblToInvoicedHeading.Text = "Ready For Invoice"
            Me.lblToInvoice.Text = "<b>No of Jobs: </b>" & dtToInvoice.Rows.Count.ToString & strFiledInvoiceMsg & "<br />"
            Me.lblToInvoice.Text &= "<b>No of Panels: </b>" & decNoPanels.ToString & "<br />"
            Me.lblToInvoice.Text &= "<b>Total SQM: </b>" & decTotalSQM.ToString("#.0")

            dtToInvoice.Dispose()
            dtToInvoice = Nothing

            'section 4
            '-------
            'jobs in progress (jobs on the schedule) - link to daily schedule page
            Dim dtInProgress As DataTable = _Service.RunSQLScheduling("select * from dbo.tblProductionScheduleList where OrderStatus=3 and ProductTypeID=" & Me.txtProductTypeID.Text)
            decNoPanels = SharedFunctions.PerformDatatableSelectSum(dtInProgress, "TotalPanels", "")
            decTotalSQM = SharedFunctions.PerformDatatableSelectSum(dtInProgress, "TotalSQM", "")

            Me.lblInProgressHeading.Text = "Work In Progress"
            Me.lblInProgress.Text = "<b>No of Jobs: </b>" & dtInProgress.Rows.Count.ToString & "<br />"
            Me.lblInProgress.Text &= "<b>No of Panels: </b>" & decNoPanels.ToString & "<br />"
            Me.lblInProgress.Text &= "<b>Total SQM: </b>" & decTotalSQM.ToString("#.0")

            dtInProgress.Dispose()
            dtInProgress = Nothing

            'section 5
            '-------
            'total jobs in system (WIP)
            cParams = New QueryParams.ProdScheduleQueryParams

            cParams.Statuses.Add(ProductionScheduleStatus.PaperworkProcessing)
            cParams.Statuses.Add(ProductionScheduleStatus.InProduction)
            cParams.Statuses.Add(ProductionScheduleStatus.EnteredIntoShutterPro)
            cParams.ProductTypes.Add(CInt(txtProductTypeID.Text))

            lProdSchedules = _Service.GetProdSchedulesByParameters(cParams)

            decNoPanels = lProdSchedules.Sum(Function(x) x.TotalPanels)
            decTotalSQM = lProdSchedules.Sum(Function(x) x.TotalSQM)

            lblWIPHeading.Text = "Work In System"
            lblWIP.Text = "<b>No of Jobs: </b>" & lProdSchedules.Count & "<br />"
            lblWIP.Text &= "<b>No of Panels: </b>" & decNoPanels & "<br />"
            lblWIP.Text &= "<b>Total SQM: </b>" & decTotalSQM.ToString("#.0")

            'section 5.1
            '-------
            'total remakes in system (WIP)
            Dim dtWIPRemakes As DataTable = _Service.RunSQLScheduling("select * from dbo.tblProductionScheduleList where OrderStatus in(2,3,6) and OrderTypeID=2 and ProductTypeID=" & Me.txtProductTypeID.Text)
            decNoPanels = SharedFunctions.PerformDatatableSelectSum(dtWIPRemakes, "TotalPanels", "")
            decTotalSQM = SharedFunctions.PerformDatatableSelectSum(dtWIPRemakes, "TotalSQM", "")

            Me.lblWIPRemakesHeading.Text = "Remakes In System"
            Me.lblWIPRemakes.Text = "<b>No of Jobs: </b>" & dtWIPRemakes.Rows.Count.ToString & "<br />"
            Me.lblWIPRemakes.Text &= "<b>No of Panels: </b>" & decNoPanels.ToString & "<br />"
            Me.lblWIPRemakes.Text &= "<b>Total SQM: </b>" & decTotalSQM.ToString("#.0")

            dtWIPRemakes.Dispose()
            dtWIPRemakes = Nothing

            'section 5.2
            '-------
            'total reorders in system (WIP)
            Dim dtWIPReorders As DataTable = _Service.RunSQLScheduling("select * from dbo.tblProductionScheduleList where OrderStatus in(2,3,6) and OrderTypeID=3 and ProductTypeID=" & Me.txtProductTypeID.Text)
            decNoPanels = SharedFunctions.PerformDatatableSelectSum(dtWIPReorders, "TotalPanels", "")
            decTotalSQM = SharedFunctions.PerformDatatableSelectSum(dtWIPReorders, "TotalSQM", "")

            Me.lblWIPReordersHeading.Text = "Reorders In System"
            Me.lblWIPReorders.Text = "<b>No of Jobs: </b>" & dtWIPReorders.Rows.Count.ToString & "<br />"
            Me.lblWIPReorders.Text &= "<b>No of Panels: </b>" & decNoPanels.ToString & "<br />"
            Me.lblWIPReorders.Text &= "<b>Total SQM: </b>" & decTotalSQM.ToString("#.0")

            dtWIPReorders.Dispose()
            dtWIPReorders = Nothing

            checkPermissions()

        End If

        pnlWarning.Visible = intErrorCount > 0

    End Sub

    Private Function GetFailedInvoices() As List(Of InvoiceOrder)
        Dim cParams As New QueryParams.InvoiceOrderQueryParams

        cParams.ProductTypeID = ProductType.Louvres
        cParams.ErrorState = 1

        Return _Service.GetInvoiceOrdersByParams(cParams)
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

    Protected Sub btnProductionList_Click(sender As Object, e As System.EventArgs) Handles btnProductionList.Click

        Response.Redirect("ProductionScheduleList.aspx", False)

    End Sub

    Protected Sub btnScheduleDateUpdate_Click(sender As Object, e As System.EventArgs) Handles btnScheduleDateUpdate.Click

        Response.Redirect("ScheduleDateUpdate.aspx", False)

    End Sub

    Protected Sub btnPlannedShippingDate_Click(sender As Object, e As System.EventArgs) Handles btnPlannedShippingDate.Click

        Response.Redirect("PlannedShippingDateUpdate.aspx", False)

    End Sub

    Protected Sub btnPricingMatrix_Click(sender As Object, e As EventArgs) Handles btnPricingMatrix.Click
        ' Dim encString As String = HttpUtility.UrlEncode(SharedFunctions.Encrypt("OrderStatus=0,customerid=" + CStr(Session("sessCustomerID"))))
        ' Dim encString As String = "PM"
        'Response.Redirect("ProductionScheduleList.aspx?var1=" + encString, False)
        'ScheduleId=0&ViewType=0

        Dim encString As String = "PM"
        Response.Redirect("LouvreJobDetailsQuick.aspx?ScheduleId=0&ViewType=0&var1=" + encString, False)
        ' Response.Redirect("ProductionScheduleList.aspx", False)
    End Sub

    Protected Sub checkPermissions()
        Dim service As New AppService
        Dim cUserPermissions As UserPermissionsContainer = _Service.Permissions.UserHasPermissions(CInt(Session("sessUserID")))

        'production schedule search button
        btnProductionList.Visible = cUserPermissions.HasPermission(SharedEnums.Permissions.AddOrderButton)

        'daily schhedule update
        pnlDailySchedule.Visible = cUserPermissions.HasPermission(SharedEnums.Permissions.TrackOrderButton)

        'schedule date update
        btnScheduleDateUpdate.Visible = cUserPermissions.HasPermission(SharedEnums.Permissions.AddLouvreButton)

        'est shipping date update
        btnPlannedShippingDate.Visible = cUserPermissions.HasPermission(SharedEnums.Permissions.TrackLouvreButton)

        'awaiting acceptance
        pnlAwaitAcceptance.Visible = cUserPermissions.HasPermission(SharedEnums.Permissions.CreateLoginButton)

        'to be entered shutter pro
        pnlShutterPro.Visible = cUserPermissions.HasPermission(SharedEnums.Permissions.TicketsButton)

        'to be despatched
        pnlToDespatch.Visible = cUserPermissions.HasPermission(SharedEnums.Permissions.NotDecidedYet)

        'to be collected
        pnlCollectFactory.Visible = cUserPermissions.HasPermission(SharedEnums.Permissions.CollectFactory)

        'to be invoiced
        pnlAwaitingInvoice.Visible = cUserPermissions.HasPermission(SharedEnums.Permissions.AwaitingInvoice)

        'high priority jobs
        pnlHighPriority.Visible = cUserPermissions.HasPermission(SharedEnums.Permissions.DeleteNote)

        'Customer Sample
        btnCustomerSample.Visible = cUserPermissions.HasPermission(SharedEnums.Permissions.CustomerSample)

    End Sub


    Private Sub btnLouvreReport_Click(sender As Object, e As EventArgs) Handles btnLouvreReport.Click

        Response.Redirect("LouvreProductionReport.aspx", False)

        'Response.Redirect("testpage.aspx", False)

    End Sub

    Private Sub btnCustomers_Click(sender As Object, e As EventArgs) Handles btnCustomers.Click

        Response.Redirect("CustomerDetails.aspx", False)

    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click

        Response.Redirect("Reports.aspx", False)

    End Sub

    Private Sub btnStockPicking_Click(sender As Object, e As EventArgs) Handles btnStockPicking.Click

        Response.Redirect("UpdateStockPicking.aspx", False)


    End Sub

    Private Sub btnCustomerSample_Click(sender As Object, e As EventArgs) Handles btnCustomerSample.Click

        Response.Redirect("CustomerSampleTracking.aspx", False)


    End Sub


    Private Sub btnDeliveryDocket_Click(sender As Object, e As EventArgs) Handles btnDeliveryDocket.Click

        LouvreJobDeliveryDocket.GetLouvreJobDeliveryDocket(10079)

    End Sub

    Private Sub btnRunningSheet_Click(sender As Object, e As EventArgs) Handles btnRunningSheet.Click

        LouvreJobRunningSheet.GetJobRunningSheetPDF(10079)


    End Sub

    Private Sub btnTimeSheet_Click(sender As Object, e As EventArgs) Handles btnTimeSheet.Click

        LouvreJobTimeSheet.JobTimeSheet(10079)

    End Sub

    Private Sub btnCoverSheet_Click(sender As Object, e As EventArgs) Handles btnCoverSheet.Click
        Dim cCoverSheet As New LouvreJobCoverSheet

        cCoverSheet.GetProjectCoverSheetPDF(10079)
    End Sub

    Private Sub btnAdmin_Click(sender As Object, e As EventArgs) Handles btnAdmin.Click
        Response.Redirect("AdminPanel.aspx", False)
    End Sub


End Class
