using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.IRepositories;
using CrudAPIWithRepositoryPattern.Models;
using CrudAPIWithRepositoryPattern.Filter;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrudAPIWithRepositoryPattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        // GET: api/<CustomerController>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var custoemrs = await customerRepository.GetCustomers(validFilter);
            var response =new PagedResponse<IEnumerable<Customer>>(custoemrs, validFilter.PageNumber, validFilter.PageSize);
            var totalCustomer= await customerRepository.TotalCustomer();
            response.TotalRecords = totalCustomer;
            response.TotalPages = totalCustomer / validFilter.PageSize;
            return Ok(response);
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await customerRepository.GetCustomer(id);
            return Ok(new Response<Customer>(customer));
        }

        // POST api/<CustomerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
