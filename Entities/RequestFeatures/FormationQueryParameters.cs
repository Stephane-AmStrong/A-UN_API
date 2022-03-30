using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class FormationQueryParameters : QueryStringParameters
    {
        public FormationQueryParameters()
        {
            OrderBy = "name";
        }

        public string ManagedByAppUserId { get; set; }
        public Guid FromUniversityId { get; set; }
        public bool ValidatedOnly { get; set; }
    }
}
