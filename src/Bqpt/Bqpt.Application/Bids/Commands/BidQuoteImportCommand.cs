using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using FluentValidation;
using MediatR;

namespace Bqpt.Application
{
    public class BidQuoteImportCommand : IRequest<TransactionResult<string>>
    {
        public BidQuoteImportCommand(BidQuoteViewModel form) => Form = form;

        public BidQuoteViewModel Form { get; }

        public class Handler : IRequestHandler<BidQuoteImportCommand, TransactionResult<string>>
        {
            private readonly IDbService _dbService;
            private readonly IAssetWorksServices _awService;

            public Handler(IDbService dbService, IAssetWorksServices awService)
            {
                _dbService = dbService;
                _awService = awService;
            }

            private async Task ImportAssetWorkVendors(CancellationToken cancellationToken)
            {
                var awVendors = await _awService.GetAssetWorksVendors();
                var localVendors = await _dbService.Set<Vendor>().ToListAsync(cancellationToken);

                awVendors = awVendors.Where(x => !string.IsNullOrEmpty(x.AssetWorksVendorEmail)).ToList();

                var newVendors = awVendors.Where(awv => !localVendors.Any(x => x.AssetWorksVendorNumber.Equals(awv.AssetWorksVendorNumber))).ToList();

                AssetWorksVendorDto awVendor;
                foreach (var v in localVendors)
                {
                    v.IsActive = false;
                    awVendor = awVendors.FirstOrDefault(awv => awv.AssetWorksVendorNumber.Equals(v.AssetWorksVendorNumber));
                    if (awVendor != null)
                    {
                        v.IsActive = true;
                        v.AssetWorksVendorNumber = awVendor.AssetWorksVendorNumber;
                        v.AssetWorksVendorAccountingSystemNumber = awVendor.AssetWorksVendorAccountingSystemNumber;
                        v.AssetWorksVendorName = awVendor.AssetWorksVendorName;
                        v.AssetWorksVendorContactName = awVendor.AssetWorksVendorContactName;
                        v.AssetWorksVendorAddress1 = awVendor.AssetWorksVendorAddress1;
                        v.AssetWorksVendorAddress2 = awVendor.AssetWorksVendorAddress2;
                        v.AssetWorksVendorAddress3 = awVendor.AssetWorksVendorAddress3;
                        v.AssetWorksVendorAddress4 = awVendor.AssetWorksVendorAddress4;
                        v.AssetWorksVendorEmail = awVendor.AssetWorksVendorEmail;
                        v.AssetWorksVendorPhone = awVendor.AssetWorksVendorPhone;
                        v.AssetWorksVendorFax = awVendor.AssetWorksVendorFax;
                        v.CommodityCodes = awVendor.CommodityCodes;
                    }
                }

                var vendors = new List<Vendor>();
                foreach (var v in newVendors)
                {
                    vendors.Add(new Vendor
                    {
                        AssetWorksVendorNumber = v.AssetWorksVendorNumber,
                        AssetWorksVendorName = v.AssetWorksVendorName,
                        AssetWorksVendorAccountingSystemNumber = v.AssetWorksVendorAccountingSystemNumber,
                        AssetWorksVendorContactName = v.AssetWorksVendorContactName,
                        AssetWorksVendorAddress1 = v.AssetWorksVendorAddress1,
                        AssetWorksVendorAddress2 = v.AssetWorksVendorAddress2,
                        AssetWorksVendorAddress3 = v.AssetWorksVendorAddress3,
                        AssetWorksVendorAddress4 = v.AssetWorksVendorAddress4,
                        AssetWorksVendorEmail = v.AssetWorksVendorEmail,
                        AssetWorksVendorPhone = v.AssetWorksVendorPhone,
                        AssetWorksVendorFax = v.AssetWorksVendorFax,
                        CommodityCodes = v.CommodityCodes,
                        IsActive = true
                    });
                }
                _dbService.Set<Vendor>().AddRange(vendors);
                await _dbService.SaveChangesAsync(cancellationToken);
            }

