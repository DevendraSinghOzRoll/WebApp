<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DailyScheduleUpdate.aspx.vb" Inherits="DailyScheduleUpdate" Theme="SknGridView" %>

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

            $("[id$=txtScheduledDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-3d', maxDate: '+3d' });

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
    
        <h1>Daily Production Schedule Update</h1>

        <div class="form" style="text-align: center;">

        <div style="text-align: center;">
            <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="form-button" UseSubmitBehavior="false" />
        </div>

        <br />

        <div style="text-align: center;">
            <b>Scheduled Date:</b>&nbsp;<asp:TextBox ID="txtScheduledDate" runat="server" CssClass="form-field" Text="" Width="120px" AutoPostBack="true" />
        </div>

        <br />


        <asp:UpdatePanel ID="pnlDetails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
            <div style="text-align: center; width: 100%;">

                    <asp:GridView ID="dgvScheduleList" runat="server" DataKeyNames="ScheduleID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" >
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="40px" />
                        <FooterStyle BackColor="#0B6134" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#0B6134" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />                        
                        <Columns>
                            <asp:BoundField DataField="ScheduleID" SortExpression="ScheduleID" HeaderText="ScheduleID" Visible="false" />
                            <asp:BoundField DataField="ShutterProNumber" SortExpression="ShutterProNumber" HeaderText="Ozroll ID" ItemStyle-VerticalAlign="Middle" />
                            <asp:BoundField DataField="OrderType" SortExpression="OrderType" HeaderText="Order Type" ItemStyle-VerticalAlign="Middle" />
                            <asp:BoundField DataField="BranchName" SortExpression="BranchName" HeaderText="Branch" ItemStyle-VerticalAlign="Middle" />
                            <asp:BoundField DataField="ReferenceNumber" SortExpression="ReferenceNumber" HeaderText="Reference No" ItemStyle-VerticalAlign="Middle" />
                            <asp:BoundField DataField="ReferenceName" SortExpression="ReferenceName" HeaderText="Customer" ItemStyle-VerticalAlign="Middle" />
                            <asp:BoundField DataField="NoOfPanels" SortExpression="NoOfPanels" HeaderText="No Of Panels" ItemStyle-VerticalAlign="Middle" />
                            <asp:BoundField DataField="TotalSQM" SortExpression="TotalSQM" HeaderText="Total SqM" ItemStyle-VerticalAlign="Middle" />
                            <asp:BoundField DataField="ScheduledDate" SortExpression="ScheduledDate" HeaderText="Scheduled Date" DataFormatString="{0:d MMM yyyy}" />
                            <asp:BoundField DataField="Priority" SortExpression="Priority" HeaderText="Priority" />
                            <asp:TemplateField HeaderText="Cutting" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidCuttingID" Value='<%#(Eval("hidCuttingID")) %>' />
                                    <asp:HiddenField runat="server" ID="hidCuttingStatus" Value='<%#(Eval("hidCuttingStatus")) %>' />
                                    <asp:CheckBox runat="server" ID="chkCutting" Checked='<%#Convert.ToBoolean(Eval("Cutting")) %>' Visible='<%# not (Convert.ToBoolean(Eval("Cutting"))) %>' />                                    
                                    <asp:Label runat="server" ID="lblCutting" Text='<%#(Eval("hidCuttingDate")) %>' Visible='<%# Convert.ToBoolean(Eval("Cutting")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pinning" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidPiningID" Value='<%#(Eval("hidPiningID")) %>' />
                                    <asp:HiddenField runat="server" ID="hidPiningStatus" Value='<%#(Eval("hidPiningStatus")) %>' />
                                    <asp:CheckBox runat="server" ID="chkPining" Checked='<%#Convert.ToBoolean(Eval("Pining")) %>' Visible='<%# not (Convert.ToBoolean(Eval("Pining"))) %>' />                                    
                                    <asp:Label runat="server" ID="lblPining" Text='<%#(Eval("hidPiningDate")) %>' Visible='<%# Convert.ToBoolean(Eval("Pining")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prep" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidPrepID" Value='<%#(Eval("hidPrepID")) %>' />
                                    <asp:HiddenField runat="server" ID="hidPrepStatus" Value='<%#(Eval("hidPrepStatus")) %>' />
                                    <asp:CheckBox runat="server" ID="chkPrep" Checked='<%#Convert.ToBoolean(Eval("Prep")) %>' Visible='<%# not (Convert.ToBoolean(Eval("Prep"))) %>' />
                                    <asp:Label runat="server" ID="lblPrep" Text='<%#(Eval("hidPrepDate")) %>' Visible='<%# Convert.ToBoolean(Eval("Prep")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assembly" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidAssemblyID" Value='<%#(Eval("hidAssemblyID")) %>' />
                                    <asp:HiddenField runat="server" ID="hidAssemblyStatus" Value='<%#(Eval("hidAssemblyStatus")) %>' />
                                    <asp:CheckBox runat="server" ID="chkAssembly" Checked='<%#Convert.ToBoolean(Eval("Assembly")) %>' Visible='<%# not (Convert.ToBoolean(Eval("Assembly"))) %>' />
                                    <asp:Label runat="server" ID="lblAssembly" Text='<%#(Eval("hidAssemblyDate")) %>' Visible='<%# Convert.ToBoolean(Eval("Assembly")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Hinging" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidHingingID" Value='<%#(Eval("hidHingingID")) %>' />
                                    <asp:HiddenField runat="server" ID="hidHingingStatus" Value='<%#(Eval("hidHingingStatus")) %>' />
                                    <asp:CheckBox runat="server" ID="chkHinging" Checked='<%#Convert.ToBoolean(Eval("Hinging")) %>' Visible='<%# not (Convert.ToBoolean(Eval("Hinging"))) %>' />
                                    <asp:Label runat="server" ID="lblHinging" Text='<%#(Eval("hidHingingDate")) %>' Visible='<%# Convert.ToBoolean(Eval("Hinging")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PackUp" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidPackUpID" Value='<%#(Eval("hidPackUpID")) %>' />
                                    <asp:HiddenField runat="server" ID="hidPackUpStatus" Value='<%#(Eval("hidPackUpStatus")) %>' />
                                    <asp:CheckBox runat="server" ID="chkPackUp" Checked='<%#Convert.ToBoolean(Eval("PackUp")) %>' Visible='<%# not (Convert.ToBoolean(Eval("PackUp"))) %>' />
                                    <asp:Label runat="server" ID="lblPackUp" Text='<%#(Eval("hidPackUpDate")) %>' Visible='<%# Convert.ToBoolean(Eval("PackUp")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Framing" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidFramingID" Value='<%#(Eval("hidFramingID")) %>' />
                                    <asp:HiddenField runat="server" ID="hidFramingStatus" Value='<%#(Eval("hidFramingStatus")) %>' />
                                    <asp:CheckBox runat="server" ID="chkFraming" Checked='<%#Convert.ToBoolean(Eval("Framing")) %>' Visible='<%# not (Convert.ToBoolean(Eval("Framing"))) %>' />
                                    <asp:Label runat="server" ID="lblFraming" Text='<%#(Eval("hidFramingDate")) %>' Visible='<%# Convert.ToBoolean(Eval("Framing")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QC" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidQCID" Value='<%#(Eval("hidQCID")) %>' />
                                    <asp:HiddenField runat="server" ID="hidQCStatus" Value='<%#(Eval("hidQCStatus")) %>' />
                                    <asp:CheckBox runat="server" ID="chkQC" Checked='<%#Convert.ToBoolean(Eval("QC")) %>' Visible='<%# not (Convert.ToBoolean(Eval("QC"))) %>' />
                                    <asp:Label runat="server" ID="lblQC" Text='<%#(Eval("hidQCDate")) %>' Visible='<%# Convert.ToBoolean(Eval("QC")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Wrapping" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidWrappingID" Value='<%#(Eval("hidWrappingID")) %>' />
                                    <asp:HiddenField runat="server" ID="hidWrappingStatus" Value='<%#(Eval("hidWrappingStatus")) %>' />
                                    <asp:CheckBox runat="server" ID="chkWrapping" Checked='<%#Convert.ToBoolean(Eval("Wrapping")) %>' Visible='<%# not (Convert.ToBoolean(Eval("Wrapping"))) %>' />
                                    <asp:Label runat="server" ID="lblWrapping" Text='<%#(Eval("hidWrappingDate")) %>' Visible='<%# Convert.ToBoolean(Eval("Wrapping")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Despatch" ItemStyle-VerticalAlign="Middle">
                                <%-- Uncommented by Michael Behar - Ticket #62859 - Code already existed - 27-11-2020 --%>
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidDespatchID" Value='<%#(Eval("hidDespatchID")) %>' />
                                    <asp:HiddenField runat="server" ID="hidDespatchStatus" Value='<%#(Eval("hidDespatchStatus")) %>' />
                                    <asp:CheckBox runat="server" ID="chkDespatch" Checked='<%#Convert.ToBoolean(Eval("Despatch")) %>' Visible='<%# not (Convert.ToBoolean(Eval("Despatch"))) %>' />
                                    <asp:Label runat="server" ID="lblDespatch" Text='<%#(Eval("hidDespatchDate")) %>' Visible='<%# Convert.ToBoolean(Eval("Despatch")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                                                           
                <table class="form-table" style="width: 100%; text-align: center;" cellspacing="0" summary="">
                    <tr>
                        <td class="form-submit-td"  style="width: 50%; text-align: right;"><img id="imgLoading2" src="images/indicator.gif" width="16px" height="16px" alt="loading" style="visibility: hidden; vertical-align: middle;" />&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" OnClientClick="return cancelchanges();" UseSubmitBehavior="false" />
                        </td>
                        <td class="form-submit-td"  style="width: 50%; text-align: left;"><img id="imgLoading1" src="images/indicator.gif" width="16px" height="16px" alt="loading" style="visibility: hidden; vertical-align: middle;" />&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnSave" runat="server" CssClass="form-button" Text="Save" UseSubmitBehavior="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form-submit-td" colspan="2" style="text-align: left;"><asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red" /></td>
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
        ProductTypeID<asp:TextBox id="txtProductTypeID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
    </div>


    <script type="text/javascript">
        $(function () {
            $("input#txtScheduledDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-3d', maxDate: '+3d' });
        });
    </script>


    </form>
</body>
</html>
