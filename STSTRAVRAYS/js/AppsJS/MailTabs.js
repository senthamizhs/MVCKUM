

//#region Service Charge 

function Loadairlinedetails() {

    var Category = document.getElementById("serv_aircategory").value;

    $.ajax({
        url: "XML/AirlineNames.xml",
        dataType: "xml",
        success: function (xmlResponse) {
            Airdata = $("AIRLINEDET", xmlResponse).map(function () {
                if (Category == $("_CATEGORY", this).text()) {
                    return {
                        value: $("_CODE", this).text(),
                        name: $("_NAME", this).text(),
                        cat: $("_CATEGORY", this).text()
                    };
                }

            }).get();


            $("body").data("Airdata", Airdata);

            LoadAir(Airdata);

        }

    });
    $("#fscairlines").css("pointer-events", "none");

}

function LoadAir(datas) {

    // var _Opt = "";
    CreateOption(datas, "txtAirLineName");
    // CreateOption(datas, );

    $("#txtAirLineName").chosen({ width: "95%" });



}

function CreateOption(_Obj, _Objid) {
    // var sel = document.getElementById(_Objid);
    var sel = document.getElementById(_Objid);

    if (_Obj) {
        var i;
        for (i = sel.options.length - 1; i >= 0; i--) {
            sel.remove(i);
        }
        //_Opt = document.createElement('option');
        ////_Opt.text = "All";
        ////_Opt.value = "All";

        //sel.options.add(_Opt);
        var elOptNew;
        for (i = 0; i < _Obj.length; i++) {
            elOptNew = document.createElement('option');
            elOptNew.text = _Obj[i].value + " - " + _Obj[i].name;
            elOptNew.value = _Obj[i].value;

            //elOptNew2 = document.createElement('option');
            //elOptNew2.text = _Obj[i].value + " - " + _Obj[i].name;
            //elOptNew2.value = _Obj[i].value;

            sel.options.add(elOptNew);
            // sel2.options.add(elOptNew2);
        }

    }

    $("#txtAirLineName").trigger("chosen:updated");
}

function Changeservcategory() {
    document.getElementById("tblservicecharge").style.display = "none";
    if ($("#serv_checkstandard")[0].checked == true) {
        document.getElementById("txtAirLineName").value = "";
        $('#txtAirLineName').empty();
        $('#txtAirLineName').trigger('');
        $('#txtAirLineName').trigger('chosen:updated');
        $('#txtAirLineName')[0].selectedIndex = "";
        $("#fscairlines").css("pointer-events", "none");
        document.getElementById("tblservicecharge").style.display = "none";
        changeservcateg();
    }
    else {
        $("#fscairlines").css("pointer-events", "all");
        document.getElementById("tblservicecharge").style.display = "none";
    }

}

function changeservcateg() {
    $("#txtAirLineName").val('').trigger("chosen:updated");

    var Category = document.getElementById("serv_aircategory").value;

    $.ajax({
        url: "XML/AirlineNames.xml",
        dataType: "xml",
        success: function (xmlResponse) {
            Airdata = $("AIRLINEDET", xmlResponse).map(function () {
                if ($("_CATEGORY", this).text() == Category) {
                    return {
                        value: $("_CODE", this).text(),
                        name: $("_NAME", this).text(),
                        cat: $("_CATEGORY", this).text()
                    };
                }
            }).get();
            $("body").data("Airdata", Airdata);

            LoadAir(Airdata);

        }

    });
}

function validate() {
    var lblErr = document.getElementById("lblError1");
    var iCharsse = "!`@#$%^&*()+=-[]\\\';/{}|\":<>?~_";

    var data = $('#txtAirLineName').val();// document.getElementById("txtcode").value;


    var Category = document.getElementById("serv_aircategory").value;

    var Allcheck = "";
    if ($("#serv_checkstandard")[0].checked == true) { Allcheck = "YES"; } else { Allcheck = "NO"; }

    if (data == null && Allcheck == "NO") {
        alert("Please select airline name");
        return false;
    }
    else {
        $.blockUI({
            message: '<img src="Images/TravRaysWait.gif" id="FareRuleLodingImage" />',
            css: { backgroundColor: '#fff', padding: '5px', }
        });

        $.ajax({
            type: "POST", 		//GET or POST or PUT or DELETE verb
            url: "NewAvailServers.asmx/btnGetDetails_Click", 		// Location of the service
            data: '{airlineNameCode: "' + data + '",AirlineCategory:"' + Category + '",Allcheck:"' + Allcheck + '"}',// ,CancelationStatus: "' + CancelationStatus + '"}',//,CancelationStatus: "' + $("#hdnPax")[0].value + '" ,BlockTicket: "' + BlockTicket + '",PaymentMode: "' + $("#ddlPaymode").val() + '",TourCode: "' + $("#Tour_Code").val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (json) {//On Successful service call
                var result = json.d//[0].split('|')

                if (result[1] != "") {
                    document.getElementById("tblservicecharge").style.display = "block";
                    document.getElementById("lblError1").style.display = "none";
                    document.getElementById('tblservicecharge').innerHTML = result[1];

                }
                else {
                    document.getElementById("tblservicecharge").style.display = "none";
                    document.getElementById("lblError1").style.display = "block";
                    document.getElementById("lblError1").innerText = result[0];
                }
                $.unblockUI();
            },
            error: function (e) {//On Successful service call                            
                if (e.status == "500") {
                    alert("Session has been Expired.");
                    window.top.location = sessionExb;
                    return false;
                }
                $.unblockUI();
            }	// When Service call fails

        });
    }





}

function clearsrvcrg() {
    document.getElementById("tblservicecharge").style.display = "none";
    document.getElementById("txtAirLineName").value = "";
    $('#txtAirLineName').empty();
    $('#txtAirLineName').trigger('');
    $('#txtAirLineName').trigger('chosen:updated');
    $('#txtAirLineName')[0].selectedIndex = "";

    changeservcateg();
}

function CheckLength(text, long) {
    var maxlength = new Number(long); // Change number to your max length.
    if (text.value.length > maxlength) {
        text.value = text.value.substring(0, maxlength);

    }
}
//#endregion