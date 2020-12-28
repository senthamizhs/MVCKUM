var aryRowCnt = [];
var XMLurl = "";
var JsonMData = "";
var jsonresults = "";
var productMNames = new Array();
var productIds = new Object();
$(document).ready(function () {
    XMLurl = $('#dvDataProp').data('airportxml-url');
    $('#ul-multi-city-routes').data('li-temp', $('#ul-multi-city-routes li:first-child').html()); //To Assign Fists Row from Multicity Search to Data field for "Add New Flight" purpose...

    var today = vb_currentdate; // new Date.today().toString("dd/MM/yyyy");
    var maxdt = vb_maxdate; // new Date.today().addMonths(6).addDays(1).toString("dd/MM/yyyy");
    $("#txtMDeparturdtFlight1,#txtMDeparturdtFlight2").datepicker({
        numberOfMonths: 1,
        showButtonPanel: false,
        dateFormat: "dd/mm/yy",
        minDate: today,
        maxDate: maxdt
    });
    $("#txtMDeparturdtFlight1,#txtMDeparturdtFlight2").on("change keyup paste", function (e) {
        SetNextMinDate(this);
    });

    /*Load Multicity Airpoart City*/

    productMNames = new Array();
});

function multiloading() {

    productMNames = new Array();
    JsonMData = loadGlobalcityArrry;

    var citynames = "";
    $.each(JsonMData, function (index, AIR) {
        if (AIR.AN.indexOf('-') != -1) {
            citynames = AIR.AN.split('-')[0].trim();
        } else { citynames = AIR.AN.trim(); }
        if ($("#hdn_producttype").val() == "RIYA") {
            if ($("#domastic")[0].checked == true) {
                if (AIR.CN != "undefined" && AIR.CN.length > 0 && AIR.CN == assignedcountry) {
                    productMNames.push(citynames + "-(" + AIR.ID + ")" + "~" + AIR.CN + "~" + AIR.AN); //if comes without ~ symbol normal typeahead ll work...
                }
            }
            else {
                if (AIR.CN != "undefined" && AIR.CN.length > 0) {
                    productMNames.push(citynames + "-(" + AIR.ID + ")" + "~" + AIR.CN + "~" + AIR.AN); //if comes without ~ symbol normal typeahead ll work...
                }
            }
        }
        else {
            if (AIR.CN != "undefined" && AIR.CN.length > 0) {
                productMNames.push(citynames + "-(" + AIR.ID + ")" + "~" + AIR.CN + "~" + AIR.AN); //if comes without ~ symbol normal typeahead ll work...
            }
        }
        //productIds[AIR.AN] = AIR.ID;
    });
    $('#txtMOrigin1, #txtMOrigin2').typeahead('destroy');
    $('#txtMOrigin1, #txtMOrigin2').typeahead({
        source: productMNames,
        items: 8,
        scrollBar: true
    });
    $('#txtMDestinations1, #txtMDestinations2').typeahead('destroy');
    $('#txtMDestinations1, #txtMDestinations2').typeahead({
        source: productMNames,
        items: 8,
        scrollBar: true
    });

   
}


