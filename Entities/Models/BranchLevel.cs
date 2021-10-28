using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class BranchLevel : IEntity
    {
        public BranchLevel()
        {
            RegistrationForms = new HashSet<RegistrationForm>();
        }

        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid BranchId { get; set; }


        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }


        public virtual ICollection<RegistrationForm> RegistrationForms { get; set; }
    }
}
