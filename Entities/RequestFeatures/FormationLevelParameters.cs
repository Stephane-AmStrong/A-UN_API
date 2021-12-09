using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class FormationLevelParameters : QueryStringParameters
    {
        public FormationLevelParameters()
        {
            OrderBy = "name";
        }

       
        public Guid FromUniversityId { get; set; }

        public string ManagedByAppUserId { get; set; }
        public Guid OfFormationId { get; set; }
        
    }
}