function LoadNewMulCalandCity(cnt) {
    
    var rowcount = $('#dvAddNewMRow').data('mul-rowcount');
    rowcount = parseInt(rowcount);

    var today = $('#ul-multi-city-routes li:nth-last-child(2) .cslMulCall').val();
    if (today == "")
        today = vb_currentdate;// new Date.today().addDays(1).toString("dd/MM/yyyy");
    var maxdt = vb_maxdate; //new Date.today().addMonths(6).addDays(1).toString("dd/MM/yyyy");
    $("#txtMDeparturdtFlight" + cnt).datepicker({
        numberOfMonths: 1,
        showButtonPanel: false,
        dateFormat: "dd/mm/yy",
        minDate: today,
        maxDate: maxdt
    });
    $("#txtMDeparturdtFlight" + cnt).on("change keyup paste", function (e) {
        SetNextMinDate(this);
        hideError1("", "");
    });
    $('#txtMOrigin' + cnt, '#txtMDestinations' + cnt).on("change keypress paste", function (e) {
        hideError1("", "");
    });

    $('#txtMOrigin' + cnt).typeahead({
        source: productMNames,
        items: 8,
        scrollBar: true
    });
    $('#txtMDestinations' + cnt).typeahead({
        source: productMNames,
        items: 8,
        scrollBar: true
    });

   

    var SelectedTo = $('#ul-multi-city-routes li:nth-child(' + (rowcount - 1) + ') .clsMflight-to').val();
    $("#txtMOrigin" + cnt).val(SelectedTo);
}
////var maxRowCnt = 5; //Max Row Count for Dynamic Control Create , if increase this more rows will create...
function AddNewMRow(arg) {//To Add New Row(Segment) for Multicity Search
    var rowcount = $('#dvAddNewMRow').data('mul-rowcount');
    rowcount = parseInt(rowcount);
    if (Number(rowcount + 1) > 3 && $("#hdn_BookletThread").val() != null && $("#hdn_BookletThread").val() != undefined && $("#hdn_BookletThread").val() != "") {
        $('.clsBookletFare').show();
    }
    if (Number(rowcount + 1) > 5 && $("#hdn_BookletThread").val() != null && $("#hdn_BookletThread").val() != undefined && $("#hdn_BookletThread").val() != "") {
        $("#Mulrd_Booklet").prop("checked", true);
        $("#Mulrd_Booklet").prop("disabled", true);
        $("#Mulrd_Booklet").trigger('change');
    }
    if (rowcount < maxRowCnt) {
        var rowIndex = "";
        if (aryRowCnt.length > 0) {
            rowIndex = aryRowCnt[0];
            aryRowCnt.shift(); //To Remove first element from an array. ( pop() used to remove last element from an array)...
        }
        else rowIndex = rowcount + 1;

        var getLi = $('#ul-multi-city-routes').data('li-temp')
            .replace('txtMDeparturdtFlight1', 'txtMDeparturdtFlight' + rowIndex)
            .replace('txtMOrigin1', 'txtMOrigin' + rowIndex)
            .replace('txtMDestinations1', 'txtMDestinations' + rowIndex)
            .replace('Flightno1', 'Flightno' + rowIndex)
            .replace('Via1', 'Via' + rowIndex)
            .replace('btnCloseMRow1', 'btnCloseMRow' + rowIndex);//$('#ul-multi-city-routes li:first-child').html();

        $('#ul-multi-city-routes').append('<li class="clsliMSearchSec">' + getLi + '</li>');
        $('#dvAddNewMRow').data('mul-rowcount', (rowcount + 1))

        $('#ul-multi-city-routes li:nth-child(' + (rowcount + 1) + ') .clsCloseMRow').attr('disabled', false);
    }
    if ((rowcount + 1) == maxRowCnt) {
        var btns = $('#dvAddNewMRow').html();
        $('#dvAddNewMRow').data('mrow-btn', btns)
        $('#btn-add-multi-city-route').remove();
    }
    LoadNewMulCalandCity(rowIndex);
}

function CloseMRow(tag) {//To Remove any Row (Segment) from Multicity Search
    var rowcount = $('#dvAddNewMRow').data('mul-rowcount');
    rowcount = parseInt(rowcount);
    $(tag).closest('li.clsliMSearchSec').remove();
    $('#dvAddNewMRow').data('mul-rowcount', (rowcount - 1));
    aryRowCnt.push(tag.id.replace('btnCloseMRow', ''));
    if (Number(rowcount - 1) <= 3 && $("#hdn_BookletThread").val() != null && $("#hdn_BookletThread").val() != undefined && $("#hdn_BookletThread").val() != "") {
        $('.clsBookletFare').hide();
        $("#Mulrd_Booklet").prop("checked", false);
        $("#Mulrd_Booklet").trigger('change');
    }
    if (Number(rowcount - 1) <= 5 && $("#hdn_BookletThread").val() != null && $("#hdn_BookletThread").val() != undefined && $("#hdn_BookletThread").val() != "") {
        $("#Mulrd_Booklet").prop("disabled", false);
    }
    if ((rowcount - 1) < maxRowCnt) {
        if ($('#dvAddNewMRow').data('mrow-btn') != "")
            $('#dvAddNewMRow').html($('#dvAddNewMRow').data('mrow-btn'));
    }
}

