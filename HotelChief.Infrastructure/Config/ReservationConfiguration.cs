namespace HotelChief.Infrastructure.Config
{
    using HotelChief.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.ReservationId);
            builder.Property(r => r.ReservationId).HasColumnName("ReservationId");

            builder.Property(r => r.CheckInDate).IsRequired();
            builder.Property(r => r.CheckOutDate).IsRequired();
            builder.Property(r => r.Amount).IsRequired();
            builder.Property(r => r.PaymentStatus).IsRequired();
            builder.Property(r => r.Timestamp).IsRequired();

            builder.HasOne(h => h.Room).WithMany().HasForeignKey(h => h.RoomNumber).IsRequired();
        }
    }
}
