using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAPIWithRepositoryPattern.ViewModels
{
    public class ExternalLoginViewModel
    {
        public string Provider { get; set; }
        public string IdToken { get; set; }
    }
}
