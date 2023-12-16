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
    internal class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.ReservationId);
            builder.Property(r => r.CheckInDate).IsRequired();
            builder.Property(r => r.CheckOutDate).IsRequired();

            builder.HasOne(r => r.Guest)
                .WithMany()
                .IsRequired();

            builder.HasOne(r => r.Room)
                .WithMany()
                .IsRequired();

        }
    }
}
