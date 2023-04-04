using Microsoft.EntityFrameworkCore;
using SocialReview.API.Middlewares.ExceptionHandlingMiddleware;
using SocialReview.API.Middlewares.ExceptionHandlingMiddleware.Abstract;
using SocialReview.API.Middlewares.RequestLoggingMiddleware;
using SocialReview.API.Middlewares.RequestLoggingMiddleware.Abstract;
using SocialReview.BLL.Authentication.Interfaces;
using SocialReview.BLL.Authentication.Services;
using SocialReview.DAL.EF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<ICustomerAuthService, CustomerAuthService>();
builder.Services.AddScoped<IEstablishmentAuthService, EstablishmentAuthService>();

// Register the error handling services with the dependency injection container
builder.Services.AddScoped<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<IErrorFactory, DefaultErrorFactory>();
builder.Services.AddScoped<IExceptionHandler, DefaultExceptionHandler>();
builder.Services.AddScoped<IHttpExceptionHandlerStrategy, DefaultHttpExceptionHandlerStrategy>();

// Register the request logging services with the dependency injection container
builder.Services.AddScoped<RequestLoggingMiddleware>();
builder.Services.AddScoped<ILogMessageBuilder, DefaultLogMessageBuilder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
