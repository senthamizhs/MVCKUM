
function ValidateAlphaNumeric(e) {
    if (/Firefox[\/\s](\d+\.\d+)/.test(navigator.userAgent)) {
        var code;
        var code1;
        if (!e) var e = window.event
        if (e.which) code = e.which;
        if ((code < 65 || code > 90) && (code < 97 || code > 122) && (code < 48 || code > 57) && (code != 13) && (code != 9) && (code != 8) && (code != 219) && (code != 220))
            return false;
        else
            if (e.keyCode) code = e.keyCode;
        if (code == 46) return true;
        return true;
    }
    else {
        var code;
        if (!e) var e = window.event
        if (e.keyCode) code = e.keyCode;
        if ((code < 65 || code > 90) && (code < 97 || code > 122) && (code < 48 || code > 57) && (code != 13) && (code != 9) && (code != 8))
            return false;
        else
            return true;
    }
}

function validateAlphandNumeric(event) { // It Allows Only Alpha Numeric Values Only..
    try {
        var keyCode = event.keyCode == 0 ? event.charCode : event.keyCode;
        var ret = ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (keyCode >= 48 && keyCode <= 57) || (specialKeys.indexOf(event.keyCode) != -1 && event.charCode != event.keyCode && (keyCode == 32)));
        return ret;
    } catch (Error) {
    }
}

