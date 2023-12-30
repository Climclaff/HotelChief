namespace HotelChief.Infrastructure.Config
{
    using HotelChief.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.RoomNumber);
            builder.Property(r => r.RoomType).HasMaxLength(50);
            builder.Property(r => r.IsAvailable).IsRequired();

            builder.HasMany(r => r.Reservations)
           .WithOne(res => res.Room)
           .HasForeignKey(res => res.RoomNumber);
        }
    }
}
