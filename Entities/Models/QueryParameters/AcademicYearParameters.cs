using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Models.QueryParameters
{
    public class AcademicYearParameters : QueryStringParameters
    {
        public AcademicYearParameters()
        {
            OrderBy = "name";
        }
    }

}
