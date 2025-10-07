////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using System.Web;
using Bqpt.Common;

namespace Bqpt.Application
{
    public class AttachmentViewModel
    {
        public string AttachmentId { get; set; }
        public string BlobUrl { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public double FileSizeKb { get; set; }
        public string AttachmentFriendlyName { get; set; }
        public string Status { get; set; }

        [Required(ErrorMessage = AppConstants.StandardValidationErrorMessage), Display(Name = "Browse for Support Document")]
        public HttpPostedFileBase File { get; set; }

        [StringLength(AppConstants.HasMaxLength512), Display(Name = "Brief Description for the current File (if any)")]
        public string FileDescription { get; set; }
    }
}