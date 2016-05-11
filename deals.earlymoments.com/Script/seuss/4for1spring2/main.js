$(document).ready(function () {
    //for the cols interchange		
    //for the cols interchange		
    enquire.register("screen and (max-width:767px)", {
        match: function () {
            $('#headerImageLanding').attr("src", "/Images/seuss/4for1spring2/mobile_headerfall.png");
            $('#headerImagePayment').attr("src", "/Images/seuss/4for1spring2/mobile_headerfall.png");
            $('#rightTopDesktop').attr("src", "/Images/seuss/4for1spring2/mobile_form_top.png");

        },
        unmatch: function () {
            $('#headerImageLanding').attr("src", "/Images/seuss/4for1spring2/top.gif");
            $('#headerImagePayment').attr("src", "/Images/seuss/4for1spring2/header.jpg");
            $('#rightTopDesktop').attr("src", "/Images/seuss/4for1spring2/right_top1.png");
        }
    })
    //end of interchange


});
function showPayment() {
    $('#paymentLeft').show();
    $('#paymentRight').show();
    $('#headerImagePayment').show();

    $('#landingLeft').hide();
    $('#landingRight').hide();
    $('#headerImageLanding').hide();
}