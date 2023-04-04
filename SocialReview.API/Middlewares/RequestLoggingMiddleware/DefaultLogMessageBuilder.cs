using SocialReview.API.Middlewares.RequestLoggingMiddleware.Abstract;
using System.Text;

namespace SocialReview.API.Middlewares.RequestLoggingMiddleware
{
    public class DefaultLogMessageBuilder : ILogMessageBuilder
    {
        /// <summary>
        /// Builds a log message string from the given HttpRequest and HttpResponse objects.
        /// </summary>
        /// <param name="request">The HttpRequest object to build the log message from.</param>
        /// <param name="response">The HttpResponse object to build the log message from.</param>
        /// <returns>A log message string containing information from the HttpRequest and HttpResponse objects.</returns>
        public async Task<string> BuildLogMessageAsync(HttpRequest request, HttpResponse response)
        {      
            if (request != null)
            {
                var requestBody = await GetRequestBodyAsync(request);

                var sb = new StringBuilder();
                sb.AppendLine($"[{DateTime.Now}]");
                sb.AppendLine($"{request.Method} {request.Path}{request.QueryString}");
                sb.AppendLine($"Request body: {requestBody}");

                if(response != null) 
                    sb.AppendLine($"Status: {response.StatusCode}");

                sb.AppendLine("Headers: ");
                foreach (string key in request.Headers.Keys)
                {
                    sb.AppendLine("\t" + key + ": " + request.Headers[key]);
                }

                return sb.ToString();
            }
            return null;
        }

        /// <summary>
        /// Asynchronously reads the request body from the given HttpRequest object.
        /// </summary>
        /// <param name="request">The HttpRequest object to read the request body from.</param>
        /// <returns>A string containing the request body.</returns>
        private async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
