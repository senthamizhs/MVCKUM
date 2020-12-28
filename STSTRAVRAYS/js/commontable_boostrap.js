/// <reference path="commontable_boostrap.js" />
function Createtable(result, appendiv, tablename, flg) {

    if (result[0] != "") {
        document.getElementById("charterror").style.display = "Block";
        document.getElementById("Div_Char").style.display = "none";
        document.getElementById("charterror").innerHTML = result[0];
        document.getElementById("charterror").style.color = "RED";
        document.getElementById("charterror").style.fontWeight = "bold";
        document.getElementById("charterror").style.textAlign = "center";

    }

    if (result[1] != "") {
        document.getElementById("Div_Char").style.display = "Block";
        document.getElementById("charterror").style.display = "none";
        var json = result[1];
        var ary = $.parseJSON(json);
        var datafields = new Array();
        var columns = new Array();
        if (ary.length > 0) {
            var s = '';
            $("#" + tablename).html('');
            s += '<table id="' + tablename + '"  class="table table-hover table-striped" data-click-to-select="true" data-show-toggle="true" data-show-columns="false" data-show-filter="true" data-search="true" data-show-refresh="false"  data-show-export="true"  data-pagination="true" style="text-align:left">';
            s += '<thead>';
            for (var i = 0; i < ary.length; i++) {
                if (i == 0) { //Header formation...
                    s += '<tr>';
                    for (var key in ary[0]) {
                        if (!($.isNumeric(ary[0][key] != null ? ary[0][key].replace(',', "") : ary[0][key]))) { //To checck not Number...

                            if ((key.includes("_") == true)) {
                                key = key.replace('_', " ");
                            }
                            s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + key + '</th>';
                        }
                        else if (!($.isNumeric(ary[0][key] != null ? ary[0][key].replace('_', "") : ary[0][key]))) {
                            if ((key.includes("_") == true)) {
                                key = key.replace('_', " ");

                            }
                            s += '<th data-field="direct' + key + '" data-align="right" data-sortable="false">' + key + '</th>';
                        }
                        else {
                            if ((key.includes("_") == true)) {
                                key = key.replace('_', " ");

                            }
                            s += '<th data-field="direct' + key + '" data-align="left" data-sortable="false">' + key + '</th>';
                        }
                    }
                    s += '</tr>';
                    s += '</thead>';
                    s += '<tbody>';
                }

                s += '<tr>';
                for (var key in ary[i]) { //All row formation...


                    ary[i][key] = ary[i][key] != null && ary[i][key] != "" ? ary[i][key] : "";

                    if (key == "SPNR") {

                        if (flg == "abh") {

                            var pas_length = "";
                            var str = ary[i].PASSENGER_NAME.replace(/\s/g, '');

                            var str_length = str.split(",").length;
                            if (str_length > 1) {
                                pas_length = 1;
                            }
                            else {
                                pas_length = 0;
                            }


                            if (ary[i][key].split("|")[1] == "H") {


                                s += '<td class="label label-success spnEarn"> <a href="#"  onclick="javascript:OappsFunction(' + i + ',' + pas_length + ');" ><label id="printSPRValue' + i + '" style="cursor: pointer;color: blue;border-bottom: 1px solid;" class="underline">' + ary[i][key].split("|")[0] + '</label></a></td>';
                            }
                            else {

                                if (ary[i][key].split("|")[1] == "B" || ary[i][key].split("|")[1] == "I" || ary[i][key].split("|")[1] == "W") {

                                    s += '<td  class="label label-success spnEarn"> <a href="#"  onclick="javascript:OappsFunction(' + i + ',' + pas_length + ');"><label id="printSPRValue' + i + '" style="cursor: pointer;color: blue;border-bottom: 1px solid;" class="underline"  title="' + ary[i][key].split("|")[0] + '|' + '">' + ary[i][key].split("|")[0] + '</label></a></td>';

                                }
                                else {

                                    s += '<td  class="label label-success spnEarn"> <a href="#"  onclick="javascript:OTAappsFunction(' + i + ',' + pas_length + ');" ><label id="printSPRValue' + i + '" style="cursor: pointer;color: blue;border-bottom: 1px solid;" class="underline">' + ary[i][key].split("|")[0] + '</label></a></td>';
                                }

                            }

                        }
                        else {
                            s += '<td><span class="mouseover_bkh" style="background-color: #4CAF50;padding: 5px;border-radius: 4px;color: white;">' + ary[i][key] + '</span></a></td>';
                        }
                    }

                    else if (key == "FLIGHT NO") {
                        s += '<td  class="label label-success spnEarn"> <a href="#" id="Val_' + i + '" onclick="OpenNewTab(this)"><label  style="cursor: pointer;color: blue;border-bottom: 1px solid;" class="underline">' + ary[i][key] + '</label></a></td>';
                    }
                    else if (key == "Firstname") {
                        s += '<td  class="label label-success spnEarn"> <a href="#"  onclick="javascript:getdetailall(' + i + ');" ><label id="namefltr_' + i + '" style="cursor: pointer;color: blue;" class="underline" title="' + ary[i][key].split("~")[1] + '">' + ary[i][key].split("~")[0] + '</label></a></td>';
                    }

                    else if (key == "TerminalID") {
                        s += '<td><span class="mouseover_bkh" style="background-color: #FF5722;padding: 5px;border-radius: 4px;color: white;">' + ary[i][key] + '</span></a></td>';
                    }
                    else if (key == "AIRPNR") {
                        s += '<td><span class="mouseover_bkh" style="padding: 5px;border-radius: 4px;color: black;margin-left: -5px;">' + ary[i][key] + '</span></a></td>';
                    }

                    else {
                        s += '<td>' + ary[i][key] + '</a></td>';
                    }
                }
                s += '</tr>';
            }
            s += '</tbody>';
            s += '</table>';

            $('#' + appendiv).html(s);
            $('#' + tablename).bootstrapTable();


        }
    }
}

