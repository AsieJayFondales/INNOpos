using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore; 

namespace InnoSpend.Models
{
    public class CustomerInfo
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Firstname { get; set; } = "";

        [MaxLength(100)]
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

        [Required]
        public string Company { get; set; } = "";

        [MaxLength(255)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        // New fields for loyalty program
        public bool IsLoyaltyMember { get; set; }
        public int LoyaltyPoints { get; set; }
        public DateTime? LoyaltyJoinDate { get; set; }

        // Customer code
        [Required]
        [MaxLength(20)]
        public string CustomerCode { get; set; } = "";

        // GDPR and data retention
        public DateTime LastActivityDate { get; set; }
        public bool MarketingConsent { get; set; }
        public DateTime? MarketingConsentDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property for purchase history
        public virtual ICollection<Purchase> PurchaseHistory { get; set; } = new List<Purchase>();
    }
    
}
