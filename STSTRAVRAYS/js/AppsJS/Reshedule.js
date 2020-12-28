
var existpnrlist = "";
var RescheduleStatus = ""
function Reschedule_pnr() {
    loadcitytypexml();

    var reschedule_spnr = $("#reche_txt_viewSPNR")[0].value;
    reschedule_spnr = reschedule_spnr.replace(/\s+/, "");
    $("#rech_dvGrpBkngerr").removeClass("has_error");
    var reschedule_airline = $("#reche_txt_viewAirPNR")[0].value;
    reschedule_airline = reschedule_airline.replace(/\s+/, "");
    var reschedule_crs = $("#reche_txt_viewCSRPNR")[0].value;
    reschedule_crs = reschedule_crs.replace(/\s+/, "");
    if (reschedule_spnr == "" && reschedule_airline == "" && reschedule_crs == "") {
        Lobibox.alert("warning", {   //eg: 'error' , 'success', 'info', 'warning'
            msg: "Please enter any PNR no.",
            closeOnEsc: false,
            callback: function ($this, type) {
            }
        });
        //showError("Please enter any PNR no.", rech_dvGrpBkngerr, rech_dvGrpBkngerr_msg, "");
        $(".showdivreschedule").css("display", "none");
        $("#air_reschedule_table").css("display", "none");
        return false;
    }
    var paymentmode = "";//$("#reche_ddl_paymentmode").val();
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + Spinner + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });
    
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: get_reschedule_process,
        data: '{spnr: "' + reschedule_spnr + '" ,airlinepnr: "' + reschedule_airline + '",crspnr: "' + reschedule_crs + '",strPaymentmode:"' + paymentmode + '"}',//,RescheduleStatus: "' + $("#hdnPax")[0].value + '" ,BlockTicket: "' + BlockTicket + '",PaymentMode: "' + $("#ddlPaymode").val() + '",TourCode: "' + $("#Tour_Code").val() + '"}',
        //async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call
            var result = json.Result;
            if (result[2] != null && result[2] != "") {
                var loadpnr = result[2].split("|");
                $("#reche_txt_viewSPNR")[0].value = loadpnr[0];
                $("#reche_txt_viewAirPNR")[0].value = loadpnr[1];
                $("#reche_txt_viewCSRPNR")[0].value = loadpnr[2];
                $("#reche_txt_viewSPNR").addClass("has-content");
                $("#reche_txt_viewAirPNR").addClass("has-content");
                $("#reche_txt_viewCSRPNR").addClass("has-content");

            }
            if (result[1] != "" && result[1] != null) {
                $('#reschehide').css("display", "block");
                var appendiv = document.getElementById('air_reschedule_table').id;

                Createtable_Reschedule(result, appendiv, "Reschedule_table", "Reschedule_airline");
                //$('.clscmbanim').niceSelect();
                //$('.clscmbanim').css('border', '1px solid #27a5f5');
                var currentDate = "@ViewBag.ServerDateTime";
                var length = $(".segement").length;
                for (var i = 0; i < length; i++) {
                    $("#rechetxt_" + i).addClass("has-content");
                    $("#rechetxt_" + i).datepicker({
                        minDate: currentDate, dateFormat: "dd/mm/yy",
                        onSelect: function (arg) {
                            
                            var ids = this.id;
                            var cnt = ids.split("_")[1];
                            cnt = Number(cnt) + Number(1);
                            if ($("#rechetxt_" + cnt).length > 0) {
                                $("#rechetxt_" + cnt).val(arg)
                                $("#rechetxt_" + cnt).datepicker("destroy");
                                $("#rechetxt_" + cnt).datepicker({
                                    minDate: arg, dateFormat: "dd/mm/yy",
                                    onSelect: function (arg) {
                                        
                                        var ids = this.id;
                                    }
                                });
                            }
                        }
                    });
                    // $("#rechetxt_" + i).val(currentDate);
                    //$("#rechetxt_" + i).val($("#DEPT_DATE" + i).data('olddate'));
                }
                $("#air_reschedule_table").slideDown();
                $.unblockUI();
            }
            if (result[3] != "" && result[3] != null) {

                var cont_det = result[3].split("|");
                $("#usern")[0].innerHTML = cont_det[0];
                $("#contno")[0].innerHTML = cont_det[1];
                $("#emailid")[0].innerHTML = cont_det[2];
                $("#bookingtermina")[0].innerHTML = cont_det[6];
                $("#p_mode")[0].innerHTML = cont_det[7];
                $("#issuedate")[0].innerHTML = cont_det[8];
                $('.showdivreschedule').css('display', 'block');
                $.unblockUI();
            }
            if (result[0] != "" && result[0] != null) {
                Alertmsg(result[0]);
                $("#reche_txt_viewSPNR").addClass("has-content");
                $("#reche_txt_viewAirPNR").addClass("has-content");
                $("#reche_txt_viewCSRPNR").addClass("has-content");
                $('#reschehide').css("display", "none");
                $.unblockUI();
            }
            if (result[4] != "" && result[4] != null) {

                var sr = JSON.parse(result[4]);
                if (sr.Table1.length > 0) {

                    var s = "";

                    s += "<div style='margi'>";
                    for (var i = 0; i < sr.Table1.length; i++) {
                        s += "<div class='res_row' style='margin-bottom: 10px;'>";
                        s += "<div id='pnrid' data-val='" + sr.Table1[i].S_PNR + "' ><span onclick='get_reschedule_view(this)' style='font-weight: 900;    cursor: pointer;'>" + sr.Table1[i].S_PNR + "</span></div>";
                        s += "</div>";
                    }
                    s += "</div>";
                    $("#reschedule_body").append('');
                    $("#reschedule_body").append(s);
                    $('#myModal_reschedule_double').modal('toggle');

                }
            }
            $.unblockUI();
        },
        error: function (e) {//On Successful service call
            if (e.status == "500") {
                Lobibox.alert("error", {   //eg: 'error' , 'success', 'info', 'warning'
                    msg: "An Internal Problem Occurred. Your Session will Expire.",
                    closeOnEsc: false,
                    callback: function ($this, type) {
                    }
                });
                //  showError("An Internal Problem Occurred. Your Session will Expire.", cancel_errPdiv, cancel_errdiv_msg, "");
                //  window.top.location = sessionExb;
                return false;
            }

            $.unblockUI();
        }	// When Service call fails

    });

    // clear_cancellation_pnr();
    //clear_pnr();
    //Btn_clr_Click();
}
var Exispnr = "";
function Createtable_Reschedule(result, appendiv, tablename, flg) {

    if (result[0] != "") {
        document.getElementById("air_reschedule_table").style.display = "Block";
        document.getElementById("Div_Char").style.display = "none";
        document.getElementById("air_reschedule_table").innerHTML = result[0];
        document.getElementById("air_reschedule_table").style.color = "RED";
        document.getElementById("air_reschedule_table").style.fontWeight = "bold";
        document.getElementById("air_reschedule_table").style.textAlign = "center";
    }

    if (result[1] != "") {
        document.getElementById("air_reschedule_table").style.display = "Block";
        //document.getElementById("charterror").style.display = "none";
        var tckt_json = result[1];
        var pax_json = result[5];

        var tckt_ary = $.parseJSON(tckt_json);
        Exispnr = tckt_ary
        existpnrlist = tckt_ary;
        var triptype = tckt_ary[tckt_ary.length - 1].TRIPNO == "1" ? "R" : "O";
        var Segment = tckt_ary[tckt_ary.length - 1].SEG_TYPE
        $("body").data("Triptype", triptype)
        $("body").data("Segment", Segment)
        var pax_ary = $.parseJSON(pax_json);
        var datafields = new Array();
        var columns = new Array();
        var pastype = "";
        sessionStorage.setItem('reschedule_pax_length', pax_ary.length);
        var showresbtn = false;
        if (pax_ary.length > 0) {
            $("#Reschedule_details_pax_tbl").html('');
            var s = '';

            // Passenger details
            s += '<div id="pax_rech" class="col-xs-12 col-md-6 col-sm-12 col-lg-6 bordercss showdivreschedule vpnrtbl pad-l-0"  >'; //01* style="overflow-x: auto;
            s += '<div class="bx-shdow-re col-xs-12 col0" style="padding-bottom:15px;">';
            s += '<table id="Reschedule_details_pax_tbl"  class="table table-hover table-striped" data-click-to-select="true" data-show-toggle="true" data-show-columns="true" data-show-filter="true" data-search="true" data-show-refresh="false"  data-show-export="true"  data-pagination="true"  style="text-align:left">';
            s += '<thead>';
            for (var i = 0; i < pax_ary.length; i++) {
                if (i == 0) { //Header formation...
                    s += '<tr>';
                    s += '<th style="text-align:center;text-transform: uppercase;"><input type="checkbox" class="chckreschpaxall" id="checkallpaxdetail" onclick="checkallpaxreschedule()"/><span>ALL</span></th>';
                    for (var key in pax_ary[0]) {
                        if (key == "PASSENGER_NAME") {
                            key = "Passenger&nbsp;Name"
                            s += '<th  data-field="direct' + key + '" style="text-align:center" data-sortable="false">' + key + '</th>';
                        }
                        else if (key == "PAX_TYPE") {
                            key = "Passenger&nbsp;type";
                            s += '<th data-field="direct' + key + '" style="text-align:center" data-sortable="false">' + key + '</th>';
                        }
                        else if (key == "STATUS") {

                            s += '<th data-field="direct' + key + '" style="text-align:center" data-sortable="false">' + key + '</th>';
                        }
                        else if (key == "PAX_REF_NO") {
                            s += '<th style="display:none;">' + key + '</th>';
                            s += '<th style="display:none;">' + key + '</th>';
                            s += '<th style="display:none;">' + key + '</th>';
                        }
                    }
                    s += '</tr>';
                    s += '</thead>';
                    s += '<tbody>';
                }

                s += '<tr>';
                var flg = false;
                var checkval = 0;
                
                for (var key in pax_ary[i]) { //All row formation...

                    if (checkval == 0) {
                        checkval++;
                        s += '<td style="text-align:center;" id="data_' + i + '"><input id="reschedule_pax_check_' + i + '" type="checkbox" class="chckreschedulepax" onclick="chckreschedulepax()" name="reschedule_pax_check_' + i + '" /></td>';
                    }
                    if (key == "PASSENGER_NAME") {
                        var paxname = pax_ary[i][key].toString();
                        s += '<td id="PASSENGER_NAME' + i + '" style="text-align:center; text-overflow: ellipsis;max-width: 185px;font-style: inherit;">' + paxname + '</td>';
                    }
                    else if (key == "PAX_TYPE") {
                        s += '<td id="pass_type' + i + '" style="text-align:center">' + (pax_ary[i][key] == "A" || pax_ary[i][key] == "ADT" || pax_ary[i][key] == "ADULT" ? "Adult" : (pax_ary[i][key] == "C" || pax_ary[i][key] == "CHD" || pax_ary[i][key] == "CHILD" ? "Child" : "Infant")) + '</a></td>';
                    }
                    else if (key == "STATUS") {
                        if (pax_ary[i][key] == "Confirmed") showresbtn = true;
                        s += '<td id="data_Status' + i + '" style="text-align:center" class="tckstauts">' + pax_ary[i][key] + '</td>';
                    }
                    else if (key == "PAX_REF_NO") {
                        
                        s += '<td style="display:none;" id="pax_ref_no' + i + '">' + pax_ary[i][key] + '</a></td>';
                        s += '<td style="display:none;" id="pax_gross' + i + '">' + pax_ary[i]["GROSSFARE"] + '</a></td>';
                        s += '<td style="display:none;" id="pax_tcktno' + i + '">' + pax_ary[i]["TICKET_NO"] + '</a></td>';
                        s += '<td style="display:none;" id="pax_netmat' + i + '">' + pax_ary[i]["NET_AMOUNT"] + '</a></td>';

                    }

                }
                s += '</tr>';
            }
            s += '</tbody>';
            s += '</table></div></div>';
            s += '<div class="col-xs-12 col-md-6 col-sm-12 col-lg-6 bordercss showdivreschedule vpnrtbl pad-r-0" style="overflow-x: auto;" >'; //01*
            s += '<div class= "bx-shdow-re col-xs-12 col0">';
            s += '<div class="col-sm-12 col0 reqgb" style=" background: #293a4a; color: #fff;">'; //NAT23
            s += '<h4 style="margin: 0; padding: 7px 10px;font-size:13px;">Required Confirmation</h4></div>';
            s += '<div class="col-xs-12 col-md-12 col-sm-12 col-lg-12 col0"></div>'
            s += '<div class="col-xs-12 col-md-12 col-sm-12 col-lg-12 col0" style="padding-top:10px;">'
            s += '<div class="col-xs-6 col-md-6 col-sm-12 col-lg-6"><input checked id="Fare_diff" type="checkbox" name="Fare_diff" /><label class="lbl labecheck" for="Fare_diff">Difference in fare</label></div>'; //01*
            s += '<div class="col-xs-6 col-md-6 col-sm-12 col-lg-6"><input checked id="Fare_change" type="checkbox" name="Fare_change" /><label class="lbl labecheck" for="Fare_change">Reschedule Charge</label></div>'; //01*
            s += '</div>';
            s += '<div class="col-xs-12 col-md-12 col-sm-12 col-lg-12 col0" style="padding-bottom:10px;"">'
            s += '<div class="col-xs-12 col-md-6 col-sm-12 col-lg-6"><label>Re-Enter Contact No.<span style="color: red">*</span></label><span>' + $("#hdn_countryphonecode").val() + '</span><input id="txt_cont_no" style="border: 1px solid #27a5f5; width: 89%;" maxlength="15"  value="" class="txt-anim" type="text" autocomplete="off" onkeypress="isNumericVal(event)" /></div>'; //01*
            s += '<div class="col-xs-12 col-md-6 col-sm-12 col-lg-6"><label style="">Remarks<span style="color: red">*</span></label><input id="txt_remark" style="border: 1px solid #27a5f5; width: 100%;" maxlength="200"  value="" class="txt-anim" type="text" autocomplete="off" /></div>'; //01*
            s += '</div>';
            s += '</div></div></div>';
            // Passenger details
            if (tckt_ary.length > 0) {
                // Ticket details

                $("#Reschedule_details_ticket_tbl").html('');
                // s += '<div> <h5 id="" class="cls-header m-b-10 showdivreschedule">Ticket Details<a href="#"><div class="fa fa-chevron-up rotate_c"></div></a></h5><div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 " id="air_tckt_reschedule_table"></div></div>';
                s += '<div class="col-lg-12" style="padding:0px;">';
                s += '<div id="tckt_rech" class="col-xs-12 col-md-11 col-sm-11 col-lg-12 bordercss" >';// style="overflow-x: auto;"
                s += '<table id="Reschedule_details_ticket_tbl"  class="table table-hover table-striped" data-click-to-select="true" data-show-toggle="true" data-show-columns="true" data-show-filter="true" data-search="true" data-show-refresh="false"  data-show-export="true"  data-pagination="true"  style="text-align:left">';
                s += '<thead>';
                var Tripno = 0;
                for (var i = 0; i < tckt_ary.length; i++) {
                    if (i == 0) { //Header formation...
                        s += '<tr>';
                        s += '<th style="text-align:center;text-transform: uppercase;width:80px;"><input type="checkbox" class="chcktcktrescheall" id="alltcktdetail" onclick="checkalltcktdetail()"/>&nbsp;ALL</th>';
                        for (var key in tckt_ary[0]) {
                            if (key == "FLIGHT_NO") {
                                var displaytext = "FlightNo.";
                                s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + displaytext + '</th>'; //RB25
                            }
                            else if (key == "ORIGIN") {
                                var displaytext = "Orgin";
                                s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + displaytext + '</th>';
                            }
                            if (key == "DESTINATION") {
                                var displaytext = "Destination";
                                s += '<th data-field="direct' + key + '" data-sortable="false">' + displaytext + '</th>';
                            }
                            else if (key == "DEPT_DATE") {
                                var displaytext = "Dept Date";
                                s += '<th data-field="direct' + key + '" data-sortable="false">' + displaytext + '</th>';
                            }
                            else if (key == "ARRIVAL_DATE") {
                                s += '<th style="display:none;">' + displaytext + '</th>';
                            }
                            else if (key == "CLASS_ID") {
                                var displaytext = "Class";
                                s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + displaytext + '</th>';
                            }
                            else if (key == "GROSSFARE") {
                                var displaytext = "Gross";
                                s += '<th style="display:none;" data-field="direct' + key + '" data-align="left" data-sortable="false">' + displaytext + '</th>';
                            }
                            else if (key == "SEGMENT_NO") {
                                s += '<th style="display:none;">' + key + '</th>';
                                s += '<th style="display:none;">' + key + '</th>';
                            }
                                //else if (key == "PAX_REF_NO") {
                                //    s += '<th style="display:none;">' + key + '</th>';
                                //}
                            else if (key == "STATUS") {
                                s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">ReSchedule</th>';
                                s += '<th style="display:none;"></th>';
                                s += '<th style="display:none;"></th>';
                                s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">Cabin</th>';
                                s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">Res.FlightNo</th>';
                                s += '<th></th>';
                                s += '<th></th>';
                            }
                        }
                        s += '</tr>';
                        s += '</thead>';
                        s += '<tbody class="appendbody">';
                    }

                    s += '<tr class="segement" id="Segment_div_' + i + '">';
                    var flg = false;
                    var checkval = 0;
                    var Spnr = "";

                    for (var key in tckt_ary[i]) { //All row formation...

                        if (checkval == 0) {
                            checkval++;
                            var ddept_date = (tckt_ary[i]["DEPT_DATE"]);
                            var dae = new Date();
                            var dd = dae.getDate();
                            var mm = dae.getMonth() + 1;
                            var yyyy = dae.getFullYear();

                            if (dd < 10) {
                                dd = '0' + dd
                            }

                            if (mm < 10) {
                                mm = '0' + mm
                            }
                            var Temp = yyyy + "/" + mm + "/" + dd;
                            var d1 = Temp.split("/")[0] + Temp.split("/")[1] + Temp.split("/")[2];
                            var d2 = ddept_date.split(" ")[0];
                            var d3 = d2.split("/")[2] + d2.split("/")[1] + d2.split("/")[0];
                            
                            if ((tckt_ary[i]["FLIGHT_NO"].indexOf("SG") != -1 || tckt_ary[i]["FLIGHT_NO"].indexOf("6E") != -1) || (Number(d3) >= Number(d1))) {
                                s += '<td  style="text-align:center;" id="data_' + i + '"> <input id="reschedule_tckt_check_' + i + '" class="rescheduleticketcheck" onchange="rescheduleticketcheck()" type="checkbox" name="rescheduletckt_check_' + i + '" /></a></td>';
                            }
                            else {
                                s += '<td style="text-align:center;" id="data_' + i + '"> <input id="reschedule_tckt_check_' + i + '" style="display:none;" class="" type="checkbox" name="rescheduletckt_check_' + i + '" /></a></td>';
                            }
                        }

                        if (key == "FLIGHT_NO") {
                            s += '<td id="Flight_no' + i + '">' + tckt_ary[i][key] + '</a></td>';
                        }
                        else if (key == "ORIGIN") {
                            s += '<td><input onkeypress="return ValidateSpecialChar(event);" onchange="return clearrescheduleinfo();"  data-origin="' + tckt_ary[i][key] + '" style="border: 1px solid #27a5f5;text-transform: uppercase;" id="Origin' + i + '"   value=' + loadcityName(tckt_ary[i][key]) + ' class="txt-anim origin" type="text" autocomplete="off" /></a></td>';
                        }
                        else if (key == "DESTINATION") {
                            s += '<td><input onkeypress="return ValidateSpecialChar(event);" onchange="return clearrescheduleinfo();" data-destination="' + tckt_ary[i][key] + '" style="border: 1px solid #27a5f5;text-transform: uppercase;" id="Destination' + i + '"  value=' + loadcityName(tckt_ary[i][key]) + ' class="txt-anim destination" type="text" autocomplete="off" /></a></td>';
                        }
                        else if (key == "DEPT_DATE") {
                            s += '<td id="Dept_date' + i + '">' + tckt_ary[i][key] + '</a></td>';
                        }
                        else if (key == "ARRIVAL_DATE") {
                            s += '<td style="display:none;" id="arr_date' + i + '">' + tckt_ary[i][key] + '</a></td>';
                        }
                        else if (key == "CLASS_ID") {
                            s += '<td id="Class_' + i + '">' + tckt_ary[i][key] + '</a></td>';
                        }
                        else if (key == "GROSSFARE") {
                            s += '<td style="display:none;" id="grossfare' + i + '" data-newgross="' + tckt_ary[i][key] + '">' + tckt_ary[i][key] + '</a></td>';
                        }
                        else if (key == "SEGMENT_NO") {
                            s += '<td style="display:none;" id="seg_ref_no' + i + '">' + tckt_ary[i][key] + '</a></td>';
                            s += '<td style="display:none;" id="reschedule_class' + i + '">' + tckt_ary[i]["CLASS_ID"] + '</a></td>';
                        }
                            //else if (key == "PAX_REF_NO") {
                            //    s += '<td style="display:none;" id="pax_ref_no' + i + '">' + tckt_ary[i][key] + '</a></td>';
                            //}
                        else if (key == "STATUS" || key == "TRIPNO") {
                            if (key == "STATUS") {
                                s += '<td id="data_' + i + '" style="width: 10%;"><input readonly="readonly" onchange="return clearrescheduleinfo();" style="border: 1px solid #27a5f5;padding: 3px 3px 6px 8px !important;" id="rechetxt_' + i + '" maxlength="7"  value="' + tckt_ary[i]["DEPT_DATE"].split(' ')[0] + '" class="txt-anim reschdule_" type="text" autocomplete="off" /></a></td>';
                                s += '<td id="data_' + i + '" style="width: 10%;"><div><select style="border: 1px solid #27a5f5;background-color: #777;color: white; height:30px; padding:0px 5px;"  class="form-control deftwid" id="txt_Class' + i + '"   onchange="return clearrescheduleinfo();">'
                                s += '<option selected value="E">Economy</option>'
                                s += '<option value="B">Business</option>'
                                s += '<option  value="F">First Class</option><option  value="P">Premium Economy</option></select></div>'
                                s += '</td>';
                                s += '<span id="reschedule_spnr' + i + '" style="display:none;" >' + tckt_ary[i]["SPNR"] + '</span>';
                                s += '<td  id="data_' + i + '"  style="width: 10%;" class="deftwid"><input placeholder="Flight No..."  style="border: 1px solid #27a5f5;width: 100%;text-transform:uppercase;" id="flight_' + i + '" maxlength="7"  value="" class="txt-anim deftwid" type="text" autocomplete="off" onkeypress="return ValidateSpecialChar(event);"  onchange="return clearrescheduleinfo();" /></td>'

                            }
                            else {
                                if (showresbtn) {
                                    var showcls = tckt_ary.length - 1 == i ? "display:block" : "display:none"
                                    if (Tripno == tckt_ary[i]["TRIPNO"] && key == "TRIPNO") {
                                        Tripno++;
                                        s += '<td><span id="trip_ref' + i + '" style="display:none;" >' + tckt_ary[i][key] + '</span>';
                                        s += '<span id="getavail_' + i + '" style="text-decoration: underline;font-size: 14px;color: red;cursor: pointer;" onclick="javascript:GetFlightavail(' + i + ');">Get Availability</span></a></td>';

                                        s += '<td><span id="Addremov_' + i + '" style=' + showcls + ' ><i class="fa fa-plus addsec" onclick="addsector()"></i> <i class="fa fa-close removesec" onclick="Removesec()"></i></span></td>';
                                    }
                                    else {
                                        s += '<td><span style="text-decoration: underline;font-size: 14px;color: red;cursor: pointer;VISIBILITY: hidden;" onclick="javascript:GetFlightavail(' + i + ');">Get Availability</span></a></td>';
                                        s += '<td><span id="Addremov_' + i + '" style=' + showcls + ' ><i class="fa fa-plus addsec" onclick="addsector()"></i> <i class="fa fa-close removesec" onclick="Removesec()"></i></span></td>';
                                    }
                                }


                            }

                        }
                        //
                    }
                    s += '</tr>';
                }
                s += '</tbody>';
                s += '</table></div>';

                //  <b onclick="addsector()">Add Sector</b></div>'
                // Ticket details
            }

            if (showresbtn) {
                s += '<div class="col-sm-12 col-xs-12 col0 reschedulebtn"  style="margin-top: 20px;">';
                s += '<ul>';
                s += '<li><input type="button" id="" class="action-button action-buttonO shadow animate color c-button b-50 bg-aqua hv-transparent cmn-bg-clr btn-round action-buttonO action-button" style="width: 100%;"  value="Confirm" onclick="reschedule_confirm();"></li>';
                if (result[6] != null && result[6] != "" && result[6] != "OFFLINE") {
                    s += '<li><input type="button" id="" class="action-button action-buttonO shadow animate colorB1 c-button b-50 bg-aqua hv-transparent cmn-bg-clr btn-round action-buttonB action-button" style="width: 100%;"  value="Offline Request" onclick="offlinereschedulereq();"></li>';
                }
                s += '</ul>';
                s += '</div>';
            }

            $('#' + appendiv).html('');
            $('#' + appendiv).html(s);
            $(".removesec").first().hide();
            loadcitytype()
        }
    }
}



