var err_field;
var err_message = '';
var i = 1;
var tmp;
var bookString = '';

jQuery.fn.exists = function () { return this.length > 0; }

$(document).ready(function () {
    $("#btnSubmit").click(function () {
        if (validate_form() == true) {
            // pleaseWait();
            $("form").submit();

        }
        else {
            if (err_message.length > 0) {
                $("#spnErrorMessage").html(err_message.toString());
                $("#dialog-message").modal("toggle");
            }
        }
    });
});


function set_errs(field, message) {
    if (typeof err_field == 'undefined') { err_field = field; }
    err_message += i++ + '. ' + message + '<br/>';;
}


function validate_form() {
    err_message = '';
    err_field = tmp;
    i = 1;

    if ($("#chkChoice1").exists()) {
        var chkCount = $(".chkChoice:checked").length;
        console.log("checkbox count" + chkCount + "book selected count " + choiceCount);

        if (choiceCount < 2 || chkCount < 2) { set_errs($("#ShipFirstName"), 'Choose any two books from the choice of books listed.\n'); }
    }

    if ($('#txtShipFirstName').exists()) {
        if ($('#txtShipFirstName').val().trim() == '') {
            set_errs($("#txtShipFirstName"), 'Shipping first name is required.\n');
        }
        else {
            if ($('#txtShipFirstName').val().trim().length < 3 || $('#txtShipFirstName').val().trim().length > 50) {
                set_errs($("#txtShipFirstName"), 'Shipping first name should be of minimum 3 chars and maximum 15.\n');
            }
        }
    }

    if ($('#txtShipLastName').exists()) {
        if ($('#txtShipLastName').val().trim() == '') {
            set_errs($("#txtShipLastName"), 'Shipping last name is required.\n');
        }
        else {
            if ($('#txtShipLastName').val().trim().length < 3 || $('#txtShipLastName').val().trim().length > 50) {
                set_errs($("#txtShipLastName"), 'Shipping last name should be minimum 3 chars and maximum 15.\n');
            }
        }
    }

    if ($('#txtShipStreet').exists()) {
        if ($('#txtShipStreet').val().trim() == '') {
            set_errs($("#txtShipStreet"), 'Shipping address required.\n');
        } else {
            if ($('#txtShipStreet').val().trim().length < 5 || $('#txtShipStreet').val().trim().length > 30) {
                set_errs($("#txtShipStreet"), 'Shipping address should be minimum 5 chars and maximum 30.\n');
            }
        }
    }

    if ($('#txtShipCity').exists()) {
        if ($('#txtShipCity').val().trim() == '') {
            set_errs($("#txtShipCity"), 'Shipping city is required.\n');
        }
        else {
            if ($('#txtShipCity').val().trim().length < 3 || $('#txtShipCity').val().trim().length > 50) {
                set_errs($("#txtShipCity"), 'Shipping City should be minimum 3 chars and maximum 16.\n');
            }
        }
    }

    if ($('#chkShipState').exists()) { if ($('#chkShipState').val() == '') { set_errs($("#chkShipState"), 'Shipping state is required.\n'); } }
    if ($('#txtShipZipCode').exists()) { if ($('#txtShipZipCode').val() == '') { set_errs($("#txtShipZipCode"), 'Shipping Zip code is required.\n'); } }
    //if ($('#txtShipEmail').exists()) { if ($('#txtShipEmail').val().trim() == '') { set_errs($("#txtShipEmail"), 'eMail is required.\n'); } }
    if ($('#txtShipEmail').exists()) {
        if ($('#txtShipEmail').val().trim() == '') {
            set_errs($("#txtShipEmail"), 'eMail is required.\n');
        }
        else {
            var emailtext = $('#txtShipEmail').val().trim().toString();
            if (validateEmail(emailtext) == false) {
                set_errs($("#txtShipEmail"), 'Enter valid eMail Address.\n');
            }
        }
    }

    if ($('#txtShipConfirmEmail').exists()) { if ($('#txtShipConfirmEmail').val().trim().toLowerCase() != $('#txtShipEmail').val().trim().toLowerCase()) { set_errs($("#txtShipConfirmEmail"), 'Confirmation eMail must match with original eMail.\n'); } }

    if ($('#txtShipChildDOB').exists()) {
        if ($('#txtShipChildDOB').val() != '') {
            if (isDate($('#txtShipChildDOB').val()) == false) {
                set_errs($("#txtShipChildDOB"), 'Enter valid Child Date Of Birth.\n');
            }
        }
    }

    // var age = document.getElementById("chkAge");
    //if (age.checked == false) { set_errs(age, 'Agree to the terms and conditions of this offer.\n'); }
    console.log(err_message)
    if (err_message.length > 0) {
        // $("#spnErrorMessage").html(err_message.toString());
        // $("#spnErrorMessage").text(err.toString());
        // $("#dialog-message").modal("toggle");
        return false;
    }
    else {
        //pleaseWait();
        return true;
    }
}


function pleaseWait() {
    $("#loading").fadeIn();
    var opts = {
        lines: 12, // The number of lines to draw
        length: 7, // The length of each line
        width: 4, // The line thickness
        radius: 10, // The radius of the inner circle
        color: '#000', // #rgb or #rrggbb
        speed: 1, // Rounds per second
        trail: 60, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false // Whether to use hardware acceleration
    };
    var target = document.getElementById('loading');
    var spinner = new Spinner(opts).spin(target);
}


function validateEmail(sEmail) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (filter.test(sEmail)) {
        return true;
    }
    else {
        return false;
    }
}

function isDate(txtDate) {
    var currVal = txtDate;
    if (currVal == '')
        return false;

    //Declare Regex  
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;

    //Checks for mm/dd/yyyy format.
    dtMonth = dtArray[1];
    dtDay = dtArray[3];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}