function ChangeTabs(arg_id, arg_trip) {

    $(".nav-item").removeClass("active");
    $("#" + arg_id).addClass("active");
    $(".dash-home").hide();
    $("#dv" + arg_id).show();
    if (arg_trip != null && arg_trip != "" && arg_trip == "D") {
        FetchupComingTrips();
    }
    else if (arg_trip != null && arg_trip != "" && arg_trip != "D") {
        $("#dvMyBookings").show();
        if (arg_trip == 'B') {
            $(".clsEditMyBookings").html("My bookings");
            $(".clshideMybookings").show();
        }
        else if (arg_trip == 'U') {
            $(".clsEditMyBookings").html("My Upcoming Trips");
            $(".clshideMybookings").hide();
        }
        else if (arg_trip == 'C') {
            $(".clsEditMyBookings").html("My Cancellations");
            $(".clshideMybookings").hide();
        }
        FetchMyBookings(arg_trip, '0');
    }
    else if (arg_id == "CSPNRDetails") {
        window.location.href = strCreditShellRedirectURL + "?Flag=CSPNR";
    }
    if (arg_id == "Support") {
        if ($(window).width() < 800) {
            $('html, body').animate({
                scrollTop: $("#dvSupport").offset().top-80
            }, 1000);
        }
    }
}

function UpdateMyProfile() {
    try {
        var fileData = new FormData();
        if ($("#profile_title").val() == "") {
            ShowAlert("please select title", 5000);
            return false;
        }
        if ($("#profile_fname").val() == "") {
            ShowAlert("please enter first name", 5000);
            return false;
        }
        if ($("#profile_lname").val() == "") {
            ShowAlert("please enter last name", 5000);
            return false;
        }
        if ($("#profile_MobileNO").val() == "") {
            ShowAlert("please enter mobile no", 5000);
            return false;
        }
        if ($("#profile_Address").val() == "") {
            ShowAlert("please enter address", 5000);
            return false;
        }
        if ($("#profile_City").val() == "") {
            ShowAlert("please enter city", 5000);
            return false;
        }
        if ($("#profile_DOB").val() == "") {
            ShowAlert("please select date of birth", 5000);
            return false;
        }
        if ($("#profile_Pincode").val() == "") {
            ShowAlert("please enter pincode", 5000);
            return false;
        }
        if ($("#profile_Pincode").val().length > 6) {
            ShowAlert("please enter valid pincode", 5000);
            return false;
        }
        if ($("#profile_PassportNO").val() != "" || $("#profile_PassportExp").val() != "" || $("#profile_PassportCountry").val() != "") {
            if ($("#profile_PassportNO").val() == "") {
                ShowAlert("please enter passport no", 5000);
                return false;
            }
            if ($("#profile_PassportExp").val() == "") {
                ShowAlert("please enter passport expiry", 5000);
                return false;
            }
            if ($("#profile_PassportCountry").val() == "") {
                ShowAlert("please enter passport country", 5000);
                return false;
            }
        }
        fileData.append("TITLE", $("#profile_title").val());
        fileData.append("FIRSTNAME", $("#profile_fname").val());
        fileData.append("LASTNAME", $("#profile_lname").val());
        fileData.append("MOBILENO", $("#profile_MobileNO").val());
        fileData.append("EMAIL", $("#profile_Email").val());
        fileData.append("ADDRESS", $("#profile_Address").val());
        fileData.append("CITY", $("#profile_City").val());
        fileData.append("DOB", $("#profile_DOB").val());
        fileData.append("PASSPORTNO", $("#profile_PassportNO").val());
        fileData.append("PASSPORTEXP", $("#profile_PassportExp").val());
        fileData.append("PASSPORTCOUNTRY", $("#profile_PassportCountry").val());
        fileData.append("PINCODE", $("#profile_Pincode").val());

        var fileUpload = $("#UserProfileImg").get(0);
        var files = fileUpload.files;
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }
        $.blockUI({
            message: '<img alt="Please Wait..." src="' + strLoaderURL + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });
        $.ajax({
            type: "POST",
            contentType: false,
            processData: false,
            url: strUpdateMyProfileURL,
            data: fileData,
            dataType: "json",
            success: function (data) {
                $.unblockUI();
                if (data.Status == "01") {
                    sessionStorage.setItem("DashBoardKey", 'P');
                    ShowAlert("Profile Updated Successfully", 5000);
                    setTimeout(function () { location.reload(true); }, 1000);
                }
                else {
                    var strMessage = data.Message != null && data.Message != "" ? data.Message : "Unable to Process your Request";
                    ShowAlert(strMessage, 5000);
                    return false;
                }
            },
            error: function (data) {
                console.log(data);
                $.unblockUI();
                ShowAlert("Unable to Process your Request", 5000);
                return false;
            }
        });
    }
    catch (e) {
        console.log(e);
        $.unblockUI();
        ShowAlert("Unable to Process your Request", 5000);
        return false;
    }
}

function FetchMyTravellers() {
    try {
        $.blockUI({
            message: '<img alt="Please Wait..." src="' + strLoaderURL + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: strFetchMyTravellersURL,
            dataType: "json",
            success: function (data) {
                if (data.Status == "01") {
                    BindMyTravellers(data.Result);
                    return false;
                }
                else {
                    $.unblockUI();
                    var strMessage = data.Message != null && data.Message != "" ? data.Message : "Unable to Process your Request";
                    ShowAlert(strMessage, 5000);
                    return false;
                }
            },
            error: function (data) {
                console.log(data);
                $.unblockUI();
                ShowAlert("Unable to Process your Request", 5000);
                return false;
            }
        });
    }
    catch (e) {
        console.log(e);
        $.unblockUI();
        ShowAlert("Unable to Process your Request", 5000);
        return false;
    }
}

function FetchMyBookings(strType, strFlag, strSPNR, strFromdate, strToDate) {

    try {
        strSPNR = strSPNR != null ? strSPNR : "";
        strFromdate = strFromdate != null ? strFromdate : "";
        strToDate = strToDate != null ? strToDate : "";
        var strParams = {
            strType: strType,
            strFromdate: strFromdate,
            strToDate: strToDate,
            strSPNR: strSPNR,
        };
        $.blockUI({
            message: '<img alt="Please Wait..." src="' + strLoaderURL + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: strFetchMyBookingsURL,
            data: JSON.stringify(strParams),
            dataType: "json",
            success: function (data) {

                if (data.Status == "01" && strFlag == "1") {
                    $.unblockUI();
                    BindUpComingTrip(data.Result);
                }
                else if (data.Status != "01" && strFlag == "1") {
                    $.unblockUI();
                    //do nothing
                }
                else if (data.Status == "01") {
                    if (strType != "B") {
                        $(".clshideMyBookings").hide();
                    }
                    BindMyBookings(data.Result, strType)
                    return false;
                }
                else {
                    $.unblockUI();
                    $("#dvNoBookingRecord").show();
                    $("#DvBookings").hide();
                    if ($(window).width() < 800) {
                        $('html, body').animate({
                            scrollTop: $("#dvMyBookings").offset().top - 80
                        }, 1000);
                    }
                    var strMessage = data.Message != null && data.Message != "" ? data.Message : "Unable to Process your Request";
                    //ShowAlert(strMessage, 5000);
                    return false;
                }
            },
            error: function (data) {
                console.log(data);
                $.unblockUI();
                if (strFlag == '1') {
                    return "";
                }
                else {
                    $("#dvNoBookingRecord").show();
                    $("#DvBookings").hide();
                    if ($(window).width() < 800) {
                        $('html, body').animate({
                            scrollTop: $("#dvMyBookings").offset().top - 90
                        }, 1000);
                    }
                    ShowAlert("Unable to Process your Request", 5000);
                }
                return false;
            }
        });
    }
    catch (e) {
        console.log(e);
        $.unblockUI();
        if (strFlag == '1') {
            return "";
        }
        else {
            ShowAlert("Unable to Process your Request", 5000);
        }
        return false;
    }
}

