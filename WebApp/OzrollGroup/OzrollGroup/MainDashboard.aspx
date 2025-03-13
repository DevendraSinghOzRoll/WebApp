<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MainDashboard.aspx.vb" Inherits="OzrollGroup.MainDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <style>
        .styled-table {
            border-collapse: collapse;
            margin: 25px 0;
            font-size: 0.9em;
            font-family: sans-serif;
            min-width: 400px;
            box-shadow: 0 0 20px rgba(0, 0, 0, 0.15);
        }

            .styled-table thead tr {
                background-color: #009879;
                color: #ffffff;
                text-align: left;
            }

            .styled-table th,
            .styled-table td {
                padding: 12px 15px;
            }

            .styled-table tbody tr {
                border-bottom: 1px solid #dddddd;
            }

                .styled-table tbody tr:nth-of-type(even) {
                    background-color: #f3f3f3;
                }

                .styled-table tbody tr:last-of-type {
                    border-bottom: 2px solid #009879;
                }

                .styled-table tbody tr.active-row {
                    font-weight: bold;
                    color: #009879;
                }
    </style>

</head>

<body>
    <form id="form1" runat="server">
        <div id="middle-container" class="login-container">
            <div class="middle-container dashmiddile">
                <div class="row">
                    <div class="col-sm-6">
                        <div class="ozrolllogo">
                            <img class="logo  img-fluid" src="images/ozrolllogo.png" style="width: 250px; margin-left: 25px">
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="text-center logout-sec">
                            <%--<h6>OZRoll Industries SA </h6>--%>
                            <h6>
                                <asp:Label ID="lblCustomerHeader" runat="server" Font-Size="Large" Font-Bold="true" CssClass="customer-header"></asp:Label></h6>
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

                        <asp:DataList ID="DataList3" runat="server" RepeatDirection="Horizontal" RepeatColumns="2" OnItemCommand="DataList3_ItemCommand" Width="100%" OnItemDataBound="DataList3_ItemDataBound" >
                            <ItemTemplate>
                                <div class="col-sm-12 border-right-none home-bx text-center" style="padding-bottom:30px">

                                    <a href="#">
                                        <img src='<%# Container.DataItem("websitelogo") %>' class="img-fluid"  />
                                        <%--<button class="btn logout-btn" style="width:140px">Rollashield </button>--%>
                                        <asp:Label ID="lblWebSiteName" runat="server" Text='<%# Container.DataItem("WebName") %>' Visible="false"></asp:Label>
                                        <asp:Button ID="btnRollShield" runat="server" CssClass="form-button home-grid-button" Width="140px"   Enabled="false"
                                            Text='<%# Container.DataItem("WebName") %>'
                                            CommandName="addtocart" />
                                        <asp:LinkButton ID="linkWebUrl" Text='<%# Container.DataItem("WebUrl") %>' runat="server"  Visible="false"></asp:LinkButton>
                                        <asp:LinkButton ID="linkWebLinkId" Text='<%# Container.DataItem("WebLinkId") %>' runat="server"  Visible="false"></asp:LinkButton>
                                    </a>
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>


                 <div class="col-sm-12 text-center">
                       
                           <asp:Label ID="lblMessgaeStatus" runat="server" ForeColor="Red"></asp:Label>
                       
                     </div>

            </div>
        </div>
    </form>
</body>
</html>
