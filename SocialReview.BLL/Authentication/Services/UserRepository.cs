using Microsoft.EntityFrameworkCore;
using SocialReview.BLL.Authentication.Interfaces;
using SocialReview.DAL.EF;
using SocialReview.DAL.Entities;

namespace SocialReview.BLL.Authentication.Services
{
    /// <summary>
    /// A repository for managing User data.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Saves a User and its associated information to the database.
        /// </summary>
        /// <typeparam name="T">The type of the user information.</typeparam>
        /// <param name="user">The User to save.</param>
        /// <param name="userInfo">The user information to save.</param>
        public async Task SaveUserAsync<T>(User user, T userInfo)
        {
            if(userInfo is Customer)
                _dbContext.Customers.Add(userInfo as Customer);
            if(userInfo is Establishment)
                _dbContext.Establishments.Add(userInfo as Establishment);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Checks if a User with the given email is registered.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the User is registered, false otherwise.</returns>
        public async Task<bool> IsRegisteredAsync(string email)
        {
            string normalizeEmail = email.ToLower();
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == normalizeEmail);
            return user != null;
        }
    }
}
