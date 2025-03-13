<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LouvreJobSearch.aspx.vb" Inherits="LouvreJobSearch" Theme="SknGridView" %>

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
		
            $("[id$=txtDateRequired]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+1d' });
		    $("[id$=txtStartDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+10w' });
		    $("[id$=txtEndDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+10w' });
		
        });

	    if (window.history.forward(1) != null)
	        window.history.forward(1);

	    function ValidateAll() {
	        var bolValid = 1;
	        var strMessage = "";
	        document.getElementById("<%=lblStatus.ClientID %>").innerHTML = "";
	        if (bolValid == 1) {
	            document.getElementById("<%=btnSearch.ClientID %>").disabled = true;
	            document.getElementById("<%=btnSearch.ClientID %>").className = "form-button-disabled";
	            document.getElementById("<%=btnSearch.ClientID %>").value = "Submitting";
	            document.getElementById("<%=btnClear.ClientID %>").disabled = true;
	            document.getElementById("<%=btnClear.ClientID %>").className = "form-button-disabled";
	            document.getElementById("<%=btnClear.ClientID %>").value = "Clear";

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
        <asp:Button ID="btnHome" runat="server" CssClass="form-button" Visible="true" Text="Home" UseSubmitBehavior="false" />&nbsp;&nbsp;
        <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="return ConfirmLeave();" UseSubmitBehavior="false" />
	    </div>
    
    <h1>OzRoll Production Schedule List</h1>
    
    <div style="height: 100%; text-align: center;">
   
     <table class="form-table" cellspacing="0" summary="">
         <tr>
            <td class="form-label-td" style="width: 50%; text-align: right;">ContractNo&nbsp;:&nbsp;
                <asp:TextBox ID="txtContractNo" runat="server" CssClass="form-field" Text="" MaxLength="20" Width="120px"></asp:TextBox>
            </td>
            <td class="form-label-td" style="width: 50%; text-align: left;">&nbsp;&nbsp;                
            </td>
         </tr>
         <tr>
            <td class="form-label-td" style="width: 50%; text-align: right;">Type&nbsp;:&nbsp;
                <asp:DropDownList ID="cboType" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width ="200px" Height="28px" />
            </td>
            <td class="form-label-td" style="width: 50%; text-align: left;">&nbsp;&nbsp;                
            </td>
         </tr>
         <tr>
            <td class="form-label-td" style="width: 50%; text-align: right;">Status&nbsp;:&nbsp;
                <asp:DropDownList ID="cboStatus" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width ="200px" Height="28px" />
            </td>
            <td class="form-label-td" style="width: 50%; text-align: left;">&nbsp;&nbsp;                
            </td>
         </tr>
         <tr>
            <asp:Panel runat="server" ID="pnlHideActiveOnly" Visible="false">
                <td class="form-label-td" style="width: 50%; text-align: right;">Show Active Jobs Only&nbsp;:&nbsp;
                    <asp:CheckBox ID="chkActiveOnly" runat="server" />
                </td>
                <td class="form-label-td" style="width: 50%; text-align: left;">&nbsp;&nbsp;                                
                </td>
            </asp:Panel>
         </tr>
        <tr>
            <td class="form-label-td" style="width: 50%; text-align: right;">Date Type&nbsp;:&nbsp;
                <asp:DropDownList ID="cboDateType" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width ="200px" Height="28px" />

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
            </td>
            <td class="form-submit-td" style="text-align: left;">&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnClear" runat="server" CssClass="form-button" Text="Clear" UseSubmitBehavior="false" />
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
                    <asp:GridView ID="dgvScheduleList" runat="server" DataKeyNames="JobRegisterId" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333"  >
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
                                    <asp:Button ID="btnViewDetail" runat="server" Text="Details" CommandName="LouvreDetail"  CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CssClass="form-button" Width="100px" Height="28px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="JobRegisterId" Visible="false" /> 
                            <asp:BoundField DataField="Type" Visible="false" /> 
                            <asp:BoundField DataField="DateSold" SortExpression="DateSold" HeaderText="Date Sold" DataFormatString="{0:d MMM yyyy}" /> 
                            <asp:BoundField DataField="StatusName" SortExpression="Status" HeaderText="Status" />   	
                            <asp:BoundField DataField="ContractNo" SortExpression="ContractNo" HeaderText="Contract Number" />
                            <asp:BoundField DataField="ProjectName" SortExpression="ProjectName" HeaderText="Project Name" />
                            <asp:BoundField DataField="DateRequired" SortExpression="DateRequired" HeaderText="Date Required" DataFormatString="{0:d MMM yyyy}" />
                            <asp:BoundField DataField="ScheduledDate" SortExpression="ScheduledDate" HeaderText="Scheduled" DataFormatString="{0:d MMM yyyy}" />

                            <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code" />
                            <asp:BoundField DataField="Type" SortExpression="Type" Visible="false" />
                            <asp:BoundField DataField="Style" SortExpression="Style" Visible="false" />
                            <asp:BoundField DataField="Colour" SortExpression="Colour" HeaderText="Colour" />
                            <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="Panels" />
                            <asp:BoundField DataField="SQM" SortExpression="SQM" HeaderText="SQM" />
                            <asp:BoundField DataField="InvoicedDate" SortExpression="InvoicedDate" HeaderText="Invoice Date" DataFormatString="{0:d MMM yyyy}" />
                            <asp:BoundField DataField="OrderType" SortExpression="OrderTypeID" HeaderText="Order Type" />
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
        <div runat="server" id="DivCur2" style="display:none;">
        txtCurState:<asp:TextBox ID="txtCurState" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
        SiteID:<asp:TextBox ID="txtSiteID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
      

    <div id="dialog-alert" title="Insufficient Information">
        <p id="dialog-alert-message" style="text-align: left;"></p>
    </div>
    <div id="dialog-confirm" title="Please Confirm" >
        <p id="dialog-confirm-message" style="text-align: left;"></p>
    </div>

    </div>   
        
    <script type="text/javascript">
    $(function () {
        $("input#txtDateRequired").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+1d' });
        $("input#txtStartDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+10w' });
        $("input#txtEndDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', maxDate: '+10w' });  
    });
    </script>

    </div>  
    
   </form>
</body>

</html>
