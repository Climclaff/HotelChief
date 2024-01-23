#pragma warning disable SA1309
namespace HotelChief.Controllers
{
    using System.Diagnostics;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;


    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBaseCRUDService<Room> _roomService;

        public HomeController(ILogger<HomeController> logger, IBaseCRUDService<Room> roomService)
        {
            _logger = logger;
            _roomService = roomService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [Authorize(AuthenticationSchemes = "oidc")] // FOR TESTING PURPOSES
        [HttpGet("/call-api")]
        public async Task<IActionResult> CallApi()
        {
            return SignOut("Cookies", "oidc");
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}