//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
    match: function () {
        $('#headerImage').attr("src", "images/mobile_headerfall.png");
        $('#rightTopDesktop').attr("src", "images/mobile_form_top.png");

    },
    unmatch: function () {
        $('#headerImage').attr("src", "images/header.jpg");
        $('#rightTopDesktop').attr("src", "images/right_top1.png");
    }
})
//end of interchange

