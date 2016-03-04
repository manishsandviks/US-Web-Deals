//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
    match: function () {
        $('#headerImage').attr("src", "/images/disney/payment4for99bonus/mobile_headerfall.png");
        $('#step1').attr("src", "/images/disney/payment4for99bonus/step1.png");

    },
    unmatch: function () {
        $('#headerImage').attr("src", "/images/disney/payment4for99bonus/header.jpg");
        $('#step1').attr("src", "/images/disney/payment4for99bonus/step1.png");
    }
})

//end of interchange
