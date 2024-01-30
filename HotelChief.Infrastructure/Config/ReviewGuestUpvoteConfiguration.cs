namespace HotelChief.Infrastructure.Config
{
    using HotelChief.Core.Entities;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class ReviewGuestUpvoteConfiguration : IEntityTypeConfiguration<ReviewGuestUpvote>
    {
        public void Configure(EntityTypeBuilder<ReviewGuestUpvote> builder)
        {
            builder.HasKey(pt => pt.UpvoteId);

            builder.ToTable("ReviewUpvotes");
        }
    }
}
