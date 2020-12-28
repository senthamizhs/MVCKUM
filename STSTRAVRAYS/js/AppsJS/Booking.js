var stypeid = [];
var shdnid = [];
function get_seatmapindex() {//index
    try {
        var seatfare = 0;
        if (book_seatx != "" && book_seatx != null) {
            for (var i = 0; i < book_seatx.length; i++) {
                if (book_seatx[i].split('~')[3] != "" && book_seatx[i].split('~')[3] != null && book_seatx[i].split('~')[3] != "Not Selected") {
                    seatfare += parseFloat(book_seatx[i].split('~')[3]);
                }
            }
        }
        var vlakey = $('#hdn_value').attr('data-val');
        var pax_count = $('#hdn_value').attr('data-count');
        var Totalpaxcount = $('#hdn_value').attr('data-paxcount');

        //var vlakey = $('#adult1').attr('data-val');
        //var pax_count = $('#adult1').attr('data-count');
        //var segmentindex = $('#seatrow_' + index).attr('data-val');
        //var Totalpaxcount = $('#adult1').attr('data-paxcount');

        var adultcount = Totalpaxcount.split('@')[0];
        var chdcount = Totalpaxcount.split('@')[1];
        var infcount = Totalpaxcount.split('@')[2];

        var Adultnamedetails = "";
        var Childnamedetails = "";
        var Infantnamedetails = "";

        var Paxname = "";

        var Primeseat = $("#hdnservicemeal").val();

        for (var ad = 0; ad < adultcount; ad++) {
            var Pax_name = "";
            var Paxnamedetails = "";
            if ($('#ad_Title_' + ad).val() != "" && $('#ad_Title_' + ad).val() != null && $('#ad_Title_' + ad).val() != 0) {
                Paxnamedetails = $('#ad_Title_' + ad).val();
            }
            else {
                showerralert("Please choose Adult" + (ad + 1) + " title");
                //$("#seatmap_checkbox")[0].checked = false;
                return false;
            }

            //if ($('#ad_firstName_' + ad).val() != "" && $('#ad_lastName_' + ad).val() != "" && $('#ad_firstName_' + ad).val() != null && $('#ad_lastName_' + ad).val() != null) {
            //    Pax_name = $('#ad_firstName_' + ad).val() + "~" + $('#ad_lastName_' + ad).val();
            //}
            //else
            if ($('#ad_firstName_' + ad).val() == "" || $('#ad_firstName_' + ad).val() == null) {
                showerralert("Please enter Adult" + (ad + 1) + " firstname");
                //$("#seatmap_checkbox")[0].checked = false;
                return false;
            }
            else if ($('#ad_lastName_' + ad).val() == "" || $('#ad_lastName_' + ad).val() == null) {
                showerralert("Please enter Adult" + (ad + 1) + " lastname");
                //$("#seatmap_checkbox")[0].checked = false;
                return false;
            }
            else if ($('#ad_gender_' + ad).val() == "" || $('#ad_gender_' + ad).val() == null) {
                showerralert("Please select Adult" + (ad + 1) + " gender");
                //$("#seatmap_checkbox")[0].checked = false;
                return false;
            }
            else if (($('#ad_DOB_' + ad).val() == "" || $('#ad_DOB_' + ad).val() == null) && $('#hdn_stocktype').val() == "NDC") {
                showerralert("Please enter Adult" + (ad + 1) + " DOB");
                //$("#seatmap_checkbox")[0].checked = false;
                return false;
            }
            //Adultnamedetails += Paxnamedetails + "@" + Pax_name + "#";
            Adultnamedetails += Paxnamedetails + "@" + $('#ad_firstName_' + ad).val() + "~" + $('#ad_lastName_' + ad).val() + "~" + $('#ad_gender_' + ad).val() + "~" + $('#ad_DOB_' + ad).val() + "#";
        }

        if (chdcount > 0) {
            for (var chd = 0; chd < chdcount; chd++) {
                var Pax_name = "";
                var Paxnamedetails = "";

                if ($('#ch_Title_' + chd).val() != "" && $('#ch_Title_' + chd).val() != null && $('#ch_Title_' + chd).val() != 0) {
                    Paxnamedetails = $('#ch_Title_' + chd).val();
                }
                else {
                    showerralert("Please enter Child" + (chd + 1) + " title");
                    //$("#seatmap_checkbox")[0].checked = false;
                    return false;
                }
                //if ($('#ch_firstName_' + chd).val() != "" && $('#ch_lastName_' + chd).val() != "" && $('#ch_firstName_' + chd).val() != null && $('#ch_lastName_' + chd).val() != null) {
                //    Pax_name = $('#ch_firstName_' + chd).val() + "~" + $('#ch_lastName_' + chd).val();
                //}
                //else
                if ($('#ch_firstName_' + chd).val() == "" || $('#ch_firstName_' + chd).val() == null) {
                    showerralert("Please enter Child" + (chd + 1) + " firstname");
                    //$("#seatmap_checkbox")[0].checked = false;
                    return false;
                }
                else if ($('#ch_lastName_' + chd).val() == "" || $('#ch_lastName_' + chd).val() == null) {
                    showerralert("Please enter Child" + (chd + 1) + " lastname");
                    //$("#seatmap_checkbox")[0].checked = false;
                    return false;
                }
                else if ($('#ch_gender_' + chd).val() == "" || $('#ch_gender_' + chd).val() == null) {
                    showerralert("Please select Child" + (chd + 1) + " gender");
                    //$("#seatmap_checkbox")[0].checked = false;
                    return false;
                }
                else if (($('#ch_DOB_' + chd).val() == "" || $('#ch_DOB_' + chd).val() == null) && $('#hdn_stocktype').val() == "NDC") {
                    showerralert("Please enter Child" + (chd + 1) + " DOB");
                    //$("#seatmap_checkbox")[0].checked = false;
                    return false;
                }
                //Childnamedetails += Paxnamedetails + "@" + Pax_name + "#";
                Childnamedetails += Paxnamedetails + "@" + $('#ch_firstName_' + chd).val() + "~" + $('#ch_lastName_' + chd).val() + "~" + $('#ch_gender_' + chd).val() + "~" + $('#ch_DOB_' + chd).val() + "#";
            }
        }
        var PrimeSeatFlag = "";
        if (primessr.length > 0) {
            var Totpaxcount = parseFloat(adultcount) + parseFloat(chdcount);
            var SegCount = $('#hdnSegCount').val();
            for (var segcnt = 0; segcnt < parseFloat(SegCount) ; segcnt++) {
                var paxcountcheck = 0, paxcheck = [];
                for (var paxre = 1; paxre <= Totpaxcount; paxre++) {
                    paxcheck = primessr.filter(function (obj) { return obj.Itin == segcnt.toString() && obj.PaxNo == paxre });
                    if (paxcheck.length > 0) {
                        paxcountcheck++;
                    }
                }
                if (paxcountcheck != 0 && paxcountcheck != Totpaxcount) {
                    paxcheck = primessr.filter(function (obj) { return obj.Itin == segcnt.toString() });
                    showerralert("Prime SSR Choosen For One Passenger It's  Manadatory for all other passenger also. (" + paxcheck[0].Origin + "-" + paxcheck[0].Destination + ")", "", "");
                    //$("#seatmap_checkbox")[0].checked = false;
                    return false;
                }
            }
            for (var p = 0; p < primessr.length; p++)
            {
                PrimeSeatFlag += primessr[p].Origin + "-" + primessr[p].Destination + "~";
            }
        }

        // $('#hdn_segmentrow').val(index);
        $.ajax({
            type: "POST", 		//GET or POST or PUT or DELETE verb
            url: Getseatmaps, 		// Location of the service                    
            data: '{valkey: "' + vlakey + '",Paxcount:"' + pax_count + '",Paxname:"' + Paxname + '",Totalpaxcount:"' + Totalpaxcount + '",Adultnamedetails:"' + Adultnamedetails + '",Childnamedetails:"' + Childnamedetails + '",ContactNumber:"' + $('#txtContactNo').val().trim() + '",MailID:"' + $('#txtEmailID').val().trim() + '",PrimeSeatFlag:"' + PrimeSeatFlag + '"}',// PrimeSeatFlag//PrimeseatIndex:"' + index + '", segmentindex:"' + segmentindex + '",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (json) {//On Successful service call
                var result = json.Result[1];
                if (result != "") {
                    document.getElementById("ifrm").src = seatmaps_returnUrl + "?" + result;
                    $('#ifrm').css("display", "block");
                    if (($("#hdn_AppHosting").val() == "BSA" || $("#hdn_AppHosting").val() == "B2B" || $("#hdn_AppHosting").val() == "BOA") && ($("#hdn_producttype").val() == "DEIRA" || $("#hdn_producttype").val() == "ROUNDTRIP")) {
                        $("#modal-getseatmaps").addClass("slideInRight").removeClass("slideOutRight").show();
                        $(".overlayssr").show();
                    }
                    else {
                        $('#modal-getseatmaps').removeClass("slideOutRight");
                        $('#modal_header').html("Seat Map");
                        $("#modal-getseatmaps").modal({
                            backdrop: 'static',
                            keyboard: false
                        });
                    }
                }
                $.unblockUI();
            },
            error: function (e) {// When Service call fails                           
                LogDetails(e.responseText, e.status, "Flight ReBook");
                //$("#seatmap_checkbox")[0].checked = false;
                if (e.status == "500") {
                    alert("An Internal Problem Occurred. Your Session will Expire.");
                    window.top.location = sessionExb;
                    return false;
                }
                $.unblockUI();
            }
        });
    }
    catch (e) {
        console.log(e.message);
    }
}

var select_adt_meal = "";
var select_chd_meal = "";
var select_meal = "";

var select_adt_BKG = "";
var select_chd_BKG = "";
var select_BKG = "";

var Paxrefindex = "";
var Paxdetailsoth = "";

function create_meal(SSRid, index) {
    Paxrefindex = index;
    Paxdetailsoth = SSRid;
    //sessionStorage.setItem("Paxrefid", index);

    var thisid = SSRid.id;
    if ($("#frqno").val() == "TRUE") {
        document.getElementById("adulttype").innerText = thisid.toUpperCase().split('_')[0] + " " + (Number(thisid.toUpperCase().split('_')[1]) + 1);
    }

    //$('.ddlclass').selectedIndex = 0;
    $('.ddlclass').prop('selectedIndex', 0);
    if (SSRid.title == "child") {
        ptype = thisid.replace("child_", "hdnchdMeals_");
        if ($("#Freq").length) {

            document.getElementById("Freq").style.display = "none";
            document.getElementById("Fre_flyr_no").style.display = "none";
        }
        $('#hdn_paxtype').val('CHD');
    }
    if (SSRid.title == "infant") {
        ptype = thisid.replace("infant_", "hdninfMeals_");
        if ($("#Freq").length) {
            document.getElementById("Freq").style.display = "none";
            document.getElementById("Fre_flyr_no").style.display = "none";
        }
        $('#hdn_paxtype').val('INF');
    }
    if (SSRid.title == "adult") {

        if (document.getElementById("frqno").value == "TRUE") {
            if ($("#Freq").length) {
                document.getElementById("Freq").style.display = "block";
                document.getElementById("Fre_flyr_no").style.display = "block";
            }
        }
        else {
            if ($("#Freq").length) {
                document.getElementById("Freq").style.display = "none";
                document.getElementById("Fre_flyr_no").style.display = "none";
            }
        }
        ptype = thisid.replace("adult_", "hdnadtMeals_");
        $('#hdn_paxtype').val('ADT');
    }

    $("#modal-SSR-container").modal({
        backdrop: 'static',
        keyboard: false
    });

    var paxtype = Paxdetailsoth.title;
    var paxtypeid = Paxdetailsoth.id;

    var paxcount = $('#' + paxtypeid).attr("data-paxcount");
    var adtcount = paxcount.split("@")[0];
    var chdcount = paxcount.split("@")[1];

    var Pax_refno = ((paxtype == "adult") ? index : (parseInt(index) + parseInt(adtcount)));

    //if (Other_SSRSPMax.length > 0) {
        if ($('.othSpicemaxcls').hasClass) {
            var otherprilen = $('.othSpicemaxcls').length;
            if (otherprilen != 0) {
                var j = 0;
                for (j = 0; j < otherprilen; j++) {
                    var paxref = parseInt(Pax_refno) + parseInt(1);
                    if ((Other_SSRSPMax).indexOf($('#othssridspicemax_' + j)[0].value + "WEbMeaLWEB" + parseInt(paxref)) > -1) {
                        $('#othssridspicemax_' + j)[0].checked = true;
                    }
                    else {
                        $('#othssridspicemax_' + j)[0].checked = false;
                    }
                }
            }
        }
    //}

    //if (Other_SSRPCIN.length > 0) {
        if ($('.otherpriorit').hasClass) {
            var otherprilen = $('.otherpriorit').length;
            if (otherprilen != 0) {
                var j = 0;
                for (j = 0; j < otherprilen; j++) {
                    var paxref = parseInt(Pax_refno) + parseInt(1);
                    if ((Other_SSRPCIN).indexOf($('#othssridpriority_' + j)[0].value + "WEbMeaLWEB" + parseInt(paxref)) > -1) {
                        $('#othssridpriority_' + j)[0].checked = true;
                    }
                    else {
                        $('#othssridpriority_' + j)[0].checked = false;
                    }
                }
            }
        }
    //}

    //if (Other_bagout.length > 0) {
        if ($('.bagoutfirclss').hasClass) {
            var otherprilen = $('.bagoutfirclss').length;
            if (otherprilen != 0) {
                var k = 0;
                for (k = 0; k < otherprilen; k++) {
                    var paxref = parseInt(Pax_refno) + parseInt(1);
                    if ((Other_bagout).indexOf($('#othssrid_' + k)[0].value + "WEbMeaLWEB" + parseInt(paxref)) > -1) {
                        $('#othssrid_' + k)[0].checked = true;
                    }
                    else {
                        $('#othssrid_' + k)[0].checked = false;
                    }
                }
            }
        }
    //}

    $('.clsprime').each(function () {
        var aryprime = $(this).val().split("WEbMeaLWEB"); var mealmode = "";
        if ($(this)[0].checked == true) {
            mealmode = "I";
        }
        else {
            mealmode = "D";
        }
        Servicemealclick("." + aryprime[4] + "CLS" + aryprime[5], mealmode, aryprime[4], aryprime[5]);
    });

    if ($('#' + ptype).val() != null && $('#' + ptype).val() != "") {
        var mm = $('#' + ptype).val().split('~')
        for (var j = 0; j < mm.length; j++) {
            if (mm[j] != "") {
                var Im = mm[j].split('SpLiTSSR');
                var Me = Im[0].toString();
                var BA = Im[1].toString();
                var FA = Im[2].toString();

                if (thisid.toUpperCase().split('_')[0].toUpperCase() == "ADULT") {
                    select_adt_meal = Me;
                    select_meal = select_adt_meal;
                    select_adt_BKG = BA;
                    select_BKG = select_adt_BKG;
                }
                else {
                    select_chd_meal = Me;
                    select_meal = select_chd_meal;
                    select_chd_BKG = BA;
                    select_BKG = select_chd_BKG;
                }
                //  $('#Meals' + j + ' option')
                //  .filter(function () { return $.trim($('#Meals' + j).val()) == select_meal; })//Me
                //  .attr('selected', true);
                $('#Meals' + j).val(select_meal);
                $('#Baggage' + j).val(select_BKG);

                if (document.getElementById("frqno").value == "TRUE")
                    $("#Freq_flyer" + j).val(FA.split('WEbMeaLWEB')[2]);
            }
        }
    }
    else {
        var rowcounti = 0;

        var par = $("#meals tbody tr");
        var par1 = $("#Baggagein tbody tr");
        var par2 = $("#frequentin tbody tr");

        if (par.length != 0 && par.length >= rowcounti)
            rowcounti = par.length;
        if (par1.length != 0 && par1.length >= rowcounti)
            rowcounti = par1.length;
        if (par2.length != 0 && par2.length >= rowcounti)
            rowcounti = par2.length;

        for (var i = 0; i < rowcounti; i++) {
            $('#Meals' + i + ' option')
             .filter(function () { return $.trim($(this).val()) == "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0"; })
             .attr('selected', true);
            $('#Baggage' + i + ' option')
            .filter(function () { return $.trim($(this).val()) == "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0"; })
            .attr('selected', true);

            if (document.getElementById("frqno").value == "TRUE")
                $("#Freq_flyer" + i).val('')

        }

        $('hdn_seatpax').val(SSRid.title);
    }

    $('#seatmaptable').css("display", "none");
    if (segmentarr.length > 0) {
        showseatdetails(SSRid, (Number(index) + 1));
    }
}

