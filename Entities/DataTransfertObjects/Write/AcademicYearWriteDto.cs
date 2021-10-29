using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Entities.DataTransfertObjects
{
    public partial class AcademicYearWriteDto
    {
        
        [Required]
        public string Name { get; set; }
    }
}
