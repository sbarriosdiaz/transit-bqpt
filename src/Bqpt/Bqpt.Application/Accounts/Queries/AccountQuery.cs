using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using FluentValidation;
using MediatR;

namespace Bqpt.Application
{
    public class AccountQuery : IRequest<VendorDto>
    {
        public AccountQuery(string emailAddress) => EmailAddress = emailAddress;

        public string EmailAddress { get; }

        public class Handler : IRequestHandler<AccountQuery, VendorDto>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService) => _dbService = dbService;

            public async Task<VendorDto> Handle(AccountQuery request, CancellationToken cancellationToken)
            {
                var vendorEmail = request.EmailAddress.ToLowerInvariant();

                return await _dbService.Set<Vendor>().AsNoTracking()
                    .Include(v => v.Contacts)
                    .Where(v => v.AssetWorksVendorEmail.Equals(vendorEmail) || v.Contacts.Any(c => c.AssetWorksContactEmail.Equals(vendorEmail)))
                    .Select(v => new VendorDto
                    {
                        DomainKey = v.DomainKey,
                        AssetWorksVendorName = v.AssetWorksVendorName
                    })
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}