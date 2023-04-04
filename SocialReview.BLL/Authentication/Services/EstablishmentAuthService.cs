using AutoMapper;
using SocialReview.BLL.Authentication.Interfaces;
using SocialReview.BLL.Authentication.Models;
using SocialReview.DAL.Entities;

namespace SocialReview.BLL.Authentication.Services
{
    /// <summary>
    /// A service for managing establishment authentication.
    /// </summary>
    public class EstablishmentAuthService : IEstablishmentAuthService
    {
        private readonly ISecurityService _securityService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public EstablishmentAuthService(ISecurityService securityService, IUserRepository userRepository, IMapper mapper)
        {
            _securityService = securityService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new establishment using the provided request.
        /// </summary>
        /// <param name="request">The establishment registration request.</param>
        /// <returns>The registered establishment.</returns>
        /// <exception cref="ArgumentException">Thrown if an establishment with the provided email is already registered.</exception>
        public async Task<Establishment> RegisterAsync(EstablishmentRegisterDto request)
        {
            var isRegistered = await _userRepository.IsRegisteredAsync(request.Email);
            if (isRegistered)
                throw new ArgumentException($"Establishment with email {request.Email} is registered.");

            _securityService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Establishment establishment = _mapper.Map<Establishment>(request);
            establishment.Id = Guid.NewGuid();

            var user = _mapper.Map<User>(request);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = Role.Establishment;
            user.EstablishmentId = establishment.Id;

            await _userRepository.SaveUserAsync(user, establishment);
            return establishment;
        }
    }
}
