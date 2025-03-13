<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CreateLogin_shutter.aspx.vb" Inherits="CreateLogin_Shutter" Theme="SknGridView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxcontroltoolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>OzRoll - Create Login</title>
    
    <meta name="copyright" content="copyright &copy; Ozroll, all rights reserved" />

    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta name="format-detection" content="telephone=no" />
    
    <link href="stylesheets/reset.css" rel="stylesheet" type="text/css" media="screen" />
    <!--[if IE]><link href="stylesheets/explorer.css" rel="stylesheet" type="text/css" media="screen" /><![endif]-->
    <link href="stylesheets/style.css?version=<%= Now.Year() & Now.Month & Now.Day() %>" rel="stylesheet" type="text/css" media="screen" />
    <link href="stylesheets/smoothness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />
     <link href="stylesheets/css/reset.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="stylesheets/css/style.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="stylesheets/css/checkBox.css" rel="stylesheet"  type="text/css" media="screen"/>
    <!--[if lt IE 8]><script src="javascript/IE-fix.js" type="text/javascript"></script><![endif]-->
    <script src="javascript/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>  
    <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <script src="Scripts/Miscellaneous.js" type="text/javascript"></script>
    <link href="stylesheets/smoothness/jquery-ui-1.8.18.custom.css" rel="stylesheet" />
    <script type="text/javascript"> 
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
        function ConfirmLeave() {
            document.getElementById("dialog-confirm-message").innerHTML = "Are you sure you want to logout?";
            //$("#dialog-confirm-message").html("Are you sure you want to logout?")
            $("#dialog-confirm").dialog("open");
            return false;
        }
        function Validation() {           

           
            if ($("#txtCustomerName").val() == "" && $("#chkUserType").prop("checked") == false) {
                alert("Enter Customer Name");
                $("#txtCustomerName").focus();
                return false;
            }

            else if ($("#chkUserType").prop("checked")==false &&  $("#hfCustomerID").val() == "" && $("#txtCustomerName").val()!= "") {
                alert("Not a valid customer.");
                $("#txtCustomerName").focus();
                return false;
            }
            
            else if ($("#txtUsername").val() == "") {
                alert("Enter Login Name.");
                $("#txtUsername").focus();
                return false;
            }
            else if ($("#txtFirstName").val() == "") {
                alert("Enter First Name");
                $("#txtFirstName").focus();
                return false;
            }
            else if ($("#txtPassword").val() == "") {
                alert("Enter Password");
                $("#txtPassword").focus();
                return false;
            }
            else if ($("#txtConfirmPassword").val() == "") {
                alert("Enter Confirm Password");
                $("#txtConfirmPassword").focus();
                return false;
            }

            else if ($("#txtConfirmPassword").val()!= $("#txtPassword").val()) {
                alert("Password and confirm password should be same");
                $("#txtConfirmPassword").focus();
                return false;
            }



        }
    </script>
    
    
    <style type="text/css">
        .auto-style1 {
            height: 100%;
            padding: 0;
            position: relative;
            width: 100%;
            left: 0px;
            top: 0px;
        }
        .auto-style13 {
            background-color: #F2F2F2;
            border-top: 5px solid #ffffff;
            color: #333333;
            font-family: Tahoma, "Trebuchet MS", Arial, sans-serif;
            font-size: 1em;
            line-height: 1.5em;
            padding: 5px 10px 5px 10px;
            text-align: left;
            vertical-align: middle;
            width: 35%;
            height: 48px;
        }
        .auto-style14 {
            width: 100%;
        }

        .auto-style43 {
            background-color: #F2F2F2;
            border-top: 5px solid #ffffff;
            color: #333333;
            font-family: Tahoma, "Trebuchet MS", Arial, sans-serif;
            font-size: 1em;
            line-height: 1.5em;
            padding: 5px 10px 5px 10px;
            text-align: left;
            vertical-align: middle;
            width: 35%;
        }
        .auto-style50 {
            background-color: #F2F2F2;
            border-top: 5px solid #ffffff;
            color: #333333;
            font-family: Tahoma, "Trebuchet MS", Arial, sans-serif;
            font-size: 1em;
            line-height: 1.5em;
            padding: 5px 10px 5px 10px;
            text-align: left;
            vertical-align: middle;
            width: 19%;
            height: 48px;
        }
        .auto-style57 {
            background-color: #F2F2F2;
            border-top: 5px solid #ffffff;
            color: #333333;
            font-family: Tahoma, "Trebuchet MS", Arial, sans-serif;
            font-size: 1em;
            line-height: 1.5em;
            padding: 5px 10px 5px 0px;
            text-align: left;
            vertical-align: middle;
            width: 50%;
        }
        .auto-style62 {
            background-color: #F2F2F2;
            border-top: 5px solid #ffffff;
            color: #333333;
            font-family: Tahoma, "Trebuchet MS", Arial, sans-serif;
            font-size: 1em;
            line-height: 1.5em;
            padding: 5px 10px 5px 10px;
            text-align: left;
            vertical-align: middle;
        }
        .auto-style66 {
            background-color: #F2F2F2;
            border-top: 5px solid #ffffff;
            color: #333333;
            font-family: Tahoma, "Trebuchet MS", Arial, sans-serif;
            font-size: 1em;
            line-height: 1.5em;
            padding: 5px 10px 5px 10px;
            text-align: left;
            vertical-align: middle;
            width: 12%;
            height: 48px;
        }
        .auto-style70 {
            background-color: #F2F2F2;
            border-top: 5px solid #ffffff;
            color: #333333;
            font-family: Tahoma, "Trebuchet MS", Arial, sans-serif;
            font-size: 1em;
            line-height: 1.5em;
            padding: 5px 10px 5px 10px;
            text-align: left;
            vertical-align: middle;
            width: 14%;
            height: 48px;
        }
        .auto-style74 {
            height: 37px;
        }
        .auto-style75 {
            background-color: #F2F2F2;
            border-top: 5px solid #ffffff;
            color: #333333;
            font-family: Tahoma, "Trebuchet MS", Arial, sans-serif;
            font-size: 1em;
            line-height: 1.5em;
            padding: 5px 10px 5px 10px;
            text-align: left;
            vertical-align: middle;
            width: 17%;
            height: 48px;
        }
          
        
        
           
        .auto-style77 {
            height: 50px;
        }
  
        
        
           
        .auto-style78 {
            background-color: #F2F2F2;
            border-top: 5px solid #ffffff;
            color: #333333;
            font-family: Tahoma, "Trebuchet MS", Arial, sans-serif;
            font-size: 1em;
            line-height: 1.5em;
            padding: 5px 10px 5px 10px;
            text-align: left;
            vertical-align: middle;
            height: 48px;
        }
  
        
        
           
     </style>
    
    
