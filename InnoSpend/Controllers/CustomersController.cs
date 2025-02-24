using InnoSpend.Models;
using InnoSpend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration; 
using System.IO;

namespace InnoSpend.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ICustomerService customerService;
        private readonly IWebHostEnvironment environment;

        public CustomersController(
            ApplicationDbContext context,
            ICustomerService customerService,
            IWebHostEnvironment environment ) //added iwebhostenv to save the new products in database
        {
            this.context = context;
            this.environment = environment;
            this.customerService = customerService;
        }

        // GET: Customers FEB24 
        public IActionResult Customers()
        {
            var customers = context.Customers
                .Where(c => c.IsActive)
                .OrderByDescending(p => p.Id)
                .ToList();
            return View(customers);
        }

        //FEB24  
        public IActionResult Create()
        {
            return View(new CustomersDto
            {
                CustomerCode = customerService.GenerateCustomerCode()
            });
        }

        //redirecting to the List of customers after clicking submit
        [HttpPost] 
        public IActionResult Create(CustomersDto customersDto)
        {
            if (!ModelState.IsValid)
                return View(customersDto);

            // Save the newly created product in the database
            var customer = new CustomerInfo
            {
                Firstname = customersDto.Firstname,
                Lastname = customersDto.Lastname,
                Email = customersDto.Email,
                Phone = customersDto.Phone,
                StreetAddress = customersDto.StreetAddress,
                City = customersDto.City,
                Province = customersDto.Province,
                PostalCode = customersDto.PostalCode,
                Country = customersDto.Country,
                Company = customersDto.Company,
                Notes = customersDto.Notes,
                CreatedAt = DateTime.Now,
                CustomerCode = customerService.GenerateCustomerCode(),
                LastActivityDate = DateTime.Now,
                MarketingConsent = customersDto.MarketingConsent,
                MarketingConsentDate = customersDto.MarketingConsent ? DateTime.Now : null
            };

            context.Customers.Add(customer);
            context.SaveChanges();

            return RedirectToAction(nameof(Customers));
        }

        //FEB24 
        // GET: Customers/PurchaseHistory
        public IActionResult PurchaseHistory(int id)
        {
            var customer = context.Customers
                .Include(c => c.PurchaseHistory)
                .FirstOrDefault(c => c.Id == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        //FEB24
        [HttpPost] 
        public IActionResult EnrollInLoyalty(int id)
        {
            var customer = context.Customers.Find(id);
            if (customer == null)
                return NotFound();

            customer.IsLoyaltyMember = true;
            customer.LoyaltyJoinDate = DateTime.Now;
            context.SaveChanges();

            return RedirectToAction(nameof(Edit), new { id });
        }


        //FEB24 v36
        [HttpPost]
        public async Task<IActionResult> ImportCustomers(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    HeaderValidated = null
                };

                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvHelper.CsvReader(reader, config);

                var records = csv.GetRecords<CustomersDto>().ToList();
                foreach (var record in records)
                {
                    var customer = new CustomerInfo
                    {
                        Firstname = record.Firstname,
                        Lastname = record.Lastname,
                        Email = record.Email,
                        Phone = record.Phone,
                        StreetAddress = record.StreetAddress,
                        City = record.City,
                        Province = record.Province,
                        PostalCode = record.PostalCode,
                        Country = record.Country,
                        Company = record.Company,
                        Notes = record.Notes,
                        CustomerCode = customerService.GenerateCustomerCode(),
                        CreatedAt = DateTime.Now,
                        LastActivityDate = DateTime.Now,
                        IsActive = true
                    };
                    context.Customers.Add(customer);
                }

                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Successfully imported {records.Count} customers.";
                return RedirectToAction(nameof(Customers));
            }
            catch (Exception)
            {
                // Log the exception details
                TempData["ErrorMessage"] = "Error importing customers. Please check the file format.";
                return RedirectToAction(nameof(Customers));
            }
        }

        public IActionResult Edit(int id)
        {
            var customer = context.Customers.Find(id);

            if (customer == null)
            {
                return RedirectToAction("Customers", "Customers");
            }

            //create productDto from product
            var customerDto = new CustomersDto()
            {
                Id = customer.Id,
                Firstname = customer.Firstname,
                Lastname = customer.Lastname,
                Email = customer.Email,
                Phone = customer.Phone,
                StreetAddress = customer.StreetAddress,
                City = customer.City,
                Province = customer.Province,
                PostalCode = customer.PostalCode,
                Country = customer.Country,
                Company = customer.Company,
                Notes = customer.Notes,
                CustomerCode = customer.CustomerCode,
                MarketingConsent = customer.MarketingConsent,
                LoyaltyPoints = customer.LoyaltyPoints,
                IsLoyaltyMember = customer.IsLoyaltyMember
            };

            ViewData["CustomerId"] = customer.Id; 
            ViewData["CreatedAt"] = customer.CreatedAt.ToString("MM/dd/yyyy");

            return View(customerDto);
        }


        [HttpPost]
        public IActionResult Edit(int id, CustomersDto customerDto)
        {
            var customer = context.Customers.Find(id);

            if (customer == null)
            {
                return RedirectToAction("Customers", "Customers");
            }

            if (!ModelState.IsValid)
            {
                ViewData["CustomerId"] = customer.Id; 
                ViewData["CreatedAt"] = customer.CreatedAt.ToString("MM/dd/yyyy");

                return View(customerDto);
            }
             

            // Update the customer details in the database
            customer.Firstname = customerDto.Firstname;
            customer.Lastname = customerDto.Lastname;
            customer.Email = customerDto.Email;
            customer.Phone = customerDto.Phone; 
            customer.Company = customerDto.Company;
            customer.StreetAddress = customerDto.StreetAddress;
            customer.City = customerDto.City;
            
            customer.Notes = customerDto.Notes;


            context.SaveChanges();
            return RedirectToAction("Customers", "Customers");
        }

        public IActionResult Delete(int id)
        {
            var customer = context.Customers.Find(id);
            if (customer == null)
            {
                return RedirectToAction("Customers", "Customers");
            }
             
            context.Customers.Remove(customer);
            context.SaveChanges(true);

            return RedirectToAction("Customers", "Customers");
        }

        //For search bar
        [HttpPost]
        public IActionResult Customers(string searchTerm)
        {
            var customers = new List<CustomerInfo>();

            if (string.IsNullOrEmpty(searchTerm))
            {
                // If no search term, show all customers
                customers = context.Customers.OrderByDescending(customer => customer.Id).ToList();
            }
            else
            {
                // Search customers by name or ID
                customers = context.Customers
                    .Where(customer => customer.Firstname.Contains(searchTerm) || customer.Id.ToString().Contains(searchTerm) || customer.Lastname.Contains(searchTerm))
                    .OrderByDescending(customer => customer.Id)
                    .ToList();
            }

            // If no customers were found, display a message
            if (!customers.Any())
            {
                TempData["SearchMessage"] = "Customer does not exist.";
            }

            // Pass the search term back to the view
            ViewData["SearchTerm"] = searchTerm;

            return View("Customers", customers);
        }


    }
}
