using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.IRepositories;
using CrudAPIWithRepositoryPattern.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudAPIWithRepositoryPattern.Repositories
{
    public class PersonSkillRepository: IPersonSkillRepository
    {
        private readonly RandomSchoolContext _context;

        public PersonSkillRepository(RandomSchoolContext context)
        {
            _context = context;
        }

        public async Task<bool> SavePersonSkills(PersonSkills[] personSkills)
        {
            await _context.PersonSkills.AddRangeAsync(personSkills);
            return true;
        }
        public IEnumerable<PersonSkills> GetSkills(int personId)
        {
            return _context.PersonSkills.Where(x => x.PersonId == personId).ToList();
        }
    }
}
