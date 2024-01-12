namespace HotelChief.Infrastructure.Config
{
    using HotelChief.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class HotelServiceOrderHistoryConfiguration : IEntityTypeConfiguration<HotelServiceOrderHistory>
    {
        public void Configure(EntityTypeBuilder<HotelServiceOrderHistory> builder)
        {
            builder.HasKey(h => h.HotelServiceOrderHistoryId);
            builder.HasIndex(h => h.EmployeeId).IsUnique(false);
            builder.HasIndex(h => h.HotelServiceId).IsUnique(false);
        }
    }
}
