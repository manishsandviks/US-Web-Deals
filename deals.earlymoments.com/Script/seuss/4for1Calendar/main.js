
$(document).ready(function () {
    //prettyphoto
    $("[rel^='lightbox']").prettyPhoto();
    $("a[rel^='lightbox']").prettyPhoto({ social_tools: false });


    //for the cols interchange		
    enquire.register("screen and (max-width:767px)", {
        match: function () {
            $('#headerImage').attr("src", "/Images/seuss/4for1Calendar/mobile_headerfall.png");
            $('#step1').attr("src", "/Images/seuss/4for1Calendar/step1.png");

        },
        unmatch: function () {
            $('#headerImage').attr("src", "/Images/seuss/4for1Calendar/header.jpg");
            $('#step1').attr("src", "/Images/seuss/4for1Calendar/step1.png");
        }
    })

    //end of interchange

});
