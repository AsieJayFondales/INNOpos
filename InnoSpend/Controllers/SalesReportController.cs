// Controllers/SalesReportController.cs/v46/v3.0.0
using InnoSpend.Models;
using InnoSpend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InnoSpend.Controllers
{
    public class SalesReportController : Controller //v3.0.2
    {
        private readonly ISalesReportService _salesReportService;
        private readonly ApplicationDbContext _context;

        public SalesReportController(
            ISalesReportService salesReportService,
            ApplicationDbContext context)
        {
            _salesReportService = salesReportService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DailyReport(DateTime? date)
        {
            date ??= DateTime.Today;
            var summary = await _salesReportService.GetSalesSummary(date.Value, date.Value);
            var hourlyData = await _salesReportService.GetDailySalesReport(date.Value);

            ViewData["Date"] = date.Value.ToString("d");
            return View(new DailyReportModel  
            {
                Summary = summary,
                HourlySales = hourlyData
            });
        }

        [HttpGet]
        public async Task<IActionResult> WeeklyReport(DateTime? startDate)
        {
            startDate ??= DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var endDate = startDate.Value.AddDays(6);
            var report = await _salesReportService.GetWeeklySalesReport(startDate.Value, endDate);
            return View(report);
        }

        [HttpGet]
        public async Task<IActionResult> MonthlyReport(int? year, int? month)
        {
            year ??= DateTime.Today.Year;
            month ??= DateTime.Today.Month;
            var report = await _salesReportService.GetMonthlySalesReport(year.Value, month.Value);
            return View(report);
        }

        [HttpGet]
        public async Task<IActionResult> SalesByItem(DateTime startDate, DateTime endDate)
        {
            var salesByItem = await _context.Sales // Changed from _salesReportService.Sales
                .Include(s => s.Product)
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .GroupBy(s => s.Product)
                .Select(g => new SalesByItemModel
                {
                    ProductName = g.Key.Name,
                    UnitsSold = g.Sum(s => s.Quantity),
                    TotalSales = g.Sum(s => s.TotalPrice),
                    AverageSalePrice = g.Average(s => s.UnitPrice)
                })
                .OrderByDescending(s => s.TotalSales)
                .ToListAsync();

            return View(salesByItem);
        }

        [HttpGet]
        public async Task<IActionResult> SalesByCategory(DateTime startDate, DateTime endDate)
        {
            // Calculate total sales once
            var totalSales = await _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .SumAsync(s => s.TotalPrice);

            var salesByCategory = await _context.Sales // Changed from _salesReportService.Sales
                .Include(s => s.Product)
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .GroupBy(s => s.Product.Category)
                .Select(g => new SalesByCategoryModel
                {
                    Category = g.Key,
                    TotalSales = g.Sum(s => s.TotalPrice),
                    PercentageOfTotalSales = g.Sum(s => s.TotalPrice) / totalSales * 100
                })
                .OrderByDescending(s => s.TotalSales)
                .ToListAsync();

            return View(salesByCategory);
        }

        [HttpGet]
        public async Task<IActionResult> Export(DateTime startDate, DateTime endDate, string reportType, string format)
        {
            var fileName = $"SalesReport_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}";
            byte[] fileContents;
            string contentType;

            if (format.ToLower() == "excel")
            {
                fileContents = await _salesReportService.ExportToExcel(startDate, endDate, reportType);
                fileName += ".xlsx";
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            else
            {
                fileContents = await _salesReportService.ExportToPdf(startDate, endDate, reportType);
                fileName += ".pdf";
                contentType = "application/pdf";
            }

            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport(DateTime startDate, DateTime endDate, string reportType)
        {
            var sales = await _context.Sales
                .Include(s => s.Product)
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .ToListAsync();

            var report = new SalesReportModel
            {
                StartDate = startDate,
                EndDate = endDate,
                GrossSales = sales.Sum(s => s.TotalPrice),
                Refunds = 0, // Implement refund logic if needed
                Discounts = sales.Sum(s => s.Discount),
                NetSales = sales.Sum(s => s.TotalPrice) - sales.Sum(s => s.Discount),
                Sales = sales
            };

            return View($"{reportType}Report", report);
        }
    }
}