function GetSeatMapReturn(retval) {
    $("#modal-getseatmaps").modal("hide");
    $('.b-close').css("display", "none");
    //var row = $('#hdn_seatrow').val();
    $("#ifrm").attr("src", "");
    getseatmap(retval);
}

function getseatmap(retval) {// 
    $('#Seat_errdiv').css("display", "none");

    var Valkey = $('#HdValKey')[0].value;

    //alert(retval);//1
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: ResultSeatmap, 		// Location of the service
        data: JSON.stringify({ retval: retval }),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call
            var result = json.Result
            //alert("controller return success");
            if (result[0] != "" && result[0] != null) {
                $("#modal-getseatmaps").modal("hide");
                $('.b-close').css("display", "none");
                $("#ifrm").attr("src", "");
                //$("#seatmap_checkbox")[0].checked = false;
                showerralert(result[0]);
                return false;
            }
            else if (result[1] != "" && result[1] != null) {
                //alert(result[1]);
                book_seatx = [];
                segmentarr = [];
                var seatresult = result[1].split('$');
                for (var st = 0; st < seatresult.length - 1; st++) {
                    if (seatresult[st] != "" && seatresult[st] != null) {
                        book_seatx.push(seatresult[st]);
                    }
                }

                if (result[2] != "" && result[2] != null) {
                    //alert(result[2]);
                    var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;
                    var tot_seat_amount = result[2];
                    var total_amount = $("#Li_Totalfare").data("li_totalfare");// //changed by udhaya...2017-07-18 (temp) document.getElementById("Li_Totalfare").innerHTML;
                    var mealamount = document.getElementById("mealamount").innerHTML;
                    var Baggageamount = document.getElementById("BaggageAmnt").innerHTML;
                    var OtherSSRAmount = document.getElementById("othssramount").innerHTML;
                    var TotalGross = Number(parseFloat(total_amount) + parseFloat(tot_seat_amount) + parseFloat(mealamount) + parseFloat(Baggageamount) + parseFloat(OtherSSRAmount)).toFixed(decimalflag);
                    document.getElementById("seatamount").innerHTML = Number(tot_seat_amount).toFixed(decimalflag);
                    if ($("#hdn_producttype").val() != "RIYA") {
                        document.getElementById("Li_Totalfare").innerHTML = Number(TotalGross).toFixed(decimalflag);// parseFloat(total_amount);
                    }
                    else {
                        document.getElementById("Li_Totalfare").innerHTML = Number(total_amount).toFixed(decimalflag);
                    }

                    if (AvailFormat == "NAT" || $("#hdn_HideComission").val() == "Y") {/* STS-166*/
                        var totalfare = $("#totalAmnt").data("totalamntd");
                        var TotalfareGross = Number(parseFloat(totalfare) + parseFloat(tot_seat_amount) + parseFloat(mealamount) + parseFloat(Baggageamount) + parseFloat(OtherSSRAmount)).toFixed(decimalflag);
                        $("#totalAmnt").html(TotalfareGross);
                    }

                    document.getElementById("spnFare").innerText = Number(TotalGross).toFixed(decimalflag);
                    var commamt = $("#hdn_comm").val() == null || $("#hdn_comm").val() == "" ? 0 : $("#hdn_comm").val();
                    var Servicecharge = $("#hdn_servicecharge").val() == null || $("#hdn_servicecharge").val() == "" ? 0 : $("#hdn_servicecharge").val();
                    var Insamt = Number($(".riyains")[0].innerHTML).toFixed(0) != 0 ? $(".riyains")[0].innerHTML : "";
                    var Ins_comm = (Insamt != "") ? Insamt / 2 : 0;
                    if ($("#hdn_bookingprev").val() == "Y") {
                        $(".riyasea").each(function (i, vale) {
                            $(".riyasea")[i].innerHTML = Number(tot_seat_amount).toFixed(decimalflag);
                        })
                        $(".riyatotal").each(function (i, vale) {
                            $(".riyatotal")[i].innerHTML = Number(TotalGross).toFixed(decimalflag);
                        })
                    }

                    if ($("#hdn_producttype").val() == "RIYA") {
                        $(".riyasea").each(function (i, vale) {
                            $(".riyasea")[i].innerHTML = Number(tot_seat_amount).toFixed(decimalflag);
                        })
                        $(".riyatotal").each(function (i, vale) {
                            $(".riyatotal")[i].innerHTML = Number(TotalGross).toFixed(decimalflag);
                        })
                            
                            $("#netframt")["0"].innerHTML = Number(parseFloat(TotalGross) - parseFloat(commamt) - parseFloat(Ins_comm) - parseFloat(Servicecharge)).toFixed(decimalflag);
                        
                    }

                    var Cspayableamt = Number(((parseFloat(TotalGross)) < parseFloat($("#hdn_CreditshellAmt").val().slice('|')[0])) ? 0 : ((parseFloat(TotalGross)) > parseFloat($("#hdn_CreditshellAmt").val().slice('|')[0])) ? ((parseFloat(TotalGross)) - parseFloat($("#hdn_CreditshellAmt").val().slice('|')[0])) : 0).toFixed(decimalflag);
                    //var Cspayableamt = Number((parseFloat(TotalGross) > parseFloat($("#hdn_CreditshellAmt").val().slice('|')[0])) ? (parseFloat(TotalGross) - parseFloat($("#hdn_CreditshellAmt").val().slice('|')[0])) : 0).toFixed(decimalflag);
                    $(".cls-payableamt").each(function (i, vale) {
                        $(".cls-payableamt")[i].innerHTML = Number(Cspayableamt).toFixed(decimalflag);
                    });
                    $(".cls-RemainCreditshellamt").each(function (i, vale) {
                        $(".cls-RemainCreditshellamt")[i].innerHTML = Number((parseFloat(Cspayableamt) < parseFloat(parseFloat($("#hdn_CreditshellAmt").val().slice('|')[0]))) ? (parseFloat(parseFloat($("#hdn_CreditshellAmt").val().slice('|')[0])) - (parseFloat(TotalGross) - parseFloat(Servicecharge) - parseFloat(Insamt))) : 0).toFixed(decimalflag);
                    });

                }
                if (result[3] != "" && result[3] != null) {
                    //alert(result[3]);
                    var segmentresult = result[3].split('@');
                    for (var i = 0; i < segmentresult.length; i++) {
                        segmentarr.push(segmentresult[i]);
                    }   
                }
                //$("#seatmap_checkbox")[0].checked = true;
            }
            else if (result[4] != "" && result[4] != null) {
                //alert(result[4]);
                $('#Seat_errdiv').css("display", "none");
                //$("#seatmap_checkbox")[0].checked = false;
                if (book_seatx != null && book_seatx != "") {
                    for (var i = 0; i < book_seatx.length; i++) {
                        var row1 = book_seatx[i].split('~')[0];
                        var row2 = book_seatx[i].split('~')[1];
                        var row3 = book_seatx[i].split('~')[2];
                        var row4 = book_seatx[i].split('~')[3];
                        var row5 = book_seatx[i].split('~')[4];
                        var row6 = book_seatx[i].split('~')[5];
                        book_seatx[i] = row1 + "~" + row2 + "~" + "Not Selected" + "~" + "Not Selected" + "~" + row5 + "~" + row6;
                    }
                }
                var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;
                var tot_seat_amount = 0;

                var total_amount = $("#Li_Totalfare").data("li_totalfare");//  document.getElementById("Li_Totalfare").innerHTML;//STS-166
                var mealamount = document.getElementById("mealamount").innerHTML;
                var Baggageamount = document.getElementById("BaggageAmnt").innerHTML;
                var OtherSSRAmount = document.getElementById("othssramount").innerHTML;
                var TotalGross = Number(parseFloat(total_amount) + parseFloat(tot_seat_amount) + parseFloat(mealamount) + parseFloat(Baggageamount) + parseFloat(OtherSSRAmount)).toFixed(decimalflag);
                document.getElementById("seatamount").innerHTML = Number(tot_seat_amount).toFixed(decimalflag);
                if ($("#hdn_producttype").val() != "RIYA") {
                    document.getElementById("Li_Totalfare").innerHTML = Number(TotalGross).toFixed(decimalflag);// parseFloat(total_amount);
                }
                else {
                    document.getElementById("Li_Totalfare").innerHTML =Number(total_amount).toFixed(decimalflag);
                }

                if (AvailFormat == "NAT" || $("#hdn_HideComission").val() == "Y") {/* STS-166*/
                    var totalfare = $("#totalAmnt").data("totalamntd");
                    var TotalfareGross = Number(parseFloat(totalfare) + parseFloat(tot_seat_amount) + parseFloat(mealamount) + parseFloat(Baggageamount) + parseFloat(OtherSSRAmount)).toFixed(decimalflag);
                    $("#totalAmnt").html(TotalfareGross);
                }

                document.getElementById("spnFare").innerText = Number(TotalGross).toFixed(decimalflag);

                if ($("#hdn_bookingprev").val() == "Y") {
                    $(".riyasea").each(function (i, vale) {
                        $(".riyasea")[i].innerHTML = Number(tot_seat_amount).toFixed(decimalflag);
                    })
                    $(".riyatotal").each(function (i, vale) {
                        $(".riyatotal")[i].innerHTML = Number(TotalGross).toFixed(decimalflag);
                    })
                }

                if ($("#hdn_producttype").val() == "RIYA") {
                    $(".riyasea").each(function (i, vale) {
                        $(".riyasea")[i].innerHTML = Number(tot_seat_amount).toFixed(decimalflag);
                    })
                    $(".riyatotal").each(function (i, vale) {
                        $(".riyatotal")[i].innerHTML = Number(TotalGross).toFixed(decimalflag);
                    })

                    var commamt = $("#hdn_comm").val() == null || $("#hdn_comm").val() == "" ? 0 : $("#hdn_comm").val();
                    var Servicecharge = $("#hdn_servicecharge").val() == null || $("#hdn_servicecharge").val() == "" ? 0 : $("#hdn_servicecharge").val();
                    var Insamt = Number($(".riyains")[0].innerHTML).toFixed(0) != 0 ? $(".riyains")[0].innerHTML : "";
                    var Ins_comm = (Insamt != "") ? Insamt / 2 : 0;
                    $("#netframt")["0"].innerHTML = Number(parseFloat(TotalGross) - parseFloat(commamt) - parseFloat(Ins_comm) - parseFloat(Servicecharge)).toFixed(decimalflag);
                }

            }
            else {
                showerralert("Unable to get Seat Map");
                //$("#seatmap_checkbox")[0].checked = false;
                //alert("Unable to get Seat Map");
                return false;
            }

        },
        error: function (e) {//On Successful service call                            
            LogDetails(e.responseText, e.status, "SeatmapResponse");
            //$("#seatmap_checkbox")[0].checked = true;
            $.unblockUI();
            if (e.status == "500") {
                window.location = sessionExb;
                return false;
            }
        }
    });
    if (($("#hdn_AppHosting").val() == "BSA" || $("#hdn_AppHosting").val() == "B2B" || $("#hdn_AppHosting").val() == "BOA") && ($("#hdn_producttype").val() == "DEIRA" || $("#hdn_producttype").val() == "ROUNDTRIP")) {
        Closemap();
    }
}

function getseatmapErrvalue(retval) {
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: ResultSeatmap, 		// Location of the service
        data: JSON.stringify({ retval: retval }),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call
            var result = json.d

            var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;
            if (result[1] != "" && result[1] != null) {
                book_seatx = [];
                var seatresult = result[1].split('$');
                for (var st = 0; st < seatresult.length - 1; st++) {
                    if (seatresult[st] != "" && seatresult[st] != null) {
                        book_seatx.push(seatresult[st]);
                    }
                }
                if (result[2] != "" && result[2] != null) {
                    var tot_seat_amount = result[2];
                    var TotalGross = Number(parseFloat(total_amount) + parseFloat(tot_seat_amount)).toFixed(decimalflag);
                    document.getElementById("seatamount").innerHTML = Number(tot_seat_amount).toFixed(decimalflag);
                    if ($("#hdn_producttype").val() != "RIYA") {
                        document.getElementById("Li_Totalfare").innerHTML = Number(TotalGross).toFixed(decimalflag); //parseFloat(total_amount);
                    }
                    else {
                        document.getElementById("Li_Totalfare").innerHTML = Number(total_amount).toFixed(decimalflag);
                    }
                    // document.getElementById("totalAmnt").innerHTML = Number(TotalGross).toFixed(decimalflag); /* Commended by Rajesh for showing only ticket amount except SSR*/
                    $("span[id='spnFare']").text(Number(TotalGross).toFixed(decimalflag));
                }
                if (result[3] != "" && result[3] != null) {
                    var segmentresult = result[3].split('@');
                    for (var i = 0; i < segmentresult.length; i++) {
                        segmentarr.push(segmentresult[i]);
                    }
                }
            }
            else if (result[4] != "" && result[4] != null) {
                $('#Seat_errdiv').css("display", "none");
            }
            else if (result[0] != "" && result[0] != null) {
                //$("#seatmap_checkbox")[0].checked = false;
                showerralert(result[0]);
                return false;
            }
            else {
                //$("#seatmap_checkbox")[0].checked = false;
                showerralert("Unable to get Seat Map");
                return false;
            }
        },
        error: function (e) {//On Successful service call                            
            LogDetails(e.responseText, e.status, "SeatmapResponse");
            $.unblockUI();
            if (e.status == "500") {
                alert("Session has been Expired.");
                window.top.location = sessionExb;
                return false;
            }
        }
    });
}

