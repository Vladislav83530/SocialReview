using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SocialReview.BLL.Verification.Authenticator;
using SocialReview.BLL.Verification.Interfaces;
using SocialReview.BLL.Verification.Models;
using SocialReview.DAL.EF;
using System.Security.Claims;

namespace SocialReview.BLL.Verification.Services
{
    public class VerificationService : IVerificationService
    {
        private IVerificationFactory _varificationFactory;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        
        public VerificationService(IVerificationFactory varificationFactory, ApplicationDbContext context, IConfiguration config)
        {
            _varificationFactory = varificationFactory;
            _context = context;
            _config = config;
        }

        public async Task SendCodeAsync(SendingType type)
        {
            if (type == SendingType.Authenticator)
                _varificationFactory = new AuthenticatorVerificationFactory(_config);
            else if (type == SendingType.PhoneNumber) { 
                throw new NotImplementedException();
            }
            else if (type == SendingType.Email) {
                throw new NotImplementedException();
            }
                
           var codeSender = _varificationFactory.CreateCodeSender();
           await codeSender.SendCodeAsync();          
        }

        public async Task<bool> VerifyUserAsync(HttpContext context, SendingType type, string code)
        {
            if (type == SendingType.Authenticator)
            {
                _varificationFactory = new AuthenticatorVerificationFactory(_config);
            }
            else if (type == SendingType.PhoneNumber) {
                throw new NotImplementedException();
            }
            else if (type == SendingType.Email) {
                throw new NotImplementedException();
            }

            var verifier = _varificationFactory.CreateVerifier();
            var result = await verifier.VerifyAsync(code);

            if (result)
            {
                string email = GetUserEmailFromClaims(context);
                await UpdateIsVerifiedValue(email);
            }

            return result;
        }

        private string GetUserEmailFromClaims(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity == null)
                throw new NullReferenceException("User isn't authrorize");

            IEnumerable<Claim> claims = identity.Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            return email;
        }

        private async Task UpdateIsVerifiedValue(string email)
        {
            var normalizeEmail = email.ToLower();

            var user = _context.Users.FirstOrDefault(u => u.Email == normalizeEmail);
            user.IsVerified = true;
            _context.Entry(user).Property(u => u.IsVerified).IsModified = true;

            await _context.SaveChangesAsync();
        }
    }
}
