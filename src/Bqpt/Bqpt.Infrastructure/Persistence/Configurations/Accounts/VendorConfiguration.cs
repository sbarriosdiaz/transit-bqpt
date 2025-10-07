////////////////////////////////////////////////////////////////////////////////////////////////////////
// Broward County Application Services Group
// Application: BC Commerce | Lead Architect: Cesar L Diaz
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Data.Entity.ModelConfiguration;
using Bqpt.Common;
using Bqpt.Domain;

namespace Bqpt.Persistence
{
    public class VendorConfiguration : EntityTypeConfiguration<Vendor>
    {
        public VendorConfiguration()
        {
            ToTable(nameof(Vendor));
            MapToStoredProcedures();

            Property(p => p.AssetWorksVendorNumber).IsRequired().HasMaxLength(AppConstants.HasMaxLength48);
            Property(p => p.AssetWorksVendorEmail).IsRequired().HasMaxLength(AppConstants.HasMaxLength256);
            Property(p => p.AssetWorksVendorAccountingSystemNumber).HasMaxLength(AppConstants.HasMaxLength48);
            Property(p => p.AssetWorksVendorName).HasMaxLength(AppConstants.HasMaxLength256);
            Property(p => p.AssetWorksVendorContactName).HasMaxLength(AppConstants.HasMaxLength128);
            Property(p => p.AssetWorksVendorAddress1).HasMaxLength(AppConstants.HasMaxLength256);
            Property(p => p.AssetWorksVendorAddress2).HasMaxLength(AppConstants.HasMaxLength256);
            Property(p => p.AssetWorksVendorAddress3).HasMaxLength(AppConstants.HasMaxLength256);
            Property(p => p.AssetWorksVendorAddress4).HasMaxLength(AppConstants.HasMaxLength256);
            Property(p => p.AssetWorksVendorPhone).HasMaxLength(AppConstants.HasMaxLength24);
            Property(p => p.AssetWorksVendorFax).HasMaxLength(AppConstants.HasMaxLength24);
            Property(p => p.CommodityCodes).HasMaxLength(AppConstants.HasMaxLength512);

            HasMany(p => p.Contacts).WithRequired(p => p.Vendor).HasForeignKey(p => p.VendorId).WillCascadeOnDelete(false);
            HasMany(p => p.Bids).WithRequired(p => p.Vendor).HasForeignKey(p => p.VendorId).WillCascadeOnDelete(false);
            HasMany(p => p.VendorInParts).WithRequired(p => p.Vendor).HasForeignKey(p => p.VendorId).WillCascadeOnDelete(false);
            HasMany(p => p.VendorBidQuoteAttachments).WithRequired(p => p.Vendor).HasForeignKey(p => p.VendorId).WillCascadeOnDelete(false);
        }
    }
}