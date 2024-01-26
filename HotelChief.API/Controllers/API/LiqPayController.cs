using HotelChief.API.Hubs;
using HotelChief.Application.IServices;
using HotelChief.Core.Entities;
using HotelChief.Core.Interfaces.IServices;
using HotelChief.Infrastructure.EFEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Telegram.Bot.Types;

namespace HotelChief.API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiqPayController : ControllerBase
    {
        private readonly UserManager<Guest> _userManager;
        private readonly ILiqPayService _iLiqPayService;
        private readonly IHubContext<EmployeeHotelServiceOrderHub> _employeeHubContext;
        private readonly IHubContext<GuestHotelServiceOrderHub> _guestHubContext;
        private readonly IConfiguration _config;
        private readonly IBaseCRUDService<HotelServiceOrder> _orderCRUDService;

        public LiqPayController(
            UserManager<Guest> userManager,
            ILiqPayService iLiqPayService,
            IConfiguration config,
            IHubContext<EmployeeHotelServiceOrderHub> employeeHubContext,
            IHubContext<GuestHotelServiceOrderHub> guestHubContext,
            IBaseCRUDService<HotelServiceOrder> orderCRUDService)
        {
            _userManager = userManager;
            _iLiqPayService = iLiqPayService;
            _config = config;
            _employeeHubContext = employeeHubContext;
            _guestHubContext = guestHubContext;
            _orderCRUDService = orderCRUDService;
        }

        [HttpPost]
        [Route("HotelServiceOrderPayment")]
        public async Task<IActionResult> HotelServiceOrderPayment()
        {
            var request_dictionary = Request.Form.Keys.ToDictionary(key => key, key => Request.Form[key]);

            byte[] request_data = Convert.FromBase64String(request_dictionary["data"]);
            string decodedString = Encoding.UTF8.GetString(request_data);
            var request_data_dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedString);

            var mySignature = Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(_config["LiqPay:ApiSecret"] + request_dictionary["data"] + _config["LiqPay:ApiSecret"])));

            if (mySignature != request_dictionary["signature"])
                return BadRequest("Signature mismatch. The provided signature does not match the expected value.");

            if (request_data_dictionary["status"] == "sandbox" || request_data_dictionary["status"] == "success")
            {
                var orderId = request_data_dictionary["order_id"];
                await _iLiqPayService.ChangePaidOrderStatus(orderId);
                await _employeeHubContext.Clients.All.SendAsync("RefreshOrders");

                var order = (await _orderCRUDService.Get(x => x.HotelServiceOrderId == Convert.ToInt32(orderId), includeProperties: "Employee")).FirstOrDefault();
                if (order != null)
                {
                    var guestEmail = (await _userManager.FindByIdAsync(order.GuestId.ToString())).Email;
                    var user = GuestHotelServiceOrderHub.ConnectedUsers.Where(x => x.Key == guestEmail).FirstOrDefault();
                    if (!user.Equals(default(KeyValuePair<string, List<string>>)))
                    {
                        for (int i = 0; i < user.Value.Count; i++)
                        {
                            await _guestHubContext.Clients.Client(user.Value[i]).SendAsync("RefreshOrders");
                        }
                    }

                    await _employeeHubContext.Clients.All.SendAsync("RefreshOrders");
                }
            }

            return StatusCode(500, "An error occured during payment process");
        }

        [HttpPost]
        [Route("RoomReservationPayment")]
        public async Task<IActionResult> RoomReservationPayment()
        {
            var request_dictionary = Request.Form.Keys.ToDictionary(key => key, key => Request.Form[key]);

            byte[] request_data = Convert.FromBase64String(request_dictionary["data"]);
            string decodedString = Encoding.UTF8.GetString(request_data);
            var request_data_dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedString);

            var mySignature = Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(_config["LiqPay:ApiSecret"] + request_dictionary["data"] + _config["LiqPay:ApiSecret"])));
            if (mySignature != request_dictionary["signature"])
                return BadRequest("Signature mismatch. The provided signature does not match the expected value.");

            var orderId = request_data_dictionary?["order_id"];
            if (orderId == null)
            {
                return BadRequest();
            }

            if (request_data_dictionary["status"] == "success")
            {
                await _iLiqPayService.ChangePaidReservationStatus(orderId);
            }
            else
            {
                await _iLiqPayService.CancelUnpaidReservation(orderId);
            }

            return StatusCode(500, "An error occured during payment process");
        }
    }
}
