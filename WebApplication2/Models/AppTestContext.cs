using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class AppTestContext : DbContext
    {
        public AppTestContext(DbContextOptions<AppTestContext> options) : base(options)
        {
                
        }

        public DbSet<Student> Students { get; set; }
    }
}
