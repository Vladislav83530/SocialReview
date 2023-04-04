using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal;
using SocialReview.API.Middlewares.RequestLoggingMiddleware;
using SocialReview.API.Middlewares.RequestLoggingMiddleware.Abstract;

namespace SocialReview.UnitTests.API.Middlewares.RequestLoggingMiddlewareTests
{
    [TestFixture]
    internal class RequestLoggingMiddlewareTests
    {
        private Mock<ILogger<RequestLoggingMiddleware>> _mockLogger;
        private Mock<ILogMessageBuilder> _mockLogMessageBuilder;
        private RequestLoggingMiddleware _middleware;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<RequestLoggingMiddleware>>();
            _mockLogMessageBuilder = new Mock<ILogMessageBuilder>();
            _middleware = new RequestLoggingMiddleware(_mockLogger.Object, _mockLogMessageBuilder.Object);
        }

        [Test]
        public async Task InvokeAsync_LogsMessage()
        {
            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/test";
            context.Response.StatusCode = 200;

            var logMessage = "Test log message";
            _mockLogMessageBuilder.Setup(x => x.BuildLogMessageAsync(context.Request, context.Response))
                .ReturnsAsync(logMessage);

            await _middleware.InvokeAsync(context, _ => Task.CompletedTask);

            _mockLogger.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals(o.ToString(), logMessage)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }
    }
}
