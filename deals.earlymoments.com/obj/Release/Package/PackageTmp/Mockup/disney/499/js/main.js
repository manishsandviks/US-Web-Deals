// JavaScript Document

//prettyphoto
$(document).ready(function(){
	$("[rel^='lightbox']").prettyPhoto();
	$("a[rel^='lightbox']").prettyPhoto({social_tools:false});
});

//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
	match : function() { 
			//$('#headerImage').attr("src", "images/mobile_headerfall.png");
			$('#step1').attr("src", "images/step1.png");
			
	},
	unmatch : function() {
			//$('#headerImage').attr("src", "images/header.jpg");
			$('#step1').attr("src", "images/step1.png");
	}
})

//end of interchange

//date picker
$(window).load(function(){
	$(".date-picker").datepicker();
	$(".date-picker").on("change", function () {
	    var id = $(this).attr("id");
	    var val = $("label[for='" + id + "']").text();
	    $("#msg").text(val + " changed");
	});
});