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
    internal class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.RoomNumber);
            builder.Property(r => r.RoomType).HasMaxLength(50); 
            builder.Property(r => r.IsAvailable).IsRequired();

        }
    }
}
