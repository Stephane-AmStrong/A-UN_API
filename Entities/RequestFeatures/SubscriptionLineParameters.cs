using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class SubscriptionLineParameters : QueryStringParameters
    {
        public SubscriptionLineParameters()
        {
            OrderBy = "name";
        }

        public string Name { get; set; }
        public Guid PersonalFileId { get; set; }
        public Guid RegistrationFormLineId { get; set; }
        public Guid SubscriptionId { get; set; }
    }
}
