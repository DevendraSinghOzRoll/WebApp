<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Reports.aspx.vb" Inherits="Reports" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxcontroltoolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="ext" Namespace="Extensions" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>OzRoll - Reports</title>
        
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

        function ConfirmLeave() {
            document.getElementById("dialog-confirm-message").innerHTML = "Are you sure you want to logout?";
            $("#dialog-confirm").dialog("open");
            return false;
        }

        function pageLoad() {
            $("input.dtePicker").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'fadeIn', minDate: '-10y', maxDate: '+1y' });
        }

    </script>

    <style type="text/css">
        
        .form-select {
            width: auto;
            padding: 4px;
        }

        .form-field {
            width: auto;
        }

        .excel-report-button {
            margin-top: 40px;
            margin-bottom: 40px;
        }

        .report-selector-panel {
            padding: 40px;
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
                <asp:Label ID="lblTitle" runat="server">Reports</asp:Label>
            </h1>
    
            <div style="height: 100%; text-align: center; border-radius: 1em; padding-bottom:20px;">
                <div style="display:inline-block; height: 100%; width: 1200px; text-align: center; background-color: #F2F2F2; box-shadow:10px 10px 10px grey; border-radius: 1em;">  
                    
                    <asp:Panel ID="pnlReportSelector" CssClass="report-selector-panel" runat="server">
                        <asp:DropDownList ID="ddlReportSelector" runat="server" CssClass="form-select" AutoPostBack="true" >
                            <asp:ListItem Text="Select Report..." Value="0"></asp:ListItem>
                            <asp:ListItem Text="-------------------" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Sales Report" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Job Cost Report" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Louvres Despatch Report" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Louvres Order Intake Report (Test)" Value="5"></asp:ListItem> <%--added by FL #63717 08-02-2021--%>
                        </asp:DropDownList>
                        <asp:Label ID="lblReportNote" runat="server"></asp:Label>
                    </asp:Panel>        

                    <asp:Panel ID="pnlReportConfig" CssClass="report-config-panel" runat="server">
                        
                        <div style="display:inline-block" ID="texthide2" runat="server">

                            Date Type &nbsp;

                            <asp:DropDownList ID="ddlDateType" runat="server" CssClass="form-select" /><br />
                            <asp:RequiredFieldValidator ID="rfvalLouvreProd" runat="server" ControlToValidate="ddlDateType" CssClass="validation-text"
                                ErrorMessage="Please select Date Type." InitialValue="0" ValidationGroup="report" Display="static" EnableClientScript="true" />
                        </div>
                        
                        <div style="display:inline-block" ID="texthide3" runat="server">

                            &nbsp; From&nbsp;

                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-field dtePicker" MaxLength="20" /><br />
                            <asp:CustomValidator runat="server" 
                                CssClass="validation-text"
                                ControlToValidate="txtStartDate"
                                Display="static"
                                ValidationGroup="report"
                                ValidateEmptyText="true"
                                EnableClientScript="true"
                                OnServerValidate="valcustDate_Validate"
                                Text="Date Invalid" /> 
                        </div>

                        <div style="display:inline-block" id="texthide" runat="server">

                            &nbsp;To&nbsp;

                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-field dtePicker" MaxLength="20" /><br />
                            <asp:CustomValidator runat="server" 
                                CssClass="validation-text"
                                ControlToValidate="txtEndDate"
                                Display="static"
                                ValidationGroup="report"
                                ValidateEmptyText="true"
                                EnableClientScript="true"
                                OnServerValidate="valcustDate_Validate"
                                Text="Date Invalid" /> 
                        </div>

                        <br />

                        <asp:Button ID="btnGenerateReportExcel" runat="server" Text="Excel Report" CssClass="form-button excel-report-button" UseSubmitBehavior="false" ValidationGroup="report" />

                    </asp:Panel>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