var ptype = "";
var meal_baggage = "";
var meals = 0;
var baggage = 0;
var mealamount = 0;
var baggageamount = 0;
var GrossTotM = 0;
var grossbagg = 0;
var book_seatx = []; var segmentarr = [];
var Other_SSRSPMax = []; var Other_bagout = []; var Other_SSRPCIN = [];
var primessr = [];
function submitssrrowindex(va) {

    var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;
    var rowcounti = 0;

    var par = $("#meals tbody tr");
    var par1 = $("#Baggagein tbody tr");
    var par2 = $("#frequentin tbody tr");
    if (par.length != 0 && par.length >= rowcounti)
        rowcounti = par.length;
    if (par1.length != 0 && par1.length >= rowcounti)
        rowcounti = par1.length;
    if (par2.length != 0 && par2.length >= rowcounti)
        rowcounti = par2.length;
    var tot = "";
    var totMeal = 0;
    var totBagg = 0;
    var frequentno = "";

    var adultcnt = $("#passengersdetails .commAdtpaxcls").length;
    var childcnt = $("#passengersdetails .commChdpaxcls").length;
    //var spicejetcont = sessionStorage.getItem("Paxrefid");


    //Other_SSRSPMax = [];
    //Other_bagout = [];
    //Other_SSRPCIN = [];

    try {

        if ($('#' + ptype).val() != null && $('#' + ptype).val() != "") {

            var mm = $('#' + ptype).val().split('~')
            for (var j = 0; j < mm.length; j++) {
                if (mm[j] != "") {//|| mm[j] != null) {
                    var Im = mm[j].split('SpLiTSSR');
                    var Me = Im[0].split('WEbMeaLWEB')[1];
                    var BA = Im[1].split('WEbMeaLWEB')[1];
                    totMeal = Number(parseFloat(totMeal) - parseFloat(Me)).toFixed(decimalflag);
                    totBagg = Number(parseFloat(totBagg) - parseFloat(BA)).toFixed(decimalflag);
                }
            }
            for (var i = 0; i < rowcounti; i++) {
                if ($("#Meals" + i).length > 0 && $("#Baggage" + i).length > 0) {
                    meals = $("#Meals" + i).val().split('WEbMeaLWEB')[0];
                    mealamount = $("#Meals" + i).val().split('WEbMeaLWEB')[1];
                    baggage = $("#Baggage" + i).val().split('WEbMeaLWEB')[0];
                    baggageamount = $("#Baggage" + i).val().split('WEbMeaLWEB')[1];
                    //tot += $("#Meals" + i).val() + "/" + $("#Baggage" + i).val() + "/" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                    tot += $("#Meals" + i).val() + "SpLiTSSR" + $("#Baggage" + i).val() + "SpLiTSSR" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                }
                else {
                    if ($("#Meals" + i).length > 0 || $("#Baggage" + i).length > 0) {
                        if ($("#Meals" + i).length > 0) {
                            meals = $("#Meals" + i).val().split('WEbMeaLWEB')[0];
                            mealamount = $("#Meals" + i).val().split('WEbMeaLWEB')[1];
                            //tot += $("#Meals" + i).val() + "/" + "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "/" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                            tot += $("#Meals" + i).val() + "SpLiTSSR" + "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "SpLiTSSR" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                        } else {
                            meals = 0;
                            mealamount = 0;
                        }
                        if ($("#Baggage" + i).length > 0) {
                            baggage = $("#Baggage" + i).val().split('WEbMeaLWEB')[0];
                            baggageamount = $("#Baggage" + i).val().split('WEbMeaLWEB')[1];
                            //tot += "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "/" + $("#Baggage" + i).val() + "/" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                            tot += "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "SpLiTSSR" + $("#Baggage" + i).val() + "SpLiTSSR" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                        }
                        else {
                            baggage = 0;
                            baggageamount = 0;
                        }
                    }
                    else {
                        if ($("#Freq_flyer" + i).val() != "") {
                            //tot += "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "/" + "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "/" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                            tot += "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "SpLiTSSR" + "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "SpLiTSSR" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                            baggage = 0;
                            baggageamount = 0;
                        }
                    }
                }
                totMeal = Number(parseFloat(totMeal) + parseFloat(mealamount)).toFixed(decimalflag);
                totBagg = Number(parseFloat(totBagg) + parseFloat(baggageamount)).toFixed(decimalflag);
            }
            $('#' + ptype).val(tot);
        }
        else {
            for (var i = 0; i < rowcounti; i++) {
                if ($("#Meals" + i).length > 0 && $("#Baggage" + i).length > 0) {
                    meals = $("#Meals" + i).val().split('WEbMeaLWEB')[0];
                    mealamount = $("#Meals" + i).val().split('WEbMeaLWEB')[1];
                    baggage = $("#Baggage" + i).val().split('WEbMeaLWEB')[0];
                    baggageamount = $("#Baggage" + i).val().split('WEbMeaLWEB')[1];
                    //tot += $("#Meals" + i).val() + "/" + $("#Baggage" + i).val() + "/" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                    tot += $("#Meals" + i).val() + "SpLiTSSR" + $("#Baggage" + i).val() + "SpLiTSSR" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                }
                else {
                    if ($("#Meals" + i).length > 0 || $("#Baggage" + i).length > 0) {
                        if ($("#Meals" + i).length > 0) {
                            meals = $("#Meals" + i).val().split('WEbMeaLWEB')[0];
                            mealamount = $("#Meals" + i).val().split('WEbMeaLWEB')[1];
                            //tot += $("#Meals" + i).val() + "/" + "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "/" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                            tot += $("#Meals" + i).val() + "SpLiTSSR" + "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "SpLiTSSR" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                        } else {
                            meals = 0;
                            mealamount = 0;
                        }
                        if ($("#Baggage" + i).length > 0) {
                            baggage = $("#Baggage" + i).val().split('WEbMeaLWEB')[0];
                            baggageamount = $("#Baggage" + i).val().split('WEbMeaLWEB')[1];
                            //tot += "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "/" + $("#Baggage" + i).val() + "/" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                            tot += "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "SpLiTSSR" + $("#Baggage" + i).val() + "SpLiTSSR" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                        }
                        else {
                            baggage = 0;
                            baggageamount = 0;
                        }
                    }
                    else {
                        if ($("#Freq_flyer").val() != "") {
                            //tot += "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "/" + "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "/" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                            tot += "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "SpLiTSSR" + "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" + "SpLiTSSR" + (document.getElementById("frqno").value == "TRUE" ? $("#_segno" + i).val() + "WEbMeaLWEB" + $("#Freq_flyer" + i).val() : "") + "~";
                            baggage = 0;
                            baggageamount = 0;
                        }
                    }
                }
                totMeal = Number(parseFloat(totMeal) + parseFloat(mealamount)).toFixed(decimalflag);
                totBagg = Number(parseFloat(totBagg) + parseFloat(baggageamount)).toFixed(decimalflag);
            }
            $('#' + ptype).val(tot);
        }

        var otherpriamount = 0;
        var bagoutfirst_amount = 0;
        var Spmaxamount = 0;
        var otherprivalue = "";

        var paxtype = Paxdetailsoth.title;
        var paxtypeid = Paxdetailsoth.id;

        var paxcount = $('#' + paxtypeid).attr("data-paxcount");
        paxcounttotal = paxcount;
        var adtcount = paxcount.split("@")[0];
        var chdcount = paxcount.split("@")[1];

        //var bagoutfirst_amount = "";
        var i = ((paxtype == "adult") ? parseInt(Paxrefindex) : (parseInt(adtcount) + parseInt(Paxrefindex)));

        //var i = parseInt(Paxrefindex);
        // for (i = 0; i <= Number(spicejetcont); i++) {
        if ($('.othSpicemaxcls').hasClass) {
            var otherprilen = $('.othSpicemaxcls').length;

            if (otherprilen != 0) {
                var j = 0;
                var id = $('.othSpicemaxcls')[0].id;
                for (j = 0; j < otherprilen; j++) {
                    var paxre = i + 1;
                    if ($('#othssridspicemax_' + j)[0].checked == true) {
                        var priorityvalue = $('#othssridspicemax_' + j)[0].value;
                        var priamt = priorityvalue.toString().split("WEbMeaLWEB");
                        Spmaxamount = Number(parseFloat(Spmaxamount) + (parseFloat(priamt[3]))).toFixed(decimalflag);
                        otherprivalue += $('#othssridspicemax_' + j)[0].value + "WEbMeaLWEB" + paxre + "~";
                        if (Other_SSRSPMax.indexOf($('#othssridspicemax_' + j)[0].value + "WEbMeaLWEB" + paxre) == -1) {
                            Other_SSRSPMax.push($('#othssridspicemax_' + j)[0].value + "WEbMeaLWEB" + paxre);
                        }
                    }
                    else {
                        var priorityvalue = Other_SSRSPMax.indexOf($('#othssridspicemax_' + j)[0].value + "WEbMeaLWEB" + paxre);//$('#othssridspicemax_' + j)[0].value;
                        //if (Other_SSRSPMax.indexOf($('#othssridspicemax_' + j)[0].value + "WEbMeaLWEB" + paxre) > -1) {
                        //    Other_SSRSPMax.pop($('#othssridspicemax_' + j)[0].value + "WEbMeaLWEB" + paxre);
                        //}
                        if (priorityvalue > -1) {
                            Other_SSRSPMax.splice(priorityvalue, 1);
                        }
                    }
                }
            }
        }

        if ($('.otherpriorit').hasClass) {
            var otherprilen = $('.otherpriorit').length;

            if (otherprilen != 0) {
                var j = 0;
                var id = $('.otherpriorit')[0].id;
                for (j = 0; j < otherprilen; j++) {
                    var paxre = i + 1;
                    if ($('#othssridpriority_' + j)[0].checked == true) {
                        var priorityvalue = $('#othssridpriority_' + j)[0].value;
                        var priamt = priorityvalue.toString().split("WEbMeaLWEB");
                        otherpriamount = Number(parseFloat(otherpriamount) + (parseFloat(priamt[3]))).toFixed(decimalflag);
                        otherprivalue += ('#othssridpriority_' + j)[0].value + "WEbMeaLWEB" + paxre + "~";
                        if (Other_SSRPCIN.indexOf($('#othssridpriority_' + j)[0].value + "WEbMeaLWEB" + paxre) == -1) {
                            Other_SSRPCIN.push($('#othssridpriority_' + j)[0].value + "WEbMeaLWEB" + paxre);
                        }
                        //if (Other_SSRSPMax.indexOf($('#othssridpriority_' + j)[0].value + "WEbMeaLWEB" + paxre) == -1) {
                        //    Other_SSRSPMax.push($('#othssridpriority_' + j)[0].value + "WEbMeaLWEB" + paxre);
                        //}
                    }
                    else {
                        var priorityvalue = Other_SSRSPMax.indexOf($('#othssridpriority_' + j)[0].value + "WEbMeaLWEB" + paxre);//$('#othssridpriority_' + j)[0].value;
                        if (Other_SSRPCIN.indexOf($('#othssridpriority_' + j)[0].value + "WEbMeaLWEB" + paxre) > -1) {
                            Other_SSRPCIN.pop($('#othssridpriority_' + j)[0].value + "WEbMeaLWEB" + paxre);
                        }
                        //if (Other_SSRSPMax.indexOf($('#othssridpriority_' + j)[0].value + "WEbMeaLWEB" + paxre) > -1) {
                        //    Other_SSRSPMax.pop($('#othssridpriority_' + j)[0].value + "WEbMeaLWEB" + paxre);
                        //}
                        if (priorityvalue > -1)
                        {
                            Other_SSRSPMax.splice(priorityvalue, 1);
                        }
                    }
                }
            }
        }

        if ($('.bagoutfirclss').hasClass) {
            var otherprilen = $('.bagoutfirclss').length;

            if (otherprilen != 0) {
                var id = $('.bagoutfirclss')[0].id;
                for (var k = 0; k < otherprilen; k++) {
                    var paxre = i + 1;
                    if ($('#othssrid_' + k)[0].checked == true) {
                        var priorityvalue = $('#othssrid_' + k)[0].value;
                        var priamt = priorityvalue.toString().split("WEbMeaLWEB");
                        bagoutfirst_amount = Number(parseFloat(bagoutfirst_amount) + (parseFloat(priamt[3]))).toFixed(decimalflag);
                        otherprivalue += $('#othssrid_' + k)[0].value + "WEbMeaLWEB" + paxre + "~";
                        if (Other_bagout.indexOf($('#othssrid_' + k)[0].value + "WEbMeaLWEB" + paxre) == -1) {
                            Other_bagout.push($('#othssrid_' + k)[0].value + "WEbMeaLWEB" + paxre);
                        }
                    }
                    else {
                        var priorityvalue = Other_bagout.indexOf($('#othssrid_' + k)[0].value + "WEbMeaLWEB" + paxre);//$('#othssrid_' + k)[0].value;
                        //if (Other_bagout.indexOf($('#othssrid_' + k)[0].value + "WEbMeaLWEB" + paxre) > -1) {
                        //    Other_bagout.pop($('#othssrid_' + k)[0].value + "WEbMeaLWEB" + paxre);
                        //}
                        if (priorityvalue > -1) {
                            Other_bagout.splice(priorityvalue, 1);
                        }
                    }
                }
            }
        }
        //}

        var Others_amt = 0;
        for (var i = 0; i < Other_SSRPCIN.length; i++) {
            var amount = Other_SSRPCIN[i].split("WEbMeaLWEB")[3];
            Others_amt = Number(parseFloat(Others_amt) + parseFloat(amount)).toFixed(decimalflag);
        }
        for (var i = 0; i < Other_SSRSPMax.length; i++) {
            var amount = Other_SSRSPMax[i].split("WEbMeaLWEB")[3];
            Others_amt = Number(parseFloat(Others_amt) + parseFloat(amount)).toFixed(decimalflag);
        }
        for (var i = 0; i < Other_bagout.length; i++) {
            var amount = Other_bagout[i].split("WEbMeaLWEB")[3];
            Others_amt = Number(parseFloat(Others_amt) + parseFloat(amount)).toFixed(decimalflag);
        }


        var Seat_amount_row = 0;
        var tot_seat_amount = 0;
        var total_amount = $("#Li_Totalfare").data("li_totalfare");// //changed by udhaya...2017-07-18 (temp) document.getElementById("Li_Totalfare").innerHTML;
        //GrossTotM += Number(totMeal).toFixed(decimalflag);
        GrossTotM = Number(parseFloat(GrossTotM) + parseFloat(totMeal)).toFixed(decimalflag);
        // grossbagg += Number(totBagg).toFixed(decimalflag);
        grossbagg = Number(parseFloat(grossbagg) + parseFloat(totBagg)).toFixed(decimalflag);
        var seatamount = document.getElementById("seatamount").innerHTML;
        tot_seat_amount = seatamount;
        var insurane_amt = 0;
        if ($("#hdn_producttype").val() == "RIYA") {
            insurane_amt = $(".riyains")[0].innerHTML;  //riyains
        }
        else {
            insurane_amt = document.getElementById("insamount").innerHTML;
        }
        var TotalGross = Number(parseFloat(total_amount) + parseFloat(GrossTotM) + parseFloat(grossbagg) + parseFloat(tot_seat_amount) + parseFloat(insurane_amt) + parseFloat(Others_amt)).toFixed(decimalflag);//+ parseFloat(Spmaxamount) + parseFloat(otherpriamount) + parseFloat(bagoutfirst_amount);// //changed by udhaya...2017-07-18 parseFloat(tot_seat_amount)
        document.getElementById("mealamount").innerHTML = Number(GrossTotM).toFixed(decimalflag);
        document.getElementById("BaggageAmnt").innerHTML = Number(grossbagg).toFixed(decimalflag);
        document.getElementById("seatamount").innerHTML = Number(tot_seat_amount).toFixed(decimalflag);

        var totalotherssrdet = Number(parseFloat(bagoutfirst_amount) + parseFloat(otherpriamount) + parseFloat(Spmaxamount)).toFixed(decimalflag);
        document.getElementById("othssramount").innerHTML = Number(Others_amt).toFixed(decimalflag);

        if ($("#hdn_producttype").val() != "RIYA") {
            document.getElementById("Li_Totalfare").innerHTML = Number(TotalGross).toFixed(decimalflag);// parseFloat(total_amount);
        }
        else {
            document.getElementById("Li_Totalfare").innerHTML = Number(total_amount).toFixed(decimalflag);
        }
        if (AvailFormat == "NAT" || $("#hdn_HideComission").val() == "Y") {/* STS-166*/
            var totalfare = $("#totalAmnt").data("totalamntd");
            var TotalfareGross = Number(parseFloat(totalfare) + parseFloat(GrossTotM) + parseFloat(grossbagg) + parseFloat(tot_seat_amount) + parseFloat(insurane_amt) + parseFloat(Others_amt)).toFixed(decimalflag);
            $("#totalAmnt").html(TotalfareGross);
        }
        document.getElementById("spnFare").innerText = Number(TotalGross).toFixed(decimalflag);

        if ($("#hdn_bookingprev").val() == "Y") {
            var totalotherssrdet = Number(parseFloat(bagoutfirst_amount) + parseFloat(otherpriamount) + parseFloat(Spmaxamount)).toFixed(decimalflag);
            $(".riyamel").each(function (i, vale) {
                $(".riyamel")[i].innerHTML = Number(GrossTotM).toFixed(decimalflag);
            })
            $(".riyabag").each(function (i, vale) {
                $(".riyabag")[i].innerHTML = Number(grossbagg).toFixed(decimalflag);
            })
            $(".riyasea").each(function (i, vale) {
                $(".riyasea")[i].innerHTML = Number(tot_seat_amount).toFixed(decimalflag);
            })

            $(".riyaohssr").each(function (i, vale) {
                $(".riyaohssr")[i].innerHTML = Number(Others_amt).toFixed(decimalflag);
            })

            $(".riyatotal").each(function (i, vale) {
                $(".riyatotal")[i].innerHTML = Number(TotalGross).toFixed(decimalflag);
            })
        }

        if ($("#hdn_producttype").val() == "RIYA") {
            var totalotherssrdet = Number(parseFloat(bagoutfirst_amount) + parseFloat(otherpriamount) + parseFloat(Spmaxamount)).toFixed(decimalflag);
            $(".riyamel").each(function (i, vale) {
                $(".riyamel")[i].innerHTML = Number(GrossTotM).toFixed(decimalflag);
            })
            $(".riyabag").each(function (i, vale) {
                $(".riyabag")[i].innerHTML = Number(grossbagg).toFixed(decimalflag);
            })
            $(".riyasea").each(function (i, vale) {
                $(".riyasea")[i].innerHTML = Number(tot_seat_amount).toFixed(decimalflag);
            })

            $(".riyaohssr").each(function (i, vale) {
                $(".riyaohssr")[i].innerHTML = Number(Others_amt).toFixed(decimalflag);
            })

            $(".riyatotal").each(function (i, vale) {
                $(".riyatotal")[i].innerHTML = Number(TotalGross).toFixed(decimalflag);
            })

            var commamt = $("#hdn_comm").val() == null || $("#hdn_comm").val() == "" ? 0 : $("#hdn_comm").val();
            var Servicecharge = $("#hdn_servicecharge").val() == null || $("#hdn_servicecharge").val() == "" ? 0 : $("#hdn_servicecharge").val();
            var Insamt = Number($(".riyains")[0].innerHTML).toFixed(0) != 0 ? $(".riyains")[0].innerHTML : "";
            var Ins_comm = (Insamt != "") ? Insamt / 2 : 0;
            $("#netframt")["0"].innerHTML = Number(parseFloat(TotalGross) - parseFloat(commamt) - parseFloat(Ins_comm) - parseFloat(Servicecharge)).toFixed(decimalflag);
            //$(".riyabag")[0].innerHTML = grossbagg;
            //$(".riyasea")[0].innerHTML = tot_seat_amount;
            //$(".riyaohssr")[0].innerHTML = Others_amt;
            //$(".riyatotal")[0].innerHTML = TotalGross.toFixed(0);
        }

        if ($("#hdn_seatavail").val().toUpperCase() == "TRUE") {
            if (seatamount != null && seatamount != "" && parseFloat(seatamount) <= 0) {
                //$("#seatmap_checkbox")[0].checked = false;
            }
        }

        for (var j = 0; j < Other_SSRSPMax.length; j++) {
            if (Other_SSRSPMax[j] != "") {
                var Primother = Other_SSRSPMax[j].split('WEbMeaLWEB');
                if (Primother[0].toUpperCase().indexOf("PRIM") > -1) {
                    var paxcheck = primessr.filter(function (obj) { return obj.Itin == Primother[1] && obj.Segment == Primother[2] && obj.Origin == Primother[4] && obj.Destination == Primother[5] && obj.PaxNo == Primother[6] });
                    if (paxcheck.length == 0) {
                        primessr.push({ Itin: Primother[1], Segment: Primother[2], Origin: Primother[4], Destination: Primother[5], PaxNo: Primother[6], MODE: "I" });
                    }
                }
            }
        }

        $("#modal-SSR-container").modal("hide");
    }
    catch (Error) {
        alert(Error.message);
    }
}

var paxrownumber = "";
var hdnpaxtype = "";
var ssrindex = "";
var paxcounttotal = "";
var clearpaxtype = "";
function showseatdetails(SSRid, index) {
    ssrindex = "";
    ssrindex = index;
    paxcounttotal = "";
    clearpaxtype = ""

    $('#seatmappopup')[0].innerHTML = "";

    paxrownumber = "";
    hdnpaxtype = "";
    var showtable = "";
    var paxtype = SSRid.title;
    clearpaxtype = paxtype;
    var paxtypeid = SSRid.id;
    paxrownumber = index;
    hdnpaxtype = SSRid.title;
    var rowid = index;

    var paxcount = $('#' + paxtypeid).attr("data-paxcount");
    paxcounttotal = paxcount;
    var adtcount = paxcount.split("@")[0];
    var chdcount = paxcount.split("@")[1];
    //data-paxcount
    if (paxtype == "adult") {
        var showsegment = "";
        showtable += "<table style='margin-top:6px;float:left;'>"
        for (i = 0; i < segmentarr.length; i++) {
            showtable += "<tr><td>" + segmentarr[i] + "</td></tr>";
        }
        showtable += "</table>";

        showtable += "<table style='width:30%;margin-left:40%;'>";

        for (var seg = 0; seg < segmentarr.length; seg++) {
            for (i = 0; i < book_seatx.length; i++) {
                if (book_seatx[i] != null && book_seatx[i] != "") {
                    var paxrowid = book_seatx[i].split('~')[0];
                    var segrowid = book_seatx[i].split('~')[1];
                    var paxtype = book_seatx[i].split('~')[4];
                    var Seamtno = book_seatx[i].split('~')[2];
                    var SeamtAmt = book_seatx[i].split('~')[3];
                    var ddd = paxtype.indexOf("Adult");
                    if (rowid == paxrowid && ddd != -1 && paxtype == "Adult" + rowid + "" && seg == parseInt(segrowid) - 1 && Seamtno != "Not Selected" && SeamtAmt != "Not Selected" && Seamtno != "" && SeamtAmt != "" && Seamtno != null && SeamtAmt != null) {
                        if (segmentarr[i].replace("->", "SEAT") == (book_seatx[i].split('~')[7] + "SEAT" + book_seatx[i].split('~')[8])) {
                            showtable += "<tr><td class='showseatdet' id='showdetail" + segmentarr[i].replace("->", "SEAT") + "'>" + book_seatx[i].split('~')[2] + "(" + book_seatx[i].split('~')[3] + ")" + "</td><td><input type='button' style='display:block;' class='Closeseat TxRedButton refreshbutton cls" + book_seatx[i].split('~')[7] + "SEAT" + book_seatx[i].split('~')[8] + "' value='Reset Seat' id='Resetseat" + i + "' data-val='" + book_seatx[i] + "'  onclick='Resetseatvalue(this,0);' /></td></tr>";//segrowid + "" + paxtype
                        }
                        //seg++;
                    }
                    else if (ddd != -1 && seg == parseInt(segrowid) - 1 && rowid == paxrowid) {
                        showtable += "<tr><td>Not Selected</td></tr>";
                        //seg++;
                    }
                }

            }
        }
        showtable += "</table>";
        $('#seatmappopup')[0].innerHTML = showtable;
    }
    else if (paxtype == "child") {
        var showsegment = "";
        showtable += "<table style='margin-top:6px;float:left;'>"
        for (i = 0; i < segmentarr.length; i++) {
            showtable += "<tr><td>" + segmentarr[i] + "</td></tr>";
        }
        showtable += "</table>";
        //$('#seatmappopup')[0].innerHTML = showtable;
        showtable += "<table style='width:30%;margin-left:40%;'>";
        for (var seg = 0; seg < segmentarr.length; seg++) {
            for (i = 0; i < book_seatx.length; i++) {
                if (book_seatx[i] != null && book_seatx[i] != "") {
                    $('#seatmaptable').css("display", "block");
                    var paxrowid = book_seatx[i].split('~')[0];
                    var segrowid = book_seatx[i].split('~')[1];
                    var paxtype = book_seatx[i].split('~')[4];
                    var Seamtno = book_seatx[i].split('~')[2];
                    var SeamtAmt = book_seatx[i].split('~')[3];
                    var ddd = paxtype.indexOf("Child");
                    if (parseInt(rowid) + parseInt(adtcount) == paxrowid && ddd != -1 && paxtype == "Child" + rowid + "" && seg == parseInt(segrowid) - 1 && Seamtno != "Not Selected" && SeamtAmt != "Not Selected" && Seamtno != "" && SeamtAmt != "" && Seamtno != null && SeamtAmt != null) {
                        if (segmentarr[i].replace("->", "SEAT") == (book_seatx[i].split('~')[7] + "SEAT" + book_seatx[i].split('~')[8])) {
                            showtable += "<tr><td class='showseatdet' id='showdetail" + segmentarr[i].replace("->", "SEAT") + "'>" + book_seatx[i].split('~')[2] + "(" + book_seatx[i].split('~')[3] + ")" + "</td><td><input type='button' class='Closeseat TxRedButton refreshbutton cls" + book_seatx[i].split('~')[7] + "SEAT" + book_seatx[i].split('~')[8] + "' id='Resetseat" + i + "' value='Reset Seat' data-val='" + book_seatx[i] + "' onclick='Resetseatvalue(this,0);' /></td></tr>";//segrowid + "" + paxtype 
                        }
                    }
                    else if (ddd != -1 && seg == parseInt(segrowid) - 1 && parseInt(rowid) + parseInt(adtcount) == paxrowid) {
                        showtable += "<tr><td>Not Selected</td></tr>";
                    }
                }
            }
        }
        showtable += "</table>";
        $('#seatmappopup')[0].innerHTML = showtable;
    }
    if (book_seatx != null && book_seatx != "") {
        $('#Seatmapheading').css('display', 'block');
    }
}

function Resetseatvalue(result,Flag) {
    try
    {
        var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;
        var valuereslt = "";
        if (Flag == "0") {
            var iddd = result.id;

            $('#' + iddd).css("display", "none");
            var valuereslt = $('#' + iddd).attr("data-val");
            var segrow = valuereslt.split('~')[1];
            var paxtype = valuereslt.split('~')[4];

            $('#showdetail' + valuereslt.split('~')[7] + 'SEAT' + valuereslt.split('~')[8])[0].innerHTML = "Not Selected";//valuereslt.split('~')[7] + 'SEAT' + valuereslt.split('~')[8]//segrow + '' + paxtype
        }
        else {
            valuereslt = result;
        }

        var cancelarr = [];
        for (var i = 0; i < book_seatx.length; i++) {
            cancelarr.push(book_seatx[i]);
        }

        var seatminusfare = 0;

        for (var cl = 0; cl < cancelarr.length; cl++) {
            if (valuereslt == cancelarr[cl]) {
                var rowid = cancelarr[cl].split('~')[0];
                var segid = cancelarr[cl].split('~')[1];
                var Setno = "Not Selected";
                var setmt = "Not Selected";
                var paxtype = cancelarr[cl].split('~')[4];
                var seatref = cancelarr[cl].split('~')[5];
                seatminusfare = cancelarr[cl].split('~')[3];
                var cancelarrval = rowid + "~" + segid + "~" + "Not Selected" + "~" + "Not Selected" + "~" + paxtype + "~" + seatref;
                cancelarr[cl] = cancelarrval;
            }
        }

        book_seatx = [];
        var seatamount = 0;
        for (var cs = 0; cs < cancelarr.length; cs++) {
            if (cancelarr[cs] != null && cancelarr[cs] != "") {
                book_seatx.push(cancelarr[cs]);
            }
        }
        for (var fr = 0; fr < book_seatx.length; fr++) {
            if (book_seatx[fr].split('~')[3] != "" && book_seatx[fr].split('~')[3] != null && book_seatx[fr].split('~')[3] != "Not Selected") {
                seatamount = Number(parseFloat(seatamount) + parseFloat(book_seatx[fr].split('~')[3])).toFixed(decimalflag);
            }
        }

        var tot_seat_amount = seatminusfare;

        var TotalGross = $("#Li_Totalfare").data("li_totalfare");// document.getElementById("Li_Totalfare").innerHTML; //STS-166
        var seatfare = document.getElementById("seatamount").innerHTML;
        var mealamount = document.getElementById("mealamount").innerHTML;
        var Baggageamount = document.getElementById("BaggageAmnt").innerHTML;
        var OtherSSRAmount = document.getElementById("othssramount").innerHTML;

        var seatminusfareless = Number(parseFloat(seatfare) - parseFloat(seatminusfare)).toFixed(decimalflag);

        var lessseatfare = Number(parseFloat(seatminusfareless) + parseFloat(TotalGross) + parseFloat(mealamount) + parseFloat(Baggageamount) + parseFloat(OtherSSRAmount)).toFixed(decimalflag);

        document.getElementById("seatamount").innerHTML = Number(seatminusfareless).toFixed(decimalflag);
        if ($("#hdn_producttype").val() != "RIYA") {
            document.getElementById("Li_Totalfare").innerHTML = parseFloat(lessseatfare).toFixed(decimalflag);// parseFloat(TotalGross);
        }
        else {
            document.getElementById("Li_Totalfare").innerHTML = parseFloat(TotalGross).toFixed(decimalflag);
        }

        if (AvailFormat == "NAT" || $("#hdn_HideComission").val() == "Y") {/* STS-166*/
            var totalfare = $("#totalAmnt").data("totalamntd");
            var TotalfareGross = Number(parseFloat(seatminusfareless) + parseFloat(totalfare) + parseFloat(mealamount) + parseFloat(Baggageamount) + parseFloat(OtherSSRAmount)).toFixed(decimalflag);
            $("#totalAmnt").html(TotalfareGross);
        }

        document.getElementById("spnFare").innerText = parseFloat(lessseatfare).toFixed(decimalflag);


        if ($("#hdn_bookingprev").val() == "Y") {
            $(".riyasea").each(function (i, vale) {
                $(".riyasea")[i].innerHTML = Number(seatminusfareless).toFixed(decimalflag);
            })
            $(".riyatotal").each(function (i, vale) {
                $(".riyatotal")[i].innerHTML = Number(lessseatfare).toFixed(decimalflag);
            })
        }

        if ($("#hdn_producttype").val() == "RIYA") {
            $(".riyasea").each(function (i, vale) {
                $(".riyasea")[i].innerHTML = Number(seatminusfareless).toFixed(decimalflag);
            })
            $(".riyatotal").each(function (i, vale) {
                $(".riyatotal")[i].innerHTML = Number(lessseatfare).toFixed(decimalflag);
            })
        }
    }
    catch (e)
    {
        console.log(e.message);
    }
}

function Resetseat(value) {

    var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;

    var cancelarr = [];
    for (var i = 0; i < book_seatx.length; i++) {
        cancelarr.push(book_seatx[i]);
    }


    var paxindex = paxrownumber;
    var paxindextype = hdnpaxtype;
    if (paxindextype == "adult") {
        for (i = 0; i < cancelarr.length; i++) {
            if (cancelarr[i] != "" && cancelarr[i] != null) {
                var paxrowid = cancelarr[i].split('~')[0];
                var segrowid = cancelarr[i].split('~')[1];
                var paxtype = cancelarr[i].split('~')[4];
                if (rowid == paxrowid && paxtype == "ADULT") {
                    cancelarr[i] = "";
                }
            }
        }
    }
    else if (paxindextype = "child") {
        for (i = 0; i < cancelarr.length; i++) {
            if (cancelarr[i] != "" && cancelarr[i] != null) {
                var paxrowid = cancelarr[i].split('~')[0];
                var segrowid = cancelarr[i].split('~')[1];
                var paxtype = cancelarr[i].split('~')[4];
                if (rowid == paxrowid && paxtype == "CHILD") {
                    cancelarr[i] = "";
                }
            }
        }
    }
    var seatamount = 0;
    for (var cs = 0; cs < cancelarr.length; cs++) {
        if (cancelarr[cs] != null && cancelarr[cs] != "") {
            book_seatx.push(cancelarr[cs]);
            seatamount = Number(parseFloat(seatamount) + parseFloat(book_seatx[cs].split('~')[4])).toFixed(decimalflag);
        }
    }

    var tot_seat_amount = seatminusfare;

    var TotalGross = Number(parseFloat(total_amount) + parseFloat(tot_seat_amount)).toFixed(decimalflag);
    document.getElementById("seatamount").innerHTML = parseFloat(tot_seat_amount).toFixed(decimalflag);

    if ($("#hdn_producttype").val() != "RIYA") {
        document.getElementById("Li_Totalfare").innerHTML = TotalGross.toFixed(decimalflag);// parseFloat(total_amount);
    }
    else {
        document.getElementById("Li_Totalfare").innerHTML = document.getElementById("Li_Totalfare").innerHTML;
    }
    //document.getElementById("totalAmnt").innerHTML = TotalGross.toFixed(decimalflag); /* Commended by Rajesh for showing only ticket amount except SSR*/
    document.getElementById("spnFare").innerText = TotalGross.toFixed(decimalflag);

    if ($("#hdn_bookingprev").val() == "Y") {
        $(".riyasea").each(function (i, vale) {
            $(".riyasea")[i].innerHTML = Number(tot_seat_amount).toFixed(decimalflag);
        })
        $(".riyatotal").each(function (i, vale) {
            $(".riyatotal")[i].innerHTML = Number(TotalGross).toFixed(decimalflag);
        })
    }

    if ($("#hdn_producttype").val() == "RIYA") {
        $(".riyasea").each(function (i, vale) {
            $(".riyasea")[i].innerHTML = Number(tot_seat_amount).toFixed(decimalflag);
        })
        $(".riyatotal").each(function (i, vale) {
            $(".riyatotal")[i].innerHTML = Number(TotalGross).toFixed(decimalflag);
        })
            var commamt = $("#hdn_comm").val() == null || $("#hdn_comm").val() == "" ? 0 : $("#hdn_comm").val();
            var Servicecharge = $("#hdn_servicecharge").val() == null || $("#hdn_servicecharge").val() == "" ? 0 : $("#hdn_servicecharge").val();
            var Insamt = Number($(".riyains")[0].innerHTML).toFixed(0) != 0 ? $(".riyains")[0].innerHTML : "";
            var Ins_comm = (Insamt != "") ? Insamt / 2 : 0;
            $("#netframt")["0"].innerHTML = Number(parseFloat(TotalGross) - parseFloat(commamt) - parseFloat(Ins_comm) - parseFloat(Servicecharge)).toFixed(decimalflag);
        
    }
}


//# Region Toticket -2017-07-20 by udhaya
function toTicket() {

    var spnr = document.getElementById('S_pnr').value;

    try {

        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });


        $.ajax({
            type: "POST",
            url: ToticketOption,
            data: '{SPNR:"' + spnr + '",Page:"FBP"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (temp) {
                $.unblockUI();

                if (temp.Status == "-1") {
                    window.top.location = sessionExb;
                }
                if (temp.Status == "1") {
                    $("#divprintticketdetails").css("display", "block");
                    $("#divtoticket").css("display", "none");

                    $("#spnBookmessage").html("Ticket Booked Successfully");

                    if (temp.TicketJson != null && temp.TicketJson != "") {
                        var parsejsonTicket = JSON.parse(temp.TicketJson);
                        for (var i = 0; i < parsejsonTicket.length; i++) {
                            $("#tdTicketNo_" + i).html(parsejsonTicket[i].TICKETNO);
                        }
                    }

                    servicepopupshow();
                } else {
                    if (temp.Message != "") {
                        showerralert(temp.Message, "", "");
                        return false;
                    } else {
                        showerralert("Unable to process your request. please contact customer care", "", "");
                        return false;
                    }

                }
            },
            error: function () {
                $.unblockUI();
                if (e.status == "500") {
                    //alert("An Internal Problem Occurred. Your Session will Expire.");
                    showerralert("An Internal Problem Occurred. Your Session will Expire.", "", "");
                    window.top.location = sessionExb;
                    return false;
                }
            }
        })
    } catch (e) {
        showerralert("Unable to process your request. please contact customer care", "", "");
        $.unblockUI();
    }
}
//# Endregion 


function get_bookinsindex() {//index



    var seatfare = 0;
    if (book_seatx != "" && book_seatx != null) {

        for (var i = 0; i < book_seatx.length; i++) {
            if (book_seatx[i].split('~')[3] != "" && book_seatx[i].split('~')[3] != null && book_seatx[i].split('~')[3] != "Not Selected") {
                seatfare += parseFloat(book_seatx[i].split('~')[3]);
            }
        }
    }


    var vlakey = $('#hdn_value_ins').attr('data-val');
    var pax_count = $('#hdn_value_ins').attr('data-count');

    var Totalpaxcount = $('#hdn_value_ins').attr('data-paxcount');

    //var vlakey = $('#adult1').attr('data-val');
    //var pax_count = $('#adult1').attr('data-count');
    //var segmentindex = $('#seatrow_' + index).attr('data-val');
    //var Totalpaxcount = $('#adult1').attr('data-paxcount');

    var adultcount = Totalpaxcount.split('@')[0];
    var chdcount = Totalpaxcount.split('@')[1];
    var infcount = Totalpaxcount.split('@')[2];

    var Adultnamedetails = "";
    var Childnamedetails = "";
    var Infantnamedetails = "";

    var Paxname = "";
    var checkflag = false;
    var Paxtyp = "";

    for (var ad = 0; ad < adultcount; ad++) {
        var Pax_name = "";
        var Paxnamedetails = "";
        if ($('#ad_Title_' + ad).val() != "" && $('#ad_Title_' + ad).val() != null && $('#ad_Title_' + ad).val() != 0) {
            Paxnamedetails = $('#ad_Title_' + ad).val();
        }
        else {
            showerralert("Please choose Adult" + (ad + 1) + " title");
            document.getElementById('insurance_confirmarion').checked = false;
            return false;
        }
        if ($('#ad_firstName_' + ad).val() != "" && $('#ad_lastName_' + ad).val() != "" && $('#ad_firstName_' + ad).val() != null && $('#ad_lastName_' + ad).val() != null && $('#ad_gender_' + ad).val() != "" && $('#ad_DOB_' + ad).val() != "" && $('#ad_passportNo_' + ad).val() != "" && $('#ad_pass_ex_date_' + ad).val() != "" && $('#ad_Cntry_' + ad).val() != "0" && $('#ad_gender_' + ad).val() != null && $('#ad_DOB_' + ad).val() != null && $('#ad_passportNo_' + ad).val() != null && $('#ad_pass_ex_date_' + ad).val() != null && $('#ad_Cntry_' + ad).val() != null) {
            Pax_name = $('#ad_firstName_' + ad).val() + "~" + $('#ad_lastName_' + ad).val() + "~" + $('#ad_gender_' + ad).val() + "~" + $('#ad_DOB_' + ad).val() + "~" + $('#ad_passportNo_' + ad).val() + "~" + $('#ad_pass_ex_date_' + ad).val() + "~" + $('#ad_Cntry_' + ad).val();
        }

        else if ($('#ad_firstName_' + ad).val() == "" || $('#ad_firstName_' + ad).val() == null) {
            showerralert("Please enter Adult" + (ad + 1) + " firstname");
            document.getElementById('insurance_confirmarion').checked = false;
            return false;
        }
        else if ($('#ad_lastName_' + ad).val() == "" || $('#ad_lastName_' + ad).val() == null) {
            showerralert("Please enter Adult" + (ad + 1) + " lastname");
            document.getElementById('insurance_confirmarion').checked = false;
            return false;
        }
        else if ($('#ad_gender_' + ad).val() == "" || $('#ad_gender_' + ad).val() == null) {
            showerralert("Please select Adult" + (ad + 1) + " gender");
            document.getElementById('insurance_confirmarion').checked = false;
            return false;
        }
        else if ($('#ad_DOB_' + ad).val() == "" || $('#ad_DOB_' + ad).val() == null) {
            showerralert("Please enter Adult" + (ad + 1) + " DOB");
            document.getElementById('insurance_confirmarion').checked = false;
            return false;
        }
        else if ($('#ad_passportNo_' + ad).val() == "" || $('#ad_passportNo_' + ad).val() == null) {
            showerralert("Please select Adult" + (ad + 1) + " passport no");
            document.getElementById('insurance_confirmarion').checked = false;
            return false;
        }
        else if ($('#ad_pass_ex_date_' + ad).val() == "" || $('#ad_pass_ex_date_' + ad).val() == null) {
            showerralert("Please enter Adult passport no" + (ad + 1) + " expired date");
            document.getElementById('insurance_confirmarion').checked = false;
            return false;
        }
        else if ($('#ad_Cntry_' + ad).val() == "0" || $('#ad_Cntry_' + ad).val() == null) {
            showerralert("Please enter Adult passport no" + (ad + 1) + " iss.country");
            document.getElementById('insurance_confirmarion').checked = false;
            return false;
        }


        if ($('#ad_DOB_' + ad).val() != null && $('#ad_DOB_' + ad).val() != "") {
            Paxtyp = Calculateage($("#ad_DOB_" + ad).val(), "ADULT");
            if (Paxtyp == false) {
                showerralert("Please enter valid Adult" + (ad + 1) + " DOB");
                document.getElementById('insurance_confirmarion').checked = false;
                //showerralert("Please Enter the valid " + paty + paxtypcnt + " DOB.", "", "");
                $("#ad_DOB_" + ad).val("");
                $("#ad_DOB_" + ad).focus();
                checkflag = true;
                return;
            }
        }


        Adultnamedetails += Paxnamedetails + "@" + Pax_name + "#";
    }



    if (chdcount > 0) {
        for (var chd = 0; chd < chdcount; chd++) {

            var Pax_name = "";
            var Paxnamedetails = "";

            if ($('#ch_Title_' + chd).val() != "" && $('#ch_Title_' + chd).val() != null && $('#ch_Title_' + chd).val() != 0) {
                Paxnamedetails = $('#ch_Title_' + chd).val();
            }
            else {
                showerralert("Please enter Child" + (chd + 1) + " title");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }
            //if ($('#ch_firstName_' + chd).val() != "" && $('#ch_lastName_' + chd).val() != "" && $('#ch_firstName_' + chd).val() != null && $('#ch_lastName_' + chd).val() != null) {
            //    Pax_name = $('#ch_firstName_' + chd).val() + "~" + $('#ch_lastName_' + chd).val();
            //}
            if ($('#ch_firstName_' + chd).val() != "" && $('#ch_lastName_' + chd).val() != "" && $('#ch_firstName_' + chd).val() != null && $('#ch_lastName_' + chd).val() != null && $('#ch_gender_' + chd).val() != "" && $('#ch_DOB_' + chd).val() != "" && $('#ch_passportNo_' + chd).val() != "" && $('#ch_pass_ex_date_' + chd).val() != "" && $('#ch_Cntry_' + chd).val() != "0" && $('#ch_gender_' + chd).val() != null && $('#ch_DOB_' + chd).val() != null && $('#ch_passportNo_' + chd).val() != null && $('#ch_pass_ex_date_' + chd).val() != null && $('#ch_Cntry_' + chd).val() != null) {
                Pax_name = $('#ch_firstName_' + chd).val() + "~" + $('#ch_lastName_' + chd).val() + "~" + $('#ch_gender_' + chd).val() + "~" + $('#ch_DOB_' + chd).val() + "~" + $('#ch_passportNo_' + chd).val() + "~" + $('#ch_pass_ex_date_' + chd).val() + "~" + $('#ch_Cntry_' + chd).val();
            }


            else if ($('#ch_firstName_' + chd).val() == "" || $('#ch_firstName_' + chd).val() == null) {
                showerralert("Please enter Child" + (chd + 1) + " firstname");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }
            else if ($('#ch_lastName_' + chd).val() == "" || $('#ch_lastName_' + chd).val() == null) {
                showerralert("Please enter Child" + (chd + 1) + " lastname");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }

            else if ($('#ch_gender_' + chd).val() == "" || $('#ch_gender_' + chd).val() == null) {
                showerralert("Please select Child" + (chd + 1) + " gender");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }
            else if ($('#ch_DOB_' + chd).val() == "" || $('#ch_DOB_' + chd).val() == null) {
                showerralert("Please enter Child" + (chd + 1) + " DOB");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }

            else if ($('#ch_passportNo_' + chd).val() == "" || $('#ch_passportNo_' + chd).val() == null) {
                showerralert("Please select Child" + (chd + 1) + " passport no");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }
            else if ($('#ch_pass_ex_date_' + chd).val() == "" || $('#ch_pass_ex_date_' + chd).val() == null) {
                showerralert("Please enter Child passport no" + (chd + 1) + " expired date");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }

            else if ($('#ch_Cntry_' + chd).val() == "0" || $('#ch_Cntry_' + chd).val() == null) {
                showerralert("Please enter Child passport no" + (chd + 1) + " iss.country");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }
            if ($('#ch_DOB_' + chd).val() != null && $('#ch_DOB_' + chd).val() != "") {
                Paxtyp = Calculateage($("#ch_DOB_" + chd).val(), "CHILD");
                if (Paxtyp == false) {
                    showerralert("Please enter valid Child" + (chd + 1) + " DOB");
                    document.getElementById('insurance_confirmarion').checked = false;
                    //showerralert("Please Enter the valid " + paty + paxtypcnt + " DOB.", "", "");
                    $("#ch_DOB_" + chd).val("");
                    $("#ch_DOB_" + chd).focus();
                    checkflag = true;
                    return;
                }




            }
            Childnamedetails += Paxnamedetails + "@" + Pax_name + "#";
        }

    }


    if (infcount > 0) {
        for (var inf = 0; inf < infcount; inf++) {

            var Pax_name = "";
            var Paxnamedetails = "";

            if ($('#in_Title_' + inf).val() != "" && $('#in_Title_' + inf).val() != null && $('#in_Title_' + inf).val() != 0) {
                Paxnamedetails = $('#in_Title_' + inf).val();
            }
            else {
                showerralert("Please enter Infant" + (inf + 1) + " title");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }
            //if ($('#ch_firstName_' + chd).val() != "" && $('#ch_lastName_' + chd).val() != "" && $('#ch_firstName_' + chd).val() != null && $('#ch_lastName_' + chd).val() != null) {
            //    Pax_name = $('#ch_firstName_' + chd).val() + "~" + $('#ch_lastName_' + chd).val();
            //}
            if ($('#in_firstName_' + inf).val() != "" && $('#in_lastName_' + inf).val() != "" && $('#in_firstName_' + inf).val() != null && $('#in_lastName_' + inf).val() != null && $('#in_gender_' + inf).val() != "" && $('#in_DOB_' + inf).val() != "" && $('#in_passportNo_' + inf).val() != "" && $('#in_pass_ex_date_' + inf).val() != "" && $('#in_Cntry_' + inf).val() != "0" && $('#in_gender_' + inf).val() != null && $('#in_DOB_' + inf).val() != null && $('#in_passportNo_' + inf).val() != null && $('#in_pass_ex_date_' + inf).val() != null && $('#in_Cntry_' + inf).val() != null) {
                Pax_name = $('#in_firstName_' + inf).val() + "~" + $('#in_lastName_' + inf).val() + "~" + $('#in_gender_' + inf).val() + "~" + $('#in_DOB_' + inf).val() + "~" + $('#in_passportNo_' + inf).val() + "~" + $('#in_pass_ex_date_' + inf).val() + "~" + $('#in_Cntry_' + inf).val();
            }


            else if ($('#in_firstName_' + inf).val() == "" || $('#in_firstName_' + inf).val() == null) {
                showerralert("Please enter Infant" + (inf + 1) + " firstname");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }
            else if ($('#in_lastName_' + inf).val() == "" || $('#in_lastName_' + inf).val() == null) {
                showerralert("Please enter Infant" + (inf + 1) + " lastname");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }

            else if ($('#in_gender_' + inf).val() == "" || $('#in_gender_' + inf).val() == null) {
                showerralert("Please select Infant" + (inf + 1) + " gender");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }
            else if ($('#in_DOB_' + inf).val() == "" || $('#in_DOB_' + inf).val() == null) {
                showerralert("Please enter Infant" + (inf + 1) + " DOB");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }

            else if ($('#in_passportNo_' + inf).val() == "" || $('#in_passportNo_' + inf).val() == null) {
                showerralert("Please select Infant " + (inf + 1) + " passport no");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }
            else if ($('#in_pass_ex_date_' + inf).val() == "" || $('#in_pass_ex_date_' + inf).val() == null) {
                showerralert("Please enter Infant passport no" + (inf + 1) + " expired date");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }

            else if ($('#in_Cntry_' + inf).val() == "0" || $('#in_Cntry_' + inf).val() == null) {
                showerralert("Please enter Infant passport no" + (inf + 1) + " iss.country");
                document.getElementById('insurance_confirmarion').checked = false;
                return false;
            }

            if ($('#in_DOB_' + inf).val() != null && $('#in_DOB_' + inf).val() != "") {
                Paxtyp = Calculateage($("#in_DOB_" + inf).val(), "INFANT");
                if (Paxtyp == false) {
                    showerralert("Please enter valid Infant" + (inf + 1) + " DOB");
                    document.getElementById('insurance_confirmarion').checked = false;
                    //showerralert("Please Enter the valid " + paty + paxtypcnt + " DOB.", "", "");
                    $("#in_DOB_" + inf).val("");
                    $("#in_DOB_" + inf).focus();
                    checkflag = true;
                    return;
                }

            }


            Infantnamedetails += Paxnamedetails + "@" + Pax_name + "#";
        }
    }




    // $('#hdn_segmentrow').val(index);
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: getbookinsurnace, 		// Location of the service                    
        data: '{valkey: "' + vlakey + '",Paxcount:"' + pax_count + '",Paxname:"' + Paxname + '",Totalpaxcount:"' + Totalpaxcount + '",Adultnamedetails:"' + Adultnamedetails + '",Childnamedetails:"' + Childnamedetails + '",Infantnamedetails:"' + Infantnamedetails + '"}',//Index:"' + index + '", segmentindex:"' + segmentindex + '",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call

            var result = json.Result[1];
            if (result != "") {
                document.getElementById("ifrm").src = Bookinsurance_returnUrl + "?" + result;
                document.getElementById("ifrm").style.minHeight = "500px";
                document.getElementById('modal_header').innerHTML = "Book Insurance";
                $('#ifrm').css("display", "block");
                $("#modal-getseatmaps").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
            $.unblockUI();
        },
        error: function (e) {// When Service call fails                           
            LogDetails(e.responseText, e.status, "Insurance ReBook");
            if (e.status == "500") {
                alert("An Internal Problem Occurred. Your Session will Expire.");
                window.top.location = sessionExb;
                return false;
            }
            $.unblockUI();
        }
    });
}

