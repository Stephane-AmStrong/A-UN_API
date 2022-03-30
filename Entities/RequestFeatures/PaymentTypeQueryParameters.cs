﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class PaymentTypeQueryParameters : QueryStringParameters
    {
        public PaymentTypeQueryParameters()
        {
            OrderBy = "name";
        }


    }
}
