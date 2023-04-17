using Microsoft.Extensions.Configuration;
using SocialReview.BLL.Verification.Interfaces;

namespace SocialReview.BLL.Verification.Authenticator
{
    public class AuthenticatorVerificationFactory : IVerificationFactory
    {
        private readonly IConfiguration _config;

        public AuthenticatorVerificationFactory(IConfiguration config)
        {
            _config = config;
        }

        public ICodeSender CreateCodeSender()
        {
            return new AuthenticatorCodeSender();
        }

        public IVerifier CreateVerifier()
        {
            return new AuthenticatorVerifier(_config);
        }
    }
}
