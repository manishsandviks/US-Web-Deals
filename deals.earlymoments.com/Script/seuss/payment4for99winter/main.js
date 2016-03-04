//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
    match: function () {
        $('#headerImage').attr("src", "/Images/seuss/payment4for99winter/mobile_headerfall.png");
        $('#rightTopDesktop').attr("src", "/Images/seuss/payment4for99winter/mobile_form_top.png");

    },
    unmatch: function () {
        $('#headerImage').attr("src", "/Images/seuss/payment4for99winter/header.jpg");
        $('#rightTopDesktop').attr("src", "/Images/seuss/payment4for99winter/right_top1.png");
    }
})
//end of interchange

