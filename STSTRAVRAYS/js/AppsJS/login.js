var BrowserDetails = "";
var geolocation = "";

// LOCATION AND BROWSWE DETAILS 
$(document).ready(function () {

    var terminal_id = localStorage.getItem("agnterminalid");
    if (terminal_id != null && terminal_id != "") {
        $("#txt_id").val(terminal_id.trim());
        $('#txt_id').addClass('has-content');
    }
    $(".input-effect input").focusout(function () {
        if ($(this).val().trim() != "") {
            $(this).addClass("has-content");
        } else {
            $(this).removeClass("has-content");
        }
    })

    $(".input-effect textarea").focusout(function () {
        if ($(this).val().trim() != "") {
            $(this).addClass("has-content");
        } else {
            $(this).removeClass("has-content");
        }
    })

    Geolocationlog();

    if (sessionStorage.getItem('logoutinsertflg') != null && sessionStorage.getItem('logoutinsertflg') == "Y") {
        logoutfun();
    }

    if (document.getElementById('txt_usrnm').innerHTML != null && document.getElementById('txt_usrnm').innerHTML != "") {
        $("#txt_usrnm").val(cookie_username);
        $("#txt_usrnm").addClass("has-content");
    }
    if (document.getElementById('txt_passwd').innerHTML != null && document.getElementById('txt_passwd').innerHTML != "") {
        $("#txt_passwd").val(cookie_password);
        $("#txt_passwd").addClass("has-content");
    }
});

$(document).on('click', "#get_login", function () {
    console.log("Login_Submit_Request: " + new Date());
    CheckLogin();
});

function CheckLogin() {
    var TerminalId,
    Username,
    Password = "";
    if (CheckVal()) {
        document.getElementById("get_login").disabled = true;
        $('#iLoading').show();
        TerminalId = $("#txt_id").val() != null ? $("#txt_id").val().toUpperCase().trim() : "";
        Username = $("#txt_usrnm").val() != null ? $("#txt_usrnm").val().trim() : "";
        Password = $("#txt_passwd").val() != null ? $("#txt_passwd").val().trim() : "";
        var logincountry = "";
        if (allowlogincountry == "" || logincountry.toString().indexOf(allowlogincountry) > -1) {
            var LoginParam = { tr_id: TerminalId, NAME: Username, PWD: Password, Environment: $("#hdnterminal_app").val() }; //W-Web, M-Mobile, mobstu-Only used for Mobile...
            try {
                if (ProductType == "ROUNDTRIP" || strAppHost == "BSA") {
                    $("#iLoading").show();
                    $.blockUI({
                        message: '<img alt="Please Wait..." src="' + loaderurl + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
                        css: { padding: '5px' }
                    });
                }
                $.ajax({
                    type: "POST",
                    url: LoginSubmit,
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(LoginParam),
                    timeout: 180000,
                    dataType: "json",
                    success: function (data) {

                        if (ProductType == "ROUNDTRIP" || strAppHost == "BSA") {
                            $.unblockUI();
                        }
                        console.log("Login_Submit_Response_time: " + new Date());
                        document.getElementById("get_login").disabled = false;
                        $('#iLoading').hide();
                        if (data.Status == "00") {
                            if (data.Message != "") {
                                showError(data.Message, "");
                                asyncafterLogin("FAILED", TerminalId, Username, Password, data.Message);
                            }
                            else {
                                showError("Unable to login (#07).", "");
                                asyncafterLogin("FAILED", TerminalId, Username, Password, "Unable to login");
                            }
                        }
                        else if (data.Status == "01") {


                            localStorage.setItem("agnterminalid", TerminalId);
                            localStorage.setItem("agnagentid", data.Result);
                            TerminalId = TerminalId.trim();
                            
                            if ($("#hdn_product").val() == "RIYA") {
                                if ($('#chkRemember').is(':checked')) {
                                    localStorage.setItem('txtUsername', $('#txt_usrnm').val());
                                    localStorage.setItem('txtPWD', $('#txt_passwd').val());
                                    localStorage.setItem('chkbxrem', "true");
                                } else {
                                    localStorage.removeItem('txtUsername');
                                    localStorage.removeItem('txtPWD');
                                    localStorage.removeItem('chkbxrem');
                                }
                            }
                            asyncafterLogin("SUCCESS", TerminalId, Username, Password, "");
                            if ($("#hdn_apptheme").val() == "THEME1") {
                                window.location.href = HomeMaster;
                            }
                            else if ($("#hdn_apptheme").val() == "THEME3") {
                                window.location.href = HomeMaster;
                            }
                            if ($("#hdn_apptheme").val() != "THEME2") {
                                window.location.href = HomeMaster;
                            }
                            else {
                                window.location.href = HomeFlight;
                            }
                            return false;
                        }
                        else if (data.Status == "02") {

                            asyncafterLogin("SUCCESS", TerminalId, Username, Password, "");
                            ShowAggrement(data.Datee, data.AgnNm);
                            return false;
                        }
                        else if (data.Status == "05" && ProductType == "RIYA") {
                            Showupdatepwd();
                            $("#txt_TerminalId").val(data.TerminalId.toUpperCase());
                            $("#txt_Username").val(data.Username);
                        }
                        else if (data.Status == "05" && ProductType == "RBOA") {
                            ShowAggrement(data.Datee, data.AgnNm, "modal-changespassword", "Change Password");
                            return false;
                        }
                        else if (data.Status == "05" && (ProductType == "ROUNDTRIP" || strAppHost == "BSA")) {
                            $("#txt_TerminalId").val(data.TerminalId.toUpperCase());
                            $("#txt_Username").val(data.Username);
                        }
                        else {
                            asyncafterLogin("FAILED", TerminalId, Username, Password, "Unable to login");
                            showError("Problem occured while login (#07).", "");
                        }
                    },
                    error: function (e) {
                        asyncafterLogin("FAILED", TerminalId, Username, Password, "Unable to login");
                        document.getElementById("get_login").disabled = false;
                        $('#iLoading').hide();
                        if (ProductType == "ROUNDTRIP" || strAppHost == "BSA") {
                            $.unblockUI();
                        }
                        showError("Unable to login (#09).", "");
                    }
                });
            } catch (e) {
                asyncafterLogin("FAILED", TerminalId, Username, Password, "Unable to login");
                document.getElementById("get_login").disabled = false;
                $('#iLoading').hide();
                showError("Unable to login (#11).", "");
                if (ProductType == "ROUNDTRIP" || strAppHost == "BSA") {
                    $.unblockUI();
                }
            }
        }
        else {
            asyncafterLogin("FAILED", TerminalId, Username, Password, "Unable to login");
            showError("Login restricted please contact support team.", "");
        }
    }
}