function bookins_sr(retvaldet, plantitle) {

    var JsonObject = JSON.parse(retvaldet);
    $("#modal-getseatmaps").modal("hide");
    $('.b-close').css("display", "none");
    //var row = $('#hdn_seatrow').val();
    $("#ifrm").attr("src", "");

    var ary = JSON.parse(retvaldet);
    document.getElementById('insurance_confirmarion').checked = true;

    if (retvaldet != null && retvaldet != "") {
        var total_amount = $("#Li_Totalfare").data("li_totalfare");
        var passenger_count = Number(ary.PolicyDetail.Adult) + Number(ary.PolicyDetail.Child)
        var PAYMET = Number(ary.Passenger[0].PremiumAmount);
        var total_tune = PAYMET * passenger_count;
        document.getElementById('insamount').innerHTML = total_tune;
        document.getElementById('insplantitle').innerHTML = "";
        document.getElementById('insplanamount').innerHTML = "";
        document.getElementById('insplantitle').innerHTML = plantitle;
        document.getElementById('insplanamount').innerHTML = total_tune;
        document.getElementById('resetinsbtn').style.visibility = "visible";

        var mealamount = document.getElementById('mealamount').innerHTML;
        var BaggageAmnt = document.getElementById('BaggageAmnt').innerHTML;
        var seatamount = document.getElementById('seatamount').innerHTML;

        var TotalGross = parseFloat(total_amount) + parseFloat(total_tune) + parseFloat(mealamount) + parseFloat(BaggageAmnt) + parseFloat(seatamount);
        if ($("#hdn_producttype").val() != "RIYA") {
            document.getElementById("Li_Totalfare").innerHTML = TotalGross.toFixed(0);// 
        }
        else {
            document.getElementById("Li_Totalfare").innerHTML = total_amount.toFixed(0);// 
        }

        if (AvailFormat == "NAT" || $("#hdn_HideComission").val() == "Y") {/* STS-166*/
            var totalfare = $("#totalAmnt").data("totalamntd");
            var TotalfareGross = parseFloat(totalfare) + parseFloat(total_tune) + parseFloat(mealamount) + parseFloat(BaggageAmnt) + parseFloat(seatamount);
            $("#totalAmnt").html(TotalfareGross.toFixed(0));
        }

        document.getElementById("spnFare").innerText = TotalGross.toFixed(0);

        if (document.getElementById('insurance_confirmarion').checked == true) {
            sessionStorage.setItem("retvaldet", "YES")
            sessionStorage.setItem("insvalues", retvaldet)
        }
        else {
            sessionStorage.setItem("retvaldet", "NO")
        }
    }
    else {
        sessionStorage.setItem("retvaldet", "NO")
        //document.getElementById('retvaldet').value = "NO";
    }
    // alert(retval);
    // document.getElementById('insamount').innerHTML = ary


}