function GetFlightavail(i) {
    
    if (document.getElementById("rechetxt_" + i).value == "") {
        Alertmsg("Please select Reschedule date");
        return false;
    }
    var tripref = "";
    var spnr = "";
    var Cabin = "";
    var date = "";
    var origin = "";
    var dest = "";
    tripref = document.getElementById("trip_ref" + i).innerHTML;
    spnr = document.getElementById("reschedule_spnr" + i).innerHTML;
    var segmetcnt = 0;
    var triptype = $("body").data("Triptype");
    var Segmenttype = $("body").data("Segment");
    var lastsdat = $(".reschdule_").last().val();

    if ($("#alltcktdetail").prop("checked") && triptype == "R" && Segmenttype == "I") {
        cnt = 0;
        origin = document.getElementById("Origin0").value;
        dest = document.getElementById("Destination0").value;
        Cabin = document.getElementById("txt_Class" + cnt).value;
        date = document.getElementById("rechetxt_" + cnt).value;
        Arrivdate = lastsdat;

    } else {
        $(".rescheduleticketcheck").each(function () {
            var ids = this.id;
            var cnt = ids.split("_")[3];
            if ($("#" + ids).prop("checked")) {
                segmetcnt++;
                if (origin == "") {

                    Cabin = document.getElementById("txt_Class" + cnt).value;
                    date = document.getElementById("rechetxt_" + cnt).value;
                    Arrivdate = date;
                    origin = document.getElementById("Origin" + cnt).value;
                    triptype = "O";
                }
                dest = document.getElementById("Destination" + cnt).value;
            }

        })
    }
    if (origin == "") {
        Alertmsg("Please select any one segment")
        return false;
    }
    // var flightno = document.getElementById("Flight_No" + i).innerHTML;

    sessionStorage.setItem('adu_clickitem', origin + "," + dest + "," + tripref + "," + i);
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + Spinner + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });


    var input = {
        Tripref: tripref,
        RescheduleDate: date,
        Arrivdate: Arrivdate,
        Spnr: spnr,
        Cabin: Cabin,
        Origin: origin.split("(")[1].replace(")", "").toUpperCase(),
        Dest: dest.split("(")[1].replace(")", "").toUpperCase(),
        Triptype: triptype,
    }
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: Rescheduleavailablity,		// Location of the service
        data: JSON.stringify(input),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        timeout: 100000,
        success: function (json) {//On Successful service call
            
            $.unblockUI();
            var result = json.Result;
            if (result[0] !== "") {
                Alertmsg(result[0]);
                return false;
            }
            if (result[1] != "") {
                var aryKV = JSON.parse(result[2]);
                var grp = [];
                var availres = JSON.parse(result[1], function (k, v) {
                    grp = [];
                    grp = $.grep(aryKV, function (n, i) { return n.CD == k; });
                    if (grp.length > 0)
                        this[grp[0].FN] = v;
                    else
                        return v;
                });
                jsonArr = availres
                if (jsonArr.length == 0) {
                    Alertmsg("No flights available.(#07)");
                    return false;
                }
                //var lengthchk = $(".segement").length; //segmetcnt;//result[5];
                //var listArr = $.grep(jsonArr, function (u, i) {
                //    return u.length ==lengthchk;
                //});
                //if (listArr.length == 0) {
                //    Alertmsg("No flights available.(#07)");
                //    return false;
                //}
                //jsonArr = listArr

                if (jsonArr.length > 0) {
                    var popup = '';
                    popup += "<div class='col-lg-12'><div class='col-lg-6' style='padding:0px;margin-left: -9px;'><input id='chkapplyall' onchange='applyallypax()' type ='checkbox'/><label for='chkapplyall' style='position: relative;left: 3px;top: -2px;cursor:pointer'>Apply to all ticket</label></div><div class='col-lg-6' style='padding:0px;'><span style='float:right'>Existing Ticket fare : " + $("#pax_gross0").html() + " -/per adult</span><br/> <span style='float:right;position:relative;left:-13px;margin-bottom: 10px;'>Existing Net fare : " + $("#pax_netmat0").html() + " -/per adult</span></div></div>"
                    popup += '<table class="table no-more-tables table table-responsive table-bordered">';
                    popup += '<thead>';
                    popup += '<tr>';
                    popup += '<th>Select</th>';
                    popup += '<th>Airline</th>';
                    popup += '<th>Origin</th>';
                    popup += '<th>Destination</th>';
                    popup += '<th>Departure Date</th>';
                    popup += '<th>Arrival Date</th>';
                    popup += '<th>FareType</th>';

                    popup += '<th>Class</th>';
                    popup += '<th>Fare Rule</th>';
                    popup += '<th>Net Fare </th>';
                    popup += '<th>Gross</th>';
                    popup += '<th>Fare Diff</th>';

                    popup += '</tr>';
                    popup += '</thead>';
                    popup += '<tbody>';
                    var chekary = "";
                    var mycnt = 0;
                    var disablecls = "";
                    var chkorgdes = false;
                    var clstxt = "";
                    


                    $.each(jsonArr, function (i, val) {
                        chkorgdes = false;

                        if (val[0].Origin.trim() == result[3].trim() && val[val.length - 1].Destination.trim() == result[4].trim()) {
                            chkorgdes = true;
                        }

                        if (chkorgdes == true) {
                            for (var l = 0; l < val.length; l++) {
                                
                                // if ($("#Origin" + l).val().split("(")[1].replace(")", "") == val[l].Origin && $("#Destination" + l).val().split("(")[1].replace(")", "") == val[l].Destination) {
                                clstxt = l > 0 ? "bgclrcls" : "";
                                popup += '<tr class="bodycontent ' + clstxt + '" for="radReSelect' + l + '' + i + '">';
                                chekary = i == "0" && l == "0" ? "checked" : "";
                                disablecls = l > 0 ? "disabled" : "";
                                if (l == "0") {
                                    popup += '<td data-title="Select"><input class="rescheduleavail" type="radio" ' + disablecls + ' id="radReSelect' + l + '' + i + '"   ' + chekary + ' data-resary=' + "'" + JSON.stringify(jsonArr[i]) + "'" + '    name="reschedule" onclick="javascript:GetRowIndexBook(' + l + ',' + i + ',' + val.length + ');"/></td>';
                                } else {
                                    popup += '<td></td>';
                                }
                                popup += '<td data-title="Airline" id="flightNo' + l + '' + i + '">' + val[l].FlightNumber + '</td>';
                                popup += '<td data-title="Origin" id="originavail' + l + '' + i + '">' + val[l].Origin + '</td>';
                                popup += '<td data-title="Destination" id="destinationavail' + l + '' + i + '">' + val[l].Destination + '</td>';
                                popup += '<td data-title="Departure Date" id="departure' + l + '' + i + '" data-value="' + val[l].Depdate + '" data-time="' + val[l].DepartureTime + '">' + val[l].DepartureDate + " " + val[l].DepartureTime + '</td>';
                                popup += '<td data-title="Arrival Date" id="arrival' + l + '' + i + '" data-value="' + val[l].Arrdate + '" data-time="' + val[l].ArrivalTime + '">' + val[l].ArrivalDate + " " + val[l].ArrivalTime + '</td>';
                                var Faretypeval = val[l].FareType == "N" ? "Normal Fare" : val[l].FareType == "C" ? "Corporate Fare" : val[l].FareType == "R" ? "Retail Fare" : val[l].FareType == "M" ? "SME Fare" : val[l].FareType == "U" ? "Flat Fare" : val[l].FareType;
                                if (val[l].FARETYPETEXT != "") {
                                    Faretypeval = val[l].FARETYPETEXT;
                                }
                                popup += '<td data-title="FareType" id="faretype' + l + '' + i + '">' + (Faretypeval || "-") + '</td>';

                                popup += '<td data-title="Class" id="ReClass' + l + '' + i + '">' + val[l].Class + '</td>';
                                popup += '<td data-title="Fare Rule"><a><span id="getfarerule' + l + '' + i + '"  style="font-size: 14px;cursor: pointer;text-decoration: underline;"  data-invavalue=' + "'" + val[l].Inva + "'" + ' onclick="javascript:getrescheduleavail(' + l + ',' + i + ');">Fare rule</span></a></td>';

                                var farediffadult = Number(val[l].GrossAmount) - Number($("#pax_gross0").html());
                                if (l == "0") {
                                    popup += '<td data-title="Class" id="netfare' + l + '' + i + '">' + (val[l].NETFare || "-") + '</td>';
                                    popup += '<td data-title="Gross" id="grossamt' + l + '' + i + '">' + val[l].GrossAmount + '</td>';
                                    popup += '<td data-title="Class" id="faredClass' + l + '' + i + '">' + farediffadult + '</td>';

                                }
                                else {
                                    popup += '<td></td>';
                                    popup += '<td data-title="Gross" style="display:none;" id="grossamt' + l + '' + i + '">' + val[l].GrossAmount + '</td>';
                                }


                                popup += '</tr>';

                                // }
                            }
                        }
                        $("#hdfRowIndexBook").val("0_0_" + val.length);
                    })
                    popup += '</tbody>';
                    popup += '</thead>';
                    popup += '</table>';
                    $('#dvReschedule').html(popup);
                    if ($(".bodycontent").html() != "" && $(".bodycontent").html() != undefined && $(".bodycontent").html() != null && $(".bodycontent").html() != "undefined") {
                        // document.getElementById("chkApplyAll").checked = false;
                        $("#myModalP").modal({
                            backdrop: 'static',
                            keyboard: false
                        });
                        $("#myModalP").show();
                    } else {
                        Alertmsg("Same sector not available .please contact support team.(03)");
                        return false;
                    }
                }
            }
            else {
                Alertmsg("No Flights available");
                return false;
            }

        },
        error: function (e) {//On Successful service call
            
            if (e.status == "500") {
                Alertmsg("An Internal Problem Occurred. Your Session will Expire.");
                window.top.location = sessionExb;
                return false;
            }
            $.unblockUI();
        }	// When Service call fails

    });

}


