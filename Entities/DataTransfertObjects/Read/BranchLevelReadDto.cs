using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class BranchLevelReadDto
    {
        public Guid Id { get; set; }
        public Guid BranchId { get; set; }
        public Guid TechnicalThemeId { get; set; }

        public string Name { get; set; }
        public string ImgLink { get; set; }


        public virtual BranchReadDto Branch { get; set; }


        public virtual TechnicalThemeReadDto TechnicalTheme { get; set; }

        public virtual RegistrationFormReadDto[] RegistrationForms { get; set; }
    }
}
