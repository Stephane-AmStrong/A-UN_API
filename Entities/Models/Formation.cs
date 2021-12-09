using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Formation : IEntity
    {
        public Formation()
        {
            FormationLevels = new HashSet<FormationLevel>();
        }
        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        public string ImgLink2 { get; set; }
        public DateTime? ValiddatedAt { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid UniversityId { get; set; }

        
        [ForeignKey ("UniversityId")]
        public virtual University University { get; set; }

        public virtual ICollection<FormationLevel> FormationLevels { get; set; }
    }
}
