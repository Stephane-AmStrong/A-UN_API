using Entities.DataTransfertObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class AppUserReadDto
    {
        public string Id { get; set; }
        public string ImgLink { get; set; }
        public string Firstname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }

        public virtual PersonalFileReadDto[] PersonalFiles { get; set; }
        public virtual SubscriptionReadDto[] Subscriptions { get; set; }
    }
}
