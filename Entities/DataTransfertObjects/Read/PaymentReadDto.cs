using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PaymentReadDto
    {
        public Guid Id { get; set; }
        public Guid PaymentTypeId { get; set; }

        public Guid SubscriptionId { get; set; }
        public string Name { get; set; }
        public float MoneyAmount { get; set; }
        public float RemainingAmount { get; set; }
        public DateTime? PaidAt { get; set; }

        public string IFU { get; set; }
        public string FV_NIM { get; set; }
        public string FV_SIG { get; set; }
        public string Compteur { get; set; }
        public string FV_CompteurType { get; set; }
        public string FV_CompteurTotal { get; set; }
        public string FV_DateMECef { get; set; }
        public string FA_NIM { get; set; }
        public string FA_SIG { get; set; }
        public string FA_CompteurType { get; set; }
        public string FA_CompteurTotal { get; set; }
        public string FA_DateMECef { get; set; }


        public virtual PaymentTypeReadDto PaymentType { get; set; }

        public virtual SubscriptionReadDto Subscription { get; set; }
    }
}
