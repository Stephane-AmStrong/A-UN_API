using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects
{
    public class FormationWriteDto
    {
        public string ImgLink { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Price")] 
        [Range(10.0, double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public long Price { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public Guid CategoryId { get; set; }

        [Required]
        [Display(Name = "University")]
        public Guid UniversityId { get; set; }

        public DateTime? ValidatedAt { get; set; }

        [Display(Name = "Picture")]
        public IFormFile File { get; set; }
    }
}
