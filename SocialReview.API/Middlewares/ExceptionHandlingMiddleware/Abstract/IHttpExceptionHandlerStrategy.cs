using System.Net;

namespace SocialReview.API.Middlewares.ExceptionHandlingMiddleware.Abstract
{
    public interface IHttpExceptionHandlerStrategy
    {
        HttpStatusCode GetStatusCode(Exception exception);
    }
}
