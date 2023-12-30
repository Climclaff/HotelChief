namespace HotelChief.API.Controllers.AdministrationControllers
{
    using AutoMapper;
    using HotelChief.API.ViewModels;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Policy = "IsAdminPolicy")]
    public class HotelServicesAdminController : Controller
    {
        private readonly IBaseCRUDService<HotelService> _crudService;
        private readonly IMapper _mapper;

        public HotelServicesAdminController(IBaseCRUDService<HotelService> crudService, IMapper mapper)
        {
            _crudService = crudService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _crudService.Get();
            if (result != null)
            {
                return View(_mapper.Map<IEnumerable<HotelService>, IEnumerable<HotelServiceViewModel>>(result));
            }

            return Problem("There are no hotel services");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindHotelService(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<HotelService, HotelServiceViewModel>(entity));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,Description,Price")] HotelService entity)
        {
            if (ModelState.IsValid)
            {
                await _crudService.AddAsync(entity);
                await _crudService.Commit();
                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<HotelService, HotelServiceViewModel>(entity));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindHotelService(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<HotelService, HotelServiceViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceName,Description,Price")] HotelService entity)
        {
            if (id != entity.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _crudService.Update(entity);
                await _crudService.Commit();
                if (await FindHotelService(id) == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<HotelService, HotelServiceViewModel>(entity));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindHotelService(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<HotelService, HotelServiceViewModel>(entity));
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _crudService.Get() == null)
            {
                return Problem("There are no hotel services");
            }

            var entity = await FindHotelService(id);
            if (entity != null)
            {
               await _crudService.DeleteAsync(entity.ServiceId);
               await _crudService.Commit();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<HotelService?> FindHotelService(int? id)
        {
          return (await _crudService.Get(m => m.ServiceId == id)).FirstOrDefault();
        }
    }
}
