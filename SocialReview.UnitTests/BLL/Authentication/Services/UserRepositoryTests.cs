using Microsoft.EntityFrameworkCore;
using SocialReview.BLL.Authentication.Services;
using SocialReview.DAL.EF;

namespace SocialReview.UnitTests.BLL.Authentication.Services
{
    [TestFixture]
    internal class UserRepositoryTests
    {
        private ApplicationDbContext _dbContext;
        private UserRepository _userRepository;
        private TestDataGenerator _dataGenerator;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _dbContext = new ApplicationDbContext(options);
            _userRepository = new UserRepository(_dbContext);
            _dataGenerator = new TestDataGenerator();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task SaveUserAsync_SavesUserAndUserInfo()
        {
            var user = _dataGenerator.GenerateUser();
            var customerInfo = _dataGenerator.GenerateCustomer();
            customerInfo.Email = user.Email;
            user.CustomerId = customerInfo.Id;

            await _userRepository.SaveUserAsync(user, customerInfo);

            var savedUser = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            Assert.IsNotNull(savedUser);
            Assert.AreEqual(user.Role, savedUser.Role);

            var savedCustomer = _dbContext.Customers.FirstOrDefault(c => c.Id == customerInfo.Id);
            Assert.IsNotNull(savedCustomer);
            Assert.AreEqual(savedUser.CustomerId, savedCustomer.Id);
        }

        [Test]
        public async Task IsRegisteredAsync_ForRegisteredUser_True()
        {
            var email = "test@test.com";
            var user = _dataGenerator.GenerateUser();
            user.Email = email;
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var result = await _userRepository.IsRegisteredAsync(email);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsRegisteredAsync_ForUnregisteredUser_False()
        {
            var email = "test@test.com";

            var result = await _userRepository.IsRegisteredAsync(email);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetUserByEmailAsync_UserIsExists_User()
        {
            var email = "test@test.com";
            var user = _dataGenerator.GenerateUser();
            user.Email = email;
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var result = await _userRepository.GetUserByEmailAsync(email);

            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
        }

        [Test]
        public async Task GetUserByEmailAsync_UserDoesNotExist_Null()
        {
            var email = "test@test.com";

            var result = await _userRepository.GetUserByEmailAsync(email);

            Assert.IsNull(result);
        }
    }
}
