<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CustomerSampleTracking.aspx.vb" Inherits="CustomerSampleTracking"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer Sample</title>
        
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

         function btnMarkAsShip() {
         __doPostBack('<%=btnMarkAsShipped.ClientID %>', '');
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
         
 
</script>




    <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <style type="text/css">

        #pnlDetailsCustomerSample tbody tr td{
            padding: 5px;
        }

        .table-button {
            margin: 5px 10px 5px 5px;
        }

        .table-header-row th {
            padding: 10px 10px 10px 10px;
            color: white;
            font-weight: normal;
            height: 20px;
            font-size: 1.1em;
        }

        .table-row td {
            padding-right: 10px;
        }

        #dgvCustomerSample{
            border-collapse: separate;
        }

        #dgvCustomerSample tr:nth-child(1) th:nth-child(1){
             border-top-left-radius: 5px;
             border-bottom-left-radius: 5px;
        }

        #dgvCustomerSample tr:nth-child(1) th:nth-child(16){
             border-top-right-radius: 5px;
             border-bottom-right-radius: 5px;
        }

    </style>



</head>
<body onload="">
    <form runat="server" id="formCustomerSampleTracking" method="post">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    <Scripts>
    <asp:ScriptReference Path="javascript/fixfocus.js" />
    </Scripts>
    </asp:ScriptManager>
    <div id="middle-container-diallerview" >

        <div id="logo" style="height: 100px; text-align: center;" ></div>
        <div style="height: 50px; text-align: center;">
            <asp:Button ID="btnHome" runat="server" CssClass="form-button" Text="Home" UseSubmitBehavior="false" />
            <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="javascript:return ConfirmLeave();" UseSubmitBehavior="false" />
	    </div>
    
        <h1>Customer Sample</h1>

        <div class="form" style="text-align: center;">
           
            <asp:Panel ID="CustomersampleSearch" runat="server">
            <table class="form-table" style="width: 35%; text-align: center;" summary="">
                <tr>
                    <td class="form-submit-td" style="text-align: right; width:25%">
                        <label>
                        Un-Shipped :</label><asp:RadioButton ID="rdoUnshipped" runat="server" AutoPostBack="true" Checked="True" GroupName="Shipping" OnCheckedChanged="rdoUnshipped_CheckedChanged" />
                    </td>
                    <td class="form-submit-td" style="text-align: left; width:26%">&nbsp;<label>Shipped :</label>
                        <asp:RadioButton ID="rdoShipped" runat="server" AutoPostBack="true" GroupName="Shipping" OnCheckedChanged="rdoShipped_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td class="form-submit-td" colspan="2" style="text-align: center; ">
                        <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="" />
                    </td>
                </tr>
            </table>
            </asp:Panel>

            <br />
            <asp:UpdatePanel ID="pnlDetailsCustomerSample" runat="server" >
            <ContentTemplate>
        
               <asp:HiddenField ID="btnDetails2" runat="server" />

                <div style="text-align: center;">
                    <asp:GridView ID="dgvCustomerSample" runat="server" DataKeyNames="OrderID" AutoGenerateColumns="False" Width="100%" ShowHeaderWhenEmpty="True" >
                          <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#0059A9" ForeColor="White" HorizontalAlign="Center"/>
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" Height="35px" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="OrderID" SortExpression="OrderID" HeaderText="OrderID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" >

                            <HeaderStyle Width="50px" />
                            </asp:BoundField>

                            <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="First Name" />
                            <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="Last Name" />
                            <asp:BoundField DataField="Street" SortExpression="Street" HeaderText="Street" />
                            <asp:BoundField DataField="Suburb" SortExpression="Suburb" HeaderText="Suburb" />
                            <asp:BoundField DataField="State" SortExpression="State" HeaderText="State" />
                            <asp:BoundField DataField="PostCode" SortExpression="PostCode" HeaderText="Post Code" />
                            <asp:BoundField DataField="PhoneNumber" SortExpression="PhoneNumber" HeaderText="Phone Number" />
                            <asp:BoundField DataField="MobileNumber" SortExpression="MobileNumber" HeaderText="Mobile Number" />
                            <asp:BoundField DataField="Colours" SortExpression="Colours" HeaderText="Colours" />
                            <asp:BoundField DataField="Product" SortExpression="Product" HeaderText="Product" />
                            <asp:BoundField DataField="RequestedDate" SortExpression="RequestedDate" HeaderText="Requested Date" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="ReceivedDate" SortExpression="ReceivedDate" HeaderText="Received Date" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="ShippedDate" SortExpression="ShippedDate" HeaderText="Shipped Date" DataFormatString="{0:d}" />

                            <asp:BoundField DataField="ConNote" SortExpression="ConNote" HeaderText="Con. Note" />
                            <asp:BoundField DataField="Courier" SortExpression="Courier" HeaderText="Courier" />

                            <asp:TemplateField HeaderText="Con. Note">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtConNote" runat="server" Text='<%# Bind("ConNote") %>' CssClass="form-field" Width="80%" AutoPostBack="true" OnTextChanged="txtConNote_TextChanged"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Courier">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCourier" runat="server" Text='<%# Bind("Courier") %>' CssClass="form-field" Width="80%" AutoPostBack="true" OnTextChanged="txtCourier_TextChanged"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Shipped">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="shipped" />
                                    <asp:CheckBox runat="server" ID="chkShipped" CssClass="checkbox" CommandName="CheckShipped" Checked='<%#Convert.ToBoolean(Eval("Shipped")) %>' OnCheckedChanged="chkShipped_OnCheckedChanged" autopostback="true" Enabled="false" />
                                </ItemTemplate>
                                <HeaderStyle Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Job Details">
                                <ItemTemplate>
                                    <asp:button runat="server" ID="btnDetails" Text="Details" CommandName="Details"  CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CssClass="form-button" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table class="form-table" style="width: 100%; text-align: center;" summary="">
                    <tr>
                         <td class="form-submit-td"  style="width: 30%; text-align: center;">                       
   						 <asp:Button ID="btnMarkAsShipped" runat="server" CssClass="form-button-disabled" Text="Mark As Shipped" UseSubmitBehavior="false" enabled="false" />
						     <br />
                             <img id="imgLoading2" src="images/indicator.gif" width="16px" height="16px" alt="loading" style="visibility: hidden; vertical-align: middle;" />
                        </td>                   
                    </tr>
                    <tr>
                        <td class="form-submit-td" style="text-align: center;"><asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red" /></td>
                    </tr>
                    </table>
                </div>

            <ajaxcontroltoolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" BehaviorID="modal1" TargetControlID="btnDetails2" PopupControlID="pnlDetails" BackgroundCssClass="modalBackground">
            </ajaxcontroltoolkit:ModalPopupExtender>

            <asp:Panel ID="pnlDetails" runat="server" CssClass="modalPopup" Width="30%" Style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="form-table" cellspacing="0" summary="">
                            <tr>
                                <td class="form-label-td-p2" style="width: 40%; text-align: right;">
                                    Notes: </td>
                                <td class="form-field-td-p2" style="width: 40%;">
                                    <asp:Label ID="lblNotes" runat="server"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td class="form-label-td-p2" style="text-align: center;" colspan="2">
                                    &nbsp;<asp:Button ID="btnCancelNotes" runat="server" CssClass="form-button" Text="Cancel" OnClick="btnCancelNotes_Click" UseSubmitBehavior="false" />
                                 </td>
                            </tr>
                        </table>                                               
                    </ContentTemplate>

                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnDetails2" />
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
                <br />
        ProductTypeID<asp:TextBox id="txtProductTypeID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />

    </div>

    </form>
</body>

</html>
