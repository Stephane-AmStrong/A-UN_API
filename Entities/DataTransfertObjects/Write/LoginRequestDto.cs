using Entities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class LoginRequestDto
    {
        [EmailAddress]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "This field is required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; } = false;
    }
}
