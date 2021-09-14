using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Branch
    {
        public Branch()
        {
            BranchLevels = new HashSet<BranchLevel>();
        }
        public Guid Id { get; set; }
        public Guid TechnicalThemeId { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid BranchLevelId { get; set; }


        [ForeignKey ("TechnicalThemeId")]
        public virtual TechnicalTheme TechnicalTheme { get; set; }

        public virtual ICollection<BranchLevel> BranchLevels { get; set; }
    }
}
