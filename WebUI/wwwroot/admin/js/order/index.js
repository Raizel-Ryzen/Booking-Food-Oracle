(function ($) {
    'use strict';

    let initTable = function () {
        let columns = [
            {
                data: function (e) {
                    return `<span title="${e.code.toUpperCase()}">${e.code.toUpperCase()}</span>`
                },
                width: '100px'
            },
            {
                data: function (e) {
                    return `<span title="${e.customerName}">${e.customerName}</span>`
                },
                width: '100px'
            },
            {
                data: function (e) {
                    return `<span title="${e.phoneNumber}">${e.phoneNumber}</span>`
                },
                width: '100px'
            },
            {
                data: function (e) {
                    return `<span title="${formatNumber(e.totalAmount)}đ">${formatNumber(e.totalAmount)}đ</span>`
                },
                width: '100px'
            },
            {
                data: function (e) {
                    let obj = switchStatusOrder(e.status);
                    return `<label title="${obj.text}" class="${obj.className}">${obj.text}</label>`
                },
                width: '10px'
            },
            {
                data: function (e) {
                    return `<div style="text-align: center;">
                              <i class="ri-eye-line hover-link" style="color: #007bff" title="Xem Đơn" onclick="viewOrderDetail('${e.code}')"></i>
                              | <i class="ri-pencil-line hover-link" style="color: #9ad717" title="Cập Nhật Trạng Thái" onclick="updateStatusOrder('${e.id}', '${e.status}')"></i>
                        </div>`
                },
                width: '40px'
            }
        ];

        coreDatatable('order-datatable', '/Admin/Order/GetListOrder?PageNumber=1&RowOfPage=1000000&Status=0', 'GET', false, true, columns, null);
    }

    let bindingControl = function () {
        addClassActiveNav('nav-order');

        $('#btn-search-filter').on('click', function () {
            let statusId = $('#StatusOption').val();
            coreReloadDatatable('order-datatable', '/Admin/Order/GetListOrder?PageNumber=1&RowOfPage=1000000&Status=' + statusId)
        })
    }

    let loadDropdownData = function () {
        singleSelectDropdown('StatusOption', 'Chọn trạng thái bạn muốn tìm', toListStatusOrderOption(), null);
    }

    let handleRealtimeMessage = function () {
        let realtimeType = constRealtimeType.BookingOrder;

        appReceiveMessageRealtime(function (receiverId, senderId, type, message, objectId) {
            if (equalsIgnoreCase(realtimeType, type)){
                // coreReloadDatatable('order-datatable');
            }
        })
    }
    
    //Load functions
    $(document).ready(function () {
        initTable();
        bindingControl();
        loadDropdownData();
        handleRealtimeMessage();    
    });

})(jQuery);
