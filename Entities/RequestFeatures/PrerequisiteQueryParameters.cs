using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class PrerequisiteQueryParameters : QueryStringParameters
    {
        public PrerequisiteQueryParameters()
        {
            OrderBy = "name";
        }

        public Guid PrerequisiteId { get; set; }
        public Guid OfFormationId { get; set; }

        public string ManagedByAppUserId { get; set; }
        public bool IsProgram { get; set; }
    }
}
