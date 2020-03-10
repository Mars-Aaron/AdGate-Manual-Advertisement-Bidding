using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class ContentProfileTag
    {
        [Required]
        [Key]
        public int ContentProfileId { get; set; }
        [Required]
        [Key]
        public int TagId { get; set; }
        public ContentProfile ContentProfile { get; set; }
        public Tag Tag { get; set; }
    }
}
