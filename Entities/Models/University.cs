using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class University : IEntity
    {
        public University()
        {
            Formations = new HashSet<Formation>();
        }

        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string ImgLink { get; set; }
        [Required]
        public string AppUserId { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreateAt { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<Formation> Formations { get; set; }
    }
}
