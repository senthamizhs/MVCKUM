
//Region Travel Calendar by vijai 21092018
function Getcalendarbookedcounts(frmdate, todate,defaultview,Approvercountflag) {
    
    Approvercountflag = Approvercountflag != undefined && Approvercountflag != null ? Approvercountflag : "";
    $("#Fullcalendarloading").show();
    $("#Fullcalendar").hide();
    var Param = {
        LoginID: "",
        Fromdate: frmdate,
        Todate: todate,
        Approvercountflag: Approvercountflag
    }
    $.ajax({
        type: "POST",
        url: CalendarBookedCount,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(Param),
        timeout: 100000,
        dataType: "json",
        success: function (data) {
            $("#Fullcalendarloading").hide();
            $("#Fullcalendar").show();
            if (data.status == "03") {
                window.location.href = sessionpath;
            }
            else if (data.status == "00") {
                alert(data.Errormsg);
                //Lobibox.alert('info', {
                //    msg: data.Errormsg,
                //    closeOnEsc: false,
                //    callback: function ($this, type) {
                //    }
                //});
            }
            else if (data.status == "01") {
                
                if (Approvercountflag == "Y") {                     
                    FormingApproverCountDetails(data.DSBDetails);
                }
                else {
                    FormingFullCalendarDetails(data.DSBDetails, frmdate, defaultview);
                }
            }
            else {
                alert("Unable to load calendar details (#05)");
                //Lobibox.alert('info', {
                //    msg: "Unable to load calendar details (#05)",
                //    closeOnEsc: false,
                //    callback: function ($this, type) {
                //    }
                //});
            }
        },
        error: function (result) {
            $("#Fullcalendarloading").hide();
            $("#Fullcalendar").show();
            //Lobibox.alert('error', {   
            //    msg: "Unable to connect remote server",
            //    closeOnEsc: false,
            //    callback: function ($this, type) {
            //    }
            //});
            alert("Unable to connect remote server");
            return false;
        }
    });
}

function Fetchbookdetails(Bookdate, Producttype, colorcode) {
    $(".wholeanchorexporteclebtn").hide();
    if ($("#TvlClndrTotalSelectedCountDetails table").length > 0) {
        $("#TvlClndrTotalSelectedCountDetails").columns("destroy");
    }
    $("#Fullcalendar").hide();
    $("#Fullcalendarloading").show();
   
    $(".travel-cal").addClass("dvposabsclsswid100");
    $("#TvlClndrTotalSelectedCountDetails").show();
    $("#F_yearcal").hide();
    $(".trvlcalheading").html("Duty Care").addClass("calposchaclss");//Travelling Details
    $(".trvlcldpopupbtnclose").show();
      

    $("#DisplayFlightdetails").css("display", "none");
    $("#Req_Hotel").css("display", "none");
    $("#Req_Flight").css("display", "none");
    $("#Req_Rail").css("display", "none");
    $("#Req_Bus").css("display", "none");
    //Dynamic color changes
    var ds = Bookdate.split('-')[0] + Bookdate.split('-')[1] + Bookdate.split('-')[2]
    $(".ui-table-sortable").css("background", colorcode);
    $(".modal-title").css("background", colorcode);
    $(".modal-title").css("color", "#fff");
    $(".ui-table").css("border", "1px solid " + colorcode + "");

    var Param = {
        BookedDate: ds,
        Producttype: Producttype
    }
    $.ajax({
        type: "POST",
        url: CalendarBookedFlightDetails,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(Param),
        timeout: 100000,
        dataType: "json",
        success: function (data) {
            $("#Fullcalendarloading").hide();
            $("#dvcalendar").css("display", "block");
            $("#dvcalendarprogress").css("display", "none");
            if (data.Errormsg != "" && data.status == "00") {
                if (data.Errormsg == "Session") {
                      
                    window.location.href = sessionpath;
                    return false;
                }
                else {
                    //Lobibox.alert('error', {   //eg: 'error' , 'success', 'info', 'warning'
                    //    msg: data.Errormsg,
                    //    closeOnEsc: false,
                    //    callback: function ($this, type) {
                    //    }
                    //});
                    alert(data.Errormsg);
                    return false;
                }
            }
                
            if (data.status == "01" && data.DSBDetails != "") {
                
                if (data.DSBDetails != "") {
                    $("#overllcheckboxtrvlcalnid").length > 0 ? $("#overllcheckboxtrvlcalnid").prop("checked", false) : "";
                    var Ftdetails = JSON.parse(data.DSBDetails)
                    $.map(Ftdetails, function (vall, cntt) {
                        vall.UniqId = Producttype + cntt
                    });
                    globaltotcalseldetailary = [];
                    globaltotcalseldetailary.push.apply(globaltotcalseldetailary, Ftdetails);
                    ShowFlightdetails(Ftdetails, '', Producttype);
                    $("#TvlClndrTotalSelectedCountDetails").show();
                }
            }
        },
        error: function (result) {
            $("#FullcalendarloadingDet").hide();
            $("#modal-Flightdetails").modal("hide");
            //Lobibox.alert('error', {   //eg: 'error' , 'success', 'info', 'warning'
            //    msg: "Unable to connect remote server",
            //    closeOnEsc: false,
            //    callback: function ($this, type) {
            //    }
            //});
            alert("Unable to connect remote server");
            return false;
        }
    });
}

