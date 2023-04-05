using Bogus;
using SocialReview.BLL.Authentication.Models;
using SocialReview.DAL.Entities;

namespace SocialReview.UnitTests
{
    internal class TestDataGenerator
    {
        private readonly Faker _faker;

        public TestDataGenerator()
        {
            _faker = new Faker();
        }

        public CustomerRegisterDto GenerateCustomerRegisterDto()
        {
            return new CustomerRegisterDto
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                PhoneNumber = _faker.Phone.PhoneNumber("+380#########"),
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password()
            };
        }

        public UserCredentialsDto GenerateUserCredentialsDto()
        {
            return new UserCredentialsDto
            {
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password()
            };
        }

        public Customer GenerateCustomer()
        {
            return new Customer
            {
                Id = _faker.Random.Guid(),
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                PhoneNumber = _faker.Phone.PhoneNumber("+380#########"),
                Email = _faker.Internet.Email()
            };
        }

        public EstablishmentRegisterDto GenerateEstablishmentRegisterDto()
        {
            return new EstablishmentRegisterDto
            {
                Name = _faker.Name.FirstName(),
                Description = _faker.Lorem.Sentence(),
                City = _faker.Address.City(),
                PhoneNumber = _faker.Phone.PhoneNumber("+380#########"),
                Address = _faker.Address.StreetAddress()
            };
        }

        public Establishment GenerateEstablishment()
        {
            return new Establishment
            {
                Id = _faker.Random.Guid(),
                Name = _faker.Name.FirstName(),
                Description = _faker.Lorem.Sentence(),
                City = _faker.Address.City(),
                PhoneNumber = _faker.Phone.PhoneNumber("+380#########"),
                Address = _faker.Address.StreetAddress(),
                MainPhoto = Array.Empty<byte>().ToString()
            };
        }

        public User GenerateUser()
        {
            return new User
            {
                Email = _faker.Internet.Email(),
                PasswordHash = new byte[] { 1, 2, 3 },
                PasswordSalt = new byte[] { 4, 5, 6 },
                Role = Role.Customer
            };
        }
    }
}
