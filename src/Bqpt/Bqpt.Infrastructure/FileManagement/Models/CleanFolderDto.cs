////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Bqpt.Infrastructure
{
    public class CleanFolderDto
    {
        [Required]
        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [Required]
        [JsonProperty("path")]
        public string Path { get; set; }
    }
}