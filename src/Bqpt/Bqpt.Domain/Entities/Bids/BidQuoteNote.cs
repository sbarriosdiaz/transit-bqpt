namespace Bqpt.Domain
{
    public class BidQuoteNote : Entity<int>
    {
        public string Note { get; set; }

        public int BidQuoteId { get; set; }
        public BidQuote BidQuote { get; set; }
    }
}