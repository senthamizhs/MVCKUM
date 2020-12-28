
$.xhrPool = [];
$.xhrPool.abortAll = function () {
    $(this).each(function (idx, jqXHR) {
        jqXHR.abort();
    });
    $.xhrPool = [];
};
var platingcarrier = new Array();
var noflight = "";
var flag_cnt = 0;
var LLP = 0;
var splfarecount = 0;
var multiclasarr = [];
var multiclasconarr = [];
var Airlinesname = [];
var refunflg = ""; //Added by saranraj on 20170504...
var flttyp = ""; //Added by saranraj on 20170504...
var arysplitacat = [];
var allAIRLINECode = "";
var allAIRLINENAME = "";
var strBuildairCheck = "";
/*********************connecting Flight Varibale Declartion********************************/
var con_url = ""; var con_Fno = ""; var con_org = ""; var con_Des = ""; var con_Dep = ""; var con_Arr = ""; var con_Dur = ""; var con_Durmin = ""; var con_min = 0; var con_cls = ""; var con_BFare = ""; var con_GFare = ""; var con_Comm = "";
var con_aircat = ""; var con_inva = ""; var con_ref; var con_seg = ""; var con_fly = ""; var First_inva = ""; var First_inva1 = ""; var con_Dep_fullcity = ""; var con_Arr_fullcity = "";
var arr_dateSceond = ""; var arr_dateSceondvalidate = ""; var fare = ""; var cabin = ""; var othrbnft = ""; var arrDatetime = ""; var depDatetime = ""; var airprtwat_con_Dur = ""; var airprtwat_con_Durmin = ""; var airprtwat_con_min = 0;
/*********************connecting Flight Varibale Declartion********************************/
var ACode = ""; var Org = ""; var Des = "";
var Dep = ""; var Arr = ""; var via = "";
var SDep = ""; var SArr = "";
var STax_build = "";
var refund = new Array();
var bestbuy = new Array();
var SAvail_build = "";
var strFlightBuild = "";
var strsubFlightBuild = "";
var subFlightBuild = "";
var StartDepttime = new Array();
var EndArrivetime = new Array();
var EndtoolArr = new Array();
var NoofStops = new Array();
var totNoofStops = 0;
var PrevDes = "";
var RDep = "";
var Riti = "0";

var ariwatarrv = "";
var ariwatdepv = "";


var dataproperty_array = [];//Added by srinath on 20171006 to improve peroformance
var extrahidem_dataproperty_array = [];//Added by srinath on 20171006 to improve peroformance

var deparrv_arrv = "";

var seatconu = ""; //added by sri availability desing changes
var basicfare = ""; //added by sri availability desing changes
var refund_grp = ""; //added by sri availability desing changes
var farerule_grp = ""; //added by sri availability desing changes
var getdetails = ""; //added by sri availability desing changes
var baggagedetails_grp = ""; //added by sri availability desing changes
var multicls_grp = ""; //added by sri availability desing changes
var returncnt = "";
var connectingflight = "";
//var roundtripdisnone = "";
var nextarraival = "";
var commissionbuild = "";

var triptype_grp = ""; //added by sri availability desing changes
var triptype_grp_disp = ""; //added by sri availability desing changes
var col_lg_2or3_new = "";
var col_lg_1or2_new = "";
var col_lg_2or4_new = "";
var aligntext = "";

var appendavail_side = "";

