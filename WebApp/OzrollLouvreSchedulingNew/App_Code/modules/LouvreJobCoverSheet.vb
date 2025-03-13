Imports Microsoft.VisualBasic
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.draw

Public Class LouvreJobCoverSheet

    Public intTotalPages As Integer

    Public Sub GetProjectCoverSheetPDF(intScheduleID As Integer)
        Dim Response = HttpContext.Current.Response
        Dim objBuffer() As Byte
        Dim output As MemoryStream = New MemoryStream()
        Dim document As Document = New Document(PageSize.A4, 50, 50, 25, 25)
        Dim writer As PdfWriter = PdfWriter.GetInstance(document, output)
        Dim cData As LouvreJobCoverSheetData = CacheData(intScheduleID)

        document.SetMargins(15, 15, 325, 50)
        writer.PageEvent = New Header(cData)
        document.Open()
        output = CreateCoverSheetPDF(cData, output, document, writer)

        objBuffer = output.ToArray

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("Content-Disposition", "attachment;filename=" & cData.strFileName)
        Response.BinaryWrite(output.ToArray())
        Response.End()

    End Sub

    Private Function CacheData(intScheduleID As Integer) As LouvreJobCoverSheetData
        Dim cData As New LouvreJobCoverSheetData
        Dim service As New AppService

        cData.cProductionSchedule = service.getProdScheduleClsByID(intScheduleID)
        cData.cCustomer = service.GetCustomerByID(cData.cProductionSchedule.CustomerID)
        cData.cDeliveryAddress = service.getAddressByID(cData.cProductionSchedule.DeliveryAddressID)


        Dim dtPhysicalAddresses As List(Of Address) = service.getAddressesByCustomerIDAndAddressType(cdata.cProductionSchedule.CustomerID, SharedEnums.AddressType.Physical)

        cData.cPhysicalAddress = dtPhysicalAddresses.Find(Function(x) x.IsPrimary AndAlso Not x.Discontinued)

        If cData.cPhysicalAddress Is Nothing Then
            ' Will be blank if non existant. Prevent crash if not found.
            cData.cPhysicalAddress = New Address
        End If

        cData.lLouvreDetails = service.getLouvreDetailsCollectionByProductionScheduleID(intScheduleID).RemoveDeleted

        If cData.lLouvreDetails Is Nothing Then
            ' Blank list.
            cData.lLouvreDetails = New List(Of LouvreDetails)
        End If

        cData.cLouvreSpecs = service.getLouvreSpecsByProductionScheduleID(cData.cProductionSchedule.ID)
        cData.cJobType = service.getJobTypeByID(cData.cLouvreSpecs.LouvreJobTypeID)
        cData.lLouvreStyles = service.getLouvreStyles()
        cData.cOrderType = service.getOrderTypeByID(cData.cProductionSchedule.OrderTypeID)
        cData.lColours = service.getColours()

        cData.strFileName = "ProjectCoverSheet_" & cData.cProductionSchedule.ShutterProNumber & "_" & Format(cData.cProductionSchedule.EnteredDatetime, "yyyyMMdd") & ".pdf"

        Return cData
    End Function

    Public Function CreateCoverSheetPDF(cData As LouvreJobCoverSheetData, ByRef output As MemoryStream, document As Document, writer As PdfWriter) As MemoryStream
        Dim baseFont2 As Font = New Font(Font.HELVETICA, 2.0F, Font.NORMAL)
        Dim BaseFont As Font = New Font(Font.HELVETICA, 10.5F, Font.NORMAL)
        Dim baseFont8 As Font = New Font(Font.HELVETICA, 8.0F, Font.NORMAL)
        Dim baseFontBold12 As Font = New Font(Font.HELVETICA, 12.0F, Font.BOLD)
        Dim baseFontBold As Font = New Font(Font.HELVETICA, 10.5F, Font.BOLD)
        Dim baseFontBoldRed As Font = New Font(Font.HELVETICA, 10.5F, Font.BOLD, Color.RED)
        Dim baseFontBold8 As Font = New Font(Font.HELVETICA, 8.0F, Font.BOLD)

        ' ACCOUNT INFORMATION HEADER
        Dim pageAccountInfoHeader As New PdfPTable({100})

        pageAccountInfoHeader.WidthPercentage = 100.0F
        pageAccountInfoHeader.DefaultCell.Border = Rectangle.NO_BORDER
        pageAccountInfoHeader.AddCell(New PdfPCell(New Phrase("Account Information", baseFontBold12)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_TOP})
        pageAccountInfoHeader.SpacingAfter = 10.0F
        pageAccountInfoHeader.SpacingBefore = 5.0F
        document.Add(pageAccountInfoHeader)

        ' ACCOUNT INFORMATION
        Const ACCOUNT_INFORMATION_COLUMN_1_WIDTH As Single = 15.0
        Const ACCOUNT_INFORMATION_COLUMN_2_WIDTH As Single = 2.5
        Const ACCOUNT_INFORMATION_COLUMN_3_WIDTH As Single = 60.0
        Const ACCOUNT_INFORMATION_COLUMN_4_WIDTH As Single = 22.5

        Dim pageAccountInfo As New PdfPTable({ACCOUNT_INFORMATION_COLUMN_1_WIDTH, ACCOUNT_INFORMATION_COLUMN_2_WIDTH, ACCOUNT_INFORMATION_COLUMN_3_WIDTH, ACCOUNT_INFORMATION_COLUMN_4_WIDTH})

        With pageAccountInfo
            .WidthPercentage = 100.0F
            .DefaultCell.Border = Rectangle.NO_BORDER

            .AddCell(New PdfPCell(New Phrase("Checklist ", baseFontBold8)) _
                    With {.FixedHeight = 20, .BorderWidthBottom = 0, .BorderWidth = 0, .PaddingBottom = 5, .VerticalAlignment = Element.ALIGN_TOP, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

            .AddCell(New PdfPCell(New Phrase("Credit Limit: ", baseFont8)) _
                    With {.BorderWidthBottom = 0, .BorderWidth = 0, .VerticalAlignment = Element.ALIGN_CENTER, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 0.5, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

            .AddCell(New PdfPCell(New Phrase("> $10K Approved By: ", baseFont8)) _
                    With {.FixedHeight = 20, .BorderWidthBottom = 0, .BorderWidth = 0, .VerticalAlignment = Element.ALIGN_BOTTOM, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 1, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 1, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

            .AddCell(New PdfPCell(New Phrase("Authorised By: ", baseFont8)) _
                    With {.FixedHeight = 20, .BorderWidthBottom = 0, .BorderWidth = 0, .VerticalAlignment = Element.ALIGN_BOTTOM, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 1, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 1, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

            .SpacingAfter = 100.0F
        End With

        document.Add(pageAccountInfo)

        ' AUTHORISATION
        Const AUTHORISATION_COLUMN_1_WIDTH As Single = 15.0
        Const AUTHORISATION_COLUMN_2_WIDTH As Single = 35.0
        Const AUTHORISATION_COLUMN_3_WIDTH As Single = 5.0
        Const AUTHORISATION_COLUMN_4_WIDTH As Single = 40.0

        Dim pageAuthorisation As New PdfPTable({AUTHORISATION_COLUMN_1_WIDTH, AUTHORISATION_COLUMN_2_WIDTH, AUTHORISATION_COLUMN_3_WIDTH, AUTHORISATION_COLUMN_4_WIDTH})

        With pageAuthorisation
            .WidthPercentage = 100.0F
            .DefaultCell.Border = Rectangle.NO_BORDER

            .AddCell(New PdfPCell(New Phrase("Authorised to Proceed ", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 1, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase("  Date", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 1, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

            .SpacingAfter = 10.0F
        End With

        document.Add(pageAuthorisation)

        ' PAGE 2

        ' Only print page if retail job type.
        If cData.cJobType.ID = SharedEnums.JobType.Retail Then

            document.NewPage()

            ' RETAIL CONTRACT CHECKLIST
            Const RETAIL_CONTRACT_CHECKLIST_COLUMN_1_WIDTH As Single = 20.0
            Const RETAIL_CONTRACT_CHECKLIST_COLUMN_2_WIDTH As Single = 30.0
            Const RETAIL_CONTRACT_CHECKLIST_COLUMN_3_WIDTH As Single = 20.0

            Dim pageContractChecklistTitle As New PdfPTable({RETAIL_CONTRACT_CHECKLIST_COLUMN_1_WIDTH, RETAIL_CONTRACT_CHECKLIST_COLUMN_2_WIDTH, RETAIL_CONTRACT_CHECKLIST_COLUMN_3_WIDTH})

            With pageContractChecklistTitle
                .WidthPercentage = 100.0F
                .DefaultCell.Border = Rectangle.NO_BORDER

                .SpacingBefore = 10.0F
                .SpacingAfter = 5.0F

                .AddCell(String.Empty)
                .AddCell(New PdfPCell(New Phrase("Retail Contract Checklist", baseFontBold12)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER, .VerticalAlignment = Element.ALIGN_TOP})
                .AddCell(String.Empty)
            End With

            document.Add(pageContractChecklistTitle)

            ' CONTRACT CHECKLIST MAIN 2
            Const CONTRACT_CHECKLIST_MAIN_2_COLUMN_1_WIDTH As Single = 50.0
            Const CONTRACT_CHECKLIST_MAIN_2_COLUMN_2_WIDTH As Single = 50.0

            Dim pageContractChecklistMain As New PdfPTable({CONTRACT_CHECKLIST_MAIN_2_COLUMN_1_WIDTH, CONTRACT_CHECKLIST_MAIN_2_COLUMN_2_WIDTH})

            With pageContractChecklistMain
                .WidthPercentage = 100.0F
                .DefaultCell.Border = Rectangle.NO_BORDER

                .SpacingBefore = 5.0F
                .SpacingAfter = 5.0F
            End With

            ' CONTRACT CHECKLIST LHS
            Const CONTRACT_CHECKLIST_LHS_COLUMN_1_WIDTH As Single = 2.0
            Const CONTRACT_CHECKLIST_LHS_COLUMN_2_WIDTH As Single = 2.0
            Const CONTRACT_CHECKLIST_LHS_COLUMN_3_WIDTH As Single = 2.0
            Const CONTRACT_CHECKLIST_LHS_COLUMN_4_WIDTH As Single = 44.0

            Dim pageContractChecklistLeft As New PdfPTable({CONTRACT_CHECKLIST_LHS_COLUMN_1_WIDTH,
                                                                CONTRACT_CHECKLIST_LHS_COLUMN_2_WIDTH,
                                                                CONTRACT_CHECKLIST_LHS_COLUMN_3_WIDTH,
                                                                CONTRACT_CHECKLIST_LHS_COLUMN_4_WIDTH})

            With pageContractChecklistLeft
                .WidthPercentage = 50.0F
                .DefaultCell.Border = Rectangle.NO_BORDER

                .SpacingBefore = 5.0F
                .SpacingAfter = 5.0F

                Dim checkbox As New CheckBox()

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0.5, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Entered to Sybiz by _____________________", baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0.5, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Figures checked by _____________________", baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0.5, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Full Deposit Received _____________________", baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0.5, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Guarantee and Instructions Sent _____________________", baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont2)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

            End With

            ' CHECKLIST HOLDER CELL
            Const CHECKLIST_HOLDER_COLUMN_1_WIDTH As Single = 50

            Dim pageContractChecklistHolderCell As New PdfPTable({CHECKLIST_HOLDER_COLUMN_1_WIDTH})

            With pageContractChecklistHolderCell
                .WidthPercentage = 50.0F
                .DefaultCell.Border = Rectangle.NO_BORDER

                .SpacingBefore = 5.0F
                .SpacingAfter = 0.0F
            End With

            pageContractChecklistHolderCell.AddCell(pageContractChecklistLeft)

            ' DISCOUNT COMISSION TABLE
            Const DISCOUNT_COMISSION_COLUMN_1_WIDTH As Single = 15.0
            Const DISCOUNT_COMISSION_COLUMN_2_WIDTH As Single = 15.0
            Const DISCOUNT_COMISSION_COLUMN_3_WIDTH As Single = 20.0

            Dim pageContractDiscountTable As New PdfPTable({DISCOUNT_COMISSION_COLUMN_1_WIDTH,
                                                                DISCOUNT_COMISSION_COLUMN_2_WIDTH,
                                                                DISCOUNT_COMISSION_COLUMN_3_WIDTH})

            With pageContractDiscountTable
                .WidthPercentage = 50.0F
                .DefaultCell.Border = Rectangle.NO_BORDER

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.FixedHeight = 11, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("Discount", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = Color.LIGHT_GRAY})
                .AddCell(New PdfPCell(New Phrase("Commission", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = Color.LIGHT_GRAY})
                .AddCell(New PdfPCell(New Phrase("Amount", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = Color.LIGHT_GRAY})

                .AddCell(New PdfPCell(New Phrase("0%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("15.00%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("1%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("15.00%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("2%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("14.75%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("3%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("14.50%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("4%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("14.25%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("5%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("14.00%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("6%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("13.75%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("7%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("13.50%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("8%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("13.25%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("9%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("13.00%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("10%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("12.75%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("11%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("12.50%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("12%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("11.50%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("13%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("11.00%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("14%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("10.50%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("15%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("10.00%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("16%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("9.00%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("17%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("8.00%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("18%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("7.00%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("19%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("6.00%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase("20%", baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("5.00%", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

                .SpacingBefore = 0.0F
            End With

            pageContractChecklistHolderCell.AddCell(pageContractDiscountTable)

            pageContractChecklistMain.AddCell(pageContractChecklistHolderCell)


            ' TABLE HOLDER CELL
            Const TABLE_HOLDER_COLUMN_1_WIDTH As Single = 50

            Dim pageContractTableHolderCell As New PdfPTable({TABLE_HOLDER_COLUMN_1_WIDTH})

            With pageContractTableHolderCell
                .WidthPercentage = 50.0F
                .DefaultCell.Border = Rectangle.NO_BORDER

                .SpacingBefore = 5.0F
                .SpacingAfter = 5.0F
            End With

            ' PRODUCT TABLE RHS 3
            Const PRODUCT_TABLE_COLUMN_1_WIDTH As Single = 2.0
            Const PRODUCT_TABLE_COLUMN_2_WIDTH As Single = 11.5
            Const PRODUCT_TABLE_COLUMN_3_WIDTH As Single = 11.5
            Const PRODUCT_TABLE_COLUMN_4_WIDTH As Single = 11.5
            Const PRODUCT_TABLE_COLUMN_5_WIDTH As Single = 2.0

            Dim pageContractProductTable As New PdfPTable({PRODUCT_TABLE_COLUMN_1_WIDTH,
                                                                 PRODUCT_TABLE_COLUMN_2_WIDTH,
                                                                 PRODUCT_TABLE_COLUMN_3_WIDTH,
                                                                 PRODUCT_TABLE_COLUMN_4_WIDTH,
                                                                 PRODUCT_TABLE_COLUMN_5_WIDTH})

            With pageContractProductTable
                .WidthPercentage = 50.0F
                .DefaultCell.Border = Rectangle.NO_BORDER

                .SpacingBefore = 5.0F
                .SpacingAfter = 5.0F

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Product", baseFontBold8)) With {.Colspan = 2, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = Color.LIGHT_GRAY})
                .AddCell(New PdfPCell(New Phrase("Goods", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = Color.LIGHT_GRAY})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Rate", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Amount", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Louvre (Less GST)", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Total Due:", baseFontBold8)) With {.BorderWidth = 1, .Colspan = 2, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.FixedHeight = 15, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .Colspan = 2, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

            End With

            pageContractTableHolderCell.AddCell(pageContractProductTable)

            ' INSTALLER TABLE
            Const INSTALLER_TABLE_COLUMN_1_WIDTH As Single = 3.0
            Const INSTALLER_TABLE_COLUMN_2_WIDTH As Single = 15.0
            Const INSTALLER_TABLE_COLUMN_3_WIDTH As Single = 15.0
            Const INSTALLER_TABLE_COLUMN_4_WIDTH As Single = 15.0
            Const INSTALLER_TABLE_COLUMN_5_WIDTH As Single = 2.0

            Dim pageContractInstallerTable As New PdfPTable({INSTALLER_TABLE_COLUMN_1_WIDTH,
                                                                      INSTALLER_TABLE_COLUMN_2_WIDTH,
                                                                      INSTALLER_TABLE_COLUMN_3_WIDTH,
                                                                      INSTALLER_TABLE_COLUMN_4_WIDTH,
                                                                      INSTALLER_TABLE_COLUMN_5_WIDTH})

            With pageContractInstallerTable
                .WidthPercentage = 50.0F
                .DefaultCell.Border = Rectangle.NO_BORDER

                .SpacingBefore = 5.0F
                .SpacingAfter = 5.0F

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.FixedHeight = 15, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Installer", baseFontBold8)) With {.Colspan = 3, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = Color.LIGHT_GRAY})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.FixedHeight = 15, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Name", baseFontBold8)) With {.Colspan = 3, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.FixedHeight = 15, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Date Installed", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase("Date", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("$", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.FixedHeight = 15, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.FixedHeight = 10, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
            End With

            pageContractTableHolderCell.AddCell(pageContractInstallerTable)

            ' SALES COMISSION TABLE
            Const SALES_COMISSION_TABLE_COLUMN_1_WIDTH As Single = 8.0
            Const SALES_COMISSION_TABLE_COLUMN_2_WIDTH As Single = 10.0
            Const SALES_COMISSION_TABLE_COLUMN_3_WIDTH As Single = 10.0
            Const SALES_COMISSION_TABLE_COLUMN_4_WIDTH As Single = 10.0
            Const SALES_COMISSION_TABLE_COLUMN_5_WIDTH As Single = 10.0
            Const SALES_COMISSION_TABLE_COLUMN_6_WIDTH As Single = 2.0

            Dim pageContractSalesComissionTable As New PdfPTable({SALES_COMISSION_TABLE_COLUMN_1_WIDTH,
                                                                 SALES_COMISSION_TABLE_COLUMN_2_WIDTH,
                                                                 SALES_COMISSION_TABLE_COLUMN_3_WIDTH,
                                                                 SALES_COMISSION_TABLE_COLUMN_4_WIDTH,
                                                                 SALES_COMISSION_TABLE_COLUMN_5_WIDTH,
                                                                 SALES_COMISSION_TABLE_COLUMN_6_WIDTH})

            With pageContractSalesComissionTable
                .WidthPercentage = 50.0F
                .DefaultCell.Border = Rectangle.NO_BORDER

                .SpacingBefore = 5.0F
                .SpacingAfter = 5.0F

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.FixedHeight = 20, .Colspan = 6, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.FixedHeight = 15, .Colspan = 2, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Sales Comission", baseFontBold8)) With {.Colspan = 3, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = Color.LIGHT_GRAY})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.FixedHeight = 15, .Colspan = 2, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("% Paid", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Date", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("$", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.FixedHeight = 15, .Colspan = 2, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 1, .FixedHeight = 15, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.FixedHeight = 15, .Colspan = 2, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase("Salesperson", baseFontBold8)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = Color.LIGHT_GRAY})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 1, .Colspan = 2, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.FixedHeight = 15, .Colspan = 2, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
            End With

            pageContractTableHolderCell.AddCell(pageContractSalesComissionTable)

            pageContractChecklistMain.AddCell(pageContractTableHolderCell)

            document.Add(pageContractChecklistMain)

        End If

        intTotalPages = writer.PageNumber

        document.Close()

        Return output

    End Function

    Public Class Header
        Inherits PdfPageEventHelper

        Private _LouvreJobCoverSheetData As LouvreJobCoverSheetData

        Public Sub New(cLouvreJobCoverSheetData As LouvreJobCoverSheetData)
            _LouvreJobCoverSheetData = cLouvreJobCoverSheetData
        End Sub

        Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document)
            Dim pageSize As Rectangle = document.PageSize
            Dim baseFont2 As Font = New Font(Font.HELVETICA, 2.0F, Font.NORMAL)
            Dim BaseFont As Font = New Font(Font.HELVETICA, 10.5F, Font.NORMAL)
            Dim baseFont8 As Font = New Font(Font.HELVETICA, 8.0F, Font.NORMAL)
            Dim baseFontBold12 As Font = New Font(Font.HELVETICA, 12.0F, Font.BOLD)
            Dim baseFontBold As Font = New Font(Font.HELVETICA, 10.5F, Font.BOLD)
            Dim baseFontBoldRed As Font = New Font(Font.HELVETICA, 10.5F, Font.BOLD, Color.RED)
            Dim baseFontBold8 As Font = New Font(Font.HELVETICA, 8.0F, Font.BOLD)

            'Footer Right

            Dim dateValue As Date = DateTime.Now
            Dim footerDate As Paragraph = New Paragraph(dateValue.ToString, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL))
            footerDate.Alignment = Element.ALIGN_LEFT
            Dim footerTblDate As PdfPTable = New PdfPTable(1)
            footerTblDate.TotalWidth = 300
            footerTblDate.HorizontalAlignment = Element.ALIGN_CENTER
            Dim cell4 As PdfPCell = New PdfPCell(footerDate)
            cell4.Border = 0
            cell4.PaddingRight = 100
            footerTblDate.AddCell(cell4)
            footerTblDate.WriteSelectedRows(0, 425, pageSize.GetLeft(450), 30, writer.DirectContent)

            'Header Text

            Dim header As Paragraph = New Paragraph("Job Cover Sheet", FontFactory.GetFont(Font.HELVETICA, 20.0F, Font.BOLD, Color.GRAY))
            header.Alignment = Element.ALIGN_CENTER
            Dim headertblText As PdfPTable = New PdfPTable(1)
            headertblText.TotalWidth = 300
            headertblText.HorizontalAlignment = Element.ALIGN_CENTER
            Dim cell2 As PdfPCell = New PdfPCell(header)
            cell2.Border = 0
            cell2.PaddingRight = 100
            headertblText.AddCell(cell2)
            headertblText.WriteSelectedRows(0, -1, pageSize.GetLeft(200), pageSize.GetTop(15), writer.DirectContent)

            'Header Image

            Dim headerImage As Image = Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/logo.png"))
            headerImage.ScaleAbsolute(150, 50)

            headerImage.Alignment = Element.ALIGN_CENTER
            Dim headertbl As PdfPTable = New PdfPTable(1)
            headertbl.TotalWidth = 300
            headertbl.HorizontalAlignment = Element.ALIGN_CENTER
            Dim cell3 As PdfPCell = New PdfPCell(headerImage)
            cell3.Border = 0
            headertbl.AddCell(cell3)
            headertbl.WriteSelectedRows(0, -1, pageSize.GetLeft(15), pageSize.GetTop(15), writer.DirectContent)

            ' HEADER CONTAINER

            headertbl = New PdfPTable(1)
            headertbl.TotalWidth = document.PageSize.Width - 30
            headertbl.DefaultCell.Border = Rectangle.BOTTOM_BORDER

            Dim strCustomerName As String = String.Empty
            Dim strDeliveryAddress As String = String.Empty
            Dim strDeliverySuburb As String = String.Empty
            Dim strPhysicalAddress As String = String.Empty
            Dim strPhysicalSuburb As String = String.Empty

            Dim strProjectName As String = _LouvreJobCoverSheetData.cProductionSchedule.CustomerName

            strCustomerName = _LouvreJobCoverSheetData.cCustomer.CustomerName & vbCrLf

            strDeliveryAddress = _LouvreJobCoverSheetData.cDeliveryAddress.Street
            strDeliverySuburb = _LouvreJobCoverSheetData.cDeliveryAddress.Suburb & " " & _LouvreJobCoverSheetData.cDeliveryAddress.State & " " & _LouvreJobCoverSheetData.cDeliveryAddress.Postcode

            strPhysicalAddress = _LouvreJobCoverSheetData.cPhysicalAddress.Street
            strPhysicalSuburb = _LouvreJobCoverSheetData.cPhysicalAddress.Suburb & " " & _LouvreJobCoverSheetData.cPhysicalAddress.State & " " & _LouvreJobCoverSheetData.cPhysicalAddress.Postcode

            Dim strSybizRef As String = _LouvreJobCoverSheetData.cProductionSchedule.OzrollContractNo
            Dim strScheduleRef As String = _LouvreJobCoverSheetData.cProductionSchedule.ShutterProNumber


            ' HEADER TABLE

            Const HEADER_COLUMN_1_WIDTH As Single = 15.0
            Const HEADER_COLUMN_2_WIDTH As Single = 45.0
            Const HEADER_COLUMN_3_WIDTH As Single = 15.0
            Const HEADER_COLUMN_4_WIDTH As Single = 25.0

            Dim pageHeaderData As New PdfPTable({HEADER_COLUMN_1_WIDTH, HEADER_COLUMN_2_WIDTH, HEADER_COLUMN_3_WIDTH, HEADER_COLUMN_4_WIDTH})

            With pageHeaderData
                .WidthPercentage = 100.0F
                .DefaultCell.Border = Rectangle.NO_BORDER

                .SpacingAfter = 2.0F

                ' Row 1
                .AddCell(New PdfPCell(New Phrase("Date:", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(Format(_LouvreJobCoverSheetData.cProductionSchedule.OrderDate, "d MMM yyyy"), baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase("Type:", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(_LouvreJobCoverSheetData.cJobType.Name, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                Dim intCounter As Integer = 0
                Dim strLouvreStyles As String = String.Empty

                If _LouvreJobCoverSheetData.lLouvreDetails IsNot Nothing Then
                    For Each id As SharedEnums.LouvreStyles In _LouvreJobCoverSheetData.lLouvreDetails.UniqueLouvreStyleIDs
                        Dim cLouvreStyle As LouvreStyle = _LouvreJobCoverSheetData.lLouvreStyles.Find(Function(x) x.ID = id)

                        If cLouvreStyle IsNot Nothing Then
                            intCounter += 1

                            If intCounter = 1 Then
                                strLouvreStyles &= cLouvreStyle.Name
                            Else
                                strLouvreStyles &= vbCrLf & cLouvreStyle.Name
                            End If
                        End If
                    Next id
                End If

                ' Row 2
                .AddCell(New PdfPCell(New Phrase("Client Name:", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(strCustomerName, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase("Style", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(strLouvreStyles, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                ' Row 3
                .AddCell(New PdfPCell(New Phrase("Client Address:", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(strPhysicalAddress, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase("Order Type:", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(_LouvreJobCoverSheetData.cOrderType.Description, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                ' Row 4
                .AddCell(New PdfPCell(New Phrase("Client Suburb:", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(strPhysicalSuburb, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase("Expected Ship Date: ", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                If _LouvreJobCoverSheetData.cProductionSchedule.ExpectedShippingDate <> SharedConstants.DEFAULT_DATE_VALUE Then
                    .AddCell(New PdfPCell(New Phrase(Format(_LouvreJobCoverSheetData.cProductionSchedule.ExpectedShippingDate, "d MMM yyyy"), baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                Else
                    .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                End If

                ' Row 5
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                ' Row 6
                .AddCell(New PdfPCell(New Phrase("Project Name:", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(strProjectName, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                ' Row 7
                .AddCell(New PdfPCell(New Phrase("Project Address:", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(strDeliveryAddress, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase("Contract No:", baseFontBold)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(strSybizRef, baseFontBoldRed)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                ' Row 8
                .AddCell(New PdfPCell(New Phrase("Project Suburb:", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(strDeliverySuburb, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase("Contract No:", baseFontBold)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(strScheduleRef, baseFontBoldRed)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                .SpacingAfter = 10.0F
            End With

            headertbl.AddCell(pageHeaderData)

            ' JOB DESCRIPTION
            Const JOB_DESCRIPTION_COLUMN_1_WIDTH As Single = 20.0
            Const JOB_DESCRIPTION_COLUMN_2_WIDTH As Single = 40.0
            Const JOB_DESCRIPTION_COLUMN_3_WIDTH As Single = 20.0
            Const JOB_DESCRIPTION_COLUMN_4_WIDTH As Single = 20.0

            Dim pageJobDescription As New PdfPTable({JOB_DESCRIPTION_COLUMN_1_WIDTH, JOB_DESCRIPTION_COLUMN_2_WIDTH, JOB_DESCRIPTION_COLUMN_3_WIDTH, JOB_DESCRIPTION_COLUMN_4_WIDTH})

            With pageJobDescription
                .WidthPercentage = 100.0F
                .DefaultCell.Border = Rectangle.NO_BORDER

                Dim strColourName As String = String.Empty
                Dim intCounter As Integer = 0

                If _LouvreJobCoverSheetData.lLouvreDetails IsNot Nothing Then
                    For Each id As Integer In _LouvreJobCoverSheetData.lLouvreDetails.UniqueColourIDs
                        Dim cColour As Colour = _LouvreJobCoverSheetData.lColours.Find(Function(x) x.ID = id)

                        If cColour IsNot Nothing Then
                            intCounter += 1

                            If intCounter = 1 Then
                                strColourName &= cColour.Name
                            Else
                                strColourName &= vbCrLf & cColour.Name
                            End If
                        End If
                    Next id
                End If

                .AddCell(New PdfPCell(New Phrase("Colour: ", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .PaddingTop = 10, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(strColourName, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .PaddingTop = 10, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .PaddingTop = 10, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .PaddingTop = 10, .HorizontalAlignment = Element.ALIGN_LEFT})


                ' SQM

                .AddCell(New PdfPCell(New Phrase("SQM: ", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .PaddingTop = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(Math.Ceiling(_LouvreJobCoverSheetData.cProductionSchedule.TotalSQM * 100) / 100, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .PaddingTop = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .PaddingTop = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .PaddingTop = 0, .HorizontalAlignment = Element.ALIGN_LEFT})


                Dim strJobDescription As String = String.Empty

                For Each ld As LouvreDetails In _LouvreJobCoverSheetData.lLouvreDetails
                    strJobDescription &= ld.NoOfPanels & " QTY " & ld.Product & " " & ld.ShutterType & vbCrLf
                Next

                .AddCell(New PdfPCell(New Phrase("Brief Job Description: ", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(strJobDescription, baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase("Goods Total: ", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(FormatCurrency(_LouvreJobCoverSheetData.cProductionSchedule.SalePrice, 2), baseFontBold8)) _
                            With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_RIGHT})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase("Freight: ", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(FormatCurrency(_LouvreJobCoverSheetData.cProductionSchedule.FreightAmount, 2), baseFontBold8)) _
                        With {.BorderWidthBottom = 1, .BorderWidth = 0, .PaddingBottom = 5, .HorizontalAlignment = Element.ALIGN_RIGHT})

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(FormatCurrency((_LouvreJobCoverSheetData.cProductionSchedule.SalePrice + _LouvreJobCoverSheetData.cProductionSchedule.FreightAmount), 2), baseFontBold8)) _
                        With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_RIGHT})

                Dim decGST As Decimal = (_LouvreJobCoverSheetData.cProductionSchedule.SalePrice + _LouvreJobCoverSheetData.cProductionSchedule.FreightAmount) * SharedConstants._DEC_GST_PERCENT

                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase("GST: ", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(FormatCurrency(decGST, 2), baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_RIGHT})

                .AddCell(New PdfPCell(New Phrase(" ", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(String.Empty, baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase("Grand Total: ", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(FormatCurrency((_LouvreJobCoverSheetData.cProductionSchedule.SalePrice + _LouvreJobCoverSheetData.cProductionSchedule.FreightAmount + decGST), 2), baseFontBold8)) _
                        With {.BorderWidthBottom = 2, .BorderWidthTop = 1, .PaddingBottom = 5, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_RIGHT})

                .AddCell(New PdfPCell(New Phrase(" ", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(" ", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(" ", baseFont8)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                .AddCell(New PdfPCell(New Phrase(" ", baseFontBold8)) With {.BorderWidthBottom = 0, .BorderWidthTop = 0, .PaddingBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_RIGHT})

                .SpacingAfter = 0.0F
                .SpacingBefore = 5.0F
            End With

            headertbl.AddCell(pageJobDescription)

            headertbl.WriteSelectedRows(0, -1, pageSize.GetLeft(15), pageSize.GetTop(70), writer.DirectContent)

        End Sub
    End Class

    Public Class LouvreJobCoverSheetData
        Public cProductionSchedule As ProductionSchedule
        Public cCustomer As Customer
        Public cDeliveryAddress As Address
        Public cPhysicalAddress As Address
        Public lLouvreDetails As LouvreDetailsCollection
        Public cLouvreSpecs As LouvreSpecs
        Public cJobType As JobType
        Public lLouvreStyles As List(Of LouvreStyle)
        Public cOrderType As OrderType
        Public lColours As List(Of Colour)
        Public strFileName As String
    End Class

End Class
