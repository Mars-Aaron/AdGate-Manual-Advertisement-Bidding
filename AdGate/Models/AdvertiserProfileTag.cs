using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class AdvertiserProfileTag
    {
        [Required]
        [Key]
        public int AdvertiserProfileId { get; set; }
        [Required]
        [Key]
        public int TagId { get; set; }
        public AdvertiserProfile AdvertiserProfile { get; set; }
        public Tag Tag { get; set; }
    }
}
