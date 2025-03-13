<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PasswordResetLouvres.aspx.vb" Inherits="PasswordResetLouvres" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title>Ozroll - Password Reset</title>

        <meta http-equiv="content-type" content="text/html; charset=utf-8" />
        <meta name="copyright" content="copyright &copy; Ozroll, all rights reserved" />

        <%--    <meta name = "viewport" content = "user-scalable = no" />--%>
    
        <!--[if lt IE 8]><script src="javascript/IE-fix.js" type="text/javascript"></script><![endif]-->
    
        <link href="stylesheets/reset.css" rel="stylesheet" type="text/css" media="screen" />
        <link href="stylesheets/style.css" rel="stylesheet" type="text/css" media="screen" />

        <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />

        <script language="javascript" type="text/javascript">

            if (window.history.forward(1) != null)
                window.history.forward(1);
        
        </script>

        <style type="text/css">

        </style>
    </head>

    <body>
        <form runat="server" id="loginForm" method="post" defaultbutton="btnSaveNewPassword" defaultfocus="txtNewPassword">

         <div id="middle-container-diallerview" >

        <div id="logo" style="height: 100px; text-align: center;" ></div>
        <div style="height: 50px; text-align: center;">
	    </div>
    
    
                <h1>Password Reset</h1>

                <div class="form">
                    <asp:Panel ID="pnlPasswordEntry" runat="server" >
                        <table class="form-table" cellspacing="0" summary="">
                            <tr>
                                <td class="form-label-td" style="text-align:right; width:50%;">New Password</td>
                                <td class="form-field-td" style="text-align:left; width:50%;">
                                    <asp:TextBox runat="server" ID="txtNewPassword" type="password" MaxLength="50" onpaste="return false" CssClass="form-field" 
                                        width="200px" OnDrop="return false;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="form-label-td" style="text-align:right; width:50%;"></td>
                                <td class="form-field-td" style="text-align:left; width:50%;">
                                    <asp:RequiredFieldValidator runat="server" ID="valrflNewPassword" CssClass="validation" 
                                        ControlToValidate="txtNewPassword" EnableClientScript="true" Display="Dynamic" ValidationGroup="password"
                                        text="Please enter a password." />
                                </td>
                            </tr>
                            <tr>
                                <td class="form-label-td" style="text-align:right; width:50%;">Confirm New Password</td>
                                <td class="form-field-td" style="text-align:left; width:50%;">
                                    <asp:TextBox runat="server" ID="txtConfirmNewPassword" type="password" MaxLength="50" onpaste="return false" CssClass="form-field" 
                                        width="200px" TextMode="Password" OnDrop="return false;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="form-label-td" style="text-align:right; width:50%;"></td>
                                <td class="form-field-td" style="text-align:left; width:50%;">
                                    <asp:RequiredFieldValidator runat="server" ID="valrfConfirmNewPassword" CssClass="validation" 
                                        ControlToValidate="txtConfirmNewPassword" EnableClientScript="true" Display="Dynamic" ValidationGroup="password"
                                        text="Please confirm the password." />
                                    <asp:CustomValidator ID="valcustConfirmNewPassword" runat="server" CssClass="validation"
                                        EnableClientScript="false" Display="Dynamic" ValidationGroup="password" Text="" />
                                </td>
                            </tr>
                            <tr>
                                <td class="form-submit-td" colspan="2" style="text-align:center;">
                                    <br />
                                    <asp:Button ID="btnSaveNewPassword" runat="server" CssClass="form-button" Text="Save Password" width="164px" ValidationGroup="password" />
                                </td>
                            </tr>
		                </table>
	                </asp:Panel>

                    <div style="text-align:center">
                        <asp:Label ID="lblStatus" runat="server" Visible="false" ></asp:Label>
                        <br />
                        <br />
                        <asp:Button ID="btnRedirectLogin" runat="server" CssClass="form-button" Text="Login" Visible="false" Enable="false" />
                    </div>

                    <br />
                </div>
            </div>
        </form>
    </body>
</html>
