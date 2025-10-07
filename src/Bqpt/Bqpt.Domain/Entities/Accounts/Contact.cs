namespace Bqpt.Domain
{
    public class Contact : Entity<int>
    {
        public string AssetWorksContactName { get; set; }
        public string AssetWorksContactPhone { get; set; }
        public string AssetWorksContactEmail { get; set; }
        public string AssetWorksContactAddress1 { get; set; }
        public string AssetWorksContactAddress2 { get; set; }
        public string AssetWorksContactAddress3 { get; set; }
        public string AssetWorksContactAddress4 { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }
}