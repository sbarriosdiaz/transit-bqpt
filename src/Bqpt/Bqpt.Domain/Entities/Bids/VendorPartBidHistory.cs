using System;

namespace Bqpt.Domain
{
    public class VendorPartBidHistory : Entity<int>
    {
        public int? QuantityAvailable { get; set; }
        public int? ItemsPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Core { get; set; }
        public DateTime? EstimateDeliveryDate { get; set; }
        public string Comment { get; set; }
        public int VendorId { get; set; }
        public int BidQuotePartId { get; set; }
        public int BidQuoteId { get; set; }
        public bool? IsSelected { get; set; }

        public string LoggedInEmailAddress { get; set; }

        public int? VendorPartBidId { get; set; }
        public VendorPartBid VendorPartBid { get; set; }

        public int? VendorPartBidNoteId { get; set; }
    }
}