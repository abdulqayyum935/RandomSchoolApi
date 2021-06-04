using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.IRepositories;

namespace CrudAPIWithRepositoryPattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillRepository _skill;

        public SkillsController(ISkillRepository skill)
        {
            _skill = skill;
        }

        [Route("/skills")]
        [HttpGet]
        public async Task<IActionResult> GetSkills()
        {
            return Ok(await _skill.Get());
        }
    }
}
