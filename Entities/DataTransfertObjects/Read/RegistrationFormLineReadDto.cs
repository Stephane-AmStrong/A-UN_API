using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class RegistrationFormLineReadDto
    {
        public Guid Id { get; set; }
        public Guid RegistrationFormId { get; set; }
        public int NumOrder { get; set; }

        public string Name { get; set; }
        public bool IsProgram { get; set; }
        public virtual RegistrationFormReadDto RegistrationForm { get; set; }
        public virtual SubscriptionLineReadDto[] SubscriptionLines { get; set; }

    }
}
