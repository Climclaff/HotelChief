namespace HotelChief.Infrastructure.Config
{
    using HotelChief.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class HotelServiceConfiguration : IEntityTypeConfiguration<HotelService>
    {
        public void Configure(EntityTypeBuilder<HotelService> builder)
        {
            builder.HasKey(hs => hs.ServiceId);
            builder.Property(hs => hs.ServiceName).IsRequired();
            builder.Property(hs => hs.Description).IsRequired();
            builder.Property(hs => hs.Price).IsRequired();
        }
    }
}
