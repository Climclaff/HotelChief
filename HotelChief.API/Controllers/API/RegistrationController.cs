namespace HotelChief.API.Controllers.API
{
    using HotelChief.API.Attributes;
    using HotelChief.API.ViewModels.IdentityServer;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly UserManager<Guest> _userManager;

        public RegistrationController(UserManager<Guest> userManager)
        {
            _userManager = userManager;
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

    }
}
