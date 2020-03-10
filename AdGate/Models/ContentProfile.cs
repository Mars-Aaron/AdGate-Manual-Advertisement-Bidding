using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class ContentProfile
    {
        public int ContentProfileId { get; set; }
        [Required]
        [Display(Name = "Content Name")]
        public string ContentName { get; set; }
        [Required]
        [Display(Name = "Content Medium")]
        [EnumDataType(typeof(ContentMedium))]
        public ContentMedium ContentMedium { get; set; }
        [Required]
        [MinLength(0)]
        [Display(Name = "Visits Per Month")]
        public int VisitsPerMonth { get; set; }
        [Required]
        [Display(Name = "Verified")]
        public bool IsVerified { get; set; }
        [Required]
        public int PublisherProfileId { get; set; }
        public PublisherProfile Owner { get; set; }
        [Display(Name = "Ad Space Listings")]
        public ICollection<AdSpaceListing> AdSpaceListings { get; set; }
        [Display(Name = "Tags")]
        public ICollection<ContentProfileTag> ContentProfileTags { get; set; }
    }

    public enum ContentMedium
    {
        Blog = 1,
        Website = 2,
        Video = 3
    }
}
