using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Course { get; set; }

        public string Subject { get; set; }

        public string Photo { get; set; }

       // public string multipl { get; set; } 
    }
}
