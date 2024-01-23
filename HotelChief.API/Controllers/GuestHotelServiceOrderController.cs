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

        public GuestHotelServiceOrderController(
            IHotelServiceOrderService orderService,
            IBaseCRUDService<HotelServiceOrder> orderCrudSerice,
            IBaseCRUDService<HotelService> hotelServicesService,
            IHubContext<EmployeeHotelServiceOrderHub> employeeHubContext,
            UserManager<Infrastructure.EFEntities.Guest> userManager,
            IConfiguration config)
        {
            _orderService = orderService;
            _orderCrudService = orderCrudSerice;
            _hotelServicesService = hotelServicesService;
            _employeeHubContext = employeeHubContext;
            _userManager = userManager;
            _config = config;
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
