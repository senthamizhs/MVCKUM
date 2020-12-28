function closedetails() {
    $("#modal-agentlist-bal").hide();
}

$(document).ready(function () {
    $(".wsmenu .wsmenu-list > li").each(function (index) {
        $(this).css({ 'animation-delay': (index / 10) + 's' });
    });

    if (strTerminalType != null && strTerminalType != "" && strTerminalType == "U") {
        $("#manulajod").css("display", "none");
    }
});

$(window).load(function (e) {
    $(".input-effect input").focusout(function () {
        if ($(this).val().trim() != "") {
            $(this).addClass("has-content");
        } else {
            $(this).removeClass("has-content");
        }
    });

    $(".input-effect textarea").focusout(function () {
        if ($(this).val().trim() != "") {
            $(this).addClass("has-content");
        } else {
            $(this).removeClass("has-content");
        }
    });
});

function showError(msg, errPtagid, errtagid, focusid) {
    $("#" + errPtagid).addClass("has_error");
    $("#" + errPtagid + " .message").addClass("animation");
    $("#" + errtagid).addClass("error_active");
    setTimeout(function () {
        $("#" + errPtagid + " .message").removeClass("animation");
    }, 200);
    $("#" + errtagid).html(msg);

    if (focusid != "")
        $('#focusid').focus();
}

function hideError(errPtagid, errtagid) {
    $("#" + errPtagid).removeClass("has_error");
    $("#" + errPtagid + " .message").removeClass("animation");
    $("#" + errtagid).removeClass("error_active");
}

function getbalance() {
    if (strTerminalType == "W") {
        if (document.getElementById("SHOWMONEY").style.display == "block") {
            $("#SHOWMONEY").hide();
            $("#popupcloseicon").hide();
        }
        else {
            $(".displaynon").css("display", "none");
            $(".displayblock").css("display", "block");
            $("#SHOWMONEY").show();
            $("#popupcloseicon").show();
            var agentid = "";
            balancecheckfunction(agentid);
        }
    }
    else {
        GetClientDetails();
    }
}

function balancecheckfunctionBOA(agentid, strBranchID) {
    // var AgentId = "";
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: BalanceCheckBOAURL,// Location of the service
        data: '{AgentId:"' + agentid + '",strBranchID:"' + strBranchID + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call         
            $("#dvBalance").hide();
            //
            $("#viewbalance_display").hide();
            $("#SHOWMONEY").hide();
            if (json.Result[0] == "-1") {
                window.location.href = SessionRedirectURL;
                return false;
            }
            else if (json.Result[0] == "0") {
                var result1 = json.Result[1];
                if (json.Result[1] != null && json.Result[1] != "") {
                    var appen = "";
                    var viewbal = result1.split('|');
                    for (var i = 0; i < viewbal.length - 1; i++) {
                        appen += "<div class='AgentLabel' id='' >"
                        appen += "<label id='Agentamount' style='text-transform: capitalize'>" + viewbal[i] + "</label> </div>"
                    }

                    if (strTerminalType == "W") {
                        if (ThemeVersion == "THEME2") {
                            $("#Agentamount").html(appen);
                            $("#viewbalance_display").show();
                            $("#SHOWMONEY").show();
                            $(".displaynon").css("display", "block");
                            $(".displayblock").css("display", "none");
                        }
                        else {
                            $("#viewbalance_display").html(appen);
                            $("#viewbalance_display").show();
                            $("#SHOWMONEY").show();
                            $(".displaynon").css("display", "block");
                            $(".displayblock").css("display", "none");
                        }
                    }
                    else {
                        $("#dvBalance").html(appen);
                        $("#dvBalance").show();
                    }
                }
                return false;
            }
            else {
                alert(json.Message);
            }
        },
        error: function (e) {//On Successful service call
            if (e.status == "500") {
                window.location.href = SessionRedirectURL;
                return false;
            }
        }
    });
}

