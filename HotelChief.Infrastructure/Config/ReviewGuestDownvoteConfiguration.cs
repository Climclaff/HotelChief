namespace HotelChief.Infrastructure.Config
{
    using HotelChief.Core.Entities;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class ReviewGuestDownvoteConfiguration : IEntityTypeConfiguration<ReviewGuestDownvote>
    {
        public void Configure(EntityTypeBuilder<ReviewGuestDownvote> builder)
        {
            builder.HasKey(pt => pt.DownvoteId);

           // builder.HasOne<Guest>().WithMany().HasForeignKey(h => h.GuestId).IsRequired();

            //builder.HasOne<Review>().WithMany().HasForeignKey(h => h.ReviewId).IsRequired();

            builder.ToTable("ReviewDownvotes");
        }
    }
}
