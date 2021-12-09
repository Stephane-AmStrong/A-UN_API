using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Entities.DataTransfertObjects
{
    public class ClaimWriteDto
    {
        public bool IsSelected { get; set; }

        public string Type { get; }
    }
}