function FocusDeptDate(tag) { //Change Next segment From city Based on Current segment To City...
    hideError1("", "");
    var ids = tag.id;
    var cnt = ids.replace('txtMDestinations', '');

    var index = $(tag).closest('li.clsliMSearchSec').index();
    $('#ul-multi-city-routes li:nth-child(' + (index + 2) + ') .clsMflight-from').val($(tag).val());

    if ($('#txtMDeparturdtFlight' + (cnt)).length)
        $('#txtMDeparturdtFlight' + (cnt)).focus();
}

function FocusDestination(ids) { //Load To City Based on From City...
    hideError1("", "");
    var cnt = ids.replace('txtMOrigin', '');
    $('#txtMDestinations' + cnt).focus();
}

function SetNextMinDate(tag) { //Change Min Date Based on Previous Segment Selected Date...
    
    var tagindex = $(tag).closest('li.clsliMSearchSec').index();
    var rowcount = $('#dvAddNewMRow').data('mul-rowcount');
    rowcount = parseInt(rowcount);


    hideoldavaildetails();

    var today = $(tag).val();
    var maxdt = vb_maxdate; // new Date.today().addMonths(6).addDays(1).toString("dd/MM/yyyy");

    for (var index = tagindex + 2; index <= rowcount; index++) { //tagindex starts from 0, child count starts from 1 (so next child is index+2)...
        $('#ul-multi-city-routes li:nth-child(' + (index) + ') .cslMulCall').datepicker('destroy');
        $('#ul-multi-city-routes li:nth-child(' + (index) + ') .cslMulCall').datepicker({
            numberOfMonths: 1,
            showButtonPanel: false,
            dateFormat: "dd/mm/yy",
            minDate: today,
            maxDate: maxdt
        });
        $('#ul-multi-city-routes li:nth-child(' + (index) + ') .cslMulCall').on("change keyup paste", function (e) {
            SetNextMinDate(this);
        });

        $('#ul-multi-city-routes li:nth-child(' + (index) + ') .cslMulCall').val(today); //Index starts from 0, child count starts from 1 (so next child is index+2)...
    }
   
    $('#ul-multi-city-routes li:nth-child(' + (tagindex + 2) + ') .clsMflight-from').focus(); //Index starts from 0, child count starts from 1 (so next child is index+2)...
}

//For Adult Child Infant change function... Can change for other application... by saranraj on 20170524...
function MAdultLoad() {
    hideoldavaildetails();
    var getAdult = document.getElementById('cmbMAdultFlight');
    var getChild = document.getElementById('cmbMChildFlight');
    var getInfant = document.getElementById('cmbMInfantFlight');
    if (getChild.length != 0)
        getChild.length = 0;
    if (getInfant.length != 0)
        getInfant.length = 0;


    for (var i = 0; i <= getAdult.selectedIndex; i++) {
        if (i <= 4) {
            var infantOption = new Option();
            if (i == 0) {
                infantOption.value = "0";
                infantOption.text ="Infant";
            }
            else {
                infantOption.value = i;
                infantOption.text = i;
            }
            getInfant[i] = infantOption;
        }
    }


    for (var i = 0; i <= (9 - getAdult.selectedIndex) ; i++) {
        var childOption = new Option();
        if (i == 0) {
            childOption.value = "0";
            childOption.text = "Child";
        }
        else {
            childOption.value = i;
            childOption.text = i;
        }
        getChild[i] = childOption;
    }

    document.getElementById('cmbMChildFlight').selectedIndex = 0;
    document.getElementById('cmbMInfantFlight').selectedIndex = 0;
    $('#hdtxt_adultcount').val($('#cmbMAdultFlight').val());
    $('#hdtxt_childcount').val($('#cmbMChildFlight').val());
    $('#hdtxt_infantcount').val($('#cmbMInfantFlight').val());

    if ($("#hdnAppTheme").val() != "THEME3" && $("#hdnAppTheme").val() != "THEME4") {
        $('#cmbMAdultFlight').niceSelect('destroy').niceSelect();
        $('#cmbMChildFlight').niceSelect('destroy').niceSelect();
        $('#cmbMInfantFlight').niceSelect('destroy').niceSelect();
    }

    if ($("#hdnAppTheme").val() == "THEME3" || $("#hdnAppTheme").val() == "THEME4") {
        TravellersMulticityclass();
    }

}

