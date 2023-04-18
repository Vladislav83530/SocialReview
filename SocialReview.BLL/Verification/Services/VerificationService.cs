using Microsoft.AspNetCore.Http;
using SocialReview.BLL.EmailSender.Interfaces;
using SocialReview.BLL.Verification.Authenticator;
using SocialReview.BLL.Verification.EmailVerification;
using SocialReview.BLL.Verification.Interfaces;
using SocialReview.BLL.Verification.Models;

namespace SocialReview.BLL.Verification.Services
{
    public class VerificationService : IVerificationService
    {
        private IVerificationFactory _verificationFactory;
        private IEmailSender _emailSender;
        private IUserVerifyService _userVerifyService;
        private string? _authenticatorSecretKey;

        public VerificationService(string? authenticatorSecretKey, IUserVerifyService userVerifyService, IEmailSender emailSender)
        {
            _authenticatorSecretKey = authenticatorSecretKey;
            _verificationFactory = new AuthenticatorVerificationFactory(_authenticatorSecretKey);
            _userVerifyService = userVerifyService;
            _emailSender = emailSender;
        }

        public async Task SendCodeAsync(HttpContext context, SendingType type)
        {
            if (type == SendingType.Authenticator)
                _verificationFactory = new AuthenticatorVerificationFactory(_authenticatorSecretKey);
            else if (type == SendingType.Email)
                _verificationFactory = new EmailVerificationFactory(_emailSender, _userVerifyService, context);  
                
           var codeSender = _verificationFactory.CreateCodeSender();
           await codeSender.SendCodeAsync();          
        }

        public async Task<bool> VerifyUserAsync(HttpContext context, SendingType type, string code)
        {
            if (type == SendingType.Authenticator)
                _verificationFactory = new AuthenticatorVerificationFactory(_authenticatorSecretKey);
            else if (type == SendingType.Email) 
                _verificationFactory = new EmailVerificationFactory(_emailSender, _userVerifyService, context);
            

            var verifier = _verificationFactory.CreateVerifier();
            var result = verifier.VerifyAsync(code);

            if (result)
            {
                string email = _userVerifyService.GetUserEmailFromClaims(context);
                await _userVerifyService.UpdateIsVerifiedValueAsync(email);
            }
            else
                throw new UnauthorizedAccessException("Verification code isn't valid. Please, try again.");

            return result;
        }
    }
}
