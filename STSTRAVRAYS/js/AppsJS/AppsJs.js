var scrollLock = true;
var FilterLock = true;
var UIDCORP = "";

$(".comlisttriptype").click(function () {
    var checkedid = this.id;
    hideoldavaildetails();

    if (checkedid == "limulticity") { //For Multicity Show other div...
        $('#dvNormalSearch').hide();
        $('#dvMSearch').slideDown();
    }
    else {
        $('#dvMSearch').hide();
        $('#dvNormalSearch').slideDown();
    }

    if ($('#hdn_producttype').val() == "RIYA" && (checkedid == "limulticity" || checkedid == "liRoundTripSpl")) {
        $('#MultiFareDiv').hide();
        $('input.clickfare').attr('checked', false);
    }
    else if ($('#hdn_producttype').val() != "RIYA" && checkedid == "limulticity") {
        $('#MultiFareDiv').hide();
        $('input.clickfare').attr('checked', false);
    }
    else {
        $('#MultiFareDiv').show();
    }

    $('#callFar').hide();
    if (checkedid == "lioneways") {
        $("#FlightName").attr("disabled", false);
        $(".comlisttriptype").removeClass("active");
        $("#lioneways").addClass("active");
        $("#txtarrivaldate").attr("disabled", true);
        $('#txtarrivaldate').parent().css('background-color', '#eee');
        $('#txtarrivaldate').css('cursor', 'not-allowed');
        $('.arrivacal').css('display', 'none');
        $('.deptvacal').css('width', '100%');
        $('.DeptDateboxs').removeClass('DeptDateboxs_edit');
        $("body").data("tripflag", "O");
        $("#roundfltnum").css("visibility", "hidden");
        $("#roundfltseg").css("visibility", "hidden");

        $("#fli_option option[value='ALL']").remove();
        $("#fli_option").append('<option value="ALL">ALL</option>');
        $("#fli_option")[0].value = "ALL";



        //  airlinecityloadfun('Travrasri');


        if ($("#hdnAppTheme").val() == "THEME2") {
            $("#designdropnic").removeClass("col-sm-3 col-xs-12");
            $("#designdropnic").addClass("col-sm-6 col-xs-12");
            $("#roundtrsricon").css("display", "none");
        }

        else if ($("#hdnAppTheme").val() == "THEME3" || $("#hdnAppTheme").val() == "THEME4" || $("#hdnAppTheme").val() == "THEME5") {
            $("#themeroundtrsricon").css("display", "none");
            $("#roundtrsricon").css("visibility", "hidden");
        }


        else {
            $("#roundtrsricon").css("visibility", "hidden");
        }

        $("#rd_roundtripcnt")[0].checked = false;

    }
    else if (checkedid == "liRoundTrip") { //Roundtrip...
        $("#FlightName").attr("disabled", false);
        $(".comlisttriptype").removeClass("active");
        $("#liRoundTripSpl").addClass("active");
        $('.arrivacal').css('display', 'block');
        $('.deptvacal').css('width', '50%');
        $('.DeptDateboxs').addClass('DeptDateboxs_edit');
        $("#txtarrivaldate").attr("disabled", false);
        $('#txtarrivaldate').parent().css('background-color', '#fff');
        $('#txtarrivaldate').css('cursor', 'pointer');
        $("body").data("tripflag", "Y");
        $("#rd_roundtripcnt")[0].checked = false;

        $("#fli_option option[value='ALL']").remove();
        $("#fli_option").append('<option value="ALL">ALL</option>');
        $("#fli_option")[0].value = "ALL";


        if ($("#hdnAppTheme").val() == "THEME2") {
            $("#designdropnic").removeClass("col-sm-6 col-xs-12");
            $("#designdropnic").addClass("col-sm-3 col-xs-12");
            $("#roundtrsricon").css("display", "block");
        }
        else {

            if ($("#hdnAppTheme").val() == "THEME3" || $("#hdnAppTheme").val() == "THEME4") {
                if ($("#twooneway")[0].value == "1") {
                    if ($("#hdn_producttype").val() == "RIYA") {
                        if ($("#international")[0].checked == true) {
                            $("#themeroundtrsricon").css("display", "block");
                            $("#roundtrsricon").css("visibility", "visible");
                        }
                        else {
                            $("#themeroundtrsricon").css("display", "none");
                            $("#roundtrsricon").css("visibility", "hidden");
                        }
                    }
                }
                else {
                    $("#themeroundtrsricon").css("display", "none");
                    $("#roundtrsricon").css("visibility", "hidden");
                }
            }
            else {
                if ($("#twooneway").val() == "1") {
                    $("#roundtrsricon").css("visibility", "visible");
                }
                else {
                    $("#roundtrsricon").css("visibility", "hidden");
                }
            }


        }
        $("#roundfltnum").css("visibility", "visible");
        $("#roundfltseg").css("visibility", "visible");
        //      airlinecityloadfun('Travrasri');

    }
    else if (checkedid == "liRoundTripSpl") { // Roundtrip Special...
        $("#FlightName").attr("disabled", false);
        $(".comlisttriptype").removeClass("active");
        $("#liRoundTripSpl").addClass("active");
        $('.arrivacal').css('display', 'block');
        $('.deptvacal').css('width', '50%');
        $('.DeptDateboxs').addClass('DeptDateboxs_edit');
        $("#txtarrivaldate").attr("disabled", false);
        $('#txtarrivaldate').parent().css('background-color', '#fff');
        $('#txtarrivaldate').css('cursor', 'pointer');
        $("body").data("tripflag", "Y");
        $("#rd_roundtripcnt")[0].checked = false;


        if ($("#hdnAppTheme").val() == "THEME2") {
            $("#designdropnic").removeClass("col-sm-6 col-xs-12");
            $("#designdropnic").addClass("col-sm-3 col-xs-12");
            $("#roundtrsricon").css("display", "block");
        }
        else {
            if ($("#hdnAppTheme").val() == "THEME3" || $("#hdnAppTheme").val() == "THEME4" || $("#hdn_AppHosting").val() == "BSA") {
                if ($("#twooneway")[0].value == "1") {
                    if ($("#hdn_producttype").val() == "RIYA" || $("#hdn_AppHosting").val() == "BSA") {
                        $("#themeroundtrsricon").css("display", "none");
                        $("#roundtrsricon").css("visibility", "hidden");
                        $("#rd_roundtripcnt").checked = true;
                        $("#fli_option option[value='ALL']").remove();
                        $("#fli_option")[0].value = "LCC";
                        $("#FlightName").val("");
                        $("#FlightName").attr("disabled", true);
                    }
                    else {
                        $("#FlightName").attr("disabled", false);
                        if ($("#hdn_producttype").val() == "JOD") {
                            $("#themeroundtrsricon").css("display", "none");
                            $("#roundtrsricon").css("visibility", "hidden");
                        }
                        else {
                            $("#themeroundtrsricon").css("display", "block");
                            $("#roundtrsricon").css("visibility", "visible");
                        }

                    }
                }
                else {
                    $("#themeroundtrsricon").css("display", "none");
                    $("#roundtrsricon").css("visibility", "hidden");
                }
            }
            else {
                if ($("#hdn_producttype").val() == "JOD") {
                    $("#themeroundtrsricon").css("display", "none");
                    $("#roundtrsricon").css("visibility", "hidden");
                }
                else {
                    if ($("#twooneway").val() == "1") {
                        $("#roundtrsricon").css("visibility", "visible");
                    }
                    else {
                        $("#roundtrsricon").css("visibility", "hidden");
                    }
                }

            }


        }
        $("#roundfltnum").css("visibility", "visible");
        $("#roundfltseg").css("visibility", "visible");
        //      airlinecityloadfun('Travrasri');
    }
    else {
        $("#FlightName").attr("disabled", false);
        $(".comlisttriptype").removeClass("active");
        $("#limulticity").addClass("active");
        $("#txtarrivaldate").attr("disabled", true);
        $('#txtarrivaldate').parent().css('background-color', '#eee');
        $('#txtarrivaldate').css('cursor', 'not-allowed');
        $("body").data("tripflag", "M");
        //$("#roundtrsricon").css("display", "none");
        // RecentSearchBuild('M');
    }
    $(".clsAirportID").change();
    $("#hdtxt_trip").val($("body").data("tripflag").trim());
});

$(".commontoggle").click(function (e) {
    var viewed_id = this.id;
    $("#divloadviewdetailsToogle_" + viewed_id.split("_")[1]).slideToggle();
});

function checkactive() {
    if ($("#chbdomestic")[0].checked == true) {
        $(".segmenttab").removeClass("active");
        $(".seldomestic").addClass("active");
    } else if ($("#chbinternational")[0].checked == true) {
        $(".segmenttab").removeClass("active");
        $(".selinternational").addClass("active");
    }
}

var AvailRequestCount = 0;
var AvailResponseCount = 0;
var Ttotlen = 0;
var flight_cat = [];
var FilterLccAirline = [];
var load_Increase = 0;
var totflt = 0;
var _ArrayFare = "";
var SelectedsplThread = "";

$(document).ready(function () {/**Document Ready Starts**/
    Ttotlen = 0;
    _ArrayFare = new Array();
});/**Document Ready End**/

function AdultLoad() {

    try {
        var getAdult = document.getElementById('ddladult');
        var getChild = document.getElementById('ddlchild');
        var getInfant = document.getElementById('ddlinfant');
        if (getChild.length != 0)
            getChild.length = 0;
        if (getInfant.length != 0)
            getInfant.length = 0;
        for (var i = 0; i <= getAdult.selectedIndex + 1; i++) {
            if (i <= 4) {
                var infantOption = new Option();
                if (i == 0) {
                    infantOption.value = "0";
                    infantOption.text = "Infant";
                }
                else {
                    infantOption.value = i;
                    infantOption.text = i;
                }
                getInfant[i] = infantOption;
            }
        }
        for (var i = 0; i < (9 - getAdult.selectedIndex) ; i++) {
            var childOption = new Option();
            if (i == 0) {
                childOption.value = "0";
                childOption.text = "Child";
            } else {
                childOption.value = i;
                childOption.text = i;
            }
            getChild[i] = childOption;
        }

        document.getElementById('ddlchild').selectedIndex = 0;
        document.getElementById('ddlinfant').selectedIndex = 0;

        $('#hdtxt_adultcount').val($('#ddladult').val());
        $('#hdtxt_childcount').val($('#ddlchild').val());
        $('#hdtxt_infantcount').val($('#ddlinfant').val());

        if ($("#hdnAppTheme").val() != "THEME3" && $("#hdnAppTheme").val() != "THEME4") {
            $('#ddladult').niceSelect('destroy').niceSelect();
            $('#ddlchild').niceSelect('destroy').niceSelect();
            $('#ddlinfant').niceSelect('destroy').niceSelect();
        }

        if ($("#hdnAppTheme").val() == "THEME3" || $("#hdnAppTheme").val() == "THEME4" || $("#hdnAppTheme").val() == "THEME5") {
            Travellersclass();
        }
    }
    catch (ex) {

    }
}

function getChildCount() {
    hideoldavaildetails();
    $("#hdtxt_childcount").val($('#ddlchild').val());
}

function getInfantCount() {
    hideoldavaildetails();
    $("#hdtxt_infantcount").val($('#ddlinfant').val());
}


$("#ddlchild,#ddlinfant").change(function () {
    $("#hdtxt_childcount").val($('#ddlchild').val());
    $("#hdtxt_infantcount").val($('#ddlinfant').val());
    hideoldavaildetails();
    if ($("#hdnAppTheme").val() == "THEME3" || $("#hdnAppTheme").val() == "THEME4" || $("#hdnAppTheme").val() == "THEME5") {
        Travellersclass();
    }
});

/* Flights Travellers and Class */
$('#FlightTravellersClass').click(function () {
    $('.travellers-dropdown').slideToggle('fast');
    var ddlAdult = $('#ddladult').val() != null && $('#ddladult').val() != "" ? $('#ddladult').val() : "0";
    var ddlChild = $('#ddlchild').val() != null && $('#ddlchild').val() != "" ? $('#ddlchild').val() : "0";
    var ddlInfant = $('#ddlinfant').val() != null && $('#ddlinfant').val() != "" ? $('#ddlinfant').val() : "0";
    var TotalCount = parseInt(ddlAdult) + parseInt(ddlChild) + parseInt(ddlInfant);
    var Cabin = "";
    $('.flight-class').each(function () {
        if (this.checked == true) {
            $("#ddlclass")[0].value = this.value;
            Cabin = $("input[name='flight-class']:checked")["0"].dataset.value; //this.labels["0"].innerHTML;
            return Cabin;
        }
    });
    $('#FlightTravellersClass').val(TotalCount + ' Traveller(s) - ' + Cabin);
});

$('.flight-class').on('click', function () {
    var ddlAdult = $('#ddladult').val() != null && $('#ddladult').val() != "" ? $('#ddladult').val() : "0";
    var ddlChild = $('#ddlchild').val() != null && $('#ddlchild').val() != "" ? $('#ddlchild').val() : "0";
    var ddlInfant = $('#ddlinfant').val() != null && $('#ddlinfant').val() != "" ? $('#ddlinfant').val() : "0";
    var TotalCount = parseInt(ddlAdult) + parseInt(ddlChild) + parseInt(ddlInfant)
    var Cabin = "";
    $('.flight-class').each(function () {
        if (this.checked == true) {
            $("#ddlclass")[0].value = this.value;
            Cabin = $("input[name='flight-class']:checked")["0"].dataset.value; //this.labels["0"].innerHTML;
            return Cabin;
        }
    });
    $('#FlightTravellersClass').val(TotalCount + ' Traveller(s) - ' + Cabin);
});

function Travellersclass() {
    var ddlAdult = $('#ddladult').val() != null && $('#ddladult').val() != "" ? $('#ddladult').val() : "0";
    var ddlChild = $('#ddlchild').val() != null && $('#ddlchild').val() != "" ? $('#ddlchild').val() : "0";
    var ddlInfant = $('#ddlinfant').val() != null && $('#ddlinfant').val() != "" ? $('#ddlinfant').val() : "0";
    var TotalCount = parseInt(ddlAdult) + parseInt(ddlChild) + parseInt(ddlInfant);
    var Cabin = $("input[name='flight-class']:checked")["0"].dataset.value;// this.labels["0"].innerHTML;
    $('.flight-class').each(function () {
        if (this.checked == true) {
            $("#ddlclass")[0].value = this.value;
            Cabin = $("input[name='flight-class']:checked")["0"].dataset.value; //this.labels["0"].innerHTML;
            return Cabin;
        }
    });
    $('#FlightTravellersClass').val(TotalCount + ' Traveller(s) - ' + Cabin);
}


/* Hide dropdown when clicking outside */
$(document).on('click', function (event) {
    if (!$(event.target).closest(".travellers-class").length) {
        $(".travellers-dropdown").hide();
    }

    /* Hide dropdown when clicking on Done Button */
    $('.submit-done').on('click', function () {
        var ddlAdult = $('#ddladult').val() != null && $('#ddladult').val() != "" ? $('#ddladult').val() : "0";
        var ddlChild = $('#ddlchild').val() != null && $('#ddlchild').val() != "" ? $('#ddlchild').val() : "0";
        var ddlInfant = $('#ddlinfant').val() != null && $('#ddlinfant').val() != "" ? $('#ddlinfant').val() : "0";
        var TotalCount = parseInt(ddlAdult) + parseInt(ddlChild) + parseInt(ddlInfant);
        var Cabin = $("input[name='flight-class']:checked")["0"].dataset.value;// this.labels["0"].innerHTML;
        var ss = $('.flight-class').each(function () {
            if (this.checked == true) {
                $("#ddlclass")[0].value = this.value;
                Cabin = $("input[name='flight-class']:checked")["0"].dataset.value; //this.labels["0"].innerHTML;
                return Cabin;
            }
        });
        $('#FlightTravellersClass').val(TotalCount + ' Traveller(s) - ' + Cabin);
        $('.travellers-dropdown').fadeOut(function () {
            $(this).hide();
        });

    });
});

function disblockall() {
    document.getElementById("fli_option").value = "ALL";
    if ($('#FlightName').val() != null && $('#FlightName').val().trim() != "") {
        //document.getElementById("fli_option").disabled = true;
        hideoldavaildetails();
        $('#fli_option').prop('disabled', true).niceSelect('update');
    } else {
        //document.getElementById("fli_option").disabled = false;
        hideoldavaildetails();
        $('#fli_option').prop('disabled', false).niceSelect('update');
    }
}
function Checklabour() {
    document.getElementById("fli_option").value = "ALL";
    if ($('#rd_labour')[0].checked == true) {
        hideoldavaildetails();
        $('#fli_option').prop('disabled', true).niceSelect('update');
        $('#FlightName').val("");
        document.getElementById("FlightName").disabled = true;
    } else {
        hideoldavaildetails();
        $('#fli_option').prop('disabled', false).niceSelect('update');
        $('#FlightName').val("");
        document.getElementById("FlightName").disabled = false;
    }
}
function blockviaother() {
    document.getElementById("fli_option").value = "ALL";
    if (($('#Via1').val() != null && $('#Via1').val().trim() != "") || ($('#Via2').val() != null && $('#Via2').val().trim() != "")) {
        hideoldavaildetails();
        $('#fli_option').prop('disabled', true).niceSelect('update');
        $('#FlightName').val("");
        document.getElementById("FlightName").disabled = true;
    } else {
        hideoldavaildetails();
        $('#fli_option').prop('disabled', false).niceSelect('update');
        $('#FlightName').val("");
        document.getElementById("FlightName").disabled = false;
    }
}
function RecentSearch2Avail(arg) {
    var a = $(arg).data('params');
    var strtrp = a[0].strTrip;
    $(".comlisttriptype").removeClass("active");
    if (strtrp == "O") {
        $('#lioneways')[0].click();
        //$('#lioneways')[0].checked = true;
        //$("#lioneways").addClass("active");
    }
    else if (strtrp == "R") {
        $('#liRoundTrip')[0].click();
        //$('#liRoundTrip')[0].checked = true;
        //$("#liRoundTrip").addClass("active");
    }
    else if (strtrp == "Y") {
        $('#liRoundTripSpl')[0].click();
        //$('#liRoundTripSpl')[0].checked = true;
        //$("#liRoundTripSpl").addClass("active");
    }
    else {
        $('#limulticity')[0].click();
        //$('#limulticity')[0].checked = true;
        //$("#limulticity").addClass("active");
    }
    //$("body").data("tripflag", strtrp);
    $('#txtorigincity').val(loadairportcityname(a[0].strfromCity) + "-(" + a[0].strfromCity + ")");
    $('#txtdestinationcity').val(loadairportcityname(a[0].strtoCity) + "-(" + a[0].strtoCity + ")");
    $('#txtdeparture').val(a[0].strDepartureDate);
    $('#txtarrivaldate').val(a[0].strRetDate);
    $('#ddladult').val(a[0].strAdults);
    $('#ddlchild').val(a[0].strChildrens);
    $('#ddlinfant').val(a[0].strInfants);
    $('#ddladult').niceSelect('destroy').niceSelect();
    $('#ddlchild').niceSelect('destroy').niceSelect();
    $('#ddlinfant').niceSelect('destroy').niceSelect();
    btn_Search("R");//D- Direct Search, R-Recent Search
}
//$("#btn_Search").click(function () {

function Searchflights(e) {  //STS185    
    origincity = $("#txtorigincity").val().trim();
    if (origincity.split("(").length > 1) {
        $("#hdtxt_origin").val(origincity.split("(")[1].split(")")[0].trim());
        $("#hd_org_fu").val(origincity.split("-")[0].trim());
    }

    else {
        msg = "Please select valid origin city.";
        showError1(msg);
        return false;
    }
    aryorgcity.push($("#hdtxt_origin").val());
    destinationcity = $("#txtdestinationcity").val().trim();
    if (destinationcity.split("(").length > 1) {
        $("#hdtxt_destination").val(destinationcity.split("(")[1].split(")")[0].trim());
        $("#hd_des_fu").val(destinationcity.split("-")[0].trim());
    }
    else {
        msg = "Please select valid destination city.";
        showError1(msg);
        return false;
    }


    var new_arr = $.grep(loadGlobalcityArrry, function (n, i) {
        return n.ID == $("#hdtxt_origin").val();
    });
    var new_arr_des = $.grep(loadGlobalcityArrry, function (n, i) {
        return n.ID == $("#hdtxt_destination").val();
    });

    $("#hdn_destincntry").val(new_arr_des[0].CN);
    $("#hdn_origincntry").val(new_arr[0].CN);

    segmenttype = new_arr[0].CN == assignedcountry && new_arr_des[0].CN == assignedcountry ? "D" : "I";

    if ($("#hdn_producttype").val() == "RIYA") {

        if ($("#international")[0].checked == true && segmenttype == "D") {
            msg = "Please select international city only.";
            showError1(msg);
            return false;
        }
        if ($("#domastic")[0].checked == true && segmenttype == "I") {
            msg = "Please select domestic city only.";
            showError1(msg);
            return false;
        }

        if (segmenttype == "D" && $("#liRoundTripSpl")[0].checked == true && $("#fli_option").val() == "LCC") {
            $("#rd_roundtripcnt")[0].checked = true;
            $("#hdn_rtsplflag").val("Y");
            roundtrip_or_tripspl();
        }
        else if (segmenttype == "D" && $("#liRoundTripSpl")[0].checked == true && ($("#fli_option").val() == "FSC" || $("#fli_option").val() == "ALL")) {
            $("#rd_roundtripcnt")[0].checked = false;
            $("#hdn_rtsplflag").val("N");
            roundtrip_or_tripspl();
        }
        else if (segmenttype == "D" && $("#liRoundTrip")[0].checked == true) {
            $("#rd_roundtripcnt")[0].checked = true;
            $("#hdn_rtsplflag").val("N");
            roundtrip_or_tripspl();
        }
        else {
            if (segmenttype == "I" && ($("body").data("tripflag").trim() == "Y" || $("body").data("tripflag").trim() == "R")) {
                roundtrip_or_tripspl();
                $("#hdn_rtsplflag").val("N");
            }
            else {
                $("#hdn_rtsplflag").val("N");
            }
        }
    }
    else if (segmenttype == "D" && $("#liRoundTrip").is(":checked") == true) {
        $("#rd_roundtripcnt")[0].checked = true;
        $("#hdn_rtsplflag").val("N");
        roundtrip_or_tripspl();
    }
    else if (segmenttype == "I" && ($("body").data("tripflag").trim() == "Y" || $("body").data("tripflag").trim() == "R")) {
        roundtrip_or_tripspl();
        $("#hdn_rtsplflag").val("N");
    }
    else {
        $("#hdn_rtsplflag").val("N");
    }

    assignedcurrency = $('#ddlNationality').val();//STS-166
    $('.clscurrency').html(assignedcurrency);
    $("#HDN_CURRENCY_code").val(assignedcurrency);
    sessionreq = 0;

    if ($("#rd_corporate_retail").prop("checked") == false) {//STS185
        $('body').data("bhdcorporatefaredt", "");
    }

    SelectedsplThread = "";
    $('input.clickfare[type=checkbox]').each(function () {
        if (this.checked) {
            var rdn_splthreadid = $('input[name=Faredivide]:checked')[0].id;
            SelectedsplThread = $("#hdn_" + rdn_splthreadid + "").val();
        }
    });

    if ($("#rd_corporate_retail").prop("checked") == true && e != 'C') {//STS185
        var clientid = ""
        if (TerminalType == "T") {
            if ($("#ddlclient").length > 0) {
                if (sessionStorage.getItem("clientid") == "" || sessionStorage.getItem("clientid") == 'undefined' || sessionStorage.getItem("clientid") == null || $('#ddlclient').val() == "" || $('#ddlclient').val() == "0") {
                    msg = "Please Select Agency name";
                    showError1(msg);
                    return false;
                }
            }
            if ($('#ddlTerminalId').length > 0) {
                if ($('#ddlTerminalId').val() == "") {
                    showError1("Please Select Terminal ID");
                    return false;
                }
                clientid = $('#ddlTerminalId').val();
            }
        }
        var airlinecodes = $("#FlightName").val();
        airlinecodes = airlinecodes != null ? airlinecodes : [];
        $.ajax({
            type: "POST",   //GET or POST or PUT or DELETE verb
            contentType: "application/json; charset=utf-8",
            url: Getcorporatefare, 		// Location of the service
            data: "{'AirlineCodes': '" + airlinecodes.join(',').toUpperCase() + "','strClientId': '" + clientid + "'}",
            dataType: "json",
            timeout: 180000,
            success: function (data) {
                var Result = data.Results;
                if (data.Status == "-1") {
                    window.location.href = sessionExb;
                    return false;
                }
                else if (data.Status == "00") {
                    msg = data.Error;
                    showError1(msg);
                    return false;
                }
                else {
                    if ((Result[1] != null && Result[1] != "") || (Result[2] != null && Result[2] != "")) {
                        if (Result[1] != null && Result[1] != "") {
                            $('#dvCorporatedetails', window.parent.document).html(Result[1]);
                        }
                        else {
                            $('#tabcorporate').remove();
                            $('#Corporatedetails').remove();
                        }
                        if (Result[2] != null && Result[2] != "") {
                            $('#dvRetaildetails', window.parent.document).html(Result[2]);
                        }
                        else {
                            $('#tabretail').remove();
                            $('#Retaildetails').remove();
                        }
                        if (Result[3] != null && Result[3] != "") {
                            $('#dvSMEdetails', window.parent.document).html(Result[3]);
                            $('#tabsme').show();
                            $('#SMEdetails').show();
                        }
                        else {
                            $('#SMEdetails').remove();
                            $('#tabsme').remove();
                        }
                        if (Result[4] != null && Result[4] != "") {
                            $('#dvMarinedetails', window.parent.document).html(Result[4]);
                        }
                        else {
                            $('#tabmarine').remove();
                            $('#Marinedetails').remove();
                        }
                        window.parent.Modalpopupshowing(2);
                    }
                }
            },
            error: function (result) {
            }
        });
        return false;
    }

    if ($("#hdn_producttype").val() == "UAE" && $("#rd_labour").prop("checked") == true) {
        if (searchvalidation()) {
            var Origincode = "";
            var Destinationcity = "";
            var Airlinecode = $("#FlightName").val();
            Airlinecode = Airlinecode != null ? Airlinecode : [];
            Airlinecode = Airlinecode.join(',')
            origincity = $("#txtorigincity").val().trim();
            if (origincity.split("(").length > 1) {
                Origincode = origincity.split("(")[1].split(")")[0].trim();
            }
            else {
                msg = "Please select valid origin city.";
                showError1(msg);
                return false;
            }
            destinationcity = $("#txtdestinationcity").val().trim();
            if (destinationcity.split("(").length > 1) {
                Destinationcity = destinationcity.split("(")[1].split(")")[0].trim();
            }
            else {
                msg = "Please select valid Destination city.";
                showError1(msg);
                return false;
            }

            if ($("#ddlchild").val() > 0 || $("#ddlinfant").val() > 0) {
                msg = "Labour fare available only for adult passenger";
                showError1(msg);
                return false;
            }

            if (TerminalType == "T") {
                if ($("#ddlclient").length > 0) {
                    if (sessionStorage.getItem("clientid") == "" || sessionStorage.getItem("clientid") == 'undefined' || sessionStorage.getItem("clientid") == null || $('#ddlclient').val() == "" || $('#ddlclient').val() == "0") {
                        msg = "Please Select Agency name";
                        showError1(msg);
                        return false;
                    }
                }
                if ($('#ddlTerminalId').length > 0) {
                    if ($('#ddlTerminalId').val() == "") {
                        showError1("Please Select Terminal ID");
                        return false;
                    }
                    sessionStorage.setItem("clientid", $('#ddlTerminalId').val());
                }
            }

            strDepartureDate = $("#txtdeparture").val().trim();
            strRetDate = $("#txtarrivaldate").val().trim();
            Class = $("#ddlclass").val();
            var result = 0;
            var Returnmsg = "";
            var new_arr = $.grep(loadGlobalcityArrry, function (n, i) {
                return n.ID == Origincode;
            });
            var new_arr_des = $.grep(loadGlobalcityArrry, function (n, i) {
                return n.ID == Destinationcity;
            });

            $("#hdn_origincntry").val(new_arr[0].CN);
            $("#hdn_destincntry").val(new_arr_des[0].CN);

            segmenttype = new_arr[0].CN == assignedcountry && new_arr_des[0].CN == assignedcountry ? "D" : "I";

            if ($("#ddlclient").length > 0) {
                ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
                BranchID = sessionStorage.getItem('branchid') != null ? sessionStorage.getItem('branchid').trim() : "";
                GroupID = sessionStorage.getItem('Groupid') != null ? sessionStorage.getItem('Groupid').trim() : "";
                Agencyname = sessionStorage.getItem('clientname') != null ? sessionStorage.getItem('clientname').trim() : "";
                availagent = ClientID;
            }
            else {

                ClientID = "";
                BranchID = "";
                GroupID = "";
            }
            var Inputparams = {
                Origincode: Origincode, Destinationcity: Destinationcity, strDepartureDate: strDepartureDate, strRetDate: strRetDate, strTrip: strTrip,
                Class: Class, segmenttype: segmenttype, AppCurrency: AppCurrency, ClientID: ClientID, Airlinecode: Airlinecode
            }
            $.ajax({
                type: "POST",   //GET or POST or PUT or DELETE verb
                contentType: "application/json; charset=utf-8",
                url: GetLabourthread, 		// Location of the service
                data: JSON.stringify(Inputparams),
                dataType: "json",
                async: false,
                success: function (data) {
                    var Result = data.Results;
                    if (Result[0] == "1") {
                        $("#hdn_labourthrd").val(Result[1]);
                        result = "1";
                    }
                    else if (Result[0] == "0" && Result[3] != "") {
                        result = "0";
                        Returnmsg = Result[3];
                    }
                    else {
                        result = "0";
                        Returnmsg = "Unable to get labour fare for this sector";
                    }
                },
                error: function (result) {
                    result = "0";
                    Returnmsg = "Unable to get labour fare for this sector";
                }
            });
        }
        if (result == "0") {
            msg = Returnmsg != "" ? Returnmsg : "Unable to get labour fare for this sector";
            showError1(msg);
            return false;
        }

    }
    else if ($("#hdn_producttype").val() == "UAE" && ($("#hdn_promothread").val() != "" || $("#hdn_dirpromothread").val() != "") && $('#hdtxt_trip').val() != "M" && $("#hdthread").val() != "") {
        if (searchvalidation()) {
            var Origincode = "";
            var Destinationcity = "";
            var Airlinecode = $("#FlightName").val();
            Airlinecode = Airlinecode != null ? Airlinecode : [];
            Airlinecode = Airlinecode.join(',')
            origincity = $("#txtorigincity").val().trim();
            if (origincity.split("(").length > 1) {
                Origincode = origincity.split("(")[1].split(")")[0].trim();
            }
            else {
                msg = "Please select valid origin city.";
                showError1(msg);
                return false;
            }
            destinationcity = $("#txtdestinationcity").val().trim();
            if (destinationcity.split("(").length > 1) {
                Destinationcity = destinationcity.split("(")[1].split(")")[0].trim();
            }
            else {
                msg = "Please select valid Destination city.";
                showError1(msg);
                return false;
            }

            if (TerminalType == "T") {
                if ($("#ddlclient").length > 0) {
                    if (sessionStorage.getItem("clientid") == "" || sessionStorage.getItem("clientid") == 'undefined' || sessionStorage.getItem("clientid") == null || $('#ddlclient').val() == "" || $('#ddlclient').val() == "0") {
                        msg = "Please Select Agency name";
                        showError1(msg);
                        return false;
                    }
                }
                if ($('#ddlTerminalId').length > 0) {
                    if ($('#ddlTerminalId').val() == "") {
                        showError1("Please Select Terminal ID");
                        return false;
                    }
                    sessionStorage.setItem("clientid", $('#ddlTerminalId').val());
                }
            }

            strDepartureDate = $("#txtdeparture").val().trim();
            strRetDate = $("#txtarrivaldate").val().trim();
            Class = $("#ddlclass").val();
            Origincode = $('#hdtxt_trip').val() == "M" ? aryOrgMul.join(',') : Origincode;
            Destinationcity = $('#hdtxt_trip').val() == "M" ? aryDstMul.join(',') : Destinationcity;

            var new_arr = $.grep(loadGlobalcityArrry, function (n, i) {
                return n.ID == Origincode;
            });
            var new_arr_des = $.grep(loadGlobalcityArrry, function (n, i) {
                return n.ID == Destinationcity;
            });

            $("#hdn_origincntry").val(new_arr[0].CN);
            $("#hdn_destincntry").val(new_arr_des[0].CN);

            segmenttype = new_arr[0].CN == assignedcountry && new_arr_des[0].CN == assignedcountry ? "D" : "I";

            if ($("#ddlclient").length > 0) {
                ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
                BranchID = sessionStorage.getItem('branchid') != null ? sessionStorage.getItem('branchid').trim() : "";
                GroupID = sessionStorage.getItem('Groupid') != null ? sessionStorage.getItem('Groupid').trim() : "";
                Agencyname = sessionStorage.getItem('clientname') != null ? sessionStorage.getItem('clientname').trim() : "";
                availagent = ClientID;
            }
            else {

                ClientID = "";
                BranchID = "";
                GroupID = "";
            }
            var Inputparams = {
                Origincode: Origincode, Destinationcity: Destinationcity, strDepartureDate: strDepartureDate, strRetDate: strRetDate, strTrip: strTrip,
                Class: Class, segmenttype: segmenttype, AppCurrency: AppCurrency, ClientID: ClientID, Airlinecode: Airlinecode
            }
            $.ajax({
                type: "POST",   //GET or POST or PUT or DELETE verb
                contentType: "application/json; charset=utf-8",
                url: Getcodecount, 		// Location of the service
                data: JSON.stringify(Inputparams),
                dataType: "json",
                async: false,
                success: function (data) {
                    var Result = data.Result;
                    if (data.Status == "1") {
                        $("#hdthread").val(Result[1]);
                        $("#hdthread_dt").val(Result[2]);
                        $("#fscthreadkey_dt").val(Result[4]);
                        $("#fscthreadkey").val(Result[3]);
                    }
                },
                error: function (result) {
                }
            });
        }
    }
    else if ($("#hdn_promothread").val() != "" && (($("#hdthread").val() != "" && $("#hdthread_dt").val() != "" && $("#hdn_producttype").val() == "RIYA") || ($("#hdthread").val() != "" && $("#hdn_producttype").val() != "RIYA"))) {
        if (searchvalidation()) {
            var Origincode = "";
            var Destinationcity = "";
            var Airlinecode = $("#FlightName").val();
            Airlinecode = Airlinecode != null ? Airlinecode : [];
            Airlinecode = Airlinecode.join(',')
            origincity = $("#txtorigincity").val().trim();
            if (origincity.split("(").length > 1) {
                Origincode = origincity.split("(")[1].split(")")[0].trim();
            }
            else {
                msg = "Please select valid origin city.";
                showError1(msg);
                return false;
            }
            destinationcity = $("#txtdestinationcity").val().trim();
            if (destinationcity.split("(").length > 1) {
                Destinationcity = destinationcity.split("(")[1].split(")")[0].trim();
            }
            else {
                msg = "Please select valid Destination city.";
                showError1(msg);
                return false;
            }

            if (TerminalType == "T") {
                if ($("#ddlclient").length > 0) {
                    if (sessionStorage.getItem("clientid") == "" || sessionStorage.getItem("clientid") == 'undefined' || sessionStorage.getItem("clientid") == null || $('#ddlclient').val() == "" || $('#ddlclient').val() == "0") {
                        msg = "Please Select Agency name";
                        showError1(msg);
                        return false;
                    }
                }
                if ($('#ddlTerminalId').length > 0) {
                    if ($('#ddlTerminalId').val() == "") {
                        showError1("Please Select Terminal ID");
                        return false;
                    }
                    sessionStorage.setItem("clientid", $('#ddlTerminalId').val());
                }
            }

            strDepartureDate = $("#txtdeparture").val().trim();
            strRetDate = $("#txtarrivaldate").val().trim();
            Class = $("#ddlclass").val();

            Origincode = $('#hdtxt_trip').val() == "M" ? aryOrgMul.join(',') : Origincode;
            Destinationcity = $('#hdtxt_trip').val() == "M" ? aryDstMul.join(',') : Destinationcity;

            var new_arr = $.grep(loadGlobalcityArrry, function (n, i) {
                return n.ID == Origincode;
            });
            var new_arr_des = $.grep(loadGlobalcityArrry, function (n, i) {
                return n.ID == Destinationcity;
            });

            $("#hdn_origincntry").val(new_arr[0].CN);
            $("#hdn_destincntry").val(new_arr_des[0].CN);

            segmenttype = new_arr[0].CN == assignedcountry && new_arr_des[0].CN == assignedcountry ? "D" : "I";

            if ($("#ddlclient").length > 0) {
                ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
                BranchID = sessionStorage.getItem('branchid') != null ? sessionStorage.getItem('branchid').trim() : "";
                GroupID = sessionStorage.getItem('Groupid') != null ? sessionStorage.getItem('Groupid').trim() : "";
                Agencyname = sessionStorage.getItem('clientname') != null ? sessionStorage.getItem('clientname').trim() : "";
                availagent = ClientID;
            }
            else {
                ClientID = "";
                BranchID = "";
                GroupID = "";
            }
            var Inputparams = {
                Origincode: Origincode, Destinationcity: Destinationcity, strDepartureDate: strDepartureDate, strRetDate: strRetDate, strTrip: $("#hdtxt_trip").val(),
                Class: Class, segmenttype: segmenttype, AppCurrency: AppCurrency, ClientID: ClientID, Airlinecode: Airlinecode
            }


            $.ajax({
                type: "POST",   //GET or POST or PUT or DELETE verb
                contentType: "application/json; charset=utf-8",
                url: GetPricingcode, 		// Location of the service
                data: JSON.stringify(Inputparams),
                dataType: "json",
                async: false,
                success: function (data) {
                    var Result = data.Result;
                    if (data.Status == "1") {
                        if ($('#hdtxt_trip').val() != "M") {
                            $("#hdthread").val(Result[1]);
                            $("#hdthread_dt").val(Result[2]);
                            $("#fscthreadkey_dt").val(Result[4]);
                            $("#fscthreadkey").val(Result[3]);
                        }
                        else {
                            $("#hdnMultiThread").val(Result[5]);
                        }
                    }
                },
                error: function (result) {
                }
            });
        }
    }


    if ($("#hdn_allowpoweruser").val() == "YES") {
        $("#modal-agentlist").show();

        $("#allbranchload").val($("#loginbranch").val());
        $("#allagentload").val($("#loginagentId").val());
        $("#allterminalload").val($("#loginterminal").val());

        Loaddefaultdatas();

        $("#modal-agentlist").modal({
            backdrop: 'static',
            keyboard: false
        });
        return false;
    }
    else {
        dataproperty_array = [];//Added by srinath on 20171006 to improve peroformance
        extrahidem_dataproperty_array = [];//Added by srinath on 20171006 to improve peroformance
        groparr = [];
        groupmain_array = [];
        groupmain_ret = [];
        filterdarray = [];
        Main_totalfilterdarray = [];
        Main_totalfilterdarray_ret = [];
        var obj1 = {
            count: 0,
            setcount: 0
        };

        $("#liern-hide")[0].checked = true;
        $('#spnEarnsort').hide('slow');
        $(".airfilterallcheck").html("");
        $("#airline_matrixhtml").html("");
        $('#spnGrandTotFare').data('fare-on', 0);
        $('#spnGrandTotFare').data('fare-rt', 0);
        $(".faretypfilterallcheck").html("");
        allAIRLINECode = "";
        Faretypefilter = "";
        btn_Search("D");  //D- Direct Search, R-Recent Search
    }

    //});
}

function Loaddefaultdatas() {

    var Loginbranch = $("#loginbranch").val();
    var Loginagent = $("#loginagentId").val();
    var Loginterminal = $("#loginterminal").val();//logintermianlcount
    var Loginterminalcnt = $("#logintermianlcount").val();

    var Branchdetails = JSON.parse(Branchmasterdetails);
    var Branchload = "";
    Branchload += "<option value=''>-- Select --</option>";
    for (var j = 0; j < Branchdetails.length; j++) {
        if (Loginbranch == Branchdetails[j].BranchId) {
            Branchload += "<option selected='selected' value=" + Branchdetails[j].BranchId + ">" + Branchdetails[j].Branchname + "</option>";
        }
        else {
            Branchload += "<option value=" + Branchdetails[j].BranchId + ">" + Branchdetails[j].Branchname + "</option>";
        }
    }
    $("#allbranchload").html(Branchload);
    $("#allbranchload").chosen('destroy');
    $("#allbranchload").chosen();

    var Agentdetails = JSON.parse(Agentmasterlist);

    var Agentload = "";
    Agentload += '<option value="">-- Select --</option>';
    for (var j = 0; j < Agentdetails.length; j++) {
        if (Loginbranch == Agentdetails[j].Branch_Id) {
            if (Loginagent == Agentdetails[j].Agent_Id) {
                Agentload += '<option selected="selected" value=\"' + Agentdetails[j].Agent_Id + '~' + Agentdetails[j].Terminal_Count + '~' + Agentdetails[j].Agency_name + '"\>' + Agentdetails[j].Agency_name + '</option>';
            }
            else {
                Agentload += '<option value=\"' + Agentdetails[j].Agent_Id + '~' + Agentdetails[j].Terminal_Count + '~' + Agentdetails[j].Agency_name + '"\>' + Agentdetails[j].Agency_name + '</option>';
            }
        }
    }
    $("#allagentload").html(Agentload);
    $("#allagentload").chosen('destroy');
    $("#allagentload").chosen();

    if (Loginagent != null && Loginagent != "") {
        var Terminalload = "";
        Terminalload += "<option value=''>-- Select --</option>";
        for (var j = 1; j <= parseInt(Loginterminalcnt) ; j++) {
            if (j <= 9) { j = '0' + j; }
            if (Loginterminal == Loginagent + j) {
                Terminalload += "<option selected='selected' value=" + Loginagent + j + ">" + Loginagent + j + "</option>";
            }
            else {
                Terminalload += "<option value=" + Loginagent + j + ">" + Loginagent + j + "</option>";
            }
        }
        $("#allterminalload").html(Terminalload);
        $("#allterminalload").chosen('destroy');
        $("#allterminalload").chosen();
    }
}

function closedetails() {
    $("#modal-agentlist").hide();
}

function Getpowerloginavail() {

    var agentId = $("#allagentload").val().split("~")[0];
    var terminal = $("#allterminalload").val();
    var Branch = $("#allbranchload").val();
    var Agencyname = $("#allagentload").val().split("~")[2];

    $("#hdn_availagent").val(agentId);
    $("#hdn_availterminal").val(terminal);
    $("#hdn_availagencyname").val(Agencyname);

    if (Branch == null || Branch == "") {
        alert("Please select market point");
        return false;
    }
    else if (agentId == null || agentId == "") {
        alert("Please select agency name");
        return false;
    }
    else if (terminal == null || terminal == "") {
        alert("Please select terminal");
        return false;
    }
    else {

        $("#modal-agentlist").hide();
        //$(".modal-open").css("overflow", "auto");
        dataproperty_array = [];//Added by srinath on 20171006 to improve peroformance
        extrahidem_dataproperty_array = [];//Added by srinath on 20171006 to improve peroformance
        groparr = [];
        groupmain_array = [];
        groupmain_ret = [];
        filterdarray = [];
        Main_totalfilterdarray = [];
        Main_totalfilterdarray_ret = [];
        var obj1 = {
            count: 0,
            setcount: 0
        };

        $("#liern-hide")[0].checked = true;
        $('#spnEarnsort').hide('slow');
        $(".airfilterallcheck").html("");
        $("#airline_matrixhtml").html("");
        $(".faretypfilterallcheck").html("");
        allAIRLINECode = "";
        Faretypefilter = "";
        btn_Search("D");  //D- Direct Search, R-Recent Search
    }
}

var aryorgcity = [];
var arydescity = [];
var arydepdate = [];
var callrequestcnt = 0;
var requestcount = 0;
var availabibindcount = 0;

function btn_Search(flag) { //flag: D- Direct Search, R-Recent Search
    aryorgcity = [];
    arydescity = [];
    arydepdate = [];
    var origincity,
        destinationcity,
        departuredate,
        arrivaldate,
        adultcount,
        childcount,
        infantcout,
        triptype,
        segmenttype,
        cabin,
        flightname,
        fltnumkey,
        fltsegmentkey,
        flightoption = "";



    if (searchvalidation()) {
        $("#chkDirFlt")[0].checked = false;
        $("#chkNonstpFlt")[0].checked = false;
        $("#chkNFare")[0].checked = false;
        $("#liern-hide")[0].checked = true;
        origincity = $("#txtorigincity").val().trim();
        if (origincity.split("(").length > 1) {
            $("#hdtxt_origin").val(origincity.split("(")[1].split(")")[0].trim());
            $("#hd_org_fu").val(origincity.split("-")[0].trim());
        }

        else {
            msg = "Please select valid origin city.";
            showError1(msg);
            return false;
        }
        aryorgcity.push($("#hdtxt_origin").val());
        destinationcity = $("#txtdestinationcity").val().trim();
        if (destinationcity.split("(").length > 1) {
            $("#hdtxt_destination").val(destinationcity.split("(")[1].split(")")[0].trim());
            $("#hd_des_fu").val(destinationcity.split("-")[0].trim());
        }

        else {
            msg = "Please select valid Destination city.";
            showError1(msg);
            return false;
        }

        if (TerminalType == "T") {
            if ($("#ddlclient").length > 0) {
                if (sessionStorage.getItem("clientid") == "" || sessionStorage.getItem("clientid") == 'undefined' || sessionStorage.getItem("clientid") == null || $('#ddlclient').val() == "" || $('#ddlclient').val() == "0") {
                    msg = "Please Select Agency name";
                    showError1(msg);
                    return false;
                }
            }
            if ($('#ddlTerminalId').length > 0) {
                if ($('#ddlTerminalId').val() == "") {
                    showError1("Please Select Terminal ID");
                    return false;
                }
                sessionStorage.setItem("clientid", $('#ddlTerminalId').val());
            }
        }

        if ($("#hdn_producttype").val() == "RIYA" && (($("#hdthread").val() == "") || ($("#hdthread_dt").val() == ""))) {
            CallWidgetpopup("Information", "modal-dvThread", "400");
            return false;
        }


        arydescity.push($("#hdtxt_destination").val());
        departuredate = $("#txtdeparture").val().trim();
        $("#hdtxt_depa_date").val(departuredate);
        arydepdate.push($("#hdtxt_depa_date").val());
        arrivaldate = $("#txtarrivaldate").val().trim();
        $("#hdtxt_Arrivedate").val(arrivaldate);
        adultcount = $("#ddladult").val();
        $("#hdtxt_adultcount").val(adultcount);
        childcount = $("#ddlchild").val() != null && $("#ddlchild").val() != "" ? $("#ddlchild").val().trim() : "0";
        $("#hdtxt_childcount").val(childcount);
        infantcout = $("#ddlinfant").val() != null && $("#ddlinfant").val() != "" ? $("#ddlinfant").val().trim() : "0";
        $("#hdtxt_infantcount").val(infantcout);
        cabin = "";
        flightname = "";
        flightoption = "";

        if ($('#hdtxt_trip').val() == "R") {
            fltnumkey = $("#Flightno1").val().toUpperCase().trim() + "," + $("#Flightno2").val().toUpperCase().trim()
            fltsegmentkey = $("#Via1").val().toUpperCase().trim() + "," + $("#Via2").val().toUpperCase().trim()
        } else {
            fltnumkey = $("#Flightno1").val().toUpperCase().trim() + "," + $("#Flightno2").val().toUpperCase().trim()
            fltsegmentkey = $("#Via1").val().toUpperCase().trim() + "," + $("#Via2").val().toUpperCase().trim()
        }

        if ($("#rd_corporate_retail").prop("checked") == true) { //Fetch Corporate details -STS185
            $("#hdtxt_corporatefare").val($('body').data("bhdcorporatefaredt"));
        }
        else {
            $("#hdtxt_corporatefare").val("");
        }

        $(".flight-listavail").html(""); //For all mode of avail (oneway, roundtrip)...
        $('.headericons').show();
        //Set Segment Type is international or Domestic...by saranraj on 20170522...
        var new_arr = $.grep(loadGlobalcityArrry, function (n, i) {
            return n.ID == $("#hdtxt_origin").val();
        });
        var new_arr_des = $.grep(loadGlobalcityArrry, function (n, i) {
            return n.ID == $("#hdtxt_destination").val();
        });

        $("#hdn_origincntry").val(new_arr[0].CN);
        $("#hdn_destincntry").val(new_arr_des[0].CN);

        segmenttype = new_arr[0].CN == assignedcountry && new_arr_des[0].CN == assignedcountry ? "D" : "I";
        $('body').data('segtype', segmenttype);
        //End...

        if ($("#hdn_AppHosting").val() != null && $("#hdn_AppHosting").val() == "BSA" &&
            (sessionStorage.getItem('refreshPageFlg') == null || sessionStorage.getItem('refreshPageFlg') == "1") &&
            $("#hdtxt_trip").val() != "M" && $("#hdn_sessAgentLogin").val().toUpperCase() != "Y" && TerminalType != "M") {
            var aryFltselectedval = [];

            aryFltselectedval.push(segmenttype);//0 airport type
            aryFltselectedval.push($("#hdtxt_trip").val());//1 trip type
            aryFltselectedval.push(origincity);//2 origin
            aryFltselectedval.push(destinationcity);//3 destiantion
            aryFltselectedval.push(departuredate);//4 depature
            aryFltselectedval.push(arrivaldate);//5 arrival
            aryFltselectedval.push(adultcount);//6 adults
            aryFltselectedval.push(childcount);//7 childs
            aryFltselectedval.push(infantcout);//8 infants
            if ($("#flightClassEconomic").is(":checked"))
                aryFltselectedval.push("E");
            else if ($("#flightClassBusiness").is(":checked"))
                aryFltselectedval.push("B");
            else if ($("#flightClassFirstClass").is(":checked"))
                aryFltselectedval.push("F");
            else if ($("#flightClassPremiumEconomic").is(":checked"))
                aryFltselectedval.push("P");
            aryFltselectedval.push($("#fli_option").val());//10 lcc / fsc filter
            var AirlineCodes = $("#FlightName").val();
            AirlineCodes = AirlineCodes != null ? AirlineCodes : [];
            aryFltselectedval.push(AirlineCodes.join(','));//11 airline filter
            if ($("#rd_Special_fare").is(":checked") == true) {
                aryFltselectedval.push(true);
            }
            else {
                aryFltselectedval.push(false);
            }
            var nowdate = new Date();
            var datetimesec = "y" + nowdate.getFullYear() + "M" + nowdate.getMonth() + "d" + nowdate.getDate() + "H" + nowdate.getHours() + "m" + nowdate.getMinutes() + "s" + nowdate.getSeconds() + "ms" + nowdate.getMilliseconds();
            sessionStorage.setItem(datetimesec, JSON.stringify(aryFltselectedval));
            sessionStorage.setItem('refreshPageFlg', '0');
            window.location.href = $("#hdn_b2cavailurl").val() + "?" + datetimesec;
            $.blockUI({
                message: '<img alt="Please Wait..." src="' + loaderurl + '" style="background-color:#fff; border-radius:8px; margin-top:12%;width:150px;" id="FareRuleLodingImage" />',
                css: { backgroundColor: '#fff', padding: '15px', 'border-radius': '5%', width: "10%" }, fadeIn: 0
            });
        }
        else {
            sessionStorage.setItem('refreshPageFlg', '1');
            PageSearch();
        }
        //qrystrsessionkey
    }
}

var Corp_aircode = "";
function PageSearch(S) {

    var ReqSaberairlines = ($('body').data('segtype') == "D" ? ($("#hdn1sdom").val() == "" ? "1S" : "") + ($("#hdn1gdom").val() == "" ? "1G" : "") : ($("#hdn1sint").val() == "" ? "1S" : "") + ($("#hdn1gint").val() == "" ? "1G" : ""));
    //To Check Internet connection is available... by saranraj on 20170725...
    var InetFlg = checkconnection();
    if (!InetFlg) {
        showerralert("Please check connectivity.", "", "");
        return false;
    }
    //End...
    Ttotlen = 0;// by udhaya  Avail count clear each and every search time 
    //$(".chkfilterplane")[0].checked = false;
    $('.clsroundAvail').html('');
    $('.zeroflt').html('0');
    $("#dvAvailView").hide();
    $('#spnTotalFAvailCntParent').hide();
    $('#spnTotalFRoundAvailCntParent').hide();
    $('#commonSorting').hide();
    $('#btnFmodifySearch').attr('disabled', true);//STS-166
    $('.clsAvailFiltericon').css('pointer-events', 'none');//STS-166
    document.getElementById("chkNFare").checked = false;
    document.getElementById("chkCheckall").checked = false;
    //Roundtrip Clear all...
    $(".seletbtmavailclr").html("");
    $('#dvStickyFltSlectRAvail').hide();
    $('#dvStickyFltSlectMultiAvail').hide();
    $('#selectclickbuttonRTrip').data('dep-id', '');
    $('#selectclickbuttonRTrip').data('ret-id', '');
    $('#spnGrandTotFare').data('fare-o', '');
    $('#spnGrandTotFare').data('fare-r', '');
    $('.clearonsearch').remove();
    $('#FltfilterTab,#sepfilterTab').html('');
    //End...
    //Hide All Avail div and Open approprite div...
    $('.availSetParent').hide();
    if ($("#hdtxt_trip").val() == "M" && $('body').data('segtype') == "D") { //For Domestic Multi City......
        $('#dvmultiCtyParent').show();
    }
    else if ($("#hdtxt_trip").val() == "R") { //For Roundtrip...
        $('#dvroundTrpParent').show();
    }
    else { //For Oneway and Roundtrip Special...
        $('#dvonewayParent').show();
    }
    //End...
    AvailRequestCount = 0;
    AvailResponseCount = 0;
    load_Increase = 0;
    totflt = 0;
    callrequestcnt = 0;
    availabibindcount = 0;
    if (bar1 != "")
        bar1.destroy();
    multiclasarr = [];
    multiclasconarr = [];
    $('#dvmoreAvailload').show();

    /* Added by Rajesh for Avoid unwanted thread request*/
    var Base_OriginCity = $("#hdtxt_origin")[0].value;
    var Base_DestinationCity = $("#hdtxt_destination")[0].value;

    var Base_origincountry = $("#hdn_origincntry")[0].value;
    var Base_desnationcountry = $("#hdn_destincntry")[0].value;
    /* Added by Rajesh for Avoid unwanted thread request*/

    var dtmm = new Date();
    UIDCORP = LoginUserUniqSessKey == "" ? dtmm.getHours().toString() + dtmm.getMinutes().toString() + dtmm.getSeconds().toString() : LoginUserUniqSessKey + dtmm.getYear().toString() + dtmm.getMonth().toString() + dtmm.getDay().toString() + dtmm.getHours().toString() + dtmm.getMinutes().toString() + dtmm.getSeconds().toString(); // by vijai LoginUserUniqSessKey cannot be empty( for safety purpose using conditional operator) 03072018

    //To Desabled Previous buttom when Current date acchieves Minimum Date...
    if (FLAG != "M") {//STS-166
        if ($("#txtdeparture").val() == vb_currentdate) {
            document.getElementById("btnPrev").disabled = true; $('#btnPrev').css('cursor', 'not-allowed');
        }
        else {
            document.getElementById("btnPrev").disabled = false; $('#btnPrev').css('cursor', 'pointer');
        }
        if ($("#txtarrivaldate").val() == vb_maxdate) {
            document.getElementById("btnNext").disabled = true; $('#btnNext').css('cursor', 'not-allowed');
        }
        else {
            document.getElementById("btnNext").disabled = false; $('#btnNext').css('cursor', 'pointer');
        }
        AirlineCode = $('#hdtxt_trip').val() == "M" ? $("#MulFlightName").val() : $("#FlightName").val();
        AirlineCode = AirlineCode != null ? AirlineCode : [];
        AirlineCode = AirlineCode.join(',');
    }
    else {
        if (departuredate == vb_currentdate) {
            document.getElementById("btnPrev").disabled = true; $('#btnPrev').css('cursor', 'not-allowed');
        }
        else {
            document.getElementById("btnPrev").disabled = false; $('#btnPrev').css('cursor', 'pointer');
        }
        if (arrivaldate == vb_maxdate) {
            document.getElementById("btnNext").disabled = true; $('#btnNext').css('cursor', 'not-allowed');
        }
        else {
            document.getElementById("btnNext").disabled = false; $('#btnNext').css('cursor', 'pointer');
        }

    }
    //End...
    var S = "";
    /***/

    if ($("#hdn_AppHosting").val() == "BSA" || $("#hdn_AppHosting").val() == "BOA" || $("#hdn_AppHosting").val() == "B2B") {
        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loaderurl + '" style="background-color:#fff; border-radius:8px; margin-top:12%;width:150px;" id="FareRuleLodingImage" />',
            css: { backgroundColor: '#fff', padding: '15px', 'border-radius': '5%', width: "10%" }, fadeIn: 0
        });
    }

    else if ($("#hdn_producttype").val() == "RIYA" || $("#hdn_producttype").val() == "RUAE") {
        $.blockUI({
            message: '<center><div class="loaddiv"><p style="display:block; color:#333; font-size:12px;"> Searching availability </p><p>Please Wait...</p><div class="loader"></div></div></center>',
            css: { backgroundColor: '#fff', padding: '15px', 'border-radius': '5%' }, fadeIn: 0
        });
    }
    else if ($("#hdn_producttype").val() == "JOD") {
        $.blockUI({
            message: '<center><div class="loaddiv"><p style="display:block; color:#333; font-size:12px;"> Searching availability </p><p>Please Wait...</p><div class="lds-dual-ring"></div></div></center>',
            css: { backgroundColor: '#fff', padding: '15px', 'border-radius': '5%' }, fadeIn: 0
        });
    }
    else {
        $.blockUI({
            message: '<center><div class="loaddiv"><p style="display:block; color:#333; font-size:12px;"> Searching availability </p><p>Please Wait...</p> <div class="sampleContainer"><div class="loader"><span class="dot dot_1"></span><span class="dot dot_2"></span><span class="dot dot_3"></span><span class="dot dot_4"></span></div></div> </div></center>',
            css: { backgroundColor: '#fff', padding: '15px', 'border-radius': '5%' }, fadeIn: 0
        });
    }

    flight_cat = [];
    platingcarrier = new Array();
    // var flyoption = $('#hdtxt_trip').val() == "M" ? $('#Mulfli_option').val() : $('#fli_option').val();

    var flyoption = "";//STS-166
    if (FLAG == "M") {
        flyoption = flightoption;
    }
    else {
        flyoption = $('#hdtxt_trip').val() == "M" ? $('#Mulfli_option').val() : $('#fli_option').val();
    }

    if ($('#hdReturnses').val() != null && $('#hdReturnses').val() != "") { //Not Working Now ... ll check... by saranraj...
        var adtcount = localStorage.getItem("Adultcount");
        var chdcount = localStorage.getItem("childcount");
        var infcount = localStorage.getItem("infantcount");
        $('#ddladult').val(adtcount);
        $('#ddlchild').val(chdcount);
        $('#ddlinfant').val(infcount);
    }/**If Part for Page Loads**/
    else {/**Else Part for Page Loads**/
        // var CategoryChecker = $("#hdthread").val().split('|'); //Common hidden field for oneway, roundtrip and multicity...

        if (($("#Via1").val() != null && $("#Via1").val()) || $("#Via2").val() != null && $("#Via2").val() != "") {
            var CategoryChecker = $("#hdn_viathread").val().split('|');
        }
        else {
            var CategoryChecker = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#hdthread_dt").val().split('|') : ($("#hdn_producttype").val() == "UAE" && $("#rd_labour").prop("checked") == true) ? $("#hdn_labourthrd").val().split('|') : $("#hdthread").val().split('|');
        }

        if ($("#rd_corporate_retail").prop("checked") == true && $('body').data("bhdcorporatefaredt") != "") { //sts185
            AirlineCode = "";
            Corp_aircode = "";
            var aircode = $('body').data("bhdcorporatefaredt").split(',');
            for (var i = 0; i < aircode.length ; i++) {
                if (aircode[i].split('~')[0] != "") {
                    AirlineCode += aircode[i].split('~')[0] + ',';
                    Corp_aircode += aircode[i].split('~')[0] + ',';
                }
            }
        }
        //To Check valid airline in Airline filteration... Added by saranraj on 20170708...
        var fn = AirlineCode;
        var aryChkvalid = fn.split(',');
        var chkvalidFlg = false;
        for (var vld = 0; vld < aryChkvalid.length; vld++) {
            if (aryChkvalid[vld].length > 2) {
                chkvalidFlg = true;
                break;
            }
        }
        if (chkvalidFlg) {
            $.unblockUI();
            showerralert("Please select valid Airline code.", "", "");
            return false;
        }
        //End...
        if ($("#Mulrd_Booklet").is(":checked") == true) {
            if ($("#hdn_BookletThread").val() != null && $("#hdn_BookletThread").val() != undefined && $("#hdn_BookletThread").val() != "") {
                $('.availSetParent').hide();
                $('#dvonewayParent').show();
                var fltnumviaopts = finalfltvia;
                var BookletThread = $("#hdn_BookletThread").val().split('|');
                $('body').data('segtype', "I");
                for (var _booklet = 0; _booklet < BookletThread.length; _booklet++) {
                    var strBookletthread = BookletThread[_booklet].split('~');
                    AsyncCall(strBookletthread[1], strBookletthread[0], aryOrgMul.join(','), aryDstMul.join(','), aryDptMul.join(','), "", fltnumviaopts);
                    console.log('hit count' + _booklet + '-' + strBookletthread[0]);
                    AvailRequestCount++;
                }
            }
        }
        else if ($("#hdn_AppHosting").val() == "BSA" && ($("#rd_Special_fare").is(":checked") == true || $("#Mulrd_Special_fare").is(":checked") == true)) {
            var fltnumviaopts = finalfltvia;
            var loopcnt = ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "D") ? aryOrgMul.length : 1;
            if (loopcnt > 1) {
                AsyncCall(AirlineCode, "OSC", aryOrgMul.join(','), aryDstMul.join(','), aryDptMul.join(','), "", fltnumviaopts); //loop
                console.log('hit count0-OSC');
                AvailRequestCount++;
            }
            else {
                AsyncCall(AirlineCode, "OSC");
                console.log('hit count0-OSC');
                AvailRequestCount++;
            }
        }
        else if (fn.length <= 3 && $("#hdn_producttype").val() == "RIYA" && $("#rd_corporate_retail").prop("checked") == true && $('body').data("bhdcorporatefaredt") != "") {

            var loopcnt = ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "D") ? aryOrgMul.length : 1;
            for (var loop = 0; loop < loopcnt; loop++) {

                var CategoryChecker = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#hdthread_dt").val().split('|') : ($("#hdn_producttype").val() == "UAE" && $("#rd_labour").prop("checked") == true) ? $("#hdn_labourthrd").val().split('|') : $("#hdthread").val().split('|');
                if (fn.split(',')[0] != "") {
                    if (CategoryChecker.indexOf(fn.split(',')[0]) > -1) {
                        $("body").data("tripcheck", "");
                        AsyncCall(fn.split(',')[0], fn.split(',')[0], "", "", "", loop);
                        console.log('hit count' + i + "-" + fn.split(',')[0]);
                        AvailRequestCount++;
                        if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                            $("body").data("tripcheck", "R");
                            AsyncCall(fn.split(',')[0], fn.split(',')[0]);
                            AvailRequestCount++;
                            console.log('hit count' + AvailRequestCount + "-" + fn.split(',')[0] + "#00");
                        }
                    }
                    else if (GetInLcccheck(fn.split(',')[0]) == false) {
                        // var Corpairrequestthread = ($('body').data('segtype') == "D" ? (($("#hdn1sdom").val().indexOf(fn.split(',')[0]) > -1) ? "1S" : ($("#hdn1gdom").val().indexOf(fn.split(',')[0]) > -1) ? "1G" : "1A") : (($("#hdn1sint").val().indexOf(fn.split(',')[0]) > -1) ? "1S" : ($("#hdn1gint").val().indexOf(fn.split(',')[0]) > -1) ? "1G" : "1A"));
                        var Corpairrequestthread = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#fscthreadkey_dt").val() : $("#fscthreadkey").val();
                        for (var k = 0; k < Corpairrequestthread.split("|").length; k++) {
                            $("body").data("tripcheck", "");
                            AsyncCall(fn.split(',')[0], Corpairrequestthread.split("|")[k], "", "", "", loop);
                            console.log('hit count' + i + "-" + Corpairrequestthread.split("|")[k]);
                            AvailRequestCount++;
                            if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                                $("body").data("tripcheck", "R");
                                AsyncCall(fn.split(',')[0], Corpairrequestthread.split("|")[k]);
                                AvailRequestCount++;
                                console.log('hit count' + AvailRequestCount + "-" + Corpairrequestthread.split("|")[k] + "#00");
                            }
                        }
                    }
                    //else if ($("#hdn_saberairline").val().indexOf(fn.split(',')[0]) > -1) {
                    //    $("body").data("tripcheck", "");
                    //    AsyncCall(fn.split(',')[0], "1S", "", "", "", loop);
                    //    console.log('hit count' + i + "-" + "1S");
                    //    AvailRequestCount++;
                    //    if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                    //        $("body").data("tripcheck", "R");
                    //        AsyncCall(fn.split(',')[0], "1S");
                    //        AvailRequestCount++;
                    //        console.log('hit count' + AvailRequestCount + "-" + "1S" + "#00");
                    //    }
                    //}
                }
            }
        }
        else if (fn.length <= 3) {

            fn = AirlineCode.split(',')[0];
            var loopcnt = ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "D") ? aryOrgMul.length : 1;

            if ($("#hdn_checkflag").val() == "2" && $("body").data("tripflag") != "O" && $("body").data("tripflag") != "M" && ($("#hdn_allowtrip").val() != null && $("#hdn_allowtrip").val() != "" && ($("#hdn_allowtrip").val().indexOf("R") > -1 && $('#hdtxt_trip').val() == "R") || $("#hdn_checkflag").val() == "2")) {
                var doublethread = $("#hdn_doublethread").val().split('|');
                for (var i = 0; i < doublethread.length; i++) {
                    $("body").data("tripcheck", "");
                    AsyncCall(AirlineCode, doublethread[i]);
                    console.log('hit count' + i + "-" + doublethread[i]);
                    AvailRequestCount++;
                    if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                        $("body").data("tripcheck", "R");
                        AsyncCall(AirlineCode, doublethread[i]);
                        AvailRequestCount++;
                        console.log('hit count' + i + "-" + doublethread[i]);
                    }
                }
            }
            else
                if (!($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "I")) { //Except International Multicity...
                    for (var loop = 0; loop < loopcnt; loop++) {  //For Domestic Multicity looping upto segment count .... For Other's loop only one time...
                        if (fn != "") {//&& fn.length <= 3
                            var fns = jQuery.inArray(fn, CategoryChecker);
                            if (fns != -1)
                            { fns = true; }
                            else
                            { fns = false; }
                            switch (fns) {
                                case (true): // Category Flights
                                    //var newthkey = $("#newthreadkey").val().split("~");
                                    var newthkey = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#newthreadkey_dt").val().split("~") : $("#newthreadkey").val().split("~");
                                    if (newthkey != null && newthkey != "") {
                                        if (multithread(newthkey, fn)) {
                                            for (var i = 0; i < flight_cat.length - 1; i++) {
                                                if ($('#hdtxt_trip').val() == "M") {
                                                    if (RequestFiltration(flight_cat[i]["category"], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                        if (ReqSaberairlines.toLocaleUpperCase().trim() == flight_cat[i]["category"].toLocaleUpperCase().trim()) {
                                                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                        }
                                                        else {
                                                            AsyncCall(flight_cat[i]["fn"], flight_cat[i]["category"], aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                                        }
                                                    }
                                                    else {
                                                        console.log("Request Void Filtered for (" + Base_origincountry + " - " + Base_desnationcountry + ") " + flight_cat[i]["category"]);
                                                    }
                                                }
                                                else {
                                                    if (RequestFiltration(flight_cat[i]["category"], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                        if (ReqSaberairlines.toLocaleUpperCase().trim() == flight_cat[i]["category"].toLocaleUpperCase().trim()) {
                                                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                        }
                                                        else {
                                                            AsyncCall(flight_cat[i]["fn"], flight_cat[i]["category"]);
                                                            AvailRequestCount++;
                                                        }
                                                    }
                                                    else {
                                                        console.log("Request Void Filtered for (" + Base_origincountry + " - " + Base_desnationcountry + ") " + flight_cat[i]["category"]);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if ($('#hdtxt_trip').val() == "M") {
                                        if (RequestFiltration(fn, Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                            AsyncCall('', fn, aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                            AvailRequestCount++;
                                            console.log('hit count' + AvailRequestCount + "-" + fn + "#001");
                                        }
                                        else {
                                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + fn);
                                        }
                                    }
                                    else {
                                        $("body").data("tripcheck", "");
                                        if (RequestFiltration(fn, Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                            AsyncCall('', fn);
                                            AvailRequestCount++;
                                        }
                                        else {
                                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + fn);
                                        }
                                        if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                                            $("body").data("tripcheck", "R");
                                            if (RequestFiltration(fn, Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                AsyncCall('', fn);
                                                AvailRequestCount++;
                                                console.log('hit count' + AvailRequestCount + "-" + fn + "#00");
                                            }
                                            else {
                                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + fn);
                                            }
                                        }
                                    }
                                    break;
                                case (false): // UnCategory Flights    

                                    // *** For WY ailine separate req added by rajesh 03/04/2019*** //
                                    //LKO|HYD|MAA|COK
                                    //if ($("#hdn_Airthread").val() != "" && $("#hdn_producttype").val() == "RIYA" && (fn == "WY" || fn == "WY,") && $('body').data('segtype') == "I" && (Base_OriginCity == "LKO" || Base_OriginCity == "HYD" || Base_OriginCity == "MAA" || Base_OriginCity == "COK")) {
                                    //    AsyncCall("WY", "1AWY");
                                    //    console.log('hit count' + i + "-" + "1AWY");
                                    //    AvailRequestCount++;
                                    //}
                                    // *** For WY ailine separate req added by rajesh 03/04/2019 *** //

                                    var bool = GetInLcccheck(fn);
                                    for (var i = 0; i < CategoryChecker.length; i++) {
                                        if (bool == true) {
                                            var LCC = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#lccthreadkey_dt").val() : $("#lccthreadkey").val();
                                            if (CategoryChecker[i] == fn || (LCC.indexOf(CategoryChecker[i]) > -1 && CategoryChecker[i].length > 2)) {
                                                if ($('#hdtxt_trip').val() == "M") {
                                                    if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                        AsyncCall(AirlineCode, CategoryChecker[i], aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                                        AvailRequestCount++;
                                                        console.log('hit count' + AvailRequestCount + "-" + CategoryChecker[i]);
                                                    }
                                                    else {
                                                        console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                    }
                                                }
                                                else {
                                                    $("body").data("tripcheck", "");
                                                    if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                        if (ReqSaberairlines.toLocaleUpperCase().trim() == CategoryChecker[i].toLocaleUpperCase().trim()) {
                                                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                        }
                                                        else {
                                                            AsyncCall(AirlineCode, CategoryChecker[i]);
                                                            console.log('hit count' + i + "-" + CategoryChecker[i]);
                                                            AvailRequestCount++;
                                                        }
                                                    }
                                                    else {
                                                        console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                    }

                                                    if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                                                        $("body").data("tripcheck", "R");
                                                        if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                            if (ReqSaberairlines.toLocaleUpperCase().trim() == CategoryChecker[i].toLocaleUpperCase().trim()) {
                                                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                            }
                                                            else {
                                                                AsyncCall(AirlineCode, CategoryChecker[i]);
                                                                AvailRequestCount++;
                                                                console.log('hit count' + AvailRequestCount + "-" + CategoryChecker[i]);
                                                            }
                                                        }
                                                        else {
                                                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else {
                                            var FSC = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#fscthreadkey_dt").val() : $("#fscthreadkey").val();
                                            var LCC = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#lccthreadkey_dt").val() : $("#lccthreadkey").val();
                                            var bool1 = GetInLcccheck(CategoryChecker[i])
                                            if (bool1 == false && FSC.indexOf(CategoryChecker[i]) > -1 && LCC.indexOf(CategoryChecker[i]) < 0) {
                                                // if ($("#hdn_producttype").val() == "RIYA" && ((AirlineCode != "" && AirlineCode.indexOf("AI") < 0 && CategoryChecker[i] == "1S") || (AirlineCode.indexOf("AI") > -1 && CategoryChecker[i] == "1A"))) {
                                                if ($("#hdn_producttype").val() == "RIYA" && (AirlineCode != "" && Checksaberairline(AirlineCode, CategoryChecker[i]) == false)) {
                                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + AirlineCode + "~" + CategoryChecker[i]);
                                                }
                                                else {
                                                    if ($('#hdtxt_trip').val() == "M") {
                                                        if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                            if (ReqSaberairlines.toLocaleUpperCase().trim() == CategoryChecker[i].toLocaleUpperCase().trim()) {
                                                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                            }
                                                            else {
                                                                AsyncCall(AirlineCode, CategoryChecker[i], aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                                                AvailRequestCount++;
                                                                console.log('hit count' + AvailRequestCount + "-" + CategoryChecker[i]);
                                                            }
                                                        }
                                                        else {
                                                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                        }
                                                    }
                                                    else {
                                                        $("body").data("tripcheck", "");
                                                        if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                            if (ReqSaberairlines.toLocaleUpperCase().trim() == CategoryChecker[i].toLocaleUpperCase().trim()) {
                                                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                            }
                                                            else {
                                                                AsyncCall(AirlineCode, CategoryChecker[i]);
                                                                console.log('hit count' + i + "-" + CategoryChecker[i]);
                                                                AvailRequestCount++;
                                                            }
                                                        }
                                                        else {
                                                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                        }
                                                        if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                                                            $("body").data("tripcheck", "R");
                                                            if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                                if (ReqSaberairlines.toLocaleUpperCase().trim() == CategoryChecker[i].toLocaleUpperCase().trim()) {
                                                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                                }
                                                                else {
                                                                    AsyncCall(AirlineCode, CategoryChecker[i]);
                                                                    AvailRequestCount++;
                                                                    console.log('hit count' + AvailRequestCount + "-" + CategoryChecker[i]);
                                                                }
                                                            }
                                                            else {
                                                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                    break;
                            }
                        }
                        else {

                            $('input.clickfare[type=checkbox]').each(function () {
                                if (this.checked) {
                                    CategoryChecker = [];
                                    if (SelectedsplThread != "" && SelectedsplThread != null) {
                                        if (SelectedsplThread.indexOf('|') > -1) {
                                            CategoryChecker = SelectedsplThread.split('|');
                                        }
                                        else {
                                            CategoryChecker[0] = SelectedsplThread;
                                        }
                                    }
                                }
                            });


                            $("body").data("tripcheck", "");
                            if (flyoption == "FSC") {
                                //var FSC = $("#fscthreadkey").val();
                                //var LCC = $("#lccthreadkey").val();

                                // *** For WY ailine separate req added by rajesh 03/04/2019*** //
                                //LKO|HYD|MAA|COK
                                //if ($("#hdn_Airthread").val() != "" && $("#hdn_producttype").val() == "RIYA" && $('body').data('segtype') == "I" && (Base_OriginCity == "LKO" || Base_OriginCity == "HYD" || Base_OriginCity == "MAA" || Base_OriginCity == "COK")) {
                                //    AsyncCall("WY", "1AWY");
                                //    console.log('hit count' + i + "-" + "1AWY");
                                //    AvailRequestCount++;
                                //}
                                // *** For WY ailine separate req added by rajesh 03/04/2019 *** //

                                var FSC = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#fscthreadkey_dt").val() : $("#fscthreadkey").val();
                                var LCC = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#lccthreadkey_dt").val() : $("#lccthreadkey").val();
                                for (var i = 0; i < CategoryChecker.length; i++) {
                                    var bool = GetInLcccheck(CategoryChecker[i])
                                    if (bool == false && FSC.indexOf(CategoryChecker[i]) > -1 && LCC.indexOf(CategoryChecker[i]) < 0) {
                                        if ($('#hdtxt_trip').val() == "M") {
                                            if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                if (ReqSaberairlines.toLocaleUpperCase().trim() == CategoryChecker[i].toLocaleUpperCase().trim()) {
                                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                }
                                                else {
                                                    AsyncCall(AirlineCode, CategoryChecker[i], aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                                    AvailRequestCount++;
                                                    console.log('hit count' + AvailRequestCount + "-" + CategoryChecker[i]);
                                                }
                                            }
                                            else {
                                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                            }
                                        }
                                        else {
                                            if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                if (ReqSaberairlines.toLocaleUpperCase().trim() == CategoryChecker[i].toLocaleUpperCase().trim()) {
                                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                }
                                                else {
                                                    $("body").data("tripcheck", "");
                                                    AsyncCall(AirlineCode, CategoryChecker[i]);
                                                    console.log('hit count' + i + "-" + CategoryChecker[i]);
                                                    AvailRequestCount++;
                                                }
                                            }
                                            else {
                                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                            }
                                            if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                                                if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                    if (ReqSaberairlines.toLocaleUpperCase().trim() == CategoryChecker[i].toLocaleUpperCase().trim()) {
                                                        console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                    }
                                                    else {
                                                        $("body").data("tripcheck", "R");
                                                        AsyncCall(AirlineCode, CategoryChecker[i]);
                                                        AvailRequestCount++;
                                                        console.log('hit count' + AvailRequestCount + "-" + CategoryChecker[i]);
                                                    }
                                                }
                                                else {
                                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (flyoption == "LCC") {
                                //var LCC = $("#lccthreadkey").val();
                                var LCC = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#lccthreadkey_dt").val() : $("#lccthreadkey").val();
                                for (var i = 0; i < CategoryChecker.length; i++) {
                                    var bool = GetInLcccheck(CategoryChecker[i])
                                    if (bool == true || LCC.indexOf(CategoryChecker[i]) > -1) {
                                        if ($('#hdtxt_trip').val() == "M") {
                                            if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                AsyncCall(AirlineCode, CategoryChecker[i], aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                                AvailRequestCount++;
                                                console.log('hit count' + AvailRequestCount + "-" + CategoryChecker[i]);
                                            }
                                            else {
                                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                            }
                                        }
                                        else {
                                            if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                $("body").data("tripcheck", "");
                                                AsyncCall(AirlineCode, CategoryChecker[i]);
                                                console.log('hit count' + i + "-" + CategoryChecker[i]);
                                                AvailRequestCount++;
                                            }
                                            else {
                                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                            }
                                            if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                                                if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                    $("body").data("tripcheck", "R");
                                                    AsyncCall(AirlineCode, CategoryChecker[i]);
                                                    AvailRequestCount++;
                                                    console.log('hit count' + AvailRequestCount + "-" + CategoryChecker[i]);
                                                }
                                                else {
                                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else {
                                for (var i = 0; i < CategoryChecker.length; i++) {

                                    // *** For WY ailine separate req added by rajesh 03/04/2019*** //
                                    //LKO|HYD|MAA|COK
                                    if (CategoryChecker[i] == "1AWY" && (Base_OriginCity != "LKO" && Base_OriginCity != "HYD" && Base_OriginCity != "MAA" && Base_OriginCity != "COK")) {
                                        // Don't send request for exclure sectors
                                    }
                                        // *** For WY ailine separate req added by rajesh 03/04/2019 *** //
                                    else {
                                        if ($('#hdtxt_trip').val() == "M") {
                                            if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                AsyncCall(AirlineCode, CategoryChecker[i], aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                                AvailRequestCount++;
                                                console.log('hit count' + AvailRequestCount + "-" + CategoryChecker[i]);
                                            }
                                            else {
                                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                            }
                                        }
                                        else {
                                            if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                if (ReqSaberairlines.toLocaleUpperCase().trim() == CategoryChecker[i].toLocaleUpperCase().trim()) {
                                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                }
                                                else {
                                                    $("body").data("tripcheck", "");
                                                    AsyncCall(AirlineCode, CategoryChecker[i]);
                                                    console.log('hit count' + i + "-" + CategoryChecker[i]);
                                                    AvailRequestCount++;
                                                }
                                            }
                                            else {
                                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                            }
                                        }
                                    }
                                }
                            }
                            if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                                $("body").data("tripcheck", "R");
                                if (flyoption == "FSC") {
                                    //var FSC = $("#fscthreadkey").val();
                                    var FSC = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#fscthreadkey_dt").val() : $("#fscthreadkey").val();
                                    //var LCC = $("#lccthreadkey").val();
                                    var LCC = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#lccthreadkey_dt").val() : $("#lccthreadkey").val();
                                    for (var i = 0; i < CategoryChecker.length; i++) {
                                        var bool = GetInLcccheck(CategoryChecker[i])
                                        if (bool == false && FSC.indexOf(CategoryChecker[i]) > -1 && LCC.indexOf(CategoryChecker[i]) < 0) {
                                            if ($('#hdtxt_trip').val() == "M") {
                                                if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                    AsyncCall(AirlineCode, CategoryChecker[i], aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                                }
                                                else {
                                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                }
                                            }
                                            else
                                                if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                    AsyncCall(AirlineCode, CategoryChecker[i]);
                                                    console.log('hit count' + i + "-" + CategoryChecker[i]);
                                                    AvailRequestCount++;
                                                }
                                                else {
                                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                }
                                        }
                                    }
                                }
                                else if (flyoption == "LCC") {
                                    //var LCC = $("#lccthreadkey").val();
                                    var LCC = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#lccthreadkey_dt").val() : $("#lccthreadkey").val();
                                    for (var i = 0; i < CategoryChecker.length; i++) {
                                        var bool = GetInLcccheck(CategoryChecker[i])
                                        if (bool == true || LCC.indexOf(CategoryChecker[i]) > -1) {
                                            if ($('#hdtxt_trip').val() == "M") {
                                                if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                    AsyncCall(AirlineCode, CategoryChecker[i], aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                                }
                                                else {
                                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                }
                                            }
                                            else {
                                                if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                    AsyncCall(AirlineCode, CategoryChecker[i]);
                                                    console.log('hit count' + i + "-" + CategoryChecker[i]);
                                                    AvailRequestCount++;
                                                }
                                                else {
                                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                }
                                            }
                                        }
                                    }
                                }
                                else {

                                    for (var i = 0; i < CategoryChecker.length; i++) {

                                        if ($("#hdn_producttype").val() == "RIYA" && (AirlineCode != "" && Checksaberairline(AirlineCode, CategoryChecker[i]) == false)) {//   $("#hdn_producttype").val() == "RIYA"
                                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + AirlineCode + "~" + CategoryChecker[i]);
                                        }
                                        else {
                                            // *** For WY ailine separate req added by rajesh 03/04/2019*** //
                                            //LKO|HYD|MAA|COK
                                            if (CategoryChecker[i] == "1AWY" && (Base_OriginCity != "LKO" && Base_OriginCity != "HYD" && Base_OriginCity != "MAA" && Base_OriginCity != "COK")) {
                                                // Don't send request for exclure sectors
                                            }
                                            // *** For WY ailine separate req added by rajesh 03/04/2019 *** //
                                            if ($('#hdtxt_trip').val() == "M") {
                                                if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                    AsyncCall(AirlineCode, CategoryChecker[i], aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                                }
                                                else {
                                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                }
                                            }
                                            else {
                                                if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                    AsyncCall(AirlineCode, CategoryChecker[i]);  //modified by sri
                                                    console.log('hit count' + i + "-" + CategoryChecker[i]);
                                                    AvailRequestCount++;
                                                }
                                                else {
                                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else {
                    var fltnumviaopts = finalfltvia;
                    CategoryChecker = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#hdnMultiThread_dt").val().split('|') : $("#hdnMultiThread").val().split('|');//$("#hdnMultiThread").val().split('|');
                    for (var i = 0; i < CategoryChecker.length; i++) {
                        if (ReqSaberairlines.toLocaleUpperCase().trim() == CategoryChecker[i].toLocaleUpperCase().trim()) {
                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                        } else {
                            AsyncCall(AirlineCode, CategoryChecker[i], aryOrgMul.join(','), aryDstMul.join(','), aryDptMul.join(','), loop, fltnumviaopts); //loop
                            console.log('hit count' + i + "-" + CategoryChecker[i]);
                            AvailRequestCount++;
                        }
                    }
                }
        }
        else {
            getfilter();
        }
    }
    if (AvailRequestCount == 0) {
        $.unblockUI();
        $("#dvAvail").css("display", "none");
        $("#InterNational_Avail").css("display", "none");
        $("#dvSearch").css("display", "block");
        $(".divdisable").remove();
        $("#btn_Search").prop('disabled', false);
        showerralert("No Flights available.", "", "");
        return false;
    }
}

function getfilter() {
    //var res = $('#hdtxt_trip').val() == "M" ? $("#MulFlightName").val() : $("#FlightName").val();
    //STS-166
    // var res = $('#hdtxt_trip').val() == "M" ? $("#MulFlightName").val() : $("#FlightName").val();

    /* Added by Rajesh for Avoid unwanted thread request*/
    var Base_OriginCity = $("#hdtxt_origin")[0].value;
    var Base_DestinationCity = $("#hdtxt_destination")[0].value;

    var Base_origincountry = $("#hdn_origincntry")[0].value;
    var Base_desnationcountry = $("#hdn_destincntry")[0].value;
    /* Added by Rajesh for Avoid unwanted thread request*/
    var res = "";
    if (FLAG == "M") {
        res = AirlineCode;
    }
    else { //sts185
        res = ($("#rd_corporate_retail").prop("checked") == true) ? AirlineCode : $('#hdtxt_trip').val() == "M" ? $("#MulFlightName").val() : $("#FlightName").val();
        res = res != null ? res : [];
        res = res.join(',');
    }

    // *** For WY ailine separate req added by rajesh 03/04/2019*** //
    //LKO|HYD|MAA|COK
    //if ($("#hdn_Airthread").val() != "" && $("#hdn_producttype").val() == "RIYA" && res.indexOf("WY") > -1 && $('body').data('segtype') == "I" && (Base_OriginCity == "LKO" || Base_OriginCity == "HYD" || Base_OriginCity == "MAA" || Base_OriginCity == "COK")) {
    //    AsyncCall("WY", "1AWY");
    //    console.log('hit count' + i + "-" + "1AWY");
    //    AvailRequestCount++;
    //}

    // *** For WY ailine separate req added by rajesh 03/04/2019 *** //

    if ($("#hdn_producttype").val() == "RIYA" && $("#rd_corporate_retail").prop("checked") == true && $('body').data("bhdcorporatefaredt") != "") {

        // 6E,SG,2T,G8,AK
        // 
        var loopcnt = ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "D") ? aryOrgMul.length : 1;
        for (var loop = 0; loop < loopcnt; loop++) {
            for (var i = 0; i < res.split(',').length; i++) {
                var CategoryChecker = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#hdthread_dt").val().split('|') : ($("#hdn_producttype").val() == "UAE" && $("#rd_labour").prop("checked") == true) ? $("#hdn_labourthrd").val().split('|') : $("#hdthread").val().split('|');
                if (res.split(',')[i] != "") {
                    if (CategoryChecker.indexOf(res.split(',')[i]) > -1) {
                        $("body").data("tripcheck", "");
                        AsyncCall(res.split(',')[i], res.split(',')[i], "", "", "", loop);
                        console.log('hit count' + i + "-" + res.split(',')[i]);
                        AvailRequestCount++;
                        if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                            $("body").data("tripcheck", "R");
                            AsyncCall(res.split(',')[i], res.split(',')[i]);
                            AvailRequestCount++;
                            console.log('hit count' + AvailRequestCount + "-" + res.split(',')[i] + "#00");
                        }
                    }
                    else if (GetInLcccheck(res.split(',')[i]) == false) { //&& $("#hdn_saberairline").val().indexOf(res.split(',')[i]) < 0 // res.split(',')[i] != "AI"

                        var Corpairrequestthread = ($('body').data('segtype') == "D" ? (($("#hdn1sdom").val().indexOf(res.split(',')[i]) > -1) ? "1S" : ($("#hdn1gdom").val().indexOf(res.split(',')[i]) > -1) ? "1G" : "1A") : (($("#hdn1sint").val().indexOf(res.split(',')[i]) > -1) ? "1S" : ($("#hdn1gint").val().indexOf(res.split(',')[i]) > -1) ? "1G" : "1A"));
                        $("body").data("tripcheck", "");
                        AsyncCall(res.split(',')[i], Corpairrequestthread, "", "", "", loop);
                        console.log('hit count' + i + "-" + Corpairrequestthread);
                        AvailRequestCount++;
                        if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                            $("body").data("tripcheck", "R");
                            AsyncCall(res.split(',')[i], Corpairrequestthread);
                            AvailRequestCount++;
                            console.log('hit count' + AvailRequestCount + "-" + Corpairrequestthread + "#00");
                        }
                    }
                    //else if ($("#hdn_saberairline").val().indexOf(res.split(',')[i]) > -1) {// res.split(',')[i] == "AI"
                    //    $("body").data("tripcheck", "");
                    //    AsyncCall(res.split(',')[i], "1S", "", "", "", loop);
                    //    console.log('hit count' + i + "-" + "1S");
                    //    AvailRequestCount++;
                    //    if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                    //        $("body").data("tripcheck", "R");
                    //        AsyncCall(res.split(',')[i], "1S");
                    //        AvailRequestCount++;
                    //        console.log('hit count' + AvailRequestCount + "-" + "1S" + "#00");
                    //    }
                    //}
                }
            }
        }

    }
    else {

        var LCC_flights = FilterLccairlines(res);
        var Directthred = threadkeyfilter(res);
        var Fscthread = FetchFSC(LCC_flights, Directthred);
        var total_request = "";
        $("body").data("tripcheck", "");
        if ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "I") { //For International Multicity...

            var CategoryChecker = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#hdthread_dt").val().split('|') : ($("#hdn_producttype").val() == "UAE" && $("#rd_labour").prop("checked") == true) ? $("#hdn_labourthrd").val().split('|') : $("#hdthread").val().split('|');//$("#hdnMultiThread").val().split('|');
            for (var i = 0; i < CategoryChecker.length; i++) {
                if (RequestFiltration(CategoryChecker[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                    AsyncCall(res, CategoryChecker[i], aryOrgMul.join(','), aryDstMul.join(','), aryDptMul.join(','), loop);
                    console.log('hit count' + i + "-" + CategoryChecker[i]);
                    AvailRequestCount++;
                }
                else {
                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + CategoryChecker[i]);
                }
            }
        }
        else {
            var loopcnt = ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "D") ? aryOrgMul.length : 1;
            for (var loop = 0; loop < loopcnt; loop++) {  //For Domestic Multicity looping upto segment count .... For Other's loop only one time...
                if (LCC_flights != null && LCC_flights != "") {

                    var CategoryChecker = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#hdthread_dt").val().split('|') : ($("#hdn_producttype").val() == "UAE" && $("#rd_labour").prop("checked") == true) ? $("#hdn_labourthrd").val().split('|') : $("#hdthread").val().split('|');
                    var Lccthreads = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#lccthreadkey_dt").val().split('|') : $("#lccthreadkey").val().split('|');
                    for (var i = 0; i < Lccthreads.length; i++) {
                        var fns = jQuery.inArray(Lccthreads[i], CategoryChecker);
                        if (fns != -1 && Lccthreads[i] != null && Lccthreads[i] != "") {
                            if ($('#hdtxt_trip').val() == "M") {
                                if ($('body').data('segtype') == "D") {
                                    if (RequestFiltration(Lccthreads[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                        AsyncCall(LCC_flights, Lccthreads[i], aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                    } else {
                                        console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + Lccthreads[i]);
                                    }
                                }
                                else {
                                    if (RequestFiltration(Lccthreads[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                        AsyncCall(LCC_flights, Lccthreads[i], aryOrgMul.join(','), aryDstMul.join(','), aryDptMul.join(','), loop);
                                    } else {
                                        console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + Lccthreads[i]);
                                    }
                                }
                            }
                            else {
                                if (RequestFiltration(Lccthreads[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                    AsyncCall(LCC_flights, Lccthreads[i]);
                                    AvailRequestCount++;
                                    console.log('hit count' + i + "-" + Lccthreads[i]);
                                } else {
                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + Lccthreads[i]);
                                }
                            }
                        }

                    }
                }
                if (Directthred != null && Directthred != "") {
                    var DIRthreads = Directthred.split(',');
                    for (var j = 0; j < DIRthreads.length; j++) {
                        if (DIRthreads[j] != null && DIRthreads[j] != "") {
                            if ($('#hdtxt_trip').val() == "M") {
                                if ($('body').data('segtype') == "D") {
                                    if (RequestFiltration(DIRthreads[j], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                        AsyncCall(DIRthreads[j], DIRthreads[j], aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                    } else {
                                        console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + DIRthreads[j]);
                                    }
                                }
                                else {
                                    if (RequestFiltration(DIRthreads[j], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                        AsyncCall(DIRthreads[j], DIRthreads[j], aryOrgMul.join(','), aryDstMul.join(','), aryDptMul.join(','), loop);
                                    } else {
                                        console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + DIRthreads[j]);
                                    }
                                }
                            }
                            else {
                                if (RequestFiltration(DIRthreads[j], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                    AsyncCall(DIRthreads[j], DIRthreads[j]);
                                    AvailRequestCount++;
                                    console.log('hit count' + j + "-" + DIRthreads[j]);
                                } else {
                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + DIRthreads[j]);
                                }
                            }
                        }
                        var newthkey = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#newthreadkey_dt").val().split("~") : $("#newthreadkey").val().split("~"); //$("#newthreadkey").val().split("~");//$("#newthreadkey").val().split("~");
                        if (newthkey != null && newthkey != "") {
                            if (multithread(newthkey, DIRthreads[i])) {
                                for (var i = 0; i < flight_cat.length - 1; i++) {
                                    if ($('#hdtxt_trip').val() == "M") {
                                        if ($('body').data('segtype') == "D") {
                                            if (RequestFiltration(flight_cat[i]["category"], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                AsyncCall(flight_cat[i]["fn"], flight_cat[i]["category"], aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                            } else {
                                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + flight_cat[i]["category"]);
                                            }
                                        }
                                        else {
                                            if (RequestFiltration(flight_cat[i]["category"], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                                AsyncCall(flight_cat[i]["fn"], flight_cat[i]["category"], aryOrgMul.join(','), aryDstMul.join(','), aryDptMul.join(','), loop);
                                            } else {
                                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + flight_cat[i]["category"]);
                                            }
                                        }
                                    }
                                    else {
                                        if (RequestFiltration(flight_cat[i]["category"], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                            AsyncCall(flight_cat[i]["fn"], flight_cat[i]["category"]);
                                            AvailRequestCount++;
                                            console.log('hit count' + i + "-" + flight_cat[i]["category"]);
                                        } else {
                                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + flight_cat[i]["category"]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (Fscthread != null && Fscthread != "") {
                    var CategoryChecker = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#hdthread_dt").val().split('|') : ($("#hdn_producttype").val() == "UAE" && $("#rd_labour").prop("checked") == true) ? $("#hdn_labourthrd").val().split('|') : $("#hdthread").val().split('|');//$("#hdthread").val().split('|');
                    var FSCthreads = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#fscthreadkey_dt").val().split('|') : $("#fscthreadkey").val().split('|');// $("#fscthreadkey").val().split('|');



                    for (var i = 0; i < FSCthreads.length; i++) {

                        //if ($("#hdn_producttype").val() == "RIYA" && (Fscthread != "" && Fscthread.indexOf("AI") < 0 && FSCthreads[i] == "1S")) {
                        if ($("#hdn_producttype").val() == "RIYA" && (Fscthread != "" && Checksaberairline(AirlineCode, FSCthreads[i]) == false)) { /* For saber thread remove amadeus airlines*/
                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + Fscthread + "~" + FSCthreads[i]);
                        }
                        else {
                            var fns = jQuery.inArray(FSCthreads[i], CategoryChecker);
                            if (fns != -1 && FSCthreads[i] != null && FSCthreads[i] != "") {
                                if ($('#hdtxt_trip').val() == "M") {
                                    if ($('body').data('segtype') == "D") {
                                        if (RequestFiltration(FSCthreads[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                            AsyncCall(Fscthread, FSCthreads[i], aryOrgMul[loop], aryDstMul[loop], aryDptMul[loop], loop);
                                        } else {
                                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + FSCthreads[i]);
                                        }
                                    }
                                    else {
                                        if (RequestFiltration(FSCthreads[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                            AsyncCall(Fscthread, FSCthreads[i], aryOrgMul.join(','), aryDstMul.join(','), aryDptMul.join(','), loop);
                                        } else {
                                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + FSCthreads[i]);
                                        }
                                    }
                                }
                                else {
                                    if (RequestFiltration(FSCthreads[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                        AsyncCall(Fscthread, FSCthreads[i]);
                                        AvailRequestCount++;
                                    } else {
                                        console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + FSCthreads[i]);
                                    }
                                }
                            }
                            console.log('hit count' + i + "-" + FSCthreads[i]);
                        }
                    }
                }
            }
            if ($('#hdtxt_trip').val() == "R" && $("#hdn_rtsplflag").val() != "Y") {//trip condition addedby udhaya
                $("body").data("tripcheck", "R");
                if (LCC_flights != null && LCC_flights != "") {

                    var CategoryChecker = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#hdthread_dt").val().split('|') : ($("#hdn_producttype").val() == "UAE" && $("#rd_labour").prop("checked") == true) ? $("#hdn_labourthrd").val().split('|') : $("#hdthread").val().split('|');//$("#hdthread").val().split('|');
                    var Lccthreads = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#lccthreadkey_dt").val().split('|') : $("#lccthreadkey").val().split('|');//$("#lccthreadkey").val().split('|');
                    for (var i = 0; i < Lccthreads.length; i++) {
                        var fns = jQuery.inArray(Lccthreads[i], CategoryChecker);
                        if (fns != -1 && Lccthreads[i] != null && Lccthreads[i] != "") {
                            if (RequestFiltration(Lccthreads[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                AsyncCall(LCC_flights, Lccthreads[i]);
                                AvailRequestCount++;
                                console.log('hit count' + i + "-" + Lccthreads[i]);
                            } else {
                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + Lccthreads[i]);
                            }
                        }

                    }
                }
                if (Directthred != null && Directthred != "") {
                    var DIRthreads = Directthred.split(',');
                    for (var j = 0; j < DIRthreads.length; j++) {
                        if (DIRthreads[j] != null && DIRthreads[j] != "") {
                            if (RequestFiltration(DIRthreads[j], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                AsyncCall(DIRthreads[j], DIRthreads[j]);
                                AvailRequestCount++;
                                console.log('hit count' + i + "-" + DIRthreads[j]);
                            } else {
                                console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + DIRthreads[j]);
                            }
                        }
                        var newthkey = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#newthreadkey_dt").val().split("~") : $("#newthreadkey").val().split("~"); //$("#newthreadkey").val().split("~");
                        if (newthkey != null && newthkey != "") {
                            if (multithread(newthkey, DIRthreads[i])) {
                                for (var i = 0; i < flight_cat.length - 1; i++) {
                                    if (RequestFiltration(flight_cat[i]["category"], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                        AsyncCall(flight_cat[i]["fn"], flight_cat[i]["category"]);
                                        AvailRequestCount++;
                                        console.log('hit count' + i + "-" + flight_cat[i]["category"]);
                                    } else {
                                        console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + flight_cat[i]["category"]);
                                    }
                                }
                            }
                        }

                    }
                }
                if (Fscthread != null && Fscthread != "") {
                    var CategoryChecker = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#hdthread_dt").val().split('|') : ($("#hdn_producttype").val() == "UAE" && $("#rd_labour").prop("checked") == true) ? $("#hdn_labourthrd").val().split('|') : $("#hdthread").val().split('|');
                    var FSCthreads = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#fscthreadkey_dt").val().split('|') : $("#fscthreadkey").val().split('|');//$("#fscthreadkey").val().split('|');
                    for (var i = 0; i < FSCthreads.length; i++) {
                        if ($("#hdn_producttype").val() == "RIYA" && (Fscthread != "" && Checksaberairline(AirlineCode, FSCthreads[i]) == false)) {
                            console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + Fscthread + "~" + FSCthreads[i]);
                        }
                        else {
                            var fns = jQuery.inArray(FSCthreads[i], CategoryChecker);
                            if (fns != -1 && FSCthreads[i] != null && FSCthreads[i] != "") {
                                if (RequestFiltration(FSCthreads[i], Base_origincountry, Base_desnationcountry, Base_OriginCity, Base_DestinationCity)) {
                                    AsyncCall(Fscthread, FSCthreads[i]);
                                    AvailRequestCount++;
                                    console.log('hit count' + i + "-" + FSCthreads[i]);
                                } else {
                                    console.log("Request Void Filtered for (" + Base_OriginCity + " - " + Base_DestinationCity + ") " + FSCthreads[i]);
                                }
                            }
                        }
                    }

                }

            }
        }
    }

}

function FilterLccairlines(res) {
    var FilterLCC = "";
    var Splitairline = res.split(',');

    var CategoryChecker = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#hdthread_dt").val().split('|') : ($("#hdn_producttype").val() == "UAE" && $("#rd_labour").prop("checked") == true) ? $("#hdn_labourthrd").val().split('|') : $("#hdthread").val().split('|');//$("#hdthread").val().split('|');
    //for (var i = 0; i < Splitairline.length; i++) {
    $.ajax({
        type: "GET",
        url: lccAirXML_path,
        async: false,
        dataType: "xml",
        success: function (xml) {
            $(xml).find('LCCAIRLINES').each(function () {
                SChar = $(this).find('AIR_AIRLINE_CODE').text();
                var fns = jQuery.inArray(SChar, Splitairline);
                var Dirthread = jQuery.inArray(SChar, CategoryChecker);
                if (fns != -1 && Dirthread == -1) {
                    FilterLCC += SChar + ",";
                }
            });
        },
        error: function (ex) {
            showerralert("An error occurred while processing XML file (07).", 5000, "");
        }
    });
    return FilterLCC;
}

function threadkeyfilter(ress) {
    var response = "";
    //var CategoryChecker = $("#hdthread").val().split('|');

    var CategoryChecker = ($('body').data('segtype') == "D" && $("#hdn_producttype").val() == "RIYA") ? $("#hdthread_dt").val().split('|') : ($("#hdn_producttype").val() == "UAE" && $("#rd_labour").prop("checked") == true) ? $("#hdn_labourthrd").val().split('|') : $("#hdthread").val().split('|');
    var splitres = ress.split(',');
    for (var i = 0; i < splitres.length; i++) {
        var fns = jQuery.inArray(splitres[i], CategoryChecker);
        if (fns != -1) {
            response += splitres[i] + ",";
        }
    }
    return response;
}

function FetchFSC(LCC_flights, Directthred) {
    if (FLAG != "M") {
        //AirlineCode = $('#hdtxt_trip').val() == "M" ? $("#MulFlightName").val().split(',') : $("#FlightName").val().split(',');
        // AirlineCode = $('#hdtxt_trip').val() == "M" ? $("#MulFlightName").val() : $("#FlightName").val();
        Airlinecode = ($("#rd_corporate_retail").prop("checked") == true) ? Corp_aircode : $('#hdtxt_trip').val() == "M" ? $("#MulFlightName").val() : $("#FlightName").val();
        Airlinecode = Airlinecode != null ? Airlinecode : [];
        Airlinecode = Airlinecode.join(',');
    }

    var response = "";
    var threadvalues = LCC_flights + Directthred;
    var Splitthread = threadvalues.split(',');
    var CategoryChecker = AirlineCode.split(',');// $('#hdtxt_trip').val() == "M" ? $("#MulFlightName").val().split(',') : $("#FlightName").val().split(',');
    for (var i = 0; i < CategoryChecker.length; i++) {
        if (threadvalues != "") {
            var fns = jQuery.inArray(CategoryChecker[i], Splitthread);
            if (fns == -1 && CategoryChecker[i].length == 2) {
                response += CategoryChecker[i] + ",";
            }
        }
        else {
            //response = $('#hdtxt_trip').val() == "M" ? $("#MulFlightName").val() : $("#FlightName").val();
            response = AirlineCode;// $('#hdtxt_trip').val() == "M" ? $("#MulFlightName").val() : $("#FlightName").val();
        }
    }
    return response;
}

function multithread(newthkey, fn) {
    flight_cat = [];
    for (var i = 0; i < newthkey.length ; i++) {
        var subkey_fn = newthkey[i].split("@")[0];
        if (subkey_fn == fn) {
            var subkey_cat = newthkey[i].split("@")[1];
            if (subkey_cat.indexOf("|") != -1) {
                for (var j = 0; j < subkey_cat.split("|").length ; j++) {
                    var sp_cat = subkey_cat.split("|")[j];
                    var flight = {
                        "fn": subkey_fn,
                        "category": sp_cat
                    }
                    flight_cat.push(flight);
                }
            }
            else {
                var flight = {
                    "fn": subkey_fn,
                    "category": subkey_cat
                }
                flight_cat.push(flight);
            }
            flight_cat.push(flight);
        }
    }
    return flight_cat;
}

var bar1 = "";
var aryAvailParam = [];
var aryPrevSearch = [];
var strfromCity = "";
var strtoCity = "";
var strDepartureDate = "";
var strRetDate = "";
var strAdults = "";
var strChildrens = "";
var strInfants = "";
var strTrip = "";
var Airline = "";
var FCategory = "";
var Class = "";
var MultiCity = "";
var HSearch = "";
var DirectSearch = "";
var O_R_Flag = 0;
var AvailParams = "";
var AppCurrency = "";
var strSegTyp = "";

var ClientID = "";
var BranchID = "";
var GroupID = "";
var reqker = "";

function AsyncCall(fn, cate, Morg, Mdes, Mdep, setcnt, fltnumvia) {

    AvailParams = "";
    strfromCity = "";
    strtoCity = "";
    strDepartureDate = "";
    strRetDate = "";
    strAdults = "";
    strChildrens = "";
    strInfants = "";
    strTrip = "";
    Airline = "";
    FCategory = "";
    Class = "";
    MultiCity = "";
    HSearch = "";
    DirectSearch = "";
    segmenttype = $('body').data('segtype'); //$("#chbdomestic")[0].checked = true ? "D" : "I";
    time = 100000;
    O_R_Flag = 0;
    UID = UIDCORP;
    roundtripflg = "0";
    AppCurrency = "";
    reqker = sessionreq;
    var Corporatedetails = "";

    var availagent = $("#hdn_availagent").val();
    var availterminal = $("#hdn_availterminal").val();
    var Agencyname = $("#hdn_availagencyname").val();

    if ($("#ddlclient").length > 0) {
        ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
        BranchID = sessionStorage.getItem('branchid') != null ? sessionStorage.getItem('branchid').trim() : "";
        GroupID = sessionStorage.getItem('Groupid') != null ? sessionStorage.getItem('Groupid').trim() : "";
        Agencyname = sessionStorage.getItem('clientname') != null ? sessionStorage.getItem('clientname').trim() : "";
        availagent = ClientID;
    }
    else {

        ClientID = "";
        BranchID = "";
        GroupID = "";
    }

    var fltnumkey = "";
    var fltsegmentkey = "";
    var mulfltnumvia = "";

    if (FLAG == "M") {
        if (SectorSpecific == "TRUE") {
            roundtripflg = "1";
        }
    }
    else {
        if ($('#rd_roundtripcnt')[0].checked == true) {
            roundtripflg = "1";
        }
    }

    if ($('#hdtxt_trip').val() != "O") {
        time = 300000;
    }

    if ($('#hdtxt_trip').val() != "M") {

        if ($("#hdn_rtsplflag").val() == "Y") { /* For round trip spl LCC avail*/

            strfromCity = $("#hdtxt_origin")[0].value;
            strtoCity = $("#hdtxt_destination")[0].value;
            strDepartureDate = $("#hdtxt_depa_date")[0].value;
            strRetDate = $("#hdtxt_Arrivedate")[0].value;
            strAdults = $("#hdtxt_adultcount")[0].value;
            strChildrens = $("#hdtxt_childcount")[0].value != "" ? $("#hdtxt_childcount")[0].value : "0";
            strInfants = $("#hdtxt_infantcount")[0].value != "" ? $("#hdtxt_infantcount")[0].value : "0";
            strTrip = $("#hdtxt_trip")[0].value;
            Class = FLAG == "M" ? MobileClass : $("#ddlclass").val(); //Class = $("#ddlclass").val();
            MultiCity = $('#hdnMulti').val();
            HSearch = $('#hdnHS').val();
            DirectSearch = $('#hdndirect').val();
            console.log('hit count' + requestcount + "-" + cate + "(" + strfromCity + "-" + strtoCity + ")");
            O_R_Flag = "1";
            UID = UIDCORP;

            if (FLAG != "M") {
                fltnumkey = $("#Flightno1").val().toUpperCase().trim() + "," + $("#Flightno2").val().toUpperCase().trim()
                fltsegmentkey = $("#Via1").val().toUpperCase().trim() + "," + $("#Via2").val().toUpperCase().trim()
            }
            AppCurrency = FLAG == "M" ? MobileAppCurrency : $("#ddlNationality").val();// AppCurrency = $("#ddlNationality").val();

            ClientID = ClientID;
            BranchID = BranchID;
            GroupID = GroupID;
        } else if ($("#hdn_rtsplflag").val() != "Y") {
            if ($("body").data("tripcheck") == "R") {
                strfromCity = $("#hdtxt_destination")[0].value;
                strtoCity = $("#hdtxt_origin")[0].value;
                strDepartureDate = $("#hdtxt_Arrivedate")[0].value;
                strRetDate = $("#hdtxt_depa_date")[0].value;
                strAdults = $("#hdtxt_adultcount")[0].value;
                strChildrens = $("#hdtxt_childcount")[0].value != "" ? $("#hdtxt_childcount")[0].value : "0";
                strInfants = $("#hdtxt_infantcount")[0].value != "" ? $("#hdtxt_infantcount")[0].value : "0";
                strTrip = $("#hdtxt_trip")[0].value;
                Class = FLAG == "M" ? MobileClass : $("#ddlclass").val(); //Class = $("#ddlclass").val();

                MultiCity = $('#hdnMulti').val();
                HSearch = $('#hdnHS').val();
                DirectSearch = $('#hdndirect').val();
                console.log('hit count' + requestcount + "-" + cate + "(" + strfromCity + "-" + strtoCity + ")");
                O_R_Flag = "2";
                UID = UIDCORP;
                if (FLAG != "M") {//STS-166
                    fltnumkey = $("#Flightno2").val().toUpperCase().trim() + "," + $("#Flightno1").val().toUpperCase().trim() /* For double grid flight no option */
                    fltsegmentkey = $("#Via2").val().toUpperCase().trim() + "," + $("#Via1").val().toUpperCase().trim() /* For double grid via option */
                }

                AppCurrency = FLAG == "M" ? MobileAppCurrency : $("#ddlNationality").val(); //AppCurrency = $("#ddlNationality").val();

                ClientID = ClientID;
                BranchID = BranchID;
                GroupID = GroupID;



            } else {
                strfromCity = $("#hdtxt_origin")[0].value;
                strtoCity = $("#hdtxt_destination")[0].value;
                strDepartureDate = $("#hdtxt_depa_date")[0].value;
                strRetDate = $("#hdtxt_Arrivedate")[0].value;
                strAdults = $("#hdtxt_adultcount")[0].value;
                strChildrens = $("#hdtxt_childcount")[0].value != "" ? $("#hdtxt_childcount")[0].value : "0";
                strInfants = $("#hdtxt_infantcount")[0].value != "" ? $("#hdtxt_infantcount")[0].value : "0";
                strTrip = $("#hdtxt_trip")[0].value;
                Class = FLAG == "M" ? MobileClass : $("#ddlclass").val(); //Class = $("#ddlclass").val();
                MultiCity = $('#hdnMulti').val();
                HSearch = $('#hdnHS').val();
                DirectSearch = $('#hdndirect').val();
                console.log('hit count' + requestcount + "-" + cate + "(" + strfromCity + "-" + strtoCity + ")");
                O_R_Flag = "1";
                UID = UIDCORP;

                if (FLAG != "M") {
                    fltnumkey = $("#Flightno1").val().toUpperCase().trim() + "," + $("#Flightno2").val().toUpperCase().trim()
                    fltsegmentkey = $("#Via1").val().toUpperCase().trim() + "," + $("#Via2").val().toUpperCase().trim()
                }
                AppCurrency = FLAG == "M" ? MobileAppCurrency : $("#ddlNationality").val();// AppCurrency = $("#ddlNationality").val();

                ClientID = ClientID;
                BranchID = BranchID;
                GroupID = GroupID;
            }
        }
        if ($("#rd_corporate_retail").prop("checked") == true && $('body').data("bhdcorporatefaredt") != "") { //sts185
            AirlineCode = "";
            if (fn.length > 3) {
                //var checkair = fn != "" ? fn.split(',')[0] : cate;
                var aircode = $('body').data("bhdcorporatefaredt").split(',');
                for (var j = 0; j < fn.split(',').length; j++) {
                    var checkair = fn.split(',')[j];
                    if (checkair != "") {
                        for (var i = 0; i < aircode.length; i++) {
                            if (aircode[i].split('~')[0] == checkair) {
                                Corporatedetails = aircode[i];
                            }
                        }
                    }
                }
                for (var i = 0; i < aircode.length - 1; i++) {
                    if (aircode[i].split('~')[0] == checkair) {
                        Corporatedetails = aircode[i];
                    }
                }
            }
            else {
                var checkair = fn != "" ? fn.split(',')[0] : cate;
                var aircode = $('body').data("bhdcorporatefaredt").split(',');
                for (var i = 0; i < aircode.length - 1; i++) {
                    if (aircode[i].split('~')[0] == checkair) {
                        Corporatedetails = aircode[i];
                    }
                }
            }
        }
    }
    else { //For Multicity on 20170524...
        strfromCity = Morg;
        strtoCity = Mdes;
        strDepartureDate = Mdep;
        strRetDate = Mdep;
        strAdults = $("#hdtxt_adultcount")[0].value;
        strChildrens = $("#hdtxt_childcount")[0].value != "" ? $("#hdtxt_childcount")[0].value : "0";
        strInfants = $("#hdtxt_infantcount")[0].value != "" ? $("#hdtxt_infantcount")[0].value : "0";
        strTrip = $("#hdtxt_trip")[0].value;
        Class = FLAG == "M" ? MobileClass : $("#grpcmbFlightClass").val();// Class = FLAG == "M" ? MobileClass : $("#ddlclass").val(); //Class = $("#grpcmbFlightClass").val();

        MultiCity = $('#hdnMulti').val();
        HSearch = $('#hdnHS').val();
        DirectSearch = $('#hdndirect').val();
        console.log('hit count' + requestcount + "-" + cate + "(" + strfromCity + "-" + strtoCity + " - " + strDepartureDate + ")");
        O_R_Flag = setcnt != null && setcnt != "undefined" && setcnt != undefined && setcnt != "" ? (setcnt + 1) : 1;
        UID = UIDCORP;
        mulfltnumvia = fltnumvia;
        AppCurrency = FLAG == "M" ? MobileAppCurrency : $("#ddlMNationality").val(); //AppCurrency = $("#ddlMNationality").val();

        ClientID = ClientID;
        BranchID = BranchID;
        GroupID = GroupID;
    }

    var UIDTID = UIDCORP + "$" + sessionreq;

    var Splthread = $("#hdn_rtsplflag").val() == "" ? "N" : $("#hdn_rtsplflag").val();
    var StdFare = $("#StudentFare").length > 0 ? $("#StudentFare")[0].checked : false;
    var ArmyFare = $("#ArmyFare").length > 0 ? $("#ArmyFare")[0].checked : false;
    var SnrcitizenFare = $("#SrCitizenFare").length > 0 ? $("#SrCitizenFare")[0].checked : false;

    var LabourFare = $("#hdn_producttype").val() == "UAE" && $("#rd_labour").prop("checked") == true ? true : false;

    AvailParams = {
        strfromCity: strfromCity, strtoCity: strtoCity, strDepartureDate: strDepartureDate, strRetDate: strRetDate, strAdults: strAdults,
        strChildrens: strChildrens, strInfants: strInfants, strTrip: strTrip, Airline: fn, FCategory: cate, Class: Class, MultiCity: MultiCity,
        HSearch: HSearch, DirectSearch: DirectSearch, segmenttype: segmenttype + "_" + O_R_Flag, roundtripflg: roundtripflg, availagent: availagent,
        availterminal: availterminal, Agencyname: Agencyname, UID: UIDTID, AppCurrency: AppCurrency, fltnumkey: fltnumkey, fltsegmentkey: fltsegmentkey,
        mulfltnumvia: mulfltnumvia, ClientID: ClientID, BranchID: BranchID, GroupID: GroupID, reqker: sessionreq, Promothread: Corporatedetails,
        Threadspl: Splthread, StudentFare: StdFare, ArmyFare: ArmyFare, SnrcitizenFare: SnrcitizenFare, LabourFare: LabourFare, strBookletFare: $("#Mulrd_Booklet").is(":checked")
    }
    requestcount++;
    FetchAvail(AvailParams, time);
    sessionreq++;
}

var groupmain_array = [];
var groupmain_ret = [];
var allAIRLINECode = "";
var Faretypefilter = "";
var filterdarray = [];
var Main_totalfilterdarray = [];
Main_totalfilterdarray_ret = [];
var firstapndclr_flg = false;
var obj1 = {
    count: 0,
    setcount: 0
};
var totalgridcount = "";
var strTrip = "";
var strSegTyp = "";
function FetchAvail(AvailParam, time) {

    strTrip = AvailParam.strTrip;
    strSegTyp = AvailParam.segmenttype.split('_')[0];
    strSegTyp = AvailParam.segmenttype.split('_')[0];
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: FlightsearchUrl, 		// Location of the service
        data: JSON.stringify(AvailParam),
        contentType: "application/json; charset=utf-8",
        async: true,
        dataType: "json",
        timeout: time,
        success: function (json) {//On Successful service call
            AvailResponseCount++;
            groparr = [];
            groparr_ret = [];
            if (json.Status == "-1") {
                window.location.href = sessionExb;
                return false;
            }
            var result = json.Result;
            var result_ex = json.Result[0];
            //var sp = result_ex.split("sri");
            load_Increase += AvailRequestCount == AvailResponseCount ? 100 : 100 / AvailRequestCount;
            if (AvailResponseCount == 1) {
                if (bar1 != "")
                    bar1.destroy();
                bar1 = $('#avail-more-load').progressbarManager({
                    totalValue: 100,
                    animate: true,
                    stripe: true,
                    showValueHandler: function (bar) {
                        if ($("#hdn_AppHosting").val() != "BSA") {
                            bar.elem.text("Searching more flights..."); //totflt+" flight(s) found."
                        }
                    }
                });
                $("#btn_Search").prop('disabled', false);
                $("#InterNational_Avail").css("display", "none");
                $('#AfterLoading').css("display", "block");
                $("#dvAvail").css('display', 'block');
                $("#dvSearch").css("display", "none");
                $('.chkfilter').prop('checked', false);
                aryPrevSearch = [];
                aryAvailParam = [];
            }

            if (bar1 != "") {
                bar1.setValue(load_Increase);
                if ($("#hdn_AppHosting").val() == "BSA") {
                    bar1.style('info');
                }
                else {
                    bar1.style(load_Increase < 30 ? 'danger' : load_Increase < 50 ? 'warning' : load_Increase < 75 ? 'info' : 'success');
                }
            }
            var a = navigator.userAgent;
            scrollLock = true;
            FilterLock = true;
            if (json.Status == "1") {
                callrequestcnt++;
                availabibindcount++;
                if (callrequestcnt == 1) {
                    $.unblockUI();

                    if ($("#hdn_producttype").val() == "RIYA") {
                        if ($("#bgid").hasClass("bg-bg-chrome_bk")) {
                            $("#bgid").removeClass("bg-bg-chrome_bk")
                        }
                        else {
                            $("#bgid").removeClass("bg-bg-chrome_bk")
                        }
                    }

                    $(".searchviews").hide();
                    $('#dvAvailView').show();
                    $('#roundtripheader').hide();
                    $('#onewayheaders').show();
                    BindSearchvaltoHeader();
                    setTimeout(function () { // To feel Animation effect of right side fixed icons have to wait 3 seconds... (for first avail binding)... (20170425);
                        $('#dvrightstckyIcons').show();
                        $('#spnSortindicate').show();
                    }, 3000);
                }

                if ($("#hdn_rtsplflag").val() == "Y") { /*For Roundtripspl Riya concept added by rajesh*/

                    var JsonObj = JSON.parse(result[4].replace("[object Object]", ""));
                    var JsonObj_Ret = JSON.parse(result[8].replace("[object Object]", ""));
                    for (var i in JsonObj) {
                        groparr.push(JsonObj[i]);
                        groupmain_array.push(JsonObj[i]);
                    }
                    for (var i in JsonObj_Ret) {
                        groparr_ret.push(JsonObj_Ret[i]);
                        groupmain_ret.push(JsonObj_Ret[i]);
                    }

                    if (availabibindcount == 1) {

                        /* for onward flight formation */
                        firstapndclr_flg = true
                        var i = 0;
                        var pos = 0;
                        var groupedfli = groparr.reduce(function (previousValue, currentValue) {
                            previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                            previousValue[currentValue.flightid].push(currentValue);
                            return previousValue;
                        }, {});  //group based on fareid and flight id srinath

                        var grouparray = [];
                        for (var obj in groupedfli) {
                            grouparray.push(groupedfli[obj]);

                        }

                        grouparray.sort(function (a, b) {
                            if ($("#hdn_AvailFormat").val() == "NAT") {
                                return a[0].NETFare - b[0].NETFare;
                                //return value[0].NETFare >= Number(minval) && value[0].NETFare <= Number(maxval);
                            }
                            else { return a[0].GFare - b[0].GFare; }
                        });  //fare soring after array
                        var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                            previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                            previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                            return previousValue;
                        }, {});

                        var grouparray_orgdes = [];
                        for (var obj in groupedorgdes) {
                            grouparray_orgdes.push(groupedorgdes[obj]);
                        }

                        grouparray_orgdes = grouparray_orgdes.length > 10 ? grouparray_orgdes.slice(0, 10) : grouparray_orgdes;
                        if (grouparray_orgdes.length < 3) {
                            availabibindcount = 0;
                        }
                        /* for onward flight formation */

                        /* for Retward flight formation */
                        firstapndclr_flg = true
                        var j = 0;
                        var pos = 0;
                        var groupedfli_ret = groparr_ret.reduce(function (previousValue, currentValue) {
                            previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                            previousValue[currentValue.flightid].push(currentValue);
                            return previousValue;
                        }, {});  //group based on fareid and flight id srinath

                        var grouparray_ret = [];
                        for (var obj in groupedfli_ret) {
                            grouparray_ret.push(groupedfli_ret[obj]);
                        }

                        grouparray_ret.sort(function (a, b) {
                            if ($("#hdn_AvailFormat").val() == "NAT") {
                                return a[0].NETFare - b[0].NETFare;
                                //return value[0].NETFare >= Number(minval) && value[0].NETFare <= Number(maxval);
                            }
                            else { return a[0].GFare - b[0].GFare; }
                        });  //fare soring after array
                        var groupedorgdes_ret = grouparray_ret.reduce(function (previousValue, currentValue) {
                            previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                            previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                            return previousValue;
                        }, {});

                        var grouparray_orgdes_ret = [];
                        for (var obj in groupedorgdes_ret) {
                            grouparray_orgdes_ret.push(groupedorgdes_ret[obj]);
                        }

                        grouparray_orgdes_ret = grouparray_orgdes_ret.length > 10 ? grouparray_orgdes_ret.slice(0, 10) : grouparray_orgdes_ret;
                        if (grouparray_orgdes_ret.length < 3) {
                            availabibindcount = 0;
                        }

                        /* for Retward flight formation */

                        /*Onward Binding*/
                        for (i = 0; i < grouparray_orgdes.length; i++) {
                            var formatarray = [];//create  same aray for loop condition  20180310
                            formatarray.push(grouparray_orgdes[i]);
                            result[4] = grouparray_orgdes[i];
                            var returnresponse_Avail_bf = LoadFlights(result[4], obj1);
                            var splt_returnresponse_Avail = returnresponse_Avail_bf.split('Av@i');
                            var returnresponse_Avail = splt_returnresponse_Avail[0];

                            if (returnresponse_Avail != null && returnresponse_Avail != "") {
                                if (callrequestcnt == 1) {
                                    $("#fromdate").html($("#hdtxt_origin")[0].value);
                                    $("#todate").html($("#hdtxt_destination")[0].value);
                                    $("#oneway_deptdate").html($("#hdtxt_depa_date")[0].value);
                                    $("#round_depdate").show();
                                    $("#singlearrow").hide();
                                    $("#doublearrow").show();
                                    $("#ifen").show();
                                    $("#round_depdate").html($("#hdtxt_Arrivedate")[0].value);
                                    $('#spnTotalFRoundAvailCntParent').show();
                                }
                                $(".sriloadani").css("display", "none");
                                $("#availset_R_1").append(returnresponse_Avail);//For Roundtrip and Multicity.... strTrip=R or M, result[6]=1 or 2 or... upto multicity count...
                                var oldcnt = $("#spnActlFAvailCnt_R_1").html();
                                $('#spnActlFAvailCnt_R_1' + ',#spnTotalFAvailCnt_R_1').html(Number(oldcnt) + obj1.setcount);
                            }
                        }
                        /*Onward Binding*/
                        /*Retward Binding*/

                        for (i = 0; i < grouparray_orgdes_ret.length; i++) {
                            var formatarray = [];//create  same aray for loop condition  20180310
                            formatarray.push(grouparray_orgdes_ret[i]);
                            result[8] = grouparray_orgdes_ret[i];
                            var returnresponse_Avail_bf = LoadFlights(result[8], obj1);
                            var splt_returnresponse_Avail = returnresponse_Avail_bf.split('Av@i');
                            var returnresponse_Avail = splt_returnresponse_Avail[0];
                            if (returnresponse_Avail != null && returnresponse_Avail != "") {
                                if (callrequestcnt == 1) {
                                    $("#fromdate").html($("#hdtxt_origin")[0].value);
                                    $("#todate").html($("#hdtxt_destination")[0].value);
                                    $("#oneway_deptdate").html($("#hdtxt_depa_date")[0].value);
                                    $("#round_depdate").show();
                                    $("#singlearrow").hide();
                                    $("#doublearrow").show();
                                    $("#ifen").show();
                                    $("#round_depdate").html($("#hdtxt_Arrivedate")[0].value);
                                    $('#spnTotalFRoundAvailCntParent').show();
                                }

                                $(".sriloadani").css("display", "none");
                                $("#availset_R_2").append(returnresponse_Avail);//For Roundtrip and Multicity.... strTrip=R or M, result[6]=1 or 2 or... upto multicity count...
                                var oldcnt = $('#spnActlFAvailCnt_R_2').html();
                                $('#spnActlFAvailCnt_R_2' + ',#spnTotalFAvailCnt_R_2').html(Number(oldcnt) + obj1.setcount);
                            }
                        }
                        /*Retward Binding*/
                        $("#dvNetfare").addClass("clsNFareRoundtrip");
                    }
                }
                else {

                    var JsonObj = JSON.parse(result[4].replace("[object Object]", ""));
                    for (var i in JsonObj) {
                        groparr.push(JsonObj[i]);
                        groupmain_array.push(JsonObj[i]);
                    }

                    //  airlinematrix_fun(groparr, groparr["0"].bindside_6_arg);  //for availabilty matrix function bind stuff 20180322
                    if (availabibindcount == 1 || strTrip == "R") {
                        firstapndclr_flg = true
                        var i = 0;
                        var pos = 0;
                        var groupedfli = groparr.reduce(function (previousValue, currentValue) {
                            previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                            previousValue[currentValue.flightid].push(currentValue);
                            return previousValue;
                        }, {});  //group based on fareid and flight id srinath

                        var grouparray = [];
                        for (var obj in groupedfli) {
                            grouparray.push(groupedfli[obj]);

                        }

                        grouparray.sort(function (a, b) {
                            if ($("#hdn_AvailFormat").val() == "NAT") {
                                return a[0].NETFare - b[0].NETFare;
                                //return value[0].NETFare >= Number(minval) && value[0].NETFare <= Number(maxval);
                            }
                            else { return a[0].GFare - b[0].GFare; }
                        });  //fare soring after array
                        var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                            previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                            previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                            return previousValue;
                        }, {});

                        var grouparray_orgdes = [];
                        for (var obj in groupedorgdes) {
                            grouparray_orgdes.push(groupedorgdes[obj]);
                        }

                        grouparray_orgdes = grouparray_orgdes.length > 10 ? grouparray_orgdes.slice(0, 10) : grouparray_orgdes;
                        if (grouparray_orgdes.length < 3) {
                            availabibindcount = 0;
                        }


                        for (i = 0; i < grouparray_orgdes.length; i++) {
                            var formatarray = [];//create  same aray for loop condition  20180310
                            formatarray.push(grouparray_orgdes[i]);
                            result[4] = grouparray_orgdes[i];
                            var returnresponse_Avail_bf = LoadFlights(result[4], obj1);
                            var splt_returnresponse_Avail = returnresponse_Avail_bf.split('Av@i');
                            var returnresponse_Avail = splt_returnresponse_Avail[0];
                            if (returnresponse_Avail != null && returnresponse_Avail != "") {
                                if (strTrip == "O" || strTrip == "Y" || (strTrip == "M" && strSegTyp == "I")) { //Oneway and Roundtrip Special...
                                    if (callrequestcnt == 1) {
                                        $("#fromdate").html(AvailParam.strfromCity);
                                        $("#todate").html(AvailParam.strtoCity);
                                        $("#oneway_deptdate").html(AvailParam.strDepartureDate);
                                        $("#round_depdate").hide();
                                        $("#ifen").hide();
                                        $("#singlearrow").show();
                                        $("#doublearrow").hide();
                                    }
                                    $("#availset_O_1").append(returnresponse_Avail);
                                    $(".sriloadani").css("display", "none");
                                    $('#spnTotalFAvailCntParent').show();
                                    $('#spnTotalFAvailCnt,#spnActlFAvailCnt').html(obj1.count);
                                }
                                else {
                                    if (strTrip == "R") {
                                        if (callrequestcnt == 1) {
                                            $("#fromdate").html($("#hdtxt_origin")[0].value);
                                            $("#todate").html($("#hdtxt_destination")[0].value);
                                            $("#oneway_deptdate").html($("#hdtxt_depa_date")[0].value);
                                            $("#round_depdate").show();
                                            $("#singlearrow").hide();
                                            $("#doublearrow").show();
                                            $("#ifen").show();
                                            $("#round_depdate").html($("#hdtxt_Arrivedate")[0].value);
                                            $('#spnTotalFRoundAvailCntParent').show();
                                            $("#dvNetfare").addClass("clsNFareRoundtrip");
                                        }
                                    }
                                    if (strTrip == "M") {
                                        if (callrequestcnt == 1) {
                                            $("#dvNetfare").addClass("clsNFareRoundtrip");
                                        }
                                    }
                                    $(".sriloadani").css("display", "none");
                                    $("#availset_" + strTrip + "_" + splt_returnresponse_Avail[1] + "").append(returnresponse_Avail);//For Roundtrip and Multicity.... strTrip=R or M, result[6]=1 or 2 or... upto multicity count...
                                    var oldcnt = $('#spnTotalFAvailCnt_' + strTrip + '_' + splt_returnresponse_Avail[1]).html();
                                    $('#spnActlFAvailCnt_' + strTrip + '_' + splt_returnresponse_Avail[1] + ',#spnTotalFAvailCnt_' + strTrip + '_' + splt_returnresponse_Avail[1]).html(Number(oldcnt) + obj1.setcount);
                                }
                            }
                        }
                    }
                }
            }
            else {
                console.log(json.Message);
            }

            if (AvailRequestCount == AvailResponseCount) {
                if ($("#hdn_rtsplflag").val() == "N") {
                    $.map(groupmain_array, function (JSONdata, j) {
                        if (allAIRLINECode.indexOf(JSONdata.plt) == -1) {
                            try {
                                allAIRLINECode += JSONdata.plt + ",";
                                availairname = airlinename(JSONdata.plt != null ? JSONdata.plt : "").split("(")[0];
                                allAIRLINENAME += availairname + ",";
                                strBuildairCheck = "";
                                strBuildairCheck += '<div class="clonedCBox cBox">'
                                strBuildairCheck += '<div class="customCBox">'
                                strBuildairCheck += '<div class="cntr">'
                                strBuildairCheck += '<input style="display: none;" id="FAairln' + JSONdata.plt + '_dynamic_1" class="cb chkfilter chkfilterplane" data-filter="air' + JSONdata.plt + '" type="checkbox" />'
                                strBuildairCheck += '<label class="cbx" for="FAairln' + JSONdata.plt + '_dynamic_1"></label>'
                                strBuildairCheck += '<label class="lbl fltcntnt" for="FAairln' + JSONdata.plt + '_dynamic_1">' + availairname + '</label>'
                                strBuildairCheck += '</div>'
                                strBuildairCheck += '</div>'
                                strBuildairCheck += '<div class="onlyBtn">Only</div>'
                                strBuildairCheck += '</div>'
                                var listairlinenames = allAIRLINENAME.replace(",", "");
                                if (listairlinenames.trim() != "") {
                                    $(".airfilterallcheck").append(strBuildairCheck);
                                } else {
                                    $(".airfilterallcheck").append("Airline filtration is empty.");
                                }
                            } catch (e) {
                                //  alert(e.message);
                            }
                        }
                        if (Faretypefilter.indexOf(JSONdata.FareType) == -1) {
                            try {
                                Faretypefilter += JSONdata.FareType + ",";
                                strBuildairCheck = "";
                                strBuildairCheck += '<div class="clonedCBox cBox">'
                                strBuildairCheck += '<div class="customCBox">'
                                strBuildairCheck += '<div class="cntr">'
                                strBuildairCheck += '<input style="display: none;" id="faretypln' + JSONdata.FareType + '_dynamic_1" class="cb chkfilter chkfilterfaretype" data-filter="faretype' + JSONdata.FareType + '" type="checkbox" />'
                                strBuildairCheck += '<label class="cbx" for="faretypln' + JSONdata.FareType + '_dynamic_1"></label>'
                                strBuildairCheck += '<label class="lbl fltcntnt" style="padding:0px 7px;" for="faretypln' + JSONdata.FareType + '_dynamic_1">' + fetchfaretype(JSONdata.FareType) + '</label>'
                                strBuildairCheck += '</div>'
                                strBuildairCheck += '</div>'
                                strBuildairCheck += '<div class="onlyBtn">Only</div>'
                                strBuildairCheck += '</div>'
                                $(".faretypfilterallcheck").append(strBuildairCheck);
                            } catch (e) {
                            }
                        }
                    });

                    $.unblockUI();
                    ShowHideNFare("flg1");
                    if ($("#hdn_producttype").val() == "RBOA") {
                        insertenqiry();
                    }

                    if (callrequestcnt == 0) {
                        var Resmessage = (json.Message != "") ? json.Message : "No Flights found.";
                        showerralert(Resmessage, "", "REDIRECT");
                    }
                    else {
                        $('#commonSorting').show();
                        $('#btnFmodifySearch').attr('disabled', false);//STS-166
                        $('.clsAvailFiltericon').css('pointer-events', 'all');//STS-166
                        $('body').data('BackFlag', 'AVAIL');
                        var segcnt = (strTrip == "O" || strTrip == "Y" || (strTrip == "M" && strSegTyp == "I")) ? 1 : strTrip == "R" ? 2 : aryOrgMul.length;
                        if (segcnt > 1) {
                            $('#FltfilterTab,#sepfilterTab').addClass('multrip');
                            if (strTrip == "M") {
                                aryorgcity = aryOrgMul;
                                arydescity = aryDstMul;
                            }
                            var activclass = "";
                            var s = "";
                            var colxs = strTrip == "R" ? "50" : parseInt(100 / aryorgcity.length);
                            for (var i = 0; i < aryorgcity.length ; i++) {
                                s = "";
                                sr = "";
                                activclass = i == 0 ? 'activ' : '';
                                s += '<div class="col10 ftabs ' + activclass + '" style="width:' + colxs + '%!important;" id="filterHead_dynamic_' + (i + 1) + '">';
                                s += '<div class="ftabs-title"><h6 class="spntit">(' + aryorgcity[i] + ' - ' + arydescity[i] + ') <span>' + (i + 1) + '</span></h6></div>';
                                s += '</div>';

                                sr += '<div class="col10 ftabs_mat ' + activclass + '" style="width:' + colxs + '%!important;" id="matrixHead_dynamic_' + (i + 1) + '">';
                                sr += '<div class="ftabs-title"><h6 class="spntit">(' + aryorgcity[i] + ' - ' + arydescity[i] + ') <span>' + (i + 1) + '</span></h6></div>';
                                sr += '</div>';

                                $('#FltfilterTab').append(s);
                                $('#sepfilterTab').append(s);
                                if (strTrip == "R") { //2nd set for Roundtrip...
                                    s = "";
                                    sr = "";
                                    s += '<div class="col10 ftabs" style="width:' + colxs + '%!important;" id="filterHead_dynamic_' + (i + 2) + '">';
                                    s += '<div class="ftabs-title"><h6 class="spntit">(' + arydescity[i] + ' - ' + aryorgcity[i] + ') <span>' + (i + 2) + '</span></h6></div>';
                                    s += '</div>';

                                    sr += '<div class="col10 ftabs_mat" style="width:' + colxs + '%!important;" id="matrixHead_dynamic_' + (i + 2) + '">';
                                    sr += '<div class="ftabs-title"><h6 class="spntit">(' + arydescity[i] + ' - ' + aryorgcity[i] + ') <span>' + (i + 2) + '</span></h6></div>';
                                    sr += '</div>';

                                    $('#FltfilterTab').append(s);
                                    $('#sepfilterTab').append(s);
                                }
                            }
                            for (var i = 2; i <= segcnt; i++) { // Minimum 1 Filter Content is there.... ( so segcnt-1 )
                                
                                var firstfilterdiv = $('#filterBody_dynamic_1').html();
                                firstfilterdiv = firstfilterdiv.replace(new RegExp("dynamic_1", "g"), "dynamic_" + i);
                                $('#divmainflilteration').append('<div id="filterBody_dynamic_' + i + '"  class="filter-body fltrbdyDynamic clearonsearch scrollfilter" style="display:none;">' + firstfilterdiv + '');  //animated fadeInDown  id="" 0512
                                //$('.matrixside_' + i + '').visible("hidden")//resultFilter

                            }
                        }
                        scrollLock = false;
                        if (firstapndclr_flg == true) { //to clear fisrt 10 pending availability sri 20180318
                            obj1 = {
                                count: 0,
                                setcount: 0
                            };
                            totalgridcount = (strTrip == "M" && strSegTyp == "D") ? $('#dvAddNewMRow').data('mul-rowcount') : result[6];

                            //airlinematrix_fun(groupmain_array, "1");
                            for (var i = 1; i <= totalgridcount; i++) {
                                if (strTrip == "O" || strTrip == "Y" || (strTrip == "M" && strSegTyp == "I")) {
                                    $("#availset_O_1").html('');
                                }
                                else {
                                    $("#availset_" + strTrip + "_" + i + "").html('');
                                }
                                var speratearr = $.grep(groupmain_array, function (value, index) {
                                    return value.bindside_6_arg == i;
                                });
                                var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                                    previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                                    previousValue[currentValue.flightid].push(currentValue);
                                    return previousValue;
                                }, {});
                                var grouparray = [];
                                for (var obj in groupedfli) {
                                    grouparray.push(groupedfli[obj]);
                                }

                                grouparray.sort(function (a, b) {
                                    if ($("#hdn_AvailFormat").val() == "NAT") {
                                        return a[0].NETFare - b[0].NETFare;
                                        //return value[0].NETFare >= Number(minval) && value[0].NETFare <= Number(maxval);
                                    }
                                    else { return a[0].GFare - b[0].GFare; }
                                });  //fare soring after array :srinath



                                $("#minfare_dynamic_" + i + "").data('min', ($("#hdn_AvailFormat").val() == "NAT" ? grouparray[0][0].NETFare : grouparray[0][0].GFare));
                                $("#maxfare_dynamic_" + i + "").data('max', ($("#hdn_AvailFormat").val() == "NAT" ? grouparray[grouparray.length - 1][0].NETFare : grouparray[grouparray.length - 1][0].GFare));

                                var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                                    return previousValue;
                                }, {});

                                var grouparray_orgdes = [];
                                for (var obj in groupedorgdes) {
                                    grouparray_orgdes.push(groupedorgdes[obj]);
                                }

                                if (strTrip == "O" || strTrip == "Y" || (strTrip == "M" && strSegTyp == "I")) {
                                    if (result[6] == "1") {
                                        if (assignedcountry == "EG") {
                                            $('#spnTotalFAvailCnt,#spnActlFAvailCnt').html(grouparray.length);
                                        }
                                        else {
                                            $('#spnTotalFAvailCnt,#spnActlFAvailCnt').html(grouparray.length);
                                            $('#spnActlFAvailCnt').html(grouparray_orgdes.length);
                                        }

                                    }
                                    else {
                                        var oldcnt = $('#spnTotalFAvailCnt_' + strTrip + '_' + i).html();

                                        if (assignedcountry == "EG") {
                                            $('#spnActlFAvailCnt_' + strTrip + '_' + i).html(grouparray.length);
                                            $('#spnTotalFAvailCnt_' + strTrip + '_' + i).html(grouparray.length);
                                        }
                                        else {
                                            $('#spnActlFAvailCnt_' + strTrip + '_' + i).html(grouparray_orgdes.length);
                                            $('#spnTotalFAvailCnt_' + strTrip + '_' + i).html(grouparray.length);
                                        }
                                    }
                                }
                                else {
                                    var oldcnt = $('#spnTotalFAvailCnt_' + strTrip + '_' + i).html();
                                    //$('#spnActlFAvailCnt_' + strTrip + '_' + i + ',#spnTotalFAvailCnt_' + strTrip + '_' + i).html(grouparray.length);
                                    if (assignedcountry == "EG") {
                                        $('#spnActlFAvailCnt_' + strTrip + '_' + i).html(grouparray.length);
                                        $('#spnTotalFAvailCnt_' + strTrip + '_' + i).html(grouparray.length);
                                    }
                                    else {
                                        $('#spnActlFAvailCnt_' + strTrip + '_' + i).html(grouparray_orgdes.length);
                                        $('#spnTotalFAvailCnt_' + strTrip + '_' + i).html(grouparray.length);
                                    }
                                }


                                grouparray_orgdes = grouparray_orgdes.length > 10 ? grouparray_orgdes.slice(0, 10) : grouparray_orgdes;
                                Commonavailability_bindingfun(grouparray_orgdes, null);
                            }

                        }

                        var strbaseTrip = (strTrip == "Y" || (strTrip == "M" && strSegTyp == "I")) ? "O" : strTrip;
                        for (var i = 1; i <= segcnt; i++) {

                            var stopfil = false;
                            var timefil = false;
                            var refufil = false;
                            $(".stopChk_all").each(function (n, i) {
                                if ($(".stopChk_all")[n].checked == true) {
                                    stopfil = true;
                                }

                            });
                            $(".detafil").each(function (n, i) {
                                if ($(".detafil")[n].checked == true) {
                                    timefil = true;
                                }

                            });
                            $(".Refundablefil").each(function (n, i) {
                                if ($(".Refundablefil")[n].checked == true) {
                                    refufil = true;
                                }

                            });

                            if (timefil != true && stopfil != true && refufil != true) { //advanced filteration check default sorting blocked
                                $('#sortFAvail_price_' + i + '').data('sortorder', 'asc'); //by default Assending order for sorting...
                                SortingFAvail('price_' + i + '', $('#sortFAvail_price_' + i + ''), 0);
                            }

                            if (strTrip == "O" || strTrip == "Y" || (strTrip == "M" && strSegTyp == "I")) {
                                if (stopfil == true) {
                                    FilterationFAvail("filterBody_dynamic_1", "0", "", "");
                                }
                                if (timefil == true) {
                                    FilterationFAvail("filterBody_dynamic_1", "0", "", "");
                                }
                                if (refufil == true) {
                                    FilterationFAvail("filterBody_dynamic_1", "0", "", "");
                                }
                            }
                            else {
                                if (stopfil == true) {
                                    FilterationFAvail("filterBody_dynamic_1", "0", "", "");

                                    FilterationFAvail("filterBody_dynamic_2", "0", "", "");
                                }
                                if (timefil == true) {
                                    FilterationFAvail("filterBody_dynamic_1", "0", "", "");
                                    FilterationFAvail("filterBody_dynamic_2", "0", "", "");
                                }
                                if (refufil == true) {
                                    FilterationFAvail("filterBody_dynamic_1", "0", "", "");
                                    FilterationFAvail("filterBody_dynamic_2", "0", "", "");
                                }

                            }

                            if ($("#availset_" + strbaseTrip + "_" + i + "").html().trim() == "") {
                                if (i == 2 && strTrip == "R") {
                                    $("#availset_" + strTrip + "_" + i + "").html("<p class='nofltfound'>No fight(s) found for <b>" + arydescity[0] + " - " + aryorgcity[0] + "</b></P");
                                }
                                else
                                    $("#availset_" + strTrip + "_" + i + "").html("<p class='nofltfound'>No fight(s) found for <b>" + aryorgcity[i - 1] + " - " + arydescity[i - 1] + "</b></P");
                            }
                        }
                        DeclareFareFilter(segcnt);
                        setTimeout(function () { // To feel Animation effect of more flights loading progress bar have to wait .5 seconds... (for last avail binding)... (Saranraj on 20170425); (commented on srinath 20170927)
                            $('#dvmoreAvailload').hide();
                        }, 500);

                        $("[data-toggle='tooltip']").tooltip();
                        $('.clsavail_dtls').tooltipster({
                            delay: 100,
                            maxWidth: 500,
                            speed: 300,
                            interactive: true,
                            animation: 'grow',
                            trigger: 'hover'
                        });

                        $('.FlightTip').tooltipster({
                            contentAsHTML: true,
                            theme: 'tooltipster-punk',
                            animation: 'grow',
                            position: 'right',
                        });
                    }
                }
                else {
                    /*Roundtrip Spl LCC airline*/

                    /*Roundtrip Onward details*/
                    $.map(groupmain_array, function (JsonObj, j) {
                        if (allAIRLINECode.indexOf(JsonObj.plt) == -1) {
                            try {
                                allAIRLINECode += JsonObj.plt + ",";
                                availairname = airlinename(JsonObj.plt != null ? JsonObj.plt : "").split("(")[0];
                                allAIRLINENAME += availairname + ",";
                                strBuildairCheck = "";
                                strBuildairCheck += '<div class="clonedCBox cBox">'
                                strBuildairCheck += '<div class="customCBox">'
                                strBuildairCheck += '<div class="cntr">'
                                strBuildairCheck += '<input style="display: none;" id="FAairln' + JsonObj.plt + '_dynamic_1" class="cb chkfilter chkfilterplane" data-filter="air' + JsonObj.plt + '" type="checkbox" />'
                                strBuildairCheck += '<label class="cbx" for="FAairln' + JsonObj.plt + '_dynamic_1"></label>'
                                strBuildairCheck += '<label class="lbl fltcntnt" for="FAairln' + JsonObj.plt + '_dynamic_1">' + availairname + '</label>'
                                strBuildairCheck += '</div>'
                                strBuildairCheck += '</div>'
                                strBuildairCheck += '<div class="onlyBtn">Only</div>'
                                strBuildairCheck += '</div>'
                                var listairlinenames = allAIRLINENAME.replace(",", "");
                                if (listairlinenames.trim() != "") {
                                    $(".airfilterallcheck").append(strBuildairCheck);
                                } else {
                                    $(".airfilterallcheck").append("Airline filtration is empty.");
                                }
                            } catch (e) {
                                //  alert(e.message);
                            }
                        }
                        if (Faretypefilter.indexOf(JsonObj.FareType) == -1) {
                            try {
                                Faretypefilter += JsonObj.FareType + ",";
                                strBuildairCheck = "";
                                strBuildairCheck += '<div class="clonedCBox cBox">'
                                strBuildairCheck += '<div class="customCBox">'
                                strBuildairCheck += '<div class="cntr">'
                                strBuildairCheck += '<input style="display: none;" id="faretypln' + JsonObj.FareType + '_dynamic_1" class="cb chkfilter chkfilterfaretype" data-filter="faretype' + JsonObj.FareType + '" type="checkbox" />'
                                strBuildairCheck += '<label class="cbx" for="faretypln' + JsonObj.FareType + '_dynamic_1"></label>'
                                strBuildairCheck += '<label class="lbl fltcntnt" style="padding:0px 7px;" for="faretypln' + JsonObj.FareType + '_dynamic_1">' + fetchfaretype(JsonObj.FareType) + '</label>'
                                strBuildairCheck += '</div>'
                                strBuildairCheck += '</div>'
                                strBuildairCheck += '<div class="onlyBtn">Only</div>'
                                strBuildairCheck += '</div>'
                                $(".faretypfilterallcheck").append(strBuildairCheck);
                            } catch (e) {
                            }
                        }
                    });

                    /*Roundtrip Onward details*/

                    /*Roundtrip Return details*/
                    $.map(groupmain_ret, function (JsonObj_Ret, j) {
                        if (allAIRLINECode.indexOf(JsonObj_Ret.plt) == -1) {
                            try {
                                allAIRLINECode += JsonObj_Ret.plt + ",";
                                availairname = airlinename(JsonObj_Ret.plt != null ? JsonObj_Ret.plt : "").split("(")[0];
                                allAIRLINENAME += availairname + ",";
                                strBuildairCheck = "";
                                strBuildairCheck += '<div class="clonedCBox cBox">'
                                strBuildairCheck += '<div class="customCBox">'
                                strBuildairCheck += '<div class="cntr">'
                                strBuildairCheck += '<input style="display: none;" id="FAairln' + JsonObj_Ret.plt + '_dynamic_1" class="cb chkfilter chkfilterplane" data-filter="air' + JsonObj_Ret.plt + '" type="checkbox" />'
                                strBuildairCheck += '<label class="cbx" for="FAairln' + JsonObj_Ret.plt + '_dynamic_1"></label>'
                                strBuildairCheck += '<label class="lbl fltcntnt" for="FAairln' + JsonObj_Ret.plt + '_dynamic_1">' + availairname + '</label>'
                                strBuildairCheck += '</div>'
                                strBuildairCheck += '</div>'
                                strBuildairCheck += '<div class="onlyBtn">Only</div>'
                                strBuildairCheck += '</div>'
                                var listairlinenames = allAIRLINENAME.replace(",", "");
                                if (listairlinenames.trim() != "") {
                                    $(".airfilterallcheck").append(strBuildairCheck);
                                } else {
                                    $(".airfilterallcheck").append("Airline filtration is empty.");
                                }
                            } catch (e) {
                                //  alert(e.message);
                            }
                        }
                        if (Faretypefilter.indexOf(JsonObj_Ret.FareType) == -1) {
                            try {
                                Faretypefilter += JsonObj_Ret.FareType + ",";
                                strBuildairCheck = "";
                                strBuildairCheck += '<div class="clonedCBox cBox">'
                                strBuildairCheck += '<div class="customCBox">'
                                strBuildairCheck += '<div class="cntr">'
                                strBuildairCheck += '<input style="display: none;" id="faretypln' + JsonObj_Ret.FareType + '_dynamic_1" class="cb chkfilter chkfilterfaretype" data-filter="faretype' + JsonObj_Ret.FareType + '" type="checkbox" />'
                                strBuildairCheck += '<label class="cbx" for="faretypln' + JsonObj_Ret.FareType + '_dynamic_1"></label>'
                                strBuildairCheck += '<label class="lbl fltcntnt" style="padding:0px 7px;" for="faretypln' + JsonObj_Ret.FareType + '_dynamic_1">' + fetchfaretype(JsonObj_Ret.FareType) + '</label>'
                                strBuildairCheck += '</div>'
                                strBuildairCheck += '</div>'
                                strBuildairCheck += '<div class="onlyBtn">Only</div>'
                                strBuildairCheck += '</div>'
                                $(".faretypfilterallcheck").append(strBuildairCheck);
                            } catch (e) {
                            }
                        }
                    });

                    /*Roundtrip Return details*/
                    $.unblockUI();
                    ShowHideNFare("flg1");
                    if ($("#hdn_producttype").val() == "RBOA") {
                        insertenqiry();
                    }

                    if (callrequestcnt == 0) {
                        var Resmessage = (json.Message != "") ? json.Message : "No Flights found.";
                        showerralert(Resmessage, "", "REDIRECT");
                    }
                    else {

                        $('#commonSorting').show();
                        $('#btnFmodifySearch').attr('disabled', false);//STS-166
                        $('.clsAvailFiltericon').css('pointer-events', 'all');//STS-166

                        var segcnt = 2;
                        if (segcnt > 1) {
                            $('#FltfilterTab,#sepfilterTab').addClass('multrip');
                            var activclass = "";
                            var s = "";
                            var colxs = aryorgcity.length == 1 ? "col-xs-6" : aryorgcity.length == 3 ? "col-xs-4" : aryorgcity.length == 4 ? "col-xs-3" : "col-xs-2"; //For Multicity Purpose...
                            for (var i = 0; i < aryorgcity.length ; i++) {
                                s = "";
                                sr = "";
                                activclass = i == 0 ? 'activ' : '';
                                s += '<div class="' + colxs + ' col10 ftabs ' + activclass + '" id="filterHead_dynamic_' + (i + 1) + '">';
                                s += '<div class="ftabs-title"><h6 class="spntit">(' + aryorgcity[i] + ' - ' + arydescity[i] + ') <span>' + (i + 1) + '</span></h6></div>';
                                s += '</div>';

                                sr += '<div class="' + colxs + ' col10 ftabs_mat ' + activclass + '" id="matrixHead_dynamic_' + (i + 1) + '">';
                                sr += '<div class="ftabs-title"><h6 class="spntit">(' + aryorgcity[i] + ' - ' + arydescity[i] + ') <span>' + (i + 1) + '</span></h6></div>';
                                sr += '</div>';

                                $('#FltfilterTab').append(s);
                                $('#sepfilterTab').append(s);
                                if (strTrip == "R") { //2nd set for Roundtrip...
                                    s = "";
                                    sr = "";
                                    s += '<div class="' + colxs + ' col10 ftabs" id="filterHead_dynamic_' + (i + 2) + '">';
                                    s += '<div class="ftabs-title"><h6 class="spntit">(' + arydescity[i] + ' - ' + aryorgcity[i] + ') <span>' + (i + 2) + '</span></h6></div>';
                                    s += '</div>';

                                    sr += '<div class="' + colxs + ' col10 ftabs_mat" id="matrixHead_dynamic_' + (i + 2) + '">';
                                    sr += '<div class="ftabs-title"><h6 class="spntit">(' + arydescity[i] + ' - ' + aryorgcity[i] + ') <span>' + (i + 2) + '</span></h6></div>';
                                    sr += '</div>';

                                    $('#FltfilterTab').append(s);
                                    $('#sepfilterTab').append(s);
                                }
                            }
                            for (var i = 2; i <= segcnt; i++) { // Minimum 1 Filter Content is there.... ( so segcnt-1 )
                                
                                var firstfilterdiv = $('#filterBody_dynamic_1').html();
                                firstfilterdiv = firstfilterdiv.replace(new RegExp("dynamic_1", "g"), "dynamic_" + i);
                                $('#divmainflilteration').append('<div id="filterBody_dynamic_' + i + '"  class="filter-body fltrbdyDynamic clearonsearch scrollfilter" style="display:none;">' + firstfilterdiv + '');  //animated fadeInDown  id="" 0512
                                //$('.matrixside_' + i + '').visible("hidden")//resultFilter

                            }
                        }
                        scrollLock = false;
                        if (firstapndclr_flg == true) { //to clear fisrt 10 pending availability sri 20180318
                            obj1 = {
                                count: 0,
                                setcount: 0
                            };
                            totalgridcount = result[6];

                            $("#availset_R_1").html('');
                            $("#availset_R_2").html('');

                            /* Onward RTspl*/
                            var speratearr = $.grep(groupmain_array, function (value, index) {
                                return value.bindside_6_arg == 1;
                            });
                            var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                                previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                                previousValue[currentValue.flightid].push(currentValue);
                                return previousValue;
                            }, {});
                            var grouparray = [];
                            for (var obj in groupedfli) {
                                grouparray.push(groupedfli[obj]);
                            }

                            grouparray.sort(function (a, b) {
                                if ($("#hdn_AvailFormat").val() == "NAT") {
                                    return a[0].NETFare - b[0].NETFare;
                                }
                                else {
                                    return a[0].GFare - b[0].GFare;
                                }
                            });  //fare soring after array :srinath 


                            var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                                previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                                previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                                return previousValue;
                            }, {});

                            $("#minfare_dynamic_1").data('min', ($("#hdn_AvailFormat").val() == "NAT" ? grouparray[0][0].NETFare : grouparray[0][0].GFare));
                            $("#maxfare_dynamic_1").data('max', ($("#hdn_AvailFormat").val() == "NAT" ? grouparray[grouparray.length - 1][0].NETFare : grouparray[grouparray.length - 1][0].GFare));

                            var grouparray_orgdes = [];
                            for (var obj in groupedorgdes) {
                                grouparray_orgdes.push(groupedorgdes[obj]);
                            }
                            var oldcnt = $('#spnTotalFAvailCnt_R_1').html();
                            $('#spnActlFAvailCnt_R_1').html(grouparray_orgdes.length);
                            $('#spnTotalFAvailCnt_R_1').html(grouparray.length);

                            grouparray_orgdes = grouparray_orgdes.length > 10 ? grouparray_orgdes.slice(0, 10) : grouparray_orgdes;
                            Commonavailability_bindingfun(grouparray_orgdes, "1");
                            /* Onward RTspl*/

                            /* Return RTspl*/

                            var speratearr_Ret = $.grep(groupmain_ret, function (value, index) {
                                return value.bindside_6_arg == 2;
                            });
                            var groupedfli_Ret = speratearr_Ret.reduce(function (previousValue, currentValue) {
                                previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                                previousValue[currentValue.flightid].push(currentValue);
                                return previousValue;
                            }, {});
                            var grouparray_Ret = [];
                            for (var obj in groupedfli_Ret) {
                                grouparray_Ret.push(groupedfli_Ret[obj]);
                            }

                            grouparray_Ret.sort(function (a, b) {
                                if ($("#hdn_AvailFormat").val() == "NAT") {
                                    return a[0].NETFare - b[0].NETFare;
                                }
                                else {
                                    return a[0].GFare - b[0].GFare;
                                }
                            });  //fare soring after array :srinath 



                            $("#minfare_dynamic_2").data('min', ($("#hdn_AvailFormat").val() == "NAT" ? grouparray_Ret[0][0].NETFare : grouparray_Ret[0][0].GFare));
                            $("#maxfare_dynamic_2").data('max', ($("#hdn_AvailFormat").val() == "NAT" ? grouparray_Ret[grouparray_Ret.length - 1][0].NETFare : grouparray_Ret[grouparray_Ret.length - 1][0].GFare));
                            //$("#minfare_dynamic_1").data('min', grouparray[0][0].GFare);
                            //$("#maxfare_dynamic_1").data('max', grouparray[grouparray.length - 1][0].GFare);

                            var groupedorgdes_ret = grouparray_Ret.reduce(function (previousValue, currentValue) {
                                previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                                previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                                return previousValue;
                            }, {});

                            var grouparray_orgdes_ret = [];
                            for (var obj in groupedorgdes_ret) {
                                grouparray_orgdes_ret.push(groupedorgdes_ret[obj]);
                            }

                            var oldcnt_ret = $('#spnTotalFAvailCnt_R_2').html();

                            $('#spnActlFAvailCnt_R_2').html(grouparray_orgdes_ret.length);
                            $('#spnTotalFAvailCnt_R_2').html(grouparray_Ret.length);

                            grouparray_orgdes_ret = grouparray_orgdes_ret.length > 10 ? grouparray_orgdes_ret.slice(0, 10) : grouparray_orgdes_ret;
                            Commonavailability_bindingfun(grouparray_orgdes_ret, "2");
                        }

                        for (var i = 1; i <= 2; i++) {

                            var stopfil = false;
                            var timefil = false;
                            var refufil = false;
                            $(".stopChk_all").each(function (n, i) {
                                if ($(".stopChk_all")[n].checked == true) {
                                    stopfil = true;
                                }

                            });
                            $(".detafil").each(function (n, i) {
                                if ($(".detafil")[n].checked == true) {
                                    timefil = true;
                                }

                            });
                            $(".Refundablefil").each(function (n, i) {
                                if ($(".Refundablefil")[n].checked == true) {
                                    refufil = true;
                                }

                            });

                            if (stopfil != true && timefil != true && refufil != true) {
                                $('#sortFAvail_price_' + i + '').data('sortorder', 'asc'); //by default Assending order for sorting...
                                SortingFAvail('price_' + i + '', $('#sortFAvail_price_' + i + ''), 0);
                            }
                            /*Advance config fitration*/


                            if (stopfil == true) {
                                FilterationFAvail("filterBody_dynamic_1", "0", "", "");
                                FilterationFAvail("filterBody_dynamic_2", "0", "", "");
                            }
                            if (timefil == true) {
                                FilterationFAvail("filterBody_dynamic_1", "0", "", "");
                                FilterationFAvail("filterBody_dynamic_2", "0", "", "");
                            }
                            if (refufil == true) {
                                FilterationFAvail("filterBody_dynamic_1", "0", "", "");
                                FilterationFAvail("filterBody_dynamic_2", "0", "", "");
                            }

                            /*Advance config fitration*/


                            if ($("#availset_R_" + i + "").html().trim() == "") {
                                if (i == 2 && strTrip == "R") {
                                    $("#availset_R_" + i + "").html("<p class='nofltfound'>No fight(s) found for <b>" + arydescity[0] + " - " + aryorgcity[0] + "</b></P");
                                }
                                else
                                    $("#availset_R_" + i + "").html("<p class='nofltfound'>No fight(s) found for <b>" + aryorgcity[i - 1] + " - " + arydescity[i - 1] + "</b></P");
                            }
                        }
                        DeclareFareFilter(2);
                        setTimeout(function () { // To feel Animation effect of more flights loading progress bar have to wait .5 seconds... (for last avail binding)... (Saranraj on 20170425); (commented on srinath 20170927)
                            $('#dvmoreAvailload').hide();
                        }, 500);

                        $("[data-toggle='tooltip']").tooltip();


                        $('.clsavail_dtls').tooltipster({
                            delay: 100,
                            maxWidth: 500,
                            speed: 300,
                            interactive: true,
                            animation: 'grow',
                            trigger: 'hover'
                        });

                        $('.FlightTip').tooltipster({
                            contentAsHTML: true,
                            theme: 'tooltipster-punk',
                            animation: 'grow',
                            position: 'right',
                        });


                    }
                    /*Roundtrip Spl LCC airline*/
                }

            }
        },
        error: function (e) {//On Successful service call
            $.unblockUI();
            //LogDetails(e.status, e.statusText, "Flight Search");
            $('#commonSorting').show();
            $('#btnFmodifySearch').attr('disabled', false);//STS-166
            $('.clsAvailFiltericon').css('pointer-events', 'all');//STS-166

            console.log("Status: " + e.statusCode + " --> Message: " + e.responseText);
            AvailResponseCount++;
            load_Increase += AvailRequestCount == AvailResponseCount ? 100 : 100 / AvailRequestCount;
            if (AvailRequestCount == AvailResponseCount)
                $('#dvmoreAvailload').hide();
            var a = navigator.userAgent;
            if (e.statusText == "timeout") {
                if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) {
                    if (document.getElementById('AvailmobResponse_ul').innerHTML != "" && document.getElementById('AvailmobResponse_ul').innerHTML != null) {
                    }
                    else {
                        showerralert("Operation timeout. Please Search Again.", "", "");
                    }
                }
                else {

                    if ((strTrip == "O" || strTrip == "Y") && document.getElementById("availset_O_1").innerHTML == "") {
                        $("#dvAvail").css("display", "none");
                        $("#InterNational_Avail").css("display", "none");
                        $("#dvSearch").css("display", "block");
                        $(".divdisable").remove();
                        $("#btn_Search").prop('disabled', false);
                        showerralert("Operation timeout. Please Search Again.", "", "");
                    }
                    else if ((strTrip == "R") && document.getElementById("availset_R_1").innerHTML == "") {
                        $("#dvAvail").css("display", "none");
                        $("#InterNational_Avail").css("display", "none");
                        $("#dvSearch").css("display", "block");
                        $(".divdisable").remove();
                        $("#btn_Search").prop('disabled', false);
                        showerralert("Operation timeout. Please Search Again.", "", "");
                    }
                    else if ((strTrip == "M" || strTrip == "MI" || strTrip == "MD") && document.getElementById("dvmultiCtyAvail").innerHTML == "") {
                        $("#dvAvail").css("display", "none");
                        $("#InterNational_Avail").css("display", "none");
                        $("#dvSearch").css("display", "block");
                        $(".divdisable").remove();
                        $("#btn_Search").prop('disabled', false);
                        showerralert("Operation timeout. Please Search Again.", "", "");
                    }
                }

            }
            else {
            }
            if (e.status == "500") {
            }

        }	// When Service call fails
    });

}

function GetInLcccheck(res) {
    var sTitle = "";
    var SChar = "";
    try {
        $.ajax({
            type: "GET",
            url: lccAirXML_path,
            async: false,
            dataType: "xml",
            success: function (xml) {
                $(xml).find('LCCAIRLINES').each(function () {
                    SChar = $(this).find('AIR_AIRLINE_CODE').text();
                    if (SChar == res) {
                        sTitle = true;
                    }
                    else {
                        SChar = "";
                    }
                });

                if (sTitle == "")
                    sTitle = false;

            },
            error: function (ex) {
                showerralert("An error occurred while processing XML file.", "", "");
            }
        });

    } catch (e) {
        showerralert(e.message.toString(), "", "");
    }
    return sTitle;
    //return false;
}

function Checksaberairline(Airline, Thread) {
    var segtype = $('body').data('segtype');

    var Saberairlines = (segtype == "D" ? $("#hdn1sdom").val() + "," + $("#hdn1gdom").val() : $("#hdn1sint").val() + "," + $("#hdn1gint").val());
    var checkairline = false;
    if (Airline != null && Airline != "") {
        for (var i = 0; i < Airline.split(",").length; i++) {
            if (Airline.split(",")[i] != "" && segtype == "D" && (($("#hdn1sdom").val().indexOf(Airline.split(",")[i]) > -1 && Thread.substr(0, 2) == "1S") || ($("#hdn1gdom").val().indexOf(Airline.split(",")[i]) > -1 && Thread.substr(0, 2) == "1G"))) {
                checkairline = true;
            }
            else if (Airline.split(",")[i] != "" && segtype == "I" && (($("#hdn1sint").val().indexOf(Airline.split(",")[i]) > -1 && Thread.substr(0, 2) == "1S") || ($("#hdn1gint").val().indexOf(Airline.split(",")[i]) > -1 && (Thread.substr(0, 2) == "1G" || Thread.substr(0, 2) == "SS")))) {
                checkairline = true;
            }
            else if (Airline.split(",")[i] != "" && (Thread.substr(0, 2) != "1S" && Thread.substr(0, 2) != "1G") && (Saberairlines.indexOf(Airline.split(",")[i]) < 0)) {
                checkairline = true;
            }
        }
    }
    return checkairline;
}

function BindSearchvaltoHeader() {
    var triptp = $("#hdtxt_trip")[0].value;
    $('#btn_Search').prop("disabled", true);
    var triptyeselection = $("body").data("tripflag") != null && $("body").data("tripflag").trim() == "O" ? "ONEWAY" : $("body").data("tripflag").trim() == "R" ? "ROUND TRIP" : $("body").data("tripflag").trim() == "Y" ? (segList.indexOf('R') == -1 ? "ROUND TRIP" : "ROUND TRIP") : "MULTI CITY";
    $('#spnmodifydettriptyp').html(triptyeselection);
    $("#hdtxt_trip").val($("body").data("tripflag").trim());

    if (FLAG == "M") {
        $('#spnmodifydetadt').html(adultcount != null && adultcount != "" ? adultcount : "0");
        $('#spnmodifydetchd').html(childcount != null && childcount != "" ? childcount : "0");
        $('#spnmodifydetinf').html(infantcount != null && infantcount != "" ? infantcount : "0");
        var sel_cabin = MobileClass == "E" ? "Economy" : MobileClass == "P" ? "Premium Economy" : MobileClass == "F" ? "First Class" : MobileClass == "B" ? "Business" : MobileClass;
        $('#dvmodifydetcls').html(sel_cabin);
    }
    else {
        $('#spnmodifydetadt').html($("#ddladult").val() != null && $("#ddladult").val().trim() != "" ? $("#ddladult").val().trim() : "0");
        $('#spnmodifydetchd').html($("#ddlchild").val() != null && $("#ddlchild").val().trim() != "" ? $("#ddlchild").val().trim() : "0");
        $('#spnmodifydetinf').html($("#ddlinfant").val() != null && $("#ddlinfant").val().trim() != "" ? $("#ddlinfant").val().trim() : "0");
        var sel_cabin = $("#ddlclass").val().trim() == "E" ? "Economy" : $("#ddlclass").val().trim() == "P" ? "Premium Economy" : $("#ddlclass").val().trim() == "F" ? "First Class" : $("#ddlclass").val().trim() == "B" ? "Business" : $("#ddlclass").val().trim();
        $('#dvmodifydetcls').html(sel_cabin);
    }

    if (triptp == "M") {
        $('#pmodifydetilsegment').html("");
        for (var i = 0; i < aryOrgMul.length; i++) {
            $('#pmodifydetilsegment').append(aryOrgMul[i]);
            $('#pmodifydetilsegment').append("<i class='fa fa-long-arrow-right'></i>");
        }
        $('#pmodifydetilsegment').append(aryDstMul[aryDstMul.length - 1]);
        if (FLAG == "M") {
            $('#spnmodifydetadt').html(adultcount != null && adultcount != "" ? adultcount : "0");
            $('#spnmodifydetchd').html(childcount != null && childcount != "" ? childcount : "0");
            $('#spnmodifydetinf').html(infantcount != null && infantcount != "" ? infantcount : "0");
            var sel_cabin = MobileClass == "E" ? "Economy" : MobileClass == "F" ? "First Class" : MobileClass == "B" ? "Business" : MobileClass == "P" ? "Premium Economy" : MobileClass;
            $('#dvmodifydetcls').html(sel_cabin);
        }
        else {
            $('#spnmodifydetadt').html($("#cmbMAdultFlight").val() != null && $("#cmbMAdultFlight").val().trim() != "" ? $("#cmbMAdultFlight").val().trim() : "0");
            $('#spnmodifydetchd').html($("#cmbMChildFlight").val() != null && $("#cmbMChildFlight").val().trim() != "" ? $("#cmbMChildFlight").val().trim() : "0");
            $('#spnmodifydetinf').html($("#cmbMInfantFlight").val() != null && $("#cmbMInfantFlight").val().trim() != "" ? $("#cmbMInfantFlight").val().trim() : "0");
            var strSelectedClass = $("input[name='flight-Mulclass']").length ? $("input[name='flight-Mulclass']:checked").val().trim() : "E";
            var sel_cabin = strSelectedClass == "E" ? "Economy" : strSelectedClass == "F" ? "First Class" : strSelectedClass == "B" ? "Business" : strSelectedClass == "P" ? "Premium Economy" : strSelectedClass;
            $('#dvmodifydetcls').html(sel_cabin);
        }
    }


    else {
        if (FLAG == "M") {
            $('#pmodifydetilsegment').html("<span>" + origincity + "</span>" + " to " + "<span>" + destinationcity + "</span>");
            $(".clsOnward").html(origincity)
            $(".clsReturn").html(destinationcity)
        }
        else {
            $('#pmodifydetilsegment').html("<span>" + $("#txtorigincity").val().trim() + "</span>" + " to " + "<span>" + $("#txtdestinationcity").val().trim() + "</span>");
            $(".clsOnward").html($("#txtorigincity").val().trim())
            $(".clsReturn").html($("#txtdestinationcity").val().trim())
        }
    }
    if (triptp == "M") {
        $('#spnmodifydetdep').html(aryDptMul[0]);
    }
    else {
        //   $('#spnmodifydetdep').html($("#txtdeparture").val().trim());
        $('#spnmodifydetdep').html(FLAG != "M" ? $("#txtdeparture").val().trim() : departuredate);//$('#spnmodifydetdep').html($("#txtdeparture").val().trim());

    }
    setTimeout(function () {

        if (triptp == "M") {
            $('#spnmodifydetRtn').html(aryDptMul[aryDptMul.length - 1]);
        }
        else {
            //var Arrivaldates = triptp == "R" || triptp == "Y" ? $("#txtarrivaldate").val().trim() : "-";
            var Arrivaldates = "";
            if (FLAG != "M") {
                Arrivaldates = triptp == "R" || triptp == "Y" ? $("#txtarrivaldate").val().trim() : "-";
            }
            else {
                Arrivaldates = triptp == "R" || triptp == "Y" ? arrivaldate : "-";
            }
            $('#spnmodifydetRtn').html(Arrivaldates);
        }
    }, 100);

    $('#dvmodifydetails').show();
}

var hit_cnt = 0;
var checklastvalue = [];
function loadClassFunction(Id, sementrow, arg, rowid, tripflag) {

    var idss = rowid;
    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == idss;
    });
    var idss_extra = rowid;
    var selectedObj_extr = $.grep(extrahidem_dataproperty_array, function (n, i) {
        return n.extrahide_datakey == idss;
    });


    var Invatt = selectedObj[0]["data-hdInva"];// $('#hdInva' + rowid).val();
    var Flightid = selectedObj_extr[0]["data-hdFlightid"]; // $('#hdFlightid' + rowid).val();
    var clsdropdownval = $('#clsduplicate_' + Id).val();
    var Defaultclas = $('#clsduplicate_' + Id)[0].innerHTML;
    if (clsdropdownval == "" || clsdropdownval == null) {
        clsdropdownval = Defaultclas;
    }
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });


    var idss_extra = Id;
    //var selectedObj_extr = $.grep(extrahidem_dataproperty_array, function (n, i) {
    //    return n.extrahide_datakey == idss_extra;
    //});

    var Invatt = selectedObj[0]["data-hdInva"];//$('#hdInva' + rowid).val();
    var availmulticlass = selectedObj_extr[0]["data-hdAvailmulticlass"]; //$('#hdAvailmulticlass' + rowid).val();
    var b_orgin = $("#hdtxt_origin")[0].value;
    var b_destin = $("#hdtxt_destination")[0].value;
    var trip_type = $('#hdtxt_trip')[0].value;
    var Class_val = ($("#hdtxt_trip")[0].value == "M" ? $("#grpcmbFlightClass").val() : $("#ddlclass").val());
    var ClientID = "";
    if ($("#ddlclient").length > 0) {
        ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
    }


    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: GetClassfare, 		// Location of the service
        data: '{FullFlag: "' + Invatt + '" ,Flightid: "' + Flightid + '",SClass: "' + Class_val + '",availmulti: "' + availmulticlass + '",Base_orgin:"' + b_orgin + '",Base_destin:"' + b_destin + '",Trip_type:"' + trip_type + '",Cabin:"' + clsdropdownval + '",sementrow:"' + sementrow + '",ClientID:"' + ClientID + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {
            var idval = 0;
            var idcntval = 0;
            var res = json.Result;
            if (json.Status == "-1") {
                window.location = sessionExb;
                return false;
            }
            else if (res[1] == "1") {
                var resultjson = res[3];
                var jsonresult = JSON.parse(resultjson);

                if (jsonresult.STU.RSC = "1" && jsonresult.STU.ERR == "" && jsonresult.ACS.length > 0 && tripflag != 100) {
                    var tbl_id = document.getElementsByClassName("conting_" + arg)[0].id;
                    var tblrowslen = jsonresult["ACS"].length;
                    for (var i = 0; i < tblrowslen ; i++) {
                        var substringval = "";
                        idcntval = Id + idval;
                        if (idval == 0) {
                            $('#clsduplicate_' + Id).css("display", "none");
                            $('#concls' + Id).css("display", "block");
                            checklastvalue[i] = '#concls' + i;
                            if (clsdropdownval != null && clsdropdownval != "") {
                                substringval += "<option value='" + clsdropdownval.split('-')[0].trim() + "'>" + clsdropdownval + "</option>";
                            }
                            if ($('#hdnsAllowmulticlass').val() == "Y") {
                                for (var j = 0; j < jsonresult.ACS[i].CLS.length; j++) {
                                    substringval += "<option value='" + jsonresult.ACS[i].CLS[j].FBC + "'>" + jsonresult.ACS[i].CLS[j].CLA + "-" + jsonresult.ACS[i].CLS[j].FBC + "</option>";
                                }
                            }
                            $('#concls' + Id).val("");
                            $('#concls' + Id).text("");
                            $('#concls' + Id).append(substringval);
                        } else {
                            $('#clsduplicate_' + idcntval).css("display", "none");
                            $('#concls' + idcntval).css("display", "block");
                            checklastvalue[i] = '#concls' + i;
                            if (clsdropdownval != null && clsdropdownval != "") {
                                substringval += "<option value='" + clsdropdownval.split('-')[0].trim() + "'>" + clsdropdownval + "</option>";
                            }
                            if ($('#hdnsAllowmulticlass').val() == "Y") {
                                for (var j = 0; j < jsonresult.ACS[i].CLS.length; j++) {
                                    substringval += "<option value='" + jsonresult.ACS[i].CLS[j].FBC + "'>" + jsonresult.ACS[i].CLS[j].CLA + "-" + jsonresult.ACS[i].CLS[j].FBC + "</option>";
                                }
                            }
                            $('#concls' + idcntval).val("");
                            $('#concls' + idcntval).text("");
                            $('#concls' + idcntval).append(substringval);

                        }
                        hit_cnt++;
                        idval++;
                    }
                    $.unblockUI();
                }
                else if (tripflag == 100) {
                    var substringval = "";
                    if (idval == 0) {
                        $('#clsduplicate_' + Id).css("display", "none");
                        $('#fareClass' + Id).css("display", "inline-block");
                        checklastvalue[i] = '#fareClass' + i;
                        if (clsdropdownval == "" || clsdropdownval == null) {
                            clsdropdownval = Defaultclas;
                        }
                        if (clsdropdownval != null && clsdropdownval != "") {
                            substringval += "<option value='" + clsdropdownval.split('-')[0].trim() + "'>" + clsdropdownval + "</option>";
                        }
                        if ($('#hdnsAllowmulticlass').val() == "Y") {
                            for (var j = 0; j < jsonresult.ACS[0].CLS.length; j++) {
                                substringval += "<option value='" + jsonresult.ACS[0].CLS[j].FBC + "'>" + jsonresult.ACS[0].CLS[j].CLA + "-" + jsonresult.ACS[0].CLS[j].FBC + "</option>";
                            }
                        }
                        $('#fareClass' + Id).val("");
                        $('#fareClass' + Id).text("");
                        $('#fareClass' + Id).append(substringval);
                    }
                }
                $.unblockUI();
            }
            else {
                if (res[0] != "") {
                    showerralert(res[0], "", "");
                    $.unblockUI();
                    return false;
                }
                else {
                    showerralert("No Class Available.", "", "");
                    $.unblockUI();
                    return false;
                }
            }
            $.unblockUI();
        },
        error: function (error) {
            //LogDetails(error.responseText, error.status, "Multiclass Load");
            if (error.status == "500") {
                window.location = sessionExb;
                return false;
            }
            $.unblockUI();
        }
    });
}

function Changemulticlasforconn(arg, rowid, id, multcls) {

    var trip_type = $('#hdtxt_trip')[0].value;
    var Cabin_value_load = "";
    var getlocaldata = 0;
    if (multcls == "1") {
        var tbl_id = document.getElementsByClassName("conting_" + arg)[0].id;
        //////var tblrowslen = $('.' + tbl_id + ' article.box');//.length; //document.getElementById(tbl_id).rows.length;
        if (multcls == 1) {
            $('#' + tbl_id + ' article.box').each(function (i, obj) {
                $(obj).find('.amenities .vcenter.mob-conn-bagg span').html('-');
                var valuecls = $(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').length ? $(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').val() : $(this).find('.mob-conn-flt .mob-conn-mclass a').length ? $(this).find('.mob-conn-flt .mob-conn-mclass a')[0].innerHTML : "";  //$(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').val();  //////document.getElementById(tbl_id).rows[i].cells[5].children[0].value;
                Cabin_value_load += valuecls + "SpLITSaTIS";
            });
        } else {

            $('#' + tbl_id + ' article.box').each(function (i, obj) {
                $(obj).find('.amenities .vcenter.mob-conn-bagg span').html('-');
                var valuecls = $(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').length ? $(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').val() : $(this).find('.mob-conn-flt .mob-conn-mclass a').length ? $(this).find('.mob-conn-flt .mob-conn-mclass a')[0].innerHTML : "";  //$(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').val();  /////document.getElementById(tbl_id).rows[i].cells[5].children[0].value;
                Cabin_value_load += valuecls + "SpLITSaTIS";
            });
        }
        if (multiclasconarr != null && multiclasconarr != "") {
            for (var i = 0; i < multiclasconarr.length; i++) {
                var getid = multiclasconarr[i].split("SPLITRAV")[0];
                var getclas = multiclasconarr[i].split("SPLITRAV")[1];
                var result = multiclasconarr[i].split("SPLITRAV")[2];
                var RBD = multiclasconarr[i].split("SplitClas")[1];
                if (rowid == getid && getclas == Cabin_value_load && getlocaldata == 0) {
                    var Clasdata = result.split("SPLITINVA")[0];
                    var resultdata = result.split("SPLITINVA")[1];
                    var oldresult = result.split("SPLITINVA")[2].split("SplitClas")[0];
                    updatelocalsetmutliclasvalue(Clasdata, resultdata, rowid, oldresult);
                    getlocaldata++;
                }
            }
        }
        else {
            $('#btn_multiclas_' + rowid).css("display", "none");
            $('#selectclickbutton' + rowid).css("display", "none");
            $('#btn_multiclasforconn_' + rowid).css("display", "block");
            $("#BaseFARE" + rowid).html('-');
            $("#GorssFareSpan2" + rowid).html('-');
            $("#showbagg" + rowid).html('-');
            // $('.conbaggageclas_' + rowid)[0].innerHTML = '-';
            if (trip_type == "R") {
                $('#btn_multiclasforconn_' + rowid).siblings('.roundTripFareRadio').hide();//radiobutton hide in Roundtrip while multiclass change... by saranraj...
                $('#btn_multiclasforconn_' + rowid).addClass('rtripgfar');
                $('#btn_multiclasforconn_' + rowid).parents('.farearea').children('.mob-mk-trc').hide(); //Checkbox hide in Roundtrip while multiclass change... by saranraj...
            }
        }
        if (getlocaldata == 0) {
            $('#btn_multiclas_' + rowid).css("display", "none");
            $('#selectclickbutton' + rowid).css("display", "none");
            $('#btn_multiclasforconn_' + rowid).css("display", "block");
            $("#BaseFARE" + rowid).html('-');
            $("#GorssFareSpan2" + rowid).html('-');
            $("#showbagg" + rowid).html('-');
            //$('.conbaggageclas_' + rowid)[0].innerHTML = '-';

            if (trip_type == "R") {
                $('#btn_multiclasforconn_' + rowid).siblings('.roundTripFareRadio').hide();//radiobutton hide in Roundtrip while multiclass change... by saranraj...
                $('#btn_multiclasforconn_' + rowid).addClass('rtripgfar');
                $('#btn_multiclasforconn_' + rowid).parents('.farearea').children('.mob-mk-trc').hide(); //Checkbox hide in Roundtrip while multiclass change... by saranraj...
            }
        }

    }
    else if (multcls == "2") {

        var tbl_id = document.getElementsByClassName("conting_" + arg)[0].id;
        var tblrowslen = $('.' + tbl_id + ' article.box'); /////document.getElementById(tbl_id).rows.length;
        if (multcls == 1) {
            $('#' + tbl_id + ' article.box').each(function (i, obj) {
                $(obj).find('.amenities .vcenter.mob-conn-bagg span').html('-');
                var valuecls = $(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').length ? $(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').val() : $(this).find('.mob-conn-flt .mob-conn-mclass a').length ? $(this).find('.mob-conn-flt .mob-conn-mclass a')[0].innerHTML : "";  //$(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').val();  //////document.getElementById(tbl_id).rows[i].cells[5].children[0].value;
                Cabin_value_load += valuecls + "SpLITSaTIS";
            });
        } else {
            $('#' + tbl_id + ' article.box').each(function (i, obj) {
                $(this).find('.amenities .vcenter.mob-conn-bagg span').html('-');
                var valuecls = $(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').length ? $(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').val() : $(this).find('.mob-conn-flt .mob-conn-mclass a').length ? $(this).find('.mob-conn-flt .mob-conn-mclass a')[0].innerHTML : "";  //$(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').val();  //////document.getElementById(tbl_id).rows[i].cells[5].children[0].value;
                Cabin_value_load += valuecls + "SpLITSaTIS";
            });
        }
        if (multiclasconarr != null && multiclasconarr != "") {
            for (var i = 0; i < multiclasconarr.length; i++) {
                var getid = multiclasconarr[i].split("SPLITRAV")[0];
                var getclas = multiclasconarr[i].split("SPLITRAV")[1];
                var result = multiclasconarr[i].split("SPLITRAV")[2];
                if (rowid == getid && getclas == Cabin_value_load && getlocaldata == 0) {
                    var Clasdata = result.split("SPLITINVA")[0];
                    var resultdata = result.split("SPLITINVA")[1];
                    var oldresult = result.split("SPLITINVA")[2].split("SplitClas")[0];
                    updatelocalsetmutliclasvalue(Clasdata, resultdata, rowid, oldresult);
                    getlocaldata++;
                }
            }
        }
        else {
            $('#btn_multiclas_' + rowid).css("display", "none");
            $('#selectclickbutton' + rowid).css("display", "none");
            $('#btn_multiclasforconn_' + rowid).css("display", "block");
            $("#BaseFARE" + rowid).html('-');
            $("#GorssFareSpan2" + rowid).html('-');
            $("#showbagg" + rowid).html('-');
            // $('.conbaggageclas_' + rowid)[0].innerHTML = '-';

            if (trip_type == "R") {
                $('#btn_multiclasforconn_' + rowid).siblings('.roundTripFareRadio').hide();//radiobutton hide in Roundtrip while multiclass change... by saranraj...
                $('#btn_multiclasforconn_' + rowid).addClass('rtripgfar');
                $('#btn_multiclasforconn_' + rowid).parents('.farearea').children('.mob-mk-trc').hide(); //Checkbox hide in Roundtrip while multiclass change... by saranraj...
            }
        }
        if (getlocaldata == 0) {
            $('#btn_multiclas_' + rowid).css("display", "none");
            $('#selectclickbutton' + rowid).css("display", "none");
            $('#btn_multiclasforconn_' + rowid).css("display", "block");
            $("#BaseFARE" + rowid).html('-');
            $("#GorssFareSpan2" + rowid).html('-');
            $("#showbagg" + rowid).html('-');
            // $('.conbaggageclas_' + rowid)[0].innerHTML = '-';
            if (trip_type == "R") {
                $('#btn_multiclasforconn_' + rowid).siblings('.roundTripFareRadio').hide();//radiobutton hide in Roundtrip while multiclass change... by saranraj...
                $('#btn_multiclasforconn_' + rowid).addClass('rtripgfar');
                $('#btn_multiclasforconn_' + rowid).parents('.farearea').children('.mob-mk-trc').hide(); //Checkbox hide in Roundtrip while multiclass change... by saranraj...
            }
        }
    }
}

function getmulticlasfareforcon(arg, rowid, multcls) {//Id, sementrow,
    var Cabin_value = "";
    var idss = rowid;
    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == idss;
    });
    var idss_extra = idss;
    var selectedObj_extr = $.grep(extrahidem_dataproperty_array, function (n, i) {
        return n.extrahide_datakey == idss_extra;
    });

    var Invatt = selectedObj[0]["data-hdInva"];//  $('.Travselect_' + rowid)[0].dataset.hdinva; //$('#hdInva' + rowid).val();
    var availmulticlass = selectedObj_extr[0]["data-hdAvailmulticlass"];// $('.extrahidem_' + rowid)[0].dataset.hdAvailmulticlass; // $('#hdAvailmulticlass' + rowid).val();
    var tbl_id = document.getElementsByClassName("conting_" + arg)[0].id;
    var Flightstock = selectedObj_extr[0]["data-hdflightstk"];//  $('.extrahidem_' + rowid)[0].dataset.hdflightstk; // $('#hdflightstk' + rowid)[0].value;
    //////var tblrowslen = document.getElementById(tbl_id).rows.length;
    if (multcls == 1) {
        $('#' + tbl_id + ' article.box').each(function (i, obj) {
            var valuecls = $(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').length ? $(this).find('.mob-conn-flt .mob-conn-mclass select.MUlticlasselect').val() : $(this).find('.mob-conn-flt .mob-conn-mclass a').length ? $(this).find('.mob-conn-flt .mob-conn-mclass a')[0].innerHTML : "";  //////document.getElementById(tbl_id).rows[i].cells[5].children[0].value;
            Cabin_value += valuecls + "SpLITSaTIS";
        });
    } else {
        if ($('#' + tbl_id + ' article.box').find("select.aftr").length > 0) {
            $('#' + tbl_id + ' article.box').find("select.aftr").each(function () {
                Cabin_value += $(this).val() + "SpLITSaTIS";
            });
        }
        else if ($(this).find('.mob-conn-flt .mob-conn-mclass a').length) {
            $('#' + tbl_id + ' article.box').each(function (i, obj) {
                var valuecls = $(this).find('.mob-conn-flt .mob-conn-mclass a').length ? $(this).find('.mob-conn-flt .mob-conn-mclass a')[0].innerHTML : ""; //////document.getElementById(tbl_id).rows[i].cells[5].children[0].value;
                Cabin_value += valuecls + "SpLITSaTIS";
            });
        }
        //$('#' + tbl_id + ' article.box').each(function (i, obj) {
        //    var valuecls = $(this).find('.mob-conn-flt .mob-conn-mclass select.multiclassddl').length ? $(this).find('.mob-conn-flt .mob-conn-mclass select.multiclassddl').val() : $(this).find('.mob-conn-flt .mob-conn-mclass a').length ? $(this).find('.mob-conn-flt .mob-conn-mclass a')[0].innerHTML : ""; //////document.getElementById(tbl_id).rows[i].cells[5].children[0].value;
        //    Cabin_value += valuecls + "SpLITSaTIS";
        //});


    }


    var b_orgin = $("#hdtxt_origin")[0].value;
    var b_destin = $("#hdtxt_destination")[0].value;
    var trip_type = $('#hdtxt_trip')[0].value;

    //var faredetails = $('#span_depature' + rowid)[0].innerHTML + "TR" + $('#span_Arrival' + rowid)[0].innerHTML + "TR" + $('#hdtoken' + rowid)[0].value + "TR" + $('#hdInva' + rowid)[0].value;

    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });
    var idss = rowid;
    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == idss;
    });
    var idss_extra = rowid;
    var selectedObj_extr = $.grep(extrahidem_dataproperty_array, function (n, i) {
        return n.extrahide_datakey == idss_extra;
    });

    var Invatt = selectedObj[0]["data-hdInva"];// $('.Travselect_' + rowid)[0].dataset.hdinva;// $('#hdInva' + rowid).val();
    var Flightid = selectedObj_extr[0]["data-hdFlightid"];//  $('.extrahidem_' + rowid)[0].dataset.hdFlightid; // $('#hdFlightid' + rowid).val();
    //var cabin = $('#fareClass' + Id)[0].value;

    var Class_val = ($("#hdtxt_trip")[0].value == "M" ? $("#grpcmbFlightClass").val() : $("#ddlclass").val());

    var ClientID = "";
    if ($("#ddlclient").length > 0) {
        ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
    }

    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: FormmulticlasssRequest, 		// Location of the service
        data: '{FullFlag: "' + Invatt + '" ,Flightid: "' + Flightid + '",SClass: "' + Class_val + '",availmulti: "' + availmulticlass + '",Base_orgin:"' + b_orgin + '",Base_destin:"' + b_destin + '",Trip_type:"' + trip_type + '",Cabin:"' + Cabin_value + '",Flightstock:"' + Flightstock + '",ClientID:"' + ClientID + '"}',// ,sementrow:"' + sementrow + '",Arrive: "' + Arrive + '" ,FullFlag: "' + Invatt + '" ,TokenKey: "' + tokenkey + '",Trip: "' + $("#hdtxt_trip")[0].value + '",BaseOrgin: "' + $("#hdtxt_origin")[0].value + '",BaseDestination: "' + $("#hdtxt_destination")[0].value + '",offflg: "' + offflg + '",Class: "' + $("#ddlclass").val() + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call  hdAvailmulticlass
            $.unblockUI();
            if (json.Status == "-1") {
                window.location = sessionExb;
                return false;
            }
            var result = json.Result;
            if (result[2] != "" && result[3] != "") {
                setmutliclasvalue(result[2], result[3], rowid, "CON", Cabin_value, Invatt);
            }
            else if (json.Result[0] != "") {
                showerralert(json.Result[0], "", "");
            }
        },
        error: function (e) {

            $.unblockUI();
            // LogDetails(e.responseText, e.status, "Multiclass Select");
            if (e.status == "500") {
                window.location = sessionExb;
                return false;
            }
            else {
                showerralert("Problem occured while processing (#09).", "", "");
            }
        }

    });
}

function Changemulticlas(rowid, id, multcls) {
    var getlocaldata = 0;
    if (multcls == "1") {
        var value = $('#fareClass' + rowid)[0].value;
        if (multiclasarr != null && multiclasarr != "") {
            for (var i = 0; i < multiclasarr.length; i++) {
                var getid = multiclasarr[i].split("SPLITRAV")[0];
                var getclas = multiclasarr[i].split("SPLITRAV")[1];
                var result = multiclasarr[i].split("SPLITRAV")[2];
                var RBD = multiclasarr[i].split("SplitClas")[1];
                if (rowid == getid && getclas == value.split("-")[0] && getlocaldata == 0 && value.split("-")[1] == RBD) {
                    var Clasdata = result.split("SPLITINVA")[0];
                    var resultdata = result.split("SPLITINVA")[1];
                    //var resultdata = result.split("SPLITINVA")[2].split("SplitClas")[0];
                    var oldresult = result.split("SPLITINVA")[2].split("SplitClas")[0];
                    updatelocalsetmutliclasvalue(Clasdata, resultdata, rowid, oldresult);
                    getlocaldata++;
                }
            }
        }
        else {
            $("#BaseFARE" + rowid).html('-');
            $("#GorssFareSpan2" + rowid).html('-');
            $("#showbagg" + rowid).html('-');
            getlocaldata++;
            getmulticlasfare(rowid, id);

        }
        if (getlocaldata == 0) {
            $("#BaseFARE" + rowid).html('-');
            $("#GorssFareSpan2" + rowid).html('-');
            $("#showbagg" + rowid).html('-');
            getlocaldata++;
            getmulticlasfare(rowid, id);
        }

    }
    else if (multcls == "2") {

        var value = $('#fareClass' + id)[0].value;

        if (multiclasarr != null && multiclasarr != "") {
            for (var i = 0; i < multiclasarr.length; i++) {
                var getid = multiclasarr[i].split("SPLITRAV")[0];
                var getclas = multiclasarr[i].split("SPLITRAV")[1];
                var result = multiclasarr[i].split("SPLITRAV")[2];
                if (rowid == getid && getclas == value && getlocaldata == 0) {
                    var Clasdata = result.split("SPLITINVA")[0];
                    var resultdata = result.split("SPLITINVA")[1];
                    //var resultdata = result.split("SPLITINVA")[2].split("SplitClas")[0];
                    var oldresult = result.split("SPLITINVA")[2].split("SplitClas")[0];
                    updatelocalsetmutliclasvalue(Clasdata, resultdata, rowid, oldresult);
                    getlocaldata++;
                }
            }
        }
        else {
            $("#BaseFARE" + rowid).html('-');
            $("#GorssFareSpan2" + rowid).html('-');
            $("#showbagg" + rowid).html('-');
            getlocaldata++;
            getmulticlasfare(rowid, id);
        }
        if (getlocaldata == 0) {
            $("#BaseFARE" + rowid).html('-');
            $("#GorssFareSpan2" + rowid).html('-');
            $("#showbagg" + rowid).html('-');
            getlocaldata++;
            getmulticlasfare(rowid, id);
        }
    }
}

function getmulticlasfare(Id, arg_LLP) {

    var idss = Id;
    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == Id;
    });
    var idss_extra = Id;
    var selectedObj_extr = $.grep(extrahidem_dataproperty_array, function (n, i) {
        return n.extrahide_datakey == idss_extra;
    });

    var segmenttype = $('body').data('segtype');
    var Invatt = selectedObj[0]["data-hdInva"]; // $('.Travselect_' + Id)[0].dataset.hdinva; // $('#hdInva' + Id).val();
    var availmulticlass = selectedObj_extr[0]["data-hdAvailmulticlass"];// selectedObj[0]["data-hdAvailmulticlass"]; // $('.extrahidem_' + Id)[0].dataset.hdAvailmulticlass; // $('#hdAvailmulticlass' + Id).val();
    var b_orgin = $("#hdtxt_origin")[0].value;
    var b_destin = $("#hdtxt_destination")[0].value;
    var trip_type = $('#hdtxt_trip')[0].value;
    var Flightstock = selectedObj_extr[0]["data-hdflightstk"];// selectedObj[0]["data-hdflightstk"];//  $('.extrahidem_' + Id)[0].dataset.hdflightstk;// $('#hdflightstk' + Id)[0].value;
    var cabin = "";
    if (($('#hdnsAllowmulticlass').val() == "Y" && $('#hdMulticlassTransFare' + Id)[0].length == "0" && $('#fareClass' + Id).length <= 1) && (selectedObj_extr[0]["data-hdmultiacess"] == "1" || selectedObj_extr[0]["data-hdmultiacess"] == "2") && (selectedObj_extr[0]["data-hdconnFlightid"] != "S")) {
        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });

        var Invatt = selectedObj[0]["data-hdInva"];//  $('.Travselect_' + Id)[0].dataset.hdinva; // $('#hdInva' + Id).val();
        var Flightid = selectedObj_extr[0]["data-hdFlightid"];// $('.extrahidem_' + Id)[0].dataset.hdFlightid;// $('#hdFlightid' + Id).val();
        if (selectedObj_extr[0]["data-hdmultiacess"] == "1") {
            cabin = $('#fareClass' + Id)[0].value;
        }
        else {
            cabin = $('#fareClass' + arg_LLP)[0].value;
        }
        var sementrow = "";
        var faredetails = $('#span_depature' + Id)[0].innerHTML + "TR" + $('#span_Arrival' + Id)[0].innerHTML + "TR" + selectedObj[0]["data-hdtoken"] + "TR" + selectedObj[0]["data-hdInva"]; //$('#hdInva' + Id)[0].value;

        var Class_val = ($("#hdtxt_trip")[0].value == "M" ? $("#grpcmbFlightClass").val() : $("#ddlclass").val());

        var ClientID = "";
        if ($("#ddlclient").length > 0) {
            ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
        }

        $.ajax({
            type: "POST", 		//GET or POST or PUT or DELETE verb
            url: FormmulticlasssRequest, 		// Location of the service
            data: '{FullFlag: "' + Invatt + '" ,Flightid: "' + Flightid + '",SClass: "' + Class_val + '",availmulti: "' + availmulticlass + '",Base_orgin:"' + b_orgin + '",Base_destin:"' + b_destin + '",Trip_type:"' + trip_type + '",Cabin:"' + cabin + '",Flightstock:"' + Flightstock + '",ClientID:"' + ClientID + '",Segmenttype:"' + segmenttype + '"}',//,sementrow:"' + sementrow + '" ,Arrive: "' + Arrive + '" ,FullFlag: "' + Invatt + '" ,TokenKey: "' + tokenkey + '",Trip: "' + $("#hdtxt_trip")[0].value + '",BaseOrgin: "' + $("#hdtxt_origin")[0].value + '",BaseDestination: "' + $("#hdtxt_destination")[0].value + '",offflg: "' + offflg + '",Class: "' + $("#ddlclass").val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (json) {//On Successful service call  hdAvailmulticlass
                $.unblockUI();
                var result = json.Result;
                if (result[2] != "" && result[3] != "") {
                    setmutliclasvalue(result[2], result[3], Id, "DIR", cabin, Invatt);
                }
                else if (json.Result[0] != "") {
                    showerralert(json.Result[0], "", "");
                }
            },
            error: function (e) {
                $.unblockUI();
                //LogDetails(e.responseText, e.status, "Multiclass Select");
                if (e.status == "500") {
                    window.location = sessionExb;
                    return false;
                }
                else {
                    showerralert("Problem occured while processing (#09).", "", "");
                }

            }
        });
    }
}

function GetMultiClassrow(id) {
    var firstclas = $('#Aspan_Class' + id)[0].text;
    var cabin = $('#Aspan_Class' + id).attr('data-val');
    var selectcabin = firstclas + "," + cabin;
    var select1 = $('#fareClass' + id);
    var mm = selectcabin.split(',')
    if (mm.length > 0) {
        document.getElementById('Aspan_Class' + id).style.display = "none";
        select1.empty();
        for (var j = 0; j < mm.length; j++) {
            if (mm[j] != "") {//|| mm[j] != null) {
                var Im = mm[j].toString();//.split('/')
                var opt = document.createElement('option');
                opt.value = Im;
                opt.text = Im;
                select1.append(opt);

            }
        }
        $('#fareClass' + id).css("display", "block");
        $('#btn_multiclas_' + id).css("display", "block");//getmulticlasfare
    }
}

function setmutliclasvalue(result, resultinva, Id, segtype, cabin, Invatt) {

    var idss = Id;
    var selectedObj_extr = $.grep(extrahidem_dataproperty_array, function (n, i) {
        return n.extrahide_datakey == idss;
    });

    var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;
    var trip_type = $('#hdtxt_trip')[0].value;
    $('#btn_multiclas_' + Id).css("display", "none");
    $('#btn_multiclasforconn_' + Id).css("display", "none");
    $('#selectclickbutton' + Id).css("display", "inline-block");
    if (trip_type == "R") {
        $('#btn_multiclasforconn_' + Id).siblings('.roundTripFareRadio').show();//radiobutton show in Roundtrip while multiclass change... by saranraj...
        $('#btn_multiclasforconn_' + Id).removeClass('rtripgfar');
        $('#btn_multiclasforconn_' + Id).parents('.farearea').children('.mob-mk-trc').show();  //Checkbox show in Roundtrip while multiclass change... by saranraj...
    }
    var STax_build = "";
    var results = result.split("SpLITSaTIS");
    var resulstinva = resultinva.split("SpLITSaTIS");
    var resoldinva = Invatt.split("SpLITSaTIS");
    var Multiclassinva = "";
    var hdninva = "";
    var MulticlassToken = "";
    var multitoken = "";
    var mclsbagg = "";
    var mbenifit = "";

    if (results.length > 1) {
        var MulticlassBaseCommFare = 0;
        for (var i = 0; i < results.length - 1; i++) {
            // $('#hdMulticlassinva' + Id).val("");

            selectedObj_extr[0]["data-hdMulticlassinva"] = "";
            //            $('.extrahidem_' + Id)[0].dataset.hdMulticlassinva = "";
            // document.getElementById('Aspan_Class' + Id).style.display = "none";
            var MulticlassFare = result.split("SpLitPResna")[6];
            var MulticlassTransFare = results[i].split("SpLitPResna")[7];
            var MulticlassBaseFare = results[i].split("SpLitPResna")[4];
            MulticlassBaseCommFare = Number(results[i].split("SpLitPResna")[14]);
            var MulticlassDiscount = results[i].split("SpLitPResna")[15];
            var Multiclassgrossfare = results[i].split("SpLitPResna")[6];
            var Multiclassgrosswithmark = results[i].split("SpLitPResna")[7];
            MulticlassToken = results[i].split("SpLitPResna")[3];
            var Multiclassdetail = results[i].split("SpLitPResna")[2];
            var MulticlassTaxBreak = results[i].split("SpLitPResna")[17];
            var Multiclassmarkup = results[i].split("SpLitPResna")[9];
            var Multiclassservicecharge = results[i].split("SpLitPResna")[12];
            var multiclassbaggchange = results[i].split("SpLitPResna")[18];
            var Multiclasfarebasecode = results[i].split("SpLitPResna")[19];
            var MulticlasIncentive = results[i].split("SpLitPResna")[16];
            var Mclassotherbnftdetail = results[i].split("SpLitPResna")[20];
            var Mclasgrossonly = results[i].split("SpLitPResna")[21];
            Multiclassinva = resulstinva[i];
            var Mclasssfamount = results[i].split("SpLitPResna")[22];
            var Mclasssfgst = results[i].split("SpLitPResna")[23];
            //$('#hdMulticlassinva' + Id).val(Multiclassinva)

            selectedObj_extr[0]["data-hdMulticlassinva"] = Multiclassinva;
            //  $('.extrahidem_' + Id)[0].dataset.hdMulticlassinva = Multiclassinva;
            var select1 = $('#fareClass' + Id);
            if (results.length > 2) {
                $('#fareClass' + Id).css("display", "none");
            } else {
                $('#fareClass' + Id).css("display", "inline-block");
            }
            // document.getElementById("BaseFARE" + Id).innerHTML = MulticlassBaseFare;
            mclsbagg += multiclassbaggchange + "@";
            mbenifit += Mclassotherbnftdetail + "~";

            var Hdnfull = selectedObj_extr[0]["data-hdMulticlassinva"];// $('.extrahidem_' + Id)[0].dataset.hdMulticlassinva;// $('#hdMulticlassinva' + Id).val();
            var RepMulticlassToken = MulticlassToken.replace(/[$$]/g, '$$$');
            Hdnfull = Hdnfull.replace("Token", RepMulticlassToken);
            Hdnfull = Hdnfull.replace("RBD", Multiclassdetail);
            Hdnfull = Hdnfull.replace("classs", Multiclassdetail);
            Hdnfull = Hdnfull.replace("BaseAmnt", MulticlassBaseFare);
            Hdnfull = Hdnfull.replace("GrossAmnt", ($("#hdn_AvailFormat").val() == "RIYA" || $("#hdn_AvailFormat").val() == "NAT") ? Multiclassgrosswithmark : Mclasgrossonly);
            Hdnfull = Hdnfull.replace("FareBasisCode", Multiclasfarebasecode);
            //Added by saranraj on 20170717...
            Hdnfull = Hdnfull.replace("STUfareID", resoldinva[i].split("SpLitPResna")[25]);
            Hdnfull = Hdnfull.replace("STUservcharg", Multiclassservicecharge);
            Hdnfull = Hdnfull.replace("STUincentive", MulticlasIncentive);
            Hdnfull = Hdnfull.replace("STUmarkup", Multiclassmarkup);
            Hdnfull = Hdnfull.replace("STUsegref", resoldinva[i].split("SpLitPResna")[29]);
            //Hdnfull = Hdnfull.replace("grosswithmark", "grosswithmarkSpLitPResnaSpLitPResnaSpLitPResna");
            Hdnfull = Hdnfull.replace("grosswithmark", Multiclassgrosswithmark);
            //Added by saranraj on 20170717 End...
            hdninva += Hdnfull + "SpLITSaTIS";
            multitoken += MulticlassToken + "SpLITSaTIS";
        }
    } else {
        var MulticlassBaseCommFare = 0;
        for (var i = 0; i < results.length ; i++) {

            selectedObj_extr[0]["data-hdMulticlassinva"] = "";
            // $('.extrahidem_' + Id)[0].dataset.hdMulticlassinva = "";
            //$('#hdMulticlassinva' + Id).val("");
            // document.getElementById('Aspan_Class' + Id).style.display = "none";
            var MulticlassFare = result.split("SpLitPResna")[6];
            var MulticlassTransFare = results[i].split("SpLitPResna")[7];
            var MulticlassBaseFare = results[i].split("SpLitPResna")[4];
            MulticlassBaseCommFare = Number(results[i].split("SpLitPResna")[14]);
            var MulticlassDiscount = results[i].split("SpLitPResna")[15];
            var Multiclassgrossfare = results[i].split("SpLitPResna")[6];
            var Multiclassgrosswithmark = results[i].split("SpLitPResna")[7];
            MulticlassToken = results[i].split("SpLitPResna")[3];
            var Multiclassdetail = results[i].split("SpLitPResna")[2];
            var MulticlassTaxBreak = results[i].split("SpLitPResna")[17];
            var Multiclassmarkup = results[i].split("SpLitPResna")[9];
            var Multiclassservicecharge = results[i].split("SpLitPResna")[12];
            var multiclassbaggchange = results[i].split("SpLitPResna")[18];
            var Multiclasfarebasecode = results[i].split("SpLitPResna")[19];
            var MulticlasIncentive = results[i].split("SpLitPResna")[16];
            var Mclassotherbnftdetail = results[i].split("SpLitPResna")[20];
            var Mclasgrossonly = results[i].split("SpLitPResna")[21];
            Multiclassinva = resulstinva[i];
            var Mclasssfamount = results[i].split("SpLitPResna")[22];
            var Mclasssfgst = results[i].split("SpLitPResna")[23];
            //$('#hdMulticlassinva' + Id).val(Multiclassinva);
            selectedObj_extr[0]["data-hdMulticlassinva"] = Multiclassinva;
            //$('.extrahidem_' + Id)[0].dataset.hdMulticlassinva = Multiclassinva;
            var select1 = $('#fareClass' + Id);
            if (results.length > 2) {
                $('#fareClass' + Id).css("display", "none");
            } else {
                $('#fareClass' + Id).css("display", "inline-block");
            }
            // document.getElementById("BaseFARE" + Id).innerHTML = MulticlassBaseFare;
            mclsbagg += multiclassbaggchange + "@";
            mbenifit += Mclassotherbnftdetail + "~";
            var Hdnfull = selectedObj_extr[0]["data-hdMulticlassinva"];// $('.extrahidem_' + Id)[0].dataset.hdMulticlassinva; // $('#hdMulticlassinva' + Id).val();
            var RepMulticlassToken = MulticlassToken.replace(/[$$]/g, '$$$');
            Hdnfull = Hdnfull.replace("Token", RepMulticlassToken);
            Hdnfull = Hdnfull.replace("RBD", Multiclassdetail);
            Hdnfull = Hdnfull.replace("classs", Multiclassdetail);
            Hdnfull = Hdnfull.replace("BaseAmnt", MulticlassBaseFare);
            Hdnfull = Hdnfull.replace("GrossAmnt", ($("#hdn_AvailFormat").val() == "RIYA" || $("#hdn_AvailFormat").val() == "NAT") ? Multiclassgrosswithmark : Mclasgrossonly);
            Hdnfull = Hdnfull.replace("FareBasisCode", Multiclasfarebasecode);
            //Added by saranraj on 20170717...
            Hdnfull = Hdnfull.replace("STUfareID", resoldinva[i].split("SpLitPResna")[25]);
            Hdnfull = Hdnfull.replace("STUservcharg", Multiclassservicecharge);
            Hdnfull = Hdnfull.replace("STUincentive", MulticlasIncentive);
            Hdnfull = Hdnfull.replace("STUmarkup", Multiclassmarkup);
            Hdnfull = Hdnfull.replace("STUsegref", resoldinva[i].split("SpLitPResna")[29]);
            //Hdnfull = Hdnfull.replace("grosswithmark", "grosswithmarkSpLitPResnaSpLitPResnaSpLitPResna");
            Hdnfull = Hdnfull.replace("grosswithmark", Multiclassgrosswithmark);

            //Added by saranraj on 20170717 End...
            hdninva += Hdnfull;
            multitoken += MulticlassToken;
        }
    }
    var mbaggarr = new Array();
    var mbnftarr = new Array();
    mbaggarr = mclsbagg.split('@');
    mbnftarr = mbenifit.split('~');
    if (results.length > 1) {
        var tbl_id = document.getElementsByClassName("conflighttab_" + Id)[0].id;
        var i = 1;
        $('#' + tbl_id + ' article.box').each(function (ii, obj) {

            $(obj).find('.amenities .vcenter.mob-conn-bagg span').html(mbaggarr[i - 1]);
            $(obj).find('.amenities .vcenter.mob-conn-bagg').attr('data-original-title', "Baggage: " + mbaggarr[i - 1]);
            //}
            var conrowid = $(this).find('.mob-conn-flt .con-flt-img img.FlightTipimg')[0].id;
            var Bidvalue = $('#' + conrowid).attr("rel");
            if (Bidvalue != null && Bidvalue != "") {
                var tree = $("<div>" + Bidvalue + "</div>");
                tree.find('#baggmul')
                    .replaceWith("<span id='baggmul'>" + ((mbaggarr[i - 1] != null && mbaggarr[i - 1] != "") ? mbaggarr[i - 1] : "N/A") + "</span>");
                var bindcls = $('#' + conrowid);
                bindcls.attr("rel", tree.html());
                bindcls.tooltipster('content', tree.html());
            }
            else {
                var getvalue = $("#fliimage" + Id).attr("data-popup");
                var Poupdetails = getvalue.split("SltP");
                var assignvalues = Poupdetails[0] + "SltP" + Poupdetails[1] + "SltP" + Poupdetails[2] + "SltP" + Poupdetails[3] + "SltP" + Poupdetails[4] + "SltP" + Poupdetails[5] + "SltP" + Poupdetails[6]
                + "SltP" + mbaggarr[i - 1] + "SltP" + Poupdetails[8] + "SltP" + Poupdetails[9] + "SltP" + Poupdetails[10] + "SltP" + Poupdetails[11];
                // $("#fliimage" + Id).data('popup', assignvalues);
                $("#fliimage" + Id).attr('data-popup', assignvalues);
            }
            // }

            i++;
        });
    }

    var idss = Id;
    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == idss;
    });
    selectedObj[0]["data-hdInva"] = hdninva;
    selectedObj[0]["data-hdtoken"] = multitoken;
    // $('#hdtoken' + Id).val(multitoken);
    var getvalue = $('#fliimage' + Id).attr('rel');
    var Mbaggage = "";
    if (mclsbagg.indexOf('@') > 0) {
        mclsbagg = mclsbagg.replace(/@/g, '/');
        Mbaggage = mclsbagg.slice(0, mclsbagg.length - 1);
    }
    else {
        Mbaggage = mclsbagg.slice(0, mclsbagg.length - 1);
    }
    if (Mbaggage != null && Mbaggage != "" && Mbaggage != "N/A") {
        $("#showbagg" + Id).html(Mbaggage);
        //document.getElementById("showbaggimg" + Id).style.display = "";
        $("#showbagg" + Id).parent('.vcenter.mob-bagg').attr("data-original-title", "Baggage: " + Mbaggage);
    }
    else {
        $("#showbagg" + Id).html(Mbaggage);
        document.getElementById("showbaggimg" + Id).style.display = "none";
    }

    if (getvalue != null && getvalue != "") {
        var found = $(getvalue).find('#baggmul');
        var tree = $("<div>" + getvalue + "</div>");
        var strbag = Mbaggage;
        tree.find('#baggmul')
            .replaceWith("<span id='baggmul'>" + strbag + "</span>");
        var _elImage = $('#fliimage' + Id);
        _elImage.attr("rel", tree.html());
        _elImage.tooltipster('content', tree.html());
    }
    else {
        var getvalue = $("#fliimage" + Id).attr("data-popup");
        var Poupdetails = getvalue.split("SltP");
        var assignvalues = Poupdetails[0] + "SltP" + Poupdetails[1] + "SltP" + Poupdetails[2] + "SltP" + Poupdetails[3] + "SltP" + Poupdetails[4] + "SltP" + Poupdetails[5] + "SltP" + Poupdetails[6]
        + "SltP" + Mbaggage + "SltP" + Poupdetails[8] + "SltP" + Poupdetails[9] + "SltP" + Poupdetails[10] + "SltP" + Poupdetails[11];
        // $("#fliimage" + Id).data('popup', assignvalues);
        $("#fliimage" + Id).attr('data-popup', assignvalues);
    }

    var markVal = Multiclassmarkup;
    var Incentive = MulticlasIncentive;
    var lessgrossincentive = "";
    var lessgrossWMincentive = "";
    var serVal = Multiclassservicecharge;
    var comVal = MulticlassBaseCommFare;
    var Lesscommissionfare = MulticlassTransFare;
    var SFtax = Mclasssfamount;
    var Sfgst = Mclasssfgst;
    if (comVal.toString() != "0") {
        if ($("#hdn_AvailFormat").val() == "RIYA") {
            document.getElementById("show_earning" + Id).innerHTML = comVal + "/" + parseFloat(parseFloat(MulticlassTransFare) - parseFloat(comVal)).toFixed(decimalflag);
            $("#show_earning" + Id).attr("title", "Comm : " + comVal + "/N.Fare :" + parseFloat(parseFloat(MulticlassTransFare) - parseFloat(comVal)).toFixed(decimalflag));
            $("#show_earning" + Id).attr("data-original-title", "Comm : " + comVal + "/N.Fare :" + parseFloat(parseFloat(MulticlassTransFare) - parseFloat(comVal)).toFixed(decimalflag));
            $("#show_earning" + Id).addClass('licommisionSpan');
        }
        else {
            document.getElementById("show_earning" + Id).innerHTML = "Earn." + comVal;
            $("#show_earning" + Id).addClass('licommisionSpan');
        }
        Lesscommissionfare = parseInt(MulticlassTransFare) - parseInt(comVal);
    }
    else {
        if ($("#hdn_AvailFormat").val() == "RIYA") {
            document.getElementById("show_earning" + Id).innerHTML = comVal + "/" + parseFloat(parseFloat(MulticlassTransFare) - parseFloat(0)).toFixed(decimalflag);
            $("#show_earning" + Id).attr("title", "Comm : 0 /N.Fare :" + parseFloat(parseFloat(MulticlassTransFare) - parseFloat(0)).toFixed(decimalflag));
            $("#show_earning" + Id).attr("data-original-title", "Comm : 0 /N.Fare :" + parseFloat(parseFloat(MulticlassTransFare) - parseFloat(0)).toFixed(decimalflag));
            $("#show_earning" + Id).addClass('licommisionSpan');
        }
        else {
            Lesscommissionfare = parseInt(MulticlassTransFare);
            document.getElementById("show_earning" + Id).innerHTML = "";
            $("#show_earning" + Id).removeClass('licommisionSpan');
        }
    }

    var eTotelGross = MulticlassTransFare;
    var eWTMTotelGross = Multiclassgrossfare;
    var eToken = MulticlassToken;
    var eCls = document.getElementById("fareClass" + Id);
    var strUser = "";
    var Stx = MulticlassTaxBreak.split('/');
    STax_build += "<table width='130px' >";
    for (var tLen = 0; tLen < Stx.length; tLen++) {
        var JsonTax = Stx[tLen].split(":");
        STax_build += "<tr><td>" + JsonTax[0] + "</td><td>" + JsonTax[1] + "</td></tr>";
    }
    if (SFtax != "" && parseInt(SFtax) != 0) {
        STax_build += "<tr><td>SF</td><td>" + SFtax + "</td></tr>";
    }
    if (Sfgst != "" && parseInt(Sfgst) != 0) {
        STax_build += "<tr><td>SF GST</td><td>" + Sfgst + "</td></tr>";
    }
    if (Incentive.toString() != "0" && Incentive.toString() != null && Incentive.toString() != "") {
        STax_build += "<tr><td>Discount</td><td>" + Incentive + "</td></tr>";
        lessgrossincentive = parseInt(eTotelGross) - parseInt(Incentive);
        lessgrossWMincentive = parseInt(eWTMTotelGross) - parseInt(Incentive);
    }
    if (markVal.toString() != "0") {
        if ((document.getElementById("Nfare").checked == true) || ($("#hdn_AvailFormat").val() == "RIYA" || $("#hdn_AvailFormat").val() == "NAT")) {
            STax_build += "<tr><td>SC</td><td>" + markVal + "</td></tr>";
        }
        else {
            STax_build += "<tr class='Markup' style='display:none'><td>SC</td><td>" + markVal + "</td></tr>";
        }
    }
    if (serVal.toString() != "0") {
        var txtflag = $("#hdn_AvailFormat").val() == "RIYA" ? "Management Fee" : "Serv.chg";
        STax_build += "<tr><td>" + txtflag + "</td><td>" + serVal + "</td></tr>";
    }
    if ((document.getElementById("Nfare").checked == true)) {
        STax_build += "<tr class='CGFare' style='display:table-row'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + eTotelGross + "</td></tr>";
        STax_build += "<tr class='WTMFare' style='display:none'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + eWTMTotelGross + "</td></tr>";
    }
    else if (($("#hdn_AvailFormat").val() == "RIYA" || $("#hdn_AvailFormat").val() == "NAT")) {
        STax_build += "<tr class='WTMFare'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + eTotelGross + "</td></tr>";
    }
    else {
        STax_build += "<tr class='CGFare'  style='display:none'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + eTotelGross + "</td></tr>";
        STax_build += "<tr class='WTMFare' style='display:table-row'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + eWTMTotelGross + "</td></tr>";
    }
    STax_build += "</table>";
    if ((document.getElementById("Nfare").checked == true) || ($("#hdn_AvailFormat").val() == "RIYA")) {
        strUser = "<span class='price mob-price' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + eTotelGross + "</span><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
    }
    else if ($("#hdn_AvailFormat").val() == "NAT") {
        if (comVal.toString() != "0") {
            strUser += "<p class='Bestbuyfarewithmarkup'><strike>" + eTotelGross + "</strike></p>"
            strUser = "<span class='price mob-price' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + Lesscommissionfare + "</span><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
        }
        else {
            strUser = "<span class='price mob-price' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + Lesscommissionfare + "</span><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
        }
    }
    else {
        strUser = "<span class='price mob-price' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + eWTMTotelGross + "</span><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
    }
    $("#GorssFareSpan2" + Id).attr('data-grrfare', eTotelGross);
    $("#GorssFareSpan2" + Id).attr('data-netfare', eTotelGross);
    $("#GorssFareSpan2" + Id).attr('data-WTMFare', eWTMTotelGross);
    $("#GorssFareSpan2" + Id).attr('data-id', Id);
    //}
    document.getElementById("GorssFareSpan2" + Id).innerHTML = strUser;

    var idss = Id;
    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == idss;
    });
    selectedObj[0]["data-hdInva"] = hdninva;
    selectedObj[0]["data-hdtoken"] = eToken;
    if (segtype == "DIR") {
        var resultvalue = Id + "SPLITRAV" + Multiclassdetail + "SPLITRAV" + result + "SPLITINVA" + resultinva + "SPLITINVA" + Invatt + "SplitClas" + Multiclasfarebasecode;
        multiclasarr.push(resultvalue);
    }
    else if (segtype == "CON") {//cabin
        var resultvalue = Id + "SPLITRAV" + cabin + "SPLITRAV" + result + "SPLITINVA" + resultinva + "SPLITINVA" + Invatt + "SplitClas" + Multiclasfarebasecode;
        multiclasconarr.push(resultvalue);
    }
    ShowHideNFare("flg1");
}


function updatelocalsetmutliclasvalue(result, resultinva, Id, Invatt) {
    var trip_type = $('#hdtxt_trip')[0].value;
    $('#btn_multiclas_' + Id).css("display", "none");
    $('#btn_multiclasforconn_' + Id).css("display", "none");
    $('#selectclickbutton' + Id).css("display", "inline-block");

    var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;
    var idss = Id;
    var selectedObj_extr = $.grep(extrahidem_dataproperty_array, function (n, i) {
        return n.extrahide_datakey == idss;
    });

    if (trip_type == "R") {
        $('#btn_multiclasforconn_' + Id).siblings('.roundTripFareRadio').show();//radiobutton show in Roundtrip while multiclass change... by saranraj...
        $('#btn_multiclasforconn_' + Id).removeClass('rtripgfar');
        $('#btn_multiclasforconn_' + Id).parents('.farearea').children('.mob-mk-trc').show();  //Checkbox show in Roundtrip while multiclass change... by saranraj...
    }
    var STax_build = "";
    var results = result.split("SpLITSaTIS");
    var resulstinva = resultinva.split("SpLITSaTIS");
    var resoldinva = Invatt.split("SpLITSaTIS");
    var Multiclassinva = "";
    var hdninva = "";
    var MulticlassToken = "";
    var multitoken = "";
    var mclasbagg = "";
    var mbenifit = "";
    if (results.length > 1) {
        for (var i = 0; i < results.length - 1; i++) {
            // $('#hdMulticlassinva' + Id).val("");

            selectedObj_extr[0]["data-hdMulticlassinva"] = "";
            //$('.extrahidem_' + Id)[0].dataset.hdMulticlassinva = "";
            // document.getElementById('Aspan_Class' + Id).style.display = "none";
            var MulticlassFare = result.split("SpLitPResna")[6];
            var MulticlassTransFare = results[i].split("SpLitPResna")[7];
            var MulticlassBaseFare = results[i].split("SpLitPResna")[4];
            var MulticlassBaseCommFare = results[i].split("SpLitPResna")[14];
            var MulticlassDiscount = results[i].split("SpLitPResna")[15];
            var Multiclassgrossfare = results[i].split("SpLitPResna")[6];
            var Multiclassgrosswithmark = results[i].split("SpLitPResna")[7];
            MulticlassToken = results[i].split("SpLitPResna")[3];
            var Multiclassdetail = results[i].split("SpLitPResna")[2];
            var MulticlassTaxBreak = results[i].split("SpLitPResna")[17];
            var Multiclassmarkup = results[i].split("SpLitPResna")[9];
            var Multiclassservicecharge = results[i].split("SpLitPResna")[12];
            // var Multiclasswithoutmarkup = result[2].split("SpLitPResna")[18];
            var multiclassbaggchange = results[i].split("SpLitPResna")[18];
            var Multiclasfarebasecode = results[i].split("SpLitPResna")[19];
            var MulticlasIncentive = results[i].split("SpLitPResna")[16];
            var Mclassotherbnftdetail = results[i].split("SpLitPResna")[20];
            var Mclasgrossonly = results[i].split("SpLitPResna")[21];
            var Mclasssfamount = results[i].split("SpLitPResna")[22];
            var Mclasssfgst = results[i].split("SpLitPResna")[23];
            Multiclassinva = resulstinva[i];
            //$('#hdMulticlassinva' + Id).val(Multiclassinva)
            selectedObj_extr[0]["data-hdMulticlassinva"] = Multiclassinva;
            //  $('.extrahidem_' + Id)[0].dataset.hdMulticlassinva = Multiclassinva;
            var select1 = $('#fareClass' + Id);
            if (results.length > 2) {
                $('#fareClass' + Id).css("display", "none");
            } else {
                $('#fareClass' + Id).css("display", "inline-block");
            }
            // document.getElementById("BaseFARE" + Id).innerHTML = MulticlassBaseFare;
            mclasbagg += multiclassbaggchange + "@";
            mbenifit += Mclassotherbnftdetail + "~";
            var Hdnfull = selectedObj_extr[0]["data-hdMulticlassinva"];// $('.extrahidem_' + Id)[0].dataset.hdMulticlassinva; // $('#hdMulticlassinva' + Id).val();
            var RepMulticlassToken = MulticlassToken.replace(/[$$]/g, '$$$');
            Hdnfull = Hdnfull.replace("Token", RepMulticlassToken)
            Hdnfull = Hdnfull.replace("RBD", Multiclassdetail)
            Hdnfull = Hdnfull.replace("classs", Multiclassdetail)
            Hdnfull = Hdnfull.replace("BaseAmnt", MulticlassBaseFare)
            Hdnfull = Hdnfull.replace("GrossAmnt", ($("#hdn_AvailFormat").val() == "RIYA" || $("#hdn_AvailFormat").val() == "NAT") ? Multiclassgrosswithmark : Mclasgrossonly)
            Hdnfull = Hdnfull.replace("FareBasisCode", Multiclasfarebasecode)
            //Added by saranraj on 20170717...
            Hdnfull = Hdnfull.replace("STUfareID", resoldinva[i].split("SpLitPResna")[25]);
            Hdnfull = Hdnfull.replace("STUservcharg", Multiclassservicecharge);
            Hdnfull = Hdnfull.replace("STUincentive", MulticlasIncentive);
            Hdnfull = Hdnfull.replace("STUmarkup", Multiclassmarkup);
            Hdnfull = Hdnfull.replace("STUsegref", resoldinva[i].split("SpLitPResna")[29]);
            //Hdnfull = Hdnfull.replace("grosswithmark", "grosswithmarkSpLitPResnaSpLitPResnaSpLitPResna");
            Hdnfull = Hdnfull.replace("grosswithmark", Multiclassgrosswithmark);

            //Added by saranraj on 20170717 End...
            hdninva += Hdnfull + "SpLITSaTIS";
            multitoken += MulticlassToken + "SpLITSaTIS";
        }
    }
    else {
        for (var i = 0; i < results.length; i++) {
            //  $('#hdMulticlassinva' + Id).val("");
            selectedObj_extr[0]["data-hdMulticlassinva"] = "";
            //$('.extrahidem_' + Id)[0].dataset.hdMulticlassinva = "";
            // document.getElementById('Aspan_Class' + Id).style.display = "none";
            var MulticlassFare = result.split("SpLitPResna")[6];
            var MulticlassTransFare = results[i].split("SpLitPResna")[7];
            var MulticlassBaseFare = results[i].split("SpLitPResna")[4];
            var MulticlassBaseCommFare = results[i].split("SpLitPResna")[14];
            var MulticlassDiscount = results[i].split("SpLitPResna")[15];
            var Multiclassgrossfare = results[i].split("SpLitPResna")[6];
            var Multiclassgrosswithmark = results[i].split("SpLitPResna")[7];
            MulticlassToken = results[i].split("SpLitPResna")[3];
            var Multiclassdetail = results[i].split("SpLitPResna")[2];
            var MulticlassTaxBreak = results[i].split("SpLitPResna")[17];
            var Multiclassmarkup = results[i].split("SpLitPResna")[9];
            var Multiclassservicecharge = results[i].split("SpLitPResna")[12];
            // var Multiclasswithoutmarkup = result[2].split("SpLitPResna")[18];
            var multiclassbaggchange = results[i].split("SpLitPResna")[18];
            var Multiclasfarebasecode = results[i].split("SpLitPResna")[19];
            var MulticlasIncentive = results[i].split("SpLitPResna")[16];
            var Mclassotherbnftdetail = results[i].split("SpLitPResna")[20];
            var Mclasgrossonly = results[i].split("SpLitPResna")[21];
            var Mclasssfamount = results[i].split("SpLitPResna")[22];
            var Mclasssfgst = results[i].split("SpLitPResna")[23];
            Multiclassinva = resulstinva[i];
            //$('#hdMulticlassinva' + Id).val(Multiclassinva);
            selectedObj_extr[0]["data-hdMulticlassinva"] = Multiclassinva;
            //   $('.extrahidem_' + Id)[0].dataset.hdMulticlassinva = Multiclassinva;
            var select1 = $('#fareClass' + Id);
            if (results.length > 2) {
                $('#fareClass' + Id).css("display", "none");
            } else {
                $('#fareClass' + Id).css("display", "inline-block");
            }

            //document.getElementById("BaseFARE" + Id).innerHTML = MulticlassBaseFare;
            mclasbagg += multiclassbaggchange + "@";
            mbenifit += Mclassotherbnftdetail + "~";
            var Hdnfull = selectedObj_extr[0]["data-hdMulticlassinva"];//  $('.extrahidem_' + Id)[0].dataset.hdMulticlassinva; // $('#hdMulticlassinva' + Id).val();
            var RepMulticlassToken = MulticlassToken.replace(/[$$]/g, '$$$');
            Hdnfull = Hdnfull.replace("Token", RepMulticlassToken)
            Hdnfull = Hdnfull.replace("RBD", Multiclassdetail)
            Hdnfull = Hdnfull.replace("classs", Multiclassdetail)
            Hdnfull = Hdnfull.replace("BaseAmnt", MulticlassBaseFare)
            //Hdnfull = Hdnfull.replace("GrossAmnt", Multiclassgrosswithmark)
            // Hdnfull = Hdnfull.replace("GrossAmnt", Multiclassgrossfare)
            Hdnfull = Hdnfull.replace("GrossAmnt", ($("#hdn_AvailFormat").val() == "RIYA" || $("#hdn_AvailFormat").val() == "NAT") ? Multiclassgrosswithmark : Mclasgrossonly)
            Hdnfull = Hdnfull.replace("FareBasisCode", Multiclasfarebasecode)
            //Added by saranraj on 20170717...
            Hdnfull = Hdnfull.replace("STUfareID", resoldinva[i].split("SpLitPResna")[25]);
            Hdnfull = Hdnfull.replace("STUservcharg", Multiclassservicecharge);
            Hdnfull = Hdnfull.replace("STUincentive", MulticlasIncentive);
            Hdnfull = Hdnfull.replace("STUmarkup", Multiclassmarkup);
            Hdnfull = Hdnfull.replace("STUsegref", resoldinva[i].split("SpLitPResna")[29]);
            // Hdnfull = Hdnfull.replace("grosswithmark", "grosswithmarkSpLitPResnaSpLitPResnaSpLitPResna");
            Hdnfull = Hdnfull.replace("grosswithmark", Multiclassgrosswithmark);

            //Added by saranraj on 20170717 End...
            hdninva += Hdnfull;
            multitoken += MulticlassToken;
        }
    }
    var mbaggarr = new Array();
    var mbnftarr = new Array();
    mbaggarr = mclasbagg.split('@');
    mbnftarr = mbenifit.split('~');
    if (results.length > 1) {
        var tbl_id = document.getElementsByClassName("conflighttab_" + Id)[0].id;
        //var tblrowslen = document.getElementById(tbl_id).rows.length;
        var i = 1;
        $('#' + tbl_id + ' article.box').each(function (ii, obj) {
            $(obj).find('.amenities .vcenter.mob-conn-bagg span').html(mbaggarr[i - 1]);
            $(obj).find('.amenities .vcenter.mob-conn-bagg').attr("title", mbaggarr[i - 1]);
            //}
            var conrowid = $(this).find('.mob-conn-flt .con-flt-img img.FlightTipimg')[0].id; //document.getElementById(tbl_id).rows[i].cells[0].children[0].children[0].id;
            var Bidvalue = $('#' + conrowid).attr("rel");
            if (Bidvalue != null && Bidvalue != "") {
                var getvalue = $("#fliimage" + Id).attr("data-popup");
                var Poupdetails = getvalue.split("SltP");
                var assignvalues = Poupdetails[0] + "SltP" + Poupdetails[1] + "SltP" + Poupdetails[2] + "SltP" + Poupdetails[3] + "SltP" + Poupdetails[4] + "SltP" + Poupdetails[5] + "SltP" + Poupdetails[6]
                + "SltP" + mbaggarr[i - 1] + "SltP" + Poupdetails[8] + "SltP" + Poupdetails[9] + "SltP" + Poupdetails[10] + "SltP" + Poupdetails[11];

                //$("#fliimage" + Id).data('popup', assignvalues);
                $("#fliimage" + Id).attr('data-popup', assignvalues);
            }
            else {
                var getvalue = $("#fliimage" + Id).attr("data-popup");
                var Poupdetails = getvalue.split("SltP");
                var assignvalues = Poupdetails[0] + "SltP" + Poupdetails[1] + "SltP" + Poupdetails[2] + "SltP" + Poupdetails[3] + "SltP" + Poupdetails[4] + "SltP" + Poupdetails[5] + "SltP" + Poupdetails[6]
                + "SltP" + mbaggarr[i - 1] + "SltP" + Poupdetails[8] + "SltP" + Poupdetails[9] + "SltP" + Poupdetails[10] + "SltP" + Poupdetails[11];

                //$("#fliimage" + Id).data('popup', assignvalues);
                $("#fliimage" + Id).attr('data-popup', assignvalues);
            }
            i++;
        });

    }

    var idss = Id;
    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == idss;
    });


    selectedObj[0]["data-hdInva"] = hdninva;
    selectedObj[0]["data-hdtoken"] = multitoken;
    var getvalue = $('#fliimage' + Id).attr('rel');
    var Mbaggage = "";
    if (mclasbagg.indexOf('@') > 0) {
        mclasbagg = mclasbagg.replace(/@/g, '/');
        Mbaggage = mclasbagg.slice(0, mclasbagg.length - 1);
    }
    else {
        Mbaggage = mclasbagg.slice(0, mclasbagg.length - 1);
    }
    if (Mbaggage != null && Mbaggage != "" && Mbaggage != "N/A") {
        $("#showbagg" + Id).html(Mbaggage);
        // document.getElementById("showbaggimg" + Id).style.display = "";
    }
    else {
        $("#showbagg" + Id).html(Mbaggage);
        document.getElementById("showbaggimg" + Id).style.display = "none";
    }

    if (getvalue != null && getvalue != "") {
        var getvalue = $("#fliimage" + Id).attr("data-popup");
        var Poupdetails = getvalue.split("SltP");
        var assignvalues = Poupdetails[0] + "SltP" + Poupdetails[1] + "SltP" + Poupdetails[2] + "SltP" + Poupdetails[3] + "SltP" + Poupdetails[4] + "SltP" + Poupdetails[5] + "SltP" + Poupdetails[6]
        + "SltP" + Mbaggage + "SltP" + Poupdetails[8] + "SltP" + Poupdetails[9] + "SltP" + Poupdetails[10] + "SltP" + Poupdetails[11];
        //$("#fliimage" + Id).data('popup', assignvalues);
        $("#fliimage" + Id).attr('data-popup', assignvalues);
    }
    else {
        var getvalue = $("#fliimage" + Id).attr("data-popup");
        var Poupdetails = getvalue.split("SltP");
        var assignvalues = Poupdetails[0] + "SltP" + Poupdetails[1] + "SltP" + Poupdetails[2] + "SltP" + Poupdetails[3] + "SltP" + Poupdetails[4] + "SltP" + Poupdetails[5] + "SltP" + Poupdetails[6]
        + "SltP" + Mbaggage + "SltP" + Poupdetails[8] + "SltP" + Poupdetails[9] + "SltP" + Poupdetails[10] + "SltP" + Poupdetails[11];
        //$("#fliimage" + Id).data('popup', assignvalues);
        $("#fliimage" + Id).attr('data-popup', assignvalues);
    }


    var markVal = Multiclassmarkup;
    var Incentive = MulticlasIncentive;
    var lessgrossincentive = "";
    var lessgrossWMincentive = "";
    var serVal = Multiclassservicecharge;
    var comVal = MulticlassBaseCommFare;
    var Lesscommissionfare = MulticlassTransFare;
    if (comVal.toString() != "0") {
        if ($("#hdn_AvailFormat").val() == "RIYA") {
            document.getElementById("show_earning" + Id).innerHTML = comVal + "/" + parseFloat(parseFloat(MulticlassTransFare) - parseFloat(comVal)).toFixed(decimalflag);
            $("#show_earning" + Id).attr("title", "Comm : " + comVal + "/N.Fare :" + parseFloat(parseFloat(MulticlassTransFare) - parseFloat(comVal)).toFixed(decimalflag));
            $("#show_earning" + Id).attr("data-original-title", "Comm : " + comVal + "/N.Fare :" + parseFloat(parseFloat(MulticlassTransFare) - parseFloat(comVal)).toFixed(decimalflag));
            $("#show_earning" + Id).addClass('licommisionSpan');
        }
        else {
            document.getElementById("show_earning" + Id).innerHTML = "Earn." + comVal;
            $("#show_earning" + Id).addClass('licommisionSpan');
        }
        Lesscommissionfare = parseInt(MulticlassTransFare) - parseInt(comVal);
    }
    else {
        if ($("#hdn_AvailFormat").val() == "RIYA") {
            document.getElementById("show_earning" + Id).innerHTML = comVal + "/" + parseFloat(parseFloat(MulticlassTransFare) - parseFloat(0)).toFixed(decimalflag);
            $("#show_earning" + Id).attr("title", "Comm : 0 /N.Fare :" + parseFloat(parseFloat(MulticlassTransFare) - parseFloat(0)).toFixed(decimalflag));
            $("#show_earning" + Id).attr("data-original-title", "Comm : 0 /N.Fare :" + parseFloat(parseFloat(MulticlassTransFare) - parseFloat(0)).toFixed(decimalflag));
            $("#show_earning" + Id).addClass('licommisionSpan');
        }
        else {
            Lesscommissionfare = parseInt(MulticlassTransFare);
            document.getElementById("show_earning" + Id).innerHTML = "";
            $("#show_earning" + Id).removeClass('licommisionSpan');
        }
    }
    var eTotelGross = MulticlassTransFare;
    var eWTMTotelGross = Multiclassgrossfare;
    var eToken = MulticlassToken;
    var SFtax = Mclasssfamount;
    var Sfgst = Mclasssfgst;
    var eCls = document.getElementById("fareClass" + Id);
    var strUser = "";
    var Stx = MulticlassTaxBreak.split('/');
    STax_build += "<table width='130px' >";
    for (var tLen = 0; tLen < Stx.length; tLen++) {
        var JsonTax = Stx[tLen].split(":");
        STax_build += "<tr><td>" + JsonTax[0] + "</td><td>" + JsonTax[1] + "</td></tr>";
    }
    if (SFtax != "" && parseInt(SFtax) != 0) {
        STax_build += "<tr><td>SF</td><td>" + SFtax + "</td></tr>";
    }
    if (Sfgst != "" && parseInt(Sfgst) != 0) {
        STax_build += "<tr><td>SF GST</td><td>" + Sfgst + "</td></tr>";
    }
    if (Incentive.toString() != "0" && Incentive.toString() != null && Incentive.toString() != "") {
        STax_build += "<tr><td>Discount</td><td>" + Incentive + "</td></tr>";
        lessgrossincentive = parseInt(eTotelGross) - parseInt(Incentive);
        lessgrossWMincentive = parseInt(eWTMTotelGross) - parseInt(Incentive);
    }
    if (markVal.toString() != "0") {
        if ((document.getElementById("Nfare").checked == true) || ($("#hdn_AvailFormat").val() == "RIYA" || $("#hdn_AvailFormat").val() == "NAT")) {
            STax_build += "<tr><td>SC</td><td>" + markVal + "</td></tr>";
        }
        else {
            STax_build += "<tr class='Markup' style='display:none'><td>SC</td><td>" + markVal + "</td></tr>";
        }
    }
    if (serVal.toString() != "0") {
        var txtflag = $("#hdn_AvailFormat").val() == "RIYA" ? "Management Fee" : "Serv.chg";
        STax_build += "<tr><td>" + txtflag + "</td><td>" + serVal + "</td></tr>";
    }
    if ((document.getElementById("Nfare").checked == true)) {
        STax_build += "<tr class='CGFare' style='display:table-row'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + eTotelGross + "</td></tr>";
        STax_build += "<tr class='WTMFare' style='display:none'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + eWTMTotelGross + "</td></tr>";
    }
    else if (($("#hdn_AvailFormat").val() == "RIYA" || $("#hdn_AvailFormat").val() == "NAT")) {
        STax_build += "<tr class='WTMFare'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + eTotelGross + "</td></tr>";
    }
    else {
        STax_build += "<tr class='CGFare'  style='display:none'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + eTotelGross + "</td></tr>";
        STax_build += "<tr class='WTMFare' style='display:table-row'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + eWTMTotelGross + "</td></tr>";
    }
    STax_build += "</table>";
    if (Incentive.toString() != "0" && Incentive.toString() != null && Incentive.toString() != "") {
        if ((document.getElementById("Nfare").checked == true) || ($("#hdn_AvailFormat").val() == "RIYA")) {
            strUser = "<p class='Bestbuyfarewithmarkup' style='margin-left: 0px;' ><strike>" + eTotelGross + "</strike></p><span class='price mob-price' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + lessgrossincentive + "</span><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
        }
        else if ($("#hdn_AvailFormat").val() == "NAT") {
            if (comVal.toString() != "0") {
                strUser += "<p class='Bestbuyfarewithmarkup'><strike>" + eTotelGross + "</strike></p>"
                strUser = "<span class='price mob-price' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + Lesscommissionfare + "</span><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
            }
            else {
                strUser = "<span class='price mob-price' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + Lesscommissionfare + "</span><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
            }
        }
        else {
            strUser = "<p class='Bestbuyfarewithoutmarkup' style='margin-left: 0px;' ><strike>" + eWTMTotelGross + "</strike></p><span class='price mob-price' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + lessgrossWMincentive + "</span><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
            // strUser = "<label id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + eWTMTotelGross + "</label><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-yellow'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
        }
        $("#GorssFareSpan2" + Id).attr('data-grrfare', lessgrossincentive);
        $("#GorssFareSpan2" + Id).attr('data-netfare', eTotelGross);
        $("#GorssFareSpan2" + Id).attr('data-WTMFare', lessgrossWMincentive);
        $("#GorssFareSpan2" + Id).attr('data-id', Id);
    }
    else {
        if ((document.getElementById("Nfare").checked == true) || ($("#hdn_AvailFormat").val() == "RIYA")) {
            strUser = "<span class='price mob-price' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + eTotelGross + "</span><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
        }
        else if ($("#hdn_AvailFormat").val() == "NAT") {
            if (comVal.toString() != "0") {
                strUser += "<p class='Bestbuyfarewithmarkup'><strike>" + eTotelGross + "</strike></p>"
                strUser = "<span class='price mob-price' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + Lesscommissionfare + "</span><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
            }
            else { strUser = "<span class='price mob-price' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + Lesscommissionfare + "</span><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>"; }
        }
        else {
            strUser = "<span class='price mob-price' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + eWTMTotelGross + "</span><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
        }
        $("#GorssFareSpan2" + Id).attr('data-grrfare', eTotelGross);
        $("#GorssFareSpan2" + Id).attr('data-netfare', eTotelGross);
        $("#GorssFareSpan2" + Id).attr('data-WTMFare', eWTMTotelGross);
        $("#GorssFareSpan2" + Id).attr('data-id', Id);
    }
    document.getElementById("GorssFareSpan2" + Id).innerHTML = strUser;

    var idss = Id;
    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == idss;
    });




    selectedObj[0]["data-hdInva"] = hdninva;
    selectedObj[0]["data-hdtoken"] = eToken;
    $("[data-toggle='tooltip']").tooltip();
    ShowHideNFare("flg1");
}



//****** For Theme2 ******//

//****** For Theme2 ******//

function searchvalidation() {
    var msg = "";
    var origincityvalidate, destinationcity = "";
    if ($("#txtorigincity").val() != null && $("#txtorigincity").val() != "") {
        origincityvalidate = $("#txtorigincity").val().split(")")[1];
        if (origincityvalidate != null && origincityvalidate != "") {
            msg = "Please select valid origin city.";
            showError1(msg);
            $("#txtorigincity").focus();
            return false;
        }
    }
    if ($("#txtdestinationcity").val() != null && $("#txtdestinationcity").val() != "") {
        origincityvalidate = $("#txtdestinationcity").val().split(")")[1];
        if (origincityvalidate != null && origincityvalidate != "") {
            msg = "Please select valid destination city.";
            showError1(msg);
            $("#txtdestinationcity").focus();
            return false;
        }
    }
    if ($("#txtorigincity").val() == null || $("#txtorigincity").val().trim() == "") {
        msg = "Please select origin city.";
        showError1(msg);
        $("#txtorigincity").focus();
        return false;
    }
    else if ($("#txtdestinationcity").val() == null || $("#txtdestinationcity").val().trim() == "") {
        msg = "Please select destination city.";
        showError1(msg);
        $("#txtdestinationcity").focus();
        return false;
    }
    else if ($("#txtorigincity").val().trim() == $("#txtdestinationcity").val().trim()) {
        msg = "Origin And Destination should not be same.";
        showError1(msg);
        $("#txtdestinationcity").val("");
        $("#txtdestinationcity").focus();
        return false;
    }
    else if ($("#txtdeparture").val() == null || $("#txtdeparture").val().trim() == "") {
        msg = "Please select departure  date.";
        showError1(msg);
        $("#txtdeparture").focus()
        return false;
    }
    else if (($("#txtarrivaldate").val() == null || $("#txtarrivaldate").val().trim() == "") && ($("body").data("tripflag") == "R" || $("body").data("tripflag") == "Y")) {
        msg = "Please select return date.";
        showError1(msg);
        $("#txtarrivaldate").focus()
        return false;
    }

    else if ($("#ddladult").val() == null || $("#ddladult").val() == "") {
        msg = "Please select no.of adult.";
        showError1(msg);
        $("#ddladult").focus()
        return false;
    }

    else if ($("#hdnAppTheme").val() == "THEME3") {
        if ($("#FlightTravellersClass").val() == null || $("#FlightTravellersClass").val() == "") {
            msg = "Please select class.";
            showError1(msg);
            $("#FlightTravellersClass").focus()
            return false;
        }
    }
    else {
        hideError1();
    }
    //******%*******// By Rajesh //******%*******//
    if ($("#hdn_checkflag").val() == "2" && $("body").data("tripflag") != "O" && $("body").data("tripflag") != "M" && (($("#hdn_checkflag").val() == "2" && $("body").data("tripflag") == "R") || ($("#hdn_checkflag").val() == "2" && $("#allowtripflag").val() == "Y"))) {
        var orgincity = "";
        var destincity = "";
        var Country = $("#hdntrip_country").val();
        var Orginid = $("#txtorigincity").val().split('(')[1].split(')')[0];
        var DestinId = $("#txtdestinationcity").val().split('(')[1].split(')')[0];

        var fiterdary = [];
        fiterdary = $.grep(loadGlobalcityArrry, function (v, j) {
            return v.CN == Country && v.ID == Orginid;
        });
        orgincity = fiterdary.length > 0 ? "1" : "";

        fiterdary = [];
        fiterdary = $.grep(loadGlobalcityArrry, function (v, j) {
            return v.CN == Country && v.ID == DestinId;
        });
        destincity = fiterdary.length > 0 ? "1" : "";

        if (orgincity == "1" && destincity == "1") {
            $("body").data("tripflag", "R");
            $("#hdn_allowtrip").val("R");
            $("#allowtripflag").val("Y");
            $("#hdtxt_trip").val("R");
        }
        else {
            $("body").data("tripflag", "Y");
            $("#hdn_allowtrip").val("Y");
            $("#allowtripflag").val("Y");
            $("#hdtxt_trip").val("Y");
        }

    }
    //******%*******// By Rajesh //******%*******//
    return true;
}

//#region Search Auto focus functions Start...
$('#txtorigincity').change(function () {

    $('#txtdestinationcity').focus();
});

$('#txtdestinationcity').change(function () {
    setTimeout(function () {
        $('#txtdeparture').focus();
    }, 300);
});

$('#txtdeparture').change(function () {
    if ($("#hdnAppTheme").val() != "THEME3") {
        if (($("body").data("tripflag") == "R") || ($("body").data("tripflag") == "Y")) {
            $('#txtarrivaldate').focus();
        }
    }
    if ($("#hdnAppTheme").val() == "THEME3" || $("#hdnAppTheme").val() == "THEME4" || $("#hdnAppTheme").val() == "THEME5") {
        if (($("body").data("tripflag") == "R") || ($("body").data("tripflag") == "Y")) {
            $('#txtarrivaldate').focus();
        }
        else {
            if ($("#hdn_MobileDevice").val() != "TRUE") {
                $('#FlightTravellersClass').click();
            }
        }
    }

});
$('#txtarrivaldate').change(function () {

    if ($("#hdnAppTheme").val() == "THEME3" || $("#hdnAppTheme").val() == "THEME4" || $("#hdnAppTheme").val() == "THEME5") {
        if ($("#hdn_MobileDevice").val() != "TRUE") {
            $('#FlightTravellersClass').click();
        }
    }
});


//#region Search Auto focus functions End...

$(document).on('change', '.clshideErrch', function () {
    hideError1("", "");
});

$('.clshideErr').on('change keyup paste', function () {
    hideError1("", "");
});

function showError1(msg) {
    if (assignedcountry == "EG") {
        showerralert(msg, "", "");
    }
    else {
        //$("#dvErrorView .message").addClass("animation");
        //$("#dvErrorView .searchpanel").addClass("has_error");
        //$("#empty-destination-box").html(msg);
        showerralert(msg, "", "");
    }


    setTimeout(function () {
        $("#dvErrorView .message").removeClass("animation");
    }, 200);
}

function hideError1(temp1, temp2) {
    $("#dvErrorView .message").removeClass("animation");
    $("#dvErrorView .searchpanel").removeClass("has_error");
}
//#region  Rays modal popups
function showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Iwidth, Itemp2) {

    $('#modal-Fare').iziModal('destroy');

    $("#modal-Fare").html(IContent);
    Iwidth = Iwidth != "" ? Iwidth : 700;

    //$(".iziModal").css("top","50%")
    $("#modal-Fare").iziModal({
        title: Ititle,
        subtitle: Isubtitle,
        fullscreen: Ifullopt,
        iconClass: 'icon-stack',
        headerColor: '#636363',
        width: Iwidth,
        padding: 20,
    });
    $('#modal-Fare').iziModal('open');
}
//#endregion

//#region  Rays modal popups
function showpopuplogin1(Ititle, Isubtitle, IContent, Ifullopt, Iwidth, Itemp2) {

    $('#modal-Fare-new').iziModal('destroy');

    $("#modal-Fare-new").html(IContent);
    Iwidth = Iwidth != "" ? Iwidth : 700;

    //$(".iziModal").css("top","50%")
    $("#modal-Fare-new").iziModal({
        title: Ititle,
        subtitle: Isubtitle,
        fullscreen: Ifullopt,
        iconClass: 'icon-stack',
        headerColor: '#636363',
        width: Iwidth,
        padding: 20,
    });
    $('#modal-Fare-new').iziModal('open');
}
//#endregion

function LogDetails(ErrorMsg, Code, Type) {

    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: logdatas, 		// Location of the service
        data: '{ErrorMsg: "' + ErrorMsg + '" ,Code: "' + Code + '" ,Type: "' + Type + '"}',

        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call
            var result = json.d;

        },
        error: function (e) {//On Successful service call
        }	// When Service call fails
    });
}
//#region Price  Filterpopup 
var pricefiltercheck = true;
var exisitId = "";
$(document).on('click', '.allFilter', function () {
    var ids = this.id;
    var arrowblock = $("#" + ids).data("target");
    var filterdivblock = $("#" + ids).data("arrowtarget");
    if ($("body").data("lastservicedata") != "") {
        exisitId = $("body").data("lastservicedata");
    }
    if (ids == exisitId) {
        $("." + arrowblock).fadeOut(500);
        $("." + filterdivblock).fadeOut(500);
        exisitId = "";
        $("body").data("lastservicedata", "");
    } else {
        $("." + arrowblock).fadeIn(500);
        $("." + filterdivblock).fadeIn(500);
        exisitId = ids;
    }

    pricefiltercheck = false;
});

$(document).mouseup(function (e) {
    var popup = $(".allFilterList");
    if (!$('.allFilter').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) {
        $("body").data("lastservicedata", exisitId);
        exisitId = "";
        $(".allFilterList").hide();
        $(".tipArrow").hide();
    }
});

function GetDetails(values) {
    if ($('#connectingFlghtdiv' + values)[0].style.display == "none") {
        $('#connectingFlghtdiv' + values).slideDown();
        if ($('#li_Rows' + values).length) {
            $('#li_Rows' + values).addClass('clsConnct_active');
        }
    }
    else {
        $('#connectingFlghtdiv' + values).slideUp(800);
        if ($('#li_Rows' + values).length) {
            $('#li_Rows' + values).removeClass('clsConnct_active');
        }
    }
}

$('#spnAvailFiltericon').click(function () {
    $('#resultFilter').slideToggle();
});

function SortingFAvail(Flag, tag, type) {
    var triptp = $("#hdtxt_trip")[0].value;
    var segtyp = $('body').data('segtype');
    var ordr = $(tag).data('sortorder');
    $(tag).data('sortorder', ordr == "asc" ? "desc" : "asc");
    var splitFlag = Flag.split('_');
    var dataprop = splitFlag.length > 0 ? splitFlag[0] : "price";
    var SetCount = splitFlag.length > 1 ? splitFlag[1] : "1";
    //var segcnt = triptp == "M" ? aryOrgMul.length : triptp == "R" ? 2 : 1; //For Oneway purpose... in future value ll come dynamically...
    var basetriptp = (triptp == "Y" || (triptp == "M" && segtyp == "I")) ? "O" : triptp;
    //for (var i = 1; i <= segcnt; i++) {
    ////tinysort('#availset_' + basetriptp + '_' + SetCount + ' article.slide-slow-down', //Particular sorting by parent ID...
    ////    {
    ////        data: dataprop,
    ////        order: ordr
    ////    });
    // }

    var dd = SetCount;
    $('.clssorting_' + basetriptp + "_" + dd + ' span i').remove();
    $(tag).find('span').append(ordr == "asc" ? '<i class="fa fa-long-arrow-down"></i>' : '<i class="fa fa-long-arrow-up"></i>');

    Afteravailsort(triptp, Flag, ordr, type); // type -> type used for sort click event find 0-> direct availbinding
}

function SortingFAvail_Earnings(Flag, tag) {
    var triptp = $("#hdtxt_trip")[0].value;
    var segtyp = $('body').data('segtype');
    var ordr = $(tag).data('sortorder');
    $(tag).data('sortorder', ordr == "asc" ? "desc" : "asc");
    var splitFlag = Flag.split('_');
    var dataprop = splitFlag.length > 0 ? splitFlag[0] : "price";
    var SetCount = splitFlag.length > 1 ? splitFlag[1] : "1";
    var segcnt = triptp == "O" ? 1 : 2; //For Oneway purpose... in future value ll come dynamically...
    var basetriptp = (triptp == "Y" || (triptp == "M" && segtyp == "I")) ? "O" : triptp;
    for (var i = 1; i <= segcnt ; i++) {
        tinysort('#availset_' + basetriptp + '_' + i + ' article.slide-slow-down', //Particular sorting by parent ID...
        {
            data: dataprop,
            order: ordr
        });
    }

    var dd = "1"; //SetCount;   //For single Sorting area for all grid availabilitis....
    $('span.clssorting_' + dd + ' i').remove();
    $(tag).append(ordr == "asc" ? '<i class="fa fa-sort-alpha-desc"></i>' : '<i class="fa fa-sort-alpha-asc"></i>');
}

//Fare Filteration definition...
function DeclareFareFilter(cnt) {
    for (var i = 1; i <= cnt; i++) {

        var minval = $("#minfare_dynamic_" + i + "").data('min') != "" ? parseInt($("#minfare_dynamic_" + i + "").data('min')) : 0;
        var maxval = $("#maxfare_dynamic_" + i + "").data('max') != "" ? parseInt($("#maxfare_dynamic_" + i + "").data('max')) : 0;
        $("#minfare_dynamic_" + i + ",#minfare2_dynamic_" + i).html(minval);
        $("#maxfare_dynamic_" + i + ",#maxfare2_dynamic_" + i).html(maxval);
        $("#slider-range_dynamic_" + i).slider({
            range: true,
            min: minval,//0,
            max: maxval,//maxval + 500,
            values: [minval, maxval],
            slide: function (event, ui) {
                
                var setID = $(this).closest('.filter-body').attr('id');
                var cont = setID.replace("filterBody_dynamic_", "");
                $("#minfare_dynamic_" + cont + ",#minfare2_dynamic_" + cont).html(ui.values[0]);
                $("#maxfare_dynamic_" + cont + ",#maxfare2_dynamic_" + cont).html(ui.values[1]);
                $("#minfare_dynamic_" + cont + "").data('min', ui.values[0]);
                $("#maxfare_dynamic_" + cont + "").data('max', ui.values[1]);
                if ($(window).width() > 767) { //For Desktop view only...
                    $("#minfare_dynamic_" + cont + ",#minfare2_dynamic_" + cont).html(ui.values[0]);
                    $("#maxfare_dynamic_" + cont + ",#maxfare2_dynamic_" + cont).html(ui.values[1]);
                    $("#minfare_dynamic_" + cont + "").data('min', ui.values[0]);
                    $("#maxfare_dynamic_" + cont + "").data('max', ui.values[1]);

                    FilterationFAvail(setID, "0", "", "");
                }
            }
        });
    }
}
//End...
function mobCloseFltr() {
    $('#resultFilter').slideUp();
}

$(document).on('change', '.chkfilter', function (event) {

    if ($(window).width() > 767) { //For Desktop view only...
        var setID = $(event.target).closest('.filter-body').attr('id');
        FilterationFAvail(setID, "0", "", "");
    }
});

$(document).on('change', '.chkcommonfilter', function (event) { //For Direct Flight and Not-Stop Flight...
    var targentid = event.currentTarget.id != null && event.currentTarget.id != "" ? event.currentTarget.id.trim() : "";
    if (targentid != "" && targentid == "chkNonstpFlt") {
        $('#chkDirFlt')[0].checked = false;
    }
    if (targentid != "" && targentid == "chkDirFlt") {
        $('#chkNonstpFlt')[0].checked = false;
    }
    FilterationFAvail("filterBody_dynamic_1", "1", "", "");
});

$(document).on('click', '.labletospancls', function (event) { //For Direct Flight and Not-Stop Flight...

    var tdelement = this.offsetParent.id;
    if ($("#" + tdelement).hasClass('selectedapndcls')) {
        $(".matrixflight").addClass('tmespn');
        $(".matrixflight").removeClass('selectedapndcls');
        $('.chkfilterdept_mat').attr('checked', false);
    }
    else {
        $(".matrixflight").addClass('tmespn');
        $(".matrixflight").removeClass('selectedapndcls');
        var tdelement = this.offsetParent.id;
        $("#" + tdelement).addClass('selectedapndcls');
        $('.chkfilterdept_mat').attr('checked', false);
        $("#" + this.control.id)[0].checked = true;
    }
    seperatematrixfil("airlinemat_main_div_1");

});



$('#applymobFAvailFltr').click(function () {
    var loopcnt = ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "D") ? aryOrgMul.length : ($('#hdtxt_trip').val() == "R" ? 2 : 1);
    for (var i = 0; i < loopcnt; i++) {
        FilterationFAvail("filterBody_dynamic_" + (i + 1), "0", "", "");
    }    
    mobCloseFltr(); //To Close Filteration div on mobile screen
});
var aryDeparture = [];
var aryArrival = [];
var arystops = [];
var aryAirlinecategory = [];
var aryRefund = [];
var aryFlightType = [];
var filtercount = 0;
var aryFaretypfilter = [];
function FilterationFAvail(segID, commflg, flgtype, matarray) {

    if ($("#hdn_rtsplflag").val() == "N") {


        var segCnt = segID.replace("filterBody_dynamic_", "");

        var triptp = $("#hdtxt_trip")[0].value;


        var segtyp = $('body').data('segtype');
        var loopCnt = 1; //For Direct and Non stop flight filteration the loop count ll increase depends on Trip type... by saranraj on 20170531...
        if (commflg == "1") {  //For Not Stop Flights or Direct Flights...
            loopCnt = (triptp == "O" || triptp == "Y" || (triptp == "M" && segtyp == "I")) ? 1 : triptp == "R" ? 2 : aryOrgMul.length;
        }

        if (triptp == "O" || triptp == "Y" || (triptp == "M" && segtyp == "I")) {
            $(window).scrollTop(0);
        }
        else {
            $('#' + 'availset_' + triptp + '_' + segCnt).scrollTop(0);
        }
        Main_totalfilterdarray = [];
        Main_totalfilterdarray_ret = [];
        for (var loop = 1; loop <= loopCnt; loop++) {
            if (commflg == "1") {  //For Not Stop Flights or Direct Flights...
                segCnt = loop;
            }
            if (flgtype != null && flgtype != "" && matarray != null && matarray != "") {
                var grouparray = [];
                grouparray = matarray;
            }
            else {
                var speratearr = $.grep(groupmain_array, function (value, index) {
                    return value.bindside_6_arg == segCnt;
                });
                var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                    previousValue[currentValue.flightid].push(currentValue);
                    return previousValue;
                }, {});
                var grouparray = [];
                for (var obj in groupedfli) {
                    grouparray.push(groupedfli[obj]);
                }
                grouparray.sort(function (a, b) {
                    if ($("#hdn_AvailFormat").val() == "NAT")
                    { return a[0].NETFare - b[0].NETFare; }
                    else { return a[0].GFare - b[0].GFare; }
                });  //fare soring after array :srinath

            }
            var minval = $("#minfare_dynamic_" + segCnt + "").data('min') != "" ? parseInt($("#minfare_dynamic_" + segCnt + "").data('min')) : 0;
            var maxval = $("#maxfare_dynamic_" + segCnt + "").data('max') != "" ? parseInt($("#maxfare_dynamic_" + segCnt + "").data('max')) : 0;
            aryDeparture = [];
            aryArrival = [];
            arystops = [];
            aryAirlinecategory = [];
            aryFaretypfilter = [];
            aryRefund = [];
            aryFlightType = [];
            aryDeparture_matrix = [];

            /*Departure Filteration*/
            $("#" + segID + " .chkfilterdept").each(function () {
                if ($(this)[0].checked == true) {
                    if ($(this).data('filter') == "dept1")
                        aryDeparture.push("0000-0600");
                    else if ($(this).data('filter') == "dept2")
                        aryDeparture.push("0601-1200");
                    else if ($(this).data('filter') == "dept3")
                        aryDeparture.push("1201-1800");
                    else if ($(this).data('filter') == "dept4")
                        aryDeparture.push("1801-2400");
                }



            });

            $(".detafil").each(function () {
                if ($(this)[0].checked == true) {
                    if ($(this).data('filter') == "dept1")
                        aryDeparture.push("0000-0600");
                    else if ($(this).data('filter') == "dept2")
                        aryDeparture.push("0601-1200");
                    else if ($(this).data('filter') == "dept3")
                        aryDeparture.push("1201-1800");
                    else if ($(this).data('filter') == "dept4")
                        aryDeparture.push("1801-2400");
                }
            });


            /*Arrival Filteration*/
            $("#" + segID + " .chkfilterarri").each(function () {
                if ($(this)[0].checked == true) {
                    if ($(this).data('filter') == "arri1")
                        aryArrival.push("0000-0600");
                    else if ($(this).data('filter') == "arri2")
                        aryArrival.push("0601-1200");
                    else if ($(this).data('filter') == "arri3")
                        aryArrival.push("1201-1800");
                    else if ($(this).data('filter') == "arri4")
                        aryArrival.push("1801-2400");
                }
            });

            /*Stops Filteration*/
            $("#" + segID + " .chkfilterstop").each(function () {
                if ($(this)[0].checked == true && $(this)[0].disabled == false) {
                    arystops.push($(this).data('filter').replace("stop", ""));
                }
            });

            $(".stopChk_all").each(function () {
                if ($(this)[0].checked == true && $(this)[0].disabled == false) {
                    arystops.push($(this).data('filter').replace("stop", ""));
                }
            });


            if ($('#chkDirFlt').is(':checked')) {  //For Direct Flights...
                arystops.push('0');
            }
            var nostop = "0";
            if ($('#chkNonstpFlt').is(':checked')) {  //For Non-Stop Flights...
                nostop = "1";
                arystops.push('0');
            }

            /*Refund Filteration*/
            $("#" + segID + " .chkfilterrefund").each(function () {
                if ($(this)[0].checked == true && $(this)[0].disabled == false) {
                    aryRefund.push($(this).data('filter'));
                }
                if (aryRefund.length === 2) {
                    aryRefund.push("");
                }
            });

            $(" .Refundablefil").each(function () {
                if ($(this)[0].checked == true && $(this)[0].disabled == false) {
                    aryRefund.push($(this).data('filter'));
                }
                if (aryRefund.length === 2) {
                    aryRefund.push("");
                }
            });




            /*Flight Type Filteration*/
            $("#" + segID + " .chkfilterflttyp").each(function () {
                if ($(this)[0].checked == true && $(this)[0].disabled == false) {
                    aryFlightType.push($(this).data('filter'));
                }
                if (aryFlightType.length === 2) {
                    aryFlightType.push("");
                }
            });

            /*Flight Filteration*/
            $("#" + segID + " .chkfilterplane").each(function () {
                if ($(this)[0].checked == true) {
                    aryAirlinecategory.push($(this).data('filter').replace("air", ""));
                    if ($(this).data('filter').replace("air", "") == "AK") {
                        aryAirlinecategory.push("I5");
                        aryAirlinecategory.push("FD");
                        aryAirlinecategory.push("D7");
                    }
                    else if ($(this).data('filter').replace("air", "") == "2T") {
                        aryAirlinecategory.push("TK");
                    }
                }
            });

            /*Fare Filteration*/
            $("#" + segID + " .chkfilterfaretype").each(function () {
                if ($(this)[0].checked == true) {
                    aryFaretypfilter.push($(this).data('filter').replace("faretype", ""));
                }
            });

            var amt = 0;
            var refund = "";
            var depttime = "";
            var arrivtime = "", stops = "", via = "", airline = "", airline_typ = "", arydeptminmax = [], aryarrivminmax = [], noofstops = [];
            var showFlag = false;
            var i = 0;
            var aryDeparturelength = aryDeparture.length, aryArrivallength = aryArrival.length, arystopslength = arystops.length, aryRefundlength = aryRefund.length, aryFlyTyplength = aryFlightType.length, aryAirlinecategorylength = aryAirlinecategory.length, aryFlightTypelength = aryFlightType.length, aryFaretypfilterlength = aryFaretypfilter.length;
            var basetripty = (triptp == "Y" || (triptp == "M" && segtyp == "I")) ? "O" : triptp;
            filtercount = 0;
            filterdarray = [];
            filterdarray = $.grep(grouparray, function (value, item) {
                if ($("#hdn_AvailFormat").val() == "NAT")
                { return parseInt(value[0].NETFare) >= Number(minval) && parseInt(value[0].NETFare) <= Number(maxval); }
                else {
                    return parseInt(value[0].GFare) >= Number(minval) && parseInt(value[0].GFare) <= Number(maxval);
                }
            });
            if ($('#chkNonstpFlt').is(':checked')) {
                filterdarray = $.grep(grouparray, function (value, item) {
                    return value[0].nonstop == "" || value[0].nonstop == null;
                });
            }

            if (filterdarray.length != 0 && aryDeparturelength != 0) {
                filterdarray = $.map(aryDeparture, function (aryDeparturevalue, index) {
                    arydeptminmax = [];
                    arydeptminmax = aryDeparturevalue.split('-');
                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        return arydeptminmax.length > 0 && Number(value[0].Dep.split(' ')[3].replace(':', '')) >= Number(arydeptminmax[0]) && Number(value[0].Dep.split(' ')[3].replace(':', '')) <= Number(arydeptminmax[1]);
                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });
            }



            if (filterdarray.length != 0 && aryArrivallength != 0) {
                filterdarray = $.map(aryArrival, function (aryArrivalvalue, index) {
                    aryarrivminmax = [];
                    aryarrivminmax = aryArrivalvalue.split('-');
                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        return aryarrivminmax.length > 0 && Number(value[value.length - 1].Arr.split(' ')[3].replace(':', '')) >= Number(aryarrivminmax[0]) && Number(value[value.length - 1].Arr.split(' ')[3].replace(':', '')) <= Number(aryarrivminmax[1]);

                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });
            }


            if (filterdarray.length != 0 && arystopslength != 0) {
                filterdarray = $.map(arystops, function (arystopsvalue, index) {

                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        if (arystopsvalue.indexOf('+') != -1) {
                            var strarystopsvalue = Number(arystopsvalue.replace('+', ''));
                            return value.length > 1 ? Number(value[0].Stops) > strarystopsvalue && Number(value[value.length - 1].Stops) > strarystopsvalue : Number(value[0].Stops) > strarystopsvalue;
                        }
                        return value.length > 1 ? value[0].Stops == arystopsvalue && value[value.length - 1].Stops == arystopsvalue : value[0].Stops == arystopsvalue;
                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });
            }

            if (filterdarray.length != 0 && aryAirlinecategorylength != 0) {
                filterdarray = $.map(aryAirlinecategory, function (aryAirlinevalue, index) {
                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        return value[0].Fno.split(' ')[0] == aryAirlinevalue.toUpperCase();
                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });
            }
            if (filterdarray.length != 0 && aryFlyTyplength != 0) {
                filterdarray = $.map(aryFlightType, function (aryFlightTypevalue, index) {
                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        return value[0].acat.split("SpLitPResna")[0] == aryFlightTypevalue.toUpperCase();
                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });


            }

            if (filterdarray.length != 0 && aryRefundlength != 0) {
                filterdarray = $.map(aryRefund, function (aryRefundvalue, index) {
                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        var aryRefundvalue_s = aryRefundvalue == "refun" ? "TRUE" : "FALSE";
                        return value[0].Refund == aryRefundvalue_s;
                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });


            }
            /*Fare type filter*/
            if (filterdarray.length != 0 && aryFaretypfilterlength != 0) {
                filterdarray = $.map(aryFaretypfilter, function (aryfaretype, index) {
                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        return value[0].FareType == aryfaretype.toUpperCase();
                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });
            }


            if (filterdarray.length < 1) {
                showerralert("We couldn't find flights to match your filters", "", "");
                return false;
            }
            else {

                scrollLock = true;
                FilterLock = false;

                filterdarray.sort(function (a, b) {
                    if ($("#hdn_AvailFormat").val() == "NAT") {
                        return a[0].NETFare - b[0].NETFare;
                    }
                    else {
                        return a[0].GFare - b[0].GFare;
                    }
                });

                if (loopCnt > 1) {
                    Main_totalfilterdarray.push(filterdarray);
                    if (triptp != "O" || triptp != "Y" || (triptp != "M" && segtyp != "I")) {
                        $('#' + 'availset_' + triptp + '_' + loop).scrollTop(0);
                    }

                }
                else {
                    if (filterdarray["0"]["0"].bindside_6_arg == loop) {
                        Main_totalfilterdarray.push(filterdarray);
                        var speratearr = $.grep(groupmain_array, function (value, index) {
                            return value.bindside_6_arg == loop + 1;
                        });
                        var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                            previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                            previousValue[currentValue.flightid].push(currentValue);
                            return previousValue;
                        }, {});
                        var grouparray = [];
                        for (var obj in groupedfli) {
                            grouparray.push(groupedfli[obj]);
                        }
                        grouparray.sort(function (a, b) {
                            if ($("#hdn_AvailFormat").val() == "NAT") {
                                return a[0].NETFare - b[0].NETFare;
                            }
                            else {
                                return a[0].GFare - b[0].GFare;
                            }

                        });  //fare soring after array :srinath 
                        Main_totalfilterdarray.push(grouparray);
                    }
                    if (filterdarray["0"]["0"].bindside_6_arg == loop + 1) {
                        var speratearr = $.grep(groupmain_array, function (value, index) {
                            return value.bindside_6_arg == loop;
                        });
                        var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                            previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                            previousValue[currentValue.flightid].push(currentValue);
                            return previousValue;
                        }, {});
                        var grouparray = [];
                        for (var obj in groupedfli) {
                            grouparray.push(groupedfli[obj]);
                        }
                        grouparray.sort(function (a, b) {
                            if ($("#hdn_AvailFormat").val() == "NAT") {
                                return a[0].NETFare - b[0].NETFare;
                            }
                            else {
                                return a[0].GFare - b[0].GFare;
                            }
                        });  //fare soring after array :srinath
                        Main_totalfilterdarray.push(grouparray);
                        Main_totalfilterdarray.push(filterdarray);

                    }

                }
                var groupedorgdes = filterdarray.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                    return previousValue;
                }, {});

                var grouparray_orgdes = [];
                for (var obj in groupedorgdes) {
                    grouparray_orgdes.push(groupedorgdes[obj]);
                }

                if (triptp == "O" || triptp == "Y" || (triptp == "M" && segtyp == "I")) {

                    if (assignedcountry == "EG") {
                        $('#spnActlFAvailCnt').html(filterdarray.length);
                        $("#availset_O_1").html('');
                    }
                    else {
                        $('#spnActlFAvailCnt').html(grouparray_orgdes.length);
                        $("#availset_O_1").html('');
                    }

                }
                else {

                    if (assignedcountry == "EG") {
                        $('#spnActlFAvailCnt_' + basetripty + '_' + segCnt).html(filterdarray.length);
                        $("#availset_" + strTrip + "_" + segCnt + "").html('');
                    }
                    else {
                        $('#spnActlFAvailCnt_' + basetripty + '_' + segCnt).html(grouparray_orgdes.length);
                        $("#availset_" + strTrip + "_" + segCnt + "").html('');
                    }

                }

                Commonavailability_bindingfun(grouparray_orgdes, filterdarray.length);
            }

        }
        //if (filtercount === 0) {

        //}
    }
    else {

        var segCnt = segID.replace("filterBody_dynamic_", "");

        var triptp = $("#hdtxt_trip")[0].value;


        var segtyp = $('body').data('segtype');
        var loopCnt = 1; //For Direct and Non stop flight filteration the loop count ll increase depends on Trip type... by saranraj on 20170531...
        if (commflg == "1") {  //For Not Stop Flights or Direct Flights...
            loopCnt = 2;
        }
        $('#' + 'availset_' + triptp + '_' + segCnt).scrollTop(0);

        Main_totalfilterdarray = [];
        Main_totalfilterdarray_ret = [];
        for (var loop = 1; loop <= loopCnt; loop++) {
            if (commflg == "1") {  //For Not Stop Flights or Direct Flights...
                segCnt = loop;
            }
            if (flgtype != null && flgtype != "" && matarray != null && matarray != "") {
                var grouparray = [];
                grouparray = matarray;
            }
            else {
                var speratearr = "";
                if (segCnt == "1") {
                    speratearr = $.grep(groupmain_array, function (value, index) {
                        return value.bindside_6_arg == "1";
                    });
                }
                else {
                    speratearr = $.grep(groupmain_ret, function (value, index) {
                        return value.bindside_6_arg == "2";
                    });
                }
                var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                    previousValue[currentValue.flightid].push(currentValue);
                    return previousValue;
                }, {});
                var grouparray = [];
                for (var obj in groupedfli) {
                    grouparray.push(groupedfli[obj]);
                }
                grouparray.sort(function (a, b) {
                    if ($("#hdn_AvailFormat").val() == "NAT") {
                        return a[0].NETFare - b[0].NETFare;
                    }
                    else {
                        return a[0].GFare - b[0].GFare;
                    }
                });  //fare soring after array :srinath 

            }
            var minval = $("#minfare_dynamic_" + segCnt + "").data('min') != "" ? parseInt($("#minfare_dynamic_" + segCnt + "").data('min')) : 0;
            var maxval = $("#maxfare_dynamic_" + segCnt + "").data('max') != "" ? parseInt($("#maxfare_dynamic_" + segCnt + "").data('max')) : 0;
            aryDeparture = [];
            aryArrival = [];
            arystops = [];
            aryAirlinecategory = [];
            aryRefund = [];
            aryFlightType = [];
            aryDeparture_matrix = [];

            /*Departure Filteration*/
            $("#" + segID + " .chkfilterdept").each(function () {
                if ($(this)[0].checked == true) {
                    if ($(this).data('filter') == "dept1")
                        aryDeparture.push("0000-0600");
                    else if ($(this).data('filter') == "dept2")
                        aryDeparture.push("0601-1200");
                    else if ($(this).data('filter') == "dept3")
                        aryDeparture.push("1201-1800");
                    else if ($(this).data('filter') == "dept4")
                        aryDeparture.push("1801-2400");
                }



            });

            $(".detafil").each(function () {
                if ($(this)[0].checked == true) {
                    if ($(this).data('filter') == "dept1")
                        aryDeparture.push("0000-0600");
                    else if ($(this).data('filter') == "dept2")
                        aryDeparture.push("0601-1200");
                    else if ($(this).data('filter') == "dept3")
                        aryDeparture.push("1201-1800");
                    else if ($(this).data('filter') == "dept4")
                        aryDeparture.push("1801-2400");
                }
            });


            /*Arrival Filteration*/
            $("#" + segID + " .chkfilterarri").each(function () {
                if ($(this)[0].checked == true) {
                    if ($(this).data('filter') == "arri1")
                        aryArrival.push("0000-0600");
                    else if ($(this).data('filter') == "arri2")
                        aryArrival.push("0601-1200");
                    else if ($(this).data('filter') == "arri3")
                        aryArrival.push("1201-1800");
                    else if ($(this).data('filter') == "arri4")
                        aryArrival.push("1801-2400");
                }
            });

            /*Stops Filteration*/
            $("#" + segID + " .chkfilterstop").each(function () {
                if ($(this)[0].checked == true && $(this)[0].disabled == false) {
                    arystops.push($(this).data('filter').replace("stop", ""));
                }
            });

            $(".stopChk_all").each(function () {
                if ($(this)[0].checked == true && $(this)[0].disabled == false) {
                    arystops.push($(this).data('filter').replace("stop", ""));
                }
            });


            if ($('#chkDirFlt').is(':checked')) {  //For Direct Flights...
                arystops.push('0');
            }
            var nostop = "0";
            if ($('#chkNonstpFlt').is(':checked')) {  //For Non-Stop Flights...
                nostop = "1";
                arystops.push('0');
            }

            /*Refund Filteration*/
            $("#" + segID + " .chkfilterrefund").each(function () {
                if ($(this)[0].checked == true && $(this)[0].disabled == false) {
                    aryRefund.push($(this).data('filter'));
                }
                if (aryRefund.length === 2) {
                    aryRefund.push("");
                }
            });

            $(" .Refundablefil").each(function () {
                if ($(this)[0].checked == true && $(this)[0].disabled == false) {
                    aryRefund.push($(this).data('filter'));
                }
                if (aryRefund.length === 2) {
                    aryRefund.push("");
                }
            });




            /*Flight Type Filteration*/
            $("#" + segID + " .chkfilterflttyp").each(function () {
                if ($(this)[0].checked == true && $(this)[0].disabled == false) {
                    aryFlightType.push($(this).data('filter'));
                }
                if (aryFlightType.length === 2) {
                    aryFlightType.push("");
                }
            });

            /*Flight Filteration*/
            $("#" + segID + " .chkfilterplane").each(function () {
                if ($(this)[0].checked == true) {
                    aryAirlinecategory.push($(this).data('filter').replace("air", ""));
                    if ($(this).data('filter').replace("air", "") == "AK") {
                        aryAirlinecategory.push("I5");
                        aryAirlinecategory.push("FD");
                        aryAirlinecategory.push("D7");
                    }
                    else if ($(this).data('filter').replace("air", "") == "2T") {
                        aryAirlinecategory.push("TK");
                    }
                }
            });

            /*Fare Filteration*/
            $("#" + segID + " .chkfilterfaretype").each(function () {
                if ($(this)[0].checked == true) {
                    aryFaretypfilter.push($(this).data('filter').replace("faretype", ""));
                }
            });


            var amt = 0;
            var refund = "";
            var depttime = "";
            var arrivtime = "", stops = "", via = "", airline = "", airline_typ = "", arydeptminmax = [], aryarrivminmax = [], noofstops = [];
            var showFlag = false;
            var i = 0;
            var aryDeparturelength = aryDeparture.length, aryArrivallength = aryArrival.length, arystopslength = arystops.length, aryRefundlength = aryRefund.length, aryFlyTyplength = aryFlightType.length, aryAirlinecategorylength = aryAirlinecategory.length, aryFaretypfilterlength = aryFaretypfilter.length;
            var basetripty = (triptp == "Y" || (triptp == "M" && segtyp == "I")) ? "O" : triptp;
            filtercount = 0;
            filterdarray = [];
            filterdarray = $.grep(grouparray, function (value, item) {
                if ($("#hdn_AvailFormat").val() == "NAT") {
                    //return a[0].NETFare - b[0].NETFare;
                    return parseInt(value[0].NETFare) >= Number(minval) && parseInt(value[0].NETFare) <= Number(maxval);
                }
                else {
                    return parseInt(value[0].GFare) >= Number(minval) && parseInt(value[0].GFare) <= Number(maxval);
                }

            });
            if ($('#chkNonstpFlt').is(':checked')) {
                filterdarray = $.grep(grouparray, function (value, item) {
                    return value[0].nonstop == "" || value[0].nonstop == null;
                });
            }

            if (filterdarray.length != 0 && aryDeparturelength != 0) {
                filterdarray = $.map(aryDeparture, function (aryDeparturevalue, index) {
                    arydeptminmax = [];
                    arydeptminmax = aryDeparturevalue.split('-');
                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        return arydeptminmax.length > 0 && Number(value[0].Dep.split(' ')[3].replace(':', '')) >= Number(arydeptminmax[0]) && Number(value[0].Dep.split(' ')[3].replace(':', '')) <= Number(arydeptminmax[1]);
                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });
            }



            if (filterdarray.length != 0 && aryArrivallength != 0) {
                filterdarray = $.map(aryArrival, function (aryArrivalvalue, index) {
                    aryarrivminmax = [];
                    aryarrivminmax = aryArrivalvalue.split('-');
                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        return aryarrivminmax.length > 0 && Number(value[value.length - 1].Arr.split(' ')[3].replace(':', '')) >= Number(aryarrivminmax[0]) && Number(value[value.length - 1].Arr.split(' ')[3].replace(':', '')) <= Number(aryarrivminmax[1]);

                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });
            }


            if (filterdarray.length != 0 && arystopslength != 0) {
                filterdarray = $.map(arystops, function (arystopsvalue, index) {

                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        if (arystopsvalue.indexOf('+') != -1) {
                            var strarystopsvalue = Number(arystopsvalue.replace('+', ''));
                            return value.length > 1 ? Number(value[0].Stops) > strarystopsvalue && Number(value[value.length - 1].Stops) > strarystopsvalue : Number(value[0].Stops) > strarystopsvalue;
                        }
                        return value.length > 1 ? value[0].Stops == arystopsvalue && value[value.length - 1].Stops == arystopsvalue : value[0].Stops == arystopsvalue;
                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });
            }

            if (filterdarray.length != 0 && aryAirlinecategorylength != 0) {
                filterdarray = $.map(aryAirlinecategory, function (aryAirlinevalue, index) {
                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        return value[0].Fno.split(' ')[0] == aryAirlinevalue.toUpperCase();
                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });
            }
            /*Fare type filter*/
            if (filterdarray.length != 0 && aryFaretypfilterlength != 0) {
                filterdarray = $.map(aryFaretypfilter, function (aryfaretype, index) {
                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        return value[0].FareType == aryfaretype.toUpperCase();
                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });
            }
            if (filterdarray.length != 0 && aryFlyTyplength != 0) {
                filterdarray = $.map(aryFlightType, function (aryFlightTypevalue, index) {
                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        return value[0].acat.split("SpLitPResna")[0] == aryFlightTypevalue.toUpperCase();
                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });


            }

            if (filterdarray.length != 0 && aryRefundlength != 0) {
                filterdarray = $.map(aryRefund, function (aryRefundvalue, index) {
                    arrayfil = [];
                    arrayfil = $.grep(filterdarray, function (value, item) {
                        var aryRefundvalue_s = aryRefundvalue == "refun" ? "TRUE" : "FALSE";
                        return value[0].Refund == aryRefundvalue_s;
                    });
                    if (arrayfil.length > 0) {
                        return arrayfil
                    }
                });


            }

            if (filterdarray.length < 1) {
                showerralert("We couldn't find flights to match your filters", "", "");
                return false;
            }
            else {

                scrollLock = true;
                FilterLock = false;

                filterdarray.sort(function (a, b) {
                    if ($("#hdn_AvailFormat").val() == "NAT") {
                        return a[0].NETFare - b[0].NETFare;
                        //return value[0].NETFare >= Number(minval) && value[0].NETFare <= Number(maxval);
                    }
                    else {
                        return a[0].GFare - b[0].GFare;
                    }
                });


                if (filterdarray["0"]["0"].bindside_6_arg == "1") {
                    Main_totalfilterdarray.push(filterdarray);
                    var speratearr = $.grep(groupmain_array, function (value, index) {
                        return value.bindside_6_arg == "1";
                    });
                    var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                        previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                        previousValue[currentValue.flightid].push(currentValue);
                        return previousValue;
                    }, {});
                    var grouparray = [];
                    for (var obj in groupedfli) {
                        grouparray.push(groupedfli[obj]);
                    }
                    grouparray.sort(function (a, b) {
                        if ($("#hdn_AvailFormat").val() == "NAT") {
                            return a[0].NETFare - b[0].NETFare;
                            //return value[0].NETFare >= Number(minval) && value[0].NETFare <= Number(maxval);
                        }
                        else { return a[0].GFare - b[0].GFare; }
                    });  //fare soring after array :srinath 
                    Main_totalfilterdarray.push(grouparray);
                }
                if (filterdarray["0"]["0"].bindside_6_arg == "2") {
                    var speratearr = $.grep(groupmain_ret, function (value, index) {
                        return value.bindside_6_arg == "2";
                    });
                    var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                        previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                        previousValue[currentValue.flightid].push(currentValue);
                        return previousValue;
                    }, {});
                    var grouparray = [];
                    for (var obj in groupedfli) {
                        grouparray.push(groupedfli[obj]);
                    }
                    grouparray.sort(function (a, b) {
                        if ($("#hdn_AvailFormat").val() == "NAT") {
                            return a[0].NETFare - b[0].NETFare;
                            //return value[0].NETFare >= Number(minval) && value[0].NETFare <= Number(maxval);
                        }
                        else {
                            return a[0].GFare - b[0].GFare;
                        }
                    });  //fare soring after array :srinath
                    Main_totalfilterdarray.push(filterdarray);
                    Main_totalfilterdarray.push(grouparray);

                }


                var groupedorgdes = filterdarray.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                    return previousValue;
                }, {});

                var grouparray_orgdes = [];
                for (var obj in groupedorgdes) {
                    grouparray_orgdes.push(groupedorgdes[obj]);
                }

                $('#spnActlFAvailCnt_' + basetripty + '_' + segCnt).html(grouparray_orgdes.length);
                $("#availset_" + strTrip + "_" + segCnt + "").html('');

                Commonavailability_bindingfun(grouparray_orgdes, segCnt);
            }

        }
    }
}
$("#btnNext").on('click', function (e) {

    var dep = "";
    if ($("#hdtxt_trip")[0].value != "M") {
        if (FLAG == "M") {
            dep = departuredate;
        }
        else {
            dep = $("#txtdeparture").val();
        }

        //var dep = $("#txtdeparture").val();
        var currentDate = $("#hdtxt_depa_date")[0].value;
        var ArrivalCurrentDate = $("#hdtxt_Arrivedate")[0].value;
        var day = currentDate.split('/');
        var newdate = moment([day[2], day[1] - 1, day[0]]).add(1, 'days').format("DD/MM/YYYY");
        var ardate = $("#hdtxt_trip")[0].value == "R" || $("#hdtxt_trip")[0].value == "Y" ? ArrivalCurrentDate : "-";
        var arrivaldate = ArrivalCurrentDate.split('/');
        var newArrivaldate = moment([arrivaldate[2], arrivaldate[1] - 1, arrivaldate[0]]).add(1, 'days').format("DD/MM/YYYY");
        if (FLAG != "M") {
            $("#txtdeparture").val(newdate != "Invalid date" ? newdate : dep);
            $("#txtarrivaldate").val(newdate != "Invalid date" ? newArrivaldate : ArrivalCurrentDate);
            $("#btnFmodifySearch").click(); //To Show Search box...
            dataproperty_array = [];//Added by srinath on 20171006 to improve peroformance
            extrahidem_dataproperty_array = [];//Added by srinath on 20171006 to improve peroformance
            groparr = [];
            groupmain_array = [];
            groupmain_ret = [];
            filterdarray = [];
            var obj1 = {
                count: 0,
                setcount: 0
            };
            Main_totalfilterdarray = [];
            Main_totalfilterdarray_ret = [];
            $(".airfilterallcheck").html("");
            $("#airline_matrixhtml").html("");
            $(".faretypfilterallcheck").html("");
            allAIRLINECode = "";
            Faretypefilter = "";
            btn_Search("D");
        }
        else {
            departuredate = newdate != "" ? newdate : dep;
            arrivaldate = newdate != "" ? newArrivaldate : ArrivalCurrentDate;
            search();
        }

    }
});

$("#btnPrev").on('click', function (e) {
    var dep = "";
    if ($("#hdtxt_trip")[0].value != "M") {
        //var dep = $("#txtdeparture").val();
        if (FLAG == "M") {
            dep = departuredate;
        }
        else {
            dep = $("#txtdeparture").val();
        }
        var currentDate = $("#hdtxt_depa_date")[0].value;
        var ArrivalCurrentDate = $("#hdtxt_Arrivedate")[0].value;
        var day = currentDate.split('/');
        var newdate = moment([day[2], day[1] - 1, day[0]]).subtract(1, 'days').format("DD/MM/YYYY");
        var ardate = $("#hdtxt_trip")[0].value == "R" || $("#hdtxt_trip")[0].value == "Y" ? ArrivalCurrentDate : "-";
        var arrivaldate = ArrivalCurrentDate.split('/');
        var newArrivaldate = moment([arrivaldate[2], arrivaldate[1] - 1, arrivaldate[0]]).subtract(1, 'days').format("DD/MM/YYYY");
        if (FLAG != "M") {
            $("#txtdeparture").val(newdate != "Invalid date" ? newdate : dep);
            $("#txtarrivaldate").val(newdate != "Invalid date" ? newArrivaldate : ArrivalCurrentDate);
            $("#btnFmodifySearch").click(); //To Show Search box...
            dataproperty_array = [];//Added by srinath on 20171006 to improve peroformance
            extrahidem_dataproperty_array = [];//Added by srinath on 20171006 to improve peroformance
            groparr = [];
            groupmain_array = [];
            groupmain_ret = [];
            filterdarray = [];
            var obj1 = {
                count: 0,
                setcount: 0
            };
            Main_totalfilterdarray = [];
            Main_totalfilterdarray_ret = [];
            $(".airfilterallcheck").html("");
            $("#airline_matrixhtml").html("");
            $(".faretypfilterallcheck").html("");
            allAIRLINECode = "";
            Faretypefilter = "";
            btn_Search("D");
        }
        else {
            departuredate = newdate != "" ? newdate : dep;
            arrivaldate = newdate != "" ? newArrivaldate : ArrivalCurrentDate;
            search();
        }


    }
});

// 

$('.clsern-show-hide').change(function () {
    if ($('#liern-show').is(':checked')) {
        $('.cls_showearning').show();
        $('#spnEarnsort').show();
    }
    else {
        $('.cls_showearning').hide();
        $('#spnEarnsort').hide('slow');
    }
});

$('.clsCommNFare').click(function () {
    if ($('#chkCommNFare').is(':checked')) {
        $('.cls_showearning').show();
        $('#spnEarnsort').show();
    }
    else {
        $('.cls_showearning').hide();
        $('#spnEarnsort').hide('slow');
    }
});

function checkoutGrossFare(to2) {
    if ($(window).width() >= 768) {
        document.getElementById('taxtdiv' + to2).style.display = "none"
    }
}

function CheckGrossFare(n) {
    if ($(window).width() >= 768) {
        $("#lblGFare" + n).mousemove(function (t) {
            if (($("#hdtxt_trip")[0].value == "M" && $('body').data('segtype') == "D")) {
                $("#taxtdiv" + n).css({ display: "block", 'margin-left': "-115px", 'margin-top': "10px" });
            }
            else {
                if ($("#hdn_producttype").val() == "JOD") {
                    $("#taxtdiv" + n).css({ display: "block", left: t.screenX - 51 + "px", top: t.screenY - 248 + "px" });
                }
                else {
                    $("#taxtdiv" + n).css({ display: "block", left: t.screenX - 51 + "px", top: t.screenY - 58 + "px" });
                }

            }
        });
    }
}

$('body').on('click', 'span[id^="lblGFare"]', function () {
    if ($(window).width() < 768) {
        var n = this.id.replace('lblGFare', '');
        if (($("#hdtxt_trip")[0].value == "M" && $('body').data('segtype') == "D")) {
            if (document.getElementById("taxtdiv" + n).style.display == "none" || document.getElementById("taxtdiv" + n).style.display == "")
                $("#taxtdiv" + n).css({ display: "block", 'margin-left': "-115px", 'margin-top': "10px", 'position': "absolute" });
            else
                document.getElementById('taxtdiv' + n).style.display = "none"
        }
        else if (($("#hdtxt_trip")[0].value == "R")) {
            if (document.getElementById("taxtdiv" + n).style.display == "none" || document.getElementById("taxtdiv" + n).style.display == "")
                $("#taxtdiv" + n).css({ display: "block", 'margin-top': "35px", 'margin-left': "2%", 'width': "90%", 'position': "absolute" });
            else
                document.getElementById('taxtdiv' + n).style.display = "none"
        }
        else {
            if (document.getElementById("taxtdiv" + n).style.display == "none" || document.getElementById("taxtdiv" + n).style.display == "")
                $("#taxtdiv" + n).css({ display: "block", 'margin-top': "35px", 'margin-left': "30%", 'position': "absolute" });
            else
                document.getElementById('taxtdiv' + n).style.display = "none"
        }
    }
});

$("#btnFAvailEmail").click(function (e) {

    var c = $('.SelecCheckbox'); //document.getElementsByName('SelectCheckbox')
    var Select_flight = "";
    var Selectflightsend = "";
    var idd = "";
    var triptp = $("#hdtxt_trip")[0].value;
    var segtyp = $('body').data('segtype');
    var loopCnt = (triptp == "O" || triptp == "Y" || (triptp == "M" && segtyp == "I")) ? 1 : triptp == "R" ? 2 : aryOrgMul.length;
    var basetripty = (triptp == "Y" || (triptp == "M" && segtyp == "I")) ? "O" : triptp;
    for (var seg = 1; seg <= loopCnt; seg++) {
        var c = $('#availset_' + basetripty + '_' + seg + ' .SelecCheckbox');
        for (var i = 0; i < c.length; i++) {
            if (c[i].checked === true) {
                var litaghtml = c[i].value;
                idd += $(c[i]).attr('data-val') + "SpRaj";
                Selectflightsend += litaghtml + "SpRaj";
                printlength++;
            }
        }
        if (seg != loopCnt) {
            idd += "SpRaj"; //For Segment Difference split... by saranraj...
            Selectflightsend += "SpRaj";//For Segment Difference split... by saranraj...
        }
    }
    if (printlength == 0) {
        showerralert("Please Select atleast one flight.", 5000, "");
        return false;
    }
    var Strflights = "";
    var aryFlightsDetails = Selectflightsend.split('SpRaj');
    var aryfligidd = idd.split('SpRaj');

    var Strflights = "";
    var aryFlightsDetails = Selectflightsend.split('SpRaj');
    var aryfligidd = idd.split('SpRaj');
    Strflights += "<div id='PrinTicket1' style='overflow-y:auto; max-height:380px;'><div style='text-align:right;'><label style='color:red;font-size:13px'>*All Fares in:" + assignedcurrency + "</label></div>";

    Strflights += "<table border='1' style='width:100%;font-size:12px;border-collapse:collapse;border-color: #ccc;border: 1px solid #ccc;'>";
    if (triptp == "Y") {
        Strflights += "<tr class='bg_clrtxt' style='background-color:#2d3e52;color:#fff;height: 35px;'><td colspan='8' style='text-align: center'><lable style='font-size:16px;'><b>" + $("#hdtxt_origin")[0].value + " - " + $("#hdtxt_destination")[0].value + " - " + $("#hdtxt_origin")[0].value + "</b></lable></td></tr>";
    }
    else {
        Strflights += "<tr class='bg_clrtxt' style='background-color:#2d3e52;color:#fff;height: 35px;'><td colspan='8' style='text-align: center'><lable style='font-size:16px;'><b>" + $("#hdtxt_origin")[0].value + " - " + $("#hdtxt_destination")[0].value + "</b></lable></td></tr>";
    }
    Strflights += "<tr><td colspan='4'><label style='margin-left: 20%;'>From&nbsp;Mail</label></td><td colspan='4' style='padding: 1.5% !important;'><input type='text' style='margin-left: 5%;width: 80%;' class='txt-anim' id='txtmailFrom' onkeypress='return AvoidSpace();' maxlength='50'/></td></tr>";
    Strflights += "<tr><td colspan='4'><label style='margin-left: 20%;'>Subject</label></td><td colspan='4' style='padding: 1.5% !important;'><input type='text' style='margin-left: 5%;width: 80%;' class='txt-anim' id='txtSubject' value='Flight Availability' maxlength='50'/></td></tr>";
    Strflights += "<tr><td colspan='4'><label style='margin-left: 20%;'>To&nbsp;Mail</label></td><td colspan='4' style='padding: 1.5% !important;'><input type='text' class='txt-anim' style='margin-left: 5%;width: 80%;' id='txtToEmail' onkeypress='return AvoidSpace();' maxlength='50' /></td></tr>";
    Strflights += "<tr style='background-color:#f4f4f4'><th style='text-align:center;padding:0 20px;'>Airline</th><th style='text-align:center;padding:0 22px;'>Departure</th><th style='text-align:center; padding:0 35px;'>Arrival</th><th style='text-align:center'>  </th><th style='text-align:center; padding:0 20px;'>Class</th><th style='text-align:center; padding:0 20px;'>Basic&nbsp;Fare</th><th style='text-align:center; padding:0 20px;'>Gross</th><th style='text-align:center; padding:0 20px;'>Baggage</th></tr>";
    for (var i = 0; i < aryFlightsDetails.length - 1; i++) {
        var imgpath = $('#fliimage' + aryfligidd[i]).attr("src");

        if (aryFlightsDetails[i] != "") {
            if (aryFlightsDetails[i].split("SpLITSaTIS").length > 1) {
                var Connflight = aryFlightsDetails[i].split("SpLITSaTIS");
                var totalduration = 0;
                for (var _dur = 0; _dur < Connflight.length; _dur++) {
                    totalduration += parseInt((Connflight[_dur].split("SpLitPResna")[22]))
                }
                for (var j = 0; j < Connflight.length; j++) {
                    Strflights += "<tr>";
                    imgpath = Connflight[j].split("SpLitPResna")[18].substring(0, 2);
                    Strflights += "<td style='text-align:center'>" + Connflight[j].split("SpLitPResna")[18] + "<br /><img src='" + airlinelogourl + "/" + imgpath + ".png?" + Versionflag + "'></td>";
                    Strflights += "<td style='text-align:center'>" + Connflight[j].split("SpLitPResna")[0] + "<br>" + Connflight[j].split("SpLitPResna")[2] + "</td>";
                    Strflights += "<td style='text-align:center'>" + Connflight[j].split("SpLitPResna")[1] + "<br>" + Connflight[j].split("SpLitPResna")[3] + "</td>";
                    if (j == 0) {
                        Strflights += "<td style='text-align:center;vertical-align:central' rowspan='" + Connflight.length + "'>" + parseInt(totalduration / 60) + "h:" + Math.round(totalduration % 60) + "m" + "</td>";
                    }
                    Strflights += "<td style='text-align:center'>" + Connflight[j].split("SpLitPResna")[5] + "-" + Connflight[j].split("SpLitPResna")[17] + "</td>";
                    if (j == 0) {
                        Strflights += "<td style='text-align:center' rowspan='" + Connflight.length + "'>" + Connflight[j].split("SpLitPResna")[13] + "</td>";
                        Strflights += "<td style='text-align:center' rowspan='" + Connflight.length + "'><input type='text' class='txt-anim' id='edit_gross_" + i + "' onkeypress='javascript:return isNumericVal(event);'  style='text-align:right;width: 80%;' value='" + Connflight[j].split("SpLitPResna")[31] + "'/></td>";
                    }
                    var packagedet = "";
                    if ((((Connflight[j].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]) != null || (((Connflight[j].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]) != "") {
                        packagedet = (((Connflight[j].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]).split("SpLITSaTIS")[0];
                    }
                    else {
                        packagedet = "N/A";
                    }
                    Strflights += "<td style='text-align:center'>" + packagedet + "</td>";
                    Strflights += "</tr>";
                }
            }
            else {
                imgpath = aryFlightsDetails[i].split("SpLitPResna")[18].substring(0, 2);
                Strflights += "<tr>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[18] + "<br /><img src='" + airlinelogourl + "/" + imgpath + ".png?" + Versionflag + "'></td>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[0] + "<br />" + aryFlightsDetails[i].split("SpLitPResna")[2] + "</td>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[1] + "<br />" + aryFlightsDetails[i].split("SpLitPResna")[3] + "</td>";
                if (aryFlightsDetails[i].split("SpLitPResna")[22] != null || aryFlightsDetails[i].split("SpLitPResna")[22] != "") {
                    Strflights += "<td style='text-align:center'>" + parseInt((aryFlightsDetails[i].split("SpLitPResna")[22]) / 60) + "h:" + Math.round((parseInt(aryFlightsDetails[i].split("SpLitPResna")[22]) % 60)) + "m" + "</td>";
                }
                else {
                    Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[22] + "</td>";
                }
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[5] + "-" + aryFlightsDetails[i].split("SpLitPResna")[17] + "</td>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[13] + "</td>";
                Strflights += "<td style='text-align:center'><input type='text' id='edit_gross_" + i + "' class='_M00cross txt-anim' onkeypress='javascript:return isNumericVal(event);' style='text-align:right;width: 80%;' value='" + aryFlightsDetails[i].split("SpLitPResna")[31].replace("SpLITSaTIS", "") + "'/></td>";
                var packagedet = "";
                if ((((aryFlightsDetails[i].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]) != null || (((aryFlightsDetails[i].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]) != "") {
                    packagedet = (((aryFlightsDetails[i].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]).split("SpLITSaTIS")[0];
                }
                else {
                    packagedet = "N/A";
                }
                Strflights += "<td style='text-align:center'>" + packagedet + "</td>";
                Strflights += "</tr>";
            }
        }

    }
    Strflights += "</table>";
    Strflights += "</div>";
    //Print Button area by saranraj on 20170516...
    Strflights += '<hr><div class="row">';
    Strflights += '<div class="col-md-offset-3 col-md-3 col-xs-6" style="margin-bottom: 15px;">';
    Strflights += '<button type="button" class="btn btn-md btn-primary width100per" id="btnOKPrint" onclick="javascript: return SendMail(' + aryFlightsDetails.length + ');">SendMail<i id="SendMailLoading" class="fa fa-spinner fa-spin" style="font-size:20px;color:#292929;display:none;float: right;"></i></button>';//<span class="spnbtnicon"><i class="fa fa-print"></i></span> 
    Strflights += '</div>';
    Strflights += '<div class="col-md-3 col-xs-6" style="margin-bottom: 15px;">';
    Strflights += '<button type="button" class="btn btn-md btn-danger width100per" id="btnClosePrint" onclick="javascript: return backingtoprint();">Cancel</button>';//<span class="spnbtnicon"><i class="fa fa-close"></i></span>
    Strflights += '</div>';
    Strflights += '</div>';
    //Print Button area by saranraj on 20170516 End...

    document.getElementById("FlightsString").value = Selectflightsend;
    ////$("#hdn_availsendmail").val(Strflights);

    //alert(Strflights);
    //alert($("#hdn_availsendmail").val());

    showpopuplogin("Send Mail", "", Strflights, true, 850, "");


    //$("#modal-SendAvailmail").modal({
    //    backdrop: 'static',
    //    keyboard: false
    //});

});


function SendMail(totlength) {
    var bValid = $("#txtToEmail").val();
    var fromvalid = $("#txtmailFrom").val(); 
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    var errPdiv = "dvavailmailerr", errdiv_msg = "dvavailmailerr_msg";
    if (fromvalid.trim() == "") {
        showerralert('Please Enter From email address', "", ""); 
        return false;
    }
    if (bValid.trim() == "") {
        showerralert('Please Enter To email address', "", "");
                return false;
    }
    if (!filter.test(bValid)) {
        showerralert('Please provide a valid email address', "", ""); 
        return false;
    }
    if (!filter.test(fromvalid)) {
        showerralert('Please provide a valid email address', "", ""); 
        return false;
    }

    var Tomailid = $("#txtToEmail").val(); 
    var Frommailid = $("#txtmailFrom").val();

    var loadvalues = "";
    for (var i = 0; i < totlength - 1; i++) {
        if ($("#edit_gross_" + i).val() != null && $("#edit_gross_" + i).val() != "" && $("#edit_gross_" + i).val() != "0") {
            loadvalues += $("#edit_gross_" + i).val() + "SplitFare";
        }
    }
    if (loadvalues == "") {
        showerralert("Please enter valid fare for flight " + i + 1, 5000, "");
        return false;
    }

    $("#hdn_availsendmail").val(loadvalues);
    Availmailsendings(Tomailid, Frommailid);
    
}

$("#btnAvailwhtsapp11").click(function (e) {

    var checlength = 0;
    var Select_flight = "";
    var Selectflightsend = "";
    var triptp = $("#hdtxt_trip")[0].value;
    var segtyp = $('body').data('segtype');
    var loopCnt = (triptp == "O" || triptp == "Y" || (triptp == "M" && segtyp == "I")) ? 1 : triptp == "R" ? 2 : aryOrgMul.length;
    var basetripty = (triptp == "Y" || (triptp == "M" && segtyp == "I")) ? "O" : triptp;
    for (var seg = 1; seg <= loopCnt; seg++) {
        var c = $('#availset_' + basetripty + '_' + seg + ' .SelecCheckbox');
        for (var i = 0; i < c.length; i++) {
            if (c[i].checked == true) {
                var litaghtml = c[i].value;
                Selectflightsend += litaghtml + "SpRaj";
                checlength++;
            }
        }
        if (seg != loopCnt) {
            Selectflightsend += "SpRaj";//For Segment Difference split... by saranraj...
        }
    }
    document.getElementById("FlightswhatsappString").value = Selectflightsend;
    if (checlength == 0) {
        showerralert("please select anyone flight.", 5000, "");
        return false;
    }
        //else if (checlength > 1) {
        //    showerralert("please select only one flight.", 5000, "");
        //    return false;
        //}
    else {
        $("#txtwhatspno").val();
        $("#modal-SendWhatsapp").show();
        $("#modal-SendWhatsapp").modal({
            backdrop: 'static',
            keyboard: false
        });
    }
});


function Airportname(IDD) {
    try {
        Filtered = jQuery.grep(Cityairport, function (a) { return a['id'] == IDD; });
        return Filtered[0].value == "undefined" ? Filtered[0].id : Filtered[0].value;
    }
    catch (ex) {
    }
}

function Availmailsendings(Tomailid, Frommailid) {

    var triptp = $("#hdtxt_trip")[0].value;
    var segtyp = $('body').data('segtype');
    var loopCnt = (triptp == "O" || triptp == "Y" || (triptp == "M" && segtyp == "I")) ? 1 : triptp == "R" ? 2 : aryOrgMul.length;
    var basetripty = (triptp == "Y" || (triptp == "M" && segtyp == "I")) ? "O" : triptp;


    $('#ipopupLoading').show();
    $('#btnOKMAil').disabled = true;
    var s = "";
    var triptp = $("#hdtxt_trip")[0].value;
    var segtyp = $('body').data('segtype');
    if (triptp == "Y") {
        s += "<tr style='background-color:#fc8727;color:#fff;'><td colspan='8' style='text-align: center'><lable style='font-size:16px;'><b>" + $("#hdtxt_origin")[0].value + " - " + $("#hdtxt_destination")[0].value + " - " + $("#hdtxt_origin")[0].value + "</b></lable></td></tr>";
    }
    else if (triptp == "M" && segtyp == "I") {
        var connflts = "";
        for (var i = 0; i < aryOrgMul.length; i++) {
            connflts += aryOrgMul[i] + " - ";
        }
        connflts += aryDstMul[aryDstMul.length - 1];
        s += "<tr style='background-color:#fc8727;color:#fff;'><td colspan='8' style='text-align: center'><lable style='font-size:16px;'><b>" + connflts + "</b></lable></td></tr>";
    }
    $("#SendMailLoading").show();
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: FlightMailUrl, 		// Location of the service
        data: '{Toadd: "' + Tomailid + '" ,Subject: "' + $('#txtSubject').val() + '",Flights: "' + $('#FlightsString').val() + '",Origin: "' + $('#hdtxt_origin').val() + '",Destination: "' + $('#hdtxt_destination').val() + '",Frommail:"' + Frommailid + '",segloopcnt:"' + triptp + '",strhdr:"' + s + '",Changedfare:"' + $("#hdn_availsendmail").val() + '"}',//,CancelationStatus: "' + $("#hdnPax")[0].value + '" ,BlockTicket: "' + BlockTicket + '",PaymentMode: "' + $("#ddlPaymode").val() + '",TourCode: "' + $("#Tour_Code").val() + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call
            $('#ipopupLoading,#SendMailLoading').hide();
            $('#modal-Fare').iziModal('close');
            $('#btnOKMAil').disabled = false;
            var result = json.Result;
            if (json.Status == "-1") {
                window.location.href = sessionExb;
                return false;
            }
            else if (json.Status == "01") {
                $("#modal-SendAvailmail").modal('hide');
                $('#modal-Fare-new').iziModal('destroy');
                if (result[0] != "") {
                    showalert(result[0], "OK", "");
                }
                else {
                    showerralert(result[1] != "" ? result[1] : "Mail sending failure. Please contact customer care (#03).", "", "");
                }
            }
            else {
                $("#modal-SendAvailmail").modal('hide');
                $('#modal-Fare-new').iziModal('destroy');
                showerralert(result[1], "", "");
            }
        },
        error: function (e) {//On Successful service call 
            $('#ipopupLoading,#SendMailLoading').hide();
            $('#modal-Fare').iziModal('close');
            $('#btnOKMAil').disabled = false;
            if (e.status == "500") {
                window.location.href = sessionExb;
                return false;
            }
        }	// When Service call fails
    });

}


function Availmailsendings1() {

    var bValid = document.getElementById('txtToEmail');
    var fromvalid = document.getElementById('txtmailFrom');
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    var errPdiv = "dvavailmailerr", errdiv_msg = "dvavailmailerr_msg";
    if (fromvalid.value.trim() == "") {
        showError('Please Enter From email address', errPdiv, errdiv_msg, ""); //Message, Error Parent ID, Error showing ID, Focus ID...
        setTimeout(function () {
            fromvalid.focus();
        }, 300);
        return false;
    }
    if (bValid.value.trim() == "") {
        showError('Please Enter To email address', errPdiv, errdiv_msg, ""); //Message, Error Parent ID, Error showing ID, Focus ID...
        setTimeout(function () {
            bValid.focus();
        }, 300);
        return false;
    }
    if (!filter.test(bValid.value)) {
        //alert('Please provide a valid email address');
        showError('Please provide a valid email address', errPdiv, errdiv_msg, ""); //Message, Error Parent ID, Error showing ID, Focus ID...
        setTimeout(function () {
            bValid.focus();
        }, 300);
        return false;
    }
    if (!filter.test(fromvalid.value)) {
        // alert('Please provide a valid email address');
        showError('Please provide a valid email address', errPdiv, errdiv_msg, ""); //Message, Error Parent ID, Error showing ID, Focus ID...
        setTimeout(function () {
            fromvalid.focus();
        }, 300);
        return false;
    }
    else {
        $('#ipopupLoading').show();
        $('#btnOKMAil').disabled = true;
        var s = "";
        var triptp = $("#hdtxt_trip")[0].value;
        var segtyp = $('body').data('segtype');
        if (triptp == "Y") {
            s += "<tr style='background-color:#fc8727;color:#fff;'><td colspan='8' style='text-align: center'><lable style='font-size:16px;'><b>" + $("#hdtxt_origin")[0].value + " - " + $("#hdtxt_destination")[0].value + " - " + $("#hdtxt_origin")[0].value + "</b></lable></td></tr>";
        }
        else if (triptp == "M" && segtyp == "I") {
            var connflts = "";
            for (var i = 0; i < aryOrgMul.length; i++) {
                connflts += aryOrgMul[i] + " - ";
            }
            connflts += aryDstMul[aryDstMul.length - 1];
            s += "<tr style='background-color:#fc8727;color:#fff;'><td colspan='8' style='text-align: center'><lable style='font-size:16px;'><b>" + connflts + "</b></lable></td></tr>";
        }
        $.ajax({
            type: "POST", 		//GET or POST or PUT or DELETE verb
            url: FlightMailUrl, 		// Location of the service
            data: '{Toadd: "' + bValid.value + '" ,Subject: "' + $('#txtSubject').val() + '",Flights: "' + $('#FlightsString').val() + '",Origin: "' + $('#hdtxt_origin').val() + '",Destination: "' + $('#hdtxt_destination').val() + '",Frommail:"' + fromvalid.value + '",segloopcnt:"' + triptp + '",strhdr:"' + s + '"}',//,CancelationStatus: "' + $("#hdnPax")[0].value + '" ,BlockTicket: "' + BlockTicket + '",PaymentMode: "' + $("#ddlPaymode").val() + '",TourCode: "' + $("#Tour_Code").val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (json) {//On Successful service call
                $('#ipopupLoading').hide();
                $('#btnOKMAil').disabled = false;
                var result = json.Result;
                if (json.Status == "-1") {
                    window.location.href = sessionExb;
                    return false;
                }
                else if (json.Status == "01") {
                    $("#modal-SendAvailmail").modal('hide');
                    if (result[0] != "") {
                        showalert(result[0], "OK", "");
                    }
                    else {
                        showerralert(result[1] != "" ? result[1] : "Mail sending failure. Please contact customer care (#03).", "", "");
                    }
                }
                else {
                    $("#modal-SendAvailmail").modal('hide');
                    showerralert(result[1], "", "");
                }
            },
            error: function (e) {//On Successful service call 
                $('#ipopupLoading').hide();
                $('#btnOKMAil').disabled = false;
                if (e.status == "500") {
                    window.location.href = sessionExb;
                    return false;
                }
            }	// When Service call fails
        });
    }
}

$('#btnclearFAvailmailpops').click(function () {
    $('#txtmailFrom').val('');
    $('#txtSubject').val('');
    $('#txtToEmail').val('');
});

$('.FApopupcng').change(function () {
    var errPdiv = "dvavailmailerr", errdiv_msg = "dvavailmailerr_msg";
    hideError(errPdiv, errdiv_msg);
});

/************print**********/
var printlength = 0;
$("#BtnFAvailPrinter").click(function (e) {

    var c = $('.SelecCheckbox'); //document.getElementsByName('SelectCheckbox')
    var Select_flight = "";
    var Selectflightsend = "";
    var idd = "";
    var triptp = $("#hdtxt_trip")[0].value;
    var segtyp = $('body').data('segtype');
    var loopCnt = (triptp == "O" || triptp == "Y" || (triptp == "M" && segtyp == "I")) ? 1 : triptp == "R" ? 2 : aryOrgMul.length;
    var basetripty = (triptp == "Y" || (triptp == "M" && segtyp == "I")) ? "O" : triptp;
    for (var seg = 1; seg <= loopCnt; seg++) {
        var c = $('#availset_' + basetripty + '_' + seg + ' .SelecCheckbox');
        for (var i = 0; i < c.length; i++) {
            if (c[i].checked === true) {
                var litaghtml = c[i].value;
                idd += $(c[i]).attr('data-val') + "SpRaj";
                Selectflightsend += litaghtml + "SpRaj";
                printlength++;
            }
        }
        if (seg != loopCnt) {
            idd += "SpRaj"; //For Segment Difference split... by saranraj...
            Selectflightsend += "SpRaj";//For Segment Difference split... by saranraj...
        }
    }
    if (printlength == 0) {
        showerralert("Please Select atleast one flight.", 5000, "");
        return false;
    }
    var Strflights = "";
    var aryFlightsDetails = Selectflightsend.split('SpRaj');
    var aryfligidd = idd.split('SpRaj');
    Strflights += "<div id='PrinTicket1' style='overflow-y:auto; max-height:380px;'><div style='text-align:right;'><label style='color:red;font-size:13px'>*All Fares in:" + assignedcurrency + "</label></div>";

    Strflights += "<br /><table border='1' style='width:100%;font-size:12px;border-collapse:collapse;border-color: #ccc;border: 1px solid #ccc;'>";
    if (triptp == "Y") {
        Strflights += "<tr class='bg_clrtxt' style='background-color:#2d3e52;color:#fff;height: 35px;'><td colspan='8' style='text-align: center'><lable style='font-size:16px;'><b>" + $("#hdtxt_origin")[0].value + " - " + $("#hdtxt_destination")[0].value + " - " + $("#hdtxt_origin")[0].value + "</b></lable></td></tr>";
    }
    else {
        Strflights += "<tr class='bg_clrtxt' style='background-color:#2d3e52;color:#fff;height: 35px;'><td colspan='8' style='text-align: center'><lable style='font-size:16px;'><b>" + $("#hdtxt_origin")[0].value + " - " + $("#hdtxt_destination")[0].value + "</b></lable></td></tr>";
    }
    Strflights += "<tr style='background-color:#f4f4f4'><th style='text-align:center;padding:0 20px;'>Airline</th><th style='text-align:center; padding:0 22px;'>Departure</th><th style='text-align:center; padding:0 35px;'>Arrival</th><th style='text-align:center;padding:0 20px;'>Duration</th><th style='text-align:center;padding:0 20px;'>Class</th><th style='text-align:center;padding:0 20px;'>Basic&nbsp;Fare</th><th style='text-align:center;padding:0 20px;'>Gross</th><th style='text-align:center;padding:0 20px;'>Baggage</th></tr>";
    for (var i = 0; i < aryFlightsDetails.length - 1; i++) {
        var imgpath = $('#fliimage' + aryfligidd[i]).attr("src");
        if (aryFlightsDetails[i] != "") {
            if (aryFlightsDetails[i].split("SpLITSaTIS").length > 1) {
                var Connflight = aryFlightsDetails[i].split("SpLITSaTIS");
                for (var j = 0; j < Connflight.length; j++) {
                    Strflights += "<tr>";
                    Strflights += "<td style='text-align:center'>" + Connflight[j].split("SpLitPResna")[18] + "<br /><img src='" + imgpath + "'></td>";
                    Strflights += "<td style='text-align:center'>" + Connflight[j].split("SpLitPResna")[0] + "<br>" + Connflight[j].split("SpLitPResna")[2] + "</td>";
                    Strflights += "<td style='text-align:center'>" + Connflight[j].split("SpLitPResna")[1] + "<br>" + Connflight[j].split("SpLitPResna")[3] + "</td>";
                    if (j == 0) {
                        if (Connflight[j].split("SpLitPResna")[22] != null && Connflight[j].split("SpLitPResna")[22] != "") {
                            Strflights += "<td style='text-align:center;vertical-align:central' rowspan='" + Connflight.length + "'>" + parseInt((Connflight[j].split("SpLitPResna")[22]) / 60) + "h:" + Math.round(parseInt(Connflight[j].split("SpLitPResna")[22]) % 60) + "m" + "</td>";
                        }
                        else {
                            Strflights += "<td style='text-align:center;vertical-align:central' rowspan='" + Connflight.length + "'>" + Connflight[j].split("SpLitPResna")[22] + "</td>";
                        }
                    }
                    Strflights += "<td style='text-align:center'>" + Connflight[j].split("SpLitPResna")[5] + "-" + Connflight[j].split("SpLitPResna")[17] + "</td>";
                    if (j == 0) {
                        Strflights += "<td style='text-align:center' rowspan='" + Connflight.length + "'>" + Connflight[j].split("SpLitPResna")[13] + "</td>";
                        Strflights += "<td style='text-align:center' rowspan='" + Connflight.length + "'>" + Connflight[j].split("SpLitPResna")[31] + "</td>";
                    }
                    var packagedet = "";
                    if ((((Connflight[j].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]) != null || (((Connflight[j].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]) != "") {
                        packagedet = (((Connflight[j].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]).split("SpLITSaTIS")[0];
                    }
                    else {
                        packagedet = "N/A";
                    }
                    Strflights += "<td style='text-align:center'>" + packagedet + "</td>";
                    Strflights += "</tr>";
                }
            }
            else {
                Strflights += "<tr>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[18] + "<br /><img src='" + imgpath + "'></td>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[0] + "<br />" + aryFlightsDetails[i].split("SpLitPResna")[2] + "</td>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[1] + "<br />" + aryFlightsDetails[i].split("SpLitPResna")[3] + "</td>";
                if (aryFlightsDetails[i].split("SpLitPResna")[22] != null || aryFlightsDetails[i].split("SpLitPResna")[22] != "") {
                    Strflights += "<td style='text-align:center'>" + parseInt((aryFlightsDetails[i].split("SpLitPResna")[22]) / 60) + "h:" + Math.round((parseInt(aryFlightsDetails[i].split("SpLitPResna")[22]) % 60)) + "m" + "</td>";
                }
                else {
                    Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[22] + "</td>";
                }
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[5] + "-" + aryFlightsDetails[i].split("SpLitPResna")[17] + "</td>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[13] + "</td>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[31].replace("SpLITSaTIS", "") + "</td>";
                var packagedet = "";
                if ((((aryFlightsDetails[i].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]) != null || (((aryFlightsDetails[i].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]) != "") {
                    packagedet = (((aryFlightsDetails[i].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]).split("SpLITSaTIS")[0];
                }
                else {
                    packagedet = "N/A";
                }
                Strflights += "<td style='text-align:center'>" + packagedet + "</td>";
                Strflights += "</tr>";
            }
        }
    }
    Strflights += "</table>";
    Strflights += "</div>";
    //Print Button area by saranraj on 20170516...
    Strflights += '<hr><div class="row">';
    Strflights += '<div class="col-md-offset-3 col-md-3 col-xs-6" style="margin-bottom: 15px;">';
    Strflights += '<button type="button" class="btn btn-md btn-primary width100per" id="btnOKPrint" onclick="javascript: return Print_sheet();">Print  </button>';//<span class="spnbtnicon"><i class="fa fa-print"></i></span> 
    Strflights += '</div>';
    Strflights += '<div class="col-md-3 col-xs-6" style="margin-bottom: 15px;">';
    Strflights += '<button type="button" class="btn btn-md btn-danger width100per" id="btnClosePrint" onclick="javascript: return backingtoprint();">Cancel  </button>';//<span class="spnbtnicon"><i class="fa fa-close"></i></span>
    Strflights += '</div>';
    Strflights += '</div>';
    //Print Button area by saranraj on 20170516 End...

    showpopuplogin("Print Flight's", "", Strflights, true, 850, "");
});

function backingtoprint() {
    $('#modal-Fare').iziModal('close');
}

function Print_sheet() {
    var contents = document.getElementById("PrinTicket1").innerHTML;
    var frame1 = document.createElement('iframe');
    frame1.name = "frame1";
    frame1.style.position = "absolute";
    frame1.style.top = "-1000000px";
    document.body.appendChild(frame1);
    var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
    frameDoc.document.open();
    frameDoc.document.write('<html><head><title>DIV Contents</title>');
    frameDoc.document.write('</head><body>');
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        document.body.removeChild(frame1);
    }, 500);
    return false;
}

var savelength = 0;
$("#btnFAvailSave").click(function (e) {
    var Select_flight = "";
    var Selectflightsend = "";
    var idd = "";
    var triptp = $("#hdtxt_trip")[0].value;
    var segtyp = $('body').data('segtype');
    var loopCnt = (triptp == "O" || triptp == "Y" || (triptp == "M" && segtyp == "I")) ? 1 : triptp == "R" ? 2 : aryOrgMul.length;
    var basetripty = (triptp == "Y" || (triptp == "M" && segtyp == "I")) ? "O" : triptp;
    for (var seg = 1; seg <= loopCnt; seg++) {
        var c = $('#availset_' + basetripty + '_' + seg + ' .SelecCheckbox');
        for (var i = 0; i < c.length; i++) {
            if (c[i].checked === true) {
                var litaghtml = c[i].value;
                idd += $(c[i]).attr('data-val') + "SpRaj";
                Selectflightsend += litaghtml + "SpRaj";
                savelength++;
            }
        }
        if (seg != loopCnt) {
            idd += "SpRaj"; //For Segment Difference split... by saranraj...
            Selectflightsend += "SpRaj";//For Segment Difference split... by saranraj...
        }
    }

    if (savelength == 0) {
        showerralert("Please Select atleast one flight.", 5000, "");
        return false;
    }
    var Strflights = "";
    var aryFlightsDetails = Selectflightsend.split('SpRaj');
    var aryfligidd = idd.split('SpRaj');
    Strflights += "<div id='PrinTicket1' style='overflow-y:auto; max-height:380px;'><div style='text-align:right;'><label style='color:red;font-size:13px'>*All Fares in:" + assignedcurrency + "</label></div>";

    Strflights += "<br /><table border='1' style='width:100%;font-size:12px;border-collapse:collapse;border-color: #ccc;border: 1px solid #ccc;'>";
    if (triptp == "Y") {
        Strflights += "<tr class='bg_clrtxt' style='background-color:#2d3e52;color:#fff;height: 35px;'><td colspan='8' style='text-align: center'><lable style='font-size:16px;'><b>" + $("#hdtxt_origin")[0].value + " - " + $("#hdtxt_destination")[0].value + " - " + $("#hdtxt_origin")[0].value + "</b></lable></td></tr>";
    }
    else {
        Strflights += "<tr class='bg_clrtxt' style='background-color:#2d3e52;color:#fff;height: 35px;'><td colspan='8' style='text-align: center'><lable style='font-size:16px;'><b>" + $("#hdtxt_origin")[0].value + " - " + $("#hdtxt_destination")[0].value + "</b></lable></td></tr>";
    }
    Strflights += "<tr style='background-color:#f4f4f4'><th style='text-align:center;padding:0 20px;'>Airline</th><th style='text-align:center;padding:0 22px;'>Departure</th><th style='text-align:center;padding:0 35px;'>Arrival</th><th style='text-align:center;padding:0 20px;'>Duration</th><th style='text-align:center;padding:0 20px;'>Class</th><th style='text-align:center;padding:0 20px;'>Basic&nbsp;Fare</th><th style='text-align:center;padding:0 20px;'>Gross</th><th style='text-align:center;padding:0 20px;'>Baggage</th></tr>";
    for (var i = 0; i < aryFlightsDetails.length - 1; i++) {
        var imgpath = $('#fliimage' + aryfligidd[i]).attr("src");
        if (aryFlightsDetails[i] != "") {


            if (aryFlightsDetails[i].split("SpLITSaTIS").length > 1) {
                var Connflight = aryFlightsDetails[i].split("SpLITSaTIS");
                for (var j = 0; j < Connflight.length; j++) {
                    Strflights += "<tr>";
                    Strflights += "<td style='text-align:center'>" + Connflight[j].split("SpLitPResna")[18] + "<br /><img src='" + imgpath + "'></td>";
                    Strflights += "<td style='text-align:center'>" + Connflight[j].split("SpLitPResna")[0] + "<br>" + Connflight[j].split("SpLitPResna")[2] + "</td>";
                    Strflights += "<td style='text-align:center'>" + Connflight[j].split("SpLitPResna")[1] + "<br>" + Connflight[j].split("SpLitPResna")[3] + "</td>";
                    if (j == 0) {
                        if (Connflight[j].split("SpLitPResna")[22] != null && Connflight[j].split("SpLitPResna")[22] != "") {
                            Strflights += "<td style='text-align:center;vertical-align:central' rowspan='" + Connflight.length + "'>" + parseInt((Connflight[j].split("SpLitPResna")[22]) / 60) + "h:" + Math.round(parseInt(Connflight[j].split("SpLitPResna")[22]) % 60) + "m" + "</td>";
                        }
                        else {
                            Strflights += "<td style='text-align:center;vertical-align:central' rowspan='" + Connflight.length + "'>" + Connflight[j].split("SpLitPResna")[22] + "</td>";
                        }
                    }
                    Strflights += "<td style='text-align:center'>" + Connflight[j].split("SpLitPResna")[5] + "-" + Connflight[j].split("SpLitPResna")[17] + "</td>";
                    if (j == 0) {
                        Strflights += "<td style='text-align:center' rowspan='" + Connflight.length + "'>" + Connflight[j].split("SpLitPResna")[13] + "</td>";
                        Strflights += "<td style='text-align:center' rowspan='" + Connflight.length + "'>" + Connflight[j].split("SpLitPResna")[31] + "</td>";
                    }
                    var packagedet = "";
                    if ((((Connflight[j].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]) != null || (((Connflight[j].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]) != "") {
                        packagedet = (((Connflight[j].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]).split("SpLITSaTIS")[0];
                    }
                    else {
                        packagedet = "N/A";
                    }
                    Strflights += "<td style='text-align:center'>" + packagedet + "</td>";
                    Strflights += "</tr>";
                }
            }
            else {
                Strflights += "<tr>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[18] + "<br /><img src='" + imgpath + "'></td>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[0] + "<br />" + aryFlightsDetails[i].split("SpLitPResna")[2] + "</td>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[1] + "<br />" + aryFlightsDetails[i].split("SpLitPResna")[3] + "</td>";
                if (aryFlightsDetails[i].split("SpLitPResna")[22] != null || aryFlightsDetails[i].split("SpLitPResna")[22] != "") {
                    Strflights += "<td style='text-align:center'>" + parseInt((aryFlightsDetails[i].split("SpLitPResna")[22]) / 60) + "h:" + Math.round((parseInt(aryFlightsDetails[i].split("SpLitPResna")[22]) % 60)) + "m" + "</td>";
                }
                else {
                    Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[22] + "</td>";
                }
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[5] + "-" + aryFlightsDetails[i].split("SpLitPResna")[17] + "</td>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[13] + "</td>";
                Strflights += "<td style='text-align:center'>" + aryFlightsDetails[i].split("SpLitPResna")[31].replace("SpLITSaTIS", "") + "</td>";
                var packagedet = "";
                if ((((aryFlightsDetails[i].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]) != null || (((aryFlightsDetails[i].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]) != "") {
                    packagedet = (((aryFlightsDetails[i].split("SpLitPResna")[24]).split("\n")[4]).split(":")[1]).split("SpLITSaTIS")[0];
                }
                else {
                    packagedet = "N/A";
                }
                Strflights += "<td style='text-align:center'>" + packagedet + "</td>";
                Strflights += "</tr>";
            }
        }
    }
    Strflights += "</table>";
    Strflights += "<div style='float:right'><label style='color:red;font-size:13px'>*All Fares in:" + assignedcurrency + "</label></div>";
    download(Strflights, "Flights.html", "text/html");
});
/* With Markup and Without Markup Added on 06012015 */
$('#chkNFare').click(function (event) {  //on click 
    ShowHideNFare("flg1");
});

function ShowHideNFare(flg) {


    var id = "";
    var val = "";
    var triptp = $("#hdtxt_trip")[0].value;
    var segtyp = $('body').data('segtype');
    var loopCnt = (triptp == "O" || triptp == "Y" || (triptp == "M" && segtyp == "I")) ? 1 : triptp == "R" ? 2 : aryOrgMul.length;
    var basetripty = (triptp == "Y" || (triptp == "M" && segtyp == "I")) ? "O" : triptp;
    var artTempfares = [];
    if ($('#chkNFare')[0].checked) { // check select status
        $('.CGFare').css("display", "table-row");
        $('.WTMFare').css("display", "none");
        $('.Markup').css("display", "table-row");
        if ($("#hdn_AvailFormat").val() != "NAT") {
            $('.Bestbuyfarewithoutmarkup').css("display", "none");
            $('.Bestbuyfarewithmarkup').css("display", "");
        }
        for (var i = 1; i <= loopCnt; i++) {
            var aryTempSubfares = [];
            $("#availset_" + basetripty + "_" + i + " .liGrossfare_span").each(function (e, obj) {

                if ($("#HideComission").val() == "Y") {
                    val = Math.ceil($(this).attr('data-netfare'));
                }
                else if ($("#hdn_AvailFormat").val() == "RIYA") {
                    val = Math.ceil($(this).attr('data-grrfare'));
                }
                else if ($("#hdn_AvailFormat").val() == "NAT") {
                    val = Math.ceil($(this).attr('data-netfare'));
                }
                else {
                    val = $(this).attr('data-grrfare');
                }
                id = $(this).attr('data-id');
                $('#lblGFare' + id).html(val);
                document.getElementById("span_GrossFare" + id).innerHTML = val;

                $('#li_Rows' + id)[0].dataset.price = val;  //For Sorting with N-Fare purpose... by saranraj...
                aryTempSubfares.push(parseInt(val));
            });
            aryTempSubfares.sort(function (a, b) { return a - b; });

            if (flg != "flg2") {

                $("#minfare_dynamic_" + i + "").data('min', parseInt(aryTempSubfares[0]));
                $("#maxfare_dynamic_" + i + "").data('max', parseInt(aryTempSubfares[aryTempSubfares.length - 1]));

                $("#minfare_dynamic_" + i + ",#minfare2_dynamic_" + i).html(parseInt(aryTempSubfares[0]));
                $("#maxfare_dynamic_" + i + ",#maxfare2_dynamic_" + i).html(parseInt(aryTempSubfares[aryTempSubfares.length - 1]));
            }
        }

        ////// added by udhaya for the purpose of Total Fare With 'N' fare

        if (triptp == "M" && segtyp == "D") {
            var newfare = 0;
            for (var i = 1; i <= aryOrgMul.length; i++) {
                var snglefare = $('#spnMGrandTotFare').data('fare-o-with-n' + i + '');
                newfare += (snglefare != null && snglefare != "" ? Number(snglefare) : 0);
                $('#spnMGrandTotFare').html(newfare);
            }
            //  Counter();
        }
        else if (triptp == "R") {
            var _withfare_o = "";
            var _withfare_r = "";
            if ($("#spnGrandTotFare").length && (($("#spnGrandTotFare").data("fare-o-with-n") != null && $("#spnGrandTotFare").data("fare-o-with-n") != "") || ($("#spnGrandTotFare").data("fare-r-with-n") != null && $("#spnGrandTotFare").data("fare-r-with-n") != ""))) {
                _withfare_o = $("#spnGrandTotFare").data("fare-o-with-n");
                _withfare_r = $("#spnGrandTotFare").data("fare-r-with-n");

                $('#spnGrandTotFare').data('fare-o', (_withfare_o != null && _withfare_o != "" ? Number(_withfare_o) : 0));
                $('#spnGrandTotFare').data('fare-r', (_withfare_r != null && _withfare_r != "" ? Number(_withfare_r) : 0));

                $("#spnGrandTotFare").html((_withfare_o != null && _withfare_o != "" ? Number(_withfare_o) : 0) + (_withfare_r != null && _withfare_r != "" ? Number(_withfare_r) : 0));
                //   Counter();
            }
        }
        ////// 

    } else {
        $('.CGFare').css("display", "none");
        $('.WTMFare').css("display", "table-row");
        $('.Markup').css("display", "none");
        if ($("#hdn_AvailFormat").val() != "NAT") {
            $('.Bestbuyfarewithoutmarkup').css("display", "");
            $('.Bestbuyfarewithmarkup').css("display", "none");
        }
        for (var i = 1; i <= loopCnt; i++) {
            var aryTempSubfares = [];
            $("#availset_" + basetripty + "_" + i + " .liGrossfare_span").each(function (e, obj) {
                if ($("#HideComission").val() == "Y") {
                    val = Math.ceil($(this).attr('data-netfare'));
                }
                else if ($("#hdn_AvailFormat").val() == "RIYA") {
                    val = Math.ceil($(this).attr('data-grrfare'));
                }
                else if ($("#hdn_AvailFormat").val() == "NAT") {
                    val = Math.ceil($(this).attr('data-netfare'));
                }
                else {
                    val = $(this).attr('data-WTMFare');
                }
                id = $(this).attr('data-id');
                $('#lblGFare' + id).html(val);
                document.getElementById("span_GrossFare" + id).innerHTML = val;

                $('#li_Rows' + id)[0].dataset.price = val;
                aryTempSubfares.push(parseInt(val));
            });
            aryTempSubfares.sort(function (a, b) { return a - b; });
            if (flg != "flg2") {
                $("#minfare_dynamic_" + i + "").data('min', parseInt(aryTempSubfares[0]));
                $("#maxfare_dynamic_" + i + "").data('max', parseInt(aryTempSubfares[aryTempSubfares.length - 1]));

                $("#minfare_dynamic_" + i + ",#minfare2_dynamic_" + i).html(parseInt(aryTempSubfares[0]));
                $("#maxfare_dynamic_" + i + ",#maxfare2_dynamic_" + i).html(parseInt(aryTempSubfares[aryTempSubfares.length - 1]));
            }
        }
        //////added by udhaya for the purpose of Total Fare Without 'N' fare 
        if (triptp == "M" && segtyp == "D") {
            var newfare = 0;
            for (var i = 1; i <= aryOrgMul.length; i++) {
                var snglefare = $('#spnMGrandTotFare').data('fare-o-with_out-n' + i + '');
                newfare += (snglefare != null && snglefare != "" ? Number(snglefare) : 0);
                $('#spnMGrandTotFare').html(newfare);
            }
            //  Counter();
        }
        else if (triptp == "R") {
            var _with_out_fare_o = "";
            var _with_out_fare_r = "";

            if ($("#spnGrandTotFare").length && (($("#spnGrandTotFare").data("fare-o-with_out-n") != null && $("#spnGrandTotFare").data("fare-o-with_out-n") != "") || $("#spnGrandTotFare").data("fare-r-with_out-n") != null && $("#spnGrandTotFare").data("fare-r-with_out-n") != "")) {
                _with_out_fare_o = $("#spnGrandTotFare").data("fare-o-with_out-n");
                _with_out_fare_r = $("#spnGrandTotFare").data("fare-r-with_out-n");
                ////check fare 
                $('#spnGrandTotFare').data('fare-o', (_with_out_fare_o != null && _with_out_fare_o != "" ? Number(_with_out_fare_o) : 0));
                $('#spnGrandTotFare').data('fare-r', (_with_out_fare_r != null && _with_out_fare_r != "" ? Number(_with_out_fare_r) : 0));
                /////
                $("#spnGrandTotFare").html((_with_out_fare_o != null && _with_out_fare_o != "" ? Number(_with_out_fare_o) : 0) + (_with_out_fare_r != null && _with_out_fare_r != "" ? Number(_with_out_fare_r) : 0));
                // Counter();
            }
        }
        ////////
    }
}
/**/


function bindSletedRndTrpAvail(idCnt, sideCnt) {
    var newfare = 0;
    var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;
    $('#modal-Fare').iziModal('close');
    var triptp = $("#hdtxt_trip")[0].value;
    if (triptp == "M") { //Bypass to Multicity Bind function... 
        bindSletedMulCtyAvail(idCnt, sideCnt);
        return false;
    }

    $('#dvStickyFltSlectRAvail').show();

    var idss = idCnt;
    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == idss;
    });


    var hdnvalus = selectedObj[0]["data-hdInva"];// $('#hdInva' + idCnt).val();
    var splitary = hdnvalus.split('SpLITSaTIS');
    var aryvalus = splitary[0].split('SpLitPResna');
    var arylstvalus = splitary[splitary.length - 1].split('SpLitPResna');
    var with_n_fare = ($("#hdn_AvailFormat").val() == "NAT" || $("#HideComission").val() == "Y") ? $("#GorssFareSpan2" + idCnt).data("netfare") : $("#GorssFareSpan2" + idCnt).data("grrfare");
    var without_n_fare = $("#GorssFareSpan2" + idCnt).data("wtmfare");
    var checknfare = "";
    var s = '';
    s += '<div class="row row5 bordrlftrit roundcheck_fare" data-withn-fare="' + with_n_fare + '" data-withoutn-fare="' + without_n_fare + '" >';//data-withn-fare="'++'"
    s += '<div class="col-sm-4 col-xs-12 col5 imgdiv">';
    s += '<span class="fltno">';
    s += '<img class="FlightTip" alt="" style="width: 28px; height: 28px;" src="' + imgurl + (aryvalus.length > 9 ? aryvalus[9] : "") + ".png?" + imgver + '" />';
    s += '<span class="li_flightno" style="white-space: nowrap;">' + aryvalus[18] + '</span>';
    s += '</span>';
    if ($("#hdn_sessAgentLogin").val() == "Y" || $("#hdn_AppHosting").val() != "BSA") {
        s += '<span class="liclass_span hidden-xs" style="font-size: 11px;font-weight: 600;"><p>' + (aryvalus.length > 5 ? aryvalus[5] : "") + " - " + (aryvalus.length > 17 ? aryvalus[17] : "") + '</p></span>';
    }
    s += '</div>';
    s += '<div class="col-sm-4 col-xs-6 col5" style="text-align:right; border-right: 1px dashed #eee;">';
    var aryOtime = aryvalus.length > 2 ? aryvalus[2].split(' ') : "";
    s += '<div class="ctydiv">' + (aryvalus.length > 0 ? aryvalus[0] : "") + '</div><div id="arrive_date_grid' + idCnt + '" class="stpdiv">' + (aryOtime.length > 2 ? aryOtime[0] + " " + aryOtime[1] + " " + aryOtime[2] : "") + '</div> <div class="timdiv">' + (aryOtime.length > 3 ? aryOtime[3] : "") + '</div>';
    s += '</div>';
    s += '<div class="col-sm-4 col-xs-6 col5">';
    var aryRtime = arylstvalus.length > 3 ? arylstvalus[3].split(' ') : "";
    s += '<div class="ctydiv">' + (arylstvalus.length > 1 ? arylstvalus[1] : "") + '</div><div id="arrive_date_grid1' + idCnt + '" class="stpdiv">' + (aryRtime.length > 2 ? aryRtime[0] + " " + aryRtime[1] + " " + aryRtime[2] : "") + '</div> <div class="timdiv">' + (aryRtime.length > 3 ? aryRtime[3] : "") + '</div>';
    s += '</div>';
    s += '</div>';

    if (sideCnt == "1") {
        $('#selectclickbuttonRTrip').data('dep-id', idCnt);
        //////////////////Added by udhaya for the purpose of with and without 'N' fare

        if ($("#hdn_AvailFormat").val() == "RIYA") {
            checknfare = Number(with_n_fare);
        }
        else if ($("#hdn_AvailFormat").val() == "NAT") {
            checknfare = Number(with_n_fare);
        }
        else {
            if ($("#chkNFare")[0].checked == true) {
                checknfare = Number(with_n_fare);
            } else {
                checknfare = Number(without_n_fare);
            }
        }

        $('#spnGrandTotFare').data("fare-o-with-n", (with_n_fare != null && with_n_fare != "" ? with_n_fare : 0));///for the purpose of with 'N' fare-- by udhaya
        $('#spnGrandTotFare').data("fare-o-with_out-n", (without_n_fare != null && without_n_fare != "" ? without_n_fare : 0));///for the purpose of with out 'N' fare -- by udhaya
        /////////////////////

        $('#spnGrandTotFare').data('fare-on', (checknfare != null && checknfare != "" ? checknfare : 0));

        //fare1 = (checknfare != null && checknfare != "" ? checknfare : 0);
        $('#seleRAvail1').html(s);
    }
    else {
        $('#selectclickbuttonRTrip').data('ret-id', idCnt);
        ///////////////// Added by udhaya for the purpose of with and without 'N' fare

        if ($("#hdn_AvailFormat").val() == "RIYA") {
            checknfare = Number(with_n_fare);
        }
        else if ($("#hdn_AvailFormat").val() == "NAT") {
            checknfare = Number(with_n_fare);
        }
        else {
            if ($("#chkNFare")[0].checked == true) {
                checknfare = Number(with_n_fare);
            } else {
                checknfare = Number(without_n_fare);
            }
        }

        $('#spnGrandTotFare').data("fare-r-with-n", (with_n_fare != null && with_n_fare != "" ? with_n_fare : 0));///for the purpose of with 'N' fare-- by udhaya
        $('#spnGrandTotFare').data("fare-r-with_out-n", (without_n_fare != null && without_n_fare != "" ? without_n_fare : 0));///for the purpose of with out 'N' fare -- by udhaya
        //////////////
        $('#spnGrandTotFare').data('fare-rt', (checknfare != null && checknfare != "" ? checknfare : 0));
        //fare2 = (checknfare != null && checknfare != "" ? checknfare : 0);
        $('#seleRAvail2').html(s);
    }

    fare1 = $('#spnGrandTotFare').data('fare-on');
    fare2 = $('#spnGrandTotFare').data('fare-rt');
    newfare = Number((fare1 != "" ? Number(fare1) : 0) + (fare2 != "" ? Number(fare2) : 0)).toFixed(decimalflag);
    $('#spnGrandTotFare').html(newfare);


    //  Counter();
}

function bindSletedMulCtyAvail(idCnt, sideCnt) {

    var idss = idCnt;
    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == idCnt;
    });
    $('#dvStickyFltSlectMultiAvail').show();
    var hdnvalus = selectedObj[0]["data-hdInva"];// $('#hdInva' + idCnt).val();
    var splitary = hdnvalus.split('SpLITSaTIS');
    var aryvalus = splitary[0].split('SpLitPResna');
    var arylstvalus = splitary[splitary.length - 1].split('SpLitPResna');
    var with_n_fare = $("#GorssFareSpan2" + idCnt).data("grrfare");
    var without_n_fare = $("#GorssFareSpan2" + idCnt).data("wtmfare");
    var checknfare = "";
    var s = '';
    // var colmd = aryOrgMul.length == "2" ? "col-md-6" : aryOrgMul.length == "3" ? "col-md-4" : aryOrgMul.length == "4" ? "col-md-3" : "col-md-2";
    // s += '<div class="' + colmd + ' col-xs-12 col5">';
    s += '<div class="row row5 bordrlftrit roundcheck_fare" data-withn-fare="' + with_n_fare + '" data-withoutn-fare="' + without_n_fare + '" >';//data-withn-fare="'++'"
    s += '<div class="col-sm-4 col-xs-4 col5 imgdiv">';
    s += '<span class="fltno">';
    s += '<img class="FlightTip" alt="" style="width: 28px; height: 28px;" src="' + imgurl + (aryvalus.length > 9 ? aryvalus[9] : "") + ".png?" + imgver + '" />';
    s += '<span class="li_flightno" style="white-space: nowrap;">' + aryvalus[18] + '</span>';
    s += '</span>';
    if ($("#hdn_sessAgentLogin").val() == "Y" || $("#hdn_AppHosting").val() != "BSA") {
        s += '<span class="liclass_span hidden-xs" style="font-size: 11px;font-weight: 600;"><p>' + (aryvalus.length > 5 ? aryvalus[5] : "") + " - " + (aryvalus.length > 17 ? aryvalus[17] : "") + '</p></span>';
    }
    s += '</div>';
    s += '<div class="col-sm-4 col-xs-4 col5 multxt-algn-lft" style="text-align:right; border-right: 1px dashed rgba(255, 255, 255, 0.4);">';
    var aryOtime = aryvalus.length > 2 ? aryvalus[2].split(' ') : "";
    s += '<div class="ctydiv">' + (aryvalus.length > 0 ? aryvalus[0] : "") + '</div><div id="mulcity_dep_date_' + sideCnt + '" class="stpdiv">' + (aryOtime.length > 2 ? aryOtime[0] + " " + aryOtime[1] + " " + aryOtime[2] : "") + '</div> <div id="mulcity_dep_time_' + sideCnt + '" class="timdiv">' + (aryOtime.length > 3 ? aryOtime[3] : "") + '</div>';
    s += '</div>';
    s += '<div class="col-sm-4 col-xs-4 col5 multxt-algn-rgt">';
    var aryRtime = arylstvalus.length > 3 ? arylstvalus[3].split(' ') : "";
    s += '<div class="ctydiv">' + (arylstvalus.length > 1 ? arylstvalus[1] : "") + '</div><div id="mulcity_dep_date_' + sideCnt + '" class="stpdiv">' + (aryRtime.length > 2 ? aryRtime[0] + " " + aryRtime[1] + " " + aryRtime[2] : "") + '</div> <div id="mulcity_dep_time_' + sideCnt + '" class="timdiv">' + (aryRtime.length > 3 ? aryRtime[3] : "") + '</div>';
    s += '</div>';
    s += '</div>';
    // s += '</div>';
    $('#selectclickbuttonMCity').data('dep-id_' + sideCnt + '', idCnt);
    if ($("#chkNFare")[0].checked == true) {
        if ($("#hdn_producttype").val() == "RUAE" || $("#hdn_producttype").val() == "RIYA" || $("#hdn_AvailFormat").val() == "NAT") {
            checknfare = Number(with_n_fare);
        } else {
            checknfare = Number(with_n_fare);
        }
    } else {
        if ($("#hdn_producttype").val() == "RUAE" || $("#hdn_producttype").val() == "RIYA" || $("#hdn_AvailFormat").val() == "NAT") {
            checknfare = Number(with_n_fare);
        } else {
            checknfare = Number(without_n_fare);
        }
    }
    $('#spnMGrandTotFare').data("fare-o-with-n" + sideCnt + "", (with_n_fare != null && with_n_fare != "" ? with_n_fare : 0));///for the purpose of with 'N' fare-- by udhaya
    $('#spnMGrandTotFare').data("fare-o-with_out-n" + sideCnt + "", (without_n_fare != null && without_n_fare != "" ? without_n_fare : 0));///for the purpose of with out 'N' fare -- by udhaya
    $('#spnMGrandTotFare').data('fare-o' + sideCnt + '', (checknfare != null && checknfare != "" ? checknfare : 0));
    $('#slctMAvail' + sideCnt).html(s);
    var newfare = 0;
    for (var i = 1; i <= aryOrgMul.length; i++) {
        var snglefare = $('#spnMGrandTotFare').data('fare-o' + i + '');
        newfare += (snglefare != null && snglefare != "" ? Math.ceil(snglefare) : 0); //#STS105
        $('#spnMGrandTotFare').html(newfare);
    }
    //   Counter();
}

$(document).on('click', '.ftabs', function () {
    $('.ftabs').removeClass('activ');
    $(this).addClass('activ');
    var idcnt = this.id.replace('filterHead_dynamic_', '');
    $('.fltrbdyDynamic').hide();
    $('#filterBody_dynamic_' + idcnt).show();  //changes id to class
    $('.matrixside_' + idcnt).show();

});


$(document).on('click', '.ftabs_mat', function () {
    $('.ftabs_mat').removeClass('activ');
    $(this).addClass('activ');
    var idcnt = this.id.replace('matrixHead_dynamic_', '');
    $('.fltrbdyDynamic').hide();
    $('#filterBody_dynamic_' + idcnt).show();  //changes id to class
});

function ShowHideRTrpSorting(arg) {
    $('#dvmobRTrpSort').slideToggle(400);
    if ($('#sortOpenClose i').hasClass('fa-angle-up')) {
        $('#sortOpenClose i').removeClass('fa-angle-up').addClass('fa-angle-down');
        $('#sortOpenClose span').html('Close');
    }
    else {
        $('#sortOpenClose i').removeClass('fa-angle-down').addClass('fa-angle-up');
        $('#sortOpenClose span').html('Open');
    }
}

function ClickCalender() {
    hideoldavaildetails(); //To hide avail...
    if (searchvalidation()) {
        var trip = "O";
        var adultcount = document.getElementById("ddladult").value;
        var childcount = document.getElementById("ddlchild").value;
        var infcount = document.getElementById("ddlinfant").value;
        //if (document.getElementById("liRoundTrip").checked == true) {
        //    trip = "R";
        //}
        if (document.getElementById("liRoundTripSpl").checked == true) {
            trip = "Y";
        }
        var param = {
            origin: $("#txtorigincity").val().toString().toUpperCase(), destination: $("#txtdestinationcity").val().toString().toUpperCase(), depdate: $("#txtdeparture").val().toString(),
            retdate: $("#txtarrivaldate").val().toString(), triptype: trip
        };
        $('#callFarinner').html("");
        $('.searchviews').show();
        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });

        $.ajax({
            type: "POST", //GET or POST or PUT or DELETE verb
            url: calendrFr,// Location of the service
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (json) { //On Successful service call  hdAvailmulticlass
                $.unblockUI();
                if (json.Status == "-1") {
                    window.location = sessionExb;
                    return false;
                }
                else if (json.Status == "01") {
                    $('#callFarinner').html(json.Result);
                    $('#callFar').slideDown();
                }
                else {
                    if (json.Message != "")
                        showerralert(json.Message, "", "");
                    else
                        showerralert("Problem occured while processing (#07).", "", "");
                }
            },
            error: function (e) {
                $.unblockUI();
                // LogDetails(e.responseText, e.status, "Multiclass Select");
                if (e.status == "500") {
                    window.location = sessionExb;
                    return false;
                }
                else {
                    showerralert("Problem occured while processing (#09).", "", "");
                }
            }
        });
    }
}

$('.callfrClose').click(function () {
    $('#callFar').slideUp();
});

function Getavailability(Id) {
    var date = $('#AvailValue' + Id).val().toString().split('|')[1];
    var dates = date.split('-')
    $("#txtdeparture").val(dates[0].trim());
    if (dates.length > 1)
        $("#txtarrivaldate").val(dates[1].trim());
    $('#btn_Search').click();
}

$('#stickyNofltFound .closebtn').click(function () {
    $('#stickyNofltFound').hide('slow');
});

function CommingSoonfun() {
    showalert("Coming soon.", "OK", "");
}




function ToggleConnectGroup(fareamt, flightno) {
    var clsname = 'group_bind' + fareamt + '_flightno_' + flightno;
    if ($('.group_bind' + fareamt + '_flightno_' + flightno)[0].style.display == "none") {
        $('.group_bind' + fareamt + '_flightno_' + flightno).addClass("_manu");
        $('.group_bind' + fareamt + '_flightno_' + flightno + ' article').css("background-color", "#fff !important")
        //$('.group_bind' + fareamt + '_flightno_' + flightno + ' article').css("border", "1px solid #28a5f5")
        //$('.group_bind' + fareamt + '_flightno_' + flightno + ' article').css("border-bottom", "2px solid #28a5f5")
        $('.symbol_' + fareamt + '_flightno_' + flightno).html('-');
        $('.group_bind' + fareamt + '_flightno_' + flightno).slideToggle(800);
    }
    else {
        $('.group_bind' + fareamt + '_flightno_' + flightno).slideToggle(800);
        $('.group_bind' + fareamt + '_flightno_' + flightno).addClass("_manu");
        $('.group_bind' + fareamt + '_flightno_' + flightno + ' article').css("background-color", "#fff !important")
        $('.group_bind' + fareamt + '_flightno_' + flightno + ' article').css("border", "0px solid #28a5f5")
        $('.group_bind' + fareamt + '_flightno_' + flightno + ' article').css("border-bottom", "0px solid #28a5f5")
        $('.symbol_' + fareamt + '_flightno_' + flightno).html('+');
    }
}

//window scroll function

$(window).scroll(function () {

    if (scrollLock != true && strTrip != "R") {
        if ($(window).scrollTop() >= $(document).height() - 1000) {
            scrollLock = false;


            for (var i = 1; i <= totalgridcount; i++) {

                var sorting = "";
                var speratearr = $.grep(groupmain_array, function (value, index) {
                    return value.bindside_6_arg == i;
                });
                var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                    previousValue[currentValue.flightid].push(currentValue);
                    return previousValue;
                }, {});
                var grouparray = [];
                for (var obj in groupedfli) {
                    grouparray.push(groupedfli[obj]);
                }
                var gridposition = grouparray[0][0].bindside_6_arg;

                if (strTrip == "O" || strTrip == "Y" || (strTrip == "M" && strSegTyp == "I")) {
                    var loacalavailcont = $("#availset_O_1")["0"].children.length;
                    $('.clssorting_O_1 span i').remove();
                    //sorting = $($('#sortFAvail_price_0')).data('sortorder');
                }
                else {
                    var loacalavailcont = $("#availset_" + strTrip + "_" + gridposition + "")["0"].children.length;
                    $('.clssorting_R_' + gridposition + ' span i').remove();
                    // sorting = $($('#sortFAvail_price_' + gridposition + '')).data('sortorder');
                }
                var strsortflg = $('body').data('sorttype');
                var strOrder = $('body').data('sortorder');

                if (strsortflg == "price") { //fare soring after array 
                    grouparray.sort(function (a, b) {
                        if ($("#hdn_AvailFormat").val() == "NAT") {
                            return (strOrder == "desc") ? b[0].NETFare - a[0].NETFare : a[0].NETFare - b[0].NETFare;
                        }
                        else {
                            return (strOrder == "desc") ? b[0].GFare - a[0].GFare : a[0].GFare - b[0].GFare;
                        }
                    });
                }
                else if (strsortflg == "airline") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ?
                            (a[0].Fno.replace(' ', '') < b[0].Fno.replace(' ', '')) ? 1 : -1 :
                            (a[0].Fno.replace(' ', '') > b[0].Fno.replace(' ', '')) ? 1 : -1;
                    });
                }
                else if (strsortflg == "depart") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ? b[0].Dep.split(' ')[3].replace(':', '') - a[0].Dep.split(' ')[3].replace(':', '') : a[0].Dep.split(' ')[3].replace(':', '') - b[0].Dep.split(' ')[3].replace(':', '');
                    });
                }
                else if (strsortflg == "duration") {
                    grouparray = WithLayoverfunc(grouparray, strOrder)
                }
                else if (strsortflg == "arrivsort") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ? b[b.length - 1].Arr.split(' ')[3].replace(':', '') - a[a.length - 1].Arr.split(' ')[3].replace(':', '') : a[a.length - 1].Arr.split(' ')[3].replace(':', '') - b[b.length - 1].Arr.split(' ')[3].replace(':', '');
                    });
                }
                else {
                    grouparray.sort(function (a, b) {
                        if ($("#hdn_AvailFormat").val() == "NAT") {
                            return a[0].NETFare - b[0].NETFare;
                            //return value[0].NETFare >= Number(minval) && value[0].NETFare <= Number(maxval);
                        }
                        else {
                            return a[0].GFare - b[0].GFare;
                        }
                    });  //fare soring after array :srinath
                }

                //grouparray = grouparray.length > 10 ? grouparray.slice(loacalavailcont, grouparray.length) : grouparray;

                var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                    return previousValue;
                }, {});

                var grouparray_orgdes = [];
                for (var obj in groupedorgdes) {
                    grouparray_orgdes.push(groupedorgdes[obj]);
                }

                //grouparray = grouparray.length > 10 ? grouparray.slice(loacalavailcont, grouparray.length) : grouparray;
                grouparray_orgdes = grouparray_orgdes.slice(loacalavailcont, grouparray_orgdes.length);
                Commonavailability_bindingfun(grouparray_orgdes, null);


            }
        }
    }


    if (FilterLock != true && strTrip != "R") {
        if ($(window).scrollTop() + $(window).height() > $(document).height() - 50) {
            var loacalavailcont = "";
            var grouparray = [];
            grouparray = filterdarray;
            if (grouparray.length > 0) {
                var gridposition = grouparray[0][0].bindside_6_arg;
                if (strTrip == "O" || strTrip == "Y" || (strTrip == "M" && strSegTyp == "I")) {
                    var loacalavailcont = $("#availset_O_1")["0"].children.length;
                    $('.clssorting_O_1 span i').remove();
                }
                else {
                    loacalavailcont = $("#availset_" + strTrip + "_" + gridposition + "")["0"].children.length;
                    $('.clssorting_R_' + gridposition + ' span i').remove();
                }
                if (grouparray.length < 1) {
                    FilterLock = true;
                }
                var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                    return previousValue;
                }, {});
                var grouparray_orgdes = [];
                for (var obj in groupedorgdes) {
                    grouparray_orgdes.push(groupedorgdes[obj]);
                }
                grouparray_orgdes = grouparray_orgdes.slice(loacalavailcont, grouparray_orgdes.length);

                if (grouparray_orgdes.length > 0) {

                    $('#spnActlFAvailCnt_' + strTrip + '_' + gridposition).html(grouparray_orgdes.length);

                    Commonavailability_bindingfun(grouparray_orgdes, filterdarray.length);
                }

            }
        }
    }
});


$('.flight-list.listing-style3.flight.col-mob-10.flight-listavail.col15.clsroundAvail').scroll(function () {
    var ss = this.id;
    if (scrollLock != true) {
        if ($('#' + ss)[0].scrollTop >= $('#' + ss)[0].scrollHeight - 680) {
            scrollLock = false;
            var grididnt = ss.replace("availset_" + strTrip + "_", "");

            if ($("#hdn_rtsplflag").val() == "N") {
                var speratearr = $.grep(groupmain_array, function (value, index) {
                    return value.bindside_6_arg == grididnt;
                });
                var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                    previousValue[currentValue.flightid].push(currentValue);
                    return previousValue;
                }, {});
                var grouparray = [];
                for (var obj in groupedfli) {
                    grouparray.push(groupedfli[obj]);
                }
                var gridposition = grouparray[0][0].bindside_6_arg;

                var strsortflg = $('body').data('sorttype');
                var strOrder = $('body').data('sortorder');

                if (strsortflg == "price") { //fare soring after array 
                    grouparray.sort(function (a, b) {
                        if ($("#hdn_AvailFormat").val() == "NAT") {
                            return (strOrder == "desc") ? b[0].NETFare - a[0].NETFare : a[0].NETFare - b[0].NETFare;
                        }
                        else {
                            return (strOrder == "desc") ? b[0].GFare - a[0].GFare : a[0].GFare - b[0].GFare;
                        }
                    });
                }
                else if (strsortflg == "airline") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ?
                            (a[0].Fno.replace(' ', '') < b[0].Fno.replace(' ', '')) ? 1 : -1 :
                            (a[0].Fno.replace(' ', '') > b[0].Fno.replace(' ', '')) ? 1 : -1;
                    });
                }
                else if (strsortflg == "depart") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ? b[0].Dep.split(' ')[3].replace(':', '') - a[0].Dep.split(' ')[3].replace(':', '') : a[0].Dep.split(' ')[3].replace(':', '') - b[0].Dep.split(' ')[3].replace(':', '');
                    });
                }
                else if (strsortflg == "duration") {
                    grouparray = WithLayoverfunc(grouparray, strOrder)
                }
                else if (strsortflg == "arrivsort") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ? b[b.length - 1].Arr.split(' ')[3].replace(':', '') - a[a.length - 1].Arr.split(' ')[3].replace(':', '') : a[a.length - 1].Arr.split(' ')[3].replace(':', '') - b[b.length - 1].Arr.split(' ')[3].replace(':', '');
                    });
                }
                else {
                    grouparray.sort(function (a, b) {
                        if ($("#hdn_AvailFormat").val() == "NAT") {
                            return a[0].NETFare - b[0].NETFare;
                            //return value[0].NETFare >= Number(minval) && value[0].NETFare <= Number(maxval);
                        }
                        else { return a[0].GFare - b[0].GFare; }
                    });  //fare soring after array :srinath
                }
                if (strTrip == "O" || strTrip == "Y" || (strTrip == "M" && strSegTyp == "I")) {
                    var loacalavailcont = $("#availset_O_1")["0"].children.length;
                    $('.clssorting_O_1 span i').remove();
                }
                else {
                    var loacalavailcont = $("#availset_" + strTrip + "_" + gridposition + "")["0"].children.length;
                    $('.clssorting_R_' + gridposition + ' span i').remove();
                }


                var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                    return previousValue;
                }, {});

                var grouparray_orgdes = [];
                for (var obj in groupedorgdes) {
                    grouparray_orgdes.push(groupedorgdes[obj]);
                }

                //grouparray = grouparray.length > 10 ? grouparray.slice(loacalavailcont, grouparray.length) : grouparray;
                grouparray_orgdes = grouparray_orgdes.slice(loacalavailcont, grouparray_orgdes.length);
                Commonavailability_bindingfun(grouparray_orgdes, null);
            }
            else {
                if (grididnt == 1) { /* For RT spl fare onward*/
                    var speratearr = $.grep(groupmain_array, function (value, index) {
                        return value.bindside_6_arg == grididnt;
                    });
                    var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                        previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                        previousValue[currentValue.flightid].push(currentValue);
                        return previousValue;
                    }, {});
                    var grouparray = [];
                    for (var obj in groupedfli) {
                        grouparray.push(groupedfli[obj]);
                    }
                    var gridposition = grouparray[0][0].bindside_6_arg;
                    grouparray.sort(function (a, b) {
                        if ($("#hdn_AvailFormat").val() == "NAT") {
                            return a[0].NETFare - b[0].NETFare;
                            //return value[0].NETFare >= Number(minval) && value[0].NETFare <= Number(maxval);
                        }
                        else { return a[0].GFare - b[0].GFare; }
                    });  //fare soring after array :srinath

                    var loacalavailcont = $("#availset_" + strTrip + "_" + gridposition + "")["0"].children.length;
                    $('.clssorting_R_1 span i').remove();

                    var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                        previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                        previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                        return previousValue;
                    }, {});

                    var grouparray_orgdes = [];
                    for (var obj in groupedorgdes) {
                        grouparray_orgdes.push(groupedorgdes[obj]);
                    }

                    grouparray_orgdes = grouparray_orgdes.slice(loacalavailcont, grouparray_orgdes.length);
                    Commonavailability_bindingfun(grouparray_orgdes, "1");
                }
                else { /* For RT spl fare Retward*/
                    var speratearr = $.grep(groupmain_ret, function (value, index) {
                        return value.bindside_6_arg == grididnt;
                    });
                    var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                        previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                        previousValue[currentValue.flightid].push(currentValue);
                        return previousValue;
                    }, {});
                    var grouparray = [];
                    for (var obj in groupedfli) {
                        grouparray.push(groupedfli[obj]);
                    }
                    var gridposition = grouparray[0][0].bindside_6_arg;
                    grouparray.sort(function (a, b) {
                        if ($("#hdn_AvailFormat").val() == "NAT") {
                            return a[0].NETFare - b[0].NETFare;
                            //return value[0].NETFare >= Number(minval) && value[0].NETFare <= Number(maxval);
                        }
                        else { return a[0].GFare - b[0].GFare; }
                    });  //fare soring after array :srinath

                    var loacalavailcont = $("#availset_" + strTrip + "_" + gridposition + "")["0"].children.length;
                    $('.clssorting_R_2 span i').remove();

                    var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                        previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                        previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                        return previousValue;
                    }, {});
                    var grouparray_orgdes = [];
                    for (var obj in groupedorgdes) {
                        grouparray_orgdes.push(groupedorgdes[obj]);
                    }

                    grouparray_orgdes = grouparray_orgdes.slice(loacalavailcont, grouparray_orgdes.length);
                    Commonavailability_bindingfun(grouparray_orgdes, "2");
                }
            }
        }

    }

    if (FilterLock != true) {
        if ($('#' + ss)[0].scrollTop >= $('#' + ss)[0].scrollHeight - 680) {
            var loacalavailcont = "";
            var grouparray = [];
            var currenctdiv = $('#' + ss)[0].id.replace("availset_" + strTrip + "_", "");

            if ($("#hdn_rtsplflag").val() == "N") {
                grouparray = Main_totalfilterdarray[currenctdiv - 1];
                var gridposition = grouparray[0][0].bindside_6_arg;
                if (strTrip == "O" || strTrip == "Y" || (strTrip == "M" && strSegTyp == "I")) {
                    var loacalavailcont = $("#availset_O_1")["0"].children.length;
                    $('.clssorting_O_1 span i').remove();
                }
                else {
                    loacalavailcont = $("#availset_" + strTrip + "_" + gridposition + "")["0"].children.length;
                    $('.clssorting_R_' + gridposition + ' span i').remove();
                }
                if (grouparray.length < 1) {
                    FilterLock = true;
                }

                var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                    return previousValue;
                }, {});
                var grouparray_orgdes = [];
                for (var obj in groupedorgdes) {
                    grouparray_orgdes.push(groupedorgdes[obj]);
                }

                grouparray_orgdes = grouparray_orgdes.slice(loacalavailcont, grouparray_orgdes.length);
                if (grouparray_orgdes.length > 0) {
                    //$('#spnActlFAvailCnt_' + strTrip + '_' + gridposition).html(grouparray_orgdes.length);
                    Commonavailability_bindingfun(grouparray_orgdes, grouparray.length);
                }
            }
            else {
                if (currenctdiv == "1") { /* For RT spl onward*/
                    grouparray = Main_totalfilterdarray[currenctdiv - 1];
                    var gridposition = "1";

                    loacalavailcont = $("#availset_" + strTrip + "_" + gridposition + "")["0"].children.length;
                    $('.clssorting_R_1 span i').remove();
                    if (grouparray.length < 1) {
                        FilterLock = true;
                    }

                    var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                        previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                        previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                        return previousValue;
                    }, {});
                    var grouparray_orgdes = [];
                    for (var obj in groupedorgdes) {
                        grouparray_orgdes.push(groupedorgdes[obj]);
                    }

                    grouparray_orgdes = grouparray_orgdes.slice(loacalavailcont, grouparray_orgdes.length);
                    if (grouparray_orgdes.length > 0) {
                        Commonavailability_bindingfun(grouparray_orgdes, "1");
                    }
                }
                else { /* For RT spl onward*/
                    //  grouparray = Main_totalfilterdarray[currenctdiv - 1];
                    grouparray = Main_totalfilterdarray["2"];
                    var gridposition = "2";
                    loacalavailcont = $("#availset_" + strTrip + "_" + gridposition + "")["0"].children.length;
                    $('.clssorting_R_2 span i').remove();
                    if (grouparray.length < 1) {
                        FilterLock = true;
                    }
                    var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                        previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                        previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                        return previousValue;
                    }, {});
                    var grouparray_orgdes = [];
                    for (var obj in groupedorgdes) {
                        grouparray_orgdes.push(groupedorgdes[obj]);
                    }

                    grouparray_orgdes = grouparray_orgdes.slice(loacalavailcont, grouparray_orgdes.length);
                    if (grouparray_orgdes.length > 0) {
                        //$('#spnActlFAvailCnt_' + strTrip + '_' + gridposition).html(grouparray_orgdes.length);
                        Commonavailability_bindingfun(grouparray_orgdes, "2");
                    }
                }
            }



        }
    }
});


//common binding availability function write by  20180312
function Commonavailability_bindingfun(array, filterarraycount) {


    array = array.length > 10 ? array.slice(0, 10) : array;
    if ($("#hdn_rtsplflag").val() == "N") {
        var segcnt = (strTrip == "O" || strTrip == "Y" || (strTrip == "M" && strSegTyp == "I")) ? 1 : strTrip == "R" ? 2 : aryOrgMul.length;
        for (var i = 0; i < array.length; i++) {
            var returnresponse_Avail_bf = LoadFlights(array[i], obj1);
            var splt_returnresponse_Avail = returnresponse_Avail_bf.split('Av@i');
            var returnresponse_Avail = splt_returnresponse_Avail[0];
            if (returnresponse_Avail != null && returnresponse_Avail != "") {
                if (strTrip == "O" || strTrip == "Y" || (strTrip == "M" && strSegTyp == "I")) { //Oneway and Roundtrip Special...
                    $("#availset_O_1").append(returnresponse_Avail);
                    //$('#spnTotalFAvailCnt,#spnActlFAvailCnt').html(ob j1.count);
                }
                else {
                    if (strTrip == "R") {
                        if (callrequestcnt == 1) {
                            $("#fromdate").html($("#hdtxt_origin")[0].value);
                            $("#todate").html($("#hdtxt_destination")[0].value);
                            $("#oneway_deptdate").html($("#hdtxt_depa_date")[0].value);
                            $("#round_depdate").show();
                            $("#singlearrow").hide();
                            $("#doublearrow").show();
                            $("#ifen").show();
                            $("#round_depdate").html($("#hdtxt_Arrivedate")[0].value);
                            $('#spnTotalFRoundAvailCntParent').show();
                        }
                    }
                    $(".sriloadani").css("display", "none");
                    $("#availset_" + strTrip + "_" + splt_returnresponse_Avail[1] + "").append(returnresponse_Avail);//For Roundtrip and Multicity.... strTrip=R or M, result[6]=1 or 2 or... upto multicity count...
                }
            }
        }
        ShowHideNFare("flg2");

        $("[data-toggle='tooltip']").tooltip();

        $('.clsavail_dtls').tooltipster({
            delay: 100,
            maxWidth: 500,
            speed: 300,
            interactive: true,
            animation: 'grow',
            trigger: 'hover'
        });

        $('.FlightTip').tooltipster({
            contentAsHTML: true,
            theme: 'tooltipster-punk',
            animation: 'grow',
            position: 'right',
        });

        if ($('#liern-show').is(':checked')) {
            $('.cls_showearning').show();
            $('#spnEarnsort').show();
        }
        else {
            $('.cls_showearning').hide();
            $('#spnEarnsort').hide('slow');
        }

        if ($("#hdnAppTheme").val() == "THEME4" || $("#hdnAppTheme").val() == "THEME5") {
            if ($('#chkCommNFare').is(':checked')) {
                $('.cls_showearning').show();
                $('#spnEarnsort').show();
            }
            else {
                $('.cls_showearning').hide();
                $('#spnEarnsort').hide('slow');
            }
        }
    }
    else {



        for (var i = 0; i < array.length; i++) {
            var returnresponse_Avail_bf = LoadFlights(array[i], obj1);
            var splt_returnresponse_Avail = returnresponse_Avail_bf.split('Av@i');
            var returnresponse_Avail = splt_returnresponse_Avail[0];
            if (returnresponse_Avail != null && returnresponse_Avail != "") {
                $("#fromdate").html($("#hdtxt_origin")[0].value);
                $("#todate").html($("#hdtxt_destination")[0].value);
                $("#oneway_deptdate").html($("#hdtxt_depa_date")[0].value);
                $("#round_depdate").show();
                $("#singlearrow").hide();
                $("#doublearrow").show();
                $("#ifen").show();
                $("#round_depdate").html($("#hdtxt_Arrivedate")[0].value);
                $('#spnTotalFRoundAvailCntParent').show();
                $(".sriloadani").css("display", "none");
                $("#availset_R_" + filterarraycount + "").append(returnresponse_Avail);//For Roundtrip and Multicity.... strTrip=R or M, result[6]=1 or 2 or... upto multicity count...

            }
        }

        ShowHideNFare("flg2");

        $("[data-toggle='tooltip']").tooltip();
        $('.clsavail_dtls').tooltipster({
            delay: 100,
            maxWidth: 500,
            speed: 300,
            interactive: true,
            animation: 'grow',
            trigger: 'hover'
        });
        $('.FlightTip').tooltipster({
            contentAsHTML: true,
            theme: 'tooltipster-punk',
            animation: 'grow',
            position: 'right',
        });
        if ($('#liern-show').is(':checked')) {
            $('.cls_showearning').show();
            $('#spnEarnsort').show();
        }
        else {
            $('.cls_showearning').hide();
            $('#spnEarnsort').hide('slow');
        }

        if ($("#hdnAppTheme").val() == "THEME4" || $("#hdnAppTheme").val() == "THEME5") {
            if ($('#chkCommNFare').is(':checked')) {
                $('.cls_showearning').show();
                $('#spnEarnsort').show();
            }
            else {
                $('.cls_showearning').hide();
                $('#spnEarnsort').hide('slow');
            }
        }
    }

}

//end common end


function airlinematrix_fun(availibilityarray, availabilitysidecound) {

    aryDeparture = [];
    aryDeparture.push("0000-0600");
    aryDeparture.push("0601-1200");
    aryDeparture.push("1201-1800");
    aryDeparture.push("1801-2400");
    var aryDeparturelength = aryDeparture.length;


    var speratearr = $.grep(availibilityarray, function (value, index) {
        return value.bindside_6_arg == availabilitysidecound;
    });
    var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
        previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
        previousValue[currentValue.flightid].push(currentValue);
        return previousValue;
    }, {});
    var grouparray = [];
    for (var obj in groupedfli) {
        grouparray.push(groupedfli[obj]);
    }

    //grouparray.sort(function (a, b) { return a[0].GFare - b[0].GFare; });
    var groupedfli = grouparray.reduce(function (previousValue, currentValue) {
        previousValue[currentValue[0].Fno.split(" ")[0]] = previousValue[currentValue[0].Fno.split(" ")[0]] || []; //For both flight Number and Departure time
        previousValue[currentValue[0].Fno.split(" ")[0]].push(currentValue);
        return previousValue;
    }, {});  //group based on fareid and flight id srinath


    var grouparray_air = [];
    for (var obj in groupedfli) {
        grouparray_air.push(groupedfli[obj]);
    }
    var matrixarra = [];
    var airline_matrixhtml = "";
    matrixarra = $.map(aryDeparture, function (aryDeparturevalue, index) {
        arydeptminmax = [];
        arydeptminmax = aryDeparturevalue.split('-');
        arrayfil = [];
        if (index == 0) {
            airline_matrixhtml += '<div class="fltrbdyDynamic col-xs-2 matrixside_' + availabilitysidecound + '" style="width: 100%;text-align: center;border: 1px dashed #ccc !important;"><table class="table table-responsive tablestyle" cellspacing="0" cellpadding="0" style="width:100%;background:#fff; margin-bottom: unset !important;"><tbody>'
            airline_matrixhtml += '<tr><td><img class="imgdescls" style="object-fit: cover;" src=' + grouparray[0][0].Fno.substr(0, 2) + '/></td></tr>'
            var flightname = airlinename(grouparray[0][0].Fno.split(" ")[0]);
            flightname = flightname.substring(0, flightname.lastIndexOf("("));
            if (flightname.trim() == "IndiGo Airlines") {
                flightname = "IndiGo";
            }
            airline_matrixhtml += '<tr><td style="font-size: 10px;white-space: nowrap;">' + flightname + '</td></tr>'
        }
        arrayfil = $.grep(grouparray, function (value, item) {
            return arydeptminmax.length > 0 && Number(value[0].Dep.split(' ')[3].replace(':', '')) >= Number(arydeptminmax[0]) && Number(value[0].Dep.split(' ')[3].replace(':', '')) <= Number(arydeptminmax[1]);
        });

        if (arrayfil.length == 0) {
            airline_matrixhtml += '<tr><td>-----</td></tr>'
            if (index == aryDeparturelength - 1) {
                airline_matrixhtml += '</tbody></table></div>'
            }
        }
        else {
            arrayfil.sort(function (a, b) {
                if ($("#hdn_AvailFormat").val() == "NAT") {
                    return a[0].NETFare - b[0].NETFare;
                    //return value[0].NETFare >= Number(minval) && value[0].NETFare <= Number(maxval);
                }
                else { return a[0].GFare - b[0].GFare; }
            });
            //airline_matrixhtml += '<tr><td style="font-weight: bolder;color: #000000d1;cursor:pointer" onclick=' + '"' + "javascript:preetyrao_fun('" + index + "')" + '"' + '>' + arrayfil[0][0].GFare + '</td></tr>'
            airline_matrixhtml += '<tr><td style="font-weight: bolder;color: #000000d1;cursor:pointer"  >' + arrayfil[0][0].GFare + '</td></tr>'
            if (index == aryDeparturelength - 1) {
                airline_matrixhtml += '</tbody></table></div>'
            }
        }
        return airline_matrixhtml

    });
    $('#airline_matrixhtml').append(airline_matrixhtml);

}

function seperatematrixfil(segID) {
    var segCnt = segID.replace("airlinemat_main_div_", "");
    var speratearr = $.grep(groupmain_array, function (value, index) {
        return value.bindside_6_arg == segCnt;
    });
    aryDeparture_matrix = [];
    arystops_matrix = [];
    var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
        previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
        previousValue[currentValue.flightid].push(currentValue);
        return previousValue;
    }, {});
    var grouparray = [];
    for (var obj in groupedfli) {
        grouparray.push(groupedfli[obj]);
    }
    grouparray.sort(function (a, b) {
        if ($("#hdn_AvailFormat").val() == "NAT") {
            return a[0].NETFare - b[0].NETFare;
            //return value[0].NETFare >= Number(minval) && value[0].NETFare <= Number(maxval);
        }
        else { return a[0].GFare - b[0].GFare; }
    });  //fare soring after array :srinath


    /*airline matrix fileteration*/
    $("#" + segID + " .chkfilterdept_mat").each(function () {
        if ($(this)[0].checked == true) {
            if ($(this).data('filter') == "dept1")
                aryDeparture_matrix.push("0000-0600");
            else if ($(this).data('filter') == "dept2")
                aryDeparture_matrix.push("0601-1200");
            else if ($(this).data('filter') == "dept3")
                aryDeparture_matrix.push("1201-1800");
            else if ($(this).data('filter') == "dept4")
                aryDeparture_matrix.push("1801-2400");
        }
    });


    $("#" + segID + " .1chkfilterstop").each(function () {
        if ($(this)[0].checked == true && $(this)[0].disabled == false) {
            arystops_matrix.push($(this).data('filter').replace("stop", ""));
        }
    });

    var aryDeparture_matrix_length = aryDeparture_matrix.length;
    var arystopsmatrix_length = arystops_matrix.length;

    /*airline matrix fileteration end*/

    if (grouparray.length != 0 && aryDeparture_matrix_length != 0) {
        grouparray = $.map(aryDeparture_matrix, function (aryDeparturevalue, index) {
            arydeptminmax = [];
            arydeptminmax = aryDeparturevalue.split('-');
            arrayfil = [];
            arrayfil = $.grep(grouparray, function (value, item) {
                return arydeptminmax.length > 0 && Number(value[0].Dep.split(' ')[3].replace(':', '')) >= Number(arydeptminmax[0]) && Number(value[0].Dep.split(' ')[3].replace(':', '')) <= Number(arydeptminmax[1]);
            });
            if (arrayfil.length > 0) {
                return arrayfil
            }
        });
    }


    if (grouparray.length != 0 && arystopsmatrix_length != 0) {
        grouparray = $.map(arystops_matrix, function (arystopsvalue, index) {
            arrayfil = [];
            arrayfil = $.grep(grouparray, function (value, item) {
                if (arystopsvalue.indexOf('+') != -1) {
                    var strarystopsvalue = Number(arystopsvalue.replace('+', ''));
                    return Number(value[0].Stops) > strarystopsvalue;
                }
                return value[0].Stops == arystopsvalue;
            });
            if (arrayfil.length > 0) {
                return arrayfil
            }
        });
    }

    if (grouparray.length < 1) {
        showerralert("We couldn't find flights to match your filters", "", "");
        return false;
    }
    else {
        FilterationFAvail("filterBody_dynamic_1", "1", "airmate", grouparray);
    }


}


function insertenqiry() {

    $.ajax({
        type: "POST",
        url: insertxmlurl,
        contentType: "application/json; charset=utf-8",
        data: {},
        timeout: 180000,
        dataType: "json",
        success: function (data) {
            try {
                if (data.Status == "01") {
                    console.log("Inserted succesfully" + data.Result)
                } else {
                    console.log("Inserted Failed")
                }

            }
            catch (ex) {
                console.log("Inserted Failed")
            }
        }, error: function (e) {
            console.log("Inserted Failed")
        }
    });
}

function moreoption() { //0108
    $('#smsemail').slideToggle(800);
    $('.fa-angle-double-down').toggleClass("rotate180");
}

function CallSpecialfareRequest() { //STS185
    var Result = "";
    var selectedCorporate = "";
    if ($("#Corporatedetails").length > 0 && window.parent.document.getElementById('Corporatedetails').checked == true && window.parent.document.getElementById('gdvCorporateDetails') != null) {
        var grdPassenger = window.parent.document.getElementById('gdvCorporateDetails').rows.length;
        var Index = 0;
        if ($("#rd_corporate_retail").length > 0 && document.getElementById('rd_corporate_retail').checked == true) {
            for (var init = 0; init < grdPassenger ; init++) {
                if ($("#gdvCorporateDetails_ddlCorporateName_" + init).length) {
                    var CorporateName = window.parent.document.getElementById("gdvCorporateDetails_ddlCorporateName_" + init)//.concat(init));
                    var Airline = window.parent.document.getElementById("gdvCorporateDetails_Airline_" + init)//.concat(init));
                    if (CorporateName != null && CorporateName.value != "0") {
                        //selectedCorporate += Airline.innerHTML + "~" + CorporateName.value + "~C" + ",";
                        selectedCorporate += Airline.innerHTML + "~" + CorporateName.value + ",";
                        Index = 1;
                    }
                }
            }

            if (Index == 0) {
                alert("Please select atleast one corporate code");
                return "0";
            }
        }
    }
    else if ($("#Retaildetails").length > 0 && window.parent.document.getElementById('Retaildetails').checked == true && window.parent.document.getElementById('gdvRetailsDetails') != null) {
        var grdPassenger = window.parent.document.getElementById('gdvRetailsDetails').rows.length;
        var selectedRetailFare = "";
        var Index = 0;
        for (var init = 0; init < grdPassenger ; init++) {
            if ($("#gdvRetailsDetails_ddlCorporateName_" + init).length) {
                var CorporateName = window.parent.document.getElementById("gdvRetailsDetails_ddlCorporateName_" + init);
                var Airline = window.parent.document.getElementById("gdvRetailsDetails_Airline_" + init);
                if (CorporateName != null && CorporateName.value != "0") {
                    //selectedCorporate += Airline.innerHTML + "~" + CorporateName.value + "~R" + ",";
                    selectedCorporate += Airline.innerHTML + "~" + CorporateName.value + ",";
                    Index = 1;
                }
            }
        }
        if (Index == 0) {
            alert("Please select atleast one retail code");
            return "0";
        }
    }
    else if ($("#SMEdetails").length > 0 && window.parent.document.getElementById('SMEdetails').checked == true && window.parent.document.getElementById('gdvSMEDetails') != null) {
        var grdPassenger = window.parent.document.getElementById('gdvSMEDetails').rows.length;
        var selectedRetailFare = "";
        var Index = 0;
        for (var init = 0; init < grdPassenger ; init++) {
            if ($("#gdvSMEDetails_ddlCorporateName_" + init).length) {
                var CorporateName = window.parent.document.getElementById("gdvSMEDetails_ddlCorporateName_" + init);
                var Airline = window.parent.document.getElementById("gdvSMEDetails_Airline_" + init);
                if (CorporateName != null && CorporateName.value != "0") {
                    //selectedCorporate += Airline.innerHTML + "~" + CorporateName.value + "~U" + ",";
                    selectedCorporate += Airline.innerHTML + "~" + CorporateName.value + ",";
                    Index = 1;
                }
            }
        }
        if (Index == 0) {
            alert("Please select atleast one SME code");
            return "0";
        }
    }
    else if ($("#Marinedetails").length > 0 && window.parent.document.getElementById('Marinedetails').checked == true && window.parent.document.getElementById('gdvMarineDetails') != null) {
        var grdPassenger = window.parent.document.getElementById('gdvMarineDetails').rows.length;
        var selectedRetailFare = "";
        var Index = 0;
        for (var init = 0; init < grdPassenger; init++) {
            if ($("#gdvMarineDetails_ddlCorporateName_" + init).length) {
                var CorporateName = window.parent.document.getElementById("gdvMarineDetails_ddlCorporateName_" + init);
                var Airline = window.parent.document.getElementById("gdvMarineDetails_Airline_" + init);
                if (CorporateName != null && CorporateName.value != "0") {
                    //selectedCorporate += Airline.innerHTML + "~" + CorporateName.value + "~G" + ",";
                    selectedCorporate += Airline.innerHTML + "~" + CorporateName.value + ",";
                    Index = 1;
                }
            }
        }
        if (Index == 0) {
            alert("Please select atleast one Marine code");
            return "0";
        }
    }
    if (selectedCorporate) {
        $('body').data("bhdcorporatefaredt", selectedCorporate)
        Searchflights('C');
        //--------
        if (Result != "0") {
            Corporatepopclose();
            $("#rd_corporate_retail")[0].checked = true;
            $("#dvCorporatedetails").show();
            $("#dvRetaildetails").hide();
        }
    }
    else {
        alert("Airlines not available for request.Please select another fare.");
        return "0";
    }

}

function Corporatepopclose() {//STS185
    $('#modal-Corporate').iziModal('close');
}

function Modalpopupshowing(obj) {//STS185
    if (obj == 1)
        $("#modal-fr").modal('show');
    else if (obj == 2) {
        $('#modal-Corporate').iziModal('destroy');
        $("#modal-Corporate").iziModal({
            title: "Special Fare Details",
            iconClass: 'icon-stack',
            headerColor: '#245b9d',
            background: '#ddd',
            //fullscreen: true,
            width: 700,
            height: 350,
            padding: 10,
            top: '10px'
        });
        $('#modal-Corporate').iziModal('open', {
            transitionIn: 'bounceInDown',
            transitionOut: 'bounceOutDown' // TransitionOut will be applied if you have any open modal.
        });
        //$('.iziModal').css("background", "#ddd");          

    }
    GetSpelFare();
}

function GetSpelFare() {//STS185
    $(".clsSpcfare").each(function () {
        $("#dv" + this.id).hide();
        if ($("#" + this.id)[0].checked == true)
            $("#dv" + this.id).show();
        else
            $("#dv" + this.id).hide();
    });
}

function ChangeCityNames() {
    var City1 = "";
    var City2 = "";
    City1 = $("#txtorigincity").val();
    City2 = $("#txtdestinationcity").val();
    $("#txtorigincity").val(City2);
    $("#txtdestinationcity").val(City1);
}


function Afteravailsort(strTrip, strFlag, strOrder, type) {

    var strsortflg = strFlag.split('_')[0];
    var strgridval = strFlag.split('_')[1];
    $('body').data('sorttype', strsortflg);
    $('body').data('sortorder', strOrder);
    if (strTrip != "R") {
        for (var i = 1; i <= totalgridcount; i++) {
            if (strgridval == i) {
                var speratearr = $.grep(groupmain_array, function (value, index) {
                    return value.bindside_6_arg == i;
                });
                var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                    previousValue[currentValue.flightid].push(currentValue);
                    return previousValue;
                }, {});
                var grouparray = [];
                for (var obj in groupedfli) {
                    grouparray.push(groupedfli[obj]);
                }
                var gridposition = grouparray[0][0].bindside_6_arg;

                if (strsortflg == "price") { //fare soring after array 
                    grouparray.sort(function (a, b) {
                        if ($("#hdn_AvailFormat").val() == "NAT") {
                            return (strOrder == "desc") ? b[0].NETFare - a[0].NETFare : a[0].NETFare - b[0].NETFare;
                        }
                        else {
                            return (strOrder == "desc") ? b[0].GFare - a[0].GFare : a[0].GFare - b[0].GFare;
                        }
                    });
                }
                else if (strsortflg == "airline") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ?
                            (a[0].Fno.replace(' ', '') < b[0].Fno.replace(' ', '')) ? 1 : -1 :
                            (a[0].Fno.replace(' ', '') > b[0].Fno.replace(' ', '')) ? 1 : -1;
                    });
                }
                else if (strsortflg == "depart") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ? b[0].Dep.split(' ')[3].replace(':', '') - a[0].Dep.split(' ')[3].replace(':', '') : a[0].Dep.split(' ')[3].replace(':', '') - b[0].Dep.split(' ')[3].replace(':', '');
                    });
                }
                else if (strsortflg == "duration") {
                    grouparray = WithLayoverfunc(grouparray, strOrder)
                }
                else if (strsortflg == "arrivsort") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ? b[b.length - 1].Arr.split(' ')[3].replace(':', '') - a[a.length - 1].Arr.split(' ')[3].replace(':', '') : a[a.length - 1].Arr.split(' ')[3].replace(':', '') - b[b.length - 1].Arr.split(' ')[3].replace(':', '');
                    });
                }
                grouparray = commonfiltrationfun(grouparray, strgridval, "filterBody_dynamic_" + i, strTrip);

                var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                    return previousValue;
                }, {});

                var grouparray_orgdes = [];
                for (var obj in groupedorgdes) {
                    grouparray_orgdes.push(groupedorgdes[obj]);
                }
                grouparray_orgdes = grouparray_orgdes.length > 10 ? grouparray_orgdes.slice(0, 10) : grouparray_orgdes;
                $("#availset_" + strTrip + "_" + i).html("");
                Commonavailability_bindingfun(grouparray_orgdes, null);
            }
        }
    }
    else {

        if ($("#hdn_rtsplflag").val() == "Y") {

            //Roundtripspl LCC airline onward sorting
            if (strgridval == "1") {
                var speratearr = $.grep(groupmain_array, function (value, index) {
                    return value.bindside_6_arg == strgridval;
                });
                var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                    previousValue[currentValue.flightid].push(currentValue);
                    return previousValue;
                }, {});
                var grouparray = [];
                for (var obj in groupedfli) {
                    grouparray.push(groupedfli[obj]);
                }
                var gridposition = grouparray[0][0].bindside_6_arg;

                if (strsortflg == "price") { //fare soring after array 
                    grouparray.sort(function (a, b) {
                        if ($("#hdn_AvailFormat").val() == "NAT") {
                            return (strOrder == "desc") ? b[0].NETFare - a[0].NETFare : a[0].NETFare - b[0].NETFare;
                        }
                        else {
                            return (strOrder == "desc") ? b[0].GFare - a[0].GFare : a[0].GFare - b[0].GFare;
                        }
                    });
                }
                else if (strsortflg == "airline") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ?
                            (a[0].Fno.replace(' ', '') < b[0].Fno.replace(' ', '')) ? 1 : -1 :
                            (a[0].Fno.replace(' ', '') > b[0].Fno.replace(' ', '')) ? 1 : -1;
                    });
                }
                else if (strsortflg == "depart") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ? b[0].Dep.split(' ')[3].replace(':', '') - a[0].Dep.split(' ')[3].replace(':', '') : a[0].Dep.split(' ')[3].replace(':', '') - b[0].Dep.split(' ')[3].replace(':', '');
                    });
                }
                else if (strsortflg == "duration") {
                    grouparray = WithLayoverfunc(grouparray, strOrder)
                }
                else if (strsortflg == "arrivsort") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ? b[b.length - 1].Arr.split(' ')[3].replace(':', '') - a[a.length - 1].Arr.split(' ')[3].replace(':', '') : a[a.length - 1].Arr.split(' ')[3].replace(':', '') - b[b.length - 1].Arr.split(' ')[3].replace(':', '');
                    });
                }
                grouparray = commonfiltrationfun(grouparray, strgridval, "filterBody_dynamic_1", strTrip);

                var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                    return previousValue;
                }, {});

                var grouparray_orgdes = [];
                for (var obj in groupedorgdes) {
                    grouparray_orgdes.push(groupedorgdes[obj]);
                }
                grouparray_orgdes = grouparray_orgdes.length > 10 ? grouparray_orgdes.slice(0, 10) : grouparray_orgdes;
                $("#availset_" + strTrip + "_1").html("");
                Commonavailability_bindingfun(grouparray_orgdes, "1");
            }
            //Roundtripspl LCC airline onward sorting

            //Roundtripspl LCC airline Retward sorting
            if (strgridval == "2") {
                var speratearr = $.grep(groupmain_ret, function (value, index) {
                    return value.bindside_6_arg == strgridval;
                });
                var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                    previousValue[currentValue.flightid].push(currentValue);
                    return previousValue;
                }, {});
                var grouparray = [];
                for (var obj in groupedfli) {
                    grouparray.push(groupedfli[obj]);
                }
                var gridposition = grouparray[0][0].bindside_6_arg;

                if (strsortflg == "price") { //fare soring after array 
                    grouparray.sort(function (a, b) {
                        if ($("#hdn_AvailFormat").val() == "NAT") {
                            return (strOrder == "desc") ? b[0].NETFare - a[0].NETFare : a[0].NETFare - b[0].NETFare;
                        }
                        else {
                            return (strOrder == "desc") ? b[0].GFare - a[0].GFare : a[0].GFare - b[0].GFare;
                        }
                    });
                }
                else if (strsortflg == "airline") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ?
                            (a[0].Fno.replace(' ', '') < b[0].Fno.replace(' ', '')) ? 1 : -1 :
                            (a[0].Fno.replace(' ', '') > b[0].Fno.replace(' ', '')) ? 1 : -1;
                    });
                }
                else if (strsortflg == "depart") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ? b[0].Dep.split(' ')[3].replace(':', '') - a[0].Dep.split(' ')[3].replace(':', '') : a[0].Dep.split(' ')[3].replace(':', '') - b[0].Dep.split(' ')[3].replace(':', '');
                    });
                }
                else if (strsortflg == "duration") {
                    grouparray = WithLayoverfunc(grouparray, strOrder)
                }
                else if (strsortflg == "arrivsort") {
                    grouparray.sort(function (a, b) {
                        return (strOrder == "desc") ? b[b.length - 1].Arr.split(' ')[3].replace(':', '') - a[a.length - 1].Arr.split(' ')[3].replace(':', '') : a[a.length - 1].Arr.split(' ')[3].replace(':', '') - b[b.length - 1].Arr.split(' ')[3].replace(':', '');
                    });
                }
                grouparray = commonfiltrationfun(grouparray, strgridval, "filterBody_dynamic_2", strTrip);

                var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                    previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                    return previousValue;
                }, {});

                var grouparray_orgdes = [];
                for (var obj in groupedorgdes) {
                    grouparray_orgdes.push(groupedorgdes[obj]);
                }
                grouparray_orgdes = grouparray_orgdes.length > 10 ? grouparray_orgdes.slice(0, 10) : grouparray_orgdes;
                $("#availset_" + strTrip + "_2").html("");
                Commonavailability_bindingfun(grouparray_orgdes, "2");
            }
            //Roundtripspl LCC airline Retward sorting
        }
        else {

            for (var i = 1; i <= totalgridcount; i++) {
                if (strgridval == i) {
                    var speratearr = $.grep(groupmain_array, function (value, index) {
                        return value.bindside_6_arg == i;
                    });
                    var groupedfli = speratearr.reduce(function (previousValue, currentValue) {
                        previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
                        previousValue[currentValue.flightid].push(currentValue);
                        return previousValue;
                    }, {});
                    var grouparray = [];
                    for (var obj in groupedfli) {
                        grouparray.push(groupedfli[obj]);
                    }
                    var gridposition = grouparray[0][0].bindside_6_arg;

                    if (strsortflg == "price") { //fare soring after array 
                        grouparray.sort(function (a, b) {
                            if ($("#hdn_AvailFormat").val() == "NAT") {
                                return (strOrder == "desc") ? b[0].NETFare - a[0].NETFare : a[0].NETFare - b[0].NETFare;
                            }
                            else {
                                return (strOrder == "desc") ? b[0].GFare - a[0].GFare : a[0].GFare - b[0].GFare;
                            }
                        });
                    }
                    else if (strsortflg == "airline") {
                        grouparray.sort(function (a, b) {
                            return (strOrder == "desc") ?
                                (a[0].Fno.replace(' ', '') < b[0].Fno.replace(' ', '')) ? 1 : -1 :
                                (a[0].Fno.replace(' ', '') > b[0].Fno.replace(' ', '')) ? 1 : -1;
                        });
                    }
                    else if (strsortflg == "depart") {
                        grouparray.sort(function (a, b) {
                            return (strOrder == "desc") ? b[0].Dep.split(' ')[3].replace(':', '') - a[0].Dep.split(' ')[3].replace(':', '') : a[0].Dep.split(' ')[3].replace(':', '') - b[0].Dep.split(' ')[3].replace(':', '');
                        });
                    }
                    else if (strsortflg == "duration") {
                        grouparray = WithLayoverfunc(grouparray, strOrder)
                    }
                    else if (strsortflg == "arrivsort") {
                        grouparray.sort(function (a, b) {
                            return (strOrder == "desc") ? b[b.length - 1].Arr.split(' ')[3].replace(':', '') - a[a.length - 1].Arr.split(' ')[3].replace(':', '') : a[a.length - 1].Arr.split(' ')[3].replace(':', '') - b[b.length - 1].Arr.split(' ')[3].replace(':', '');
                        });
                    }
                    grouparray = commonfiltrationfun(grouparray, strgridval, "filterBody_dynamic_" + i, strTrip);

                    var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
                        previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
                        previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
                        return previousValue;
                    }, {});

                    var grouparray_orgdes = [];
                    for (var obj in groupedorgdes) {
                        grouparray_orgdes.push(groupedorgdes[obj]);
                    }
                    grouparray_orgdes = grouparray_orgdes.length > 10 ? grouparray_orgdes.slice(0, 10) : grouparray_orgdes;
                    $("#availset_" + strTrip + "_" + i).html("");
                    Commonavailability_bindingfun(grouparray_orgdes, null);
                }
            }
        }
    }

}

function commonfiltrationfun(grouparray, segCnt, segID, triptp) {
    var segtyp = $('body').data('segtype');
    var minval = $("#minfare_dynamic_" + segCnt + "").data('min') != "" ? parseInt($("#minfare_dynamic_" + segCnt + "").data('min')) : 0;
    var maxval = $("#maxfare_dynamic_" + segCnt + "").data('max') != "" ? parseInt($("#maxfare_dynamic_" + segCnt + "").data('max')) : 0;
    aryDeparture = [];
    aryArrival = [];
    arystops = [];
    aryAirlinecategory = [];
    aryFaretypfilter = [];
    aryRefund = [];
    aryFlightType = [];
    aryDeparture_matrix = [];
    /*Departure Filteration*/
    $("#" + segID + " .chkfilterdept").each(function () {
        if ($(this)[0].checked == true) {
            if ($(this).data('filter') == "dept1")
                aryDeparture.push("0000-0600");
            else if ($(this).data('filter') == "dept2")
                aryDeparture.push("0601-1200");
            else if ($(this).data('filter') == "dept3")
                aryDeparture.push("1201-1800");
            else if ($(this).data('filter') == "dept4")
                aryDeparture.push("1801-2400");
        }
    });
    $(".detafil").each(function () {
        if ($(this)[0].checked == true) {
            if ($(this).data('filter') == "dept1")
                aryDeparture.push("0000-0600");
            else if ($(this).data('filter') == "dept2")
                aryDeparture.push("0601-1200");
            else if ($(this).data('filter') == "dept3")
                aryDeparture.push("1201-1800");
            else if ($(this).data('filter') == "dept4")
                aryDeparture.push("1801-2400");
        }
    });
    /*Arrival Filteration*/
    $("#" + segID + " .chkfilterarri").each(function () {
        if ($(this)[0].checked == true) {
            if ($(this).data('filter') == "arri1")
                aryArrival.push("0000-0600");
            else if ($(this).data('filter') == "arri2")
                aryArrival.push("0601-1200");
            else if ($(this).data('filter') == "arri3")
                aryArrival.push("1201-1800");
            else if ($(this).data('filter') == "arri4")
                aryArrival.push("1801-2400");
        }
    });
    /*Stops Filteration*/
    $("#" + segID + " .chkfilterstop").each(function () {
        if ($(this)[0].checked == true && $(this)[0].disabled == false) {
            arystops.push($(this).data('filter').replace("stop", ""));
        }
    });
    $(".stopChk_all").each(function () {
        if ($(this)[0].checked == true && $(this)[0].disabled == false) {
            arystops.push($(this).data('filter').replace("stop", ""));
        }
    });
    if ($('#chkDirFlt').is(':checked')) {  //For Direct Flights...
        arystops.push('0');
    }
    var nostop = "0";
    if ($('#chkNonstpFlt').is(':checked')) {  //For Non-Stop Flights...
        nostop = "1";
        arystops.push('0');
    }
    /*Refund Filteration*/
    $("#" + segID + " .chkfilterrefund").each(function () {
        if ($(this)[0].checked == true && $(this)[0].disabled == false) {
            aryRefund.push($(this).data('filter'));
        }
        if (aryRefund.length === 2) {
            aryRefund.push("");
        }
    });
    $(" .Refundablefil").each(function () {
        if ($(this)[0].checked == true && $(this)[0].disabled == false) {
            aryRefund.push($(this).data('filter'));
        }
        if (aryRefund.length === 2) {
            aryRefund.push("");
        }
    });
    /*Flight Type Filteration*/
    $("#" + segID + " .chkfilterflttyp").each(function () {
        if ($(this)[0].checked == true && $(this)[0].disabled == false) {
            aryFlightType.push($(this).data('filter'));
        }
        if (aryFlightType.length === 2) {
            aryFlightType.push("");
        }
    });
    /*Flight Filteration*/
    $("#" + segID + " .chkfilterplane").each(function () {
        if ($(this)[0].checked == true) {
            aryAirlinecategory.push($(this).data('filter').replace("air", ""));
            if ($(this).data('filter').replace("air", "") == "AK") {
                aryAirlinecategory.push("I5");
                aryAirlinecategory.push("FD");
                aryAirlinecategory.push("D7");
            }
            else if ($(this).data('filter').replace("air", "") == "2T") {
                aryAirlinecategory.push("TK");
            }
        }
    });
    /*Fare Filteration*/
    $("#" + segID + " .chkfilterfaretype").each(function () {
        if ($(this)[0].checked == true) {
            aryFaretypfilter.push($(this).data('filter').replace("faretype", ""));
        }
    });

    var amt = 0;
    var refund = "";
    var depttime = "";
    var arrivtime = "", stops = "", via = "", airline = "", airline_typ = "", arydeptminmax = [], aryarrivminmax = [], noofstops = [];
    var showFlag = false;
    var i = 0;
    var aryDeparturelength = aryDeparture.length, aryArrivallength = aryArrival.length, arystopslength = arystops.length, aryRefundlength = aryRefund.length, aryFlyTyplength = aryFlightType.length, aryAirlinecategorylength = aryAirlinecategory.length, aryFaretypfilterlength = aryFaretypfilter.length;
    var basetripty = (triptp == "Y" || (triptp == "M" && segtyp == "I")) ? "O" : triptp;
    filtercount = 0;
    filterdarray = [];
    filterdarray = $.grep(grouparray, function (value, item) {
        if ($("#hdn_AvailFormat").val() == "NAT") {
            //return a[0].NETFare - b[0].NETFare;
            return parseInt(value[0].NETFare) >= Number(minval) && parseInt(value[0].NETFare) <= Number(maxval);
        }
        else {
            return parseInt(value[0].GFare) >= Number(minval) && parseInt(value[0].GFare) <= Number(maxval);
        }

    });
    if ($('#chkNonstpFlt').is(':checked')) {
        filterdarray = $.grep(grouparray, function (value, item) {
            return value[0].nonstop == "" || value[0].nonstop == null;
        });
    }
    if (filterdarray.length != 0 && aryDeparturelength != 0) {
        filterdarray = $.map(aryDeparture, function (aryDeparturevalue, index) {
            arydeptminmax = [];
            arydeptminmax = aryDeparturevalue.split('-');
            arrayfil = [];
            arrayfil = $.grep(filterdarray, function (value, item) {
                return arydeptminmax.length > 0 && Number(value[0].Dep.split(' ')[3].replace(':', '')) >= Number(arydeptminmax[0]) && Number(value[0].Dep.split(' ')[3].replace(':', '')) <= Number(arydeptminmax[1]);
            });
            if (arrayfil.length > 0) {
                return arrayfil
            }
        });
    }
    if (filterdarray.length != 0 && aryArrivallength != 0) {
        filterdarray = $.map(aryArrival, function (aryArrivalvalue, index) {
            aryarrivminmax = [];
            aryarrivminmax = aryArrivalvalue.split('-');
            arrayfil = [];
            arrayfil = $.grep(filterdarray, function (value, item) {
                return aryarrivminmax.length > 0 && Number(value[value.length - 1].Arr.split(' ')[3].replace(':', '')) >= Number(aryarrivminmax[0]) && Number(value[value.length - 1].Arr.split(' ')[3].replace(':', '')) <= Number(aryarrivminmax[1]);

            });
            if (arrayfil.length > 0) {
                return arrayfil
            }
        });
    }
    if (filterdarray.length != 0 && arystopslength != 0) {
        filterdarray = $.map(arystops, function (arystopsvalue, index) {

            arrayfil = [];
            arrayfil = $.grep(filterdarray, function (value, item) {
                if (arystopsvalue.indexOf('+') != -1) {
                    var strarystopsvalue = Number(arystopsvalue.replace('+', ''));
                    return value.length > 1 ? Number(value[0].Stops) > strarystopsvalue && Number(value[value.length - 1].Stops) > strarystopsvalue : Number(value[0].Stops) > strarystopsvalue;
                }
                return value.length > 1 ? value[0].Stops == arystopsvalue && value[value.length - 1].Stops == arystopsvalue : value[0].Stops == arystopsvalue;
            });
            if (arrayfil.length > 0) {
                return arrayfil
            }
        });
    }
    if (filterdarray.length != 0 && aryAirlinecategorylength != 0) {
        filterdarray = $.map(aryAirlinecategory, function (aryAirlinevalue, index) {
            arrayfil = [];
            arrayfil = $.grep(filterdarray, function (value, item) {
                return value[0].Fno.split(' ')[0] == aryAirlinevalue.toUpperCase();
            });
            if (arrayfil.length > 0) {
                return arrayfil
            }
        });
    }
    /*Fare type filter*/
    if (filterdarray.length != 0 && aryFaretypfilterlength != 0) {
        filterdarray = $.map(aryFaretypfilter, function (aryfaretype, index) {
            arrayfil = [];
            arrayfil = $.grep(filterdarray, function (value, item) {
                return value[0].FareType == aryfaretype.toUpperCase();
            });
            if (arrayfil.length > 0) {
                return arrayfil
            }
        });
    }
    if (filterdarray.length != 0 && aryFlyTyplength != 0) {
        filterdarray = $.map(aryFlightType, function (aryFlightTypevalue, index) {
            arrayfil = [];
            arrayfil = $.grep(filterdarray, function (value, item) {
                return value[0].acat.split("SpLitPResna")[0] == aryFlightTypevalue.toUpperCase();
            });
            if (arrayfil.length > 0) {
                return arrayfil
            }
        });
    }
    if (filterdarray.length != 0 && aryRefundlength != 0) {
        filterdarray = $.map(aryRefund, function (aryRefundvalue, index) {
            arrayfil = [];
            arrayfil = $.grep(filterdarray, function (value, item) {
                var aryRefundvalue_s = aryRefundvalue == "refun" ? "TRUE" : "FALSE";
                return value[0].Refund == aryRefundvalue_s;
            });
            if (arrayfil.length > 0) {
                return arrayfil
            }
        });
    }
    return filterdarray;
}

function WithLayoverfunc(deciderary, ascflag) {


    var alayduration = 0;
    var blayduration = 0;
    var atotflyingtime = 0; var btotflyingtime = 0;

    //withlaykey = "Y";
    for (var j = 0; j < deciderary.length - 1; j++) {
        for (var i = 0, swapping; i < deciderary.length - 1; i++) {
            atotflyingtime = 0; btotflyingtime = 0; alayduration = 0; blayduration = 0;
            var a = deciderary[i];
            var b = deciderary[i + 1]

            // Calculate total Layovertime starts
            if (a.length > 1) {
                for (var vj = 0; vj < a.length - 1; vj++) {
                    alayduration += CalculateDepminarrtime(a[vj].Arr.split(' ')[3], a[vj + 1].Dep.split(' ')[3]);

                }
            }
            if (b.length > 1) {
                for (var vj = 0; vj < b.length - 1; vj++) {
                    blayduration += CalculateDepminarrtime(b[vj].Arr.split(' ')[3], b[vj + 1].Dep.split(' ')[3]);

                }
            }

            // Calucate total layovertiem ends

            //Calculate total flying time starts
            $.map(a, function (jn, ji) {
                atotflyingtime += Math.abs(jn.fly)
            });
            $.map(b, function (bjn, bji) {
                btotflyingtime += Math.abs(bjn.fly)
            });
            // Calculate total flying time ends

            if (Number(atotflyingtime + alayduration) > Number(btotflyingtime + blayduration)) {
                swapping = deciderary[i + 1];
                deciderary[i + 1] = deciderary[i];
                deciderary[i] = swapping;
            };
        };
    };

    if (ascflag == "asc") {
        return deciderary;
    }
    else {
        return deciderary.reverse();
    }


}

function CalculateDepminarrtime(deptime, arrtime) {
    var departure_time = (deptime.split(":")[0] * 60) + Number(deptime.split(":")[1]);
    var arrival_time = (arrtime.split(":")[0] * 60) + Number(arrtime.split(":")[1]);

    if (departure_time > arrival_time) {
        arrival_time = arrival_time + 1440;
    }
    return arrival_time - departure_time;
}

function fetchfaretype(type) {
    var FareTypename = '';
    if (type.toUpperCase() == "C")
        FareTypename = "Corporate Fare";
    else if (type.toUpperCase() == "R")
        FareTypename = "Retail Fare";
    else if (type.toUpperCase() == "U")
        FareTypename = "SME Corporate Fare";
    else if (type.toUpperCase() == "V")
        FareTypename = "SME Retail Fare";
    else if (type.toUpperCase() == "E")
        FareTypename = "Coupon Fare";
    else if (type.toUpperCase() == "S" || type.toUpperCase() == "L" || type.toUpperCase() == "Y")
        FareTypename = "Special Fare";
    else if (type.toUpperCase() == "F")
        FareTypename = "Flexi Fare";
    else if (type.toUpperCase() == "B")
        FareTypename = "Booklet Fare";//Business
    else if (type.toUpperCase() == "H")
        FareTypename = "Hand Baggage";
    else if ((type.toUpperCase() == "P") || type.toUpperCase() == "W")
        FareTypename = "Flat Fare";
    else if (type.toUpperCase() == "M")
        FareTypename = "SME Fare";
    else if (type.toUpperCase() == "G")
        FareTypename = "Marine Fare";
    else if (type.toUpperCase() == "N")
        FareTypename = "Normal Fare";
    else if (type.toUpperCase() == "I")
        FareTypename = "SME Corporate Fare";
    else if (type.toUpperCase() == "J")
        FareTypename = "SME Retail Fare";
    else {
        FareTypename = type;
    }
    return FareTypename;
}

function LoadAirlineNames(ids) {
    $.ajax({
        url: AirlineNamesXmlURL,
        dataType: "xml",
        success: function (xmlResponse) {
            $("body").data("airlinenamexmldata", xmlResponse);
            Airdata = $("AIRLINEDET", xmlResponse).map(function () {
                return {
                    value: $("_NAME", this).text() + " ( " + ($.trim($("_CODE", this).text()) + " )"),
                    id: $("_CODE", this).text(),
                    _CATEGORY: $("_CATEGORY", this).text(),
                };

            }).get();
            FilterAirlines(ids, "");
        }
    });
}

function FilterAirlines(ids, thisid) {
    var FilteredAirline = [];
    if (thisid != null && thisid != undefined && thisid != "" && $("#" + thisid).val() != "") {
        FilteredAirline = $.grep(Airdata, function (n, i) {
            return n["_CATEGORY"] == $("#" + thisid).val();
        });
    }
    else {
        FilteredAirline = Airdata;
    }
    FilteredAirline = FilteredAirline.reduce(function (item, e1) {
        var matches = item.filter(function (e2)
        { return e1.id == e2.id });
        if (matches.length == 0) {
            item.push(e1);
        }
        return item;
    }, []);
    var options = "";
    for (var s = 0; s < FilteredAirline.length; s++) {
        options += "<option value='" + FilteredAirline[s].id + "'>" + FilteredAirline[s].value + "</option>";
    }
    for (var i = 0; i < ids.split(',').length; i++) {
        $('#' + ids.split(',')[i] + '').html(options);
        $('#' + ids.split(',')[i] + '').chosen({
            placeholder_text: "Select airlines",
            no_results_text: "no airlines found!",
        });
    }
}

$(document).on('change', '#fli_option', function () {
    FilterAirlines('FlightName', this.id);
});

$(document).on('change', '#Mulfli_option', function () {
    FilterAirlines('MulFlightName', this.id);
});


function CalcTimeDiff(ArrivalDate, DepatureDate) {
    var Durations = Math.abs((new Date(ArrivalDate)) - (new Date(DepatureDate)))
    Durations = Math.floor((Durations / 1000) / 60)
    var Durmind = (Durations) % (60 * 60);
    var Durminutes = Math.floor(Durmind / 60);
    var Dursecd = Durmind % 60;
    var Dursecond = Math.ceil(Dursecd);
    if (Dursecond.value < 10) {
        Dursecond = "0" + Dursecond;
    }
    return Durminutes + "h :" + Dursecond + " m";
}

function ResetAirlineFiltration() {
    $(".chkfilter").each(function () {
        $(this).prop("checked", false);
    });
    var loopcnt = ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "D") ? aryOrgMul.length : ($('#hdtxt_trip').val() == "R" ? 2 : 1);
    $("#minfare_dynamic_" + i + "").data('min', grouparray[0][0].GFare);
    $("#maxfare_dynamic_" + i + "").data('max', grouparray[grouparray.length - 1][0].GFare);
    DeclareFareFilter(loopcnt);
    $('#applymobFAvailFltr').click();
}

function SlideToggle(ids, divid) {
    $("#" + divid).slideToggle();
    if ($("#" + ids + ' i').hasClass('fa-angle-down')) {
        $("#" + ids + ' i').removeClass('fa-angle-down').addClass('fa-angle-up');
    }
    else if ($("#" + ids + ' i').hasClass('fa-angle-up')) {
        $("#" + ids + ' i').removeClass('fa-angle-up').addClass('fa-angle-down');
    }
}

function MobileAppBackFunction() {
    try {
        var flag = $('body').data('BackFlag');
        console.log(flag);
        if (flag == "AVAIL") {
            $("#btnFmodifySearch").click();
        }
        else if (flag == "FAREPOPUP") {
            $(".iziModal-button-close").click();
            $('body').data('BackFlag', 'AVAIL');
        }
        else if (flag == "SELECT") {
            $('.btnbacktoavail').click();
        }
        else if (flag == "PREVIEW") {
            $("#BacktoSelect").click();
        }
    }
    catch (e) {
        console.log(e);
    }
}