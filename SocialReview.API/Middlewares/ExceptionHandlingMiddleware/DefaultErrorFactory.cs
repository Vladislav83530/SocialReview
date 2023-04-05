using SocialReview.API.Middlewares.ExceptionHandlingMiddleware.Abstract;
using SocialReview.API.Models;

namespace SocialReview.API.Middlewares.ExceptionHandlingMiddleware
{
    public class DefaultErrorFactory : IErrorFactory
    {
        /// <summary>
        /// Creates an Error object from the given Exception object.
        /// </summary>
        /// <param name="exception">The Exception object to create the Error object from.</param>
        /// <returns>An Error object containing information about the exception.</returns>
        public Error CreateError(Exception exception)
        {
            return new Error
            {
                Message = "An error occurred.",
                Details = exception.Message
            };
        }
    }
}