$("#cmbMChildFlight,#cmbMInfantFlight").change(function () {
    $("#hdtxt_childcount").val($('#cmbMChildFlight').val());
    $("#hdtxt_infantcount").val($('#cmbMInfantFlight').val());
    hideoldavaildetails();
    TravellersMulticityclass();
});


function TravellersMulticityclass() {
    var ddlAdult = $('#cmbMAdultFlight').val() != null && $('#cmbMAdultFlight').val() != "" ? $('#cmbMAdultFlight').val() : "0";
    var ddlChild = $('#cmbMChildFlight').val() != null && $('#cmbMChildFlight').val() != "" ? $('#cmbMChildFlight').val() : "0";
    var ddlInfant = $('#cmbMInfantFlight').val() != null && $('#cmbMInfantFlight').val() != "" ? $('#cmbMInfantFlight').val() : "0";
    var TotalCount = parseInt(ddlAdult) + parseInt(ddlChild) + parseInt(ddlInfant);
    var Cabin = $("input[name='flight-class']:checked")["0"].dataset.value;// this.labels["0"].innerHTML;
    $('.flight-Mulclass').each(function () {
        if (this.checked == true) {
            $("#grpcmbFlightClass")[0].value = this.value;
            Cabin = $("input[name='flight-Mulclass']:checked")["0"].dataset.value; //this.labels["0"].innerHTML;
            return Cabin;
        }
    });
    $('#flightTravellersClass1').val(TotalCount + ' Traveller(s) - ' + Cabin);
}

//End...

