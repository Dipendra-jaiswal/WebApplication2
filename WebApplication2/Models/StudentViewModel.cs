﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Course { get; set; }

        public string Subject { get; set; }

        public IFormFile Photo { get; set; }
    }
}
