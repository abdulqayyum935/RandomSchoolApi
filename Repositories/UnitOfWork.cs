using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Models;
using CrudAPIWithRepositoryPattern.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CrudAPIWithRepositoryPattern.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly RandomSchoolContext _context;
        public UnitOfWork(RandomSchoolContext context)
        {
            _context = context;
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
