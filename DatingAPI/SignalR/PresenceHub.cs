using System;
using System.Threading.Tasks;
using DatingAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DatingAPI.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _presenceTracker;
        public PresenceHub(PresenceTracker presenceTracker)
        {
            _presenceTracker = presenceTracker;
        }

        public override async Task OnConnectedAsync()
        {
            var isOnline = await _presenceTracker.UserConnected(Context.User.GetUserName(),Context.ConnectionId);
            if(isOnline)
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUserName());

            var onlineUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers",onlineUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
           var isOffline = await _presenceTracker.UserDisconnected(Context.User.GetUserName(),Context.ConnectionId);
           if(isOffline)
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUserName());

            await base.OnDisconnectedAsync(exception);
            //  var onlineUsers = await _presenceTracker.GetOnlineUsers();
            // await Clients.All.SendAsync("GetOnlineUsers",onlineUsers);
        }
    }
}