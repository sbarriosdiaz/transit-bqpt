using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Bqpt.Common;

namespace Bqpt.Application
{
    public class VendorPartBidViewModel : BaseViewModel<int>
    {
        public string BidQuoteDomainKey { get; set; }
        public int? VendorId { get; set; }
        public int? BidQuotePartId { get; set; }

        [Display(Name = "Part Number")]
        public string AssetWorksPartNumber { get; set; }

        [Display(Name = "Part Manufacturer Number")]
        public string AssetWorksPartManufacturerPartNumber { get; set; }

        [Display(Name = "Part Description")]
        public string AssetWorksPartDescription { get; set; }

        [Display(Name = "Requested Quantity")]
        public int? RequestedQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = " Must be a whole number")]
        [Display(Name = "Unit Quantity")]
        [Required]
        public int? QuantityAvailable { get; set; }

        [Range(0,int.MaxValue,ErrorMessage =" Must be a whole number")]
        [Display(Name = "Items Per Unit")]
        [Required]
        public int? ItemsPerUnit { get; set; }

        [Display(Name = "Unit Price")]
        [Required]
        public decimal? UnitPrice { get; set; }
        [Display(Name = "Core Charge")]        
        public decimal? Core { get; set; }

        [Display(Name = "Estimate Delivery Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required]
        public DateTime? EstimateDeliveryDate { get; set; }
        [AllowHtml]
        [MaxLength(100, ErrorMessage = "Comment cannot be greater than 100")]
        public string Comment { get; set; }

        public bool? IsSelected { get; set; }
    }
}