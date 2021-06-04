using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAPIWithRepositoryPattern.CustomValidations
{
    public class ValidDateOfBirthAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dob = Convert.ToDateTime(value);
            if (dob == DateTime.MinValue) return false;
            if (dob < DateTime.Now) return true;
            return false;
        }
    }
}
