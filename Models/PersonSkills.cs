using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAPIWithRepositoryPattern.Models
{
    public class PersonSkills
    {
        [Key]
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int SkillId { get; set; }
        public Skill Skill { get; set; }
    }
}
