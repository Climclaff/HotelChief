namespace HotelChief.API.Controllers
{
    using HotelChief.Core.Interfaces.IServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    [Authorize(AuthenticationSchemes = "oidc", Policy = "IsAdminPolicy")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IStringLocalizer<ReportController> _localizer;

        public ReportController(
            IReportService reportService,
            IStringLocalizer<ReportController> localizer)
        {
            _reportService = reportService;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GenerateEmployeeProductivityReport()
        {
            return View("EmployeeProductivity");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateEmployeeProductivityReport(DateTime startDate, DateTime endDate)
        {
            var report = await _reportService.GenerateEmployeeProductivityReportAsync(startDate, endDate);
            if (report == null)
            {
                TempData["Error_Message"] = _localizer["Error_Message"];
            }

            return View("EmployeeProductivity", report);
        }

        [HttpGet]
        public IActionResult GenerateRevenueReport()
        {
            return View("Revenue");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateRevenueReport(DateTime startDate, DateTime endDate)
        {
            var report = await _reportService.GenerateRevenueReportAsync(startDate, endDate);
            if (report == null)
            {
                TempData["Error_Message"] = _localizer["Error_Message"];
            }

            return View("Revenue", report);
        }

        [HttpGet]
        public IActionResult GenerateTopHotelServiceRevenueReport()
        {
            return View("TopHotelServiceRevenue");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateTopHotelServiceRevenueReport(int topN, DateTime startDate, DateTime endDate)
        {
            var report = await _reportService.GenerateTopHotelServiceRevenueReportAsync(topN, startDate, endDate);
            if (report == null)
            {
                TempData["Error_Message"] = _localizer["Error_Message"];
            }

            return View("TopHotelServiceRevenue", report);
        }

        [HttpGet]
        public IActionResult GenerateTopRoomRevenueReport()
        {
            return View("TopRoomRevenue");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateTopRoomRevenueReport(int topN, DateTime startDate, DateTime endDate)
        {
            var report = await _reportService.GenerateTopRoomRevenueReportAsync(topN, startDate, endDate);
            if (report == null)
            {
                TempData["Error_Message"] = _localizer["Error_Message"];
            }

            return View("TopRoomRevenue", report);
        }
    }
}
