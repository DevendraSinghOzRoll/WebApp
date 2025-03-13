<%@ Page Language="VB" AutoEventWireup="false" CodeFile="StockPick.aspx.vb" Inherits="StockPick" Theme="SknGridView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxcontroltoolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Daily Production Schedule Update</title>
        
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta name="format-detection" content="telephone=no" />
    
    <meta name="viewport" content="width=1200">

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


           function IsNumericKey(evt, el,DecimalPlace) {            
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
                   evt.key == "ArrowLeft" || evt.key == "ArrowRight" || evt.key == "Home" || evt.key == "End")
               {
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
                if (DecimalPlace > 0){
                   if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                     return false;
                   }
                }
               //For adding Commas to the value
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

    </script>
    <style type="text/css">
        .form-Field-process {
         background-color: #F2F2F2;        
         padding: 5px 10px 5px 10px;
         text-align: left;
         vertical-align: middle;
        width: 35%;
       }
        .auto-style1 {
            background-color: #F2F2F2;
            padding: 5px 10px 5px 10px;
            text-align: left;
            vertical-align: middle;
            width: 15%;
            height: 25px;
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
    <div id="middle-container-1" >

        <div id="logo" style="height: 100px; text-align: center;" ></div>
        <div style="height: 50px; text-align: center;">
            <asp:Button ID="btnHome" runat="server" CssClass="form-button" Text="Home" UseSubmitBehavior="false" />
            <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="javascript:return ConfirmLeave();" UseSubmitBehavior="false" />
	    </div>
    
        <h1><asp:Label ID="lblHeader" runat="server" Text="Picking List for"/></h1>

        <div class="form" style="text-align: center;">

        <div style="text-align: center;">
        </div>

        <br />

        <div style="text-align: center;">          
          <asp:Panel ID="Panel1" runat="server">         
            <table class="form-table" style="width: 100%; text-align: center;" cellspacing="0" summary="">
                    <tr>
                        <td class="form-submit-td"  style="text-align: right;width:15%;">
                            Customer:
                        </td>
                        <td class="form-submit-td"  style="text-align: left;width:15%;">
                            <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                        </td>
                            <td class="form-submit-td"  style="text-align: right;width:15%;">
                            Customer Ref:
                        </td>
                        <td class="form-submit-td"  style="text-align: left;width:15%;">
                            <asp:Label ID="lblCusReference" runat="server"></asp:Label>
                        </td>
                        <td class="form-submit-td"  style="text-align: right;width:15%;">
                            Order Ref:
                        </td>
                        <td class="form-submit-td"  style="text-align: left;width:15%;">
                            <asp:Label ID="lblOrderRef" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-submit-td"  style="text-align: right;width:15%;">
                            Order Type:
                        </td>
                        <td class="form-submit-td"  style="text-align: left;width:15%;">
                            <asp:Label ID="lblOrderType" runat="server"></asp:Label>
                        </td>
                            <td class="form-submit-td"  style="text-align: right;width:15%;">
                            Total SQM:
                        </td>
                        <td class="form-submit-td"  style="text-align: left;width:15%;">
                            <asp:Label ID="lblTotalSQM" runat="server"></asp:Label>
                        </td>
                        <td class="form-submit-td"  style="text-align: right;width:15%;">
                            Total Panels:
                        </td>
                        <td class="form-submit-td"  style="text-align: left;width:15%;">
                            <asp:Label ID="lblPanels" runat="server"></asp:Label>
                        </td>
                    </tr>
                <asp:Panel ID="hidethis" runat="server" Visible="false">
                <tr>
                    <td class="form-submit-td"  style="text-align: right;width:15%;">
                        Picking List For : 
                    </td>
                    
                    <td colspan="2"  style="text-align: left;width:15%;">
                        <asp:DropDownList ID="cboProcess" CssClass="form-Field-process" runat="server" AutoPostBack="True"></asp:DropDownList>
                    </td>
               
                    <td>
                         <asp:Button ID="btnAddStock" runat="server" CssClass="form-button"  Visible="false" Text="Add a Row"  UseSubmitBehavior="false" />
                    </td>
                </tr>
                </asp:Panel>
                </table>
         </asp:Panel>
           
        </div>

        <br />


        <asp:UpdatePanel ID="pnlDetails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
            <div style="text-align: center; width: 100%;">
                    <asp:GridView ID="dgvStockPick" runat="server" DataKeyNames="StockUsageID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" >
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#0059A9" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />                        
                        <Columns>
                            <asp:BoundField DataField="StockUsageID" SortExpression="StockUsageID" HeaderText="StockUsageID" ItemStyle-VerticalAlign="Middle" Visible="false" />                         
                          <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description" ItemStyle-VerticalAlign="Middle" />
                            <asp:BoundField DataField="OptimiserQuantity" SortExpression="OptimiserQuantity" HeaderText="Optimiser Quantity" ItemStyle-VerticalAlign="Middle" />
                            <asp:TemplateField SortExpression="ActualQuantity" HeaderText="Quantity Picked" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>                                   
                                   <asp:TextBox id="txtQuantity" runat="server" Width="70px"></asp:TextBox>                                 
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                                                           
                <table class="form-table" style="width: 100%; text-align: center;" cellspacing="0" summary="">
                    <tr>
                        <td class="form-submit-td"  style="width: 50%; text-align: right;">
                            <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" OnClientClick="return cancelchanges();" Width="100px" UseSubmitBehavior="false" />
                        </td>
                        <td class="form-submit-td"  style="width: 50%; text-align: left;">
                            <asp:Button ID="btnSave" runat="server" CssClass="form-button" Text="Save" Width="100px" UseSubmitBehavior="false" />
                        </td>
<%--                         <td class="form-submit-td"  style="width: 30%; text-align: left;">
                         <asp:Button ID="btnAddStockNew" runat="server" CssClass="form-button"  Text="Add a Row"  UseSubmitBehavior="false" />
                        </td>--%>
                    </tr>
                    <tr>
                        <td class="form-submit-td" colspan="2" style="text-align: left;">
                            <asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red" /></td>
                    </tr>
                </table>
            </div>

        </ContentTemplate>
        </asp:UpdatePanel>

        </div>
          <asp:Panel ID="pnlBottom" runat="server" Visible="true" CssClass="form-table">         
             <asp:Label ID="lblPanelBottom" runat="server" Font-Size="Large" Font-Bold="True" Font-Underline="True"></asp:Label>
            <table style="width: 100%;text-align: center;border: 5px solid #0F89D0;" cellspacing="0" summary="" >
                    <tr>
                        <td class="form-Field-process"  style="text-align: left;width:15%;">
                        </td>
                        <td class="form-Field-process"  style="text-align: left;width:15%;">
                        </td>
                        <td class="form-Field-process"  style="text-align: right;width:15%;">
                            PowderCoater:
                        </td>
                        <td class="form-Field-process"  style="text-align: left;width:15%;">
                            <asp:DropDownList ID="cboPC" runat="server" CssClass="form-select" Width="120px"></asp:DropDownList>
                        </td>
                        <td class="form-Field-process"  style="text-align: left;width:15%;">
                        </td>
                        <td class="form-Field-process"  style="text-align: left;width:15%;">
                        </td>
                    </tr>
        
                    <tr>
                        <td class="form-Field-process" style="text-align: right; width: 15%;">Pick Date:</td>
                        <td class="form-Field-process" style="text-align: left; width: 15%;">
                            <asp:TextBox ID="txtPickDate" runat="server" CssClass="form-field" style="width: 60%" MaxLength="12"></asp:TextBox>
                        </td>
                        <td class="form-Field-process" style="text-align: right; width: 15%;">Despatch Date:</td>
                        <td class="form-Field-process" style="text-align: left; width: 15%;">
                            <asp:TextBox ID="txtDespatchDate" runat="server" CssClass="form-field" style="width: 60%" MaxLength="12"></asp:TextBox>
                        </td>
                        <td class="form-Field-process" style="text-align: right; width: 15%;">ETA Date:</td>
                        <td class="form-Field-process" style="text-align: left; width: 15%;">
                            <asp:TextBox ID="txtReturnDate" runat="server" CssClass="form-field" style="width: 60%" MaxLength="12"></asp:TextBox>
                        </td>
                    </tr>
        
                </table>
         </asp:Panel>
    </div>

  <ajaxcontroltoolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnAddStock" PopupControlID="pnlAddStock" BackgroundCssClass="modalBackground">
                </ajaxcontroltoolkit:ModalPopupExtender>

              <asp:Panel ID="pnlAddStock" runat="server" CssClass="modalPopup" Style="display: none; left: 35%;top: 45%;width: 30% !important;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="form-table" cellspacing="0" summary="">
                                <tr>
                                    <td class="form-field-td-p2">Stock Item&nbsp;:&nbsp;
                                    </td>
                                    <td class="form-field-td-p2">
                                        <asp:DropDownList ID="cboStockItem" runat="server" Width="120%" CssClass="form-select"></asp:DropDownList>
                                    </td>
                                    <td class="form-field-td-p2"></td>
                                    <td class="form-field-td-p2"></td>
                                </tr>
                                <tr>
                                    <td class="form-field-td-p2">Quantity To Book Out&nbsp;:&nbsp;
                                    </td>
                                    <td class="form-field-td-p2">
                                        <asp:TextBox ID="txtQuantityBookOut" Width="50%" onkeydown="return IsNumericKey(event,this,2)" runat="server" CssClass="form-field"></asp:TextBox>
                                    </td>
                                    <td class="form-field-td-p2"></td>
                                    <td class="form-field-td-p2"></td>
                                </tr>
                                <tr>
                                    <td class="form-submit-td" style="width: 50%">
                                        <div style="float: right;">
                                            <asp:Button runat="server" ID="btnCancelAddStock" Text="Cancel" CssClass="form-button" />
                                        </div>
                                    </td>
                                    <td class="form-submit-td">
                                        <div style="float: left;">
                                            <asp:Button runat="server" ID="btnSaveAddStock" Text="Save" CssClass="form-button" />
                                        </div>
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>

                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSaveAddStock" />
                            <asp:PostBackTrigger ControlID="btnCancelAddStock" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>


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
        StockUsageID<asp:TextBox id="txtStockUsage" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
        ProductTypeID<asp:TextBox id="txtProductTypeID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
        Job No<asp:TextBox id="txtJobNo" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
    </div>


   <script type="text/javascript">
            $(function () {
                $("input#txtPickDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1d', maxDate: '+5w' });
                $("input#txtDespatchDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1d', maxDate: '+5w' });
                $("input#txtReturnDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1d', maxDate: '+5w' });
            });
        </script>



    </form>
</body>
</html>
