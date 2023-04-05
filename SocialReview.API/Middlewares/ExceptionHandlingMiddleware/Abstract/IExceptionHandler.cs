namespace SocialReview.API.Middlewares.ExceptionHandlingMiddleware.Abstract
{
    public interface IExceptionHandler
    {
        Task HandleAsync(HttpContext context, Exception exception);
    }
}