function Calculateage(birthday, Paxtype) {

    var re = /^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d+$/;
    var lblAge = '';
    var retValue = "";
    if (birthday != '') {

        if (re.test(birthday)) {
            var newdate = moment().format("DD/MM/YYYY");
            var day = birthday.split('/');
            var dateNow = $("#hdn_arrvaldate")[0].value.split('/');
            var a = moment([day[2], day[1] - 1, day[0]]);
            var b = moment([dateNow[2], dateNow[1] - 1, dateNow[0]]);
            lblAge = b.diff(a, 'days') // 1
        }
        else {
            //alert('Date must be dd/mm/yyyy format!');
            showerralert("Date must be dd/mm/yyyy format!", "", "");
            return false;
        }
    }
    if (lblAge > 4380 && lblAge <= 37000) { retValue = "ADULT"; }
    else if (lblAge <= 731 && lblAge >= 0) { retValue = "INFANT"; }
    else if (lblAge <= 4380 && lblAge > 730) { retValue = "CHILD"; }
    else {
        //showerralert("Date must be dd/mm/yyyy format!", "", "");
        return false;
    }
    if (retValue == Paxtype) {
        return true;
    }
    else {
        return false;
    }
}

function clkspicemax() {
    if ($('.othSpicemaxcls')[0].checked == true) {
        $("#prioritycheckinheading").css("display", "none");
        $("#prioritycheckinpopup").css("display", "none");
        $("#bagoutheading").css("display", "none");
        $("#bagoutpoup").css("display", "none");
        $('.otherpriorit')[0].checked = false;
        $('.bagoutfirclss')[0].value = "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0";
        $(".bagoutfirclss").prop("disabled", true);
        Other_SSRPCIN = [];
        Other_bagout = [];
    }
    else {
        $("#prioritycheckinheading").css("display", "block");
        $("#prioritycheckinpopup").css("display", "block");
        $("#bagoutheading").css("display", "block");
        $("#bagoutpoup").css("display", "block");
        $(".bagoutfirclss").prop("disabled", false);
    }
}

