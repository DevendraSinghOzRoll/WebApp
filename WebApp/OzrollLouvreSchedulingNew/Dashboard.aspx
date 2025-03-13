<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Dashboard.aspx.vb" Inherits="Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   	<title>Dashboard</title>
	<meta charset="UTF-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />

	<link rel="stylesheet" type="text/css" href="stylesheets/bootstrap.min.css" />
	<link rel="stylesheet" type="text/css" href="stylesheets/style.css" />
	<link href="https://fonts.googleapis.com/css2?family=Open+Sans:wght@300;400;600;700&display=swap" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
       <div class="middle-container padding-0">
	
	<div class="row">
	<div class="col-sm-4">
	<div class=""> 
	<img class="logo" src="images/ozrolllogo.png" width="200px" />	
	</div>
	</div>
	
	<div class="col-sm-4">
	<img class="logo" src="images/Rollashield-logo.jpg" width="200px" />	
	</div>
	
	<div class="col-sm-4">
	<div class="text-center logout-sec">
	<h6><asp:Label runat="server" ID="lblUserName"></asp:Label> </h6>
	
		<asp:Button runat="server" ID="btnLogOut" CssClass="btn logout-btn" Text="Logout" />

	</div>
	</div>
	
	</div>
	
	
	

	<div class="col-sm-12 text-center">
	<h5 class="pagetitle "> OZRoll Home  </h5>
	</div>

<div class="col-sm-12">
	<div class="row">
	<div  class="col-sm-6 border-right-none home-bx text-center">
	<a href="/LouvreJobDetails.aspx?ScheduleId=0&ViewType=0"><img src="images/svg/job.svg" />
	<h6>New Elipso Aluminium Order / Quote </h6>  </a>
	</div>
	
	<div  class="col-sm-6 home-bx text-center">
	<a href="/ProductionScheduleList.aspx"><img src="images/svg/tracking.svg" />
	<h6>Order Tracking</h6> </a>
	
	</div>
	
	
	<div  class="col-sm-6 border-btm-none border-right-none home-bx text-center">
		<asp:LinkButton runat="server" ID="lnkPriceMatrix"><img src="images/svg/price-matrix-2.svg" />
	
	<h6>Pricing Matrix</h6> </asp:LinkButton>
	</div>
	
	<div runat="server" id="dvAdmin" class="col-sm-6 border-btm-none home-bx text-center">
	<asp:LinkButton runat="server" ID="lnkAdministration"><img src="images/svg/technical-support.svg" />
		<h6>Admin</h6>
	</asp:LinkButton>
	<%--<a href="#">
	<h6>Administration</h6> </a>--%>
	
	</div>
	
	
	</div>
	</div>
	</div>
    </form>
	<!--===============================================================================================-->
	<script src="js/jquery-3.2.1.min.js"></script>
	<script src="js/bootstrap.min.js"></script>
</body>
</html>
