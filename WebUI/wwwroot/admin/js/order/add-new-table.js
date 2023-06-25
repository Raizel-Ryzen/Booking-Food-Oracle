(function ($) {
    'use strict';
    
    let bindingControl = function () {
        addClassActiveNav('nav-table');

        $("#SlotNumber").keyup(function() {
            let val = $(this).val()
            if (val){
                let newVal = val.replaceAll('.', '');
                $(this).val(formatNumber(newVal))
            }
        });
        
        $('#btn-add-new-table').on('click', function (e) {
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

            coreAjax(check, '/Admin/Table/SubmitAddNewTable', JSON.stringify(data), 'POST', function (res) {
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
    });

})(jQuery);
