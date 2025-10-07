////////////////////////////////////////////////////////////////////////////////////////////////////////
// Broward County Application Services Group
// Application: BC Commerce | Lead Architect: Cesar L Diaz
////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using Microsoft.AspNet.Identity;

namespace Bqpt.Persistence
{
    public class AppDbContext : DbContext, IDbService
    {
        private const int _timeOut = 180;

        private DbContextTransaction _currentTransaction;

        public AppDbContext() : base("AppConnectionString")
        {
            Database.SetInitializer<AppDbContext>(null);
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = _timeOut;
        }

        #region Entities

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<BidQuote> BidQuotes { get; set; }
        public DbSet<BidQuoteNote> BidQuoteNotes { get; set; }
        public DbSet<BidQuotePart> BidQuoteParts { get; set; }
        public DbSet<BidQuoteStatusHistory> BidQuoteStatusHistory { get; set; }
        public DbSet<VendorPartBid> VendorPartBids { get; set; }
        public DbSet<VendorBidQuoteAttachment> VendorBidQuoteAttachments { get; set; }
        public DbSet<VendorPartBidHistory> VendorPartBidHistory { get; set; }
        public DbSet<VendorPartBidSelectedHistory> VendorPartBidSelectedHistory { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        #endregion Entities

        public void BeginTransaction()

        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public async Task CloseTransactionAsync(Exception exception)
        {
            try
            {
                if (_currentTransaction != null && exception != null)
                {
                    _currentTransaction.Rollback();
                    return;
                }

                await SaveChangesAsync();

                if (_currentTransaction != null)
                {
                    _currentTransaction.Commit();
                }
            }
            catch (Exception)
            {
                if (_currentTransaction != null && _currentTransaction.UnderlyingTransaction.Connection != null)
                {
                    _currentTransaction.Rollback();
                }

                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public string IdentityProvider => HttpRuntime.AppDomainAppId != null
                                                                        && HttpContext.Current != null
                                                                        && HttpContext.Current.User.Identity.IsAuthenticated
                                             ? HttpContext.Current.User.Identity.GetUserId()
                                             : Guid.Empty.ToString();

        public Func<DateTime> TimeStampProvider { get; set; } = () => DateTime.Now;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            TrackChanges();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void TrackChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Entity is IAuditable)
                {
                    var auditable = entry.Entity as IAuditable;

                    if (entry.State == EntityState.Added)
                    {
                        auditable.CreatedBy = IdentityProvider;
                        auditable.CreatedOn = TimeStampProvider();
                    }
                    else
                    {
                        auditable.UpdatedBy = IdentityProvider;
                        auditable.UpdatedOn = TimeStampProvider();

                        if (auditable.IsActive is false)
                        {
                            auditable.DeactivatedBy = IdentityProvider;
                            auditable.DeactivatedOn = TimeStampProvider();
                        }
                        if (auditable.IsDeleted is false)
                        {
                            auditable.IsActive = false;
                            auditable.DeletedBy = IdentityProvider;
                            auditable.DeletedOn = TimeStampProvider();
                        }
                    }
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder != null)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.Properties()
                   .Where(x => x.PropertyType.FullName.Equals("System.String")
                                && !x.GetCustomAttributes(false).OfType<ColumnAttribute>()
                                .Any(q => q.TypeName != null
                                && q.TypeName.Equals("varchar", StringComparison.InvariantCultureIgnoreCase)))
                   .Configure(c => c.IsUnicode(false));

                // DB - Schema Conventions
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                modelBuilder.HasDefaultSchema("dbo");
                modelBuilder.Conventions.Add(new DecimalPropertyConvention(AppConstants.HasDecimals18, AppConstants.HasDecimals2));

                // Domain Models Configurations
                modelBuilder.Configurations.Add(new ContactConfiguration());
                modelBuilder.Configurations.Add(new VendorConfiguration());

                modelBuilder.Configurations.Add(new BidQuoteConfiguration());
                modelBuilder.Configurations.Add(new BidQuoteNoteConfiguration());
                modelBuilder.Configurations.Add(new BidQuotePartConfiguraiton());
                modelBuilder.Configurations.Add(new BidQuoteStatusHistoryConfiguration());
                modelBuilder.Configurations.Add(new VendorBidQuoteAttachmentConfiguration());
                modelBuilder.Configurations.Add(new VendorPartBidConfiguration());
                modelBuilder.Configurations.Add(new VendorPartBidHistoryConfiguration());

                modelBuilder.Configurations.Add(new AttachmentConfiguration());
                modelBuilder.Configurations.Add(new VendorInPartConfiguration());
                modelBuilder.Configurations.Add(new VendorPartBidSelectedHistoryConfiguration());

                modelBuilder.Entity<VendorPartBid>()
                    .HasMany(vpb => vpb.VendorPartBidHistories)
                    .WithRequired(vph => vph.VendorPartBid)
                    .WillCascadeOnDelete(false);

                
            }
        }
    }
}