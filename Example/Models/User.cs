using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Models
{
    public class User : IdentityUser
    {
        public string SName { get; set; }

        public bool News { get; set; }
    }
}
