namespace HotelChief.API.Controllers.AdministrationControllers
{
    using AutoMapper;
    using HotelChief.API.ViewModels;
    using HotelChief.Application.Services;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize(Policy = "IsAdminPolicy")]
    public class ReservationsAdminController : Controller
    {
        private readonly IBaseCRUDService<Reservation> _crudService;
        private readonly IBaseCRUDService<Guest> _guestService;
        private readonly IBaseCRUDService<Room> _roomService;
        private readonly IMapper _mapper;

        public ReservationsAdminController(IBaseCRUDService<Reservation> crudService,
            IBaseCRUDService<Room> roomService,
            IBaseCRUDService<Guest> guestService,
            IMapper mapper)
        {
            _crudService = crudService;
            _guestService = guestService;
            _roomService = roomService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _crudService.Get();
            if (result != null)
            {
                return View(_mapper.Map<IEnumerable<Reservation>, IEnumerable<ReservationViewModel>>(result));
            }

            return Problem("There are no reservations");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindReservation(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Reservation, ReservationViewModel>(entity));
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new ReservationViewModel();

            viewModel = await FillTheLists(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationViewModel entity)
        {
            if (ModelState.IsValid)
            {
                var mappedEntity = _mapper.Map<ReservationViewModel, Reservation>(entity);
                await _crudService.AddAsync(mappedEntity);
                await _crudService.Commit();
                return RedirectToAction(nameof(Index));
            }

            entity = await FillTheLists(entity);
            return View(entity);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindReservation(id);
            if (entity == null)
            {
                return NotFound();
            }

            var mappedEntity = _mapper.Map<Reservation, ReservationViewModel>(entity);
            mappedEntity = await FillTheLists(mappedEntity);
            return View(mappedEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservationViewModel entity)
        {
            if (id != entity.ReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var mappedEntity = _mapper.Map<ReservationViewModel, Reservation>(entity);
                _crudService.Update(mappedEntity);
                await _crudService.Commit();
                if (await FindReservation(id) == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            entity = await FillTheLists(entity);
            return View(entity);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindReservation(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Reservation, ReservationViewModel>(entity));
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _crudService.Get() == null)
            {
                return Problem("There are no reservations");
            }

            var entity = await FindReservation(id);
            if (entity != null)
            {
                await _crudService.DeleteAsync(entity.ReservationId);
                await _crudService.Commit();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<Reservation?> FindReservation(int? id)
        {
            return (await _crudService.Get(m => m.ReservationId == id)).FirstOrDefault();
        }

        private async Task<ReservationViewModel> FillTheLists(ReservationViewModel viewModel)
        {
            viewModel.Guests = (await _guestService.Get()).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Id.ToString() + " " + e.FullName,
            });
            viewModel.Rooms = (await _roomService.Get()).Select(s => new SelectListItem
            {
                Value = s.RoomNumber.ToString(),
                Text = s.RoomNumber.ToString() + " " + s.RoomType,
            });
            return viewModel;
        }
    }
}