</head>
<body>
      
<form runat="server" id="createLoginForm" method="post">


    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    <Scripts>
    <asp:ScriptReference Path="javascript/fixfocus.js" />
    </Scripts>
    </asp:ScriptManager>

    <div id="middle-container-diallerview" >

        <div id="header" style="text-align: center;">
            <div id="ozrollLogo" style="text-align: left; float:left;" >
                <img src="../images/ozrollLogo.png" alt="OZROLL" />
<%--                <asp:Image id="imgShutters1" runat="server" src="../Images/RollashieldLogo2016.png" alt="ROLLASHIELD" width="287px" height="97px" style="padding-left: 40px;" />--%>
            </div>
            <div style="float:right; text-align: center;">
                <asp:Label id="lblCustomerHeader" runat="server" Font-Size="Large" Font-Bold="true" CssClass="customer-header" ></asp:Label>&nbsp;&nbsp;
                <asp:Button ID="btnHome" runat="server" CssClass="form-button toolbar-button" Text="Home" UseSubmitBehavior="false" width="80px" />
                <asp:Button ID="btnDashBoard" runat="server" CssClass="form-button toolbar-button" Visible="false" Text="Dashboard" UseSubmitBehavior="false" width="80px" />
                <%-- <asp:Button ID="btnSettings" runat="server" CssClass="form-button toolbar-button" Text="Settings" UseSubmitBehavior="false" width="80px" /> --%>
                <asp:Button ID="btnLogout" runat="server" CssClass="form-button toolbar-button" Visible="true" Text="Logout" OnClientClick="return ConfirmLeave();" UseSubmitBehavior="false" Width="80px" />
            </div>
        </div>
            
    <h1>Admin Login Mgmt.</h1>
    

    <div style="text-align: center;" class="auto-style74">
   
     <table class="auto-style14" cellspacing="0" summary="">
         <%--<tr>
            <td class="auto-style57" style="text-align: right;">Customer Name:</td>
            <td class="auto-style57" style="width: 50%; text-align: left;">
                <asp:Label ID="lblCustomerName" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                <asp:HiddenField ID="hfCustomerID" runat="server" />
                <asp:HiddenField ID="hfUserID" runat="server" />
                <asp:DropDownList ID="cboCustomerName" runat="server" Width="200px" Height="28px" AutoPostBack="True" />
             </td>
         </tr>--%>
         <%--       <tr>
            <td class="auto-style57" style="text-align: right;">Customer ID:</td>
            <td class="form-label-td" style="width: 50%; text-align: left;">
                <asp:Label ID="lblCustomerID" runat="server" Text="CustomerID"></asp:Label>
             </td>
         </tr>
         <tr>
            <td class="auto-style57" style="text-align: right;">Email ID:</td>
            <td class="form-label-td" style="width: 50%; text-align: left;">
                <asp:Label ID="lblEmailID" runat="server" Text="EmailID"></asp:Label>
             </td>
         </tr>--%>
         <%--<asp:Panel ID="pnlDates" runat="server" Visible="false">
             <tr>
                <td class="form-label-td" style="width: 50%; text-align: right;">&nbsp;&nbsp;                    
                </td>
                <td class="form-label-td" style="width: 50%; text-align: left;">Order Date&nbsp;:&nbsp;
                    <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form-field" Text="" MaxLength="12" Width="120px"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td class="form-label-td" style="width: 50%; text-align: right;">Invoice Date&nbsp;:&nbsp;
                    <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-field" Text="" MaxLength="12" Width="120px"></asp:TextBox>
                </td>
                <td class="form-label-td" style="width: 50%; text-align: left;">Shipping Date&nbsp;:&nbsp;
                    <asp:TextBox ID="txtActualShippingDate" runat="server" CssClass="form-field" Text="" MaxLength="12" Width="120px"></asp:TextBox>
                </td>
            </tr>
        </asp:Panel>--%>
        
     </table>
     
   </div>
    <br />
   
  <asp:UpdatePanel ID="pnlResults" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
  <ContentTemplate>

  <div style="height: 100%; text-align: center;">
    <table class="form-table" cellspacing="0" summary="">
        <tr>
            <td class="form-submit-td" style="text-align: center">
                <asp:Panel ID="Panel1" runat="server" style="text-align: center">
                    <table id="tblNewCustomer" cellspacing="0" class="auto-style41" summary="" visible="false">
                           <tr>
                               <td class="auto-style70">Internal User</td>
                                  <td class="auto-style70">                                      
                                <div  class="slideThree">
                                       <asp:CheckBox ID="chkUserType" runat="server" Text="" AutoPostBack="true" />
                                        <label for="chkUserType"></label>    
                                   </div>                                                             
                            </td>
                              <%-- <td class="auto-style66">Customer Name </td>
                               <td class="auto-style75">
                                   <asp:TextBox ID="txtEmail" runat="server" CssClass="form-field" Height="27px" MaxLength="255" Width="157px" onpaste="return false" OnDrop="return false;"  />
                               </td>
                               <td class="auto-style13">
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEmail" Display="Dynamic" EnableClientScript="false" ErrorMessage="E-mail Required" ForeColor="Red" ValidationGroup="frmValidation" />
                                   <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmail" Display="Dynamic" EnableClientScript="false" ErrorMessage="Invalid email." ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="frmValidation"/>
                               </td>--%>
                               <td class="auto-style75"></td>
                                <td class="auto-style66">Customer Name</td>
                               <td class="auto-style75">
                                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-field"></asp:TextBox>
                                <asp:HiddenField ID="hfCustomerID" runat="server" Value="" />
                                <asp:HiddenField ID="hfUserID" runat="server" Value="" />                               </td>
                               <td class="auto-style13">                               
                              <asp:ImageButton ID="imgButton" runat="server" ImageUrl="images/download.jpg" Height="20px" Width="20px" />
                               </td>

                               
                               
                        </tr>
                           <tr>
                               <td class="auto-style70">Login Name </td>
                               <td class="auto-style50">
                                   <asp:TextBox ID="txtUsername" runat="server" CssClass="form-field" Height="25px" MaxLength="30" Width="158px" onpaste="return false" OnDrop="return false;"  />
                               </td>
                               <td class="auto-style75">
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUsername" EnableClientScript="false" ErrorMessage="Login Name Required" ForeColor="Red" ValidationGroup="frmValidation" />
                               </td>
                            <td class="auto-style70">First Name</td>
                            <td class="auto-style50">
                                <asp:TextBox ID="txtFirstName" runat="server" Height="25px"  Width="158px" MaxLength="50" CssClass="form-field" onpaste="return false" OnDrop="return false;" />
                            </td>
                            <td class="auto-style75">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFirstName"
                                     EnableClientScript="false" ErrorMessage="First Name Required" ForeColor="Red" ValidationGroup="frmValidation"/>
                            </td>
                           
                         </tr>
                        <tr>
                             <td class="auto-style70">Last Name</td>
                            <td class="auto-style50">
                                <asp:TextBox ID="txtLastName" runat="server" Height="28px"  Width="158px" MaxLength="50" CssClass="form-field" onpaste="return false" OnDrop="return false;"  />
                            </td>                      
                             <td class="auto-style43"></td>

                            <td class="auto-style70">Password </td>
                            <td class="auto-style50">
                                <asp:TextBox ID="txtPassword" runat="server" Height="25px"  Width="158px" MaxLength="40" CssClass="form-field" onpaste="return false" OnDrop="return false;"  />
                            </td>
                            <td class="auto-style75">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                                     EnableClientScript="false" ErrorMessage="Password Required" ForeColor="Red" ValidationGroup="frmValidation"/>
                            </td>
                            
                         </tr>
                            
                          <tr>
                              <td class="auto-style70">Confirm Password</td>
                            <td class="auto-style50">
                                <asp:TextBox ID="txtConfirmPassword" runat="server" Height="28px" Width="158px" MaxLength="40" CssClass="form-field" onpaste="return false" OnDrop="return false;" />
                            </td>
                            <td class="auto-style43">
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" 
                                    EnableClientScript="false" ErrorMessage="Passwords do not match." ForeColor="Red" ValidationGroup="frmValidation" />
                            </td>
                            <td class="auto-style70">Is Active </td>                       
                               <td class="auto-style70">
                                    <div class="slideThree">     
                                       <asp:CheckBox ID="chkContinued" runat="server" Text="" Checked="true" />
                                        <label for="chkContinued"></label>
                                   </div>
                              </td>             
                         </tr> 
                        <tr>
                            <td class="auto-style70">Administrator</td>
                              <td class="auto-style70">                                      
                                <div  class="slideThree">
                                       <asp:CheckBox ID="chkAdmin" runat="server" Text="" AutoPostBack="true" />
                                        <label for="chkAdmin"></label>    
                                   </div>                                                             
                            </td>
                            <td class="auto-style70">&nbsp;</td>
                              <td class="auto-style70">                                      
                                  Product</td>
                            <td class="auto-style70">&nbsp; Plantation<div  class="slideThree">
                                       <asp:CheckBox ID="chkPlantation" runat="server" Text="" AutoPostBack="true" />
                                 <label for="chkPlantation"></label>    
                                    
                                   </div>      
                                   <td class="auto-style70">Louvre<div class="slideThree">
                                       <asp:CheckBox ID="chkLouvre" runat="server" AutoPostBack="true" Text="" />
                                          <label for="chkLouvre"></label>    
                                       </div>   
                            </td>
                                 <td class="auto-style75"></td>

                        </tr>

                              <tr>
                                <td class="auto-style62" colspan="6" style="text-align: center;">
                                    <asp:Button ID="btnAddLogin" runat="server" CssClass="form-button_NoPadding" Text="Add User" style="width: 130px; height: 35px;" CausesValidation="true" ValidationGroup="frmValidation" OnClientClick="return Validation();"/>
                                    &nbsp;<img id="imgLoading2" src="images/indicator.gif" width="16px" height="16px" alt="loading" style="visibility: hidden; vertical-align: middle;" />
                                    <asp:Button ID="btnCancelLogin" runat="server" CssClass="form-button_NoPadding"  Text="Reset" style="width: 130px; height: 35px;"/>
                                    &nbsp;&nbsp;
                                     <%--<asp:Button ID="btnSearch" runat="server" CssClass="form-button_NoPadding"  Text="Search" style="width: 130px; height: 35px;"/>--%>
                                    <asp:TextBox ID="txtGridrowindex" runat="server" TabIndex="-1" CssClass="HideElement" Text="" Visible="false"></asp:TextBox>
                                </td>
                            </tr>

                        

                            <tr>
                                <td colspan="6">
                                    <div style="text-align: center;">
                                        &nbsp;</div>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                       
                    </table>
                </asp:Panel>
                <asp:Label ID="lblStatus" runat="server" Text="" style="font-size:large;"  ForeColor="Red"></asp:Label>
            </td>
        </tr>
     </table>
  </div>

  <div class="form">              
                <asp:Panel runat="server" ID="pnlList" Visible="True">
                    <asp:GridView ID="dgvCustomerLogin" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID" ForeColor="#333333" Height="177px" Width="1438px" AllowPaging="true"
                        PageSize="10">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
                        <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                       <%-- <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />--%>
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="Green" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Height="26px"/>
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnViewDetail" runat="server" Text="Edit" CommandName="LoginDetail"  width="70px"
                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CssClass="form-button_NoPadding" style="width: 90px; height: 35px;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Font-Bold="false" DataField="CustomerName" HeaderText="Customer Name" SortExpression="CustomerName"/>
                                        <asp:BoundField HeaderStyle-Font-Bold="false" DataField="UserName" HeaderText="User Name" SortExpression="LoginName"/>
                                        <asp:BoundField HeaderStyle-Font-Bold="false" DataField="UserFirstName" HeaderText="First Name" SortExpression="FirstName"/>
                                        <asp:BoundField HeaderStyle-Font-Bold="false" DataField="UserLastName" HeaderText="Last Name" SortExpression="LastName"/>                                        
                                                                               
                               <asp:TemplateField HeaderText="Is Active" HeaderStyle-Font-Bold="false">
                                <ItemTemplate>
                                        <div class="roundedOne">
                                            <asp:CheckBox ID="chkGridContinued" runat="server" checked='<%#Eval("Discontinued")%>' />
                                            <label for="chkGridContinued"></label>
                                          </div>                                                                         
                                </ItemTemplate>
                               </asp:TemplateField>

                              <asp:TemplateField HeaderText="Administrator" HeaderStyle-Font-Bold="false">
                                <ItemTemplate>
                                  <div class="roundedOne">     
                                         <asp:CheckBox ID="chkGridAdmin" runat="server" checked='<%#Eval("IsAdmin") %>' />
                                       <%--<asp:CheckBox ID="CheckBox1" runat="server" Checked="" Text='<%#Convert.ToBoolean(Eval("staff"))' />--%>
                                      
                                          <label for="chkGridAdmin"></label>                                                
                                  </div>         
                                </ItemTemplate>
                            </asp:TemplateField>                                                                      
                        </Columns>
                    </asp:GridView>
                </asp:Panel>	   
