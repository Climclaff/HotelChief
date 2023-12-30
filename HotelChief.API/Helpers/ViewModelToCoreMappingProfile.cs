namespace HotelChief.API.Helpers
{
    using AutoMapper;
    using HotelChief.Core.Entities;
    using HotelChief.Infrastructure.EFEntities;

    public class ViewModelToCoreMappingProfile : Profile
    {
        public ViewModelToCoreMappingProfile()
        {
            CreateMap<HotelChief.API.ViewModels.EmployeeViewModel, HotelChief.Core.Entities.Employee>();
            CreateMap<HotelChief.API.ViewModels.GuestViewModel, HotelChief.Infrastructure.EFEntities.Guest>();
            CreateMap<HotelChief.API.ViewModels.HotelServiceViewModel, HotelChief.Core.Entities.HotelService>();
            CreateMap<HotelChief.API.ViewModels.HotelServiceOrderViewModel, HotelChief.Core.Entities.HotelServiceOrder>();
            CreateMap<HotelChief.API.ViewModels.ReservationViewModel, HotelChief.Core.Entities.Reservation>();
            CreateMap<HotelChief.API.ViewModels.ReviewViewModel, HotelChief.Core.Entities.Review>();
            CreateMap<HotelChief.API.ViewModels.RoomViewModel, HotelChief.Core.Entities.Room>();
        }
    }
}
