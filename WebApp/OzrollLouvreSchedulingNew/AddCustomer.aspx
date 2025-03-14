﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddCustomer.aspx.vb" Inherits="AddCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxcontroltoolkit" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>OzRoll Add/Update Customer</title>
        
 <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta name="format-detection" content="telephone=no" />
    
    <link href="stylesheets/reset.css" rel="stylesheet" type="text/css" media="screen" />
    <!--[if IE]><link href="stylesheets/explorer.css" rel="stylesheet" type="text/css" media="screen" /><![endif]-->
    <link href="stylesheets/style.css?version=<%= Now.Year() & Now.Month & Now.Day() %>" rel="stylesheet" type="text/css" media="screen" />
    <link href="stylesheets/smoothness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />
  
    <!--[if lt IE 8]><script src="javascript/IE-fix.js" type="text/javascript"></script><![endif]-->
    <script src="javascript/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
   
    <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
    
    <script language="javascript" type="text/javascript">

        (function(document, navigator, standalone) {
            if ((standalone in navigator) && navigator[standalone]) {
                var curnode, location = document.location, stop = /^(a|html)$/i;
                document.addEventListener('click', function(e) {
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

        $(function() {
            $("#dialog-alert").dialog({
            modal: true,
            autoOpen: false,
            draggable: false,
            buttons: {
                "Ok": function() {
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
		        "Yes": function() {
		            $(this).dialog("close");
		            __doPostBack('<%=btnLogout.ClientID %>', '');
		            return true;
		        },
		        "No": function() {
		            $(this).dialog("close");
		            return false;
		        }
		    }
		    });
		
        });

	    if (window.history.forward(1) != null)
	        window.history.forward(1);

	    function validateABN(source, arguments) {
	        var isValid = true;
	        var weight = [10, 1, 3, 5, 7, 9, 11, 13, 15, 17, 19];
	        var weightedSum = 0;
	        var abn = arguments.Value;

	        // Remove spaces
	        abn = abn.replace(/\s+/g, '');
	        
	        // ABN must be 11 digits long
	        if (abn.length === 0) 
	        {
	            arguments.IsValid = false;
	            return;
	        }

	        if (abn.search(abn, /^\d{11}$/) )
	        {
	            arguments.IsValid = false;
	            return;
	        }

	        var newABN = abn.substring(1);
	        newABN = String((parseInt(abn.charAt(0)) - 1)) + newABN

	        // Rules: 1,2,3                                  
	        for (var i = 0; i < weight.length; i++)
	        {
	            weightedSum += parseInt(newABN.charAt(i)) * weight[i];
	        }

	        // Rules: 4,5                 
	        arguments.IsValid = Boolean((weightedSum % 89) === 0);
	    }

	    $(document).ready(function () {
	        styleCheckboxes();
	        hideSybizButtons();
	    });

	    function styleCheckboxes() {
	        $(".checkbox").addClass("checkbox-no-focus");

	        $(".checkbox").focusin(function () {
	            $(this).addClass("checkbox-focus");
	            $(this).removeClass("checkbox-no-focus");
	        });
	        $(".checkbox").focusout(function () {
	            $(this).removeClass("checkbox-focus");
	            $(this).addClass("checkbox-no-focus");
	        });
	    };

	    function hideSybizButtons() {
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_beginRequest(
                function (sender, args) {
                    $get("<%=btnAddCustomerToSybiz.ClientID %>").style.display = "none";
                    $get("<%=btnDoNotAddCustomerToSybiz.ClientID %>").style.display = "none";
                }
            );
            prm.add_endRequest(
                function (sender, args) {
                    $get("<%=btnAddCustomerToSybiz.ClientID %>").style.display = "";
                    $get("<%=btnDoNotAddCustomerToSybiz.ClientID %>").style.display = "";
                }
            );
	    };

        function ChangeSybizCode() {
            if (confirm('Changing the Sybiz Code will change this customers linked Sybiz account and any associated data. Are you sure?'))
            {
                $("#<%=txtCode.ClientID %>").removeAttr('disabled');
                $("#<%=btnChangeSybizCode.ClientID %>").attr('style', 'display:none;');
                $("#<%=btnSybizLink.ClientID %>").attr('style', 'display:inline;');
            }
            else
            {
                $("#<%=txtCode.ClientID %>").attr('disabled', 'disabled');
                $("#<%=btnChangeSybizCode.ClientID %>").attr('style', 'display:inline;');
                $("#<%=btnSybizLink.ClientID %>").attr('style', 'display:none;');
            }
        }


    </script>
    
    
    <style type="text/css">
        .auto-style5 {
            height: 50px;
        }
        .form-cell {
            background-color: #F2F2F2;
            color: #333333;
            font-family: Tahoma, "Trebuchet MS", Arial, sans-serif;
            font-size: 1em;
            line-height: 30px;
            padding: 5px 10px 5px 10px;
            text-align: left;
            vertical-align: middle;
        }
        .auto-style8 {
            width: 502px;
        }
        
        .address-field {
            margin: 5px;
        }
        .address-button {
            margin: 10px 5px 10px 5px;
            width: 100px;
        }
        .address-header th {
            padding: 10px;
        }
        .notes {
            width: 80%;
            margin: auto;
            margin: 20px auto 10px auto;
            background-color: #F2F2F2;
            padding: 1px 25px 15px 25px;
        }
        .valSummary {
            color: red;
            font: bold;
        }
        .valSummary ul {
            display: none;
            visibility: hidden;
        }
        .validation-cell {
            text-align: left;
            padding-left: 10px;
            padding-bottom : 5px;
        }

        .address-table th:first-child {
            border-top-left-radius: 1em;
        }

        .address-table th:last-child {
            border-top-right-radius: 1em;
        }

        .address-table tr:last-child td:last-child {
            border-bottom-right-radius: 1em;
        }

        .address-table tr:last-child td:first-child {
            border-bottom-left-radius: 1em;
        }

        .sybiz-popup {
            text-align: center; 
            width:1000px;
            background-color: white;
            padding: 20px 20px 60px 20px;
            border: solid 1px grey;
            box-shadow: 10px 10px 10px grey;
            border-radius: 10px;
        }

        .status-box {
            height: 100%; 
            text-align: center; 
            background-color:#F2F2F2; 
            width:80%;
            margin:0 auto; 
            padding:30px;
        }

        .change-button {
            padding: 6px 12px;
        }

    </style>
    
</head>
<body>
    <form runat="server" id="form1" method="post">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    <Scripts>
    <asp:ScriptReference Path="javascript/fixfocus.js" />
    </Scripts>
    </asp:ScriptManager>

    <div id="middle-container-diallerview" >

        <div id="logo" style="height: 100px; text-align: center;" ></div>
        <div style="height: 50px; text-align: center;">
        <asp:Button ID="btnHome" runat="server" CssClass="form-button" Visible="true" Text="Home" UseSubmitBehavior="false" TabIndex="-1" />&nbsp;&nbsp;
        <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="return ConfirmLeave();" UseSubmitBehavior="false" TabIndex="-1" />
	    </div>
    
    <h1>
        <asp:Label ID="lblTitle" runat="server"></asp:Label>
    </h1>
    

    <div style="height: 100%; text-align: center; border-radius: 1em;">
        <div style="display:inline-block; height: 100%; width: 1200px; text-align: center; background-color: #F2F2F2; box-shadow:10px 10px 10px grey; border-radius: 1em;">

            <asp:UpdatePanel ID="pnlupdateMain" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>

                    <asp:Panel ID="pnlMain" runat="server">

                    <h2 class="sub-heading">
                        <asp:Label ID="lblCustomerSubHeading" Text="Customer Details" runat="server"></asp:Label>
                    </h2>

                    <div style="height: 100%; text-align: center;">

                        <div style="display:inline-block; margin: 0 auto; padding-top: 10px; text-align: center;">

                            <table class="form-table" cellspacing="0" summary="">
                                <tr>
                                    <td class="form-cell" style="text-align: right;">Sybiz Code</td>
                                    <td class="form-cell" style="text-align: left;">
                                        <asp:TextBox ID="txtCode" runat="server" CssClass="form-field" Text="" MaxLength="20" Width="100px" TabIndex="1" Enabled="false" ></asp:TextBox>
                                        <asp:Image ID="imgSybizLinked" runat="server" ></asp:Image>
                                        <asp:Button ID="btnChangeSybizCode" Text="Change" runat="server" OnClientClick="ChangeSybizCode(); return false;" CssClass="form-button change-button" TabIndex="1" />
                                        <asp:Button ID="btnSybizLink" Text="Link" runat="server" style="display:none;" CssClass="form-button" TabIndex="1" />
                                    </td>
                                    <td class="form-cell" style="text-align: right;">Freight Percentage</td>
                                    <td class="form-cell" style="text-align: left;"> 
                                        <asp:TextBox ID="txtFreightPercentage" runat="server" CssClass="form-field" Text="" MaxLength="6" Width="60px"  TabIndex="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td class="validation-cell"></td>
                                    <td></td>
                                    <td class="validation-cell">
                                        <asp:RegularExpressionValidator runat="server" 
                                            ID="valregxFreightPercentage"
                                                ValidationExpression="^((\d{3})*|(\d{0,2}))(\.\d{0,2})?$"
                                                ControlToValidate="txtFreightPercentage" 
                                                Text="Freight Percentage is not valid.<br />" 
                                                ValidationGroup="customer"
                                                CssClass="validation-text"
                                                Display="Dynamic" />
                                        <asp:RangeValidator ID="valrangeFreightPercentage" 
                                            ControlToValidate="txtFreightPercentage" 
                                            ValidationGroup="customer" 
                                            Text="Must be 0.00 - 100.00." 
                                            runat="server" 
                                            MaximumValue="100" 
                                            MinimumValue="0" 
                                            Type="Double" 
                                            CssClass="validation-text"
                                            display="Dynamic" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell" style="text-align: right; ">Customer Name</td>
                                    <td class="form-cell" style="text-align: left;">
                                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-field" Text="" MaxLength="200" Width="200px"  TabIndex="2"></asp:TextBox>
                                        
                                    </td>
                                    <td class="form-cell" style="text-align: right;">Discount Percentage</td>
                                    <td class="form-cell" style="text-align: left;">
                                        <asp:TextBox ID="txtDiscountPercentage" runat="server" CssClass="form-field" Text="" MaxLength="6" Width="60px"  TabIndex="11"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td class="validation-cell">
                                        <asp:CustomValidator 
                                            ID="valcustCustomerName" 
                                            runat="server" 
                                            EnableClientScript="false" 
                                            ControlToValidate="txtCustomerName"
                                            ValidateEmptyText="true"
                                            ValidationGroup="customer"
                                            CssClass="validation-text"
                                            Display="Dynamic"
                                            OnServerValidate="valcustCustomerName_ServerValidate" >
                                        </asp:CustomValidator>
                                    </td>
                                    <td></td>
                                    <td class="validation-cell">
                                        <asp:RegularExpressionValidator runat="server" 
                                            ID="valregxDiscountPercentage"
                                                ValidationExpression="^((\d{3})*|(\d{0,2}))(\.\d{0,2})?$"
                                                ControlToValidate="txtDiscountPercentage" 
                                                Text="Discount Percentage is not valid.<br />" 
                                                ValidationGroup="customer"
                                                CssClass="validation-text"
                                                Display="Dynamic" />
                                        <asp:RangeValidator ID="valrangeDiscountPercentage" 
                                            ControlToValidate="txtDiscountPercentage" 
                                            ValidationGroup="customer" 
                                            Text="Must be 0.00 - 100.00." 
                                            runat="server" 
                                            MaximumValue="100" 
                                            MinimumValue="0" 
                                            Type="Double" 
                                            CssClass="validation-text"
                                            display="Dynamic" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell" style="text-align: right;">Trading Name</td>
                                    <td class="form-cell" style="text-align: left;">
                                        <asp:TextBox ID="txtTradingName" runat="server" CssClass="form-field" Text="" MaxLength="200" Width="200px"  TabIndex="3"></asp:TextBox>
                                        
                                    </td>
                                    <td class="form-cell" style="text-align: right;">Trading Terms</td>
                                    <td class="form-cell" style="text-align: left;">
                                        <asp:DropDownList ID="ddlTradingTerms" CssClass="form-field form-field-ddl form-select-disable" runat="server" style="width:auto;" Enabled="false" TabIndex="12" >
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td class="validation-cell"></td>
                                    <td></td>
                                    <td class="validation-cell"><asp:RequiredFieldValidator ID="valrfTradingTerms" runat="server" ControlToValidate="ddlTradingTerms" ErrorMessage="Please select Trading Terms."
                                                CssClass="validation-text" ValidationGroup="customer" Display="Dynamic" InitialValue="0" Enabled="false" ></asp:RequiredFieldValidator>
                                    </td>
                                </tr>

                                <tr>
                                    <td></td>
                                    <td class="validation-cell">
                                        <asp:CustomValidator 
                                            ID="valCustTradingName" 
                                            runat="server" 
                                            EnableClientScript="false" 
                                            ControlToValidate="txtTradingName"
                                            ValidateEmptyText="true"
                                            ValidationGroup="customer"
                                            CssClass="validation-text"
                                            Display="Dynamic"
                                            OnServerValidate="valcustTradingName_ServerValidate" >
                                        </asp:CustomValidator>
                                    </td>
                                    <td></td>
                                    <td class="validation-cell">
                                        
                                    </td>
                                </tr>

                                <tr>
                                    <td class="form-cell" style="text-align: right;">Customer Abbreviation</td>
                                    <td class="form-cell" style="text-align: left;"> 
                                        <asp:TextBox ID="txtCustomerAbbreviation" runat="server" CssClass="form-field" Text="" MaxLength="50" Width="200px"  TabIndex="4"></asp:TextBox>
                                    </td>
                                    <td class="form-cell" style="text-align: right;">Tax Status</td>
                                    <td class="form-cell" style="text-align: left;">
                                        <asp:DropDownList ID="ddlTaxStatus" CssClass="form-field form-field-ddl form-select-disable" runat="server" style="width:auto;" Enabled="false" TabIndex="13">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td class="validation-cell"></td>
                                    <td></td>
                                    <td class="validation-cell"><asp:RequiredFieldValidator ID="valrfTaxStatus" runat="server" ControlToValidate="ddlTaxStatus" ErrorMessage="Please select Tax Status."
                                                CssClass="validation-text" ValidationGroup="customer" Display="Dynamic" InitialValue="0" Enabled="false" ></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell" style="text-align: right;">Email</td>
                                    <td class="form-cell" style="text-align: left;"> 
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-field" Text="" MaxLength="1000" Width="200px"  TabIndex="5"></asp:TextBox>
                                    </td>
                                    <td class="form-cell" style="text-align: right;">ABN</td>
                                    <td class="form-cell" style="text-align: left;">
                                        <asp:TextBox ID="txtABN" runat="server" CssClass="form-field" Text="" MaxLength="20" Width="120px"  TabIndex="14"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td class="validation-cell">
                                        <asp:RequiredFieldValidator ID="valrfEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required."
                                                CssClass="validation-text" ValidationGroup="customer" Display="Dynamic" ></asp:RequiredFieldValidator>
                                    </td>
                                    <td></td>
                                    <td class="validation-cell">
                                        <asp:CustomValidator ID="valcusABN" runat="server" ControlToValidate="txtABN" ErrorMessage="ABN is not valid."
                                                CssClass="validation-text" ValidationGroup="customer" Display="Dynamic" OnServerValidate="valcusABN_ServerValidate" ClientValidationFunction="validateABN"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell" style="text-align: right;">Customer Phone 1</td>
                                    <td class="form-cell" style="text-align: left;"> 
                                        <asp:TextBox ID="txtPhone1" runat="server" CssClass="form-field" Text="" MaxLength="20" Width="200px"  TabIndex="6"></asp:TextBox>
                                    </td>
                                    <td class="form-cell" style="text-align: right;">Sort Code</td>
                                    <td class="form-cell" style="text-align: left;">
                                        <asp:DropDownList ID="ddlSortCode" CssClass="form-field form-field-ddl form-select-disable" runat="server" style="width:auto;" Enabled="false" TabIndex="15"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td class="validation-cell"></td>
                                    <td></td>
                                    <td class="validation-cell"><asp:RequiredFieldValidator ID="valrfSortCode" runat="server" ControlToValidate="ddlSortCode" ErrorMessage="Please select Sort Code."
                                                CssClass="validation-text" ValidationGroup="customer" Display="Dynamic" InitialValue="0" Enabled="false" ></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell" style="text-align: right;">Customer Phone 2</td>
                                    <td class="form-cell" style="text-align: left;"> 
                                        <asp:TextBox ID="txtPhone2" runat="server" CssClass="form-field" Text="" MaxLength="20" Width="200px"  TabIndex="7"></asp:TextBox>
                                    </td>
                                    <td class="form-cell" style="text-align: right;">Analysis Code</td>
                                    <td class="form-cell" style="text-align: left;">
                                        <asp:DropDownList ID="ddlAnalysisCode" CssClass="form-field form-field-ddl form-select-disable" runat="server" style="width:auto;" Enabled="false" TabIndex="17"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td class="validation-cell"></td>
                                    <td></td>
                                    <td class="validation-cell"><asp:RequiredFieldValidator ID="valrfAnalysisCode" runat="server" ControlToValidate="ddlAnalysisCode" ErrorMessage="Please select Analysis Code."
                                                CssClass="validation-text" ValidationGroup="customer" Display="Dynamic" InitialValue="0" Enabled="false" ></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell" style="text-align: right;">Customer Phone 3</td>
                                    <td class="form-cell" style="text-align: left;"> 
                                        <asp:TextBox ID="txtPhone3" runat="server" CssClass="form-field" Text="" MaxLength="20" Width="200px"  TabIndex="8"></asp:TextBox>
                                    </td>
                                    <td class="form-cell" style="text-align: right;">Sybiz Price Scale</td>
                                    <td class="form-cell" style="text-align: left;">
                                        <asp:DropDownList ID="ddlPriceScale" CssClass="form-field form-field-ddl form-select-disable" runat="server" style="width:auto;" Enabled="false" TabIndex="18"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td class="validation-cell"></td>
                                    <td></td>
                                    <td class="validation-cell"><asp:RequiredFieldValidator ID="valrfPriceScale" runat="server" ControlToValidate="ddlPriceScale" ErrorMessage="Please select Price Scale."
                                                CssClass="validation-text" ValidationGroup="customer" Display="Dynamic" InitialValue="0" Enabled="false" ></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell" style="text-align: right;">Credit Limit</td>
                                    <td class="form-cell" style="text-align: left;"> 
                                        <asp:TextBox ID="txtCreditLimit" runat="server" CssClass="form-field" 
                                            Text="" MaxLength="15" Width="200px" TabIndex="9" Enabled="false" ></asp:TextBox>
                                    <td class="form-cell" style="text-align: right;">Louvre Price Category</td>
                                    <td class="form-cell" style="text-align: left;">
                                        <asp:DropDownList ID="ddlLouvreCategory" CssClass="form-field form-field-ddl" runat="server" style="width:auto;"  TabIndex="18"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td class="validation-cell">
                                        <asp:RegularExpressionValidator runat="server" ID="valregxCreditLimit"
                                            ValidationExpression="\$?\d+((,\d{1,12})+)?(\.\d{1,2})?"
                                            ControlToValidate="txtCreditLimit" Text="Invalid Credit Limit." 
                                            ValidationGroup="customer"
                                            CssClass="validation-text"
                                            Display="Dynamic" />
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div style="padding-top: 20px; width:98%">
                            <div style="display:inline-block;">
                                <table class="form-table" cellspacing="0" summary="">
                                    <tr>
                                        <td class="form-cell" style="text-align: right;">Plantations</td>
                                        <td class="form-cell" style="text-align: left;">
                                            <asp:CheckBox ID="chkPlantation" CssClass="checkbox" runat="server" TabIndex="19" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-cell" style="text-align: right;">Wholesale Louvres</td>
                                        <td class="form-cell" style="text-align: left;">
                                            <asp:CheckBox ID="chkWholesaleLouvers" CssClass="checkbox" runat="server" TabIndex="20" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-cell" style="text-align: right;">Retail Louvres</td>
                                        <td class="form-cell" style="text-align: left;">
                                            <asp:CheckBox ID="chkRetailLouvers" CssClass="checkbox" runat="server" TabIndex="21" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="display:inline-block;">
                                <table class="form-table" cellspacing="0" summary="">
                                    <tr>
                                        <td class="form-cell" style="text-align: right;">Collection From Factory</td>
                                        <td class="form-cell" style="text-align: left;"> 
                                            <asp:CheckBox ID="chkCollectionFactory" CssClass="checkbox" runat="server" TabIndex="22" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-cell" style="text-align: right;">External Customer</td>
                                        <td class="form-cell" style="text-align: left;">
                                            <asp:CheckBox ID="chkExternalCustomer" CssClass="checkbox" runat="server" TabIndex="23" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-cell" style="text-align: right;">Discontinued</td>
                                        <td class="form-cell" style="text-align: left;">
                                            <asp:CheckBox ID="chkDiscontinued" CssClass="checkbox" runat="server" TabIndex="24" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>

                    <h2 class="sub-heading">
                        <asp:Label ID="lblMailingLists" Text="Mailing Lists" runat="server"></asp:Label>
                    </h2>

                    <div style="padding-top: 20px; width:98%">
                        <div style="display:inline-block;">
                            <table class="form-table" cellspacing="0" summary="">
                                <tr>
                                    <td class="form-cell" style="text-align: right;">Order Confirmation
                                    </td>
                                    <td class="form-cell" style="text-align: left;">
                                        <asp:CheckBox ID="chkMailingListOrderConfirmation" CssClass="checkbox" runat="server" TabIndex="25" />
                                        <asp:Image ID="imgTipMailingListOrderConfirmation" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" 
                                            ToolTip="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell" style="text-align: right;">Order Completed
                                    </td>
                                    <td class="form-cell" style="text-align: left;">
                                        <asp:CheckBox ID="chkMailingListOrderCompleted" CssClass="checkbox" runat="server" TabIndex="26" />
                                        <asp:Image ID="imgTipMailingListOrderCompleted" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" 
                                            ToolTip="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell" style="text-align: right;">Weekly Order Status Update
                                    </td>
                                    <td class="form-cell" style="text-align: left;"> 
                                        <asp:CheckBox ID="chkMailingListWeeklyOrderStatusUpdate" CssClass="checkbox" runat="server" TabIndex="27" />
                                        <asp:Image ID="imgTipMailingListWeeklyOrderStatusUpdate" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" 
                                            ToolTip="" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                    <h2 class="sub-heading">
                        <asp:Label ID="lblPostalAddressHeading" Text="Postal Address" runat="server"></asp:Label>
                    </h2>

                    <div style="margin:0 auto; display:inline-block; text-align:center; padding-top:15px; width:98%">
                        <div style="margin:0 auto; display:inline-block; text-align:center;">

                            <asp:UpdatePanel ID="pnlUpdatePostalAddress" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <asp:GridView ID="gdvPostalAddress" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" CssClass="address-table">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="40px" />
                                        <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" CssClass="address-header" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>   
                                            <asp:TemplateField HeaderText="Street" ItemStyle-VerticalAlign="Middle" Visible="false">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblPostalAddressID" visible="False"  Text='<%#(Eval("ID")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label runat="server" ID="lblPostalAddressID" visible="False"  Text='<%#(Eval("ID")) %>' >' ></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField> 
                             
                                            <asp:TemplateField HeaderText="Street" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblPostalAddressStreet" CssClass="address-field" Text='<%#(Eval("Street")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtPostalAddressStreet" runat="server" CssClass="form-field address-field" Text='<%#(Eval("Street"))%>' MaxLength="100" Width="200px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valrfPostalAddressStreet" runat="server" ControlToValidate="txtPostalAddressStreet" ErrorMessage="<br />Please enter Street."
                                                        CssClass="validation-text" ValidationGroup="postalAddress" Display="dynamic" ></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Suburb" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblPostalAddressSuburb" CssClass="address-field" Text='<%#(Eval("Suburb")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtPostalAddressSuburb" runat="server" CssClass="form-field address-field" Text='<%#(Eval("Suburb"))%>' MaxLength="100" Width="200px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valrfPostalAddressSuburb" runat="server" ControlToValidate="txtPostalAddressSuburb" ErrorMessage="<br />Please enter Suburb."
                                                        CssClass="validation-text" ValidationGroup="postalAddress" Display="dynamic" ></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="State" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblPostalAddressState" CssClass="address-field" Text='<%#(Eval("State")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlPostalAddressState" CssClass="form-field address-field" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="valrfPostalAddressState" runat="server" ControlToValidate="ddlPostalAddressState" ErrorMessage="<br />Select State."
                                                        CssClass="validation-text" ValidationGroup="postalAddress" Display="dynamic" InitialValue="0" ></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Postcode" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblPostalAddressPostcode" CssClass="address-field" Text='<%#(Eval("Postcode")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtPostalAddressPostcode" runat="server" CssClass="form-field address-field" Text='<%#(Eval("Postcode"))%>' MaxLength="4" Width="50px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valrfPostalAddressPostcode" runat="server" ControlToValidate="txtPostalAddressPostcode" ErrorMessage="<br />Select Postcode."
                                                        CssClass="validation-text" ValidationGroup="postalAddress" Display="dynamic" ></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Primary" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:CheckBox ID="chkPostalAddressPrimary" runat="server" CssClass="address-field checkbox-primary" Checked='<%#(Eval("IsPrimary"))%>' Enabled="false"></asp:CheckBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkPostalAddressPrimary" runat="server" CssClass="checkbox address-field checkbox-primary" Checked='<%#(Eval("IsPrimary"))%>' Enabled="true"></asp:CheckBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Discontinued" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:CheckBox ID="chkPostalAddressDiscontinued" runat="server" CssClass="checkbox address-field" Checked='<%#(Eval("Discontinued"))%>' Enabled="false"></asp:CheckBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkPostalAddressDiscontinued" runat="server" CssClass="checkbox address-field" Checked='<%#(Eval("Discontinued"))%>' Enabled="true"></asp:CheckBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="" ItemStyle-VerticalAlign="Middle">
                                                <ItemStyle Width="130px" />
                                                <ItemTemplate>
                                                    <asp:Button ID="btnPostalAddressEdit" ToolTip="Edit" runat="server" CssClass="edit-button" CommandName="Edit"  />
                                                    <asp:Button ID="btnPostalAddressCopyToClipboard" ToolTip="Copy to Clipboard" runat="server" CssClass="copy-button" 
                                                        CommandName="Copy" CommandArgument='<%#(Eval("ID")) %>'  />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Button ID="btnPostalAddressUpdate" ToolTip="Save" runat="server" CssClass="save-button" 
                                                        CommandName="Update" ValidationGroup="postalAddress"  />
                                                    <asp:Button ID="btnPostalAddressCancel" ToolTip="Cancel" runat="server" CssClass="cancel-button" 
                                                        CommandName="Cancel" />
                                                    <asp:Button ID="btnPostalAddressPasteFromClipboard" ToolTip="Paste from Clipboard" runat="server" CssClass="paste-button" 
                                                        CommandName="Paste" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>

                                    <asp:Button ID="btnPostalAddressAddNew" runat="server" CssClass="form-button address-button" Visible="true" Text="Add" UseSubmitBehavior="false" />

                                </ContentTemplate>

                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnPostalAddressAddNew" EventName="Click"/>
                                </Triggers>

                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <h2 class="sub-heading">
                        <asp:Label ID="lblPhysicalAddressHeading" Text="Physical Address" runat="server"></asp:Label>
                    </h2>

                    <div style="margin:0 auto; display:inline-block; text-align:center;  padding-top:15px;  width:98%">
                        <div style="margin:0 auto; display:inline-block; text-align:center;">

                            <asp:UpdatePanel ID="pnlUpdatePhysicalAddress" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <asp:GridView ID="gdvPhysicalAddress" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" CssClass="address-table">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="40px" />
                                        <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" CssClass="address-header" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>   
                                            <asp:TemplateField HeaderText="Street" ItemStyle-VerticalAlign="Middle" Visible="false">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblPhysicalAddressID" visible="False"  Text='<%#(Eval("ID")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label runat="server" ID="lblPhysicalAddressID" visible="False"  Text='<%#(Eval("ID")) %>' >' ></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField> 
                             
                                            <asp:TemplateField HeaderText="Street" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblPhysicalAddressStreet" CssClass="address-field" Text='<%#(Eval("Street")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtPhysicalAddressStreet" runat="server" CssClass="form-field address-field" Text='<%#(Eval("Street"))%>' MaxLength="100" Width="200px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valrfPhysicalAddressStreet" runat="server" ControlToValidate="txtPhysicalAddressStreet" ErrorMessage="<br />Please select Street."
                                                        CssClass="validation-text" ValidationGroup="physicalAddress" Display="dynamic" ></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Suburb" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblPhysicalAddressSuburb" CssClass="address-field" Text='<%#(Eval("Suburb")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtPhysicalAddressSuburb" runat="server" CssClass="form-field address-field" Text='<%#(Eval("Suburb"))%>' MaxLength="100" Width="200px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valrfPhysicalAddressSuburb" runat="server" ControlToValidate="txtPhysicalAddressSuburb" ErrorMessage="<br />Please select Suburb."
                                                        CssClass="validation-text" ValidationGroup="physicalAddress" Display="dynamic" ></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="State" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblPhysicalAddressState" CssClass="address-field" Text='<%#(Eval("State")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlPhysicalAddressState" CssClass="form-field address-field" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="valrfPhysicalAddressState" runat="server" ControlToValidate="ddlPhysicalAddressState" ErrorMessage="<br />Select State."
                                                        CssClass="validation-text" ValidationGroup="physicalAddress" Display="dynamic" InitialValue="0" ></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Postcode" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblPhysicalAddressPostcode" CssClass="address-field" Text='<%#(Eval("Postcode")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtPhysicalAddressPostcode" runat="server" CssClass="form-field address-field" Text='<%#(Eval("Postcode"))%>' MaxLength="4" Width="50px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valrfPhysicalAddressPostcode" runat="server" ControlToValidate="txtPhysicalAddressPostcode" ErrorMessage="<br />Select Postcode."
                                                        CssClass="validation-text" ValidationGroup="physicalAddress" Display="dynamic" ></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Primary" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:CheckBox ID="chkPhysicalAddressPrimary" runat="server" CssClass="address-field" Checked='<%#(Eval("IsPrimary"))%>' Enabled="false"></asp:CheckBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkPhysicalAddressPrimary" runat="server" CssClass="checkbox address-field" Checked='<%#(Eval("IsPrimary"))%>' Enabled="true"></asp:CheckBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Discontinued" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:CheckBox ID="chkPhysicalAddressDiscontinued" runat="server" CssClass="address-field" Checked='<%#(Eval("Discontinued"))%>' Enabled="false"></asp:CheckBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkPhysicalAddressDiscontinued" runat="server" CssClass="checkbox address-field" Checked='<%#(Eval("Discontinued"))%>' Enabled="true"></asp:CheckBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="" ItemStyle-VerticalAlign="Middle">
                                                <ItemStyle Width="130px" />
                                                <ItemTemplate>
                                                    <asp:Button ID="btnPhysicalAddressEdit" ToolTip="Edit" runat="server" CssClass="edit-button" CommandName="Edit" />
                                                    <asp:Button ID="btnPhysicalAddressCopyToClipboard" ToolTip="Copy to Clipboard" runat="server" CssClass="copy-button" 
                                                        CommandName="Copy" CommandArgument='<%#(Eval("ID")) %>'  />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Button ID="btnPhysicalAddressUpdate" ToolTip="Save" runat="server" CssClass="save-button" CommandName="Update" ValidationGroup="physicalAddress" />
                                                    <asp:Button ID="btnPhysicalAddressCancel" ToolTip="Cancel" runat="server" CssClass="cancel-button" 
                                                        CommandName="Cancel" />
                                                    <asp:Button ID="btnPhysicalAddressPasteFromClipboard" ToolTip="Paste from Clipboard" runat="server" CssClass="paste-button" 
                                                        CommandName="Paste" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>

                                    <asp:Button ID="btnPhysicalAddressAddNew" runat="server" CssClass="form-button address-button" Visible="true" Text="Add" UseSubmitBehavior="false" />

                                </ContentTemplate>

                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnPhysicalAddressAddNew" EventName="Click"/>
                                </Triggers>

                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <h2 class="sub-heading">
                        <asp:Label ID="lblDeliveryAddressHeading" Text="Delivery Address" runat="server"></asp:Label>
                    </h2>

                    <div style="margin:0 auto; display:inline-block; text-align:center;  padding-top:15px;  width:98%">
                        <div style="margin:0 auto; display:inline-block; text-align:center;">

                            <asp:UpdatePanel ID="pnlUpdateDeliveryAddress" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <asp:GridView ID="gdvDeliveryAddress" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" CssClass="address-table">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="40px" />
                                        <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" CssClass="address-header" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>   
                                            <asp:TemplateField HeaderText="Street" ItemStyle-VerticalAlign="Middle" Visible="false">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblDeliveryAddressID" visible="False"  Text='<%#(Eval("ID")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label runat="server" ID="lblDeliveryAddressID" visible="False"  Text='<%#(Eval("ID")) %>' >' ></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField> 
                             
                                            <asp:TemplateField HeaderText="Street" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblDeliveryAddressStreet" CssClass="address-field" Text='<%#(Eval("Street")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDeliveryAddressStreet" runat="server" CssClass="form-field address-field" Text='<%#(Eval("Street"))%>' MaxLength="100" Width="200px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valrfDeliveryAddressStreet" runat="server" ControlToValidate="txtDeliveryAddressStreet" ErrorMessage="<br />Please select Street."
                                                        CssClass="validation-text" ValidationGroup="deliveryAddress" Display="dynamic" ></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Suburb" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblDeliveryAddressSuburb" CssClass="address-field" Text='<%#(Eval("Suburb")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDeliveryAddressSuburb" runat="server" CssClass="form-field address-field" Text='<%#(Eval("Suburb"))%>' MaxLength="100" Width="200px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valrfDeliveryAddressSuburb" runat="server" ControlToValidate="txtDeliveryAddressSuburb" ErrorMessage="<br />Please select Suburb."
                                                        CssClass="validation-text" ValidationGroup="deliveryAddress" Display="dynamic" ></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="State" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblDeliveryAddressState" CssClass="address-field" Text='<%#(Eval("State")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="cboDeliveryAddressState" CssClass="form-field address-field" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="valrfDeliveryAddressState" runat="server" ControlToValidate="cboDeliveryAddressState" ErrorMessage="<br />Select State."
                                                        CssClass="validation-text" ValidationGroup="deliveryAddress" Display="dynamic" InitialValue="0" ></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Postcode" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:Label runat="server" ID="lblDeliveryAddressPostcode" CssClass="address-field" Text='<%#(Eval("Postcode")) %>' >' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDeliveryAddressPostcode" runat="server" CssClass="form-field address-field" Text='<%#(Eval("Postcode"))%>' MaxLength="4" Width="50px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valrfDeliveryAddressPostcode" runat="server" ControlToValidate="txtDeliveryAddressPostcode" ErrorMessage="<br />Select Postcode."
                                                        CssClass="validation-text" ValidationGroup="deliveryAddress" Display="dynamic"  ></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Primary" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:CheckBox ID="chkDeliveryAddressPrimary" runat="server" CssClass="address-field" Checked='<%#(Eval("IsPrimary"))%>'  Enabled="false"></asp:CheckBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkDeliveryAddressPrimary" runat="server" CssClass="checkbox address-field" Checked='<%#(Eval("IsPrimary"))%>'  Enabled="true"></asp:CheckBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Discontinued" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:CheckBox ID="chkDeliveryAddressDiscontinued" runat="server" CssClass="address-field" Checked='<%#(Eval("Discontinued"))%>'  Enabled="false"></asp:CheckBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkDeliveryAddressDiscontinued" runat="server" CssClass="checkbox address-field" Checked='<%#(Eval("Discontinued"))%>'  Enabled="true"></asp:CheckBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Instructions" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>                                   
                                                    <asp:Button ID="btnDeliveryAddressInstructions" runat="server" CommandName="DeliveryInstructions" CommandArgument='<%# Container.DataItemIndex %>' CssClass="form-button address-button" Visible="true" 
                                                        Text='<%# "View (" & CountDeliveryInstructions(CInt(Eval("ID"))).ToString & ")" %>' UseSubmitBehavior="false" />
                                                    <ajaxcontroltoolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnDeliveryAddressInstructions" PopupControlID="pnlDeliveryInstructions" BackgroundCssClass="modalBackground">
                                                    </ajaxcontroltoolkit:ModalPopupExtender>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                    
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="" ItemStyle-VerticalAlign="Middle">
                                                <ItemStyle Width="130px" />
                                                <ItemTemplate>
                                                    <asp:Button ID="btnDeliveryAddressEdit" ToolTip="Edit" runat="server" CssClass="edit-button" CommandName="Edit" />
                                                    <asp:Button ID="btnDeliveryAddressCopyToClipboard" ToolTip="Copy to Clipboard" runat="server" CssClass="copy-button" 
                                                        CommandName="Copy" CommandArgument='<%#(Eval("ID")) %>'  />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Button ID="btnDeliveryAddressUpdate" ToolTip="Save" runat="server" CssClass="save-button" CommandName="Update" ValidationGroup="deliveryAddress" />
                                                    <asp:Button ID="btnDeliveryAddressCancel" ToolTip="Cancel" runat="server" CssClass="cancel-button" 
                                                        CommandName="Cancel" />
                                                    <asp:Button ID="btnDeliveryAddressPasteFromClipboard" ToolTip="Paste from Clipboard" runat="server" CssClass="paste-button" 
                                                        CommandName="Paste" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>

                                    <asp:Button ID="btnDeliveryAddressAddNew" runat="server" CssClass="form-button address-button" Visible="true" Text="Add" UseSubmitBehavior="false" />

                                </ContentTemplate>

                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnDeliveryAddressAddNew" EventName="Click"/>
                                </Triggers>

                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <asp:Panel ID="pnlDeliveryInstructions" runat="server" CssClass="modalPopup" Style="display: none;">
                        <asp:UpdatePanel ID="pnlUpdateDeliveryInstructions" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <h2>Delivery Instructions</h2>
                                <ajaxcontroltoolkit:Accordion ID="accDeliveryInstructions" runat="server" FadeTransitions="true"  HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                                    <HeaderTemplate>
                                        <%#DataBinder.Eval(Container.DataItem, "DisplayHeaderText")%>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        Delivery Instructions: <%#DataBinder.Eval(Container.DataItem, "InstructionText")%>
                                    </ContentTemplate>
                                </ajaxcontroltoolkit:Accordion>

                                <br />

                                <div style="text-align: center;">
                                    <asp:Button ID="btnAddNewInstruction" runat="server" Text="Add Delivery Instruction" CssClass="form-button" />
                                </div>

                                <br />

                                <asp:Panel ID="pnlAddInstruction" runat="server" Visible="false">
                                    <table class="form-table" cellspacing="0" summary="">
                                        <tr>
                                            <td class="form-field-td-p2" style="text-align: right;">
                                                Instruction:&nbsp;&nbsp; 
                                            </td>
                                            <td class="form-field-td-p2">
                                                <asp:TextBox ID="txtNewInstructionText" runat="server" CssClass="form-field" TextMode="MultiLine"></asp:TextBox>
                                            </td>                                    
                                        </tr>
                                        <tr>
                                            <td class="form-field-td-p2">
                                                <div style="float: right;">
                                                    <asp:Button runat="server" ID="btnCancelInstruction" Text="Cancel" CssClass="form-button address-button" />
                                                </div>
                                            </td>
                                            <td class="form-field-td-p2">
                                                <div style="float: left;">
                                                    <asp:Button runat="server" ID="btnSaveInstruction" Text="Save" CssClass="form-button address-button" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <br />
                                <br />
                                <div style="text-align: center;">
                                    <asp:Button runat="server" ID="btnCloseInstruction" Text="Close" CssClass="form-button" />
                                </div>
                            </contentTemplate>

                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnCloseInstruction" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>

                    <h2 class="sub-heading">
                        <asp:Label ID="lblNotesHeading" Text="Notes" runat="server"></asp:Label>
                    </h2>

                    <asp:Panel ID="pnlNotes" runat="server" CssClass="notes" >
                        <asp:UpdatePanel ID="pnlUpdateNotes" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <ajaxcontroltoolkit:Accordion ID="accNotes" runat="server" FadeTransitions="true" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                                    <HeaderTemplate>
                                        <%#DataBinder.Eval(Container.DataItem, "DisplayHeaderText")%>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        Notes: <%#DataBinder.Eval(Container.DataItem, "NoteText")%>
                                    </ContentTemplate>
                                </ajaxcontroltoolkit:Accordion>

                                <br />

                                <div style="text-align: center;">
                                    <asp:Button ID="btnAddNewNote" runat="server" Text="Add Note" CssClass="form-button" />
                                </div>

                                <br />

                                <asp:Panel ID="pnlAddNote" runat="server" Visible="false">
                                    <table class="form-table" cellspacing="0" summary="">
                                        <tr>
                                            <td class="form-field-td-p2" style="text-align: right;">
                                                Note:&nbsp;&nbsp; 
                                            </td>
                                            <td class="form-field-td-p2">
                                                <asp:TextBox ID="txtNewNoteText" runat="server" CssClass="form-field" TextMode="MultiLine"></asp:TextBox>
                                            </td>                                    
                                        </tr>
                                        <tr>
                                            <td class="form-field-td-p2">
                                                <div style="float: right;">
                                                    <asp:Button runat="server" ID="btnCancelNote" Text="Cancel" CssClass="form-button address-button" />
                                                </div>
                                            </td>
                                            <td class="form-field-td-p2">
                                                <div style="float: left;">
                                                    <asp:Button runat="server" ID="btnSaveNote" Text="Save" CssClass="form-button address-button" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                        
                                </asp:Panel>
                            </contentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="pnlAddToSybizParent" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlAddToSybiz" runat="server" CssClass="sybiz-popup" >
                        <asp:UpdatePanel ID="pnlupdateAddToSybiz" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <h2>Would you like to add this customer to Sybiz?</h2>
                                <br />
                                <br />
                                <asp:Button ID="btnAddCustomerToSybiz" runat="server" Text="Save customer and add them to Sybiz" CssClass="form-button" />
                                <br />
                                <br />
                                <br />
                                <asp:Button ID="btnDoNotAddCustomerToSybiz" runat="server" Text="Save customer and do NOT add them to Sybiz" CssClass="form-button" />
                            </contentTemplate>
                        </asp:UpdatePanel>

                        <asp:UpdateProgress ID="pnlprogressAddToSybiz" runat="server" AssociatedUpdatePanelID="pnlupdateAddToSybiz">
                            <ProgressTemplate>
                                <image src="images\indicator.gif" alt="Saving customer..." />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="pnlupdateResults" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <asp:Panel ID="pnlResults" runat="server" Visible="false" CssClass="status-box">
                        <asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="pnlupdateAddRemoveCustomerButtons" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <asp:Panel ID="pnlAddRemoveCustomerButtons" runat="server">                    
                        <div style="height: 100%; text-align: center; padding-bottom:30px;">
                            <table class="form-table" cellspacing="0" summary="">
                                <tr>
                                    <td style="text-align: right;" align="left" colspan="2">
                                        <div style="text-align: center;" class="auto-style5">
                                            &nbsp;&nbsp;

                                            <asp:ValidationSummary ID="valSummary" CssClass="valSummary" DisplayMode="BulletList" HeaderText="There are errors in the page. Please correct these errors before saving the customer.<br /><br />" 
                                                ForeColor="" ValidationGroup="customer" runat="server"/>
                                            <asp:Button ID="btnCancel" runat="server" CssClass="form-button address-button" Visible="true" Text="Cancel" UseSubmitBehavior="false" />
                                            <asp:Button ID="btnAddCustomer" runat="server" CssClass="form-button address-button" Visible="true" Text="Save" UseSubmitBehavior="false" ValidationGroup="customer" />
                                            <asp:Button ID="btnUpdateCustomer" runat="server" CssClass="form-button address-button" Visible="true" Text="Save" UseSubmitBehavior="false" ValidationGroup="customer" />
                                        
                                            <asp:Panel ID="pnlDummy" runat="server" />

                                            <ajaxcontroltoolkit:ModalPopupExtender ID="mpeAddToSybiz" runat="server" TargetControlID="pnlDummy" PopupControlID="pnlAddToSybiz" BackgroundCssClass="modalBackground">
                                            </ajaxcontroltoolkit:ModalPopupExtender>
	                                    </div>    
                                    </td>
                                </tr>  

                                <tr>
                                    <td style="text-align: right;" align="left" class="auto-style8">
                                        &nbsp;&nbsp;&nbsp;<img id="imgLoading2" src="images/indicator.gif" width="16px" height="16px" alt="loading" style="visibility: hidden; vertical-align: middle;" />
                                    </td>
                                    <td style="text-align: right;" align="left">
                                        &nbsp;</td>
                                </tr> 
                            </table>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <br />
        <div runat="server" id="DivCur2" style="display:none;">
        txtCurState:<asp:TextBox ID="txtCurState" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
        SiteID:<asp:TextBox ID="txtSiteID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
      

    <div id="dialog-alert" title="Insufficient Information">
        <p id="dialog-alert-message" style="text-align: left;"></p>
    </div>
    <div id="dialog-confirm" title="Please Confirm" >
        <p id="dialog-confirm-message" style="text-align: left;"></p>
    </div>
      
    </div>   
        <asp:TextBox ID="txtHidFlag" runat="server" Hidden="true" TabIndex="-1" Text=""></asp:TextBox>
            <asp:TextBox ID="txtHidCustomerID" runat="server" Hidden="true" TabIndex="-1" Text=""></asp:TextBox>
            <asp:TextBox ID="txtHidCustomerName" runat="server" Hidden="true" TabIndex="-1" Text=""></asp:TextBox>
            <asp:TextBox ID="txtHidDiscontinued" runat="server" Hidden="true" TabIndex="-1" Text=""></asp:TextBox>
            <asp:TextBox ID="txtHidExternal" runat="server" Hidden="true" TabIndex="-1" Text=""></asp:TextBox>
            <asp:TextBox ID="txtHidFactory" runat="server" Hidden="true" TabIndex="-1" Text=""></asp:TextBox>
            <asp:TextBox ID="txtHidFreightPercentage" runat="server" Hidden="true" TabIndex="-1" Text=""></asp:TextBox>
            <asp:TextBox ID="txtHidCustAbreviation" runat="server" Hidden="true" TabIndex="-1" Text=""></asp:TextBox>
            <asp:TextBox ID="txtHidCustFullName" runat="server" Hidden="true" TabIndex="-1" Text=""></asp:TextBox>
            
    </div>  
    
   </form>
</body>

</html>
