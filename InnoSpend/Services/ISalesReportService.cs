// Services/ISalesReportService.cs
//v3.0.2
using InnoSpend.Models;

namespace InnoSpend.Services
{
    public interface ISalesReportService
    {
        Task<SalesSummary> GetSalesSummary(DateTime startDate, DateTime endDate);
        Task<List<DailySales>> GetDailySalesReport(DateTime date);
        Task<List<WeeklySales>> GetWeeklySalesReport(DateTime startDate, DateTime endDate);
        Task<List<MonthlySales>> GetMonthlySalesReport(int year, int month);
        Task<List<ProductSales>> GetProductSalesReport(DateTime startDate, DateTime endDate);
        Task<List<CategorySales>> GetCategorySalesReport(DateTime startDate, DateTime endDate);
        Task<byte[]> ExportToExcel(DateTime startDate, DateTime endDate, string reportType);
        Task<byte[]> ExportToPdf(DateTime startDate, DateTime endDate, string reportType);
    }
}