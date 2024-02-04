namespace HotelChief.Core.Interfaces.IServices
{
    using HotelChief.Core.Entities;

    public interface IReservationService
    {
        Task<double> CalculateReservationPriceAsync(int roomNumber, DateTime startDate, DateTime endDate);

        Task<Reservation> ReserveRoomAsync(Reservation reservation);

        Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate);

        Task<IEnumerable<Tuple<DateTime, DateTime>>> GetAvailableTimeSlotsAsync(int roomNumber, DateTime startDate, DateTime endDate);

        Task<bool> ContainsDuplicateReservationAsync(Reservation reservation);

        Task<IEnumerable<Reservation>> GetUserReservationsAsync(int userId);

        Task CommitAsync();
    }
}
