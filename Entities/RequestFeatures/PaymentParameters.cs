using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class PaymentParameters : QueryStringParameters
    {

        public PaymentParameters()
        {
            OrderBy = "name";
        }
        
        public Guid PaymentTypeId { get; set; }

        public Guid SubscriptionId { get; set; }
        public string Name { get; set; }
        public float MoneyAmount { get; set; }
        public float RemainingAmount { get; set; }
        public DateTime? PaidAt { get; set; }

        public string IFU { get; set; }
    }
}
