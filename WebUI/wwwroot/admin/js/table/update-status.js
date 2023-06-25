(function ($) {
    'use strict';
    
    let bindingControl = function () {
        addClassActiveNav('none');
        
        $('#btn-update-status').on('click', function (e) {
            e.preventDefault();
            let check = true;
            let data = {
                Id: $('#TableId').val(),
                Status: $('#TableStatus').val(),
            };
            coreAjax(check, '/Admin/Table/SubmitUpdateStatus', JSON.stringify(data), 'PUT', function (res) {
                notifyMessage('success', res.message);
                coreReloadDatatable('table-datatable');
                $('#modal-action').modal('hide')
            });
        });
        
    }
    
    //Load functions
    $(document).ready(function () {
        bindingControl();
    });

})(jQuery);
