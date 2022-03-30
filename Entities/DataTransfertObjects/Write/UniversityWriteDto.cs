using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class UniversityWriteDto
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [JsonIgnore]
        public string ImgLink { get; set; }
        public string AppUserId { get; set; }
        [Display(Name = "Picture")]
        public IFormFile File { get; set; }
    }
}
