using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class University
    {
        public University()
        {
            Branches = new HashSet<Branch>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImgLink { get; set; }
        public long Price { get; set; }
        public string AppUserId { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreateAt { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<Branch> Branches { get; set; }
    }
}
