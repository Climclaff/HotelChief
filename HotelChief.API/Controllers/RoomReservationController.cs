﻿#pragma warning disable CS8603, SA1309
namespace HotelChief.API.Controllers
{
    using System.Security.Claims;
    using AutoMapper;
    using HotelChief.API.Hubs;
    using HotelChief.API.ViewModels;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Localization;

    [Authorize(AuthenticationSchemes = "oidc")]
    public class RoomReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;
        private readonly IHubContext<RoomReservationHub> _hubContext;
        private readonly IStringLocalizer<RoomReservationController> _localizer;
        private readonly UserManager<Guest> _userManager;

        public RoomReservationController(
            IReservationService reservationService,
            UserManager<Guest> userManager,
            IMapper mapper,
            IHubContext<RoomReservationHub> hubContext,
            IStringLocalizer<RoomReservationController> localizer)
        {
            _reservationService = reservationService;
            _mapper = mapper;
            _hubContext = hubContext;
            _localizer = localizer;
            _userManager = userManager;
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

            return RedirectToAction("ReservationSuccess");
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
    }
}