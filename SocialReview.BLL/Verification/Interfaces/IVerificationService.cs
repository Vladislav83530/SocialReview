using Microsoft.AspNetCore.Http;
using SocialReview.BLL.Verification.Models;

namespace SocialReview.BLL.Verification.Interfaces
{
    public interface IVerificationService
    {
        Task SendCodeAsync(SendingType type);
        Task<bool> VerifyUserAsync(HttpContext context, SendingType type, string code);
    }
}
