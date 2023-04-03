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
        private readonly ICustomerAuthService _authService;
        private readonly IUserRepository _userRepository;

        public CustomerAuthController(ICustomerAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
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
                return BadRequest(ModelState);

            var isRegistered = await _userRepository.IsRegisteredAsync(request.Email);
            if (isRegistered)
                return BadRequest($"Customer with email {request.Email} is registered.");

            var customer = await _authService.RegisterAsync(request);

            return Ok(customer);
        }
    }
}
