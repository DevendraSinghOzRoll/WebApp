Imports OzrollPSLVSchedulingModel.SharedEnums
Imports OzrollPSLVSchedulingModel.SharedFunctions
Imports OzrollPSLVSchedulingModel.Reporting.ReportEnums

Partial Class Reports
    Inherits System.Web.UI.Page

    Private Sub Reports_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            ConfigureUIForReport(0)
        End If
    End Sub

    Private Sub ddlReportSelector_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlReportSelector.SelectedIndexChanged
        ConfigureUIForReport(ddlReportSelector.SelectedValue)
    End Sub

    Private Sub ConfigureUIDefaults()

        lblReportNote.Text = String.Empty
        pnlReportConfig.Visible = False

        ddlDateType.Visible = False
        ddlDateType.Items.Clear()
        ddlDateType.Items.Add(New ListItem(String.Empty, Reporting.ReportEnums.DateTypeID.NONE))

        txtStartDate.Visible = False
        txtStartDate.Text = String.Empty
        txtEndDate.Visible = False
        txtEndDate.Text = String.Empty

        btnGenerateReportExcel.Visible = False

        ViewState("ReportID") = ReportID.NONE
    End Sub

    Private Sub ConfigureUIForReport(enumReportID As ReportID)

        ConfigureUIDefaults()

        Select Case enumReportID
            Case ReportID.SalesReport

                pnlReportConfig.Visible = True

                ddlDateType.Visible = True

                texthide.Visible = True
                texthide2.Visible = True
                texthide3.Visible = True

                ddlDateType.Items.Clear()
                ddlDateType.Items.Add(New ListItem(String.Empty, Reporting.ReportEnums.DateTypeID.NONE))
                ddlDateType.Items.Add(New ListItem("Received", Reporting.ReportEnums.DateTypeID.Received))
                ddlDateType.Items.Add(New ListItem("Invoiced", Reporting.ReportEnums.DateTypeID.Invoiced))
                ddlDateType.Enabled = True

                txtStartDate.Visible = True
                txtEndDate.Visible = True

                btnGenerateReportExcel.Visible = True

                ViewState("ReportID") = CInt(enumReportID)

            Case ReportID.JobCost

                pnlReportConfig.Visible = True

                ddlDateType.Visible = True
                texthide.Visible = True
                texthide2.Visible = True
                texthide3.Visible = True

                ddlDateType.Items.Clear()
                ddlDateType.Items.Add(New ListItem("Invoiced", Reporting.ReportEnums.DateTypeID.Invoiced))
                ddlDateType.Enabled = True

                txtStartDate.Visible = True
                txtEndDate.Visible = True

                btnGenerateReportExcel.Visible = True

                ViewState("ReportID") = CInt(enumReportID)

                lblReportNote.Text = "<br/><br/><span style='color:red;font-style:italic;font-weight:bold;'>* Cost data is accurate from 20th June 2019.</span>"

            Case ReportID.ProductionReport

                pnlReportConfig.Visible = True

                txtStartDate.Visible = False
                txtEndDate.Visible = False
                texthide.Visible = False
                texthide2.Visible = False
                texthide3.Visible = False

                ddlDateType.Visible = True
                ddlDateType.Items.Clear()
                ddlDateType.Items.Add(New ListItem(" ", -1))
                ddlDateType.Enabled = False

                txtStartDate.Text = "1 Aug 2017"
                txtEndDate.Text = "30 Aug 2020"

                btnGenerateReportExcel.Visible = True

                ViewState("ReportID") = CInt(enumReportID)

                'added by FL #63717 08-02-2021
            Case ReportID.OrderIntakeReport

                pnlReportConfig.Visible = True

                ddlDateType.Visible = True

                texthide.Visible = True
                texthide2.Visible = True
                texthide3.Visible = True

                ddlDateType.Items.Clear()
                ddlDateType.Items.Add(New ListItem(String.Empty, Reporting.ReportEnums.DateTypeID.NONE))
                ddlDateType.Items.Add(New ListItem("Received", Reporting.ReportEnums.DateTypeID.Received))
                ddlDateType.Items.Add(New ListItem("Invoiced", Reporting.ReportEnums.DateTypeID.Invoiced))
                ddlDateType.Enabled = True

                txtStartDate.Visible = True
                txtEndDate.Visible = True

                btnGenerateReportExcel.Visible = True

                ViewState("ReportID") = CInt(enumReportID)
                'added by FL #63717 08-02-2021

        End Select

    End Sub

    Private Sub btnGenerateReportExcel_Click(sender As Object, e As EventArgs) Handles btnGenerateReportExcel.Click
        Page.Validate()

        If Page.IsValid() Then
            Dim enumReportID As ReportID = ViewState("ReportID")

            Select Case enumReportID
                Case ReportID.SalesReport
                    Dim cReport As New OzrollPSLVSchedulingModel.Reporting.SalesReport
                    Dim cConfig As New Reporting.SalesReport.SalesReportConfig

                    With cConfig
                        .DateType = ddlDateType.SelectedValue
                        .StartDate = txtStartDate.Text.Trim
                        .EndDate = txtEndDate.Text.Trim
                        .ProductType = ProductType.Louvres
                    End With

                    ReportToResponse("SalesReport_" & cConfig.ProductType.ToString & ".xlsx", cReport.GenerateExcel(cConfig))

                Case ReportID.JobCost
                    Dim cReport As New OzrollPSLVSchedulingModel.Reporting.JobCostReport
                    Dim cConfig As New Reporting.JobCostReport.JobCostReportConfig

                    With cConfig
                        .DateType = ddlDateType.SelectedValue
                        .StartDate = txtStartDate.Text.Trim
                        .EndDate = txtEndDate.Text.Trim
                        .ProductType = ProductType.Louvres
                    End With

                    ReportToResponse("SalesReport_" & cConfig.ProductType.ToString & ".xlsx", cReport.GenerateExcel(cConfig))

                Case ReportID.ProductionReport
                    Dim cReport As New OzrollPSLVSchedulingModel.Reporting.LouvresPlantationDespatchReport
                    Dim cConfig As New Reporting.LouvresPlantationDespatchReport.ProductionReportConfig

                    With cConfig
                        '.StartDate = txtStartDate.Text.Trim
                        '.EndDate = txtEndDate.Text.Trim
                        .ProductType = ProductType.Louvres
                    End With

                    ReportToResponse("ProductionReport_" & cConfig.ProductType.ToString & ".xlsx", cReport.GenerateExcel(cConfig))

                    'added by FL #63717 08-02-2021
                Case ReportID.OrderIntakeReport
                    Dim cReport As New OzrollPSLVSchedulingModel.Reporting.OrderIntakeReport
                    Dim cConfig As New Reporting.OrderIntakeReport.OrderIntakeReportConfig

                    With cConfig
                        .DateType = ddlDateType.SelectedValue
                        .StartDate = txtStartDate.Text.Trim
                        .EndDate = txtEndDate.Text.Trim
                        .ProductType = ProductType.Louvres
                    End With

                    ReportToResponse("OrderIntakeReport_" & cConfig.ProductType.ToString & ".xlsx", cReport.GenerateExcel(cConfig))
                    'added by FL #63717 08-02-2021
            End Select

        End If
    End Sub

    Public Sub ReportToResponse(strFileName As String, bytesExcel As Byte())

        Page.Response.Clear()
        Page.Response.ContentType = ResponseContentType(ProductionScheduleFileType.XLSX)
        Page.Response.AddHeader("Content-Disposition", "attachment;filename=" & strFileName)
        Page.Response.BinaryWrite(bytesExcel)
        Page.Response.End()
    End Sub

    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        Response.Redirect("Logout.aspx", False)
    End Sub

    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        Response.Redirect("Home.aspx", False)
    End Sub

#Region "Validation"

    Protected Sub valcustDate_Validate(source As Object, e As ServerValidateEventArgs)
        Dim dteDate As Date

        e.IsValid = False

        If Not String.IsNullOrWhiteSpace(e.Value.Trim) Then
            e.IsValid = Date.TryParse(e.Value, dteDate)
        End If
    End Sub

#End Region

End Class
