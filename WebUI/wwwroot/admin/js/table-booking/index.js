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
                    return `<span title="${e.tableName}">${e.tableName}</span>`
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
                    let date = new Date(e.receivedDate);
                    let strDate = date.getMonth() + 1 + '/' + date.getDay() + '/' + date.getFullYear()
                    return `<span title="${strDate}">${strDate}</span>`
                },
                width: '100px'
            },
            {
                data: function (e) {
                    let strTimeCome = e.intendTime + 'h ' + '(' + e.timeInfo + ')' 
                    return `<span title="${strTimeCome}">${strTimeCome}</span>`
                },
                width: '100px'
            },
            {
                data: function (e) {
                    let obj = switchStatusTableBooking(e.status);
                    return `<label title="${obj.text}" class="${obj.className}">${obj.text}</label>`
                },
                width: '10px'
            },
            {
                data: function (e) {
                    if (e.isEdit){
                        return `<div style="text-align: center;">
                            <i class="ri-eye-line hover-link" style="color: #007bff" title="Xem Chi Tiết" onclick="viewBookingTableDetail('${e.code}')"></i>
                        </div>`
                    }
                    return `<div style="text-align: center;">
                            <i class="ri-eye-line hover-link" style="color: #007bff" title="Xem Chi Tiết" onclick="viewBookingTableDetail('${e.code}')"></i>
                             | <i class="ri-pencil-line hover-link" style="color: #9ad717" title="Cập Nhật Trạng Thái" onclick="updateStatusTableBooking('${e.id}', '${e.status}', '${e.sessionId}')"></i>
                        </div>`
                },
                width: '40px'
            }
        ];

        let status = '0';
        if (window.location.pathname.toLowerCase() === '/admin'){
            status = '1';
        }
        
        coreDatatable('table-booking-datatable', '/Admin/TableBooking/GetListTableBooking?PageNumber=1&RowOfPage=1000000&Status=' + status, 'GET', false, true, columns, null);
    }

    let bindingControl = function () {
        if (window.location.pathname.toLowerCase() === '/admin'){
            addClassActiveNav('nav-dashboard')
        }else{
            addClassActiveNav('nav-table-booking')
        }

        $('#btn-search-filter').on('click', function () {
            let statusId = $('#StatusOption').val();
            coreReloadDatatable('table-booking-datatable', '/Admin/TableBooking/GetListTableBooking?PageNumber=1&RowOfPage=1000000&Status=' + statusId)
        })
    }

    let loadDropdownData = function () {
        singleSelectDropdown('StatusOption', 'Chọn trạng thái bạn muốn tìm', toListStatusTableBookingOption(), null);
    }

    let handleRealtimeMessage = function () {
        let realtimeType = constRealtimeType.BookingTable;

        appReceiveMessageRealtime(function (receiverId, senderId, type, message, objectId) {
            if (equalsIgnoreCase(realtimeType, type)){
                coreReloadDatatable('table-booking-datatable');
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
