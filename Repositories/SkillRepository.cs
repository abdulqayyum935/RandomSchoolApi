using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Models;
using Microsoft.EntityFrameworkCore;
using CrudAPIWithRepositoryPattern.IRepositories;

namespace CrudAPIWithRepositoryPattern.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly RandomSchoolContext _context;
        public SkillRepository(RandomSchoolContext context)
        {
            this._context = context;
        }
        public async Task<IEnumerable<Skill>> Get()
        {
            return await _context.Skills.ToArrayAsync();
        }
    }
}