function balancecheckfunction(agentid) {
    // var AgentId = "";
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: BalanceCheckURL,// Location of the service
        data: '{AgentId:"' + agentid + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call         
            $("#dvBalance").hide();
            //
            $("#viewbalance_display").hide();
            $("#SHOWMONEY").hide();
            if (json.Result[0] == "-1") {
                window.location.href = SessionRedirectURL;
                return false;
            }
            else if (json.Result[0] == "0") {
                var result1 = json.Result[1];
                if (json.Result[1] != null && json.Result[1] != "") {
                    var appen = "";
                    var viewbal = result1.split('|');
                    for (var i = 0; i < viewbal.length - 1; i++) {
                        appen += "<div class='AgentLabel' id='' >"
                        appen += "<label id='Agentamount' style='text-transform: capitalize'>" + viewbal[i] + "</label> </div>"
                    }

                    if (strTerminalType == "W") {
                        if (ThemeVersion == "THEME2") {
                            $("#Agentamount").html(appen);
                            $("#viewbalance_display").show();
                            $("#SHOWMONEY").show();
                            $(".displaynon").css("display", "block");
                            $(".displayblock").css("display", "none");
                        }
                        else {
                            $("#viewbalance_display").html(appen);
                            $("#viewbalance_display").show();
                            $("#SHOWMONEY").show();
                            $(".displaynon").css("display", "block");
                            $(".displayblock").css("display", "none");
                        }
                    }
                    else {
                        $("#dvBalance").html(appen);
                        $("#dvBalance").show();
                    }
                }
                return false;
            }
            else {
                alert(json.Message);
            }
        },
        error: function (e) {//On Successful service call
            if (e.status == "500") {
                window.location.href = SessionRedirectURL;
                return false;
            }
        }
    });
}

function AnimateAgnBal() {
    document.getElementById("aAgnbal").disabled = true;

    if (ThemeVersion == "THEME2") {
        if ($("body").hasClass("mini-navbar")) {
            $("#dvAgnbal").addClass("walletcls1")
            $("#dvAgnbal").removeClass("clsAgnBal1")
        } else {
            $("#dvAgnbal").removeClass("walletcls1")
            $("#dvAgnbal").addClass("clsAgnBal1")
        }
        $("#dvAgnbal").css("display", "block")
        $("#dvAgnbal").addClass('animated1 slideInLeft');
        setTimeout(function () {
            $("#dvAgnbal").hide('slow').removeClass('animated1').removeClass('slideInLeft');
            document.getElementById("aAgnbal").disabled = false;
        }, 5000);
    }
    else {
        $("#dvAgnbal").show().addClass('animated1 slideInUp');
        setTimeout(function () {
            $("#dvAgnbal").hide('slow').removeClass('animated1').removeClass('slideInUp');
            document.getElementById("aAgnbal").disabled = false;
        }, 10000);//10000
    }
}


$('.clsAothers').click(function () {
    var aa = this.id;
    sessionStorage.setItem("testing", aa);
});


$("#withfare").click(function () {
    if ($("#Chk_fare")[0].checked == true) {
        $("#Chk_fare")[0].checked = false;
    }
    else {
        $("#Chk_fare")[0].checked = true;
    }
});

$("#single").click(function () {
    if ($("#Chk_fare2")[0].checked == true) {
        $("#Chk_fare2")[0].checked = false;
    }
    else {
        $("#Chk_fare2")[0].checked = true;
    }
});

$('.aLogout').click(function () {
    $("#modal-fr").modal({
        backdrop: 'static',
        keyboard: false
    });
});

$('#btnyeslogout').click(function () {
    sessionStorage.setItem("logoutinsertflg", "Y");
    window.location.href = LogoutURL;
});

$(".otherProduct").click(function () {
    var ID = this.id;
    var Otherproducturl = "";
    if (ID == "TOP") {
        Otherproducturl = PGRedirectURL;
    }
    else if (ID == "CTB") {
        Otherproducturl = CheckTopup;
    }
    else {
        Otherproducturl = OtherProductURL + "?Flag=" + ID;
    }
    window.location.href = Otherproducturl;
});

function checkconnection() {
    try {
        if (checkmobile.toUpperCase() == "TRUE") {
            return true; //For Mobile need to develop.... now temporarly return true....
        }
        else {
            var status = navigator.onLine;
            if (status) {
                return true;
            } else {
                return false;
            }
        }
    }
    catch (e) {
        return true; //For Browser capability Safty purpose ...
    }
}

