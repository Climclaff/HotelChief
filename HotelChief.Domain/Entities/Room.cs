namespace HotelChief.Core.Entities
{
    public class Room
    {
        public int RoomNumber { get; set; }

        public string? RoomType { get; set; }

        public bool IsAvailable { get; set; }
    }
}
