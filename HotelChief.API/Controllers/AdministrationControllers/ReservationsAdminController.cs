namespace HotelChief.API.Controllers.AdministrationControllers
{
    using AutoMapper;
    using HotelChief.API.ViewModels;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Policy = "IsAdminPolicy")]
    public class ReservationsAdminController : Controller
    {
        private readonly IBaseCRUDService<Reservation> _crudService;
        private readonly IMapper _mapper;

        public ReservationsAdminController(IBaseCRUDService<Reservation> crudService, IMapper mapper)
        {
            _crudService = crudService;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationId,GuestId,RoomNumber,CheckInDate,CheckOutDate,Amount,PaymentStatus,Timestamp")] ReservationViewModel entity)
        {
            if (ModelState.IsValid)
            {
                var mappedEntity = _mapper.Map<ReservationViewModel, Reservation>(entity);
                await _crudService.AddAsync(mappedEntity);
                await _crudService.Commit();
                return RedirectToAction(nameof(Index));
            }

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

            return View(_mapper.Map<Reservation, ReservationViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationId,GuestId,RoomNumber,CheckInDate,CheckOutDate,Amount,PaymentStatus,Timestamp")] Reservation entity)
        {
            if (id != entity.ReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _crudService.Update(entity);
                await _crudService.Commit();
                if (await FindReservation(id) == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<Reservation, ReservationViewModel>(entity));
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
    }
}
