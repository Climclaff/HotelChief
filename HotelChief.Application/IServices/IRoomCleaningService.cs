namespace HotelChief.Application.IServices
{
    using HotelChief.Core.Entities;

    public interface IRoomCleaningService
    {
        Task<IEnumerable<RoomCleaning>> GetSchedule();

        Task ScheduleRoomCleaning();

        Task CleanRoom(int roomNumber, int employeeId, DateTime startDate, DateTime endDate);
    }
}
