namespace HotelChief.API.Controllers.AdministrationControllers
{
    using AutoMapper;
    using HotelChief.API.ViewModels;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(AuthenticationSchemes = "oidc", Policy = "IsAdminPolicy")]
    public class GuestsAdminController : Controller
    {
        private readonly IBaseCRUDService<Guest> _crudService;
        private readonly IMapper _mapper;
        private readonly UserManager<Guest> _userManager;
        private readonly IGuestRepository _guestRepository;

        public GuestsAdminController(
            IBaseCRUDService<Guest> crudService,
            UserManager<Guest> userManager,
            IMapper mapper,
            IGuestRepository guestRepository)
        {
            _crudService = crudService;
            _userManager = userManager;
            _mapper = mapper;
            _guestRepository = guestRepository;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _crudService.GetAsync();
            if (result != null)
            {
                var viewModel = _mapper.Map<IEnumerable<Guest>, IEnumerable<GuestViewModel>>(result);
                return View(viewModel);
            }

            return Problem("There are no guests");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _crudService.GetAsync() == null)
            {
                return NotFound();
            }

            var entity = await FindGuest(id);
            if (entity == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<Guest, GuestViewModel>(entity);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _crudService.GetAsync() == null)
            {
                return NotFound();
            }

            var entity = await FindGuest(id);
            if (entity == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<Guest, GuestViewModel>(entity);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,PhoneNumber,UserName,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumberConfirmed,TwoFactorEnabled,NormalizedUserName,LockoutEnd,LockoutEnabled,AccessFailedCount,LoyaltyPoints")] GuestViewModel entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var guest = await _userManager.FindByIdAsync(entity.Id.ToString());
                if (guest == null)
                {
                    return View(entity);
                }

                guest.FullName = entity.FullName;
                guest.PhoneNumber = entity.PhoneNumber;
                guest.LoyaltyPoints = entity.LoyaltyPoints;
                _crudService.Update(guest);
                await _crudService.CommitAsync();
                if (await FindGuest(id) == null)
                {
                    return NotFound();
                }

                await _userManager.UpdateSecurityStampAsync(guest);
                return RedirectToAction(nameof(Index));
            }

            return View(entity);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _crudService.GetAsync() == null)
            {
                return NotFound();
            }

            var entity = await FindGuest(id);
            if (entity == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<Guest, GuestViewModel>(entity);
            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _crudService.GetAsync() == null)
            {
                return Problem("There are no guests");
            }

            var entity = await FindGuest(id);
            await _guestRepository.RemoveGuestReviewVotesAsync(id);
            await _guestRepository.RemoveEmployeeInfoAsync(id);
            if (entity != null)
            {
                await _crudService.DeleteAsync(entity.Id);
                await _crudService.CommitAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<Guest?> FindGuest(int? id)
        {
            return (await _crudService.GetAsync(m => m.Id == id)).FirstOrDefault();
        }
    }
}
