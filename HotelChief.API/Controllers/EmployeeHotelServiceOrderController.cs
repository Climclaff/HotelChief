namespace HotelChief.API.Controllers
{
    using HotelChief.API.Hubs;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using System.Security.Claims;

    [Authorize(AuthenticationSchemes = "oidc", Policy = "IsEmployeePolicy")]
    public class EmployeeHotelServiceOrderController : Controller
    {
        private readonly IHotelServiceOrderService _serviceOrderService;
        private readonly IBaseCRUDService<HotelServiceOrder> _orderCrudService;
        private readonly IBaseCRUDService<Employee> _employeeCrudService;
        private readonly IHubContext<GuestHotelServiceOrderHub> _guestHubContext;
        private readonly IHotelServiceOrderHistoryService _hotelHistoryService;
        private readonly UserManager<Infrastructure.EFEntities.Guest> _userManager;

        public EmployeeHotelServiceOrderController(
            IBaseCRUDService<HotelServiceOrder> orderCrudService,
            IBaseCRUDService<Employee> employeeCrudService,
            IHubContext<GuestHotelServiceOrderHub> guestHubContext,
            IHotelServiceOrderService serviceOrderService,
            IHotelServiceOrderHistoryService hotelHistoryService,
            UserManager<Infrastructure.EFEntities.Guest> userManager)
        {
            _orderCrudService = orderCrudService;
            _employeeCrudService = employeeCrudService;
            _guestHubContext = guestHubContext;
            _serviceOrderService = serviceOrderService;
            _hotelHistoryService = hotelHistoryService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var pendingOrders = await _orderCrudService.Get(
                o => o.EmployeeId == null,
                includeProperties: "HotelService");

            return View(pendingOrders);
        }

        public async Task<IActionResult> EmployeeOrders()
        {
            var userEmail = HttpContext.User.FindFirst("email")?.Value;
            var businessUser = await _userManager.FindByEmailAsync(userEmail);
            if (businessUser == null)
            {
                return RedirectToAction("Index");
            }

            var employeeOrders = await _serviceOrderService.GetEmployeeOrders(businessUser.Id);

            return View(employeeOrders);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptOrder(int orderId)
        {
            var order = (await _orderCrudService.Get(o => o.HotelServiceOrderId == orderId, includeProperties: "HotelService")).FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            var userEmail = HttpContext.User.FindFirst("email")?.Value;
            var businessUser = await _userManager.FindByEmailAsync(userEmail);
            if (businessUser == null)
            {
                return RedirectToAction("Index");
            }

            var employee = (await _employeeCrudService.Get(e => e.GuestId == businessUser.Id)).FirstOrDefault();
            if (employee == null)
            {
                return NotFound();
            }

            order.EmployeeId = employee.EmployeeId;
            order.OrderStatus = "Accepted";
            _orderCrudService.Update(order);
            await _orderCrudService.Commit();
            await _guestHubContext.Clients.User(Convert.ToString(businessUser.Id)).SendAsync("UpdateOrderStatus", orderId, employee.EmployeeId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> FulfillOrder(int orderId)
        {
            var order = (await _orderCrudService.Get(o => o.HotelServiceOrderId == orderId)).FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = "Fulfilled";
            _orderCrudService.Update(order);
            await _orderCrudService.Commit();

            await _hotelHistoryService.MoveToFulfilledHistory(orderId);

            return RedirectToAction("EmployeeOrders");
        }
    }
}