function LoadFlights(arg, obj1) {
    refunflg = "";
    flttyp = "";
    strBuildairCheck = "";
    deparrv_arrv = "";
    arysplitacat = [];
    triptype_grp = "";
    triptype_grp_disp = "";
    appendavail_side = "";

    try {
        if (arg != "" && arg != null) {
            var singletripifflg = "Sgrd";
            var doubletripifflg = "Dgrd";
            var segLen = 0;
            var conLen = 0; //Added by saranraj on 20170523 to calculate connecting flight length...
            var fltcnt = 0; //Added by saranraj on 20170601 to calculate flight Count...
            var stopscnt = 0; //Added by saranraj on 20170607 to calculate stops count for Roundtrip Spl and International Multicity...
            col_lg_2or3_new = ""; ///added 
            col_lg_1or2_new = "";
            col_lg_2or4_new = "";
            returncnt = "";
            var paddingrigt = "";


            var MultiFlt = "";
            var Sameflight = "Same";
            var sameflighturl = new Array();
            ACode = ""; Org = ""; Des = "";
            Dep = ""; Arr = ""; via = "";
            SDep = ""; SArr = "";
            /*********************connecting Flight Varibale Declartion********************************/
            con_Fno = ""; con_Dep = ""; con_Dur = ""; con_Durmin = ""; con_min = 0; con_cls = ""; con_BFare = ""; con_GFare = ""; con_Comm = ""; var con_org = ""; var con_Des = ""; var con_Arr = "";
            con_aircat = ""; con_ref; con_seg = ""; con_fly = ""; First_inva = ""; First_inva1 = ""; con_Dep_fullcity = ""; con_Arr_fullcity = "";
            arr_dateSceond = ""; arr_dateSceondvalidate = ""; fare = ""; cabin = ""; arrDatetime = ""; depDatetime = ""; airprtwat_con_Dur = ""; airprtwat_con_Durmin = ""; airprtwat_con_min = 0;// othrbnft = "";
            /*********************connecting Flight Varibale Declartion********************************/
            STax_build = "";
            refund = new Array();
            bestbuy = new Array();
            SAvail_build = "";
            connectingflight = "";

         

            strFlightBuild = "";
            strsubFlightBuild = "";
            subFlightBuild = "";
            StartDepttime = new Array();
            EndArrivetime = new Array();
            EndtoolArr = new Array();
            NoofStops = new Array();
            totNoofStops = 0;
            PrevDes = "";
            RDep = "";
            Riti = "0";
            //var JsonObj = JSON.parse(arg[4].replace("[object Object]", ""));
            ariwatarrv = "";
            ariwatdepv = "";

            //var JsonObj = arg;
            var setofgroupedflight = arg;

            document.getElementById("AvaiText").value += setofgroupedflight[0][0].availbilitysession_1_arg + "~";

            //Added by Saranraj on 20170516 for dynamic class for oneway and roundtrip...
            var col_xs_2or4 = "", col_md_4or6 = "", col_md_6or8 = "", col5or0 = "", col_xs_4or_2 = "", col_xs_8or_10 = "";
            if ($('#hdtxt_trip').val() == "O" || $('#hdtxt_trip').val() == "Y" || ($('#hdtxt_trip').val() == "M") && $('body').data('segtype') == "I") { //Oneway or Roundtrip Special...
                col_xs_2or4 = "col-xs-4";
                col_md_4or6 = "col-md-4"; //col-md-6
                col_md_6or8 = "col-md-8"; //col-md-6
                col5or0 = "col5";
                col_lg_2or3_new = "col-lg-2";  //added sri
                aligntext = "center";
                col_lg_1or2_new = "col-lg-1";
                col_lg_2or4_new = "col-lg-2";
                paddingrigt = "unset";
                col_xs_4or_2 = "col-xs-2";
                col_xs_8or_10 = "col-xs-10";
            }
            else {
                col_xs_2or4 = "col-xs-2";
                col_md_4or6 = "col-md-4";
                col_md_6or8 = "col-md-8";
                col5or0 = "col0";
                col_lg_2or3_new = "col-lg-3"  //added sri
                aligntext = "left";
                col_lg_1or2_new = "col-lg-2";
                col_lg_2or4_new = "col-lg-4";
                paddingrigt = "0px !important";
                col_xs_4or_2 = "col-xs-4";
                col_xs_8or_10 = "col-xs-8";
                //roundtripdisnone = "dispnonecls";
            }
            if ($('#hdtxt_trip').val() == "Y") {
                triptype_grp = "col-lg-5 col-xs-5";
                triptype_grp_disp = "block";
            }
            else {
                triptype_grp = "col-lg-12";
                triptype_grp_disp = "none";
            }
            //End...
            var destinationcity = "";
            var JSONdata = [];
            var availairname = "";
            var mind = "";
            var minutes = "";
            var flymind = "";
            var flyminutes = "";
            var flysecd = "";
            var flysecond = "";
            var secd = "";
            var seconds = "";
            var iti = "";
            var SegmentDetails = "";
            var Deps = "", toolDep = "";
            var toolArr = "";
            var Arrs = "";
            var Deps = "";
            var StoolDep = "";
            var StoolArr = "";
            var Arrs = "";
            var lessincentivefare = "";
            var lessincentivefarewitgros = "";
            var flightname = "";
            var connflightpop = "";
            var splitFlightnumber = "";

            var splitairlinename = "";

            var viaflight1 = ""; var otherbenifit = ""; var viaflight2 = ""; var hdndestinat = ""; var avildest = ""; var adt = ""; var cdt = ""; var Seats = ""; var Refu = "";
            var JsonTax = ""; var crntBagg = ""; var originAirport = ""; var desinationAirport = ""; var viaflight3 = ""; var viaflight4 = ""; var arrivtimewithnextdt = "";
            var resultvalue = "";
            var uniqueBagg = [];

            var waitmin = "";
            var Flytimebuild = "";
            var Flytimebuilddashedlin = "";
            var orgincount = "";

            var JsonObj = "";
            for (var sr = 0; sr < setofgroupedflight.length; sr++) {
                var groupbind = "";
                JsonObj = setofgroupedflight[sr];
                for (var jr = 0; jr < JsonObj.length; jr++) {
                    groupbind += JsonObj[jr]["Dep"].split(' ')[3].replace(':', '') + JsonObj[jr]["Arr"].split(' ')[3].replace(':', '') + JsonObj[jr]["Fno"].replace(' ', '');
                }

                for (var len = 0; len < JsonObj.length; len++) {
                JSONdata = [];
                //var pencen = 0;

                seatconu = ""; //added by srainth availability dsing
                basicfare = ""; //added by srainth availability dsing
                refund_grp = ""; //added by srainth availability dsing
                farerule_grp = ""; //added by sri availability desing changes
                getdetails = ""; //added by sri availability desing changes
                baggagedetails_grp = ""; //added by sri availability desing changes
                multicls_grp = ""; //added by sri availability desing changes
                nextarraival = "";
                commissionbuild = "";
                
                

                availairname = "";
                mind = "";
                minutes = "";
                flymind = "";
                flyminutes = "";
                flysecd = "";
                flysecond = "";
                secd = "";
                seconds = "";
                iti = "";
                SegmentDetails = "";
                Deps = "";
                toolDep = "";
                toolArr = "";
                Arrs = "";
                Deps = "";
                StoolDep = "";
                StoolArr = "";
                Arrs = "";
                flightname = "";
                splitFlightnumber = "";

                splitairlinename = "";

                viaflight1 = "";
                adt = ""; cdt = ""; Seats = "";
                JSONdata = JsonObj[len];
                totNoofStops = 0;

                airprtwat_con_min = parseInt(JsonObj[0].Dur) - parseInt(JsonObj[0].fly);
                //alert(airprtwat_con_min);
                appendavail_side = JSONdata["bindside_6_arg"];
                /* region added by udhaya for the purpose of airline filtration*/
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
                /* region added by udhaya for the purpose of airline filtration*/
                refunflg = JSONdata["Refund"] == "TRUE" ? "refun" : (JSONdata["Refund"] == "FALSE" ? "nonrefun" : "");
                arysplitacat = JSONdata["acat"].split("SpLitPResna");
                mind = JSONdata["Dur"] % (60 * 60);
                minutes = Math.floor(mind / 60);
                flymind = JSONdata["fly"] % (60 * 60);
                flyminutes = Math.floor(flymind / 60);
                flysecd = flymind % 60;
                flysecond = Math.ceil(flysecd);
                if (flysecond.value < 10) {
                    flysecond = "0" + flysecond;
                }
                secd = mind % 60;
                seconds = Math.ceil(secd);
                if (seconds.value < 10) {
                    seconds = "0" + seconds;
                }
                iti = JSONdata["iti"]
                //var nextValidateDep = "", nextValidatearr = "";
                sameflighturl.push(JSONdata["Conurl"]);
                SegmentDetails = JSONdata["seg"].split(':');
                if (segLen == 0) {
                    Org = JSONdata["Org"];
                    destinationcity = "";
                    ariwatdepv = JSONdata["Dep"]; //write by srinath for airport waiting show
                    Deps = JSONdata["Dep"].split(' ');
                    fare = JSONdata["Incentive"];
                    depDatetime = JSONdata["Dep"];
                    toolDep = JSONdata["Dep"];
                    toolDep = toolDep.substring(0, toolDep.length - 5);
                    arrDatetime = JSONdata["Arr"];
                    toolArr = JSONdata["Arr"];
                    toolArr = toolArr.substring(0, toolArr.length - 5);
                    Dep = Deps[3];
                    Arrs = JSONdata["Arr"].split(' ');
                    Arr = Arrs[3];
                    con_url = JSONdata["ur"];
                    con_Fno = JSONdata["Fno"];
                    con_org = JSONdata["Org"];//nonstop
                    con_Des = JSONdata["Des"];
                    var via_fl = ((JSONdata["nonstop"] != "" && JSONdata["nonstop"] != null) ? JSONdata["nonstop"] : "N/A");
                    nonstop = JSONdata["nonstop"];
                    con_Dep_fullcity = JSONdata["originfull"];
                    con_Dep = Dep;
                    con_Arr = Arr;
                    cabin = JSONdata["Cab"];
                    con_cls = JSONdata["Cls"];
                    con_BFare = JSONdata["BFare"];
                    con_GFare = JSONdata["GFare"];
                    con_WTMFare = JSONdata["WTMFare"];
                    con_Comm = JSONdata["com"];
                    con_aircat = JSONdata["acat"];
                    con_inva = JSONdata["Inva"];
                    con_ref = JSONdata["Ref"];
                    //othrbnft = JSONdata["othrbnft"];
                    con_seg = JSONdata["seg"].split('\n');
                    con_fly = flyminutes + "h :" + flysecond + " m";
                    var nextValidateDep = new Date(toolDep);
                    var nextValidatearr = new Date(toolArr);
                    _ArrayFare.push(JSONdata["GFare"]);
                }
                if (segLen >= 0) {
                    Deps = JSONdata["Dep"].split(' ');
                    StoolDep = JSONdata["Dep"];
                    StoolDep = toolDep.substring(0, toolDep.length - 5);
                    StoolArr = JSONdata["Arr"];
                    StoolArr = toolArr.substring(0, toolArr.length - 5);
                    SDep = Deps[3];
                    Arrs = JSONdata["Arr"].split(' ');
                    SArr = Arrs[3];
                    StartDepttime.push(SDep);
                    con_ref += JSONdata["Ref"] + "/"
                }
                lessincentivefare = "";
                lessincentivefarewitgros = "";
                if (fare != "0" && fare != "" && fare != null) {
                    lessincentivefare = parseInt(con_WTMFare) - parseInt(fare);
                    lessincentivefarewitgros = parseInt(con_GFare) - parseInt(fare);
                }

                flightname = con_Fno.split(' ')[0];
                if (Airlinesname.indexOf(flightname) < 0) {
                    Airlinesname.push(flightname);
                }
                //Connecting flights condtions start
                if (JSONdata["Con"] == "Y") {
                    if (destinationcity == "" && JSONdata["iti"] == "1") {
                        destinationcity = JSONdata["Org"];
                        // con_Dep_fullcity = JSONdata["originfull"];
                        // con_Arr_fullcity = JSONdata["destfull"];
                        con_Arr_fullcity = JSONdata["originfull"];
                        if (con_Arr_fullcity == "") {
                            con_Arr_fullcity = JSONdata["originfull"];
                        }
                    }
                    else {
                        // if (con_Arr_fullcity == "") {
                        con_Arr_fullcity = JSONdata["destfull"];
                        //}
                    }
                    if (con_Dep_fullcity == "") {
                        con_Dep_fullcity = JSONdata["originfull"];
                    }
                    segLen++;
                    conLen++;
                    con_min += parseInt(JSONdata["fly"]);
                    connflightpop = "";
                    splitFlightnumber = JSONdata["Fno"].split(' ');

                    splitairlinename = airlinename(splitFlightnumber[0]);


                    viaflight1 = ((JSONdata["nonstop"] != "" && JSONdata["nonstop"] != null) ? JSONdata["nonstop"] : "N/A");
                    //Connecting flights set
                    MultiFlt += "<article class='box  commonbox'  style='padding: 0px;'>";
                    //con set #1
                    MultiFlt += "<div class='row row5 row-mob-0'>";
                    //load airline  #001
                    MultiFlt += " <div class='col-xs-12 col-lg-2 col-md-2 col-sm-2 col5 l-hight l-mob-hight mob-conn-flt' style='text-align: center;z-index: 1;'>";
                    MultiFlt += " <p class='con-flt-img loadfightinfo' id='fliimage" + Ttotlen + "' data-popup='" + LLP + "SltP" + splitFlightnumber[0] + "SltP" + JSONdata["originfull"] + "SltP" + JSONdata["destfull"] + "SltP" + JSONdata["plt"] + "SltP" + (con_seg[2].split(':')[1].replace("\r", "").trim() == "" ? "N/A" : con_seg[2].split(':')[1].replace("\r", "")) + "SltP" + (con_seg[3].split(':')[1].replace("\r", "").trim() == "" ? "N/A" : con_seg[3].split(':')[1].replace("\r", "")) + "SltP" + (uniqueBagg.length == 1 ? uniqueBagg[0] : JSONdata["Bagg"]) + "SltP" + JSONdata["multiclassKey"] + "SltP" + JSONdata["StkType"] + "SltP" + viaflight4 + "SltPconflightdet_' onmouseover='showflightpoup(this)'>";
                    //MultiFlt += "<img  alt='' class='FlightTip FlightTipimg' style='width: 28px; height: 28px;' id='conflightdet_" + LLP + "' src=\"" + JSONdata["Conurl"] + "\" rel=\"" + connflightpop + "\">";
                    MultiFlt += "<img  alt='' class='FlightTip FlightTipimg' style='width: 28px; height: 28px;' id='conflightdet_" + LLP + "' src=\"" + JSONdata["Conurl"] + "\" rel=\"\">";//" + connflightpop + "

                    MultiFlt += "<span style='white-space: nowrap;'>" + JSONdata["Fno"] + "</span>";
                    MultiFlt += "</p>";

                    //returncnt1 += " <p class='con-flt-img loadfightinfo' id='fliimage" + Ttotlen + "' data-popup='" + LLP + "SltP" + splitFlightnumber[0] + "SltP" + JSONdata["originfull"] + "SltP" + JSONdata["destfull"] + "SltP" + JSONdata["plt"] + "SltP" + (con_seg[2].split(':')[1].replace("\r", "").trim() == "" ? "N/A" : con_seg[2].split(':')[1].replace("\r", "")) + "SltP" + (con_seg[3].split(':')[1].replace("\r", "").trim() == "" ? "N/A" : con_seg[3].split(':')[1].replace("\r", "")) + "SltP" + (uniqueBagg.length == 1 ? uniqueBagg[0] : JSONdata["Bagg"]) + "SltP" + JSONdata["multiclassKey"] + "SltP" + JSONdata["StkType"] + "SltP" + viaflight4 + "SltPconflightdet_' onmouseover='showflightpoup(this)'>";
                    ////MultiFlt += "<img  alt='' class='FlightTip FlightTipimg' style='width: 28px; height: 28px;' id='conflightdet_" + LLP + "' src=\"" + JSONdata["Conurl"] + "\" rel=\"" + connflightpop + "\">";
                    //returncnt1 += "<img  alt='' class='FlightTip FlightTipimg' style='width: 28px; height: 28px;' id='conflightdet_" + LLP + "' src=\"" + JSONdata["Conurl"] + "\" rel=\"\">";//" + connflightpop + "

                    //returncnt1 += "<span style='white-space: nowrap;'>" + JSONdata["Fno"] + "</span>";
                    //returncnt1 += "</p>";


                    //load airline 
                    //load class
                    if (JSONdata["multiclass"] == "1") {
                        MultiFlt += "<span class='mob-conn-mclass'>"
                        MultiFlt += "<select id='concls" + LLP + "' style='line-height: 16px; height: 30px; border-radius: 6px; font-size: 11px; border: 1px dashed #ddd; background-color: #fff;' class='MUlticlasselect' onchange='Changemulticlasforconn(" + flag_cnt + "," + Ttotlen + "," + LLP + "," + JSONdata["multiclass"] + ")'>"
                        MultiFlt += "<option value=" + JSONdata["Cls"] + ">" + JSONdata["Cls"] + "</option>";
                        var Multi_clas = JSONdata["Cab"].split(',');
                        if ($('#hdnsAllowmulticlass').val() == "Y") {
                            for (var i = 0; i < Multi_clas.length; i++) {
                                if (Multi_clas[i] != "" && Multi_clas[i] != null) {
                                    MultiFlt += "  <option value=" + Multi_clas[i] + ">" + Multi_clas[i] + "</option>";
                                }
                            }
                        }
                        MultiFlt += "</select>"
                        MultiFlt += "</span>"
                    }
                    else if (JSONdata["multiclass"] == "2") {
                        tripflag = 0;
                        MultiFlt += "<span class='mob-conn-mclass'>"
                        MultiFlt += "<select id='clsduplicate_" + LLP + "' style='' class='MUlticlasselect bfor multiclassddl' onclick='loadClassFunction(" + LLP + "," + segLen + "," + flag_cnt + "," + Ttotlen + "," + tripflag + ")'>"
                        MultiFlt += "<option value=" + JSONdata["Cls"] + ">" + JSONdata["Cls"] + "</option>";
                        MultiFlt += "</select>"
                        MultiFlt += "<select id='concls" + LLP + "' style='display:none;' class='MUlticlasselect aftr multiclassddl' onchange='Changemulticlasforconn(" + flag_cnt + "," + Ttotlen + "," + LLP + "," + JSONdata["multiclass"] + ")'>"
                        MultiFlt += "<option value=" + JSONdata["Cls"] + ">" + JSONdata["Cls"] + "</option>";
                        MultiFlt += "</select>"
                        MultiFlt += "</span>"
                    }
                    else {
                        MultiFlt += "<span align='center' class='mob-conn-mclass' ><a id='concls" + len + "'>" + JSONdata["Cls"] + "</a></span>";
                    }
                    MultiFlt += "</div>"
                    //load Class
                    //load airline  #002
                    MultiFlt += "<div class='col-xs-12 col-sm-10 col-md-10 col-lg-10 col5' style='margin-top: 0px;z-index: 1;'>";
                    MultiFlt += "<div class='row row5'>"
                    //load airline  #002.1
                    MultiFlt += "<div class='col-md-12 col-lg-12 col-sm-12 col5'>"
                    MultiFlt += "<div class='row row5'>"
                    MultiFlt += "<div class='col-md-10 col5' style='border-right: 1px solid #f5f5f5;'>"
                    MultiFlt += "<div class='take-off col-sm-12 col5'>"
                    MultiFlt += "<div style='padding-left: 0px;'>"
                    MultiFlt += "<div class='wh33percent left fontsize11 bold dark'>" + JSONdata["Org"] + ""
                    orgincount = JSONdata["Org"];
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='wh33percent left center fontsize11 bold dark Durationcls'>Duration"
                    MultiFlt += "</div>"

                    // var time = "";

                  
                    // if (len > 0) {

                    //     time = airportwaitingcalc(JsonObj[len - 1].Arr, JsonObj[len].Dep);
                    //     MultiFlt += "<div class='hidden-xs wh33percent right textright fontsize11 bold dark' style='color:#fa7b23'>loyout Time=" + time + ""
                    //     MultiFlt += "</div>"
                    //}

                    MultiFlt += "<div class='wh33percent right textright fontsize11 bold dark'>" + JSONdata["Des"] + ""
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='clearfix'></div>"
                    MultiFlt += "<div class='wh33percent left'>"
                    MultiFlt += "<div class='fcircle'>"
                    MultiFlt += "<div class='icon' style='padding: 0px 2px 0 2px; transform: rotate(-35deg);'><i class='soap-icon-plane-right yellow-color trnsformrotatedep'></i></div>"
                    MultiFlt += "</div>"
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='wh33percent left'>"
                    MultiFlt += "<div class='fcircle center'>"
                    MultiFlt += "<div class='icon' style='padding: 0px 5px 0px 0px;'><i class='soap-icon-clock yellow-color timerpadding'></i></div>"
                    MultiFlt += "</div>"
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='wh33percent right'>"
                    MultiFlt += "<div class='fcircle right'>"
                    MultiFlt += "<div class='icon' style='padding: 0px 0 0 2px; transform: rotate(30deg);'><i class='soap-icon-plane-right yellow-color trnsformrotatearr'></i></div>"
                    MultiFlt += "</div>"
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='clearfix'></div>"
                    MultiFlt += "<div class='fline2px'></div>"
                    MultiFlt += "<div class='wh33percent left fontsize11 bold'>" + JSONdata["Dep"] + ""
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='wh33percent left center fontsize11'>" + flyminutes + "h :" + flysecond + "m"
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='wh33percent right textright fontsize11 bold'>" + JSONdata["Arr"] + ""
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='clearfix'></div>"
                    MultiFlt += "</div>"
                    MultiFlt += "</div>"
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='col-md-2 col5'>"
                    MultiFlt += "<div class='amenities mob-conn-other conbaggageclas_" + Ttotlen + "' id='conbagg_" + len + "'>"
                    MultiFlt += "<span class='vcenter mob-conn-bagg' data-toggle='tooltip' data-placement='top' title='Baggage'><i class='soap-icon-suitcase' style='font-size: 20px;'></i><span class='baggfontcolr'>" + SegmentDetails[5].replace("\r\n", "") + "</span></span>";
                    MultiFlt += " <img src='../Images/info.gif' class='FlightTip' style='width: 15px; display: none;' id='extradetails_" + Ttotlen + "' rel=\"" + otherbenifit + "\"/></div>"
                    // }

                    MultiFlt += "</div>"
                    MultiFlt += "</div>"
                    MultiFlt += "</div>"
                    //load airline  #002.1
                    MultiFlt += "</div>"
                    MultiFlt += "</div>";
                    //load airline  #002
                    //Connecting flights set
                    MultiFlt += "</div>"
                    //con set #1
                    MultiFlt += "</article>";
                    if ($('#hdtxt_trip').val() == "O" || $('#hdtxt_trip').val() == "Y" || ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "I")) {
                        $("#CheckStops").css("display", "block");
                        $("#StopsDet").css("display", "block");
                        via += JSONdata["Org"] + "<i class='fa fa-long-arrow-right'></i>";  //<img  class='Connect' src='Images/ConnecArrow.png' />
                    }
                    else if ($('#hdtxt_trip').val() == "R") {
                        $("#CheckStops").css("display", "block");
                        $("#StopsDet").css("display", "block");
                        var StDept = StartDepttime.length > 0 ? StartDepttime[0] : Dep;
                        /*****/
                        /**FOR RUYA STK**/
                        hdndestinat = $('#hdtxt_destination').val();
                        avildest = JSONdata["Des"];
                        if (JSONdata["Inva"].split("SpLitPResna")[23] == "RST") {
                            if (JsonObj[len + 1]["iti"] == Riti)
                            { via += JSONdata["Org"] + "<i class='fa fa-long-arrow-right'></i>"; }   //<img  class='Connect' src='Images/ConnecArrow.png' />
                            else
                            {
                                RDep = segLen;
                                via += JSONdata["Org"] + "<i class='fa fa-long-arrow-right'></i>" + JSONdata["Des"] + " &nbsp; <br />";  //<img  class='Connect' src='Images/ConnecArrow.png' />
                                via += "<p class='lbldeaprtrnd' > &nbsp; <B>" + StDept + "</B> &nbsp; &nbsp; <B> " + SArr + "</B></p>"
                            }
                            $("#slider-time").css("display", "none");
                            Riti = JsonObj[len + 1]["iti"];
                        }
                        else {
                            if (JsonObj[len + 1]["iti"] == Riti)
                            { via += JSONdata["Org"] + "<i class='fa fa-long-arrow-right'></i>"; }  //<img  class='Connect' src='Images/ConnecArrow.png' />
                            else
                            {
                                RDep = segLen;
                                via += JSONdata["Org"] + "<i class='fa fa-long-arrow-right'></i>" + JSONdata["Des"] + " &nbsp; <br />";  //<img  class='Connect' src='Images/ConnecArrow.png' />
                                via += "<p class='lbldeaprtrnd' > &nbsp; <B>" + StDept + "</B> &nbsp; &nbsp; <B> " + SArr + "</B></p>"
                            }
                            $("#slider-time").css("display", "none");
                            Riti = JsonObj[len + 1]["iti"];
                        }
                    }
                    else if ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "D") {

                        if (PrevDes == JSONdata["Org"]) {
                            via += JSONdata["Des"] + "<i class='fa fa-long-arrow-right'></i>";  //<img  class='Connect' src='Images/ConnecArrow.png' />

                        }
                        else if (PrevDes == "") {
                            via += JSONdata["Org"] + "<i class='fa fa-long-arrow-right'></i>" + JSONdata["Des"] + " <i class='fa fa-long-arrow-right'></i>";  //<img  class='Connect' src='Images/ConnecArrow.png' />
                            PrevDes = JSONdata["Des"];
                        }

                        else { via += JSONdata["Org"] + "<i class='fa fa-long-arrow-right'></i>" + JSONdata["Des"] + " | "; }  //<img  class='Connect' src='Images/ConnecArrow.png' />
                        PrevDes = JSONdata["Des"];
                    }

                    First_inva += JSONdata["Inva"] + "SpLITSaTIS"
                    //First_inva1 += JSONdata["Inva1"] + "SpLITSaTIS"
                    NoofStops.push(JSONdata["Stops"]);
                    refund.push(JSONdata["Refund"]);
                    bestbuy.push(JSONdata["bestbuy"]);
                    LLP++;

                   
                    continue;

                }
                else {

                    con_min += parseInt(JSONdata["fly"]);

                    conLen = 0;
                    if (destinationcity == "" && JSONdata["iti"] == "1") {
                        destinationcity = JSONdata["Org"];
                        // con_Dep_fullcity = JSONdata["destfull"];
                        //con_Dep_fullcity = JSONdata["originfull"];
                        con_Arr_fullcity = JSONdata["originfull"];
                        if (con_Arr_fullcity == "") {
                            con_Arr_fullcity = JSONdata["originfull"];
                        }
                    }
                    else {
                        // if (con_Arr_fullcity == "") {
                        con_Arr_fullcity = JSONdata["destfull"];
                        // }
                    }
                    if (con_Dep_fullcity == "") {
                        con_Dep_fullcity = JSONdata["originfull"];
                    }
                }

                //Connecting flights condtions end
                if ($('#hdtxt_trip').val() != "O" && $('#hdtxt_trip').val() != "Y" && !($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "I")) {
                    var StDept = StartDepttime.length > 0 ? (RDep != "" ? StartDepttime[RDep] : StartDepttime[0]) : SDep;
                    if ($('#hdtxt_trip').val() == "M") {
                        StDept = StartDepttime.length > 0 ? StartDepttime[0] : SDep;
                        if (PrevDes == JSONdata["Org"]) {
                            via += JSONdata["Des"] + "  "

                        }
                        else { via += JSONdata["Org"] + "<i class='fa fa-long-arrow-right'></i>" + JSONdata["Des"] + " &nbsp; <br>" }
                        PrevDes = JSONdata["Des"];
                    }
                    else {

                        via += JSONdata["Org"] + "<i class='fa fa-long-arrow-right'></i>" + JSONdata["Des"] + " &nbsp; <br>"

                    }
                    via += "<p style='display:none;' class='lbldeaprtrnd' >&nbsp; <B> " + StDept + "</B>&nbsp; &nbsp; <B> " + SArr + "</B></p>";
                }
                else {
                    via += JSONdata["Org"] + "<i class='fa fa-long-arrow-right'></i>" + JSONdata["Des"];
                }

                if (segLen == 0) {
                    via = "";
                }
                else {

                    connflightpop = "";
                    viaflight2 = ((JSONdata["nonstop"] != "" && JSONdata["nonstop"] != null) ? JSONdata["nonstop"] : "N/A");
                    splitFlightnumber = JSONdata["Fno"].split(' ');
                    splitairlinename = airlinename(splitFlightnumber[0]);

                    MultiFlt += "<article class='box  commonbox' style='padding: 0px;'>";
                    //con set #1
                    MultiFlt += "<div class='row row5 row-mob-0'>"//8
                    //load airline  #001
                    MultiFlt += " <div class='col-xs-12 col-lg-2 col-md-2 col-sm-2 col5 l-hight l-mob-hight mob-conn-flt' style='text-align: center;z-index: 1;'>"
                    MultiFlt += " <p class='con-flt-img loadfightinfo' id='fliimage" + Ttotlen + "' data-popup='" + LLP + "SltP" + splitFlightnumber[0] + "SltP" + JSONdata["originfull"] + "SltP" + JSONdata["destfull"] + "SltP" + JSONdata["plt"] + "SltP" + (con_seg[2].split(':')[1].replace("\r", "").trim() == "" ? "N/A" : con_seg[2].split(':')[1].replace("\r", "")) + "SltP" + (con_seg[3].split(':')[1].replace("\r", "").trim() == "" ? "N/A" : con_seg[3].split(':')[1].replace("\r", "")) + "SltP" + (uniqueBagg.length == 1 ? uniqueBagg[0] : JSONdata["Bagg"]) + "SltP" + JSONdata["multiclassKey"] + "SltP" + JSONdata["StkType"] + "SltP" + viaflight2 + "SltPconflightdet_' onmouseover='" + ($("#conflightdet_" + LLP).prop('rel') == "" ? '' : 'showflightpoup(this)') + "'>"
                    //MultiFlt += "<img  alt='' class='FlightTip FlightTipimg' style='width: 28px; height: 28px;' id='conflightdet_" + LLP + "' src=\"" + JSONdata["Conurl"] + "\" rel=\"" + connflightpop + "\">"
                    MultiFlt += "<img  alt='' class='FlightTip FlightTipimg' style='width: 28px; height: 28px;' id='conflightdet_" + LLP + "' src=\"" + JSONdata["Conurl"] + "\"  rel=\"\">"//" + connflightpop + "

                    MultiFlt += "  <span style='white-space: nowrap;'>" + JSONdata["Fno"] + "</span>"
                    MultiFlt += "  </p>"
                    //load class
                    if (JSONdata["multiclass"] == "1") {
                        MultiFlt += "<span class='mob-conn-mclass'>"
                        MultiFlt += "<select id='concls" + LLP + "' style='line-height: 16px; height: 30px; border-radius: 6px; font-size: 11px; border: 1px dashed #ddd; background-color: #fff;' class='MUlticlasselect' onchange='Changemulticlasforconn(" + flag_cnt + "," + Ttotlen + "," + LLP + "," + JSONdata["multiclass"] + ")'>"
                        MultiFlt += "<option value=" + JSONdata["Cls"] + ">" + JSONdata["Cls"] + "</option>";
                        if (JSONdata["multiclass"] == "1") {
                            var Multi_clas = JSONdata["Cab"].split(',');
                            if ($('#hdnsAllowmulticlass').val() == "Y") {
                                for (var i = 0; i < Multi_clas.length; i++) {
                                    if (Multi_clas[i] != "" && Multi_clas[i] != null) {
                                        MultiFlt += "  <option value=" + Multi_clas[i] + ">" + Multi_clas[i] + "</option>";
                                    }
                                }
                            }
                        }
                        MultiFlt += "</select>"
                        MultiFlt += "</span>"
                    }
                    else if (JSONdata["multiclass"] == "2") {
                        tripflag = 0;
                        MultiFlt += "<span class='mob-conn-mclass'>"
                        MultiFlt += "<select id='clsduplicate_" + LLP + "' style='line-height: 16px; height: 30px; border-radius: 6px; font-size: 11px; border: 1px dashed #ddd; background-color: #fff;' class='MUlticlasselect bfor' onclick='loadClassFunction(" + LLP + "," + segLen + "," + flag_cnt + "," + Ttotlen + "," + tripflag + ")'>"
                        MultiFlt += "<option value=" + JSONdata["Cls"] + ">" + JSONdata["Cls"] + "</option>";

                        MultiFlt += "</select>"

                        MultiFlt += "<select id='concls" + LLP + "' style='display:none; line-height: 16px; height: 30px; border-radius: 6px; font-size: 11px; border: 1px dashed #ddd; background-color: #fff;' class='MUlticlasselect aftr' onchange='Changemulticlasforconn(" + flag_cnt + "," + Ttotlen + "," + LLP + "," + JSONdata["multiclass"] + ")'>"
                        MultiFlt += "<option value=" + JSONdata["Cls"] + ">" + JSONdata["Cls"] + "</option>";

                        MultiFlt += "</select>"
                        MultiFlt += "</span>"

                    }
                    else {
                        MultiFlt += "<span align='center' class='mob-conn-mclass' ><a id='concls" + len + "'>" + JSONdata["Cls"] + "</a></span>";
                    }
                    MultiFlt += "</div>"
                    //load Class
                    //load airline  #002
                    MultiFlt += "<div class='col-xs-12 col-sm-10 col-md-10 col-lg-10 col5' style='margin-top: 0px;z-index: 1;'>";//7
                    MultiFlt += "<div class='row row5'>"//6
                    //load airline  #002.1
                    MultiFlt += "<div class='col-xs-12 col5'>"//5
                    MultiFlt += "<div class='row row5'>"//4
                    MultiFlt += "<div class='col-md-10 col-xs-12 col5' style='border-right: 1px solid #f5f5f5;'>"//2
                    MultiFlt += "<div class='take-off col-sm-12 col5'>"//1
                    MultiFlt += "<div style='padding-left: 0px;'>"
                    MultiFlt += "<div class='wh33percent left fontsize11 bold dark'>" + JSONdata["Org"] + ""
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='wh33percent left center fontsize11 bold dark Durationcls'>Duration"
                    MultiFlt += "</div>"

                    //   var time = "";

                    
                       
                    //if (len > 0) {

                    //    time = airportwaitingcalc(JsonObj[len - 1].Arr, JsonObj[len].Dep);
                    //    MultiFlt += "<div class='hidden-xs wh33percent right textright fontsize11 bold dark' style='color:#fa7b23' >loyout Time=" + time + ""
                    //    MultiFlt += "</div>"
                    //}
                  




                    MultiFlt += "<div class='wh33percent right textright fontsize11 bold dark'>" + JSONdata["Des"] + ""
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='clearfix'></div>"
                    MultiFlt += "<div class='wh33percent left'>"
                    MultiFlt += "<div class='fcircle'>"
                    MultiFlt += "<div class='icon' style='padding: 0px 2px 0 2px; transform: rotate(-35deg);'><i class='soap-icon-plane-right yellow-color trnsformrotatedep'></i></div>"
                    MultiFlt += "</div>"
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='wh33percent left'>"
                    MultiFlt += "<div class='fcircle center'>"
                    MultiFlt += "<div class='icon' style='padding: 0px 5px 0px 0px;'><i class='soap-icon-clock yellow-color timerpadding'></i></div>"
                    MultiFlt += "</div>"
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='wh33percent right'>"
                    MultiFlt += "<div class='fcircle right'>"
                    MultiFlt += "<div class='icon' style='padding: 0px 0 0 2px; transform: rotate(30deg);'><i class='soap-icon-plane-right yellow-color trnsformrotatearr'></i></div>"
                    MultiFlt += "</div>"
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='clearfix'></div>"
                    MultiFlt += "<div class='fline2px'></div>"
                    MultiFlt += "<div class='wh33percent left fontsize11 bold'>" + JSONdata["Dep"] + ""
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='wh33percent left center fontsize11'>" + flyminutes + "h :" + flysecond + "m"
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='wh33percent right textright fontsize11 bold'>" + JSONdata["Arr"] + ""
                    MultiFlt += "</div>"
                    MultiFlt += "<div class='clearfix'></div>"
                    MultiFlt += "</div>"//1
                    MultiFlt += "</div>"//2
                    MultiFlt += "</div>"//3
                    MultiFlt += "<div class='col-md-2 col5'>"//3
                    MultiFlt += "<div class='amenities mob-conn-other conbaggageclas_" + Ttotlen + "' id='conbagg_" + len + "'>"
                    MultiFlt += "<span class='vcenter mob-conn-bagg' data-toggle='tooltip' data-placement='top' title='Baggage'><i class='soap-icon-suitcase ' style='font-size: 20px;'></i> <span class='baggfontcolr'>" + SegmentDetails[5].replace("\r\n", "") + "</span></span>"
                    MultiFlt += "<img src='../Images/info.gif' class='FlightTip' style='width: 15px; display:none;' id='extradetails_" + Ttotlen + "' rel=\"" + otherbenifit + "\"/></div>"
                    MultiFlt += "</div>"//4
                    MultiFlt += "</div>"//5
                    MultiFlt += "</div>"//6
                    //load airline  #002.1
                    MultiFlt += "</div>"//7
                    MultiFlt += "</div>";//8
                    //load airline  #002
                    //MultiFlt += "</div>"
                    MultiFlt += "</article>"
                    //load airline 
                    //connection another set
                    arr_dateSceond = JSONdata["Arr"];
                    arr_dateSceond = arr_dateSceond.substring(0, arr_dateSceond.length - 5);
                    arr_dateSceondvalidate = new Date(arr_dateSceond);
                    var EArrs = JSONdata["Arr"].split(' ');
                    EndArrivetime.push(EArrs[3]); EndtoolArr.push(JSONdata["Arr"].substring(0, JSONdata["Arr"].length - 5));

                    ariwatarrv = JSONdata["Arr"];

                    StartDepttime = new Array();


                  
                    

                    LLP++;
                }
                adt = $('#hdtxt_adultcount').val();
                cdt = $('#hdtxt_childcount').val();
                Seats = 'RushNo';
                if (JSONdata["Seats"] != "") {
                    if (JSONdata["Seats"] <= (parseInt(adt) + parseInt(cdt) + 2)) { Seats = "Rush" }
                }
                var tripflag = 0;

                if ($('#hdtxt_trip').val() == "Y") {
                    stopscnt = segLen - 1;
                }
                else if ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "I") { //Wanna check this block...
                    stopscnt = segLen - (aryOrgMul.length - 1);
                }
                else {
                    stopscnt = NoofStops.length > 0 ? NoofStops[0] : 0;
                }

                con_Dur = Math.floor((con_min % (60 * 60)) / 60);
                con_Durmin = Math.ceil((con_min % (60 * 60)) % 60);
                if (con_Durmin.value < 10) {
                    con_Durmin = "0" + con_Durmin;
                }
                /*-----------------------------------------------------------------------*/
                if (MultiFlt == "") { /*****************without Details Flight Binding ***********/

                    platingcarrier.push(JSONdata["plt"]);
                    /*-----------------------Flight Details*----------------------------------------------*/
                    splitFlightnumber = JSONdata["Fno"].split(' ');
                    splitairlinename = airlinename(splitFlightnumber[0]);
                    viaflight3 = ((JSONdata["nonstop"] != "" && JSONdata["nonstop"] != null) ? JSONdata["nonstop"] : "N/A");
                    originAirport = JSONdata["originfull"];  //$('#span_origin').text();
                    desinationAirport = JSONdata["destfull"];  //$('#span_Desination').text();
                    if (sameflighturl.length > 1) {
                        for (var fl = 0; fl < sameflighturl.length - 1; fl++) {
                            if (Sameflight == "Same") {
                                Sameflight = (sameflighturl[fl] == sameflighturl[fl + 1] ? "Same" : "Change");
                            }
                        }
                    }

                    //
                    //#region Start Availbuilding process
                    arrivtimewithnextdt = Arr.replace(':', '');
                    if (nextValidateDep < nextValidatearr) {
                        if ($('#hdtxt_trip').val() != "Y" && !($('#hdtxt_trip').val() == "M") && $('body').data('segtype') == "I") // Coz in Roundtrip special and Domestic Multicity fair has been combained...
                            arrivtimewithnextdt = Number(arrivtimewithnextdt) + 2400; //if Next Date arrival means sort this flight as last flight... by saranraj...
                    }


                    SAvail_build += "<div class='row boxshadcls'>"
                    if ($('#hdtxt_trip').val() == "Y") {
                       
                        onward = jQuery.grep(JsonObj, function (n, i) {
                            return n.iti == "0";
                        });
                        var onward_len = (onward.length - 1);
                        var onwardflig_set1 = onward[0].Org;
                        var onwardflig_set2 = onward[onward_len].Des;

                        var onwardflig_time1 = onward[0].Dep.split(' ')[3];
                        var onwardflig_time2 = onward[onward_len].Arr.split(' ')[3];


                        returnward = jQuery.grep(JsonObj, function (n, i) {
                            return n.iti == "1";
                        });
                        var returnward_len = (returnward.length - 1);
                        var returnwardflig_set1 = returnward[0].Org;
                        var returnwardflig_set2 = returnward[returnward_len].Des;
                        var returnwardflig_time1 = returnward[0].Dep.split(' ')[3];
                        var returnwardflig_time2 = returnward[returnward_len].Arr.split(' ')[3];

                       // deparrv_arrv = returnwardflig_time2.replace(':', '') + "_" + returnwardflig_time1.replace(':', '') + "_" + onwardflig_time2.replace(':', '') + "_" + onwardflig_time1.replace(':', '') + "_" + onwardflig_set1.replace(':', '') + "_" + onwardflig_set2.replace(':', '') + "_" + returnwardflig_set1.replace(':', '') + "_" + returnwardflig_set2.replace(':', '') + "_" + con_Fno.split(' ')[0];
                       // deparrv_arrv = Dep.replace(':', '') + "_" + Arr.replace(':', '') + "_" + con_Fno.split(' ')[0];
                        //var dddd = groupbind;
                        deparrv_arrv = groupbind;
                    }
                    else {
                        //deparrv_arrv = Dep.replace(':', '') + "_" + Arr.replace(':', '') + "_" + con_Fno.split(' ')[0];
                        //var dddd = groupbind;
                        deparrv_arrv = groupbind;
                    }
                    

                    //#set Avail 1
                    if (sr == 0) {
                        SAvail_build += "<article class='artiboxsha box  commonbox slide-slow-down articlegroupavail" + con_WTMFare + "  group_" + deparrv_arrv + "_flightno_" + con_Fno.split(' ')[1] + "'  data-grpclssname='group_" + deparrv_arrv + "_flightno_" + con_Fno.split(' ')[1] + "'  data-depdatetime='" + JSONdata["Dep"] + "'data-arrdatetime='" + JSONdata["Arr"] + "' data-snglpnr='" + JSONdata["SinglePNR"] + "' data-via='" + JSONdata["nonstop"] + "' data-price='" + (JSONdata["Incentive"].toString() == "0" ? con_WTMFare : lessincentivefare) + "' data-airline='" + JSONdata["Fno"] + "' data-depart='" + Dep.replace(':', '') + "' data-duration='" + con_Dur + con_Durmin + "' data-arriv='" + Arr.replace(':', '') + "' data-arrivsort='" + arrivtimewithnextdt + "' data-stops='" + (stopscnt > 2 ? '2+' : stopscnt) + "' data-refund='" + refunflg + "' data-fltTyp='" + arysplitacat[0] + "' data-earning='" + JSONdata["com"] + "' data-grporgvcity='" + JSONdata["Org"] + "' data-grpdesvcity='" + JSONdata["Des"] + "' data-grpairlinename='" + splitairlinename + "' data-parentdiv='availset_" + $('#hdtxt_trip').val() + "_" + JSONdata["bindside_6_arg"] + "' data-clssname='" + con_cls + "'  data-arg='" + JSONdata["bindside_6_arg"] + "' id='li_Rows" + Ttotlen + "'>";
                    }
                    else {
                        SAvail_build += "<article class='artiboxsha box  commonbox  articlegroupavail" + con_WTMFare + "  group_" + deparrv_arrv + "_flightno_" + con_Fno.split(' ')[1] + "'  data-grpclssname='group_" + deparrv_arrv + "_flightno_" + con_Fno.split(' ')[1] + "'  data-depdatetime='" + JSONdata["Dep"] + "'data-arrdatetime='" + JSONdata["Arr"] + "' data-snglpnr='" + JSONdata["SinglePNR"] + "' data-via='" + JSONdata["nonstop"] + "' data-price='" + (JSONdata["Incentive"].toString() == "0" ? con_WTMFare : lessincentivefare) + "' data-airline='" + JSONdata["Fno"] + "' data-depart='" + Dep.replace(':', '') + "' data-duration='" + con_Dur + con_Durmin + "' data-arriv='" + Arr.replace(':', '') + "' data-arrivsort='" + arrivtimewithnextdt + "' data-stops='" + (stopscnt > 2 ? '2+' : stopscnt) + "' data-refund='" + refunflg + "' data-fltTyp='" + arysplitacat[0] + "' data-earning='" + JSONdata["com"] + "' data-grporgvcity='" + JSONdata["Org"] + "' data-grpdesvcity='" + JSONdata["Des"] + "' data-grpairlinename='" + splitairlinename + "' data-parentdiv='availset_" + $('#hdtxt_trip').val() + "_" + JSONdata["bindside_6_arg"] + "' data-clssname='" + con_cls + "'  data-arg='" + JSONdata["bindside_6_arg"] + "' id='li_Rows" + Ttotlen + "'>";

                    }
                    //#div 1 start
                    SAvail_build += "<div class='row row0 row-mob-0' style='padding-bottom: 2px;'>"
                    //checkbox region start
                    SAvail_build += "<div class='"+col_xs_4or_2+" col-lg-2 col-md-2 col-sm-2 col5 l-hight l-mob-hight' style='text-align: center;z-index: 1;'>"
                    SAvail_build += "<span class='fltno'>";
                    SAvail_build += "<img class='FlightTip FlightTipimg loadfightinfo' data-popup='" + Ttotlen + "SltP" + splitFlightnumber[0] + "SltP" + originAirport + "SltP" + desinationAirport + "SltP" + JSONdata["plt"] + "SltP" + SegmentDetails[3].replace("\r\nEndTerminal", "") + "SltP" + SegmentDetails[4].replace("\r\nBaggage", "") + "SltP" + JSONdata["Bagg"] + "SltP" + JSONdata["multiclassKey"] + "SltP" + JSONdata["StkType"] + "SltP" + viaflight3 + "SltPfliimage' onmouseover='showflightpoup(this)' alt='' id='fliimage" + Ttotlen + "' style='width: 28px; height: 28px;'src=\"" + (Sameflight == "Same" ? JSONdata["ur"] : JSONdata["urlcnt"]) + "\"  rel=\"\"/></img>"//" + strFlightBuild + "
                    SAvail_build += "<br/ class='hidden-lg hidden-md hidden-sm'>";

                    SAvail_build += "<span class='li_flightno' style='white-space: nowrap;' id=FlightID" + Ttotlen + ">" + JSONdata["Fno"] + " </span>"
                    SAvail_build += "</span>" 
                    //Avali Airline Logo region
                    //Avail Multiclass region
                    if (MultiFlt == "") {
                        tripflag = 100;
                    }
                    else {
                        tripflag = 000;
                    }
                    //
                    if (JSONdata["multiclass"] == "1") {
                        var clscbn = JSONdata["Cab"].split(",");

                        multicls_grp += "<span>"
                        multicls_grp += "<select style='' class='MUlticlasselect multiclassoptions' id='fareClass" + Ttotlen + "' onchange='Changemulticlas(" + Ttotlen + "," + LLP + "," + JSONdata["multiclass"] + ")'>"
                        multicls_grp += "<option value=" + con_cls + ">" + con_cls + "</option>";
                        if ($('#hdnsAllowmulticlass').val() == "Y") {
                            for (var i = 0; i < clscbn.length; i++) {
                                if (clscbn[i] != "" && clscbn[i] != null) {
                                    multicls_grp += "<option value=" + clscbn[i] + ">" + clscbn[i] + "</option>"
                                }
                            }
                        }
                        multicls_grp += "</select>";
                        multicls_grp += "</span>"

                    }
                    else if (JSONdata["multiclass"] == "2") {
                        multicls_grp += "<span>"
                        multicls_grp += "<a href='javascript:void(0);' style='color: #0000fc;text-decoration: underline;' value='" + JSONdata["Cls"] + "' id='clsduplicate_" + LLP + "'  onclick='loadClassFunction(" + LLP + "," + segLen + "," + flag_cnt + "," + Ttotlen + "," + tripflag + ")'>" + JSONdata["Cls"] + "</a>";
                        multicls_grp += "<select style='display:none;' class='MUlticlasselect aftr multiclassoptions' id='fareClass" + LLP + "' onchange='Changemulticlas(" + Ttotlen + "," + LLP + "," + JSONdata["multiclass"] + ")'>";
                        multicls_grp += "<option value=" + JSONdata["Cls"] + ">" + JSONdata["Cls"] + "</option>"
                        multicls_grp += "</select>";
                        multicls_grp += " <br/><input style='display:none;' type='button' class='MuLtiClas' value='GetFare' />";
                        multicls_grp += "</span>";
                    }
                    else {
                        multicls_grp += "<span class='liclass_span' style='font-size: 11px;font-weight: 600;'><span class='afarecls multiclassoptions' style='cursor: default;' >" + con_cls + "</span></span>"//
                    }
                    //Avail Multiclass region
                    SAvail_build += " </div>"
                    //Avail details region
                    SAvail_build += "<div class=' " + col_xs_8or_10 + " col-sm-8 col15 col-mob-5 mobmrgbtn'>"
                    SAvail_build += "<div class='row col5'>"
                    SAvail_build += "<div class='col-xs-12 col10 sectordetailscls'>"
                    SAvail_build += "<div class='row row10'>"

                    var onward = "";
                    if ($('#hdtxt_trip').val()=="Y") {

                        onward = jQuery.grep(JsonObj, function (n, i) {
                             return n.iti == "0";
                        });
                    }
                    else {
                     
                        onward = JsonObj;
                    }
                  

                    var availabilitybind = layouttimebind(onward, stopscnt, $('#hdtxt_trip').val(), Ttotlen, nextarraival);
                    SAvail_build += "<div class='" + triptype_grp + " col5' style='border-right: 1px solid #f5f5f5;'>" + availabilitybind + "</div>"



                  
                    //End... ***

                    if (JSONdata["nonstop"] != "" && JSONdata["nonstop"] != null) {
                        SAvail_build += "<div class='row row1 test1' class='hidden-xs'>"
                        SAvail_build += '<div class="col-xs-4 col5">';
                        SAvail_build += "</div>"
                        SAvail_build += '<div class="col-sm-4 col5 ' + col_xs_2or4 + '" style="text-align: center;">';
                        SAvail_build += "<label style='text-align: center;color:red;font-size: 10px !important;max-width: 170px;text-overflow: ellipsis; width: 100%;white-space: nowrap;font-weight: 600;'>Via:" + JSONdata["nonstop"] + "</label>"
                        //SAvail_build += "<label style='text-align: center;color:red;font-size: 10px !important;max-width: 170px;text-overflow: ellipsis; width: 100%;white-space: nowrap;font-weight: 600;'>Via:BLR</label>"
                        SAvail_build += "</div>"
                        SAvail_build += '<div class="col-xs-4 col5">';
                        SAvail_build += "</div>"
                        SAvail_build += "</div>"
                    }

                    //SAvail_build += "</div>"


                    Refu = "";
                    Refu = refund.length > 0 ? refund[0] : JSONdata["Refund"];
                    var best = bestbuy.length > 0 ? bestbuy[0] : JSONdata["bestbuy"];
                    Refu = (Refu == "" ? "N/A" : Refu);


                    SAvail_build += "<div class='" + triptype_grp + " col5' style='display:" + triptype_grp_disp + "'>"


                    if ($('#hdtxt_trip').val() == "O") {
                        if ($.isNumeric(JSONdata["Seats"]) && Number(JSONdata["Seats"]) < 3) {
                            seatconu += (JSONdata["Seats"] != "" ? "<span class='Seatsleft rush '><span class='Seatcnt'>" + ($.isNumeric(JSONdata["Seats"]) ? JSONdata["Seats"] : "0") + "</span>Seat(s) Left</span>" : "<span class='Seatsleft rush  vis-hidn'><span class='Seatcnt'>0</span>Seat(s) Left</span>") // vis-hidn for Visibility hidden purpose (to zigzag order)... by saranraj on 20170517...
                        }
                        else {
                            seatconu += (JSONdata["Seats"] != "" ? "<span class='Seatsleft '><span class='Seatcnt'>" + ($.isNumeric(JSONdata["Seats"]) ? JSONdata["Seats"] : "0") + "</span>Seat(s) Left</span>" : "<span class='Seatsleft rush  vis-hidn'><span class='Seatcnt'>0</span>Seat(s) Left</span>")

                        }
                    }
                    else {
                        if ($.isNumeric(JSONdata["Seats"]) && Number(JSONdata["Seats"]) < 3) {

                            seatconu += (JSONdata["Seats"] != "" ? "<span class='Seatsleft rush '><span class='Seatcnt'>" + ($.isNumeric(JSONdata["Seats"]) ? JSONdata["Seats"] : "0") + "</span>Seat(s) Left</span>" : "<span class='Seatsleft rush  vis-hidn'><span class='Seatcnt'>0</span>Seat(s) Left</span>")

                        }
                        else {
                          
                            seatconu += (JSONdata["Seats"] != "" ? "<span class='Seatsleft '><span class='Seatcnt'>" + ($.isNumeric(JSONdata["Seats"]) ? JSONdata["Seats"] : "0") + "</span>Seat(s) Left</span>" : "<span class='Seatsleft rush  vis-hidn'><span class='Seatcnt'>0</span>Seat(s) Left</span>")

                        }
                    }

                    commissionbuild += (JSONdata["com"] != "0" ? " <span class='cls_showearning animated swing ' id='show_earning" + Ttotlen + "' style='margin-right: 16px;' data-toggle='tooltip' data-placement='top' title='Earnings'>Earn. " + JSONdata["com"] + "</span>" : "<span id='show_earning" + Ttotlen + "' ></span>")

                    basicfare += "<div id='Div2' class='basicfares ' style='text-align: left;'>";
                    basicfare += "<span style='color: #999; font-size: 9px;' data-toggle='tooltip' data-placement='bottom' title='Basic Fare'>Basic :</span>";
                    basicfare += "<span style='font-size: 15px;color: #222;font-weight: 400;' id='BaseFARE" + Ttotlen + "'>" + JSONdata["BFare"] + "</span>";
                    basicfare += "</div>";


                  
                    refund_grp += (Refu != "N/A" ? (Refu == "TRUE" ? "<i class='soap-icon circle green Refundable refund ' data-toggle='tooltip' data-placement='top' title='Refundable' style='font-weight: 500;'>R</i>" : "<i class='soap-icon circle orange NonRefundable refund ' style='font-weight: 500;' data-toggle='tooltip' data-placement='top' title='Non Refundable' >N</i>") : "<i class='soap-icon circle green refund ' style='display:none'>N/A</i>");


                    if (Checkbaggfunction(JSONdata["Bagg"]) == true) {
                        baggagedetails_grp += "<span class='vcenter mob-bagg cls5' data-toggle='tooltip' data-placement='top' title='Baggage: " + JSONdata["Bagg"].split('/')[0] + "'><i class='soap-icon-suitcase less400' id='showbaggimg" + Ttotlen + "'></i><span class='baggfontcolr' id='showbagg" + Ttotlen + "'>" + JSONdata["Bagg"].split('/')[0] + "</span></span>"
                    }
                    else {
                        baggagedetails_grp += "<span class='vcenter mob-bagg cls6' data-toggle='tooltip' data-placement='top' title='Baggage: " + SegmentDetails[5].replace("\r\n", "") + "'><i class='soap-icon-suitcase less400' id='showbaggimg" + Ttotlen + "'></i><span class='baggfontcolr' id='showbagg" + Ttotlen + "'>" + SegmentDetails[5].replace("\r\n", "") + "</span></span>"
                    }
                
            
                    farerule_grp += "<span class='commontoggle clsavail_dtls' onclick=GetFareRule(" + Ttotlen + ");>Fare Rule</span>";
                    SAvail_build += "</div>";


                    SAvail_build += "</div>";
                    SAvail_build += "</div>";
                    SAvail_build += " </div>"
                    SAvail_build += "</div>"
                    //For Mobile view only ***
                    //SAvail_build += "<div class='col-xs-12 col-mob-5 hidden-lg hidden-md hidden-sm clsmobavailothers'></div>";
                    //For Mobile view only End ***
                    //button area ***






                    SAvail_build += "<div class='col-sm-2 col-xs-12 col-mob-5 farearea'>";
                    SAvail_build += "<div class='mk-trc mob-mk-trc' data-style='check' data-text='true' style='float: right'>";
                    SAvail_build += "<input id='lichechbox" + Ttotlen + "' type='checkbox' data-val='" + Ttotlen + "' value='" + First_inva + JSONdata["Inva"] + "' name='radio1' class='checkbox_availchk SelecCheckbox  classcehcksr'>";

                    SAvail_build += "<label for='lichechbox" + Ttotlen + "' class='lblcheckbox'><i class='checkbox_avail checkbox_availchk1'></i>"

                    if (JSONdata["cachetrueorfalse_7_arg"] == true) {

                        SAvail_build += "<br/><i class='checkbox_avail1 checkbox_availchk1 mcache' style='height: 1px;margin-top: -6px;background-color: green;'></i> </label>"
                    }
                    else {
                        SAvail_build += "<br/><i class='checkbox_avail1 checkbox_availchk1 mcache' style='height: 1px;margin-top: -6px;background-color: red;'></i> </label>";
                    }

                    SAvail_build += "</div>";


                    //Fare Popup by saranraj on 20170513...
                    STax_build += "<table width='130px' >";
                    for (var tLen = 0; tLen < JSONdata["tax"].length; tLen++) {
                        JsonTax = "";
                        JsonTax = JSONdata["tax"][tLen];
                        STax_build += "<tr><td>" + JsonTax["COD"] + "</td><td>" + JsonTax["AMT"] + "</td></tr>";
                    }
                    if (JSONdata["Markup"].toString() != "0") {
                        STax_build += "<tr class='Markup' ><td>SC</td><td>" + JSONdata["Markup"] + "</td></tr>";
                    }
                    if (JSONdata["Service"].toString() != "0") {
                        STax_build += "<tr><td> Serv. Chg.</td><td>" + JSONdata["Service"] + "</td></tr>";
                    }
                    if (JSONdata["Incentive"].toString() != "0") {
                        STax_build += "<tr><td>Discount</td><td>" + JSONdata["Incentive"] + "</td></tr>";
                    }
                   
                    STax_build += "<tr class='WTMFare' ><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + JSONdata["WTMFare"] + "</td></tr>";
                    /**/

                    /**/
                    STax_build += "<tr class='CGFare' ><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + con_GFare + "</td></tr>";
                    /**/

                    STax_build += "</table>";
                    //Fare Popup by saranraj on 20170513 End...

                    if (JSONdata["Incentive"].toString() == "0") {
                        SAvail_build += "<div style='padding: 0px 0px 0px 0px !important; text-align:center;' id='GorssFareSpan2" + Ttotlen + "' data-WTMFare='" + con_WTMFare + "' data-grrFare='" + con_GFare + "' data-id='" + Ttotlen + "' class='liGrossfare_span'>"
                        SAvail_build += "<span class='price mob-price' style='white-space: nowrap;' for='radioRTavail_" + Ttotlen + "' id='lblGFare" + Ttotlen + "' onmousemove='CheckGrossFare(" + Ttotlen + ");' onmouseout='checkoutGrossFare(" + Ttotlen + ")'><span  class='currencyclass'>" + assignedcurrency + "</span>" + con_WTMFare + "</span>"
                        SAvail_build += " </div>"
                        SAvail_build += "<div id='taxtdiv" + Ttotlen + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div>";
                        SAvail_build += "<div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
                        SAvail_build += "<span id='span_GrossFare" + Ttotlen + "' style='display:none;'> " + con_WTMFare + " </span> ";
                    }
                    else {
                        SAvail_build += "<div style='padding: 0px 0px 0px 0px !important; text-align:center;' id='GorssFareSpan2" + Ttotlen + "' data-WTMFare='" + lessincentivefare + "' data-grrFare='" + lessincentivefarewitgros + "' data-id='" + Ttotlen + "' class='liGrossfare_span'>"
                        SAvail_build += "<p class='Bestbuyfarewithmarkup' style='margin-left: 0px;display:none' ><strike>" + con_GFare + "</strike></p><p class='Bestbuyfarewithoutmarkup' style='margin-left: 0px;' ><strike>" + con_WTMFare + "</strike></p>";
                        SAvail_build += "<span class='price mob-price' style='white-space: nowrap;' for='radioRTavail_" + Ttotlen + "' id='lblGFare" + Ttotlen + "' onmousemove='CheckGrossFare(" + Ttotlen + ");' onmouseout='checkoutGrossFare(" + Ttotlen + ")'><span  class='currencyclass'>" + assignedcurrency + "</span>" + con_WTMFare + "</span>"
                        SAvail_build += " </div>"

                        SAvail_build += "<div id='taxtdiv" + Ttotlen + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div>";
                        SAvail_build += "<div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
                        SAvail_build += "<span id='span_GrossFare" + Ttotlen + "' style='display:none;'> " + lessincentivefare + " </span> ";
                    }

                    //JSONdata["availsr_id"]= Ttotlen;
                    //first array assigned sri
                    // dataproperty_array.push(JSONdata["availsr_id"]);
                    dataproperty_array.push({
                        "datakey": Ttotlen,
                        "data-bestbuypopupvalues": '',
                        "data-hdnmarkupfare": JSONdata["Markup"],
                        "data-hdbestbuyy": best,
                        "data-hdAlterQueue": 'N',
                        "data-hdInva": JSONdata["Inva"],
                        "data-hdtoken": JSONdata["Ref"],
                        "data-hdoffflg": JSONdata["offFlag"],
                        "data-platcarrierr": JSONdata["plt"],
                        "data-AirlineCategory": JSONdata["acat"],
                        "data-refundableforbest": Refu,
                        "data-oldtaxbreakup": JSONdata["taxbreakupold"],
                        "data-oldcommission": JSONdata["com"],
                        "data-grpclssname": "group_" + deparrv_arrv + "_flightno_" + con_Fno.split(' ')[1] + "",
                        "data-clssname": con_cls,
                        "data-arg": JSONdata["bindside_6_arg"],

                        "data-grporgvcitymul": JSONdata["Org"],
                        "data-grpdesvcitymul": JSONdata["Des"]

                        // data-arg

                    });

                    SAvail_build += " <div class='second-row'>"
                    SAvail_build += "<div class='action Travselect_" + Ttotlen + "' style='text-align: center;'>"
                    //SAvail_build += "<a  class='button btn-small full-width'>SELECT</a>"
                    if ($('#hdtxt_trip').val() == "O" || $('#hdtxt_trip').val() == "Y" || ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "I")) {

                        var tripfg = $('#hdtxt_trip').val();
                        SAvail_build += (best == "TRUE" ? "<a id='bestbuyclickbutton" + Ttotlen + "' class=\" button btn-small book_flight_button Specialfare_btn book_btn-primary mob-selectbutton splselebtn \" onclick='javascript:getSplfare(" + Ttotlen + ")'><i class='fa fa-thumbs-o-up splfricon'></i>Select</a><a style='display:none' id='selectclickbutton" + Ttotlen + "' class=\" book_flight_button book_btn book_btn-primary SpecialfareBlue button btn-small mob-selectbutton splselebtn \"  onclick='javascript:GetRowIndexSelect_old(" + Ttotlen + ",\"singletripifflg\",\"tripfg\")'>Select  </a>" : "<a id='selectclickbutton" + Ttotlen + "' class=\" button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton selebtn \"  onclick='javascript:GetRowIndexSelect_old(" + Ttotlen + ",\"singletripifflg\",\"tripfg\")'>Select</a>");

                        //SAvail_build += (best == "TRUE" ? "<a id='bestbuyclickbutton" + Ttotlen + "' class=\" button btn-small book_flight_button Specialfare_btn book_btn-primary mob-selectbutton splselebtn \" onclick='javascript:getSplfare(" + Ttotlen + ")'><i class='fa fa-thumbs-o-up splfricon'></i>Select</a><a style='display:none' id='selectclickbutton" + Ttotlen + "' class=\" book_flight_button book_btn book_btn-primary SpecialfareBlue button btn-small mob-selectbutton splselebtn \"  onclick='javascript:GetRowIndexSelect_old(" + Ttotlen + ")>Select  </a>" : "<a id='selectclickbutton" + Ttotlen + "' class=\" button btn-small book_flight_button book_btn book_btn-primary book_btn-action mob-selectbutton selebtn \"  onclick='javascript:GetRowIndexSelect_old(" + Ttotlen + ")'>Select</a>");


                    }
                    else {
                        if (best == "TRUE") {
                            SAvail_build += '<div class="rb-div roundTripFareRadio bestFareRadio">';
                            SAvail_build += '<input type="radio" class="rb" id="radioRTavail_' + Ttotlen + '" name="nameradio' + JSONdata["bindside_6_arg"] + '" onclick=' + '"' + "javascript:GetRowIndexSelect_old('" + Ttotlen + "','" + doubletripifflg + "','" + JSONdata["bindside_6_arg"] + "')" + '"' + '><label for="radioRTavail_' + Ttotlen + '" class="rblbl"></label>';
                            SAvail_build += '<div class="bullet"> <div class="line zero"></div> <div class="line one"></div> <div class="line two"></div> <div class="line three"></div> <div class="line four"></div> <div class="line five"></div> <div class="line six"></div> <div class="line seven"></div> </div>';
                            SAvail_build += '</div>';
                        }
                        else {
                            SAvail_build += '<div class="rb-div roundTripFareRadio">';
                            SAvail_build += '<input class="tgl tgl-flip" id="radioRTavail_' + Ttotlen + '" name="nameradio' + JSONdata["bindside_6_arg"] + '" onclick=' + '"' + "javascript:GetRowIndexSelect_old('" + Ttotlen + "','" + doubletripifflg + "','" + JSONdata["bindside_6_arg"] + "')" + '"' + '  name="nameradio' + JSONdata["bindside_6_arg"] + '" onclick=' + '"' + "javascript:GetRowIndexSelect_old('" + Ttotlen + "','" + doubletripifflg + "','" + JSONdata["bindside_6_arg"] + "')" + '"' + ' type="radio" style="height:0px;"> <label class="tgl-btn" data-tg-off="Select" data-tg-on="Selected" for="radioRTavail_' + Ttotlen + '"></label>';
                            //<input type="radio" class="rb" id="radioRTavail_' + Ttotlen + '" name="nameradio' + arg[6] + '" onclick="javascript:bindSletedRndTrpAvail(' + Ttotlen + ',' + arg[6] + ')"><label for="radioRTavail_' + Ttotlen + '" class="rblbl"></label>';
                            //SAvail_build += '<div class="bullet"> <div class="line zero"></div> <div class="line one"></div> <div class="line two"></div> <div class="line three"></div> <div class="line four"></div> <div class="line five"></div> <div class="line six"></div> <div class="line seven"></div> </div>';
                            SAvail_build += '</div>';
                        }
                    }
                    SAvail_build += "<input type='button' style='display:none;' class=' button btn-small mob-selectbutton selebtn MuLtiClas' value='GetFare' id='btn_multiclas_" + Ttotlen + "' onclick='getmulticlasfare(" + Ttotlen + "," + LLP + ")'/>";
                    SAvail_build += "<input type='button' style='display:none;margin-left: 20%;' class=' button btn-small mob-selectbutton selebtn MuLtiClas' id='btn_multiclasforconn_" + Ttotlen + "' onclick='getmulticlasfareforcon(" + flag_cnt + "," + Ttotlen + "," + JSONdata["multiclass"] + ")' value='Get Fare'>";
                    //TravRsri
                    SAvail_build += "</div>"
                    SAvail_build += "</div>"
                    SAvail_build += "</div>"
                    //button area End***
                    //Avail details region
                    SAvail_build += "</div>"
                    //#div 1 end
                    SAvail_build += "<div class='col-xs-12 col-mob-5 clsMobRavailBotm'></div>"; //Added by Saranraj for Roundtrip Mobile view only... (not using... only empty space)...
                    LLP++;
                }
                else //***************** with Details Flight Binding ***********/
                {
                    if (JSONdata["Bagg"].indexOf("N/A") != 0) {
                        crntBagg = JSONdata["Bagg"].split('/');
                        uniqueBagg = []; //jQuery.unique(crntBagg);
                        for (var cc = 0; cc < crntBagg.length; cc++) {
                            if (uniqueBagg.indexOf(crntBagg[cc].trim()) == -1) {
                                uniqueBagg.push(crntBagg[cc].trim());
                            }
                        }
                    }
                    else {
                        uniqueBagg = [];
                        uniqueBagg.push("N/A");
                    }
                    platingcarrier.push(JSONdata["plt"]);
                    /*-----------------------Flight Details*----------------------------------------------*/
                    splitFlightnumber = con_Fno.split(' ');
                    splitairlinename = airlinename(splitFlightnumber[0]);
                    originAirport = con_Dep_fullcity; //$('#span_origin').text();
                    viaflight4 = ((JSONdata["nonstop"] != "" && JSONdata["nonstop"] != null) ? JSONdata["nonstop"] : "N/A");
                    desinationAirport = con_Arr_fullcity; //$('#span_Desination').text();
                    if (sameflighturl.length > 1) {
                        for (var fl = 0; fl < sameflighturl.length - 1; fl++) {
                            if (Sameflight == "Same") {
                                Sameflight = (sameflighturl[fl] == sameflighturl[fl + 1] ? "Same" : "Change");
                            }
                        }
                    }
                    arrivtimewithnextdt = EndArrivetime[0].replace(':', '');
                    if (nextValidateDep < arr_dateSceondvalidate) {
                        if ($('#hdtxt_trip').val() != "Y" && !($('#hdtxt_trip').val() == "M") && $('body').data('segtype') == "I") // Coz in Roundtrip special and Domestic Multicity fair has been combained...
                            arrivtimewithnextdt = Number(arrivtimewithnextdt) + 2400; //if Next Date arrival means sort this flight as last flight... by saranraj...
                    }
                    //start avail
                    //load new availss
                    SAvail_build += "<div class='row boxshadcls'>"

                    

                    if ($('#hdtxt_trip').val() == "Y")  {
                        onward = jQuery.grep(JsonObj, function (n, i) {
                             return n.iti == "0";
                        });
                        var onward_len = (onward.length - 1);
                        var onwardflig_set1 = onward[0].Org;
                        var onwardflig_set2 = onward[onward_len].Des;
                        var onwardflig_time1 = onward[0].Dep.split(' ')[3];
                        var onwardflig_time2 = onward[onward_len].Arr.split(' ')[3];

                        returnward = jQuery.grep(JsonObj, function (n, i) {
                            return n.iti == "1";
                        });
                        var returnward_len = (returnward.length - 1);
                        var returnwardflig_set1 = returnward[0].Org;
                        var returnwardflig_set2 = returnward[returnward_len].Des;
                        var returnwardflig_time1 = returnward[0].Dep.split(' ')[3];
                        var returnwardflig_time2 = returnward[returnward_len].Arr.split(' ')[3];

                     //   deparrv_arrv = returnwardflig_time2.replace(':', '') + "_" + returnwardflig_time1.replace(':', '') + "_" + onwardflig_time2.replace(':', '') + "_" + onwardflig_time1.replace(':', '') + "_" + onwardflig_set1.replace(':', '') + "_" + onwardflig_set2.replace(':', '') + "_" + returnwardflig_set1.replace(':', '') + "_" + returnwardflig_set2.replace(':', '') + "_" + con_Fno.split(' ')[0];
                        //deparrv_arrv = con_Dep.replace(':', '') + "_" + EndArrivetime[0].replace(':', '') + "_" + con_Fno.split(' ')[0];
                        deparrv_arrv = groupbind;
                    }
                    else {
                        //deparrv_arrv = con_Dep.replace(':', '') + "_" + EndArrivetime[0].replace(':', '') + "_" + con_Fno.split(' ')[0];
                        deparrv_arrv = groupbind;
                    }
                    
                    if (sr == 0) {
                        SAvail_build += "<article class='artiboxsha box commonbox litagclass slide-slow-down  articlegroupavail" + con_WTMFare + " group_" + deparrv_arrv + "_flightno_" + con_Fno.split(' ')[1] + " ' data-grpclssname='group_" + deparrv_arrv + "_flightno_" + con_Fno.split(' ')[1] + "' data-depdatetime='" + depDatetime + "'data-arrdatetime='" + JSONdata["Arr"] + "' data-snglpnr='" + JSONdata["SinglePNR"] + "' data-via='" + JSONdata["nonstop"] + "' data-price='" + (JSONdata["Incentive"].toString() == "0" ? con_WTMFare : lessincentivefare) + "' data-airline='" + con_Fno + "' data-depart='" + con_Dep.replace(':', '') + "' data-duration='" + con_Dur + con_Durmin + "' data-arriv='" + EndArrivetime[0].replace(':', '') + "' data-arrivsort='" + arrivtimewithnextdt + "' data-stops='" + (stopscnt > 2 ? '2+' : stopscnt) + "' data-refund='" + refunflg + "' data-fltTyp='" + arysplitacat[0] + "' data-earning='" + con_Comm + "' data-grporgvcity='" + orgincount + "' data-grpdesvcity='" + JSONdata["Des"] + "' data-grpairlinename='" + splitairlinename + "' data-parentdiv='availset_" + $('#hdtxt_trip').val() + "_" + JSONdata["bindside_6_arg"] + "' data-clssname='" + con_cls + "' data-arg='" + JSONdata["bindside_6_arg"] + "' id='li_Rows" + Ttotlen + "'>";

                    }
                    else {
                        SAvail_build += "<article class='artiboxsha box commonbox litagclass articlegroupavail" + con_WTMFare + " group_" + deparrv_arrv + "_flightno_" + con_Fno.split(' ')[1] + " ' data-grpclssname='group_" + deparrv_arrv + "_flightno_" + con_Fno.split(' ')[1] + "' data-depdatetime='" + depDatetime + "'data-arrdatetime='" + JSONdata["Arr"] + "' data-snglpnr='" + JSONdata["SinglePNR"] + "' data-via='" + JSONdata["nonstop"] + "' data-price='" + (JSONdata["Incentive"].toString() == "0" ? con_WTMFare : lessincentivefare) + "' data-airline='" + con_Fno + "' data-depart='" + con_Dep.replace(':', '') + "' data-duration='" + con_Dur + con_Durmin + "' data-arriv='" + EndArrivetime[0].replace(':', '') + "' data-arrivsort='" + arrivtimewithnextdt + "' data-stops='" + (stopscnt > 2 ? '2+' : stopscnt) + "' data-refund='" + refunflg + "' data-fltTyp='" + arysplitacat[0] + "' data-earning='" + con_Comm + "' data-grporgvcity='" + orgincount + "' data-grpdesvcity='" + JSONdata["Des"] + "' data-grpairlinename='" + splitairlinename + "' data-parentdiv='availset_" + $('#hdtxt_trip').val() + "_" + JSONdata["bindside_6_arg"] + "' data-clssname='" + con_cls + "' data-arg='" + JSONdata["bindside_6_arg"] + "' id='li_Rows" + Ttotlen + "'>";

                    }
                    //avail #div 1-S
                    SAvail_build += "<div class='row row0 row-mob-0' style='padding-bottom: 2px;'>";
                    SAvail_build += "<div class='" + col_xs_4or_2 + " col-lg-2 col-md-2 col-sm-2 col5 l-mob-hight' style='text-align: center;z-index: 1;'>"
                    SAvail_build += "<span class='fltno'>"
                    //SAvail_build += "<img class='FlightTip FlightTipimg' alt='' id='fliimage" + Ttotlen + "' style='width: 28px; height: 28px;' src=\"" + (Sameflight == "Same" ? JSONdata["ur"] : JSONdata["urlcnt"]) + "\" rel=\"" + strFlightBuild + "\">"
                    SAvail_build += "<img class='FlightTip FlightTipimg loadfightinfo'  data-popup='" + Ttotlen + "SltP" + splitFlightnumber[0] + "SltP" + originAirport + "SltP" + desinationAirport + "SltP" + JSONdata["plt"] + "SltP" + (con_seg[2].split(':')[1].replace("\r", "").trim() == "" ? "N/A" : con_seg[2].split(':')[1].replace("\r", "")) + "SltP" + (con_seg[3].split(':')[1].replace("\r", "").trim() == "" ? "N/A" : con_seg[3].split(':')[1].replace("\r", "")) + "SltP" + (uniqueBagg.length == 1 ? uniqueBagg[0] : JSONdata["Bagg"]) + "SltP" + JSONdata["multiclassKey"] + "SltP" + JSONdata["StkType"] + "SltP" + viaflight4 + "SltPfliimage' onmouseover='showflightpoup(this)' alt='' id='fliimage" + Ttotlen + "' style='width: 28px; height: 28px;' src=\"" + (Sameflight == "Same" ? JSONdata["ur"] : JSONdata["urlcnt"]) + "\" rel=\"\">"//" + strFlightBuild + "

                    SAvail_build += "<br/ class='hidden-lg hidden-md hidden-sm'>";
                    SAvail_build += "<span class='li_flightno' id=FlightID" + Ttotlen + ">" + con_Fno + " </span>"
                    SAvail_build += "</span>"

                  

                    multicls_grp += "<span class='liclass_span'>"
                    multicls_grp += "<span style='font-size: 11px;font-weight: 600;'>" + (JSONdata["multiclass"] == "1" || JSONdata["multiclass"] == "2" ? "<span class='afarecls multiclassoptions' style='cursor: default;padding: 5px;' onclick='javascript:GetMultiClassrow(" + Ttotlen + "," + LLP + ")'>" : "<span class='afarecls multiclassoptions' style='cursor: default;padding: 5px;' onclick='javascript:GetMultiClassrow(" + Ttotlen + ")'>") + con_cls + "</span><select class='farecls multiclassoptions' style='' id='fareClass" + Ttotlen + "'><option value=" + JSONdata["Cls"] + ">" + con_cls + "</option></select></span>"
                    multicls_grp += " </span>"

                    if (nextValidateDep < arr_dateSceondvalidate) {
                        if ($('#hdtxt_trip').val() != "Y" && !($('#hdtxt_trip').val() == "M") && $('body').data('segtype') == "I") // Coz in Roundtrip special and Domestic Multicity fair has been combained...
                            nextarraival += '<span class="nextdayclss" data-toggle="tooltip" data-placement="bottom" title="Next Day Arrival" style="position: absolute;float: left;left: 7px;bottom: 24px;">+1</span>';
                    }

                    SAvail_build += "</div>"
                    Refu = "";
                    Refu = refund.length > 0 ? refund[0] : JSONdata["Refund"];
                    var best = bestbuy.length > 0 ? bestbuy[0] : JSONdata["bestbuy"];
                    //avail load details new
                    SAvail_build += " <div class='TEST " + col_xs_8or_10 + " col-sm-8 col15 col-mob-5 mobmrgbtn'>"
                    SAvail_build += " <div class='row col5'>"
                    SAvail_build += "<div class='col-xs-12 col10 sectordetailscls'>"
                    SAvail_build += "<div class='row row10'>"


                    var onward = "";
                    if ($('#hdtxt_trip').val() == "Y") {

                        onward = jQuery.grep(JsonObj, function (n, i) {
                            return n.iti == "0";
                        });
                        var availabilitybind = layouttimebind(onward, stopscnt, $('#hdtxt_trip').val(), Ttotlen, nextarraival);
                        SAvail_build += "<div class='" + triptype_grp + " col5' style='border-right: 1px solid #f5f5f5;'>" + availabilitybind + "</div>"

                    }
                    else if ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "I") {

                        for (var j = 0; j < aryOrgMul.length; j++) {

                            onward = jQuery.grep(JsonObj, function (n, i) {
                                return n.iti == j.toString();
                            });
                            var wid = 100 / Number(aryOrgMul.length);
                            var availabilitybind = layouttimebind(onward, stopscnt, $('#hdtxt_trip').val(), Ttotlen, nextarraival);
                            var paddinglft = "";
                            if (j==0) {
                                paddinglft = "initial";
                            }
                            else {
                                paddinglft = "15px";
                            }
                            SAvail_build += "<div class='multicityclsoption' style='width:" + wid + "%;border-right: 1px solid #f5f5f5;float: left;padding-left: " + paddinglft + ";'>" + availabilitybind + "</div>"

                        }


                    }
                    else {
                        onward = JsonObj;
                        var availabilitybind = layouttimebind(onward, stopscnt, $('#hdtxt_trip').val(), Ttotlen, nextarraival);
                        SAvail_build += "<div class='" + triptype_grp + " col5' style='border-right: 1px solid #f5f5f5;'>" + availabilitybind + "</div>"
                    }
                   
                    //SAvail_build += '<div class="row row10"> <div class="col-xs-12"> <div class="clsavalilcitydivid"></div> </div> </div>';
                    //SAvail_build += "<div class='take-off pt row row5 tet2'>"
                    //By Saranraj... and srinath***

                    //SAvail_build += '<div class="col-xs-4 col5">';
                    //SAvail_build += '<span class="bold dark clsavailcitycode">' + con_org + '</span>';
                    //SAvail_build += '<span class="clsavailcityicon s"></span>';
                    //SAvail_build += '<span class="clsavailarrow"><i class="fa fa-long-arrow-right"></i></span>'; //Only for Roundtrip Mobile view
                    //SAvail_build += '<br>';
                    //SAvail_build += '<span class="bold dark clsavailTimes" id="span_depature' + Ttotlen + '" data-toggle="tooltip" data-placement="bottom" title="DEP: ' + toolDep + '">' + con_Dep + '</span>';
                    //SAvail_build += '</div>';

                    

                    //SAvail_build += '<div class="col-sm-4 col5 ' + col_xs_2or4 + '">';
                    //SAvail_build += '<div class="icon clsDuration_show_hide" style="text-align: center;" data-toggle="tooltip" data-placement="bottom" title="Duration">';
                    //if (($("#hdn_allowtrip").val().indexOf("Y") > -1 && $('#hdtxt_trip').val() == "Y") && destinationcity != "") {
                    //    SAvail_build += '<span class="rtsarrivcity">' + destinationcity + '</span>';
                    //}
                    //SAvail_build += '<i class="soap-icon-clock yellow-color timerpadding"></i>';
                    //SAvail_build += '<br>';
                    //SAvail_build += '<span class="clsavailDuration s" id="span_dur' + Ttotlen + '" >' + con_Dur + 'h : ' + con_Durmin + 'm' + '</span>';
                    //SAvail_build += '</div>';
                    //SAvail_build += '</div>';



                    //SAvail_build += '<div class="col-xs-4 col5" style="text-align:right;">';
                    //SAvail_build += '<span class="clsavailcityicon rt-28"></span>';
                    //SAvail_build += '<span class="bold dark clsavailcitycode">' + JSONdata["Des"] + '</span>';
                    //SAvail_build += '<br>';
                   
                    //SAvail_build += '<span class="bold dark clsavailTimes" id="span_Arrival' + Ttotlen + '" data-toggle="tooltip" data-placement="bottom" title="ARR: ' + EndtoolArr[0] + '">' + EndArrivetime[0] + '</span>';
                    //SAvail_build += '</div>';

                    //SAvail_build += '</div>';


                    // SAvail_build += '<div class="row row10"><div class="col-xs-12 col5">';   ////hide  by srianth design develop purpose
                    // SAvail_build += '<div class="clsAvailStopsbg">';
                    // if (stopscnt > 0)
                    //     SAvail_build += '<span id="noOfstops' + Ttotlen + '" class="clsAvailStops" data-toggle="tooltip" data-placement="top" title="No. of stops">' + stopscnt + '</span>';
                    // else
                    //     SAvail_build += '<span id="noOfstops' + Ttotlen + '" class="clsAvailStops" style="visibility:hidden;">' + stopscnt + '</span>'; //visibility Hidden mode for allignment mismatch purpose...

                    // SAvail_build += '<span class="clsAvailallcnxcity">' + via + '</span>';

                    if ($('#hdtxt_trip').val() == "R") {
                        getdetails += "<span id='Viewbutton" + Ttotlen + "' class='commontoggle clsavail_dtls positionrela' onclick='GetDetails(" + Ttotlen + ")'> Details";
                    }
                    else {
                        getdetails += "<span id='Viewbutton" + Ttotlen + "' class='commontoggle clsavail_dtls '  onclick='GetDetails(" + Ttotlen + ")'> Details";

                    }
                    //End... ***




                   
                    //////////////////////////////////////////////////////////////////////

                    Refu = "";
                    Refu = refund.length > 0 ? refund[0] : JSONdata["Refund"];
                    var best = bestbuy.length > 0 ? bestbuy[0] : JSONdata["bestbuy"];
                    Refu = (Refu == "" ? "N/A" : Refu);

                    
                    var onward = "";
                    if ($('#hdtxt_trip').val() == "Y") {

                        onward = jQuery.grep(JsonObj, function (n, i) {
                             return n.iti == "1";
                        });
                    }
                    else {
                        onward = JsonObj;
                    }

                    
                    var retunrfli = returnflighticon(onward);
                    SAvail_build += " <div class='col-xs-2 col-lg-2 col5' style='display:" + triptype_grp_disp + ";text-align: center;position: relative;'>" + retunrfli + "</div>";
                    var availabilitybind = layouttimebind(onward, stopscnt, $('#hdtxt_trip').val(), Ttotlen, nextarraival);
                    SAvail_build += "<div class='" + triptype_grp + " col5' style='display:" + triptype_grp_disp + "'>" + availabilitybind + "</div>";

                 
                    if ($.isNumeric(JSONdata["Seats"]) && Number(JSONdata["Seats"]) < 3) {
                         seatconu += (JSONdata["Seats"] != "" ? "<span class='Seatsleft rush '><span class='Seatcnt'>" + ($.isNumeric(JSONdata["Seats"]) ? JSONdata["Seats"] : "0") + "</span>Seat(s) Left</span>" : "<span class='Seatsleft rush  vis-hidn'><span class='Seatcnt'>0</span>Seat(s) Left</span>")

                    }
                    else {
                        seatconu += (JSONdata["Seats"] != "" ? " <span class='Seatsleft '><span class='Seatcnt'>" + ($.isNumeric(JSONdata["Seats"]) ? JSONdata["Seats"] : "0") + "</span>Seat(s) Left</span>" : "<span class='Seatsleft rush  vis-hidn'><span class='Seatcnt'>0</span>Seat(s) Left</span>");

                    }

                    commissionbuild += (con_Comm != "0" ? "<span class='cls_showearning animated swing' id='show_earning" + Ttotlen + "' data-toggle='tooltip' data-placement='top' title='Earnings' >Earn. " + con_Comm + "</span>" : "<span id='show_earning" + Ttotlen + "' ></span>")

                
                    basicfare += "<div id='Div3' class='basicfares hidden-xs' style='text-align: left;'>";
                    basicfare += "<span style='color: #999; font-size: 9px;' data-toggle='tooltip' data-placement='bottom' title='Basic Fare'>Basic :</span>";
                    basicfare += "<span style='font-size: 15px;color: #222;font-weight: 400;' id='BaseFARE" + Ttotlen + "'>" + con_BFare + "</span>";
                    basicfare += "</div>";
                
                    refund_grp += (Refu != "N/A" ? (Refu == "TRUE" ? "<i class='soap-icon circle green Refundable refund ' data-toggle='tooltip' data-placement='top' title='Refundable' style='font-weight: 500;'>R</i>" : "<i class='soap-icon circle orange NonRefundable refund ' style='font-weight: 500;' data-toggle='tooltip' data-placement='top' title='Non Refundable' >N</i>") : "<i class='soap-icon circle green refund' style='display:none'>N/A</i>");


                    if (nextValidateDep < arr_dateSceondvalidate) {
                        if ($('#hdtxt_trip').val() == "O" || $('#hdtxt_trip').val() == "Y") {
                            baggagedetails_grp += "<span class='vcenter mob-bagg cls9' data-toggle='tooltip' data-placement='top' title='Baggage: " + (uniqueBagg.length == 1 ? uniqueBagg[0] : JSONdata["Bagg"]) + "'><i class='soap-icon-suitcase less400' id='showbaggimg" + Ttotlen + "'></i><span class='baggfontcolr' id='showbagg" + Ttotlen + "'>" + (uniqueBagg.length == 1 ? uniqueBagg[0] : JSONdata["Bagg"]) + "</span></span>"
                        }
                        else {
                            if ($('#hdtxt_trip').val() == "R") {
                                baggagedetails_grp += "<span class='vcenter mob-bagg cls12' data-toggle='tooltip' data-placement='top' title='Baggage: " + (uniqueBagg.length == 1 ? uniqueBagg[0] : JSONdata["Bagg"]) + "'><i class='soap-icon-suitcase less400' id='showbaggimg" + Ttotlen + "' ></i><span class='baggfontcolr' id='showbagg" + Ttotlen + "'>" + (uniqueBagg.length == 1 ? uniqueBagg[0] : JSONdata["Bagg"]) + "</span></span>"

                            } else {
                                //  SAvail_build += "<span class='liDuration_span2' ></span>  ";
                            }
                        }
                    }
                    else {
                        if ($('#hdtxt_trip').val() == "O" || $('#hdtxt_trip').val() == "Y") {
                            baggagedetails_grp += "<span class='vcenter mob-bagg cls15' data-toggle='tooltip' data-placement='top' title='Baggage: " + (uniqueBagg.length == 1 ? uniqueBagg[0] : JSONdata["Bagg"]) + "'><i class='soap-icon-suitcase less400' id='showbaggimg" + Ttotlen + "'></i><span class='baggfontcolr' id='showbagg" + Ttotlen + "'>" + (uniqueBagg.length == 1 ? uniqueBagg[0] : JSONdata["Bagg"]) + "</span></span>"

                        } else {
                            if ($('#hdtxt_trip').val() == "R") {
                                baggagedetails_grp += "<span class='vcenter mob-bagg cls18'  data-toggle='tooltip' data-placement='top' title='Baggage: " + (uniqueBagg.length == 1 ? uniqueBagg[0] : JSONdata["Bagg"]) + "'><i class='soap-icon-suitcase less400' id='showbaggimg" + Ttotlen + "'></i><span class='baggfontcolr' id='showbagg" + Ttotlen + "'>" + (uniqueBagg.length == 1 ? uniqueBagg[0] : JSONdata["Bagg"]) + "</span></span>";

                            }
                            else {
                                //        SAvail_build += "<span class='liDuration_span2' ></span>  ";
                            }
                        }
                    }
                  if ($('#hdtxt_trip').val() == "R") {
                        farerule_grp += "<span class='commontoggle clsavail_dtls positionrela' onclick=GetFareRule(" + Ttotlen + ");>Fare Rule</span>";
                    }
                    else {
                        farerule_grp += "<span class='commontoggle clsavail_dtls' onclick=GetFareRule(" + Ttotlen + ");>Fare Rule</span>";
                    }
                   

                    //   SAvail_build += " </div>";

                    ////////////////////////////////////////////////////////////////////
                    SAvail_build += " </div>";
                    SAvail_build += " </div>";
                    SAvail_build += " </div>"
                    SAvail_build += " </div>"
                    //For Mobile view only ***
                    //SAvail_build += "<div class='col-xs-12 col-mob-5 hidden-lg hidden-md hidden-sm clsmobavailothers'></div>"
                    //For Mobile view only End ***

                    //Buttor Area start ***
                    SAvail_build += " <div class='col-sm-2 col-xs-12 col-mob-5 farearea'>";
                    SAvail_build += "<div class='mk-trc mob-mk-trc' data-style='check' data-text='true' style='float: right'>"
                    SAvail_build += "<input id='lichechbox" + Ttotlen + "' type='checkbox' data-val='" + Ttotlen + "' value='" + First_inva + JSONdata["Inva"] + "' name='radio1' class='checkbox_availchk SelecCheckbox  checkcls'>";
                    SAvail_build += "<label for='lichechbox" + Ttotlen + "' class='lblcheckbox'><i class='checkbox_avail checkbox_availchk1'></i>"


                    if (JSONdata["cachetrueorfalse_7_arg"] == true) {

                        SAvail_build += "<br/><i class='checkbox_avail1 checkbox_availchk1 mcache' style='height: 1px;margin-top: -6px;background-color: green;'></i> </label>"
                    }
                    else {
                        SAvail_build += "<br/><i class='checkbox_avail1 checkbox_availchk1 mcache' style='height: 1px;margin-top: -6px;background-color: red;'></i> </label>";
                    }

                    SAvail_build += "</div>"
                    //Fare Popup Added by Saranraj on 20170513...
                    STax_build += "<table width='130px' >";
                    for (var tLen = 0; tLen < JSONdata["tax"].length; tLen++) {
                        JsonTax = "";
                        JsonTax = JSONdata["tax"][tLen];
                        STax_build += "<tr><td>" + JsonTax["COD"] + "</td><td>" + JsonTax["AMT"] + "</td></tr>";
                    }
                    if (JSONdata["Markup"].toString() != "0") {
                        STax_build += "<tr class='Markup' ><td>SC</td><td>" + JSONdata["Markup"] + "</td></tr>";
                    }
                    if (JSONdata["Service"].toString() != "0") {
                        STax_build += "<tr><td> Serv. Chg.</td><td>" + JSONdata["Service"] + "</td></tr>";
                    }
                    if (JSONdata["Incentive"].toString() != "0") {
                        STax_build += "<tr><td>Discount</td><td>" + JSONdata["Incentive"] + "</td></tr>";
                    }
                    /*Without Markup Fare added by Hariprasad on 06012015*/
                    STax_build += "<tr class='WTMFare' ><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + JSONdata["WTMFare"] + "</td></tr>";
                    /**/

                    /**/
                    STax_build += "<tr class='CGFare' ><td>Total Fare:</td><td style='font-weight: bolder; font-size: 14pt;'>" + JSONdata["GFare"] + "</td></tr>";
                    /**/

                    STax_build += "</table>";
                    //Fare Popup Added by Saranraj on 20170513 End...

                    if (JSONdata["Incentive"].toString() == "0") {
                        SAvail_build += "<div style='padding: 0px 0px 0px 0px !important; text-align:center;' id='GorssFareSpan2" + Ttotlen + "' data-WTMFare='" + con_WTMFare + "' data-grrFare='" + con_GFare + "' data-id='" + Ttotlen + "' class='liGrossfare_span' >";
                        SAvail_build += "<span class='price mob-price' style='white-space: nowrap;' id='lblGFare" + Ttotlen + "' onmousemove='CheckGrossFare(" + Ttotlen + ");' onmouseout='checkoutGrossFare(" + Ttotlen + ")'><span   class='currencyclass'>" + assignedcurrency + "</span>" + con_WTMFare + "</span>";
                        SAvail_build += "</div>";
                        SAvail_build += "<div id='taxtdiv" + Ttotlen + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div>";
                        SAvail_build += "<div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>";
                        SAvail_build += "<span id='span_GrossFare" + Ttotlen + "' style='display:none;'> " + con_WTMFare + " </span> ";
                    } else {
                        SAvail_build += "<div style='padding: 0px 0px 0px 0px !important; text-align:center;' id='GorssFareSpan2" + Ttotlen + "' data-WTMFare='" + lessincentivefare + "' data-grrFare='" + lessincentivefarewitgros + "' data-id='" + Ttotlen + "' class='liGrossfare_span' >"
                        SAvail_build += "<p class='Bestbuyfarewithmarkup' style='display:none;' ><strike>" + con_GFare + "</strike></p>"
                        SAvail_build += "<p class='Bestbuyfarewithoutmarkup' style='margin-left: 0px;' ><strike>" + con_WTMFare + "</strike></p>"
                        SAvail_build += "<span class='price mob-price' style='white-space: nowrap;' id='lblGFare" + Ttotlen + "' onmousemove='CheckGrossFare(" + Ttotlen + ");' onmouseout='checkoutGrossFare(" + Ttotlen + ")'><span   class='currencyclass'>" + assignedcurrency + "</span>" + lessincentivefare + "</span>"
                        SAvail_build += "</div>"
                        SAvail_build += "<div id='taxtdiv" + Ttotlen + "'class='Grossdivtooltip' ><div class='div-arrow div-green'></div>"
                        SAvail_build += "<div class='Goesstooltip_head'>1 Adult Gross Fare</div>" + STax_build + "</div>"
                        SAvail_build += "<span id='span_GrossFare" + Ttotlen + "' style='display:none;'> " + lessincentivefare + " </span> ";
                    }
                    SAvail_build += "<div class='second-row'>"

                    JSONdata["availsr_id"] = Ttotlen;
                    //second array assigned sri
                    // dataproperty_array.push(JSONdata["availsr_id"]);

                    dataproperty_array.push({
                        "datakey": Ttotlen,
                        "data-bestbuypopupvalues": '',
                        "data-hdnmarkupfare": JSONdata["Markup"],
                        "data-hdbestbuyy": best,
                        "data-hdAlterQueue": 'N',
                        "data-hdInva": First_inva + JSONdata["Inva"],
                        "data-hdtoken": con_ref,
                        "data-hdoffflg": JSONdata["offFlag"],
                        "data-platcarrierr": JSONdata["plt"],
                        "data-AirlineCategory": con_aircat,
                        "data-refundableforbest": Refu,
                        "data-oldtaxbreakup": JSONdata["taxbreakupold"],
                        "data-oldcommission": con_Comm,
                        "data-grpclssname": "group_" + deparrv_arrv + "_flightno_" + con_Fno.split(' ')[1] + "",
                        "data-clssname": con_cls,
                        "data-arg": JSONdata["bindside_6_arg"],
                        "data-grporgvcitymul": JSONdata["Org"],
                        "data-grpdesvcitymul": JSONdata["Des"],



                    });
                    SAvail_build += "<div class='action Travselect_" + Ttotlen + "' style='text-align: center;'>"
                    SAvail_build += "<input style='display:none;' type='button' id='btn_multiclas_" + Ttotlen + "' class='button btn-small mob-selectbutton selebtn MuLtiClas' value='GetFare' onclick='getmulticlasfare(" + Ttotlen + "," + LLP + ")' />"
                    SAvail_build += "<input style='display:none;margin-left: 20%;' type='button'  class='button btn-small mob-selectbutton selebtn MuLtiClas' id='btn_multiclasforconn_" + Ttotlen + "' onclick='getmulticlasfareforcon(" + flag_cnt + "," + Ttotlen + "," + JSONdata["multiclass"] + ")' value='Get Fare'>";
                    if ($('#hdtxt_trip').val() == "O" || $('#hdtxt_trip').val() == "Y" || ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "I")) {
                        //  var groupflg="group_"+con_WTMFare+"_flightno_"+con_Fno.split(' ')[1]+"";
                        // var tripifflg = 'Sgrd';
                        var tripfg = $('#hdtxt_trip').val();
                        SAvail_build += (best == "TRUE" ? "<a id='bestbuyclickbutton" + Ttotlen + "' class=\" button btn-small mob-selectbutton splselebtn \" onclick='javascript:getSplfare(" + Ttotlen + ")'><i class='fa fa-thumbs-o-up splfricon'></i> Select</a><a style='display:none' id='selectclickbutton" + Ttotlen + "' class=\" SpecialfareBlue button btn-small mob-selectbutton splselebtn \"  onclick='javascript:GetRowIndexSelect_old(" + Ttotlen + ",\"singletripifflg\",\"tripfg\")'>Select  </a>" : "<a id='selectclickbutton" + Ttotlen + "' class=\" button btn-small mob-selectbutton selebtn \"  onclick='javascript:GetRowIndexSelect_old(" + Ttotlen + ",\"singletripifflg\",\"tripfg\")'>Select</a>");

                        //   SAvail_build += (best == "TRUE" ? "<a id='bestbuyclickbutton" + Ttotlen + "' class=\" button btn-small mob-selectbutton splselebtn \" onclick='javascript:getSplfare(" + Ttotlen + ")'><i class='fa fa-thumbs-o-up splfricon'></i> Select</a><a style='display:none' id='selectclickbutton" + Ttotlen + "' class=\" SpecialfareBlue button btn-small mob-selectbutton splselebtn \"  onclick='javascript:GetRowIndexSelect_old(" + Ttotlen + ")>Select  </a>" : "<a id='selectclickbutton" + Ttotlen + "' class=\" button btn-small mob-selectbutton selebtn \"  onclick='javascript:GetRowIndexSelect_old(" + Ttotlen + ")'>Select</a>");
                    }
                    else {
                        if (best == "TRUE") {
                            SAvail_build += '<div class="rb-div roundTripFareRadio bestFareRadio">';
                            SAvail_build += '<input type="radio" class="rb" id="radioRTavail_' + Ttotlen + '" name="nameradio' + JSONdata["bindside_6_arg"] + '" onclick=' + '"' + "javascript:GetRowIndexSelect_old('" + Ttotlen + "','" + doubletripifflg + "','" + JSONdata["bindside_6_arg"] + "')" + '"' + '><label for="radioRTavail_' + Ttotlen + '" class="rblbl"></label>';
                            SAvail_build += '<div class="bullet"> <div class="line zero"></div> <div class="line one"></div> <div class="line two"></div> <div class="line three"></div> <div class="line four"></div> <div class="line five"></div> <div class="line six"></div> <div class="line seven"></div> </div>';
                            SAvail_build += '</div>';
                        }
                        else {
                            SAvail_build += '<div class="rb-div roundTripFareRadio">';
                            SAvail_build += '<input class="tgl tgl-flip" id="radioRTavail_' + Ttotlen + '" name="nameradio' + JSONdata["bindside_6_arg"] + '" onclick=' + '"' + "javascript:GetRowIndexSelect_old('" + Ttotlen + "','" + doubletripifflg + "','" + JSONdata["bindside_6_arg"] + "')" + '"' + '  name="nameradio' + JSONdata["bindside_6_arg"] + '" onclick=' + '"' + "javascript:GetRowIndexSelect_old('" + Ttotlen + "','" + doubletripifflg + "','" + JSONdata["bindside_6_arg"] + "')" + '"' + ' type="radio" style="height:0px;"> <label class="tgl-btn" data-tg-off="Select" data-tg-on="Selected" for="radioRTavail_' + Ttotlen + '"></label>';
                            SAvail_build += '</div>';
                        }
                    }

                    SAvail_build += "</div>"
                    SAvail_build += "</div>"
                    SAvail_build += " </div>"
                    SAvail_build += "<div class='col-xs-12 col-mob-5 clsMobRavailBotm'></div>"; //Added by Saranraj for Roundtrip Mobile view only...
                    connectingflight += "<div id='connectingFlghtdiv" + Ttotlen + "' class='connectFlighTooltip' style='display:none; float:left;'>"
                    connectingflight += "<div class='animated fadeInDown slideOutUp'><div id='condet_" + LLP + "' class='conting_" + flag_cnt + " conflighttab_" + Ttotlen + "'>" + MultiFlt + "</div></div></div>";
                    //avail load details new
                    SAvail_build += "</div>";
                    //avail #div 1-E
                    //  SAvail_build += "</article>";
                    //   SAvail_build += "</div>";

                    flag_cnt++;
                    LLP++;
                }
                //}
                //*****MUltiClassHidden VAlues*********//  JSONdata["StkType"]

                if (JSONdata["multiclass"] == "1" || JSONdata["multiclass"] == "2") {
                    resultvalue = Ttotlen + "SPLITRAV" + JSONdata["Cls"].split('-')[0] + "SPLITRAV" + JSONdata["mclassresult"] + "SPLITINVA" + JSONdata["mclassinva"] + "SPLITINVA" + JSONdata["Inva"] + "SplitClas" + JSONdata["Cls"].split('-')[1];
                    multiclasarr.push(resultvalue);
                    // multiclasconarr.push(resultvalue);
                }
                extrahidem_dataproperty_array.push({
                    "extrahide_datakey": Ttotlen,
                    "data-viaflight": JSONdata["nonstop"],
                    "data-hdMulticlassinva": '',
                    "data-OriginalClassgrossFare": JSONdata["GFare"],
                    "data-OriginalClassBaseFare": JSONdata["BFare"],
                    "data-OriginalClass": JSONdata["Cls"],
                    "data-hdAvailmulticlass": JSONdata["others"],
                    "data-hdflightstk": JSONdata["StkType"],
                    "data-hdmultiacess": JSONdata["multiclass"],
                    "data-hdFlightid": JSONdata["flightid"],
                    "data-hdconnFlightid": JSONdata["connflightid"]
                });
                SAvail_build += "<div class='extrahidem_" + Ttotlen + "' style='display:none'></div>";
                if (JSONdata["bestbuy"] == "true" || JSONdata["bestbuy"] == "TRUE") {
                    SAvail_build += "<input type='hidden' id='hdnsplfare" + Ttotlen + "' runat='server' value='" + JSONdata["bestbuy"] + "'/>";
                    splfarecount++;
                }
                else {
                    SAvail_build += "<input type='hidden' id='hdnsplfare" + Ttotlen + "' runat='server' value='" + JSONdata["bestbuy"] + "'/>";
                }
                SAvail_build += "<select style='display:none' id='hdMulticlassTransFare" + Ttotlen + "'></select>";
                //**************************************//
                segLen = 0;
                con_min = 0;
                airprtwat_con_min = 0;

                MultiFlt = "";
                via = "";
                STax_build = "";
                strFlightBuild = "";
                strsubFlightBuild = "";
                subFlightBuild = "";
                First_inva = "";
                //First_inva1 = "";
                var gropfincon = deparrv_arrv;
                
              
                baggagedetails_grp = baggagedetails_grp != null && baggagedetails_grp != "" ? baggagedetails_grp : "";
                getdetails = getdetails != null && getdetails != "" ? getdetails : "";
                seatconu = seatconu != null && seatconu != "" ? seatconu : "";
                refund_grp = refund_grp != null && refund_grp != "" ? refund_grp : "";
                basicfare = basicfare != null && basicfare != "" ? basicfare : "";
                multicls_grp = multicls_grp != null && multicls_grp != "" ? multicls_grp : "";
               

            
                //hide by srinath-deveoping array concept
                if ($('#hdtxt_trip').val() == "O" || $('#hdtxt_trip').val() == "Y" || ($('#hdtxt_trip').val() == "M")) {   // && $('body').data('segtype') == "I"
                    if (setofgroupedflight.length > 1) {
                        if (sr == 0) {
                            var totalfli = (setofgroupedflight.length) - 1;
                            SAvail_build += "<div class='commonclsgrp row row0 row-mob-0 dummymoreMAN" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + "' >";
                            SAvail_build += "<div class='HECKmore" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + " " + col_lg_2or3_new + " col-xs-4 fontcls' style='text-align: " + aligntext + ";'>" + multicls_grp + "</div>";
                            SAvail_build += "<div class='" + col_lg_1or2_new + " col-xs-2' style='text-decoration: underline;text-decoration-color: #28a5f5;'>" + getdetails + "</div>";
                            SAvail_build += "<div class='" + col_lg_1or2_new + " col-xs-2' style='padding-left: 0px;text-decoration: underline;text-decoration-color: #28a5f5;'>" + farerule_grp + "</div>";
                            SAvail_build += "<div class='" + col_lg_2or4_new + " col-xs-4  mobclswidth' style='padding:0px'>" + baggagedetails_grp + "<span class=''>" + seatconu + "</span>" + refund_grp + "</div>";

                            if ($('#hdtxt_trip').val() == "M" && $('body').data('segtype') == "D") {
                                SAvail_build += '<div class="b dummymore' + gropfincon + '_flightno_' + con_Fno.split(' ')[1] + ' col-sm-2 col-lg-6 col-xs-8  ssws" style="text-align: left;"><span class="dummymore' + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + '  moreflightcss" ><span class="groupcntflicont_' + gropfincon + '_flightno_' + con_Fno.split(' ')[1] + '" ><span class="symbol_' + gropfincon + '_flightno_' + con_Fno.split(' ')[1] + '">+</span>' + totalfli + '</span><span onclick=' + '"' + "javascript:ToggleConnectGroup('" + gropfincon + "','" + con_Fno.split(' ')[1] + "')" + '"' + '> More Fare Available</span></span></div>';
                            }
                            else {
                                SAvail_build += '<div class="b dummymore' + gropfincon + '_flightno_' + con_Fno.split(' ')[1] + ' col-sm-2 col-lg-2 col-xs-8  ssws" style="text-align: center;"><span class="dummymore' + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + '  moreflightcss" ><span class="groupcntflicont_' + gropfincon + '_flightno_' + con_Fno.split(' ')[1] + '" ><span class="symbol_' + gropfincon + '_flightno_' + con_Fno.split(' ')[1] + '">+</span>' + totalfli + '</span><span onclick=' + '"' + "javascript:ToggleConnectGroup('" + gropfincon + "','" + con_Fno.split(' ')[1] + "')" + '"' + '> More Fare Available</span></span></div>';
                            }
                            SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + basicfare + "</div>";


                            if (commissionbuild != null && commissionbuild != "") {
                                SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + commissionbuild + "</div>";
                            }
                            else {
                                SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + basicfare + "</div>";
                            }
                            SAvail_build += "</div>";
                            SAvail_build += "<div class='col-lg-12 col-xs-12' style='text-align: center;'>" + connectingflight + "</div>";

                            SAvail_build += "<div style='display:none;' class='col15 group_bind" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + "'>";
                        }
                        else {

                            SAvail_build += "<div class='commonclsgrp row row0 row-mob-0 dummymoreMAN" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + "' >";
                            SAvail_build += "<div class='HECKmore" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + " " + col_lg_2or3_new + " col-xs-4 fontcls' style='text-align: " + aligntext + ";'>" + multicls_grp + "</div>";
                            SAvail_build += "<div class='" + col_lg_1or2_new + " col-xs-2' style='text-decoration: underline;text-decoration-color: #28a5f5;'>" + getdetails + "</div>";
                            SAvail_build += "<div class='" + col_lg_1or2_new + " col-xs-2' style='padding-left: 0px;text-decoration: underline;text-decoration-color: #28a5f5;'>" + farerule_grp + "</div>";
                            SAvail_build += "<div class='" + col_lg_2or4_new + " col-xs-4  mobclswidth' style='padding:0px'>" + baggagedetails_grp + "<span class=''>" + seatconu + "</span>" + refund_grp + "</div>";
                            SAvail_build += '<div class="a dummymore' + gropfincon + '_flightno_' + con_Fno.split(' ')[1] + ' col-sm-2 col-lg-2 col-xs-8" style="text-align: center;"></div>';
                            SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + basicfare + "</div>";
                            if (commissionbuild != null && commissionbuild != "") {
                                SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + commissionbuild + "</div>";
                            }
                            else {
                                SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + basicfare + "</div>";
                            }
                            SAvail_build += "</div>";
                            SAvail_build += "<div class='col-lg-12 col-xs-12' style='text-align: center;'>" + connectingflight + "</div>";
                              SAvail_build += "</article>"
                            SAvail_build += "</div>"
                            if (sr == setofgroupedflight.length) {
                                SAvail_build += "</div>"
                                SAvail_build += "</article>"
                                SAvail_build += "</div>"
                            }
                        }
                    }
                    else {

                        SAvail_build += "<div class='commonclsgrp row row0 row-mob-0 dummymoreMAN" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + "' >";
                        SAvail_build += "<div class='HECKmore" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + " " + col_lg_2or3_new + " col-xs-4 fontcls' style='text-align: " + aligntext + ";'>" + multicls_grp + "</div>";
                        SAvail_build += "<div class='" + col_lg_1or2_new + " col-xs-2' style='text-decoration: underline;text-decoration-color: #28a5f5;'>" + getdetails + "</div>";
                        SAvail_build += "<div class='" + col_lg_1or2_new + " col-xs-2' style='padding-left: 0px;text-decoration: underline;text-decoration-color: #28a5f5;'>" + farerule_grp + "</div>";
                        SAvail_build += "<div class='col-lg-3 col-xs-4  mobclswidth' style='padding:0px'>" + baggagedetails_grp + "<span class=''>" + seatconu + "</span>" + refund_grp + "</div>";
                        SAvail_build += '<div class="s dummymore' + gropfincon + '_flightno_' + con_Fno.split(' ')[1] + ' col-sm-2 col-lg-2 col-xs-8" style="text-align: center;"></div>';
                        SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + basicfare + "</div>";
                        if (commissionbuild != null && commissionbuild != "") {
                            SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + commissionbuild + "</div>";
                        }
                        else {
                            SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + basicfare + "</div>";
                        }
                        SAvail_build += "</div>";
                        SAvail_build += "<div class='col-lg-12 col-xs-12' style='text-align: center;'>" + connectingflight + "</div>";


                        SAvail_build += "</article>"
                        SAvail_build += "</div>"
                    }
                }
                if ($('#hdtxt_trip').val() == "R") {
                    
                    if (setofgroupedflight.length > 1) {
                        if (sr == 0) {
                            var totalfli = (setofgroupedflight.length) - 1;
                            SAvail_build += "<div class='hegcls commonclsgrp row row0 row-mob-0 dummymoreMAN" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + "' >";
                            SAvail_build += "<div class='HECKmore" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + " " + col_lg_2or3_new + " col-xs-4 dispnonecls fontcls' style='text-align: " + aligntext + ";'>" + multicls_grp + "</div>";
                            SAvail_build += "<div class='" + col_lg_1or2_new + " col-xs-2 colxs50' style='text-decoration: underline;text-decoration-color: #28a5f5;'>" + getdetails + "</div>";
                            SAvail_build += "<div class='" + col_lg_1or2_new + " col-xs-2 colxs50' style='padding-left: 0px;text-decoration: underline;text-decoration-color: #28a5f5;'>" + farerule_grp + "</div>";
                            SAvail_build += "<div class='" + col_lg_2or4_new + " col-xs-4  mobclswidth  dispnonecls ' style='padding:0px'>" + baggagedetails_grp + "<span class=''>" + seatconu + "</span>" + refund_grp + "</div>";
                            SAvail_build += '<div class="dummymore' + gropfincon + '_flightno_' + con_Fno.split(' ')[1] + ' col-sm-2 col-lg-6 col-xs-8" style="text-align: left;"><span class="dummymore' + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + '  moreflightcss roundtripcls TES" ><span class="groupcntflicont_' + gropfincon + '_flightno_' + con_Fno.split(' ')[1] + '"><span class="symbol_' + gropfincon + '_flightno_' + con_Fno.split(' ')[1] + '">+</span>' + totalfli + '</span><span onclick=' + '"' + "javascript:ToggleConnectGroup('" + gropfincon + "','" + con_Fno.split(' ')[1] + "')" + '"' + '> More <span class="hidden-xs">Fare Available</span> </span></span></div>';
                            SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + basicfare + "</div>";
                            if (commissionbuild != null && commissionbuild != "") {
                                SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + commissionbuild + "</div>";
                            }
                            else {
                                SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + basicfare + "</div>";
                            }
                            SAvail_build += "</div>";
                            SAvail_build += "<div class='col-lg-12 col-xs-12' style='text-align: center;'>" + connectingflight + "</div>";

                            SAvail_build += "<div style='display:none;' class='col15 group_bind" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + "'>";
                        }
                        else {

                            SAvail_build += "<div class='hegcls commonclsgrp row row0 row-mob-0 dummymoreMAN" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + "' >";
                            SAvail_build += "<div class='HECKmore" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + " " + col_lg_2or3_new + " col-xs-4 dispnonecls fontcls' style='text-align: " + aligntext + ";'>" + multicls_grp + "</div>";
                            SAvail_build += "<div class='" + col_lg_1or2_new + " col-xs-2 colxs50' style='text-decoration: underline;text-decoration-color: #28a5f5;'>" + getdetails + "</div>";
                            SAvail_build += "<div class='" + col_lg_1or2_new + " col-xs-2 colxs50' style='padding-left: 0px;text-decoration: underline;text-decoration-color: #28a5f5;'>" + farerule_grp + "</div>";
                            SAvail_build += "<div class='" + col_lg_2or4_new + " col-xs-4  mobclswidth  dispnonecls ' style='padding:0px'>" + baggagedetails_grp + "<span class=''>" + seatconu + "</span>" + refund_grp + "</div>";
                            SAvail_build += '<div class="dummymore' + gropfincon + '_flightno_' + con_Fno.split(' ')[1] + ' col-sm-2 col-lg-6 col-xs-8" style="text-align: left;"></div>';
                            SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + basicfare + "</div>";
                            if (commissionbuild != null && commissionbuild != "") {
                                SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + commissionbuild + "</div>";
                            }
                            else {
                                SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + basicfare + "</div>";
                            }
                            SAvail_build += "</div>";
                            SAvail_build += "<div class='col-lg-12 col-xs-12' style='text-align: center;'>" + connectingflight + "</div>";




                            SAvail_build += "</article>"
                            SAvail_build += "</div>"
                            if (sr == setofgroupedflight.length) {
                                SAvail_build += "</div>"
                                SAvail_build += "</article>"
                                SAvail_build += "</div>"
                            }
                        }
                    }
                    else {

                        SAvail_build += "<div class='hegcls commonclsgrp row row0 row-mob-0 dummymoreMAN" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + "' >";
                        SAvail_build += "<div class='HECKmore" + gropfincon + "_flightno_" + con_Fno.split(' ')[1] + " " + col_lg_2or3_new + " col-xs-4 dispnonecls fontcls' style='text-align: " + aligntext + ";'>" + multicls_grp + "</div>";
                        SAvail_build += "<div class='" + col_lg_1or2_new + " col-xs-2 colxs50' style='text-decoration: underline;text-decoration-color: #28a5f5;'>" + getdetails + "</div>";
                        SAvail_build += "<div class='" + col_lg_1or2_new + " col-xs-2 colxs50' style='padding-left: 0px;text-decoration: underline;text-decoration-color: #28a5f5;'>" + farerule_grp + "</div>";
                        SAvail_build += "<div class='col-lg-5 col-xs-4  mobclswidth  dispnonecls ' style='padding:0px'>" + baggagedetails_grp + "<span class=''>" + seatconu + "</span>" + refund_grp + "</div>";
                        SAvail_build += '<div class="dummymore' + gropfincon + '_flightno_' + con_Fno.split(' ')[1] + ' col-sm-2 col-lg-6 col-xs-8" style="text-align: left;"></div>';
                        SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + basicfare + "</div>";
                        if (commissionbuild != null && commissionbuild != "") {
                            SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + commissionbuild + "</div>";
                        }
                        else {
                            SAvail_build += "<div class='col-lg-2 col-xs-4 hidden-xs' style='text-align: center;'>" + basicfare + "</div>";
                        }
                        SAvail_build += "</div>";
                        SAvail_build += "<div class='col-lg-12 col-xs-12' style='text-align: center;'>" + connectingflight + "</div>";


                        SAvail_build += "</article>"
                        SAvail_build += "</div>"
                    }
                }

               
                }
                EndArrivetime = new Array();
                EndtoolArr = new Array();
                NoofStops = new Array();
                refund = new Array();
                bestbuy = new Array();
                sameflighturl = new Array();
                Sameflight = "Same";
                PrevDes = "";
                Riti = "0"
                Ttotlen++;
                fltcnt++;
        }
            //for loop end--srinath
            obj1.count = Ttotlen;
            obj1.setcount = fltcnt;
        }
        else if (arg[5] != "" && arg[5] != null) {
            if (noflight != null && noflight != "") {
                noflight = noflight + arg[5] + ",";
            }
            else {
                noflight = arg[5] + ",";
            }

        }
        else {
            $('#FlightError').innerHTML = arg[0]; 
        }
    } catch (e) {
        $("#availset_1").html("");
        $("#availresponse").css("display", "none");

        showError1("Unable to get availability,Please contact customer care-(#05) ");
        $.unblockUI();
        console.log(e.message + "problem in availability loading");
        return false;
    }


    return SAvail_build + "Av@i" + appendavail_side;
}
function showflightpoup(idvalue) {

    var getvalue = $(idvalue).attr("data-popup");
    var Poupdetails = getvalue.split("SltP");
    var connflightpop = "";
    connflightpop += "<table  width='100%'  class='connflidetails_" + Poupdetails[0] + "'>";
    connflightpop += "<tr><td width='5%'></td><td>Airline Name</td><td>:</td><td>" + airlinename(Poupdetails[1]) + "</td></tr>";
    connflightpop += "<tr><td width='5%'></td><td>Departure Airport</td> <td>:</td><td>" + Poupdetails[2] + "</td></tr>";
    connflightpop += "<tr><td width='5%'></td><td>Arrival Airport</td><td>:</td><td>" + Poupdetails[3] + "</td></tr>";
    connflightpop += "<tr><td width='5%'></td><td>Operated by</td><td>:</td><td>" + Poupdetails[4] + "</td></tr>";
    connflightpop += "<tr><td width='5%'></td><td>Depa.Terminal</td><td>:</td><td>" + Poupdetails[5] + "</td></tr>";
    connflightpop += "<tr><td width='5%'></td><td>Arr.Terminal</td><td>:</td><td>" + Poupdetails[6] + "</td></tr>";
    connflightpop += "<tr><td width='5%'></td><td>Baggage</td><td>:</td><td><span id='baggmul'>" + Poupdetails[7] + "</span> </td></tr>";
    connflightpop += "<tr><td width='5%'></td><td>Multiclass</td><td>:</td><td>" + Poupdetails[8] + " </td></tr>";
    connflightpop += "<tr><td width='5%'></td><td>Stock Type </td><td>:</td><td>" + Poupdetails[9] + " </td></tr>";
    connflightpop += "<tr><td width='5%'></td><td>Via </td><td>:</td><td>" + (Poupdetails[10] == "" ? "N/A" : Poupdetails[10]) + " </td></tr>";
    connflightpop += "</table>";

    var _eoffer = $('#' + Poupdetails[11] + Poupdetails[0]);
    _eoffer.attr("rel", connflightpop);
    _eoffer.tooltipster('content', connflightpop);
}   


