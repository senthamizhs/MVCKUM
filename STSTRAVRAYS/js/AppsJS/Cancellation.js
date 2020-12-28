var RequestData = "";

function CalculateTravelDays(startdate, enddate) {
    var start = startdate.split('/');
    var end = enddate.split('/');
    startdate = new Date(start[2], Number(start[1]) - 1, start[0]);
    enddate = new Date(end[2], Number(end[1]) - 1, end[0]);
    var Traveldays = Math.floor((enddate - startdate) / (24 * 60 * 60 * 1000)) + 1;
    $("#txtTravelDays").val(Traveldays);
}

function CustomDateFormat(date, years, months, days, type) {
    var temp = date.split('/');
    date = new Date(temp[2], Number(temp[1]) - 1, temp[0]);
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

function GetTodayDate() {
    var Today = new Date();
    var TDate = (Today.getDate() < 10 ? "0" : "") + Today.getDate();
    var TMonth = ((Today.getMonth() + 1) < 10 ? "0" : "") + (Today.getMonth() + 1);
    var TYear = Today.getFullYear();
    return TDate + "/" + TMonth + "/" + TYear;
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

$(document).on('input', '.clsFare', function () {
    var txtQty = $(this).val().replace(/[^0-9\.]/g, '');
    if (txtQty.indexOf('.') != -1 && (txtQty.split('.').length > 2 || txtQty.split('.')[1].length > 2))
        txtQty = $(this).val().substr(0, $(this).val().length - 1);
    $(this).val(txtQty);
});

function showalert(msg, timout, session) {
    Lobibox.alert('success', {   //eg: 'error' , 'success', 'info', 'warning'
        msg: msg,
        closeOnEsc: false, callback: function ($this, type) {

        }
    });
    return false;
    //$('#modal-success').iziModal('destroy');
    //$("#modal-success").iziModal({
    //    title: msg,
    //    icon: 'fa fa-info',
    //    headerColor: '#5bbd72',
    //    width: "500px"
    //});
    //$('#modal-success').iziModal('open');
}

function showerralertnew(msg, timout, session, msgtype) {
    Lobibox.alert(msgtype || "info", {   //eg: 'error' , 'success', 'info', 'warning'
        msg: msg,
        closeOnEsc: false, callback: function ($this, type) {
            if (session == "S") {
                window.location.href = SessionRedrectURL;
            }
        }
    });
    return false;
    //$('#modal-alert').iziModal('destroy');
    //if (timout == null || timout == "") {
    //    $("#modal-alert").iziModal({
    //        title: msg,
    //        icon: 'fa fa-warning',
    //        headerColor: '#CCCCCC',
    //        width: "500px"
    //    });
    //}
    //else {
    //    $("#modal-alert").iziModal({
    //        title: msg,
    //        icon: 'fa fa-warning',
    //        headerColor: '#CCCCCC',
    //        width: "500px",
    //        timeout: timout
    //    });
    //}
    //$('#modal-alert').iziModal('open');
    //if (session == "S") {
    //    window.location.href = SessionRedrectURL;
    //}
}

//function LoadClient() {
//    $.blockUI({
//        message: '<img alt="Please Wait..." src=' + LoaderURL + 'style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
//        css: { padding: '5px' }
//    });
//    $.ajax({
//        type: "POST",
//        url: GetClientdetailsURL,
//        contentType: "application/json; charset=utf-8",
//        timeout: 180000,
//        dataType: "json",
//        success: function (data) {
//            $.unblockUI();
//            try {
//                var data = data["Data"];
//                if (data.Status == "00") {
//                    showerralertnew(data.Error, 5000, '');
//                    console.log(data.Error);
//                }

//                if (data.Status == "01") {
//                    arr = JSON.parse(data.Result);//
//                    if (strPlatform == "BOA") {
//                        var UniqueGroup = arr.reduce(function (memo, e1) {
//                            var matches = memo.filter(function (e2) {
//                                return e1.GROUPID == e2.GROUPID
//                            })
//                            if (matches.length == 0)
//                                memo.push(e1)
//                            return memo;
//                        }, [])
//                        $("#txtGroupName").typeahead('destroy');
//                        $('#txtGroupName').typeahead({
//                            source: UniqueGroup,
//                            items: 5,
//                            minLength: 0,
//                            displayField: "GROUPNAME",
//                            valueField: "GROUPID",
//                            scrollBar: true,
//                            onSelect: displayGroupResults,
//                        });

//                        function displayGroupResults(strGroup) {
//                            var selectedGroup = strGroup.value;
//                            var filterarr = $.grep(arr, function (obj) {
//                                return obj.GROUPID == selectedGroup;
//                            });
//                            var UniqueBranch = filterarr.reduce(function (memo, e1) {
//                                var matches = memo.filter(function (e2) {
//                                    return e1.BID == e2.BID
//                                })
//                                if (matches.length == 0)
//                                    memo.push(e1)
//                                return memo;
//                            }, [])
//                            $("#txtBranchName").typeahead('destroy');
//                            $('#txtBranchName').typeahead({
//                                source: UniqueBranch,
//                                items: 5,
//                                minLength: 0,
//                                displayField: "BNAME",
//                                valueField: "BID",
//                                scrollBar: true,
//                                onSelect: displayBranchResults,
//                            });
//                        }
//                    }
//                    var UniqueBranch = arr.reduce(function (memo, e1) {
//                        var matches = memo.filter(function (e2) {
//                            return e1.BID == e2.BID
//                        })
//                        if (matches.length == 0)
//                            memo.push(e1)
//                        return memo;
//                    }, [])
//                    $("#txtBranchName").typeahead('destroy');
//                    $('#txtBranchName').typeahead({
//                        source: UniqueBranch,
//                        items: 5,
//                        minLength: 0,
//                        displayField: "BNAME",
//                        valueField: "BID",
//                        scrollBar: true,
//                        onSelect: displayBranchResults,
//                    });
//                    function displayBranchResults(strBranch) {
//                        var selectedbranch = strBranch.value;
//                        var filterarr = $.grep(arr, function (obj) {
//                            return obj.BID == selectedbranch;
//                        });
//                        $('#txtAgentname').typeahead('destroy');
//                        $('#txtAgentname').typeahead({
//                            source: filterarr,
//                            items: 5,
//                            minLength: 0,
//                            displayField: "CLTNAME",
//                            valueField: "CLTCODE",
//                            scrollBar: true,
//                            onSelect: displayResults,
//                        });
//                    }
//                    $('#txtAgentname').typeahead('destroy');
//                    $('#txtAgentname').typeahead({
//                        source: arr,
//                        items: 5,
//                        minLength: 0,
//                        displayField: "CLTNAME",
//                        valueField: "CLTCODE",
//                        scrollBar: true,
//                        onSelect: displayResults,
//                    });
//                    function displayResults(item) {
//                        var selectedvalues = item.value;
//                        $('#hdnClientID').val(selectedvalues);
//                        $('#hdnBranchID').val(item.bid_s);
//                        var clientcount = item.CLT_CNT;
//                        var selectedid = $.grep(arr, function (obj) {
//                            return obj.CLTCODE == selectedvalues;
//                        });
//                        $("#txtBranchName").val(selectedid[0].BNAME)//.trigger('keyup');//selectedid[0].BNAME //item.bid_s
//                        if (strPlatform == "BOA") {
//                            $('#txtGroupName').val(selectedid[0].GROUPNAME)//.trigger('keyup');//selectedid[0].GROUPNAME //item.GROUPID_s
//                        }
//                    }
//                }
//                else {
//                    $('#txtAgentname').val("No clients found.")
//                }
//            }
//            catch (err) {
//                console.log(err.message);
//            }
//        },
//        error: function (e) {
//            $.unblockUI();
//            console.log(e.responseText);
//        }
//    });
//    //ELSE END
//}

function PageLoadAirlines() {
    $.ajax({
        url: AirlineNamesURL,
        dataType: "xml",
        success: function (xmlResponse) {
            var jsonResponse = $.xml2json(xmlResponse);
            jsonResponse = jsonResponse["AIRLINE_NAME"];
            jsonResponse = jsonResponse.reduce(function (item, e1) {
                var matches = item.filter(function (e2)
                { return e1._NAME == e2._NAME });
                if (matches.length == 0) {
                    item.push(e1);
                }
                return item;
            }, []);
            $("#ddlAirlines").html('<option value="">ALL</option>');
            $(jsonResponse).each(function (n, i) {
                $("#ddlAirlines").append('<option value="' + i["_CODE"] + '">' + i["_NAME"] + ' - (' + i["_CODE"] + ')</option>');
            });
            if ($("#ddlAirlines_chosen").length) {
                $('#ddlAirlines').chosen('destroy');
            }
            $("#ddlAirlines").chosen();
        },
        error: function (result) {
            showerralertnew('Unable to connect remote server.', 5000, '');
            return false;
        }
    });
}

function clearhistory() {
    $(".form-control").val("");
    $("#txtFrmDate").val(GetTodayDate());
    $("#txtTodate").val(GetTodayDate());
    $(".MisMainDivSelect").val("").trigger("chosen:updated");
    $("#chk_cancel").attr("checked", true);
    $("#chk_noshow").attr("checked", false);
    $("#btnExport").hide();
    $("#dvTickettocancel").w2destroy('Ticket_to_cancel');
    $("#dvGrid").hide();
}

function gethistory() {
    try {
        var strBranchid = $("#hdnBranchID").val();
        var strCorpType = $("#ddlAgentType").val();
        var strAgencyname = $("#txtAgentname").val();
        var strAgentID = $("#hdnClientID").val();
        var strPaymentmode = $("#ddlPaymentMode").val();
        var strAircategory = $("#ddlAircategory").val();
        var strAirportID = $("#ddlAirportType").val();
        var strCancelMode = $("#ddlCancelMode").val();
        var strFromdate = $("#txtFrmDate").val();
        var strTodate = $("#txtTodate").val();
        var strAirlinename = $("#ddlAirlines").val();
        var strRefundstatus = $("#ddlStatus").val();
        var strGroupID = $("#hdnGroupID").val();
        var strNoshow = $("#chk_noshow").is(":checked") ? "Y" : "N";
        $("#dvTickettocancel").w2destroy('Ticket_to_cancel');
        $('#dvGrid').hide();
        var param = {
            strBranchid: strBranchid,
            strCorpType: strCorpType,
            strAgencyname: strAgencyname,
            strGroupID: strGroupID,
            strAgentID: strAgentID,
            strPaymentmode: strPaymentmode,
            strAircategory: strAircategory,
            strAirportID: strAirportID,
            strCancelMode: strCancelMode,
            strFromdate: strFromdate,
            strTodate: strTodate,
            strAirlinename: strAirlinename,
            strRefundstatus: strRefundstatus,
            strNoshow: strNoshow,
        }

        $.blockUI({
            message: '<img alt="Please Wait..." src=' + LoaderURL + ' style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });

        $.ajax({
            type: "POST",
            url: GetHIstoryURL,
            dataType: "json",
            data: param,
            success: function (data) {
                $.unblockUI();
                $("#btnexport").hide();
                if (data.Status == "-1") {
                    window.location.href = SessionRedrectURL;
                }
                else if (data.Status == "1") {
                    var jsonresult = data.Result;
                    jsonresult = JSON.parse(jsonresult);
                    GridFormation(jsonresult);
                    $("#btnexport").show();
                }
                else {
                    showerralertnew("No records Found.");
                }
            },
            error: function (error) {
                $.unblockUI();
                showerralertnew("unable to process your request.try again later.(#03)")
                $("#btn_export").hide();
                console.log(error);
            }
        });
    }
    catch (error) {
        showerralertnew("unable to process your request.try again later.(#05)")
        $.unblockUI();
        $("#btn_export").hide();
        console.log(error);
    }
}

function GridFormation(jsondata) {
    try {
        $('#dvTickettocancel').show();
        $("#dvGrid").css("display", "block");
        var columns = [];
        $.each(jsondata[0], function (obj, val) {
            if (obj == "Agency_Name") {
                columns.push({
                    field: strClientLabel + " Name", caption: strClientLabel + " Name", size: "300px", resizable: true, frozen: true, sortable: true, attr: "align=left",
                    render: function (record, index, column_index) {
                        var html = record["Agency_Name"] != null && record["Agency_Name"] != "" ? record["Agency_Name"] + "(" + record["Agency_ID"] + ")" : record["Agency_ID"];
                        return html;
                    }
                });
            }
            else if (obj == "Remarks") {
                columns.push({
                    field: "Remarks", caption: "Remarks", size: "250px", resizable: true, sortable: true, attr: "align=left",
                    render: function (record, index, column_index) {
                        return (record.Remarks.indexOf("~") != -1 ? record.Remarks.split("~")[1] : record);
                    }
                });
            }
            else if (obj == "SPNR") {
                columns.push({
                    field: "SPNR", caption: strPNRLabel, size: "150px", resizable: true, frozen: true, sortable: true, attr: "align=left",
                    render: function (record, index, column_index) {
                        var html = '<a class="boa-clr" style="color:#0000ff;cursor: pointer !important;" id="txtcancelpnr' + index + '" data-pnr=' + record["SPNR"]
                            + ' onclick="javascript:getcancellationdetails(\'' + record["SPNR"] + '\',\'' + record["Sequence_No"] + '\');">' + record["SPNR"] + '</a>';
                        return html;
                    }
                });
            }
            else if (obj == "Request_Action") {
                columns.push({
                    field: "Action", caption: "Action", size: "150px", resizable: true, sortable: true, attr: "align=left",
                    render: function (record, index, column_index) {
                        var html = '<a class="boa-clr" style="color:#0000ff;cursor: pointer !important;" id="txtcancelreq' + index + '" data-pnr=' + record["SPNR"]
                            + ' onclick="javascript:ShowRemarksPopup(\'' + record["SPNR"] + '\',\'' + record["Sequence_No"] + '\')">Cancel Request</a>';
                        return html;
                    }
                });
            }
            else if (obj != "Sequence_No" && obj != "Agency_ID") {
                columns.push({ field: obj, caption: obj.replace('_', ' '), size: "150px", resizable: true, sortable: true, attr: "align=left" });
            }
        });

        var count = 0;
        for (var i = 0; i < jsondata.length; i++) {
            var object1 = {
                "recid": count++
            }
            $.extend(jsondata[i], object1);
        }
        $("#dvTickettocancel").w2destroy('Ticket_to_cancel');
        $("#dvTickettocancel").css("height", "400px");
        $("#dvTickettocancel").w2grid({
            name: 'Ticket_to_cancel',
            recordHeight: 30,
            show: { footer: true, toolbar: false },
            columns: columns,
            records: jsondata,

        });
        $("#dvTickettocancel").show();
    }
    catch (e) {
        console.log(e);
    }
}

function getcancellationdetails(strSPNR, strCancelSeq) {
    try {
        var param = {
            strSPNR: strSPNR,
            strCancelSeq: strCancelSeq,
        }

        $.blockUI({
            message: '<img alt="Please Wait..." src=' + LoaderURL + ' style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });

        $.ajax({
            type: "POST",
            url: GetToCancelDetailsURL,
            dataType: "json",
            data: param,
            success: function (data) {
                $.unblockUI();
                if (data.Status == "-1") {
                    window.location.href = SessionRedrectURL;
                }
                else if (data.Status == "1") {
                    var jsonresult = data.Result;
                    console.log(jsonresult);
                    RequestData = jsonresult[5];
                    var ticketdata = JSON.parse(jsonresult[5]);
                    $("#hdnSPNR").val(ticketdata["Table1"][0]["SPNR"]);
                    $("#hdnAirPNR").val(ticketdata["Table1"][0]["AirlinePNR"]);
                    $("#hdnCRSPNR").val(ticketdata["Table1"][0]["CRSPNR"]);
                    BindPenalityAirlineData(JSON.parse(jsonresult[0]));
                    BindPenalityTravelData(JSON.parse(jsonresult[1]), JSON.parse(jsonresult[2]), JSON.parse(jsonresult[3]));
                    BindAgencyDetails(JSON.parse(jsonresult[4]));
                }
                else {
                    $.unblockUI();
                    var msg = data.Message != null && data.Message != undefined && data.Message != "" ? data.Message : "unable to process your request.try again later.(#01)";
                    showerralertnew(data.Message);
                }
            },
            error: function (error) {
                $.unblockUI();
                showerralertnew("unable to process your request.try again later.(#03)")
                $("#btn_export").hide();
                console.log(error);
            }
        });
    }
    catch (error) {
        showerralertnew("unable to process your request.try again later.(#05)")
        $.unblockUI();
        console.log(error);
    }
}

function BindPenalityAirlineData(strAirData) {
    try {
        var sb = '';

        sb += '<label class="cls-header">';
        if (strimagesURL != null && strimagesURL != undefined && strimagesURL != "") {
            sb += '<img src="' + strimagesURL + 'PNR-Details.png" />';
        }
        sb += 'Flight Details</label>';
        sb += '<table class="table table-striped" style="margin-bottom:10px !important;">';
        sb += '<thead>';
        sb += '<tr>';
        sb += '<th class="text-center">Flight No</th>';
        sb += '<th class="text-center">Segment</th>';
        sb += '<th class="text-center">Origin</th>';
        sb += '<th class="text-center">Depature</th>';
        sb += '<th class="text-center">Destination</th>';
        sb += '<th class="text-center">Arrival</th>';
        sb += '<th class="text-center">Class</th>';
        sb += '</tr>';
        sb += '</thead>';
        sb += '<tbody>';
        for (var _Air = 0; _Air < strAirData.length; _Air++) {
            sb += '<tr>';
            sb += '<td class="text-center">' + strAirData[_Air]["Flight_No"] + '</td>';
            sb += '<td class="text-center">' + strAirData[_Air]["Segment_No"] + '</td>';
            sb += '<td class="text-center">' + strAirData[_Air]["Origin"] + '</td>';
            sb += '<td class="text-center">' + strAirData[_Air]["Depature"] + '</td>';
            sb += '<td class="text-center">' + strAirData[_Air]["Destination"] + '</td>';
            sb += '<td class="text-center">' + strAirData[_Air]["Arrival"] + '</td>';
            sb += '<td class="text-center">' + strAirData[_Air]["Class"] + '</td>';
            sb += '</tr>';
        }
        sb += '</tbody>';
        sb += '</table>';
        $("#dvPenalityAirline").html("");
        $("#dvPenalityAirline").html(sb);
        $("#dvPenalityAirline").show();
        if (strAirData[0]["VOID"] == "Y") {
            var wablr = "";
            wablr += '<div class="dv-anim input-effect m-0 m-sm-t15">';
            wablr += ' <textarea style="height:60px;width:100%;" id="txtwaivertext" class="txt-anim has-content" autocomplete="off"></textarea>';
            wablr += '   <label>Waiver Text<span style="color:RED!important;">*</span></label>';
            wablr += ' <span class="focus-border">';
            wablr += '   <i></i>';
            wablr += ' </span>';
            wablr += '  </div>';

            $(".dvclswaivertext").html(wablr).show();
            $(".dvclscancelremarks").removeClass("col-sm-offset-2"); //.addClass("col-sm-2");
        }

    }
    catch (e) {
        $("#dvPenalityAirline").html("");
        $("#dvPenalityAirline").hide();
        console.log("==================================");
        console.log("Unable to bind penality airlines data");
        console.log("Message: " + e.message);
        console.log("==================================");
        console.log(e);
        console.log("==================================");
        console.log(e.toString());
        console.log("==================================");
    }
}

function BindPenalityTravelData(strTravelData, strPenalityData, strMarkupData, Onlinepenaltyamt, OnlineRefundamt, strSSRDetails) {
    try {
        var sb = '';
        var roevalue = 1;
        var PenalityADT = 0;
        var PenalityCHD = 0;
        var PenalityINF = 0;
        var totaldebit = 0;
        var markup = 0;
        var totolcredit = 0;
        var totssramount = 0;
        strSSRDetails = strSSRDetails || [];
        var Aircategory = "";

        $.map(strSSRDetails, function (v, j) {
            totssramount += Number(v["Reverse amount"]);
        });

        strTravelData[0][0][0]["CANCELMODE"] == "O" ? $(".dvclscanceltype").html("Online Cancellation") : $(".dvclscanceltype").html("We are unable to cancel / refund the ticket online. Please make sure that do  the ticket cancellation in respective Airlines / GDS directly and calculate the penalties / refund manually.");

        strTravelData[0][0][0]["CANCELMODE"] == "O" && strTerminalType == "T" ? $(".dvclsofflineaccouting").show() : $(".dvclsofflineaccouting").hide();

        totssramount = strTravelData[0][0][0]["PARTIAL"] == "Y" ? 0 : totssramount;
        roevalue = strTravelData[0][0][0]["PCI_DISPLAY_ROE"] || 1;
        roevalue = Number(roevalue);
        Aircategory = strTravelData[0][0][0]["AirlineCateogry"] || "";

        // totolcredit += totssramount;

        BindSSRPopupDetails(strSSRDetails, strTravelData[0][0][0]["PARTIAL"]);
        // checkallssrdetails();

        $(".dvclslblretnssr").html('<label onclick=' + '"' + "javascript:return ShowSSRPopup()" + '"' + ' style="color:red!important;font-weight:600 !important;font-size:14px !important;text-decoration:underline">Return SSR<span>:</span></label>');

        sb += '<label class="cls-header">';
        if (strimagesURL != null && strimagesURL != undefined && strimagesURL != "") {
            sb += '<img src="' + strimagesURL + 'Passanger-Details.png" />';
        }
        sb += 'Ticket Details</label>';
        for (var _Pax = 0; _Pax < strTravelData.length; _Pax++) {
            var strSinglePax = strTravelData[_Pax];
            var strShowApplyAll = "";
            if (strSinglePax[0][0]["Pax_Type"] == "ADT" && PenalityADT == 0) {
                strShowApplyAll = "Y";
                PenalityADT++;
            }
            else if (strSinglePax[0][0]["Pax_Type"] == "CHD" && PenalityCHD == 0) {
                strShowApplyAll = "Y";
                PenalityCHD++;
            }
            else if (strSinglePax[0][0]["Pax_Type"] == "INF" && PenalityINF == 0) {
                strShowApplyAll = "Y";
                PenalityINF++;
            }
            var loaddata = $.grep(strMarkupData, function (obj, val) {
                return obj["PCI_PAX_REF_NO"] == strSinglePax[0][0]["Pax_Ref"];
            });
            var markup_type = "";
            if (loaddata.length > 0 && loaddata[0]["CPR_MARKUP_BASED"] == "PPT") {
                markup = loaddata[0]["CPR_MARKUP"] != null && loaddata[0]["CPR_MARKUP"] != undefined && loaddata[0]["CPR_MARKUP"] != '' ? Number(loaddata[0]["CPR_MARKUP"]) : 0;

                totaldebit += Math.ceil(Number(markup) * roevalue);
                markup_type = loaddata[0]["CPR_MARKUP_BASED"];
            }
            else if (loaddata.length > 0 && loaddata[0]["CPR_MARKUP_BASED"] == "PPI") {
                //&& strSinglePax[0][0]["TripNo"] == "ONEWAY"
                markup_type = loaddata[0]["CPR_MARKUP_BASED"];
                var Loaddata = strSinglePax[0].map(function (obj) {
                    return obj.TripNo.toUpperCase();
                });
                Loaddata = Loaddata.filter(function (v, i) {
                    return Loaddata.indexOf(v) == i;
                });
                for (var mrkp = 0; mrkp < Loaddata.length; mrkp++) {
                    markup = loaddata[0]["CPR_MARKUP"] != null && loaddata[0]["CPR_MARKUP"] != undefined && loaddata[0]["CPR_MARKUP"] != '' ? Number(loaddata[0]["CPR_MARKUP"]) : 0;

                    totaldebit += Math.ceil(Number(markup) * roevalue);
                }
            }
            sb += '<table class="table table-striped">';
            sb += '<tbody>';
            sb += '<tr>';
            sb += '<td class="text-left">' + strSinglePax[0][0]["Pax_Name"] + '</td>';
            sb += '</tr>';
            sb += '</tbody>';
            sb += '</table>';
            for (var _Fare = 0; _Fare < strSinglePax.length; _Fare++) {
                var strSegment = "";
                var strData = strSinglePax[_Fare];
                var strSector = "";
                strSector = strData[0]["OriginCode"].toString();
                for (var _Sec = 0; _Sec < strData.length; _Sec++) {
                    strSector += " - " + strData[_Sec]["DestinationCode"];
                    strSegment += "" + strData[_Sec]["Segment_No"] + ",";
                }

                var loaddata = $.grep(strMarkupData, function (obj, val) {
                    return obj["PCI_PAX_REF_NO"] == strSinglePax[0][0]["Pax_Ref"];
                });
                if (loaddata.length > 0 && loaddata[0]["CPR_MARKUP_BASED"] == "PPS") {
                    for (var _Sec = 0; _Sec < strData.length; _Sec++)
                        markup = loaddata[0]["CPR_MARKUP"] != null && loaddata[0]["CPR_MARKUP"] != undefined && loaddata[0]["CPR_MARKUP"] != '' ? Number(loaddata[0]["CPR_MARKUP"]) : 0;//CPR_PENALTY

                    totaldebit += Math.ceil(Number(markup) * roevalue);
                    markup_type = loaddata[0]["CPR_MARKUP_BASED"];
                }

                strSegment = strSegment.substring(0, strSegment.length - 1);
                sb += '<table class="table table-striped" style="margin-bottom:10px !important;">';
                sb += '<thead>';
                sb += '<tr>';
                sb += '<th class="text-center">Ticket No</th>';
                sb += '<th class="text-center">Sector</th>';
                sb += '<th class="text-center">Status</th>';
                sb += '<th class="text-center" style="width: 100px;">Basic Fare</th>';
                sb += '<th class="text-center" style="width: 100px;">Tax Fare</th>';
                sb += '<th class="text-center" style="width: 100px;">Gross Fare</th>';
                if (strTerminalType == "T") {
                    sb += '<th class="text-center">Supplier Penality</th>';
                    sb += '<th class="text-center">Cancellation charges</th>';
                    sb += '<th class="text-center">Cancellation markup</th>';
                    sb += '<th class="text-center ' + (Aircategory != "LCC" ? "" : "dis-none") + ' ">Refund Amount</th>';
                }
                else if (strTerminalType == "W" && Aircategory == "LCC") {
                    sb += '<th class="text-center">Supplier Penality</th>';
                }
                sb += '</tr>';
                sb += '</thead>';
                sb += '<tbody>';
                sb += '<tr>';
                sb += '<td class="text-center">' + strData[0]["Ticket_No"] + '</td>';
                sb += '<td class="text-center">' + strSector + '</td>';
                sb += '<td class="text-center">' + strData[0]["Status"] + '</td>';
                sb += '<td class="text-center">' + Math.ceil(Number(strData[0]["BasicFare"]) * roevalue) + '</td>';
                sb += '<td class="text-center">' + Math.ceil(Number(strData[0]["TaxFare"]) * roevalue) + '</td>';
                if (Number(strData[0]["GrossFare"]) > 0 && strTerminalType == "T" && Aircategory != "LCC") {
                    sb += '<td class="text-center"><a class="boa-clr" style="color:#0000ff;cursor: pointer !important;text-decoration:underline;font-weight: 600;font-size: 15px;" id="txt_pen_Gross_' + (_Pax + 1) + '_' + (_Fare + 1) + '" data-breakup="' + strData[0]["FareBreakup"] + '" data-partial="' + strData[0]["PARTIAL"] + '" onclick="javascript:ShowPenalityPopup(\'txtPenalityRefund_' + (_Pax + 1) + '_' + (_Fare + 1) + '\',\'' + strShowApplyAll + '\',\'' + strData[0]["FareBreakup"] + '\',\'' + (_Pax + 1) + '\',\'' + (_Fare + 1) + '\',\'' + strData[0]["Pax_Type"] + '\',\'' + strData[0]["PCI_DISPLAY_ROE"] + '\');">' + (Math.ceil(Number(strData[0]["GrossFare"]) * roevalue)) + '</a></td>';
                }
                else {
                    sb += '<td class="text-center">' + (Math.ceil(Number(strData[0]["GrossFare"]) * roevalue)) + '</td>';
                }
                totolcredit += (Math.ceil(Number(strData[0]["GrossFare"]) * roevalue));
                var penality = 0;
                Onlinepenaltyamt = Onlinepenaltyamt || "";
                if (Aircategory == "LCC" && strSinglePax[0][0]["Pax_Type"] == "INF") { // told by vasan and dhanasekar
                    penality = 0;
                }
                else if (Onlinepenaltyamt == "") {
                    var loaddata = $.grep(strPenalityData, function (obj, val) {
                        return obj["PCI_SEGMENT_NO"] == strData[0]["Segment_No"] && obj["PCI_PAX_REF_NO"] == strData[0]["Pax_Ref"];
                    });
                    if (loaddata.length > 0) {
                        penality = loaddata[0]["CPR_PENALTY"] != null && loaddata[0]["CPR_PENALTY"] != undefined && loaddata[0]["CPR_PENALTY"] != '' ? Number(loaddata[0]["CPR_PENALTY"]) : 0;//CPR_PENALTY
                    }
                }
                else {
                    penality = Number(Onlinepenaltyamt);
                }

                

                totaldebit += Math.ceil(Number(penality) * roevalue);
               
                var clsDispnone = strTerminalType == "T" ? "" : "dis-none";
                var clsDisabled = strTerminalType == "T" ? "" : "disabled";
                sb += '<td class="text-center ' + clsDispnone + '"><input data-roevalue =' + roevalue + ' data-val=' + penality + ' maxlength="10"  data-type="' + markup_type + '" id="txt_Penality_Supp' + (_Pax + 1) + '_' + (_Fare + 1) + '" value="' + Math.ceil(penality * roevalue) + '" class="txt-anim form-control clsNumeric clsDebit text-right" type="text" autocomplete="off" style="text-transform: uppercase" /></td>';
                sb += '<td class="text-center ' + clsDispnone + '"><input data-roevalue =' + roevalue + ' data-val="0" maxlength="10"  data-type="' + markup_type + '" id="txtPenalityCharge' + (_Pax + 1) + '_' + (_Fare + 1) + '" value="0" class="txt-anim form-control clsNumeric clsDebit text-right" type="text" autocomplete="off" style="text-transform: uppercase" /></td>';
                sb += '<td class="text-center ' + clsDispnone + '"><input data-roevalue =' + roevalue + ' data-val=' + markup + ' maxlength="10"  data-type="' + markup_type + '" id="txtPenalityMarkup' + (_Pax + 1) + '_' + (_Fare + 1) + '" value="' + Math.ceil(markup * roevalue) + '" class="txt-anim form-control clsNumeric clsDebit text-right" type="text" autocomplete="off" style="text-transform: uppercase" /></td>';
                if (strTerminalType == "W" && Aircategory == "LCC") {
                    sb += '<td class="text-center"> <label style="font-size:14px !important;">' + (Math.ceil(penality * roevalue) + Math.ceil(markup * roevalue)) + '</label> </td>';
                }
                var RefundFare = 0;
                //OnlineRefundamt = OnlineRefundamt || "";
                // if (OnlineRefundamt == "") {
                var RefundBreakup = strData[0]["FareBreakup"] ? strData[0]["FareBreakup"].split('/') : [];
                for (var _Brkp = 0; _Brkp < RefundBreakup.length; _Brkp++) {
                    RefundFare += Number(RefundBreakup[_Brkp].split(':')[1]);
                }
                //}
                //else {
                //    RefundFare = Number(OnlineRefundamt);
                //}

                RefundFare = strData[0]["PARTIAL"] == "Y" ? 0 : RefundFare;
                RefundFare = Math.ceil(RefundFare * roevalue);
                totaldebit + (Math.ceil(Number(strData[0]["GrossFare"]) * roevalue)) - RefundFare;
                // totolcredit += Math.ceil(RefundFare * roevalue);
                sb += '<td class="text-center ' + (Aircategory != "LCC" ? "" : "dis-none") + ' ' + clsDispnone + '"><input data-val="' + RefundFare + '" data-partial="' + strData[0]["PARTIAL"] + '" maxlength="15" ' + clsDisabled + ' data-breakup="' + strData[0]["FareBreakup"] + '" data-penalityid="txt_Penality_Supp' + (_Pax + 1) + '_' + (_Fare + 1) + '" data-chargesid="txtPenalityCharge' + (_Pax + 1) + '_' + (_Fare + 1) + '" data-fareid="' + strData[0]["FareID"] + '" data-segments="' + strSegment + '" data-markupid="txtPenalityMarkup' + (_Pax + 1) + '_' + (_Fare + 1) + '" data-fare="0" data-pax="' + strData[0]["Pax_Ref"] + '" id="txtPenalityRefund_' + (_Pax + 1) + '_' + (_Fare + 1) + '" value="' + Math.ceil(RefundFare * roevalue) + '" class="txt-anim form-control clsBreakup ' + strData[0]["Pax_Type"] + ' clsNumeric text-right" type="text" autocomplete="off" style="text-transform: uppercase" disabled/></td>';

                sb += '</tr>';
                sb += '</tbody>';
                sb += '</table>';

            }
        }
        $("#lbldebit").html(totaldebit);
        totssramount = Math.ceil(totssramount * roevalue);
      //  totolcredit = totolcredit + Math.ceil(totssramount * roevalue);
        $("#lblcredit").html(totolcredit + totssramount).data("creditamount", totolcredit).data("returnssramount", totssramount);
        $("#lblretrunssr").html(totssramount);


        $("#dvPenalityTravel").html("");
        $("#dvPenalityTravel").html(sb);
        $("#dvPenalityTravel").show();
        $("#dvPenality").show();
        $("#can_viewpnrShowDiv").hide();
        $("#dvRequest").hide();

        if (strTerminalType == "W")
            $(".dvclscreditdebit").hide();

    } catch (e) {
        $("#dvPenalityTravel").html("");
        $("#dvPenalityTravel").hide();
        $("#dvPenality").hide();
        showerralertnew("Problem occured while processing.please contact support team(#09).");
        console.log("==================================");
        console.log("Unable to bind penality travel data");
        console.log("Message: " + e.message);
        console.log("==================================");
        console.log(e);
        console.log("==================================");
        console.log(e.toString());
        console.log("==================================");
    }
}

function BindAgencyDetails(strAgentDetails) {
    try {
        var sb = '';

        sb += '<label class="cls-header" onclick="$(\'.clsdvPenalityAgent\').slideToggle();" style="border-bottom: 2px solid #b00002;">';
        if (strimagesURL != null && strimagesURL != undefined && strimagesURL != "") {
            sb += '<img src="' + strimagesURL + 'Contact-Details.png" />';
        }
        sb += 'Agency Details<span style="text-align: right;float: right;">More details <i class="fa fa-angle-double-down"></i></span></label>';
        sb += '<div class="col-xs-12 col-sm-6 clsdvPenalityAgent" style="padding: 5px;display:none;">';
        sb += '<table class="table table-striped">';
        sb += '<tbody>';
        sb += '<tr>';
        sb += '<td><label style="color:red!important;font-weight:600 !important;font-size:14px !important;">' + strPNRLabel + '</label></td>';
        sb += '<td><label style="color:#000!important;font-weight:600 !important;font-size:14px !important;">' + strAgentDetails[0]["SPNR"] + '</label></td>';
        sb += '</tr>';
        sb += '<tr>';
        sb += '<td><label style="color:red!important;font-weight:600 !important;font-size:14px !important;">CRS PNR</label></td>';
        sb += '<td><label style="color:#000!important;font-weight:600 !important;font-size:14px !important;">' + strAgentDetails[0]["CRSPNR"] + '</label></td>';
        sb += '</tr>';
        sb += '<tr>';
        sb += '<td><label style="color:red!important;font-weight:600 !important;font-size:14px !important;">Airline PNR</label></td>';
        sb += '<td><label style="color:#000!important;font-weight:600 !important;font-size:14px !important;">' + strAgentDetails[0]["AirlinePNR"] + '</label></td>';
        sb += '</tr>';
        sb += '<tr>';
        sb += '<td><label style="color:red!important;font-weight:600 !important;font-size:14px !important;">Airline Name</label></td>';
        sb += '<td><label style="color:#000!important;font-weight:600 !important;font-size:14px !important;">' + strAgentDetails[0]["Airlines"] + '</label></td>';
        sb += '</tr>';
        sb += '<tr>';
        sb += '<td><label style="color:red!important;font-weight:600 !important;font-size:14px !important;">Payment Mode</label></td>';
        sb += '<td><label style="color:#000!important;font-weight:600 !important;font-size:14px !important;">' + strAgentDetails[0]["PaymentMode"] + '</label></td>';
        sb += '</tr>';
        sb += '<tr>';
        sb += '<td><label style="color:red!important;font-weight:600 !important;font-size:14px !important;">' + strClientLabel + ' Balance</label></td>';
        sb += '<td><label style="color:#000!important;font-weight:600 !important;font-size:14px !important;">' + strAgentDetails[0]["AgencyBalance"] + '</label></td>';
        sb += '</tr>';
        sb += '</tbody>';
        sb += '</table>';
        sb += '</div>';

        sb += '<div class="col-xs-12 col-sm-6 clsdvPenalityAgent" style="padding: 5px;display:none;">';
        sb += '<table class="table table-striped">';
        sb += '<tbody>';
        sb += '<tr>';
        sb += '<td><label style="color:red!important;font-weight:600 !important;font-size:14px !important;">' + strClientLabel + ' Name</label></td>';
        sb += '<td><label style="color:#000!important;font-weight:600 !important;font-size:14px !important;">' + strAgentDetails[0]["AgencyName"] + '</label></td>';
        sb += '</tr>';
        sb += '<tr>';
        sb += '<td><label style="color:red!important;font-weight:600 !important;font-size:14px !important;">' + strClientLabel + ' ID</label></td>';
        sb += '<td><label style="color:#000!important;font-weight:600 !important;font-size:14px !important;">' + strAgentDetails[0]["AgentID"] + '</label></td>';
        sb += '</tr>';
        sb += '<tr>';
        sb += '<td><label style="color:red!important;font-weight:600 !important;font-size:14px !important;">Terminal ID</label></td>';
        sb += '<td><label style="color:#000!important;font-weight:600 !important;font-size:14px !important;">' + strAgentDetails[0]["TerminalID"] + '</label></td>';
        sb += '</tr>';
        sb += '<tr>';
        sb += '<td><label style="color:red!important;font-weight:600 !important;font-size:14px !important;">Email ID</label></td>';
        sb += '<td><label style="color:#000!important;font-weight:600 !important;font-size:14px !important;">' + strAgentDetails[0]["EmailID"] + '</label></td>';
        sb += '</tr>';
        sb += '<tr>';
        sb += '<td><label style="color:red!important;font-weight:600 !important;font-size:14px !important;">Phone No</label></td>';
        sb += '<td><label style="color:#000!important;font-weight:600 !important;font-size:14px !important;">' + strAgentDetails[0]["PhoneNo"] + '</label></td>';
        sb += '</tr>';
        sb += '<tr>';
        sb += '<td><label style="color:red!important;font-weight:600 !important;font-size:14px !important;">Address</label></td>';
        sb += '<td><label style="color:#000!important;font-weight:600 !important;font-size:14px !important;">' + strAgentDetails[0]["Address"] + '</label></td>';
        sb += '</tr>';
        sb += '</tbody>';
        sb += '</table>';
        sb += '</div>';

        $("#dvPenalityAgent").html("");
        $("#dvPenalityAgent").html(sb);
        $("#dvPenalityAgent").show();
    }
    catch (e) {
        $("#dvPenalityAgent").html("");
        $("#dvPenalityAgent").hide();
        console.log("==================================");
        console.log("Unable to bind penality agent data");
        console.log("Message: " + e.message);
        console.log("==================================");
        console.log(e);
        console.log("==================================");
        console.log(e.toString());
        console.log("==================================");
    }
}

$(document).on('input', '.clsFare', function () {
    var txtQty = $(this).val().replace(/[^0-9]/g, '');
    if (Number(txtQty[0]) == 0) {
        txtQty = txtQty.substring(1, txtQty.length);
    }
    if (Number(txtQty) >= Number($(this).data('fare'))) {
        txtQty = $(this).data('fare');
    }
    $(this).val(txtQty);
});

$(document).on('input', '.clsNumeric', function () {
    var txtQty = $(this).val().replace(/[^0-9]/g, '');
    $(this).val(txtQty);
});

$(document).on('input', '.clsDebit', function () {
    CalculateTotalCreditDebit();
    //var TotalDebit = 0;

    //$('.clsDebit').each(function () {
    //    var roevalue = $(this).data("roevalue") || 1;
    //    roevalue = Number(roevalue);
    //    $(this).data("val", Math.ceil(Number($(this).val()) / roevalue));
    //    TotalDebit += Number($(this).val());
    //});
    //$("#lbldebit").html(TotalDebit);
});

function ShowPenalityPopup(ids, ApplyALL, strBreakUp, pax, fare, paxtype, roevalue) {
    try {
        var strData = strBreakUp.split('/');
        roevalue = roevalue || 1;
        roevalue = Number(roevalue);
        var sb = '';
        sb += '<div class="row" style="background-color:#CCC !important;padding: 10px;font-size: 16px;font-weight: 600;">';
        sb += '<div class="col-xs-12 col-sm-offset-1 col-sm-3">Fare Code</div>'
        sb += '<div class="col-xs-12 col-sm-4">Sales Fare</div>'
        sb += '<div class="col-xs-12 col-sm-4">Refund Amount</div>'
        sb += '</div>'
        if (Number($("#" + ids).val()) != 0) {
            for (var _Brkp = 0; _Brkp < strData.length; _Brkp++) {
                sb += '<div class="row" style="padding: 10px;font-size: 16px;font-weight: 600;">';
                var previousbrkp = $("#" + ids).data('breakup').split('/');
                var partialvalue = $("#" + ids).data('partial');
                var currentvalue = Number(strData[_Brkp].split(':')[1]);
                var blnvalue = 'disabled="false"';
                var checked = '';
                for (var i = 0; i < previousbrkp.length; i++) {
                    if (strData[_Brkp].split(':')[0] == previousbrkp[i].split(':')[0]) {
                        currentvalue = Number(previousbrkp[i].split(':')[1]);
                        blnvalue = '';
                        checked = 'checked'
                    }
                }
                checked = partialvalue == "Y" ? "" : checked;

                sb += '<div class="col-xs-12 col-sm-2 clsalign checkbox">'
                sb += '<label for="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '">';
                sb += '<input type="checkbox" id="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '" ' + checked + ' data-ids="txt_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '" class="cb clsPenality">';
                //sb += '<span class="cr"><i class="cr-icon fa fa-check"></i></span>';
                sb += '<label class="lbl" style="padding-left: 5px !important;font-weight:900;" for="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '"></label>';
                sb += '</label>';
                //sb += '<input style="display: none;" ' + checked + ' id="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '" data-ids="txt_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '" class="cb clsPenality" type="checkbox">'
                //sb += '<label class="cbx" for="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '" style="margin-bottom: 3px;"></label>'
                sb += '</div>'
                sb += '<div class="col-xs-12 col-sm-2">'
                sb += '<label for="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '">' + (strData[_Brkp].split(':')[0] == "BF" ? "Basic Fare" : strData[_Brkp].split(':')[0]) + '</label>'
                sb += '</div>'
                sb += '<div class="col-xs-12 col-sm-4">'
                sb += '<label for="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '">' + Math.ceil(strData[_Brkp].split(':')[1] * roevalue) + '</label>';
                sb += '</div>'
                sb += '<div class="col-xs-12 col-sm-4">'
                sb += '<input type="text" class="clsFare form-control" id="txt_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '" data-code="' + strData[_Brkp].split(':')[0] + '" data-fare=' + strData[_Brkp].split(':')[1] + ' value="' + (Math.ceil(currentvalue * roevalue)) + '" ' + blnvalue + ' />';
                sb += '</div>'

                sb += '</div>'
            }
        }
        else {
            for (var _Brkp = 0; _Brkp < strData.length; _Brkp++) {
                sb += '<div class="row" style="padding: 10px;font-size: 16px;font-weight: 600;">';

                sb += '<div class="col-xs-12 col-sm-2 clsalign checkbox">'
                sb += '<label for="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '">';
                sb += '<input type="checkbox" id="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '" ' + checked + ' data-ids="txt_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '" class="cb clsPenality">';
                sb += '<span class="cr"><i class="cr-icon fa fa-check"></i></span>';
                sb += '<label class="lbl" style="padding-left: 5px !important;font-weight:900;" for="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '"></label>';
                sb += '</label>';
                //sb += '<input style="display: none;" id="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '" data-ids="txt_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '" class="cb clsPenality" type="checkbox">'
                //sb += '<label class="cbx" for="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '" style="margin-bottom: 3px;"></label>'
                sb += '</div>'
                sb += '<div class="col-xs-12 col-sm-2">'
                sb += '<label for="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '">' + (strData[_Brkp].split(':')[0] == "BF" ? "Basic Fare" : strData[_Brkp].split(':')[0]) + '</label>'
                sb += '</div>'
                sb += '<div class="col-xs-12 col-sm-4">'
                sb += '<label for="chk_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '">' + Math.ceil(Number(strData[_Brkp].split(':')[1]) * roevalue) + '</label>';
                sb += '</div>'
                sb += '<div class="col-xs-12 col-sm-4">'
                sb += '<input type="text" class="clsFare form-control" id="txt_' + strData[_Brkp].split(':')[0] + '_' + pax + '_' + fare + '" data-code="' + strData[_Brkp].split(':')[0] + '" data-fare=' + strData[_Brkp].split(':')[1] + ' value="' + Math.ceil(Number(strData[_Brkp].split(':')[1]) * roevalue) + '" disabled="true" />';
                sb += '</div>'

                sb += '</div>'
            }
        }

        sb += '<div class="row m-0 modal-footer">';
        sb += '<div class="col-sm-4 col-xs-4 text-right">';
        if (ApplyALL == "Y")
            sb += '<input type="button" style="width: 100%;" id="btn_Pen_ApplyALL" class="btn shadow animate color c-button" onclick="SaveBreakup(\'ALL\',\'\',\'\',\'' + paxtype + '\',\'' + roevalue + '\');" value="Apply to ALL" />';
        sb += '</div>';
        sb += '<div class="col-sm-4 col-xs-4 text-right">';
        sb += '<input type="button" style="width: 100%;" id="btn_Pen_Apply" class="btn shadow animate color c-button" onclick="SaveBreakup(\'\',\'' + pax + '\',\'' + fare + '\',\'\',\'' + roevalue + '\')" value="Apply" />';
        sb += '</div>';
        sb += '<div class="col-sm-4 col-xs-4 text-left">';
        sb += '<input type="button" style="width: 100%;" id="btn_Pen_Cancel"  class="btn shadow animate colorB1" data-dismiss="modal" onclick="$(\'#modal_Penality_Breakup\').modal(\'hide\');" value="Cancel" />';
        sb += '</div>';
        sb += '</div>';
        $("#dvBreakup").html(sb);

        $("#modal_Penality_Breakup").modal({
            backdrop: 'static',
            keyboard: false
        });
        //$("#modal_Penality_Breakup").iziModal({
        //    title: "Penality Breakup",
        //    icon: 'fa fa-info',
        //    headerColor: '#CCCCCC',
        //    width: "700px"
        //});
        //$('#modal_Penality_Breakup').iziModal('open');

    }
    catch (e) {
        showerralertnew("Problem occured while processing breakup.please contact support team(#09).");
        console.log("==================================");
        console.log("Unable to bind penality popup data");
        console.log("Message: " + e.message);
        console.log("==================================");
        console.log(e);
        console.log("==================================");
        console.log(e.toString());
        console.log("==================================");
    }

}

function BindSSRPopupDetails(ssrdetails, partialflag) {
    var ssrbldr = "";
    ssrbldr += '<div class="row" style="background-color:#CCC !important;padding: 10px;font-size: 16px;font-weight: 600;">';
    ssrbldr += '<div  class="col-xs-12  col-sm-1">';

    ssrbldr += '<label for="chk_allssrdetails">';
    ssrbldr += '<input ' + (partialflag == "Y" ? "" : 'checked') + ' onchange="checkallssrdetails()" type="checkbox" id="chk_allssrdetails"> All';
    ssrbldr += '</label>';
    ssrbldr += '</div>'
    ssrbldr += '<div class="col-xs-12  col-sm-3">Passenger</div>'
    ssrbldr += '<div class="col-xs-12 col-sm-1">Segment</div>'
    ssrbldr += '<div class="col-xs-12 col-sm-1">SSR Type</div>'
    ssrbldr += '<div class="col-xs-12 col-sm-2">SSR Name</div>'
    ssrbldr += '<div class="col-xs-12 col-sm-2">Actual amount</div>'
    ssrbldr += '<div class="col-xs-12 col-sm-2">Reverse amount</div>'
    ssrbldr += '</div>'
    for (var ssrcnt = 0; ssrcnt < ssrdetails.length; ssrcnt++) {
        ssrbldr += '<div class="row" style="padding: 10px;font-weight: 500;">';

        var checked = partialflag == "Y" ? "" : 'checked';


        ssrbldr += '<div class="col-xs-12 col-sm-1 clsalign checkbox">'
        ssrbldr += '<label for="chk_' + ssrdetails[ssrcnt].PAXID + '_' + ssrdetails[ssrcnt].seq1 + '">';
        ssrbldr += '<input data-reverseamount = "' + (ssrdetails[ssrcnt]["Reverse amount"]) + '" onchange="chckcancelallssrdetails()" type="checkbox" id="chk_' + ssrdetails[ssrcnt].PAXID + '_' + ssrdetails[ssrcnt].seq1 + '" ' + checked + '  class="cb clscmnchkssrdetails">';
        ssrbldr += '</label>';

        ssrbldr += '</div>'
        ssrbldr += '<div class="col-xs-12 col-sm-3">' + (ssrdetails[ssrcnt].Passenger)

        ssrbldr += '</div>'
        ssrbldr += '<div class="col-xs-12 col-sm-1">' + (ssrdetails[ssrcnt].Segment)

        ssrbldr += '</div>'
        ssrbldr += '<div class="col-xs-12 col-sm-1">' + (ssrdetails[ssrcnt]["SSR Type"])

        ssrbldr += '</div>'

        ssrbldr += '<div class="col-xs-12 col-sm-2">' + (ssrdetails[ssrcnt]["SSR Name"] || "-")

        ssrbldr += '</div>'

        ssrbldr += '<div class="col-xs-12 col-sm-2">' + (ssrdetails[ssrcnt]["Actual amount"])
        //  ssrbldr += '<label>'  '</label>'
        ssrbldr += '</div>'

        ssrbldr += '<div class="col-xs-12 col-sm-2">' + (ssrdetails[ssrcnt]["Reverse amount"])

        ssrbldr += '</div>'

        ssrbldr += '</div>'
    }

    $("#dvssrdetails").html(ssrbldr);
}

function ShowSSRPopup() {


    $("#modal_ssr_details").modal({
        backdrop: 'static',
        keyboard: false
    });
}

function SaveBreakup(arg, pax, fare, type, roevalue) {
    var value = 0;
    roevalue = roevalue || 1;
    var breakup = "";
    $('.clsPenality').each(function (obj, val) {
        if ($(this).is(":checked")) {
            var ids = $(this).data('ids');
            breakup += $("#" + ids).data('code') + ":" + ($("#" + ids).val() || "0") + "/"; //$("#" + ids).val()
            value += Number($("#" + ids).val());
        }
    });
    breakup = breakup.substring(0, breakup.length - 1);
    if (arg == "ALL") {
        $('.clsBreakup').each(function (obj, val) {
            if ($(this).hasClass(type)) {
                $(this).val(value);
                $(this).data("val", Math.ceil(Number(value) * roevalue));
                $(this).data("breakup", breakup);
            }
        });
    }
    else {
        $("#txtPenalityRefund_" + pax + "_" + fare + "").val(value);
        $("#txtPenalityRefund_" + pax + "_" + fare + "").data("val", Math.ceil(Number(value) * roevalue));
        $("#txtPenalityRefund_" + pax + "_" + fare + "").data("breakup", breakup);
    }
    CalculateTotalCreditDebit();
   
    //$("#modal_Penality_Breakup").hide();
    $('#modal_Penality_Breakup').modal('hide')
    //$('#modal_Penality_Breakup').iziModal('close');
}

function CalculateTotalCreditDebit() {
    var TotalRefund = 0;
    var TotalCreditAmount = $("#lblcredit").data("creditamount") || 0;
    var TotalSSRAmount = $("#lblcredit").data("returnssramount") || 0;
    TotalCreditAmount = Number(TotalCreditAmount) + Number(TotalSSRAmount);
    $('.clsBreakup').each(function (obj, val) {
        TotalRefund += Number($(this).val());
    });

    var TotalDebit = 0;
    $('.clsDebit').each(function () {
        var roevalue = $(this).data("roevalue") || 1;
        roevalue = Number(roevalue);
        $(this).data("val", Math.ceil(Number($(this).val()) / roevalue));
        TotalDebit += Number($(this).val());
    });

    var totaldebitamount = (TotalCreditAmount - TotalRefund) + TotalDebit;

    $("#lbldebit").html(totaldebitamount);
    $("#lblcredit").html(TotalCreditAmount + TotalSSRAmount).data("returnssramount", TotalSSRAmount);
}

    $(document).on('change', '.clsPenality', function () {
        var ids = $(this).data('ids');
        if ($(this).is(":checked")) {
            $("#" + ids).attr("disabled", false);
        }
        else {
            $("#" + ids).attr("disabled", true);
        }
    });

    function ConfirmCancellation(Cancelflag, remarksid, skipconfirmation, RefundFlag) {
        var strCancellationdata = "";
        try {
            var Waivertext = $("#txtwaivertext").length ? $("#txtwaivertext").val() : "";
            var Remarks = $("#" + remarksid).val().trim();
            var S_PNR = $("#hdnSPNR").val().trim();
            var airline_pnr = $("#hdnAirPNR").val().trim();
            var crs_pnr = $("#hdnCRSPNR").val().trim();
            var strCancelSeq = $("#hdnCancelSeq").val().trim();
            if (Remarks == "") {
                showerralertnew("Please enter remarks");
                return false;
            }
            if ($("#txtwaivertext").length && Waivertext == "") {
                showerralertnew("Please enter waiver text");
                return false;
            }

            RefundFlag = RefundFlag || '';
            if (skipconfirmation != "Y") {
                Lobibox.confirm({
                    msg: Cancelflag == "N" ? "Are you sure, want to ignore the Cancellation request ?" : "Are you sure, want to Cancel?",
                    buttons: {
                        yes: { 'class': 'btn btn-success', text: 'Yes', closeOnClick: true },
                        no: { 'class': 'btn btn-warning', text: 'No', closeOnClick: true }
                    },
                    callback: function (lobibox, type) {
                        if (type === 'no') {
                            return false;
                        } else if (type === 'yes') {
                            ConfirmCancellation(Cancelflag, remarksid, "Y")
                        }
                    }
                });
                return false;
            }


            if (Cancelflag != "N") {

                if (S_PNR == "" && airline_pnr == "" && (crs_pnr == "" || crs_pnr.toUpperCase() == "N/A")) {
                    showerralertnew("Please enter any pnr");
                    return false;
                }

                $(".clsBreakup").each(function () {
                    var markupid = $(this).data("markupid");
                    var chargesid = $(this).data("chargesid");
                    var penalityid = $(this).data("penalityid");

                    strCancellationdata += $(this).data('pax') + "SPLITPENALITY"        //0
                                        + $(this).data('breakup') + "SPLITPENALITY"     //1
                                        + $(this).data('fareid') + "SPLITPENALITY"      //2
                                        + $(this).data('segments') + "SPLITPENALITY"    //3
                                        + $(this).data("val") + "SPLITPENALITY"               //4
                                        + $("#" + markupid).data("val") + "SPLITPENALITY"     //5
                                        + $("#" + penalityid).data("val") + "SPLITPENALITY"   //6
                                        + $("#" + chargesid).data("val") + "SPLITPENALITY"    //7
                                        + $("#" + markupid).data('type') + "SPLITPAX"   //8

                    //+ $("#" + markupid).val() + "SPLITPENALITY"     //5
                    //+ $("#" + penalityid).val() + "SPLITPENALITY"   //6
                    //+ $("#" + chargesid).val() + "SPLITPENALITY"    //7
                    //+ $("#" + markupid).data('type') + "SPLITPAX"   //8
                });
            }
            var param = {
                strSPNR: S_PNR,
                strAirPNR: airline_pnr,
                strCRSPNR: crs_pnr,
                strRemarks: Remarks,
                strCancellationdata: strCancellationdata,
                strCancelflag: Cancelflag,
                strRequestData: RequestData,
                strCancelSeq: strCancelSeq,
                strWaivertext: Waivertext,
                strRefundFlag: RefundFlag
            }

            $.blockUI({
                message: '<img alt="Please Wait..." src=' + LoaderURL + ' style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
                css: { padding: '5px' }
            });

            //$.blockUI({
            //    message: '<img alt="Please Wait..." src=' + LoaderURL + ' class="clsLoaderimage" id="FareRuleLodingImage" />',
            //    css: { padding: '5px' }
            //});
            $('#modal_Cancel_Remarks').modal('hide');
            // $('#modal_Cancel_Remarks').iziModal('close');
            $.ajax({
                type: "POST",
                url: CancellationURL,
                data: JSON.stringify(param),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (json) {
                    $.unblockUI();
                    var result = json.Status;
                    if (result == "1" || result == "2") {
                        result == "2" ? showerralertnew(json.Message, '', '', "info") : showerralertnew(json.Message);
                        ClearCancellationDet();
                        ClearCancellationPNR();
                        if (strFunction == "TOCANCEL") {
                            gethistory();
                        }
                        return false;
                    }
                    else if (result == "3") {
                        Lobibox.confirm({
                            msg: json.Message,
                            buttons: {
                                yes: { 'class': 'btn btn-success', text: 'Yes', closeOnClick: true },
                                no: { 'class': 'btn btn-warning', text: 'No', closeOnClick: true }
                            },
                            callback: function (lobibox, type) {
                                if (type === 'no') {
                                    ClearCancellationPNR();
                                    return false;
                                } else if (type === 'yes') {
                                    ConfirmCancellation('O', 'txt_cancellation_remarks', "Y");
                                }
                            }
                        });

                        return false;
                    }
                    else if (result == "4") {
                        Lobibox.confirm({
                            msg: json.Message,
                            buttons: {
                                yes: { 'class': 'btn btn-success', text: 'Yes', closeOnClick: true },
                                no: { 'class': 'btn btn-warning', text: 'No', closeOnClick: true }
                            },
                            callback: function (lobibox, type) {
                                if (type === 'no') {
                                    ClearCancellationPNR();
                                    return false;
                                } else if (type === 'yes') {
                                    ConfirmCancellation('Y', 'txt_cancellation_remarks', "Y", "Y");
                                }
                            }
                        });
                        return false;
                    }
                    else {
                        showerralertnew(json.Message);
                        ClearCancellationDet();
                        ClearCancellationPNR();
                        if (strFunction == "TOCANCEL") {
                            gethistory();
                        }
                        return false;
                    }
                },
                error: function (e) {
                    if (e.status == "500") {
                        showerralertnew("An Internal Problem Occurred. Your Session will Expire.");
                        window.top.location = sessionExpURL;
                        return false;
                    }
                    else {
                        showerralertnew("unable to process your request.please contact support team.(#09)");
                        return false;
                    }
                    $.unblockUI();
                }

            });
        }
        catch (e) {
            $.unblockUI();
            showerralertnew("Problem occured while processing cancellation.please contact support team(#09).");
            console.log("==================================");
            console.log("Unable to bind penality popup data");
            console.log("Message: " + e.message);
            console.log("==================================");
            console.log(e);
            console.log("==================================");
            console.log(e.toString());
            console.log("==================================");
        }
    }

    function ShowRemarksPopup(strSPNR, strCancelSeq) {
        $("#hdnSPNR").val(strSPNR);
        $("#hdnCancelSeq").val(strCancelSeq);
        //$('#modal_Cancel_Remarks').iziModal('destroy');
        //$("#modal_Cancel_Remarks").iziModal({
        //    title: "Reject Cancellation",
        //    icon: 'fa fa-info',
        //    headerColor: '#CCCCCC',
        //    width: "400px"
        //});
        //$(".mdtlecnclremarkstitle").html("Reject Cancellation");
        //  $('#modal_Cancel_Remarks').iziModal('open');
        $("#modal_Cancel_Remarks").modal({
            backdrop: 'static',
            keyboard: false
        });

    }

    function ClearCancellationDet() {
        $("#dvPenality").hide();
        $("#dvRequest").show();
        $("#dvPenalityTravel").hide();
        $("#dvPenalityAgent").hide();
        $("#dvPenalityAirline").hide();
        $("#dvPenalityTravel").html("");
        $("#dvPenalityAgent").html("");
        $("#dvPenalityAirline").html("");

    }

    function CreateCancellationDet(strResult) {
        try {
            console.log(strResult);
            var strData = JSON.parse(strResult);
            var pax_json = strData[1];
            var paxary = $.parseJSON(pax_json);
            var tckt_json = strData[2];
            var tcktary = $.parseJSON(tckt_json);
            sessionStorage.setItem("pax_det", paxary);

            var showonlinebtn = true;
            if (strData.length > 3 && strData[3] == "OFFLINE") {
                showonlinebtn = false;
            }


            var datafields = new Array();
            var columns = new Array();
            sessionStorage.setItem("lenthpax_Arr_cancel", paxary.length);
            sessionStorage.setItem("lenthtckt_Arr_cancel", tcktary.length);

            if (paxary.length > 0) {
                $("#cancel_details").html('');
                var s = '';
                s += '<div class="col-xs-12 col-md-12 col-sm-12 col-lg-12 bordercss" style="overflow-x: auto;" >';
                s += '<table id="pax_cancel_details"  class="table table-hover table-striped" data-click-to-select="true" data-show-toggle="true" data-show-columns="true" data-show-filter="true" data-search="true" data-show-refresh="false"  data-show-export="true"  data-pagination="true"  style="text-align:left" >';
                s += '<thead>';
                for (var i = 0; i < paxary.length; i++) {
                    if (i == 0) { //Header formation...
                        s += '<tr>';
                        s += '<th style="text-transform: uppercase;"><input type="checkbox" class="chckcnclpaxall" id="checkallpaxdetailcncl" onclick="checkallpaxcancel()"/><span class="whiter"> ALL</span></th>';
                        for (var key in paxary[0]) {
                            if (key == "PAX_REF_NO") {
                                s += '<th style="display:none;"  data-field="direct' + key + '" data-align="left" data-sortable="false">' + key + '</th>';
                            }
                            else if (key == "SPNR") {
                                var text = strPNRLabel;
                                s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + text + '</th>';
                            }
                            else if (key == "PASSENGER_TYPE") {
                                var text = "Passenger&nbsp;Type";
                                s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + text + '</th>';
                            }
                            else if (key == "PASSENGER_NAME") {
                                var text = "Passenger&nbsp;Name";
                                s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + text + '</th>';
                            }
                            else if (key == "TICKET_NO") {
                                var text = "Ticket&nbsp;No";
                                s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + text + '</th>';
                            }
                            else if (key == "GROSSFARE") {
                                var text = "Gross";
                                s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + text + '</th>';
                            }
                            else if (key == "STATUS") {
                                var text = "Status";
                                s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + text + '</th>';
                            }
                        }
                        s += '</tr>';
                        s += '</thead>';
                        s += '<tbody>';
                    }
                    s += '<tr>';
                    var flg = false;
                    for (var key in paxary[i]) {
                        if (flg == false) {
                            //s += '<td><input style="display: none;border: 2px solid #3c763d !important;" type="checkbox" id="Chk_fare_' + i + '"  class="cb" name="Chk_fare_' + i + '" /><label style="border: 2px solid #00afe1 !important;" class="cbx" for="Chk_fare_' + i + '"></label><label class="lbl" for="Chk_fare_' + i + '"></label></td>';
                            s += '<td style="" id="data_' + i + '"><input id="cancel_pax_check_' + i + '" type="checkbox" class="chckcancelpax" onclick="chckcancelpax()" name="cancel_pax_check_' + i + '" /></td>';
                            flg = true;
                        }
                        if (key == "PASSENGER_TYPE") {
                            s += '<td id="cncl_pax_type' + i + '">' + paxary[i][key] + '</a></td>';
                        }
                        else if (key == "PAX_REF_NO") {
                            s += '<td style="display:none" id="cncl_pax_ref' + i + '">' + paxary[i][key] + '</a></td>';
                        }
                        else if (key == "SPNR") {
                            s += '<td id="cncl_spnr' + i + '">' + paxary[i][key] + '</a></td>';
                        }
                        else if (key == "PASSENGER_NAME") {
                            s += '<td id="cncl_pax_name' + i + '">' + paxary[i][key] + '</a></td>';
                        }
                        else if (key == "TICKET_NO") {
                            s += '<td id="cncl_ticket_no' + i + '">' + paxary[i][key] + '</a></td>';
                        }
                        else if (key == "GROSSFARE") {
                            s += '<td id="cncl_gross' + i + '">' + paxary[i][key] + '</a></td>';
                        }
                        else if (key == "STATUS") {
                            s += '<td id="cncl_status' + i + '">' + paxary[i][key] + '</a></td>';
                        }

                    }
                    s += '</tr>';
                }
                s += '</tbody>';
                s += '</table></div>';

                if (tcktary.length > 0) {
                    s += '<div class="col-xs-12 col-md-12 col-sm-12 col-lg-12 bordercss" style="overflow-x: auto;" >';
                    s += '<table id="tckt_cancel_details"  class="table table-hover table-striped" data-click-to-select="true" data-show-toggle="true" data-show-columns="true" data-show-filter="true" data-search="true" data-show-refresh="false"  data-show-export="true"  data-pagination="true"  style="text-align:left" >';
                    s += '<thead>';
                    for (var i = 0; i < tcktary.length; i++) {
                        if (i == 0) { //Header formation...
                            s += '<tr>';
                            //s += '<th data-field="direct' + i + '" data-align="left" data-sortable="false">Select</th>';
                            s += '<th style="text-transform: uppercase;"><input type="checkbox" class="chckcncltcktall" id="checkalltcktdetailcncl" onclick="checkalltcktcancel()"/><span class="whiter">ALL</span></th>';
                            for (var key in tcktary[0]) {
                                if (key == "FLIGHT_NO") {
                                    var text = "Flight&nbsp;No";
                                    s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + text + '</th>';
                                }
                                else if (key == "ORIGIN") {
                                    var text = "Origin";
                                    s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + text + '</th>';
                                }
                                else if (key == "DESTINATION") {
                                    var text = "Destination";
                                    s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + text + '</th>';
                                }
                                else if (key == "DEPT_DATE") {
                                    var text = "Dept. Date";
                                    s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + text + '</th>';
                                }
                                else if (key == "ARR_DATE") {
                                    var text = "Arr. Date";
                                    s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + text + '</th>';
                                }
                                else if (key == "SEG_NO") {
                                    s += '<th style="display:none">' + text + '</th>';
                                }
                            }
                            s += '</tr>';
                            s += '</thead>';
                            s += '<tbody>';
                        }
                        s += '<tr>';
                        var flg = false;
                        for (var key in tcktary[i]) {
                            if (flg == false) {
                                if (tcktary[i]["STATUS"] == "Confirmed") {
                                    var ddept_date = moment(tcktary[i]["DEPART_DATE"]).format('DD/MM/YYYY HH:mm A'); //(ary[0]["DEPT_DATE"]);
                                    var dae = new Date();
                                    var mon = "0";
                                    if (Number(dae.getMonth()) < 9) {
                                        mon = "0" + (Number(dae.getMonth()) + 1);
                                    }
                                    else {
                                        mon = (Number(dae.getMonth()) + 1);
                                    }
                                    var d1 = dae.getFullYear() + mon + dae.getDate();
                                    var d2 = ddept_date.split(" ")[0];
                                    var d3 = d2.split("/")[2] + d2.split("/")[1] + d2.split("/")[0];
                                    if ((Number(d3) > Number(d1)) || strTerminalType == "T") {
                                        // s += '<td><input style="display: none;border: 2px solid #3c763d !important;" type="checkbox" id="Chk_fare_' + i + '"  class="cb" name="Chk_fare_' + i + '" /><label style="border: 2px solid #00afe1 !important;" class="cbx" for="Chk_fare_' + i + '"></label><label class="lbl" for="Chk_fare_' + i + '"></label></td>';
                                        s += '<td id="data_' + i + '"><input id="cancel_tckt_check_' + i + '" class="chckcanceltckt" onclick="chckcanceltckt()" type="checkbox" name="cancel_tckt_check_' + i + '" /></td>';
                                    }
                                    else {
                                        //s += '<td><input style="display: none;" type="checkbox" id="Chk_fare_' + i + '"  class="cb" name="Chk_fare_' + i + '" /><label style="border: 2px solid #00afe1 !important;display:none;" class="cbx" for="Chk_fare_' + i + '"></label><label class="lbl" for="Chk_fare_' + i + '"></label></td>';
                                        s += '<td  id="data_' + i + '"><input id="cancel_tckt_check_' + i + '" style="display:none;" type="checkbox" name="cancel_tckt_check_' + i + '" /></td>';
                                    }
                                }
                                else {
                                    s += '<td id="data_' + i + '"><input id="cancel_tckt_check_' + i + '" style="display:none;" type="checkbox" name="cancel_tckt_check_' + i + '" /></td>';
                                }

                                flg = true;
                            }

                            if (key == "FLIGHT_NO") {
                                s += '<td id="cncl_flight_no' + i + '">' + tcktary[i][key] + '</a></td>';
                            }
                            else if (key == "ORIGIN") {
                                s += '<td id="cncl_origin' + i + '">' + tcktary[i][key] + '</a></td>';
                            }
                            else if (key == "DESTINATION") {
                                s += '<td id="cncl_destination' + i + '">' + tcktary[i][key] + '</a></td>';
                            }
                            else if (key == "DEPT_DATE") {
                                s += '<td id="cncl_dept_date' + i + '">' + tcktary[i][key] + '</a></td>';
                            }
                            else if (key == "ARR_DATE") {
                                s += '<td id="cncl_arr_date' + i + '">' + tcktary[i][key] + '</a></td>';
                            }
                            else if (key == "SEG_NO") {
                                s += '<td style="display:none;" id="cncl_seg_ref' + i + '">' + tcktary[i][key] + '</a></td>';
                            }
                        }
                        s += '</tr>';
                    }
                    s += '</tbody>';
                    s += '</table></div>';
                }

                if (strTerminalType == "W") {
                    s += '<div class="col-xs-12 col-md-12 col-sm-12 col-lg-12 bordercss" style="margin-top: 10px;">';
                    s += '<div class="alertinfomodify alert alert-info mt-10" role="alert" style="text-align: left;font-size:12px;">';
                    s += '<p class="mb-0"><strong>Cancel online : </strong>Display the Penalties and cancellation charges for the booking, Based on the confirmation booking will cancel and refund instantly. </p>';
                    s += '<p class="mb-0"><strong> Request for Cancellation : </strong>We have taken your request and process the cancellation / refund by our support team.</p>';
                    s += '</div>';
                    s += '</div>';
                }
              

                s += '<div class="col-xs-12 col-sm-3 col-sm-offset-3 martpplus10"><label style="">Remarks<span style="color: red">*</span></label><textarea id="txt_cancel_request_remarks" style="border: 1px solid #27a5f5; width: 100%;height:60px;" maxlength="200" class="txt-anim cls_cal_remarks" type="text" autocomplete="off"></textarea></div>';
                if (showonlinebtn)
                    s += '<div style="margin-top:35px;display:none;" class="dvclsonlinecancelbtn col-xs-12 col-sm-2 martpplus10"><input type="button" id="" class="action-button color c-button b-50 bg-aqua hv-transparent cmn-bg-clr btn-round badge-primary rounded-20 cursor-point btn w-100 lh-9 mg-b-10-f" style="height: 35px; padding: 1px;" value="Cancel Online" onclick="RequestCancellation()"></div>';

                s += '<div style="margin-top:35px;" class="col-xs-12 col-sm-2 martpplus10"><input type="button" id="" class="action-button colorB1 c-button b-50 bg-aqua hv-transparent cmn-bg-clr btn-round badge-primary rounded-20 cursor-point btn w-100 lh-9 mg-b-10-f" style="height: 35px; padding: 1px;" value="Request for Cancellation" onclick=' + '"' + "javascript:return RequestCancellation('Y')" + '"' + '></div>';
                $('#can_viewpnrShowDiv').html('');
                $('#can_viewpnrShowDiv').html(s);
                $('#can_viewpnrShowDiv').show();
            }
        }
        catch (e) {
            console.log(e);
            showerralertnew("unable to process your request(#09)", '', '');
        }
    }

    function ClearCancellationPNR() {
        $("#can_txt_viewSPNR").val("");
        $("#can_txt_viewAirPNR").val("");
        $("#can_txt_viewCSRPNR").val("");
        $("#can_txt_viewSPNR").removeClass("has-content");
        $("#can_txt_viewAirPNR").removeClass("has-content");
        $("#can_txt_viewCSRPNR").removeClass("has-content");
        $("#can_viewpnrShowDiv").css("display", "none");
        $("#dvPenality").css("display", "none");
        $("#lblcredit").html("0");
        $("#lbldebit").html("0");
        $("#txt_cancellation_remarks").val("");
    }

    function GetCancellationPNR() {
        $("#can_viewpnrShowDiv").length ? $("#can_viewpnrShowDiv").html("") : "";
        try {
            if ($("#can_txt_viewSPNR").val() == "" && $("#can_txt_viewAirPNR").val() == "" && $("#can_txt_viewCSRPNR").val() == "") {
                showerralertnew("Please enter any one PNR.", '', '');
                return false;
            }
            var Param = {
                strSPNR: $("#can_txt_viewSPNR").val(),
                strArilinePNR: $("#can_txt_viewAirPNR").val(),
                strCRSPNR: $("#can_txt_viewCSRPNR").val()
            };
            $.blockUI({
                message: '<img alt="Please Wait..." src="' + LoaderURL + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
                css: { padding: '5px' }
            });
            $.ajax({
                type: "POST", 		//GET or POST or PUT or DELETE verb
                url: cancelPNRDetails,
                data: JSON.stringify(Param),
                async: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (json) {//On Successful service call
                    $.unblockUI();
                    if (json.Status == "1") {
                        CreateCancellationDet(json.Result);
                        var data = JSON.parse(json.Result);
                        var PNRDATA = data[0].split('|');
                        $("#can_txt_viewSPNR").val(PNRDATA[0])
                        $("#can_txt_viewAirPNR").val(PNRDATA[1])
                        $("#can_txt_viewCSRPNR").val(PNRDATA[2])
                        $("#can_txt_viewSPNR").addClass("has-content");
                        $("#can_txt_viewAirPNR").addClass("has-content");
                        $("#can_txt_viewCSRPNR").addClass("has-content");
                        $("#hdnSPNR").val(PNRDATA[0]);
                        $("#hdnAirPNR").val(PNRDATA[1]);
                        $("#hdnCRSPNR").val(PNRDATA[2]);
               
                    }
                    else {
                        showerralertnew(json.Message, "", "");
                        return false;
                    }
                },
                error: function (e) {//On Successful service call
                    if (e.status == "500") {
                        showerralertnew("An Internal Problem Occurred. Your Session will Expire.", "", "S");
                        window.top.location = sessionExb;
                        return false;
                    }
                    $.unblockUI();
                }	// When Service call fails
            });
        }
        catch (e) {
            console.log(e);
            showerralertnew("unable to process your request(#09)", '', '');
            return false;
        }
    }

    function RequestCancellation(offlinerequest) {
        try {
            $(".dvclswaivertext").html("").hide();
            $("#dvPenality").hide();
            offlinerequest = offlinerequest || "";
            var TotalPax = "";
            var Totaltcktdet = "";
            var CancelationStatus_Pax = "";
            var CancelationStatus_Tckt = "";
            var checkedAdult = 0;
            var checkedChild = 0;
            var checkedInfant = 0;
            var Adultcount = 0;
            var Childcount = 0;
            var Infantcount = 0;
            var checkpax = 0;
            var checktckt = 0;
            var paxtable = sessionStorage.getItem("lenthpax_Arr_cancel");
            var tckttable = sessionStorage.getItem("lenthtckt_Arr_cancel");

            for (var i = 0; i < parseInt(paxtable) ; i++) {
                if (($("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "ADULT" || $("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "A" || $("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "ADT") && ($("#cncl_status" + i)[0].innerHTML.toUpperCase() == "CONFIRMED")) {
                    Adultcount++;
                }
                if (($("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "CHILD" || $("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "C" || $("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "CHD") && ($("#cncl_status" + i)[0].innerHTML.toUpperCase() == "CONFIRMED")) {
                    Childcount++;
                }
                if (($("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "INFANT" || $("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "I" || $("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "INF") && ($("#cncl_status" + i)[0].innerHTML.toUpperCase() == "CONFIRMED")) {
                    Infantcount++;
                }
                var select = $("#cancel_pax_check_" + i)[0];
                if (select.checked == true) {
                    checkpax++;
                    if (($("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "ADULT" || $("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "A" || $("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "ADT") && ($("#cncl_status" + i)[0].innerHTML.toUpperCase() == "CONFIRMED")) {
                        checkedAdult++;
                    }
                    if (($("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "CHILD" || $("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "C" || $("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "CHD") && ($("#cncl_status" + i)[0].innerHTML.toUpperCase() == "CONFIRMED")) {
                        checkedChild++;
                    }
                    if (($("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "INFANT" || $("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "I" || $("#cncl_pax_type" + i)[0].innerHTML.toUpperCase() == "INF") && ($("#cncl_status" + i)[0].innerHTML.toUpperCase() == "CONFIRMED")) {
                        checkedInfant++;
                    }
                    var S_PNR = $("#cncl_spnr" + i)[0].innerHTML;
                    var status = $("#cncl_status" + i)[0].innerHTML;
                    var pax_ref_no = $("#cncl_pax_ref" + i)[0].innerHTML;
                    var pass_name = $("#cncl_pax_name" + i)[0].innerHTML;
                    TotalPax += pass_name + "," + S_PNR + "," + pax_ref_no + "|";
                }
                else {
                    CancelationStatus_Pax = "P";
                }
            }

            for (var i = 0; i < parseInt(tckttable) ; i++) {
                var select = $("#cancel_tckt_check_" + i)[0];
                if (select.checked == true) {
                    checktckt++;
                    var seg_no = $("#cncl_seg_ref" + i)[0].innerHTML;
                    var dep_date = $("#cncl_dept_date" + i)[0].innerHTML;
                    Totaltcktdet += seg_no + "," + dep_date + "|";
                }
                else {
                    CancelationStatus_Tckt = "P";
                }
            }

            if (checkpax == 0) {
                showerralertnew("Please select any one passanger", "", "");
                return false;
            }
            if (checktckt == 0) {
                showerralertnew("Please select any one sector", "", "");
                return false;
            }

            var Adult = Adultcount - checkedAdult;
            var infant = Infantcount - checkedInfant;
            var child = Childcount - checkedChild

            if ((Infantcount > 0) && (Adult < infant)) {
                showerralertnew("Infant not to travel without Adult", "", "");
                return false;
            }
            if ((child > 0) && (Adult == 0)) {
                showerralertnew("Child not to travel without Adult", "", "");
                return false;
            }

            var remar = $("#txt_cancel_request_remarks").val();
            var re = remar.trim();

            if (re == null || re == "") {
                showerralertnew("Please Enter the Remarks", '', '');
                return false;
            }


            $("#myModal_cancellation_request").hide();

            $('#myModal').modal('hide');
            $.blockUI({
                message: '<img alt="Please Wait..." src="' + LoaderURL + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
                css: { padding: '5px' }
            });

            var param = {
                strSPNR: $("#can_txt_viewSPNR").val(),
                PaxDetails: TotalPax,
                Ticketdetails: Totaltcktdet,
                PaxcancelationStatus: CancelationStatus_Pax,
                Ticketcancelstatus: CancelationStatus_Tckt,
                Remarks: re,
                OfflineRequest: offlinerequest
            };


            $.ajax({
                type: "POST", 		//GET or POST or PUT or DELETE verb
                url: CancelRequestURL,
                data: JSON.stringify(param),
                async: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (json) {//On Successful service call

                    var result = json.Result;
                    console.log(json);
                    $.unblockUI();
                    if (json.Status == "1") { // online cancellation alert
                        var jsonresult = JSON.parse(result);
                        var ticketdata = JSON.parse(jsonresult[5]);
                        $("#hdnSPNR").val(ticketdata["Table1"][0]["SPNR"]);
                        $("#hdnAirPNR").val(ticketdata["Table1"][0]["AirlinePNR"]);
                        $("#hdnCRSPNR").val(ticketdata["Table1"][0]["CRSPNR"]);
                        BindPenalityAirlineData(JSON.parse(jsonresult[0]));
                        BindPenalityTravelData(JSON.parse(jsonresult[1]), JSON.parse(jsonresult[2]), JSON.parse(jsonresult[3]),
                            jsonresult[6], jsonresult[7], JSON.parse(jsonresult[8]));
                        RequestData = jsonresult[5]
                    }
                    else if (json.Status == "2") {
                        ClearCancellationPNR();
                        showerralertnew(json.Result, '', '', "info");
                        return false;
                    }

                    else { // offline cancellation alert
                        ClearCancellationPNR();
                        showerralertnew(json.Message, '', '');
                        return false;
                    }
                },
                error: function (e) {//On Successful service call
                    if (e.status == "500") {
                        showError("An Internal Problem Occurred. Your Session will Expire.", cancel_errPdiv, cancel_errdiv_msg, "");
                        window.top.location = sessionExb;
                        return false;
                    }

                    $.unblockUI();
                }	// When Service call fails

            });
        }
        catch (e) {
            console.log(e);
            showerralertnew("unable to process your request(#09)", '', '');
            return false;
        }
    }

    function checkalltcktcancel() {
        if ($("#checkalltcktdetailcncl")[0].checked == true) {
            $('.chckcanceltckt').prop('checked', true);
        }
        else {
            $('.chckcanceltckt').prop('checked', false);
        }
        Checkandshowonlinecancelbtn();
    }

    function checkallpaxcancel() {
        if ($("#checkallpaxdetailcncl")[0].checked == true) {
            $('.chckcancelpax').prop('checked', true);
        }
        else {
            $('.chckcancelpax').prop('checked', false);
        }
        Checkandshowonlinecancelbtn();
    }

    function chckcancelpax() {
        var length = sessionStorage.getItem("lenthpax_Arr_cancel");
        var check = 0;
        for (var i = 0; i < length; i++) {
            if ($("#cancel_pax_check_" + i)[0].checked == true) {
                check++;
            }
        }
        if (length == check) {
            $('#checkallpaxdetailcncl').prop('checked', true);
        }
        else {
            $('#checkallpaxdetailcncl').prop('checked', false);
        }

        Checkandshowonlinecancelbtn();
    }

    function chckcanceltckt() {
        var length = sessionStorage.getItem("lenthtckt_Arr_cancel");
        var check = 0;
        for (var i = 0; i < length; i++) {
            if ($("#cancel_tckt_check_" + i)[0].checked == true) {
                check++;
            }
        }
        if (length == check) {
            $('#checkalltcktdetailcncl').prop('checked', true);
        }
        else {
            $('#checkalltcktdetailcncl').prop('checked', false);
        }
        Checkandshowonlinecancelbtn();
    }

    function Checkandshowonlinecancelbtn() {
        if ($("#checkalltcktdetailcncl").length && $("#checkalltcktdetailcncl").prop("checked") &&
            $("#checkallpaxdetailcncl").length && $("#checkallpaxdetailcncl").prop("checked"))
            $(".dvclsonlinecancelbtn").length ? $(".dvclsonlinecancelbtn").show() : "";
        else
            $(".dvclsonlinecancelbtn").length ? $(".dvclsonlinecancelbtn").hide() : "";
    }

    function checkallssrdetails() {
        if ($("#chk_allssrdetails")[0].checked == true) {
            $('.clscmnchkssrdetails').prop('checked', true);
        }
        else {
            $('.clscmnchkssrdetails').prop('checked', false);
        }
    }

    function chckcancelallssrdetails() {

        if ($(".clscmnchkssrdetails").length == $(".clscmnchkssrdetails:checked").length) {
            $('#chk_allssrdetails').prop('checked', true);
        }
        else {
            $('#chk_allssrdetails').prop('checked', false);
        }
    }

    function CalculateSSRAmount() {
       
        var ssramount = 0;
        var totcreditamount = $("#lblcredit").data("creditamount") || 0;
        totcreditamount = Number(totcreditamount);

        $(".clscmnchkssrdetails").each(function () {
            if ($(this).prop("checked"))
                ssramount += Number($(this).data("reverseamount"));
        });

        $("#lblretrunssr").html(ssramount);
        $("#lblcredit").data("returnssramount", ssramount);
        CalculateTotalCreditDebit();

        $("#modal_ssr_details").modal("hide");


    }