(function ($) {
    'use strict';
    
    let bindingControl = function () {
        addClassActiveNav('nav-category');
        
        $('#btn-add-new-category').on('click', function (e) {
            e.preventDefault();
            let check = true;
            let data = {
                Title: $('#CategoryName').val(),
            };

            if (isNullOrEmpty(data.Title)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Tên Thể Loại')
            }

            coreAjax(check, '/Admin/Category/SubmitAddNewCategory', JSON.stringify(data), 'POST', function (res) {
                notifyMessage('success', res.message);
                setTimeout(function () {
                    window.location.href = '/Admin/Category';
                }, 1000)
            });
        });
        
    }
    
    //Load functions
    $(document).ready(function () {
        bindingControl();
    });

})(jQuery);