var Filtered = [];
function airlinename(ID) {
        
    try {
        if (ID == "HR") {
        }
        Filtered = jQuery.grep(Airdata, function (a) { return a['id'] == ID; });
        return Filtered[0].value == "undefined" ? "" : Filtered[0].value;
    }
    catch (ex) {
    }
}


function Checkbaggfunction(Baggage) {
    Baggage = Baggage.replace("N/A", "RaJEsH");
    var response = false;
    if (Baggage.indexOf('/') > 0) {
        for (var i = 0; i < Baggage.split('/').length - 1; i++) {
            if (Baggage.split('/')[i].replace(/\s/g, "") != Baggage.split('/')[i + 1].replace(/\s/g, "")) {
                response = true;
            }
        }
    }
    return response;
}
function closemsg() {
    $('#listingsloading').css("display", "none");
    $('#errload')[0].innerHTML = "";
    noflight = "";
}
function TaxTable(Obj) {
    $('#taxtdiv' + Obj).show();

}
function TaxtTablenone(obn) {
    $('#taxtdiv' + obn).hide();
}
var rlen = 0;
function rowscountt() {
    var Lirows = document.getElementById('AvailResponse_ul').getElementsByTagName('li');
    var totrows = Lirows.length - rlen;
    var totf = "<span class='FlightCount' title='No. of Flights Available' id='noOfstops'>" + totrows + " </span> <span>Flights</span>";
    $('#totFlights').innerHTML = totf;
    $('#totFlights').html(totf);
}
function rowscounttt() {
    var Lirows = document.getElementById('AvailResponse_ul').getElementsByTagName('li');
    var totrows = rlen;
    var totf = "<span class='FlightCount' title='No. of Flights Available' id='noOfstops'>" + totrows + " </span> <span>Flights</span>";
    $('#totFlights').innerHTML = totf;
    $('#totFlights').html(totf);
}
function GetFareRule(val) {

    var idss = val;
    var selectedObj = $.grep(dataproperty_array, function (n, i) {
        return n.datakey == idss;
    });

    var Triptype = $("#hdtxt_trip").val();

    AirCategory = selectedObj[0]["data-AirlineCategory"];// $('.Travselect_' + val)[0].dataset.airlinecategory;// document.getElementById('AirlineCategory' + val).value;
    AirCategory11 = selectedObj[0]["data-hdInva"];//$('.Travselect_' + val)[0].dataset.hdinva; // document.getElementById('hdInva' + val).value;
    var Flightno = document.getElementById('FlightID' + val).innerHTML;
    var sfcthread = document.getElementById("hdnsfcthread").value;
    var Paxcount = $("#hdtxt_adultcount").val() + "|" + $("#hdtxt_childcount").val() + "|" + $("#hdtxt_infantcount").val();
    var Pnrdetails = "";
    var Cabin = $("#ddlclass").val();
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loadingimg + '" style="background-color:#fff; border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });
    $.ajax({
        type: "POST", 		//GET or POST or PUT or DELETE verb
        url: getfarerule, 		// Location of the service
        data: '{FlightFareRuleid: "' + Flightno + '", FareRuleAvailstring: "' + AirCategory11 + '" ,Triptype:"' + Triptype + '",Paxcount:"' + Paxcount + '",Pnrdetails:"' + Pnrdetails + '",Cabin:"' + Cabin + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (json) {//On Successful service call
            var result = json.Result;
            $.unblockUI();
            FareRuleFunctionBack(result, Flightno);

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
function FareRuleFunctionBack(arg, Flightno) {
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
        if (category[6] == "FSC" || category[6] == "D3" || category[6] == "1A") {
            //var Air_json = JSON.parse(arg[1]);
            //strFareRuleBulid += "<table>";
            //for (var jsoncount = 0; jsoncount < Air_json["FARERULE"].length; jsoncount++) {
            //    strFareRuleBulid += "<tr><td width=10%>" + Air_json["FARERULE"][jsoncount]["RULE"].replace("F.", "") + "</td><td width=25%><pre>  " + Air_json["FARERULE"][jsoncount]["TEXT"] + "</pre> </td></tr>";
            //}
            //strFareRuleBulid += "</table>";
            //Ititle = "Fare Rule " + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
            //Isubtitle = "";
            //IContent = strFareRuleBulid;
            //Ifullopt = false;
            //Itemp1 = "";
            //Itemp2 = "";
            //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);

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

               // str += '<div class="clsSpanText"><span class="clsHeadNote">Note : </span>' + Resultss["IMPORTANT_NOTE"].NOTE + '</div>';
            }


            Ititle = "Fare Rule " + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
            Isubtitle = "";
            IContent = str;
            Ifullopt = false;
            Itemp1 = "";
            Itemp2 = "";
            showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);


            //if (comparefarepopflg == "CF") {
            //    $(".cmnnewfarecls").html("");
            //    $("#Farerulediv").html("<span class='farecompfarerulespan' onclick='closenewfarerulefn()' style='float:right'><img src='../Img/close-icn.png'></span><pre>" + str + "</pre>");//_" + cntdiv
            //    $(".cmclsloaderfare").hide();
            //    $("#Farerulediv").slideToggle();//_" + cntdiv
            //} else {
            //    $('#dvinnerFR').html("<pre>" + str + "</pre>")
            //    $("#modal-fr").modal({
            //        backdrop: 'static',
            //        keyboard: false
            //    });
            //}


        }
        else if (category[0] == "LCC") {
            Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
            Isubtitle = "";
            IContent = "<pre>" + arg[1] + "</pre>";
            Ifullopt = false;
            Itemp1 = "";
            Itemp2 = "";
            showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
        }
        else if (category[6] == "TZ" || category[6] == "6E") {
            var Air_json = JSON.parse(arg[1]);
            strFareRuleBulid += "<table>";
            for (var jsoncount = 0; jsoncount < Air_json["FARERULE"].length; jsoncount++) {
                strFareRuleBulid += "<tr><td width=25%><pre style='white-space: pre-wrap;'>  " + Air_json["FARERULE"][jsoncount]["TEXT"] + "</pre> </td></tr>";
            }
            strFareRuleBulid += "</table>";
            Ititle = "Fare Rule " + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
            Isubtitle = "";
            IContent = strFareRuleBulid;
            Ifullopt = false;
            Itemp1 = "";
            Itemp2 = "";
            showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
        }
        else if (category[5] == arg[2]) {
            Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
            Isubtitle = "";
            IContent = "<pre>" + arg[1] + "</pre>";
            Ifullopt = false;
            Itemp1 = "";
            Itemp2 = "";
            showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
        }

        else if (category[6] == "UAI") {
            if (category[5] == arg[2]) {
                Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
                Isubtitle = "";
                IContent = "<pre>" + arg[1] + "</pre>";
                Ifullopt = false;
                Itemp1 = "";
                Itemp2 = "";
                showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            }
            else {
                //var Air_json = JSON.parse(arg[1]);
                //strFareRuleBulid += "<table style='width:100%;'>";
                //for (var jsoncount = 0; jsoncount < Air_json["FARERULE"].length; jsoncount++) {
                //    strFareRuleBulid += "<tr><td width=10%>" + Air_json["FARERULE"][jsoncount]["RULE"].replace("F.", "") + "</td><td width=25%><pre style='width: 500px;' >  " + Air_json["FARERULE"][jsoncount]["TEXT"] + "</pre> </td></tr>";
                //}
                //strFareRuleBulid += "</table>";
                //Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
                //Isubtitle = "";
                //IContent = strFareRuleBulid;
                //Ifullopt = false;
                //Itemp1 = "";
                //Itemp2 = "";
                //showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);

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
                Ititle = "Fare Rule " + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
                Isubtitle = "";
                IContent = str;
                Ifullopt = false;
                Itemp1 = "";
                Itemp2 = "";
                showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            }

        }
        else if (category[6] == "LCC") {
            Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
            Isubtitle = "";
            IContent = "<pre>" + arg[1] + "</pre>";
            Ifullopt = false;
            Itemp1 = "";
            Itemp2 = "";
            showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);

        }
        else if (category[6] == "LCCG9" || category[6] == "G9" || category[6] == "FZ") {
            Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
            Isubtitle = "";
            IContent = "<pre>" + arg[1] + "</pre>";
            Ifullopt = false;
            Itemp1 = "";
            Itemp2 = "";
            showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);


        }
        else if (category[6] == arg[2]) {
            Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
            Isubtitle = "";
            IContent = "<pre>" + arg[1] + "</pre>";
            Ifullopt = false;
            Itemp1 = "";
            Itemp2 = "";
            showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
        }
        else if (category[6] == "OSC" || category[6] == "DXB") {
            var Air_json = JSON.parse(arg[1]);
            strFareRuleBulid += "<table>";
            for (var jsoncount = 0; jsoncount < Air_json["FARERULE"].length; jsoncount++) {
                strFareRuleBulid += "<tr><td width=10%>" + Air_json["FARERULE"][jsoncount]["RULE"].replace("F.", "") + "</td><td width=25%><pre>  " + Air_json["FARERULE"][jsoncount]["TEXT"] + "</pre> </td></tr>";
            }
            strFareRuleBulid += "</table>";
            Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
            Isubtitle = "";
            IContent = strFareRuleBulid;
            Ifullopt = false;
            Itemp1 = "";
            Itemp2 = "";
            showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);

        }
        else if (category[6] == "RST") {
            if (category[5] == arg[2]) {
                Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
                Isubtitle = "";
                IContent = "<pre>" + arg[1] + "</pre>";
                Ifullopt = false;
                Itemp1 = "";
                Itemp2 = "";
                showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            }
            else {
                var Air_json = JSON.parse(arg[1]);
                strFareRuleBulid += "<table style='width:100%;'>";
                for (var jsoncount = 0; jsoncount < Air_json["FARERULE"].length; jsoncount++) {
                    strFareRuleBulid += "<tr><td width=10%>" + Air_json["FARERULE"][jsoncount]["RULE"].replace("F.", "") + "</td><td width=25%><pre style='width: 500px;' >  " + Air_json["FARERULE"][jsoncount]["TEXT"] + "</pre> </td></tr>";
                }
                Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
                Isubtitle = "";
                IContent = strFareRuleBulid;
                Ifullopt = false;
                Itemp1 = "";
                Itemp2 = "";
                showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            }
        }
        else if (category[6] == "IRST") {
            if (category[5] == arg[2]) {
                Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + "</span>";
                Isubtitle = "";
                IContent = "<pre>" + arg[1] + "</pre>";
                Ifullopt = false;
                Itemp1 = "";
                Itemp2 = "";
                showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            }
            else {
                var Air_json = JSON.parse(arg[1]);
                strFareRuleBulid += "<table style='width:100%;'>";
                for (var jsoncount = 0; jsoncount < Air_json["FARERULE"].length; jsoncount++) {
                    strFareRuleBulid += "<tr><td width=10%>" + Air_json["FARERULE"][jsoncount]["RULE"].replace("F.", "") + "</td><td width=25%><pre style='width: 500px;' >  " + Air_json["FARERULE"][jsoncount]["TEXT"] + "</pre> </td></tr>";
                }
                strFareRuleBulid += "</table>";
                Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
                Isubtitle = "";
                IContent = strFareRuleBulid;
                Ifullopt = false;
                Itemp1 = "";
                Itemp2 = "";
                showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
            }
        }
        else {
            Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
            Isubtitle = "";
            IContent = "<pre>" + arg[0] + "</pre>";
            Ifullopt = false;
            Itemp1 = "";
            Itemp2 = "";
            showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
        }

    }
    else {

        var category = AirCategory.split("SpLitPResna");
        Ititle = "Fare Rule" + "<span>( " + category[1] + " - " + category2[1] + ") " + Flightno.substring(0, 2) + " </span>";
        Isubtitle = "";
        IContent = "<pre>" + arg[0] + "</pre>";
        Ifullopt = false;
        Itemp1 = "";
        Itemp2 = "";
        showpopuplogin(Ititle, Isubtitle, IContent, Ifullopt, Itemp1, Itemp2);
    }
}