</div>

     </ContentTemplate>


    </asp:UpdatePanel>

    <br />
  
        <%--<div runat="server" id="DivCur2" style="display:none;">
        txtCurState: txtCurState:<asp:TextBox ID="txtCurState" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
        SiteID:<asp:TextBox ID="txtSiteID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
             SiteID:<asp:TextBox ID="txtCurSiteID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
                    <br />
                    UserID:<asp:TextBox ID="txtCurUserID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
                    <br />
                    UserName:<asp:TextBox ID="txtCurUserName" runat="server" TabIndex="-1" Text=""></asp:TextBox>
                    <br />
                    CustomerID:<asp:TextBox ID="txtCustID" runat="server" TabIndex="-1" Text=""></asp:TextBox>
                    <br />
                    Login Add:<asp:TextBox ID="txtUpdateCheck" runat="server" TabIndex="-1" Text=""></asp:TextBox>
                    <br />
                  

    

    </div> --%>  

        <div id="dialog-alert" title="Insufficient Information">
        <p id="dialog-alert-message" style="text-align: left;"></p>
    </div>
    <div id="dialog-confirm" title="Please Confirm" >
        <p id="dialog-confirm-message" style="text-align: left;"></p>
    </div>
       

    </div>  
     </form>
  
</body>

</html>

<script type="text/javascript">

    $(document).ready(function () {
        SearchCustomer();
        
       

    });
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (prm != null) {
        prm.add_endRequest(function (sender, e) {
            if (sender._postBackSettings.panelsToUpdate != null) {
                SearchCustomer();
            }
        });
    };

    function SearchCustomer() {



        $("#txtCustomerName")
            .autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '/CreateLogin_Shutter.aspx/AutoSearchCustomerName',
                        data: "{'text':'" + $.trim(request.term) + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            data.d = JSON.parse(data.d);
                            response($.map(data.d, function (item) {

                                return {
                                    label: item.CustomerName,
                                    val: item.CustomerID
                                }
                            }));
                            $("#ui-id-1").css("z-index", "1999");
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Server is busy now.Please try after some time.");
                        }
                    });
                },
                autoFocus: false,
                minLength: 1,
                select: function (event, ui) {

                    $("#txtCustomerName").val(ui.item.label);
                    $("#hfCustomerID").val(ui.item.val);
                }
            });
    }



</script>

