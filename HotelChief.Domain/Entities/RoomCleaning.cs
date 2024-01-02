namespace HotelChief.Core.Entities
{
    public class RoomCleaning
    {
        public int RoomCleaningId { get; set; }

        public int? RoomNumber { get; set; }

        public Room? Room { get; set; }

        public int? EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
