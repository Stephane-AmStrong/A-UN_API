﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Objective
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}