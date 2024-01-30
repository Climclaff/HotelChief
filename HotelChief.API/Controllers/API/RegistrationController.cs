namespace HotelChief.API.Controllers.API
{
    using AutoMapper;
    using HotelChief.API.Attributes;
    using HotelChief.API.ViewModels.IdentityServer;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly UserManager<Guest> _userManager;
        private readonly IBaseCRUDService<Guest> _crudService;
        private readonly IGuestRepository _guestRepository;

        public RegistrationController(
            UserManager<Guest> userManager,
            IBaseCRUDService<Guest> crudService,
            IGuestRepository guestRepository)
        {
            _userManager = userManager;
            _crudService = crudService;
            _guestRepository = guestRepository;
        }

        [HttpPost]
        [IdentityServerOnly] // Attrubute to secure calls (for development)
        [Route("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] RegisterViewModel model)
        {
            var user = new Guest()
            {
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpPost]
        [IdentityServerOnly] // Attrubute to secure calls (for development)
        [Route("CreateGoogleAccount")]
        public async Task<IActionResult> CreateGoogleAccount([FromBody] string email)
        {
            var user = new Guest()
            {
                Email = email,
                UserName = email,
            };

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpPost]
        [IdentityServerOnly] // Attrubute to secure calls (for development)
        [Route("RemoveAccount")]
        public async Task<IActionResult> RemoveAccount([FromBody] string email)
        {
            var entity = (await _crudService.Get(m => m.Email == email)).FirstOrDefault();
            if (entity != null)
            {
                await _guestRepository.RemoveGuestReviewVotes(entity.Id);
                await _guestRepository.RemoveEmployeeInfo(entity.Id);

                await _crudService.DeleteAsync(entity.Id);
                await _crudService.Commit();
                return Ok();
            }

            return StatusCode(500);
        }

    }
}
