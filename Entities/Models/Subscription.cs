using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Subscription
    {
        public Subscription()
        {
            SubscriptionLines = new HashSet<SubscriptionLine>();
        }


        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<SubscriptionLine> SubscriptionLines { get; set; }
    }
}
