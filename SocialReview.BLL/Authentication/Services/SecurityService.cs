using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialReview.BLL.Authentication.Interfaces;
using SocialReview.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SocialReview.BLL.Authentication.Services
{
    /// <summary>
    /// A service for managing security-related operations.
    /// </summary>
    public class SecurityService : ISecurityService
    {
        private readonly IConfiguration _config;

        public SecurityService(IConfiguration config)
        {
            _config = config;
        }

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

        /// <summary>
        /// Creates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to create the token.</param>
        /// <returns>A JWT token for the specified user.</returns>
        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        /// <summary>
        /// Verifies that the provided password matches the stored password hash and salt.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="passwordHash">The stored password hash.</param>
        /// <param name="passwordSalt">The stored password salt.</param>
        /// <returns>True if the password is verified, false otherwise.</returns>
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
