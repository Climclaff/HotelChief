namespace HotelChief.API.Helpers
{
    using AutoMapper;

    public class InfrastructureToViewModelMappingProfile : Profile
    {
        public InfrastructureToViewModelMappingProfile()
        {
            CreateMap<HotelChief.Infrastructure.EFEntities.Employee, HotelChief.API.ViewModels.EmployeeViewModel>();
        }
    }
}
