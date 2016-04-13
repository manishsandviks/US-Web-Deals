var err_field;
var err_message = '';
var i = 1;
var tmp;
var bookString = '';

jQuery.fn.exists = function () { return this.length > 0; }

$(document).ready(function () {
    //Submit Button Click
    $("#btnaddupsell").click(function () {
        //alert("HI");
        //alert(validate_form());
        if (validate_form() == true) {
             pleaseWait();
            $("form").submit();

        }
        else {
            if (err_message.length > 0) {
                $("#spnErrorMessage").html(err_message.toString());
                $("#dialog-message").modal("toggle");
                return false;
            }
        }
    });
    $("#btnNoUpsell").click(function () {
        $("form").submit();
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
    var age = document.getElementById("chkAge");
    if (age.checked == false) { set_errs(age, 'Agree to the terms and conditions of this offer.\n'); }

    console.log(err_message)
    if (err_message.length > 0) {
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


