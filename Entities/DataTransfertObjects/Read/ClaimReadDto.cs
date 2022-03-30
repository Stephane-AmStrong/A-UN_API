using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Entities.DataTransfertObjects
{
    public class ClaimReadDto
    {
        public bool IsSelected { get; set; }

        public string Type { get; set; }
        public string Issuer { get; set; }
        public string ValueType { get; set; }
        public string Value { get; set; }
    }
}
