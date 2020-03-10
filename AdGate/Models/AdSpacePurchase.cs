using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class AdSpacePurchase
    {
        [Key]
        [Required]
        public int AdvertiserProfileId { get; set; }
        [Key]
        [Required]
        public int AdSpaceListingId { get; set; }
        [Required]
        [EnumDataType(typeof(AdSpacePaymentModel))]
        public AdSpacePaymentModel PaymentModel { get; set; }

        public AdvertiserProfile AdvertiserProfile { get; set; }
        public AdSpaceListing AdSpaceListing { get; set; }
    }

    public enum AdSpacePaymentModel
    {
        CPM, CPC, CPV, CPM_CPC, CPM_CPV, CPC_CPV, CPM_CPC_CPV
    }
}
