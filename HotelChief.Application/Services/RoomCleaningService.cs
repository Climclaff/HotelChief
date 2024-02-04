namespace HotelChief.Application.Services
{
    using System.Collections.Immutable;
    using System.Data.Common;
    using System.Threading;
    using Hangfire;
    using HotelChief.Application.IServices;
    using HotelChief.Application.Services.Helpers;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.DependencyInjection;
    using static HotelChief.Application.Services.RoomCleaningService;

    public class RoomCleaningService : IRoomCleaningService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITelegramBotService _botService;

        public RoomCleaningService(IUnitOfWork unitOfWork, ITelegramBotService botService)
        {
            _unitOfWork = unitOfWork;
            _botService = botService;
        }

        public async Task<IEnumerable<RoomCleaning>> GetScheduleAsync()
        {
            return (await _unitOfWork.GetRepository<RoomCleaning>().GetAsync(x => x.StartDate.Day == DateTime.UtcNow.Day, includeProperties: "Employee")).ToImmutableList();
        }

        public async Task ScheduleRoomCleaningAsync()
        {
            await _unitOfWork.CommitAsync();

            var janitors = (await _unitOfWork.GetRepository<Employee>().GetAsync(e => e.Role == "Janitor" && e.OnVacation == false, includeProperties: "RoomCleanings"))
                .OrderBy(e => e.RoomCleanings.Count)
                .ToList();

            var roomsToClean = (await _unitOfWork.GetRepository<Room>().GetAsync()).ToList();

            if (janitors.Count == 0)
            {
                return;
            }

            var janitorIndex = 0;
            List<JobParameter> jobParametersList = new List<JobParameter>();
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

                jobParametersList.Add(new JobParameter(
                    room.RoomNumber,
                    janitor.FullName,
                    janitor.EmployeeId,
                    startDate,
                    endDate));
                await CleanRoomAsync(room.RoomNumber, janitor.EmployeeId, startDate, endDate);
            }

            await _unitOfWork.CommitAsync();
            foreach (var jobParameter in jobParametersList)
            {
                BackgroundJob.Schedule(() => SendCleaningNotification(jobParameter), jobParameter.StartDate.Subtract(TimeSpan.FromMinutes(5)));
            }
        }

        public async Task CleanRoomAsync(int roomNumber, int employeeId, DateTime startDate, DateTime endDate)
        {
            RoomCleaning roomCleaning = new RoomCleaning() { EmployeeId = employeeId, RoomNumber = roomNumber, StartDate = startDate, EndDate = endDate };
            await _unitOfWork.GetRepository<RoomCleaning>().AddAsync(roomCleaning);
        }

        public void SendCleaningNotification(JobParameter jobParameter)
        {
            _botService.SendTextMessageAsync(
                $"Upcoming cleaning: \r\n" +
                $"Room number: {jobParameter.RoomNumber} \r\n" +
                $"Janitor: {jobParameter.FullName}({jobParameter.EmployeeId}) \r\n" +
                $"{jobParameter.StartDate} - {jobParameter.EndDate}").GetAwaiter().GetResult();
        }

        private bool IsCleaningTimeAllowed()
        {
            var currentHour = DateTime.UtcNow.Hour;
            return currentHour >= 7 && currentHour < 21;
        }
    }
}
