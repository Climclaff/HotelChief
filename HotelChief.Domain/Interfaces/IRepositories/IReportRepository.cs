namespace HotelChief.Core.Interfaces.IRepositories
{
    using HotelChief.Core.DTO;

    public interface IReportRepository
    {
        Task<RevenueReportDto?> GenerateRevenueReport(DateTime startDate, DateTime endDate);

        Task<IEnumerable<PopularRoomReportDto>?> GeneratePopularRoomsReport(int topN, DateTime startDate, DateTime endDate);

        Task<IEnumerable<ProfitableRoomReportDto>?> GenerateTopRoomRevenueReport(int topN, DateTime startDate, DateTime endDate);

        Task<IEnumerable<ProfitableHotelServiceReportDto>?> GenerateTopHotelServiceRevenueReport(int topN, DateTime startDate, DateTime endDate);

        Task<IEnumerable<EmployeeProductivityReportDto>?> GenerateEmployeeProductivityReport(DateTime startDate, DateTime endDate);
    }
}
