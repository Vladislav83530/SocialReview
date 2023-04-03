using SocialReview.BLL.Authentication.Models;
using SocialReview.DAL.Entities;

namespace SocialReview.BLL.Authentication.Interfaces
{
    public interface ICustomerAuthService
    {
        Task<Customer> RegisterAsync(CustomerRegisterDto request);
    }
}
