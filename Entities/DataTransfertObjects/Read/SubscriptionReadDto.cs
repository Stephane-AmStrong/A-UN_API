using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class SubscriptionReadDto
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Inscrit le")]
        public DateTime SubscribedAt { get; set; }
        [Display(Name = "Crée le")]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Display(Name = "Validé le")]
        public DateTime? ValiddatedAt { get; set; }
        [Required]
        public Guid AcademicYearId { get; set; }
        [Required]
        public string AppUserId { get; set; }
        [Required]
        public Guid FormationLevelId { get; set; }


        public virtual AcademicYearReadDto AcademicYear { get; set; }

        public virtual AppUserReadDto AppUser { get; set; }

        public virtual FormationLevelReadDto FormationLevel { get; set; }

        public virtual SubscriptionLineReadDto[] SubscriptionLines { get; set; }
    }
}
