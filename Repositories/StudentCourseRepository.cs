using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Models;
using Microsoft.EntityFrameworkCore;
using CrudAPIWithRepositoryPattern.IRepositories;

namespace CrudAPIWithRepositoryPattern.Repositories
{
    public class StudentCourseRepository : IStudentCourseRepository
    {
        private readonly RandomSchoolContext _context;
        public StudentCourseRepository(RandomSchoolContext context)
        {
            _context = context;
        }
        public void RemoveRange(int studentId)
        {
            _context.StudentCourses.RemoveRange(_context.StudentCourses.Where(x => x.StudentId == studentId));
        }
        public StudentCourse Add(StudentCourse studentCourse)
        {
            _context.StudentCourses.Add(studentCourse);
            return studentCourse;
        }
        public IEnumerable<StudentCourse> GetStudentCoursesByStudentId(int studentId)
        {
            return _context.StudentCourses.Where(x => x.StudentId == studentId);
        }
    }
}
