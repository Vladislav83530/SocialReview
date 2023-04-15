﻿using Microsoft.Extensions.Configuration;
using Moq;
using SocialReview.BLL.Authentication.Services;
using SocialReview.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialReview.UnitTests.BLL.Authentication.Services
{
    [TestFixture]
    internal class SecurityServiceTests
    {
        private SecurityService _securityService;
        private IConfiguration _configuration;

        [SetUp] 
        public void SetUp() 
        {
            _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "AppSettings:Token", "5266556A586E3272357538782F413F44" }
            })
            .Build();
            _securityService = new SecurityService(_configuration);
        }

        [Test]
        public void CreatePasswordHash_WhenCalled_CreatePasswordHash_WhenCalled_OutputsPasswordHashAndSalt()
        {
            var password = "password";
            byte[] passwordHash;
            byte[] passwordSalt;

            _securityService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            Assert.That(passwordHash, Is.Not.Null);
            Assert.That(passwordSalt, Is.Not.Null);
        }

        [Test]
        public void CreatePasswordHash_DifferentPasswords_OutputDifferentHashes()
        {
            var password1 = "password1";
            byte[] passwordHash1;
            byte[] passwordSalt1;

            var password2 = "password2";
            byte[] passwordHash2;
            byte[] passwordSalt2;

            _securityService.CreatePasswordHash(password1, out passwordHash1, out passwordSalt1);
            _securityService.CreatePasswordHash(password2, out passwordHash2, out passwordSalt2);

            Assert.That(passwordHash1, Is.Not.EqualTo(passwordHash2));
        }

        [Test]
        public void CreateToken_WhenCalled_ReturnsToken()
        {
            var user = new User { Email = "test@example.com", Role = Role.Customer };
            var expectedEmailClaim = new Claim(ClaimTypes.Email, user.Email);
            var expectedRoleClaim = new Claim(ClaimTypes.Role, user.Role.ToString());

            var token = _securityService.CreateToken(user);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            Assert.AreEqual(expectedEmailClaim.Value, emailClaim.Value);
            Assert.AreEqual(expectedRoleClaim.Value, roleClaim.Value);
        }

        [Test]
        public void CreateToken_DifferentUsers_ReturnsDifferentTokens()
        {
            var user1 = new User { Email = "test1@test.com", Role = Role.Customer };
            var user2 = new User { Email = "test2@test.com", Role = Role.Establishment };

            var result1 = _securityService.CreateToken(user1);
            var result2 = _securityService.CreateToken(user2);

            Assert.That(result1, Is.Not.EqualTo(result2));
        }

        [Test]
        public void VerifyPasswordHash_PasswordIsCorrect_ReturnsTrue()
        {
            var password = "password";
            byte[] passwordHash;
            byte[] passwordSalt;
            _securityService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var result = _securityService.VerifyPasswordHash(password, passwordHash, passwordSalt);

            Assert.That(result, Is.True);
        }

        [Test]
        public void VerifyPasswordHash_PasswordIsIncorrect_ReturnsFalse()
        {
            var password = "password";
            byte[] passwordHash;
            byte[] passwordSalt;
            _securityService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var result = _securityService.VerifyPasswordHash("wrongpassword", passwordHash, passwordSalt);

            Assert.That(result, Is.False);
        }
    }
}
