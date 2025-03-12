//version 10 

using InnoSpend.Models;
using InnoSpend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace InnoSpend.Controllers
{
    public class SalesTransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesTransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // This action should show the sales transaction form v3.0.5
        public IActionResult Index()
        {
            try
            {
                // Load available products and customers for dropdowns
                var products = _context.Products
                    .Where(p => p.IsAvailableForSale)
                    .OrderBy(p => p.Name)
                    .ToList();

                var customers = _context.Customers
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.Lastname)
                    .ToList();

                // Debug information
                Debug.WriteLine($"Products count: {products.Count}");
                if (products.Count > 0)
                {
                    Debug.WriteLine($"First product: {products[0].Name}, Price: {products[0].Price}");
                }

                ViewBag.Products = products;
                ViewBag.Customers = customers;

                return View(new SalesTransactionModel());
            }
            catch (Exception ex)
            {
                // Log the exception
                Debug.WriteLine($"Error in SalesTransactionController.Index: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);

                // Return a view with error information
                ViewBag.ErrorMessage = "An error occurred while loading products and customers.";
                ViewBag.Products = new List<Product>();
                ViewBag.Customers = _context.Customers
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.Lastname)
                    .ToList();

                return View(new SalesTransactionModel());
            }
        }

        // Rest of your controller code remains the same
    }
}

