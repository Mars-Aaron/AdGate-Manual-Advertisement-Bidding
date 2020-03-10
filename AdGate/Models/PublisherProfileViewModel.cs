using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class PublisherProfileViewModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }
        [Required]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        [ProfilePictureValidator]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile ProfilePicture { get; set; }
    }
}
