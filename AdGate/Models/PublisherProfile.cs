using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class PublisherProfile: Profile
    {
        public ICollection<ContentProfile> ContentProfiles { get; set; }
    }
}
