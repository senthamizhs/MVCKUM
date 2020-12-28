var specialKeys = new Array();
specialKeys.push(32); //space
specialKeys.push(8); //Backspace
specialKeys.push(9); //Tab
specialKeys.push(46); //Delete
specialKeys.push(36); //Home
specialKeys.push(35); //End
specialKeys.push(37); //Left
specialKeys.push(39); //Right

function ValidateAlphaCaps(e) {


    if (e.currentTarget.id == "txtorigincity" || e.currentTarget.id == "txtdestinationcity" || e.currentTarget.id.indexOf("txtMOrigin") != -1 || e.currentTarget.id.indexOf("txtMDestinations") != -1) {
        hideoldavaildetails();
    }
    if (/Firefox[\/\s](\d+\.\d+)/.test(navigator.userAgent)) {
        var code;
        var code1;
        if (!e) var e = window.event
        //if (e.keyCode) code = e.keyCode;
        //else if (e.which) code = e.which;
        if (e.which) code = e.which;
        if ((code < 65 || code > 90) && (code < 97 || code > 122) && (code != 13) && (code != 9) && (code != 8) && (code != 219) && (code != 220))
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
        // else if (e.which) code = e.which;
        if ((code < 65 || code > 90) && (code < 97 || code > 122) && (code != 13) && (code != 9) && (code != 8))
            return false;
        else
            return true;
    }
}

// Validate Alpha & Space Fields for availability
function ValidateAlphaCapsspace(e) {


    if (e.currentTarget.id == "txtorigincity" || e.currentTarget.id == "txtdestinationcity" || e.currentTarget.id.indexOf("txtMOrigin") != -1 || e.currentTarget.id.indexOf("txtMDestinations") != -1) {
        hideoldavaildetails();
    }
    if (/Firefox[\/\s](\d+\.\d+)/.test(navigator.userAgent)) {
        var code;
        var code1;
        if (!e) var e = window.event
        //if (e.keyCode) code = e.keyCode;
        //else if (e.which) code = e.which;
        if (e.which) code = e.which;
        if ((code < 65 || code > 90) && (code < 97 || code > 122) && (code != 13) && (code != 9) && (code != 8) && (code != 219) && (code != 220) && (code != 32))
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
        // else if (e.which) code = e.which;
        if ((code < 65 || code > 90) && (code < 97 || code > 122) && (code != 13) && (code != 9) && (code != 8) && (code != 32))
            return false;
        else
            return true;
    }
}

// Validate Alpha & Numeric Fields
function ValidateAlphaNumeric(event) {
    if (/Firefox[\/\s](\d+\.\d+)/.test(navigator.userAgent)) {
        var code;
        var code1;
        if (!event) var event = window.event
        if (event.which) code = eeventwhich;
        if ((code < 65 || code > 90) && (code < 97 || code > 122) && (code < 48 || code > 57) && (code != 13) && (code != 9) && (code != 8) && (code != 219) && (code != 220))
            return false;
        else
            if (event.keyCode) code = event.keyCode;
        if (code == 46) return true;
        return true;
    }
    else {
        var code;
        if (!event) var event = window.event
        if (event.keyCode) code = event.keyCode;
        if ((code < 65 || code > 90) && (code < 97 || code > 122) && (code < 48 || code > 57) && (code != 13) && (code != 9) && (code != 8))
            return false;
        else
            return true;
    }
}
/****/

// To validate  numeric fields
function isNumericVal(event) {
    if (/Firefox[\/\s](\d+\.\d+)/.test(navigator.userAgent)) {
        var code;
        if (!event) var event = window.event
        //      if (e.keyCode) code = e.keyCode;
        if (event.which) code = event.which;

        if ((code != 8) && (code < 48) || (code > 57)) {
            event.preventDefault();
            return false;
        }
        else
            return true;
    }
    else {
        var code;
        if (!event) var event = window.event
        if (event.keyCode) code = event.keyCode;
        //if (e.which) code = e.which;
        if ((code < 48) || (code > 57) && (code != 8) && (code != 46)) {
            event.preventDefault();
            return false;
        }
        else
            return true;
    }
}
/********/

