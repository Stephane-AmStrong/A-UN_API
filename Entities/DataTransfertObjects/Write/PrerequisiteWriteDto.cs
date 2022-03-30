using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PrerequisiteWriteDto
    {
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [Display(Name = "N°")]
        public int NumOrder { get; set; }
        public Guid FormationId { get; set; }
    }
}
