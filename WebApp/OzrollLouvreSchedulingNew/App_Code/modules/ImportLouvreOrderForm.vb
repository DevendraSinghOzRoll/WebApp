﻿Option Strict Off
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports OfficeOpenXml
Imports System.Xml
Imports System.Drawing
Imports OfficeOpenXml.Style

Public Class importLouvreOrderForm

    Public Shared Function readExcel(ByVal strExcelFilePath As String, ByRef strCompanyName As String, ByRef strLastName As String, _
                                     ByRef strContractNo As String, ByRef dteReqDate As Date, ByRef intNoOfPanels As Integer) As List(Of LouvreDetails)

        Dim objNewFile As FileInfo = Nothing
        If chkTempExcel(strExcelFilePath, objNewFile) = False Then
            objNewFile = Nothing
            Return New List(Of LouvreDetails)
        End If
        Dim package As ExcelPackage = New ExcelPackage(objNewFile)
        Dim xlWks As ExcelWorksheet = package.Workbook.Worksheets("Order Form")
        '
        Dim arrLouvreDetCls As New List(Of LouvreDetails)
        Dim intRowCounter As Integer = 5
        Dim objCell As Object = Nothing
        strCompanyName = String.Empty
        If xlWks.Cells(intRowCounter, 2).Value <> String.Empty Then
            objCell = xlWks.Cells(intRowCounter, 2)
            If Not IsDBNull(objCell) Then
                strCompanyName = objCell.Value 'strCompanyName
            End If
        End If
        If xlWks.Cells(intRowCounter, 6).Value <> String.Empty Then
            objCell = xlWks.Cells(intRowCounter, 6)
            strLastName = String.Empty
            If Not IsDBNull(objCell) Then
                strLastName = objCell.Value 'strTradingName
            End If
        End If
        If xlWks.Cells(intRowCounter, 12).Value <> String.Empty Then
            objCell = xlWks.Cells(intRowCounter, 12)
            strContractNo = String.Empty
            If Not IsDBNull(objCell) Then
                strContractNo = objCell.Value
            End If
        End If
        If xlWks.Cells(intRowCounter, 15).Value <> String.Empty Then
            objCell = xlWks.Cells(intRowCounter, 15)
            dteReqDate = SharedConstants.DEFAULT_DATE_VALUE
            If Not IsDBNull(objCell) Then
                dteReqDate = CDate(objCell.Value)
            End If
        End If
        '
        'get no of panels
        intRowCounter = 21
        If xlWks.Cells(intRowCounter, 9).Value.ToString = String.Empty Then
        Else
            objCell = xlWks.Cells(intRowCounter, 9)
            intNoOfPanels = 0
            If Not IsDBNull(objCell) Then
                intNoOfPanels = objCell.Value
            End If
        End If
        '
        Dim intBlankRowCNT As Integer = 1
        intRowCounter = 8
        readExcelData(intRowCounter, intBlankRowCNT, arrLouvreDetCls, xlWks)
        clearTempExcel(strExcelFilePath)
        Return arrLouvreDetCls

    End Function

    Public Shared Function chkTempExcel(ByVal strTmpExcelPath As String, ByRef objNewFile As FileInfo) As Boolean

        Dim bolFileFound As Boolean = False
        objNewFile = New FileInfo(strTmpExcelPath)
        If objNewFile.Exists Then
            bolFileFound = True
        End If
        Return bolFileFound

    End Function

    Public Shared Sub clearTempExcel(ByVal strTmpExcelPath As String)

        Dim objExcelPath As FileInfo = New FileInfo(strTmpExcelPath)
        If objExcelPath.Exists Then
            objExcelPath.Delete()
        End If

    End Sub

    Public Shared Sub readExcelData(ByRef intRowCounter As Integer, ByRef intBlankRowCNT As Integer, ByRef arrLouvreDetCls As List(Of LouvreDetails),
                                    ByRef xlWks As ExcelWorksheet)

        Dim clsTLouvreDet As New LouvreDetails
        readSShtToCls(clsTLouvreDet, intRowCounter, xlWks)
        If clsTLouvreDet.Location = String.Empty Then
            intBlankRowCNT += 1
        Else
            arrLouvreDetCls.Add(clsTLouvreDet)
        End If
        If intRowCounter <= 40 And intBlankRowCNT <= 10 Then
            readExcelData(intRowCounter + 1, intBlankRowCNT, arrLouvreDetCls, xlWks)
        End If

    End Sub

    Public Shared Sub readSShtToCls(ByRef clsTLouvreDet As LouvreDetails, ByVal intcurRow As Integer, ByRef xlWks As ExcelWorksheet)

        clsTLouvreDet = New LouvreDetails
        If Not IsDBNull(xlWks.Cells(intcurRow, 1)) Then                                 'Locations
            clsTLouvreDet.Location = xlWks.Cells(intcurRow, 1).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 2)) Then                                 'Colour
            clsTLouvreDet.Colour = xlWks.Cells(intcurRow, 2).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 3)) Then                                 'Gross Open Height
            clsTLouvreDet.Height = xlWks.Cells(intcurRow, 3).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 4)) Then                                 'Gross Open Width
            clsTLouvreDet.Width = xlWks.Cells(intcurRow, 4).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 5)) Then                                 'Make Or Open Sizes
            clsTLouvreDet.MakeOrOpenSizes = xlWks.Cells(intcurRow, 5).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 6)) Then                                 'Product
            If Trim(xlWks.Cells(intcurRow, 6).Value) <> String.Empty Then
                clsTLouvreDet.Product = Trim(xlWks.Cells(intcurRow, 6).Value)
            End If
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 7)) Then                                 'Shutter Type
            clsTLouvreDet.ShutterType = xlWks.Cells(intcurRow, 7).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 8)) Then                                 'BiFold / Hinged Door In / Out
            clsTLouvreDet.BiFoldHingedDoorInOut = xlWks.Cells(intcurRow, 8).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 9)) Then                                 'No Of Panels 
            clsTLouvreDet.NoOfPanels = xlWks.Cells(intcurRow, 9).Value
        Else
            clsTLouvreDet.NoOfPanels = 1
        End If

        clsTLouvreDet.NoOfOpenings = 1

        If Not IsDBNull(xlWks.Cells(intcurRow, 10)) Then                                'Blade Size
            clsTLouvreDet.BladeSize = xlWks.Cells(intcurRow, 10).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 11)) Then                                'EndCap Colour
            clsTLouvreDet.EndCapColour = xlWks.Cells(intcurRow, 11).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 12)) Then                                'BladeClip Colour
            clsTLouvreDet.BladeClipColour = xlWks.Cells(intcurRow, 12).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 13)) Then                                'PileColour
            clsTLouvreDet.PileColour = xlWks.Cells(intcurRow, 13).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 14)) Then                                'Top Track Type
            clsTLouvreDet.BottomTrackType = xlWks.Cells(intcurRow, 14).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 15)) Then                                'Bottom Track Type
            clsTLouvreDet.BottomTrackType = xlWks.Cells(intcurRow, 15).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 16)) Then                                'Curved track
            clsTLouvreDet.CurvedTrack = convertYesNoOption(xlWks.Cells(intcurRow, 16).Value)
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 17)) Then                                'Extra track
            clsTLouvreDet.ExtraTrack = xlWks.Cells(intcurRow, 17).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 18)) Then                                'Mid Rail Height
            clsTLouvreDet.MidRailHeight = xlWks.Cells(intcurRow, 18).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 19)) Then                                'FlushBolts Top
            clsTLouvreDet.FlushBoltsTop = xlWks.Cells(intcurRow, 19).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 20)) Then                                'FlushBolts Bottom
            clsTLouvreDet.FlushBoltsBottom = xlWks.Cells(intcurRow, 20).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 21)) Then                                'Lock Options
            clsTLouvreDet.LockOptions = xlWks.Cells(intcurRow, 21).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 22)) Then                                'Blade Locks
            clsTLouvreDet.BladeLocks = convertYesNoOption(xlWks.Cells(intcurRow, 22).Value)
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 23)) Then                                'CChannel /fixed panel channel T & B
            clsTLouvreDet.CChannel = convertYesNoOption(xlWks.Cells(intcurRow, 23).Value)
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 24)) Then                                'fixed panel sides
            clsTLouvreDet.FixedPanelChannel = xlWks.Cells(intcurRow, 24).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 25)) Then                                'H Joiner
            clsTLouvreDet.HChannel = convertYesNoOption(xlWks.Cells(intcurRow, 25).Value)
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 26)) Then                                'L REVEAL
            clsTLouvreDet.LChannelString = xlWks.Cells(intcurRow, 26).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 27)) Then                                'Z REVEAL
            clsTLouvreDet.ZChannelString = xlWks.Cells(intcurRow, 27).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 28)) Then                                'Main Operation 
            clsTLouvreDet.BladeOperation = xlWks.Cells(intcurRow, 28).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 29)) Then                                'Bottom Operation 
            clsTLouvreDet.BladeOperationBottom = xlWks.Cells(intcurRow, 29).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 30)) Then                                'Main Insert
            clsTLouvreDet.InsertTop = xlWks.Cells(intcurRow, 30).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 31)) Then                                'Bottom Insert
            clsTLouvreDet.InsertBottom = xlWks.Cells(intcurRow, 31).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 32)) Then                                'Winder
            clsTLouvreDet.WinderString = xlWks.Cells(intcurRow, 32).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 33)) Then                                'Flyscreen
            clsTLouvreDet.FlyScreen = convertYesNoOption(xlWks.Cells(intcurRow, 33).Value)   'xlWks.Cells(intcurRow, 33).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 34)) Then                                'Slide
            clsTLouvreDet.Slide = xlWks.Cells(intcurRow, 34).Value
        End If
        If Not IsDBNull(xlWks.Cells(intcurRow, 35)) Then                                'Stacker Location
            clsTLouvreDet.StackerLocation = xlWks.Cells(intcurRow, 35).Value
        End If
        '

    End Sub

    Public Shared Sub chkCellType(ByVal objCell As Object, ByRef strCellValue As String, ByRef intCellValue As Integer,
                                  ByRef decCellValue As Decimal, ByRef dteCellValue As Date, ByRef bolCellValue As Boolean)

        If Not IsDBNull(objCell) Then

        End If

    End Sub

    Public Shared Function convertYesNoOption(ByVal strFieldValue As String) As Integer

        Dim intReturnString As Integer = 0
        If strFieldValue <> String.Empty Then
            If strFieldValue.ToLower <> "no" Then
                intReturnString = 1
            End If
        End If
        Return intReturnString

    End Function

End Class
