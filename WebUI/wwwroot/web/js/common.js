"use strict";

let constStatus = {
    Reject: 1,
    Approved: 2,
    AwaitingApproval: 3,
};

let constVariableLocalStorage = {
    SearchText: "searchText",
    SearchCategory: "searchCategoryId",
    SearchCity: "searchCityId",
    SearchCityTitle: "searchCityTitle",
    SearchCategoryTitle: "searchCategoryTitle",
};

let constPaymentMethod = {
    Bank: 'Bank',
    Momo: 'Momo',
};

let toUrlOrderBy = function (orderBy, orderType) {
    let rowOfPage = 100000;
    let basePath = '/Product/GetListProduct?PageNumber=1&RowOfPage='
    let categoryParam = '&CategoryId=' + getLocalStorage(constVariableLocalStorage.SearchCategory);
    let searchText = getLocalStorage(constVariableLocalStorage.SearchText);
    return basePath + rowOfPage + '&OrderBy=' + orderBy + '&IsOrderByOnly=1&OrderType=' + orderType + '&SearchText=' + searchText + categoryParam;
}

let toUrlOrderByDefault = function (orderBy, orderType) {
    let rowOfPage = 100000;
    let basePath = '/Product/GetListProduct?PageNumber=1&RowOfPage='
    let searchText = getLocalStorage(constVariableLocalStorage.SearchText);
    return basePath + rowOfPage + '&OrderBy=' + orderBy + '&IsOrderByOnly=1&OrderType=' + orderType + '&SearchText=' + searchText;
}

let onClickCategory = function (dataId, dataCode, dataTitle) {
    let rowOfPage = 100000;
    let orderParam = '&OrderBy=CreatedAt&OrderType=Desc&IsOrderByOnly=0';
    let searchParam = '&SearchText=' + getLocalStorage(constVariableLocalStorage.SearchText);
    let categoryParam = '&CategoryId=' + dataCode;
    let pageIndex = '?PageNumber=1&RowOfPage=' + rowOfPage;
    let basePath = '/Product/GetListProduct'
    let url = basePath + pageIndex + orderParam + searchParam + categoryParam
    let path = window.location.pathname;

    setLocalStorage(constVariableLocalStorage.SearchCategory, dataCode);
    setLocalStorage(constVariableLocalStorage.SearchCategoryTitle, dataTitle);
    
    if (path === '' || path === '/'){
        coreAjaxGetPartialView(true, url, null, function (response) {
            $('#title-category-select').text('Thể Loại: ' + dataTitle);
            $("#div-product-list").html(response);
        });
    }else{
        window.location.href = '/'
    }
}

let onClickTopDeal = function () {
    let path = window.location.pathname;
    if (path === '' || path === '/'){
        coreAjaxGetPartialView(true, toUrlOrderByDefault('Bought', 'Desc'), null, function (response) {
            $("#div-product-list").html(response);
        });
    }else{
        window.location.href = '/'
    }
}

let onClickDealToday = function () {
    let path = window.location.pathname;
    if (path === '' || path === '/'){
        coreAjaxGetPartialView(true, toUrlOrderByDefault('CreatedAt', 'Desc'), null, function (response) {
            $("#div-product-list").html(response);
        });
    }else{
        window.location.href = '/'
    }
}

let addClassActiveNav = function (idNav) {
    $('#nav-logout').removeClass('active');
    $('#nav-change-password').removeClass('active');
    $('#nav-profile').removeClass('active');
    $('#nav-manage-deal-bought').removeClass('active');
    $('#nav-manage-deal').removeClass('active');
    $('#nav-register-shop').removeClass('active');
    $('#nav-shop-info').removeClass('active');
    
    $('#' + idNav).addClass('active');
}


let switchStatus = function (statusCode) {
    let obj = {};
    switch (statusCode) {
        case constStatus.Reject:
            obj = {
                className: 'label-status-danger',
                text: 'Đã Từ Chối'
            };
            break;
        case constStatus.Approved:
            obj = {
                className: 'label-status-success',
                text: 'Hoạt Động'
            };
            break;
        case constStatus.AwaitingApproval:
            obj = {
                className: 'label-status-warning',
                text: 'Chờ Xét Duyệt'
            };
            break;
    }

    return obj;
}

let letCheckLogin = function () {
    coreAjax(true, '/Common/CheckLogin', null, 'POST')
}

let onClickTableStatus = function (dataId, dataCode, dataTitle) {
    let url = '/Table/GetListTable?PageNumber=1&RowOfPage=1000000&Status=' + dataId
    let path = window.location.pathname;
    if (path === '' || path === '/'){
        window.location.href = '/table'
    }else{
        coreAjaxGetPartialView(true, url, null, function (response) {
            $('#title-status-select').text('Trạng Thái: ' + dataTitle);
            $("#div-table-list").html(response);
        });
    }
}

let onClickOrder = function (id, title, amount, image, unit) {
    let quantity = $('#' + id).val();
    let data = {
        Id: id,
        Quantity: quantity,
        Title: title,
        Amount: amount,
        Image: image,
        Unit: unit,
        Total: parseInt(amount) * parseInt(quantity)
    };
    coreAjax(true, '/Cart/AddToCart', JSON.stringify(data), 'POST', function (res) {
        notifyMessage('success', res.message);
        let path = window.location.pathname;
        if (path === '/cart'){
            let url = '/Cart/GetCartList'
            coreAjaxGetPartialView(true, url, null, function (response) {
                $("#cart-content").html(response);
            });
        }
    });
}

let onClickRemoveOrder = function (id) {
    let data = {
        Id: id
    };
    coreAjax(true, '/Cart/RemoveFromCart', JSON.stringify(data), 'POST', function (res) {
        notifyMessage('success', res.message);
        let path = window.location.pathname;
        if (path === '/cart'){
            let url = '/Cart/GetCartList'
            coreAjaxGetPartialView(true, url, null, function (response) {
                $("#cart-content").html(response);
            });
        }
    })
}

let viewOrderDetail = function (code) {
    coreAjaxGetPartialView(true, '/Order/GetOrderTrackingDetail?code=' + code, null, function (res) {
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

let onClickBookingTable = function (id, title) {
    let data = {
        Id: id
    };
    coreAjax(true, '/Table/BookingTable', JSON.stringify(data), 'POST', function (res) {
        notifyMessage('success', 'Bạn đã đặt ' + title + ' thành công. Vui lòng chờ trong giây lát');
        appSendMessageRealtime('admin', 'client', constRealtimeType.BookingTable, 'Booked', '');
        setTimeout(function () {
            //window.location.href = '/user-booking-table'
        }, 3000)
    });
}

let onClickBookingTablePopup = function (id) {
    coreAjaxGetPartialView(true, '/Table/TableBookingInfo?id=' + id, null, function (res) {
        $('#table-booking-detail-content').html(res);
        $('#modal-table-booking-detail').modal('show')
    })
}