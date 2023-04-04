using Microsoft.AspNetCore.Http;
using Moq;
using SocialReview.API.Middlewares.ExceptionHandlingMiddleware;
using SocialReview.API.Middlewares.ExceptionHandlingMiddleware.Abstract;

namespace SocialReview.UnitTests.API.Middlewares.ExceptionHandlingMiddlewareTests
{
    [TestFixture]
    public class ExceptionHandlingMiddlewareTests
    {
        private Mock<IExceptionHandler> _exceptionHandler;
        private ExceptionHandlingMiddleware _middleware;

        [SetUp]
        public void SetUp()
        {
            _exceptionHandler = new Mock<IExceptionHandler>();
            _middleware = new ExceptionHandlingMiddleware(_exceptionHandler.Object);
        }

        [Test]
        public async Task InvokeAsync_WhenNextDelegateThrowsException_HandlesException()
        {
            var context = new DefaultHttpContext();
            var exception = new Exception();
            RequestDelegate next = ctx => throw exception;

            await _middleware.InvokeAsync(context, next);

            _exceptionHandler.Verify(x => x.HandleAsync(context, exception));
        }

        [Test]
        public async Task InvokeAsync_WhenNextDelegateDoesNotThrowException_DoesNotHandleException()
        {
            var context = new DefaultHttpContext();
            RequestDelegate next = ctx => Task.CompletedTask;

            await _middleware.InvokeAsync(context, next);

            _exceptionHandler.Verify(x => x.HandleAsync(It.IsAny<HttpContext>(), It.IsAny<Exception>()), Times.Never);
        }
    }
}
