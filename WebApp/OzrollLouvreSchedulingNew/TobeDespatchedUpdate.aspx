<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TobeDespatchedUpdate.aspx.vb" Inherits="TobeDespatchedUpdate" Theme="SknGridView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Daily To be Despatched Update</title>
        
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

    <link href="stylesheets/focus.css" rel="stylesheet" type="text/css" />
    <script src="javascript/focus.js" type="text/javascript"></script>
   
    <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
    
        <script language="javascript" type="text/javascript">
            
            //Added By Michael Behar - 15-09-2020
            function checkLabelTextbox(event) {
                document.getElementById("<%=lblStatus.ClientID%>").innerText = "";
            }

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


            function validSave() {
                    

            }


            $("[id$=txtDespatchDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1d', maxDate: '+5w' });


    </script>

    <style type="text/css">

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
            padding: 10px;
        }

        .form-button {
            margin: 5px;
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
            <asp:Button ID="btnHome" runat="server" CssClass="form-button" Text="Home" UseSubmitBehavior="false" Width="8%" />
            <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="javascript:return ConfirmLeave();" UseSubmitBehavior="false" Width="8%" />
	    </div>
    
        <h1>Daily To Be Despatched Update</h1>
        <div class="form" style="text-align: center;">


            <div style="text-align: center;">
                            <table class="form-table" style="width: 100%; text-align: center;" cellspacing="0" summary="">

                                <tr>
                                    <td class="form-label-td" style="width: 50%; text-align: right;">Despatch Date:</td>
                                    <td class="form-label-td" style="width: 50%; text-align: left;">
                                        <asp:TextBox ID="txtDespatchDate" runat="server" CssClass="form-field" Text="" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-label-td" style="width: 50%; text-align: right;">Consignment Note:</td>
                                    <td class="form-label-td" style="width: 50%; text-align: left;">
                                        <asp:TextBox ID="txtConNote" runat="server" CssClass="form-field" Text="" Width="200px" onKeyPress="checkLabelTextbox()" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="form-label-td" style="width: 50%; text-align: right;">
                                        <asp:RadioButton ID="rdoShowToBeDespatched" runat="server" Checked="True" GroupName="CheckBox" AutoPostBack="true" Text="Show To Be Despatched" />
                                    </td>
                                    <td class="form-label-td" style="width: 50%; text-align: left;">
                                        <asp:RadioButton ID="rdoDespatched" runat="server" GroupName="CheckBox" AutoPostBack="true" Text="Show Despatched" />
                                    </td>
                                </tr>

                            </table>

            </div>

            <br />

            <asp:UpdatePanel ID="pnlDetails" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
        
                <div style="text-align: center;">
                    <asp:GridView ID="dgvDespatchedList" runat="server" DataKeyNames="ScheduleID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" >
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="table-row" />
                        <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#01853f" ForeColor="White" CssClass="table-header-row" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField HeaderStyle-Font-Bold="false" DataField="ScheduleID" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol"  /> 
                            <asp:BoundField DataField="BranchName" SortExpression="BranchName" HeaderText="Branch" />
                            <asp:BoundField DataField="ReferenceNumber" SortExpression="ReferenceNumber" HeaderText="Reference No" />
                            <asp:BoundField DataField="ReferenceName" SortExpression="ReferenceName" HeaderText="Customer" />
                            <asp:BoundField DataField="NoOfPanels" SortExpression="NoOfPanels" HeaderText="No Of Panels" />
                            <asp:BoundField DataField="TotalSQM" SortExpression="TotalSQM" HeaderText="Total SqM" />

                            <asp:TemplateField HeaderText="Stop Deliveries">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStopDeliveries" Text='<%#(IIf(CBool(Eval("StopDeliveries")), "<span style=""color:red;font-weight:bold;"">STOP!</span>", "")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Con. Note">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtInternalConNote" Text='<%# Bind("ConNote")%>' DataField="" SortExpression="ConNote" HeaderText="Con. Note" Enabled="false"/>
                                </ItemTemplate>
                            </asp:TemplateField>

                          
                            <asp:BoundField DataField="hidDespatchDate" SortExpression="DespatchDate" HeaderText="Despatch Date" DataFormatString="{0:d MMM yyyy}" />

                            <asp:TemplateField HeaderText="Despatch">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidDespatchID" Value='<%#(Eval("hidDespatchID")) %>' />
                                    <asp:HiddenField runat="server" ID="hidDespatchStatus" Value='<%#(Eval("hidDespatchStatus")) %>' />
                                    <asp:CheckBox runat="server" ID="chkDespatch" CssClass="checkbox" AutoPostBack="true" oncheckedchanged="ChkDespatch_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkEdit" CssClass="checkbox" AutoPostBack="true" oncheckedchanged="ChkEdit_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:BoundField HeaderStyle-Font-Bold="false" DataField="ConNote" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol"  /> 

                        </Columns>
                    </asp:GridView>
                    <table class="form-table" style="width: 100%; text-align: center;" cellspacing="0" summary="">
                    <tr>
                        <td class="form-submit-td"  style="width: 50%; text-align: right;"><img id="imgLoading2" src="images/indicator.gif" width="16px" height="16px" alt="loading" style="visibility: hidden; vertical-align: middle;" />&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" OnClientClick="return cancelchanges();" UseSubmitBehavior="false" Width="15%" />
                        </td>
                        <td class="form-submit-td"  style="width: 50%; text-align: left;"><img id="imgLoading1" src="images/indicator.gif" width="16px" height="16px" alt="loading" style="visibility: hidden; vertical-align: middle;" />&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnSave" runat="server" CssClass="form-button-disabled" Text="Save" UseSubmitBehavior="false" Enabled="False" Width="15%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form-submit-td" colspan="2" style="text-align: center;"><asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red" /></td>
                    </tr>
                    </table>
                </div>

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

    <script type="text/javascript">
        $(function () {
            $("input#txtDespatchDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1d', maxDate: '+5w' });
        });
    </script>


    </form>
</body>
</html>
