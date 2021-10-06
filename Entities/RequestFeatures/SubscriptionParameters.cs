﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class SubscriptionParameters : QueryStringParameters
    {
        public SubscriptionParameters()
        {
            OrderBy = "name";
        }

        public string Name { get; set; }

        public string AppUserId { get; set; }
    }
}