function ValidateSpecialChar(event) {
    var regex = new RegExp("^[a-zA-Z0-9\b]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    var keyCode = (event.which) ? event.which : event.keyCode
    if (!regex.test(key) && keyCode != 9 && keyCode != 46 && keyCode != 32) {
        event.preventDefault();
        return false;
    }
    else { return true; }
}

function Queuechange() {
    if ($("#ddl_QueueStock").val() == "1G") {
        $(".load-instruction").show();
        $(".load-queue-no").show();
    }
    else if ($("#ddl_QueueStock").val() == "1A") {
        $(".load-instruction").hide();
        $(".load-queue-no").hide();
    }
    else {
        $(".load-instruction").hide();
        $(".load-queue-no").hide();
    }
    BindOfficeID();
    clearretrievepnr();
}

$(document).on("input", ".clscomdetails", function () {

    var txtQty = $(this).val().replace(/[^0-9\.]/g, '');
    $(this).val(txtQty);

    if (this.id.indexOf('Comm') != -1 || this.id.indexOf('plb') != -1) {
        var chklength = $(this).attr('maxlength');
        if (chklength == "4") {
            var value = $(this).val();
            if ((value !== '') && (value.indexOf('.') === -1)) {
                $(this).val(Math.max(Math.min(value, 100), 0));
            }
        }
        if (this.id.indexOf('Comm') != -1) {
            GetCommission(this.id);
        }
        else if (this.id.indexOf('plb') != -1) {
            GetPLB(this.id);
        }
    }
});

$(".input-effect input").focusout(function () {
    if ($(this).val().trim() != "") {
        $(this).addClass("has-content");
    } else {
        $(this).removeClass("has-content");
    }
});

function retriveaccounting() {
    if ($("#retriveaccounting")[0].checked == true) {
        $("#btn_getfare span").html("Accounting");
    } else {
        $("#btn_getfare span").html("Check Fare");
    }
}
var RetrieveIsGST = false;
var SpecialFlag = "";
var strtaxbreakupjson = [];
function RetrievePnr() {
    try {
        VarX = "0";
        RetrieveIsGST = false;
        SpecialFlag = "";
        $(".corporate_codecls").show();

        $("#ddl_QueueDebitCostCenter").html("");
        $("#guestempradiodv").hide();
        $("#ddl_QueueRefid").html("");
        if ($('#ddl_QueueDebitCostCenter').length) {
            $('#ddl_QueueDebitCostCenter').chosen('destroy');
        }
        if ($('#ddl_QueueRefid').length) {
            $('#ddl_QueueRefid').chosen('destroy');
        }
        $('#dvPaxCount').html("");
        $("#ddl_platingcarrier").val("").trigger("chosen:updated");
        if (ValidationQueue()) {
            var RAT = true;
            var AirlinePNR = "";
            var AirlineName = "";
            var AirlineCategory = "";
            var CRSPNR = "";
            var CRSTYPE = "";
            if ($("#ddl_QueueEmpId").length) {
                $("#ddl_QueueEmpId").val("");
            }

            if ($(".slttxtfrereason").length) {
                $(".slttxtfrereason").val("");
            }

            $(".udk-faretype-show").hide();
            $(".udk-totalfare-show").hide();

            if ($("#rdoaircat_FSC")[0].checked) {
                AirlineCategory = "FSC";
                CRSPNR = $("#txt_CRSpnr_Queue").val().trim();
                CRSTYPE = $("#ddl_QueueStock").val().trim();
                RAT = false;
            }
            else {
                RAT = true;
                AirlineCategory = "LCC";
                CRSPNR = $("#txt_Airlinepnr_Queue").val();
                CRSTYPE = $("#ddl_AirlineStock").val().split('|')[0];
                AirlineCategory = $("#ddl_AirlineStock").val().split('|')[1];
                AirlinePNR = $("#txt_Airlinepnr_Queue").val();
                AirlineName = $("#ddl_AirlineStock").val().split('|')[0];
            }

            if (CRSTYPE == "1A") {
                $("#txtgstStatecode").hide();
                $("#txtgstCitycode").hide();
                $("#txtgstpinecode").hide();
                $("#thstate").hide();
                $("#thcity").hide();
                $("#thpin").hide();
            }
            else {
                $("#txtgstStatecode").show();
                $("#txtgstCitycode").show();
                $("#txtgstpinecode").show();
                $("#thstate").show();
                $("#thcity").show();
                $("#thpin").show();
            }

            var Queuenumber = $("#txt_CRSpnr_QueueNO").val();
            var AirportType = "";//  $("#ddl_QueueAT").val().trim();
            var Corporatename = $("#ddlclient").val();
            var Employeename = $("body").data("employeeid") != null ? $("body").data("employeeid") : "";//$("#ddl_QueueEmpId").val()!=null?$("#ddl_QueueEmpId").val().trim():"";
            var EmpCostCenter = $("#ddl_QueueDebitCostCenter").val() != null ? $("#ddl_QueueDebitCostCenter").val().trim() : "";
            var EmpRefID = $("#ddl_QueueRefid").val() != null ? $("#ddl_QueueRefid").val().trim() : "";
            //loadCorporateDetails(Corporatename);
            var office_id = $("#ddlOfficeIdRetrieve").length ? $("#ddlOfficeIdRetrieve").val() : "";
            var AgentID = $('#hdnClientID').val();
            var BranchID = $('#hdnBranchID').val();

            if (AgentID == "" || BranchID == "") {
                showerralert("Please select valid Agency name.", "", "");
                $('#ddlclient').focus();
                return false;
            }

            //FetchgstdetailsTDK(AgentID, "");
            clearfaredetails();
            $(".clscheckfarehide").show();
            $("#hdf_pnrinfo").val("");
            $("#hdf_tstcount").val("0");
            $("#hdf_farechanged_session").val("");
            $("#hdn_crspnr").val(CRSPNR);
            $("#hdn_crs").val(CRSTYPE);
            $("#hdn_queuenum").val(Queuenumber);
            $("#uenquentFlyer").html("");
            $("#sp_pnr_details").html("");
            $("#loadGstDetails").hide();
            $("#dv_row1").hide();
            //$("#btn_getfare").hide();
            $("#dvFrequentFlyer").hide();
            // $("#lbl_corcode").hide();
            // $("#txt_corporateCode").length ? $("#txt_corporateCode").hide() : "";
            $(".wholeclassdiv").hide();
            $("#dvEarnings").html("");
            $("#dvCommdetails").html("");
            $(".clsgstclr").val("");
            $("#ddlgststate").val("ALL");
            $("#allow_gst").attr('checked', false);
            $("#allow_gst").attr('disabled', false);
            $("#ddlgststate").attr('disabled', false);
            $(".clsgstclr").attr('disabled', false);
            $("#gst_details").hide();
            $("#txt_corporateCode").val("");
            $("#txtEndorsement").val("");
            $('#dvPccDetails').hide();
            $(".txtclscominpt").val("");

            $.blockUI({
                message: '<img alt="Please Wait..." src="' + loadurl + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
                css: { padding: '5px' }
            });

            $("#ddl_OfficeID").val("").trigger("chosen:updated");
            var Query = {};
            if ($("#rdoaircat_FSC")[0].checked) {
                Query = {
                    CrsPnr: CRSPNR,
                    CRS: CRSTYPE,
                    QueueingNumber: Queuenumber,
                    AirportTypes: AirportType,
                    Corporatename: AgentID,
                    Employeename: Employeename,
                    EmpCostCenter: EmpCostCenter,
                    EmpRefID: EmpRefID,
                    BranchID: BranchID,
                    OfficeID: office_id,//$("#ddl_OfficeID").val(),
                    Faretype: $("#ddl_Faretype").val(),
                    //GroupId : sessionStorage.getItem('Groupid')!=null?sessionStorage.getItem('Groupid').trim():"";
                }
            }
            else {
                Query = {
                    CrsPnr: CRSPNR,
                    CRS: CRSTYPE,
                    QueueingNumber: Queuenumber,
                    AirportTypes: AirportType,
                    Corporatename: AgentID,
                    Employeename: Employeename,
                    EmpCostCenter: EmpCostCenter,
                    EmpRefID: EmpRefID,
                    BranchID: BranchID,
                    Faretype: $("#ddl_Faretype").val(),
                    AirlineCategory: AirlineCategory,
                    AirlinePNR: AirlinePNR,
                    AirlineName: AirlineName,
                    OfficeID: office_id,//$("#ddl_OfficeID").val(),
                    // GroupId: sessionStorage.getItem('Groupid') != null ? sessionStorage.getItem('Groupid').trim() : "";
                }
            }

            $.ajax({
                type: "POST",
                contentType: "Application/json; charset=utf-8",
                url: RAT == false ? QueueticketingUrl : RetrivePNRDetails,
                data: JSON.stringify(Query),
                datatype: "json",
                success: function (data) {

                    $.unblockUI();
                    if (RAT == false) {

                        if (data.Status == "-1") {
                            window.location.href = $('#dvDataProp').data('session-url');
                            return false;
                        }

                        if ((data.Status == "0" || data.Status == "1") && data.Result[0] != "") {
                            showerralert(data.Result[0], "", "");
                            return false;
                        }

                        if (data.Status == "1" && data.Result[1] != "") {

                            $(".wholeclassdiv").show();
                            $(".dv_width").show();
                            $(".employee_details").hide();

                            sessionStorage.setItem("ticketstatus", data.Result[6]);

                            $("#sp_pnr_details").html(data.Result[1]);
                            //if ($("#rdoaircat_FSC")[0].checked == true) {
                            //    _loadFrequentflyerDet(data.AirlineCET, data.TotalCount, data.adtchild);
                            //    $("#dvFrequentFlyer").show();
                            //}

                            $("body").data("TotalCount", data.TotalCount);
                            $("body").data("Paxdet", data.adtchild);
                            $("body").data("Platingcarrier", data.AirlineCET);
                            $("body").data("AirlinePlatingCarriers", data.Result[7]);
                            var StrFareFlag = data.Result[8];
                            //if (StrFareFlag == "N") {
                            //    $('#dvEditEarnings').hide();
                            //}
                            //else {
                            //    $('#dvEditEarnings').show();
                            //}
                            $("body").data("StrFareFlag", StrFareFlag);
                            $('#hdn_pricingToken').val(data.Result[10]);
                            $("#spnTotalgrossfare").html(data.TOTALGrossFare);
                            $("#spnTotalgrossfare").data("originalfare", data.TOTALGrossFare);
                            $(".appcurrency").html(data.Result[11]);
                            //if (StrFareFlag == "N") {
                            //    $(".udk-totalfare-show").hide();
                            //} else {
                            //    $(".udk-totalfare-show").show();
                            //}
                            $("#loadGstDetails").show();
                            $("#dv_row1").show();
                            //$("#btn_getfare").show();
                            //$("#dvCommdetails").html(data.Result[14]);

                            if (data.Result[6] == "1") {
                                addtotalgrsfare();
                                $(".udk-corporate-code").length ? $(".udk-corporate-code").hide() : "";
                                $("#dvdirectbooking").hide();
                                
                                //$("#btn_getfare").hide();
                                //STS-166
                                //$("#btn_getfare span").data("Retrieveflag", "1").html("Accounting");
                                //if (TerminalType != "") {
                                //    if (TerminalType == "T") {
                                //        $("#btn_getfare").show();
                                //    }
                                //    else {
                                //        $("#btn_getfare").hide();
                                //    }
                                //}
                                //$(".wholeclassdiv").show();//STS-166
                                //$(".guestempradiodv").show();
                                $("#loadGstDetails").show();
                                ShowHideAccountingBtn();
                            }
                            else {
                                $("#txt_corporateCode").show();
                                $("#txt_corporateCode").val(data.Corporatecode != null ? data.Corporatecode : "");
                                $("body").data("DataCorporatecode", data.Corporatecode);
                                $("#btn_getfare span").data("Retrieveflag", "0").html("Check Fare");
                                $("#loadGstDetails").show();
                                // $(".wholeclassdiv").hide();//STS-166
                                // $(".guestempradiodv").hide();
                                $("#dv_row1").show();
                                $("#dvdirectbooking").show();
                                $(".udk-corporate-code").show();
                                $("#btn_getfare").show();
                            }

                            // OFFICE ID TEST
                            if (data.Result.length > 12 && data.Result[12] != "" && data.Result[12] != null) {
                                var load_details = [];
                                try { load_details = JSON.parse(data.Result[12]) } catch (e) { BindOfficeID(data.Result[12], "ddl_OfficeID") }
                                if (load_details.length) {
                                    $("#ddl_OfficeID").empty();
                                    if (TerminalType == "T") {
                                        $("#ddl_OfficeID").append("<option value='' selected='selected'> --Select-- </option>");
                                    }

                                    $.each(load_details, function (val, t) {
                                        $("#ddl_OfficeID").append($("<option value='" + t.CCR_PCC + "' >" + t.CCR_PCC + "</option>"));
                                    });
                                    $("#ddl_OfficeID").chosen();
                                }
                              
                            }

                            ////OFFICE ID TEST
                            //   $("#ddl_OfficeID").append($("<option value='" + data.Result[12] + "' >" + data.Result[12] + "</option>"));
                            ////OFFICE ID TEST

                            if (data.Result[3] != "" && data.Result[3] != null) {
                                SpecialFlag = data.Result[3];
                                if (SpecialFlag == "S") {
                                    $('#spnCorporatecodemsg').show();
                                }
                                else {
                                    $('#spnCorporatecodemsg').hide();
                                }
                            }

                            if (data.Result[9] != "") {
                                //  $("#ddl_paymenttype").val("B");
                                //$("#ddl_paymenttype").addClass("disabledclass")
                                // $("#ddl_paymenttype").attr("disabled", true);
                                //  var strcard = data.Result[9].split('|')[0];
                                // $("#ddl_PassthrougUATP").val(strcard);
                                // $("#ddl_PassthrougUATP").addClass("disabledclass")
                                // $("#ddl_PassthrougUATP").attr("disabled", true);
                                // $(".payment-track-ref").hide();
                                // $(".passthrough-details-ref").show();
                            }
                            else {
                                $("#ddl_paymenttype").val("0");
                                //$("#ddl_paymenttype").removeClass("disabledclass");
                                //$("#ddl_paymenttype").attr("disabled", false);
                                //$("#ddl_PassthrougUATP").val('');
                                //$("#ddl_PassthrougUATP").removeClass("disabledclass")
                                //$("#ddl_PassthrougUATP").attr("disabled", false);
                            }

                            $(".employee_details").show();

                            if (data.Result[6] == "1") {
                                $(".RadioSelected").addClass("disabledclass")
                                $(".disabledclass").attr("disabled", true);
                            }
                            else {
                                $(".disabledclass").attr("disabled", false);
                                $(".RadioSelected").removeClass("disabledclass");
                            }

                            if (data.Result[13] != "" && data.Result[13] != null) {
                                var gstdata = data.Result[13];
                                if (gstdata.indexOf("API~") != -1) {
                                    var gstdetails = JSON.parse(gstdata.replace("API~", ""));
                                    var statecode = gstdetails[0].GSTStateCode != "" ? gstdetails[0].GSTStateCode : "ALL";
                                    var gstnumber = gstdetails[0].GSTNumber;
                                    //if (gstnumber != "") {
                                    RetrieveIsGST = true;
                                    $("#ddlgststate").val(statecode);
                                    $("#txtgstNumber").val(gstdetails[0].GSTNumber);
                                    $("#txtgstCompanyname").val(gstdetails[0].GSTCompanyName);
                                    $("#txtgstAddress").val(gstdetails[0].GSTAddress);
                                    $("#txtgstEmailid").val(gstdetails[0].GSTEmailID);
                                    $("#txtgstMobileNumber").val(gstdetails[0].GSTMobileNumber);
                                    $("#allow_gst").attr('checked', true);
                                    $("#allow_gst").attr('disabled', true);
                                    $("#ddlgststate").attr('disabled', true);
                                    $(".clsgstclr").attr('disabled', true);
                                    $("#gst_details").show();
                                    //}
                                    //else {
                                    //    $("#allow_gst").attr('checked', false);
                                    //    $("#allow_gst").attr('disabled', false);
                                    //    $("#ddlgststate").attr('disabled', false);
                                    //    $(".clsgstclr").attr('disabled', false);
                                    //    $("#gst_details").hide();
                                    //}
                                }
                                else {
                                    StrGstdetailsretrieve = JSON.parse(gstdata.replace("APP~", ""));
                                    Changeddlretrievegststate(StrGstdetailsretrieve[0].CLT_GST_STATE);
                                    $("#allow_gst").attr('checked', true);
                                    $("#gst_details").show();
                                }
                            }

                        }

                        if (data.Status == "1" && data.Result[2] != "") {
                            $('#hdf_pnrinfo').val(data.Result[2].replace('[object Object]', ''));
                        }
                        if (data.Status == "1" && ((StrFareFlag == "Y" && data.Result[4] != "") || StrFareFlag == "N")) {
                            $('#hdf_tstcount').val(data.Result[4].replace('[object Object]', ''));

                            var tstcount = data.Result[4].replace('[object Object]', '').split(',');
                            var paxcount = data.Result[5].replace('[object Object]', '').split(',');

                            //for (var i = 0; i < tstcount.length; i++) {
                            //    var input = document.createElement('input');
                            //    input.type = 'hidden';
                            //    input.value = paxcount[i];
                            //    input.id = tstcount[i] + 'hdf_paxcount';
                            //    document.body.appendChild(input);
                            //}
                            var input = "";
                            for (var i = 0; i < tstcount.length; i++) {
                                input += "<input type='hidden' id='" + tstcount[i] + "hdf_paxcount' value='" + paxcount[i] + "' />";
                            }
                            $('#dvPaxCount').html(input);
                        }
                        else {
                            if (data.Message != "") {
                                infoAlert(data.Message, "");
                                return false;
                            } else {
                                infoAlert("No data found!", "");
                                return false;
                            }
                        }
                    }
                    else {

                        if ((data.Status == "0")) {
                            infoAlert(data.Message, "");
                            return false;
                        }
                        else {

                            var ResultData = JSON.parse(data.Result);
                            RetriveDetailsBuilderFlt(ResultData);
                            $(".dv_width").show();
                            $("body").data("sessionkey", data.SessionKey);
                            addtotalgrsfare();
                           
                            //$(".udk-corporate-code").length ? $(".udk-corporate-code").hide() : "";
                            //STS-166
                            //$("#btn_getfare span").data("Retrieveflag", "1").html("Accounting");

                            //$("#btn_getfare").show();
                            $(".wholeclassdiv").show();
                            $("#loadGstDetails").show();
                            ShowHideAccountingBtn();
                        }
                    }

                    if (data.Paymentdetails != null && data.Paymentdetails != "") {
                        loadPaymentDEtails(data.Paymentdetails);//STS-166
                    }
                    else {
                        showerralert("Paymentmode not assigned. Please contact support team (#06).", "", "");
                    }

                    if (AirportType == "I") {
                        $.ajax({
                            type: "GET",
                            url: xmlcountry,
                            dataType: "xml",
                            success: function (xml) {

                                var sBuild = "";
                                //var sBuild = "<option value=''>----Select City----</option>";

                                $(xml).find('COUNTRYDET').each(function () {
                                    var Text = $(this).find('Name').text();
                                    var Value = $(this).find('ID').text();
                                    if (Value == "" || Value == "0") {
                                        sBuild = '<option value="' + Value + '" >' + Text + '</option>'
                                        sBuild += '<option value="IN" >India</option>'
                                    }
                                    if (Value != "IN" && Value != "0" && Value != "")
                                        sBuild += '<option value="' + Value + '" >' + Text + '</option>'
                                });
                                $(".PassIssueCountry").html(sBuild);
                                //if (Paxdetails != null && Paxdetails != "")

                                //    Loadempdetails();
                            },
                            error: function (e) {
                                //Lobibox.alert('error', {   //eg: 'error' , 'success', 'info', 'warning'
                                //    msg: e,
                                //    closeOnEsc: false,
                                //    callback: function ($this, type) {
                                //    }
                                //});
                                showerralert(e.statusText, "", "");
                            }
                        });
                    }

                    //Adult date of birth calendar

                    var dadt = new Date;
                    var day = dadt.getDate();
                    var Month = dadt.getMonth() + 1;
                    var Year = dadt.getFullYear() - 12;
                    if (day < 10) { day = '0' + day } if (Month < 10) { Month = '0' + Month }
                    var maxadtyearadt = day + "/" + Month + "/" + Year;

                    dadt = new Date;
                    day = dadt.getDate();
                    Month = dadt.getMonth() + 1;
                    Year = dadt.getFullYear() - 100;
                    if (day < 10) { day = '0' + day } if (Month < 10) { Month = '0' + Month }
                    var Totyearadt = day + "/" + Month + "/" + Year;
                    ADultdob = Totyearadt;

                    $('.ADTDatepickerint').datepicker({
                        showButtonPanel: true,
                        dateFormat: "dd/mm/yy",
                        minDate: Totyearadt,
                        maxDate: maxadtyearadt
                    });

                    //Child date of birth calendar
                    dadt = new Date;
                    day = dadt.getDate();
                    Month = dadt.getMonth() + 1;
                    Year = dadt.getFullYear() - 2;
                    if (day < 10) { day = '0' + day } if (Month < 10) { Month = '0' + Month }
                    var maxadtyearchd = day + "/" + Month + "/" + Year;

                    Year = dadt.getFullYear() - 12;
                    var Totyearchd = day + "/" + Month + "/" + Year;
                    ChildDob = Totyearchd;

                    $(".CHDDatepickerint").datepicker({
                        showButtonPanel: true,
                        dateFormat: "dd/mm/yy",
                        minDate: Totyearchd,
                        maxDate: maxadtyearchd
                    });

                    //Infant date of birth calendar
                    dadt = new Date;
                    day = dadt.getDate();
                    Month = dadt.getMonth() + 1;
                    Year = dadt.getFullYear();
                    if (day < 10) { day = '0' + day } if (Month < 10) { Month = '0' + Month }
                    var maxChdyearinf = day + "/" + Month + "/" + Year;

                    Year = dadt.getFullYear() - 2;
                    var Totyearinf = day + "/" + Month + "/" + Year;
                    infantDob = Totyearinf;

                    var dd11ss = "";
                    var dd22ss = "";
                    if ($(".Infantagerestricted").html() != null || $(".Infantagerestricted").html() != "") {
                        var dss = new Date($(".Infantagerestricted").html());
                        var nss = dss.toLocaleDateString();
                        var n2ss = "";
                        if (Number(nss.split('/')[0]) <= 9) {
                            n2ss = "0" + nss.split('/')[0];
                        }
                        else {
                            n2ss = nss.split('/')[0];
                        }

                        dd11ss = nss.split('/')[1] + "/" + n2ss + "/" + nss.split('/')[2];
                        dd22ss = nss.split('/')[1] + "/" + n2ss + "/" + (Number(nss.split('/')[2]) - 2);
                    }
                    else {
                        dd22ss = Totyearinf;
                        dd11ss = maxChdyearinf;
                    }

                    $('.INFDatepickerint').datepicker({
                        showButtonPanel: true,
                        dateFormat: "dd/mm/yy",
                        minDate: dd22ss,
                        maxDate: dd11ss
                    });

                    dadt = new Date;
                    day = dadt.getDate();
                    Month = dadt.getMonth() + 1;
                    Year = dadt.getFullYear() + 10;
                    if (day < 10) { day = '0' + day } if (Month < 10) { Month = '0' + Month }
                    var maxpassportexp = day + "/" + Month + "/" + Year;

                    dadt = new Date;
                    day = dadt.getDate();
                    Month = dadt.getMonth() + 1;
                    Year = dadt.getFullYear();
                    if (day < 10) { day = '0' + day } if (Month < 10) { Month = '0' + Month }
                    var maxChdyearinf = day + "/" + Month + "/" + Year;

                    $('.PASSPORTDatepickerint').datepicker({
                        showButtonPanel: true,
                        dateFormat: "dd/mm/yy",
                        minDate: maxChdyearinf,
                        maxDate: maxpassportexp
                    });
                },
                error: function (e) {
                    $.unblockUI();
                    infoAlert('Inter server error!', "");
                    return false;
                }
            });
        }
    }
    catch (e) {

    }
}

function _loadFrequentflyerDet(PlatingCarriers, TotalCount, paxdet) {
    var Num_FLT = PlatingCarriers.split('*');

    var strBuilder = "";
    var noOfCheckBox = document.getElementById('table1').getElementsByTagName('input');

    var ffncnt = 0;
    var ss = 0;
    var cnt = 0;
    var chdcnt = 0;

    for (var k = 0; k < noOfCheckBox.length; k++) {
        if (noOfCheckBox[k].checked == true) {

            TST = noOfCheckBox[k].id.replace('tst', '');
            var paxcount = $('#' + TST + 'hdf_paxcount').val();
            var paxsplit = paxcount.split("|")
            var adult_count = paxcount.split('|')[0];
            var child_count = paxcount.split('|')[1];
            var infant_count = paxcount.split('|')[2];

            var ffn = $.grep(Num_FLT, function (n, i) {
                return n.split('~')[3] == TST
            });

            //var ffnnn = Num_FLT.reduce(function (pre, cru) {
            //    pre[pre.split('~')[3]] = pre[pre.split('~')[3]] || []
            //    pre[pre.split('~')[3]].push(cru);
            //    return pre;
            //});

            TotalCount = parseInt(adult_count) + parseInt(child_count);
            // for (var i = 0; i < TotalCount; i++)
            //{
            // cnt = i + Number(1);
            if (TotalCount > 0) {
                strBuilder += '<div class="col-xs-12 col5">'; //1127
                strBuilder += '<div class="row row0">'; //1127
                strBuilder += '<div class="col-xs-12 col10">';
                strBuilder += '<div class="row row10 tabheaderflg">';
                strBuilder += '<div class="col-sm-3 col-xs-12 col-pad col0">'; //1127
                strBuilder += '<h5>Frequent Flyer Info</h5>';
                strBuilder += '</div>';
                strBuilder += '</div>';
                strBuilder += '<div class="clearfix"></div>';
                strBuilder += '<div class="acc-body row row10">'
                strBuilder += '<div class="row row10">'
                strBuilder += '<div class="col-md-2 col-xs-2 col10 fntwt6">Type'
                strBuilder += '</div>'
                strBuilder += '<div class="col-md-2 col-xs-2 col10 fntwt6">Sector'
                strBuilder += '</div>'
                strBuilder += '<div class="col-md-2 col-xs-4 col10 fntwt6">Airline'
                strBuilder += '</div>'
                strBuilder += '<div class="col-md-2 col-xs-4 col10 fntwt6">FFN Number'
                strBuilder += '</div>'
                strBuilder += '</div>'
                strBuilder += '<div class="row row10">'
                strBuilder += '<div class="col-sm-12 col-xs-12 col10" id="Getalldata" style="margin-top: 10px;">'

                var paxref = '';
                var sgeref = '';

                for (var j = 0; j < ffn.length; j++) {
                    cnt = j + Number(1);
                    if (TST == ffn[j].split("~")[3]) {
                        strBuilder += '<div class="row row10">'
                        if (paxsplit[0] >= cnt) {
                            strBuilder += '<div class="col-sm-3 col-lg-2 col-xs-2 col-pad clspadding">Adult' + cnt + '</div>'
                        }
                        else {
                            chdcnt = cnt - paxsplit[1]
                            strBuilder += '<div class="col-sm-3 col-lg-2 col-xs-2 col-pad clspadding">Child' + chdcnt + '</div>'
                        }

                        strBuilder += '<div class="col-sm-2 col-xs-2 col10">'
                        strBuilder += '<span>' + ffn[j].split("~")[1] + '</span>'
                        strBuilder += '</div>'
                        strBuilder += '<div class="col-sm-2 col-xs-4 col10">'
                        strBuilder += '<select class="form-control clsFFNAirline" data-intinrefffn="0" id="' + TST + 'FFNAirline_' + ffncnt + ffn[j].split("~")[0] + '" onchange="GetFFNNumber(this.id);">'
                        strBuilder += '<option value=' + ffn[j].split("~")[0] + '> ' + loadairportcityname(ffn[j].split("~")[0]) + ' </option>'
                        strBuilder += '</select>'
                        strBuilder += '</div>'
                        strBuilder += '<div class="col-sm-2 col-xs-4 col10">'
                        strBuilder += '<input type="text" class="clspstnumeric txt-anim clshideErr losefocus has-content" autocomplete="off" id="' + TST + 'FFNNUMBER_' + ffncnt + ffn[j].split("~")[0] + '" value="" data-paxref="' + ffn[j].split("~")[2] + '" data-segref="1" data-itinref="0" maxlength="20" onkeypress="javascript:return isNumericVal(event);">'
                        strBuilder += '</div> '
                        strBuilder += '</div>'
                        strBuilder += '<br>'
                        ffncnt++;
                    }
                }
                // }
                strBuilder += '</div>'
                strBuilder += '</div>'
                strBuilder += '</div>';
                strBuilder += '</div>';
                strBuilder += '</div>';
                strBuilder += '</div>';
            }
        }
    }

    $("#dvFrequentFlyer").html(strBuilder);

}


function loadairportcityname(arg) {

    if (arg != null || arg != "" || arg.length != 0) { //Rare scenario...
        if (aryglobalcitys == null || aryglobalcitys.length == 0) {
            loadGlobalcityArrry();
        }
        var aircitynamefilter = aryglobalcitys;
        var aircityname = $.grep(aircitynamefilter, function (n, i) {
            return n._CODE == arg
        });
        var cityfullname = aircityname[0]._NAME;
        cityfullname = cityfullname != "" ? cityfullname : arg;

        return cityfullname;
    }
}

function RetriveDetailsBuilderFlt(arg) {

    var _strload = '';
    var totalgrossamt = 0;
    try {
        //#region Flightdetails//
        _strload += '<div><h4 class="bStyle">Flight Details</h4></div>';
        _strload += '<table id="table1" width="100%" cellpadding="3px" class="no-more-tables table-striped">'
        _strload += '<thead><tr class="dv_table_header" style="border: 1px solid #ddd;text-align: center;"><th>Airline PNR</th><th>CRS PNR</th><th>Airline</th><th>Origin</th><th>Destination</th><th>Dept.Date</th><th>Arr.Date</th><th>Ticketing Carrier</th><th style="padding:5px;">Office ID</th></tr></thead>' //01127
        _strload += '<tbody>'
        for (var i = 0; i < arg.T_T_TICKET_INFO.length; i++) {
            _strload += '<tr id="segrow_' + i + '">'
            _strload += '<td class="clswheat" data-title="Airline PNR">' + arg.T_T_TICKET_INFO[i].TCK_AIRLINE_PNR + '</td>'
            _strload += '<td class="clswheat" data-title="CRS PNR">' + arg.T_T_TICKET_INFO[i].TCK_CRS_PNR + '</td>'
            _strload += '<td class="clswheat" data-title="Airline">' + arg.T_T_TICKET_INFO[i].TCK_FLIGHT_NO + '</td>'
            _strload += '<td class="clswheat" data-title="Origin">' + arg.T_T_TICKET_INFO[i].TCK_ORIGIN_CITY_ID + '</td>'
            _strload += '<td class="clswheat" data-title="Destination">' + arg.T_T_TICKET_INFO[i].TCK_DESTINATION_CITY_ID + '</td>'
            _strload += '<td class="clswheat" data-title="Dept.Date">' + arg.T_T_TICKET_INFO[i].TCK_DEPT_DATE + '</td>'
            _strload += '<td class="clswheat" data-title="Arr.Date">' + arg.T_T_TICKET_INFO[i].TCK_ARRIVAL_DATE + '</td>'
            _strload += '<td class="clswheat" data-title="Ticketing Carrier">' + arg.T_T_TICKET_INFO[i].TCK_PLATING_CARRIER + '</td>'
            _strload += '<td class="clswheat" data-title="Office ID">' + arg.T_T_TICKET_INFO[i].TCK_OFFICE_ID + '</td>'
            _strload += '</tr>'
        }
        BindOfficeID(arg.T_T_TICKET_INFO[0].TCK_OFFICE_ID, "ddl_OfficeID");

        $("#txt_PromoCode").val(arg.T_T_TICKET_INFO[0].TCK_PROMOTIONCODE);

        if (arg.T_T_TICKET_INFO[0].TCK_FARETYPE_CODE != null && arg.T_T_TICKET_INFO[0].TCK_FARETYPE_CODE != "") {
            arg.T_T_TICKET_INFO[0].TCK_FARETYPE_CODE = arg.T_T_TICKET_INFO[0].TCK_FARETYPE_CODE == "H" ? "N" : arg.T_T_TICKET_INFO[0].TCK_FARETYPE_CODE;
            $("#ddl_Faretype").val(arg.T_T_TICKET_INFO[0].TCK_FARETYPE_CODE);
        }
        _strload += '</tbody>'
        _strload += '</table>'
        //#endregion

        $("#spnfareType").html(arg.T_T_TICKET_INFO[0].TCK_FARETYPE_DESCRIPTION);
        $(".udk-faretype-show").show();

        //#region Ticketing Details

        _strload += '<div style="margin-top: 2%;"><h4 class="bStyle">Pax Details</h4></div>';

        _strload += '<table id="table2" class="table no-more-tables table-striped" width="100%" cellpadding="3px">';
        _strload += '<thead><tr class="dv_table_header" style="text-align: center;background:#b60725!important;"><th class="td_center">Title</th><th class="td_center">Name</th><th class="td_center">Ticket No.</th><th class="td_center">Pax Type</th><th class="td_center">DOB</th><th class="td_right"><nobr>Base Amount</nobr><nobr></nobr></th><th class="td_right"><nobr>Gross Amount</nobr></th> <th class="td_right"><nobr>Discount</nobr></th> <th class="td_right"><nobr>Markup</nobr></th></tr></thead>'

        _strload += '<tbody>'

        var discount = "";
        var markup = "";
        var TotalFares = 0;
        for (var i = 0; i < arg.T_T_PASSENGER_INFO.length; i++) {

            discount = arg.T_T_PAX_CLASS_INFO[i].PCI_DISCOUNT_AMT != null && arg.T_T_PAX_CLASS_INFO[i].PCI_DISCOUNT_AMT != "" ? arg.T_T_PAX_CLASS_INFO[i].PCI_DISCOUNT_AMT : "0";

            markup = arg.T_T_PAX_CLASS_INFO[i].PCI_MARKUP != null && arg.T_T_PAX_CLASS_INFO[i].PCI_MARKUP != "" ? arg.T_T_PAX_CLASS_INFO[i].PCI_MARKUP : "0";

            _strload += '<tr id="PS_segrow_' + i + '">'
            _strload += '<td class="clswheat" data-title="Title">' + arg.T_T_PASSENGER_INFO[i].PSG_PASSENGER_TITLE + '</td>'
            _strload += '<td class="clswheat" data-title="Name">' + arg.T_T_PASSENGER_INFO[i].PSG_FIRST_NAME + ' ' + arg.T_T_PASSENGER_INFO[i].PSG_LAST_NAME + ' </td>'
            _strload += '<td class="clswheat" data-title="Ticket No.">' + arg.T_T_PAX_CLASS_INFO[i].PCI_TICKET_NO + '</td>'
            _strload += '<td class="clswheat" data-title="Pax Type">' + arg.T_T_PASSENGER_INFO[i].PSG_PASSENGER_TYPE + '</td>'
            _strload += '<td class="clswheat" data-title="DOB">' + arg.T_T_PASSENGER_INFO[i].PSG_DOB + '</td>'
            _strload += '<td class="clswheat" data-title="Base Amount">' + arg.T_T_PAX_CLASS_INFO[i].PCI_BASIC_FARE + '</td>'
            _strload += '<td class="clswheat" data-title="Gross Amount">' + arg.T_T_PAX_CLASS_INFO[i].PCI_GROSSAMOUNT + '</td>'
            totalgrossamt += Number(arg.T_T_PAX_CLASS_INFO[i].PCI_GROSSAMOUNT);

            _strload += "<td class='td_right clswheat' data-paxtype=" + arg.T_T_PASSENGER_INFO[i].PSG_PASSENGER_TYPE + " data-title='Discount'> <input class='disvalue' data-paxtype=" + arg.T_T_PASSENGER_INFO[i].PSG_PASSENGER_TYPE + " type='text' value=" + discount + ">  </td>";
            _strload += "<td class='td_right clswheat' data-paxtype=" + arg.T_T_PASSENGER_INFO[i].PSG_PASSENGER_TYPE + " data-title='Markup'> <input class='markvalue' data-paxtype=" + arg.T_T_PASSENGER_INFO[i].PSG_PASSENGER_TYPE + " type='text' value=" + markup + " onkeypress='javascript:return isNumericVal(event);' onkeyup='addtotalgrsfare(this)'> </td>"
            //_strload += '<td class="clswheat" data-title="Discount disvalue">' + arg.T_T_PAX_CLASS_INFO[i].PCI_DISCOUNT_AMT + '</td>'
            // _strload += '<td class="clswheat" data-title="Markup markvalue">' + arg.T_T_PAX_CLASS_INFO[i].PCI_MARKUP + '</td>'
            _strload += '</tr>'
        }

        _strload += '<tbody>'
        _strload += '</table>'
        _strload += '<div style="margin-top: 2%;"><h4 class="bStyle">SSR DETAILS</h4></div>'
        _strload += '<table id="table3" class="table no-more-tables table-striped" width="100%" cellpadding="3px">';
        _strload += '<thead><tr class="dv_table_header" style="text-align: center;"><th class="td_center">Origin</th><th class="td_center">Destination</th><th class="td_center">Passenger Name</th><th class="td_center">Baggage</th><th class="td_right"><nobr>Baggage Amt</nobr><nobr></nobr></th>  <th class="td_center">Meal</th>    <th class="td_right"><nobr>Meal Amt</nobr></th> <th class="td_center">Seat</th><th class="td_right"><nobr>Seat Amt</nobr></th> <th class="td_center">Wheel</th> <th class="td_right"><nobr>Wheel Amt</nobr></th></tr></thead>'
        _strload += '<tbody>'

        var Seat = "";
        var Seatamt = "";
        var meal = "";
        var mealamt = "";
        var wheel = "";
        var wheelamt = "";



        for (var i = 0; i < arg.T_T_PAX_CLASS_INFO.length; i++) {
            arg.T_T_PAX_CLASS_INFO[i].PSG_FIRST_NAME = "";
            arg.T_T_PAX_CLASS_INFO[i].PSG_LAST_NAME = "";
            arg.T_T_PAX_CLASS_INFO[i].TCK_ORIGIN_CITY_ID = "";
            arg.T_T_PAX_CLASS_INFO[i].TCK_DESTINATION_CITY_ID = "";
        }
        var T_T_PAX_CLASS_INFO = filterPAX_CLS(arg.T_T_PAX_CLASS_INFO, arg.T_T_PASSENGER_INFO)

        for (var i = 0; i < T_T_PAX_CLASS_INFO.length; i++) {

            for (var j = 0; j < arg.T_T_TICKET_INFO.length; j++) {

                if (T_T_PAX_CLASS_INFO[i].PCI_SEGMENT_NO == arg.T_T_TICKET_INFO[j].TCK_SEGMENT_NO) {
                    T_T_PAX_CLASS_INFO[i].TCK_ORIGIN_CITY_ID = arg.T_T_TICKET_INFO[j].TCK_ORIGIN_CITY_ID;
                    T_T_PAX_CLASS_INFO[i].TCK_DESTINATION_CITY_ID = arg.T_T_TICKET_INFO[j].TCK_DESTINATION_CITY_ID;
                }
            }
        }

        var T_T_TICKET_INFO = T_T_PAX_CLASS_INFO;

        for (var i = 0; i < T_T_PAX_CLASS_INFO.length; i++) {
            _strload += '<tr id="PS_segrow_' + i + '">'

            Seat = T_T_PAX_CLASS_INFO[i].PCI_SEAT_SEL != null && T_T_PAX_CLASS_INFO[i].PCI_SEAT_SEL != "" ? T_T_PAX_CLASS_INFO[i].PCI_SEAT_SEL : " - ";
            Seatamt = T_T_PAX_CLASS_INFO[i].PCI_SEAT_SEL_AMT != null && T_T_PAX_CLASS_INFO[i].PCI_SEAT_SEL_AMT != "" ? T_T_PAX_CLASS_INFO[i].PCI_SEAT_SEL_AMT : "0";
            mealamt = T_T_PAX_CLASS_INFO[i].PCI_MEALS_AMOUNT != null && T_T_PAX_CLASS_INFO[i].PCI_MEALS_AMOUNT != "" ? T_T_PAX_CLASS_INFO[i].PCI_MEALS_AMOUNT : "0";
            meal = T_T_PAX_CLASS_INFO[i].PCI_MEALS != null && T_T_PAX_CLASS_INFO[i].PCI_MEALS != "" ? T_T_PAX_CLASS_INFO[i].PCI_MEALS : " - ";
            wheel = T_T_PAX_CLASS_INFO[i].PCI_WCR != null && T_T_PAX_CLASS_INFO[i].PCI_WCR != "" ? T_T_PAX_CLASS_INFO[i].PCI_WCR : " - ";
            wheelamt = T_T_PAX_CLASS_INFO[i].PCI_WCR_AMOUNT != null && T_T_PAX_CLASS_INFO[i].PCI_WCR_AMOUNT != "" ? T_T_PAX_CLASS_INFO[i].PCI_WCR_AMOUNT : "0";

            _strload += '<td class="clswheat" data-title="Origin">' + T_T_TICKET_INFO[i].TCK_ORIGIN_CITY_ID + '</td>'
            _strload += '<td class="clswheat" data-title="Destination">' + T_T_TICKET_INFO[i].TCK_DESTINATION_CITY_ID + ' </td>'
            _strload += '<td class="clswheat" data-title="Passenger Name">' + T_T_PAX_CLASS_INFO[i].PSG_FIRST_NAME + ' ' + T_T_PAX_CLASS_INFO[i].PSG_LAST_NAME + ' </td>'
            _strload += '<td class="clswheat" data-title="Baggage">' + T_T_PAX_CLASS_INFO[i].PCI_BAGGAGE + '</td>'
            _strload += '<td class="clswheat" data-title="Baggage Amt">' + T_T_PAX_CLASS_INFO[i].PCI_BAGGAGE_AMOUNT + '</td>'
            _strload += '<td class="clswheat" data-title="Meal">' + meal + '</td>'
            _strload += '<td class="clswheat" data-title="Meal Amt">' + mealamt + '</td>'
            _strload += '<td class="clswheat" data-title="Seat">' + Seat + '</td>'
            _strload += '<td class="clswheat" data-title="Seat Amt">' + Seatamt + '</td>'
            _strload += '<td class="clswheat" data-title="Wheel">' + wheel + '</td>'
            _strload += '<td class="clswheat" data-title="Wheel Amt">' + wheelamt + '</td>'
            _strload += '</tr>'
        }
        _strload += '<tbody>'
        _strload += '</table>'
        //#endregion
        $("#sp_pnr_details").html(_strload);
        $("#spnTotalgrossfare").html(totalgrossamt);//+ " INR"
        $("#spnTotalgrossfare").data("originalfare", totalgrossamt);
        $(".udk-totalfare-show").show();
        $("#dv_row1").show();
        $("#loadGstDetails").show();
        $("#btn_getfare").show();
        $("#btn_getfare").parent().removeClass('col-sm-offset-1');
    } catch (e) {
        // alert("TEST");
    }
    $("body").data("totalgrossamt", totalgrossamt);
}

function filterPAX_CLS(list, filterArr) {
    var testgrpavail, grpAvailtest = [];
    $.map(filterArr, function (r, k) {
        testgrpavail = $.grep(list, function (l, j) {
            if (r.PSG_PAX_REF_NO == l.PCI_PAX_REF_NO) {
                return l.PSG_FIRST_NAME = r.PSG_FIRST_NAME, l.PSG_LAST_NAME = r.PSG_LAST_NAME;
            }
        });
        if (testgrpavail.length > 0) {
            grpAvailtest.push.apply(grpAvailtest, testgrpavail);
        }
    });
    return grpAvailtest;
}

function filterTICKET_INFO(list, ticket_info) {

    var testgrpavail, grpAvailtest = [];
    $.map(ticket_info, function (r, k) {
        testgrpavail = $.grep(list, function (l, j) {
            if (r.PCI_SEGMENT_NO == l.TCK_SEGMENT_NO) {
                return l.TCK_ORIGIN_CITY_ID = r.TCK_ORIGIN_CITY_ID, l.TCK_DESTINATION_CITY_ID = r.TCK_DESTINATION_CITY_ID;
            }
        });
        if (testgrpavail.length > 0) {
            grpAvailtest.push.apply(grpAvailtest, testgrpavail);
        }
    });

    return grpAvailtest;
}

function checkClick(arg) {

    if (document.all && !document.addEventListener) {
        var forIE8andLower = arg.parentNode.parentNode.childNodes[0];
        if (forIE8andLower.checked == true) {
            forIE8andLower.checked = false;
        }
        else if (forIE8andLower.checked == false) {
            forIE8andLower.checked = true;
        }
    }
    if (arg.className == 'RadioDeselected') {
        arg.className = 'RadioSelected';
        var id = arg.id;
        for (var i = 0; i < 50; i++) {
            if ($('#' + i + 'segrow' + id).length) {
                $('#' + i + 'segrow' + id).removeClass('clswite').addClass("clsgrey");
            }
            if ($('#' + i + 'title_adult_tr' + id).length) {
                $('#' + i + 'title_adult_tr' + id).removeClass('clswite').addClass("clsgrey");
            }
            if ($('#' + i + 'title_child_tr' + id).length) {
                $('#' + i + 'title_child_tr' + id).removeClass('clswite').addClass("clsgrey");
            }
            if ($('#' + i + 'title_infant_tr' + id).length) {
                // document.getElementById(i + 'title_infant_tr' + id).style.background = "lightgrey";
                $('#' + i + 'title_infant_tr' + id).removeClass('clswite').addClass("clsgrey");
            }
        }
    }
    else if (arg.className == 'RadioSelected') {
        arg.className = 'RadioDeselected';
        var id = arg.id;
        for (var i = 0; i < 50; i++) {
            if ($('#' + i + 'segrow' + id).length) {
                $('#' + i + 'segrow' + id).removeClass('clsgrey').addClass("clswite");
            }
            if ($('#' + i + 'title_adult_tr' + id).length) {
                $('#' + i + 'title_adult_tr' + id).removeClass('clsgrey').addClass("clswite");
            }
            if ($('#' + i + 'title_child_tr' + id).length) {
                $('#' + i + 'title_child_tr' + id).removeClass('clsgrey').addClass("clswite");
            }
            if ($('#' + i + 'title_infant_tr' + id).length) {
                //document.getElementById(i + 'title_infant_tr' + id).style.background = "white";
                $('#' + i + 'title_infant_tr' + id).removeClass('clsgrey').addClass("clswite");
            }
        }
    }

}

function ValidationQueue() {

    if ($("#rdoaircat_FSC")[0].checked == true) {
        if ($("#txt_CRSpnr_Queue").val() == null || $.trim($("#txt_CRSpnr_Queue").val()) == "") {
            infoAlert('Please enter the CRS PNR!', "txt_CRSpnr_Queue");
            return false;
        }
    } else {
        if ($("#txt_Airlinepnr_Queue").val() == null || $.trim($("#txt_Airlinepnr_Queue").val()) == "") {
            infoAlert('Please enter the Airline PNR!', "txt_Airlinepnr_Queue");
            return false;
        }
    }

    if ($("#rdoaircat_FSC")[0].checked == true) {
        if ($("#ddl_QueueStock").val() == null || $.trim($("#ddl_QueueStock").val()) == "-1") {
            infoAlert('Please select the CRS name!', "ddl_QueueStock");
            return false;
        }
    } else {
        if ($("#ddl_AirlineStock").val() == null || $.trim($("#ddl_AirlineStock").val()) == "-1") {
            infoAlert('Please select the Airline name!', "ddl_AirlineStock");
            return false;
        }
    }

    if ($("#ddl_QueueStock").val() == "1G") {
        if ($("#txt_CRSpnr_QueueNO").val() == "") {
            infoAlert('Please enter the Queueing Number!', "txt_CRSpnr_QueueNO");
            return false;
        }
    }

    if ($("#ddlOfficeIdRetrieve").length && $("#ddlOfficeIdRetrieve").val() == "") {
        infoAlert('Please select Office ID', "ddlOfficeIdRetrieve");
        return false;
    }

    if (TerminalType == "T" && $("#ddlclient").val() == "") {
        infoAlert('Please select Agency name !', "ddlclient");
        return false;
    }
    //STS-166
    //if ($("#ddl_Faretype").val() == "-1" || $("#ddl_Faretype").val() == "") {
    //    infoAlert('Please select fare type !', "ddl_Faretype");
    //    return false; 
    //}

    return true;
}
var boolresofficeid = false;
var Strcardnumber = "";
var selectedpaxdetails = [], faredetailsarray = [];
function Get_Fare(arg) {
    try {
        $("#spnSuppComm").html("");
        $('#txt_PricingCode').val("");

        var temp = "";
        var missing_title = "";
        var missing_Passport = "";
        var TST = "";
        var totalfare = 0;
        var totalfares = 0;
        var noOfCheckBox = document.getElementById('table1').getElementsByTagName('input');
        var status = false;
        var RAT = true;
        var RATLCC = true;
        var CRSTYPE = "";

        if ($("#rdoaircat_FSC")[0].checked) {
            RATLCC = false;
            CRSTYPE = $("#ddl_QueueStock").val().trim();
        }
        else {
            RATLCC = true;
            CRSTYPE = $("#ddl_AirlineStock").val().split('|')[0];
        }

        var AirportType = $("#ddl_QueueAT").val().trim();

        if (sessionStorage.getItem("ticketstatus") == "1") {
            RAT = false;
            $("#btn_getfare span").html("Accounting");
        }
        else {
            RAT = true;
            $("#btn_getfare span").next().html("Check Fare");
        }

        for (var j = 0; j < noOfCheckBox.length; j++) {
            if (noOfCheckBox[j].checked == true) {
                status = true;
                totalfare = totalfare + noOfCheckBox[j].value;
                TST = noOfCheckBox[j].id.replace('tst', '');

                var StrFareFlag = $("body").data("StrFareFlag");
                var FareType = "";
                if (StrFareFlag != "N") {

                    var FareType = $('#' + TST).data("farequalifier").toUpperCase().trim();
                }

                $("body").data("FareTypeCorp", FareType);
                //if (FareType == "S" && $("#txt_corporateCode").length > 0 && ($("#txt_corporateCode").val() == null || $("#txt_corporateCode").val() == ""))
                //{
                //    $("#txt_corporateCode").val("N");
                //}

                var paxcount = $('#' + TST + 'hdf_paxcount').val();
                var adult_count = paxcount.split('|')[0];
                var child_count = paxcount.split('|')[1];
                var infant_count = paxcount.split('|')[2];

                if ($('#' + TST + 'title_adult1').length) {
                    for (var i = 1; i <= adult_count; i++) {
                        if ($('#' + TST + 'title_adult' + i).length) missing_title += $('#' + TST + 'title_adult' + i).val() + ",";
                        else break;
                    }
                }
                if ($('#' + TST + 'title_child1').length) {
                    for (var i = 1; i <= child_count; i++) {
                        if ($('#' + TST + 'title_child' + i).length) missing_title += $('#' + TST + 'title_child' + i).val() + ",";
                        else break;
                    }
                }
                if ($('#' + TST + 'title_infant1').length) {
                    for (var i = 1; i <= infant_count; i++) {
                        if ($('#' + TST + 'title_infant' + i).length) missing_title += $('#' + TST + 'title_infant' + i).val() + ",";
                        else break;
                    }
                }
            }
        }

        for (var i = 1; i <= adult_count; i++) {
            if (($('#' + TST + 'Gender_adult' + i).length) > 1) {
                if (AirportType == "I") {
                    if (($('#' + TST + 'Gender_adult' + i).length) > 1 && $('#' + TST + 'Gender_adult' + i).val() == null || $('#' + TST + 'Gender_adult' + i).val() == "0" || $('#' + TST + 'Gender_adult' + i).val() == "") {
                        infoAlert("Please select gender for adult " + i, "");
                        return false;
                    }
                    if ($('#' + TST + 'Passwort_adult' + i).val() != null && $('#' + TST + 'Passwort_adult' + i).val() != "") {
                        if ($('#' + TST + 'ADultDOB' + i).val() == null || $('#' + TST + 'ADultDOB' + i).val() == "") {
                            infoAlert("Please select date of birth for adult " + i, "");
                            return false;
                        }
                        if ($('#' + TST + 'IssuedCountry' + i).val() == null || $('#' + TST + 'IssuedCountry' + i).val() == "0" || $('#' + TST + 'IssuedCountry' + i).val() == "") {
                            infoAlert("Please select passport issued country for adult " + i, "");
                            return false;
                        }
                        if ($('#' + TST + 'ExpiryDate' + i).val() == null || $('#' + TST + 'ExpiryDate' + i).val() == "") {
                            infoAlert("Please select passport expiry date for adult " + i, "");
                            return false;
                        }
                    }
                }
            }
        }

        for (var i = 1; i <= child_count; i++) {
            if (($('#' + TST + 'Gender_Child' + i).length) > 1) {
                if (AirportType == "I") {
                    if ($('#' + TST + 'Passwort_child' + i).val() != null && $('#' + TST + 'Passwort_child' + i).val() != "") {
                        if ($('#' + TST + 'ChildDOB' + i).val() == null || $('#' + TST + 'ChildDOB' + i).val() == "") {
                            infoAlert("Please select date of birth for child " + i, "");
                            return false;
                        }
                        if ($('#' + TST + 'Gender_Child' + i).val() == null || $('#' + TST + 'Gender_Child' + i).val() == "0" || $('#' + TST + 'Gender_Child' + i).val() == "") {
                            infoAlert("Please select gender for child " + i, "");
                            return false;
                        }
                        if ($('#' + TST + 'childIssuedCountry' + i).val() == null || $('#' + TST + 'childIssuedCountry' + i).val() == "0" || $('#' + TST + 'childIssuedCountry' + i).val() == "0") {
                            infoAlert("Please select passport issued country for child " + i, "");
                            return false;
                        }
                        if ($('#' + TST + 'childExpiryDate' + i).val() == null || $('#' + TST + 'childExpiryDate' + i).val() == "") {
                            infoAlert("Please select passport expiry date for child " + i, "");
                            return false;
                        }
                    }
                }
            }
        }

        for (var i = 1; i <= infant_count; i++) {
            if (($('#' + TST + 'Gender_Infant' + i).length) > 1) {

                if (AirportType == "I") {
                    if ($('#' + TST + 'Gender_Infant' + i).val() == null || $('#' + TST + 'Gender_Infant' + i).val() == "0" || $('#' + TST + 'Gender_Infant' + i).val() == "") {
                        infoAlert("Please select gender for infant " + i, "");
                        return false;
                    }
                    if ($('#' + TST + 'Passwort_infant' + i).val() != null && $('#' + TST + 'Passwort_infant' + i).val() != "") {
                        if ($('#' + TST + 'InfantDOB' + i).val() == null || $('#' + TST + 'InfantDOB' + i).val() == "") {
                            infoAlert("Please select date of birth for infant " + i, "");
                            return false;
                        }
                        if ($('#' + TST + 'infantIssuedCountry' + i).val() == null || $('#' + TST + 'infantIssuedCountry' + i).val() == "0" || $('#' + TST + 'infantIssuedCountry' + i).val() == "") {
                            infoAlert("Please select passport issued country for infant " + i, "");
                            return false;
                        }
                        if ($('#' + TST + 'infantExpiryDate' + i).val() == null || $('#' + TST + 'infantExpiryDate' + i).val() == "") {
                            infoAlert("Please select passport expiry date for infant " + i, "");
                            return false;
                        }
                    }
                }
            }
        }

        if (status == false && RAT != false && RATLCC == false) {
            infoAlert("Select atleast one TST to check fare!", "");
            return false;
        }

        if (missing_title.indexOf("Title") > -1 && RAT != false && RATLCC == false) {
            infoAlert("Please select Title", "");
            return false;
        }

        //if (missing_Passport.replace(",", "") != "" && $("#ddl_QueueAT").val()=="I" )// && RAT != false && RATLCC == false && $("#ddl_QueueAT").val()=="I") {
        //    infoAlert("Please enter passport No.", "");
        //    return false;
        //}

        var strCount = $(".RadioSelected").length;
        if (parseInt(strCount) > 1 && RAT != false && RATLCC == false) {
            infoAlert("Only one TST is allowed at a time.", "");
            return false;
        }

        //if (sessionStorage.getItem("ticketstatus") == "1") {

        if (GST == "Y" && $("#ddlgststate").val() != "ALL") {
            if ($("#ddlgststate").val() == null || $("#ddlgststate").val() == "") {
                infoAlert("Please select GST state.", "ddlgststate");
                return false;
            }
            if ($("#txtgstNumber").val() == null || $("#txtgstNumber").val() == "") {
                infoAlert("Please enter GST number.", "txtgstNumber");
                return false;
            }
            if ($("#txtgstCompanyname").val() == null || $("#txtgstCompanyname").val() == "") {
                infoAlert("Please enter GST Company .", "txtgstCompanyname");
                return false;
            }
            if ($("#txtgstAddress").val() == null || $("#txtgstAddress").val() == "") {
                infoAlert("Please enter GST Address .", "txtgstAddress");
                return false;
            }
            if ($("#txtgstEmailid").val() == null || $("#txtgstEmailid").val() == "") {
                infoAlert("Please enter GST Mail id .", "txtgstEmailid");
                return false;
            }

            var gstmailid = $('#txtgstEmailid').val();
            if (gstmailid != "" && gstmailid != null) {
                var emailReg = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
                if (!emailReg.test(gstmailid)) {
                    alert('Please Enter Valid GST Mail Id');
                    return false;
                }
            }

            if ($("#txtgstMobileNumber").val() == null || $("#txtgstMobileNumber").val() == "") {
                infoAlert("Please enter GST Mobile number .", "txtgstMobileNumber");
                return false;
            }

            if (CRSTYPE == "1G") {
                if ($("#txtgstStatecode").val() == null || $("#txtgstStatecode").val() == "") {
                    infoAlert("Please enter GST State code .", "txtgstStatecode");
                    return false;
                }
                if ($("#txtgstCitycode").val() == null || $("#txtgstCitycode").val() == "") {
                    infoAlert("Please enter GST City Code .", "txtgstCitycode");
                    return false;
                }
                if ($("#txtgstpinecode").val() == null || $("#txtgstpinecode").val() == "") {
                    infoAlert("Please enter GST Pin Code .", "txtgstpinecode");
                    return false;
                }
            }
        }

        var noOfCheckBox = document.getElementById('table1').getElementsByTagName('input');
        var tst_count = "";
        for (var i = 0; i < noOfCheckBox.length; i++) {
            if (noOfCheckBox[i].checked) {
                if (tst_count == "") {
                    tst_count += noOfCheckBox[i].id.replace('tst', '');
                }
                else {
                    tst_count += "," + noOfCheckBox[i].id.replace('tst', '');
                }
            }
        }


        var Corporatecode = $("#txt_corporateCode").length ? $("#txt_corporateCode").val() : "";

        if (Corporatecode == "" && SpecialFlag == "S") {
            Corporatecode = "N";
        }
        var hdf_pnrinfo = $('#hdf_pnrinfo').val();
        var crspnr = $("#hdn_crspnr").val();
        var crs = $("#hdn_crs").val();
        var queuingNum = $("#txt_CRSpnr_QueueNO").val();

        var Corporatename = $("#ddlclient").val();
        var Employeename = "";
        var Empmailname = "";
        var Guestmailid = "";
        var GuestPhoneno = "";
        var EmpCostCenter = "";
        var EmpRefID = "";
        var Newempflag = "";

        //var BranchID = sessionStorage.getItem("branchid") != null ? sessionStorage.getItem("branchid") : "";
        //var AgentID = sessionStorage.getItem("clientid") != null ? sessionStorage.getItem("clientid") : "";

        var AgentID = $('#hdnClientID').val();
        var BranchID = $('#hdnBranchID').val();

        var GstDetails = "";
        var GSTstate = "";
        var GSTNumber = "";
        var GSTCompany = "";
        var GSTAddress = "";
        var GSTemailid = "";
        var GSTmobilenumber = "";
        var GSTStatecode = "";
        var GSTCityCode = "";
        var GSTPincode = "";

        if (GST == "Y") {
            GSTstate = $("#ddlgststate").val();
            GSTNumber = $("#txtgstNumber").val();
            GSTCompany = $("#txtgstCompanyname").val();
            GSTAddress = $("#txtgstAddress").val();
            GSTemailid = $("#txtgstEmailid").val();
            GSTmobilenumber = $("#txtgstMobileNumber").val();
            GSTStatecode = $("#txtgstStatecode").val();
            GSTCityCode = $("#txtgstCitycode").val();
            GSTPincode = $("#txtgstpinecode").val();
        }

        //if (gstflag == "0") {
        if (CRSTYPE == "1A") {
            GstDetails = GSTstate + '|' + GSTNumber + '|' + GSTCompany + '|' + GSTAddress + '|' + GSTemailid + '|' + GSTmobilenumber;
        }
        else {
            GstDetails = GSTstate + '|' + GSTNumber + '|' + GSTCompany + '|' + GSTAddress + '|' + GSTemailid + '|' + GSTmobilenumber + '|' + GSTStatecode + '|' + GSTCityCode + '|' + GSTPincode;
        }
        //}
        //else {
        //    GstDetails = "";
        //}
        var FFNDetails = "";
        var PassportDetails = "";
        var adtPP = "", chdPP = "", infPP = "";
        var paxtype = "";
        $(".passportno").each(function () {
            paxtype = $(this).data("paxtype")
            if (paxtype == "ADT") {
                adtPP += $(this).val() != "" ? $(this).val() + "~" : "";
            }
            else if (paxtype == "CHD") {
                chdPP += $(this).val() != "" ? $(this).val() + "~" : "";
            }
            else {
                infPP += $(this).val() != "" ? $(this).val() + "~" : "";
            }
        });

        PassportDetails = adtPP + chdPP + infPP;
        //STS-166
        var discountvalue = "";
        var markupvalue = "";
        paxtype = "";
        var adtdisc = "", chddisc = "", infdisc = "";
        var adtmark = "", chdmark = "", infmark = "";

        $(".clsgrey .disvalue").each(function () {
            paxtype = $(this).data("paxtype")
            if (paxtype == "ADT") {
                adtdisc += $(this).val() != "" ? $(this).val() + "~" : "";
            }
            else if (paxtype == "CHD") {
                chddisc += $(this).val() != "" ? $(this).val() + "~" : "";
            }
            else {
                infdisc += $(this).val() != "" ? $(this).val() + "~" : "";
            }

        });
        discountvalue = adtdisc + chddisc + infdisc;
        $(".clsgrey .markvalue").each(function () {
            paxtype = $(this).data("paxtype")
            if (paxtype == "ADT") {
                adtmark += $(this).val() != "" ? $(this).val() + "~" : "";
            }
            else if (paxtype == "CHD") {
                chdmark += $(this).val() != "" ? $(this).val() + "~" : "";
            }
            else {
                infmark += $(this).val() != "" ? $(this).val() + "~" : "";
            }

        });
        markupvalue = adtmark + chdmark + infmark;
        //STS-166

        if ($("#btn_getfare span").data("Retrieveflag").trim() != "0") {
            var faretypereason = "";
            var OtherTicketInfo = "";
            var erromanflag = false;
            var errorval = "";
            var checkCondition_BUDGET,
                checkCondition_TRAVEL_REQUEST,
                checkCondition_Sub_Reason,
                checkCondition_Recharge,
                checkCondition_Reason_of_travel,
                checkCondition_CostcenterID,
                checkCondition_Job_number,
                checkCondition_Package_id = "";

            $(".slttxtfrereason").each(function () {
                if ($(this).data("mandatoryflag") == "Y") {
                    if ($(this).val() == "") {
                        erromanflag = true;
                        errorval = $(this).data("idname");
                        return false; //This line is coming out from the each function  by vijai 23032018
                    }
                }
                if ($(this).data("idname").toUpperCase().indexOf("BUDGET CODE") != -1)
                    checkCondition_BUDGET = $(this).data("idname").toUpperCase().indexOf("BUDGET CODE") != -1 ? $(this).val() : "";
                else if ($(this).data("idname").toUpperCase().indexOf("TRAVEL REQUEST NO.") != -1)
                    checkCondition_TRAVEL_REQUEST = $(this).data("idname").toUpperCase().indexOf("TRAVEL REQUEST NO.") != -1 ? $(this).val() : "";
                else if ($(this).data("idname").toUpperCase().indexOf("SUB REASON") != -1)
                    checkCondition_Sub_Reason = $(this).data("idname").toUpperCase().indexOf("SUB REASON") != -1 ? $(this).val() : "";
                else if ($(this).data("idname").toUpperCase().indexOf("RECHARGE") != -1)
                    checkCondition_Recharge = $(this).data("idname").toUpperCase().indexOf("RECHARGE") != -1 ? $(this).val() : "";
                else if ($(this).data("idname").toUpperCase().indexOf("REASON OF TRAVEL") != -1)
                    checkCondition_Reason_of_travel = $(this).data("idname").toUpperCase().indexOf("REASON OF TRAVEL") != -1 ? $(this).val() : "";
                else if (true)
                    checkCondition_CostcenterID = "";
                else if (true)
                    checkCondition_Job_number = "";
                else if (true)
                    checkCondition_Package_id = "";

                faretypereason += $(this).data("dataid") + "|" + $(this).data("idname") + "|" + $(this).val() + "~";

            });

            checkCondition_BUDGET = checkCondition_BUDGET != null ? checkCondition_BUDGET : ""
            checkCondition_TRAVEL_REQUEST = checkCondition_TRAVEL_REQUEST != null ? checkCondition_TRAVEL_REQUEST : ""
            checkCondition_Sub_Reason = checkCondition_Sub_Reason != null ? checkCondition_Sub_Reason : ""
            checkCondition_Recharge = checkCondition_Recharge != null ? checkCondition_Recharge : ""
            checkCondition_Reason_of_travel = checkCondition_Reason_of_travel != null ? checkCondition_Reason_of_travel : ""
            checkCondition_CostcenterID = checkCondition_CostcenterID != null ? checkCondition_CostcenterID : ""
            checkCondition_Job_number = checkCondition_Job_number != null ? checkCondition_Job_number : ""
            checkCondition_Package_id = checkCondition_Package_id != null ? checkCondition_Package_id : ""

            OtherTicketInfo = checkCondition_BUDGET + "|" +
                               checkCondition_TRAVEL_REQUEST + "|" +
                                   checkCondition_Sub_Reason + "|" +
                                   checkCondition_Recharge + "|" +
                                   checkCondition_Reason_of_travel + "|" +
                                   checkCondition_CostcenterID + "|" +
                                   checkCondition_Job_number + "|" +
                                   checkCondition_Package_id;

            if (erromanflag) {
                //Lobibox.alert('warning', {   //eg: 'error' , 'success', 'warning', 'warning'
                //    msg: "Please select " + errorval,
                //    closeOnEsc: false,
                //    callback: function ($this, type) {
                //        $('#' + errorval).focus();
                //    }
                //});
                infoAlert("Please select " + errorval, errorval);
                return false;
            }

            if ($("#ddl_paymenttype").val() == "0" || $("#ddl_paymenttype").val() == "") {
                //Lobibox.alert('warning', {   //eg: 'error' , 'success', 'warning', 'warning'
                //    msg: "Please select payment mode.",
                //    closeOnEsc: false,
                //    callback: function ($this, type) {
                //        $('#ddl_paymenttype').focus();
                //    }
                //});
                infoAlert("Please select payment mode.", "ddl_paymenttype");
                return false;
            }

            if ($("#ddl_paymenttype").val() == "P") {
                if ($("#ddl_paymenttype").val() == "") {
                    infoAlert("Please enter payment track id.", "ddl_PaymentREF");
                    return false;
                }

            }

            //if ($('input[name="B"]:checked').length < 0) {
            //    infoAlert("Please select Card Type.", "rdioriya");
            //    return false;
            //}

            if ($("#ddl_paymenttype").val() == "B") {
                if ($("#ddl_PassthrougUATP").val() == "") {
                    infoAlert("Please enter UATP No.", "ddl_PassthrougUATP");
                    return false;
                }
            }
        }

        var strPlatingcarrier = $("#ddl_platingcarrier").val();
        strPlatingcarrier = ((strPlatingcarrier != null && strPlatingcarrier != undefined && strPlatingcarrier != "") ? strPlatingcarrier : "")
        $("#dvPaymentdata").hide();

        Strcardnumber = "";
        //$("#Loadpaymentpopup").modal("hide");

        var faretype = $("#ddl_Faretype").val();

        var cardGetSet = $('#ddl_paymenttype').val();

        var PAssthroughCardType = $("#ddl_paymenttype").val() == "B" ? $("#rdiocustomer")[0].checked ? "C" : "R" : "";
        var TotalGuestDetails = Guestmailid + "~" + GuestPhoneno;
        var PassthroughCardNo = $("#ddl_paymenttype").val() == "B" ? $("#ddl_PassthrougUATP").val() : "";
        var Com_Val = $("#ddl_PaymentREF").val() + "|" + $("#remarksretrivePNR").val() + "|" + PAssthroughCardType + "|" + PassthroughCardNo + "|" + TotalGuestDetails + "|" + $("#ddl_paymenttype").val();
        var Officeid = $("#ddl_OfficeID").val();
        boolresofficeid = false;

        hdf_pnrinfo = arg == 1 ? "1" : hdf_pnrinfo;


        //region for cash payment details validation and formation STS-195
        var strCashPaymentinfo = "";
        if (cardGetSet == "H") { //  && !RAT
            if ($("#txtcash_contactno").val() == null || $("#txtcash_contactno").val() == undefined || $("#txtcash_contactno").val() == "") {
                infoAlert("Please enter customer contact number.", "", "");
                return false;
            }
            if ($("#txtcash_email").val() == null || $("#txtcash_email").val() == "") {
                infoAlert("Please enter your email id.", "", "");
                return false;
            }
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if ($("#txtcash_email").val() != "" && !filter.test($("#txtcash_email").val())) {
                infoAlert("Please enter valid email id.", "", "");
                return false;
            }
            if ($("#txtcash_EmpCode").val() == null || $("#txtcash_EmpCode").val() == undefined || $("#txtcash_EmpCode").val() == "") {
                infoAlert("Please enter your employee code.", "", "");
                return false;
            }
            if ($("#hdncash_Empname").val() == "") {//$("#hdncash_Empbranch").val() == "" || $("#hdncash_Empemail").val() == "" |||| $("#hdncash_Empmobile").val() == ""
                infoAlert("Please enter valid employee id.", "", "");
                return false;
            }
            if ($("#txtcash_Paymentdate").val() == null || $("#txtcash_Paymentdate").val() == undefined || $("#txtcash_Paymentdate").val() == "") {
                infoAlert("Please select date of payment.", "", "");
                return false;
            }
            if ($("#txtcash_PaymentMethod").val() == null || $("#txtcash_PaymentMethod").val() == undefined || $("#txtcash_PaymentMethod").val() == "") {
                infoAlert("Please select payment method.", "", "");
                return false;
            }
            if (($("#txtcash_pancard").val() == null || $("#txtcash_pancard").val() == undefined || $("#txtcash_pancard").val() == "")
                && (Number($("#totalAmnt").data("totalamntd")) > 50000)) {
                infoAlert("Please enter customer contact number.", "", "");
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
        //region for cash payment details validation and formation STS-195

        var query = {
            hdf_pnrinfo: hdf_pnrinfo,
            CorporateCode: Corporatecode,
            CrsPnr: crspnr,
            CRS: crs,
            QueueingNumber: queuingNum,
            TSTCOUNT: tst_count,
            AirportTypes: AirportType,
            Corporatename: AgentID,
            Employeename: Employeename,
            Empmailname: Empmailname,
            EmpCostCenter: EmpCostCenter,
            EmpRefID: EmpRefID,
            BranchID: BranchID,
            SessionKey: $("#ddl_OfficeID").length > 0 ? $("#ddl_OfficeID").val() : $("body").data("sessionkey") != null ? $("body").data("sessionkey") : "",
            GSTDetails: GstDetails,
            Faretype: faretype,
            DiscountVal: discountvalue,
            Markupval: markupvalue,
            TotalGrossamt: $("body").data("totalgrossamt"),
            Newempflag: Newempflag,
            faretypereason: faretypereason,
            PaymentMode: $("#ddl_paymenttype").val(),
            CommonValue: Com_Val,
            OtherTicketInfo: OtherTicketInfo,
            PassportDetails: PassportDetails,
            StrFareFlag: StrFareFlag,
            Ticketing_PCC: $("#ddl_OfficeID").val(),
            strPlatingcarrier: strPlatingcarrier,
            strCashPaymentDet: strCashPaymentinfo,
            cardGetSet: cardGetSet,
            AirlineCategory: RATLCC ? "LCC" : "FSC",
            Tourcode: ($("#txttourcode").length ? $("#txttourcode").val() : ""),
            //PassportDetails: PassportDetails
        };

        $('#modal-AccountingPopup').iziModal('close');
        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loadurl + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });

        $.ajax({
            type: "POST",
            contentType: "Application/json; charset=utf-8",
            url: RAT == true && RATLCC == false ? QueueticketingCheckFareUrl : QueueticketingAccountingUrl,
            data: JSON.stringify(query),//'{hdf_pnrinfo:"' + hdf_pnrinfo + '",CorporateCode:"' + Corporatecode + '",CrsPnr:"' + crspnr + '",CRS:"' + crs + '",QueueingNumber:"' + queuingNum + '",TSTCOUNT:"' + tst_count + '"}',
            datatype: "json",
            success: function (data) {

                $.unblockUI();

                if (data.Status == "-1") {
                    infoAlert("Your session has expired!", "");
                    window.location.href = $('#dvDataProp').data('session-url');
                }

                if (data.Status == "1") {
                    if (!(RAT == true && RATLCC == false)) { // Accounting
                        Lobibox.alert('success', {
                            msg: data.Result,
                            closeonesc: false,
                            callback: function ($this, type) {
                                window.location.href = window.location.href;
                            }
                        });
                        return false;
                    }
                    if (data.Paymentdetails != null && data.Paymentdetails != "") {
                        loadPaymentDEtails(data.Paymentdetails);
                    }
                    else {
                        showerralert("Paymentmode not assigned. Please contact support team (#06).", "", "");
                        return false;
                    }
                    if (APP_Terminal == "T" && data.ApprovalDetails != null && data.ApprovalDetails != undefined && data.ApprovalDetails != "") {
                        ApprovalData = JSON.parse(data.ApprovalDetails);
                        var sb = "<option value=''>-- Select Approver --- </option>";
                        for (var _app = 0; _app < ApprovalData.length; _app++) {
                            sb += "<option value='" + ApprovalData[_app]["LGN_EMAILID"] + "'>" + (ApprovalData[_app]["LGN_EMPLOYEE_ID"] != null & ApprovalData[_app]["LGN_EMPLOYEE_ID"] != "" ? ApprovalData[_app]["LGN_EMPLOYEE_ID"] : ApprovalData[_app]["LGN_EMAILID"]) + "</option>";
                        }
                        $("#txt_ApprovedBY").html(sb);
                        //$('.Approvalclass').show();
                    }
                    else {
                        $('.Approvalclass').hide();
                    }
                }

                if (RAT != false && RATLCC == false) {
                    if (data.Result != null && data.Result != "") {
                        if (data.Result[0] != null && data.Result[0] != "") {
                            if (data.Result[0] == "") {
                                infoAlert("Unable to retrive Fare details. Please try later.", "");
                                return false;
                            }
                            else {
                                infoAlert(data.Result[0], "");
                                return false;
                            }
                        }
                        else {
                            var test = [];
                            test = jQuery.parseJSON(data.Result[5]);
                            var fare_info = test.FarePriceDetails.length;
                            //var Obcdetails = test.OBCDetails.length;
                            $("#spnSuppComm").html(test["rootNode"][0].SupplierCommission);
                            var str_build = '', RB_build = '';
                            var fare = "", farecolor = "";
                            var obcAMount = "0";
                            //STS-166
                            if (parseInt(fare_info) > 1) {
                                $('#popup_fare').addClass("clsfare");
                                var res = 100 / parseInt(fare_info);
                                var st1 = res + "%";
                            }
                            else {
                                var st1 = "100%";
                            }

                            $('#hdnOBCAmountTopup').val("0");
                            $('#hdnOBCAmount').val("0");

                            if (test.OBCDetails != null && test.OBCDetails != undefined && test.OBCDetails != "") {
                                for (var N = 0; test.OBCDetails.length > N; N++) {
                                    if (test.OBCDetails[N].CardMode == "O") {
                                        obcAMount = "0";
                                        obcAMount = test.OBCDetails[N].OBCAmount;
                                        $('#hdnOBCAmountTopup').val(obcAMount);
                                    }
                                    else {
                                        obcAMount = "0";
                                        obcAMount = test.OBCDetails[N].OBCAmount;
                                        $('#hdnOBCAmount').val(obcAMount);
                                        Strcardnumber = test.OBCDetails[N].CardType + "|" + test.OBCDetails[N].CardNumber;
                                    }
                                }
                            }

                            //if (parseInt(Obcdetails) > 0) {
                            //    obcAMount = test.OBCDetails[0].OBCAmount;
                            //    $('#hdnOBCAmountTopup').val(obcAMount);
                            //}
                            //STS-166
                            var arr = [];
                            for (var faredetails = 0; faredetails < fare_info; faredetails++) {
                                arr.push(test["FarePriceDetails"][faredetails].GROSSFARE);
                            }

                            var valSmall = Math.min.apply(null, arr); // 1
                            var adultcount = 0;
                            var childcount = 0;
                            var infantcount = 0;
                            adultcount = parseInt(data.Result[6]);
                            childcount = parseInt(data.Result[7]);
                            infantcount = parseInt(data.Result[8]);

                            var totalpax = parseInt(adultcount) + parseInt(childcount) + parseInt(infantcount);
                            var totalAMount = 0;

                            if (data.Result[9] != "") {
                                $('#hdn_pricingToken').val(data.Result[9]);
                            }

                            str_build += '<div class="col-lg-12 scroll-fix" style="justify-content: center;align-items: center;">'
                            for (var faredetails = 0; faredetails < fare_info; faredetails++) {

                                if (test["FarePriceDetails"][faredetails].FAREQUALIFIER == "N") {
                                    fare = "Normal Fare";
                                    farecolor = "style='background-color:#6b6464;'";
                                    totalAMount = parseInt(Number(test["FarePriceDetails"][faredetails].GROSSFARE) + (parseInt($('#hdnOBCAmountTopup').val()) * parseInt(totalpax)));//+ MarkupValue

                                    if (test["FarePriceDetails"][faredetails].GROSSFARE == valSmall) {
                                        $('#popupfare').html(test["FarePriceDetails"][faredetails].GROSSFARE);
                                        $('#spnPopupFare').html(totalAMount);
                                        $('#hdn_farequalifier').val(test["FarePriceDetails"][faredetails].FAREQUALIFIER);
                                        $('#hdn_TSTReference').val(test["FarePriceDetails"][faredetails].TSTReference);
                                        $('#hdnCorporateCode').val(test["FarePriceDetails"][faredetails].CORPORATECODE);
                                        str_build += '<div class="clsdiv col-md-6 col-sm-6" >';//width:' + st1 + '//clsSelected
                                        RB_build = '<div data-toggle="buttons"><label for="dv_normal' + faredetails + '" class="btn active"><input type="radio" name="farecheck" checked data-CorporateCode="' + test["FarePriceDetails"][faredetails].CORPORATECODE + '" data-farequalifier="' + test["FarePriceDetails"][faredetails].FAREQUALIFIER + '" data-TSTReference="' + test["FarePriceDetails"][faredetails].TSTReference + '"data-freetext="' + test["FarePriceDetails"][faredetails].FREETEXT + '" style="float:left;cursor:pointer;" id="dv_normal' + faredetails + '" onclick="ClickNormal(id);" />';
                                        RB_build += '<i for="dv_normal' + faredetails + '" class="fa fa-square-o fa-2x"></i><i for="dv_normal' + faredetails + '" class="fa fa-check-square-o fa-2x"></i></label></div>';

                                        $(".chkclass").removeClass('col-md-offset-5').addClass('col-md-offset-2');
                                        $(".tourclass").show();
                                    }
                                    else {
                                        str_build += '<div class="clsdiv col-md-6 col-sm-6" >';//width:' + st1 + '//clsunSelected
                                        RB_build = '<div data-toggle="buttons"><label for="dv_normal' + faredetails + '" class="btn"><input type="radio" name="farecheck" data-CorporateCode="' + test["FarePriceDetails"][faredetails].CORPORATECODE + '" data-farequalifier="' + test["FarePriceDetails"][faredetails].FAREQUALIFIER + '" data-TSTReference="' + test["FarePriceDetails"][faredetails].TSTReference + '" data-freetext="' + test["FarePriceDetails"][faredetails].FREETEXT + '" style="float:left;cursor:pointer;" id="dv_normal' + faredetails + '" onclick="ClickNormal(id);">';
                                        RB_build += '<i class="fa fa-square-o fa-2x"></i><i class="fa fa-check-square-o fa-2x"></i></label></div>';
                                    }
                                }
                                else if (test["FarePriceDetails"][faredetails].FAREQUALIFIER == "S") {
                                    fare = "Special Fare";
                                    farecolor = "style='background-color:lightcoral;'";
                                    totalAMount = parseInt(Number(test["FarePriceDetails"][faredetails].GROSSFARE) + (parseInt($('#hdnOBCAmountTopup').val()) * parseInt(totalpax)))

                                    if (test["FarePriceDetails"][faredetails].GROSSFARE == valSmall) {
                                        $('#popupfare').html(test["FarePriceDetails"][faredetails].GROSSFARE);
                                        $('#spnPopupFare').html(totalAMount);
                                        $('#hdn_farequalifier').val(test["FarePriceDetails"][faredetails].FAREQUALIFIER);
                                        $('#hdn_TSTReference').val(test["FarePriceDetails"][faredetails].TSTReference);
                                        $('#hdnCorporateCode').val(test["FarePriceDetails"][faredetails].CORPORATECODE);
                                        str_build += '<div class="clsdiv col-md-6 col-sm-6">';//width:' + st1 + '//clsSelected
                                        RB_build = '<div data-toggle="buttons"><label for="dv_normal' + faredetails + '" class="btn active"><input type="radio" name="farecheck" checked data-CorporateCode="' + test["FarePriceDetails"][faredetails].CORPORATECODE + '" data-farequalifier="' + test["FarePriceDetails"][faredetails].FAREQUALIFIER + '" data-TSTReference="' + test["FarePriceDetails"][faredetails].TSTReference + '" data-freetext="' + test["FarePriceDetails"][faredetails].FREETEXT + '" style="float:left;cursor:pointer;" id="dv_normal' + faredetails + '" onclick="ClickNormal(id);">';
                                        RB_build += '<i class="fa fa-square-o fa-2x"></i><i class="fa fa-check-square-o fa-2x"></i></label></div>';
                                    }
                                    else {
                                        str_build += '<div class="clsdiv col-md-6 col-sm-6" >';//width:' + st1 + '//clsunSelected
                                        RB_build = '<div data-toggle="buttons"><label for="dv_normal' + faredetails + '" class="btn"><input type="radio" name="farecheck" data-CorporateCode="' + test["FarePriceDetails"][faredetails].CORPORATECODE + '" data-farequalifier="' + test["FarePriceDetails"][faredetails].FAREQUALIFIER + '" data-TSTReference="' + test["FarePriceDetails"][faredetails].TSTReference + '" data-freetext="' + test["FarePriceDetails"][faredetails].FREETEXT + '" style="float:left;cursor:pointer;" id="dv_normal' + faredetails + '" onclick="ClickNormal(id);">';
                                        RB_build += '<i class="fa fa-square-o fa-2x"></i><i class="fa fa-check-square-o fa-2x"></i></label></div>';
                                    }
                                }
                                else {
                                    fare = "Special fare";
                                    farecolor = "style='background-color:lightcoral;'";
                                    totalAMount = parseInt(Number(test["FarePriceDetails"][faredetails].GROSSFARE) + (parseInt($('#hdnOBCAmountTopup').val()) * parseInt(totalpax)))

                                    if (test["FarePriceDetails"][faredetails].GROSSFARE == valSmall) {// if (faredetails == 0) {
                                        $('#popupfare').html(test["FarePriceDetails"][faredetails].GROSSFARE);
                                        $('#spnPopupFare').html(totalAMount);
                                        $('#hdn_farequalifier').val(test["FarePriceDetails"][faredetails].FAREQUALIFIER);
                                        $('#hdn_TSTReference').val(test["FarePriceDetails"][faredetails].TSTReference);
                                        $('#hdnCorporateCode').val(test["FarePriceDetails"][faredetails].CORPORATECODE);
                                        str_build += '<div class="clsdiv col-md-6 col-sm-6" >';//clsSelected

                                        RB_build = '<div data-toggle="buttons"><label for="dv_normal' + faredetails + '" class="btn active"><input type="radio" name="farecheck" checked data-CorporateCode="' + test["FarePriceDetails"][faredetails].CORPORATECODE + '" data-farequalifier="' + test["FarePriceDetails"][faredetails].FAREQUALIFIER + '" data-TSTReference="' + test["FarePriceDetails"][faredetails].TSTReference + '" data-freetext="' + test["FarePriceDetails"][faredetails].FREETEXT + '" style="float:left;cursor:pointer;width:' + st1 + '" id="dv_normal' + faredetails + '" onclick="ClickNormal(id);"/>';
                                        RB_build += '<i class="fa fa-square-o fa-2x"></i><i class="fa fa-check-square-o fa-2x"></i></label></div>';
                                    }
                                    else {
                                        str_build += '<div class="clsdiv col-md-6 col-sm-6" >';//clsunSelected

                                        RB_build = '<div data-toggle="buttons"><label for="dv_normal' + faredetails + '" class="btn"><input type="radio" name="farecheck" data-CorporateCode="' + test["FarePriceDetails"][faredetails].CORPORATECODE + '"  data-farequalifier="' + test["FarePriceDetails"][faredetails].FAREQUALIFIER + '" data-TSTReference="' + test["FarePriceDetails"][faredetails].TSTReference + '" data-freetext="' + test["FarePriceDetails"][faredetails].FREETEXT + '" style="float:left;cursor:pointer;width:' + st1 + '" id="dv_normal' + faredetails + '" onclick="ClickNormal(id);"/>';
                                        RB_build += '<i class="fa fa-square-o fa-2x"></i><i class="fa fa-check-square-o fa-2x"></i></label></div>';
                                    }
                                }

                                str_build += '<div class="width100" style="border:1px dashed lightgrey;Ticket Detailsmargin: 1% 2%;">';//1127 1px solid #000//height: 200px;
                                str_build += '<div class="clsdiv2" ' + farecolor + '>';
                                str_build += '<label style="cursor: pointer;font-weight: 600;font-size: 16px;padding: 1%;color:#FFF" >' + fare + '</label>';
                                str_build += '</div>';
                                if (test["FarePriceDetails"][faredetails].ADTGROSSFARE != "" && test["FarePriceDetails"][faredetails].ADTGROSSFARE > 0) {
                                    var totalAMountAdult = 0;
                                    totalAMountAdult = parseInt(Number(test["FarePriceDetails"][faredetails].ADTGROSSFARE) + (parseInt($('#hdnOBCAmountTopup').val())));

                                    str_build += '<div class="clsdiv2" style="font-size: 16px;">';
                                    str_build += '<table class="cltblfare"><tr><td class="clstdleft"><i class="fa fa-male" aria-hidden="true"></i> Adult Fare</td><td class="clstdrt">:</td><td class="clstdleft"><i class="fa fa-inr"></i> <span style="font-weight: bold;" id="lbl_normalAdult' + faredetails + '">' + totalAMountAdult + '</span></td></tr></table>';
                                    str_build += '</div>';

                                }

                                if (test["FarePriceDetails"][faredetails].CHDGROSSFARE != "" && parseInt(test["FarePriceDetails"][faredetails].CHDGROSSFARE) > 0) {
                                    var totalAMountChild = 0;
                                    totalAMountChild = parseInt(Number(test["FarePriceDetails"][faredetails].CHDGROSSFARE) + (parseInt($('#hdnOBCAmountTopup').val())));

                                    str_build += '<div class="clsdiv2" style="font-size: 16px;">';
                                    str_build += '<table class="cltblfare"><tr><td class="clstdleft"><i class="fa fa-child" aria-hidden="true"></i> Child Fare</td><td class="clstdrt">:</td><td class="clstdleft"><i class="fa fa-inr"></i> <span style="font-weight: bold;" id="lbl_normalChild' + faredetails + '">' + totalAMountChild + '</span> </td></tr></table>';
                                    str_build += '</div>';

                                }
                                if (test["FarePriceDetails"][faredetails].INFGROSSFARE != "" && parseInt(test["FarePriceDetails"][faredetails].INFGROSSFARE) > 0) {
                                    var totalAMountInfant = 0;
                                    totalAMountInfant = parseInt(Number(test["FarePriceDetails"][faredetails].INFGROSSFARE) + (parseInt($('#hdnOBCAmountTopup').val())));
                                    str_build += '<div class="clsdiv2" style="font-size: 16px;">';
                                    str_build += '<table class="cltblfare"><tr><td class="clstdleft"><i class="fa fa-child" aria-hidden="true"></i> Infant Fare</td><td class="clstdrt">:</td><td class="clstdleft"><i class="fa fa-inr"></i> <span style="font-weight: bold;" id="lbl_normalInfant' + faredetails + '">' + totalAMountInfant + '</span></td></tr></table>';
                                    str_build += '</div>';

                                }
                                str_build += '<div class="clsdiv2" style="font-size: 16px;">';
                                if (test["FarePriceDetails"][faredetails].GROSSFARE == valSmall) {
                                    str_build += '<table class="cltblfare"><tr><td class="clstdleft"><i class="fa fa-money" aria-hidden="true"></i> Total Fare</td><td class="clstdrt">:</td><td class="clstdleft"><i class="fa fa-inr"></i> <span style="font-size: 15px;font-weight: 600;" class="clsTotFare"  id="lbl_normal' + faredetails + '">' + totalAMount + '</span></td></tr></table>';
                                }
                                else {
                                    str_build += '<table class="cltblfare"><tr><td class="clstdleft"><i class="fa fa-money" aria-hidden="true"></i> Total Fare</td><td class="clstdrt">:</td><td class="clstdleft"><i class="fa fa-inr"></i> <span style="font-size: 15px;font-weight: 600;" class="clsTotFare"  id="lbl_normal' + faredetails + '">' + totalAMount + '</span></td></tr></table>';
                                }
                                str_build += '</div>';

                                if (test["FarePriceDetails"][faredetails].ADTALLOWBAGGAGE != "") {
                                    str_build += '<div class="clsdiv2">';
                                    str_build += '<table class="cltblfare"><tr><td class="clstdleft"><i class="fa fa-briefcase" aria-hidden="true"></i> Baggage</td><td class="clstdrt">:</td><td class="clstdleft"><span style="font-weight: bold;">' + test["FarePriceDetails"][faredetails].ADTALLOWBAGGAGE + '</span></td></tr></table>';
                                    str_build += '</div>';
                                }
                                if (test["FarePriceDetails"][faredetails].CHDALLOWBAGGAGE != "") {
                                    str_build += '<div class="clsdiv2">';
                                    str_build += '<table class="cltblfare"><tr><td class="clstdleft"><i class="fa fa-briefcase" aria-hidden="true"></i> Baggage</td><td class="clstdrt">:</td><td class="clstdleft"><span style="font-weight: bold;">' + test["FarePriceDetails"][faredetails].CHDALLOWBAGGAGE + '</span></td></tr></table>';
                                    str_build += '</div>';
                                }
                                if (test["FarePriceDetails"][faredetails].INFALLOWBAGGAGE != "") {
                                    str_build += '<div class="clsdiv2">';
                                    str_build += '<table class="cltblfare"><tr><td class="clstdleft"><i class="fa fa-briefcase" aria-hidden="true"></i> Baggage</td><td class="clstdrt">:</td><td class="clstdleft"><span style="font-weight: bold;">' + test["FarePriceDetails"][faredetails].INFALLOWBAGGAGE + '</span></td></tr></table>';
                                    str_build += '</div>';
                                }

                                str_build += '<div class="clsdiv2">';
                                str_build += '<table class="cltblfare"><tbody><tr><td class="clstdleft">Fare Basis Code</td>';
                                str_build += '<td class="clstdrt">:</td><td class="clstdleft"><span class="clsfarecode">' + test["FarePriceDetails"][faredetails].FAREBASISCODE + '</span></td></tr></tbody></table>';
                                str_build += '</div>';

                                str_build += '<div class="clsdiv2">';
                                if (test["FarePriceDetails"][faredetails].FREETEXT != "") {
                                    if (test["FarePriceDetails"][faredetails].GROSSFARE == valSmall) {
                                        str_build += '<a class="clsfare" id="anchor' + faredetails + '"  data-CorporateCode="' + test["FarePriceDetails"][faredetails].CORPORATECODE + '" data-farequalifier="' + test["FarePriceDetails"][faredetails].FAREQUALIFIER + '" data-TSTReference="' + test["FarePriceDetails"][faredetails].TSTReference + '" style="color:blue;font-weight:bold;cursor:pointer;" onclick="ShowFreetext(this);">Fare rule</a>';
                                    }
                                    else {
                                        str_build += '<a class="clsfare" id="anchor' + faredetails + '" data-CorporateCode="' + test["FarePriceDetails"][faredetails].CORPORATECODE + '" data-farequalifier="' + test["FarePriceDetails"][faredetails].FAREQUALIFIER + '" data-TSTReference="' + test["FarePriceDetails"][faredetails].TSTReference + '" style="color:blue;font-weight:bold;cursor:pointer;" onclick="ShowFreetext(this);">Fare rule</a>';// style="color: #d0091c; border: 2px solid #d0091c; padding: 3px 8px 2px 9px; border-radius: 5px; font-weight: bold;background-color: #fff;"
                                    }
                                }
                                str_build += RB_build;
                                str_build += '</div>';
                                str_build += ' </div>';
                                str_build += '</div>';
                            }

                            str_build += '</div>';

                            $('#dv_farDetails').html(str_build);

                            $("#hdf_fareinfo").val(data.Result[1]);

                            var diff = 0;
                            if (parseInt(data.Result[2]) != parseInt(data.Result[3])) {
                                diff = parseInt(data.Result[2]) - parseInt(data.Result[3]);
                            }

                            ShowBookdata();

                            //166
                            //CallFarepopup("Fare Details", "modal-FarePopup", "700");

                            faredetailsarray = test["FarePriceDetails"];
                            var newpaxstring = data.Result[10];
                            if (newpaxstring != "" && newpaxstring != null) {
                                selectedpaxdetails = JSON.parse(newpaxstring);
                            }

                            if (TerminalType == "T") {
                                $("#dvSelectedpax").show();
                            }
                            else {
                                $("#dvSelectedpax").hide();
                            }                          
                            boolresofficeid = data.Result[12];
                            $("#spnPNFOfficeID").html(((data.Result[13] != null && data.Result[13] != "") ? data.Result[13] : ""));
                            Selectedbreakup(valSmall);
                            //166
                            $("#dvshowmoreicn").show();
                            $(".clscheckfarehide").slideUp();

                            // $("#btn_getfare").hide();
                            // $("#txt_corporateCode").length ? $("#txt_corporateCode").hide() : "";
                            //$("#lbl_corcode").hide();
                        }
                    }
                    else {
                        $.unblockUI();
                        showerralert("No data found!", "", "");
                        return false;
                    }
                } else {
                    if (data.Status == "2") {

                        $('#div_reverce_content').html(data.Result);
                        $('#modal_confirm').modal({
                            backdrop: "static"
                        });
                    }
                    else {
                        if (data.Status == "0") {
                            showerralert(data.Message, "", "");
                            return false;
                        } else if (data.Status == "1") {
                            modalExp(data.Result, window.location.href);
                            return false;
                        } else {
                            modalExp(data.Result, window.location.href);
                            return false;
                        }
                    }
                }
                $.unblockUI();
            },
            error: function (e) {
                $.unblockUI();
                infoAlert('Inter server error!', "");
                return false;
            }
        });
        //******************************************************
    }
    catch (e) {
        infoAlert('Unable to Process Fare Details!', "(#06)");
        console.log(e.message);
    }
}

function Callpopup(Title, id, widthAS) {
    $('#' + id).iziModal('destroy');
    $("#" + id).iziModal({
        title: Title,
        iconClass: 'icon-stack',
        headerColor: '#bd5b5b',//'#27a5f5',
        width: widthAS,
        closeOnEscape: false,
        overlayClose: false,
        onClosing: function () {
            //alert("close");
        },
    });

    $('#' + id).iziModal('open', {
        transitionIn: 'bounceInDown',
        transitionOut: 'bounceOutDown' // TransitionOut will be applied if you have any open modal.
    });
}

function CallFarepopup(Title, id, widthAS) {
    // $('#' + id).iziModal('destroy');
    $("#" + id).iziModal({
        title: Title,
        //fullscreen: Ifullopt,
        iconClass: 'icon-stack',
        headerColor: '#b4002a',
        width: widthAS,
        closeOnEscape: false,
        overlayClose: false,
        onClosing: function () {
            hideFarepopup(1);
        },
        transitionIn: 'bounceInDown',
        transitionOut: 'bounceOutDown',
    });

    $('#' + id).iziModal('open', {
        transitionIn: 'bounceInDown',
        transitionOut: 'bounceOutDown' // TransitionOut will be applied if you have any open modal.
    });
}

function CallTicketingpopup(Title, id, widthAS) {
    // $('#' + id).iziModal('destroy');
    $("#" + id).iziModal({
        title: Title,
        //subtitle: Isubtitle,
        //fullscreen: Ifullopt,
        iconClass: 'icon-stack',
        headerColor: '#b4002a',
        width: widthAS,
        closeOnEscape: false,
        overlayClose: false,
        onClosing: function () {
            hidePopUp();
        },
        transitionIn: 'bounceInDown',
        transitionOut: 'bounceOutDown',

    });
    $('#' + id).iziModal('open', {
        transitionIn: 'bounceInDown',
        transitionOut: 'bounceOutDown' // TransitionOut will be applied if you have any open modal.
    });
}

function hidePopUp() {
    //CallFarepopup("Fare Details", "modal-FarePopup", "1068");
    //$("#dvBookdata").show();
    //$("#dvPaymentdata").hide();
    $('#ddlPaymentMode').val("0");
    clearcarddetails();
    $('#modal-FarePopup').iziModal('close');
}

function ConfirmTicketing() {
    var temp = "";
    var missing_title = "";
    var TST = "";
    $("#ddlPaymentMode").val("0");
    $("#ddlPaymentMode").attr('disabled', false);
    //(checkbox checked or not) and  getting checked fares
    var totalfare = 0;
    var totalfares = 0;
    var noOfCheckBox = document.getElementById('table1').getElementsByTagName('input');
    var status = false;
    for (var j = 0; j < noOfCheckBox.length; j++) {
        if (noOfCheckBox[j].checked) {
            status = true;
            totalfare = totalfare + +noOfCheckBox[j].value;
            TST = noOfCheckBox[j].id.replace('tst', '');

            var paxcount = $('#' + TST + 'hdf_paxcount').val();
            var adult_count = paxcount.split('|')[0];
            var child_count = paxcount.split('|')[1];
            var infant_count = paxcount.split('|')[2];

            if ($('#' + TST + 'title_adult1').length) {
                for (var i = 1; i <= adult_count; i++) {
                    if ($('#' + TST + 'title_adult' + i).length) missing_title += $('#' + TST + 'title_adult' + i).val() + ",";
                    else break;
                }
            }

            if ($('#' + TST + 'title_child1').length) {
                for (var i = 1; i <= child_count; i++) {
                    if ($('#' + TST + 'title_child' + i).length) missing_title += $('#' + TST + 'title_child' + i).val() + ",";
                    else break;
                }
            }

            if ($('#' + TST + 'title_infant1').length) {
                for (var i = 1; i <= infant_count; i++) {
                    if ($('#' + TST + 'title_infant' + i).length) missing_title += $('#' + TST + 'title_infant' + i).val() + ",";
                    else break;
                }
            }
        }
    }

    if (status == false) {
        showerralert("Select atleast one TST for ticketing!", "", "");
        return false;
    }

    if (missing_title.indexOf("Title") > -1) {
        showerralert("Please select Title", "", "");
        return false;
    }

    //$('#modal-FarePopup').iziModal('close');
    $("#dvinnerFaRe").html("");
    $("#btn_getfare").hide();
    //$("#dvBookdata").hide();
    $("#dvPaymentdata").show();
    $("#dvcashpayment,#dvEmpDetails").hide();
    $(".clsClear").val("");
    $("#dvEmployee").hide();
    $("#hdncash_Empname").val("");
    $("#hdncash_Empemail").val("");
    $("#hdncash_Empmobile").val("");
    $("#hdncash_Empbranch").val("");
    CallFarepopup("Payment Details", "modal-FarePopup", "700");
    //CallTicketingpopup("Ticketing Details", "modal-TicketingPopup", "1068");//sts-166
    $("#ddlPaymentMode").val("0");
    $("#txtar_remarks").val("");
    $('#btnTicketingPopUp').focus();
}

var OBCAmount = "0"
var ReBookingFare = "0";

function Ticketing(arg) {
    try {
        var cardGetSet = $('#ddlPaymentMode').val();
        var PassthroughValue = "";
        if (cardGetSet == "0") {
            infoAlert("Please select payment mode to do ticketing", "");
            $('#ddlPaymentMode').focus();
            return false;
        }

        if (cardGetSet == "B") {
            if ($("#txtar_CardType").val() == "") {
                showerralert("Please select Card.", "", "");
                $("#txtar_CardType").focus();
                return false;
            }
            else if ($("#txtar_cardNo").val() == "") {
                showerralert("Please Enter Card No.", "", "");
                $("#txtar_cardNo").focus();
                $(".clsbookingbtn").prop("disabled", false);
                return false;
            }
            else if ($("#ddlMonth").val() == "") {
                showerralert("Please Select Month.", "", "");
                $("#ddlMonth").focus();
                $(".clsbookingbtn").prop("disabled", false);
                return false;
            }
            else if ($("#ddlyear").val() == "" || $("#ddlyear").val() == "0") {
                showerralert("Please Select Year.", "", "");
                $("#ddlyear").focus();
                $(".clsbookingbtn").prop("disabled", false);
                return false;
            }
        }

        //region for cash payment details validation and formation STS-195
        var strCashPaymentinfo = "";
        if (cardGetSet == "H") {
            if ($("#txtcash_contactno").val() == null || $("#txtcash_contactno").val() == undefined || $("#txtcash_contactno").val() == "") {
                infoAlert("Please enter customer contact number.", "", "");
                return false;
            }
            if ($("#txtcash_email").val() == null || $("#txtcash_email").val() == "") {
                infoAlert("Please enter your email id.", "", "");
                return false;
            }
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if ($("#txtcash_email").val() != "" && !filter.test($("#txtcash_email").val())) {
                infoAlert("Please enter valid email id.", "", "");
                return false;
            }
            if ($("#txtcash_EmpCode").val() == null || $("#txtcash_EmpCode").val() == undefined || $("#txtcash_EmpCode").val() == "") {
                infoAlert("Please enter your employee code.", "", "");
                return false;
            }
            if ($("#hdncash_Empname").val() == "") {//$("#hdncash_Empbranch").val() == "" || $("#hdncash_Empemail").val() == "" |||| $("#hdncash_Empmobile").val() == ""
                infoAlert("Please enter valid employee id.", "", "");
                return false;
            }
            if ($("#txtcash_Paymentdate").val() == null || $("#txtcash_Paymentdate").val() == undefined || $("#txtcash_Paymentdate").val() == "") {
                infoAlert("Please select date of payment.", "", "");
                return false;
            }
            if ($("#txtcash_PaymentMethod").val() == null || $("#txtcash_PaymentMethod").val() == undefined || $("#txtcash_PaymentMethod").val() == "") {
                infoAlert("Please select payment method.", "", "");
                return false;
            }
            if (($("#txtcash_pancard").val() == null || $("#txtcash_pancard").val() == undefined || $("#txtcash_pancard").val() == "")
                && (Number($("#totalAmnt").data("totalamntd")) > 50000)) {
                infoAlert("Please enter customer contact number.", "", "");
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
        //region for cash payment details validation and formation STS-195

        var TST = "";
        var missing_title = "";
        var noOfCheckBox = document.getElementById('table1').getElementsByTagName('input');
        var status = false;
        for (var j = 0; j < noOfCheckBox.length; j++) {
            if (noOfCheckBox[j].checked) {
                TST = noOfCheckBox[j].id.replace('tst', '');

                var paxcount = $('#' + TST + 'hdf_paxcount').val();
                var adult_count = paxcount.split('|')[0];
                var child_count = paxcount.split('|')[1];
                var infant_count = paxcount.split('|')[2];

                if ($('#' + TST + 'title_adult1').length) {
                    for (var i = 1; i <= adult_count; i++) {
                        if ($('#' + TST + 'title_adult' + i).length) missing_title += $('#' + TST + 'title_adult' + i).val() + ",";
                        else break;
                    }
                }
                if ($('#' + TST + 'title_child1').length) {
                    for (var i = 1; i <= child_count; i++) {
                        if ($('#' + TST + 'title_child' + i).length) missing_title += $('#' + TST + 'title_child' + i).val() + ",";
                        else break;
                    }
                }
                if ($('#' + TST + 'title_infant1').length) {
                    for (var i = 1; i <= infant_count; i++) {
                        if ($('#' + TST + 'title_infant' + i).length) missing_title += $('#' + TST + 'title_infant' + i).val() + ",";
                        else break;
                    }
                }
            }
        }

        if (missing_title.indexOf("Title") > -1) {
            showerralert("Please select Title", "", "");
            return false;
        }

        var hdf_pnrinfo = $('#hdf_pnrinfo').val();
        var hdf_fareinfo = $('#hdf_fareinfo').val();

        if (cardGetSet == "B") {
            var CardTYPEs = $("#txtar_CardType").val().split('(')[1].split(')')[0];
            PassthroughValue = CardTYPEs + "|" + $("#txtar_CardType").val().split('(')[0] + "|" + $("#txtar_cardNo").val().replace(/ /gi, "") + "|" + $("#txtar_cvv").val().trim() + "|" + $("#ddlMonth").val() + $("#ddlyear").val().slice(-2);
        } else {
            PassthroughValue = "";
        }

        var BookingAmount = $('#popupfare').html();
        var ObcAmount = "0";
        var farequalif = $('#hdn_farequalifier').val();
        var fareToken = $('#hdn_pricingToken').val();
        var tstReference = $('#hdn_TSTReference').val();
        var AirportType = $("#ddl_QueueAT").val().trim();
        var Corporatename = $("#ddlclient").val();
        var Employeename = "";
        var EmpCostCenter = "";
        var EmpRefID = "";
        var Newempflag = "";
        var AgentID = $('#hdnClientID').val();
        var BranchID = $('#hdnBranchID').val();
        var GstDetails = "";
        var GSTstate = "";
        var GSTNumber = "";
        var GSTCompany = "";
        var GSTAddress = "";
        var GSTemailid = "";
        var GSTmobilenumber = "";
        var GSTStatecode = "";
        var GSTCityCode = "";
        var GSTPincode = "";
        var email = $('#txtgstEmailid');
        var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

        if (GST == "Y") {

            GSTstate = $("#ddlgststate").val();
            GSTNumber = $("#txtgstNumber").val();
            if (GSTNumber != "") {
                GSTemailid = $("#txtgstEmailid").val();
            }
            else if (GSTNumber == "") {
                GSTemailid = "";
            }
            if (email.val() != "" && GSTNumber != "") {
                if (filter.test(email.val()) == false) {
                    alert("Please provide a valid email address.");
                    email.focus();
                    return false;
                }
            }

            GSTNumber = $("#txtgstNumber").val();
            GSTCompany = $("#txtgstCompanyname").val();
            GSTAddress = $("#txtgstAddress").val();
            GSTmobilenumber = $("#txtgstMobileNumber").val();
            GSTStatecode = $("#txtgstStatecode").val();
            GSTCityCode = $("#txtgstCitycode").val();
            GSTPincode = $("#txtgstpinecode").val();
        }
        var Employee_Contact_No = $("body").data("employee_contact_no");
        var CRSTYPE = "";

        if ($("#rdoaircat_FSC")[0].checked) {
            CRSTYPE = $("#ddl_QueueStock").val().trim();
        }
        else {
            CRSTYPE = $("#ddl_AirlineStock").val().split('|')[0];
        }
        //if (gstflag = "0") {
        if (CRSTYPE == "1A") {
            GstDetails = GSTstate + '|' + GSTNumber + '|' + GSTCompany + '|' + GSTAddress + '|' + GSTemailid + '|' + GSTmobilenumber;
        }
        else {
            GstDetails = GSTstate + '|' + GSTNumber + '|' + GSTCompany + '|' + GSTAddress + '|' + GSTemailid + '|' + GSTmobilenumber + '|' + GSTStatecode + '|' + GSTCityCode + '|' + GSTPincode;
        }
        //}
        //else {
        //    GstDetails = "";
        //}
        var PaxType = ""; var PaxViseMeals = "";
        var PaxTitle = "";
        var PaxFirstName = "";
        var PaxLastName = "";
        var PaxGender = "";
        var PaxDOB = "";
        var PassportNo = "";
        var Passexpdate = "";
        var Ppissuedcontry = "";
        var Markupval = "";
        var Commissionval = "";
        var PLBval = "";
        var TotalEarnings = "";
        var AdultEarning = "";
        var ChildEarning = "";
        var InfantEarning = "";
        var totalPaxdetails = "";

        var comtype = "";
        var compert = "";
        var comamnt = "";
        var newcomamnt = "";
        var newplbamnt = "";
        var newmarkamnt = "";
        var newservicefeeamnt = "";
        var newincentiveamnt = "";
        var plbtype = "";
        var plbpert = "";
        var plbamnt = "";
        var marktype = "";
        var markamnt = "";
        var servicefeeamnt = "";
        var incentiveamnt = "";
        var passengernumber = "";

        var Adult = "";
        var Child = "";
        var Infant = "";

        var Adultsplit = "";
        var childsplit = "";

        var noOfCheckBox = document.getElementById('table1').getElementsByTagName('input');
        var strApprovalPerson = "";
        var showflag = false;
        $('.clsApproval').each(function () {
            if (Number($(this).val()) != Number($(this).data('oldvalue')) && APP_Terminal == "T") {
                showflag = true;
            }
        });
        if (showflag && ApprovalData.length == 0 && APP_Terminal == "T") {
            alert("Approval person not available please contact support team.");
            return false;
        }

        for (var j = 0; j < noOfCheckBox.length; j++) {
            if (noOfCheckBox[j].checked == true) {
                status = true;

                TST = noOfCheckBox[j].id.replace('tst', '');
                if (APP_Terminal == "T") {
                    strApprovalPerson = $("#txt_ApprovedBY").val();
                }
                var paxcount = $('#' + TST + 'hdf_paxcount').val();
                var adtcnt = paxcount.split('|')[0];
                var chdcnt = paxcount.split('|')[1];
                var infcnt = paxcount.split('|')[2];

                for (var i = 1; i <= adtcnt ; i++) {
                    if (AirportType == "I") {
                        if ($('#' + TST + 'Gender_adult' + i).length) {
                            PaxGender = $('#' + TST + 'Gender_adult' + i).val();
                        }
                        if ($('#' + TST + 'Passwort_adult' + i).length) {
                            PassportNo = $('#' + TST + 'Passwort_adult' + i).val();
                        }
                        if ($('#' + TST + 'IssuedCountry' + i).length) {
                            Passexpdate = $('#' + TST + 'IssuedCountry' + i).val();
                        }
                        if ($('#' + TST + 'ExpiryDate' + i).length) {
                            Ppissuedcontry = $('#' + TST + 'ExpiryDate' + i).val();
                        }
                        if ($('#' + TST + 'ADultDOB' + i).length) {
                            PaxDOB = $('#' + TST + 'ADultDOB' + i).val();
                        }
                    }

                    //adtcnt += PaxType + "|" + PaxTitle + "|" + PaxFirstName + "|" + PaxLastName + "|" + PaxGender + "|" + PaxDOB + "|" + PassportNo + "|" + Passexpdate + "|" + Ppissuedcontry + "|";//+ "#" + PaxViseMeals + "_";
                    Adult += PassportNo + "|" + Passexpdate + "|" + Ppissuedcontry + "|" + PaxDOB + "|" + PaxGender + "_";
                    Adultsplit += "_";

                    passengernumber = $('#txt_breakup_' + TST + '_ADT_' + i).data('paxno');
                    comtype = $('#ddl_Comm_' + TST + '_ADT_' + i).val();
                    compert = $('#txt_Comm_' + TST + '_ADT_' + i).val();
                    comamnt = $('#amt_Comm_' + TST + '_ADT_' + i).data('comm');
                    newcomamnt = $('#amt_Comm_' + TST + '_ADT_' + i).val();
                    plbtype = $('#ddl_plb_' + TST + '_ADT_' + i).val();
                    plbpert = $('#txt_plb_' + TST + '_ADT_' + i).val();
                    plbamnt = $('#amt_plb_' + TST + '_ADT_' + i).data('plb');
                    newplbamnt = $('#amt_plb_' + TST + '_ADT_' + i).val();
                    marktype = $('#ddl_mark_' + TST + '_ADT_' + i).val();
                    markamnt = $('#amt_mark_' + TST + '_ADT_' + i).data('markup');
                    newmarkamnt = $('#amt_mark_' + TST + '_ADT_' + i).val();
                    servicefeeamnt = $('#txt_sf_' + TST + '_ADT_' + i).data('servicefee');
                    newservicefeeamnt = $('#txt_sf_' + TST + '_ADT_' + i).val();
                    incentiveamnt = $('#txt_inc_' + TST + '_ADT_' + i).data('incentives');
                    newincentiveamnt = $('#txt_inc_' + TST + '_ADT_' + i).val();
                    if ((Number(comamnt) < Number(newcomamnt) || Number(plbamnt) < Number(newplbamnt) || Number(incentiveamnt) < Number(newincentiveamnt)) && (strApprovalPerson == "") && (APP_Terminal == "T")) {
                        alert("please select approval person");
                        $(".Approvalclass").show();
                        $(".chkclass").removeClass('col-md-offset-2')
                    }
                    else if ((Number(comamnt) > Number(newcomamnt) || Number(plbamnt) > Number(newplbamnt) || Number(incentiveamnt) > Number(newincentiveamnt)) && (strApprovalPerson == "") && (APP_Terminal == "T")) {
                        strApprovalPerson = ApprovalData[0]["LGN_EMAILID"];
                        $(".Approvalclass").hide();
                        $(".chkclass").addClass('col-md-offset-2')
                    }
                    else {
                        strApprovalPerson = "";
                        $(".Approvalclass").hide();
                        $(".chkclass").addClass('col-md-offset-2')
                    }
                    AdultEarning += passengernumber + "|" + comtype + "|" + compert + "|" + comamnt + "|" + plbtype + "|" + plbpert + "|" + plbamnt + "|" + marktype + "|" + markamnt + "|" + servicefeeamnt + "|" + incentiveamnt + "|" + newcomamnt + "|" + newplbamnt + "|" + newmarkamnt + "|" + newservicefeeamnt + "|" + newincentiveamnt + "|" + strApprovalPerson + "~";
                }

                for (var i = 1; i <= chdcnt ; i++) {

                    if (AirportType == "I") {
                        if ($('#' + TST + 'Gender_Child' + i).length) {
                            PaxGender = $('#' + TST + 'Gender_Child' + i).val();
                        }
                        if ($('#' + TST + 'Passwort_child' + i).length) {
                            PassportNo = $('#' + TST + 'Passwort_child' + i).val();
                        }
                        if ($('#' + TST + 'childIssuedCountry' + i).length) {
                            Passexpdate = $('#' + TST + 'childIssuedCountry' + i).val();
                        }
                        if ($('#' + TST + 'childExpiryDate' + i).length) {
                            Ppissuedcontry = $('#' + TST + 'childExpiryDate' + i).val();
                        }
                        if ($('#' + TST + 'ChildDOB' + i).length) {
                            PaxDOB = $('#' + TST + 'ChildDOB' + i).val();
                        }
                    }

                    //chdcnt += PaxType + "|" + PaxTitle + "|" + PaxFirstName + "|" + PaxLastName + "|" + PaxGender + "|" + PaxDOB + "|" + PassportNo + "|" + Passexpdate + "|" + Ppissuedcontry + "|";// + "#" + PaxViseMeals + "_";
                    Child += PassportNo + "|" + Passexpdate + "|" + Ppissuedcontry + "|" + PaxDOB + "|" + PaxGender + "_";
                    childsplit += "_";

                    passengernumber = $('#txt_breakup_' + TST + '_CHD_' + i).data('paxno');
                    comtype = $('#ddl_Comm_' + TST + '_CHD_' + i).val();
                    compert = $('#txt_Comm_' + TST + '_CHD_' + i).val();
                    comamnt = $('#amt_Comm_' + TST + '_CHD_' + i).data('comm');
                    newcomamnt = $('#amt_Comm_' + TST + '_CHD_' + i).val();
                    plbtype = $('#ddl_plb_' + TST + '_CHD_' + i).val();
                    plbpert = $('#txt_plb_' + TST + '_CHD_' + i).val();
                    plbamnt = $('#amt_plb_' + TST + '_CHD_' + i).data('plb');
                    newplbamnt = $('#amt_plb_' + TST + '_CHD_' + i).val();
                    marktype = $('#ddl_mark_' + TST + '_CHD_' + i).val();
                    markamnt = $('#amt_mark_' + TST + '_CHD_' + i).data('markup');
                    newmarkamnt = $('#amt_mark_' + TST + '_CHD_' + i).val();
                    servicefeeamnt = $('#txt_sf_' + TST + '_CHD_' + i).data('servicefee');
                    newservicefeeamnt = $('#txt_sf_' + TST + '_CHD_' + i).val();
                    incentiveamnt = $('#txt_inc_' + TST + '_CHD_' + i).data('incentives');
                    newincentiveamnt = $('#txt_inc_' + TST + '_CHD_' + i).val();
                    if ((Number(comamnt) < Number(newcomamnt) || Number(plbamnt) < Number(newplbamnt) || Number(incentiveamnt) < Number(newincentiveamnt)) && (strApprovalPerson == "") && (APP_Terminal == "T")) {
                        alert("please select approval person");
                        $(".Approvalclass").show();
                        $(".chkclass").removeClass('col-md-offset-2')
                    }
                    else if ((Number(comamnt) > Number(newcomamnt) || Number(plbamnt) > Number(newplbamnt) || Number(incentiveamnt) > Number(newincentiveamnt)) && (strApprovalPerson == "") && (APP_Terminal == "T")) {
                        strApprovalPerson = ApprovalData[0]["LGN_EMAILID"];
                        $(".Approvalclass").hide();
                        $(".chkclass").addClass('col-md-offset-2')
                    }
                    else {
                        strApprovalPerson = "";
                        $(".Approvalclass").hide();
                        $(".chkclass").addClass('col-md-offset-2')
                    }
                    ChildEarning += passengernumber + "|" + comtype + "|" + compert + "|" + comamnt + "|" + plbtype + "|" + plbpert + "|" + plbamnt + "|" + marktype + "|" + markamnt + "|" + servicefeeamnt + "|" + incentiveamnt + "|" + newcomamnt + "|" + newplbamnt + "|" + newmarkamnt + "|" + newservicefeeamnt + "|" + newincentiveamnt + "|" + strApprovalPerson + "~";
                }


                for (var i = 1; i <= infcnt ; i++) {

                    if (AirportType == "I") {
                        if ($('#' + TST + 'Gender_Infant' + i).length) {
                            PaxGender = $('#' + TST + 'Gender_Infant' + i).val();
                        }
                        if ($('#' + TST + 'Passwort_infant' + i).length) {
                            PassportNo = $('#' + TST + 'Passwort_infant' + i).val();
                        }
                        if ($('#' + TST + 'infantIssuedCountry' + i).length) {
                            Passexpdate = $('#' + TST + 'infantIssuedCountry' + i).val();
                        }
                        if ($('#' + TST + 'infantExpiryDate' + i).length) {
                            Ppissuedcontry = $('#' + TST + 'infantExpiryDate' + i).val();
                        }
                        if ($('#' + TST + 'InfantDOB' + i).length) {
                            PaxDOB = $('#' + TST + 'InfantDOB' + i).val();
                        }
                    }
                    Infant += PassportNo + "|" + Passexpdate + "|" + Ppissuedcontry + "|" + PaxDOB + "|" + PaxGender + "_";

                    passengernumber = $('#txt_breakup_' + TST + '_INF_' + i).data('paxno');
                    comtype = $('#ddl_Comm_' + TST + '_INF_' + i).val();
                    compert = $('#txt_Comm_' + TST + '_INF_' + i).val();
                    comamnt = $('#amt_Comm_' + TST + '_INF_' + i).data('comm');
                    newcomamnt = $('#amt_Comm_' + TST + '_INF_' + i).val();
                    plbtype = $('#ddl_plb_' + TST + '_INF_' + i).val();
                    plbpert = $('#txt_plb_' + TST + '_INF_' + i).val();
                    plbamnt = $('#amt_plb_' + TST + '_INF_' + i).data('plb');
                    newplbamnt = $('#amt_plb_' + TST + '_INF_' + i).val();
                    marktype = $('#ddl_mark_' + TST + '_INF_' + i).val();
                    markamnt = $('#amt_mark_' + TST + '_INF_' + i).data('markup');
                    newmarkamnt = $('#amt_mark_' + TST + '_INF_' + i).val();
                    servicefeeamnt = $('#txt_sf_' + TST + '_INF_' + i).data('servicefee');
                    newservicefeeamnt = $('#txt_sf_' + TST + '_INF_' + i).val();
                    incentiveamnt = $('#txt_inc_' + TST + '_INF_' + i).data('incentives');
                    newincentiveamnt = $('#txt_inc_' + TST + '_INF_' + i).val();
                    if ((Number(comamnt) < Number(newcomamnt) || Number(plbamnt) < Number(newplbamnt) || Number(incentiveamnt) < Number(newincentiveamnt)) && (strApprovalPerson == "") && (APP_Terminal == "T")) {
                        alert("please select approval person");
                        $(".Approvalclass").show();
                        $(".chkclass").removeClass('col-md-offset-2')
                    }
                    else if ((Number(comamnt) > Number(newcomamnt) || Number(plbamnt) > Number(newplbamnt) || Number(incentiveamnt) > Number(newincentiveamnt)) && (strApprovalPerson == "") && (APP_Terminal == "T")) {
                        strApprovalPerson = ApprovalData[0]["LGN_EMAILID"];
                        $(".Approvalclass").hide();
                        $(".chkclass").addClass('col-md-offset-2')
                    }
                    else {
                        strApprovalPerson = "";
                        $(".Approvalclass").hide();
                        $(".chkclass").addClass('col-md-offset-2')
                    }
                    InfantEarning += passengernumber + "|" + comtype + "|" + compert + "|" + comamnt + "|" + plbtype + "|" + plbpert + "|" + plbamnt + "|" + marktype + "|" + markamnt + "|" + servicefeeamnt + "|" + incentiveamnt + "|" + newcomamnt + "|" + newplbamnt + "|" + newmarkamnt + "|" + newservicefeeamnt + "|" + newincentiveamnt + "|" + strApprovalPerson + "~";
                }
            }
            var cuntdatadetails = "";
            var ss = 0;
            var data3 = $("body").data("arrcount");
            var PlatingCarriers = $("body").data("Platingcarrier");
            TotalCount = parseInt(adtcnt) + parseInt(chdcnt) + parseInt(infcnt);

            TotalEarnings = AdultEarning + ChildEarning + InfantEarning;

            var Num_FLT = PlatingCarriers.split('*');
            var ffnNew = $.grep(Num_FLT, function (n, i) {
                return n.split('~')[3] == TST
            });
            for (var i = 0; ffnNew.length > i; i++) {
                var arrffnair = $("#" + TST + "FFNAirline_" + i + ffnNew[i].split("~")[0]).length ? $("#" + TST + "FFNAirline_" + i + ffnNew[i].split("~")[0]).val() : "";
                var arrffnnumber = $("#" + TST + "FFNNUMBER_" + i + ffnNew[i].split("~")[0]).length ? $("#" + TST + "FFNNUMBER_" + i + ffnNew[i].split("~")[0]).val() : "";
                var FFNPaxRefNo = $("#" + TST + "FFNNUMBER_" + i + ffnNew[i].split("~")[0]).length ? $("#" + TST + "FFNNUMBER_" + i + ffnNew[i].split("~")[0]).data("paxref") : "";
                //var FFNsegrefNo = $("#FFNNUMBER_" + i).length ? $("#FFNNUMBER_" + i).data("segref") : "";
                //var FFNItinrefNo = $("#FFNNUMBER_" + i).length ? $("#FFNNUMBER_" + i).data("itinref") : "";
                if (arrffnnumber != "" && arrffnnumber != null && arrffnnumber != "undefined") {
                    ss++;
                    cuntdatadetails += arrffnair + "*" + arrffnnumber + "*" + FFNPaxRefNo + "~"
                }
            }

            if (ss != 0) {
                $("body").data("arrffnairlinedetails", cuntdatadetails);
            }
            else {
                $("body").data("arrffnairlinedetails", "");
            }
        }

        //if (cardGetSet == "B") 
        //{
        //    ObcAmount = $("#hdnOBCAmount").val();
        //}

        if (cardGetSet == "B") {
            var strcardtype = $("#txtar_CardType").val().split('(')[1].split(')')[0];
            if ((strcardtype.toUpperCase().trim() == Strcardnumber.split('|')[0].toUpperCase().trim()) && ($("#txtar_cardNo").val().trim() == Strcardnumber.split('|')[1].trim())) {
                ObcAmount = $("#hdnOBCAmount").val();
            }
        }

        else if (cardGetSet == "T") {
            ObcAmount = $('#hdnOBCAmountTopup').val();
        }
        ObcAmount = ObcAmount == "" ? "0" : ObcAmount;

        var WithoutTaxBookingAMount = parseInt(parseInt(BookingAmount) + (parseInt(TotalCount) * parseInt(ObcAmount)));
        var StrFareFlag = $("body").data("StrFareFlag");

        PassportDetails = Adult + "~" + Adultsplit + Child + "~" + Adultsplit + childsplit + Infant;

        //$('#modal-TicketingPopup').iziModal('close');
        $('#modal-FarePopup').iziModal('close');
        //document.getElementById('ddlPaymentMode').disabled = true;

        var faretypereason = "";
        var OtherTicketInfo = "";
        var erromanflag = false;
        var errorval = "";
        var checkCondition_BUDGET,
            checkCondition_TRAVEL_REQUEST,
            checkCondition_Sub_Reason,
            checkCondition_Recharge,
            checkCondition_Reason_of_travel,
            checkCondition_CostcenterID,
            checkCondition_Job_number,
            checkCondition_Package_id = "";

        $(".slttxtfrereason").each(function () {
            if ($(this).data("mandatoryflag") == "Y") {
                if ($(this).val() == "") {
                    erromanflag = true;
                    errorval = $(this).data("idname");
                    return false; //This line is coming out from the each function  by vijai 23032018
                }
            }
            if ($(this).data("idname").toUpperCase().indexOf("BUDGET CODE") != -1)
                checkCondition_BUDGET = $(this).data("idname").toUpperCase().indexOf("BUDGET CODE") != -1 ? $(this).val() : "";
            else if ($(this).data("idname").toUpperCase().indexOf("TRAVEL REQUEST NO.") != -1)
                checkCondition_TRAVEL_REQUEST = $(this).data("idname").toUpperCase().indexOf("TRAVEL REQUEST NO.") != -1 ? $(this).val() : "";
            else if ($(this).data("idname").toUpperCase().indexOf("SUB REASON") != -1)
                checkCondition_Sub_Reason = $(this).data("idname").toUpperCase().indexOf("SUB REASON") != -1 ? $(this).val() : "";
            else if ($(this).data("idname").toUpperCase().indexOf("RECHARGE") != -1)
                checkCondition_Recharge = $(this).data("idname").toUpperCase().indexOf("RECHARGE") != -1 ? $(this).val() : "";
            else if ($(this).data("idname").toUpperCase().indexOf("REASON OF TRAVEL") != -1)
                checkCondition_Reason_of_travel = $(this).data("idname").toUpperCase().indexOf("REASON OF TRAVEL") != -1 ? $(this).val() : "";
            else if (true)
                checkCondition_CostcenterID = "";
            else if (true)
                checkCondition_Job_number = "";
            else if (true)
                checkCondition_Package_id = "";

            faretypereason += $(this).data("dataid") + "|" + $(this).data("idname") + "|" + $(this).val() + "~";

        });
        var adtdisc = 0, chddisc = 0, infdisc = 0, adtmark = 0, chdmark = 0, infmark = 0, adtplb = 0, chdplb = 0, infplb = 0, paxtype = "";

        //STS-166
        $(".clsgrey .disvalue").each(function () {
            paxtype = $(this).data("paxtype")
            if (paxtype == "ADT") {
                adtdisc += $(this).val() != "" ? Number($(this).val()) : 0;
            }
            else if (paxtype == "CHD") {
                chddisc += $(this).val() != "" ? Number($(this).val()) : 0;
            }
            else {
                infdisc += $(this).val() != "" ? Number($(this).val()) : 0;
            }
        });
        var CommissionValue = adtdisc + "|" + chddisc + "|" + infdisc;

        $(".clsgrey .markvalue").each(function () {
            paxtype = $(this).data("paxtype")
            if (paxtype == "ADT") {
                adtmark += $(this).val() != "" ? Number($(this).val()) : 0;
            }
            else if (paxtype == "CHD") {
                chdmark += $(this).val() != "" ? Number($(this).val()) : 0;
            }
            else {
                infmark += $(this).val() != "" ? Number($(this).val()) : 0;
            }
        });
        var MarkupValue = adtmark + "|" + chdmark + "|" + infmark;

        $(".clsgrey .plbvalue").each(function () {
            paxtype = $(this).data("paxtype")
            if (paxtype == "ADT") {
                adtplb += $(this).val() != "" ? Number($(this).val()) : 0;
            }
            else if (paxtype == "CHD") {
                chdplb += $(this).val() != "" ? Number($(this).val()) : 0;
            }
            else {
                infplb += $(this).val() != "" ? Number($(this).val()) : 0;
            }
        });
        var PLBValue = adtplb + "|" + chdplb + "|" + infplb;

        //STS-166

        checkCondition_BUDGET = checkCondition_BUDGET != null ? checkCondition_BUDGET : ""
        checkCondition_TRAVEL_REQUEST = checkCondition_TRAVEL_REQUEST != null ? checkCondition_TRAVEL_REQUEST : ""
        checkCondition_Sub_Reason = checkCondition_Sub_Reason != null ? checkCondition_Sub_Reason : ""
        checkCondition_Recharge = checkCondition_Recharge != null ? checkCondition_Recharge : ""
        checkCondition_Reason_of_travel = checkCondition_Reason_of_travel != null ? checkCondition_Reason_of_travel : ""
        checkCondition_CostcenterID = checkCondition_CostcenterID != null ? checkCondition_CostcenterID : ""
        checkCondition_Job_number = checkCondition_Job_number != null ? checkCondition_Job_number : ""
        checkCondition_Package_id = checkCondition_Package_id != null ? checkCondition_Package_id : ""

        OtherTicketInfo = checkCondition_BUDGET + "|" +
                           checkCondition_TRAVEL_REQUEST + "|" +
                               checkCondition_Sub_Reason + "|" +
                               checkCondition_Recharge + "|" +
                               checkCondition_Reason_of_travel + "|" +
                               checkCondition_CostcenterID + "|" +
                               checkCondition_Job_number + "|" +
                               checkCondition_Package_id;

        var strPlatingcarrier = $("#ddl_platingcarrier").val();
        strPlatingcarrier = ((strPlatingcarrier != null && strPlatingcarrier != undefined && strPlatingcarrier != "") ? strPlatingcarrier : "")

        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loadurl + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });

        var ReBookingFlag = false;

        if (arg == 1) {
            ReBookingFlag = true;
            if (OBCAmount != "" && OBCAmount != null && OBCAmount != "0") {
                ObcAmount = OBCAmount;
            }
            else {
                return false;
            }
            if (ReBookingFare != "" && ReBookingFare != null && ReBookingFare != "0") {
                WithoutTaxBookingAMount = ReBookingFare;
            }
            else {
                return false;
            }
        }
        var AsPercrsvalue = (cardGetSet == "A" ? selectedpaxdetails[0].PAYMENTINFO.split('/')[0] : "");

        var txtPricingfare = $('#txt_PricingCode').val();

        var Query = {
            hdf_pnrinfo: hdf_pnrinfo,
            missing_title_select: missing_title,
            TSTCOUNT: TST,
            cardGetSet: cardGetSet,
            BookingAmt: BookingAmount,
            hdffareinfo: hdf_fareinfo,
            farequalifier: farequalif,
            faretoken: fareToken,
            AirportTypes: AirportType,
            Corporatename: AgentID,
            Employeename: Employeename,
            EmpCostCenter: EmpCostCenter,
            EmpRefID: EmpRefID,
            BranchID: BranchID,
            GSTDetails: GstDetails,
            SessionKey: $("#ddl_OfficeID").length > 0 ? $("#ddl_OfficeID").val() : "",
            Employee_MobileNo: Employee_Contact_No,
            PassportDetails: PassportDetails,
            tstReference: tstReference,
            FFNDetails: cuntdatadetails,
            Passthrough: PassthroughValue,
            OBCTAX: ObcAmount,
            WithoutTaxBookingAMount: WithoutTaxBookingAMount,
            StrFareFlag: StrFareFlag,
            CRSTYPE: CRSTYPE,
            ERPDetails: faretypereason,
            OtherTicketInfo: OtherTicketInfo,
            Markup: MarkupValue,
            Commission: CommissionValue,
            PLB: PLBValue,
            TicketingMode: TicketingMode,
            CorporteCode: $("#txt_corporateCode").val(),
            RetrieveIsGST: RetrieveIsGST,
            Earnings: TotalEarnings,
            CRSPNR: $('#txt_CRSpnr_Queue').val().trim(),
            strEndorsement: $('#txtEndorsement').val(),
            strRebookFlag: ReBookingFlag,
            strFOPFlag: FOPFlag,
            strAsPerCRS: AsPercrsvalue,// $('#CRSFOP').html().trim(),
            strPlatingcarrier: strPlatingcarrier,
            strPricingcode: txtPricingfare,
            strCashPaymentDet: strCashPaymentinfo,
        };

        $.ajax({
            type: "POST",
            contentType: "Application/json; charset=utf-8",
            url: TicketingConfirm,
            data: JSON.stringify(Query),
            datatype: "json",
            success: function (data) {
                $.unblockUI();
                if (data != null) {
                    $(".udk-totalfare-show").show();

                    if (data.Status == "-1") {
                        infoAlert("Your session has been expired!", "");
                        window.location.href = $('#dvDataProp').data('session-url');
                    }

                    if (data.Result[4] == "TRUE") {
                        var responsejson = JSON.parse(data.Result[2]);
                        OBCAmount = responsejson.Result["OPCAmount"];
                        ReBookingFare = responsejson.Result["RebookingFare"];
                        var Message = data.Result[0] + "<br><br>Are you sure you want to continue with ReBooking.....";
                        $('#spnFareChangeMsg').html(Message);
                        Callpopup("Confirm Fare", "modal-FareChangePopup", "500px");
                        return false;
                    }

                    if (data.Result[0] != null && data.Result[0] != "") {
                        if (data.Result[0] == "") {
                            infoAlert("Unable to process ticketing. Please contact customer care (#06).", "");
                        }
                        else {
                            infoAlert(data.Result[0], "");
                        }

                        if (data.Result[2] != null && data.Result[2] != "") {
                            var responsejson = JSON.parse(data.Result[2]);
                            asyncafterbooking(JSON.stringify(responsejson));
                        }
                        $('#modal-FarePopup').iziModal('close');//
                        return false;
                    }

                    if (data.Result[2] != null && data.Result[2] != "") {
                        clearfaredetails();
                        var responsejson = JSON.parse(data.Result[2]);
                        asyncafterbooking(JSON.stringify(responsejson));
                        BuildTicketPage(data.Result[2]);//, data.Result[5], data.Result[6]
                        $("#retriveinformation").hide();
                        $("#loadTicket").show();
                    }
                }
                else {
                    infoAlert("Error occured please contact customer care!", "");
                    $('#modal-FarePopup').iziModal('close');
                    $('#sp_pnr_details').html("");
                    $('.wholeclassdiv').hide();
                }
            },
            error: function (e) {
                $.unblockUI();
                $('#modal-FarePopup').iziModal('close');
                $('#sp_pnr_details').html("");
                //document.getElementById('btn_ticketing').style.display = 'none';
                $('.wholeclassdiv').hide();
                clearfaredetails();
                infoAlert("Inter server error!", "");
                return false;
            }
        });
    }
    catch (e) {
        infoAlert("Unable to process Ticketing! (#06)", "");
    }
}

function BuildTicketPage(tables) {//,servicefee,markup
    var jsonDetails = JSON.parse(tables);
    var _ticketpage = '';
    var totalGrossFare = 0;
    try {
        //#region Total Set #00
        _ticketpage += '<div class="row">';

        //#region set #01
        _ticketpage += '<div class="col-md-12 ">';
        _ticketpage += '        <div class="main-title" style="color: initial;text-align: center;">'
        _ticketpage += '          <h3>Ticket Information '
        _ticketpage += '              <button id="iprinticon" onclick="iclickPrinticon()" style="color: rgb(255, 102, 51); display: none;">'
        _ticketpage += '                  <i class="fa fa-print" aria-hidden="true"></i>'
        _ticketpage += '              </button>'
        _ticketpage += '          </h3>'
        _ticketpage += '      </div>'
        _ticketpage += ' </div>'
        //#endregion


        //#region Set #02
        _ticketpage += '<div class="clearfix" style="padding-top: 5%;"></div>';

        _ticketpage += '<div class="row">'
        _ticketpage += '     <div class="col-xs-12 col-md-12 col10" style="text-align: center;">'
        _ticketpage += '         <label id="lblbookmsg" style="color: green; font-size: 17px; font-weight: bold;">Ticket Booked Successfully &nbsp;&nbsp; <i class="fa fa-check-circle" style="color: #188e14;"></i>'
        _ticketpage += ' </label>'
        _ticketpage += '</div>'
        _ticketpage += '</div>';
        //#endregion

        //#region set #03

        _ticketpage += ' <div class="col-xs-12 col-md-12 col10" id="dvBookingdetails" style="text-align: center; display: block;margin-top: 20px;">'

        //#region sub set #03-001
        _ticketpage += ' <div class="row" style="padding-left: 1.4%;text-align: left;padding-right: 0.4%;margin-bottom:10px;">'
        _ticketpage += '           <div class="col-lg-3 col10" style="padding: 5px;">'
        _ticketpage += '             <span style="font-size: 16px; font-weight: bold;">Ticket Details</span>'
        _ticketpage += '          </div>'
        _ticketpage += '         <div class="col-lg-offset-6 col-lg-2 clscheckBlockPNR" style="float: right;">'
        _ticketpage += '             <label style="background: #f60; padding: 5px; border-radius: 2px; border: 1px dashed #fff;color: #FFF; width: 100%;">'
        _ticketpage += '                <span style="font-size: 16px; font-weight: bold;">Total Fare :</span> <span style="font-size: 16px; font-weight: bold; float: right;" id="spntotalfares_udk">' + jsonDetails.PassengerPNRDetails[0].GROSSAMT + '</span><label>'
        _ticketpage += '                 </label>'
        _ticketpage += '            </label>'
        _ticketpage += '         </div>'
        _ticketpage += '     </div>'

        //#endregion

        //#region sub set #03-002
        _ticketpage += '  <div class="row" id="dvMultiBookingdetails">'
        _ticketpage += '  <div id="Bookingdetails" class="columns col-lg-12">'


        _ticketpage += '<table id="table1" width="100%" cellpadding="3px" class="no-more-tables table-striped">'

        _ticketpage += '<thead><tr class="dv_table_header" style="border: 1px solid #ddd;text-align: center;">'
        _ticketpage += '<th>' + PNRFlag + '</th><th>CRS PNR</th><th>Airline PNR</th><th>Flight No</th><th>Class</th><th>Origin</th><th>Destination</th>'
        _ticketpage += '<th>Departure</th><th style="padding:5px;">Arrival</th></tr></thead>'

        _ticketpage += '<tbody>'

        var segment_car = "";
        var tempvar = 0;


        var currentvalue = [];
        //var comgroupedfli = availres.reduce(function (result, current) {
        //    result[current[0].FlightNumber + "~" + current[0].DepartureTime] = result[current[0].FlightNumber + "~" + current[0].DepartureTime] || []; //For both flight Number and Departure time
        //    result[current[0].FlightNumber + "~" + current[0].DepartureTime].push(current);
        //    return result;
        //}, {});
        var comgroupedfli = jsonDetails.PassengerPNRDetails.reduce(function (previousvalue, currentvalue) {

            previousvalue[currentvalue.SEGMENTNO] = previousvalue[currentvalue.SEGMENTNO] || []
            previousvalue[currentvalue.SEGMENTNO].push(currentvalue);
            return previousvalue;
        }, {});
        var comgrouparry = [];
        for (var obj in comgroupedfli) {
            comgrouparry.push(comgroupedfli[obj]);
        }
        // return comgrouparry;


        for (var j = 0; j < comgrouparry.length; j++) {
            _ticketpage += '<tr class="dv_table_header" style="border: 1px solid #ddd;text-align: center;background: #fff;">'
            _ticketpage += '<td class="clswheat" id="Retrive_riyaPnr" data-title="S PNR">' + comgrouparry[j][0].SPNR + '</td>'
            _ticketpage += '<td class="clswheat" data-title="CRS PNR">' + comgrouparry[j][0].CRSPNR + '</td>'
            _ticketpage += '<td class="clswheat" data-title="Airline PNR">' + comgrouparry[j][0].AIRLINEPNR + '</td>'
            _ticketpage += '<td class="clswheat" data-title="Flight No">' + comgrouparry[j][0].FLIGHTNO + '</td>'
            _ticketpage += '<td class="clswheat" data-title="Class">' + comgrouparry[j][0].CLASS + '</td>'
            _ticketpage += '<td class="clswheat" data-title="Origin">' + comgrouparry[j][0].ORIGIN + '</td>'
            _ticketpage += '<td class="clswheat" data-title="Destination">' + comgrouparry[j][0].DESTINATION + '</td>'
            _ticketpage += '<td class="clswheat" data-title="Departure">' + comgrouparry[j][0].DEPARTUREDATE + ' ' + comgrouparry[j][0].DEPARTURETIME + '</td>'
            _ticketpage += '<td class="clswheat" data-title="Arrival">' + comgrouparry[j][0].ARRIVALDATE + ' ' + comgrouparry[j][0].ARRIVALTIME + '</td>'
            _ticketpage += '</tr>'

            //segment_car = jsonDetails.PassengerPNRDetails[i].SEGMENTNO;
            //}
        }

        _ticketpage += '</tbody>'
        _ticketpage += '</table>'
        _ticketpage += '  </div>'
        _ticketpage += '  </div>'
        //#endregion

        _ticketpage += '</div>';
        //#endregion

        //#region set #04
        _ticketpage += '<div class="col-xs-12 col-md-12 col10" id="dvPaxdetails" style="text-align: center; display: block; padding-top: 2%;">'

        _ticketpage += '<div class="row">'
        _ticketpage += '<span style="font-size: 15px; font-weight: bold; margin-bottom: 10px; float: left; width: 100%;">Passenger Details</span>'
        _ticketpage += '</div>'

        //#region sub set #04-001
        _ticketpage += '<div class="row">'
        _ticketpage += '<div id="Paxdetails" class="columns col-lg-12">'

        _ticketpage += '<table id="table2" width="100%" cellpadding="3px" class="no-more-tables table-striped">'
        _ticketpage += '<thead><tr class="dv_table_header" style="border: 1px solid #ddd;text-align: center;">'
        _ticketpage += '<th>First Name</th><th>Last Name</th><th>Passenger Type</th><th>DOB</th><th>Ticket No.</th></tr></thead>'
        _ticketpage += '<tbody>'

        var tempvarpAX = '';
        for (var i = 0; i < jsonDetails.PassengerPNRDetails.length; i++) {
            if (tempvar != jsonDetails.PassengerPNRDetails[i].PAXREFERENCE) {// Number(
                totalGrossFare += Number(jsonDetails.PassengerPNRDetails[i].GROSSAMT);
            }
            tempvar = jsonDetails.PassengerPNRDetails[i].PAXREFERENCE;
            if (tempvarpAX != jsonDetails.PassengerPNRDetails[i].PAXREFERENCE) {//Number(
                tempvarpAX = jsonDetails.PassengerPNRDetails[i].PAXREFERENCE;
                _ticketpage += '<tr class="dv_table_header" style="border: 1px solid #ddd;text-align: center;">'
                _ticketpage += '<td class="clswheat" data-title="First Name">' + jsonDetails.PassengerPNRDetails[i].FIRSTNAME + '</td>'
                _ticketpage += '<td class="clswheat" data-title="Last Name">' + jsonDetails.PassengerPNRDetails[i].LASTNAME + '</td>'
                _ticketpage += '<td class="clswheat" data-title="Passenger Type">' + jsonDetails.PassengerPNRDetails[i].PAXTYPE + '</td>'
                _ticketpage += '<td class="clswheat" data-title="DOB">' + jsonDetails.PassengerPNRDetails[i].DATEOFBIRTH + '</td>'
                _ticketpage += '<td class="clswheat" data-title="Ticket No.">' + jsonDetails.PassengerPNRDetails[i].TICKETNO + '</td>'
                _ticketpage += '</tr>'
            }
        }

        _ticketpage += '</tbody>'
        _ticketpage += '</table>'
        _ticketpage += '</div>';
        _ticketpage += '</div>';
        //#endregion
        _ticketpage += '</div>';
        //#endregion    
        _ticketpage += '</div>';
        //#endregion
        $("#ticketPageDetails").html(_ticketpage);

        var markupsf = $("body").data("MarkupSF");
        $("#spntotalfares_udk").html(totalGrossFare + Number(markupsf));// servicefee + markup
    }
    catch (e) {
        console.log(e.message);
    }
}

function hideFarepopup(arg) {
    var strfareFlag = $("body").data("StrFareFlag");
    $('#modal-FarePopup').iziModal('close');
    $("#btn_getfare").show();
    //$("#txt_corporateCode").val("");//STS-166
    //$("#txt_corporateCode").show();

    if (arg != "1" && strfareFlag != "N")
        $(".udk-totalfare-show").show();
    if (arg == "0" && strfareFlag == "N")
        $(".udk-totalfare-show").hide();
    // $("#lbl_corcode").show();
}

function ConfirmFare() {
    var hdf_pnrinfo = $('#hdf_pnrinfo').val();
    var farequalif = "";
    farequalif = $('#hdn_farequalifier').val();

    var BookingAmount = "0";
    BookingAmount = $('#popupfare').html();
    var topupfare = parseInt(BookingAmount) + parseInt($('#hdnOBCAmountTopup').val());

    //
    $('#CRSFOP').html('');
    var paxdet = selectedpaxdetails[0].PAYMENTINFO;
    if (paxdet != "" && paxdet != null && TerminalType == "T" && $("#ddlPaymentMode option[value='A']").length == 0) {
        $("#ddlPaymentMode").append('<option value="A">As Per CRS</option>');
    }
    //
    //var markupvalue = $("body").data("markupvalue");
    try {
        var CommAMT = 0, PLBAMT = 0, INCAMT = 0, ChkEaringAMT = 0;
        $(".clscommamt").each(function () {
            if ($(this).val().trim() != "") {
                CommAMT += parseFloat($(this).val());
            }
        });
        $(".clsplbamt").each(function () {
            if ($(this).val().trim() != "") {
                PLBAMT += parseFloat($(this).val());
            }
        });
        $(".clsincentive").each(function () {
            if ($(this).val().trim() != "") {
                INCAMT += parseFloat($(this).val());
            }
        });
        ChkEaringAMT = parseFloat(CommAMT) + parseFloat(PLBAMT) + parseFloat(INCAMT);
        if (ChkEaringAMT > topupfare) {
            alert("Edited earing amount is greater than Ticket Amount!");
            return false;
        }
    }
    catch (e) {

    }
    //

    var showflag = false;
    $('.clsApproval').each(function () {
        if (Number($(this).val()) > Number($(this).data('oldvalue')) && APP_Terminal == "T") {
            showflag = true;
        }
    });
    if (showflag && ApprovalData.length == 0 && APP_Terminal == "T") {
        alert("approver not available please contact support team(#01).");
        return false;
    }
    if (showflag && $("#txt_ApprovedBY").val() == "") {
        alert("Please select approver.");
        return false;
    }

    var markupvalue = 0;
    var servicefee = 0;

    $(".clsmarkamt").each(function () {
        if ($(this).val().trim() != "") {
            markupvalue += parseFloat($(this).val());
        }
    });
    $(".clsserfee").each(function () {
        if ($(this).val().trim() != "") {
            servicefee += parseFloat($(this).val());
        }
    });

    var markupsf = (Number(markupvalue) + Number(servicefee)).toFixed(2);
    $("body").data("MarkupSF", markupsf);
    $('#spnPopupFare').html(Number(topupfare) + Number(markupsf));

    var hdf_fareinfo = $('#hdf_fareinfo').val();

    var TSTReference = "";
    TSTReference = $('#hdn_TSTReference').val();
    var strfareFlag = $("body").data("StrFareFlag");
    var Query = {
        hdf_pnrinfo: hdf_pnrinfo,
        BookingAmt: BookingAmount,
        hdffareinfo: hdf_fareinfo,
        farequalifier: farequalif,
        strfareFlag: strfareFlag
    };

    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadurl + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });

    $.ajax({
        type: "POST",
        contentType: "Application/json; charset=utf-8",
        url: ConfirmFareURL,
        data: JSON.stringify(Query),
        datatype: "json",
        success: function (data) {
            if (data.Status == "-1") {
                window.location.href = $('#dvDataProp').data('session-url');
                return false;
            }

            if (data.Result != null && data.Result[1] != null && data.Result[1] == "SUCCCESS") {
                $.unblockUI();
                ConfirmTicketing();
            }
            else if (data.Result != null && data.Result[1] != null && data.Result[1] == "FAILED") {
                $.unblockUI();
                infoAlert("Unable to process your request.", "");
                return false;
            }
            else if (data.Result != null && data.Result[0] != null && data.Result[0] != "") {
                $.unblockUI();
                infoAlert(data.Result[0], "");
            }
        },
        error: function (e) {
            $.unblockUI();
            infoAlert("Inter server error!", "");
            return false;
        }
    });
}

