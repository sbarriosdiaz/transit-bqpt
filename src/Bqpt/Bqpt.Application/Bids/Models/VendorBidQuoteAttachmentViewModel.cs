using System.ComponentModel.DataAnnotations;
using System.Web;
using Bqpt.Common;

namespace Bqpt.Application
{
    public class VendorBidQuoteAttachmentViewModel : BaseViewModel<int>
    {
        public int VendorId { get; set; }
        public int BidQuoteId { get; set; }

        [Required(ErrorMessage = "File is Required"), Display(Name = "Bid Quote Invoice File")]
        public HttpPostedFileBase File { get; set; }

        [StringLength(AppConstants.HasMaxLength512), Display(Name = "Name for current File (if any)")]
        public string FileFriendlyName { get; set; }
    }
}