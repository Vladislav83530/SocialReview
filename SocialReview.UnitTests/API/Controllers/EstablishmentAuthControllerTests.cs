using Microsoft.AspNetCore.Mvc;
using Moq;
using SocialReview.API.Controllers;
using SocialReview.BLL.Authentication.Interfaces;
using SocialReview.BLL.Authentication.Models;
using SocialReview.DAL.Entities;

namespace SocialReview.UnitTests.API.Controllers
{
    [TestFixture]
    internal class EstablishmentAuthControllerTests
    {
        private Mock<IAuthService> _authServiceMock;
        private EstablishmentAuthController _controller;
        private TestDataGenerator _dataGenerator;

        [SetUp]
        public void Setup()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new EstablishmentAuthController(_authServiceMock.Object);
            _dataGenerator = new TestDataGenerator();
        }

        [Test]
        public async Task EstablishmentRegister_WithValidModel_ReturnOk()
        {
            var request = _dataGenerator.GenerateEstablishmentRegisterDto();
            var establishment = _dataGenerator.GenerateEstablishment();
            establishment.Name = request.Name;
            establishment.Description = request.Description;
            establishment.City = request.City;
            establishment.Email = request.Email;
            establishment.PhoneNumber = request.PhoneNumber;

            _authServiceMock.Setup(x =>
            x.RegisterAsync<Establishment, EstablishmentRegisterDto>(request))
                .ReturnsAsync(establishment);

            var result = await _controller.EstablishmentRegister(request);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.That((result.Result as OkObjectResult).Value, Is.EqualTo(establishment));
        }

        [Test]
        public async Task EstablishmentRegister_WithInvalidModel_ReturnsBadRequest()
        {
            var request = _dataGenerator.GenerateEstablishmentRegisterDto();
            _controller.ModelState.AddModelError("error", "Wrong request model");

            var result = await _controller.EstablishmentRegister(request);

            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task EstablishmentLogin_WithValidModel_ReturnsOk()
        {
            var request = _dataGenerator.GenerateUserCredentialsDto();
            var token = "test token";

            _authServiceMock.Setup(x => x.LoginAsync(request)).ReturnsAsync(token);

            var result = await _controller.EstablishmentLogin(request);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.That((result.Result as OkObjectResult).Value, Is.EqualTo(token));
        }

        [Test]
        public async Task EstablishmentLogin_WithInvalidModel_ReturnsBadRequest()
        {
            var request = _dataGenerator.GenerateUserCredentialsDto();
            _controller.ModelState.AddModelError("error", "Wrong request model");

            var result = await _controller.EstablishmentLogin(request);

            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
    }
}
