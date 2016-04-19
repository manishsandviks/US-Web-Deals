﻿$(document).ready(function () {
    $("html, body").animate({ scrollTop: 0 }, "slow");
    //Submit Button Click
    $("#btnaddupsell").click(function () {
        $("form").submit();
        pleaseWait();
    });
    $("#btnaddupsell2").click(function () {
        $("form").submit();
        pleaseWait();
    });
    $("#btnNoUpsell").click(function () {
        $("form").submit();
        pleaseWait();
    });
});

function pleaseWait() {
    $("#loading").fadeIn();
    var opts = {
        lines: 12, // The number of lines to draw
        length: 7, // The length of each line
        width: 4, // The line thickness
        radius: 10, // The radius of the inner circle
        color: '#000', // #rgb or #rrggbb
        speed: 1, // Rounds per second
        trail: 60, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false // Whether to use hardware acceleration
    };
    var target = document.getElementById('loading');
    var spinner = new Spinner(opts).spin(target);
}


