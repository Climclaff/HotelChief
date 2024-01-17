namespace HotelChief.API.Controllers
{
    using System.Security.Claims;
    using HotelChief.API.Hubs;
    using HotelChief.API.ViewModels;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    [Authorize(AuthenticationSchemes = "oidc")]
    public class GuestHotelServiceOrderController : Controller
    {
        private readonly IHotelServiceOrderService _orderService;
        private readonly IBaseCRUDService<HotelServiceOrder> _orderCrudService;
        private readonly IBaseCRUDService<HotelService> _hotelServicesService;
        private readonly IHubContext<EmployeeHotelServiceOrderHub> _employeeHubContext;
        private readonly UserManager<Infrastructure.EFEntities.Guest> _userManager;

        public GuestHotelServiceOrderController(
            IHotelServiceOrderService orderService,
            IBaseCRUDService<HotelServiceOrder> orderCrudSerice,
            IBaseCRUDService<HotelService> hotelServicesService,
            IHubContext<EmployeeHotelServiceOrderHub> employeeHubContext,
            UserManager<Infrastructure.EFEntities.Guest> userManager)
        {
            _orderService = orderService;
            _orderCrudService = orderCrudSerice;
            _hotelServicesService = hotelServicesService;
            _employeeHubContext = employeeHubContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var hotelServices = await _hotelServicesService.Get();
            var userEmail = HttpContext.User.FindFirst("email")?.Value;
            var businessUser = await _userManager.FindByEmailAsync(userEmail);
            if (businessUser == null)
            {
                return RedirectToAction("Index");
            }

            var userOrders = await _orderService.GetUserOrders(Convert.ToInt32(businessUser.Id));

            var model = new HotelServiceOrderViewModel
            {
                HotelServices = hotelServices,
                UserOrders = userOrders,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(int serviceId, int quantity)
        {
            var userEmail = HttpContext.User.FindFirst("email")?.Value;
            var businessUser = await _userManager.FindByEmailAsync(userEmail);
            if (businessUser == null)
            {
                return RedirectToAction("Index");
            }

            var hotelService = (await _hotelServicesService.Get(s => s.ServiceId == serviceId)).FirstOrDefault();

            if (hotelService == null)
            {
                return NotFound();
            }

            var order = new HotelServiceOrder
            {
                GuestId = businessUser.Id,
                HotelServiceId = serviceId,
                Quantity = quantity,
                Amount = quantity * hotelService.Price,
                ServiceOrderDate = DateTime.UtcNow,
                PaymentStatus = false,
                OrderStatus = "In queue",
                Timestamp = DateTime.UtcNow,
            };

            await _orderCrudService.AddAsync(order);
            await _orderCrudService.Commit();
            await _employeeHubContext.Clients.All.SendAsync("RefreshOrders");
            return RedirectToAction("Index");
        }
    }
}
