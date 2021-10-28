using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Branch : IEntity
    {
        public Branch()
        {
            BranchLevels = new HashSet<BranchLevel>();
        }
        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid UniversityId { get; set; }

        
        [ForeignKey ("UniversityId")]
        public virtual University University { get; set; }

        public virtual ICollection<BranchLevel> BranchLevels { get; set; }
    }
}
