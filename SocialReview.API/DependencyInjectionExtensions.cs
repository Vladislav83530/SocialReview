using SocialReview.API.Middlewares.ExceptionHandlingMiddleware.Abstract;
using SocialReview.API.Middlewares.ExceptionHandlingMiddleware;
using SocialReview.API.Middlewares.RequestLoggingMiddleware.Abstract;
using SocialReview.API.Middlewares.RequestLoggingMiddleware;
using SocialReview.BLL.Authentication.Interfaces;
using SocialReview.BLL.Authentication.Services;
using SocialReview.BLL.EmailSender.Email;
using SocialReview.BLL.EmailSender.Interfaces;
using SocialReview.BLL.Verification.Interfaces;
using SocialReview.BLL.Verification.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SocialReview.API
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Registers middleware services for error handling and request logging with the dependency injection container.
        /// </summary>
        /// <param name="serviceCollection">The IServiceCollection to add the services to.</param>
        /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
        public static IServiceCollection AddMiddlewares(this IServiceCollection serviceCollection)
        {
            // Register the error handling services with the dependency injection container
            serviceCollection.AddScoped<ExceptionHandlingMiddleware>();
            serviceCollection.AddScoped<IErrorFactory, DefaultErrorFactory>();
            serviceCollection.AddScoped<IExceptionHandler, DefaultExceptionHandler>();
            serviceCollection.AddScoped<IHttpExceptionHandlerStrategy, DefaultHttpExceptionHandlerStrategy>();

            // Register the request logging services with the dependency injection container
            serviceCollection.AddScoped<RequestLoggingMiddleware>();
            serviceCollection.AddScoped<ILogMessageBuilder, DefaultLogMessageBuilder>();

            return serviceCollection;
        }

        /// <summary>
        /// Registers authentication and authorization services with the dependency injection container and configures JWT bearer authentication.
        /// </summary>
        /// <param name="serviceCollection">The IServiceCollection to add the services to.</param>
        /// <param name="configuration">The IConfiguration instance to retrieve configuration values from.</param>
        /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
        public static IServiceCollection AddAuthServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped<IUserAuthService, UserAuthService>();
            serviceCollection.AddScoped<ISecurityService, SecurityService>();
            serviceCollection.AddScoped<IAuthService, AuthService>();


            serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(configuration["AppSettings:Token"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                    };
                });

            return serviceCollection;
        }

        /// <summary>
        /// Registers verification services with the dependency injection container.
        /// </summary>
        /// <param name="serviceCollection">The IServiceCollection to add the services to.</param>
        /// <param name="configuration">The IConfiguration instance to retrieve configuration values from.</param>
        /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
        public static IServiceCollection AddVerificationServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped<IUserVerifyService, UserVerifyService>();

            serviceCollection.AddScoped<IVerificationService, VerificationService>(provider => new VerificationService(
                    authenticatorSecretKey: configuration["AppSettings:AuthenticatorSecretKey"],
                    userVerifyService: provider.GetRequiredService<IUserVerifyService>(),
                    emailSender: provider.GetRequiredService<IEmailSender>()));

            return serviceCollection;
        }

        /// <summary>
        /// Registers an email sender service with the dependency injection container.
        /// </summary>
        /// <param name="serviceCollection">The IServiceCollection to add the services to.</param>
        /// <param name="configuration">The IConfiguration instance to retrieve configuration values from.</param>
        /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
        public static IServiceCollection AddEmailSender(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<IEmailSender, SmtpEmailSender>(provider => new SmtpEmailSender(
                   fromAddress: configuration["Email:FromAddress"],
                   fromName: configuration["Email:FromName"],
                   fromPassword: configuration["Email:FromPassword"],
                   smtpHost: configuration["Email:SmtpHost"],
                   smtpPort: int.Parse(configuration["Email:SmtpPort"])
               ));

            return serviceCollection;
        }
    }
}
