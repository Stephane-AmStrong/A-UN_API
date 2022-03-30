﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Category : IEntity
    {
        public Category()
        {
            Formations = new HashSet<Formation>();
        }

        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgLink { get; set; }

        public virtual ICollection<Formation> Formations { get; set; }
    }
}
