using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAPIWithRepositoryPattern.Models
{
    public class Course
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Course Title"), Required(ErrorMessage = "Course Title is required."), MinLength(3, ErrorMessage = "Invalid Title, Title Should be longer than 3 character.")]
        public string Title { get; set; }

        [Display(Name = "Credit Hours"), Required(ErrorMessage = "Credit Hour value is required."), Range(1, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int CreditHour { get; set; }
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

        public IList<StudentCourse> StudentCourses { get; set; }
    }
}
