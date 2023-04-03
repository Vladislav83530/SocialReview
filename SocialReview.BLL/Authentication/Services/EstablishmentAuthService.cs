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
        /// Registers a new establishment using the provided request data.
        /// </summary>
        /// <param name="request">A EstablishmentRegisterDto object containing the registration data.</param>
        /// <returns>A Establishment object representing the newly registered establishment.</returns>
        public async Task<Establishment> RegisterAsync(EstablishmentRegisterDto request)
        {
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
