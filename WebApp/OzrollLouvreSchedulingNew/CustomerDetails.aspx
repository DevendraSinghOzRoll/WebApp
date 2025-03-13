<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CustomerDetails.aspx.vb" Inherits="CustomerDetails" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>OzRoll Customer Details</title>
        
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
		
		    $("[id$=txtActualShippingDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+5w' });
		    $("[id$=txtOrderDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+1d' });
		    $("[id$=txtInvoiceDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+1d' });
		    $("[id$=txtStartDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+10w' });
		    $("[id$=txtEndDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+10w' });
		
        });

	    if (window.history.forward(1) != null)
	        window.history.forward(1);

	   
        function addnew() {
	        document.getElementById("<%=btnAddCustomer.ClientID %>").disabled = true;
	        document.getElementById("<%=btnAddCustomer.ClientID %>").className = "form-button-disabled";
	        document.getElementById("<%=btnAddCustomer.ClientID %>").value = "Submitting";
            document.getElementById("imgLoading2").style.visibility = "visible";
	        __doPostBack('<%=btnAddCustomer.ClientID %>', '');
	        return true;
	    }

       


            function reloadPage() {
                setTimeout("location.reload(true);", 10000);
                return true;
            }

            function showMe() {
                if (document.getElementById("DivCur2").style.display == "none") {
                    document.getElementById("DivCur2").style.display = "block";
                }
                else {
                    document.getElementById("DivCur2").style.display = "none";
                }
            }

            function ConfirmLeave() {
                document.getElementById("dialog-confirm-message").innerHTML = "Are you sure you want to logout?";
                $("#dialog-confirm").dialog("open");
                return false;
            }

            function saveLocation() {
                localStorage.setItem('sTopLevelPage', window.location);
            }

            function restBTNs() {
                document.getElementById("<%=btnAddCustomer.ClientID %>").disabled = false;
                document.getElementById("<%=btnAddCustomer.ClientID %>").className = "form-button";
                document.getElementById("<%=btnAddCustomer.ClientID %>").value = "Add Job";
                document.getElementById("imgLoading2").style.visibility = "invisible";
            }

            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
                return true;
            }

   </script>

    <style type="text/css">
        .gdv_linkButton {
            color: white !important;
        }
        .gdv_linkButton:hover {
            color: white !important;
        }
        .gdv_linkButton:visited {
            color: white !important;
        }
        .gdv_linkButton:active {
            color: white !important;
        }
        .customers-table th, .customers-table td {
            padding: 5px;
        }
        .ozrol-btn {
	                -moz-box-shadow: inset 0px 1px 0px 0px #a1a1a1;
	                -webkit-box-shadow: inset 0px 1px 0px 0px #a1a1a1;
	                box-shadow: inset 0px 1px 0px 0px #a1a1a1;
	                background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ffffff), color-stop(1, #FAFAFA) );
	                background: -moz-linear-gradient( center top, #ffffff 5%, #FAFAFA 100% );
	                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr= '#ffffff', endColorstr= '#FAFAFA' );
	                background-color: #ffffff;
	                -moz-border-radius: 9px;
	                -webkit-border-radius: 9px;
	                border-radius: 9px;
	                border: 2px solid #000000;
	                display: inline-block;
	                color: #000000;
	                font-family: 'Open Sans', Helvetica, Arial, Lucida, sans-serif;
	                font-size: 13px;
	                font-weight: bold;
	                padding: 10px 20px 10px 20px;
	                margin-right: 10px;
	                text-decoration: none;
	                text-shadow: 1px 1px 0px #E1E1E1;
	                text-align: center;
	                box-shadow: 4px 4px 4px grey;
	                cursor:pointer;
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

    <div id="middle-container-diallerview" >

        <div id="logo" style="height: 100px; text-align: center;" ></div>
        <div style="height: 50px; text-align: center;">
        <asp:Button ID="btnHome" runat="server" CssClass="form-button" Visible="true" Text="Home" UseSubmitBehavior="false" />&nbsp;&nbsp;
        <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="return ConfirmLeave();" UseSubmitBehavior="false" />
	    </div>
    
    <h1>Customer Details</h1>
    

    <br />
        
   <%--Added By Pradeep Singh against ticket #66945.--%>
   <asp:Button ID="btnGenerateReportExcel" runat="server" style="margin-top: 0px;position: absolute;left: 32%;z-index: 5;top: 258px;width: 14%;" Text="Export" CssClass="ozrol-btn" UseSubmitBehavior="false" ValidationGroup="report" />
     
  <asp:UpdatePanel ID="pnlResults" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
  <ContentTemplate>

  <div style="height: 100%; text-align: center;">
    <table class="form-table" cellspacing="0" summary="">
        <tr>
            <td class="form-submit-td" style="text-align: center;">
                <asp:Button ID="btnAddCustomer" style="width:14%;margin-left: 12%;" runat="server" CssClass="form-button" Text="Add Customer" UseSubmitBehavior="false" />
            </td>
        </tr>
        <tr>
            <td class="form-submit-td" style="text-align: center;">
                <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Text=""></asp:Label>
            </td>
        </tr>
     </table>
  </div>

  <div class="form" style="text-align: center;">  
            <div class="step" style="width: 100%">
                <asp:Panel  style="width:100%" runat="server" ID="pnlList" Visible="true">

                    <br />
                    <br />

                    <asp:CheckBox ID="chkShowDiscontinued" runat="server" Text="" AutoPostBack="true" CssClass="checkbox" />Include Discontinued Customers

                    <br />
                    <br />
                    <br />

                    <asp:GridView ID="dgvCustomerList" runat="server" DataKeyNames="CustomerID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" style="width: 100%" 
                         AllowSorting="true" SortColumn="" SortDirection="" CssClass="customers-table" >
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnViewCustomer" runat="server" Text="Update" CommandName="CustomerUpdate"  CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CssClass="form-button" Width="100px" Height="28px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CustomerID" runat="server" HeaderText="Customer ID" Visible="false" /> 
                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" SortExpression="CustomerName" Visible="true" /> 

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Discontinued" text="Discontinued" 
                                            CssClass="gdv_linkButton" OnCommand="dgvCustomerList_LinkSort" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkDiscontinued" runat="server" Checked='<%# Eval("Discontinued") %>' Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="ExternalCustomer" text="External Customer" 
                                            CssClass="gdv_linkButton" OnCommand="dgvCustomerList_LinkSort" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkExternalCustomer" runat="server" Checked='<%# Eval("ExternalCustomer") %>' Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="CollectionFromFactory" text="Collection From Factory" 
                                            CssClass="gdv_linkButton" OnCommand="dgvCustomerList_LinkSort" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCollectionFromFactory" runat="server" Checked='<%# Eval("CollectionFromFactory") %>' Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="FreightPercentage" text="Freight Percentage" 
                                            CssClass="gdv_linkButton" OnCommand="dgvCustomerList_LinkSort" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFreightPercentage" runat="server" text='<%# Format(CDec(Eval("FreightPercentage")), "0.##%") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="CustomerAbbreviation" HeaderText="Customer Abberviation" SortExpression="CustomerAbbreviation" Visible="true" />
                            <asp:BoundField DataField="TradingName" HeaderText="Full Customer Name" SortExpression="TradingName" Visible="true" /> 
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Plantations" text="Plantations" 
                                            CssClass="gdv_linkButton" OnCommand="dgvCustomerList_LinkSort" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPlantations" runat="server" Checked='<%# Eval("Plantations") %>' Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="WholesaleLouvres" text="Wholesale Louvres" 
                                            CssClass="gdv_linkButton" OnCommand="dgvCustomerList_LinkSort" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWholesaleLouvres" runat="server" Checked='<%# Eval("WholesaleLouvres") %>' Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="RetailLouvres" text="Retail Louvres" 
                                            CssClass="gdv_linkButton" OnCommand="dgvCustomerList_LinkSort" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRetailLouvres" runat="server" Checked='<%# Eval("RetailLouvres") %>' Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="PricingCategory" text="Louvre Price Category" 
                                            CssClass="gdv_linkButton" OnCommand="dgvCustomerList_LinkSort" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLouvreCategory" runat="server" Text="<%# GetLouvreCategoryNameByID(DirectCast(GetDataItem(), Customer).LouvreCategoryID) %>" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </asp:Panel>

	        </div>
     </div>   

     </ContentTemplate>
 

    </asp:UpdatePanel>

    <br />
    <div runat="server" id="DivCur1" onclick="javascript:showMe();" style="width: 10px; height: 10px;"></div>
        <div runat="server" id="DivCur2" style="display:none;">
        txtCurState:<asp:TextBox ID="txtCurState" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
        SiteID:<asp:TextBox ID="txtSiteID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
      <asp:TextBox ID="txtGridrowindex" runat="server" CssClass="HideElement" TabIndex="-1" Text=""></asp:TextBox>
                                                                <asp:TextBox ID="txtCurSiteID" runat="server" CssClass="HideElement"  TabIndex="-1" Text="0"></asp:TextBox>
                                                                <asp:TextBox ID="txtCurUserID" runat="server" CssClass="HideElement"  TabIndex="-1" Text="0"></asp:TextBox>
                                                                <asp:TextBox ID="txtCurUserName" runat="server" CssClass="HideElement"  TabIndex="-1" Text=""></asp:TextBox>    

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
