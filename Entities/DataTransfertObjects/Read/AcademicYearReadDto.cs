using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Entities.DataTransfertObjects
{
    public partial class AcademicYearReadDto
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Commence le")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime StartsOn { get; set; }

        [Required]
        [Display(Name = "Se termine")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime EndsOn { get; set; }
        [Display(Name = "Frais de souscription")]

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public float SubscriptionFee { get; set; }
        [Display(Name = "Est ouvert")]
        public bool IsOpen { get; set; }
    }
}
