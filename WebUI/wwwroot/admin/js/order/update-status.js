(function ($) {
    'use strict';
    
    let bindingControl = function () {
        addClassActiveNav('none');
        
        $('#btn-update-status').on('click', function (e) {
            e.preventDefault();
            let check = true;
            let data = {
                Id: $('#OrderId').val(),
                Status: $('#OrderStatus').val(),
            };
            coreAjax(check, '/Admin/Order/SubmitUpdateStatus', JSON.stringify(data), 'PUT', function (res) {
                notifyMessage('success', res.message);
                coreReloadDatatable('order-datatable');
                $('#modal-action').modal('hide')
            });
        });
        
    }
    
    //Load functions
    $(document).ready(function () {
        bindingControl();
    });

})(jQuery);
