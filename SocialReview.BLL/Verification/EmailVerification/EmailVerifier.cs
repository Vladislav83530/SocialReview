using Microsoft.AspNetCore.Http;
using SocialReview.BLL.Verification.Interfaces;

namespace SocialReview.BLL.Verification.EmailVerification
{
    public class EmailVerifier : IVerifier
    {
        private readonly IUserVerifyService _userVerifyService;
        private readonly HttpContext _context;
        public EmailVerifier(IUserVerifyService userVerifyService, HttpContext context )
        {
            _userVerifyService = userVerifyService;
            _context = context;
        }

        public bool VerifyAsync(string code)
        {
            var userEmail = _userVerifyService.GetUserEmailFromClaims(_context);
            var trueCode = _userVerifyService.GetUserVerificationCode(userEmail);
            return code == trueCode;
        }
    }
}
