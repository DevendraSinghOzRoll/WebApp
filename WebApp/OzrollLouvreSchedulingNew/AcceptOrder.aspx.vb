Imports System.Linq

Partial Class AcceptOrder
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        SharedFunctions.RedirectIfInvalidUserSession(Session, Response)

        If Not IsPostBack Then

            txtProductTypeID.Text = Session("sessProductTypeID").ToString

            Dim intScheduleID As Integer = 0

            If Not Request.QueryString.Count = 0 Then
                If Not IsNothing(Request.Params("ScheduleID")) Then
                    intScheduleID = CInt(Request.Params("ScheduleID"))
                End If
            End If

            txtID.Text = intScheduleID.ToString

            Dim service As New AppService
            service.addWebsitePageAccess("Ozroll Plantation Scheduling", CInt(Session("sessUserID")), Session("sessUserName").ToString, System.IO.Path.GetFileName(Request.PhysicalPath), Request.QueryString.ToString(), Now)

            populateDetails(intScheduleID)

        End If

    End Sub

    Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        SharedFunctions.PageError(Page, form1)
    End Sub

    Private Sub populateDetails(ProductionScheduleID As Integer)
        Dim service As New AppService
        Dim cProductionSchedule As ProductionSchedule = service.GetProdScheduleClsByID(txtID.Text)
        Dim strLabel As String = String.Empty

        lblOZNumberHeading.Text = String.Empty

        If cProductionSchedule.ID > 0 Then

            lblOZNumberHeading.Text = cProductionSchedule.ShutterProNumber

            strLabel = "<table cellspacing=""0"" class=""form-table"" summary="""">"

            Dim cLouvreDetails As LouvreDetailsCollection = service.GetLouvreDetailsCollectionByProductionScheduleID(ProductionScheduleID).RemoveDeleted

            For Each l As LouvreDetails In cLouvreDetails

                strLabel &= "<tr>"
                strLabel &= "<td class=""form-label-td"" style=""width: 15%; text-align: center;"">"
                strLabel &= "<input type=""submit"" id=""" & l.LouvreDetailID & """ name=""btnViewDetails"" value=""View Details"" OnClick=""getViewDetailsModalPopupButton(this.id)"" class=""updatebutton"" style=""font-size: 13px;height: 29px;"">"
                strLabel &= "</td>"
                strLabel &= "<td class=""form-label-td"" style=""width: 15%; text-align: center;"">"
                strLabel &= "</td>"
                strLabel &= "<td class=""form-label-td"" style=""width: 15%; text-align: center;"">"
                strLabel &= "</td>"
                strLabel &= "<td class=""form-label-td"" style=""text-align: left;"">"
                strLabel &= "<b>Opening Type:</b> " & l.ShutterType & " "
                strLabel &= "<b>Product:</b> " & l.Product & " "
                strLabel &= "<b>Height:</b> " & l.Height & " "
                strLabel &= "<b>Width:</b> " & l.Width
                strLabel &= "<br/ >"
                strLabel &= "<b>Location:</b> " & l.Location & "<br/ >"

                If l.MakeOrOpenSizesId = 2 Then
                    strLabel &= "<b>Make/Opening:</b> Opening Size<br/ >"
                Else
                    strLabel &= "<b>Make/Opening:</b> Make Size<br/ >"
                End If

                strLabel &= "<b>No of Panels:</b> " & l.NoOfPanels & "<br/ >"
                strLabel &= "<br/ >"

                ' Special requirements
                If Not String.IsNullOrEmpty(l.SpecialRequirements) Then
                    strLabel &= "<br/ >"
                    strLabel &= "Special Requirements:" & l.SpecialRequirements
                End If

                ' Check that production sheet exists - if not display message that it needs to be done manually for now
                Dim cSpec As LouvreSpecDesign = service.GetLouvreSpecDesignByLouvreDetailsID(l.LouvreDetailID)

                Dim intWideRail As Boolean = cSpec.WideRail

                If Not ProductionSheetExistsForOpening(l.MakeOrOpenSizesId, l.ShutterTypeId) Then

                    strLabel &= "<br/ ><br />"
                    strLabel &= "<b><span style='color:red;'>PRODUCTION SHEET NOT CURRENTLY AVAILABLE FOR MAKE SIZE</span></b>"

                End If

                strLabel &= "</td>"
                strLabel &= "</tr>"
            Next l

            strLabel &= "</table>"
        End If

        lblOpenings.Text = strLabel

    End Sub
    Protected Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        Response.Redirect("Home.aspx")
    End Sub
    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        Response.Redirect("Logout.aspx", False)
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("UpdateAwaitingAcceptance.aspx", False)
    End Sub

    Private Sub btnProcessProdSheet_Click(sender As Object, e As EventArgs) Handles btnProcessProdSheet.Click
        Dim strErrors As String = String.Empty
        Dim cProdSchedule As ProductionSchedule = SaveProductionSchedule(strErrors)

        lblErrorMsg.Text = String.Empty

        If cProdSchedule IsNot Nothing AndAlso cProdSchedule.ID > 0 Then
            ' Return a response if the production schedule was saved, or was just loaded where a save was not required. If not here, something PS related went wrong.
            PDFResponse()
        Else
            lblErrorMsg.Text = "ERROR : " & strErrors
        End If
    End Sub

    Private Function SaveProductionSchedule(ByRef strErrorsOut As String) As ProductionSchedule

        strErrorsOut = String.Empty

        Dim cService As New AppService
        Dim dbConn As New DBConnection
        Dim cnn As SqlClient.SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim trans As SqlClient.SqlTransaction = Nothing
        Dim cProductionSchedule As ProductionSchedule = cService.GetProdScheduleClsByID(txtID.Text, cnn)
        Dim cNewProductionSchedule As ProductionSchedule = Nothing

        Dim bolContinue As Boolean = True

        If cProductionSchedule.ID > 0 Then
            Dim cRules As New RulesLouvreDetails

            cNewProductionSchedule = cProductionSchedule.Clone

            cRules.CacheExtraProductLouvres.CacheAllExtras(cService)

            ' Only update the schedule and push to Sybiz if AA status.
            If cProductionSchedule.OrderStatus = SharedEnums.ProductionScheduleStatus.AwaitingAcceptance Then

                Try
                    cnn.Open()
                    trans = cnn.BeginTransaction

                    'update orderstatus to accepted plus set received date
                    cNewProductionSchedule.OrderStatus = SharedEnums.ProductionScheduleStatus.PaperworkProcessing
                    cNewProductionSchedule.ReceivedDate = Date.Now.Date

                    If bolContinue Then
                        bolContinue = cService.UpdateProductionScheduleByID(cNewProductionSchedule, cnn, trans)
                    End If

                    If bolContinue Then
                        bolContinue = cService.AddProdScheduleHistoryRcd(0, cProductionSchedule, cnn, trans)
                    End If

                    If bolContinue Then
                        Dim cNewProdScheduleContainer As New ProductionScheduleContainerLouvres

                        With cNewProdScheduleContainer
                            .ProductionSchedule = cNewProductionSchedule
                            .LouvreDetails = cService.GetLouvreDetailsCollectionByProductionScheduleID(cNewProductionSchedule.ID)
                            .LouvreSpecs = cService.GetLouvreSpecsByProductionScheduleID(cNewProductionSchedule.ID)
                            .Notes = cService.GetProdScheduleNotesByProductionScheduleID(cNewProductionSchedule.ID)
                        End With

                        bolContinue = SyBizShared.AddJobToSybizIfNeeded(cProductionSchedule, cNewProdScheduleContainer, strErrorsOut, cnn, trans)
                    End If

                    If bolContinue Then
                        trans.Commit()
                    Else
                        EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - Transaction Rolled Back" & Environment.NewLine & SharedFunctions.GetPageInfo(form1, Session))
                        trans.Rollback()
                        cNewProductionSchedule = Nothing
                    End If

                Catch ex As Exception

                    cNewProductionSchedule = Nothing

                    If Not trans Is Nothing Then
                        trans.Rollback()
                    End If

                    If cnn.State = ConnectionState.Open Then
                        cnn.Close()
                    End If

                    EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message & Environment.NewLine & SharedFunctions.GetPageInfo(form1, Session))
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
            End If
        End If

        Return cNewProductionSchedule
    End Function

    Private Sub PDFResponse()
        Dim cPDF As New ProductionScheduleGenerator
        Dim cResult As PDFGenerationResult = cPDF.GeneratePDF(CInt(txtID.Text))

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("Content-Disposition", "attachment;filename=" & cResult.FileName)
        Response.BinaryWrite(cResult.PDFBytes)
        Response.End()
    End Sub

    Protected Function SetLouvreSpecDetailsData(intScheduleID As Integer,
                                                cRules As RulesLouvreDetails,
                                                cnn As SqlClient.SqlConnection,
                                                ByRef trans As SqlClient.SqlTransaction) As Boolean

        Dim service As New AppService
        Dim bolContinue As Boolean = True
        Dim cLouvreDetails As New LouvreDetailsCollection

        cLouvreDetails = service.GetLouvreDetailsCollectionByProductionScheduleID(intScheduleID)

        For Each l As LouvreDetails In cLouvreDetails
            Dim cSpec As LouvreSpecDesign = service.GetLouvreSpecDesignByLouvreDetailsID(l.LouvreDetailID)

            If cSpec.LouvreDetailsID > 0 Then
                cSpec = SetSpecDesignDataForOpening(l, cRules, cSpec)
                bolContinue = service.UpdateLouvreSpecDesign(cSpec, cnn, trans)
            Else
                cSpec = SetSpecDesignDataForOpening(l, cRules, cSpec)
                bolContinue = service.AddLouvreSpecDesign(cSpec, cnn, trans)
            End If

            If bolContinue = False Then
                Exit For
            End If
        Next l

        Return bolContinue
    End Function

    Protected Function SetSpecDesignDataForOpening(cLouvreDetails As LouvreDetails,
                                                   cRules As RulesLouvreDetails,
                                                   cLouvreDesign As LouvreSpecDesign) As LouvreSpecDesign

        Dim cService As New AppService

        With cLouvreDesign
            .LouvreDetailsID = cLouvreDetails.LouvreDetailID
            .OpeningHeight = cLouvreDetails.Height
            .OpeningWidth = cLouvreDetails.Width
            .NoOfPanels = cLouvreDetails.NoOfPanels
            .PileColour = cLouvreDetails.PileColourId
            .BladeLockQuantity = cRules.SpecBladeLockTotalQty(cLouvreDetails)

            .TopFlushBoltsType = cLouvreDetails.FlushBoltsTopId
            .TopFlushBoltsQuantity = cRules.SpecFlushBoltTopQty(cLouvreDetails)
            .BottomFlushBoltsType = cLouvreDetails.FlushBoltsBottomId
            .BottomFlushBoltsQuantity = cRules.SpecFlushBoltBottomQty(cLouvreDetails)

            ' BTODO: not used anymore?
            .WideRail = 0

            .NoOfTransom = Math.Abs(Int(cLouvreDetails.PanelMidRailType > 0))

            .GivenTransomHeight = cLouvreDetails.MidRailHeight
            .PanelTransomHeight = cRules.SpecPanelMidRailHeightMinusOpeningDeductions(cLouvreDetails)

            Dim cBladeQty As Rules.Louvres.BladeQty = cRules.SpecBladeQtyTopPanelSection(cLouvreDetails, False)

            .BottomFirstBladeHoleStartPoint = cBladeQty.FirstBladeHole
            .TopInfillBottomSize = cRules.CacheExtraProductLouvres.ItemByID(cBladeQty.TopInfillType).DeductionWidth
            .BottomInfillBottomSize = cRules.CacheExtraProductLouvres.ItemByID(cBladeQty.BottomInfillType).DeductionWidth
            .ReadyToGenerate = Date.Now
            .GenerationComplete = SharedConstants.MIN_DATE

        End With

        Return cLouvreDesign
    End Function

    Private Sub btnViewDetails_Click(sender As Object, e As EventArgs) Handles btnViewDetails.Click
        Dim service As New AppService
        Dim cLouvreDetails As LouvreDetails = service.GetLouvreDetailsByLouvreDetailID(txtHiddenLouvreDetail.Text)

        With cLouvreDetails
            lblLocation.Text = .Location
            lblColour.Text = .Colour
            lblHeight.Text = .Height
            lblWidth.Text = .Width

            If .MakeOrOpenSizesId = SharedEnums.MakeOpen.Make Then
                lblMakeOpening.Text = "Make Sizes"
            ElseIf .MakeOrOpenSizesId = SharedEnums.MakeOpen.Opening Then
                lblMakeOpening.Text = "Opening Sizes"
            Else
                lblMakeOpening.Text = String.Empty
            End If

            lblProduct.Text = .Product
            lblShutterType.Text = .ShutterType
            lblOpenInOut.Text = .BiFoldHingedDoorInOut
            lblNoOfPanels.Text = .NoOfPanels
            lblBladeSize.Text = .BladeSize
            lblEndPlugColour.Text = .EndCapColour
            lblBladeClipColour.Text = .BladeClipColour
            lblPileColour.Text = .PileColour
            lblTopTrackType.Text = .TopTrack
            lblBottomTrackType.Text = .BottomTrackType
            lblCurvedTrack.Text = .CurvedTrack.ToString
            lblExtraTrack.Text = .ExtraTrack
            lblMidRailHeight.Text = .MidRailHeight
            lblFlushBoltsTop.Text = .FlushBoltsTop
            lblFlushBoltsBottom.Text = .FlushBoltsBottom
            lblSpecialLockOptions.Text = .LockOptions
            lblBladeLocks.Text = .BladeLocks.ToString
            lblFixedPanelChannelTAndB.Text = .CChannel.ToString
            lblFixedPanelChannel.Text = .FixedPanelChannel
            lblHJoiner.Text = .HChannel.ToString

            If .LChannel = SharedEnums.LReveal.Reveal3Sided Then
                lblLReveal.Text = "Reveal 3 Sided"
            ElseIf .LChannel = SharedEnums.LReveal.Reveal4Sided Then
                lblLReveal.Text = "Reveal 4 Sided"
            ElseIf .LChannel = SharedEnums.LReveal.Facefit3Sided Then
                lblLReveal.Text = "Facefit 3 Sided"
            ElseIf .LChannel = SharedEnums.LReveal.Facefit4Sided Then
                lblLReveal.Text = "Facefit 4 Sided"
            Else
                lblLReveal.Text = String.Empty
            End If

            If .ZChannel = SharedEnums.ZReveal.Reveal3Sided Then
                lblZReveal.Text = "Reveal 3 Sided"
            ElseIf .ZChannel = SharedEnums.ZReveal.Reveal4Sided Then
                lblZReveal.Text = "Reveal 4 Sided"
            Else
                lblZReveal.Text = String.Empty
            End If

            lblTopOperation.Text = .BladeOperation
            lblBottomOperation.Text = .BladeOperationBottom
            lblTopInsert.Text = .InsertTop
            lblBottomInsert.Text = .InsertBottom

            lblFlyScreen.Text = cLouvreDetails.FlyScreen.ToString

            lblWinder.Text = .Winder.ToString
            lblSlide.Text = .Slide
            lblStackerLocation.Text = .StackerLocation
            lblSpecialRequirements.Text = .SpecialRequirements

            If lblHeight.Text <> String.Empty Then
                lblHeight.Text = lblHeight.Text + " (mm)"
            End If

            If lblWidth.Text <> String.Empty Then
                lblWidth.Text = lblWidth.Text + " (mm)"
            End If

            If lblExtraTrack.Text <> String.Empty Then
                lblExtraTrack.Text = lblExtraTrack.Text + " (mm)"
            End If

            If lblMidRailHeight.Text <> String.Empty Then
                lblMidRailHeight.Text = lblMidRailHeight.Text + " (mm)"
            End If

        End With

        ModalPopupExtender2.Show()

    End Sub

    Protected Function ProductionSheetExistsForOpening(enumMakeOpen As SharedEnums.MakeOpen, enumShutterTypeID As SharedEnums.LouvreTypes) As Boolean

        If enumMakeOpen = SharedEnums.MakeOpen.Opening Then
            Return True
        ElseIf enumShutterTypeID = SharedEnums.LouvreTypes.PrivacyScreen Then
            Return True
        End If

        Return False
    End Function

End Class
