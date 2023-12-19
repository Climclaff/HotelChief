namespace HotelChief.Infrastructure.Mappers
{
    using AutoMapper;

    public class DomainToInfrastructureMappingProfile : Profile
    {
        public DomainToInfrastructureMappingProfile()
        {
            CreateMap<HotelChief.Core.Entities.Identity.Guest, HotelChief.Infrastructure.EFEntities.Guest>();
            CreateMap<HotelChief.Core.Entities.Employee, HotelChief.Infrastructure.EFEntities.Employee>();
            CreateMap<HotelChief.Core.Entities.HotelService, HotelChief.Infrastructure.EFEntities.HotelService>();
            CreateMap<HotelChief.Core.Entities.HotelServiceOrder, HotelChief.Infrastructure.EFEntities.HotelServiceOrder>();
            CreateMap<HotelChief.Core.Entities.Reservation, HotelChief.Infrastructure.EFEntities.Reservation>();
            CreateMap<HotelChief.Core.Entities.Review, HotelChief.Infrastructure.EFEntities.Review>();
            CreateMap<HotelChief.Core.Entities.Room, HotelChief.Infrastructure.EFEntities.Room>();
        }
    }
}
