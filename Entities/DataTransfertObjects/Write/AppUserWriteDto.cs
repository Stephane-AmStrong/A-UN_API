using Entities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class AppUserWriteDto
    {
        [Required]
        [Display(Name = "Prénoms")]
        public string Firstname { get; set; }
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [Display(Name = "Sexe")]
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        [Display(Name = "Rôle")]
        public string WorkstationName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