function BindMyBookings(strRealData, strType) {
    try {
        var strBookedDet = JSON.parse(strRealData);
        var strMyBookings = "";
        var strData = "";

        for (var s = 0; s < strBookedDet.length; s++) {
            strData = strBookedDet[s];

            strData = strBookedDet[s][0];
            var clsFiltertion = '';
            var ClsFilterText = '';

            var Today = new Date();
            var DepatureDate = new Date(strData[0].Dep_sort);
            if (strData[0].Status.indexOf("CANCELLED") != -1) {
                clsFiltertion = 'ClsCancellation';
                ClsFilterText = strData[0].Status;
            }
            else if (strData[0].Status.indexOf("CANCELLED") == -1 && Today < DepatureDate) {
                clsFiltertion = 'clsUpComing';
                ClsFilterText = 'UPCOMING';
            }
            else if (strData[0].Status.indexOf("CANCELLED") == -1 && Today > DepatureDate) {
                clsFiltertion = 'clsCompleted';
                ClsFilterText = "COMPLETED";
            }
            else {
                clsFiltertion = 'clsOthers';
                ClsFilterText = strData[0].Status;
            }

            strMyBookings += '<div class="row row0 clsAllTrips ' + clsFiltertion + '">';
            strMyBookings += '<div class="col-lg-12 col-md-12 col-xs-12 mg-btm-15 overall_tripdets" id="">';
            strMyBookings += '<div class="brdr-radius-2 brdr-full float-left w-100 bg-lt-blu-lt shadow">';
            strMyBookings += '<div class="col-sm-9 pt-10 pb-10 bg-white col-xs-12">';
            strMyBookings += '<div class="itnry-flt-header brdr-btm-full mg-btm-15">';
            strMyBookings += '<div class="col-sm-12 col-xs-12 col0">';
            strMyBookings += '<div class="col-sm-3 col-xs-12 mg-btm-15-res p-0">';
            strMyBookings += '<i class="las la-plane float-left w-100 float-left themeclr-blu txt-cntr mr-10"></i>';
            strMyBookings += '<p class="fnt-wt-600 font-12 mg-btm-0 widget_data w-68">' + strData[0].Depature + '</p>';
            strMyBookings += '</div>';
            strMyBookings += '<div class="rvw-labelView col-sm-6 col-xs-8 pad-0-res mb-10">';
            strMyBookings += '<p class="para_1 mg-btm-0"><span>' + strData[0].Origin + ' - ' + strData[strData.length - 1].Destination + '</span></p>';
            strMyBookings += '<p class="para_2 fnt-wt-600 font-12 mg-btm-0 widget_data"></p>';
            strMyBookings += '</div>';
            strMyBookings += '<div class="col-sm-3 col-xs-4 pl-5 pr-5 pad-0-res">';
            var strClsStatus = strData[0].Status == "LIVE" ? "bkdspn" : strData[0].Status.indexOf("CANCELLED") != -1 ? "canclspn" : "tcanclspn";
            strMyBookings += '<div class="' + strClsStatus + ' float-left"><span class="spnmngstatus">' + ClsFilterText + '</span></div>';//strData[s].Status
            strMyBookings += '</div>';
            strMyBookings += '</div>';
            strMyBookings += '</div>';


            for (var k = 0; k < strBookedDet[s].length; k++) {
                var data = strBookedDet[s][k];
                strMyBookings += '<div class="col-xs-12 col-sm-12 p-0">';
                strMyBookings += '<div class="row row0 mg-btm-15">';

                strMyBookings += '<div class="col-lg-2 col-sm-2 col-xs-3 col-md-1 txt-al-cntr p-b-10 pad-lft-0-res">';
                strMyBookings += '<span>';
                strMyBookings += '<img class="pb-5" src="' + strAirlineLogoURL + '/' + data[0].AirlineCode + '.png?V' + strFlightImgVersion + '" />';
                strMyBookings += '<br>';
                strMyBookings += '<span class="fnt-wt-600 font-12 mg-btm-0 widget_data" style="white-space: nowrap;">' + data[0].FlightNo + '</span>';
                strMyBookings += '<br>';
                strMyBookings += '<span class="fnt-wt-600 font-12 mg-btm-0" style="white-space: nowrap;">' + data[0].AirlineName + '</span>';
                strMyBookings += '</span>';
                strMyBookings += '</div>';

                strMyBookings += '<div class="col-lg-3 col-xs-3 col-sm-3 col-md-3 org-lft pad-0-res">';
                strMyBookings += '<span>';
                strMyBookings += '<label class="fnt-wt-600-im font-13 mg-btm-0 w-100">';
                strMyBookings += '' + data[0].OriginCode + '';
                strMyBookings += '</label>';
                strMyBookings += '<br>';
                strMyBookings += '<span class="font-12 mg-btm-0 widget_data w-100 float-left"><span class="float-left pr-5">' + data[0].DepatureDay + '</span><span class="float-left">' + data[0].DepatureDate + '</span></span>';
                strMyBookings += '<span class="font-12 mg-btm-0 widget_data w-100 float-left">' + data[0].DepatureTime + '</span>';
                strMyBookings += '</span>';
                strMyBookings += '</div>';

                strMyBookings += '<div class="col-lg-4 col-sm-4 col-md-4 col-xs-3 txt-al-cntr mt-10 pad-0-res">';

                strMyBookings += '<div class="line-ht position-relative w-100 mg-tp-20">';
                var JourneyTime = 0;
                for (var j = 0; j < data.length; j++) {
                    JourneyTime += Number(data[j].JourneyTime);
                }
                strMyBookings += '<div class="position-absolute dotsec"></div>';
                strMyBookings += '<div class="position-absolute time-sec">' + CalculateJourneyTime(JourneyTime) + '</div>';
                strMyBookings += '<div class="position-absolute planesec"><i class="fa fa-plane" style="color: #ec182d;"></i></div>';

                strMyBookings += '</div>';

                strMyBookings += '</div>';

                strMyBookings += '<div class="col-lg-3 col-xs-3 col-sm-3 col-md-3 destination-city-cls destination-city-top pad-0-res">';
                strMyBookings += '<span class="float-right w-100 txt-right">';
                strMyBookings += '<label class="fnt-wt-600-im font-13 mg-btm-0">';
                strMyBookings += '' + data[data.length - 1].DestinationCode + '';
                strMyBookings += '</label>';
                strMyBookings += '<br>';
                strMyBookings += '<span class="font-12 mg-btm-0 widget_data w-100 float-right w-100"><span class="float-right pl-5">' + data[data.length - 1].ArrivalDay + '</span><span class="float-right">' + data[data.length - 1].ArrivalDate + '</span></span>';
                strMyBookings += '<span class="font-12 mg-btm-0 widget_data w-100 float-left">' + data[data.length - 1].ArrivalTime + '</span>';
                strMyBookings += '</span>';
                strMyBookings += '</div>';

                strMyBookings += '</div>';
                strMyBookings += '</div>';
            }

            strMyBookings += '</div>';
            strMyBookings += '<div class="col-sm-3 col-xs-12 mg-tp-15">';
            strMyBookings += '<div class="float-left w-100">';
            //TO VIEW PNR BUTTON
            strMyBookings += '<button type="button" onclick="FetchMyViewPNR(\'' + strData[0].S_PNR + '\')" class="ViewItinery b-white float-left w-100 mg-btm-15 mt-10 font-12"><i class="las la-file-alt pr-5 font-15"></i>View Itinerary</button>';
            //TO VIEW PNR BUTTON
            strMyBookings += '<div class="float-left w-100">';
            strMyBookings += '<label class="float-left w-100 widget_data font-11">Booking Refrence No.</label>';
            strMyBookings += '<span class="float-left w-100 font-15 mg-btm-15 txt-transform-u fnt-wt-600">' + strData[0].S_PNR + '</span>';
            strMyBookings += '</div>';
            strMyBookings += '<div class="row">';
            strMyBookings += '<div class="col-sm-6 pr-5 col-xs-6 mb-10">';
            //TO PRINT PNR BUTTON 
            strMyBookings += '<a class="b-white float-left w-100 hidden-sm hidden-md hidden-lg txt-cntr font-12 fnt-wt-600" onclick=' + '"' + "javascript:return PrintTicketMobile('" + strData[0].S_PNR + "')" + '"' + '><i class="las la-print pr-5 fnt-siz-20 pos-rel t-2"></i>Download</a>';
            strMyBookings += '<a class="b-white float-left w-100 hidden-xs txt-cntr font-12 fnt-wt-600" onclick=' + '"' + "javascript:return bookedhis_print('" + strData[0].S_PNR + "')" + '"' + '><i class="las la-print pr-5 fnt-siz-20 pos-rel t-2"></i>Print</a>';
            //TO PRINT PNR BUTTON
            strMyBookings += '</div>';
            strMyBookings += '<div class="col-sm-6 pl-5 col-xs-6 mb-10">';
            //TO EMAIL PNR BUTTON
            strMyBookings += '<a class="b-white float-left w-100 txt-cntr font-12 fnt-wt-600" href="#" onclick="ShowMailPopup(\'' + strData[0].S_PNR + '\')"><i class="las la-envelope pr-5 fnt-siz-20 pos-rel t-2"></i>Mail</a>';
            //TO EMAIL PNR BUTTON
            strMyBookings += '</div>';
            strMyBookings += '</div>';
            strMyBookings += '</div>';
            strMyBookings += '</div>';
            strMyBookings += '</div>';
            strMyBookings += '</div>';
            strMyBookings += '</div>';
            //}
        }
        $("#DvBookings").html("");
        $("#DvBookings").html(strMyBookings);
        $("#dvNoBookingRecord").hide();
        $("#DvBookings").show();
        if ($(window).width() < 800) {
            $('html, body').animate({
                scrollTop: $("#dvMyBookings").offset().top - 90
            }, 1000);
        }
        $.unblockUI();
        return false;
    }
    catch (e) {
        console.log(e);
        $("#DvBookings").html("");
        $("#dvNoBookingRecord").show();
        $("#DvBookings").hide();
        if ($(window).width() < 800) {
            $('html, body').animate({
                scrollTop: $("#dvMyBookings").offset().top - 90
            }, 1000);
        }
        $.unblockUI();
        return false;
    }
}