function infoAlert(MSG, ID) {
    //Lobibox.alert('info', {
    //    msg: MSG,
    //    closeonesc: false,
    //    callback: function ($this, type) {
    //        $("#" + ID).focus();
    //    }
    //});
    //return false;
    $('#modal-alert').iziModal('destroy');
    $("#modal-alert").iziModal({
        title: MSG,
        icon: 'fa fa-info',
        headerColor: '#bd5b5b',
        width: "500px",
        onClosed: function () {
            $("#" + ID).focus();
        }
    });
    $('#modal-alert').iziModal('open');
    return false;
}

function showerralert(msg, timout, ID) {
    $('#modal-alert').iziModal('destroy');
    if (timout == null || timout == "") {
        $("#modal-alert").iziModal({
            title: msg,
            icon: 'fa fa-warning',
            headerColor: '#bd5b5b',
            width: "500px",
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

function includeGstdet() {

    if ($("#includeGST")[0].checked == true) {
        $("#divGstDetails").show();
        $("#includeNewGST").attr("disabled", false);
    } else {
        $("#divGstDetails").hide();
        $("#includeNewGST").attr("disabled", true);
    }
}

function BtnQueueOk_Clear() {

    if ($(".dv_width").length) {
        $(".dv_width").hide();
    }

    if ($(".wholeclassdiv").length) {
        $(".wholeclassdiv").hide();
    }

    $("#rdoaircat_FSC")[0].checked = true;
    //checkcatogory(0);
    $(".udk-totalfare-show").hide();
    $("#spnTotalgrossfare").html("0");
    $("#txt_CRSpnr_Queue").val("");
    $("#ddl_QueueStock").val("-1");
    $("#ddl_QueueAT").val("-1");
    $(".guestempradiodv").hide();
    $("#ddl_Faretype").val("-1");
    $("#ddlclient").val("");
    $(".clsfaredetails").hide();
    $("#dvshowmoreicn").hide();
    $('#dvFrequentFlyer').html("");
    $("body").data("TotalCount", "");
    $("body").data("Paxdet", "");
    $("body").data("Platingcarrier", "");
    $("#txt_CRSpnr_QueueNO").val("");
    $("#dvEarnings").html("");
    $("#dvCommdetails").html("");
}

function addtotalgrsfare(arg) {
    var values = 0;
    // var values = arg.value;
    if ($(".clsgrey").find(".markvalue").length) {
        $(".clsgrey .markvalue").each(function () {
            if ($(this).val() != "") {
                values += parseFloat($(this).val());
            }
            else {
                values += 0;
            }
        });
    }
    else {
        $(".markvalue").each(function () {
            if ($(this).val() != "") {
                values += parseFloat($(this).val());
            }
            else {
                values += 0;
            }
        });
    }
    $("body").data("markupvalue", values);
    var grss = $("#spnTotalgrossfare").data("originalfare");
    $("#spnTotalgrossfare").html(Number(Number(grss) + Number(values)));
}

function ClearFareType(arg) {
    //STS-166
    var i = 0;
    var spntotalfare = 0;
    $('.clscheck').each(function () {
        if (this.checked == true) {
            spntotalfare += parseFloat($('#' + this.id).val());
            i++;
        }
    });
    $("#spnTotalgrossfare").data("originalfare", spntotalfare);
    $("#spnTotalgrossfare").html(spntotalfare);
    addtotalgrsfare();
    if (i > 0) {
        $(".udk-totalfare-show").show();
    }
    else {
        $(".udk-totalfare-show").hide();
    }
    //STS-166

    var idd = arg.id.substring(3, 5);
    var StrFareFlag = $("body").data("StrFareFlag");

    var TotalCount = $("body").data("TotalCount");
    var Paxdet = $("body").data("Paxdet");
    var Platingcarrier = $("body").data("Platingcarrier");

    if ($("#rdoaircat_FSC")[0].checked == true) {
        _loadFrequentflyerDet(Platingcarrier, TotalCount, Paxdet);
        $("#dvFrequentFlyer").show();
    }

    $(".clsfaredetails").hide();
}

//PASSTHORUGH
function fetchpassthrough(arg, Airpayment) {
    var TST = "";
    var noOfCheckBox = document.getElementById('table1').getElementsByTagName('input');
    var AirlineCode = $("body").data("AirlinePlatingCarriers") || "";
    var NewAir = AirlineCode.split('|');
    for (var k = 0; k < noOfCheckBox.length; k++) {
        if (noOfCheckBox[k].checked == true) {
            TST = noOfCheckBox[k].id.replace('tst', '');
        }
    }
    var ffn = $.grep(NewAir, function (n, i) {
        return n.split('~')[1] == TST
    });

    $("#dvPGTotaltaxamt").css("display", "none");
    //if ($("#" + arg).val() != "B") {
    //    $(".paymentdynachangclss").removeClass("col-sm-offset-0"); // by vijai
    //    $(".paymentdynachangclss").addClass("col-sm-offset-4"); // by vijai
    //}

    var BookingAmount = "0";
    BookingAmount = $('#popupfare').html();

    //var topupfare = parseInt(BookingAmount) + parseInt( $('#hdnOBCAmountTopup').val())
    //$('#spnPopupFare').html(topupfare);

    if ($('#' + arg).val() == "A") {
        var AsPercrsvalue = selectedpaxdetails[0].PAYMENTINFO;
        $('#CRSFOP').html(AsPercrsvalue.split('/')[0]);
    }
    else {
        $('#CRSFOP').html('');
    }

    if ($("#" + arg).val() == "B") {
        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loadurl + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' }
        });
        //var AgentID = sessionStorage.getItem("clientid") != null ? sessionStorage.getItem("clientid") : "";

        var AgentID = $('#hdnClientID').val();
        var AirportID = "";
        var CRSTYPE = "";
        if ($("#rdoaircat_FSC")[0].checked) {
            CRSTYPE = $("#ddl_QueueStock").val().trim();
        }
        else {
            CRSTYPE = $("#ddl_AirlineStock").val().split('|')[0];
        }

        var Segment_type = $("#ddl_QueueAT").val().trim();
        if (Segment_type != null)
            AirportID = Segment_type != "null" && Segment_type != "" ? Segment_type : "";
        var EMPID = '';//$("#UserID").val();//$("#ddlclient").val()
        var passthroughvalue = { Corporate: AgentID, AirlineCode: ffn[0].split('~')[0].trim(), AIRPORT_ID: AirportID, EMPCODE: EMPID, CRSTYPE: CRSTYPE, BOOKINGAMOUNT: BookingAmount };
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: FetchPassthrought_URL,
            data: JSON.stringify(passthroughvalue),
            dataType: "json",
            success: function (data) {
                if (data.status == "01" && data.arrfetchdata != "") {
                    var Result = JSON.parse(data.arrfetchdata);

                    $("#txtar_CardType").val(Result[0].PG_CARD_NAME.toUpperCase() + "(" + Result[0].PG_CARD_TYPE.toUpperCase() + ")");
                    $("#txtar_CardType option:selected").attr('data-txtar_cardtype', JSON.stringify(Result));
                    //$("#txtar_CardType option:selected").text(Result[0].PG_BANK_NAME + "(" + Result[0].PG_CARD_TYPE + ")" + (Result[0].PG_CARD_NAME != null && Result[0].PG_CARD_NAME != "" ? "[" + Result[0].PG_CARD_NAME + "]" : "") + (Result[0].PG_HOLDER_NAME != null && Result[0].PG_HOLDER_NAME != "" ? "[" + Result[0].PG_HOLDER_NAME + "]" : ""));
                    $("#txtar_cardNo").val(Result[0].PG_CARD_NUMBER);
                    $("#txt_BankName").val(Result[0].PG_BANK_NAME);
                    $("#txt_HolderName").val(Result[0].PG_HOLDER_NAME);
                    $("#txtar_cvv").val(Result[0].PG_CVV);
                    $("#ddlMonth").val(Result[0].PG_EXPIRY_DATE.substring(0, 2));
                    $("#ddlyear").val("20" + Result[0].PG_EXPIRY_DATE.substring(2, 4));
                    $("#divviewcarddetails").css("display", "block");
                    //STS-166
                    //$("#hdnOBCAmount").val(Result[0].PG_CARD_OBC_AMOUNT);
                    //var TotalAmt = parseInt(Result[0].PG_CARD_OBC_AMOUNT) + parseInt(BookingAmount);
                    //$('#spnPopupFare').html(TotalAmt)

                    var TotalAmt = parseInt($("#hdnOBCAmount").val()) + parseInt(BookingAmount);
                    $('#spnPopupFare').html(TotalAmt)
                    //STS-166
                    //$(".paymentdynachangclss").removeClass("col-sm-offset-4"); // by vijai
                    //$(".paymentdynachangclss").addClass("col-sm-offset-0"); // by vijai
                }
                else {
                    $("#divviewcarddetails").css("display", "block");
                    //$(".paymentdynachangclss").removeClass("col-sm-offset-4"); // by vijai
                    //$(".paymentdynachangclss").addClass("col-sm-offset-0"); // by vijai
                }
                $.unblockUI();
            },
            error: function (result) {
                $.unblockUI();
                showerralert("Unable to connect remote server.", "", "");
                //alert("Unable to connect remote server.");
                return false;
                $.unblockUI();
            },
        });
    }
    else if ($("#" + arg).val() == "P") {
        $("#divviewcarddetails").css("display", "none");
        var Taxdetails = PG_ServiceCharges;
        Taxdetails = Taxdetails.split('|');
        var Totalbkamt = $("#spnptnamount").html();
        var Param = { Totalpayamount: Totalbkamt }
        if (Taxdetails[0] == "Y" && PGtaxdetailsshowing == true) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: Service_Tax_Calculation_URL,
                data: JSON.stringify(Param),
                dataType: "json",
                success: function (data) {
                    $("#spnptntaxamount").html(data.Servicecharge);
                    $("#spnptntotalamount").html(data.Totalamt);
                    $("#spntaxtext").html("(" + data.Taxdetails + ")");
                    $("#dvPGTotaltaxamt").css("display", "block");
                    PGtaxdetailsshowing = false;
                },
                error: function (result) {
                    $.unblockUI();
                    showerralert("Unable to connect remote server.", "", "");
                    return false;
                }
            });
        }
        else if (Taxdetails[0] == "Y") {
            $("#dvPGTotaltaxamt").css("display", "block");
        }
        clearcarddetails();
    }
    else {
        $("#divviewcarddetails").css("display", "none");
        clearcarddetails();
    }

    if ($("#" + arg).val() == "H") {
        $("#dvcashpayment,#dvEmpDetails").show();
        $("#txtcash_Paymentdate").removeClass('hasDatepicker');
        var dadt = new Date;
        var day = dadt.getDate();
        var Month = dadt.getMonth() + 1;
        var Year = dadt.getFullYear() + 1;
        if (day < 10) { day = '0' + day } if (Month < 10) { Month = '0' + Month }
        var maxadtyearadt = day + "/" + Month + "/" + Year;
        var minadtyearadt = day + "/" + Month + "/" + dadt.getFullYear();
        $("#txtcash_Paymentdate").datepicker({
            numberOfMonths: 1,
            showButtonPanel: false,
            dateFormat: "dd/mm/yy",
            maxDate: maxadtyearadt,
            minDate: minadtyearadt,
        });
        //$("#txtcash_Paymentdate").val(minadtyearadt)
    }
    else {
        $("#dvcashpayment,#dvEmpDetails").hide();
        $(".clsClear").val("");
        $("#dvEmployee").hide();
    }
}