//******Multicity Requests Start*******//
var mulcnt = 0;
var aryOrgMul = [];
var aryDstMul = [];
var aryDptMul = [];
var aryViafltNum = "";
var finalfltvia = "";
function MulFlightSearch() {
    assignedcurrency = $('#ddlMNationality').val();//STS-166
    $('.clscurrency').html(assignedcurrency);
    $("#HDN_CURRENCY_code").val(assignedcurrency);

    dataproperty_array = [];//Added by srinath on 20171006 to improve peroformance
    extrahidem_dataproperty_array = [];//Added by srinath on 20171006 to improve peroformance
    groparr = [];
    groupmain_array = [];
    groupmain_ret = [];
    filterdarray = [];
    Main_totalfilterdarray = [];
    var obj1 = {
        count: 0,
        setcount: 0
    };
    if ($("#hdnAppTheme").val() == "THEME3") {
        $("#grpcmbFlightClass")[0].value = $("input[name='flight-Mulclass']:checked")["0"].dataset.value;
    }
    sessionreq = 0;
  
    $(".flight-listavail").html(""); //For all mode of avail (Multicity)...
    $('.headericons').hide();
    $('#seleMulAvailstikbtm').html('');
    $('#dvStickyFltSlectMultiAvail').hide();

    $("#dvAvailView").hide();
    $('#spnTotalFAvailCntParent').hide();
    document.getElementById("chkNFare").checked = false;
    document.getElementById("chkCheckall").checked = false; 
   
    try {

        alertFlag = "0";
        var adtCntMul = $('#cmbMAdultFlight').val() != null && $('#cmbMAdultFlight').val() != "" ? $('#cmbMAdultFlight').val() : 1;
        var chdCntMul = $('#cmbMChildFlight').val() != null && $('#cmbMChildFlight').val() != "" ? $('#cmbMChildFlight').val() : 0; //$('#cmbMChildFlight').val();
        var infCntMul = $('#cmbMInfantFlight').val() != null && $('#cmbMInfantFlight').val() != "" ? $('#cmbMInfantFlight').val() : 0; //$('#cmbMInfantFlight').val();

        $("#hdtxt_adultcount").val(adtCntMul);
        $("#hdtxt_childcount").val(chdCntMul);
        $("#hdtxt_infantcount").val(infCntMul);

        aryOrgMul = [];
        aryDstMul = [];
        aryDptMul = [];

        var aryMorigin = [];
        var aryMdestination = [];

        var val_flag = true;
        $('.clsliMSearchSec').each(function () {
            $e1 = $(this);
            $aaa = $e1.find('.clsMflight-from');
            if ($($aaa).val() == "" || $($aaa).val() == null) {
                showError1('Please select Origin city.');
                $($aaa).focus();
                val_flag = false;
                return false;
            }
            else {
                var SplitOrgMul = $($aaa).val().split("(");

                $fltno = $e1.find('.flightnumopt');
                $via = $e1.find('.viafltopt');
                var flightnoval = $($fltno).val()
                var viaval = $($via).val()

                aryViafltNum = flightnoval + "," + viaval + "|";
                finalfltvia += aryViafltNum;

                if (SplitOrgMul.length > 1) {
                    aryOrgMul.push(SplitOrgMul[1].split(")")[0]);
                    aryMorigin.push($($aaa).val());
                }
                else {
                    showError1('Please select valid Origin city.');
                    $($aaa).focus();
                    val_flag = false;
                    return false;
                }
            }
            $bbb = $e1.find('.clsMflight-to');
            if ($($bbb).val() == "" || $($bbb).val() == null) {
                showError1('Please select Destination city.');
                $($bbb).focus();
                val_flag = false;
                return false;
            }
            else {
                var SplitDesMul = $($bbb).val().split("(");
                if (SplitDesMul.length > 1) {
                    aryDstMul.push(SplitDesMul[1].split(")")[0]);
                    aryMdestination.push($($bbb).val());
                }
                else {
                    showError1('Please select valid Destination city.');
                    $($aaa).focus();
                    val_flag = false;
                    return false;
                }
            }

            if ($($aaa).val() == $($bbb).val()) {
                showError1('Please select different origin and destination.');
                val_flag = false;
                return false;
            }
            $ccc = $e1.find('.cslMulCall');
            if ($($ccc).val() == "" || $($ccc).val() == null) {
                showError1('Please select Departure date.');
                $($ccc).focus();
                val_flag = false;
                return false;
            }
            else {
                aryDptMul.push($($ccc).val());
            }
        });

        if ($("#cmbMAdultFlight").val() == null || $("#cmbMAdultFlight").val() == "" || $("#cmbMAdultFlight").val() == "0") {
            msg = "Please select no.of adult.";
            showError1(msg);
            $("#cmbMAdultFlight").focus();
            val_flag = false;
            return false;
        }
        if (TerminalType == "T") {
            if ($("#ddlMulClient").length > 0) {
                if (sessionStorage.getItem("clientid") == "" || sessionStorage.getItem("clientid") == 'undefined' || sessionStorage.getItem("clientid") == null || $('#ddlMulClient').val() == "" || $('#ddlMulClient').val() == "0") {
                    msg = "Please Select Agency Name.";
                    showError1(msg);
                    $("#ddlMulClient").focus();
                    val_flag = false;
                    return false;
                }
            }
            if ($('#ddlMulTerminalId').length > 0) {
                if ($('#ddlMulTerminalId').val() == "") {
                    showError1("Please Select Terminal ID");
                    return false;
                }
                sessionStorage.setItem("clientid", $('#ddlMulTerminalId').val());
            }
        }

        if ($("#hdn_BookletThread").val() != null && $("#hdn_BookletThread").val() != undefined && $("#hdn_BookletThread").val() != ""
             && $("#Mulrd_Booklet").is(":checked") == true) {
            for (var _bkl = 0; _bkl < aryOrgMul.length; _bkl++) {
                var new_arr = $.grep(loadGlobalcityArrry, function (n, i) {
                    return n.ID == aryOrgMul[_bkl];
                });
                var new_arr_des = $.grep(loadGlobalcityArrry, function (n, i) {
                    return n.ID == aryDstMul[_bkl];
                });
                if (new_arr[0].CN != assignedcountry || new_arr_des[0].CN != assignedcountry) {
                    showError1("Booklet fare is only allowed for domestic sectors");
                    val_flag = false;
                    return false;
                }
            }
        }
        if (val_flag == true) {
            var segmenttype = "";
            //Set Segment Type is international or Domestic...by saranraj on 20170522...
            if (segList.indexOf('MD') != -1) { //To check Domestic Multicity Rights enabled or not...
                for (var ii = 0; ii < aryOrgMul.length; ii++) {
                    var new_arr = $.grep(loadGlobalcityArrry, function (n, i) {
                        return n.ID == aryOrgMul[ii];
                    });
                    var new_arr_des = $.grep(loadGlobalcityArrry, function (n, i) {
                        return n.ID == aryDstMul[ii];
                    });
                    if (TerminalType.toString().toUpperCase() == "C") {
                        segmenttype = "D";
                        sessionStorage.setItem('seglensr', aryOrgMul.length);
                    }
                    else {
                        segmenttype = new_arr[0].CN == assignedcountry && new_arr_des[0].CN == assignedcountry ? "D" : "I";
                    }
                    
                    if (segmenttype == "I")
                        break;
                }
            }
            else {
                segmenttype = "I";
            }

            $('body').data('segtype', segmenttype);
            //End...

            var colmd = aryOrgMul.length == "2" ? "col-md-6" : aryOrgMul.length == "3" ? "col-md-4" : aryOrgMul.length == "4" ? "col-md-3" : "col-md-2"; //For Selected avail at sticky bottom...

            var Mul_s = "", mul_select=""; 
            Mul_s += '<div id="MulAvailCarousel" class="owl-carousel owl-theme mb-10" style="background-color: rgb(255, 255, 255);">';
            for (var i = 1; i <= aryOrgMul.length; i++) {

                Mul_s += '<section class="add-section">';
                Mul_s += '<span class="clsfltAvailCnt">Showing <span id="spnActlFAvailCnt_M_' + i + '" class="zeroflt">0</span> of <span id="spnTotalFAvailCnt_M_' + i + '" class="zeroflt">0</span> Flights found. </span>'; //Flight Count Showing per Segment... by saranraj...

                Mul_s += '<ul class="sort-bar clearfix block-sm onewayhdrsort" style="margin-bottom: 0px;">';
                Mul_s += '<li class="sort-by-name clssorting fst clssorting_M_' + i + '" data-sortorder="asc" onclick=' + '"' + "javascript: SortingFAvail('airline_" + i + "',this)" + '"' + '><span>Airline <i></i></span></li>';
                Mul_s += '<li class="sort-by-rating clssorting clssorting_M_' + i + '" data-sortorder="asc" onclick=' + '"' + "javascript: SortingFAvail('depart_" + i + "',this)" + '"' + '><span>Departure <i></i></span></li>';
                Mul_s += '<li class="sort-by-rating clssorting clssorting_M_' + i + '" data-sortorder="asc" onclick=' + '"' + "javascript: SortingFAvail('duration_" + i + "',this)" + '"' + '><span>Duration <i></i></span></li>';
                Mul_s += '<li class="sort-by-rating clssorting clssorting_M_' + i + '" data-sortorder="asc" onclick=' + '"' + "javascript: SortingFAvail('arrivsort_" + i + "',this)" + '"' + '><span>Arrival <i></i></span></li>';
                Mul_s += '<li class="sort-by-price clssorting clssorting_M_' + i + '" id="sortFAvail_price_dynamic_M_' + i + '" data-sortorder="asc" onclick=' + '"' + "javascript: SortingFAvail('price_" + i + "',this)" + '"' + '><span class="float-left pad-lft-5">Fare <i class="fa fa-long-arrow-down"></i></span></li>';
                Mul_s += '</ul>';

                Mul_s += '<div class="sort-by-title block-sm clsonewayallcheck hidden-xs" style="float: right;">';
                Mul_s += '<div class="mk-trc" data-style="check" data-text="true" style="float: right">';
                Mul_s += '<input id="chkCheckall_dynamic_M_' + i + '" type="checkbox" name="radio1" class="checkbox_availchkAll clsDynamicChkAll">';
                Mul_s += '<label for="chkCheckall_dynamic_M_' + i + '"><i class="checkbox_avail checkbox_availchk1"></i></label>';
                Mul_s += '</div>';
                Mul_s += '</div>';

                Mul_s += '<div class="item listing-style3 flight clsroundAvail col-xs-12" id="availset_M_' + i + '">';
                 //Do stuff...
                Mul_s +='</div>';
                Mul_s += '</section>';

                mul_select = '';
                mul_select += '<div class="' + colmd + ' col-xs-12 col5 pt-5 pb-4" id="slctMAvail' + i + '">';
                mul_select += '</div>';
                $('#seleMulAvailstikbtm').append(mul_select);

                $('#selectclickbuttonMCity').data('dep-id_' + i, ''); //To clear Data Property for Modify search...
                $('#spnMGrandTotFare').data('fare-o' + i, '');
            }
            Mul_s += '</div>';

            $('#dvmultiCtyAvail').html(Mul_s);

            var Mul_sel_flt = '';
            Mul_sel_flt += '<div id="MulSelectCarousel" class="owl-carousel owl-theme">';
            for (var i = 1; i <= aryOrgMul.length; i++) {
                Mul_sel_flt += '<section class="add-section col0">';
                Mul_sel_flt += '<div class="item" id="availset_M_' + i + '">';
                //Do stuff...
                Mul_sel_flt += '</div>';
                Mul_sel_flt += '</section>';
            }
            Mul_sel_flt += '</div>';

            Mul_sel_flt += '';

            var owl = $('#MulAvailCarousel');
            owl.owlCarousel({
                //stagePadding: 50,
                margin: 10,
                nav: true,
                loop: false,
                autoplay: false,
                responsive: {
                    0: {
                        items: 1
                    },
                    600: {
                        items: 2
                    },
                    992: {
                        items: 2
                    }
                }
            });

            if ($("#hdn_AppHosting").val() != null && $("#hdn_AppHosting").val() == "BSA" &&
                (sessionStorage.getItem('refreshPageFlg') == null || sessionStorage.getItem('refreshPageFlg') == "1") &&
                $("#hdtxt_trip").val() == "M" && $("#hdn_sessAgentLogin").val().toUpperCase() != "Y" && TerminalType != "M") {
                var aryFltselectedval = [];
                aryFltselectedval.push(segmenttype);//0 airport type
                aryFltselectedval.push($("#hdtxt_trip").val());//1 trip type
                aryFltselectedval.push(aryMorigin.length);//2 multicity count
                aryFltselectedval.push(JSON.stringify(aryMorigin));//3 origin
                aryFltselectedval.push(JSON.stringify(aryMdestination));//4 destiantion
                aryFltselectedval.push(JSON.stringify(aryDptMul));//5 depature
                aryFltselectedval.push(adtCntMul);//6 adults
                aryFltselectedval.push(chdCntMul);//7 childs
                aryFltselectedval.push(infCntMul);//8 infants
                if ($("#MulflightClassEconomic").is(":checked"))
                    aryFltselectedval.push("E");
                else if ($("#MulflightClassBusiness").is(":checked"))
                    aryFltselectedval.push("B");
                else if ($("#MulflightClassFirstClass").is(":checked"))
                    aryFltselectedval.push("F");
                else if ($("#MulflightClassPremiumEconomic").is(":checked"))
                    aryFltselectedval.push("P");
                aryFltselectedval.push($("#Mulfli_option").val());//10 LCC/FSC filter
                var AirlineCodes = $("#MulFlightName").val();
                AirlineCodes = AirlineCodes != null ? AirlineCodes : [];
                aryFltselectedval.push(AirlineCodes.join(','));//11 Airline filter
                if ($("#Mulrd_Special_fare").is(":checked") == true) {
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
        }
    }
    catch (e) {
        showError1(e.message);
        return false;
    }

    //});
}
//******Multicity Requests End*******//


//******Direct Select  Start******//
function loadMulselectedflight(results, id) {
    $('#dvMulStickbottom').show();
    var imgpath = $('#dvDataProp').data('airline-url');

    var stringloadingselectedflights = "";

    //if (results[0].Stock == "RST_1") {
    var arystock = results[0].Stock.split('_');
    if (arystock.length > 1) {
        var AirLogo = imgpath + results[0].CarrierCode + ".png";
        $("#dvinnerMulSelectedFlt_" + arystock[1].trim()).html("");

        var colsmsize = aryOrgMul.length == 2 ? "col-sm-4" : aryOrgMul.length == 3 ? "col-sm-4" : aryOrgMul.length == 4 ? "col-sm-6" : "col-sm-12";
        var colsmsize1 = aryOrgMul.length == 2 ? "col-sm-4" : aryOrgMul.length == 3 ? "col-sm-4" : aryOrgMul.length == 4 ? "col-sm-12" : "col-sm-12";


        stringloadingselectedflights += "<div class='row row10 selectedfltOuter' data-content='" + id + "'>";
        stringloadingselectedflights += "<div style='padding: 10px 0px;' class='" + colsmsize + " col-xs-12 col10'><img style='max-width:35px;' id='imgRflight_0' src='" + AirLogo + "' alt=''><h6>" + results[0].FlightNumber + " / " + results[0].CNX + "</h6></div>"
        stringloadingselectedflights += "<div class='" + colsmsize + " col-xs-12 col10' style='margin-top:5px; white-space:nowrap; border-left: 1px dotted; border-right: 1px dotted;'><h6><img class='fi_icon TakeOffRPlane' src='" + ImageUrl + "/" + $("#hdn_producttype").val() + "/flight_icon_2.png' alt=''>" + results[0].Origin + " - " + results[results.length - 1].Destination + "</h6>"
        var minutestohour = (Number(results[0].FlyingTime) / 60);
        var finalminutes = (Number(results[0].FlyingTime) % 60);
        var finalhours = minutestohour.toString().split('.')[0];

        stringloadingselectedflights += "<h6 style='white-space: nowrap;'>" + results[0].DepartureTime + " - " + results[results.length - 1].ArrivalTime + "</h6><h6 class='hidden-xs'> <span style='font-size: 11px;'>(" + finalhours + "hrs " + finalminutes + "mins )</span></h6></div>"
        stringloadingselectedflights += "<div class='" + colsmsize1 + " col-xs-12 col10'style='margin: 10px 0px;'><h4 style='text-align:center;border: 1px solid #b74d06;border-radius: 8px;box-shadow: 0px 0px 0px 5px #ccc;'>" + Discountfare(results[0].GrossAmount) + "</h4>"
        stringloadingselectedflights += "</div>"

        stringloadingselectedflights += "</div>"

        $("#dvinnerMulSelectedFlt_" + arystock[1].trim()).append(stringloadingselectedflights);
    }
    // }
}
//******Direct Select End******//

function showHideMulAvail()  //Show Hide Selected Availablily in sticky bottom for Mobile screen...
{
    $('#dvMulselectedFlights').slideToggle();
}

$(document).on('change', "#Mulrd_Booklet", function () {
    $('#MulFlightName').val('');
    if ($(this).is(":checked")) {
        $('#MulFlightName').prop('disabled', true);
    }
    else {
        $('#MulFlightName').prop('disabled', false);
    }
});