function FetchupComingTrips() {

    try {
        var strData = FetchMyBookings("UD", "1");
        return false;
    }
    catch (e) {
        console.log(e)
        return false;
    }
}

function BindUpComingTrip(strData) {
    try {
        var strUpComingTrips = "";
        if (strData != null && strData != "") {
            strData = JSON.parse(strData);

            for (var s = 0; s < strData.length; s++) {
                strUpComingTrips += '<div class="clstripfltcnt p-10 w-100 shadow border float-left mg-btm-15">';

                strUpComingTrips += '<div class="col-md-2 col-xs-2 float-left p-0">';
                strUpComingTrips += '<div class="text-center m_avatar">';
                strUpComingTrips += '<img src="' + strAirlineLogoURL + '/' + strData[s].AirlineCode + '.png?V' + strFlightImgVersion + '" />';
                strUpComingTrips += '</div>';
                strUpComingTrips += '</div>';

                strUpComingTrips += '<div class="col-md-10 col-xs-10 float-left pd-l-5" style="font-size:12px;">';
                strUpComingTrips += '<div class="gws-flights-results__leg-flight gws-flights__flex-box gws-flights__align-center">';
                strUpComingTrips += '<div class="w-100 float-left">';
                strUpComingTrips += '<span class="float-left fnt-wt-600 w-100 pb-2">' + strData[s].AirlineName + '</span> <br>';
                strUpComingTrips += '<span class="float-left fnt-wt-600 w-100 font-11 pb-5 widget_data"><span>' + strData[s].FlightNo + '</span></span>';
                strUpComingTrips += '<span class="float-left fnt-wt-600 w-100 font-12 widget_data">' + strData[s].DepatureDate + '</span>';
                strUpComingTrips += '</div>';
                strUpComingTrips += '</div>';
                strUpComingTrips += '</div>';

                strUpComingTrips += '<div class="col-md-12 col-xs-12 float-left clsdatetimetrip pd-r-5 mg-t-8">';
                strUpComingTrips += '<div class="gws-flights-results__leg-itinerary">';
                strUpComingTrips += '<div class="gws-flights-results__dotted-flight-icon"></div>';
                strUpComingTrips += '<div class="gws-flights-results__leg-departure gws-flights__flex-box">';
                strUpComingTrips += '<div><span><span class="fnt-wt-600">' + strData[s].DepatureTime + '</span> </span></div>';
                strUpComingTrips += '<div class="gws-flights__separator" aria-hidden="true"></div>';
                strUpComingTrips += '<div><span class="fnt-wt-600">' + strData[s].Origin + '</span>&nbsp;<span class="gws-flights-results__iata-code">(' + strData[s].OriginCode + ')</span></div>';
                strUpComingTrips += '</div>';
                strUpComingTrips += '<div class="gws-flights-results__leg-duration gws-flights__flex-box flt-body2">';
                strUpComingTrips += '<div>Travel time:&nbsp;<span>' + CalculateJourneyTime(strData[s].JourneyTime) + '</span></div>';
                strUpComingTrips += '</div>';
                strUpComingTrips += '<div class="gws-flights-results__leg-arrival gws-flights__flex-box">';
                strUpComingTrips += '<div><span><span class="fnt-wt-600">' + strData[s].ArrivalTime + '</span> </span></div>';
                strUpComingTrips += '<div class="gws-flights__separator" aria-hidden="true"></div>';
                strUpComingTrips += '<div><span class="fnt-wt-600">' + strData[s].Destination + '</span>&nbsp;<span class="gws-flights-results__iata-code">(' + strData[s].DestinationCode + ')</span></div>';
                strUpComingTrips += '</div>';
                strUpComingTrips += '</div>';
                strUpComingTrips += '<div class="gws-flights__flex-box cbt-travel_details w-100 float-left">';
                strUpComingTrips += '<div class="gws-flights-results__seating-class-be-non-be w-100">';
                strUpComingTrips += '<span>' + strData[s].S_PNR + '</span>';
                strUpComingTrips += '<span class="cbt-flights__separator" aria-hidden="true">|</span>';
                strUpComingTrips += '<span><span>' + strData[s].TotalPassenger + '</span>&nbsp;<span>Traveller(s)</span></span>';
                strUpComingTrips += '<span class="tx-color-R1 fnt-wt-600 float-right w-100 font-15 mt-10 txt txt-right"> <i class="la la-rupee-sign p-r-5"></i>' + strData[s].GrossFare + '</span>';
                strUpComingTrips += '</div>';
                strUpComingTrips += '</div>';
                strUpComingTrips += '</div>';

                strUpComingTrips += '</div>';
            }
            $("#dvUpComingTrip").html("");
            $("#dvUpComingTrip").html(strUpComingTrips);
            $("#dvUpComingTrip").show();
            $("#dvNoUpComingTrips").hide();
            $.unblockUI();
        }
        else {
            $("#dvUpComingTrip").hide();
            $("#dvNoUpComingTrips").show();
            $.unblockUI();
            return false;
        }
    }
    catch (e) {
        console.log(e);
        $.unblockUI();
        return false;
    }
}

