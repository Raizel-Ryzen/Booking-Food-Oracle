"use strict";

let toListStatusProductOption = function () {
    return [
        {
            id: "0",
            text: "Tất Cả Trạng Thái"
        },
        {
            id: "1",
            text: "Còn Hàng"
        },
        {
            id: "2",
            text: "Đã Hết"
        },
    ]
}

let toListStatusTableOption = function () {
    return [
        {
            id: "0",
            text: "Tất Cả Trạng Thái"
        },
        {
            id: "1",
            text: "Còn Trống"
        },
        {
            id: "2",
            text: "Đã Được Đặt"
        },
        {
            id: "3",
            text: "Yêu Cầu Thanh Toán"
        },
        {
            id: "4",
            text: "Yêu Cầu Gọi Món"
        },
    ]
}

let toListStatusTableBookingOption = function () {
    return [
        {
            id: "0",
            text: "Tất Cả Trạng Thái"
        },
        {
            id: "1",
            text: "Đang Chờ Xác Nhận"
        },
        {
            id: "2",
            text: "Đã Được Xác Nhận"
        },
        {
            id: "3",
            text: "Đã Từ Chối"
        },
    ]
}

let addClassActiveNav = function (idNav) {
    $('#nav-dashboard').removeClass('active');
    $('#nav-table').removeClass('active');
    $('#nav-product').removeClass('active');
    $('#nav-category').removeClass('active');
    $('#nav-order').removeClass('active');
    $('#nav-bill').removeClass('active');
    $('#nav-page-user').removeClass('active');
    $('#nav-table-booking').removeClass('active');
    
    $('#' + idNav).addClass('active');
}

let constStatusProduct = {
    Available: 1,
    NotAvailable: 2,
};

let switchStatusProduct = function (statusCode) {
    let obj = {};
    switch (statusCode) {
        case constStatusProduct.Available:
            obj = {
                className: 'label-status-success',
                text: 'Còn Hàng'
            };
            break;
        case constStatusProduct.NotAvailable:
            obj = {
                className: 'label-status-danger',
                text: 'Đã Hết'
            };
            break;
    }

    return obj;
}

let constStatusTable = {
    Available: 1,
    NotAvailable: 2,
    RequestPay: 3,
    RequestOrder: 4,
};

let constStatusTableBooking = {
    AwaitingReview: 1,
    Approved: 2,
    Reject: 3,
};

let switchStatusTable = function (statusCode) {
    let obj = {};
    switch (statusCode) {
        case constStatusTable.Available:
            obj = {
                className: 'label-status-success',
                text: 'Còn Trống'
            };
            break;
        case constStatusTable.NotAvailable:
            obj = {
                className: 'label-status-danger',
                text: 'Đã Được Đặt'
            };
            break;
        case constStatusTable.RequestPay:
            obj = {
                className: 'label-status-warning',
                text: 'Yêu Cầu Thanh Toán'
            };
            break;
        case constStatusTable.RequestOrder:
            obj = {
                className: 'label-status-warning',
                text: 'Yêu Cầu Gọi Món'
            };
            break;
    }

    return obj;
}

let switchStatusTableBooking = function (statusCode) {
    let obj = {};
    switch (statusCode) {
        case constStatusTableBooking.AwaitingReview:
            obj = {
                className: 'label-status-warning',
                text: 'Đang Chờ Xác Nhận'
            };
            break;
        case constStatusTableBooking.Approved:
            obj = {
                className: 'label-status-success',
                text: 'Đã Xác Nhận Yêu Cầu'
            };
            break;
        case constStatusTableBooking.Reject:
            obj = {
                className: 'label-status-danger',
                text: 'Đã Hủy Yêu Cầu'
            };
            break;
    }

    return obj;
}

let updateStatusProduct = function (productId, currentStatus) {
    coreAjaxPostPartialView(true, '/Admin/Product/UpdateStatus', JSON.stringify({ Id: productId, Status: currentStatus}), function (res) {
        $('#content-modal-action').html(res)
        $('#modal-action').modal('show')
    });
}

