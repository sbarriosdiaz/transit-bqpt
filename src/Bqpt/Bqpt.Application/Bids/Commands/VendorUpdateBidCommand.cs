using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using MediatR;

namespace Bqpt.Application
{
    public class VendorUpdateBidCommand : IRequest<TransactionResult<string>>
    {
        public VendorUpdateBidCommand(VendorPartBidViewModel form) => Form = form;

        public VendorPartBidViewModel Form { get; }

        public class Handler : IRequestHandler<VendorUpdateBidCommand, TransactionResult<string>>
        {
            private readonly IDbService _dbService;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IDbService dbService, ICurrentUserService currentUserService)
            {
                _dbService = dbService;
                _currentUserService = currentUserService;
            }

            public async Task<TransactionResult<string>> Handle(VendorUpdateBidCommand request, CancellationToken cancellationToken)
            {
                var bid = await _dbService.Set<VendorPartBid>()
                    .Include(vb => vb.BidQuotePart)
                    .Include(vp=>vp.Vendor)                    
                    .Include(vp=>vp.VendorPartBidHistories)
                    .FirstOrDefaultAsync(vb => vb.DomainKey.Equals(request.Form.DomainKey), cancellationToken);

                if (bid is null) return new TransactionResult<string>("Error", "Error: Bid does not exist");

                bid.EstimateDeliveryDate = request.Form.EstimateDeliveryDate;
                bid.QuantityAvailable = request.Form.QuantityAvailable;
                bid.UnitPrice = request.Form.UnitPrice;
                bid.ItemsPerUnit = request.Form.ItemsPerUnit;
                bid.Core = request.Form.Core;
                bid.Comment = Regex.Replace(string.IsNullOrEmpty(request.Form.Comment) ? "" : request.Form.Comment, @"\s+", " ");
                foreach (var history in bid.VendorPartBidHistories)
                {
                    history.IsDeleted = true;
                }

                bid.VendorPartBidHistories.Add(new VendorPartBidHistory
                {
                    VendorPartBid = bid,
                    EstimateDeliveryDate = bid.EstimateDeliveryDate,
                    QuantityAvailable = bid.QuantityAvailable,
                    UnitPrice = bid.UnitPrice,
                    ItemsPerUnit = bid.ItemsPerUnit,
                    Core=bid.Core,
                    IsDeleted = false,                    
                    Comment = bid.Comment,
                    LoggedInEmailAddress = _currentUserService.UserName.ToLowerInvariant(),
                    VendorId = bid.VendorId,
                    BidQuoteId = bid.BidQuotePart.BidQuoteId,
                    BidQuotePartId = bid.BidQuotePart.Id,

                });

                await _dbService.SaveChangesAsync(cancellationToken);

                return new TransactionResult<string>(bid.BidQuotePart.AssetWorksPartDescription, AppConstants.TransactionSuccess);
            }
        }
    }
}