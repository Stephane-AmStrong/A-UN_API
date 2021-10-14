using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class BranchLevelParameters : QueryStringParameters
    {
        public BranchLevelParameters()
        {
            OrderBy = "name";
        }


        public Guid BranchId { get; set; }
        public Guid TechnicalThemeId { get; set; }
    }
}
