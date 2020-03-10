using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        [Required]
        [Display(Name = "Type Name")]
        public string TypeName { get; set; }
        [Display(Name = "Type Description")]
        public string TypeDescription { get; set; }
        [Display(Name = "Content Profiles")]
        public ICollection<ContentProfileTag> ContentProfileTags { get; set; }
        [Display(Name = "Advertiser Profiles")]
        public ICollection<AdvertiserProfileTag> AdvertiserProfileTags { get; set; }
    }
}
