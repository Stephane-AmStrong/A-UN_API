using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class RegistrationFormLine
    {
        public RegistrationFormLine()
        {
            SubscriptionLines = new HashSet<SubscriptionLine>();
        }

        public Guid Id { get; set; }
        public Guid RegistrationFormId { get; set; }
        public int NumOrder { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsProgram { get; set; }


        [ForeignKey("RegistrationFormId")]
        public virtual RegistrationForm RegistrationForm { get; set; }
        public virtual ICollection<SubscriptionLine> SubscriptionLines { get; set; }

    }
}