function Showupdatepwd() {
    $(".Changepassword_open").trigger('click');
    $("#txt_terminalId").prop('readonly', true);
}
function Geolocationlog() {

    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: GeoLocationUrl,
        data: "{}",
        timeout: 10000,
        dataType: "json",
        success: function (locdata) {
            geolocation = locdata;
        },
        error: function (result) {
            AnotherGeolocationlog();
        }
    });
}

function AnotherGeolocationlog() {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: GeoLocationUrl,
        data: "{}",
        timeout: 10000,
        dataType: "json",
        success: function (locdata) {
            geolocation = locdata;
        },
        error: function (result) {

        }
    });
}
//----------

// COMMON LOG METHOD
function asyncafterLogin(loginstatus, TerminalId, Username, Password, Remarks) {

    var user = detect.parse(navigator.userAgent);
    var browsernm = user.browser.family;
    var browsername = user.browser.name;
    var browserversion = user.browser.version;
    var operatingsys = user.os.name;
    var devc = user.device.type;

    var inputdata = {
        CLIENT_ID: TerminalId,
        USERNAME: Username,
        PASSWORD: Password,
        PLATFORM: devc == "Desktop" ? "Web - " : "Mobile - ",  // BrowserDetails.mobile == true ? "Mobile - " : "Web - ",
        STATUS: loginstatus,
        BROWSER: browsernm + "-" + browserversion, // BrowserDetails.browser + " - " + BrowserDetails.browserVersion,
        IP: geolocation.ip,
        ISP: geolocation.org,
        LATITUDE: geolocation.latitude,
        LONGITUDE: geolocation.longitude,
        CITY: geolocation.city,
        COUNTRY: geolocation.country_name,
        STATE: geolocation.region,
        REMARKS: Remarks
    }

    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: CommonLog,// Location of the service
        data: JSON.stringify(inputdata),
        timeout: 200000,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (json) {//On Successful service call
            var result = json.Result[0];
            $.unblockUI();
        },
        error: function (e) {// When Service call fails                           
            $.unblockUI();
        }
    });
}

