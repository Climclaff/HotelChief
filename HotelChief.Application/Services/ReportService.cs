namespace HotelChief.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HotelChief.Core.DTO;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Core.Interfaces.IServices;

    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<IEnumerable<EmployeeProductivityReportDto>?> GenerateEmployeeProductivityReport(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateEmployeeProductivityReport(startDate, endDate);
        }

        public async Task<IEnumerable<PopularRoomReportDto>?> GeneratePopularRoomsReport(int topN, DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GeneratePopularRoomsReport(topN, startDate, endDate);
        }

        public async Task<RevenueReportDto?> GenerateRevenueReport(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateRevenueReport(startDate, endDate);
        }

        public async Task<IEnumerable<ProfitableHotelServiceReportDto>?> GenerateTopHotelServiceRevenueReport(int topN, DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateTopHotelServiceRevenueReport(topN, startDate, endDate);
        }

        public async Task<IEnumerable<ProfitableRoomReportDto>?> GenerateTopRoomRevenueReport(int topN, DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateTopRoomRevenueReport(topN, startDate, endDate);
        }
    }
}
