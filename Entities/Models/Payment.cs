using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Payment : IEntity
    {
        public Guid Id { get; set; }
        public Guid AcademicYearId { get; set; }
        [Required]
        public string AppUserId { get; set; }
        public float MoneyAmount { get; set; }
        public float RemainingAmount { get; set; }
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

        /*
         
            public string IFU { get; set; }
            public string FV_NIM { get; set; }
            public string FV_SIG { get; set; }
            public string Compteur { get; set; }
            public string FV_CompteurType { get; set; }
            public string FV_CompteurTotal { get; set; }
            public string FV_DateMECef { get; set; }
            public string FA_NIM { get; set; }
            public string FA_SIG { get; set; }
            public string FA_CompteurType { get; set; }
            public string FA_CompteurTotal { get; set; }
            public string FA_DateMECef { get; set; }

         */

        [ForeignKey("AcademicYearId")]
        public virtual AcademicYear AcademicYear { get; set; }


        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }
    }
}
