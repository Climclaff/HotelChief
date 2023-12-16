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
    internal class HotelServiceOrderConfiguration : IEntityTypeConfiguration<HotelServiceOrder>
    {
        public void Configure(EntityTypeBuilder<HotelServiceOrder> builder)
        {
            builder.HasKey(hso => hso.HotelServiceOrderId);
            builder.Property(hso => hso.ServiceOrderDate).IsRequired();
            builder.Property(hso => hso.Quantity).IsRequired();

            builder.HasOne(hso => hso.Guest).WithMany().IsRequired();
            builder.HasOne(hso => hso.Service).WithMany().IsRequired();
            builder.HasOne(hso => hso.Employee).WithMany().IsRequired();

        }
    }
}
