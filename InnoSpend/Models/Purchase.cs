using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InnoSpend.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime PurchaseDate { get; set; }

        [Precision(18, 2)] // This attribute specifies decimal precision
        public decimal Amount { get; set; }

        [MaxLength(100)]
        public string InvoiceNumber { get; set; } = "";

        // Navigation property
        public virtual CustomerInfo Customer { get; set; } = null!;
    }
}