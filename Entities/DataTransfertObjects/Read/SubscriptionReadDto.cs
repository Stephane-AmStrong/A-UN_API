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
        [Display(Name = "Registered on")]
        public DateTime SubscribedAt { get; set; }
        [Required]
        [Display(Name = "Created on")]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Display(Name = "Validated on")]
        public DateTime? ValidatedAt { get; set; }
        [Required]
        [Display(Name = "Academic Year")]
        public Guid AcademicYearId { get; set; }
        [Required]
        public string AppUserId { get; set; }
        [Required]
        [Display(Name = "Training")]
        public Guid FormationId { get; set; }


        [Display(Name = "Academic Year")]
        public virtual AcademicYearReadDto AcademicYear { get; set; }

        public virtual AppUserReadDto AppUser { get; set; }

        [Display(Name = "Training")]
        public virtual FormationReadDto Formation { get; set; }

        public virtual SubscriptionLineReadDto[] SubscriptionLines { get; set; }
    }
}
