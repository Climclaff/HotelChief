namespace HotelChief.Application.Services
{
    using System.Threading.Tasks;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces;
    using HotelChief.Core.Interfaces.IServices;

    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Commit()
        {
            await _unitOfWork.Commit();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByGuestId(int guestId)
        {
            return await _unitOfWork.GetRepository<Reservation>().Get(r => r.GuestId == guestId);
        }

        public async Task<Reservation> ReserveRoom(Reservation reservation)
        {
            await _unitOfWork.GetRepository<Reservation>().AddAsync(reservation);
            await _unitOfWork.Commit();
            return reservation;
        }

        public async Task<IEnumerable<Room>> GetAvailableRooms(DateTime checkInDate, DateTime checkOutDate)
        {
            return await _unitOfWork.ReservationRepository.GetAvailableRooms(checkInDate, checkOutDate);
        }

        public async Task<IEnumerable<Tuple<DateTime, DateTime>>> GetAvailableTimeSlots(int roomNumber, DateTime startDate, DateTime endDate)
        {
            var existingReservations = await _unitOfWork.GetRepository<Reservation>().Get(
                res => res.RoomNumber == roomNumber &&
                       (res.CheckInDate < endDate && res.CheckOutDate > startDate));

            var minimumReservationTime = TimeSpan.FromDays(1);

            var availableTimeSlots = new List<Tuple<DateTime, DateTime>>();
            var currentDate = startDate.Date;

            while (currentDate < endDate)
            {
                var timeSlotEnd = currentDate.Add(minimumReservationTime);

                var isAvailable = !existingReservations.Any(res =>
                    res.CheckInDate < timeSlotEnd &&
                    res.CheckOutDate >= currentDate);

                if (isAvailable)
                {
                    availableTimeSlots.Add(new Tuple<DateTime, DateTime>(currentDate, timeSlotEnd));
                }

                currentDate = currentDate.AddDays(1);
            }

            return availableTimeSlots;
        }

        public async Task<double> CalculateReservationPrice(int roomNumber, DateTime startDate, DateTime endDate)
        {
            var room = (await _unitOfWork.GetRepository<Room>().Get(r => r.RoomNumber == roomNumber)).FirstOrDefault();
            double totalPrice = 0;
            if (endDate.Date != startDate.Date)
            {
                totalPrice = room.PricePerDay * (endDate.Date - startDate.Date).TotalDays;
            }

            double remainingHours = (startDate.Date.AddDays(1) - DateTime.UtcNow).TotalHours;
            double firstDayPrice = remainingHours / 24 * room.PricePerDay;

            return totalPrice + firstDayPrice;
        }

        public async Task<bool> ContainsDuplicateReservation(Reservation reservation)
        {
            var duplicate = (await _unitOfWork.GetRepository<Reservation>().Get(res => res.CheckInDate == reservation.CheckInDate ||
            res.CheckOutDate == reservation.CheckOutDate)).FirstOrDefault();

            if (duplicate != null)
            {
                return true;
            }

            return false;
        }
    }
}
