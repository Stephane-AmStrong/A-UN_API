using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class TechnicalThemeReadDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string IsBranch { get; set; }

        public virtual BranchLevelReadDto[] FieldLevels { get; set; }
        public virtual BranchReadDto[] Branches { get; set; }
    }
}