function ShowAlert(strMessage, strTimeOut) {
    $('#modal-alert').iziModal('destroy');
    if (strTimeOut == null || strTimeOut == "") {
        $("#modal-alert").iziModal({
            title: strMessage,
            icon: 'fa fa-warning',
            headerColor: '#bd5b5b',
            width: "500px"
        });
    }
    else {
        $("#modal-alert").iziModal({
            title: strMessage,
            icon: 'fa fa-warning',
            headerColor: '#bd5b5b',
            width: "500px",
            timeout: strTimeOut
        });
    }
    $('#modal-alert').iziModal('open');
}

function bookedhis_print(SPNR) {
    var test = "";

    var Pnr = SPNR;
    var fare = "yes";
    var single = "no";

    if ($(window).width() < 768) { //For Mobile View...
        var iurl = PrintTicketRedirect + "?strSPNRNO=" + Pnr + "|" + fare + "&single=" + single + "&Page=" + "P";
        var ititle = "Print Ticket";
        var isubtitle = "";
        var ifull = false;
        IziModalIFramePop(iurl, ititle, isubtitle, ifull);
    }
    else {
        newwindow = window.open(PrintTicketRedirect + "?strSPNRNO=" + Pnr + "|" + fare + "&single=" + single + "&Page=" + "P");
    }
    if (window.focus) { newwindow.focus() }
    return false;

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

function MybookingFiltrations(arg) {
    $("#dvNoBookingRecord").hide();
    $("#DvBookings").hide();
    $(".clsAllTrips").hide();
    $(".clsFiltertrips").removeClass('active');
    $(this).addClass('active');

    if (arg == null || arg == "" || arg == "ALL") {
        if ($(".clsAllTrips").length > 0) {
            $(".clsAllTrips").show();
            $("#DvBookings").show();
        }
        else {
            $("#dvNoBookingRecord").show();
        }
        $("#FilterAllTrips").addClass('active');
    }
    else if (arg != null && arg != "") {
        if (arg == "U") {
            if ($(".clsUpComing").length > 0) {
                $(".clsUpComing").show();
                $("#DvBookings").show();
            }
            else {
                $("#dvNoBookingRecord").show();
            }
            $("#FilterUpComing").addClass('active');
        }
        else if (arg == "COM") {
            if ($(".clsCompleted").length > 0) {
                $(".clsCompleted").show();
                $("#DvBookings").show();
            }
            else {
                $("#dvNoBookingRecord").show();
            }
            $("#FilterCompleted").addClass('active');
        }
        else if (arg == "CAN") {
            if ($(".ClsCancellation").length > 0) {
                $(".ClsCancellation").show();
                $("#DvBookings").show();
            }
            else {
                $("#dvNoBookingRecord").show();
            }
            $("#FilterCancelled").addClass('active');
        }
    }
}

function ShowMailPopup(SPNR) {
    $("#modal-sendemail").modal({
        backdrop: 'static',
        keyboard: false
    });
    $("#eticket_pnr").val(SPNR);
    $("#mail_id").val(strUserEmailID);
}

function SendMail() {
    try {
        var bValid = true;
        var strEmail = $("#mail_id").val();
        if (strEmail == "") {
            showerralert("Please Enter Email", 3000);
            $("#mail_id").focus();
            bValid = false;
            return false;
        }
        var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        if (strEmail != "" && !filter.test(strEmail)) {
            showerralert("Please Enter valid Email ID", 3000);
            $("#mail_id").focus();
            bValid = false;
            return false;
        }

        if (bValid) {
            $.blockUI({
                message: '<img alt="Please Wait..." src="' + strLoaderURL + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
                css: { padding: '5px' }
            });
            $("#modal-sendemail").modal('hide');
            $.ajax({
                type: "POST", 		//GET or POST or PUT or DELETE verb
                url: Mailsendingurl, 		// Location of the service
                data: '{strSPNRNO: "' + $("#eticket_pnr").val() + '" ,txtEmailID: "' + $("#mail_id").val() + '",pdf: "YES",strSingle: "NO",Subject:"' + $("#eticket_pnr").val() + '" ,strPaymentmode : "P" ,blntktmailflag : "' + false + '"}',
                //async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (json) {//On Successful service call
                    $.unblockUI();

                    var result = json.Result;
                    if (result[1] != "") {
                        ShowAlert(result[1]);
                        $("#mail_id").val("");
                    }
                    else {
                        ShowAlert(result[0]);
                        $("#mail_id").val("");
                    }
                },
                error: function (e) {//On Successful service call
                    $.unblockUI();
                    console.log(e);
                    $("#modal-sendemail").modal('hide');
                    if (e.status == "500") {
                        ShowAlert("An Internal Problem Occurred. Your Session will Expire.");
                        window.top.location = sessionExb;
                        return false;
                    }
                    else {
                        ShowAlert("Unable to send Mail.Please contact supprt team.", "");
                        return false;
                    }
                }
            });
        }
    }
    catch (e) {
        console.log(e);
        ShowAlert("Unable to send Mail.Please contact supprt team.", "");
        $.unblockUI();
        return false;
    }
}

