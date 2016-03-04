$(document).ready(function () {
    //for the cols interchange		
    enquire.register("screen and (max-width:767px)", {
        match: function () {
            $('#headerImage').attr("src", "/Images/seuss/4for99spring/mobile_headerfall.png");
            $('#step1').attr("src", "/Images/seuss/4for99spring/step1Mob.png");

        },
        unmatch: function () {
            $('#headerImage').attr("src", "/Images/seuss/4for99spring/top.gif");
            $('#step1').attr("src", "/Images/seuss/4for99spring/step1.png");
        }
    })
    //end of interchange
});