using AutoMapper;
using Moq;
using SocialReview.BLL.Authentication.Interfaces;
using SocialReview.BLL.Authentication.Models;
using SocialReview.BLL.Authentication.Services;
using SocialReview.DAL.Entities;

namespace SocialReview.UnitTests.BLL.Authentication.Services
{
    [TestFixture]
    internal class AuthServiceTests
    {
        private Mock<ISecurityService> _mockSecurityService;
        private Mock<IUserAuthService> _mockUserRepository;
        private Mock<IMapper> _mockMapper;
        private AuthService _authService;
        private TestDataGenerator _dataGenerator;

        [SetUp]
        public void SetUp()
        {
            _mockSecurityService = new Mock<ISecurityService>();
            _mockUserRepository = new Mock<IUserAuthService>();
            _mockMapper = new Mock<IMapper>();
            _authService = new AuthService(_mockSecurityService.Object, _mockUserRepository.Object, _mockMapper.Object);
            _dataGenerator = new TestDataGenerator();
        }

        [Test]
        public async Task RegisterAsync_UserIsNotRegistered_SaveUserAndEntity()
        {
            var request = _dataGenerator.GenerateCustomerRegisterDto();
            var entity = new Customer();
            var user = new User();

            _mockUserRepository.Setup(x => x.IsRegisteredAsync(request.Email)).ReturnsAsync(false);
            _mockMapper.Setup(x => x.Map<Customer>(request)).Returns(entity);
            _mockMapper.Setup(x => x.Map<User>(request)).Returns(user);

            var result = await _authService.RegisterAsync<Customer, CustomerRegisterDto>(request);

            Assert.That(result, Is.EqualTo(entity));
            _mockUserRepository.Verify(x => x.SaveUserAsync(user, entity));
            _mockSecurityService.Verify(x => x.CreatePasswordHash(request.Password, out It.Ref<byte[]>.IsAny, out It.Ref<byte[]>.IsAny));
        }

        [Test]
        public async Task RegisterAsync_UserIsAlreadyRegistered_ThrowsArgumentException()
        {
            var request = _dataGenerator.GenerateCustomerRegisterDto();
            _mockUserRepository.Setup(x => x.IsRegisteredAsync(request.Email))
                .ReturnsAsync(true);

            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync<Customer, CustomerRegisterDto>(request));
        }

        [Test]
        public async Task RegisterAsync_UserIsAlreadyRegisteredCheckPhoneNumber_ThrowsArgumentException()
        {
            var request = _dataGenerator.GenerateCustomerRegisterDto();
            _mockUserRepository.Setup(x => x.IsRegisteredByPhoneNumberAsync(request.PhoneNumber))
                .ReturnsAsync(true);

            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync<Customer, CustomerRegisterDto>(request));
        }

        [Test]
        public async Task LoginAsync_UserIsNotRegistered_ThrowsArgumentException()
        {
            var request = _dataGenerator.GenerateUserLoginDto();
            _mockUserRepository.Setup(x => x.IsRegisteredAsync(request.Email))
                .ReturnsAsync(false);

            Assert.ThrowsAsync<ArgumentException>(() => _authService.LoginAsync(request));
        }

        [Test]
        public async Task LoginAsync_PasswordIsIncorect_ThrowsArgumentException()
        {
            var request = _dataGenerator.GenerateUserLoginDto();
            var user = new User { Email = request.Email, PasswordHash = new byte[0], PasswordSalt = new byte[0] };

            _mockUserRepository.Setup(x=>x.IsRegisteredAsync(request.Email))
                .ReturnsAsync(true);
            _mockUserRepository.Setup(x=>x.GetUserByEmailAsync(request.Email))
                .ReturnsAsync(user);
            _mockSecurityService.Setup(x=>x.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                .Returns(false);

            Assert.ThrowsAsync<ArgumentException>(()=>_authService.LoginAsync(request));
        }

        [Test]
        public async Task LoginAsync_CredentialsAreCorrect_ReturnsToken()
        {
            var request = _dataGenerator.GenerateUserLoginDto();
            var user = new User { Email = request.Email, PasswordHash = new byte[0], PasswordSalt = new byte[0] };
            var token = "token";
            _mockUserRepository.Setup(x => x.IsRegisteredAsync(request.Email))
                .ReturnsAsync(true);
            _mockUserRepository.Setup(x => x.GetUserByEmailAsync(request.Email))
                .ReturnsAsync(user);
            _mockSecurityService.Setup(x => x.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                .Returns(true);
            _mockSecurityService.Setup(x => x.CreateToken(user))
                .Returns(token);

            var result = await _authService.LoginAsync(request);

            Assert.That(result, Is.EqualTo(token));
        }
    }
}
