namespace HotelChief.API.Controllers.AdministrationControllers
{
    using AutoMapper;
    using HotelChief.API.ViewModels;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Policy = "IsAdminPolicy")]
    public class RoomsAdminController : Controller
    {
        private readonly IBaseCRUDService<Room> _crudService;
        private readonly IMapper _mapper;

        public RoomsAdminController(IBaseCRUDService<Room> crudService, IMapper mapper)
        {
            _crudService = crudService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _crudService.Get();
            if (result != null)
            {
                return View(_mapper.Map<IEnumerable<Room>, IEnumerable<RoomViewModel>>(result));
            }

            return Problem("There are no rooms");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindRoom(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Room, RoomViewModel>(entity));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomNumber,PricePerDay,RoomType,IsAvailable")] Room entity)
        {
            if (ModelState.IsValid)
            {
                await _crudService.AddAsync(entity);
                await _crudService.Commit();
                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<Room, RoomViewModel>(entity));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindRoom(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Room, RoomViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomNumber,PricePerDay,RoomType,IsAvailable")] Room entity)
        {
            if (id != entity.RoomNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _crudService.Update(entity);
                await _crudService.Commit();
                if (await FindRoom(id) == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<Room, RoomViewModel>(entity));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindRoom(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Room, RoomViewModel>(entity));
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _crudService.Get() == null)
            {
                return Problem("There are no rooms");
            }

            var entity = await FindRoom(id);
            if (entity != null)
            {
                await _crudService.DeleteAsync(entity.RoomNumber);
                await _crudService.Commit();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<Room?> FindRoom(int? id)
        {
            return (await _crudService.Get(m => m.RoomNumber == id)).FirstOrDefault();
        }
    }
}
