using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PartnerReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImgLink { get; set; }
    }
}
