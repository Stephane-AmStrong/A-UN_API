using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class RegistrationFormParameters : QueryStringParameters
    {
        public RegistrationFormParameters()
        {
            OrderBy = "name";
        }

        public string Name { get; set; }
        public float Price { get; set; }
        public Guid FieldLevelId { get; set; }
    }
}
