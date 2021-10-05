using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class UniversityParameters : QueryStringParameters
    {
        public UniversityParameters()
        {
            OrderBy = "name";
        }

        public string Name { get; set; }
        public string AppUserId { get; set; }
        public DateTime? MinBirthday { get; set; }
        public DateTime? MaxBirthday { get; set; }
        //[JsonIgnore]
        //public bool ValidBirthdayRange => MaxBirthday > MinBirthday;

        public DateTime? MinCreateAt { get; set; }
        public DateTime? MaxCreateAt { get; set; }
        //[JsonIgnore]
        //public bool ValidCreateAtRange => MaxCreateAt > MinCreateAt;

    }
}