function clearcarddetails() {
    $("#txtar_CardType").val("");
    $("#txtar_cardNo").val("");
    $("#txtar_cvv").val("");
    $("#ddlMonth").val("");
    $("#ddlyear").val("");
    $("#txt_BankName").val("");
    $("#txt_HolderName").val("");
    $("#divviewcarddetails").css("display", "none");
}

function loadPaymentDEtails(arg) {
    var Paymentdetails = arg;
    if (Paymentdetails != null && Paymentdetails != "") {
        var Paymentoptions = Paymentdetails.trim().split('~');
        
        $("#ddlPaymentMode,#ddl_paymenttype").empty();
        $("#ddlPaymentMode,#ddl_paymenttype").append('<option value="0" selected="selected">--Select--</option>');
        
        for (var k = 0; k < Paymentoptions.length; k++) {
            if (Paymentoptions[k] == "B") {
                $("#ddlPaymentMode,#ddl_paymenttype").append('<option id="passthrough" value="B">Pass&nbsp;Through</option>');
            }
            if (Paymentoptions[k] == "C") {
                $("#ddlPaymentMode,#ddl_paymenttype").append('<option value="C">Credit</option>');
            }
            if (Paymentoptions[k] == "H") {
                $("#ddlPaymentMode,#ddl_paymenttype").append('<option value="H">Cash</option>');
            }
            if (Paymentoptions[k] == "T") {
                $("#ddlPaymentMode,#ddl_paymenttype").append('<option value="T">Topup</option>');
            }
        }
    }
}

