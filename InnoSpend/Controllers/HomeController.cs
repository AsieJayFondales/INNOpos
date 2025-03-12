using System.Diagnostics;
using InnoSpend.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnoSpend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() //v6
        {
            var viewModel = new HomeViewModel
            {
                TransactionActivities = GetTransactionActivities(),
                Categories = GetCategories(),
                RecentOrders = GetRecentOrders()
            };

            return View(viewModel);
        }

        private List<TransactionActivity> GetTransactionActivities()
        {
            // Sample data - replace with actual data from your database
            return new List<TransactionActivity>
        {
            new TransactionActivity { Date = DateTime.Now.AddDays(-7), Amount = 1200 },
            new TransactionActivity { Date = DateTime.Now.AddDays(-6), Amount = 1500 },
            // Add more data points
        };
        }

        private List<Category> GetCategories()
        {
            return new List<Category>
        {
            new Category { Name = "All Menu", Icon = "menu", ItemCount = 31 },
            new Category { Name = "Meals", Icon = "restaurant", ItemCount = 8 },
            new Category { Name = "Snacks", Icon = "cookie", ItemCount = 4 },
            new Category { Name = "Side dish", Icon = "utensils", ItemCount = 16 }
        };
        }

        private List<RecentOrder> GetRecentOrders()
        {
            return new List<RecentOrder>
        {
            new RecentOrder
            {
                OrderId = "Order#123",
                Amount = 25.00M,
                Date = DateTime.Now.AddHours(-2),
                Status = "Success"
            }
            // Add more orders
        };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult HomePage()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
