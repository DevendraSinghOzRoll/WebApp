Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports OfficeOpenXml
Imports System.Xml
Imports System.Drawing
Imports OfficeOpenXml.Style

Public Class LouvreDailyScheduleExport

    Public Shared Function generateDailyScheduleExport(ByVal stroutputDir As String, ByRef objBuffer As Byte(), ByVal dtReportData As DataTable, dteReportDate As Date) As String

        Dim outputDir As DirectoryInfo = New DirectoryInfo(stroutputDir)
        Dim newFile As FileInfo = New FileInfo(outputDir.FullName & "\LouvreDailyProductionScheduleTemplate.xltx")
        If Not newFile.Exists Then
            Return "Template Not found."
        End If
        '
        Dim strCurr As String = Now.ToString.Replace("/", "")
        strCurr = strCurr.Replace(":", "")
        strCurr = strCurr.Replace(" ", "")
        strCurr = strCurr.Substring(0, strCurr.Length - 3)
        strCurr = String.Empty
        Dim strFileFullPath As String = stroutputDir & "\DailyScheduleReport-" & strCurr & ".xlsx"
        Dim objfileVirtualPath As FileInfo = New FileInfo(strFileFullPath)
        If objfileVirtualPath.Exists Then
            objfileVirtualPath.Delete()
        End If

        Dim intColCNT As Integer = 27
        Dim package As ExcelPackage = New ExcelPackage(newFile)
        'Dim xlWks As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
        Dim xlWks As ExcelWorksheet = package.Workbook.Worksheets("Sheet1")

        'xlWks.Cells(2, 2).Value = dteReportDate.ToString("d MMM yyyy")
        xlWks.Cells(2, 1).Value = "Production Date - " & dteReportDate.ToString("dddd d MMMM yyyy")

        If dtReportData.Rows.Count > 0 Then
            loadDailyScheduleExportData(dtReportData, xlWks)
        End If
        '

        'Dim rangeTMP = xlWks.Cells(1, 1, 1, 27)
        'rangeTMP.Style.Font.Bold = True
        'rangeTMP.Style.Fill.PatternType = ExcelFillStyle.Solid
        'rangeTMP.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue)
        'rangeTMP.Style.Font.Color.SetColor(Color.White)


        objBuffer = package.GetAsByteArray()
        'rangeTMP = Nothing
        xlWks.Dispose()
        xlWks = Nothing
        package = Nothing
        Return objfileVirtualPath.Name

    End Function

    Public Shared Sub loadDailyScheduleExportData(dtReportData As DataTable, ByRef xlWks As ExcelWorksheet)

        Dim intColNum As Integer = 1
        Dim intRowNum As Integer = 5

        For i As Integer = 0 To dtReportData.Rows.Count - 1
            intColNum = 1

            xlWks.Cells(intRowNum, intColNum).Value = dtReportData.Rows(i)("ShutterProNumber").ToString
            intColNum += 1
            xlWks.Cells(intRowNum, intColNum).Value = dtReportData.Rows(i)("OrderType").ToString
            intColNum += 1
            xlWks.Cells(intRowNum, intColNum).Value = dtReportData.Rows(i)("BranchName").ToString
            intColNum += 1

            xlWks.Cells(intRowNum, intColNum).Value = dtReportData.Rows(i)("ReferenceNumber").ToString
            intColNum += 1
            xlWks.Cells(intRowNum, intColNum).Value = dtReportData.Rows(i)("ReferenceName").ToString
            intColNum += 1
            xlWks.Cells(intRowNum, intColNum).Value = dtReportData.Rows(i)("State").ToString
            intColNum += 1
            xlWks.Cells(intRowNum, intColNum).Value = dtReportData.Rows(i)("NoOfPanels").ToString
            intColNum += 1
            xlWks.Cells(intRowNum, intColNum).Value = CDate(dtReportData.Rows(i)("ScheduledDate")).ToString("d MMM yyyy")
            intColNum += 1
            xlWks.Cells(intRowNum, intColNum).Value = dtReportData.Rows(i)("Priority").ToString
            intColNum += 1
            If Not IsDBNull(dtReportData.Rows(i)("ExpectedShippingDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = CDate(dtReportData.Rows(i)("ExpectedShippingDate")).ToString("d MMM yyyy")
            End If
            'xlWks.Cells(intRowNum, intColNum).Value = dtReportData.Rows(i)("PlannedShippingDate").ToString
            intColNum += 2
            If Not IsDBNull(dtReportData.Rows(i)("hidCuttingDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = CDate(dtReportData.Rows(i)("hidCuttingDate")).ToString("d MMM yyyy")
            End If
            intColNum += 1
            If Not IsDBNull(dtReportData.Rows(i)("hidPiningDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = CDate(dtReportData.Rows(i)("hidPiningDate")).ToString("d MMM yyyy")
            End If
            intColNum += 1
            If Not IsDBNull(dtReportData.Rows(i)("hidPrepDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = CDate(dtReportData.Rows(i)("hidPrepDate")).ToString("d MMM yyyy")
            End If
            intColNum += 1
            If Not IsDBNull(dtReportData.Rows(i)("hidAssemblyDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = CDate(dtReportData.Rows(i)("hidAssemblyDate")).ToString("d MMM yyyy")
            End If
            intColNum += 1
            If Not IsDBNull(dtReportData.Rows(i)("hidHingingDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = CDate(dtReportData.Rows(i)("hidHingingDate")).ToString("d MMM yyyy")
            End If
            intColNum += 1
            If Not IsDBNull(dtReportData.Rows(i)("hidPackupDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = CDate(dtReportData.Rows(i)("hidPackupDate")).ToString("d MMM yyyy")
            End If

            If Not IsDBNull(dtReportData.Rows(i)("hidFramingStatus")) Then
                If CInt(dtReportData.Rows(i)("hidFramingStatus")) = 4 Then
                    'not required
                    Dim rangeTMP = xlWks.Cells(intRowNum, intColNum)
                    rangeTMP.Style.Fill.PatternType = ExcelFillStyle.Solid
                    rangeTMP.Style.Fill.BackgroundColor.SetColor(Color.DarkSlateGray)
                End If
            End If

            intColNum += 1
            If Not IsDBNull(dtReportData.Rows(i)("hidQCDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = CDate(dtReportData.Rows(i)("hidQCDate")).ToString("d MMM yyyy")
            End If

            intColNum += 1
            'If Not IsDBNull(dtReportData.Rows(i)("hidDespatchDate")) Then
            '    xlWks.Cells(intRowNum, intColNum).Value = CDate(dtReportData.Rows(i)("hidDespatchDate")).ToString("d MMM yyyy")
            'End If

            intRowNum += 1

        Next

        xlWks.DeleteRow(intRowNum, 1000)

    End Sub

End Class
