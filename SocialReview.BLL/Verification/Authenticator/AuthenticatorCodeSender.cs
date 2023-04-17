using SocialReview.BLL.Verification.Interfaces;

namespace SocialReview.BLL.Verification.Authenticator
{
    internal class AuthenticatorCodeSender : ICodeSender
    {
        public Task SendCodeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