function ShowFlightdetails(Jsondata, id, Producttype) {



    if (Producttype == "Airline") {
        
        var count = 0;
        $('#TvlClndrTotalSelectedCountDetails').columns({
            size: 10,
            data: Jsondata,
            schema: [
             {
                 "header": "Select", "key": "SELECT",
                 "condition": function (val, row) {
                     
                     var build = '<input type="checkbox" data-eachuniqid=' + row.UniqId + '  class="clsTotalseltrvlcldets" id="chkgetPasngr_' + count + '" onchange="SelectedTrvlCaldetails(this.id)" />' //data-paxtype="' + passengertype + '" data-paxtitle="' + title + '" data-paxfirstname="' + firstname + '"data-paxlastname="' + lastname + '" data-paxmobno="' + mobileno + '" data-paxdob="' + DOB + '"  " data-paxemailid="' + row.PAX_EMAIL_ID_BOA + '
                     count++;
                     return build
                 }
             },
            {
                "header": "Corporate", "key": "Company", "condition": function (val, row) {
                    var b = '<span class="txt-lfths" style="white-space:nowrap" > ' + val + '</span>' //
                    return b;
                }
            },
            { "header": "PNR", "key": "PNR" },

            {
                "header": "Airline", "key": "Img_Logo", "condition": function (val, row) {
                    var b = '<span class="txt-lfths" style="white-space:nowrap"> ' + val + '</span>'
                    return b;
                }
            },
            {
                "header": "Sector", "key": "Dep_city", "condition": function (val, row) {
                    var b = '<span class="txt-lfths" style="white-space:nowrap"> ' + val + '</span>'
                    return b;
                }
            },

            {
                "header": "Departure", "key": "Dep_Time", "condition": function (val, row) {
                    var b = '<span class="txt-lfths"> ' + val + '</span>'
                    return b;
                }
            },
            {
                "header": "Arrival", "key": "Dest_Time", "condition": function (val, row) {
                    var b = '<span class="txt-lfths"> ' + val + '</span>'
                    return b;
                }
            },
             {
                 "header": "Travellername", "key": "Travellername", "condition": function (val, row) {
                     var b = '<span class="txt-lfths"> ' + val + '</span>'
                     return b;
                 }
             },
               {
                   "header": "Traveller Type", "key": "TRAVELLERTYPE", "condition": function (val, row) {
                       val = val != null && val != "" && val != "NONE" ? val : "-";
                       var b = '<span class=""> ' + val + '</span>'
                       return b;
                   }
               }

            ]
        });
    }
    if (Producttype == "Hotel") {
       
        $('#TvlClndrTotalSelectedCountDetails').columns({
            size: 100,
            data: Jsondata,
            pagination: false,
            schema: [
                  {
                      "header": "Select", "key": "SELECT",
                      "condition": function (val, row) {
                          
                          var build = '<input type="checkbox" data-eachuniqid=' + row.UniqId + '  class="clsTotalseltrvlcldets" id="chkgetPasngr_' + count + '" onchange="SelectedTrvlCaldetails(this.id)" />' //data-paxtype="' + passengertype + '" data-paxtitle="' + title + '" data-paxfirstname="' + firstname + '"data-paxlastname="' + lastname + '" data-paxmobno="' + mobileno + '" data-paxdob="' + DOB + '"  " data-paxemailid="' + row.PAX_EMAIL_ID_BOA + '
                          count++;
                          return build
                      }
                  },
                { "header": "Corporate", "key": "Company" },
                {
                    "header": "Corporate", "key": "Company", "condition": function (val, row) {
                        var b = '<span class="txt-lfths"> ' + val + '</span>'
                        return b;
                    }
                },
                { "header": "PNR", "key": "PNR" },
                {
                    "header": "Hotel Name", "key": "hOTELNAME", "condition": function (val, row) {
                        var b = '<span class="txt-lfths"> ' + val + '</span>'
                        return b;
                    }
                },
                {
                    "header": "City Name", "key": "CITYNAME", "condition": function (val, row) {
                        var b = '<span class="txt-lfths"> ' + val + '</span>'
                        return b;
                    }
                },
                {
                    "header": "Location", "key": "LOCATION", "condition": function (val, row) {
                        var b = '<span class="txt-lfths"> ' + val + '</span>'
                        return b;
                    }
                },
                { "header": "Check In Date", "key": "CHECKINDATE" },
                 { "header": "Check Out Date", "key": "CHECKOUTDATE" },
                  {
                      "header": "Username", "key": "USERNAME", "condition": function (val, row) {
                          var b = '<span class="txt-lfths"> ' + val + '</span>'
                          return b;
                      }
                  }
            ]
        });
    }
    if (Producttype == "Rail") {
       
        $('#TvlClndrTotalSelectedCountDetails').columns({
            size: 100,
            data: Jsondata,
            pagination: false,
            schema: [
                {
                    "header": "Select", "key": "SELECT",
                    "condition": function (val, row) {
                        
                        var build = '<input type="checkbox" data-eachuniqid=' + row.UniqId + '  class="clsTotalseltrvlcldets" id="chkgetPasngr_' + count + '" onchange="SelectedTrvlCaldetails(this.id)" />' //data-paxtype="' + passengertype + '" data-paxtitle="' + title + '" data-paxfirstname="' + firstname + '"data-paxlastname="' + lastname + '" data-paxmobno="' + mobileno + '" data-paxdob="' + DOB + '"  " data-paxemailid="' + row.PAX_EMAIL_ID_BOA + '
                        count++;
                        return build
                    }
                },
                { "header": "Corporate", "key": "Company" },
                {
                    "header": "Corporate", "key": "Company", "condition": function (val, row) {
                        var b = '<span class="txt-lfths"> ' + val + '</span>'
                        return b;
                    }
                },
                { "header": "PNR", "key": "PNR" },
                {
                    "header": "Train Name", "key": "TRAINNAME", "condition": function (val, row) {
                        var b = '<span class="txt-lfths"> ' + val + '</span>'
                        return b;
                    }
                },
                {
                    "header": "ORIGIN", "key": "ORIGIN", "condition": function (val, row) {
                        var b = '<span class="txt-lfths"> ' + val + '</span>'
                        return b;
                    }
                },
                {
                    "header": "DESTINATION", "key": "DESTINATION", "condition": function (val, row) {
                        var b = '<span class="txt-lfths"> ' + val + '</span>'
                        return b;
                    }
                },
                { "header": "Journey Date", "key": "JOURNEYDATE" },
                 {
                     "header": "Deaprture Time", "key": "DEPARTURETIME", "condition": function (val, row) {
                         var b = '<span class="txt-lfths"> ' + val + '</span>'
                         return b;
                     }
                 },
                  {
                      "header": "Arrival Time", "key": "ARRIVALTIME", "condition": function (val, row) {
                          var b = '<span class="txt-lfths"> ' + val + '</span>'
                          return b;
                      }
                  },
                    {
                        "header": "Username", "key": "USERNAME", "condition": function (val, row) {
                            var b = '<span class="txt-lfths"> ' + val + '</span>'
                            return b;
                        }
                    },
            ]
        });
    }
    if (Producttype == "Bus") {
       
        $('#TvlClndrTotalSelectedCountDetails').columns({
            size: 100,
            data: Jsondata,
            pagination: false,
            schema: [
                {
                    "header": "Select", "key": "SELECT",
                    "condition": function (val, row) {
                        
                        var build = '<input type="checkbox" data-eachuniqid=' + row.UniqId + '  class="clsTotalseltrvlcldets" id="chkgetPasngr_' + count + '" onchange="SelectedTrvlCaldetails(this.id)" />' //data-paxtype="' + passengertype + '" data-paxtitle="' + title + '" data-paxfirstname="' + firstname + '"data-paxlastname="' + lastname + '" data-paxmobno="' + mobileno + '" data-paxdob="' + DOB + '"  " data-paxemailid="' + row.PAX_EMAIL_ID_BOA + '
                        count++;
                        return build
                    }
                },
                
                {
                    "header": "Corporate", "key": "Company", "condition": function (val, row) {
                        var b = '<span class="txt-lfths"> ' + val + '</span>'
                        return b;
                    }
                },
                { "header": "PNR", "key": "PNR" },
                {
                    "header": "BUS Name", "key": "BUSNAME", "condition": function (val, row) {
                        var b = '<span class="txt-lfths"> ' + val + '</span>'
                        return b;
                    }
                },
                {
                    "header": "ORIGIN", "key": "ORIGIN", "condition": function (val, row) {
                        var b = '<span class="txt-lfths"> ' + val + '</span>'
                        return b;
                    }
                },
                {
                    "header": "DESTINATION", "key": "DESTINATION", "condition": function (val, row) {
                        var b = '<span class="txt-lfths"> ' + val + '</span>'
                        return b;
                    }
                },
                {
                    "header": "Deaprture Time", "key": "DEPARTUREDATE", "condition": function (val, row) {
                        var b = '<span class="txt-lfths"> ' + val + '</span>'
                        return b;
                    }
                },
                  {
                      "header": "Arrival Time", "key": "ARRIVALDATE", "condition": function (val, row) {
                          var b = '<span class="txt-lfths"> ' + val + '</span>'
                          return b;
                      }
                  },
                    {
                        "header": "Username", "key": "USERNAME", "condition": function (val, row) {
                            var b = '<span class="txt-lfths"> ' + val + '</span>'
                            return b;
                        }
                    },
            ]
        });
    }
    $(".wholeanchorexporteclebtn").show();
 
}

