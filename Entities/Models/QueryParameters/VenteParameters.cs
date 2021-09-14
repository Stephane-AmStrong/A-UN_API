using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Models.QueryParameters
{
    public class VenteParameters : QueryStringParameters
    {
        //public uint MinYearOfBirth { get; set; }
        //public uint MaxYearOfBirth { get; set; } = (uint)DateTime.Now.Year;
        //public bool ValidYearRange => MaxYearOfBirth > MinYearOfBirth;

        public VenteParameters()
        {
            OrderBy = "DateVent";
        }

        [JsonIgnore]
        public Guid? ClientsId { get; set; }
        public Guid? IdUserEnr { get; set; }

        public DateTime? debutPeriode { get; set; }
        public DateTime? finPeriode { get; set; }
        public string Name { get; set; }
    }
}
