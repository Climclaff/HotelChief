namespace HotelChief.Application.Services
{
    using Hangfire;
    using HotelChief.Application.IServices;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces;
    using System.Collections.Immutable;

    public class RoomCleaningService : IRoomCleaningService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomCleaningService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RoomCleaning>> GetSchedule()
        {
            return (await _unitOfWork.GetRepository<RoomCleaning>().Get(includeProperties: "Employee")).ToImmutableList();
        }

        public async Task ScheduleRoomCleaning()
        {
            var janitors = (await _unitOfWork.GetRepository<Employee>().Get(e => e.Role == "Janitor", includeProperties: "RoomCleanings"))
                .OrderBy(e => e.RoomCleanings.Count)
                .ToList();

            var roomsToClean = (await _unitOfWork.GetRepository<Room>().Get()).ToList();

            if (janitors.Count == 0)
            {
                return;
            }

            var janitorIndex = 0;

            foreach (var room in roomsToClean)
            {
                if (!IsCleaningTimeAllowed())
                {
                    continue;
                }

                var cleaningTime = room.RoomType switch
                {
                    "big" => TimeSpan.FromMinutes(40),   // 40 minutes for big rooms
                    "medium" => TimeSpan.FromMinutes(30), // 30 minutes for medium rooms
                    _ => TimeSpan.FromMinutes(20) // 20 minutes for small rooms (default)
                };

                var breakTime = TimeSpan.FromMinutes(20); // 20 minutes break between cleanings

                // Select the janitor in a circular fashion
                var janitor = janitors[janitorIndex];
                janitorIndex = (janitorIndex + 1) % janitors.Count;

                // Calculate start and end dates based on cleaning time and break time
                var lastCleaning = janitor.RoomCleanings.MaxBy(r => r.EndDate);

                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;
                if (lastCleaning != null)
                {
                    if (lastCleaning.EndDate.Date == DateTime.UtcNow.Date)
                    {
                         startDate = lastCleaning.EndDate + breakTime;
                         endDate = lastCleaning.EndDate + breakTime + cleaningTime;
                    }
                }
                else
                {
                     breakTime = TimeSpan.Zero;
                     startDate = DateTime.UtcNow.Date.AddHours(7);
                     endDate = startDate + cleaningTime;
                }

                await CleanRoom(room.RoomNumber, janitor.EmployeeId, startDate, endDate);
            }

            await _unitOfWork.Commit();
        }

        public async Task CleanRoom(int roomNumber, int employeeId, DateTime startDate, DateTime endDate)
        {
            RoomCleaning roomCleaning = new RoomCleaning() { EmployeeId = employeeId, RoomNumber = roomNumber, StartDate = startDate, EndDate = endDate };
            await _unitOfWork.GetRepository<RoomCleaning>().AddAsync(roomCleaning);
        }

        private bool IsCleaningTimeAllowed()
        {
            var currentHour = DateTime.UtcNow.Hour;
            return currentHour >= 7 && currentHour < 21;
        }
    }
}