function CustomDateFormat(date, years, months, days, type) {
    date = new Date(date[2], Number(date[1]) - 1, date[0]);
    if (type == '-') {
        date.setFullYear(date.getFullYear() - Number(years));
        date.setMonth(date.getMonth() - Number(months))
        date.setDate(date.getDate() - Number(days))
    }
    else if (type == '+') {
        date.setFullYear(date.getFullYear() + Number(years));
        date.setMonth(date.getMonth() + Number(months))
        date.setDate(date.getDate() + Number(days))
    }
    var day = (date.getDate() < 10 ? "0" : "") + (date.getDate());
    var month = (date.getMonth() + 1 < 10 ? "0" : "") + (date.getMonth() + 1);
    var year = date.getFullYear();
    return day + "/" + month + "/" + year;
}

function GetTodayDate()
{
    var Today = new Date();
    var TDate = (Today.getDate() < 10 ? "0" : "") + Today.getDate();
    var TMonth = ((Today.getMonth() + 1) < 10 ? "0" : "") + (Today.getMonth() + 1);
    var TYear = Today.getFullYear();
    return TDate + "/" + TMonth + "/" + TYear;
}

$(document).on('click', '#btn_mybookclr', function () {
    $("#txtFromDate_Book").datepicker("destroy");
    $("#txtToDate_Book").datepicker("destroy");
    $("#txtFromDate_Book").val(Today);
    $("#txtFromDate_Book").datepicker({
        minDate: mindate, maxDate: Today, dateFormat: "dd/mm/yy",
    });
    $("#txtToDate_Book").val(Today);
    $("#txtToDate_Book").datepicker({
        minDate: $("#txtFromDate_Book").val(), maxDate: maxdate, dateFormat: "dd/mm/yy",
    });
    $("#txtSPNR_Book").val("")
});

$(document).on('click', '#btn_mybooksubmit', function () {
    var strSPNR = $("#txtSPNR_Book").val();
    var strFromdate = $("#txtFromDate_Book").val();
    var strToDate = $("#txtToDate_Book").val();
    FetchMyBookings('B', '0', strSPNR, strFromdate, strToDate)
});

function FetchMyViewPNR(strSPNR)
{
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + strLoaderURL + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });
    var strParams = {
        strSPNR: strSPNR
    };
    try {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: strFetchMyViewPNRURL,
            data: JSON.stringify(strParams),
            dataType: "json",
            success: function (data) {
                if (data.Status == "01") {
                    BindMyViewPNR(data.Result, data.strPaymentDetails, data.strPaxDetails);
                    return false;
                }
                else {
                    $.unblockUI();
                    var strMessage = data.Message != null && data.Message != "" ? data.Message : "Unable to Process your Request";
                    ShowAlert(strMessage, 5000);
                    return false;
                }
            },
            error: function (data) {
                console.log(data);
                $.unblockUI();
                ShowAlert("Unable to Process your Request", 5000);
                return false;
            }
        });
    }
    catch (e) {
        console.log(e);
        $.unblockUI();
        ShowAlert("Unable to Process your Request", 5000);
        return false;
    }

}

