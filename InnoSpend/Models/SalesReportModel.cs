// Models/SalesReportModel.cs

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InnoSpend.Models
{
    public class SalesReportModel //v53/v3.0.0
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal GrossSales { get; set; }
        public decimal Refunds { get; set; }
        public decimal Discounts { get; set; }
        public decimal NetSales { get; set; }
        public List<Sale> Sales { get; set; } = new List<Sale>();
    }
        

     //v60/v3.0.0
        public class SalesSummary
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal GrossSales { get; set; }
            public decimal Refunds { get; set; }
            public decimal Discounts { get; set; }
            public decimal NetSales { get; set; }
            public int TransactionCount { get; set; }
            public decimal AverageTransactionValue { get; set; }
        }

        public class DailySales
        {
            public int Hour { get; set; }
            public decimal Sales { get; set; }
            public int TransactionCount { get; set; }
        }
     
    public class WeeklySales //v3.0.0
    {
        public int WeekNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Sales { get; set; }
        public int TransactionCount { get; set; }
    }

    public class MonthlySales
    {
        public DateTime Date { get; set; }
        public decimal Sales { get; set; }
        public int TransactionCount { get; set; }
    }

    public class ProductSales
        {
            public string ProductName { get; set; } = "";
            public string Category { get; set; } = "";
            public int QuantitySold { get; set; }
            public decimal TotalSales { get; set; }
            public decimal AveragePrice { get; set; }
        }

        public class CategorySales
        {
            public string CategoryName { get; set; } = "";
            public decimal TotalSales { get; set; }
            public int ProductCount { get; set; }
            public decimal PercentageOfTotal { get; set; }
        }

        public class DailyReportModel 
        {
            public SalesSummary Summary { get; set; } = new SalesSummary();
            public List<DailySales> HourlySales { get; set; } = new List<DailySales>();
        }

    public class WeeklyReportModel //v3.0.3 manually
    {
        public SalesSummary Summary { get; set; } = new SalesSummary();
        public List<WeeklySales> WeeklySales { get; set; } = new List<WeeklySales>();
    }

}