function FormingFullCalendarDetails(arydata, defaultdate, defaultview) {
    
    if (arydata != null && arydata != "") {
        defaultdate = defaultdate.split("/")[2] + "-" + defaultdate.split("/")[1] + "-" + defaultdate.split("/")[0];
        var starday = new Date(defaultdate);
        starday = starday.getDay();
        defaultview = defaultview == 'month' ? 'month' : defaultview == "week" ? 'listWeek' : 'listWeek'; //basicWeek
        var events = "";
        var newobjchangeary = [];
        var totdata = JSON.parse(arydata);

        totdata = totdata.P_FETCH_USERS_ALL_TRAVEL_CALENDER;
        for (var i = 0; i < totdata.length; i++) {
            var prodname = (totdata[i].PRODUCT_TYPE == "A" ? "Airline" : totdata[i].PRODUCT_TYPE == "B" ? "Bus" : totdata[i].PRODUCT_TYPE == "I" ? "Insurance" : totdata[i].PRODUCT_TYPE == "R" ? "Rail" : totdata[i].PRODUCT_TYPE)
            var iconname = (totdata[i].PRODUCT_TYPE == "A" ? "plane" : totdata[i].PRODUCT_TYPE == "B" ? "bus" : totdata[i].PRODUCT_TYPE == "I" ? "leaf" : totdata[i].PRODUCT_TYPE == "R" ? "train" : plane)
            var colorcode = (totdata[i].PRODUCT_TYPE == "A" ? "#27a5f5" : totdata[i].PRODUCT_TYPE == "B" ? "#f60" : totdata[i].PRODUCT_TYPE == "I" ? "#bf4040" : totdata[i].PRODUCT_TYPE == "R" ? "#bf4040" : '#27a5f5')
            newobjchangeary.push({
                title: prodname + " - " + totdata[i].TOTALCOUNT,//+ (totdata[i].VIPCOUNT != "" && totdata[i].VIPCOUNT != 0 ? " (VIP - " + totdata[i].VIPCOUNT + ")" : ""),
                start: moment(totdata[i].DEPARTUREDATE, "YYYYMMDD").format("YYYY-MM-DD"),//"YYYY-MM-DD"
                id: moment(totdata[i].DEPARTUREDATE, "YYYYMMDD").format("YYYY-MM-DD"),//"YYYY-MM-DD"
                icon: iconname.toLowerCase(),
                textColor: colorcode,
                color: '#fff'
            });
        }

        var showcustombtnflag = false;

        //if (apptype == "CBT" && TrvlCordinatorflag != "Y") {
        //    showcustombtnflag = true;
        //}

        $('#Fullcalendar').fullCalendar("destroy");
        $('#Fullcalendar').fullCalendar({
            eventClick: function (calEvent, jsEvent, view) {
                Fetchbookdetails(calEvent.id, calEvent.title.split("-")[0].trim(), calEvent.textColor);
            },

            customButtons: {
                planmytripbutton: {
                    text: 'Plan My Trip',
                    click: function () {
                        window.location.href = triploadfn;
                    }
                }
            },

            header: {
                left: 'listWeek,month',//today,
                center: 'prev,title,next',
                right: showcustombtnflag == true ? 'planmytripbutton' : 'none' //basicDay
            },
            firstDay: starday,
            defaultView: defaultview,
            defaultDate: defaultdate,
            fixedWeekCount: false,
            navLinks: false, // can click day/week names to navigate views
            editable: false,
            eventLimit: 2, // allow "more" link when too many events
            events: newobjchangeary,
            contentHeight: 'auto',
            eventRender: function (event, element) {
                if (event.icon) {
                    var colorcode = (event.icon == "plane" ? "#27a5f5" : event.icon == "bus" ? "#f60" : event.icon == "leaf" ? "#bf4040" : event.icon == "train" ? "#bf4040" : '#27a5f5')
                    element.find(".fc-title").prepend('<i style="padding-right:7px;background:' + colorcode + '!important; " class="fa fa-' + event.icon + '"></i>');
                }
            },
        });
    }
}

