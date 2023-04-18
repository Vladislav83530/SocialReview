using Microsoft.AspNetCore.Http;
using SocialReview.BLL.Verification.Interfaces;
using SocialReview.DAL.EF;
using System.Security.Claims;

namespace SocialReview.BLL.Verification.Services
{
    public class UserVerifyService : IUserVerifyService
    {
        private readonly ApplicationDbContext _context;

        public UserVerifyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetUserEmailFromClaims(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity == null)
                throw new NullReferenceException("User isn't authrorize");

            IEnumerable<Claim> claims = identity.Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            return email;
        }

        public async Task UpdateIsVerifiedValueAsync(string email)
        {
            var normalizeEmail = email.ToLower();

            var user = _context.Users.FirstOrDefault(u => u.Email == normalizeEmail);
            user.IsVerified = true;
            _context.Entry(user).Property(u => u.IsVerified).IsModified = true;

            await _context.SaveChangesAsync();
        }

        public async Task<string> GenerateVerificationCode(string email)
        {
            Random random = new Random();
            int code = random.Next(100000, 999999);

            var normalizeEmail = email.ToLower();
            var user = _context.Users.FirstOrDefault(u => u.Email == normalizeEmail);
            user.VerificationCode = code.ToString();
            _context.Entry(user).Property(u => u.IsVerified).IsModified = true;

            await _context.SaveChangesAsync();
            return code.ToString();
        }

        public string GetUserVerificationCode(string email) =>
            _context.Users.FirstOrDefault(x => x.Email == email.ToLower()).VerificationCode;
    }
}
