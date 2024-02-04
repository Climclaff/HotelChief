namespace HotelChief.Core.Interfaces.IRepositories
{
    public interface IGuestRepository
    {
        Task RemoveGuestReviewVotesAsync(int guestId);

        Task RemoveEmployeeInfoAsync(int guestId);
    }
}
