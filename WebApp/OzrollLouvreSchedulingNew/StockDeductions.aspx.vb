Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Linq
Imports OzrollPSLVSchedulingModel.SharedEnums
Imports OzrollPSLVSchedulingModel.SharedFunctions

Partial Class StockDeductions
    Inherits System.Web.UI.Page

    Private _Service As New AppService

    Private Const _STOCK_DEDUCTIONS_COMPLETE_MSG As String = "Complete"
    Private Const _STOCK_DEDUCTIONS_PROCESSING_MSG As String = "Processing..."
    Private Const _STOCK_DEDUCTIONS_FAILURE_MSG As String = "ERROR"

    Private Sub StockDeductions_Load(sender As Object, e As EventArgs) Handles Me.Load

        RedirectIfInvalidUserSession(Session, Response)

        If Not IsPostBack Then

            'don't allow access to this page if in as plantation
            If Session("sessProductTypeID").ToString = "1" Then
                Response.Redirect("Home.aspx", False)
                Exit Sub
            End If

            ' The URL to return to when closing the schedule (if set)
            ViewState("ReturnURL") = 0

            If Not Request.QueryString.Count = 0 Then
                If Not IsNothing(Request.Params("ScheduleID")) Then
                    ViewState("ScheduleID") = CInt(Request.Params("ScheduleID"))
                End If
                If Not IsNothing(Request.Params("ReturnURL")) Then
                    ViewState("ReturnURL") = CInt(Request.Params("ReturnURL"))
                End If
            End If

            ConfigureUI()

            If pnlStockDeductions.Visible Then
                PopulateDeductions()
            End If
        Else
            ConfigureUI()
        End If

        ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(String), "StyleCheckBoxesForFocus", "styleCheckboxes();", True)
    End Sub

    Private Sub ConfigureUI()

        pnlStockDeductions.Visible = True
        lblOZNumberHeading.Text = String.Empty
        lblStockDeductionsStatus.Text = String.Empty
        pnlStockDeductionsStatus.Visible = False
        btnResetDeductions.Visible = False

        If ViewState("ScheduleID") > 0 Then
            Dim cProductionSchedule As ProductionSchedule = _Service.GetProdScheduleClsByID(ViewState("ScheduleID"))

            If cProductionSchedule.ID > 0 Then
                If Not cProductionSchedule.Deleted Then
                    If cProductionSchedule.ProductTypeID = Session("sessProductTypeID") Then

                        lblOZNumberHeading.Text = cProductionSchedule.ShutterProNumber

                        If cProductionSchedule.OrderStatus > ProductionScheduleStatus.PaperworkProcessing Then
                            Dim lStockDeductions As List(Of StockDeduction) = _Service.GetStockDeductionsByProductionScheduleID(ViewState("ScheduleID"))
                            Dim enumDeductionStatus As StockDeductionStatus = StockDeductionStatus.Success

                            If lStockDeductions.Where(Function(x) x.Status = StockDeductionStatus.Failure).Any() Then
                                enumDeductionStatus = StockDeductionStatus.Failure
                            ElseIf lStockDeductions.Where(Function(x) x.Status = StockDeductionStatus.AwaitingProcessing).Any() Then
                                enumDeductionStatus = StockDeductionStatus.AwaitingProcessing
                            End If

                            If enumDeductionStatus = StockDeductionStatus.AwaitingProcessing Then
                                lblStockDeductionsStatus.ForeColor = Color.Orange
                                lblStockDeductionsStatus.Text = _STOCK_DEDUCTIONS_PROCESSING_MSG
                                pnlStockDeductions.Visible = False

                            ElseIf enumDeductionStatus = StockDeductionStatus.Failure Then
                                lblStockDeductionsStatus.ForeColor = Color.Red
                                lblStockDeductionsStatus.Text = _STOCK_DEDUCTIONS_FAILURE_MSG & " - " & lStockDeductions(0).StatusMessage
                                pnlStockDeductions.Visible = False
                                btnResetDeductions.Visible = True

                            ElseIf (lStockDeductions.Count > 0) AndAlso enumDeductionStatus = StockDeductionStatus.Success Then
                                lblStockDeductionsStatus.ForeColor = Color.Green
                                lblStockDeductionsStatus.Text = _STOCK_DEDUCTIONS_COMPLETE_MSG
                                pnlStockDeductions.Visible = False
                            End If
                        Else
                            btnBack_Click(Nothing, Nothing)
                            pnlStockDeductions.Visible = False
                        End If
                    Else
                        btnBack_Click(Nothing, Nothing)
                        pnlStockDeductions.Visible = False
                    End If
                Else
                    btnBack_Click(Nothing, Nothing)
                    pnlStockDeductions.Visible = False
                End If
            Else
                btnBack_Click(Nothing, Nothing)
                pnlStockDeductions.Visible = False
            End If
        Else
            btnBack_Click(Nothing, Nothing)
            pnlStockDeductions.Visible = False
        End If

        If lblStockDeductionsStatus.Text.Length > 0 Then
            pnlStockDeductionsStatus.Visible = True
        End If

    End Sub

    Private Sub PopulateDeductions()

        ViewState("OptimisedItems") = Nothing

        If ViewState("ScheduleID") > 0 Then
            Dim cProdScheduleGen As New ProductionScheduleGenerator
            Dim cOptimiser As Optimisation.Optimiser = Nothing
            Dim cPDF As PDFGenerationResult = cProdScheduleGen.GeneratePDF(ViewState("ScheduleID"), cOptimiser)
            Dim lOptimisedItems As New List(Of Optimisation.OptimisedItem)

            For Each kvp As KeyValuePair(Of String, Optimisation.OptimisedItem) In cOptimiser.OptimisedListByCode
                lOptimisedItems.Add(kvp.Value)
            Next kvp

            ViewState("OptimisedItems") = lOptimisedItems
            gdvStockDeductions.DataSource = lOptimisedItems
            gdvStockDeductions.DataBind()
        End If

    End Sub

    Private Sub btnBeginDeductions_Click(sender As Object, e As EventArgs) Handles btnBeginDeductions.Click
        Dim lOptimisedItems As New List(Of Optimisation.OptimisedItem)(DirectCast(ViewState("OptimisedItems"), List(Of Optimisation.OptimisedItem)))
        Dim strStatusMsg As String = String.Empty
        Dim lItemsToRemove As New List(Of Optimisation.OptimisedItem)

        ' Remove unchecked items.
        For Each cRow As GridViewRow In gdvStockDeductions.Rows
            Dim cchkDeduct As CheckBox = cRow.Cells(5).FindControl("chkDeduct")

            If Not cchkDeduct.Checked Then
                lItemsToRemove.Add(lOptimisedItems(cRow.DataItemIndex))
            End If
        Next cRow

        lOptimisedItems.RemoveAll(Function(x) lItemsToRemove.Contains(x))

        If lOptimisedItems IsNot Nothing AndAlso lOptimisedItems.Count > 0 Then
            Dim cProductionSchedule As ProductionSchedule = _Service.GetProdScheduleClsByID(ViewState("ScheduleID"))

            If cProductionSchedule.ID > 0 Then
                Dim lStockDeductions As New List(Of StockDeduction)
                Dim cReader As New StockDeductionsReader(Session("sessUserID"), cProductionSchedule.ID, cProductionSchedule.SybizJobCode)

                If cReader.ReadOptimisedList(lOptimisedItems, lStockDeductions, strStatusMsg) Then
                    Dim dbConn As New DBConnection
                    Dim cnn As SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
                    Dim trans As SqlTransaction = Nothing
                    Dim bolContinue As Boolean = True

                    Try
                        cnn.Open()
                        trans = cnn.BeginTransaction

                        For Each d As StockDeduction In lStockDeductions
                            If bolContinue Then
                                ' Write to local table
                                bolContinue = (_Service.AddOrUpdateStockDeduction(d, cnn, trans) > 0)
                            Else
                                Exit For
                            End If
                        Next d

                        If bolContinue Then
                            bolContinue = SetProductionScheduleCostPrices(cProductionSchedule, cnn, trans, strStatusMsg)
                        End If

                        If bolContinue Then
                            trans.Commit()
                        Else
                            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - Transaction Rolled Back" & Environment.NewLine & GetPageInfo(form1, Session))
                            trans.Rollback()
                        End If

                    Catch ex As Exception

                        If Not trans Is Nothing Then
                            trans.Rollback()
                        End If

                        If cnn.State = ConnectionState.Open Then
                            cnn.Close()
                        End If

                        EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message & Environment.NewLine & GetPageInfo(form1, Session))
                        bolContinue = False

                    Finally
                        trans.Dispose()
                        trans = Nothing

                        If cnn.State = ConnectionState.Open Then
                            cnn.Close()
                        End If

                        cnn.Dispose()
                        cnn = Nothing
                    End Try

                    If bolContinue Then
                        ConfigureUI()

                        gdvStockDeductions.DataSource = Nothing
                        gdvStockDeductions.DataBind()

                        pnlStockDeductions.Visible = False

                        Exit Sub
                    End If
                End If
            End If
        End If

        lblErrorMsg.Text = strStatusMsg
        lblErrorMsg.ForeColor = Color.Red
    End Sub

    ''' <summary>
    ''' ' Sets the local cost prices from Sybiz for the given production schedule.
    ''' </summary>
    ''' <param name="cProductionSchedule"></param>
    ''' <param name="cnn"></param>
    ''' <param name="trans"></param>
    Private Function SetProductionScheduleCostPrices(cProductionSchedule As ProductionSchedule,
                                                     cnn As SqlConnection,
                                                     trans As SqlTransaction,
                                                     ByRef strErrorMsgOut As String) As Boolean

        strErrorMsgOut = String.Empty

        Dim cCacheSybizProducts As New OzrollPSLVSchedulingModel.Cache.CacheSybizProducts
        Dim lOptimisedItems As List(Of Optimisation.OptimisedItem) = ViewState("OptimisedItems")
        Dim lCodes As New List(Of String)
        Dim decTotalCost As Decimal = 0
        Dim boolContinue As Boolean = True

        For Each i As Optimisation.OptimisedItem In lOptimisedItems
            lCodes.Add(i.ProductCode)
        Next i

        cCacheSybizProducts.CacheSybizProductsByCodes(_Service, lCodes)

        For Each i As Optimisation.OptimisedItem In lOptimisedItems
            Dim cSybizProduct As SybizProduct = cCacheSybizProducts.ItemByCode(i.ProductCode)

            If cSybizProduct.ID > 0 Then
                decTotalCost = decTotalCost + (cSybizProduct.StandardCost * i.MaterialLengthTotalFullLengthsRequiredMetres)
            Else
                ' Product not found in sybiz so can't create a total cost.
                boolContinue = False
                strErrorMsgOut = "ERROR: " & i.ProductCode & " not found in Sybiz. Could not get cost price."
                Exit For
            End If
        Next i

        If boolContinue Then
            ' Update PS cost price
            cProductionSchedule.CostPrice = decTotalCost

            boolContinue = _Service.UpdateProductionScheduleByID(cProductionSchedule, cnn, trans)
        End If

        Return boolContinue
    End Function

    Protected Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        Response.Redirect("Home.aspx")
    End Sub
    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        Response.Redirect("Logout.aspx", False)
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Dim enumReturnURL As SharedEnums.ReturnURLID = ViewState("ReturnURL")

        Select Case enumReturnURL
            Case SharedEnums.ReturnURLID.LouvreJobDetails
                Response.Redirect("LouvreJobDetails.aspx?ScheduleId=" & ViewState("ScheduleID") & "&ViewType=1", False)
            Case Else
                Response.Redirect("UpdateAwaitingAcceptance.aspx", False)

        End Select

    End Sub

    Private Sub ResetStockDeductionsForProductionSchedule()

        If ViewState("ScheduleID") > 0 Then
            _Service.DeleteStockDeductionsByProductionScheduleID(ViewState("ScheduleID"))
        End If
    End Sub

    Private Sub btnResetDeductions_Click(sender As Object, e As EventArgs) Handles btnResetDeductions.Click
        ResetStockDeductionsForProductionSchedule()
        Response.Redirect(Request.RawUrl, False)
    End Sub
End Class
