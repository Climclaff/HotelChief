namespace HotelChief.Infrastructure.Repositories
{
    using HotelChief.Core.DTO;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeProductivityReportDto>?> GenerateEmployeeProductivityReport(DateTime startDate, DateTime endDate)
        {
            if (!await _context.Employees!.AnyAsync())
            {
                return null;
            }

            var employeeProductivity = await _context.Employees!
            .Select(employee => new EmployeeProductivityReportDto
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                HotelServiceOrdersCompleted = employee.HotelServiceOrders.Count(hso => hso.ServiceOrderDate >= startDate && hso.ServiceOrderDate <= endDate),
                RoomCleaningsCompleted = employee.RoomCleanings.Count(rc => rc.StartDate >= startDate && rc.EndDate <= endDate),
            })
            .ToListAsync();

            return employeeProductivity;
        }

        public async Task<IEnumerable<PopularRoomReportDto>?> GeneratePopularRoomsReport(int topN, DateTime startDate, DateTime endDate)
        {
            if (!await _context.Reservations!.AnyAsync())
            {
                return null;
            }

            var popularRooms = await _context.Reservations!
            .Where(r => r.CheckInDate >= startDate && r.CheckOutDate <= endDate)
            .GroupBy(r => r.RoomNumber)
            .OrderByDescending(group => group.Count())
            .Take(topN)
            .Select(group => new PopularRoomReportDto
            {
                RoomNumber = group.Key,
                ReservationsCount = group.Count(),
            })
            .ToListAsync();

            return popularRooms;
        }

        public async Task<RevenueReportDto?> GenerateRevenueReport(DateTime startDate, DateTime endDate)
        {
            if (!await _context.Reservations!.AnyAsync() && !await _context.HotelServiceOrders!.AnyAsync())
            {
                return null;
            }

            var reservationsRevenue = await _context.Reservations!
            .Where(r => r.CheckInDate >= startDate && r.CheckOutDate <= endDate)
            .SumAsync(r => r.Amount);

            var hotelServiceOrdersRevenue = await _context.HotelServiceOrders!
                .Where(hso => hso.ServiceOrderDate >= startDate && hso.ServiceOrderDate <= endDate)
                .SumAsync(hso => hso.Amount);

            var totalRevenue = reservationsRevenue + hotelServiceOrdersRevenue;

            var reservationsCount = await _context.Reservations!
                .Where(r => r.CheckInDate >= startDate && r.CheckOutDate <= endDate)
                .CountAsync();

            var hotelServiceOrdersCount = await _context.HotelServiceOrders!
                .Where(hso => hso.ServiceOrderDate >= startDate && hso.ServiceOrderDate <= endDate)
                .CountAsync();

            return new RevenueReportDto
            {
                TotalRevenue = totalRevenue,
                ReservationsCount = reservationsCount,
                HotelServiceOrdersCount = hotelServiceOrdersCount,
            };
        }

        public async Task<IEnumerable<ProfitableHotelServiceReportDto>?> GenerateTopHotelServiceRevenueReport(int topN, DateTime startDate, DateTime endDate)
        {
            if (!await _context.HotelServiceOrders!.AnyAsync())
            {
                return null;
            }

            var topHotelServiceRevenue = await _context.HotelServiceOrders!
            .Where(hso => hso.ServiceOrderDate >= startDate && hso.ServiceOrderDate <= endDate)
            .GroupBy(hso => hso.HotelServiceId)
            .OrderByDescending(group => group.Sum(hso => hso.Amount))
            .Take(topN)
            .Select(group => new ProfitableHotelServiceReportDto
            {
                HotelServiceId = group.Key,
                TotalRevenue = group.Sum(hso => hso.Amount),
                TotalOrders = group.Count(),
            })
            .ToListAsync();

            return topHotelServiceRevenue;
        }

        public async Task<IEnumerable<ProfitableRoomReportDto>?> GenerateTopRoomRevenueReport(int topN, DateTime startDate, DateTime endDate)
        {
            if (!await _context.Reservations!.AnyAsync())
            {
                return null;
            }

            var topRoomRevenue = await _context.Reservations!
            .Where(r => r.CheckInDate >= startDate && r.CheckOutDate <= endDate)
            .GroupBy(r => r.RoomNumber)
            .OrderByDescending(group => group.Sum(r => r.Amount))
            .Take(topN)
            .Select(group => new ProfitableRoomReportDto
            {
                RoomNumber = group.Key,
                TotalRevenue = group.Sum(r => r.Amount),
                TotalReservations = group.Count(),
            })
            .ToListAsync();

            return topRoomRevenue;
        }
    }
}
