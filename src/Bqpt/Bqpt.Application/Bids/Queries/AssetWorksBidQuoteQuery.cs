using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Bqpt.Application
{
    public class AssetWorksBidQuoteQuery : IRequest<AssetWorksBidQuoteDto>
    {
        public AssetWorksBidQuoteQuery(string assetWorksBidQuoteId)
        {
            AssetWorksBidQuoteId = assetWorksBidQuoteId;
        }

        public string AssetWorksBidQuoteId { get; }

        public class Handler : IRequestHandler<AssetWorksBidQuoteQuery, AssetWorksBidQuoteDto>
        {
            private readonly IAssetWorksServices _awService;

            public Handler(IAssetWorksServices awService) => _awService = awService;

            public async Task<AssetWorksBidQuoteDto> Handle(AssetWorksBidQuoteQuery request, CancellationToken cancellationToken)
            {
                var awBidQuote = await _awService.GetAssetWorksBidQuotesById(request.AssetWorksBidQuoteId);
                var awBidQuoteParts = await _awService.GetAssetWorksBidQuotePartsAll(request.AssetWorksBidQuoteId);
                var awVendors = await _awService.GetAssetWorksVendors();
                var awVendorContacts = await _awService.GetAssetWorksVendorContacts();

                if (string.IsNullOrEmpty(awBidQuote.AssetWorksBidQuoteId)) return new AssetWorksBidQuoteDto() { Parts = new List<AssetWorksBidQuotePartDto>() };

                var assetWorksBidQuote = new AssetWorksBidQuoteDto
                {
                    AssetWorksBidQuoteId = awBidQuote.AssetWorksBidQuoteId,
                    AssetWorksDateInserted = awBidQuote.AssetWorksDateInserted,
                    AssetWorksDateApproval = awBidQuote.AssetWorksDateApproval,
                    AssetWorksDateRequired = awBidQuote.AssetWorksDateRequired,
                    AssetWorksDateRequested = awBidQuote.AssetWorksDateRequested,

                    Parts = awBidQuoteParts?.OrderBy(p => p.AssetWorksMinPartLineNumber.Length).ThenBy(p => p.AssetWorksMinPartLineNumber).Select(p => new AssetWorksBidQuotePartDto
                    {
                        AssetWorksPartNumber = p.AssetWorksPartNumber,
                        AssetWorksPartCommodityCode = p.AssetWorksPartCommodityCode,
                        AssetWorksPartManufacturerPartNumber = p.AssetWorksPartManufacturerPartNumber,
                        AssetWorksPartSuffix = p.AssetWorksPartSuffix,
                        AssetWorksPartDescription = p.AssetWorksPartDescription,
                        AssetWorksPartCategory = p.AssetWorksPartCategory,
                        AssetWorksPartLineNumber = p.AssetWorksPartLineNumber,
                        AssetWorksPartLocationCode = p.AssetWorksPartLocationCode,

                        AssetWorksPartDateInserted = p.AssetWorksPartDateInserted,
                        AssetWorksPartDateRequired = p.AssetWorksPartDateRequired,

                        AssetWorksPartQuantityRequested = p.AssetWorksPartQuantityRequested,
                        AssetWorksPartQuantityOnHand = p.AssetWorksPartQuantityOnHand,
                        AssetWorksPartQuantityOnOrder = p.AssetWorksPartQuantityOnOrder,
                        AssetWorksPartQuantityCommited = p.AssetWorksPartQuantityCommited,

                        AssetWorksUnitPrice = p.AssetWorksUnitPrice,

                        Vendors = awVendors?.Where(v => v.CommodityCodes != null && v.CommodityCodes.Split(',').Contains(p.AssetWorksPartCommodityCode)).OrderBy(v => v.AssetWorksVendorName).Select(v => new VendorDto
                        {
                            AssetWorksVendorNumber = v.AssetWorksVendorNumber,
                            AssetWorksVendorName = v.AssetWorksVendorName,
                            AssetWorksVendorAccountingSystemNumber = v.AssetWorksVendorAccountingSystemNumber,
                            AssetWorksVendorContactName = v.AssetWorksVendorContactName,
                            AssetWorksVendorEmail = v.AssetWorksVendorEmail,
                            AssetWorksVendorPhone = v.AssetWorksVendorPhone,
                            AssetWorksVendorFax = v.AssetWorksVendorFax,
                            AssetWorksVendorAddress1 = v.AssetWorksVendorAddress1,
                            AssetWorksVendorAddress2 = v.AssetWorksVendorAddress2,
                            AssetWorksVendorAddress3 = v.AssetWorksVendorAddress3,
                            AssetWorksVendorAddress4 = v.AssetWorksVendorAddress4,
                            CommodityCodes = v.CommodityCodes,

                            Contacts = awVendorContacts?.Where(c => c.AssetWorksVendorNumber.Equals(v.AssetWorksVendorNumber)).Select(c => new ContactDto
                            {
                                AssetWorksVendorName = c.AssetWorksVendorName,
                                AssetWorksVendorNumber = c.AssetWorksVendorNumber,
                                AssetWorksContactName = c.AssetWorksContactName,
                                AssetWorksContactEmail = c.AssetWorksContactEmail,
                                AssetWorksContactPhone = c.AssetWorksContactPhone,
                                AssetWorksContactAddress1 = c.AssetWorksContactAddress1,
                                AssetWorksContactAddress2 = c.AssetWorksContactAddress2,
                                AssetWorksContactAddress3 = c.AssetWorksContactAddress3,
                                AssetWorksContactAddress4 = c.AssetWorksContactAddress4
                            }).ToList()
                        }).ToList()
                    }).ToList()
                };

                return assetWorksBidQuote;
            }
        }
    }
}