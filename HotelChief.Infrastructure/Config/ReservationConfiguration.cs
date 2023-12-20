namespace HotelChief.Infrastructure.Config
{
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.ReservationId);
            builder.Property(r => r.GuestId).IsRequired();
            builder.Property(r => r.CheckInDate).IsRequired();
            builder.Property(r => r.CheckInDate).IsRequired();
            builder.Property(r => r.CheckOutDate).IsRequired();

            builder.HasOne(r => r.Guest)
                .WithMany()
                .HasForeignKey(r => r.GuestId)
                .IsRequired();

            builder.HasOne(r => r.Room)
                .WithMany()
                .HasForeignKey(r => r.RoomNumber)
                .IsRequired();
        }
    }
}
