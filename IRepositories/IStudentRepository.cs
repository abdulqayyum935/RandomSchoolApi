using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Models;
using CrudAPIWithRepositoryPattern.ViewModels;

namespace CrudAPIWithRepositoryPattern.IRepositories
{
   public interface IStudentRepository
    {
        //Task<IEnumerable<Student>> Get();
        // Task<IEnumerable<Student>> GetGetStudent(int? id);
        IEnumerable<Student> Get();

        Task<Student> GetStudent(int? id);
        Task<Student> Add(Student student);

        IQueryable<Student> GetStudentAsQueryable(int? id);

        Student Update(Student student);

        void RemoveStudent(Student student);

        bool StudentExists(int? id);
    }
}
