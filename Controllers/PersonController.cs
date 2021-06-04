using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Models;
using CrudAPIWithRepositoryPattern.ViewModels;
using CrudAPIWithRepositoryPattern.IRepositories;

namespace CrudAPIWithRepositoryPattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPersonSkillRepository _personSkillRepository;
        private readonly IUnitOfWork _unitofWork;
        public PersonController(IPersonRepository personRepository, IUnitOfWork unitOfWork, IPersonSkillRepository personSkillRepository)
        {
            _personRepository = personRepository;
            _unitofWork = unitOfWork;
            _personSkillRepository = personSkillRepository;
        }


        [Route("/Add")]
        [HttpPost]
        public async Task<Person> Add([FromBody] PersonViewModel p)
        {
            var person = new Person()
            {
                Country = p.Country,
                DateOfBirth = p.DateOfBirth,
                EmailAddress = p.EmailAddress,
                Gender = p.Gender,
                Name = p.Name,
                YearsOfExperience = p.YearsOfExperience
            };
            await _personRepository.Add(person);
            await _unitofWork.SaveChangesAsync();

            if (p.ChoosenSkills.Length > 0)
            {
                var list = new List<PersonSkills>();

                foreach (var item in p.ChoosenSkills)
                {
                    list.Add(new PersonSkills() { PersonId = person.ID, SkillId = item });
                }
                await _personSkillRepository.SavePersonSkills(list.ToArray());
                await _unitofWork.SaveChangesAsync();
                
            }

            return person;
        }
    }
}
