using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bqpt.Application
{
    public interface IAssetWorksServices
    {
        Task<IEnumerable<AssetWorksBidQuoteDto>> GetAssetWorksBidQuotes();

        Task<AssetWorksBidQuoteDto> GetAssetWorksBidQuotesById(string assetWorksBidQuoteId);

        Task<IEnumerable<AssetWorksBidQuotePartDto>> GetAssetWorksBidQuoteParts(string assetWorksBidQuoteId);

        Task<IEnumerable<AssetWorksBidQuotePartDto>> GetAssetWorksBidQuotePartsAll(string assetWorksBidQuoteId);

        Task<IEnumerable<AssetWorksVendorDto>> GetAssetWorksVendors();

        Task<IEnumerable<AssetWorksVendorContactDto>> GetAssetWorksVendorContacts();
    }
}