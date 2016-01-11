//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
    match: function () {
        $('#headerImage').attr("src", "/Images/seuss/Payment4for99/mobile_headerfall.png");
        $('#rightTopDesktop').attr("src", "/Images/seuss/Payment4for99/mobile_form_top.png");

    },
    unmatch: function () {
        $('#headerImage').attr("src", "/Images/seuss/Payment4for99/header.jpg");
        $('#rightTopDesktop').attr("src", "/Images/seuss/Payment4for99/right_top1.png");
    }
})
//end of interchange

$(document).ready(function () {
    //billing address section on-off
    $('#showBillInfo').hide();
    $('#radbillno').click(function () {
        $('#showBillInfo').slideDown(200);
    });
    $('#radbillyes').click(function () {
        $('#showBillInfo').slideUp(200);
    });
    //end of billing address section on-off

    //toggle view membership agreement
    $("#divViewTermsAndConditions").click(function () {
        if ($("#divMemberAgreement").hasClass("scroll")) {
            $("#divMemberAgreement").removeClass("scroll");
            $("#divMemberAgreement").addClass("MemberAgreement-Large");
            $("#spanTermsAndConditionsSummaryLable").text("Click here to collapse Terms & Conditions");
        }
        else {
            $("#divMemberAgreement").removeClass("MemberAgreement-Large");
            $("#divMemberAgreement").addClass("scroll");
            $("#spanTermsAndConditionsSummaryLable").text("Click here to view complete Terms & Conditions");
            $('html,body').animate({ scrollTop: $('#divMemberAgreement').offset().top }, 'slow');
        }
    });
    //end of toggle view membership agreement
});