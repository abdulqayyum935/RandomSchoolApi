using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Models;
using CrudAPIWithRepositoryPattern.IRepositories;
using Microsoft.EntityFrameworkCore;
using CrudAPIWithRepositoryPattern.ViewModels;

namespace CrudAPIWithRepositoryPattern.Repositories
{
    public class StudentRepository:IStudentRepository
    {
        private readonly RandomSchoolContext _context;
        public StudentRepository(RandomSchoolContext context)
        {
            _context = context;
        }
        public  IEnumerable<Student> Get()
        {

            //var result = (from student in _context.Students
            //              join studentCourse in _context.StudentCourses on student.ID equals studentCourse.StudentId
            //              group studentCourse by student into r
            //              select new StudentListViewModel
            //              {
            //                  Count = r.Count(),
            //                  Name = r.Key.Name,
            //                  ID = r.Key.ID
            //              }

            //            ).ToList();
            //return result;

           return _context.Students.Select(x=>new Student{ID=x.ID,Name=x.Name,StudentCourses=x.StudentCourses,Email=x.Email,DateOfBirth=x.DateOfBirth }).ToList();
        }
        public async Task<Student> GetStudent(int? id)
        {
            return await _context.Students
                .Include(x => x.StudentCourses).ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(m => m.ID == id);
        }
        public IQueryable<Student> GetStudentAsQueryable(int? id)
        {
            return _context.Students.Where(x => x.ID == id)
                .Include(x => x.StudentCourses).ThenInclude(x => x.Course)
                .AsQueryable();
        }

        public async Task<Student> Add(Student student)
        {
            await _context.Students.AddAsync(student);
            return student;
        }

        public Student Update(Student student)
        {
            _context.Update(student);
            return student;
        }
        public void RemoveStudent(Student student)
        {
            _context.Students.Remove(student);
        }
        public bool StudentExists(int? id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
