using System.Data.Entity.ModelConfiguration;
using Bqpt.Common;
using Bqpt.Domain;

namespace Bqpt.Persistence
{
    public class BidQuoteStatusHistoryConfiguration : EntityTypeConfiguration<BidQuoteStatusHistory>
    {
        public BidQuoteStatusHistoryConfiguration()
        {
            ToTable(nameof(BidQuoteStatusHistory));
            MapToStoredProcedures();

            Property(p => p.Status).IsRequired().HasMaxLength(AppConstants.HasMaxLength24);
        }
    }
}