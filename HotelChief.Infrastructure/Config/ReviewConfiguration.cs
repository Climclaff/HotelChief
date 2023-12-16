using HotelChief.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelChief.Infrastructure.Config
{
    internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.ReviewId);
            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.Comment).HasMaxLength(255);
            builder.Property(r => r.Timestamp).IsRequired();


            builder.HasOne(r => r.Guest)
                .WithMany()
                .IsRequired();

        }
    }
}
