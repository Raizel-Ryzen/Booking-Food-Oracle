(function ($) {
    'use strict';

    let updateRole = function () {
        $('#btn-submit-update-role').on('click', function (e) {
            e.preventDefault();
            let roles = [];
            let check = true;
            let data = {
                Id: $('#UserId').val()
            };
            
            $("input[name='Role']").each(function () {
                if ($(this).is(":checked")) {
                    roles.push($(this).val())
                }
            });

            if (isNullOrEmpty(data.Id)) {
                notifyMessage('error', 'Vui lòng kiểm tra Id')
                check = false;
            }
            if (roles.length === 0) {
                notifyMessage('error', 'Vui lòng chọn ít nhất 1 Quyền Cho Người Dùng')
                check = false;
            }

            if (check) {
                data.Roles = roles;
                coreAjax(
                    check
                    , '/Admin/User/SubmitUpdateRole'
                    , JSON.stringify(data)
                    , 'POST'
                    , function (res) {
                        notifyMessage('success', res.message);
                        setTimeout(function () {
                            window.location.reload();
                        }, 2000);
                    }
                );
            }
        })
    }
    
    let bindingControl = function () {
        addClassActiveNav('none');

        let totalInputs = $('input[name="Role"]').length;
        let totalCheckedLoad = $(':checkbox[name="Role"]:checked').length;
        
        if(totalCheckedLoad===totalInputs){
            $('#check-full-role').prop('checked', true);
        }else{
            $('#check-full-role').prop('checked', false);
        }
        
        $('input[name="Role"]').change(function(){
            let totalChecked = $(':checkbox[name="Role"]:checked').length;
            if(totalChecked===totalInputs){
                $('#check-full-role').prop('checked', true);
            }else{
                $('#check-full-role').prop('checked', false);
            }
        })

        $('#check-full-role').change(function(){
            let totalChecked = $(':checkbox[name="Role"]:checked').length;
            if(totalChecked===totalInputs){
                $('input[name="Role"]').each(function(){
                    let val1 = $(this).val();
                    if(val1.toLocaleLowerCase()!=='user'.toLocaleLowerCase()){
                        $(this).prop('checked', false);
                    }
                })
            }else{
                $('input[name="Role"]').each(function(){
                    let val2 = $(this).val();
                    if(val2.toLocaleLowerCase()!=='user'.toLocaleLowerCase()){
                        $(this).prop('checked', true);
                    }
                })
            }
        })


    }
    
    //Load functions
    $(document).ready(function () {
        bindingControl();
        updateRole();
    });

})(jQuery);
