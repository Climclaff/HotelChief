using AutoMapper;
using HotelChief.IdentityProvider.Attributes;
using HotelChief.IdentityProvider.ViewModels;
using IdentityProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelChief.IdentityProvider.Quickstart.Administration
{
    [AdminAuthorization]
    public class AdministrationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        public AdministrationController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _context.Users.ToListAsync();
            if (result != null)
            {
                var viewModel = _mapper.Map<IEnumerable<IdentityUser>, IEnumerable<IdentityUserViewModel>>(result);
                foreach (var user in viewModel)
                {
                    var guest = await _userManager.FindByIdAsync(user.Id.ToString());
                    var claims = await _userManager.GetClaimsAsync(guest);
                    var claim = claims.Where(x => x.Type == "IsEmployee").FirstOrDefault();
                    user.IsEmployee = claim?.Value;
                }

                return View(viewModel);
            }

            return Problem("There are no guests");
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || !await _context.Users.AnyAsync())
            {
                return NotFound();
            }

            var entity = await FindGuest(id);
            if (entity == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<IdentityUser, IdentityUserViewModel>(entity);
            var guest = await _userManager.FindByIdAsync(id.ToString());
            var claims = await _userManager.GetClaimsAsync(guest);
            var claim = claims.Where(x => x.Type == "IsEmployee").FirstOrDefault();

            viewModel.IsEmployee = claim?.Value;
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || !await _context.Users.AnyAsync())
            {
                return NotFound();
            }

            var entity = await FindGuest(id);
            if (entity == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<IdentityUser, IdentityUserViewModel>(entity);
            var guest = await _userManager.FindByIdAsync(id.ToString());
            var claims = await _userManager.GetClaimsAsync(guest);
            var claim = claims.Where(x => x.Type == "IsEmployee").FirstOrDefault();

            viewModel.IsEmployee = claim?.Value;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Email,PhoneNumber,UserName,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumberConfirmed,TwoFactorEnabled,NormalizedUserName,LockoutEnd,LockoutEnabled,AccessFailedCount,IsEmployee")] IdentityUserViewModel entity)
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

                var claims = await _userManager.GetClaimsAsync(guest);
                var claim = claims.Where(x => x.Type == "IsEmployee").FirstOrDefault();

                if (claim?.Value != entity.IsEmployee)
                {
                    await _userManager.ReplaceClaimAsync(guest, claim, new Claim("IsEmployee", entity.IsEmployee));
                }

                guest.PhoneNumber = entity.PhoneNumber;
                _context.Update(guest);
                await _context.SaveChangesAsync();
                if (await FindGuest(id) == null)
                {
                    return NotFound();
                }

                await _userManager.UpdateSecurityStampAsync(guest);
                return RedirectToAction(nameof(Index));
            }

            return View(entity);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || !await _context.Users.AnyAsync())
            {
                return NotFound();
            }

            var entity = await FindGuest(id);
            if (entity == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<IdentityUser, IdentityUserViewModel>(entity);
            var guest = await _userManager.FindByIdAsync(id.ToString());
            var claims = await _userManager.GetClaimsAsync(guest);
            var claim = claims.Where(x => x.Type == "IsEmployee").FirstOrDefault();

            viewModel.IsEmployee = claim?.Value;
            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!await _context.Users.AnyAsync())
            {
                return Problem("There are no guests");
            }

            var entity = await FindGuest(id);
            if (entity != null)
            {
                _context.Remove(entity.Id);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<IdentityUser?> FindGuest(string id)
        {
            return await _context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();
        }
    }
}
