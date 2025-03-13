<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Packup.aspx.vb" Inherits="PackupDate" %>

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
    <link href="stylesheets/checkBox_Yellow.css" rel="stylesheet" />
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
            <asp:Button ID="btnHome" runat="server" CssClass="form-button" Text="Home" UseSubmitBehavior="false" />
            <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="javascript:return ConfirmLeave();" UseSubmitBehavior="false" />
	    </div>
    
        <h1>Packup</h1>

            <h2><asp:Label ID="lblProdSchedID" runat="server"></asp:Label></h2>

        <div class="form" style="text-align: center;">

        <div style="text-align: center;">
            <div class="form" style="text-align: center;">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table cellspacing="0" class="form-table" summary="">
                            <tr>
                                <td class="form-label-td" style="width: 40%;text-align:right;">Type: </td>
                                        
                                 <td class="form-label-td" colspan="2">
                                     <asp:Label ID="lblType" runat="server"></asp:Label>                                     
                                </td>                                 
                             </tr>
                             <tr>
                                <td class="form-label-td" style="width: 40%;text-align:right;">Height: </td>                                        
                                 <td class="form-label-td" colspan="2">
                                       <asp:Label ID="lblHeight" runat="server"></asp:Label>
                                </td>                                 
                             </tr>                            
                             <tr>
                                <td class="form-label-td" style="width: 40%;text-align:right;">Width:</td>
                                        
                                 <td class="form-label-td" colspan="2">
                                       <asp:Label ID="lblWidth" runat="server"></asp:Label>
                                </td>                                 
                             </tr>
                             <tr>
                                <td class="form-label-td" style="width: 40%;text-align:right;">DLi/CL: </td>
                                        
                                 <td class="form-label-td" colspan="2">
                                       <asp:Label ID="lblDLiCL" runat="server"></asp:Label>
                                </td>                                  
                             </tr>
                             <tr>
                                <td class="form-label-td" style="width: 40%;text-align:right;">Double Bottom Rail: </td>                                        

                                 <td class="form-label-td" style="width: 10%;text-align:left;">
                                     <div class="roundedTwo">     
                                                 <asp:CheckBox ID="chkDoubleRail" runat="server" Text="" Checked="true" AutoPostBack="True" />
                                       <label for="chkDoubleRail"></label>                                        
                                     </div>                                         
                                </td>
                                    <td class="form-label-td" style="width: 40%;">
                                        <asp:Label ID="lblCheckBoxStatus" runat="server" Text="Present" Font-Bold="True" Font-Size="Medium" ForeColor="#FB9E25"></asp:Label>
                                    </td>
                             </tr>                            
                            <tr>
                                <td class="form-label-td" style="width: 40%;text-align:right;">Show Special Requirements: </td>
                                        
                                 <td class="form-label-td" colspan="2">
                                         <asp:Label ID="lblShowSpecialReqTitle" runat="server"></asp:Label>
                                </td>                                
                            </tr>
                            <tr>
                                <td class="form-label-td" colspan="3" style="text-align: center;">
                                    <asp:Button ID="btnViewJobDetails" runat="server" CssClass="form-button" Text="Job Details" UseSubmitBehavior="false" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
            </div>
            <br />
            <br />
        </div>

                            <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" OnClientClick="return cancelchanges();" UseSubmitBehavior="false" />
                            <asp:Button ID="btnSave" runat="server" CssClass="form-button" Text="Save" UseSubmitBehavior="false" />

        <br />
        <div style="text-align: center;">          
           
        </div>

             <asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red" />

        <br />


        </div>
    </div>

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
