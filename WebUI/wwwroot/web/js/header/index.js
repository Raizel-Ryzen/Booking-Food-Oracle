(function ($) {
    'use strict';

    let getListCategoryMenu = function () {
        let rowOfPage = 100000;
        let pageParam = '?PageNumber=1&RowOfPage=' + rowOfPage;
        let basePath = '/Category/GetListCategoryMenu'
        let url = basePath + pageParam;

        coreAjaxGetPartialView(true, url, null, function (response) {
            $("#div-category-list-menu").html(response);
        });
    }

    //Load functions
    $(document).ready(function () {
        getListCategoryMenu();
    });

})(jQuery);