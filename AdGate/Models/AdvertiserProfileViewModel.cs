using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class AdvertiserProfileViewModel
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
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        [ProfilePictureValidator]
        public IFormFile ProfilePicture { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Required]
        [Display(Name = "Company Tags")]
        public string[] SelectedTags { get; set; }
        public List<SelectListItem> Tags { get; set; }
    }

    public class CheckboxTag
    {
        public int TagId { get; set; }
        public string Tag { get; set; }
        public bool Selected { get; set; }
    }

    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _Extensions;
        public AllowedExtensionsAttribute(string[] Extensions)
        {
            _Extensions = Extensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            var extension = Path.GetExtension(file.FileName);
            if (!(file == null))
            {
                if (!_Extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This photo extension is not allowed!";
        }
    }

    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            //var extension = Path.GetExtension(file.FileName);
            //var allowedExtensions = new[] { ".jpg", ".png" };`enter code here`
            if (file != null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is { _maxFileSize} bytes.";
        }
    }
}
