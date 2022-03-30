using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class UniversityReadDto : IComparable<UniversityReadDto>
    {
        public Guid Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
        public string ImgLink { get; set; }
        public string AppUserId { get; set; }
        [Display(Name = "Creation date")]
        public DateTime Birthday { get; set; }
        [Display(Name = "Created on")]
        public DateTime CreateAt { get; set; }
        public virtual AppUserReadDto AppUser { get; set; }

        [Display(Name = "Trainings")]
        public virtual FormationReadDto[] Formations { get; set; }

        public int CompareTo(UniversityReadDto universityReadDto)
        {
            /*
            if (this.Salary < other.Salary)
            {
                return 1;
            }
            else if (this.Salary > other.Salary)
            {
                return -1;
            }
            else
            {
                return 0;
            } 
             */


            if (this.Id == universityReadDto.Id)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public override bool Equals(object obj)
        {
            UniversityReadDto universityReadDto = obj as UniversityReadDto;
            return universityReadDto != null && universityReadDto.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode() ^ this.Id.GetHashCode();
        }
    }
}
