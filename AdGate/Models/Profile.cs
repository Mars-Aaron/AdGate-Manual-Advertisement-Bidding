using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public string ProfilePicture { get; set; }
        [Required]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }
        [Required]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }
        [Display(Name = "Full Name")]
        public string FullName {
            get { return GivenName + " " + FamilyName; }
        }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
