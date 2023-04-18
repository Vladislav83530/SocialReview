using OtpNet;
using SocialReview.BLL.Verification.Interfaces;

namespace SocialReview.BLL.Verification.Authenticator
{
    internal class AuthenticatorVerifier : IVerifier
    {        
        private readonly string _authenticatorSecretKey;
        public AuthenticatorVerifier(string authenticatorSecretKey)
        {
            _authenticatorSecretKey = authenticatorSecretKey;
        }

        public bool VerifyAsync(string code)
        {
            var secretKey = Base32Encoding.ToBytes(_authenticatorSecretKey);
            var totp = new Totp(secretKey);
            var isValid = totp.VerifyTotp(code, out long timeWindowUsed);
            return isValid;
        }
    }
}
