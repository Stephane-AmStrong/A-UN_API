using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Entities.DataTransfertObjects
{
    public partial class AcademicYearWriteDto
    {
        [Required]
        [Display(Name = "Commence le")]
        //[BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)] 
        //[BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [BindProperty, DataType(DataType.Date)]
        public DateTime StartsOn { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Se termine")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime EndsOn { get; set; } = DateTime.Now;

        [Range(10.0, Double.MaxValue, ErrorMessage = "Le champ {0} doit être supérieur à {1}.")]
        [Display(Name = "Frais de souscription")]
        public float SubscriptionFee { get; set; }
        [Display(Name = "Est ouvert")]
        public bool IsOpen { get; set; }
    }
}
