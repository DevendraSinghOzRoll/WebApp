Imports Microsoft.VisualBasic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO

Public Class LouvreJobDeliveryDocket

    Public Shared Sub GetLouvreJobDeliveryDocket(intScheduleID As Integer)
        Dim Response = HttpContext.Current.Response

        Dim objBuffer() As Byte
        Dim output As MemoryStream = New MemoryStream()

        Dim document As Document = New Document(PageSize.A4, 50, 50, 25, 25)
        Dim writer As PdfWriter = PdfWriter.GetInstance(document, output)
        Dim strFileName As String = String.Empty

        document.Open()
        LouvreJobDeliveryDocket.generateLouvreJobDeliveryDocket(intScheduleID, output, document, strFileName)

        objBuffer = output.ToArray

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("Content-Disposition", "attachment;filename=" & strFileName)
        Response.BinaryWrite(output.ToArray())
        Response.End()

    End Sub

    Public Shared Function GetLouvreJobDeliveryDocketMemoryStream(intScheduleID As Integer) As Byte()
        Dim Response = HttpContext.Current.Response
        Dim objBuffer() As Byte
        Dim output As MemoryStream = New MemoryStream()

        Dim document As Document = New Document(PageSize.A4, 50, 50, 25, 25)
        Dim writer As PdfWriter = PdfWriter.GetInstance(document, output)
        Dim strFileName As String = String.Empty

        document.Open()
        LouvreJobDeliveryDocket.generateLouvreJobDeliveryDocket(intScheduleID, output, document, strFileName)

        objBuffer = output.ToArray

        Return objBuffer

    End Function


    Public Shared Function generateLouvreJobDeliveryDocket(intScheduleID As Integer, ByRef output As MemoryStream, document As Document, ByRef strFileNameOut As String) As Boolean
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

        strFileNameOut = String.Empty

        Dim service As New AppService

        Dim dt As DataTable = New DataTable

        Dim strOzrollAddress As String = String.Empty
        Dim strOzrollPhone As String = String.Empty

        Dim pageTable As PdfPTable
        pageTable = New PdfPTable({35.0F, 35.0F, 15.0F, 15.0F})
        pageTable.WidthPercentage = 100.0F
        pageTable.AddCell(New PdfPCell(New Phrase(strOzrollAddress, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_TOP})

        Dim image As Image = Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/logo.png"))
        image.ScaleAbsolute(500, 300)
        pageTable.AddCell(image)

        pageTable.AddCell(New PdfPCell(New Phrase(strOzrollPhone, baseFontBold)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        document.Add(pageTable)
        pageTable = Nothing

        Dim strClientAddress As String = String.Empty
        Dim strDeliverAddress As String = String.Empty

        Dim cProductionSchedule As ProductionSchedule = service.getProdScheduleClsByID(intScheduleID)

        strFileNameOut = "LouvreJobDeliveryDocket_" & cProductionSchedule.ShutterProNumber & "_" & Format(cProductionSchedule.EnteredDatetime, "yyyyMMdd") & ".pdf"

        Dim customer As Customer = service.GetCustomerByID(cProductionSchedule.CustomerID)

        Dim dtDeliveryAddress As Address = service.getAddressByID(cProductionSchedule.DeliveryAddressID)
        Dim dtPhysicalAddresses As List(Of Address) = service.getAddressesByCustomerIDAndAddressType(cProductionSchedule.CustomerID, SharedEnums.AddressType.Physical)
        Dim cPhysicalAddress As Address = dtPhysicalAddresses.Find(Function(x) x.IsPrimary AndAlso Not x.Discontinued)

        If dtDeliveryAddress IsNot Nothing Then
            strDeliverAddress = dtDeliveryAddress.Street & " " & dtDeliveryAddress.Suburb & " " & dtDeliveryAddress.State & " " & dtDeliveryAddress.Postcode
        End If

        Dim lDeliveryNotes As List(Of DeliveryInstruction) = service.getDeliveryInstructionsListByAddressID(dtDeliveryAddress.ID)

        If cPhysicalAddress IsNot Nothing Then
            strClientAddress = customer.CustomerName & ", " & cPhysicalAddress.Street & " " & cPhysicalAddress.Suburb & " " & cPhysicalAddress.State & " " & cPhysicalAddress.Postcode
        End If

        Dim strScheduleRef As String = cProductionSchedule.ShutterProNumber

        pageTable = New PdfPTable({15.0F, 40.0F, 20.0F, 20.0F})
        pageTable.WidthPercentage = 100.0F
        pageTable.SpacingBefore = 5.0F
        pageTable.SpacingAfter = 5.0F

        pageTable.AddCell(New PdfPCell(New Phrase("DELIVERY DOCKET", baseFontBoldGrey28)) With {.Colspan = 4, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageTable.AddCell(New PdfPCell(New Phrase(vbCrLf)) With {.Colspan = 4, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

        pageTable.AddCell(New PdfPCell(New Phrase("SOLD TO:", baseFontBold)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_TOP})
        pageTable.AddCell(New PdfPCell(New Phrase(strClientAddress, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase("Customer Name:", baseFontBold)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(cProductionSchedule.CustomerName, baseFontBold)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_TOP})
        pageTable.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase("Customer P/O:", baseFontBold)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(cProductionSchedule.OrderReference, baseFontBold)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase("DELIVER TO:", baseFontBold)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT, .VerticalAlignment = Element.ALIGN_TOP})
        pageTable.AddCell(New PdfPCell(New Phrase(strDeliverAddress, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase("Ozroll Ref:", baseFontBold)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(strScheduleRef, baseFontBold)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        document.Add(pageTable)
        pageTable = Nothing

        ' Job Notes

        Dim lNotes As List(Of ProdScheduleNote) = service.GetProdScheduleNotesByProductionScheduleID(cProductionSchedule.ID)

        Const NOTES_COLUMN_1_WIDTH As Single = 15.0
        Const NOTES_COLUMN_2_WIDTH As Single = 85.0

        pageTable = New PdfPTable({NOTES_COLUMN_1_WIDTH, NOTES_COLUMN_2_WIDTH})

        With pageTable
            .WidthPercentage = 100.0F
            .SpacingBefore = 5.0F
            .SpacingAfter = 5.0F

            .AddCell(New PdfPCell(New Phrase("Job Notes:", baseFontBold12)) With {.BorderWidth = 0, .PaddingLeft = 4, .HorizontalAlignment = Element.ALIGN_LEFT})

            If lNotes.Count = 0 Then
                .AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            End If

            Dim i As Integer = 0

            For Each n As ProdScheduleNote In lNotes

                i += 1

                If i > 1 Then
                    .AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
                End If

                .AddCell(New PdfPCell(New Phrase(n.NoteDetails, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            Next n
        End With

        document.Add(pageTable)
        pageTable = Nothing

        Dim strQuantity As String = cProductionSchedule.TotalPanels.ToString
        Dim strSQM As String = cProductionSchedule.TotalSQM.ToString
        Dim strColour As String = String.Empty

        Dim lLouvreDetails As LouvreDetailsCollection = service.getLouvreDetailsCollectionByProductionScheduleID(intScheduleID)

        If lLouvreDetails IsNot Nothing Then
            Dim lColours As List(Of Colour) = service.getColours()
            Dim intCounter As Integer = 0

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
        End If

        pageTable = New PdfPTable({15.0F, 10.0F, 10.0F, 10.0F, 15.0F, 40.0F})
        pageTable.WidthPercentage = 100.0F
        pageTable.SpacingBefore = 5.0F
        pageTable.SpacingAfter = 5.0F

        pageTable.AddCell(New PdfPCell(New Phrase("LOUVRES AND PACK-UP AS PER ATTACHED PRODUCTION SHEETS", baseFontBold12)) With {.Colspan = 6, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageTable.AddCell(New PdfPCell(New Phrase(vbCrLf)) With {.Colspan = 6, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

        pageTable.AddCell(New PdfPCell(New Phrase("Quantity:", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(strQuantity, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase("SQM:", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(Math.Ceiling(strSQM * 100) / 100, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase("Colour:", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(strColour, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase(vbCrLf)) With {.Colspan = 6, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_CENTER})

        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.Colspan = 2, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.Colspan = 4, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        document.Add(pageTable)
        pageTable = Nothing

        'Delivery notes

        If lDeliveryNotes.Count > 0 Then

            pageTable = New PdfPTable({100.0F})
            pageTable.WidthPercentage = 100.0F
            pageTable.SpacingBefore = 5.0F
            pageTable.SpacingAfter = 5.0F

            pageTable.AddCell(New PdfPCell(New Phrase("DELIVERY NOTES:", baseFontBold12)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

            Dim strDeliveryNote As String = String.Empty

            For Each n As DeliveryInstruction In lDeliveryNotes
                pageTable.AddCell(New PdfPCell(New Phrase(n.InstructionText, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            Next

            'pageTable.AddCell(New PdfPCell(New Phrase(strDeliveryNote, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

            document.Add(pageTable)
            pageTable = Nothing
        End If

        service = Nothing

        'Added by surendra Ticket.#66134

        Dim _baseFontBold As Font = New Font(Font.HELVETICA, 10.0F, Font.BOLD)

        pageTable = New PdfPTable({25.0F, 22.0F, 22.0F, 22.0F})
        pageTable.WidthPercentage = 100.0F
        pageTable.SpacingBefore = 5.0F
        pageTable.SpacingAfter = 5.0F


        pageTable.AddCell(New PdfPCell(New Phrase("PRE-DESPATCH CHECKLIST – sign off", _baseFontBold)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .Colspan = 4, .Padding = 2})
        pageTable.AddCell(New PdfPCell(New Phrase("ITEM", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT, .Padding = 2})
        pageTable.AddCell(New PdfPCell(New Phrase("READY (YES/NO)", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 2})
        pageTable.AddCell(New PdfPCell(New Phrase("DATE", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 2})
        pageTable.AddCell(New PdfPCell(New Phrase("INITIAL", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 2})

        pageTable.AddCell(New PdfPCell(New Phrase("Pack Up", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase("Flyscreens", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase("Flashing/Signboard", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase("Extra Track", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase("Curved Track", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase("Hardware", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase("Extra Loose Materials", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})


        For i As Integer = 0 To 1
            pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        Next

        document.Add(pageTable)
        pageTable = Nothing

        pageTable = New PdfPTable({15.0F, 20.0F, 20.0F, 20.0F, 20.0F})
        pageTable.WidthPercentage = 100.0F
        pageTable.SpacingBefore = 5.0F
        pageTable.SpacingAfter = 5.0F

        pageTable.AddCell(New PdfPCell(New Phrase("Panel Qty", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .Rowspan = 2})
        pageTable.AddCell(New PdfPCell(New Phrase("Package Type ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .Colspan = 2})
        pageTable.AddCell(New PdfPCell(New Phrase("Delivery Method (input courier company)", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER, .Colspan = 2})
        'pageTable.AddCell(New PdfPCell(New Phrase("", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageTable.AddCell(New PdfPCell(New Phrase("# Bundles", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageTable.AddCell(New PdfPCell(New Phrase("# Pallets", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageTable.AddCell(New PdfPCell(New Phrase("Local", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageTable.AddCell(New PdfPCell(New Phrase("Interstate", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})

        pageTable.AddCell(New PdfPCell(New Phrase("", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase("/", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageTable.AddCell(New PdfPCell(New Phrase("/", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_CENTER})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 1, .HorizontalAlignment = Element.ALIGN_LEFT})


        For i As Integer = 0 To 3
            pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
            pageTable.AddCell(New PdfPCell(New Phrase(" ", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        Next

        document.Add(pageTable)
        pageTable = Nothing

        pageTable = New PdfPTable({15.0F, 45.0F, 20.0F, 20.0F})
        pageTable.WidthPercentage = 100.0F
        pageTable.SpacingBefore = 5.0F
        pageTable.SpacingAfter = 5.0F

        pageTable.AddCell(New PdfPCell(New Phrase("Signature:", baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont)) With {.BorderWidthBottom = 1, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase("Despatch Date:", baseFont)) With {.BorderWidth = 0, .PaddingLeft = 20, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont)) With {.BorderWidthBottom = 1, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase("Print Name:", baseFont)) With {.BorderWidth = 0, .PaddingTop = 10, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont)) With {.BorderWidthBottom = 1, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        pageTable.AddCell(New PdfPCell(New Phrase("Connote Ref:", baseFont)) With {.BorderWidth = 0, .PaddingTop = 10, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont)) With {.BorderWidthBottom = 1, .BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})
        pageTable.AddCell(New PdfPCell(New Phrase(String.Empty, baseFont)) With {.BorderWidth = 0, .HorizontalAlignment = Element.ALIGN_LEFT})

        document.Add(pageTable)
        pageTable = Nothing


        'page number/of



        'date time generated


        document.Close()

        Return True

    End Function





End Class
