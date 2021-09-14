using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class FileReadDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Link { get; set; }

        public string AppUserId { get; set; }
        public virtual AppUserReadDto AppUser { get; set; }
    }
}