function GetRowIndexBook(arg, txt, Len) {
    $("#hdfRowIndexBook").val(arg + "_" + txt + "_" + Len);
}

function getrescheduleavail(val, row) {
    var availjson = $("#getfarerule" + val + '' + row).attr("data-invavalue");

    $.blockUI({
        message: '<img alt="Please Wait..." src="' + Spinner + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });

    var param = { Availjson: availjson };

    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: Reschedulefarerule,
        // data: '{Paxddetails: "' + Totpax_new + '",Ticketdetails: "' + Totticket_new + '",Remarks:"' + Remarks + '",Contactdet:"' + con + '",farediff:"' + fare_diff + '",farechange:"' + fare_change + '",RescheduleFlag:"' + RescheduleFlag + '",SPNR:"' + SPNR + '",AirlinePNR:"' + AirlinePNR + '",CRSPNR:"' + CRSPNR + '",Availjson:"' + availjson + '",Flag:"' + Flag + '"}',
        data: JSON.stringify(param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (json) {//On Successful service call
            $.unblockUI();
            var response = json.Result;
            FareRuledisplay(response);
        },
        error: function (e) {//On Successful service call
            Alertmsg(e.responseText);
            if (e.status == "500") {
                Alertmsg("An Internal Problem Occurred. Your Session will Expire.");
                window.top.location = sessionExb;
                return false;
            }
            $.unblockUI();
        }	// When Service call fails
    });

}

function FareRuledisplay(response) {

    var result = response[1];
    var category = response[2];
    var error = response[0];
    var strFareRuleBulid = ""
    $('#FareRuleLodingImage').hide();
    $('#closeimage').show();
    if (result != "" && result != null) {
        var Ititle = "";
        var Isubtitle = "";
        var IContent = "";
        var Ifullopt = true;
        var Itemp1 = "";
        var Itemp2 = "";
        if (category == "FSC" || category == "UAI" || category == "1A" || category == "1G") {
            var Resultss = $.xml2json(result);
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
                        str += '<div class="clsRuleHead" style="cursor:pointer;" id="dvcheck_' + new_arrkjjj[i][k].RULE.split('.')[0] + i + '" onclick="javascript:CloseSegWiseFareRule(this.id);">' + new_arrkjjj[i][k].RULE + '<i class="fa fa-chevron-down"></i></div>';
                        str += '<div style="width:100%;border-bottom: 1px dashed;">';
                        str += '<pre readonly="readonly" class="clsFaretextArea" id="' + new_arrkjjj[i][k].RULE.split('.')[0] + i + '" style="display: none;">' + new_arrkjjj[i][k].TEXT + '</pre>';
                        str += '</div>';
                    }
                    str += '</div>';
                }
            }
            Ititle = "Fare Rule";
            Isubtitle = "";
            IContent = str;
            Ifullopt = false;
            Itemp1 = "";
            Itemp2 = "";
            showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
        }
        else {
            Ititle = "Fare Rule";
            Isubtitle = "";
            IContent = "<pre>" + result + "</pre>";
            Ifullopt = true;
            Itemp1 = "";
            Itemp2 = "";
            showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
        }
    }
    else {
        Ititle = "Fare Rule";
        Isubtitle = "";
        IContent = "<pre>" + error + "</pre>";
        Ifullopt = true;
        Itemp1 = "";
        Itemp2 = "";
        showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
    }
}


function RescheduleAvail() {
    
    var tripty = "";
    var sr_n = sessionStorage.getItem('adu_clickitem').toString();
    var spt = sr_n.split(',');
    var c = document.getElementsByName('chkbox');
    var tab = "";
    var pAdultDetail = "";
    var pChildDetail = "";
    var pInfantDetail = "";
    var checkedvalue = 1;
    var fli_no = "";
    var Origin = "";
    var Destination = "";
    var Deptdate = "";
    var rowindex = $("#hdfRowIndexBook").val();
    var selectindex = spt[3];
    var tripindex = spt[2];
    var listid = [];
    var selectflt = "";
    $(".rescheduleavail").each(function () {
        var ids = this.id;

        if ($("#" + ids).prop("checked")) {

            selectflt = JSON.parse($("#" + ids).attr("data-resary"));
            return false
        }

    });
    var oldpnrsector = $(".segement").length
    if (oldpnrsector != selectflt.length) {
        if (oldpnrsector < selectflt.length) {
            var dif = Number(selectflt.length) - Number(oldpnrsector)
            for (var i = 0; i < dif; i++) {
                addsector()
            }
        }
        if (oldpnrsector > selectflt.length) {
            var dif = Number(selectflt.length) - Number(oldpnrsector)
            for (var i = oldpnrsector - 1; i > selectflt.length - 1; i--) {
                Removesec()
            }
        }

    }
    sessionStorage.setItem('reschedule_tckt_length', oldpnrsector);
    if (rowindex == "") {
        Alertmsg("Please select any one of the flight");
        RetValue = false;
    }
    else {
        var availlen = selectflt.length;
        var rowposition = rowindex.split('_')[1];
        for (var i = 0; i < parseInt(availlen) ; i++) {
            var idappend = i//(parseInt(i) + parseInt(selectindex));

            $("#Origin" + idappend).val(loadcityName(selectflt[i].Origin));
            $("#Destination" + idappend).val(loadcityName(selectflt[i].Destination));
            $("#flight_" + idappend).val(selectflt[i].FlightNumber);
            // $("#Dept_date" + idappend)[0].innerHTML = selectflt[i].DepartureDate;

            if (i == "0") {
                if (tripindex == "0") {
                    $("#hdn_onwardrescheduleavail").val(JSON.stringify(selectflt));
                }
                else {
                    $("#hdn_returnrescheduleavail").val($("#radReSelect" + i + '' + rowposition).attr("data-resary"));
                }
            }
            var rescheduledate = selectflt[i].Depdate;
            var rescheduledeptdate = selectflt[i].Depdate + " " + selectflt[i].DepartureTime;
            var reschedulearrdate = selectflt[i].Arrdate + " " + selectflt[i].ArrivalTime;
            var reschedulefare = selectflt[i].GrossAmount
            $("#rechetxt_" + idappend).addClass("has-content");
            $("#rechetxt_" + idappend).datepicker({
                minDate: rescheduledate, dateFormat: "dd/mm/yy",
                onSelect: function (arg) {
                    
                    var ids = this.id;
                }
            });
            $("#rechetxt_" + idappend).val(rescheduledate);
            $("#rechetxt_" + idappend).attr("data-deptdate", rescheduledeptdate);// = rescheduledeptdate;
            $("#rechetxt_" + idappend).attr("data-arrdate", reschedulearrdate);// = reschedulearrdate;
            $("#grossfare" + idappend).attr("data-newgross", reschedulefare);
        }
    }
    $('#myModalP').modal('hide');
}

var Totpax_new = "";
var Tottckt_new = "";
function reschedule_confirm() {
    
    // var TotalPax = "";
    // var Totpax_new = "";
    Totpax_new = "";
    Tottckt_new = "";
    var adultcount = 0;
    var childcount = 0;
    var Infantcount = 0;
    var checkedAdult = 0;
    var checkedChild = 0;
    var checkedInfant = 0;
    var totallen = 0;
    var checkedlen = 0;
    var checkedtcktlen = 0;

    adult_data = "";
    child_data = "";
    infant_data = "";
    var newfare = "";

    var allowreschedule = true;
    var checkstatus = "";
    $("#hdn_Totpax_new").val("");
    $("#hdn_Tottckt_new").val("");

    if ($("#reche_txt_viewSPNR").val() == "" && $("#reche_txt_viewAirPNR").val() == "" && $("#reche_txt_viewCSRPNR").val() == "") {
        Alertmsg("Please enter any pnr number");
        return false;
    }

    var S_PNR = $("#reche_txt_viewSPNR").val();
    var AirlinePNR = $("#reche_txt_viewAirPNR").val();
    var CRSPnr = $("#reche_txt_viewCSRPNR").val();
    var seqid = "";

    var VPassDet = $("#pax_rech").find('table');
    for (var j = 0; j < VPassDet.length; j++) {
        var par = VPassDet[j].rows;
        var arr = sessionStorage.getItem('reschedule_pax_length');
        totallen_pax = arr;
        for (var i = 0; i < arr; i++) {
            if (($("#pass_type" + i)[0].innerHTML.toUpperCase() == "A" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "ADULT" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "ADT") && ($("#data_Status" + i)[0].innerHTML.toUpperCase() == "CONFIRMED")) {
                adultcount++;
            }
            if (($("#pass_type" + i)[0].innerHTML.toUpperCase() == "C" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "CHILD" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "CHD") && ($("#data_Status" + i)[0].innerHTML.toUpperCase() == "CONFIRMED")) {
                childcount++;
            }
            if (($("#pass_type" + i)[0].innerHTML.toUpperCase() == "I" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "INFANT" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "INF") && ($("#data_Status" + i)[0].innerHTML.toUpperCase() == "CONFIRMED")) {
                Infantcount++;
            }
            var select = $("#reschedule_pax_check_" + i)[0];
            if (select.checked == true) {
                checkedlen++;
                if ($("#pass_type" + i)[0].innerHTML.toUpperCase() == "A" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "ADULT" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "ADT") {
                    checkedAdult++;
                }
                if ($("#pass_type" + i)[0].innerHTML.toUpperCase() == "C" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "CHILD" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "CHD") {
                    checkedChild++;
                }
                if ($("#pass_type" + i)[0].innerHTML.toUpperCase() == "I" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "INFANT" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "INF") {
                    checkedInfant++;
                }
                var pas_type = $("#pass_type" + i)[0].innerHTML;
                var Paxname = $("#PASSENGER_NAME" + i)[0].innerHTML;
                var status = $("#data_Status" + i)[0].innerHTML;
                var pax_ref = $("#pax_ref_no" + i)[0].innerHTML;
                var pax_gross = $("#pax_gross" + i)[0].innerHTML;
                var pax_tcktno = $("#pax_tcktno" + i)[0].innerHTML;
                if (status.toUpperCase() != "CONFIRMED") {
                    allowreschedule = false;
                    checkstatus = status;
                }
                //              0             1             2                 3               4               5                6              7                 8                 9
                Totpax_new += seqid + "," + S_PNR + "," + pas_type + "," + Paxname + "," + status + "," + AirlinePNR + "," + CRSPnr + "," + pax_ref + "," + pax_gross + "," + pax_tcktno + "|";
            }
            else {
                RescheduleStatus = "P";
            }

        }
    }

    var Tcktdet = $("#tckt_rech").find('table');
    for (var j = 0; j < Tcktdet.length; j++) {
        var par = Tcktdet[j].rows;
        var tckt_arr = $(".segement").length;
        totallen_tckt = tckt_arr;
        for (var i = 0; i < tckt_arr; i++) {
            var select = $("#reschedule_tckt_check_" + i)[0];
            if (select.checked == true) {
                checkedtcktlen++;
                var Origin = $("#Origin" + i).val();
                var Destination = $("#Destination" + i).val();
                var Prev_Origin = $("#Origin" + i).attr("data-origin");
                var Prev_Destination = $("#Destination" + i).attr("data-destination");
                var Class = $("#Class_" + i)[0].innerHTML;
                var Grossfare = $("#grossfare" + i)[0].innerHTML;
                var Grossfare = $("#grossfare" + i)[0].innerHTML;
                var Rescheduledate = $("#rechetxt_" + i).val();
                var deptdate = $("#rechetxt_" + i).attr("data-deptdate") != null && $("#rechetxt_" + i).attr("data-deptdate") != undefined ? $("#rechetxt_" + i).attr("data-deptdate") : $("#rechetxt_" + i).val() + " " + "00:00";
                var arrdate = $("#rechetxt_" + i).attr("data-arrdate") != null && $("#rechetxt_" + i).attr("data-arrdate") != undefined ? $("#rechetxt_" + i).attr("data-arrdate") : $("#rechetxt_" + i).val() + " " + "00:00";// $("#rechetxt_" + i).attr("data-arrdate");
                var Cabin = $("#txt_Class" + i).val();
                var FlightNo = $("#flight_" + i).val();
                var Existflightno = $("#Flight_no" + i)[0].innerHTML;
                var Tripno = $("#trip_ref" + i).val();
                var seg_ref = $("#seg_ref_no" + i)[0].innerHTML;
                var pax_ref = "";
                var Existdeptdate = $("#Dept_date" + i)[0].innerHTML;
                var Existarrdate = $("#arr_date" + i)[0].innerHTML;
                var rescheduleclass = $("#reschedule_class" + i)[0].innerHTML;
                newfare = $("#grossfare" + i).attr("data-newgross");
                var regularExpression = /[A-Za-z]+(\s[A-Za-z]+)?/;
                if (FlightNo == "") {//par[i].cells[20].lastChild.value
                    Alertmsg("Please enter flight number");
                    return false;
                }
                else if (!regularExpression.test(FlightNo)) {
                    Alertmsg("Please enter valid flight number");
                    return false;
                }
                else {
                    FlightNo = FlightNo;
                }
                

                //                                   0                              1                                            2               3                   4                  5              6                7              8                9                10                   11                       12                    13              14               15                      16                  17                 18                 19                     
                var Destination = $("#Destination" + i).val();
                var addsectorvar = Exispnr.length < $(".segement").length ? $(".segement").length - Exispnr.length : "";
                Tottckt_new += Origin.split("(")[1].replace(")", "") + "," + Destination.split("(")[1].replace(")", "") + "," + Class + "," + Grossfare + "," + Rescheduledate + "," + Cabin + "," + FlightNo + "," + Tripno + "," + seg_ref + "," + pax_ref + "," + Existflightno + "," + Existdeptdate + "," + rescheduleclass + "," + deptdate + "," + arrdate + "," + Prev_Origin + "," + Prev_Destination + "," + arrdate + "," + Existarrdate + "," + addsectorvar + "|";
            }
            else {
                RescheduleStatus = "P";
            }

        }
    }

    if (allowreschedule == false) {
        Alertmsg("PNR already in " + checkstatus + " status, Unable to reschedule the pnr");
        return false;
    }

    if (Totpax_new == null || Totpax_new == "") {
        Alertmsg("Please select passenger");
        return false;
    }
    else if (Tottckt_new == null || Tottckt_new == "") {
        Alertmsg("Please select any sector");
        return false;
    }
    else {
        $("#hdn_Totpax_new").val(Totpax_new);
        $("#hdn_Tottckt_new").val(Tottckt_new);
    }
    var contact_num = $("#txt_cont_no").val();
    var con = contact_num.trim();
    var Remarks = $("#txt_remark").val();

    if (con == null || con == "") {
        Alertmsg("Please enter contact no.");
        return false;
    }
    else if (con.length < 10) {
        Alertmsg("Please enter valid phone no.");
        return;
    }
    if (Remarks == null || Remarks == "") {
        Alertmsg("Please enter the remarks");
        return false;
    }
    var Adult = adultcount - checkedAdult;
    var infant = Infantcount - checkedInfant;
    var child = childcount - checkedChild;
    if ((Infantcount > 0) && (Adult < infant)) {
        Alertmsg("Infant not to travel without Adult");
        return false;
    }
    else if ((checkedInfant > 0) && (checkedInfant > checkedAdult)) {
        Alertmsg("Infant not to travel without Adult");
        return;
    }
    if ((checkedChild > 0) && (0 >= checkedAdult)) {
        Alertmsg("Child not to travel without Adult");
        return false;
    }
    else if ((Adult <= 0) && (checkedChild == 0) && childcount > 0) {
        Alertmsg("Child not to travel without Adult");
        return;
    }

    //#endregion
    
    if (totallen_pax == checkedlen && checkedtcktlen == totallen_tckt && $("#hdn_onwardrescheduleavail").val() != "") // Online reschedule request for fully reschedule --
    {

        $("#Reschedule_faredetails").html("");
        $("#get_fare_reschedule").css("display", "block");
        $("#confirm_reschedule").css("display", "none");
        $("#hdn_reschedule_airpenalty").val("");
        $("#hdn_reschedule_farediff").val("");

        var flightdet = "";
        flightdet += "<div class='col-xs-12 col-md-12 col-sm-12 col-lg-12 col0'><div class='col-xs-12 col-md-6 col-sm-6 col-lg-6 bg-clr-white'><div class='hdr-tit'>Existing Flight details</div>";
        flightdet += "<table>";
        flightdet += "<thead><tr><th>Airline</th><th>Origin</dth><th>Destination</th><th>Departure Date</th><th>Arrival Date</th><tr></thead>";
        flightdet += "<tbody>";
        for (var i = 0; i < existpnrlist.length; i++) {


            flightdet += "<tr><td>" + existpnrlist[i].FLIGHT_NO + "</td><td>" + existpnrlist[i].ORIGIN + "</td><td>" + existpnrlist[i].DESTINATION + "</td>";
            flightdet += "<td>" + existpnrlist[i].DEPT_DATE + "</td><td>" + existpnrlist[i].ARRIVAL_DATE + "</td></tr>";


        }
        flightdet += "</tbody>";
        flightdet += "</table>";
        flightdet += "</div>";

        flightdet += "<div class='col-xs-12 col-md-6 col-sm-6 col-lg-6 bg-clr-white'><div class='hdr-tit'>Rescheduled Flight details</div>";
        flightdet += "<table>";
        flightdet += "<thead><tr><th>Airline</th><th>Origin</th><th>Destination</th><th>Departure Date</th><th>Arrival Date</th></tr>";
        flightdet += "<tbody>";
        for (var i = 0; i < Tottckt_new.split('|').length; i++) {
            if (Tottckt_new.split('|')[i] != "") {

                flightdet += "<tr><td>" + Tottckt_new.split('|')[i].split(',')[6] + "</td><td>" + Tottckt_new.split('|')[i].split(',')[0] + "</td><td>" + Tottckt_new.split('|')[i].split(',')[1] + "</td>";
                flightdet += "<td>" + Tottckt_new.split('|')[i].split(',')[13] + "</td><td>" + Tottckt_new.split('|')[i].split(',')[14] + "</td></tr>";

            }
        }
        flightdet += "</tbody>";
        flightdet += "</table>";
        flightdet += "</div>";


        $("#Exist_flight_details").html(flightdet);

        var passdet = "";
        passdet += "<div class='col-xs-12 col-md-12 col-sm-12 col-lg-12 col0'><div class='col-xs-12 col-md-6 col-sm-6 col-lg-6 bg-clr-white'><div class='hdr-tit'>Rescheduled Passenger details</div>";
        passdet += "<table>";
        passdet += "<thead><tr><th>Name</th><th>Old Ticket No.</th><th>Old Fare</th><th>Reschedule Fare</th></tr></thead>";
        passdet += "<tbody>";
        for (var i = 0; i < Totpax_new.split('|').length; i++) {
            if (Totpax_new.split('|')[i] != "") {
                passdet += "<tr><td>" + Totpax_new.split('|')[i].split(',')[3] + "</td><td>" + Totpax_new.split('|')[i].split(',')[9] + "</td><td>" + Totpax_new.split('|')[i].split(',')[8] + "</td><td>" + newfare + "</td></tr>";
            }
        }
        passdet += "</tbody>";
        passdet += "</table>";
        passdet += "</div>";

        passdet += "<div class='col-xs-12 col-md-6 col-sm-6 col-lg-6 bg-clr-white'><div class='hdr-tit'>Terms & Conditions :</div>";
        passdet += "<div class='col-sm-12 col0 termcon col-xs-12'>";
        passdet += "<div class='termpar col-sm-12'>- Partial Rescheduled is not allowed to online.</div>";
        passdet += "<div class='termdep col-sm-12'>- Departure date should be same as all passenger.</div>";
        passdet += "</div></div></div>";

        $("#Reschedule_paxdet").html(passdet);

        $("#hdn_reschedulereqtype").val("ONLINE");
        $('#myModal_reschdule_confirm').modal('toggle');
    }
    else // Offline reschedule request for partial reschedule
    {
        $("#hdn_reschedulereqtype").val("OFFLINE");
        //$('#myModal_reschdule_confirm').modal('toggle');
        Confirmreschedule();
    }

}

