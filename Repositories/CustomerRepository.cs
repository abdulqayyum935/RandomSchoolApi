using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Filter;
using CrudAPIWithRepositoryPattern.IRepositories;
using CrudAPIWithRepositoryPattern.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudAPIWithRepositoryPattern.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly RandomSchoolContext context;

        public CustomerRepository(RandomSchoolContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Customer>> GetCustomers(PaginationFilter filter)
        {
            return await context.Customers
                .Skip((filter.PageNumber-1)*filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }
        public async Task<Customer> GetCustomer(int id)
        {
            return await context.Customers.FindAsync(id);
        }
         public async Task<int> TotalCustomer()
        {
            return await context.Customers.CountAsync();
        }
    }
}
