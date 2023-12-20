namespace HotelChief.Infrastructure.Config
{
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class HotelServiceOrderConfiguration : IEntityTypeConfiguration<HotelServiceOrder>
    {
        public void Configure(EntityTypeBuilder<HotelServiceOrder> builder)
        {
            builder.HasKey(hso => hso.HotelServiceOrderId);
            builder.Property(hso => hso.ServiceOrderDate).IsRequired();
            builder.Property(hso => hso.Quantity).IsRequired();

            builder.HasOne(hso => hso.Guest).WithMany().HasForeignKey(so => so.GuestId).IsRequired();
            builder.HasOne(hso => hso.Service).WithMany().HasForeignKey(so => so.ServiceId).IsRequired();
            builder.HasOne(hso => hso.Employee).WithMany().HasForeignKey(so => so.EmployeeId).IsRequired();

        }
    }
}
