using SocialReview.API.Middlewares.RequestLoggingMiddleware.Abstract;

namespace SocialReview.API.Middlewares.RequestLoggingMiddleware
{
    /// <summary>
    /// Middleware that logs request and response information.
    /// </summary>
    public class RequestLoggingMiddleware : IMiddleware
    {
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly ILogMessageBuilder _logMessageBuilder;

        public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger, ILogMessageBuilder logMessageBuilder)
        {
            _logger = logger;
            _logMessageBuilder = logMessageBuilder;
        }

        /// <summary>
        /// Asynchronously invokes the next middleware in the pipeline and logs the request and response information.
        /// </summary>
        /// <param name="context">The HttpContext object representing the current request and response.</param>
        /// <param name="next">The next middleware in the pipeline.</param>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            finally
            {
                var message = await _logMessageBuilder.BuildLogMessageAsync(context.Request, context.Response);
                _logger.LogInformation(message);
            }
        }
    }
}
