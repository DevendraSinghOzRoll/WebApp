<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProductionScheduleList.aspx.vb" Inherits="ProductionScheduleList" Theme="SknGridView" %>
<%@ Register TagPrefix="ext" Namespace="Extensions" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>OzRoll Production Schedule List</title>

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

            $("[id$=txtActualShippingDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+5w' });
            $("[id$=txtOrderDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+1d' });
            $("[id$=txtInvoiceDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+1d' });
            $("[id$=txtStartDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+10w' });
            $("[id$=txtEndDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+10w' });

        });

        if (window.history.forward(1) != null)
            window.history.forward(1);

        function openreport() {
            document.getElementById("<%=btnReport.ClientID%>").disabled = true;
            document.getElementById("<%=btnReport.ClientID%>").className = "form-button-disabled";
            document.getElementById("<%=btnReport.ClientID%>").value = "Submitting";
            document.getElementById("<%=btnAdd.ClientID %>").disabled = true;
            document.getElementById("<%=btnAdd.ClientID %>").className = "form-button-disabled";
            document.getElementById("<%=btnAdd.ClientID %>").value = "Add Job";
            document.getElementById("<%=btnSearch.ClientID %>").disabled = true;
            document.getElementById("<%=btnSearch.ClientID %>").className = "form-button-disabled";
            document.getElementById("<%=btnSearch.ClientID %>").value = "Search";
            document.getElementById("<%=btnClear.ClientID %>").disabled = true;
            document.getElementById("<%=btnClear.ClientID %>").className = "form-button-disabled";
            document.getElementById("<%=btnClear.ClientID %>").value = "Clear";
            document.getElementById("imgLoading2").style.visibility = "visible";
            __doPostBack('<%=btnReport.ClientID%>', '');
            return true;
        }

        function addnew() {
            //condition added by surendra 23-07-2020
            var userType = document.getElementById("hfCustomerId").value
            //added by surendra dt:02/06/2023 check user permission
            var adminPermission = document.getElementById("hfAdminPermission").value
            if (userType == 0 && adminPermission === "True") {

                document.getElementById("<%=btnReport.ClientID%>").disabled = true;
                document.getElementById("<%=btnReport.ClientID%>").className = "form-button-disabled";
                document.getElementById("<%=btnReport.ClientID%>").value = "Report";
                document.getElementById("<%=btnAdd.ClientID %>").disabled = true;
                document.getElementById("<%=btnAdd.ClientID %>").className = "form-button-disabled";
                document.getElementById("<%=btnAdd.ClientID %>").value = "Submitting";
            }
            else {
                document.getElementById("<%=btnAddJobExtarnalCustomer.ClientID %>").disabled = true;
                document.getElementById("<%=btnAddJobExtarnalCustomer.ClientID %>").className = "form-button-disabled";
                document.getElementById("<%=btnAddJobExtarnalCustomer.ClientID %>").value = "Submitting";
            }
            document.getElementById("<%=btnSearch.ClientID %>").disabled = true;
            document.getElementById("<%=btnSearch.ClientID %>").className = "form-button-disabled";
            document.getElementById("<%=btnSearch.ClientID %>").value = "Search";
            document.getElementById("<%=btnClear.ClientID %>").disabled = true;
            document.getElementById("<%=btnClear.ClientID %>").className = "form-button-disabled";
            document.getElementById("<%=btnClear.ClientID %>").value = "Clear";
            document.getElementById("imgLoading2").style.visibility = "visible";

            //condition added by surendra 23-07-2020
            if (userType == 0 && adminPermission === "True") {
                __doPostBack('<%=btnAdd.ClientID %>', '');
            }
            else {
                __doPostBack('<%=btnAddJobExtarnalCustomer.ClientID %>', '');
            }
            return true;
        }

        function ValidateAll() {
            var bolValid = 1;
            var strMessage = "";
            var userType = document.getElementById("hfCustomerId").value
            document.getElementById("<%=lblStatus.ClientID %>").innerHTML = "";
//	        if (document.getElementById("<%=txtActualShippingDate.ClientID %>").value.replace(/^\s+|\s+$/g, '') == "") {
//	            strMessage = "Please enter the Actual Shipping date.";
//	            bolValid = 0;
//	        } else {
//	            if (isNaN(Date.parse(document.getElementById("<%=txtActualShippingDate.ClientID %>").value)) == true) {
//	                strMessage = "Please enter a valid Actual Shipping date.";
//	                bolValid = 0;
//	            }
//	        }
//	        if (bolValid == 0) {
//	            if (document.getElementById("<%=txtOrderDate.ClientID %>").value.replace(/^\s+|\s+$/g, '') == "") {
//	                strMessage = "Please enter the Order date.";
//	            } else {
//	                if (isNaN(Date.parse(document.getElementById("<%=txtOrderDate.ClientID %>").value)) == true) {
//	                    strMessage = "Please enter a valid Order date.";
//	                } else {
//	                    bolValid = 1;
//	                }
//	            }
//	        }
//	        if (bolValid == 0) {
//	            if (document.getElementById("<%=cboCustomerName.ClientID %>").selectedIndex <= 0) {
//	                strMessage = "Please select the Customer Name.";
//	            } else {
//	                bolValid = 1;
//	            }
//	        }
//	        if (bolValid == 0) {
//	            if (document.getElementById("<%=cboOrderStatus.ClientID %>").selectedIndex <= 0) {
            //	                strMessage = "Please select the Order Status.";
            //	            } else {
            //	                bolValid = 1;
            //	            }
            //	        }

            if (bolValid == 1) {
                //Condition Added by surenda -24-05-2020
                //commented by surendra -8-09-2020
                <%--if (userType == 0) {
                    document.getElementById("<%=btnReport.ClientID%>").disabled = true;
                    document.getElementById("<%=btnReport.ClientID%>").className = "form-button-disabled";
                    document.getElementById("<%=btnReport.ClientID%>").value = "Report";
                    document.getElementById("<%=btnAdd.ClientID %>").disabled = true;
                    document.getElementById("<%=btnAdd.ClientID %>").className = "form-button-disabled";
                    document.getElementById("<%=btnAdd.ClientID %>").value = "Add Job";
                }
                else {
                    document.getElementById("<%=btnAddJobExtarnalCustomer.ClientID %>").disabled = true;
                    document.getElementById("<%=btnAddJobExtarnalCustomer.ClientID %>").className = "form-button-disabled";
                    document.getElementById("<%=btnAddJobExtarnalCustomer.ClientID %>").value = "Submitting";
                }--%>
                //condition commented by surendra -25-07-202
	            <%--document.getElementById("<%=btnSearch.ClientID %>").disabled = true;
	            document.getElementById("<%=btnSearch.ClientID %>").className = "form-button-disabled";
	            document.getElementById("<%=btnSearch.ClientID %>").value = "Submitting";
	            document.getElementById("<%=btnClear.ClientID %>").disabled = true;
	            document.getElementById("<%=btnClear.ClientID %>").className = "form-button-disabled";
	            document.getElementById("<%=btnClear.ClientID %>").value = "Clear";--%>

                __doPostBack('<%=btnSearch.ClientID %>', '');
                return true;
            }
            else {
                if (strMessage != "") {
                    document.getElementById("dialog-alert-message").innerHTML = strMessage;
                    $("#dialog-alert").dialog("open");
                }
                return false;
            }
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
            document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
            document.getElementById("<%=btnAdd.ClientID %>").className = "form-button";
            document.getElementById("<%=btnAdd.ClientID %>").value = "Add Job";
            var customerId = document.getElementById("<%#hfCustomerId.ClientID%>").value

            document.getElementById("<%=btnReport.ClientID%>").disabled = false;
            document.getElementById("<%=btnReport.ClientID%>").className = "form-button";
            document.getElementById("<%=btnReport.ClientID%>").value = "Report";

            document.getElementById("<%=btnSearch.ClientID %>").disabled = false;
            document.getElementById("<%=btnSearch.ClientID %>").className = "form-button";
            document.getElementById("<%=btnSearch.ClientID %>").value = "Search";
            document.getElementById("<%=btnClear.ClientID %>").disabled = false;
            document.getElementById("<%=btnClear.ClientID %>").className = "form-button";
            document.getElementById("<%=btnClear.ClientID %>").value = "Clear";
            document.getElementById("imgLoading2").style.visibility = "invisible";
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }


       <%-- function AutoCompleteEnsureValues() {
            document.getElementById('<%#txtColour.ClientID %>').value = document.getElementById('<%#hdnColourName.ClientID %>').value;
        }--%>

    </script>

    <style type="text/css">
        .table-button {
            margin: 5px 10px 5px 5px;
        }

        .table-header-row th {
            padding: 6px 10px 6px 10px;
            color: white;
            font-weight: normal;
            height: 20px;
            font-size: 1.0em;
        }

        .table-row td {
            padding-right: 10px;
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
                <asp:Button ID="btnAdmin" runat="server" CssClass="form-button" Visible="false" Text="Admin" UseSubmitBehavior="false" />&nbsp;&nbsp;
            <asp:Button ID="btnDashBoard" runat="server" CssClass="form-button" Visible="false" Text="Dashboard" UseSubmitBehavior="false" />
                <asp:Button ID="btnHome" runat="server" CssClass="form-button" Visible="true" Text="Home" UseSubmitBehavior="false" />&nbsp;&nbsp;
        <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="return ConfirmLeave();" UseSubmitBehavior="false" />
            </div>

            <h1>OzRoll Production Schedule List</h1>


            <div style="height: 100%; text-align: center;">

                <table class="form-table" cellspacing="0" summary="">
                    <tr>
                        <td class="form-label-td" style="width: 50%; text-align: right;">Customer Name&nbsp;:&nbsp;
                <asp:DropDownList ID="cboCustomerName" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width="200px" Height="28px" />
                        </td>
                        <td class="form-label-td" style="width: 50%; text-align: left;">Order Reference&nbsp;:&nbsp;
                <asp:TextBox ID="txtOrderReference" runat="server" CssClass="form-field" Text="" MaxLength="20" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label-td" style="width: 50%; text-align: right;">Order Type&nbsp;:&nbsp;
                <asp:DropDownList ID="cboOrderType" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width="200px" Height="28px" />
                        </td>
                        <td class="form-label-td" style="width: 50%; text-align: left;">Shutter Pro Number&nbsp;:&nbsp;
                <asp:TextBox ID="txtShutterProNumber" runat="server" CssClass="form-field" Text="" MaxLength="20" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label-td" style="width: 50%; text-align: right;">Order Status&nbsp;:&nbsp;
                <asp:DropDownList ID="cboOrderStatus" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width="200px" Height="28px" />
                        </td>
                        <td class="form-label-td" style="width: 50%; text-align: left;">Priority&nbsp;:&nbsp;                
                <asp:DropDownList ID="cboPriority" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width="200px" Height="28px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label-td" style="width: 50%; text-align: right;">Show Active Jobs Only&nbsp;:&nbsp;
                <asp:CheckBox ID="chkActiveOnly" runat="server" Checked="True" />
                        </td>
                        <td class="form-label-td" style="width: 50%; text-align: left;">&nbsp;&nbsp;                                
                        </td>
                    </tr>
                    <asp:Panel ID="pnlDates" runat="server" Visible="false">
                        <tr>
                            <td class="form-label-td" style="width: 50%; text-align: right;">&nbsp;&nbsp;                    
                            </td>
                            <td class="form-label-td" style="width: 50%; text-align: left;">Order Date&nbsp;:&nbsp;
                    <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form-field" Text="" MaxLength="12" Width="120px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="form-label-td" style="width: 50%; text-align: right;">Invoice Date&nbsp;:&nbsp;
                    <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-field" Text="" MaxLength="12" Width="120px"></asp:TextBox>
                            </td>
                            <td class="form-label-td" style="width: 50%; text-align: left;">Shipping Date&nbsp;:&nbsp;
                    <asp:TextBox ID="txtActualShippingDate" runat="server" CssClass="form-field" Text="" MaxLength="12" Width="120px"></asp:TextBox>
                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>
                        <td class="form-label-td" style="width: 50%; text-align: right;">Date Type&nbsp;:&nbsp;
                <asp:DropDownList ID="cboDateType" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width="200px" Height="28px" />
                        </td>
                        <td class="form-label-td" style="width: 50%; text-align: left;">&nbsp;&nbsp;                
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label-td" style="width: 50%; text-align: right;">Start Date&nbsp;:&nbsp;
                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-field" Text="" MaxLength="12" Width="120px"></asp:TextBox>
                        </td>
                        <td class="form-label-td" style="width: 50%; text-align: left;">End Date&nbsp;:&nbsp;
                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-field" Text="" MaxLength="12" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-submit-td" style="text-align: right;">&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnSearch" runat="server" CssClass="form-button" Text="Search" OnClientClick="return ValidateAll();" UseSubmitBehavior="false" />
                            <asp:HiddenField runat="server" ID="hfCustomerId" Value="0" />
                            <asp:HiddenField runat="server" ID="hfAdminPermission" Value="0" />
                        </td>
                        <td class="form-submit-td" style="text-align: left;">
                            <asp:Button ID="btnClear" runat="server" CssClass="form-button" Text="Clear" UseSubmitBehavior="false" />
                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnAddJobExtarnalCustomer" runat="server" Visible="false" CssClass="form-button" Text="Add Job" OnClientClick="return addnew();" UseSubmitBehavior="false" />
                        </td>

                    </tr>
                    <tr>
                        <td class="form-submit-td" style="text-align: right;">
                            <asp:Button ID="btnReport" runat="server" CssClass="form-button" Text="Report" OnClientClick="javascript:reloadPage();openreport();" UseSubmitBehavior="false" />

                        </td>
                        <td class="form-submit-td" style="text-align: left;">

                            <asp:Button ID="btnAdd" runat="server" CssClass="form-button" Text="Add Job" OnClientClick="return addnew();" UseSubmitBehavior="false" />
                            &nbsp;&nbsp;&nbsp;&nbsp;<img id="imgLoading2" src="images/indicator.gif" width="16px" height="16px" alt="loading" style="visibility: hidden; vertical-align: middle;" />
                        </td>
                    </tr>

                </table>

            </div>
            <br />

            <asp:UpdatePanel ID="pnlResults" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>

                    <div style="height: 100%; text-align: center;">
                        <table class="form-table" cellspacing="0" summary="">
                            <tr>
                                <td class="form-submit-td" style="text-align: center;">
                                    <asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div class="form">
                        <div class="step">
                            <asp:Panel runat="server" ID="pnlList" Visible="false">
                                <asp:GridView ID="dgvScheduleList" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" AllowSorting="true">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="table-row" />
                                    <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#0b6134" ForeColor="White" CssClass="table-header-row" />
                                    <EditRowStyle BackColor="#999999" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="btnViewDetail" runat="server" Text="Details" CommandName="ScheduleDetail" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CssClass="form-button table-button" Width="90px" Height="35px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Id" Visible="false" />
                                        <asp:BoundField DataField="SiteId" Visible="false" />
                                        <asp:BoundField DataField="OrderTypeID" Visible="false" />
                                        <asp:BoundField DataField="CustomerID" SortExpression="CustomerID" Visible="false" />
                                        <asp:BoundField DataField="JobNumber" SortExpression="JobNumber" Visible="false" />
                                        <asp:BoundField DataField="EnteredDatetime" SortExpression="EnteredDatetime" HeaderText="Created Date" DataFormatString="{0:d MMM yyyy}" />
                                        <asp:BoundField DataField="OrderDate" SortExpression="OrderDate" HeaderText="Order Date" DataFormatString="{0:d MMM yyyy}" />
                                        <asp:BoundField DataField="ShutterProNumber" SortExpression="ShutterProNumber" HeaderText="Ozroll ID" />
                                        <asp:BoundField DataField="OrderReference" SortExpression="OrderReference" HeaderText="Order Reference" />
                                        <asp:BoundField DataField="CustomerName" SortExpression="CustomerName" HeaderText="Customer Ref" />
                                        <asp:BoundField DataField="Customer" SortExpression="Customer" HeaderText="Customer" />
                                        <asp:BoundField DataField="OrderStatusName" SortExpression="OrderStatus" HeaderText="Order Status" />
                                        <%--                            <asp:BoundField DataField="FullMonthName" SortExpression="FullMonthName" HeaderText="Invoice Month" />
                            <asp:BoundField DataField="WeekNumber" SortExpression="WeekNumber" HeaderText="Invoice Week" />--%>
                                        <asp:BoundField DataField="SalePrice" SortExpression="SalePrice" HeaderText="Sale Price" DataFormatString="{0:C}" />
                                        <asp:BoundField DataField="FreightAmount" SortExpression="FreightAmount" HeaderText="Freight Amt" DataFormatString="{0:C}" />
                                        <asp:BoundField DataField="TotalSQM" SortExpression="TotalSQM" HeaderText="Total SqM" />
                                        <asp:BoundField DataField="TotalPanels" SortExpression="TotalPanels" HeaderText="Total Panels" />

                                        <%--                            <asp:BoundField DataField="PanelsLess700" SortExpression="PanelsLess700" HeaderText="Panels < 700" />
                            <asp:BoundField DataField="PanelsMore700" SortExpression="PanelsMore700" HeaderText="Panels > 700" />
                            <asp:BoundField DataField="QtyHinges" SortExpression="QtyHinges" HeaderText="Hinges" />
                            <asp:BoundField DataField="QtySliding" SortExpression="QtySliding" HeaderText="Sliding" />
                            <asp:BoundField DataField="QtyBifold" SortExpression="QtyBifold" HeaderText="Bifold" />
                            <asp:BoundField DataField="QtyFixed" SortExpression="QtyFixed" HeaderText="Fixed" />

                            <asp:BoundField DataField="QtyZFrame" SortExpression="QtyZFrame" HeaderText="ZFrame" />
                            <asp:BoundField DataField="QtyLFrame" SortExpression="QtyLFrame" HeaderText="LFrame" />
                                        --%>
                                        <asp:BoundField DataField="OffWhiteYN" Visible="false" SortExpression="OffWhite" HeaderText="Off White" />
                                        <asp:BoundField DataField="BrightWhiteYN" Visible="false" SortExpression="BrightWhite" HeaderText="Bright White" />

                                        <%--                            <asp:BoundField DataField="PlannedShippingDate" SortExpression="PlannedShippingDate" HeaderText="Planned Shipping Date" />--%>
                                        <asp:BoundField DataField="ExpectedShippingDate" SortExpression="ExpectedShippingDate" HeaderText="Est Shipping Date" DataFormatString="{0:d MMM yyyy}" />
                                        <asp:BoundField DataField="ActualShippingDate" SortExpression="ActualShippingDate" HeaderText="Actual Shipping Date" DataFormatString="{0:d MMM yyyy}" />
                                        <asp:BoundField DataField="InvoicedDate" SortExpression="InvoicedDate" HeaderText="Invoiced Date" DataFormatString="{0:d MMM yyyy}" />
                                        <asp:BoundField DataField="ScheduledDate" SortExpression="ScheduledDate" HeaderText="Scheduled Date" DataFormatString="{0:d MMM yyyy}" />
                                        <asp:BoundField DataField="OrderType" SortExpression="OrderType" HeaderText="Order Type" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>

                        </div>
                    </div>

                </ContentTemplate>

                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>

            </asp:UpdatePanel>

            <br />
            <div runat="server" id="DivCur1" onclick="javascript:showMe();" style="width: 10px; height: 10px;"></div>
            <div runat="server" id="DivCur2" style="display: none;">
                txtCurState:<asp:TextBox ID="txtCurState" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
                <br />
                SiteID:<asp:TextBox ID="txtSiteID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
                <br />
                ProductTypeID<asp:TextBox ID="txtProductTypeID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
                <br />


                <div id="dialog-alert" title="Insufficient Information">
                    <p id="dialog-alert-message" style="text-align: left;"></p>
                </div>
                <div id="dialog-confirm" title="Please Confirm">
                    <p id="dialog-confirm-message" style="text-align: left;"></p>
                </div>

            </div>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnlNotesHide" runat="server" Style="display: none;">
                            <div>
                                <asp:Button ID="btnViewNotes" runat="server" Visible="true" />
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>


            <ajaxcontroltoolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnViewNotes" PopupControlID="pnlNotesDetails" BackgroundCssClass="modalBackground">
            </ajaxcontroltoolkit:ModalPopupExtender>
          
            <asp:Panel ID="pnlNotesDetails" runat="server" CssClass="modalPopup" Style="display: none; width: 550px !important; left: 400px !important">
                 <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>
                        <asp:Panel ID="pnlAddLouvers" runat="server">
                        <asp:TextBox ID="txtHiddenPSDetailID" runat="server" Visible="false" Text="0"></asp:TextBox>

                        <div class="div-column">
                            <table id="column1" class="form-table" cellspacing="0" summary="">
                                <asp:Panel ID="pnlRemakeType" runat="server" Visible="false">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Remake Type
                                        </td>
                                        <td class="form-field-td-p2 no-wrap">
                                            <asp:DropDownList ID="cboRemakeType" runat="server" CssClass="form-select" Width="150px">
                                                <asp:ListItem Value="0" Text="" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Whole Opening"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Single Panel"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <tr>
                                    <td class="form-label-td-p2 no-wrap" style="text-align: right;">
                                        <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>
                                    </td>

                                    <td class="form-field-td-p2 no-wrap" >
                                        <asp:TextBox ID="txtLocation" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="100" usesubmitbehavior="false" Width="200px" />
                                        <asp:RequiredFieldValidator ID="valrfLocation" runat="server" ControlToValidate="txtLocation" CssClass="validation-text"
                                            ErrorMessage="<br />Please enter a Location." ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="form-label-td-p2 no-wrap" style="text-align: right;">Colour</td>
                                    <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:TextBox ID="txtColour" runat="server" CssClass="form-field" Width="200px" 
                                                        onfocus="this.select();" onblur="AutoCompleteEnsureValues()"  />
                                        <asp:Image ID="imgColour" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />

                                        <ajaxcontroltoolkit:AutoCompleteExtender
                                                        ID="aceColour"
                                                        runat="server"
                                                        TargetControlID="txtColour"
                                                        ServicePath="AutoComplete.asmx"
                                                        ServiceMethod="GetMatchingColourList"
                                                        MinimumPrefixLength="1"
                                                        CompletionInterval="1000"
                                                        CompletionSetCount="0"
                                                        EnableCaching="false"
                                                        CompletionListCssClass="auto-complete-list"
                                                        CompletionListHighlightedItemCssClass="auto-complete-higlighted-item"
                                                        CompletionListItemCssClass="auto-complete-item"
                                                        OnClientItemSelected="AutoCompleteGetColourID"
                                                        OnClientPopulating="AutoCompleteStart"
                                                        OnClientPopulated="AutoCompleteEnd"
                                                        OnClientHiding="AutoCompleteEnd"
                                                        FirstRowSelected="true"></ajaxcontroltoolkit:AutoCompleteExtender>

                                                    <ext:HiddenFieldExtended ID="hdnColourID" runat="server" />
                                                    <ext:HiddenFieldExtended ID="hdnColourName" runat="server" />

                                        <asp:RequiredFieldValidator ID="valrfColour" runat="server" ControlToValidate="hdnColourID" CssClass="validation-text"
                                            ErrorMessage="<br />Please select a Colour." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                              
                                    </td>

                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Product</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                            
                                            <asp:DropDownList ID="cboLouvreProd" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="DLi" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="CL" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgLouvreProd" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="rfvalLouvreProd" runat="server" ControlToValidate="cboLouvreProd" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Product." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                    
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Louvre Type</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:DropDownList ID="cboLouvreType" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                    
                                        </asp:DropDownList>
                                        <asp:Image ID="imgLouvreType" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        <asp:RequiredFieldValidator ID="rfvalLouvreType" runat="server" ControlToValidate="cboLouvreType" CssClass="validation-text"
                                            ErrorMessage="<br />Please select Louvre Type." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                    
                                    </td>

                                    <asp:Panel ID="pnlBiFoldHingedDoor" runat="server">
                                        <tr>
                                            <td class="form-label-td-p2 no-wrap" style="text-align: right;">Open In / Out</td>
                                            <td class="form-field-td-p2 no-wrap" >
                                                
                                                <asp:DropDownList ID="cboBiFoldHingedDoor" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                    <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="In" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Out" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgBiFoldHingedDoor" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                                <asp:RequiredFieldValidator ID="valrfBiFoldHingedDoor" runat="server" ControlToValidate="cboBiFoldHingedDoor" CssClass="validation-text"
                                                    ErrorMessage="<br />Please select Open In / Out." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />   
                                            </td>
                                        </tr>
                                    </asp:Panel>

                                    </tr>
                                        

                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Height</td>
                                        <td class="form-field-td-p2" >
                                                
                                            <asp:TextBox ID="txtHeight" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="4" usesubmitbehavior="false" Width="95px" /> (mm)
                                            <asp:Image ID="imgHeight" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfHeight" runat="server" ControlToValidate="txtHeight" CssClass="validation-text"
                                                ErrorMessage="<br />Height is required." ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            <asp:CompareValidator ID="valcompHeight" runat="server" Operator="DataTypeCheck" Type="Integer" CssClass="validation-text"
                                                ControlToValidate="txtHeight" ErrorMessage="<br />Height is invalid." ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            <asp:RangeValidator ID="valrangeHeight" runat="server" Type="Double" CssClass="validation-text"
                                                MinimumValue="1" MaximumValue="9999" ControlToValidate="txtHeight" ValidationGroup="details"
                                                ErrorMessage="<br />Please enter a valid height." Display="Dynamic" EnableClientScript="false" />
                                            <asp:CustomValidator ID="valcustPanelHeight" runat="server"
                                                    Display="Dynamic" Text="" 
                                                    ControlToValidate="txtHeight"
                                                    ValidationGroup="details"
                                                    EnableClientScript="false"
                                                    CssClass="validation-text" />   
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Width</td>
                                        <td class="form-field-td-p2" >
                                                
                                            <asp:TextBox ID="txtWidth" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="6" usesubmitbehavior="false" Width="95px"  /> (mm)
                                            <asp:Image ID="imgWidth" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valreqWidth" runat="server" ControlToValidate="txtWidth" CssClass="validation-text"
                                                ErrorMessage="<br />Width is required." ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            <asp:CompareValidator ID="valcompWidth" runat="server" Operator="DataTypeCheck" Type="Integer" CssClass="validation-text"
                                                ControlToValidate="txtWidth" ErrorMessage="<br />Width is invalid." ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            <asp:RangeValidator ID="valrangeWidth" runat="server" Type="Double" CssClass="validation-text"
                                                MinimumValue="1" MaximumValue="999999" ControlToValidate="txtWidth" ValidationGroup="details"
                                                ErrorMessage="<br />Please enter a valid width." Display="Dynamic" EnableClientScript="false" />
                                            <asp:CustomValidator ID="valcustPanelWidth" runat="server" 
                                                Display="Dynamic" Text="" 
                                                ValidationGroup="details"
                                                ControlToValidate="txtWidth"
                                                EnableClientScript="false"
                                                CssClass="validation-text" />
                                              
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">No of Panels</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:DropDownList ID="cboNoOfPanels" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                            <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Image ID="imgNoOfPanels" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            
                                        </td>
                                    </tr>

                                    

                                     
                                    <asp:Panel ID="pnlPanelBottomRail" runat="server">
                                        <tr>
                                            <td class="form-label-td-p2 no-wrap" style="text-align: right;">Panel Bottom Rail</td>
                                            <td class="form-field-td-p2 no-wrap" >
                                                
                                                <asp:DropDownList ID="cboPanelBottomRail" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                    <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgPanelBottomRail" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                                <asp:RequiredFieldValidator ID="valrfPanelBottomRail" runat="server" ControlToValidate="cboPanelBottomRail" CssClass="validation-text"
                                                    ErrorMessage="<br />Please select Panel Bottom Rail." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            
                                            </td>
                                        </tr>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlPanelMidRail" runat="server">
                                        <tr>
                                            <td class="form-label-td-p2 no-wrap" style="text-align: right;">Panel Mid Rail</td>
                                            <td class="form-field-td-p2 no-wrap" >
                                                
                                                <asp:DropDownList ID="cboPanelMidRail" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                    <asp:ListItem Selected="true" Text="None" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgPanelMidRail" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            
                                            </td>
                                        </tr>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlMidRailHeight" runat="server">
                                        <tr>
                                            <td class="form-label-td-p2 no-wrap" style="text-align: right;">
                                                <asp:Label ID="MidRailLabel" runat="server" Text="Mid Rail Height" Visible="true" />
                                            </td>
                                            <td class="form-field-td-p2 no-wrap" >
                                                
                                                <asp:TextBox ID="txtMidRailHeight" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="4" usesubmitbehavior="false" Width="100px" />
                                                    (mm)
                                                <asp:Image ID="imgMidRailHeight" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />

                                                <asp:RequiredFieldValidator ID="valrfMidRailHeight" runat="server" ControlToValidate="txtMidRailHeight" CssClass="validation-text"
                                                    ErrorMessage="<br />Please enter Mid Rail Height." ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                                <asp:CustomValidator ID="valcustMidRailHeight" runat="server" CssClass="validation-text" ControlToValidate="txtMidRailHeight"
                                                    ErrorMessage="" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" ValidateEmptyText="true" />
                                            </td>
                                        <tr />
                                    </asp:Panel>

                            </table>
                        </div>

                        <div class="div-column">
                            <table id="column2" class="form-table" cellspacing="0" summary="">

                                <asp:Panel ID="pnlEndCapColour" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">DLi End Plug Colour</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:DropDownList ID="cboEndCapColour" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                            <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Black" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="White" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Image ID="imgEndCapColour" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfEndCapColour" runat="server" ControlToValidate="cboEndCapColour" CssClass="validation-text"
                                                ErrorMessage="<br />Please select End Plug Colour." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <%--added by surendra ticket #63195--%>
                                <asp:Panel ID="pnlBladeClipColour" runat="server">
                                <tr>
                                    <td class="form-label-td-p2 no-wrap" style="text-align: right;">Blade Clip Colour</td>
                                    <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:DropDownList ID="cboBladeClipColour" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                            <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Black" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="White" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Image ID="imgBladeClipColour" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        <asp:RequiredFieldValidator ID="varfBladeClipColour" runat="server" ControlToValidate="cboBladeClipColour" CssClass="validation-text"
                                            ErrorMessage="<br />Please select Blade Clip Colour." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />                                           
                                    </td>
                                </tr>
                                </asp:Panel>
                                <%--added by surendra ticket #63195--%>
                                <asp:Panel ID="pnlPileColour" runat="server">
                                <tr>  
                                    <td class="form-label-td-p2 no-wrap" style="text-align: right;">Pile Colour</td>
                                    <td class="form-field-td-p2 no-wrap" >
                                                
                                    <asp:DropDownList ID="cboPileColour" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                        <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Black" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Grey" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Image ID="imgPileColour" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                    <asp:RequiredFieldValidator ID="valrfPileColour" runat="server" ControlToValidate="cboPileColour" CssClass="validation-text"
                                        ErrorMessage="<br />Please select Pile Colour." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            
                                    </td>
                                </tr>
                                 </asp:Panel>
                                <asp:Panel ID="pnlTopTrackType" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Top Track Type</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:DropDownList ID="cboTopTrackType" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                            <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Image ID="imgTopTrackType" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        <asp:RequiredFieldValidator ID="valrfTopTrackType" runat="server" ControlToValidate="cboTopTrackType" CssClass="validation-text"
                                            ErrorMessage="<br />Please select Top Track Type." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                              
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlBottomTrackType" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Bottom Track Type</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboBottomTrackType" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="12mm Dli" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="25mm CL" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgBottomTrackType" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfBottomTrackType" runat="server" ControlToValidate="cboBottomTrackType" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Bottom Track Type." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                              
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlCurvedTrack" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Curved Track</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboCurvedTrack" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgCurvedTrack" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />                                            
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlCurvedTrackMaxDeflection" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Max Deflection</td>
                                        <td class="form-field-td-p2 no-wrap" >

                                            <asp:TextBox ID="txtCurvedTrackMaxDeflection" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="6"
                                                usesubmitbehavior="false" Width="100px" /> (mm)
                                            <asp:Image ID="imgCurvedTrackMaxDeflection" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:CustomValidator ID="valcustCurvedTrackMaxDeflection" runat="server" CssClass="validation-text" ControlToValidate="txtCurvedTrackMaxDeflection"
                                                    ErrorMessage="" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" ValidateEmptyText="true" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlExtraTrack" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Extra Track</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:TextBox ID="txtExtraTrack" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="6" 
                                                usesubmitbehavior="false" Width="100px" /> (mm)

                                            <asp:Image ID="imgExtraTrack" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlFlushBoltsTop" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Flush Bolts Top</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboFlushBoltsTop" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="None" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Short 235mm" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Short 235mm with extended tip" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="Long 430mm" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Long 430mm with extended tip" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Ex Long 890mm" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Ex Long 890mm with extended tip" Value="7"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgFlushBoltsTop" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlFlushBoltsBottom" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Flush Bolts Bottom</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboFlushBoltsBottom" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="None" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Short 235mm" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Short 235mm with extended tip" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="Long 430mm" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Long 430mm with extended tip" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Ex Long 890mm" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Ex Long 890mm with extended tip" Value="7"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgFlushBoltsBottom" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlFlushBoltsPosition" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Flush Bolts L/R</td>
                                        <td class="form-field-td-p2 no-wrap" >

                                        <asp:DropDownList ID="cboFlushBoltsPosition" runat="server" AutoPostBack="true" CssClass="form-select" Width="50px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="L" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="R" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgFlushBoltsPosition" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        <asp:RequiredFieldValidator ID="valrfFlushBoltsPosition" runat="server" ControlToValidate="cboFlushBoltsPosition" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Flush Bolts Position." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" /> 
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlLockOptions" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Special Lock Options</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboLockOptions" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="None" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Handle Lock – Inside &amp; Out (Tasman)" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Premium Handle Lock – Inside &amp; Out (Finista/Killara)" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Knobset Lock – Inside &amp; Out" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Twin Flush Bolt – Locking" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Twin Flush Bolt – Non Locking" Value="5"></asp:ListItem>

                                            </asp:DropDownList>
                                            <asp:Image ID="imgLockOptions" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlBladeLocks" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Blade Locks</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboBladeLocks" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgBladeLocks" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfBladeLocks" runat="server" ControlToValidate="cboBladeLocks" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Blade Locks." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlCChannel" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">T&B Fixing Channel</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboCChannel" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgCChannel" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfCChannel" runat="server" ControlToValidate="cboCChannel" CssClass="validation-text"
                                                ErrorMessage="<br />Please select T&B Fixing Channel." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlFixedPanelSides" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Side Fixing</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboFixedPanelSides" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="None" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Angle Sides" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="25mm Channel Sides" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="50mm Sides" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgFixedPanelSides" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />        
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlHinges" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Hinges</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                               
                                            <asp:DropDownList ID="cboHinges" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px" Visible="true" >
                                                <asp:ListItem Selected="true" Text="None" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgHinges" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" Visible="false" />
                                                    
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlWinder" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Winder</td>
                                        <td class="form-field-td-p2 no-wrap" >

                                            <asp:DropDownList ID="cboWinder" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px" Visible="false" >
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Left" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgWinder" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" Visible="false" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                            </table>
                        </div>

                        <div class="div-column">
                            <table id="column3" class="form-table" cellspacing="0" summary="">

                                <asp:Panel ID="pnlHChannel" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">H Joiner</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboHChannel" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgHChannel" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfHChannel" runat="server" ControlToValidate="cboHChannel" CssClass="validation-text"
                                                ErrorMessage="<br />Please select H Joiner." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlLReveal" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">L Reveal</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboLReveal" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Reveal 3 Sided" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Reveal 4 Sided" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Facefit 3 Sided" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Facefit 4 Sided" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="No Frame" Value="5"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgLReveal" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfLReveal" runat="server" ControlToValidate="cboLReveal" CssClass="validation-text"
                                                ErrorMessage="<br />Please select L Reveal." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                              
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlZReveal" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Z Reveal</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboZReveal" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Reveal 3 Sided" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Reveal 4 Sided" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgZReveal" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfZReveal" runat="server" ControlToValidate="cboZReveal" CssClass="validation-text"
                                                ErrorMessage="<br />Please select L Reveal." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                              
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlInsertTop" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">
                                                <asp:Label ID="lblMainInsert" runat="server" Text="Main Insert" Visible="true" />
                                        </td>
                                        <td class="form-field-td-p2 no-wrap" >
                                            <asp:DropDownList ID="cboInsertTop" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Horizontal Blade" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Vertical Blade" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Clear Glass" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Grey Glass" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgInsertTop" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfInsertTop" runat="server" ControlToValidate="cboInsertTop" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Insert." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlBladeOperation" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">
                                            <asp:Label ID="lblOperationMain" runat="server" Text="Main Operation" Visible="true" />
                                        </td>
                                        <td class="form-field-td-p2 no-wrap" >
                                            
                                            <asp:DropDownList ID="cboBladeOperation" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Standard" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Fixed Open" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Fixed Closed " Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgBladeOperation" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfBladeOperation" runat="server" ControlToValidate="cboBladeOperation" CssClass="validation-text"
                                                    ErrorMessage="<br />Please select Blade Operation." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlInsertBottom" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Bottom Insert</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboInsertBottom" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Horizontal Blade" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Vertical Blade" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Clear Glass" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Grey Glass" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgInsertBottom" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfInsertBottom" runat="server" ControlToValidate="cboInsertBottom" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Bottom Insert." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                                    
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlBladeOperationBottom" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Bottom Operation</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                            <asp:DropDownList ID="cboBladeOperationBottom" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Standard" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Fixed Open" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Fixed Closed " Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgBladeOperationBottom" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfBladeOperationBottom" runat="server" ControlToValidate="cboBladeOperationBottom" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Bottom Operation." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                                
                                        </td>    
                                    </tr>
                                </asp:Panel>
                                <%--Added by surendra ticket#63195--%>
                                <asp:Panel ID="pnlFlyScreen" runat="server">
                                <tr>
                                    <td class="form-label-td-p2 no-wrap" style="text-align: right;">Fly Screen</td>
                                    <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:DropDownList ID="cboFlyScreen" runat="server" CssClass="form-select" Width="150px">
                                            <asp:ListItem Selected="true" Text="No" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Image ID="imgFlyScreen" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                                    
                                    </td>
                                </tr>
                                </asp:Panel>
                                <asp:Panel ID="pnlStacker" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Stacker Bay Location</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboStacker" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Left" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgStacker" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfStacker" runat="server" ControlToValidate="cboStacker" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Stacker Location." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                                    
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlSlide" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Slide Track Spacing</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboSlide" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Open Blade Track Spacing" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Shut Blade Track Spacing" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgSlide" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfSlide" runat="server" ControlToValidate="cboSlide" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Slide Track Spacing." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                                    
                                        </td>
                                    </tr>
                                </asp:Panel>

                            </table>
                        </div>

                         
                        </asp:Panel>
                         

                        <div class="hider">
                            <div class="hider-loading">
                                <img src="Images/loading-gif-transparent-green.gif" alt="Loading..." width="100px" height="100px" />
                            </div>
                        </div>
                                    
                    </ContentTemplate>

                    <Triggers>
                        <%--<asp:PostBackTrigger ControlID="btnSaveDetails" />
                        <asp:PostBackTrigger ControlID="btnCancelDetails" />--%>
                    </Triggers>

                </asp:UpdatePanel>
            </asp:Panel>

            <asp:HiddenField ID="hiddenPowderCoatPopupDummy" runat="server" />
           <script type="text/javascript">
                $(function () {
                    $("input#txtActualShippingDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+5w' });
                    $("input#txtOrderDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+1d' });
                    $("input#txtInvoiceDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+1d' });
                    $("input#txtStartDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+10w' });
                    $("input#txtEndDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+10w' });
                });
            </script>

        </div>

    </form>
</body>

</html>
