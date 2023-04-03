using SocialReview.BLL.Authentication.Models;
using SocialReview.DAL.Entities;

namespace SocialReview.BLL.Authentication.Interfaces
{
    public interface IEstablishmentAuthService
    {
        Task<Establishment> RegisterAsync(EstablishmentRegisterDto request);
    }
}
