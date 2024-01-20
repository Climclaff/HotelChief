namespace HotelChief.API.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize(AuthenticationSchemes = "oidc")]
    public class ReviewHub : Hub
    {
        public async Task UpdateVotes(int reviewId, int upvotes, int downvotes)
        {
            await Clients.All.SendAsync("UpdateVotes", reviewId, upvotes, downvotes);
        }
    }
}
