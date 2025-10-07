using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using MediatR;

namespace Bqpt.Application
{
    public class FlagUpdatePOCommand : IRequest<TransactionResult<string>>
    {
        public string DomainKey { get; set; }

        public FlagUpdatePOCommand(string domainKey)
        {
            DomainKey = domainKey;
        }

        public class Handler : IRequestHandler<FlagUpdatePOCommand, TransactionResult<string>>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService) => _dbService = dbService;

            public async Task<TransactionResult<string>> Handle(FlagUpdatePOCommand request, CancellationToken cancellationToken)
            {
                var bidQuote = await _dbService.Set<BidQuote>().FirstOrDefaultAsync(vb => vb.DomainKey.Equals(request.DomainKey), cancellationToken);
                if (bidQuote != null && bidQuote.DisplayOrder == 0)
                {
                    bidQuote.DisplayOrder = 1;
                }
                else
                { 
                    bidQuote.DisplayOrder = 2; 
                }
                
                await _dbService.SaveChangesAsync(cancellationToken);

                return new TransactionResult<string>(bidQuote.DomainKey, AppConstants.TransactionSuccess);
            }
        }
    }
}