function CheckVal() {

    var terminalidvalidation_msg, usernamevalidation_msg, passwordvalidation_msg = "";

    if ($("#hdnterminal_app").val() != "T") {
        if ($("#txt_id").val() == null || $("#txt_id").val().trim() == "") {
            showError("Please enter the terminal id.", "Login");
            $("#txt_id").focus();
            return false;
        }

        else if ($("#txt_usrnm").val() == null || $("#txt_usrnm").val().trim() == "") {
            showError("Please enter the user name.", "Login");
            $("#txt_usrnm").focus();
            return false;
        }
        else if ($("#txt_passwd").val() == null || $("#txt_passwd").val().trim() == "") {
            showError("Please enter the password.", "Login");
            $("#txt_passwd").focus();
            return false;
        }
        else if ($("#txt_id").val().length < 14 && ProductType == "ROUNDTRIP") {
            showError("Please enter 14 digit terminal id.", "Login");
            $("#txt_id").focus();
            return false;
        }
        else {
            hideError();
        }
        if (ProductType == "ROUNDTRIP") {
            if ($('#chkRemember').is(':checked')) {
                localStorage.setItem('TERMINALID', $('#txt_id').val());
                localStorage.setItem('USERNAME', $('#txt_usrnm').val());
                localStorage.setItem('PASSWORD', $('#txt_passwd').val());
                localStorage.setItem('CHK_REM', $('#chkRemember').is(":checked"));
            }
            else {
                localStorage.removeItem('TERMINALID');
                localStorage.removeItem('USERNAME');
                localStorage.removeItem('PASSWORD');
                localStorage.removeItem('CHK_REM');
            }
        }
        return true;
    }
    else {
        if ($("#txt_usrnm").val() == null || $("#txt_usrnm").val().trim() == "") {
            showError("Please enter the user name.", "Login");
            $("#txt_usrnm").focus();
            return false;
        }
        else if ($("#txt_passwd").val() == null || $("#txt_passwd").val().trim() == "") {
            showError("Please enter the password.", "Login");
            $("#txt_passwd").focus();
            return false;
        }
        else {
            hideError();
        }
        if (ProductType == "ROUNDTRIP") {
            if ($('#chkRemember').is(':checked')) {
                localStorage.setItem('TERMINALID', $('#txt_id').val());
                localStorage.setItem('USERNAME', $('#txt_usrnm').val());
                localStorage.setItem('PASSWORD', $('#txt_passwd').val());
                localStorage.setItem('CHK_REM', $('#chkRemember').is(":checked"));
            }
            else {
                localStorage.removeItem('TERMINALID');
                localStorage.removeItem('USERNAME');
                localStorage.removeItem('PASSWORD');
                localStorage.removeItem('CHK_REM');
            }
        }
        return true;
    }
}

//LOGIN FIELD CLEAR FUNCTION
$("#get_clear").click(function () {
    $("#txt_id").val("");
    $("#txt_usrnm").val("");
    $("#txt_passwd").val("");
    $("#txt_id").removeClass("has-content");
    $("#txt_usrnm").removeClass("has-content");
    $("#txt_passwd").removeClass("has-content");
    hideError();
});

var samperror = "";
$('.clshideErr').keyup(function () {
    if (ProductType == "RIYA" || ProductType == "ROUNDTRIP" || strAppHost == "BSA") {
        hideErrorValidation();
    }
    else { hideError(); }
});


function showError(msg, arg, id) {
    id = id != null && id != "" ? id : "empty-destination-box";
    $(".message").addClass("animation");
    $(".empty-destination-box").addClass("error_active");
    if (arg == "Login") {
        $(".loginpanel").addClass("has_error");
    }
    else if (arg == "Forget") {
        $(".forget").addClass("has_error");
    }
    else if (arg == "Update") {
        $(".update").addClass("has_error");
    } else {
        $(".loginpanel").addClass("has_error");
    }

    setTimeout(function () {
        $(".message").removeClass("animation");
    }, 200);
    $("#" + id).html(msg);
    $(".empty-destination-box").html(msg);
    setTimeout(function () {
        $(".message").removeClass("animation");
        $(".empty-destination-box").removeClass("error_active");
        if (arg == "Login") {
            $(".loginpanel").removeClass("has_error");
        }
        else if (arg == "Forget") {
            $(".forget").removeClass("has_error");
        }
        else if (arg == "Update") {
            $(".update").removeClass("has_error");
        }
        else {
            $(".loginpanel").removeClass("has_error");
        }
        $("#" + id).html("");
    }, 5000);
}

