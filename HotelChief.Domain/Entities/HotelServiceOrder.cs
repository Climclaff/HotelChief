using HotelChief.Domain.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelChief.Domain.Entities
{
    public class HotelServiceOrder : Booking
    {
        public int HotelServiceOrderId { get; set; }
        public Guest Guest { get; set; }
        public HotelService? Service { get; set; }
        public Employee? Employee { get; set; }
        public DateTime ServiceOrderDate { get; set; }
        public int Quantity { get; set; }
    }
}
