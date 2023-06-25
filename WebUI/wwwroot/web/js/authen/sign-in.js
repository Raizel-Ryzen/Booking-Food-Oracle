(function ($) {
    'use strict';

    let submitSignIn = function () {
        $('#btn-sign-in').on('click', function (e) {
            e.preventDefault();
            let check = true;
            let data = {
                Email: $('#Email').val(),
                Password: $('#Password').val()
            };
            
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
                , '/Authenticate/SignIn'
                , JSON.stringify(data)
                , 'POST'
                , function (res) {
                    notifyMessage('success', res.message);
                    let redirect = $("#redirectUrl").val();
                    
                    if ($("#redirectUrl").val() !== '') {
                        redirect = redirect.replaceAll("#", "");
                        setTimeout(function () {
                            window.location.href = redirect;
                        }, 1000);
                    } else {
                        setTimeout(function () {
                            window.location.href = "/";
                        }, 1000);
                    }
                }
                , function () { }
            );
        });
    }

    let initLogin = function () {
        if (window.location.pathname === '/sign-in' && window.location.hash !== '') {
            $("#redirectUrl").val(window.location.hash);
            setTimeout(function () {
                window.location.hash = '';
            }, 500);
        }
    }
    
    //Load functions
    $(document).ready(function () {
        submitSignIn();
        initLogin();
    });

})(jQuery);