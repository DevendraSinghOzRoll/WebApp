<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="ozroll_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Login - Ozroll</title>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta name="copyright" content="copyright &copy; Ozroll, all rights reserved" />

    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
<%--    <meta name = "viewport" content = "user-scalable = no" />--%>
    <meta name="format-detection" content="telephone=no" />
    
    <!--[if lt IE 8]><script src="javascript/IE-fix.js" type="text/javascript"></script><![endif]-->
    
    <link href="stylesheets/reset.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="stylesheets/style.css" rel="stylesheet" type="text/css" media="screen" />
    <!--[if IE]><link href="../stylesheets/explorer.css" rel="stylesheet" type="text/css" media="screen" /><![endif]-->
    <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
<%--    <link href="../images/apple-touch-icon.png" rel="apple-touch-icon" type="image/x-icon" />--%>
    
    <link href="modern.png" rel="apple-touch-icon" type="image/x-icon" />
    
    <script language="javascript" type="text/javascript">
        if (window.history.forward(1) != null)
            window.history.forward(1);
        
        function DoTheTest() {
            document.cookie = 'TemporaryTestCookie=yes; path=/';
            setTimeout('TestIfCookieWasSet()',1000);
        }
        function TestIfCookieWasSet() {
            var testcookie = '';
            if (document.cookie.length > 0) {
	            var cookiename = 'TemporaryTestCookie=';
	            var cookiebegin = document.cookie.indexOf(cookiename);
	            var cookieend = 0;
	            if (cookiebegin > -1) {
		            cookiebegin += cookiename.length;
		            cookieend = document.cookie.indexOf(";",cookiebegin);
		            if (cookieend < cookiebegin) { cookieend = document.cookie.length; }
		            testcookie = document.cookie.substring(cookiebegin,cookieend);
		        }
	        }
	        if (testcookie == 'yes') {
	            document.getElementById("<%=btnLogin.ClientID %>").disabled = false;
	            document.getElementById("<%=btnLogin.ClientID %>").className = "form-button";
	            document.getElementById("<%=lblStatus.ClientID %>").innerHTML = "";
	        }
	        else {
	            document.getElementById("<%=btnLogin.ClientID %>").disabled = true;
	            document.getElementById("<%=btnLogin.ClientID %>").className = "form-button-disabled";
	            document.getElementById("<%=lblStatus.ClientID %>").innerHTML = "Please enable cookies on your computer before logging in.";
	        }
        }
        DoTheTest();
        function ValidateAll() {
            var bolValid = 1;
            if (bolValid == 1) {
                if (document.getElementById("<%=txtUsername.ClientID %>").value == "") {
                    alert("Please enter your username.");
                    bolValid = 0;
                }
            }
            if (bolValid == 1) {
                if (document.getElementById("<%=txtPassword.ClientID %>").value == "") {
                    alert("Please enter your password.");
                    bolValid = 0;
                }
            }
            if (bolValid == 1) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
</head>

<body>

<form runat="server" id="loginForm" method="post" defaultbutton="btnLogin" defaultfocus="txtUsername">

<div id="middle-container">

    <div id="logo" style="height: 100px; text-align: center;" ></div>
    
    <h1>Login</h1>

    <div class="form">
        <div class="step">
            <table class="form-table" cellspacing="0" summary="">
                <tr>
                    <td class="form-label-td">Username:</td>
                    <td class="form-field-td">
                        <asp:TextBox runat="server" ID="txtUsername" MaxLength="50" CssClass="form-field" />
                    </td>
                </tr>
                <tr>
                    <td class="form-label-td">Password:</td>
                    <td class="form-field-td">
                        <asp:TextBox runat="server" ID="txtPassword" MaxLength="50" CssClass="form-field" TextMode="Password" />
                    </td>
                </tr>
                <tr>
                    <td class="form-submit-td" colspan="2">
                        <asp:Button ID="btnLogin" runat="server" CssClass="form-button-disabled" Text="Login" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td class="form-submit-td" colspan="2">
                        <asp:Label ID="lblStatus" runat="server" ForeColor="Red"><noscript>Please enable javascript to run on your computer before logging in.</noscript></asp:Label>
                        <asp:Label ID="lblLoginMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
		    </table>
	    </div>
    </div>
    
</div>

</form>

</body>
</html>
