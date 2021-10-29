using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class UniversityWriteDto
    {
        public string Name { get; set; }
        [JsonIgnore]
        public string AppUserId { get; set; }
        public DateTime Birthday { get; set; }
        public IFormFile file { get; set; }
    }
}
