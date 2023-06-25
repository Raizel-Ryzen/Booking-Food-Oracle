(function ($) {
    'use strict';

    let checkoutProcess = function () {
        $('#btn-checkout').on('click', function (e) {
            e.preventDefault();
            let check = true;
            let data = {
                CustomerName: $('#CustomerName').val(),
                Address: $('#Address').val(),
                PhoneNumber: $('#PhoneNumber').val(),
            };

            if (isNullOrEmpty(data.CustomerName)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Tên Của Bạn')
            }
            if (isNullOrEmpty(data.Address)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Địa Chỉ')
            }
            if (isNullOrEmpty(data.PhoneNumber)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Số Điện Thoại')
            }
            coreAjax(check, '/Cart/SubmitCheckout', JSON.stringify(data), 'POST', function (res) {
                notifyMessage('success', res.message);
                appSendMessageRealtime('admin', 'guest', constRealtimeType.BookingOrder, 'Checkout', '')
                setTimeout(function () {
                    window.location.href = '/order-success?code=' + res.result;
                }, 2000)
            });
        })
    }
    
    
    //Load functions
    $(document).ready(function () {
        checkoutProcess();
    });

})(jQuery);
