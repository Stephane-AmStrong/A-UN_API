using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class SubscriptionWriteDto
    {
        [Required]
        [Display(Name = "Souscrit le")]
        public DateTime SubscribedAt { get; set; }
        [Required]
        [Display(Name = "Créé le")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Mise à jour le")]
        public DateTime? UpdatedAt { get; set; }
        [Display(Name = "Validé le")]
        public DateTime? ValidatedAt { get; set; }
        [Required]
        public Guid AcademicYearId { get; set; }
        [Required]
        public string AppUserId { get; set; }
        [Required]
        public Guid FormationId { get; set; }
    }
}