function callpaymentgateway() {
    window.location.href = PaymentGatewayURL;
}

var arr = [];
function GetClientDetails() {

    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loaderbal + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" />',
        css: { padding: '5px' }
    });

    $("#dvBalance").hide();
    $.ajax({
        type: "POST",
        url: ClientDetailsURL,
        contentType: "application/json; charset=utf-  8",
        timeout: 180000,
        data: '{}',
        dataType: "json",
        success: function (data) {
            $.unblockUI();
            try {
                arr = [];
                var data = data["Data"];
                if (data.Status == "-1") {
                    console.log("Session Expired !");
                    //window.location.href = SessionRedirectURL;
                    return false;
                }
                if (data.Status == "00") {
                    showerralert(data.Result, "", "");
                    console.log(data.Error);
                }

                if (data.Status == "01") {
                    arr = JSON.parse(data.Result);
                    $("#txtAgencyName").val('');
                    //
                    $('#txtAgencyName').typeahead('destroy');
                    $('#txtAgencyName').typeahead({
                        source: arr,
                        items: 5,
                        displayField: "CLTNAME",
                        valueField: "CLTCODE",
                        scrollBar: true,
                        onSelect: displayResults,
                    });

                    //$("#modal-agentlist-bal").show();
                    $("#modal-agentlist-bal").modal({
                        backdrop: 'static',
                        keyboard: false
                    });

                }
                //$(".displayblock").css("display", "none");
                //$.unblockUI();

            }
            catch (err) {
                console.log(err.message)
            }
        },
        error: function (e) {
            $.unblockUI();
            console.log(e.responseText)
        }
    });

}

function displayResults(item) {
    var selectedvalues = item.value;
    filter = $.grep(arr, function (u, i) {
        return u.CLTCODE == selectedvalues;
    });
    balancecheckfunctionBOA(item.value, filter[0].BID);
}

//iziModal POPUP
function showalert(msg, okbtn, reurl) {
    $('#modal-alert').iziModal('destroy');
    if (okbtn != "") {
        if (reurl == "") {
            msg += '<center><a href="javascript:void(0)" class="clsizibtn" data-izimodal-close="">' + okbtn + '</a></center>';
        }
        else {
            msg += '<center><a href="' + reurl + '" class="clsizibtn">' + okbtn + '</a></center>';
        }
    }
    $("#modal-alert").iziModal({
        title: msg,
        icon: 'fa fa-info',
        headerColor: '#5bbd72',
        width: "500px"
        // timeout: 5000
        // 'max-width':"300px"
    });
    $('#modal-alert').iziModal('open');
}

function showerralert(msg, timout, temp2) {
    $('#modal-alert').iziModal('destroy');
    if (timout == null || timout == "") {
        $("#modal-alert").iziModal({
            title: msg,
            icon: 'fa fa-warning',
            headerColor: '#bd5b5b',
            width: "500px"
        });
    }
    else {
        $("#modal-alert").iziModal({
            title: msg,
            icon: 'fa fa-warning',
            headerColor: '#bd5b5b',
            width: "500px",
            timeout: timout
        });
    }
    $('#modal-alert').iziModal('open');
}

//  IziModalIFramePop("https://www.indifi.com/associate/RiyaTravelsandTours?utm_source=Direct&utm_medium=Banner&utm_campaign=1217", "Info", "", true);

// CallWidgetpopup("Indifi", "modal-WIDGETPOPUP", "800");

function CallWidgetpopup(Title, id, widthAS) {
    $("#" + id).iziModal('destroy');
    $("#" + id).iziModal({
        iframe: false,
        title: Title,
        iconClass: 'icon-stack',
        headerColor: '#bd5b5b',
        width: widthAS,
    });
    $('#' + id).iziModal('open', {
        transitionIn: 'bounceInDown',
        transitionOut: 'bounceOutDown' // TransitionOut will be applied if you have any open modal.
    });
}

