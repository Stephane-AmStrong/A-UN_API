using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class FormationReadDto
    {
        public Guid Id { get; set; }
        [Display(Name = "Picture")]
        public string ImgLink { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Price")]
        public long Price { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid UniversityId { get; set; }
        [Display(Name = "Validated on")]
        public DateTime? ValidatedAt { get; set; }


        public virtual CategoryReadDto Category { get; set; }
        public virtual UniversityReadDto University { get; set; }

        public virtual PrerequisiteReadDto[] Prerequisites { get; set; }
        public virtual SubscriptionReadDto[] Subscriptions { get; set; }
    }
}
