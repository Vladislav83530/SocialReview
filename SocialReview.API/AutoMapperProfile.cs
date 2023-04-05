using AutoMapper;
using SocialReview.BLL.Authentication.Models;
using SocialReview.DAL.Entities;

namespace SocialReview.API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CustomerRegisterDto, User>();
            CreateMap<CustomerRegisterDto, Customer>();
            CreateMap<EstablishmentRegisterDto, User>();
            CreateMap<EstablishmentRegisterDto, Establishment>();
        }
    }
}