function offlinereschedulereq() {
    Totpax_new = "";
    Tottckt_new = "";
    var adultcount = 0;
    var childcount = 0;
    var Infantcount = 0;
    var checkedAdult = 0;
    var checkedChild = 0;
    var checkedInfant = 0;
    var totallen = 0;
    var checkedlen = 0;
    var checkedtcktlen = 0;

    adult_data = "";
    child_data = "";
    infant_data = "";
    var newfare = "";

    var allowreschedule = true;
    var checkstatus = "";
    $("#hdn_Totpax_new").val("");
    $("#hdn_Tottckt_new").val("");

    if ($("#reche_txt_viewSPNR").val() == "" && $("#reche_txt_viewAirPNR").val() == "" && $("#reche_txt_viewCSRPNR").val() == "") {
        Alertmsg("Please enter any pnr number");
        return false;
    }

    var S_PNR = $("#reche_txt_viewSPNR").val();
    var AirlinePNR = $("#reche_txt_viewAirPNR").val();
    var CRSPnr = $("#reche_txt_viewCSRPNR").val();
    var seqid = "";

    var VPassDet = $("#pax_rech").find('table');
    for (var j = 0; j < VPassDet.length; j++) {
        var par = VPassDet[j].rows;
        var arr = sessionStorage.getItem('reschedule_pax_length');
        totallen_pax = arr;
        for (var i = 0; i < arr; i++) {
            if (($("#pass_type" + i)[0].innerHTML.toUpperCase() == "A" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "ADULT" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "ADT") && ($("#data_Status" + i)[0].innerHTML.toUpperCase() == "CONFIRMED")) {
                adultcount++;
            }
            if (($("#pass_type" + i)[0].innerHTML.toUpperCase() == "C" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "CHILD" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "CHD") && ($("#data_Status" + i)[0].innerHTML.toUpperCase() == "CONFIRMED")) {
                childcount++;
            }
            if (($("#pass_type" + i)[0].innerHTML.toUpperCase() == "I" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "INFANT" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "INF") && ($("#data_Status" + i)[0].innerHTML.toUpperCase() == "CONFIRMED")) {
                Infantcount++;
            }
            var select = $("#reschedule_pax_check_" + i)[0];
            if (select.checked == true) {
                checkedlen++;
                if ($("#pass_type" + i)[0].innerHTML.toUpperCase() == "A" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "ADULT" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "ADT") {
                    checkedAdult++;
                }
                if ($("#pass_type" + i)[0].innerHTML.toUpperCase() == "C" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "CHILD" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "CHD") {
                    checkedChild++;
                }
                if ($("#pass_type" + i)[0].innerHTML.toUpperCase() == "I" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "INFANT" || $("#pass_type" + i)[0].innerHTML.toUpperCase() == "INF") {
                    checkedInfant++;
                }
                var pas_type = $("#pass_type" + i)[0].innerHTML;
                var Paxname = $("#PASSENGER_NAME" + i)[0].innerHTML;
                var status = $("#data_Status" + i)[0].innerHTML;
                var pax_ref = $("#pax_ref_no" + i)[0].innerHTML;
                var pax_gross = $("#pax_gross" + i)[0].innerHTML;
                var pax_tcktno = $("#pax_tcktno" + i)[0].innerHTML;
                if (status.toUpperCase() != "CONFIRMED") {
                    allowreschedule = false;
                    checkstatus = status;
                }
                //              0             1             2                 3               4               5                6              7                 8                 9
                Totpax_new += seqid + "," + S_PNR + "," + pas_type + "," + Paxname + "," + status + "," + AirlinePNR + "," + CRSPnr + "," + pax_ref + "," + pax_gross + "," + pax_tcktno + "|";
            }
            else {
                RescheduleStatus = "P";
            }

        }
    }

    var Tcktdet = $("#tckt_rech").find('table');
    for (var j = 0; j < Tcktdet.length; j++) {
        var par = Tcktdet[j].rows;
        var tckt_arr = $(".segement").length;
        totallen_tckt = tckt_arr;
        for (var i = 0; i < tckt_arr; i++) {
            var select = $("#reschedule_tckt_check_" + i)[0];
            if (select.checked == true) {
                checkedtcktlen++;
                var Origin = $("#Origin" + i).val();
                var Destination = $("#Destination" + i).val();
                var Prev_Origin = $("#Origin" + i).attr("data-origin");
                var Prev_Destination = $("#Destination" + i).attr("data-destination");
                var Class = $("#Class_" + i)[0].innerHTML;
                var Grossfare = $("#grossfare" + i)[0].innerHTML;
                var Grossfare = $("#grossfare" + i)[0].innerHTML;
                var Rescheduledate = $("#rechetxt_" + i).val();//data-arrdate
                var deptdate = $("#rechetxt_" + i).attr("data-deptdate") != null && $("#rechetxt_" + i).attr("data-deptdate") != undefined ? $("#rechetxt_" + i).attr("data-deptdate") : $("#rechetxt_" + i).val() + " " + "00:00";
                var arrdate = $("#rechetxt_" + i).attr("data-arrdate") != null && $("#rechetxt_" + i).attr("data-arrdate") != undefined ? $("#rechetxt_" + i).attr("data-arrdate") : $("#rechetxt_" + i).val() + " " + "00:00";
                var Cabin = $("#txt_Class" + i).val();
                var FlightNo = $("#flight_" + i).val();
                var Existflightno = $("#Flight_no" + i)[0].innerHTML;
                var Tripno = $("#trip_ref" + i).val();
                var seg_ref = $("#seg_ref_no" + i)[0].innerHTML;
                var pax_ref = "";
                var Existdeptdate = $("#Dept_date" + i)[0].innerHTML;
                var Existarrdate = $("#arr_date" + i)[0].innerHTML;
                var rescheduleclass = $("#reschedule_class" + i)[0].innerHTML;
                newfare = $("#grossfare" + i).attr("data-newgross");
                var regularExpression = /[A-Za-z]+(\s[A-Za-z]+)?/;
                if (FlightNo == "") {//par[i].cells[20].lastChild.value
                    Alertmsg("Please enter flight number");
                    return false;
                }
                else if (!regularExpression.test(FlightNo)) {
                    Alertmsg("Please enter valid flight number");
                    return false;
                }
                else {
                    FlightNo = FlightNo;
                }
                var addsectorvar = Exispnr.length < $(".segement").length ? $(".segement").length - Exispnr.length : "";
                //                       0                                        1                                              2               3                   4                  5              6                7              8                9                10                   11                       12                    13              14               15                      16                  17                 18
                Tottckt_new += Origin.split("(")[1].replace(")", "") + "," + Destination.split("(")[1].replace(")", "") + "," + Class + "," + Grossfare + "," + Rescheduledate + "," + Cabin + "," + FlightNo + "," + Tripno + "," + seg_ref + "," + pax_ref + "," + Existflightno + "," + Existdeptdate + "," + rescheduleclass + "," + deptdate + "," + arrdate + "," + Prev_Origin + "," + Prev_Destination + "," + arrdate + "," + Existarrdate + "," + addsectorvar + "|";
            }
            else {
                RescheduleStatus = "P";
            }

        }
    }

    if (allowreschedule == false) {
        Alertmsg("PNR already in " + checkstatus + " status, Unable to reschedule the pnr");
        return false;
    }

    if (Totpax_new == null || Totpax_new == "") {
        Alertmsg("Please select passenger");
        return false;
    }
    else if (Tottckt_new == null || Tottckt_new == "") {
        Alertmsg("Please select any sector");
        return false;
    }
    else {
        $("#hdn_Totpax_new").val(Totpax_new);
        $("#hdn_Tottckt_new").val(Tottckt_new);
    }
    var contact_num = $("#txt_cont_no").val();
    var con = contact_num.trim();
    var Remarks = $("#txt_remark").val();

    if (con == null || con == "") {
        Alertmsg("Please enter contact no.");
        return false;
    }
    else if (con.length < 10) {
        Alertmsg("Please enter valid phone no.");
        return;
    }

    if (Remarks == null || Remarks == "") {
        Alertmsg("Please enter the remarks");
        return false;
    }
    var Adult = adultcount - checkedAdult;
    var infant = Infantcount - checkedInfant;
    var child = childcount - checkedChild;
    if ((Infantcount > 0) && (Adult < infant)) {
        Alertmsg("Infant not to travel without Adult");
        return false;
    }
    else if ((checkedInfant > 0) && (checkedInfant > checkedAdult)) {
        Alertmsg("Infant not to travel without Adult");
        return false;
    }
    if ((checkedChild > 0) && (0 >= checkedAdult)) {
        Alertmsg("Child not to travel without Adult");
        return false;
    }
    else if ((Adult <= 0) && (checkedChild == 0) && childcount > 0) {
        Alertmsg("Child not to travel without Adult");
        return false;
    }

    //#endregion

    $("#hdn_reschedulereqtype").val("OFFLINE");
    //$('#myModal_reschdule_confirm').modal('toggle');
    Confirmreschedule();
}

function checkalltcktdetail() {
    if ($("#alltcktdetail")[0].checked == true) {
        $('.rescheduleticketcheck').prop('checked', true);
    }
    else {
        $('.rescheduleticketcheck').prop('checked', false);
    }
}

function checkallpaxreschedule() {
    if ($("#checkallpaxdetail")[0].checked == true) {
        $('.chckreschedulepax').prop('checked', true);
    }
    else {
        $('.chckreschedulepax').prop('checked', false);
    }
}


function Confirmreschedule() {

    var Totpax_new = $("#hdn_Totpax_new").val();
    var Totticket_new = $("#hdn_Tottckt_new").val();
    var Remarks = $("#txt_remark").val();

    var SPNR = $("#reche_txt_viewSPNR").val();
    var AirlinePNR = $("#reche_txt_viewAirPNR").val();
    var CRSPNR = $("#reche_txt_viewCSRPNR").val();

    var contact_num = $("#txt_cont_no").val();
    var con = contact_num.trim();

    if (con == null || con == "") {
        Alertmsg("Please enter contact no.");
        return false;
    }
    if (Remarks == null || Remarks == "") {
        Alertmsg("Please enter the remarks");
        return false;
    }
    var fare_diff = "N";
    if (document.getElementById("Fare_diff").checked == true) {
        fare_diff = 'Y';
    }
    else {
        fare_diff = "N";
    }
    var fare_change = "N";
    if (document.getElementById("Fare_change").checked == true) {
        fare_change = "Y";
    }
    else {
        fare_change = "N";
    }

    var RescheduleFlag = $("#hdn_reschedulereqtype").val();
    var onward_availjson = $("#hdn_onwardrescheduleavail").val();
    var return_availjson = $("#hdn_returnrescheduleavail").val();

    var Flag = "CONFIRM_RESCHEDULE";
    $('#myModal_reschdule_confirm').modal('hide');
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + Spinner + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });

    var param = {
        Paxddetails: Totpax_new, Ticketdetails: Totticket_new, Remarks: Remarks, Contactdet: con, farediff: fare_diff, farechange: fare_change, RescheduleFlag: RescheduleFlag,
        SPNR: SPNR, AirlinePNR: AirlinePNR, CRSPNR: CRSPNR, OnwardAvailjson: onward_availjson, ReturnAvailjson: return_availjson, Flag: Flag
    };
    //   FetchActiondetails("SI20BF0009", "BOM", "DEL", "RDL000000496")
    //return false;
    
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: Onlinereschedule,
        //data: '{ContactDet: "' + Totpax_new + '",RescheduleStatus: "' + RescheduleStatus + '",remarks:"' + remarks + '",Spnr:"' + Spnr + '",AirlinePNR:"' + AirlinePNR + '",CrsPNR:"' + CRSPNR + '",RescheduleFlag:"' + RescheduleFlag + '"}',//,RescheduleStatus: "' + $("#hdnPax")[0].value + '" ,BlockTicket: "' + BlockTicket + '",PaymentMode: "' + $("#ddlPaymode").val() + '",TourCode: "' + $("#Tour_Code").val() + '"}',
        data: JSON.stringify(param),
        contentType: "application/json; ch666arset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call
            
            $.unblockUI();
            var response = json.Results;
            if (response[1] == "1" && response[0] != "") { // Success Result


                //Alertmsg(response[0]);
                Lobibox.alert('info', {   //eg: 'error' , 'success', 'info', 'warning'
                    msg: response[0],
                    closeOnEsc: false,
                    callback: function ($this, type) {
                    }
                });
                clear_rescheule_pnr();
                return false;

            }
            else if (response[0] != "") { // Failure Result
                if (response[0].indexOf(":") != -1) {
                    var Rsdlno = response[0].split(":")[1];


                    Lobibox.alert('info', {   //eg: 'error' , 'success', 'info', 'warning'
                        msg: response[0],
                        closeOnEsc: false,
                        callback: function ($this, type) {
                        }
                    });
                    clear_rescheule_pnr();
                    return false;

                    //Lobibox.confirm({
                    //    msg: "" + response[0] + "</br> Do you wish to continue with Offline Accounting ?",
                    //    buttons: {
                    //        yes: {
                    //            'class': 'btn btn-success',
                    //            text: 'Yes',
                    //            closeOnClick: true
                    //        },
                    //        no: {
                    //            'class': 'btn btn-warning',
                    //            text: 'No',
                    //            closeOnClick: true
                    //        }
                    //    },
                    //    callback: function (lobibox, type) {
                    //        var btnType;
                    //        if (type === 'no') {
                    //            clear_rescheule_pnr();
                    //            return false;
                    //        } else if (type === 'yes') {
                    //            FetchActiondetails(SPNR, $(".origin").first().val().split("(")[1].replace(")", "").trim(), $(".destination").first().val().split("(")[1].replace(")", "").trim(), Rsdlno)
                    //        }
                    //    }
                    //});

                }
                else {

                    Lobibox.alert('info', {   //eg: 'error' , 'success', 'info', 'warning'
                        msg: response[0],
                        closeOnEsc: false,
                        callback: function ($this, type) {
                        }
                    });
                    clear_rescheule_pnr();
                    return false;
                }
            }
            else {
                // alert("Unable to process your request");
                Lobibox.alert('info', {   //eg: 'error' , 'success', 'info', 'warning'
                    msg: "Unable to process your request",
                    closeOnEsc: false,
                    callback: function ($this, type) {
                    }
                });
                clear_rescheule_pnr();
                return false;
            }
        },
        error: function (e) {//On Successful service call
            //alert(e.responseText);
            if (e.status == "500") {
                alert("An Internal Problem Occurred. Your Session will Expire.");
                window.top.location = sessionExb;
                return false;
            }
            $.unblockUI();
        }	// When Service call fails
    });
}


