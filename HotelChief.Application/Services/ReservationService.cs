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

        public async Task CommitAsync()
        {
            await _unitOfWork.CommitAsync();
        }

        public async Task<Reservation> ReserveRoomAsync(Reservation reservation)
        {
            await _unitOfWork.GetRepository<Reservation>().AddAsync(reservation);
            await _unitOfWork.CommitAsync();
            return reservation;
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate)
        {
            return await _unitOfWork.ReservationRepository.GetAvailableRoomsAsync(checkInDate, checkOutDate);
        }

        public async Task<IEnumerable<Tuple<DateTime, DateTime>>> GetAvailableTimeSlotsAsync(int roomNumber, DateTime startDate, DateTime endDate)
        {
            var existingReservations = await _unitOfWork.GetRepository<Reservation>().GetAsync(
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

        public async Task<double> CalculateReservationPriceAsync(int roomNumber, DateTime startDate, DateTime endDate)
        {
            var room = (await _unitOfWork.GetRepository<Room>().GetAsync(r => r.RoomNumber == roomNumber)).FirstOrDefault();
            if (room == null)
            {
                return 0;
            }

            double totalPrice = 0;
            totalPrice = (room.PricePerDay / 24) * (endDate - startDate).TotalHours;
            if (startDate.Day != DateTime.UtcNow.Day)
            {
                return totalPrice;
            }

            double remainingHours = (startDate.Date.AddDays(1) - DateTime.UtcNow).TotalHours;
            double firstDayPrice = remainingHours / 24 * room.PricePerDay;
            return totalPrice + firstDayPrice;
        }

        public async Task<bool> ContainsDuplicateReservationAsync(Reservation reservation)
        {
            var duplicate = (await _unitOfWork.GetRepository<Reservation>().GetAsync(res =>
            (res.CheckInDate == reservation.CheckInDate && res.RoomNumber == reservation.RoomNumber)
            ||
            (res.CheckOutDate == reservation.CheckOutDate && res.RoomNumber == reservation.RoomNumber))).FirstOrDefault();

            if (duplicate != null)
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Reservation>> GetUserReservationsAsync(int guestId)
        {
            return await _unitOfWork.GetRepository<Reservation>().GetAsync(
                res => res.GuestId == guestId, includeProperties: "Room");
        }
    }
}
