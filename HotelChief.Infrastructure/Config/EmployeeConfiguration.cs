namespace HotelChief.Infrastructure.Config
{
    using HotelChief.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.EmployeeId);
            builder.Property(e => e.FullName).IsRequired();
            builder.Property(e => e.DateOfBirth).IsRequired();
            builder.Property(e => e.Role).IsRequired();
            builder.Property(e => e.Salary).IsRequired();
            builder.Property(e => e.HireDate).IsRequired();
        }
    }
}
