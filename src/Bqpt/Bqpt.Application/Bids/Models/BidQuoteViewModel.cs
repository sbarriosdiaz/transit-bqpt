using System;
using System.ComponentModel.DataAnnotations;
using BC.Identity.Kernel;
using Bqpt.Common;

namespace Bqpt.Application
{
    public class BidQuoteViewModel : BaseViewModel<int>
    {
        [Required(ErrorMessage = AppSettings.STANDARD_VALIDATION_ERROR_MESSAGE), StringLength(AppSettings.HAS_MAXLENGHT96), Display(Name = "Asset Works Bid ID")]
        public string AssetWorksBidQuoteId { get; set; }

        [Display(Name = "Scheduled End Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm}")]
        public DateTime? BidScheduledEndTime { get; set; }

        public DateTime? OriginalBidScheduledEndTime => BidScheduledEndTime;

        [Display(Name = "Scheduled Start Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm}")]
        public DateTime? BidScheduledStartTime { get; set; }

        public DateTime? BidStartTime { get; set; }
    }
}