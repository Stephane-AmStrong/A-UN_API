using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PaymentReadDto
    {
        public Guid Id { get; set; }
        public Guid AcademicYearId { get; set; }
        [Required]
        public string AppUserId { get; set; }
        [Display(Name = "Montant Reçu")]
        public float MoneyAmount { get; set; }
        public float RemainingAmount { get; set; }
        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Payé le")]
        public DateTime? PaidAt { get; set; }

        public string Feda_Klass { get; set; }
        public string Feda_Id { get; set; }
        public string Feda_Amount { get; set; }
        public string Feda_Description { get; set; }
        public string Feda_CallbackUrl { get; set; }
        public string Feda_Status { get; set; }
        public string Feda_Customer_id { get; set; }
        public string Feda_Currency_id { get; set; }
        public string Feda_Mode { get; set; }

        public virtual AcademicYearReadDto AcademicYear { get; set; }
        public virtual AppUserReadDto AppUser { get; set; }
    }
}