function BindMyViewPNR(strTicketDetails, strPaymentDetails, strPaxDetails) {

    strTicketDetails = JSON.parse(strTicketDetails);
    strPaxDetails = JSON.parse(strPaxDetails);
    strPaymentDetails = JSON.parse(strPaymentDetails);
    try {
        $("#hdn_spnr").val(strTicketDetails[0][0].S_PNR);
        //REGION TICKET DETAILS
        var sb_TicketlDet = "";
        sb_TicketlDet += '<div class="itnry-flt-header brdr-btm-full mg-btm-15">';
        sb_TicketlDet += '<div class="col-sm-12 col-xs-12 col0">';
        sb_TicketlDet += '<div class="col-sm-2 col-xs-3 mg-btm-15-res p-0">';
        sb_TicketlDet += '<i class="las la-plane float-left w-100 float-left themeclr-blu txt-cntr mr-10"></i>';
        sb_TicketlDet += '<span>';
        sb_TicketlDet += '<p class="font-12 mg-btm-0 widget_data widfix">' + strTicketDetails[0][0].DepatureDay + '</p>';
        sb_TicketlDet += '<p class="font-12 mg-btm-0 widget_data widfix">' + strTicketDetails[0][0].DepatureDate + '</p>';
        sb_TicketlDet += '<p class="font-12 mg-btm-0 widget_data widfix">' + strTicketDetails[0][0].DepatureTime + '</p>';
        sb_TicketlDet += '<span>';
        sb_TicketlDet += '</div>';
        sb_TicketlDet += '<div class="rvw-labelView col-sm-4 col-xs-9 pad-0-res mg-btm-10-res">';
        sb_TicketlDet += '<p class="para_1 mg-btm-0"><span>' + strTicketDetails[0][0].Origin + ' - ' + strTicketDetails[0][strTicketDetails[0].length - 1].Destination + '</span></p>';
        sb_TicketlDet += '<div class="float-left w-100">';
        sb_TicketlDet += '<span class="font-12 mg-btm-0 widget_data">' + strTicketDetails[0][0].Class + '</span>';
        sb_TicketlDet += '</div>';
        sb_TicketlDet += '</div>';
        sb_TicketlDet += '<div class="col-sm-6 col-xs-12 p-0">';
        sb_TicketlDet += '<div class="col-sm-4 col-xs-4 float-left mg-btm-10-res pad-0-res">';
        sb_TicketlDet += '<span class="font-12 mg-btm-0 widget_data">Passengers</span>';
        sb_TicketlDet += '<span class="font-12 float-left w-100 fnt-wt-600">' + strTicketDetails[0][0].TotalPassenger + ' Passengers</span>';
        sb_TicketlDet += '</div>';
        sb_TicketlDet += '<div class="col-sm-4 col-xs-4 float-left mg-btm-10-res pad-0-res">';
        sb_TicketlDet += '<span class="font-12 mg-btm-0 widget_data">Reference No.</span>';
        sb_TicketlDet += '<span class="font-12 float-left w-100 fnt-wt-600">' + strTicketDetails[0][0].S_PNR + '</span>';
        sb_TicketlDet += '</div>';
        sb_TicketlDet += '<div class="col-sm-4 col-xs-4 float-left booksec pad-0-res mg-btm-10-res">';
        var strClsStatus = (strTicketDetails[0][0].Status == "LIVE" || strTicketDetails[0][0].Status == "CONFIRMED") ? "bkdspn" : strTicketDetails[0][0].Status.indexOf("CANCELLED") != -1 ? "canclspn" : "tcanclspn";
        sb_TicketlDet += '<div class="' + strClsStatus + '"><span class="spnmngstatus">' + strTicketDetails[0][0].Status + '</span></div>';
        sb_TicketlDet += '</div>';
        sb_TicketlDet += '</div>';
        sb_TicketlDet += '</div>';
        sb_TicketlDet += '</div>';

        for (var i = 0; i < strTicketDetails.length; i++) {
            var strData = strTicketDetails[i];
            sb_TicketlDet += '<div class="row row0">';
            sb_TicketlDet += '<div class="col-lg-2 col-sm-2 col-xs-3 col-md-2 col10 txt-al-cntr p-b-10">';
            sb_TicketlDet += '<span>';
            sb_TicketlDet += '<img class="pb-5" src="' + strAirlineLogoURL + '/' + strData[0].AirlineCode + '.png?V' + strFlightImgVersion + '">';
            sb_TicketlDet += '<br>';
            sb_TicketlDet += '<span class="fnt-wt-600 font-12 mg-btm-0 widget_data txt-cntr-res" style="white-space: nowrap;">' + strData[0].FlightNO + '</span>';
            sb_TicketlDet += '<br>';
            sb_TicketlDet += '<span class="font-12 mg-btm-0 txt-cntr-res" style="white-space: nowrap;">' + strData[0].AirlineName + '</span>';
            sb_TicketlDet += '</span>';
            sb_TicketlDet += '</div>';
            sb_TicketlDet += '<div class="col-lg-3 col-xs-3 col-sm-3 col-md-3 org-lft pad-0-res">';
            sb_TicketlDet += '<span>';
            sb_TicketlDet += '<span class="float-left w-100 font-15 themeclr-blu fnt-wt-600">' + strData[0].DepatureTime.split(' ')[0] + '</span>';
            sb_TicketlDet += '<label class="fnt-wt-600-im font-13 mg-btm-0 w-100 float-left w-100">';
            sb_TicketlDet += '' + strData[0].OriginCode + '';
            sb_TicketlDet += '</label>';
            sb_TicketlDet += '<br>';
            sb_TicketlDet += '<span class="font-12 mg-btm-0 w-100">' + strData[0].DepatureDay + ' ' + strData[0].DepatureDate + '</span>';
            sb_TicketlDet += '<span class="font-12 mg-btm-0 widget_data w-100 float-left">';
            sb_TicketlDet += '' + strData[0].Origin + '';
            sb_TicketlDet += '</span>';
            sb_TicketlDet += '</span>';
            sb_TicketlDet += '</div>';
            sb_TicketlDet += '<div class="col-lg-4 col-sm-1 col-md-1 col-xs-3 txt-al-cntr mt-10 pad-0-res">';
            sb_TicketlDet += '<div class="line-ht position-relative w-100 mg-tp-20">';
            sb_TicketlDet += '<div class="position-absolute dotsec">';
            sb_TicketlDet += '</div>';
            var JourneyTime = 0;
            for (var k = 0; k < strData.length; k++) {
                JourneyTime += Number(strData[k].JourneyTime);
            }
            sb_TicketlDet += '<div class="position-absolute time-sec">' + CalculateJourneyTime(JourneyTime) + '</div>';
            sb_TicketlDet += '<div class="position-absolute planesec">';
            sb_TicketlDet += '<i class="fa fa-plane" style="color: #ec182d;"></i>';
            sb_TicketlDet += '</div>';
            sb_TicketlDet += '</div>';
            sb_TicketlDet += '</div>';
            sb_TicketlDet += '<div class="col-lg-3 col-xs-3 col-sm-3 col-md-3 destination-city-cls destination-city-top pad-0-res">';
            sb_TicketlDet += '<span class="float-right w-100 txt-right">';
            sb_TicketlDet += '<span class="float-left w-100 font-15 themeclr-blu fnt-wt-600">' + strData[strData.length - 1].ArrivalTime.split(' ')[0] + '</span>';
            sb_TicketlDet += '<label class="fnt-wt-600-im font-13 mg-btm-0 float-right w-100">';
            sb_TicketlDet += '' + strData[strData.length - 1].DestinationCode + '';
            sb_TicketlDet += '</label>';
            sb_TicketlDet += '<br>';
            sb_TicketlDet += '<span class="font-12 mg-btm-0">' + strData[strData.length - 1].ArrivalDay + ' ' + strData[strData.length - 1].ArrivalDate + '</span>';
            sb_TicketlDet += '<span class="font-12 mg-btm-0 widget_data w-100 float-left">';
            sb_TicketlDet += '' + strData[strData.length - 1].Destination + '';
            sb_TicketlDet += '</span>';
            sb_TicketlDet += '</span>';
            sb_TicketlDet += '</div>';
            sb_TicketlDet += '</div>';
        }
        $("#dvViewTravelDet").html(sb_TicketlDet);
        //REGION TICKET DETAILS
        //REGION PAX DETAILS
        var sb_TravelDet = "";
        sb_TravelDet += '<table class="table mb-0">';
        sb_TravelDet += '<thead>';
        sb_TravelDet += '<tr>';
        sb_TravelDet += '<th scope="col" class="">Ticket No.</th>';
        sb_TravelDet += '<th scope="col" class="hidden-xs">Passenger Type</th>';
        sb_TravelDet += '<th scope="col" class="">Name</th>';
        sb_TravelDet += '<th scope="col" class="">Status</th>';
        sb_TravelDet += '<th scope="col" class="hidden-xs">Meals</th>';
        sb_TravelDet += '<th scope="col" class="hidden-xs">Baggage</th>';
        sb_TravelDet += '<th scope="col" class="hidden-xs">Seats No.</th>';
        sb_TravelDet += '</tr>';
        sb_TravelDet += '</thead>';
        sb_TravelDet += '<tbody>';
        for (var i = 0; i < strPaxDetails.length; i++) {
            var strData = strPaxDetails[i];
            for (var j = 0; j < strData.length; j++) {
                sb_TravelDet += '<tr>';
                sb_TravelDet += '<th scope="row" class="font-12 fnt-wt-600">' + strData[j].TICKET_NO + '</th>';
                if (j == 0) {
                    sb_TravelDet += '<td class="hidden-xs" rowspan="' + strData.length + '">' + strData[j].PASSENGER_TYPE + '</td>';
                    sb_TravelDet += '<td class="" rowspan="' + strData.length + '">' + strData[j].PASSENGER_NAME + '</td>';
                }
                sb_TravelDet += '<td class="">' + strData[j].STATUS + '</td>';
                sb_TravelDet += '<td class="hidden-xs">' + (strData[j].MEALS != null && strData[j].MEALS != "" ? strData[j].MEALS : "-") + '</td>';
                sb_TravelDet += '<td class="hidden-xs">' + (strData[j].BAGGAGE != null && strData[j].BAGGAGE != "" ? strData[j].BAGGAGE : "-") + '</td>';
                sb_TravelDet += '<td class="hidden-xs">' + (strData[j].SEAT != null && strData[j].SEAT != "" ? strData[j].SEAT : "-") + '</td>';
                sb_TravelDet += '</tr>';
            }
        }
        sb_TravelDet += '</tbody>';
        sb_TravelDet += '</table>';
        $("#dvViewPaxDet").html(sb_TravelDet);
        //REGION PAX DETAILS
        //REGION PAYMENT DETAILS
        var sb_PaymentDet = "";
        sb_PaymentDet += '<table class="table mb-0">';
        sb_PaymentDet += '<thead>';
        sb_PaymentDet += '<tr>';
        sb_PaymentDet += '<th scope="col" class="">Base Fare</th>';
        sb_PaymentDet += '<th scope="col" class="">Taxes &amp; Fees</th>';
        sb_PaymentDet += '<th scope="col" class="hidden-xs">Meals Amount</th>';
        sb_PaymentDet += '<th scope="col" class="hidden-xs">Baggages Amount</th>';
        sb_PaymentDet += '<th scope="col" class="hidden-xs">Seat Amount</th>';
        sb_PaymentDet += '<th scope="col" class="">Gross Fare</th>';
        sb_PaymentDet += '</tr>';
        sb_PaymentDet += '</thead>';
        sb_PaymentDet += '<tbody>';
        sb_PaymentDet += '<tr>';
        sb_PaymentDet += '<td scope="row" class="fnt-wt-500"> ₹&nbsp;' + strPaymentDetails[0].BASIC_FARE + '</td>';//txt-rt
        sb_PaymentDet += '<td class="fnt-wt-500"> ₹&nbsp;' + strPaymentDetails[0].TAX_FARE + '</td>';
        sb_PaymentDet += '<td class="fnt-wt-500 hidden-xs"> ₹&nbsp;' + strPaymentDetails[0].MEALS_FARE + '</td>';
        sb_PaymentDet += '<td class="fnt-wt-500 hidden-xs"> ₹&nbsp;' + strPaymentDetails[0].BAGGAGE_FARE + '</td>';
        sb_PaymentDet += '<td class="fnt-wt-500 hidden-xs"> ₹&nbsp;' + strPaymentDetails[0].SEAT_FARE + '</td>';
        sb_PaymentDet += '<td class="fnt-wt-600 fnt-siz-16 themeclr-blu"> ₹&nbsp;' + strPaymentDetails[0].GROSS_FARE + '</td>';
        sb_PaymentDet += '</tr>';
        sb_PaymentDet += '</tbody>';
        sb_PaymentDet += '</table>';
        $("#dvViewPaymentDet").html(sb_PaymentDet);
        //REGION PAYMENT DETAILS
        $.unblockUI();
        $("#DvBookingDetail").hide();
        $("#DvViewPNR").show();
    }
    catch (e) {
        $.unblockUI();
        $("#DvBookingDetail").show();
        $("#DvViewPNR").hide();
        $("#dvViewPaymentDet").html("");
        $("#dvViewPaxDet").html("");
        $("#dvViewTravelDet").html("");
        ShowAlert("Unable to Process your Request", 5000);
    }
}

