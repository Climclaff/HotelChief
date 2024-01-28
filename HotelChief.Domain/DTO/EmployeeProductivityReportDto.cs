namespace HotelChief.Core.DTO
{
    public class EmployeeProductivityReportDto
    {
        public int EmployeeId { get; set; }

        public string? FullName { get; set; }

        public int HotelServiceOrdersCompleted { get; set; }

        public int RoomCleaningsCompleted { get; set; }
    }
}
