using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class AboutWriteDto
    {
        [Display(Name = "Bannière")]
        public IFormFile File { get; set; }
        [Required]
        public string ImgLink { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