function BackToMyBookings()
{
    $("#DvBookingDetail").show();
    $("#DvViewPNR").hide();
    $("#dvViewPaymentDet").html("");
    $("#dvViewPaxDet").html("");
    $("#dvViewTravelDet").html("");
}

function CalculateJourneyTime(JourneyTime)
{
    var hours = Math.floor(JourneyTime / 60);
    var minutes = JourneyTime % 60;
    return hours + " hrs " + minutes + " min";
}

function ViewPNR_Popup(arg) {
    var strSPNR = $("#hdn_spnr").val();
    if (arg=='M') {
        ShowMailPopup(strSPNR)
    }
    else {
        if ($(window).width() < 768) {
            PrintTicketMobile(strSPNR)
        }
        else {
            bookedhis_print(strSPNR)
        }
    }
}

function LoadCountry()
{
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + strLoaderURL + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });
    try {
        $.ajax({
            dataType: 'xml',
            url: CountryxmlURL,
            async: false,
            success: function (data) {
                var CountryData = $.xml2json(data);
                var SB_CountryData = $.map(CountryData["COUNTRYDET"], function (value, index) {
                    if (value["ID"] != "0") {
                        return "<option value='" + value["ID"] + "'>" + value["Name"] + "</option>";
                    }
                    else if (index == 0) {
                        return "<option value=''>-- Select --</option>";
                    }
                });
                $(".clsCountry").html(SB_CountryData);
                ResetProfile();
                $.unblockUI();
            },
            error: function (data) {
                console.log(data);
                $.unblockUI();
                ShowAlert("Unable to load country. please contact support team.", 5000);
                return false;
            }
        });
    }
    catch (e) {
        console.log(e);
        $.unblockUI();
        ShowAlert("Unable to Process your Request", 5000);
        return false;
    }
}

function ResetProfile() {
    CheckProfileImage(strUserProfileImg);
    $("#profile_title").val(strUserTitle.replace(".", ""));
    $("#profile_fname").val(strUserFname);
    $("#profile_lname").val(strUserLname);
    $("#profile_MobileNO").val(strUserMobileNO);
    $("#profile_Email").val(strUserEmailID);
    $("#profile_Address").val(strUserAddress);
    $("#profile_City").val(strUserCity);
    $("#profile_Country").val(strUserCountry);
    var DOBmindate = CustomDateFormat(strDate, 12, 0, 0, '-');
    var DOBmaxdate = CustomDateFormat(strDate, 100, 0, 0, '-');
    $("#profile_DOB").datepicker({
        changeYear: true, changeMonth: true, maxDate: DOBmindate, minDate: DOBmaxdate, dateFormat: "dd/mm/yy"
    });
    $("#profile_DOB").val(strUserDOB);
    var Passportmindate = GetTodayDate();
    var Passportmaxdate = CustomDateFormat(strDate, 20, 0, 0, '+');
    $("#profile_PassportExp").datepicker({
        changeYear: true, changeMonth: true, maxDate: Passportmaxdate, minDate: Passportmindate, dateFormat: "dd/mm/yy"
    });
    $("#profile_PassportNO").val(strUserPassportNo);
    $("#profile_PassportExp").val(strUserPassEXP);
    $("#profile_PassportCountry").val(strUserPassCountry);
    $("#profile_Pincode").val(strUserPincode);
}

function togglePassword() {
    if ($("#serv_checkstandard").is(":checked")) {
        $("#Old_pwdd").attr('type', 'text');
        $("#New_pwdd").attr('type', 'text');
        $("#Cnf_pwdd").attr('type', 'text');
    }
    else {
        $("#Old_pwdd").attr('type', 'password');
        $("#New_pwdd").attr('type', 'password');
        $("#Cnf_pwdd").attr('type', 'password');
    }
}

function clearPassword() {
    $("#Old_pwdd").attr('type', 'password');
    $("#New_pwdd").attr('type', 'password');
    $("#Cnf_pwdd").attr('type', 'password');
    $("#Old_pwdd").val();
    $("#New_pwdd").val();
    $("#Cnf_pwdd").val();
    $("#serv_checkstandard").prop('checked', false);
}

