using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PrerequisiteReadDto
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "N°")]
        public int NumOrder { get; set; }
        [Display(Name = "Training")]
        public Guid FormationId { get; set; }

        public virtual FormationReadDto Formation { get; set; }
        public virtual SubscriptionLineReadDto[] SubscriptionLines { get; set; }
    }
}