let deleteProduct = function (id) {
    coreAjaxPostPartialView(true, '/Admin/Product/Delete', JSON.stringify({ Id: id}), function (res) {
        $('#content-modal-action').html(res)
        $('#modal-action').modal('show')
    });
}

let deleteCategory = function (id) {
    coreAjaxPostPartialView(true, '/Admin/Category/Delete', JSON.stringify({ Id: id}), function (res) {
        $('#content-modal-action').html(res)
        $('#modal-action').modal('show')
    });
}

let deleteTable = function (id) {
    coreAjaxPostPartialView(true, '/Admin/Table/Delete', JSON.stringify({ Id: id}), function (res) {
        $('#content-modal-action').html(res)
        $('#modal-action').modal('show')
    });
}

let updateStatusTable = function (tableId, currentStatus) {
    coreAjaxPostPartialView(true, '/Admin/Table/UpdateStatus', JSON.stringify({ Id: tableId, Status: currentStatus}), function (res) {
        $('#content-modal-action').html(res)
        $('#modal-action').modal('show')
    });
}

let updateStatusTableBooking = function (tableBookingId, currentStatus, sessionId) {
    coreAjaxPostPartialView(true, '/Admin/TableBooking/UpdateStatus', JSON.stringify({ Id: tableBookingId, Status: currentStatus, SessionId: sessionId}), function (res) {
        $('#content-modal-action').html(res)
        $('#modal-action').modal('show')
    });
}

let updateStatusOrder = function (orderId, currentStatus) {
    coreAjaxPostPartialView(true, '/Admin/Order/UpdateStatus', JSON.stringify({ Id: orderId, Status: currentStatus}), function (res) {
        $('#content-modal-action').html(res)
        $('#modal-action').modal('show')
    });
}

let resetDefaultStatusTable = function (tableId) {
    let data = {
        Id: tableId,
        Status: 1,
    };
    coreAjax(true, '/Admin/Table/SubmitUpdateStatus', JSON.stringify(data), 'PUT', function (res) {
        notifyMessage('success', res.message);
        coreReloadDatatable('table-datatable');
    });
}

let constStatusOrder = {
    AwaitingReview: 1,
    Paid: 2,
    Shipping: 3,
    Cancel: 4,
};

let switchStatusOrder = function (statusCode) {
    let obj = {};
    switch (statusCode) {
        case constStatusOrder.AwaitingReview:
            obj = {
                className: 'label-status-warning',
                text: 'Chờ Duyệt Đơn'
            };
            break;
        case constStatusOrder.Paid:
            obj = {
                className: 'label-status-success',
                text: 'Đã Thanh Toán'
            };
            break;
        case constStatusOrder.Shipping:
            obj = {
                className: 'label-status-info',
                text: 'Đang Ship'
            };
            break;
        case constStatusOrder.Cancel:
            obj = {
                className: 'label-status-danger',
                text: 'Đã Hủy'
            };
            break;
    }

    return obj;
}

let toListStatusOrderOption = function () {
    return [
        {
            id: "0",
            text: "Tất Cả Trạng Thái"
        },
        {
            id: "1",
            text: "Chờ Duyệt Đơn"
        },
        {
            id: "2",
            text: "Đã Thanh Toán"
        },
        {
            id: "3",
            text: "Đang Ship"
        },
        {
            id: "4",
            text: "Đã Hủy"
        },
    ]
}

let viewOrderDetail = function (code) {
    coreAjaxGetPartialView(true, '/Admin/Order/Detail?code=' + code, null, function (res) {
        $('#order-detail-content').html(res)
        $('#modal-order-detail').modal('show')
    });
}

let viewBookingTableDetail = function (code) {
    coreAjaxGetPartialView(true, '/Table/GetBookingTableTrackingDetail?code=' + code, null, function (res) {
        $('#booking-table-detail-content').html(res)
        $('#modal-booking-table-detail').modal('show')
    });
}