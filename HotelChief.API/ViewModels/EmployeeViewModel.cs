namespace HotelChief.API.ViewModels
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        public string? FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? Role { get; set; }

        public decimal Salary { get; set; }

        public DateTime HireDate { get; set; }
    }
}
