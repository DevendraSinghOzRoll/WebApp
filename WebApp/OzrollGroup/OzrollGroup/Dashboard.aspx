<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Dashboard.aspx.vb" Inherits="ozroll_Dashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Ozroll - Dashboard</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta name="copyright" content="copyright &copy; ModernTHIS, all rights reserved" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <%--    <meta name = "viewport" content = "user-scalable = no" />--%>
   
    <link href="Content/smoothness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
        
    <link href="Content/reset.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="Content/stylenew.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
   
    <!--[if IE]><link href="../stylesheets/explorer.css" rel="stylesheet" type="text/css" media="screen" /><![endif]-->
    <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
<%--    <link href="../images/apple-touch-icon.png" rel="apple-touch-icon" type="image/x-icon" />--%>
    
    <link href="modern.png" rel="apple-touch-icon" type="image/x-icon" />

    <style type="text/css">
        .padding-0 {
            padding: 0px !important;
            padding-bottom: 50px !important;
        }
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

    <form id="form1" runat="server">
    
<div id="middle-container" class="login-container">
    <div class="middle-container dashmiddile">
        <div class="row">
            <div class="col-sm-6">
                <div class="ozrolllogo">
                    <img class="logo  img-fluid" src="images/ozrolllogo.png" style="width:250px;margin-left:25px">
                </div>
            </div>

            

            <div class="col-sm-6">
                <div class="text-center logout-sec">
                    <%--<h6>OZRoll Industries SA </h6>--%>
      <h6><asp:Label id="lblCustomerHeader" runat="server" Font-Size="Large" Font-Bold="true" CssClass="customer-header" ></asp:Label></h6>
                    <%--<button class="btn logout-btn">Logout </button>--%>
<%--                    <asp:Button ID="btnLogOut" runat="server" CssClass="btn logout-btn" Width="140" Text="Logout" />--%>
                    <asp:Button ID="btnLogOut" runat="server" CssClass="btn logout-btn" Text="Logout" UseSubmitBehavior="false" Width="140px" />

                </div>
            </div>

        </div>




        <div class="col-sm-12 text-center">
            <h5 class="pagetitle pagetitle002">OZRoll Home  </h5>
        </div>

        <div class="col-sm-12">
            <div class="row">
                <div class="col-sm-6 border-right-none home-bx text-center">
                    <a href="#">
                        <img src="../images/RollashieldLogo2016.png" class="img-fluid"/>
                         <%--<button class="btn logout-btn" style="width:140px">Rollashield </button>--%>
                  <asp:Button ID="btnRollShield" runat="server" CssClass="form-button home-grid-button" Width="140px" Text="Enter site" />

                    </a>
                </div>

                <div class="col-sm-6 home-bx home-bx2 text-center">
                    <a href="#">
                        <%--<img src="../images/SlideTrack.png" style="width: 280px; height:80px;"/><br /><br />--%>
                        <img src="../images/SlideTrack.png" class="img-fluid"/>

                        <asp:Button ID="btnSlideTrack" runat="server" CssClass="form-button home-grid-button" Width="140px" Text="Enter site" />
                       <%--<button class="btn logout-btn" style="width:140px">Slidetrack </button>--%>
                    </a>

                </div>
            </div>

            <br /><br /> 
             <div class="col-sm-12">
            <div class="row">
                <div class="col-sm-6 border-right-none home-bx text-center">
                    <a href="#">
                        <img src="../images/RollashieldLogo2016.png" class="img-fluid"/>
                         <%--<button class="btn logout-btn" style="width:140px">Rollashield </button>--%>
                  <asp:Button ID="Button1" runat="server" CssClass="form-button home-grid-button" Width="140px" Text="Enter site" />

                    </a>
                </div>

                <div class="col-sm-6 home-bx home-bx2 text-center">
                    <a href="#">
                        <%--<img src="../images/SlideTrack.png" style="width: 280px; height:80px;"/><br /><br />--%>
                        <img src="../images/SlideTrack.png" class="img-fluid"/>

                        <asp:Button ID="Button2" runat="server" CssClass="form-button home-grid-button" Width="140px" Text="Enter site" />
                       <%--<button class="btn logout-btn" style="width:140px">Slidetrack </button>--%>
                    </a>

                </div>
            </div>
        </div>
        </div>
    </div>
    <div class="row">

    <div class="col-md-12" style="text-align: center;"">
     <asp:Label ID="lblStatus" runat="server" ForeColor="Red"></asp:Label>
    </div>
  </div>

    
</div>

</form>
</body>
</html>
