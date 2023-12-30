namespace HotelChief.API.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using HotelChief.API.Attributes;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ReservationViewModel
    {
        public int ReservationId { get; set; }

        [Display(Name = "Guest")]
        public int GuestId { get; set; }

        public IEnumerable<SelectListItem>? Guests { get; set; }

        [Display(Name = "Room")]

        public int RoomNumber { get; set; }

        public IEnumerable<SelectListItem>? Rooms { get; set; }

        public DateTime CheckInDate { get; set; }

        [DateGreaterThan("CheckInDate")]
        public DateTime CheckOutDate { get; set; }

        public decimal Amount { get; set; }

        public bool PaymentStatus { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
