using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework.Internal;
using SocialReview.API.Controllers;
using SocialReview.BLL.Authentication.Interfaces;
using SocialReview.BLL.Authentication.Models;
using SocialReview.DAL.Entities;

namespace SocialReview.UnitTests.API.Controllers
{
    [TestFixture]
    internal class CustomerAuthControllerTests
    {
        private Mock<IAuthService> _authServiceMock;
        private CustomerAuthController _controller;
        private TestDataGenerator _dataGenerator;

        [SetUp]
        public void Setup()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new CustomerAuthController(_authServiceMock.Object);
            _dataGenerator = new TestDataGenerator();
        }

        [Test]
        public async Task CustomerRegister_WithValidModel_ReturnOk()
        {
            var request = _dataGenerator.GenerateCustomerRegisterDto();
            var customer = _dataGenerator.GenerateCustomer();
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
            customer.PhoneNumber = request.PhoneNumber;

            _authServiceMock.Setup(x => 
            x.RegisterAsync<Customer, CustomerRegisterDto>(request))
                .ReturnsAsync(customer);

            var result = await _controller.CustomerRegister(request);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.That((result.Result as OkObjectResult).Value, Is.EqualTo(customer));
        }

        [Test]
        public async Task CustomerRegister_WithInvalidModel_ReturnsBadRequest()
        {
            var request = _dataGenerator.GenerateCustomerRegisterDto();
            _controller.ModelState.AddModelError("error", "Wrong request model");

            var result = await _controller.CustomerRegister(request);

            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task CustomerLogin_WithValidModel_ReturnsOk()
        {
            var request = _dataGenerator.GenerateUserLoginDto();
            var token = "test token";

            _authServiceMock.Setup(x => x.LoginAsync(request)).ReturnsAsync(token);

            var result = await _controller.CustomerLogin(request);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.That((result.Result as OkObjectResult).Value, Is.EqualTo(token));
        }

        [Test]
        public async Task CustomerLogin_WithInvalidModel_ReturnsBadRequest()
        {
            var request = _dataGenerator.GenerateUserLoginDto();
            _controller.ModelState.AddModelError("error", "Wrong request model");

            var result = await _controller.CustomerLogin(request);

            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
    }
}
