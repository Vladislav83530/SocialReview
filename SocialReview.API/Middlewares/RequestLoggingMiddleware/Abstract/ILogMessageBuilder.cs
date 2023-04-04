namespace SocialReview.API.Middlewares.RequestLoggingMiddleware.Abstract
{
    public interface ILogMessageBuilder
    {
        Task<string> BuildLogMessageAsync(HttpRequest request, HttpResponse response);
    }
}
