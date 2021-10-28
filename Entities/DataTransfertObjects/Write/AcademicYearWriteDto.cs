using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Entities.DataTransfertObjects
{
    public partial class AcademicYearWriteDto
    {
        public Guid? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
