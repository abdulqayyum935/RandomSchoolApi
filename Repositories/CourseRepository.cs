using CrudAPIWithRepositoryPattern.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudAPIWithRepositoryPattern.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly RandomSchoolContext _context;
        public CourseRepository(RandomSchoolContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Course>> Get()
        {
            return await _context.Courses.ToListAsync();
        }
        public async Task<Course> GetCourse(int? id)
        {
            return await _context.Courses.FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<Course> Add(Course course)
        {
            await _context.AddAsync(course);
            return course;
        }
        public Course Update(int id, Course course)
        {

            _context.Entry(course).State = EntityState.Modified;

            return course;
        }

        public bool Delete(int? id)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return false;
            _context.Courses.Remove(course);
            return true;
        }
        public bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.ID == id);
        }
        public IEnumerable<Course> GetActiveCourse()
        {
            return _context.Courses.Where(x => x.IsActive).AsQueryable();
        }

    }
}
