let constRealtimeUrl = '/realtime';

let constRealtimeMethod = 'ReceiveMessage';

let constRealtimeType = {
    Product: 'Product',
    Shop: 'Shop',
    BookingOrder: 'BookingOrder',
    BookingTable: 'BookingTable',
    BookingTableRequest: 'BookingTableRequest',
};

let constBookingTableStatus = {
    AwaitingReview: 1,
    Approved: 2,
    Reject: 3,
};

const realtimeConnection = new signalR.HubConnectionBuilder()
    .withUrl(constRealtimeUrl)
    .configureLogging(signalR.LogLevel.Information)
    .build();

realtimeConnection.start().then(function (){
    console.log('listening...');
}).catch(function(err) {
    console.error(err.toString());
});


let appReceiveMessageRealtime = function (callBack) {
    realtimeConnection.on(constRealtimeMethod, (receiverIds, senderId, type, message, objectId) => {
        console.log('Listen \ntype: ' + type + '\nmsg: ' + message + '\nreceiverIds: ' + receiverIds + '\nsenderId: ' + senderId + '\nobjectId: ' + objectId)
        if (callBack){
            callBack(receiverIds, senderId, type, message, objectId)
        }
    });
}

let appSendMessageRealtime = function (receiverIds, senderId, type, message, objectId, callBack) {
    realtimeConnection.invoke(constRealtimeMethod, receiverIds, senderId, type, message, objectId).then(function() {
        console.log('Sent \ntype: ' + type + '\nmsg: ' + message + '\nreceiverIds: ' + receiverIds + '\nsenderId: ' + senderId + '\nobjectId: ' + objectId)
        if (callBack){
            callBack(receiverIds, senderId, type, message, objectId)
        }
    }).catch(function(err) {
        console.error(err.toString());
    });
}