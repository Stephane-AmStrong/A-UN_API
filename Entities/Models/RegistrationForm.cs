using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class RegistrationForm
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public float Price { get; set; }
        public Guid FieldLevelId { get; set; }


        [ForeignKey ("FieldLevelId")]
        public virtual BranchLevel FieldLevel { get; set; }

        public virtual ICollection<RegistrationFormLine> RegistrationFormRequirements { get; set; }
    }
}
