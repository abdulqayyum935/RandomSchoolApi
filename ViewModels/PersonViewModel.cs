using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Models;

namespace CrudAPIWithRepositoryPattern.ViewModels
{
    public class PersonViewModel:Person
    {
        public int[] ChoosenSkills { get; set; }
    }
}
