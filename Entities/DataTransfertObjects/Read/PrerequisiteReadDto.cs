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
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [Display(Name = "N°")]
        public int NumOrder { get; set; }
        public Guid FormationLevelId { get; set; }

        public virtual FormationLevelReadDto FormationLevel { get; set; }
        public virtual SubscriptionLineReadDto[] SubscriptionLines { get; set; }
    }
}
