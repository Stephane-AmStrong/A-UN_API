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
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "First Name")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Last Name")]
        public string Name { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Role")]
        public string WorkstationName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "This field is required")]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
