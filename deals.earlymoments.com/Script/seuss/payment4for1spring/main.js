//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
    match: function () {
        $('#headerImage').attr("src", "/Images/seuss/payment4for1spring/mobile_headerfall.png");
        $('#rightTopDesktop').attr("src", "/Images/seuss/payment4for1spring/mobile_form_top.png");

    },
    unmatch: function () {
        $('#headerImage').attr("src", "/Images/seuss/payment4for1spring/header.jpg");
        $('#rightTopDesktop').attr("src", "/Images/seuss/payment4for1spring/right_top1.png");
    }
})
//end of interchange

