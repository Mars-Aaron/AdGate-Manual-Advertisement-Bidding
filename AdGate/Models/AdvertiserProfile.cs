using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class AdvertiserProfile: Profile
    {
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Tags")]
        public ICollection<AdvertiserProfileTag> AdvertiserProfileTags { get; set; }
        public ICollection<AdSpacePurchase> AdSpacePurchases { get; set; }
    }
}
