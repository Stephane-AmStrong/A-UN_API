using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Subscription : IEntity
    {
        public Subscription()
        {
            SubscriptionLines = new HashSet<SubscriptionLine>();
        }


        public Guid Id { get; set; }
        [Required]
        public DateTime SubscribedAt  { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }       
        public DateTime? ValidatedAt { get; set; }
        [Required]
        public Guid AcademicYearId { get; set; }          
        [Required]
        public string AppUserId { get; set; }        
        [Required]
        public Guid FormationId { get; set; }


        [ForeignKey("AcademicYearId")]
        public virtual AcademicYear AcademicYear { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }

        [ForeignKey("FormationId")]
        public virtual Formation Formation { get; set; }

        public virtual ICollection<SubscriptionLine> SubscriptionLines { get; set; }
    }
}
