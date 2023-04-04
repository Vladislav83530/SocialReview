using SocialReview.API.Middlewares.ExceptionHandlingMiddleware;
using System.Net;

namespace SocialReview.UnitTests.API.Middlewares.ExceptionHandlingMiddlewareTests
{
    [TestFixture]
    public class DefaultHttpExceptionHandlerStrategyTests
    {
        private DefaultHttpExceptionHandlerStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _strategy = new DefaultHttpExceptionHandlerStrategy();
        }

        [Test]
        public void GetStatusCode_WhenExceptionIsArgumentException_ReturnsBadRequest()
        {
            var exception = new ArgumentException();

            var statusCode = _strategy.GetStatusCode(exception);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void GetStatusCode_WhenExceptionIsInvalidOperationException_ReturnsConflict()
        {
            var exception = new InvalidOperationException();

            var statusCode = _strategy.GetStatusCode(exception);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.Conflict));
        }

        [Test]
        public void GetStatusCode_WhenExceptionIsNotHandled_ReturnsInternalServerError()
        {
            var exception = new Exception();

            var statusCode = _strategy.GetStatusCode(exception);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }
    }
}
