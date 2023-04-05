using SocialReview.API.Models;

namespace SocialReview.API.Middlewares.ExceptionHandlingMiddleware.Abstract
{
    public interface IErrorFactory
    {
        Error CreateError(Exception exception);
    }
}
