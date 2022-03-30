using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Formation : IEntity
    {
        public Formation()
        {
            Prerequisites = new HashSet<Prerequisite>();
            Subscriptions = new HashSet<Subscription>();
        }

        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        [Required]
        public string Name { get; set; }
        public long Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid UniversityId { get; set; }
        public DateTime? ValidatedAt { get; set; }


        [ForeignKey ("CategoryId")]
        public virtual Category Category { get; set; }
        
        [ForeignKey ("UniversityId")]
        public virtual University University { get; set; }

        public virtual ICollection<Prerequisite> Prerequisites { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
