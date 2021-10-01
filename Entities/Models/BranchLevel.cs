using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class BranchLevel
    {
        public BranchLevel()
        {
            RegistrationForms = new HashSet<RegistrationForm>();
        }

        public Guid Id { get; set; }
        public Guid BranchId { get; set; }
        public Guid TechnicalThemeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string ImgLink { get; set; }


        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }


        [ForeignKey("TechnicalThemeId")]
        public virtual TechnicalTheme TechnicalTheme { get; set; }

        public virtual ICollection<RegistrationForm> RegistrationForms { get; set; }
    }
}
