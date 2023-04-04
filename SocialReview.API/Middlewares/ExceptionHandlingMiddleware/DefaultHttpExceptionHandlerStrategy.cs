using SocialReview.API.Middlewares.ExceptionHandlingMiddleware.Abstract;
using System.Net;

namespace SocialReview.API.Middlewares.ExceptionHandlingMiddleware
{
    /// <summary>
    /// Default implementation of the IHttpExceptionHandlerStrategy interface.
    /// </summary>
    public class DefaultHttpExceptionHandlerStrategy : IHttpExceptionHandlerStrategy
    {
        /// <summary>
        /// Gets the appropriate HTTP status code for the given Exception object.
        /// </summary>
        /// <param name="exception">The Exception object to get the status code for.</param>
        /// <returns>An HttpStatusCode value representing the appropriate status code for the given exception.</returns>
        public HttpStatusCode GetStatusCode(Exception exception)
        {
            if (exception is ArgumentException)
                return HttpStatusCode.BadRequest;

            if (exception is InvalidOperationException)
                return HttpStatusCode.Conflict;

            return HttpStatusCode.InternalServerError;
        }
    }
}
