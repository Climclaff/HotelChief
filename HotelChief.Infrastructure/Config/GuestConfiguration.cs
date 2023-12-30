namespace HotelChief.Infrastructure.Config
{
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class GuestConfiguration : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.ToTable("dbo.AspNetUsers");
            builder.Property(g => g.FullName).HasMaxLength(255);
        }
    }
}
