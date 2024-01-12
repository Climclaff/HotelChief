namespace HotelChief.Application.Services
{
    using System.Threading.Tasks;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces;
    using HotelChief.Core.Interfaces.IServices;

    public class HotelServiceOrderHistoryService : IHotelServiceOrderHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HotelServiceOrderHistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task MoveToFulfilledHistory(int orderId)
        {
            var order = (await _unitOfWork.GetRepository<HotelServiceOrder>().Get(o => o.HotelServiceOrderId == orderId)).FirstOrDefault();

            if (order != null && order.OrderStatus == "Fulfilled")
            {
                var orderHistory = new HotelServiceOrderHistory
                {
                    HotelServiceOrderId = order.HotelServiceOrderId,
                    OrderStatus = order.OrderStatus,
                    Amount = order.Amount,
                    GuestId = order.GuestId,
                    EmployeeId = order.EmployeeId,
                    HotelServiceId = order.HotelServiceOrderId,
                    Quantity = order.Quantity,
                    ServiceOrderDate = order.ServiceOrderDate,
                    PaymentStatus = order.PaymentStatus,
                    Timestamp = order.Timestamp,
                };

                await _unitOfWork.GetRepository<HotelServiceOrderHistory>().AddAsync(orderHistory);

                await _unitOfWork.GetRepository<HotelServiceOrder>().DeleteAsync(order.HotelServiceOrderId);

                await _unitOfWork.Commit();
            }
        }
    }
}
