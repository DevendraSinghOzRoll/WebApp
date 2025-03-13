<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Home.aspx.vb" Inherits="Home" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Ozroll Louvre Production Home</title>
        
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

            });

    </script>

    <style type="text/css">

        .menu-button {
            margin: 5px;
        }

        .header-menu-button {
            margin: 5px;
            width: 140px !important;
        }

        .warning {
            text-align:center;
            width: 100%;
            margin-top: 20px;
        }

    </style>

</head>
<body>
    <form runat="server" id="form1" method="post">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    <Scripts>
    <asp:ScriptReference Path="javascript/fixfocus.js" />
    </Scripts>
    </asp:ScriptManager>
    <div id="middle-container-diallerview" >

        <div id="logo" style="height: 100px; text-align: center;" ></div>
        <div style="height: 50px; text-align: center;">
            <asp:Button ID="btnLogout" runat="server" CssClass="form-button header-menu-button" Visible="true" Text="Logout" Width="100px" OnClientClick="javascript:return ConfirmLeave();" UseSubmitBehavior="false" />
            <asp:Button ID="btnAdmin" runat="server" CssClass="form-button header-menu-button" Visible="false" Text="Admin" Width="100px" UseSubmitBehavior="false" />
	    </div>

        <div class="form" style="text-align: center;">            
            <asp:Button ID="btnLouvreReport" runat="server" CssClass="form-button header-menu-button" Visible="false" Text="Louvre Report" UseSubmitBehavior="false" />
            <asp:Button ID="btnReports" runat="server" CssClass="form-button header-menu-button" Visible="true" Text="Reports" UseSubmitBehavior="false" />
            <asp:Button ID="btnCustomers" runat="server" CssClass="form-button header-menu-button" Visible="true" Text="Customers" UseSubmitBehavior="false" />
            <asp:Button ID="btnStockPicking" runat="server" CssClass="form-button header-menu-button" Visible="false" Text="Stock Picking" UseSubmitBehavior="false" />
        &nbsp;<asp:Button ID="btnCustomerSample" runat="server" CssClass="form-button" Visible="true" Text="Customer Sample" UseSubmitBehavior="false" Width="180px" />
        </div>

        <div class="form" style="text-align: center;">            
            <asp:Button ID="btnDeliveryDocket" runat="server" CssClass="form-button" Visible="false" Text="Delivery Docket" UseSubmitBehavior="false" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnRunningSheet" runat="server" CssClass="form-button" Visible="false" Text="Running Sheet" UseSubmitBehavior="false" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnTimeSheet" runat="server" CssClass="form-button" Visible="false" Text="Time Sheet" UseSubmitBehavior="false" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnCoverSheet" runat="server" CssClass="form-button" Visible="false" Text="Cover Sheet" UseSubmitBehavior="false" />
            &nbsp;&nbsp;&nbsp;
            </div>
    
        <h1>OzRoll Production Home</h1>

        <div class="form" style="text-align: center;">            
            <asp:Button ID="btnProductionList" runat="server" CssClass="form-button menu-button" Visible="true" Text="Production Search" Width="270px" UseSubmitBehavior="false" />
            <asp:Button ID="btnScheduleDateUpdate" runat="server" CssClass="form-button menu-button" Visible="true" Text="Schedule Date Update" Width="270px" UseSubmitBehavior="false" />
            <asp:Button ID="btnPlannedShippingDate" runat="server" CssClass="form-button menu-button" Visible="true" Text="Planned Shipping Date Update" Width="270px" UseSubmitBehavior="false" />
            
            <asp:Button ID="btnPricingMatrix" runat="server" CssClass="form-button home-grid-button" Text="Pricing Matrix" UseSubmitBehavior="false" Width="200px"/>
        </div>

        <asp:panel id="pnlWarning" runat="server" CssClass="warning blink">
            <img src="images/warning.png" height="40px" width="40px" alt="warning" title="There are unresolved errors on this page." />
        </asp:panel>

        <div class="form" style="text-align: center;">            
            <h5><asp:Label ID="lblScheduleTodayHeading" runat="server"></asp:Label></h5>
            <asp:Label ID="lblScheduleToday" runat="server"></asp:Label>  
            <br />
            <br />
            <asp:Panel ID="pnlDailySchedule" runat="server" Visible="false">
                <a href="DailyScheduleUpdate.aspx">Update Daily Schedule</a>         
            </asp:Panel>
        </div>

        <div class="form" style="text-align: center;">            
            <h5><asp:Label ID="lblScheduleTomorrowHeading" runat="server"></asp:Label></h5>
            <asp:Label ID="lblScheduleTomorrow" runat="server"></asp:Label>           
        </div>

        <asp:Panel ID="pnlNextDay" runat="server" Visible="false">
            <div class="form" style="text-align: center;">            
                <h5><asp:Label ID="lblScheduleNextDayHeading" runat="server"></asp:Label></h5>
                <asp:Label ID="lblScheduleNextDay" runat="server"></asp:Label>           
            </div>
        </asp:Panel>

        <br />

        <div class="form" style="text-align: center;">            
            <h5><asp:Label ID="lblHighPriorityJobsHeading" runat="server"></asp:Label></h5>
            <asp:Label ID="lblHighPriorityJobs" runat="server"></asp:Label> 
            <br />
            <br />  
            <asp:Panel ID="pnlHighPriority" runat="server" Visible="false">
                <a href="ProductionScheduleList.aspx?prioritylevel=1&activeonly=1">List High Priority Jobs</a>
            </asp:Panel>
            
        </div>

        <br />

        <div class="form" style="text-align: center;">            
            <h5><asp:Label ID="lblAwaitAcceptanceHeading" runat="server"></asp:Label></h5>
            <asp:Label ID="lblAwaitAcceptance" runat="server"></asp:Label>           
            <br />
            <br />
            <asp:Panel ID="pnlAwaitAcceptance" runat="server" Visible="false">
                <a href="UpdateAwaitingAcceptance.aspx">Update Awaiting Acceptance</a>
            </asp:Panel>
        </div>

        <br />

        <asp:Panel ID="pnlShowShutterPro" runat="server">
            <div class="form" style="text-align: center;">            
                <h5><asp:Label ID="lblEnterShutterProHeading" runat="server"></asp:Label></h5>
                <asp:Label ID="lblEnterShutterPro" runat="server"></asp:Label>     
                <br />
                <br />
                <asp:Panel ID="pnlShutterPro" runat="server" Visible="false">
                    <a href="UpdateEnteredShutterPro.aspx">Update To Be Entered Into Shutter Pro</a>
                </asp:Panel>
            </div>
        </asp:Panel>

        <br />

        <div class="form" style="text-align: center;">            
            <h5><asp:Label ID="lblToDespatchHeading" runat="server"></asp:Label></h5>
            <asp:Label ID="lblToDespatch" runat="server"></asp:Label>     
            <br />
            <br />
            <asp:Panel ID="pnlToDespatch" runat="server" Visible="false">
                <a href="TobeDespatchedUpdate.aspx">Update To Be Despatched</a>
            </asp:Panel>
        </div>

        <br />

        <div class="form" style="text-align: center;">            
            <h5><asp:Label ID="lblToCollectHeading" runat="server"></asp:Label></h5>
            <asp:Label ID="lblToCollect" runat="server"></asp:Label>     
            <br />
            <br />
            <asp:Panel ID="pnlCollectFactory" runat="server" Visible="false">
                <a href="UpdateCollectedFromFactory.aspx">Update To Be Collected</a>
            </asp:Panel>
        </div>

        <br />

        <div class="form" style="text-align: center;">            
            <h5><asp:Label ID="lblToInvoicedHeading" runat="server"></asp:Label></h5>
            <asp:Label ID="lblToInvoice" runat="server"></asp:Label>     
            <br />
            <br />
            <asp:Panel ID="pnlAwaitingInvoice" runat="server" Visible="false">
                <a href="UpdateAwaitingInvoicing.aspx">Update To Be Invoiced</a>
            </asp:Panel>
        </div>

        <br />

        <div class="form" style="text-align: center;">            
            <h5><asp:Label ID="lblInProgressHeading" runat="server"></asp:Label></h5>
            <asp:Label ID="lblInProgress" runat="server"></asp:Label>           
        </div>

        <br />

        <div class="form" style="text-align: center;">            
            <h5><asp:Label ID="lblWIPHeading" runat="server"></asp:Label></h5>
            <asp:Label ID="lblWIP" runat="server"></asp:Label>           
        </div>

        <br />

        <div class="form" style="text-align: center;">            
            <h5><asp:Label ID="lblWIPRemakesHeading" runat="server"></asp:Label></h5>
            <asp:Label ID="lblWIPRemakes" runat="server"></asp:Label>           
        </div>

        <br />

        <div class="form" style="text-align: center;">            
            <h5><asp:Label ID="lblWIPReordersHeading" runat="server"></asp:Label></h5>
            <asp:Label ID="lblWIPReorders" runat="server"></asp:Label>           
        </div>

    </div>

    <div id="dialog-alert" title="Insufficient Information">
        <p id="dialog-alert-message" style="text-align: left;"></p>
    </div>

    <div id="dialog-confirm" title="Please Confirm" >
        <p id="dialog-confirm-message" style="text-align: left;"></p>
    </div>
        
    <div id="dialog-confirm-cancel" title="Please Confirm" >
        <p id="dialog-confirm-cancel-message" style="text-align: left;"></p>
    </div>

    <div runat="server" id="DivCur1" style="display:none;">
    <br />
        ProductTypeID<asp:TextBox id="txtProductTypeID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />

    </div>

    </form>
</body>
</html>
