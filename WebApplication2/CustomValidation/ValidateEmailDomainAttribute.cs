using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.CustomValidation
{
    public class ValidateEmailDomainAttribute : ValidationAttribute
    {
        private readonly string _allowDomain;

        public ValidateEmailDomainAttribute(string allowDomain)
        {
            this._allowDomain = allowDomain;
        }

        public override bool IsValid(object value)
        {
           string[] strArray = value.ToString().Split('@');
            return strArray[1].ToUpper() == _allowDomain.ToUpper();
        }
    }

    public class ValidateUserNameAttribute : ValidationAttribute
    {
        private readonly string _allowDomain;

        public ValidateUserNameAttribute(string allowDomain)
        {
            this._allowDomain = allowDomain;
        }

        public override bool IsValid(object value)
        {
            return value.ToString().ToUpper() == _allowDomain.ToUpper();
        }
    }
}
