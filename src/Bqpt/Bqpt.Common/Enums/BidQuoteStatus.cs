using System.ComponentModel.DataAnnotations;

namespace Bqpt.Common
{
    public enum BidQuoteStatus
    {
        [Display(Name = "System Bids (New)", Description = "System Bid Imported from AssetWorks", Order = 1)]
        New = 1,

        [Display(Name = "System Bids (Scheduled)", Description = "System Bid Scheduled for Bidding", Order = 2)]
        Scheduled,

        [Display(Name = "System Bids (Open)", Description = "System Bid Open for Bidding", Order = 3)]
        Open,

        [Display(Name = "System Bids (Expired)", Description = "System Bid Expired for Bidding", Order = 4)]
        Expired,

        [Display(Name = "System Bids (Ended)", Description = "System Bid Ended for Bidding", Order = 5)]
        Ended,

        [Display(Name = "System Bids (Closed)", Description = "System Bid Closed", Order = 6)]
        Closed
    }
}