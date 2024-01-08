namespace HotelChief.Core.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string? FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? Role { get; set; }

        public double Salary { get; set; }

        public bool OnVacation { get; set; }

        public DateTime HireDate { get; set; }

        public ICollection<HotelServiceOrder> HotelServiceOrders { get; } = new List<HotelServiceOrder>();

        public ICollection<RoomCleaning> RoomCleanings { get; set; } = new List<RoomCleaning>();
    }
}