function IziModalIFramePop(Iurl, ITitle, Isubtitle, ifull) {
    $('#modal-iframe').iziModal('destroy');
    $("#modal-iframe").iziModal({
        top: 200,
        headerColor: '#000',
        title: ITitle, //'iziModal with iframe',
        subtitle: Isubtitle, //'Video example using the Vimeo embed.',
        icon: 'icon-settings_system_daydream',
        overlayClose: false,
        iframe: true,
        iframeURL: Iurl,
        iframeHeight: 700,
        fullscreen: ifull, //true
        openFullscreen: false,
        width: 700,
        padding: 20,
        group: 'grupo1'
    });
    $('#modal-iframe').iziModal('open', {
        transitionIn: 'bounceInDown',// comingIn, bounceInDown, bounceInUp, fadeInDown, fadeInUp, fadeInLeft, fadeInRight, flipInX
        transitionOut: 'bounceOutDown'// comingOut, bounceOutDown, bounceOutUp, fadeOutDown, fadeOutUp, , fadeOutLeft, fadeOutRight, flipOutX
    });
}

$("#btnWIDGETPOPUP").click(function () {

    var strAgentID = ""; var strTerminalID = ""; var strUserName = ""; var strTerminalType = "";
    var dcSequenceId = ""; var strIPAddress = ""; var strBranchID = "";
    if ($('#chk_show').is(':checked') == true) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "AvailService.asmx/WIDGETPOPUP",
            data: "{'Agentid':'" + strAgentID + "','Terminalid':'" + strTerminalID + "','Username':'" + strUserName + "','Terminaltype':'" + strTerminalType + "','Status':'" + "UPDATE" + "','Seqno':'" + dcSequenceId + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d[2] == "01") {
                    $('.iziModal').iziModal('close');
                }
            },
            error: function (result) {
                alert(result);
                return false;
            }
        });
    }
});

//<!-- SEND SMS #STS177 --> 
function ViewSMS() {
    $("#ViewPnrdetailSms").modal({
        backdrop: 'static',
        keyboard: false,
        show: true
    });
}
function SMSVIEW() {
    $("#wholecontentdiv").slideUp();
    $("#sendsmsticket").slideDown();
}
function CloseSMSPopup() {
    $("#wholecontentdiv").slideDown();
    $("#sendsmsticket").slideUp();
    $("#ViewPnrdetailSms").modal("hide")
}
function btnSMSSend(arg) {
    var strSPNRNO = "";
    var mobnum = "";
    if (arg == "B") {
        strSPNRNO = sessionStorage.getItem('viewpnr_spnr');
        mobnum = $("#SMSNumber").val();
        if (mobnum == null || mobnum == "") {
            $("#SMSNumber").addClass("bordercls");
            $("#SMSNumber").attr("placeholder", "Please Enter Mobile Number");
            return false;
        }
        $("#SMSNumber").removeClass("bordercls");
    }
    else {
        strSPNRNO = document.getElementById('txt_viewSPNR').value;
        mobnum = $("#txtSMSNumber").val();
        if (mobnum == null || mobnum == "") {
            $("#txtSMSNumber").addClass("bordercls");
            $("#txtSMSNumber").attr("placeholder", "Please Enter Mobile Number");
            return false;
        }
        $("#txtSMSNumber").removeClass("bordercls");
    }

    var input = {
        strSPNRNO: strSPNRNO,
        MobileNumber: mobnum,
        Flag: "V",
        strPaymentmode: sessionStorage.getItem('paymentmode'),
    }
    if (arg == "B") {
        $('#smsemail').slideToggle(800);
        $('.fa-angle-double-down').toggleClass("rotate180");
    }
    $("#pnrpopup").modal("hide");
    $("#sendsmsticket").slideDown();
    $("#sendsmsticket").modal("hide");
    $("#ViewPnrdetailSms").modal("hide");

    $.ajax({
        type: "POST",
        url: SmsUrl,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(input),
        dataType: 'json',
        success: function (data) {
            if (data.Status == "01") {
                $("#SMSNumber").val("");
                $("#txtSMSNumber").val("");
                showalert(data.Error, "", "");
                return false;
            }
            else if (data.Status == "00") {
                $("#SMSNumber").val("");
                $("#txtSMSNumber").val("");
                showerralert(data.Error, "", "");
                return false;
            }

        },
        error: function (e) {

        }
    });
}
//<!-- SEND SMS #STS177 --> 

