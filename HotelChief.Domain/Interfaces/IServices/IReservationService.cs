﻿namespace HotelChief.Core.Interfaces.IServices
{
    using HotelChief.Core.Entities;

    public interface IReservationService
    {
        Task<double> CalculateReservationPrice(int roomNumber, DateTime startDate, DateTime endDate);

        Task<Reservation> ReserveRoom(Reservation reservation);

        Task<IEnumerable<Room>> GetAvailableRooms(DateTime checkInDate, DateTime checkOutDate);

        Task<IEnumerable<Tuple<DateTime, DateTime>>> GetAvailableTimeSlots(int roomNumber, DateTime startDate, DateTime endDate);

        Task Commit();
    }
}
