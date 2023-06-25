using Microsoft.AspNetCore.SignalR;

namespace WebUI.CodeMegaSignalR;

public class CodeMegaSignalR : Hub
{
    public async Task ReceiveMessage(string receiverIds, string senderId, string type, string message, string objectId)
    {
        if (!string.IsNullOrEmpty(receiverIds) 
            && !string.IsNullOrEmpty(senderId) 
            && !string.IsNullOrEmpty(type))
        {
            await Clients.All.SendAsync("ReceiveMessage", receiverIds, senderId, type, message, objectId);
        }
    }
}