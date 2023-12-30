namespace HotelChief.Infrastructure.Config
{
    using HotelChief.Core.Entities;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class HotelServiceOrderConfiguration : IEntityTypeConfiguration<HotelServiceOrder>
    {
        public void Configure(EntityTypeBuilder<HotelServiceOrder> builder)
        {
            builder.HasKey(h => h.HotelServiceOrderId);

            builder.Property(h => h.ServiceOrderDate).IsRequired();
            builder.Property(h => h.Quantity).IsRequired();
            builder.Property(h => h.Amount).IsRequired();
            builder.Property(h => h.PaymentStatus).IsRequired();
            builder.Property(h => h.Timestamp).IsRequired();

            builder.HasOne<Guest>().WithMany().HasForeignKey(h => h.GuestId).IsRequired();
        }
    }
}
