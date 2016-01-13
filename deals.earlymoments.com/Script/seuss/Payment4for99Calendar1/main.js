$(document).ready(function () {
    //for the cols interchange		
    enquire.register("screen and (max-width:767px)", {
        match: function () {
            $('#headerImage').attr("src", "/Images/seuss/Payment4for99Calendar1/mobile_headerfall.png");
            // $('#step1').attr("src", "/Images/seuss/Payment4for99Calendar1/step1Mob.png");
            $('#rightTopDesktop').attr("src", "/Images/seuss/Payment4for99Calendar1/mobile_form_top.png");

        },
        unmatch: function () {
            $('#headerImage').attr("src", "/Images/Seuss/Payment4for99Calendar1/header.jpg");
            // $('#step1').attr("src", "/Images/seuss/Payment4for99Calendar1/step1.png");
            $('#rightTopDesktop').attr("src", "/Images/seuss/Payment4for99Calendar1/right_top1.png");
        }
    })
    //end of interchange   

});