using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Controllers;

namespace WebApplication2.Models
{
    public class StudentClass
    {
        private readonly IList<Student> students = new List<Student>();

        public StudentClass()
        {            
            students = new List<Student> {
            new Student{ Id = 1, Name ="Ramesh",Course="MCA",Subject="CS" },
            new Student{ Id = 2, Name ="Suresh",Course="BTECH",Subject="IT" },
            new Student{ Id = 3, Name ="Rajesh",Course="MCA",Subject="CS" }
            };
           // throw new Exception("exception");
        }



    }
}
