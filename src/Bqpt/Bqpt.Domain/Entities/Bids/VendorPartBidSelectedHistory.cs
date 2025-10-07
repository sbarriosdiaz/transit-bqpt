namespace Bqpt.Domain
{
    public class VendorPartBidSelectedHistory : Entity<int>
    {
        public int VendorPartBidId { get; set; }
        public VendorPartBid VendorPartBid { get; set; }
    }
}