function airportwaitingcalc(departuredate_time, arrivaldate_time) {


    airprtwat_con_min = (moment(arrivaldate_time)) - (moment(departuredate_time));
    var duration = moment.duration(airprtwat_con_min);
    var hours = duration.asHours();
    var minutes = Math.floor(airprtwat_con_min / 60000);
    airprtwat_con_Dur = Math.floor((minutes % (60 * 60)) / 60);
    airprtwat_con_Durmin = Math.ceil((minutes % (60 * 60)) % 60);
    if (airprtwat_con_Durmin.value < 10) {
        airprtwat_con_Durmin = "0" + airprtwat_con_Durmin;
    }
    var totalwat = airprtwat_con_Dur + 'h :' + airprtwat_con_Durmin + 'm';
    return totalwat
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

function layouttimebind(layoutfiled, stopscnt, trip, Ttotlen, nextarraival) {

    var buildlaydes = "";
    var cont =0 
    var width =0
    var wid = 0;
  
    if (trip == "M") {                         //for multi city seperate build condition write by sri

        if (trip == "M") {
            cont = (layoutfiled.length);

            if (layoutfiled.length == 1) {
                cont = (layoutfiled.length);
                stopscnt = 0;
               // width = Number(stopscnt) + Number(stopscnt) + 1;
                width = Number(Number(layoutfiled.length) - 1) + Number(Number(layoutfiled.length) - 1) + 1;
            }
            else {
                //cont = Number(stopscnt) + 1;
                stopscnt = Number(Number(layoutfiled.length) - 1);
                width = Number(Number(stopscnt) + 1);
            }
        }
        else {
            cont = Number(Number(layoutfiled.length) - 1) + 1;
            //width = Number(stopscnt) + Number(stopscnt) + 1;
            width = Number(Number(layoutfiled.length) - 1) + Number(Number(layoutfiled.length) - 1) + 1;
            wid = 99 / Number(width);

        }
        //var buildlaydes = "";
        //var cont = Number(stopscnt) + 1;

        var wid = 99 / Number(width);


        for (var i = 0; i < cont; i++) {

            var Deps = layoutfiled[i].Dep.split(' ');
            var Dep = Deps[3];
            var Arrs = layoutfiled[i].Arr.split(' ');
            var Arr = Arrs[3];
            if (i == 0) {

                buildlaydes += '<div class="bordecls " style="width:' + wid + '%;float:left">';
                buildlaydes += '<span class="rtsarrivcity orglaycls" style="font-weight: 600 !important;font-size: 12px !important;">' + layoutfiled[i].Org + '</span>';
                if (trip == "M") {
                    var layouttime = "";
                }
                else {
                    var layouttime = airportwaitingcalc(layoutfiled[i].Dep, layoutfiled[i].Arr);
                }
                buildlaydes += '<span class="clsavailcityicon doticonlaycls" style="width: 10px;height: 10px;top: -6px !important;background-color: #1da1f9;"></span>';
                buildlaydes += '<span class="clsavailarrow "><i class="fa fa-long-arrow-right"></i></span>';
                buildlaydes += '<span class="bold dark clsavailTimes hidden-xs" style="margin-left: 47%;font-size: 10px;font-weight: 400 !important;" id="1" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="layouttime:' + layouttime + '">' + layouttime + '</span>';
                buildlaydes += '<br><span class="bold dark clsavailTimes flitimelay fontstylmul" id="span_depature' + Ttotlen + '"  data-toggle="tooltip" data-placement="bottom" title="" data-original-title="Dep:' + layoutfiled[i].Dep + ' ">' + Dep + '</span>';
                buildlaydes += '</div>';
            }

            else {
                if (trip != "M") {
                    buildlaydes += '<div class="bordecls" style="width:' + wid + '%;float:left">';
                    buildlaydes += '<span class="rtsarrivcity orglaycls" style="">' + layoutfiled[i].Org + '</span>';
                    if (trip == "M") {
                        var layouttime = "";
                    }
                    else {
                        var layouttime = airportwaitingcalc(layoutfiled[i].Dep, layoutfiled[i].Arr);
                    }
                    buildlaydes += '<span class="clsavailcityicon doticonlaycls" style="border-color: #FF5722 !important;background-color: #FF5722 !important;" ></span>';
                    buildlaydes += '<span class="clsavailarrow "><i class="fa fa-long-arrow-right"></i></span>';
                    buildlaydes += '<span class="bold dark clsavailTimes hidden-xs" style="margin-left: 47%;font-size: 10px;font-weight: 400 !important;" id="1" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="DEP: ">' + layouttime + '</span>';
                    buildlaydes += '<br><span class="bold dark clsavailTimes flitimelay fontstylmul" id="span_depature11" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="DEP:' + layoutfiled[i].Dep + '" style="font-size: 10px !important;font-weight: 400 !important;">' + Dep + '</span>';
                    buildlaydes += '</div>';
                }
            }

            if (i == (cont - 1)) {
                buildlaydes += '<div class="bordecls" style="width:auto;float:left">';
                buildlaydes += '<span class="rtsarrivcity orglaycls finaldescls mobdestmargin" style="font-weight: 600 !important;font-size: 12px !important;">' + layoutfiled[i].Des + '</span>';
                buildlaydes += '<span class="clsavailcityicon doticonlaycls mobdestdot" style="width: 10px;height: 10px;top: -6px !important;background-color: #1da1f9;" ></span>';
                buildlaydes += '<span class="clsavailarrow"><i class="fa fa-long-arrow-right"></i></span>';
                buildlaydes += '<br><span class="bold dark clsavailTimes flitimelay finaltimecls mobdestnatime fontstylmul" id="span_Arrival' + Ttotlen + '" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="Arr:' + layoutfiled[i].Arr + ' ">' + Arr + '</span>';
                buildlaydes += '</div>';
            }

            else {
                if (trip == "M")    {

                    if (trip == "M") {
                        var layouttime = "";
                    }
                    else {
                        var layouttime = airportwaitingcalc(layoutfiled[i].Arr, layoutfiled[i].Dep);
                    }
                    buildlaydes += '<div class="bordecls " style="width:' + wid + '%;float:left;">';
                    buildlaydes += '<span class="rtsarrivcity orglaycls " >' + layoutfiled[i].Des + '</span>';

                    buildlaydes += '<span class="rtsarrivcity laytime hidden-xs" style="">' + layouttime + '</span>';
                    buildlaydes += '<span class="clsavailcityicon doticonlaycls" style="background-color: #ccc !important;border-color: #ccc !important;" ></span>';
                    buildlaydes += '<span class="clsavailarrow"><i class="fa fa-long-arrow-right"></i></span>';
                    buildlaydes += '<br><span class="bold dark clsavailTimes flitimelay fontstylmul" id="span_depature11" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="DEP:' + layoutfiled[i].Arr + '"style="font-size: 10px !important;font-weight: 400 !important;">' + Arr + '</span>';
                    buildlaydes += '</div>';
                }
                else {
                    if (trip == "M") {
                        var layouttime = "";
                    }
                    else {
                        var layouttime = airportwaitingcalc(layoutfiled[i].Arr, layoutfiled[i + 1].Dep);
                    }

                    buildlaydes += '<div class="bordecls multicitycls" style="width:' + wid + '%;float:left;border-top: 1px dashed #FF5722;">';
                    buildlaydes += '<span class="rtsarrivcity orglaycls " >' + layoutfiled[i].Des + '</span>';
                    buildlaydes += '<span class="rtsarrivcity laytime hidden-xs" style="">' + layouttime + '</span>';
                    buildlaydes += '<span class="clsavailcityicon doticonlaycls" style="border-color: #FF5722 !important;background-color: #FF5722 !important;" ></span>';
                    buildlaydes += '<span class="clsavailarrow"><i class="fa fa-long-arrow-right"></i></span>';
                    buildlaydes += '<br><span class="bold dark clsavailTimes flitimelay fontstylmul" id="span_depature11" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="DEP:' + layoutfiled[i].Arr + ' "style="font-size: 10px !important;font-weight: 400 !important;">' + Arr + '</span>';
                    buildlaydes += '</div>';
                }


            }
        }
        //buildlaydes += "</div>";
      
    }   

    else {   //for roundtrip and oneway seperate build condition write by sri


        if (trip == "Y" || trip == "R") {
            cont = (layoutfiled.length);
            if (layoutfiled.length == 1) {
                cont = (layoutfiled.length);
                stopscnt = 0;
                //width = Number(stopscnt) + Number(stopscnt) + 1;
                width = Number(Number(layoutfiled.length) - 1) + Number(Number(layoutfiled.length) - 1) + 1;
            }
            else {
                stopscnt = stopscnt;
                width = Number(layoutfiled.length);
            }
        }
        else {
            cont = layoutfiled.length;
         //   width = Number(stopscnt) + Number(stopscnt) + 1;
            width = Number(Number(layoutfiled.length) - 1) + Number(Number(layoutfiled.length) - 1) + 1;
            wid = 99 / Number(width);
            }
        
        var wid = 99 / Number(width);
        for (var i = 0; i < cont; i++) {

            var Deps = layoutfiled[i].Dep.split(' ');
            var Dep = Deps[3];
            var Arrs = layoutfiled[i].Arr.split(' ');
            var Arr = Arrs[3];
            if (i == 0) {

                buildlaydes += '<div class="bordecls 1" style="width:' + wid + '%;float:left">';
                buildlaydes += '<span class="rtsarrivcity orglaycls" style="font-weight: 600 !important;font-size: 12px !important;">' + layoutfiled[i].Org + '</span>';
                if (trip == "Y" || trip == "R") {
                    if (layoutfiled.length==1) {
                        var flymind = (layoutfiled[i].fly) % (60 * 60);
                        var flyminutes = Math.floor(flymind / 60);
                        var flysecd = flymind % 60;
                        var flysecond = Math.ceil(flysecd);
                        if (flysecond.value < 10) {
                            flysecond = "0" + flysecond;
                        }
                        var layouttime = flyminutes + "h :" + flysecond + " m";
                    }
                    else {
                        var layouttime = "";
                    }
                   
                }
                else {
                    var flymind = (layoutfiled[i].fly) % (60 * 60);
                var flyminutes = Math.floor(flymind / 60);
                  var  flysecd = flymind % 60;
                 var   flysecond = Math.ceil(flysecd);
                    if (flysecond.value < 10) {
                        flysecond = "0" + flysecond;
                    }
                    var layouttime = flyminutes + "h :" + flysecond + " m";
                    buildlaydes += '<span class="rtsarrivcity laytime hidden-xs" style="">'+ layoutfiled[i].Air + '</span>';
                    //buildlaydes += '<img  alt="" class="FlightTip FlightTipimg mobroundtripclsimg" style="width: 19px; height: 14px;position: absolute;    right: 38px;" id="oneway" src=\'' + layoutfiled[i].Conurl + '\'>'
                        

                }
                buildlaydes += '<span class="clsavailcityicon doticonlaycls" style="width: 10px;height: 10px;top: -6px !important;background-color: #1da1f9;"></span>';

                if (wid == 99 && trip=="R") {
                    buildlaydes += '<span class="clsavailarrow " style="left: 45%;"><i class="fa fa-long-arrow-right"></i></span>';
                }
                else {
                    buildlaydes += '<span class="clsavailarrow "><i class="fa fa-long-arrow-right"></i></span>';
                }
             
               
                buildlaydes += '<span class="bold dark clsavailTimes hidden-xs" style="margin-left: 47%;font-size: 10px;font-weight: 400 !important;" id="1" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="Flying Time:' + layouttime + '">' + layouttime + '</span>';
                buildlaydes += '<br><span class="bold dark clsavailTimes flitimelay" id="span_depature' + Ttotlen + '" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="DEP:' + layoutfiled[i].Dep + ' ">' + Dep + '</span>';
                buildlaydes += '</div>';
            }

            else {
                if (trip != "Y" && trip != "R") {
                    buildlaydes += '<div class="bordecls 2" style="width:' + wid + '%;float:left">';
                    buildlaydes += '<span class="rtsarrivcity orglaycls" style="">' + layoutfiled[i].Org + '</span>';
                    if (trip == "Y") {
                        var layouttime = "";
                    }
                    else {
                        var flymind = (layoutfiled[i].fly) % (60 * 60);
                        var flyminutes = Math.floor(flymind / 60);
                        var flysecd = flymind % 60;
                        var flysecond = Math.ceil(flysecd);
                        if (flysecond.value < 10) {
                            flysecond = "0" + flysecond;
                        }
                        var layouttime = flyminutes + "h :" + flysecond + " m";
                        buildlaydes += '<span class="rtsarrivcity laytime hidden-xs" style="">'+ layoutfiled[i].Air + '</span>';
                            //<img  alt="" class="FlightTip FlightTipimg mobroundtripclsimg" style="width: 19px; height: 14px;position: absolute;    right: 38px;" id="oneway" src=\'' + layoutfiled[i].Conurl + '\'>' + layoutfiled[i].Air + '</span>';

                    }

                    buildlaydes += '<span class="clsavailcityicon doticonlaycls" style="border-color: #FF5722 !important;background-color: #FF5722 !important;" ></span>';
                    buildlaydes += '<span class="bold dark clsavailTimes hidden-xs" style="margin-left: 47%;font-size: 10px;font-weight: 400 !important;" id="1" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="Flying Time:' + layouttime + '">' + layouttime + '</span>';
                    buildlaydes += '<br><span class="bold dark clsavailTimes flitimelay" id="span_depature11" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="DEP:' + layoutfiled[i].Dep + ' " style="font-size: 10px !important;font-weight: 400 !important;">' + Dep + '</span>';
                    buildlaydes += '</div>';
                }
            }

            if (i == (cont - 1)) {
                buildlaydes += '<div class="bordecls 3" style="width:auto;float:left">';
                buildlaydes += '<span class="rtsarrivcity orglaycls finaldescls mobdestmargin" style="font-weight: 600 !important;font-size: 12px !important;">' + layoutfiled[i].Des + '</span>';
                buildlaydes += '<span class="clsavailcityicon doticonlaycls mobdestdot" style="width: 10px;height: 10px;top: -6px !important;background-color: #1da1f9;" ></span>';
                buildlaydes += '<div class="finaltimecls" style="position: relative;width: 100%;">';
                if (trip == "R") {
                    buildlaydes += '<br><span class="bold dark clsavailTimes flitimelay finaltimecls  mobdestnatime" id="span_Arrival' + Ttotlen + '" style="float: right;right: 0;" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="Arr:' + layoutfiled[i].Arr + ' ">' + Arr + '</span>';
                   }
                else {
                    if (nextarraival != null && nextarraival != "") {
                        buildlaydes += '<span class="hidden-xs">' + nextarraival + '</span>';
                    }
                    buildlaydes += '<br><span class="bold dark clsavailTimes flitimelay finaltimecls mobdestnatime" id="span_Arrival' + Ttotlen + '" style="float: right;right: 0;" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="Arr:' + layoutfiled[i].Arr + ' ">' + Arr + '</span>';
                    }
                buildlaydes += '</div>';
                buildlaydes += '</div>';
            }

            else {
                if (trip == "Y" || trip == "R") {

                    if (trip == "Y" || trip == "R") {
                        var layouttime = "";
                    }
                    else {
                        var layouttime = airportwaitingcalc(layoutfiled[i].Arr, layoutfiled[i].Dep);
                    }
                    buildlaydes += '<div class="bordecls 4" style="width:' + wid + '%;float:left;">';
                    buildlaydes += '<span class="rtsarrivcity orglaycls hidden-xs" >' + layoutfiled[i].Des + '</span>';
                    buildlaydes += '<span class="rtsarrivcity laytime hidden-xs" style="">' + layouttime + '</span>';
                    buildlaydes += '<span class="clsavailcityicon doticonlaycls" style="background-color: #ccc !important;border-color: #ccc !important;" ></span>';
                    buildlaydes += '<br><span class="bold dark clsavailTimes flitimelay hidden-xs" id="span_depature11" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="Arr:' + layoutfiled[i].Arr + ' "style="font-size: 10px !important;font-weight: 400 !important;">' + Arr + '</span>';
                    buildlaydes += '</div>';
                }
                else {
                    if (trip == "Y" || trip == "R") {
                        var layouttime = "";
                    }
                    else {
                        var layouttime = airportwaitingcalc(layoutfiled[i].Arr, layoutfiled[i + 1].Dep);
                    }

                    buildlaydes += '<div class="bordecls 5" style="width:' + wid + '%;float:left;border-top: 1px dashed #FF5722;">';
                    buildlaydes += '<span class="rtsarrivcity orglaycls " >' + layoutfiled[i].Des + '</span>';

                    buildlaydes += '<span class="rtsarrivcity laytime hidden-xs" style="" data-toggle="tooltip" data-original-title="Transit Time:' + layouttime + '">' + layouttime + '</span>';
                    buildlaydes += '<span class="clsavailcityicon doticonlaycls" style="border-color: #FF5722 !important;background-color: #FF5722 !important;" ></span>';
                    buildlaydes += '<br><span class="bold dark clsavailTimes flitimelay" id="span_depature11" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="Arr:' + layoutfiled[i].Arr + ' "style="font-size: 10px !important;font-weight: 400 !important;">' + Arr + '</span>';
                    buildlaydes += '</div>';
                }   


            }
        }
        return buildlaydes;
    }



    return buildlaydes;

}


function returnflighticon(layoutfiled) {

    var flightimg = "";

    if (layoutfiled[0].iti=="1") {
        flightimg += "<img  alt='' class='FlightTip FlightTipimg mobroundtripclsimg' style='width: 28px; height: 28px;position: absolute;    left: 53px;' id='returnflight' src=\"" + layoutfiled[0].Conurl + "\">";
        flightimg += "<br/><span class='li_flightno mobroundtripclstext' style='white-space: nowrap;font-size: 10px;position: absolute;left: 54px;top: 26px;' id=Fl11ightID" + Ttotlen + ">" + layoutfiled[0].Fno + " </span>";
    }
    return flightimg;
}