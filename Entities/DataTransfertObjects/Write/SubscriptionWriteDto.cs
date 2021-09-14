﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class SubscriptionWriteDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string AppUserId { get; set; }

        //public virtual SubscriptionLineWriteDto[] SubscriptionLines { get; set; }
    }
}