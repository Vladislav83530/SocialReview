using SocialReview.API.Middlewares.ExceptionHandlingMiddleware.Abstract;
using System.Text.Json;

namespace SocialReview.API.Middlewares.ExceptionHandlingMiddleware
{
    /// <summary>
    /// Default implementation of the IExceptionHandler interface.
    /// </summary>
    public class DefaultExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<DefaultExceptionHandler> _logger;
        private readonly IErrorFactory _errorFactory;
        private readonly IHttpExceptionHandlerStrategy _httpExceptionHandlerStrategy;

        public DefaultExceptionHandler(
            ILogger<DefaultExceptionHandler> logger,
            IErrorFactory errorFactory,
            IHttpExceptionHandlerStrategy httpExceptionHandlerStrategy)
        {
            _logger = logger;
            _errorFactory = errorFactory;
            _httpExceptionHandlerStrategy = httpExceptionHandlerStrategy;
        }

        /// <summary>
        /// Handles the given exception by logging it and writing an error response to the HttpContext object.
        /// </summary>
        /// <param name="context">The HttpContext object representing the current request and response.</param>
        /// <param name="exception">The Exception object to handle.</param>
        public async Task HandleAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An error occurred.");

            context.Response.StatusCode = (int)_httpExceptionHandlerStrategy.GetStatusCode(exception);
            context.Response.ContentType = "application/json";

            var error = _errorFactory.CreateError(exception);

            var json = JsonSerializer.Serialize(error);

            await context.Response.WriteAsync(json);
        }
    }
}
