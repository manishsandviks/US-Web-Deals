//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
    match: function () {
        $('#headerImage').attr("src", "/Images/seuss/4for1/mobile_headerfall.png");
        $('#step1').attr("src", "/Images/seuss/4for1/step1Mob.png");

    },
    unmatch: function () {
        $('#headerImage').attr("src", "/Images/seuss/4for1/top.gif");
        $('#step1').attr("src", "/Images/seuss/4for1/step1.png");
    }
})
//end of interchange
