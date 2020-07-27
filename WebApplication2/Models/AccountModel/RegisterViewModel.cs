﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models.AccountModel
{
    public class RegisterViewModel
    {
        [Required]
        [Remote(action: "IsUserNameInUse", controller: "Account")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password and confirmed password is not matched")]
        public string ConfirmPassword { get; set; }
    }
}