function hideError(temp1, temp2) {
    $(".message").removeClass("animation");
    $("#empty-destination-box,#Agent_empty-destination-box,#B2c_empty-destination-box").removeClass("error_active");
    $(".loginpanel").removeClass("has_error");
}

function hideErrorValidation(temp1, temp2) {
    $(".message").removeClass("animation");
    $("#empty-destination-box").removeClass("error_active");
    $(".loginpanel").removeClass("has_error");

    $("#" + samperror).css("display", "none");

}

function ShowAggrement(date, agnnm, modalname, title) {
    $.ajax({
        url: ShowAgreement,
        dataType: 'html',
        anync: false,
        success: function (response) {
            try {
                response = response.replace(new RegExp("#Current_Date#", "g"), date);
                response = response.replace(new RegExp("#Agency_Name#", "g"), agnnm);

                var Title = ProductType == "RBOA" && title != null ? title : "Subscriber Agreement";
                var Ititle = Title;
                var Isubtitle = "";
                var IContent = response;
                var Ifullopt = true;

                var modalid = ProductType == "RBOA" && modalname != null ? modalname : "modal-aggremnt";
                $('#' + modalid).iziModal('destroy');
                $("#txtRule").html(IContent);

                $("#" + modalid).iziModal({
                    title: Ititle,
                    subtitle: Isubtitle,
                    fullscreen: Ifullopt,
                    iconClass: 'icon-stack',
                    headerColor: '#636363',
                    width: 700,
                    padding: 20,
                });
                $('#' + modalid).iziModal('open');
            }
            catch (ex) {
                alert("unable to load Agreement page (#07).");
            }
        },
        error: function (e) {
            alert("unable to load Agreement page (#03).");
        }
    });
}

function insert_agrement() {
    $("#agreebuttons").css("display", "none");
    $("#agreement_loadimg").css("display", "block");
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: InsertAgreement, 		// Location of the service
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {//On Successful service call
            $('#modal-aggremnt').iziModal('close');
            if (result.Status == "-1") {
                window.location.href = SessionExpire;
            }
            else if (result.Status == "01") {
                if (result.Result == true) {
                    if ($("#hdn_apptheme")[0].value == "THEME1") {
                        window.location.href = HomeMaster;
                    }
                    else {
                        window.location.href = HomeFlight;
                    }
                    return false;
                }
                else {
                    showError("Problem occured while agree Agreement (#01).");
                }
            }
            else {
                showError("Problem occured while agree Agreement (#05).");
            }
        },
        error: function (e) {//On Successful service call
            if (e.status == "500") {
                window.location.href = SessionExpire;
                return false;
            }
            else {
                alert(e.MessageText + " (#07).");
            }
        }	// When Service call fails

    });
}
function Dagree_agrement() {
    $('#modal-aggremnt').iziModal('close');
}

// RAYSMODAL POPUP 
function showRayspopup(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2) {
    $('#modal-TandC').iziModal('destroy');
    $("#modal-TandC").html(IContent);
    $("#modal-TandC").iziModal({
        title: Ititle,
        subtitle: Isubtitle,
        fullscreen: Ifullopt,
        iconClass: 'icon-stack',
        headerColor: '#636363',
        width: 700,
        padding: 20,
    });
    $('#modal-TandC').iziModal('open');
}


