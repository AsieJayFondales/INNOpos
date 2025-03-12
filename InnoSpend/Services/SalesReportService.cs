// Services/SalesReportService.cs/ v59/v3.0.0
using ClosedXML.Excel;
using InnoSpend.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;

namespace InnoSpend.Services
{
    public class SalesReportService : ISalesReportService
    {
        private readonly ApplicationDbContext _context;

        public SalesReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SalesSummary> GetSalesSummary(DateTime startDate, DateTime endDate)
        {
            var purchases = await _context.Purchases
                .Include(p => p.Items)
                .Where(p => p.PurchaseDate.Date >= startDate.Date &&
                           p.PurchaseDate.Date <= endDate.Date)
                .ToListAsync();

            return new SalesSummary
            {
                StartDate = startDate,
                EndDate = endDate,
                GrossSales = purchases.Sum(p => p.Amount),
                Refunds = purchases.Sum(p => p.Items.Sum(i => i.Discount)),
                Discounts = purchases.Sum(p => p.Discount),
                NetSales = purchases.Sum(p => p.NetAmount),
                TransactionCount = purchases.Count,
                AverageTransactionValue = purchases.Any() ?
                    purchases.Average(p => p.NetAmount) : 0
            };
        }

        public async Task<List<DailySales>> GetDailySalesReport(DateTime date)
        {
            return await _context.Purchases
                .Where(p => p.PurchaseDate.Date == date.Date)
                .GroupBy(p => new { Hour = p.PurchaseDate.Hour })
                .Select(g => new DailySales
                {
                    Hour = g.Key.Hour,
                    Sales = g.Sum(p => p.NetAmount),
                    TransactionCount = g.Count()
                })
                .OrderBy(d => d.Hour)
                .ToListAsync();
        }

        public async Task<List<WeeklySales>> GetWeeklySalesReport(DateTime startDate, DateTime endDate)
        {
            return await _context.Purchases
                .Where(p => p.PurchaseDate.Date >= startDate.Date && p.PurchaseDate.Date <= endDate.Date)
                .GroupBy(p => EF.Functions.DateDiffDay(startDate, p.PurchaseDate) / 7)
                .Select(g => new WeeklySales
                {
                    WeekNumber = g.Key + 1,
                    StartDate = startDate.AddDays(g.Key * 7),
                    EndDate = startDate.AddDays((g.Key + 1) * 7 - 1),
                    Sales = g.Sum(p => p.NetAmount),
                    TransactionCount = g.Count()
                })
                .OrderBy(w => w.WeekNumber)
                .ToListAsync();
        }

        public async Task<List<MonthlySales>> GetMonthlySalesReport(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            return await _context.Purchases
                .Where(p => p.PurchaseDate.Date >= startDate && p.PurchaseDate.Date <= endDate)
                .GroupBy(p => p.PurchaseDate.Date)
                .Select(g => new MonthlySales
                {
                    Date = g.Key,
                    Sales = g.Sum(p => p.NetAmount),
                    TransactionCount = g.Count()
                })
                .OrderBy(m => m.Date)
                .ToListAsync();
        }

        public async Task<List<ProductSales>> GetProductSalesReport(DateTime startDate, DateTime endDate)
        {
            return await _context.PurchaseItems
                .Where(pi => pi.Purchase.PurchaseDate.Date >= startDate.Date && pi.Purchase.PurchaseDate.Date <= endDate.Date)
                .GroupBy(pi => pi.Product)
                .Select(g => new ProductSales
                {
                    ProductName = g.Key.Name,
                    Category = g.Key.Category,
                    QuantitySold = g.Sum(pi => pi.Quantity),
                    TotalSales = g.Sum(pi => pi.TotalAmount),
                    AveragePrice = g.Average(pi => pi.UnitPrice)
                })
                .OrderByDescending(ps => ps.TotalSales)
                .ToListAsync();
        }

        public async Task<List<CategorySales>> GetCategorySalesReport(DateTime startDate, DateTime endDate)
        {
            var totalSales = await _context.Purchases
                .Where(p => p.PurchaseDate.Date >= startDate.Date && p.PurchaseDate.Date <= endDate.Date)
                .SumAsync(p => p.NetAmount);

            return await _context.PurchaseItems
                .Where(pi => pi.Purchase.PurchaseDate.Date >= startDate.Date && pi.Purchase.PurchaseDate.Date <= endDate.Date)
                .GroupBy(pi => pi.Product.Category)
                .Select(g => new CategorySales
                {
                    CategoryName = g.Key,
                    TotalSales = g.Sum(pi => pi.TotalAmount),
                    ProductCount = g.Select(pi => pi.ProductId).Distinct().Count(),
                    PercentageOfTotal = g.Sum(pi => pi.TotalAmount) / totalSales * 100
                })
                .OrderByDescending(cs => cs.TotalSales)
                .ToListAsync();
        }

        public async Task<byte[]> ExportToExcel(DateTime startDate, DateTime endDate, string reportType)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sales Report");

            // Add header
            worksheet.Cell(1, 1).Value = "Sales Report";
            worksheet.Cell(2, 1).Value = $"Period: {startDate:d} - {endDate:d}";

            // Add data based on report type
            var summary = await GetSalesSummary(startDate, endDate);

            worksheet.Cell(4, 1).Value = "Gross Sales";
            worksheet.Cell(4, 2).Value = summary.GrossSales;
            worksheet.Cell(5, 1).Value = "Discounts";
            worksheet.Cell(5, 2).Value = summary.Discounts;
            worksheet.Cell(6, 1).Value = "Net Sales";
            worksheet.Cell(6, 2).Value = summary.NetSales;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<byte[]> ExportToPdf(DateTime startDate, DateTime endDate, string reportType)
        {
            using var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            document.Add(new Paragraph($"Sales Report ({startDate:d} - {endDate:d})"));

            var summary = await GetSalesSummary(startDate, endDate);
            document.Add(new Paragraph($"Gross Sales: {summary.GrossSales:C}"));
            document.Add(new Paragraph($"Discounts: {summary.Discounts:C}"));
            document.Add(new Paragraph($"Net Sales: {summary.NetSales:C}"));

            document.Close();
            return stream.ToArray();
        }
         
    }
}