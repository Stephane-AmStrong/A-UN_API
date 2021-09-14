using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class BranchWriteDto
    {
        public Guid TechnicalThemeId { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid BranchLevelId { get; set; }
    }
}
