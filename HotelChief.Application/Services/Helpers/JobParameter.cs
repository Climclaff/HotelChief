namespace HotelChief.Application.Services.Helpers
{
    public class JobParameter
    {
        public JobParameter(int roomNumber, string? fullName, int employeeId, DateTime startDate, DateTime endDate)
        {
            RoomNumber = roomNumber;
            FullName = fullName;
            EmployeeId = employeeId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int RoomNumber { get; set; }

        public string? FullName { get; set; }

        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
