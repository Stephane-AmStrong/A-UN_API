
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class WorkstationReadDto
    {
        public string Id { get; set; }
        [Display(Name = "Nom")]
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public IList<ClaimReadDto> Claims { get; set; }
        public virtual AppUserReadDto[] AppUsers { get; set; }

    }
}