function bookingsummarypassengerdet() {

    $("#spnptnamountsamp").html($("#spngrossamds").html());
    var s = "";
    var Paxdetails = $("body").data("ValidateBookingDET");
    var paxvisemeals = $("body").data("SelectedMealsInPaxx");
    var paxviseBaggages = $("body").data("SelectedBaggageallpax");
    var strpassendet = "";

    var SelectSeats = "";
    for (var i = 0; i < book_seatx.length; i++) {
        SelectSeats += book_seatx[i] + "_";
    }
    s += '<div class="row row0 scrl-btm-line" style="width: 100%;margin-bottom:0px;border-bottom: 2px solid #5bb6ef;padding-bottom: 5px;">';
    s += '<div class="col-sm-12 col0">';
    s += '<h5 id="" style="float: left;font-size:15px !important;">Passenger details</h5>';
    s += '</div>';
    s += '</div>';
    s += '<div class="row row0" style="border-bottom: 1px solid #ccc;">';
    s += '<div class="col-sm-4 col-xs-4 cmnclsspassnger">';
    s += '<span>Name </span>';
    s += '</div>';
    s += '<div class="col-sm-3 col-xs-3 cmnclsspassnger">';
    s += '<span>Meals</span>';
    s += '</div>';
    s += '<div class="col-sm-3 col-xs-3 cmnclsspassnger">';
    s += '<span> Baggage </span>';
    s += '</div>';
    s += '<div class="col-sm-2 col-xs-2 cmnclsspassnger">';
    s += '<span> Seat </span>';
    s += '</div>';

    s += '</div>';
    for (var h = 0; h < Paxdetails.split("~").length; h++) {
        if (Paxdetails.split("~")[h] != "") {
            var eachpaxmeals = "";
            var eachpaxbaggage = "";



            var eachpaxcount = Paxdetails.split("~")[h].split("^").length;
            if (Paxdetails.split("~")[h].split("|")[0].indexOf("Infant") == -1 && Paxdetails.split("~")[h].split("|")[0].indexOf("INF") == -1) {
                if (paxvisemeals != null && paxvisemeals != "") {
                    eachpaxmeals = paxvisemeals.split("~")[h].split("#");
                }
                if (paxviseBaggages != null && paxviseBaggages != "") {
                    if (($("#chkMulticity").length > 0 && $("#chkMulticity")[0].checked == true)) {
                        eachpaxbaggage = paxviseBaggages.split("&")[h].split("^");
                    } else {
                        eachpaxbaggage = paxviseBaggages.split("&")[h].split("~");
                    }
                }
            }
            var eachpax = Paxdetails.split("~")[h].split("^");
            for (var b = 0; b < eachpaxcount - 1; b++) {
                s += '<div class="row row0" style="">';
                s += '<div class="col-sm-4 col-xs-4 cmnclsspassnger">';
                s += '<span> ' + eachpax[b].split("|")[1] + " " + eachpax[b].split("|")[2] + " " + eachpax[b].split("|")[3] + ' </span>';
                s += '</div>';
                s += '<div class="col-sm-3 col-xs-3 cmnclsspassnger">';
                if (eachpaxmeals != "") {
                    if (($("#chkMulticity").length > 0 && $("#chkMulticity")[0].checked == true)) { //Multicity Meals

                        var meltmpamt = "";
                        if (h == 0) {
                            var paxmelary = $.grep(eachpaxmeals, function (n, l) {
                                return n.split("SplitKey")[4] == "Adt" + Number(b + 1);
                            });
                            if (paxmelary.length > 0) {
                                for (var ml = 0 ; ml < paxmelary.length ; ml++) {
                                    var MealNamemul = paxmelary[ml].split("SplitKey")[1].indexOf("|") != -1 ? paxmelary[ml].split("SplitKey")[1].split("|")[0] : paxmelary[ml].split("SplitKey")[1].split("*")[0];
                                    meltmpamt += MealNamemul + ", ";
                                }
                            }
                            meltmpamt = removelastcomma(meltmpamt);
                            meltmpamt = meltmpamt == "" ? "--" : meltmpamt;
                            s += '<span>' + meltmpamt + ' <span>';
                        }
                        else if (h == 1) {
                            var segcnt = Selected_Onward_Avail_Counts;
                            for (var chdm = b; chdm < eachpaxmeals.length - 1; chdm = chdm + Number(segcnt) + 1) {
                                if (eachpaxmeals[chdm] != "") {
                                    var MealNamemul = eachpaxmeals[chdm].split("SplitKey")[1].indexOf("|") != -1 ? eachpaxmeals[chdm].split("SplitKey")[1].split("|")[0] : eachpaxmeals[chdm].split("SplitKey")[1].split("*")[0];
                                    meltmpamt += MealNamemul + ", ";
                                }
                            }
                            meltmpamt = removelastcomma(meltmpamt);
                            meltmpamt = meltmpamt == "" ? "--" : meltmpamt;
                            s += '<span>' + meltmpamt + ' <span>';
                        }

                    }
                    else if (eachpaxmeals[b] != "" && eachpaxmeals[b].indexOf("$") != -1) { //Roundtrip Meals
                        var tempmelamt = "";
                        for (var me = 0; me < eachpaxmeals[b].split("$").length ; me++) {
                            if (eachpaxmeals[b].split("$")[me] != "") {
                                tempmelamt += eachpaxmeals[b].split("$")[me].split("SplitKey")[1].split("|")[0] + ", ";
                            }
                        }
                        tempmelamt = removelastcomma(tempmelamt);
                        tempmelamt = tempmelamt == "" ? "--" : tempmelamt;
                        s += '<span>' + tempmelamt + ' <span>';
                    }
                    else if (eachpaxmeals[b] != "") { //Oneway Meals
                        var MealName = eachpaxmeals[b].split("SplitKey")[1].indexOf("|") != -1 ? eachpaxmeals[b].split("SplitKey")[1].split("|")[0] : eachpaxmeals[b].split("SplitKey")[1].split("*")[0];
                        s += '<span>' + MealName + ' <span>';
                    }
                    else {
                        s += '<span> -- </span>';
                    }

                } else {
                    s += '<span> -- </span>';
                }
                s += '</div>';
                s += '<div class="col-sm-3 col-xs-3 cmnclsspassnger">';
                if (eachpaxbaggage != "" && eachpaxbaggage[b] != "" && eachpaxbaggage[b] != undefined) {
                    if (($("#chkMulticity").length > 0 && $("#chkMulticity")[0].checked == true)) { // Multicity
                        var tempmulbagamt = "";
                        var mulbag = eachpaxbaggage[b].split("~");
                        for (var c = 0 ; c < mulbag.length; c++) {
                            if (mulbag[c] != "") {
                                tempmulbagamt += mulbag[c].split("*")[5] + ", ";
                            }
                        }
                        tempmulbagamt = removelastcomma(tempmulbagamt);
                        tempmulbagamt = tempmulbagamt == "" ? "--" : tempmulbagamt;
                        s += '<span>' + tempmulbagamt + ' <span>';

                    }
                    else if (eachpaxbaggage[b].indexOf("%") != -1) { //Roundtrip
                        var tempbgamt = "";
                        for (var bg = 0 ; bg < eachpaxbaggage[b].split("%").length ; bg++) {
                            if (eachpaxbaggage[b].split("%")[bg] != "") {
                                tempbgamt += eachpaxbaggage[b].split("%")[bg].split("*")[5] + ", ";
                            }
                        }

                        tempbgamt = removelastcomma(tempbgamt);
                        tempbgamt = tempbgamt == "" ? "--" : tempbgamt;
                        s += '<span>' + tempbgamt + ' <span>';
                    }
                    else {//Oneway
                        s += '<span>' + eachpaxbaggage[b].split("*")[5] + ' <span>';
                    }

                } else {
                    s += '<span> -- </span>';
                }
                s += '</div>';
                s += '<div class="col-sm-2 col-xs-2 cmnclsspassnger">';
                var totstlpcnt = 0;
                totstlpcnt = ($("#chkMulticity").length > 0 && $("#chkMulticity")[0].checked == true) && ($(".clsDomestic")[0].checked == true || sessionStorage.getItem("starttwooneway") == "Y") ? aryOrgMul.length : 1;

                for (var deflopp = 0; deflopp < totstlpcnt; deflopp++) {
                    var testbook_seatx = totstlpcnt != 1 ? book_seatx[deflopp] : book_seatx;
                    if (testbook_seatx != null && testbook_seatx != "" && testbook_seatx.length != 0 && Paxdetails.split("~")[h].split("|")[0].indexOf("Infant") == -1) {

                        var seattempamt = "";
                        if (h == 0) {
                            var paxseat = $.grep(testbook_seatx, function (n, g) {
                                return testbook_seatx[g].split("~")[4] == "Adult" + Number(b + 1);
                            });
                        }
                        else if (h == 1) {
                            var paxseat = $.grep(testbook_seatx, function (n, r) {
                                return testbook_seatx[r].split("~")[4] == "Child" + Number(b + 1);
                            });
                        }
                        if (paxseat.length > 0) {
                            for (var st = 0 ; st < paxseat.length ; st++) {
                                seattempamt += paxseat[st].split("~")[2] + ",";
                            }
                        }
                        seattempamt = deflopp == 0 || deflopp == totstlpcnt.length - 1 ? removelastcomma(seattempamt) : seattempamt;
                        s += '<span>' + seattempamt + '</span>';

                    }

                    else {
                        s += '<span> -- </span>';
                    }
                }
                s += '</div>';
                s += '</div>';
            }
        }

    }
    $(".passengerdetailsummary").html(s);
}

