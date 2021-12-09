using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class BannerReadDto
    {
        public Guid Id { get; set; }
        [Display(Name = "Bannière")]
        public string ImgLink { get; set; }
    }
}
