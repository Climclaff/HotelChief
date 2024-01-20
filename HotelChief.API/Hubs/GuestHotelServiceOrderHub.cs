namespace HotelChief.API.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize(AuthenticationSchemes = "oidc")]
    public class GuestHotelServiceOrderHub : Hub
    {
    }
}
