(function ($) {
    'use strict';

    let handleOnChangeImage = function () {
        handleSelectImageByIdWithCallback('Image', function () {
            previewImage($('#Image')[0], 'preview-image');
        });
    }

    let bindingControl = function () {
        addClassActiveNav('nav-product');

        $("#Amount").keyup(function() {
            let val = $(this).val()
            if (val){
                let newVal = val.replaceAll('.', '');
                $(this).val(formatNumber(newVal))
            }
        });
        
        $('#btn-add-new-product').on('click', function (e) {
            e.preventDefault();
            let check = true;
            let formData = new FormData();
            let data = {
                Title: $('#ProductName').val(),
                Category: $('#Category').val(),
                Amount: $('#Amount').val(),
                Unit: $('#Unit').val(),
                Image: $('#Image')[0].files[0],
            };

            if (isNullOrEmpty(data.Title)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Tên Món Ăn')
            }
            if (isNullOrEmpty(data.Category)){
                check = false;
                notifyMessage('error', 'Vui lòng chọn Thể Loại Sản Phẩm')
            }
            if (isNullOrEmpty(data.Amount)){
                check = false;
                notifyMessage('error', 'Vui lòng điền Giá Bán')
            }
            if (isNullOrEmpty(data.Unit)){
                check = false;
                notifyMessage('error', 'Vui lòng chọn Đơn Vị Giá')
            }
            if (isNullOrEmpty(data.Image)){
                check = false;
                notifyMessage('error', 'Vui lòng chọn Hình Ảnh Sản Phẩm')
            }

            formData.append('Title', data.Title);
            formData.append('CategoryId', data.Category);
            formData.append('Amount', data.Amount.replaceAll('.', ''));
            formData.append('UnitId', data.Unit);
            formData.append('Image', data.Image);

            coreAjaxWithFormData(check, '/Admin/Product/SubmitAddNewProduct', formData, 'POST', function (res) {
                notifyMessage('success', res.message);
                setTimeout(function () {
                    window.location.href = '/Admin/Product';
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
