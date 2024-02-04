namespace HotelChief.Core.Interfaces.IServices
{
    using HotelChief.Core.Entities;

    public interface IHotelServiceOrderService
    {
        Task<IEnumerable<HotelServiceOrder>> GetUserOrdersAsync(int guestId);

        Task<IEnumerable<HotelServiceOrder>> GetEmployeeOrdersAsync(int employeeId);

        Task CancelUnpaidOrderAsync(int hotelServiceOrderId);

        Task CommitAsync();
    }
}
