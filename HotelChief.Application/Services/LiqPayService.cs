namespace HotelChief.Application.Services
{
    using System.Linq;
    using HotelChief.Application.IServices;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces;

    public class LiqPayService : ILiqPayService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LiqPayService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ChangePaidOrderStatusAsync(string orderId)
        {
            List<string> arr = orderId.Split(',').ToList();
            var orders = (await _unitOfWork.GetRepository<HotelServiceOrder>().GetAsync(x => arr.Contains(x.HotelServiceOrderId.ToString()))).ToList();
            foreach (var order in orders)
            {
                order.PaymentStatus = true;
                _unitOfWork.GetRepository<HotelServiceOrder>().Update(order);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task ChangePaidReservationStatusAsync(string orderId)
        {
            int roomReservationId = Convert.ToInt32(orderId);
            var reservation = (await _unitOfWork.GetRepository<Reservation>().GetAsync(x => x.ReservationId == roomReservationId)).FirstOrDefault();
            if (reservation != null)
            {
                reservation.PaymentStatus = true;
                reservation.Timestamp = DateTime.UtcNow;
                _unitOfWork.GetRepository<Reservation>().Update(reservation);

                await _unitOfWork.CommitAsync();
            }
        }

        public async Task CancelUnpaidReservationAsync(string orderId)
        {
            int roomReservationId = Convert.ToInt32(orderId);
            var reservation = (await _unitOfWork.GetRepository<Reservation>().GetAsync(x => x.ReservationId == roomReservationId)).FirstOrDefault();
            if (reservation != null)
            {
                if (reservation.PaymentStatus == false)
                {
                    await _unitOfWork.GetRepository<Reservation>().DeleteAsync(reservation.ReservationId);
                    await _unitOfWork.CommitAsync();
                }
            }
        }

    }
}