function fullcalmonthclickent() {
    
    var date = $("#Fullcalendar").fullCalendar('getDate');
    var currentdate = date._d
    var fromdate = currentdate.setMonth(currentdate.getMonth(), 1);
    var todate = currentdate.setMonth(currentdate.getMonth() + 1, 0);
    fromdate = moment(fromdate).format("DD/MM/YYYY");
    todate = moment(todate).format("DD/MM/YYYY")
    // End

    Getcalendarbookedcounts(fromdate, todate, "month")
}

function fullcalweekevent() {
    var fromdate = new Date();
    var todate = new Date().setDate(new Date().getDate() + 6);
    fromdate = moment(fromdate).format("DD/MM/YYYY");
    todate = moment(todate).format("DD/MM/YYYY");
    Getcalendarbookedcounts(fromdate, todate, 'week');
}

function Nextmonthweekfn() {
    var date = $("#Fullcalendar").fullCalendar('getDate');
    var currentdate = date._d//, y = currentdate.getFullYear(), m = currentdate.getMonth();
    var fromdate = "";
    var todate = "";
    var defaultview = "";
    if ($(".fc-month-button").hasClass("fc-state-active")) {
        fromdate = currentdate.setMonth(currentdate.getMonth() + 1, 1);
        todate = currentdate.setMonth(currentdate.getMonth() + 1, 0);
        defaultview = "month";
    }
    if ($(".fc-listWeek-button").hasClass("fc-state-active")) {
        var endOfWeek = date.endOf('week');
        var tempweek = $("#Fullcalendar").fullCalendar('getDate').endOf('week');
        fromdate = endOfWeek._d;//endOfWeek._d.clone();
        todate = tempweek._d.setDate(tempweek._d.getDate() + 6);// endOfWeek._d.getDay() + 6;
        defaultview = "week";
    }
    fromdate = moment(fromdate).format("DD/MM/YYYY");
    todate = moment(todate).format("DD/MM/YYYY")
    Getcalendarbookedcounts(fromdate, todate, defaultview)
}

