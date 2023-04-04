using SocialReview.API.Middlewares.ExceptionHandlingMiddleware;

namespace SocialReview.UnitTests.API.Middlewares.ExceptionHandlingMiddlewareTests
{
    [TestFixture]
    public class DefaultErrorFactoryTests
    {
        private DefaultErrorFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new DefaultErrorFactory();
        }

        [Test]
        public void CreateError_WhenCalled_ReturnsErrorWithExpectedValues()
        {
            var exception = new Exception("Test exception message");

            var error = _factory.CreateError(exception);

            Assert.That(error.Message, Is.EqualTo("An error occurred."));
            Assert.That(error.Details, Is.EqualTo("Test exception message"));
        }
    }
}
