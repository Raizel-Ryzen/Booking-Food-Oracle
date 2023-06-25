(function ($) {
    'use strict';

    let handleOnChangeImage = function () {
        handleSelectImageByIdWithCallback('Image', function () {
            previewImage($('#Image')[0], 'preview-image');
        });
    }

    let bindingControl = function () {
        addClassActiveNav('nav-table');
        
        $('#btn-update-table').on('click', function (e) {
            e.preventDefault();
            let check = true;
            let data = {
                Title: $('#TableName').val(),
                SlotNumber: $('#SlotNumber').val(),
            };

            if (isNullOrEmpty(data.Title)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Tên Bàn Ăn')
            }
            if (isNullOrEmpty(data.SlotNumber)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Số Lượng Người Trên Bàn')
            }

            coreAjax(check, '/Admin/Table/SubmitUpdateTable', JSON.stringify(data), 'PUT', function (res) {
                notifyMessage('success', res.message);
                setTimeout(function () {
                    window.location.href = '/Admin/Table';
                }, 1000)
            });
        });
        
    }
    
    //Load functions
    $(document).ready(function () {
        bindingControl();
        handleOnChangeImage();
    });

})(jQuery);
