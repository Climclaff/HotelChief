namespace HotelChief.Core.Interfaces.IServices
{
    using HotelChief.Core.Entities.Abstract;

    public interface ILoyaltyService<T>
        where T : Booking
    {
        Task AssignLoyaltyPointsAsync(T paymentActivity, int userId);

        Task<T?> ApplyDiscountAsync(T paymentActivity, int userId);

        Task CommitAsync();
    }
}
