using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using webcam_solution_backend.models;

namespace webcam_solution_backend.hubs
{
    public class SignalrWebrtc : Hub
    {

        public async Task NewUser(string username)
        {
            var userInfo = new UserInfo() { userName = username, connectionId = Context.ConnectionId };
            await Clients.Others.SendAsync("NewUserArrived", JsonSerializer.Serialize(userInfo));
        }
        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            await Clients.All.SendAsync("UserDisconnect", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendSignal(string signal, string userTocall, string callFromUserName)
        {
            var webRTCSignalCall = new WebRTCSignalCall()
            {
                callFrom = Context.ConnectionId,
                userTocall = userTocall,
                signal = signal,
                callFromUserName = callFromUserName
            };
            await Clients.Client(userTocall).SendAsync("SendSignal", JsonSerializer.Serialize(webRTCSignalCall));
        }

        public async Task CallAccepted(string caller, string signal)
        {
            await Clients.Client(caller).SendAsync("CallAccepted", signal);
        }

        
    }
}
