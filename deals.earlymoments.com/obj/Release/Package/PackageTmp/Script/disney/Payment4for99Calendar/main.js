//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
    match: function () {
        $('#headerImage').attr("src", "/images/disney/Payment4for99calendar/mobile_headerfall.png");
        $('#step1').attr("src", "/images/disney/Payment4for99calendar/step1.png");

    },
    unmatch: function () {
        $('#headerImage').attr("src", "/images/disney/Payment4for99calendar/header.jpg");
        $('#step1').attr("src", "/images/disney/Payment4for99calendar/step1.png");
    }
})

//end of interchange
