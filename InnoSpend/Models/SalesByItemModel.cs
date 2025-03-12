//Models/SalesByItemModel.cs
//v53/v3.0.0 


namespace InnoSpend.Models
{
    public class SalesByItemModel
    {
        public string ProductName { get; set; } = "";
        public int UnitsSold { get; set; }
        public decimal TotalSales { get; set; }
        public decimal AverageSalePrice { get; set; }
    }
}