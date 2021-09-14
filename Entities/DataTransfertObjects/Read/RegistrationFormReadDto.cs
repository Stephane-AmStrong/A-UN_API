using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class RegistrationFormReadDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public float Price { get; set; }
        public Guid FieldLevelId { get; set; }


        public virtual BranchLevelReadDto FieldLevel { get; set; }

        public virtual RegistrationFormLineWriteDto[] RegistrationFormRequirements { get; set; }
    }
}
