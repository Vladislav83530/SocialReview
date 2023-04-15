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
        private readonly IAuthService _authService;

        public EstablishmentAuthController(IAuthService authService)
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

            var establishment = await _authService.RegisterAsync<Establishment, EstablishmentRegisterDto>(request);
            return Ok(establishment);
        }

        /// <summary>
        /// Logs in a user with the provided email and password.
        /// </summary>
        /// <param name="request">The user login data transfer object containing the email and password.</param>
        /// <returns>An ActionResult containing a token for the authenticated user or a BadRequest if the model state is invalid.</returns>
        [HttpPost("establishment-login")]
        public async Task<ActionResult<string>> EstablishmentLogin(UserLoginDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest($"Wrong request model");

            var token = await _authService.LoginAsync(request);
            return Ok(token);
        }
    }
}