//TERMS AND CONDITIONS 
function visitterms() {
    $.ajax({
        url: TermsAndConditions,
        dataType: 'html',
        anync: false,
        success: function (response) {
            try {
                var Ititle = "Terms and Condition";
                var Isubtitle = "";
                var IContent = response;
                var Ifullopt = true;
                var Itemp1 = "";
                var Itemp2 = "";
                showRayspopup(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            }
            catch (ex) {
                alert("unable to load Terms and Condition (#07).");
            }
        },
        error: function (e) {
            alert("unable to load Terms and Condition (#03).");
        }
    });
}

// PRIVACY POLICY
function visitpolicy() {
    $.ajax({
        url: PrivacyPolicy,
        dataType: 'html',
        anync: false,
        success: function (response) {
            try {
                var Ititle = "Privacy Policy";
                var Isubtitle = "";
                var IContent = response;
                var Ifullopt = true;
                var Itemp1 = "";
                var Itemp2 = "";
                showRayspopup(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            }
            catch (ex) {
                alert("unable to load Privacy Policy (#07).");
            }
        },
        error: function (e) {
            alert("unable to load Privacy Policy (#03).");
        }
    });
}

// AGENT REGISTRATION POPUP
function openmodelregistration() {
    var Ititle = "Agent Registration";
    var Isubtitle = "";
    var Ifullopt = true;
    $("#agree_loadingimg").css("display", "none");
    $('#modal-agnreg').iziModal('destroy');
    $("#modal-agnreg").iziModal({
        title: Ititle,
        subtitle: Isubtitle,
        fullscreen: Ifullopt,
        iconClass: 'icon-stack',
        headerColor: '#636363',
        width: 800,
        padding: 20,
    });
    $('#modal-agnreg').iziModal('open');
}

// AGENT REGISTRATION
function Registeragent() {
    $("#agree_loadingimg").css("display", "block");
    var agencyname = $("#txt_agencyname").val();
    var address = $("#txt_address").val();
    var cityname = $("#txt_city").val();
    var contctperson = $("#txt_contactperson").val();
    var mobileno = $("#txt_mobno").val();
    var emailadd = $("#txt_emailadd").val();

    if ($("#txt_agencyname").val() == null || $("#txt_agencyname").val() == "") {
        alert("Please enter agency name");
        $("#txt_agencyname").focus();
        $("#agree_loadingimg").css("display", "none");
        return false;
    }
    else if ($("#txt_contactperson").val() == null || $("#txt_contactperson").val() == "") {
        alert("Please enter contact person");
        $("#txt_contactperson").focus();
        $("#agree_loadingimg").css("display", "none");
        return false;
    }
    else if ($("#txt_address").val() == null || $("#txt_address").val() == "") {
        alert("Please enter address details");
        $("#txt_address").focus();
        $("#agree_loadingimg").css("display", "none");
        return false;
    }
    else if ($("#txt_city").val() == null || $("#txt_city").val() == "") {
        alert("Please enter city name");
        $("#txt_city").focus();
        $("#agree_loadingimg").css("display", "none");
        return false;
    }
    else if ($("#txt_mobno").val() == null || $("#txt_mobno").val() == "") {
        alert("Please enter mobile number");
        $("#txt_mobno").focus();
        $("#agree_loadingimg").css("display", "none");
        return false;
    }

    else if ($("#txt_emailadd").val() == null || $("#txt_emailadd").val() == "") {
        alert("Please enter email address");
        $("#agree_loadingimg").css("display", "none");
        return false;
    }
    else {
        $("#agree_loadingimg").css("display", "block");

        var Registrationform = {
            agencyname: agencyname, address: address, cityname: cityname, mobileno: mobileno, emailadd: emailadd, contctperson: contctperson
        };

        $.ajax({
            type: "POST",
            url: Registration,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(Registrationform),
            timeout: 180000,
            dataType: "json",
            success: function (data) {
                $("#agree_loadingimg").css("display", "none");
                if (data.Status == "00") {
                    if (data.Message != "") {
                        alert(data.Message);
                        $('#modal-agnreg').iziModal('close');
                        return false;
                    }
                    else {
                        alert("Unable to process your request please contact support team (#03)");
                        $('#modal-agnreg').iziModal('close');
                        return false;
                    }
                }
                else if (data.Status == "01") {
                    if (data.Message != "") {
                        //alert(data.Message);
                        $("#txt_agencyname").val("");
                        $("#txt_address").val("");
                        $("#txt_city").val("");
                        $("#txt_contactperson").val("");
                        $("#txt_mobno").val("");
                        $("#txt_emailadd").val("");
                        $("#displaymsg").css("display", "block");
                        $("#displaymsg")[0].innerHTML = data.Message;//.val(data.Message);
                        //$('#modal-agnreg').iziModal('close');
                        //return false;
                    }
                    else {
                        alert("Unable to process your request please contact support team (#04)");
                        $('#modal-agnreg').iziModal('close');
                        return false;
                    }
                }
                else if (data.Status == "02") {
                    $("#agree_loadingimg").css("display", "none");
                    if (data.Message != "") {
                        alert(data.Message);
                        return false;
                    }
                    else {
                        alert("Unable to process your request please contact support team (#05)");
                        $('#modal-agnreg').iziModal('close');
                        return false;
                    }
                }
                else {
                    alert("Unable to process your request please contact support team (#06)");
                    $('#modal-agnreg').iziModal('close');
                    return false;
                }
            },
            error: function (e) {
                alert("Unable to process your request please contact support team (#07)");
                $('#modal-agnreg').iziModal('close');
                return false;
            }
        });
    }
}

//AGENT REGISTRATION FIELD CLEAR FUNCTION
function clearvalue() {
    $("#txt_agencyname").val("");
    $("#txt_address").val("");
    $("#txt_city").val("");
    $("#txt_contactperson").val("");
    $("#txt_mobno").val("");
    $("#txt_emailadd").val("");

}


$(document).on('keydown', '#txt_passwd', function (e) {
    var keyCode = e.which;
    console.log("keyup (" + keyCode + ")")
    if (keyCode == 13) {
        $('#get_login').click();
        return false;
    }
});
$(document).on('keydown', '#txtpasswd', function (e) {
    var keyCode = e.which;
    console.log("keyup (" + keyCode + ")")
    if (keyCode == 13) {
        $('#get_signin').click();
        return false;
    }
});
$(document).on('keydown', '#txtcnfmpasswd', function (e) {
    var keyCode = e.which;
    console.log("keyup (" + keyCode + ")")
    if (keyCode == 13) {
        $('#get_signup').click();
        return false;
    }
});



$(document).on('click', 'button[type="reset"]', function () {
    $("#txtusremail").val("");
    $("#txtpasswd").val("");
    $("#txtnewusrnm").val("");
    $("#txtnewemail").val("");
    $("#txtnewcontact").val("");
    $("#txtnewpasswd").val("");
    $("#txtcnfmpasswd").val("");
    $("#txt_id").val("");
    $("#txt_usrnm").val("");
    $("#txt_passwd").val("");
});


// FORGOT PASWORD FIELD CLEAR FUNCTION - RIYA 
function Rclearvalue() {
    $("#terminalid").val("");
    $("#username").val("");
    $("#otpcode").val("");
    $("#password").val("");
    $("#confirmpassword").val("");
    $("#OTPspan").hide();
    $("#OTPres").hide();
    $("#OTPnew").show();

    $('#modal-forget').iziModal('close');
}


// GENERATE OTP FUNCTION
function GenerateOTP(flag) {
    if (ProductType == "ROUNDTRIP" || strAppHost == "BSA") {
        var a = $("#txtf_tid").val();
        var b = $("#txtf_uname").val();
    } else {
        var a = $("#terminalid").val();
        var b = $("#username").val();
    }

    if (a == null || a == undefined || a == "") {
        showError("Please enter terminal id")
        ProductType == "ROUNDTRIP" || strAppHost == "BSA" ? $("#txtf_tid").focus() : $("#terminalid").focus();
        return false;
    }
    if (b == null || b == undefined || b == "") {
        showError("Please enter username")
        ProductType == "ROUNDTRIP" || strAppHost == "BSA" ? $("#txtf_uname").focus() : $("#username").focus();
        return false;
    }

    var params = {
        strOTPtype: "LP",
        strOTPname: flag,
        strTerminalid: a.toUpperCase().trim(),
        strUsername: b,
        strTerminalType: "W",
        Type: $("#hdn_passwordflag").val()
    }

    $.ajax({
        type: "POST",
        url: ForgetPassword,
        data: JSON.stringify(params),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.status == "01") {
                $("#OTPres").show();
                $("#OTPnew").hide();
                $("#OTPspan").show();
            }
            else {
                showError(result.errMsg);
            }
        },
        error: function (e) {

        }
    });
}

