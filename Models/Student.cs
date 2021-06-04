using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAPIWithRepositoryPattern.Models
{
    public class Student
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [Display(Name = "Date of birth")]
        public DateTime? DateOfBirth { get; set; }

        public IList<StudentCourse> StudentCourses { get; set; }
    }
}
