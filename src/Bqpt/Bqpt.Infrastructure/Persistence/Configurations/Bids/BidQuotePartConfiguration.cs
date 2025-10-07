////////////////////////////////////////////////////////////////////////////////////////////////////////
// Broward County Application Services Group
// Application: BC Commerce | Lead Architect: Cesar L Diaz
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Data.Entity.ModelConfiguration;
using Bqpt.Common;
using Bqpt.Domain;

namespace Bqpt.Persistence
{
    public class BidQuotePartConfiguraiton : EntityTypeConfiguration<BidQuotePart>
    {
        public BidQuotePartConfiguraiton()
        {
            ToTable(nameof(BidQuotePart));
            MapToStoredProcedures();

            Property(p => p.AssetWorksPartNumber).IsRequired().HasMaxLength(AppConstants.HasMaxLength48);
            Property(p => p.AssetWorksPartCommodityCode).IsRequired().HasMaxLength(AppConstants.HasMaxLength48);

            Property(p => p.AssetWorksPartCategory).HasMaxLength(AppConstants.HasMaxLength48);
            Property(p => p.AssetWorksPartDescription).HasMaxLength(AppConstants.HasMaxLength1024);
            Property(p => p.AssetWorksUnitPrice).HasMaxLength(AppConstants.HasMaxLength12);
            Property(p => p.AssetWorksPartManufacturerPartNumber).HasMaxLength(AppConstants.HasMaxLength48);
            Property(p => p.AssetWorksPartSuffix).HasMaxLength(AppConstants.HasMaxLength48);
            Property(p => p.AssetWorksPartLineNumber).HasMaxLength(AppConstants.HasMaxLength12);
            Property(p => p.AssetWorksPartLocationCode).HasMaxLength(AppConstants.HasMaxLength48);

            HasMany(p => p.Bids).WithRequired(p => p.BidQuotePart).HasForeignKey(p => p.BidQuotePartId).WillCascadeOnDelete(false);
            HasMany(p => p.VendorInParts).WithRequired(p => p.BidQuotePart).HasForeignKey(p => p.BidQuotePartId).WillCascadeOnDelete(false);
        }
    }
}