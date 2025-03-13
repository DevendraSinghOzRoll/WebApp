<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ConfirmCancelJob.aspx.vb" Inherits="ConfirmCancelJob" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>OzRoll Production Schedule Details</title>
        
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


            if (window.history.forward(1) != null)
                window.history.forward(1);

            function ConfirmLeave() {
                document.getElementById("dialog-confirm-message").innerHTML = "Are you sure you want to logout?";
                $("#dialog-confirm").dialog("open");
                return false;
            }

    </script>

</head>
<body>
    <form id="form1" runat="server">


    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    <Scripts>
    <asp:ScriptReference Path="javascript/fixfocus.js" />
    </Scripts>
    </asp:ScriptManager>
    <div id="middle-container-diallerview" >

        <div id="logo" style="height: 100px; text-align: center;" ></div>
        <div style="height: 50px; text-align: center;">
            <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="javascript:return ConfirmLeave();" UseSubmitBehavior="false" />
	    </div>
    
        <h1>Cancel Job</h1>

        <div class="form" style="text-align: center;">
        
            <div style="text-align: center;">

                <table class="form-table" style="width: 100%; text-align: center;" cellspacing="0" summary="">
                    <tr>
                        <td class="form-label-td" style="width: 50%; text-align: right;">Reason for Cancellation&nbsp;:&nbsp;</td>
                        <td class="form-label-td" style="width: 50%; text-align: left;">
                            <asp:TextBox ID="txtCancelNote" runat="server" CssClass="form-field" Text="" Width ="400px" TextMode="MultiLine" ></asp:TextBox>
                        </td> 
                    </tr>
                    <tr>
                    </tr>
                    <tr>
                        <td class="form-label-td" colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="form-label-td" colspan="2" style="text-align: center;"><b>Are you sure you want to cancel this job?</b></td>
                    </tr>
                    <tr>
                        <td class="form-label-td" style="width: 50%; text-align: right;">
                            <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Visible="true" Text="No" Width="150px" UseSubmitBehavior="false" />
                        </td>
                        <td class="form-label-td" style="width: 50%; text-align: left;">
                            <asp:Button ID="btnSave" runat="server" CssClass="form-button" Visible="true" Text="Yes" Width="150px" UseSubmitBehavior="false" />
                        </td> 
                    </tr>
                    <tr>
                        <td class="form-submit-td" colspan="2" style="text-align: left;"><asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red" /></td>
                    </tr>
                </table>


            </div>
        </div>
    </div>
        
    <div runat="server" id="DivCur1" style="display:none;">
        ID:<asp:TextBox ID="txtId" runat="server" TabIndex="-1" Text="0" />
        <br />
        ProductTypeID<asp:TextBox id="txtProductTypeID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
    </div>

    </form>
</body>
</html>
