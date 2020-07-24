using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class CustomIdentityUser: IdentityUser
    {
        public virtual string UserProfession { get; set; }
    }
}
