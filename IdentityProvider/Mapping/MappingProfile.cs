using AutoMapper;
using HotelChief.IdentityProvider.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace HotelChief.IdentityProvider.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<IdentityUser, IdentityUserViewModel>();
            CreateMap<IdentityUserViewModel, IdentityUser>();
        } 
    }
}
