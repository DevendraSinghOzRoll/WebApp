'*******************************************************************************
' * You may amend and distribute as you like, but don't remove this header!
' * 
' * All rights reserved.
' * 
' * EPPlus is an Open Source project provided under the 
' * GNU General Public License (GPL) as published by the 
' * Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
' * 
' * EPPlus provides server-side generation of Excel 2007 spreadsheets.
' * See http://www.codeplex.com/EPPlus for details.
' *
' *
' * 
' * The GNU General Public License can be viewed at http://www.opensource.org/licenses/gpl-license.php
' * If you unfamiliar with this license or have questions about it, here is an http://www.gnu.org/licenses/gpl-faq.html
' * 
' * The code for this project may be used and redistributed by any means PROVIDING it is 
' * not sold for profit without the author's written consent, and providing that this notice 
' * and the author's name and all copyright notices remain intact.
' * 
' * All code and executables are provided "as is" with no warranty either express or implied. 
' * The author accepts no liability for any damage or loss of business that this product may cause.
' *
' *
' * Code change notes:
' * 
' * Author							Change						Date
' *******************************************************************************
' * Jan Källman		Added		10-SEP-2009
' *******************************************************************************/

imports System
imports System.Collections.Generic
imports System.Text
imports System.IO
imports OfficeOpenXml
imports System.Xml
imports System.Drawing
imports OfficeOpenXml.Style

