﻿namespace HotelChief.Infrastructure.EFEntities
{
    public class HotelService
    {
        public int ServiceId { get; set; }

        public string? ServiceName { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }
    }
}
