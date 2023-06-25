(function ($) {
    'use strict';

    let handleRealtimeMessage = function () {
        let currentUserId = $('#SessionId').val();
        let realtimeType = constRealtimeType.BookingTableRequest;

        appReceiveMessageRealtime(function (receiverId, senderId, type, message, objectId) {
            if (equalsIgnoreCase(currentUserId, receiverId) && equalsIgnoreCase(realtimeType, type)){
                if (equalsIgnoreCase(constBookingTableStatus.Approved, message)){
                    notifyMessage('success', 'Bàn bạn đặt đã được xác nhận');
                }else if (equalsIgnoreCase(constBookingTableStatus.Reject, message)){
                    notifyMessage('error', 'Bàn bạn đặt đã bị hủy');
                }
            }
        })
    }

    //Load functions
    $(document).ready(function () {
        handleRealtimeMessage();
    });

})(jQuery);