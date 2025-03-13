<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RemakeDetails.aspx.vb" Inherits="RemakeDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxcontroltoolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Plantation Remake Details</title>
        
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
                $("#dialog-confirm-cancel").dialog({
                    modal: true,
                    autoOpen: false,
                    draggable: false,
                    buttons: {
                        "Yes": function () {
                            $(this).dialog("close");
                            __doPostBack('<%=btnCancel.ClientID%>', '');
                            return true;
                        },
                        "No": function () {
                            $(this).dialog("close");
                            document.getElementById("<%=btnCancel.ClientID%>").disabled = false;
                            document.getElementById("<%=btnCancel.ClientID%>").className = "form-button";
                            document.getElementById("<%=btnCancel.ClientID%>").value = "Cancel";
                            document.getElementById("<%=btnSave.ClientID%>").disabled = false;
                            document.getElementById("<%=btnSave.ClientID%>").className = "form-button";
                            document.getElementById("<%=btnSave.ClientID%>").value = "Save";
                            document.getElementById("imgLoading2").style.visibility = "hidden";
                            return false;
                        }
                    }
                });
            });

            $("[id$=txtOrderDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1d', maxDate: '+1w' });
            $("[id$=txtScheduledDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1d', maxDate: '+2w' });

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


            function cancelchanges() {
                var strMessage = "Are you sure you want to discard changes?";
                document.getElementById("<%=lblStatus.ClientID %>").innerHTML = "";
                document.getElementById("<%=btnCancel.ClientID%>").disabled = true;
                document.getElementById("<%=btnCancel.ClientID%>").className = "form-button-disabled";
                document.getElementById("<%=btnCancel.ClientID%>").value = "Submitting";
                document.getElementById("<%=btnSave.ClientID%>").disabled = true;
                document.getElementById("<%=btnSave.ClientID%>").className = "form-button-disabled";
                document.getElementById("<%=btnSave.ClientID%>").value = "Save";
                document.getElementById("imgLoading2").style.visibility = "visible";
                document.getElementById("dialog-confirm-cancel-message").innerHTML = strMessage;
                $("#dialog-confirm-cancel").dialog("open");
                return false;
            }

            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
                return true;
            }

    </script>

</head>
<body onload="saveLocation();onloadctls();">
    <form runat="server" id="form1" method="post">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    <Scripts>
    <asp:ScriptReference Path="javascript/fixfocus.js" />
    </Scripts>
    </asp:ScriptManager>
    <div id="middle-container-diallerview" >

        <div id="logo" style="height: 100px; text-align: center;" ></div>
        <div style="height: 50px; text-align: center;">
            <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="javascript:return ConfirmLeave();" UseSubmitBehavior="false" />
	    </div>
    
        <h1>Plantation Remake Details</h1>

        <div class="form" style="text-align: center;">

        <asp:UpdatePanel ID="pnlDetails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
            <div style="text-align: center;">
            <table class="form-table" style="width: 100%; text-align: center;" cellspacing="0" summary="">
                <tr>
                    <td class="form-label-td" style="width: 15%; text-align: right;">Contract No&nbsp;:&nbsp;</td>
                    <td class="form-label-td" style="width: 35%; text-align: left;">
                        <asp:TextBox ID="txtContractNumber" runat="server" CssClass="form-field" Text="" Width="200px" />
                    </td>
                    <td class="form-label-td" style="width: 15%; text-align: right;">Shutter Pro Number&nbsp;:&nbsp;</td>
                    <td class="form-label-td" style="width: 35%; text-align: left;">
                        <asp:Label ID="lblShutterProNumber" runat="server" Text=""></asp:Label>
                    </td> 
                </tr>
                <tr>
                    <td class="form-label-td" style="width: 15%; text-align: right;">Customer&nbsp;:&nbsp;</td>
                    <td class="form-label-td" style="width: 35%; text-align: left;">
                       <asp:DropDownList ID="cboCustomer" runat="server" UseSubmitBehavior="false" OnChange="javascript:return getstate();" CssClass="form-select" Width="200px" Height="28px" />
                    </td>
                    <td class="form-label-td" style="width: 15%; text-align: right;">Order Type&nbsp;:&nbsp;</td>
                    <td class="form-label-td" style="width: 35%; text-align: left;">
                        <asp:DropDownList ID="cboOrderType" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width ="200px" Height="28px" />
                    </td> 
                </tr>
                <tr>
                    <td class="form-label-td" style="width: 15%; text-align: right;">Customer Name&nbsp;:&nbsp;</td>
                    <td class="form-label-td" style="width: 35%; text-align: left;">
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-field" Text="" Width="200px" />
                    </td>
                    <td class="form-label-td" style="width: 15%; text-align: right;">State&nbsp;:&nbsp;</td>
                    <td class="form-label-td" style="width: 35%; text-align: left;">
                        <asp:TextBox ID="txtState" runat="server" CssClass="form-field" Text="" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td class="form-label-td" style="width: 15%; text-align: right;">Order Date&nbsp;:&nbsp;</td>
                    <td class="form-label-td" style="width: 35%; text-align: left;">
                        <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form-field" Text="" Width="120px" />
                    </td>
                    <td class="form-label-td" style="width: 15%; text-align: right;">Order Status&nbsp;:&nbsp;</td>
                    <td class="form-label-td" style="width: 35%; text-align: left;">
                        <asp:DropDownList ID="cboOrderStatus" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width ="200px" Height="28px" />
                    </td>
                </tr>
                <tr>
                    <td class="form-label-td" style="width: 15%; text-align: right;">Schedule Date&nbsp;:&nbsp;</td>
                    <td class="form-label-td" style="width: 35%; text-align: left;">
                        <asp:TextBox ID="txtScheduledDate" runat="server" CssClass="form-field" Text="" Width="120px" />
                    </td>
                    <td class="form-label-td" style="width: 15%; text-align: right;">Priority&nbsp;:&nbsp;</td>
                    <td class="form-label-td" style="width: 35%; text-align: left;">
                        <asp:DropDownList ID="cboPriority" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width ="200px" Height="28px" />
                    </td>
                </tr>
                <tr>
                    <td class="form-label-td" style="width: 40%; text-align: right;">New Note&nbsp;:&nbsp;</td>
                    <td class="form-label-td" style="width: 60%; text-align: left;">
                        <asp:TextBox ID="txtNotes" runat="server" CssClass="form-field" Text="" TextMode="MultiLine"  Width="90%" Height="200px" />
                    </td>
                </tr>
            </table>

            <div style="text-align: center;">
                <asp:Button ID="btnAddDetails" runat="server" Text="Add Detail" CssClass="form-button" />

                <asp:GridView ID="dgvDetails" runat="server" DataKeyNames="PSDetailID" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnViewDetail" runat="server" Text="Details" CommandName="PlantationDetail"  CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CssClass="form-button" Width="100px" Height="28px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PSDetailID" Visible="false" /> 
                        <asp:BoundField DataField="PlantationScheduleID" Visible="false" /> 
                        <asp:BoundField DataField="ShutterID" SortExpression="ShutterID" HeaderText="Opening #" /> 
                        <asp:BoundField DataField="Width" SortExpression="Width" HeaderText="Width" />
                        <asp:BoundField DataField="Height" SortExpression="Height" HeaderText="Height" />
                        <asp:BoundField DataField="PanelQtyID" SortExpression="PanelQtyID" HeaderText="# Panels" />
                        <asp:BoundField DataField="Colour" SortExpression="Colour" HeaderText="Colour" />                        
                    </Columns>
                </asp:GridView>                       
            
            </div>

            <table class="form-table" style="width: 100%; text-align: center;" cellspacing="0" summary="">
                <tr>
                    <td class="form-submit-td"  style="width: 50%; text-align: right;"><img id="imgLoading2" src="images/indicator.gif" width="16px" height="16px" alt="loading" style="visibility: hidden; vertical-align: middle;" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" OnClientClick="return cancelchanges();" UseSubmitBehavior="false" />
                    </td>
                    <td class="form-submit-td"  style="width: 50%; text-align: left;"><img id="imgLoading1" src="images/indicator.gif" width="16px" height="16px" alt="loading" style="visibility: hidden; vertical-align: middle;" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnSave" runat="server" CssClass="form-button" Text="Save" OnClientClick="return ValidateAll();" UseSubmitBehavior="false" />
                    </td>
                </tr>
                <tr>
                    <td class="form-submit-td" colspan="2" style="text-align: left;"><asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red" /></td>
                </tr>
            </table>
            </div>

            <ajaxcontroltoolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" BehaviorID="modal1" TargetControlID="btnAddDetails" PopupControlID="pnlRemakeDetails" BackgroundCssClass="modalBackground">
            </ajaxcontroltoolkit:ModalPopupExtender>

            <asp:Panel ID="pnlRemakeDetails" runat="server" CssClass="modalPopup" Style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="form-table" cellspacing="0" summary="">
                            <tr>
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    Installation Area:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboInstallationArea" runat="server" CssClass="form-select" Width="150px" AutoPostBack="true" >
                                    </asp:DropDownList>
                                </td>
                                <td class="form-field-td-p2" style="width: 11%; text-align: right;">                                    
                                </td>
                                <td class="form-field-td-p2"  style="width: 22%;" >                                    
                                </td>
                                <td class="form-field-td-p2" style="width: 11%; text-align: right;">                                    
                                </td>
                                <td class="form-field-td-p2"  style="width: 22%;" >                                    
                                </td>
                            </tr>
                            <tr>
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    Room Location:
                                </td>
                                <td colspan ="5" class="form-field-td-p2" >
                                    <table>
                                        <tr>
                                            <td class="form-field-td-ip2" style="width: 50%;">
                                                <asp:DropDownList ID="cboRoomLocation" runat="server" CssClass="form-select" Width="150px"
                                                    AutoPostBack="true" >
                                                </asp:DropDownList>
                                            </td>
                                            <asp:Panel ID="pnlRoomLocation" runat="server">
                                                <td class="form-label-td-ip2" style="width: 30%; text-align: right;">
                                                    <asp:Label ID="lblOtherdesc" runat="server" Text="Other Description:" ></asp:Label>
                                                </td>
                                                <td class="form-field-td-ip2" style="width: 20%;">
                                                    <asp:TextBox ID="txtRLOther" runat="server" CssClass="form-field" Width="150px" MaxLength="100"> </asp:TextBox>
                                                </td>
                                            </asp:Panel> 
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                                <tr>
                                <td class="form-field-td-p2" style="width: 11%; text-align: right;">
                                    Width :
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:TextBox ID="txtWidth" runat="server" CssClass="form-field" Width="100px" MaxLength="4" onkeypress="return isNumberKey(event);"></asp:TextBox>&nbsp;&nbsp;(mm)
                                </td>
                                <td class="form-field-td-p2" style="width: 11%; text-align: right;">
                                    Height :
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:TextBox ID="txtHeight" runat="server" CssClass="form-field" Width="100px" MaxLength="4" onkeypress="return isNumberKey(event);"></asp:TextBox>&nbsp;&nbsp;(mm)
                                </td>
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    Panel Quantity:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboPanelQty" runat="server" CssClass="form-select" Width="150px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    Mount Config :
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboMountConfig" runat="server" CssClass="form-select" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                    <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                        Mount Style:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboMountStyle" runat="server" CssClass="form-select" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                        Material:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboMaterial" runat="server" CssClass="form-select" Width="150px">
                                        <asp:ListItem Value="0" Text=""></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Insulite"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    Blade Size:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboBladeSize" runat="server" CssClass="form-select" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                    <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                        Colour:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboColour" runat="server" CssClass="form-select" Width="150px" AutoPostBack="true" >
                                    </asp:DropDownList>
                                </td>
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                        Mount Method:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboMountMethod" runat="server" CssClass="form-select" Width="150px" AutoPostBack="true" >
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-label-td-p2"  colspan="6">
                                    <asp:Panel ID="pnlMidRail" runat="server" Visible="false" >
                                        <table style="width:100%">
                                            <tr>
                                                <td class="form-label-td-p2" style="width: 40%; text-align: right; border-top: 0px;">
                                                    Mid Rail Height:
                                                </td>
                                                <td class="form-field-td-p2" style="width: 60%;border-top: 0px;">
                                                    <asp:TextBox ID="txtMidRailHeight" runat="server" CssClass="form-field" Width="100px" AutoPostBack="true" MaxLength="4" onkeypress="return isNumberKey(event);"></asp:TextBox>&nbsp;&nbsp;(mm)
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel> 
                                </td>
                            </tr>
                                <asp:Panel ID="pnlboards" runat="server" Visible="false" >
                                    <tr>
                                    <td class="form-label-td-p2"  colspan="3">
                                        <table style="width:100%">
                                            <tr>
                                                <td class="form-label-td-p2" style="width: 40%; text-align: right; border-top: 0px;">
                                                    Sideboards:
                                                </td>
                                                <td class="form-field-td-p2" style="width: 60%;border-top: 0px;">
                                                    <asp:DropDownList ID="cboSideboards" runat="server" Width="150px" CssClass="form-select" >
                                                        <asp:ListItem Value="0" Text=""></asp:ListItem>
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="2">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                        <td class="form-label-td-p2"  colspan="3">
                                        <table style="width:100%">
                                            <tr>
                                                <td class="form-label-td-p2" style="width: 40%; text-align: right; border-top: 0px;">
                                                    Bottomboards:
                                                </td>
                                                <td class="form-field-td-p2" style="width: 60%;border-top: 0px;">
                                                    <asp:DropDownList ID="cboBottomboards" runat="server" Width="150px" CssClass="form-select" >
                                                        <asp:ListItem Value="0" Text=""></asp:ListItem>
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="2">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>                                
                            </asp:Panel>                                                                 
                            <tr>
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    Layout Code:
                                </td>
                                <td  class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboLayout" runat="server" CssClass="form-select" Width="150px" AutoPostBack="true" >
                                    </asp:DropDownList>
                                </td>             
                                <td colspan="2" class="form-field-td-p2"  >
                                    <asp:Panel ID="pnlLayoutOther" runat="server" Visible="false" >
                                        <asp:TextBox ID="txtLayoutOther" runat="server" CssClass="form-field" Width="110px" AutoPostBack="true" MaxLength="100"></asp:TextBox>
                                    </asp:Panel>        
                                </td>
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    Hinge Colour:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboHingeColour" runat="server" CssClass="form-select" Width="150px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <asp:Panel ID="pnlSliding" runat="server">
                                <tr >
                                    <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                        Bottom Guide:
                                    </td>
                                    <td  class="form-field-td-p2" style="width: 22%;">
                                        <asp:DropDownList ID="cboBottomGuide" runat="server" CssClass="form-select" Width="150px">
                                        </asp:DropDownList>
                                    </td> 
                                    <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                        Sliding:
                                    </td>
                                    <td  class="form-field-td-p2" style="width: 22%;">
                                        <asp:DropDownList ID="cboSliding" runat="server" CssClass="form-select" Width="150px">
                                        </asp:DropDownList>
                                    </td> 
                                    <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    </td>
                                    <td class="form-label-td-p2" style="width: 22%; text-align: right;">
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    Frame Type:
                                </td>   
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboFrameType" runat="server" CssClass="form-select" Width="150px" AutoPostBack="true" >
                                    </asp:DropDownList>
                                </td>   
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    Sides:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboSides" runat="server" CssClass="form-select" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    Control Type:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboControlType" runat="server" CssClass="form-select" Width="150px">
                                    </asp:DropDownList>
                                </td>
                            </tr> 
                            <tr> 
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    Track:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboTrack" runat="server" Width="150px" CssClass="form-select" AutoPostBack="true" >
                                        <asp:ListItem Value="0" Text=""></asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                        <asp:ListItem Value="2">No</asp:ListItem>
                                    </asp:DropDownList>
                                </td> 
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    T Post Qty:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboTPostQty" runat="server" CssClass="form-select" Width="150px">
                                    </asp:DropDownList>
                                </td> 
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                </td>
                                <td class="form-label-td-p2" style="width: 22%; text-align: right;">
                                </td>
                            </tr> 
                            <asp:Panel ID="pnlSplitBlade" runat="server">
                                <tr>  
                                    <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                        Split Blade:
                                    </td>
                                        <td colspan ="5" class="form-field-td-p2" >
                                        <table>
                                            <tr>
                                                <td class="form-field-td-ip2" style="width: 50%;">
                                                    <asp:DropDownList ID="cboSplitBlade" runat="server" CssClass="form-select" Width="150px"  AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td> 
                                                <asp:Panel ID="pnlSplitBladeHeight" runat="server">
                                                <td class="form-label-td-ip2" style="width: 20%; text-align: right;">
                                                    Height of Split Blade:
                                                </td>
                                                <td class="form-field-td-ip2" style="width: 30%;">
                                                    <asp:TextBox ID="txtHeightSplitBlade" runat="server" CssClass="form-field" Width="100px" MaxLength="4" onkeypress="return isNumberKey(event);"></asp:TextBox>&nbsp;&nbsp;(mm)
                                                </td>
                                                </asp:Panel> 
                                            </tr>
                                        </table> 
                                    </td> 
                                </tr>   
                            </asp:Panel> 
                            <tr> 
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                        Hang Strip:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboHangStrip" runat="server" CssClass="form-select" Width="150px">
                                    </asp:DropDownList>
                                </td> 
                                <td class="form-label-td-p2" colspan="2">
                                    <asp:Panel ID="pnlLightBlock" runat="server" Visible="true" >
                                        <table  style="width:100%">
                                            <tr>
                                                <td style="width: 40%; text-align: right; border-top: 0px;padding: 2px 10px 2px 8px;">
                                                    Light Block:
                                                </td>
                                                <td  style="width: 60%;padding: 2px 10px 2px 8px;">
                                                    <asp:DropDownList ID="cboLightBlock" runat="server" CssClass="form-select" Width="150px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table> 
                                    </asp:Panel>
                                </td> 
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    Angle Bay:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboAngleBay" runat="server" CssClass="form-select" Width="150px">
                                    </asp:DropDownList>
                                </td> 
                            </tr> 
                            <tr>
                                <td class="form-label-td-p2" style="width: 11%; text-align: right;">
                                    Fixed Blades:
                                </td>
                                <td class="form-field-td-p2" style="width: 22%;">
                                    <asp:DropDownList ID="cboFixedBlades" runat="server" Width="150px" CssClass="form-select" AutoPostBack="true" >
                                        <asp:ListItem Value="0" Text=""></asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                        <asp:ListItem Value="2">No</asp:ListItem>
                                    </asp:DropDownList>
                                </td> 
                                <asp:Panel ID="pnlStainlessSteelWheels" runat="server">
                                    <td class="form-label-td-ip2" style="width: 11%; text-align: right;">
                                        Stainless Steel Wheels:
                                    </td>
                                    <td colspan="3" class="form-field-td-p2" style="width: 22%;">
                                        <asp:DropDownList ID="cboStainlessSteelWheels" runat="server" Width="150px" CssClass="form-select" AutoPostBack="true" >
                                            <asp:ListItem Value="0" Text=""></asp:ListItem>
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="2">No</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </asp:Panel>
                            </tr>  
                            <tr>
                                <td class="form-label-td-p2" style="width: 20%; text-align: right;">
                                    Special Requirements:
                                </td>
                                <td colspan="5" class="form-field-td-p2" style="width: 80%">
                                    <asp:TextBox ID="txtSpecialRequirements" runat="server" CssClass="form-field" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                           
                            <tr>
                                <td class="form-submit-td" >
                                    <div style="float: left;">
                                        <asp:Button runat="server" ID="btnSaveDetails" text="Save" CssClass="form-button" />
                                    </div>
                                </td>
                                <td class="form-submit-td" >
                                    <div style="float: right;">
                                        <asp:Button runat="server" ID="btnCancelDetails" Text="Cancel" CssClass="form-button" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-submit-td" colspan="2">
                                    <asp:Label ID="lblShutterStatus" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>                        
                        </table>                                               
                    </ContentTemplate>

                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSaveDetails" />
                        <asp:PostBackTrigger ControlID="btnCancelDetails" />
                    </Triggers>

                </asp:UpdatePanel>
            </asp:Panel>


        </ContentTemplate>
        </asp:UpdatePanel>

        </div>

    </div>

    <div id="dialog-alert" title="Insufficient Information">
        <p id="dialog-alert-message" style="text-align: left;"></p>
    </div>
    <div id="dialog-confirm" title="Please Confirm" >
        <p id="dialog-confirm-message" style="text-align: left;"></p>
    </div>
        
    <div id="dialog-confirm-cancel" title="Please Confirm" >
        <p id="dialog-confirm-cancel-message" style="text-align: left;"></p>
    </div>

    <div runat="server" id="DivCur1" style="display:none;">
    Jobnumber:<asp:TextBox ID="txtJobnumber" runat="server" TabIndex="-1" Text="0" />
    <br />
    ID:<asp:TextBox ID="txtId" runat="server" TabIndex="-1" Text="0" />
    <br />
    CustomerId:<asp:TextBox ID="txtCustomerId" runat="server" TabIndex="-1" Text="0" />
    <br />
    EnteredDatetime:<asp:TextBox ID="txtEnteredDatetime" runat="server" TabIndex="-1" Text="0" />
    <br />
    ViewType:<asp:TextBox ID="txtViewType" runat="server" TabIndex="-1" Text="0" />
    <br />
    PSDetailID:<asp:TextBox ID="txtPSDetailID" runat="server" TabIndex="-1" Text="0" />
    <br />
    </div>

    </form>
</body>

</html>
