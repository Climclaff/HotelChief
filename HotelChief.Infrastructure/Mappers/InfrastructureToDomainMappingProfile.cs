namespace HotelChief.Infrastructure.Mappers
{
    using AutoMapper;

    public class InfrastructureToDomainMappingProfile : Profile
    {
        public InfrastructureToDomainMappingProfile()
        {
            CreateMap<HotelChief.Infrastructure.EFEntities.Guest, HotelChief.Core.Entities.Identity.Guest>();
            CreateMap<HotelChief.Infrastructure.EFEntities.Employee, HotelChief.Core.Entities.Employee>();
            CreateMap<HotelChief.Infrastructure.EFEntities.HotelService, HotelChief.Core.Entities.HotelService>();
            CreateMap<HotelChief.Infrastructure.EFEntities.HotelServiceOrder, HotelChief.Core.Entities.HotelServiceOrder>();
            CreateMap<HotelChief.Infrastructure.EFEntities.Reservation, HotelChief.Core.Entities.Reservation>();
            CreateMap<HotelChief.Infrastructure.EFEntities.Review, HotelChief.Core.Entities.Review>();
            CreateMap<HotelChief.Infrastructure.EFEntities.Room, HotelChief.Core.Entities.Room>();
        }
    }
}
