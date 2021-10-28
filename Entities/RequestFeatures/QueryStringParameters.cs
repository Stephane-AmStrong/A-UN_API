﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public abstract class QueryStringParameters
    {
        const int maxPageSize = 250000;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 1000;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public string OrderBy { get; set; }
        public string Fields { get; set; }
        public string SearchTerm { get; set; }
    }
}