// AGENT CHANGE PASSWORD FUNCTION
function Resetpassword() {
    if (ProductType == "ROUNDTRIP" || strAppHost == "BSA") {
        var a = $("#txtf_tid").val();
        var b = $("#txtf_uname").val();
        var otp = $("#txtOTP").val();
        var newpass = $("#txtf_password").val();
        var confirmpass = $("#txtf_cpassword").val();
    } else {
        var a = $("#terminalid").val();
        var b = $("#username").val();
        var otp = $("#otpcode").val();
        var newpass = $("#password").val();
        var confirmpass = $("#confirmpassword").val();
    }

    if (a == null || a == undefined || a == "") {
        showError("Please enter terminal id", "Forget");
        return false;
    }
    if (b == null || b == undefined || b == "") {
        showError("Please enter username", "Forget");
        return false;
    }
    if (otp == null || otp == undefined || otp == "") {
        showError("Please enter OTP Number", "Forget");
        return false;
    }
    if (newpass == "" || confirmpass == "") {
        showError("Please enter password details", "Forget");
        return false;
    }
    if (newpass != confirmpass) {
        showError("New Password & Confirm Password are not Same", "Forget");
        return false;
    }

    var params = {
        strOTPtype: "LP",
        strTerminalid: a,
        strUsername: b,
        strOTP: otp,
        strPassword: newpass,
        strTerminalType: "W",
        description: "",
        OTPFor: "F"
    }
    $("#resetpwdload").show();
    $.ajax({
        type: "POST",
        url: ResetPassword,
        data: JSON.stringify(params),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#resetpwdload").hide();
            if (result.status == "00") {
                showError(result.errMsg, "Forget");
            }
            else {
                $(".forgotpasword_close").trigger('click');
                infoAlert(result.errMsg, "Forget");
            }
        },
        error: function (e) {
            $("#resetpwdload").hide();
            console.log(e);
            alert("unable to generate OTP");
            return false;
        }
    });
}

