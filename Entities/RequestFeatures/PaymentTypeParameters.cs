using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class PaymentTypeParameters : QueryStringParameters
    {
        public PaymentTypeParameters()
        {
            OrderBy = "name";
        }

        public string Name { get; set; }
    }
}
