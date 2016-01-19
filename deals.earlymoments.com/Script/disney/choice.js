
var choiceCount;
var choiceBookPrice = 0.00;

$(document).ready(function () {
    /* image click will trigger the checkbox choice selection */
    $("#imgChoice1").click(function () {
        // alert("Hi");
        if ($("#chkChoice1").is(":disabled") == false) {
            if ($("#chkChoice1").is(":checked")) {
                $("#chkChoice1").attr("checked", false);
                $("#chkChoice1").prop("checked", false);
            }
            else {
                $("#chkChoice1").attr("checked", true);
                $("#chkChoice1").prop("checked", true);
            }
            selected_books(ContentPlaceHolder1_chkChoice1);
        }
    });

    $("#imgChoice2").click(function () {
        if ($("#chkChoice2").is(":disabled") == false) {
            if ($("#chkChoice2").is(":checked")) {
                $("#chkChoice2").attr("checked", false);
                $("#chkChoice2").prop("checked", false);
            }
            else {
                $("#chkChoice2").attr("checked", true);
                $("#chkChoice2").prop("checked", true);
            }
            selected_books(ContentPlaceHolder1_chkChoice2);
        }
    });

    $("#imgChoice3").click(function () {
        if ($("#chkChoice3").is(":disabled") == false) {
            if ($("#chkChoice3").is(":checked")) {
                $("#chkChoice3").attr("checked", false);
                $("#chkChoice3").prop("checked", false);
            }
            else {
                $("#chkChoice3").attr("checked", true);
                $("#chkChoice3").prop("checked", true);
            }
            //$("#chkChoice3").toggle(this.checked);
            selected_books(ContentPlaceHolder1_chkChoice3);
        }
    });

    $("#imgChoice4").click(function () {
        if ($("#chkChoice4").is(":disabled") == false) {
            if ($("#chkChoice4").is(":checked")) {
                $("#chkChoice4").attr("checked", false);
                $("#chkChoice4").prop("checked", false);
            }
            else {
                $("#chkChoice4").attr("checked", true);
                $("#chkChoice4").prop("checked", true);
            }
            selected_books(ContentPlaceHolder1_chkChoice4);
        }
    });
    /* image click end here*/
    //Changes on 01/31/2014 11:52AM IST
    var chCount = $(".chkChoice:checked").length;
    //  alert(chCount);
    if (chCount == 2) {
        $(".chkChoice").each(function () {
            //alert(this.checked);
            if (this.checked == false)
                this.disabled = true;
            else
                selected_books(this);
        });
    }
    //Changes end here on 01/31/2014 11:52AM IST
});

function selected_books(control) {
    // alert("Id " + control.id);
    var lcName = control.id.substring(control.id.toString().length - 1)
    // alert("lc Name " + lcName);
    // var c = control.id.toString().replace(lcName, '');
    var c = control.id.toString().substring(0, (control.id.toString().length - 1));

    //alert("c1: " + c);
    choiceCount = 0;
    choiceBookPrice = 0.00;
    var opts = 0;
    var bk1;
    var bk2;
    var bk3;
    var bk4;
    var bookName1 = '';
    var bookName2 = '';
    var bookName3 = '';
    var bookName4 = '';
    if (c == 'chkChoice') {
        // if checkbox in second section of page
        bk1 = document.getElementById("chkChoice1");
        bk2 = document.getElementById("chkChoice2");
        bk3 = document.getElementById("chkChoice3");
        bk4 = document.getElementById("chkChoice4");
    }

    if (bk1.checked) { opts++; choiceCount++; bookName1 = "Frozen"; }
    if (bk2.checked) { opts++; choiceCount++; bookName2 = "Planes"; }
    if (bk3.checked) { opts++; choiceCount++; bookName3 = "Monsters University"; }
    if (bk4.checked) { opts++; choiceCount++; bookName4 = "Brave"; }
    //alert(choiceCount);
    zipCode = $("#txtShipZipCode").val();

    if (opts == 2) {
        choiceBookPrice = 0.00;

        if (bk1.checked == false) {
            bk1.disabled = true;

        }

        if (bk2.checked == false) {
            bk2.disabled = true;
        }

        if (bk3.checked == false) {
            bk3.disabled = true;
        }
        if (bk4.checked == false) {
            bk4.disabled = true;
        }
    }
    else if (opts < 2) {
        //alert("choice count: " + choiceCount + "Opts: " + opts);

        if (bk1.disabled == true) {
            bk1.disabled = false;
        }
        if (bk2.disabled == true) {
            bk2.disabled = false;
        }
        if (bk3.disabled == true) {
            bk3.disabled = false;
        }
        if (bk4.disabled == true) {
            bk4.disabled = false;
        }
    }
    else {

        control.checked = false;
        if (bk1.disabled == true) {
            bk1.disabled = false;
        }
        if (bk2.disabled == true) {
            bk2.disabled = false;
        }
        if (bk3.disabled == true) {
            bk3.disabled = false;
        }
        if (bk4.disabled == true) {
            bk4.disabled = false;
        }
    }
}