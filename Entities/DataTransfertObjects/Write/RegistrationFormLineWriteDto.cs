using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class RegistrationFormLineWriteDto
    {
        public Guid RegistrationFormId { get; set; }
        public int NumOrder { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsProgram { get; set; }
    }
}