function Prevmonthweekfn() {
    
    var date = $("#Fullcalendar").fullCalendar('getDate');
    var currentdate = date._d//, y = currentdate.getFullYear(), m = currentdate.getMonth();
    var fromdate = "";
    var todate = "";
    if ($(".fc-month-button").hasClass("fc-state-active")) {
        fromdate = currentdate.setMonth(currentdate.getMonth() - 1, 1);
        todate = currentdate.setMonth(currentdate.getMonth() + 1, 0);
        defaultview = "month";
    }
    if ($(".fc-listWeek-button").hasClass("fc-state-active")) {
        var startofweek = date.startOf('week');
        todate = startofweek._d;//.clone();
        todate = startofweek._d.setDate(startofweek._d.getDate() - 1);
        fromdate = startofweek._d.setDate(startofweek._d.getDate() - 6);
        defaultview = "week";
    }
    fromdate = moment(fromdate).format("DD/MM/YYYY");
    todate = moment(todate).format("DD/MM/YYYY")

    Getcalendarbookedcounts(fromdate, todate, defaultview)
}

function fullcaltodayevent() {
    
    var currentdate = new Date();
    var fromdate = "";
    var todate = "";
    var defaultview = "";
    //if ($(".fc-listWeek-button").hasClass("fc-state-active")) {
    //     fromdate = new Date();
    //     todate = currentdate.setDate(currentdate.getDate() + 6);
    //    defaultview = 'week';
    //}
    // if ($(".fc-month-button").hasClass("fc-state-active")) {        
    fromdate = currentdate.setMonth(currentdate.getMonth(), 1);
    todate = currentdate.setMonth(currentdate.getMonth() + 1, 0);
    defaultview = 'month';
    // }
    fromdate = moment(fromdate).format("DD/MM/YYYY");
    todate = moment(todate).format("DD/MM/YYYY")
    Getcalendarbookedcounts(fromdate, todate, defaultview)
}

