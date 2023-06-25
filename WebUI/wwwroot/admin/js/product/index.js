(function ($) {
    'use strict';

    let initProductTable = function () {
        let columns = [
            {
                data: function (e) {
                    let newCode = e.code.toUpperCase();
                    return `<span title="${e.newCode}">${newCode}</span>`
                },
                width: '20px'
            },
            {
                data: function (e) {
                    let newTitle = truncateString(e.title, 35);
                    return `<span title="${e.title}">${newTitle}</span>`
                },
                width: '140px'
            },
            {
                data: function (e) {
                    return `<span title="${formatCurrencyVND(e.amount)}">${formatCurrencyVND(e.amount)}</span>`
                },
                width: '55px'
            },
            {
                data: function (e) {
                    return `<span title="${formatNumber(e.bought)}">${formatNumber(e.bought)}</span>`
                },
                width: '25px'
            },
            {
                data: function (e) {
                    let obj = switchStatusProduct(e.status);
                    return `<label title="${obj.text}" class="${obj.className}">${obj.text}</label>`
                },
                width: '10px'
            },
            {
                data: function (e) {
                    return `<img class="custom-img-border code-mega-border datatable-img" src="${e.thumbnail}" alt="thumbnail">`
                },
                width: '30px'
            },
            {
                data: function (e) {
                    return `<div style="text-align: center;">
                              <i class="ri-pencil-line hover-link" style="color: #9ad717" title="Cập Nhật Trạng Thái" onclick="updateStatusProduct('${e.id}', '${e.status}')"></i>
                            | <i class="ri-pencil-line hover-link" style="color: #0a03ff" title="Chỉnh Sửa" onclick="onClickLink(true, '/Admin/Product/Detail?id=${e.id}')"></i> 
                            | <i class="ri-delete-bin-line hover-link" style="color: red" title="Xóa" onclick="deleteProduct('${e.id}')"></i>
                        </div>`
                },
                width: '40px'
            }
        ];

        coreDatatable('product-datatable', '/Admin/Product/GetListProduct?PageNumber=1&RowOfPage=1000000&Status=0', 'GET', false, true, columns, null);
    }

    let bindingControl = function () {
        addClassActiveNav('nav-product');

        $('#btn-search-filter').on('click', function () {
            let statusId = $('#StatusOption').val();
            coreReloadDatatable('product-datatable', '/Admin/Product/GetListProduct?PageNumber=1&RowOfPage=1000000&Status=' + statusId)
        })
    }

    let loadDropdownData = function () {
        singleSelectDropdown('StatusOption', 'Chọn trạng thái bạn muốn tìm', toListStatusProductOption(), null);
    }

    //Load functions
    $(document).ready(function () {
        initProductTable();
        bindingControl();
        loadDropdownData();
    });

})(jQuery);