function infoAlert(MSG, ARG) {
    $('#modal-alert').iziModal('destroy');
    $("#modal-alert").iziModal({
        title: MSG,
        icon: 'fa fa-info',
        headerColor: '#5bbd72',
        width: "500px",
        onClosed: function () {
            if (ARG == "UPDATE") {
                $(".Changepassword_close").trigger('click');
                $("#txt_passwd").val("");
                $("#txt_usrnm").val("");
                Clearupdatepassword();
            }
            else {
                $(".forgotpasword_close").trigger('click');
                Rclearvalue();
            }
        }
    });
    $('#modal-alert').iziModal('open');
    return false;
}

$(document).on('input', '.clsAlpha', function () {
    var txtQty = $(this).val().replace(/[^a-zA-Z\ ]/g, '');
    txtQty = txtQty.replace(/  +/g, ' ');
    $(this).val(txtQty);
});
$(document).on('input', '.clsAlphaNumeric', function () {
    var txtQty = $(this).val().replace(/[^a-zA-Z0-9\ ]/g, '');
    txtQty = txtQty.replace(/  +/g, ' ');
    $(this).val(txtQty);
});
$(document).on('input', '.clsNumeric', function () {
    var txtQty = $(this).val().replace(/[^0-9]/g, '');
    $(this).val(txtQty);
});

$(document).on('input', '.clsPassword', function () {
    var txtQty = $(this).val();
    if (txtQty == null || txtQty == undefined || txtQty == "") {
        $(this).attr("Type", "Text");
    }
    else {
        $(this).attr("Type", "Password");
    }
});

// UPDATE PASSWORD FUNCTION
function Updatepassword() {
    var TerminalId = $("#txt_TerminalId").val();
    var Username = $("#txt_Username").val();
    var Oldpwd = $("#txt_Oldpassword").val();
    var Newpwd = $("#txt_Newpassword").val();
    var Confirmpwd = $("#txt_Confirmpassword").val();

    if (Oldpwd == "") {
        showError("Please enter the Old Password!", "Update");
        $("#txt_Oldpassword").focus();
        return false;
    }
    else if (Newpwd == "") {
        showError("Please enter the New Password!", "Update");
        $("#txt_Newpassword").focus();
        return false;
    }
    else if (Confirmpwd == "") {
        showError("Please enter the Confirm Password!", "Update");
        $("#txt_Confirmpassword").focus();
        return false;
    }
    else if (Oldpwd == Newpwd) {
        showError("Old password and New Password should not same !", "Update");
        $("#New_pwdd").focus();
        return false;
    }
    if (Newpwd != Confirmpwd) {
        showError("New password doesnt match with confim password !", "Update");
        $("#txt_Confirmpassword").focus();
        return false;
    }
    $("#updatepwdload").show();
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: UpdatePassword,
        data: '{strOldpwd: "' + Oldpwd + '",strNewpwd: "' + Newpwd + '",strTerminalId: "' + TerminalId + '",strUsername: "' + Username + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {
            $("#updatepwdload").hide();
            var result = json.Result;
            if (result[0] != "") {
                showError(result[0], "Update");
            }
            else {
                $(".Changepassword_close").trigger('click');
                infoAlert(result[1], "UPDATE");
            }
        },
        error: function (e) {
            $("#updatepwdload").hide();
            if (e.status == "500") {
                showError("An Internal Problem Occurred. Your Session will Expire.", "Update");
                window.location.href = SessionExpire;
                return false;
            }
        }
    });
}
function Clearupdatepassword() {
    $("#txt_TerminalId").html("");
    $("#txt_Username").html("");
    $("#txt_Oldpassword").val("");
    $("#txt_Newpassword").val("");
    $("#txt_Confirmpassword").val("");
    //$('#modal-Changepassword').iziModal('close');
}

