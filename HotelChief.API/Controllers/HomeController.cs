#pragma warning disable SA1309
namespace HotelChief.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.ViewModels;
    using Microsoft.AspNetCore.Authentication;
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
            var res = await HttpContext.AuthenticateAsync("Cookies");
            if (res.Succeeded == true)
            {
                var identity = new ClaimsIdentity(res.Principal.Claims, "Identity.Application", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                HttpContext.User = new ClaimsPrincipal(identity);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = "oidc")]
        public async Task<IActionResult> SignOut()
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