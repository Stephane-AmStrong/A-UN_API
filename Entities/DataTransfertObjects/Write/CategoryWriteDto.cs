using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class CategoryWriteDto
    {
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
    }
}
