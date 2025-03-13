<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LouvreProductionReport.aspx.vb" Inherits="LouvreProductionReport" Theme="SknGridView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>Louvre Jobs List</title>
        
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
		
        });

	    if (window.history.forward(1) != null)
	        window.history.forward(1);

	    function openreport() {
	        document.getElementById("<%=btnReport.ClientID%>").disabled = true;
	        document.getElementById("<%=btnReport.ClientID%>").className = "form-button-disabled";
	        document.getElementById("<%=btnReport.ClientID%>").value = "Submitting";
	        document.getElementById("imgLoading2").style.visibility = "visible";
	        __doPostBack('<%=btnReport.ClientID%>', '');
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
    
    <h1>Louvre Production Schedule</h1>
    
    <div style="height: 100%; text-align: center;">
   
     <table class="form-table" cellspacing="0" summary="">
        <tr>
            <td class="form-submit-td"  style="text-align: center;">
                <asp:Button ID="btnReport" runat="server" CssClass="form-button" Text="Report" OnClientClick="javascript:reloadPage();openreport();" UseSubmitBehavior="false" />
                    &nbsp;&nbsp;&nbsp;&nbsp;<img id="imgLoading2" src="images/indicator.gif" width="16px" height="16px" alt="loading" style="visibility: hidden; vertical-align: middle;" />
            </td>
        </tr>  
     </table>
     
   </div>
    <br />
   
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
                <asp:Panel runat="server" ID="pnlList" Visible="true">
                    <asp:GridView ID="dgvScheduleList" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333"  >
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Large" />
                        <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" Height="20px" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField >
                                <ItemTemplate>                                    
                                    <asp:Button ID="btnViewDetail" runat="server" Text="Details" CommandName="LouvreDetail"  CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CssClass="form-button" Width="90px" Height="28px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ID" Visible="false" /> 
                            <asp:TemplateField HeaderText="Job" >
                                <ItemTemplate>                                    
                                    <asp:Label runat="server" ID="lblJobDetails" Text='<%#(Eval("JobDisplay")) %>' Width="300px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:BoundField DataField="StyleName" SortExpression="StyleName" HeaderText="Style<br />Name" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PanelWk1" SortExpression="PanelWk1" HeaderText="Wk1" DataFormatString="{0:##0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PanelWk2" SortExpression="PanelWk2" HeaderText="Wk2" DataFormatString="{0:##0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PanelWk3" SortExpression="PanelWk3" HeaderText="Wk3" DataFormatString="{0:##0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  />
                            <asp:BoundField DataField="PanelWk4" SortExpression="PanelWk4" HeaderText="Wk4" DataFormatString="{0:##0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PanelWk5" SortExpression="PanelWk5" HeaderText="Wk5" DataFormatString="{0:##0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PanelWk6" SortExpression="PanelWk6" HeaderText="Wk6" DataFormatString="{0:##0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PanelWk7" SortExpression="PanelWk7" HeaderText="Wk7" DataFormatString="{0:##0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="SQMWk1" SortExpression="SQMWk1" HeaderText="Wk1" DataFormatString="{0:##0.0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="SQMWk2" SortExpression="SQMWk2" HeaderText="Wk2" DataFormatString="{0:##0.0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="SQMWk3" SortExpression="SQMWk3" HeaderText="Wk3" DataFormatString="{0:##0.0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="SQMWk4" SortExpression="SQMWk4" HeaderText="Wk4" DataFormatString="{0:##0.0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="SQMWk5" SortExpression="SQMWk5" HeaderText="Wk5" DataFormatString="{0:##0.0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="SQMWk6" SortExpression="SQMWk6" HeaderText="Wk6" DataFormatString="{0:##0.0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="SQMWk7" SortExpression="SQMWk7" HeaderText="Wk7" DataFormatString="{0:##0.0}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Notes" >
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblNote" Text='<%#(Eval("ProdNotes")) %>' Width="300px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ScheduledDate" SortExpression="ScheduledDate" HeaderText="Scheduled Date" DataFormatString="{0:d MMM yyyy}" />
                            <asp:BoundField DataField="DateRequired" SortExpression="DateRequired" HeaderText="Date Required" DataFormatString="{0:d MMM yyyy}" />
                            
                            <asp:BoundField DataField="WeekId" SortExpression="WeekId" HeaderText="Week Id"  Visible="false" />    
                        </Columns>
                    </asp:GridView>
                </asp:Panel>

	        </div>
     </div>   

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

    </div>  
    
   </form>
</body>

</html>
