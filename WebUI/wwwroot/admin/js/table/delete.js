(function ($) {
    'use strict';

    let bindingControl = function () {
        addClassActiveNav('none');

        $('#btn-delete').on('click', function (e) {
            e.preventDefault();
            let check = true;
            let data = {
                Id: $('#DataId').val(),
            };
            coreAjax(check, '/Admin/Table/SubmitDelete', JSON.stringify(data), 'PUT', function (res) {
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
