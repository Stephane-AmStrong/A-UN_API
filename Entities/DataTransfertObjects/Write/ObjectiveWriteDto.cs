﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class ObjectiveWriteDto
    {

        
        [Required]
        public string Name { get; set; }
    }
}
