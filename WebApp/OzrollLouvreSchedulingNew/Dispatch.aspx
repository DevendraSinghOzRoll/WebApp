<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Dispatch.aspx.vb" Inherits="Dispatch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxcontroltoolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Daily Production Schedule Update</title>
        
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta name="format-detection" content="telephone=no" />
    
    <meta name="viewport" content="width=1200">
    <link href="stylesheets/checkBox_Yellow.css" rel="stylesheet" />
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
                $("#dialog-confirm-cancel").dialog({
                    modal: true,
                    autoOpen: false,
                    draggable: false,
                    buttons: {
                        "Yes": function () {
                            $(this).dialog("close");
                            __doPostBack('<%=btnCancel.ClientID%>', '');
                            return true;
                        },
                        "No": function () {
                            $(this).dialog("close");
                            document.getElementById("<%=btnCancel.ClientID%>").disabled = false;
                            document.getElementById("<%=btnCancel.ClientID%>").className = "form-button";
                            document.getElementById("<%=btnCancel.ClientID%>").value = "Cancel";
                            document.getElementById("<%=btnSave.ClientID%>").disabled = false;
                            document.getElementById("<%=btnSave.ClientID%>").className = "form-button";
                            document.getElementById("<%=btnSave.ClientID%>").value = "Save";
                            document.getElementById("imgLoading2").style.visibility = "hidden";
                            return false;
                        }
                    }
                });
            });

            function ConfirmLeave() {
                document.getElementById("dialog-confirm-message").innerHTML = "Are you sure you want to logout?";
                $("#dialog-confirm").dialog("open");
                return false;
            }

            function saveLocation() {
                localStorage.setItem('sTopLevelPage', window.location);
            }


            function cancelchanges() {
                var strMessage = "Are you sure you want to discard changes?";
                document.getElementById("<%=lblStatus.ClientID %>").innerHTML = "";
                document.getElementById("<%=btnCancel.ClientID%>").disabled = true;
                document.getElementById("<%=btnCancel.ClientID%>").className = "form-button-disabled";
                document.getElementById("<%=btnCancel.ClientID%>").value = "Submitting";
                document.getElementById("<%=btnSave.ClientID%>").disabled = true;
                document.getElementById("<%=btnSave.ClientID%>").className = "form-button-disabled";
                document.getElementById("<%=btnSave.ClientID%>").value = "Save";
                document.getElementById("imgLoading2").style.visibility = "visible";
                document.getElementById("dialog-confirm-cancel-message").innerHTML = strMessage;
                $("#dialog-confirm-cancel").dialog("open");
                return false;
            }

            function playSound() {
                var snd = new Audio("data:audio/wav;base64,//uQRAAAAWMSLwUIYAAsYkXgoQwAEaYLWfkWgAI0wWs/ItAAAGDgYtAgAyN+QWaAAihwMWm4G8QQRDiMcCBcH3Cc+CDv/7xA4Tvh9Rz/y8QADBwMWgQAZG/ILNAARQ4GLTcDeIIIhxGOBAuD7hOfBB3/94gcJ3w+o5/5eIAIAAAVwWgQAVQ2ORaIQwEMAJiDg95G4nQL7mQVWI6GwRcfsZAcsKkJvxgxEjzFUgfHoSQ9Qq7KNwqHwuB13MA4a1q/DmBrHgPcmjiGoh//EwC5nGPEmS4RcfkVKOhJf+WOgoxJclFz3kgn//dBA+ya1GhurNn8zb//9NNutNuhz31f////9vt///z+IdAEAAAK4LQIAKobHItEIYCGAExBwe8jcToF9zIKrEdDYIuP2MgOWFSE34wYiR5iqQPj0JIeoVdlG4VD4XA67mAcNa1fhzA1jwHuTRxDUQ//iYBczjHiTJcIuPyKlHQkv/LHQUYkuSi57yQT//uggfZNajQ3Vmz+Zt//+mm3Wm3Q576v////+32///5/EOgAAADVghQAAAAA//uQZAUAB1WI0PZugAAAAAoQwAAAEk3nRd2qAAAAACiDgAAAAAAABCqEEQRLCgwpBGMlJkIz8jKhGvj4k6jzRnqasNKIeoh5gI7BJaC1A1AoNBjJgbyApVS4IDlZgDU5WUAxEKDNmmALHzZp0Fkz1FMTmGFl1FMEyodIavcCAUHDWrKAIA4aa2oCgILEBupZgHvAhEBcZ6joQBxS76AgccrFlczBvKLC0QI2cBoCFvfTDAo7eoOQInqDPBtvrDEZBNYN5xwNwxQRfw8ZQ5wQVLvO8OYU+mHvFLlDh05Mdg7BT6YrRPpCBznMB2r//xKJjyyOh+cImr2/4doscwD6neZjuZR4AgAABYAAAABy1xcdQtxYBYYZdifkUDgzzXaXn98Z0oi9ILU5mBjFANmRwlVJ3/6jYDAmxaiDG3/6xjQQCCKkRb/6kg/wW+kSJ5//rLobkLSiKmqP/0ikJuDaSaSf/6JiLYLEYnW/+kXg1WRVJL/9EmQ1YZIsv/6Qzwy5qk7/+tEU0nkls3/zIUMPKNX/6yZLf+kFgAfgGyLFAUwY//uQZAUABcd5UiNPVXAAAApAAAAAE0VZQKw9ISAAACgAAAAAVQIygIElVrFkBS+Jhi+EAuu+lKAkYUEIsmEAEoMeDmCETMvfSHTGkF5RWH7kz/ESHWPAq/kcCRhqBtMdokPdM7vil7RG98A2sc7zO6ZvTdM7pmOUAZTnJW+NXxqmd41dqJ6mLTXxrPpnV8avaIf5SvL7pndPvPpndJR9Kuu8fePvuiuhorgWjp7Mf/PRjxcFCPDkW31srioCExivv9lcwKEaHsf/7ow2Fl1T/9RkXgEhYElAoCLFtMArxwivDJJ+bR1HTKJdlEoTELCIqgEwVGSQ+hIm0NbK8WXcTEI0UPoa2NbG4y2K00JEWbZavJXkYaqo9CRHS55FcZTjKEk3NKoCYUnSQ0rWxrZbFKbKIhOKPZe1cJKzZSaQrIyULHDZmV5K4xySsDRKWOruanGtjLJXFEmwaIbDLX0hIPBUQPVFVkQkDoUNfSoDgQGKPekoxeGzA4DUvnn4bxzcZrtJyipKfPNy5w+9lnXwgqsiyHNeSVpemw4bWb9psYeq//uQZBoABQt4yMVxYAIAAAkQoAAAHvYpL5m6AAgAACXDAAAAD59jblTirQe9upFsmZbpMudy7Lz1X1DYsxOOSWpfPqNX2WqktK0DMvuGwlbNj44TleLPQ+Gsfb+GOWOKJoIrWb3cIMeeON6lz2umTqMXV8Mj30yWPpjoSa9ujK8SyeJP5y5mOW1D6hvLepeveEAEDo0mgCRClOEgANv3B9a6fikgUSu/DmAMATrGx7nng5p5iimPNZsfQLYB2sDLIkzRKZOHGAaUyDcpFBSLG9MCQALgAIgQs2YunOszLSAyQYPVC2YdGGeHD2dTdJk1pAHGAWDjnkcLKFymS3RQZTInzySoBwMG0QueC3gMsCEYxUqlrcxK6k1LQQcsmyYeQPdC2YfuGPASCBkcVMQQqpVJshui1tkXQJQV0OXGAZMXSOEEBRirXbVRQW7ugq7IM7rPWSZyDlM3IuNEkxzCOJ0ny2ThNkyRai1b6ev//3dzNGzNb//4uAvHT5sURcZCFcuKLhOFs8mLAAEAt4UWAAIABAAAAAB4qbHo0tIjVkUU//uQZAwABfSFz3ZqQAAAAAngwAAAE1HjMp2qAAAAACZDgAAAD5UkTE1UgZEUExqYynN1qZvqIOREEFmBcJQkwdxiFtw0qEOkGYfRDifBui9MQg4QAHAqWtAWHoCxu1Yf4VfWLPIM2mHDFsbQEVGwyqQoQcwnfHeIkNt9YnkiaS1oizycqJrx4KOQjahZxWbcZgztj2c49nKmkId44S71j0c8eV9yDK6uPRzx5X18eDvjvQ6yKo9ZSS6l//8elePK/Lf//IInrOF/FvDoADYAGBMGb7FtErm5MXMlmPAJQVgWta7Zx2go+8xJ0UiCb8LHHdftWyLJE0QIAIsI+UbXu67dZMjmgDGCGl1H+vpF4NSDckSIkk7Vd+sxEhBQMRU8j/12UIRhzSaUdQ+rQU5kGeFxm+hb1oh6pWWmv3uvmReDl0UnvtapVaIzo1jZbf/pD6ElLqSX+rUmOQNpJFa/r+sa4e/pBlAABoAAAAA3CUgShLdGIxsY7AUABPRrgCABdDuQ5GC7DqPQCgbbJUAoRSUj+NIEig0YfyWUho1VBBBA//uQZB4ABZx5zfMakeAAAAmwAAAAF5F3P0w9GtAAACfAAAAAwLhMDmAYWMgVEG1U0FIGCBgXBXAtfMH10000EEEEEECUBYln03TTTdNBDZopopYvrTTdNa325mImNg3TTPV9q3pmY0xoO6bv3r00y+IDGid/9aaaZTGMuj9mpu9Mpio1dXrr5HERTZSmqU36A3CumzN/9Robv/Xx4v9ijkSRSNLQhAWumap82WRSBUqXStV/YcS+XVLnSS+WLDroqArFkMEsAS+eWmrUzrO0oEmE40RlMZ5+ODIkAyKAGUwZ3mVKmcamcJnMW26MRPgUw6j+LkhyHGVGYjSUUKNpuJUQoOIAyDvEyG8S5yfK6dhZc0Tx1KI/gviKL6qvvFs1+bWtaz58uUNnryq6kt5RzOCkPWlVqVX2a/EEBUdU1KrXLf40GoiiFXK///qpoiDXrOgqDR38JB0bw7SoL+ZB9o1RCkQjQ2CBYZKd/+VJxZRRZlqSkKiws0WFxUyCwsKiMy7hUVFhIaCrNQsKkTIsLivwKKigsj8XYlwt/WKi2N4d//uQRCSAAjURNIHpMZBGYiaQPSYyAAABLAAAAAAAACWAAAAApUF/Mg+0aohSIRobBAsMlO//Kk4soosy1JSFRYWaLC4qZBYWFRGZdwqKiwkNBVmoWFSJkWFxX4FFRQWR+LsS4W/rFRb/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////VEFHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAU291bmRib3kuZGUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMjAwNGh0dHA6Ly93d3cuc291bmRib3kuZGUAAAAAAAAAACU=");
                snd.play();              
            }

            function IsNumericKey(evt, el,DecimalPlace,CommaNeeded) {            
                var number = el.value.split('.');
                var charCode;
                if (evt.key.match(/[-.0-9]/)) {
                    charCode = evt.key.charCodeAt(0);
                } else {
                    if (evt.key == "Backspace")
                    { charCode = 8; }
                    else if (evt.key == "Delete")
                    { charCode = 127; }                                     
                    else
                    { charCode = (evt.which) ? evt.which : event.keyCode; }
                }               
                
                if (evt.key == "Backspace" || evt.key == "Delete" || evt.key == "ArrowUp" || evt.key == "ArrowDown" ||
                    evt.key == "ArrowLeft" || evt.key == "ArrowRight" || evt.key == "Home" || evt.key == "End")
                {
                    return true;
                }

                if (charCode != 46 && charCode != 190 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                    playSound();
                    return false;
                }
          
                // dot = 46
                if ((number.length > 1 && charCode == 46) || (charCode == 46 && DecimalPlace == 0)) {
                    playSound();
                    return false;
                }
                //get the carat position
                var caratPos = getSelectionStart(el);
                var dotPos = el.value.indexOf(".");
                if (typeof DecimalPlace !== 'undefined') {
                    if (DecimalPlace > -1) {
                        if (caratPos > dotPos && dotPos > -1 && (number[1].length > DecimalPlace - 1)) {
                            playSound();
                            return false;
                        }
                    }
                }
                //For adding Commas to the value
                if (typeof CommaNeeded !== 'undefined') {
                    CommaNeeded = CommaNeeded.toLowerCase();
                    if (CommaNeeded = "true")
                    { el.value = replaceCommas(el.value); }
                }

                return true;
            }

            function getSelectionStart(o) {
                if (o.createTextRange) {
                    var r = document.selection.createRange().duplicate()
                    r.moveEnd('character', o.value.length)
                    if (r.text == '') return o.value.length
                    return o.value.lastIndexOf(r.text)
                } else return o.selectionStart
            }
            function replaceCommas(yourNumber) {
                var components = yourNumber.toString().split(".");
                if (components.length === 1)
                    components[0] = yourNumber;
                components[0] = components[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                if (components.length === 2)
                    components[1] = components[1].replace(/\D/g, "");
                return components.join(".");
            }
          
    </script>
    <style type="text/css">
        .form-Field-process {
         background-color: #F2F2F2;        
         padding: 5px 10px 5px 10px;
         text-align: left;
         vertical-align: middle;
        width: 35%;
       }
        </style>
</head>
<body onload="saveLocation();">
    <form runat="server" id="form1" method="post">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    <Scripts>
    <asp:ScriptReference Path="javascript/fixfocus.js" />
    </Scripts>
    </asp:ScriptManager>
    <div id="middle-container-diallerview" >

        <div id="logo" style="height: 100px; text-align: center;" ></div>
        <div style="height: 50px; text-align: center;">
            <asp:Button ID="btnHome" runat="server" CssClass="form-button" Text="Home" UseSubmitBehavior="false" />
            <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="javascript:return ConfirmLeave();" UseSubmitBehavior="false" />
	    </div>
    
        <h1>Dispatch</h1>

            <h2><asp:Label ID="lblProdSchedID" runat="server"></asp:Label></h2>

        <div class="form" style="text-align: center;">

        <div style="text-align: center;">
            <div class="form" style="text-align: center;">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table cellspacing="0" class="form-table" summary="">
                            <tr>
                                <td class="form-label-td" style="width: 20%;text-align:right;">Customer: </td>
                                        
                                 <td class="form-label-td" style="width: 20%">
                                     <asp:Label ID="lblCustomer" runat="server"></asp:Label>                                     
                                </td>                               
                                
                                <td class="form-label-td" style="width: 20%;text-align:right;">Invoice: </td>
                                        
                                 <td class="form-label-td" colspan="2">
                                     <asp:Label ID="lblInvoice" runat="server"></asp:Label>               
                                </td>                               
                                  
                             </tr>
                            <tr>
                                <td class="form-label-td" style="width: 20%;text-align:right;">Dispatch To: </td>
                                        
                                 <td class="form-label-td" style="width: 20%">
                                     <asp:Label ID="lblTo" runat="server"></asp:Label>                                     
                                </td>                               
                                
                                <td class="form-label-td" style="width: 20%;text-align:right;">Dispatch Local: </td>
                                        
                                 <td class="form-label-td" style="width: 20%">
                                     <div class="slideTwo">     
                                                 <asp:CheckBox ID="chkLocal" runat="server" Text="" Checked="False" AutoPostBack="True" />
                                       <label for="chkLocal"></label>                                        
                                     </div>                  
                                </td>                               
                                  <td class="form-label-td" style="width: 15%;text-align:left;">
                                         <asp:Label ID="lblCheckLocal" runat="server" Text="Local Dispatch" Font-Bold="True" Font-Size="Medium" ForeColor="#FB9E25"></asp:Label>
                                  </td>
                             </tr>
                               <tr>
                                <td class="form-label-td" style="width: 20%;text-align:right;">Carrier: </td>
                                        
                                 <td class="form-label-td" style="width: 20%">
                                     <asp:Label ID="lblCarrier" runat="server"></asp:Label>                                     
                                </td>                               
                                
                                <td class="form-label-td" style="width: 20%;text-align:right;">Transport Type: </td>
                                        
                                 <td class="form-label-td" colspan="2">
                                     <asp:Label ID="lblTransType" runat="server"></asp:Label>               
                                </td>                               
                                  
                             </tr>
                             <tr>
                                <td class="form-label-td" style="width: 20%;text-align:right;">Freight: </td>                                        
                                 <td class="form-label-td" style="width: 20%">
                                       <asp:Label ID="lblHeight" runat="server"></asp:Label>
                                </td>       
                                <td class="form-label-td" style="width: 20%;text-align:right;">Insurance: </td>
                                        
                                 <td class="form-label-td" colspan="2">
                                     <asp:Label ID="lblInsurance" runat="server"></asp:Label>               
                                </td>                             
                             </tr>      
                             <tr>
                                <td class="form-label-td" style="width: 20%;text-align:right;">Delivery Date: </td>                                        
                                 <td class="form-label-td" style="width: 20%">
                                     <asp:TextBox ID="txtDelDate" onkeyDown="return false" runat="server"></asp:TextBox>
                                </td>       
                                <td class="form-label-td" style="width: 20%;text-align:right;">No of Packets:</td>
                                        
                                 <td class="form-label-td" colspan="2">
                                    <asp:TextBox ID="txtPacketNos" onkeyDown="return IsNumericKey(event, this,0)" runat="server"></asp:TextBox>       
                                </td>                             
                             </tr>  
                                            <tr>
                             <td class="form-label-td" style="width: 20%;text-align:right;">Total Weight: </td>                                        
                                 <td class="form-label-td" style="width: 20%">
                                     <asp:TextBox ID="txtWeight" onkeyDown="return IsNumericKey(event, this,2,'True')" runat="server"></asp:TextBox>
                                </td>       
                                <td class="form-label-td" style="width: 20%;text-align:right;"></td>
                                        
                                 <td class="form-label-td" colspan="2">
                                       
                                </td>                             
                             </tr>         
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
            </div>
            <br />
            <br />
        </div>

                            <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" OnClientClick="return cancelchanges();" UseSubmitBehavior="false" />
                            <asp:Button ID="btnSave" runat="server" CssClass="form-button" Text="Save" UseSubmitBehavior="false" />

        <br />
        <div style="text-align: center;">          
           
        </div>

             <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Size="X-Large" />

        <br />


        </div>
    </div>

              <asp:Panel ID="pnlAddStock" runat="server" CssClass="modalPopup" Style="display: none; left: 35%;top: 45%;width: 30% !important;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="form-table" cellspacing="0" summary="">
                                <tr>
                                    <td class="form-field-td-p2">Stock Item&nbsp;:&nbsp;
                                    </td>
                                    <td class="form-field-td-p2">
                                        <asp:DropDownList ID="cboStockItem" runat="server" Width="120%" CssClass="form-select"></asp:DropDownList>
                                    </td>
                                    <td class="form-field-td-p2"></td>
                                    <td class="form-field-td-p2"></td>
                                </tr>
                                <tr>
                                    <td class="form-field-td-p2">Quantity To Book Out&nbsp;:&nbsp;
                                    </td>
                                    <td class="form-field-td-p2">
                                        <asp:TextBox ID="txtQuantityBookOut" Width="50%" onkeydown="return IsNumericKey(event,this,2)" runat="server" CssClass="form-field"></asp:TextBox>
                                    </td>
                                    <td class="form-field-td-p2"></td>
                                    <td class="form-field-td-p2"></td>
                                </tr>
                                <tr>
                                    <td class="form-submit-td" style="width: 50%">
                                        <div style="float: right;">
                                            <asp:Button runat="server" ID="btnCancelAddStock" Text="Cancel" CssClass="form-button" />
                                        </div>
                                    </td>
                                    <td class="form-submit-td">
                                        <div style="float: left;">
                                            <asp:Button runat="server" ID="btnSaveAddStock" Text="Save" CssClass="form-button" />
                                        </div>
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>

                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSaveAddStock" />
                            <asp:PostBackTrigger ControlID="btnCancelAddStock" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>


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
        ProductTypeID<asp:TextBox id="txtProductTypeID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
        Job No<asp:TextBox id="txtJobNo" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
    </div>


   <script type="text/javascript">
            $(function () {
                $("input#txtDelDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1d', maxDate: '+5w' });                
            });
        </script>



    </form>
</body>
</html>
