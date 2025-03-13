<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AdminPanel.aspx.vb" Inherits="AdminPanel" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxcontroltoolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="ext" Namespace="Extensions" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>OzRoll - Admin</title>
        
 <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta name="format-detection" content="telephone=no" />
    
    <link href="stylesheets/reset.css" rel="stylesheet" type="text/css" media="screen" />
    <!--[if IE]><link href="stylesheets/explorer.css" rel="stylesheet" type="text/css" media="screen" /><![endif]-->
    <link href="stylesheets/style.css?version=<%# Now.Year() & Now.Month & Now.Day() %>" rel="stylesheet" type="text/css" media="screen" />
    <link href="stylesheets/smoothness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />
  
    <!--[if lt IE 8]><script src="javascript/IE-fix.js" type="text/javascript"></script><![endif]-->
    <script src="javascript/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>

    <link href="stylesheets/focus.css" rel="stylesheet" type="text/css" />
    <script src="javascript/focus.js" type="text/javascript"></script>
   
    <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
    
    <script language="javascript" type="text/javascript">

        if (window.history.forward(1) != null)
            window.history.forward(1);

        $(function() {
        
            $("#dialog-confirm").dialog({
                modal: true,
                autoOpen: false,
                draggable: false,
                buttons: {
                    "Yes": function() {
                        $(this).dialog("close");
                        __doPostBack('<%# btnLogout.ClientID %>', '');
                        return true;
                    },
                    "No": function() {
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
        
        });

        function ConfirmLeave() {
            document.getElementById("dialog-confirm-message").innerHTML = "Are you sure you want to logout?";
            $("#dialog-confirm").dialog("open");
            return false;
        }

        function pageLoad() {
            $("input.dtePicker").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'fadeIn', minDate: '-10y', maxDate: '+10y' });
        }

        $(document).ready(function () {
            var xPos, yPos;
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(BeginRequestHandler);
            prm.add_endRequest(EndRequestHandler);

            function BeginRequestHandler(sender, args) {
                xPos = $get('scrollDiv').scrollLeft;
                yPos = $get('scrollDiv').scrollTop * 1.5;
            }

            function EndRequestHandler(sender, args) {
                $get('scrollDiv').scrollLeft = xPos;
                $get('scrollDiv').scrollTop = yPos * 1.5;
            }
        });

        function afuLouvrePriceAjaxUploader_UploadComplete(sender, e) {
            if (sender._filesInQueue[sender._filesInQueue.length - 1]._isUploaded)
                // Do post back only after all files have been uploaded
                __doPostBack('btnLouvrePricesUploaded', '');
        }

    </script>

    <style type="text/css">
        
        .admin-panel {
            text-align: center;
        }

        .log-table {
            margin: 0 auto;
            border: solid 1px lightgrey;
            table-layout: fixed;
        }

        .log-table th, .log-table td {
            padding: 5px;
        }

        .break-word {
            word-wrap: break-word;
        }

        .logins-table {
            margin: 0 auto;
            border: solid 1px lightgrey;
        }

        .logins-table th, .logins-table td {
            padding: 5px;
        }

        .row-edit-button {
            padding:5px 20px 5px 20px;
        }

        .admin-chk {
            margin-left: 6px;
            margin-top: 10px;
            margin-bottom: 10px;
        }

        .validation {
            margin-left: 10px;
        }

        .reset-button {
            margin-top: 20px;
        }

        a:focus, a:hover {
            text-shadow: 0px 1px 1px white !important;
        }

        .footer-textbox{
            margin-top: 10px;
        }

        .gdv_linkButton {
            color: white !important;
            font-weight: bold;
        }
        .gdv_linkButton:hover {
            color: white !important;
            font-weight: bold;
        }
        .gdv_linkButton:visited {
            color: white !important;
            font-weight: bold;
        }
        .gdv_linkButton:active {
            color: white !important;
            font-weight: bold;
        }

        .subheading{
            font-size: 0.9em;
            margin-top: -20px;
            display: inline-block;
            color: #ffde00;
        }

        .validation-text
        {
            background-color: lightgray;
            display:  block;
            margin-top: 2px;
            padding: 4px;
            height: 0.8em;
        }

        .admin-menu {
            padding: 20px;
        }

        .admin-menu-button {
            margin: 5px;
        }

        .louvreprices-searchfiltertable {
            width:100%;
            text-align: center;
        }

        .louvreprices-searchfiltertable th, .louvreprices-searchfiltertable td {
            padding: 5px;
        }

        .louvreprices-table {
            margin-left: auto;
            margin-right: auto;
        }

        .louvreprices-table th {
            padding: 10px;
        }

        .file-uploader {
            margin: 0 auto;
            width: 100%;
        }

        .file-upload-panel-louvreprices {
            margin: 0 auto;
            text-align: center;
            width: 30%;
        }

        .hidden {
            display: none;
        }

        .criteria-description {
            text-align:right;
        }

        .criteria-contol {
            text-align:left;
        }

        .LouvrePriceFileDropHeader {
            font-style: italic;
        }

        .validation-text {
            background-color:transparent;
        }

    </style>
    
</head>
<body>
    <form runat="server" id="form1" method="post">

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" AsyncPostBackTimeout="300" >
            <Scripts>
                <asp:ScriptReference Path="javascript/fixfocus.js" />
            </Scripts>
        </asp:ScriptManager>

        <div id="middle-container-diallerview" >

            <div id="logo" style="height: 100px; text-align: center;" ></div>
            <div style="height: 50px; text-align: center;">
                <asp:Button ID="btnHome" runat="server" CssClass="form-button" Visible="true" Text="Home" UseSubmitBehavior="false" TabIndex="-1" />&nbsp;&nbsp;
                <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" UseSubmitBehavior="false" TabIndex="-1" />
	        </div>
    
            <h1>
                <asp:Label ID="lblTitle" runat="server"></asp:Label>
            </h1>
    

            <div style="height: 100%; text-align: center; border-radius: 1em;">
                <div style="display:inline-block; height: 100%; width: 1200px; text-align: center; background-color: #F2F2F2; box-shadow:10px 10px 10px grey; border-radius: 1em;">           

                    <asp:Panel ID="pnlAdminMenu" CssClass="admin-menu" runat="server">
                        <br />
                        <asp:Button ID="btnLogins" runat="server" CssClass="form-button admin-menu-button" Text="Customer Logins" UseSubmitBehavior="false" Width="170px" />
                        <asp:Button ID="btnAddressZones" runat="server" CssClass="form-button admin-menu-button" Text="Address Zones" UseSubmitBehavior="false" Width="170px" />
                        <asp:Button ID="btnExtraProducts" runat="server" CssClass="form-button admin-menu-button" Text="Extra Products" UseSubmitBehavior="false" Width="170px" />
                        <asp:Button ID="btnLouvrePrices" runat="server" CssClass="form-button admin-menu-button" Text="Louvre Prices" UseSubmitBehavior="false" Width="170px" />
                        <asp:Button ID="btnColours" runat="server" CssClass="form-button admin-menu-button" Text="Colours" UseSubmitBehavior="false" Width="170px" />
                        <asp:Button ID="btnPermissions" runat="server" CssClass="form-button admin-menu-button" Text="Permissions" UseSubmitBehavior="false" Width="170px" />
                        <asp:Button ID="btnLog" runat="server" CssClass="form-button admin-menu-button" Text="Log" UseSubmitBehavior="false" Width="170px" />
                        <asp:Button ID="btnAddUser" runat="server" CssClass="form-button header-menu-button" Text="User Mgmt." UseSubmitBehavior="false" />

                    </asp:Panel>

                    <asp:Panel ID="pnlLogins" runat="server" CssClass="admin-panel">

                        <h2>Logins</h2>  

                        <br />
                        <br />

                        <asp:Panel ID="pnlLoginsSearchCriteria" runat="server" >
                            <asp:CheckBox ID="chkLoginsShowDiscontinued" runat="server" Text="" AutoPostBack="true" CssClass="checkbox" />Include Discontinued Logins
                        </asp:Panel>

                        <br />
                        <br />
                        <br />

                        <ext:GridViewExtended ID="gdvLogins" runat="server" DataKeyNames="LoginID" AutoGenerateColumns="false" CellPadding="4" 
                                    CssClass="logins-table" ForeColor="#333333" Width="90%" AllowSorting="true" SortColumn="" SortDirection="" 
                                    ShowHeaderWhenEmpty="true" ShowFooterWhenEmpty="true">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="Green" Font-Bold="false" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="LoginID" Visible="false" />
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="LoginName" SortExpression="LoginName" HeaderText="Username" />
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="FirstName" SortExpression="FirstName" HeaderText="First Name" />
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="LastName" SortExpression="LastName" HeaderText="Last Name" />
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="EmailAddress" SortExpression="EmailAddress" HeaderText="Email" />

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Customer" text="Customer" 
                                            CssClass="gdv_linkButton" OnCommand="gdvLogins_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:label ID="lblLoginsCustomer" runat="server" Enabled="false" text='<%# GetCustomerNameByID(DirectCast(GetDataItem(), LoginDetails).CustomerID) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Administrator" text="Administrator" 
                                            CssClass="gdv_linkButton" OnCommand="gdvLogins_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAdminUser" runat="server" Enabled="false" Checked='<%# Eval("AdminUser") %>' CssClass="checkbox" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Discontinued" text="Discontinued" 
                                            CssClass="gdv_linkButton" OnCommand="gdvLogins_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDiscontinued" runat="server" Enabled="false" Checked='<%# Eval("Discontinued") %>' CssClass="checkbox" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnEditLogin" ToolTip="Edit" runat="server" CssClass="edit-button"  
                                            CommandArgument='<%# Eval("LoginID") %>' CommandName="EditLogin" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </ext:GridViewExtended>
                        <br />
                        <asp:Button ID="btnAddNewLogin" runat="server" CssClass="form-button update-disable" Text="Create New Login" UseSubmitBehavior="false" />
                        <br />
                        <br />
                    </asp:Panel>

                    <asp:Panel ID="pnlAddressZones" runat="server" CssClass="admin-panel">

                        <h2>Address Zones</h2>

                        <br />
                        <br />

                        <asp:Panel ID="pnlAddressZonesFilterCriteria" runat="server" >
                            
                        </asp:Panel>

                        <br />
                        <br />
                        <br />

                        <ext:GridViewExtended ID="gdvAddressZones" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CellPadding="4" 
                                    CssClass="logins-table" ForeColor="#333333" Width="90%" AllowSorting="false" SortColumn="" SortDirection="" 
                                    ShowHeaderWhenEmpty="true" ShowFooterWhenEmpty="true" >
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="44" />
                            <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="Green" Font-Bold="false" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddressZoneID" runat="server" text='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="ZoneName" text="Zone Name" 
                                            CssClass="gdv_linkButton" OnCommand="gdvAddressZones_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" text='<%# Eval("ZoneName") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNewAddressZoneName" CssClass="form-field footer-textbox" runat="server" MaxLength="100" text='<%# Eval("ZoneName") %>' ></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewAddressZoneName"
                                            Display="static"
                                            ValidationGroup="NewAddressZone"
                                            Text="Zone Name Required" />
                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:TextBox ID="txtNewAddressZoneName" CssClass="form-field footer-textbox" MaxLength="100" runat="server"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewAddressZoneName"
                                            Display="static"
                                            ValidationGroup="NewAddressZone"
                                            Text="Zone Name Required" />

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Price" text="Price (% of order)" 
                                            CssClass="gdv_linkButton" OnCommand="gdvAddressZones_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" text='<%# FormatPercent(Eval("Price")) %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNewAddressZonePrice" CssClass="form-field footer-textbox" runat="server" MaxLength="10" text='<%# FormatNumber((CDec(Eval("Price")) * 100), 2) %>' ></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewAddressZonePrice"
                                            Display="Dynamic"
                                            ValidationGroup="NewAddressZone"
                                            Text="Price Required" />
                                        <asp:RegularExpressionValidator runat="server"
                                            ValidationExpression="\d+((,\d{1,4})+)?(\.\d{1,2})?"
                                            ControlToValidate="txtNewAddressZonePrice" 
                                            Text="Price Invalid"
                                            ValidationGroup="NewAddressZone"
                                            CssClass="validation-text"
                                            Display="static" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewAddressZonePrice" CssClass="form-field footer-textbox" MaxLength="10" runat="server"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewAddressZonePrice"
                                            Display="Dynamic"
                                            ValidationGroup="NewAddressZone"
                                            Text="Price Required" />
                                        <asp:RegularExpressionValidator runat="server"
                                            ValidationExpression="\d+((,\d{1,4})+)?(\.\d{1,2})?"
                                            ControlToValidate="txtNewAddressZonePrice" 
                                            Text="Price Invalid"
                                            ValidationGroup="NewAddressZone"
                                            CssClass="validation-text"
                                            Display="static" />

                                    </FooterTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Effective" text="Effective" 
                                            CssClass="gdv_linkButton" OnCommand="gdvAddressZones_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" text='<%# Eval("EffectiveDateTime") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNewAddressZoneEffectiveDate" CssClass="form-field footer-textbox dtePicker" runat="server" MaxLength="50" text='<%# Eval("EffectiveDateTime") %>' ></asp:TextBox>

                                        <asp:CustomValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewAddressZoneEffectiveDate"
                                            Display="static"
                                            ValidationGroup="NewAddressZone"
                                            ValidateEmptyText="true"
                                            EnableClientScript="true"
                                            OnServerValidate="valcustNewAddressZoneEffectiveDate_Validate"
                                            Text="Effective Date Invalid" /> 
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewAddressZoneEffectiveDate" CssClass="form-field footer-textbox dtePicker" MaxLength="50" runat="server"></asp:TextBox>

                                        <asp:CustomValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewAddressZoneEffectiveDate"
                                            Display="static"
                                            ValidationGroup="NewAddressZone"
                                            ValidateEmptyText="true"
                                            EnableClientScript="true"
                                            OnServerValidate="valcustNewAddressZoneEffectiveDate_Validate"
                                            Text="Effective Date Invalid" /> 

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Created" text="Created" 
                                            CssClass="gdv_linkButton" OnCommand="gdvAddressZones_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" text='<%# Eval("CreationDateTime") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle width="100px" />
                                    <ItemTemplate>
                                        <asp:Button ID="btnEditAddressZone" runat="server" CssClass="edit-button" 
                                            CommandName="edit" ToolTip="Edit" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnEditAddressZoneRanges" runat="server" CssClass="postcode-button" 
                                            CommandArgument='<%# Eval("ID") %>' CommandName="EditAddressZoneRanges" ToolTip="Postcodes" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button ID="btnSaveNewAddressZone" runat="server" CssClass="save-button" tooltip="Save" 
                                            UseSubmitBehavior="false" CommandName="update" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnCancelNewAddressZone" runat="server" CssClass="cancel-button" tooltip="Cancel" 
                                            UseSubmitBehavior="false" CommandName="cancel" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Button ID="btnSaveNewAddressZone" runat="server" CssClass="save-button" tooltip="Save" 
                                            UseSubmitBehavior="false" CommandName="SaveNewAddressZone" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnCancelNewAddressZone" runat="server" CssClass="cancel-button" tooltip="Cancel" 
                                            UseSubmitBehavior="false" CommandName="CancelNewAddressZone" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </ext:GridViewExtended>

                        <br />

                        <asp:Button ID="btnAddAddressZone" runat="server" CssClass="form-button" Text="New Address Zone" UseSubmitBehavior="false" />

                        <br />
                        <br />

                    </asp:Panel>
                    
                    <asp:Panel ID="pnlExtraProducts" runat="server" CssClass="admin-panel">

                        <h2>Extra Products</h2>

                        <br />
                        <br />

                        <asp:Panel ID="pnlExtraProductsSearchCriteria" runat="server" >
                            <asp:CheckBox ID="chkExtraProductsShowDiscontinued" runat="server" Text="" AutoPostBack="true" CssClass="checkbox" />Include Discontinued Extra Products
                        </asp:Panel>

                        <br />
                        <br />
                        <br />

                        <ext:GridViewExtended ID="gdvExtraProducts" runat="server" DataKeyNames="ExtraProductID" AutoGenerateColumns="false" CellPadding="4" 
                                    CssClass="logins-table" ForeColor="#333333" Width="90%" AllowSorting="false" SortColumn="" SortDirection="" ShowFooter="false"
                                    ShowHeaderWhenEmpty="true" ShowFooterWhenEmpty="true" >
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="43" />
                            <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="Green" Font-Bold="false" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>

                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExtraProductID" runat="server" text='<%# Eval("ExtraProductID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Description" text="Description" 
                                            CssClass="gdv_linkButton" OnCommand="gdvExtraProducts_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" text='<%# Eval("Description") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <asp:TextBox ID="txtNewExtraProductDescription" CssClass="form-field footer-textbox" text='<%# Eval("Description") %>' MaxLength="100" runat="server"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductDescription"
                                            Display="Static"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Description Required" />

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:TextBox ID="txtNewExtraProductDescription" CssClass="form-field footer-textbox" MaxLength="100" runat="server"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductDescription"
                                            Display="Static"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Description Required" />

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="ProductCode" text="Product Code" 
                                            CssClass="gdv_linkButton" OnCommand="gdvExtraProducts_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" text='<%# Eval("ProductCode") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <asp:TextBox ID="txtNewExtraProductProductCode" CssClass="form-field footer-textbox" text='<%# Eval("ProductCode") %>' MaxLength="50" runat="server"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductProductCode"
                                            Display="Static"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Product Code Required" />

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:TextBox ID="txtNewExtraProductProductCode" CssClass="form-field footer-textbox" MaxLength="50" runat="server"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductProductCode"
                                            Display="Static"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Product Code Required" />

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="UnitOfMeasurement" text="Max Length (mm)" 
                                            CssClass="gdv_linkButton" OnCommand="gdvExtraProducts_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" text='<%# Eval("UnitOfMeasurement") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <asp:TextBox ID="txtNewExtraProductUnitOfMeasurement" CssClass="form-field footer-textbox" text='<%# Eval("UnitOfMeasurement") %>' MaxLength="8" runat="server" Width="40px"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductUnitOfMeasurement"
                                            Display="Static"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Measurement Required" />
                                        <asp:CompareValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductUnitOfMeasurement"
                                            Display="dynamic"
                                            Type="Integer"
                                            Operator="DataTypeCheck"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Measurement Invalid" />

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:TextBox ID="txtNewExtraProductUnitOfMeasurement" CssClass="form-field footer-textbox" MaxLength="8" runat="server" Width="40px"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductUnitOfMeasurement"
                                            Display="Static"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Measurement Required" />
                                        <asp:CompareValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductUnitOfMeasurement"
                                            Display="dynamic"
                                            Type="Integer"
                                            Operator="DataTypeCheck"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Measurement Invalid" />

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="SortOrder" text="Sort Order" 
                                            CssClass="gdv_linkButton" OnCommand="gdvExtraProducts_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" text='<%# Eval("SortOrder") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <asp:TextBox ID="txtNewExtraProductSortOrder" CssClass="form-field footer-textbox" text='<%# Eval("SortOrder") %>' MaxLength="6" runat="server" Width="30px"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductSortOrder"
                                            Display="Static"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Sort Order Required" />
                                        <asp:CompareValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductSortOrder"
                                            Display="dynamic"
                                            Type="Integer"
                                            Operator="DataTypeCheck"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Sort Order Invalid" />

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:TextBox ID="txtNewExtraProductSortOrder" CssClass="form-field footer-textbox" MaxLength="6" runat="server" Width="30px"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductSortOrder"
                                            Display="Static"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Sort Order Required" />
                                        <asp:CompareValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductSortOrder"
                                            Display="dynamic"
                                            Type="Integer"
                                            Operator="DataTypeCheck"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Sort Order Invalid" />

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="PageVisibility" text="Page Visibility" 
                                            CssClass="gdv_linkButton" OnCommand="gdvExtraProducts_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblNewExtraProductPageVisibility" text='<%# GetPageVisibilityString(DirectCast(GetDataItem(), ExtraProductLouvres).PageVisibility) %>' runat="server" />
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <div class="no-wrap"><asp:CheckBoxList ID="cblNewExtraProductPageVisibility" runat="server"></asp:CheckBoxList></div>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div class="no-wrap"><asp:CheckBoxList ID="cblNewExtraProductPageVisibility" runat="server"></asp:CheckBoxList></div>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="VisibilityLevel" text="Visibility Level" 
                                            CssClass="gdv_linkButton" OnCommand="gdvExtraProducts_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblNewExtraProductVisibilityLevel" text='<%# GetVisibilityLevelString(DirectCast(GetDataItem(), ExtraProductLouvres).VisibilityLevel) %>' runat="server" />
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <div class="no-wrap"><asp:CheckBoxList ID="cblNewExtraProductVisibilityLevel" runat="server"></asp:CheckBoxList></div>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div class="no-wrap"><asp:CheckBoxList ID="cblNewExtraProductVisibilityLevel" runat="server"></asp:CheckBoxList></div>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="DeductionWidth" text="Deduction Width (mm)" 
                                            CssClass="gdv_linkButton" OnCommand="gdvExtraProducts_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" text='<%# Eval("DeductionWidth") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <asp:TextBox ID="txtNewExtraProductDeductionWidth" CssClass="form-field footer-textbox" text='<%# Eval("DeductionWidth") %>' MaxLength="4" runat="server" Width="30px"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductDeductionWidth"
                                            Display="Static"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Deduction Width Required" />
                                        <asp:CompareValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductDeductionWidth"
                                            Display="dynamic"
                                            Type="Integer"
                                            Operator="DataTypeCheck"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Deduction Width Invalid" />

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:TextBox ID="txtNewExtraProductDeductionWidth" CssClass="form-field footer-textbox" MaxLength="4" runat="server" Width="30px"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductDeductionWidth"
                                            Display="Static"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Deduction Width Required" />
                                        <asp:CompareValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtNewExtraProductDeductionWidth"
                                            Display="dynamic"
                                            Type="Integer"
                                            Operator="DataTypeCheck"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Deduction Width Invalid" />

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Supplier" text="Supplier Name" 
                                            CssClass="gdv_linkButton" OnCommand="gdvExtraProducts_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" text='<%# Eval("Supplier") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <asp:TextBox ID="txtSupplier" CssClass="form-field footer-textbox" text='<%# Eval("Supplier") %>' MaxLength="60" runat="server" placeholder="Supplier"></asp:TextBox>
                                        <%--commented by surendra date:18/11/2020 ticket #62230--%>
                                        <%--<asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtSupplier"
                                            Display="Static"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Supplier Required" />--%>

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:TextBox ID="txtSupplier" CssClass="form-field footer-textbox" MaxLength="50" runat="server"></asp:TextBox>

                                       <%-- <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text"
                                            ControlToValidate="txtSupplier"
                                            Display="Static"
                                            ValidationGroup="NewExtraProduct"
                                            Text="Supplier Required" />--%>

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="AppendColourCode" text="Append Colour Code" 
                                            CssClass="gdv_linkButton" OnCommand="gdvExtraProducts_LinkSort" />
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chkAppendColourCode" runat="server" Enabled="true" Checked='<%# Eval("AppendColourCode") %>' CssClass="checkbox" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAppendColourCode" runat="server" Enabled="false" Checked='<%# Eval("AppendColourCode") %>' CssClass="checkbox" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="chkAppendColourCode" runat="server" Enabled="true" Checked='false' CssClass="checkbox" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Discontinued" text="Discontinued" 
                                            CssClass="gdv_linkButton" OnCommand="gdvExtraProducts_LinkSort" />
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chkDiscontinued" runat="server" Enabled="true" Checked='<%# Eval("Discontinued") %>' CssClass="checkbox" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDiscontinued" runat="server" Enabled="false" Checked='<%# Eval("Discontinued") %>' CssClass="checkbox" />
                                    </ItemTemplate>
                                    <FooterTemplate>

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <asp:Button ID="btnEditAddressZone" runat="server" CssClass="edit-button" 
                                            CommandName="edit" ToolTip="Edit" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnEditLouvreExtraPrice" runat="server" CssClass="prices-button" CommandArgument='<%# Eval("ExtraProductID") %>' 
                                            CommandName="EditExtraProductPrices" Width="32px" Height="32px" ToolTip="Prices" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button ID="btnSaveNewExtraProduct" runat="server" CssClass="save-button" tooltip="Save" 
                                            UseSubmitBehavior="false" CommandName="update" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnCancelNewExtraProduct" runat="server" CssClass="cancel-button" tooltip="Cancel" 
                                            UseSubmitBehavior="false" CommandName="cancel" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Button ID="btnSaveNewExtraProduct" runat="server" CssClass="save-button" tooltip="Save" 
                                            UseSubmitBehavior="false" CommandName="SaveNewExtraProduct" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnCancelNewExtraProduct" runat="server" CssClass="cancel-button" tooltip="Cancel" 
                                            UseSubmitBehavior="false" CommandName="CancelNewExtraProduct" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </ext:GridViewExtended>

                        <br />
                        <asp:Button ID="btnAddExtraProduct" runat="server" CssClass="form-button update-disable" Text="New Extra Product" UseSubmitBehavior="false" />
                        <br />
                        <br />

                    </asp:Panel>

                    <asp:Panel ID="pnlPermissions" runat="server" CssClass="admin-panel">

                        <h2>Permissions</h2>  

                        <br />
                        <br />

                        <asp:Panel ID="pnlPermissionsSearchCriteria" runat="server" >
                            Username&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlPermissionsSearchCriteriaUser" CssClass="form-field" Width="200px" runat="server" AutoPostBack="true" ></asp:DropDownList>
                        </asp:Panel>

                        <br />
                        <br />
                        <br />

                        <ext:GridViewExtended ID="gdvPermissions" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CellPadding="4" 
                                    CssClass="logins-table" ForeColor="#333333" Width="40%" AllowSorting="false" SortColumn="" SortDirection="" ShowFooter="false"
                                    ShowHeaderWhenEmpty="true" ShowFooterWhenEmpty="true" >
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="43" />
                            <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="Green" Font-Bold="false" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>

                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPermissionID" runat="server" text='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Category
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPermissionCategory" runat="server" text='<%# Eval("Category") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Permission Name
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPermissionName" runat="server" text='<%# Eval("Name") %>' />
                                        <asp:Image ID="imgTipPermissionDescription" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" 
                                            ToolTip='<%# Eval("Description") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Has Permission
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="lblPermissionEnabled" runat="server" CssClass="checkbox" Checked='<%# UserHasPermissionChecked(DirectCast(GetDataItem(), Permission).ID) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </ext:GridViewExtended>

                        <br />
                        <asp:Button ID="btnSavePermissions" runat="server" CssClass="form-button update-disable" Text="Save" UseSubmitBehavior="false" />
                        <br />
                        <br />

                    </asp:Panel>

                    <asp:Panel ID="pnlLog" runat="server" CssClass="admin-panel">

                        <h2>Log</h2>

                        <br />
                        <br />

                        <asp:Panel ID="pnlLogSearchCriteria" runat="server" >
                            
                        </asp:Panel>

                        <br />
                        <br />
                        <br />

                        <ext:GridViewExtended ID="gdvLog" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CellPadding="4" 
                                    CssClass="log-table" ForeColor="#333333" Width="90%" AllowSorting="true" SortColumn="" SortDirection="" 
                                    ShowHeaderWhenEmpty="true" ShowFooterWhenEmpty="true" >
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="break-word" />
                            <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="Green" Font-Bold="false" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="ID" Visible="false" />
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="SiteID" SortExpression="SiteID" HeaderText="Site ID" />

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="User" text="User" 
                                            CssClass="gdv_linkButton" OnCommand="gdvLog_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:label runat="server" text='<%# GetLoginNameByLoginID(DirectCast(GetDataItem(), LogEntry).UserID, DirectCast(GetDataItem(), LogEntry).SiteID) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="CategoryID" SortExpression="Category" HeaderText="Category" />
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="ChangeTypeID" SortExpression="ChangeType" HeaderText="Change Type" />
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="BeforeChange" SortExpression="BeforeChange" HeaderText="Before Change" />
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="AfterChange" SortExpression="AfterChange" HeaderText="After Change" />
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="AdditionalInfo" SortExpression="AdditionalInfo" HeaderText="Additional Info" />
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="CreationDateTime" SortExpression="CreationDateTime" HeaderText="Date & Time" />
                            </Columns>
                        </ext:GridViewExtended>

                        <br />
                        <br />

                    </asp:Panel>

                    <asp:HiddenField ID="hidPopupDummy" runat="server" />

                    <ajaxcontroltoolkit:ModalPopupExtender ID="mpeLogin" runat="server" TargetControlID="hidPopupDummy" PopupControlID="pnlLoginPopup" BackgroundCssClass="modalBackground">
                    </ajaxcontroltoolkit:ModalPopupExtender>

                    <asp:Panel ID="pnlLoginPopup" runat="server" CssClass="modalPopup-loginpopup" Style="display: none;">
                        <h2><asp:Label ID="lblLoginPopupHeader" runat="server" /></h2>

                        <table class="form-table">

                            <tr>
                                <td class="form-field-td-p2" style="text-align: right; width:30%;">Username 
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtLoginUserName" runat="server" CssClass="form-field" Width="300px" MaxLength="50" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td style="text-align: left;">
                                    <asp:RequiredFieldValidator ID="valrfUsername" runat="server" ControlToValidate="txtLoginUserName" Display="Dynamic" 
                                        ErrorMessage="Please enter a username." ValidationGroup="loginpopup"  
                                        EnableClientScript="true" CssClass="validation" />
                                    <asp:CustomValidator ID="valcustUsername" runat="server" 
                                        Display="Dynamic" Text="" 
                                        ValidationGroup="loginpopup"
                                        ControlToValidate="txtLoginUserName"
                                        CssClass="validation" />
                                </td>
                            </tr>

                            <tr>
                                <td class="form-field-td-p2" style="text-align: right;">First Name 
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtLoginFirstName" runat="server" CssClass="form-field" Width="300px" MaxLength="50" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td style="text-align: left;">
                                    <asp:RequiredFieldValidator ID="valrfFirstName" runat="server" ControlToValidate="txtLoginFirstName" Display="Dynamic" 
                                        ErrorMessage="Please enter a first name." ValidationGroup="loginpopup"  
                                        EnableClientScript="true" CssClass="validation" />
                                </td>
                            </tr>

                            <tr>
                                <td class="form-field-td-p2" style="text-align: right;">Last Name 
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtLoginLastName" runat="server" CssClass="form-field" Width="300px" MaxLength="50" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td style="text-align: left;">
                                    <asp:RequiredFieldValidator ID="valrfLastName" runat="server" ControlToValidate="txtLoginLastName" Display="Dynamic" 
                                        ErrorMessage="Please enter a last name." ValidationGroup="loginpopup"  
                                        EnableClientScript="true" CssClass="validation" />
                                </td>
                            </tr>

                            <tr>
                                <td class="form-field-td-p2" style="text-align: right;">Email 
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtLoginEmail" runat="server" CssClass="form-field" Width="300px" MaxLength="50" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td style="text-align: left;">
                                    <asp:RequiredFieldValidator ID="valrfEmail" runat="server" ControlToValidate="txtLoginEmail" Display="Dynamic" 
                                        ErrorMessage="Please enter an email." ValidationGroup="loginpopup"  
                                        EnableClientScript="true" CssClass="validation" />
                                </td>
                            </tr>

                            <tr>
                                <td class="form-field-td-p2" style="text-align: right;">Customer 
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:DropDownList ID="ddlLoginPopupCustomer" runat="server" CssClass="form-field" Width="310px" AutoPostBack="true" ></asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td class="form-field-td-p2" style="text-align: right;">Delivery Address 
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:DropDownList ID="ddlDeliveryAddress" runat="server" CssClass="form-field" Width="310px" ></asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td class="form-field-td-p2" style="text-align: center; padding-top:10px;" colspan="2">
                                    <asp:CheckBox ID="chkLoginIsAdmin" runat="server" Text="" CssClass="checkbox admin-chk"></asp:CheckBox>Administrator
                                    <br />
                                    <asp:CheckBox ID="chkLoginIsDiscontinued" runat="server" Text="" CssClass="checkbox discontinued-chk"></asp:CheckBox>Discontinued
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2">
                                    <asp:Button runat="server" ID="btnSendPasswordResetEmail" Text="Send Password Reset Email" CssClass="form-button reset-button" UseSubmitBehavior="false" />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2">
                                    <br />
                                    <br />
                                    <asp:label runat="server" ID="lblLoginPopupStatus" UseSubmitBehavior="false" Width="200" />
                                </td>
                            </tr>
                        </table>

                        <br />
                        <br />
                        <div style="text-align: center;">
                            <asp:Button runat="server" ID="btnCancelLogin" Text="Cancel" CssClass="form-button" UseSubmitBehavior="false" Width="100px" />
                            &nbsp;&nbsp;
                            <asp:Button runat="server" ID="btnSaveLogin" Text="Save" CssClass="form-button" UseSubmitBehavior="false" Width="100px" />
                        </div>
                        <br />
                    </asp:Panel>

                    <ajaxcontroltoolkit:ModalPopupExtender ID="mpeAddressZone" runat="server" TargetControlID="hidPopupDummy" PopupControlID="pnlAddressZonePopup" BackgroundCssClass="modalBackground">
                    </ajaxcontroltoolkit:ModalPopupExtender>

                    <asp:Panel ID="pnlAddressZonePopup" runat="server" CssClass="modalPopup-AddressZoneRanges" Style="display: none;">
                        <h2><asp:Label ID="lblAddressZoneHeader" runat="server" /></h2>

                        <ext:GridViewExtended ID="gdvAddressZoneRanges" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CellPadding="4" 
                                    CssClass="logins-table" ForeColor="#333333" Width="90%" AllowSorting="true" SortColumn="" SortDirection="" 
                                    ShowHeaderWhenEmpty="true" ShowFooterWhenEmpty="true" >
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="Green" Font-Bold="false" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="ID" Visible="false" />
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="AddressZoneID" Visible="false" />
                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="Start" SortExpression="Start" HeaderText="Start Postcode" />

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        -
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField HeaderStyle-Font-Bold="false" DataField="End" SortExpression="End" HeaderText="End Postcode" />

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnDeleteAddressZoneRange" runat="server" CssClass="delete-button" 
                                            CommandArgument='<%# Eval("ID") %>' CommandName="DeleteAddressZoneRange" Width="32px" Height="32px" 
                                            OnClientClick="return confirm('Delete this address zone range?');" ToolTip="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </ext:GridViewExtended>

                        <br />

                        <asp:Panel ID="pnlAddAddressZoneRange" runat="server" Visible="false">

                            <asp:TextBox ID="txtAddAddressZoneRangeStartPostcode"  MaxLength="4" runat="server"></asp:TextBox> - <asp:TextBox ID="txtAddAddressZoneRangeEndPostcode" MaxLength="4" runat="server"></asp:TextBox>
                            <asp:Button ID="btnSaveAddressZoneRange" runat="server" CssClass="save-button" tooltip="Save" UseSubmitBehavior="false" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnCancelAddressZoneRange" runat="server" CssClass="cancel-button" tooltip="Cancel" UseSubmitBehavior="false" />
                            
                        </asp:Panel>

                        <br />

                        <asp:Button ID="btnAddAddressZoneRange" runat="server" CssClass="form-button update-disable" Text="Create New Range" UseSubmitBehavior="false" />

                        <br />
                        <br />

                        <div style="text-align: center;">
                            <asp:Button runat="server" ID="btnCloseAddressZoneRangePopup" Text="Close" CssClass="form-button" UseSubmitBehavior="false" Width="100px" />
                        </div>
                        <br />
                    </asp:Panel>

                    <ajaxcontroltoolkit:ModalPopupExtender ID="mpeLouvreExtraPrices" runat="server" TargetControlID="hidPopupDummy" PopupControlID="pnlLouvreExtraPricesPopup" BackgroundCssClass="modalBackground">
                    </ajaxcontroltoolkit:ModalPopupExtender>

                    <asp:Panel ID="pnlLouvreExtraPricesPopup" runat="server" CssClass="modalPopup-LouvreExtraPrices" Style="display: none;">
                        <h2><asp:Label ID="lblLouvreExtraPricesHeader" runat="server" /></h2>

                        <br />
                        <br />

                        <asp:Panel ID="pnlLouvreExtraPricesSearchCriteria" runat="server" >
                            <asp:CheckBox ID="chkLouvreExtraPricesShowDiscontinued" runat="server" Text="" AutoPostBack="true" CssClass="checkbox" />Include Discontinued Prices
                        </asp:Panel>

                        <br />
                        <br />
                        <br />

                        <ext:GridViewExtended ID="gdvLouvreExtraPrices" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CellPadding="4" 
                                    CssClass="logins-table" ForeColor="#333333" Width="90%" AllowSorting="true" SortColumn="" SortDirection="" ShowHeaderWhenEmpty="true" ShowFooterWhenEmpty="true" >
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="43px" />
                            <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="Green" Font-Bold="false" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>

                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:label ID="lblLouvreExtraPriceID" runat="server" Text='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:label ID="lblLouvreExtraPriceExtraProductID" runat="server" Text='<%# Eval("ExtraProductID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="UnitPrice" text="Unit Price" 
                                            CssClass="gdv_linkButton" OnCommand="gdvLouvreExtraPrices_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:label ID="lblLouvreExtraPriceUnitPrice" runat="server" Text='<%# FormatCurrency(Eval("UnitPrice")) %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <asp:TextBox ID="txtNewLouvreExtraPriceUnitPrice" CssClass="form-field footer-textbox" runat="server" Text='<%# Format(Eval("UnitPrice"), "0.00") %>' MaxLength="10"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceUnitPrice"
                                            Display="static"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Unit Price Required" />
                                        <asp:CompareValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceUnitPrice"
                                            Display="Dynamic"
                                            Type="Currency"
                                            Operator="DataTypeCheck"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Unit Price Invalid" />

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:TextBox ID="txtNewLouvreExtraPriceUnitPrice" CssClass="form-field footer-textbox" runat="server" MaxLength="10"></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceUnitPrice"
                                            Display="static"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Unit Price Required" />
                                        <asp:CompareValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceUnitPrice"
                                            Display="Dynamic"
                                            Type="Currency"
                                            Operator="DataTypeCheck"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Unit Price Invalid" />

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Unit" text="Unit" 
                                            CssClass="gdv_linkButton" OnCommand="gdvLouvreExtraPrices_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlLouvreExtraPriceUnitDescription" CssClass="form-field footer-textbox" runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <asp:DropDownList ID="ddlLouvreExtraPriceUnitDescription" CssClass="form-field footer-textbox"  runat="server"></asp:DropDownList>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="ddlLouvreExtraPriceUnitDescription"
                                            Display="static"
                                            InitialValue="0"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Unit Required" />

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:DropDownList ID="ddlLouvreExtraPriceUnitDescription" CssClass="form-field footer-textbox" runat="server"></asp:DropDownList>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="ddlLouvreExtraPriceUnitDescription"
                                            Display="static"
                                            InitialValue="0"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Unit Required" />

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="PriceisPercent" text="Price is Percent" 
                                            CssClass="gdv_linkButton" OnCommand="gdvLouvreExtraPrices_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkLouvreExtraPricePriceIsPercentage" runat="server" Enabled="false" Checked='<%# Eval("PriceIsPercentage") %>' CssClass="checkbox" />
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <asp:CheckBox ID="chkNewLouvreExtraPricePriceIsPercentage" CssClass="footer-textbox checkbox" Checked='<%# Eval("PriceIsPercentage") %>' runat="server"></asp:CheckBox>

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:CheckBox ID="chkNewLouvreExtraPricePriceIsPercentage" CssClass="footer-textbox checkbox" runat="server"></asp:CheckBox>

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="UnitSize" text="Unit Size" 
                                            CssClass="gdv_linkButton" OnCommand="gdvLouvreExtraPrices_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtUnitSize" runat="server" Enabled="false" CssClass="form-field footer-textbox" text='<%# Eval("UnitSize") %>' Width="50px" />
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <asp:TextBox ID="txtNewLouvreExtraPriceUnitSize" CssClass="form-field footer-textbox" runat="server" text='<%# Eval("UnitSize") %>' Width="50px" MaxLength="4" ></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceUnitSize"
                                            Display="static"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Unit Size Required" />
                                        <asp:CompareValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceUnitSize"
                                            Display="Dynamic"
                                            Type="Integer"
                                            Operator="DataTypeCheck"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Unit Size Invalid" />

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:TextBox ID="txtNewLouvreExtraPriceUnitSize" CssClass="form-field footer-textbox" runat="server" Width="50px" MaxLength="4" ></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceUnitSize"
                                            Display="static"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Unit Size Required" />
                                        <asp:CompareValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceUnitSize"
                                            Display="Dynamic"
                                            Type="Integer"
                                            Operator="DataTypeCheck"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Unit Size Invalid" />

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="MinimumCharge" text="Minimum Charge" 
                                            CssClass="gdv_linkButton" OnCommand="gdvLouvreExtraPrices_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:label ID="lblMinimumCharge" runat="server" Text='<%# FormatCurrency(Eval("MinimumCharge")) %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <asp:TextBox ID="txtNewLouvreExtraPriceMinimumCharge" CssClass="form-field footer-textbox" Text='<%# Format(Eval("MinimumCharge"), "0.00") %>'  MaxLength="10" runat="server"  ></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceMinimumCharge"
                                            Display="static"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Minimum Charge Required" />
                                        <asp:CompareValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceMinimumCharge"
                                            Display="Dynamic"
                                            Type="Currency"
                                            Operator="DataTypeCheck"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Minimum Charge Invalid" />

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:TextBox ID="txtNewLouvreExtraPriceMinimumCharge" CssClass="form-field footer-textbox"  MaxLength="10" runat="server"  ></asp:TextBox>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceMinimumCharge"
                                            Display="static"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Minimum Charge Required" />
                                        <asp:CompareValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceMinimumCharge"
                                            Display="Dynamic"
                                            Type="Currency"
                                            Operator="DataTypeCheck"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Minimum Charge Invalid" />

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" Width="100px" />
                                    <FooterStyle Font-Bold="false" Width="100px" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="PricingCategory" text="Pricing Category" 
                                            CssClass="gdv_linkButton" OnCommand="gdvLouvreExtraPrices_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlLouvreExtraPriceCategoryID" CssClass="form-field footer-textbox" runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <asp:DropDownList ID="ddlLouvreExtraPriceCategoryID" CssClass="form-field footer-textbox" runat="server"></asp:DropDownList>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="ddlLouvreExtraPriceCategoryID"
                                            Display="static"
                                            InitialValue="0"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Pricing Category Required" />

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:DropDownList ID="ddlLouvreExtraPriceCategoryID" CssClass="form-field footer-textbox" runat="server"></asp:DropDownList>

                                        <asp:RequiredFieldValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="ddlLouvreExtraPriceCategoryID"
                                            Display="static"
                                            InitialValue="0"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            Text="Pricing Category Required" />

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Discontinued" text="Discontinued" 
                                            CssClass="gdv_linkButton" OnCommand="gdvLouvreExtraPrices_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDiscontinued" runat="server" Enabled="false" Checked='<%# Eval("Discontinued") %>' CssClass="checkbox" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chkDiscontinued" runat="server" Enabled="true" Checked='<%# Eval("Discontinued") %>' CssClass="checkbox" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Effective" text="Effective" 
                                            CssClass="gdv_linkButton" OnCommand="gdvLouvreExtraPrices_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" text='<%# Eval("EffectiveDateTime") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <asp:TextBox ID="txtNewLouvreExtraPriceEffectiveDateTime" CssClass="form-field footer-textbox dtePicker" text='<%# Eval("EffectiveDateTime") %>' MaxLength="50" runat="server"></asp:TextBox>

                                        <asp:CustomValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceEffectiveDateTime"
                                            Display="static"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            ValidateEmptyText="true"
                                            EnableClientScript="true"
                                            OnServerValidate="valcustNewLouvreExtraPriceEffectiveDateTime_Validate"
                                            Text="Effective Date Invalid" /> 

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <asp:TextBox ID="txtNewLouvreExtraPriceEffectiveDateTime" CssClass="form-field footer-textbox dtePicker" MaxLength="50" runat="server"></asp:TextBox>

                                        <asp:CustomValidator runat="server" 
                                            CssClass="validation-text no-wrap"
                                            ControlToValidate="txtNewLouvreExtraPriceEffectiveDateTime"
                                            Display="static"
                                            ValidationGroup="NewLouvreExtraPrice"
                                            ValidateEmptyText="true"
                                            EnableClientScript="true"
                                            OnServerValidate="valcustNewLouvreExtraPriceEffectiveDateTime_Validate"
                                            Text="Effective Date Invalid" /> 

                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Font-Bold="false" />
                                    <HeaderTemplate>
                                        <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Created" text="Created" 
                                            CssClass="gdv_linkButton" OnCommand="gdvLouvreExtraPrices_LinkSort" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" text='<%# Eval("CreationDateTime") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle Width="100px" />
                                    <FooterStyle Width="100px" />
                                    <ItemTemplate>
                                        <asp:Button ID="btnEditLouvreExtraPrice" runat="server" CssClass="edit-button" 
                                            CommandName="edit" ToolTip="Edit" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button ID="btnSaveNewLouvreExtraPrice" runat="server" CssClass="save-button" tooltip="Save" 
                                            UseSubmitBehavior="false" CommandName="update" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnCancelNewLouvreExtraPrice" runat="server" CssClass="cancel-button" tooltip="Cancel" 
                                            UseSubmitBehavior="false" CommandName="cancel" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div style="width: 100px !important;" />
                                            <asp:Button ID="btnSaveNewLouvreExtraPrice" runat="server" CssClass="save-button"  ToolTip="Save" UseSubmitBehavior="false" 
                                                CommandName="SaveNewLouvreExtraPrice" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnCancelNewLouvreExtraPrice" runat="server" CssClass="cancel-button"  ToolTip="Cancel" UseSubmitBehavior="false" 
                                                CommandName="CancelNewLouvreExtraPrice" />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </ext:GridViewExtended>

                        <asp:Label ID="lblNewLouvreExtraPriceStatus" runat="server" Visible="false" />

                        <br />
                        <br />

                        <asp:Button ID="btnAddLouvreExtraPrice" runat="server" CssClass="form-button update-disable" Text="Create New Price" UseSubmitBehavior="false" />

                        <br />
                        <br />

                        <div style="text-align: center;">
                            <asp:Button runat="server" ID="btnCloseLouvreExtraPricePopup" Text="Close" CssClass="form-button" UseSubmitBehavior="false" Width="100px" />
                        </div>
                        <br />
                    </asp:Panel>

                    <asp:Panel ID="pnlLouvrePrices" runat="server">
                        <h2><asp:Label ID="lblLouvrePricesHeader" runat="server" />Louvre Prices</h2>

                        <br />
                        <br />

                        <asp:Panel ID="pnlLouvrePricesSearchCriteria" runat="server" CssClass="louvreprices-searchfiltertable" >
                            <table style="margin-left:auto;margin-right:auto;">
                                <tr>
                                    <td class="criteria-description">Price Category</td>
                                    <td class="criteria-contol">
                                        <asp:DropDownList ID="ddlLouvrePriceCategory" runat="server" AutoPostBack="false" CssClass="form-select" Width="200px" />
                                        <asp:RequiredFieldValidator ID="valrfLouvrePriceCategory" runat="server" ControlToValidate="ddlLouvrePriceCategory" 
                                            Display="Dynamic" EnableClientScript="false" ValidationGroup="LouvrePrice" InitialValue="0"  Text="Category required." CssClass="validation-text"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="criteria-description">Louvre Style</td>
                                    <td class="criteria-contol">
                                        <asp:DropDownList ID="ddlLouvrePriceStyle" runat="server" AutoPostBack="false" CssClass="form-select" />
                                        <asp:RequiredFieldValidator ID="valrfLouvrePriceStyle" runat="server" ControlToValidate="ddlLouvrePriceStyle" 
                                            Display="Dynamic" EnableClientScript="false" ValidationGroup="LouvrePrice" InitialValue="0"  Text="Style  required." CssClass="validation-text"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="criteria-description">Coating Type</td>
                                    <td class="criteria-contol">
                                        <asp:DropDownList ID="ddlLouvrePriceCoatingType" runat="server" AutoPostBack="false" CssClass="form-select" Width="200px" />
                                        <asp:RequiredFieldValidator ID="valrfLouvrePriceCoatingType" runat="server" ControlToValidate="ddlLouvrePriceCoatingType" 
                                            Display="Dynamic" EnableClientScript="false" ValidationGroup="LouvrePrice" InitialValue="0"  Text="Coating type required." CssClass="validation-text"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="criteria-description">Louvre Type</td>
                                    <td class="criteria-contol">
                                        <asp:DropDownList ID="ddlLouvrePriceType" runat="server" AutoPostBack="false" CssClass="form-select" Width="200px" />
                                        <asp:RequiredFieldValidator ID="valrfLouvrePriceType" runat="server" ControlToValidate="ddlLouvrePriceType" 
                                            Display="Dynamic" EnableClientScript="false" ValidationGroup="LouvrePrice" InitialValue="0"  Text="Louvre type required." CssClass="validation-text"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="criteria-description">Effective Date</td>
                                    <td class="criteria-contol">
                                        <asp:TextBox ID="txtLouvrePriceDate" runat="server" AutoPostBack="false" CssClass="form-select dtePicker" Width="100px" />
                                        <asp:CustomValidator ID="valcustDate" runat="server" ControlToValidate="txtLouvrePriceDate" EnableClientScript="false" 
                                            ValidationGroup="LouvrePrice" Text="Date not valid." CssClass="validation-text" ValidateEmptyText="true"></asp:CustomValidator>
                                    </td>
                                </tr>
                            </table>

                            <br />

                            <asp:Button ID="btnLouvrePriceSubmitSearch" runat="server" Text="Search" CssClass="form-button" />

                            <br />
                            <br />
                            <br />

                            <asp:Panel ID="pnlLouvrePriceUploadFiles" runat="server" CssClass="file-upload-panel-louvreprices" Visible="true" >

                                <asp:label ID="lblLouvrePriceFileDropHeader" runat="server" CssClass="LouvrePriceFileDropHeader" Text="Drop .xlsx document below to add new records to the louvre price table...<br /><br />"></asp:label>

                                    <ajaxcontroltoolkit:AjaxFileUpload 
                                        ID="afuLouvrePriceAjaxUploader" 
                                        MaximumNumberOfFiles="10"
                                        AllowedFileTypes="xls,xlsx"
                                        MaxFileSize="<%# SharedConstants.MAX_UPLOAD_FILE_SIZE %>"
                                        AutoStartUpload="true"
                                        CssClass="file-uploader"
                                        OnClientUploadComplete="afuLouvrePriceAjaxUploader_UploadComplete"
                                        runat="server" />

                                <asp:Button ID="btnLouvrePricesUploaded" runat="server" CssClass="hidden" />

                                </asp:Panel>

                                <asp:Label ID="lblLouvrePriceFileUploadStatus" runat="server"></asp:Label>

                        </asp:Panel>

                        <br />
                        <br />
                        <br />

                        <div style="text-align:center;width:100%">
                            <ext:GridViewExtended ID="gdvLouvrePrice" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CellPadding="4" 
                                        CssClass="louvreprices-table" ForeColor="#333333" Width="90%" AllowSorting="false" SortColumn="" SortDirection="" 
                                        ShowHeaderWhenEmpty="true" ShowFooterWhenEmpty="false" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="44" />
                                <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="Green" Font-Bold="false" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLouvrePriceID" runat="server" text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Effective Date
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEffectiveDate" runat="server" text='<%# Eval("EffectiveDateTime") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Price Category
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCategoryID" runat="server" text='<%# GetPriceCategoryStringForID(CInt(Eval("CategoryID"))) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            LouvreStyleID
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLouvreStyleID" runat="server" text='<%# GetLouvreStyleStringForID(CInt(Eval("LouvreStyleID"))) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            LouvreTypeID
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLouvreTypeID" runat="server" text='<%# GetLouvreTypeStringForID(CInt(Eval("LouvreTypeID"))) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            CoatingTypeID
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCoatingTypeID" runat="server" text='<%# GetCoatingTypeStringForID(CInt(Eval("CoatingTypeID"))) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Height
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblHeight" runat="server" text='<%# Eval("Height") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Width
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblWidth" runat="server" text='<%# Eval("Width") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Sale Price
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalePrice" runat="server" text='<%# FormatCurrency(Eval("SalePrice")) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </ext:GridViewExtended>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlColours" runat="server">
                        <h2>Colours</h2>

                        <br />
                        <br />

                        <asp:Panel ID="pnlColoursSearchCriteria" runat="server" >
                            <asp:CheckBox ID="chkColoursShowDiscontinued" runat="server" Text="" AutoPostBack="true" CssClass="checkbox" />Include Discontinued Colours
                        </asp:Panel>

                        <br />
                        <br />
                        <br />

                        <div style="text-align:center;width:100%">
                            <ext:GridViewExtended ID="gdvColours" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CellPadding="4" 
                                        CssClass="louvreprices-table" ForeColor="#333333" Width="90%" AllowSorting="false" SortColumn="" SortDirection="" 
                                        ShowHeaderWhenEmpty="true" ShowFooterWhenEmpty="false" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="44" />
                                <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="Green" Font-Bold="false" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblColourID" runat="server" text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Name" text="Colour Name" 
                                            CssClass="gdv_linkButton" OnCommand="gdvColours_LinkSort" />
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:Label ID="lblColourName" runat="server" text='<%# Eval("Name") %>' />
                                        </ItemTemplate>

                                        <EditItemTemplate>

                                            <asp:TextBox ID="txtColourName" CssClass="form-field footer-textbox" runat="server" Text='<%# Eval("Name") %>' MaxLength="100"></asp:TextBox>

                                            <asp:RequiredFieldValidator runat="server" 
                                                CssClass="validation-text no-wrap"
                                                ControlToValidate="txtColourName"
                                                Display="static"
                                                ValidationGroup="NewColour"
                                                Text="Colour Name Required" />

                                        </EditItemTemplate>

                                        <FooterTemplate>
                                            <asp:TextBox ID="txtColourName" CssClass="form-field footer-textbox" runat="server" Text='<%# Eval("Name") %>' MaxLength="100"></asp:TextBox>

                                            <asp:RequiredFieldValidator runat="server" 
                                                CssClass="validation-text no-wrap"
                                                ControlToValidate="txtColourName"
                                                Display="static"
                                                ValidationGroup="NewColour"
                                                Text="Colour Name Required" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="CoatingType" text="Coating Type" 
                                            CssClass="gdv_linkButton" OnCommand="gdvColours_LinkSort" />
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlCoatingType" CssClass="form-field footer-textbox" runat="server" Enabled="false"></asp:DropDownList>
                                        </ItemTemplate>

                                        <EditItemTemplate>

                                            <asp:DropDownList ID="ddlCoatingType" CssClass="form-field footer-textbox" runat="server"></asp:DropDownList>

                                            <asp:RequiredFieldValidator runat="server" 
                                                CssClass="validation-text no-wrap"
                                                ControlToValidate="ddlCoatingType"
                                                Display="static"
                                                InitialValue="0"
                                                ValidationGroup="NewColour"
                                                Text="Coating Type Required" />

                                        </EditItemTemplate>

                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlCoatingType" CssClass="form-field footer-textbox" runat="server"></asp:DropDownList>

                                            <asp:RequiredFieldValidator runat="server" 
                                                CssClass="validation-text no-wrap"
                                                ControlToValidate="ddlCoatingType"
                                                Display="static"
                                                InitialValue="0"
                                                ValidationGroup="NewColour"
                                                Text="Coating Type Required" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Discontinued" text="Discontinued" 
                                            CssClass="gdv_linkButton" OnCommand="gdvColours_LinkSort" />
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDiscontinued" runat="server" Checked='<%# Eval("Discontinued") %>' Enabled="false" />
                                        </ItemTemplate>

                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkDiscontinued" runat="server" Enabled="true" Checked='<%# Eval("Discontinued") %>' CssClass="checkbox" />
                                        </EditItemTemplate>

                                        <FooterTemplate>

                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="ColourCode" text="Colour Code" 
                                            CssClass="gdv_linkButton" OnCommand="gdvColours_LinkSort" />
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:Label ID="lblColourCode" runat="server" text='<%# Eval("ColourCode") %>' />
                                        </ItemTemplate>

                                        <EditItemTemplate>

                                            <asp:TextBox ID="txtColourCode" CssClass="form-field footer-textbox" runat="server" Text='<%# Eval("ColourCode") %>' MaxLength="50"></asp:TextBox>

                                            <asp:RequiredFieldValidator runat="server" 
                                                CssClass="validation-text no-wrap"
                                                ControlToValidate="txtColourCode"
                                                Display="static"
                                                ValidationGroup="NewColour"
                                                Text="Colour Code Required" />

                                        </EditItemTemplate>

                                        <FooterTemplate>
                                            <asp:TextBox ID="txtColourCode" CssClass="form-field footer-textbox" runat="server" Text='<%# Eval("ColourCode") %>' MaxLength="50"></asp:TextBox>

                                            <asp:RequiredFieldValidator runat="server" 
                                                CssClass="validation-text no-wrap"
                                                ControlToValidate="txtColourCode"
                                                Display="static"
                                                ValidationGroup="NewColour"
                                                Text="Colour Code Required" />
                                        </FooterTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="SortOrder" text="Sort Order" 
                                            CssClass="gdv_linkButton" OnCommand="gdvColours_LinkSort" />
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:Label ID="lblSortOrder" runat="server" text='<%# Eval("SortOrder") %>' />
                                        </ItemTemplate>

                                        <EditItemTemplate>

                                            <asp:TextBox ID="txtSortOrder" CssClass="form-field footer-textbox" runat="server" Text='<%# Eval("SortOrder") %>' MaxLength="10"></asp:TextBox>

                                            <asp:RequiredFieldValidator runat="server" 
                                                CssClass="validation-text no-wrap"
                                                ControlToValidate="txtSortOrder"
                                                Display="static"
                                                ValidationGroup="NewColour"
                                                Text="Sort Order Required" />

                                        </EditItemTemplate>

                                        <FooterTemplate>
                                            <asp:TextBox ID="txtSortOrder" CssClass="form-field footer-textbox" runat="server" Text='<%# Eval("SortOrder") %>' MaxLength="10"></asp:TextBox>

                                            <asp:RequiredFieldValidator runat="server" 
                                                CssClass="validation-text no-wrap"
                                                ControlToValidate="txtSortOrder"
                                                Display="static"
                                                ValidationGroup="NewColour"
                                                Text="Sort Order Required" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderStyle Width="100px" />
                                        <ItemTemplate>
                                            <asp:Button ID="btnEditColour" runat="server" CssClass="edit-button" CommandArgument='<%# Eval("ID") %>' 
                                                CommandName="Edit" Width="32px" Height="32px" ToolTip="Prices" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnSaveNewColour" runat="server" CssClass="save-button" tooltip="Save" 
                                                UseSubmitBehavior="false" CommandName="update" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnCancelNewColour" runat="server" CssClass="cancel-button" tooltip="Cancel" 
                                                UseSubmitBehavior="false" CommandName="cancel" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="btnSaveNewColour" runat="server" CssClass="save-button" tooltip="Save" 
                                                UseSubmitBehavior="false" CommandName="SaveNewColour" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnCancelNewColour" runat="server" CssClass="cancel-button" tooltip="Cancel" 
                                                UseSubmitBehavior="false" CommandName="CancelNewColour" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </ext:GridViewExtended>

                            <br />
                            <asp:Button ID="btnAddColour" runat="server" CssClass="form-button update-disable" Text="New Colour" UseSubmitBehavior="false" />
                            <br />
                            <br />

                        </div>
                    </asp:Panel>

                </div>
            </div>

            <div id="dialog-confirm" title="Please Confirm" >
                <p id="dialog-confirm-message" style="text-align: left;"></p>
            </div>

        </div>
    </form>
</body>
</html>
