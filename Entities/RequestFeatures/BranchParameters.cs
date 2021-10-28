using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class BranchParameters : QueryStringParameters
    {
        public BranchParameters()
        {
            OrderBy = "name";
        }


        public Guid TechnicalThemeId { get; set; }
        public Guid UniversityId { get; set; }
    }
}
