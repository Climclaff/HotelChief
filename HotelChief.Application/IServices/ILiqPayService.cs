namespace HotelChief.Application.IServices
{
    public interface ILiqPayService
    {
        Task ChangePaidOrderStatus(string orderId);

        Task ChangePaidReservationStatus(string orderId);

        Task CancelUnpaidReservation(string orderId);
    }
}
