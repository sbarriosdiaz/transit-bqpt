////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace Bqpt.Infrastructure
{
    public class ApplicationLoginViewModel
    {
        public string UserIpAddress { get; set; }

        [Required]
        [Display(Name = "Username")]
        [MinLength(3, ErrorMessage = "Username should be at least 3 characters long...")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}