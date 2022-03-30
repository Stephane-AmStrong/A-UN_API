using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class CategoryQueryParameters : QueryStringParameters
    {
        public CategoryQueryParameters()
        {
            OrderBy = "name";
        }

    }
}