            private async Task ImportAssetWorkVendorContacts(CancellationToken cancellationToken)
            {
                var awContacts = await _awService.GetAssetWorksVendorContacts();
                var localContacts = await _dbService.Set<Contact>().Include(c => c.Vendor).ToListAsync(cancellationToken);
                var localVendors = await _dbService.Set<Vendor>().Where(v => v.IsActive.Equals(true)).ToListAsync(cancellationToken);

                awContacts = awContacts.Where(x => !string.IsNullOrEmpty(x.AssetWorksContactEmail)).ToList();

                var newContacts = awContacts.Where(awc => !localContacts.Any(x => x.AssetWorksContactEmail.Equals(awc.AssetWorksContactEmail))).ToList();

                AssetWorksVendorContactDto awContact;
                foreach (var c in localContacts)
                {
                    c.IsActive = false;
                    awContact = awContacts.FirstOrDefault(awc => awc.AssetWorksContactEmail.Equals(c.AssetWorksContactEmail));
                    if (awContact != null)
                    {
                        c.IsActive = c.Vendor.IsActive;
                        c.AssetWorksContactName = awContact.AssetWorksContactName;
                        c.AssetWorksContactPhone = awContact.AssetWorksContactPhone;
                        c.AssetWorksContactEmail = awContact.AssetWorksContactEmail;
                        c.AssetWorksContactAddress1 = awContact.AssetWorksContactAddress1;
                        c.AssetWorksContactAddress2 = awContact.AssetWorksContactAddress2;
                        c.AssetWorksContactAddress3 = awContact.AssetWorksContactAddress3;
                        c.AssetWorksContactAddress4 = awContact.AssetWorksContactAddress4;
                    }
                }

                var contacts = new List<Contact>();
                Vendor vendor;
                foreach (var c in newContacts)
                {
                    vendor = localVendors.FirstOrDefault(x => x.AssetWorksVendorNumber.Equals(c.AssetWorksVendorNumber));

                    if (vendor != null)
                    {
                        contacts.Add(new Contact
                        {
                            IsActive = true,
                            AssetWorksContactName = c.AssetWorksContactName,
                            AssetWorksContactPhone = c.AssetWorksContactPhone,
                            AssetWorksContactEmail = c.AssetWorksContactEmail,
                            AssetWorksContactAddress1 = c.AssetWorksContactAddress1,
                            AssetWorksContactAddress2 = c.AssetWorksContactAddress2,
                            AssetWorksContactAddress3 = c.AssetWorksContactAddress3,
                            AssetWorksContactAddress4 = c.AssetWorksContactAddress4,
                            Vendor = vendor
                        });
                    }
                }
                _dbService.Set<Contact>().AddRange(contacts);
                await _dbService.SaveChangesAsync(cancellationToken);
            }

            public async Task<TransactionResult<string>> Handle(BidQuoteImportCommand request, CancellationToken cancellationToken)
            {
                var normalizedQuoteId = request.Form.AssetWorksBidQuoteId.ToUpperInvariant();
                var bidQuote = await _dbService.Set<BidQuote>().FirstOrDefaultAsync(b => b.AssetWorksBidQuoteId.Equals(normalizedQuoteId), cancellationToken);

                if (bidQuote != null) return new TransactionResult<string>(bidQuote.DomainKey, AppConstants.TransactionSuccess);

                var awBidQuote = await _awService.GetAssetWorksBidQuotesById(request.Form.AssetWorksBidQuoteId);
                var awBidQuoteParts = await _awService.GetAssetWorksBidQuoteParts(request.Form.AssetWorksBidQuoteId);

                await ImportAssetWorkVendors(cancellationToken);
                await ImportAssetWorkVendorContacts(cancellationToken);

                bidQuote = new BidQuote
                {
                    AssetWorksBidQuoteId = awBidQuote.AssetWorksBidQuoteId.ToUpperInvariant(),
                    AssetWorksDateInserted = awBidQuote.AssetWorksDateInserted,
                    AssetWorksDateApproval = awBidQuote.AssetWorksDateApproval,
                    AssetWorksDateRequired = awBidQuote.AssetWorksDateRequired,
                    AssetWorksDateRequested = awBidQuote.AssetWorksDateRequested,
                    SetStatus = BidQuoteStatus.New,
                    BidImportedTime = DateTime.Now,
                    DisplayOrder=0,
                };

                foreach (var p in awBidQuoteParts.OrderBy(x => x.AssetWorksMinPartLineNumber.Length).ThenBy(x => x.AssetWorksMinPartLineNumber))
                {
                    bidQuote.Parts.Add(new BidQuotePart
                    {
                        AssetWorksPartNumber = p.AssetWorksPartNumber,
                        AssetWorksPartCommodityCode = p.AssetWorksPartCommodityCode,

                        AssetWorksPartManufacturerPartNumber = p.AssetWorksPartManufacturerPartNumber,
                        AssetWorksPartSuffix = p.AssetWorksPartSuffix,
                        AssetWorksPartDescription = p.AssetWorksPartDescription,
                        AssetWorksUnitPrice = p.AssetWorksUnitPrice,

                        AssetWorksPartCategory = p.AssetWorksPartCategory,
                        AssetWorksPartLineNumber = p.AssetWorksPartLineNumber,
                        AssetWorksMinPartLineNumber = p.AssetWorksMinPartLineNumber,
                        AssetWorksMinPartLineNumberInt=Convert.ToInt32(p.AssetWorksMinPartLineNumber),
                        AssetWorksPartLocationCode = p.AssetWorksPartLocationCode,

                        AssetWorksPartDateInserted = p.AssetWorksPartDateInserted,
                        AssetWorksPartDateRequired = p.AssetWorksPartDateRequired,

                        AssetWorksPartQuantityRequested = p.AssetWorksPartQuantityRequested,
                        AssetWorksPartQuantityOnHand = p.AssetWorksPartQuantityOnHand,
                        AssetWorksPartQuantityOnOrder = p.AssetWorksPartQuantityOnOrder,
                        AssetWorksPartQuantityCommited = p.AssetWorksPartQuantityCommited,

                        RequestedQuantity = p.AssetWorksPartQuantityRequested,
                        PurchaseOrderQuantity = p.AssetWorksPartQuantityRequested
                    });
                }

                bidQuote.StatusHistories.Add(new BidQuoteStatusHistory
                {
                    Status = bidQuote.Status
                });

                _dbService.Set<BidQuote>().Add(bidQuote);

                await _dbService.SaveChangesAsync(cancellationToken);
                return new TransactionResult<string>(bidQuote.DomainKey, AppConstants.TransactionSuccess);
            }
        }
    }
}