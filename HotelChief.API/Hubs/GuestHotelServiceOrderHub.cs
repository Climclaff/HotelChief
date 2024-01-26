namespace HotelChief.API.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize(AuthenticationSchemes = "oidc")]
    public class GuestHotelServiceOrderHub : Hub
    {
        private static readonly object _lockObject = new object();
        public static Dictionary<string, List<string>> ConnectedUsers = new();

        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst("email")?.Value;
            var connectionId = Context.ConnectionId;
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
            var userId = Context.User?.FindFirst("email")?.Value;
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
    }
}
