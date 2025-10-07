////////////////////////////////////////////////////////////////////////////////////////////////////////
// Broward County Application Services Group
// Application: BC Commerce | Lead Architect: Cesar L Diaz
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
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
    public class BidQuoteQuery : IRequest<BidQuoteResponseDto>
    {
        public BidQuoteQuery(string domainKey)
        {
            DomainKey = domainKey;
        }

        public string DomainKey { get; }

        public class Handler : IRequestHandler<BidQuoteQuery, BidQuoteResponseDto>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService) => _dbService = dbService;

            public async Task<BidQuoteResponseDto> Handle(BidQuoteQuery request, CancellationToken cancellationToken)
            {
                var systemVendors = await _dbService.Set<Vendor>()
                    .AsNoTracking()
                    .Include(v => v.Contacts)
                    .Where(x => x.IsActive)
                     .Select(v => new VendorDto
                     {
                         DomainKey = v.DomainKey,
                         Id = v.Id,
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
                         Contacts = v.Contacts
                        .OrderBy(c => c.AssetWorksContactName)
                        .Where(c => c.IsActive && c.VendorId == v.Id)
                        .Select(c => new ContactDto
                        {
                            AssetWorksVendorName = v.AssetWorksVendorName,
                            AssetWorksVendorNumber = v.AssetWorksVendorNumber,
                            AssetWorksContactName = c.AssetWorksContactName,
                            AssetWorksContactEmail = c.AssetWorksContactEmail,
                            AssetWorksContactPhone = c.AssetWorksContactPhone,
                            AssetWorksContactAddress1 = c.AssetWorksContactAddress1,
                            AssetWorksContactAddress2 = c.AssetWorksContactAddress2,
                            AssetWorksContactAddress3 = c.AssetWorksContactAddress3,
                            AssetWorksContactAddress4 = c.AssetWorksContactAddress4
                        })
                        .ToList(),
                     }).ToListAsync(cancellationToken);

                var bidQuote = await _dbService.Set<BidQuote>()
                    .AsNoTracking()
                    .Include(x => x.Parts)
                    .Include(q => q.Parts.Select(v => v.Bids))
                    .Include(q => q.VendorBidQuoteAttachments.Select(a => a.Attachment))
                    .Include(q => q.Parts.Select(v => v.Bids.Select(b => b.Vendor.Contacts)))
                    .Include(q => q.Parts.Select(v => v.VendorInParts.Select(b => b.Vendor.Contacts)))
                    .Where(x => x.DomainKey.Equals(request.DomainKey) && x.IsActive)
                    .Select(bq => new BidQuoteDto
                    {
                        Id = bq.Id,
                        AssetWorksBidQuoteId = bq.AssetWorksBidQuoteId,
                        DomainKey = bq.DomainKey,
                        Status = (bq.BidScheduledEndTime < DateTime.Now && bq.BidEndTime == null) ? (nameof(BidQuoteStatus.Expired)) : bq.Status,
                        BidStartTime = bq.BidStartTime,
                        BidEndTime = bq.BidEndTime,
                        BidScheduledStartTime = bq.BidScheduledStartTime,
                        BidScheduledEndTime = bq.BidScheduledEndTime,
                        BidImportedTime = bq.BidImportedTime,
                        BidClosedTime = bq.BidClosedTime,
                        DisplayOrder=bq.DisplayOrder,
                        AssetWorksDateInserted = bq.AssetWorksDateInserted,
                        AssetWorksDateApproval = bq.AssetWorksDateApproval,
                        AssetWorksDateRequired = bq.AssetWorksDateRequired,
                        AssetWorksDateRequested = bq.AssetWorksDateRequested,
                        Attachments = bq.VendorBidQuoteAttachments.Select(a => new VendorBidQuoteAttachmentDto
                        {
                            Id = a.Id,
                            DomainKey = a.DomainKey,
                            AttachmentId = a.Attachment.AttachmentId,
                            BlobUrl = a.Attachment.BlobUrl,
                            FileName = a.Attachment.FileName,
                            FileExtension = a.Attachment.FileExtension,
                            FileFriendlyName = a.Attachment.AttachmentFriendlyName,
                            Status = a.Attachment.Status
                        }),
                        Notes = bq.Notes.Select(n => new BidQuoteNoteDto
                        {
                            Id = n.Id,
                            Note = n.Note,
                        }).ToList(),
                        Parts = bq.Parts
                            .Where(p => p.IsActive)                            
                            .OrderBy(p => p.AssetWorksMinPartLineNumberInt )// Ensuring parts are ordered
                            .Select(p => new BidQuotePartDto
                            {
                                Id = p.Id,
                                DomainKey = p.DomainKey,
                                BidQuoteStatus = bq.Status,
                                AssetWorksPartNumber = p.AssetWorksPartNumber,
                                AssetWorksPartCommodityCode = p.AssetWorksPartCommodityCode,
                                AssetWorksPartCategory = p.AssetWorksPartCategory,
                                AssetWorksPartDescription = p.AssetWorksPartDescription,
                                AssetWorksPartManufacturerPartNumber = p.AssetWorksPartManufacturerPartNumber,
                                AssetWorksPartSuffix = p.AssetWorksPartSuffix,
                                AssetWorksPartLineNumber = p.AssetWorksPartLineNumber,
                                AssetWorksPartLocationCode = p.AssetWorksPartLocationCode,
                                AssetWorksPartDateInserted = p.AssetWorksPartDateInserted,
                                AssetWorksPartDateRequired = p.AssetWorksPartDateRequired,
                                AssetWorksPartQuantityOnHand = p.AssetWorksPartQuantityOnHand,
                                AssetWorksPartQuantityOnOrder = p.AssetWorksPartQuantityOnOrder,
                                AssetWorksPartQuantityCommited = p.AssetWorksPartQuantityCommited,
                                AssetWorksPartQuantityRequested = p.AssetWorksPartQuantityRequested,
                                AssetWorksUnitPrice = p.AssetWorksUnitPrice,
                                RequestedQuantity = p.RequestedQuantity,
                                PurchaseOrderQuantity = p.PurchaseOrderQuantity,
                                VendorInParts = p.VendorInParts
                                    .Where(vp => vp.IsActive)
                                    .OrderBy(vp => vp.Vendor.AssetWorksVendorName)
                                    .Select(vp => new VendorInPartDto
                                    {
                                        Id = vp.Id,
                                        DomainKey = vp.DomainKey,
                                        NotificationSent = vp.NotificationSent,
                                        VendorId = vp.VendorId,
                                        BidQuotePartId = p.Id,
                                        Vendor = new VendorDto
                                        {
                                            DomainKey = vp.Vendor.DomainKey,
                                            Id = vp.Vendor.Id,
                                            AssetWorksVendorNumber = vp.Vendor.AssetWorksVendorNumber,
                                            AssetWorksVendorName = vp.Vendor.AssetWorksVendorName,
                                            AssetWorksVendorAccountingSystemNumber = vp.Vendor.AssetWorksVendorAccountingSystemNumber,
                                            AssetWorksVendorContactName = vp.Vendor.AssetWorksVendorContactName,
                                            AssetWorksVendorEmail = vp.Vendor.AssetWorksVendorEmail,
                                            AssetWorksVendorPhone = vp.Vendor.AssetWorksVendorPhone
                                        }
                                    }).ToList(),
                                VendorBids = p.Bids
                                    .Where(vb => vb.IsActive)
                                    .OrderBy(vb => vb.Vendor.AssetWorksVendorName)
                                    .Select(vb => new VendorPartBidDto
                                    {
                                        Id = vb.Id,
                                        DomainKey = vb.DomainKey,
                                        VendorId = vb.VendorId,
                                        BidQuoteStatus = bq.Status,
                                        BidQuotePartId = p.Id,
                                        QuantityAvailable = vb.QuantityAvailable ?? 0,
                                        ItemsPerUnit = vb.ItemsPerUnit ?? 0,
                                        UnitPrice = vb.UnitPrice ?? 0,
                                        EstimateDeliveryDate = vb.EstimateDeliveryDate,
                                        Core=vb.Core,
                                        Comment = vb.Comment,
                                        IsSelected = vb.IsSelected,
                                        Vendor = new VendorDto
                                        {
                                            DomainKey = vb.Vendor.DomainKey,
                                            Id = vb.Vendor.Id,
                                            AssetWorksVendorNumber = vb.Vendor.AssetWorksVendorNumber,
                                            AssetWorksVendorName = vb.Vendor.AssetWorksVendorName,
                                            AssetWorksVendorAccountingSystemNumber = vb.Vendor.AssetWorksVendorAccountingSystemNumber,
                                            AssetWorksVendorContactName = vb.Vendor.AssetWorksVendorContactName,
                                            AssetWorksVendorEmail = vb.Vendor.AssetWorksVendorEmail,
                                            AssetWorksVendorPhone = vb.Vendor.AssetWorksVendorPhone
                                        },
                                        Attachment = bq.VendorBidQuoteAttachments
                                            .Where(v => v.VendorId == vb.VendorId)
                                            .OrderByDescending(a => a.Id)
                                            .Select(a => new VendorBidQuoteAttachmentDto
                                            {
                                                Id = a.Id,
                                                DomainKey = a.DomainKey,
                                                AttachmentId = a.Attachment.AttachmentId,
                                                BlobUrl = a.Attachment.BlobUrl,
                                                FileName = a.Attachment.FileName,
                                                FileExtension = a.Attachment.FileExtension,
                                                FileFriendlyName = a.Attachment.AttachmentFriendlyName,
                                                Status = a.Attachment.Status
                                            }).FirstOrDefault()
                                    }).ToList()
                            }).ToList()
                    }).FirstOrDefaultAsync(cancellationToken);

                return new BidQuoteResponseDto { BidQuote = bidQuote, SystemVendors = systemVendors };
            }
        }
    }
}