/*Below functio for Direct from BKH and VPNR Email0- STS185*/
function SendEmail() {
    $("#wholecontentdiv").slideUp();
    $("#sendemailticket").slideDown();
}
function SendtoMail() {
    $("#modal-ViewpnrsendMail").modal({
        backdrop: 'static',
        keyboard: false,
        show: true
    });
}
function btnEmailSend(arg) {
    var strSPNRNO = "";
    var emailid = "";
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (arg == "B") {
        strSPNRNO = sessionStorage.getItem('viewpnr_spnr');
        emailid = $("#EmailId").val();
        if (EmailId == null || EmailId == "") {
            $("#EmailId").addClass("bordercls");
            alert("Please Enter Email Id");
            return false;
        }

        if (!filter.test(emailid)) {
            $("#EmailId").addClass("bordercls");
            $("#EmailId").attr("placeholder", "Please provide a valid email address");
            alert("Please provide a valid email address");
            return false;
        }
        $("#EmailId").removeClass("bordercls");
    }
    else {
        strSPNRNO = document.getElementById('txt_viewSPNR').value;
        emailid = $("#txtEmailId").val();
        if (emailid == null || emailid == "") {
            $("#txtEmailId").addClass("bordercls");
            alert("Please Enter Email Id", "", "");
            return false;
        }
        if (!filter.test(emailid)) {
            $("#EmailId").addClass("bordercls");
            alert("Please provide a valid email address");
            return false;
        }
        $("#txtEmailId").removeClass("bordercls");
    }

    var input = {
        strSPNRNO: strSPNRNO,
        txtEmailID: emailid,
        pdf: "NO",
        strSingle: "yes",
        Subject: strSPNRNO,
        strPaymentmode: sessionStorage.getItem('paymentmode'),
        blntktmailflag: false,
    }
    if (arg == "B") {
        $('#smsemail').slideToggle(800);
        $('.fa-angle-double-down').toggleClass("rotate180");
    }
    $("#pnrpopup").modal("hide");
    $("#sendemailticket").slideDown();
    $("#sendemailticket").modal("hide");
    $("#modal-ViewpnrsendMail").modal("hide");

    $.ajax({
        type: "POST",
        url: EMailsendingurl,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(input),
        dataType: 'json',
        success: function (data) {
            if (data.Status == "1") {
                $("#EmailId").val("");
                $("#txtSEmailId").val("");
                showalert(data.Message, "", "");
                return false;
            }
            else if (data.Status == "0") {
                $("#EmailId").val("");
                $("#txtSEmailId").val("");
                showerralert(data.Message, "", "");
                return false;
            }

        },
        error: function (e) {

        }
    });
}
function CloseEmailPopup() {
    $("#wholecontentdiv").slideDown();
    $("#sendemailticket").slideUp();
    $("#CloseEmailPopup").modal("hide")
}

