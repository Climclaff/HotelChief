namespace HotelChief.API.Controllers
{
    using HotelChief.API.ViewModels;
    using HotelChief.Application.IServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Policy = "IsEmployeePolicy")]
    public class RoomCleaningController : Controller
    {
        private readonly IRoomCleaningService _roomCleaningService;

        public RoomCleaningController(IRoomCleaningService roomCleaningService)
        {
            _roomCleaningService = roomCleaningService;
        }

        public async Task<IActionResult> Index()
        {
            var schedule = await _roomCleaningService.GetSchedule();
            RoomCleaningViewModel model = new RoomCleaningViewModel();
            model.RoomCleanings = schedule;
            return View(model);
        }
    }
}
