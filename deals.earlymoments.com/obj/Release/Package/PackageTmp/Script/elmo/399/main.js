

//scroll
function goToByScroll(id) {
    id = id.replace("link", "");
    $('html,body').animate({
        scrollTop: $("#" + id).offset().top
    },
		'slow');
}
$("#aboutlink").click(function (e) {
    e.preventDefault();
    goToByScroll($(this).attr("id"));
});