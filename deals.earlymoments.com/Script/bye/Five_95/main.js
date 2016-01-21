
//for the cols interchange		
enquire.register("screen and (max-width:767px)", {
    match: function () {
        $('#formProduct').attr("src", "/images/disney/4for99/form_product_sm.jpg");
        $('#goUp').insertBefore('#goDown');
    },
    unmatch: function () {
        $('#formProduct').attr("src", "/images/disney/4for99/form_product.jpg");
        $('#goUp').insertAfter('#goDown');
    }
})
//end of interchange