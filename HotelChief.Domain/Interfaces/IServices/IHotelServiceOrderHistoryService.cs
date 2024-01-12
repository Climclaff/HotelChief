namespace HotelChief.Core.Interfaces.IServices
{
    public interface IHotelServiceOrderHistoryService
    {
        Task MoveToFulfilledHistory(int orderId);
    }
}
