// JavaScript Document

//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
	match : function() { 
			$('#formProduct').attr("src", "images/form_product_sm.jpg");
			$('#goUp').insertBefore('#goDown');
	},
	unmatch : function() {
			$('#formProduct').attr("src", "images/form_product.jpg");
			$('#goUp').insertAfter('#goDown');
	}
})
//end of interchange

//billing address-shipping address radio on-off
$('#showBillInfo').hide();
$('#radbillno').click(function(){
	$('#showBillInfo').slideDown(1000);

});
$('#radbillyes').click(function(){
	$('#showBillInfo').slideUp(1000);
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