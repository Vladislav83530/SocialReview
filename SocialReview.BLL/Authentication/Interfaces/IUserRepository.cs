using SocialReview.DAL.Entities;

namespace SocialReview.BLL.Authentication.Interfaces
{
    public interface IUserRepository
    {
        Task SaveUserAsync<T>(User user, T userInfo);
        Task<bool> IsRegisteredAsync(string email);
        Task<bool> IsRegisteredByPhoneNumberAsync(string phoneNumber);
        Task<User> GetUserByEmailAsync(string email);
    }
}
