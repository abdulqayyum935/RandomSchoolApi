using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Models;

namespace CrudAPIWithRepositoryPattern.IRepositories
{
   public interface IStudentCourseRepository
    {
        StudentCourse Add(StudentCourse studentCourse);
        IEnumerable<StudentCourse> GetStudentCoursesByStudentId(int studentId);
        void RemoveRange(int studentId);
    }
}
