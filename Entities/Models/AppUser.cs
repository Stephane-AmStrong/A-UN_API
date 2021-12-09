using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Payments = new HashSet<Payment>();
            PersonalFiles = new HashSet<PersonalFile>();
            Subscriptions = new HashSet<Subscription>();
        }

        public string ImgLink { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<PersonalFile> PersonalFiles { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
