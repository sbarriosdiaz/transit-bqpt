using System.Data.Entity.ModelConfiguration;
using Bqpt.Common;
using Bqpt.Domain;

namespace Bqpt.Persistence
{
    public class BidQuoteNoteConfiguration : EntityTypeConfiguration<BidQuoteNote>
    {
        public BidQuoteNoteConfiguration()
        {
            ToTable(nameof(BidQuoteNote));
            MapToStoredProcedures();

            Property(p => p.Note).HasMaxLength(AppConstants.HasMaxLength2048);
        }
    }
}