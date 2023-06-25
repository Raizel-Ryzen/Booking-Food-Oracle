(function ($) {
    'use strict';

    let bindEvents = function () {
        let searchLocal = getLocalStorage('searchText');

        $("#SearchText").val(searchLocal);

        $("#SearchText").keyup(function () {
            let val = $(this).val()
            setLocalStorage(constVariableLocalStorage.SearchText, val);
        });

        $('#btn-search').on('click', function () {
            let path = window.location.pathname;
            if (path === '' || path === '/') {
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
            } else {
                window.location.href = '/'
            }
        })
    }
    
    let loadUserSearchOption = function () {
        let category = getLocalStorage(constVariableLocalStorage.SearchCategoryTitle);

        if (category){
            $('#title-category-select').text('Thể Loại: ' + category);
        }else{
            $('#title-category-select').text('Thể Loại');
        }
    }

    //Load functions
    $(document).ready(function () {
        bindEvents();
        loadUserSearchOption();
    });

})(jQuery);
