namespace HotelChief.Core.Interfaces.IServices
{
    using HotelChief.Core.Entities.Abstract;

    public interface ILoyaltyService<T>
        where T : Booking
    {
        Task AssignLoyaltyPoints(T paymentActivity, int userId);

        Task<T?> ApplyDiscount(T paymentActivity, int userId);

        Task Commit();
    }
}
