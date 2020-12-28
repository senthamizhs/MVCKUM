
function closedetails() {
    $("#modal-agentlist-bal").hide();
}

function loadmasteragentdetails() {

    var basebranchid = $("#allbranchload").val();

    var Agnlist = JSON.parse(Agentmasterlst);

    var Agentload = "";
    Agentload += "<option value=''>-- Select --</option>";
    for (var i = 0; i < Agnlist.length; i++) {
        if (Agnlist[i].Branch_Id == basebranchid) {
            Agentload += '<option value=\"' + Agnlist[i].Agent_Id + '"\>' + Agnlist[i].Agency_name + '</option>';
        }
    }

    $("#allagentload").chosen('destroy');
    $("#allagentload").html(Agentload);
    $("#allagentload").chosen('destroy');
    $("#allagentload").chosen();
}

$(document).ready(function () {

    $(".wsmenu .wsmenu-list > li").each(function (index) {
        $(this).css({ 'animation-delay': (index / 10) + 's' });
    });

    if (jodproduct != null && jodproduct != "" && jodproduct == "U") {
        
        $("#manulajod").css("display", "none");
    }

});

$(window).on('resize', function () {
    //pushmobmenu();
});

function pushmobmenu() {
    $('#dvusrnm').pushme({
        element: '#dv-desk-usrnm',
        pushAction: 'appendTo',
        mq: 'screen and (min-width:768px)'
    });
    $('#dvusrnm').pushme({
        element: '#dv-mob-usrnm',
        pushAction: 'appendTo',
        mq: 'screen and (max-width:767px)'
    });
}

$('#aLogout').click(function () {
    $("#modal-fr").modal({
        backdrop: 'static',
        keyboard: false
    });
});

$('#btnyeslogout').click(function () {
    sessionStorage.setItem("logoutinsertflg", "Y");
    window.location.href = LogoutURL;
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
            //LoadClient_theme2_checkbala();
            var agentid = "";
            balancecheckfunction(agentid);
        }
    }
    else {
        GetClientDetails();
        //Loadagentlist();
    }
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
            
            //
            $("#dvBalance").hide();
            //
            $("#viewbalance_display").hide();
            $("#SHOWMONEY").hide();
            if (json.Result[0] == "-1") {
                window.location.href = SessionRedirectURL;
                return false;
            }
            else if (json.Result[0] == "0") {
                var result1 = json.Result[1]
                if (json.Result[1] != null && json.Result[1] != "") {
                    var appen = "";
                    var viewbal = result1.split('|');
                    for (var i = 0; i < viewbal.length - 1; i++) {
                        appen += "<div class='AgentLabel' id='' >"
                        appen += "<label id='Agentamount' style='text-transform: capitalize'>" + viewbal[i] + "</label> </div>"
                    }

                    if (strTerminalType == "W") {
                        if (ThemeVersion=="THEME2") {
                            $("#Agentamount").html(appen);
                        }
                        else
                        {
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
                alert(json.Result[0]);
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



$(".otherProduct").click(function () {
    var ID = this.id;
    var Otherproducturl = "";
    if (ID == "TOP")
        Otherproducturl = PGRedirectURL;
    else
        Otherproducturl = OtherProductURL + "?Flag=" + ID;

    window.location.href = Otherproducturl;
});



function checkconnection() {
    try {
        if (checkmobile.toUpperCase() == "TRUE") {
            return true; //For Mobile need to develop.... now temporarly return true.... by saranraj...
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

function GetClientDetails() {//LoadClient_theme2_checkbala
    
    $("#dvBalance").hide();
    //CallWidgetpopup('Balance', 'modal-balance', '400')
  
        $.ajax({
            type: "POST",
            url: ClientDetailsURL,
            contentType: "application/json; charset=utf-  8",
            timeout: 180000,
            data: '{}',
            dataType: "json",
            success: function (data) {
                try {
                    ;
                    var data = data["Data"];
                    
                    if (data.Status == "00") {
                        showerralert(data.Result, "", "");
                        console.log(data.Error);
                    }

                    if (data.Status == "01") {
                        var arr = JSON.parse(data.Result);
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

                        $("#modal-agentlist-bal").show();
                        $("#modal-agentlist-bal").modal({
                            backdrop: 'static',
                            keyboard: false
                        });
                    }
                    //$(".displayblock").css("display", "none");
                }
                catch (err) {
                    console.log(err.message)
                }
            },
            error: function (e) {
                console.log(e.responseText)
            }
        });
    //}
    //else {
    //    $("#SHOWMONEY").show();
    //    //$(".displaynon").css("display", "block");
    //    $(".displayblock").css("display", "none");
    //    $('#hCorp_x').hide();
    //    $('#ddlclient_agentbal').hide();
    //    var agentid = "";
    //    balancecheckfunction(agentid);
    //}
}

function displayResults(item) {
    var selectedvalues = item.value;
    //$('.alert').show().html('You selected <strong>' + item.text + '</strong>: <strong>' + item.text + '</strong>');
    balancecheckfunction(item.value);
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
        headerColor: '#245b9d',
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
