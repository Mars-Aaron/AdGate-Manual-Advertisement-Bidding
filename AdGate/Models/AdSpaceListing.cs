using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdGate.Models
{
    public class AdSpaceListing
    {
        public int AdSpaceListingId { get; set; }
        [Required]
        [EnumDataType(typeof(AdBanner))]
        public AdBanner AdBanner { get; set; }
        [Required]
        public decimal CostPerClick { get; set; }
        [Required]
        public decimal CostPerView { get; set; }
        [Required]
        public decimal CostPerThousand { get; set; }
        [Required]
        public DateTime ListingExpiry { get; set; }
        [Required]
        [EnumDataType(typeof(AdListingStatus))]
        public AdListingStatus Status { get; set; }
        public int ContentProfileId { get; set; }
        public ContentProfile ContentProfile { get; set; }
        public ICollection<AdSpacePurchase> AdSpacePurchases { get; set; }
    }

    public enum AdListingStatus
    {
        Purchased, Expired
    }


    public enum AdBanner
    {
        Billboard, HalfPage, HalfBanner, LargeLeaderboard, LargeMobile, LargeRect,
        Leaderbaord, MainBanner, MediumRect, Mobile, Portrait, Skyscraper, SmallRect, 
        SmallSquare, Square, VerticalBanner, WideSkycraper
    }

    public static class Extensions
    {
        public static int Width(this AdBanner adBanner)
        {
            switch (adBanner)
            {
                case AdBanner.Billboard: return 970;
                case AdBanner.HalfBanner: return 234;
                case AdBanner.HalfPage: return 300;
                case AdBanner.LargeLeaderboard: return 970;
                case AdBanner.LargeMobile: return 320;
                case AdBanner.LargeRect: return 336;
                case AdBanner.Leaderbaord: return 728;
                case AdBanner.MainBanner: return 468;
                case AdBanner.MediumRect: return 300;
                case AdBanner.Mobile: return 320;
                case AdBanner.Portrait: return 300;
                case AdBanner.Skyscraper: return 120;
                case AdBanner.SmallRect: return 180;
                case AdBanner.SmallSquare: return 200;
                case AdBanner.Square: return 250;
                case AdBanner.VerticalBanner: return 120;
                case AdBanner.WideSkycraper: return 160;
                default: return 0;
            }
        }

        public static int Height(this AdBanner adBanner)
        {
            switch (adBanner)
            {
                case AdBanner.Billboard: return 250;
                case AdBanner.HalfBanner: return 60;
                case AdBanner.HalfPage: return 600;
                case AdBanner.LargeLeaderboard: return 90;
                case AdBanner.LargeMobile: return 100;
                case AdBanner.LargeRect: return 280;
                case AdBanner.Leaderbaord: return 90;
                case AdBanner.MainBanner: return 60;
                case AdBanner.MediumRect: return 250;
                case AdBanner.Mobile: return 50;
                case AdBanner.Portrait: return 1050;
                case AdBanner.Skyscraper: return 600;
                case AdBanner.SmallRect: return 150;
                case AdBanner.SmallSquare: return 200;
                case AdBanner.Square: return 250;
                case AdBanner.VerticalBanner: return 240;
                case AdBanner.WideSkycraper: return 600;
                default: return 0;
            }
        }

        public static string Name(this AdBanner adBanner)
        {
            switch (adBanner)
            {
                case AdBanner.Billboard: return "Billboard";
                case AdBanner.HalfBanner: return "Half Banner";
                case AdBanner.HalfPage: return "Half Page";
                case AdBanner.LargeLeaderboard: return "Large Leaderboard";
                case AdBanner.LargeMobile: return "Large Mobile";
                case AdBanner.LargeRect: return "Large Rectangle";
                case AdBanner.Leaderbaord: return "Leaderboard";
                case AdBanner.MainBanner: return "Main Banner";
                case AdBanner.MediumRect: return "Medium Rectangle";
                case AdBanner.Mobile: return "Mobile";
                case AdBanner.Portrait: return "Portrait";
                case AdBanner.Skyscraper: return "Skyscraper";
                case AdBanner.SmallRect: return "Small Rectangle";
                case AdBanner.SmallSquare: return "Small Square";
                case AdBanner.Square: return "Square";
                case AdBanner.VerticalBanner: return "Vertical Banner";
                case AdBanner.WideSkycraper: return "Wide Skyscraper";
                default: return "";
            }
        }
    }
}
