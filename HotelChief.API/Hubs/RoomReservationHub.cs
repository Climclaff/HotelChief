namespace HotelChief.API.Hubs
{
    using System.Collections.Concurrent;
    using System.Security.Claims;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.AspNetCore.SignalR;

    public class RoomReservationHub : Hub
    {
        private static readonly object _lockObject = new object();
        public static Dictionary<string, List<string>> ConnectedUsers = new ();

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                lock (_lockObject)
                {
                    if (!ConnectedUsers.ContainsKey(userId))
                    {
                        ConnectedUsers[userId] = new ();
                    }

                    ConnectedUsers[userId].Add(connectionId);
                }
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? ex)
        {
            var connectionId = Context.ConnectionId;
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                lock (_lockObject)
                {
                    if (ConnectedUsers.ContainsKey(userId))
                    {
                        ConnectedUsers[userId].Remove(connectionId);
                        if (ConnectedUsers[userId].Count == 0)
                        {
                            ConnectedUsers.Remove(userId);
                        }
                    }
                }
            }

            return base.OnDisconnectedAsync(ex);
        }

        public async Task UpdateAvailableRooms()
        {
            await Clients.All.SendAsync("UpdateAvailableRooms");
        }
    }
}
