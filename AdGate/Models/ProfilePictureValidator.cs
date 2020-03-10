using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class ProfilePictureValidator: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            } else
            {
                IFormFile profilePicture = value as IFormFile;
                using (var memoryStream = new MemoryStream())
                {
                    profilePicture.CopyTo(memoryStream);
                    using (var img = Image.FromStream(memoryStream))
                    {
                        if (img.Width > 300 || img.Height > 300)
                        {
                            return new ValidationResult("Profile picture cannot exceed size of 300x300");
                        }
                        else if (img.Width < 150 || img.Height < 150)
                        {
                            return new ValidationResult("Profile picture cannot be less than 150x150 in size");
                        }
                        else if (img.Width != img.Height)
                        {
                            return new ValidationResult("Profile picture must be a square, e.g: 100x100");
                        } else
                        {
                            return ValidationResult.Success;
                        }
                    }
                }
            }
        }
    }
}
