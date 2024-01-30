namespace HotelChief.Core.Interfaces.IRepositories
{
    public interface IGuestRepository
    {
        Task RemoveGuestReviewVotes(int guestId);

        Task RemoveEmployeeInfo(int guestId);
    }
}
