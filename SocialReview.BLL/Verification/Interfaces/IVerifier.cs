namespace SocialReview.BLL.Verification.Interfaces
{
    public interface IVerifier
    {
        bool VerifyAsync(string code);
    }
}
