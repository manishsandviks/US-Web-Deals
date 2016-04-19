﻿var err_field;
var err_message = '';
var i = 1;
var tmp;
var bookString = '';

jQuery.fn.exists = function () { return this.length > 0; }

$(document).ready(function () {
    //    $(window).scrollTop();
    $("html, body").animate({ scrollTop: 0 }, "slow");
    $("#txtShipFirstName").focus();
    //alow numeric values
    $('.ccnumeric').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });

    //billing address-shipping address radio on-off
    $('#showBillInfo').hide();

    // console.log("radio bill no" + $("#radbillno").is(":checked"))
    if ($("#radbillno").is(":checked")) {
        $('#showBillInfo').slideDown(0);
    }

    $('#radbillno').click(function () {
        $('#showBillInfo').slideDown(0);

    });
    $('#radbillyes').click(function () {
        $('#showBillInfo').slideUp(0);
    });
    //end of billing address-shipping address radio on-off

    $('#tab2').addClass('disabled');
    $('#tab2').find('a').removeAttr("data-toggle");

    $("#btnContinueShipping").click(function () {
        if (validate_shipping() == true) {
            // pleaseWait();
            // $("#form1").submit();
            $('#tab1').removeClass('active');
            $('#tab1').addClass('disabled');
            $('#tab1').removeAttr("data-toggle");
            $('#tab1').find('a').attr("aria-expanded", "false");

            $('#tab2').removeClass('disabled');
            $('#tab2').addClass('active');
            $('#tab2').attr("data-toggle", "tab");
            $('#tab2').find('a').attr("aria-expanded", "true");

            $("#shipping").removeClass("active");
            $("#billing").addClass("active in");
            $("html, body").animate({ scrollTop: 0 }, "slow");
            $("#txtCreditCardNumber").focus();
        }
        else {
            if (err_message.length > 0) {
                $("#spnErrorMessage").html(err_message.toString());
                $("#dialog-message").modal("toggle");                
                $("html, body").animate({ scrollTop: 0 }, "slow");                
            }
        }
    });

    //Submit Button Click
    $("#btnSubmit").click(function () {

        if (validate_billingPayment() == true) {
            $("form").submit();
            pleaseWait();
            //$("#form1").submit();

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


function validate_shipping() {
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


function validate_billingPayment() {
    err_message = '';
    err_field = tmp;
    i = 1;
    //Payment and billing validation
    if ($("#radbillno").is(":checked")) {
        if ($('#txtBillFirstName').exists()) {
            if ($('#txtBillFirstName').val() == '') {
                set_errs($("#txtBillFirstName"), 'Billing first name is required.\n');
            }
            else {
                if ($('#txtBillFirstName').val().trim().length < 3 || $('#txtBillFirstName').val().trim().length > 50) {
                    set_errs($("#txtBillFirstName"), 'Billing first name should be of minimum 3 chars and maximum 15.\n');
                }
            }
        }

        if ($('#txtBillLastName').exists()) {
            if ($('#txtBillLastName').val().trim() == '') {
                set_errs($("#txtBillLastName"), 'Billing last name is required.\n');
            }
            else {
                if ($('#txtBillLastName').val().trim().length < 3 || $('#txtBillLastName').val().trim().length > 50) {
                    set_errs($("#txtBillLastName"), 'Billing last name should be minimum 3 chars and maximum 15.\n');
                }
            }
        }

        if ($('#txtBillStreet').exists()) {
            if ($('#txtBillStreet').val().trim() == '') {
                set_errs($("#txtBillStreet"), 'Billing address required.\n');
            }
            else {
                if ($('#txtBillStreet').val().trim().length < 5 || $('#txtBillStreet').val().trim().length > 30) {
                    set_errs($("#txtBillStreet"), 'Billing address should be minimum 5 chars and maximum 30.\n');
                }
            }
        }

        if ($('#txtBillCity').exists()) {
            if ($('#txtBillCity').val().trim() == '') {
                set_errs($("#txtBillCity"), 'Billing city is required.\n');
            }
            else {
                if ($('#txtBillCity').val().trim().length < 3 || $('#txtBillCity').val().trim().length > 50) {
                    set_errs($("#txtBillCity"), 'Billing City should be minimum 3 chars and maximum 16.\n');
                }
            }
        }
        if ($('#chkBillState').exists()) { if ($('#chkBillState').val() == '') { set_errs($("#chkBillState"), 'Billing state is required.\n'); } }
        if ($('#txtBillZipCode').exists()) { if ($('#txtBillZipCode').val() == '') { set_errs($("#txtBillZipCode"), 'Billing Zip code is required.\n'); } }

    }

    if ($('#txtCreditCardNumber').exists()) {
        if ($('#txtCreditCardNumber').val() == '') {
            set_errs($("#txtCreditCardNumber"), 'Credit Card number is required.\n');
        }
        else {
            if ($('#txtCreditCardNumber').val().trim().length < 15 || $('#txtCreditCardNumber').val().trim().length > 16) {
                set_errs($("#txtCreditCardNumber"), 'Credit Card number should be minimum 15 chars and maximum 17.\n');
            }
        }
    }

    if ($('#optCardExpiryMonth').exists()) { if ($('#optCardExpiryMonth').val() == '' || $('#optCardExpiryMonth').val() == 'Month') { set_errs($("#optCardExpiryMonth"), 'Credit Card expiration month is required.\n'); } }
    if ($('#optCardExpiryYear').exists()) { if ($('#optCardExpiryYear').val() == '' || $('#optCardExpiryYear').val() == 'Year') { set_errs($("#optCardExpiryYear"), 'Credit Card expiration year is required.\n'); } }
    if (!ValidateExpDate()) { err_message += i++ + '. ' + "Invalid Credit Card Expiration date.<br/>" }
    if ($('#txtCVV').exists()) {
        if ($('#txtCVV').val() == '') {
            set_errs($("#txtCVV"), 'Credit Card security code (CVV) is required.\n');
        }
        else {
            if ($('#txtCVV').val().trim().length < 3 || $('#txtCVV').val().trim().length > 4) {
                set_errs($("#txtCVV"), 'Credit Card security code should be 4 chars.\n');
            }
        }
    }

    //if ($('#optADOBMonth').exists()) { if ($('#optADOBMonth').val() == '' || $('#optADOBMonth').val() == 'Month') { set_errs($("#optADOBMonth"), 'Adult DOB month is required.\n'); } }
    //if ($('#optADOBDay').exists()) { if ($('#optADOBDay').val() == '' || $('#optADOBDay').val() == 'Year') { set_errs($("#optADOBDay"), 'Adult DOB Day is required.\n'); } }
    //if ($('#optADOBYear').exists()) { if ($('#optADOBYear').val() == '' || $('#optADOBYear').val() == 'Year') { set_errs($("#optADOBYear"), 'Adult DOB year is required.\n'); } }

    //if (!ValidateDOBDate()) { err_message += i++ + '. ' + "Invalid Adult Date Of Birth.<br/>" }

    //if ($('#txtKey').exists()) { if ($('#txtKey').val() == '') { set_errs($("#txtCVV"), 'Security captcha is required.\n'); } }
    //var age = document.getElementById("chkAge");
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


function ValidateExpDate() {
    var ccExpYear = $('#optCardExpiryYear').val();
    var ccExpMonth = $('#optCardExpiryMonth').val();
    var expDate = new Date();
    expDate.setFullYear(ccExpYear, ccExpMonth, 1);
    expDate.setDate(expDate.getDate() - 1);
    var today = new Date();

    if (expDate < today) {
        return false;
    }
    else {
        return true;
    }
}


function OpenNewWindow(theURL) {
    window.open(theURL, 'newwindow', 'toolbar=no,location=no,menubar=no,scrollbars=yes, resizable=no, top=100,left=100');
    return false;
}