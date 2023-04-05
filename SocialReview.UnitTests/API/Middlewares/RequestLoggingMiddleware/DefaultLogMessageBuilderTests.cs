using Microsoft.AspNetCore.Http;
using SocialReview.API.Middlewares.RequestLoggingMiddleware;
using System.Text;

namespace SocialReview.UnitTests.API.Middlewares.RequestLoggingMiddlewareTests
{
    [TestFixture]
    public class DefaultLogMessageBuilderTests
    {
        private DefaultLogMessageBuilder _builder;

        [SetUp]
        public void SetUp()
        {
            _builder = new DefaultLogMessageBuilder();
        }

        [Test]
        public async Task BuildLogMessageAsync_ReturnsExpectedMessage()
        {
            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/test";
            context.Request.QueryString = new QueryString("?param=value");
            context.Request.Headers.Add("HeaderKey", "HeaderValue");
            context.Response.StatusCode = 200;

            var requestBody = "Test request body";
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody)))
            {
                context.Request.Body = stream;

                var message = await _builder.BuildLogMessageAsync(context.Request, context.Response);

                Assert.That(message, Does.Contain("GET /test?param=value"));
                Assert.That(message, Does.Contain("Request body: Test request body"));
                Assert.That(message, Does.Contain("Status: 200"));
                Assert.That(message, Does.Contain("HeaderKey: HeaderValue"));
            }
        }

        [Test]
        public async Task BuildLogMessageAsync_WhenRequestIsNull_ReturnsNull()
        {
            var context = new DefaultHttpContext();

            var message = await _builder.BuildLogMessageAsync(null, context.Response);

            Assert.IsNull(message);
        }

        [Test]
        public async Task BuildLogMessageAsync_WhenResponseIsNull_ReturnsExpectedMessage()
        {
            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/test";

            var message = await _builder.BuildLogMessageAsync(context.Request, null);

            Assert.That(message, Does.Contain("GET /test"));
        }
    }
}
