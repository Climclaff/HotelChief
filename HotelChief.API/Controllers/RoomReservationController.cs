#pragma warning disable CS8603, SA1309
namespace HotelChief.API.Controllers
{
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using AutoMapper;
    using Hangfire;
    using HotelChief.API.Hubs;
    using HotelChief.API.ViewModels;
    using HotelChief.Application.IServices;
    using HotelChief.Application.Services;
    using HotelChief.Application.Services.Helpers;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.Infrastructure.EFEntities;
    using IdentityModel;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Localization;
    using Newtonsoft.Json;

    [Authorize(AuthenticationSchemes = "oidc")]
    public class RoomReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;
        private readonly IHubContext<RoomReservationHub> _hubContext;
        private readonly IStringLocalizer<RoomReservationController> _localizer;
        private readonly UserManager<Guest> _userManager;
        private readonly IConfiguration _config;
        private readonly IBaseCRUDService<Reservation> _baseCRUDService;
        private readonly ILiqPayService _payService;
        private readonly ILoyaltyService<Reservation> _loyaltyService;

        public RoomReservationController(
            IReservationService reservationService,
            UserManager<Guest> userManager,
            IMapper mapper,
            IHubContext<RoomReservationHub> hubContext,
            IStringLocalizer<RoomReservationController> localizer,
            IConfiguration config,
            IBaseCRUDService<Reservation> baseCRUDService,
            ILiqPayService payService,
            ILoyaltyService<Reservation> loyaltyService)
        {
            _reservationService = reservationService;
            _mapper = mapper;
            _hubContext = hubContext;
            _localizer = localizer;
            _userManager = userManager;
            _config = config;
            _baseCRUDService = baseCRUDService;
            _payService = payService;
            _loyaltyService = loyaltyService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await GetGuestReservationViewModel();
            if (viewModel != null)
            {
                return View(viewModel);
            }

            return Problem(_localizer["No_Rooms_To_Reserve"].ToString());
        }

        public async Task<IActionResult> MyReservations()
        {
            var userEmail = HttpContext.User.FindFirst("email")?.Value;
            var businessUser = await _userManager.FindByEmailAsync(userEmail);
            if (businessUser == null)
            {
                return RedirectToAction("Index");
            }

            var userReservations = (await _reservationService.GetUserReservations(businessUser.Id)).ToList();

            return View(userReservations);
        }

        [HttpPost]
        public async Task<IActionResult> ReserveRoom(int roomNumber, List<string> selectedTimeSlots)
        {
            if (selectedTimeSlots == null || !selectedTimeSlots.Any())
            {
                ModelState.AddModelError("selectedTimeSlots", _localizer["Select_Timeslot"].ToString());
                return View("Index", await GetGuestReservationViewModel());
            }

            var selectedSlots = GetSelectedSlots(selectedTimeSlots);

            if (!AreTimeSlotsAdjacent(selectedSlots))
            {
                ModelState.AddModelError("selectedTimeSlots", _localizer["Adjacent_slots"].ToString());
                return View("Index", await GetGuestReservationViewModel());
            }

            var minStartDate = selectedSlots.Min(slot => slot.Item1);
            var maxEndDate = selectedSlots.Max(slot => slot.Item2);
            var userEmail = HttpContext.User.FindFirst("email")?.Value;
            var businessUser = await _userManager.FindByEmailAsync(userEmail);
            if (businessUser == null)
            {
                return View("Index", await GetGuestReservationViewModel());
            }

            var price = await _reservationService.CalculateReservationPrice(roomNumber, minStartDate, maxEndDate);
            var reservation = new Reservation
            {
                RoomNumber = roomNumber,
                CheckInDate = minStartDate,
                CheckOutDate = maxEndDate,
                Amount = price,
                GuestId = businessUser.Id,
            };

            bool duplicateFound = await _reservationService.ContainsDuplicateReservation(reservation);
            if (duplicateFound == true)
            {
                return BadRequest();
            }

            await _reservationService.ReserveRoom(reservation);
            var connId = RoomReservationHub.ConnectedUsers.TryGetValue(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, out var conn);
            if (conn != null)
            {
                await _hubContext.Clients.AllExcept(conn).SendAsync("UpdateAvailableRooms");
            }

            var dbReservation = (await _baseCRUDService.Get((x => x.GuestId == businessUser.Id && x.RoomNumber == roomNumber && x.CheckInDate == minStartDate && x.CheckOutDate == maxEndDate && x.Amount == price))).FirstOrDefault();
            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            BackgroundJob.Schedule(() => _payService.CancelUnpaidReservation(reservation.ReservationId.ToString()), DateTime.UtcNow + TimeSpan.FromMinutes(10));
            var reservationJson = JsonConvert.SerializeObject(dbReservation, jsonSettings);
            TempData["ReservationInfo"] = reservationJson;

            return RedirectToAction("ReservationPayment");
        }

