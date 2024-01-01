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

            builder.HasMany(r => r.Upvoters)
                .WithOne()
                .HasForeignKey(ur => ur.ReviewId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(r => r.Downvoters)
                .WithOne()
                .HasForeignKey(dr => dr.ReviewId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
