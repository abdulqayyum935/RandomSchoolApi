using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Models;
using CrudAPIWithRepositoryPattern.IRepositories;

namespace CrudAPIWithRepositoryPattern.Repositories
{
    

    public class PersonRepository : IPersonRepository
    {
        private readonly RandomSchoolContext _context;
        public PersonRepository(RandomSchoolContext context)
        {
            _context = context;
        }
        public async Task<Person> Add(Person person)
        {
            await _context.Person.AddAsync(person);
            return person;
        }
    }
}
