<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LouvreJobDetails.aspx.vb" Inherits="LouvreJobDetails" Theme="SknGridView"  EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="ext" Namespace="Extensions" %>
<%@ Register Src="~/UserControls/PrivacyScreenDetails.ascx" TagPrefix="uc1" TagName="PrivacyScreenDetails" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server" >
    <title>OzRoll Louvre Job Details</title>
        
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta name="format-detection" content="telephone=no" />
    
    <link href="stylesheets/reset.css" rel="stylesheet" type="text/css" media="screen" />
    <!--[if IE]><link href="stylesheets/explorer.css" rel="stylesheet" type="text/css" media="screen" /><![endif]-->
    <link href="stylesheets/style.css?version=<%# Now.Year() & Now.Month & Now.Day() %>" rel="stylesheet" type="text/css" media="screen" />


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
                        __doPostBack('<%#btnLogout.ClientID %>', '');
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
                        __doPostBack('<%#btnCancel.ClientID%>', '');
                        return true;
                    },
                    "No": function () {
                        $(this).dialog("close");
                        return false;
                    }
                }
            });

            $("#dialog-saved").dialog({
                modal: true,
                autoOpen: false,
                draggable: false,
                buttons: {
                    "OK": function () {
                        $(this).dialog("close");
                        return true;
                    }
                }
            });
        });

        $("[id$=txtActualShippingDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
        $("[id$=txtPlannedShippingDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+52w' });
        $("[id$=txtOrderDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+1w' });
        $("[id$=txtReceived]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtScheduledDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtCuttingDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtPiningDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtPrepDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtAssemblyDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtHingingDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtPackupDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtFramingDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtQCDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtWrappingDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtPostProduction1]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtPostProduction2]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });

        $("[id$=txtCheckMeasureDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtPickingDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
        $("[id$=txtInstallDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });

        $("[id$=txtPCStartDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
        $("[id$=txtPCPickDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
        $("[id$=txtPCETADate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
        $("[id$=txtPCCompleteDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
        $("[id$=txtRequirementPickDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
        $("[id$=txtRequirementStartDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
        $("[id$=txtRequirementETADate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
        $("[id$=txtRequirementCompleteDate]").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });

        if (window.history.forward(1) != null)
            window.history.forward(1);

        function ConfirmLeave() {
            document.getElementById("dialog-confirm-message").innerHTML = "Are you sure you want to logout?";
            $("#dialog-confirm").dialog("open");
            return false;
        }

        function validfloat() {
            var bolValid = 1;
            var strMessage = "";
            document.getElementById("<%#txtSalePrice.ClientID%>").value = document.getElementById("<%#txtSalePrice.ClientID%>").value.trim();
            if (document.getElementById("<%#txtSalePrice.ClientID%>").value != "") {
                document.getElementById("<%#txtSalePrice.ClientID%>").value = document.getElementById("<%#txtSalePrice.ClientID%>").value.replace('$', '');
                document.getElementById("<%#txtSalePrice.ClientID%>").value = document.getElementById("<%#txtSalePrice.ClientID%>").value.replace(',', '');
                document.getElementById("<%#txtSalePrice.ClientID%>").value = document.getElementById("<%#txtSalePrice.ClientID%>").value.trim();
                if (isNaN(parseFloat(document.getElementById("<%#txtSalePrice.ClientID%>").value))) {
                    strMessage = "The Sales Price must be numeric.";
                    bolValid = 0;
                } else {
                    if (parseFloat(document.getElementById("<%#txtSalePrice.ClientID%>").value) != document.getElementById("<%#txtSalePrice.ClientID%>").value) {
                        strMessage = "The Sales Price must be numeric.";
                        bolValid = 0;
                    }
                }
                if (bolValid == 0){
                    document.getElementById("<%#txtSalePrice.ClientID%>").value = "";
                }
            }
            if (bolValid == 1) {
                if (document.getElementById("<%#txtTotalSQM.ClientID%>").value != "") {
                    if (isNaN(parseFloat(document.getElementById("<%#txtTotalSQM.ClientID%>").value))) {
                        strMessage = "The Total SQM. must be numeric.";
                        bolValid = 0;
                    } else {
                        if (parseFloat(document.getElementById("<%#txtTotalSQM.ClientID%>").value) != document.getElementById("<%#txtTotalSQM.ClientID%>").value) {
                            strMessage = "The Total SQM. must be numeric.";
                            bolValid = 0;
                        }
                    }
                    if (bolValid == 0) {
                        document.getElementById("<%#txtTotalSQM.ClientID%>").value = "";
                    }
                }
            }
            if (strMessage != "" && bolValid == 0) {
                document.getElementById("dialog-alert-message").innerHTML = strMessage;
                $("#dialog-alert").dialog("open");
            }
            return false;
        }

        function cancelchanges() {
            var strMessage = "Are you sure you want to discard any unsaved changes?";
            document.getElementById("<%#lblStatus.ClientID %>").innerHTML = "";
            document.getElementById("dialog-confirm-cancel-message").innerHTML = strMessage;
            $("#dialog-confirm-cancel").dialog("open");
            return false;
        }

        function ValidateAll(btnName) {
            var bolValid = 1;
            var strMessage = "";
            document.getElementById("<%#lblStatus.ClientID %>").innerHTML = "";
            var userType = document.getElementById("<%#hfUserType.ClientID%>").value
            
            if (bolValid == 1) {
                if (document.getElementById("<%#txtContractNumber.ClientID%>")) {
                    if (document.getElementById("<%#txtContractNumber.ClientID%>").value == "") {
                        strMessage = "Please enter the Contract Ref.";
                        bolValid = 0;
                    }
                }
            }
             // validating for internal user only. Append single condition "&& userType==0":By Surendra
            //commented by surendra 12 aug.
            <%--if (bolValid == 1 && userType == 0) {
                if (document.getElementById("<%#txtCustomerName.ClientID%>").value == "") {
                    strMessage = "Please enter the Customer Name.";
                    bolValid = 0;
                }
            }--%>
                
            //don't allow invoice date without sales price entered
            // validating for internal user only. Append single condition "&& userType==0":By Surendra
            if (bolValid == 1 && userType==0) {
                if (document.getElementById("<%#lblPostProduction1.ClientID%>").innerText == "Invoice Date") {
                    if (document.getElementById("<%#txtPostProduction1.ClientID%>").value !== "") {
                        if (document.getElementById("<%#txtSalePrice.ClientID%>").value == "") {
                            strMessage = "The Sales Price needs to be entered before Invoicing.";
                            bolValid = 0;
                        }
                    }
                }
            }
           // validating for internal user only. Append single condition "&& userType==0":By Surendra
            if (bolValid == 1 && userType == 0) {
                if (document.getElementById("<%#lblPostProduction2.ClientID%>").innerText == "Invoice Date") {
                    if (document.getElementById("<%#txtPostProduction2.ClientID%>").value !== "") {
                        if (document.getElementById("<%#txtSalePrice.ClientID%>").value == "") {
                            strMessage = "The Sales Price needs to be entered before Invoicing.";
                            bolValid = 0;
                        }
                    }
                }
            }
            // Added By Pradeep Singh against ticket #72024
            
            var Price = document.getElementById("<%#txtSalePrice.ClientID%>").value
            Price = Price.replace(',', '');
            
            var gridView = document.getElementById('<%#dgvDetails.ClientID%>');
            if (Price[0] == "$") {
                Price = Price.substring(1);
            }
            Price = parseFloat(Price);
            
            if (bolValid == 1 && btnName == "btnSave" && gridView != null) {               

                var OrderType = document.getElementById("<%#cboOrderType.ClientID%>").value;
               
                if (Price < 800.0 && OrderType==1) {
                    
                    var strMessage = "Order price should not be less than $800 due to minimum order capping";
                    document.getElementById("dialog-confirm-delete-message").innerHTML = strMessage;
                    $("#dialog-confirm-copy").dialog("open");
                    $("#dialog-confirm-delete").dialog({
                        modal: true,
                        autoOpen: false,
                        draggable: false,
                        buttons: {
                            "OK": function () {
                                $(this).dialog("close");
                                __doPostBack(btnName, '');
                            }
                        }
                    }).dialog("open");
                } else {
                    var strMessage = "";
                }
               
            }
            
            if (bolValid == 1 && (Price >= 800.0 || gridView == null || OrderType != 1)) {
                __doPostBack(btnName, '');
                return true;
            }
            else if (bolValid != 1 && (Price >= 800.0 || gridView == null)) {
                
                if (strMessage != "") {
                    document.getElementById("dialog-alert-message").innerHTML = strMessage;
                    $("#dialog-alert").dialog("open");
                }
                return false;
            }
        }

        function checkDates(datetextbox1, datetextbox2) {
            var date1 = new date(datetime.parse(datetextbox1.value));
            var date2 = new date(datetime.parse(datetextbox2.value));

            var intDays = dayDiff(date1, date2);

            if (intDays < 0) {
                datetextbox2.value == '';
                return false;
            }
            else {
                return true;
            }              
        }

        function dayDiff(startdate, enddate) {
            var dayCount = 0;

            while (enddate >= startdate) {
                dayCount++;
                startdate.setDate(startdate.getDate() + 1);
            }

            return dayCount;
        }

        function PostDelete(obj) {
            var strMessage = "Are you sure you want to delete?";
            document.getElementById("dialog-confirm-delete-message").innerHTML = strMessage;
            document.getElementById("<%#lblOrderForm.ClientID%>").value = "";
            if (document.getElementById("<%#lblOrderForm.ClientID%>") instanceof Object) {
                document.getElementById("<%#lblOrderForm.ClientID%>").value = "";
            }
            if (document.getElementById("<%#txtbuttonName.ClientID%>") instanceof Object) {
                document.getElementById("<%#txtbuttonName.ClientID%>").value = obj;
            }
            $("#dialog-confirm-delete").dialog("open");
         <%-- __doPostBack('<%#UploadButton2.ClientID%>', '');--%>
            return false;
        }

        var funcID;

        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(BeginRequestHandler);
            prm.add_endRequest(EndRequestHandler);

            function BeginRequestHandler(sender, args) {
                funcID = window.setTimeout(ShowHider, 100);
                $('body').on('scroll mousewheel touchmove', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    return false;
                });
            }

            function EndRequestHandler(sender, args) {
                window.clearTimeout(funcID);
                HideHider();
                $('body').off('scroll mousewheel touchmove');
            }
        });

        function HideHider() {
            $(".hider").hide();
        }

        function ShowHider() {
            $(".hider").show();
        }

        function generatePDF(docName) {
            switch (docName) {

                case 'emailquote':
                    setTimeout(emailQuotePDF, 1000);
                    break;

                case 'order':
                    setTimeout(generateOrderPDF, 1000);
                    break;

                case 'deliverydocket':
                    setTimeout(generateDeliveryDocketPDF, 1000);
                    break;

                case 'runningsheet':
                    setTimeout(generateRunningSheetPDF, 1000);
                    break;

                case 'timesheet':
                    setTimeout(generateTimeSheetPDF, 1000);
                    break;

                case 'coversheet':
                    setTimeout(generateCoverSheetPDF, 1000);
                    break;

                case 'productionsheet':
                    setTimeout(generateProductionSheetPDF, 1000);
                    break;
                //added by surendra 22-10-2020
                case 'optimisersheet':
                    setTimeout(generateOptimiserSheetPDF, 1000);
                    break;
                //Added by Pradeep Singh 05-10-2021
                case 'excelpowdercoat':
                    setTimeout(generateOptimiserSheetExcel, 1000);
                    break;

            }
        }

        function generateOrderPDF() {
            __doPostBack('<%#lnkGenerateOrderPDFDummy.ClientID%>', '');
        }

        function generateDeliveryDocketPDF() {
            __doPostBack('<%#lnkGenerateDeliveryDocketPDFDummy.ClientID%>', '');
        }

        function generateRunningSheetPDF() {
            __doPostBack('<%#lnkGenerateRunningSheetPDFDummy.ClientID%>', '');
        }

        function generateTimeSheetPDF() {
            __doPostBack('<%#lnkGenerateTimeSheetPDFDummy.ClientID%>', '');
        }

        function generateCoverSheetPDF() {
            __doPostBack('<%#lnkGenerateCoverSheetPDFDummy.ClientID%>', '');
        }

        function generateProductionSheetPDF() {
            __doPostBack('<%#lnkGenerateProductionSheetPDFDummy.ClientID%>', '');
        }

        function emailQuotePDF() {
            __doPostBack('<%#lnkEmailQuotePDFDummy.ClientID%>', '');
        }
        function generateOptimiserSheetPDF() {
            __doPostBack('<%#lnkGenerateOptimiserSheetPDFDummy.ClientID%>', '');
        }
        function generateOptimiserSheetExcel() {
            __doPostBack('<%#lnkGenerateOptimiserSheetExcelDummy.ClientID%>', '');
        }
        

        function AutoCompleteGetColourID(source, eventArgs) {
            var id = eventArgs.get_value();
            var name = eventArgs.get_text();

            // Remove the colour tag from the name
            name = name.replace('<%# SharedConstants.STR_STANDARD_COLOUR_TAG %>', '');
            name = name.replace('<%# SharedConstants.STR_PREMIUM_COLOUR_TAG %>', '');
            name = name.replace('<%# SharedConstants.STR_PRESTIGE_COLOUR_TAG %>', '');
           
            document.getElementById('<%#hdnColourID.ClientID %>').value = id;
            document.getElementById('<%#hdnColourName.ClientID %>').value = name;
            document.getElementById('<%#txtColour.ClientID %>').title = name;
        }

        function AutoCompleteGetColourIDExtra(source, eventArgs) {
            var id = eventArgs.get_value();
            var name = eventArgs.get_text();

            var hiddenfieldID = source.get_id().replace("aceColour", "hdnColourID");
            var hiddenfieldName = source.get_id().replace("aceColour", "hdnColourName");
            
            // Remove the colour tag from the name
            name = name.replace('<%# SharedConstants.STR_STANDARD_COLOUR_TAG %>', '');
            name = name.replace('<%# SharedConstants.STR_PREMIUM_COLOUR_TAG %>', '');
            name = name.replace('<%# SharedConstants.STR_PRESTIGE_COLOUR_TAG %>', '');
           
            document.getElementById(hiddenfieldID).value = id;
            document.getElementById(hiddenfieldName).value = name;
            source.title = name;
        }

        function AutoCompleteStart(sender, e) {
            sender._element.className = sender._element.className + ' auto-complete-loading';
        }

        function AutoCompleteEnd(sender, e) {
            var re = new RegExp(' auto-complete-loading', 'g');
            sender._element.className = sender._element.className.replace(re, '');
        }

        function AutoCompleteEnsureValues() {
            document.getElementById('<%#txtColour.ClientID %>').value = document.getElementById('<%#hdnColourName.ClientID %>').value;
        }

        function ajaxUploader_UploadComplete(sender, e) {
            if (sender._filesInQueue[sender._filesInQueue.length - 1]._isUploaded)
                // Do post back only after all files have been uploaded
                __doPostBack('btnCancelUploadFiles', '');
        }

    </script>

    <style type="text/css">

        .div-column {
            width: 29%; 
            float: left;
            padding: 20px;
            position: relative;
        }

        .valSummary {
            color: red;
            font-weight: bold;
        }

        .valSummary ul {
            display: none;
            visibility: hidden;
        }

        .extras-button {
            margin: 10px 2px 10px 2px;
            padding: 5px;
            width: 100px;
        }

        .extras-field {
            margin: 10px 2px 10px 2px !important;
            padding: 5px;
        }

        .extras-table {
            margin: 0 auto;
            margin-top: 10px;
            margin-bottom: 20px;
            border: solid grey 1px;
        }

        .details-button {
            margin: 10px 2px 10px 2px !important;
            padding: 5px;
        }

        .Grid {
            margin: 0 auto;
            margin-top: 10px;
            margin-bottom: 20px;
            border: solid grey 1px;
        }

        .extras-table tr:first-child th {
            padding:5px;
            font-weight: normal;
        }

        .extras-table tr td {
            padding:5px;
        }

        .form-select {
            margin-bottom: 2px;
        }

        .form-field {
            margin-bottom: 2px;
        }

        .hider {
            width: 100%;
            height: 100%;
            position: fixed;
            top: 0px;
            left: 0px;
            background-color: transparent;
        }

        .hider-loading {
            position: absolute;
            top: 44%;
            left: 47%;
        }

        .table-cell {
            padding: 5px;
        }

        .button-td {
            width: 140px;
        }

        .vertical-right-img {
            padding-top:13px;
        }

        .details-table tr td {
            vertical-align: middle !important;
        }

        .file-upload-panel {
            margin: 0 auto;
            text-align: center;
            width: 80%;
        }

        .file-upload-panel-stock-deductions {
            margin: 0 auto;
            text-align: center;
            width: 100%;
        }

        .file-uploader {
            margin: 0 auto;
            width: 100%;
        }

        .quote {
            background-color: #bfd777;
        }

        .awaitingAcceptance {
            background-color: #bfd7ff;
        }

        .orderAccepted {
            background-color: #cdf7e4;
        }

        .checkMeasure {
            background-color: #f7f3b9;
        }

        .picking {
            background-color: #77f3b9;
        }

        .inProduction {
            background-color: #f7e7d7;
        }

        .despatched {
            background-color: #fca9a9;
        }

        .installed {
            background-color: #fc77a9;
        }

        .invoiced {
            background-color: #f9e0f9;
        }

        .collected {
            background-color: #e8e8f7;
        }

        .auto-complete-list {
            margin: 0px !important;
            z-index: 99999 !important;
            background-color: ivory;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            overflow: auto;
            max-height: 400px;
            text-align: left;
            left: 0px;
            list-style-type: none;
            width: auto !important;
            padding: 3px;
        }

        .auto-complete-higlighted-item {
            z-index: 99999 !important;
            background-color: #ffff99;
            color: black;
            padding: 1px;
        }

        .auto-complete-item {
            z-index: 99999 !important;
            background-color: Window;
            color: WindowText;
            padding: 1px;
        }

        .auto-complete-loading {
            background-image: url(images/indicator.gif) !important;
            background-position: right !important;
            background-repeat: no-repeat !important;
            background-size: 16px 16px !important;
        }

        .bottom-menu-bar {
            position: fixed;
            bottom: 0px;
            left: 0px;
            width: 100%;
            text-align: center;
            background-color: #F2F2F2;
            border-top: 1px solid #fb9e25;
            padding-top: 5px;
        }

        .bottom-menu-button {
            margin: 5px;
        }

        .menu-button {
            margin: 5px;
        }

        #middle-container-diallerview {
            margin-bottom: 90px !important;
        }

        .hidden {
            display: none;
        }

        .grey-background {
            background-color: #F2F2F2;
        }

        .form-submit-td {
            background-color: #F2F2F2;
            border-top: 1px solid #F2F2F2;
        }

        .form-label-td-p2, .form-field-td-p2 {
            background-color: #F2F2F2;
            border-top: 1px solid #F2F2F2;
        }

    </style>

</head>
<body onload="HideHider();">
    <form runat="server" id="form1" method="post">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" AsyncPostBackTimeout="300" >
    <Scripts>
        <asp:ScriptReference Path="javascript/fixfocus.js" />
    </Scripts>
    </asp:ScriptManager>
    <div id="middle-container-diallerview" >

        <div id="logo" style="height: 100px; text-align: center;" ></div>
        <input type="hidden" id="hfUserType" runat="server" />

        <div style="height: 50px; text-align: center;">
            <asp:Button ID="btnDashBoard" runat="server" CssClass="form-button" Visible="false" Text="Dashboard" UseSubmitBehavior="false" />
            <asp:Button ID="btnHome" runat="server" CssClass="form-button" Visible="true" Text="Home" UseSubmitBehavior="false" />
            <asp:Button ID="btnLogout" runat="server" CssClass="form-button" Visible="true" Text="Logout" OnClientClick="javascript:return ConfirmLeave();" UseSubmitBehavior="false" />
	    </div>

        <div style="padding:10px 5px 5px 5px;" >
            <asp:Panel ID="PanelErrors" runat="server" CssClass=" form">
                <asp:ListBox ID="lstErrors" runat="server" Width="914px" Visible="false"></asp:ListBox>
            </asp:Panel>
        </div>

        <h1>OzRoll Louvre Job Details</h1>

        <div class="form" style="text-align: center; padding-left: 50px">

            <asp:Panel ID="pnlReports" runat="server" Visible="true">
                
                    <asp:Panel ID="pnlHideShowButton" runat="server" Visible="true">
                        <div style="height: 50px; text-align: center;">
                    <asp:Button ID="btnRepairForm" runat="server" CssClass="form-button" Enabled="true" Visible="true" Text="Repair Form" UseSubmitBehavior="false" />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnDeliveryDocket" runat="server" CssClass="form-button" Enabled="true" OnClientClick="return ValidateAll('btnDeliveryDocket');" Text="Delivery Docket" UseSubmitBehavior="false" Visible="true" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnRunningSheet" runat="server" CssClass="form-button" Enabled="true" OnClientClick="return ValidateAll('btnRunningSheet');" Text="Running Sheet" UseSubmitBehavior="false" Visible="true" />
                            &nbsp;&nbsp;&nbsp;
                                   <asp:Button ID="btnQCChecklist" runat="server" CssClass="form-button" Enabled="true" Text="QC Checklist" UseSubmitBehavior="false" Visible="true" />

                    <asp:Button ID="btnTimeSheet" runat="server" CssClass="form-button" Enabled="true" Visible="false" Text="Time Sheet" 
                        OnClientClick="return ValidateAll('btnTimeSheet');" UseSubmitBehavior="false" />&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCoverSheet" runat="server" CssClass="form-button" Enabled="true" Visible="false" Text="Cover Sheet" 
                        OnClientClick="return ValidateAll('btnCoverSheet');" UseSubmitBehavior="false" />&nbsp;&nbsp;&nbsp;
                       </div>
                            </asp:Panel>
                <div style="height: 50px; text-align: center;">
                    <asp:Button ID="btnGenerateOrder" runat="server" CssClass="form-button" Enabled="true" Visible="true" Text="Generate PDF Order" 
                        OnClientClick="return ValidateAll('btnGenerateOrder');" UseSubmitBehavior="false" />&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnEmailQuote" runat="server" CssClass="form-button" Enabled="true" Visible="true" Text="Email Quote to Customer" 
                        OnClientClick="return confirm('Email quote to customer?') && ValidateAll('btnEmailQuote');" UseSubmitBehavior="false" />
                    <asp:Button ID="btnFactoryPaperwork" runat="server" CssClass="form-button" Enabled="true" Text="Factory Paperwork" UseSubmitBehavior="false" Visible="False" />&nbsp;
                    <asp:Button ID="btnGenerateProductionSheet" runat="server" CssClass="form-button" Enabled="true" Visible="true" Text="Generate Production Sheet" 
                        OnClientClick="return ValidateAll('btnGenerateProductionSheet');" UseSubmitBehavior="false" />&nbsp;&nbsp;&nbsp;
                 
                       <asp:Button ID="btnGeneratesOPTIMISER" runat="server" CssClass="form-button" Enabled="true" Visible="true" Text="Generate OPTIMISER" 
                        OnClientClick="return ValidateAll('btnGeneratesOPTIMISER');" UseSubmitBehavior="false" />&nbsp;&nbsp;&nbsp;

                  <asp:Button ID="btExcelOPTIMISER" runat="server" CssClass="form-button" Enabled="true" Visible="true" Text="Generate PC.Sheet" 
                        OnClientClick="return ValidateAll('btExcelOPTIMISER');" UseSubmitBehavior="false" />&nbsp;&nbsp;&nbsp;


                    <asp:LinkButton ID="lnkEmailQuotePDFDummy" runat="server" Text="" CssClass="" UseSubmitBehavior="false" TabIndex="-1" />
                    <asp:LinkButton ID="lnkGenerateOrderPDFDummy" runat="server" Text="" CssClass="" UseSubmitBehavior="false" TabIndex="-1" />
                    <asp:LinkButton ID="lnkGenerateDeliveryDocketPDFDummy" runat="server" Text="" CssClass="" UseSubmitBehavior="false" TabIndex="-1" />
                    <asp:LinkButton ID="lnkGenerateRunningSheetPDFDummy" runat="server" Text="" CssClass="" UseSubmitBehavior="false" TabIndex="-1" />
                    <asp:LinkButton ID="lnkGenerateTimeSheetPDFDummy" runat="server" Text="" CssClass="" UseSubmitBehavior="false" TabIndex="-1" />
                    <asp:LinkButton ID="lnkGenerateCoverSheetPDFDummy" runat="server" Text="" CssClass="" UseSubmitBehavior="false" TabIndex="-1" />
                    <asp:LinkButton ID="lnkGenerateProductionSheetPDFDummy" runat="server" Text="" CssClass="" UseSubmitBehavior="false" TabIndex="-1" />
                    <asp:LinkButton ID="lnkGenerateOptimiserSheetPDFDummy" runat="server" Text="" CssClass="" UseSubmitBehavior="false" TabIndex="-1" />
                    <asp:LinkButton ID="lnkGenerateOptimiserSheetExcelDummy" runat="server" Text="" CssClass="" UseSubmitBehavior="false" TabIndex="-1" />

                    <%--<asp:Button ID="btnPDFTest" runat="server" CssClass="form-button" Enabled="true" Visible="true" Text="PDF TEST" UseSubmitBehavior="false" /> --%>
	            </div>
            </asp:Panel>
                </div>

        <asp:UpdatePanel ID="pnlDetails" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

            <div style="text-align: center;">
               
                <div style="text-align: center;">
                    <asp:Panel ID="pnlAcceptanceAlert" runat="server" Visible="false">
                        <div style="width: 100%; height: 30px; left: 0px; top: 0px; z-index: 100; background-color: Red; color: White; font-weight: bold; text-align: center; padding: 10px 0px 10px 0px;">
                            This job requires action before acceptance to production. <br />
                            Please check and confirm.
                        </div>

                        <div style="margin: 10px;">
                            <asp:Button ID="btnOutstanding" runat="server" CssClass="form-button" Text="Acceptance Items" />
                        </div>
                    </asp:Panel>
                </div>

                <div style="text-align: center;">
                    <asp:Panel ID="pnlStopCustomer" runat="server" Visible="false">
                        <div style="width: 100%; height: 30px; left: 0px; top: 0px; z-index: 100; background-color: Red; color: White; font-weight: bold; text-align: center; padding-top: 10px;">
                            <asp:Label runat="server" ID="lblStopCustomerLabel" Text=""></asp:Label>
                        </div>

                        <asp:Button ID="btnViewCreditDetails" runat="server" CssClass="form-button" Text="View Details" />

                    </asp:Panel>
                </div>

                <div class="container_24">
                    <div class="grid_25">
                        <div class="grid_26 alpha"><!--left hand side information for the order-->
                            <table class="form-table" style="width: 100%; text-align: center;" cellspacing="0" summary="">
                                <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;">Order Type</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                        <asp:DropDownList ID="cboOrderType" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width ="200px" Height="28px" AutoPostBack="true" /><asp:Label ID="lblOrderCharge" runat="server" Text=""></asp:Label>
                                        <asp:RequiredFieldValidator ID="valrfOrderType" runat="server" ControlToValidate="cboOrderType" cssclass="validation-text"
                                                ErrorMessage="<br />Please select an Order Type." InitialValue="0" ValidationGroup="productionschedule" Display="Dynamic" EnableClientScript="false" SetFocusOnError="true" />
                                    </td> 
                                </tr>
                                <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;"><asp:Label ID="lblJobtype" runat="server" Text="Job Type"></asp:Label></td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                        <asp:DropDownList ID="cboJobType" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width="200px" Height="28px" AutoPostBack="true" >
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="valrfJobType" runat="server" ControlToValidate="cboJobType" cssclass="validation-text"
                                                ErrorMessage="<br />Please select a Job Type." InitialValue="0" ValidationGroup="productionschedule" Display="Dynamic" EnableClientScript="false" SetFocusOnError="true" />
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;">Customer</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                        <asp:DropDownList ID="cboCustomer" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width="200px" Height="28px" AutoPostBack="true" />
                                        <asp:Panel ID="pnlImages" runat="server">
                                        <asp:Image ID="imgTipCustomer" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" 
                                            ToolTip="Only customers that have a price category are listed." />
                                        <asp:Image ID="imgSybizLinked" runat="server" ></asp:Image>
                                        <asp:Image ID="imgCustomerPricingCategory" runat="server" ></asp:Image>
                                        </asp:Panel>
                                        <asp:RequiredFieldValidator ID="valrfCustomer" runat="server" ControlToValidate="cboCustomer" cssclass="validation-text"
                                                ErrorMessage="<br />Please select a Customer." InitialValue="0" ValidationGroup="productionschedule" Display="Dynamic"  EnableClientScript="false" SetFocusOnError="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;">Customer Details</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                        <asp:Label ID="lblCustomerDetails" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;">Ozroll ID Number</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                        <asp:Label ID="lblOzrollID" runat="server"></asp:Label>
                                    </td>                            
                                </tr>

                                <asp:Panel ID="pnlOzrollContract" runat="server">
                                    <tr>
                                        <td class="form-label-td" style="width: 30%; text-align: right;">Ozroll Contract</td>
                                        <td class="form-label-td" style="width: 70%; text-align: left;">
                                            <asp:TextBox ID="txtOzrollContract" runat="server" CssClass="form-field" Width="200px" Enabled="false" />
                                            <asp:Image ID="imgTipOzrollContract" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" 
                                                ToolTip="Ozroll Contract is not required for new orders. This field is disabled in order to display old order contract numbers only." />
                                        </td>                            
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlContractNumber" runat="server">
                                    <tr>
                                        <td class="form-label-td" style="width: 30%; text-align: right;">Contract Ref.</td>
                                        <td class="form-label-td" style="width: 70%; text-align: left;">
                                            <asp:TextBox ID="txtContractNumber" runat="server" CssClass="form-field" MaxLength="17" Width="200px" />
                                        </td>                            
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="pnlCustomerName" runat="server">
                                <tr style="display:none">
                                    <td class="form-label-td" style="width: 30%; text-align: right;">Customer Name</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-field" Text="" Width="200px" />
                                    </td>
                                </tr>
                                    </asp:Panel>
                                <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;">Delivery Address</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                        <asp:RadioButton ID="radPickup" runat="server" GroupName="optDelivery" Text="Pickup" AutoPostBack="true" CssClass="radioPickup" />&nbsp;&nbsp;
                                        <asp:RadioButton ID="radDelivery" runat="server" GroupName="optDelivery" Checked="true" Text="Delivery" AutoPostBack="true" CssClass="radioDelivery" />
                                       
                                        <asp:Panel ID="pnlDeliveryAddress" runat="server">
                                            <asp:DropDownList ID="cboDeliveryAddress" runat="server" EnableViewState="true" CssClass="form-field" Width="380px" AutoPostBack="true" />
                                            <asp:RequiredFieldValidator ID="valrfDeliveryAddress" runat="server" ControlToValidate="cboDeliveryAddress" cssclass="validation-text"
                                                    ErrorMessage="<br />Please select a Delivery Address." InitialValue="0" ValidationGroup="productionschedule" Display="Dynamic" EnableClientScript="false" SetFocusOnError="true" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;">State</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                        <asp:TextBox ID="txtState" runat="server" CssClass="form-field" Text="" Width="80px" Enabled="false" />
                                    </td>
                                </tr>
                                <asp:Panel ID="pnlPriority" runat="server" Visible="false">
                                    <tr>
                                        <td class="form-label-td" style="width: 30%; text-align: right;">Priority</td>
                                        <td class="form-label-td" style="width: 70%; text-align: left;">
                                            <asp:DropDownList ID="cboPriority" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width ="200px" Height="28px" />
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="pnlOriginalOrder" runat="server" Visible="false">
                                    <tr>
                                        <td class="form-label-td" style="width: 30%; text-align: right;">Original Order No</td>
                                        <td class="form-label-td" style="width: 70%; text-align: left;">
                                            <asp:TextBox ID="txtOriginalOrder" runat="server" CssClass="form-field" Text="" Width="200px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-label-td" style="width: 30%; text-align: right;">&nbsp;&nbsp;</td>
                                        <td class="form-label-td" style="width: 70%; text-align: left;">
                                            <asp:Label ID="lblOriginalOrderDetails" runat="server" Text=""></asp:Label>
                                        </td> 
                                    </tr>
                                </asp:Panel>

                                <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;">Contract Total</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                        <asp:TextBox ID="txtSalePrice" runat="server" CssClass="form-field" OnChange="javascript:validfloat();" Text="" Width="100px" Enabled="false" /> ex GST
                                    </td>                            
                                </tr>

                                <asp:Panel ID="pnlFreightTotal" runat="server">
                                    <tr>
                                        <td class="form-label-td" style="width: 30%; text-align: right;">Freight Total</td>
                                        <td class="form-label-td" style="width: 70%; text-align: left;">
                                            <asp:TextBox ID="txtFreightTotal" runat="server" CssClass="form-field" OnChange="javascript:validfloat();" Text="" Width="100px" Enabled="false" /> ex GST

                                            <div style="vertical-align:middle; display:inline-block; height:32px;">
                                                <asp:Button ID="btnFreightPriceOverride" runat="server" CssClass="override-button" Width="32px" Height="32px" ToolTip="Override" />

                                                <asp:Button ID="btnFreightPriceOverrideRemove" runat="server" CssClass="remove-override-button" Width="32px" Height="32px" ToolTip="Remove price override."
                                                    OnClientClick="return confirm('Removing an overridden price will auto calculate the new price. Are you sure?');" />
                                            </div>

                                            <div style="vertical-align:middle; display:inline-block; height:32px;">
                                                <asp:Button ID="btnFreightPriceOverrideDetailSave" runat="server" CssClass="save-button" Width="32px" Height="32px" 
                                                    ToolTip="Save Override" Visible="false" />

                                                <asp:Button ID="btnFreightPriceOverrideDetailCancel" runat="server" CssClass="cancel-button" Width="32px" Height="32px" 
                                                    ToolTip="Cancel Override" Visible="false" />
                                            </div>

                                        </td>
                                    </tr>
                                </asp:Panel>

                                <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;">Total Sq.M.</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                        <asp:TextBox ID="txtTotalSQM" runat="server" CssClass="form-field" OnChange="javascript:validfloat();" Enabled="false" Text="" Width="80px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;">Total Panels</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                        <asp:TextBox ID="txtTotalPanels" runat="server" CssClass="form-field" Text="" Enabled="false" Width="80px" />
                                    </td>
                                </tr>
                                  <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;">Consignment Note</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                         <asp:Label ID="lblConNote" runat="server" />
                                    </td>
                                </tr>
                                <asp:Panel ID="pnlStockDeduction" runat="server">
                                <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;">Stock Deductions</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">  
                                        <asp:Label ID="lblStockDeductionsStatus" runat="server" />
                                        <asp:Button ID="btnStockDeductionsPage" runat="server" Text="Deductions" CssClass="form-button" Visible="false" />
                                    </td>
                                </tr>
                                    </asp:Panel>

                                <asp:Panel ID="pnlRetailInstallDetails" runat="server" Visible="false">
                                    <tr>
                                        <td class="form-label-td" style="width: 30%; text-align: right;">Check Measurer</td>
                                        <td class="form-label-td" style="width: 70%; text-align: left;">
                                            <asp:DropDownList ID="cboCheckMeasure" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width="200px" Height="28px" >
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-label-td" style="width: 30%; text-align: right;">Installer</td>
                                        <td class="form-label-td" style="width: 70%; text-align: left;">
                                            <asp:DropDownList ID="cboInstaller" runat="server" UseSubmitBehavior="false" CssClass="form-select" Width="200px" Height="28px" >
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="pnlRemake" runat="server" Visible="false">
                                    <tr>
                                        <td class="form-label-td" style="width: 30%; text-align: right;">Remake Description</td>
                                        <td class="form-label-td" style="width: 70%; text-align: left;">
                                            <asp:TextBox ID="txtRemakeDescription" runat="server" TextMode="MultiLine" CssClass="form-field" /> 
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="pnlSybizJobcode" runat="server">
                                <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;">Sybiz Job Code</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                        <asp:Label ID="lblSybizJobCode" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                </asp:Panel>
                                <asp:Panel ID="pnlsyBizInoviceNumber" runat="server">
                                <tr>
                                    <td class="form-label-td" style="width: 30%; text-align: right;">Sybiz Invoice No.</td>
                                    <td class="form-label-td" style="width: 70%; text-align: left;">
                                        <asp:Label ID="lblSybizSalesInvoiceNo" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                    </asp:Panel>

                            </table>

                            <asp:Panel ID="pnlStockUsageButton" runat="server" Visible="false">
                                <div style="text-align: center;">
                                    <asp:Button ID="btnStockUsage" runat="server" Text="Stock Usage" CssClass="form-button" Width="150px" />
                                </div>
                            </asp:Panel>

                            <br />
                            <br />

                            <h2>Attach Drawing / Files</h2>

                                <asp:Panel ID="pnlProdSheduleFiles" runat="server" Visible="true" >
                                    <asp:GridView ID="gdvProdScheduleFiles" runat="server" EnableTheming="false" DataKeyNames="ID" CssClass="extras-table" AutoGenerateColumns="false" 
                                            CellPadding="4" ForeColor="#333333" ShowHeader="true" BorderStyle="None" Width="90%" >
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#0059A9" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                            <EditRowStyle BackColor="#999999" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:label ID="lblID" runat="server" CssClass="form-select" Text='<%# Eval("ID") %>' Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkProdScheduleFileName" runat="server" CommandName="GetProdScheduleFile" CommandArgument='<%# Eval("ID") %>' 
                                                            Text='<%# Eval("FileName").ToString %>' />
                                                        <asp:Label ID="lblProdScheduleFileSize" runat="server" Text='<%#"<span style=""font-style:Italic;"">(" & Format(IIf(DirectCast(Eval("FileSize"), Integer) > 0, DirectCast(Eval("FileSize"), Integer) / SharedConstants.KB_SIZE, 0), "0.##") & "KB)</ span>"%>' />
                                                        <asp:Label ID="lblProdScheduleFileUnsaved" runat="server" Visible='<%# FileIsUnsaved(DirectCast(Eval("ID"), Integer)) %>' Text='<%#"<span style=""font-style:Italic; color:red;"">UNSAVED</ span>"%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderStyle Width="70px" />
                                                    <HeaderTemplate>
                                                        Visible Customer
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkProdScheduleFileVisiblePortal" runat="server" Checked='<%# Eval("VisiblePortal") %>' AutoPostBack="true" OnCheckedChanged="chkProdScheduleFileVisiblePortal_CheckChanged"
                                                            CommandName="ProdScheduleFileVisiblePortal" CommandArgument='<%# Eval("ID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderStyle Width="70px" />
                                                    <HeaderTemplate>
                                                        Customer Delete
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkProdScheduleFileCanDeletePortal" runat="server" Checked='<%# Eval("CanDeletePortal") %>' AutoPostBack="true" OnCheckedChanged="chkProdScheduleFileVisiblePortal_CheckChanged"
                                                            CommandName="ProdScheduleFileCanDeletePortal" CommandArgument='<%# Eval("ID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnProdScheduleDelete" runat="server" CommandName="DeleteProdScheduleFile" CommandArgument='<%# Eval("ID") %>' 
                                                            CssClass="delete-button" ToolTip="Delete" OnClientClick="return confirm('Delete this file from the production sheet? The file will not be removed until the production schedule is saved.');" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            

                                <asp:Panel ID="pnlUploadFiles" runat="server" CssClass="file-upload-panel" Visible="false" >
                                    <ajaxcontroltoolkit:AjaxFileUpload 
                                        ID="ajaxUploader" 
                                        MaximumNumberOfFiles="10"
                                        AllowedFileTypes="jpg,jpeg,png,pdf,doc,docx,xls,xlsx"
                                        MaxFileSize="<%# SharedConstants.MAX_UPLOAD_FILE_SIZE %>"
                                        AutoStartUpload="true"
                                        CssClass="file-uploader"
                                        OnClientUploadComplete="ajaxUploader_UploadComplete"
                                        runat="server" />

                                    <br />

                                    <div style="font-style:italic; padding-bottom: 10px;"><asp:Label ID="lblProdScheduleFilesSupportedFormats" runat="server" Text="" /></div>

                                    <div style="font-style:italic; color:red;">Files will not be saved until the production schedule is saved.</div>

                                    <br />

                                    <asp:Button ID="btnCancelUploadFiles" runat="server" CssClass="form-button" Text="Cancel File Upload" />

                                </asp:Panel>

                                <asp:Button ID="btnUploadFiles" runat="server" CssClass="form-button" Text="Upload Files" Width="115px" />
                        </div>

                        <div class="grid_27 omega"><!--right hand side information for the order and statuses-->
                            <div><!--current status and hold-->
                                <table class="form-table" style="width: 100%; text-align: center;" cellspacing="0" summary="">
                                    <tr>
                                        <td id="tdCurrentStatus1" class="form-label-td" runat="server" style="width: 30%; text-align: right;">Current Status</td>
                                        <td id="tdCurrentStatus2" class="form-label-td" runat="server" style="width: 70%; text-align: left;">
                                            <asp:Label ID="lblCurrentStatus" runat="server"></asp:Label>

                                            <asp:Panel ID="pnlCreditHold" runat="server" >
                                                <span style="color:red; font-weight:bold; text-align:center;">On Credit Hold</span>
                                                <asp:Image ID="imgCreditHold" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" 
                                                    ToolTip="The credit check can be overridden by someone with the appropriate permission." />
                                            </asp:Panel>

                                        </td>
                                    </tr>    
                                    <tr>
                                        <td class="form-label-td" style="width: 30%; text-align: right;">Expiry</td>
                                        <td class="form-label-td" style="width: 70%; text-align: left;">
                                            <asp:Label ID="lblExpiryDateTime" runat="server"></asp:Label>
                                        </td>
                                    </tr>  
                                    <asp:Panel runat="server" ID="pnlOnHold">
                                    <tr>
                                        <td class="form-label-td" style="width: 30%; text-align: right;">On Hold</td>
                                        <td class="form-label-td" style="width: 70%; text-align: left;">
                                            <asp:CheckBox ID="chkHoldJob" runat="server" />
                                        </td>
                                    </tr>
                                        </asp:Panel>

                                    <asp:Panel ID="pnlCreditCheckOverride" runat="server" Visible="<%# UserHasCreditCheckOverridePermission() %>">
                                        <tr>
                                            <td class="form-label-td" style="width: 30%; text-align: right;">Override Credit Check</td>
                                            <td class="form-label-td" style="width: 70%; text-align: left;">
                                                <asp:CheckBox ID="chkCreditCheckOverride" runat="server" />
                                                <asp:Image ID="imgCreditCheckOverride" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" 
                                                    ToolTip="Overriding the customer credit check will allow the creation of a job in Sybiz and allow the production schedule to progress past awaiting acceptance (ordered) status." />
                                            </td>
                                        </tr> 
                                    </asp:Panel>
                                                             
                                </table>                            
                            </div>
                            <div class="column"><!--start status area-->
                                <table class="form-table" style="width: 100%; text-align: center;" cellspacing="0" summary="">
                                    <tr>
                                        <td class="form-label-td quote" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="Quote">Created</asp:label></td>
                                        <td class="form-label-td quote" style="text-align: left;">
                                            <asp:TextBox ID="txtEnteredDatetime" runat="server" CssClass="form-field" Text="" Width="120px" Enabled="false" />
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td class="form-label-td awaitingAcceptance" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="Awaiting Acceptance">Ordered</asp:label></td>
                                        <td class="form-label-td awaitingAcceptance" style="text-align: left;">
                                            <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form-field" Text="" Width="120px" />
                                        </td>
                                    </tr> 
                                    <asp:Panel ID="pnlShowHideTextBox" runat="server" >
                                    <tr>
                                        <td class="form-label-td orderAccepted" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="Paperwork Processing">Received</asp:label></td>
                                        <td class="form-label-td orderAccepted" style="text-align: left;">
                                            <asp:TextBox ID="txtReceived" runat="server" CssClass="form-field" Text="" Width="120px" AutoPostBack="true" ClearButton="btnClearReceived" ></asp:TextBox>
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnClearReceived" runat="server" CssClass="form-button" Visible="false" Text="Clear" CommandArgument="txtReceived" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnAcceptOrder" runat="server" Text="Accept Order" CssClass="form-button" Visible="false" Width="150px" />
                                        </td>
                                    </tr>
                                    <asp:Panel ID="pnlCheckMeasureDate" runat="server" Visible="false">
                                        <tr>
                                            <td class="form-label-td checkMeasure" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="Check Measure">Check Measure</asp:label></td>
                                            <td class="form-label-td checkMeasure" style="text-align: left;">
                                                <asp:TextBox ID="txtCheckMeasureDate" runat="server" CssClass="form-field" Text="" Width="120px" AutoPostBack="true" ClearButton="btnClearCM" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnClearCM" runat="server" CssClass="form-button" Visible="false" Text="Clear" CommandArgument="txtCheckMeasureDate" />
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td class="form-label-td picking" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="Picking">Picking</asp:label></td>
                                        <td class="form-label-td picking" style="text-align: left;">
                                            <asp:TextBox ID="txtPickingDate" runat="server" CssClass="form-field" Text="" Width="120px" AutoPostBack="true" ClearButton="btnClearPicking" ></asp:TextBox>
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnClearPicking" runat="server" CssClass="form-button" Visible="false" Text="Clear" CommandArgument="txtPickingDate" />
                                             &nbsp;&nbsp;
                                            <asp:Button ID="btnPickingList" runat="server" Text="Picking List" CssClass="form-button" Visible="false" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-label-td inProduction" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="In Production">In Production</asp:label></td>
                                        <td class="form-label-td inProduction" style="text-align: left;">
                                            <asp:TextBox ID="txtScheduledDate" runat="server" CssClass="form-field" Text="" Width="120px" AutoPostBack="true" ClearButton="btnClearScheduled" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnClearScheduled" runat="server" CssClass="form-button" Visible="false" Text="Clear" CommandArgument="txtScheduledDate" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="form-label-td inProduction" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="In Production">Cutting</asp:label></td>
                                        <td class="form-label-td inProduction" style="text-align: left;">
                                            <asp:TextBox ID="txtCuttingDate" runat="server" CssClass="form-field" Text="" Width="120px" AutoPostBack="true" ClearButton="btnClearCutting" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnClearCutting" runat="server" CssClass="form-button" Visible="false" Text="Clear" CommandArgument="txtCuttingDate" />
                                        </td>                                
                                    </tr>
                                    <tr>
                                        <td class="form-label-td inProduction" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="In Production">Pinning</asp:label></td>
                                        <td class="form-label-td inProduction" style="text-align: left;">
                                            <asp:TextBox ID="txtPiningDate" runat="server" CssClass="form-field" Text="" Width="120px" AutoPostBack="true" ClearButton="btnClearPining" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnClearPining" runat="server" CssClass="form-button" Visible="false" Text="Clear" CommandArgument="txtPiningDate" />
                                         </td>                                
                                    </tr>
                                    <tr>
                                        <td class="form-label-td inProduction" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="In Production">Prep</asp:label></td>
                                        <td class="form-label-td inProduction" style="text-align: left;">
                                            <asp:TextBox ID="txtPrepDate" runat="server" AutoPostBack="true" CssClass="form-field" Text="" Width="120px" ClearButton="btnClearPrep" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnClearPrep" runat="server" CssClass="form-button" Text="Clear" Visible="false" CommandArgument="txtPrepDate" />
                                        </td>
                                    <tr>
                                        <td class="form-label-td inProduction" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="In Production">Assembly</asp:label></td>
                                        <td class="form-label-td inProduction" style="text-align: left;">
                                            <asp:TextBox ID="txtAssemblyDate" runat="server" AutoPostBack="true" CssClass="form-field" Text="" Width="120px" ClearButton="btnClearAssembly" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnClearAssembly" runat="server" CssClass="form-button" Text="Clear" Visible="false" CommandArgument="txtAssemblyDate" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-label-td inProduction" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="In Production">Hinging</asp:label></td>
                                        <td class="form-label-td inProduction" style="text-align: left;">
                                            <asp:TextBox ID="txtHingingDate" runat="server" AutoPostBack="true" CssClass="form-field" Text="" Width="120px" ClearButton="btnClearHinging" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnClearHinging" runat="server" CssClass="form-button" Text="Clear" Visible="false" CommandArgument="txtHingingDate" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-label-td inProduction" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="In Production">Packup</asp:label></td>
                                        <td class="form-label-td inProduction" style="text-align: left;">
                                            <asp:TextBox ID="txtPackupDate" runat="server" AutoPostBack="true" CssClass="form-field" Text="" Width="120px" ClearButton="btnClearPackup" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnClearPackup" runat="server" CssClass="form-button" Text="Clear" Visible="false" CommandArgument="txtPackupDate" />
                                            &nbsp;&nbsp;
                                           <asp:Button ID="btnPackUp" runat="server" Text="Packup" CssClass="form-button" Visible="false" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-label-td inProduction" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="In Production">QC</asp:label></td>
                                        <td class="form-label-td inProduction" style="text-align: left;">
                                            <asp:TextBox ID="txtQCDate" runat="server" AutoPostBack="true" CssClass="form-field" Text="" Width="120px" ClearButton="btnClearQC" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnClearQC" runat="server" CssClass="form-button" Text="Clear" Visible="false" CommandArgument="txtQCDate" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdPostProduction11" runat="server" class="form-label-td" style="width: 22%; text-align: right;">
                                            <asp:Label ID="lblPostProduction1" runat="server"></asp:Label>
                                            </td>
                                        <td id="tdPostProduction12" runat="server" class="form-label-td" style="text-align: left;">
                                            <asp:TextBox ID="txtPostProduction1" runat="server" AutoPostBack="true" CssClass="form-field" Text="" Width="120px" ClearButton="btnClearPostProduction1" Enabled="false" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnClearPostProduction1" runat="server" CssClass="form-button" Text="Clear" Visible="false" CommandArgument="txtPostProduction1" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnDispatchDate" runat="server" Text="Despatch" CssClass="form-button" Visible="false" Width="150px" />
                                        </td>
                                    </tr>
                                    <asp:Panel ID="pnlInstallDate" runat="server" Visible="false">
                                        <tr>
                                            <td class="form-label-td installed" style="width: 22%; text-align: right;"><asp:label runat="server" ToolTip="Installed">Install</asp:label></td>
                                            <td class="form-label-td installed" style="text-align: left;">
                                                <asp:TextBox ID="txtInstallDate" runat="server" AutoPostBack="true" CssClass="form-field" Text="" Width="120px" ClearButton="btnClearInstall" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnClearInstall" runat="server" CssClass="form-button" Text="Clear" Visible="false" CommandArgument="txtInstallDate" />
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td id="tdPostProduction21" runat="server" class="form-label-td" style="width: 22%; text-align: right;">
                                            <asp:Label ID="lblPostProduction2" runat="server"></asp:Label>
                                            </td>
                                        <td id="tdPostProduction22" runat="server" class="form-label-td" style="text-align: left;">
                                            <asp:TextBox ID="txtPostProduction2" runat="server" AutoPostBack="true" CssClass="form-field" Text="" Width="120px" ClearButton="btnClearPostProduction2" Enabled="false" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnClearPostProduction2" runat="server" CssClass="form-button" Text="Clear" Visible="false" CommandArgument="txtPostProduction2" />
                                        </td>
                                    </tr>
                                        </asp:Panel>
                                </table>                                                        
                            </div>
                            <div><!--other info remaining under status areas-->
                                <table class="form-table" style="width: 100%; text-align: center;" cellspacing="0" summary="">
                                    <asp:Panel runat="server" ID="pnlPromisedExpectedShipingDate">
                                   <tr>
                                        <td class="form-label-td" style="width: 35%; text-align: right;">Promised Date</td>
                                        <td class="form-label-td" style="width: 20%; text-align: left;">
                                            <asp:TextBox ID="txtPromisedDate" runat="server" CssClass="form-field" Text="" Enabled="false" Width="120px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form-label-td" style="width: 35%; text-align: right;">Expected Shipping Date</td>
                                        <td class="form-label-td" style="width: 20%; text-align: left;">
                                            <asp:TextBox ID="txtPlannedShippingDate" runat="server" CssClass="form-field" Text="" Width="120px" AutoPostBack="True" />
                                        </td>
                                    </tr>
                                        </asp:Panel>
                                    <asp:Panel runat="server" ID="pnlShowHideActualShippingDate">
                                    <tr>
                                        <td class="form-label-td" style="width: 35%; text-align: right;">Actual Shipping Date</td>
                                        <td class="form-label-td" style="width: 20%; text-align: left;">
                                            <asp:TextBox ID="txtActualShippingDate" runat="server" CssClass="form-field" Text="" Width="120px" />
                                        </td>
                                    </tr></asp:Panel>
                            
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlOrderForms" runat="server" Visible="false">
                                                <div style="text-align: center;">
                                                    <asp:Label ID="lblOrderForm" runat="server"></asp:Label>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlOrderForm" runat="server" Visible="false">
                                                <div style="text-align: center;">
                                                    <asp:Button ID="btnOrderForm" runat="server" CssClass="form-button" Text="Order Form" />                                                   
                                                </div>                                       
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlNotesHide" runat="server" Visible="true">
                                                <div style="text-align: center;">
                                                    <asp:Button ID="btnViewNotes" runat="server" CssClass="form-button" Visible="true" Text="Notes" Width="90px" />
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >
                                            <asp:Panel ID="pnlCancellation" runat="server" Visible="false">
                                                <div style="text-align: center;">
                                                    <asp:Button ID="btnCancellation" runat="server" CssClass="form-button" Visible="true" Text="Cancel Job" Width="90px" />
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                        

            </div>


            <div class="container_24">
                <div class="grid_25">                
                    <asp:Panel ID="pnlOpeningDetails" runat="server" Visible="true"  >
                        <div style="text-align: center;">
                            <h2>Details</h2>

                            <asp:GridView ID="dgvDetails" runat="server" DataKeyNames="LouvreDetailID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" CssClass="details-table" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <FooterStyle BackColor="#01853f" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#01853f" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-CssClass="table-cell button-td" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnViewDetail" runat="server" Text="Details" CommandName="LouvreDetail" CommandArgument='<%# Eval("LouvreDetailID") %>' CssClass="form-button details-button" Width="100px" Height="28px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField Visible="false" >
                                        <ItemTemplate>
                                            <asp:label ID="lblLouvreDetailID" runat="server" Text='<%# Eval("LouvreDetailID") %>' Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="PlantationScheduleID" Visible="false" />
                                    <asp:BoundField DataField="ShutterID" SortExpression="ShutterID" HeaderText="Opening #" ReadOnly="true" />
                                    <asp:BoundField DataField="Height" SortExpression="Height" HeaderText="Height" ReadOnly="true" />
                                    <asp:BoundField DataField="Width" SortExpression="Width" HeaderText="Width" ReadOnly="true" />
                                    <asp:BoundField DataField="NoOfPanels" SortExpression="NoOfPanels" HeaderText="# Panels" ReadOnly="true" />
                                    <asp:BoundField DataField="Product" SortExpression="Product" HeaderText="Style" ReadOnly="true" />
                                    <asp:BoundField DataField="ShutterType" SortExpression="ShutterType" HeaderText="Type" ReadOnly="true" />
                                    <asp:BoundField DataField="Colour" SortExpression="Colour" HeaderText="Colour" ReadOnly="true" />

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Price
                                        </HeaderTemplate>
                                        <ItemTemplate>

                                            <div style="vertical-align:middle; display:inline-block;">
                                                <asp:Label ID="lblSalePrice" runat="server" ForeColor="<%# GetSalePriceTextColour(DirectCast(GetDataItem(), LouvreDetails)) %>" 
                                                    Text="<%# GetLouvreSalePriceForRow(DirectCast(GetDataItem(), LouvreDetails)) %>" ToolTip ="<%# GetSalePriceDetailToolTipText(DirectCast(GetDataItem(), LouvreDetails)) %>" ></asp:Label>
                                            </div>

                                            <div style="vertical-align:middle; display:inline-block; height:32px;">
                                                <asp:Button ID="btnSalePriceOverrideDetail" runat="server" CommandName="edit" CssClass="override-button" Width="32px" Height="32px" 
                                                    ToolTip="Override" Visible="<%# ((CalculateStatus() > SharedEnums.ProductionScheduleStatus.Quote) AndAlso UserHasPriceOverridePermission() AndAlso (Not DirectCast(GetDataItem(), LouvreDetails).LouvrePriceIsOverridden)) %>" />

                                                <asp:Button ID="btnSalePriceOverrideDetailRemove" runat="server" CommandName="SalePriceOverrideRemove" CommandArgument='<%# Eval("LouvreDetailID") %>' 
                                                    CssClass="remove-override-button" Width="32px" Height="32px" ToolTip="Remove price override." OnClientClick="return confirm('Removing an overridden price will auto calculate the new price. Are you sure?');"
                                                    Visible="<%# ((CalculateStatus() > SharedEnums.ProductionScheduleStatus.Quote) AndAlso UserHasPriceOverridePermission() AndAlso DirectCast(GetDataItem(), LouvreDetails).LouvrePriceIsOverridden) %>" />
                                            </div>

                                        </ItemTemplate>
                                        <EditItemTemplate>

                                            <div style="vertical-align:middle; display:inline-block;">
                                                <asp:TextBox ID="txtSalePriceDetail" runat="server" CssClass="form-field" width="100px"
                                                    Text="<%# GetLouvreSalePriceForRow(DirectCast(GetDataItem(), LouvreDetails)) %>" ></asp:TextBox>
                                            </div>

                                            <div style="vertical-align:middle; display:inline-block; height:32px;">
                                                <asp:Button ID="btnSalePriceOverrideDetailSave" runat="server" CommandName="update" CommandArgument='<%# Eval("LouvreDetailID") %>' 
                                                    CssClass="save-button" Width="32px" Height="32px" ToolTip="Save Override" Visible="<%# UserHasPriceOverridePermission() %>" />
                                                <asp:Button ID="btnSalePriceOverrideDetailCancel" runat="server" CommandName="cancel" CommandArgument='<%# Eval("LouvreDetailID") %>' 
                                                    CssClass="cancel-button" Width="32px" Height="32px" ToolTip="Cancel Override" />
                                            </div>

                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField ItemStyle-CssClass="table-cell" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDuplicateDetail" runat="server" Text="" CommandName="DetailDuplicate" CommandArgument='<%# Eval("LouvreDetailID") %>' 
                                                CssClass="duplicate-button" Width="32px" Height="32px" OnClientClick="return confirm('Duplicate this opening and all of its contents?');" 
                                                ToolTip="Duplicate" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnDeleteDetail" runat="server" Text="" CommandName="DeleteDetail" CommandArgument='<%# Eval("LouvreDetailID") %>' 
                                                CssClass="delete-button" Width="32px" Height="32px" OnClientClick="return confirm('Delete this opening and all of its contents?');" 
                                                ToolTip="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                     
                                </Columns>
                            </asp:GridView>
                            
                            <br />
                            
                            <div style="text-align: center;">
                                <asp:Button ID="btnAddDetails" runat="server" Text="Add Louvre" CssClass="form-button menu-button" />   
                                <%--Added visible false property against ticket #73896 by Pradeep Singh on 15/02/2023--%>
                                <asp:Button ID="btnAddPrivacyScreen" runat="server" Text="Add Privacy Screen" CssClass="form-button menu-button" Visible="false" /> 
                            </div>                    
                    
                        </div>
                    </asp:Panel>

                    <br />
                    <br />

                    <asp:Panel ID="pnlExtrasProdSchedule" runat="server" Visible="true"  >
                        <div style="text-align: center;">

                            <h2>Extras</h2>

                            <asp:GridView ID="gdvExtrasProdSchedule" runat="server" EnableTheming="false" DataKeyNames="ID" CssClass="extras-table" AutoGenerateColumns="false" 
                                CellPadding="4" ForeColor="#333333" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#01853f" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#01853f" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:label ID="lblID" runat="server" CssClass="form-select" Text='<%# Eval("ID") %>' Width ="95%" Visible="false">
                                            </asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Extra
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlExtraName" runat="server" CssClass="form-select extras-field" Width ="95%" AutoPostBack="false" Enabled="false" >
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlExtraName" runat="server" CssClass="form-select extras-field" Width ="95%" AutoPostBack="true" OnSelectedIndexChanged="ddlExtraName_SelectedIndexChanged" Enabled="true" >
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="valrfExtraName" runat="server" ControlToValidate="ddlExtraName" cssclass="validation-text"
                                                ErrorMessage="<br />Please select an Extra Name." InitialValue="0" ValidationGroup="productionscheduleextras" Display="Dynamic" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Qty
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQty" Text='<%# Eval("Quantity") %>' CssClass="extras-field" style="text-align: center" runat="server" Width="70px" MaxLength="4" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtQty" Text='<%# Eval("Quantity") %>' CssClass="extras-field" style="text-align: center" runat="server" Width="70px" MaxLength="4" Enabled="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="valrfQty" runat="server" ControlToValidate="txtQty" cssclass="validation-text"
                                                ErrorMessage="<br />Qty is required." ValidationGroup="productionscheduleextras" Display="Dynamic" />
                                            <asp:CompareValidator ID="valcompQty" runat="server" Operator="DataTypeCheck" Type="Integer" cssclass="validation-text"
                                                ControlToValidate="txtQty" ErrorMessage="<br />Qty is invalid." ValidationGroup="productionscheduleextras" Display="Dynamic" />
                                            <asp:RangeValidator runat="server" Type="Double" cssclass="validation-text"
                                                MinimumValue="1" MaximumValue="9999" ControlToValidate="txtQty" ValidationGroup="productionscheduleextras"
                                                ErrorMessage="<br />Qty must be greater than 0." Display="Dynamic" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Length (mm)
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCutLength" Text='<%# Eval("CutLength") %>' Visible="<%# GetCutLengthVisibleForExtra(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" 
                                                CssClass="extras-field" style="text-align: center" runat="server" Width="70px" MaxLength="4" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                        <EditItemTemplate>

                                                <asp:TextBox ID="txtCutLength" Text='<%# Eval("CutLength") %>' Visible="<%# GetCutLengthVisibleForExtra(DirectCast(GetDataItem(), LouvreExtraProduct)) %>"
                                                     CssClass="extras-field" style="text-align: center" runat="server" Width="70px" MaxLength="4" Enabled="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="valrfCutLength" runat="server" ControlToValidate="txtCutLength" cssclass="validation-text"
                                                    Enabled="<%# GetCutLengthVisibleForExtra(DirectCast(GetDataItem(), LouvreExtraProduct)) %>"
                                                    ErrorMessage="<br />Length is required." ValidationGroup="productionscheduleextras" Display="Dynamic" />
                                                <asp:CompareValidator ID="valcompCutLength" runat="server" Operator="DataTypeCheck" Type="Integer" cssclass="validation-text"
                                                    Enabled="<%# GetCutLengthVisibleForExtra(DirectCast(GetDataItem(), LouvreExtraProduct)) %>"
                                                    ControlToValidate="txtCutLength" ErrorMessage="<br />Length is invalid." ValidationGroup="productionscheduleextras" Display="Dynamic" />
                                                <asp:CustomValidator ID="valcustCutLengthMax" runat="server"
                                                    Enabled="<%# GetCutLengthVisibleForExtra(DirectCast(GetDataItem(), LouvreExtraProduct)) %>"  
                                                    OnServerValidate="valcustCutLengthMax_ServerValidate"
                                                    Display="Dynamic" Text="<br />Length exceeds extra maximum." 
                                                    ValidationGroup="productionscheduleextras"
                                                    ControlToValidate="txtCutLength"
                                                    cssclass="validation-text" /> 

                                        </EditItemTemplate>
                                    </asp:TemplateField> 

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Colour
                                        </HeaderTemplate>
                                        <ItemTemplate>

                                            <asp:TextBox ID="txtColour" Text="<%# GetColourNameForID(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" 
                                                Visible="<%# GetColourNameVisibleForExtra(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" runat="server" 
                                                CssClass="extras-field" Width="200px" style="text-align: center" Enabled="false" />
                                            <ext:HiddenFieldExtended ID="hdnColourID" runat="server" Value="<%# DirectCast(GetDataItem(), LouvreExtraProduct).ColourID %>" />
                                            <ext:HiddenFieldExtended ID="hdnColourName" runat="server" Value="<%# GetColourNameForID(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" />

                                        </ItemTemplate>
                                        <EditItemTemplate>

                                            <asp:TextBox ID="txtColour" Text="<%# GetColourNameForID(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" 
                                                Visible="<%# GetColourNameVisibleForExtra(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" runat="server" 
                                                CssClass="extras-field" Width="200px" style="text-align: center" Enabled="true" onfocus="this.select();" />

                                            <ajaxcontroltoolkit:AutoCompleteExtender
                                                            ID="aceColour"
                                                            runat="server"
                                                            TargetControlID="txtColour"
                                                            ServicePath="AutoComplete.asmx"
                                                            ServiceMethod="GetMatchingColourList"
                                                            MinimumPrefixLength="1"
                                                            CompletionInterval="1000"
                                                            CompletionSetCount="0"
                                                            EnableCaching="false"
                                                            CompletionListCssClass="auto-complete-list"
                                                            CompletionListHighlightedItemCssClass="auto-complete-higlighted-item"
                                                            CompletionListItemCssClass="auto-complete-item"
                                                            OnClientItemSelected="AutoCompleteGetColourIDExtra"
                                                            OnClientPopulating="AutoCompleteStart"
                                                            OnClientPopulated="AutoCompleteEnd"
                                                            OnClientHiding="AutoCompleteEnd"
                                                            FirstRowSelected="true"></ajaxcontroltoolkit:AutoCompleteExtender>

                                                        <ext:HiddenFieldExtended ID="hdnColourID" runat="server" Value="<%# DirectCast(GetDataItem(), LouvreExtraProduct).ColourID %>" />
                                                        <ext:HiddenFieldExtended ID="hdnColourName" runat="server" Value="<%# GetColourNameForID(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" />

                                            <asp:RequiredFieldValidator ID="valrfColour" runat="server" ControlToValidate="hdnColourID" CssClass="validation-text" 
                                                Enabled="<%# GetColourNameVisibleForExtra(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" ErrorMessage="<br />Please select a Colour." 
                                                InitialValue="0" ValidationGroup="productionscheduleextras" Display="Dynamic" EnableClientScript="false" />
                                            
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Price
                                        </HeaderTemplate>
                                        <ItemTemplate>

                                            <div style="vertical-align:middle; display:inline-block;">
                                                <asp:Label ID="lblSalePrice" runat="server" ForeColor="<%# GetSalePriceTextColour(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" 
                                                    Text="<%# GetExtraSalePriceForRow(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" ToolTip ="<%# GetSalePriceDetailToolTipText(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" ></asp:Label>
                                            </div>

                                            <div style="vertical-align:middle; display:inline-block; height:32px;">
                                                <asp:Button ID="btnSalePriceOverrideDetail" runat="server" CommandName="SalePriceOverride" CommandArgument='<%# Container.DataItemIndex %>' CssClass="override-button" Width="32px" Height="32px" 
                                                    ToolTip="Override" Visible="<%# ((CalculateStatus() > SharedEnums.ProductionScheduleStatus.Quote) AndAlso UserHasPriceOverridePermission() AndAlso (Not DirectCast(GetDataItem(), LouvreExtraProduct).LouvreExtraPriceIsOverridden)) %>" />

                                                <asp:Button ID="btnSalePriceOverrideDetailRemove" runat="server" CommandName="SalePriceOverrideRemove" CommandArgument='<%# Eval("ID") %>' 
                                                    CssClass="remove-override-button" Width="32px" Height="32px" ToolTip="Remove price override." OnClientClick="return confirm('Removing an overridden price will auto calculate the new price. Are you sure?');"
                                                    Visible="<%# ((CalculateStatus() > SharedEnums.ProductionScheduleStatus.Quote) AndAlso UserHasPriceOverridePermission() AndAlso DirectCast(GetDataItem(), LouvreExtraProduct).LouvreExtraPriceIsOverridden) %>" />
                                            </div>

                                        </ItemTemplate>
                                        <EditItemTemplate>

                                            <div style="vertical-align:middle; display:inline-block;">
                                                <asp:Label ID="lblSalePrice" runat="server" ForeColor="<%# GetSalePriceTextColour(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" Visible="false" 
                                                    Text="<%# GetExtraSalePriceForRow(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" ToolTip ="<%# GetSalePriceDetailToolTipText(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" ></asp:Label>
                                            </div>

                                            <div style="vertical-align:middle; display:inline-block;">
                                                <asp:TextBox ID="txtSalePriceDetail" runat="server" CssClass="form-field" width="100px"
                                                    Text="<%# GetExtraSalePriceForRow(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" ></asp:TextBox>
                                            </div>

                                            <div style="vertical-align:middle; display:inline-block; height:32px;">
                                                <asp:Button ID="btnSalePriceOverrideDetailSave" runat="server" CommandName="update" CommandArgument='<%# Eval("ID") %>' 
                                                    CssClass="save-button" Width="32px" Height="32px" ToolTip="Save Override" Visible="<%# UserHasPriceOverridePermission() %>" />
                                                <asp:Button ID="btnSalePriceOverrideDetailCancel" runat="server" CommandName="cancel" CommandArgument='<%# Eval("ID") %>' 
                                                    CssClass="cancel-button" Width="32px" Height="32px" ToolTip="Cancel Override" />
                                            </div>

                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                                            
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEditExtra" tooltip="Edit" runat="server" CssClass="edit-button" CommandName="EditExtra" CommandArgument='<%# Container.DataItemIndex %>' />
                                            &nbsp;
                                            <asp:Button runat="server" ID="btnDeleteExtra" CommandName="DeleteExtraItem"  
                                                CommandArgument='<%# Eval("ID") %>' CssClass="delete-button" ToolTip="Delete" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnOpeningExtraUpdate" tooltip="Save" runat="server" CssClass="save-button" 
                                                CommandName="Update" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" ValidationGroup="productionscheduleextras" />
                                            &nbsp;           
                                            <asp:Button ID="btnOpeningExtraCancel" tooltip="Cancel" runat="server" CssClass="cancel-button" 
                                                CommandName="Cancel" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <br />
                            
                            <div style="text-align: center;">
                                <asp:Button ID="btnAddExtrasProdSchedule" runat="server" CssClass="form-button update-disable" Text="Add Extra" UseSubmitBehavior="false" Width="120px" />
                            </div>  

                        </div>
                    </asp:Panel>

                    <br />
                    <br />

                    <asp:Panel ID="pnlGRADetails" runat="server" Visible="false">
                        <div style="text-align: center;">
                            <h2>GRA</h2>

                            <asp:GridView ID="dgvGRADetails" runat="server" DataKeyNames="GRAID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" Width="100%" Visible="false">
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnViewDetail" runat="server" Text="Details" CommandName="GRAIDDetail"  CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CssClass="form-button" Width="100px" Height="28px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="GRAID" Visible="false" /> 
                                    <asp:BoundField DataField="ScheduleID" Visible="false"/> 
                                    <asp:BoundField DataField="OpeningNumber" SortExpression="OpeningNumber" HeaderText="# Opening No." />
                                    <asp:BoundField DataField="PanelNumber" SortExpression="PanelNumber" HeaderText=" # Panel No." />
                                    <asp:BoundField DataField="IssueDescription" SortExpression="IssueDescription" HeaderText="Issue Description" />      
                                    <asp:BoundField DataField="CauseOfIssue" SortExpression="CauseOfIssue" HeaderText="Cause Of Issue" /> 
                                    <asp:BoundField DataField="SuggestedAction" SortExpression="SuggestedAction" HeaderText="Suggested Action" />       
                                </Columns>
                            </asp:GridView>  
                        </div>

                        <br />
                        <br />
                    </asp:Panel>


                    <asp:Panel ID="pnlRequirements" runat="server" >
                        <!-- powdercoating -->
                        <div style="text-align: center;">
                            <h2>Powder Coating</h2>

                            <asp:GridView ID="dgvPowdercoat" runat="server" DataKeyNames="AdditionalRequirementsID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333">
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-CssClass="table-cell button-td" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnViewPowdercoat" runat="server" Text="Details" CommandName="PowdercoatDetail"  CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CssClass="form-button" Width="100px" Height="28px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="AdditionalRequirementsID" Visible="false" /> 

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Powder Coater
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPowderCoaterName" runat="server" Text="<%# GetPowderCoaterName(DirectCast(GetDataItem(), AdditionalRequirements)) %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>    
                                                      
                                    <asp:BoundField DataField="StartDate" SortExpression="StartDate" HeaderText="Start Date" DataFormatString="{0:d MMM yyyy}" />                        
                                    <asp:BoundField DataField="ETADate" SortExpression="ETADate" HeaderText="ETA Date" DataFormatString="{0:d MMM yyyy}" />                        
                                    <asp:BoundField DataField="CompleteDate" SortExpression="CompleteDate" HeaderText="Received Date" DataFormatString="{0:d MMM yyyy}" />                        
                                    <asp:BoundField DataField="DescriptionText" SortExpression="DescriptionText" HeaderText="Description" /> 
                                
                                </Columns>
                            </asp:GridView>

                            <br />

                            <div style="text-align: center;">
                                <asp:Button ID="btnAddPowdercoat" runat="server" Text="Add Powdercoat" CssClass="form-button" />
                            </div>

                        </div>

                        <br />
                        <br />

                        <!-- delays -->
                        <div style="text-align: center;">
                            <h2>Additional Requirements</h2>

                            <asp:GridView ID="dgvAdditionalRequirements" runat="server" DataKeyNames="AdditionalRequirementsID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333">
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-CssClass="table-cell button-td" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnViewRequirement" runat="server" Text="Details" CommandName="RequirementDetail"  CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CssClass="form-button" Width="100px" Height="28px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="AdditionalRequirementsID" Visible="false" />
                                     
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Req. Type
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdditionalRequirementsTypeName" runat="server" Text="<%# GetAdditionalRequirementTypeName(DirectCast(GetDataItem(), AdditionalRequirements)) %>" />
                                        </ItemTemplate> 
                                    </asp:TemplateField>
                                                      
                                    <asp:BoundField DataField="StartDate" SortExpression="StartDate" HeaderText="Start Date" DataFormatString="{0:d MMM yyyy}" />                        
                                    <asp:BoundField DataField="ETADate" SortExpression="ETADate" HeaderText="ETA Date" DataFormatString="{0:d MMM yyyy}" />                        
                                    <asp:BoundField DataField="CompleteDate" SortExpression="CompleteDate" HeaderText="Received Date" DataFormatString="{0:d MMM yyyy}" />                        
                                    <asp:BoundField DataField="DescriptionText" SortExpression="DescriptionText" HeaderText="Description" /> 
                                </Columns>
                            </asp:GridView>

                            <br />

                            <div style="text-align: center;">
                                <asp:Button ID="btnAddRequirement" runat="server" Text="Add Requirement" CssClass="form-button" />
                            </div>

                        </div>
            
                    </asp:Panel>
            
                    <br />
                    <br />

                    <div class="bottom-menu-bar">
                        <asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red" Font-Bold="true" />
                        <br />
                        <asp:Button ID="btnCancel" runat="server" CssClass="form-button bottom-menu-button" Text="Close" OnClientClick="return cancelchanges();" UseSubmitBehavior="false" />
                        <asp:Button ID="btnSave" runat="server" CssClass="form-button bottom-menu-button" Text="Save" OnClientClick="return ValidateAll('btnSave');" UseSubmitBehavior="false" />
                        <asp:Button ID="btnPlaceOrder" runat="server" Visible="false" CssClass="form-button bottom-menu-button" Text="Place Order" OnClientClick="return ValidateAll('btnPlaceOrder');" UseSubmitBehavior="false" />
                        <div style="height: 10px;"></div>
                    </div>
                </div>
            </div>

            <uc1:PrivacyScreenDetails runat="server" ID="ucPrivacyScreenDetails" />

            <asp:HiddenField ID="hiddenAddDetailsDummy" runat="server" />
            <ajaxcontroltoolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" BehaviorID="modal4" TargetControlID="hiddenAddDetailsDummy"  PopupControlID="pnlRemakeDetails" BackgroundCssClass="modalBackground">
            </ajaxcontroltoolkit:ModalPopupExtender>

            <asp:Panel ID="pnlRemakeDetails" runat="server" CssClass="modalPopup-louvredetails" Style="display: none; max-height: 95%;" >
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>
                        <asp:Panel ID="pnlAddLouvers" runat="server">
                        <asp:TextBox ID="txtHiddenPSDetailID" runat="server" Visible="false" Text="0"></asp:TextBox>

                        <div class="div-column">
                            <table id="column1" class="form-table" cellspacing="0" summary="">
                                <asp:Panel ID="pnlRemakeType" runat="server" Visible="false">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Remake Type
                                        </td>
                                        <td class="form-field-td-p2 no-wrap">
                                            <asp:DropDownList ID="cboRemakeType" runat="server" CssClass="form-select" Width="150px">
                                                <asp:ListItem Value="0" Text="" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Whole Opening"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Single Panel"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <tr>
                                    <td class="form-label-td-p2 no-wrap" style="text-align: right;">
                                        <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>
                                    </td>

                                    <td class="form-field-td-p2 no-wrap" >
                                        <asp:TextBox ID="txtLocation" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="100" usesubmitbehavior="false" Width="200px" />
                                        <asp:RequiredFieldValidator ID="valrfLocation" runat="server" ControlToValidate="txtLocation" CssClass="validation-text"
                                            ErrorMessage="<br />Please enter a Location." ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="form-label-td-p2 no-wrap" style="text-align: right;">Colour</td>
                                    <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:TextBox ID="txtColour" runat="server" CssClass="form-field" Width="200px" 
                                                        onfocus="this.select();" onblur="AutoCompleteEnsureValues()"  />
                                        <asp:Image ID="imgColour" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />

                                        <ajaxcontroltoolkit:AutoCompleteExtender
                                                        ID="aceColour"
                                                        runat="server"
                                                        TargetControlID="txtColour"
                                                        ServicePath="AutoComplete.asmx"
                                                        ServiceMethod="GetMatchingColourList"
                                                        MinimumPrefixLength="1"
                                                        CompletionInterval="1000"
                                                        CompletionSetCount="0"
                                                        EnableCaching="false"
                                                        CompletionListCssClass="auto-complete-list"
                                                        CompletionListHighlightedItemCssClass="auto-complete-higlighted-item"
                                                        CompletionListItemCssClass="auto-complete-item"
                                                        OnClientItemSelected="AutoCompleteGetColourID"
                                                        OnClientPopulating="AutoCompleteStart"
                                                        OnClientPopulated="AutoCompleteEnd"
                                                        OnClientHiding="AutoCompleteEnd"
                                                        FirstRowSelected="true"></ajaxcontroltoolkit:AutoCompleteExtender>

                                                    <ext:HiddenFieldExtended ID="hdnColourID" runat="server" />
                                                    <ext:HiddenFieldExtended ID="hdnColourName" runat="server" />

                                        <asp:RequiredFieldValidator ID="valrfColour" runat="server" ControlToValidate="hdnColourID" CssClass="validation-text"
                                            ErrorMessage="<br />Please select a Colour." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                              
                                    </td>

                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Product</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                            
                                            <asp:DropDownList ID="cboLouvreProd" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="DLi" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="CL" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgLouvreProd" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="rfvalLouvreProd" runat="server" ControlToValidate="cboLouvreProd" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Product." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                    
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Louvre Type</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:DropDownList ID="cboLouvreType" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                    
                                        </asp:DropDownList>
                                        <asp:Image ID="imgLouvreType" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        <asp:RequiredFieldValidator ID="rfvalLouvreType" runat="server" ControlToValidate="cboLouvreType" CssClass="validation-text"
                                            ErrorMessage="<br />Please select Louvre Type." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                    
                                    </td>

                                    <asp:Panel ID="pnlBiFoldHingedDoor" runat="server">
                                        <tr>
                                            <td class="form-label-td-p2 no-wrap" style="text-align: right;">Open In / Out</td>
                                            <td class="form-field-td-p2 no-wrap" >
                                                
                                                <asp:DropDownList ID="cboBiFoldHingedDoor" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                    <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="In" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Out" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgBiFoldHingedDoor" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                                <asp:RequiredFieldValidator ID="valrfBiFoldHingedDoor" runat="server" ControlToValidate="cboBiFoldHingedDoor" CssClass="validation-text"
                                                    ErrorMessage="<br />Please select Open In / Out." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />   
                                            </td>
                                        </tr>
                                    </asp:Panel>

                                    </tr>
                                        <asp:Panel ID="pnlMakeOpening" runat="server" Visible="false">
                                        <tr>
                                            <td class="form-label-td-p2 no-wrap" style="text-align: right;">Make / Opening</td>
                                            <td class="form-field-td-p2 no-wrap">
                                                <asp:DropDownList ID="cboMakeOpenSize" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                    <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                    <%--<asp:ListItem Text="Make Sizes" Value="1"></asp:ListItem>--%>
                                                    <asp:ListItem Text="Opening Sizes" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgMakeOpenSize" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                                <asp:RequiredFieldValidator ID="rfvalMakeOpening" runat="server" ControlToValidate="cboMakeOpenSize" CssClass="validation-text" Display="Dynamic" EnableClientScript="false" ErrorMessage="&lt;br /&gt;Please select Make / Opening." InitialValue="0" ValidationGroup="details" />
                                            </td>
                                    </tr>
                                            </asp:Panel>

                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Height</td>
                                        <td class="form-field-td-p2" >
                                                
                                            <asp:TextBox ID="txtHeight" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="4" usesubmitbehavior="false" Width="95px" /> (mm)
                                            <asp:Image ID="imgHeight" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfHeight" runat="server" ControlToValidate="txtHeight" CssClass="validation-text"
                                                ErrorMessage="<br />Height is required." ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            <asp:CompareValidator ID="valcompHeight" runat="server" Operator="DataTypeCheck" Type="Integer" CssClass="validation-text"
                                                ControlToValidate="txtHeight" ErrorMessage="<br />Height is invalid." ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            <asp:RangeValidator ID="valrangeHeight" runat="server" Type="Double" CssClass="validation-text"
                                                MinimumValue="1" MaximumValue="9999" ControlToValidate="txtHeight" ValidationGroup="details"
                                                ErrorMessage="<br />Please enter a valid height." Display="Dynamic" EnableClientScript="false" />
                                            <asp:CustomValidator ID="valcustPanelHeight" runat="server"
                                                    Display="Dynamic" Text="" 
                                                    ControlToValidate="txtHeight"
                                                    ValidationGroup="details"
                                                    EnableClientScript="false"
                                                    CssClass="validation-text" />   
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Width</td>
                                        <td class="form-field-td-p2" >
                                                
                                            <asp:TextBox ID="txtWidth" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="6" usesubmitbehavior="false" Width="95px"  /> (mm)
                                            <asp:Image ID="imgWidth" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valreqWidth" runat="server" ControlToValidate="txtWidth" CssClass="validation-text"
                                                ErrorMessage="<br />Width is required." ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            <asp:CompareValidator ID="valcompWidth" runat="server" Operator="DataTypeCheck" Type="Integer" CssClass="validation-text"
                                                ControlToValidate="txtWidth" ErrorMessage="<br />Width is invalid." ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            <asp:RangeValidator ID="valrangeWidth" runat="server" Type="Double" CssClass="validation-text"
                                                MinimumValue="1" MaximumValue="999999" ControlToValidate="txtWidth" ValidationGroup="details"
                                                ErrorMessage="<br />Please enter a valid width." Display="Dynamic" EnableClientScript="false" />
                                            <asp:CustomValidator ID="valcustPanelWidth" runat="server" 
                                                Display="Dynamic" Text="" 
                                                ValidationGroup="details"
                                                ControlToValidate="txtWidth"
                                                EnableClientScript="false"
                                                CssClass="validation-text" />
                                              
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">No of Panels</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:DropDownList ID="cboNoOfPanels" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                            <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Image ID="imgNoOfPanels" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Blade Size</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                                <asp:DropDownList ID="cboBladeSize" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Text="90" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="150" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgBladeSize" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            
                                        </td>
                                    </tr>

                                    <asp:Panel ID="pnlPanelTopRail" runat="server">
                                        <tr>
                                            <td class="form-label-td-p2 no-wrap" style="text-align: right;">Panel Top Rail</td>
                                            <td class="form-field-td-p2 no-wrap" >
                                                
                                                <asp:DropDownList ID="cboPanelTopRail" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                    <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgPanelTopRail" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                                <asp:RequiredFieldValidator ID="valrfPanelTopRail" runat="server" ControlToValidate="cboPanelTopRail" CssClass="validation-text"
                                                    ErrorMessage="<br />Please select Panel Top Rail." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            
                                            </td>
                                        </tr>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlPanelBottomRail" runat="server">
                                        <tr>
                                            <td class="form-label-td-p2 no-wrap" style="text-align: right;">Panel Bottom Rail</td>
                                            <td class="form-field-td-p2 no-wrap" >
                                                
                                                <asp:DropDownList ID="cboPanelBottomRail" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                    <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgPanelBottomRail" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                                <asp:RequiredFieldValidator ID="valrfPanelBottomRail" runat="server" ControlToValidate="cboPanelBottomRail" CssClass="validation-text"
                                                    ErrorMessage="<br />Please select Panel Bottom Rail." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            
                                            </td>
                                        </tr>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlPanelMidRail" runat="server">
                                        <tr>
                                            <td class="form-label-td-p2 no-wrap" style="text-align: right;">Panel Mid Rail</td>
                                            <td class="form-field-td-p2 no-wrap" >
                                                
                                                <asp:DropDownList ID="cboPanelMidRail" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                    <asp:ListItem Selected="true" Text="None" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgPanelMidRail" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            
                                            </td>
                                        </tr>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlMidRailHeight" runat="server">
                                        <tr>
                                            <td class="form-label-td-p2 no-wrap" style="text-align: right;">
                                                <asp:Label ID="MidRailLabel" runat="server" Text="Mid Rail Height" Visible="true" />
                                            </td>
                                            <td class="form-field-td-p2 no-wrap" >
                                                
                                                <asp:TextBox ID="txtMidRailHeight" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="4" usesubmitbehavior="false" Width="100px" />
                                                    (mm)
                                                <asp:Image ID="imgMidRailHeight" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />

                                                <asp:RequiredFieldValidator ID="valrfMidRailHeight" runat="server" ControlToValidate="txtMidRailHeight" CssClass="validation-text"
                                                    ErrorMessage="<br />Please enter Mid Rail Height." ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                                <asp:CustomValidator ID="valcustMidRailHeight" runat="server" CssClass="validation-text" ControlToValidate="txtMidRailHeight"
                                                    ErrorMessage="" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" ValidateEmptyText="true" />
                                            </td>
                                        <tr />
                                    </asp:Panel>

                            </table>
                        </div>

                        <div class="div-column">
                            <table id="column2" class="form-table" cellspacing="0" summary="">

                                <asp:Panel ID="pnlEndCapColour" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">DLi End Plug Colour</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:DropDownList ID="cboEndCapColour" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                            <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Black" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="White" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Image ID="imgEndCapColour" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfEndCapColour" runat="server" ControlToValidate="cboEndCapColour" CssClass="validation-text"
                                                ErrorMessage="<br />Please select End Plug Colour." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <%--added by surendra ticket #63195--%>
                                <asp:Panel ID="pnlBladeClipColour" runat="server">
                                <tr>
                                    <td class="form-label-td-p2 no-wrap" style="text-align: right;">Blade Clip Colour</td>
                                    <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:DropDownList ID="cboBladeClipColour" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                            <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Black" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="White" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Image ID="imgBladeClipColour" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        <asp:RequiredFieldValidator ID="varfBladeClipColour" runat="server" ControlToValidate="cboBladeClipColour" CssClass="validation-text"
                                            ErrorMessage="<br />Please select Blade Clip Colour." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />                                           
                                    </td>
                                </tr>
                                </asp:Panel>
                                <%--added by surendra ticket #63195--%>
                                <asp:Panel ID="pnlPileColour" runat="server">
                                <tr>  
                                    <td class="form-label-td-p2 no-wrap" style="text-align: right;">Pile Colour</td>
                                    <td class="form-field-td-p2 no-wrap" >
                                                
                                    <asp:DropDownList ID="cboPileColour" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                        <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Black" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Grey" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Image ID="imgPileColour" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                    <asp:RequiredFieldValidator ID="valrfPileColour" runat="server" ControlToValidate="cboPileColour" CssClass="validation-text"
                                        ErrorMessage="<br />Please select Pile Colour." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            
                                    </td>
                                </tr>
                                 </asp:Panel>
                                <asp:Panel ID="pnlTopTrackType" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Top Track Type</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:DropDownList ID="cboTopTrackType" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                            <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Image ID="imgTopTrackType" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        <asp:RequiredFieldValidator ID="valrfTopTrackType" runat="server" ControlToValidate="cboTopTrackType" CssClass="validation-text"
                                            ErrorMessage="<br />Please select Top Track Type." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                              
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlBottomTrackType" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Bottom Track Type</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboBottomTrackType" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="12mm Dli" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="25mm CL" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgBottomTrackType" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfBottomTrackType" runat="server" ControlToValidate="cboBottomTrackType" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Bottom Track Type." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                              
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlCurvedTrack" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Curved Track</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboCurvedTrack" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgCurvedTrack" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />                                            
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlCurvedTrackMaxDeflection" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Max Deflection</td>
                                        <td class="form-field-td-p2 no-wrap" >

                                            <asp:TextBox ID="txtCurvedTrackMaxDeflection" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="6"
                                                usesubmitbehavior="false" Width="100px" /> (mm)
                                            <asp:Image ID="imgCurvedTrackMaxDeflection" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:CustomValidator ID="valcustCurvedTrackMaxDeflection" runat="server" CssClass="validation-text" ControlToValidate="txtCurvedTrackMaxDeflection"
                                                    ErrorMessage="" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" ValidateEmptyText="true" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlExtraTrack" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Extra Track</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:TextBox ID="txtExtraTrack" runat="server" AutoPostBack="true" CssClass="form-field" MaxLength="6" 
                                                usesubmitbehavior="false" Width="100px" /> (mm)

                                            <asp:Image ID="imgExtraTrack" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlFlushBoltsTop" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Flush Bolts Top</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboFlushBoltsTop" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="None" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Short 235mm" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Short 235mm with extended tip" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="Long 430mm" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Long 430mm with extended tip" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Ex Long 890mm" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Ex Long 890mm with extended tip" Value="7"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgFlushBoltsTop" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlFlushBoltsBottom" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Flush Bolts Bottom</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboFlushBoltsBottom" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="None" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Short 235mm" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Short 235mm with extended tip" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="Long 430mm" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Long 430mm with extended tip" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Ex Long 890mm" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Ex Long 890mm with extended tip" Value="7"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgFlushBoltsBottom" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlFlushBoltsPosition" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Flush Bolts L/R</td>
                                        <td class="form-field-td-p2 no-wrap" >

                                        <asp:DropDownList ID="cboFlushBoltsPosition" runat="server" AutoPostBack="true" CssClass="form-select" Width="50px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="L" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="R" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgFlushBoltsPosition" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                        <asp:RequiredFieldValidator ID="valrfFlushBoltsPosition" runat="server" ControlToValidate="cboFlushBoltsPosition" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Flush Bolts Position." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" /> 
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlLockOptions" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Special Lock Options</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboLockOptions" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="None" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Handle Lock – Inside &amp; Out (Tasman)" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Premium Handle Lock – Inside &amp; Out (Finista/Killara)" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Knobset Lock – Inside &amp; Out" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Twin Flush Bolt – Locking" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Twin Flush Bolt – Non Locking" Value="5"></asp:ListItem>

                                            </asp:DropDownList>
                                            <asp:Image ID="imgLockOptions" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlBladeLocks" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Blade Locks</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboBladeLocks" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgBladeLocks" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfBladeLocks" runat="server" ControlToValidate="cboBladeLocks" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Blade Locks." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlCChannel" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">T&B Fixing Channel</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboCChannel" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgCChannel" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfCChannel" runat="server" ControlToValidate="cboCChannel" CssClass="validation-text"
                                                ErrorMessage="<br />Please select T&B Fixing Channel." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlFixedPanelSides" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Side Fixing</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboFixedPanelSides" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="None" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Angle Sides" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="25mm Channel Sides" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="50mm Sides" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgFixedPanelSides" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />        
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlHinges" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Hinges</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                               
                                            <asp:DropDownList ID="cboHinges" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px" Visible="true" >
                                                <asp:ListItem Selected="true" Text="None" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgHinges" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" Visible="false" />
                                                    
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlWinder" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Winder</td>
                                        <td class="form-field-td-p2 no-wrap" >

                                            <asp:DropDownList ID="cboWinder" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px" Visible="false" >
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Left" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgWinder" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" Visible="false" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                            </table>
                        </div>

                        <div class="div-column">
                            <table id="column3" class="form-table" cellspacing="0" summary="">

                                <asp:Panel ID="pnlHChannel" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">H Joiner</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboHChannel" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgHChannel" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfHChannel" runat="server" ControlToValidate="cboHChannel" CssClass="validation-text"
                                                ErrorMessage="<br />Please select H Joiner." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                            
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlLReveal" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">L Reveal</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboLReveal" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Reveal 3 Sided" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Reveal 4 Sided" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Facefit 3 Sided" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Facefit 4 Sided" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="No Frame" Value="5"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgLReveal" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfLReveal" runat="server" ControlToValidate="cboLReveal" CssClass="validation-text"
                                                ErrorMessage="<br />Please select L Reveal." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                              
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlZReveal" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Z Reveal</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboZReveal" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Reveal 3 Sided" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Reveal 4 Sided" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgZReveal" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfZReveal" runat="server" ControlToValidate="cboZReveal" CssClass="validation-text"
                                                ErrorMessage="<br />Please select L Reveal." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                              
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlInsertTop" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">
                                                <asp:Label ID="lblMainInsert" runat="server" Text="Main Insert" Visible="true" />
                                        </td>
                                        <td class="form-field-td-p2 no-wrap" >
                                            <asp:DropDownList ID="cboInsertTop" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Horizontal Blade" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Vertical Blade" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Clear Glass" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Grey Glass" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgInsertTop" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfInsertTop" runat="server" ControlToValidate="cboInsertTop" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Insert." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlBladeOperation" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">
                                            <asp:Label ID="lblOperationMain" runat="server" Text="Main Operation" Visible="true" />
                                        </td>
                                        <td class="form-field-td-p2 no-wrap" >
                                            
                                            <asp:DropDownList ID="cboBladeOperation" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Standard" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Fixed Open" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Fixed Closed " Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgBladeOperation" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfBladeOperation" runat="server" ControlToValidate="cboBladeOperation" CssClass="validation-text"
                                                    ErrorMessage="<br />Please select Blade Operation." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlInsertBottom" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Bottom Insert</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboInsertBottom" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Horizontal Blade" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Vertical Blade" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Clear Glass" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Grey Glass" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgInsertBottom" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfInsertBottom" runat="server" ControlToValidate="cboInsertBottom" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Bottom Insert." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                                    
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlBladeOperationBottom" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Bottom Operation</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                            <asp:DropDownList ID="cboBladeOperationBottom" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Standard" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Fixed Open" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Fixed Closed " Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgBladeOperationBottom" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfBladeOperationBottom" runat="server" ControlToValidate="cboBladeOperationBottom" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Bottom Operation." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                                
                                        </td>    
                                    </tr>
                                </asp:Panel>
                                <%--Added by surendra ticket#63195--%>
                                <asp:Panel ID="pnlFlyScreen" runat="server">
                                <tr>
                                    <td class="form-label-td-p2 no-wrap" style="text-align: right;">Fly Screen</td>
                                    <td class="form-field-td-p2 no-wrap" >
                                                
                                        <asp:DropDownList ID="cboFlyScreen" runat="server" CssClass="form-select" Width="150px">
                                            <asp:ListItem Selected="true" Text="No" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Image ID="imgFlyScreen" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                                    
                                    </td>
                                </tr>
                                </asp:Panel>
                                <asp:Panel ID="pnlStacker" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Stacker Bay Location</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboStacker" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Left" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgStacker" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfStacker" runat="server" ControlToValidate="cboStacker" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Stacker Location." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                                    
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlSlide" runat="server">
                                    <tr>
                                        <td class="form-label-td-p2 no-wrap" style="text-align: right;">Slide Track Spacing</td>
                                        <td class="form-field-td-p2 no-wrap" >
                                                
                                            <asp:DropDownList ID="cboSlide" runat="server" AutoPostBack="true" CssClass="form-select" Width="150px">
                                                <asp:ListItem Selected="true" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Open Blade Track Spacing" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Shut Blade Track Spacing" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Image ID="imgSlide" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                            <asp:RequiredFieldValidator ID="valrfSlide" runat="server" ControlToValidate="cboSlide" CssClass="validation-text"
                                                ErrorMessage="<br />Please select Slide Track Spacing." InitialValue="0" ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                                    
                                        </td>
                                    </tr>
                                </asp:Panel>

                            </table>
                        </div>

                        <div style="clear:both;">
                            <table class="form-table" cellspacing="0" summary="">
                                <tr>
                                    <td class="form-label-td-p2" style="width: 13%; text-align: right;">
                                        Special Requirements
                                    </td>
                                    <td class="form-field-td-p2 no-wrap" style="width: 87%">
                                        <asp:TextBox ID="txtSpecialRequirements" runat="server" CssClass="form-field" TextMode="MultiLine" width="92%" Height="50px" ></asp:TextBox>
                                        <asp:Image ID="imgSpecialRequirements" runat="server" Height="16px" CssClass="info-tooltip" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        </asp:Panel>
                        <table class="form-table grey-background" cellspacing="0" summary="">
                            <asp:Panel ID="pnlAddLouversDetails" runat="server">
                            <tr>
                                <td colspan="6">
                                    <asp:Panel ID="pnlVerticalOpen" runat="server" Visible="false" >
                                        <table width="100%">
                                            <tr>
                                                <td class="form-field-td-p2" style="width: 17%; text-align: right;">
                                                    <div style="display:inline-block; text-align: center;">
                                                        <asp:Image ID="imgVerticalLeft" runat="server" ImageUrl="Images/VerticalOpenL.png" />
                                                        <br />
                                                        <asp:RadioButton ID="rdoOpenLeft" runat="server" Checked="false" GroupName="Options" Text="Open Left" AutoPostBack="true" />
                                                        <asp:Image ID="imgVerticalLeftInfo" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                                    </div>
                                                </td>
                                                <td class="form-field-td-p2" style="width: 17%; text-align: left; background-color: white !important;">
                                                    <div style="display:inline-block; text-align: center;">
                                                        <asp:Image ID="imgVerticalRight" runat="server" ImageUrl="Images/VerticalOpenR.png" CssClass="vertical-right-img" />
                                                        <br />
                                                        <asp:RadioButton ID="rdoOpenRight" runat="server" Checked="false" GroupName="Options" Text="Open Right" AutoPostBack="true" />
                                                        <asp:Image ID="imgVerticalRightInfo" runat="server" Height="16px" ImageUrl="images/info_button_icon16x16_t35.png" Width="16px" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align:center;" colspan="6">
                                                    <asp:CustomValidator ID="valcustOpenDirection" runat="server" cssclass="validation-text"
                                                        ErrorMessage="<br />Please select Open Direction." ValidationGroup="details" Display="Dynamic" EnableClientScript="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                                </asp:Panel>
                            <tr>
                                <td class="form-submit-td" style="text-align: center;">
                                    <asp:UpdatePanel ID="pnlupdateExtras" runat="server" UpdateMode="Conditional" >
                                        <ContentTemplate>
                                            <asp:GridView ID="gdvOpeningExtras" runat="server" EnableTheming="false" DataKeyNames="ID" CssClass="extras-table" AutoGenerateColumns="false" 
                                                CellPadding="4" ForeColor="#333333" >
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <FooterStyle BackColor="Green" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#0059A9" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#999999" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:label ID="lblID" runat="server" CssClass="form-select" Text='<%# Eval("ID") %>' Width ="95%" Visible="false">
                                                            </asp:label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Extra
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlExtraName" runat="server" CssClass="form-select extras-field" Width ="95%" AutoPostBack="false" Enabled="false" >
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlExtraName" runat="server" CssClass="form-select extras-field" Width ="95%" AutoPostBack="true" OnSelectedIndexChanged="ddlExtraName_SelectedIndexChanged" Enabled="true" >
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="valrfExtraName" runat="server" ControlToValidate="ddlExtraName" cssclass="validation-text"
                                                                ErrorMessage="<br />Please select an Extra Name." InitialValue="0" ValidationGroup="extras" Display="Dynamic" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Qty
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQty" Text='<%# Eval("Quantity") %>' CssClass="extras-field" style="text-align: center" runat="server" Width="70px" MaxLength="4" Enabled="false"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtQty" Text='<%# Eval("Quantity") %>' CssClass="extras-field" style="text-align: center" runat="server" Width="70px" MaxLength="4" Enabled="true"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="valrfQty" runat="server" ControlToValidate="txtQty" cssclass="validation-text"
                                                                ErrorMessage="<br />Qty is required." ValidationGroup="extras" Display="Dynamic" />
                                                            <asp:CompareValidator ID="valcompQty" runat="server" Operator="DataTypeCheck" Type="Integer" cssclass="validation-text"
                                                                ControlToValidate="txtQty" ErrorMessage="<br />Qty is invalid." ValidationGroup="extras" Display="Dynamic" />
                                                            <asp:RangeValidator runat="server" Type="Double" cssclass="validation-text"
                                                                MinimumValue="1" MaximumValue="9999" ControlToValidate="txtQty" ValidationGroup="extras"
                                                                ErrorMessage="<br />Qty must be greater than 0." Display="Dynamic" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Length (mm)
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtCutLength" Text='<%# Eval("CutLength") %>' Visible="<%# GetCutLengthVisibleForExtra(DirectCast(GetDataItem(), LouvreExtraProduct)) %>" 
                                                                CssClass="extras-field" style="text-align: center" runat="server" Width="70px" MaxLength="4" Enabled="false"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>

                                                                <asp:TextBox ID="txtCutLength" Text='<%# Eval("CutLength") %>' Visible="<%# GetCutLengthVisibleForExtra(DirectCast(GetDataItem(), LouvreExtraProduct)) %>"
                                                                     CssClass="extras-field" style="text-align: center" runat="server" Width="70px" MaxLength="4" Enabled="true"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="valrfCutLength" runat="server" ControlToValidate="txtCutLength" cssclass="validation-text"
                                                                    Enabled="<%# GetCutLengthVisibleForExtra(DirectCast(GetDataItem(), LouvreExtraProduct)) %>"
                                                                    ErrorMessage="<br />Length is required." ValidationGroup="extras" Display="Dynamic" />
                                                                <asp:CompareValidator ID="valcompCutLength" runat="server" Operator="DataTypeCheck" Type="Integer" cssclass="validation-text"
                                                                    Enabled="<%# GetCutLengthVisibleForExtra(DirectCast(GetDataItem(), LouvreExtraProduct)) %>"
                                                                    ControlToValidate="txtCutLength" ErrorMessage="<br />Length is invalid." ValidationGroup="extras" Display="Dynamic" />
                                                                <asp:CustomValidator ID="valcustCutLengthMax" runat="server"
                                                                    Enabled="<%# GetCutLengthVisibleForExtra(DirectCast(GetDataItem(), LouvreExtraProduct)) %>"  
                                                                    OnServerValidate="valcustCutLengthMax_ServerValidate"
                                                                    Display="Dynamic" Text="<br />Length exceeds extra maximum." 
                                                                    ValidationGroup="extras"
                                                                    ControlToValidate="txtCutLength"
                                                                    cssclass="validation-text" /> 

                                                        </EditItemTemplate>
                                                    </asp:TemplateField> 
                                                                            
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnEditExtra" tooltip="Edit" runat="server" CssClass="edit-button" CommandName="Edit"  />
                                                            &nbsp;
                                                            <asp:Button runat="server" ID="btnDeleteExtra" CommandName="DeleteExtraItem"  
                                                                CommandArgument='<%# Eval("ID") %>' CssClass="delete-button" ToolTip="Delete" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Button ID="btnOpeningExtraUpdate" tooltip="Save" runat="server" CssClass="save-button" 
                                                                CommandName="Update" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" ValidationGroup="extras" />
                                                            &nbsp;           
                                                            <asp:Button ID="btnOpeningExtraCancel" tooltip="Cancel" runat="server" CssClass="cancel-button" 
                                                                CommandName="Cancel" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                            <asp:Button ID="btnAddExtra" runat="server" CssClass="form-button update-disable" Text="Add Extra" UseSubmitBehavior="false" Width="120px" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-submit-td" style="text-align: center;">
                                    <asp:ValidationSummary ID="valSummary" CssClass="valSummary" DisplayMode="BulletList" HeaderText="Please provide the missing information in the form above before saving the opening.<br /><br />" 
                                    ForeColor="" ValidationGroup="details" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-submit-td" style="text-align: center;">
                                    <asp:UpdatePanel ID="pnlupdateSaveCancel" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                                <asp:Button ID="btnCancelDetails" runat="server" CssClass="form-button update-disable" Text="Cancel" UseSubmitBehavior="false" Width="100px" />&nbsp;&nbsp;
                                                <asp:Button ID="btnSaveDetails" runat="server" CssClass="form-button update-disable" ValidationGroup="details" Text="Save" UseSubmitBehavior="false" Width="100px" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-label-td-p2" style="text-align: center;">
                                    <asp:Label ID="lblShutterStatus" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <div class="hider">
                            <div class="hider-loading">
                                <img src="Images/loading-gif-transparent-green.gif" alt="Loading..." width="100px" height="100px" />
                            </div>
                        </div>
                                    
                    </ContentTemplate>

                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSaveDetails" />
                        <asp:PostBackTrigger ControlID="btnCancelDetails" />
                    </Triggers>

                </asp:UpdatePanel>
            </asp:Panel>



            <div class="hider">
                <div class="hider-loading">
                    <img src="Images/loading-gif-transparent-green.gif" alt="Loading..." width="100px" height="100px" />
                </div>
            </div>

            <ajaxcontroltoolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnViewNotes" PopupControlID="pnlNotesDetails" BackgroundCssClass="modalBackground">
            </ajaxcontroltoolkit:ModalPopupExtender>

            <asp:Panel ID="pnlNotesDetails" runat="server" CssClass="modalPopup" Style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h2>Job Notes</h2>
                        <ajaxcontroltoolkit:Accordion ID="acc1" runat="server" FadeTransitions="true"  HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                            <HeaderTemplate>
                                <%# GetNoteHeaderText(DirectCast(Container.DataItem, ProdScheduleNote))%>
                            </HeaderTemplate>
                            <ContentTemplate>
                                Note Text: <%#DataBinder.Eval(Container.DataItem, "NoteDetails")%>
                            </ContentTemplate>
                        </ajaxcontroltoolkit:Accordion>

                        <br />

                        <div style="text-align: center;">
                            <asp:Button ID="btnAddNewNote" runat="server" Text="Add New Note" CssClass="form-button" />
                        </div>

                        <br />
                        <asp:Panel ID="pnlAddNote" runat="server" Visible="false">
                            <table class="form-table" cellspacing="0" summary="">
                                <tr>
                                    <td class="form-field-td-p2" style="text-align: right;">
                                        Note:&nbsp;&nbsp; 
                                    </td>
                                    <td class="form-field-td-p2">
                                        <asp:TextBox ID="txtNewNoteText" runat="server" CssClass="form-field" TextMode="MultiLine"></asp:TextBox>
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td colspan="2" class="form-field-td-p2">
                                        <div style="text-align: center;">
                                            <asp:CheckBox runat="server" ID="chkNoteVisible" Text="Visible To Customer" />
                                        </div>
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td class="form-field-td-p2">
                                        <div style="float: right;">
                                            <asp:Button runat="server" ID="btnCancelNote" Text="Cancel Note" CssClass="form-button" />
                                        </div>
                                    </td>
                                    <td class="form-field-td-p2">
                                        <div style="float: left;">
                                            <asp:Button runat="server" ID="btnSaveNote" Text="Save Note" CssClass="form-button" />
                                        </div>
                                    </td>
                                </tr>
                            </table>

                        </asp:Panel>

                        <br />
                        <br />
                        <div style="text-align: center;">
                            <asp:Button runat="server" ID="btnCloseNotes" Text="Close" CssClass="form-button" />
                        </div>

                    </contentTemplate>

                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnCloseNotes" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>


            <asp:HiddenField ID="hiddenPowderCoatPopupDummy" runat="server" />

            <ajaxcontroltoolkit:ModalPopupExtender ID="mpePowderCoat" runat="server" TargetControlID="hiddenPowderCoatPopupDummy" PopupControlID="pnlPowdercoatDetails" BackgroundCssClass="modalBackground">
            </ajaxcontroltoolkit:ModalPopupExtender>

            <asp:Panel ID="pnlPowdercoatDetails" runat="server" CssClass="modalPopup" Style="display:none;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtHidPCRequirementsID" runat="server" Visible="false" Text="0"></asp:TextBox>
                        <table class="form-table" cellspacing="0" summary="">
                            <tr>
                                <td class="form-field-td-p2">Powdercoater
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:DropDownList ID="cboPowdercoater" runat="server" Width="150px" CssClass="form-select" ></asp:DropDownList>
                                </td>
                                <td class="form-field-td-p2">                                    
                                </td>
                                <td class="form-field-td-p2">                                    
                                </td>
                            </tr>
                            <tr>
                                <td class="form-field-td-p2">Description
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtPCDescription" runat="server" CssClass="form-field" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td class="form-field-td-p2">                                    
                                </td>
                                <td class="form-field-td-p2">                                    
                                </td>
                            </tr>
                            <tr>
                                <td class="form-field-td-p2">Pick Date</td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtPCPickDate" runat="server" CssClass="form-field" Width="120px" ></asp:TextBox>
                                </td>
                                <td class="form-field-td-p2">ETA Date
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtPCETADate" runat="server" CssClass="form-field" Width="120px" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-field-td-p2">Despatch Date
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtPCStartDate" runat="server" CssClass="form-field" Width="120px" ></asp:TextBox>
                                </td>
                                <td class="form-field-td-p2">                                    
                                    Return Date
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtPCCompleteDate" runat="server" CssClass="form-field" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-field-td-p2">Purchase Order No
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtPCPurchaseOrder" runat="server" CssClass="form-field" Width="120px" ></asp:TextBox>
                                </td>
                                <td class="form-field-td-p2">Cost Price
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtPCCostPrice" runat="server" CssClass="form-field" Width="120px" ></asp:TextBox>
                                </td>
                            </tr>
                        </table>

                         <div  style="padding:15px 7px 15px 7px">
                             <asp:Label ID="lblStockList2Header" runat="server" Text="Stock List" Font-Size="X-Large"  ForeColor="#0059A9"></asp:Label>
                                           <asp:GridView ID="dgvStockList2" runat="server" DataKeyNames="AdditionalRequirementsID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>                         
                                    <asp:BoundField DataField="AdditionalRequirementsID" Visible="false" /> 
                                    <asp:BoundField DataField="Description" HeaderText="Article Description"/> 
                                    <asp:BoundField DataField="ActualQuantity" SortExpression="ShutterID" HeaderText="Quantity Picked" />                                     
                                </Columns>
                            </asp:GridView>  
                        </div>   

                        <table class="form-table" cellspacing="0" summary="">
                            <tr>
                                <td class="form-submit-td" >
                                    <div style="float: right;">
                                        <asp:Button runat="server" ID="btnCancelPowdercoat" text="Cancel" CssClass="form-button" />
                                    </div>
                                </td>
                                <td class="form-submit-td" >
                                    <div style="float: left;">
                                        <asp:Button runat="server" ID="btnSavePowdercoat" Text="Save" CssClass="form-button" />
                                    </div>
                                </td>
                            </tr>
                        </table>

                    </ContentTemplate>

                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSavePowdercoat" />
                        <asp:PostBackTrigger ControlID="btnCancelPowdercoat" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>

            <asp:HiddenField ID="hiddenAdditionalRequirementsPopupDummy" runat="server" />

            <ajaxcontroltoolkit:ModalPopupExtender ID="mpeAdditionalRequirements" runat="server" TargetControlID="hiddenAdditionalRequirementsPopupDummy" PopupControlID="pnlRequirementDetails" BackgroundCssClass="modalBackground">
            </ajaxcontroltoolkit:ModalPopupExtender>

            <asp:Panel ID="pnlRequirementDetails" runat="server" CssClass="modalPopup" Style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtHidRequirementsID" runat="server" Visible="false" Text="0"></asp:TextBox>
                        <table class="form-table" cellspacing="0" summary="">
                            <tr>
                                <td class="form-field-td-p2">Requirement Type
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:DropDownList ID="cboRequirementType" runat="server" Width="150px" CssClass="form-select" ></asp:DropDownList>
                                </td>
                                <td class="form-field-td-p2">                                    
                                </td>
                                <td class="form-field-td-p2">                                    
                                </td>
                            </tr>
                            <tr>
                                <td class="form-field-td-p2">Description
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtRequirementDescription" runat="server" CssClass="form-field" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td class="form-field-td-p2">                                    
                                </td>
                                <td class="form-field-td-p2">                                    
                                </td>
                            </tr>
                            <tr>
                                <td class="form-field-td-p2">Pick Date
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtRequirementPickDate" runat="server" CssClass="form-field" Width="120px" ></asp:TextBox>
                                </td>
                                <td class="form-field-td-p2">ETA Date
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtRequirementETADate" runat="server" CssClass="form-field" Width="120px" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-field-td-p2">Despatch Date</td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtRequirementStartDate" runat="server" CssClass="form-field" Width="120px" ></asp:TextBox>
                                </td>
                                <td class="form-field-td-p2" colspan="1" >                                    
                                    Return Date                                    
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtRequirementCompleteDate" runat="server" CssClass="form-field" Width="120px" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-field-td-p2">Purchase Order No
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtRequirementPurchaseOrder" runat="server" CssClass="form-field" Width="120px" ></asp:TextBox>
                                </td>
                                <td class="form-field-td-p2">Cost Price
                                </td>
                                <td class="form-field-td-p2">
                                    <asp:TextBox ID="txtRequirementCostPrice" runat="server" CssClass="form-field" Width="120px" ></asp:TextBox>
                                </td>
                            </tr>
                       </table>
                        <div  style="padding:15px 7px 15px 7px">
                             <asp:Label ID="lblStockListHeader" runat="server" Text="Stock List" Font-Size="X-Large"  ForeColor="#0059A9"></asp:Label>
                                           <asp:GridView ID="dgvStockList" runat="server" DataKeyNames="AdditionalRequirementsID" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <FooterStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#0059A9" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>                         
                                    <asp:BoundField DataField="AdditionalRequirementsID" Visible="false" /> 
                                    <asp:BoundField DataField="Description" HeaderText="Article Description"/> 
                                    <asp:BoundField DataField="ActualQuantity" SortExpression="ShutterID" HeaderText="Quantity Picked" />                                     
                                </Columns>
                            </asp:GridView>  
                        </div>                            

                        <table class="form-table" cellspacing="0" summary="">
                            <tr>
                                <td class="form-submit-td" >
                                    <div style="float: right;">
                                        <asp:Button runat="server" ID="btnCancelRequirement" text="Cancel" CssClass="form-button" />
                                    </div>
                                </td>
                                <td class="form-submit-td" >
                                    <div style="float: left;">
                                        <asp:Button runat="server" ID="btnSaveRequirement" Text="Save" CssClass="form-button" />
                                    </div>
                                </td>
                            </tr>
                        </table>

                    </ContentTemplate>

                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSaveRequirement" />
                        <asp:PostBackTrigger ControlID="btnCancelRequirement" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>

            <ajaxcontroltoolkit:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="btnOutstanding" PopupControlID="pnlOutstandingItems" BackgroundCssClass="modalBackground">
            </ajaxcontroltoolkit:ModalPopupExtender>

            <asp:Panel ID="pnlOutstandingItems" runat="server" CssClass="modalPopup" Style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h2>Outstanding Items For Acceptance</h2>

                        <table class="form-table" cellspacing="0" summary="">
                            <tr>
                                <td class="form-field-td-p2" colspan="2">
                                    Confirm all the items below have been attended to before completing acceptance
                                </td>
                            </tr>
                            <tr>
                                <td class="form-field-td-p2" colspan="2">
                                    <asp:Label ID="lblAcceptance" runat="server" CssClass="form-label-td"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-submit-td" >
                                    <div style="float: right;">
                                        <asp:Button runat="server" ID="btnCancelOutstanding" text="Cancel" CssClass="form-button" />
                                    </div>
                                </td>
                                <td class="form-submit-td" >
                                    <div style="float: left;">
                                        <asp:Button runat="server" ID="btnConfirmOutstanding" Text="Confirm" CssClass="form-button" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>

                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnConfirmOutstanding" />
                        <asp:PostBackTrigger ControlID="btnCancelOutstanding" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
        
            <ajaxcontroltoolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnViewCreditDetails" PopupControlID="pnlCustomerCredit" BackgroundCssClass="modalBackground">
            </ajaxcontroltoolkit:ModalPopupExtender>

            <asp:Panel ID="pnlCustomerCredit" runat="server" CssClass="modalPopup" Style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h2>Customer Credit Details</h2>

                        <table class="form-table" cellspacing="0" summary="">
                            <tr>
                                <td class="form-field-td-p2" colspan="2">
                                    <asp:Label ID="lblCreditDetails" runat="server" CssClass="form-label-td"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-submit-td" >
                                    <div style="float: right;">
                                        <asp:Button runat="server" ID="btnCreditCancel" text="Cancel" CssClass="form-button" />
                                    </div>
                                </td>
                                <td class="form-submit-td" >
                                    <div style="float: left;">
                                        <asp:Button runat="server" ID="btnCreditApprove" Text="Approve" CssClass="form-button" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>

                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnCreditApprove" />
                        <asp:PostBackTrigger ControlID="btnCreditCancel" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
        
        </ContentTemplate>
        </asp:UpdatePanel>
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
    <div id="dialog-confirm-delete" title="Please Confirm" >
        <p id="dialog-confirm-delete-message" style="text-align: left;"></p>
    </div>

    <div runat="server" id="DivCur1" style="display:none;">
        ID:<asp:TextBox ID="txtId" runat="server" TabIndex="-1" Text="0" />
        <br />
        CustomerId:<asp:TextBox ID="txtCustomerId" runat="server" TabIndex="-1" Text="0" />
        <br />
        SyBizCustomerId:<asp:TextBox ID="txtSybizCustomerID" runat="server" TabIndex="-1" Text="0" />
        <br />
        JobNumber:<asp:TextBox ID="txtJobNumber" runat="server" TabIndex="-1" Text="0" />
        <br />
        ViewType:<asp:TextBox ID="txtViewType" runat="server" TabIndex="-1" Text="0" />
        <br />
        PSDetailID:<asp:TextBox ID="txtPSDetailID" runat="server" TabIndex="-1" Text="0" />
        <br />
        ProductTypeID<asp:TextBox id="txtProductTypeID" runat="server" TabIndex="-1" Text="0"></asp:TextBox>
        <br />
        Slide:<asp:TextBox ID="txtSlide" runat="server" Text="0" TabIndex="-1"></asp:TextBox>
        <br />
        MidRail:<asp:TextBox ID="txtMR" runat="server" Text="0" TabIndex="-1"></asp:TextBox>
        <br />
        OtherColour:<asp:TextBox ID="txtOthColour" runat="server" Text="0" TabIndex="-1"></asp:TextBox>
        <br />
        Stacker:<asp:TextBox ID="txtStack" runat="server" Text="0" TabIndex="-1"></asp:TextBox>  
        <br />
        No Of Panel:<asp:TextBox ID="txtNoPanel" runat="server" Text="0" TabIndex="-1"></asp:TextBox>   
        <br />
        RoomLocation:<asp:TextBox ID="txtRL" runat="server" Text="0" TabIndex="-1"></asp:TextBox>        
        <br />
        Open:<asp:TextBox ID="txtOpen" runat="server" Text="0" TabIndex="-1"></asp:TextBox>  
        <br />
        Pivot:<asp:TextBox ID="txtPivot" runat="server" Text="0" TabIndex="-1"></asp:TextBox>    
        <br />
        Channel:<asp:TextBox ID="txtChannel" runat="server" Text="0" TabIndex="-1"></asp:TextBox>    
        <br />
        BottomTrack:<asp:TextBox ID="txtBottomTrack" runat="server" Text="0" TabIndex="-1"></asp:TextBox>    
        <br />
        BladeOperation:<asp:TextBox ID="txtBladeOperation" runat="server" Text="0" TabIndex="-1"></asp:TextBox>
        <br />
        BladeOperationBottom:<asp:TextBox ID="txtBladeOperationBottom" runat="server" Text="0" TabIndex="-1"></asp:TextBox>
        <br />
        CurvedTrack:<asp:TextBox ID="txtCurvedTrack" runat="server" Text="0" TabIndex="-1"></asp:TextBox>
        <br />
        StandOff:<asp:TextBox ID="txtStandOff" runat="server" Text="0" TabIndex="-1"></asp:TextBox>
        <br />
        Winder:<asp:TextBox ID="txtWinder" runat="server" Text="0" TabIndex="-1"></asp:TextBox>
        <br />
        ControllerSide:<asp:TextBox ID="txtControllerSide" runat="server" Text="0" TabIndex="-1"></asp:TextBox>
        <br />
        ScheduleID:<asp:TextBox ID="txtIntScheduleID" runat="server" TabIndex="-1" Text="" />
        <br /> 
        TempButton:<asp:TextBox ID="txtbuttonName" runat="server" TabIndex="-1" Text="" />
        <br />  
        HiddenShutterValue<asp:HiddenField ID="hiddenShutterID" runat="server" /> 
        <br />
        HiddenMidRail<asp:HiddenField ID="hiddenMidRail" runat="server" /> 
         <%--added by surendra dt:07/06/2023 ticket #75293--%>
        HiddenOrderStatus<asp:HiddenField ID="hfOrderStatus" runat="server" Value="0" /> 

    </div>

    <script type="text/javascript">

        function pageLoad() {
            $(function () {
                $("input#txtActualShippingDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
                $("input#txtPlannedShippingDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+52w' });
                $("input#txtOrderDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+1w' });
                $("input#txtReceived").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '--1w', maxDate: '+1w' });
                $("input#txtScheduledDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
                $("input#txtCuttingDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
                $("input#txtPiningDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
                $("input#txtPrepDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
                $("input#txtAssemblyDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
                $("input#txtHingingDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
                $("input#txtPackupDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
                $("input#txtFramingDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
                $("input#txtQCDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
                $("input#txtWrappingDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
                $("input#txtPostProduction1").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
                $("input#txtPostProduction2").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });

                $("input#txtCheckMeasureDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
                $("input#txtPickingDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });
                $("input#txtInstallDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+2w' });

                $("input#txtPCPickDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
                $("input#txtPCStartDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
                $("input#txtPCETADate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
                $("input#txtPCCompleteDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
                $("input#txtRequirementPickDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
                $("input#txtRequirementStartDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
                $("input#txtRequirementETADate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
                $("input#txtRequirementCompleteDate").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd M yy', gotoCurrent: true, showAnim: 'slideDown', minDate: '-1w', maxDate: '+5w' });
            });
        }
        
    </script>

    </form>
</body>
</html>
