using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class AcademicYearParameters : QueryStringParameters
    {
        public AcademicYearParameters()
        {
            OrderBy = "name";
        }

        public string Name { get; set; }
    }
}
