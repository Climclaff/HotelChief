namespace HotelChief.API.Controllers
{
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using Hangfire.Server;
    using HotelChief.API.Hubs;
    using HotelChief.API.ViewModels;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Localization;
    using Newtonsoft.Json;

    [Authorize(AuthenticationSchemes = "oidc")]
    public class GuestHotelServiceOrderController : Controller
    {
        private readonly IHotelServiceOrderService _orderService;
        private readonly IBaseCRUDService<HotelServiceOrder> _orderCrudService;
        private readonly IBaseCRUDService<HotelService> _hotelServicesService;
        private readonly IHubContext<EmployeeHotelServiceOrderHub> _employeeHubContext;
        private readonly UserManager<Infrastructure.EFEntities.Guest> _userManager;
        private readonly IConfiguration _config;
        private readonly IStringLocalizer<GuestHotelServiceOrderController> _localizer;
        private readonly ILoyaltyService<HotelServiceOrder> _loyaltyService;

        public GuestHotelServiceOrderController(
            IHotelServiceOrderService orderService,
            IBaseCRUDService<HotelServiceOrder> orderCrudSerice,
            IBaseCRUDService<HotelService> hotelServicesService,
            IHubContext<EmployeeHotelServiceOrderHub> employeeHubContext,
            UserManager<Infrastructure.EFEntities.Guest> userManager,
            IStringLocalizer<GuestHotelServiceOrderController> localizer,
            IConfiguration config,
            ILoyaltyService<HotelServiceOrder> loyaltyService)
        {
            _orderService = orderService;
            _orderCrudService = orderCrudSerice;
            _hotelServicesService = hotelServicesService;
            _employeeHubContext = employeeHubContext;
            _userManager = userManager;
            _config = config;
            _localizer = localizer;
            _loyaltyService = loyaltyService;
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

            var totalCost = userOrders.Where(x => x.PaymentStatus == false).Sum(s => s.Amount);
            if (totalCost <= 0)
            {
                ViewBag.HidePayment = "true";
            }
            else
            {
                var dataAndSignature = GenerateDataAndSignature(userOrders.Where(x => x.PaymentStatus == false).ToList());
                ViewBag.Data = dataAndSignature.Item1;
                ViewBag.Signature = dataAndSignature.Item2;
            }

            var model = new HotelServiceOrderViewModel
            {
                HotelServices = hotelServices,
                UserOrders = userOrders,
            };

            model.OrdersCount = userOrders.Where(x => x.PaymentStatus == false).Count();
            model.LoyaltyPoints = businessUser.LoyaltyPoints;

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

            var userOrders = await _orderService.GetUserOrders(Convert.ToInt32(businessUser.Id));
            if (userOrders.Where(x => x.PaymentStatus == false).Count() >= 20)
            {
                TempData["Orders_Amount_Exceeded"] = _localizer["Orders_Amount_Exceeded"].ToString();
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

        [HttpPost]
        public async Task<IActionResult> CancelUnpaidOrder(int hotelServiceOrderId)
        {
            var order = (await _orderCrudService.Get(x => x.HotelServiceOrderId == hotelServiceOrderId)).FirstOrDefault();

            if (order == null || order.PaymentStatus == true)
            {
                return BadRequest();
            }

            var userEmail = HttpContext.User.FindFirst("email")?.Value;
            var businessUser = await _userManager.FindByEmailAsync(userEmail);
            if (businessUser == null)
            {
                return RedirectToAction("Index");
            }

            await _orderService.CancelUnpaidOrder(order.HotelServiceOrderId);
            await _orderService.Commit();

            await _employeeHubContext.Clients.All.SendAsync("RefreshOrders");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ApplyDiscount(int orderId)
        {
            var userEmail = HttpContext.User.FindFirst("email")?.Value;
            var businessUser = await _userManager.FindByEmailAsync(userEmail);
            if (businessUser == null)
            {
                return RedirectToAction("Index");
            }

            var order = (await _orderCrudService.Get(x => x.HotelServiceOrderId == orderId)).FirstOrDefault();
            if (order == null)
            {
                TempData["Discount_Status"] = _localizer["Order_Not_Found"].ToString();
                return RedirectToAction("Index");
            }

            var discountedOrder = await _loyaltyService.ApplyDiscount(order, businessUser.Id);
            if (discountedOrder == null)
            {
                TempData["Discount_Status"] = _localizer["Unable_To_Apply_Discount"].ToString();
                return RedirectToAction("Index");
            }

            _orderCrudService.Update(discountedOrder);
            await _orderCrudService.Commit();
            TempData["Discount_Status"] = _localizer["Discount_Success"].ToString();
            return RedirectToAction("Index");
        }

        private Tuple<string, string> GenerateDataAndSignature(List<HotelServiceOrder> orders)
        {
            StringBuilder orderIdBuilder = new StringBuilder();
            for (int i = 0; i < orders.Count; i++)
            {
                if (i != orders.Count - 1)
                {
                    orderIdBuilder.Append(orders[i].HotelServiceOrderId.ToString() + ",");
                }
                else
                {
                    orderIdBuilder.Append(orders[i].HotelServiceOrderId.ToString());
                }
            }

            var jsonString = JsonConvert.SerializeObject(new
            {
                public_key = _config["LiqPay:ApiPublicKey"],
                version = "3",
                action = "pay",
                amount = orders.Sum(s => s.Amount).ToString(),
                currency = "UAH",
                description = "Test payment",
                order_id = orderIdBuilder.ToString(),
                server_url = _config["LiqPay:HotelServiceOrderServerCallbackUrl"],
                result_url = _config["LiqPay:HotelServiceOrderResultUrl"],
            });

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(jsonString);
            var data = Convert.ToBase64String(plainTextBytes);
            var signString = _config["LiqPay:ApiSecret"] + data + _config["LiqPay:ApiSecret"];

            var shaEncodedSignStringBytes = System.Text.Encoding.UTF8.GetBytes(signString);
            var encodedBytes = SHA1.Create().ComputeHash(shaEncodedSignStringBytes);

            var signature = Convert.ToBase64String(encodedBytes);
            return Tuple.Create(data, signature);
        }
    }
}
