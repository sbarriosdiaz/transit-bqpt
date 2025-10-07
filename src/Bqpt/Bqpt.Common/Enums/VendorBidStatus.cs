using System.ComponentModel.DataAnnotations;

namespace Bqpt.Common
{
    public enum VendorBidStatus
    {
        [Display(Name = "Not-Submitted", Description = "Vendor Not-Submitted")]
        NotSubmitted,

        [Display(Name = "Submitted", Description = "Vendor Submitted")]
        Submitted
    }
}