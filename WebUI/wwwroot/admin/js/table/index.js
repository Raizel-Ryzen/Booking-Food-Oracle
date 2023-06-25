(function ($) {
    'use strict';

    let initTable = function () {
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
                    return `<span title="${formatNumber(e.slotNumber)}">${formatNumber(e.slotNumber)}</span>`
                },
                width: '55px'
            },
            {
                data: function (e) {
                    let obj = switchStatusTable(e.status);
                    return `<label title="${obj.text}" class="${obj.className}">${obj.text}</label>`
                },
                width: '10px'
            },
            {
                data: function (e) {
                    return `<img class="custom-img-border code-mega-border datatable-img" src="${e.qrCode}" alt="thumbnail">`
                },
                width: '30px'
            },
            {
                data: function (e) {
                    return `<div style="text-align: center;">
                              <i class="ri-pencil-line hover-link" style="color: #9ad717" title="Cập Nhật Trạng Thái" onclick="updateStatusTable('${e.id}', '${e.status}')"></i>
                            | <i class="ri-refresh-line hover-link" style="color: #0a03ff" title="Đặt Lại Trạng Thái Ban Đầu" onclick="resetDefaultStatusTable('${e.id}')"></i> 
                            | <i class="ri-pencil-line hover-link" style="color: #0a03ff" title="Chỉnh Sửa" onclick="onClickLink(true, '/Admin/Table/Detail?id=${e.id}')"></i> 
                            | <i class="ri-delete-bin-line hover-link" style="color: red" title="Xóa" onclick="deleteTable('${e.id}')"></i>
                        </div>`
                },
                width: '40px'
            }
        ];

        coreDatatable('table-datatable', '/Admin/Table/GetListTable?PageNumber=1&RowOfPage=1000000&Status=0', 'GET', false, true, columns, null);
    }

    let bindingControl = function () {
        addClassActiveNav('nav-table');

        $('#btn-search-filter').on('click', function () {
            let statusId = $('#StatusOption').val();
            coreReloadDatatable('table-datatable', '/Admin/Table/GetListTable?PageNumber=1&RowOfPage=1000000&Status=' + statusId)
        })
    }

    let loadDropdownData = function () {
        singleSelectDropdown('StatusOption', 'Chọn trạng thái bạn muốn tìm', toListStatusTableOption(), null);
    }

    let handleRealtimeMessage = function () {
        let realtimeType = constRealtimeType.BookingTable;
        let realtimeType2 = constRealtimeType.BookingTableRequest;
        
        appReceiveMessageRealtime(function (receiverId, senderId, type, message, objectId) {
            if (equalsIgnoreCase(realtimeType, type) || equalsIgnoreCase(realtimeType2, type)){
                coreReloadDatatable('table-datatable');
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
