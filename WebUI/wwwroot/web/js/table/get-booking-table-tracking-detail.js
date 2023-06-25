(function ($) {
    'use strict';

    let bindControls = function () {
        $("#booking-table-tracking-code").keyup(function () {
            let val = $(this).val()
            if (val){
                $(this).val(val.toUpperCase());
            }
        });

        $('#btn-get-booking-table-tracking').on('click', function () {
            let code = $("#booking-table-tracking-code").val();
            if (code){
                viewBookingTableDetail(code.toUpperCase())
            }else{
                notifyMessage('error', 'Vui lòng nhập Mã Đơn Hàng')
            }
        })
    }
    
    //Load functions
    $(document).ready(function () {
        bindControls();
    });

})(jQuery);