//Add Traveller Edit Popup
function FnEdit() {
    $("#modal-addtravel").modal({
        backdrop: 'static',
        keyboard: false
    });
}

//Send Email Popup
function SendEmail() {
    $("#modal-sendemail").modal({
        backdrop: 'static',
        keyboard: false
    });
}

//change passowrd Popup
function PasswordPopup() {
    $("#Old_pwdd,#New_pwdd,#Cnf_pwdd").val("");
    $("#serv_checkstandard").prop("checked", false);
    $("#change-password").modal({
        backdrop: 'static',
        keyboard: false
    });
}

$(document).on('change', '#UserProfileImg', function () {
    var name = "";
    if (this.files.length > 0) {
        name = this.files[0].name
        var oFReader = new FileReader();
        oFReader.readAsDataURL(this.files[0]);
        $(this).attr("data-value", name);
        oFReader.onload = function (oFREvent) {
            $('.clsUserProfileImg').attr('src', oFREvent.target.result);
        };
        console.log(name);
    }
    else {
        $('.clsUserProfileImg').attr('src', strUserProfileImg);
    }
});

function UpdatePassword() {
    var txtOld_pwdd = $("#Old_pwdd").val();
    var txtNew_pwdd = $("#New_pwdd").val();
    var txtCnf_pwdd = $("#Cnf_pwdd").val();
    if (txtOld_pwdd == "") {
        ShowAlert("Please enter old password", 5000);
        return false;
    }
    if (txtNew_pwdd == "") {
        ShowAlert("Please enter new password", 5000);
        return false;
    }
    if (txtCnf_pwdd == "") {
        ShowAlert("Please enter Confirm password", 5000);
        return false;
    }
    if (txtOld_pwdd != $('body').data('OLD_PWD')) {
        ShowAlert("Please enter correct old password", 5000);
        return false;
    }
    if (txtNew_pwdd != txtCnf_pwdd) {
        ShowAlert("new password and confirm password does not match", 5000);
        return false;
    }
    if (txtOld_pwdd == txtCnf_pwdd) {
        ShowAlert("old password and new password cannot be same", 5000);
        return false;
    }
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + strLoaderURL + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });
    var strParams = {
        strOldPassword: txtOld_pwdd,
        strNewPassword: txtNew_pwdd,
    };
    clearPassword();
    $("#change-password").modal('hide')
    try {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: strUpdatePasswordURL,
            data: JSON.stringify(strParams),
            dataType: "json",
            success: function (data) {
                $.unblockUI();
                if (data.Status == "01") {
                    $('body').data('OLD_PWD', txtNew_pwdd);
                    $("#profilepassword").val(txtNew_pwdd);
                    ShowAlert("Password Updated", 5000);
                    return false;
                }
                else {
                    var strMessage = data.Message != null && data.Message != "" ? data.Message : "Unable to update password. please contact customer care.";
                    ShowAlert(strMessage, 5000);
                    return false;
                }
            },
            error: function (data) {
                console.log(data);
                $.unblockUI();
                ShowAlert("Unable to update password. please contact customer care.", 5000);
                return false;
            }
        });
    }
    catch (e) {
        console.log(e);
        $.unblockUI();
        ShowAlert("Unable to update password. please contact customer care.", 5000);
        return false;
    }

}

function PrintTicketMobile(SPNR) {
    try {

        $.blockUI({
            message: '<img alt="Please Wait..." src="' + strLoaderURL + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });
        var param = {
            strSPNRNO: SPNR + "|yes",
            single: "no",
        }
        $.ajax({
            type: "POST",
            url: FetchPrintTicketURL,
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (json) {
                console.log(json);
                if (json.Status == "01") {
                    PrintTicketPDF(SPNR);
                }
                else {
                    showerralert("unable to download ticket.please contact support team(#00)", "", "");
                    $.unblockUI();
                }
            },
            error: function (e) {
                showerralert("unable to download ticket.please contact support team(#03)", "", "");
                $.unblockUI();
                if (e.status == "500") {
                    window.location.href = sessionExb;
                    return false;
                }
            }
        });
    }
    catch (e) {
        showerralert("unable to download ticket.please contact support team(#05)", "", "");
    }
}

function PrintTicketPDF(SPnr) {
    var param = { strPNR: SPnr }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: ConvertPDF_URL,
        data: JSON.stringify(param),
        dataType: "json",
        success: function (result) {
            var tktparse = result.Result;
            if (tktparse[1] != null && tktparse[1] != "") {
                var url = tktparse[1];
                setTimeout(function () { window.location.href = url; }, 500);
                setTimeout(function () { PrintTicketPDF(); }, 10000);
                $.unblockUI();
            }
            else if (tktparse[0] != null && tktparse[0] != "") {
                $.unblockUI();
            }
        },
        error: function (result) {
            $.unblockUI();
        }
    });
}

function CheckProfileImage(strImage) {
    try {
        $.ajax({
            url: strImage,
            error: function (data) {
                console.log(data)
                strUserProfileImg = strDefaultProfile
                $('.clsUserProfileImg').attr('src', strUserProfileImg);
            },
            success: function (data) {
                console.log(data)
                strUserProfileImg = strImage
                $('.clsUserProfileImg').attr('src', strUserProfileImg);
            }
        });
    }
    catch (e) {
        console.log(e)
        strUserProfileImg = strDefaultProfile
        $('.clsUserProfileImg').attr('src', strUserProfileImg);
    }
}

function LoadSupportDetails() {
    try {

        $.ajax({
            type: "GET",
            url: supportDetailsXM,
            async: true,
            dataType: "xml",
            success: function (xml) {
                var json = $.xml2json(xml);
                var ProductSupportdetails = json[ProductType];
                if (ProductSupportdetails != "undefined" && ProductSupportdetails != null && ProductSupportdetails != "") {
                    var Address = "";
                    if (Array.isArray(ProductSupportdetails.ADDRESS))
                    $.each(ProductSupportdetails.ADDRESS, function (i, val) {
                        Address += val.DETAILS + "<br/>";
                    })
                    else
                        Address = ProductSupportdetails.ADDRESS != undefined && ProductSupportdetails.ADDRESS.DETAILS != undefined ? ProductSupportdetails.ADDRESS.DETAILS : "";

                    var PhnNO = Array.isArray(ProductSupportdetails.CALLCENTERNO) ? ProductSupportdetails.CALLCENTERNO[0].NUMBER : ProductSupportdetails.CALLCENTERNO.NUMBER;
                    var EmailID = Array.isArray(ProductSupportdetails.ENQUIRY) ? ProductSupportdetails.ENQUIRY[0].EMAILID : ProductSupportdetails.ENQUIRY.EMAILID;
                    $('#pAddress').html(Address);
                    $('#pCallCenterNo').html(PhnNO);
                    $('#dvenquiry').html(EmailID);
                }

            },
            error: function () {
                showerralert("An error occurred while processing product support details XML file.", "", "");
            }
        });
    }
    catch (ex) {
        console.log(ex);
    }
}