using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class RegistrationFormLineParameters : QueryStringParameters
    {
        public RegistrationFormLineParameters()
        {
            OrderBy = "name";
        }

        public Guid RegistrationFormId { get; set; }
        public int NumOrder { get; set; }

        public string Name { get; set; }
        public bool IsProgram { get; set; }
    }
}
