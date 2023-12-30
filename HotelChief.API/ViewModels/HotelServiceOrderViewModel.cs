using HotelChief.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HotelChief.API.ViewModels
{
    public class HotelServiceOrderViewModel
    {
        public int HotelServiceOrderId { get; set; }

        [Display(Name = "Guest")]
        public int GuestId { get; set; }

        public IEnumerable<SelectListItem>? Guests { get; set; }

        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        public IEnumerable<SelectListItem>? Employees { get; set; }

        [Display(Name = "Service")]

        public int HotelServiceId { get; set; }

        public IEnumerable<SelectListItem>? Services { get; set; }

        public DateTime ServiceOrderDate { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }

        public bool PaymentStatus { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
