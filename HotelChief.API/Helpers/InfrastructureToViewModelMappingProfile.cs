namespace HotelChief.API.Helpers
{
    using AutoMapper;

    public class InfrastructureToViewModelMappingProfile : Profile
    {
        public InfrastructureToViewModelMappingProfile()
        {
            CreateMap<HotelChief.Infrastructure.EFEntities.Employee, HotelChief.API.ViewModels.EmployeeViewModel>();
            CreateMap<HotelChief.Infrastructure.EFEntities.Guest, HotelChief.API.ViewModels.GuestViewModel>();
            CreateMap<HotelChief.Infrastructure.EFEntities.HotelService, HotelChief.API.ViewModels.HotelServiceViewModel>();
            CreateMap<HotelChief.Infrastructure.EFEntities.HotelServiceOrder, HotelChief.API.ViewModels.HotelServiceOrderViewModel>();
            CreateMap<HotelChief.Infrastructure.EFEntities.Reservation, HotelChief.API.ViewModels.ReservationViewModel>();
            CreateMap<HotelChief.Infrastructure.EFEntities.Review, HotelChief.API.ViewModels.ReviewViewModel>();
            CreateMap<HotelChief.Infrastructure.EFEntities.Room, HotelChief.API.ViewModels.RoomViewModel>();
        }
    }
}
