using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.CustomValidation;

namespace WebApplication2.Models.AccountModel
{
    public class RegisterViewModel
    {
        [Required]
        [Remote(action: "IsUserNameInUse", controller: "Account")]
        [ValidateUserName(allowDomain: "Amit", ErrorMessage = " user name must be Amit")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [ValidateEmailDomain(allowDomain: "gmail.com", ErrorMessage = " Email domainmust be gmail.com")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password and confirmed password is not matched")]
        public string ConfirmPassword { get; set; }
    }
}
