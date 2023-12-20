namespace HotelChief.API.Helpers
{
    using AutoMapper;
    using HotelChief.Infrastructure.EFEntities;

    public class ViewModelToInfrastructureMappingProfile : Profile
    {
        public ViewModelToInfrastructureMappingProfile()
        {
            CreateMap<HotelChief.API.ViewModels.EmployeeViewModel, HotelChief.Infrastructure.EFEntities.Employee>();
            CreateMap<HotelChief.API.ViewModels.GuestViewModel, HotelChief.Infrastructure.EFEntities.Guest>();
            CreateMap<HotelChief.API.ViewModels.HotelServiceViewModel, HotelChief.Infrastructure.EFEntities.HotelService>();
            CreateMap<HotelChief.API.ViewModels.HotelServiceOrderViewModel, HotelChief.Infrastructure.EFEntities.HotelServiceOrder>();
            CreateMap<HotelChief.API.ViewModels.ReservationViewModel, HotelChief.Infrastructure.EFEntities.Reservation>();
            CreateMap<HotelChief.API.ViewModels.ReviewViewModel, HotelChief.Infrastructure.EFEntities.Review>();
            CreateMap<HotelChief.API.ViewModels.RoomViewModel, HotelChief.Infrastructure.EFEntities.Room>();
        }
    }
}
