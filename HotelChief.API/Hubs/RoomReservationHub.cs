namespace HotelChief.API.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class RoomReservationHub : Hub
    {
        public async Task UpdateAvailableRooms()
        {
            await Clients.All.SendAsync("UpdateAvailableRooms");
        }
    }
}
