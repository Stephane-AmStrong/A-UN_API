using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class SubscriptionLineWriteDto
    {

        [Required]
        public string Name { get; set; }
        public Guid FileId { get; set; }
        public Guid RegistrationFormLineId { get; set; }
        public Guid SubscriptionId { get; set; }
    }
}
