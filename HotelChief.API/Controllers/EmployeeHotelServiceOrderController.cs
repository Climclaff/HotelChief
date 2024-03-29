﻿namespace HotelChief.API.Controllers
{
    using HotelChief.API.Hubs;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<EmployeeHotelServiceOrderController> _localizer;
        IHubContext<EmployeeHotelServiceOrderHub> _employeeHubContext;

        public EmployeeHotelServiceOrderController(
            IBaseCRUDService<HotelServiceOrder> orderCrudService,
            IBaseCRUDService<Employee> employeeCrudService,
            IHubContext<GuestHotelServiceOrderHub> guestHubContext,
            IHotelServiceOrderService serviceOrderService,
            IHotelServiceOrderHistoryService hotelHistoryService,
            UserManager<Infrastructure.EFEntities.Guest> userManager,
            IStringLocalizer<EmployeeHotelServiceOrderController> localizer,
            IHubContext<EmployeeHotelServiceOrderHub> employeeHubContext)
        {
            _orderCrudService = orderCrudService;
            _employeeCrudService = employeeCrudService;
            _guestHubContext = guestHubContext;
            _serviceOrderService = serviceOrderService;
            _hotelHistoryService = hotelHistoryService;
            _userManager = userManager;
            _localizer = localizer;
            _employeeHubContext = employeeHubContext;
        }

        public async Task<IActionResult> Index()
        {
            var pendingOrders = await _orderCrudService.GetAsync(
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

            var employeeOrders = await _serviceOrderService.GetEmployeeOrdersAsync(businessUser.Id);

            return View(employeeOrders);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptOrder(int orderId)
        {
            var order = (await _orderCrudService.GetAsync(o => o.HotelServiceOrderId == orderId, includeProperties: "HotelService")).FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            if (order.PaymentStatus == false)
            {
                TempData["Unable_Accept_Unpaid"] = _localizer["Unable_Accept_Unpaid"].ToString();
                return RedirectToAction("Index");
            }

            var userEmail = HttpContext.User.FindFirst("email")?.Value;
            var businessUser = await _userManager.FindByEmailAsync(userEmail);
            if (businessUser == null)
            {
                return RedirectToAction("Index");
            }

            var employee = (await _employeeCrudService.GetAsync(e => e.GuestId == businessUser.Id)).FirstOrDefault();
            if (employee == null)
            {
                return NotFound();
            }

            order.EmployeeId = employee.EmployeeId;
            order.OrderStatus = "Accepted";
            _orderCrudService.Update(order);
            await _orderCrudService.CommitAsync();

            var targetUser = await _userManager.FindByIdAsync(order.GuestId.ToString());

            var user = GuestHotelServiceOrderHub.ConnectedUsers.Where(x => x.Key == targetUser.Email).FirstOrDefault();
            if (!user.Equals(default(KeyValuePair<string, List<string>>)))
            {
                for (int i = 0; i < user.Value.Count; i++)
                {
                    await _guestHubContext.Clients.Client(user.Value[i]).SendAsync("UpdateOrderStatus", orderId, employee.EmployeeId);
                }
            }

            await _employeeHubContext.Clients.All.SendAsync("RefreshOrders");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> FulfillOrder(int orderId)
        {
            var order = (await _orderCrudService.GetAsync(o => o.HotelServiceOrderId == orderId)).FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = "Fulfilled";
            _orderCrudService.Update(order);
            await _orderCrudService.CommitAsync();

            await _hotelHistoryService.MoveToFulfilledHistoryAsync(orderId);
            TempData["Order_Fulfilled"] = _localizer["Order_Fulfilled"].ToString();
            return RedirectToAction("EmployeeOrders");
        }
    }
}
