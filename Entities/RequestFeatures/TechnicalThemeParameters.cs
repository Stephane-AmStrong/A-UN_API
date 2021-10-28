using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class TechnicalThemeParameters : BranchParameters
    {
        public TechnicalThemeParameters()
        {
            OrderBy = "name";
        }


        public string Code { get; set; }
        public string IsBranch { get; set; }
    }
}
