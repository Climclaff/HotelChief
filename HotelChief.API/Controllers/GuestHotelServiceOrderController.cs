namespace HotelChief.API.Controllers
{
    using System.Security.Claims;
    using HotelChief.API.Hubs;
    using HotelChief.API.ViewModels;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class GuestHotelServiceOrderController : Controller
    {
        private readonly IHotelServiceOrderService _orderService;
        private readonly IBaseCRUDService<HotelServiceOrder> _orderCrudService;
        private readonly IBaseCRUDService<HotelService> _hotelServicesService;
        private readonly IHubContext<EmployeeHotelServiceOrderHub> _employeeHubContext;

        public GuestHotelServiceOrderController(
            IHotelServiceOrderService orderService,
            IBaseCRUDService<HotelServiceOrder> orderCrudSerice,
            IBaseCRUDService<HotelService> hotelServicesService,
            IHubContext<EmployeeHotelServiceOrderHub> employeeHubContext)
        {
            _orderService = orderService;
            _orderCrudService = orderCrudSerice;
            _hotelServicesService = hotelServicesService;
            _employeeHubContext = employeeHubContext;
        }

        public async Task<IActionResult> Index()
        {
            var hotelServices = await _hotelServicesService.Get();
            var guestId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (guestId == null)
            {
                return RedirectToAction("Index");
            }

            var userOrders = await _orderService.GetUserOrders(Convert.ToInt32(guestId));

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
            var guestId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var hotelService = (await _hotelServicesService.Get(s => s.ServiceId == serviceId)).FirstOrDefault();

            if (hotelService == null)
            {
                return NotFound();
            }

            var order = new HotelServiceOrder
            {
                GuestId = Convert.ToInt32(guestId),
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
