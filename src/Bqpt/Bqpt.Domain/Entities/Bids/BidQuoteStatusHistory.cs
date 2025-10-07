namespace Bqpt.Domain
{
    public class BidQuoteStatusHistory : Entity<int>
    {
        public string Status { get; set; }

        public int BidQuoteId { get; set; }
        public BidQuote BidQuote { get; set; }
    }
}