using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelChief.Domain.Entities
{
    public class Room
    {
        public int RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public bool IsAvailable { get; set; }
    }
}
