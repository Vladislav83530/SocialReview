using Microsoft.AspNetCore.Mvc;
using SocialReview.BLL.Authentication.Interfaces;
using SocialReview.BLL.Authentication.Models;
using SocialReview.DAL.Entities;

namespace SocialReview.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerAuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public CustomerAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new customer using the provided request data.
        /// </summary>
        /// <param name="request">A CustomerRegisterDto object containing the registration data.</param>
        /// <returns>A Customer object or a BadRequest if the registration fails.</returns>
        [HttpPost("customer-register")]
        public async Task<ActionResult<Customer>> CustomerRegister(CustomerRegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Wrong request model");

            var customer = await _authService.RegisterAsync<Customer, CustomerRegisterDto>(request);
            return Ok(customer);
        }

        /// <summary>
        /// Logs in a user with the provided email and password.
        /// </summary>
        /// <param name="request">The user login data transfer object containing the email and password.</param>
        /// <returns>An ActionResult containing a token for the authenticated user or a BadRequest if the model state is invalid.</returns>
        [HttpPost("customer-login")]
        public async Task<ActionResult<string>> CustomerLogin(UserCredentialsDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest($"Wrong request model");

            var token = await _authService.LoginAsync(request);
            return Ok(token);
        }
    }
}
