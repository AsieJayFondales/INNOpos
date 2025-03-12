// Models/Sale.cs/v44/v3.0.0
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InnoSpend.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string CustomerCode { get; set; } = ""; //switch to string to match CustomerInfo v3.0.4
        public int Quantity { get; set; }

        [Precision(18, 2)]
        public decimal UnitPrice { get; set; }

        [Precision(18, 2)]
        public decimal TotalPrice { get; set; }

        [Precision(18, 2)]
        public decimal Discount { get; set; }

        public DateTime SaleDate { get; set; }
        public string PaymentMethod { get; set; } = "";
        public string InvoiceNumber { get; set; } = "";

        // Navigation properties
        public virtual Product Product { get; set; } = null!;
        public virtual CustomerInfo Customer { get; set; } = null!;
    }
}