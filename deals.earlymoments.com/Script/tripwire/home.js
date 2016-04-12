var err_field;
var err_message = '';
var i = 1;
var tmp;

jQuery.fn.exists = function () { return this.length > 0; }

$(document).ready(function () {

    //Submit Button Click
    $("#heroFormButton").click(function () {
        $("#form1").submit();
    });

    $("#section3Button").click(function () {
        $("#form1").submit();
    });
});


function set_errs(field, message) {
    if (typeof err_field == 'undefined') { err_field = field; }
    err_message += i++ + '. ' + message + '<br/>';;
}
