////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using Newtonsoft.Json;

namespace Bqpt.Infrastructure
{
    public class BlobFileEnvelop
    {
        [JsonProperty("domainKey")]
        public string DomainKey { get; set; }

        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("fileExtension")]
        public string FileExtension { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("bytes")]
        public byte[] Bytes { get; set; }
    }
}