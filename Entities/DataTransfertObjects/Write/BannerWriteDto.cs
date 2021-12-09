using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class BannerWriteDto
    {
        [Required]
        [Display(Name = "Bannière")]
        public IFormFile File { get; set; }
    }
}
