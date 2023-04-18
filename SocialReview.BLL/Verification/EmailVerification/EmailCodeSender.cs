using Microsoft.AspNetCore.Http;
using SocialReview.BLL.EmailSender.Interfaces;
using SocialReview.BLL.Verification.Interfaces;

namespace SocialReview.BLL.Verification.EmailVerification
{
    public class EmailCodeSender : ICodeSender
    {
        private readonly IEmailSender _emailSender;
        private readonly IUserVerifyService _userVerifyService;
        private readonly HttpContext _context;

        public EmailCodeSender(IEmailSender emailSender, IUserVerifyService userVerifyService, HttpContext context)
        {
            _emailSender = emailSender;
            _userVerifyService = userVerifyService;
            _context = context;
        }

        public async Task SendCodeAsync()
        {
            var recipientEmail = _userVerifyService.GetUserEmailFromClaims(_context);
            var verificationCode = await _userVerifyService.GenerateVerificationCode(recipientEmail);

            string subject = "Email Verification";
            string body = $"Your verification code is: {verificationCode}. Please, enter this code on the site to be verified.";
            await _emailSender.SendEmailAsync(recipientEmail, subject, body);
        }
    }
}
