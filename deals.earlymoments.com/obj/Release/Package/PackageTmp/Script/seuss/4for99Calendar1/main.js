//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
    match: function () {
        $('#headerImage').attr("src", "/Images/seuss/4for99Calendar1/mobile_headerfall.png");
        $('#step1').attr("src", "/Images/seuss/4for99Calendar1/step1.png");

    },
    unmatch: function () {
        $('#headerImage').attr("src", "/Images/seuss/4for99Calendar1/top.gif");
        $('#step1').attr("src", "/Images/seuss/4for99Calendar1/step1.png");
    }
})

//end of interchange
