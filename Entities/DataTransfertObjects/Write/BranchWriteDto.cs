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
    public class BranchWriteDto
    {
        
        public string ImgLink { get; set; }
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid UniversityId { get; set; }

        public IFormFile file { get; set; }

        public virtual UniversityWriteDto University { get; set; }

        public virtual BranchLevelWriteDto[] BranchLevels { get; set; }
    }
}