//LOG OUT FUNCTION
function logoutfun() {

    sessionStorage.setItem('logoutinsertflg', "");
    sessionStorage.removeItem('chcktouchpointlog');
    var param = {
        termID: localStorage.getItem("agnterminalid"),
        agnID: localStorage.getItem("agnagentid")
    };
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: ProductType == "ROUNDTRIP" || strAppHost == "BSA" ? LogoutURL : Logout,// Location of the service
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(param),
        dataType: "json",
        success: function (json) {//On Successful service call
            if (json.Status == "01") {
                console.log("Logout Succeded.");
            }
            else {
                console.log(json.Message != "" ? json.Message : "Problem occured while insert logout details. (#03).");
            }
            //Do Stuff when we need...
        },
        error: function (e) {//On Successful service call
            console.log("Internal Problem occured while insert logout details. (#07).");
        }	// When Service call fails
    });
}

function showError_exp(msg, errPtagid, errtagid, focusid) {
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

// PASSWORD CHANGE FUNCTION -  RIYA BOA 
function submitchangepwd() {
    if ($("#Old_pwdd").val() == "") {
        showError_exp("Please enter the Old Password!", errPdiv, errdiv_msg, "");
        $("#Old_pwdd").focus();
        return false;
    }
    else if ($("#New_pwdd").value == "") {
        showError_exp("Please enter the New Password!", errPdiv, errdiv_msg, "");
        $("#New_pwdd").focus();
        return false;
    }
    else if ($("#Cnf_pwdd").value == "") {
        showError_exp("Please enter the Confirm Password!", errPdiv, errdiv_msg, "");
        $("#Cnf_pwdd").focus();
        return false;
    }
    else if ($("#Old_pwdd").value == $("#New_pwdd").value) {
        showError_exp("Old password and New Password should not same !", errPdiv, errdiv_msg, "");
        $("#New_pwdd").focus();
        return false;
    }
    if ($("#New_pwdd").value != $("#Cnf_pwdd").value) {
        showError_exp("New password doesnt match with confim password !", errPdiv, errdiv_msg, "");
        $("#Cnf_pwdd").focus();
        return false;
    }

    $('#iLoading_x').show();


    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: RBOAChangePassword,
        data: '{oldpasswd: "' + document.getElementById("Old_pwdd").value + '",newpasswd: "' + document.getElementById("New_pwdd").value + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call
            var result = json.Result;
            if (result[0] != "") {
                showError_exp(result[0], errPdiv, errdiv_msg, "");
                window.location.href = Login;
            }
            else {
                showalert("Password updated successfully. Please login with new password", "OK", "");
                $("#Old_pwdd").val("");
                $("#New_pwdd").val("");
                $("#Cnf_pwdd").val("");

                $("#Old_pwdd").removeClass("has-content");
                $("#New_pwdd").removeClass("has-content");
                $("#Cnf_pwdd").removeClass("has-content");
            }
            $("#modal-changespassword").iziModal('close');
            $("#iLoading_x").hide();
            $("#txt_passwd")[0].value = "";
            logoutfun();
        },
        error: function (e) {// When Service call fails                           
            if (e.status == "500") {
                showError_exp("An Internal Problem Occurred. Your Session will Expire.", errPdiv, errdiv_msg, "");
                window.location.href = SessionExpire;
                return false;
            }
            $("#iLoading_x").hide();
            $("#modal-changespassword").iziModal('close');
        }
    });
}

function ShowHidePassword(arg) {
    var obj = document.getElementById(arg);
    if (obj.type.toUpperCase() == "PASSWORD") {
        obj.type = "text";
        $('.spneye .fa').removeClass('fa-eye');
        $('.spneye .fa').addClass('fa-eye-slash');
    }
    else {
        obj.type = "password";
        $('.spneye .fa').removeClass('fa-eye-slash');
        $('.spneye .fa').addClass('fa-eye');
    }
}