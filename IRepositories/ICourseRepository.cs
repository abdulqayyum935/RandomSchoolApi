using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Models;

namespace CrudAPIWithRepositoryPattern.IRepositories
{
   public interface ICourseRepository
    {
        Task<IEnumerable<Course>> Get();

        Task<Course> GetCourse(int? id);

        IEnumerable<Course> GetActiveCourse();
        Task<Course> Add(Course course);

        Course Update(int id,Course course);

        bool Delete(int? id);
        public bool CourseExists(int id);
    }
}
