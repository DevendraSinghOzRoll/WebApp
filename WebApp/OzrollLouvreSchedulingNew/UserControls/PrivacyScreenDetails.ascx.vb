Imports OzrollPSLVSchedulingModel.SharedEnums

Partial Class UserControls_PrivacyScreenDetails
    Inherits System.Web.UI.UserControl

    Public Event SaveDetails(intPSDetailID As Integer)
    Public Event CancelDetails(intPSDetailID As Integer)

    Private _lLouvreDetails As LouvreDetailsCollection = Nothing
    Private _RulesLouvreDetails As New RulesLouvreDetails
    Private _PageIsReadOnly As Boolean = False

    Public Sub Show(Optional boolReadOnly As Boolean = False)

        _PageIsReadOnly = boolReadOnly

        If boolReadOnly Then
            LockPage()
        End If

        mpePrivacyScreenDetails.Show()
    End Sub

    Public Sub Hide()
        mpePrivacyScreenDetails.Hide()
    End Sub

    Public Sub ShowAddPrivacyScreen(lLouvreDetails As LouvreDetailsCollection)
        Dim cNewDetails As New LouvreDetails

        _lLouvreDetails = lLouvreDetails

        ' Add new louvre details with unique negative ID.
        cNewDetails = lLouvreDetails.Add(cNewDetails, True)

        ResetPopupControlsToDefault()
        txtHiddenPSDetailID.Text = cNewDetails.LouvreDetailID

        ' Hard coded values need to be set straight away so we can tell this is privacy screens out in the main page.
        cNewDetails.ShutterTypeId = LouvreTypes.PrivacyScreen
        cNewDetails.ProductId = LouvreStyles.PrivacyScreen
        cNewDetails.Product = "Privacy Screen"

        ShowHideFixingQty()

        mpePrivacyScreenDetails.Show()
    End Sub

    Private Sub ResetPopupControlsToDefault()

        txtLocation.Text = String.Empty
        txtPrivacyScreenColour.Text = String.Empty
        hdnPrivacyScreenColourID.Value = 0
        hdnPrivacyScreenColourName.Value = String.Empty
        txtHeight.Text = String.Empty
        txtWidth.Text = String.Empty
        ddlNoOfPanels.SelectedIndex = 0
        ddlBladeSize.SelectedValue = 3
        ddlFixing.SelectedValue = 0
        ddlFixingQty.SelectedValue = 4
        ddlFixingLocation.SelectedValue = 0
        txtSpecialRequirements.Text = String.Empty

        _RulesLouvreDetails.PopulateNoOfPanelsDDL(Nothing, ddlNoOfPanels, MakeOpen.NONE, LouvreTypes.NONE, 0, 0)

    End Sub

    Public Sub ShowEditPrivacyScreen(lLouvreDetails As LouvreDetailsCollection, intPSDetailID As Integer, Optional boolReadOnly As Boolean = False)

        _PageIsReadOnly = boolReadOnly

        _lLouvreDetails = lLouvreDetails

        Dim cQuoteDetail As LouvreDetails = _lLouvreDetails.Find(Function(x) x.LouvreDetailID = intPSDetailID)

        If cQuoteDetail.LouvreDetailID <> 0 Then
            txtHiddenPSDetailID.Text = intPSDetailID.ToString

            txtLocation.Text = cQuoteDetail.Location

            If cQuoteDetail.Width > 0 Then
                txtWidth.Text = CInt(cQuoteDetail.Width)
            Else
                txtWidth.Text = String.Empty
            End If

            If cQuoteDetail.Height > 0 Then
                txtHeight.Text = CInt(cQuoteDetail.Height)
            Else
                txtHeight.Text = String.Empty
            End If

            ddlBladeSize.SelectedValue = cQuoteDetail.BladeSizeId
            txtLocation.Text = cQuoteDetail.Location

            If cQuoteDetail.ColourId > 0 Then
                hdnPrivacyScreenColourID.Value = cQuoteDetail.ColourId
                hdnPrivacyScreenColourName.Value = cQuoteDetail.Colour
                txtPrivacyScreenColour.Text = cQuoteDetail.Colour
                txtPrivacyScreenColour.ToolTip = txtPrivacyScreenColour.Text
            Else
                hdnPrivacyScreenColourID.Value = 0
                hdnPrivacyScreenColourName.Value = String.Empty
                txtPrivacyScreenColour.Text = String.Empty
                txtPrivacyScreenColour.ToolTip = txtPrivacyScreenColour.Text
            End If

            txtSpecialRequirements.Text = cQuoteDetail.SpecialRequirements
            ddlFixing.SelectedValue = cQuoteDetail.FixedPanelChannelID
            ddlFixingQty.SelectedValue = cQuoteDetail.PanelFixingQty
            ddlFixingLocation.SelectedValue = cQuoteDetail.PanelFixingLocation

            ' Need to populate values as they may be set to the previously loaded details panel number list.
            _RulesLouvreDetails.PopulateNoOfPanelsDDL(cQuoteDetail, ddlNoOfPanels, MakeOpen.Make, LouvreTypes.PrivacyScreen, txtWidth.Text, 0)

            If Not cQuoteDetail.NoOfPanels = SharedConstants.DEFAULT_INTEGER_VALUE Then
                ddlNoOfPanels.SelectedValue = cQuoteDetail.NoOfPanels
            Else
                ddlNoOfPanels.SelectedIndex = 0
            End If

            mpePrivacyScreenDetails.Show()
        End If

        ShowHideFixingQty()

        If boolReadOnly Then
            LockPage()
        End If

    End Sub

    Public Function PopulateLouvreDetailsFromUI() As LouvreDetails
        Return (New LouvreDetails)
    End Function

    Public Function PopulateLouvreDetailsFromUI(cLouvreDetails As LouvreDetails) As LouvreDetails

        With cLouvreDetails

            ' Values from UI
            .Location = txtLocation.Text.Trim()
            .ColourId = hdnPrivacyScreenColourID.Value
            .Colour = hdnPrivacyScreenColourName.Value
            .Height = txtHeight.Text.Trim()
            .Width = txtWidth.Text.Trim()
            .NoOfPanels = ddlNoOfPanels.SelectedValue
            .BladeSizeId = ddlBladeSize.SelectedValue
            .BladeSize = ddlBladeSize.SelectedItem.Text
            .FixedPanelChannelID = ddlFixing.SelectedValue
            .FixedPanelChannel = ddlFixing.SelectedItem.Text

            If .FixedPanelChannelID > 0 Then
                .PanelFixingQty = ddlFixingQty.SelectedValue
            Else
                .PanelFixingQty = 0
            End If

            If .FixedPanelChannelID = .FixedPanelChannelID = PanelFixing.Angle40x20 Then
                .PanelFixingLocation = ddlFixingLocation.SelectedValue
            Else
                .PanelFixingLocation = 0
            End If

            .SpecialRequirements = txtSpecialRequirements.Text.Trim()

            ' Hard coded values
            .ShutterTypeId = LouvreTypes.PrivacyScreen
            .ShutterType = "Privacy Screen"
            .ProductId = LouvreStyles.PrivacyScreen
            .Product = "Privacy Screen"
            .MakeOrOpenSizesId = MakeOpen.Make
            .MakeOrOpenSizes = "Make"
            .InsertTopId = InsertTypes.HorizontalBlade
            .InsertTop = "Horizontal Blade"

        End With

        Return cLouvreDetails
    End Function

    Private Sub txtWidth_TextChanged(sender As Object, e As EventArgs) Handles txtWidth.TextChanged
        Dim intWidth As Integer = 0
        Dim cLouvreDetails As LouvreDetails = PopulateLouvreDetailsFromUI()

        If IsNumeric(txtWidth.Text) Then
            intWidth = txtWidth.Text
        End If

        If String.IsNullOrEmpty(ddlNoOfPanels.SelectedValue) Then
            _RulesLouvreDetails.PopulateNoOfPanelsDDL(cLouvreDetails, ddlNoOfPanels, MakeOpen.Make, LouvreTypes.PrivacyScreen, intWidth, 0)
        Else
            _RulesLouvreDetails.PopulateNoOfPanelsDDL(cLouvreDetails, ddlNoOfPanels, MakeOpen.Make, LouvreTypes.PrivacyScreen, intWidth, ddlNoOfPanels.SelectedValue)
        End If
    End Sub

    Private Sub btnSaveDetails_Click(sender As Object, e As EventArgs) Handles btnSaveDetails.Click

        Page.Validate("privacyScreenDetails")

        If Page.IsValid Then
            ' Readonly check just incase user somehow gets access to button. Should not happen.
            If Not _PageIsReadOnly Then
                RaiseEvent SaveDetails(txtHiddenPSDetailID.Text)
            Else
                Show()
            End If
        Else
            Show()
        End If
    End Sub

    Private Sub btnCancelDetails_Click(sender As Object, e As EventArgs) Handles btnCancelDetails.Click
        RaiseEvent CancelDetails(txtHiddenPSDetailID.Text)
    End Sub

    Private Sub UserControls_PrivacyScreenDetails_Load(sender As Object, e As EventArgs) Handles Me.Load

        SetToolTips()

        ' Register all the controls used in javascript so they can be accessed by external js files.
        Dim strScript As String = "<script type=text/javascript> var std = '" & SharedConstants.STR_STANDARD_COLOUR_TAG & "'; " &
                                  "var prem = '" & SharedConstants.STR_PREMIUM_COLOUR_TAG & "'; " &
                                  "var prest = '" & SharedConstants.STR_PRESTIGE_COLOUR_TAG & "'; " &
                                  "var ctrlPSColourID = '" & hdnPrivacyScreenColourID.ClientID & "'; " &
                                  "var ctrlPSColourName = '" & hdnPrivacyScreenColourName.ClientID & "'; " &
                                  "var ctrlPSColour = '" & txtPrivacyScreenColour.ClientID & "'; </script>"

        Page.ClientScript.RegisterClientScriptBlock(GetType(Page), "SetPrivacyScreenVars", strScript)

        ' Register external js file
        Page.ClientScript.RegisterClientScriptInclude("PrivacyScreenDetails", "UserControls/PrivacyScreenDetails.js")

    End Sub

    Private Sub SetToolTips()

        imgColour.ToolTip = "The colour of the Privacy Screen."
        imgHeight.ToolTip = "The height of the Privacy Screen."
        imgWidth.ToolTip = "The width of the Privacy Screen."
        imgNoOfPanels.ToolTip = "The number of Privacy Screens."
        imgBladeSize.ToolTip = "The blade size to be used within the Privacy Screen"
        imgFixing.ToolTip = "The type of fixing (if any) to be used with this Privacy Screen."
        imgFixingQty.ToolTip = "The quantity of fixings required for this Privacy Screen."
        imgFixingLocation.ToolTip = "The sides that the fixing is to be attached to."
        imgSpecialRequirements.ToolTip = "Type any special requirements or notes to go with this Privacy Screen here."
    End Sub

    Private Sub ShowHideFixingQty()
        pnlFixingQty.Visible = (ddlFixing.SelectedValue > 0)

        If (ddlFixing.SelectedValue = PanelFixing.Angle40x20) Then

            ' Chanels are hard set to 2.
            ddlFixingQty.SelectedValue = 2
            ddlFixingQty.Enabled = False
        Else
            ddlFixingQty.Enabled = IIf(_PageIsReadOnly, False, True)
        End If

        ShowHideFixingLocation()
    End Sub

    Private Sub ShowHideFixingLocation()
        pnlFixingLocation.Visible = (ddlFixing.SelectedValue = PanelFixing.Angle40x20)
    End Sub

    Private Sub ddlFixing_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFixing.SelectedIndexChanged
        ShowHideFixingQty()
    End Sub

    ''' <summary>
    ''' Checks the given original louvre details against the current UI state for relevant changes.
    ''' </summary>
    ''' <param name="cOriginalLouvreDetails">The original state of the louvre details to compare against.</param>
    ''' <returns>True if a change is found, otherwise false.</returns>
    Public Function DetailsNeedCostRecalculation(cOriginalLouvreDetails As LouvreDetails) As Boolean
        Dim service As New AppService

        ' CHECKING FOR CHANGES TO:
        ' * Colour
        ' * Width
        ' * Height
        ' * FixedPanelChannelID - (Panel fixing for privacy screens)
        ' * Panel Fixing Qty
        ' * Panel Fixing Location

        ' Colour change Privacy Screens
        If IsNumeric(hdnPrivacyScreenColourID.Value) AndAlso hdnPrivacyScreenColourID.Value > 0 Then
            ' Only recalculate the price if the colour has changed grade (std to premium, etc).
            Dim colours As List(Of Colour) = service.getColours().FindAll(Function(x) x.Discontinued = False)

            ' get original value
            Dim newColour As Colour = colours.Find(Function(x) x.ID = hdnPrivacyScreenColourID.Value)
            Dim oldColour As Colour = colours.Find(Function(x) x.ID = cOriginalLouvreDetails.ColourId)

            If newColour IsNot Nothing AndAlso oldColour IsNot Nothing Then
                If newColour.CoatingTypeID <> oldColour.CoatingTypeID Then
                    Return True
                End If
            Else
                Return True
            End If
        Else
            ' No value so recalculate
            If cOriginalLouvreDetails.ColourId > 0 Then
                Return True
            End If
        End If

        ' Width
        If txtWidth.Text <> cOriginalLouvreDetails.Width Then
            Return True
        End If

        ' Height
        If txtHeight.Text <> cOriginalLouvreDetails.Height Then
            Return True
        End If

        ' Panel Fixing
        If ddlFixing.SelectedValue <> cOriginalLouvreDetails.FixedPanelChannelID Then
            Return True
        End If

        ' Panel Fixing Qty
        If ddlFixingQty.SelectedValue <> cOriginalLouvreDetails.PanelFixingQty Then
            Return True
        End If

        ' Panel Fixing Location
        If ddlFixingLocation.SelectedValue <> cOriginalLouvreDetails.PanelFixingLocation Then
            Return True
        End If

        Return False
    End Function

    Private Sub LockPage()

        ' Location
        RemoveControlsFormClasses(txtLocation)
        txtLocation.Enabled = False
        txtLocation.CssClass &= "form-field-disabled"

        'Colour
        RemoveControlsFormClasses(txtPrivacyScreenColour)
        txtPrivacyScreenColour.Enabled = False
        txtPrivacyScreenColour.CssClass &= "form-field-disabled"

        ' Height
        RemoveControlsFormClasses(txtHeight)
        txtHeight.Enabled = False
        txtHeight.CssClass &= "form-field-disabled"

        ' Width
        RemoveControlsFormClasses(txtWidth)
        txtWidth.Enabled = False
        txtWidth.CssClass &= "form-field-disabled"

        ' No of Panels
        RemoveControlsFormClasses(ddlNoOfPanels)
        ddlNoOfPanels.Enabled = False
        ddlNoOfPanels.CssClass &= "form-select-disabled"

        ' Blade Size
        RemoveControlsFormClasses(ddlBladeSize)
        ddlBladeSize.Enabled = False
        ddlBladeSize.CssClass &= "form-select-disabled"

        ' Fixing
        RemoveControlsFormClasses(ddlFixing)
        ddlFixing.Enabled = False
        ddlFixing.CssClass &= "form-select-disabled"

        ' Fixing Qty
        RemoveControlsFormClasses(ddlFixingQty)
        ddlFixingQty.Enabled = False
        ddlFixingQty.CssClass &= "form-select-disabled"

        ' Fixing Location
        RemoveControlsFormClasses(ddlFixingLocation)
        ddlFixingLocation.Enabled = False
        ddlFixingLocation.CssClass &= "form-select-disabled"

        ' Special Requirements
        txtSpecialRequirements.Enabled = False

        ' Save Button
        btnSaveDetails.Enabled = False

    End Sub

    Private Sub RemoveControlsFormClasses(cControl As Object)
        If TypeOf cControl Is DropDownList Then
            Dim ddl As DropDownList = cControl

            ddl.CssClass = ddl.CssClass.Replace("form-select-disabled", String.Empty)
            ddl.CssClass = ddl.CssClass.Replace("form-select", String.Empty)

        ElseIf TypeOf cControl Is TextBox Then
            Dim tb As TextBox = cControl

            tb.CssClass = tb.CssClass.Replace("form-field-disabled", String.Empty)
            tb.CssClass = tb.CssClass.Replace("form-field", String.Empty)

        ElseIf TypeOf cControl Is Button Then
            Dim tb As Button = cControl

            tb.CssClass = tb.CssClass.Replace("form-button-disabled", String.Empty)
            tb.CssClass = tb.CssClass.Replace("form-button", String.Empty)
        End If
    End Sub

End Class
