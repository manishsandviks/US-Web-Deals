$(document).ready(function () {
    //for the cols interchange		
    enquire.register("screen and (max-width:767px)", {
        match: function () {
            $('#headerImage').attr("src", "/Images/seuss/4for1winter/mobile_headerfall.png");
            $('#step1').attr("src", "/Images/seuss/4for1winter/step1Mob.png");

        },
        unmatch: function () {
            $('#headerImage').attr("src", "/Images/seuss/4for1winter/top.gif");
            $('#step1').attr("src", "/Images/seuss/4for1winter/step1.png");
        }
    })
    //end of interchange
});