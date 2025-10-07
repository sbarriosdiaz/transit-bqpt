////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel.DataAnnotations;
using Bqpt.Common;

namespace Bqpt.Domain
{
    public abstract class ControlFields : IAuditable
    {
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
    }
}