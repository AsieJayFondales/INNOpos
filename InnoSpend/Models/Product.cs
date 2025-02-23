using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InnoSpend.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(100)]
        public string Brand { get; set; } = "";

        [MaxLength(100)]
        public string Category { get; set; } = "";

        [Precision(16, 2)]
        public decimal? Price { get; set; }

        [Precision(16, 2)]
        public decimal Cost { get; set; }

        public string Description { get; set; } = "";

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public bool IsAvailableForSale { get; set; } = true;

        public string SoldBy { get; set; } = "Each"; // "Each" or "Weight/Volume"

        [MaxLength(50)]
        public string? SKU { get; set; }

        [MaxLength(100)]
        public string? Barcode { get; set; }
    }

}
