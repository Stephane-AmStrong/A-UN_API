using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class UniversityWriteDto
    {
        public string Name { get; set; }
        public long Price { get; set; }
        public string AppUserId { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
