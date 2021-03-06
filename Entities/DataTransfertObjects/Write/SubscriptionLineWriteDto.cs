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
        [Display(Name = "Nom")]
        public string Name { get; set; }
        public Guid PersonalFileId { get; set; }
        public Guid PrerequisiteId { get; set; }
        public Guid SubscriptionId { get; set; }
    }
}