// Validate Alpha Fields
function ValidateAlpha(event) {
    var regex = new RegExp("^[a-zA-Z\b]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    var keyCode = (event.which) ? event.which : event.keyCode
    if (!regex.test(key) && keyCode != 9 && keyCode != 32) {
        event.preventDefault();
        return false;
    }
    else { return true; }
}

function ValidateEmailtxt(event) { // Allows AlphaNumeric and only . _ @ for EMail purpose..
    try {
        var keyCode = event.keyCode == 0 ? event.charCode : event.keyCode;
        var ret = ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (keyCode >= 48 && keyCode <= 57) || (keyCode == 95) || (keyCode == 46) || (keyCode == 64) || (specialKeys.indexOf(event.keyCode) != -1 && event.charCode != event.keyCode));
        return ret;
    }
    catch (Error) {
    }
}

function validateAlphabets(event) { //Allows Only Alphabets with Spaces..
    try {


        var keyCode = event.keyCode == 0 ? event.charCode : event.keyCode;
        var ret = ((keyCode == 32) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(event.keyCode) != -1 && event.charCode != event.keyCode));
        return ret;
    } catch (Error) {
    }
}

function validateAlphabetswithoutspc(event) { //Allows Only Alphabets without Spaces..
    try {
        var keyCode = event.keyCode == 0 ? event.charCode : event.keyCode;
        var ret = ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(event.keyCode) != -1 && event.charCode != event.keyCode));
        return ret;
    } catch (Error) {
    }
}

function validateAlphandNumeric(event) { // It Allows Only Alpha Numeric Values Only..
    try {
        var keyCode = event.keyCode == 0 ? event.charCode : event.keyCode;
        var ret = ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (keyCode >= 48 && keyCode <= 57) || (specialKeys.indexOf(event.keyCode) != -1 && event.charCode != event.keyCode));
        return ret;
    } catch (Error) {
    }
}

$('.clspstnumeric').on('paste', function (e) {

    setTimeout($.proxy(function () {
        var input = $(this);
        var value = input.val();
        for (var i = 0; i < value.length; i++) {
            var code = value[i].charCodeAt(0);
            if ((code < 48) || (code > 57) && (code != 8) && (code != 46)) {
                alert("Invalid content");
                input.val("");
                break;
            }
        }
    }, this), 100);
});

$('.clspstAlphaWithSpace').on('paste', function (e) {

    setTimeout($.proxy(function () {
        var input = $(this);
        var value = input.val();
        for (var i = 0; i < value.length; i++) {
            var code = value[i].charCodeAt(0);
            if ((code < 65 || code > 90) && (code < 97 || code > 122)) {
                alert("Invalid content");
                input.val("");
                break;
            }
        }
    }, this), 100);
});

$('.clspstAlphaNumaric').on('paste', function (e) {

    setTimeout($.proxy(function () {
        var input = $(this);
        var value = input.val();
        for (var i = 0; i < value.length; i++) {
            var code = value[i].charCodeAt(0);
            if ((code < 65 || code > 90) && (code < 97 || code > 122) && (code < 48 || code > 57)) {
                alert("Invalid content");
                input.val("");
                break;
            }
        }
    }, this), 100);
});

$('.clspstAlphaWithOutSpace').on('paste', function (e) {

    setTimeout($.proxy(function () {
        var input = $(this);
        var value = input.val();
        for (var i = 0; i < value.length; i++) {
            var code = value[i].charCodeAt(0);
            if ((code < 64 || code > 90) && (code < 97 || code > 122)) {
                alert("Invalid content");
                input.val("");
                break;
            }
        }
    }, this), 100);
});

$('.clspstNumericWithHypen').on('paste', function (e) {

    setTimeout($.proxy(function () {
        var input = $(this);
        var value = input.val();
        for (var i = 0; i < value.length; i++) {
            var code = value[i].charCodeAt(0);
            if ((code < 48 || code > 57) && (code != 45)) {
                alert("Invalid content");
                input.val("");
                break;
            }
        }
    }, this), 100);
});

function ValidateSpecialChar(event) {
    //var k = String.fromCharCode(e.which);

    //if (k.match(/[^a-zA-Z0-9]/g)) {
    //    e.preventDefault();
    //    return false;
    //}
    //else
    //    return true;
    var regex = new RegExp("^[a-zA-Z0-9\b]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    var keyCode = (event.which) ? event.which : event.keyCode
    if (!regex.test(key) && keyCode != 9 && keyCode != 46 && keyCode != 32) {
        event.preventDefault();
        return false;
    }
    else { return true; }
}


//#region Avoid space 

function AvoidSpace(e, flg, idd) {
    if (event.keyCode == 32) {
        event.returnValue = false;
        return false;
    }

    if (flg != "" && flg == "caps") { //Two functions in same keypress event...
        isCapLockOn(e, idd);
    }
}
//#endregion