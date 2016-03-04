$(document).ready(function () {
    //for the cols interchange		
    enquire.register("screen and (max-width:767px)", {
        match: function () {
            $('#headerImage').attr("src", "/Images/seuss/4for99winter/mobile_headerfall.png");
            $('#step1').attr("src", "/Images/seuss/4for99winter/step1Mob.png");

        },
        unmatch: function () {
            $('#headerImage').attr("src", "/Images/seuss/4for99winter/top.gif");
            $('#step1').attr("src", "/Images/seuss/4for99winter/step1.png");
        }
    })
    //end of interchange
});