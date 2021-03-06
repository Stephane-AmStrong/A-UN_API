using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class SubscriptionLineQueryParameters : QueryStringParameters
    {
        public SubscriptionLineQueryParameters()
        {
            OrderBy = "name";
        }


        public Guid PersonalFileId { get; set; }
        public Guid FormationLevelFileId { get; set; }
        public Guid SubscriptionId { get; set; }
    }
}
