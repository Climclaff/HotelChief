namespace HotelChief.Core.Interfaces.IRepositories
{
    using HotelChief.Core.DTO;

    public interface IReportRepository
    {
        Task<RevenueReportDto?> GenerateRevenueReportAsync(DateTime startDate, DateTime endDate);

        Task<IEnumerable<PopularRoomReportDto>?> GeneratePopularRoomsReportAsync(int topN, DateTime startDate, DateTime endDate);

        Task<IEnumerable<ProfitableRoomReportDto>?> GenerateTopRoomRevenueReportAsync(int topN, DateTime startDate, DateTime endDate);

        Task<IEnumerable<ProfitableHotelServiceReportDto>?> GenerateTopHotelServiceRevenueReportAsync(int topN, DateTime startDate, DateTime endDate);

        Task<IEnumerable<EmployeeProductivityReportDto>?> GenerateEmployeeProductivityReportAsync(DateTime startDate, DateTime endDate);
    }
}