function ShowSSRPopup(ID, SSR) {
    try{
        var paxcount = $('#' + ID).data("paxcount");
        var popupid = $('#' + ID).data("popupid");
        var adtcount = paxcount.split("@")[0];
        var chdcount = paxcount.split("@")[1];
        var SegCount = $('#hdnSegCount').val();
        var Totpaxcount = parseFloat(adtcount) + parseFloat(chdcount);

        if (SSR == "O") {
            for (var paxre = 1; paxre <= Totpaxcount; paxre++) {
                if (Other_SSRSPMax.length > 0) {
                    if ($('.clsOtherSSR_' + paxre).hasClass) {
                        $('.clsOtherSSR_' + paxre).each(function () {
                            if (Other_SSRSPMax.indexOf($(this).val() + "WEbMeaLWEB" + paxre) > -1) {
                                $(this)[0].checked = true;
                            }
                            else {
                                $(this)[0].checked = false;
                            }
                        });
                    }
                }
                if (Other_SSRPCIN.length > 0) {
                    if ($('.clsspicemax_' + paxre).hasClass) {
                        $('.clsspicemax_' + paxre).each(function () {
                            if (Other_SSRPCIN.indexOf($(this).val() + "WEbMeaLWEB" + paxre) > -1) {
                                $(this)[0].checked = true;
                            }
                            else {
                                $(this)[0].checked = false;
                            }
                        });
                    }
                }
                if (Other_bagout.length > 0) {
                    if ($('.bagoutfirclss_' + paxre).hasClass) {
                        $('.bagoutfirclss_' + paxre).each(function () {
                            if (Other_bagout.indexOf($(this).val() + "WEbMeaLWEB" + paxre) > -1) {
                                $(this)[0].checked = true;
                            }
                            else {
                                $(this)[0].checked = false;
                            }
                        });
                    }
                }
            }
        }
        else {
            for (var adtcnt = 1; adtcnt <= parseFloat(adtcount) ; adtcnt++) {
                if ($('#hdnadtMeals_' + (adtcnt - 1)).val() != "" && $('#hdnadtMeals_' + (adtcnt - 1)).val() != null) {
                    var mm = $('#hdnadtMeals_' + (adtcnt - 1)).val().split('~')
                    for (var segcnt = 0; segcnt < parseFloat(SegCount) ; segcnt++) {
                        var Me = "", BA = "", FA = "";
                        //
                        //var paxcheck = primessr.filter(function (obj) { return obj.Itin == segcnt.toString() && obj.PaxNo == adtcnt });
                        //
                        if (mm.length > segcnt && mm[segcnt] != "") {
                            var Im = mm[segcnt].split('SpLiTSSR');
                            Me = Im[0].toString();
                            BA = Im[1].toString();
                            FA = Im[2].toString();
                        }
                        if ($("#MealNameAdt_" + adtcnt + "_" + segcnt).length > 0 && SSR == "M" && Me != "") {
                            var primemeal = $("#MealNameAdt_" + adtcnt + "_" + segcnt).data('primemeal');
                            var paxcheck = primessr.filter(function (obj) { return (obj.Origin + "CLS" + obj.Destination + "CLS" + obj.PaxNo) == primemeal });
                            if (paxcheck.length > 0) {
                                Servicemealclick("#MealNameAdt_" + adtcnt + "_" + segcnt, paxcheck[0].MODE, paxcheck[0].Origin, paxcheck[0].Destination);
                            }
                            $("#MealNameAdt_" + adtcnt + "_" + segcnt).val(Me);
                        }
                        if ($("#BaggageNameAdt_" + adtcnt + "_" + segcnt).length > 0 && SSR == "B" && BA != "") {
                            $("#BaggageNameAdt_" + adtcnt + "_" + segcnt).val(BA);
                        }
                        if ($("#frqno").val() == "TRUE" && $("#_segno_" + adtcnt + "_" + segcnt).length > 0 && $("#Freq_flyer_" + adtcnt + "_" + segcnt).length > 0 && SSR == "F") {
                            $("#Freq_flyer_" + adtcnt + "_" + segcnt).val((FA != "" ? FA.split('WEbMeaLWEB')[2] : ""));
                        }
                    }
                }
            }
            if (SSR != "F") {
                for (var chdcnt = 1; chdcnt <= parseFloat(chdcount) ; chdcnt++) {
                    var paxcount = (adtcnt - 1) + chdcnt;
                    if ($('#hdnchdMeals_' + (chdcnt - 1)).val() != "" && $('#hdnchdMeals_' + (chdcnt - 1)).val() != null) {
                        var mm = $('#hdnchdMeals_' + (chdcnt - 1)).val().split('~')
                        for (var segcnt = 0; segcnt < parseFloat(SegCount) ; segcnt++) {
                            var Me = "", BA = "", FA = "";
                            //var paxcheck = primessr.filter(function (obj) { return obj.Itin == segcnt.toString() && obj.PaxNo == paxcount });
                            if (mm.length > segcnt && mm[segcnt] != "") {
                                var Im = mm[segcnt].split('SpLiTSSR');
                                Me = Im[0].toString();
                                BA = Im[1].toString();
                                FA = Im[2].toString();
                            }
                            if ($("#MealNameChd_" + chdcnt + "_" + segcnt).length > 0 && SSR == "M" && Me != "") {
                                var primemeal = $("#MealNameChd_" + chdcnt + "_" + segcnt).data('primemeal');
                                var paxcheck = primessr.filter(function (obj) { return (obj.Origin + "CLS" + obj.Destination + "CLS" + obj.PaxNo) == primemeal });
                                if (paxcheck.length > 0) {
                                    Servicemealclick("#MealNameChd_" + chdcnt + "_" + segcnt, paxcheck[0].MODE, paxcheck[0].Origin, paxcheck[0].Destination);
                                }
                                $("#MealNameChd_" + chdcnt + "_" + segcnt).val(Me);
                            }
                            if ($("#BaggageNameChd_" + chdcnt + "_" + segcnt).length > 0 && SSR == "B" && BA != "") {
                                $("#BaggageNameChd_" + chdcnt + "_" + segcnt).val(BA);
                            }
                        }
                    }
                }
            }
        }

        if (ID == "dvPrimeSSRbox") {
            $('.clstblprime').show();
            $('.clstblotherssr').hide();
        }
        else if (ID == "dvOtherSSRbox") {
            $('.clstblotherssr').show();
            $('.clstblprime').hide();
        }
        if (($("#hdn_AppHosting").val() == "BSA" || $("#hdn_AppHosting").val() == "B2B" || $("#hdn_AppHosting").val() == "BOA") && ($("#hdn_producttype").val() == "DEIRA" ||$("#hdn_producttype").val()=="ROUNDTRIP") ) {
            $(popupid).addClass("slideInRight").removeClass("slideOutRight").show();
            $(".overlayssr").show();
        }
        else {
            $(popupid).removeClass('slideOutRight');
            $(popupid).modal({
                backdrop: 'static',
                keyboard: false
            });
        }
    }
    catch(e){
        console.log(e.message);
    }
}

