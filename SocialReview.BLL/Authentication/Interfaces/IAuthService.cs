using SocialReview.BLL.Authentication.Models;

namespace SocialReview.BLL.Authentication.Interfaces
{
    public interface IAuthService
    {
        Task<T> RegisterAsync<T, TDto>(TDto request) where T : class where TDto : UserCredentialsDto;
        Task<string> LoginAsync(UserLoginDto userLoginDto);
    }
}
