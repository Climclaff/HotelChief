namespace HotelChief.Core.Entities
{
    public class HotelService
    {
        public int ServiceId { get; set; }

        public string? ServiceName { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public ICollection<HotelServiceOrder> HotelServiceOrders { get; } = new List<HotelServiceOrder>();
    }
    }
