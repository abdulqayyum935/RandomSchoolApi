using CrudAPIWithRepositoryPattern.Filter;
using CrudAPIWithRepositoryPattern.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAPIWithRepositoryPattern.IRepositories
{
   public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomers(PaginationFilter filter);
        Task<Customer> GetCustomer(int id);
        Task<int> TotalCustomer();

    }
}
