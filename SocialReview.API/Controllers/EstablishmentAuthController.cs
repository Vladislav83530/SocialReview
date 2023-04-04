using Microsoft.AspNetCore.Mvc;
using SocialReview.BLL.Authentication.Interfaces;
using SocialReview.BLL.Authentication.Models;
using SocialReview.DAL.Entities;

namespace SocialReview.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstablishmentAuthController : ControllerBase
    {
        private readonly IEstablishmentAuthService _authService;

        public EstablishmentAuthController(IEstablishmentAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new establishment using the provided request data.
        /// </summary>
        /// <param name="request">A EstablishmentRegisterDto object containing the registration data.</param>
        /// <returns>A Establishment object or a BadRequest if the registration fails.</returns>
        [HttpPost("establishment-register")]
        public async Task<ActionResult<Establishment>> EstablishmentRegister(EstablishmentRegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var establishment = await _authService.RegisterAsync(request);
            return Ok(establishment);
        }
    }
}