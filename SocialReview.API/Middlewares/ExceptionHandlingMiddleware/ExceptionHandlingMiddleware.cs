using SocialReview.API.Middlewares.ExceptionHandlingMiddleware.Abstract;

namespace SocialReview.API.Middlewares.ExceptionHandlingMiddleware
{
    /// <summary>
    /// Middleware that handles exceptions thrown by the next middleware in the pipeline.
    /// </summary>
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly IExceptionHandler _exceptionHandler;

        public ExceptionHandlingMiddleware(IExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Asynchronously invokes the next middleware in the pipeline and handles any exceptions that are thrown.
        /// </summary>
        /// <param name="context">The HttpContext object representing the current request and response.</param>
        /// <param name="next">The next middleware in the pipeline.</param>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await _exceptionHandler.HandleAsync(context, ex);
            }
        }
    }
}
