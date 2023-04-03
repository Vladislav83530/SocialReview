using SocialReview.DAL.Entities;

namespace SocialReview.BLL.Authentication.Interfaces
{
    public interface ISecurityService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(User user);
    }
}
