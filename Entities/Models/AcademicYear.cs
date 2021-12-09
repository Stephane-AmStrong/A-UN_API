using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class AcademicYear : IEntity
    {
        public Guid Id { get; set; }
        [Required]
        public DateTime StartsOn { get; set; }
        [Required]
        public DateTime EndsOn { get; set; }
        public float SubscriptionFee { get; set; }
        public bool IsOpen { get; set; }
    }
}
