(function ($) {
    'use strict';

    let getListTable = function () {
        let url = '/Table/GetListTable?PageNumber=1&RowOfPage=1000000&Status=0'
        
        coreAjaxGetPartialView(true, url, null, function (response) {
            $("#div-table-list").html(response);
        });
    }

    let getListStatusLeftBar = function () {
        let url = '/Table/GetListStatusLeftBar'

        coreAjaxGetPartialView(true, url, null, function (response) {
            $("#div-status-list").html(response);
        });
    }
    
    //Load functions
    $(document).ready(function () {
        getListTable();
        getListStatusLeftBar();
    });

})(jQuery);
