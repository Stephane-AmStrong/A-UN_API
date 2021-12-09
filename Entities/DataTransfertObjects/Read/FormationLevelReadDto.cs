using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class FormationLevelReadDto
    {
        public Guid? Id { get; set; }
        public string ImgLink { get; set; }
        public string Code { get; set; }
        [Display(Name = "Nom")]
        public string Name { get; set; }
        public DateTime? ValiddatedAt { get; set; }
        [Display(Name = "Prix")]
        public long Price { get; set; }
        public Guid FormationId { get; set; }

        public virtual FormationReadDto Formation { get; set; }

        public virtual PrerequisiteReadDto[] Prerequisites { get; set; }
        public virtual SubscriptionReadDto[] Subscriptions { get; set; }
    }
}
