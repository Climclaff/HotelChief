using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelChief.Domain.Entities.Identity
{
    public class Guest : IdentityUser<int>
    {
        public string? FullName { get; set; }

    }
}
