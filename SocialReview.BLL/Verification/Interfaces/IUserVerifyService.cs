using Microsoft.AspNetCore.Http;

namespace SocialReview.BLL.Verification.Interfaces
{
    public interface IUserVerifyService
    {
        string GetUserEmailFromClaims(HttpContext context);
        Task UpdateIsVerifiedValueAsync(string email);
        Task<string> GenerateVerificationCode(string email);
        string GetUserVerificationCode(string email);
    }
}
