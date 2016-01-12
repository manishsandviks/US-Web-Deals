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
