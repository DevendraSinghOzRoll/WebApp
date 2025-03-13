<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login.aspx.vb" Inherits="OzrollGroup.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  
    <title>Ozroll - Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta name="copyright" content="copyright &copy; ModernTHIS, all rights reserved" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
<%--    <meta name = "viewport" content = "user-scalable = no" />--%>
    <meta name="format-detection" content="telephone=no" />
    
    <!--[if lt IE 8]><script src="javascript/IE-fix.js" type="text/javascript"></script><![endif]-->


    <link href="Content/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <link href="Content/smoothness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-3.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
        
    <link href="Content/reset.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="Content/stylenew.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
   
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
	            //document.getElementById("<%=lblStatus.ClientID %>").innerHTML = "";
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
                    UserNamePasswordRequired("Please enter your username.", "username");
                    bolValid = 0;
                }
            }
            if (bolValid == 1) {
                if (document.getElementById("<%=txtPassword.ClientID %>").value == "") {
                    UserNamePasswordRequired("Please enter your password.", "paasword");
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

        function UserNamePasswordRequired(msg, type) {
            if (type == "username") {
                document.getElementById("dialog-confirm-user-message").innerHTML = msg;
                $("#dialog-confirm-user").dialog("open");
                return false;
            }
            else if (type == "paasword") {
                document.getElementById("dialog-confirm-password-message").innerHTML = msg;
                $("#dialog-confirm-password").dialog("open");
                return false;
            }

        }

      function ConfirmReset() {
            document.getElementById("dialog-confirm-reset-message").innerHTML = "Would you like to reset your password?";
             $("#dialog-confirm-reset").dialog("open");
             return false;
         }

        $(function () {
            $("#dialog-confirm-reset").dialog({
                modal: true,
                autoOpen: false,
                draggable: false,
                buttons: {
                    "Yes": function () {
                        $(this).dialog("close");
                        __doPostBack('<%=btnPswdReset.ClientID %>', '');
                        return true;
                    },
                    "No": function () {
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
        });

        $(function () {
            $("#dialog-confirm-user").dialog({
                modal: true,
                autoOpen: false,
                draggable: false,
                buttons: {
                    "Ok": function () {
                        $(this).dialog("close");
                        document.getElementById("<%=txtUsername.ClientID %>").focus();
                        return false;
                    },
                }
            });
        });

        $(function () {
            $("#dialog-confirm-password").dialog({
                modal: true,
                autoOpen: false,
                draggable: false,
                buttons: {
                    "Ok": function () {
                        $(this).dialog("close");
                        document.getElementById("<%=txtPassword.ClientID %>").focus();
                        return false;
                    },
                }
            });
        });
    </script>
    <style type="text/css">
        .auto-style1 {
            background-color: #ffffff;
            border-top: 5px solid #ffffff;
            padding: 5px 10px 5px 10px;
            text-align: right;
            vertical-align: middle;
            width: 35%;
            height: 51px;
        }        

    </style>
</head>

<body class="login-bg">



<div id="middle-container" class="login-container">

<%--    <div id="logo" style="height: 100px; text-align: center;" ></div>--%>
    <div id="header" style="text-align: center;margin-bottom:70px">
       <div class="col-sm-12 col-md-12 col-lg-12">
           
           </div>
      <div id="ozrollLogo" style="text-align: left;float:left;width:25%" >
           <img src="../images/ozrollLogo.png" alt="OZROLL" />                   
        </div>
        <div style="text-align: left;float:left;width:25%" >
          <asp:Image id="Image2" runat="server" src="../Images/RollashieldLogo2016.png" alt="ROLLASHIELD" width="170px" />                
        </div>
        
        <div style="float:left; text-align: center;width:25%">
          <asp:Image id="Image1" runat="server" src="../Images/SlideTrack.png" alt="ROLLASHIELD" width="170px" Heigh="48px" />  
        </div>
        <div style="text-align: left;float:left;width:25%" >
           <asp:Image id="imgLouvres1" runat="server" src="../Images/OZRollElipso.png" alt="ELIPSO" width="170px" />              
        </div>
    </div>
    
    <div class="header-spacer" ></div>
        <div class="clearfix"></div>
    
    <div class="col-sm-12 col-md-12 col-lg-12">
      <div class="login-inner">
        <%--<div class="card card-signin my-5">--%>

          <div class="card-body login">
            <h3 class="">One Login</h3>
              <form runat="server"  class="form-signin" id="loginForm" method="post" defaultbutton="btnLogin" defaultfocus="txtUsername">
           
              <div class="form-label-group">
                   <label for="inputEmail">Username</label>
                   <asp:TextBox runat="server" ID="txtUsername" MaxLength="50" onpaste="return false" CssClass="form-control" OnDrop="return false;" />         
              </div>

              <div class="form-label-group">
                   <label for="inputPassword">Password</label>
                   <asp:TextBox runat="server" ID="txtPassword" MaxLength="50" onpaste="return false" CssClass="form-control" TextMode="Password" OnDrop="return false;" />

              </div>

                 <asp:Button ID="btnLogin" runat="server" CssClass="ozrol-btn" Text="Login" Enabled="false" />
                <asp:Label ID="lblStatus" runat="server" ForeColor="Red"><noscript>Please enable javascript to run on your computer before logging in.</noscript></asp:Label> 
               
               <asp:Button ID="btnPswdReset" runat="server" Text="Forgot Password" style="color:orangered;text-align:center; background-color:#fff;"  BorderStyle="None" Font-Underline="True" />    
                        <asp:Button ID="btnLouvrePswdReset" runat="server" Text="Reset" style="color:orangered;text-align:center; background-color:#fff;" BorderStyle="None" Font-Underline="True" OnClientClick="return ConfirmReset();" />                        
                    <asp:Label ID="lblResetSent" runat="server" Text="Forgot Password" style="text-align:center" Visible="false" />    

            </form>
          </div>
       <%-- </div>--%>
      </div>

    

    

    <div runat="server" id="DivCur1" style="display:none;">
        <div id="dialog-confirm-reset" title="Please Confirm">
            <p id="dialog-confirm-reset-message" style="text-align: left;"></p>
        </div> 
         <div id="dialog-confirm-user" title="Please Confirm">
            <p id="dialog-confirm-user-message" style="text-align: left;"></p>
        </div> 
        <div id="dialog-confirm-password" title="Please Confirm">
            <p id="dialog-confirm-password-message" style="text-align: left;"></p>
        </div> 
    </div>    
</div>

   </div>


</body>
</html>