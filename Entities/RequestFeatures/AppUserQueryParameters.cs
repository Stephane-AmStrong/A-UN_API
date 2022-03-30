using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class AppUserQueryParameters : QueryStringParameters
    {
        public AppUserQueryParameters()
        {
            OrderBy = "name";
        }


        public string ManagedByAppUserId { get; set; }
        public string WithRoleName { get; set; }
        public Guid FromUniversityId { get; set; }
        public Guid OfFormationId { get; set; }
        public string Gender { get; set; }
        public bool DisplayStudentOnly { get; set; }
        public string WhitchPaidFrom { get; set; }
        public string WhitchPaidTo { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}