function Createtable_cancel(result, appendiv, tablename, flg) {

    if (result[0] != "") {
        document.getElementById("can_viewpnrShowDiv").style.display = "Block";
        document.getElementById("Div_Char").style.display = "none";
        document.getElementById("can_viewpnrShowDiv").innerHTML = result[0];
        document.getElementById("can_viewpnrShowDiv").style.color = "RED";
        document.getElementById("can_viewpnrShowDiv").style.fontWeight = "bold";
        document.getElementById("can_viewpnrShowDiv").style.textAlign = "center";
    }
    if (result[1] != "") {
        document.getElementById("can_viewpnrShowDiv").style.display = "Block";
        var pax_json = result[1];
        var paxary = $.parseJSON(pax_json);
        var tckt_json = result[4];
        var tcktary = $.parseJSON(tckt_json);
        sessionStorage.setItem("pax_det", paxary);
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
                    s += '<th style="text-align:center;text-transform: uppercase;"><input type="checkbox" class="chckcnclpaxall" id="checkallpaxdetailcncl" onclick="checkallpaxcancel()"/><span class="clrwhite">ALL</span></th>';
                    for (var key in paxary[0]) {
                        if (key == "PAX_REF_NO") {
                            s += '<th style="display:none;"  data-field="direct' + key + '" data-align="left" data-sortable="false">' + key + '</th>';
                        }
                        else if (key == "SPNR") {
                            var text = $("#hdn_pnrflag").val();
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
                        //s += '<td><input style="display: none;border: 2px solid #3c763d !important;" type="checkbox" id="Chk_fare_' + i + '" runat="server" class="cb" name="Chk_fare_' + i + '" /><label style="border: 2px solid #00afe1 !important;" class="cbx" for="Chk_fare_' + i + '"></label><label class="lbl" for="Chk_fare_' + i + '"></label></td>';
                        s += '<td style="text-align:center;" id="data_' + i + '"><input id="cancel_pax_check_' + i + '" type="checkbox" class="chckcancelpax" onclick="chckcancelpax()" name="cancel_pax_check_' + i + '" /></td>';
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
                        s += '<th style="text-align:center;text-transform: uppercase;"><input type="checkbox" class="chckcncltcktall" id="checkalltcktdetailcncl" onclick="checkalltcktcancel()"/><span>ALL</span></th>';
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
                                if (Number(d3) > Number(d1)) {
                                    // s += '<td><input style="display: none;border: 2px solid #3c763d !important;" type="checkbox" id="Chk_fare_' + i + '" runat="server" class="cb" name="Chk_fare_' + i + '" /><label style="border: 2px solid #00afe1 !important;" class="cbx" for="Chk_fare_' + i + '"></label><label class="lbl" for="Chk_fare_' + i + '"></label></td>';
                                    s += '<td style="text-align:center;" id="data_' + i + '"><input id="cancel_tckt_check_' + i + '" class="chckcanceltckt" onclick="chckcanceltckt()" type="checkbox" name="cancel_tckt_check_' + i + '" /></td>';
                                }
                                else {
                                    //s += '<td><input style="display: none;" type="checkbox" id="Chk_fare_' + i + '" runat="server" class="cb" name="Chk_fare_' + i + '" /><label style="border: 2px solid #00afe1 !important;display:none;" class="cbx" for="Chk_fare_' + i + '"></label><label class="lbl" for="Chk_fare_' + i + '"></label></td>';
                                    s += '<td style="text-align:center;" id="data_' + i + '"><input id="cancel_tckt_check_' + i + '" style="display:none;" type="checkbox" name="cancel_tckt_check_' + i + '" /></td>';
                                }
                            }
                            else {
                                s += '<td style="text-align:center;" id="data_' + i + '"><input id="cancel_tckt_check_' + i + '" style="display:none;" type="checkbox" name="cancel_tckt_check_' + i + '" /></td>';
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
            s += '<div class="col-xs-12 col-sm-3 col-sm-offset-3 martpplus10"><label class="float-left mb-0" style="">Remarks<span style="color: red">*</span></label><textarea id="txt_cancel_request_remarks" style="border: 1px solid #27a5f5; width: 100%;" maxlength="200" class="txt-anim cls_cal_remarks" type="text" autocomplete="off"></textarea></div>';
            s += '<div class="col-xs-12 col-md-2"><input type="button" id="" class="action-button action-buttonO shadow animate color" style="width: 100%;margin-bottom: 20px;margin-top: 30px;" runat="server" value="Cancellation" onclick="cancel_request()"></div>';
            $('#' + appendiv).html('');
            $('#' + appendiv).html(s);
        }
    }
}

function checkalltcktcancel() {
    if ($("#checkalltcktdetailcncl")[0].checked == true) {
        $('.chckcanceltckt').prop('checked', true);
    }
    else {
        $('.chckcanceltckt').prop('checked', false);
    }
}

function checkallpaxcancel() {
    if ($("#checkallpaxdetailcncl")[0].checked == true) {
        $('.chckcancelpax').prop('checked', true);
    }
    else {
        $('.chckcancelpax').prop('checked', false);
    }
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
}