        [HttpGet]
        public async Task<IActionResult> ReservationPayment()
        {
            var reservationJson = TempData.Peek("ReservationInfo") as string;
            if (reservationJson == null)
            {
                return BadRequest();
            }

            var reservation = JsonConvert.DeserializeObject<Reservation>(reservationJson);
            if (reservation == null)
            {
                return BadRequest();
            }

            var dataAndSignature = GenerateDataAndSignature(reservation);
            ViewBag.Data = dataAndSignature.Item1;
            ViewBag.Signature = dataAndSignature.Item2;

            var model = _mapper.Map<Reservation, ReservationViewModel>(reservation);
            var userEmail = HttpContext.User.FindFirst("email")?.Value;
            var businessUser = await _userManager.FindByEmailAsync(userEmail);
            if (businessUser == null)
            {
                return RedirectToAction("Index");
            }

            model.LoyaltyPoints = businessUser.LoyaltyPoints;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyDiscount(int reservationId)
        {
            var userEmail = HttpContext.User.FindFirst("email")?.Value;
            var businessUser = await _userManager.FindByEmailAsync(userEmail);
            if (businessUser == null)
            {
                return RedirectToAction("Index");
            }

            var reservation = (await _baseCRUDService.Get(x => x.ReservationId == reservationId)).FirstOrDefault();
            if (reservation == null)
            {
                TempData["Discount_Status"] = _localizer["Reservation_Not_Found"].ToString();
                return RedirectToAction("Index");
            }

            var discountedOrder = await _loyaltyService.ApplyDiscount(reservation, businessUser.Id);
            if (discountedOrder == null)
            {
                TempData["Discount_Status"] = _localizer["Unable_To_Apply_Discount"].ToString();
                return RedirectToAction("Index");
            }

            _baseCRUDService.Update(discountedOrder);
            await _baseCRUDService.Commit();
            TempData["Discount_Status"] = _localizer["Discount_Success"].ToString();
            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var reservationJson = JsonConvert.SerializeObject(discountedOrder, jsonSettings);
            TempData["ReservationInfo"] = reservationJson;
            return RedirectToAction("ReservationPayment");
        }

        private async Task<GuestReservationViewModel> GetGuestReservationViewModel()
        {
            var availableRooms = await _reservationService.GetAvailableRooms(DateTime.UtcNow, DateTime.UtcNow.AddDays(7));
            if (availableRooms.Any())
            {
                var availableTimeSlots = new Dictionary<int, IEnumerable<Tuple<DateTime, DateTime>>>();

                foreach (var room in availableRooms)
                {
                    var timeSlots = await _reservationService.GetAvailableTimeSlots(room.RoomNumber, DateTime.UtcNow, DateTime.UtcNow.AddDays(7));
                    availableTimeSlots.TryAdd(room.RoomNumber, timeSlots);
                }

                var viewModel = new GuestReservationViewModel
                {
                    AvailableRooms = availableRooms,
                    AvailableTimeSlots = availableTimeSlots,
                };

                return viewModel;
            }

            return null;
        }

        private bool AreTimeSlotsAdjacent(List<Tuple<DateTime, DateTime>> selectedSlots)
        {
            selectedSlots.Sort((slot1, slot2) => slot1.Item1.CompareTo(slot2.Item1));

            for (int i = 0; i < selectedSlots.Count - 1; i++)
            {
                DateTime currentEndTime = selectedSlots[i].Item2.AddMinutes(1);
                DateTime nextStartTime = selectedSlots[i + 1].Item1;

                if (currentEndTime != nextStartTime)
                {
                    return false;
                }
            }

            return true;
        }

        private List<Tuple<DateTime, DateTime>?> GetSelectedSlots(List<string> selectedTimeSlots)
        {
            var selectedSlots = selectedTimeSlots.Select(slot =>
            {
                var parts = slot.Split(new[] { " - " }, StringSplitOptions.None);

                if (parts.Length != 2)
                {
                    return null;
                }

                var startDate = DateTime.Parse(parts[0]);
                var endDate = DateTime.Parse(parts[1]).AddMinutes(-1);

                return new Tuple<DateTime, DateTime>(startDate, endDate);
            })
            .Where(slot => slot != null)
            .ToList();
            return selectedSlots;
        }

        private Tuple<string, string> GenerateDataAndSignature(Reservation reservation)
        {
            var jsonString = JsonConvert.SerializeObject(new
            {
                public_key = _config["LiqPay:ApiPublicKey"],
                version = "3",
                action = "pay",
                amount = reservation.Amount.ToString(),
                currency = "UAH",
                description = "Test payment",
                order_id = reservation.ReservationId.ToString(),
                server_url = _config["LiqPay:RoomReservationServerCallbackUrl"],
                result_url = _config["LiqPay:RoomReservationResultUrl"],
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