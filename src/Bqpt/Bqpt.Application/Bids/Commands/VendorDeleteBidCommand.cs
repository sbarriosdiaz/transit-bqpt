using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using FluentValidation;
using MediatR;

namespace Bqpt.Application
{
    public class VendorDeleteBidCommand : IRequest<TransactionResult<string>>
    {
        public VendorDeleteBidCommand(VendorPartBidViewModel form) => Form = form;

        public VendorPartBidViewModel Form { get; }

        public class Handler : IRequestHandler<VendorDeleteBidCommand, TransactionResult<string>>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService)
            {
                _dbService = dbService;
            }

            public async Task<TransactionResult<string>> Handle(VendorDeleteBidCommand request, CancellationToken cancellationToken)
            {
                var bid = await _dbService.Set<VendorPartBid>().Include(vb => vb.BidQuotePart)
                                          .Include(ls => ls.Vendor)                                          
                                          .Include(lb => lb.VendorPartBidHistories)
                                          .Where(vb => vb.DomainKey.Equals(request.Form.DomainKey))
                                          .FirstOrDefaultAsync(cancellationToken);

                
                if (bid == null) return new TransactionResult<string>("Error", "Error: Bid does not exist");

                var partDescription = bid?.BidQuotePart?.AssetWorksPartDescription;
                
                if (bid.VendorPartBidHistories != null)
                {
                    
                    foreach (var history in bid.VendorPartBidHistories.ToList())
                    {
                        

                        if (history == bid.VendorPartBidHistories.AsEnumerable().Last())
                        {
                            history.VendorPartBid = null;
                            history.IsActive = false;
                            history.IsDeleted = true;
                            history.VendorId = bid.VendorId;
                            history.BidQuoteId = bid.BidQuotePart.BidQuoteId;
                            history.BidQuotePartId = bid.BidQuotePart.Id;
                        }
                        else
                        {
                            history.VendorPartBidId = null;
                            history.IsActive = false;
                            
                        }
                        
                    }
                }
                await _dbService.SaveChangesAsync(cancellationToken);

               

                _dbService.Set<VendorPartBid>().Remove(bid);
                

                await _dbService.SaveChangesAsync(cancellationToken);

                return new TransactionResult<string>(partDescription, AppConstants.TransactionSuccess);
            }
        }
    }
}