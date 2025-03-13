Imports Microsoft.VisualBasic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO

Public Class LouvreJobRunningSheet

    Public Shared Sub GetJobRunningSheetPDF(intScheduleID As Integer)
        Dim Response = HttpContext.Current.Response

        Dim objBuffer() As Byte
        Dim output As MemoryStream = New MemoryStream()

        Dim document As Document = New Document(PageSize.A4, 50, 50, 25, 25)
        Dim writer As PdfWriter = PdfWriter.GetInstance(document, output)
        Dim strFileName As String = String.Empty

        document.Open()
        output = LouvreJobRunningSheet.CreateJobRunningSheet(intScheduleID, output, document, strFileName)

        objBuffer = output.ToArray

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("Content-Disposition", "attachment;filename=" & strFileName)
        Response.BinaryWrite(output.ToArray())
        Response.End()

    End Sub

    Public Shared Function GetJobRunningSheetMemoryStream(intScheduleID As Integer) As Byte()
        Dim Response = HttpContext.Current.Response

        Dim output As MemoryStream = New MemoryStream()
        Dim objBuffer As Byte()
        Dim document As Document = New Document(PageSize.A4, 50, 50, 25, 25)
        Dim writer As PdfWriter = PdfWriter.GetInstance(document, output)
        Dim strFileName As String = String.Empty

        document.Open()
        output = LouvreJobRunningSheet.CreateJobRunningSheet(intScheduleID, output, document, strFileName)

        objBuffer = output.ToArray


        Return objBuffer

    End Function

    Public Shared Function CreateJobRunningSheet(intScheduleID As Integer, ByRef output As MemoryStream, document As Document, ByRef strFileNameOut As String) As MemoryStream

        Dim BaseFont As Font = New Font(Font.HELVETICA, 10.5F, Font.NORMAL)
        Dim baseFont12 As Font = New Font(Font.HELVETICA, 12.0F, Font.NORMAL)
        Dim baseFont14 As Font = New Font(Font.HELVETICA, 14.0F, Font.NORMAL)

        Dim baseFontBold12 As Font = New Font(Font.HELVETICA, 12.0F, Font.BOLD)
        Dim baseFontBold As Font = New Font(Font.HELVETICA, 10.5F, Font.BOLD)
        Dim baseFontBoldGrey28 As Font = New Font(Font.HELVETICA, 28.0F, Font.BOLD, Color.GRAY)
        Dim baseFontBold14 As Font = New Font(Font.HELVETICA, 14.0F, Font.BOLD)
        Dim baseFontBoldRed14 As Font = New Font(Font.HELVETICA, 14.0F, Font.BOLD, Color.RED)

        strFileNameOut = String.Empty

        document.SetMargins(2, 2, 2, 2)

        Dim pageTop As New PdfPTable({25.0F, 50.0F, 25.0F})
        pageTop.WidthPercentage = 100.0F
        pageTop.DefaultCell.Border = Rectangle.NO_BORDER


        Dim image As Image = Image.GetInstance(HttpContext.Current.Server.MapPath(".") + "/images/logo.png")
        image.ScaleAbsolute(500, 300)
        pageTop.AddCell(image)

        pageTop.AddCell(New PdfPCell(New Phrase("Job Running Sheet", baseFontBoldGrey28)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER, .VerticalAlignment = Element.ALIGN_TOP})
        pageTop.AddCell(New PdfPCell(New Phrase("", baseFontBold12)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_RIGHT})

        document.Add(pageTop)

        Dim service As New AppService

        Dim cProductionSchedule As ProductionSchedule = service.getProdScheduleClsByID(intScheduleID)

        strFileNameOut = "LouvreJobRunningSheet_" & cProductionSchedule.ShutterProNumber & "_" & Format(cProductionSchedule.EnteredDatetime, "yyyyMMdd") & ".pdf"

        Dim strCustomerName As String = String.Empty
        Dim strCustomerAddress As String = String.Empty
        Dim strCustomerSuburb As String = String.Empty       
	
       'Ticket#62478
       'Dim strProjectName As String = cProductionSchedule.CustomerName
        Dim strProjectName As String = cProductionSchedule.OrderReference
        Dim strProjectAddress As String = String.Empty
        Dim strProjectSuburb As String = String.Empty

        Dim dtCustomer As DataTable = service.runSQLScheduling("select * from dbo.tblCustomers where CustomeriD = " & cProductionSchedule.CustomerID)

        If dtCustomer.Rows.Count > 0 Then
            strCustomerName = dtCustomer.Rows(0)("CustomerName").ToString & vbCrLf
        End If
        dtCustomer.Dispose()
        dtCustomer = Nothing

        Dim dtDeliveryAddress As Address = service.getAddressByID(cProductionSchedule.DeliveryAddressID)

        strCustomerAddress = dtDeliveryAddress.Street
        strCustomerSuburb = dtDeliveryAddress.Suburb & " " & dtDeliveryAddress.State & " " & dtDeliveryAddress.Postcode

        'Main Content
        Dim pageTableMain As New PdfPTable({48.0F, 4.0F, 48.0F})
        pageTableMain.WidthPercentage = 100.0F
        pageTableMain.DefaultCell.Border = Rectangle.NO_BORDER

        pageTableMain.SpacingBefore = 12.5F
        pageTableMain.SpacingAfter = 12.5F

        'Main Left
        Dim pageTableMainLeft As New PdfPTable({48.0F})
        pageTableMainLeft.WidthPercentage = 100.0F
        pageTableMainLeft.DefaultCell.Border = Rectangle.NO_BORDER

        pageTableMainLeft.SpacingBefore = 12.5F
        pageTableMainLeft.SpacingAfter = 12.5F

        'Main Content - Left Top
        Dim pageTableJobRunningSheetLeft As New PdfPTable({18.0F, 22.0F})
        pageTableJobRunningSheetLeft.WidthPercentage = 100.0F
        pageTableJobRunningSheetLeft.DefaultCell.Border = Rectangle.NO_BORDER

        pageTableJobRunningSheetLeft.AddCell(New PdfPCell(New Phrase("Order No: ", baseFontBold)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTableJobRunningSheetLeft.AddCell(New PdfPCell(New Phrase(cProductionSchedule.ShutterProNumber, BaseFont)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        'Main Content - Left Bottom
        Dim pageTableJobRunningSheetLeftBottom As New PdfPTable({18.0F, 22.0F})
        pageTableJobRunningSheetLeftBottom.WidthPercentage = 100.0F
        pageTableJobRunningSheetLeftBottom.DefaultCell.Border = Rectangle.NO_BORDER

        pageTableJobRunningSheetLeftBottom.AddCell(New PdfPCell(New Phrase("Client Name: ", baseFontBold)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTableJobRunningSheetLeftBottom.AddCell(New PdfPCell(New Phrase(strCustomerName, BaseFont)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTableJobRunningSheetLeftBottom.AddCell(New PdfPCell(New Phrase("Client Address: ", baseFontBold)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTableJobRunningSheetLeftBottom.AddCell(New PdfPCell(New Phrase(strCustomerAddress, BaseFont)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTableJobRunningSheetLeftBottom.AddCell(New PdfPCell(New Phrase("Client Suburb: ", baseFontBold)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTableJobRunningSheetLeftBottom.AddCell(New PdfPCell(New Phrase(strCustomerSuburb, BaseFont)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTableMainLeft.AddCell(pageTableJobRunningSheetLeft)
        pageTableMainLeft.AddCell(pageTableJobRunningSheetLeftBottom)

        'Center
        Dim pageTableMainCenter As New PdfPTable({4.0F})
        pageTableMainCenter.WidthPercentage = 4.0F
        pageTableMainCenter.DefaultCell.Border = Rectangle.NO_BORDER

        pageTableMainCenter.SpacingBefore = 12.5F
        pageTableMainCenter.SpacingAfter = 12.5F

        Dim pageTableJobRunningSheetCenter As New PdfPTable({4.0F})
        pageTableJobRunningSheetCenter.WidthPercentage = 4.0F
        pageTableJobRunningSheetCenter.DefaultCell.Border = Rectangle.NO_BORDER

        pageTableJobRunningSheetCenter.AddCell(New PdfPCell(New Phrase("", BaseFont)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTableMainCenter.AddCell(pageTableJobRunningSheetCenter)

        'Right
        Dim pageTableMainRight As New PdfPTable({48.0F})
        pageTableMainRight.WidthPercentage = 48.0F
        pageTableMainRight.DefaultCell.Border = Rectangle.NO_BORDER

        pageTableMainRight.SpacingBefore = 12.5F
        pageTableMainRight.SpacingAfter = 12.5F

        Dim pageTableJobRunningSheetRight As New PdfPTable({20.0F, 28.0F})
        pageTableJobRunningSheetRight.WidthPercentage = 48.0F
        pageTableJobRunningSheetRight.DefaultCell.Border = Rectangle.NO_BORDER

        pageTableJobRunningSheetRight.AddCell(New PdfPCell(New Phrase("Date Required:", baseFontBold)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        If cProductionSchedule.ExpectedShippingDate <> SharedConstants.DEFAULT_DATE_VALUE Then
            pageTableJobRunningSheetRight.AddCell(New PdfPCell(New Phrase(Format(cProductionSchedule.ExpectedShippingDate, "d MMM yyyy"), BaseFont)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        Else
            pageTableJobRunningSheetRight.AddCell(New PdfPCell(New Phrase(String.Empty, BaseFont)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        End If


        'Main Content - Right Bottom
        Dim pageTableJobRunningSheetRightBottom As New PdfPTable({18.0F, 22.0F})
        pageTableJobRunningSheetRightBottom.WidthPercentage = 100.0F
        pageTableJobRunningSheetRightBottom.DefaultCell.Border = Rectangle.NO_BORDER

        pageTableJobRunningSheetRightBottom.AddCell(New PdfPCell(New Phrase("Project Name: ", baseFontBold)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTableJobRunningSheetRightBottom.AddCell(New PdfPCell(New Phrase(strProjectName, BaseFont)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTableJobRunningSheetRightBottom.AddCell(New PdfPCell(New Phrase("Project Address: ", baseFontBold)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTableJobRunningSheetRightBottom.AddCell(New PdfPCell(New Phrase(strProjectAddress, BaseFont)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTableJobRunningSheetRightBottom.AddCell(New PdfPCell(New Phrase("Project Suburb: ", baseFontBold)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTableJobRunningSheetRightBottom.AddCell(New PdfPCell(New Phrase(strProjectSuburb, BaseFont)) With {.BorderWidthBottom = 0, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTableMainRight.AddCell(pageTableJobRunningSheetRight)
        pageTableMainRight.AddCell(pageTableJobRunningSheetRightBottom)

        pageTableMain.AddCell(pageTableMainLeft)
        pageTableMain.AddCell(pageTableMainCenter)
        pageTableMain.AddCell(pageTableMainRight)

        document.Add(pageTableMain)

        'Quantity, SQM, Colour
        Dim pageQuantitySQMColour As New PdfPTable({8.0F, 8.0F, 3.0F, 5.0F, 5.0F, 3.0F, 8.0F, 15.0F})
        pageQuantitySQMColour.WidthPercentage = 100.0F
        pageQuantitySQMColour.DefaultCell.Border = Rectangle.NO_BORDER

        pageQuantitySQMColour.SpacingBefore = 5.0F
        pageQuantitySQMColour.SpacingAfter = 5.0F


        Dim strQuantity As String = cProductionSchedule.TotalPanels.ToString
        Dim strColour As String = String.Empty
        Dim strJobDescription As String = String.Empty
        Dim lColours As List(Of Colour) = service.getColours()
        Dim intCounter As Integer = 0

        Dim lLouvreDetails As LouvreDetailsCollection = service.getLouvreDetailsCollectionByProductionScheduleID(intScheduleID)

        If lLouvreDetails IsNot Nothing Then
            For Each id As Integer In lLouvreDetails.UniqueColourIDs
                Dim cColour As Colour = lColours.Find(Function(x) x.ID = id)

                If cColour IsNot Nothing Then
                    intCounter += 1

                    If intCounter = 1 Then
                        strColour &= cColour.Name
                    Else
                        strColour &= vbCrLf & cColour.Name
                    End If
                End If
            Next id

            For Each l As LouvreDetails In lLouvreDetails
                strJobDescription &= l.NoOfPanels & " " & l.Product & " " & l.ShutterType & vbCrLf
            Next l
        End If

        pageQuantitySQMColour.AddCell(New PdfPCell(New Phrase("Quantity: ", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageQuantitySQMColour.AddCell(New PdfPCell(New Phrase(strQuantity, BaseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

        pageQuantitySQMColour.AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageQuantitySQMColour.AddCell(New PdfPCell(New Phrase("SQM: ", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageQuantitySQMColour.AddCell(New PdfPCell(New Phrase(Math.Ceiling(cProductionSchedule.TotalSQM * 100) / 100, BaseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

        pageQuantitySQMColour.AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageQuantitySQMColour.AddCell(New PdfPCell(New Phrase("Colour: ", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageQuantitySQMColour.AddCell(New PdfPCell(New Phrase(strColour, BaseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

        document.Add(pageQuantitySQMColour)

        'Job Description
        Dim pageJobDescription As New PdfPTable({20.0F, 80.0F})
        pageJobDescription.WidthPercentage = 100.0F
        pageJobDescription.DefaultCell.Border = Rectangle.NO_BORDER

        pageJobDescription.SpacingBefore = 5.0F
        pageJobDescription.SpacingAfter = 5.0F

        pageJobDescription.AddCell(New PdfPCell(New Phrase("Job Description: ", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageJobDescription.AddCell(New PdfPCell(New Phrase(strJobDescription, BaseFont)) With {.BorderWidth = 1, .PaddingLeft = 5, .HorizontalAlignment = Element.ALIGN_LEFT})

        document.Add(pageJobDescription)

        'Main Detail Section Top Half
        Dim pageMainContent As New PdfPTable({60.0F, 40.0F})
        pageMainContent.WidthPercentage = 100.0F
        pageMainContent.DefaultCell.Border = Rectangle.NO_BORDER

        pageMainContent.SpacingBefore = 5.0F
        pageMainContent.SpacingAfter = 5.0F

        pageMainContent.AddCell(New PdfPCell(New Phrase("Paperwork Recieved In Production:", baseFontBold14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageMainContent.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageMainContent.AddCell(New PdfPCell(New Phrase("Material Ordered:", baseFontBold14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageMainContent.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageMainContent.AddCell(New PdfPCell(New Phrase("Delivered To Powder Coating:", baseFontBold14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageMainContent.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageMainContent.AddCell(New PdfPCell(New Phrase("Powder Coating ETA:", baseFontBold14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageMainContent.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageMainContent.AddCell(New PdfPCell(New Phrase("Received Back From Powder Coating:", baseFontBold14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageMainContent.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageMainContent.AddCell(New PdfPCell(New Phrase("ProductionSheets Given To Floor:", baseFontBold14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageMainContent.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

        document.Add(pageMainContent)

        'Main Detail Section Center
        Dim pageMainContentCenter As New PdfPTable({60.0F, 40.0F})
        pageMainContentCenter.WidthPercentage = 100.0F
        pageMainContentCenter.DefaultCell.Border = Rectangle.NO_BORDER

        pageMainContentCenter.SpacingBefore = 5.0F
        pageMainContentCenter.SpacingAfter = 5.0F

        pageMainContentCenter.AddCell(New PdfPCell(New Phrase("Saw Calibration - Print Name", baseFontBoldRed14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT, .BorderColor = Color.RED, .BackgroundColor = Color.YELLOW})
        pageMainContentCenter.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

        document.Add(pageMainContentCenter)

        'Main Detail Section Bottom Half
        Dim pageMainContentBottom As New PdfPTable({60.0F, 40.0F})
        pageMainContentBottom.WidthPercentage = 100.0F
        pageMainContentBottom.DefaultCell.Border = Rectangle.NO_BORDER

        pageMainContentBottom.SpacingBefore = 5.0F
        pageMainContentBottom.SpacingAfter = 5.0F

        pageMainContentBottom.AddCell(New PdfPCell(New Phrase("Cutting Done:", baseFontBold14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageMainContentBottom.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageMainContentBottom.AddCell(New PdfPCell(New Phrase("Prep Done:", baseFontBold14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageMainContentBottom.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageMainContentBottom.AddCell(New PdfPCell(New Phrase("Assembly Done:", baseFontBold14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageMainContentBottom.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageMainContentBottom.AddCell(New PdfPCell(New Phrase("QC Done:", baseFontBold14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageMainContentBottom.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageMainContentBottom.AddCell(New PdfPCell(New Phrase("No. of Reworks/Repairs To Be Done:", baseFontBold14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageMainContentBottom.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageMainContentBottom.AddCell(New PdfPCell(New Phrase("Reworks/Repairs Done:", baseFontBold14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageMainContentBottom.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageMainContentBottom.AddCell(New PdfPCell(New Phrase("Handed To Despatch:", baseFontBold14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageMainContentBottom.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont14)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

        document.Add(pageMainContentBottom)

        ' Job Notes

        Dim lNotes As List(Of ProdScheduleNote) = service.GetProdScheduleNotesByProductionScheduleID(cProductionSchedule.ID)

        Const NOTES_COLUMN_1_WIDTH As Single = 15.0
        Const NOTES_COLUMN_2_WIDTH As Single = 85.0

        Dim pdfTableNotes As New PdfPTable({NOTES_COLUMN_1_WIDTH, NOTES_COLUMN_2_WIDTH})

        With pdfTableNotes
            .WidthPercentage = 100.0F
            .SpacingBefore = 5.0F
            .SpacingAfter = 5.0F

            .AddCell(New PdfPCell(New Phrase("Job Notes:", baseFontBold12)) With {.BorderWidth = 0, .PaddingLeft = 4, .HorizontalAlignment = Element.ALIGN_LEFT})

            If lNotes.Count = 0 Then
                .AddCell(New PdfPCell(New Phrase(" ", BaseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            End If

            Dim i As Integer = 0

            For Each n As ProdScheduleNote In lNotes

                i += 1

                If i > 1 Then
                    .AddCell(New PdfPCell(New Phrase(" ", BaseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                End If

                .AddCell(New PdfPCell(New Phrase(n.NoteDetails, BaseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            Next n
        End With

        document.Add(pdfTableNotes)

        service = Nothing

        document.Close()

        Return output

    End Function

End Class