function ShowFreetext(arg) {

    var idd = arg.id;
    var TST = "";

    var AirlinePNR = "";
    var AirlineName = "";
    var AirlineCategory = "";
    var CRSPNR = "";
    var CRSTYPE = "";

    var noOfCheckBox = document.getElementById('table1').getElementsByTagName('input');

    var hdf_pnrinfo = $('#hdf_pnrinfo').val();
    var hdf_fareinfo = $('#hdf_fareinfo').val();

    var FareType = $('#hdn_farequalifier').val($('#' + idd).data("farequalifier"));
    $('#hdn_TSTReference').val($('#' + idd).data("tstreference"));
    $('#hdnCorporateCode').val($('#' + idd).data("corporatecode"));

    var farequalif = "";
    farequalif = $('#hdn_farequalifier').val();

    var fareToken = "";
    fareToken = $('#hdn_pricingToken').val();
    var tstReference = "";

    tstReference = $('#hdn_TSTReference').val();
    var RAT = true;
    var RATLCC = true;
    if ($("#rdoaircat_FSC")[0].checked) { RATLCC = false } else { RATLCC = true }

    //STS-166
    //if (sessionStorage.getItem("ticketstatus") == "1") {
    //    RAT = false;
    //    $("#btn_getfare span").html("Accounting");
    //} else {
    //    RAT = true;
    //    $("#btn_getfare span").next().html("Check Fare");
    //}

    var AirportType = $("#ddl_QueueAT").val().trim();

    var Corporatename = $("#ddlclient").val();
    var Employeename = "";
    var EmpCostCenter = "";
    EmpRefID = "";
    Newempflag = "";

    for (var j = 0; j < noOfCheckBox.length; j++) {
        if (noOfCheckBox[j].checked == true) {
            status = true;
            TST = noOfCheckBox[j].id.replace('tst', '');
            // var FareType = $('#' + TST).data("farequalifier").toUpperCase().trim();
        }
    }

    var AirlinePNR = "";
    var AirlineName = "";
    var AirlineCategory = "";
    var CRSPNR = "";
    var CRSTYPE = "";

    if ($("#rdoaircat_FSC")[0].checked) {
        AirlineCategory = "FSC";
        CRSPNR = $("#txt_CRSpnr_Queue").val().trim();
        CRSTYPE = $("#ddl_QueueStock").val().trim();
        RAT = false;
    }
    else {
        RAT = true;
        AirlineCategory = "LCC";
        CRSPNR = $("#txt_Airlinepnr_Queue").val();
        CRSTYPE = $("#ddl_AirlineStock").val().split('|')[0];
        AirlineCategory = $("#ddl_AirlineStock").val().split('|')[1];
        AirlinePNR = $("#txt_Airlinepnr_Queue").val();
        AirlineName = $("#ddl_AirlineStock").val().split('|')[0];
    }

    var Queuenumber = $("#txt_CRSpnr_QueueNO").val();

    //var BranchID = sessionStorage.getItem("branchid") != null ? sessionStorage.getItem("branchid") : "";
    //var AgentID = sessionStorage.getItem("clientid") != null ? sessionStorage.getItem("clientid") : "";

    var AgentID = $('#hdnClientID').val();
    var BranchID = $('#hdnBranchID').val();

    var strCorporateCode = $('#hdnCorporateCode').val();//$("#txt_corporateCode").val();

    var StrFareFlag = $("body").data("StrFareFlag");
    var Query = {
        hdf_pnrinfo: hdf_pnrinfo,
        TSTCOUNT: TST,
        //cardGetSet: cardGetSet,
        hdffareinfo: hdf_fareinfo,
        farequalifier: farequalif,
        faretoken: fareToken,
        FareType: farequalif,
        AirportTypes: AirportType,
        Corporatename: AgentID,
        Employeename: Employeename,
        EmpCostCenter: EmpCostCenter,
        EmpRefID: EmpRefID,
        BranchID: BranchID,
        SessionKey: $("#ddl_OfficeID").length > 0 ? $("#ddl_OfficeID").val() : "",
        CRSPNR: CRSPNR,
        AirlinePNR: AirlinePNR,
        CRSTYPE: CRSTYPE,
        AirlineCategory: AirlineCategory,
        Queuenumber: Queuenumber,
        strCorporateCode: strCorporateCode,
        StrFareFlag: StrFareFlag
    };

    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadurl + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });
    $.ajax({
        type: "POST",
        contentType: "Application/json; charset=utf-8",
        url: FareRule,
        data: JSON.stringify(Query),
        datatype: "json",
        success: function (data) {
            try {
                $.unblockUI();
                if (data != null) {

                    if (data.Status == "-1") {
                        window.location.href = $('#dvDataProp').data('session-url');
                        return false;
                    }

                    if (data.status != "" && data.status == "1") {

                        var Resultss = $.xml2json(data.message);
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
                        var new_arrkjjj = [];
                        if (uniqueNames.length > 0) {
                            for (var K = 0; K < uniqueNames.length ; K++) {
                                new_arr = $.grep(Resultss["Table1"], function (n, i) {
                                    return n.SEGMENT == uniqueNames[K];

                                });
                                new_arrkjjj.push(new_arr);
                            }
                        }

                        var str = '';

                        if (new_arrkjjj.length > 0) {
                            //str += '<h4 style="text-align:center;">Fare Rule <span style="float:right;color:red;cursor:pointer;" onclick="ShowBookdata();"><i class="fa fa-times-circle"></i> Close<span></h4>'
                            for (var j = 0; j < new_arrkjjj.length ; j++) {
                                if (j == 0) {
                                    str += '<span class="clsAllFarerule clsFareruleSelect" id="spnFareSelect' + j + '" onclick="javascript:ShowSegWiseFareRule(' + j + ');">' + new_arrkjjj[j][0].SEGMENT + '</span>';
                                }
                                else {
                                    str += '<span class="clsAllFarerule clsFarerule" id="spnFareSelect' + j + '" onclick="javascript:ShowSegWiseFareRule(' + j + ');">' + new_arrkjjj[j][0].SEGMENT + '</span>';
                                }
                            }

                            str += '</div>';
                            for (var i = 0; i < new_arrkjjj.length ; i++) {
                                if (i == 0) {
                                    str += '<div  class="clsSgFareRule"  id="dvFareRuletbl_' + i + '"  >';
                                }
                                else {
                                    str += '<div  class="clsSgFareRule"  id="dvFareRuletbl_' + i + '" style="display:none" >';
                                }
                                for (var k = 0; k < new_arrkjjj[i].length ; k++) {

                                    str += '<div class="clsRuleHead" style="cursor:pointer;" id="dvcheck_' + new_arrkjjj[i][k].RULE.split('.')[0] + i + '" onclick="javascript:CloseSegWiseFareRule(this.id);">' + new_arrkjjj[i][k].RULE + ' <i class="fa fa-chevron-down"></i></div>';
                                    str += '<div style="width:100%;border-bottom: 1px dashed;">';
                                    str += '<pre readonly="readonly" class="clsFaretextArea" id="' + new_arrkjjj[i][k].RULE.split('.')[0] + i + '" style="display: none;">' + new_arrkjjj[i][k].TEXT + '</pre>';
                                    str += '</div>';
                                }
                                str += '</div>';

                            }

                            str += '<div class="clsSpanText"><span class="clsHeadNote">Note : </span>' + Resultss["IMPORTANT_NOTE"].NOTE + '</div>';
                        }

                        //$('#dvBookdata').hide();
                        $("#dvPaymentdata").hide();
                        $('#dvinnerFaRe').html("<pre>" + str + "</pre>");
                        CallFarepopup("Fare Rule", "modal-FarePopup", "700");

                    }
                    else {
                        showerralert(data.error + "-(#05)", "", "");
                    }
                }
            }

            catch (ex) {
                $.unblockUI();
                showerralert("Problem occured while getting fare rules (#05)", "", "");
            }
        },
        error: function (e) {
            $.unblockUI();
            $('#modal-FarePopup').iziModal('close');
            $('#sp_pnr_details').html("");
            //document.getElementById('btn_ticketing').style.display = 'none';
            $('.wholeclassdiv').hide();
            //Lobibox.alert('info', {
            //    msg: 'Inter server error!',
            //    closeOnEsc: false,
            //    callback: function ($this, type) {
            //    }
            //});
            showerralert("Inter server error!", "", "");
            return false;
        }
    });
}

