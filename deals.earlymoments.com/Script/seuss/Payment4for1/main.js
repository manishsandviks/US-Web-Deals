//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
    match: function () {
        $('#headerImage').attr("src", "/Images/seuss/Payment4for1/mobile_headerfall.png");
        $('#rightTopDesktop').attr("src", "/Images/seuss/Payment4for1/mobile_form_top.png");

    },
    unmatch: function () {
        $('#headerImage').attr("src", "/Images/seuss/Payment4for1/header.jpg");
        $('#rightTopDesktop').attr("src", "/Images/seuss/Payment4for1/right_top1.png");
    }
})
//end of interchange

