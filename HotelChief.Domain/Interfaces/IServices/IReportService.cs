namespace HotelChief.Core.Interfaces.IServices
{
    using HotelChief.Core.DTO;

    public interface IReportService
    {
        Task<IEnumerable<ProfitableHotelServiceReportDto>?> GenerateTopHotelServiceRevenueReportAsync(int topN, DateTime startDate, DateTime endDate);

        Task<IEnumerable<ProfitableRoomReportDto>?> GenerateTopRoomRevenueReportAsync(int topN, DateTime startDate, DateTime endDate);

        Task<RevenueReportDto?> GenerateRevenueReportAsync(DateTime startDate, DateTime endDate);

        Task<IEnumerable<EmployeeProductivityReportDto>?> GenerateEmployeeProductivityReportAsync(DateTime startDate, DateTime endDate);
    }
}
