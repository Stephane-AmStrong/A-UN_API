using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class SubscriptionLineReadDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Nom")]
        public string Name { get; set; }
        public Guid PersonalFileId { get; set; }
        public Guid PrerequisiteId { get; set; }
        public Guid SubscriptionId { get; set; }


        public virtual PersonalFileReadDto PersonalFile { get; set; }
        public virtual PrerequisiteWriteDto Prerequisite { get; set; }
        public virtual SubscriptionReadDto Subscription { get; set; }
    }
}
