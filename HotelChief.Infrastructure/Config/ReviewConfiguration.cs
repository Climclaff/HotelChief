namespace HotelChief.Infrastructure.Config
{
    using HotelChief.Core.Entities;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.ReviewId);
            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.Comment).HasMaxLength(255);
            builder.Property(r => r.Timestamp).IsRequired();

            builder.HasOne<Guest>()
                .WithMany()
                .HasForeignKey(r => r.GuestId)
                .IsRequired();
        }
    }
}
