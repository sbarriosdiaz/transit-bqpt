using System.Data.Entity.ModelConfiguration;
using Bqpt.Common;
using Bqpt.Domain;

namespace Bqpt.Persistence
{
    public class BidQuoteConfiguration : EntityTypeConfiguration<BidQuote>
    {
        public BidQuoteConfiguration()
        {
            ToTable(nameof(BidQuote));
            MapToStoredProcedures();

            Property(p => p.AssetWorksBidQuoteId).IsRequired().HasMaxLength(AppConstants.HasMaxLength48);
            Property(p => p.Status).IsRequired().HasMaxLength(AppConstants.HasMaxLength24);

            HasMany(p => p.Parts).WithRequired(p => p.BidQuote).HasForeignKey(p => p.BidQuoteId).WillCascadeOnDelete(false);
            HasMany(p => p.StatusHistories).WithRequired(p => p.BidQuote).HasForeignKey(p => p.BidQuoteId).WillCascadeOnDelete(false);
            HasMany(p => p.Notes).WithRequired(p => p.BidQuote).HasForeignKey(p => p.BidQuoteId).WillCascadeOnDelete(false);
            HasMany(p => p.VendorBidQuoteAttachments).WithRequired(p => p.BidQuote).HasForeignKey(p => p.BidQuoteId).WillCascadeOnDelete(false);
            Ignore(p => p.SetStatus);
        }
    }
}