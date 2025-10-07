using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Management;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using MediatR;

namespace Bqpt.Application
{
    public class SyncVendorInfo : IRequest<TransactionResult<string>>
    {
        public class Handler : IRequestHandler<SyncVendorInfo, TransactionResult<string>>
        {
            private readonly IDbService _dbService;
            private readonly IAssetWorksServices _awService;

            public Handler(IDbService dbService, IAssetWorksServices awService)
            {
                _dbService = dbService;
                _awService = awService;
            }

            public async Task<TransactionResult<string>> Handle(SyncVendorInfo request, CancellationToken cancellationToken)
            {
                var awVendors = await _awService.GetAssetWorksVendors();
                var localVendors = await _dbService.Set<Vendor>().ToListAsync(cancellationToken);

                awVendors = awVendors.Where(x => !string.IsNullOrEmpty(x.AssetWorksVendorEmail)).ToList();
                var newVendors = awVendors.Where(awv => !localVendors.Any(x => x.AssetWorksVendorNumber.Equals(awv.AssetWorksVendorNumber))).ToList();

                foreach (var v in localVendors)
                {
                    v.IsActive = false;
                    var awVendor = awVendors.FirstOrDefault(awv => awv.AssetWorksVendorNumber.Equals(v.AssetWorksVendorNumber));
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
                return new TransactionResult<string>("Sucess","Updated");
            }
        }
    }
}

