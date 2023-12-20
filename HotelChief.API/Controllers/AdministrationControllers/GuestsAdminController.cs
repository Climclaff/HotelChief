namespace HotelChief.API.Controllers.AdministrationControllers
{
    using AutoMapper;
    using HotelChief.API.ViewModels;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.AspNetCore.Mvc;

    public class GuestsAdminController : Controller
    {
        private readonly IBaseCRUDService<Guest> _crudService;
        private readonly IMapper _mapper;

        public GuestsAdminController(IBaseCRUDService<Guest> crudService, IMapper mapper)
        {
            _crudService = crudService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _crudService.Get();
            if (result != null)
            {
                return View(_mapper.Map<IEnumerable<Guest>, IEnumerable<GuestViewModel>>(result));
            }

            return Problem("There are no guests");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindGuest(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Guest, GuestViewModel>(entity));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindGuest(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Guest, GuestViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,PhoneNumber,UserName,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumberConfirmed,TwoFactorEnabled,NormalizedUserName,LockoutEnd,LockoutEnabled,AccessFailedCount")] Guest entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _crudService.Update(entity);
                await _crudService.Commit();
                if (await FindGuest(id) == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<Guest, GuestViewModel>(entity));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindGuest(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Guest, GuestViewModel>(entity));
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _crudService.Get() == null)
            {
                return Problem("There are no guests");
            }

            var entity = await FindGuest(id);
            if (entity != null)
            {
                await _crudService.DeleteAsync(entity.Id);
                await _crudService.Commit();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<Guest?> FindGuest(int? id)
        {
            return (await _crudService.Get(m => m.Id == id)).FirstOrDefault();
        }
    }
}
