namespace HotelChief.Application.IServices
{
    using HotelChief.Core.Entities;

    public interface IRoomCleaningService
    {
        Task<IEnumerable<RoomCleaning>> GetScheduleAsync();

        Task ScheduleRoomCleaningAsync();

        Task CleanRoomAsync(int roomNumber, int employeeId, DateTime startDate, DateTime endDate);
    }
}
