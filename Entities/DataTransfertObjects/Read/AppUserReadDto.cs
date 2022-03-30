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
        [Display(Name = "First Name")]
        public string Firstname { get; set; }
        [Display(Name = "Last Name")]
        public string Name { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        public string Role { get; set; }
        public WorkstationReadDto Workstation { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }

        public virtual PersonalFileReadDto[] PersonalFiles { get; set; }
        public virtual SubscriptionReadDto[] Subscriptions { get; set; }
        public virtual PaymentReadDto[] Payments { get; set; }
    }
}
