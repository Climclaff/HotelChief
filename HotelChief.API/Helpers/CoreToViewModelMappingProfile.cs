namespace HotelChief.API.Helpers
{
    using AutoMapper;

    public class CoreToViewModelMappingProfile : Profile
    {
        public CoreToViewModelMappingProfile()
        {
            CreateMap<HotelChief.Core.Entities.Employee, HotelChief.API.ViewModels.EmployeeViewModel>();
            CreateMap<HotelChief.Infrastructure.EFEntities.Guest, HotelChief.API.ViewModels.GuestViewModel>();
            CreateMap<HotelChief.Core.Entities.HotelService, HotelChief.API.ViewModels.HotelServiceViewModel>();
            CreateMap<HotelChief.Core.Entities.HotelServiceOrder, HotelChief.API.ViewModels.HotelServiceOrderViewModel>();
            CreateMap<HotelChief.Core.Entities.Reservation, HotelChief.API.ViewModels.ReservationViewModel>();
            CreateMap<HotelChief.Core.Entities.Review, HotelChief.API.ViewModels.ReviewViewModel>();
            CreateMap<HotelChief.Core.Entities.Room, HotelChief.API.ViewModels.RoomViewModel>();
        }
    }
}
