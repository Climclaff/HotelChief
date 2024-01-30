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

            builder.HasMany(r => r.UpvotedReviews)
                .WithOne()
                .HasForeignKey(ur => ur.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.DownvotedReviews)
                .WithOne()
                .HasForeignKey(dr => dr.GuestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
