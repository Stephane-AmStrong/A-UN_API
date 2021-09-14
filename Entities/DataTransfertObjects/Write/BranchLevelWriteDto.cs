using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class BranchLevelWriteDto
    {
        public Guid BranchId { get; set; }
        public Guid TechnicalThemeId { get; set; }
        [Required]
        public string Name { get; set; }


        public virtual TechnicalThemeWriteDto TechnicalTheme { get; set; }

        public virtual RegistrationFormWriteDto[] RegistrationForms { get; set; }
    }
}
