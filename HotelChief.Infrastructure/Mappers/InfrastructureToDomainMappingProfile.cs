namespace HotelChief.Infrastructure.Mappers
{
    using AutoMapper;

    public class InfrastructureToDomainMappingProfile : Profile
    {
        public InfrastructureToDomainMappingProfile()
        {
            CreateMap<HotelChief.Infrastructure.EFEntities.Guest, HotelChief.Core.Entities.Identity.Guest>();
        }
    }
}
