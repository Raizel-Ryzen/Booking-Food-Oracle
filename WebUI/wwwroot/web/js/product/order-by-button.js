(function ($) {
    'use strict';

    let bindEvents = function () {
        $('#BoughtDesc').on('click', function () {
            coreAjaxGetPartialView(true, toUrlOrderBy('BoughtDesc', 'Desc'), null, function (response) {
                $("#div-product-list").html(response);
            });
        })
        $('#AmountAsc').on('click', function () {
            coreAjaxGetPartialView(true, toUrlOrderBy('Amount', 'Asc'), null, function (response) {
                $("#div-product-list").html(response);
            });
        })
        $('#AmountDesc').on('click', function () {
            coreAjaxGetPartialView(true, toUrlOrderBy('Amount', 'Desc'), null, function (response) {
                $("#div-product-list").html(response);
            });
        })
        $('#CreatedAtDesc').on('click', function () {
            coreAjaxGetPartialView(true, toUrlOrderBy('CreatedAt', 'Desc'), null, function (response) {
                $("#div-product-list").html(response);
            });
        })
    }

    //Load functions
    $(document).ready(function () {
        bindEvents();
    });

})(jQuery);
