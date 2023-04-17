using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SocialReview.DAL.EF;
using System.Security.Claims;

namespace SocialReview.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class VerifyAttribute : Attribute, IAuthorizationFilter
    {
        private readonly bool _isVerified;

        public VerifyAttribute(bool isVerified)
        {
            _isVerified = isVerified;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);

            IEnumerable<Claim> claims = identity.Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var _dbcontext = context.HttpContext.RequestServices.GetService<ApplicationDbContext>();
            var user = _dbcontext.Users.FirstOrDefault(x => x.Email == email);

            if (user != null && user.IsVerified == _isVerified)
                await Task.CompletedTask;
            else
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }
    }
}
