//#region Select process
var mulreq = "";
var multicity_dom = "";

function GetRowIndexSelect_old(Id, grid, trip) {
    //    function GetRowIndexSelect_old(Id) {
    try {

        if (document.getElementById("GorssFareSpan2" + Id).innerHTML == "-") {
            showerralert("Selected class not having fare. Please change class.", "", "");
            return false;
        }
        var element = $("#li_Rows" + Id);
        var id = element.attr("class");
        var selectedObj = $.grep(dataproperty_array, function (n, i) {
            return n.datakey == Id;
        });

        var pardiv = element[0].dataset.parentdiv;
        var totaldur = element[0].dataset.duration;
        sessionStorage.setItem("totaldur", totaldur);

        $(".selebtn").removeClass("selectedbtndesn");
        $("#selectclickbutton" + Id).addClass("selectedbtndesn");

        if ($("#hdn_MobileDevice").val().toUpperCase() == "TRUE") {
            var grpcls_na = selectedObj[0]["data-grpclssname"].toString();
            if (grid == "Dgrd") {
                gorupfareformation(grpcls_na, $('#' + pardiv + ' .' + grpcls_na + '').length, pardiv, grid, Id, trip);
            }
            else {
                gorupfareformation(grpcls_na, $('#availset_O_1 .' + grpcls_na + '').length, pardiv, grid, Id, trip);
            }
        }
        else if (selectedObj[0]["data-grpclssname"] != "") {
            var grpcls_na = selectedObj[0]["data-grpclssname"].toString();

            if (grid == "Dgrd") {

                if ($('#' + pardiv + ' .' + grpcls_na + '').length > 1) {
                    sessionStorage.setItem("selectedid", grpcls_na);

                    gorupfareformation(grpcls_na, $('#' + pardiv + ' .' + grpcls_na + '').length, pardiv, grid, Id, trip);
                }
                else {
                    bindSletedRndTrpAvail(Id, trip);
                }
            }
            else {
                sessionStorage.setItem("selectedid", grpcls_na);
                if ($('#availset_O_1 .' + grpcls_na + '').length > 1) {
                    gorupfareformation(grpcls_na, $('#availset_O_1 .' + grpcls_na + '').length, pardiv, grid, Id, trip);
                }
                else {
                    GetRowIndexSelect(Id);
                }
            }
        }
    }
    catch (e) {
        showerralert(e.message, "", "");
    }
}

function GetRowIndexSelect(Id) {
    try {
        $('#modal-Fare').iziModal('close');
        if (document.getElementById("GorssFareSpan2" + Id).innerHTML == "-") {
            showerralert("Selected class not having fare. Please change class.", "", "");
            return false;
        }

        var element = $("#li_Rows" + Id);
        var id = element.attr("class");

        var selectedObj = $.grep(dataproperty_array, function (n, i) {
            return n.datakey == Id;
        });

        if (selectedObj[0]["data-bestbuypopupvalues"] != "") {
            OTApopupopenFunction(Id);
        }
        else {
            if ($.xhrPool.length > 1) {
                $($.xhrPool).each(function (idx, jqXHR) {
                    jqXHR.abort();
                });
            }

             if ($("#hdn_producttype").val() == "DEIRA") {
                 if (strProcessingImage.indexOf("loader") != -1) {
                $.blockUI({
                         message: '<img alt="Please Wait..." src="' + strProcessingImage + '" style="background-color:#fff; border-radius:8px; margin-top:12%;width:150px;"  id="' + selectloadingId + '" />',
                    css: { padding: '5px' }
                });
                 } else {

                     var img = new Image();
                     img.src = strProcessingImage;
                     img.onload = function () {
                         var ImageWidth = this.width;
                         $.blockUI({
                             message: '<div class="avail_loader"><div class="avail_img"><img alt="Please Wait..." src="' + strProcessingImage + '" style="margin-top:8%;" id="' + selectloadingId + '" /><div class="load_btm" style="width:' + ImageWidth + 'px"><img src="../../Images/Deira/process_loader.gif"/></div></div></div>',
                             css: { padding: '5px' }
                         });
                     };
                 }
             } else {
                $.blockUI({
                    message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;width:150px;" id="FareRuleLodingImage" />',
                    css: { padding: '5px' }
                });
            }

            var Fno = $('#FlightID' + Id)[0].innerHTML;
            var Deaprt = $('#span_depature' + Id)[0].innerHTML;//spanTag[2].textContent;
            var Arrive = $('#span_Arrival' + Id)[0].innerHTML; //spanTag[3].textContent;
            var alterqueue = selectedObj[0]["data-hdAlterQueue"];// $('.Travselect_' + Id)[0].dataset.hdalterqueue; //$('#hdAlterQueue' + Id).val();
            // if ($('.Travselect_' + Id)[0].dataset.hdoffflg == "1")
            if (selectedObj[0]["data-hdoffflg"] == "1") {
                if (confirm("Fare might get change because it is an offline flight..  Do you want to continue the flight selection")) {
                    var idss = Id;
                    var selectedObj = $.grep(dataproperty_array, function (n, i) {
                        return n.datakey == idss;
                    });
                    var Specialflagfare = "";
                    CheckAvail(Fno, Deaprt, Arrive, selectedObj[0]["data-hdInva"], selectedObj[0]["data-hdtoken"], selectedObj[0]["data-hdoffflg"], selectedObj[0]["data-hdbestbuyy"], selectedObj[0]["data-hdnmarkupfare"], "FALSE", alterqueue, Id, "", Specialflagfare);
                }
                $.unblockUI();
            }
            else {
                var idss = Id;
                var selectedObj = $.grep(dataproperty_array, function (n, i) {
                    return n.datakey == idss;
                });
                var Specialflagfare = "";
                CheckAvail(Fno, Deaprt, Arrive, selectedObj[0]["data-hdInva"], selectedObj[0]["data-hdtoken"], selectedObj[0]["data-hdoffflg"], selectedObj[0]["data-hdbestbuyy"], selectedObj[0]["data-hdnmarkupfare"], "FALSE", alterqueue, Id, "", Specialflagfare);

            }
        }
    }
    catch (e) {
        showerralert(e.message, "", "");
        //alert(e.message);
    }
}

function getSplfare(Id) {

    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });

    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == Id;
    });


    var Fno = $('#FlightID' + Id)[0].innerHTML;
    var Deaprt = $('#span_depature' + Id)[0].innerHTML;//spanTag[2].textContent;
    var Arrive = $('#span_Arrival' + Id)[0].innerHTML; //spanTag[3].textContent;
    var Inva = selectedObj[0]["data-hdInva"];// $('#hdInva' + Id).val();
    var tokenkey = selectedObj[0]["data-hdtoken"];
    var offflg = selectedObj[0]["data-hdoffflg"];
    var best = selectedObj[0]["data-hdbestbuyy"];// $('#hdbestbuyy' + Id).val();
    var oldfaremarkup = selectedObj[0]["data-hdnmarkupfare"];

    var check = "";
    if (document.getElementById("chkNFare").checked) {
        check = "TRUE";
    }
    else {
        check = "FALSE";
    }

    var Class_val = ($("#hdtxt_trip")[0].value == "M" ? $("#grpcmbFlightClass").val() : $("#ddlclass").val());

    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: splfrurl, 		// Location of the service
        data: '{FliNum: "' + Fno + '" ,Deaprt: "' + Deaprt + '" ,Arrive: "' + Arrive + '" ,FullFlag: "' + Inva + '" ,TokenKey: "'
            + tokenkey + '",Trip: "' + $("#hdtxt_trip")[0].value + '",BaseOrgin: "' + $("#hdtxt_origin")[0].value + '",BaseDestination: "' + $("#hdtxt_destination")[0].value
            + '",offflg: "' + offflg + '",Class: "' + Class_val + '",TKey: "' + document.getElementById("AvaiText").value + '",DEPTDATE: "' + $("#txtdeparture")[0].value
            + '",ARRDATE: "' + $("#txtarrivaldate")[0].value + '",Nfarecheck: "' + check + '",BestBuy: "' + best + '",oldfaremarkup: "' + oldfaremarkup + '",AlterQueue: "' + selectedObj[0]["data-hdAlterQueue"] + '"}',
        timeout: 100000,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call
            $.unblockUI();
            var result = json.Result;
            document.getElementById("bestbuyclickbutton" + Id).style.display = "none";
            document.getElementById("selectclickbutton" + Id).style.display = "";
            if (result[9] == "-1") {
                window.location = sessionExb;
                return false;
            }
            else if (result[9] == "0") {
                //alert(result[0]);
                showerralert(result[0], "", "");
                $("#selectclickbutton" + Id).removeClass("splselebtn").addClass('selebtn');
            }
            else if (result[9] == "1") {
                var idss = Id;
                selectedObj[0]["data-bestbuypopupvalues"] = result[8];
                // $('.Travselect_' + Id)[0].dataset.bestbuypopupvalues = result[8];
                // document.getElementById("bestbuypopupvalues" + Id).value = result[8];
                var STax_build = "";
                var strUser = "";
                var eTax = result[3];
                var Stx = eTax.split('/');
                STax_build += "<table width='130px'>";

                for (var tLen = 0; tLen < Stx.length; tLen++) {
                    var JsonTax = Stx[tLen].split(":");
                    STax_build += "<tr><td>" + JsonTax[0] + "</td><td>" + JsonTax[1] + "</td></tr>";
                }
                if (result[5] != "0") {
                    if (document.getElementById("chkNFare").checked == true) {
                        STax_build += "<tr class='Markup' style='display:table-row'><td>SC</td><td>" + result[5] + "</td></tr>";
                    }
                    else {
                        STax_build += "<tr class='Markup' style='display:none'><td>SC</td><td>" + result[5] + "</td></tr>";
                    }
                }

                if (document.getElementById("Nfare").checked == true) {
                    STax_build += "<tr class='CGFare' style='display:table-row'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + result[2] + "</td></tr>";
                    STax_build += "<tr class='WTMFare' style='display:none'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + result[1] + "</td></tr>";
                }
                else {
                    STax_build += "<tr class='CGFare'  style='display:none'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + result[2] + "</td></tr>";
                    STax_build += "<tr class='WTMFare' style='display:table-row'><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + result[1] + "</td></tr>";
                }
                STax_build += "</table>";

                if (document.getElementById("chkNFare").checked == true) {
                    strUser = "<p class='Bestbuyfarewithmarkup' style='margin-left: 0px;' ><strike>" + result[4] + "</strike></p><p class='Bestbuyfarewithoutmarkup' style='margin-left: 0px;display:none' ><strike>" + result[6] + "</strike></p><label class='lblsplfare' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + result[2] + "</label><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
                }
                else {
                    strUser = "<p class='Bestbuyfarewithmarkup' style='margin-left: 0px;display:none' ><strike>" + result[4] + "</strike></p><p class='Bestbuyfarewithoutmarkup' style='margin-left: 0px;' ><strike>" + result[6] + "</strike></p><label class='lblsplfare' id='lblGFare" + Id + "' onmousemove='CheckGrossFare(" + Id + ");' onmouseout='checkoutGrossFare(" + Id + ")'>" + result[1] + "</label><div id='taxtdiv" + Id + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
                }

                $("#GorssFareSpan2" + Id).attr('data-grrfare', result[2]);

                $("#GorssFareSpan2" + Id).attr('data-WTMFare', result[1]);

                document.getElementById("GorssFareSpan2" + Id).innerHTML = strUser;
                document.getElementById("BaseFARE" + Id).innerHTML = result[7];

                var values = result[8];
                var newValues = values.split("Splitoldnew")[1];
                var newearnings = newValues.split("Splitold")[7];
                if (newearnings != "0") {
                    $("#show_earning" + Id).addClass("licommisionSpan");
                    document.getElementById("show_earning" + Id).innerHTML = "Earn. " + newearnings;
                }
                OTApopupopenFunction(Id);
            }
            else if (result[9] == "2" || result[9] == "3") {
                $("#selectclickbutton" + Id).removeClass("splselebtn").addClass('selebtn');

                var Specialflagfare = "";
                CheckAvail(Fno, Deaprt, Arrive, selectedObj[0]["data-hdInva"], selectedObj[0]["data-hdtoken"], selectedObj[0]["data-hdoffflg"], selectedObj[0]["data-hdbestbuyy"], selectedObj[0]["data-hdnmarkupfare"], "FALSE", "S", Id, "", Specialflagfare); //Again call Select function... by saranraj...
            }
            else if (result[9] == "4") {
                $("#selectclickbutton" + Id).removeClass("splselebtn").addClass('selebtn');

                selectedObj[0]["data-hdAlterQueue"] = "S";

                if (confirm(result[0])) {
                    var Fno = $('#FlightID' + Id)[0].innerHTML;
                    var Deaprt = $('#span_depature' + Id)[0].innerHTML;
                    var Arrive = $('#span_Arrival' + Id)[0].innerHTML;
                    var alterqueue = selectedObj[0]["data-hdAlterQueue"];

                    var Specialflagfare = "";
                    CheckAvail(Fno, Deaprt, Arrive, selectedObj[0]["data-hdInva"], selectedObj[0]["data-hdtoken"], selectedObj[0]["data-hdoffflg"], selectedObj[0]["data-hdbestbuyy"], selectedObj[0]["data-hdnmarkupfare"], "FALSE", "S", Id, "", Specialflagfare);//Again call Select function after Confirmation... by saranraj...
                }
            }
            else {
                $("#selectclickbutton" + Id).removeClass("splselebtn").addClass('selebtn');
                showerralert(result[0] + " (#09).", "", "");
            }
        },
        error: function (e) {//On Successful service call
            //LogDetails(e.responseText, e.status, "Flight Select");
            if (e.statusText == "timeout") {
                showerralert("Operation timeout. Please try Again.", "", "");
            }
            if (e.status == "500") {
                window.location = sessionExb;
                return false;
            }


            $.unblockUI();
        }	// When Service call fails

    });

}

function OTApopupopenFunction(Id) {

    var idss = Id;
    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == idss;
    });

    //var values = document.getElementById("bestbuypopupvalues" + Id).value;

    var values = selectedObj[0]["data-bestbuypopupvalues"];
    var oldvalues = values.split("Splitoldnew")[0];
    var newValues = values.split("Splitoldnew")[1];
    var oldrefundable = selectedObj[0]["data-refundableforbest"]; //document.getElementById("refundableforbest" + Id).value;
    var newrefundable = newValues.split("Splitold")[4];

    var oldref = oldrefundable == "" ? "N/A" : oldrefundable == "TRUE" ? "<span class='Refundablefr'>Refundable</span>" : oldrefundable == "FALSE" ? "<span class='NonRefundablefr'>Non Refundable</span>" : "N/A";
    var newref = newrefundable == "" ? (oldrefundable == "TRUE" ? "<span class='Refundablefr'>Refundable</span>" : "<span class='NonRefundablefr'>Non Refundable</span>") : newrefundable == "TRUE" ? "<span class='Refundablefr'>Refundable</span>" : newrefundable == "FALSE" ? "<span class='NonRefundablefr'>Non Refundable</span>" : "N/A";
    var oldtaxbreakup = selectedObj[0]["data-oldtaxbreakup"]; // document.getElementById("oldtaxbreakup" + Id).value;
    var oldearnings = selectedObj[0]["data-oldcommission"];// document.getElementById("oldcommission" + Id).value;
    var newearnings = newValues.split("Splitold")[7];
    var currencycode = newValues.split("Splitold")[8];


    var STax_build = "";
    var oGrsfare = "";
    var eTax = newValues.split("Splitold")[5];
    var Stx = eTax.split('/');
    STax_build += "<table width='130px' >";

    for (var tLen = 0; tLen < Stx.length; tLen++) {
        var JsonTax = Stx[tLen].split(":");
        STax_build += "<tr><td>" + JsonTax[0] + "</td><td>" + JsonTax[1] + "</td></tr>";
    }
    if (newValues.split("Splitold")[6] != "0") {
        if (document.getElementById("Nfare").checked == true) {
            STax_build += "<tr class='Markup' style='display:table-row'><td>SC</td><td>" + newValues.split("Splitold")[6] + "</td></tr>";
        }
        else {
            STax_build += "<tr class='Markup' style='display:none'><td>SC</td><td>" + newValues.split("Splitold")[6] + "</td></tr>";
        }
    }

    if (document.getElementById("Nfare").checked == true) {
        STax_build += "<tr class='CGFare' style='display:table-row'><td>Total&nbsp;Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + newValues.split("Splitold")[2] + "</td></tr>";
        STax_build += "<tr class='WTMFare' style='display:none'><td>Total&nbsp;Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + newValues.split("Splitold")[3] + "</td></tr>";
    }
    else {
        STax_build += "<tr class='CGFare'  style='display:none'><td>Total&nbsp;Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + newValues.split("Splitold")[2] + "</td></tr>";
        STax_build += "<tr class='WTMFare' style='display:table-row'><td>Total&nbsp;Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + newValues.split("Splitold")[3] + "</td></tr>";
    }
    STax_build += "</table>";

    if (document.getElementById("Nfare").checked == true) {
        oGrsfare = "<p id='GorssFareSpa0' data-WTMFare='" + newValues.split("Splitold")[3] + "' data-grrfare='" + newValues.split("Splitold")[2] + "'><label class='lblsplfare' id='lblGFare'  onmousemove='CheckGrossFar();' onmouseout='checkoutGrossFar()'><B>" + newValues.split("Splitold")[2] + " (" + currencycode + ")" + "</B></label><div id='taxtdiv' class='Grossdivtooltip' ><div class='div-arrow div-yellow'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div></p>";
    }
    else {
        oGrsfare = "<p id='GorssFareSpa0' data-WTMFare='" + newValues.split("Splitold")[3] + "' data-grrfare='" + newValues.split("Splitold")[2] + "'><label class='lblsplfare' id='lblGFare' onmousemove='CheckGrossFar();' onmouseout='checkoutGrossFar()'><B>" + newValues.split("Splitold")[3] + " (" + currencycode + ")" + "</B></label><div id='taxtdiv' class='Grossdivtooltip' ><div class='div-arrow div-yellow'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div></p>";
    }


    var STax_build_old = "";
    var nGrsfare = "";
    var eTax_old = oldtaxbreakup;
    var Stx_old = eTax_old.split('|');
    STax_build_old += "<table width='130px' >";

    for (var tLen = 0; tLen < Stx_old.length; tLen++) {
        var JsonTax = Stx_old[tLen].split(":");
        STax_build_old += "<tr><td>" + JsonTax[0] + "</td><td>" + JsonTax[1] + "</td></tr>";
    }
    if (oldvalues.split("Splitold")[4] != "0") {
        if (document.getElementById("Nfare").checked == true) {
            STax_build_old += "<tr class='Markup' style='display:table-row'><td>SC</td><td>" + oldvalues.split("Splitold")[4] + "</td></tr>";
        }
        else {
            STax_build_old += "<tr class='Markup' style='display:none'><td>SC</td><td>" + oldvalues.split("Splitold")[4] + "</td></tr>";
        }
    }

    if (document.getElementById("Nfare").checked == true) {
        STax_build_old += "<tr class='CGFare' style='display:table-row'><td>Total&nbsp;Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + oldvalues.split("Splitold")[2] + "</td></tr>";
        STax_build_old += "<tr class='WTMFare' style='display:none'><td>Total&nbsp;Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + oldvalues.split("Splitold")[3] + "</td></tr>";
    }
    else {
        STax_build_old += "<tr class='CGFare'  style='display:none'><td>Total&nbsp;Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + oldvalues.split("Splitold")[2] + "</td></tr>";
        STax_build_old += "<tr class='WTMFare' style='display:table-row'><td>Total&nbsp;Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + oldvalues.split("Splitold")[3] + "</td></tr>";
    }
    STax_build_old += "</table>";


    if (document.getElementById("chkNFare").checked == true) {
        nGrsfare = "<span id='GorssFareSpan1' data-WTMFare='" + oldvalues.split("Splitold")[3] + "' data-grrfare='" + oldvalues.split("Splitold")[2] + "'><label id='lblGFar' onmousemove='CheckGrossFar1();' onmouseout='checkoutGrossFar1()'><B>" + oldvalues.split("Splitold")[2] + " (" + currencycode + ")" + "</B></label><div id='taxtd1' class='Grossdivtooltip' ><div class='div-arrow div-yellow'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build_old + "</div></span>";
    }
    else {
        nGrsfare = "<span id='GorssFareSpan1' data-WTMFare='" + oldvalues.split("Splitold")[3] + "' data-grrfare='" + oldvalues.split("Splitold")[2] + "'><label id='lblGFar' onmousemove='CheckGrossFar1();' onmouseout='checkoutGrossFar1()'><B>" + oldvalues.split("Splitold")[3] + " (" + currencycode + ")" + "</B></label><div id='taxtd1' class='Grossdivtooltip' ><div class='div-arrow div-yellow'></div><div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build_old + "</div></span>";
    }


    var nDiffwithoutmarkup = (oldvalues.split("Splitold")[3] - newValues.split("Splitold")[3]) + parseInt(newearnings);
    var nDiffwithmarkup = (oldvalues.split("Splitold")[2] - newValues.split("Splitold")[2]) + parseInt(newearnings);

    var bestbuypopup = "<table class='SpecFare'><thead><tr class='plan' ><th></th><th  > <h2 class='nrmlFr' >Normal Fare </h2></th><th   ><h2 class='splFr' >Special Fare </h2></th></tr>";

    bestbuypopup += "<tr class='frrow'> <th></th> <th > " + nGrsfare + "  </th>";
    bestbuypopup += "<th>" + oGrsfare + "  </th></tr></thead>";

    bestbuypopup += "<tr><td>Class</td><td>" + oldvalues.split("Splitold")[0] + "</td>";
    bestbuypopup += "<td>" + newValues.split("Splitold")[0] + "</td></tr>";
    bestbuypopup += "<tr><td>Basic&nbsp;fare</td><td>" + oldvalues.split("Splitold")[1] + " (" + currencycode + ")" + "</td>";
    bestbuypopup += "<td>" + newValues.split("Splitold")[1] + " (" + currencycode + ")" + "</td></tr>";

    bestbuypopup += "<tr><td>Earnings</td><td>" + oldearnings + " (" + currencycode + ")" + "</td>";
    bestbuypopup += "<td>" + newearnings + " (" + currencycode + ")" + "</td></tr>";
    bestbuypopup += "<tr><td></td><td>" + oldref + "</td>";
    bestbuypopup += "<td>" + newref + "</td></tr>";
    bestbuypopup += "<tr><td></td><td ><a class='color-3 puerto-btn-1 btnSpecial nrmlFr' data-check='old' onclick='javascript:SplFrselecttime(this," + Id + ")'> <span class='hidden-xs'><i class='fa fa-thumbs-down splfricon'></i></span><small> <span class='hidden-xs'>No, Not Intreseted</span><span class='hidden-lg hidden-md hidden-sm'>Select</span> </small> </a> </td>";
    bestbuypopup += "<td ><a class='color-2 puerto-btn-1  btnSpecial splFr' data-check='new' onclick='javascript:SplFrselecttime(this," + Id + ")'> <span class='hidden-xs'><i class='fa fa-thumbs-up splfricon'></i></span><small> <span class='hidden-xs'>Yes,I Save <b>" + nDiffwithmarkup + " (" + currencycode + ") </b></span><span class='hidden-lg hidden-md hidden-sm'>Select</span> </small> </a></td></tr></table>";
    document.getElementById("dvSplFareCmpr").innerHTML = bestbuypopup;

    $("#modal-splfare").modal({
        backdrop: 'static',
        keyboard: false
    });
}

function SplFrselecttime(type, Id) {
    try {


        var idss = Id;
        var selectedObj = $.grep(dataproperty_array, function (n, i) {
            return n.datakey == Id;
        });

        $("#modal-splfare").modal('hide');
        var btnselect = $(type).attr("data-check");
        var bestbuywant = "";
        if (btnselect == "old") {
            bestbuywant = "FALSE";
        }
        else {
            bestbuywant = "TRUE";
        }
        var Fno = $('#FlightID' + Id)[0].innerHTML;
        var Deaprt = $('#span_depature' + Id)[0].innerHTML;//spanTag[2].textContent;
        var Arrive = $('#span_Arrival' + Id)[0].innerHTML; //spanTag[3].textContent;
        var alterqueue = selectedObj[0]["data-hdAlterQueue"];

        var Specialflagfare = "";
        CheckAvail(Fno, Deaprt, Arrive, selectedObj[0]["data-hdInva"], selectedObj[0]["data-hdtoken"], selectedObj[0]["data-hdoffflg"], selectedObj[0]["data-hdbestbuyy"], selectedObj[0]["data-hdnmarkupfare"], bestbuywant, alterqueue, Id, "", Specialflagfare);
    }
    catch (e)
    { alert(e.message); }
}
//#region Roundtrip Select...
var roundtrip_earnings = 0;
function SelectRtripFAvail(flg, roundtripflg) {

    try {

        //  roundtrip_earnings = 0;
        var Depid = $('#selectclickbuttonRTrip').data('dep-id');
        var Rtnid = $('#selectclickbuttonRTrip').data('ret-id');
        if (Depid === "") {
            showerralert("Please Select onward flight.", 5000, "");
            return false;
        }
        if (Rtnid === "") {
            showerralert("Please Select return flight.", 5000, "");
            return false;
        }

        var Fno = $('#FlightID' + Depid)[0].innerHTML;
        var RtnFno = $('#FlightID' + Rtnid)[0].innerHTML;
        var Deaprt = $('#span_depature' + Depid)[0].innerHTML;//spanTag[2].textContent;
        var Arrive = $('#span_Arrival' + Depid)[0].innerHTML; //spanTag[3].textContent;
        var Deaprt_grid_2 = $('#span_depature' + Rtnid)[0].innerHTML;//spanTag[2].textContent;
        var Arrive_grid_2 = $('#span_Arrival' + Rtnid)[0].innerHTML; //spanTag[3].textContent;
        var Arrive_date_grid1 = $('#arrive_date_grid1' + Depid)[0].innerHTML;
        var Deaprt_date_grid2 = $('#arrive_date_grid' + Rtnid)[0].innerHTML;

        var arr_date_grid1 = new Date(Arrive_date_grid1);
        var dep_date_grid2 = new Date(Deaprt_date_grid2);
        var test_arr = arr_date_grid1.toLocaleDateString();           //convert to date
        var test_dep = dep_date_grid2.toLocaleDateString();
        Arrive = (Arrive.split(":"));
        Deaprt_grid_2 = (Deaprt_grid_2.split(":"));
        Arrive_hours = Arrive[0] * 60;
        Deaprt_grid_2_hours = Deaprt_grid_2[0] * 60;
        Arrive_total_min = Number(Arrive_hours) + Number(Arrive[1]);
        Deaprt_grid_2_total_min = Number(Deaprt_grid_2_hours) + Number(Deaprt_grid_2[1]);
        var diff_min = Deaprt_grid_2_total_min - Arrive_total_min;
        var time_interval = document.getElementById('time_interval').value;
        var time_interval_alert = document.getElementById('time_interval_alert').value;

        var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
        var temparrday = Number(test_arr.split('/')[1]) < 10 ? ("0" + test_arr.split('/')[1]) : test_arr.split('/')[1];
        var temparrmonth = (Number(test_arr.split('/')[0])) < 10 ? ("0" + test_arr.split('/')[0]) : test_arr.split('/')[0];
        var firstDate = test_arr.split('/')[2] + temparrmonth + temparrday;
        var temparrday = Number(test_dep.split('/')[1]) < 10 ? ("0" + test_dep.split('/')[1]) : test_dep.split('/')[1];
        var temparrmonth = (Number(test_dep.split('/')[0])) < 10 ? ("0" + test_dep.split('/')[0]) : test_dep.split('/')[0];
        var secondDate = test_dep.split('/')[2] + temparrmonth + temparrday;


        if (Number(secondDate) <= Number(firstDate)) {
            if (Number(diff_min) <= Number(time_interval)) {  //40<30
                showerralert("Please recheck the flights you selected.Your second flight leaves before your first flight arrives!", 5000, "");
                return false;
            }
            else if (Number(secondDate) < Number(firstDate)) {
                showerralert("Please recheck the flights you selected.Your second flight leaves before your first flight arrives!", 5000, "");
                return false;
            }
        }
        //else if (Number(secondDate) < Number(firstDate)) {
        //    showerralert("Please recheck the flights you selected.Your second flight leaves before your first flight arrives!", 5000, "");
        //    return false;
        //}
        var Inva = "", token = "", ffflg = "";
        var arylftSglpnr = $('#li_Rows' + Depid).data('snglpnr').split('~');
        var aryritSglpnr = $('#li_Rows' + Rtnid).data('snglpnr').split('~');

        var Depid_id_hidden = Depid;
        var selectedObj_Depid = $.grep(dataproperty_array, function (n, i) {
            return n.datakey == Depid_id_hidden;
        });
        var Rtnid_id_hidden = Rtnid;
        var selectedObj_Rtnid = $.grep(dataproperty_array, function (n, i) {
            return n.datakey == Rtnid_id_hidden;
        });
        var alterqueue = selectedObj_Depid[0]["data-hdAlterQueue"]; //$('#hdAlterQueue' + Depid).val();

        if ($("#hdn_producttype").val() == "RIYA" && $("#liRoundTripSpl")[0].checked == true) {
            if (Fno.split(' ')[0] != RtnFno.split(' ')[0]) {
                showerralert("Please select return flight in " + Fno.split(' ')[0], 5000, "");
                return false;
            }

            ffflg = selectedObj_Depid[0]["data-hdoffflg"] + "~" + selectedObj_Rtnid[0]["data-hdoffflg"];
            Inva = selectedObj_Depid[0]["data-hdInva"] + "SpLiTWeB" + selectedObj_Rtnid[0]["data-hdInva"];   //For Seperate Itinary... by Saranraj...
            token = selectedObj_Depid[0]["data-hdtoken"] + "SpLiTWeB" + selectedObj_Rtnid[0]["data-hdtoken"];
            $("#hdtxt_trip").val("Y"); //Roundtrip Special Select for single pnr...
            var Specialflagfare = "S"; // For Roundtrip spl pass this and in controller pass flag as empty 
            CheckAvail(Fno, Deaprt, Arrive, Inva, token, ffflg, selectedObj_Depid[0]["data-hdbestbuyy"], selectedObj_Depid[0]["data-hdnmarkupfare"], "FALSE", alterqueue, Depid, "1", Specialflagfare); //Single-PNR for Roundtrip Flight without compart Popup...
        }
        else {

            //if (arylftSglpnr.length > 3 && aryritSglpnr.length > 3 && (arylftSglpnr[0] != null && aryritSglpnr[0] != null && arylftSglpnr[0] != "" && aryritSglpnr[0] != "" && arylftSglpnr[0] == "R") && (arylftSglpnr[1] == aryritSglpnr[1]) && (arylftSglpnr[2] == aryritSglpnr[2]) && (arylftSglpnr[3] == aryritSglpnr[3]) && flg == "0" && ($("#hdn_rtsinglepnrthread").val().indexOf(arylftSglpnr[2]) > -1 && $("#hdn_rtsinglepnrthread").val().indexOf(aryritSglpnr[2]) > -1)) { // RoundTrip Special Select Process for Same Flights in Roundtrip... (to show comparable fare popup)... by saranraj...
            //    ffflg = selectedObj_Depid[0]["data-hdoffflg"] + "~" + selectedObj_Rtnid[0]["data-hdoffflg"];
            //    Inva = selectedObj_Depid[0]["data-hdInva"] + "SpLiTWeB" + selectedObj_Rtnid[0]["data-hdInva"];   //For Seperate Itinary... by Saranraj...
            //    token = selectedObj_Depid[0]["data-hdtoken"] + "SpLiTWeB" + selectedObj_Rtnid[0]["data-hdtoken"];
            //    $("#hdtxt_trip").val("R"); //Roundtrip Select for single pnr...
            //    var Specialflagfare = "P"; /* Commended by Rajesh for booking problem*/
            //    CheckAvail(Fno, Deaprt, Arrive, Inva, token, ffflg, selectedObj_Depid[0]["data-hdbestbuyy"], selectedObj_Depid[0]["data-hdnmarkupfare"], "FALSE", alterqueue, Depid, "1", Specialflagfare); //Roundtrip Spl Req for Roundtrip Avail (Comparable Popup...)
            //}
            //else
            if (arylftSglpnr.length > 3 && aryritSglpnr.length > 3 && (arylftSglpnr[0] != null && aryritSglpnr[0] != null && arylftSglpnr[0] != "" && aryritSglpnr[0] != "") && (arylftSglpnr[1] == aryritSglpnr[1]) && (arylftSglpnr[2] == aryritSglpnr[2]) && (arylftSglpnr[3] == aryritSglpnr[3]) && flg == "1" && ($("#hdn_rtsplthread").val().indexOf(arylftSglpnr[2]) > -1 && $("#hdn_rtsplthread").val().indexOf(aryritSglpnr[2]) > -1)) { // && (aryritSglpnr[2] != "1A" && aryritSglpnr[2] != "1S") RoundTrip Special Select Process for Same Flights in Roundtrip... (to show comparable fare popup)... by saranraj...
                if (arylftSglpnr[0] == "R" && arylftSglpnr[0] == "R") { //Not-Single PNR... eg for W9, AI... ****Commended by rajesh ****
                    //if (arylftSglpnr[0] == "Y" && arylftSglpnr[0] == "Y") {

                    //ffflg = selectedObj_Depid[0]["data-hdoffflg"];// $('.Travselect_' + Depid)[0].dataset.hdoffflg;
                    //  Inva = selectedObj_Depid[0]["data-hdInva"] + "SpLITSaTIS~" + selectedObj_Rtnid[0]["data-hdInva"]; //For Single Itinary... by Saranraj...
                    //   token = selectedObj_Depid[0]["data-hdtoken"] + "SpLITSaTIS" + selectedObj_Rtnid[0]["data-hdtoken"];

                    ffflg = selectedObj_Depid[0]["data-hdoffflg"] + "~" + selectedObj_Rtnid[0]["data-hdoffflg"];
                    Inva = selectedObj_Depid[0]["data-hdInva"] + "SpLiTWeB" + selectedObj_Rtnid[0]["data-hdInva"];   //For Seperate Itinary... by Saranraj...
                    token = selectedObj_Depid[0]["data-hdtoken"] + "SpLiTWeB" + selectedObj_Rtnid[0]["data-hdtoken"];

                    $("#hdtxt_trip").val("R"); //Roundtrip Special Select from Roundtrip Avail...
                    var Specialflagfare = "Y"; /* Commended by Rajesh for booking problem*/
                    //var Specialflagfare = "";
                    CheckAvail(Fno, Deaprt, Arrive, Inva, token, ffflg, selectedObj_Depid[0]["data-hdbestbuyy"], selectedObj_Depid[0]["data-hdnmarkupfare"], "FALSE", alterqueue, Depid, "0", Specialflagfare); //Roundtrip Spl Req for Roundtrip Avail (Comparable Popup...)
                }
                else { //For Single PNR... eg: YJ...
                    ffflg = selectedObj_Depid[0]["data-hdoffflg"] + "~" + selectedObj_Rtnid[0]["data-hdoffflg"];
                    Inva = selectedObj_Depid[0]["data-hdInva"] + "SpLiTWeB" + selectedObj_Rtnid[0]["data-hdInva"];   //For Seperate Itinary... by Saranraj...
                    token = selectedObj_Depid[0]["data-hdtoken"] + "SpLiTWeB" + selectedObj_Rtnid[0]["data-hdtoken"];
                    $("#hdtxt_trip").val("R"); //Roundtrip Special Select from Roundtrip Avail...
                    var Specialflagfare = "";
                    CheckAvail(Fno, Deaprt, Arrive, Inva, token, ffflg, selectedObj_Depid[0]["data-hdbestbuyy"], selectedObj_Depid[0]["data-hdnmarkupfare"], "FALSE", alterqueue, Depid, "1", Specialflagfare); //Single-PNR for Roundtrip Flight without compart Popup...
                }
            }
            else { //Direct Roundtrip Select Process... by saranraj on 20170617... by Saranraj on 20170617...
                Inva = selectedObj_Depid[0]["data-hdInva"] + "SpLiTWeB" + selectedObj_Rtnid[0]["data-hdInva"];
                token = selectedObj_Depid[0]["data-hdtoken"] + "SpLiTWeB" + selectedObj_Rtnid[0]["data-hdtoken"];
                ffflg = selectedObj_Depid[0]["data-hdoffflg"] + "~" + selectedObj_Rtnid[0]["data-hdoffflg"];
                $("#hdtxt_trip").val("R"); //As usual Roundtrip Select... by Saranraj on 20170617...
                var Specialflagfare = "";
                CheckAvail(Fno, Deaprt, Arrive, Inva, token, ffflg, selectedObj_Depid[0]["data-hdbestbuyy"], selectedObj_Depid[0]["data-hdnmarkupfare"], "FALSE", alterqueue, Depid + "~" + Rtnid, roundtripflg, Specialflagfare);  //Other common direct request...
            }
        }


    }
    catch (e) {
        showerralert(e.message, 5000, "");
        //alert(e.message);
    }
}

var arySelectedFltIds = [];
function SelectMcityFAvail() {

    try {
        var Depid = "";
        var allselect = false;
        arySelectedFltIds = [];
        for (var i = 1; i <= aryOrgMul.length; i++) {
            Depid = $('#selectclickbuttonMCity').data('dep-id_' + i);
            if (typeof Depid === "undefined" || Depid === "") {
                //alert("Please Select flight for " + aryOrgMul[i - 1] + " to " + aryDstMul[i - 1] + ".");
                showerralert("Please Select flight for " + aryOrgMul[i - 1] + " to " + aryDstMul[i - 1] + ".", 5000, "");
                allselect = false;
                return false;
            }
            arySelectedFltIds.push(Depid);
            allselect = true;
        }
        if (allselect) {
            var Fno = $('#FlightID' + arySelectedFltIds[0])[0].innerHTML;  //Wanna check First Flight id enough or not... by saranraj on 20170609...
            var Deaprt = $('#span_depature' + arySelectedFltIds[0])[0].innerHTML;//spanTag[2].textContent;
            var Arrive = $('#span_Arrival' + arySelectedFltIds[0])[0].innerHTML; //spanTag[3].textContent;
            var idss = 0;
            var selectedObj = $.grep(dataproperty_array, function (n, i) {
                return n.datakey == idss;
            });
            var alterqueue = selectedObj[0]["data-hdAlterQueue"];
            var ary_Inva = [];
            var ary_token = [];
            var ary_ffflg = [];
            for (var j = 0; j < arySelectedFltIds.length; j++) {
                var idss = j;
                var selectedObj = $.grep(dataproperty_array, function (n, i) {
                    return n.datakey == arySelectedFltIds[j];
                });

                ary_Inva.push(selectedObj[0]["data-hdInva"]);
                ary_Inva.push(selectedObj[0]["data-hdtoken"]);
                ary_Inva.push(selectedObj[0]["data-hdoffflg"]);
            }
            BeforeMFltSelect_PageShow(Fno, Deaprt, Arrive, ary_Inva, ary_token, ary_ffflg, arySelectedFltIds, "FALSE", alterqueue); //This function for Show Select page with flight details before start Select Process...
        }
    }
    catch (e) {
        showerralert(e.message, 5000, "");
    }
}

function BeforeMFltSelect_PageShow(Fno, Deaprt, Arrive, ary_Inva, ary_token, ary_ffflg, arySelectedFltIds, bestbuyflag, alterqueue) {

    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });

    $("#hdn_allowfarecal").val("2");
    var Class_val = ($("#hdtxt_trip")[0].value == "M" ? $("#grpcmbFlightClass").val() : $("#ddlclass").val());
    var param = {
        aryInva: ary_Inva,
        arytoken: ary_token,
        aryffflg: ary_ffflg,
        Class: Class_val,
    }

    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: domMulb4selecturl, // Location of the service
        data: JSON.stringify(param),
        timeout: 100000,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        async: true,
        success: function (json) {//On Successful service call
            $.unblockUI();
            var result = json;
            if (result != "") {
                if (result.indexOf("disessionExp005") != -1) {
                    window.location.href = sessionExb;
                }
                else if (result.indexOf("availselecteddive") != -1) {
                    $("#dvSelectView").html(json);

                    $("html, body").animate({ scrollTop: 0 }, 600);
                    $("#dvAvailView").css("display", "none");
                    $("#dvSearchArea").css("display", "none");
                    $("#dvSelectView").css("display", "block");

                    setTimeout(function () {
                        CheckMulAvail(Fno, Deaprt, Arrive, ary_Inva, ary_token, ary_ffflg, arySelectedFltIds, bestbuyflag, alterqueue, "0");  //For Temporary purpose by saranraj...
                    }, 200);
                }
                else {
                    $("#dvSelectView").html(json);
                    var errormessage = $("#diverrormsg").html();
                    if (errormessage != null || errormessage != "") {
                        //alert(errormessage);
                        showerralert(errormessage, "", "");
                    } else {
                        errormessage = "Problem occured while select-(#3)"; //Problem in Partial view...
                        //alert(errormessage);
                        showerralert(errormessage, "", "");
                    }
                }
            }
            else {
                //alert("Unable to select. problem occured internally. Please Contact Customer Care.. Error:000");
                showerralert("Unable to select. problem occured internally. Please Contact Customer Care.. Error:000", "", "");
            }
        },
        error: function (e) {//On Successful service call
            $.unblockUI();

            LogDetails(e.responseText, e.status, "Flight Select");
            if (e.statusText == "timeout") {
                //alert("Operation timeout. Please try Again");
                showerralert("Operation timeout. Please try Again.", "", "");
            }
            if (e.status == "500") {
                //alert("Session has been Expired.");
                window.location = sessionExb;
                return false;
            }
        }	// When Service call fails

    });
}
var adtcount = "", chdcount = "", infcount = "", shoqcommitons = "";
var ary_res = [], ary_tkn = [], ary_chang = [];




function CheckMulAvail(Fno, Deaprt, Arrive, ary_Inva, ary_tokenkey, ary_offflg, arySelectedFltIds, bestbuyrequired, alterqueue, RtripComparFlg) {

    multicity_dom = "";
    var InetFlg = checkconnection();
    if (!InetFlg) {
        showerralert("Please check connectivity.", "", "");
        return false;
    }

    var check = "";
    if (document.getElementById("chkNFare").checked) {
        check = "TRUE";
    }
    else {
        check = "FALSE";
    }
    var mob = "FALSE";

    var oldmarkup = "";
    var best = "";

    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });

    var ClientID = "";
    var BranchID = "";
    var GroupID = "";


    if ($("#ddlclient").length > 0) {
        ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
        BranchID = sessionStorage.getItem('branchid') != null ? sessionStorage.getItem('branchid').trim() : "";
        GroupID = sessionStorage.getItem('Groupid') != null ? sessionStorage.getItem('Groupid').trim() : "";
    }
    else {
        ClientID = "";
        BranchID = "";
        GroupID = "";
    }

    var segment_type = $('body').data('segtype');
    var res = 0, res_success = 0;
    ary_res = []; ary_tkn = []; ary_chang = [];
    Other_SSRSPMax = []; Other_bagout = []; Other_SSRPCIN = []; primessr = []; // used in OtherSSR dont delete
    adtcount = ""; chdcount = ""; infcount = "";
    for (var req = 0; req < arySelectedFltIds.length; req++) {
        //$('.Travselect_' + arySelectedFltIds[req])[0].dataset.hdnmarkupfare
        var idss = req;
        var selectedObj = $.grep(dataproperty_array, function (n, i) {
            return n.datakey == arySelectedFltIds[req];
        });
        var selectedObj_multihit = $.grep(ary_Inva, function (n, i) {
            if (n != 0) {
                var ss = n.replace(/(\r\n|\n|\r)/gm, " ");
                if (ss.indexOf("SpLitPResna") >= 0) {
                    return n;
                }
            }


        });

        oldmarkup = selectedObj[0]["data-hdnmarkupfare"];
        best = selectedObj[0]["data-hdbestbuyy"];// $('#hdbestbuyy' + arySelectedFltIds[req]).val();


        $("#hdtxt_origin")[0].value = selectedObj[0]["data-grporgvcitymul"];
        $("#hdtxt_destination")[0].value = selectedObj[0]["data-grpdesvcitymul"];


        var Specialflagfare = "";

        mulreq = "Y";//Multireqst=Y means return as json else return html...
        $("#hdn_allowfarecal").val("2");
        var Class_val = ($("#hdtxt_trip")[0].value == "M" ? $("#grpcmbFlightClass").val() : $("#ddlclass").val());


        if (selectedObj_multihit[req].indexOf("SpLitPResna") >= 0) {
            $.ajax({
                type: "POST", 		//GET or POST or PUT or DELETE verb
                url: selecturl, 		// Location of the service
                data: '{FliNum: "' + Fno + '" ,Deaprt: "' + Deaprt + '" ,Arrive: "' + Arrive + '" ,FullFlag: "' + selectedObj_multihit[req] + '" ,TokenKey: "'
                    + ary_tokenkey[req] + '",Trip: "' + $("#hdtxt_trip")[0].value + '",BaseOrgin: "' + $("#hdtxt_origin")[0].value + '",BaseDestination: "' + $("#hdtxt_destination")[0].value
                    + '",offflg: "' + ary_offflg[req] + '",Class: "' + Class_val + '",TKey: "' + document.getElementById("AvaiText").value + '",DEPTDATE: "' + $("#txtdeparture")[0].value
                    + '",ARRDATE: "' + $("#txtarrivaldate")[0].value + '",Nfarecheck: "' + check
                    + '",oldfaremarkup: "' + oldmarkup + '",mobile:"' + mob + '",bestbuyrequired:"' + bestbuyrequired
                    + '", AlterQueue:"' + alterqueue + '",SegmentType:"' + segment_type + '",Multireqst:"' + mulreq
                    + '",reqcnt:"' + req + '",RtripComparFlg:"' + RtripComparFlg + '",Specialflagfare:"' + Specialflagfare + '",ClientID:"' + ClientID + '",BranchID:"' + BranchID + '",GroupID:"' + GroupID + '", StudentFare:"' + false + '", ArmyFare:"' + false + '", SnrcitizenFare:"' + false + '"}',
                timeout: 100000,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (json) {//On Successful service call

                    $.unblockUI();
                    res++;
                    var result = json; var segment_type = $('body').data('segtype');
                    var earnings_calculate = 0;
                    if (result.status == "-1") {
                        window.location.href = sessionExb;
                    }
                    else if (result.status == "0" || result.status == "00") {
                        $('#sletedfltinSelect_' + (res - 1)).removeClass('opacty');
                        $('#spn_selectinprocess_' + (res - 1)).html('<i class="fa fa-close stas nosucss"></i>');
                        showerralert(result.Message, "", "");
                        $("#btnbacktoavail").click();
                    }
                    else {
                        res_success++;
                        var aryTemp = JSON.parse(result.Result);
                        ary_res.push(aryTemp);
                        ary_tkn.push({ itiref: res, tkn: result.token }); //ItiRef and Token values...
                        multicity_dom += result.ValKey + "|";
                        if (res_success == 1) {
                            adtcount = result.adtcount;
                            chdcount = result.chdcount;
                            infcount = result.infcount;
                            shoqcommitons = result.shoqcommitons;

                            $("#hdnDObMand").val(result.hdnDObMand);
                            $("#hdnPassmand").val(result.hdnPassmand);
                            validate_DOB = result.hdnDObMand;
                            validate_PWD = result.hdnPassmand;
                            $("#hdnPaxval").val(result.Totslpaxcount);
                            $("#HDValKey").val(result.ValKey);
                            hidden_HDValKey = result.ValKey;
                            //added by sri multicity domastic booking purpose
                            $("#hdnqueue").val("N");
                        }

                        if (result.status == "1" || result.status == "01") { //Success...
                            $('#sletedfltinSelect_' + (res - 1)).removeClass('opacty');
                            $('#spn_selectinprocess_' + (res - 1)).html('<i class="fa fa-check stas sucss"></i>');
                        }
                        else if (result.status == "2" || result.status == "02") {
                            ary_chang.push({ org: aryOrgMul[res - 1], des: aryDstMul[res - 1], msg: result.Message }); //ItiRef and Token values...
                            $('#sletedfltinSelect_' + (res - 1)).removeClass('opacty');
                            $('#spn_selectinprocess_' + (res - 1)).html('<i class="fa fa-check stas sucss"></i>');
                        }
                        else if (result.status == "3" || result.status == "03") {
                            ary_chang.push({ org: aryOrgMul[res - 1], des: aryDstMul[res - 1], msg: result.Message }); //ItiRef and Token values...
                            $('#sletedfltinSelect_' + (res - 1)).removeClass('opacty');
                            $('#spn_selectinprocess_' + (res - 1)).html('<i class="fa fa-check stas sucss"></i>');
                        }
                        else {// Not - Success... //it ll not execute forever... but for safty purpose...
                            $('#sletedfltinSelect_' + (res - 1)).removeClass('opacty');
                            $('#spn_selectinprocess_' + (res - 1)).html('<i class="fa fa-close stas nosucss"></i>');
                        }
                        if ($("#hdn_allowgst").val() == "Y")
                        { $(".clsgst").show(); }
                    }

                    if (res == arySelectedFltIds.length) {
                        $(".spn_selectinprocess img").hide();
                        if (ary_chang.length > 0) {
                            $('#div_reverce_content').html('');
                            var s = '';
                            for (var i = 0; i < ary_chang.length; i++) {
                                s += '<h5 class="cls-header" style="margin-bottom: 10px;">' + ary_chang[i].org + " - " + ary_chang[i].des + '</h5>';
                                s += '<span>' + ary_chang[i].msg + '</span>';
                            }
                            $('#div_reverce_content').html(s);

                            $("#modal_frORclass_reverse_confirm").modal({
                                backdrop: 'static',
                                keyboard: false
                            });
                        }
                        else {
                            $('.fareprocessing_select').hide();
                            BindDomMulFares(ary_res);
                        }
                    }
                },
                error: function (e) {//On Successful service call
                    $.unblockUI();
                    LogDetails(e.responseText, e.status, "Flight Select");
                    if (e.statusText == "timeout") {
                        //alert("Operation timeout. Please try Again");
                        showerralert("Operation timeout. Please try Again.", "", "");
                    }
                    if (e.status == "500") {
                        //alert("Session has been Expired.");
                        window.top.location = sessionExb;
                        return false;
                    }
                }	// When Service call fails
            });
        }
        else {

        }
    }
}
function ConfirmSelect() { //Confirmation or Fare or Class reverse popup...
    $("#modal_frORclass_reverse_confirm").modal('hide');
    $('.fareprocessing_select').hide();
    BindDomMulFares(ary_res);
}
function BindDomMulFares(ary) {
    var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;
    if (ary != null && ary.length > 0) {

        var amount = [];
        var taxamount = [], paxtype = [], tot_txbreakup = [], commSplit = [], insentSplit = [], ServSplit = [], Markup = [], aryadult_taxSplitFirst = [], SFTAX = [], SFONGST = [], Serv_fee = [];
        var adtgross = 0, Grsscmk = 0, servamt = 0, Mrkamt = 0, grossAmount = 0, addcomm = 0, addins = 0, totcom = 0, totinst = 0, sercharge = 0, SF = 0, SFGST = 0, Servfee = 0;
        var Tot_net = 0;
        for (var i = 0; i < ary.length; i++) { //Flight loop (depends on segment count...)...
            amount.push(ary[i][0].GrossAmount.split('|')); //Here 0 means first flight coz connecting flt also have same datas...
            taxamount.push(ary[i][0].TotalTaxAmount.split('|'));
            tot_txbreakup.push(ary[i][0].TAXBREAKUP.split('|'));
            commSplit.push(ary[i][0].Commission.split('|'));
            insentSplit.push(ary[i][0].Incentive.split('|'));
            ServSplit.push(ary[i][0].Servicecharge.split('|'));
            Markup.push(ary[i][0].MarkUp.split('|'));
            SFTAX.push(ary[i][0].Sftax.split('|'));
            SFONGST.push(ary[i][0].SFGST.split('|'));
            Serv_fee.push(ary[i][0].Servicefee.split('|'));
            if (i == 0)
                paxtype = ary[0][0].PaxType.split('|');
        }

        for (var i = 0; i < amount.length; i++) {

            adtgross = parseFloat(adtgross) + (parseFloat(amount[i][0]) * Number(adtcount.toString()));
            Grsscmk = parseFloat(adtgross) + (parseFloat(Markup[i][0]) + parseFloat(amount[i][0]) + parseFloat(ServSplit[i][0]));
            servamt = parseFloat(servamt) + (parseFloat(ServSplit[i][0]) * Number(adtcount.toString()));
            Mrkamt = parseFloat(Mrkamt) + (parseFloat(Markup[i][0]) * Number(adtcount.toString()));
            addcomm = parseFloat(addcomm) + (parseFloat(commSplit[i][0]) * Number(adtcount.toString()));
            addins = parseFloat(addins) + (parseFloat(insentSplit[i][0]) * Number(adtcount.toString()));
            SF = parseFloat(SF) + (parseFloat(SFTAX[i][0]) * Number(adtcount.toString()));
            SFGST = parseFloat(SFGST) + (parseFloat(SFONGST[i][0]) * Number(adtcount.toString()));
            Servfee = parseFloat(Servfee) + (parseFloat(Serv_fee[i][0]) * Number(adtcount.toString()));

        }
        grossAmount = Number(parseFloat(adtgross) + parseFloat(servamt) + parseFloat(Mrkamt) + parseFloat(SF) + parseFloat(SFGST) + parseFloat(Servfee)).toFixed(decimalflag);
        totcom = addcomm;
        totinst = addins;

        var s = '';
        s += '<div class="panel-body">';
        s += '<ul class="list-unstyled list-real-estate">';

        if (paxtype != null) {
            var TotFr = 0;

            var paxID = "", paxtoggle = "", paxclickId = "";
            for (var pax = 0; pax < paxtype.length; pax++) {
                s += '<li>';

                aryadult_taxSplitFirst = [];
                var GrssFr = 0, SCFr = 0, SerChrg = 0, RSFTAX = 0, RSFGST = 0, Rserfee = 0, Netfare = 0;
                var getpaxtype = paxtype[pax] == "ADT" ? "Adult Fare" : paxtype[pax] == "INF" ? "Infant Fare" : "Child Fare";
                var countOfpax = paxtype[pax] == "ADT" ? adtcount : paxtype[pax] == "INF" ? infcount : chdcount;
                paxtoggle = paxtype[pax] == "ADT" ? "adultfares_adulttoggle" : paxtype[pax] == "INF" ? "infantfares_childttoggle" : "childfares_childttoggle";
                paxID = paxtype[pax] == "ADT" ? "adultfares" : paxtype[pax] == "INF" ? "infantfares" : "childfares";
                paxclickId = paxtype[pax] == "ADT" ? "htoggle_adultfare" : paxtype[pax] == "INF" ? "htoggle_infantfare" : "htoggle_childefare";


                for (var i = 0; i < amount.length; i++) {
                    GrssFr = Number(parseFloat(GrssFr) + parseFloat(Markup[i][pax]) + parseFloat(amount[i][pax]) + parseFloat(ServSplit[i][pax]) + parseFloat(SFTAX[i][pax]) + parseFloat(SFONGST[i][pax]) + parseFloat(Serv_fee[i][pax])).toFixed(decimalflag);
                    Netfare = Number(parseFloat(Netfare) + parseFloat(Markup[i][pax]) + parseFloat(amount[i][pax]) + parseFloat(SFTAX[i][pax]) + parseFloat(SFONGST[i][pax]) + parseFloat(Serv_fee[i][pax])).toFixed(decimalflag);
                    SCFr = Number(parseFloat(SCFr) + parseFloat(Markup[i][pax])).toFixed(decimalflag);
                    SerChrg = Number(parseFloat(SerChrg) + parseFloat(ServSplit[i][pax])).toFixed(decimalflag);
                    RSFTAX = Number(parseFloat(RSFTAX) + parseFloat(SFTAX[i][pax])).toFixed(decimalflag);
                    RSFGST = Number(parseFloat(RSFGST) + parseFloat(SFONGST[i][pax])).toFixed(decimalflag);
                    Rserfee = Number(parseFloat(Rserfee) + parseFloat(Serv_fee[i][pax])).toFixed(decimalflag);
                    aryadult_taxSplitFirst.push(tot_txbreakup[i][pax].split('/'));
                }
                TotFr = Number(TotFr + (GrssFr * countOfpax)).toFixed(decimalflag);
                Tot_net = Number(parseFloat(Netfare * countOfpax) - parseFloat(totcom)).toFixed(decimalflag);
                s += '<div style="cursor: pointer;">';
                s += '<div class="pull-left fnt-wt" style="width: 100%;">';
                s += '<h3 id="' + paxclickId + '" data-faretoggel="' + paxtoggle + '" style="margin: 0 0 5px;" onclick="Faretoggel(this.id)">';
                s += '<span class="commFare">' + getpaxtype + '</span>';
                s += '<i class="ion-chevron-down pull-right" id="adulttoggle" style="float: inherit !important;"></i>';
                s += '<span class="spanshow" style="float: right;">' + Number((parseFloat(GrssFr) * countOfpax)).toFixed(decimalflag) + '</span>';
                s += '</h3>';
                s += '</div>';

                s += '<div class="row row5 slide-slow-down" id="' + paxID + '" style="display: none;">';
                s += '<div class="col-lg-12 col-xs-12 " style="border: 1px solid #ddd;">';
                s += '<h5 style="text-align: center; background: #e6e5e5; margin-top: 5px; padding: 5px 0px 5px 0px;">Fare Details</h5>';

                var ary_TotTax = [];
                var tot_adultFARE = "";
                var yesflag = false;
                for (var ss = 0; ss < aryadult_taxSplitFirst.length; ss++) { //Flight Count loop...
                    var adult_taxSplitFirst = aryadult_taxSplitFirst[ss];

                    for (var tax = 0; tax < adult_taxSplitFirst.length; tax++) { // BreakUp list loop...
                        tot_adultFARE = "";
                        tot_adultFARE = adult_taxSplitFirst[tax].split(':');

                        yesflag = false;
                        for (var taxsum = 0; taxsum < ary_TotTax.length; taxsum++) { // Common array for calculated breakup loop...
                            if (ary_TotTax[taxsum].code == tot_adultFARE[0]) { //Wanna change this area...
                                ary_TotTax[taxsum].val += parseInt(tot_adultFARE[1]);
                                yesflag = true;
                            }
                        }
                        if (!yesflag) {
                            ary_TotTax.push({ code: tot_adultFARE[0], val: parseInt(tot_adultFARE[1]) });
                        }
                    }
                }

                for (var breakup = 0; breakup < ary_TotTax.length; breakup++) {
                    s += '<div class="row row5" style="">';
                    s += '<div class="col-lg-4 col-xs-4"> <span class="spanshow font-13">' + ary_TotTax[breakup].code + '</span> </div>';
                    s += '<div class="col-lg-4 col-xs-4 col0 fare-text-right"> <span class="spanshow font-13">' + ary_TotTax[breakup].val + " x " + countOfpax + '</span> </div>';
                    s += '<div class="col-lg-4 col-xs-4 fare-text-right"> <span class="spanshow font-13">' + Number((parseFloat(ary_TotTax[breakup].val) * Number(countOfpax.toString()))).toFixed(decimalflag) + '</span> </div>';
                    s += '</div>';
                    s += '<hr style="border-top: 1px dashed #DDD !important; margin-top: 5px; margin-bottom: 5px;">';
                    s += '';
                }

                if (Markup != null && Markup.length > pax && Markup[pax].toString() != "0" && Markup[pax].toString() != "0.00") {
                    s += '<div class="row row5" style="">';
                    s += '<div class="col-lg-4 col-xs-4"> <span class="spanshow font-13">SC</span> </div>';
                    s += '<div class="col-lg-4 col-xs-4 col0 fare-text-right"> <span class="spanshow font-13">' + SCFr + " x " + countOfpax + '</span> </div>';
                    s += '<div class="col-lg-4 col-xs-4 fare-text-right"> <span class="spanshow font-13">' + ((SCFr) * Number(countOfpax.toString())) + '</span> </div>';
                    s += '</div>';
                    s += '<hr style="border-top: 1px dashed #DDD !important; margin-top: 5px; margin-bottom: 5px;">';
                }
                if (SFTAX != null && SFTAX.length > pax && SFTAX[pax].toString() != "0" && SFTAX[pax].toString() != "0.00") {
                    s += '<div class="row row5" style="">';
                    s += '<div class="col-lg-4 col-xs-4"> <span class="spanshow font-13">SF</span> </div>';
                    s += '<div class="col-lg-4 col-xs-4 col0 fare-text-right"> <span class="spanshow font-13">' + RSFTAX + " x " + countOfpax + '</span> </div>';
                    s += '<div class="col-lg-4 col-xs-4 fare-text-right"> <span class="spanshow font-13">' + ((RSFTAX) * Number(countOfpax.toString())) + '</span> </div>';
                    s += '</div>';
                    s += '<hr style="border-top: 1px dashed #DDD !important; margin-top: 5px; margin-bottom: 5px;">';
                }

                if (SFONGST != null && SFONGST.length > pax && SFONGST[pax].toString() != "0" && SFONGST[pax].toString() != "0.00") {
                    s += '<div class="row row5" style="">';
                    s += '<div class="col-lg-4 col-xs-4"> <span class="spanshow font-13">SFGST</span> </div>';
                    s += '<div class="col-lg-4 col-xs-4 col0 fare-text-right"> <span class="spanshow font-13">' + RSFGST + " x " + countOfpax + '</span> </div>';
                    s += '<div class="col-lg-4 col-xs-4 fare-text-right"> <span class="spanshow font-13">' + ((RSFGST) * Number(countOfpax.toString())) + '</span> </div>';
                    s += '</div>';
                    s += '<hr style="border-top: 1px dashed #DDD !important; margin-top: 5px; margin-bottom: 5px;">';
                }
                if (Serv_fee != null && Serv_fee.length > pax && Serv_fee[pax].toString() != "0" && Serv_fee[pax].toString() != "0.00") {
                    s += '<div class="row row5" style="">';
                    s += '<div class="col-lg-4 col-xs-4"> <span class="spanshow font-13">Service&nbsp;Fee</span> </div>';
                    s += '<div class="col-lg-4 col-xs-4 col0 fare-text-right"> <span class="spanshow font-13">' + Rserfee + " x " + countOfpax + '</span> </div>';
                    s += '<div class="col-lg-4 col-xs-4 fare-text-right"> <span class="spanshow font-13">' + ((Rserfee) * Number(countOfpax.toString())) + '</span> </div>';
                    s += '</div>';
                    s += '<hr style="border-top: 1px dashed #DDD !important; margin-top: 5px; margin-bottom: 5px;">';
                }

                if (ServSplit != null && ServSplit.length > pax && ServSplit[pax].toString() != "0" && ServSplit[pax].toString() != "0.00") {
                    sercharge += SerChrg * Number(countOfpax.toString());
                    s += '<div class="row row5" style="">';
                    s += '<div class="col-lg-4 col-xs-4"> <span class="spanshow font-13">Management&nbsp;Fee</span> </div>';
                    s += '<div class="col-lg-4 col-xs-4 col0 fare-text-right"> <span class="spanshow font-13">' + SerChrg + " x " + countOfpax + '</span> </div>';
                    s += '<div class="col-lg-4 col-xs-4 fare-text-right"> <span class="spanshow font-13">' + ((SerChrg) * Number(countOfpax.toString())) + '</span> </div>';
                    s += '</div>';
                    s += '<hr style="border-top: 1px dashed #DDD !important; margin-top: 5px; margin-bottom: 5px;">';
                }

                s += '<div class="row row5" style="">';
                s += '<div class="col-lg-4 col-xs-4"> <span class="spanshow fnt-wt-600 font-13">Gross&nbsp;Amount</span> </div>';
                s += '<div class="col-lg-4 col-xs-4 col0 fare-text-right"> <span class="spanshow fnt-wt-600 font-13">' + GrssFr + " x " + countOfpax + '</span> </div>';
                s += '<div class="col-lg-4 col-xs-4 fare-text-right"> <span class="spanshow fnt-wt-600 font-13">' + (parseFloat(GrssFr) * countOfpax) + '</span> </div>';
                s += '</div>';
                s += '<hr style="border-top: 1px dashed #DDD !important; margin-top: 5px; margin-bottom: 5px;">';

                s += '</div> </div>';

                s += '<div class="clearfix"></div>';
                s += '</div>';
                s += '</li>';
            }
        }
        TotFr = Number(TotFr).toFixed(decimalflag);
        s += '<li>';
        s += '<div style="cursor: pointer;">';
        s += '<div class="pull-left" style="width: 100%;">';
        s += '<h3 id="htoggle_mealfare" style="margin: 0 0 5px;"> <span class="commFare">Meal Fare</span>';
        s += '<span class="spanshow" style="float: right;"> <span id="mealamount">0</span> <i class="ion-chevron-down pull-right" id="inmealtoggle" style="float: right; display: none;"></i> </span>';
        s += '</h3>';
        s += '</div> <div class="clearfix"></div>';
        s += '</div>';
        s += '</li>';

        s += '<li>';
        s += '<div style="cursor: pointer;">';
        s += '<div class="pull-left" style="width: 100%;">';
        s += '<h3 id="htoggle_baggagefare" style="margin: 0 0 5px;"> <span class="commFare">Baggage Fare</span>';
        s += '<span class="spanshow" style="float: right;"> <span id="BaggageAmnt">0</span> <i class="ion-chevron-down pull-right" id="baggagetoggle" style="float: right; display: none;"></i> </span>';
        s += '</h3>';
        s += '</div> <div class="clearfix"></div>';
        s += '</div>';
        s += '</li>';

        s += '<li>';
        s += '<div style="cursor: pointer;">';
        s += '<div class="pull-left" style="width: 100%;">';
        s += '<h3 id="htoggle_seatsfare" style="margin: 0 0 5px;"> <span class="commFare">Seat Selection Fare</span>';
        s += '<span class="spanshow" style="float: right;"> <span id="seatamount">0</span> <i class="ion-chevron-down pull-right" id="seatstoggle" style="float: right; display: none;"></i> </span>';
        s += '</h3>';
        s += '</div> <div class="clearfix"></div>';
        s += '</div>';
        s += '</li>';


        s += '<li>';
        s += '<div style="cursor: pointer;">';
        s += '<div class="pull-left" style="width: 100%;">';
        s += '<h3 id="htoggle_totalfare" style="margin: 0 0 5px;"> <span class="commFare">Total Fare</span>';
        s += '<span class="spanshow" style="float: right;" id="Li_Totalfare">' + TotFr + '<i class="ion-chevron-down pull-right" id="totaltoggle" style="float: right; display: none;"></i> </span>';
        s += '</h3>';
        s += '</div> <div class="clearfix"></div>';
        s += '</div>';
        s += '</li>';

        if (totcom != null && ($("#hdn_AppHosting").val() != "BSA")) {
            s += '<li>';
            s += '<div style="cursor: pointer;">';
            s += '<div class="pull-left" style="width: 100%;">';
            s += '<h3 id="htoggle_totalfare" style="margin: 0 0 5px;"> <span class="commFare">Earnings</span>';
            s += '<span class="spanshow" style="float: right;" id="totalserviceAmnt">' + totcom + '<i class="ion-chevron-down pull-right" id="totaltoggle" style="float: right; display: none;"></i> </span>';
            s += '</h3>';
            s += '</div> <div class="clearfix"></div>';
            s += '</div>';
            s += '</li>';
        }
        s += '</ul> </div>';
        $('#dvinnerFareDetinSelect').html(s);

        $('#totalAmnt,#spnFare').html(Number(TotFr).toFixed(decimalflag));
        $("#totalAmnt").data("withoutservicecharge", Number(TotFr).toFixed(decimalflag));

        hidden_hdntotalserv = sercharge;
        $("#hdntotalserv").val(Number(sercharge).toFixed(decimalflag));
        $(".riyatcktamt").html(Number(TotFr).toFixed(decimalflag));
        $(".riyatotal").html(Number(TotFr).toFixed(decimalflag));
        $('.sele_btnhideshow').show(); //Book and Block Buttons showing...
        $("#netframt")["0"].innerHTML = Tot_net;
        $("#aircommamt")["0"].innerHTML = totcom;
    }
}
//#endregion
//#region Check Avail
function CheckAvail(Fno, Deaprt, Arrive, Inva, tokenkey, offflg, best, oldmarkup, bestbuyrequired, alterqueue, Id, RtripComparFlg, Specialflagfare) {

    var ClientID = "";
    var BranchID = "";
    var GroupID = "";
    //To Check Internet connection is available... by saranraj on 20170725...
    var InetFlg = checkconnection();
    if (!InetFlg) {
        showerralert("Please check connectivity.", "", "");
        return false;
    }
    //End...


    if ($("#ddlclient").length > 0) {
        ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
        BranchID = sessionStorage.getItem('branchid') != null ? sessionStorage.getItem('branchid').trim() : "";
        GroupID = sessionStorage.getItem('Groupid') != null ? sessionStorage.getItem('Groupid').trim() : "";
    }
    else {

        ClientID = "";
        BranchID = "";
        GroupID = "";
    }


    var check = "";
    if (document.getElementById("chkNFare").checked) {
        check = "TRUE";
    }
    else {
        check = "FALSE";
    }
    var mob = "FALSE";

    if ($("#hdn_producttype").val() == "DEIRA") {
        debugger
        if (strProcessingImage.indexOf("loader") != -1) {
        $.blockUI({
                message: '<img alt="Please Wait..." src="' + strProcessingImage + '" style="background-color:#fff; border-radius:8px; margin-top:12%;width:150px;" id="' + selectloadingId + '" />',
            css: { padding: '5px' }
        });
        } else {

            var img = new Image();
            img.src = strProcessingImage;
            img.onload = function () {
                var ImageWidth = this.width;
                $.blockUI({
                    message: '<div class="avail_loader"><div class="avail_img"><img alt="Please Wait..." src="' + strProcessingImage + '" style="margin-top:8%;" id="' + selectloadingId + '" /><div class="load_btm" style="width:' + ImageWidth + 'px"><img src="../../Images/Deira/process_loader.gif"/></div></div></div>',
                    css: { padding: '5px' }
                });
            };
        }
    } else {
        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;width:150px;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });
    }
    
    var segment_type = $('body').data('segtype');
    mulreq = "N"; //Multireqst=N means return as html else return json...
    $("#hdn_allowfarecal").val("2");
    // var Class_val = ($("#hdtxt_trip")[0].value == "M" ? $("#grpcmbFlightClass").val() : $("#ddlclass").val());

    var Class_val = "";
    if (FLAG != "M") {
        Class_val = ($("#hdtxt_trip")[0].value == "M" ? $("#grpcmbFlightClass").val() : $("#ddlclass").val());
    }
    else {
        Class_val = MobileClass;
    }

    //var Class_val = ($("#hdtxt_trip")[0].value == "M" ? $("#grpcmbFlightClass").val() : Class);//var Class_val = ($("#hdtxt_trip")[0].value == "M" ? $("#grpcmbFlightClass").val() : $("#ddlclass").val());

    var StdFare = $("#StudentFare").length > 0 ? $("#StudentFare")[0].checked : false;
    var ArmyFare = $("#ArmyFare").length > 0 ? $("#ArmyFare")[0].checked : false;
    var SnrcitizenFare = $("#SrCitizenFare").length > 0 ? $("#SrCitizenFare")[0].checked : false;

    var param = {
        FliNum: Fno, Deaprt: Deaprt, Arrive: Arrive, FullFlag: Inva, TokenKey: tokenkey, Trip: $("#hdtxt_trip")[0].value, BaseOrgin: $("#hdtxt_origin")[0].value,
        BaseDestination: $("#hdtxt_destination")[0].value, offflg: offflg, Class: Class_val, TKey: document.getElementById("AvaiText").value,
        //DEPTDATE: $("#txtdeparture")[0].value,
        //ARRDATE: $("#txtarrivaldate")[0].value,

        DEPTDATE: FLAG == "M" ? departuredate : $("#txtdeparture").val(),
        ARRDATE: FLAG == "M" ? arrivaldate : $("#txtarrivaldate").val(),

        Nfarecheck: check, oldfaremarkup: oldmarkup, mobile: mob, bestbuyrequired: bestbuyrequired,
        AlterQueue: alterqueue, SegmentType: segment_type, Multireqst: mulreq, reqcnt: "0", RtripComparFlg: RtripComparFlg, Specialflagfare: Specialflagfare,
        ClientID: ClientID, BranchID: BranchID, GroupID: GroupID, StudentFare: StdFare, ArmyFare: ArmyFare, SnrcitizenFare: SnrcitizenFare
    };
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: selecturl, 		// Location of the service
        data: JSON.stringify(param),
        timeout: 100000,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        async: true,
        success: function (json) {//On Successful service call
            $.unblockUI();
            $(".Bkpaymentmode").prop("disabled", false);
            sessionStorage.setItem("CHECKREBOOK", "NO");
            var result = json;
            var earnings_calculate = 0;
            if ($("#Mulrd_Booklet").is(":checked") == true) {
                $('body').data('segtype', "D");
            }
            if (result != "") {
                if (result.indexOf("disessionExp005") != -1) {
                    window.location.href = sessionExb;
                }
                if (result.indexOf("availselecteddive") != -1) {
                    GrossTotM = 0;
                    grossbagg = 0;
                    $("#dvSelectView").html(json);
                    var aryids = Id.toString().split('~');
                    var i = 0;
                    $('.FlightTip').tooltipster({
                        contentAsHTML: true,
                        theme: 'tooltipster-punk',
                        animation: 'grow',
                        position: 'right',
                    });

                    if (RtripComparFlg == "0") { //Comparable Popup...
                        var Depid = $('#selectclickbuttonRTrip').data('dep-id');
                        var Rtnid = $('#selectclickbuttonRTrip').data('ret-id');

                        var RtrpSplFlt = $('#hdnRtrpSplFltfrmRtrp').val();
                        var myarys = JSON.parse(RtrpSplFlt);
                        myarys = JSON.parse(myarys);

                        showComparisionPop(Depid, Rtnid, myarys);
                    }
                    else {
                        var statuscode = $("#divstatuscode").html();
                        var displayalertmsg = ""; //To show two msg  STS185
                        var alertmsg = $("#divstatuscode1").data("returnmessage");
                        if (alertmsg != null && alertmsg != "") {
                            displayalertmsg = alertmsg;
                        }
                        if (statuscode != null && (statuscode == "2" || statuscode == "02" || statuscode == "3" || statuscode == "03") && RtripComparFlg != "0") {  //RtripComparFlg- For Roundtrp Spl Select from Roundtrp Avail no need to show reverse pupup's...
                            var alert_message = $("#divstatuscode").data("returnmessage");
                            var Displayalert = (alert_message != null && alert_message != "") ? alert_message : "";
                            displayalertmsg += "<b>" + Displayalert + "</b>";
                        }

                        var Gststatuscode = $("#divgst_statuscode").html(); //Added for GST Manadatory purposse 
                        $("#allow_gst").prop('checked', false);
                        $("#allow_gst").prop('disabled', false);
                        var selectpopup = $("#divgst_statuscode").data("returnmessage");
                        if (Gststatuscode != null && (Gststatuscode == "04" || Gststatuscode == "4") && RtripComparFlg != "0") { //STS185
                            $("#allow_gst").prop('checked', true);
                            $("#allow_gst").prop('disabled', true);
                            Allowgst();
                        }
                        if (selectpopup != "" && selectpopup != null) {
                            if (displayalertmsg != "")
                                displayalertmsg += "</br>" + selectpopup;
                            else
                                displayalertmsg += selectpopup;
                        }

                        if (displayalertmsg != "" && displayalertmsg != null) {
                            $('#pfrConfirmMsg').html(displayalertmsg);
                            $("#modal_fr_confirm").modal({
                                backdrop: 'static',
                                keyboard: false
                            });
                            return false;
                        }
                        RedirectToSelect();
                    }

                }
                else {

                    if (RtripComparFlg == "0") { //Dont show any errormsg for Roundtrip Spl Selct process from RoundTrip Avail... just call Roundtrip Selct process....
                        SelectRtripFAvail("0"); //0-No need to check Roundtrip Special Select process from Roundtrip Avail... 1- check it... by saranraj on 20170619...
                        return false;
                    }

                    $("#dvSelectView").html(json);


                    var errormessage = $("#diverrormsg").html();
                    if (errormessage != null || errormessage != "") {
                        showerralert(errormessage, "", "");

                    } else {
                        errormessage = "Selected flight not available-(#03)";
                        showerralert(errormessage, "", "");
                    }

                }
            }
            else if (result = "") {
                showerralert("Unable to select. problem occured internally. Please Contact Customer Care. (#09).", "", "");
            }

        },
        error: function (e) {//On Successful service call
            $.unblockUI();
            if (e.statusText == "timeout") {
                showerralert("Operation timeout. Please try Again.", "", "");
            }
            if (e.status == "500") {
                window.location = sessionExb;
                return false;
            }
        }	// When Service call fails

    });
}

$('#btnContinueFrChang').click(function () {
    $("#modal_fr_confirm").modal("hide");
    RedirectToSelect();
});

$('#btnContinueChange').click(function () {
    $("#modal_confirm").modal("hide");
    RedirectToSelect();
});

$('#agreeroundfare').click(function () {
    $("#modal_rsp_confirm").modal("hide");
    RedirectToSelect();
});

function Bestbuycheck(stu) {

    if (validatebestbuy()) {

        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });


        $("#diverrordetails").html('');
        var oldgrossamount = $("#totalAmnt")[0].innerHTML;
        var mealamount = $("#mealamount")[0].innerHTML;
        var BaggageAmnt = $("#BaggageAmnt")[0].innerHTML;
        var seatamount = $("#seatamount")[0].innerHTML;
        var Li_Totalfare = $("#Li_Totalfare")[0].innerHTML;

        var param = {
            oldgrossamount: oldgrossamount, mealamount: mealamount, BaggageAmnt: BaggageAmnt, seatamount: seatamount, Li_Totalfare: Li_Totalfare
        };

        $.ajax({
            type: "POST", 		//GET or POST or PUT or DELETE verb
            url: bestbuyurl,// Location of the service data: JSON.stringify(param),
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (json) {//On Successful service call
                $.unblockUI();

                var result = json;
                var earnings_calculate = 0;
                if (result != "") {
                    if (result.indexOf("disessionExp005") != -1) {
                        window.location.href = sessionExb;
                    }
                    if (result.indexOf("availselecteddive") != -1) {
                        $("#dvSelectView").html(json);
                        $("#farelbl")[0].innerHTML = "Best&nbsp;Buy&nbsp;Fare";
                        $("#ruletype")[0].innerHTML = "Best&nbsp;Buy";
                        $("#bbruleheader")[0].innerHTML = "Best&nbsp;Buy&nbsp;Rules";
                        $("#booking_type").val("BB");
                        $("#BB_fareheader").css("display", "block");
                        $("#dvinnerFareDetinBestbuy").css("display", "block");
                        $("#btn_block").css("display", "block");
                        $("#btn_book").css("display", "block");
                        $("#best_buy").css("display", "none");
                        var statuscode = $("#divstatuscode").html();
                        if (statuscode != null && (statuscode == "2" || statuscode == "02" || statuscode == "3" || statuscode == "03")) {
                            var alert_message = $("#divstatuscode").data("returnmessage");
                            showerralert(alert_message, "", "");
                        }
                    }
                    else {
                        $("#diverrordetails").html(json);
                        var errormessage = $("#diverrormsg").html();
                        if (errormessage != null || errormessage != "") {
                            showerralert(errormessage, "", "");
                        } else {
                            errormessage = "Selected flight not available-(#03)";
                            showerralert(errormessage, "", "");
                        }
                    }
                }
                $.unblockUI();
            },
            error: function (e) {//On Successful service call  
                $.unblockUI();
                console.log("Internal Problem occured while insert logout details. (#07).");
                //Do Sutff when we need...
            }	// When Service call fails
        });
    }
}

function validatebestbuy() {

    var email = $('#txtEmailID');
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    if ($('#txtContactNo').val().trim() == "" && $('#txtEmailID').val().trim() == "") {
        showerralert("Please Enter Contact No. (or) Email Id.", "", "");
        $("#txtContactNo").focus();
        return;
    }
    if ($('#txtContactNo').val().trim() == "") {
        showerralert("Please Enter Contact Number.", "", "");
        $("#txtContactNo").focus();
        return;
    }
    if ($('#txtEmailID').val().trim() == "" && BookingcoordinatorDet.toUpperCase() != "Y") {
        showerralert("Please Enter  email address.", "", "");
        $("#txtEmailID").focus();
        return;
    } else if (($('#txtTitle').val() == null || $('#txtTitle').val() == "") && BookingcoordinatorDet.toUpperCase() != "Y") {
        showerralert("Please select the Title.", "", "");
        return;
        $('#txtTitle').focus();
    }

    else if ($('#txtFirstName').val().trim() == "" && BookingcoordinatorDet.toUpperCase() != "Y") {
        showerralert("Please Enter the First Name.", "", "");
        $("#txtFirstName").focus();
        return;
    }
    else if ($('#txtLastName').val().trim() == "" && BookingcoordinatorDet.toUpperCase() != "Y") {
        showerralert("Please Enter the Last name.", "", "");
        $("#txtLastName").focus();
        return;
    }
    else if (($('#Txt_Emirates').val() == null || $('#Txt_Emirates').val() == "") && BookingcoordinatorDet.toUpperCase() != "Y") {
        showerralert("Please Select the " + $("#hdn_statetitle")[0].value, "", "");
        $("#Txt_Emirates").focus();
        return;
    }

    else if (email.val() != "") {
        if (filter.test(email.val()) == false) {
            showerralert("Please provide a valid email address.", "", "");
            email.focus();
            return;
        }
    }

    var Contactlenght = document.getElementById("txtContactNo").value;
    var Agnmoblenght = document.getElementById("txt_AgnNo").value;
    var Homemoblenght = document.getElementById("txt_HomeNo").value;
    var Bussmoblenght = document.getElementById("txt_BusiNo").value;
    if (Contactlenght.length > 12 || Agnmoblenght.length > 12 || Homemoblenght.length > 12 || Bussmoblenght.length > 12) {
        showerralert("Mobile No. exceed with 12 digits.Please enter valid Mobile No.(eg : 056XXXXX05)", "", "");
        return;
    }
    else if (Contactlenght.length != "" || Agnmoblenght.length != "" || Homemoblenght.length != "" || Bussmoblenght.length != "") {
        if (Contactlenght.length != "10" && Agnmoblenght.length != "10" && Homemoblenght.length != "10" && Bussmoblenght.length != "10") {
            showerralert("Please enter any one 10 digits Contact Mobile No. (eg : 056XXXXX05)", "", "");
            return;
        }
    }
    document.getElementById('hdnPax').value = "";
    var par = $("#passengersdetails .commpaxcls");
    var InfantRefrence = new Array();
    var InfantRef = "";
    var Ids_split = "";
    var paty = "";

    var totalpaxdetails = "";
    var TotalPax = "";
    var FirstPax = "";
    var pasIds = "";
    var paxtype = "";
    var title = "";
    var firstName = "";
    var lastName = "";
    var gender = "";
    var passportNo = "";
    var DOB = "";
    var pass_ex_date = "";
    var i = 0;
    var checkflag = false;
    var adtcheckPendingflag = "";
    var chdcheckPendingflag = "";
    var infcheckPendingflag = "";

    var pax_type_det = "";
    var Paxtyp = "";
    var paxtypcnt = 0;

    $("#passengersdetails .commAdtpaxcls").each(function () {

        i++;
        paxtypcnt++;
        Ids_split = this.id.split("_")[1];
        pax_type_det = $("#ad_type_" + Ids_split).data("paxvalue").split(':')[0];

        paty = $("#ad_type_" + Ids_split).data("paxvalue").split(':')[0];
        if ($("#ad_Title_" + Ids_split).val() == null || $("#ad_Title_" + Ids_split).val() == "0") {
            showerralert("Please Enter Adult" + paxtypcnt + " Title.", "", "");
            $("#ad_Title_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if ($("#ad_firstName_" + Ids_split).val() == null || $("#ad_firstName_" + Ids_split).val().trim() == "") {
            showerralert("Please Enter Adult" + paxtypcnt + " First name.", "", "");
            $("#ad_firstName_" + Ids_split).val("");
            $("#ad_firstName_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if ($("#ad_lastName_" + Ids_split).val() == null || $("#ad_lastName_" + Ids_split).val().trim() == "") {
            showerralert("Please Enter Adult" + paxtypcnt + " Last Name.", "", "");
            $("#ad_lastName_" + Ids_split).val("");
            $("#ad_lastName_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if ($("#ad_gender_" + Ids_split).val() == null || $("#ad_gender_" + Ids_split).val().trim() == "" || $("#ad_gender_" + Ids_split + ' option:selected').text().trim() == "Select") {
            showerralert("Please Select Adult" + paxtypcnt + " Gender.", "", "");
            $("#ad_gender_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if (($("#ad_DOB_" + Ids_split).val() == null || $("#ad_DOB_" + Ids_split).val().trim() == "") && validate_DOB == "TRUE" || (($("#ad_passportNo_" + Ids_split).val() == null || $("#ad_passportNo_" + Ids_split).val().trim() == "") && $("#ad_passportNo_" + Ids_split).val().trim() != "")) {
            showerralert("Please Enter the " + paty + paxtypcnt + " DOB.", "", "");
            $("#ad_DOB_" + Ids_split).val("");
            $("#ad_DOB_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if ($("#ad_DOB_" + Ids_split).val() != null && $("#ad_DOB_" + Ids_split).val() != "") {
            Paxtyp = Calculateage($("#ad_DOB_" + Ids_split).val(), "ADULT");
            if (Paxtyp == false) {
                showerralert("Please Enter the valid " + paty + paxtypcnt + " DOB.", "", "");
                $("#ad_DOB_" + Ids_split).val("");
                $("#ad_DOB_" + Ids_split).focus();
                checkflag = true;
                return;
            }
        }

        if (($("#ad_passportNo_" + Ids_split).val() == null || $("#ad_passportNo_" + Ids_split).val().trim() == "") && validate_PWD == "TRUE") {
            showerralert("Please Enter the " + paty + paxtypcnt + " Passport No.", "", "");
            $("#ad_passportNo_" + Ids_split).val("");
            $("#ad_passportNo_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if (($("#ad_pass_ex_date_" + Ids_split).val() == null || $("#ad_pass_ex_date_" + Ids_split).val().trim() == "") && validate_PWD == "TRUE" || (($("#ad_pass_ex_date_" + Ids_split).val() == null || $("#ad_pass_ex_date_" + Ids_split).val().trim() == "") && $("#ad_passportNo_" + Ids_split).val().trim() != "")) {
            showerralert("Please Enter the " + paty + paxtypcnt + "  PassPort Exp. date.", "", "");
            $("#ad_pass_ex_date_" + Ids_split).val("");
            $("#ad_pass_ex_date_" + Ids_split).focus();
            checkflag = true;
            return;
        }

        if ($("#ad_pass_ex_date_" + Ids_split).val() != null && $("#ad_pass_ex_date_" + Ids_split).val() != "") {
            var ppexpdate = PassportExp($("#ad_pass_ex_date_" + Ids_split).val());
            if (ppexpdate != true) {
                showerralert("Please Enter the valid " + paty + paxtypcnt + " PassPort Exp. date.", "", "");
                $("#ad_pass_ex_date_" + Ids_split).val("");
                $("#ad_pass_ex_date_" + Ids_split).focus();
                checkflag = true;
                return;
            }
        }

        if (($("#ad_Cntry_" + Ids_split).val() == null || $("#ad_Cntry_" + Ids_split).val().trim() == "" || $("#ad_Cntry_" + Ids_split).val().trim() == "0") && validate_PWD == "TRUE" && $("#ad_passportNo_" + Ids_split).val().trim() != "") {
            showerralert("Please Select the " + paty + paxtypcnt + "  Passport Issued Country.", "", "");
            $("#ad_Cntry_" + Ids_split).focus();
            checkflag = true;
            return;
        }

        var SSR = ""
        if ($("#hdnadtMeals_" + Ids_split).val() != null && $("#hdnadtMeals_" + Ids_split).val().trim() != "") {
            SSR = $("#hdnadtMeals_" + Ids_split).val();
        }
        paxtype = paty;
        title = $("#ad_Title_" + Ids_split).val() != null ? $("#ad_Title_" + Ids_split).val().trim() : "";
        firstName = $("#ad_firstName_" + Ids_split).val().trim();
        lastName = $("#ad_lastName_" + Ids_split).val().trim();
        gender = $("#ad_gender_" + Ids_split).val().trim();
        DOB = $("#ad_DOB_" + Ids_split).val().trim();
        passportNo = $("#ad_passportNo_" + Ids_split).val().trim();
        pass_ex_date = $("#ad_pass_ex_date_" + Ids_split).val().trim();
        IssCon = $("#ad_Cntry_" + Ids_split).val();
        SSR = SSR;

        mealbaggapreifor(SSR, Ids_split, "Adult");
        TotalPax = title + "SPLITSCRIPT" + firstName + "SPLITSCRIPT" + lastName + "SPLITSCRIPT" + gender + "SPLITSCRIPT" + DOB + "SPLITSCRIPT" + passportNo + "SPLITSCRIPT" + pass_ex_date + "SPLITSCRIPT" + IssCon + "SPLITSCRIPT" + paxtype + "SPLITSCRIPT" + InfantRef + "SPLITSCRIPT" + SSR;

        if (i == 1) {
            FirstPax = TotalPax;
        }
        document.getElementById('hdnPax').value += TotalPax + "|"
    });

    TotalPax = "";
    paxtypcnt = 0;
    $("#passengersdetails .commChdpaxcls").each(function () {

        if (checkflag == true) {
            return;
        }
        checkflag = false;
        i++;
        paxtypcnt++;
        Ids_split = this.id.split("_")[1];

        pax_type_det = $("#ch_type_" + Ids_split).data("paxvalue").split(':')[0];

        paty = $("#ch_type_" + Ids_split).data("paxvalue").split(':')[0];
        if ($("#ch_Title_" + Ids_split).val() == null || $("#ch_Title_" + Ids_split).val() == "0") {
            showerralert("Please Enter Child" + paxtypcnt + " Title.", "", "");
            $("#ch_Title_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if ($("#ch_firstName_" + Ids_split).val() == null || $("#ch_firstName_" + Ids_split).val().trim() == "") {
            showerralert("Please Enter Child" + paxtypcnt + " First name.", "", "");
            $("#ch_firstName_" + Ids_split).val("");
            $("#ch_firstName_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if ($("#ch_lastName_" + Ids_split).val() == null || $("#ch_lastName_" + Ids_split).val().trim() == "") {
            showerralert("Please Enter Child" + paxtypcnt + " Last Name.", "", "");
            $("#ch_lastName_" + Ids_split).val("");
            $("#ch_lastName_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if ($("#ch_gender_" + Ids_split).val() == null || $("#ch_gender_" + Ids_split).val().trim() == "" || $("#ch_gender_" + Ids_split + ' option:selected').text().trim() == "Select") {
            showerralert("Please Select Child" + paxtypcnt + " Gender.", "", "");
            $("#ch_gender_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if (($("#ch_DOB_" + Ids_split).val() == null || $("#ch_DOB_" + Ids_split).val().trim() == "") && validate_DOB == "TRUE" || (($("#ch_passportNo_" + Ids_split).val() == null || $("#ch_passportNo_" + Ids_split).val().trim() == "") && $("#ch_passportNo_" + Ids_split).val().trim() != "")) {
            showerralert("Please Enter the " + paty + paxtypcnt + " DOB.", "", "");
            $("#ch_DOB_" + Ids_split).val("");
            $("#ch_DOB_" + Ids_split).focus();
            checkflag = true;
            return;
        }

        if ($("#ch_DOB_" + Ids_split).val() != null && $("#ch_DOB_" + Ids_split).val() != "") {
            Paxtyp = Calculateage($("#ch_DOB_" + Ids_split).val(), "CHILD");
            if (Paxtyp == false) {
                showerralert("Please Enter the valid " + paty + paxtypcnt + " DOB.", "", "");
                $("#ch_DOB_" + Ids_split).val("");
                $("#ch_DOB_" + Ids_split).focus();
                checkflag = true;
                return;
            }
        }

        if (($("#ch_passportNo_" + Ids_split).val() == null || $("#ch_passportNo_" + Ids_split).val().trim() == "") && validate_PWD == "TRUE") {
            showerralert("Please Enter the " + paty + paxtypcnt + " Passport No.", "", "");
            $("#ch_passportNo_" + Ids_split).val("");
            $("#ch_passportNo_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if (($("#ch_pass_ex_date_" + Ids_split).val() == null || $("#ch_pass_ex_date_" + Ids_split).val().trim() == "") && validate_PWD == "TRUE" || (($("#ch_pass_ex_date_" + Ids_split).val() == null || $("#ch_pass_ex_date_" + Ids_split).val().trim() == "") && $("#ch_passportNo_" + Ids_split).val().trim() != "")) {
            showerralert("Please Enter the " + paty + paxtypcnt + "  PassPort Exp. date.", "", "");
            $("#ch_pass_ex_date_" + Ids_split).val("");
            $("#ch_pass_ex_date_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if ($("#ch_pass_ex_date_" + Ids_split).val() != null && $("#ch_pass_ex_date_" + Ids_split).val() != "") {
            var ppexpdate = PassportExp($("#ch_pass_ex_date_" + Ids_split).val());
            if (ppexpdate != true) {
                showerralert("Please Enter the valid " + paty + paxtypcnt + " PassPort Exp. date.", "", "");
                $("#ch_pass_ex_date_" + Ids_split).val("");
                $("#ch_pass_ex_date_" + Ids_split).focus();
                checkflag = true;
                return;
            }

        }

        if (($("#ch_Cntry_" + Ids_split).val() == null || $("#ch_Cntry_" + Ids_split).val().trim() == "" || $("#ch_Cntry_" + Ids_split).val().trim() == "0") && validate_PWD == "TRUE" && $("#ch_passportNo_" + Ids_split).val().trim() != "") {
            showerralert("Please Select the " + paty + paxtypcnt + "  Passport Issued Country.", "", "");
            $("#ch_Cntry_" + Ids_split).focus();
            checkflag = true;
            return;
        }

        var SSR = ""
        if ($("#hdnchdMeals_" + Ids_split).val() != null && $("#hdnchdMeals_" + Ids_split).val().trim() != "") {
            SSR = $("#hdnchdMeals_" + Ids_split).val();
        }

        paxtype = paty;
        title = $("#ch_Title_" + Ids_split).val() != null ? $("#ch_Title_" + Ids_split).val().trim() : "";
        firstName = $("#ch_firstName_" + Ids_split).val().trim();
        lastName = $("#ch_lastName_" + Ids_split).val().trim();
        gender = $("#ch_gender_" + Ids_split).val().trim();
        DOB = $("#ch_DOB_" + Ids_split).val().trim();
        passportNo = $("#ch_passportNo_" + Ids_split).val().trim();
        pass_ex_date = $("#ch_pass_ex_date_" + Ids_split).val().trim();
        IssCon = $("#ch_Cntry_" + Ids_split).val();
        SSR = SSR;

        TotalPax = title + "SPLITSCRIPT" + firstName + "SPLITSCRIPT" + lastName + "SPLITSCRIPT" + gender + "SPLITSCRIPT" + DOB + "SPLITSCRIPT" + passportNo + "SPLITSCRIPT" + pass_ex_date + "SPLITSCRIPT" + IssCon + "SPLITSCRIPT" + paxtype + "SPLITSCRIPT" + InfantRef + "SPLITSCRIPT" + SSR;

        document.getElementById('hdnPax').value += TotalPax + "|"
    });

    TotalPax = "";
    paxtypcnt = 0;
    $("#passengersdetails .commInfpaxcls").each(function () {

        if (checkflag == true) {
            return;
        }
        checkflag = false;
        i++;
        paxtypcnt++;
        Ids_split = this.id.split("_")[1];

        pax_type_det = $("#in_type_" + Ids_split).data("paxvalue").split(':')[0];

        paty = $("#in_type_" + Ids_split).data("paxvalue").split(':')[0];
        if ($("#in_Title_" + Ids_split).val() == null || $("#in_Title_" + Ids_split).val() == "0") {
            //alert("Please Enter Infant Title");
            showerralert("Please Enter Infant" + paxtypcnt + " Title.", "", "");
            $("#in_Title_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if ($("#in_firstName_" + Ids_split).val() == null || $("#in_firstName_" + Ids_split).val().trim() == "") {
            //alert("Please Enter Infant First name");
            showerralert("Please Enter Infant" + paxtypcnt + " First name.", "", "");
            $("#in_firstName_" + Ids_split).val("");
            $("#in_firstName_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if ($("#in_lastName_" + Ids_split).val() == null || $("#in_lastName_" + Ids_split).val().trim() == "") {
            //alert("Please Enter Infant Last Name");
            showerralert("Please Enter Infant" + paxtypcnt + " Last Name.", "", "");
            $("#in_lastName_" + Ids_split).val("");
            $("#in_lastName_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if ($("#in_gender_" + Ids_split).val() == null || $("#in_gender_" + Ids_split).val().trim() == "" || $("#in_gender_" + Ids_split + ' option:selected').text().trim() == "Select") {
            //alert("Please Select Infant Gender");
            showerralert("Please Select Infant" + paxtypcnt + " Gender.", "", "");
            $("#in_gender_" + Ids_split).val("");
            $("#in_gender_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if (($("#in_DOB_" + Ids_split).val() == null || $("#in_DOB_" + Ids_split).val().trim() == "") && validate_DOB == "TRUE" || (($("#in_passportNo_" + Ids_split).val() == null || $("#in_passportNo_" + Ids_split).val().trim() == "") && $("#in_passportNo_" + Ids_split).val().trim() != "")) {

            showerralert("Please Enter the " + paty + paxtypcnt + " DOB.", "", "");
            $("#in_DOB_" + Ids_split).val("");
            $("#in_DOB_" + Ids_split).focus();
            checkflag = true;
            return;
        }


        if ($("#in_DOB_" + Ids_split).val() != null && $("#in_DOB_" + Ids_split).val() != "") {
            Paxtyp = Calculateage($("#in_DOB_" + Ids_split).val(), "INFANT");
            if (Paxtyp == false) {
                showerralert("Please Enter the valid " + paty + paxtypcnt + " DOB.", "", "");
                $("#in_DOB_" + Ids_split).val("");
                $("#in_DOB_" + Ids_split).focus();
                checkflag = true;
                return;
            }
        }

        if (($("#in_passportNo_" + Ids_split).val() == null || $("#in_passportNo_" + Ids_split).val().trim() == "") && validate_PWD == "TRUE") {
            //alert("Please Enter the " + paty + " Passport No.");

            showerralert("Please Enter the " + paty + paxtypcnt + " Passport No.", "", "");
            $("#in_passportNo_" + Ids_split).val("");
            $("#in_passportNo_" + Ids_split).focus();
            checkflag = true;
            return;
        }
        if (($("#in_pass_ex_date_" + Ids_split).val() == null || $("#in_pass_ex_date_" + Ids_split).val().trim() == "") && validate_PWD == "TRUE" || (($("#in_pass_ex_date_" + Ids_split).val() == null || $("#in_pass_ex_date_" + Ids_split).val().trim() == "") && $("#in_passportNo_" + Ids_split).val().trim() != "")) {
            //alert("Please Enter the " + paty + "  PassPort Exp. date");
            showerralert("Please Enter the " + paty + paxtypcnt + "  PassPort Exp. date.", "", "");
            $("#in_pass_ex_date_" + Ids_split).val("");
            $("#in_pass_ex_date_" + Ids_split).focus();
            checkflag = true;
            return;
        }

        if ($("#in_pass_ex_date_" + Ids_split).val() != null && $("#in_pass_ex_date_" + Ids_split).val() != "") {
            var ppexpdate = PassportExp($("#in_pass_ex_date_" + Ids_split).val());
            if (ppexpdate != true) {
                showerralert("Please Enter the valid " + paty + paxtypcnt + " PassPort Exp. date.", "", "");
                $("#in_pass_ex_date_" + Ids_split).val("");
                $("#in_pass_ex_date_" + Ids_split).focus();
                checkflag = true;
                return;
            }
        }

        if (paty.toUpperCase() == "INFANT") {
            if (jQuery.inArray($("#ch_Infant_" + Ids_split).val(), InfantRefrence) != -1) {
                //alert("Please select different carry infant");
                showerralert("Please select different carry infant.", "", "");
                $("#ch_Infant_" + Ids_split).focus();
                checkflag = true;
                return;
            }
            else {
                InfantRefrence.push($("#ch_Infant_" + Ids_split).val());
                InfantRef = $("#ch_Infant_" + Ids_split).val();
            }
        }

        if (($("#in_Cntry_" + Ids_split).val() == null || $("#in_Cntry_" + Ids_split).val().trim() == "" || $("#in_Cntry_" + Ids_split).val().trim() == "0") && validate_PWD == "TRUE" && $("#in_passportNo_" + Ids_split).val().trim() != "") {
            //alert("Please Select the " + paty + "  Passport Issued Country");
            showerralert("Please Select the " + paty + paxtypcnt + "  Passport Issued Country.", "", "");
            $("#in_Cntry_" + Ids_split).focus();
            checkflag = true;
            return;
        }

        var SSR = "";
        paxtype = paty;
        title = $("#in_Title_" + Ids_split).val() != null ? $("#in_Title_" + Ids_split).val().trim() : "";
        firstName = $("#in_firstName_" + Ids_split).val().trim();
        lastName = $("#in_lastName_" + Ids_split).val().trim();
        gender = $("#in_gender_" + Ids_split).val().trim();
        DOB = $("#in_DOB_" + Ids_split).val().trim();
        passportNo = $("#in_passportNo_" + Ids_split).val().trim();
        pass_ex_date = $("#in_pass_ex_date_" + Ids_split).val().trim();
        IssCon = $("#in_Cntry_" + Ids_split).val();
        // InfantRef = "";
        SSR = SSR;
        TotalPax = title + "SPLITSCRIPT" + firstName + "SPLITSCRIPT" + lastName + "SPLITSCRIPT" + gender + "SPLITSCRIPT" + DOB + "SPLITSCRIPT" + passportNo + "SPLITSCRIPT" + pass_ex_date + "SPLITSCRIPT" + IssCon + "SPLITSCRIPT" + paxtype + "SPLITSCRIPT" + InfantRef + "SPLITSCRIPT" + SSR;

        document.getElementById('hdnPax').value += TotalPax + "|"
    });

    return true;
}

function Changebookapptype() {
    var booktype = document.getElementById("booking_type").value;
    if (booktype == "BB") {
        $("#ruletype")[0].innerHTML = "Best&nbsp;Buy";
        $("#bbruleheader")[0].innerHTML = "Best&nbsp;Buy&nbsp;Rules";
        $("#btn_block").css("display", "none");
        $("#btn_book").css("display", "none");
        $("#best_buy").css("display", "block");
    }
    else if (booktype == "SM") {
        $("#ruletype")[0].innerHTML = "Seamless&nbsp;Booking";
        $("#bbruleheader")[0].innerHTML = "Seamless&nbsp;Booking&nbsp;Rules";
        $("#btn_block").css("display", "block");
        $("#btn_book").css("display", "block");
        $("#best_buy").css("display", "none");

        //$.blockUI({
        //    message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;width: 10%;" id="FareRuleLodingImage" />',
        //    css: { padding: '5px' }
        //});

        $.ajax({
            type: "POST", 		//GET or POST or PUT or DELETE verb
            url: Fetchlocaldata,// Location of the service data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (json) {//On Successful service call

                //$.unblockUI();
                var result = json;
                var earnings_calculate = 0;
                if (result != "") {
                    if (result.indexOf("disessionExp005") != -1) {
                        window.location.href = sessionExb;
                    }
                    if (result.indexOf("availselecteddive") != -1) {
                        $("#dvSelectView").html(json);
                        $("#farelbl")[0].innerHTML = "Total&nbsp;Fare";
                        var statuscode = $("#divstatuscode").html();
                        //if (statuscode != null && (statuscode == "2" || statuscode == "02" || statuscode == "3" || statuscode == "03")) {  //RtripComparFlg- For Roundtrp Spl Select from Roundtrp Avail no need to show reverse pupup's...
                        //    var errormessage = $("#returnmessage").html();
                        //    showerralert(errormessage, "", "");
                        //}
                    }
                    else {
                        $("#dvSelectView").html(json);
                        var errormessage = $("#diverrormsg").html();
                        if (errormessage != null || errormessage != "") {
                            showerralert(errormessage, "", "");
                        } else {
                            errormessage = "Selected flight not available-(#03)";
                            showerralert(errormessage, "", "");
                        }
                    }
                }
                else if (result = "") {
                    showerralert("Unable to select. problem occured internally. Please Contact Customer Care. (#09).", "", "");
                }
                // $.unblockUI();
            },
            error: function (e) {//On Successful service call  
                $.unblockUI();
                console.log("Internal Problem occured while insert logout details. (#07).");
            }	// When Service call fails
        });
    }
    else if (booktype == "BF") {
        $("#ruletype")[0].innerHTML = "Bargain&nbsp;Fare";
        $("#bbruleheader")[0].innerHTML = "Bargain&nbsp;Fare&nbsp;Rules";
        $("#btn_block").css("display", "block");
        $("#btn_book").css("display", "block");
        $("#best_buy").css("display", "none");
        $("#BB_fareheader").css("display", "none");
        $("#dvinnerFareDetinBestbuy").css("display", "none");
    }
}

function Ruledisplay() {
    var Bookingtype = document.getElementById("booking_type").value;
    var param = {
        Booktype: Bookingtype
    };
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: Bestbuyrule,// Location of the service
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(param),
        dataType: "json",
        success: function (json) {//On Successful service call
            $.unblockUI();
            var result = json.Result;
            if (result[1] != null && result[1] != "") {
                $("#displayrules")[0].innerHTML = result[1];
                $("#modal-getrulepopup").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
            else {
                console.log(json.Message != "" ? json.Message : "Problem occured while fetch details. (#03).");
            }
            //Do Stuff when we need...
        },
        error: function (e) {//On Successful service call  
            $.unblockUI();
            console.log("Internal Problem occured while insert logout details. (#07).");
            //Do Sutff when we need...
        }	// When Service call fails
    });
}

function showComparisionPop(Depid, Rtnid, RtrpSplAry) {
    var aryids = [];
    aryids.push(Depid);
    aryids.push(Rtnid);
    var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;
    var s = '';
    s += '<div class="row">';
    s += '<div class="col-md-8 col-xs-12">';
    s += '<div class="row cmpRoundtrpParnt p-t-15">';
    var totfr = 0;
    for (var i = 0; i < aryids.length; i++) {

        var idss = aryids[i];
        var selectedObj = $.grep(dataproperty_array, function (n, i) {
            return n.datakey == idss;
        });
        // ary_Inva.push($('.Travselect_' + arySelectedFltIds[i])[0].dataset.hdInva);
        var hdnvalus = selectedObj[0]["data-hdInva"];
        var grossfare = selectedObj[0]["data-grossfare"];
        //$('.Travselect_' + aryids[i])[0].dataset.hdinva;  //$('#hdInva' + aryids[i]).val();
        var arysplitconn = hdnvalus.split('SpLITSaTIS');
        var aryvalus = arysplitconn[0].split('SpLitPResna');
        var arylstvalus = arysplitconn[arysplitconn.length - 1].split('SpLitPResna');

        //GrossAmt                                          //Service Charge                                                                                    
        // var gross = (aryvalus.length > 31 ? parseInt(aryvalus[31]) : 0);//+ ((aryvalus.length > 31 && aryvalus[31] != null && aryvalus[31] != "") ? parseInt(aryvalus[31]) : 0)
        var gross = grossfare;

        totfr = Number(parseFloat(totfr) + parseFloat(gross)).toFixed(decimalflag);

        s += '<div class="col-sm-6 col-xs-12">';

        s += '<div class="rt pricingTable">';
        s += '<span class="icon"><img class="FlightTip" alt="" style="width: 25px; height: 25px;" src="' + imgurl + (aryvalus.length > 9 ? aryvalus[9] : "") + ".png?" + imgver + '" /></span>';

        s += '<div class="pricingTable-header">';
        if (i == 0)
            s += '<h3 class="title">Onward</h3>';
        else
            s += '<h3 class="title">Return</h3>';

        s += '<span class="price-value"><span class="assincur">' + assignedcurrency + '</span>' + gross + '</span>';
        s += '</div>';

        s += '<ul class="pricing-content">';
        s += '<li>' + (aryvalus.length > 18 ? aryvalus[18] : "") + '</li>';
        s += '<li>' + (aryvalus.length > 5 ? aryvalus[5] : "") + " - " + (aryvalus.length > 17 ? aryvalus[17] : "") + '</li>';

        //Departure
        var aryOtime = aryvalus.length > 2 ? aryvalus[2].split(' ') : "";
        s += '<li>' + (aryvalus.length > 0 ? aryvalus[0] : "") + " - <span class='boldtxt'>" + (aryOtime.length > 3 ? aryOtime[3] : "") + '</span><span class="m-l-5"><img src="' + strImageUrl + '/' + $("#hdn_producttype").val() + '/flight_icon_2.png"/></span></li>';

        //Arrival
        var aryRtime = arylstvalus.length > 3 ? arylstvalus[3].split(' ') : "";
        s += '<li>' + (arylstvalus.length > 1 ? arylstvalus[1] : "") + " - <span class='boldtxt'>" + (aryRtime.length > 3 ? aryRtime[3] : "") + '</span><span class="m-l-5"><img class="rtnflt" src="' + strImageUrl + '/' + $("#hdn_producttype").val() + '/flight_icon_1.png"/></span></li>';

        var refund = $('#li_Rows' + aryids[i]).data('refund');
        s += '<li>' + (refund == "refun" ? "Refundable: <span class='re'>Yes</span>" : (refund == "nonrefun" ? "Refundable: <span class='non-re'>No</span>" : "Refundable: <span class='no-re'>N/A</span>")) + '</li>';

        //s += '<li>15 Domains</li>';
        s += '</ul>';

        s += '</div>';

        s += '</div>';

        if (i == 0)
            s += '<span class="flPlus"> <i class="fa fa-plus"></i> </span>';
    }

    s += '<div class="col-xs-12">';
    s += '<center><p class="ptotfr"><span>Total Fare: </span><span class="fr">' + totfr + '</span></p><button class="button btn-small selebtn btncmpr" onclick="javascript:GoWithRoundTrpFare();">Roundtrip fare</button></center>';
    s += '</div>';

    s += '</div>';


    s += '</div>';
    var rsp_gross = Number(parseFloat(RtrpSplAry[0].GrossAmount)).toFixed(decimalflag);
    var rsp_sertax = Number(parseFloat(RtrpSplAry[0].ServiceTax)).toFixed(decimalflag);
    var rsp_serfee = Number(parseFloat(RtrpSplAry[0].Servicefee)).toFixed(decimalflag);
    var rsp_sftax = Number(parseFloat(RtrpSplAry[0].Sftax)).toFixed(decimalflag);
    var rsp_sfgst = Number(parseFloat(RtrpSplAry[0].SFGST)).toFixed(decimalflag);
    var rsp_servicecharge = Number(parseFloat(RtrpSplAry[0].Servicecharge)).toFixed(decimalflag);
    var rsp_mkp = Number(parseFloat(RtrpSplAry[0].MarkUp)).toFixed(decimalflag);
    var rspfare = Number(parseFloat(rsp_gross) + parseFloat(rsp_sertax) + parseFloat(rsp_serfee) + parseFloat(rsp_sftax) + parseFloat(rsp_sfgst) + parseFloat(rsp_servicecharge) + parseFloat(rsp_mkp)).toFixed(decimalflag);
    //var rspfare = Number(parseFloat(RtrpSplAry[0].GrossAmount)).toFixed(decimalflag) + (RtrpSplAry[0].ServiceTax != null && RtrpSplAry[0].ServiceTax != "" ? Number(parseFloat(RtrpSplAry[0].ServiceTax)).toFixed(decimalflag) : 0);
    //rspfare = Number(parseFloat(rspfare)).toFixed(decimalflag) + (RtrpSplAry[0].Servicefee != null && RtrpSplAry[0].Servicefee != "" ? Number(parseFloat(RtrpSplAry[0].Servicefee)).toFixed(decimalflag) : 0);
    //rspfare = Number(parseFloat(rspfare)).toFixed(decimalflag) + (RtrpSplAry[0].Sftax != null && RtrpSplAry[0].Sftax != "" ? Number(parseInt(RtrpSplAry[0].Sftax)).toFixed(decimalflag) : 0);
    //rspfare = Number(parseFloat(rspfare)).toFixed(decimalflag) + (RtrpSplAry[0].SFGST != null && RtrpSplAry[0].SFGST != "" ? Number(parseInt(RtrpSplAry[0].SFGST)).toFixed(decimalflag) : 0);
    //rspfare = Number(parseFloat(rspfare)).toFixed(decimalflag) + (RtrpSplAry[0].Servicecharge != null && RtrpSplAry[0].Servicecharge != "" ? Number(parseInt(RtrpSplAry[0].Servicecharge)).toFixed(decimalflag) : 0);
    //rspfare = Number(parseFloat(rspfare)).toFixed(decimalflag) + (RtrpSplAry[0].MarkUp != null && RtrpSplAry[0].MarkUp != "") ? Number(parseInt(RtrpSplAry[0].MarkUp)).toFixed(decimalflag) : 0;
    s += '<div class="col-md-4 col-md-offset-0 col-sm-offset-3 col-sm-6 col-xs-12">';
    s += '<div class="row p-t-15">';
    s += '<div class="col-xs-12">';
    s += '<div class="rts pricingTable">';
    s += '<span class="icon"><img class="FlightTip" alt="" style="width: 25px; height: 25px;" src="' + imgurl + RtrpSplAry[0].PlatingCarrier + ".png?" + imgver + '" /></span>';

    s += '<div class="pricingTable-header">';
    s += '<h3 class="title">Roundtrip Special</h3>';

    s += '<span class="price-value"><span class="assincur">' + assignedcurrency + '</span>' + rspfare + '</span>';
    s += '</div>';

    s += '<ul class="pricing-content">';

    s += '<li>' + RtrpSplAry[0].FlightNumber + '</li>';
    s += '<li>' + RtrpSplAry[0].Class + " - " + RtrpSplAry[0].FAREBASISCODE + '</li>';

    var aryOtime = RtrpSplAry[0].DepartureDate.split(' ');
    s += '<li>' + RtrpSplAry[0].Origin + " - <span class='boldtxt'>" + (aryOtime.length > 3 ? aryOtime[3] : "") + '</span><span class="m-l-5"><img src="' + strImageUrl + '/' + $("#hdn_producttype").val() + '/flight_icon_2.png"/></span></li> ';

    var aryRtime = RtrpSplAry[RtrpSplAry.length - 1].ArrivalDate.split(' ');
    s += '<li>' + RtrpSplAry[RtrpSplAry.length - 1].Destination + " - <span class='boldtxt'>" + (aryRtime.length > 3 ? aryRtime[3] : "") + '</span><span class="m-l-5"><img class="rtnflt" src="' + strImageUrl + '/' + $("#hdn_producttype").val() + '/flight_icon_1.png"/></span></li>';

    var refund = RtrpSplAry[0].refund;
    s += '<li>' + (refund == "refun" ? "Refundable: <span class='re'>Yes</span>" : (refund == "nonrefun" ? "Refundable: <span class='non-re'>No</span>" : "Refundable: <span class='no-re'>N/A</span>")) + '</li>';

    //s += '<li>15 Domains</li>';

    s += '</ul>';
    s += '</div>';
    s += '</div>';
    s += '</div>';

    s += '<div class="row">';
    s += '<div class="col-xs-12">';
    s += '<center><p class="ptotfr"><span>Total Fare: </span><span class="fr">' + rspfare + '</span></p><button class="button btn-small selebtn btncmpr" style="min-width:180px;" onclick="javascript:GoWithRoundTrpSplFare();">Go Roundtrip Special Fare</button></center>';
    s += '</div>';
    s += '</div>';

    s += '</div>';
    s += '</div>';

    s += '';

    if (rspfare < totfr) {
        var Ititle = "Flight Comparision";
        var Isubtitle = "Comparision between Roundtrip fare vs Roundtrip Special fare.";
        var IContent = s;
        var Ifullopt = true;
        var Itemp1 = "";
        var Itemp2 = "";
        $('#modal-Fare').removeClass('addtop');
        $("#hdtxt_trip").val("R"); //Reset Trip flag from Roundtrop Spl to Roundtrip for work as Roundtrip if Close popup...
        showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
    }
    else {
        GoWithRoundTrpFare();
    }
}

function GoWithRoundTrpFare() {
    $('#modal-Fare').iziModal('close');
    SelectRtripFAvail("0", "1"); //0-No need to check Roundtrip Special Select process from Roundtrip Avail... 1- check it... by saranraj on 20170619...
}

function GoWithRoundTrpSplFare() {
    $('#modal-Fare').iziModal('close');
    $("#hdtxt_trip").val("Y"); //Set Trip flag from Roundtrop  to Roundtrip Spl for work as Roundtrip Spl...


    var displayalertmsg = ""; //To show two msg  STS185
    var Gststatuscode = $("#divgst_statuscode").html(); //Added for GST Manadatory purposse 
    $("#allow_gst").prop('checked', false);
    $("#allow_gst").prop('disabled', false);
    var selectpopup = $("#divgst_statuscode").data("returnmessage");
    if (Gststatuscode != null && (Gststatuscode == "04" || Gststatuscode == "4")) { //STS185
        $("#allow_gst").prop('checked', true);
        $("#allow_gst").prop('disabled', true);
        Allowgst();
    }
    else if (selectpopup != "" && selectpopup != null) {
        if (displayalertmsg != "")
            displayalertmsg += "</br>" + selectpopup;
        else
            displayalertmsg += selectpopup;
    }

    if (displayalertmsg != "" && displayalertmsg != null) {
        $('#pfrConfirmMsg').html(displayalertmsg);
        $("#modal_fr_confirm").modal({
            backdrop: 'static',
            keyboard: false
        });
        return false;

    }

    RedirectToSelect();
}

function RedirectToSelect() {
    $('body').data('BackFlag', 'SELECT');
    $("html, body").animate({ scrollTop: 0 }, 600);
    $("#dvAvailView").css("display", "none");
    $("#dvSearchArea").css("display", "none");
    $("#dvSelectView").css("display", "block");
}

function createqueuepopup(result, Id) {
    var res = result.split('|').length > 1 ? "yes" : "no";
    var bestbuypopup = "";
    if (res == "yes") {
        for (i = 0; i < result.split('|').length ; i++) {
            bestbuypopup += "<div class='qContent' ><pre>" + result.split('|')[i] + "</pre></div><br />";
        }

    }
    else {
        bestbuypopup += "<div class='qContent' ><pre>" + result + "</pre></div><br /><br />";
    }
    bestbuypopup += "<table class='qTable' ><tr><td></td><td ><a class='color-2 puerto-btn-1  btnSpecial' data-check='Y' onclick='javascript:queueselect(this," + Id + ")'> <i  class='faNew fa-check-square-o'></i> <small> Yes, I Agree</small> </a> </td>";
    bestbuypopup += "<td ><a class='color-2 puerto-btn-1  btnSpecial' data-check='N' onclick='javascript:queueselect(this," + Id + ")'> <i class='faNew fa-dot-circle-o'></i> <small> No, Normal Ticket </small> </a></td>";
    bestbuypopup += "<td ><a class='color-2 puerto-btn-1  btnSpecial' data-check='C' onclick='javascript:queueselect(this," + Id + ")'> <i class='faNew fa-times'></i>  <small> Cancel </small> </a></td></tr></table>";
    document.getElementById("dvSplFareCmpr").innerHTML = bestbuypopup;

    $('#fadeandscale').popup({
        opacity: 0.3,
        transition: 'all 0.3s'
    });
    $('#fadeandscale').popup('show');

    $('.btn-close').click(function () {
        $('#fadeandscale').popup('hide');
    });
}

//#endregion
function BookRequest(stu) {
    try {

        $("#dvEmployee,#dvEmpDetails").hide();
        $("#hdncash_Empname").val("");
        $("#hdncash_Empemail").val("");
        $("#hdncash_Empmobile").val("");
        $("#hdncash_Empbranch").val("");
        $(".clsClear").val("");

        var emailReg = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        var nameReg = /[ ](?=[ ])_-|[^A-Za-z0-9 ]+/gi;
        var paxmailid = $('#txtAirlinemailid').val();
        if (paxmailid != "" && paxmailid != null) {
            if (!emailReg.test(paxmailid)) {
                alert('Please Enter Valid Mail Id');
                $("#txtAirlinemailid").focus();
                return false;
            }
        }
        if ($("#hdn_allowgst").val() == "Y" && document.getElementById('allow_gst').checked == true) {
            var gst_regnumber = document.getElementById("gst_regnumber").value;
            var gst_cmpnyname = document.getElementById("gst_cmpnyname").value;
            var gst_address = document.getElementById("gst_address").value;
            var gst_mailid = document.getElementById("gst_mailid").value;
            var gst_mobno = document.getElementById("gst_mobno").value;


            //if (gst_regnumber == "" && gst_cmpnyname == "" && gst_address == "" && gst_mailid == "" && gst_mobno == "") {

            //}
            //else {
            if (gst_regnumber == null || gst_regnumber == "") {
                showerralert('Please Enter GST Registration Number');
                return false;
            }
            else {
                var idfilter = /^[0-9]{2}[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}[a-zA-Z0-9]{1}[zZ]{1}[a-zA-Z0-9]{1}$/;
                if (!idfilter.test(gst_regnumber)) {
                    showerralert("Please enter a valid GST No.");
                    return false;
                }
            }
            if (gst_cmpnyname == null || gst_cmpnyname == "") {
                showerralert('Please Enter GST Company Name');
                return false;
            }
            if (gst_address == null || gst_address == "") {
                showerralert('Please Enter GST Address');
                return false;
            }
            if (gst_mailid == null || gst_mailid == "") {
                showerralert('Please Enter GST MailId');
                return false;
            }
            else {
                if (!emailReg.test(gst_mailid)) {
                    showerralert('Please Enter Valid GST Mail Id');
                    return false;
                }
            }
            if (gst_mobno == null || gst_mobno == "") {
                showerralert('Please Enter GST MobileNo');
                return false;
            }
            // }
        }

        //To Check Internet connection is available... by saranraj on 20170725...
        var InetFlg = checkconnection();
        if (!InetFlg) {
            showerralert("Please check connectivity.", "", "");
            return false;
        }
        //End...

        if (Validate()) {

            $("#ssrprev").html('');

            if (stu == 0) {
                BlockTicket = true;
            }
            else if (stu == 3) {
                Bargainflag = true;
            }
            else {
                BlockTicket = false;
            }

            //  BlockTicket = false;
            document.getElementById('hdnPax').value = "";
            var par = $("#passengersdetails .commpaxcls");
            //var par = document.getElementById('tbl2').getElementsByTagName('tr');
            var InfantRefrence = new Array();
            var InfantRef = "";
            var Ids_split = "";
            var paty = "";

            var totalpaxdetails = "";
            var TotalPax = "";
            var FirstPax = "";
            var BajajInsdetails = "";

            var pasIds = "";
            var paxtype = "";
            var title = "";
            var firstName = "";
            var lastName = "";
            var gender = "";
            var passportNo = "";
            var DOB = "";
            var pass_ex_date = "";
            var pass_issue_date = "";
            var i = 0;
            var checkflag = false;
            var adtcheckPendingflag = "";
            var chdcheckPendingflag = "";
            var infcheckPendingflag = "";
            var adt_nomineedet = "";
            var chd_nomineedet = "";
            var inf_nomineedet = "";

            var pax_type_det = "";
            var Paxtyp = "";
            var paxtypcnt = 0;
            var Producttype = $("#hdn_producttype").val();

            var paxdet = $('#hdnTotalPaxcount').val();
            var SegCount = $('#hdnSegCount').val();
            var adtcount = paxdet.split("@")[0];
            var chdcount = paxdet.split("@")[1];
            var Totpaxcount = parseFloat(adtcount) + parseFloat(chdcount);
            var paxcount = 0;
            var strPaxArray = [];

            for (var adtcnt = 1; adtcnt <= parseFloat(adtcount) ; adtcnt++) {
                strPaxArray[paxcount] = { paxlabel: "Adult" + adtcnt, hdnpax: "hdnadtMeals_" + (adtcnt - 1) };
                paxcount++;
            }
            for (var chdcnt = 1; chdcnt <= parseFloat(chdcount) ; chdcnt++) {
                strPaxArray[paxcount] = { paxlabel: "Child" + chdcnt, hdnpax: "hdnchdMeals_" + (chdcnt - 1) };
                paxcount++;
            }

            if ($("#hdn_producttype").val() == "RIYA") {

                //$(".fliclass").html("Class -" + $("#ddlclass").text().trim())
                $(".timedur").html(sessionStorage.getItem('totaldur'))
                // $("#timedur").va


            }

            $("#passengersdetails .commAdtpaxcls").each(function () {
                Nationality = "";
                i++;
                paxtypcnt++;
                Ids_split = this.id.split("_")[1];

                pax_type_det = $("#ad_type_" + Ids_split).data("paxvalue").split(':')[0];

                paty = $("#ad_type_" + Ids_split).data("paxvalue").split(':')[0];
                if ($("#ad_Title_" + Ids_split).val() == null || $("#ad_Title_" + Ids_split).val() == "0") {
                    showerralert("Please Enter Adult" + paxtypcnt + " Title.", "", "");
                    $("#ad_Title_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if ($("#ad_firstName_" + Ids_split).val() == null || $("#ad_firstName_" + Ids_split).val().trim() == "") {
                    showerralert("Please Enter Adult" + paxtypcnt + " First name.", "", "");
                    $("#ad_firstName_" + Ids_split).val("");
                    $("#ad_firstName_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if (nameReg.test($("#ad_firstName_" + Ids_split).val())) {
                    showerralert("Please Enter Valid Adult" + paxtypcnt + " First name.", "", "");
                    $("#ad_firstName_" + Ids_split).val("");
                    $("#ad_firstName_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if (($("#hdn_lastnamemand").val() == "" || $("#hdn_lastnamemand").val() == "1") && ($("#ad_lastName_" + Ids_split).val() == null || $("#ad_lastName_" + Ids_split).val().trim() == "")) {
                    showerralert("Please Enter Adult" + paxtypcnt + " Last Name.", "", "");
                    $("#ad_lastName_" + Ids_split).val("");
                    $("#ad_lastName_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if (($("#hdn_lastnamemand").val() == "" || $("#hdn_lastnamemand").val() == "1") && nameReg.test($("#ad_lastName_" + Ids_split).val())) {
                    showerralert("Please Enter Valid Adult" + paxtypcnt + " Last name.", "", "");
                    $("#ad_lastName_" + Ids_split).val("");
                    $("#ad_lastName_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if ($("#ad_gender_" + Ids_split).val() == null || $("#ad_gender_" + Ids_split).val().trim() == "" || $("#ad_gender_" + Ids_split + ' option:selected').text().trim() == "Select") {
                    // alert("Please Select Adult Gender");
                    showerralert("Please Select Adult" + paxtypcnt + " Gender.", "", "");
                    $("#ad_gender_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if (($("#ad_DOB_" + Ids_split).val() == null || $("#ad_DOB_" + Ids_split).val().trim() == "") && (validate_DOB == "TRUE")) {
                    //alert("Please Enter the " + paty + " DOB");
                    showerralert("Please Enter the " + paty + paxtypcnt + " DOB.", "", "");
                    $("#ad_DOB_" + Ids_split).val("");
                    $("#ad_DOB_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if ($("#ad_DOB_" + Ids_split).val() != null && $("#ad_DOB_" + Ids_split).val() != "") {
                    Paxtyp = Calculateage($("#ad_DOB_" + Ids_split).val(), "ADULT");
                    if (Paxtyp == false) {
                        showerralert("Please Enter the valid " + paty + paxtypcnt + " DOB.", "", "");
                        $("#ad_DOB_" + Ids_split).val("");
                        $("#ad_DOB_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                }

                if (($("#hdn_producttype").val() == "JOD" || $("#hdn_producttype").val() == "RIYA" || $("#hdn_AppHosting").val() == "BSA") && $('body').data('segtype') != "D") {
                    if (($("#ad_passportNo_" + Ids_split).val() == null || $("#ad_passportNo_" + Ids_split).val().trim() == "") && validate_PWD == "TRUE") {
                        //alert("Please Enter the " + paty + " Passport No.");

                        showerralert("Please Enter the " + paty + paxtypcnt + " Passport No.", "", "");
                        $("#ad_passportNo_" + Ids_split).val("");
                        $("#ad_passportNo_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if ((($("#ad_pass_ex_date_" + Ids_split).val() == null || $("#ad_pass_ex_date_" + Ids_split).val().trim() == "") && validate_PWD == "TRUE") || (($("#ad_pass_ex_date_" + Ids_split).val() == null || $("#ad_pass_ex_date_" + Ids_split).val().trim() == "") && ($("#ad_passportNo_" + Ids_split).val().trim() != ""))) {
                        //alert("Please Enter the " + paty + "  PassPort Exp. date");

                        showerralert("Please Enter the " + paty + paxtypcnt + "  PassPort Exp. date.", "", "");
                        $("#ad_pass_ex_date_" + Ids_split).val("");
                        $("#ad_pass_ex_date_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if ($("#ad_pass_ex_date_" + Ids_split).val() != null && $("#ad_pass_ex_date_" + Ids_split).val() != "") {
                        var ppexpdate = PassportExp($("#ad_pass_ex_date_" + Ids_split).val());
                        if (ppexpdate != true) {
                            showerralert("Please Enter the valid " + paty + paxtypcnt + " PassPort Exp. date.", "", "");
                            $("#ad_pass_ex_date_" + Ids_split).val("");
                            $("#ad_pass_ex_date_" + Ids_split).focus();
                            checkflag = true;
                            return false;
                        }

                    }
                    if (($("#ad_Cntry_" + Ids_split).val() == null || $("#ad_Cntry_" + Ids_split).val().trim() == "" || $("#ad_Cntry_" + Ids_split).val().trim() == "0") && $("#ad_passportNo_" + Ids_split).val().trim() != "") {
                        //alert("Please Select the " + paty + "  Passport Issued Country");
                        showerralert("Please Select the " + paty + paxtypcnt + "  Passport Issued Country.", "", "");
                        $("#ad_Cntry_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if ($("#hdn_producttype").val() == "RIYA" && $('body').data('segtype') == "I") {
                        if (($("#ad_nationality_" + Ids_split).val() == null || $("#ad_nationality_" + Ids_split).val().trim() == "" || $("#ad_nationality_" + Ids_split).val().trim() == "0") && $("#ad_passportNo_" + Ids_split).val().trim() != "") {
                            showerralert("Please Select the " + paty + paxtypcnt + "  Nationality", "", "");
                            $("#ad_nationality_" + Ids_split).focus();
                            checkflag = true;
                            return false;
                        }
                        else {
                            Nationality = $("#ad_nationality_" + Ids_split).val();
                        }
                    }
                }
                else if ($("#hdn_producttype").val() != "JOD" && $("#hdn_producttype").val() != "RIYA" && $("#hdn_AppHosting").val() != "BSA") {
                    if (($("#ad_passportNo_" + Ids_split).val() == null || $("#ad_passportNo_" + Ids_split).val().trim() == "") && validate_PWD == "TRUE") {
                        //alert("Please Enter the " + paty + " Passport No.");

                        showerralert("Please Enter the " + paty + paxtypcnt + " Passport No.", "", "");
                        $("#ad_passportNo_" + Ids_split).val("");
                        $("#ad_passportNo_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if ((($("#ad_pass_ex_date_" + Ids_split).val() == null || $("#ad_pass_ex_date_" + Ids_split).val().trim() == "") && validate_PWD == "TRUE") || (($("#ad_pass_ex_date_" + Ids_split).val() == null || $("#ad_pass_ex_date_" + Ids_split).val().trim() == "") && ($("#ad_passportNo_" + Ids_split).val().trim() != ""))) {
                        //alert("Please Enter the " + paty + "  PassPort Exp. date");

                        showerralert("Please Enter the " + paty + paxtypcnt + "  PassPort Exp. date.", "", "");
                        $("#ad_pass_ex_date_" + Ids_split).val("");
                        $("#ad_pass_ex_date_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if ($("#ad_pass_ex_date_" + Ids_split).val() != null && $("#ad_pass_ex_date_" + Ids_split).val() != "") {
                        var ppexpdate = PassportExp($("#ad_pass_ex_date_" + Ids_split).val());
                        if (ppexpdate != true) {
                            showerralert("Please Enter the valid " + paty + paxtypcnt + " PassPort Exp. date.", "", "");
                            $("#ad_pass_ex_date_" + Ids_split).val("");
                            $("#ad_pass_ex_date_" + Ids_split).focus();
                            checkflag = true;
                            return false;
                        }

                    }
                    if (($("#ad_Cntry_" + Ids_split).val() == null || $("#ad_Cntry_" + Ids_split).val().trim() == "" || $("#ad_Cntry_" + Ids_split).val().trim() == "0") && $("#ad_passportNo_" + Ids_split).val().trim() != "") {
                        //alert("Please Select the " + paty + "  Passport Issued Country");
                        showerralert("Please Select the " + paty + paxtypcnt + "  Passport Issued Country.", "", "");
                        $("#ad_Cntry_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if (($("#hdnPassmand").val() == "TRUE" && Producttype == "EGY1") && ($("#ad_pass_issue_date_" + Ids_split).val() == null || $("#ad_pass_ex_date_" + Ids_split).val() == "")) {
                        showerralert("Please Enter the valid " + paty + paxtypcnt + " PassPort Issued date.", "", "");
                        $("#ad_pass_issue_date_" + Ids_split).val("");
                        $("#ad_pass_issue_date_" + Ids_split).focus();
                        checkflag = true;
                        return false;

                    }
                    else if (($("#hdnPassmand").val() == "TRUE" && Producttype == "EGY1" && $('body').data('segtype') != "D")) {
                        pass_issue_date = $("#ad_pass_issue_date_" + Ids_split).val().trim();
                    }
                }
                if ($("#hdn_producttype").val() == "RIYA") {

                    if ($("#hdn_allowetravel").val() == "1" && $("#hdn_trip").val() != "M" && $('body').data('segtype') == "D" && $("#bajaj_confirmarion")[0].checked == true) {
                        if (($("#ad_Nominee_" + Ids_split).val() == null || $("#ad_Nominee_" + Ids_split).val().trim() == "")) {
                            showerralert("Please enter the" + paty + paxtypcnt + " nominee details", "", "");
                            $("#ad_Nominee_" + Ids_split).focus();
                            checkflag = true;
                            return false;
                        }
                        else {
                            adt_nomineedet = $("#ad_Nominee_" + Ids_split).val();
                        }

                        var Paxyr = calculateageinsurance($("#ad_DOB_" + Ids_split).val(), pax_type_det.toUpperCase());
                        if (Paxyr < 0.5 || Paxyr > 70) {
                            showerralert("This insurance is applicable to passengers from 6 months to 70 years", "", "");
                            checkflag = true;
                            return false;
                        }
                    }
                }

                var SSR = ""
                if ($("#hdnadtMeals_" + Ids_split).val() != null && $("#hdnadtMeals_" + Ids_split).val().trim() != "") {
                    SSR = $("#hdnadtMeals_" + Ids_split).val();
                }


                paxtype = paty;
                title = $("#ad_Title_" + Ids_split).val() != null ? $("#ad_Title_" + Ids_split).val().trim() : "";
                firstName = $("#ad_firstName_" + Ids_split).val().trim();
                lastName = $("#ad_lastName_" + Ids_split).val().trim();
                gender = $("#ad_gender_" + Ids_split).val().trim();
                DOB = $("#ad_DOB_" + Ids_split).val().trim();
                passportNo = $("#ad_passportNo_" + Ids_split).val().trim();
                pass_ex_date = $("#ad_pass_ex_date_" + Ids_split).val().trim();
                IssCon = $("#ad_Cntry_" + Ids_split).val();
                //InfantRef = "";
                SSR = SSR;

                var otherssr_pre = "";
                if (Other_bagout.length > 0) {
                    for (var i = 0; i < Other_bagout.length; i++) {
                        if (Other_bagout[i] != null && Other_bagout[i] != "") {
                            var aryotherssr = Other_bagout[i].split('WEbMeaLWEB');
                            if (aryotherssr[6] == parseFloat(Ids_split) + 1) {
                                otherssr_pre += Other_bagout[i] + "~";
                            }
                        }
                    }
                }
                if (Other_SSRPCIN.length > 0) {
                    for (var i = 0; i < Other_SSRPCIN.length; i++) {
                        if (Other_SSRPCIN[i] != null && Other_SSRPCIN[i] != "") {
                            var aryotherssr = Other_SSRPCIN[i].split('WEbMeaLWEB');
                            if (aryotherssr[6] == parseFloat(Ids_split) + 1) {
                                otherssr_pre += Other_SSRPCIN[i] + "~";
                            }
                        }
                    }
                }
                if (Other_SSRSPMax.length > 0) {
                    for (var i = 0; i < Other_SSRSPMax.length; i++) {
                        if (Other_SSRSPMax[i] != null && Other_SSRSPMax[i] != "") {
                            var aryotherssr = Other_SSRSPMax[i].split('WEbMeaLWEB');
                            if (aryotherssr[6] == parseFloat(Ids_split) + 1) {
                                otherssr_pre += Other_SSRSPMax[i] + "~";
                            }
                        }
                    }
                }
                var book_seatarr = "";
                var othersselect = "";
                var spicevalue = "";
                var citysplit = "";
                var Tcity = "";
                var primevalidate = "";
                var seatmaplength = book_seatx.length;
                if (seatmaplength != "" && seatmaplength != null) {
                    for (var i = 0; i < seatmaplength; i++) {
                        if (book_seatx[i] != "" && book_seatx[i] != null) {
                            if (book_seatx[i].split('~')[3] != null && book_seatx[i].split('~')[3] != "" && book_seatx[i].split('~')[3] != "Not Selected") {
                                book_seatarr += book_seatx[i] + "$";
                            } else if (book_seatx[i].split('~')[3] == "Not Selected") {
                                book_seatarr += "$";
                            }
                        }
                    }

                }

                if ($("#hdn_bookingprev").val() == "Y") {
                    mealbaggapreifor(SSR, Ids_split, "Adult", otherssr_pre, book_seatarr, "ad");
                }
                TotalPax = title + "SPLITSCRIPT" + firstName + "SPLITSCRIPT" + lastName + "SPLITSCRIPT" + gender + "SPLITSCRIPT" + DOB + "SPLITSCRIPT" + passportNo + "SPLITSCRIPT" + pass_ex_date + "SPLITSCRIPT" + IssCon + "SPLITSCRIPT" + paxtype + "SPLITSCRIPT" + InfantRef + "SPLITSCRIPT" + SSR + "SPLITSCRIPT" + adt_nomineedet + "SPLITSCRIPT" + Nationality + "SPLITSCRIPT" + pass_issue_date;
                if (i == 1) {
                    FirstPax = TotalPax;
                }
                document.getElementById('hdnPax').value += TotalPax + "|"
            });

            TotalPax = "";
            paxtypcnt = 0;
            $("#passengersdetails .commChdpaxcls").each(function () {

                var Nationality = "";
                if (checkflag == true) {
                    return false;
                }
                checkflag = false;
                i++;
                paxtypcnt++;
                Ids_split = this.id.split("_")[1];

                pax_type_det = $("#ch_type_" + Ids_split).data("paxvalue").split(':')[0];

                paty = $("#ch_type_" + Ids_split).data("paxvalue").split(':')[0];
                if ($("#ch_Title_" + Ids_split).val() == null || $("#ch_Title_" + Ids_split).val() == "0") {
                    showerralert("Please Enter Child" + paxtypcnt + " Title.", "", "");
                    $("#ch_Title_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if ($("#ch_firstName_" + Ids_split).val() == null || $("#ch_firstName_" + Ids_split).val().trim() == "") {
                    showerralert("Please Enter Child" + paxtypcnt + " First name.", "", "");
                    $("#ch_firstName_" + Ids_split).val("");
                    $("#ch_firstName_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if (nameReg.test($("#ch_firstName_" + Ids_split).val())) {
                    showerralert("Please Enter Valid Child" + paxtypcnt + " First name.", "", "");
                    $("#ch_firstName_" + Ids_split).val("");
                    $("#ch_firstName_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if (($("#hdn_lastnamemand").val() == "" || $("#hdn_lastnamemand").val() == "1") && ($("#ch_lastName_" + Ids_split).val() == null || $("#ch_lastName_" + Ids_split).val().trim() == "")) {
                    showerralert("Please Enter Child" + paxtypcnt + " Last Name.", "", "");
                    $("#ch_lastName_" + Ids_split).val("");
                    $("#ch_lastName_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if (($("#hdn_lastnamemand").val() == "" || $("#hdn_lastnamemand").val() == "1") && nameReg.test($("#ch_lastName_" + Ids_split).val())) {
                    showerralert("Please Enter Valid Child" + paxtypcnt + " Last name.", "", "");
                    $("#ch_lastName_" + Ids_split).val("");
                    $("#ch_lastName_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if ($("#ch_gender_" + Ids_split).val() == null || $("#ch_gender_" + Ids_split).val().trim() == "" || $("#ch_gender_" + Ids_split + ' option:selected').text().trim() == "Select") {
                    //alert("Please Select Child Gender");
                    showerralert("Please Select Child" + paxtypcnt + " Gender.", "", "");
                    $("#ch_gender_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if (($("#ch_DOB_" + Ids_split).val() == null || $("#ch_DOB_" + Ids_split).val().trim() == "") && validate_DOB == "TRUE") {// || (($("#ch_passportNo_" + Ids_split).val() == null || $("#ch_passportNo_" + Ids_split).val().trim() == "") && $("#ch_passportNo_" + Ids_split).val().trim() != "")
                    //alert("Please Enter the " + paty + " DOB");
                    showerralert("Please Enter the " + paty + paxtypcnt + " DOB.", "", "");
                    $("#ch_DOB_" + Ids_split).val("");
                    $("#ch_DOB_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }

                if ($("#ch_DOB_" + Ids_split).val() != null && $("#ch_DOB_" + Ids_split).val() != "") {
                    Paxtyp = Calculateage($("#ch_DOB_" + Ids_split).val(), "CHILD");
                    if (Paxtyp == false) {
                        showerralert("Please Enter the valid " + paty + paxtypcnt + " DOB.", "", "");
                        $("#ch_DOB_" + Ids_split).val("");
                        $("#ch_DOB_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                }

                if (($("#hdn_producttype").val() == "JOD" || $("#hdn_producttype").val() == "RIYA" || $("#hdn_AppHosting").val() == "BSA") && $('body').data('segtype') != "D") {
                    if (($("#ch_passportNo_" + Ids_split).val() == null || $("#ch_passportNo_" + Ids_split).val().trim() == "")) {// && validate_PWD == "TRUE"
                        //alert("Please Enter the " + paty + " Passport No.");
                        showerralert("Please Enter the " + paty + paxtypcnt + " Passport No.", "", "");
                        $("#ch_passportNo_" + Ids_split).val("");
                        $("#ch_passportNo_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if (($("#ch_pass_ex_date_" + Ids_split).val() == null || $("#ch_pass_ex_date_" + Ids_split).val().trim() == "")) {// && validate_PWD == "TRUE" || (($("#ch_pass_ex_date_" + Ids_split).val() == null || $("#ch_pass_ex_date_" + Ids_split).val().trim() == "") && $("#ch_passportNo_" + Ids_split).val().trim() != "")
                        //alert("Please Enter the " + paty + "  PassPort Exp. date");
                        showerralert("Please Enter the " + paty + paxtypcnt + "  PassPort Exp. date.", "", "");
                        $("#ch_pass_ex_date_" + Ids_split).val("");
                        $("#ch_pass_ex_date_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if ($("#ch_pass_ex_date_" + Ids_split).val() != null && $("#ch_pass_ex_date_" + Ids_split).val() != "") {
                        var ppexpdate = PassportExp($("#ch_pass_ex_date_" + Ids_split).val());
                        if (ppexpdate != true) {
                            showerralert("Please Enter the valid " + paty + paxtypcnt + " PassPort Exp. date.", "", "");
                            $("#ch_pass_ex_date_" + Ids_split).val("");
                            $("#ch_pass_ex_date_" + Ids_split).focus();
                            checkflag = true;
                            return false;
                        }

                    }
                    if (($("#ch_Cntry_" + Ids_split).val() == null || $("#ch_Cntry_" + Ids_split).val().trim() == "" || $("#ch_Cntry_" + Ids_split).val().trim() == "0")) {// && validate_PWD == "TRUE" && $("#ch_passportNo_" + Ids_split).val().trim() != ""
                        //alert("Please Select the " + paty + "  Passport Issued Country");
                        showerralert("Please Select the " + paty + paxtypcnt + "  Passport Issued Country.", "", "");
                        $("#ch_Cntry_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if ($("#hdn_producttype").val() == "RIYA" && $('body').data('segtype') == "I") {
                        if (($("#ch_nationality_" + Ids_split).val() == null || $("#ch_nationality_" + Ids_split).val().trim() == "" || $("#ch_nationality_" + Ids_split).val().trim() == "0")) {
                            showerralert("Please Select the " + paty + paxtypcnt + "  Nationality", "", "");
                            $("#ch_nationality_" + Ids_split).focus();
                            checkflag = true;
                            return false;
                        }
                        else {
                            Nationality = $("#ch_nationality_" + Ids_split).val();
                        }
                    }
                }
                else if ($("#hdn_producttype").val() != "JOD" && $("#hdn_producttype").val() != "RIYA" && $("#hdn_AppHosting").val() != "BSA") {
                    if (($("#ch_passportNo_" + Ids_split).val() == null || $("#ch_passportNo_" + Ids_split).val().trim() == "")) {// && validate_PWD == "TRUE"
                        //alert("Please Enter the " + paty + " Passport No.");
                        showerralert("Please Enter the " + paty + paxtypcnt + " Passport No.", "", "");
                        $("#ch_passportNo_" + Ids_split).val("");
                        $("#ch_passportNo_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if (($("#ch_pass_ex_date_" + Ids_split).val() == null || $("#ch_pass_ex_date_" + Ids_split).val().trim() == "")) {// && validate_PWD == "TRUE" || (($("#ch_pass_ex_date_" + Ids_split).val() == null || $("#ch_pass_ex_date_" + Ids_split).val().trim() == "") && $("#ch_passportNo_" + Ids_split).val().trim() != "")
                        //alert("Please Enter the " + paty + "  PassPort Exp. date");
                        showerralert("Please Enter the " + paty + paxtypcnt + "  PassPort Exp. date.", "", "");
                        $("#ch_pass_ex_date_" + Ids_split).val("");
                        $("#ch_pass_ex_date_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if ($("#ch_pass_ex_date_" + Ids_split).val() != null && $("#ch_pass_ex_date_" + Ids_split).val() != "") {
                        var ppexpdate = PassportExp($("#ch_pass_ex_date_" + Ids_split).val());
                        if (ppexpdate != true) {
                            showerralert("Please Enter the valid " + paty + paxtypcnt + " PassPort Exp. date.", "", "");
                            $("#ch_pass_ex_date_" + Ids_split).val("");
                            $("#ch_pass_ex_date_" + Ids_split).focus();
                            checkflag = true;
                            return false;
                        }

                    }
                    if (($("#ch_Cntry_" + Ids_split).val() == null || $("#ch_Cntry_" + Ids_split).val().trim() == "" || $("#ch_Cntry_" + Ids_split).val().trim() == "0")) {// && validate_PWD == "TRUE" && $("#ch_passportNo_" + Ids_split).val().trim() != ""
                        //alert("Please Select the " + paty + "  Passport Issued Country");
                        showerralert("Please Select the " + paty + paxtypcnt + "  Passport Issued Country.", "", "");
                        $("#ch_Cntry_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if (($("#hdnPassmand").val() == "TRUE" && Producttype == "EGY1") && ($("#ch_pass_issue_date_" + Ids_split).val() == null || $("#ch_pass_issue_date_" + Ids_split).val() == "")) {
                        showerralert("Please Enter the valid " + paty + paxtypcnt + " PassPort Issued date.", "", "");
                        $("#ch_pass_issue_date_" + Ids_split).val("");
                        $("#ch_pass_issue_date_" + Ids_split).focus();
                        checkflag = true;
                        return false;

                    }
                    else if (($("#hdnPassmand").val() == "TRUE" && Producttype == "EGY1" && $('body').data('segtype') != "D")) {
                        pass_issue_date = $("#ch_pass_issue_date_" + Ids_split).val().trim();
                    }
                }
                if ($("#hdn_producttype").val() == "RIYA") {

                    if ($("#hdn_allowetravel").val() == "1" && $("#hdn_trip").val() != "M" && $('body').data('segtype') == "D" && $("#bajaj_confirmarion")[0].checked == true) {
                        if (($("#ch_Nominee_" + Ids_split).val() == null || $("#ch_Nominee_" + Ids_split).val().trim() == "")) {
                            showerralert("Please enter the" + paty + paxtypcnt + " nominee details", "", "");
                            $("#ch_Nominee_" + Ids_split).focus();
                            checkflag = true;
                            return false;
                        }
                        else {
                            chd_nomineedet = $("#ch_Nominee_" + Ids_split).val();
                        }
                    }
                }


                var SSR = ""
                if ($("#hdnchdMeals_" + Ids_split).val() != null && $("#hdnchdMeals_" + Ids_split).val().trim() != "") {
                    SSR = $("#hdnchdMeals_" + Ids_split).val();
                }

                paxtype = paty;
                title = $("#ch_Title_" + Ids_split).val() != null ? $("#ch_Title_" + Ids_split).val().trim() : "";
                firstName = $("#ch_firstName_" + Ids_split).val().trim();
                lastName = $("#ch_lastName_" + Ids_split).val().trim();
                gender = $("#ch_gender_" + Ids_split).val().trim();
                DOB = $("#ch_DOB_" + Ids_split).val().trim();
                passportNo = $("#ch_passportNo_" + Ids_split).val().trim();
                pass_ex_date = $("#ch_pass_ex_date_" + Ids_split).val().trim();
                IssCon = $("#ch_Cntry_" + Ids_split).val();
                // InfantRef = "";
                SSR = SSR;


                var otherssr_pre = "";
                if (Other_bagout.length > 0) {
                    for (var i = 0; i < Other_bagout.length; i++) {
                        if (Other_bagout[i] != null && Other_bagout[i] != "") {
                            var aryotherssr = Other_bagout[i].split('WEbMeaLWEB');
                            if (aryotherssr[6] == parseFloat(adtcount) + parseFloat(Ids_split) + 1) {
                                otherssr_pre += Other_bagout[i] + "~";
                            }
                        }
                    }
                }
                if (Other_SSRPCIN.length > 0) {
                    for (var i = 0; i < Other_SSRPCIN.length; i++) {
                        if (Other_SSRPCIN[i] != null && Other_SSRPCIN[i] != "") {
                            var aryotherssr = Other_SSRPCIN[i].split('WEbMeaLWEB');
                            if (aryotherssr[6] == parseFloat(adtcount) + parseFloat(Ids_split) + 1) {
                                otherssr_pre += Other_SSRPCIN[i] + "~";
                            }
                        }
                    }
                }
                if (Other_SSRSPMax.length > 0) {
                    for (var i = 0; i < Other_SSRSPMax.length; i++) {
                        if (Other_SSRSPMax[i] != null && Other_SSRSPMax[i] != "") {
                            var aryotherssr = Other_SSRSPMax[i].split('WEbMeaLWEB');
                            if (aryotherssr[6] == parseFloat(adtcount) + parseFloat(Ids_split) + 1) {
                                otherssr_pre += Other_SSRSPMax[i] + "~";
                            }
                        }
                    }
                }
                var book_seatarr = "";

                var seatmaplength = book_seatx.length;
                if (seatmaplength != "" && seatmaplength != null) {
                    for (var i = 0; i < seatmaplength; i++) {
                        if (book_seatx[i] != "" && book_seatx[i] != null) {
                            if (book_seatx[i].split('~')[3] != null && book_seatx[i].split('~')[3] != "" && book_seatx[i].split('~')[3] != "Not Selected") {
                                book_seatarr += book_seatx[i] + "$";
                            } else if (book_seatx[i].split('~')[3] == "Not Selected") {
                                book_seatarr += "$";
                            }
                        }
                    }
                }

                if ($("#hdn_bookingprev").val() == "Y") {
                    mealbaggapreifor(SSR, Ids_split, "Child", otherssr_pre, book_seatarr, "ch");
                }
                TotalPax = title + "SPLITSCRIPT" + firstName + "SPLITSCRIPT" + lastName + "SPLITSCRIPT" + gender + "SPLITSCRIPT" + DOB + "SPLITSCRIPT" + passportNo + "SPLITSCRIPT" + pass_ex_date + "SPLITSCRIPT" + IssCon + "SPLITSCRIPT" + paxtype + "SPLITSCRIPT" + InfantRef + "SPLITSCRIPT" + SSR + "SPLITSCRIPT" + chd_nomineedet + "SPLITSCRIPT" + Nationality + "SPLITSCRIPT" + pass_issue_date;
                document.getElementById('hdnPax').value += TotalPax + "|"
            });



            TotalPax = "";
            paxtypcnt = 0;
            $("#passengersdetails .commInfpaxcls").each(function () {
                var Nationality = "";
                if (checkflag == true) {
                    return false;
                }
                checkflag = false;
                i++;
                paxtypcnt++;
                Ids_split = this.id.split("_")[1];

                pax_type_det = $("#in_type_" + Ids_split).data("paxvalue").split(':')[0];

                paty = $("#in_type_" + Ids_split).data("paxvalue").split(':')[0];
                if ($("#in_Title_" + Ids_split).val() == null || $("#in_Title_" + Ids_split).val() == "0") {
                    //alert("Please Enter Infant Title");
                    showerralert("Please Enter Infant" + paxtypcnt + " Title.", "", "");
                    $("#in_Title_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if ($("#in_firstName_" + Ids_split).val() == null || $("#in_firstName_" + Ids_split).val().trim() == "") {
                    //alert("Please Enter Infant First name");
                    showerralert("Please Enter Infant" + paxtypcnt + " First name.", "", "");
                    $("#in_firstName_" + Ids_split).val("");
                    $("#in_firstName_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if (nameReg.test($("#in_firstName_" + Ids_split).val())) {
                    showerralert("Please Enter Infant Child" + paxtypcnt + " First name.", "", "");
                    $("#in_firstName_" + Ids_split).val("");
                    $("#in_firstName_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if (($("#hdn_lastnamemand").val() == "" || $("#hdn_lastnamemand").val() == "1") && ($("#in_lastName_" + Ids_split).val() == null || $("#in_lastName_" + Ids_split).val().trim() == "")) {
                    //alert("Please Enter Infant Last Name");
                    showerralert("Please Enter Infant" + paxtypcnt + " Last Name.", "", "");
                    $("#in_lastName_" + Ids_split).val("");
                    $("#in_lastName_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if (($("#hdn_lastnamemand").val() == "" || $("#hdn_lastnamemand").val() == "1") && nameReg.test($("#in_lastName_" + Ids_split).val())) {
                    showerralert("Please Enter Valid Infant" + paxtypcnt + " Last name.", "", "");
                    $("#in_lastName_" + Ids_split).val("");
                    $("#in_lastName_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if ($("#in_gender_" + Ids_split).val() == null || $("#in_gender_" + Ids_split).val().trim() == "" || $("#in_gender_" + Ids_split + ' option:selected').text().trim() == "Select") {
                    //alert("Please Select Infant Gender");
                    showerralert("Please Select Infant" + paxtypcnt + " Gender.", "", "");
                    $("#in_gender_" + Ids_split).val("");
                    $("#in_gender_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }
                if (($("#in_DOB_" + Ids_split).val() == null || $("#in_DOB_" + Ids_split).val().trim() == "") && validate_DOB == "TRUE") {//&& validate_DOB == "TRUE" || (($("#in_passportNo_" + Ids_split).val() == null || $("#in_passportNo_" + Ids_split).val().trim() == "") && $("#in_passportNo_" + Ids_split).val().trim() != "")

                    showerralert("Please Enter the " + paty + paxtypcnt + " DOB.", "", "");
                    $("#in_DOB_" + Ids_split).val("");
                    $("#in_DOB_" + Ids_split).focus();
                    checkflag = true;
                    return false;
                }


                if ($("#in_DOB_" + Ids_split).val() != null && $("#in_DOB_" + Ids_split).val() != "") {
                    Paxtyp = Calculateage($("#in_DOB_" + Ids_split).val(), "INFANT");
                    if (Paxtyp == false) {
                        showerralert("Please Enter the valid " + paty + paxtypcnt + " DOB.", "", "");
                        $("#in_DOB_" + Ids_split).val("");
                        $("#in_DOB_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                }
                if (($("#hdn_producttype").val() == "JOD" || $("#hdn_producttype").val() == "RIYA" || $("#hdn_AppHosting").val() == "BSA") && $('body').data('segtype') != "D") {
                    if (($("#in_passportNo_" + Ids_split).val() == null || $("#in_passportNo_" + Ids_split).val().trim() == "")) {// && validate_PWD == "TRUE"
                        showerralert("Please Enter the " + paty + paxtypcnt + " Passport No.", "", "");
                        $("#in_passportNo_" + Ids_split).val("");
                        $("#in_passportNo_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if (($("#in_pass_ex_date_" + Ids_split).val() == null || $("#in_pass_ex_date_" + Ids_split).val().trim() == "")) {// && validate_PWD == "TRUE" || (($("#in_pass_ex_date_" + Ids_split).val() == null || $("#in_pass_ex_date_" + Ids_split).val().trim() == "") && $("#in_passportNo_" + Ids_split).val().trim() != "")
                        //alert("Please Enter the " + paty + "  PassPort Exp. date");
                        showerralert("Please Enter the " + paty + paxtypcnt + "  PassPort Exp. date.", "", "");
                        $("#in_pass_ex_date_" + Ids_split).val("");
                        $("#in_pass_ex_date_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if ($("#in_pass_ex_date_" + Ids_split).val() != null && $("#in_pass_ex_date_" + Ids_split).val() != "") {
                        var ppexpdate = PassportExp($("#in_pass_ex_date_" + Ids_split).val());
                        if (ppexpdate != true) {
                            showerralert("Please Enter the valid " + paty + paxtypcnt + " PassPort Exp. date.", "", "");
                            $("#in_pass_ex_date_" + Ids_split).val("");
                            $("#in_pass_ex_date_" + Ids_split).focus();
                            checkflag = true;
                            return false;
                        }
                    }

                    if (($("#in_Cntry_" + Ids_split).val() == null || $("#in_Cntry_" + Ids_split).val().trim() == "" || $("#in_Cntry_" + Ids_split).val().trim() == "0")) {// && validate_PWD == "TRUE" && $("#in_passportNo_" + Ids_split).val().trim() != ""
                        //alert("Please Select the " + paty + "  Passport Issued Country");
                        showerralert("Please Select the " + paty + paxtypcnt + "  Passport Issued Country.", "", "");
                        $("#in_Cntry_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if ($("#hdn_producttype").val() == "RIYA" && $('body').data('segtype') == "I") {
                        if (($("#in_nationality_" + Ids_split).val() == null || $("#in_nationality_" + Ids_split).val().trim() == "" || $("#in_nationality_" + Ids_split).val().trim() == "0")) {
                            showerralert("Please Select the " + paty + paxtypcnt + "  Nationality", "", "");
                            $("#in_nationality_" + Ids_split).focus();
                            checkflag = true;
                            return false;
                        }
                        else {
                            Nationality = $("#in_nationality_" + Ids_split).val();
                        }
                    }
                }
                else if ($("#hdn_producttype").val() != "JOD" && $("#hdn_producttype").val() != "RIYA" && $("#hdn_AppHosting").val() != "BSA") {
                    if (($("#in_passportNo_" + Ids_split).val() == null || $("#in_passportNo_" + Ids_split).val().trim() == "")) {// && validate_PWD == "TRUE"
                        showerralert("Please Enter the " + paty + paxtypcnt + " Passport No.", "", "");
                        $("#in_passportNo_" + Ids_split).val("");
                        $("#in_passportNo_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if (($("#in_pass_ex_date_" + Ids_split).val() == null || $("#in_pass_ex_date_" + Ids_split).val().trim() == "")) {// && validate_PWD == "TRUE" || (($("#in_pass_ex_date_" + Ids_split).val() == null || $("#in_pass_ex_date_" + Ids_split).val().trim() == "") && $("#in_passportNo_" + Ids_split).val().trim() != "")
                        //alert("Please Enter the " + paty + "  PassPort Exp. date");
                        showerralert("Please Enter the " + paty + paxtypcnt + "  PassPort Exp. date.", "", "");
                        $("#in_pass_ex_date_" + Ids_split).val("");
                        $("#in_pass_ex_date_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if ($("#in_pass_ex_date_" + Ids_split).val() != null && $("#in_pass_ex_date_" + Ids_split).val() != "") {
                        var ppexpdate = PassportExp($("#in_pass_ex_date_" + Ids_split).val());
                        if (ppexpdate != true) {
                            showerralert("Please Enter the valid " + paty + paxtypcnt + " PassPort Exp. date.", "", "");
                            $("#in_pass_ex_date_" + Ids_split).val("");
                            $("#in_pass_ex_date_" + Ids_split).focus();
                            checkflag = true;
                            return false;
                        }
                    }

                    if (($("#in_Cntry_" + Ids_split).val() == null || $("#in_Cntry_" + Ids_split).val().trim() == "" || $("#in_Cntry_" + Ids_split).val().trim() == "0")) {// && validate_PWD == "TRUE" && $("#in_passportNo_" + Ids_split).val().trim() != ""
                        //alert("Please Select the " + paty + "  Passport Issued Country");
                        showerralert("Please Select the " + paty + paxtypcnt + "  Passport Issued Country.", "", "");
                        $("#in_Cntry_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    if (($("#hdnPassmand").val() == "TRUE" && Producttype == "EGY1") && ($("#in_pass_issue_date_" + Ids_split).val() == null || $("#in_pass_issue_date_" + Ids_split).val() == "")) {
                        showerralert("Please Enter the valid " + paty + paxtypcnt + " PassPort Issued date.", "", "");
                        $("#in_pass_issue_date_" + Ids_split).val("");
                        $("#in_pass_issue_date_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    else if (($("#hdnPassmand").val() == "TRUE" && Producttype == "EGY1" && $('body').data('segtype') != "D")) {
                        pass_issue_date = $("#in_pass_issue_date_" + Ids_split).val().trim();
                    }
                }
                if ($("#hdn_producttype").val() == "RIYA") {

                    if ($("#hdn_allowetravel").val() == "1" && $("#hdn_trip").val() != "M" && $('body').data('segtype') == "D" && $("#bajaj_confirmarion")[0].checked == true) {
                        if (($("#in_Nominee_" + Ids_split).val() == null || $("#in_Nominee_" + Ids_split).val().trim() == "")) {
                            showerralert("Please enter the" + paty + paxtypcnt + " nominee details", "", "");
                            $("#in_Nominee_" + Ids_split).focus();
                            checkflag = true;
                            return false;
                        }
                        else {
                            inf_nomineedet = $("#in_Nominee_" + Ids_split).val();
                        }
                        var Paxyr = calculateageinsurance($("#in_DOB_" + Ids_split).val(), pax_type_det.toUpperCase());
                        if (Paxyr < 0.5 || Paxyr > 70) {
                            showerralert("This insurance is applicable to passengers from 6 months to 70 years", "", "");
                            checkflag = true;
                            return false;
                        }
                    }
                }


                if (paty.toUpperCase() == "INFANT") {
                    if (jQuery.inArray($("#ch_Infant_" + Ids_split).val(), InfantRefrence) != -1) {
                        //alert("Please select different carry infant");
                        showerralert("Please select different carry infant.", "", "");
                        $("#ch_Infant_" + Ids_split).focus();
                        checkflag = true;
                        return false;
                    }
                    else {
                        InfantRefrence.push($("#ch_Infant_" + Ids_split).val());
                        InfantRef = $("#ch_Infant_" + Ids_split).val();
                    }
                }
                var SSR = "";

                paxtype = paty;
                title = $("#in_Title_" + Ids_split).val() != null ? $("#in_Title_" + Ids_split).val().trim() : "";
                firstName = $("#in_firstName_" + Ids_split).val().trim();
                lastName = $("#in_lastName_" + Ids_split).val().trim();
                gender = $("#in_gender_" + Ids_split).val().trim();
                DOB = $("#in_DOB_" + Ids_split).val().trim();
                passportNo = $("#in_passportNo_" + Ids_split).val().trim();
                pass_ex_date = $("#in_pass_ex_date_" + Ids_split).val().trim();
                IssCon = $("#in_Cntry_" + Ids_split).val();
                // InfantRef = "";
                SSR = SSR;

                var otherssr_pre = "";
                var book_seatarr = "";

                var seatmaplength = book_seatx.length;
                if (seatmaplength != "" && seatmaplength != null) {
                    for (var i = 0; i < seatmaplength; i++) {
                        if (book_seatx[i] != "" && book_seatx[i] != null) {
                            if (book_seatx[i].split('~')[3] != null && book_seatx[i].split('~')[3] != "" && book_seatx[i].split('~')[3] != "Not Selected") {
                                book_seatarr += book_seatx[i] + "$";
                            } else if (book_seatx[i].split('~')[3] == "Not Selected") {
                                book_seatarr += "$";
                            }
                        }
                    }

                }
                if ($("#hdn_bookingprev").val() == "Y") {
                    mealbaggapreifor(SSR, Ids_split, "Infant", otherssr_pre, book_seatarr, "in");
                }
                //           0                          1                          2                        3                       4                       5                               6                           7                       8                           9                       10                        11                              12                         13   
                TotalPax = title + "SPLITSCRIPT" + firstName + "SPLITSCRIPT" + lastName + "SPLITSCRIPT" + gender + "SPLITSCRIPT" + DOB + "SPLITSCRIPT" + passportNo + "SPLITSCRIPT" + pass_ex_date + "SPLITSCRIPT" + IssCon + "SPLITSCRIPT" + paxtype + "SPLITSCRIPT" + InfantRef + "SPLITSCRIPT" + SSR + "SPLITSCRIPT" + inf_nomineedet + "SPLITSCRIPT" + Nationality + "SPLITSCRIPT" + pass_issue_date;

                document.getElementById('hdnPax').value += TotalPax + "|"
            });


            if (primessr.length > 0) {
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
                        checkflag = true;
                        return false;
                    }
                }
            }
            //
            if (Other_SSRSPMax.length > 0) {
                for (var paxre = 1; paxre <= Totpaxcount; paxre++) {
                    for (var o = 0; o < Other_SSRSPMax.length; o++) {
                        if (Other_SSRSPMax[o] != null && Other_SSRSPMax[o] != "") {
                            var aryotherssr = Other_SSRSPMax[o].split('WEbMeaLWEB');
                            if (aryotherssr[0].toUpperCase().indexOf("PRIM") > -1 && aryotherssr[6] == paxre) {

                                //MEAL
                                var mealchk = $('#' + strPaxArray[paxre - 1].hdnpax).val().split('~');
                                for (var j = 0; j < mealchk.length; j++) {
                                    if (mealchk[j] != "") {
                                        var Im = mealchk[j].split('SpLiTSSR')
                                        var Me = Im[0].split('WEbMeaLWEB')[0];
                                        var seg = Im[0].split('WEbMeaLWEB')[3];
                                        if (Me == "0" && j == parseInt(seg)) {
                                            showerralert("Prime SSR Choosen For the " + strPaxArray[paxre - 1].paxlabel + " " + aryotherssr[4] + "-" + aryotherssr[5] + ", Its mandatory to pick the Prime meal.", "", "");
                                            checkflag = true;
                                            return false;
                                        }
                                    }
                                }
                                //MEAL

                                //FFD
                                for (var t = 0; t < Other_SSRSPMax.length; t++) {
                                    if (Other_SSRSPMax[t] != null && Other_SSRSPMax[t] != "") {
                                        var ffdotherssr = Other_SSRSPMax[t].split('WEbMeaLWEB');
                                        if (ffdotherssr[0].toUpperCase().indexOf("FFWD") > -1 && ffdotherssr[6] == paxre) {//if (Other_SSRSPMax.indexOf(Other_SSRSPMax[o].replace("PRIM", "Fast Forward|FFWD")) > -1) {
                                            if (aryotherssr[4] == ffdotherssr[4] && aryotherssr[5] == ffdotherssr[5]) {
                                                var datashow = "Prime SSR Choosen For the " + strPaxArray[paxre - 1].paxlabel + " " + ffdotherssr[4] + "-" + ffdotherssr[5] + " Not allowed to pick the Fast Forward Services.";
                                                showerralert(datashow, "", "");
                                                checkflag = true;
                                                return false;
                                            }
                                        }
                                    }
                                }

                                var primeevalue = "";
                                if (book_seatx.length > 0) {
                                    for (var i = 0; i < book_seatx.length; i++) {
                                        if (book_seatx[i] != "" && book_seatx[i] != null) {
                                            if (book_seatx[i].split('~')[3] != null && book_seatx[i].split('~')[3] != "" && book_seatx[i].split('~')[3] != "Not Selected") {
                                                if (aryotherssr[4] == book_seatx[i].split('~')[7] && aryotherssr[5] == book_seatx[i].split('~')[8] && book_seatx[i].split('~')[0] == paxre) {
                                                    primeevalue = "1";
                                                }
                                            }
                                        }
                                    }
                                }
                                if (primeevalue == "") {
                                    var datashow = "Prime SSR Choosen For the " + strPaxArray[paxre - 1].paxlabel + " " + aryotherssr[4] + "-" + aryotherssr[5] + " Its mandatory to pick you are entitled to choose a seat.";
                                    showerralert(datashow, "", "");
                                    checkflag = true;
                                    return false;
                                }
                                //SEAT

                            }
                        }
                    }
                }
            }

            if (Other_SSRPCIN.length > 0) {
                for (var paxre = 1; paxre <= Totpaxcount; paxre++) {
                    for (var o = 0; o < Other_SSRPCIN.length; o++) {
                        if (Other_SSRPCIN[o] != null && Other_SSRPCIN[o] != "") {
                            var aryotherssr = Other_SSRPCIN[o].split('WEbMeaLWEB');
                            if (aryotherssr[0].toUpperCase().indexOf("SPICEMAX") > -1 && aryotherssr[6] == paxre) {
                                var spicevalue = "";
                                if (book_seatx.length > 0) {
                                    for (var i = 0; i < book_seatx.length; i++) {
                                        if (book_seatx[i] != "" && book_seatx[i] != null) {
                                            if (book_seatx[i].split('~')[3] != null && book_seatx[i].split('~')[3] != "" && book_seatx[i].split('~')[3] != "Not Selected") {
                                                if (aryotherssr[4] == book_seatx[i].split('~')[7] && aryotherssr[5] == book_seatx[i].split('~')[8] && book_seatx[i].split('~')[0] == paxre) {
                                                    spicevalue = "1";
                                                }
                                            }
                                        }
                                    }
                                }
                                if (spicevalue == "") {
                                    var datashow = "SpiceMax SSR Choosen For the " + strPaxArray[paxre - 1].paxlabel + " " + aryotherssr[4] + "-" + aryotherssr[5] + " Its mandatory to pick the SpiceMAX Extra Leg-room seat.";
                                    showerralert(datashow, "", "");
                                    checkflag = true;
                                    return false;
                                }
                                //SEAT
                            }
                        }
                    }
                }
            }
            if ($("#hdn_NeedPaxAddlInfo").val() == "Y" && $("#hdn_PaxAddlInfoData").val() == "") {
                showAddlPaxError("Passenger additional information details is mandatory for this booking.", "", "");
                PaxaddlinfoPopup("dvPaxAddlInfo");
                return false;
            }
            //
            if ($("#hdn_producttype").val() == "RIYA") {
                if ($("#hdn_allowetravel").val() == "1" && $('body').data('segtype') == "D" && $("#hdn_trip").val() != "M" && $("#bajaj_confirmarion")[0].checked == true && BlockTicket == false) {
                    BajajInsdetails = "";
                    if ($("#Load_location").val() == null || $("#Load_location").val() == "") {
                        showerralert("Please select the city", "", "");
                        return false;
                    }
                    else if ($("#txt_address").val() == null || $("#txt_address").val() == "") {
                        showerralert("Please enter address details", "", "");
                        return false;
                    }
                    else if ($("#txt_pincode").val() == null || $("#txt_pincode").val() == "") {
                        showerralert("Please enter pincode", "", "");
                        return false;
                    }
                    else {
                        BajajInsdetails = $("#Load_location").val() + "SplItInS" + $("#txt_address").val() + "SplItInS" + $("#txt_pincode").val();
                    }
                    if ($("#chkMore")[0].checked == false) {
                        showerralert("Please accept insurance terms and conditions and go ahead for booking", "", "");
                        return false;
                    }
                }
                else if ($("#hdn_allowetravel").val() == "1" && $('body').data('segtype') == "D" && $("#hdn_trip").val() != "M" && $("#bajaj_confirmarion")[0].checked == true && BlockTicket == true) {
                    showerralert("Travel Insurance not applicable for Block PNR", "", "");
                    return false;
                }
                if ($("#chkTermsAir")[0].checked == false) {
                    showerralert("Please accept terms and conditions and go ahead for booking", "", "");
                    return false;
                }
            }
            if ($("#hdn_AppHosting").val() == "BSA" && $("#chkTermsAir")[0].checked == false) {
                showerralert("Please accept terms and conditions and go ahead for booking", "", "");
                return false;
            }
            $("#hdn_bajaj").val(BajajInsdetails);
            var strflag = sessionStorage.getItem("CHECKREBOOK");
            if (BlockTicket == true) {
                $("#paymentoption").enabled;
                $(".Bkpaymentmode").prop('checked', false);
                $(".Pay-mode").css("visibility", "hidden");
                $("#btn_booking").val("Block Now");
            }
            else if (strflag == "YES") {
                $(".Bkpaymentmode").prop("disabled", true);
                $(".Pay-mode").css("visibility", "visible");
                $("#btn_booking").val("Pay Now");
            }
            else {
                $("#paymentoption").enabled;
                $(".Bkpaymentmode").prop('checked', false);
                $(".Pay-mode").css("visibility", "visible");
                $("#btn_booking").val("Pay Now");
            }
            $('.riyagstno').html($('#gst_regnumber').val());

            /*Bind Payment Options*/
            if ($("#hdn_producttype").val() == "RBOA") {

                var PaymentRights = ($("#hdnPaymode").val() != null && $("#hdnPaymode").val() != "") ? $("#hdnPaymode").val().trim() : "N|N|N|N"; //Topup|Credit|PG|Pass
                var Topupbal = $("#hdnTopupbal").val().trim();
                var Creditbal = $("#hdnCreditbal").val().trim();
                var Grandtotal = $("#spnFare").html();
                var Topupbranch = "N";//Topup branch rights
                var topupamt = "Y";//By default Topup amount higer than Grandtotal
                var Creditamt = "Y";//By default Credit amount higer than Grandtotal
                var flightcarrier = $("#hdnAircategory").val().trim();
                var paymentoption = PaymentRights.split("|");

                if (parseFloat(Grandtotal) > parseFloat(Topupbal)) {
                    topupamt = "N";
                    if (paymentoption[1] == "Y" && (parseFloat(Grandtotal) < parseFloat(Creditbal))) {
                        Topupbranch = "Y";
                    }
                    //else {
                    //    Creditamt = paymentoption[1];
                    //}
                }

                //if (topupamt == "N" && paymentoption[1] != "Y" && (paymentoption.length > 4 && paymentoption[5] == "N")) {
                //    showerralert("Due to Insufficient Balance unlabe to process booking,Please contact support team", "", "");
                //    return false;
                //}
                var ddlpaymentmode = "";

                var paymode = "";


                if ($("#hdnTerminaltype").val() == "T" && (paymentoption[0] == "Y" || paymentoption[1] == "Y") && (topupamt == "N")) {
                    if (flightcarrier == "LCC" && Topupbranch == "Y" && paymentoption[4] == "Y") {
                        paymode += '<div class="radio">';//Topup amount should be less and credit higher than grand total but LCC carrier
                        paymode += '<input id="Topups" class="Bkpaymentmode" name="radio" type="radio" value="R" onclick="assignpayment(this.value)">';
                        paymode += ' <label for="Topups" class="radio-label RDT" title="Topups">Topup Branch</label>';
                        paymode += ' </div>';

                        ddlpaymentmode = '<option value="R">Topup Branch</option>';
                    }
                    if (flightcarrier == "FSC" && paymentoption[1] == "Y") { // && Creditamt == "Y"
                        paymode += '<div class="radio">';
                        paymode += '<input id="Credits" class="Bkpaymentmode" name="radio" type="radio" value="C" onclick="assignpayment(this.value)">';
                        paymode += '<label for="Credits" class="radio-label RDT" title="Credits">Credit Account</label>';
                        paymode += '</div>';

                        ddlpaymentmode += '<option value="C">Credit</option>';
                    }
                    if (paymentoption.length > 4 && paymentoption[5] == "Y") {
                        paymode += '<div class="radio Pay-mode">';
                        paymode += '<input id="Payments" class="Bkpaymentmode" name="radio" type="radio" value="H" onclick="assignpayment(this.value)">';
                        paymode += '<label for="Payments" class="radio-label RDT" title="Payments">Cash</label>';
                        paymode += '</div>';
                        ddlpaymentmode += '<option value="H">Cash</option>';
                    }
                    if (paymode == "") {
                        showerralert("Due to Insufficient Balance unlabe to process booking,Please contact support team", "", "");
                        return false;
                    }
                }
                else {
                    Creditamt = flightcarrier == "FSC" ? "Y" : "N";
                    if (paymentoption[0] == "Y" && topupamt == "Y") {
                        paymode += '<div class="radio">';
                        paymode += '<input id="Topups" class="Bkpaymentmode" name="radio" type="radio" value="T" onclick="assignpayment(this.value)">';
                        paymode += '<label for="Topups" class="radio-label RDT" title="Topups">Topup Account</label>';
                        paymode += '</div>';
                        ddlpaymentmode += '<option value="T">Topup</option>';
                    }
                    if (paymentoption[1] == "Y" && Creditamt == "Y") { 
                        paymode += '<div class="radio">';
                        paymode += '<input id="Credits" class="Bkpaymentmode" name="radio" type="radio" value="C" onclick="assignpayment(this.value)">';
                        paymode += '<label for="Credits" class="radio-label RDT" title="Credits">Credit Account</label>';
                        paymode += '</div>';
                        ddlpaymentmode += '<option value="C">Credit</option>';
                    }
                    if (paymentoption[2] == "Y") {
                        paymode += '<div class="radio Pay-mode">';
                        paymode += '<input id="Payments" class="Bkpaymentmode" name="radio" type="radio" value="P" onclick="assignpayment(this.value)">';
                        paymode += '<label for="Payments" class="radio-label RDT" title="Payments">Payment Gateway</label>';
                        paymode += '</div>';
                        ddlpaymentmode += '<option value="P">Payment&nbsp;Gateway</option>';
                    }
                    if (paymentoption.length > 4 && paymentoption[5] == "Y") {
                        paymode += '<div class="radio Pay-mode">';
                        paymode += '<input id="Payments" class="Bkpaymentmode" name="radio" type="radio" value="H" onclick="assignpayment(this.value)">';
                        paymode += '<label for="Payments" class="radio-label RDT" title="Payments">Cash</label>';
                        paymode += '</div>';
                        ddlpaymentmode += '<option value="H">Cash</option>';
                    }
                }

                $("#dv_paymentoptions,#ddlPaymode").html("");
                if (paymode == "") {
                    showerralert("Your Booking request currently blocked.Please contact support team", "", "");
                    return false;
                }
                $("#dv_paymentoptions").html(paymode);
                $("#ddlPaymode").html(ddlpaymentmode);
            }
            if ($("#hdn_AllowBookReq_Only").val() == "Y") {
                var ddlpaymentmode = "";
                var paymode = "";
                paymode += '<div class="radio" style="display:none;">';
                paymode += '<input id="Topups" class="Bkpaymentmode" name="radio" type="radio" value="T" checked="checked">';
                paymode += '<label for="Topups" class="radio-label RDT" title="Topups">Topup Account</label>';
                paymode += '</div>';
                ddlpaymentmode += '<option value="T">Topup</option>';
                $("#dv_paymentoptions").html(paymode);
                $("#ddlPaymode").html(ddlpaymentmode);
                $("#btn_booking").val("Proceed");
            }

            if (checkflag == true) {
                return false;
            }


            PendingCheck(FirstPax, checkflag);

        }

        // BlockTicket = false;
    }
    catch (e) {
        console.log(e.message);
    }
}

function Calculateage(birthday, Paxtype) {
    //var re = /^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d+$/;
    var re = /^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d+$/;
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

function ValidateDate(dtValue) {
    var dtRegex = new RegExp(/\b\d{1,2}[\/]\d{1,2}[\/]\d{4}\b/);
    return dtRegex.test(dtValue);
}

function PassportExp(Expdat) {

    //var re = /^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d+$/;
    var re = /^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d+$/;
    var lblAge = '';
    var retValue = false;
    var currentDate = $("#hdn_currentdate").val();
    var day = currentDate.split('/');
    var day = day[0]
    var day2 = day[0]
    var month = day[1]
    var year = day[2]

    if (Expdat != '') {

        if (re.test(Expdat)) {
            ExpDate = new Date(Expdat);
            var newdate = moment().format("DD/MM/YYYY");
            var passexpdate = $("#hdn_arrvaldate")[0].value;
            var day = Expdat.split('/');
            var dateNow = currentDate.split('/');
            var a = moment([day[2], day[1] - 1, day[0]]);
            var b = moment([dateNow[2], dateNow[1] - 1, dateNow[0]]);
            var df = a.diff(b, 'days') // 1

            var passdade = passexpdate.split('/');
            var c = moment([passdade[2], passdade[1] - 1, passdade[0]]);
            var psf = c.diff(b, 'days') // 1
            if (df < psf) {
                retValue = false;
            }
            else { retValue = true; }
        }
        else {
            //showerralert("Date must be dd/mm/yyyy format!", "", "");
            retValue = false;
        }
    }
    return retValue;
}

function Validate() {
    var email = $('#txtEmailID');
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    if ($('#txtContactNo').val().trim() == "" && $('#txtEmailID').val().trim() == "") {
        showerralert("Please Enter Contact Number", "", "");
        $("#txtContactNo").focus();
        return;
    }
    if ($('#txtContactNo').val().trim() == "") {
        showerralert("Please Enter Contact Number.", "", "");
        $("#txtContactNo").focus();
        return;
    }

    if ($('#txtEmailID').val().trim() == "") {
        showerralert("Please enter email address.", "", "");
        $("#txtEmailID").focus();
        return;
    }


    if ($('#txtEmailID').val().trim() == "" && BookingcoordinatorDet.toUpperCase() != "Y") {
        showerralert("Please enter email address.", "", "");
        $("#txtEmailID").focus();
        return;
    }
    else if (email.val() != "") {
        if (filter.test(email.val()) == false) {
            showerralert("Please provide a valid email address.", "", "");
            email.focus();
            return false;
        }
    }


    var Contactlenght = document.getElementById("txtContactNo").value;
    var Agnmoblenght = document.getElementById("txt_AgnNo").value;
    var Homemoblenght = document.getElementById("txt_HomeNo").value;
    var Bussmoblenght = document.getElementById("txt_BusiNo").value;
    if (Contactlenght.length > 12 || Agnmoblenght.length > 12 || Homemoblenght.length > 12 || Bussmoblenght.length > 12) {
        //alert("Mobile No. exceed with 12 digits.Please enter valid Mobile No.");
        showerralert("Mobile No. exceed with 12 digits.Please enter valid Mobile No.", "", "");
        return false;
    }
    else if (Contactlenght.length != "" || Agnmoblenght.length != "" || Homemoblenght.length != "" || Bussmoblenght.length != "") {
        if (Contactlenght.length != "10" && Agnmoblenght.length != "10" && Homemoblenght.length != "10" && Bussmoblenght.length != "10") {
            //alert("Please enter any one 10 digits Contact Mobile No. (eg : 056XXXXX05)");
            showerralert("Please enter any one 10 digits Contact Mobile No. (eg : 056XXXXX05)", "", "");
            return false;
        }
    }
    return true;
}

function validatebooking(booktype) {

    if ($("#hdn_bookingprev").val() == "Y") {
        if ($('input[type=radio][class=Bkpaymentmode]:checked').length == 0) {
            alert("Please select Payment mode");
            return false;
        }
    }

    if ($("#hdn_producttype").val() == "RIYA" && $('body').data('Agent_Type') == "RI") {
        var Monumber = ""; //sts185
        var ClientAgenttype = $('body').data('Agent_Type'); //sts185
        var filtermo = /^[a-zA-Z]{2}[0-9]{2}[a-zA-Z]{3}[a-zA-Z0-9]{8}$/;

        if ($("#txtMonumber").val().trim() != null && $("#txtMonumber").val().trim() != "") {
            if (filtermo.test($('#txtMonumber').val())) {
                Monumber = $('#txtMonumber').val();
                checkmonumber(Monumber);
            } else {
                alert("Please Enter valid MO Number", "", "");
                return false;
            }
        }
        else {
            alert("Please Enter MO Number", "", "");
            return false;
        }
    } else {
        var strflag = sessionStorage.getItem("CHECKREBOOK");
        if (strflag == "YES") {
            SampleRebooking(booktype);
        }
        else {
            paymentTobooking(booktype, booktype);
        }
    }
}

function checkmonumber(Monumber, booktype) {

    var ClientID = "";
    var BranchID = "";
    var GroupID = "";

    if ($("#ddlclient").length > 0) {
        ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
        BranchID = sessionStorage.getItem('branchid') != null ? sessionStorage.getItem('branchid').trim() : "";
        GroupID = sessionStorage.getItem('Groupid') != null ? sessionStorage.getItem('Groupid').trim() : "";
    }

    $.ajax({
        type: "POST",
        url: MOnumbercheck,
        data: '{MONumber:"' + Monumber + '",ClientID:"' + ClientID + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            var Result_val = response.Result;
            if (Result_val[1] != null && Result_val[1] != "" && Result_val[1] != "0") {
                paymentTobooking(booktype);
            }
            else if (Result_val[1] == "0" && Result_val[0] != "") {
                alert(Result_val[0]);
                $("#txtMonumber").val("");
                $("#txtMonumber").focus();
                return false;
            }
            else {
                alert("Unable to validate MO Number ,Please contact support team");
                return false;
            }

        },
        error: function () {
            if (e.status == "500") {
                showerralert("An Internal Problem Occurred. Your Session will Expire.", "", "");
                window.top.location = sessionExb;
                return false;
            }
        }
    });
}

var strRebook = "";
function paymentTobooking(booktype) {

    var adultcnt = $("#passengersdetails .commAdtpaxcls").length;
    var childcnt = $("#passengersdetails .commChdpaxcls").length;
    var spicejetcont = sessionStorage.getItem("Paxrefid");


    var otherssr = "";


    if (Other_bagout.length > 0) {
        for (var i = 0; i < Other_bagout.length; i++) {
            otherssr += Other_bagout[i] + "~";
        }
    }
    if (Other_SSRPCIN.length > 0) {
        for (var i = 0; i < Other_SSRPCIN.length; i++) {
            otherssr += Other_SSRPCIN[i] + "~";
        }
    }
    if (Other_SSRSPMax.length > 0) {
        for (var i = 0; i < Other_SSRSPMax.length; i++) {
            otherssr += Other_SSRSPMax[i] + "~";
        }
    }

    var gstdeta = "";
    var passthrow = "";
    var ontourcode = "";
    var rttourcode = "";

    var Allow_Duplicatebooking = false;

    var gst_regnumber = document.getElementById("gst_regnumber").value;
    var gst_cmpnyname = document.getElementById("gst_cmpnyname").value;
    var gst_address = document.getElementById("gst_address").value;
    var gst_mailid = document.getElementById("gst_mailid").value;
    var gst_mobno = document.getElementById("gst_mobno").value;
    if ((gst_regnumber != null && gst_regnumber != "") && (gst_cmpnyname != null && gst_cmpnyname != "") && (gst_address != null && gst_address != "") && (gst_mailid != null && gst_mailid != "") && (gst_mobno != null && gst_mobno != "")) {

        gstdeta = gst_regnumber + "~" + gst_cmpnyname + "~" + gst_address + "~" + gst_mailid + "~" + gst_mobno;

    }

    if ($("#hdn_producttype").val() == "RIYA") {
        ontourcode += document.getElementById("onwrdtor").value + "|";
        if ($("#hdtxt_trip")[0].value == "R") {
            ontourcode += document.getElementById("onwrdtor").value + "|";
            rttourcode = document.getElementById("rtwrdtor").value;
        }
        var Contact_no = $("#txtContactNo").val();
        $("#txt_AgnNo").val(Contact_no);
        $("#txt_BusiNo").val(Contact_no);
        $("#txt_HomeNo").val(Contact_no);
    }

    if ($("#ddlPaymode").val() == "B") {

        var ddlCardType = document.getElementById("ddlCardType").value;
        var CardName = $("#ddlCardType option:selected").text();// document.getElementById("ddlCardType").text;
        var txtBTACardNumber = document.getElementById("txtBTACardNumber").value;
        var ddlExpiryMonth = document.getElementById("ddlExpiryMonth").value;
        var ddlExpiryYear = document.getElementById("ddlExpiryYear").value;
        var txtCVV = document.getElementById("txtCVV").value;

        if (document.getElementById("ddlCardType").value == "0") {
            alert("Please select card type");
            document.getElementById("ddlCardType").focus();
            return false;
        }
        if (document.getElementById("txtBTACardNumber").value == "" || document.getElementById("txtBTACardNumber").value == " " ||
            document.getElementById("txtBTACardNumber").value.trim().indexOf("  ") != -1) {
            alert("Please enter card number");
            document.getElementById("txtBTACardNumber").focus();
            return false;
        }
        else if (document.getElementById("ddlCardType").value == "AB") {
            if (document.getElementById("txtBTACardNumber").value.length == 16 ||
                document.getElementById("txtBTACardNumber").value.length == 15) {
            }
            else {
                alert("Please enter valid card number");
                document.getElementById("txtBTACardNumber").focus();
                return false;
            }
        }
        else if (document.getElementById("txtBTACardNumber").value.length != 16) {
            //document.getElementById("lblErrorBookPopup").innerHTML = "Please enter valid card number";
            alert("Please enter valid card number");
            document.getElementById("txtBTACardNumber").focus();
            return false;
        }
        if (document.getElementById("ddlExpiryMonth").value == "0") {
            //document.getElementById("lblErrorBookPopup").innerHTML = "Please select expiry month";
            alert("Please select expiry month");
            document.getElementById("ddlExpiryMonth").focus();
            return false;
        }
        if (document.getElementById("ddlExpiryYear").value == "0") {
            //document.getElementById("lblErrorBookPopup").innerHTML = "Please select expiry year";
            alert("Please select expiry year");
            document.getElementById("ddlExpiryYear").focus();
            return false;
        }


        passthrow = CardName + "~" + ddlCardType + "~" + txtBTACardNumber + "~" + ddlExpiryMonth + "~" + ddlExpiryYear + "~" + txtCVV;
    }



    //To Check Internet connection is available... by saranraj on 20170725...
    var InetFlg = checkconnection();
    if (!InetFlg) {
        alert("Please check connectivity.", "", "");
        return false;
    }
    //End...
    var Bargainfare = document.getElementById("expected_fare").value;

    if (booktype == "BF" && (Bargainfare == "0" || Bargainfare == "")) {
        $("#modal-bargain").modal("hide");
        alert("Please enter valid expected fare", "", "");
        return false;
    }


    var ContactDet = $("#txtTitle").val() + "SPLITSCRIPT" + $("#txtContactNo").val() + "SPLITSCRIPT" + $("#txtEmailID").val() + "SPLITSCRIPT" + $("#txtFirstName").val() + "SPLITSCRIPT" + $("#txtLastName").val() + "SPLITSCRIPT" + $("#txt_AlterEmail").val() + "SPLITSCRIPT" + $("#txtfareremark").val() + "SPLITSCRIPT" + $("#Txt_Emirates").val() + "SPLITSCRIPT" + $("#Txt_Location").val() + "SPLITSCRIPT" + $("#Countryname").val() + "SPLITSCRIPT" + $("#txtaddress").val() + "SPLITSCRIPT" + $("#txt_AgnNo").val() + "SPLITSCRIPT" + $("#txt_BusiNo").val() + "SPLITSCRIPT" + $("#txt_HomeNo").val() + "SPLITSCRIPT" + $("#txt_UserName").val();

    if ($("#hdn_producttype").val() == "RIYA") {
        callpreviewfunction();
    }

    var Paymentmode = $("#ddlPaymode").val();
    if (Paymentmode == null || Paymentmode == "" || Paymentmode == "0") {
        showerralert("Please select Payment Type", "", "");
        return false;
    }

    var strCashPaymentinfo = "";
    if (Paymentmode == "H") {

        if ($("#txtcash_contactno").val() == null || $("#txtcash_contactno").val() == undefined || $("#txtcash_contactno").val() == "") {
            showerralert("Please enter customer contact number.", "", "");
            return false;
        }
        if ($("#txtcash_email").val() == null || $("#txtcash_email").val() == undefined || $("#txtcash_email").val() == "") {
            showerralert("Please enter your email Id.", "", "");
            return false;
        }
        var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if ($("#txtcash_email").val() != "" && !filter.test($("#txtcash_email").val())) {
            showerralert("Please enter valid email Id.", "", "");
            return false;
        }
        if ($("#txtcash_EmpCode").val() == null || $("#txtcash_EmpCode").val() == undefined || $("#txtcash_EmpCode").val() == "") {
            showerralert("Please enter your employee code.", "", "");
            return false;
        }
        if ($("#hdncash_Empname").val() == "") {//$("#hdncash_Empbranch").val() == "" || $("#hdncash_Empemail").val() == "" || || $("#hdncash_Empmobile").val() == ""
            showerralert("Please enter valid employee Id.", "", "");
            return false;
        }

        if ($("#txtcash_Paymentdate").val() == null || $("#txtcash_Paymentdate").val() == undefined || $("#txtcash_Paymentdate").val() == "") {
            showerralert("Please select date of payment.", "", "");
            return false;
        }
        if ($("#txtcash_PaymentMethod").val() == null || $("#txtcash_PaymentMethod").val() == undefined || $("#txtcash_PaymentMethod").val() == "") {
            showerralert("Please select payment method.", "", "");
            return false;
        }
        if (($("#txtcash_pancard").val() == null || $("#txtcash_pancard").val() == undefined || $("#txtcash_pancard").val() == "")
            && (Number($("#totalAmnt").data("totalamntd")) > 50000)) {
            showerralert("Please enter customer Pancard number.", "", "");
            return false;
        }
        strCashPaymentinfo = $("#txtcash_contactno").val() + "SPLITCASH"        //0
                            + $("#txtcash_email").val() + "SPLITCASH"           //1
                            + $("#txtcash_Ref").val() + "SPLITCASH"             //2
                            + $("#txtcash_EmpCode").val() + "SPLITCASH"         //3
                            + $("#hdncash_Empname").val() + "SPLITCASH"         //4
                            + $("#hdncash_Empemail").val() + "SPLITCASH"        //5
                            + $("#hdncash_Empmobile").val() + "SPLITCASH"       //6
                            + $("#hdncash_Empbranch").val() + "SPLITCASH"       //7
                            + $("#txtcash_Paymentdate").val() + "SPLITCASH"     //8
                            + $("#txtcash_PaymentMethod").val() + "SPLITCASH"   //9
                            + $("#txtcash_pancard").val() + "SPLITCASH"         //10
                            + $("#txtcash_freetext").val() + "SPLITCASH";       //11
    }

    $("#modal-changepayment").modal("hide");
    $("#modal-bargain").modal("hide");




    var book_seatarr = "";

    var seatmaplength = book_seatx.length;
    if (seatmaplength != "" && seatmaplength != null) {
        for (var i = 0; i < seatmaplength; i++) {
            if (book_seatx[i] != "" && book_seatx[i] != null) {
                if (book_seatx[i].split('~')[3] != null && book_seatx[i].split('~')[3] != "" && book_seatx[i].split('~')[3] != "Not Selected") {
                    book_seatarr += book_seatx[i] + "$";
                } else if (book_seatx[i].split('~')[3] == "Not Selected") {
                    book_seatarr += "$";
                }
            }
        }
    }

    var Paxdetails = $("#hdnPax")[0].value;

    var Tour_code = $("#Tour_Code").val();
    var Queuee = $("#hdnqueue").val();
    var Appcurrency = $("#HDN_CURRENCY_code").val();

    var MailID = "";
    var MobileNumber = "";

    var ClientID = "";
    var BranchID = "";
    var GroupID = "";

    if ($("#ddlclient").length > 0) {
        ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
        BranchID = sessionStorage.getItem('branchid') != null ? sessionStorage.getItem('branchid').trim() : "";
        GroupID = sessionStorage.getItem('Groupid') != null ? sessionStorage.getItem('Groupid').trim() : "";
    }
    else {

        ClientID = "";
        BranchID = "";
        GroupID = "";
    }


    //var strRebook = "";
    var Rebookpnr = "false";
    var Reselect = "";

    if ($("#txtAirlinemailid").val() != null && $("#txtAirlinemailid").val() != "") {
        MailID = $("#txtAirlinemailid").val();
    }
    else {
        MailID = $("#txtEmailID").val();
    }
    if ($("#priority_mobileno").val() != null && $("#priority_mobileno").val() != "") {
        MobileNumber = $("#priority_mobileno").val();
    }
    else if ($("#txt_BusiNo").val() != null && $("#txt_BusiNo").val() != "") {
        MobileNumber = $("#txt_BusiNo").val();
    }
    else if ($("#txt_HomeNo").val() != null && $("#txt_HomeNo").val() != "") {
        MobileNumber = $("#txt_HomeNo").val();
    }
    else if ($("#txt_AgnNo").val() != null && $("#txt_AgnNo").val() != "") {
        MobileNumber = $("#txt_AgnNo").val();
    }
    else {
        MobileNumber = $("#txtContactNo").val();
    }

    var Monumber = "";
    if ($("#hdn_producttype").val() == "RIYA" && $('body').data('Agent_Type') == "RI") {
        //sts185
        var filtermo = /^[a-zA-Z]{2}[0-9]{2}[a-zA-Z]{3}[a-zA-Z0-9]{8}$/;
        if ($("#txtMonumber").val().trim() != null && $("#txtMonumber").val().trim() != "") {
            if (filtermo.test($('#txtMonumber').val())) {
                Monumber = $('#txtMonumber').val();
            } else {
                alert("Please Enter valid MO Number", "", "");
                return false;
            }
        }
        else {
            alert("Please Enter MO Number", "", "");
            return false;
        }
    }

    var BajajInsdetails = $("#hdn_bajaj").val();

    var multicity_dom_ss_lth = "";
    var multicity_dom_ss = "";
    if ($('body').data('segtype') == "D" && mulreq == "Y") {
        var multicity_dom_ss = multicity_dom.split('|');
        multicity_dom_ss_lth = multicity_dom_ss.length;
        $("#dvProgress").show();

        var RESULT = "1";
        for (var i = 0; i < multicity_dom_ss_lth - 1; i++) {
            try {

                var temp = true;
                var segment_type = $('body').data('segtype');
                var TKey = multicity_dom_ss != null && multicity_dom_ss.length > 0 ? multicity_dom_ss[i] : hidden_HDValKey;
                var searvicekey = hidden_hdntotalserv;
                // var strRebook = "";
                sessionStorage.setItem("rebookTKey", TKey);
                var Reselect = "";

                $.blockUI({
                    message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
                    css: { padding: '5px' }
                });
                if (RESULT == "1") {
                    RESULT = "0";
                    $.ajax({
                        type: "POST", 		//GET or POST or PUT or DELETE verb
                        url: bookingUrl, 		// Location of the service
                        async: false,
                        data: '{ContactDet: "' + ContactDet + '" ,PaxDet: "' + Paxdetails + '" ,BlockTicket: "' + BlockTicket
                            + '",PaymentMode: "' + $("#ddlPaymode").val() + '",TourCode: "' + $("#Tour_Code").val() + '",TKey: "' + TKey
                            + '",service:"' + searvicekey + '",queue:"' + $("#hdnqueue").val() + '",Seatvalue:"' + book_seatarr
                            + '",SegmentType:"' + segment_type + '",mulreq:"' + mulreq + '",Bargainfare:"' + Bargainfare
                            + '",booktype:"' + booktype + '",gstdeta:"' + gstdeta + '",otherssr:"' + otherssr
                            + '",Appcurrency:"' + $("#HDN_CURRENCY_code").val() + '",ReBook:"' + "false" + '",ClientID:"' + ClientID
                            + '",BranchID:"' + BranchID + '",GroupID:"' + GroupID + '",reqcont:"' + i + '",MailID:"' + MailID + '",MobileNumber:"' + MobileNumber
                            + '",passthrow:"' + passthrow + '",ontourcode:"' + ontourcode + '",rttourcode:"' + rttourcode + '",Insdetails:"' + BajajInsdetails + '", MONumber:"' + Monumber + '",AllowDuplicateBooking:"' + Allow_Duplicatebooking 
                            + '",CreditshellPNRDetails:"' + $("#hdn_CreditshellAmt").val() + '",strPaxAddlInfo:"' + $("#hdn_PaxAddlInfoData").val() + '",strCashPaymentDet:"' + strCashPaymentinfo + '"}',//,Appcurrency:"' + $("#HDN_CURRENCY_code").val() + '",strRebook:"' + strRebook + '",Reselect:"' + Reselect + '"
                        //  timeout: 200000,
                        contentType: "application/json; charset=utf-8",
                        dataType: "html",
                        success: function (json) {//On Successful service call
                            // $("#dvProgress").hide();
                            callpreviewfunction();
                            if (i == 0) {
                                $("#dvBookinsuccess").html("");
                            }

                            if (temp == true) {
                                if (json.indexOf("errordetails") != -1) {
                                    $("#dvBookinsuccess").append(json); //To get error message id's...
                                    multicityDomasticfun($('body').data('segtype'), mulreq, i, multicity_dom_ss.length - 1);
                                    showerralert($("#errormessage").html().trim(), "", "");

                                    if (($("#hdn_producttype").val() == "UAE") && document.getElementById('hdn_allowinsurance').value == "Y" || document.getElementById('hdn_allowinsurance').value == "1") {
                                        if (document.getElementById('insurance_confirmarion').checked == true) {
                                            if (sessionStorage.getItem("retvaldet") == "YES") {
                                                var tune_track_insert = "0";
                                                bookins(sessionStorage.getItem("insvalues"), $("#p_Airlinetrackid_tune").html().trim(), tune_track_insert);
                                            }
                                        }
                                    }
                                    $("#dvBookinsuccess").html("");
                                    return false;
                                }
                                else if (json.indexOf("sessionerrormessage") != -1) {

                                    $("#dvBookinsuccess").append(json);
                                    multicityDomasticfun($('body').data('segtype'), mulreq, i, multicity_dom_ss.length - 1);
                                    $('#PpopContnt').html($('#sessionerrormessage').html().trim());
                                    if (($("#hdn_producttype").val() == "UAE") && document.getElementById('hdn_allowinsurance').value == "Y" || document.getElementById('hdn_allowinsurance').value == "1") {
                                        if (document.getElementById('insurance_confirmarion').checked == true) {
                                            if (sessionStorage.getItem("retvaldet") == "YES") {
                                                var tune_track_insert = "0";
                                                bookins(sessionStorage.getItem("insvalues"), $("#p_Airlinetrackid_tune").html().trim(), tune_track_insert);
                                            }
                                        }
                                    }
                                    $("#modal-bkngsess").modal({
                                        backdrop: 'static',
                                        keyboard: false
                                    });
                                    return false;
                                }
                                else if (json.indexOf("alertbookingmsg") != -1) { /* Duplicate booking alert for Result code 8*/
                                    $("#dvBookinsuccess").append(json);
                                    var message = $("#alertbookingmsg").html();
                                    if (confirm(message)) {
                                        asyncafterbooking($('body').data('BookingResponse'), "8|YES");
                                        bookingflights(ContactDet, Paxdetails, BlockTicket, Paymentmode, Tour_code, TKey, Queuee, searvicekey, Appcurrency, book_seatarr, segment_type, otherssr, mulreq, Bargainfare, booktype, gstdeta, strRebook, "false", Reselect, "TRUE");
                                    }
                                    else {
                                        asyncafterbooking($('body').data('BookingResponse'), "8|NO");
                                    }
                                    return false;
                                }
                                    /* Duplicate booking alert for Result code 8*/
                                else if (json.indexOf("spnBookmessage") != -1) {
                                    $("#dvBookinsuccess").append(json); RESULT = "1";
                                    multicityDomasticfun($('body').data('segtype'), mulreq, i, multicity_dom_ss.length - 1);
                                    $("#dvBookinsuccess").css("display", "block");
                                    $("#dvSelectView").css("display", "none");
                                    $("#dvAvailView").css("display", "none");
                                    if (BlockTicket == true || booktype == "BF") {

                                        if (($("#hdn_producttype").val() == "UAE") && document.getElementById('hdn_allowinsurance').value == "Y" || document.getElementById('hdn_allowinsurance').value == "1") {
                                            if (document.getElementById('insurance_confirmarion').checked == true) {
                                                if (sessionStorage.getItem("retvaldet") == "YES") {
                                                    var tune_track_insert = "0";
                                                    bookins(sessionStorage.getItem("insvalues"), "", tune_track_insert);
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        if (($("#hdn_producttype").val() == "UAE") && document.getElementById('hdn_allowinsurance').value == "Y" || document.getElementById('hdn_allowinsurance').value == "1") {
                                            if (document.getElementById('insurance_confirmarion').checked == true) {
                                                if (sessionStorage.getItem("retvaldet") == "YES") {

                                                    var tune_track_insert = "1";
                                                    bookins(sessionStorage.getItem("insvalues"), "", tune_track_insert);
                                                }
                                            }
                                        }
                                    }

                                    return false;
                                }
                                else if (json.indexOf("hdnpaymentgateway_code") != -1) {

                                    window.location.href = $("#hdnpaymentgatewayurl").html().trim();
                                    return false;
                                }
                                else {
                                    var response = JSON.parse(json);
                                    if (response.Result[8] == "7") {
                                        var msg = "";
                                        strRebook = response.Result[9];
                                        msg += response.Result[3] != null && response.Result[3] != "" ? response.Result[3].toString() : response.Result[7];
                                        msg += "Are you sure you want to continue with ReBooking.....";
                                        if (confirm(msg)) {
                                            var responsejson = JSON.parse(response.Result[6]);
                                            $('body').data('BookingResponse', JSON.stringify(responsejson));
                                            asyncafterbooking($('body').data('BookingResponse'), "7|YES"); /* flag added by rajesh for track update with result code*/
                                            bookingflights(ContactDet, Paxdetails, BlockTicket, Paymentmode, Tour_code, TKey, Queuee, searvicekey, Appcurrency, book_seatarr, segment_type, otherssr, mulreq, Bargainfare, booktype, gstdeta, strRebook, "true", Reselect, "FALSE");
                                        }
                                        else {
                                            var responsejson = JSON.parse(response.Result[6]);
                                            $('body').data('BookingResponse', JSON.stringify(responsejson));
                                            asyncafterbooking($('body').data('BookingResponse'), "7|NO");/* flag added by rajesh for track update with result code*/
                                            document.getElementById("btnokBkngsessionExp").click();
                                        }
                                        return false;
                                    }
                                    if (response.Result[12] != null && response.Result[12] != "") {
                                        window.top.location.href = response.Result[12];
                                        return false;
                                    }
                                }
                            }
                            var result = json.Result[0];
                            if (result == "1") {
                                var pres = json.Result[2];
                                if (json.Result[2] === "P") {
                                    window.location = json.Result[1]
                                    $("#dvProgress").hide();
                                }
                                else {
                                    book_ticket(json.Result);
                                    $("#dvProgress").hide();
                                }
                            }
                            else if (result == "2") {
                                var msg = json.Result[3]
                                if (confirm(msg)) {
                                    Rebook("B");
                                }
                                else {
                                    LogDetails("User response : NO", e.status, "Flight ReeBook");
                                }
                            }
                            else if (result == "3") {
                                var msg = json.Result[3];
                                showerralert(msg, 5000, "");
                                document.getElementById("btnback").click();
                                return false;
                            }
                            else {
                                showerralert(result, 5000, "");
                            }
                            $("#dvProgress").hide();
                            $.unblockUI();
                        },
                        error: function (e) {// When Service call fails                           
                            LogDetails(e.responseText, e.status, "Flight Book");
                            if (e.statusText == "timeout") {
                                showerralert("Due to Internet slow the selected Operation is timed out. Please check booked history (or) search again.", "", "");
                            }
                        }
                    });
                }
            } catch (e) {
                // alert("sample");
                showerralert(e.message, 5000, "");
            }
        }
    }
    else {

        try {
            $("#dvProgress").show();
            var temp = true;
            var segment_type = $('body').data('segtype');
            var TKey = hidden_HDValKey;
            var searvicekey = hidden_hdntotalserv;
            var strRebook = "";
            var Reselect = "";
            sessionStorage.setItem("rebookTKey", TKey);
            $.ajax({
                type: "POST", 		//GET or POST or PUT or DELETE verb
                url: bookingUrl, 		// Location of the service
                data: '{ContactDet: "' + ContactDet + '" ,PaxDet: "' + Paxdetails + '" ,BlockTicket: "' + BlockTicket
                    + '",PaymentMode: "' + $("#ddlPaymode").val() + '",TourCode: "' + $("#Tour_Code").val() + '",TKey: "' + TKey
                    + '",service:"' + searvicekey + '",queue:"' + $("#hdnqueue").val() + '",Seatvalue:"' + book_seatarr
                    + '",SegmentType:"' + segment_type + '",mulreq:"' + mulreq + '",Bargainfare:"' + Bargainfare
                    + '",booktype:"' + booktype + '",gstdeta:"' + gstdeta + '",otherssr:"' + otherssr
                    + '",Appcurrency:"' + $("#HDN_CURRENCY_code").val() + '",ReBook:"' + "false" + '",ClientID:"' + ClientID
                    + '",BranchID:"' + BranchID + '",GroupID:"' + GroupID + '",reqcont:"' + i + '",MailID:"' + MailID + '",MobileNumber:"' + MobileNumber
                    + '",passthrow:"' + passthrow + '",ontourcode:"' + ontourcode + '",rttourcode:"' + rttourcode + '",Insdetails:"' + BajajInsdetails + '", MONumber:"' + Monumber + '",AllowDuplicateBooking:"' + Allow_Duplicatebooking
                    + '",CreditshellPNRDetails:"' + $("#hdn_CreditshellAmt").val() + '",strPaxAddlInfo:"' + $("#hdn_PaxAddlInfoData").val() + '",strCashPaymentDet:"' + strCashPaymentinfo + '"}',//,Appcurrency:"' + $("#HDN_CURRENCY_code").val() + '",strRebook:"' + strRebook + '",Reselect:"' + Reselect + '"
                //  timeout: 200000,
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                success: function (json) {//On Successful service call
                    $("#dvProgress").hide();
                    callpreviewfunction();
                    if (i == 0) {
                        $("#dvBookinsuccess").html("");
                    }

                    if (temp == true) {
                        if (json.indexOf("errordetails") != -1) {
                            $("#dvBookinsuccess").append(json); //To get error message id's...
                            multicityDomasticfun($('body').data('segtype'), mulreq, i, multicity_dom_ss.length - 1);
                            showerralert($("#errormessage").html().trim(), "", "");

                            if (($("#hdn_producttype").val() == "UAE") && document.getElementById('hdn_allowinsurance').value == "Y" || document.getElementById('hdn_allowinsurance').value == "1") {
                                if (document.getElementById('insurance_confirmarion').checked == true) {
                                    if (sessionStorage.getItem("retvaldet") == "YES") {
                                        var tune_track_insert = "0";
                                        bookins(sessionStorage.getItem("insvalues"), $("#p_Airlinetrackid_tune").html().trim(), tune_track_insert);
                                    }
                                }
                            }

                            $("#dvBookinsuccess").html("");
                            return false;
                        }
                        else if (json.indexOf("sessionerrormessage") != -1) {

                            $("#dvBookinsuccess").append(json);
                            multicityDomasticfun($('body').data('segtype'), mulreq, i, multicity_dom_ss.length - 1);
                            $('#PpopContnt').html($('#sessionerrormessage').html().trim());
                            if (($("#hdn_producttype").val() == "UAE") && document.getElementById('hdn_allowinsurance').value == "Y" || document.getElementById('hdn_allowinsurance').value == "1") {
                                if (document.getElementById('insurance_confirmarion').checked == true) {
                                    if (sessionStorage.getItem("retvaldet") == "YES") {
                                        var tune_track_insert = "0";
                                        bookins(sessionStorage.getItem("insvalues"), $("#p_Airlinetrackid_tune").html().trim(), tune_track_insert);
                                    }
                                }
                            }
                            $("#modal-bkngsess").modal({
                                backdrop: 'static',
                                keyboard: false
                            });
                            return false;
                        }
                        else if (json.indexOf("alertbookingmsg") != -1) { /* Duplicate booking alert for Result code 8*/
                            $("#dvBookinsuccess").append(json);
                            var message = $("#alertbookingmsg").html();
                            if (confirm(message)) {
                                asyncafterbooking($('body').data('BookingResponse'), "8|YES");
                                bookingflights(ContactDet, Paxdetails, BlockTicket, Paymentmode, Tour_code, TKey, Queuee, searvicekey, Appcurrency, book_seatarr, segment_type, otherssr, mulreq, Bargainfare, booktype, gstdeta, strRebook, "false", Reselect, "TRUE");
                            }
                            else {
                                asyncafterbooking($('body').data('BookingResponse'), "8|NO");
                            }
                            return false;
                        }
                            /* Duplicate booking alert for Result code 8*/
                        else if (json.indexOf("spnBookmessage") != -1) {
                            $("#dvBookinsuccess").append(json); RESULT = "1";
                            multicityDomasticfun($('body').data('segtype'), mulreq, i, multicity_dom_ss.length - 1);
                            $("#dvBookinsuccess").css("display", "block");
                            $("#dvSelectView").css("display", "none");
                            $("#dvAvailView").css("display", "none");
                            if (BlockTicket == true || booktype == "BF") {

                                if (($("#hdn_producttype").val() == "UAE") && document.getElementById('hdn_allowinsurance').value == "Y" || document.getElementById('hdn_allowinsurance').value == "1") {
                                    if (document.getElementById('insurance_confirmarion').checked == true) {
                                        if (sessionStorage.getItem("retvaldet") == "YES") {
                                            var tune_track_insert = "0";
                                            bookins(sessionStorage.getItem("insvalues"), "", tune_track_insert);
                                        }
                                    }
                                }
                            }
                            else {
                                if (($("#hdn_producttype").val() == "UAE") && document.getElementById('hdn_allowinsurance').value == "Y" || document.getElementById('hdn_allowinsurance').value == "1") {
                                    if (document.getElementById('insurance_confirmarion').checked == true) {
                                        if (sessionStorage.getItem("retvaldet") == "YES") {

                                            var tune_track_insert = "1";
                                            bookins(sessionStorage.getItem("insvalues"), "", tune_track_insert);
                                        }
                                    }
                                }
                            }

                            return false;
                        }
                        else if (json.indexOf("hdnpaymentgateway_code") != -1) {

                            window.location.href = $("#hdnpaymentgatewayurl").html().trim();
                            return false;
                        }
                        else {
                            var response = JSON.parse(json);
                            if (response.Result[8] == "7") {
                                var msg = "";
                                sessionStorage.setItem("CHECKREBOOK", "YES");
                                strRebook = response.Result[9];
                                var sampinfo = JSON.parse(strRebook);
                                var samp = sampinfo.PriceItenarys[0].PRS;
                                if (samp.length > 0) {
                                    var buildresult = rebookinfo(samp);
                                    $("#previewflightdet").html(buildresult);
                                }
                                msg += response.Result[3] != null && response.Result[3] != "" ? response.Result[3].toString() : response.Result[7];
                                msg += "Are you sure you want to continue with ReBooking.....";
                                if (confirm(msg)) {
                                    var responsejson = JSON.parse(response.Result[6]);
                                    $('body').data('BookingResponse', JSON.stringify(responsejson));
                                    asyncafterbooking($('body').data('BookingResponse'), "7|YES"); /* flag added by rajesh for track update with result code*/
                                    // bookingflights(ContactDet, Paxdetails, BlockTicket, Paymentmode, Tour_code, TKey, Queuee, searvicekey, Appcurrency, book_seatarr, segment_type, otherssr, mulreq, Bargainfare, booktype, gstdeta, strRebook, "true", Reselect, "FALSE");
                                    BookRequest(1);
                                }
                                else {
                                    var responsejson = JSON.parse(response.Result[6]);
                                    $('body').data('BookingResponse', JSON.stringify(responsejson));
                                    asyncafterbooking($('body').data('BookingResponse'), "7|NO");/* flag added by rajesh for track update with result code*/
                                    document.getElementById("btnokBkngsessionExp").click();
                                }
                                return false;
                            }
                            //if (response.Result[8] == "8") {
                            //    var msg = "";
                            //    strRebook = response.Result[9];
                            //    msg += response.Result[3] != null && response.Result[3] != "" ? response.Result[3].toString() : response.Result[7];
                            //    if (confirm(msg)) {
                            //        bookingflights(ContactDet, Paxdetails, BlockTicket, Paymentmode, Tour_code, TKey, Queuee, searvicekey, Appcurrency, book_seatarr, segment_type, otherssr, mulreq, Bargainfare, booktype, gstdeta, strRebook, Rebookpnr, Reselect);
                            //    }
                            //    return false;
                            //}
                            if (response.Result[12] != null && response.Result[12] != "") {
                                //window.location.href = response.Result[12];
                                window.top.location.href = response.Result[12];

                                //$("#dvSelectView").css("display", "none");
                                //$("#dvAvailView").css("display", "none");

                                //$("#dvSearchView").css("display", "block");
                                //$("#dvSearchArea").css("display", "block");

                                // window.open(response.Result[12], '_blank');
                                return false;
                            }
                        }
                    }
                    var result = json.Result[0];
                    if (result == "1") {
                        var pres = json.Result[2];
                        if (json.Result[2] === "P") {
                            window.location = json.Result[1]
                            $("#dvProgress").hide();
                        }
                        else {
                            book_ticket(json.Result);
                            $("#dvProgress").hide();
                        }

                    }
                    else if (result == "2") {
                        var msg = json.Result[3]
                        if (confirm(msg)) {
                            Rebook("B");
                        }
                        else {
                            LogDetails("User response : NO", e.status, "Flight ReeBook");
                        }
                    }
                    else if (result == "3") {
                        var msg = json.Result[3];
                        showerralert(msg, 5000, "");
                        document.getElementById("btnback").click();
                        return false;
                    }
                    else {
                        showerralert(result, 5000, "");
                    }
                    $("#dvProgress").hide();
                },
                error: function (e) {// When Service call fails                           
                    LogDetails(e.responseText, e.status, "Flight Book");
                    if (e.statusText == "timeout") {
                        showerralert("Due to Internet slow the selected Operation is timed out. Please check booked history (or) search again.", "", "");
                    }
                }
            });

        } catch (e) {
            // alert("sample");
            showerralert(e.message, 5000, "");
        }
    }
}


function SampleRebooking(booktype) {
    var ContactDet = $("#txtTitle").val() + "SPLITSCRIPT" + $("#txtContactNo").val() + "SPLITSCRIPT" + $("#txtEmailID").val() + "SPLITSCRIPT" + $("#txtFirstName").val() + "SPLITSCRIPT" + $("#txtLastName").val() + "SPLITSCRIPT" + $("#txt_AlterEmail").val() + "SPLITSCRIPT" + $("#txtfareremark").val() + "SPLITSCRIPT" + $("#Txt_Emirates").val() + "SPLITSCRIPT" + $("#Txt_Location").val() + "SPLITSCRIPT" + $("#Countryname").val() + "SPLITSCRIPT" + $("#txtaddress").val() + "SPLITSCRIPT" + $("#txt_AgnNo").val() + "SPLITSCRIPT" + $("#txt_BusiNo").val() + "SPLITSCRIPT" + $("#txt_HomeNo").val() + "SPLITSCRIPT" + $("#txt_UserName").val();
    var Paxdetails = $("#hdnPax")[0].value;
    var Paymentmode = $("#ddlPaymode").val();

    var Tour_code = $("#Tour_Code").val();
    var Queuee = $("#hdnqueue").val();
    var searvicekey = hidden_hdntotalserv;
    var Appcurrency = $("#HDN_CURRENCY_code").val();
    var segment_type = $('body').data('segtype');
    var Bargainfare = document.getElementById("expected_fare").value;
    var book_seatarr = "";
    var Reselect = "";
    var TKey = sessionStorage.getItem("rebookTKey");
    var seatmaplength = book_seatx.length;
    if (seatmaplength != "" && seatmaplength != null) {
        for (var i = 0; i < seatmaplength; i++) {
            if (book_seatx[i] != "" && book_seatx[i] != null) {
                if (book_seatx[i].split('~')[3] != null && book_seatx[i].split('~')[3] != "" && book_seatx[i].split('~')[3] != "Not Selected") {
                    book_seatarr += book_seatx[i] + "$";
                } else if (book_seatx[i].split('~')[3] == "Not Selected") {
                    book_seatarr += "$";
                }
            }
        }

    }
    var otherssr = "";


    if (Other_bagout.length > 0) {
        for (var i = 0; i < Other_bagout.length; i++) {
            otherssr += Other_bagout[i] + "~";
        }
    }
    if (Other_SSRPCIN.length > 0) {
        for (var i = 0; i < Other_SSRPCIN.length; i++) {
            otherssr += Other_SSRPCIN[i] + "~";
        }
    }
    if (Other_SSRSPMax.length > 0) {
        for (var i = 0; i < Other_SSRSPMax.length; i++) {
            otherssr += Other_SSRSPMax[i] + "~";
        }
    }
    var gstdeta = "";
    var gst_regnumber = document.getElementById("gst_regnumber").value;
    var gst_cmpnyname = document.getElementById("gst_cmpnyname").value;
    var gst_address = document.getElementById("gst_address").value;
    var gst_mailid = document.getElementById("gst_mailid").value;
    var gst_mobno = document.getElementById("gst_mobno").value;
    if ((gst_regnumber != null && gst_regnumber != "") && (gst_cmpnyname != null && gst_cmpnyname != "") && (gst_address != null && gst_address != "") && (gst_mailid != null && gst_mailid != "") && (gst_mobno != null && gst_mobno != "")) {

        gstdeta = gst_regnumber + "~" + gst_cmpnyname + "~" + gst_address + "~" + gst_mailid + "~" + gst_mobno;

    }

    bookingflights(ContactDet, Paxdetails, BlockTicket, Paymentmode, Tour_code, TKey, Queuee, searvicekey, Appcurrency, book_seatarr, segment_type, otherssr, mulreq, Bargainfare, booktype, gstdeta, strRebook, "true", Reselect, "FALSE");
}

function rebookinfo(det) {
    var strbuild = "";
    var strconbuil = "";
    var path = $("#hdn_previewlogo").val();
    var i = 0;
    var AdtAmt = 0;
    var ChdAmt = 0;
    var InfAmt = 0;
    sessionStorage.setItem("totaldur", "");
    for (i = 0; det.length > i; i++) {
        var detstr = det[i].FL[i];
        var fares = det[i].FR[i].FRD;

        strbuild += '<div class="airline-pannel">';
        strbuild += '<div class="row margin-none">';
        strbuild += '<div class="col-xs-12 col-sm-6 col0">       ';
        strbuild += '<div class="flight-img">';
        strbuild += '<img src="' + path + detstr.FN.split(' ')[0] + ".png" + '" rel="" class="FlightTip FlightTipimg" data-popup="" onmouseover="showflightpoup(this)" id="loadselectedairlinename_pre_0_' + i + '' + detstr.FN.replace(" ", "").replace("/", "") + '">';
        strbuild += '</div>';
        strbuild += '<div class="flight-name">';
        strbuild += '<p class="f-name"><span class="fl">' + airlinename(detstr.FN.split(' ')[0]).split('(')[0] + ' Airlines</span><em class="fl" style="padding-left: 5px;">-</em>&nbsp;&nbsp;<span style="white-space: nowrap; color: #000;">' + detstr.FN + '</span></p>';
        strbuild += '</div>';
        strbuild += '</div>';
        strbuild += '<div class="col-xs-6 col-sm-3 col0">';
        strbuild += '<div class="flight-information">';
        strbuild += '<p class="flight-class fliclass" id="fliclass"></p>';
        strbuild += '</div>';
        strbuild += '</div>';
        strbuild += '<div class="col-sm-3 col-xs-6 col-md-3 class-farebasis txt-al-cntr">';
        strbuild += '<div class="row">';
        strbuild += '<div class="col-lg-12 col-xs-12" style="text-align: center;">';
        if ($("#hdn_sessAgentLogin").val() == "Y" || $("#hdn_AppHosting").val() != "BSA") {
            strbuild += '<span>';
            strbuild += '<span style="font-weight: 600; font-size: 11px; color: #333;">' + detstr.FBC + '</span>';
            strbuild += '</span>';
        }
        strbuild += '</div>';
        if (det[i].FL.length > 1) {
            var j = 0;
            strbuild += '<div class="col-lg-12 col-xs-12">';
            strbuild += '<label class="spanDetailsview" id="parentviewconnecting_pre_' + i + '">';
            strbuild += '<span id="viewconnecting_pre_' + i + '" onclick="toggelDetailsrebook(this.id)">+ Details</span>';
            strbuild += '</label>';
            strbuild += '</div>';

            for (j = 0; det[i].FL.length > j; j++) {
                strconbuil += '<div class="col-lg-12 col15" style="@border_class">';
                strconbuil += '<div class="row row5">';
                strconbuil += '<div class="col-lg-2 col-sm-2 col-md-2 col10 txt-al-cntr p-b-10">';
                strconbuil += '<span style="">';
                strconbuil += '<img src="' + path + detstr.FN.split(' ')[0] + ".png" + '" rel="" class="FlightTip FlightTipimg" data-popup="" onmouseover="showflightpoup(this)" id="loadselectedairlinename_pre_0_' + i + '' + detstr.FN.replace(" ", "").replace("/", "") + '">';
                strconbuil += '<span style="white-space: nowrap;">' + detstr.FN + '</span>';
                strconbuil += '</span>';
                strconbuil += '</div>';
                strconbuil += '<div class="col-lg-4 col-xs-6 col-md-4 col-sm-4 col10 org-lft">';
                strconbuil += '<span style="text-align: center;">';
                strconbuil += '<label style="font-size: 15px; color: #222; margin-bottom: 0px;">' + detstr.ORG;
                strconbuil += '</label>';
                strconbuil += '<br />';
                strconbuil += '<span style="padding: 5px 0px;">' + getdayvalue(detstr.SDT) + '</span>';
                strconbuil += '</span>';
                strconbuil += '</div>';
                strconbuil += '<div class="col-lg-4 col-xs-6 col-md-4 col-sm-4 col10 destination-city-cls">';
                strconbuil += '<span style="text-align: center;">';
                strconbuil += '<label style="font-size: 15px; color: #222; margin-bottom: 0px;">' + detstr.DES;
                strconbuil += '</label>';
                strconbuil += '<br />';
                strconbuil += '<span style="padding: 5px 0px; color: #222;">' + getdayvalue(detstr.EDT) + '</span>';
                strconbuil += '</span>';
                strconbuil += '</div>';
                strconbuil += '<div class="col-lg-2 col-xs-12 col-md-2 col-sm-2 txt-al-cntr p-b-10">     ';
                strconbuil += '<span>';
                strconbuil += '<span>' + detstr.FBC + '</span>';
                strconbuil += '</span>';
                strconbuil += '</div>';
                strconbuil += '</div>';
                strconbuil += '</div>';
            }


        }
        strbuild += '</div>';
        strbuild += '</div>';
        strbuild += '</div>';
        strbuild += '</div>';

        strbuild += '<div class="row row0 @cc_fl_cls @str_Dom_Mul_cls1 previreflight" id="sletedfltinSelect_pre_' + i + '" style="display:none;">';
        strbuild += '<div class="airline-details col-sm-12 col-xs-12 col0">';
        strbuild += '<div class="duration col-xs-12">';
        strbuild += '<div class="linebk">';
        strbuild += '<div class="flight-code">' + detstr.FN + '</div>';
        strbuild += '<span class="orginbkpre">' + detstr.ORG + ' </span>';
        strbuild += '<span class="time-org">' + getdayvalue(detstr.SDT) + ' </span>';
        strbuild += '<span class="time-hr timedur" id="timedur"></span>';
        strbuild += '<span class="time-dur"><i class="fa fa-clock-o" aria-hidden="true"></i> </span>';
        strbuild += '<span class="destibkpre">' + detstr.DES + '</span>';
        strbuild += '<span class="time-des">' + getdayvalue(detstr.EDT) + '</span>';
        strbuild += '</div>';
        strbuild += '</div>';
        strbuild += '</div>';
        strbuild += '</div>';
        var m = 0
        for (m = 0; m < fares.length; m++) {
            if (fares[m].PAX == "ADT") {
                AdtAmt = fares[m].GA;
            }
            else if (fares[m].PAX == "CHD") {
                ChdAmt = fares[m].GA;
            }
            else if (fares[m].PAX == "INF") {
                InfAmt = fares[m].GA;
            }
        }
        var amount = ((parseFloat(AdtAmt) * Readtcount) + (parseFloat(ChdAmt) * Rechdcount) + (parseFloat(InfAmt) * Reinfcount));
        var GrosseAmt = (parseFloat($(".riyains")[0].innerHTML) + parseFloat($(".riyamel")[0].innerHTML) + parseFloat($(".riyabag")[0].innerHTML) + parseFloat($(".riyasea")[0].innerHTML) + parseFloat($(".riyaohssr")[0].innerHTML));
        $(".riyatcktamt").html(amount);
        $(".riyatotal").html(amount + GrosseAmt);

    }
    $("#previewflightdet > #connectingflights_pre_0").html(strconbuil);
    return strbuild;
}

function toggelDetailsrebook(ids) {
    var toggle_id = ids.split("_pre_")[1]
    $("#connectingflights_pre_" + toggle_id).slideToggle();
    //$("#previewflightdet").find("#viewconnecting_pre_" + toggle_id).html("- details");
}

function bookingflights(ContactDet, Paxdetails, BlockTicket, Paymentmode, Tour_code, TKey, Queuee, searvicekey, Appcurrency, book_seatarr, segment_type, otherssr, mulreq, Bargainfare, booktype, gstdeta, strRebook, Rebookpnr, Reselect, AllowDuplicateBooking) {

    var Paymentmode = Paymentmode;

    var strRebook = strRebook;

    var ClientID = "";
    var BranchID = "";
    var GroupID = "";
    var MailID = "";
    var MobileNumber = "";
    var passthrow = "";
    var ontourcode = "";


    if ($("#ddlclient").length > 0) {
        ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
        BranchID = sessionStorage.getItem('branchid') != null ? sessionStorage.getItem('branchid').trim() : "";
        GroupID = sessionStorage.getItem('Groupid') != null ? sessionStorage.getItem('Groupid').trim() : "";
    }
    else {
        ClientID = "";
        BranchID = "";
        GroupID = "";
    }

    if ($("#txtAirlinemailid").val() != null && $("#txtAirlinemailid").val() != "") {
        MailID = $("#txtAirlinemailid").val();
    }
    else {
        MailID = $("#txtEmailID").val();
    }
    if ($("#priority_mobileno").val() != null && $("#priority_mobileno").val() != "") {
        MobileNumber = $("#priority_mobileno").val();
    }
    else if ($("#txt_BusiNo").val() != null && $("#txt_BusiNo").val() != "") {
        MobileNumber = $("#txt_BusiNo").val();
    }
    else if ($("#txt_HomeNo").val() != null && $("#txt_HomeNo").val() != "") {
        MobileNumber = $("#txt_HomeNo").val();
    }
    else if ($("#txt_AgnNo").val() != null && $("#txt_AgnNo").val() != "") {
        MobileNumber = $("#txt_AgnNo").val();
    }
    else {
        MobileNumber = $("#txtContactNo").val();
    }

    if ($("#hdn_producttype").val() == "RIYA") {
        ontourcode = document.getElementById("onwrdtor").value + "|";

        if ($("#hdtxt_trip")[0].value == "R") {
            ontourcode = document.getElementById("rtwrdtor").value + "|";
        }

    }

    var Monumber = "";
    if ($("#hdn_producttype").val() == "RIYA" && $('body').data('Agent_Type') == "RI") {
        //sts185
        var filtermo = /^[a-zA-Z]{2}[0-9]{2}[a-zA-Z]{3}[a-zA-Z0-9]{8}$/;
        if ($("#txtMonumber").val().trim() != null && $("#txtMonumber").val().trim() != "") {
            if (filtermo.test($('#txtMonumber').val())) {
                Monumber = $('#txtMonumber').val();
            } else {
                alert("Please Enter valid MO Number", "", "");
                return false;
            }
        }
        else {
            alert("Please Enter MO Number", "", "");
            return false;
        }
    }

    var BajajInsdetails = $("#hdn_bajaj").val();

    var ddlCardType = document.getElementById("ddlCardType").value;
    var CardName = $("#ddlCardType option:selected").text();// document.getElementById("ddlCardType").text;
    var txtBTACardNumber = document.getElementById("txtBTACardNumber").value;
    var ddlExpiryMonth = document.getElementById("ddlExpiryMonth").value;
    var ddlExpiryYear = document.getElementById("ddlExpiryYear").value;
    var txtCVV = document.getElementById("txtCVV").value;
    passthrow = CardName + "~" + ddlCardType + "~" + txtBTACardNumber + "~" + ddlExpiryMonth + "~" + ddlExpiryYear + "~" + txtCVV;


    var strCashPaymentinfo = "";
    if (Paymentmode == "H") {
        if ($("#txtcash_contactno").val() == null || $("#txtcash_contactno").val() == undefined || $("#txtcash_contactno").val() == "") {
            showerralert("Please enter customer contact number.", "", "");
            return false;
        }
        if ($("#txtcash_email").val() == null || $("#txtcash_email").val() == undefined || $("#txtcash_email").val() == "") {
            showerralert("Please enter your email id.", "", "");
            return false;
        }
        var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if ($("#txtcash_email").val() != "" && filter.test($("#txtcash_email").val())) {
            showerralert("Please enter valid email id.", "", "");
            return false;
        }
        if ($("#txtcash_EmpCode").val() == null || $("#txtcash_EmpCode").val() == undefined || $("#txtcash_EmpCode").val() == "") {
            showerralert("Please enter your employee code.", "", "");
            return false;
        }
        if ($("#hdncash_Empname").val() == "") {//$("#hdncash_Empbranch").val() == "" || $("#hdncash_Empemail").val() == "" || || $("#hdncash_Empmobile").val() == ""
            showerralert("Please enter valid employee Id.", "", "");
            return false;
        }
        if ($("#txtcash_Paymentdate").val() == null || $("#txtcash_Paymentdate").val() == undefined || $("#txtcash_Paymentdate").val() == "") {
            showerralert("Please select date of payment.", "", "");
            return false;
        }
        if ($("#txtcash_PaymentMethod").val() == null || $("#txtcash_PaymentMethod").val() == undefined || $("#txtcash_PaymentMethod").val() == "") {
            showerralert("Please select payment method.", "", "");
            return false;
        }
        if (($("#txtcash_pancard").val() == null || $("#txtcash_pancard").val() == undefined || $("#txtcash_pancard").val() == "")
            && (Number($("#totalAmnt").data("totalamntd")) > 50000)) {
            showerralert("Please enter customer contact number.", "", "");
            return false;
        }
        strCashPaymentinfo = $("#txtcash_contactno").val() + "SPLITCASH"        //0
                            + $("#txtcash_email").val() + "SPLITCASH"           //1
                            + $("#txtcash_Ref").val() + "SPLITCASH"             //2
                            + $("#txtcash_EmpCode").val() + "SPLITCASH"         //3
                            + $("#hdncash_Empname").val() + "SPLITCASH"         //4
                            + $("#hdncash_Empemail").val() + "SPLITCASH"        //5
                            + $("#hdncash_Empbranch").val() + "SPLITCASH"       //6
                            + $("#txtcash_Paymentdate").val() + "SPLITCASH"     //7
                            + $("#txtcash_PaymentMethod").val() + "SPLITCASH"   //8
                            + $("#txtcash_pancard").val() + "SPLITCASH"         //9
                            + $("#txtcash_freetext").val() + "SPLITCASH";       //10
    }

    var sbooking = {
        ContactDet: ContactDet,
        PaxDet: Paxdetails,
        BlockTicket: BlockTicket,
        PaymentMode: Paymentmode,
        TourCode: Tour_code,
        TKey: TKey,
        service: searvicekey,
        queue: Queuee,
        Seatvalue: book_seatarr,
        SegmentType: segment_type,
        mulreq: mulreq,
        Bargainfare: Bargainfare,
        booktype: booktype,
        gstdeta: gstdeta,
        otherssr: otherssr,
        Appcurrency: Appcurrency,
        ReBook: Rebookpnr,
        strRebook: strRebook,
        ClientID: ClientID,
        BranchID: BranchID,
        GroupID: GroupID,
        MailID: MailID,
        MobileNumber: MobileNumber,
        passthrow: passthrow,
        ontourcode: ontourcode,
        Insdetails: BajajInsdetails,
        MONumber: Monumber,
        AllowDuplicateBooking: AllowDuplicateBooking,
        CreditshellPNRDetails: $("#hdn_CreditshellAmt").val(),
        strPaxAddlInfo: $("#hdn_PaxAddlInfoData").val(),
        strCashPaymentDet: strCashPaymentinfo,
    }
    var temp = true;
    $("#dvProgress").show();

    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: bookingUrl, 		// Location of the service
        data: JSON.stringify(sbooking),
        //timeout: 200000,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (json) {//On Successful service call
            callpreviewfunction();
            $("#dvProgress").hide();
            if (temp == true) {
                if (json.indexOf("errordetails") != -1) {
                    $("#dvBookinsuccess").html(json); //To get error message id's...
                    showerralert($("#errormessage").html().trim(), "", "");

                    if (($("#hdn_producttype").val() == "UAE") && document.getElementById('hdn_allowinsurance').value == "Y" || document.getElementById('hdn_allowinsurance').value == "1") {
                        if (document.getElementById('insurance_confirmarion').checked == true) {
                            if (sessionStorage.getItem("retvaldet") == "YES") {
                                var tune_track_insert = "0";
                                bookins(sessionStorage.getItem("insvalues"), $("#p_Airlinetrackid_tune").html().trim(), tune_track_insert);
                            }
                        }
                    }
                    $("#dvBookinsuccess").html("");
                    return false;
                }
                else if (json.indexOf("sessionerrormessage") != -1) {
                    $("#dvBookinsuccess").html(json);
                    $('#PpopContnt').html($('#sessionerrormessage').html().trim());
                    if (($("#hdn_producttype").val() == "UAE") && document.getElementById('hdn_allowinsurance').value == "Y" || document.getElementById('hdn_allowinsurance').value == "1") {
                        if (document.getElementById('insurance_confirmarion').checked == true) {
                            if (sessionStorage.getItem("retvaldet") == "YES") {
                                var tune_track_insert = "0";
                                bookins(sessionStorage.getItem("insvalues"), $("#p_Airlinetrackid_tune").html().trim(), tune_track_insert);
                            }
                        }
                    }
                    $("#modal-bkngsess").modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    return false;
                }
                else if (json.indexOf("alertbookingmsg") != -1) {
                    $("#dvBookinsuccess").append(json);
                    var message = $("#alertbookingmsg").html().trim();
                    if (confirm(message)) {
                        asyncafterbooking($('body').data('BookingResponse'), "8|YES"); /* flag added by rajesh for track update with result code*/
                        bookingflights(ContactDet, Paxdetails, BlockTicket, Paymentmode, Tour_code, TKey, Queuee, searvicekey, Appcurrency, book_seatarr, segment_type, otherssr, mulreq, Bargainfare, booktype, gstdeta, strRebook, "false", Reselect, "TRUE");
                    } else {
                        asyncafterbooking($('body').data('BookingResponse'), "8|NO"); /* flag added by rajesh for track update with result code*/
                    }
                    return false;
                }
                else if (json.indexOf("spnBookmessage") != -1) {
                    $("#dvBookinsuccess").html(json);
                    $("#dvBookinsuccess").css("display", "block");
                    $("#dvSelectView").css("display", "none");
                    $("#dvAvailView").css("display", "none");
                    if (BlockTicket == true || booktype == "BF") {

                        if (($("#hdn_producttype").val() == "UAE") && document.getElementById('hdn_allowinsurance').value == "Y" || document.getElementById('hdn_allowinsurance').value == "1") {
                            if (document.getElementById('insurance_confirmarion').checked == true) {
                                if (sessionStorage.getItem("retvaldet") == "YES") {
                                    var tune_track_insert = "0";
                                    bookins(sessionStorage.getItem("insvalues"), "", tune_track_insert);
                                }
                            }
                        }
                    }
                    else {
                        if (($("#hdn_producttype").val() == "UAE") && document.getElementById('hdn_allowinsurance').value == "Y" || document.getElementById('hdn_allowinsurance').value == "1") {
                            if (document.getElementById('insurance_confirmarion').checked == true) {
                                if (sessionStorage.getItem("retvaldet") == "YES") {

                                    var tune_track_insert = "1";
                                    bookins(sessionStorage.getItem("insvalues"), "", tune_track_insert);
                                }
                            }
                        }
                    }

                    return false;
                }
                else {
                    var response = JSON.parse(json);
                    if (response.Result[8] == "7") {
                        var msg = "";
                        strRebook = response.Result[9];
                        msg += response.Result[3] != null && response.Result[3] != "" ? response.Result[3].toString() : response.Result[7];
                        msg += "Are you sure you want to continue with ReBooking.....";
                        if (confirm(msg)) {
                            var responsejson = JSON.parse(response.Result[6]);
                            $('body').data('BookingResponse', JSON.stringify(responsejson));
                            asyncafterbooking($('body').data('BookingResponse'), "7|YES"); /* flag added by rajesh for track update with result code*/
                            bookingflights(ContactDet, Paxdetails, BlockTicket, Paymentmode, Tour_code, TKey, Queuee, searvicekey, Appcurrency, book_seatarr, segment_type, otherssr, mulreq, Bargainfare, booktype, gstdeta, strRebook, "true", Reselect, "FALSE"); //,false
                        }
                        else {
                            var responsejson = JSON.parse(response.Result[6]);
                            $('body').data('BookingResponse', JSON.stringify(responsejson));
                            asyncafterbooking($('body').data('BookingResponse'), "7|NO"); /* flag added by rajesh for track update with result code*/
                            document.getElementById("btnokBkngsessionExp").click();
                        }
                        return false;
                    }
                }
            }
            var result = json.Result[0];
            if (result == "1") {
                var pres = json.Result[2];
                if (json.Result[2] === "P") {
                    window.location = json.Result[1]
                    $("#dvProgress").hide();
                }
                else {
                    $("#dvProgress").hide();
                    book_ticket(json.Result);
                }

            }
            else if (result == "2") {
                var msg = json.Result[3]
                if (confirm(msg)) {
                    Rebook("B");
                }
                else {
                    LogDetails("User response : NO", e.status, "Flight ReeBook");
                }
            }
            else if (result == "3") {
                var msg = json.Result[3];
                showerralert(msg, 5000, "");
                document.getElementById("btnback").click();
                return false;
            }
            else if (result == "10") {
                $("#dvProgress").hide();
                book_ticket(json.Result);
                return false;
            }
            else {
                showerralert(result, 5000, "");
            }
            $("#dvProgress").hide();
        },
        error: function (e) {// When Service call fails                           
            LogDetails(e.responseText, e.status, "Flight Book");
            if (e.statusText == "timeout") {
                showerralert("Due to Internet slow the selected Operation is timed out. Please check booked history (or) search again.", "", "");
            }
            //if (e.status == "500") {
            //    window.location = sessionExb;
            //    return false;
            //}
            $("#dvProgress").hide();
        }

    });



}


function rebookreselectfunction() {

}



$('#btnokBkngsessionExp').click(function () {
    $("#prioritycheckinpopup").html("");
    $("#otherssrspmax").html("");
    $("#bagoutpoup").html("");
    Other_SSRSPMax = [];
    Other_SSRPCIN = [];
    Other_bagout = [];
    primessr = [];
    $('#modal-bkngsess').modal('hide');
    //window.location.href = searchPageUrl;
    $("#dvSearchArea").css("display", "none");
    $("#dvAvailView").css("display", "block");
    $("#dvSelectView").css("display", "none");
});

function PendingCheck(TotalPax, checkflag) {

    var ValCheck = "";

    var hdvalkey = hidden_HDValKey;
    var hdservicech = hidden_hdntotalserv;
    var bookingtype = "";
    if ($("#hdn_allowbargain").val() == "1" || $("#hdn_allowbestbuy").val() == "1") {
        bookingtype = document.getElementById("booking_type").value;//"SR";
    }
    else {
        bookingtype = "SM";
    }
    var totalamount = $("#totalAmnt")[0].innerHTML;

    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });


    callpreviewfunction();

    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: checkTrackstatusUrl, 		// Location of the service
        data: '{PaxDet: "' + TotalPax + '",TKey: "' + hdvalkey + '",service:"' + hdservicech + '",Totalamount:"' + totalamount + '",mulreq:"' + mulreq + '"}',
        //timeout: 200000,
        contentType: "application/json; charset=utf-8",
        //async: false,
        dataType: "json",
        success: function (json) {//On Successful service call

            $.unblockUI();
            var result = json.Result[1];
            if (result == false) {
                //alert(json.Result[0]);
                showerralert(json.Result[0], "", "");
                ValCheck = "1"
            }
            else if (result == true) {
                var msg = json.Result[0]
                if (msg != "") {
                    if (confirm(msg))
                        ValCheck = "0"
                    else {
                        ValCheck = "1"
                    }
                }
                else
                    ValCheck = "0"
            }
            else {
                //alert(result);
                showerralert(result, "", "");
            }
            if (checkflag != true && ValCheck != "1") {
                $('body').data('BackFlag', 'PREVIEW');
                if (bookingtype == "BF") {
                    $("#modal-bargain").modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    $("#expected_fare").val("");
                    $("#txtfareremark").val("");
                }
                else if ($("#hdn_AppHosting").val() == "BSA") {
                    $("#dvBookingPreview").show();
                    $("#divavailselect").hide();
                    if ($("#hdn_sessAgentLogin").val() == "N") {
                        $("#Payments").click();
                    }
                }
                else {
                    $("#modal-changepayment").modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    $("#txtfareremark").val("");
                }

            }
        },
        error: function (e) {// When Service call fails 
            $.unblockUI();
            LogDetails(e.responseText, e.status, "Flight Book");
            if (e.statusText == "timeout") {
                // alert("Due to Internet slow the  Operation is timed out. Please do it again.");
                showerralert("Due to Internet slow the  Operation is timed out. Please do it again.", "", "");
                ValCheck = "1"
            }

            if (e.status == "500") {
                //alert("An Internal Problem Occurred. Your Session will Expire.");
                showerralert("An Internal Problem Occurred. Your Session will Expire.", "", "");
                window.top.location = sessionExb;
                return false;
            }


        }

    });

    //return ValCheck;
}

function asyncafterbooking(bookresponse, Flag) {

    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: Asyncbookingres_update,
        data: JSON.stringify({ Bookresponse: bookresponse, Responseflag: Flag }),
        timeout: 200000,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (json) {//On Successful service call

            var result = json.Result[0];
            $.unblockUI();
        },
        error: function (e) {// When Service call fails                           
            LogDetails(e.responseText, e.status, "AfterBooking");
            $.unblockUI();
        }

    });
}

function Getvalidation() {
    if ($("#txtContactNo").val() == "" && $("#txtEmailID").val() == "") {
        //alert("Please Enter Contact No. (or) Email Id");
        showerralert("Please Enter Contact No. (or) Email Id.", "", "");
        $('#txtContactNo').focus();
        return false;
    } else {
        if ($("#txtContactNo").val() != "" && $("#txtContactNo").val().length < 10) {
            showerralert("Please Enter Valid Contact No.", "", "");
            $('#txtContactNo').focus();
            return false;
        } else {
            if ($("#txtEmailID").val() != "") {
                var email = $('#txtEmailID');
                var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (filter.test(email.val()) == false) {
                    showerralert("Please provide a valid email address.", "", "");
                    email.focus();
                    return false;
                }
            }
        }
    }
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });

    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: Getcontact, 		// Location of the service                    
        data: '{MobNo: "' + $("#txtContactNo").val() + '",EmailId: "' + $("#txtEmailID").val() + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call

            var result = json.Result;
            if (result[1] != "") {
                rowss = result[3];
                $.unblockUI();
                loadContactgetable(result[1], rowss);

            }
            else if (result[2] != "") {
                $.unblockUI();
                contactfetch(result[2]);
            }
            else {
                $.unblockUI();
                //alert(result[0]);
                showerralert(result[0], "", "");
            }
            $.unblockUI();
        },
        error: function (e) {// When Service call fails      
            $.unblockUI();
            LogDetails(e.responseText, e.status, "Flight ReBook");
            if (e.status == "500") {
                // alert("An Internal Problem Occurred. Your Session will Expire.");
                //showerralert("An Internal Problem Occurred. Your Session will Expire.", "", "");
                window.location = sessionExb;
                return false;
            }
            $.unblockUI();

        }

    });

}

function loadContactgetable(Res, Allcheck) {


    $("#div_gtcontact").html("");
    var jsnarr = JSON.parse(Res);
    var jsonresponse = jsnarr.P_FETCH_CONTACT_DETAILS;
    var loopcount = 0;
    var Servicecharges = "";

    var txt_BusiNo = "";
    var txt_HomeNo = "";

    var ariname = "";
    var s = "";
    s += '<table class="no-more-tables table table-responsive table-bordered">';
    s += '<thead><tr><th>Select</th><th>Contact No.</th><th>Email ID</th><th>Title</th>'
    s += '<th>FirstName</th> <th>LastName</th>  <th style="display:none">Bussiness No.</th>'
    s += '<th style="display:none">Home no</th><th style="display:none">Address</th><th>Emirate</th><th style="display:none">Location</th> </tr></thead>';
    s += '<tbody>';
    for (var i = 0; i < jsonresponse.length; i++) {

        txt_BusiNo = jsonresponse[i]["BUSINESS NO"].indexOf('|') != -1 ? jsonresponse[i]["BUSINESS NO"].Split('|')[1] : jsonresponse[i]["BUSINESS NO"];
        txt_HomeNo = jsonresponse[i]["HOME NO"].indexOf('|') != -1 ? jsonresponse[i]["HOME NO"].Split('|')[1] : jsonresponse[i]["HOME NO"];


        s += '<tr>';

        s += '<td id="lblcheckbox_' + i + '" data-title="Select :"><input type="checkbox" class="comm_check_box" name="chkbox_contact" style="margin-top=20px" id="checkbox_' + i + '"/></td>';
        s += '<td id="con_mobno' + i + '" data-title="Contact No. :">' + jsonresponse[i]["MOBILE_NO"] + '</td>';

        s += '<td id="con_mailid' + i + '" data-title="Email ID :">' + jsonresponse[i]["EMAIL_ID"] + '</td>';
        s += '<td id="con_ti' + i + '" data-title="FirstName :">' + jsonresponse[i]["CONTACT_TITLE"] + '</td>';

        s += '<td id="con_fn' + i + '"  data-title="LastName :">' + jsonresponse[i]["FIRST_NAME"] + '</td>';
        s += '<td id="con_ln' + i + '" data-title="Bussiness No. :">' + jsonresponse[i]["LAST_NAME"] + '</td>';

        s += '<td style="display:none" id="con_bussno' + i + '" data-title="Bussiness No. :">' + txt_BusiNo + '</td>';
        s += '<td style="display:none" id="con_homeno' + i + '" data-title="Home no :">' + txt_HomeNo + '</td>';

        s += '<td style="display:none" id="con_pincd' + i + '" data-title="Pincode :">' + jsonresponse[i]["PINCODE"] + '</td>';
        s += '<td style="display:none" id="con_addrs' + i + '" data-title="Address :">' + jsonresponse[i]["ADDRESS"] + '</td>';

        s += '<td style="" id="con_emir' + i + '" data-title="Emirate :">' + jsonresponse[i]["STATE_ID"] + '</td>';
        s += '<td style="display:none" id="con_locat' + i + '" data-title="Location :">' + jsonresponse[i]["CITY_ID"] + '</td>';

        s += '</tr>';
    }
    s += '</tbody>';
    s += '</table>';


    $("#div_gtcontact").html(s);

    $("#modal-Pass_pg_formContainer_contact").modal({
        backdrop: 'static',
        keyboard: false
    });


}

function clearvalidation() {

    adCount = 1;
    chCount = 1;
    inCount = 1;

    $("#txtContactNo").val("");
    $("#txtContactNo").removeClass("has-content");

    $("#txtEmailID").val("");
    $("#txtEmailID").removeClass("has-content");

    $("#txtFirstName").val("");
    $("#txtFirstName").removeClass("has-content");

    $("#txtLastName").val("");
    $("#txtLastName").removeClass("has-content");

    $("#txt_AlterEmail").val("");
    $("#txt_AlterEmail").removeClass("has-content");

    $("#txtaddress").val("");
    $("#txtaddress").removeClass("has-content");

    //$("#txtRemark").val("");
    $("#txtPono").val("");
    $("#txtPono").removeClass("has-content");

    //$("#GetPassenger").css("display","none");
    //document.getElementById("exiting_passenger_details").innerHTML = "";

    $('#Txt_Emirates option')
 .filter(function () { return $.trim($(this).val()) == ""; })
 .attr('selected', true);

    $('#Txt_Emirates').niceSelect('destroy').niceSelect();

    $('#Txt_Location option')
 .filter(function () { return $.trim($(this).val()) == ""; })
 .attr('selected', true);

    $('#Txt_Location').niceSelect('destroy').niceSelect();

    $('#txtTitle option')
 .filter(function () { return $.trim($(this).val()) == ""; })
 .attr('selected', true);

    $('#txtTitle').niceSelect('destroy').niceSelect();

}

function loadpassengersdetails(Res, Allcheck) {


    $("#div_gtpassenger").html("");
    var jsnarr = JSON.parse(Res);
    var jsonresponse = jsnarr.P_FETCH_PASSENGERDETAILS;
    var loopcount = 0;
    var Servicecharges = "";

    var DOB = "";
    var txt_HomeNo = "";

    var ariname = "";
    var Pax_emaild = "";

    var Pax_Mobile = "";
    var FFNo = "";

    var s = "";
    var proofno = "";
    var issuedcountry = "";
    var passportexpiry = "";
    s += '<table class="no-more-tables table table-responsive table-bordered">';
    s += '<thead><tr><th>Select</th><th>Type</th><th>Title</th><th>FirstName</th>'
    s += '<th>LastName</th><th>DOB</th><th>Proof no</th>'
    s += '<th>Pass. Exp. date</th><th>Country</th> </tr></thead>';
    s += '<tbody>';
    for (var i = 0; i < jsonresponse.length; i++) {

        DOB = (jsonresponse[i]["DOB"] != "" ? jsonresponse[i]["DOB"] : "");

        Pax_emaild = jsonresponse[0]["EMAIL"];
        Pax_Mobile = jsonresponse[0]["MOBILE"];

        s += '<tr>';
        var selected_code = $.each(lst_Rcode, function (obj) {
            if (lst_Rcode[obj] == jsonresponse[i]["RCODE"]) {
                return lst_Rcode[obj];
            }
        });
        var checked = selected_code[0] == jsonresponse[i]["RCODE"] ? "checked" : "";

        s += '<td id="lblcheckbox_' + i + '" data-rcode="' + jsonresponse[i]["RCODE"] + '" data-title="Select :"><input type="checkbox" name="chkbox" ' + checked + ' style=""; onclick="getPassengerRowIndex(this,' + i + ')"/></td>';
        s += '<td class="dis-none" id="p_ty' + i + '" data-title="Type :">' + jsonresponse[i]["PASSENGER_TYPE"] + '</td>';

        s += '<td class="dis-none" id="p_ti' + i + '" data-title="Title :">' + jsonresponse[i]["PAX_PASSENGER_TITLE"] + '</td>';
        s += '<td id="p_fn' + i + '" data-title="FirstName :">' + jsonresponse[i]["PAX_PASSENGER_FIRST_NAME"] + '</td>';

        s += '<td id="p_ln' + i + '"  data-title="LastName :">' + jsonresponse[i]["PAX_PASSENGER_LAST_NAME"] + '</td>';
        s += '<td class="dis-none" id="p_dob' + i + '" data-title="DOB :">' + DOB + '</td>';
        proofno = (jsonresponse[i]["PROOFNO"] == null || jsonresponse[i]["PROOFNO"] == "") ? "--" : jsonresponse[i]["PROOFNO"];
        s += '<td style="" class="dis-none" id="p_Proof' + i + '" data-title="Proof no :">' + proofno + '</td>';
        passportexpiry = jsonresponse[i]["PAX_IDEXPIRY_DATE"] == null ? "--" : jsonresponse[i]["PAX_IDEXPIRY_DATE"];
        s += '<td style="" class="dis-none" id="p_idexp' + i + '" data-title="Pass. Exp. date :">' + passportexpiry + '</td>';
        issuedcountry = (jsonresponse[i]["ISSUED_COUNTRY"] == null || jsonresponse[i]["ISSUED_COUNTRY"] == "") ? "--" : jsonresponse[i]["ISSUED_COUNTRY"];
        s += '<td style="" class="dis-none" id="country' + i + '" data-title="Country :">' + issuedcountry + '</td>';
        //FFNo = (jsonresponse[i]["FREQUENT_FLYER_NO"] == null || jsonresponse[i]["FREQUENT_FLYER_NO"] == "") ? "" : jsonresponse[i]["FREQUENT_FLYER_NO"];
        //s += '<td style="" id="P_ffn' + i + '" data-title="Frequent Flyer No :">' + FFNo + '</td>';

        s += '</tr>';
    }
    s += '</tbody>';
    s += '</table>';

    $("#div_gtpassenger").html(s);
    if (($("#txtEmailID").val() == null || $("#txtEmailID").val() == "" && Pax_emaild != null && Pax_emaild != "")) {
        $("#txtEmailID").val(Pax_emaild);
    }

    if (($("#txtContactNo").val() == null || $("#txtContactNo").val() == "" && Pax_Mobile != null && Pax_Mobile != "")) {
        $("#txtContactNo").val(Pax_Mobile);
    }

    $("#txtContactNo").addClass("has-content");

    $("#txtEmailID").addClass("has-content");

    $("#modal-Pass_pg_formContainer_passenger").modal({
        backdrop: 'static',
        keyboard: false
    });


}

var pasa_adult = "";
var rowss = "";
var adCount = 1;
var chCount = 1;
var inCount = 1;
var lst_Rcode = [];
function getPassengerRowIndex(checkbox, rowindex) {


    var splitpass = $("#hdnPaxval").val();// document.getElementById("hdnPax").value.toString().split(',');
    var cur_adult_count = splitpass.split(',')[0];// document.getElementById("hdnPax").toString().split(',');
    var cur_child_count = splitpass.split(',')[1];// document.getElementById("hd_childCount").value;
    var cur_infant_count = splitpass.split(',')[2];// document.getElementById("hd_infant").value;
    var p_type = document.getElementById("p_ty" + rowindex).innerHTML;
    var p_code = $("#lblcheckbox_" + rowindex).data('rcode')
    if (checkbox.checked) {

        if (p_type == "Adult") {

            if (adCount <= cur_adult_count) {

                //var x = document.getElementById("exit_pass_table").getElementsByTagName("tr");
                //x[1].style.backgroundColor = "yellow";

                //var clr = document.getElementById('exit_pass_table').getElementsByTagName('tr');

                lst_Rcode.push(p_code);
                adCount++;


            }
            else {
                //alert("Please Choose Limit Adult");
                checkbox.checked = false;

            }

        }
        else if (p_type == "Child") {

            if (chCount <= cur_child_count) {
                //alert(chCount);
                chCount++;
                lst_Rcode.push(p_code);

            }
            else {
                //alert("Please Choose Limit Child");
                checkbox.checked = false;
            }

        }
        else if (p_type == "Infant") {

            if (inCount <= cur_infant_count) {
                //alert(inCount);
                inCount++;
                lst_Rcode.push(p_code);

            }
            else {
                //alert("Please Choose Limit Infant");
                checkbox.checked = false;
            }

        }

    }
    else {
        if (p_type == "Adult") {
            adCount += -1;
            //var x = document.getElementById("exit_pass_table").getElementsByTagName("tr");
            //x[1].style.backgroundColor = "red";
            lst_Rcode.pop(p_code);

        }
        else if (p_type == "Child") {
            chCount += -1;
            lst_Rcode.pop(p_code);

        }
        else if (p_type == "Infant") {
            inCount += -1;
            lst_Rcode.pop(p_code);

        }
    }



}

function contactfetch(value) {

    if (value != "") {
        var contactdetails = value.split("SPlIT")

        $('#txtTitle option')
     .filter(function () { return $.trim($(this).val()) == contactdetails[0]; })
     .attr('selected', true);
        $('#txtTitle').niceSelect('destroy').niceSelect();

        $("#txtFirstName").val(contactdetails[1]);

        $("#txtFirstName").focus();

        $("#txtLastName").val(contactdetails[2]);

        $("#txtLastName").focus();

        $("#txtContactNo").val(contactdetails[3]);

        $("#txtContactNo").focus();

        $("#txt_BusiNo").val(contactdetails[4]);

        $("#txt_BusiNo").focus();

        $("#txt_HomeNo").val(contactdetails[5]);

        $("#txt_HomeNo").focus();

        $("#txtEmailID").val(contactdetails[6]);

        $("#txtEmailID").focus();

        $("#txtaddress").val(contactdetails[7]);

        $("#txtaddress").focus();

        $("#txtPono").val(contactdetails[10]);

        $("#txtPono").focus();

        $('#Txt_Emirates option')
        .filter(function () { return $.trim($(this).val()) == contactdetails[8]; })
        .attr('selected', true);

        $('#Txt_Emirates').niceSelect('destroy').niceSelect();

        $('#Txt_Location option')
       .filter(function () { return $.trim($(this).val()) == contactdetails[8]; })
       .attr('selected', true);

        $('#Txt_Location').niceSelect('destroy').niceSelect();

        $('#GetPassenger').css("display", "block");

        getcity();
    }
}

function submitcontactRowIndex() {

    var c = document.getElementsByName('chkbox_contact')
    var pAdultDetail = "";
    var checkedvalue = 0;

    for (var i = 0; i < parseInt(rowss) ; i++) {
        if (c[i].checked === true) {
            checkedvalue++;

            var c_title = document.getElementById("con_ti" + i).innerHTML;
            var c_fname = document.getElementById("con_fn" + i).innerHTML;
            var c_lname = document.getElementById("con_ln" + i).innerHTML;
            var c_contactno = document.getElementById("con_mobno" + i).innerHTML;
            var c_bussinessno = document.getElementById("con_bussno" + i).innerHTML;
            var c_homeno = document.getElementById("con_homeno" + i).innerHTML;
            var c_mailid = document.getElementById("con_mailid" + i).innerHTML;
            var c_address = document.getElementById("con_addrs" + i).innerHTML;
            var c_emitars = document.getElementById("con_emir" + i).innerHTML;
            var c_location = document.getElementById("con_locat" + i).innerHTML;
            var c_pincd = document.getElementById("con_pincd" + i).innerHTML;
            //var c_pin = document.getElementById("con_pinco" + i).innerHTML;


            pAdultDetail = c_title + "SPlIT" + c_fname + "SPlIT" + c_lname + "SPlIT" + c_contactno + "SPlIT" + c_bussinessno + "SPlIT" + c_homeno + "SPlIT" + c_mailid + "SPlIT"
                + c_address + "SPlIT" + c_emitars + "SPlIT" + c_location + "SPlIT" + c_pincd;

        }


    }
    var RetValue = true;
    if (checkedvalue == 0) {
        // alert("Please select any one of the contact detail");
        showerralert("Please select any one of the contact detail.", "", "");
        RetValue = false;
    }
    else if (checkedvalue > 1) {
        //alert("Please select only one contact detail");
        showerralert("Please select only one contact detail.", "", "");
        RetValue = false;
    }
    else {
        contactfetch(pAdultDetail);
    }

    $("#modal-Pass_pg_formContainer_contact").modal("hide");

    return RetValue;
}

function submitPassengerRowIndex() {

    var c = document.getElementsByName('chkbox')
    var pAdultDetail = "";
    var pChildDetail = "";
    var pInfantDetail = "";
    var checkedvalue = 0;

    var Allowpassport = true; //STS185
    if (($("#hdn_producttype").val() == "JOD" || $("#hdn_producttype").val() == "RIYA") && $('body').data('segtype') == "D") {
        Allowpassport = false;
    }


    for (var i = 0; i < parseInt(rowss) ; i++) {

        if (c[i].checked === true) {
            checkedvalue++;
            var p_type = document.getElementById("p_ty" + i).innerHTML;
            var p_title = document.getElementById("p_ti" + i).innerHTML;
            var p_fname = document.getElementById("p_fn" + i).innerHTML;
            var p_lname = document.getElementById("p_ln" + i).innerHTML;
            var p_dob = document.getElementById("p_dob" + i).innerHTML;
            var proof = document.getElementById("p_Proof" + i).innerHTML;
            var idexp = document.getElementById("p_idexp" + i).innerHTML;
            var country = document.getElementById("country" + i).innerHTML;
            //var P_ffno = document.getElementById("P_ffn" + i).innerHTML;
            if (p_type == "Adult") {
                pAdultDetail += p_title + "~" + p_fname + "~" + p_lname + "~" + p_dob + "~" + proof + "~" + idexp + "~" + country + "|";

            }
            else if (p_type == "Child") {
                pChildDetail += p_title + "~" + p_fname + "~" + p_lname + "~" + p_dob + "~" + proof + "~" + idexp + "~" + country + "|";

            }
            else if (p_type == "Infant") {
                pInfantDetail += p_title + "~" + p_fname + "~" + p_lname + "~" + p_dob + "~" + proof + "~" + idexp + "~" + country + "|";

            }
        }


    }
    var RetValue = true;
    if (checkedvalue == 0) {
        //alert("Please select any one of the Passenger");
        showerralert("Please select any one of the Passenger.", "", "");
        RetValue = false;
    }
        //
    else {
        var pAd_first_split = pAdultDetail.split('|');
        var pCh_first_split = pChildDetail.split('|');
        var pIn_first_split = pInfantDetail.split('|');
        //Adult details


        var newdate = "";
        var adt_pass_exp = "";
        for (a_index = 0; a_index < pAd_first_split.length; a_index++) {
            if (pAd_first_split.length > 0) {
                var p_second_split = pAd_first_split[a_index].split('~');
                if (pAd_first_split[a_index] != "") {

                    $("#ad_firstName_" + (a_index)).val(p_second_split[1]);

                    $("#ad_firstName_" + (a_index)).addClass('has-content');

                    $("#ad_lastName_" + (a_index)).val(p_second_split[2]);

                    $("#ad_lastName_" + (a_index)).addClass('has-content');
                    if (p_second_split[3] != null && p_second_split[3].toUpperCase().trim() != "NULL" && p_second_split[3] != "") {
                        day = p_second_split[3];//.split("-");

                        newdate = day;//[day[2] + "/" + day[1] + "/" + day[0]];
                    } else {
                        newdate = "";
                    }
                    $("#ad_DOB_" + (a_index)).val(newdate);


                    $("#ad_DOB_" + (a_index)).addClass('has-content');
                    if (Allowpassport == true) {
                        if (p_second_split[4] != "--") {
                            $("#ad_passportNo_" + (a_index)).val(p_second_split[4]);
                            $("#ad_passportNo_" + (a_index)).addClass('has-content');
                        }
                        adt_pass_exp = p_second_split[5] != null ? p_second_split[5].toUpperCase().trim() != "NULL" ? p_second_split[5] : "" : "";
                        $("#ad_pass_ex_date_" + (a_index)).val(adt_pass_exp);
                        $("#ad_pass_ex_date_" + (a_index)).addClass('has-content');
                        if (p_second_split[6] != "--") {
                            $('#ad_Cntry_' + (a_index) + ' option')
                            .filter(function () { return $.trim($(this).val()) == p_second_split[6]; })
                            .attr('selected', true);
                        }
                    }

                    $('#ad_Title_' + (a_index) + ' option')
                   .filter(function () { return $.trim($(this).val()) == p_second_split[0]; })
                   .attr('selected', true);
                    $('#ad_Title_' + (a_index)).niceSelect('destroy').niceSelect();

                    if ($('#ad_Title_' + (a_index)).val() == "DR" || $('#ad_Title_' + (a_index)).val() == "MR") {

                        $('#ad_gender_' + (a_index) + ' option')
                   .filter(function () { return $.trim($(this).val()) == "M"; })
                   .attr('selected', true);

                        $('#ad_gender_' + (a_index)).niceSelect('destroy').niceSelect();

                    }
                    else {

                        $('#ad_gender_' + (a_index) + ' option')
                 .filter(function () { return $.trim($(this).val()) == "F"; })
                 .attr('selected', true);
                        $('#ad_gender_' + (a_index)).niceSelect('destroy').niceSelect();
                    }
                }

            }

        }
        //child detials
        var newdate = "";
        var chd_pass_exp = "";
        for (ch_index = 0; ch_index < pCh_first_split.length; ch_index++) {
            if (pCh_first_split.length > 0) {
                var p_second_split = pCh_first_split[ch_index].split('~');
                if (pCh_first_split[ch_index] != "") {
                    $("#ch_firstName_" + (ch_index)).val(p_second_split[1]);
                    $("#ch_firstName_" + (ch_index)).addClass('has-content');
                    $("#ch_lastName_" + (ch_index)).val(p_second_split[2]);
                    $("#ch_lastName_" + (ch_index)).addClass('has-content');
                    //document.getElementById("ch_lastName_" + (ch_index + 1)).value = p_second_split[2];
                    if (p_second_split[3] != null && p_second_split[3].toUpperCase().trim() != "NULL" && p_second_split[3] != "") {
                        day = p_second_split[3]

                        newdate = day;
                    } else {
                        newdate = "";
                    }

                    $("#ch_DOB_" + (ch_index)).val(newdate);

                    $("#ch_DOB_" + (ch_index)).addClass('has-content');


                    if (Allowpassport == true) {
                        if (p_second_split[4] != "--") {
                            $("#ch_passportNo_" + (ch_index)).val(p_second_split[4]);
                            $("#ch_passportNo_" + (ch_index)).addClass('has-content');
                        }
                        chd_pass_exp = p_second_split[5] != null ? p_second_split[5].toUpperCase().trim() != "NULL" ? p_second_split[5] : "" : "";
                        $("#ch_pass_ex_date_" + (ch_index)).val(chd_pass_exp);
                        $("#ch_pass_ex_date_" + (ch_index)).addClass('has-content');
                        if (p_second_split[6] != "--") {
                            $('#ch_Cntry_' + (ch_index) + ' option')
                           .filter(function () { return $.trim($(this).val()) == p_second_split[6]; })
                           .attr('selected', true);
                        }

                    }


                    $('#ch_Title_' + (ch_index) + ' option')
                   .filter(function () { return $.trim($(this).val()) == p_second_split[0]; })
                   .attr('selected', true);

                    $('#ch_Title_' + (ch_index)).niceSelect('destroy').niceSelect();


                    if ($('#ch_Title_' + (ch_index)).val() == "MISS") {

                        $('#ch_gender_' + (ch_index) + ' option')
                   .filter(function () { return $.trim($(this).val()) == "F"; })
                   .attr('selected', true);

                        $('#ch_gender_' + (ch_index)).niceSelect('destroy').niceSelect();

                    }
                    else {

                        $('#ch_gender_' + (ch_index) + ' option')
                .filter(function () { return $.trim($(this).val()) == "M"; })
                .attr('selected', true);

                        $('#ch_gender_' + (ch_index)).niceSelect('destroy').niceSelect();
                    }

                }

            }


        }
        //infant details
        var inf_pass_exp = "";

        for (in_index = 0; in_index < pIn_first_split.length; in_index++) {
            if (pIn_first_split.length > 0) {
                var p_second_split = pIn_first_split[in_index].split('~');
                if (pIn_first_split[in_index] != "") {


                    $("#in_firstName_" + (in_index)).val(p_second_split[1]);

                    $("#in_firstName_" + (in_index)).addClass('has-content');



                    $("#in_lastName_" + (in_index)).val(p_second_split[2]);

                    $("#in_lastName_" + (in_index)).addClass('has-content');


                    if (p_second_split[3] != null && p_second_split[3].toUpperCase().trim() != "NULL" && p_second_split[3] != "") {
                        day = p_second_split[3]

                        newdate = day;
                    } else {
                        newdate = "";
                    }

                    $("#in_DOB_" + (in_index)).val(newdate);

                    $("#in_DOB_" + (in_index)).addClass('has-content');

                    if (Allowpassport == true) {
                        if (p_second_split[4] != "--") {
                            $("#in_passportNo_" + (in_index)).val(p_second_split[4]);
                            $("#in_passportNo_" + (in_index)).addClass('has-content');
                        }
                        inf_pass_exp = p_second_split[5] != null ? p_second_split[5].toUpperCase().trim() != "NULL" ? p_second_split[5] : "" : "";
                        $("#in_pass_ex_date_" + (in_index)).val(inf_pass_exp);
                        $("#in_pass_ex_date_" + (in_index)).addClass('has-content');
                        if (p_second_split[4] != "--") {
                            $('#in_Cntry_' + (in_index) + ' option')
                           .filter(function () { return $.trim($(this).val()) == p_second_split[6]; })
                           .attr('selected', true);
                        }
                    }

                    $('#in_Title_' + (in_index) + ' option')
                   .filter(function () { return $.trim($(this).val()) == p_second_split[0]; })
                   .attr('selected', true);

                    $('#in_Title_' + (in_index)).niceSelect('destroy').niceSelect();

                    if ($('#in_Title_' + (in_index)).val() == "MISS") {

                        $('#in_gender_' + (in_index) + ' option')
                   .filter(function () { return $.trim($(this).val()) == "F"; })
                   .attr('selected', true);
                        $('#in_gender_' + (in_index)).niceSelect('destroy').niceSelect();

                    }
                    else {

                        $('#in_gender_' + (in_index) + ' option')
                 .filter(function () { return $.trim($(this).val()) == "M"; })
                 .attr('selected', true);

                        $('#in_gender_' + (in_index)).niceSelect('destroy').niceSelect();
                    }

                }

            }


        }
        $("#modal-Pass_pg_formContainer_passenger").modal("hide");
    }

    return RetValue;
}

function GetPass() {
    var flg = false;

    if ($("#txtContactNo").val() == "" && $("#txtEmailID").val() == "") {
        showerralert("Please Enter Contact No. (or) Email Id.", "", "");
    }
    else {

        var strMobileNo = $("#txtContactNo").val();
        var strEmailID = $("#txtEmailID").val();
        var strPaxType = $("#hdnPaxval").val();
        var strAction = "";

        var strParam = {
            strMobileNo: strMobileNo,
            strEmailID: strEmailID,
            strPaxType: strPaxType,
            strAction: strAction,
        };
        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });

        $.ajax({
            type: "POST",
            url: Getpassenger,
            data: JSON.stringify(strParam),
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (json) {//On Successful service call
                var json = json["Data"];
                var strStatus = json.Status;
                if (strStatus == "-1") {
                    window.location = sessionExb;
                    return false;
                }
                $.unblockUI();
                var result = json.Result
                if (strStatus == "1" && result[1] != "") {
                    rowss = result[2];
                    adCount = 1;
                    chCount = 1;
                    inCount = 1;
                    lst_Rcode = [];
                    loadpassengersdetails(result[1], result[2])
                }
                else {
                    var strmessage = result[0] != "" ? result[0] : "unable to get passenger details."
                    showerralert(strmessage, "", "");
                }
            },
            error: function (e) {//On Successful service call                            
                $.unblockUI();
                if (e.status == "500") {
                    window.location = sessionExb;
                    return false;
                }
                showerralert("unable to get passenger details(#07)", "", "");
                console.log(e);
                return false;
            }
        });
    }
}

function cancelsrrowindex(va) {
    var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;
    var seatamount = 0;
    var adtcount = paxcounttotal.split('@')[0];
    var chdcount = paxcounttotal.split('@')[1];

    if (book_seatx != null && book_seatx != "") {
        for (var i = 0; i < book_seatx.length; i++) {
            var paxrowindex = book_seatx[i].split('~')[0];
            var segrowid = book_seatx[i].split('~')[1];
            var paxtype = book_seatx[i].split('~')[4];
            var seatref = book_seatx[i].split('~')[5];
            var ddd = paxtype.indexOf("Child");

            if ((parseInt(adtcount) + parseInt(ssrindex)) == paxrowindex && ddd > -1 && clearpaxtype == "child") {
                if (book_seatx[i].split('~')[3] != "Not Selected") {
                    $('.showseatdet').html("Not Selected");
                    $('.Closeseat').css("display", "none");
                    book_seatx[i] = paxrowindex + "~" + segrowid + "~" + "Not Selected" + "~" + "Not Selected" + "~" + paxtype + "~" + seatref;

                }

            }
            else if (ssrindex == paxrowindex && paxtype.indexOf("Adult") > -1 && clearpaxtype == "adult") {
                if (book_seatx[i].split('~')[3] != "Not Selected") {
                    $('.showseatdet').html("Not Selected");
                    $('.Closeseat').css("display", "none");
                    book_seatx[i] = paxrowindex + "~" + segrowid + "~" + "Not Selected" + "~" + "Not Selected" + "~" + paxtype + "~" + seatref;
                }
            }
        }
    }

    for (var fr = 0; fr < book_seatx.length; fr++) {
        if (book_seatx[fr].split('~')[3] != "" && book_seatx[fr].split('~')[3] != null && book_seatx[fr].split('~')[3] != "Not Selected") {
            seatamount = Number(parseFloat(seatamount) + parseFloat(book_seatx[fr].split('~')[3])).toFixed(decimalflag);
        }
    }

    var par = $('#' + ptype).val().split('~');
    var tot = "";
    var totMeal = 0;
    var totBagg = 0;
    var GrossTot = 0;
    var grossbag = 0;
    try {

        if ($('#' + ptype).val() != "") {
            //
            var mm = $('#' + ptype).val().split('~')
            for (var j = 0; j < mm.length; j++) {
                if (mm[j] != "") {
                    var Im = mm[j].split('SpLiTSSR')
                    var Me = Im[0].split('WEbMeaLWEB')[1];
                    var BA = Im[1].split('WEbMeaLWEB')[1];
                    totMeal = Number(parseFloat(totMeal) - parseFloat(Me)).toFixed(decimalflag);
                    totBagg = Number(parseFloat(totBagg) - parseFloat(BA)).toFixed(decimalflag);
                }
            }
        }

        var new_seatamot = 0;
        var Seat_amount_row = 0;

        tot_seat_amount = seatamount;
        var total_amount = document.getElementById("Li_Totalfare").innerHTML;
        var insamt = document.getElementById('insamount').innerHTML;
        GrossTotM = Number(parseFloat(GrossTotM) + parseFloat(totMeal)).toFixed(decimalflag);
        grossbagg = Number(parseFloat(grossbagg) + parseFloat(totBagg)).toFixed(decimalflag);

        var paxtype = Paxdetailsoth.title;
        var paxtypeid = Paxdetailsoth.id;

        var paxcount = $('#' + paxtypeid).attr("data-paxcount");
        paxcounttotal = paxcount;
        var adtcount = paxcount.split("@")[0];
        var chdcount = paxcount.split("@")[1];

        //var bagoutfirst_amount = "";
        var Pax_ref = ((paxtype == "adult") ? (parseInt(Paxrefindex) + parseInt(1)) : (parseInt(adtcount) + parseInt(Paxrefindex) + parseInt(1)));

        // var Pax_ref = parseInt(Paxrefindex) + parseInt(1);
        var otherpriamount = 0;
        var Remove_SSR = [];
        if ($('.otherpriorit').hasClass) {
            Remove_SSR = [];
            var otherprilen = $('.otherpriorit').length;
            for (var i = 0; i < Other_SSRPCIN.length; i++) {
                var Paxref = Other_SSRPCIN[i].split("WEbMeaLWEB")[6];
                if (Pax_ref == Paxref) {
                    otherpriamount = Number(parseFloat(otherpriamount) + parseFloat(Other_SSRPCIN[i].split("WEbMeaLWEB")[3])).toFixed(decimalflag);
                    Remove_SSR.push(Other_SSRPCIN[i]);
                }
            }
            for (var ij = 0; ij < otherprilen; ij++) {
                $('#othssridpriority_' + ij)[0].checked = false;
            }
            for (var kk = 0; kk < Remove_SSR.length; kk++) {
                //if (Other_SSRPCIN.indexOf(Remove_SSR[kk]) > -1) {
                //    Other_SSRPCIN.pop(Remove_SSR[kk]);
                //}
                var popvalue = Other_SSRPCIN.indexOf(Remove_SSR[kk]);
                if (popvalue > -1) {
                    Other_SSRPCIN.splice(popvalue, 1);
                }
            }
        }
        var org = "";
        var des = "";
        var spmax_amount = 0;
        if ($('.othSpicemaxcls').hasClass) {
            Remove_SSR = [];
            var otherprilen = $('.othSpicemaxcls').length;
            for (var i = 0; i < Other_SSRSPMax.length; i++) {
                var Paxref = Other_SSRSPMax[i].split("WEbMeaLWEB")[6];
                org = Other_SSRSPMax[i].split("WEbMeaLWEB")[4];
                des = Other_SSRSPMax[i].split("WEbMeaLWEB")[5];
                if (Pax_ref == Paxref) {
                    spmax_amount = Number(parseFloat(spmax_amount) + parseFloat(Other_SSRSPMax[i].split("WEbMeaLWEB")[3])).toFixed(decimalflag);
                    Remove_SSR.push(Other_SSRSPMax[i]);
                }
            }
            for (var ij = 0; ij < otherprilen; ij++) {
                $('#othssridspicemax_' + ij)[0].checked = false;
            }
            for (var kk = 0; kk < Remove_SSR.length; kk++) {
                //if (Other_SSRSPMax.indexOf(Remove_SSR[kk]) > -1) {
                //    Other_SSRSPMax.pop(Remove_SSR[kk]);
                //}
                var popvalue = Other_SSRSPMax.indexOf(Remove_SSR[kk]);
                if (popvalue > -1) {
                    Other_SSRSPMax.splice(popvalue, 1);
                }

                var Primother = Remove_SSR[kk].split('WEbMeaLWEB');
                if (Primother[0].toUpperCase().indexOf("PRIM") > -1) {
                    $.each(primessr, function (obj, val) {
                        if (val.PaxNo == Pax_ref) {
                            primessr.splice(obj, 1);
                        }
                    });
                }
            }
        }
        Servicemealclear(org, des);
        var bagoutfirst_amount = 0;
        if ($('.bagoutfirclss').hasClass) {
            var otherprilen = $('.bagoutfirclss').length;
            //for (var i = 0; i < otherprilen; i++) {
            //    $('#othssrid_'+i)[0].checked = false;
            //}
            for (var i = 0; i < Other_bagout.length; i++) {
                var Paxref = Other_bagout[i].split("WEbMeaLWEB")[6];
                if (Pax_ref == Paxref) {
                    bagoutfirst_amount = Number(parseFloat(bagoutfirst_amount) + parseFloat(Other_bagout[i].split("WEbMeaLWEB")[3])).toFixed(decimalflag);
                    //Other_bagout.pop(Other_bagout[i]);
                    var popvalue = Other_bagout.indexOf(Other_bagout[i]);
                    if (popvalue > -1) {
                        Other_bagout.splice(popvalue, 1);
                    }
                }
            }
            //$('.bagoutfirclss').val("0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0");
            // $('.bagoutfirclss').prop('checked', false);
            // $(".bagoutfirclss")[0].checked = false;
        }


        $('.bagoutfirclss').attr('checked', false);
        $('.othSpicemaxcls').attr('checked', false);
        $('.otherpriorit').attr('checked', false);
        $('.bagoutfirclss_' + Pax_ref).attr('checked', false);
        $('.clsspicemax_' + Pax_ref).attr('checked', false);
        $('.clsOtherSSR_' + Pax_ref).attr('checked', false);

        var TotalGross = Number(parseFloat(total_amount) + parseFloat(GrossTotM) + parseFloat(grossbagg) + parseFloat(tot_seat_amount) - parseFloat(insamt) - parseFloat(otherpriamount) - parseFloat(bagoutfirst_amount) - parseFloat(spmax_amount)).toFixed(decimalflag);
        document.getElementById("mealamount").innerHTML = Number(GrossTotM).toFixed(decimalflag);
        document.getElementById("BaggageAmnt").innerHTML = Number(grossbagg).toFixed(decimalflag);
        document.getElementById("Li_Totalfare").innerHTML = Number(TotalGross).toFixed(decimalflag);// parseFloat(total_amount);
        document.getElementById("seatamount").innerHTML = parseFloat(tot_seat_amount).toFixed(decimalflag);
        document.getElementById("totalAmnt").innerHTML = Number(TotalGross).toFixed(decimalflag);
        document.getElementById("insamount").innerHTML = "0";

        $("span[id='spnFare']").text(TotalGross);

        var DDL = $('.ddlclass');
        $('.ddlclass').selectedIndex = 0
        for (var j = 0; j < DDL.length; j++) {

            //$('#Meals' + j + ' option')
            //.filter(function () { return $.trim($(this).val()) == "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0"; })
            //.attr('selected', true);

            $('#Meals' + j + '').val("0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0");
            //.filter(function () { return $.trim($(this).val()) == "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0"; })
            //.attr('selected', true);
            $('#Baggage' + j + '').val("0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0");
            //$('#Baggage' + j + ' option')
            //.filter(function () { return $.trim($(this).val()) == "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0"; })
            //.attr('selected', true);
            if (document.getElementById("frqno").value == "TRUE")
                $('#Freq_flyer' + j).val('')
        }
        $('#' + ptype).val('');
        //ptype
    }
    catch (Error) {
        showerralert(Error.message, "", "");
        //alert(Error.message);
    }
}

function gorupfareformation(clsname, count, pardiv, grid, id, trip) {

    if ($("#hdn_MobileDevice").val().toUpperCase() == "TRUE") {
        var maingrd = "";
        if (grid == "Dgrd") {
            maingrd = pardiv;
        }
        else {
            maingrd = "availset_O_1";
        }
        var currency = assignedcurrency;
        var cont = $('#' + maingrd + ' .' + clsname + '')[0].dataset;
        var aircode = cont.airline.split(' ')[0];
        var totalsec = cont.duration;
        //////////////////////
        var groupfarereqbuild = "";
        var groupedfli = groupmain_array.reduce(function (previousValue, currentValue) {
            previousValue[currentValue.flightid] = previousValue[currentValue.flightid] || []; //For both flight Number and Departure time
            previousValue[currentValue.flightid].push(currentValue);
            return previousValue;
        }, {});  //group based on flight id 
        var grouparray = [];
        for (var obj in groupedfli) {
            grouparray.push(groupedfli[obj]);
        }
        grouparray.sort(function (a, b) {
            return a[0].GFare - b[0].GFare;
        });

        var selectedflightpopup = [];
        selectedflightpopup = $.grep(grouparray, function (_obj) {
            return _obj[0]["flightid"] == cont["flightid"];
        })
        var flightspopup = [];
        $.each(selectedflightpopup[0], function (val, obj) {
            var fltiti = Number(obj["iti"]);
            if (flightspopup[fltiti] == undefined) {
                flightspopup[fltiti] = [];
            }
            flightspopup[fltiti].push(obj);
        });
        groupfarereqbuild += '<div class="container-fluid" id="mainfrmcom" style="padding: 1px 16px 1px !important; margin-bottom: 15px;">';

        //Static Start
        groupfarereqbuild += '<div class="row">';
        groupfarereqbuild += '<div class="col-sm-12 col-xs-12 clsTotalfarecount">';
        var onwardsecinter = flightspopup[0];
        groupfarereqbuild += '<div class="col-sm-6 col-xs-6 p-0">';
        groupfarereqbuild += '<span class="flt-lft fnt-13 fnt-wt-600">' + loadairportcityname(onwardsecinter[0].Org) + '</span><span class="flt-lft pl-5 pr-5"><i class="fa fa-long-arrow-right"></i></span><span class="flt-lft fnt-13 fnt-wt-600">' + loadairportcityname(onwardsecinter[onwardsecinter.length - 1].Des) + '</span>';
        groupfarereqbuild += '<div class="flt-lft w-100">';
        groupfarereqbuild += '<span class="com_orgdate font-12">' + getdayvalue(onwardsecinter[0].Dep) + '</span>';
        groupfarereqbuild += '</div>';
        groupfarereqbuild += '</div>';

        if ($('#hdtxt_trip').val() == "Y" && $('body').data('segtype') == "I") {
            var returnsecinter = flightspopup[1];
            groupfarereqbuild += '<div class="col-sm-6 col-xs-6 p-0">';
            groupfarereqbuild += '<span class="flt-rgt text-right fnt-13 fnt-wt-600">' + loadairportcityname(returnsecinter[returnsecinter.length - 1].Des) + '</span><span class="flt-rgt text-right pl-5 pr-5"><i class="fa fa-long-arrow-right"></i></span><span class="flt-rgt text-right fnt-13 fnt-wt-600">' + loadairportcityname(returnsecinter[0].Org) + '</span>';
            groupfarereqbuild += '<div class="flt-lft w-100 txt-right">';
            groupfarereqbuild += '<span class="com_orgdate font-12">' + getdayvalue(returnsecinter[0].Dep) + '</span>';
            groupfarereqbuild += '</div>';
            groupfarereqbuild += '</div>';
        }

        groupfarereqbuild += '</div>';
        groupfarereqbuild += '</div>';
        //End

        groupfarereqbuild += '<div class="row" style="background: #fff; padding-top: 10px; padding-bottom: 15px; color: black;">';
        groupfarereqbuild += '<div class="col-sm-8 col-xs-8">';
        groupfarereqbuild += '<div class="flt-lft pr-8">';
        groupfarereqbuild += '<img src="' + airlinelogourl + '/' + aircode + '.png?' + Versionflag + '" class="flt-lft"">';
        groupfarereqbuild += '</div>';
        groupfarereqbuild += '<div class="flt-lft">';
        groupfarereqbuild += '<span class="flt-lft w-100 font-12 fnt-wt-600">' + cont.airline + '</span>';
        groupfarereqbuild += '<span class="flt-lft w-100 font-12">' + cont.grpairlinename + '</span></div>';
        groupfarereqbuild += '</div>';

        groupfarereqbuild += '<div class="col-sm-4 col-xs-4">';
        groupfarereqbuild += '<span class="flt-rgt txt-al-rgt w-100 font-12">Class</span>';
        $('.flight-class').each(function () {
            if (this.checked == true) {
                $("#ddlclass")[0].value = this.value;
                Cabin = $("input[name='flight-class']:checked")["0"].dataset.value;
                groupfarereqbuild += '<span class="flt-rgt txt-al-rgt w-100 font-13 fnt-wt-600">' + Cabin + '</span></div>';
            }
        });

        groupfarereqbuild += '</div>';

        for (var _flt = 0; _flt < flightspopup.length; _flt++) {
            var fltdata = flightspopup[_flt];
            groupfarereqbuild += '<div class="col-sm-12 col-xs-12 p-0 mb-15 flt-lft w-100 bg-lt-blu-lt brdr-radius-2 brdr-full">';

            groupfarereqbuild += '<div class="col-sm-4 col-xs-4 pt-10 pb-10">';
            groupfarereqbuild += '<div class="com_orgin">';
            groupfarereqbuild += '<span class="flt-lft w-100 font-13 fnt-wt-600">' + loadairportcityname(fltdata[0].Org) + '</span>';
            groupfarereqbuild += '<span class="flt-lft w-100 font-12 fnt-wt-500">' + fltdata[0].Dep + '</span>';
            groupfarereqbuild += '</div>';
            groupfarereqbuild += '</div>';

            groupfarereqbuild += '<div class="col-sm-4 col-xs-4 pr-10 pl-10 pt-10 pb-10">';
            groupfarereqbuild += '<div class="flt-lft w-100 txt-cntr font-12"><span>' + CalcTimeDiff(fltdata[fltdata.length - 1].Arr, fltdata[0].Dep) + '</span></div>';
            groupfarereqbuild += '<div class="pos-rel line-hgt flt-lft w-100">';
            groupfarereqbuild += '<div class="dotleft pos-abs"></div>';
            groupfarereqbuild += '<div class="flticon pos-abs">';
            groupfarereqbuild += '<i class="fa fa-plane themeclr-blu"></i>';
            groupfarereqbuild += '</div>';
            groupfarereqbuild += '</div>';
            if (Number(fltdata[0].Stops) > 0) {
                groupfarereqbuild += '<span class="flt-lft w-100 txt-cntr font-12">' + Number(fltdata[0].Stops) + ' stop</span>';
            }
            else {
                groupfarereqbuild += '<span class="flt-lft w-100 txt-cntr font-12">Non-stop</span>';
            }
            groupfarereqbuild += '</div>';

            groupfarereqbuild += '<div class="col-sm-4 col-xs-4 pt-10 pb-10">';
            groupfarereqbuild += '<div class="com_orgin">';
            groupfarereqbuild += '<span class="flt-rgt txt-al-rgt w-100 font-13 fnt-wt-600">' + loadairportcityname(fltdata[fltdata.length - 1].Des) + '</span>';
            groupfarereqbuild += '<span class="com_orgdate flt-rgt txt-al-rgt w-100 font-12 fnt-wt-500">' + fltdata[fltdata.length - 1].Arr + '</span>';
            groupfarereqbuild += '</div>';
            groupfarereqbuild += '</div>';
            if (fltdata.length > 1) {
                groupfarereqbuild += '<div class="col-sm-12 col-xs-12 p-0">';
                groupfarereqbuild += '<a htef="#" onclick="$(\'#fltdetails_popup_' + _flt + '\').slideToggle();" class="flt-lft w-100 txt-cntr font-13 pt-5 pb-5 brdr-top">More Details<i class="fa fa-angle-down pl-8"></i></a>';
                groupfarereqbuild += '</div>';
                groupfarereqbuild += '<div class="col-sm-12 col-xs-12 bg-white pt-10 pb-10" style="display:none;" id="fltdetails_popup_' + _flt + '">';
                for (var _fltdet = 0; _fltdet < fltdata.length; _fltdet++) {
                    groupfarereqbuild += '<div class="col-sm-4 col-xs-4 p-0 mb-5">';
                    groupfarereqbuild += '<div class="com_orgin">';
                    groupfarereqbuild += '<span class="flt-lft w-100 font-13 fnt-wt-600">' + loadairportcityname(fltdata[_fltdet].Org) + '</span>';
                    groupfarereqbuild += '<span class="flt-lft w-100 font-12 fnt-wt-500">' + fltdata[_fltdet].Dep + '</span>';
                    groupfarereqbuild += '</div>';
                    groupfarereqbuild += '</div>';

                    groupfarereqbuild += '<div class="col-sm-4 col-xs-4 pr-10 pl-10 mb-5">';
                    groupfarereqbuild += '<div class="flt-lft w-100 txt-cntr font-12"><span>' + CalcTimeDiff(fltdata[_fltdet].Arr, fltdata[_fltdet].Dep) + '</span></div>';
                    groupfarereqbuild += '<div class="pos-rel line-hgt flt-lft w-100">';
                    groupfarereqbuild += '<div class="dotleft pos-abs"></div>';
                    groupfarereqbuild += '<div class="flticon pos-abs">';
                    groupfarereqbuild += '<i class="fa fa-plane themeclr-blu"></i>';
                    groupfarereqbuild += '</div>';
                    groupfarereqbuild += '</div>';
                    if (Number(fltdata[_fltdet].Stops) > 0) {
                        groupfarereqbuild += '<span class="flt-lft w-100 txt-cntr font-12">' + Number(fltdata[_fltdet].Stops) + ' stop</span>';
                    }
                    else {
                        groupfarereqbuild += '<span class="flt-lft w-100 txt-cntr font-12">Non-stop</span>';
                    }
                    groupfarereqbuild += '</div>';

                    groupfarereqbuild += '<div class="col-sm-4 col-xs-4 p-0 mb-5">';
                    groupfarereqbuild += '<div class="com_orgin">';
                    groupfarereqbuild += '<span class="flt-rgt txt-al-rgt w-100 font-13 fnt-wt-600">' + loadairportcityname(fltdata[_fltdet].Des) + '</span>';
                    groupfarereqbuild += '<span class="com_orgdate flt-rgt txt-al-rgt w-100 font-12 fnt-wt-500">' + fltdata[_fltdet].Arr + '</span>';
                    groupfarereqbuild += '</div>';
                    groupfarereqbuild += '</div>';
                }
                groupfarereqbuild += '</div>';
            }

            groupfarereqbuild += '</div>';
        }

        groupfarereqbuild += '</div>';
        groupfarereqbuild += '</div>';
        selectedflightpopup = selectedflightpopup[0];
        var groupidfilter = selectedflightpopup[0].Fno + "~" + selectedflightpopup[0].Org + "~" + selectedflightpopup[selectedflightpopup.length - 1].Des + "~" + selectedflightpopup[0].Dep + "~" + selectedflightpopup[selectedflightpopup.length - 1].Arr + "~" + selectedflightpopup[selectedflightpopup.length - 1].Fno + "~" + (selectedflightpopup.length > 2 ? selectedflightpopup[selectedflightpopup.length - 2].Fno : "") + "~" + (selectedflightpopup.length > 3 ? selectedflightpopup[selectedflightpopup.length - 3].Fno : "")
        var groupedorgdes = grouparray.reduce(function (previousValue, currentValue) {
            previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] = previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")] || []; //For both flight Number and Departure time
            previousValue[currentValue[0].Fno + "~" + currentValue[0].Org + "~" + currentValue[currentValue.length - 1].Des + "~" + currentValue[0].Dep + "~" + currentValue[currentValue.length - 1].Arr + "~" + currentValue[currentValue.length - 1].Fno + "~" + (currentValue.length > 2 ? currentValue[currentValue.length - 2].Fno : "") + "~" + (currentValue.length > 3 ? currentValue[currentValue.length - 3].Fno : "")].push(currentValue);
            return previousValue;
        }, {});
        var filteredarray = groupedorgdes[groupidfilter];

        groupfarereqbuild += '<div class="col-sm-12 col-xs-12">';
        groupfarereqbuild += '<div class="col-sm-12 col-xs-12" style="box-shadow: 0px 0px 3px 1px #eee;margin-bottom:70px;">';
        groupfarereqbuild += '<span class="flt-lft w-100"><span class="flt-lft themeclrrd pt-8 pb-8 font-13 fnt-wt-600">Fare Comparable</span>';
        groupfarereqbuild += '<span class="flt-rgt txt-al-rgt font-12 pt-8 pb-8">Total Fare: <span class="font-13 fnt-wt-600 themeclrrd">' + filteredarray.length + '</span></span>';

        groupfarereqbuild += '<div class="col-sm-12 col-xs-12 p-0" style="display:block">';

        for (var i = 0; i < filteredarray.length; i++) {
            var articleid = $('#' + maingrd + ' .' + clsname + '')[i].id.toString().replace("li_Rows", "");
            var loop_flt = filteredarray[i];
            groupfarereqbuild += '<div class="col-sm-12 col-xs-12 pl-0 pb-0 br-b pb-10 pt-10">';
            groupfarereqbuild += '<div class="col-sm-6 col-xs-6 pl-0">';
            groupfarereqbuild += '<div class="flt-lft w-100 mb-8"><span class="flt-lft font-12 pr-5">Baggage:</span> <span class="flt-lft font-12"><i class="fa fa-suitcase pr-5 font-13 themeclr-red"></i>' + loop_flt[0].Bagg + '</span></div>';
            if (loop_flt[0].AIF.toUpperCase().trim().indexOf("NO COMPLEMENTARY MEALS") != -1) {
                groupfarereqbuild += '<div class="flt-lft w-100 mb-8"><span class="flt-lft font-12 pr-5">Baggage:</span> <span class="flt-lft font-12">';
                groupfarereqbuild += '<span class="mealswdth" style="padding-left: 5px;" data-minirule="No Complementry Meals." data-toggle="tooltip" data-placement="top" title="' + loop_flt[0].AIF + '" ><img src="' + strImageUrl + '/' + $("#hdn_producttype").val() + '/MEAL.png?' + Versionflag + '" /></span>';
                groupfarereqbuild += '</div>';
            }
            if (loop_flt[0].Refund == "TRUE") {
                groupfarereqbuild += '<div class="flt-lft w-100"><span class="farepopup-refun Faretype-Ref font-12 flt-lft">Refundable</span></div>';
            }
            else if (loop_flt[0].Refund == "FALSE") {
                groupfarereqbuild += '<div class="flt-lft w-100"><span class="farepopup-refun Faretype-NONRef font-12 flt-lft">Non-Refundable</span></div>';
            }
            groupfarereqbuild += '</div>';
            groupfarereqbuild += '<div class="col-sm-5 col-xs-5">';
            var currencyIcon = assignedcurrency == "INR" ? "<i class='fa fa-inr'></i>" : assignedcurrency;
            var fare = loop_flt[0].GFare;
            if ($("#HideComission").val() == "Y") {
                fare = loop_flt[0].NETFare
            }
            groupfarereqbuild += '<div class="flt-lft w-100"><span class="flt-rgt txt-al-rgt font-12">' + currencyIcon + ' <span class="font-18 themeclr-blu">' + fare + '</span></span></div>';
            groupfarereqbuild += '<div class="flt-rgt txt-al-rgt w-100"><span class="cursor-point font-12 fnt-wt-500 txt-blck" style="color:blue;" onclick="javascript:return GetFareRule_grpfare(' + articleid + ')">Fare Rule</span></div>';
            groupfarereqbuild += '</div>';
            groupfarereqbuild += '<div class="col-sm-1 col-xs-1 pr-0">';
            var checkedclass = loop_flt[0].flightid == selectedflightpopup[0].flightid ? "checked=true" : "";
            groupfarereqbuild += '<div class="radio"><input id="selectclickbuttongrp' + articleid + '" ' + checkedclass + ' onclick="javascript:ChangeFareForSelect(\'' + articleid + '\',\'' + fare + '\')" class="Bkpaymentmode" name="radio" type="radio"><label for="selectclickbuttongrp' + articleid + '" class="radio-label RDT" title=""></label>';
            groupfarereqbuild += '</div>';
            groupfarereqbuild += '</div>';
            groupfarereqbuild += '</div>';
        }
        groupfarereqbuild += '<div class="container-fluid" id="grpfarerul" style="display:none">';
        groupfarereqbuild += '<div class="row" id="appndfarerule">';
        groupfarereqbuild += '</div>';
        groupfarereqbuild += '</div>';

        groupfarereqbuild += '</div>';
        groupfarereqbuild += '</div>';
        groupfarereqbuild += '</div>';

        groupfarereqbuild += '<div class="position-fixed flt-lft w-100 bg-themecle-blu b-0 pt-10 pb-10">';
        groupfarereqbuild += '<div class="col-sm-6 col-xs-6">';
        groupfarereqbuild += '<span class="flt-lft w-100 font-13 white-text">Total Fare</span>';
        var CurrencyCLs = assignedcurrency == "INR" ? 'fa fa-inr' : "";
        var fare = flightspopup[0][0].GFare;
        if ($("#HideComission").val() == "Y") {
            fare = flightspopup[0][0].NETFare;
        }
        groupfarereqbuild += '<span class="flt-lft w-100 font-18 white-text ' + CurrencyCLs + '" id="spnChosenPopupFare">' + fare + '</span>';
        groupfarereqbuild += '</div>';
        groupfarereqbuild += '<div class="col-sm-6 col-xs-6">';
        $("body").data("selectedfareid_select", id);
        groupfarereqbuild += '<button type="button" onclick="PopupRedirectToSelect(this.id,\'' + grid + '\')" data-selectedfareid="' + id + '" data-arg="' + cont["arg"] + '" id="btn_fare" class="flt-lft w-100">Proceed</button>';
        groupfarereqbuild += '</div>';
        groupfarereqbuild += '</div>';
        //End

        sessionStorage.setItem("flggrp", "N");
    }
    else if ($("#hdn_AppHosting").val() == "BSA" || $("#hdn_producttype").val() == "DEIRA") {
        var maingrd = "";
        if (grid == "Dgrd") {
            maingrd = pardiv;
        }
        else {
            maingrd = "availset_O_1";
        }

        var currency = assignedcurrency;
        var cont = $('#' + maingrd + ' .' + clsname + '')[0].dataset;

        var aircode = cont.airline.split(' ')[0];

        var totalsec = cont.duration;

        var groupfarereqbuild = "";
        groupfarereqbuild += '<div class="container-fluid" id="mainfrmcom" style="background-color: #e8e7e7; padding: 1px 16px 1px !important; margin-bottom: 15px;">';
        groupfarereqbuild += '<div class="row" style="background: #fff; padding-top: 10px; padding-bottom: 10px; color: black;"><div class="col-sm-4 col-xs-12 fnt-wt-5 mg-btm-20-res" style="text-align: center; white-space: nowrap;"><img src="' + airlinelogourl + '/' + aircode + '.png?' + Versionflag + '"><br><span style="white-space: nowrap;">' + cont.airline + '</span><br><span>' + cont.grpairlinename + '</span></div>';

        groupfarereqbuild += '<div class="col-sm-8 col-sm-offset-0 col-xs-11 col-xs-offset-1">';

        groupfarereqbuild += '<div class="dotted_icon"></div>';

        groupfarereqbuild += '<div class="com_orgin">';
        groupfarereqbuild += '<span class="com_orctry">' + cont.grporgvcity + '</span>';
        groupfarereqbuild += '<span class="com_orgdate">' + getdayvalue(cont.depdatetime) + '</span>';
        groupfarereqbuild += '</div>';

        groupfarereqbuild += '<div class="com_laytime"><span>Travel time: ' + totalsec + '</span></div>';

        groupfarereqbuild += '<div class="com_orgin">';
        groupfarereqbuild += '<span class="com_orctry">' + cont.grpdesvcity + '</span>';
        groupfarereqbuild += '<span class="com_orgdate">' + getdayvalue(cont.arrdatetime) + '</span>';
        groupfarereqbuild += '</div>';

        groupfarereqbuild += '</div>';

        groupfarereqbuild += '</div>';
        groupfarereqbuild += "</div>"
        groupfarereqbuild += '<div class="row row0 clsTotalfarecount"><span class="totalgroupcnt">Total Fares : ' + count + '</span></div>'
        groupfarereqbuild += '<div class="row row0 scrollmenu" style="overflow: auto;color:black;box-shadow: 0px 1px 1px 1px #eee;"><div class="col-md-12" style="box-sizing: border-box;" id="dvMultiflights0">'
        groupfarereqbuild += '<table cellspacing="0" cellpadding="0" style="width:100%;"><tbody><tr>'

        for (var i = 0; i < count; i++) {

            var articleid = $('#' + maingrd + ' .' + clsname + '')[i].id.toString().replace("li_Rows", "");
            var cont_loop = $('#' + maingrd + ' .' + clsname + '')[i].dataset;
            var bagg_Det_con = "";
            if ($('#showbagg' + articleid).length > 0) {
                var bagg_Det = $('#showbagg' + articleid)[0].innerHTML;
                bagg_Det_con = true;
            }
            else {
                var bagg_Det = "No Baggage"
                bagg_Det_con = false;
            }
            var farecls = cont_loop.clssname;
            var fare_typ = cont_loop.faretype;

            groupfarereqbuild += '<td class="col-sm-12 col-xs-12 col0" style=float:left;>'
            groupfarereqbuild += '<div class="row brdr-btm-das txt-cntr p-t-15 p-b-15">'
            groupfarereqbuild += '<div class="col-sm-4 ClsChild ClsCompGrsAmt blink_me brdr-rt-das p-t-10 col-xs-4">'
            groupfarereqbuild += '<span style="font-size:15px;font-weight:600;">'


            groupfarereqbuild += '<i style="font-size: 10px;font-variant: petite-caps;font-style: normal;">' + currency + '</i>'

            groupfarereqbuild += '&nbsp;' + cont_loop.price + '</span>'

            groupfarereqbuild += ''
            groupfarereqbuild += ''
            groupfarereqbuild += '<br><span style="font-weight:600;font-size:10px;white-space:nowrap;cursor:pointer;color:blue;" onclick="javascript:return GetFareRule_grpfare(' + articleid + ')">FARE RULE</span></div>'
            groupfarereqbuild += '<div class="col-sm-2 ClsHead brdr-rt-das col-xs-2" style="padding:5px 0;"><span style="">'

            if (bagg_Det_con == true) {
                groupfarereqbuild += '<i class="soap-icon-suitcase"></i>'
                groupfarereqbuild += '<br><label style="font-size:10px;color:red;margin-bottom: 0px; font-weight: 600;">' + bagg_Det + '</label></span></div>';

            }
            if (bagg_Det_con == false) {
                groupfarereqbuild += '<img src="' + strImageUrl + '/' + $("#hdn_producttype").val() + '/cancelbag.png" alt="' + bagg_Det + '">'
                groupfarereqbuild += '<br><label style="font-size:10px;color:red;">' + bagg_Det + '</label></span></div>';
            }
            if ($("#hdn_sessAgentLogin").val() == "Y") {
                groupfarereqbuild += '<div class="col-sm-2 ClsHead brdr-rt-das col-xs-2" style="padding:10px;">';
                groupfarereqbuild += '<span class="spanFaretype clsHiddenText"><span>' + farecls + '</span></span>' //Addeed the farecls Deira Updated
                groupfarereqbuild += '<span class="farecomp" style="font-weight:400;font-size:11px;">' + Faretype(fare_typ) + '</span>';
                groupfarereqbuild += '</div>';
            }
            groupfarereqbuild += '<div class="col-sm-4 p-t-10 col-xs-4">'
            if (grid == "Dgrd") {

                if (id == articleid) {
                    groupfarereqbuild += '<a id="selectclickbuttongrp' + articleid + '" class=" button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton  seletedcls"  onclick=' + '"' + "javascript:bindSletedRndTrpAvail('" + articleid + "','" + cont_loop.arg + "')" + '"' + '>Selected  </a></div></div></td>'
                }
                else {
                    groupfarereqbuild += '<a id="selectclickbuttongrp' + articleid + '" class="gropbtncls button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton selebtn "  onclick=' + '"' + "javascript:bindSletedRndTrpAvail('" + articleid + "','" + cont_loop.arg + "')" + '"' + '>Select  </a></div></div></td>'
                }
            }
            else {

                if (id == articleid) {

                    groupfarereqbuild += '<a id="selectclickbuttongrp' + articleid + '" class=" button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton  seletedcls"  onclick="javascript:GetRowIndexSelect(' + articleid + ')">Selected  </a></div></div></td>'
                }
                else {

                    groupfarereqbuild += '<a id="selectclickbuttongrp' + articleid + '" class="gropbtncls button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton selebtn "  onclick="javascript:GetRowIndexSelect(' + articleid + ')">Select  </a></div></div></td>'
                }

            }
        }
        groupfarereqbuild += '</tr></tbody></table></div></div>'
        groupfarereqbuild += '</div>';
        groupfarereqbuild += '<div class="container-fluid" id="grpfarerul" style="display:none">';
        groupfarereqbuild += '<div class="row" id="appndfarerule">';
        groupfarereqbuild += '</div>';
        groupfarereqbuild += '</div>';
        sessionStorage.setItem("flggrp", "N");
    }
    else if ($("#hdn_producttype").val() == "RIYA") {
        var maingrd = "";
        if (grid == "Dgrd") {
            maingrd = pardiv;
        }
        else {
            maingrd = "availset_O_1";
        }

        var currency = assignedcurrency;
        var cont = $('#' + maingrd + ' .' + clsname + '')[0].dataset;

        var aircode = cont.airline.split(' ')[0];
        //var totalhrs = Math.floor((cont.duration % (60 * 60)) / 60);
        //var totalsec = ((cont.duration) % 60);
        //if (totalsec.value < 10) {
        //    totalsec = "0" + totalsec;
        //}

        var totalsec = cont.duration;

        var groupfarereqbuild = "";
        groupfarereqbuild += '<div class="container-fluid" id="mainfrmcom" style="background-color: #e8e7e7; padding: 1px 16px 1px !important; margin-bottom: 15px;">';
        groupfarereqbuild += '<div class="row" style="background:#fff;padding-top:10px;padding-bottom:10px;color:black;"><div class="col-sm-3 col-xs-12 fnt-wt-5" style="text-align: center;white-space:nowrap;"><img src="' + airlinelogourl + '/' + aircode + '.png?' + Versionflag + '"><br><span style="white-space:nowrap;">' + cont.airline + '</span><br><span>' + cont.grpairlinename + '</span></div><div class="col-sm-3 col-xs-6 p-t-15" style="text-alignleft;"><div class="fl fnt-wt-5"><span style="font-weight: 600; font-size: 13px;">' + cont.grporgvcity + '</span><img class="ft" src="' + strImageUrl + '/' + $("#hdn_producttype").val() + '/icons/flight_icon_2.png" style="margin-left: 34%;"><br><span>' + getdayvalue(cont.depdatetime) + '</span></div></div> <div class="col-sm-3 col-xs-6 p-t-15"><div class="ft fnt-wt-5" style="text-align:right;"><img src="' + strImageUrl + '/' + $("#hdn_producttype").val() + '/icons/flight_icon_1.png"><span class="ft" style="font-weight: 600; font-size: 13px;margin-left: 34%;">' + cont.grpdesvcity + '</span><br><span>' + getdayvalue(cont.arrdatetime) + '</span></div></div> <div class="col-sm-3 p-t-10 fnt-wt-5 col-xs-12"><span class="dur txt-cntr fl width-100p"><i class="ti-time fl txt-cntr width-100p" style="margin-bottom:5px;"></i>&nbsp;' + totalsec + '</span></div></div>'
        groupfarereqbuild += "</div>"
        groupfarereqbuild += '<div class="row row0 clsTotalfarecount"><span class="totalgroupcnt">Total Fares : ' + count + '</span></div>'
        groupfarereqbuild += '<div class="row row0 scrollmenu" style="overflow: auto;color:black;box-shadow: 0px 1px 1px 1px #eee;"><div class="col-md-12" style="box-sizing: border-box;" id="dvMultiflights0">'
        groupfarereqbuild += '<table cellspacing="0" cellpadding="0" style="width:100%;"><tbody><tr>'

        for (var i = 0; i < count; i++) {

            var articleid = $('#' + maingrd + ' .' + clsname + '')[i].id.toString().replace("li_Rows", "");
            var cont_loop = $('#' + maingrd + ' .' + clsname + '')[i].dataset;
            var bagg_Det_con = "";
            if ($('#showbagg' + articleid).length > 0) {
                var bagg_Det = $('#showbagg' + articleid)[0].innerHTML;
                bagg_Det_con = true;
            }
            else {
                var bagg_Det = "No Baggage"
                bagg_Det_con = false;
            }
            var farecls = cont_loop.clssname;
            var fare_typ = cont_loop.faretype;

            groupfarereqbuild += '<td class="col-sm-12 col-xs-12 col0" style=float:left;>'
            groupfarereqbuild += '<div class="row brdr-btm-das txt-cntr p-t-15 p-b-15">'
            groupfarereqbuild += '<div class="col-sm-2 ClsHead brdr-rt-das p-t-15 col10 col-xs-12">'
            groupfarereqbuild += '<span class="spanFaretype clsHiddenText"><span>' + farecls + '</span></span>'
            groupfarereqbuild += '<br><br><span style="font-weight:400;font-size:11px;">' + Faretype(fare_typ) + '</span></div>'
            groupfarereqbuild += '<div class="col-sm-4 ClsChild ClsCompGrsAmt blink_me brdr-rt-das p-t-10 col-xs-4">'
            groupfarereqbuild += '<span style="font-size:15px;font-weight:600;">'


            groupfarereqbuild += '<i style="font-size: 10px;font-variant: petite-caps;font-style: normal;">' + currency + '</i>'

            groupfarereqbuild += '&nbsp;' + cont_loop.price + '</span>'


            groupfarereqbuild += ''
            groupfarereqbuild += ''
            groupfarereqbuild += '<br><span style="font-weight:600;font-size:10px;white-space:nowrap;cursor:pointer;color:blue;" onclick="javascript:return GetFareRule_grpfare(' + articleid + ')">FARE RULE</span></div>'
            groupfarereqbuild += '<div class="col-sm-2 ClsHead brdr-rt-das col-xs-4" style="padding:5px 0;"><span style="">'

            if (bagg_Det_con == true) {
                groupfarereqbuild += '<i class="soap-icon-suitcase"></i>'
                groupfarereqbuild += '<br><label style="font-size:10px;color:red;margin-bottom: 0px; font-weight: 600;">' + bagg_Det + '</label></span></div><div class="col-sm-4 p-t-10 col-xs-4">'
            }
            if (bagg_Det_con == false) {
                groupfarereqbuild += '<img src="' + strImageUrl + '/' + $("#hdn_producttype").val() + '/cancelbag.png" alt="' + bagg_Det + '">'
                groupfarereqbuild += '<br><label style="font-size:10px;color:red;">' + bagg_Det + '</label></span></div><div class="col-sm-4 p-t-10 col-xs-4">'
            }
            if (grid == "Dgrd") {

                if (id == articleid) {
                    groupfarereqbuild += '<a id="selectclickbuttongrp' + articleid + '" class=" button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton  seletedcls"  onclick=' + '"' + "javascript:bindSletedRndTrpAvail('" + articleid + "','" + cont_loop.arg + "')" + '"' + '>Selected  </a></div></div></td>'
                }
                else {
                    groupfarereqbuild += '<a id="selectclickbuttongrp' + articleid + '" class="gropbtncls button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton selebtn "  onclick=' + '"' + "javascript:bindSletedRndTrpAvail('" + articleid + "','" + cont_loop.arg + "')" + '"' + '>Select  </a></div></div></td>'
                }
            }
            else {

                if (id == articleid) {

                    groupfarereqbuild += '<a id="selectclickbuttongrp' + articleid + '" class=" button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton  seletedcls"  onclick="javascript:GetRowIndexSelect(' + articleid + ')">Selected  </a></div></div></td>'
                }
                else {

                    groupfarereqbuild += '<a id="selectclickbuttongrp' + articleid + '" class="gropbtncls button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton selebtn "  onclick="javascript:GetRowIndexSelect(' + articleid + ')">Select  </a></div></div></td>'
                }

            }
        }
        groupfarereqbuild += '</tr></tbody></table></div></div>'
        groupfarereqbuild += '</div>';
        groupfarereqbuild += '<div class="container-fluid" id="grpfarerul" style="display:none">';
        groupfarereqbuild += '<div class="row" id="appndfarerule">';
        groupfarereqbuild += '</div>';
        groupfarereqbuild += '</div>';
        sessionStorage.setItem("flggrp", "N");
    }
    else {
        var maingrd = "";
        if (grid == "Dgrd") {

            maingrd = pardiv;
        }
        else {
            maingrd = "availset_O_1";
        }

        var currency = assignedcurrency;
        var cont = $('#' + maingrd + ' .' + clsname + '')[0].dataset;

        var aircode = cont.airline.split(' ')[0];
        //var totalhrs = Math.floor((cont.duration % (60 * 60)) / 60);
        //var totalsec = ((cont.duration) % 60);
        //if (totalsec.value < 10) {
        //    totalsec = "0" + totalsec;
        //}
        var totalsec = cont.duration;

        var groupfarereqbuild = "";
        groupfarereqbuild += '<div class="container-fluid" id="mainfrmcom" style="background-color: #d2d2d2;padding: 15px 30px 15px !important; ">';
        groupfarereqbuild += '<div class="row" style="background:#fff;padding-top:10px;padding-bottom:10px;color:black;"><div class="col-md-3 col-xs-4" style="text-align: center;border-right:dotted #eee;white-space:nowrap;"><span>' + cont.grpairlinename + '</span><br><img src="' + airlinelogourl + '/' + aircode + '.png?' + Versionflag + '"><br><span style="white-space:nowrap;">' + cont.airline + '</span></div><div class="col-md-3 col-xs-4" style="text-align: right;border-right:dotted #eee"><span>' + cont.grporgvcity + '</span><br><span>' + getdayvalue(cont.depdatetime) + '</span></div> <div class="col-md-3 col-xs-4"><span>' + cont.grpdesvcity + '</span><br><span>' + getdayvalue(cont.arrdatetime) + '</span></div> <div class="col-md-3"><span class="dur hidden-xs"><i class="fa fa-clock-o"></i>&nbsp;' + totalsec + '</span></div></div>'
        groupfarereqbuild += "</div>"
        groupfarereqbuild += '<div class="row clsTotalfarecount" style="color:black;text-align: left;font-size: 12px;font-variant: petite-caps;font-weight: bold;padding-top: 3px;padding-bottom: 3px;"><span class="totalgroupcnt">Total Fares : ' + count + '</span></div>'
        groupfarereqbuild += '<div class="row" style="overflow: auto;color:black;"><div class="col-md-12" style="padding: 1px 0px 0px 1px;box-sizing: border-box;" id="dvMultiflights0">'
        groupfarereqbuild += '<table cellspacing="0" cellpadding="0" style="width:100%;background:#fff;"><tbody><tr>'

        for (var i = 0; i < count; i++) {

            var articleid = $('#' + maingrd + ' .' + clsname + '')[i].id.toString().replace("li_Rows", "");
            var cont_loop = $('#' + maingrd + ' .' + clsname + '')[i].dataset;
            var bagg_Det_con = "";
            if ($('#showbagg' + articleid).length > 0) {
                var bagg_Det = $('#showbagg' + articleid)[0].innerHTML;
                bagg_Det_con = true;
            }
            else {
                var bagg_Det = "No Baggage"
                bagg_Det_con = false;
            }
            var farecls = cont_loop.clssname;

            groupfarereqbuild += '<td>'
            groupfarereqbuild += '<div class="row Newflight" style="padding-left:15px ;">'
            groupfarereqbuild += '<div class="col-md-12 ClsHead" style="border: 1px dashed #ddd;">'
            groupfarereqbuild += '<span class="spanFaretype clsHiddenText hidden-xs" style="font-size: 12px;border: 1px dashed #e61a1a;color: black;padding: 12px;"><span style="font-size: 12px;color: black;">' + farecls + '</span></span>'
            groupfarereqbuild += '<br><span style="visibility:hidden;font-weight:400;font-size:11px;">(P - P08AP)</span></div>'
            groupfarereqbuild += '<div class="col-md-12 ClsChild ClsCompGrsAmt blink_me" style="    border: 1px dashed #ddd;border-top: 0px;">'
            groupfarereqbuild += '<span style="font-size:17px;">'


            groupfarereqbuild += '<i style="font-size: 10px;font-variant: petite-caps;font-style: normal;">' + currency + '</i>'

            groupfarereqbuild += '&nbsp;' + cont_loop.price + '</span>'


            groupfarereqbuild += ''
            groupfarereqbuild += ''
            groupfarereqbuild += '<br><span style="font-weight:600;font-size:10px;white-space:nowrap;cursor:pointer;color:#898a02;" onclick="javascript:return GetFareRule_grpfare(' + articleid + ')">FARE RULE</span></div>'
            groupfarereqbuild += '<div class="col-md-12 ClsHead" style="height:40px;border: 1px dashed #ddd;border-top: 0px;"><span style="">'

            if (bagg_Det_con == true) {
                groupfarereqbuild += '<i class="soap-icon-suitcase" style="font-size: 20px;"></i>'
                groupfarereqbuild += '<br><label style="font-size:10px;color:red;">' + bagg_Det + '</label></span></div></div><div class="row Newflight clsSelectBox">'
            }
            if (bagg_Det_con == false) {
                groupfarereqbuild += '<img src="' + strImageUrl + '/' + $("#hdn_producttype").val() + '/cancelbag.png" alt="' + bagg_Det + '">'
                groupfarereqbuild += '<br><label style="font-size:10px;color:red;">' + bagg_Det + '</label></span></div></div><div class="row Newflight clsSelectBox">'
            }
            if (grid == "Dgrd") {

                if (id == articleid) {
                    groupfarereqbuild += '<a id="selectclickbuttongrp' + articleid + '" class=" button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton  seletedcls"  onclick=' + '"' + "javascript:bindSletedRndTrpAvail('" + articleid + "','" + cont_loop.arg + "')" + '"' + '>Selected  </a></div></td>'
                }
                else {
                    groupfarereqbuild += '<a id="selectclickbuttongrp' + articleid + '" class="gropbtncls button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton selebtn "  onclick=' + '"' + "javascript:bindSletedRndTrpAvail('" + articleid + "','" + cont_loop.arg + "')" + '"' + '>Select  </a></div></td>'
                }
            }
            else {

                if (id == articleid) {

                    groupfarereqbuild += '<a id="selectclickbuttongrp' + articleid + '" class=" button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton  seletedcls"  onclick="javascript:GetRowIndexSelect(' + articleid + ')" >Selected  </a></div></td>'
                }
                else {

                    groupfarereqbuild += '<a id="selectclickbuttongrp' + articleid + '" class="gropbtncls button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton selebtn "  onclick="javascript:GetRowIndexSelect(' + articleid + ')">Select  </a></div></td>'
                }

            }
        }
        groupfarereqbuild += '</tr></tbody></table></div></div>'
        groupfarereqbuild += '</div>';
        groupfarereqbuild += '<div class="container-fluid" id="grpfarerul" style="display:none">';
        groupfarereqbuild += '<div class="row" id="appndfarerule">';
        groupfarereqbuild += '</div>';
        groupfarereqbuild += '</div>';
        sessionStorage.setItem("flggrp", "N");

    }
    $('body').data('BackFlag', 'FAREPOPUP');
    showpopuplogin("Fares Comparable", "", groupfarereqbuild, true, 750, "");


}

function getdayvalue(Date) {
    var Returnval = "";
    try {
        var weekdays = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        var FullWeekdays = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        var D_date = moment(Date);
        var D_dow = D_date.day();
        var D_weekday = $("#hdn_AppHosting").val() == "BSA" ? FullWeekdays[D_dow] : weekdays[D_dow];
        //var D_weekday = weekdays[D_dow];
        Returnval = $("#hdn_AppHosting").val() == "BSA" ? D_weekday + ", " + Date : Date + ", " + D_weekday;
    }
    catch (e) {
        Returnval = Date;
    }
    return Returnval;
}

function Faretype(type) {
    var FareTypename = '';

    if (type.toUpperCase() == "C")
        FareTypename = "<i class='Faretype-CorRet' data-toggle='tooltip' data-placement='top' title='Corporate Fare' >C</i> <span class='spanFaretype hidden-xs hidden-sm'>Corporate Fare</span> ";
    else if (type.toUpperCase() == "R")
        FareTypename = "<i class='Faretype-RetFare' data-toggle='tooltip' data-placement='top' title='Retail Fare' >R</i> <span class='spanFaretype hidden-xs hidden-sm'>Retail Fare</span>";
    else if (type.toUpperCase() == "U")
        FareTypename = "<i class='Faretype-CorRet' data-toggle='tooltip' data-placement='top' title='SME Corporate Fare' >U</i> <span class='spanFaretype hidden-xs hidden-sm'>SME Corporate Fare</span>";
    else if (type.toUpperCase() == "V")
        FareTypename = "<i class='Faretype-CorRet' data-toggle='tooltip' data-placement='top' title='SME Retail Fare' >V</i> <span class='spanFaretype hidden-xs hidden-sm'>SME Retail Fare</span>";
    else if (type.toUpperCase() == "E")
        FareTypename = "<i class='Faretype-Special' data-toggle='tooltip' data-placement='top' title='Coupon Fare' >E</i> <span class='spanFaretype hidden-xs hidden-sm'>Coupon Fare</span>";
    else if (type.toUpperCase() == "S" || type.toUpperCase() == "L")
        FareTypename = "<i class='Faretype-SpecialFare' data-toggle='tooltip' data-placement='top' title='Special Fare' >S</i> <span class='spanFaretype hidden-xs hidden-sm'>Special Fare</span>";
    else if (type.toUpperCase() == "F")
        FareTypename = "<i class='Faretype-Flexifare' data-toggle='tooltip' data-placement='top' title='Flexi Fare' >F</i> <span class='spanFaretype hidden-xs hidden-sm'>Flexi Fare</span>";
    else if (type.toUpperCase() == "B")
        FareTypename = "<i class='Faretype-CorRet' data-toggle='tooltip' data-placement='top' title='Business Fare' >B</i> <span class='spanFaretype hidden-xs hidden-sm' >Booklet Fare</span>";
    else if (type.toUpperCase() == "H")
        FareTypename = "<i class='Faretype-Normalfare' data-toggle='tooltip' data-placement='top' title='Hand Baggage' >H</i> <span class='spanFaretype hidden-xs hidden-sm'>Hand Baggage</span>";
    else if (type.toUpperCase() == "P" || type.toUpperCase() == "W")
        FareTypename = "<i class='Faretype-CorRet' data-toggle='tooltip' data-placement='top' title='Flat Fare' >" + type.toUpperCase() + "</i> <span class='spanFaretype hidden-xs hidden-sm'>Flat Fare</span>";
    else if (type.toUpperCase() == "Q")
        FareTypename = "<i class='Faretype-SMEfare' data-toggle='tooltip' data-placement='top' title='SME Fare' >Q</i> <span class='spanFaretype hidden-xs hidden-sm'>SME Fare</span>";
    else if (type.toUpperCase() == "M")
        FareTypename = "<i class='Faretype-SMEfare' data-toggle='tooltip' data-placement='top' title='SME Fare' >M</i> <span class='spanFaretype hidden-xs hidden-sm'>SME Fare</span>";
    else if (type.toUpperCase() == "G")
        FareTypename = "<i class='Faretype-SupSpecial' data-toggle='tooltip' data-placement='top' title='Marine Fare'  >G</i> <span class='spanFaretype hidden-xs hidden-sm'>Marine Fare</span>";
    else if (type.toUpperCase() == "A")
        FareTypename = "<i class='Faretype-Normalfare' data-toggle='tooltip' data-placement='top' title='Normal Fare' >A</i><span class='spanFaretype hidden-xs hidden-sm' >Normal Fare</span>";
    else if (type.toUpperCase() == "N")
        FareTypename = "<i class='Faretype-Normalfare' data-toggle='tooltip' data-placement='top' title='Normal Fare' >N</i><span class='spanFaretype hidden-xs hidden-sm' >Normal Fare</span>";
    else if (type.toUpperCase() == "I")
        FareTypename = "<i class='Faretype-CorRet' data-toggle='tooltip' data-placement='top' title='SME Corporate Fare' >I</i><span class='spanFaretype hidden-xs hidden-sm' >SME Corporate Fare</span>";
    else if (type.toUpperCase() == "J")
        FareTypename = "<i class='Faretype-CorRet' data-toggle='tooltip' data-placement='top' title='SME Retail Fare' >J</i><span class='spanFaretype hidden-xs hidden-sm' >SME Retail Fare</span>";
    else {
        if (type != "")
            FareTypename = "<i class='Faretype-Normal' data-placement='top'  >" + type.toUpperCase() + "</i><span class='spanFaretype hidden-xs hidden-sm'></span>";
        else
            FareTypename = "";
    }

    return FareTypename;
}


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



function calculateageinsurance(birthday, Paxtype) {
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
            lblAge = b.diff(a, 'days')
        }
        else {
            showerralert("Date must be dd/mm/yyyy format!", "", "");
            return false;
        }
    }
    lblAge = lblAge / 365.2425;
    lblAge = lblAge.toFixed(2);
    return lblAge;
}


function GetFareRule_grpfare(val) {

    var idss = val;
    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == idss;
    });

    AirCategory = selectedObj[0]["data-AirlineCategory"];// $('.Travselect_' + val)[0].dataset.airlinecategory;// document.getElementById('AirlineCategory' + val).value;
    AirCategory11 = selectedObj[0]["data-hdInva"];//$('.Travselect_' + val)[0].dataset.hdinva; // document.getElementById('hdInva' + val).value;
    var Flightno = document.getElementById('FlightID' + val).innerHTML;
    var sfcthread = document.getElementById("hdnsfcthread").value;
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: getfarerule, 		// Location of the service
        data: '{FlightFareRuleid: "' + Flightno + '", FareRuleAvailstring: "' + AirCategory11 + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (json) {//On Successful service call
            var result = json.Result;
            $.unblockUI();
            FareRuleFunctionBack_grpfare(result, Flightno);

        },
        error: function (e) {
            $.unblockUI();
            LogDetails(e.responseText, e.status, "Flight Select");
            if (e.status == "500") {
                //alert("Session has been Expired.");
                window.location = sessionExb;
                return false;
            }
        }
    });

}

function FareRuleFunctionBack_grpfare(arg, Flightno) {

    var strFareRuleBulid = ""
    $('#FareRuleLodingImage').hide();
    $('#closeimage').show();
    var arycategory = AirCategory11.split('SpLITSaTIS');
    var category2 = arycategory[arycategory.length - 1].split('SpLitPResna');
    if (arg[1] != "" && arg[1] != null) {
        var category = AirCategory.split("SpLitPResna");
        var arycategory = AirCategory11.split('SpLITSaTIS');
        var category2 = arycategory[arycategory.length - 1].split('SpLitPResna');
        var Ititle = "";
        var Isubtitle = "";
        var IContent = "";
        var Ifullopt = false;
        var Itemp1 = "";
        var Itemp2 = "";
        if (category[6] == "FSC" || category[6] == "1A" || category[6] == "1S") {
            var Resultss = $.xml2json(arg[1]);
            var uniqueNames = [];
            var TTrounddetails = [];
            if (Resultss["Table1"].length > 0) {
                for (var i = 0; Resultss["Table1"].length > i; i++) {
                    TTrounddetails.push(Resultss["Table1"][i].SEGMENT);
                }
            }

            if (TTrounddetails.length > 0) {
                $.each(TTrounddetails, function (i, el) {
                    if ($.inArray(el, uniqueNames) === -1) uniqueNames.push(el);
                });
            }
            var new_arr = [];
            var new_arrtext = [];
            if (uniqueNames.length > 0) {
                for (var K = 0; K < uniqueNames.length ; K++) {
                    new_arr = $.grep(Resultss["Table1"], function (n, i) {
                        return n.SEGMENT == uniqueNames[K];

                    });
                    new_arrtext.push(new_arr);
                }
            }

            var str = '';
            if (new_arrtext.length > 0) {

                for (var j = 0; j < new_arrtext.length ; j++) {
                    if (j == 0) {
                        str += '<span class="clsAllFarerule clsFareruleSelect" id="spnFareSelect' + j + '" onclick="javascript:ShowSegWiseFareRule(' + j + ');">' + new_arrtext[j][0].SEGMENT + '</span>';
                    }
                    else {
                        str += '<span class="clsAllFarerule clsFarerule" id="spnFareSelect' + j + '" onclick="javascript:ShowSegWiseFareRule(' + j + ');">' + new_arrtext[j][0].SEGMENT + '</span>';
                    }
                }

                str += '</div>';
                for (var i = 0; i < new_arrtext.length ; i++) {
                    if (i == 0) {
                        str += '<div  class="clsSgFareRule"  id="dvFareRuletbl_' + i + '"  >';
                    }
                    else {
                        str += '<div  class="clsSgFareRule"  id="dvFareRuletbl_' + i + '" style="display:none" >';
                    }
                    for (var k = 0; k < new_arrtext[i].length ; k++) {
                        str += '<div class="clsRuleHead" style="cursor:pointer;" id="dvcheck_' + k + 'fr' + i + '" onclick="javascript:CloseSegWiseFareRule(this.id);">' + new_arrtext[i][k].RULE + '<i class="fa fa-chevron-down"></i></div>';
                        str += '<div style="width:100%;border-bottom: 1px dashed;">';
                        str += '<pre readonly="readonly" class="clsFaretextArea" id="' + k + 'fr' + i + '" style="display: none;">' + new_arrtext[i][k].TEXT + '</pre>';
                        str += '</div>';
                    }
                    str += '</div>';

                }
            }
            //     Ititle = "Fare Rule " + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
            //Isubtitle = "";
            //IContent = str;
            //Ifullopt = false;
            //Itemp1 = "";
            //Itemp2 = "";
            //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);

            $("#appndfarerule").html("");
            $("#appndfarerule").append(str);
            $("#grpfarerul").css("display", "block");
            $("#mainfrmcom").css("display", "none");



        }
        else if (category[0] == "LCC") {
            //Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
            //Isubtitle = "";
            //IContent = "<pre>" + arg[1] + "</pre>";
            //Ifullopt = false;
            //Itemp1 = "";
            //Itemp2 = "";
            //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            $("#appndfarerule").html("");
            $("#appndfarerule").append("<div class='col-xs-12 col-sm-12 ' style='text-align: right;'><i class='fa fa-close' style='font-size:25px;color:red' onclick='functionclsfare()'></i></div><pre>" + arg[1] + "</pre>");
            $("#grpfarerul").css("display", "block");
            $("#mainfrmcom").css("display", "none");
        }
        else if (category[6] == "TZ" || category[6] == "6E") {
            var Air_json = JSON.parse(arg[1]);
            strFareRuleBulid += "<table>";
            for (var jsoncount = 0; jsoncount < Air_json["FARERULE"].length; jsoncount++) {
                strFareRuleBulid += "<tr><td width=25%><pre style='white-space: pre-wrap;'>  " + Air_json["FARERULE"][jsoncount]["TEXT"] + "</pre> </td></tr>";
            }
            strFareRuleBulid += "</table>";
            //Ititle = "Fare Rule " + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
            //Isubtitle = "";
            //IContent = strFareRuleBulid;
            //Ifullopt = false;
            //Itemp1 = "";
            //Itemp2 = "";
            //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);

            $("#appndfarerule").html("");
            $("#appndfarerule").append("<div class='col-xs-12 col-sm-12 ' style='text-align: right;'><i class='fa fa-close' style='font-size:25px;color:red' onclick='functionclsfare()'></i></div><pre>" + strFareRuleBulid + "</pre>");
            $("#grpfarerul").css("display", "block");
            $("#mainfrmcom").css("display", "none");

        }
        else if (category[5] == arg[2]) {
            //Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
            //Isubtitle = "";
            //IContent = "<pre>" + arg[1] + "</pre>";
            //Ifullopt = false;
            //Itemp1 = "";
            //Itemp2 = "";
            //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);

            $("#appndfarerule").html("");
            $("#appndfarerule").append("<div class='col-xs-12 col-sm-12 ' style='text-align: right;'><i class='fa fa-close' style='font-size:25px;color:red' onclick='functionclsfare()'></i></div><pre>" + arg[1] + "</pre>");
            $("#grpfarerul").css("display", "block");
            $("#mainfrmcom").css("display", "none");
        }

        else if (category[6] == "UAI") {
            if (category[5] == arg[2]) {
                //Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
                //Isubtitle = "";
                //IContent = "<pre>" + arg[1] + "</pre>";
                //Ifullopt = false;
                //Itemp1 = "";
                //Itemp2 = "";
                //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);

                $("#appndfarerule").html("");
                $("#appndfarerule").append("<div class='col-xs-12 col-sm-12 ' style='text-align: right;'><i class='fa fa-close' style='font-size:25px;color:red' onclick='functionclsfare()'></i></div><pre>" + arg[1] + "</pre>");
                $("#grpfarerul").css("display", "block");
                $("#mainfrmcom").css("display", "none");

            }
            else {
                var Resultss = $.xml2json(arg[1]);
                var uniqueNames = [];
                var TTrounddetails = [];
                if (Resultss["Table1"].length > 0) {
                    for (var i = 0; Resultss["Table1"].length > i; i++) {
                        TTrounddetails.push(Resultss["Table1"][i].SEGMENT);
                    }
                }

                if (TTrounddetails.length > 0) {
                    $.each(TTrounddetails, function (i, el) {
                        if ($.inArray(el, uniqueNames) === -1) uniqueNames.push(el);
                    });
                }
                var new_arr = [];
                var new_arrtext = [];
                if (uniqueNames.length > 0) {
                    for (var K = 0; K < uniqueNames.length ; K++) {
                        new_arr = $.grep(Resultss["Table1"], function (n, i) {
                            return n.SEGMENT == uniqueNames[K];

                        });
                        new_arrtext.push(new_arr);
                    }
                }
                var str = '';
                if (new_arrtext.length > 0) {

                    for (var j = 0; j < new_arrtext.length ; j++) {
                        if (j == 0) {
                            str += '<span class="clsAllFarerule clsFareruleSelect" id="spnFareSelect' + j + '" onclick="javascript:ShowSegWiseFareRule(' + j + ');">' + new_arrtext[j][0].SEGMENT + '</span>';
                        }
                        else {
                            str += '<span class="clsAllFarerule clsFarerule" id="spnFareSelect' + j + '" onclick="javascript:ShowSegWiseFareRule(' + j + ');">' + new_arrtext[j][0].SEGMENT + '</span>';
                        }
                    }

                    str += '</div>';
                    for (var i = 0; i < new_arrtext.length ; i++) {
                        if (i == 0) {
                            str += '<div  class="clsSgFareRule"  id="dvFareRuletbl_' + i + '"  >';
                        }
                        else {
                            str += '<div  class="clsSgFareRule"  id="dvFareRuletbl_' + i + '" style="display:none" >';
                        }
                        for (var k = 0; k < new_arrtext[i].length ; k++) {
                            str += '<div class="clsRuleHead" style="cursor:pointer;" id="dvcheck_' + k + 'fr' + i + '" onclick="javascript:CloseSegWiseFareRule(this.id);">' + new_arrtext[i][k].RULE + '<i class="fa fa-chevron-down"></i></div>';
                            str += '<div style="width:100%;border-bottom: 1px dashed;">';
                            str += '<pre readonly="readonly" class="clsFaretextArea" id="' + k + 'fr' + i + '" style="display: none;">' + new_arrtext[i][k].TEXT + '</pre>';
                            str += '</div>';
                        }
                        str += '</div>';

                    }

                    // str += '<div class="clsSpanText"><span class="clsHeadNote">Note : </span>' + Resultss["IMPORTANT_NOTE"].NOTE + '</div>';
                }
                //Ititle = "Fare Rule " + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
                //Isubtitle = "";
                //IContent = str;
                //Ifullopt = false;
                //Itemp1 = "";
                //Itemp2 = "";
                //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);

                $("#appndfarerule").html("");
                $("#appndfarerule").append("<div class='col-xs-12 col-sm-12 ' style='text-align: right;'><i class='fa fa-close' style='font-size:25px;color:red' onclick='functionclsfare()'></i></div><pre>" + str + "</pre>");
                $("#grpfarerul").css("display", "block");
                $("#mainfrmcom").css("display", "none");


            }

        }
        else if (category[6] == "LCC") {
            //Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
            //Isubtitle = "";
            //IContent = "<pre>" + arg[1] + "</pre>";
            //Ifullopt = false;
            //Itemp1 = "";
            //Itemp2 = "";
            //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            $("#appndfarerule").html("");
            $("#appndfarerule").append("<div class='col-xs-12 col-sm-12 ' style='text-align: right;'><i class='fa fa-close' style='font-size:25px;color:red' onclick='functionclsfare()'></i></div><pre>" + arg[1] + "</pre>");
            $("#grpfarerul").css("display", "block");
            $("#mainfrmcom").css("display", "none");


        }
        else if (category[6] == "LCCG9" || category[6] == "G9" || category[6] == "FZ") {
            //Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
            //Isubtitle = "";
            //IContent = "<pre>" + arg[1] + "</pre>";
            //Ifullopt = false;
            //Itemp1 = "";
            //Itemp2 = "";
            //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            $("#appndfarerule").html("");
            $("#appndfarerule").append("<div class='col-xs-12 col-sm-12 ' style='text-align: right;'><i class='fa fa-close' style='font-size:25px;color:red' onclick='functionclsfare()'></i></div><pre>" + arg[1] + "</pre>");
            $("#grpfarerul").css("display", "block");
            $("#mainfrmcom").css("display", "none");


        }
        else if (category[6] == arg[2]) {
            //Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
            //Isubtitle = "";
            //IContent = "<pre>" + arg[1] + "</pre>";
            //Ifullopt = false;
            //Itemp1 = "";
            //Itemp2 = "";
            //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);


            $("#appndfarerule").html("");
            $("#appndfarerule").append("<div class='col-xs-12 col-sm-12 ' style='text-align: right;'><i class='fa fa-close' style='font-size:25px;color:red' onclick='functionclsfare()'></i></div><pre>" + arg[1] + "</pre>");
            $("#grpfarerul").css("display", "block");
            $("#mainfrmcom").css("display", "none");

        }
        else if (category[6] == "OSC" || category[6] == "DXB") {
            var Air_json = JSON.parse(arg[1]);
            strFareRuleBulid += "<table>";
            for (var jsoncount = 0; jsoncount < Air_json["FARERULE"].length; jsoncount++) {
                strFareRuleBulid += "<tr><td width=10%>" + Air_json["FARERULE"][jsoncount]["RULE"].replace("F.", "") + "</td><td width=25%><pre>  " + Air_json["FARERULE"][jsoncount]["TEXT"] + "</pre> </td></tr>";
            }
            strFareRuleBulid += "</table>";
            //Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
            //Isubtitle = "";
            //IContent = strFareRuleBulid;
            //Ifullopt = false;
            //Itemp1 = "";
            //Itemp2 = "";
            //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            $("#appndfarerule").html("");
            $("#appndfarerule").append("<div class='col-xs-12 col-sm-12 ' style='text-align: right;'><i class='fa fa-close' style='font-size:25px;color:red' onclick='functionclsfare()'></i></div><pre>" + strFareRuleBulid + "</pre>");
            $("#grpfarerul").css("display", "block");
            $("#mainfrmcom").css("display", "none");



        }
        else if (category[6] == "RST") {
            //Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
            //Isubtitle = "";
            //IContent = "<pre>" + arg[1] + "</pre>";
            //Ifullopt = false;
            //Itemp1 = "";
            //Itemp2 = "";
            //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            $("#appndfarerule").html("");
            $("#appndfarerule").append("<div class='col-xs-12 col-sm-12 ' style='text-align: right;'><i class='fa fa-close' style='font-size:25px;color:red' onclick='functionclsfare()'></i></div><pre>" + arg[1] + "</pre>");
            $("#grpfarerul").css("display", "block");
            $("#mainfrmcom").css("display", "none");
        }
        else if (category[6] == "IRST") {
            //Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
            //Isubtitle = "";
            //IContent = "<pre>" + arg[1] + "</pre>";
            //Ifullopt = false;
            //Itemp1 = "";
            //Itemp2 = "";
            //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            $("#appndfarerule").html("");
            $("#appndfarerule").append("<div class='col-xs-12 col-sm-12 ' style='text-align: right;'><i class='fa fa-close' style='font-size:25px;color:red' onclick='functionclsfare()'></i></div><pre>" + arg[1] + "</pre>");
            $("#grpfarerul").css("display", "block");
            $("#mainfrmcom").css("display", "none");

        }
        else {
            //Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
            //Isubtitle = "";
            //IContent = "<pre>" + arg[0] + "</pre>";
            //Ifullopt = false;
            //Itemp1 = "";
            //Itemp2 = "";
            //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            $("#appndfarerule").html("");
            $("#appndfarerule").append("<div class='col-xs-12 col-sm-12 ' style='text-align: right;'><i class='fa fa-close' style='font-size:25px;color:red' onclick='functionclsfare()'></i></div><pre>" + arg[0] + "</pre>");
            $("#grpfarerul").css("display", "block");
            $("#mainfrmcom").css("display", "none");
        }

    }
    else {

        var category = AirCategory.split("SpLitPResna");
        //Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
        //Isubtitle = "";
        //IContent = "<pre>" + arg[0] + "</pre>";
        //Ifullopt = false;
        //Itemp1 = "";
        //Itemp2 = "";
        //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
        $("#appndfarerule").html("");
        $("#appndfarerule").append("<div class='col-xs-12 col-sm-12 ' style='text-align: right;'><i class='fa fa-close' style='font-size:25px;color:red' onclick='functionclsfare()'></i></div><pre>" + arg[0] + "</pre>");
        $("#grpfarerul").css("display", "block");
        $("#mainfrmcom").css("display", "none");
    }
}



function functionclsfare() {

    $("#grpfarerul").css("display", "none");
    $("#mainfrmcom").css("display", "block");
}

function timeConvert(n) {
    var num = n;
    var hours = (num / 60);
    var rhours = Math.floor(hours);
    var minutes = (hours - rhours) * 60;
    var rminutes = Math.round(minutes);
    return rhours + " hour(s) and " + rminutes + " minute(s).";
}

function updateservicecharge() {


    var bValid = true;
    //allFields.removeClass("ui-state-error");
    //$('#lod').css("display", "block");
    this.enabled = false;
    if (document.getElementById("hdnadultservice_").value == document.getElementById("txtadult").value && document.getElementById("hdnchildservice_").value == document.getElementById("txtChild").value && document.getElementById("hdninfservice_").value == document.getElementById("txtinfant").value) {
        $("#modal-servicecharge").modal("hide");
        //if (sessionStorage.getItem("retvaldet") == "YES") {

        //    bookins(sessionStorage.getItem("insvalues"));
        //}
    }
    else {
        var adultamt = $("#txtadult").val();
        var childamt = $("#txtChild").val();
        var infantamt = $("#txtinfant").val();
        var spnr = $("#SPNR").val();
        var adt_markup = 0;
        var chd_markup = 0;
        var inf_markup = 0;
        if (tracklength > 1) {
            adultamt = $("#txtadult").val() + "|" + $("#txtadult_roundtrip").val();
            childamt = $("#txtChild").val() + "|" + $("#txtChild_roundtrip").val();
            infantamt = $("#txtinfant").val() + "|" + $("#txtinfant_roundtrip").val();
            spnr = $("#SPNR").val();
            spnr = spnr.replace("/", "|");
            var adt_markup = 0 + "|" + 0;
            var chd_markup = 0 + "|" + 0;
            var inf_markup = 0 + "|" + 0;
        }



        $("#iLoading").css("display", "block");

        $.ajax({
            type: "POST", 		//GET or POST or PUT or DELETE verb
            url: servicechargeupdate, 		// Location of the service
            data: '{AdultAmount: "' + adultamt + '" ,ChildAmount: "' + childamt + '" ,InfantAmount: "' + infantamt + '",SPNR: "' + spnr + '",Adultmarkup:"' + adt_markup + '",Childmarkup:"' + chd_markup + '",Infantmarkup:"' + inf_markup + '"}',//,CancelationStatus: "' + $("#hdnPax")[0].value + '" ,BlockTicket: "' + BlockTicket + '",PaymentMode: "' + $("#ddlPaymode").val() + '",TourCode: "' + $("#Tour_Code").val() + '"}',
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (json) {//On Successful service call

                var result = json.Result
                $("#iLoading").css("display", "none");
                if (result[1] != "") {
                    bValid = result[1];
                    var adt_service_amt = 0;
                    var chd_service_amt = 0;
                    var inf_service_amt = 0;


                    var adt_service_amt_roundtrip = 0;
                    var chd_service_amt_roundtrip = 0;
                    var inf_service_amt_roundtrip = 0;



                    if ($("#txtadult").val() != null && $("#txtadult").val() != "" && $("#txtadult").val() != "0") {
                        adt_service_amt += parseInt($("#txtadult").val()) * parseInt($("#TAdultcnt")[0].innerHTML);
                    }
                    if ($("#txtChild").val() != null && $("#txtChild").val() != "" && $("#txtChild").val() != "0") {
                        chd_service_amt += parseInt($("#txtChild").val()) * parseInt($("#TChildcnt")[0].innerHTML);
                    }
                    if ($("#txtinfant").val() != null && $("#txtinfant").val() != "" && $("#txtinfant").val() != "0") {
                        inf_service_amt += parseInt($("#txtinfant").val()) * parseInt($("#Tinfantcnt")[0].innerHTML);
                    }


                    if (tracklength > 1) {
                        if ($("#txtadult_roundtrip").val() != null && $("#txtadult_roundtrip").val() != "" && $("#txtadult_roundtrip").val() != "0") {
                            adt_service_amt_roundtrip += parseInt($("#txtadult_roundtrip").val()) * parseInt($("#TAdultcnt_roundtrip")[0].innerHTML);
                        }
                        if ($("#txtChild").val() != null && $("#txtChild_roundtrip").val() != "" && $("#txtChild_roundtrip").val() != "0") {
                            chd_service_amt_roundtrip += parseInt($("#txtChild").val()) * parseInt($("#TChildcnt_roundtrip")[0].innerHTML);
                        }
                        if ($("#txtinfant").val() != null && $("#txtinfant_roundtrip").val() != "" && $("#txtinfant_roundtrip").val() != "0") {
                            inf_service_amt_roundtrip += parseInt($("#txtinfant").val()) * parseInt($("#Tinfantcnt_roundtrip")[0].innerHTML);
                        }

                    }


                    var totalamt = $("#currencyupda")[0].innerHTML;
                    $("#currencyupda").html('');

                    $("#currencyupda").html(parseInt(totalamt.trim()) + parseInt(adt_service_amt) + parseInt(chd_service_amt) + parseInt(inf_service_amt) + parseInt(adt_service_amt_roundtrip) + parseInt(chd_service_amt_roundtrip) + parseInt(inf_service_amt_roundtrip));

                    alert(result[1]);

                }
                else {
                    bValid = result[1];
                    showerralert(result[0]);
                    // alert(result[0]);
                    //  $.unblockUI();

                }
                $("#modal-servicecharge").modal("hide");
                //if (sessionStorage.getItem("retvaldet") == "YES") {

                //    bookins(sessionStorage.getItem("insvalues"));
                //}
            },
            error: function (e) {//On Successful service call            

                LogDetails(e.responseText, e.status, "InsertServiceAmount");

                if (e.status == "500") {
                    alert("Session has been Expired.");
                    window.top.location = sessionExb;
                    return false;
                }
                $("#modal-servicecharge").modal("hide");

            }	// When Service call fails

        });

    }
    //$("#modal-servicecharge").modal("hide");


}

function mealbaggapreifor(SSR, Ids_split, Pax, otherssr_pre, book_seatarr, textid) {
    // book_seatarr = "1~1~5A~300~Adult1~5A~0~DEL~MAA~$1~2~6A~300~Adult1~6A~1~MAA~DEL~$2~1~5B~250~Child1~5B~0~DEL~MAA~$2~2~6B~250~Child1~6B~1~MAA~DEL~$";
    try {
        var str = "";
        var ssrprev = "";
        var sedr = "";
        var meal = "";
        var org_Des_arr = "";
        var destination = "";
        var orgin = "";
        var baggage = "";
        var otherssr_arr = "";
        var others = "";
        var seat_arr = "";



        var seat = '-';
        if (SSR != null && SSR != "") {
            ssrprev = SSR.split('SpLiTSSR');
        }
        orgin = $("#hdtxt_origin").val();
        destination = $("#hdtxt_destination").val();
        if (otherssr_pre != null && otherssr_pre != "" && otherssr_pre.length > 0) {
            otherssrarr = otherssr_pre.split('~');
        }
        if (book_seatarr != null && book_seatarr != "" && book_seatarr.length > 0) {
            seat_arr = book_seatarr.split('$');
        }

        //Other SSR
        var OtherSSR = "";
        if (otherssr_pre != null && otherssr_pre != "" && otherssr_pre.length > 0) {
            otherssrarr = otherssr_pre.split('~');
            for (var j = 0; j < otherssrarr.length; j++) {
                var others = otherssrarr[j].split('WEbMeaLWEB');
                var city = others[4] + "_" + others[5];
                var citys = (city == orgin + "_" + destination ? orgin + "_" + destination : destination + "_" + orgin);
                if (city == citys) {
                    var SGorht = others[0] != null && others[0] != "" ? others[0].toString() : "";
                    OtherSSR += '<span class="seat1">' + SGorht.split('|')[0] + ' </span>';
                }
                else if (others[0] != null && others[0] != "") {
                    var SGorht = others[0] != null && others[0] != "" ? others[0].toString() : "";
                    OtherSSR += '<span class="seat1">' + SGorht.split('|')[0] + ' </span>';
                }
            }
        }
        else {
            OtherSSR += '<span class="seat1">- </span>';
        }
        //Other SSR

        //SEAT
        var SeatSSR = "";
        if (seat_arr.length > 0) {
            var newseat_ary = $.grep(seat_arr, function (v, j) {
                return v.split("~").length > 4 && v.split("~")[4] == Pax + (parseInt(Ids_split) + parseInt(1));
            });
            if (newseat_ary.length > 0) {
                var viewseats = "";
                $.map(newseat_ary, function (r, t) {
                    viewseats += r.split("~")[2] + ",";
                });
                SeatSSR += '<span class="seat1">' + viewseats + ' </span>'
            }
            else {
                SeatSSR += '<span class="seat1"> - </span>';
            }
        }
        else {
            SeatSSR += '<span class="seat1"> - </span>';
        }
        //SEAT

        //BAGGAGE AND MEALS
        if (ssrprev != null && ssrprev != "" && ssrprev.length > 0) {
            var paxmeal = "";
            var paxbagg = "";
            for (var i = 0; i < ssrprev.length; i++) {
                if (ssrprev[i] != "~") {
                    if (ssrprev[i] != '0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0' && ssrprev[i] != '~0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0') {
                        meal = ssrprev[i].split('WEbMeaLWEB');
                        org_Des_arr = meal[meal.length - 1].split('MEALSRSPLITbaGG');
                        orgin = org_Des_arr[1];
                        destination = org_Des_arr[2];
                    }
                    else {
                        meal = meal.length = 0
                        orgin = $("#hdtxt_origin").val();
                        destination = $("#hdtxt_destination").val();
                    }
                    i++;
                    if (ssrprev[i] != '0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0' && ssrprev[i] != '~0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0') {
                        if (ssrprev.length > i) {
                            baggage = ssrprev[i].split('WEbMeaLWEB');
                            org_Des_arr = baggage[baggage.length - 2].split('BAggSPLITbaGG');
                            if (orgin == null || orgin == "") {
                                orgin = org_Des_arr[1];
                                destination = org_Des_arr[2];
                            }

                        }
                    }
                    else {
                        baggage = baggage.length = 0;// ssrprev[1].split('WEbMeaLWEB');
                        orgin = $("#hdtxt_origin").val();
                        destination = $("#hdtxt_destination").val();
                    }
                    // meal[0].toString().split('@')[0].replace(new RegExp("~" , "g"), "");
                    paxmeal += meal.length > 0 && meal[0].toString().split('@')[0] != "0" ? meal[0].toString().split('@')[0].replace(new RegExp("~", "g"), "") + "," : "";
                    paxbagg += baggage.length > 0 ? baggage[0].toString().split('@')[0] + "," : "";

                }
            }
        }
        else {
            paxbagg = "-";
            paxmeal = "-";
        }
        //BAGGAGE AND MEALS

        str += '<div class="airline-details less-pad brdr-btm-res" id="pre_' + textid + '_' + Ids_split + '">'
        str += '<div class="row margin-0-res">'
        str += '<div class="col-xs-12 col-sm-3 mg-btm-15-res box-shd pos-rel pad-0-res">' //01 
        str += '<div class="pass-info font-13" id="dv' + textid + '_details_' + Ids_split + '" onclick="SlideToggle(this.id,\'' + textid + '_details_' + Ids_split + '\')">'
        str += '<b class="flt-lft-res font-13">' + Pax + '-' + (parseInt(Ids_split) + parseInt(1)) + ' </b> '
        str += '<p class="p-name mb-0 flt-lft-res pl-10-res" title ="' + $("#" + textid + "_Title_" + Ids_split).val() + '. ' + $("#" + textid + "_firstName_" + Ids_split).val() + ' ' + $("#" + textid + "_lastName_" + Ids_split).val() + '" >' + $("#" + textid + "_Title_" + Ids_split).val() + '. ' + $("#" + textid + "_firstName_" + Ids_split).val() + ' ' + $("#" + textid + "_lastName_" + Ids_split).val() + '</p>'
        str += '<span class="pos-abs dis-nonemin hgt-15" style="right: 10px;"><i class="fa fa-angle-down themeclr-red font-20"></i></span>';
        //str += '<span class="way1"><b>' + orgin + ' - ' + destination + '</b></span>'

        str += '</div>'
        str += '</div>'

        str += '<div id="' + textid + '_details_' + Ids_split + '" class="dis-blockmin" style="display:none;">'
        str += '<div class="col-xs-12 col-sm-3 mg-btm-15-res">' //01
        str += '<div class="meal font-13">'
        str += '<p>Meals</p>'
        str += '<span class="meal1" data-toggle="tooltip" data-title ="' + paxmeal + '"  data-original-title="' + paxmeal + '"data-placement="bottom">' + paxmeal == "" ? "-" : paxmeal + '</span>';

        str += '</div>'
        str += '</div>'
        str += '<div class="col-xs-12 col-sm-2 mg-btm-15-res">' //01
        str += '<div class="package font-13">'
        str += '<p>Ext Bag</p>'
        str += '<span class="pack1" data-toggle="tooltip" data-title ="' + paxbagg + '"  data-original-title="' + paxbagg + '"data-placement="bottom">' + paxbagg == "" ? "-" : paxbagg + '</span>';

        str += '</div>'
        str += '</div>'
        str += '<div class="col-xs-12 col-sm-1 mg-btm-15-res">' //01
        str += '<div class="seat font-13">'
        str += '<p>Seat</p>'
        str += SeatSSR;
        str += '</div>'
        str += '</div>'
        str += '<div class="col-xs-12 col-sm-3 mg-btm-15-res">' //01
        str += '<div class="ossr font-13">'
        str += '<p>Other SSR</p>'

        str += OtherSSR;

        str += '</div>'
        str += '</div>'
        str += '</div>'
        str += '</div>'
        str += '</div>'


        $("#ssrprev").append(str);
    }
    catch (e) {

    }
}

function callpreviewfunction() {

    if ($("#hdn_bookingprev").val() == "Y") {
        $(".previreflight").css("display", "block");
        $(".confirpg").css("display", "none");
        $(".confirpg_pre").css("display", "block");
    }
    else {
        $(".previreflight").css("display", "none");
        $(".confirpg").css("display", "block");
        $(".confirpg_pre").css("display", "none");
    }
}

function calsdur(airprtwat_con_min) {

    var num = parseInt(airprtwat_con_min);
    var hours = (num / 60);
    var rhours = Math.floor(hours);
    var minutes = (hours - rhours) * 60;
    var rminutes = Math.round(minutes);
    var totalwat = rhours + 'h :' + rminutes + 'm';

    return totalwat;
}

function ShowIntBaggage(arg) {
    var strsamp = $("#" + arg + " option:selected").data("connectbaggage");
    strsamp = strsamp.replace("''", "");
    if (strsamp != "" && strsamp != null && strsamp != " ") {
        $("#INTbaggageinfo").html(strsamp);
        $("#modal-baggage").modal('show');
    }
}
function ClearIntBaggage() {
    var DDL = $('.ddlclass');
    $('.ddlclass').selectedIndex = 0
    for (var j = 0; j < DDL.length; j++) {
        $('#Baggage' + j + '').val("0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0");

    }
    $("#modal-baggage").modal("hide");
}

function RemovePaxlist() {
    var flg = false;
    var Rcode = "";
    var c = document.getElementsByName('chkbox')
    var checkedvalue = 0;
    for (var i = 0; i < parseInt(rowss) ; i++) {
        if (c[i].checked === true) {
            checkedvalue++;
            Rcode += $("#lblcheckbox_" + i + "").data('rcode') + ',';
        }
    }
    if (checkedvalue == 0) {
        showerralert("Please select any one of the Passenger.", "", "");
        return false;
    }
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });

    $.ajax({
        type: "POST",
        url: Getpassenger,
        data: '{strMobileNo: "' + Rcode + '" ,strEmailID: "' + $("#txtEmailID").val() + '" ,strPaxType: "' + $("#hdnPaxval").val() + '",strAction: "DELETE"}',
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {
            var json = json["Data"];
            var result = json.Result
            if (result[1] != "") {
                $.unblockUI();
            }
            else {
                adCount = 1;
                chCount = 1;
                inCount = 1;
                lst_Rcode = [];
                showerralert(result[0], "", "");
                $.unblockUI();
            }
        },
        error: function (e) {
            $.unblockUI();
            if (e.status == "500") {
                window.location = sessionExb;
                return false;
            }
        }
    });
}


function Servicemealclick(arg, Flag, Origin, Destination) {
    try {
        var flag = false;
        var strOrigin = "";
        var strDestination = "";
        if (Flag == "I") {
            flag = true;
            strOrigin = Origin;
            strDestination = Destination;
        }
        else if (Flag == "D") {
            flag = false;
            strOrigin = Origin;
            strDestination = Destination;
        }
        else {
            var segment = $("#" + arg).val();
            segment = segment.split("WEbMeaLWEB");
            strOrigin = segment[4];
            strDestination = segment[5];
            var cls = strOrigin + "CLS" + strDestination;

            $('.cls' + strOrigin + 'SEAT' + strDestination).click();
            //var sampprim = $("#" + arg).data("primmeal");
            //var sampmeal = $("#Meals" + sampprim).data("primselectmeal");
            if ($('#' + arg)[0].checked == true) {
                flag = true;
            }
        }

        //
        //var paxref = ptype.split('_')[0].replace("hdnadtMeals", "Adt").replace("hdnchdMeals", "Chd");
        //var paxcnt = "";
        //if (arg.length > 2) {
        //    paxcnt = arg.split('_')[1];
        //}
        //else {
        //    var paxtype = Paxdetailsoth.title;
        //    var paxtypeid = Paxdetailsoth.id;

        //    var paxcount = $('#' + paxtypeid).attr("data-paxcount");
        //    var adtcount = paxcount.split("@")[0];
        //    var chdcount = paxcount.split("@")[1];

        //    var Pax_refno = ((paxtype == "adult") ? index : (parseInt(index) + parseInt(adtcount)));
        //    paxcnt = parseFloat(Pax_refno) + 1;
        //}
        //
        //var paxref = arg.length > 2 ? arg.split('_')[1] : paxno;
        //if ($('#' + arg)[0].checked == true) {
        //    flag = true;
        //    primessr.push({ Itin: segment[1], Segment: segment[2], Origin: segment[4], Destination: segment[5], PaxNo: paxcnt });
        //}
        //else {
        //    var index = primessr.indexOf(segment[1]);
        //    $.each(primessr, function (obj, val) {
        //        if (val.PaxNo == paxcnt && val.Origin == segment[4] && val.Destination == segment[5]) {
        //            primessr.splice(obj, 1);
        //        }
        //    });
        //}
        //if ($('#' + arg)[0].checked == true) {
        //    flag = true;
        //    //
        //    $("." + cls).attr("checked", false);
        //    $("." + cls).attr("disabled", true);
        //    //
        //    if (arg.length > 1) {
        //        $("." + cls + arg.split('_')[1]).attr("checked", false);
        //        $("." + cls + arg.split('_')[1]).attr("disabled", true);
        //    }
        //    else {
        //        $("." + cls + paxno).attr("checked", false);
        //        $("." + cls + paxno).attr("disabled", true);
        //    }
        //}
        //else {
        //    $("." + cls).attr("disabled", false);
        //    //
        //    if (arg.length > 1) {
        //        $("." + cls + arg.split('_')[1]).attr("disabled", false);
        //    }
        //}
        var Param = {
            Flag: flag,
            Origin: strOrigin,
            Destination: strDestination
        }

        $.ajax({
            type: "POST", 		//GET or POST or PUT or DELETE verb
            url: Primservicemeal,// Location of the service data: JSON.stringify(param),
            data: JSON.stringify(Param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (json) {
                var res = json.Result;

                if (Flag != "0") {
                    $(arg).html('');
                    $(arg).append(res);
                }
                else {
                    $('.' + cls).html('');
                    $('.' + cls).append(res);
                }
                //if (arg.length > 2) {
                //    $("#MealName" + paxref + "_" + arg.split('_')[1] + "_" + sampprim).html('');
                //    $("#MealName" + paxref + "_" + arg.split('_')[1] + "_" + sampprim).append(res);
                //}
                //else {
                //    $("#MealName" + paxref + "_" + paxno + "_" + sampprim).html('');
                //    $("#MealName" + paxref + "_" + paxno + "_" + sampprim).append(res);
                //}
            },
            error: function (e) {
                console.log("Internal Problem occured while select PRIM Service meal. (#07).");
            }
        });
    }
    catch (e) {
        console.log(e.message);
    }
}

function Servicemealclear(org, des) {
    var flag = false;
    var Param = {
        Flag: flag,
        Origin: org,
        Destination: des
    }

    $.ajax({
        type: "POST",
        url: Primservicemeal,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {
            var res = json.Result;
            var SegCount = $('#hdnSegCount').val();
            for (var segcnt = 0; segcnt < parseFloat(SegCount) ; segcnt++) {
                $("#Meals" + segcnt).html('');
                $("#Meals" + segcnt).append(res);
            }
        },
        error: function (e) {
            console.log("Internal Problem occured while select PRIM Service meal. (#07).");

        }
    });
};

$(document).on('click', '.clsprime,.clsPrimeFull', function () {
    if ($(this)[0].checked == true) {
        $('#modal-prime').modal({
            backdrop: 'static',
            keyboard: false
        });
    }
});


function GetCreditshellold() {
    var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;
    if ($("#txtCsAirlinePNR").val() == "" && $("#txtCsAirlinePNR").val() == "") {
        showerralert("Please Enter the credit shell PNR.", "", "");
    }
    else {

        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });

        var ClientID = "";
        var BranchID = "";
        var GroupID = "";

        if ($("#ddlclient").length > 0) {
            ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
            BranchID = sessionStorage.getItem('branchid') != null ? sessionStorage.getItem('branchid').trim() : "";
            GroupID = sessionStorage.getItem('Groupid') != null ? sessionStorage.getItem('Groupid').trim() : "";
        }
        else {

            ClientID = "";
            BranchID = "";
            GroupID = "";
        }

        var Param = {
            CsAirlinePNR: $("#txtCsAirlinePNR").val().trim(),
            strClientID: ClientID,
            strBranchID: BranchID,
            strPaxCount: $("#hdnTotalPaxcount").val().trim(),
        }

        $.ajax({
            type: "POST", 		//GET or POST or PUT or DELETE verb
            url: GetCreditShellPNR, 		// Location of the service
            data: JSON.stringify(Param),
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (json) {//On Successful service call
                try {


                    var json = json["Data"];
                    if (json["Statuscode"] == "01") {
                        if (json["Result"] != "") {

                            var jsonResult = JSON.parse(json["Result"]);
                            $("#hdn_CreditshellAmt").val(jsonResult.CSAmount + "|" + $("#txtCsAirlinePNR").val().trim())
                            $(".cls-Creditshellamt").html(parseFloat(jsonResult.CSAmount).toFixed(decimalflag));
                            $(".cls-creditshellpnr").show();

                            var TotalGross = Number(parseFloat($(".riyatcktamt").html()) - parseFloat($("#hdn_servicecharge").val()));
                            var Cspayableamt = Number((parseFloat($(".riyatcktamt").html()) < parseFloat(jsonResult.CSAmount)) ? 0 : ((parseFloat($(".riyatcktamt").html()) > parseFloat(jsonResult.CSAmount)) ? (parseFloat($(".riyatcktamt").html()) - parseFloat(jsonResult.CSAmount)) : 0)).toFixed(decimalflag);

                            $(".cls-payableamt").each(function (i, vale) {
                                $(".cls-payableamt")[i].innerHTML = Number(Cspayableamt).toFixed(decimalflag);
                            });
                            $(".cls-RemainCreditshellamt").each(function (i, vale) {
                                $(".cls-RemainCreditshellamt")[i].innerHTML = Number((parseFloat(Cspayableamt) < parseFloat(parseFloat(jsonResult.CSAmount))) ? (parseFloat(parseFloat(jsonResult.CSAmount)) - parseFloat(TotalGross)) : 0).toFixed(decimalflag);
                            });

                            var ADT_Det = $.grep(jsonResult.Passengers, function (obj) {
                                return obj.PaxType == "ADT";
                            });
                            $.each(ADT_Det, function (obj, val) {
                                $("#ad_Title_" + obj).val(val.Title);
                                $('#ad_Title_' + obj + ' option').filter(function () { return $.trim($(this).val()) == val.Title; }).attr('selected', true);
                                $('#ad_Title_' + (obj)).niceSelect('destroy').niceSelect();
                                $("#ad_firstName_" + obj).val(val.FirstName);
                                $("#ad_lastName_" + obj).val(val.LastName);
                                if (val.Gender.toUpperCase() == "MALE") {
                                    $('#ad_gender_' + obj).val("M");
                                }
                                else if (val.Gender.toUpperCase() == "FEMALE") {
                                    $("#ad_gender_" + obj).val("F");
                                }
                                else {
                                    $("#ad_gender_" + obj).val(val.Gender);
                                }
                                $('#ad_gender_' + (obj)).niceSelect('destroy').niceSelect();
                                $("#ad_DOB_" + obj).val(val.DOB != null ? val.DOB : "");
                                $("#ad_passportNo_" + obj).val(val.PassportNo != null ? val.PassportNo : "");
                                $("#ad_pass_ex_date_" + obj).val(val.PassportExpiry != null ? val.PassportExpiry : "");
                                $("#ad_nationality_" + obj).val(val.Nationality != null ? val.Nationality : "");
                            });
                            var CHD_Det = $.grep(jsonResult.Passengers, function (obj) {
                                return obj.PaxType == "CHD";
                            });
                            $.each(CHD_Det, function (obj, val) {
                                $("#ch_Title_" + obj).val(val.Title);
                                $('#ch_Title_' + obj + ' option').filter(function () { return $.trim($(this).val()) == val.Title; }).attr('selected', true);
                                $('#ch_Title_' + (obj)).niceSelect('destroy').niceSelect();
                                $("#ch_firstName_" + obj).val(val.FirstName);
                                $("#ch_lastName_" + obj).val(val.LastName);
                                if (val.Gender.toUpperCase() == "MALE") {
                                    $("#ch_gender_" + obj).val("M");
                                }
                                else if (val.Gender.toUpperCase() == "FEMALE") {
                                    $("#ch_gender_" + obj).val("F");
                                }
                                else {
                                    $("#ch_gender_" + obj).val(val.Gender);
                                }
                                $('#ch_gender_' + (obj)).niceSelect('destroy').niceSelect();
                                $("#ch_DOB_" + obj).val(val.DOB != null ? val.DOB : "");
                                $("#ch_passportNo_" + obj).val(val.PassportNo != null ? val.PassportNo : "");
                                $("#ch_pass_ex_date_" + obj).val(val.PassportExpiry != null ? val.PassportExpiry : "");
                                $("#ch_nationality_" + obj).val(val.Nationality != null ? val.Nationality : "");
                            });
                            var INF_Det = $.grep(jsonResult.Passengers, function (obj) {
                                return obj.PaxType == "INF";
                            });
                            $.each(INF_Det, function (obj, val) {
                                $("#in_Title_" + obj).val(val.Title);
                                $('#in_Title_' + obj + ' option').filter(function () { return $.trim($(this).val()) == val.Title; }).attr('selected', true);
                                $('#in_Title_' + (obj)).niceSelect('destroy').niceSelect();
                                $("#in_firstName_" + obj).val(val.FirstName);
                                $("#in_lastName_" + obj).val(val.LastName);
                                if (val.Gender.toUpperCase() == "MALE") {
                                    $("#in_gender_" + obj).val("M");
                                }
                                else if (val.Gender.toUpperCase() == "FEMALE") {
                                    $("#in_gender_" + obj).val("F");
                                }
                                else {
                                    $("#in_gender_" + obj).val(val.Gender);
                                }
                                $('#in_gender_' + (obj)).niceSelect('destroy').niceSelect();
                                $("#in_DOB_" + obj).val(val.DOB != null ? val.DOB : "");
                                $("#in_passportNo_" + obj).val(val.PassportNo != null ? val.PassportNo : "");
                                $("#in_pass_ex_date_" + obj).val(val.PassportExpiry != null ? val.PassportExpiry : "");
                                $("#in_nationality_" + obj).val(val.Nationality != null ? val.Nationality : "");
                            });
                        }
                        else {
                            showerralert("unable to retrieve details.please contact support team(#01)", "", "");
                        }
                        $.unblockUI();
                    }
                    else {
                        $("#hdn_CreditshellAmt").val("");
                        $(".cls_Creditshellamt").html("");
                        showerralert(json["Message"], "", "");
                        $.unblockUI();
                    }
                } catch (e) {
                    console.log(e.toString());
                }
            },
            error: function (e) {//On Successful service call                            
                LogDetails(e.responseText, e.status, "InsertServiceAmount");
                showerralert("unable to retrieve details.please contact support team(#07)", "", "");
                $.unblockUI();
                if (e.status == "500") {
                    window.location = sessionExb;
                    return false;
                }
            }
        });
    }

}

var CSPAXDETILS = [];
var CSadCount = 0;
var CSchCount = 0;
var CSinCount = 0;
var CSpaxadCount = 0;
var CSpaxchCount = 0;
var CSpaxinCount = 0;

function GetCreditshell() {

    var decimalflag = ($("#hdn_allowdecimal").val() != null && $("#hdn_allowdecimal").val() != "") ? $("#hdn_allowdecimal").val() == "Y" ? 2 : 0 : 0;
    if ($("#txtCsAirlinePNR").val() == "" && $("#txtCsAirlinePNR").val() == "") {
        showerralert("Please Enter the credit shell PNR.", "", "");
    }
    else {

        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });

        var ClientID = "";
        var BranchID = "";
        var GroupID = "";

        if ($("#ddlclient").length > 0) {
            ClientID = sessionStorage.getItem('clientid') != null ? sessionStorage.getItem('clientid').trim() : "";
            BranchID = sessionStorage.getItem('branchid') != null ? sessionStorage.getItem('branchid').trim() : "";
            GroupID = sessionStorage.getItem('Groupid') != null ? sessionStorage.getItem('Groupid').trim() : "";
        }
        else {

            ClientID = "";
            BranchID = "";
            GroupID = "";
        }

        var Param = {
            CsAirlinePNR: $("#txtCsAirlinePNR").val().trim(),
            strClientID: ClientID,
            strBranchID: BranchID,
            strPaxCount: $("#hdnTotalPaxcount").val().trim(),
        }

        $.ajax({
            type: "POST", 		//GET or POST or PUT or DELETE verb
            url: GetCreditShellPNR, 		// Location of the service
            data: JSON.stringify(Param),
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (json) {//On Successful service call
                try {

                    var json = json["Data"];
                    if (json["Statuscode"] == "01") {
                        if (json["Result"] != "") {
                            $(".Pay-mode").hide();
                            CSadCount = 0;
                            CSchCount = 0;
                            CSinCount = 0;
                            CSpaxadCount = 0;
                            CSpaxchCount = 0;
                            CSpaxinCount = 0;
                            var jsonResult = JSON.parse(json["Result"]);
                            $("#hdn_CreditshellAmt").val(jsonResult.CSAmount + "|" + $("#txtCsAirlinePNR").val().trim())
                            $(".cls-Creditshellamt").html(parseFloat(jsonResult.CSAmount).toFixed(decimalflag));
                            $(".cls-creditshellpnr").show();
                            $("#liEarnings").hide();
                            $(".insurancePanel").hide();
                            $("#bajaj_confirmarion").prop("checked", false);

                            AddBajaj_insAmt();
                            var Insamt = Number($(".riyains")[0].innerHTML).toFixed(0) != 0 ? $(".riyains")[0].innerHTML : "";
                            var Ins_comm = (Insamt != "") ? Insamt : 0;
                            var TotalGross = Number(parseFloat($(".riyatotal").html()) - parseFloat($("#hdn_servicecharge").val()) - parseFloat(Ins_comm));

                            var Cspayableamt = Number((parseFloat($(".riyatotal").html()) < parseFloat(jsonResult.CSAmount)) ? 0 : (parseFloat($(".riyatotal").html()) > parseFloat(jsonResult.CSAmount)) ? (parseFloat($(".riyatotal").html()) - parseFloat(jsonResult.CSAmount)) : 0).toFixed(decimalflag);

                            $(".cls-payableamt").each(function (i, vale) {
                                $(".cls-payableamt")[i].innerHTML = Number(Cspayableamt).toFixed(decimalflag);
                            });
                            $(".cls-RemainCreditshellamt").each(function (i, vale) {
                                $(".cls-RemainCreditshellamt")[i].innerHTML = Number((parseFloat(TotalGross) < parseFloat(parseFloat(jsonResult.CSAmount))) ? (parseFloat(parseFloat(jsonResult.CSAmount)) - parseFloat(TotalGross)) : 0).toFixed(decimalflag);
                            });
                            CSPAXDETILS = jsonResult.Passengers;

                            if (jsonResult.PhoneNumber != null && jsonResult.PhoneNumber != "")
                                $("#txtContactNo").val(jsonResult.PhoneNumber);
                            if (jsonResult.EmailAddress != null && jsonResult.EmailAddress != "")
                                $("#txtEmailID").val(jsonResult.EmailAddress);

                            $("#btn_block").hide();

                            var cspaxdet = "";
                            cspaxdet += '<table class="no-more-tables table table-responsive table-bordered">';
                            cspaxdet += '<thead>';
                            cspaxdet += '<tr>';
                            cspaxdet += '<th>select</th>';
                            cspaxdet += '<th>Pax Type</th>';
                            cspaxdet += '<th>Passenger Name</th>';
                            cspaxdet += '<th>Gender</th>';
                            cspaxdet += '<th>DOB</th>';
                            cspaxdet += '<th>Passport No</th>';
                            cspaxdet += '<th>Passport Exp</th>';
                            cspaxdet += '</tr>';
                            cspaxdet += '</thead>';
                            cspaxdet += '<tbody>';

                            var ADT_Det = $.grep(jsonResult.Passengers, function (obj) {
                                return obj.PaxType == "ADT";
                            });
                            $.each(ADT_Det, function (obj, val) {
                                cspaxdet += '<tr>';
                                cspaxdet += '<th>';
                                cspaxdet += '<span class="_M002">';
                                cspaxdet += '<input style="display: none;" id="chkclsADTCS' + obj + '" data-PaxRef="' + val.PaxRefNumber + '" onclick="getCSPaxCount(this.id,\'ADULT\')" class="cb chkclsADTCS chkallCSPax" type="checkbox" />';
                                cspaxdet += '<label class="cbx" for="chkclsADTCS' + obj + '" style="margin-bottom: 0px !important;"></label>';
                                cspaxdet += '</span>';
                                cspaxdet += '</th>';
                                cspaxdet += '<th>Adult</th>';
                                cspaxdet += '<th>' + val.Title + ' ' + val.FirstName + ' ' + val.LastName + '</th>';
                                cspaxdet += '<th>' + val.Gender + '</th>';
                                var passexp = FormatDate(val.DOB)
                                cspaxdet += '<th>' + (passexp == "" ? "N/A" : passexp) + '</th>';
                                cspaxdet += '<th>' + (val.PassportNo == "" ? "N/A" : val.PassportNo) + '</th>';
                                var passexp = FormatDate(val.PassportExpiry)
                                cspaxdet += '<th>' + (passexp == "" ? "N/A" : passexp) + '</th>';
                                cspaxdet += '</tr>';
                            });
                            var CHD_Det = $.grep(jsonResult.Passengers, function (obj) {
                                return obj.PaxType == "CHD";
                            });
                            $.each(CHD_Det, function (obj, val) {
                                cspaxdet += '<tr>';
                                cspaxdet += '<th>';
                                cspaxdet += '<span class="_M002">';
                                cspaxdet += '<input style="display: none;" id="chkclsCHDCS' + obj + '" data-PaxRef="' + val.PaxRefNumber + '" onclick="getCSPaxCount(this.id,\'CHILD\')" class="cb chkclsCHDCS chkallCSPax" type="checkbox" />';
                                cspaxdet += '<label class="cbx" for="chkclsCHDCS' + obj + '" style="margin-bottom: 0px !important;"></label>';
                                cspaxdet += '</span>';
                                cspaxdet += '</th>';
                                cspaxdet += '<th>Child</th>';
                                cspaxdet += '<th>' + val.Title + ' ' + val.FirstName + ' ' + val.LastName + '</th>';
                                cspaxdet += '<th>' + val.Gender + '</th>';
                                var passexp = FormatDate(val.DOB)
                                cspaxdet += '<th>' + (passexp == "" ? "N/A" : passexp) + '</th>';
                                cspaxdet += '<th>' + (val.PassportNo == "" ? "N/A" : val.PassportNo) + '</th>';
                                var passexp = FormatDate(val.PassportExpiry)
                                cspaxdet += '<th>' + (passexp == "" ? "N/A" : passexp) + '</th>';
                                cspaxdet += '</tr>';
                            });
                            var INF_Det = $.grep(jsonResult.Passengers, function (obj) {
                                return obj.PaxType == "INF";
                            });
                            $.each(INF_Det, function (obj, val) {
                                cspaxdet += '<tr>';
                                cspaxdet += '<th>';
                                cspaxdet += '<span class="_M002">';
                                cspaxdet += '<input style="display: none;" id="chkclsINFCS' + obj + '" data-PaxRef="' + val.PaxRefNumber + '" onclick="getCSPaxCount(this.id,\'INFANT\')" class="cb chkclsINFCS chkallCSPax" type="checkbox" />';
                                cspaxdet += '<label class="cbx" for="chkclsINFCS' + obj + '" style="margin-bottom: 0px !important;"></label>';
                                cspaxdet += '</span>';
                                cspaxdet += '</th>';
                                cspaxdet += '<th>Infant</th>';
                                cspaxdet += '<th>' + val.Title + ' ' + val.FirstName + ' ' + val.LastName + '</th>';
                                cspaxdet += '<th>' + val.Gender + '</th>';
                                var passexp = FormatDate(val.DOB)
                                cspaxdet += '<th>' + (passexp == "" ? "N/A" : passexp) + '</th>';
                                cspaxdet += '<th>' + (val.PassportNo == "" ? "N/A" : val.PassportNo) + '</th>';
                                var passexp = FormatDate(val.PassportExpiry)
                                cspaxdet += '<th>' + (passexp == "" ? "N/A" : passexp) + '</th>';
                                cspaxdet += '</tr>';
                            });
                            //showerralert(json["Message"], "", "");
                            cspaxdet += '</tbody>';
                            cspaxdet += '</table>';
                            $("#dvCSPassengerDetails").html(cspaxdet)
                            $('#popupConfirmMsg').html(json["Message"]);
                            $("#modal_confirm_popup").modal({
                                backdrop: 'static',
                                keyboard: false
                            });

                        }
                        else {
                            showerralert("unable to retrieve details.please contact support team(#01)", "", "");
                        }
                        $.unblockUI();
                    }
                    else {
                        $("#hdn_CreditshellAmt").val("");
                        $(".cls_Creditshellamt").html("");
                        showerralert(json["Message"], "", "");
                        $.unblockUI();
                    }
                } catch (e) {
                    console.log(e.toString());
                }
            },
            error: function (e) {//On Successful service call                            
                LogDetails(e.responseText, e.status, "InsertServiceAmount");
                showerralert("unable to retrieve details.please contact support team(#07)", "", "");
                $.unblockUI();
                if (e.status == "500") {
                    window.location = sessionExb;
                    return false;
                }
            }
        });
    }

}

function ShowHideCSPNR() {
    $('#DvCSPNR').slideToggle();
    if (!$("#showcspnr").is(":checked")) {
        $("#txtCsAirlinePNR").val("");
        $("#hdn_CreditshellAmt").val("");
        $(".cls-creditshellpnr").hide();
        $(".Pay-mode").show();
        $("#liEarnings").show();
        $(".insurancePanel").show();
        $("#hdn_CreditshellAmt").val("");
        if (!$("#bajaj_confirmarion").is(":checked")) {
            $("#bajaj_confirmarion").prop("checked", true);
            AddBajaj_insAmt();
        }
    }
}

$(document).on('click', '#btnCspnr_Clear', function () {
    $("#txtCsAirlinePNR").val("");
    $("#hdn_CreditshellAmt").val("");
    $(".cls-creditshellpnr").hide();
    $(".Pay-mode").show();
    $("#liEarnings").show();
    $(".insurancePanel").show();
    $("#hdn_CreditshellAmt").val("");
    if (!$("#bajaj_confirmarion").is(":checked")) {
        $("#bajaj_confirmarion").prop("checked", true);
        AddBajaj_insAmt();
    }
});

function FormatDate(strDate) {
    if (strDate != null && strDate != undefined && strDate != "") {
        strDate = new Date(strDate);
        var tempday = strDate.getDate() < 10 ? "0" + strDate.getDate() : strDate.getDate();
        var tempmon = strDate.getMonth() + 1;
        tempmon = tempmon < 10 ? "0" + tempmon : tempmon;
        var tempyear = strDate.getFullYear();
        return tempday + "/" + tempmon + "/" + tempyear;
    }
    else {
        return "";
    }
}

function getCSPaxCount(ids, paxtype) {
    var splitpass = $("#hdnTotalPaxcount").val();// document.getElementById("hdnPax").value.toString().split(',');
    var cur_adult_count = Number(splitpass.split('@')[0]);// document.getElementById("hdnPax").toString().split(',');
    var cur_child_count = Number(splitpass.split('@')[1]);// document.getElementById("hd_childCount").value;
    var cur_infant_count = Number(splitpass.split('@')[2]);// document.getElementById("hd_infant").value;

    if ($("#" + ids).is(":checked")) {
        if (paxtype == "ADULT") {
            if (CSadCount < cur_adult_count) {
                CSadCount++;
            }
            else {
                $("#" + ids).prop("checked", false);
            }
        }
        else if (paxtype == "CHILD") {
            if (CSchCount < cur_child_count) {
                CSchCount++;
            }
            else {
                $("#" + ids).prop("checked", false);
            }
        }
        else if (paxtype == "INFANT") {
            if (CSinCount < cur_infant_count) {
                CSinCount++;
            }
            else {
                $("#" + ids).prop("checked", false);
            }
        }
    }
    else {
        if (paxtype == "ADULT") {
            CSadCount--;
        }
        else if (paxtype == "CHILD") {
            CSchCount--;
        }
        else if (paxtype == "INFANT") {
            CSinCount--;
        }
    }
}

$(document).on('click', '#btnContinuetoproceed', function () {
    var chkvalidate = true;
    $('.chkallCSPax').each(function (obj, val) {
        var ids = this.id;
        if ($("#" + ids).is(":checked")) {
            chkvalidate = false;
        }
    });
    var strCSPaxCount = $("#hdnTotalPaxcount").val().trim().split('@');
    if (chkvalidate) {
        //showerralert("Please select atleast one passenger");
        var msg = "";
        msg += "Please select atleast " + Number(strCSPaxCount[0]) + " Adult ";
        if (Number(strCSPaxCount[1]) > 0) {
            msg += "& " + Number(strCSPaxCount[1]) + " Child ";
        }
        if (Number(strCSPaxCount[1]) > 0) {
            msg += "& " + Number(strCSPaxCount[2]) + " Infant ";
        }
        msg += "passenger";
        $("#spncspaxerr").html(msg);
        $("#dvcspaxerr").show();
        setTimeout(function () { $("#dvcspaxerr").hide() }, 5000);
        return false;
    }

    if (CSadCount < Number(strCSPaxCount[0])) {
        var msg = "";
        msg += "Please select atleast " + Number(strCSPaxCount[0]) + " Adult passenger";
        $("#spncspaxerr").html(msg);
        $("#dvcspaxerr").show();
        setTimeout(function () { $("#dvcspaxerr").hide() }, 5000);
        return false;
    }
    if (CSchCount < Number(strCSPaxCount[1])) {
        var msg = "";
        msg += "Please select atleast " + Number(strCSPaxCount[1]) + " Child passenger";
        $("#spncspaxerr").html(msg);
        $("#dvcspaxerr").show();
        setTimeout(function () { $("#dvcspaxerr").hide() }, 5000);
        return false;
    }
    if (CSinCount < Number(strCSPaxCount[2])) {
        var msg = "";
        msg += "Please select atleast " + Number(strCSPaxCount[2]) + " Infant passenger";
        $("#spncspaxerr").html(msg);
        $("#dvcspaxerr").show();
        setTimeout(function () { $("#dvcspaxerr").hide() }, 5000);
        return false;
    }
    $("#modal_confirm_popup").modal("hide");
    $('.chkallCSPax').each(function (obj, val) {
        var ids = this.id;

        if ($("#" + ids).is(":checked")) {
            var paxref = Number($(this).data("paxref"))
            var strCSPaxDetails = $.grep(CSPAXDETILS, function (obj) {
                return obj.PaxRefNumber == paxref;
            });
            if (strCSPaxDetails[0].PaxType == "ADT") {
                $("#ad_Title_" + CSpaxadCount).val(strCSPaxDetails[0].Title);
                $('#ad_Title_' + CSpaxadCount + ' option').filter(function () { return $.trim($(this).val()) == strCSPaxDetails[0].Title; }).attr('selected', true);
                $('#ad_Title_' + (CSpaxadCount)).niceSelect('destroy').niceSelect();
                $("#ad_firstName_" + CSpaxadCount).val(strCSPaxDetails[0].FirstName);
                $("#ad_lastName_" + CSpaxadCount).val(strCSPaxDetails[0].LastName);
                if (strCSPaxDetails[0].Gender.toUpperCase() == "MALE") {
                    $('#ad_gender_' + CSpaxadCount).val("M");
                }
                else if (strCSPaxDetails[0].Gender.toUpperCase() == "FEMALE") {
                    $("#ad_gender_" + CSpaxadCount).val("F");
                }
                else {
                    $("#ad_gender_" + CSpaxadCount).val(strCSPaxDetails[0].Gender);
                }
                $('#ad_gender_' + (CSpaxadCount)).niceSelect('destroy').niceSelect();
                $("#ad_DOB_" + CSpaxadCount).val(FormatDate(strCSPaxDetails[0].DOB));
                $("#ad_passportNo_" + CSpaxadCount).val(strCSPaxDetails[0].PassportNo != null ? strCSPaxDetails[0].PassportNo : "");
                $("#ad_pass_ex_date_" + CSpaxadCount).val(FormatDate(strCSPaxDetails[0].PassportExpiry));
                $("#ad_nationality_" + CSpaxadCount).val(strCSPaxDetails[0].Nationality != null ? strCSPaxDetails[0].Nationality : "");
                CSpaxadCount++;
            }
            else if (strCSPaxDetails[0].PaxType == "CHD") {
                $("#ch_Title_" + CSpaxchCount).val(strCSPaxDetails[0].Title);
                $('#ch_Title_' + CSpaxchCount + ' option').filter(function () { return $.trim($(this).val()) == strCSPaxDetails[0].Title; }).attr('selected', true);
                $('#ch_Title_' + (CSpaxchCount)).niceSelect('destroy').niceSelect();
                $("#ch_firstName_" + CSpaxchCount).val(strCSPaxDetails[0].FirstName);
                $("#ch_lastName_" + CSpaxchCount).val(strCSPaxDetails[0].LastName);
                if (strCSPaxDetails[0].Gender.toUpperCase() == "MALE") {
                    $("#ch_gender_" + CSpaxchCount).val("M");
                }
                else if (strCSPaxDetails[0].Gender.toUpperCase() == "FEMALE") {
                    $("#ch_gender_" + CSpaxchCount).val("F");
                }
                else {
                    $("#ch_gender_" + CSpaxchCount).val(strCSPaxDetails[0].Gender);
                }
                $('#ch_gender_' + (CSpaxchCount)).niceSelect('destroy').niceSelect();
                $("#ch_DOB_" + CSpaxchCount).val(FormatDate(strCSPaxDetails[0].DOB));
                $("#ch_passportNo_" + CSpaxchCount).val(strCSPaxDetails[0].PassportNo != null ? strCSPaxDetails[0].PassportNo : "");
                $("#ch_pass_ex_date_" + CSpaxchCount).val(FormatDate(strCSPaxDetails[0].PassportExpiry));
                $("#ch_nationality_" + CSpaxchCount).val(strCSPaxDetails[0].Nationality != null ? strCSPaxDetails[0].Nationality : "");
                CSpaxchCount++;

            }
            else if (strCSPaxDetails[0].PaxType == "INF") {
                $("#in_Title_" + CSpaxinCount).val(strCSPaxDetails[0].Title);
                $('#in_Title_' + CSpaxinCount + ' option').filter(function () { return $.trim($(this).val()) == strCSPaxDetails[0].Title; }).attr('selected', true);
                $('#in_Title_' + (CSpaxinCount)).niceSelect('destroy').niceSelect();
                $("#in_firstName_" + CSpaxinCount).val(strCSPaxDetails[0].FirstName);
                $("#in_lastName_" + CSpaxinCount).val(strCSPaxDetails[0].LastName);
                if (strCSPaxDetails[0].Gender.toUpperCase() == "MALE") {
                    $("#in_gender_" + CSpaxinCount).val("M");
                }
                else if (strCSPaxDetails[0].Gender.toUpperCase() == "FEMALE") {
                    $("#in_gender_" + CSpaxinCount).val("F");
                }
                else {
                    $("#in_gender_" + CSpaxinCount).val(strCSPaxDetails[0].Gender);
                }
                $('#in_gender_' + (CSpaxinCount)).niceSelect('destroy').niceSelect();
                $("#in_DOB_" + CSpaxinCount).val(FormatDate(strCSPaxDetails[0].DOB));
                $("#in_passportNo_" + CSpaxinCount).val(strCSPaxDetails[0].PassportNo != null ? strCSPaxDetails[0].PassportNo : "");
                $("#in_pass_ex_date_" + CSpaxinCount).val(FormatDate(strCSPaxDetails[0].PassportExpiry));
                $("#in_nationality_" + CSpaxinCount).val(strCSPaxDetails[0].Nationality != null ? strCSPaxDetails[0].Nationality : "");
                CSpaxinCount++;

            }
        }
    })
});


//for passenger additional information details by STS-195
function bindAddlpaxinfo(strCount, strType) {
    var strPaxAddlInfo = "";
    for (var count = 1; count <= Number(strCount) ; count++) {
        var paxNAme = "";
        if (strType == "ADT" && ($("#ad_Title_" + (count - 1)).val() != "" && $("#ad_firstName_" + (count - 1)).val() != "" && $("#ad_lastName_" + (count - 1)).val() != "")) {
            paxNAme = $("#ad_Title_" + (count - 1)).val() + ". " + $("#ad_firstName_" + (count - 1)).val() + " " + $("#ad_lastName_" + (count - 1)).val();
        }
        else if (strType == "CHD" && ($("#ch_Title_" + (count - 1)).val() != "" && $("#ch_firstName_" + (count - 1)).val() != "" && $("#ch_lastName_" + (count - 1)).val() != "")) {
            paxNAme = $("#ch_Title_" + (count - 1)).val() + ". " + $("#ch_firstName_" + (count - 1)).val() + " " + $("#ch_lastName_" + (count - 1)).val();
        }
        else {
            paxNAme = (strType == "ADT" ? "Adult" : "Child") + ' ' + count
        }
        strPaxAddlInfo += '<div class="col-xs-12 col-sm-12 txt-l form-group" style="margin-bottom:20px;">';
        strPaxAddlInfo += '<span class="label" style="font-size: 13px;color:black;font-weight:600;padding:0px;">' + paxNAme + '</span>';
        strPaxAddlInfo += '</div> ';

        strPaxAddlInfo += '<div class="col-xs-12 col-sm-3 form-group" style="margin-bottom:25px;">';
        strPaxAddlInfo += '<div class="dv-anim input-effect m-0 m-sm-t15">';
        strPaxAddlInfo += '<input class="txt-anim hideErr" id="txtaddlinfo_Email_' + (strType) + '_' + count + '" type="text" maxlength="75">';
        strPaxAddlInfo += '<label>Email Id</label';
        strPaxAddlInfo += '<span class="focus-border">';
        strPaxAddlInfo += '<i></i>';
        strPaxAddlInfo += '</span>';
        strPaxAddlInfo += '</div> ';
        strPaxAddlInfo += '</div>';

        strPaxAddlInfo += '<div class="col-xs-12 col-sm-3 form-group" style="margin-bottom:25px;">';
        strPaxAddlInfo += '<div class="dv-anim input-effect m-0 m-sm-t15">';
        strPaxAddlInfo += '<input class="txt-anim hideErr clsNumeric" id="txtaddlinfo_Contact_' + (strType) + '_' + count + '" type="text"  maxlength="10">';
        strPaxAddlInfo += '<label>Contact No</label';
        strPaxAddlInfo += '<span class="focus-border">';
        strPaxAddlInfo += '<i></i>';
        strPaxAddlInfo += '</span>';
        strPaxAddlInfo += '</div> ';
        strPaxAddlInfo += '</div>';

        strPaxAddlInfo += '<div class="col-xs-12 col-sm-3 form-group">';
        strPaxAddlInfo += '<div class="dv-anim input-effect m-0 m-sm-t15" style="">';
        strPaxAddlInfo += '<input class="txt-anim hideErr " id="txtaddlinfo_Reason_' + (strType) + '_' + count + '" type="text" maxlength="75">';
        strPaxAddlInfo += '<label>Reason for travel</label';
        strPaxAddlInfo += '<span class="focus-border">';
        strPaxAddlInfo += '<i></i>';
        strPaxAddlInfo += '</span>';
        strPaxAddlInfo += '</div>';
        strPaxAddlInfo += '</div>';

        strPaxAddlInfo += '<div class="col-xs-12 col-sm-3 form-group" style="margin-bottom:25px;">';
        strPaxAddlInfo += '<div class="dv-anim input-effect m-0 m-sm-t15">';
        strPaxAddlInfo += '<span class="clsspntitle">Search State</span>';
        strPaxAddlInfo += '<select class="txt-anim hideErr form-control ddlstate" id="ddladdlinfo_state_' + (strType) + '_' + count + '">';
        strPaxAddlInfo += '<option value="0">--Select State--</option>';
        strPaxAddlInfo += '</select>';
        strPaxAddlInfo += '<span class="focus-border">';
        strPaxAddlInfo += '<i></i>';
        strPaxAddlInfo += '</span>';
        strPaxAddlInfo += '</div>';
        strPaxAddlInfo += '</div>';

        strPaxAddlInfo += '<div class="col-xs-12 col-sm-3 form-group" style="margin-bottom:10px;">';
        strPaxAddlInfo += '<div class="dv-anim input-effect m-0 m-sm-t15" style="">';
        strPaxAddlInfo += '<input class="txt-anim hideErr clsAlpha" id="ddladdlinfo_city_' + (strType) + '_' + count + '" type="text" maxlength="50">';
        strPaxAddlInfo += '<label>City</label';
        strPaxAddlInfo += '<span class="focus-border">';
        strPaxAddlInfo += '<i></i>';
        strPaxAddlInfo += '</span>';
        strPaxAddlInfo += '</div>';
        strPaxAddlInfo += '</div>';

        strPaxAddlInfo += '<div class="col-xs-12 col-sm-3 form-group" style="margin-bottom:10px;">';
        strPaxAddlInfo += '<div class="dv-anim input-effect m-0 m-sm-t15" style="">';
        strPaxAddlInfo += '<input class="txt-anim hideErr clsNumeric w-100" id="txtaddlinfo_pincode_' + (strType) + '_' + count + '" maxlength="6">';
        strPaxAddlInfo += '<label>Pincode</label';
        strPaxAddlInfo += '<span class="focus-border">';
        strPaxAddlInfo += '<i></i>';
        strPaxAddlInfo += '</span>';
        strPaxAddlInfo += '</div>';
        strPaxAddlInfo += '</div>';

        strPaxAddlInfo += '<div class="col-xs-12 col-sm-3 form-group" style="margin-bottom:10px;">';
        strPaxAddlInfo += '<div class="dv-anim input-effect m-0 m-sm-t15" style="">';
        strPaxAddlInfo += '<input class="txt-anim hideErr w-100" id="txtaddlinfo_Address_' + (strType) + '_' + count + '" maxlength="75">';
        strPaxAddlInfo += '<label>Address</label';
        strPaxAddlInfo += '<span class="focus-border">';
        strPaxAddlInfo += '<i></i>';
        strPaxAddlInfo += '</span>';
        strPaxAddlInfo += '</div>';
        strPaxAddlInfo += '</div>';

        strPaxAddlInfo += '<div class="col-xs-12 col-sm-3 form-group" style="margin-bottom:15px;">';
        strPaxAddlInfo += '<span class="flt-lft w-100" style="font-size: 13px;font-weight: 600;color: red;">Is Covid-19 test done</span>';
        strPaxAddlInfo += '<div class="radio Pay-mode flt-lft flt-lftr" style="visibility: visible;margin-right:20px;">';
        strPaxAddlInfo += '<input id="rdnaddlinfo_yes_' + (strType) + '_' + count + '" class="" name="rdnaddlinfo_covid_' + (strType) + '_' + count + '" type="radio" value="YES">';
        strPaxAddlInfo += '<label for="rdnaddlinfo_yes_' + (strType) + '_' + count + '" class="radio-label RDT" title="Yes" style="font-size:13px;">Yes</label>';
        strPaxAddlInfo += '</div>';
        strPaxAddlInfo += '<div class="radio Pay-mode flt-lft flt-lftr" style="visibility: visible;">';
        strPaxAddlInfo += '<input id="rdnaddlinfo_no_' + (strType) + '_' + count + '" class="" name="rdnaddlinfo_covid_' + (strType) + '_' + count + '" type="radio" value="NO">';
        strPaxAddlInfo += '<label for="rdnaddlinfo_no_' + (strType) + '_' + count + '" class="radio-label RDT" title="No" style="font-size:13px;">No</label>';
        strPaxAddlInfo += '</div>';
        strPaxAddlInfo += '</div>';
    }
    return strPaxAddlInfo;
}

function ValidateAddlInfo_Pax() {
    var strPaxAddlBookingDet = "";
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    try {
        $("#hdn_PaxAddlInfoData").val("");
        var paxcount = $('#hdnTotalPaxcount').val();
        var adtcount = paxcount.split("@")[0];
        var chdcount = paxcount.split("@")[1];
        for (var i = 0; i < adtcount; i++) {
            if ($("#txtaddlinfo_Email_ADT_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please enter Adult " + (i + 1) + " email address.");
                $("#txtaddlinfo_Email_ADT_" + (i + 1) + "").focus();
                return false;
            }
            if (filter.test($("#txtaddlinfo_Email_ADT_" + (i + 1) + "").val()) == false) {
                showAddlPaxError("Please enter valid Adult " + (i + 1) + " email address.");
                $("#txtaddlinfo_Email_ADT_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#txtaddlinfo_Contact_ADT_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please enter Adult " + (i + 1) + " contact no.");
                $("#txtaddlinfo_Contact_ADT_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#txtaddlinfo_Contact_ADT_" + (i + 1) + "").val().length != 10) {
                showAddlPaxError("Please enter 10 digit valid contact no for Adult " + (i + 1) + "");
                $("#txtaddlinfo_Contact_ADT_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#txtaddlinfo_Reason_ADT_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please enter Adult " + (i + 1) + " Reason for travel.");
                $("#txtaddlinfo_Reason_ADT_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#ddladdlinfo_state_ADT_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please select Adult " + (i + 1) + " state");
                $("#ddladdlinfo_state_ADT_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#ddladdlinfo_city_ADT_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please enter Adult " + (i + 1) + " city.");
                $("#ddladdlinfo_city_ADT_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#txtaddlinfo_pincode_ADT_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please enter Adult " + (i + 1) + " pincode.");
                $("#txtaddlinfo_pincode_ADT_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#txtaddlinfo_pincode_ADT_" + (i + 1) + "").val().length != 6) {
                showAddlPaxError("Please enter valid 6 digit pincode for Adult " + (i + 1) + "");
                $("#txtaddlinfo_pincode_ADT_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#txtaddlinfo_Address_ADT_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please enter Adult " + (i + 1) + " address.");
                $("#txtaddlinfo_Address_ADT_" + (i + 1) + "").focus();
                return false;
            }
            if ($("input[name=rdnaddlinfo_covid_ADT_" + (i + 1) + "]:checked").val() == null ||
                $("input[name=rdnaddlinfo_covid_ADT_" + (i + 1) + "]:checked").val() == undefined ||
                $("input[name=rdnaddlinfo_covid_ADT_" + (i + 1) + "]:checked").val() == "") {
                showAddlPaxError("Please select Adult " + (i + 1) + " covid test.");
                $("#rdnaddlinfo_covid_ADT_" + (i + 1) + "").focus();
                return false;
            }

            strPaxAddlBookingDet += "ADULT" + "SPLITSCRIPT" + $("#txtaddlinfo_Email_ADT_" + (i + 1) + "").val() + "SPLITSCRIPT"
                        + $("#txtaddlinfo_Contact_ADT_" + (i + 1) + "").val() + "SPLITSCRIPT" + $("#txtaddlinfo_Reason_ADT_" + (i + 1) + "").val() + "SPLITSCRIPT"
                        + $("#ddladdlinfo_state_ADT_" + (i + 1) + "").val() + "SPLITSCRIPT" + $("#ddladdlinfo_city_ADT_" + (i + 1) + "").val() + "SPLITSCRIPT"
                        + $("#txtaddlinfo_pincode_ADT_" + (i + 1) + "").val() + "SPLITSCRIPT" + $("#txtaddlinfo_Address_ADT_" + (i + 1) + "").val() + "SPLITSCRIPT"
                        + $("input[name=rdnaddlinfo_covid_ADT_" + (i + 1) + "]").val() + "SPLITSCRIPTPAX";
        }
        for (var i = 0; i < chdcount; i++) {
            if ($("#txtaddlinfo_Email_CHD_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please enter Child " + (i + 1) + " email address.");
                $("#txtaddlinfo_Email_CHD_" + (i + 1) + "").focus();
                return false;
            }
            if (filter.test($("#txtaddlinfo_Email_CHD_" + (i + 1) + "").val()) == false) {
                showAddlPaxError("Please enter valid Child " + (i + 1) + " email address.");
                $("#txtaddlinfo_Email_CHD_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#txtaddlinfo_Contact_CHD_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please enter Child " + (i + 1) + " contact no.");
                $("#txtaddlinfo_Contact_CHD_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#txtaddlinfo_Contact_CHD_" + (i + 1) + "").val().length != 10) {
                showAddlPaxError("Please enter 10 digit valid contact no for Child " + (i + 1) + "");
                $("#txtaddlinfo_Contact_CHD_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#txtaddlinfo_Reason_CHD_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please enter Child " + (i + 1) + " Reason for travel.");
                $("#txtaddlinfo_Reason_CHD_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#ddladdlinfo_state_CHD_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please select Child " + (i + 1) + " state");
                $("#ddladdlinfo_state_CHD_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#ddladdlinfo_city_CHD_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please select Child " + (i + 1) + " city.");
                $("#ddladdlinfo_city_CHD_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#txtaddlinfo_pincode_CHD_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please enter Child " + (i + 1) + " pincode.");
                $("#txtaddlinfo_pincode_CHD_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#txtaddlinfo_pincode_CHD_" + (i + 1) + "").val().length != 6) {
                showAddlPaxError("Please enter valid 6 digit pincode for Child " + (i + 1) + "");
                $("#txtaddlinfo_pincode_CHD_" + (i + 1) + "").focus();
                return false;
            }
            if ($("#txtaddlinfo_Address_CHD_" + (i + 1) + "").val() == "") {
                showAddlPaxError("Please enter Child " + (i + 1) + " address.");
                $("#txtaddlinfo_Address_CHD_" + (i + 1) + "").focus();
                return false;
            }
            if ($("input[name=rdnaddlinfo_covid_CHD_" + (i + 1) + "]:checked").val() == null ||
                $("input[name=rdnaddlinfo_covid_CHD_" + (i + 1) + "]:checked").val() == undefined ||
                $("input[name=rdnaddlinfo_covid_CHD_" + (i + 1) + "]:checked").val() == "") {
                showAddlPaxError("Please select Child " + (i + 1) + " covid test.");
                $("#rdnaddlinfo_covid_CHD_" + (i + 1) + "").focus();
                return false;
            }

            strPaxAddlBookingDet += "CHILD" + "SPLITSCRIPT" + $("#txtaddlinfo_Email_CHD_" + (i + 1) + "").val() + "SPLITSCRIPT"
                        + $("#txtaddlinfo_Contact_CHD_" + (i + 1) + "").val() + "SPLITSCRIPT" + $("#txtaddlinfo_Reason_CHD_" + (i + 1) + "").val() + "SPLITSCRIPT"
                        + $("#ddladdlinfo_state_CHD_" + (i + 1) + "").val() + "SPLITSCRIPT" + $("#ddladdlinfo_city_CHD_" + (i + 1) + "").val() + "SPLITSCRIPT"
                        + $("#txtaddlinfo_pincode_CHD_" + (i + 1) + "").val() + "SPLITSCRIPT" + $("#txtaddlinfo_Address_CHD_" + (i + 1) + "").val() + "SPLITSCRIPT"
                        + $("input[name=rdnaddlinfo_covid_CHD_" + (i + 1) + "]:checked").val() + "SPLITSCRIPTPAX";
        }
    }
    catch (e) {
        console.log(e);
        strPaxAddlBookingDet = "";
        showAddlPaxError("unable to validate passenger additional details. please contact support team.(#09)");
    }
    $("#hdn_PaxAddlInfoData").val(strPaxAddlBookingDet);
    $("#modal-Pax-Addl-details").modal('hide');
}

function PaxaddlinfoPopup(ID) {
    var paxcount = $('#' + ID).data("paxcount");
    var popupid = $('#' + ID).data("popupid");
    var adtcount = paxcount.split("@")[0];
    var chdcount = paxcount.split("@")[1];
    var SegCount = $('#hdnSegCount').val();
    var Totpaxcount = parseFloat(adtcount) + parseFloat(chdcount);

    Ids_split = "";
    paxtypcnt = 0;
    var nameReg = /[ ](?=[ ])_-|[^A-Za-z0-9 ]+/gi;
    $("#passengersdetails .commAdtpaxcls").each(function () {
        paxtypcnt++;
        Ids_split = this.id.split("_")[1];

        pax_type_det = $("#ad_type_" + Ids_split).data("paxvalue").split(':')[0];

        paty = $("#ad_type_" + Ids_split).data("paxvalue").split(':')[0];
        if ($("#ad_Title_" + Ids_split).val() == null || $("#ad_Title_" + Ids_split).val() == "0") {
            showerralert("Please Enter Adult" + paxtypcnt + " Title.", "", "");
            $("#ad_Title_" + Ids_split).focus();
            checkflag = true;
            return false;
        }
        if ($("#ad_firstName_" + Ids_split).val() == null || $("#ad_firstName_" + Ids_split).val().trim() == "") {
            showerralert("Please Enter Adult" + paxtypcnt + " First name.", "", "");
            $("#ad_firstName_" + Ids_split).val("");
            $("#ad_firstName_" + Ids_split).focus();
            checkflag = true;
            return false;
        }
        if (nameReg.test($("#ad_firstName_" + Ids_split).val())) {
            showerralert("Please Enter Valid Adult" + paxtypcnt + " First name.", "", "");
            $("#ad_firstName_" + Ids_split).val("");
            $("#ad_firstName_" + Ids_split).focus();
            checkflag = true;
            return false;
        }
        if (($("#hdn_lastnamemand").val() == "" || $("#hdn_lastnamemand").val() == "1") && ($("#ad_lastName_" + Ids_split).val() == null || $("#ad_lastName_" + Ids_split).val().trim() == "")) {
            showerralert("Please Enter Adult" + paxtypcnt + " Last Name.", "", "");
            $("#ad_lastName_" + Ids_split).val("");
            $("#ad_lastName_" + Ids_split).focus();
            checkflag = true;
            return false;
        }
        if (($("#hdn_lastnamemand").val() == "" || $("#hdn_lastnamemand").val() == "1") && nameReg.test($("#ad_lastName_" + Ids_split).val())) {
            showerralert("Please Enter Valid Adult" + paxtypcnt + " Last name.", "", "");
            $("#ad_lastName_" + Ids_split).val("");
            $("#ad_lastName_" + Ids_split).focus();
            checkflag = true;
            return false;
        }
        checkflag = false;
    });
    paxtypcnt = 0;
    $("#passengersdetails .commChdpaxcls").each(function () {
        if (checkflag == true) {
            return false;
        }
        checkflag = false;
        paxtypcnt++;
        Ids_split = this.id.split("_")[1];

        pax_type_det = $("#ch_type_" + Ids_split).data("paxvalue").split(':')[0];

        paty = $("#ch_type_" + Ids_split).data("paxvalue").split(':')[0];
        if ($("#ch_Title_" + Ids_split).val() == null || $("#ch_Title_" + Ids_split).val() == "0") {
            showerralert("Please Enter Child" + paxtypcnt + " Title.", "", "");
            $("#ch_Title_" + Ids_split).focus();
            checkflag = true;
            return false;
        }
        if ($("#ch_firstName_" + Ids_split).val() == null || $("#ch_firstName_" + Ids_split).val().trim() == "") {
            showerralert("Please Enter Child" + paxtypcnt + " First name.", "", "");
            $("#ch_firstName_" + Ids_split).val("");
            $("#ch_firstName_" + Ids_split).focus();
            checkflag = true;
            return false;
        }
        if (nameReg.test($("#ch_firstName_" + Ids_split).val())) {
            showerralert("Please Enter Valid Child" + paxtypcnt + " First name.", "", "");
            $("#ch_firstName_" + Ids_split).val("");
            $("#ch_firstName_" + Ids_split).focus();
            checkflag = true;
            return false;
        }
        if (($("#hdn_lastnamemand").val() == "" || $("#hdn_lastnamemand").val() == "1") && ($("#ch_lastName_" + Ids_split).val() == null || $("#ch_lastName_" + Ids_split).val().trim() == "")) {
            showerralert("Please Enter Child" + paxtypcnt + " Last Name.", "", "");
            $("#ch_lastName_" + Ids_split).val("");
            $("#ch_lastName_" + Ids_split).focus();
            checkflag = true;
            return false;
        }
        if (($("#hdn_lastnamemand").val() == "" || $("#hdn_lastnamemand").val() == "1") && nameReg.test($("#ch_lastName_" + Ids_split).val())) {
            showerralert("Please Enter Valid Child" + paxtypcnt + " Last name.", "", "");
            $("#ch_lastName_" + Ids_split).val("");
            $("#ch_lastName_" + Ids_split).focus();
            checkflag = true;
            return false;
        }
        checkflag = false;
    });
    if (checkflag == true) {
        return false;
    }

    var strPaxaddlinfo_details_dv = bindAddlpaxinfo(adtcount, "ADT");
    strPaxaddlinfo_details_dv += bindAddlpaxinfo(chdcount, "CHD");
    $("#dvPaxAddldetails").html(strPaxaddlinfo_details_dv);

    PageLoadState();

    $(popupid).modal({
        backdrop: 'static',
        keyboard: false
    });
}

function showAddlPaxError(Msg) {
    $("#spnError").html(Msg);
    $("#spnError").show();
    setTimeout(function () {
        $("#spnError").hide();
        $("#spnError").html("");
    }, 5000);
}

var stateid = "";
function PageLoadState() {
    $(".ddlstate option").remove();
    $(".ddlstate").append('<option value="">Select State</option>');
    $.ajax({
        url: StateXML,
        dataType: "xml",
        success: function (xmlResponse) {
            var jsonResponse = $.xml2json(xmlResponse);
            jsonResponse = jsonResponse["STATE"];
            var filteredary = $.grep(jsonResponse, function (n, j) {
                return n.COUNTRY_ID == "IN";
            });
            $(filteredary).each(function (n, i) {
                $(".ddlstate").append('<option value="' + i["STATE_ID"] + '">' + i["STATE_NAME"] + '</option>');
            });
            //  $("#ddlstate").chosen();
        },
        error: function (result) {
            showerralert('Unable to connect remote server.', '', '');
            return false;
        }
    });
}

$(document).on('input', '.clsAlpha', function () {
    var txtQty = $(this).val().replace(/[^a-zA-Z\ ]/g, '');
    txtQty = txtQty.replace(/  +/g, ' ');
    $(this).val(txtQty);
});
$(document).on('input', '.clsAlphaNumeric', function () {
    var txtQty = $(this).val().replace(/[^a-zA-Z0-9]/g, '');
    $(this).val(txtQty);
});
$(document).on('input', '.clsNumeric', function () {
    var txtQty = $(this).val().replace(/[^0-9]/g, '');
    $(this).val(txtQty);
});
$(document).on('input', '#dvPaxAddldetails .txt-anim', function () {
    if ($(this).val() != "") {
        $(this).addClass('has-content');
    }
    else {
        $(this).removeClass('has-content');
    }
});
//for passenger additional information details by STS-195

function GetEmployeeDetails(ids) {
    try {

        if ($('#txtcash_EmpCode').val().length < 6) {
            return false;
        }
        var param = {
            strEmployeeCode: $('#txtcash_EmpCode').val(),
        }
        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' },
            baseZ: 10000000
        });
        $.ajax({
            type: "POST", 		//GET or POST or PUT or DELETE verb
            url: GetEmployeeDetURL, 		// Location of the service                    
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (json) {//On Successful service call
                var Response = json;
                if (Response.Status == "01") {
                    var data = JSON.parse(Response.Result);
                    var html = '';
                    html += '<table class="table table-striped text-left">';
                    html += '<thead>';
                    html += '<tr>';
                    html += '<th><b>Employee ID</b></th>';
                    html += '<th><b>Name</b></th>';
                    html += '<th><b>Email</b></th>';
                    //html += '<th><b>Mobile No</b></th>';
                    html += '<th><b>Branch</b></th>';
                    html += '</tr>';
                    html += '</thead>';
                    html += '<tbody>';
                    html += '<tr>';
                    html += '<td>' + data["EmpCode"] + '</td>';
                    html += '<td>' + data["Name"] + '</td>';
                    html += '<td>' + data["Email"] + '</td>';
                    //html += '<td>' + data["MobileNo"] + '</td>';                   
                    html += '<td>' + data["Branch"] + '</td>';
                    html += '</tr>';
                    html += '</tbody>';
                    html += '</table>';
                    $("#dvEmployee").html(html);
                    $("#dvEmployee,#dvEmpDetails").show();
                    $("#hdncash_Empname").val(data["Name"]);
                    $("#hdncash_Empemail").val(data["Email"]);
                    $("#hdncash_Empmobile").val(data["MobileNo"]);
                    $("#hdncash_Empbranch").val(data["Branch"]);
                }
                else {
                    var Message = (Response.Message != null && Response.Message != "") ? Response.Message : "Unable to get employee details.please contact support team";
                    var html = '<span style="color:#fff;background: #ff0000;font-size: 14px;text-align:center;padding: 10px;">' + Message + '</span>';
                    $("#dvEmployee").html(html);
                    $("#dvEmployee,#dvEmpDetails").show();
                }
                $.unblockUI();
                return false;
            },
            error: function (e) {// When Service call fails      
                var html = '<span style="color:#fff;background: #ff0000;font-size: 14px;text-align:center;padding: 10px;">Unable to get employee details.please contact support team(#07)</span>';
                $("#dvEmployee").html(html);
                $("#dvEmployee,#dvEmpDetails").show();
                $.unblockUI();
                return false;
            }

        });
    }
    catch (e) {
        var html = '<span style="color:#fff;background: #ff0000;font-size: 14px;text-align:center;padding: 10px;">Unable to get employee details.please contact support team(#09)</span>';
        $("#dvEmployee").html(html);
        $("#dvEmployee,#dvEmpDetails").show();
        $.unblockUI();
        return false;
    }
}

$(document).on('input', '#txtcash_EmpCode', function () {
    $("#dvEmployee,#dvEmpDetails").hide();
    $("#hdncash_Empname").val("");
    $("#hdncash_Empemail").val("");
    $("#hdncash_Empmobile").val("");
    $("#hdncash_Empbranch").val("");
    GetEmployeeDetails();
});

function ChangeFareForSelect(strarticleid, strGrossFare) {
    $("#spnChosenPopupFare").html(strGrossFare);
    $("body").data("selectedfareid_select", strarticleid);
}

function PopupRedirectToSelect(ids, grid) {
    var strarticleids = $("body").data("selectedfareid_select")
    if (grid != null && grid != undefined && grid != "" && grid == "Dgrd") {
        var arg = $("#" + ids).data("arg");
        bindSletedRndTrpAvail(strarticleids, arg)
    }
    else {
        GetRowIndexSelect(strarticleids);
    }
}