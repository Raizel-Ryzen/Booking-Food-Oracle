(function ($) {
    'use strict';
    
    let bindingControl = function () {
        addClassActiveNav('none');
        
        $('#btn-update-status').on('click', function (e) {
            e.preventDefault();
            let check = true;
            let data = {
                Id: $('#ProductId').val(),
                Status: $('#ProductStatus').val(),
            };
            coreAjax(check, '/Admin/Product/SubmitUpdateStatus', JSON.stringify(data), 'PUT', function (res) {
                notifyMessage('success', res.message);
                coreReloadDatatable('product-datatable');
                $('#modal-action').modal('hide')
            });
        });
        
    }
    
    //Load functions
    $(document).ready(function () {
        bindingControl();
    });

})(jQuery);
