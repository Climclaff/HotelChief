namespace HotelChief.Core.Interfaces.IServices
{
    using HotelChief.Core.Entities;

    public interface IHotelServiceOrderService
    {
        Task<IEnumerable<HotelServiceOrder>> GetUserOrders(int guestId);

    }
}
