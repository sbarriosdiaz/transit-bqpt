using System.Data.Entity.ModelConfiguration;
using Bqpt.Common;
using Bqpt.Domain;

namespace Bqpt.Persistence
{
    public class ContactConfiguration : EntityTypeConfiguration<Contact>
    {
        public ContactConfiguration()
        {
            ToTable(nameof(Contact));
            MapToStoredProcedures();

            Property(p => p.AssetWorksContactName).IsRequired().HasMaxLength(AppConstants.HasMaxLength128);
            Property(p => p.AssetWorksContactEmail).IsRequired().HasMaxLength(AppConstants.HasMaxLength256);
            Property(p => p.AssetWorksContactPhone).HasMaxLength(AppConstants.HasMaxLength96);
            Property(p => p.AssetWorksContactAddress1).HasMaxLength(AppConstants.HasMaxLength256);
            Property(p => p.AssetWorksContactAddress2).HasMaxLength(AppConstants.HasMaxLength256);
            Property(p => p.AssetWorksContactAddress3).HasMaxLength(AppConstants.HasMaxLength256);
            Property(p => p.AssetWorksContactAddress4).HasMaxLength(AppConstants.HasMaxLength256);
        }
    }
}