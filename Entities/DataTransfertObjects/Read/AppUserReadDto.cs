using Entities.DataTransfertObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class AppUserReadDto
    {
        public string Id { get; set; }
        public string ImgLink { get; set; }
        [Display(Name = "Prénoms")]
        public string Firstname { get; set; }
        [Display(Name = "Nom")]
        public string Name { get; set; }
        public string Email { get; set; }
        [Display(Name = "Sexe")]
        public string Gender { get; set; }
        public WorkstationReadDto Workstation { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }

        public virtual PersonalFileReadDto[] PersonalFiles { get; set; }
        public virtual SubscriptionReadDto[] Subscriptions { get; set; }
    }
}
