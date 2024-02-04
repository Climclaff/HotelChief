namespace HotelChief.Application.IServices
{
    public interface ILiqPayService
    {
        Task ChangePaidOrderStatusAsync(string orderId);

        Task ChangePaidReservationStatusAsync(string orderId);

        Task CancelUnpaidReservationAsync(string orderId);
    }
}
