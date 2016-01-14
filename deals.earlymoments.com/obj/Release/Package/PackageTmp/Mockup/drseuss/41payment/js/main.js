// JavaScript Document

// JavaScript Document

//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
	match : function() { 
			$('#headerImage').attr("src", "images/mobile_headerfall.png");
			$('#rightTopDesktop').attr("src", "images/mobile_form_top.png");
			
	},
	unmatch : function() {
			$('#headerImage').attr("src", "images/header.jpg");
			$('#rightTopDesktop').attr("src", "images/right_top1.png");
	}
})

//end of interchange

//date picker
$(window).load(function(){
	$(".date-picker").datepicker();
	$(".date-picker").on("change", function () {
	    var id = $(this).attr("id");
	    var val = $("label[for='" + id + "']").text();
	    $("#msg").text(val + " changed");
	});
});	

//billing address-shipping address radio on-off
$('#showBillInfo').hide();
$('#radbillno').click(function(){
	$('#showBillInfo').slideDown(0);

});
$('#radbillyes').click(function(){
	$('#showBillInfo').slideUp(0);
});
//end of billing address-shipping address radio on-off

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