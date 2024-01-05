namespace HotelChief.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAvailableRooms(DateTime checkInDate, DateTime checkOutDate)
        {
            var allRooms = await _context.Rooms.ToListAsync();
            var reservedRoomNumbers = await _context.Reservations
                .Where(res => res.CheckOutDate >= checkInDate && res.CheckInDate <= checkOutDate)
                .Select(res => res.RoomNumber)
                .Distinct()
                .ToListAsync();

            var availableRooms = new List<Room>();
            foreach (var roomNumber in allRooms.Select(r => r.RoomNumber))
            {
                var existingReservations = await _context.Reservations.Where(
                    res => res.RoomNumber == roomNumber &&
                           (res.CheckInDate < checkOutDate && res.CheckOutDate > checkInDate)).ToListAsync();

                var minimumReservationTime = TimeSpan.FromDays(1);

                var currentDate = checkInDate.Date;

                var isRoomAvailable = false;

                while (currentDate < checkOutDate)
                {
                    var timeSlotEnd = currentDate.Add(minimumReservationTime);

                    var hasReservation = existingReservations.Any(res =>
                        res.CheckInDate < timeSlotEnd &&
                        res.CheckOutDate >= currentDate);

                    if (!hasReservation)
                    {
                        isRoomAvailable = true;
                        break;
                    }

                    currentDate = currentDate.AddDays(1);
                }

                if (isRoomAvailable)
                {
                    var room = allRooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
                    if (room != null)
                    {
                        availableRooms.Add(room);
                    }
                }
            }

            return availableRooms;
        }
    }
}
