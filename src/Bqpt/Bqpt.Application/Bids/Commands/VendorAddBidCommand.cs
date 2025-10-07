using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using MediatR;

namespace Bqpt.Application
{
    public class VendorAddBidCommand : IRequest<TransactionResult<string>>
    {
        public VendorAddBidCommand(VendorPartBidViewModel form) => Form = form;

        public VendorPartBidViewModel Form { get; }

        public class Handler : IRequestHandler<VendorAddBidCommand, TransactionResult<string>>
        {
            private readonly IDbService _dbService;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IDbService dbService, ICurrentUserService currentUserService)
            {
                _dbService = dbService;
                _currentUserService = currentUserService;
            }

            public async Task<TransactionResult<string>> Handle(VendorAddBidCommand request, CancellationToken cancellationToken)
            {
                var vendorEmail = _currentUserService.UserName.ToLowerInvariant();

                var bidQuotePartId = request.Form.BidQuotePartId ?? 0;

                var vendor = await _dbService.Set<Vendor>()
                                .FirstOrDefaultAsync(v => v.AssetWorksVendorEmail.Equals(vendorEmail) || v.Contacts.Select(c => c.AssetWorksContactEmail).Contains(vendorEmail), cancellationToken);

                if (vendor is null) return new TransactionResult<string>("Error", "Error: Vendor does not exist");

                var vendorId = vendor.Id;

                var part = await _dbService.Set<BidQuotePart>().FirstOrDefaultAsync(a => a.Id == bidQuotePartId, cancellationToken);

                if (part == null) return new TransactionResult<string>("Error", "Error: Part does not exist");

                var bidCheck = await _dbService.Set<VendorPartBid>()
                                          .FirstOrDefaultAsync(a => a.BidQuotePartId == bidQuotePartId && a.VendorId == vendorId, cancellationToken);

                if (bidCheck != null) return new TransactionResult<string>("Error", "Error: Bid already exist");

                var partCheck = await _dbService.Set<VendorInPart>()
                                          .FirstOrDefaultAsync(a => a.BidQuotePartId == bidQuotePartId && a.VendorId == vendorId, cancellationToken);

                if (partCheck == null) return new TransactionResult<string>("Error", "Error: Bid Part does not exist");

                var bid = new VendorPartBid()
                {
                    BidQuotePart = part,
                    Vendor = vendor,
                    EstimateDeliveryDate = request.Form.EstimateDeliveryDate,
                    QuantityAvailable = request.Form.QuantityAvailable,
                    UnitPrice = request.Form.UnitPrice,
                    ItemsPerUnit = request.Form.ItemsPerUnit,
                    Core=request.Form.Core,
                    Comment = Regex.Replace(string.IsNullOrEmpty(request.Form.Comment) ? "" : request.Form.Comment, @"\s+", " ")
                };

                bid.VendorPartBidHistories.Add(new VendorPartBidHistory
                {
                    VendorPartBid = bid,
                    EstimateDeliveryDate = bid.EstimateDeliveryDate,
                    QuantityAvailable = bid.QuantityAvailable,
                    Core = bid.Core,
                    UnitPrice = bid.UnitPrice,
                    IsDeleted=false,
                    ItemsPerUnit = bid.ItemsPerUnit,
                    Comment = bid.Comment,
                    LoggedInEmailAddress = vendorEmail,
                    VendorId = vendorId,
                    BidQuoteId = bid.BidQuotePart.BidQuoteId,
                    BidQuotePartId= bid.BidQuotePart.Id,

                });

                _dbService.Set<VendorPartBid>().Add(bid);

                await _dbService.SaveChangesAsync(cancellationToken);

                return new TransactionResult<string>(part.AssetWorksPartDescription, AppConstants.TransactionSuccess);
            }
        }
    }
}