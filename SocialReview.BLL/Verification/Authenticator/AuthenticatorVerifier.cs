using Microsoft.Extensions.Configuration;
using OtpNet;
using SocialReview.BLL.Verification.Interfaces;

namespace SocialReview.BLL.Verification.Authenticator
{
    internal class AuthenticatorVerifier : IVerifier
    {        
        private readonly IConfiguration _config;
        public AuthenticatorVerifier(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> VerifyAsync(string code)
        {
            var secretKey = Base32Encoding.ToBytes(_config.GetSection("AppSettings:AuthenticatorSecretKey").Value);
            var totp = new Totp(secretKey);
            var isValid = totp.VerifyTotp(code, out long timeWindowUsed);
            return isValid;
        }
    }
}
