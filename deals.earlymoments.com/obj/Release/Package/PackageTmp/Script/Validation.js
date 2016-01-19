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
            $("#form1").submit();

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
    if ($('#txtShipFirstName').exists()) {
        if ($('#txtShipFirstName').val().trim() == '') {
            set_errs($("#txtShipFirstName"), 'Shipping first name is required.\n');
        }
        else {
            if ($('#txtShipFirstName').val().trim().length < 3 || $('#txtShipFirstName').val().trim().length > 15) {
                set_errs($("#txtShipFirstName"), 'Shipping first name should be of minimum 3 chars and maximum 15.\n');
            }
        }
    }

    if ($('#txtShipLastName').exists()) {
        if ($('#txtShipLastName').val().trim() == '') {
            set_errs($("#txtShipLastName"), 'Shipping last name is required.\n');
        }
        else {
            if ($('#txtShipLastName').val().trim().length < 3 || $('#txtShipLastName').val().trim().length > 15) {
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
            if ($('#txtShipCity').val().trim().length < 3 || $('#txtShipCity').val().trim().length > 16) {
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

    //if ($("#radbillno").is(":checked")) {
    //    if ($('#txtBillFirstName').exists()) {
    //        if ($('#txtBillFirstName').val() == '') {
    //            set_errs($("#txtBillFirstName"), 'Billing first name is required.\n');
    //        }
    //        else {
    //            if ($('#txtBillFirstName').val().trim().length < 3 || $('#txtBillFirstName').val().trim().length > 15) {
    //                set_errs($("#txtBillFirstName"), 'Billing first name should be of minimum 3 chars and maximum 15.\n');
    //            }
    //        }
    //    }

    //    if ($('#txtBillLastName').exists()) {
    //        if ($('#txtBillLastName').val().trim() == '') {
    //            set_errs($("#txtBillLastName"), 'Billing last name is required.\n');
    //        }
    //        else {
    //            if ($('#txtBillLastName').val().trim().length < 3 || $('#txtBillLastName').val().trim().length > 15) {
    //                set_errs($("#txtBillLastName"), 'Billing last name should be minimum 3 chars and maximum 15.\n');
    //            }
    //        }
    //    }

    //    if ($('#txtBillStreet').exists()) {
    //        if ($('#txtBillStreet').val().trim() == '') {
    //            set_errs($("#txtBillStreet"), 'Billing address required.\n');
    //        }
    //        else {
    //            if ($('#txtBillStreet').val().trim().length < 5 || $('#txtBillStreet').val().trim().length > 30) {
    //                set_errs($("#txtBillStreet"), 'Billing address should be minimum 5 chars and maximum 30.\n');
    //            }
    //        }
    //    }

    //    if ($('#txtBillCity').exists()) {
    //        if ($('#txtBillCity').val().trim() == '') {
    //            set_errs($("#txtBillCity"), 'Billing city is required.\n');
    //        }
    //        else {
    //            if ($('#txtBillCity').val().trim().length < 3 || $('#txtBillCity').val().trim().length > 16) {
    //                set_errs($("#txtBillCity"), 'Billing City should be minimum 3 chars and maximum 16.\n');
    //            }
    //        }
    //    }
    //    if ($('#chkBillState').exists()) { if ($('#chkBillState').val() == '') { set_errs($("#chkBillState"), 'Billing state is required.\n'); } }
    //    if ($('#txtBillZipCode').exists()) { if ($('#txtBillZipCode').val() == '') { set_errs($("#txtBillZipCode"), 'Billing Zip code is required.\n'); } }

    //}

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