Public Class LouvreJobsReport

    Public Shared Function createReport(ByVal stroutputDir As String, ByRef objBuffer As Byte(), ByVal dtScheduleListTBL As DataTable) As String

        Dim outputDir As DirectoryInfo = New DirectoryInfo(stroutputDir)
        Dim newFile As FileInfo = New FileInfo(outputDir.FullName & "\LouvreProductionSchedule.xlsx")
        'Dim newFile As FileInfo = New FileInfo(outputDir.FullName & "\ProductionSchedule2017.xltx")       
        If Not newFile.Exists Then
            Return "Template Not found."
        End If
        '
        Dim strCurr As String = Now.ToString.Replace("/", "")
        strCurr = strCurr.Replace(":", "")
        strCurr = strCurr.Replace(" ", "")
        strCurr = strCurr.Substring(0, strCurr.Length - 3)
        'strCurr = String.Empty
        Dim strFileFullPath As String = stroutputDir & "\LouvreProductionSchedule-" & strCurr & ".xlsx"
        Dim objfileVirtualPath As FileInfo = New FileInfo(strFileFullPath)
        If objfileVirtualPath.Exists Then
            objfileVirtualPath.Delete()
        End If

        Dim intColCNT As Integer = 27
        Dim intColNum As Integer = 1
        Dim intRowNum As Integer = 1
        Dim package As ExcelPackage = New ExcelPackage(newFile)
        Dim xlWks As ExcelWorksheet = package.Workbook.Worksheets("Production Schedule")

        'setupTitleRow(intRowNum, intColNum, xlWks)
        intRowNum = 5
        If dtScheduleListTBL.Rows.Count > 0 Then
            populateDataROWs(intRowNum, dtScheduleListTBL, xlWks)
        End If
        '

        objBuffer = package.GetAsByteArray()
        'rangeTMP = Nothing
        xlWks.Dispose()
        xlWks = Nothing
        package = Nothing
        Return objfileVirtualPath.Name

    End Function

    Shared Sub setupTitleRow(ByRef intRowNum As Integer, ByRef intColNum As Integer, ByRef xlWks As ExcelWorksheet)

        xlWks.Cells(intRowNum, intColNum).Value = "Samples Dli & CL"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "TYPE"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "QtyWeek1"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "QtyWeek2"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "QtyWeek3"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "QtyWeek4"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "QtyWeek5"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "QtyWeek6"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "SqmWeek1"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "SqmWeek2"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "SqmWeek3"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "SqmWeek4"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "SqmWeek5"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "SqmWeek6"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "Material"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "Status"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "ScheduleDate"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "dateReq"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "Notes"
        '

        'job stage date columns added to right
        intColNum += 3
        'xlWks.Cells(intRowNum, intColNum).Value = "Picking"
        'intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "Cutting"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "Pinning"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "Prep"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "Assembly"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "Packup"
        intColNum += 1
        xlWks.Cells(intRowNum, intColNum).Value = "QC"




    End Sub

    Shared Sub populateDataROWs(ByRef intRowNum As Integer, ByVal dtScheduleListTBL As DataTable, ByRef xlWks As ExcelWorksheet)

        '
        Dim dtWKMonthsTBL As DataTable = New DataTable
        If dtScheduleListTBL.Rows.Count > 0 Then
            populateDataROW(intRowNum, dtScheduleListTBL, dtWKMonthsTBL, xlWks)
        Else
            intRowNum += 1
            xlWks.Cells(intRowNum, 1).Value = ""                    '"No Records Found."
            'Dim rangeTMP As ExcelRange = xlWks.Cells(intRowNum, 1, intRowNum, 1)
            'rangeTMP.Style.Font.Size = 12
            'rangeTMP.Style.Font.Bold = True
            'rangeTMP.Style.Font.Color.SetColor(Color.Red)
            'rangeTMP = xlWks.Cells(intRowNum, 1, intRowNum, 27)
        End If
        dtWKMonthsTBL = Nothing

    End Sub

    Shared Sub populateDataROW(ByRef intRowNum As Integer, ByRef dtScheduleListTBL As DataTable, ByRef dtWKMonthsTBL As DataTable, ByRef xlWks As ExcelWorksheet)

        Dim intCurrWkId As Integer = 0
        'Dim dtDayWeekId As DataTable = getAllweekmonth()
        'Dim drTMP() As DataRow = dtDayWeekId.Select("DayDate='" & Format(Today, "d/MMM/yyyy") & "'")
        'If drTMP.Length > 0 Then
        '    intCurrWkId = drTMP(0)("WeekID")
        'End If
        Dim intColNum As Integer = 1
        Dim intWkNo As Integer = 0
        For intDBrows = 0 To dtScheduleListTBL.Rows.Count - 1
            intColNum = 1
            intWkNo = 0
            Dim strTMP As String = String.Empty
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("JobDisplay")) Then
                strTMP = dtScheduleListTBL.Rows(intDBrows)("JobDisplay")
                xlWks.Cells(intRowNum, intColNum).Value = dtScheduleListTBL.Rows(intDBrows)("JobDisplay").ToString
            End If
            intColNum += 1
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("StyleName")) Then
                xlWks.Cells(intRowNum, intColNum).Value = dtScheduleListTBL.Rows(intDBrows)("StyleName").ToString
            End If
            'If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("WeekId")) Then
            '    intWkNo = dtScheduleListTBL.Rows(intDBrows)("WeekId") - intCurrWkId
            '    Select Case intWkNo
            '        Case Is <= 0              'current week
            xlWks.Cells(intRowNum, 3).Value = dtScheduleListTBL.Rows(intDBrows)("PanelWk1").ToString
            '        Case 1
            xlWks.Cells(intRowNum, 4).Value = dtScheduleListTBL.Rows(intDBrows)("PanelWk2").ToString
            '        Case 2
            xlWks.Cells(intRowNum, 5).Value = dtScheduleListTBL.Rows(intDBrows)("PanelWk3").ToString
            '        Case 3
            xlWks.Cells(intRowNum, 6).Value = dtScheduleListTBL.Rows(intDBrows)("PanelWk4").ToString
            '        Case 4
            xlWks.Cells(intRowNum, 7).Value = dtScheduleListTBL.Rows(intDBrows)("PanelWk5").ToString
            '        Case 5
            xlWks.Cells(intRowNum, 8).Value = dtScheduleListTBL.Rows(intDBrows)("PanelWk6").ToString
            '        Case Is >= 6
            xlWks.Cells(intRowNum, 9).Value = dtScheduleListTBL.Rows(intDBrows)("PanelWk7").ToString
            '    End Select
            'End If
            'If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("WeekId")) Then
            '    Select Case intWkNo
            '        Case 0              'current week
            xlWks.Cells(intRowNum, 11).Value = dtScheduleListTBL.Rows(intDBrows)("SQMWk1").ToString
            '        Case 1
            xlWks.Cells(intRowNum, 12).Value = dtScheduleListTBL.Rows(intDBrows)("SQMWk2").ToString
            '        Case 2
            xlWks.Cells(intRowNum, 13).Value = dtScheduleListTBL.Rows(intDBrows)("SQMWk3").ToString
            '        Case 3
            xlWks.Cells(intRowNum, 14).Value = dtScheduleListTBL.Rows(intDBrows)("SQMWk4").ToString
            '        Case 4
            xlWks.Cells(intRowNum, 15).Value = dtScheduleListTBL.Rows(intDBrows)("SQMWk5").ToString
            '        Case 5
            xlWks.Cells(intRowNum, 16).Value = dtScheduleListTBL.Rows(intDBrows)("SQMWk6").ToString
            '        Case 6
            xlWks.Cells(intRowNum, 17).Value = dtScheduleListTBL.Rows(intDBrows)("SQMWk7").ToString
            '    End Select
            'End If
            intColNum = 18
            '----for dummy
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("ProdNotes")) Then
                xlWks.Cells(intRowNum, intColNum).Value = dtScheduleListTBL.Rows(intDBrows)("ProdNotes").ToString
            End If
            '
            intColNum += 1
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("ColourName")) Then
                xlWks.Cells(intRowNum, intColNum).Value = dtScheduleListTBL.Rows(intDBrows)("ColourName").ToString
            End If
            intColNum += 1
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("DateRequired")) Then
                xlWks.Cells(intRowNum, intColNum).Value = Format(CDate(dtScheduleListTBL.Rows(intDBrows)("DateRequired")), "d MMM yyyy")
            End If
            intColNum += 1
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("ScheduledDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = Format(CDate(dtScheduleListTBL.Rows(intDBrows)("ScheduledDate")), "d MMM yyyy")
            End If

            intColNum += 1
            'If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("ProdNotes")) Then
            '    xlWks.Cells(intRowNum, intColNum).Value = dtScheduleListTBL.Rows(intDBrows)("ProdNotes").ToString
            'End If


            'job stage date columns added to right
            intColNum += 2
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("PickingDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = Format(CDate(dtScheduleListTBL.Rows(intDBrows)("PickingDate")), "d MMM yyyy")
            End If

            intColNum += 1 'to powdercoater
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("PowdercoatStartDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = Format(CDate(dtScheduleListTBL.Rows(intDBrows)("PowdercoatStartDate")), "d MMM yyyy")
            End If
            intColNum += 1 'from powdercoater
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("PowdercoatEndDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = Format(CDate(dtScheduleListTBL.Rows(intDBrows)("PowdercoatEndDate")), "d MMM yyyy")
            End If

            intColNum += 1
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("CuttingDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = Format(CDate(dtScheduleListTBL.Rows(intDBrows)("CuttingDate")), "d MMM yyyy")
            End If

            intColNum += 1
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("PiningDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = Format(CDate(dtScheduleListTBL.Rows(intDBrows)("PiningDate")), "d MMM yyyy")
            End If

            intColNum += 1
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("PrepDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = Format(CDate(dtScheduleListTBL.Rows(intDBrows)("PrepDate")), "d MMM yyyy")
            End If

            intColNum += 1
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("AssemblyDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = Format(CDate(dtScheduleListTBL.Rows(intDBrows)("AssemblyDate")), "d MMM yyyy")
            End If

            intColNum += 1 'hinging
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("HingingDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = Format(CDate(dtScheduleListTBL.Rows(intDBrows)("HingingDate")), "d MMM yyyy")
            End If

            intColNum += 1
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("PackupDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = Format(CDate(dtScheduleListTBL.Rows(intDBrows)("PackupDate")), "d MMM yyyy")
            End If

            intColNum += 1
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("QCDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = Format(CDate(dtScheduleListTBL.Rows(intDBrows)("QCDate")), "d MMM yyyy")
            End If

            intColNum += 1
            If Not IsDBNull(dtScheduleListTBL.Rows(intDBrows)("DespatchDate")) Then
                xlWks.Cells(intRowNum, intColNum).Value = Format(CDate(dtScheduleListTBL.Rows(intDBrows)("DespatchDate")), "d MMM yyyy")
            End If

            intRowNum += 1
            If intRowNum >= 213 Then
                Exit For
            End If
            'xlWks.InsertRow(intRowNum, 1)
        Next

    End Sub

    Shared Function xgetAllweekmonth() As DataTable

        Dim strSQL As String = "Select DayDate,WeekID from loctblDays"
        Dim service As New AppService
        Dim dtAllweekmonth As DataTable = service.runSQLOSCDatabase(strSQL)
        service = Nothing
        Return dtAllweekmonth

    End Function



End Class


		
