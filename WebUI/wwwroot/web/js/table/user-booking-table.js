(function ($) {
    'use strict';

    let bindControls = function () {
        $('#btn-submit-table-booking').on('click', function (e) {
            e.preventDefault();
            $('#msg-error-booking').text('')
            let check = true;
            let data = {
                CustomerName: $('#CustomerName').val(),
                Address: $('#Address').val(),
                PhoneNumber: $('#PhoneNumber').val(),
                TableId: $('#TableId').val(),
                IntendTime: $('#IntendTime').val(),
                ReceivedDate: $('#ReceivedDate').val(),
                TimeInfo: $('input[name="TimeInfo"][type="radio"]:checked').val()
            }
            if (isNullOrEmpty(data.CustomerName)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Tên Của Bạn')
                $('#msg-error-booking').text('Vui lòng điền Tên Của Bạn')
                return
            }
            if (isNullOrEmpty(data.PhoneNumber)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Số Điện Thoại Của Bạn')
                $('#msg-error-booking').text('Vui lòng điền Số Điện Thoại Của Bạn')
                return
            }
            if (isNullOrEmpty(data.IntendTime)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Giờ Dự Kiến Tới')
                $('#msg-error-booking').text('Vui lòng điền Giờ Dự Kiến Tới')
                return
            }
            if (isNullOrEmpty(data.ReceivedDate)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Ngày Nhận Bàn')
                $('#msg-error-booking').text('Vui lòng điền Ngày Nhận Bàn')
                return
            }
            $('#msg-error-booking').text('')
            coreAjax(check, '/Table/SubmitBookingTable', JSON.stringify(data), 'POST', function (res) {
                $('#modal-table-booking-detail').modal('hide')
                notifyMessage('success', res.message);
                appSendMessageRealtime('admin', 'client', constRealtimeType.BookingTable, 'booked', '')
                setTimeout(function () {
                    window.location.href = '/booking-table-success?code=' + res.result;
                }, 1500)
            })
        })
    }
    
    //Load functions
    $(document).ready(function () {
        bindControls();
    });

})(jQuery);