function SubmitSSR(ID, SSR, MODE) {
     try {
        var paxcount = $('#' + ID).data("paxcount");//hdn_value
        var adtcount = paxcount.split("@")[0];
        var chdcount = paxcount.split("@")[1];
        var Totpaxcount = parseFloat(adtcount) + parseFloat(chdcount);
        var totMeal = 0, totBaggage = 0, Others_amt = 0;
        var SegCount = $('#hdnSegCount').val();
        var MealsRequest = "";
        var BaggageRequest = "";
        var FFNRequest = "";
        var TotalSSRRequest = "";
        var Me = "", BA = "", FA = "";

        GrossTotM = 0, grossbagg = 0;
        for (var adtcnt = 1; adtcnt <= parseFloat(adtcount) ; adtcnt++) {
            TotalSSRRequest = "";
            var mm = $('#hdnadtMeals_' + (adtcnt - 1)).val().split('~');
            for (var segcnt = 0; segcnt < parseFloat(SegCount) ; segcnt++) {
                MealsRequest = "", BaggageRequest = "", FFNRequest = "", Me = "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0", BA = "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0", FA = "";

                if (mm.length > segcnt && mm[segcnt] != "") {
                    var Im = mm[segcnt].split('SpLiTSSR');
                    Me = Im[0].toString();
                    BA = Im[1].toString();
                    FA = Im[2].toString();
                }
                
                if ($("#MealNameAdt_" + adtcnt + "_" + segcnt).length > 0 && SSR == "M") {
                    if (MODE == "D") {
                        $("#MealNameAdt_" + adtcnt + "_" + segcnt).val("0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0");
                    }
                    totMeal += Number($("#MealNameAdt_" + adtcnt + "_" + segcnt).val().split('WEbMeaLWEB')[1]);
                    MealsRequest = $("#MealNameAdt_" + adtcnt + "_" + segcnt).val();// + "SPLITSEGMEALS"
                }
                else {
                    totMeal += Me.split('WEbMeaLWEB').length > 1 ? (Me.split('WEbMeaLWEB')[1] != "" && Me.split('WEbMeaLWEB')[1] != null ? Number(Me.split('WEbMeaLWEB')[1]) : 0) : 0;
                    MealsRequest = Me;
                }

                if ($("#BaggageNameAdt_" + adtcnt + "_" + segcnt).length > 0 && SSR == "B") {
                    if (MODE == "D") {
                        $("#BaggageNameAdt_" + adtcnt + "_" + segcnt).val("0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0");
                    }
                    totBaggage += Number($("#BaggageNameAdt_" + adtcnt + "_" + segcnt).val().split('WEbMeaLWEB')[1]);
                    BaggageRequest = $("#BaggageNameAdt_" + adtcnt + "_" + segcnt).val();
                }
                else {
                    totBaggage += BA.split('WEbMeaLWEB').length > 1 ? (BA.split('WEbMeaLWEB')[1] != "" && BA.split('WEbMeaLWEB')[1] != null ? Number(BA.split('WEbMeaLWEB')[1]) : 0) : 0;
                    BaggageRequest = BA;
                }

                if ($("#frqno").val() == "TRUE" && $("#_segno_" + adtcnt + "_" + segcnt).length > 0 && $("#Freq_flyer_" + adtcnt + "_" + segcnt).length > 0 && SSR == "F") {
                    if (MODE == "D") {
                        $("#Freq_flyer_" + adtcnt + "_" + segcnt).val("");
                    }
                    FFNRequest = $("#_segno_" + adtcnt + "_" + segcnt).val() + "WEbMeaLWEB" + $("#Freq_flyer_" + adtcnt + "_" + segcnt).val() + "WEbMeaLWEB" + $("#ItRef_Freq_flyer_" + adtcnt + "_" + segcnt).val();
                }
                else {
                    FFNRequest = FA;
                }
                TotalSSRRequest += MealsRequest + "SpLiTSSR" + BaggageRequest + "SpLiTSSR" + FFNRequest + "~";
            }
            //MealsRequest += "SPLITPAXMEALS";
            $('#hdnadtMeals_' + (adtcnt - 1)).val(TotalSSRRequest);
        }

        for (var chdcnt = 1; chdcnt <= parseFloat(chdcount) ; chdcnt++) {
            TotalSSRRequest = "";
            var mm = $('#hdnchdMeals_' + (chdcnt - 1)).val().split('~');
            for (var segcnt = 0; segcnt < parseFloat(SegCount) ; segcnt++) {
                MealsRequest = "", BaggageRequest = "", FFNRequest = "", Me = "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0", BA = "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0", FA = "";
                if (mm.length > segcnt && mm[segcnt] != "") {
                    var Im = mm[segcnt].split('SpLiTSSR');
                    Me = Im[0].toString();
                    BA = Im[1].toString();
                }
                if ($("#MealNameChd_" + chdcnt + "_" + segcnt).length > 0 && SSR == "M") {
                    if (MODE == "D") {
                        $("#MealNameChd_" + chdcnt + "_" + segcnt).val("0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0");
                    }
                    totMeal += Number($("#MealNameChd_" + chdcnt + "_" + segcnt).val().split('WEbMeaLWEB')[1]);
                    MealsRequest = $("#MealNameChd_" + chdcnt + "_" + segcnt).val();
                }
                else {
                    totMeal += Me.split('WEbMeaLWEB').length > 1 ? (Me.split('WEbMeaLWEB')[1] != "" && Me.split('WEbMeaLWEB')[1] != null ? Number(Me.split('WEbMeaLWEB')[1]) : 0) : 0;
                    MealsRequest = Me;
                }
                if ($("#BaggageNameChd_" + chdcnt + "_" + segcnt).length > 0 && SSR == "B") {
                    if (MODE == "D") {
                        $("#BaggageNameChd_" + chdcnt + "_" + segcnt).val("0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0");
                    }
                    totBaggage += Number($("#BaggageNameChd_" + chdcnt + "_" + segcnt).val().split('WEbMeaLWEB')[1]);
                    BaggageRequest = $("#BaggageNameChd_" + chdcnt + "_" + segcnt).val();
                }
                else {
                    totBaggage += BA.split('WEbMeaLWEB').length > 1 ? (BA.split('WEbMeaLWEB')[1] != "" && BA.split('WEbMeaLWEB')[1] != null ? Number(BA.split('WEbMeaLWEB')[1]) : 0) : 0;
                    BaggageRequest = BA;
                }
                TotalSSRRequest += MealsRequest + "SpLiTSSR" + BaggageRequest + "SpLiTSSR" + FFNRequest + "~";
            }
            $('#hdnchdMeals_' + (chdcnt - 1)).val(TotalSSRRequest);
        }
       
        if (SSR == "O") {
            var RemoveSSR = [];
           
            $('.bagoutfirclss').attr('checked', false);
            $('.othSpicemaxcls').attr('checked', false);
            $('.otherpriorit').attr('checked', false);

            for (var paxre = 1; paxre <= Totpaxcount; paxre++) {
                if (MODE == "I") {
                    if ($('.clsOtherSSR_' + paxre).hasClass) {
                        $('.clsOtherSSR_' + paxre).each(function () {
                            if ($(this)[0].checked == true) {
                                if (Other_SSRSPMax.indexOf($(this).val() + "WEbMeaLWEB" + paxre) == -1) {
                                    Other_SSRSPMax.push($(this).val() + "WEbMeaLWEB" + paxre);
                                }
                            }
                            else {
                                var popvalue = Other_SSRSPMax.indexOf($(this).val() + "WEbMeaLWEB" + paxre);
                                if (popvalue > -1) {
                                    RemoveSSR.push($(this).val() + "WEbMeaLWEB" + paxre);
                                    Other_SSRSPMax.splice(popvalue, 1);
                                }
                            }
                        });
                    }
                    if ($('.clsspicemax_' + paxre).hasClass) {
                        $('.clsspicemax_' + paxre).each(function () {
                            if ($(this)[0].checked == true) {
                                if (Other_SSRPCIN.indexOf($(this).val() + "WEbMeaLWEB" + paxre) == -1) {
                                    Other_SSRPCIN.push($(this).val() + "WEbMeaLWEB" + paxre);
                                }
                            }
                            else {
                                var popvalue = Other_SSRPCIN.indexOf($(this).val() + "WEbMeaLWEB" + paxre);
                                if (popvalue > -1) {
                                    Other_SSRPCIN.splice(popvalue, 1);
                                }
                            }
                        });
                    }
                    if ($('.bagoutfirclss_' + paxre).hasClass) {
                        $('.bagoutfirclss_' + paxre).each(function () {
                            if ($(this)[0].checked == true) {
                                if (Other_bagout.indexOf($(this).val() + "WEbMeaLWEB" + paxre) == -1) {
                                    Other_bagout.push($(this).val() + "WEbMeaLWEB" + paxre);
                                }
                            }
                            else {
                                var popvalue = Other_bagout.indexOf($(this).val() + "WEbMeaLWEB" + paxre);
                                if (popvalue > -1) {
                                    Other_bagout.splice(popvalue, 1);
                                }
                            }
                        });
                    }
                }
                else {
                    Other_SSRSPMax = [];
                    Other_bagout = [];
                    Other_SSRPCIN = [];

                    $('.bagoutfirclss_' + paxre).attr('checked', false);
                    $('.clsspicemax_' + paxre).attr('checked', false);
                    $('.clsOtherSSR_' + paxre).attr('checked', false);
                }
            }

            //PRIME
            var removeprime = [], addprime = [], mealprime=[];
            if (MODE == "I") {
                //primessr = [];
                for (var j = 0; j < Other_SSRSPMax.length; j++) {
                    if (Other_SSRSPMax[j] != "") {
                        var Primother = Other_SSRSPMax[j].split('WEbMeaLWEB');
                        if (Primother[0].toUpperCase().indexOf("PRIM") > -1) {
                            var paxcheck = primessr.filter(function (obj) { return obj.Itin == Primother[1] && obj.Segment == Primother[2] && obj.Origin == Primother[4] && obj.Destination == Primother[5] && obj.PaxNo == Primother[6] });
                            if (paxcheck.length == 0) {
                                primessr.push({ Itin: Primother[1], Segment: Primother[2], Origin: Primother[4], Destination: Primother[5], PaxNo: Primother[6], MODE: "I" });
                                addprime.push({ Itin: Primother[1], Segment: Primother[2], Origin: Primother[4], Destination: Primother[5], PaxNo: Primother[6], MODE: "I" });
                            }
                        }
                    }
                }
                for (var j = 0; j < RemoveSSR.length; j++) {
                    if (RemoveSSR[j] != "") {
                        var Primother = RemoveSSR[j].split('WEbMeaLWEB');
                        if (Primother[0].toUpperCase().indexOf("PRIM") > -1) {
                            removeprime.push({ Itin: Primother[1], Segment: Primother[2], Origin: Primother[4], Destination: Primother[5], PaxNo: Primother[6], MODE: "D" });
                        }
                    }
                }

                $.each(removeprime, function (obj, val) {
                    $.each(primessr, function (obj1, val1) {
                        if (val1.Itin == val.Itin && val1.Segment == val.Segment && val1.Origin == val.Origin && val1.Destination == val.Destination && val1.PaxNo == val.PaxNo) {
                            primessr.splice(obj1, 1);
                            return false;
                        }
                    });
                });

                mealprime = $.merge(addprime, removeprime);
            }
            else {
                mealprime = primessr;
                primessr = [];
            }
            
            if (mealprime.length > 0) {
                //MEAL
                totMeal = 0;
                for (var adtcnt = 1; adtcnt <= parseFloat(adtcount) ; adtcnt++) {
                    TotalSSRRequest = "";
                    var mm = $('#hdnadtMeals_' + (adtcnt - 1)).val().split('~');
                    for (var segcnt = 0; segcnt < parseFloat(SegCount) ; segcnt++) {
                        //var paxcheck = mealprime.filter(function (obj) { return obj.Itin == segcnt.toString() && obj.PaxNo == adtcnt });
                        Me = "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0", BA = "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0", FA = "";
                        if ($("#MealNameAdt_" + adtcnt + "_" + segcnt).length > 0) {
                            var primemeal = $("#MealNameAdt_" + adtcnt + "_" + segcnt).data('primemeal');
                            var paxcheck = mealprime.filter(function (obj) { return (obj.Origin + "CLS" + obj.Destination + "CLS" + obj.PaxNo) == primemeal });
                            if (mm.length > segcnt && mm[segcnt] != "") {
                                var Im = mm[segcnt].split('SpLiTSSR');
                                if (paxcheck.length > 0) {
                                    Me = "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0"
                                }
                                else {
                                    Me = Im[0].toString();
                                }
                                BA = Im[1].toString();
                                FA = Im[2].toString();
                            }

                            if (paxcheck.length > 0) {
                                Servicemealclick("#MealNameAdt_" + adtcnt + "_" + segcnt, MODE == "D" ? "D" : paxcheck[0].MODE, paxcheck[0].Origin, paxcheck[0].Destination);
                            }
                            totMeal += Me.split('WEbMeaLWEB').length > 1 ? (Me.split('WEbMeaLWEB')[1] != "" && Me.split('WEbMeaLWEB')[1] != null ? Number(Me.split('WEbMeaLWEB')[1]) : 0) : 0;
                        }
                        TotalSSRRequest += Me + "SpLiTSSR" + BA + "SpLiTSSR" + FA + "~";
                    }
                    $('#hdnadtMeals_' + (adtcnt - 1)).val(TotalSSRRequest);
                }
                for (var chdcnt = 1; chdcnt <= parseFloat(chdcount) ; chdcnt++) {
                    var paxcount = parseFloat(adtcount) + chdcnt;
                    TotalSSRRequest = "";
                    var mm = $('#hdnchdMeals_' + (chdcnt - 1)).val().split('~');
                    for (var segcnt = 0; segcnt < parseFloat(SegCount) ; segcnt++) {
                        //var paxcheck = mealprime.filter(function (obj) { return obj.Itin == segcnt.toString() && obj.PaxNo == paxcount });
                        Me = "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0", BA = "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0", FA = "";
                        if ($("#MealNameChd_" + chdcnt + "_" + segcnt).length > 0) {
                            var primemeal = $("#MealNameChd_" + chdcnt + "_" + segcnt).data('primemeal');
                            var paxcheck = mealprime.filter(function (obj) { return (obj.Origin + "CLS" + obj.Destination + "CLS" + obj.PaxNo) == primemeal });
                            if (mm.length > segcnt && mm[segcnt] != "") {
                                var Im = mm[segcnt].split('SpLiTSSR');
                                if (paxcheck.length > 0) {
                                    Me = "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0"
                                }
                                else {
                                    Me = Im[0].toString();
                                }
                                BA = Im[1].toString();
                                FA = Im[2].toString();
                            }

                            if (paxcheck.length > 0) {
                                Servicemealclick("#MealNameChd_" + chdcnt + "_" + segcnt, MODE == "D" ? "D" : paxcheck[0].MODE, paxcheck[0].Origin, paxcheck[0].Destination);
                            }
                            totMeal += Me.split('WEbMeaLWEB').length > 1 ? (Me.split('WEbMeaLWEB')[1] != "" && Me.split('WEbMeaLWEB')[1] != null ? Number(Me.split('WEbMeaLWEB')[1]) : 0) : 0;
                        }
                        TotalSSRRequest += Me + "SpLiTSSR" + BA + "SpLiTSSR" + FA + "~";
                    }
                    $('#hdnchdMeals_' + (chdcnt - 1)).val(TotalSSRRequest);
                }
                //MEAL
                //SEAT
                var primeseatreset = [];
                if (book_seatx.length > 0) {
                    for (var i = 0; i < book_seatx.length; i++) {
                        if (book_seatx[i] != "" && book_seatx[i] != null) {
                            if (book_seatx[i].split('~')[3] != null && book_seatx[i].split('~')[3] != "" && book_seatx[i].split('~')[3] != "Not Selected") {
                                primeseatreset = mealprime.filter(function (obj) { return obj.Origin == book_seatx[i].split('~')[7] && obj.Destination == book_seatx[i].split('~')[8] && book_seatx[i].split('~')[0] == obj.PaxNo })
                                if (primeseatreset.length > 0)
                                {
                                    Resetseatvalue(book_seatx[i], '1');
                                }
                            }
                        }
                    }
                }
                //SEAT
            }
            //PRIME
        }
        GrossTotM = totMeal;
        grossbagg = totBaggage;
       
        for (var i = 0; i < Other_SSRPCIN.length; i++) {
            var amount = Other_SSRPCIN[i].split("WEbMeaLWEB")[3];
            Others_amt = Number(parseFloat(Others_amt) + parseFloat(amount)).toFixed(decimalflag);
        }
        for (var i = 0; i < Other_SSRSPMax.length; i++) {
            var amount = Other_SSRSPMax[i].split("WEbMeaLWEB")[3];
            Others_amt = Number(parseFloat(Others_amt) + parseFloat(amount)).toFixed(decimalflag);
        }
        for (var i = 0; i < Other_bagout.length; i++) {
            var amount = Other_bagout[i].split("WEbMeaLWEB")[3];
            Others_amt = Number(parseFloat(Others_amt) + parseFloat(amount)).toFixed(decimalflag);
        }
        if (MODE == "I") {
            $("#MealsPopup,#BaggagePopup,#FFNPopup,#OtherSSRPopup").modal('hide');
        }
        //

        var total_amount = $("#Li_Totalfare").data("li_totalfare");
        var seatamount = $("#seatamount").html();
        var insurane_amt = 0;
        if ($("#hdn_producttype").val() == "RIYA") {
            insurane_amt = $(".riyains")[0].innerHTML;  
        }
        else {
            insurane_amt = $("#insamount").html();
        }
        var TotalGross = Number(parseFloat(total_amount) + parseFloat(totMeal) + parseFloat(totBaggage) + parseFloat(seatamount) + parseFloat(insurane_amt) + parseFloat(Others_amt)).toFixed(decimalflag);

        $("#mealamount").html( Number(totMeal).toFixed(decimalflag));
        $("#BaggageAmnt").html(Number(totBaggage).toFixed(decimalflag));
        $("#othssramount").html(Number(Others_amt).toFixed(decimalflag));

        if ($("#hdn_producttype").val() != "RIYA") {
            $("#Li_Totalfare").html(Number(TotalGross).toFixed(decimalflag));
        }
        else {
            $("#Li_Totalfare").html(Number(total_amount).toFixed(decimalflag));
        }
        if (AvailFormat == "NAT" || $("#hdn_HideComission").val() == "Y") {
            var totalfare = $("#totalAmnt").data("totalamntd");
            var TotalfareGross = Number(parseFloat(totalfare) + parseFloat(totMeal) + parseFloat(totBaggage) + parseFloat(seatamount) + parseFloat(insurane_amt) + parseFloat(Others_amt)).toFixed(decimalflag);
            $("#totalAmnt").html(TotalfareGross);
        }
        document.getElementById("spnFare").innerText = Number(TotalGross).toFixed(decimalflag);

        var commamt = $("#hdn_comm").val() == null || $("#hdn_comm").val() == "" ? 0 : $("#hdn_comm").val();
        var Servicecharge = $("#hdn_servicecharge").val() == null || $("#hdn_servicecharge").val() == "" ? 0 : $("#hdn_servicecharge").val();
        var Insamt = Number($(".riyains")[0].innerHTML).toFixed(0) != 0 ? $(".riyains")[0].innerHTML : "";
        var Ins_comm = (Insamt != "") ? Insamt / 2 : 0;
        
        ;
        if ($("#hdn_bookingprev").val() == "Y") {
            $(".riyamel").each(function (i, vale) {
                $(".riyamel")[i].innerHTML = Number(totMeal).toFixed(decimalflag);
            });
            $(".riyabag").each(function (i, vale) {
                $(".riyabag")[i].innerHTML = Number(totBaggage).toFixed(decimalflag);
            });
            $(".riyasea").each(function (i, vale) {
                $(".riyasea")[i].innerHTML = Number(seatamount).toFixed(decimalflag);
            });
            $(".riyaohssr").each(function (i, vale) {
                $(".riyaohssr")[i].innerHTML = Number(Others_amt).toFixed(decimalflag);
            });
            $(".riyatotal").each(function (i, vale) {
                $(".riyatotal")[i].innerHTML = Number(TotalGross).toFixed(decimalflag);
            });
            var Cspayableamt = Number(((parseFloat(TotalGross)) < parseFloat($("#hdn_CreditshellAmt").val().split('|')[0])) ? 0 : ((parseFloat(TotalGross)) > parseFloat($("#hdn_CreditshellAmt").val().split('|')[0])) ? ((parseFloat(TotalGross)) - parseFloat($("#hdn_CreditshellAmt").val().split('|')[0])) : 0).toFixed(decimalflag);
            //var Cspayableamt = Number((parseFloat(TotalGross) > parseFloat($("#hdn_CreditshellAmt").val().slice('|')[0])) ? (parseFloat(TotalGross) - parseFloat($("#hdn_CreditshellAmt").val().slice('|')[0])) : 0).toFixed(decimalflag);
            $(".cls-payableamt").each(function (i, vale) {
                $(".cls-payableamt")[i].innerHTML = Number(Cspayableamt).toFixed(decimalflag);
            });
            var csRemainamt = Number(parseFloat(TotalGross) - parseFloat(Servicecharge) - parseFloat(Ins_comm)).toFixed(decimalflag);
            $(".cls-RemainCreditshellamt").each(function (i, vale) {
                $(".cls-RemainCreditshellamt")[i].innerHTML = Number((parseFloat(csRemainamt) < parseFloat(parseFloat($("#hdn_CreditshellAmt").val().split('|')[0]))) ? ((parseFloat($("#hdn_CreditshellAmt").val().split('|')[0])) - parseFloat(csRemainamt)) : 0).toFixed(decimalflag);
            });
        }

        if ($("#hdn_producttype").val() == "RIYA") {
            $(".riyamel").each(function (i, vale) {
                $(".riyamel")[i].innerHTML = Number(totMeal).toFixed(decimalflag);
            });
            $(".riyabag").each(function (i, vale) {
                $(".riyabag")[i].innerHTML = Number(totBaggage).toFixed(decimalflag);
            });
            $(".riyasea").each(function (i, vale) {
                $(".riyasea")[i].innerHTML = Number(seatamount).toFixed(decimalflag);
            });
            $(".riyaohssr").each(function (i, vale) {
                $(".riyaohssr")[i].innerHTML = Number(Others_amt).toFixed(decimalflag);
            });
            $(".riyatotal").each(function (i, vale) {
                $(".riyatotal")[i].innerHTML = Number(TotalGross).toFixed(decimalflag);
            });
            var Cspayableamt = Number(((parseFloat(TotalGross)) < parseFloat($("#hdn_CreditshellAmt").val().split('|')[0])) ? 0 : ((parseFloat(TotalGross)) > parseFloat($("#hdn_CreditshellAmt").val().split('|')[0])) ? ((parseFloat(TotalGross)) - parseFloat($("#hdn_CreditshellAmt").val().split('|')[0])) : 0).toFixed(decimalflag);
            //var Cspayableamt = Number((parseFloat(TotalGross) > parseFloat($("#hdn_CreditshellAmt").val().slice('|')[0])) ? (parseFloat(TotalGross) - parseFloat($("#hdn_CreditshellAmt").val().slice('|')[0])) : 0).toFixed(decimalflag);
            $(".cls-payableamt").each(function (i, vale) {
                $(".cls-payableamt")[i].innerHTML = Number(Cspayableamt).toFixed(decimalflag);
            });
            var csRemainamt = Number(parseFloat(TotalGross) - parseFloat(Servicecharge) - parseFloat(Ins_comm)).toFixed(decimalflag);
            $(".cls-RemainCreditshellamt").each(function (i, vale) {
                $(".cls-RemainCreditshellamt")[i].innerHTML = Number((parseFloat(csRemainamt) < parseFloat(parseFloat($("#hdn_CreditshellAmt").val().split('|')[0]))) ? ((parseFloat($("#hdn_CreditshellAmt").val().split('|')[0])) - parseFloat(csRemainamt)) : 0).toFixed(decimalflag);
            });
            $("#netframt").html(Number(parseFloat(TotalGross) - parseFloat(commamt) - parseFloat(Ins_comm) - parseFloat(Servicecharge)).toFixed(decimalflag)); 
        }
        if (($("#hdn_AppHosting").val() == "BSA" || $("#hdn_AppHosting").val() == "B2B" || $("#hdn_AppHosting").val() == "BOA") && ($("#hdn_producttype").val() == "DEIRA" || $("#hdn_producttype").val() == "ROUNDTRIP")) {
            $("#Li_Totalfare").html(Number(parseFloat(total_amount) + parseFloat(totMeal) + parseFloat(totBaggage) + parseFloat(seatamount) + parseFloat(insurane_amt) + parseFloat(Others_amt)).toFixed(decimalflag));
            Closemap();
        }
    }
    catch (e) {
        console.log(e.message);
    }
}
