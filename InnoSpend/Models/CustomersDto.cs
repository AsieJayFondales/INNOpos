using System.ComponentModel.DataAnnotations;

namespace InnoSpend.Models
{
    public class CustomersDto
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Firstname { get; set; } = "";

        [Required, MaxLength(100)]
        public string Lastname { get; set; } = "";

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = "";
         
        public string? StreetAddress { get; set; } 
         
        public string? City { get; set; }  

        public string? Province { get; set; } 
         
        public int? PostalCode { get; set; } 
         
        public string? Country { get; set; }

        public string Company { get; set; } = "";

        public string? Notes { get; set; }

        public bool MarketingConsent { get; set; }

        // For display only
        public string? CustomerCode { get; set; }
        public int? LoyaltyPoints { get; set; }
        public bool? IsLoyaltyMember { get; set; }
    }
}
