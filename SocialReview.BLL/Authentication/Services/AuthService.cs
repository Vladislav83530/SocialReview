using AutoMapper;
using SocialReview.BLL.Authentication.Interfaces;
using SocialReview.BLL.Authentication.Models;
using SocialReview.DAL.Entities;

namespace SocialReview.BLL.Authentication.Services
{
    /// <summary>
    /// A service for managing customer authentication.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly ISecurityService _securityService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthService(ISecurityService securityService, IUserRepository userRepository, IMapper mapper)
        {
            _securityService = securityService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new user with the provided information.
        /// </summary>
        /// <typeparam name="T">The type of the entity to create.</typeparam>
        /// <typeparam name="TDto">The type of the data transfer object containing the user information.</typeparam>
        /// <param name="request">The data transfer object containing the user information.</param>
        /// <returns>The created entity.</returns>
        /// <exception cref="ArgumentException">Thrown if the user is already registered.</exception>
        public async Task<T> RegisterAsync<T, TDto>(TDto request) where T : class where TDto : UserCredentialsDto
        {
            var isRegistered = await _userRepository.IsRegisteredAsync(request.Email);
            if (isRegistered)
                throw new ArgumentException($"{typeof(T).Name} with email {request.Email} is registered.");

            var isRegisteredByPhoneNumber = await _userRepository.IsRegisteredByPhoneNumberAsync(request.PhoneNumber);
            if(isRegisteredByPhoneNumber)
                throw new ArgumentException($"{typeof(T).Name} with phone number {request.PhoneNumber} is registered.");

            _securityService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var entity = _mapper.Map<T>(request);
            var entityId = Guid.NewGuid();
            typeof(T).GetProperty("Id").SetValue(entity, entityId);

            var user = _mapper.Map<User>(request);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = typeof(T) == typeof(Customer) ? Role.Customer : Role.Establishment;
            typeof(User).GetProperty($"{typeof(T).Name}Id").SetValue(user, entityId);

            await _userRepository.SaveUserAsync(user, entity);
            return entity;
        }

        /// <summary>
        /// Logs in a user with the provided email and password.
        /// </summary>
        /// <param name="userLoginDto">The user login data transfer object containing the email and password.</param>
        /// <returns>A token for the authenticated user.</returns>
        /// <exception cref="ArgumentException">Thrown if the user is not registered or if the password is incorrect.</exception>
        public async Task<string> LoginAsync(UserLoginDto userLoginDto)
        {
            var isRegistered = await _userRepository.IsRegisteredAsync(userLoginDto.Email);
            if (!isRegistered)
                throw new ArgumentException($"Customer with email {userLoginDto.Email} is not registered.");

            var user = await _userRepository.GetUserByEmailAsync(userLoginDto.Email);

            var isVerified = _securityService.VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt);
            if (!isVerified)
                throw new ArgumentException("Wrong password. Try again.");

            string token = _securityService.CreateToken(user);
            return token;
        }
    }
}
