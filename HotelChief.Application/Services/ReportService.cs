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

        public async Task<IEnumerable<EmployeeProductivityReportDto>?> GenerateEmployeeProductivityReportAsync(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateEmployeeProductivityReportAsync(startDate, endDate);
        }

        public async Task<RevenueReportDto?> GenerateRevenueReportAsync(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateRevenueReportAsync(startDate, endDate);
        }

        public async Task<IEnumerable<ProfitableHotelServiceReportDto>?> GenerateTopHotelServiceRevenueReportAsync(int topN, DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateTopHotelServiceRevenueReportAsync(topN, startDate, endDate);
        }

        public async Task<IEnumerable<ProfitableRoomReportDto>?> GenerateTopRoomRevenueReportAsync(int topN, DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateTopRoomRevenueReportAsync(topN, startDate, endDate);
        }
    }
}