function SelectedTrvlCaldetails(id) {
    
    if ($("#" + id).prop("checked") == false) {
        $("#overllcheckboxtrvlcalnid").attr("checked", false);
    }
    else {
        if ($('.clsTotalseltrvlcldets:checked').length == $('.clsTotalseltrvlcldets').length) {
            $("#overllcheckboxtrvlcalnid").attr("checked", true);
        }
    }
}

function trvlclnderexportexl() {
    
    var allowflag = false;
    var selectedidss = [];
    var selecteddetary = [];
    if ($("#overllcheckboxtrvlcalnid").length > 0 && $("#overllcheckboxtrvlcalnid").prop("checked")) {
        $.map(globaltotcalseldetailary, function (n, i) {
            var grepedary = $.grep(globaltotcalseldetailary, function (grepvall, grepcnt) {
                return grepvall.UniqId != "";
            });
            if (grepedary.length) {
                selecteddetary.push(grepedary);
            }
        });
    }
    else {
        $(".clsTotalseltrvlcldets").each(function () {
            if ($(this).prop("checked")) {
                allowflag = true;
                selectedidss.push($(this).data("eachuniqid"));

            }
        });
        if (allowflag == false) {
            alert("Please select atleast one travel details");
            //Lobibox.alert('warning', {   //eg: 'error' , 'success', 'info', 'warning'
            //    msg: "Please select atleast one travel details",
            //    closeOnEsc: false,
            //    callback: function ($this, type) {
            //    }
            //});
            return false;
        }
        $.map(selectedidss, function (n, i) {
            var grepedary = $.grep(globaltotcalseldetailary, function (grepvall, grepcnt) {
                return grepvall.UniqId == n;
            });
            if (grepedary.length) {
                selecteddetary.push(grepedary);
            }
        });
    }
    var totformedstrng = "";
    if (selecteddetary.length) {
        var selectedprodtype = selecteddetary[0][0].UniqId;
        totformedstrng += '<div style="width:100%; overflow:auto;" id="dvavailmail">';
        totformedstrng += '<table  style="width:100%;border:1px solid black;border-collapse:collapse;font-family:Verdana;font-size:12px;">';

        totformedstrng += '<tr style="border:1px solid gray;background-color:#c6e4ff;">'
        totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Corporate</td>';
        totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;"> PNR</td>';
        if (selectedprodtype.indexOf("Airline") != -1) {
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Airline</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Sector</td>';
            //totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Destination</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Departure</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Arrival</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Traveller Name</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Traveller Type</td>';
        } else if (selectedprodtype.indexOf("Bus") != -1) {
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Bus Name</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Origin</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Destination</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Departure Date</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Arrival Date</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">User Name</td>';
        } else if (selectedprodtype.indexOf("Hotel") != -1) {
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Hotel Name</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">City Name</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Location</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Check in date</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Check out date</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">User Name</td>';
        } else {
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Train Name</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Origin</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Destination</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Journey Date</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Departure Time</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">Arrival Time</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">User Name</td>';
        }

        totformedstrng += '</tr>';

        for (var totselval = 0; totselval < selecteddetary.length; totselval++) {
            totformedstrng += '<tr style="border:1px solid gray;background-color:#c6e4ff;">'
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].Company + '</td>';
            totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;"> ' + selecteddetary[totselval][0].PNR + '</td>';
            if (selectedprodtype.indexOf("Airline") != -1) {
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].Img_Logo + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].Dep_city + '</td>';
                //totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].Dest_city + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].Dep_Time + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].Dest_Time + ' </td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].Travellername + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + (selecteddetary[totselval][0].TRAVELLERTYPE != null && selecteddetary[totselval][0].TRAVELLERTYPE != "" && selecteddetary[totselval][0].TRAVELLERTYPE != "none" ? selecteddetary[totselval][0].TRAVELLERTYPE : "") + '</td>';

            }
            else if (selectedprodtype.indexOf("Bus") != -1) {
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].BUSNAME + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].ORIGIN + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].DESTINATION + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].DEPARTUREDATE + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].ARRIVALDATE + ' </td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].USERNAME + '</td>';
            }
            else if (selectedprodtype.indexOf("Hotel") != -1) {
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].hOTELNAME + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].CITYNAME + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].LOCATION + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].CHECKINDATE + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].CHECKOUTDATE + ' </td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].USERNAME + '</td>';
            }
            else {
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].TRAINNAME + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].ORIGIN + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].DESTINATION + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].JOURNEYDATE + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].DEPARTURETIME + ' </td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].ARRIVALTIME + '</td>';
                totformedstrng += '<td style="text-align:center;color:black;border:1px solid black;border-collapse:collapse;font-weight:bold;">' + selecteddetary[totselval][0].USERNAME + '</td>';
            }

            totformedstrng += '</tr>';
        }
        totformedstrng += '</table></div>';
    }

    var data_type = 'data:application/vnd.ms-excel';
    var a = document.createElement('a');
    a.href = data_type + ', ' + encodeURIComponent(totformedstrng);
    a.download = 'TravellingDetails.xls';
    a.click();
    $.unblockUI();
}

function overalltrvlcldecheckboxfn(arg) {
    if ($(arg).prop("checked")) {
        $(".clsTotalseltrvlcldets").prop("checked", true);
    }
    else {
        $(".clsTotalseltrvlcldets").attr("checked", false);
    }
}

function trvlcalclosefun() {
    $(".wholeanchorexporteclebtn").hide();
    $(".trvlcalheading").html("Duty Care").removeClass("calposchaclss");//Travel Calendar
    $(".travel-cal").removeClass("dvposabsclsswid100");
    $("#TvlClndrTotalSelectedCountDetails").find(".ui-columns-search").hide();
    $("#TvlClndrTotalSelectedCountDetails").hide();
    $("#Fullcalendar").show();
    $(".trvlcldpopupbtnclose").hide();
}



//End region


