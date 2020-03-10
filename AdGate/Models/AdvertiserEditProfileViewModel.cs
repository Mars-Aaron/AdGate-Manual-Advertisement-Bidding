using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class AdvertiserEditProfileViewModel
    {
        [Required]
        public int ProfileId { get; set; }
        public string ProfilePicture { get; set; }
        [ProfilePictureValidator]
        public IFormFile NewProfilePicture { get; set; }
        [Required]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }
        [Required]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Required]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
            ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Confirm password doesn't match")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Company Tags")]
        public string[] SelectedTags { get; set; }
        public List<SelectListItem> Tags { get; set; }
    }
}
