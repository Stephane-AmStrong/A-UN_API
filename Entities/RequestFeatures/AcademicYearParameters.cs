using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class AcademicYearParameters : QueryStringParameters
    {
        public AcademicYearParameters()
        {
            OrderBy = "startson";
        }

        public new DateTime? SearchTerm { get; set; }
        public bool DisplaysTheOpenOneOnly { get; set; }
    }
}
