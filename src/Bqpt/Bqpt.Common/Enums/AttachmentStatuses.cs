using System.ComponentModel.DataAnnotations;

namespace Bqpt.Common
{
    public enum AttachmentStatus : byte
    {
        [Display(Name = "On Scanning Process")]
        OnScanning = 1,

        [Display(Name = "Active")]
        Active,

        [Display(Name = "Archived")]
        Archived,

        [Display(Name = "Inactive")]
        InActive
    }
}