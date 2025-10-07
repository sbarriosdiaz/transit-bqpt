////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Bqpt.Infrastructure
{
    public class Credentials
    {
        [Required]
        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [Required]
        [JsonProperty("clientSecret")]
        public string ClientSecret { get; set; }

        [Required]
        [JsonProperty("tenantId")]
        public string TenantId { get; set; }
    }
}