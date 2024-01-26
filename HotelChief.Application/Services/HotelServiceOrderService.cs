namespace HotelChief.Application.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces;
    using HotelChief.Core.Interfaces.IServices;

    public class HotelServiceOrderService : IHotelServiceOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HotelServiceOrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HotelServiceOrder>> GetUserOrders(int guestId)
        {
            return await _unitOfWork.GetRepository<HotelServiceOrder>().Get(
                order => order.GuestId == guestId, includeProperties: "HotelService");
        }

        public async Task<IEnumerable<HotelServiceOrder>> GetEmployeeOrders(int guestId)
        {
            return await _unitOfWork.GetRepository<HotelServiceOrder>().Get(
                    order => order.Employee.GuestId == guestId && order.OrderStatus != "Fulfilled",
                    includeProperties: "HotelService");
        }

        public async Task CancelUnpaidOrder(int hotelServiceOrderId)
        {
            await _unitOfWork.GetRepository<HotelServiceOrder>().DeleteAsync(hotelServiceOrderId);
        }

        public async Task Commit()
        {
            await _unitOfWork.Commit();
        }
    }
}