function ShowSegWiseFareRule(id) {
    $('.clsSgFareRule').css("display", "none");
    $('#dvFareRuletbl_' + id).css("display", "block");
    $('.clsAllFarerule').removeClass('clsFareruleSelect').removeClass('clsFarerule');
    $('.clsAllFarerule').addClass('clsFarerule');
    $('#spnFareSelect' + id).removeClass('clsFarerule').addClass('clsFareruleSelect');
}

function CloseSegWiseFareRule(id) {
    var Id = id.split('_')[1];
    $('#' + Id).toggle(500);
    $('#' + Id).focusin();
    if ($('#dvcheck_' + Id + ' i').hasClass('fa-chevron-down')) {
        $('#dvcheck_' + Id + ' i').removeClass('fa-chevron-down').addClass('fa-chevron-up');
    }
    else if ($('#dvcheck_' + Id + ' i').hasClass('fa-chevron-up')) {
        $('#dvcheck_' + Id + ' i').removeClass('fa-chevron-up').addClass('fa-chevron-down');
    }
}

function ShowBookdata() {
    $('#dvBookdata').show();
    $('#dvinnerFaRe').html("");
}

$(document).on('change', 'input[name="farecheck"]', function () {
    var arg = this.id;
    var lbl_normalID = arg.replace("dv_normal", "lbl_normal");
    //var anc_normalID = arg.replace("dv_normal", "anchor");
    $('.clsaa').css("color", "#000");
    $('#popupfare').html($('#' + lbl_normalID).html());
    $('#spnPopupFare').html($('#' + lbl_normalID).html());
    $('#hdn_farequalifier').val($('#' + arg).data("farequalifier"));
    $('#hdn_TSTReference').val($('#' + arg).data("tstreference"));
    $('#hdnCorporateCode').val($('#' + arg).data("corporatecode"));
    $('#' + lbl_normalID).css("font-weight", "600");

    if (TerminalType == "T") {
        var farequalifier = $("#" + arg).attr('data-farequalifier');
        if (farequalifier == "N") {
            $(".chkclass").removeClass('col-md-offset-5').addClass('col-md-offset-2');
            $(".tourclass").show();
        } else {
            $("#txt_PricingCode").val("");
            $(".chkclass").removeClass('col-md-offset-2').addClass('col-md-offset-5');
            $(".tourclass").hide();
        }
    }

    Selectedbreakup($('#' + lbl_normalID).html());
});

