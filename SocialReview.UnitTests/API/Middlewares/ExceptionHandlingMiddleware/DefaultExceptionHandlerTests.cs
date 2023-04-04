using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using SocialReview.API.Middlewares.ExceptionHandlingMiddleware.Abstract;
using SocialReview.API.Middlewares.ExceptionHandlingMiddleware;
using System.Net;

namespace SocialReview.UnitTests.API.Middlewares.ExceptionHandlingMiddlewareTests
{
    [TestFixture]
    public class DefaultExceptionHandlerTests
    {
        private Mock<ILogger<DefaultExceptionHandler>> _logger;
        private Mock<IErrorFactory> _errorFactory;
        private Mock<IHttpExceptionHandlerStrategy> _httpExceptionHandlerStrategy;
        private DefaultExceptionHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<DefaultExceptionHandler>>();
            _errorFactory = new Mock<IErrorFactory>();
            _httpExceptionHandlerStrategy = new Mock<IHttpExceptionHandlerStrategy>();
            _handler = new DefaultExceptionHandler(
                _logger.Object,
                _errorFactory.Object,
                _httpExceptionHandlerStrategy.Object);
        }

        [Test]
        public async Task HandleAsync_WhenCalled_LogsError()
        {
            var context = new DefaultHttpContext();
            var exception = new Exception();

            await _handler.HandleAsync(context, exception);

            _logger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "An error occurred."),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()));
        }

        [Test]
        public async Task HandleAsync_WhenCalled_SetsResponseStatusCode()
        {
            var context = new DefaultHttpContext();
            var exception = new Exception();
            _httpExceptionHandlerStrategy.Setup(x => x.GetStatusCode(exception)).Returns(HttpStatusCode.BadRequest);

            await _handler.HandleAsync(context, exception);

            Assert.That(context.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }
    }
}
