using CrudAPIWithRepositoryPattern.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAPIWithRepositoryPattern.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        [Required,MaxLength(20,ErrorMessage ="Name could not exceed 20 characters."),MinLength(3,ErrorMessage ="Name can not be less than 3 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Email is required"),EmailAddress(ErrorMessage ="Invalid email address"), ValidEmailDomain(allowedDomain:"gmail.com", ErrorMessage ="Only Gmail address is required")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Date of birth field is required.")]
        [ValidDateOfBirth(ErrorMessage ="Invalid date of birth")]
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
    }
}
