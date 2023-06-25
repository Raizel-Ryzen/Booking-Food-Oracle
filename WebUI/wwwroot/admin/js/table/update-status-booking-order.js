(function ($) {
    'use strict';
    
    let bindingControl = function () {
        addClassActiveNav('none');
        
        $('#btn-update-status').on('click', function (e) {
            e.preventDefault();
            let check = true;
            let data = {
                Id: $('#TableBookingId').val(),
                Status: $('#TableBookingStatus').val(),
            };
            coreAjax(check, '/Admin/Table/SubmitUpdateTableBookingStatus', JSON.stringify(data), 'PUT', function (res) {
                notifyMessage('success', res.message);
                coreReloadDatatable('table-booking');
                $('#modal-action').modal('hide')
            });
        });
        
    }
    
    //Load functions
    $(document).ready(function () {
        bindingControl();
    });

})(jQuery);