function Cancelreschedule() {

    var Totpax_new = $("#hdn_Totpax_new").val();
    var Totticket_new = $("#hdn_Tottckt_new").val();
    var Remarks = $("#txt_remark").val();

    var SPNR = $("#reche_txt_viewSPNR").val();
    var AirlinePNR = $("#reche_txt_viewAirPNR").val();
    var CRSPNR = $("#reche_txt_viewCSRPNR").val();

    var contact_num = $("#txt_cont_no").val();
    var con = contact_num.trim();

    if (con == null || con == "") {
        Alertmsg("Please enter contact no.");
        return false;
    }
    if (Remarks == null || Remarks == "") {
        Alertmsg("Please enter the remarks");
        return false;
    }
    var fare_diff = "N";
    if (document.getElementById("Fare_diff").checked == true) {
        fare_diff = 'Y';
    }
    else {
        fare_diff = "N";
    }
    var fare_change = "N";
    if (document.getElementById("Fare_change").checked == true) {
        fare_change = "Y";
    }
    else {
        fare_change = "N";
    }

    var RescheduleFlag = $("#hdn_reschedulereqtype").val();
    var onward_availjson = $("#hdn_onwardrescheduleavail").val();
    var return_availjson = $("#hdn_returnrescheduleavail").val();
    var Flag = "CANCELREQUEST";
    $('#myModal_reschdule_confirm').modal('hide');
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + Spinner + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });

    var param = {
        Paxddetails: Totpax_new, Ticketdetails: Totticket_new, Remarks: Remarks, Contactdet: con, farediff: fare_diff, farechange: fare_change, RescheduleFlag: RescheduleFlag,
        SPNR: SPNR, AirlinePNR: AirlinePNR, CRSPNR: CRSPNR, OnwardAvailjson: onward_availjson, ReturnAvailjson: return_availjson, Flag: Flag, strPaymentmode: "T"
    };

    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: Onlinereschedule,
        //data: '{ContactDet: "' + Totpax_new + '",RescheduleStatus: "' + RescheduleStatus + '",remarks:"' + remarks + '",Spnr:"' + Spnr + '",AirlinePNR:"' + AirlinePNR + '",CrsPNR:"' + CRSPNR + '",RescheduleFlag:"' + RescheduleFlag + '"}',//,RescheduleStatus: "' + $("#hdnPax")[0].value + '" ,BlockTicket: "' + BlockTicket + '",PaymentMode: "' + $("#ddlPaymode").val() + '",TourCode: "' + $("#Tour_Code").val() + '"}',
        data: JSON.stringify(param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {//On Successful service call
            $.unblockUI();
            var response = json.Results;
            if (response[1] == "1" && response[0] != "") { // Success Result
                Alertmsg(response[0]);
                clear_rescheule_pnr();
                return false;
            }
            else if (response[1] == "0" && response[0] != "") { // Failure Result
                Alertmsg(response[0]);
                clear_rescheule_pnr();
                return false;
            }
            else {
                Alertmsg("Unable to process your request");
                clear_rescheule_pnr();
                return false;
            }
        },
        error: function (e) {//On Successful service call
            //Alertmsg(e.responseText);
            $.unblockUI();
            if (e.status == "500") {
                Alertmsg("An Internal Problem Occurred. Your Session will Expire.");
                window.top.location = sessionExb;
                return false;
            }
            $.unblockUI();
        }	// When Service call fails
    });
}
function clear_rescheule_pnr() {
    $("#Exist_flight_details").val("");
    $("#Reschedule_paxdet").val("");
    $("#Reschedule_faredetails").val("");
    $("#get_fare_reschedule").css("display", "block");
    $("#confirm_reschedule").css("display", "none");
    $("#reche_txt_viewSPNR").val("");
    $("#reche_txt_viewAirPNR").val("");
    $("#reche_txt_viewCSRPNR").val("");
    $("#reche_txt_viewCSRPNR").removeClass("has-content");
    $("#reche_txt_viewAirPNR").removeClass("has-content");
    $("#reche_txt_viewSPNR").removeClass("has-content");
    $(".showdivreschedule").css("display", "none");
    $("#rech_dvGrpBkngerr_alert").removeClass("has_error");
    $("#rech_dvGrpBkngerr").removeClass("has_error");
    $("#air_reschedule_table").css("display", "none");
}

function clearrescheduleinfo() {
    $("#hdn_onwardrescheduleavail").val("");
    $("#hdn_returnrescheduleavail").val("");
}

