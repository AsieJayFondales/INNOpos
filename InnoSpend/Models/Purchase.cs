// Models/Purchase.cs/v52/v3.0.0/v58 same
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InnoSpend.Models
{
    public class Purchase //v3.0.3
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }

        [Precision(18, 2)]
        public decimal Amount { get; set; }

        [Precision(18, 2)]
        public decimal Discount { get; set; }

        [Precision(18, 2)]
        public decimal NetAmount { get; set; }

        public string? InvoiceNumber { get; set; }
        public virtual ICollection<PurchaseItem> Items { get; set; } = new List<PurchaseItem>();
    }
     //v3.0.2
    public class PurchaseItem 
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        
        //v3.0.3
        [Precision(18, 2)] 
        public decimal UnitPrice { get; set; }

        [Precision(18, 2)]
        public decimal TotalAmount { get; set; }

        [Precision(18, 2)]
        public decimal Discount { get; set; }

        public virtual Purchase Purchase { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}