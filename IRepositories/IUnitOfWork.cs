using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAPIWithRepositoryPattern.IRepositories
{
    public interface IUnitOfWork
    {
        public void SaveChanges();
        public Task<int> SaveChangesAsync();

    }
}
