(function ($) {
    'use strict';

    let getListCart = function () {
        let url = '/Cart/GetCartList'
        
        coreAjaxGetPartialView(true, url, null, function (response) {
            $("#cart-content").html(response);
        });
    }
    
    
    //Load functions
    $(document).ready(function () {
        getListCart();
    });

})(jQuery);
