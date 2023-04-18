using Microsoft.AspNetCore.Http;
using SocialReview.BLL.EmailSender.Interfaces;
using SocialReview.BLL.Verification.Interfaces;

namespace SocialReview.BLL.Verification.EmailVerification
{
    public class EmailVerificationFactory : IVerificationFactory
    {
        private readonly IEmailSender _emailSender;
        private readonly IUserVerifyService _userVerifyService;
        private readonly HttpContext _context;

        public EmailVerificationFactory(IEmailSender emailSender, IUserVerifyService userVerifyService, HttpContext context)
        {
            _emailSender = emailSender;
            _userVerifyService = userVerifyService;
            _context = context;
        }

        public ICodeSender CreateCodeSender()
        {
            return new EmailCodeSender(_emailSender, _userVerifyService, _context);
        }

        public IVerifier CreateVerifier()
        {
            return new EmailVerifier(_userVerifyService, _context);
        }
    }
}
