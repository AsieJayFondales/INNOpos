using InnoSpend.Models;
using InnoSpend.Services;
using Microsoft.AspNetCore.Mvc;

namespace InnoSpend.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public CustomersController(ApplicationDbContext context, IWebHostEnvironment environment)//added iwebhostenv to save the new products in database
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Customers()
        {
            var customers = context.Customers.OrderByDescending(p => p.Id).ToList();
            return View(customers);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        //redirecting to the List of customers after clicking submit
        [HttpPost]
        public IActionResult Create(CustomersDto customersDto)
        {
          
            // Save the newly created product in the database
            CustomerInfo customer = new CustomerInfo()
            {
                Firstname = customersDto.Firstname,
                Lastname = customersDto.Lastname,
                Email = customersDto.Email,
                Phone = customersDto.Phone,
                Address = customersDto.Address,
                Company = customersDto.Company,
                Notes = customersDto.Notes, 
                CreatedAt = DateTime.Now,
            };

            context.Customers.Add(customer);
            context.SaveChanges();

            return RedirectToAction("Customers", "Customers");
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
                Address = customer.Address,
                Company = customer.Company,
                Notes = customer.Notes
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
            customer.Address = customerDto.Address;
            customer.Company = customerDto.Company;
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
