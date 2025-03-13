Imports Microsoft.VisualBasic
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class LouvreJobTimeSheet

    Public Shared Sub JobTimeSheet(intScheduleID As Integer)
        Dim response = HttpContext.Current.Response
        Dim objBuffer() As Byte
        Dim output As MemoryStream = New MemoryStream()
        Dim document As Document = New Document(PageSize.A4, 50, 50, 25, 25)
        Dim writer As PdfWriter = PdfWriter.GetInstance(document, output)
        Dim strFileName As String = String.Empty

        document.Open()
        output = ProccessTimeSheets(intScheduleID, output, document, strFileName)
        objBuffer = output.ToArray

        response.Clear()
        response.ContentType = "application/pdf"
        response.AddHeader("Content-Disposition", "attachment;filename=" & strFileName)
        response.BinaryWrite(output.ToArray())
        response.End()

    End Sub

    Shared Function ProccessTimeSheets(intScheduleID As Integer, ByRef output As MemoryStream, document As Document, ByRef strFileNameOut As String) As MemoryStream
        Dim baseFont As Font = New Font(Font.HELVETICA, 10.5F, Font.NORMAL)
        Dim baseFont9 As Font = New Font(Font.HELVETICA, 9.0F, Font.NORMAL)
        Dim baseFont12 As Font = New Font(Font.HELVETICA, 12.0F, Font.NORMAL)

        Dim baseFontBold12 As Font = New Font(Font.HELVETICA, 12.0F, Font.BOLD)
        Dim baseFontBold20 As Font = New Font(Font.HELVETICA, 20.0F, Font.BOLD)
        Dim baseFontBold As Font = New Font(Font.HELVETICA, 10.5F, Font.BOLD)
        Dim baseFontBold9 As Font = New Font(Font.HELVETICA, 9.0F, Font.BOLD)
        Dim baseFontBold7 As Font = New Font(Font.HELVETICA, 7.0F, Font.BOLD)
        Dim baseFontBold11 As Font = New Font(Font.HELVETICA, 15.0F, Font.BOLD)
        Dim baseFontBoldGrey28 As Font = New Font(Font.HELVETICA, 28.0F, Font.BOLD, Color.GRAY)

        Dim baseFontBoldRed As Font = New Font(Font.HELVETICA, 10.5F, Font.BOLD, Color.RED)

        Dim service As New AppService

        strFileNameOut = String.Empty

        document.Open()
        document.SetMargins(2, 2, 2, 2)
        Dim pageTable As PdfPTable
        pageTable = New PdfPTable({25.0F, 75.0F})
        pageTable.WidthPercentage = 100.0F
        Dim date01 As String = Now.Date
        Dim quoteExpiryDate As String = ""
        quoteExpiryDate = date01


        Dim orderTitleText As String = "     Job Time Sheet"
        Dim white As Color = Color.WHITE
        Dim lightGrey As Color = New Color(238, 233, 233)
        Dim currentColour As Color = Color.WHITE
        Dim lineNo As Integer = 1
        Dim strRequiredDate As String

        Dim dateCreated As Date = Date.Now
        strRequiredDate = Year(dateCreated).ToString
        strRequiredDate = If(strRequiredDate.Length = 2, "20" + strRequiredDate, strRequiredDate)
        strRequiredDate = Day(dateCreated).ToString + "/" + Month(dateCreated).ToString + "/" + strRequiredDate

        'Dim strPath As String = System.Web.HttpContext.Current.Server.MapPath("~/images/logo.png")
        'Dim image__1 = Image.GetInstance(strPath)

        'Dim maxHeight As Integer = 20
        'Dim maxWidth As Integer = 25

        'Dim newHeight As Single = 0
        'Dim newWidth As Single = 0

        'Dim aspectRatio As Single = 0

        'Dim fixedHeight As Single = maxHeight

        'If image__1.Width > maxWidth Then
        '    aspectRatio = maxWidth / Math.Max(image__1.Width, 1)
        '    newWidth = maxWidth
        '    newHeight = image__1.Height * aspectRatio
        'End If


        'If image__1.Height > maxHeight Then
        '    aspectRatio = maxHeight / Math.Max(image__1.Height, 1)
        '    newHeight = maxHeight
        '    newWidth = image__1.Width * aspectRatio
        'End If

        'If newHeight < maxHeight AndAlso CInt(newHeight) <> 0 Then
        '    fixedHeight = newHeight
        'ElseIf image__1.Height < maxHeight Then
        '    fixedHeight = image__1.Height
        'End If

        'image__1.ScaleAbsolute(newWidth, newHeight)
        'Dim cell As New PdfPCell(image__1, False) With {.BorderWidth = 0}

        'pageTable.AddCell(cell)

        Dim image As Image = Image.GetInstance(HttpContext.Current.Server.MapPath(".") + "/images/logo.png")
        image.ScaleAbsolute(500, 300)
        pageTable.AddCell(image)

        pageTable.AddCell(New PdfPCell(New Phrase(orderTitleText + (vbCrLf), baseFontBoldGrey28)) With {.BorderWidth = 0, .Colspan = 2, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(vbCrLf, baseFontBoldGrey28)) With {.Colspan = 2, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

        document.Add(pageTable)

        Dim cProductionSchedule As ProductionSchedule = service.getProdScheduleClsByID(intScheduleID)

        strFileNameOut = "LouvreJobTimeSheet_" & cProductionSchedule.ShutterProNumber & "_" & Format(cProductionSchedule.EnteredDatetime, "yyyyMMdd") & ".pdf"

        Dim strCustomerName As String = String.Empty
        Dim strCustomerAddress As String = String.Empty
        Dim strCustomerSuburb As String = String.Empty

        Dim strProjectName As String = cProductionSchedule.CustomerName
        Dim strProjectAddress As String = String.Empty
        Dim strProjectSuburb As String = String.Empty

        Dim dtCustomer As DataTable = service.runSQLScheduling("select * from dbo.tblCustomers where CustomeriD = " & cProductionSchedule.CustomerID)

        If dtCustomer.Rows.Count > 0 Then
            strCustomerName = dtCustomer.Rows(0)("CustomerName").ToString & vbCrLf
        End If

        dtCustomer.Dispose()
        dtCustomer = Nothing

        Dim dtDeliveryAddress As Address = service.getAddressByID(cProductionSchedule.DeliveryAddressID)

        strProjectAddress = dtDeliveryAddress.Street
        strProjectSuburb = dtDeliveryAddress.Suburb & " " & dtDeliveryAddress.State & " " & dtDeliveryAddress.Postcode

        Dim dtPhysicalAddresses As List(Of Address) = service.getAddressesByCustomerIDAndAddressType(cProductionSchedule.CustomerID, SharedEnums.AddressType.Physical)
        Dim cPhysicalAddress As Address = dtPhysicalAddresses.Find(Function(x) x.IsPrimary AndAlso Not x.Discontinued)

        If cPhysicalAddress IsNot Nothing Then
            strCustomerAddress = cPhysicalAddress.Street
            strCustomerSuburb = cPhysicalAddress.Suburb & " " & cPhysicalAddress.State & " " & cPhysicalAddress.Postcode
        End If

        Dim strSybizRef As String = cProductionSchedule.OzrollContractNo
        Dim strScheduleRef As String = cProductionSchedule.ShutterProNumber

        pageTable = New PdfPTable({25.0F, 25.0F, 25.0F, 25.0F})
        pageTable.WidthPercentage = 100.0F
        pageTable.AddCell(New PdfPCell(New Phrase("Contract No:", baseFontBold12)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(strSybizRef, baseFontBold12)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase("Date Required:", baseFontBold12)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_RIGHT})

        If cProductionSchedule.ExpectedShippingDate <> SharedConstants.DEFAULT_DATE_VALUE Then
            pageTable.AddCell(New PdfPCell(New Phrase(Format(cProductionSchedule.ExpectedShippingDate, "d MMM yyyy"), baseFontBold12)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        Else
            pageTable.AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold12)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        End If

        pageTable.AddCell(New PdfPCell(New Phrase("Contract No:", baseFontBold12)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(strScheduleRef, baseFontBold12)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold12)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_RIGHT})
        pageTable.AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold12)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase(vbCrLf, baseFont)) With {.Colspan = 4, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

        pageTable.AddCell(New PdfPCell(New Phrase("Name:", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(strCustomerName, baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase("Project Name:", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(strProjectName, baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase("Address:", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(strCustomerAddress, baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase("Project Address:", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(strProjectAddress, baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase("Suburb:", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(strCustomerSuburb, baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase("Project Suburb:", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(strProjectSuburb, baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase(vbCrLf, baseFont)) With {.Colspan = 4, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
        document.Add(pageTable)

        Dim DetailSection As PdfPTable = New PdfPTable({17.0F, 17.0F, 8.5F, 8.5F, 15.0F, 8.5F, 8.5F, 17.0F})

        DetailSection.WidthPercentage = 100.0F
        DetailSection.AddCell(New PdfPCell(New Phrase("Target MIN", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        DetailSection.AddCell(New PdfPCell(New Phrase("CL", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

        'If cProductionSchedule.TotalSQM <> SharedConstants.DEFAULT_DECIMAL_VALUE Then
        '    DetailSection.AddCell(New PdfPCell(New Phrase(Format(CDec(cProductionSchedule.TotalSQM * 40.85), "#.00"), baseFontBoldRed)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER}) '=[sqm]*40.85
        '    DetailSection.AddCell(New PdfPCell(New Phrase(Format(CDec(cProductionSchedule.TotalSQM * 54.28), "#.00"), baseFontBoldRed)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER}) '=[sqm]*54.28
        '    DetailSection.AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        '    DetailSection.AddCell(New PdfPCell(New Phrase(Format(CDec(cProductionSchedule.TotalSQM * 40.1), "#.00"), baseFontBoldRed)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER}) '=[sqm]*40.1
        '    DetailSection.AddCell(New PdfPCell(New Phrase(Format(CDec(cProductionSchedule.TotalSQM * 31.25), "#.00"), baseFontBoldRed)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER}) '=[sqm]*31.25        
        'Else
        DetailSection.AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold)) With {.Colspan = 5, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        'End If

        If cProductionSchedule.TotalSQM <> SharedConstants.DEFAULT_DECIMAL_VALUE Then
            DetailSection.AddCell(New PdfPCell(New Phrase(Math.Ceiling(cProductionSchedule.TotalSQM * 100) / 100, baseFontBold12)) With {.Rowspan = 2, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        Else
            DetailSection.AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold12)) With {.Rowspan = 2, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        End If

        DetailSection.AddCell(New PdfPCell(New Phrase("Target MIN:", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        DetailSection.AddCell(New PdfPCell(New Phrase("DL/DLi", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

        'If cProductionSchedule.TotalSQM <> SharedConstants.DEFAULT_DECIMAL_VALUE Then
        '    DetailSection.AddCell(New PdfPCell(New Phrase(Format(CDec(cProductionSchedule.TotalSQM * 38), "#.00"), baseFontBoldRed)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER}) '=[sqm]*38
        '    DetailSection.AddCell(New PdfPCell(New Phrase(Format(CDec(cProductionSchedule.TotalSQM * 43.51), "#.00"), baseFontBoldRed)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER}) '=[sqm]*43.51
        'Else
        DetailSection.AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold)) With {.Colspan = 2, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        'End If

        DetailSection.AddCell(New PdfPCell(New Phrase("BLADES", baseFontBold)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

        'If cProductionSchedule.TotalSQM <> SharedConstants.DEFAULT_DECIMAL_VALUE Then
        '    DetailSection.AddCell(New PdfPCell(New Phrase(Format(CDec(cProductionSchedule.TotalSQM * 64.51), "#.00"), baseFontBoldRed)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER}) '=[sqm]*64.51
        '    DetailSection.AddCell(New PdfPCell(New Phrase(Format(CDec(cProductionSchedule.TotalSQM * 31.25), "#.00"), baseFontBoldRed)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER}) '=[sqm]*31.25
        'Else
        DetailSection.AddCell(New PdfPCell(New Phrase(String.Empty, baseFontBold)) With {.Colspan = 2, .BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        'End If

        document.Add(DetailSection)

        DetailSection = New PdfPTable({14.0F, 16.0F, 8.5F, 8.5F, 9.5F, 7.5F, 7.0F, 10.0F, 5.0F, 12.0F, 5.0F})
        DetailSection.WidthPercentage = 100.0F
        DetailSection.AddCell(New PdfPCell(New Phrase("DATE", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        DetailSection.AddCell(New PdfPCell(New Phrase("NAME", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        DetailSection.AddCell(New PdfPCell(New Phrase("CUT", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        DetailSection.AddCell(New PdfPCell(New Phrase("CNC", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        DetailSection.AddCell(New PdfPCell(New Phrase("PINNED", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        DetailSection.AddCell(New PdfPCell(New Phrase("QTY", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        DetailSection.AddCell(New PdfPCell(New Phrase("PREP", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        DetailSection.AddCell(New PdfPCell(New Phrase("ASSEM", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        DetailSection.AddCell(New PdfPCell(New Phrase("QC", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        DetailSection.AddCell(New PdfPCell(New Phrase("REMAKE", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        DetailSection.AddCell(New PdfPCell(New Phrase("NO", baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

        For j As Integer = 0 To 25 - 1

            If currentColour.B = white.B And currentColour.G = white.G And currentColour.R = white.R Then
                currentColour = lightGrey
            Else
                currentColour = white
            End If

            DetailSection.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = currentColour, .Padding = 5})
            DetailSection.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = currentColour, .Padding = 5})
            DetailSection.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = currentColour, .Padding = 5})
            DetailSection.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = currentColour, .Padding = 5})
            DetailSection.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = currentColour, .Padding = 5})
            DetailSection.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = currentColour, .Padding = 5})
            DetailSection.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = currentColour, .Padding = 5})
            DetailSection.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = currentColour, .Padding = 5})
            DetailSection.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = currentColour, .Padding = 5})
            DetailSection.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = currentColour, .Padding = 5})
            DetailSection.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = currentColour, .Padding = 5})
            lineNo += 1
        Next

        document.Add(New Phrase(vbCrLf))
        document.Add(DetailSection)
        document.Close()

        service = Nothing

        Return output

    End Function

End Class
