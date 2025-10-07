////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bqpt.Common;
using Newtonsoft.Json;

namespace Bqpt.Domain
{
    /// <summary>
    /// this is a standard Attachment Entity, will cover pretty much all scenarios if needed extend
    /// the entity and complete configuration in EF Configuration file
    /// </summary>
    public class Attachment : IAuditable
    {
        public string AttachmentId { get; set; }
        public string BlobUrl { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public double FileSizeKb { get; set; }

        public string AttachmentFriendlyName { get; set; }

        public string Status
        {
            get { return SetStatus.ToString(); }
            private set { SetStatus = value.ParseEnum<AttachmentStatus>(); }
        }

        [NotMapped]
        public AttachmentStatus SetStatus { get; set; }

        public byte? DisplayOrder { get; set; }
        public bool? IsDeleted { get; set; }

        [StringLength(AppConstants.HasMaxLength48)]
        public string DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }
        public bool IsActive { get; set; } = true;

        [StringLength(AppConstants.HasMaxLength48)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(AppConstants.HasMaxLength48)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [StringLength(AppConstants.HasMaxLength48)]
        public string DeactivatedBy { get; set; }

        public DateTime? DeactivatedOn { get; set; }

        [StringLength(AppConstants.HasMaxLength512)]
        public virtual string Description { get; set; }

        public static explicit operator Attachment(string json) => JsonConvert.DeserializeObject<Attachment>(json);

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}