namespace HotelChief.Infrastructure.Config
{
    using HotelChief.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class RoomCleaningConfiguration : IEntityTypeConfiguration<RoomCleaning>
    {
        public void Configure(EntityTypeBuilder<RoomCleaning> builder)
        {
            builder.HasKey(rc => rc.RoomCleaningId);

            builder.HasOne(rc => rc.Room)
                .WithMany(r => r.RoomCleanings)
                .HasForeignKey(rc => rc.RoomNumber)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rc => rc.Employee)
                .WithMany(e => e.RoomCleanings)
                .HasForeignKey(rc => rc.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