function clearretrievepnr() {
    $(".wholeclassdiv").hide();
    $(".udk-totalfare-show").hide();
    $("#dvEarnings").html("");
    $("#dvCommdetails").html("");
    $("#dvshowmoreicn").hide();
    clearfaredetails();
}

$(document).on('input', '#txt_CRSpnr_Queue', function () {
    clearretrievepnr();
});

$(document).on('input', '#txt_corporateCode', function () {
    clearfaredetails();
});

function clearfaredetails() {
    $("#dvBookdata").hide();
    $("#dvshowmoreicn").hide();
}

function modalExp(msg, url) {

    $('#modal-alert').iziModal('destroy');
    $("#modal-alert").iziModal({
        title: msg,
        icon: 'fa fa-info',
        headerColor: '#5bbd72',//bd5b5b
        width: "500px",
        onClosed: function () {
            window.top.location = url;
        }
    });
    $('#modal-alert').iziModal('open');

}

function asyncafterbooking(bookresponse) {

    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: Asyncbookingresponse_update,
        data: JSON.stringify({ Bookresponse: bookresponse }),
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

function Selectedbreakup(selectedfare) {
    //$("#dvSelectedpax").show();
    try {

        var paxdet = selectedpaxdetails;
        var columns = [];
        var records = [];
        var columngroups = [];

        var faredet = $.grep(faredetailsarray, function (obj, val) {
            return obj.GROSSFARE == selectedfare;
        });

        //if (selectedfare == $("#spnTotalgrossfare").data("originalfare")) {
        //    faredet = [];
        //}

        var clsdisplay = "", clscolspan = "3", clsdisabled = "";
        if (TerminalType == "T") {
            //clsdisplay = "clsblock";
            //clscolspan = "3";
            clsdisabled = "";
        }
        else {
            //clsdisplay = "clsnone";
            //clscolspan = "1";
            clsdisabled = "disabled";
        }

        //region incentive disabled for same agent id
        var clsincdisabled = "";
        if (boolresofficeid) {
            clsincdisabled = "";            
        }
        else {
            clsincdisabled = "disabled style='cursor:no-drop;background:#CCC!important;'";
        }
        //end region

        var sbcommdetails = "<table id='table4' class='table no-more-tables clscenter' style='display: block;overflow-x: auto;white-space: nowrap;'> <thead>";
        sbcommdetails += "<tr>";
        sbcommdetails += "<th class=''>Title</th>";
        sbcommdetails += "<th class=''>Name</th>";
        sbcommdetails += "<th class=''>Type</th>";
        sbcommdetails += "<th class=''>Sector</th>";
        sbcommdetails += "<th class='tdright'>Base Amount</th>";
        sbcommdetails += "<th class='tdright'>YQ/YR</th>";
        sbcommdetails += "<th class='tdright'>Tax Amount</th>";
        sbcommdetails += "<th class='tdright'>Gross Amount</th>";
        sbcommdetails += "<th class='' colspan='" + clscolspan + "'>Commission</th>";
        sbcommdetails += "<th class='' colspan='" + clscolspan + "'>PLB</th>";
        sbcommdetails += "<th class='' colspan='2'>Markup</th>";
        sbcommdetails += "<th class=''>Service Fee</th>";
        sbcommdetails += "<th class=''>Incentives</th>";
        sbcommdetails += "<th class=''></th>";
        sbcommdetails += "</tr>";
        sbcommdetails += "</thead><tbody>";
        var adultcount = 1, childcount = 1, infantcount = 1, paxcount = 1;
        var commselect = "", plbselect = "";
        for (var i = 0; i < paxdet.length; i++) {
            var taxbreakup = [], taxamount = 0;
            var YRAmount = "0", YQAmount = "0";
            //New fare
            if (faredet.length > 0) {
                if (paxdet[i].PAXTYPE == "ADT") {
                    paxdet[i].BASEAMT = faredet[0].ADTBASEFARE;
                    paxdet[i].GROSSAMT = faredet[0].ADTGROSSFARE;
                    paxdet[i].TAXBREAKUP = faredet[0].ADTTAXBREAKUP;
                    taxbreakup = faredet[0].ADTTAXBREAKUP.split('/');
                }
                else if (paxdet[i].PAXTYPE == "CHD") {
                    paxdet[i].BASEAMT = faredet[0].CHDBASEFARE;
                    paxdet[i].GROSSAMT = faredet[0].CHDGROSSFARE;
                    paxdet[i].TAXBREAKUP = faredet[0].CHDTAXBREAKUP;
                    taxbreakup = faredet[0].CHDTAXBREAKUP.split('/');
                }
                else {
                    paxdet[i].BASEAMT = faredet[0].INFBASEFARE;
                    paxdet[i].GROSSAMT = faredet[0].INFGROSSFARE;
                    paxdet[i].TAXBREAKUP = faredet[0].INFTAXBREAKUP;
                    taxbreakup = faredet[0].INFTAXBREAKUP.split('/');
                }

                $.each(taxbreakup, function (obj, val) {
                    if (val != null && val != "" && val != "0") {
                        taxamount += parseFloat(val.split(':')[1]);
                    }
                });
                paxdet[i].TOTALTAXAMT = taxamount;
            }
            //New fare

            taxbreakup = paxdet[i].TAXBREAKUP.split('/');
            $.each(taxbreakup, function (obj, val) {
                if (val != null && val != "") {
                    if (val.split(':')[0] == "YQ") {
                        YQAmount = val.split(':')[1];
                    }
                    if (val.split(':')[0] == "YR") {
                        YRAmount = val.split(':')[1];
                    }
                }
            });

            var commEarningformat = (paxdet[i].EARNINGS_REF_ID != "" && paxdet[i].EARNINGS_REF_ID != null) ? paxdet[i].EARNINGS_REF_ID.split('REFSPLIT')[0] : "";
            var plbEarningformat = (paxdet[i].EARNINGS_REF_ID != "" && paxdet[i].EARNINGS_REF_ID != null) ? paxdet[i].EARNINGS_REF_ID.split('REFSPLIT')[1] : "";
            var incEarningformat = (paxdet[i].EARNINGS_REF_ID != "" && paxdet[i].EARNINGS_REF_ID != null) ? paxdet[i].EARNINGS_REF_ID.split('REFSPLIT')[2] : "";

            var commEarningarr = commEarningformat.split('|');
            var plbEarningarr = plbEarningformat.split('|');
            var commpercent = "0", commamount = "0", plbpercent = "0", plbamount = "0";
            commselect = "", plbselect = "";
            $.each(commEarningarr, function (obj, val) {
                if (val != "" && val != null) {
                    if (val.split(':')[0] == "BF" && val.split(':')[1] != "0") {
                        commselect = val.split(':')[0];
                        commpercent = val.split(':')[1];
                        commamount = GetEarningsload(commEarningformat, "BF:" + paxdet[i].BASEAMT + "/" + paxdet[i].TAXBREAKUP, '0');//0-Default Load,1-Onchange
                        return false;
                    }
                    if (val.split(':')[0] == "FL" && val.split(':')[1] != "0") {
                        commselect = val.split(':')[0];
                        commamount = GetEarningsload(commEarningformat, "BF:" + paxdet[i].BASEAMT + "/" + paxdet[i].TAXBREAKUP, '0');//0-Default Load
                        commpercent = val.split(':')[1];
                        return false;
                    }
                    if (val.split(':')[0] == "YQ" && val.split(':')[1] != "0") {
                        commselect = val.split(':')[0];
                        commamount = GetEarningsload(commEarningformat, "BF:" + paxdet[i].BASEAMT + "/" + paxdet[i].TAXBREAKUP, '0');//0-Default Load
                        commpercent = val.split(':')[1];
                        return false;
                    }
                }
            });

            $.each(plbEarningarr, function (obj, val) {
                if (val != "" && val != null) {
                    if (val.split(':')[0] == "NNB" && val.split(':')[1] != "0") {
                        plbselect = val.split(':')[0];
                        plbpercent = val.split(':')[1];
                        plbamount = GetEarningsload(plbEarningformat, "BF:" + paxdet[i].BASEAMT + "/" + paxdet[i].TAXBREAKUP, '0');//0-Default Load
                        return false;
                    }
                    if (val.split(':')[0] == "NNBYQ" && val.split(':')[1] != "0") {
                        plbselect = val.split(':')[0];
                        plbpercent = val.split(':')[1];
                        plbamount = GetEarningsload(plbEarningformat, "BF:" + paxdet[i].BASEAMT + "/" + paxdet[i].TAXBREAKUP, '0');//0-Default Load
                        return false;
                    }
                    if (val.split(':')[0] == "YR" && val.split(':')[1] != "0") {
                        plbselect = val.split(':')[0];
                        plbpercent = val.split(':')[1];
                        plbamount = GetEarningsload(plbEarningformat, "BF:" + paxdet[i].BASEAMT + "/" + paxdet[i].TAXBREAKUP, '0');//0-Default Load
                        return false;
                    }
                    if (val.split(':')[0] == "FL" && val.split(':')[1] != "0") {
                        plbselect = val.split(':')[0];
                        plbamount = GetEarningsload(plbEarningformat, "BF:" + paxdet[i].BASEAMT + "/" + paxdet[i].TAXBREAKUP, '0');//0-Default Load
                        plbpercent = val.split(':')[1];
                        return false;
                    }
                    if (val.split(':')[0] == "GR" && val.split(':')[1] != "0") {
                        plbselect = val.split(':')[0];
                        plbamount = GetEarningsload(plbEarningformat, "BF:" + paxdet[i].BASEAMT + "/" + paxdet[i].TAXBREAKUP, '0');//0-Default Load
                        plbpercent = val.split(':')[1];
                        return false;
                    }
                }
            });
            //
            var paxtype = "";
            if (paxdet[i].PAXTYPE == "ADT") {
                paxtype = "Adult";
                paxcount = adultcount;
            }
            else if (paxdet[i].PAXTYPE == "CHD") {
                paxtype = "Child";
                paxcount = childcount;
            }
            else {
                paxtype = "Infant";
                paxcount = infantcount;
                clsdisabled = "disabled";
            }

            var StrFareFlag = $("body").data("StrFareFlag");
            paxdet[i].TSTCOUNT = (StrFareFlag == "Y" ? paxdet[i].TSTCOUNT : paxdet[i].TICKETINGCARRIER);

            sbcommdetails += "<tr class='clspaxbreakup' id='txt_breakup_" + paxdet[i].TSTCOUNT + "_" + paxdet[i].PAXTYPE + "_" + paxcount + "' data-breakup='BF:" + paxdet[i].BASEAMT + "/" + paxdet[i].TAXBREAKUP + "' data-paxno='" + paxdet[i].PAXNO + "'>";
            sbcommdetails += "<td class='' style='width:50px;'>" + paxdet[i].TITLE + "</td>";
            sbcommdetails += "<td class=''>" + paxdet[i].FIRSTNAME + " " + paxdet[i].LASTNAME + "</td>"
            sbcommdetails += "<td class='tdright' style='width:70px;'>" + paxtype + "</td>";
            sbcommdetails += "<td class='tdright' style='width:70px;'>" + paxdet[i].SECTOR + "</td>";
            sbcommdetails += "<td class='tdright' style='width:110px;'>" + paxdet[i].BASEAMT + "</td>";
            sbcommdetails += "<td class='tdright' style='width:100px;'>" + YQAmount + "/" + YRAmount + "</td>";
            sbcommdetails += "<td class='tdright' style='width:100px;'>" + paxdet[i].TOTALTAXAMT + "</td>";
            sbcommdetails += "<td class='tdright' style='width:120px;'>" + paxdet[i].GROSSAMT + "</td>";
            //Commission
            sbcommdetails += "<td class='tdright " + clsdisplay + "' style='width:100px;'><select class='txt-anim clscommtype' " + clsdisabled + "  onchange=loadpercentage(this.id,'COM'); id='ddl_Comm_" + paxdet[i].TSTCOUNT + "_" + paxdet[i].PAXTYPE + "_" + paxcount + "' >";
            sbcommdetails += "<option value=''>--Select--</option>";
            sbcommdetails += "<option value='BF'>Base</option>";
            sbcommdetails += "<option value='YQ'>Base+YQ</option>";
            sbcommdetails += "<option value='FL'>Fixed Amount</option>";
            sbcommdetails += "</select></td>";//comrefsb
            sbcommdetails += "<td class='tdright " + clsdisplay + "' style='width:100px;'><input type='text' maxlength='4' class='txt-anim clscommper clscomdetails' " + clsdisabled + " style='width:50px;' id='txt_Comm_" + paxdet[i].TSTCOUNT + "_" + paxdet[i].PAXTYPE + "_" + paxcount + "' value='" + commpercent + "' /></td>";//onblur='GetCommission(this.id);' // onkeypress=GetEarnings(this.id,'COM'); 
            sbcommdetails += "<td class='tdright' style='width:100px;'><input type='text' class='txt-anim clsApproval clscommamt' " + clsdisabled + " style='width:70px;' disabled id='amt_Comm_" + paxdet[i].TSTCOUNT + "_" + paxdet[i].PAXTYPE + "_" + paxcount + "' data-oldvalue='" + commamount + "' data-comm='" + commamount + "' value='" + commamount + "' /></td>";//paxdet[i].COMMISSION
            //PLB
            sbcommdetails += "<td class='tdright " + clsdisplay + "' style='width:100px;'><select class='txt-anim clsplbtype' " + clsdisabled + "  onchange=loadpercentage(this.id,'PLB'); id='ddl_plb_" + paxdet[i].TSTCOUNT + "_" + paxdet[i].PAXTYPE + "_" + paxcount + "'  >";
            sbcommdetails += "<option value=''>--Select--</option>";
            sbcommdetails += "<option value='NNB'>NNBase</option>";
            sbcommdetails += "<option value='NNBYQ'>NNBase+YQ</option>";
            sbcommdetails += "<option value='YR'>Base+YR</option>";
            sbcommdetails += "<option value='GR'>Gross</option>";
            sbcommdetails += "<option value='FL'>Fixed Amount</option>";
            sbcommdetails += "</select></td>";//plbrefsb
            sbcommdetails += "<td class='tdright " + clsdisplay + "' style='width:100px;'><input type='text' maxlength='4' class='txt-anim clsplbper clscomdetails' " + clsdisabled + " style='width:50px;' id='txt_plb_" + paxdet[i].TSTCOUNT + "_" + paxdet[i].PAXTYPE + "_" + paxcount + "' value='" + plbpercent + "' /></td>";//onblur='GetPLB(this.id);'// onkeypress=GetEarnings(this.id,'PLB');
            sbcommdetails += "<td class='tdright' style='width:100px;'><input type='text' class='txt-anim clsplbamt clsApproval' " + clsdisabled + " style='width:70px;' disabled id='amt_plb_" + paxdet[i].TSTCOUNT + "_" + paxdet[i].PAXTYPE + "_" + paxcount + "' data-oldvalue='" + plbamount + "' data-plb='" + plbamount + "' value='" + plbamount + "' /></td>";//paxdet[i].PLB
            //Markup
            sbcommdetails += "<td class='tdright' style='width:100px;'><select class='txt-anim clsmarktype' id='ddl_mark_" + paxdet[i].TSTCOUNT + "_" + paxdet[i].PAXTYPE + "_" + paxcount + "'  >";//onchange=loadpercentage(this.id,'MAR'); 
            sbcommdetails += "<option value=''>--Select--</option>";
            sbcommdetails += "<option value='BF'>Add in Base Fare</option>";
            sbcommdetails += "<option value='TX'>Add in Taxes</option>";
            sbcommdetails += "</select></td>";//markrefsb
            //sbcommdetails += "<td class='tdright' style='width:100px;'><input type='text' class='txt-anim clscomdetails' " + clsdisabled + " style='width:50px;' id='txt_mark_" + paxdet[i].TSTCOUNT + "_" + paxdet[i].PAXTYPE + "_" + paxcount + "' value='" + 0 + "' /></td>";
            sbcommdetails += "<td class='tdright' style='width:100px;'><input type='text' maxlength='6' class='txt-anim clscomdetails clsmarkamt' " + clsdisabled + " style='width:70px;' id='amt_mark_" + paxdet[i].TSTCOUNT + "_" + paxdet[i].PAXTYPE + "_" + paxcount + "' data-markup='" + paxdet[i].MARKUP + "' value='" + paxdet[i].MARKUP + "' /></td>";

            sbcommdetails += "<td class='tdright' style='width:100px;'><input type='text' maxlength='6' class='txt-anim clscomdetails clsserfee' " + clsdisabled + " id='txt_sf_" + paxdet[i].TSTCOUNT + "_" + paxdet[i].PAXTYPE + "_" + paxcount + "' data-servicefee='" + paxdet[i].SERVICE_FEE + "' value='" + paxdet[i].SERVICE_FEE + "' /></td>";
            sbcommdetails += "<td class='tdright' style='width:100px;'><input type='text' maxlength='6' class='txt-anim clscomdetails clsApproval clsincentive' " + clsdisabled + " " + clsincdisabled + " id='txt_inc_" + paxdet[i].TSTCOUNT + "_" + paxdet[i].PAXTYPE + "_" + paxcount + "' data-oldvalue='" + paxdet[i].INCENTIVES + "' data-incentives='" + paxdet[i].INCENTIVES + "' value='" + paxdet[i].INCENTIVES + "' /></td>";

            //if (paxcount == 1 && paxdet[i].PAXTYPE != "INF") {
            sbcommdetails += "<td><input type='button' style='width:100px;' onclick='PasteEarnings(this.id," + paxcount + ");' class='btn btn-primary'  id='btn_" + paxdet[i].TSTCOUNT + "_" + paxdet[i].PAXTYPE + "' value='Apply All' /></td>";
            //}

            sbcommdetails += "</tr>";
            if (paxdet[i].PAXTYPE == "ADT") {
                adultcount++;
            }
            else if (paxdet[i].PAXTYPE == "CHD") {
                childcount++;
            }
            else {
                infantcount++;
            }
        }
        sbcommdetails += "</tbody></table>";

        $('#dvSelectedCommdetails').html(sbcommdetails);
        $('.clscommtype').each(function () {
            $(this).val(commselect);
            var loadpercentid = this.id.replace('ddl', 'txt');
            var loadamountid = this.id.replace('ddl', 'amt');
            var loadpercentvalue = $("#" + loadpercentid).val();
            var loadamountvalue = $("#" + loadamountid).val();
            loadpercentage(this.id, "COM")
            $("#" + loadpercentid).val(loadpercentvalue);
            $("#" + loadamountid).val(loadamountvalue)
        });
        $('.clsplbtype').each(function () {
            $(this).val(plbselect);
            var loadpercentid = this.id.replace('ddl', 'txt');
            var loadamountid = this.id.replace('ddl', 'amt');
            var loadpercentvalue = $("#" + loadpercentid).val();
            var loadamountvalue = $("#" + loadamountid).val();
            loadpercentage(this.id, "PLB")
            $("#" + loadpercentid).val(loadpercentvalue);
            $("#" + loadamountid).val(loadamountvalue)
        });
    }
    catch (e) {
        console.log(e.message);
    }
}

function loadpercentage(id, Flag) {
    var loadpercentid = id.replace('ddl', 'txt');
    var loadamountid = id.replace('ddl', 'amt');
    $('#' + loadpercentid).val("0");
    $('#' + loadamountid).val("0");
    //$('#' + loadpercentid).val($('#' + id).val());
    if (Flag == "COM") {
        var plbtypeid = id.replace('Comm', 'plb');
        var plbtype = $('#' + plbtypeid).val();
        var plbperid = loadpercentid.replace('Comm', 'plb');
        var plbamtid = loadamountid.replace('Comm', 'plb');
        if (plbtype == "NNB" || plbtype == "NNBYQ") {
            $('#' + plbtypeid).val("");
            $('#' + plbperid).val("0");
            $('#' + plbamtid).val("0");
        }
    }

    if ($('#' + id).val() == "FL") {
        $('#' + loadpercentid).attr('maxlength', '6');
    }
    else {
        $('#' + loadpercentid).attr('maxlength', '4');
    }
}

function showretrievedetails() {
    $('.clscheckfarehide').slideToggle();
    if ($('#dvshowicon').hasClass("fa-angle-double-down")) {
        $('#dvshowicon').removeClass("fa-angle-double-down").addClass("fa-angle-double-up");
    }
    else {
        $('#dvshowicon').removeClass("fa-angle-double-up").addClass("fa-angle-double-down");
    }
}

function GetPLB(id) {

    try {
        var typeid = id.replace('txt', 'ddl');
        var amountid = id.replace('txt', 'amt');

        var taxbreakupid = id.split('_').slice(2, 5).join('_');
        var percentage = $('#' + id).val();
        var Earntype = $('#' + typeid).val();

        var strEarningsTaxBreakup = $('#txt_breakup_' + taxbreakupid).data("breakup");
        var ReferenceID = Earntype + ":" + percentage;
        if (Earntype == "") {
            return false;
        }
        if (percentage == "") {
            $('#' + amountid).val("0");
            return false;
        }
        //
        var commtypeid = typeid.replace('plb', 'Comm');
        var commperid = id.replace('plb', 'Comm');
        var commtype = $('#' + commtypeid).val();
        var commpercentage = $('#' + commperid).val();

        if ((Earntype == "NNB" && commtype == "BF") || (Earntype == "NNBYQ" && commtype == "YQ")) {
            ReferenceID += ("|" + commtype + ":" + commpercentage);
        }
        //

        var Earningamount = GetEarningsload(ReferenceID, strEarningsTaxBreakup, '1');
        $('#' + amountid).val(Earningamount);
        var showflag = false;
        $('.clsApproval').each(function () {
            if (Number($(this).val()) > Number($(this).data('oldvalue')) && APP_Terminal == "T") {
                showflag = true;
            }
        });
        if (showflag) {
            $('.Approvalclass').show();
            $(".chkclass").removeClass('col-md-offset-2')
        }
        else {
            $('.Approvalclass').hide();
            $(".chkclass").addClass('col-md-offset-2') ``
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function GetCommission(id) {//,Flag//GetEarnings

    try {
        var typeid = id.replace('txt', 'ddl');
        var amountid = id.replace('txt', 'amt');
        var taxbreakupid = id.split('_').slice(2, 5).join('_');
        var percentage = $('#' + id).val();
        var Earntype = $('#' + typeid).val();
        var strEarningsTaxBreakup = $('#txt_breakup_' + taxbreakupid).data("breakup");
        var ReferenceID = Earntype + ":" + percentage;
        if (Earntype == "") {
            return false;
        }
        if (percentage == "") {
            $('#' + amountid).val("0");
            return false;
        }
        //COM
        var plbtypeid = typeid.replace('Comm', 'plb');
        var plbpercid = id.replace('Comm', 'plb');
        var plbtype = $('#' + plbtypeid).val();
        //COM

        var Earningamount = GetEarningsload(ReferenceID, strEarningsTaxBreakup, '1');
        $('#' + amountid).val(Earningamount);
        var showflag = false;
        $('.clsApproval').each(function () {
            if (Number($(this).val()) > Number($(this).data('oldvalue')) && APP_Terminal == "T") {
                showflag = true;
            }
        });
        if (showflag) {
            $('.Approvalclass').show();
            $(".chkclass").removeClass('col-md-offset-2')
        }
        else {
            $('.Approvalclass').hide();
            $(".chkclass").addClass('col-md-offset-2') ``
        }
        if ((Earntype == "BF" && plbtype == "NNB") || (Earntype == "YQ" && plbtype == "NNBYQ")) {
            GetPLB(plbpercid);
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function GetEarningsload(ReferenceID, strEarningsTaxBreakup, Flag) {

    try {
        var DecimalFlag = "Y";
        var Earningamount = 0;

        var BFPercent = 0;
        var YQPercent = 0;
        var YRPercent = 0;
        var JNPercent = 0;
        var WOPercent = 0;
        var GRPercent = 0;
        var NNBPercent = 0;
        var NNBYQPercent = 0;

        var FLAmount = 0;
        var BFAmonut = 0;
        var YQAmonut = 0;
        var YRAmonut = 0;
        var JNAmonut = 0;
        var WOAmonut = 0;
        var GRAmonut = 0;
        var GRamt = 0;
        var TXamt = 0;
        var TaxAmount = 0;
        var NNBAmount = 0;
        var NNBYQAmount = 0;
        var NNBFlag = "0", NNBYQFlag = "0";
        var ArrCal = ReferenceID.split('|');

        $.each(ArrCal, function (obj, val) {
            if (val != null && val != "" && val.indexOf(":") != -1) {
                var TypeFare = val.split(':');
                if (TypeFare.length == 2) {
                    if (TypeFare[0].toString().toUpperCase().trim() == "BF") {
                        BFPercent = parseFloat(TypeFare[1]);
                    }
                    else if (TypeFare[0].toString().toUpperCase().trim() == "YQ") {
                        YQPercent = parseFloat(TypeFare[1]);
                    }
                    else if (TypeFare[0].toString().toUpperCase().trim() == "YR") {
                        YRPercent = parseFloat(TypeFare[1]);
                    }
                    else if (TypeFare[0].toString().toUpperCase().trim() == "JN") {
                        JNPercent = parseFloat(TypeFare[1]);
                    }
                    else if (TypeFare[0].toString().toUpperCase().trim() == "WO") {
                        WOPercent = parseFloat(TypeFare[1]);
                    }
                    else if (TypeFare[0].toString().toUpperCase().trim() == "FL") {
                        FLAmount = parseFloat(TypeFare[1]);
                    }
                    else if (TypeFare[0].toString().toUpperCase().trim() == "GR") {
                        GRPercent = parseFloat(TypeFare[1]);
                    }
                    else if (TypeFare[0].toString().toUpperCase().trim() == "NNB") {
                        NNBPercent = parseFloat(TypeFare[1]);
                        NNBFlag = "1";
                    }
                    else if (TypeFare[0].toString().toUpperCase().trim() == "NNBYQ") {
                        NNBYQPercent = parseFloat(TypeFare[1]);
                        NNBYQFlag = "1";
                    }
                }
            }
        });

        var ArrBreak = strEarningsTaxBreakup.split('/');

        $.each(ArrBreak, function (obj, val) {
            if (val != null && val != "" && val.indexOf(":") != -1) {
                var TypeFare = val.split(':');
                if (TypeFare.length == 2) {
                    if (TypeFare[0].toString().toUpperCase().trim() == "BF") {
                        BFAmonut = parseFloat(TypeFare[1]);
                    }
                    else if (TypeFare[0].toString().toUpperCase().trim() == "YQ") {
                        YQAmonut = parseFloat(TypeFare[1]);
                    }
                    else if (TypeFare[0].toString().toUpperCase().trim() == "YR") {
                        YRAmonut = parseFloat(TypeFare[1]);
                    }
                    else if (TypeFare[0].toString().toUpperCase().trim() == "JN") {
                        JNAmonut = parseFloat(TypeFare[1]);
                    }
                    else if (TypeFare[0].toString().toUpperCase().trim() == "WO") {
                        WOAmonut = parseFloat(TypeFare[1]);
                    }
                    else if (TypeFare[0].toString().toUpperCase().trim() == "GR") {
                        GRAmonut = parseFloat(TypeFare[1]);
                    }
                    if (TypeFare[0].toString().toUpperCase().trim() == "BF") {

                    }
                    else {
                        TXamt = parseFloat(TypeFare[1]);
                        TaxAmount += TXamt;
                    }
                    GRamt = parseFloat(TypeFare[1]);
                    GRAmonut += GRamt;
                }
            }
        });

        //
        if (NNBFlag == "1") {
            NNBAmount = ((BFAmonut - ((BFAmonut / 100) * BFPercent)) / 100) * NNBPercent;
            if (Flag == "1") {
                NNBAmount = (DecimalFlag == "Y" ? NNBAmount.toFixed(2) : Math.round(NNBAmount));
                return NNBAmount;
            }
        }

        if (NNBYQFlag == "1") {
            NNBYQAmount = ((BFAmonut + YQAmonut - (((BFAmonut + YQAmonut) / 100) * YQPercent)) / 100) * NNBYQPercent;
            if (Flag == "1") {
                NNBYQAmount = (DecimalFlag == "Y" ? NNBYQAmount.toFixed(2) : Math.round(NNBYQAmount));
                return NNBYQAmount;
            }
        }

        Earningamount = (((BFAmonut / 100) * BFPercent) + (((BFAmonut + YQAmonut) / 100) * YQPercent) + ((GRAmonut / 100) * GRPercent) + (((BFAmonut + YRAmonut) / 100) * YRPercent) + FLAmount + NNBAmount + NNBYQAmount);
        Earningamount = (DecimalFlag == "Y" ? Earningamount.toFixed(2) : Math.round(Earningamount));
        //
    }
    catch (e) {
        console.log(e.message);
    }
    return Earningamount;
}

function PasteEarnings(id, slctcnt) {
    try {
        var TST = id.split('_')[1];
        var paxtype = id.split('_')[2];
        var commtype = $('#ddl_Comm_' + TST + '_' + paxtype + '_' + slctcnt).val();
        var commper = $('#txt_Comm_' + TST + '_' + paxtype + '_' + slctcnt).val();
        var plbtype = $('#ddl_plb_' + TST + '_' + paxtype + '_' + slctcnt).val();
        var plbper = $('#txt_plb_' + TST + '_' + paxtype + '_' + slctcnt).val();
        var commamount = $('#amt_Comm_' + TST + '_' + paxtype + '_' + slctcnt).val();
        var plbamount = $('#amt_plb_' + TST + '_' + paxtype + '_' + slctcnt).val();
        $('.clscommtype').val(commtype);
        $('.clscommper').val(commper);
        $('.clsplbtype').val(plbtype);
        $('.clsplbper').val(plbper);
        $('.clsmarktype').val($('#ddl_mark_' + TST + '_' + paxtype + '_' + slctcnt).val());
        $('.clsmarkamt').val($('#amt_mark_' + TST + '_' + paxtype + '_' + slctcnt).val());
        $('.clsserfee').val($('#txt_sf_' + TST + '_' + paxtype + '_' + slctcnt).val());
        $('.clsincentive').val($('#txt_inc_' + TST + '_' + paxtype + '_' + slctcnt).val());
        if (commamount == "0") {
            $('.clscommamt').val(commamount);
        }
        if (plbamount == "0") {
            $('.clsplbamt').val(plbamount);
        }

        $('.clspaxbreakup').each(function () {
            var id = this.id.split('_').slice(2, 5).join('_');
            var plbpercid = "txt_plb_" + id;
            var commperid = "txt_Comm_" + id;
            GetCommission(commperid);
            GetPLB(plbpercid);
        });
    }
    catch (e) {
        console.log(e.message);
    }
}

function ShowPccdetails() {
    clearfaredetails();
    var Param = {
        strOfficeID: $("#ddl_OfficeID").val().trim(),
        strCRSPNR: $("#txt_CRSpnr_Queue").val().trim()
    };
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: PCCDetailsURL,
        data: JSON.stringify(Param),
        dataType: "json",
        success: function (data) {

            //if (data.Result == "-1") {
            //    location.href = SessionRedirectURL;
            //}

            if (data.Result != "" && data.Result != null) {
                var jsondata = JSON.parse(data.Result);
                var arr = jsondata.CREDENTIALS;
                var str = "";
                if (arr.length > 0) {
                    str += "<table id='table6' class='table table-responsive no-more-tables' style='border: 1px solid #ccc;margin:0px !important;'>";
                    str += "<thead><tr>";
                    $.each(arr[0], function (obj, val) {
                        str += "<th>" + obj + "</th>";
                    });
                    str += "</th></thead><tbody>";
                    for (var i = 0; i < arr.length; i++) {
                        str += "<tr>";
                        $.each(arr[i], function (obj, val) {
                            str += "<td>" + val + "</td>";
                        });
                        str += "</tr>";
                    }
                    str += "</tbody></table>";
                    $('#dvtablePcc').html(str);
                    $('#dvPccDetails').show();
                }
                else {
                    $('#dvtablePcc').html('');
                    $('#dvPccDetails').hide();
                }
            }
            else {
                $('#dvtablePcc').html('');
                $('#dvPccDetails').hide();
            }
        },
        error: function (e) {
            $.unblockUI();
            alert("Unable to connect remote server");
            return false;
        }
    });
}


$(document).on('input', '.clsApproval', function () {
    var showflag = false;
    $('.clsApproval').each(function () {
        if (Number($(this).val()) > Number($(this).data('oldvalue')) && APP_Terminal == "T") {
            showflag = true;
        }
    });
    if (showflag) {
        $('.Approvalclass').show();
        $(".chkclass").removeClass('col-md-offset-2')
    }
    else {
        $('.Approvalclass').hide();
        $(".chkclass").addClass('col-md-offset-2') ``
    }
});

function GetEmployeeDetails() {
    try {
        if ($('#txtcash_EmpCode').val().length < 6) {
            return false;
        }
        var param = {
            strEmployeeCode: $('#txtcash_EmpCode').val(),
        }
        $.blockUI({
            message: '<img alt="Please Wait..." src="' + loadurl + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
            css: { padding: '5px' },
            baseZ: 10000000
        });
        $.ajax({
            type: "POST", 		//GET or POST or PUT or DELETE verb
            url: GetEmployeeDetURL, 		// Location of the service                    
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(param),
            dataType: "json",
            success: function (json) {//On Successful service call
                var Response = json;
                if (Response.Status == "01") {
                    var data = JSON.parse(Response.Result);
                    var html = '';
                    html += '<table class="table table-striped text-left">';
                    html += '<tbody>';
                    html += '<tr>';
                    html += '<td><b>Employee ID</b><span style="float:right;">:</span></td>';
                    html += '<td>' + data["EmpCode"] + '</td>';
                    html += '</tr>';
                    html += '<tr>';
                    html += '<tr>';
                    html += '<td><b>Name</b><span style="float:right;">:</span></td>';
                    html += '<td>' + data["Name"] + '</td>';
                    html += '</tr>';
                    html += '<tr>';
                    html += '<td><b>Email</b><span style="float:right;">:</span></td>';
                    html += '<td>' + data["Email"] + '</td>';
                    html += '</tr>';
                    //html += '<tr>';
                    //html += '<td><b>Mobile No</b><span style="float:right;">:</span></td>';
                    //html += '<td>' + data["MobileNo"] + '</td>';
                    //html += '</tr>';
                    html += '<tr>';
                    html += '<td><b>Branch</b><span style="float:right;">:</span></td>';
                    html += '<td>' + data["Branch"] + '</td>';
                    html += '</tr>';
                    html += '</tbody>';
                    html += '</table>';
                    $("#dvEmployee").html(html);
                    $("#dvEmployee,#btnTicketingPopUp").show();
                    $("#hdncash_Empname").val(data["Name"]);
                    $("#hdncash_Empemail").val(data["Email"]);
                    $("#hdncash_Empmobile").val(data["MobileNo"]);
                    $("#hdncash_Empbranch").val(data["Branch"]);
                }
                else {
                    var Message = (Response.Message != null && Response.Message != "") ? Response.Message : "Unable to get employee details.please contact support team";
                    var html = '<span style="color:#fff;background: #ff0000;font-size: 14px;text-align:center;padding: 10px;">' + Message + '</span>';
                    $("#dvEmployee").html(html);
                    $("#dvEmployee").show();
                    $("#btnTicketingPopUp").hide();
                }
                $.unblockUI();
                return false;
            },
            error: function (e) {// When Service call fails      
                var html = '<span style="color:#fff;background: #ff0000;font-size: 14px;text-align:center;padding: 10px;">Unable to get employee details.please contact support team(#07)</span>';
                $("#dvEmployee").html(html);
                $("#dvEmployee").show();
                $("#btnTicketingPopUp").hide();
                $.unblockUI();
                return false;
            }
        });
    }
    catch (e) {
        showerralert("Unable to get employee details.please contact support team(#09)", "", "");
        return false;
    }
}

$(document).on('input', '#txtcash_EmpCode', function () {
    $("#dvEmployee").hide();
    $("#hdncash_Empname").val("");
    $("#hdncash_Empemail").val("");
    $("#hdncash_Empmobile").val("");
    $("#hdncash_Empbranch").val("");
    GetEmployeeDetails();
});

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

//Common For Both ticketing and accounting

var arr = [];
function LoadClient() {
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadurl + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });
    $.ajax({
        type: "POST",
        url: AgentLoadURL,//     //GetClientforRetrievePNR
        contentType: "application/json; charset=utf-8",
        timeout: 180000,
        dataType: "json",
        success: function (data) {
            $.unblockUI();
            try {
                var data = data["Data"];

                if (data.Status == "00") {
                    showerralert(data.Result, "", "");
                    console.log(data.Error);
                }

                if (data.Status == "01") {
                    arr = JSON.parse(data.Result);//
                    $('#ddlclient').typeahead('destroy');
                    $('#ddlclient').typeahead({
                        source: arr,
                        items: 5,
                        displayField: "CLTNAME",
                        valueField: "CLTCODE",
                        scrollBar: true,
                        onSelect: displayResults,
                    });

                    function displayResults(item) {
                        var selectedvalues = item.value;
                        $('#hdnClientID').val(selectedvalues);
                        $('#hdnBranchID').val(item.bid_s);
                        clearretrievepnr();
                        $('#ddlclient').addClass("has-content");
                        if ($("#hdnLoadOfficeID").length && $("#hdnLoadOfficeID").val() == "Y") {
                            LoadOfficeIdNew();
                        }
                    }
                }
                else {
                    $('#ddlclient').hide();
                    $('#styles-container h4:last').append("<span> No clients found <span>")
                }
            }
            catch (err) {
                console.log(err.message);
            }
        },
        error: function (e) {

            $.unblockUI();

            console.log(e.responseText);
            //alert("error");
        }
    });
}

var TotalOfficeIDDetails = [];

function LoadOfficeIdNew() {

    $.ajax({
        type: "POST",
        contentType: "Application/json; charset=utf-8",
        url: LoadOfficeIDURL,
        data: "{}",
        datatype: "json",
        success: function (data) {
            if (data.Status == "1") {
                var ResultData = JSON.parse(data.Result);
                TotalOfficeIDDetails = ResultData.P_INSERT_SUPPLIER_CRADENTIAL_PCC1;
                TotalOfficeIDDetails = $.grep(TotalOfficeIDDetails, function (v, j) {
                    return v.CCR_PRODUCT_CODE == "AIR";
                });
                BindOfficeID();
            }
        },
        error: function (e) {

        }
    });

}

function BindOfficeID(defualtofficeid, id) {
    id = id || "";
    defualtofficeid = defualtofficeid || "";
    if (TotalOfficeIDDetails.length) {
        var filtercrsid = $("#ddl_category").val() == "LCC" ? $("#ddl_AirlineStock").val().split("|")[0] : $("#ddl_QueueStock").val();
        var filterdofficeids = $.grep(TotalOfficeIDDetails, function (v, j) {
            return v.CCR_CRS_ID == filtercrsid;
        });
        var uniquary = [];
        var officeidblr = "<option value =''> -- Select --</option>";
        if (filterdofficeids.length) {
            $.each(filterdofficeids, function (cnt, officeidobj) {
                if (officeidobj.CCR_PCC != "")
                    if (uniquary.indexOf(officeidobj.CCR_PCC) == -1) {
                        uniquary.push(officeidobj.CCR_PCC);
                        officeidblr += '<option value="' + officeidobj.CCR_PCC + '" > ' + officeidobj.CCR_PCC + ' </option>';
                    }
            });
        }
        if (id != "") {
            if ($("#ddl_OfficeID_chosen").length)
                $('#ddl_OfficeID').chosen('destroy');

            $("#ddl_OfficeID").html(officeidblr).chosen();

            if (defualtofficeid != "") {
                defualtofficeid = defualtofficeid.split(",")[0];
                $("#ddl_OfficeID").val(defualtofficeid).trigger("chosen:updated");
            }
        }
        else {
            if ($('#ddlOfficeIdRetrieve_chosen').length)
                $('#ddlOfficeIdRetrieve').chosen('destroy');

            $("#ddlOfficeIdRetrieve").html(officeidblr).chosen();
        }
       
    }
}


function checkcatogory(arg) {
    var str_builder = '';
    var str_air_build = '';
    if (arg == 0) {
        str_builder += ' <input type="text" class="txt-anim clshideErr losefocus has-content" id="txt_CRSpnr_Queue" maxlength="7" onkeypress="javascript:return ValidateAlphaNumeric(event);" autocomplete="off" style="width: 100%; text-transform: uppercase">'
        str_builder += '<label for="txt_CRSpnr_Queue" class="colrcls">CRS PNR <span class="color-red-udk">*</span></label>';
        $(".udk-Category-cls").html(str_builder);

        str_air_build += '<select class="txt-anim clshideErr losefocus has-content" id="ddl_QueueStock" style="padding: 0 6px !important;width:100%;" onchange="Queuechange();">'
        str_air_build += '<option value="-1">--Select--</option>'
        str_air_build += '<option value="1A">Amadeus</option>'
        str_air_build += '<option value="1G">Galileo</option>'
        str_air_build += '</select>'
        str_air_build += '<label class="colrcls" for="ddl_QueueStock">CRS Name <span class="color-red-udk">*</span></label>';

        $(".udk-Airline-cls").html(str_air_build);

        //$(".udk-corporate-code").html('<input id="txt_corporateCode" onkeypress="javascript:return ValidateAlphaNumeric(event);" style="height: 34px; " class="txt-anim clshideErr losefocus has-content" type="text" maxlength="25" /><label for="txt_corporateCode" class="colrcls" >Corporate Pricing Code <span class="color-red-udk"></span></label>');

        // $("#ddl_QueueAT").val("-1");
        //$(".udk-office-id").show();//STS-166
        //LoadOfficeId("");
    }
    else {
        str_builder += ' <input type="text" class="txt-anim clshideErr losefocus has-content" id="txt_Airlinepnr_Queue" maxlength="7" onkeypress="javascript:return ValidateAlphaNumeric(event);" autocomplete="off" style="width: 100%; text-transform: uppercase">'
        str_builder += '<label for="txt_Airlinepnr_Queue" class="colrcls">Airline PNR <span class="color-red-udk">*</span></label>';
        $(".udk-Category-cls").html(str_builder);
        str_air_build += '<select class="txt-anim clshideErr losefocus has-content" id="ddl_AirlineStock" style="padding: 0 6px !important;width:100%;" onchange="BindOfficeID()">'
        str_air_build += '<option value="-1">--Select--</option>'
        str_air_build += '</select>'
        str_air_build += '<label class="colrcls" for="ddl_AirlineStock">Airline Name <span class="color-red-udk">*</span></label>';
        $(".udk-Airline-cls").html(str_air_build);
        //$(".udk-corporate-code").html('<input id="txt_PromoCode" onkeypress="javascript:return ValidateAlphaNumeric(event);" style="height: 34px; " class="txt-anim clshideErr losefocus has-content" type="text" maxlength="25" /><label for="txt_PromoCode" class="colrcls" >Promo Code <span class="color-red-udk"></span></label>');
        arrydomestic_ret = [];

        var aryvalload = "";
        aryvalload += '<option value="-1">--Select--</option>';
        var airporttype = "";
        for (var i = 0; i < Airdata1_ret.length; i++) {
            airporttype = $("#ddl_QueueAT").val();

            if (Airdata1_ret[i].care == "LCC") { //&& Airdata1_ret[i].airport_id == airporttype

                aryvalload += "<option  value=" + Airdata1_ret[i].id + '|' + Airdata1_ret[i].care + ">" + Airdata1_ret[i].value + "</option>";
                arrydomestic_ret.push(Airdata1_ret[i].id + '|' + Airdata1_ret[i].care);
            }
        }
        $("#ddl_AirlineStock").html(aryvalload);

        if ($('#ddl_AirlineStock').length) {
            $('#ddl_AirlineStock').chosen('destroy');
        }

        $('#ddl_AirlineStock').chosen();
        // $("#ddl_QueueAT").val("-1");
        //  LoadOfficeId("");
        //$(".udk-office-id").hide();
    }
}

var arrydomestic_ret = [];
var Airdata1_ret = "";
var Airlinedatacount_ret = "";

function InvokeAirlineXml() {
    //LoadOfficeId();
    $.ajax({
        type: "GET",
        url: AirlinenamesXML,
        async: false,
        dataType: "xml",
        success: function (xmlResponse) {

            Airdata1_ret = $("AIRLINEDET", xmlResponse).map(function () {
                return {
                    value: $("_NAME", this).text(),
                    id: $("_CODE", this).text(),
                    care: $("_CATEGORY", this).text(),
                    airport_id: $("AIRPORT_ID", this).text(),
                };
            }).get();
        }
    });
}

function ChangePaymentmode(arg)
{

    if ($(arg).val() == "B") {
        $(".dvclspassthroughdetails").show();
        if ($("#rdioriya").prop("checked")) {
            Fetchowncarddetails();
            $(".dvclsowncardaccoutdetails").show();
        }
    }
    else {
        $(".dvclspassthroughdetails").hide();
    }

    if ($(arg).val() == "H") {
        $("#dvcashpayment,#dvEmpDetails").show();
        $("#txtcash_Paymentdate").removeClass('hasDatepicker');
        var dadt = new Date;
        var day = dadt.getDate();
        var Month = dadt.getMonth() + 1;
        var Year = dadt.getFullYear() + 1;
        if (day < 10) { day = '0' + day } if (Month < 10) { Month = '0' + Month }
        var maxadtyearadt = day + "/" + Month + "/" + Year;
        var minadtyearadt = day + "/" + Month + "/" + dadt.getFullYear();
        $("#txtcash_Paymentdate").datepicker({
            numberOfMonths: 1,
            showButtonPanel: false,
            dateFormat: "dd/mm/yy",
            maxDate: maxadtyearadt,
            minDate: minadtyearadt,
        });
        //$("#txtcash_Paymentdate").val(minadtyearadt)
    }
    else {
        $("#dvcashpayment,#dvEmpDetails").hide();
        $(".clsClear").val("");
        $("#dvEmployee").hide();
    }
}
function Fetchowncarddetails() {
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadurl + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: FetchPassthroughnewURL,
        data: '{}',
        dataType: "json",
        success: function (data) {
            $.unblockUI();
            if (data.Status == "1") {
                var carddetails = JSON.parse(data.Result);
                carddetails = carddetails.Table;

                var carddetbldr = "<option value=''> Select </option>";
                $.each(carddetails, function (v, j) {
                    carddetbldr += '<option value=' + j.FOP_CARD_NUMBER + '> ' + j.FOP_CARD_NUMBER + ' </option>';
                });

                $("#ddlcardno").html(carddetbldr);
            }

        },
        error: function (result) {
            $.unblockUI();
            showerralert("Unable to connect remote server.", "", "");
            return false;
        },
    });
}

$(document).on('change', '.iprdnclscardtype', function () {
    if ($("#rdioriya").prop("checked")) {
        Fetchowncarddetails();
        $(".dvclsowncardaccoutdetails").show();
    }
    else {
        $(".dvclscorporatecarddetails").show();
        $(".dvclsowncardaccoutdetails").hide();
    }
});

$(document).on('change', '.rdnaccountcls', function () {
    if ($("#rdaccpg").prop("checked")) {
        $(".dvclspgtrackdetails").show().find("#txtpgrefnumber").val("");
    }
    else {
        $(".dvclspgtrackdetails").hide().find("#txtpgrefnumber").val("");
    }
});