<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PrivacyScreenDetails.ascx.vb" Inherits="UserControls_PrivacyScreenDetails" %>
<%@ Register TagPrefix="ext" Namespace="Extensions" %>

<asp:HiddenField ID="hiddenAddPrivacyScreenDummy" runat="server" />
<ajaxcontroltoolkit:ModalPopupExtender ID="mpePrivacyScreenDetails" runat="server" BehaviorID="modal666" TargetControlID="hiddenAddPrivacyScreenDummy" PopupControlID="pnlPrivacyScreenDetails" BackgroundCssClass="modalBackground">
</ajaxcontroltoolkit:ModalPopupExtender>

<asp:Panel ID="pnlPrivacyScreenDetails" runat="server" CssClass="modalPopup-privacyscreendetails" Style="display: none; max-height: 95%;" >
    <asp:UpdatePanel ID="upnlPrivacyScreenDetails" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

            <asp:TextBox ID="txtHiddenPSDetailID" runat="server" Visible="false" Text="0"></asp:TextBox>

            <br />
            <br />

            <h2>Privacy Screen</h2>

            <table class="form-table" summary="">

                <tr class="no-wrap">
                    <td class="form-field-td-p2" style="width: 40%; text-align: right;">
                        <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>
                    </td>

                    <td class="form-field-td-p2" style="width: 60%;">
                        <asp:TextBox ID="txtLocation" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="100" TabIndex="1" usesubmitbehavior="false" Width="200px" />
                        <asp:RequiredFieldValidator ID="valrfLocation" runat="server" ControlToValidate="txtLocation" CssClass="validation-text"
                            ErrorMessage="<br />Please enter a Location." ValidationGroup="privacyScreenDetails" Display="Dynamic" EnableClientScript="false" />
                    </td>
                </tr>

                <tr class="no-wrap">
                    <td class="form-field-td-p2" style="width: 40%; text-align: right;">Colour</td>
                    <td class="form-field-td-p2" style="width: 60%;">
                                                
                        <asp:TextBox ID="txtPrivacyScreenColour" runat="server" CssClass="form-field" TabIndex="2" Width="200px" 
                                        onfocus="this.select();" onblur="AutoCompletePrivacyScreenEnsureValues()"  />
                        <asp:Image ID="imgColour" runat="server" Height="16px" ImageUrl="~/images/info_button_icon16x16_t35.png" Width="16px" />

                        <ajaxcontroltoolkit:AutoCompleteExtender
                                        ID="acePrivacyScreenColour"
                                        runat="server"
                                        TargetControlID="txtPrivacyScreenColour"
                                        ServicePath="AutoComplete.asmx"
                                        ServiceMethod="GetMatchingColourList"
                                        MinimumPrefixLength="1"
                                        CompletionInterval="1000"
                                        CompletionSetCount="0"
                                        EnableCaching="false"
                                        CompletionListCssClass="auto-complete-list"
                                        CompletionListHighlightedItemCssClass="auto-complete-higlighted-item"
                                        CompletionListItemCssClass="auto-complete-item"
                                        OnClientItemSelected="AutoCompletePrivacyScreenGetColourID"
                                        OnClientPopulating="AutoCompletePrivacyScreenStart"
                                        OnClientPopulated="AutoCompletePrivacyScreenEnd"
                                        OnClientHiding="AutoCompletePrivacyScreenEnd"
                                        FirstRowSelected="true"></ajaxcontroltoolkit:AutoCompleteExtender>

                                    <ext:HiddenFieldExtended ID="hdnPrivacyScreenColourID" runat="server" />
                                    <ext:HiddenFieldExtended ID="hdnPrivacyScreenColourName" runat="server" />

                        <asp:RequiredFieldValidator ID="valrfColour" runat="server" ControlToValidate="hdnPrivacyScreenColourID" CssClass="validation-text"
                            ErrorMessage="<br />Please select a Colour." InitialValue="0" ValidationGroup="privacyScreenDetails" Display="Dynamic" EnableClientScript="false" />
                                              
                    </td>
                </tr>

                <tr class="no-wrap">
                    <td class="form-field-td-p2" style="width: 40%; text-align: right;">Height</td>
                    <td class="form-field-td-p2" style="width: 60%;">
                                                
                        <asp:TextBox ID="txtHeight" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="4" TabIndex="3" usesubmitbehavior="false" Width="95px" /> (mm)
                        <asp:Image ID="imgHeight" runat="server" Height="16px" ImageUrl="~/images/info_button_icon16x16_t35.png" Width="16px" />
                        <asp:RequiredFieldValidator ID="valrfHeight" runat="server" ControlToValidate="txtHeight" CssClass="validation-text"
                            ErrorMessage="<br />Height is required." ValidationGroup="privacyScreenDetails" Display="Dynamic" EnableClientScript="false" />
                        <asp:CompareValidator ID="valcompHeight" runat="server" Operator="DataTypeCheck" Type="Integer" CssClass="validation-text"
                            ControlToValidate="txtHeight" ErrorMessage="<br />Height is invalid." ValidationGroup="privacyScreenDetails" Display="Dynamic" EnableClientScript="false" />
                        <asp:RangeValidator ID="valrangeHeight" runat="server" Type="Double" CssClass="validation-text"
                            MinimumValue="1" MaximumValue="9999" ControlToValidate="txtHeight" ValidationGroup="privacyScreenDetails"
                            ErrorMessage="<br />Please enter a valid height." Display="Dynamic" EnableClientScript="false" />
                        <asp:CustomValidator ID="valcustPanelHeight" runat="server"
                                Display="Dynamic" Text="" 
                                ControlToValidate="txtHeight"
                                ValidationGroup="privacyScreenDetails"
                                EnableClientScript="false"
                                CssClass="validation-text" />
                                              
                    </td>
                </tr>

                <tr class="no-wrap">
                    <td class="form-field-td-p2" style="width: 40%; text-align: right;">Width</td>
                    <td class="form-field-td-p2" style="width: 60%;">
                                                
                        <asp:TextBox ID="txtWidth" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="6" TabIndex="4" usesubmitbehavior="false" Width="95px"  /> (mm)
                        <asp:Image ID="imgWidth" runat="server" Height="16px" ImageUrl="~/images/info_button_icon16x16_t35.png" Width="16px" />
                        <asp:RequiredFieldValidator ID="valreqWidth" runat="server" ControlToValidate="txtWidth" CssClass="validation-text"
                            ErrorMessage="<br />Width is required." ValidationGroup="privacyScreenDetails" Display="Dynamic" EnableClientScript="false" />
                        <asp:CompareValidator ID="valcompWidth" runat="server" Operator="DataTypeCheck" Type="Integer" CssClass="validation-text"
                            ControlToValidate="txtWidth" ErrorMessage="<br />Width is invalid." ValidationGroup="privacyScreenDetails" Display="Dynamic" EnableClientScript="false" />
                        <asp:RangeValidator ID="valrangeWidth" runat="server" Type="Double" CssClass="validation-text"
                            MinimumValue="1" MaximumValue="999999" ControlToValidate="txtWidth" ValidationGroup="privacyScreenDetails"
                            ErrorMessage="<br />Please enter a valid width." Display="Dynamic" EnableClientScript="false" />
                        <asp:CustomValidator ID="valcustPanelWidth" runat="server" 
                            Display="Dynamic" Text="" 
                            ValidationGroup="privacyScreenDetails"
                            ControlToValidate="txtWidth"
                            EnableClientScript="false"
                            CssClass="validation-text" />
                                              
                    </td>
                </tr>

                <tr class="no-wrap">
                    <td class="form-field-td-p2" style="width: 40%; text-align: right;">No of Panels</td>
                    <td class="form-field-td-p2" style="width: 60%;">
                                                
                        <asp:DropDownList ID="ddlNoOfPanels" runat="server" AutoPostBack="true" CssClass="form-select" TabIndex="5" Width="150px">
                            <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Image ID="imgNoOfPanels" runat="server" Height="16px" ImageUrl="~/images/info_button_icon16x16_t35.png" Width="16px" />
                                            
                    </td>
                </tr>

                <tr class="no-wrap">
                    <td class="form-field-td-p2" style="width: 40%; text-align: right;">Blade Size</td>
                    <td class="form-field-td-p2" style="width: 60%;">
                                                
                            <asp:DropDownList ID="ddlBladeSize" runat="server" AutoPostBack="true" CssClass="form-select" TabIndex="6" Width="150px">
                            <asp:ListItem Text="38" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Image ID="imgBladeSize" runat="server" Height="16px" ImageUrl="~/images/info_button_icon16x16_t35.png" Width="16px" />
                                            
                    </td>
                </tr>

                <tr>
                    <td class="form-label-td-p2" style="width: 40%; text-align: right;">Fixing</td>
                    <td class="form-field-td-p2" style="width: 60%;">
                                                
                        <asp:DropDownList ID="ddlFixing" runat="server" AutoPostBack="true" CssClass="form-select" TabIndex="7" Width="290px">
                            <asp:ListItem Text="None" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Standoff Foot 200mm" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Standoff Foot 500mm" Value="6"></asp:ListItem>
                            <asp:ListItem Text="40x20 Angle" Value="7"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Image ID="imgFixing" runat="server" Height="16px" ImageUrl="~/images/info_button_icon16x16_t35.png" Width="16px" />
                                            
                    </td>
                </tr>

                <asp:Panel ID="pnlFixingQty" runat="server">
                    <tr>
                        <td class="form-label-td-p2" style="width: 40%; text-align: right;">Fixing Quantity</td>
                        <td class="form-field-td-p2" style="width: 60%;">
                                                
                            <asp:DropDownList ID="ddlFixingQty" runat="server" AutoPostBack="true" CssClass="form-select" TabIndex="8" Width="150px">
                                <asp:ListItem Text="" Value="0"></asp:ListItem>
                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Image ID="imgFixingQty" runat="server" Height="16px" ImageUrl="~/images/info_button_icon16x16_t35.png" Width="16px" />
                            <asp:RequiredFieldValidator ID="valrfFixingQty" runat="server" ControlToValidate="ddlFixingQty" CssClass="validation-text"
                                ErrorMessage="<br />Please select Fixing Quantity." InitialValue="0" ValidationGroup="privacyScreenDetails" Display="Dynamic" EnableClientScript="false" />          
                        </td>
                    </tr>
                </asp:Panel>

                <asp:Panel ID="pnlFixingLocation" runat="server">
                    <tr>
                        <td class="form-label-td-p2" style="width: 40%; text-align: right;">Fixing Location</td>
                        <td class="form-field-td-p2" style="width: 60%;">
                                                
                            <asp:DropDownList ID="ddlFixingLocation" runat="server" AutoPostBack="true" CssClass="form-select" TabIndex="8" Width="150px">
                                <asp:ListItem Text="" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Left & Right" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Top & Bottom" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Image ID="imgFixingLocation" runat="server" Height="16px" ImageUrl="~/images/info_button_icon16x16_t35.png" Width="16px" />
                            <asp:RequiredFieldValidator ID="valrfFixingLocation" runat="server" ControlToValidate="ddlFixingLocation" CssClass="validation-text"
                                ErrorMessage="<br />Please select Fixing Location." InitialValue="0" ValidationGroup="privacyScreenDetails" Display="Dynamic" EnableClientScript="false" />            
                        </td>
                    </tr>
                </asp:Panel>

                <tr class="no-wrap">
                    <td class="form-label-td-p2" style="width: 40%; text-align: right;">Special Requirements</td>
                    <td class="form-field-td-p2" style="width: 60%; text-align: right;">
                                                
                            <asp:TextBox ID="txtSpecialRequirements" runat="server" CssClass="form-field" style="resize: vertical" TabIndex="9" TextMode="MultiLine"></asp:TextBox>
                            <asp:Image ID="imgSpecialRequirements" runat="server" Height="16px" ImageUrl="~/images/info_button_icon16x16_t35.png" Width="16px" />
                                                    
                    </td>
                </tr>

                <tr>
                    <td class="form-submit-td" colspan="2" style="text-align: center;">
                        <asp:ValidationSummary ID="valSummary" CssClass="valSummary" DisplayMode="BulletList" HeaderText="Please provide the missing information in the form above before saving the opening.<br /><br />" 
                        ForeColor="" ValidationGroup="privacyScreenDetails" runat="server"/>
                    </td>
                </tr>

                <tr>
                    <td class="form-submit-td" colspan="2" style="text-align: center;background-color:white;">
                        <asp:UpdatePanel ID="pnlupdateSaveCancel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                    <asp:Button ID="btnCancelDetails" runat="server" CssClass="form-button update-disable" Text="Cancel" UseSubmitBehavior="false" Width="100px" />&nbsp;&nbsp;
                                    <asp:Button ID="btnSaveDetails" runat="server" CssClass="form-button update-disable" ValidationGroup="privacyScreenDetails" Text="Save" UseSubmitBehavior="false" Width="100px" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="form-label-td-p2" colspan="2" style="text-align: center;background-color:white;">
                        <asp:Label ID="lblShutterStatus" runat="server" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <div class="hider">
                <div class="hider-loading">
                    <img src="Images/loading-gif-transparent-green.gif" alt="Loading..." width="100" height="100" />
                </div>
            </div>
                                    
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnSaveDetails" />
            <asp:PostBackTrigger ControlID="btnCancelDetails" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Panel>
