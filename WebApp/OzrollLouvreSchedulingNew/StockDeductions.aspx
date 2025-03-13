<%@ Page Language="VB" AutoEventWireup="false" CodeFile="StockDeductions.aspx.vb" Inherits="StockDeductions" Theme="SknGridView"  EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="ext" Namespace="Extensions" %>
<%@ Register Src="~/UserControls/PrivacyScreenDetails.ascx" TagPrefix="uc1" TagName="PrivacyScreenDetails" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server" >
    <title>OzRoll Stock Deductions</title>
        
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta name="format-detection" content="telephone=no" />
    
    <link href="stylesheets/reset.css" rel="stylesheet" type="text/css" media="screen" />
    <!--[if IE]><link href="stylesheets/explorer.css" rel="stylesheet" type="text/css" media="screen" /><![endif]-->
    <link href="stylesheets/style.css?version=<%# Now.Year() & Now.Month & Now.Day() %>" rel="stylesheet" type="text/css" media="screen" />


    <link href="stylesheets/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <link href="stylesheets/smoothness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />

    <!--[if lt IE 8]><script src="javascript/IE-fix.js" type="text/javascript"></script><![endif]-->

    <script src="javascript/jquery-1.9.1.js" type="text/javascript"></script>

    <script src="javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>

    <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
    
    <script language="javascript" type="text/javascript">

        if (window.history.forward(1) != null)
            window.history.forward(1);

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

    </script>

    <style type="text/css">

        .stockdeductions-table tr:first-child th {
            padding:5px;
            font-weight: normal;
        }

        .stockdeductions-table tr td {
            padding:5px;
        }

        .deduct-button {
            margin:10px;
        }

        .status-text {
            font-size: 1.5em;
            padding: 20px;
            background-color: rgb(240,240,240);
            border: solid 4px #ffd800;
            border-radius: 20px;
        }

        .status-div {
            padding: 30px;
            padding-bottom: 50px;
        }

        .reset-button {
            margin-bottom: 20px;
        }

    </style>

</head>
<body onload="HideHider();">
    <form runat="server" id="form1" method="post">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" AsyncPostBackTimeout="300" >
    <Scripts>
        <asp:ScriptReference Path="javascript/fixfocus.js" />
    </Scripts>
    </asp:ScriptManager>
    <div id="middle-container-diallerview" >

        <div id="logo" style="height: 100px; text-align: center;" ></div>

        <div style="height: 50px; text-align: center;">
            <asp:Button ID="btnBack" runat="server" CssClass="form-button" Text="Back" UseSubmitBehavior="false" />
            <asp:Button ID="btnHome" runat="server" CssClass="form-button" Text="Home" UseSubmitBehavior="false" />
            <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="javascript:return ConfirmLeave();" UseSubmitBehavior="false" />
	    </div>

        <h1>OzRoll Stock Deductions</h1>

        <h2><asp:label ID="lblOZNumberHeading" runat="server"></asp:label></h2>

        <div class="form" style="text-align: center;">
            <div class="form" style="text-align: center; width: 95%; display:inline-block;">
                <asp:Panel ID="pnlStockDeductions" runat="server">
                    <asp:GridView ID="gdvStockDeductions" runat="server" EnableTheming="false" AutoGenerateColumns="false" 
                        CellPadding="4" ForeColor="#333333" ShowHeader="true" BorderStyle="None" Width="100%" CssClass="stockdeductions-table" >
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#0059A9" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Code
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Description
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Length
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLength" runat="server" Text='<%# Eval("MaterialLengthForDisplay") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Qty
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Total
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("MaterialLengthTotalFullLengthsRequired") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Deduct
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkDeduct" runat="server" Checked="True" AutoPostBack="false" CssClass="checkbox" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
                    <br />
                    <asp:Button ID="btnBeginDeductions" runat="server" Text="Deduct Stock" CssClass="form-button deduct-button" 
                        OnClientClick="javascript:return confirm('Once the deduction has been completed it cannot be run again for this production schedule.');" />    
                    <br />  
                </asp:Panel> 

                <asp:Panel ID="pnlStockDeductionsStatus" runat="server">
                    <div class="status-div">
                        <asp:Label ID="lblStockDeductionsStatus" runat="server" CssClass="status-text" />  
                    </div>
                    <div>
                        <asp:Button ID="btnResetDeductions" runat="server" CssClass="form-button reset-button" Text="Reset Deductions" />  
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    </form>
</body>
</html>