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

            builder.HasOne(hso => hso.Guest).WithMany().IsRequired();
            builder.HasOne(hso => hso.Service).WithMany().IsRequired();
            builder.HasOne(hso => hso.Employee).WithMany().IsRequired();

        }
    }
}
