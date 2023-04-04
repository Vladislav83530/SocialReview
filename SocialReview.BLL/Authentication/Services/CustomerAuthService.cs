using AutoMapper;
using SocialReview.BLL.Authentication.Interfaces;
using SocialReview.BLL.Authentication.Models;
using SocialReview.DAL.Entities;

namespace SocialReview.BLL.Authentication.Services
{
    /// <summary>
    /// A service for managing customer authentication.
    /// </summary>
    public class CustomerAuthService : ICustomerAuthService
    {
        private readonly ISecurityService _securityService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CustomerAuthService(ISecurityService securityService, IUserRepository userRepository, IMapper mapper)
        {
            _securityService = securityService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new customer using the provided request.
        /// </summary>
        /// <param name="request">The customer registration request.</param>
        /// <returns>The registered customer.</returns>
        /// <exception cref="ArgumentException">Thrown if an customer with the provided email is already registered.</exception>
        public async Task<Customer> RegisterAsync(CustomerRegisterDto request)
        {
            var isRegistered = await _userRepository.IsRegisteredAsync(request.Email);
            if (isRegistered)
               throw new ArgumentException($"Customer with email {request.Email} is registered.");

            _securityService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Customer customer = _mapper.Map<Customer>(request);
            customer.Id = Guid.NewGuid();

            var user = _mapper.Map<User>(request);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = Role.Customer;
            user.CustomerId = customer.Id;

            await _userRepository.SaveUserAsync(user, customer);
            return customer;
        }
    }
}
