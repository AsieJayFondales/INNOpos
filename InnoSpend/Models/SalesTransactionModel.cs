
//For Handling Sales Transactions
//v3.0.4

using System.ComponentModel.DataAnnotations;

namespace InnoSpend.Models
{
    public class SalesTransactionModel
    {
        [Required]
        public int ProductId { get; set; }

        public int CustomerCode { get; set; } //v3.0.5 Optional, for walk-in customers | will be converted to string when saving to database

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; } = 1;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; } = 0;

        [Required]
        public string PaymentMethod { get; set; } = "Cash";

        // Calculated properties
        public decimal TotalBeforeDiscount => Quantity * UnitPrice;
        public decimal TotalAfterDiscount => TotalBeforeDiscount - Discount;
    }
}
