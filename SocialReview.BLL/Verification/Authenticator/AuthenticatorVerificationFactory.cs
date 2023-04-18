using SocialReview.BLL.Verification.Interfaces;

namespace SocialReview.BLL.Verification.Authenticator
{
    public class AuthenticatorVerificationFactory : IVerificationFactory
    {
        private readonly string _authenticatorSecretKey;

        public AuthenticatorVerificationFactory(string authenticatorSecretKey)
        {
            _authenticatorSecretKey = authenticatorSecretKey;
        }

        public ICodeSender CreateCodeSender()
        {
            return new AuthenticatorCodeSender();
        }

        public IVerifier CreateVerifier()
        {
            return new AuthenticatorVerifier(_authenticatorSecretKey);
        }
    }
}
