﻿namespace HotelChief.API.Controllers
{
    using HotelChief.API.Hubs;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using System.Security.Claims;

    [Authorize(Policy = "IsEmployeePolicy")]
    public class EmployeeHotelServiceOrderController : Controller
    {
        private readonly IHotelServiceOrderService _serviceOrderService;
        private readonly IBaseCRUDService<HotelServiceOrder> _orderCrudService;
        private readonly IBaseCRUDService<Employee> _employeeCrudService;
        private readonly IHubContext<GuestHotelServiceOrderHub> _guestHubContext;
        private readonly IHotelServiceOrderHistoryService _hotelHistoryService;

        public EmployeeHotelServiceOrderController(
            IBaseCRUDService<HotelServiceOrder> orderCrudService,
            IBaseCRUDService<Employee> employeeCrudService,
            IHubContext<GuestHotelServiceOrderHub> guestHubContext,
            IHotelServiceOrderService serviceOrderService,
            IHotelServiceOrderHistoryService hotelHistoryService)
        {
            _orderCrudService = orderCrudService;
            _employeeCrudService = employeeCrudService;
            _guestHubContext = guestHubContext;
            _serviceOrderService = serviceOrderService;
            _hotelHistoryService = hotelHistoryService;
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
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Index");
            }

            var employeeOrders = await _serviceOrderService.GetEmployeeOrders(Convert.ToInt32(userId));

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

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Index");
            }

            var employee = (await _employeeCrudService.Get(e => e.GuestId == Convert.ToInt32(userId))).FirstOrDefault();
            if (employee == null)
            {
                return NotFound();
            }

            order.EmployeeId = employee.EmployeeId;
            order.OrderStatus = "Accepted";
            _orderCrudService.Update(order);
            await _orderCrudService.Commit();
            await _guestHubContext.Clients.User(Convert.ToString(order.GuestId)).SendAsync("UpdateOrderStatus", orderId, employee.EmployeeId);
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