//Online Reschedule Request//
function Reschedule_confirm_request() {
    
    $('#iLoading').show();
    //$('#myModal_reschdule_confirm').modal('hide');
    var Totpax_new = $("#hdn_Totpax_new").val();
    var Totticket_new = $("#hdn_Tottckt_new").val();
    var Remarks = $("#txt_remark").val();

    var SPNR = $("#reche_txt_viewSPNR").val();
    var AirlinePNR = $("#reche_txt_viewAirPNR").val();
    var CRSPNR = $("#reche_txt_viewCSRPNR").val();

    var contact_num = $("#txt_cont_no").val();
    var con = contact_num.trim();
    var Remarks = $("#txt_remark").val();

    if (con == null || con == "") {
        Alertmsg("Please enter contact no.");
        $('#iLoading').hide();
        return false;
    }
    if (Remarks == null || Remarks == "") {
        Alertmsg("Please enter the remarks");
        $('#iLoading').hide();
        return false;
    }
    var fare_diff = "N";
    if (document.getElementById("Fare_diff").checked == true) {
        fare_diff = 'Y';
    }
    else {
        fare_diff = "N";
    }
    var fare_change = "N";
    if (document.getElementById("Fare_change").checked == true) {
        fare_change = "Y";
    }
    else {
        fare_change = "N";
    }

    var RescheduleFlag = $("#hdn_reschedulereqtype").val();
    var onward_availjson = $("#hdn_onwardrescheduleavail").val();
    var return_availjson = $("#hdn_returnrescheduleavail").val();

    var Flag = "CHECKFARE";

    var param = {
        Paxddetails: Totpax_new, Ticketdetails: Totticket_new, Remarks: Remarks, Contactdet: con, farediff: fare_diff, farechange: fare_change, RescheduleFlag: RescheduleFlag,
        SPNR: SPNR, AirlinePNR: AirlinePNR, CRSPNR: CRSPNR, OnwardAvailjson: onward_availjson, ReturnAvailjson: return_availjson, Flag: Flag, strPaymentmode: ""//$("#reche_ddl_paymentmode").val()
    };

    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: Onlinereschedule,
        // data: '{Paxddetails: "' + Totpax_new + '",Ticketdetails: "' + Totticket_new + '",Remarks:"' + Remarks + '",Contactdet:"' + con + '",farediff:"' + fare_diff + '",farechange:"' + fare_change + '",RescheduleFlag:"' + RescheduleFlag + '",SPNR:"' + SPNR + '",AirlinePNR:"' + AirlinePNR + '",CRSPNR:"' + CRSPNR + '",Availjson:"' + availjson + '",Flag:"' + Flag + '"}',
        data: JSON.stringify(param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // async: false,
        success: function (json) {//On Successful service call
            $('#iLoading').hide();
            var response = json.Results;
            
            if (response[1] != "" && response[1] == "1") {
                var Rsdlno = "";
                if (response[0].indexOf(":") != -1) {
                    Rsdlno = response[0].split(":")[1];
                }
                $('#myModal_reschdule_confirm').modal('hide');
                FetchActiondetails(SPNR, $(".origin").first().val().split("(")[1].replace(")", "").trim(), $(".destination").first().val().split("(")[1].replace(")", "").trim(), Rsdlno, response);

                //var GetFareDetails = JSON.parse(response[3]);
                //var Penaltyamt = GetFareDetails.rootNode[0].Penalty;// response[3].split("|")[0];
                //var FareDiff = GetFareDetails.rootNode[0].FareDifference; //response[3].split("|")[1];
                //var creditshel = GetFareDetails.rootNode[0].CSAmount != null && GetFareDetails.rootNode[0].CSAmount != undefined ? GetFareDetails.rootNode[0].CSAmount : "";
                //var faredet = "";
                //faredet += "<div class='col-xs-12 col-md-12 col-sm-12 col-lg-12'>";
                //faredet += "<div><div class='hdr-tit'>Online Rescheduled Fare Details</div>";
                //faredet += "<table>"
                //faredet += "<thead><tr><th>Penalty</th><th>Fare Difference (Appx)</th>"
                //if (creditshel != "") { faredet += "<th>Credit Shell</th>"; }

                //faredet += "<th>Message</th></tr></thead>";
                //faredet += "<tbody><tr>";
                //faredet += "<td>" + Penaltyamt + "</td><td>" + FareDiff + "</td>"
                //if (creditshel != "") { faredet += "<td>" + creditshel + "</td>"; }
                //faredet += " <td>(With this penalty Reissue charges will be applicable)</td></tr>";
                //if (response[2] != null && response[2] != "") {
                //    faredet += "<tr><td colspan='3'>" + response[2] + "</td></tr>";
                //}
                //faredet += "</tbody>";
                //faredet += "<table>";
                //faredet += "</div>";

                //if (GetFareDetails.Status[0].RSC == "1" && GetFareDetails.Status[0].ERR.indexOf("CS") != -1 && GetFareDetails.rootNode[0].hasOwnProperty('CSAmount')) {
                //    faredet += "<div style='font-weight: 700!important;margin: 10px 20px;'>";
                //    faredet += "<span style='color: red !important; display: inline;'>Credit Shell</span>";
                //    faredet += "<span style='color: black !important; display: inline;margin-left:4px'>Amount : " + GetFareDetails.rootNode[0].CSAmount + "</span>";
                //    faredet += "</div>";
                //}

                //faredet += "</div>"
                //$("#Reschedule_faredetails").html(faredet);
                //$("#get_fare_reschedule").css("display", "none");
                //$("#confirm_reschedule").css("display", "block");
                //$("#hdn_reschedule_airpenalty").val(Penaltyamt);
                //$("#hdn_reschedule_farediff").val(FareDiff);
            }
            else if (response[1] == "0" && response[0] != "") {
                
                if (response[0].indexOf(":") != -1) {
                    var Rsdlno = response[0].split(":")[1];
                    Lobibox.alert('info', {   //eg: 'error' , 'success', 'info', 'warning'
                        msg: response[0],
                        closeOnEsc: false,
                        callback: function ($this, type) {
                        }
                    });
                    clear_rescheule_pnr();
                    return false;
                    //return false;
                    //Lobibox.confirm({
                    //    msg: "" + response[0] + "</br> Do you wish to continue with Offline Accounting ?",
                    //    buttons: {
                    //        yes: {
                    //            'class': 'btn btn-success',
                    //            text: 'Yes',
                    //            closeOnClick: true
                    //        },
                    //        no: {
                    //            'class': 'btn btn-warning',
                    //            text: 'No',
                    //            closeOnClick: true
                    //        }
                    //    },
                    //    callback: function (lobibox, type) {
                    //        var btnType;
                    //        if (type === 'no') {
                    //            $('#myModal_reschdule_confirm').modal('hide');
                    //            clear_rescheule_pnr();
                    //            return false;
                    //        } else if (type === 'yes') {
                    //            $('#myModal_reschdule_confirm').modal('hide');
                    //            FetchActiondetails(SPNR, $(".origin").first().val().split("(")[1].replace(")", "").trim(), $(".destination").first().val().split("(")[1].replace(")", "").trim(), Rsdlno)
                    //        }
                    //    }
                    //});

                } else {
                    Alertmsg(response[0]);
                    $('#myModal_reschdule_confirm').modal('hide');
                    clear_rescheule_pnr();
                    return false;
                }
            }
            else {
                Alertmsg("Unable to process your request");
                $('#myModal_reschdule_confirm').modal('hide');
                clear_rescheule_pnr();
                return false;
            }
        },
        error: function (e) {//On Successful service call
            $('#iLoading').hide();
            //Alertmsg(e.responseText);
            if (e.status == "500") {
                Alertmsg("An Internal Problem Occurred. Your Session will Expire.");
                $('#myModal_reschdule_confirm').modal('hide');
                window.top.location = sessionExb;
                return false;
            }
        }	// When Service call fails
    });
    // $('#myModal_reschdule_confirm').modal('hide');
}
//Online Reschedule Request//
var json = "";
function loadcitytypexml() {
    //@Url.Content("~/XML/CityAirport_Lst.xml")
    $.ajax({
        type: "GET",
        url: CityAirport_Lst,
        dataType: 'xml',
        anync: false,
        success: function (response) {
            json = $.xml2json(response);
        },
    })
}
var productNames = [];
function loadcitytype(arg) {
    

    arg = "";
    try {
        ;
        var Seg = "I";
        //  var json = $.xml2json(response);
        var JsonData = json.AIR;
        var jsnICitys = JsonData;//To Find Segment type while search flights...
        var jsonresults = JSON.stringify(JsonData);
        productNames = [];


        $("body").data("segreqcheck", "I");
        jsnICitys = JsonData;
        var citynames = "";
        $.each(JsonData, function (index, AIR) {
            if (AIR.AN.indexOf('-') != -1) {
                citynames = AIR.AN.split('-')[0].trim();
            } else { citynames = AIR.AN.trim(); }

            if (AIR.CN != "undefined" && AIR.CN.length > 0) {
                productNames.push(citynames + "-(" + AIR.ID + ")" + "~" + AIR.CN + "~" + AIR.AN + "-" + arg);
                productIds[AIR.AN] = AIR.ID;
            }
        });




    }
    catch (ex) {
        Lobibox.alert('error', {   //eg: 'error' , 'success', 'info', 'warning'
            msg: ex,
            closeOnEsc: false,
            callback: function ($this, type) {
            }
        });
        //alert(ex);
    }
    // 
    $('.origin').typeahead({
        source: productNames,
        items: 8,
        scrollBar: true,
        onSelect: function (view, elements) {
            
            var ids = view.text.split("-")[3]
            $(".origin").data("origin", view.citycode);
            $("body").data("Origin_Code", view.citycode);
        }

    });
    $('.destination').typeahead({
        source: productNames,
        items: 8,
        scrollBar: true,
        onSelect: function (view, elements) {
            
            var ids = view.text.split("-")[3]
            $(".destination").data("data-destination", view.citycode);
            //     $("#textDestination11").data("code", view.citycode);
            $("body").data("Destination_Code", view.citycode);
        }
    });

}
function close_confirm_request() {
    $('#myModal_reschdule_confirm').modal('hide');
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


function chckreschedulepax() {
    var length = sessionStorage.getItem('reschedule_pax_length');
    var check = 0;
    for (var i = 0; i < length; i++) {
        if ($("#reschedule_pax_check_" + i)[0].checked == true) {
            check++;
        }
    }
    if (length == check) {
        $('#checkallpaxdetail').prop('checked', true);
    }
    else {
        $('#checkallpaxdetail').prop('checked', false);
    }
}
function checkallpaxreschedule() {
    if ($("#checkallpaxdetail")[0].checked == true) {
        $('.chckreschedulepax').prop('checked', true);
    }
    else {
        $('.chckreschedulepax').prop('checked', false);
    }
}


function Alertmsg(msg) {
    Lobibox.alert('info', {   //eg: 'error' , 'success', 'info', 'warning'
        msg: msg,
        closeOnEsc: false,
        callback: function ($this, type) {
        }
    });
}
function loadcityName(arg) {
    
    var Arrlist = $.grep(json.AIR, function (u, i) {
        return u.ID == arg.trim()
    });
    return Arrlist[0].AN.split("-")[0] + "-" + "(" + Arrlist[0].ID + ")";
}

function addsector() {
    
    if ($(".segement").length >= 5) {
        return false
    }
    var lentcnt = $(".segement").length
    lentcnt = lentcnt - 1;
    var nxcnt = $(".segement").length
    var content = $(".segement").last().html();
    content = content.replace("reschedule_tckt_check_" + lentcnt + "", "reschedule_tckt_check_" + nxcnt + "").replace("data_" + lentcnt + "", "data_" + nxcnt + "")
    .replace("Flight_no" + lentcnt + "", "Flight_no" + nxcnt + "").replace("Origin" + lentcnt + "", "Origin" + nxcnt + "").replace("Destination" + lentcnt + "", "Destination" + nxcnt + "")
    .replace("Dept_date" + lentcnt + "", "Dept_date" + nxcnt + "").replace("Class_" + lentcnt + "", "Class_" + nxcnt + "").replace("grossfare" + lentcnt + "", "grossfare" + nxcnt + "").replace("rechetxt_" + lentcnt + "", "rechetxt_" + nxcnt + "")
    .replace("txt_Class" + lentcnt + "", "txt_Class" + nxcnt + "").replace("flight_" + lentcnt + "", "flight_" + nxcnt + "").replace("seg_ref_no" + lentcnt + "", "seg_ref_no" + nxcnt + "").replace("reschedule_class" + lentcnt + "", "reschedule_class" + nxcnt + "")
    .replace("arr_date" + lentcnt + "", "arr_date" + nxcnt + "").replace("trip_ref" + lentcnt + "", "trip_ref" + nxcnt + "").replace("getavail_" + lentcnt + "", "getavail_" + nxcnt + "")
    .replace("Segment_div_" + lentcnt + "", "Segment_div_" + nxcnt + "").replace("Addremov_" + lentcnt + "", "Addremov_" + nxcnt + "")

    var htmldiv = "<tr class='segement' id='Segment_div_" + nxcnt + "'>" + content + "</tr>"

    $(".appendbody").append(htmldiv)
    $("#Addremov_" + lentcnt).hide();
    $(".removesec").hide();
    $(".removesec").last().show();
    $("#seg_ref_no" + nxcnt).html($(".segement").length);
    $("#reschedule_tckt_check_" + nxcnt).prop("checked", true);
    $("#getavail_" + nxcnt).hide();
    $("#rechetxt_" + nxcnt).removeClass("hasDatepicker");
    $("#rechetxt_" + nxcnt).datepicker({
        minDate: $("#Dept_date" + lentcnt).val(), dateFormat: "dd/mm/yy",
        onSelect: function (arg) {
            
            var ids = this.id;
            var cnt = ids.split("_")[1];
            cnt = Number(cnt) + Number(1);
            if ($("#rechetxt_" + cnt).length > 0) {
                $("#rechetxt_" + cnt).val(arg)
                $("#rechetxt_" + cnt).datepicker("destroy");
                $("#rechetxt_" + cnt).datepicker({
                    minDate: arg, dateFormat: "dd/mm/yy",
                    onSelect: function (arg) {
                        
                        var ids = this.id;
                    }
                });
            }
        }
    });
    loadcitytype();
}
function applyallypax() {
    
    $('.chckreschpaxall').prop('checked', true);
    $('.chckreschedulepax').prop('checked', true);

    $('.alltcktdetail').prop('checked', true);
    $('.rescheduleticketcheck').prop('checked', true);


}


// To be reshedule Region start


var AllcorporateDetails = [];
var Passengerdetails = [];
var Segmentdetails = [];
var Supplierdetails = [];
var GSTDetails = []
var SSRDetails = [];
var Totalinputdetails = [];
var CityAirportary = [];
var Airlinenameary = [];
var TotalErpAttributes = "";





$('body').on('click', '.closesearch', function () {
    $(this).closest(".dvclstypeahead").find(".form-control").val("");
});
$("#sltbookpcc").change(function () {
    $("#slttktpcc").val($(this).val());
});

var arg = 0;


function Clearinputfields(clas) {
    $("." + clas).find(".form-control").val("");
    $(".columndiv").hide();
    $("#Txt_Fromdate , #Txt_todate").val(ServerDate);
    $("#chkAll").prop("checked", false);
}


var AirlineNames = [];
$(document).ready(function () {
    $(".form-control").keyup(function () {
        $(".form-control").removeClass("bordercls")
    })


    $(".masktimeformatingcls").focus(function () {
        $(this).parent().find(".timeerrmsg").remove();
    });

});







function FetchActiondetails(Strspnr, Orgcode, Descode, Rsdlno, GetFareDetails) {

    $.blockUI({
        message: '<img alt="Please Wait..." src="' + Spinner + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });
    var input = {
        Strspnr: Strspnr,
        Orgcode: Orgcode,
        Descode: Descode,
        Rsdlno: Rsdlno,

    }

    $.ajax({
        type: "POST",
        url: Getreshdel_details,
        data: JSON.stringify(input),
        contentType: "application/json; charset=utf-8",
        timeout: 180000,
        dataType: "json",
        success: function (data) {
            
            $.unblockUI();
            if (data.Status == "03") {
                var res = JSON.parse(data.Result);
                Reshedule_details(res, GetFareDetails)
            } else {
                $(".columndiv").hide();
                //   alert("No Record Found");
                LobiboxAlert('error', "No Record Found", '', '');
            }

        },
        error: function (e) {

        }
    });
}
var Reshpaxdeta = "";
var ResheduleFlt = "";
var SSRdet = "";
function Reshedule_details(arg, response) {
    ResheduleFlt = arg.P_SELECT_RESCHDULE_DETAILS_BOA;
    var Ext_flight = arg.P_SELECT_RESCHDULE_DETAILS_BOA7 || [];
    Reshpaxdeta = arg.P_SELECT_RESCHDULE_DETAILS_BOA1
    SSRdet = arg.P_SELECT_RESCHDULE_DETAILS_BOA4;
    var Corpbal = arg.P_SELECT_RESCHDULE_DETAILS_BOA5;
    newssr = [];
    GloabSSR = 0;
    $(".Newssr").hide()
    $(".SSRamt").html("0")
    $(".totsmt").html("0");
    $("#currency").val(Corpbal[0].CCD_CURRENCY_CODE)
    if (arg != "") {

        $('#btnExport').show();
        if ($('#divTicket_Exist_flt table').length > 0) {
            $('#divTicket_Exist_flt').columns("destroy");
        }
        if ($('#divTicket_New_flt table').length > 0) {
            $('#divTicket_New_flt').columns("destroy");
        }

        Ext_flight = Ext_flight.reduce(function (memo, e1) {
            var matches = memo.filter(function (e2) {
                return e1.segment_no == e2.segment_no
            })
            if (matches.length == 0)
                memo.push(e1)
            return memo;
        }, [])

        $('#divTicket_Exist_flt').columns({

            data: Ext_flight,
            schema: [

                { "header": "Origin", "key": "origin", },
                { "header": "Destination", "key": "destination", },
                { "header": "Departure", "key": "departuretime", },
                { "header": "Arrival ", "key": "arrivaltime" },
                { "header": "Flight No.", "key": "exisitflightno" },
                { "header": "Class", "key": "classcode" },
                //{ "header": "TICKET STATUS FLAG", "key": "TICKET_STATUS_FLAG" },
                { "header": "Fare Basis", "key": "fare_basis" },
                { "header": "Dept.Terminal", "key": "TCK_DEP_TERMINAL" },
                { "header": "Arr.Terminal", "key": "TCK_ARR_TERMINAL" },
                { "header": "Via", "key": "TCK_VIA" },
                { "header": "Flying Time", "key": "TCK_FLYING_HOURS" },

            ]
        });
        $('#divTicket_New_flt').columns({

            data: ResheduleFlt,
            schema: [

                { "header": "Origin", "key": "origin", },
                { "header": "Destination", "key": "destination", },
                { "header": "Departure", "key": "reschduledate", },
                { "header": "Arrival ", "key": "reschduledate" },
                { "header": "Flight No.", "key": "reschduleflight" },
                { "header": "Class", "key": "classcode" },
                //{ "header": "TICKET STATUS FLAG", "key": "TICKET_STATUS_FLAG" },
                { "header": "Fare Basis", "key": "fare_basis" },
                {
                    "header": "Dept.Terminal", "key": "TCK_DEP_TERMINAL",
                    "condition": function (val, row) {
                        var retval = '<input type="text" class="form-control clsnewfltdeptterminal" value="' + val + '"  id="">'
                        return retval;
                    }
                },
                {
                    "header": "Arr.Terminal", "key": "TCK_ARR_TERMINAL",
                    "condition": function (val, row) {
                        var retval = '<input type="text" class="form-control clsnewfltarrterminal" value="' + val + '"  id="">'
                        return retval;
                    }
                },
                {
                    "header": "Via", "key": "TCK_VIA",
                    "condition": function (val, row) {
                        var retval = '<input type="text" class="form-control clsnewfltvia" value="' + val + '"  id="">'
                        return retval;
                    }
                },
                {
                    "header": "Flying Time", "key": "TCK_FLYING_HOURS",
                    "condition": function (val, row) {
                        var retval = '<input type="text" class="form-control clsnewfltflyhours timevalidate" value="' + val + '" placeholder="00:00" onblur="javascript:onHrsBlur(this.id);" onkeyup="javascript:onHrsValid(this.id);"  id="txtidnewfltflyhrs">'
                        return retval;
                    }
                },

            ]
        });
    }
    //ReshedulePaxdetails
    var s = "";
    s += ' <table class="ui-table no-more-tables">';

    s += '<thead class="tablehead">';
    s += '<tr>';
    s += '<th>Passenger  Name</th>';
    s += '<th>Ticket No.</th>';

    s += '<th>Supplier Penalty</th>';

    s += '<th>Reshedule Charges</th>';
    s += '<th>Fare Difference</th>';
    s += '<th>Markup</th>';
    s += '<th>Gross Fare</th>';
    s += '<th>YQ</th>';
    s += '<th>YQ</th>';
    s += '<th>OT</th>';
    s += '<th>OT</th>';
    s += '<th>K3</th>';
    s += '<th>K3</th>';
    s += '</tr>';
    s += '</thead>';

    s += '<tbody>';

    response = response || [];
    var Penaltyamt = 0;
    var FareDiff = 0;
    var creditshel = 0;
    $("#hdn_confirmreschdule").val("");
    if (response.length) {
        var GetFareDetails = JSON.parse(response[3]);

        if (GetFareDetails) {
            Penaltyamt = GetFareDetails.rootNode[0].Penalty;// response[3].split("|")[0];
            FareDiff = GetFareDetails.rootNode[0].FareDifference; //response[3].split("|")[1];
            creditshel = GetFareDetails.rootNode[0].CSAmount != null && GetFareDetails.rootNode[0].CSAmount != undefined ? GetFareDetails.rootNode[0].CSAmount : "";

            $("#hdn_reschedule_airpenalty").val(Penaltyamt);
            $("#hdn_reschedule_farediff").val(FareDiff);

            $("#hdn_confirmreschdule").val("Y");
        }
    }



    var Yq = 0;
    var Ot = 0;
    var K3 = 0;
    for (var scnt = 0; scnt < Reshpaxdeta.length; scnt++) {
        var txcode = Reshpaxdeta[scnt].PCI_TAX_BREAKUP.split("/");
        Yq = 0;
        for (var i = 0; i < txcode.length; i++) {
            if (txcode[i].split(":")[0] == "YQ")
                Yq = txcode[i].split(":")[1]
            else if (txcode[i].split(":")[0] == "OT")
                Ot = txcode[i].split(":")[1]
            else if (txcode[i].split(":")[0] == "K3")
                K3 = txcode[i].split(":")[1]
        }
        s += '<tr class="Reshedclassdetails">';
        s += '<td id="Paxname_' + scnt + '">' + Reshpaxdeta[scnt]["passenger"] + '</td>';
        s += '<td><input type="text" class="form-control" id="Ticktno_' + scnt + '" /></td>';

        var enabldisfields = Number(Reshpaxdeta[scnt]["GROSS"]) > 0 ? "" : "disabled";

        s += '<td><input type="text" ' + enabldisfields + ' class="form-control chragecls" value="' + (Number(Reshpaxdeta[scnt]["GROSS"]) > 0 ? Penaltyamt : 0) + '" onkeypress="isNumericVal(event)" id="Supplier_Penalty_' + scnt + '" /></td>';
        s += '<td><input type="text" ' + enabldisfields + ' class="form-control chragecls" value="0" onkeypress="isNumericVal(event)" id="Reshedule_Charges_' + scnt + '" /></td>';
        s += '<td><input type="text" ' + enabldisfields + ' class="form-control chragecls" value="' + (Number(Reshpaxdeta[scnt]["GROSS"]) > 0 ? FareDiff : 0) + '" onkeypress="isNumericVal(event)" id="Fare_Difference_' + scnt + '" /></td>';
        s += '<td><input type="text"  ' + enabldisfields + ' class="form-control chragecls" value="0" onkeypress="isNumericVal(event)" id="Markup_' + scnt + '" /></td>';
        s += '<td id="GrossAMT_' + scnt + '" style="text-align:right">' + Reshpaxdeta[scnt]["GROSS"] + '</td>';

        s += '<td id="YQ_' + scnt + '" style="text-align:right">' + Yq + '</td>';
        s += '<td><input type="text" ' + enabldisfields + '  class="form-control" value="0" onkeypress="isNumericVal(event)" id="YQ_edit' + scnt + '" /></td>';
        s += '<td id="OT_' + scnt + '" style="text-align:right">' + Ot + '</td>';
        s += '<td><input type="text" ' + enabldisfields + '  class="form-control" value="0" onkeypress="isNumericVal(event)" id="OT_edit' + scnt + '" /></td>';
        s += '<td id="K3' + scnt + '" style="text-align:right">' + K3 + '</td>';
        s += '<td><input type="text" ' + enabldisfields + '  class="form-control" value="0" onkeypress="isNumericVal(event)" id="K3_edit' + scnt + '" /></td>';
        s += '</tr>';
    }
    s += '</tbody>';

    s += '</table>';
    $(".ReshedulePaxdetails").html(s);

    s = "";
    for (var scnt = 0; scnt < ResheduleFlt.length; scnt++) {

        s += "<div class='col-lg-12 Reshfltdetails' style='Padding:0px;'>";
        s += "<label style='color:red'>" + ResheduleFlt[scnt]["origin"] + " - " + ResheduleFlt[scnt]["destination"] + "</label>"
        s += "<div class='row'>";

        s += "<div class='col-sm-3 text-left'>";
        s += "<label>Origin</label>"
        s += '<input type="text" class="form-control txtclscityairport" id="Org_Code' + scnt + '" value="' + Loadname(ResheduleFlt[scnt]["origin"]) + '" />';
        s += "</div>";
        s += "<div class='col-sm-3 text-left'>";
        s += "<label>Destination</label>"
        s += '<input type="text" class="form-control txtclscityairport" id="Des_Code' + scnt + '" value="' + Loadname(ResheduleFlt[scnt]["destination"]) + '" />';
        s += "</div>";
        s += "<div class='col-sm-2 text-left'>";
        s += "<label>Flight No.</label>"
        s += '<input type="text" class="form-control" id="Flt_no' + scnt + '" value="' + ResheduleFlt[scnt]["reschduleflight"] + '" />';
        s += "</div>";
        s += "<div class='col-sm-2 text-left'>";
        s += "<label>Airline PNR</label>"
        s += '<input type="text" class="form-control" style="text-transform: uppercase;" id="Airline_pnr' + scnt + '"  />';
        s += "</div>";
        s += "<div class='col-sm-2 text-left'>";
        s += "<label>CRS PNR</label>"
        s += '<input type="text" class="form-control" style="text-transform: uppercase;" id="CRS_PNR' + scnt + '"  />';
        s += "</div>";
        s += "</div>";
        
        s += "<div class='row' style='margin-bottom:10px;'>";
        s += "<div class='col-sm-3 text-left'>";
        s += "<label>Departure Date</label>"
        s += '<input type="text" class="form-control Datepicker" id="Departure' + scnt + '" value="' + ResheduleFlt[scnt]["reschduledate"].split(" ")[0] + '" />';
        s += "</div>";
        s += "<div class='col-sm-3 text-left'>";
        var deptime = ResheduleFlt[scnt]["reschduledate"].split(" ")[1].trim() != "00:00" ? ResheduleFlt[scnt]["reschduledate"].split(" ")[1].trim() : "";
        var arrtime = ResheduleFlt[scnt]["reschduledate"].split(" ")[1].trim() != "00:00" ? ResheduleFlt[scnt]["reschduledate"].split(" ")[1].trim() : "";
        s += "<label>Departure Time</label>"
        s += '<input type="text" class="form-control timevalidate" id="Departure_time' + scnt + '" value="' + deptime + '"  onchange="javascript:myfunction(this.id); return false;" onblur="javascript:onHrsBlur(this.id);" onkeyup="javascript:onHrsValid(this.id);" onkeypress="javascript:return isNumericVal(event);"  maxlength="5" placeholder="00:00" />';
        s += "</div>";
        s += "<div class='col-sm-3 text-left'>";
        s += "<label>Arrival  Date</label>"
        s += '<input type="text" class="form-control Datepicker" id="Arrival' + scnt + '" value="' + ResheduleFlt[scnt]["reschduledate"].split(" ")[0] + '"  />';
        s += "</div>";
        s += "<div class='col-sm-3 text-left'>";
        s += "<label>Arrival Time</label>"
        s += '<input type="text" class="form-control timevalidate" id="Arrival_time' + scnt + '"  value="' + arrtime + '"   onchange="javascript:myfunction(this.id); return false;" onblur="javascript:onHrsBlur(this.id);" onkeyup="javascript:onHrsValid(this.id);" onkeypress="javascript:return isNumericVal(event);" maxlength="5" placeholder="00:00" />';
        s += "</div>";

        s += "</div>";
        s += "</div>";
    }


    $(".Edit_resh_flt_details").html(s);
    loaddatepicker(ResheduleFlt.length);
    $(".form-control").keyup(function () {
        $(".form-control").removeClass("bordercls")
    })
    $(".timevalidate").mask('00:00');
    $(".chragecls").keyup(function (u, i) {
        
        var totmt = 0;
        $(".chragecls").each(function (s, m) {
            var ids = this.id;
            totmt += $("#" + ids).val() == "" ? Number(0) : Number($("#" + ids).val());
        })
        var amtc = $(".SSRamt").html();
        var totcalc = Number(amtc) + Number(totmt);
        $(".totamtcal").show()
        if (totcalc > 0)
            $(".totsmt").html(Number(amtc) + Number(totmt))
        else
            $(".totamtcal").hide()
        //  $(".Netamot").html("Net Amount : " + totmt + "")

    })
    $(".reshedule").slideUp()
    $(".TobeReshcolumndiv").slideDown("slow");
}
$(".popclose").click(function () {
    $(".TobeReshcolumndiv").slideUp()
    $(".reshedule").slideDown("slow");
})
function Toclose(spnr, Orgcode, DesCode, Rsdlno) {
    $("#txt_remarkMdl").val("");
    $("#ModalRemarks").modal('show');
    $("body").data("Toclose", spnr + "|" + Orgcode + "|" + DesCode + "|" + Rsdlno);
    // $("body").data("Rsdlno", Rsdlno);
}
function Tobereshduel() {
    
    //if ($("#hdn_confirmreschdule").val() == "Y") {
    //    Confirmreschedule();
    //}
    var validate = false;
    var aircate = ResheduleFlt[0].TCK_AIRLINE_CATEGORY
    $(".Reshfltdetails .form-control").each(function () {
        if ($(this).val().trim() == "") {
            if ((aircate == "LCC" &&
                (this.id.indexOf("CRS_PNR") != -1)
                && $(this).val().trim() == "")
                ) { }
            else {
                validate = true;
                $(this).addClass("bordercls");
                $(this).focus();
            }
        }
    })
    if ($("#Txtremarksboa").val().trim() == "") {
        validate = true;
        $("#Txtremarksboa").addClass("bordercls")
    }
    $(".ReshedulePaxdetails .form-control").each(function () {
        if ($(this).val().trim() == "") {
            if ((aircate == "LCC" &&
               (this.id.indexOf("Ticktno_") != -1)
               && $(this).val().trim() == "")
               ) { }
            else {
                validate = true;
                $(this).addClass("bordercls");
                $(this).focus();
            }

        }
    })

    if (validate) {
        return false;
    }



    var paxary = [];

    var s = "<event>";
    var Adultcnt = 0;
    var childcnt = 0;
    var infantcnt = 0;
    var m = "";
    if (newssr.length > 0) {
        m = "<event>";
        for (var i = 0; i < newssr.length; i++) {
            m += "<SSR><SSR_PAX_REF_NO>" + newssr[i].SSR_PAX_REF_NO + "</SSR_PAX_REF_NO>"
            m += "<SSR_SEGMENT_NO>" + newssr[i].SSR_SEGMENT_NO + "</SSR_SEGMENT_NO><SSR_SSR_FLAG>" + newssr[i].SSR_SSR_FLAG + "</SSR_SSR_FLAG>"
            m += "<SSR_SSR_NAME>" + newssr[i].SSR_SSR_NAME + "</SSR_SSR_NAME><SSR_SSR_AMOUNT>" + newssr[i].SSR_SSR_AMOUNT + "</SSR_SSR_AMOUNT></SSR>"
        }
        m += "</event>"
    }
    // else if (SSRdet.length > 0) {
    //    m = "<event>";
    //    for (var i = 0; i < SSRdet.length; i++) {
    //        m += "<SSR><SSR_PAX_REF_NO>" + SSRdet[i].SSR_PAX_REF_NO + "</SSR_PAX_REF_NO>"
    //        m += "<SSR_SEGMENT_NO>" + SSRdet[i].SSR_SEGMENT_NO + "</SSR_SEGMENT_NO><SSR_SSR_FLAG>" + SSRdet[i].SSR_SSR_FLAG + "</SSR_SSR_FLAG>"
    //        m += "<SSR_SSR  Paxdfeta_NAME>" + SSRdet[i].SSR_SSR_NAME + "</SSR_SSR_NAME><SSR_SSR_AMOUNT>" + SSRdet[i].SSR_SSR_AMOUNT + "</SSR_SSR_AMOUNT></SSR>"
    //    }
    //    m += "</event>"
    //}
    var paxdta = Reshpaxdeta.reduce(function (memo, e1) {
        var matches = memo.filter(function (e2) {
            return e1.pax_ref_no == e2.pax_ref_no
        })
        if (matches.length == 0)
            memo.push(e1)
        return memo;
    }, [])
    for (var t = 0; t < paxdta.length; t++) {
        paxdta[t].passenger_type == "ADT" ? Adultcnt++ : Reshpaxdeta[t].passenger_type == "CHD" ? childcnt++ : Reshpaxdeta[t].passenger_type == "INF" ? infantcnt++ : "";
    }
    $(".Reshedclassdetails").each(function (u, i) {


        s += "<Passenger>";
        s += "<Passengername>" + $("#Paxname_" + u).text() + "</Passengername>";
        s += "<PASSTYPE>" + Reshpaxdeta[u].passenger_type + "</PASSTYPE>";
        s += "<PNR></PNR>";
        s += "<SEGMENT_NO>" + Reshpaxdeta[u].PCI_SEGMENT_NO + "</SEGMENT_NO>";

        s += "<TICKETNO>" + $("#Ticktno_" + u).val() + "</TICKETNO>";
        s += "<PAX_REF_NO>" + ResheduleFlt[u].pax_ref_no + "</PAX_REF_NO>";
        s += "<SUPPILER_PENALITY>" + $("#Supplier_Penalty_" + u).val() + "</SUPPILER_PENALITY>";
        s += "<AGENT_PENALITY>" + $("#Reshedule_Charges_" + u).val() + "</AGENT_PENALITY>";
        s += "<FARE_DIFFERENCE>" + $("#Fare_Difference_" + u).val() + "</FARE_DIFFERENCE>";
        s += "<FARE_ID>" + Reshpaxdeta[u].fare_id + "</FARE_ID>";
        s += "<BAGGAGE>" + Reshpaxdeta[u].BAGGAGE + "</BAGGAGE>";
        s += "<BAGGAGE_AMT>" + Reshpaxdeta[u]["BAGGAGE AMT"] + "</BAGGAGE_AMT>";
        s += "<MEALS>" + Reshpaxdeta[u].MEALS + "</MEALS>";
        s += "<MEALS_AMT>" + Reshpaxdeta[u]["MEALS AMT"] + "</MEALS_AMT>";
        s += "<SEAT>" + Reshpaxdeta[u].SEAT + "</SEAT>";
        s += "<SEAT_AMT>" + Reshpaxdeta[u]["SEAT AMT"] + "</SEAT_AMT>";
        s += "<WC>" + Reshpaxdeta[u]["WHEEL CHAR"] + "</WC>";
        s += "<WC_AMT>" + Reshpaxdeta[u]["WHEEL CHAR AMT"] + "</WC_AMT>";
        s += "<OK>" + Reshpaxdeta[u].OKBOARD + "</OK>";
        s += "<OK_AMT>" + Reshpaxdeta[u]["OKBOARD AMT"] + "</OK_AMT>";
        s += "<MARKUP>" + $("#Markup_" + u).val() + "</MARKUP>";
        s += "<CLIENTPENALTY>" + $("#Reshedule_Charges_" + u).val() + "</CLIENTPENALTY>";
        s += "<CURRENCY>" + $("#currency").val() + "</CURRENCY>";
        s += "<YQOLD>" + $("#YQ_" + u).text() + "</YQOLD>";
        s += "<YQ>" + $("#YQ_edit" + u).val() + "</YQ>";
        s += "<K3OLD>" + $("#K3" + u).text() + "</K3OLD>";
        s += "<K3>" + $("#K3_edit" + u).val() + "</K3>";
        s += "<OTOLD>" + $("#OT_" + u).text() + "</OTOLD>";
        s += "<OT>" + $("#OT_edit" + u).val() + "</OT>";
        s += "</Passenger>";
    })
    s += "</event>";
    var t = "<event>";
    var depdate = "";
    var arrdate = "";
    $(".Reshfltdetails").each(function (s, m) {
        
        depdate = $("#Departure" + s).val().trim().split("/")[1] + "/" + $("#Departure" + s).val().trim().split("/")[0] + "/" + $("#Departure" + s).val().trim().split("/")[2];
        arrdate = $("#Arrival" + s).val().trim().split("/")[1] + "/" + $("#Arrival" + s).val().trim().split("/")[0] + "/" + $("#Arrival" + s).val().trim().split("/")[2];
        t += "<Ticket>"
        t += "<PNR></PNR>"
        t += "<AGENTID>" + ResheduleFlt[s].agentid + "</AGENTID>"
        t += "<TERMINALID>" + ResheduleFlt[s].terminalid + "</TERMINALID>"
        t += "<FLIGHTNO>" + $("#Flt_no" + s).val().trim() + "</FLIGHTNO>"
        t += "<ORIGIN>" + $("#Org_Code" + s).val().trim().split("(")[1].replace(")", "").trim() + "</ORIGIN>"
        t += "<DESTINATION>" + $("#Des_Code" + s).val().trim().split("(")[1].replace(")", "").trim() + "</DESTINATION>"
        t += "<DEPARTURE_DATE>" + depdate + " " + $("#Departure_time" + s).val().trim() + "</DEPARTURE_DATE>"
        t += "<ARRIVAL_DATE>" + arrdate + " " + $("#Arrival_time" + s).val().trim() + "</ARRIVAL_DATE>"
        t += "<ADULT>" + Adultcnt + "</ADULT>"
        t += "<CHILD>" + childcnt + "</CHILD>"
        t += "<INFANT>" + infantcnt + "</INFANT>"
        t += "<USERNAME>" + Username + "</USERNAME>"
        t += "<AIRPORT_ID>" + ResheduleFlt[s].airport_id + "</AIRPORT_ID>"
        t += "<AIRPNR>" + $("#Airline_pnr" + s).val().trim() + "</AIRPNR>"
        t += "<CRSPNR>" + $("#CRS_PNR" + s).val().trim() + "</CRSPNR>"
        t += "<SEGMENT_NO>" + ResheduleFlt[s].segment_no + "</SEGMENT_NO>"
        t += "<EXIST_S_SPNR>" + ResheduleFlt[s].spnr + "</EXIST_S_SPNR>"
        t += "<BOOKINGTYPE />"
        t += "<PAX_REF_NO>" + ResheduleFlt[s].pax_ref_no + "</PAX_REF_NO>"
        t += "<CLASS>" + ResheduleFlt[s].classcode + "</CLASS>"
        t += "<FAREBASIS>" + ResheduleFlt[s].fare_basis + "</FAREBASIS>"
        t += "<CLIENTID>" + ResheduleFlt[s].clientid + "</CLIENTID>"
        t += "<COORDINATORCODE>" + ResheduleFlt[s].coodinatorcode + "</COORDINATORCODE>"
        t += "<BOABRANCHID>" + ResheduleFlt[s].branchid + "</BOABRANCHID>"
        t += "<BOAPOSID>" + ResheduleFlt[s].POSID + "</BOAPOSID>"
        t += "<BOATERMINALID>" + ResheduleFlt[s].POSterminalid + "</BOATERMINALID>"
        t += "<FORMOFPAYMENT>" + ResheduleFlt[s].paymentmodeclient + "</FORMOFPAYMENT>"
        t += "<STOCKTYPE>" + ResheduleFlt[s].STOCKTYPE + "</STOCKTYPE>"
        t += "<EMPLOYEEID />"
        t += "<TCK_DEP_TERMINAL>" + $(".clsnewfltdeptterminal").eq(s).val() + "</TCK_DEP_TERMINAL>";
        t += "<TCK_ARR_TERMINAL>" + $(".clsnewfltarrterminal").eq(s).val() + "</TCK_ARR_TERMINAL>"
        t += "<TCK_VIA>" + $(".clsnewfltvia").eq(s).val() + "</TCK_VIA>"
        t += "<TCK_FLYING_HOURS>" + $(".clsnewfltflyhours").eq(s).val() + "</TCK_FLYING_HOURS>"
        t += "<REMARKS />"
        t += "</Ticket>"

    })

    t += "</event>";
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + Spinner + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });
    var input = {
        Paxdetails: s,
        Ticketdata: t,
        spnr: ResheduleFlt[0].spnr,
        SSRinfo: m,
        Paxcount: paxdta.length,
        RescheduleStatus: RescheduleStatus,
        OnlineFlag: $("#hdn_confirmreschdule").val() || ""
    }

    $.ajax({
        type: "POST",
        url: Toresheule_boa,
        data: JSON.stringify(input),
        contentType: "application/json; charset=utf-8",
        timeout: 180000,
        dataType: "json",
        success: function (data) {
            
            $.unblockUI();
            //alert(data.Result);
            if (data.Status == "01") {
                Lobibox.alert('success', {   //eg: 'error' , 'success', 'info', 'warning'
                    msg: data.Result,
                    closeOnEsc: false,
                    callback: function ($this, type) {
                        // location.reload();
                        FormingResSuccessDetails(data.Result);
                    }
                });
            } else {
                Lobibox.alert('error', {   //eg: 'error' , 'success', 'info', 'warning'
                    msg: data.Result,
                    closeOnEsc: false,
                    callback: function ($this, type) {
                        location.reload();
                    }
                });
            }

        },
        error: function (e) {

        }
    });
}

