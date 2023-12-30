namespace HotelChief.Infrastructure.Mappers
{
    using AutoMapper;

    public class DomainToInfrastructureMappingProfile : Profile
    {
        public DomainToInfrastructureMappingProfile()
        {
            CreateMap<HotelChief.Core.Entities.Identity.Guest, HotelChief.Infrastructure.EFEntities.Guest>();
        }
    }
}
