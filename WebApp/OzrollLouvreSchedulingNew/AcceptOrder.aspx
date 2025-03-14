﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AcceptOrder.aspx.vb" Inherits="AcceptOrder" Theme="SknGridView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxcontroltoolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Review and Accept Order</title>

    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta name="format-detection" content="telephone=no" />

    <link href="stylesheets/reset.css" rel="stylesheet" type="text/css" media="screen" />
    <!--[if IE]><link href="stylesheets/explorer.css" rel="stylesheet" type="text/css" media="screen" /><![endif]-->
    <link href="stylesheets/style.css?version=<%= Now.Year() & Now.Month & Now.Day() %>" rel="stylesheet" type="text/css" media="screen" />


    <link href="stylesheets/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <link href="stylesheets/smoothness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />

    <!--[if lt IE 8]><script src="javascript/IE-fix.js" type="text/javascript"></script><![endif]-->

    <script src="javascript/jquery-1.9.1.js" type="text/javascript"></script>

    <script src="javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>

    <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <script language="javascript" type="text/javascript">



        (function (document, navigator, standalone) {
            if ((standalone in navigator) && navigator[standalone]) {
                var curnode, location = document.location, stop = /^(a|html)$/i;
                document.addEventListener('click', function (e) {
                    curnode = e.target;
                    while (!(stop).test(curnode.nodeName))
                        curnode = curnode.parentNode;
                    if ('href' in curnode) {
                        e.preventDefault();
                        location.href = curnode.href;
                    }
                }, false);
            }
        })(document, window.navigator, 'standalone');

        $(function () {
            $("#dialog-alert").dialog({
                modal: true,
                autoOpen: false,
                draggable: false,
                buttons: {
                    "Ok": function () {
                        $(this).dialog("close");
                        return true;
                    }
                }
            });

            $("#dialog-confirm").dialog({
                modal: true,
                autoOpen: false,
                draggable: false,
                buttons: {
                    "Yes": function () {
                        $(this).dialog("close");
                        __doPostBack('<%=btnLogout.ClientID %>', '');
                        return true;
                    },
                    "No": function () {
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
        });


        if (window.history.forward(1) != null)
            window.history.forward(1);

        function ConfirmLeave() {
            document.getElementById("dialog-confirm-message").innerHTML = "Are you sure you want to logout?";
            $("#dialog-confirm").dialog("open");
            return false;
        }

        function saveLocation() {
            localStorage.setItem('sTopLevelPage', window.location);
        }

        function IsNumericKey(evt, el, DecimalPlace) {
            var number = el.value.split('.');
            var charCode;
            if (evt.key.match(/[-.0-9]/)) {
                charCode = evt.key.charCodeAt(0);
            } else {
                if (evt.key == "Backspace")
                { charCode = 8; }
                else if (evt.key == "Delete")
                { charCode = 127; }
                else
                { charCode = (evt.which) ? evt.which : event.keyCode; }
            }

            if (evt.key == "Backspace" || evt.key == "Delete" || evt.key == "ArrowUp" || evt.key == "ArrowDown" ||
                evt.key == "ArrowLeft" || evt.key == "ArrowRight" || evt.key == "Home" || evt.key == "End") {
                return true;
            }

            if (charCode != 46 && charCode != 190 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            // dot = 46
            if (number.length > 1 && charCode == 46) {
                return false;
            }
            //get the carat position
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (DecimalPlace > 0) {
                if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                    return false;
                }
            }
            el.value = replaceCommas(el.value);
            return true;
        }

        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
        }

        function replaceCommas(yourNumber) {
            var components = yourNumber.toString().split(".");
            if (components.length === 1)
                components[0] = yourNumber;
            components[0] = components[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            if (components.length === 2)
                components[1] = components[1].replace(/\D/g, "");
            return components.join(".");
        }

         function getViewDetailsModalPopupButton(id) {
             debugger;
             document.getElementById("<%=txtHiddenLouvreDetail.ClientID %>").value = id;
             __doPostBack('<%=btnViewDetails.ClientID %>', '');
         }

    </script>
    <style type="text/css">
        .auto-style4 {
            font-size: 100%;
        }
        .auto-style6 {
            background-color: #F2F2F2;
            border-top: 1px solid #ffffff;
            padding: 2px 10px 2px 10px;
            text-align: left;
            vertical-align: middle;
            width: 30%;
            font-family: Tahoma, "Trebuchet MS" , Arial, sans-serif;
            font-size: .94em;
            height: 40px;
        }
        .auto-style8 {
            background-color: #F2F2F2;
            border-top: 1px solid #ffffff;
            padding: 2px 10px 2px 10px;
            text-align: left;
            vertical-align: middle;
            width: 25%;
            font-family: Tahoma, "Trebuchet MS" , Arial, sans-serif;
            font-size: .94em;
            height: 40px;
        }
    </style>
</head>
<body onload="saveLocation();">
    <form runat="server" id="form1" method="post">

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
            <Scripts>
                <asp:ScriptReference Path="javascript/fixfocus.js" />
            </Scripts>
        </asp:ScriptManager>
        <div id="middle-container-1">

            <div id="logo" style="height: 100px; text-align: center;"></div>
            <div style="height: 50px; text-align: center;">
                <asp:Button ID="btnBack" runat="server" CssClass="form-button" Text="Back" UseSubmitBehavior="false" />
                <asp:Button ID="btnHome" runat="server" CssClass="form-button" Text="Home" UseSubmitBehavior="false" />
                <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="javascript:return ConfirmLeave();" UseSubmitBehavior="false" />
            </div>

            <h1><span class="auto-style4">Review and Accept Order</span></h1>

            <h2><asp:label ID="lblOZNumberHeading" runat="server"></asp:label></h2>

            <div style="text-align: center;">

                <div class="form" style="text-align: center;">

                    <asp:Label ID="lblOpenings" runat="server" Text=""></asp:Label>

                    <br />


                    <br />

                    <div style="text-align: center;">
                        <table class="form-table" style="width: 100%; text-align: center;" summary="">
                            <tr>
                                <asp:Button ID="btnViewDetails" runat="server" Text="View Details" CssClass="form-button" Style="visibility: hidden" />

                                <td class="form-submit-td" style="text-align: center;">
                                    <asp:Button ID="btnProcessProdSheet" runat="server" CssClass="form-button" Text="Generate Production Sheet" UseSubmitBehavior="false" Width="256px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; color: red; padding-top: 10px; padding-bottom: 10px; font-weight: bold;">
                                    <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>

                </div>

            </div>
        </div>


        <ajaxcontroltoolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnViewDetails" PopupControlID="pnlViewDetails" BackgroundCssClass="modalBackground">
        </ajaxcontroltoolkit:ModalPopupExtender>

        <asp:Panel ID="pnlViewDetails" runat="server" CssClass="modalPopup" style="display: none;">
            <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <h2>Shutter Details</h2>

                    <table class="form-table" summary="">
                        <asp:Panel ID="Panel2" runat="server" Visible="false">
                            <tr>
                                <td class="form-field-td-p2" colspan="2">Shutter Details</td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td class="auto-style8">Location:</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblLocation" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">Flush Bolts Top :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblFlushBoltsTop" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Colour :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblColour" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">Flush Bolts Bottom :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblFlushBoltsBottom" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Height :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblHeight" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">Special Lock Options :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblSpecialLockOptions" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Width :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblWidth" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">Blade Locks :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblBladeLocks" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Make / Opening :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblMakeOpening" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">Fixed Panel Channel T&amp;B :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblFixedPanelChannelTAndB" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Product :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblProduct" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">Fixed Panel Channel :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblFixedPanelChannel" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Shutter Type :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblShutterType" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">H Joiner :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblHJoiner" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Open In/Out :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblOpenInOut" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">L Reveal :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblLReveal" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">No Of Panels :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblNoOfPanels" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">Z Reveal :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblZReveal" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Blade Size :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblBladeSize" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">Top Operation :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblTopOperation" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">DLi End Plug Colour :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblEndPlugColour" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">Bottom Operation :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblBottomOperation" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Blade Clip Colour :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblBladeClipColour" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">Top Insert :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblTopInsert" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Pile Colour :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblPileColour" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">Bottom Insert :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblBottomInsert" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Top Track Type :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblTopTrackType" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">Winder :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblWinder" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Bottom Track Type :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblBottomTrackType" runat="server"></asp:Label>
                            </td>
                            <td class="auto-style8">Fly Screen :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblFlyScreen" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Curved Track :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblCurvedTrack" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="auto-style8">Slide :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblSlide" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Extra Track :</td>
                            <td class="auto-style8">
                                <asp:Label ID="lblExtraTrack" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="auto-style8">Stacker Location :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblStackerLocation" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Mid Rail Height : </td>
                            <td class="auto-style8">
                                <asp:Label ID="lblMidRailHeight" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="auto-style8">Special Requirments :</td>
                            <td class="form-field-td-p2">
                                <asp:Label ID="lblSpecialRequirements" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="form-submit-td" colspan="4" style="text-align: center">
                                <asp:Button ID="btnViewDetailsCancelChanges" runat="server" CssClass="form-button" Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>

                <Triggers>
                    <asp:PostBackTrigger ControlID="btnViewDetailsCancelChanges" />
                </Triggers>

            </asp:UpdatePanel>
        </asp:Panel>

        <div id="dialog-alert" title="Insufficient Information">
            <p id="dialog-alert-message" style="text-align: left;"></p>
        </div>
        <div id="dialog-confirm" title="Please Confirm">
            <p id="dialog-confirm-message" style="text-align: left;"></p>
        </div>

        <div id="dialog-confirm-cancel" title="Please Confirm">
            <p id="dialog-confirm-cancel-message" style="text-align: left;"></p>
        </div>

        <div runat="server" id="DivCur1" style="display: none;">
            <br />
            ProductTypeID<asp:TextBox ID="txtProductTypeID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
            <br />
            ScheduleID<asp:TextBox ID="txtID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
            <br />
            LouvreDetailID<asp:TextBox ID="txtHiddenLouvreDetail" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
            <br />
            PanelTransomHeight<asp:TextBox ID="txtHiddenPanelTransomHeight" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
            <br />
            GivenTransomHeight<asp:TextBox ID="txtHiddenGivenTransomHeight" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
            <br />
            WideRail<asp:TextBox ID="txtHiddenWideRail" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
            <br />
        </div>

    </form>
</body>

</html>

