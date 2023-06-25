(function ($) {
    'use strict';

    var submitSignUp = function () {
        $('#btn-sign-up').on('click', function (e) {
            e.preventDefault();
            
            let check = true;
            let data = {
                FullName: $('#FullName').val(),
                Email: $('#Email').val(),
                Password: $('#Password').val()
            };

            if (isNullOrEmpty(data.FullName)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Họ Tên')
            }
            if (isNullOrEmpty(data.Email)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Email')
            }
            if (isNullOrEmpty(data.Password)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Mật Khẩu')
            }
            
            coreAjax(
                check
                , '/Authenticate/SignUp'
                , JSON.stringify(data)
                , 'POST'
                , function (res) {
                    notifyMessage('success', res.message);
                    setTimeout(function () {
                        window.location.reload();
                    }, 1000)
                }
                , function () { }
            );
        });
    }

    //Load functions
    $(document).ready(function () {
        submitSignUp();
    });

})(jQuery);