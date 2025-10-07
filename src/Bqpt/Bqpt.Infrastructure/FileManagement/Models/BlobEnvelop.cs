////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Configuration;
using Newtonsoft.Json;

namespace Bqpt.Infrastructure
{
    public class BlobEnvelop
    {
        [JsonProperty("domainKey")]
        public string DomainKey { get; set; }

        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("blobUrl")]
        public string BlobUrl { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("completeBlobUrl")]
        public Uri CompleteBlobUrl => new Uri($"{ConfigurationManager.AppSettings["Files::Api"]}{BlobUrl}");
    }
}