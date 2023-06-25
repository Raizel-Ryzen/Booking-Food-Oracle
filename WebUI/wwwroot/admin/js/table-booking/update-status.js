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
            coreAjax(check, '/Admin/TableBooking/SubmitUpdateStatus', JSON.stringify(data), 'PUT', function (res) {
                notifyMessage('success', res.message);
                let sessionId = $('#SessionId').val();
                
                appSendMessageRealtime(sessionId, 'admin', constRealtimeType.BookingTableRequest, data.Status.toString(), '');
                
                coreReloadDatatable('table-booking-datatable');
                $('#modal-action').modal('hide')
            });
        });
        
    }
    
    //Load functions
    $(document).ready(function () {
        bindingControl();
    });

})(jQuery);
