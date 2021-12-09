using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class FormationLevel : IEntity
    {
        public FormationLevel()
        {
            Prerequisites = new HashSet<Prerequisite>();
            Subscriptions = new HashSet<Subscription>();
        }

        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        [Required]
        public string Code { get; set; }
        public long Price { get; set; }
        public DateTime? ValiddatedAt { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid FormationId { get; set; }


        [ForeignKey("FormationId")]
        public virtual Formation Formation { get; set; }


        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<Prerequisite> Prerequisites { get; set; }
    }
}