function loaddatepicker(totcnt) {

    for (dtcnt = 0; dtcnt < totcnt; dtcnt++) {
        $("#Departure" + dtcnt).datepicker({
            numberOfMonths: 1,
            showButtonPanel: false,
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            minDate: ServerDate,
            onSelect: function (view, elements) {
                var curscnt = elements.id[elements.id.length - 1];
                InvokeArrDatePicker(curscnt, view, "Y");
            },
        });
        InvokeArrDatePicker(dtcnt, ServerDate);
    }



    $('.txtclscityairport').typeahead({
        source: productNames,
        items: 8,
        scrollBar: true,


    });

}
function InvokeArrDatePicker(dtcnt, mindate, defaultflg) {
    $("#Arrival" + dtcnt).datepicker("destroy");
    $("#Arrival" + dtcnt).datepicker({
        numberOfMonths: 1,
        showButtonPanel: false,
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        minDate: mindate,
    });
    if (defaultflg == "Y")
        $("#Arrival" + dtcnt).val(mindate);
}

function Loadname(arg) {
    
    try {
        var Arrlist = $.grep(json.AIR, function (u, i) {
            return u.ID == arg.trim()
        });
        return Arrlist[0].AN.split("-")[0] + "-" + "(" + Arrlist[0].ID + ")";
        //return text
    } catch (e) {

        return arg
    }

}
var texcodemat = 0;
function Addssr() {
    
    var seg = "";
    var s = "";
    s += "<div class='row ssrcls' style='margin-bottom:15px;'>";
    s += "<div class='col-sm-6'>";
    s += "<div class='row'>";
    s += '<div class="col-sm-3">';
    s += '<label for="sltcurrcode">Passenger</label>';
    s += '    <select name="Currency" id="PassengerSSR" class="form-control validatecls">';
    s += "<option value=''>Select</option>";
    seg = "<option value=''>Select</option>";
    for (var i = 0; i < Reshpaxdeta.length; i++) {

        s += "<option value=" + Reshpaxdeta[i].PSG_PAX_REF_NO + ">" + Reshpaxdeta[i].passenger + "</option>";
        //seg += "<option value=" + SSRdet[i].SSR_SEGMENT_NO + ">" + SSRdet[i].SSR_SEG_DETAILS + "</option>";
    }
    for (var i = 0; i < ResheduleFlt.length; i++) {

        //s += "<option value=" + Reshpaxdeta[i].PSG_PAX_REF_NO + ">" + Reshpaxdeta[i].passenger + "</option>";
        seg += "<option value=" + ResheduleFlt[i].segment_no + ">" + ResheduleFlt[i].origin + "->" + ResheduleFlt[i].destination + "</option>";
    }
    s += '    </select>';
    s += '</div>';
    s += '<div class="col-sm-3">';
    s += '<label for="sltcurrcode">Segment</label>';
    s += '    <select name="Currency" id="SSR_seg" class="form-control validatecls">';
    s += seg;
    s += '    </select>';
    s += '</div>';
    s += '<div class="col-sm-3">';
    s += '<label for="sltcurrcode">SSR Type</label>';
    s += '<select name="Currency" id="SSR_Type" class="form-control validatecls">';

    s += "<option value=''>Select</option>";
    s += "<option value='B'>Baggage</option>";
    s += "<option value='E'>Meal</option>";
    s += "<option value='K'>Seat</option>";
    s += "<option value='K'>Wheel Chair</option>";
    s += "<option value='K'>Fast Forward Service</option>";
    s += "<option value='K'>OkToBoard</option>";
    s += "<option value='K'>Other SSR</option>";
    s += ' </select>';
    s += '</div>';
    s += '<div class="col-sm-3">';
    s += '<label for="sltcurrcode">SSR Details</label>';
    s += ' <input type="text" name="Corporate Name" data-mandatory="Y" id="SSR_Details" class="form-control validatecls " value="">';

    s += '</div>'
    s += "</div>";
    s += "</div>";
    s += "<div class='col-sm-6'>";
    s += "<div class='row'>";
    s += '<div class="col-sm-3">';
    s += '<label for="sltcurrcode">Amount</label>';
    s += ' <input type="text" name="Corporate Name" data-mandatory="Y" id="SSR_Amount"  onkeypress="isNumericVal(event)" class="form-control validatecls " value="">';

    s += '</div>'
    s += '<div class="col-sm-3">';
    s += '<label for="sltcurrcode">Mark up</label>';
    s += ' <input type="text" name="Corporate Name" data-mandatory="Y" id="SSR_Markup"  onkeypress="isNumericVal(event)" class="form-control  " value="">';

    s += '</div>'
    s += '<div class="col-sm-2">';
    s += '<label for="sltcurrcode">Ticket No.</label>';
    s += ' <input type="text" name="Corporate Name" data-mandatory="Y" id="SSR_TicketNo" class="form-control  " value="">';

    s += '</div>'
    s += '<div class="col-sm-2">';
    s += '<label for="sltcurrcode">Tax Breakup</label>';
    s += ' <input type="text" name="Corporate Name" data-mandatory="Y" disabled style="margin-bottom:6px;" id="SSR_Taxbreakup" class="form-control  " value="">';
    s += "<span style='postion:absolute;cursor:pointer;text-decoration: underline;color:red;white-space:nowrap;' onclick='Addbreak()'>Add Breakup</span>"
    s += '</div>'
    s += '<div class="col-sm"-2>';

    s += ' <button type="button" class="btn btn-primary" style="width:79px;position:relative;top:25px;" onclick="Addbtn()">Add</button>';

    s += '</div>'
    s += '</div>'
    s += "</div>";
    s += "</div>";
    $(".paxssr").html(s);
    if ($('#divTicket_SSR table').length > 0) {
        $('#divTicket_SSR').columns("destroy");
    } else {
        $("#divTicket_SSR").html("No SSR details")
    }
    var cnt = 0;
    if (SSRdet.length > 0) {
        $('#divTicket_SSR').columns({

            data: SSRdet,
            schema: [
                 //{
                 //    "header": "Select", "key": "Select",//(Tmc == "RIYA" ? "SUPPLIER_PNR" : "S_PNR"),

                 //    "condition": function (val, row) {
                 //        cnt++;
                 //        val = '<input type="checkbox" id="selectbox_' + cnt + '">';
                 //        return val;
                 //    }

                 //},
                 {
                     "header": "Edit", "key": "Edit",//(Tmc == "RIYA" ? "SUPPLIER_PNR" : "S_PNR"),

                     "condition": function (val, row) {

                         val = '<button class="showPopup clsmyButton btn-color" onclick="ssredit(' + row.ID + ')">Edit</button>';
                         return val;
                     }

                 },
                { "header": "Passenger", "key": "PASSENGER", },
                { "header": "SSR Type", "key": "SSR_SSR_TYPE", },
                { "header": "SSR Name", "key": "SSR_SSR_NAME", },
                { "header": "Amount ", "key": "SSR_SSR_AMOUNT" },
                { "header": "Segment Details", "key": "SSR_SEG_DETAILS" },
                { "header": "Markup", "key": "SSR_MARKUP" },

                { "header": "Ticket No.", "key": "SSR_TICKET_NO" },
                { "header": "Tax Breakup", "key": "SSR_TAX_BREAKUP" },


            ]
        });
    }
    $(".Addbreakup").slideUp()
    $(".Fullssr").slideDown("slow");
    $("#ModalSSRpopupResh").modal({
        backdrop: 'static',
        keyboard: false
    });
}

