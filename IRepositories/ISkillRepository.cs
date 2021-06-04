using CrudAPIWithRepositoryPattern.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAPIWithRepositoryPattern.IRepositories
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skill>> Get();
    }
}
