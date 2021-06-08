using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudAPIWithRepositoryPattern.Repositories
{
    public class CustomerRepository
    {
        private readonly RandomSchoolContext context;

        public CustomerRepository(RandomSchoolContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await context.Customers.ToListAsync();
        }
    }
}
