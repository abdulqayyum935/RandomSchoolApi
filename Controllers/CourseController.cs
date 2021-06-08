using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.IRepositories;
using CrudAPIWithRepositoryPattern.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrudAPIWithRepositoryPattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Student")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        public CourseController(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
        {
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/<CourseController>
        [HttpGet]
        public async Task<IEnumerable<Course>> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var user = HttpContext.User.Identity.Name;
            return await _courseRepository.Get();
        }

        // GET api/<CourseController>/5
        [HttpGet("{id}")]
        public async Task<Course> Get(int id)
        {
            return await _courseRepository.GetCourse(id);
        }

        // POST api/<CourseController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Course course)
        {
            await _courseRepository.Add(course);
            await _unitOfWork.SaveChangesAsync();
            return Ok(course);
        }

        // PUT api/<CourseController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Course course)
        {
            if (id != course.ID)
            {
                return BadRequest();
            }
            _courseRepository.Update(id, course);
            await _unitOfWork.SaveChangesAsync();
            return Ok(course);
        }

        // DELETE api/<CourseController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this._courseRepository.Delete(id);
            _unitOfWork.SaveChanges();
            return Ok();
        }
    }
}
