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

        public Guid? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Link { get; set; }
        [JsonIgnore]
        public string AppUserId { get; set; }
    }
}