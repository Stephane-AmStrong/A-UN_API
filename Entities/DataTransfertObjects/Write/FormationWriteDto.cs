using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class FormationWriteDto
    {
        public string ImgLink { get; set; }
        public string ImgLink2 { get; set; }
        public string Code { get; set; }
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [Required]
        public Guid UniversityId { get; set; }

        [Display(Name = "Photo")]
        public IFormFile File { get; set; }
        public IFormFile File2 { get; set; }

        //public virtual FormationLevelWriteDto[] FormationLevels { get; set; }
    }
}
