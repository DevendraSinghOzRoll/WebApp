<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PackupDetails.aspx.vb" Inherits="PackupDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxcontroltoolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Packup Details</title>

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
                document.getElementById("<%=lblPackupStatus.ClientID %>").innerHTML = "";
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
        <div id="middle-container-diallerview">

            <div id="logo" style="height: 100px; text-align: center;"></div>
            <div style="height: 50px; text-align: center;">
                <asp:Button ID="btnHome" runat="server" CssClass="form-button" Text="Home" UseSubmitBehavior="false" />
                <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="javascript:return ConfirmLeave();" UseSubmitBehavior="false" />
            </div>

            <div class="form" style="text-align: center;">

                <div style="text-align: center;">
                    <div class="form" style="text-align: center;">
                        <h1>Packup Details</h1>
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table cellspacing="0" class="form-table" summary="">
                                    <tr>
                                        <td class="form-label-td" style="width: 50%; text-align: right;">Packup ID:</td>
                                        <td class="form-field-td" style="width: 50%;"><asp:Label ID="lblPackupID" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-label-td" style="width: 50%; text-align: right;">Packup Date:</td>
                                        <td class="form-field-td" style="width: 50%;">
                                            <asp:TextBox ID="txtPackupDate" runat="server" CssClass="form-field" Width="30%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-label-td" style="width: 50%; text-align: right;">Status:</td>
                                        <td class="form-field-td" style="width: 50%;">
                                            <asp:Label ID="lblPackupStatus" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-label-td" style="width: 50%; text-align: right;">Additional Height:</td>
                                        <td class="form-field-td" style="width: 50%;">
                                            <asp:Label ID="lblPackupHeight" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-label-td" style="width: 50%; text-align: right;">Additional Width:</td>
                                        <td class="form-field-td" style="width: 50%;">
                                            <asp:Label ID="lblPackupWidth" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <br />
                        <asp:GridView ID="dgvPackupDetails" runat="server" DataKeyNames="ID" Width="100%" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" Visible="false" />
                                <asp:BoundField DataField="Date" SortExpression="Date" HeaderText="Date" />
                                <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status" />
                                <asp:BoundField DataField="Height" SortExpression="Height" HeaderText="Height" />
                                <asp:BoundField DataField="Width" SortExpression="Width" HeaderText="Width" />
                            </Columns>
                        </asp:GridView>
                        <br />
                    </div>
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
            </div>

               <div runat="server" id="Div1" style="display:none;">
        <br />
        <asp:TextBox id="txtPackupID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />

    </div>

   

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
            ProductTypeID<asp:TextBox ID="txtProductTypeID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
            <br />
            Job No<asp:TextBox ID="txtJobNo" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
            <br />
        </div>

        <script type="text/javascript">
            $(function () {
                $("[id$=txtPackupDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1d', maxDate: '+5w' });
            });
        </script>



    </form>
</body>
</html>
