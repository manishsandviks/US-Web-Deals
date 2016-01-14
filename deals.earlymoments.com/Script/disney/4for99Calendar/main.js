// JavaScript Document

//prettyphoto
$(document).ready(function () {
    $("[rel^='lightbox']").prettyPhoto();
    $("a[rel^='lightbox']").prettyPhoto({ social_tools: false });
});

//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
    match: function () {
        //$('#headerImage').attr("src", "images/mobile_headerfall.png");
        $('#step1').attr("src", "/images/disney/4for99calendar/step1.png");

    },
    unmatch: function () {
        //$('#headerImage').attr("src", "images/header.jpg");
        $('#step1').attr("src", "/images/disney/4for99calendar/step1.png");
    }
})

//end of interchange
