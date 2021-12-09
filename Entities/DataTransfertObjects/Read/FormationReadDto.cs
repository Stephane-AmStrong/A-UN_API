using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class FormationReadDto
    {
        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        public string ImgLink2 { get; set; }
        public string Code { get; set; }
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [Display(Name = "Validé le")]
        public DateTime? ValiddatedAt { get; set; }
        public Guid UniversityId { get; set; }


        public virtual UniversityReadDto University { get; set; }

        public virtual FormationLevelReadDto[] FormationLevels { get; set; }
    }
}
