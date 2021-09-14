﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class SubscriptionLine
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid FileId { get; set; }
        public Guid RegistrationFormLineId { get; set; }
        public Guid SubscriptionId { get; set; }


        [ForeignKey ("FileId")]
        public virtual File File { get; set; }
        
        [ForeignKey ("RegistrationFormLineId")]
        public virtual RegistrationFormLine RegistrationFormLine { get; set; }

        [ForeignKey ("SubscriptionId")]
        public virtual Subscription Subscription { get; set; }
    }
}