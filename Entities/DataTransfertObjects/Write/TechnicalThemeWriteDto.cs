using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class TechnicalThemeWriteDto
    {
        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
        public string IsBranch { get; set; }
    }
}
