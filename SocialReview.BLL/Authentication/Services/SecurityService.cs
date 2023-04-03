using SocialReview.BLL.Authentication.Interfaces;
using SocialReview.DAL.Entities;
using System.Security.Cryptography;
using System.Text;

namespace SocialReview.BLL.Authentication.Services
{
    /// <summary>
    /// A service for managing security-related operations.
    /// </summary>
    public class SecurityService : ISecurityService
    {
        /// <summary>
        /// Creates a password hash and salt using the HMACSHA512 hashing algorithm.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="passwordHash">The resulting password hash.</param>
        /// <param name="passwordSalt">The resulting password salt.</param>
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public string CreateToken(User user)
        {
            throw new NotImplementedException();
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            throw new NotImplementedException();
        }
    }
}
