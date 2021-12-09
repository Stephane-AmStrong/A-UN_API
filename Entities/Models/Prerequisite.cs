using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Prerequisite : IEntity
    {
        public Prerequisite()
        {
            SubscriptionLines = new HashSet<SubscriptionLine>();
        }

        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int NumOrder { get; set; }
        public Guid FormationLevelId { get; set; }

        [ForeignKey("FormationLevelId")]
        public virtual FormationLevel FormationLevel { get; set; }
        public virtual ICollection<SubscriptionLine> SubscriptionLines { get; set; }
    }
}
