(function ($) {
    'use strict';

    let initCategoryTable = function () {
        let columns = [
            {
                data: function (e) {
                    let newTitle = truncateString(e.title, 35);
                    return `<span title="${e.title}">${newTitle}</span>`
                },
                width: '140px'
            },
            {
                data: function (e) {
                    return `<span title="${e.code}">${formatNumber(e.code)}</span>`
                },
                width: '55px'
            },
            {
                data: function (e) {
                    return `<div style="text-align: center;">
                              <i class="ri-pencil-line hover-link" style="color: #0a03ff" title="Chỉnh Sửa" onclick="onClickLink(true, '/Admin/Table/Detail?id=${e.id}')"></i> 
                            | <i class="ri-delete-bin-line hover-link" style="color: red" title="Xóa" onclick="deleteCategory('${e.id}')"></i>
                        </div>`
                },
                width: '40px'
            }
        ];

        coreDatatable('category-datatable', '/Admin/Category/GetListCategory?PageNumber=1&RowOfPage=1000000&Status=0', 'GET', false, true, columns, null);
    }

    let bindingControl = function () {
        addClassActiveNav('nav-category');
    }
    
    //Load functions
    $(document).ready(function () {
        initCategoryTable();
        bindingControl();
    });

})(jQuery);
