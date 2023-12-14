using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelChief.Domain.Entities
{
    public class Guest // Considering this class is going to inherit IdentityUser, we are not adding too much here.
    {
        public string? FullName { get; set; }

    }
}
