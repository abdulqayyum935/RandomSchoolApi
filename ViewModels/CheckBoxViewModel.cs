using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAPIWithRepositoryPattern.ViewModels
{
    public class CheckBoxViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsChecked { get; set; }
    }
}
