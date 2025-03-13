<%@ Page Language="VB" AutoEventWireup="false" CodeFile="readExcel.aspx.vb" Inherits="readExcel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">

<head>
    <title>Read Excel File</title>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta name="format-detection" content="telephone=no" />
    
    <link href="stylesheets/reset.css" rel="stylesheet" type="text/css" media="screen" />
    <!--[if IE]><link href="stylesheets/explorer.css" rel="stylesheet" type="text/css" media="screen" /><![endif]-->
    <link href="stylesheets/style.css?version=<%= Now.Year() & Now.Month & Now.Day() %>" rel="stylesheet" type="text/css" media="screen" />
    <link href="stylesheets/smoothness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />
    <link href="stylesheets/jquery.lightbox-0.5.css" rel="stylesheet" type="text/css" media="screen" />
    
    <!--[if lt IE 8]><script src="javascript/IE-fix.js" type="text/javascript"></script><![endif]-->
    <script src="javascript/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="javascript/jquery.lightbox-0.5.js" type="text/javascript"></script>
    <script src="javascript/mobile-device.js" type="text/javascript"></script>
    
    <script src="javascript/binaryajax.js" type="text/javascript"></script>
    <script src="javascript/exif.js" type="text/javascript"></script>
    
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

        function saveLocation() {
            localStorage.setItem('sTopLevelPage', window.location);
        }

        function ClearBrowseContent(control) {
            var browse = document.getElementById(control);
            var newbrowse = browse.cloneNode(false);
            browse.parentNode.replaceChild(newbrowse, browse);
        }

        </script>
</head>
<body onload="saveLocation();">


<form runat="server" id="form1" method="post" defaultbutton="btnHome">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        <Scripts><asp:ScriptReference Path="javascript/fixfocus.js" /></Scripts>
    </asp:ScriptManager>

<asp:Literal ID="lblWarning" runat="server"></asp:Literal>

<div id="middle-container-diallerview" >

    <div id="logo" style="height: 100px; text-align: center;" ></div>
    <div style="height: 50px; text-align: center;">
        <asp:Button ID="btnHome" runat="server" CssClass="form-button" Text="Home" UseSubmitBehavior="false" />
        <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="javascript:return ConfirmLeave();" UseSubmitBehavior="false" />
	</div>
    
    <h1>Import Wholesale Louvre Order Form</h1>

    <div class="form">
        <br/>
        <div style="height: 100%; text-align: center; vertical-align:middle;">
            <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name" style="vertical-align:middle;" />&nbsp;:&nbsp;
            <asp:DropDownList ID="cboCustomerName" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width="200px" Height="28px" style="vertical-align:middle;"  />
        </div>
        <br />
        <br />
        <div style="height: 100%; text-align: center; vertical-align:middle;">
        <asp:FileUpload ID="FileUpload1" runat="server" Text=""/> 
        <asp:Button ID="btnUpload" runat="server" Text="Upload" AutoPostBack="True" />
        
        <asp:Panel ID="pnlShowData" runat="server" Visible="true">
            <asp:UpdatePanel ID="pnlDetails" runat="server" UpdateMode="Conditional">
                <ContentTemplate>   
                    <div style="width: 90%; text-align: center;"><asp:Label ID="lblChecklist" runat="server" Text=""></asp:Label></div>
                    <div style="text-align: center;">
                        <asp:GridView ID="dgvLouvreDet" runat="server" DataKeyNames="AutoId" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" >
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="40px" />
                            <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />                        
                        <Columns>
                        <asp:BoundField DataField="AutoId" HeaderText="Item" />
                        <asp:BoundField DataField="ShutterId" HeaderText="#" />
                        <asp:BoundField DataField="Location" HeaderText="Location" />
                        <asp:BoundField DataField="OtherLocation" HeaderText="Other Location" />
                        <asp:BoundField DataField="Width" HeaderText="Width" />
                        <asp:BoundField DataField="Height" HeaderText="Height" />
                        <asp:BoundField DataField="MakeOrOpenSizes" HeaderText="Make/Open Sizes" />
                        <asp:BoundField DataField="ProductId" HeaderText="ProductId" />
                        <asp:BoundField DataField="ShutterType" HeaderText="ShutterType" />
                        <asp:BoundField DataField="BiFoldHingedDoorInOut" HeaderText="BiFold Hinged Door In/Out" />
                        </Columns>
                        </asp:GridView>
                    </div>
                    <hr />
               </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        </div>
        <div style="width: 90%;"><asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red"></asp:Label></div>
    </div>

    <div runat="server" id="DivCur1" style="display:none;">
        ProductTypeID<asp:TextBox id="txtProductTypeID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
    <br />
        <div id="dialog-confirm" title="Please Confirm">
            <p id="dialog-confirm-message" style="text-align: left;"></p>
        </div>   
        <div id="dialog-alert" title="Error Occurred">
            <p id="dialog-alert-message" style="text-align: left;"></p>
        </div>
    </div>

</div>

</form>

</body>
</html>