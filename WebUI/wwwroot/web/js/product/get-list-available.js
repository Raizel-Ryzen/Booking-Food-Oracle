(function ($) {
    'use strict';

    let getListProductAvailable = function () {
        let rowOfPage = 100000;
        let orderBy = 'CreatedAt';
        let orderType = 'Desc';
        let searchText = getLocalStorage(constVariableLocalStorage.SearchText);
        let categoryParam = '&CategoryId=' + getLocalStorage(constVariableLocalStorage.SearchCategory);
        let basePath = '/Product/GetListProduct?PageNumber=1&RowOfPage='
        let url = basePath + rowOfPage + '&OrderBy=' + orderBy + '&IsOrderByOnly=1&OrderType=' + orderType + '&SearchText=' + searchText + categoryParam;
        
        coreAjaxGetPartialView(true, url, null, function (response) {
            $("#div-product-list").html(response);
        });
    }

    let getListCategoryLeftBar = function () {
        let rowOfPage = 100000;
        let pageParam = '?PageNumber=1&RowOfPage=' + rowOfPage;
        let basePath = '/Category/GetListCategoryLeftBar'
        let url = basePath + pageParam;

        coreAjaxGetPartialView(true, url, null, function (response) {
            $("#div-category-list").html(response);
        });
    }
    
    //Load functions
    $(document).ready(function () {
        getListProductAvailable();
        getListCategoryLeftBar();
    });

})(jQuery);
