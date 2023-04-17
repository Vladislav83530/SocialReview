namespace SocialReview.BLL.Verification.Interfaces
{
    public interface IVerifier
    {
        Task<bool> VerifyAsync(string code);
    }
}
