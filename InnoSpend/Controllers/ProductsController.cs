using InnoSpend.Models;
using InnoSpend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InnoSpend.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;


        private string GenerateSKU()
        {
            // Get the current year (last 2 digits)
            string year = DateTime.Now.Year.ToString().Substring(2);

            // Get all SKUs that start with the year and bring them to memory
            var skuList = context.Products
                .Where(p => p.SKU != null && p.SKU.StartsWith(year))
                .Select(p => p.SKU)
                .ToList(); // Fetch data into memory

            // Extract numbers safely
            int highestNumber = skuList
            .Select(sku => {
                int num;
                return int.TryParse(sku?.Substring(2) ?? "0", out num) ? num : 0;
            })
            .DefaultIfEmpty(0) // Avoid empty sequence errors
            .Max();


            // Generate the new SKU
            int newNumber = highestNumber + 1;
            string sku = $"{year}{newNumber:D5}";

            return sku;
        }

        [HttpGet]
        public IActionResult GenerateNewSKU()
        {
            string newSKU = GenerateSKU();
            return Json(new { sku = newSKU });
        }



        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)//added iwebhostenv to save the new products in database
        {
            this.context = context;
            this.environment = environment; 
        }
        public IActionResult Products()
        {
            var products = context.Products.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            var categories = context.Categories.ToList();
            ViewData["Categories"] = categories;
            return View();
        }




        //redirecting to the List of products after clicking submit
    [HttpPost]
public IActionResult Create(ProductDto productDto)
{
    if (productDto.ImageFile == null)
    {
        ModelState.AddModelError("ImageFile", "The image file is required");
    }

    if (!ModelState.IsValid)
    {
        ViewData["Categories"] = context.Categories.ToList();
        return View(productDto);
    }

    // Use the SKU from the form if it's provided, otherwise generate a new one
    string sku = !string.IsNullOrEmpty(productDto.SKU) ? productDto.SKU : GenerateSKU();

    // Save the file image of the newly created product
    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

    string imageFullPath = Path.Combine(environment.WebRootPath, "products", newFileName);
    using (var stream = System.IO.File.Create(imageFullPath))
    {
        productDto.ImageFile.CopyTo(stream);
    }

    // Save the newly created product in the database
    Product product = new Product()
    {
        Name = productDto.Name,
        Category = productDto.Category,
        Price = productDto.Price, // Allow null value
        Cost = productDto.Cost,
        Description = productDto.Description,
        ImageFileName = newFileName,
        CreatedAt = DateTime.Now,
        IsAvailableForSale = productDto.IsAvailableForSale,
        SoldBy = productDto.SoldBy,
        SKU = sku,
        Barcode = productDto.Barcode
    };

    
    context.Products.Add(product);
    context.SaveChanges();

    return RedirectToAction("Products", "Products");
}



        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Products", "Products");
            }

            var productDto = new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Price = product.Price,
                Cost = product.Cost,
                Description = product.Description,
                IsAvailableForSale = product.IsAvailableForSale,
                SoldBy = product.SoldBy,
                SKU = product.SKU,
                Barcode = product.Barcode
            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
            ViewData["Categories"] = context.Categories.ToList();

            return View(productDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Products", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
                ViewData["Categories"] = context.Categories.ToList();
                return View(productDto);
            }


            // Update the image file if we have a new image file
            string newFileName = product.ImageFileName;
            if (productDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.ImageFile.FileName);

                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFile.CopyTo(stream);
                }

                // Delete the old image file
                string oldImageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
                  System.IO.File.Delete(oldImageFullPath); 
            }


            // Update the product in the database
            product.Name = productDto.Name;
            product.Category = productDto.Category;
            product.Price = productDto.Price;
            product.Cost = productDto.Cost;
            product.Description = productDto.Description;
            product.IsAvailableForSale = productDto.IsAvailableForSale;
            product.SoldBy = productDto.SoldBy;
            // Note: We're not updating the SKU here
            product.Barcode = productDto.Barcode;

            context.SaveChanges();

            return RedirectToAction("Products", "Products");
        }

        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Products", "Products");
            }

            string imageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Products.Remove(product);
            context.SaveChanges(true);

            return RedirectToAction("Products", "Products");
        }


        //For adding new category
        [HttpPost]
        public IActionResult AddCategory(string categoryName)
        {
            if (!string.IsNullOrEmpty(categoryName))
            {
                var category = new Category()
                {
                    Name = categoryName
                };

                context.Categories.Add(category);
                context.SaveChanges();

                // Return a success response
                return Json(new { success = true, categoryName = category.Name });
            }

            ViewData["Categories"] = context.Categories.ToList();

            return Json(new { success = false });
        }


    }


}

