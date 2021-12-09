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
    public class PersonalFileWriteDto
    {
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        public string Link { get; set; }
        [JsonIgnore]
        public string AppUserId { get; set; }

        [Display(Name = "Document")]
        [Required]
        public IFormFile File { get; set; }
    }
}