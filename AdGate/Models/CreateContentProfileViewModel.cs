using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdGate.Models
{
    public class CreateContentProfileViewModel
    {
        [Key]
        [Required]
        public int PublisherProfileId { get; set; }
        [Required]
        [Display(Name = "Content Name")]
        public string ContentName { get; set; }
        [Required]
        [Display(Name = "Content Medium")]
        [EnumDataType(typeof(ContentMedium))]
        public ContentMedium ContentMedium { get; set; }
        [Required]
        [Range(0, 999999999999)]
        [Display(Name = "Average Visits Per Month")]
        public int VisitsPerMonth { get; set; }
        [Required]
        [Display(Name = "Preferred Tags")]
        public string[] SelectedTags { get; set; }
        public List<SelectListItem> Tags { get; set; }
    }
}
