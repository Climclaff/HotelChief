namespace HotelChief.Core.Interfaces.IServices
{
    public interface IHotelServiceOrderHistoryService
    {
        Task MoveToFulfilledHistoryAsync(int orderId);
    }
}
