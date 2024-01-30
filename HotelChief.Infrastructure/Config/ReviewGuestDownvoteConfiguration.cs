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

            builder.ToTable("ReviewDownvotes");
        }
    }
}
