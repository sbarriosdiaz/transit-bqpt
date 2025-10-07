using System.ComponentModel.DataAnnotations;
using Bqpt.Common;

namespace Bqpt.Application
{
    public class BidQuotePartViewModel : BaseViewModel<int>
    {
        [Range(1, 9999)]
        public int PurchaseOrderQuantity { get; set; }
    }
}