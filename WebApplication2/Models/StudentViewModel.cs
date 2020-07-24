using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name should not empty.")]
        [MinLength(2,ErrorMessage ="minimum 2 char")]
        [MaxLength(7, ErrorMessage ="max 10 char accepted")]
        public string Name { get; set; }
        //[Range(2,10,ErrorMessage ="between range")]
        // [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email format")]
        [Required(ErrorMessage = "Course should not empty.")]
        [Display(Name = "Office Email")]
        public string Course { get; set; }
        public string Subject { get; set; }

        public IList<IFormFile> Photo { get; set; }

        public string ExistingPath { get; set; }

    }
}
