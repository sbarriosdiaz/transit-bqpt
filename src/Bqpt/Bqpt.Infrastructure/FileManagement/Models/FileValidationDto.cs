////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Bqpt.Infrastructure
{
    public class FileValidationDto
    {
        [Required]
        [JsonProperty("domainKey")]
        public string DomainKey { get; set; }

        [Required]
        [JsonProperty("tenantId")]
        public string TenantId { get; set; }
    }
}