// JavaScript Document
//toggle payment
$(document).ready(function(){
    $("#viewOrderSummeryBtn").click(function(){
        $("#viewOrderSummery").toggle();
    });
});

//date picker
$(window).load(function(){
	$(".date-picker").datepicker();
	$(".date-picker").on("change", function () {
	    var id = $(this).attr("id");
	    var val = $("label[for='" + id + "']").text();
	    $("#msg").text(val + " changed");
	});
});

//scroll
function goToByScroll(id){
	id = id.replace("link", "");
	$('html,body').animate({
		scrollTop: $("#"+id).offset().top},
		'slow');
}
$("#aboutlink").click(function(e) { 
	e.preventDefault(); 
	goToByScroll($(this).attr("id"));           
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
		$("#viewOrderSummeryBtn").text("Click here to collapse your Order Summary");
	}
	else {
		$("#divMemberAgreement").removeClass("MemberAgreement-Large");
		$("#divMemberAgreement").addClass("scroll");
		$("#spanTermsAndConditionsSummaryLable").text("Click here to view complete Terms & Conditions");
		$("#viewOrderSummeryBtn").text("Click here to view your Order Summary");
		
		$('html,body').animate({ scrollTop: $('#divMemberAgreement').offset().top }, 'slow');
	}
});
//end of toggle view membership agreement
