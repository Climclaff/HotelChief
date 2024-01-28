namespace HotelChief.Core.Interfaces.IServices
{
    using HotelChief.Core.DTO;

    public interface IReportService
    {
        Task<IEnumerable<ProfitableHotelServiceReportDto>?> GenerateTopHotelServiceRevenueReport(int topN, DateTime startDate, DateTime endDate);

        Task<IEnumerable<ProfitableRoomReportDto>?> GenerateTopRoomRevenueReport(int topN, DateTime startDate, DateTime endDate);

        Task<RevenueReportDto?> GenerateRevenueReport(DateTime startDate, DateTime endDate);

        Task<IEnumerable<PopularRoomReportDto>?> GeneratePopularRoomsReport(int topN, DateTime startDate, DateTime endDate);

        Task<IEnumerable<EmployeeProductivityReportDto>?> GenerateEmployeeProductivityReport(DateTime startDate, DateTime endDate);
    }
}
