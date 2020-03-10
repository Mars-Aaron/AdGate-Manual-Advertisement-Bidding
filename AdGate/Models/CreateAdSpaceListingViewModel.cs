using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class CreateAdSpaceListingViewModel
    {
        [Required(ErrorMessage = "Ad banner should be selected")]
        [EnumDataType(typeof(AdBanner))]
        public AdBanner AdBanner { get; set; }
        [Required]
        public int ContentProfileId { get; set; }
        public ContentProfile ContentProfile { get; set; }
    }
}
