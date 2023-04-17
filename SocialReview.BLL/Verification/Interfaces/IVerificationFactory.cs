namespace SocialReview.BLL.Verification.Interfaces
{
    public interface IVerificationFactory
    {
        IVerifier CreateVerifier();
        ICodeSender CreateCodeSender();
    }
}
