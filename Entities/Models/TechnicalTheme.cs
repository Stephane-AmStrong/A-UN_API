using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class TechnicalTheme
    {
        public TechnicalTheme()
        {
            FieldLevels = new HashSet<BranchLevel>();
            Branches = new HashSet<Branch>();
        }

        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
        public string IsBranch { get; set; }

        public virtual ICollection<BranchLevel> FieldLevels { get; set; }
        public virtual ICollection<Branch> Branches { get; set; }
    }
}
