(function ($) {
    'use strict';

    let bindControls = function () {
        $("#order-tracking-code").keyup(function () {
            let val = $(this).val()
            if (val){
                $(this).val(val.toUpperCase());
            }
        });
        
        $('#btn-get-order-tracking').on('click', function () {
            let code = $("#order-tracking-code").val();
            if (code){
                viewOrderDetail(code.toUpperCase())
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