function ssredit(arg) {
    
    var ssrd = $.grep(SSRdet, function (s, m) {
        return s.ID == arg
    })
    $("#PassengerSSR").val(ssrd[0].SSR_PAX_REF_NO)
    $("#SSR_seg").val(ssrd[0].SSR_SEGMENT_NO)
    $("#SSR_Type").val(ssrd[0].SSR_SSR_FLAG)
    $("#SSR_Details").val(ssrd[0].SSR_SSR_NAME)
    $("#SSR_Amount").val(ssrd[0].SSR_SSR_AMOUNT == "" ? "0" : ssrd[0].SSR_SSR_AMOUNT)
    $("#SSR_Markup").val(ssrd[0].SSR_MARKUP == "" ? "0" : ssrd[0].SSR_MARKUP)
    $("#SSR_TicketNo").val(ssrd[0].SSR_TICKET_NO)
    $("#SSR_Taxbreakup").val(ssrd[0].SSR_TAX_BREAKUP)
}
var newssr = [];
var uniqkey = 0;
function Addbtn() {
    //var ssrd = $.grep(SSRdet, function (s, m) {
    //    return s.SSR_PAX_REF_NO == $("#PassengerSSR").val()
    //})
    
    var valideflag = false;
    $(".validatecls").each(function () {
        if ($(this).val().trim() == "") {
            valideflag = true;
            $(this).addClass("bordercls")
        }
    })

    if (valideflag) {
        return false;
    }
    //if ($("#SSR_Amount").val() != texcodemat) {
    //    // alert("SSR amount and breakup amount should be same ")
    //    LobiboxAlert('warning', "SSR amount and breakup amount should be same", '', '');
    //    return false;
    //}
    //  var amttot = Number($(".totsmt").html()) + Number(texcodemat);
    // $(".totsmt").html(amttot);
    texcodemat = 0;
    uniqkey++;
    var newssrinfo = {
        SSR_PAX_REF_NO: $("#PassengerSSR").val(),
        PASSENGER: $("#PassengerSSR option:selected").text(),
        SSR_SEGMENT_NO: $("#SSR_seg").val(),
        SSR_SEG_DETAILS: $("#SSR_seg option:selected").text(),
        SSR_SSR_TYPE: $("#SSR_Type  option:selected").text(),
        SSR_SSR_FLAG: $("#SSR_Type").val(),
        SSR_SSR_NAME: $("#SSR_Details").val(),
        SSR_SSR_AMOUNT: $("#SSR_Amount").val(),
        SSR_MARKUP: $("#SSR_Markup").val(),
        SSR_TICKET_NO: $("#SSR_TicketNo").val(),
        SSR_TAX_BREAKUP: $("#SSR_Taxbreakup").val(),
        SSR_unikey: uniqkey,
    }
    newssr.push(newssrinfo)
    $(".Newssr").show()
    if ($('#divTicket_New_SSR table').length > 0) {
        $('#divTicket_New_SSR').columns("destroy");
    }
    var cnt = 0;
    $('#divTicket_New_SSR').columns({

        data: newssr,
        schema: [
             //{
             //    "header": "Select", "key": "Select",//(Tmc == "RIYA" ? "SUPPLIER_PNR" : "S_PNR"),

             //    "condition": function (val, row) {
             //        cnt++;
             //        val = '<input type="checkbox" id="selectbox_' + cnt + '">';
             //        return val;
             //    }

             //},
             {
                 "header": "Action", "key": "Edit",//(Tmc == "RIYA" ? "SUPPLIER_PNR" : "S_PNR"),

                 "condition": function (val, row) {

                     val = '<button class="showPopup clsmyButton btn-color" id="Toremove_' + cnt + '" onclick="Remove(' + row.SSR_unikey + ')">Cancel</button>';
                     cnt++;
                     return val;
                 }

             },
            { "header": "Passenger", "key": "PASSENGER", },
            { "header": "SSR Type", "key": "SSR_SSR_TYPE", },
            { "header": "SSR Name", "key": "SSR_SSR_NAME", },
            { "header": "Amount ", "key": "SSR_SSR_AMOUNT" },
            { "header": "Segment Details", "key": "SSR_SEG_DETAILS" },
            { "header": "Markup", "key": "SSR_MARKUP" },

            { "header": "Ticket No.", "key": "SSR_TICKET_NO" },
            { "header": "Tax Breakup", "key": "SSR_TAX_BREAKUP" },


        ]
    });
    $(".ssrcls .form-control").val("");
}
function Addbreak() {
    
    $(".taxbreddiv .form-control").val("");
    $(".Fullssr").slideUp()
    $(".Addbreakup").slideDown("slow")
}
$(".SSRclose").click(function () {
    $(".Addbreakup").slideUp()
    $(".Fullssr").slideDown("slow");
})
function Adddivbreakup(arg) {
    
    if (5 > $(".taxbreddiv").length) {
        var i = arg.id.split("_")[1];
        i = Number(i) + Number(1);
        var h = $(".taxbreddiv").first().html();
        h = h.replace("Div_1", "Div_" + i).replace("Codetext_1", "Codetext_" + i)
           .replace("Codeamount_1", "Codeamount_" + i)
           .replace("Addbrekbtn_1", "Addbrekbtn_" + i)
           .replace("Removebtn_1", "Removebtn_" + i)
        var s = '<div class="row taxbreddiv" id="Div_' + i + '">' + h + '</div>';
        var prev = arg.id.split("_")[1];
        $("#Addbrekbtn_" + prev).hide()
        $("#Removebtn_" + prev).hide()

        $("#addbreakuptext").append(s);
        $("#Addbrekbtn_" + i).show()
        $("#Removebtn_" + i).show()
    }
}
function Removediv(arg) {
    var cnt = $(".taxbreddiv").length;
    if (cnt != 1) {
        var i = arg.id.split("_")[1];
        var prev = arg.id.split("_")[1] - 1;
        $("#Div_" + i).remove();
        $("#Addbrekbtn_" + prev).show()
        $("#Removebtn_" + prev).show()
    }

}
function Taxokbtn() {
    
    var validate = false;
    var text = "";
    $("#addbreakuptext .form-control").each(function () {
        if ($(this).val() == "") {
            $(this).addClass("bordercls")
            validate = true;
        }
    })
    if (validate) {
        return false;
    }
    var totlent = $(".taxbreddiv").length;
    var c = 0;
    for (var i = 0; i < totlent; i++) {
        c = Number(i) + 1;
        text += $("#Codetext_" + c).val() + ":" + $("#Codeamount_" + c).val() + "/";
        texcodemat += Number($("#Codeamount_" + c).val())
    }

    // text = text.replace("/", "");
    $("#SSR_Taxbreakup").val(text.toUpperCase());
    $(".Addbreakup").slideUp()
    $(".Fullssr").slideDown("slow");
}

function myfunction(ids) {
    
    // var indexx = ids.replace("txtTimeStart", "");
    var value = $("#" + ids).val();
    var parts = value.split(':');
    //if ($("#" + ids).val().length != 5) {
    //    Lobibox.alert('warning', {   //eg: 'error' , 'success', 'info', 'warning'
    //        msg:'Please enter valid start time.',
    //        closeOnEsc: false,
    //        callback: function ($this, type) {
    //            $("#" + ids).val("");
    //            $("#" + ids).focus();

    //        }
    //    });
    //    return false;
    //}
    if ($("#" + ids).val().length != 5 || parts[0] > 23 || parts[1] > 59 || parts[2] > 59) {
        LobiboxAlert('warning', "Please enter valid start time", '', '');
        // alert("Please enter valid start time.")
        return false;
    }
    var T_HH_and_MM = $("#" + ids).val().split(":");
    var taketime = "";

    var newHr = T_HH_and_MM[0];
    var newMins = T_HH_and_MM[1];
    newMins = Number(newMins) - 15;
    if (newMins < 0) {
        newMins = newMins * (-1);
        newMins = 60 - (Number(newMins));

        newHr = newHr > 0 ? Number(newHr) - 1 : 23;
    }
    newMins = newMins < 10 ? "0" + newMins : newMins;
    newHr = newHr < 10 ? newHr : newHr;

    taketime = newHr.toString() + ":" + newMins.toString();
    //$('#textTimeReport' + indexx).val(taketime);
}
function onHrsBlur(args) {
    
    var txtHrs = document.getElementById(args).value;
    if (txtHrs.length == 1) {
        document.getElementById(args).value = "0" + document.getElementById(args).value;
    }
}
function onHrsValid(args) {
    
    var txtHrs = document.getElementById(args).value;
    if (parseInt(txtHrs) >= 24) {
        document.getElementById(args).value = "00";
    }

}
function clearssr() {
    //newssr = [];
    //$(".Newssr").hide()

}
function Remove(arg) {
    
    var ids = arg.id;
    var ssr = newssr
    var key = $.grep(newssr, function (s, m) {
        return s.SSR_unikey != arg;
    })
    newssr = key
    $(".Newssr").hide()
    if (newssr.length > 0) {
        $(".Newssr").show()
        if ($('#divTicket_New_SSR table').length > 0) {
            $('#divTicket_New_SSR').columns("destroy");
        }
        var cnt = 0;
        $('#divTicket_New_SSR').columns({

            data: newssr,
            schema: [
                 //{
                 //    "header": "Select", "key": "Select",//(Tmc == "RIYA" ? "SUPPLIER_PNR" : "S_PNR"),

                 //    "condition": function (val, row) {
                 //        cnt++;
                 //        val = '<input type="checkbox" id="selectbox_' + cnt + '">';
                 //        return val;
                 //    }

                 //},
                 {
                     "header": "Action", "key": "Edit",//(Tmc == "RIYA" ? "SUPPLIER_PNR" : "S_PNR"),

                     "condition": function (val, row) {

                         val = '<button class="showPopup clsmyButton btn-color" id="Toremove_' + cnt + '" onclick="Remove(' + row.SSR_unikey + ')">Cancel</button>';;
                         cnt++;
                         return val;
                     }

                 },
                { "header": "Passenger", "key": "PASSENGER", },
                { "header": "SSR Type", "key": "SSR_SSR_TYPE", },
                { "header": "SSR Name", "key": "SSR_SSR_NAME", },
                { "header": "Amount ", "key": "SSR_SSR_AMOUNT" },
                { "header": "Segment Details", "key": "SSR_SEG_DETAILS" },
                { "header": "Markup", "key": "SSR_MARKUP" },

                { "header": "Ticket No.", "key": "SSR_TICKET_NO" },
                { "header": "Tax Breakup", "key": "SSR_TAX_BREAKUP" },


            ]
        });
    }
}
var GloabSSR = 0;
function Confirmssr() {
    
    var totssramt = 0;
    for (var i = 0; i < newssr.length; i++) {
        if (newssr[i].SSR_TAX_BREAKUP != "") {
            for (var k = 0; k < newssr[i].SSR_TAX_BREAKUP.split("/").length; k++) {
                if (newssr[i].SSR_TAX_BREAKUP.split("/")[k] != "")
                    totssramt += Number(newssr[i].SSR_TAX_BREAKUP.split("/")[k].split(":")[1])
            }
        }
        totssramt += Number(newssr[i].SSR_SSR_AMOUNT) + Number(newssr[i].SSR_MARKUP);
    }
    GloabSSR = totssramt
    $(".SSRamt").html(totssramt)
    var totmt = 0;
    $(".chragecls").each(function (s, m) {
        var ids = this.id;
        if ($("#" + ids).val() != "")
            totmt += Number($("#" + ids).val());
    })
    var text = Number(totmt) + Number(totssramt);
    $(".totsmt").html(text)
    if (text == "0")
        $(".totamtcal").hide()
    else
        $(".totamtcal").show()
}
function LobiboxAlert(Type, message, flag, url) {
    Lobibox.alert(Type, {   //eg: 'error' , 'success', 'info', 'warning'
        msg: message,
        closeOnEsc: false,
        callback: function ($this, type) {

        }
    }
    );


}
function Removesec() {
    
    var lentcnt = $(".segement").length - 1
    var prevcnt = $(".segement").length - 2
    $("#Segment_div_" + lentcnt).remove();
    $("#Addremov_" + prevcnt).show();

    $(".removesec").last().show();
    $(".removesec").first().hide();
}
// To be reshedule Region end


function FormingResSuccessDetails(newspnr) {
    newspnr = newspnr.split("-").length > 1 ? newspnr.split("-")[1] : "";
    var succbldr = "";
    succbldr += ' <div class="col-lg-12 Margcls columns " style="margin-top:15px;">';
    succbldr += '<label>Rescheduled Flight Details</label>';

    succbldr += ' <table class="ui-table no-more-tables" style="width:100%;">';

    succbldr += '<thead class="tablehead">';
    succbldr += '<tr>';
    succbldr += '<th>S PNR</th>';
    succbldr += '<th>Airline PNR.</th>';
    succbldr += '<th>CRS PNR</th>';
    succbldr += '<th>Origin</th>';
    succbldr += '<th>Destination</th>';
    succbldr += '<th>Departure DateTime</th>';
    succbldr += '<th>Arrival DateTime</th>';
    succbldr += '<th>Flight No.</th>';
    succbldr += '</tr>';
    succbldr += '</thead>';

    succbldr += '<tbody>';
    for (resflcnt = 0; resflcnt < ResheduleFlt.length; resflcnt++) {
        succbldr += '<tr>';
        succbldr += '<td>' + newspnr + '</td>';
        succbldr += '<td>' + ($("#Airline_pnr" + resflcnt).val().trim()) + '</td>';
        succbldr += '<td>' + ($("#CRS_PNR" + resflcnt).val().trim()) + '</td>';
        succbldr += '<td>' + ResheduleFlt[resflcnt].origin + '</td>';
        succbldr += '<td>' + ResheduleFlt[resflcnt].destination + '</td>';
        succbldr += '<td>' + ResheduleFlt[resflcnt]["reschduledate"] + '</td>';
        succbldr += '<td>' + ResheduleFlt[resflcnt]["reschduledate"] + '</td>';
        succbldr += '<td>' + ResheduleFlt[resflcnt].reschduleflight + '</td>';
        succbldr += '</tr>';
    }
    succbldr += '</tbody>';
    succbldr += '</table>';
    succbldr += '</div>';

    succbldr += ' <div class="col-lg-12 Margcls columns " style="margin-top:15px;">';
    succbldr += '<label>Rescheduled Passenger Details</label>';
    succbldr += ' <table class="ui-table no-more-tables" style="width:100%;">';

    succbldr += '<thead class="tablehead">';
    succbldr += '<tr>';
    succbldr += '<th>Passenger Type</th>';
    succbldr += '<th>Ticket No.</th>';
    succbldr += '<th>Passenger Name</th>';

    succbldr += '</tr>';
    succbldr += '</thead>';
    succbldr += '<tbody>';
    for (var respcnt = 0; respcnt < Reshpaxdeta.length; respcnt++) {
        succbldr += '<tr>';
        succbldr += '<td> ' + (Reshpaxdeta[respcnt]["passenger_type"] == "ADT" ? "Adult" : Reshpaxdeta[respcnt]["passenger_type"] == "CHD" ? "Child" : Reshpaxdeta[respcnt]["passenger_type"] == "INF" ? "Infant" : "Adult") + '  </td>';
        succbldr += '<td>' + ($("#Ticktno_" + respcnt).val() || (newspnr + " - " + (respcnt + 1))) + '</td>';
        succbldr += '<td>' + Reshpaxdeta[respcnt]["passenger"] + '</td>';
        succbldr += '</tr>';
    }
    succbldr += '</table>';
    succbldr += '</div>';

    succbldr += '<div class="col-sm-12" style="text-align: center;margin-bottom: 10px;">';
    succbldr += '<button type="button" class="btn btn-primary wd200" onclick="reloadpage()">Ok</button>';
    succbldr += '</div>';

    $(".dvclsreshsuccdetails").html(succbldr).show();
    $("#Tobereshid").hide();
}

function reloadpage() {
    location.reload();
}
function rescheduleticketcheck() {
    var length = $(".segement").length
    var check = 0;
    for (var i = 0; i < length; i++) {
        if ($("#reschedule_tckt_check_" + i)[0].checked == true) {
            check++;
        }
    }
    if (length == check) {
        $('#alltcktdetail').prop('checked', true);
    }
    else {
        $('#alltcktdetail').prop('checked', false);
    }
}