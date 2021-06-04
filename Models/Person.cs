using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAPIWithRepositoryPattern.Models
{
    public class Person
    {
        [Key]
        public int ID { get; set; }
        [Required,MinLength(3)]
        public string Name { get; set; }
        [Required,EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required,Range(1,150)]
        public Int16 YearsOfExperience { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public IList<Skill> Skills { get; set; }
    }
}