function LoadProductSupportdetails() {
    try {
        $.ajax({
            type: "GET",
            url: ProductSupportDetails,
            async: true,
            dataType: "xml",
            success: function (xml) {
                var json = $.xml2json(xml);
                var ProductSupportdetails = json[ProductType];
                if (ProductSupportdetails != undefined && ProductSupportdetails != null && ProductSupportdetails != "") {
                    var Address = "";

                    if (Array.isArray(ProductSupportdetails.ADDRESS))
                    $.each(ProductSupportdetails.ADDRESS, function (i, val) {
                        Address += val.DETAILS + "<br/>";
                    })
                    else
                        Address = ProductSupportdetails.ADDRESS != undefined && ProductSupportdetails.ADDRESS.DETAILS != undefined ? ProductSupportdetails.ADDRESS.DETAILS : "";

                    var PhnNO = Array.isArray(ProductSupportdetails.CALLCENTERNO) ? ProductSupportdetails.CALLCENTERNO[0].NUMBER : ProductSupportdetails.CALLCENTERNO.NUMBER;
                    var EmailID = Array.isArray(ProductSupportdetails.ENQUIRY) ? ProductSupportdetails.ENQUIRY[0].EMAILID : ProductSupportdetails.ENQUIRY.EMAILID;
                   
                    //Footer Content Region Start
                    var className = "";
                    var Link = "";

                    var FooterBuilder = '<div class="col-md-3 col-sm-3 col-xs-12 mg-btm-20-res">';
                    FooterBuilder += '<h5>Contact Us</h5>'
                    FooterBuilder += '<p id="pAddress"> ' + Address + '</p>'
                    FooterBuilder += '<p><i class="las la-envelope pr-0"></i><a href="mailto:' + EmailID + '" target="_top" id="asupportmailid">&nbsp;' + EmailID + '</a></p>'
                    FooterBuilder += '<p id="pContactno"><i class="las la-tty pr-0"></i><a href="Tel:' + PhnNO + '" target="_top" id="asupportmailid">&nbsp;' + PhnNO + '</a></p>'
                    FooterBuilder += '</div>'

                    FooterBuilder += '<div class="col-md-9 col-sm-9 col-xs-12 pad-l-r-0-res">'
                    var col = Math.ceil(12 / ProductSupportdetails.FOOTER.CONTENT.length);
                    col = col <= 3 ? 3 : col;
                    var colcls = "col-md-" + col + " col-sm-" + col + " col-xs-12 mg-btm-20-res";
                    $.each(ProductSupportdetails.FOOTER.CONTENT, function (i, val) {
                        FooterBuilder += '<div class="' + colcls + '">'
                        FooterBuilder += '<h5>' + val.HEAD + '</h5>'
                        if (val.HEAD == "PAYMENT") {
                            $.each(val.DETAILS, function (j, value) {
                                FooterBuilder += '<i class="' + value.NAME + '" style="font-size: 40px;"></i>'
                            });

                        }
                        else {
                            FooterBuilder += '<ul>'
                            $.each(val.DETAILS, function (j, value) {
                                className = value.LINK != "" ? "" : "point-none";
                                Link = value.LINK != "" ? value.LINK : "#";
                                FooterBuilder += '<li><a href=' + Link + ' class="' + className + '">' + value.NAME + '</a></li>'
                            });
                            FooterBuilder += '</ul>'
                        }

                        FooterBuilder += '</div>'
                    });
                    FooterBuilder += '</div>'

                    $(".dvFotterContent").html(FooterBuilder);
                }
                else {
                    $(".dvFooter").hide();
                }
                //Footer Content Region End

            },
            error: function (data) {
                $(".dvFooter").hide();
                showerralert("An error occurred while processing product support details XML file.", "", "");
                console.log(data);
            }
        });
    }
    catch (ex) {
        $(".dvFooter").hide();
        console.log(ex)
    }
}

function LoadProductdetails() {
    try {
        $.ajax({
            type: "GET",
            url: ProductDetails,
            async: true,
            dataType: "xml",
            success: function (xml) {

                var json = $.xml2json(xml);
                var ProductOfferdetails = json[ProductType];
                var stringBuilder = "<h2>What We Offer</h2>";
                $.each(ProductOfferdetails, function (i, val) {
                    stringBuilder += '<div class="col-md-3 info">'
                    stringBuilder += '<div class="sup_service22 sup_service_type2">'
                    stringBuilder += '<div class="sqbox">'
                    stringBuilder += '<img src="' + ImageUrl + '/' + ProductType + '/icon_shape.png">'
                    stringBuilder += '<img class="icon" src="' + ImageUrl + '/' + ProductType + '/' + val.ICON + '.png">'
                    stringBuilder += '</div>'
                    stringBuilder += '<div class="sup_title_border">'
                    stringBuilder += '<h4>' + val.TITLE + '</h4>'
                    stringBuilder += '<p>' + val.CONTENT + '</p>'
                    stringBuilder += '</div>'
                    stringBuilder += '</div>'
                    stringBuilder += '</div>'
                });
                $("#dvdynmicOffer").html(stringBuilder);
            },
            error: function () {
                showerralert("An error occurred while processing product support details XML file.", "", "");
            }
        });
    }
    catch (ex) {
        console.log(ex)
    }
}
