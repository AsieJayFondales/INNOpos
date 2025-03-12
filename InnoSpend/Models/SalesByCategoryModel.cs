//Models/SalesByCategoryModel.cs
//v53/v3.0.0

namespace InnoSpend.Models
{
    public class SalesByCategoryModel
    {
        public string Category { get; set; } = "";
        public decimal TotalSales { get; set; }
        public decimal PercentageOfTotalSales { get; set; }
    }
}