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
    internal class HotelServiceConfiguration : IEntityTypeConfiguration<HotelService>
    {
        public void Configure(EntityTypeBuilder<HotelService> builder)
        {
            builder.HasKey(hs => hs.ServiceId);
            builder.Property(hs => hs.ServiceName).IsRequired();
            builder.Property(hs => hs.Description).IsRequired();
            builder.Property(hs => hs.Price).IsRequired();
        }
    }
}
