namespace HotelChief.API.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        public string? FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public string? Role { get; set; }

        public decimal Salary { get; set; }

        public DateTime HireDate { get; set; }

        public string? NormalizedUserName { get; set; }
    }
}
